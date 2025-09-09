using System;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.Security.Principal;
using System.Windows.Forms;
using System.Xml;

namespace ClusterJockey
{
    public partial class MainForm : Form
    {
        private Logger _logger;
        private VMManager _vmManager;
        private System.Windows.Forms.Timer _vmStatusTimer;

        public MainForm()
        {
            InitializeComponent();
            _logger = new Logger(consoleOutputListView);
            _vmManager = new VMManager((line, level) =>
            {
                _logger.Log("VMManager", line, level);
            });

            Load += (s, e) =>
            {
                EnsureRequirements();
                InitVMStatusTimer();
            };
        }

        private void EnsureRequirements()
        {
            bool isAdmin = new WindowsPrincipal(WindowsIdentity.GetCurrent())
                .IsInRole(WindowsBuiltInRole.Administrator);

            if (!isAdmin)
            {
                MessageBox.Show("This application requires administrative privileges.", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
            }

            bool hypervOk = CheckAndLog(
                "Checking if Hyper-V is enabled...",
                "systeminfo | findstr /i \"Hyper-V\"",
                "Hyper-V is detected.",
                "Hyper-V is NOT detected.");

            bool multipassOk = CheckAndLog(
                "Checking if Multipass is installed...",
                "where multipass",
                "Multipass is installed.",
                "Multipass is NOT installed.");

            if (!hypervOk || !multipassOk)
            {
                string reason = "";
                if (!hypervOk) reason += "- Hyper-V is NOT enabled.\n";
                if (!multipassOk) reason += "- Multipass is NOT installed.\n";

                MessageBox.Show(
                    "Missing system requirements:\n" + reason + "\nApplication will now exit.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                Application.Exit();
            }
        }

        private void InitVMStatusTimer()
        {
            UpdateVMStatusUI();
            _vmStatusTimer = new System.Windows.Forms.Timer();
            _vmStatusTimer.Interval = 10000;
            _vmStatusTimer.Tick += (s, e) => UpdateVMStatusUI();
            _vmStatusTimer.Start();
        }

        private void UpdateVMStatusUI()
        {
            var ubuntuStatus = _vmManager.GetVMStatus(VMType.Multipass, _vmManager.GetVMName(VMType.Multipass));
            _logger.Log("Multipass", $"VM status: {ubuntuStatus.State}", ubuntuStatus.Level);
            linuxGroupBox.Text = $"Linux VM: {ubuntuStatus.State}";

            var hypervStatus = _vmManager.GetVMStatus(VMType.HyperV, _vmManager.GetVMName(VMType.HyperV));
            _logger.Log("Hyper-V", $"VM status: {hypervStatus.State}", hypervStatus.Level);
            windowsGroupBox.Text = $"Windows VM: {hypervStatus.State}";

            // check for poll cmd
            string command = Environment.GetEnvironmentVariable("VM_POLL_CMD") ?? "";
            if (!string.IsNullOrEmpty(command))
            {
                // lin
                if (ubuntuStatus.State.Equals("Running", StringComparison.OrdinalIgnoreCase))
                {
                    RunCommand(command, VMType.Multipass);
                }

                // win
                if (hypervStatus.State.Equals("Running", StringComparison.OrdinalIgnoreCase))
                {
                    RunCommand(command, VMType.HyperV);
                }
            }
        }

        private void RunCommand(string command, VMType vmType)
        {
            // log
            _logger.Log(vmType.ToString(), $"[RunCommand] {command}", LogLevel.Info);

            // identify output control
            RichTextBox output = vmType == VMType.HyperV ? windowsTextBox : linuxTextBox;

            // print command
            output.AppendText($"> {command}{Environment.NewLine}");
            output.SelectionStart = output.Text.Length;
            output.ScrollToCaret();

            // run command
            string result = _vmManager.RunOnVM(vmType, _vmManager.GetVMName(vmType), command);
            if (!string.IsNullOrWhiteSpace(result))
            {
                // log
                _logger.Log(vmType.ToString(), result.Trim(), LogLevel.Info);

                // print output
                output.AppendText($"{result}{Environment.NewLine}");
                output.SelectionStart = output.Text.Length;
                output.ScrollToCaret();
            }
        }

        private bool CheckAndLog(string checkingMsg, string command, string successMsg, string failureMsg, LogLevel failureLevel = LogLevel.Error)
        {
            _logger.Log(this.Text, checkingMsg, LogLevel.Info);
            return RunCommandWithOutputLogging(command, successMsg, failureMsg, failureLevel);
        }

        private bool RunCommandWithOutputLogging(string command, string successMsg, string failureMsg, LogLevel failureLevel = LogLevel.Error)
        {
            string outputBuffer = "";
            var done = new AutoResetEvent(false);
            bool success = false;

            _logger.Log(this.Text, $"> {command}", LogLevel.Info);
            _vmManager.RunCommand(command,
                line =>
                {
                    if (line == null) return;

                    outputBuffer += line + Environment.NewLine;
                    _logger.Log(this.Text, $"[RunCommandWithOutputLogging] {line}", LogLevel.Info);
                },
                result =>
                {
                    success = result && !string.IsNullOrWhiteSpace(outputBuffer);
                    done.Set();
                });

            done.WaitOne(); // block until the command finishes

            _logger.Log(this.Text, success ? successMsg : failureMsg, success ? LogLevel.Success : failureLevel);
            return success;
        }


        // private void RunAndDisplay(string command)
        // {
        //     // commandOutputRichTextBox.Clear();

        //     _vmManager.RunCommand(command,
        //         line =>
        //         {
        //             if (commandOutputRichTextBox.InvokeRequired)
        //             {
        //                 commandOutputRichTextBox.Invoke(new Action(() =>
        //                     commandOutputRichTextBox.AppendText(line + Environment.NewLine)));
        //             }
        //             else
        //             {
        //                 commandOutputRichTextBox.AppendText(line + Environment.NewLine);
        //             }
        //         },
        //         success =>
        //         {
        //             _logger.Log(
        //                 this.Text,
        //                 success ? "Command completed successfully." : "Command failed.",
        //                 success ? LogLevel.Success : LogLevel.Error
        //             );
        //         });
        // }

        private void mainFormToolStripFileExitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void mainFormToolStripViewConsoleButton_Click(object sender, EventArgs e)
        {
            mainFormSplitContainer.Panel2Collapsed = !mainFormSplitContainer.Panel2Collapsed;
        }

        private void linuxGroupBoxCreateButton_Click(object sender, EventArgs e)
        {
            var btn = (ToolStripButton)sender;
            btn.Enabled = false;
            _logger.Log(this.Text, "Creating Multipass Linux VM...", LogLevel.Info);
            Task.Run(() =>
            {
                bool created = _vmManager.CreateVM(VMType.Multipass, _vmManager.GetVMName(VMType.Multipass));
                Invoke(new Action(() =>
                {
                    _logger.Log(
                        this.Text,
                        created ? "Multipass VM created successfully." : "Failed to create Multipass VM.",
                        created ? LogLevel.Success : LogLevel.Error
                    );
                    UpdateVMStatusUI();
                    btn.Enabled = true;
                }));
            });
        }

        private void linuxGroupBoxStartButton_Click(object sender, EventArgs e)
        {
            var btn = (ToolStripButton)sender;
            btn.Enabled = false;
            _logger.Log(this.Text, "Starting Multipass Linux VM...", LogLevel.Info);
            Task.Run(() =>
            {
                bool started = _vmManager.StartVM(VMType.Multipass, _vmManager.GetVMName(VMType.Multipass));
                Invoke(new Action(() =>
                {
                    _logger.Log(
                        this.Text,
                        started ? "Multipass VM started." : "Failed to start Multipass VM.",
                        started ? LogLevel.Success : LogLevel.Error
                    );
                    UpdateVMStatusUI();
                    btn.Enabled = true;
                }));
            });
        }

        private void linuxGroupBoxStopButton_Click(object sender, EventArgs e)
        {
            var btn = (ToolStripButton)sender;
            btn.Enabled = false;
            _logger.Log(this.Text, "Stopping Multipass Linux VM...", LogLevel.Info);
            Task.Run(() =>
            {
                bool stopped = _vmManager.StopVM(VMType.Multipass, _vmManager.GetVMName(VMType.Multipass));
                Invoke(new Action(() =>
                {
                    _logger.Log(
                        this.Text,
                        stopped ? "Multipass VM stopped." : "Failed to stop Multipass VM.",
                        stopped ? LogLevel.Success : LogLevel.Error
                    );
                    UpdateVMStatusUI();
                    btn.Enabled = true;
                }));
            });
        }

        private void linuxGroupBoxDeleteButton_Click(object sender, EventArgs e)
        {
            var btn = (ToolStripButton)sender;
            btn.Enabled = false;
            _logger.Log(this.Text, "Deleting Multipass Linux VM...", LogLevel.Info);
            Task.Run(() =>
            {
                bool deleted = _vmManager.DeleteVM(VMType.Multipass, _vmManager.GetVMName(VMType.Multipass));
                Invoke(new Action(() =>
                {
                    _logger.Log(
                        this.Text,
                        deleted ? "Multipass VM deleted." : "Failed to delete Multipass VM.",
                        deleted ? LogLevel.Success : LogLevel.Error
                    );
                    UpdateVMStatusUI();
                    btn.Enabled = true;
                }));
            });
        }

        private void windowsGroupBoxCreateButton_Click(object sender, EventArgs e)
        {
            var btn = (ToolStripButton)sender;
            btn.Enabled = false;
            _logger.Log(this.Text, "Creating Hyper-V Windows VM...", LogLevel.Info);
            Task.Run(() =>
            {
                bool created = _vmManager.CreateVM(VMType.HyperV, _vmManager.GetVMName(VMType.HyperV));
                Invoke(new Action(() =>
                {
                    _logger.Log(
                        this.Text,
                        created ? "Hyper-V VM created successfully." : "Failed to create Hyper-V VM.",
                        created ? LogLevel.Success : LogLevel.Error
                    );
                    UpdateVMStatusUI();
                    btn.Enabled = true;
                }));
            });
        }

        private void windowsGroupBoxStartButton_Click(object sender, EventArgs e)
        {
            var btn = (ToolStripButton)sender;
            btn.Enabled = false;
            _logger.Log(this.Text, "Starting Hyper-V Windows VM...", LogLevel.Info);
            Task.Run(() =>
            {
                bool started = _vmManager.StartVM(VMType.HyperV, _vmManager.GetVMName(VMType.HyperV));
                Invoke(new Action(() =>
                {
                    _logger.Log(
                        this.Text,
                        started ? "Hyper-V VM started." : "Failed to start Hyper-V VM.",
                        started ? LogLevel.Success : LogLevel.Error
                    );
                    UpdateVMStatusUI();
                    btn.Enabled = true;
                }));
            });
        }

        private void windowsGroupBoxStopButton_Click(object sender, EventArgs e)
        {
            var btn = (ToolStripButton)sender;
            btn.Enabled = false;
            _logger.Log(this.Text, "Stopping Hyper-V Windows VM...", LogLevel.Info);
            Task.Run(() =>
            {
                bool stopped = _vmManager.StopVM(VMType.HyperV, _vmManager.GetVMName(VMType.HyperV));
                Invoke(new Action(() =>
                {
                    _logger.Log(
                        this.Text,
                        stopped ? "Hyper-V VM stopped." : "Failed to stop Hyper-V VM.",
                        stopped ? LogLevel.Success : LogLevel.Error
                    );
                    UpdateVMStatusUI();
                    btn.Enabled = true;
                }));
            });
        }

        private void windowsGroupBoxDeleteButton_Click(object sender, EventArgs e)
        {
            var btn = (ToolStripButton)sender;
            btn.Enabled = false;
            _logger.Log(this.Text, "Deleting Hyper-V Windows VM...", LogLevel.Info);
            Task.Run(() =>
            {
                bool deleted = _vmManager.DeleteVM(VMType.HyperV, _vmManager.GetVMName(VMType.HyperV));
                Invoke(new Action(() =>
                {
                    _logger.Log(
                        this.Text,
                        deleted ? "Hyper-V VM deleted." : "Failed to delete Hyper-V VM.",
                        deleted ? LogLevel.Success : LogLevel.Error
                    );
                    UpdateVMStatusUI();
                    btn.Enabled = true;
                }));
            });
        }

        private void mainFormToolStripViewMultipassButton_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("C:\\Program Files\\Multipass\\bin\\multipass.gui.exe"); // or check actual executable name
            }
            catch (Exception ex)
            {
                _logger.Log(this.Text, $"Failed to launch Multipass GUI: {ex.Message}", LogLevel.Error);
            }
        }

        private void mainFormToolStripViewHyperVButton_Click(object sender, EventArgs e)
        {
            try
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = Environment.ExpandEnvironmentVariables(@"%windir%\System32\mmc.exe"),
                    Arguments = Environment.ExpandEnvironmentVariables(@"%windir%\System32\virtmgmt.msc"),
                    WorkingDirectory = Environment.ExpandEnvironmentVariables(@"%windir%\System32")
                };

                Process.Start(startInfo);
            }
            catch (Exception ex)
            {
                _logger.Log(this.Text, $"Failed to launch Hyper-V Manager: {ex.Message}", LogLevel.Error);
            }
        }

        private void linuxCommandInputTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;

                string command = linuxCommandInputTextBox.Text.Trim();
                if (string.IsNullOrWhiteSpace(command)) return;

                linuxCommandInputTextBox.Clear();
                linuxTextBox.AppendText($"> {command}{Environment.NewLine}");
                linuxTextBox.SelectionStart = linuxTextBox.Text.Length;
                linuxTextBox.ScrollToCaret();

                string vmName = _vmManager.GetVMName(VMType.Multipass);

                Task.Run(() =>
                {
                    _vmManager.RunCommand($"multipass exec {vmName} -- {command}",
                    line =>
                    {
                        Invoke(new Action(() =>
                        {
                            linuxTextBox.AppendText($"{line}{Environment.NewLine}");
                            linuxTextBox.SelectionStart = linuxTextBox.Text.Length;
                            linuxTextBox.ScrollToCaret();
                        }));
                    },
                    success =>
                    {
                        // Invoke(new Action(() =>
                        // {
                        //     linuxTextBox.AppendText($"[VM] Command {(success ? "completed successfully." : "failed.")}{Environment.NewLine}");
                        // }));
                    });
                });
            }
        }

        private void windowsCommandInputTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;

                string command = windowsCommandInputTextBox.Text.Trim();
                if (string.IsNullOrWhiteSpace(command)) return;

                windowsCommandInputTextBox.Clear();
                

                RunCommand(command, VMType.HyperV);
            }
        }

        private void consoleOutputListView_DoubleClick(object sender, EventArgs e)
        {
            if (consoleOutputListView.SelectedItems.Count > 0)
            {
                var selectedItem = consoleOutputListView.SelectedItems[0];
                Clipboard.SetText(selectedItem.SubItems[2].Text);
                MessageBox.Show(selectedItem.SubItems[2].Text, $"{selectedItem.SubItems[0].Text} - {selectedItem.SubItems[1].Text}", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
