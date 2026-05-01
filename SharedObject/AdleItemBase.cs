using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SharedObject
{
    public abstract class AdleItemBase : IBase, IDatabaseMember
    {
        #region IBase, IDatabaseMember

        public int ID { get; set; }

        private AdleSCUBase _manager;
        public AdleSCUBase Manager
        {
            get
            {
                return _manager;
            }

            set
            {
                _manager = value;
            }
        }

        public string Name { get; set; }

        #endregion IBase

        #region Constractor

        public AdleItemBase()
        {
            AreaOfItem = null;
        }

        public AdleItemBase(AdleAreaBase areaOfItem, string name) : this()
        {
            AreaOfItem = areaOfItem;
            Name = name;
        }

        public AdleItemBase(AdleAreaBase areaOfItem) : this()
        {
            AreaOfItem = areaOfItem;
            Name = Guid.NewGuid().ToString();
        }

        #endregion Constractor

        #region Properties

        public bool Availablity { get; set; }

        public string IpV4 { get; set; }

        public string IpV6 { get; set; }

        public AdleAreaBase AreaOfItem { get; set; }


        #endregion Properties

        #region Methods

        public void PlaceCommand(IAction command, params object[] param)
        {
            command.SetParams(param);

            if (Manager != null)
                Manager.Invoker.PlaceCommand(command);
            else
                command.Execute(DateTime.Now);
        }


        public List<string> GetActionableCommands()
        {
            var list = this.GetType().GetProperties().Where(x => x.GetCustomAttributes(typeof(ActionableAttribute), false).Length > 0).Select(x => x.Name).ToList();
            return list;
        }

        public IAction GetAction(PropertyInfo info)
        {
            return (IAction)info.GetValue(this);
        }

        #endregion Methods
    }
}
