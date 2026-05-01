using Accord.MachineLearning.DecisionTrees;
using Accord.MachineLearning.DecisionTrees.Learning;
using AdleGraph.Interfaces;
using SimulationObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;


namespace GUI_Simulation.AnomalyExploration
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region Ctor
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            GraphList = new List<IGraph>();
        }

        #endregion Ctor

        #region Fields
        private List<IGraph> _graphList;
        private IGraph _selectedGRaph;
        private int _deviceCount;

        private int _inputWindowLength = 10;
        private int _countOfDecisionTreesInForest = 5;
        private int _scenarioCount = 20;
        private string _randomForestDetails = "";



        #endregion

        #region Properties
        public List<DeviceBase> Devices { get; set; }
        public List<DecisionTree> randomForest { get; set; } = new List<DecisionTree>();
        public List<IGraph> GraphList
        {
            get
            {
                return _graphList;
            }

            private set
            {
                _graphList = value;

                GraphListRenewed();
            }
        }

        public int DeviceCount
        {
            get
            {
                return _deviceCount;
            }

            set
            {
                _deviceCount = value;
                propertyChanged();
            }
        }

        public int InputWindowLength
        {
            get
            {
                return _inputWindowLength;
            }

            set
            {
                _inputWindowLength = value;
                propertyChanged();
            }
        }

        public int CountOfDecisionTreesInForest
        {
            get
            {
                return _countOfDecisionTreesInForest;
            }

            set
            {
                _countOfDecisionTreesInForest = value;
                propertyChanged();
            }
        }

        public int SenaryoCount
        {
            get
            {
                return _scenarioCount;
            }

            set
            {
                _scenarioCount = value;
                propertyChanged();
            }
        }

        #region Algorithm Properties

        public List<Sensor> sensors { get; set; } = new List<Sensor>();
        public List<Scenario> scenarios { get; set; } = new List<Scenario>();
        public List<InputWindow> s0 { get; set; } = new List<InputWindow>();
        public List<InputWindow> s1 { get; set; } = new List<InputWindow>();
        public double[][] proximityMatrix { get; set; }

        #endregion Algorithm Properties

        #endregion Properties

        #region Events
        private void btnSelectGraph_Click(object sender, RoutedEventArgs e)
        {
            if (cmbGraphs.SelectedItem == null)
            {
                MessageBox.Show("Lütfen Çizelge Seçiniz.");
                return;
            }

            if (((ListBoxItem)cmbGraphs.SelectedItem).Tag == null)
            {
                MessageBox.Show("Lütfen Çizelge Seçiniz.");
                return;
            }

            _selectedGRaph = (IGraph)(((ListBoxItem)cmbGraphs.SelectedItem).Tag);
            Devices = new List<DeviceBase>();
            sensors = new List<Sensor>();

            for (int i = 0; i < _selectedGRaph.NodeList.Count; i++)
            {
                var _device = (DeviceBase)_selectedGRaph.NodeList[i].Tag;
                var _foundDevice = Devices.Where(x => x.Name == _device.Name).FirstOrDefault();
                if (_foundDevice != null)
                    continue;

                Devices.Add(_device);

                sensors.Add(new Sensor()
                {
                    ID = i + 1,
                    Name = _device.Name,
                    Type = _device.Type,
                    IP = _device.ip
                });
            }

            DeviceCount = Devices.Count;
            btnShowSensors_Click(null, null);
        }

        private void btnManageGraphs_Click(object sender, RoutedEventArgs e)
        {
            MainWindowForGraph graphWindow = new MainWindowForGraph();
            graphWindow.ShowDialog();

            GraphList = graphWindow.GraphList;
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedGRaph == null || _selectedGRaph?.NodeList?.Count <= 0)
            {
                MessageBox.Show("Lütfen geçerli bir çizelge seçiniz.");
                return;
            }

            //TODO: Bunlar seçilebilir olmalı.
            INode startNode = _selectedGRaph.NodeList.Find(x => x.Name.StartsWith("8P"));
            INode stopNode = _selectedGRaph.NodeList.Find(x => x.Name.StartsWith("10P"));
            var sequences = MainWindowForGraph.RunGraph(_selectedGRaph, SenaryoCount, startNode, stopNode);

            if (!convertSequenceToScenarios(sequences)) return;
            if (!createS0()) return;
            if (!createS1()) return;
            if (!createRandomForest()) return;
            if (!cerateProximityMatrix()) return;

            btnShowSenaryo_Click(null, null);
        }

        private void btnShowSensors_Click(object sender, RoutedEventArgs e)
        {
            FillList(sensors);
        }

        private void btnShowSenaryo_Click(object sender, RoutedEventArgs e)
        {
            FillList(scenarios);
        }

        private void btnS0_Click(object sender, RoutedEventArgs e)
        {
            FillListWithData(s0);
        }

        private void btnS1_Click(object sender, RoutedEventArgs e)
        {
            FillListWithData(s1);
        }

        private void btnRF_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_randomForestDetails))
            {
                MessageBox.Show("Karar ağaçları oluşmamış. Lütfen öncelikle cizelge seçerek başla tuşuna basınız.");
                return;
            }
            new DecisionTreeShown(_randomForestDetails).ShowDialog();
        }

        private void btnGrafic_Click(object sender, RoutedEventArgs e)
        {
            if (!showInGraph()) return;
        }
        #endregion Events

        #region Methods

        private void GraphListRenewed()
        {
            cmbGraphs.Items.Clear();

            ListBoxItem firstItem = new ListBoxItem();
            firstItem.Content = _graphList.Count > 0 ? "Lütfen işlem yapmak istediğiniz çizelgeyi seçiniz."
                    : "Lütfen Çizelgeler butonu ile çizelge yükleyiniz.";
            firstItem.Tag = null;

            cmbGraphs.Items.Add(firstItem);
            cmbGraphs.SelectedIndex = 0;

            foreach (var graph in _graphList)
            {
                ListBoxItem item = new ListBoxItem();
                item.Tag = graph;
                item.Content = graph.Name;
                cmbGraphs.Items.Add(item);
            }
        }

        private void FillList(IList list)
        {
            ArrayList itemsList = new ArrayList();
            if (list?.Count <= 0)
            {
                itemsList.Add("Gösterilecek eleman yok.");
                lvList.ItemsSource = itemsList;
                return;
            }

            foreach (var item in list)
            {
                itemsList.Add(item.ToString());
            }

            lvList.ItemsSource = itemsList;
        }

        private void FillListWithData(List<InputWindow> data)
        {
            ArrayList itemsList = new ArrayList();

            if (data == null || data?.Count <= 0)
            {
                itemsList.Add("Gösterilecek eleman yok.");
                lvList.ItemsSource = itemsList;
                return;
            }

            itemsList.Add("|----------- Time ----------->");

            for (int i = 0; i < sensors.Count; i++)
            {
                StringBuilder line = new StringBuilder();
                line.Append($"{i + 1}. sensor:\t");
                foreach (var windowInData in data)
                {
                    for (int j = 0; j < windowInData.states[i].Length; j++)
                    {
                        double result = windowInData.states[i][j];
                        line.Append(result); //Console.Write(result);
                    }
                    line.Append(" "); //Console.Write(" ");
                }
                itemsList.Add(line.ToString());//Console.WriteLine();
            }

            lvList.ItemsSource = itemsList;
        }

        private bool convertSequenceToScenarios(List<List<INode>> sequences)
        {
            if (sequences?.Count <= 0)
            {
                MessageBox.Show("Sequence Oluşmamış. Lütfen Öncelikle çizelge seçerek başlat tuşuna basın");
                return false;
            }

            scenarios = new List<Scenario>();
            for (int i = 0; i < _scenarioCount; i++)
            {
                Scenario newScenario = new Scenario()
                { name = $"{i + 1}. Senaryo" };

                for (int j = 0; j < sequences[i].Count; j++)
                {
                    var foundSensor = sensors.Where(x => x.Name == ((DeviceBase)sequences[i][j].Tag).Name).FirstOrDefault();
                    if (foundSensor != null)
                        newScenario.sensors.Add(foundSensor);
                }

                scenarios.Add(newScenario);
            }
            return true;
        }

        private bool createS0()
        {
            if (scenarios == null || scenarios.Count <= 0)
            {
                MessageBox.Show("Senaryo oluşmamış. \nLütfen seçmediyseniz çizelge seçerek başlat tuşuna basın.\nSeçtiniğiniz çizelgenin uygunluğunu denetleyiniz. ");
                return false;
            }

            InputWindow window = new InputWindow(sensors.Count, _inputWindowLength);
            window.order = 1;

            int count = 0;
            int step = 0;
            s0 = new List<InputWindow>();
            foreach (var scenario in scenarios)
            {
                foreach (var sensor in scenario.sensors)
                {
                    if (step == _inputWindowLength)
                    {
                        s0.Add(window);
                        int lastOrder = window.order;
                        window = new InputWindow(sensors.Count, _inputWindowLength);
                        window.order = lastOrder + 1;
                        step = 0;
                    }

                    window.states[sensor.ID - 1][step] = 1.0;

                    step++;
                    count++;
                    if (count == scenarios.Count)
                    {
                        s0.Add(window);
                    }
                }

            }

            foreach (var data in s0)
            {
                data.ConvolveWindow();
            }

            return true;
        }

        private bool createS1()
        {
            if (s0 == null || s0.Count <= 0)
            {
                return false;
            }

            List<InputWindow> copyOfS0 = new List<InputWindow>();
            s1 = new List<InputWindow>();
            foreach (var item in s0)
            {
                copyOfS0.Add(item);
            }

            while (copyOfS0.Count > 0)
            {
                int selected = Accord.Math.Vector.Random(1, 0, copyOfS0.Count)[0];
                s1.Add(copyOfS0[selected]);
                copyOfS0.RemoveAt(selected);
            }

            foreach (var data in s1)
            {
                data.ConvolveWindow();
            }

            return true;
        }

        private bool createRandomForest()
        {
            try
            {

                randomForest = new List<DecisionTree>();
                int lenghtOfDecisionTreeInput = 2;
                _randomForestDetails = "";
                //Debug.WriteLine($"1 for S0, 0 for S1");
                for (int j = 0; j < _countOfDecisionTreesInForest; j++)
                {
                    double[][] inputs = new double[lenghtOfDecisionTreeInput * sensors.Count][];
                    int[] output = new int[lenghtOfDecisionTreeInput * sensors.Count];
                    DecisionVariable[] variables = new DecisionVariable[_inputWindowLength];

                    for (int i = 0; i < _inputWindowLength; i++)
                    {
                        variables[i] = new DecisionVariable($"{i}", new Accord.DoubleRange(0.0, 1.0));
                    }

                    for (int i = 0; i < s0[j].convolvedStates.Length; i++)
                    {
                        inputs[i] = s0[j].convolvedStates[i];
                        output[i] = 1;
                    }

                    for (int i = 0; i < s1[j].convolvedStates.Length; i++)
                    {
                        inputs[i + s0[j].convolvedStates.Length] = s1[j].convolvedStates[i];
                        output[i + s0[j].convolvedStates.Length] = 0;
                    }

                    var c45 = new C45Learning(variables);
                    DecisionTree tree = c45.Learn(inputs, output);
                    //string a = tree.ToCode();
                    //Console.WriteLine(a);

                    //Console.WriteLine($"{j + 1}. tree in forest");
                    _randomForestDetails += $"{j + 1}. tree in forest\n";
                    _randomForestDetails += tree.Root.PrintPretty2(" ", true);
                    randomForest.Add(tree);
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        private bool showInGraph()
        {
            if (proximityMatrix == null || proximityMatrix.Count() <= 0)
            {
                MessageBox.Show("Yakınlık matrisi hesaplanmamış.\nLütfen cizelge seçip başka tuşuna basınız.");
                return false;
            }

            GraficView form = new GraficView(new List<ObservationSet>() { new ObservationSet() { Observations = proximityMatrix, Name = "test" } });
            form.ShowDialog();

            return true;
        }

        private bool cerateProximityMatrix()
        {
            if (s0 == null || s0?.Count <= 0)
            {
                MessageBox.Show("S0 verisi oluşmamış.\nLütfen cizelge seçip başka tuşuna basınız.");
                return false;
            }

            proximityMatrix = new double[s0.Count][];
            int counter = 0;
            for (int f = 0; f < randomForest.Count; f++)
            {
                for (int i = 0; i < s0.Count; i++)
                {
                    for (int t = 0; t < s1.Count; t++)
                    {
                        for (int j = 0; j < s0[i].convolvedStates.Length; j++)
                        {
                            var resultOfS0 = randomForest[f].Decide(s0[i].convolvedStates[j]);
                            var resultOfS1 = randomForest[f].Decide(s1[t].convolvedStates[j]);
                            if (resultOfS0 == resultOfS1)
                            {
                                if (proximityMatrix[i] == null)
                                {
                                    proximityMatrix[i] = new double[s1.Count];
                                }

                                proximityMatrix[i][t] = proximityMatrix[i][t] + 1;
                            }
                            counter++;
                        }
                    }
                }
            }

            return true;
        }

        #endregion Methods

        #region PropertyChagedInterfaceImplementaion
        public event PropertyChangedEventHandler PropertyChanged;

        private void propertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion PropertyChagedInterfaceImplementaion
    }
}
