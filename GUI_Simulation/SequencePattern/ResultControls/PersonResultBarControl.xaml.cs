using AdleGraph.Interfaces;
using SequentialPattern;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace GUI_Simulation.SequencePattern
{
    /// <summary>
    /// Interaction logic for PersonResultBarControl.xaml
    /// </summary>
    public partial class PersonResultBarControl : UserControl, INotifyPropertyChanged
    {
        public PersonResultBarControl(int order, int sequenceOrder, Sequence<INode> sequence, string person, string exceptedPerson)
        {
            InitializeComponent();

            DataContext = this;

            Order = order;
            SequenceOrder = sequenceOrder;
            _sequence = sequence;
            SequenceString = _sequence.ToString();
            SequenceLength = _sequence.Length;
            PersonValue = person;
            ExpectedPersonValue = exceptedPerson;

            SolidColorBrush brus = new SolidColorBrush(person == exceptedPerson ? Colors.LightGreen : Colors.LightPink);
            ChangeBackgroud(brus);
        }

        #region Fields

        private int _order;

        private int _sequenceOrder;

        private string _sequenceString;

        private int _sequenceLength;

        private string _expectedPersonValue;

        private string _personValue;

        private Sequence<INode> _sequence;

        #endregion Fields

        #region Properties

        public int Order
        {
            get
            {
                return _order;
            }

            set
            {
                _order = value;
                onpropertyChanged();
            }
        }

        public int SequenceOrder
        {
            get
            {
                return _sequenceOrder;
            }

            set
            {
                _sequenceOrder = value;
                onpropertyChanged();
            }
        }

        public string SequenceString
        {
            get
            {
                return _sequenceString;
            }

            set
            {
                _sequenceString = value;
                onpropertyChanged();
            }
        }

        public string ExpectedPersonValue
        {
            get
            {
                return _expectedPersonValue;
            }

            set
            {
                _expectedPersonValue = value;
                onpropertyChanged();
            }
        }

        public string PersonValue
        {
            get
            {
                return _personValue;
            }

            set
            {
                _personValue = value;
                onpropertyChanged();
            }
        }

        public int SequenceLength
        {
            get
            {
                return _sequenceLength;
            }

            set
            {
                _sequenceLength = value;
                onpropertyChanged();
            }
        }


        #endregion Properties

        #region Methods

        public void ChangeBackgroud(SolidColorBrush brush = null)
        {
            SolidColorBrush color = (brush == null) ? new SolidColorBrush(Colors.WhiteSmoke) : brush;
            this.Background = color;
        }

        #endregion Methods

        #region INotfyPropertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged;

        private void onpropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
