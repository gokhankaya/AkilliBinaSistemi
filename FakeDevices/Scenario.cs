using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimulationObjects
{
    public class Scenario
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public virtual List<Actor> ActorsInScenario { get; set; }

        public virtual List<AreaBase> AreasInScenario { get; set; }

    }
}
