using ActionModel;
using SharedObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ItemModel.Actions
{
    public class OpenAction : ParameredCommand, IAction
    {
        #region Fields
        private readonly AdleItemBase _item;
        private DateTime _time;
        #endregion Fields

        #region Constractor
        public OpenAction(AdleItemBase item)
        {
            _item = item;
        }

        #endregion Constractor

        #region Properties
        public string Definition
        {
            get
            {
                return $"{_item.Name} nesnesi açma aksiyonu";
            }
        }

        public DateTime ExecutionTime
        {
            get
            {
                return _time;
            }
        }
        #endregion Properties

        #region Methods
        public string Name
        {
            get
            {
                return "ToP";//TurnOnPower
            }
        }

        public void Execute(DateTime time)
        {
            this._time = time;

            IAdaptor<bool> adaptor = new Adaptor<bool>();

            base.Params[1] = "Open";

            adaptor.Execute(Params);
            Console.WriteLine(GetLog());
            Debug.WriteLine(GetLog());
        }

        public string GetLog()
        {
            return $" {_time.ToString()}: {_item.Name} açıldı.";
        }

        public object GetValue()
        {
            return null;
        }

        #endregion   Methods
    }
}
