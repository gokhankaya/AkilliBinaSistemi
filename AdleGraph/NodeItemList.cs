using AdleGraph.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdleGraph
{
    public class NodeItemList
    {
        public INode Node { get; set; }

        public List<INode> ConnectedNodes { get; set; }
    }
}
