using CortanaCommand.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.VoiceCommands;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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
    /// <summary>
    /// 既定の Application クラスを補完するアプリケーション固有の動作を提供します。
    /// </summary>
    sealed partial class App : Application
    {
        public static MainViewModel ViewModel;
        private string viewModelSaveDataStr = "ViewModelSaveData";
        /// <summary>
        /// 単一アプリケーション オブジェクトを初期化します。これは、実行される作成したコードの
        ///最初の行であるため、main() または WinMain() と論理的に等価です。
        /// </summary>
        public App()
        {
            Microsoft.ApplicationInsights.WindowsAppInitializer.InitializeAsync(
                Microsoft.ApplicationInsights.WindowsCollectors.Metadata |
                Microsoft.ApplicationInsights.WindowsCollectors.Session);
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            this.Resuming += OnResuming;
            
            ViewModel = new MainViewModel();
            ResumeAsync().Wait();
            ViewModel.OnRegisterVoiceCommand += async(xml) =>
            {
                try {
                    var folder = ApplicationData.Current.LocalFolder;
                    var file = await folder.CreateFileAsync("VoiceCommandFile", CreationCollisionOption.ReplaceExisting);
                    await FileIO.WriteTextAsync(file, xml);
                    await VoiceCommandDefinitionManager.InstallCommandDefinitionsFromStorageFileAsync(file);
                    var dialog = new MessageDialog("Cortanaの更新が完了しました");
                    await dialog.ShowAsync();
                }
                catch (Exception e)
                {
                    var dialog = new MessageDialog(e.Message,"Cortanaの更新に失敗しました");
                    await dialog.ShowAsync();
                }
            };
        }

        

        /// <summary>
        /// アプリケーションがエンド ユーザーによって正常に起動されたときに呼び出されます。他のエントリ ポイントは、
        /// アプリケーションが特定のファイルを開くために起動されたときなどに使用されます。
        /// </summary>
        /// <param name="e">起動の要求とプロセスの詳細を表示します。</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {

#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            Frame rootFrame = Window.Current.Content as Frame;

            // ウィンドウに既にコンテンツが表示されている場合は、アプリケーションの初期化を繰り返さずに、
            // ウィンドウがアクティブであることだけを確認してください
            if (rootFrame == null)
            {
                // ナビゲーション コンテキストとして動作するフレームを作成し、最初のページに移動します
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: 以前中断したアプリケーションから状態を読み込みます
                }

                // フレームを現在のウィンドウに配置します
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // ナビゲーション スタックが復元されない場合は、最初のページに移動します。
                // このとき、必要な情報をナビゲーション パラメーターとして渡して、新しいページを
                //構成します
                //var appView = ApplicationView.GetForCurrentView();
                //appView.TitleBar.BackgroundColor = (Resources["ApplicationThemeBrush"] as SolidColorBrush).Color;

                rootFrame.Navigate(typeof(MainPage), e.Arguments);
            }
            // 現在のウィンドウがアクティブであることを確認します
            Window.Current.Activate();


        }

        /// <summary>
        /// 特定のページへの移動が失敗したときに呼び出されます
        /// </summary>
        /// <param name="sender">移動に失敗したフレーム</param>
        /// <param name="e">ナビゲーション エラーの詳細</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// アプリケーションの実行が中断されたときに呼び出されます。
        /// アプリケーションが終了されるか、メモリの内容がそのままで再開されるかに
        /// かかわらず、アプリケーションの状態が保存されます。
        /// </summary>
        /// <param name="sender">中断要求の送信元。</param>
        /// <param name="e">中断要求の詳細。</param>
        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: アプリケーションの状態を保存してバックグラウンドの動作があれば停止します
            var json = JsonConvert.SerializeObject(App.ViewModel, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });
            var folder = ApplicationData.Current.LocalFolder;
            var file = await folder.CreateFileAsync(viewModelSaveDataStr, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file,json);
            deferral.Complete();
        }

        private async void OnResuming(object sender, object e)
        {
            await ResumeAsync();
        }

        private async Task ResumeAsync()
        {
            var folder = ApplicationData.Current.LocalFolder;
            var files = await folder.GetFilesAsync();
            if (files.Any(q => q.Name == viewModelSaveDataStr))
            {
                //try
                //{
                var file = files.First(q => q.Name == viewModelSaveDataStr);
                var json = await FileIO.ReadTextAsync(file);
                var viewModel = JsonConvert.DeserializeObject<MainViewModel>(json, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });
                App.ViewModel = viewModel;
                /*}
                catch (Exception)
                {

                }
                */
            }
        }
    }
}
