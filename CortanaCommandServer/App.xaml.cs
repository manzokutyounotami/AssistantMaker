using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CortanaCommandServer
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        public static ServerViewModel ViewModel;
        private const string _saveFileName = "saveData";
        public App()
        {
            ViewModel = new ServerViewModel();
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            StreamReader reader = null;
            try {
                if (File.Exists(_saveFileName))
                {
                    reader = new StreamReader(_saveFileName);
                    var json = reader.ReadToEndAsync().Result;
                    ViewModel.Setting = (ServerSetting)JsonConvert.DeserializeObject(json,typeof(ServerSetting));
                    reader.Close();
                }
            }catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                reader.Close();
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            var json = JsonConvert.SerializeObject(ViewModel.Setting);
            var writer = new StreamWriter(_saveFileName);
            writer.WriteAsync(json).Wait();
            writer.Close();
            try {
                if (ViewModel.Setting.IsAutoStart)
                {
                    SetStartUp();
                }
                else
                {
                    DeleteStartUp();
                }
            }catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void SetStartUp()
        {
            var runFile = Environment.GetCommandLineArgs()[0];
            Microsoft.Win32.RegistryKey regkey =
                Microsoft.Win32.Registry.CurrentUser.OpenSubKey(
                @"Software\Microsoft\Windows\CurrentVersion\Run", true);
            regkey.SetValue("CortanaCommandServer",runFile);
            regkey.Close();
        }

        private void DeleteStartUp()
        {
            Microsoft.Win32.RegistryKey regkey =
                Microsoft.Win32.Registry.CurrentUser.OpenSubKey(
                @"Software\Microsoft\Windows\CurrentVersion\Run", true);
            regkey.DeleteValue("CortanaCommandServer",true);
            regkey.Close();
        }
    }
}
