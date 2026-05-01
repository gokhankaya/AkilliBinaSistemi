using DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeDataProvider
{
    public class Provider
    {
        public Provider()
        {
            Area = new List<Area>();
            AreaType = new List<AreaType>();
            Item = new List<Item>();
            Memory = new List<Memory>();
        }

        public List<T> Set<T>() where T : class
        {
            var info = this.GetType().GetField(typeof(T).Name, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

            if (info == null)
            {
                return null;
            }

            return (List<T>)info.GetValue(this);

        }

        private List<Area> Area;

        private List<AreaType> AreaType;

        private List<Item> Item;

        private List<Memory> Memory;
    }
}
