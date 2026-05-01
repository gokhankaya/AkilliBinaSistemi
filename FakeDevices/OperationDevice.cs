using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationObjects
{
    public class OperationDevice
    {
        public int ID { get; set; }

        public int Sira { get; set; }

        public string ActionName { get; set; }

        public int OperationID { get; set; }

        public int DeviceBaseID { get; set; }

        public int? AreaID { get; set; }

        [ForeignKey("DeviceBaseID")]
        public virtual DeviceBase DeviceBase { get; set; }

        [ForeignKey("OperationID")]
        public virtual Operation Operation { get; set; }

        [ForeignKey("AreaID")]
        public virtual AreaBase AreaOfOperation { get; set; }
    }
}
