using CortanaCommand.View;
using CortanaCommand.View.Pages;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace CortanaCommand
{
    public sealed partial class MainPage : Page
    {
        
        public MainPage()
        {
            this.InitializeComponent();

            
            //アプリのライフサイクルをフック
            App.Current.Resuming += Current_Resuming;
            App.Current.Suspending += Current_Suspending;
            App.OnChangeAppState += (state,prev) =>
            {
                switch (state)
                {
                    case AppState.Mobile:
                        VisualStateManager.GoToState(this, "MobileState", false);

                        App.NavigateFrame(frameContent, typeof(MainPage), null);

                        break;
                    case AppState.Normal:
                        VisualStateManager.GoToState(this, "NormalState", false);

                        App.RootFrame.Navigate(typeof(MainPage));
                        
                        break;
                    case AppState.Wide:
                        VisualStateManager.GoToState(this, "WideState", false);
                        
                        break;
                }
            };
            Messenger.Default.Register<string>("","error",async (message)=>
            {
                var dialog = new MessageDialog(message,"error");
                await dialog.ShowAsync();
            });
            Messenger.Default.Register<bool>("","updating",(bol)=> 
            {
                
            });

            this.DataContext = App.ViewModel;
            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            switch (App.StateManager.CurrentState)
            {
                case AppState.Mobile:
                    
                    VisualStateManager.GoToState(this, "MobileState", false);

                    break;
                case AppState.Normal:
                    VisualStateManager.GoToState(this, "NormalState", false);

                    break;
                case AppState.Wide:
                    VisualStateManager.GoToState(this, "WideState", false);

                    break;
            }
        }

        //ページが読み込まれた時
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //HomePageにNavigateする
            
            frameContent.Navigate( typeof(HomePage), null);
        }

        //検索ボックスで検索しようとしたとき
        private void suggestBoxSearch_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            var text = args.QueryText;
        }

        //SplitViewのPaneに作ったナビゲーションボタンをおした時、各Pageに移動
        private void appButtonSetting_Click(object sender, RoutedEventArgs e)
        {
            frameContent.Navigate( typeof(SettingPage), null);
            if (App.StateManager.CurrentState != AppState.Wide)
            {
                splitView.IsPaneOpen = false;
            }
        }

        private void appButtonEditor_Click(object sender, RoutedEventArgs e)
        {
            frameContent.Navigate( typeof(HomePage), null);
            if (App.StateManager.CurrentState != AppState.Wide)
            {
                splitView.IsPaneOpen = false;
            }
        }

        private void appButtonXml_Click(object sender, RoutedEventArgs e)
        {
            frameContent.Navigate( typeof(CurrentXmlPage), null);
            if (App.StateManager.CurrentState != AppState.Wide)
            {
                splitView.IsPaneOpen = false;
            }
        }

        //アプリが一時停止しようとしたとき
        private void Current_Suspending(object sender, Windows.ApplicationModel.SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //ここでアプリのデータや状態を保存するdeferral.Complete()が呼ばれるまではawaitしても待ってくれる
            //例 ApplicationData.Current.LocalSettings.Values["MyData"] = 1;

            deferral.Complete();
        }

        //アプリが再開しようとしたとき
        private void Current_Resuming(object sender, object e)
        {
            //ここでアプリのデータや状態を復元する
            /*例
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("MyData"))
            {
                var data = ApplicationData.Current.LocalSettings.Values["MyData"];
            }
            */
        }

        private void appButtonPreview_Click(object sender, RoutedEventArgs e)
        {
            frameContent.Navigate(typeof(PreviewPage), null);
            if (App.StateManager.CurrentState != AppState.Wide)
            {
                splitView.IsPaneOpen = false;
            }
        }

        private async void appButtonHelp_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("https://github.com/garicchi/CortanaCommand/wiki"));
        }

        private void appButtonProfile_Click(object sender, RoutedEventArgs e)
        {
            frameContent.Navigate(typeof(ProfilePage), null);
            if (App.StateManager.CurrentState != AppState.Wide)
            {
                splitView.IsPaneOpen = false;
            }
        }
    }


}
