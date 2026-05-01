using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace GUI_Simulation.SequencePattern.Scoring
{
    public class AccuracyInformation : INotifyPropertyChanged
    {
        public AccuracyInformation(string name = "")
        {
            _name = string.IsNullOrEmpty(name) ? "Rapor" : name;
        }

        #region Fields

        private readonly string _name;

        private int _total;

        private int _truePositive;
        private int _falsePositive;
        private int _falseNegative;

        private double _accuracy;
        private double _error;

        private double _precision;
        private double _recall;

        private double _fscore;

        #endregion Fields

        #region Properties

        public double Accuracy
        {
            get
            {
                return _accuracy;
            }

            set
            {
                _accuracy = value;
                onproertyChanged();
            }
        }

        public double Error
        {
            get
            {
                return _error;
            }

            set
            {
                _error = value;
                onproertyChanged();
            }
        }

        public double Precision
        {
            get
            {
                return _precision;
            }

            set
            {
                _precision = value;
                onproertyChanged();
            }
        }

        public double Recall
        {
            get
            {
                return _recall;
            }

            set
            {
                _recall = value;
                onproertyChanged();
            }
        }

        public double Fscore
        {
            get
            {
                return _fscore;
            }

            set
            {
                _fscore = value;
                onproertyChanged();
            }
        }

        public int TruePositive
        {
            get
            {
                return _truePositive;
            }

            private set
            {
                _truePositive = value;
                onproertyChanged();
            }
        }

        public int FalsePositive
        {
            get
            {
                return _falsePositive;
            }

            private set
            {
                _falsePositive = value;
                onproertyChanged();
            }
        }

        public int FalseNegative
        {
            get
            {
                return _falseNegative;
            }

            private set
            {
                _falseNegative = value;
                onproertyChanged();
            }
        }

        public int Total
        {
            get
            {
                return _total;
            }

            set
            {
                _total = value;
                onproertyChanged();
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }

        }

        #endregion Properties

        #region Methods

        public void IncreaseTruePositive()
        {
            TruePositive++;
            IncreaseTotal();
            CalculateAccuracyPrecisionRecallAndFscore();
        }

        public void IncreaseFalsePositive()
        {
            FalsePositive++;
            IncreaseTotal();
            CalculateAccuracyPrecisionRecallAndFscore();
        }

        public void IncreaseFalseNegative()
        {
            FalseNegative++;
            IncreaseTotal();
            CalculateAccuracyPrecisionRecallAndFscore();
        }

        public void IncreaseTotal()
        {
            Total++;
        }

        public void CalculateAccuracyPrecisionRecallAndFscore(int truePositive = -1, int falsePositive = -1, int falseNegative = -1, int total = -1)
        {
            int TP = truePositive >= 0 ? truePositive : TruePositive;
            int FP = falsePositive >= 0 ? falsePositive : FalsePositive;
            int FN = falseNegative >= 0 ? falseNegative : FalseNegative;
            int N = total >= 0 ? total : Total;

            Accuracy = (double)(TP + FP) / N;
            Accuracy = SetValueToZeroIfValueIsNaNorInfinity(Accuracy);
            Error = 1 - Accuracy;

            Precision = (double)TP / (TP + FP);
            Precision = SetValueToZeroIfValueIsNaNorInfinity(Precision);

            Recall = (double)TP / (TP + FN);
            Recall = SetValueToZeroIfValueIsNaNorInfinity(Recall);

            Fscore = 2 * (Precision * Recall / (Precision + Recall));
            Fscore = SetValueToZeroIfValueIsNaNorInfinity(Fscore);

            dataChanged?.Invoke(this, new DataEventArgs() { Accuracy = Accuracy, Fscore = Fscore, Recall = Recall, Precision = Precision });
        }

        private double SetValueToZeroIfValueIsNaNorInfinity(double value)
        {
            double checkedValue = double.IsNaN(value) || double.IsInfinity(value) ? 0.0 : value;
            return checkedValue;
        }


        #endregion Methods

        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;

        private void onproertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion INotifyPropertyChanged Implementation

        #region Event Definitons
        public delegate void dataChangedHandler(object sender, DataEventArgs e);
        public event dataChangedHandler dataChanged;

        #endregion Event Definitons
    }

    public class DataEventArgs : RoutedEventArgs
    {
        public double Accuracy { get; set; }

        public double Precision { get; set; }

        public double Recall { get; set; }

        public double Fscore { get; set; }

    }
}
