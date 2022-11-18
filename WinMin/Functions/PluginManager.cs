using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using static WinMin.MainWindow;

namespace WinMin.Functions
{
    internal class PluginManager
    {
        public readonly string patchPath = "C:\\Users\\Public\\WinMin\\Patches";
        public void WMPatchInstaller()
        {
            if (!Directory.Exists(patchPath))
            {
                Directory.CreateDirectory(patchPath);
            }
            if (!Directory.Exists($"{patchPath}\\Installed"))
            {
                Directory.CreateDirectory($"{patchPath}\\Installed");
            }
            foreach (string file in Directory.GetFiles(patchPath))
            {
                ZipArchive archive = ZipFile.OpenRead(file);
                var sample = archive.GetEntry("manifest.json");
                Stream zipEntryStream = sample.Open();
                StreamReader reader = new StreamReader(zipEntryStream);
                string json = reader.ReadToEnd();
                WMManifest manifest = JsonConvert.DeserializeObject<WMManifest>(json);
                if (manifest.supportedVersions.Contains(manifest.manifest))
                {
                    if (Directory.Exists($"{patchPath}\\Installed\\{manifest.name}"))
                        DeleteDirectory($"{patchPath}\\Installed\\{manifest.name}");
                    Directory.CreateDirectory($"{patchPath}\\Installed\\{manifest.name}");
                    archive.ExtractToDirectory($"{patchPath}\\Installed\\{manifest.name}");
                    archive.Dispose();
                    CreateUI(manifest);
                    File.Delete(file);
                    LoadFiles(manifest);
                }
                else
                {
                    MessageBox.Show("Manifest Version Not Supported");
                }
            }
        }
        public void LoadInstalled()
        {
            if (Directory.Exists($"{patchPath}\\Installed"))
            {
                string[] plugins = Directory.GetDirectories($"{patchPath}\\Installed");
                foreach (string plugin in plugins)
                {
                    string json = File.ReadAllText($"{plugin}\\manifest.json");
                    WMManifest manifest = JsonConvert.DeserializeObject<WMManifest>(json);
                    CreateUI(manifest);
                }
            }
        }
        private void DeleteDirectory(string target_dir)
        {
            string[] files = Directory.GetFiles(target_dir);
            string[] dirs = Directory.GetDirectories(target_dir);

            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (string dir in dirs)
            {
                DeleteDirectory(dir);
            }

            Directory.Delete(target_dir, false);
        }
        private void CreateUI(WMManifest manifest)
        {
            Grid grid = new Grid
            {
                Name = manifest.name,
                Height = 120,
                Width = 406,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            StackPanel panel = new StackPanel
            {
                Width = 310,
                HorizontalAlignment = HorizontalAlignment.Right
            };

            TextBlock text = new TextBlock
            {
                Height = 20,
                HorizontalAlignment = HorizontalAlignment.Left,
                Text = "Name: " + manifest.name
            };
            panel.Children.Add(text);

            TextBlock text2 = new TextBlock
            {
                Height = 20,
                HorizontalAlignment = HorizontalAlignment.Left,
                Text = "Version: " + manifest.version
            };
            panel.Children.Add(text2);

            TextBlock text3 = new TextBlock
            {
                Height = 20,
                HorizontalAlignment = HorizontalAlignment.Left,
                Text = "Author: " + manifest.author
            };
            panel.Children.Add(text3);

            TextBlock text4 = new TextBlock
            {
                Height = 52,
                HorizontalAlignment = HorizontalAlignment.Left,
                TextWrapping = TextWrapping.WrapWithOverflow,
                Text = "Description: " + manifest.description
            };
            panel.Children.Add(text4);

            StackPanel panel2 = new StackPanel
            {
                Width = 90,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.UriSource = new Uri($"{patchPath}\\Installed\\{manifest.name}\\{manifest.cover}", UriKind.RelativeOrAbsolute);
            bitmap.EndInit();
            Image image = new Image
            {
                Source = bitmap,
                Height = 90,
                Stretch = System.Windows.Media.Stretch.Uniform
            };
            panel2.Children.Add(image);

            Button button = new Button();
            button.VerticalAlignment = VerticalAlignment.Bottom;
            button.Content = "Remove";
            button.Tag = manifest.name;
            button.Height = 30;
            button.Click += Remove_Button;

            panel2.Children.Add(button);

            grid.Children.Add(panel);
            grid.Children.Add(panel2);
            window.WMInstalled.Items.Add(grid);
        }

        private void Remove_Button(object sender, RoutedEventArgs e)
        {
            string name = (string)(sender as Button).Tag;
            for (int x = window.WMInstalled.Items.Count - 1; x >= 0; --x)
            {
                Grid grid = (Grid)window.WMInstalled.Items[x];
                if(grid.Name.Equals(name))
                {
                    window.WMInstalled.Items.Remove(grid);
                    DeleteDirectory($"{patchPath}\\Installed\\{name}");
                    return;
                }
            }
        }

        private void LoadFiles(WMManifest manifest)
        {
            //Load all of the keys from the wmpatch file into the registry
            foreach (string keyPath in manifest.patchFiles)
            {
                Process process = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    FileName = "cmd.exe",
                    Arguments = $"/C reg import \"{patchPath}\\Installed\\{manifest.name}\\{keyPath}\""
                };
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();
            }
        }
    }
}