using DataAccess;
using DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimulationObjects
{
    public class Operate
    {
        private static bool _threadCheck;
        private Scenario _senaryo;
        public Operate(Scenario senaryo)
        {
            _threadCheck = false;
            _senaryo = senaryo;

            //using (IUnitOfWork uow = UnitOfWorkFactory.CreateContext())
            //{
            //    var senaryo = uow.Repository<Scenario>().FindAll()
            //        .Include("ActorsInScenario")
            //        .Include("ActorsInScenario.HabitsOfActor")
            //        .Include("ActorsInScenario.HabitsOfActor.OperationHabitMappingEntry")
            //        .Include("ActorsInScenario.HabitsOfActor.OperationHabitMappingEntry.Operation")
            //        .Include("ActorsInScenario.HabitsOfActor.OperationHabitMappingEntry.Operation.DevicesOfOperation")
            //        .Include("ActorsInScenario.HabitsOfActor.OperationHabitMappingEntry.Operation.DevicesOfOperation.DeviceBase")
            //        .Include("ActorsInScenario.HabitsOfActor.OperationHabitMappingEntry.Operation.DevicesOfOperation.AreaOfOperation")
            //        .Where(x => x.Name == senaryoName).FirstOrDefault();

            //    if (senaryo == null)
            //        return;

            //    _senaryo = senaryo;
            //}


        }

        public void Begin(DateTime begingingDate, int delayTime = 100, int howLong = 2, int periodByMin = 5)
        {
            int _delayTime = delayTime < 0 ? 100 : delayTime;
            int _howLong = howLong < 0 ? 2 : howLong;
            DateTime _startDate = begingingDate;
            int _periodByMin = periodByMin < 0 ? 5 : periodByMin;
            Random rnd;

            var task = Task.Factory.StartNew(async () =>
             {
                 if (_threadCheck)
                     return;

                 if (begingingDate == null)
                     return;

                 if (_senaryo == null)
                     return;

                 if (_senaryo.ActorsInScenario == null || _senaryo.ActorsInScenario.Count <= 0)
                     return;

                 DateTime endDate = DateTime.Today.AddDays(_howLong);
                 writeDateDetails(_startDate, detay: "StartDate:");
                 writeDateDetails(endDate, detay: "EndDate:");

                 DateTime currentTime = _startDate;
                 TimeSpan cycle = new TimeSpan(0, _periodByMin, 0);

                 int no = 1;
                 for (; currentTime < endDate;)
                 {
                     writeDateDetails(currentTime, no);

                     foreach (var actor in _senaryo.ActorsInScenario)
                     {
                         if (actor.HabitsOfActor == null || actor.HabitsOfActor.Count <= 1)
                             continue;

                         if (actor.CurrentWorkEndTime > currentTime)
                             continue;

                         foreach (var habit in actor.HabitsOfActor)
                         {
                             var olasiOperasyonlar = habit.OperationHabitMappingEntry.Where(x =>
                              ((x.Operation.StartTime.Hour < currentTime.Hour) ||
                              (x.Operation.StartTime.Hour == currentTime.Hour && x.Operation.StartTime.Minute < currentTime.Minute))
                              &&
                              (x.Operation.StartTime.AddMinutes(x.Operation.Duration.Minutes).Hour > currentTime.Hour) ||
                              (x.Operation.StartTime.AddMinutes(x.Operation.Duration.Minutes).Hour == currentTime.Hour &&
                              x.Operation.StartTime.AddMinutes(x.Operation.Duration.Minutes).Minute > currentTime.Minute)).ToList();

                             rnd = new Random((int)DateTime.Now.Ticks);
                             int random = rnd.Next(0, olasiOperasyonlar.Count);
                             await Task.Delay(50);
                             int randomDuration = rnd.Next(olasiOperasyonlar[random].MinDuration, olasiOperasyonlar[random].MaxDuration);

                             actor.CurrentWorkEndTime = currentTime.AddMinutes(randomDuration);

                             foreach (var operationDevice in olasiOperasyonlar[random].Operation.DevicesOfOperation.OrderBy(x => x.Sira))
                             {
                                 MethodInfo mInfo = operationDevice.DeviceBase.GetType().GetMethod(operationDevice.ActionName);

                                 var device = Device.Devices.DevicesList.Find(x => x.ip == operationDevice.DeviceBase.ip);

                                 if (device != null)
                                     mInfo.Invoke(device, null);
                             }
                         }
                     }

                     currentTime = currentTime + cycle;
                     no++;
                     await Task.Delay(_delayTime);
                 }

                 Console.ReadLine();

             });

            task.Start();
            task.Wait();

            if (task.Exception != null)
            {
                writeDateDetails(DateTime.Now, detay: task.Exception.Message);
            }
            else
            {
                writeDateDetails(DateTime.Now, detay: "Tamamlandı");
            }

        }

        private static void writeDateDetails(DateTime date, int no = 0, string detay = "")
        {
#if DEBUG
            string yazi = no == 0 ? detay : $"{no} -)";
            Console.WriteLine($"{yazi} {date.ToLongDateString() + " " + date.ToLongTimeString()}");
#endif
        }

    }
}
