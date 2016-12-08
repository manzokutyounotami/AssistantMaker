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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace CortanaSwitch.View.Control
{
    public sealed partial class Node : NodeBase
    {
        public Node()
        {
            this.InitializeComponent();

            
        }

        public override void Initialize(int nodeId)
        {
            base.Initialize(nodeId);
            this.Width = 300;
            this.Height = 150;
            this.AddInConnector(this.inConnector1);
            this.AddOutConnector(this.outConnector1);
        }
    }
}
