using System.Collections.Generic;

namespace SharedObject
{
    public interface IInvoker
    {
        List<IAction> ExecutedCommands();
        void PlaceCommand(IAction command);
        void PlaceCommand(AdleItemBase Item, string actionName);

        void Stop();
        void StartInvoker();
      
    }
}