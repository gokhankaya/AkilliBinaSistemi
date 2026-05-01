using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedObject
{
    public interface IIoTExecutor
    {
        void Execute(params object[] param);

        object GetValue();
    }
}
