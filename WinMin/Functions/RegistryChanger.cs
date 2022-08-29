using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
                        //TODO: Need to write the value of the key to a file
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
        
        public static void SetUserRegistry(Button button, string keyName, string newValue, string oldValue, string keyPath)
        {
            RegistryKey key = Registry.Users.OpenSubKey(keyPath, true);
            int buttonTag = (int)button.Tag;
            if (buttonTag == 1)
            {
                if (key != null)
                {
                    key.SetValue(keyName, newValue);
                    key.Close();
                }
                button.Tag = 0;
                button.Content = "Disable";
            }
            else
            {
                if (key != null)
                {
                    key.SetValue(keyName, oldValue);
                    key.Close();
                }
                button.Tag = 1;
                button.Content = "Enable";
            }
        }
    }
}
