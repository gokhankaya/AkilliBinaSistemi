using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationObjects
{
    public class GraphObject
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string MatrixValue { get; set; }

        public virtual List<GraphNodeDeviceMapping> GraphNodeDeviceMappings { get; set; }
    }
}
