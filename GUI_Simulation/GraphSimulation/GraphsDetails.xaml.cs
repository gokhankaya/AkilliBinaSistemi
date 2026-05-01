using AdleGraph.Interfaces;
using DataAccess;
using GUI_Simulation.GraphSimulation;
using SimulationDB_Migrations;
using SimulationObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace GUI_Simulation
{
    /// <summary>
    /// Interaction logic for GraphsDetails.xaml
    /// </summary>
    public partial class GraphsDetails : Window, INotifyPropertyChanged
    {
        #region Constractors
        public GraphsDetails(Window main, IGraph graph)
        {
            InitializeComponent();
            DataContext = this;
            _addedItems = new List<DeviceBase>();
            _window = main;
            _graph = graph;

            FillLists();
            FillDevices().ConfigureAwait(false);

            this.Closing += GraphsDetails_Closing;
        }

        #endregion Constractors

        #region Fields

        private readonly Window _window;
        private IGraph _graph;
        private string _graphName;
        private string _nodeName;
        private List<DeviceBase> _addedItems;

        #endregion Fields

        #region Properties

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

        public string NodeName
        {
            get
            {
                return _nodeName;
            }

            set
            {
                _nodeName = value;
                OnPropertyChanged();
            }
        }


        #endregion  Properties

        #region Methods
        private DataAccess.Repository.IUnitOfWork GetOuw()
        {
            return UnitOfWorkFactory.CreateBasicContext(new DB());
        }

        private async Task FillDevices()
        {
            Container.IsEnabled = false;

            using (var uow = GetOuw())
            {
                var items = await Task.Run(() =>
                {
                    var query = uow.Repository<DeviceBase>().FindAll().Include("AreaBase");
                    var data = query.ToList();
                    return data;
                });
                foreach (var item in items)
                {
                    ComboBoxItem li = new ComboBoxItem();
                    li.Tag = item;
                    li.Content = $"{item.Type}: {item.AreaBase.Name} {item.ip}";
                    cmbItems.Items.Add(li);
                }
            }

            Container.IsEnabled = true;
        }

        private void FillLists()
        {
            GraphName = _graph.Name;

            FillNodeList();

            FillEdgeList();
        }

        private void FillEdgeList()
        {
            List<ListBoxItem> edgeCache = new List<ListBoxItem>();
            foreach (var edge in _graph.EdgeList)
            {
                ListBoxItem item = new ListBoxItem();
                item.Tag = edge;
                item.Content = edge.Name;
                edgeCache.Add(item);
            }

            lsbEdges.ItemsSource = edgeCache.OrderBy(x => x.Content).ToList();
            CollectionView view = CollectionViewSource.GetDefaultView(lsbEdges.ItemsSource) as CollectionView;
            view.Filter = CustomFilterEdge;
        }

        private void FillNodeList()
        {
            _addedItems.Clear();
            _graph.NodeList = _graph.NodeList.OrderBy(x => x.Name).ToList();
            List<ListBoxItem> nodeCache = new List<ListBoxItem>();
            foreach (var node in _graph.NodeList)
            {
                ListBoxItem item = new ListBoxItem();
                item.Tag = node;
                item.Content = node.Name;
                nodeCache.Add(item);
                _addedItems.Add((DeviceBase)node.Tag);
            }

            lsbNodes.ItemsSource = nodeCache.OrderBy(x => x.Content).ToList();
            CollectionView view = CollectionViewSource.GetDefaultView(lsbNodes.ItemsSource) as CollectionView;
            view.Filter = CustomFilterNode;
        }

        private bool CustomFilterNode(object obj)
        {
            bool result = true;
            if (!string.IsNullOrEmpty(txtNodeFilter.Text))
                result = (obj.ToString().IndexOf(txtNodeFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);

            return result;
        }

        private bool CustomFilterEdge(object obj)
        {
            bool result = true;
            if (!string.IsNullOrEmpty(txtEdgeFilter.Text))
                result = (obj.ToString().IndexOf(txtEdgeFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);

            return result;
        }

        #endregion Methods

        #region Events

        private void GraphsDetails_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ((MainWindowForGraph)_window).SetDetails(_graph);
            _window.Show();
        }

        private void btnAddNode_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cmbItems.SelectedItem == null)
                {
                    MessageBox.Show("Lütfen düğüm için eşlenecek bir cihaz seçimi yapınız.");
                    return;
                }

                var selectedDevice = (DeviceBase)((ComboBoxItem)cmbItems.SelectedItem).Tag;

                if (selectedDevice == null)
                {
                    MessageBox.Show("Seçili cihazın durumu belli değil. Lütfen farklı bir cihaz seçiniz.");
                    return;
                }

                var addedNode = _graph.AddNode(NodeName);
                addedNode.Tag = selectedDevice;
                _addedItems.Add(selectedDevice);

                FillNodeList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAddEdge_Click(object sender, RoutedEventArgs e)
        {
            EdgeAddingForm form = new EdgeAddingForm(ref _graph);
            form.ShowDialog();

            //lsbEdges.Items.Clear();
            //lsbNodes.Items.Clear();
            FillEdgeList();
        }

        private void btnShowGraph_Click(object sender, RoutedEventArgs e)
        {
            graphShow window = new graphShow();
            window.UpdateBoad(_graph);
            window.Show();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (lsbEdges.SelectedItem == null)
                return;

            if (!(((ListBoxItem)lsbEdges.SelectedItem).Tag is IEdge))
                return;

            var edge = (IEdge)((ListBoxItem)lsbEdges.SelectedItem).Tag;

            EdgeAddingForm form = new EdgeAddingForm(ref _graph, edge);
            form.ShowDialog();

            //lsbEdges.Items.Clear();
            //lsbNodes.Items.Clear();

            FillEdgeList();
        }

        private void btnRemoveNode_Click(object sender, RoutedEventArgs e)
        {
            if (lsbNodes.SelectedItem == null) return;

            if (!(((INode)((ListBoxItem)lsbNodes.SelectedItem).Tag) is INode)) return;

            var node = (INode)((ListBoxItem)lsbNodes.SelectedItem).Tag;

            if (MessageBox.Show($"{node.Name} düğümünü kaldırmak istediğinizden emin misiniz?", "", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No) return;

            var foundNode = _graph.NodeList.Find(x => x.Name == node.Name);
            var foundEdgesNode1 = _graph.EdgeList.FindAll(x => x.Node1.Name == foundNode.Name);
            var foundEdgesNode2 = _graph.EdgeList.FindAll(x => x.Node2.Name == foundNode.Name);

            for (int i = 0; i < foundEdgesNode1.Count; i++)
            {
                var node1 = _graph.EdgeList.Find(x => x.Name == foundEdgesNode1[i].Name);
                if (node1 == null) continue;

                _graph.EdgeList.Remove(node1);
                i = i - 1;
            }

            for (int i = 0; i < foundEdgesNode2.Count; i++)
            {
                var node2 = _graph.EdgeList.Find(x => x.Name == foundEdgesNode2[i].Name);
                if (node2 == null) continue;

                _graph.EdgeList.Remove(node2);
            }

            _graph.NodeList.Remove(foundNode);

            //lsbEdges.Items.Clear();
            //lsbNodes.Items.Clear();

            FillLists();
        }

        private void btnRemoveEdge_Click(object sender, RoutedEventArgs e)
        {
            if (lsbEdges.SelectedItem == null) return;

            if (!(((ListBoxItem)lsbEdges.SelectedItem).Tag is IEdge)) return;

            var edge = (IEdge)((ListBoxItem)(lsbEdges.SelectedItem)).Tag;

            if (MessageBox.Show($"{edge.Name} kenarını kaldırmak istediğinizden emin misiniz?", "", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No) return;

            _graph.EdgeList.Remove(edge);
            //lsbEdges.Items.Clear();
            //lsbNodes.Items.Clear();

            FillLists();

        }

        private void txtNodeFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(lsbNodes.ItemsSource).Refresh();
        }

        private void txtEdgeFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(lsbEdges.ItemsSource).Refresh();
        }

        private void lsbNodes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lsbNodes.SelectedItem == null)
                return;

            NodeName = ((ListBoxItem)lsbNodes.SelectedItem).Content.ToString();
            INode selectedNode = (INode)((ListBoxItem)lsbNodes.SelectedItem).Tag;

            for (int i = 0; i < cmbItems.Items.Count; i++)
            {
                ((ComboBoxItem)cmbItems.Items[i]).IsSelected = ((DeviceBase)((ComboBoxItem)cmbItems.Items[i]).Tag).Name == ((DeviceBase)selectedNode.Tag).Name;
            }
        }


        #endregion  Events

        #region INotifyPropertyChanged Implementaion
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion INotifyPropertyChanged Implementaion

        private void btnRemoveWithOutWieght_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show($"Ağırlıksız olan kenarı kaldırmak istediğinizden emin misiniz?", "", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No) return;


            for (int i = 0; i < _graph.EdgeList.Count; i++)
            {
                if (_graph.EdgeList[i].Weight > 0) continue;

                _graph.EdgeList.RemoveAt(i);
                i--;
            }

            FillLists();
        }
    }
}
