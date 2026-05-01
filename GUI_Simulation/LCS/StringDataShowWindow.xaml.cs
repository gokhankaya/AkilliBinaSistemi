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

namespace GUI_Simulation.LCS
{
    /// <summary>
    /// Interaction logic for StringDataShowWindow.xaml
    /// </summary>
    public partial class StringDataShowWindow : Window, INotifyPropertyChanged
    {
        #region Fileds
        private string _data;

        #endregion Fileds

        #region Ctor
        public StringDataShowWindow(string data)
        {
            InitializeComponent();
            this.DataContext = this;
            Data = data;
        }

        #endregion Ctor

        #region Properties
        public string Data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
            }
        }
        #endregion Properties

        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;
        private void onpropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion INotifyPropertyChanged Implementation
    }
}
