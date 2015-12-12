using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CortanaCommandCore.Model
{
    public class CommandPreview:ObservableObject
    {
        private string _title;

        public string Title
        {
            get { return _title; }
            set { this.Set(ref _title,value); }
        }

        private string _description;

        public string Description
        {
            get { return _description; }
            set { this.Set(ref _description,value); }
        }

        public CommandPreview(string title,string description)
        {
            this.Title = title;
            this.Description = description;
        }

    }
}
