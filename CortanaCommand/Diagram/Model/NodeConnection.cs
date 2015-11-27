using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CortanaSwitch.Model.Node
{
    public class NodeConnection:ObservableObject
    {
        private int _connectedConnectorId;
        
        public int ConnectedConnectorId
        {
            get
            {
                return _connectedConnectorId;
            }

            set
            {
                this.Set(ref _connectedConnectorId,value);
            }
        }

        public int ConnectedNodeId
        {
            get
            {
                return _connectedNodeId;
            }

            set
            {
                this.Set(ref _connectedNodeId,value);
            }
        }

        private int _connectedNodeId;

        public NodeConnection(int connectorId,int nodeId)
        {
            this.ConnectedConnectorId = connectorId;
            this.ConnectedNodeId = nodeId;
        }
    }
}
