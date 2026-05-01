using ItemModel.Actions;
using SharedObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemModel
{
    [ADLEDevice]
    public class Priz : AdleItemBase
    {
        #region Action Definitions
        [Actionable]
        public IAction Close { get; private set; }

        [Actionable]
        public IAction Open { get; private set; }
        #endregion Action Definitions

        #region Methods

        private void DefineActions()
        {
            Close = new CloseAction(this);
            Open = new OpenAction(this);
        }
        #endregion Methods

        #region Constructor 

        public Priz()
        {
            DefineActions();
        }

        public Priz(AdleAreaBase areaOfItem) : base(areaOfItem)
        {
            DefineActions();
        }

        public Priz(AdleAreaBase areaOfItem, string name) : base(areaOfItem, name)
        {
            DefineActions();
        }

        #endregion Constructor 

    }
}
