using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedObject
{
    public class ParameredCommand
    {
        public virtual void SetParams(params object[] param)
        {
            if (param == null || param.Length <= 0)
                return;

            Params = param;
        }


        private object[] _params;
        public virtual object[] Params
        {
            get
            {
                if (_params == null)
                {
                    _params = new object[3];
                }

                return _params;
            }
            set
            {
                _params = value;
            }
        }

    }
}
