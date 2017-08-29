using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CortanaCommandServer
{
    public class ServerSetting:ObservableObject
    {
        public ServerSetting()
        {
            PowerShellPath = @"C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe";
            PassCode = "";
            IsAutoStart = false;
        }

        private string _passCode;

        public string PassCode
        {
            get { return _passCode; }
            set { this.Set(ref _passCode, value); }
        }

        private string _powershellPath;

        public string PowerShellPath
        {
            get { return _powershellPath; }
            set { this.Set(ref _powershellPath, value); }
        }

        private bool _isAutoStart;

        public bool IsAutoStart
        {
            get { return _isAutoStart; }
            set { this.Set(ref _isAutoStart, value); }
        }

        


    }
}
