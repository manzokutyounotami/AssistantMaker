using CortanaCommand.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// 空白ページのアイテム テンプレートについては、http://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

namespace CortanaCommand.View.Pages
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class ProfilePage : Page
    {
        private MainViewModel _viewModel;
        public ProfilePage()
        {
            this.InitializeComponent();
            this.DataContext = App.ViewModel;
            this._viewModel = App.ViewModel;
        }

        private async void buttonStore_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("https://github.com/garicchi/CortanaCommand/tree/master/Store"));
        }

        private async void buttonExport_Click(object sender, RoutedEventArgs e)
        {
            var picker = new FileSavePicker();
            picker.FileTypeChoices.Add("Cortana Profile", new List<string> { ".cortana" });
            var file = await picker.PickSaveFileAsync();
            if(file != null)
            {
                await App.SaveProfileAsync(file);
                var dialog = new MessageDialog("プロファイル出力完了しました");
                await dialog.ShowAsync();
            }
            else
            {
                var dialog = new MessageDialog("出力ファイルを指定してください");
                await dialog.ShowAsync();
            }
        }

        private async void buttonImport_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new MessageDialog("プロファイルを新しくインポートすると今のコマンドリストが消えます。よろしいですか？");
            dialog.Commands.Add(new UICommand("はい",async(arg)=>
            {
                var picker = new FileOpenPicker();
                picker.FileTypeFilter.Add(".cortana");
                var file = await picker.PickSingleFileAsync();
                if (file != null)
                {
                    await App.LoadProfileAsync(file);
                    var dialogConfirm = new MessageDialog("プロファイルをインポートしました");
                    await dialogConfirm.ShowAsync();
                }
            }));
            dialog.Commands.Add(new UICommand("いいえ", (arg) =>
            {
            }));
            await dialog.ShowAsync();
        }
    }
}
