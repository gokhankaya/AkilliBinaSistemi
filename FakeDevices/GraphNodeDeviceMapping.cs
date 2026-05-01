using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationObjects
{
    public class GraphNodeDeviceMapping
    {
        public int ID { get; set; }

        public string NodeName { get; set; }

        public int DeviceID { get; set; }
        public int GraphID { get; set; }


        [ForeignKey("GraphID")]
        public virtual GraphObject Graph { get; set; }

        [ForeignKey("DeviceID")]
        public virtual DeviceBase Device { get; set; }
    }
}
