using AdleGraph.Interfaces;
using GUI_Simulation.GraphSimulation;
using SimulationObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static IoC.Container;

namespace GUI_Simulation
{
    /// <summary>
    /// Interaction logic for MainWindowForGraph.xaml
    /// </summary>
    public partial class MainWindowForGraph : Window, INotifyPropertyChanged
    {
        #region Fields
        private List<IGraph> _graphList;
        private string _graphName;
        private int _tekrarSayisi;
        #endregion Fields

        #region ctor
        public MainWindowForGraph()
        {
            InitializeComponent();
            DataContext = this;
            _graphList = new List<IGraph>();
            tekrarSayisi = 1;
        }

        public MainWindowForGraph(bool loader)
        {
            InitializeComponent();
            if (loader)
                this.Visibility = Visibility.Collapsed;

            DataContext = this;
            _graphList = new List<IGraph>();
            tekrarSayisi = 1;



            if (loader)
            {
                returnGraphValue = graphLoader(loader);
            }
        }

        #endregion ctor

        #region PropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion PropertyChanged Members

        #region Events
        private void btnAddNewGraph_Click(object sender, RoutedEventArgs e)
        {
            AddNewGraph();
        }

        private void lsbGraphs_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lsbGraphs.SelectedItems.Count <= 0)
                return;

            var selectedItem = (IGraph)((ListBoxItem)lsbGraphs.SelectedItem).Tag;
            GraphsDetails details = new GraphsDetails(this, selectedItem);
            details.Show();
            this.Hide();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (lsbGraphs.SelectedItem == null)
            {
                MessageBox.Show("Koşacak Çizelgeyi seçiniz.");
                return;
            }

            var graph = (IGraph)((ListBoxItem)lsbGraphs.SelectedItem).Tag;

            RunGraph(graph, tekrarSayisi);
        }

        public static List<List<INode>> RunGraph(IGraph graph, int tekrarSayisi)
        {
            var runSequence = Task.Factory.StartNew(() => { return graph.Run(graph.NodeList.First(), graph.NodeList.Last(), false, tekrarSayisi, false, 10); });

            runSequence.Wait();

            var devicesSequences = runSequence.Result;

            foreach (var sequence in devicesSequences)
            {
                foreach (var node in sequence)
                {
                    var device = (DeviceBase)node.Tag;
                    var actions = device.GetActionableMethodsOFDevice();
                    actions[0].Invoke(device, new object[] { device.ip });
                    //    var device = (Item)node.Tag;
                    //    var obje = new Utility.Converters.ItemConverter().DomainObjectToAdleObject(device, true, null);
                    //    //obje.
                }
            }

            return devicesSequences;
        }

        public static List<List<INode>> RunGraph(IGraph graph, int tekrarSayisi, INode startNode, INode stopNode)
        {
            if (startNode == null)
            {
                return null;
            }

            if (stopNode == null)
            {
                return null;
            }

            var runSequence = Task.Factory.StartNew(() => { return graph.Run(startNode, stopNode, false, tekrarSayisi, false, 10); });

            runSequence.Wait();

            var devicesSequences = runSequence.Result;

            foreach (var sequence in devicesSequences)
            {
                foreach (var node in sequence)
                {
                    var device = (DeviceBase)node.Tag;
                    var actions = device.GetActionableMethodsOFDevice();
                    actions[0].Invoke(device, new object[] { device.ip });
                }
            }

            return devicesSequences;
        }

        private void btnLoadNewGraph_Click(object sender, RoutedEventArgs e)
        {
            graphLoader();
        }

        private void btnSaveNewGraph_Click(object sender, RoutedEventArgs e)
        {
            if (lsbGraphs.SelectedItem == null)
                return;

            using (var uow = Utility.GetOuw())
            {
                IGraph selectedGraph = (IGraph)((ListBoxItem)lsbGraphs.SelectedItem).Tag;
                var foundGraph = uow.Repository<GraphObject>().Find(x => x.Name == selectedGraph.Name).FirstOrDefault();

                if (selectedGraph.NodeList == null || selectedGraph.NodeList.Count <= 0)
                {
                    MessageBox.Show($"{selectedGraph.Name} düğümü yok.");
                    return;
                }

                if (foundGraph != null)
                {
                    if (MessageBox.Show($"{foundGraph.Name} zaten ekli. Güncellemek ister misiniz?", "", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                        return;

                    foundGraph.MatrixValue = selectedGraph.ToMatrisString();
                    uow.Repository<GraphObject>().Update(foundGraph);
                    uow.SaveChanges();

                    var deviceMappingList = uow.Repository<GraphNodeDeviceMapping>().Find(x => x.GraphID == foundGraph.ID).ToList();

                    string deviceNodeMappingCheck = "Aşağıdaki düğümler için cihaz eklenemedi.\n";
                    foreach (var node in selectedGraph.NodeList)
                    {
                        var device = (DeviceBase)node.Tag;
                        if (device == null)
                        {
                            deviceNodeMappingCheck += $" {node.Name},";
                            continue;
                        }

                        if (deviceMappingList.Exists(x => x.NodeName == node.Name && x.DeviceID == device.ID)) continue;

                        foundGraph.GraphNodeDeviceMappings.Add(new GraphNodeDeviceMapping()
                        {
                            Device = null,
                            DeviceID = device.ID,
                            Graph = null,
                            GraphID = foundGraph.ID,
                            NodeName = node.Name
                        });
                    }

                    uow.SaveChanges();
                    MessageBox.Show($"{selectedGraph.Name} güncellendi.");
                }

                if (foundGraph == null)
                    Ekle(uow, selectedGraph);
            }
        }

        private static void Ekle(DataAccess.Repository.IUnitOfWork uow, IGraph selectedGraph)
        {
            var graphobject = new GraphObject() { MatrixValue = selectedGraph.ToMatrisString(), Name = selectedGraph.Name };
            var savedGraph = uow.Repository<GraphObject>().Add(graphobject);
            uow.SaveChanges();

            if (graphobject.GraphNodeDeviceMappings == null)
            {
                graphobject.GraphNodeDeviceMappings = new List<GraphNodeDeviceMapping>();
            }


            foreach (var node in selectedGraph.NodeList)
            {
                graphobject.GraphNodeDeviceMappings.Add(new GraphNodeDeviceMapping()
                {
                    Device = null,
                    DeviceID = ((DeviceBase)node.Tag).ID,
                    Graph = null,
                    GraphID = savedGraph.ID,
                    NodeName = node.Name
                });
            }

            uow.SaveChanges();
            MessageBox.Show($"{selectedGraph.Name} veritabanına kaydedildi.");
        }

        private void btnRemoveSelectedGraph_Click(object sender, RoutedEventArgs e)
        {
            if (lsbGraphs.SelectedItem == null)
                return;

            IGraph selectedGraph = (IGraph)((ListBoxItem)lsbGraphs.SelectedItem).Tag;

            lsbGraphs.Items.RemoveAt(lsbGraphs.SelectedIndex);
            var graph = _graphList.Find(x => x.Name == selectedGraph.Name);
            if (graph != null)
                _graphList.Remove(graph);
        }

        private void btnShowGraph_Click(object sender, RoutedEventArgs e)
        {
            if (lsbGraphs.SelectedItems.Count <= 0)
                return;

            var selectedItem = (IGraph)((ListBoxItem)lsbGraphs.SelectedItem).Tag;
            graphShow window = new graphShow();
            window.UpdateBoad(selectedItem);
            window.ShowDialog();
        }

        private void btnShowMatrix_Click(object sender, RoutedEventArgs e)
        {
            if (lsbGraphs.SelectedItems.Count <= 0)
                return;

            var selectedItem = (IGraph)((ListBoxItem)lsbGraphs.SelectedItem).Tag;
            ShowMatrixWindow window = new ShowMatrixWindow();
            window.Show();
            window.Load(selectedItem.ToMatrisString());
        }

        private void btnCopy_Click(object sender, RoutedEventArgs e)
        {
            if (lsbGraphs.SelectedItems.Count <= 0) return;

            var selectedGraph = (IGraph)((ListBoxItem)lsbGraphs.SelectedItem).Tag;
            if (!AddNewGraph()) return;
            var newGraph = _graphList.Find(x => x.Name == GraphName);

            foreach (var node in selectedGraph.NodeList)
            {
                newGraph.AddNode(node);
            }

            foreach (var edge in selectedGraph.EdgeList)
            {
                newGraph.AddEdge(edge.Node1.Name, edge.Node2.Name, edge.Weight, edge.IsDirected);
            }

            newGraph.ShowEdgesWeights = selectedGraph.ShowEdgesWeights;
            SetDetails(newGraph);
        }

        private void btnDetail_Click(object sender, RoutedEventArgs e)
        {
            if (lsbGraphs.SelectedItems.Count <= 0) return;

            StringBuilder builder = new StringBuilder();
            var selectedGraph = (IGraph)((ListBoxItem)lsbGraphs.SelectedItem).Tag;
            builder.Append("Düğüm adı\t");
            builder.Append("Çıkan kenar sayısı\t");
            builder.Append("Gelen kenar sayısı\t");
            builder.Append("Cihaz\t");
            builder.Append("\n");

            var orderedList = selectedGraph.NodeList.OrderBy(x => x.Name).ToList();

            for (int i = 0; i < orderedList.Count; i++)
            {
                var node = orderedList[i];
                var startEdges = selectedGraph.EdgeList.Where(x => x.Node1.Name == node.Name).ToList();
                var startEdgeCount = startEdges == null ? "0" : startEdges.Count.ToString();

                var endEdges = selectedGraph.EdgeList.Where(x => x.Node2.Name == node.Name).ToList();
                var endEdgeCount = endEdges == null ? "0" : endEdges.Count.ToString();

                string value = $"{node.Name}\t{startEdgeCount}\t{endEdgeCount}\t{((DeviceBase)node.Tag).Name}\n";

                builder.Append(value);
            }

            string path = $@"C:\ADLE_REPORTS\{selectedGraph.Name}_{DateTime.Now.ToShortDateString()}_{DateTime.Now.ToShortTimeString().Replace(':', '-')}_{Guid.NewGuid().ToString()}.txt";
            System.IO.File.WriteAllText(path, builder.ToString());
            Process.Start(path);
        }

        private void btnGraphml_Click(object sender, RoutedEventArgs e)
        {
            string start = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><graphml><graph id=\"Graph\" uidGraph=\"[uidGraph]\" uidEdge=\"[uidEdge]\">";
            string end = "</graph></graphml>";
            string nodeStart = "<nodes>";
            string nodeEnd = "</nodes>";
            string edgeStart = "<edges>";
            string edgeEnd = "</edges>";

            string node = "<node positionX=\"500\" positionY=\"150\" id=\"[id]\" mainText=\"[name]\" upText=\"\" ></node>";
            string edge = "<edge vertex1=\"[id1]\" vertex2=\"[id2]\" isDirect=\"true\" weight=\"[w]\" useWeight=\"true\" hasPair=\"false\" id=\"[id]\" text=\"\" arrayStyleStart=\"\" arrayStyleFinish=\"\" model_width=\"4\" model_type=\"0\" model_curvedValue=\"0.1\" ></edge>";

            StringBuilder builder = new StringBuilder();

            if (lsbGraphs.SelectedItems.Count <= 0) return;

            var selectedGraph = (IGraph)((ListBoxItem)lsbGraphs.SelectedItem).Tag;

            var orderedList = selectedGraph.NodeList.OrderBy(x => x.Name).ToList();

            builder.Append(start);
            builder.Append(nodeStart);
            string uidGraph = orderedList.Count.ToString();
            for (int i = 0; i < orderedList.Count; i++)
            {
                INode nodeKey = orderedList[i];
                string nodeValue = node.Replace("[id]", (i).ToString()).Replace("[name]", nodeKey.Name);
                builder.Append(nodeValue);
            }
            builder.Append(nodeEnd);

            builder.Append(edgeStart);
            string uidEdge = (selectedGraph.EdgeList.Count() + 10000).ToString();
            for (int i = 0; i < selectedGraph.EdgeList.Count; i++)
            {
                IEdge edgeKey = selectedGraph.EdgeList[i];
                string startNodeIndex = (orderedList.FindIndex(x => x.Name == edgeKey.Node1.Name)).ToString();
                string endNodeIndex = (orderedList.FindIndex(x => x.Name == edgeKey.Node2.Name)).ToString();

                string edgeValue = edge.Replace("[id]", (10000 + i).ToString()).Replace("[id1]", startNodeIndex).Replace("[id2]", endNodeIndex).Replace("[w]", edgeKey.Weight.ToString().Replace(',', '.'));
                builder.Append(edgeValue);
            }
            builder.Append(edgeEnd);
            builder.Append(end);

            string path = $@"C:\ADLE_REPORTS\{selectedGraph.Name}_{DateTime.Now.ToShortDateString()}_{DateTime.Now.ToShortTimeString().Replace(':', '-')}_{Guid.NewGuid().ToString()}.graphml";
            System.IO.File.WriteAllText(path, builder.Replace("[uidGraph]", uidGraph.ToString()).Replace("[uidEdge]", uidEdge).ToString());
        }

        #endregion Events

        #region Properties

        public List<IGraph> GraphList
        {
            get
            {
                return _graphList;
            }
        }

        public string GraphName
        {
            get
            {
                return _graphName;
            }
            set
            {
                _graphName = value;
                OnPropertyChanged();
            }
        }

        public int tekrarSayisi
        {
            get
            {
                return _tekrarSayisi;
            }

            set
            {
                _tekrarSayisi = value;
                if (_tekrarSayisi <= 0)
                {
                    _tekrarSayisi = 1;
                }

                OnPropertyChanged();
            }
        }

        public IGraph returnGraphValue { get; set; }

        #endregion

        #region Methods
        private void Updatelsb(IGraph graph)
        {
            ListBoxItem item = new ListBoxItem();
            item.Tag = graph;
            item.Content = graph.ToString();

            lsbGraphs.Items.Add(item);
        }

        public void SetDetails(IGraph graph)
        {
            var Index = _graphList.FindIndex(x => graph.Name == x.Name);
            _graphList[Index] = graph;
            lsbGraphs.Items.Clear();

            foreach (var graphItem in _graphList)
            {
                Updatelsb(graphItem);
            }
        }

        private IGraph graphLoader(bool directLoader = false)
        {
            LoadForm frm = new LoadForm();
            frm.ShowDialog();

            IGraph selectedGraph = frm.SelecttedGraph;

            if (!directLoader && frm.SelecttedGraph != null)
            {
                _graphList.Add(selectedGraph);
                Updatelsb(selectedGraph);
            }
            frm = null;

            return selectedGraph;
        }

        private bool AddNewGraph()
        {
            if (string.IsNullOrEmpty(GraphName))
            {
                MessageBox.Show("Çizelge Adı Giriniz.");
                return false;
            }

            if (_graphList.Exists(x => x.Name == GraphName))
            {
                MessageBox.Show($"{GraphName} zaten var.");
                return false;
            }

            IGraph graph = Resolve<IGraph>(null, GraphName);
            _graphList.Add(graph);

            Updatelsb(graph);
            return true;
        }

        #endregion   Methods
    }
}
