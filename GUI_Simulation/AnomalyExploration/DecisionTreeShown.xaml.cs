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
using System.Windows.Shapes;

namespace GUI_Simulation.AnomalyExploration
{
    /// <summary>
    /// Interaction logic for DecisionTreeShown.xaml
    /// </summary>
    public partial class DecisionTreeShown : Window, INotifyPropertyChanged
    {
        public DecisionTreeShown(string details)
        {
            InitializeComponent();
            this.DataContext = this;
            this.Details = details;
        }

        private string _details = "";
        public string Details
        {
            get
            {
                return _details;
            }
            set
            {
                _details = value; propertyChaned();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void propertyChaned([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
