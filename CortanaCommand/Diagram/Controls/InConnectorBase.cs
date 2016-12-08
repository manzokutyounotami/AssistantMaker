using CortanaSwitch.Diagram.Common;
using CortanaSwitch.Model;
using CortanaSwitch.Model.Node;
using CortanaSwitch.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace CortanaSwitch.View.Control
{
    public class InConnectorBase:UserControl
    {
        public int NodeId
        {
            get { return (int)GetValue(NodeIdProperty); }
            set { SetValue(NodeIdProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NodeId.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NodeIdProperty =
            DependencyProperty.Register("NodeId", typeof(int), typeof(InConnectorBase), new PropertyMetadata(0));

        public int ConnectorId
        {
            get { return (int)GetValue(ConnectorIdProperty); }
            set { SetValue(ConnectorIdProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ConnectorId.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ConnectorIdProperty =
            DependencyProperty.Register("ConnectorId", typeof(int), typeof(InConnectorBase), new PropertyMetadata(0));



        public List<NodeConnection> ConnectionList
        {
            get { return (List<NodeConnection>)GetValue(ConnectionListProperty); }
            set { SetValue(ConnectionListProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ConnectionListProperty =
            DependencyProperty.Register("ConnectionListProperty", typeof(List<NodeConnection>), typeof(InConnectorBase), new PropertyMetadata(new List<NodeConnection>()));


        public event Action<NodeConnection, NodeConnection> Linked;

        public InConnectorBase()
        {
            Linked += (s, e) =>
            {

            };
            this.AllowDrop = true;
            this.DragEnter += (s, e) =>
            {
                e.AcceptedOperation = DataPackageOperation.Move;
                e.DragUIOverride.Caption = "Link";
            };
            this.Drop += async (s, e) =>
            {
                var deferral = e.GetDeferral();

                var dragType = await e.DataView.GetDataAsync("DragType");

                if ((DiagramDragType)(dragType) == DiagramDragType.NodeConnect)
                {

                    if (e.DataView.Contains("NodeId"))
                    {
                        int fromNodeId = int.Parse((await e.DataView.GetDataAsync("NodeId")).ToString());
                        int fromConnectorId = int.Parse((await e.DataView.GetDataAsync("ConnectorId")).ToString());
                        
                        Linked(new NodeConnection(fromConnectorId,fromNodeId), new NodeConnection(this.ConnectorId,this.NodeId));
                    }
                }

                deferral.Complete();
            };

        }

        public void Initialize(int connectorId,int nodeId)
        {
            this.NodeId = nodeId;
            this.ConnectorId = connectorId;
        }

        public Point GetPosition()
        {
            var left = Canvas.GetLeft(this);
            var top = Canvas.GetTop(this);
            return new Point(left+this.ActualWidth/2, top + this.ActualHeight/2);
        }
    }
}
