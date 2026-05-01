using System;
using System.Collections;
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
    /// Interaction logic for DataShowWindow.xaml
    /// </summary>
    public partial class DataShowWindow : Window, INotifyPropertyChanged
    {
        private ArrayList _list;

        public DataShowWindow(ArrayList list)
        {
            InitializeComponent();
            this.DataContext = this;
            this.list = list;
        }

        public ArrayList list
        {
            get
            {
                return _list;
            }
            set
            {
                _list = value;
                onPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void onPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
