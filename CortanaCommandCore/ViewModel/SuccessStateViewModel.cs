using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CortanaCommand.ViewModel
{
    public class SuccessStateViewModel:StateViewModel
    {
        private string _utterance;

        public string Utterance
        {
            get
            {
                return _utterance;
            }

            set
            {
                this.Set(ref _utterance,value);
            }
        }

        public string Script
        {
            get
            {
                return _script;
            }

            set
            {
                this.Set(ref _script,value);
            }
        }

        private string _script;

        public SuccessStateViewModel()
        {
            
        }

        
    }
}
