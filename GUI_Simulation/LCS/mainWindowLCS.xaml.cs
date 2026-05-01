using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using GUI_Simulation.AnomalyExploration;
using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using AdleGraph.Interfaces;

namespace GUI_Simulation.LCS
{
    /// <summary>
    /// Interaction logic for mainWindowLCS.xaml
    /// </summary>
    public partial class mainWindowLCS : Window, INotifyPropertyChanged
    {
        #region Fields

        private int _scerioCount = 25;
        private string _totalString = "";
        private string _testResult = "";

        #endregion Fields

        #region Properties

        public List<string> bireyler { get; set; }

        public string scerioCount
        {
            get
            {
                return _scerioCount.ToString();
            }
            set
            {
                int _cache = _scerioCount;
                if (!int.TryParse(value, out _scerioCount))
                    _scerioCount = _cache;
                onPropertyChanged();
            }
        }

        public string TestResult
        {
            get
            {
                return _testResult;
            }

            set
            {
                _testResult = value;
                onPropertyChanged();
            }
        }

        #endregion Properties

        #region Ctor
        public mainWindowLCS()
        {
            InitializeComponent();
            this.DataContext = this;

            bireyler = new List<string>() { "Baba", "Anne", "Kız Çocuk", "Erkek Çocuk" };
        }

        #endregion Ctor

        #region Events
        private void btnAddNewPersonController_Click(object sender, RoutedEventArgs e)
        {
            string personSegestion = bireyler.Count > pnlControlContainer.Children.Count ? bireyler[pnlControlContainer.Children.Count] : "";
            AddPersonControl control = new AddPersonControl(pnlControlContainer.Children.Count + 1, personSegestion);
            control.Margin = new Thickness(0, 2, 0, 2);
            control.OnClose += Control_OnClose;
            control.OnMessageAdding += Control_OnMessageAdding;
            control.showContentEvent += Control_showContentEvent;
            control.test += Control_test; ;
            pnlControlContainer.Children.Add(control);
        }

        private void Control_test(object sender, testEventArgs<INode> e)
        {
            List<AddPersonControl> controlList = new List<AddPersonControl>();
            List<result> results = new List<result>();

            foreach (var item in pnlControlContainer.Children)
            {
                if (!(item is AddPersonControl))
                    continue;

                if (!((AddPersonControl)item).familyMember)
                    continue;

                controlList.Add((AddPersonControl)item);
            }

            foreach (var control in controlList)
            {
                result _result = new result();
                _result.LCS = control.testLCS(e.sequence);
                _result.subsequence = control.FindLCS(e.sequence);
                _result.name = control.person;
                results.Add(_result);
            }

            TestResult = "";
            foreach (var result in results)
            {
                string resultItem = "";
                resultItem = $"{result.name} ==>\tLCS:{result.LCS} \n\t\t subsequence:{result.subsequence}";
                TestResult += resultItem + "\n";
            }
        }

        struct result
        {
            public string name { get; set; }
            public int LCS { get; set; }
            public string subsequence { get; set; }
        }

        private void Control_showContentEvent(object sender, ShowContentEventArgs e)
        {
            FillList(e.ListToShow, ((AddPersonControl)sender).ID.ToString());
        }

        private void Control_OnMessageAdding(object sender, LogAddedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                TextBlock log = new TextBlock();
                log.Text = e.Message;
                log.TextWrapping = TextWrapping.Wrap;
                log.Margin = new Thickness(1);
                pnlLogs.Children.Insert(0, log);

                if (pnlLogs.Children.Count > 100)
                    pnlLogs.Children.RemoveAt(100);
            });
        }

        private void Control_OnClose(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                int index = pnlControlContainer.Children.IndexOf((UIElement)sender);
                pnlControlContainer.Children.RemoveAt(index);

                for (int i = 0; i < pnlControlContainer.Children.Count; i++)
                {
                    if (!(pnlControlContainer.Children[i] is AddPersonControl))
                        continue;

                    ((AddPersonControl)pnlControlContainer.Children[i]).order = i + 1;
                }
            });
        }

        private void btnRun_Click(object sender, RoutedEventArgs e)
        {
            List<AddPersonControl> controlList = new List<AddPersonControl>();

            foreach (var item in pnlControlContainer.Children)
            {
                if (!(item is AddPersonControl))
                    continue;


                if (!((AddPersonControl)item).familyMember)
                    continue;

                controlList.Add((AddPersonControl)item);
            }

            _totalString = "";
            foreach (var control in controlList)
            {
                control.Run(_scerioCount);
                _totalString += control.PersonSequenceString;
            }

            ShowData();
        }

        private void btnShowTotalSequence_Click(object sender, RoutedEventArgs e)
        {
            ShowData();
        }

        private void btnGraphSettings_Click(object sender, RoutedEventArgs e)
        {
            new MainWindowForGraph(false).ShowDialog();
        }

        #endregion Events

        #region Methdos

        private void FillList(IList list, string title = "")
        {
            if (list == null)
                return;

            ArrayList itemsList = new ArrayList();
            if (list?.Count <= 0)
            {
                itemsList.Add("Gösterilecek eleman yok.");
                new DataShowWindow(itemsList) { Title = title }.Show();
                return;
            }

            foreach (var item in list)
            {
                itemsList.Add(item.ToString());
            }

            new DataShowWindow(itemsList) { Title = title }.Show();
        }

        private void ShowData()
        {
            string data = string.IsNullOrEmpty(_totalString) ? "Gösterilecek veri yok." : _totalString;
            new StringDataShowWindow(data) { Title = "Data" }.Show();
        }


        #endregion

        #region INotifyPRopertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged;

        private void onPropertyChanged([CallerMemberName] string propertName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertName));
        }

        #endregion INotifyPRopertyChanged Implementation
    }
}
