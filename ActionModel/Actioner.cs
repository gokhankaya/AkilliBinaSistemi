using SharedObject;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActionModel
{
    public class Actioner : IActioner, IBase
    {
        public Actioner(AdleSCUBase manager)
        {
            Manager = manager;
        }

        private bool _listen = false;
        private bool _listedOnGoing = false;

        private static List<string> items = new List<string>();


        public void Stop()
        {
            _listen = false;
        }

        public void StartActioner()
        {
            var areas = Manager.Areas;

            foreach (var area in areas)
            {
                CreateProcess(area);
            }
        }


        public AdleSCUBase Manager { get; set; }

        public string Name { get; set; }

        private void CreateProcess(AdleAreaBase area)
        {
            if (area.AreaHasItems)
            {
                if (_listedOnGoing)
                {
                    return;
                }

                _listen = true;

                foreach (var item in area.Items)
                {
                    var task = Task.Factory.StartNew(async () =>
                    {
                        items.Add(item.IpV4);
                        while (_listen)
                        {
                            var methods = item.GetType().GetMethods().Where(x => x.GetCustomAttributes(typeof(CheckMethodAttribute), false).Length > 0).ToList();

                            foreach (var method in methods)
                            {
                                var result = method.Invoke(item, null);
                                AdleMemoryObject memory = new AdleMemoryObject()
                                {
                                    ActionName = method.Name,
                                    ActionValue = result.ToString(),
                                    Area = area,
                                    Item = item,
                                    MemoryMoment = DateTime.Now
                                };

                                Manager.MemoryManager.AddMemory(memory);
                                Debug.WriteLine($"{item} {item.IpV4} {method.Name} için {result} değeri alındı.");
                                await Task.Delay(1000);
                            }


                            await Task.Delay(5000);
                        }

                        if (!_listen)
                        {
                            Console.WriteLine($"{item} dinleme durdu.");
                        }
                    });
                }
            }


            if (area.AreaHasSubAreas)
            {
                foreach (var subArea in area.SubAreas)
                {
                    CreateProcess(subArea);
                }
            }

        }
    }
}
