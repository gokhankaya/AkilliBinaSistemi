using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedObject
{
    public interface IAdaptor<T>
    {
        void Execute(params object[] param);

        T GetValue(params object[] param);
    }
}
