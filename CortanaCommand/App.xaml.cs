using CortanaCommand.ViewModel;
using CortanaCommandCore.Model;
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
    /// <summary>
    /// 既定の Application クラスを補完するアプリケーション固有の動作を提供します。
    /// </summary>
    sealed partial class App : Application
    {
        public static MainViewModel ViewModel;

        public static Frame RootFrame
        {
            get { return Window.Current.Content as Frame; }
            set { Window.Current.Content = value; }
        }

        public static AppStateManager StateManager { get; set; }
        public static event Action<AppState,AppState> OnChangeAppState;

        public static void NavigateFrame(Frame frameIfNoMobileMode, Type pageType, object param)
        {
            switch (StateManager.CurrentState)
            {
                case AppState.Mobile:
                    RootFrame.Navigate(pageType, param);
                    break;
                case AppState.Normal:
                case AppState.Wide:
                    frameIfNoMobileMode.Navigate(pageType, param);
                    break;
            }
        }

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
            StateManager = new AppStateManager();
            StateManager.StateList.Add(AppState.Mobile, 0);
            StateManager.StateList.Add(AppState.Normal, 800);
            StateManager.StateList.Add(AppState.Wide, 1600);
            
            ViewModel = new MainViewModel();
            var vm = DataLoadAsync().Result;
            if (vm != null)
            {
                ViewModel = vm;
            }
            ViewModel.OnRegisterVoiceCommand += async (xml) =>
            {
                try
                {
                    var folder = ApplicationData.Current.LocalFolder;
                    var file = await folder.CreateFileAsync("VoiceCommandFile", CreationCollisionOption.ReplaceExisting);
                    await FileIO.WriteTextAsync(file, xml);
                    await VoiceCommandDefinitionManager.InstallCommandDefinitionsFromStorageFileAsync(file);
                    await DataSaveAsync();
                    var dialog = new MessageDialog("Cortanaの更新が完了しました");
                    await dialog.ShowAsync();
                }
                catch (Exception e)
                {
                    var dialog = new MessageDialog(e.Message, "Cortanaの更新に失敗しました");
                    await dialog.ShowAsync();
                }
            };
            OnChangeAppState += (s,s2) => { };
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
                ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(340, 400));
                Window.Current.SizeChanged += (s, ex) =>
                {
                    OnWindowSizeChanged(ex.Size);
                };
                SystemNavigationManager.GetForCurrentView().BackRequested += (s, ex) =>
                {
                    if (RootFrame.CanGoBack)
                    {
                        ex.Handled = true;
                        RootFrame.GoBack();
                    }
                };

                rootFrame.Navigate(typeof(MainPage), e.Arguments);

                OnWindowSizeChanged(new Size(Window.Current.Bounds.Width,Window.Current.Bounds.Height));
                
            }
            // 現在のウィンドウがアクティブであることを確認します
            Window.Current.Activate();


        }

        private void OnWindowSizeChanged(Size newSize)
        {
            var prevState = App.StateManager.CurrentState;
            bool isChange = StateManager.TryChangeState(newSize.Width);
            if (isChange)
            {
                switch (StateManager.CurrentState)
                {
                    case AppState.Mobile:
                        SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
                        ViewModel.UpdateListWidth(Window.Current.Bounds.Width);
                        break;
                    case AppState.Normal:
                    case AppState.Wide:
                        SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
                        ViewModel.UpdateListWidth(340);
                        break;
                }
                OnChangeAppState(StateManager.CurrentState,prevState);
            }
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
            await DataSaveAsync();
            deferral.Complete();
        }

        private async void OnResuming(object sender, object e)
        {
            var vm = await DataLoadAsync();
            if(vm != null)
            {
                App.ViewModel = vm;
            }
        }

        private async Task DataSaveAsync()
        {
            var json = JsonConvert.SerializeObject(App.ViewModel, new JsonSerializerSettings
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
