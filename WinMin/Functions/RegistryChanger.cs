using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WinMin.Properties;

namespace WinMin.Functions
{
    internal class RegistryChanger
    {
        public static void LoadUserRegistry(Button button, string keyName, string keyPath)
        {
            RegistryKey key = Registry.Users.OpenSubKey(keyPath, true);

            string value;
            if (key != null)
            {
                //Key location exists
                if (key.GetValueNames().Contains(keyName))
                {
                    //Key Exists
                    value = key.GetValue(keyName).ToString();
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        //If Key is unblocked from WinMin
                        button.Tag = 0;
                        button.Content = "Disable";
                    }
                    else
                    {
                        //If Key is still blocked
                        button.Tag = 1;
                        button.Content = "Enable";
                        UpdateJson(keyName, value);
                    }
                }
                else
                {
                    //Key doesnt Exist
                    button.IsEnabled = false;
                    button.Content = "Not Blocked";
                }
            }
            else
            {
                //Key Location doesnt exists
                button.IsEnabled = false;
            }
        }
        
        public static void SetUserRegistry(Button button, string keyName, string newValue, string oldValue, string keyPath, RegistryValueKind valueKind)
        {
            RegistryKey key = Registry.Users.OpenSubKey(keyPath, true);
            int buttonTag = (int)button.Tag;
            if (buttonTag == 1)
            {
                if (key != null)
                {
                    key.SetValue(keyName, newValue, valueKind);
                    key.Close();
                }
                button.Tag = 0;
                button.Content = "Disable";
            }
            else
            {
                if (key != null)
                {
                    key.SetValue(keyName, oldValue, valueKind);
                    key.Close();
                }
                button.Tag = 1;
                button.Content = "Enable";
            }
        }
        public static void CreateJson()
        {
            if (!File.Exists($"C:\\Users\\Public\\WinMin\\Settings.json"))
            {
                JObject rss = new JObject(new JProperty("DefaultSettings", new JObject()));
                File.WriteAllText($"C:\\Users\\Public\\WinMin\\Settings.json", rss.ToString());
            }
        }
        public static void UpdateJson(string settingName, string settingValue)
        {
            string json = File.ReadAllText($"C:\\Users\\Public\\WinMin\\Settings.json");
            JObject rss = JObject.Parse(json);
            JObject defaultSettings = (JObject)rss["DefaultSettings"];
            JToken token = defaultSettings[settingName];
            if (token != null)
                defaultSettings.Property(settingName).Remove();
            defaultSettings.Add(new JProperty(settingName, settingValue));
            File.WriteAllText($"C:\\Users\\Public\\WinMin\\Settings.json", rss.ToString());
        }
        public static string DefaultReadValue(string settingName)
        {
            JObject rss = JObject.Parse(File.ReadAllText($"C:\\Users\\Public\\WinMin\\Settings.json"));
            return (string)rss["DefaultSettings"][settingName];
        }
    }
}
