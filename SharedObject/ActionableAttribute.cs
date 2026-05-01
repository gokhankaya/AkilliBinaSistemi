using System;

namespace SharedObject
{
    public class ActionableAttribute : Attribute
    {
        private readonly string _actionName;

        public ActionableAttribute(string name = "")
        {
            _actionName = name;
        }
    }
}