using System;

namespace SimulationObjects.Device
{
    [DeviceDefition]
    public class Light : DeviceWithState, IShowableOnListcs
    {
        public override string Type
        {
            get
            {
                return "Light";
            }
        }

        [ActionableMethod]
        public void Open(string ip)
        {
            CloseOpen(ip, true);
        }


        [ActionableMethod]
        public void Close(string ip)
        {
            CloseOpen(ip, false);
        }

        private void CloseOpen(string ip, bool state)
        {
            var light = Devices.find(ip);

            if (light == null)
            {
                light = new Light() { ip = ip, state = false, Name = Guid.NewGuid().ToString() };
                Devices.Add(light);
            }

            ((Light)light).state = state;
        }

        public bool State(string ip)
        {
            var light = Devices.find(ip);

            if (light == null)
            {
                light = new Light() { ip = ip, state = false, Name = Guid.NewGuid().ToString() };
                Devices.Add(light);
            }

            return ((Light)light).state;
        }

        public string GetValue()
        {
            return $"{Type} {Name} {ip} {AreaBase.Name}";
        }

        public override string ToString()
        {
            return GetValue();
        }
    }
}
