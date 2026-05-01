using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedObject
{
    public class AdleAreaBase : IBase, IDatabaseMember, IDimentionalMember
    {

        #region Fields 

        private List<AdleItemBase> items;

        private List<AdleAreaBase> subAreas;


        #endregion Fields

        #region IBase IDatabaseMember IDimentionalMember Implementation

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

        public double Height { get; set; }

        public double Width { get; set; }


        #endregion IBase IDatabaseMember IDimentionalMember Implementation

        #region Constractor

        public AdleAreaBase()
        {
            SubAreas = null;
            RootArea = null;
            Items = new List<AdleItemBase>();
            Name = Guid.NewGuid().ToString();
        }

        public AdleAreaBase(string name) : this()
        {
            Name = name;
        }

        public AdleAreaBase(AdleAreaBase rootArea, string name) : this()
        {
            RootArea = rootArea;
            Name = name;
        }

        public AdleAreaBase(AdleAreaBase rootArea) : this()
        {
            RootArea = rootArea;
            Name = Guid.NewGuid().ToString();
        }

        #endregion Constractor

        #region Properties

        public List<AdleItemBase> Items
        {
            get
            {
                if (items == null)
                    items = new List<AdleItemBase>();

                return items;
            }
            set
            {
                items = value;
            }
        }

        public List<AdleAreaBase> SubAreas
        {
            get
            {
                if (subAreas == null)
                    subAreas = new List<AdleAreaBase>();

                return subAreas;
            }
            set
            {
                subAreas = value;
            }
        }

        public AdleAreaBase RootArea { get; set; }

        public bool AreaHasSubAreas { get { return subAreas?.Count > 0; } }

        public bool AreaHasItems { get { return items?.Count > 0; } }

        #endregion Properties

        #region Methods

        #region RegistertationMethods

        public virtual void RegisterToSubAreas(AdleAreaBase area)
        {
            if (SubAreas == null)
            {
                SubAreas = new List<AdleAreaBase>();
            }

            area.Manager = Manager;
            area.RootArea = this;
            SubAreas.Add(area);
        }

        public virtual void RegisterItem(AdleItemBase item)
        {
            if (Items == null)
            {
                Items = new List<AdleItemBase>();
            }

            item.Manager = Manager;
            Items.Add(item);
        }

        #endregion RegistertationMethods

        #region Public Methods
        public override string ToString()
        {
            string name = "";
            GetRootName(this, ref name);

            name = name.TrimEnd(',');
            return name;
        }

        #endregion Public Methods

        #region Private Methods
        private void GetRootName(AdleAreaBase area, ref string measaj)
        {
            measaj = $"{area.Name}, {measaj}";

            if (area.RootArea != null)
            {
                GetRootName(area.RootArea, ref measaj);
            }
        }

        #endregion Private Methods

        #endregion Methods

    }
}
