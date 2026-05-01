using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationObjects.Device
{
    public class Devices
    {
        private static List<DeviceBase> _devices;

        internal static List<DeviceBase> DevicesList
        {
            get
            {
                if (_devices == null)
                {
                    _devices = new List<DeviceBase>();
                }
                return _devices;
            }

            private set
            {
                _devices = value;
            }
        }

        public static void Reset()
        {
            _devices = new List<DeviceBase>();
        }

        internal static void Add(DeviceBase device)
        {
            if (DevicesList == null)
            {
                Reset();
            }

            if (DevicesList.Exists(x => x.ip == device.ip))
            {
                return;
            }

            DevicesList.Add(device);
        }

        public static DeviceBase find(string ip)
        {
            if (DevicesList == null)
            {
                Reset();
            }

            var foundDevice = DevicesList.Find(x => x.ip == ip);

            return foundDevice;
        }

        public static void Register(DeviceBase device)
        {
            Add(device);
        }

    }
}
