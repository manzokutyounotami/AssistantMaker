using CortanaCommand.Core.Cortana;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CortanaCommand.ViewModel
{
    public class MainViewModel: ViewModelBase
    {
        private ObservableCollection<CommandViewModel> _commandList;

        public ObservableCollection<CommandViewModel> CommandList
        {
            get
            {
                return _commandList;
            }

            set
            {
                this.Set(ref _commandList,value);
            }
        }

        private string _commandPrefix;

        public string CommandPrefix
        {
            get
            {
                return _commandPrefix;
            }

            set
            {
                this.Set(ref _commandPrefix,value);
            }
        }

        private string _example;

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

        private string _currentXml;

        public string CurrentXml
        {
            get
            {
                return _currentXml;
            }

            set
            {
                this.Set(ref _currentXml,value);
            }
        }

        private double _listWidth;

        public double ListWidth
        {
            get { return _listWidth; }
            set { this.Set(ref _listWidth,value); }
        }

        private string _passCode;

        public string PassCode
        {
            get
            {
                return _passCode;
            }

            set
            {
                this.Set(ref _passCode,value);
            }
        }

        

        public RelayCommand AddCommandCommand{ get; set; }

        public RelayCommand<CommandViewModel> DeleteCommandCommand { get; set; }

        public RelayCommand UpdateCortanaCommand { get; set; }

        public RelayCommand ChangePassCodeCommand { get; set; }

        private string voiceCommandServiceName = "CortanaCommandService";

        public event Action<string> OnRegisterVoiceCommand;

        public MainViewModel()
        {
            CommandList = new ObservableCollection<CommandViewModel>();

            AddCommandCommand = new RelayCommand(()=>
            {
                var list = CommandList.Where(q => q.Name.StartsWith("Command")).ToList();
                int maxNum = 0;
                foreach (var cmd in list)
                {
                    var numStr = cmd.Name.Replace("Command", "");
                    int result;
                    bool isConvert = int.TryParse(numStr,out result);
                    if (isConvert)
                    {
                        if(maxNum < result)
                        {
                            maxNum = result;
                        }
                    }
                }
                maxNum++;
                var command = new CommandViewModel();
                command.Name = "Command"+maxNum;
                command.AddSuccessStateCommand.Execute(null);
                this.CommandList.Add(command);
            });

            DeleteCommandCommand = new RelayCommand<CommandViewModel>(command =>
            {
               CommandList.Remove(command);
            });

            UpdateCortanaCommand = new RelayCommand(()=>
            {
                if (CommandList.GroupBy(q => q.Name).Where(w => w.Count() > 1).Count() > 0)
                {
                    Messenger.Default.Send<string>("Commandの名前が重複しています","error");
                    return;
                }
                
                CortanaXmlGenerator generator = new CortanaXmlGenerator(CommandPrefix,Example);
                foreach(var command in CommandList)
                {
                    foreach(var state in command.StateList)
                    {
                        if(state is SuccessStateViewModel)
                        {
                            generator.AddCommandService(command.Name+"_"+state.Name,state.Example,
                                state.ListenFor.Replace("\r","").Split('\n'),
                                state.FeedBack,
                                voiceCommandServiceName);
                        }
                    }
                    if (command.StateList.GroupBy(q => q.Name).Where(w => w.Count() > 1).Count() > 0)
                    {
                        Messenger.Default.Send<string>(command.Name+"コマンドのstateの名前が重複しています", "error");
                        return;
                    }
                }

                var xml = generator.GenerateXml();
                CurrentXml = xml;
                //Debug.Write(xml);
                OnRegisterVoiceCommand(xml);
                
            });

            OnRegisterVoiceCommand += e =>
            {

            };

            ChangePassCodeCommand = new RelayCommand(() =>
            {
                this.PassCode = Guid.NewGuid().ToString();
            });

            CommandPrefix = "コルタナ";
            Example = "こんにちはコルタナ";
            ChangePassCodeCommand.Execute(null);

            ListWidth = 340;
        }

        public void UpdateListWidth(double width)
        {
            this.ListWidth = width;
        }

        
    }
}
