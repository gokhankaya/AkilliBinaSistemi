using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedObject
{
    public class AdleMemoryObject
    {
        public string Key { get; set; }

        public AdleAreaBase Area { get; set; }

        public AdleItemBase Item { get; set; }

        public DateTime MemoryMoment { get; set; }

        public string ActionName { get; set; }

        public string ActionValue { get; set; }

        public override string ToString()
        {
            return $"{MemoryMoment.ToShortDateString()} {MemoryMoment.ToLongTimeString()} - {Area.Name} {Item.GetType().Name} {Item.IpV4} ({Item.Name}) - {ActionName}:{ActionValue}";
        }

        public string BasiclyShow { get; set; }
    }
}
