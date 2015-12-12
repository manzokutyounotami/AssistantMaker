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

        

        public SuccessStateViewModel()
        {
            Utterance = "";
            StateCategory = "正常応答状態";   
        }

        
    }
}
