using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.Background;
using Windows.ApplicationModel.VoiceCommands;
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

            var triggerDetails = taskInstance.TriggerDetails as AppServiceTriggerDetails;
            if (triggerDetails != null && triggerDetails.Name == "CortanaCommandService")
            {

                voiceServiceConnection = VoiceCommandServiceConnection.FromAppServiceTriggerDetails(triggerDetails);
                var voiceCommand = await voiceServiceConnection.GetVoiceCommandAsync();
                Debug.WriteLine(voiceCommand.CommandName);

                var message = new VoiceCommandUserMessage();
                message.SpokenMessage = "ほげ";
                message.DisplayMessage = "ほげ";
                var response = VoiceCommandResponse.CreateResponse(message);
                await voiceServiceConnection.ReportSuccessAsync(response);
            }
            
            var toast = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText01);
            ToastNotificationManager.CreateToastNotifier().Show(new ToastNotification(toast));

            this.serviceDeferral.Complete();
        }
    }
}
