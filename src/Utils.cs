using IMAPI2FS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ClusterJockey
{
    internal class Utils
    {
        private readonly Action<string, LogLevel> _uiOutputLogger;

        public Utils(Action<string, LogLevel> uiOutputLogger)
        {
            _uiOutputLogger = uiOutputLogger;
        }

        public void DownloadFile(string downloadUrl, string targetPath)
        {
            // Download VHD file specified from env
            if (File.Exists(targetPath))
            {
                _uiOutputLogger?.Invoke($"File already exists: {downloadUrl}", LogLevel.Info);
                return;
            }
            _uiOutputLogger?.Invoke($"Downloading file from: {downloadUrl}", LogLevel.Info);

            using (var httpClient = new HttpClient())
            using (var response = httpClient.GetAsync(downloadUrl, HttpCompletionOption.ResponseHeadersRead).Result)
            using (var stream = response.Content.ReadAsStreamAsync().Result)
            using (var fileStream = new FileStream(targetPath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                response.EnsureSuccessStatusCode();
                long totalBytes = response.Content.Headers.ContentLength ?? 0;
                byte[] buffer = new byte[8192];
                int bytesRead;
                long downloadedBytes = 0;
                int lastLoggedPercent = -1;

                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    fileStream.Write(buffer, 0, bytesRead);
                    downloadedBytes += bytesRead;

                    if (totalBytes > 0)
                    {
                        int percent = (int)(downloadedBytes * 100 / totalBytes);
                        if (percent >= lastLoggedPercent + 5)
                        {
                            lastLoggedPercent = percent;
                            _uiOutputLogger?.Invoke($"[{Path.GetFileName(downloadUrl)}] Downloaded {percent}% ({downloadedBytes / (1024 * 1024)} MB)/({totalBytes / (1024 * 1024)} MB)", LogLevel.Info);
                        }
                    }
                    else if (downloadedBytes >= (lastLoggedPercent + 10) * 1024 * 1024)
                    {
                        lastLoggedPercent += 10;
                        _uiOutputLogger?.Invoke($"Downloaded {downloadedBytes / (1024 * 1024)} MB...", LogLevel.Info);
                    }
                }

                _uiOutputLogger?.Invoke($"[{Path.GetFileName(downloadUrl)}] Download complete ({downloadedBytes / (1024 * 1024)} MB)", LogLevel.Info);
            }
        }

        public void CreateIsoFromDirectory(string sourceDir, string isoPath)
        {
            var fsImage = new IMAPI2FS.MsftFileSystemImage();
            fsImage.ChooseImageDefaultsForMediaType(IMAPI2FS.IMAPI_MEDIA_PHYSICAL_TYPE.IMAPI_MEDIA_TYPE_DISK);
            fsImage.FileSystemsToCreate = FsiFileSystems.FsiFileSystemJoliet | FsiFileSystems.FsiFileSystemISO9660;
            fsImage.VolumeName = "AUTO";
            fsImage.Root.AddTree(sourceDir, false);

            IMAPI2FS.IFileSystemImageResult result = fsImage.CreateResultImage();

            // Marshal FsiStream to IStream
            object fsiStream = result.ImageStream;
            IntPtr unk = Marshal.GetIUnknownForObject(fsiStream);
            var imageStream = (System.Runtime.InteropServices.ComTypes.IStream)
                Marshal.GetTypedObjectForIUnknown(unk, typeof(System.Runtime.InteropServices.ComTypes.IStream));

            using (var isoStream = new FileStream(isoPath, FileMode.Create, FileAccess.Write))
            {
                var buffer = new byte[8192];
                IntPtr readPtr = Marshal.AllocHGlobal(sizeof(int));
                try
                {
                    while (true)
                    {
                        imageStream.Read(buffer, buffer.Length, readPtr);
                        int bytesRead = Marshal.ReadInt32(readPtr);
                        if (bytesRead <= 0)
                            break;
                        isoStream.Write(buffer, 0, bytesRead);
                    }
                }
                finally
                {
                    Marshal.FreeHGlobal(readPtr);
                }
            }
        }
    }
}
