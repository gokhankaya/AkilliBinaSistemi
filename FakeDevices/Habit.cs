using SimulationObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationObjects
{
    public class Habit
    {
        public int HabitID { get; set; }
        public string Name { get; set; }

        public virtual List<Actor> Actors { get; set; }
        public virtual List<OperationHabitMapping> OperationHabitMappingEntry { get; set; }
    }
}
