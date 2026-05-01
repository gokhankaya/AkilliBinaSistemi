using ActionModel;
using SharedObject;
using System;
using System.Diagnostics;

namespace ItemModel.Actions
{
    public class BoolGetAction : ParameredCommand, IAction
    {
        #region Fields
        private readonly AdleItemBase _item;
        private DateTime _time;
        private bool _state = false;

        #endregion Fields

        #region Constractor
        public BoolGetAction(AdleItemBase item)
        {
            _item = item;
        }

        #endregion Constractor

        #region Properties

        public string Name
        {
            get
            {
                return "Durum";
            }
        }

        public DateTime ExecutionTime
        {
            get
            {
                return _time;
            }
        }

        public string Definition
        {
            get
            {
                return "";
            }
        }



        #endregion Properties

        #region Methods
        public void Execute(DateTime time)
        {
            _time = time;
            //TODO :IoC
            base.Params[2] = _item.IpV4;
            IAdaptor<bool> adaptor = new Adaptor<bool>();
            _state = (bool)adaptor.GetValue(Params);

            //Debug.WriteLine(GetLog());

        }

        public string GetLog()
        {
            return $" {_time.ToString()}: {_item.Name} Durum Çekildi: {_state}.";
        }

        public object GetValue()
        {
            return _state;
        }

        #endregion Methods
    }
}
