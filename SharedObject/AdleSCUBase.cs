using DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharedObject
{
    public abstract class AdleSCUBase
    {
        #region Fields

        internal protected List<AdleAreaBase> _areas;
        internal protected List<AdleItemBase> _items;
        internal protected List<IAction> _actions;
        internal protected IInvoker _invoker;
        internal protected List<Type> _knownItems;
        internal protected IMemoryManager _memoryManager;
        internal protected IActioner _actioner;
        private ICollection<AdleMemoryObject> _memories;

        #endregion Fields

        #region Properties
        public List<AdleItemBase> ItemsList { get { if (_items == null) _items = new List<AdleItemBase>(); return _items; } internal protected set { _items = value; } }

        public List<Type> KnownItems { get { if (_knownItems == null) _knownItems = new List<Type>(); return _knownItems; } internal protected set { _knownItems = value; } }


        public List<AdleAreaBase> Areas { get { if (_areas == null) _areas = new List<AdleAreaBase>(); return _areas; } internal protected set { _areas = value; } }

        public List<IAction> Actions { get { if (_actions == null) _actions = new List<IAction>(); return _actions; } internal protected set { _actions = value; } }

        public IInvoker Invoker { get { return _invoker; } internal protected set { _invoker = value; } }

        public IMemoryManager MemoryManager { get { return _memoryManager; } internal protected set { _memoryManager = value; } }

        public IActioner ActionListener { get { return _actioner; } internal protected set { _actioner = value; } }

        protected internal ICollection<AdleMemoryObject> Memories
        {
            get
            {
                if (_memories == null)
                    _memories = new List<AdleMemoryObject>();
                return _memories;
            }

            set
            {
                _memories = value;
            }
        }
        #endregion Properties

        #region Methods

        #region Search Methods

        public AdleAreaBase GetAreaByName(string nameOfArea, List<AdleAreaBase> space = null)
        {
            if (space == null)
            {
                space = _areas;
            }

            if (space == null || space.Count <= 0)
            {
                return null;
            }

            AdleAreaBase foundArea = null;

            foreach (var item in _areas)
            {
                if (item.Name == nameOfArea)
                {
                    foundArea = item;
                    break;
                }

                if (item.AreaHasSubAreas)
                {
                    foundArea = GetAreaByName(nameOfArea, item.SubAreas);
                }
            }

            return foundArea;
        }

        public AdleItemBase GetItemByNameFromAnAreaName(string AreaName, string ItemName)
        {
            AdleAreaBase area = GetAreaByName(AreaName);
            if (area == null)
            {
                return null;
            }

            var item = area.Items.Find(x => x.Name == ItemName);
            return item;
        }

        public List<string> GetActionableCommands(Type type)
        {
            var list = type.GetProperties().Where(x => x.GetCustomAttributes(typeof(ActionableAttribute), false).Length > 0).Select(x => x.Name).ToList();
            return list;
        }

        #endregion Search Methods

        #region Abstarct Methods

        public abstract IUnitOfWork GetContextMember();

        public abstract void RegisterArea(AdleAreaBase area);

        public abstract void RegisterItem(AdleAreaBase area, AdleItemBase item);

        public abstract void RegisterAction(AdleAreaBase area, AdleItemBase item, IAction action);

        public abstract bool InitSystem(bool IncludeSearhing = false);

        public abstract void BeginLister();

        public abstract void BeginInvokerListener();

        public abstract void BeginActionListener();

        public abstract void Stop();

        public abstract void GetItems();

        public abstract void GetAreas();

        public abstract void GetActions();

        public abstract void GetMemories();

        public abstract List<string> SearcItems();

        #endregion  Abstarct Methods

        public void PlaceCommand(AdleItemBase Item, string ActionName)
        {
            Invoker?.PlaceCommand(Item, ActionName);
        }

        public string GetCurrentUserName()
        {
            return "Current User";
        }

        #endregion Methods
    }
}
