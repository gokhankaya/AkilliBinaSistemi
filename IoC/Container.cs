using System;
using System.Collections.Generic;
using System.Linq;

namespace IoC
{
    public class Container
    {
        private static Container _iocContainer;
        private Dictionary<string, Type> _dictionary = null;

        public Container()
        {
            _dictionary = new Dictionary<string, Type>();
        }

        public static Container IoCcontainer
        {
            get
            {
                if (_iocContainer == null)
                {
                    _iocContainer = new Container();
                }
                return _iocContainer;
            }

            private set
            {
                _iocContainer = value;
            }
        }

        public static void InitContainer(bool createNew = false)
        {
            if (IoCcontainer == null || createNew)
            {
                IoCcontainer = new Container();
            }
        }

        public static void Register<TInput, Toutput>()
        {
            string nameOfInputType = typeof(TInput).Name;

            var foundType = IoCcontainer._dictionary.FirstOrDefault(x => x.Key == nameOfInputType);

            if (foundType.Value == null)
                IoCcontainer._dictionary.Add(nameOfInputType, typeof(Toutput));
        }

        public static TInput Resolve<TInput>()
        {
            var newObject = Resolve<TInput>(null);
            return newObject;
        }

        public static TInput Resolve<TInput>(params object[] param)
        {
            var foundValue = IoCcontainer._dictionary.FirstOrDefault(x => x.Key == typeof(TInput).Name);

            var newObject = param == null ? Activator.CreateInstance(foundValue.Value) : Activator.CreateInstance(foundValue.Value, param);

            return (TInput)newObject;
        }
    }
}
