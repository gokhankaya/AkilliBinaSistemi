using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;

namespace SimulationObjects
{
    public class DeviceBase
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public virtual string Type { get; }
        public string ip { get; set; }
        public int AreaID { get; set; }
        [ForeignKey("AreaID")]
        public virtual AreaBase AreaBase { get; set; }
        public virtual List<OperationDevice> DevicesOfOperation { get; set; }

        public virtual List<GraphNodeDeviceMapping> GraphNodeDeviceMappings { get; set; }


        public List<MethodInfo> GetActionableMethodsOFDevice()
        {
            var list = this.GetType().GetMethods().Where(x => x.GetCustomAttributes(typeof(ActionableMethodAttribute), true).Length > 0).ToList();

            return list;
        }
    }
}
