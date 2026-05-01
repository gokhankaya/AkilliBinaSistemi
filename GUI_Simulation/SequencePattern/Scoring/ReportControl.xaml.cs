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
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZedGraph;

namespace GUI_Simulation.SequencePattern.Scoring
{
    /// <summary>
    /// Interaction logic for ReportControl.xaml
    /// </summary>
    public partial class ReportControl : UserControl, INotifyPropertyChanged
    {
        public ReportControl(AccuracyInformation information)
        {
            InitializeComponent();
            DataContext = this;
            this.information = information;
            this.information.dataChanged += Information_dataChanged;
            ReportStartingTime = DateTime.Now;
            setSummary();

            _accuracyObservations = new Dictionary<int, double>();
            _recallObservations = new Dictionary<int, double>();
            _precisionObservations = new Dictionary<int, double>();
            _fscoreObservations = new Dictionary<int, double>();

            InitGraph();
        }

        #region Fields
        private DateTime _reportStartingTime;
        private int _trainingSetCount;
        private int _newAddedDataCount;
        private int _testCount;
        private string _summaryValue = "";
        private Dictionary<int, double> _accuracyObservations;
        private Dictionary<int, double> _recallObservations;
        private Dictionary<int, double> _precisionObservations;
        private Dictionary<int, double> _fscoreObservations;

        #endregion Fields

        #region Events
        private void Information_dataChanged(object sender, DataEventArgs e)
        {
            setSummary();
            int index = _accuracyObservations.Values.Count;
            _accuracyObservations.Add(index, e.Accuracy);
            _recallObservations.Add(index, e.Recall);
            _precisionObservations.Add(index, e.Precision);
            _fscoreObservations.Add(index, e.Fscore);
            RefleshGraphic();
        }

        #endregion Events

        #region Properties
        public AccuracyInformation information { get; set; }

        public DateTime ReportStartingTime
        {
            get { return _reportStartingTime; }
            set
            {
                _reportStartingTime = value;
                onPropertyCahnged();
                onPropertyCahnged("TimeValue");
            }
        }

        public string TimeValue
        {
            get { return $"{ReportStartingTime.ToShortDateString()} {ReportStartingTime.ToShortTimeString()}"; }
        }

        public string Summary
        {
            get
            {
                if (information == null)
                    return "Özet bilgisi bulunamadı";

                return _summaryValue;
            }
        }

        public int TrainingSetCount
        {
            get
            {
                return _trainingSetCount;
            }

            set
            {
                _trainingSetCount = value;
                onPropertyCahnged();
                onPropertyCahnged("TotalCount");
            }
        }

        public int NewAddedDataCount
        {
            get
            {
                return _newAddedDataCount;
            }

            set
            {
                _newAddedDataCount = value;
                onPropertyCahnged();
                onPropertyCahnged("TotalCount");
                setSummary();
            }
        }

        public int TestCount
        {
            get
            {
                return _testCount;
            }

            set
            {
                _testCount = value;
                onPropertyCahnged();
            }
        }

        public int TotalCount
        {
            get { return TrainingSetCount + NewAddedDataCount; }
        }


        #endregion Properties

        #region Public Methods

        public void RemoveLastPointsFromGraphic()
        {
            int index = _accuracyObservations.Count;
            _accuracyObservations.Remove(index - 1);
            _recallObservations.Remove(index - 1);
            _precisionObservations.Remove(index - 1);
            _fscoreObservations.Remove(index - 1);

            index--;
            if (index >= 1)
            {
                information.Accuracy = _accuracyObservations[index - 1];
                information.Precision = _precisionObservations[index - 1];
                information.Recall = _recallObservations[index - 1];
                information.Fscore = _fscoreObservations[index - 1];

                information.CalculateAccuracyPrecisionRecallAndFscore();

            }


            RefleshGraphic();
            setSummary();
        }

        #endregion Public Methods

        #region Private Methods

        private void setSummary()
        {
            string accuracy = string.Format("{0:0.00}", information.Accuracy);
            string fscore = string.Format("{0:0.00}", information.Fscore);

            _summaryValue = $"{TimeValue}' de  başlayan simülasyonda, {TrainingSetCount} adet eğitim verisine {NewAddedDataCount} adet yeni veri eklenerek {TestCount} adet deneme yapılmıştır.\n{accuracy} doğruluk oranıyla ve {fscore} doğruluk ortalamasıyla tahminler yapılmıştır.";
            onPropertyCahnged("Summary");
        }

        private void RefleshGraphic()
        {
            GraphPane pane = zgc.GraphPane;
            pane.CurveList.Clear();
            AddLine(ref pane, _accuracyObservations, System.Drawing.Color.Green, "Doğruluk Oranı");
            AddLine(ref pane, _recallObservations, System.Drawing.Color.Blue, "Hasasiyet");
            AddLine(ref pane, _precisionObservations, System.Drawing.Color.BlueViolet, "Kesinlik");
            AddLine(ref pane, _fscoreObservations, System.Drawing.Color.Red, "Ortalama");

            zgc.AxisChange();
            zgc.Invalidate();
        }

        private void AddLine(ref GraphPane pane, Dictionary<int, double> obsevations, System.Drawing.Color color, string name)
        {
            PointPairList pairList = new PointPairList();
            foreach (var observation in obsevations)
            {
                pairList.Add(observation.Key, observation.Value);
            }

            System.Drawing.Color selectedColor = color;


            LineItem accuracyCurve = pane.AddCurve(name, pairList, selectedColor, SymbolType.Default);
            accuracyCurve.Line.IsVisible = true;
            accuracyCurve.Symbol.Border.IsVisible = true;
            accuracyCurve.Symbol.Fill = new Fill(selectedColor);
        }

        private GraphPane InitGraph()
        {
            GraphPane pane = zgc.GraphPane;
            zgc.IsShowVScrollBar = true;
            zgc.IsShowHScrollBar = true;

            pane.Title.Text = "";
            pane.XAxis.Title.Text = "Deneme Sayısı";
            pane.YAxis.Title.Text = "Sonuç";
            pane.XAxis.Scale.Max = 45;
            pane.XAxis.Scale.Min = -1;
            pane.YAxis.Scale.Max = 1.3;
            pane.YAxis.Scale.Min = -0.3;
            pane.XAxis.IsAxisSegmentVisible = true;
            pane.YAxis.IsAxisSegmentVisible = true;
            pane.YAxis.IsVisible = true;
            pane.XAxis.IsVisible = true;
            pane.Border.IsVisible = true;
            pane.Fill = new Fill(System.Drawing.Color.WhiteSmoke);
            return pane;
        }

        #endregion Private Methods

        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;

        private void onPropertyCahnged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion INotifyPropertyChanged Implementation

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("Accuracy\n");

            foreach (var item in _accuracyObservations)
            {
                builder.Append($"{item.Key}\t{item.Value}\n");
            }

            builder.Append("\nRecall\n");

            foreach (var item in _recallObservations)
            {
                builder.Append($"{item.Key}\t{item.Value}\n");
            }

            builder.Append("\nPrecision\n");

            foreach (var item in _precisionObservations)
            {
                builder.Append($"{item.Key}\t{item.Value}\n");
            }
            builder.Append("\nF1Score\n");

            foreach (var item in _fscoreObservations)
            {
                builder.Append($"{item.Key}\t{item.Value}\n");
            }

            string path = $@"C:\ADLE_Observations\{Name}_{DateTime.Now.ToShortDateString()}_{DateTime.Now.ToShortTimeString().Replace(':', '-')}_{Guid.NewGuid().ToString()}.txt";
            System.IO.File.WriteAllText(path, builder.ToString());
            Process.Start(path);
        }
    }
}
