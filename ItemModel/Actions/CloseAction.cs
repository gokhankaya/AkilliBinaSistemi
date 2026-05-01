using ActionModel;
using SharedObject;
using System;
using System.Diagnostics;

namespace ItemModel.Actions
{
    public class CloseAction : ParameredCommand, IAction
    {
        #region Fields
        private readonly AdleItemBase _item;
        private DateTime _time;
        #endregion Fields

        #region Constractor
        public CloseAction(AdleItemBase item)
        {
            _item = item;
        }

        #endregion Constractor

        #region Properties
        public string Definition
        {
            get
            {
                return $"{_item.Name} nesnesi kapatma aksiyonu";
            }
        }

        public DateTime ExecutionTime
        {
            get
            {
                return _time;
            }
        }

        public string Name
        {
            get
            {
                return "CoP"; //CutOffPower
            }
        }

        #endregion Properties

        #region Methods

        public void Execute(DateTime time)
        {
            this._time = time;

            IAdaptor<bool> adaptor = new Adaptor<bool>();

            Params[1] = "Close";
            Params[2] = _item.IpV4;

            adaptor.Execute(Params);
            Console.WriteLine(GetLog());
            Debug.WriteLine(GetLog());
        }

        public string GetLog()
        {
            return $" {_time.ToString()}: {_item.Name} kapandı.";
        }

        public object GetValue()
        {
            throw new NotImplementedException();
        }


        #endregion   Methods
    }
}
