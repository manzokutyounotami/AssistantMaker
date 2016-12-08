using CortanaCommand.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CortanaCommandCore.ViewModel
{

    public class ProtocolStateViewModel:StateViewModel
    {
        private string _protocol;

        public string Protocol
        {
            get { return _protocol; }
            set { this.Set(ref _protocol, value); }
        }

        public ProtocolStateViewModel()
        {
            this.Protocol = "ms-settings:";
            this.StateCategory = "UWP連携状態";
        }
    }
}
