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

        string _rightTappedGuid;
        public HomePage()
        {
            this.InitializeComponent();

            this.DataContext = App.ViewModel;
            this._viewModel = App.ViewModel;
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
            ResizeContentWidth();
        }

        private void gridListTemplate_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            var grid = sender as Grid;
            var childCount = VisualTreeHelper.GetChildrenCount(grid);
            for(int i = 0; i < childCount; i++)
            {
                var child = VisualTreeHelper.GetChild(grid,i);
                if(child is TextBlock)
                {
                    if(child.GetValue(TextBlock.NameProperty).ToString() == "text_uniqueId")
                    {
                        _rightTappedGuid = child.GetValue(TextBlock.TextProperty).ToString();
                    }
                }
            }
            
            FlyoutBase.ShowAttachedFlyout(grid);
        }

        private void menuItemDelete_Click(object sender, RoutedEventArgs e)
        {
            var rightTappedItem = _viewModel.CommandList.First(q => q.UniqueId == _rightTappedGuid);
            _viewModel.DeleteCommandCommand.Execute(rightTappedItem);
        }

        private void listBox_command_ItemClick(object sender, ItemClickEventArgs e)
        {
            App.NavigateFrame(frameCommand, typeof(CommandPage), e.ClickedItem);
        }
    }
}
