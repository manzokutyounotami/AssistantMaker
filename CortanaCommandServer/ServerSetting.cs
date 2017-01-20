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
            IsMinimizeStart = false;
            IsMinimizeOnClosing = false;
            UseTaskTray = false;
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

        private bool _isMinimizeStart;

        public bool IsMinimizeStart
        {
            get { return _isMinimizeStart; }
            set { this.Set(ref _isMinimizeStart, value); }
        }

        private bool _isMinimizeOnClosing;

        public bool IsMinimizeOnClosing
        {
            get { return _isMinimizeOnClosing; }
            set { this.Set(ref _isMinimizeOnClosing, value); }
        }

        private bool _useTaskTray;

        public bool UseTaskTray
        {
            get { return _useTaskTray; }
            set { this.Set(ref _useTaskTray, value); }
        }


    }
}
