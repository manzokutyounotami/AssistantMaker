﻿#pragma checksum "C:\Users\garicchi\Projects\Repository\Windows\CortanaCommand\CortanaCommand\View\Pages\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "DC28BF78F35E7FEDA6E68EB6FFA265E2"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CortanaCommand
{
    partial class MainPage : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                {
                    global::Windows.UI.Xaml.Controls.Page element1 = (global::Windows.UI.Xaml.Controls.Page)(target);
                    #line 8 "..\..\..\..\View\Pages\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Page)element1).Loaded += this.Page_Loaded;
                    #line default
                }
                break;
            case 2:
                {
                    this.VisualStateGroup = (global::Windows.UI.Xaml.VisualStateGroup)(target);
                }
                break;
            case 3:
                {
                    this.MobileState = (global::Windows.UI.Xaml.VisualState)(target);
                }
                break;
            case 4:
                {
                    this.NormalState = (global::Windows.UI.Xaml.VisualState)(target);
                }
                break;
            case 5:
                {
                    this.WideState = (global::Windows.UI.Xaml.VisualState)(target);
                }
                break;
            case 6:
                {
                    this.splitView = (global::Windows.UI.Xaml.Controls.SplitView)(target);
                }
                break;
            case 7:
                {
                    this.appButtonSetting = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                    #line 81 "..\..\..\..\View\Pages\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.AppBarButton)this.appButtonSetting).Click += this.appButtonSetting_Click;
                    #line default
                }
                break;
            case 8:
                {
                    this.appButtonEditor = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                    #line 72 "..\..\..\..\View\Pages\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.AppBarButton)this.appButtonEditor).Click += this.appButtonEditor_Click;
                    #line default
                }
                break;
            case 9:
                {
                    this.appButtonXml = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                    #line 73 "..\..\..\..\View\Pages\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.AppBarButton)this.appButtonXml).Click += this.appButtonXml_Click;
                    #line default
                }
                break;
            case 10:
                {
                    this.appButtonPreview = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                    #line 74 "..\..\..\..\View\Pages\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.AppBarButton)this.appButtonPreview).Click += this.appButtonPreview_Click;
                    #line default
                }
                break;
            case 11:
                {
                    this.appButtonHelp = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                    #line 75 "..\..\..\..\View\Pages\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.AppBarButton)this.appButtonHelp).Click += this.appButtonHelp_Click;
                    #line default
                }
                break;
            case 12:
                {
                    this.frameContent = (global::Windows.UI.Xaml.Controls.Frame)(target);
                }
                break;
            case 13:
                {
                    this.paneButton = (global::Windows.UI.Xaml.Controls.Primitives.ToggleButton)(target);
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

