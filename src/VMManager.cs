using System;
using System.Diagnostics;
using System.Net;
using System.Runtime.InteropServices;
using System.Xml.Linq;
//using IMAPI2;
//using IMAPI2FS;
using System.Runtime.InteropServices.ComTypes;
using IMAPI2FS;

namespace ClusterJockey
{
    public enum VMType
    {
        Multipass,
        HyperV
    }

    public class VMStatusResult
    {
        public string State { get; set; } = "Unknown";
        public LogLevel Level { get; set; } = LogLevel.Info;
    }

    public class VMManager
    {
        private readonly Action<string, LogLevel> _uiOutputLogger;

        public VMManager(Action<string, LogLevel> uiOutputLogger)
        {
            _uiOutputLogger = uiOutputLogger;
        }
        private string ParseMemory(string memory)
        {
            return memory.ToUpper().EndsWith("G")
                ? (long.Parse(memory.TrimEnd('G', 'g')) * 1024 * 1024 * 1024).ToString()
                : memory;
        }

        public string GetVMName(VMType type)
        {
            return type switch
            {
                VMType.Multipass => "ClusterJockey-lin",
                VMType.HyperV => "ClusterJockey-win",
                _ => "ClusterJockey-unknown"
            };
        }

        public VMStatusResult GetVMStatus(VMType type, string vmName)
        {
            return type switch
            {
                VMType.Multipass => GetMultipassVMStatus(vmName),
                VMType.HyperV => GetHyperVVMStatus(vmName),
                _ => new VMStatusResult { State = "Unknown Type", Level = LogLevel.Error }
            };
        }

        public string RunOnVM(VMType type, string vmName, string command)
        {
            return type switch
            {
                VMType.Multipass => RunCommand($"multipass exec {vmName} -- {command}"),
                VMType.HyperV => RunCommand($@"powershell -NoProfile -Command ""Invoke-Command -VMName '{vmName}' -ScriptBlock {{{command}}}"" -Authentication Default"""),
                _ => string.Empty
            };
        }

        public bool StartVM(VMType type, string vmName)
        {
            return type switch
            {
                VMType.Multipass => RunCommand($"multipass start {vmName}") != null,
                VMType.HyperV => RunCommand($"powershell -NoProfile -Command \"Start-VM -Name '{vmName}'\"") != null,
                _ => false
            };
        }

        public bool StopVM(VMType type, string vmName)
        {
            return type switch
            {
                VMType.Multipass => RunCommand($"multipass stop {vmName}") != null,
                VMType.HyperV => RunCommand($"powershell -NoProfile -Command \"Stop-VM -Name '{vmName}' -Force\"") != null,
                _ => false
            };
        }

        public bool CreateVM(VMType type, string vmName)
        {
            switch (type)
            {
                case VMType.Multipass:
                    {
                        string cpus = Environment.GetEnvironmentVariable("LINUX_VM_CPUS") ?? "2";
                        string memory = Environment.GetEnvironmentVariable("LINUX_VM_MEMORY") ?? "4G";
                        string disk = Environment.GetEnvironmentVariable("LINUX_VM_DISK") ?? "20G";
                        string os = Environment.GetEnvironmentVariable("LINUX_VM_OS") ?? "22.04";

                        // exec multipass with cloud config to automate creation of linux HyperV VM
                        string cmd = $"multipass launch {os} --cpus {cpus} --memory {memory} --disk {disk} --cloud-init assets/cloud-config.yaml -n {vmName}";
                        return RunCommand(cmd)?.Contains($"Launched: {vmName}") == true;
                    }

                case VMType.HyperV:
                    {
                        // vm specs
                        string cpus = Environment.GetEnvironmentVariable("WIN_VM_CPUS") ?? "2";
                        string memory = Environment.GetEnvironmentVariable("WIN_VM_MEMORY") ?? "4G";
                        string disk = Environment.GetEnvironmentVariable("WIN_VM_DISK") ?? "40G";

                        // OS stuff
                        string vhdUrl = Environment.GetEnvironmentVariable("WIN_VM_VHD") ?? "";
                        string isoUrl = Environment.GetEnvironmentVariable("WIN_VM_ISO") ?? "";
                        int vmGeneration = Int32.Parse(Environment.GetEnvironmentVariable("WIN_VM_GEN") ?? "1");

                        // where to put the unattended file (bootdisk, iso)
                        string unattendedMethod = "iso";

                        // config paths
                        string vmBasePath = Path.Combine("C:\\VMs", vmName);
                        string vhdPath = Path.Combine(vmBasePath, $"{vmName}.vhd");
                        string vhdxPath = Path.Combine(vmBasePath, $"{vmName}.vhdx");
                        string isoPath = Path.Combine(vmBasePath, $"{vmName}.iso");

                        // set path for vhd/vhdx file depending on target VM generation
                        string bootDisk = vmGeneration == 1 ? vhdPath : vhdxPath;

                        // check if VM already exists,
                        // and if so, ask if it should be recreated
                        RecreateCheck(type, vmName);

                        // Create base directory for VM config
                        _uiOutputLogger?.Invoke($"Creating VM base path: {vmBasePath}", LogLevel.Info);
                        Directory.CreateDirectory(vmBasePath);

                        // download specified VHD file
                        if (!string.IsNullOrWhiteSpace(vhdUrl))
                        {
                            DownloadVHD(vhdPath, vhdxPath, vhdUrl, vmGeneration);
                        }

                        // download specified iso file
                        if (!string.IsNullOrWhiteSpace(isoUrl))
                        {
                            DownloadISO(isoUrl, isoPath);
                        }

                        string unattendIso = Path.Combine(vmBasePath, "autounattend.iso");
                        // Build autounattend ISO to skip VM setup
                        if (unattendedMethod == "iso")
                        {
                            _uiOutputLogger?.Invoke("Creating autounattend ISO...", LogLevel.Info);

                            string unattendSource = Path.Combine(AppContext.BaseDirectory, "Assets", "autounattend.xml");
                            string isoStagingDir = Path.Combine(vmBasePath, "unattend_staging");

                            // Build filesystem
                            Directory.CreateDirectory(isoStagingDir);

                            // Copy file to root of ISO staging directory with correct casing
                            string destPath = Path.Combine(isoStagingDir, "autounattend.xml");
                            File.Copy(unattendSource, destPath, true);

                            _uiOutputLogger?.Invoke($"Copied unattend file to staging: {destPath}", LogLevel.Info);

                            // create the iso
                            _uiOutputLogger?.Invoke("Creating autounattend ISO...", LogLevel.Info);
                            CreateIsoFromDirectory(isoStagingDir, unattendIso);
                            _uiOutputLogger?.Invoke($"ISO created at: {unattendIso}", LogLevel.Info);
                        }

                        //if (unattendedMethod == "bootdisk")
                        //{
                        //    string unattendSource = Path.Combine(AppContext.BaseDirectory, "Assets", "autounattend.xml");
                        //    _uiOutputLogger?.Invoke("Injecting autounattend.xml into VHD...", LogLevel.Info);

                        //    // Mount the VHDX
                        //    string mountCmd = $@"powershell -NoProfile -Command ""Mount-VHD -Path '{bootDisk}' -PassThru | Get-Disk | Get-Partition | Get-Volume | Select -ExpandProperty DriveLetter""";
                        //    string driveLetterOutput = RunCommand(mountCmd, true);

                        //    // Extract the drive letter (should be a single letter, e.g., "E")
                        //    char driveLetter = driveLetterOutput.Trim().FirstOrDefault();
                        //    if (char.IsLetter(driveLetter))
                        //    {
                        //        string volumeRoot = $"{driveLetter}:\\";
                        //        string destPath = Path.Combine(volumeRoot, "Autounattend.xml");

                        //        // Copy file
                        //        File.Copy(unattendSource, destPath, true);
                        //        _uiOutputLogger?.Invoke($"Copied unattend file to {destPath}", LogLevel.Info);

                        //        // Dismount
                        //        string dismountCmd = $@"powershell -NoProfile -Command ""Dismount-VHD -Path '{bootDisk}'""";
                        //        RunCommand(dismountCmd, true);
                        //        _uiOutputLogger?.Invoke("VHD dismounted.", LogLevel.Info);
                        //    }
                        //    else
                        //    {
                        //        _uiOutputLogger?.Invoke("[ERR] Failed to resolve mounted volume drive letter.", LogLevel.Error);
                        //    }
                        //}

                        // verify boot disk exists
                        if (!File.Exists(bootDisk))
                        {
                            _uiOutputLogger?.Invoke($"Creating empty virtual disk at {bootDisk}...", LogLevel.Info);

                            string createVhdCmd = $@"powershell -NoProfile -Command ""New-VHD -Path '{bootDisk}' -SizeBytes {ParseMemory(disk)} -Dynamic""";
                            RunCommand(createVhdCmd, true);

                            _uiOutputLogger?.Invoke($"VHD created at {bootDisk}", LogLevel.Info);
                        }


                        // start creation of VM
                        var commands = new List<string>
                        {
                            $"New-VM -Name '{vmName}' -MemoryStartupBytes {ParseMemory(memory)} -Generation {vmGeneration} -VHDPath '{bootDisk}' -Path '{vmBasePath}' -SwitchName 'Default Switch'",
                            $"Set-VMProcessor -VMName '{vmName}' -Count {cpus}"
                        };

                        // attach install ISO
                        commands.Add($"Set-VMDvdDrive -VMName '{vmName}' -Path '{isoPath}' -ControllerNumber 1 -ControllerLocation 0 -ErrorAction SilentlyContinue");

                        // attach unattended ISO
                        if (unattendedMethod == "iso")
                        {
                            commands.Add($"Add-VMDvdDrive -VMName '{vmName}' -Path '{unattendIso}' -ControllerNumber 1 -ControllerLocation 1 -ErrorAction SilentlyContinue");
                        }

                        // set boot to install ISO (Gen 2 only)
                        if (vmGeneration == 2)
                        {
                            commands.Add($"Set-VMFirmware -VMName '{vmName}' -FirstBootDevice (Get-VMDvdDrive -VMName '{vmName}' | Where-Object {{$_.Path -eq '{isoPath}'}})");
                        }

                        // finally start VM
                        commands.Add($"Start-VM -Name '{vmName}'");

                        bool ret = false;
                        foreach (var command in commands)
                        {
                            _uiOutputLogger?.Invoke($"> {command}", LogLevel.Info);
                            string cmd = $@"powershell -NoProfile -Command ""{command}""";
                            ret = RunCommand(cmd) != null;
                        }

                        return ret;
                    }

                default:
                    return false;
            }
        }

        public void DownloadVHD(string vhdPath, string vhdxPath, string vhdUrl, int vmGeneration)
        {
            DownloadFile(vhdUrl, vhdPath);

            // if vm is gen2 then needs to be vhdx
            if (vmGeneration == 2 && File.Exists(vhdPath) && !File.Exists(vhdxPath))
            {
                _uiOutputLogger?.Invoke($"Converting {Path.GetFileName(vhdPath)} to VHDX...", LogLevel.Info);
                string convertCmd = $@"powershell -NoProfile -Command ""Convert-VHD -Path '{vhdPath}' -DestinationPath '{vhdxPath}' -VHDType Dynamic -Verbose""";
                string output = null;
                var task = Task.Run(() => output = RunCommand(convertCmd, true));

                while (!task.IsCompleted)
                {
                    _uiOutputLogger?.Invoke($"Converting {Path.GetFileName(vhdPath)}...", LogLevel.Info);
                    Thread.Sleep(1000);
                }

                task.Wait();

                if (output != null)
                    _uiOutputLogger?.Invoke($"Conversion complete: {Path.GetFileName(vhdxPath)}", LogLevel.Info);
                else
                    _uiOutputLogger?.Invoke($"VHD conversion failed for {Path.GetFileName(vhdPath)}", LogLevel.Error);
            }
        }

        public void DownloadISO(string isoUrl, string isoPath)
        {
            DownloadFile(isoUrl, isoPath);
        }

        public bool RecreateCheck(VMType type, string vmName)
        {
            switch (type)
            {
                case VMType.HyperV:
                    {
                        // check check if Windows HyperV VM already exists
                        string checkVmCmd = $@"powershell -NoProfile -Command ""if (Get-VM -Name '{vmName}' -ErrorAction SilentlyContinue) {{ Write-Output 'exists' }}""";
                        string result = RunCommand(checkVmCmd, true);
                        bool vmExists = result != null && result.Contains("exists");

                        // offer removal of existing, allowing forced recreation of vm
                        if (vmExists)
                        {
                            _uiOutputLogger?.Invoke($"VM '{vmName}' already exists.", LogLevel.Warning);

                            var response = MessageBox.Show(
                                $"A virtual machine named '{vmName}' already exists.\nDo you want to overwrite it?",
                                "VM Exists",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Warning
                            );

                            if (response != DialogResult.Yes)
                            {
                                _uiOutputLogger?.Invoke("VM creation cancelled by user.", LogLevel.Info);
                                return false;
                            }

                            string removeCmd = $@"powershell -NoProfile -Command "" $vm = Get-VM -Name '{vmName}' -ErrorAction SilentlyContinue;if ($vm.State -ne 'Off') {{ Stop-VM -Name '{vmName}' -Force -TurnOff -Confirm:\$false }};Remove-VM -Name '{vmName}' -Force""";
                            RunCommand(removeCmd, true);
                            _uiOutputLogger?.Invoke($"Removed existing VM '{vmName}'.", LogLevel.Info);
                        }
                        return false;
                    }
                case VMType.Multipass:
                    {
                        return false;
                    }
                default:
                    return false;
            }
        }

        public bool DeleteVM(VMType type, string vmName)
        {
            return type switch
            {
                VMType.Multipass => RunCommand($"multipass delete {vmName} && multipass purge") != null,
                VMType.HyperV => RunCommand($"powershell -NoProfile -Command \"Remove-VM -Name '{vmName}' -Force\"") != null,
                _ => false
            };
        }

        private VMStatusResult GetMultipassVMStatus(string vmName)
        {
            string output = RunCommand($"multipass list | findstr /i \"{vmName}\"");
            if (string.IsNullOrWhiteSpace(output))
                return new VMStatusResult { State = "Not Found", Level = LogLevel.Warning };

            if (output.IndexOf("Running", StringComparison.OrdinalIgnoreCase) >= 0)
                return new VMStatusResult { State = "Running", Level = LogLevel.Success };

            return new VMStatusResult { State = "Stopped", Level = LogLevel.Warning };
        }

        private VMStatusResult GetHyperVVMStatus(string vmName)
        {
            string output = RunCommand($"powershell -NoProfile -Command \"Get-VM -Name '{vmName}' -ErrorAction SilentlyContinue | Select-Object -ExpandProperty State\""); if (output.Contains("unable to find") || string.IsNullOrWhiteSpace(output))
                return new VMStatusResult { State = "Not Found", Level = LogLevel.Warning };

            string state = output.Trim();
            LogLevel level = state.Equals("Running", StringComparison.OrdinalIgnoreCase) ? LogLevel.Success : LogLevel.Warning;

            return new VMStatusResult { State = state, Level = level };
        }

        public string RunCommand(string command, bool waitForExit = false)
        {
            try
            {
                _uiOutputLogger?.Invoke($"> {command}", LogLevel.Info);

                var psi = new ProcessStartInfo("cmd.exe", "/c " + command)
                {
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (var process = Process.Start(psi))
                {
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();

                    if (waitForExit)
                        process.WaitForExit();

                    if (!string.IsNullOrWhiteSpace(output))
                        _uiOutputLogger?.Invoke(output, LogLevel.Info);

                    if (!string.IsNullOrWhiteSpace(error))
                        _uiOutputLogger?.Invoke("[ERR] " + error, LogLevel.Error);

                    return error + Environment.NewLine + output;
                }
            }
            catch (Exception ex)
            {
                _uiOutputLogger?.Invoke("[EX] " + ex.Message, LogLevel.Error);
                return null;
            }
        }

        public void RunCommand(string command, Action<string> outputCallback, Action<bool> onExit = null)
        {
            try
            {
                _uiOutputLogger?.Invoke($"> {command}", LogLevel.Info);

                var psi = new ProcessStartInfo("cmd.exe", "/c " + command)
                {
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                var process = new Process
                {
                    StartInfo = psi,
                    EnableRaisingEvents = true
                };

                process.OutputDataReceived += (s, e) =>
                {
                    if (e.Data != null)
                    {
                        outputCallback?.Invoke(e.Data);
                        _uiOutputLogger?.Invoke(e.Data, LogLevel.Info);
                    }
                };

                process.ErrorDataReceived += (s, e) =>
                {
                    if (e.Data != null)
                    {
                        outputCallback?.Invoke("[ERR] " + e.Data);
                        _uiOutputLogger?.Invoke("[ERR] " + e.Data, LogLevel.Error);
                    }
                };

                process.Exited += (s, e) =>
                {
                    bool success = process.ExitCode == 0;
                    onExit?.Invoke(success);
                    process.Dispose();
                };

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
            }
            catch (Exception ex)
            {
                outputCallback?.Invoke("[EX] " + ex.Message);
                _uiOutputLogger?.Invoke("[EX] " + ex.Message, LogLevel.Error);
                onExit?.Invoke(false);
            }
        }
    }
}
