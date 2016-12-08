using CortanaSwitch.Model;
using CortanaSwitch.Model.Node;
using CortanaSwitch.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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
    public sealed partial class DiagramCanvas : UserControl
    {
        public List<NodeBase> NodeList
        {
            get { return (List<NodeBase>)GetValue(NodeListProperty); }
            set { SetValue(NodeListProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NodeList.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NodeListProperty =
            DependencyProperty.Register("NodeList", typeof(List<NodeBase>), typeof(DiagramCanvas), new PropertyMetadata(new List<NodeBase>()));

        public DiagramCanvas()
        {
            this.InitializeComponent();

            AllowDrop = true;

            this.Drop += async (s, e) =>
            {
                var deferral = e.GetDeferral();
                e.AcceptedOperation = DataPackageOperation.Move;
                var dragType = await e.DataView.GetDataAsync("DragType");

                if ((DiagramDragType)(dragType) == DiagramDragType.NodeMove)
                {
                    var pos = e.GetPosition(this.canvasDiagram);
                    var data = await e.DataView.GetDataAsync("NodeId");
                    var clickPosX = double.Parse((await e.DataView.GetDataAsync("ClickPosX")).ToString());
                    var clickPosY = double.Parse((await e.DataView.GetDataAsync("ClickPosY")).ToString());
                    var nodeId = int.Parse(data.ToString());
                    foreach (var elem in this.canvasDiagram.Children)
                    {
                        if (elem is NodeBase)
                        {
                            var node = elem as NodeBase;
                            if (node.NodeId == nodeId)
                            {
                                Canvas.SetLeft(node, pos.X - clickPosX);
                                Canvas.SetTop(node, pos.Y - clickPosY);
                            }
                        }
                    }
                }

                deferral.Complete();
            };

            this.DragOver += (s, e) =>
            {
                e.AcceptedOperation = DataPackageOperation.Move;
                e.DragUIOverride.IsContentVisible = false;
            };
        }

        public void AddNode(NodeBase node)
        {
            node.NodeLinked += NodeLinked;
            node.Initialize(NodeIdManager.GenerateId());
            this.canvasDiagram.Children.Add(node);
        }

        public void RemoveNode(NodeBase node)
        {
            node.NodeLinked -= NodeLinked;
            this.canvasDiagram.Children.Remove(node);
        }

        private void NodeLinked(NodeConnection from, NodeConnection to)
        {
            Debug.WriteLine("Linked {0} -> {1}", from.ConnectedNodeId, to.ConnectedNodeId);
            NodeBase nodeFrom = null;
            NodeBase nodeTo = null;

            foreach (var child in this.canvasDiagram.Children)
            {
                if (child is NodeBase)
                {
                    var node = child as NodeBase;
                    if (node.NodeId == from.ConnectedNodeId)
                    {
                        nodeFrom = node;
                    }
                    if (node.NodeId == to.ConnectedNodeId)
                    {
                        nodeTo = node;
                    }
                }
            }

            var fromPoint = nodeFrom.GetPosition();
            var toPoint = nodeTo.GetPosition();
            Point distance = new Point(toPoint.X - fromPoint.X, toPoint.Y - fromPoint.Y);

            BezierSegment segment = new BezierSegment();
            segment.Point1 = new Point(fromPoint.X + distance.X / 3, fromPoint.Y - 100);
            segment.Point2 = new Point(fromPoint.X + distance.X * 2 / 3, toPoint.Y + 100);
            segment.Point3 = nodeTo.GetPosition();

            PathFigure figure = new PathFigure();
            figure.StartPoint = nodeFrom.GetPosition();
            figure.Segments.Add(segment);

            PathGeometry geo = new PathGeometry();
            geo.Figures.Add(figure);

            Windows.UI.Xaml.Shapes.Path path = new Windows.UI.Xaml.Shapes.Path();
            path.StrokeThickness = 2;
            path.Stroke = new SolidColorBrush(Colors.Black);

            path.Data = geo;

            this.canvasDiagram.Children.Add(path);
        }
    }
}
