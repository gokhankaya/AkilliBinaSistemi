using Accord.MachineLearning;
using Accord.MachineLearning.Clustering;
using Accord.Math.Random;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using ZedGraph;

namespace GUI_Simulation.AnomalyExploration
{
    /// <summary>
    /// Interaction logic for GraficView.xaml
    /// </summary>
    public partial class GraficView : Window, INotifyPropertyChanged
    {
        private double _perplexityValue = 0;

        public event PropertyChangedEventHandler PropertyChanged;

        private void onPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public List<ObservationSet> observations { get; set; }

        public string PerplexityValue
        {
            get
            {
                return _perplexityValue.ToString();
            }
            set
            {
                if (double.TryParse(value, out _perplexityValue))
                {
                    onPropertyChanged();
                }
            }
        }

        public GraficView(List<ObservationSet> observations)
        {
            InitializeComponent();
            PerplexityValue = "0.9";
            txtPerplexityValue.Text = PerplexityValue;
            this.observations = observations;
            this.Loaded += GraficView_Loaded;
        }

        private void GraficView_Loaded(object sender, RoutedEventArgs e)
        {
            generateGraficView();
        }

        private void generateGraficView()
        {
            Generator.Seed = 0;

            if (!double.TryParse(txtPerplexityValue.Text, out _perplexityValue))
            {
                MessageBox.Show("Perplexity değeri ondalık bir sayı olmalıdır.");
                return;
            }

            TSNE tSNE = new TSNE()
            {
                NumberOfOutputs = 2,
                Perplexity = _perplexityValue
            };

            var colorList = typeof(System.Drawing.Color).GetProperties(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);

            GraphPane myPane = zgc.GraphPane;
            zgc.IsShowHScrollBar = true;
            zgc.IsShowHScrollBar = true;
            myPane.CurveList.Clear();

            myPane.Title.Text = "";
            myPane.XAxis.Title.Text = "X";
            myPane.YAxis.Title.Text = "Y";
            myPane.XAxis.Scale.Max = 30;
            myPane.XAxis.Scale.Min = -5;
            myPane.YAxis.Scale.Max = 30;
            myPane.YAxis.Scale.Min = -5;
            myPane.XAxis.IsAxisSegmentVisible = true;
            myPane.YAxis.IsAxisSegmentVisible = true;
            myPane.YAxis.IsVisible = true;
            myPane.XAxis.IsVisible = true;
            myPane.Border.IsVisible = true;
            myPane.Fill = new Fill(System.Drawing.Color.WhiteSmoke);
            var gmm = new GaussianMixtureModel(2);

            int colorIndex = 22;

            foreach (var observation in observations)
            {
                double[][] output = tSNE.Transform(observation.Observations);
                PointPairList pairList = new PointPairList();

                for (int i = 0; i < output.Length; i++)
                {
                    pairList.Add(output[i][0], output[i][1]);
                }

                System.Drawing.Color selectedColor = (System.Drawing.Color)colorList[colorIndex].GetValue(null);

                LineItem myCurve = myPane.AddCurve(observation.Name, pairList, selectedColor, SymbolType.Diamond);
                myCurve.Line.IsVisible = false;
                myCurve.Symbol.Border.IsVisible = false;
                myCurve.Symbol.Fill = new Fill(selectedColor);
                colorIndex++;
            }

            zgc.AxisChange();
            zgc.Invalidate();
        }


        private void btnReflesh_Click(object sender, RoutedEventArgs e)
        {
            generateGraficView();
        }
    }
}
