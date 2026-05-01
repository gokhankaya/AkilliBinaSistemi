using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationObjects
{
    public class AreaBase : IShowableOnListcs
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public virtual List<DeviceBase> DevicesInArea { get; set; }
        public virtual List<Scenario> Scenarios { get; set; }
        public virtual List<OperationDevice> OperationDeviceMaping { get; set; }

        public string GetValue()
        {
            return $"{Name}";
        }
    }
}
