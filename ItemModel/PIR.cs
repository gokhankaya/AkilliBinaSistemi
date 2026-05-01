using ItemModel.Actions;
using SharedObject;
using System;

namespace ItemModel
{
    [ADLEDevice]
    public class PIR : AdleItemBase
    {
        #region Action Definitions

        [Actionable]
        public IAction GetState { get; set; }

        #endregion Action Definitions

        #region Properties

        public bool CurrentState { get; private set; } = false;

        #endregion

        #region Methods

        private void DefineActions()
        {
            GetState = new BoolGetAction(this);
        }

        [CheckMethod]
        [Actionable("State")]
        public bool GetPIRState()
        {
            GetState.SetParams("PIR", "State", IpV4);
            GetState.Execute(DateTime.Now);
            CurrentState = (bool)GetState.GetValue();
            return CurrentState;
        }

        #endregion Methods

        #region Constractor
        public PIR()
        {
            DefineActions();
        }

        public PIR(AdleAreaBase areaOfItem) : base(areaOfItem)
        {
            DefineActions();
        }

        public PIR(AdleAreaBase areaOfItem, string name) : base(areaOfItem, name)
        {
            DefineActions();
        }

        #endregion Constractor
    }
}
