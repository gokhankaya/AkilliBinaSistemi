using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationObjects
{
    public class Actor
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public virtual List<Habit> HabitsOfActor { get; set; }

        public virtual List<Scenario> Scenarios { get; set; }

        [NotMapped]
        public DateTime CurrentWorkEndTime { get; set; }
    }
}
