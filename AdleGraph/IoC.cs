using System;
using System.Collections.Generic;
using System.Linq;

namespace AdleGraph
{
    public class IoC
    {
        private static IoC _ioc;

        private Dictionary<string, Type> _dictionary = null;

        private IoC()
        {
            _dictionary = new Dictionary<string, Type>();
        }

        public static IoC Ioc
        {
            get
            {
                if (_ioc == null)
                {
                    _ioc = new IoC();
                }
                return _ioc;
            }

            private set
            {
                _ioc = value;
            }
        }

        public static void InitContainer(bool createNew = false)
        {
            if (Ioc == null || createNew)
            {
                Ioc = new IoC();
            }
        }

        public static void Register<TInput, Toutput>()
        {
            string nameOfInputType = typeof(TInput).Name;

            var foundType = _ioc._dictionary.FirstOrDefault(x => x.Key == nameOfInputType);

            if (foundType.Value == null)
                _ioc._dictionary.Add(nameOfInputType, typeof(Toutput));
        }

        public static TInput Resolve<TInput>()
        {
            var newObject = Resolve<TInput>(null);
            return newObject;
        }

        public static TInput Resolve<TInput>(params object[] param)
        {
            var foundValue = _ioc._dictionary.FirstOrDefault(x => x.Key == typeof(TInput).Name);

            var newObject = Activator.CreateInstance(foundValue.Value, param);

            return (TInput)newObject;
        }
    }
}
