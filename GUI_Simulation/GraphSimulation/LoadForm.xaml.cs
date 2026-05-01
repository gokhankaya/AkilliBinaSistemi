using AdleGraph;
using AdleGraph.Interfaces;
using SimulationObjects;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace GUI_Simulation.GraphSimulation
{
    /// <summary>
    /// Interaction logic for LoadForm.xaml
    /// </summary>
    public partial class LoadForm : Window
    {
        public LoadForm()
        {
            InitializeComponent();
            DataContext = this;
            GraphsList = new ObservableCollection<IGraph>();
            loadGraphs();
        }

        private void loadGraphs()
        {
            using (var ouw = Utility.GetOuw())
            {
                var list = ouw.Repository<GraphObject>().FindAll().Include("GraphNodeDeviceMappings").Include("GraphNodeDeviceMappings.Device").ToList();

                if (list == null)
                    return;

                foreach (var item in list)
                {
                    IGraph graph = Graph.CreateNewGraphFromMatrixString(item.MatrixValue, item.Name);

                    foreach (var maping in item.GraphNodeDeviceMappings)
                    {
                        var foundNode = graph.NodeList.Find(x => x.Name == maping.NodeName);
                        if (foundNode == null)
                            continue;

                        if (maping.Device == null)
                            continue;

                        foundNode.Tag = maping.Device;
                    }

                    GraphsList.Add(graph);
                }
            }
        }

        public ObservableCollection<IGraph> GraphsList { get; set; }
        public IGraph SelecttedGraph { get; private set; }

        private void lstGraphs_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lstGraphs.SelectedItem == null)
                return;

            SelecttedGraph = (IGraph)lstGraphs.SelectedItem;
            this.Close();
        }
    }
}
