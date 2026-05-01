using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationObjects
{
    public class OperationHabitMapping
    {
        public int ID { get; set; }

        public int MaxDuration { get; set; }
        public int MinDuration { get; set; }

        public int? OperationID { get; set; }

        public int? HabitID { get; set; }

        [ForeignKey("HabitID")]
        public virtual Habit Habit { get; set; }

        [ForeignKey("OperationID")]
        public virtual Operation Operation { get; set; }

    }
}
