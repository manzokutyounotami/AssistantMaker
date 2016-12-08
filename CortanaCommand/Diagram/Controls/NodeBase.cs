using CortanaSwitch.Diagram.Common;
using CortanaSwitch.Model;
using CortanaSwitch.Model.Node;
using CortanaSwitch.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace CortanaSwitch.View.Control
{
    public class NodeBase: UserControl
    {
        public int NodeId
        {
            get { return (int)GetValue(NodeIdProperty); }
            set { SetValue(NodeIdProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NodeId.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NodeIdProperty =
            DependencyProperty.Register("NodeId", typeof(int), typeof(NodeBase), new PropertyMetadata(-1));



        public List<InConnectorBase> InConnectorList
        {
            get { return (List<InConnectorBase>)GetValue(InConnectorListProperty); }
            set { SetValue(InConnectorListProperty, value); }
        }

        // Using a DependencyProperty as the backing store for InConnectorList.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InConnectorListProperty =
            DependencyProperty.Register("InConnectorList", typeof(List<InConnectorBase>), typeof(NodeBase), new PropertyMetadata(new List<InConnectorBase>()));



        public List<OutConnectorBase> OutConnectorList
        {
            get { return (List<OutConnectorBase>)GetValue(OutConnectorListProperty); }
            set { SetValue(OutConnectorListProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OutConnectorList.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OutConnectorListProperty =
            DependencyProperty.Register("OutConnectorList", typeof(List<OutConnectorBase>), typeof(NodeBase), new PropertyMetadata(new List<OutConnectorBase>()));


        public event Action<NodeConnection, NodeConnection> NodeLinked;

        public NodeBase()
        {
            this.CanDrag = true;
            this.DragStarting += (s, e) =>
            {
                e.Data.RequestedOperation = DataPackageOperation.Move;
                e.Data.SetData("NodeId", NodeId);
                Point relativePos = e.GetPosition(this);
                e.Data.SetData("ClickPosX",relativePos.X);
                e.Data.SetData("ClickPosY",relativePos.Y);
                e.Data.SetData("DragType", (int)DiagramDragType.NodeMove);
            };

            this.NodeLinked += (s, e) =>
            {

            };
        }

        public virtual void Initialize(int nodeId)
        {
            this.NodeId = nodeId;
        }

        public void AddInConnector(InConnectorBase connector)
        {
            connector.NodeId = this.NodeId;
            connector.ConnectorId = InConnectorList.Count;
            connector.Initialize(connector.ConnectorId, NodeId);
            connector.Linked += ConnectorLinked;
        }

        private void ConnectorLinked(NodeConnection from,NodeConnection to)
        {
            NodeLinked(from,to);
        }

        public void AddOutConnector(OutConnectorBase connector)
        {
            connector.NodeId = this.NodeId;
            connector.ConnectorId = OutConnectorList.Count;
            connector.Initialize(connector.ConnectorId, NodeId);
        }

        public Point GetPosition()
        {
            var left = Canvas.GetLeft(this);
            var top = Canvas.GetTop(this);
            return new Point(left,top);
        }

        public void NodeInConnected(int connectorId,NodeConnection connectedInfo)
        {
            InConnectorList.First(q => q.ConnectorId == connectorId).ConnectionList.Add(connectedInfo);
        }

        public void NodeOutConnected(int connectorId,NodeConnection connectedInfo)
        {
            OutConnectorList.First(q => q.ConnectorId == connectorId).ConnectionList.Add(connectedInfo);
        }
    }
}
