﻿#pragma checksum "c:\users\garic\documents\visual studio 2015\Projects\CortanaCommand\CortanaCommand\View\Pages\CommandPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "8F2DC719ADDF5C147AF8CD7FE6EFB3B9"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CortanaCommand.View.Pages
{
    partial class CommandPage : 
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
                    this.frameState = (global::Windows.UI.Xaml.Controls.Frame)(target);
                }
                break;
            case 2:
                {
                    this.textBlock = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 3:
                {
                    this.listBoxState = (global::Windows.UI.Xaml.Controls.ListBox)(target);
                    #line 29 "..\..\..\..\View\Pages\CommandPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.ListBox)this.listBoxState).SelectionChanged += this.listBoxState_SelectionChanged;
                    #line default
                }
                break;
            case 4:
                {
                    global::Windows.UI.Xaml.Controls.Grid element4 = (global::Windows.UI.Xaml.Controls.Grid)(target);
                    #line 37 "..\..\..\..\View\Pages\CommandPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Grid)element4).RightTapped += this.Grid_RightTapped;
                    #line default
                }
                break;
            case 5:
                {
                    global::Windows.UI.Xaml.Controls.MenuFlyoutItem element5 = (global::Windows.UI.Xaml.Controls.MenuFlyoutItem)(target);
                    #line 41 "..\..\..\..\View\Pages\CommandPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.MenuFlyoutItem)element5).Click += this.menuItemState_Click;
                    #line default
                }
                break;
            case 6:
                {
                    this.textBox = (global::Windows.UI.Xaml.Controls.TextBox)(target);
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
