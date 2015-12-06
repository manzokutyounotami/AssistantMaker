using CortanaCommand.ViewModel;
using CortanaCommandCore.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.Background;
using Windows.ApplicationModel.VoiceCommands;
using Windows.Networking;
using Windows.Networking.Connectivity;
using Windows.Networking.Sockets;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.System;
using Windows.UI.Notifications;

namespace CortanaCommandService
{
    public sealed class CommandService : IBackgroundTask
    {
        private BackgroundTaskDeferral serviceDeferral;
        VoiceCommandServiceConnection voiceServiceConnection;

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            this.serviceDeferral = taskInstance.GetDeferral();
            try {
                var triggerDetails = taskInstance.TriggerDetails as AppServiceTriggerDetails;
                if (triggerDetails != null && triggerDetails.Name == "CortanaCommandService")
                {

                    voiceServiceConnection = VoiceCommandServiceConnection.FromAppServiceTriggerDetails(triggerDetails);
                    var voiceCommand = await voiceServiceConnection.GetVoiceCommandAsync();
                    Debug.WriteLine(voiceCommand.CommandName);

                    MainViewModel viewModel = new MainViewModel();
                    var vm = await DataLoadAsync();
                    if (vm != null)
                    {
                        viewModel = vm;
                    }

                    var cols = voiceCommand.CommandName.Split('_');
                    var commandName = cols[0];
                    var stateName = cols[1];

                    var commandViewModel = viewModel.CommandList.First(q => q.Name == commandName);
                    
                    commandViewModel.CurrentStateNum++;
                    var stateViewModel = commandViewModel.StateList.ElementAt(commandViewModel.CurrentStateNum - 1);
                    if (commandViewModel.CurrentStateNum>=commandViewModel.StateList.Count)
                    {
                        commandViewModel.CurrentStateNum = 0;
                    }
                    if(stateViewModel is CortanaCommand.ViewModel.SuccessStateViewModel)
                    {
                        SuccessStateViewModel state = stateViewModel as SuccessStateViewModel;
                        if (!string.IsNullOrEmpty(state.Script))
                        {
                            try {
                                ConnectionData connectionData = new ConnectionData();
                                connectionData.AcceptPass = viewModel.PassCode;
                                connectionData.Script = state.Script.Replace("\n", ";").Replace("\r", "").Replace("\t", "");
                                string json = JsonConvert.SerializeObject(connectionData);
                                var byteData = Encoding.UTF8.GetBytes(json);
                                StreamSocket socket = new StreamSocket();

                                await socket.ConnectAsync(new HostName("127.0.0.1"), SettingManager.ServerPort);
                                var writer = new DataWriter(socket.OutputStream);
                                writer.WriteBytes(byteData);
                                await writer.StoreAsync();
                                await writer.FlushAsync();
                                writer.Dispose();
                                socket.Dispose();
                                
                            }
                            catch (Exception)
                            {
                                var errorMsg = new VoiceCommandUserMessage();
                                string msg = "スクリプトの実行を試みましたがサーバーが起動してませんでした";
                                errorMsg.SpokenMessage = msg;
                                errorMsg.DisplayMessage = msg;
                                var errorResponse = VoiceCommandResponse.CreateResponse(errorMsg);
                                await voiceServiceConnection.ReportFailureAsync(errorResponse);
                                return;
                            }
                        }

                        if (!string.IsNullOrEmpty(state.Protocol))
                        {
                            var result = await Launcher.LaunchUriAsync(new Uri(state.Protocol));
                            if (!result)
                            {
                                var errorMsg = new VoiceCommandUserMessage();
                                string msg = "プロトコル起動を試みましたがURLが間違っているようです";
                                errorMsg.SpokenMessage = msg;
                                errorMsg.DisplayMessage = msg;
                                var errorResponse = VoiceCommandResponse.CreateResponse(errorMsg);
                                await voiceServiceConnection.ReportFailureAsync(errorResponse);
                                return;
                            }
                        }

                        if (string.IsNullOrEmpty(state.Utterance))
                        {
                            state.Utterance = "";
                        }
                        var message = new VoiceCommandUserMessage();
                        message.SpokenMessage = state.Utterance;
                        message.DisplayMessage = state.Utterance;
                        var response = VoiceCommandResponse.CreateResponse(message);
                        await voiceServiceConnection.ReportSuccessAsync(response);
                        
                    }

                    await DataSaveAsync(viewModel);
                }

            }catch(Exception e)
            {
                var message = new VoiceCommandUserMessage();
                message.SpokenMessage = "何かしらのエラーが起きました";
                message.DisplayMessage = e.Message;
                var response = VoiceCommandResponse.CreateResponse(message);
                await voiceServiceConnection.ReportSuccessAsync(response);

                var toast = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText01);
                ToastNotificationManager.CreateToastNotifier().Show(new ToastNotification(toast));
            }
            

            this.serviceDeferral.Complete();
        }

        private async Task DataSaveAsync(MainViewModel viewModel)
        {
            var json = JsonConvert.SerializeObject(viewModel, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });
            var folder = ApplicationData.Current.LocalFolder;
            var file = await folder.CreateFileAsync(SettingManager.ViewModelDataFileName, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, json);
        }

        private async Task<MainViewModel> DataLoadAsync()
        {
            var folder = ApplicationData.Current.LocalFolder;
            var files = await folder.GetFilesAsync();
            if (files.Any(q => q.Name == SettingManager.ViewModelDataFileName))
            {
                try
                {
                    var file = files.First(q => q.Name == SettingManager.ViewModelDataFileName);
                    var json = await FileIO.ReadTextAsync(file);
                    var viewModel = JsonConvert.DeserializeObject<MainViewModel>(json, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Auto
                    });
                    return viewModel;
                }
                catch (Exception)
                {

                }
            }
            return null;
        }
    }
}
