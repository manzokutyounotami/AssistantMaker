using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CortanaCommand.Model
{
    public static class NodeIdManager
    {
        public static List<int> NodeIdList = new List<int>();
        public static int GenerateId()
        {
            var index = 0;
            while (NodeIdList.Any(q => q == index))
            {
                index++;
            }

            NodeIdList.Add(index);
            return index;
        }
    }
}
