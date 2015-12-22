using CortanaCommand.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CortanaCommandCore.ViewModel
{
    public class ScriptStateViewModel:StateViewModel
    {
        public string Script
        {
            get
            {
                return _script;
            }

            set
            {
                this.Set(ref _script, value);
            }
        }

        private string _utterance;

        public string Utterance
        {
            get
            {
                return _utterance;
            }

            set
            {
                this.Set(ref _utterance, value);
            }
        }

        private string _script;

        public ScriptStateViewModel()
        {
            Utterance = "";
            Script = "Get-ChildItem";
            StateCategory = "スクリプト実行状態";   
        }
    }
}
