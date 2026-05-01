using System;

namespace SharedObject
{
    public interface IAction
    {
        void Execute(DateTime time);

        DateTime ExecutionTime { get; }

        string GetLog();

        string Name { get; }

        string Definition { get; }

        object GetValue();

        void SetParams(params object[] param);
    }
}