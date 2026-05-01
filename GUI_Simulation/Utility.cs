using Accord.MachineLearning.DecisionTrees;
using Accord.MachineLearning.DecisionTrees.Learning;
using Accord.Math;
using DataAccess;
using GUI_Simulation;
using SimulationDB_Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI_Simulation
{
    public class Utility
    {
        public static DataAccess.Repository.IUnitOfWork GetOuw()
        {
            return UnitOfWorkFactory.CreateBasicContext(new DB());
        }
    }
}
