using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ToolStripMenuItem=System.Windows.Forms.ToolStripMenuItem;


namespace CortanaCommandServer
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        private NotifyIconWrapper notifyIconWrapper;
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = App.ViewModel;

            if (App.ViewModel.Setting.IsMinimizeStart) {
                this.WindowState = WindowState.Minimized;
                ShowNotifyIcon();
            }

            App.ViewModel.StartServerCommand.Execute(null);
            App.Current.Exit += (s, e) =>
            {
                App.ViewModel.StopServerCommand.Execute(null);
            };
            
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (App.ViewModel.Setting.IsMinimizeOnClosing) {
                e.Cancel = true;
                this.WindowState = WindowState.Minimized;
            }
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Minimized && App.ViewModel.Setting.UseTaskTray) {
                ShowNotifyIcon();
            }
        }

        private void ShowWindow(object sender = null, EventArgs e = null)
        {
            this.Show();
            this.WindowState = WindowState.Normal;
            notifyIconWrapper?.Dispose();
        }

        private void ShowNotifyIcon()
        {
            this.Hide();

            notifyIconWrapper = new NotifyIconWrapper();
            var info = Application.GetResourceStream(new Uri("pack://application:,,,/favicon.ico", UriKind.Absolute));
            notifyIconWrapper.NotifyIcon.Icon = new System.Drawing.Icon(info.Stream);
            notifyIconWrapper.NotifyIcon.DoubleClick += ShowWindow;

            var toolStripMenuItemSetting = new ToolStripMenuItem("設定", null, ShowWindow);
            var toolStripMenuItemExit = new ToolStripMenuItem("終了", null, (s, e) => App.Current.Shutdown());

            notifyIconWrapper.ContextMenuStrip.Items.AddRange(new[]
            {
                    toolStripMenuItemSetting,
                    toolStripMenuItemExit
            });
        }
    }
}
