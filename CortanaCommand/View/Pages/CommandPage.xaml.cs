using CortanaCommand.ViewModel;
using CortanaCommandCore.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class CommandPage : Page
    {
        CommandViewModel _viewModel;
        public CommandPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this._viewModel = e.Parameter as CommandViewModel;
            this.DataContext = _viewModel;
        }

        private void menuItemState_Click(object sender, RoutedEventArgs e)
        {
            if(listBoxState.SelectedIndex != -1)
            {
                _viewModel.DeleteStateCommand.Execute(listBoxState.SelectedItem);
            }
        }

        private void Grid_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout(sender as Grid);
        }

        private void listBoxState_ItemClick(object sender, ItemClickEventArgs e)
        {
            var vm = e.ClickedItem;
            if (vm is SuccessStateViewModel)
            {
                App.NavigateFrame(frameState, typeof(SuccessStatePage), vm);
            }else if (vm is ScriptStateViewModel)
            {
                App.NavigateFrame(frameState, typeof(ScriptStateViewModel), vm);
            }else if (vm is ProtocolStateViewModel)
            {
                App.NavigateFrame(frameState, typeof(ProtocolStateViewModel), vm);
            }
        }
    }
}
