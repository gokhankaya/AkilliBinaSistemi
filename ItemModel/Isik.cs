using ItemModel.Actions;
using SharedObject;
using System;

namespace ItemModel
{
    [ADLEDevice]
    public class Isik : AdleItemBase
    {
        #region Action Definitions
        [Actionable]
        public IAction Close { get; private set; }

        [Actionable]
        public IAction Open { get; private set; }

        [Actionable]
        public IAction GetState { get; set; }

        #endregion Action Definitions

        #region Properties

        public bool CurrentState { get; private set; } = false;

        #endregion

        #region Methods

        private void DefineActions()
        {
            Close = new CloseAction(this);
            Open = new OpenAction(this);
            GetState = new BoolGetAction(this);
        }

        [Actionable("Open")]
        public void OpenLight()
        {
            PlaceCommand(Open, "Light", "Open", IpV4);
            CurrentState = true;
        }

        [Actionable("Close")]
        public void CloseLight()
        {
            PlaceCommand(Close, "Light", "Close", IpV4);
            CurrentState = false;
        }

        [CheckMethod]
        [Actionable("State")]
        public bool GetLigthState()
        {
            GetState.SetParams("Light", "State", IpV4);
            GetState.Execute(DateTime.Now);
            CurrentState = (bool)GetState.GetValue();
            return CurrentState;
        }

        #endregion Methods

        #region Constructor 
        public Isik() : base()
        {
            DefineActions();
        }

        public Isik(string name) : base(null, name)
        {
            DefineActions();
        }

        public Isik(AdleAreaBase areaOfItem, string name) : base(areaOfItem, name)
        {
            DefineActions();
        }

        public Isik(AdleAreaBase areaOfItem) : base(areaOfItem)
        {
            DefineActions();
        }
        #endregion Constructor 

    }
}
