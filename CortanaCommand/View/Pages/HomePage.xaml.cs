using CortanaCommand.View.Pages;
using CortanaCommand.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// 空白ページのアイテム テンプレートについては、http://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

namespace CortanaCommand
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class HomePage : Page
    {
        MainViewModel _viewModel;
        public HomePage()
        {
            this.InitializeComponent();

            this.DataContext = App.ViewModel;
            this._viewModel = App.ViewModel;
            
        }


        private void gridListTemplate_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout(sender as Grid);
        }

        private void menuItemDelete_Click(object sender, RoutedEventArgs e)
        {
            if (listBox_command.SelectedIndex != -1)
            {
                _viewModel.DeleteCommandCommand.Execute(listBox_command.SelectedItem);
            }
        }

        private void listBox_command_ItemClick(object sender, ItemClickEventArgs e)
        {
            App.NavigateFrame(frameCommand, typeof(CommandPage), e.ClickedItem);
        }
    }
}
