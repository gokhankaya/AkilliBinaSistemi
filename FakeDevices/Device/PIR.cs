using System;

namespace SimulationObjects.Device
{
    [DeviceDefition]
    public class PIR : DeviceWithState, IShowableOnListcs
    {
        public override string Type
        {
            get
            {
                return "PIR";
            }
        }

        [ActionableMethod]
        public void Activate(string ip)
        {
            CloseOpen(ip, true);
        }

        [ActionableMethod]
        public void DeActivate(string ip)
        {
            CloseOpen(ip, false);
        }

        private void CloseOpen(string ip, bool state)
        {
            var pir = Devices.find(ip);

            if (pir == null)
            {
                pir = new PIR() { ip = ip, state = false, Name = Guid.NewGuid().ToString() };
                Devices.Add(pir);
            }

            ((PIR)pir).state = state;
        }

        public bool State(string ip)
        {
            var pir = Devices.find(ip);

            if (pir == null)
            {
                pir = new PIR() { ip = ip, state = false, Name = Guid.NewGuid().ToString() };
                Devices.Add(pir);
            }

            return ((PIR)pir).state;
        }

        public string GetValue()
        {
            return $"{Type} {Name} {ip} {AreaBase?.Name}";
        }

        public override string ToString()
        {
            return GetValue();
        }
    }
}
