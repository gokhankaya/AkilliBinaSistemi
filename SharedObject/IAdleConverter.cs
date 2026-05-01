using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedObject
{
    public interface IAdleConverter<T_ADLE_OBJECT, T_TO_OBJECT>
    {
        T_ADLE_OBJECT DomainObjectToAdleObject(T_TO_OBJECT Obj, bool addSubs = true, AdleSCUBase SCU = null);

        T_TO_OBJECT AdleObjectToDomainObject(T_ADLE_OBJECT AdleObject, bool addSubs = true);
    }
}
