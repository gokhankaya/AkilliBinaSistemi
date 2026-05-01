using AdleGraph;
using AdleGraph.Interfaces;
using IoC;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace GUI_Simulation
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Container.InitContainer();
            Container.Register<IGraph, Graph>();

            new SimulationPortal.PortalWindow().Show();
        }
    }
}
