using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CortanaCommand.ViewModel
{
    public class StateViewModel:ViewModelBase
    {
        private string _name;

        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                this.Set(ref _name, value);
            }
        }

        public string Example
        {
            get
            {
                return _example;
            }

            set
            {
                this.Set(ref _example,value);
            }
        }

        private string _example;

        public string ListenFor
        {
            get
            {
                return _listenFor;
            }

            set
            {
                this.Set(ref _listenFor,value);
            }
        }

        public string FirstListenFor
        {
            get
            {
                return ListenFor.Split('\n').First();
            }
        }

        public string FeedBack
        {
            get
            {
                return _feedBack;
            }

            set
            {
                this.Set(ref _feedBack,value);
            }
        }

        private string _listenFor;

        private string _feedBack;

        private string _stateCategory;

        private string _uniqueId;

        public string UniqueId
        {
            get { return _uniqueId; }
            set { this.Set(ref _uniqueId, value); }
        }
        

        public string StateCategory
        {
            get { return _stateCategory; }
            set { this.Set(ref _stateCategory,value); }
        }



        public StateViewModel()
        {
            Example = "こんにちはコルタナ";
            ListenFor = "[こんにちは]コルタナ";
            FeedBack = ".";
            StateCategory = "";
            this.UniqueId = Guid.NewGuid().ToString();
        }

        
    }
}
