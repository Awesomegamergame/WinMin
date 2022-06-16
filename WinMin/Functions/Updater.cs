using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Windows;
using System.Reflection;
using static WinMin.MainWindow;

namespace WinMin.Functions
{
    class Updater
    {
        public static string rootPath = AppDomain.CurrentDomain.BaseDirectory;
        public static string AppLink = "https://raw.githubusercontent.com/awesomegamergame/WinMin/master/WinMin/Webdata/WinMin%20Temp.zip";
        public static string AppVerLink = "https://raw.githubusercontent.com/awesomegamergame/WinMin/master/WinMin/Webdata/WinMinVersion.txt";
        public static string startPath = $"{rootPath}\\WinMin Temp";
        public static string AppZip = Path.Combine(rootPath, "WinMin Temp.zip");
        public static int VersionDetector = 0;
        public static Version onlineVersion;
        public static Version localVersion;
        public static bool IsOnline;
        public static void CheckInternetState()
        {
            try
            {
                using (var client = new WebClient())
                using (var stream = client.OpenRead("https://www.google.com"))
                {
                    IsOnline = true;
                }
            }
            catch
            {
                IsOnline = false;
            }
        }
        public static void Update()
        {
            string exeOld = Path.Combine(rootPath, "WinMin.exe.old");

            if (File.Exists(exeOld))
            {
                File.Delete(exeOld);
            }
            if (Directory.Exists(startPath))
            {
                Directory.Delete(startPath);
            }
            Assembly assembly = Assembly.GetExecutingAssembly();
            System.Version version = assembly.GetName().Version;
            string versionS = version.ToString();
            versionS = versionS.Substring(0, versionS.Length - 2);
            localVersion = new Version(versionS);

            try
            {
                WebClient webClient = new WebClient();
                onlineVersion = new Version(webClient.DownloadString(AppVerLink));
                if (onlineVersion.IsDifferentThan(localVersion))
                {
                    VersionDetector += 1;
                    window.UpdateScreen.Visibility = Visibility.Visible;
                    window.Yes.Visibility = Visibility.Visible;
                    window.No.Visibility = Visibility.Visible;
                    window.UpdateText1.Visibility = Visibility.Visible;
                    window.UpdateText2.Visibility = Visibility.Visible;
                    window.LocalVersion.Visibility = Visibility.Visible;
                    window.OnlineVersion.Visibility = Visibility.Visible;
                    window.LocalVersionNumber.Content = localVersion;
                    window.OnlineVersionNumber.Content = onlineVersion;
                    window.LocalVersionNumber.Visibility = Visibility.Visible;
                    window.OnlineVersionNumber.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error checking for application updates: {ex}");
            }
        }

        public static void InstallAppFiles(bool _isUpdate, Version _onlineVersion)
        {
            try
            {
                FileDownloader AppDownload = new FileDownloader();
                WebClient webClient = new WebClient();
                if (!_isUpdate)
                {
                    _onlineVersion = new Version(webClient.DownloadString(AppVerLink));
                }
                AppDownload.DownloadFileAsync(AppLink, AppZip, _onlineVersion);
                AppDownload.DownloadProgressChanged += AppDownload_DownloadProgressChanged;
                AppDownload.DownloadFileCompleted += new AsyncCompletedEventHandler(AppGameCompletedCallback);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error installing application files: {ex}");
            }
        }

        private static void AppDownload_DownloadProgressChanged(object sender, FileDownloader.DownloadProgress progress)
        {
            window.UpdateProgress.Value = progress.ProgressPercentage;
        }

        private static void AppGameCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            string Exe = Path.Combine(rootPath, "WinMin.exe");
            try
            {
                string onlineVersion = ((Version)e.UserState).ToString();
                if (!Directory.Exists(startPath))
                {
                    if (File.Exists(Exe))
                    {
                        File.Move($"{rootPath}\\WinMin.exe", $"{rootPath}\\WinMin.exe.old");
                    }
                    try
                    {
                        string tempPath = Path.Combine(rootPath, "tempUnzip");
                        ZipFile.ExtractToDirectory(AppZip, tempPath);

                        //build an array of the unzipped files
                        string[] files = Directory.GetFiles(tempPath);

                        foreach (string file in files)
                        {
                            FileInfo f = new FileInfo(file);
                            if (File.Exists(Path.Combine(rootPath, f.Name)))
                            {
                                File.Delete(Path.Combine(rootPath, f.Name));
                                File.Move(f.FullName, Path.Combine(rootPath, f.Name));
                            }
                            else
                            {
                                File.Move(f.FullName, Path.Combine(rootPath, f.Name));
                            }
                        }
                        Directory.Delete(tempPath);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                    File.Delete(AppZip);
                    Process.Start(Exe);
                    Application.Current.Shutdown();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error finishing download: {ex}");
            }
        }
        public static void UpdaterVersion()
        {
            if(VersionDetector == 1)
            {
                InstallAppFiles(true, onlineVersion);
            }
            else if(VersionDetector == 2)
            {
                InstallAppFiles(false, Version.zero);
            }
        }
    }

    public struct Version
    {
        internal static Version zero = new Version(0, 0, 0);

        private readonly short major;
        private readonly short minor;
        private readonly short subMinor;

        internal Version(short _major, short _minor, short _subMinor)
        {
            major = _major;
            minor = _minor;
            subMinor = _subMinor;
        }
        internal Version(string _version)
        {
            string[] _versionStrings = _version.Split('.');
            if (_versionStrings.Length != 3)
            {
                major = 0;
                minor = 0;
                subMinor = 0;
                return;
            }

            major = short.Parse(_versionStrings[0]);
            minor = short.Parse(_versionStrings[1]);
            subMinor = short.Parse(_versionStrings[2]);
        }
        internal bool IsDifferentThan(Version _otherVersion)
        {
            if (major != _otherVersion.major)
            {
                return true;
            }
            else
            {
                if (minor != _otherVersion.minor)
                {
                    return true;
                }
                else
                {
                    if (subMinor != _otherVersion.subMinor)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public override string ToString()
        {
            return $"{major}.{minor}.{subMinor}";
        }
    }
}