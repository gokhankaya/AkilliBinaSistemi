using AdleGraph.Interfaces;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Linq.Expressions;

namespace GUI_Simulation
{
    /// <summary>
    /// Interaction logic for EdgeAddingForm.xaml
    /// </summary>
    public partial class EdgeAddingForm : Window, INotifyPropertyChanged
    {
        #region Ctor
        public EdgeAddingForm(ref IGraph graph)
        {
            InitializeComponent();
            DataContext = this;

            Graph = graph;

            AddToListbox(lsbKaynakNodeList);
            AddToListbox(lsbHedefNodeList);
            IsDirected = true;

            WindowTitle = "Kenar Ekle";
            ButtonContent = "Kaydet";
        }

        public EdgeAddingForm(ref IGraph graph, IEdge updateEdge) : this(ref graph)
        {
            var edgeFound = graph.EdgeList.Find(x => x.Name == updateEdge.Name);
            if (edgeFound == null)
                return;

            OnUpdateMode = true;
            SelectNode(updateEdge.Node1, lsbKaynakNodeList);
            SelectNode(updateEdge.Node2, lsbHedefNodeList);

            IsDirected = updateEdge.IsDirected;

            Weight = updateEdge.ShowWeight ? updateEdge.Weight : Weight;
            ShowWeights = updateEdge.ShowWeight;
            _newEdge = graph.EdgeList.Find(x => x.Name == updateEdge.Name);
            _oldEdgeName = updateEdge.Name;

            WindowTitle = "Kenar Güncelle";
            ButtonContent = "Güncelle";
        }

        #endregion Ctor

        #region Fields
        private IGraph _graph;
        private bool _isDirected;
        private bool _showWeights;
        private double _weight;

        private string _oldEdgeName = "";
        private IEdge _newEdge = null;
        private string _windowTitle;
        private string _buttonContent;

        #endregion Fields

        #region Properties
        public string WindowTitle
        {
            get
            {
                return _windowTitle;
            }
            set
            {
                _windowTitle = value; OnPropertyChanged();
            }
        }

        public string ButtonContent
        {
            get { return _buttonContent; }
            set
            {
                _buttonContent = value;
                OnPropertyChanged();
            }
        }

        public IGraph Graph { get { return _graph; } private set { _graph = value; } }

        public IEdge AddedEdge { get; private set; }

        public bool IsDirected
        {
            get
            {
                return _isDirected;
            }

            set
            {
                _isDirected = value;
                OnPropertyChanged();
                OnPropertyChanged("OK");
            }
        }

        public string OK
        {
            get
            {
                return IsDirected ? "---->" :
                                    "----";
            }

        }

        public bool ShowWeights
        {
            get
            {
                return _showWeights;
            }

            set
            {
                _showWeights = value;
                OnPropertyChanged();
            }
        }

        public double Weight
        {
            get
            {
                return _weight;
            }

            set
            {
                _weight = value;
                OnPropertyChanged();
            }
        }

        public bool OnUpdateMode { get; private set; } = false;
        #endregion Properties

        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion INotifyPropertyChanged Implementation

        #region Events
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var ResourceNode = (INode)((ListBoxItem)lsbKaynakNodeList.SelectedItem).Tag;
                var TargetNode = (INode)((ListBoxItem)lsbHedefNodeList.SelectedItem).Tag;

                if (OnUpdateMode)
                {
                    _newEdge.Node1 = ResourceNode;
                    _newEdge.Node2 = TargetNode;
                    _newEdge.IsDirected = IsDirected;
                    _newEdge.ShowWeight = ShowWeights;
                    _newEdge.Weight = Weight;
                }
                else
                {
                    AddedEdge = ShowWeights && Weight > 0 ?
                        Graph.AddEdge(ResourceNode, TargetNode, Weight, IsDirected) :
                        Graph.AddEdge(ResourceNode, TargetNode, null, IsDirected);
                }
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion Events

        #region Private Methods

        private void AddToListbox(ListBox control)
        {
            foreach (var node in Graph.NodeList)
            {
                ListBoxItem li = new ListBoxItem();
                li.Tag = node;
                li.Content = node.Name;
                control.Items.Add(li);
            }
        }

        private void SelectNode(INode node, ListBox control)
        {
            if (node == null)
                return;

            if (control == null || !control.HasItems)
                return;

            foreach (ListBoxItem item in control.Items)
            {
                if (!(item.Tag is INode))
                    continue;

                if (((INode)item.Tag).Name == node.Name)
                {
                    item.IsSelected = true;
                    break;
                }
            }
        }



        #endregion Private Methods
    }
}
