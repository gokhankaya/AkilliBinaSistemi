using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationObjects
{
    public class Operation : IShowableOnListcs
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public DateTime StartTime { get; set; }

        public TimeSpan Duration { get; set; }

        public virtual List<OperationHabitMapping> OperationHabitMappingEntry { get; set; }

        public virtual List<OperationDevice> DevicesOfOperation { get; set; }

        public string GetValue()
        {
            string areas = string.Empty;
            string devices = string.Empty;

            areas = areas.TrimEnd(',');

            foreach (var device in DevicesOfOperation.OrderBy(x => x.Sira))
            {
                devices += $" {device.Sira}-) {device.DeviceBase.ip} {device.DeviceBase.GetType().Name}\n";
            }

            return $"{Name} - {StartTime.ToShortTimeString()} => {StartTime.AddMinutes(Duration.Minutes).ToShortTimeString()}\n{areas}\n{devices}";
        }
    }
}
