using ActionModel;
using DataAccess;
using DataAccess.Repository;
using DatabaseMigration;
using DomainObjects;
using FakeDataProvider;
using GUI.Converters;
using MemoryModel;
using SharedObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GUI
{
    public class SCU : AdleSCUBase
    {
        #region Ctor and singleton
        public static SCU GetSCU()
        {
            if (_scu == null)
                _scu = new SCU();

            return _scu;
        }

        private SCU()
        {
            Invoker = new Invoker(this);
            MemoryManager = new MemoryManager(this);
            ActionsNameList = new List<string>();
            ActionListener = new Actioner(this);
        }
        #endregion Ctor and singleton

        #region Private Members
        private static SCU _scu;
        private IUnitOfWork GetContext { get { return UnitOfWorkFactory.CreateBasicContext(new DB()); } }

        #endregion Private Members

        #region Properties
        public List<string> ActionsNameList { get; set; }

        #endregion Properties

        #region Methods

        public override IUnitOfWork GetContextMember()
        {
            return new SCU().GetContext;
        }

        public static IUnitOfWork GetContextMemberFromSCU()
        {
            return new SCU().GetContext;
        }

        #region Start Stop Methods
        public override void BeginActionListener()
        {
            _scu.ActionListener.StartActioner();
        }

        public override void BeginInvokerListener()
        {
            _scu.Invoker.StartInvoker();
        }

        public override void Stop()
        {
            _scu.Invoker.Stop();
            _scu._actioner.Stop();
        }

        public override bool InitSystem(bool IncludeSearhing = false)
        {
            GetAreas();
            GetItems();
            GetActions();

            if (IncludeSearhing)
            {
                SearcItems();
                //MapDevicesAndFeilds
            }

            return true;
        }

        public override void BeginLister()
        {
            BeginActionListener();
            BeginInvokerListener();
        }

        #endregion Start Stop Methods

        #region GetMethods

        public override List<string> SearcItems()
        {
            return new List<string>();
        }
        public override void GetActions()
        {
            using (IUnitOfWork ouw = GetContext)
            {
                List<Item> items = ouw.Repository<Item>().FindAll().ToList();

                List<AdleItemBase> AdleitemsList = new List<AdleItemBase>();

                foreach (var item in items)
                {
                    AdleitemsList.Add(new ItemConverter().DomainObjectToAdleObject(item));
                }

                Actions = new List<IAction>();

                foreach (var item in AdleitemsList)
                {
                    var Actions = item.GetActionableCommands();
                    ActionsNameList.AddRange(Actions);
                }
            }
        }

        public override void GetAreas()
        {
            using (IUnitOfWork ouw = GetContext)
            {
                List<Area> areas = ouw.Repository<Area>().FindAll().ToList();

                foreach (var area in areas)
                {
                    var newArea = new AreaConverter().DomainObjectToAdleObject(area);
                    newArea.Manager = this;
                    Areas.Add(newArea);
                }
            }
        }

        public override void GetItems()
        {
            Assembly asm = Assembly.LoadFile($"{Environment.CurrentDirectory}\\ItemModel.dll");
            if (asm == null)
                throw new Exception(); //TODO: Throw Exception

            var knownItemsTypes = asm.GetTypes().Where(x => x.GetCustomAttributes(typeof(ADLEDeviceAttribute), false).Length > 0).ToList();
            KnownItems.AddRange(knownItemsTypes);
        }

        public override void GetMemories()
        {
            throw new NotImplementedException();
        }

        #endregion GetMethods

        #region Registeration Methods
        public override void RegisterAction(AdleAreaBase area, AdleItemBase item, IAction action)
        {
            throw new NotImplementedException();
        }

        public override void RegisterArea(AdleAreaBase area)
        {
            if (Areas == null)
                Areas = new List<AdleAreaBase>();

            area.Manager = this;
            area.RootArea = area.RootArea;
            area.SubAreas = null;
            Areas.Add(area);

            using (var ouw = GetContext)
            {
                ouw.Repository<Area>().Add(new AreaConverter().AdleObjectToDomainObject(area));
                ouw.SaveChanges();
            }

        }

        public override void RegisterItem(AdleAreaBase area, AdleItemBase item)
        {
            try
            {
                var foundArea = GetAreaByName(area.Name);
                if (foundArea == null)
                    return;

                item.AreaOfItem = area;
                item.Manager = this;
                item.Name = $"{area.Name} {item.GetType().Name} ({item.IpV4})";

                using (var ouw = GetContext)
                {
                    var domainItem = new ItemConverter().AdleObjectToDomainObject(item);
                    domainItem.CreatedDate = DateTime.Now;
                    domainItem.CreatedBy = GetCurrentUserName();
                    var addedIem = ouw.Repository<Item>().Add(domainItem);
                    ouw.SaveChanges();

                    foundArea.RegisterItem(new ItemConverter().DomainObjectToAdleObject(addedIem));
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion Registeration Methods

        #endregion Methods
    }
}
