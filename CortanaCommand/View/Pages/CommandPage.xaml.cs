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
        string _rightTappedGuid;
        public CommandPage()
        {
            this.InitializeComponent();
            Window.Current.SizeChanged += (s, e) =>
            {
                ResizeContentWidth();
            };
        }

        private void ResizeContentWidth()
        {
            if (App.StateManager.CurrentState == AppState.Mobile)
            {
                listColumn.Width = new GridLength(Window.Current.Bounds.Width);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this._viewModel = e.Parameter as CommandViewModel;
            this.DataContext = _viewModel;
            ResizeContentWidth();
        }

        private void menuItemState_Click(object sender, RoutedEventArgs e)
        {
            var rightTappedItem = _viewModel.StateList.First(q => q.UniqueId == _rightTappedGuid);
            _viewModel.DeleteStateCommand.Execute(rightTappedItem);
        }

        private void Grid_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            var grid = sender as Grid;
            var childCount = VisualTreeHelper.GetChildrenCount(grid);
            for (int i = 0; i < childCount; i++)
            {
                var child = VisualTreeHelper.GetChild(grid, i);
                if (child is TextBlock)
                {
                    if (child.GetValue(TextBlock.NameProperty).ToString() == "text_uniqueId")
                    {
                        _rightTappedGuid = child.GetValue(TextBlock.TextProperty).ToString();
                    }
                }
            }
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
                App.NavigateFrame(frameState, typeof(ScriptStatePage), vm);
            }else if (vm is ProtocolStateViewModel)
            {
                App.NavigateFrame(frameState, typeof(ProtocolStatePage), vm);
            }
        }
    }
}
