using CortanaCommandCore.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace CortanaCommandServer
{
    public class ServerViewModel:ViewModelBase
    {
        TcpListener listner;
        TcpClient client;
        NetworkStream stream;

        private ObservableCollection<string> _logList;

        public ObservableCollection<string> LogList
        {
            get
            {
                return _logList;
            }

            set
            {
                this.Set(ref _logList,value);
            }
        }

        public ServerViewModel()
        {
            StartServerCommand = new RelayCommand(async()=>
            {
                await StartServerAsync();
            });

            StopServerCommand = new RelayCommand(()=>
            {
                if(client != null)
                {
                    client.Close();
                }
                if (listner != null)
                {
                    listner.Stop();
                    listner = null;
                }
            });

            LogList = new ObservableCollection<string>();
        }

        private async Task StartServerAsync()
        {
            try {
                
                listner = new TcpListener(IPAddress.Parse("127.0.0.1"), int.Parse(SettingManager.ServerPort));
                listner.Start();
                LogList.Add("ServerListen 127.0.0.1 Port=" + SettingManager.ServerPort);
                client = await listner.AcceptTcpClientAsync();
                LogList.Add("Client Accepted");
                stream = client.GetStream();
                while (true)
                {
                    if (client.Available > 0) {
                        byte[] buff = new byte[client.Available];
                        var read = await stream.ReadAsync(buff, 0, buff.Length);
                        var json = Encoding.UTF8.GetString(buff);
                        var data = JsonConvert.DeserializeObject<ConnectionData>(json);
                        if (data.AcceptPass == SettingManager.AcceptPass)
                        {
                            LogList.Add("Script Run ["+data.Script+"]");
                            Process.Start(@"C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe",data.Script);
                        }
                        client.Close();
                        listner.Stop();
                        await StartServerAsync();
                        break;
                    }
                    else
                    {
                        await Task.Delay(100);
                    }
                }
            }
            catch (Exception e)
            {
                LogList.Add(e.Message);
                client.Close();
                listner.Stop();
                await StartServerAsync();
            }
        }

        

        public RelayCommand StartServerCommand { get; set; }

        public RelayCommand StopServerCommand { get; set; }


    }
}
