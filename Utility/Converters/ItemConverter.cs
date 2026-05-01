using DomainObjects;
using SharedObject;
using System;
using System.Linq;
using System.Reflection;

namespace Utility.Converters
{
    public class ItemConverter : IAdleConverter<AdleItemBase, Item>
    {
        public Item AdleObjectToDomainObject(AdleItemBase AdleObject, bool addSubs = true)
        {
            if (AdleObject == null)
                throw new ArgumentNullException("AdleObject");

            Item domainItem = new Item();
            AdleItemBase adleItem = AdleObject;

            domainItem.ID = adleItem.ID;
            domainItem.ItemType = adleItem.GetType().Name;
            domainItem.Availablity = adleItem.Availablity;
            domainItem.IpV4 = adleItem.IpV4;
            domainItem.IpV6 = adleItem.IpV6;
            domainItem.Name = adleItem.Name == null ? Guid.NewGuid().ToString() : adleItem.Name;

            if (adleItem.AreaOfItem != null)
                domainItem.AreaOfItemID = adleItem.AreaOfItem.ID;

            return domainItem;
        }

        public AdleItemBase DomainObjectToAdleObject(Item Obj, bool addSubs = true, AdleSCUBase SCU = null)
        {
            if (Obj == null)
                throw new ArgumentOutOfRangeException();

            Item domainObjectItem = Obj;

            AdleItemBase adleItem = null;

            if (domainObjectItem.ItemType == null)
                return null;
            else
            {
                Assembly asm = Assembly.LoadFile($"{Environment.CurrentDirectory}\\ItemModel.dll");
                if (asm == null)
                    throw new Exception(); //TODO: Throw Exception

                var type = asm.GetTypes().Where(x => x.Name == domainObjectItem.ItemType).FirstOrDefault();
                if (type == null)
                    throw new Exception(); //TODO: Throw Exception 

                adleItem = (AdleItemBase)Activator.CreateInstance(type);
            }

            adleItem.Name = domainObjectItem.Name;
            adleItem.ID = domainObjectItem.ID;
            adleItem.Availablity = domainObjectItem.Availablity;
            adleItem.IpV4 = domainObjectItem.IpV4;
            adleItem.IpV6 = domainObjectItem.IpV6;

            if (domainObjectItem.AreaOfItem != null && addSubs)
                adleItem.AreaOfItem = new AreaConverter().DomainObjectToAdleObject(domainObjectItem.AreaOfItem, false);

            adleItem.Manager = SCU;
            return adleItem;
        }
    }
}