using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedObject
{
    public class Util
    {
        public delegate void RequestResponseListener();
        public delegate void ActionListener();
        public delegate void MemoryCaptureHandler();
    }
}
