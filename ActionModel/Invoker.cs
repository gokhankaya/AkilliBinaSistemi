using DataAccess.Repository;
using SharedObject;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static SharedObject.Util;

namespace ActionModel
{
    public class Invoker : IInvoker, IBase
    {

        public Invoker()
        {

        }

        public Invoker(AdleSCUBase manager)
        {
            this.Manager = manager;
        }

        #region IBase

        private AdleSCUBase _manager;
        public AdleSCUBase Manager
        {

            get
            {
                if (_manager == null)
                {
                    throw new Exception("Manager Boş Olamaz");
                }

                return _manager;
            }

            set
            {
                _manager = value;
            }
        }

        public string Name { get; set; }
        #endregion IBase

        #region Fields

        private List<IAction> _commands;
        private List<IAction> _execudedCommands;
        private bool _listen = false;
        private bool _listedOnGoing = false;

        #endregion Fields

        #region Methods

        public List<IAction> ExecutedCommands()
        {
            return _execudedCommands;
        }

        public void PlaceCommand(IAction command)
        {
            if (_commands == null)
            {
                _commands = new List<IAction>();
            }

            _commands.Add(command);
        }

        public void Stop()
        {
            _listen = false;
        }

        public void StartInvoker()
        {
            if (_listedOnGoing)
            {
                Console.WriteLine("Sadece bir kere olabilir.");
                return;
            }

            _listen = true;
            _listedOnGoing = true;

            Task.Factory.StartNew(async () =>
            {
                while (_listen)
                {
                    if (_commands == null)
                    {
                        await Task.Delay(3000);
                        continue;
                    }

                    int l = _commands.Count;
                    for (int i = 0; i < _commands.Count; i++)
                    {
                        var command = _commands[i];

                        if (command == null)
                        {
                            continue;
                        }

                        //preExecute logs vs
                        command.Execute(DateTime.Now);
                        //postExecute logs vs

                        if (_execudedCommands == null)
                            _execudedCommands = new List<IAction>();

                        _execudedCommands.Add(command);
                        _commands.Remove(command);

                        i--;
                    }

                    await Task.Delay(3000);
                }

                if (!_listen)
                {
                    _listedOnGoing = false;
                    Console.WriteLine("Dinleme Durdu.");
                }
            });

            Console.WriteLine("Dinleme Başladı.");
            //a.EndInvoke(null);
        }
    

        public void PlaceCommand(AdleItemBase Item, string ActionName)
        {
            var item = Item;
            string actionName = ActionName;
            var actions = item.GetType().GetMethods().Where(x => x.GetCustomAttributes(typeof(ActionableAttribute), false).Length > 0).ToList();
            foreach (var method in actions)
            {
                foreach (var attribute in method?.CustomAttributes)
                {
                    foreach (var Argument in attribute.ConstructorArguments)
                    {
                        if (Argument.Value.ToString() != actionName)
                            continue;

                        method.Invoke(item, null);
                    }
                }
            }
        }

        #endregion
    }
}
