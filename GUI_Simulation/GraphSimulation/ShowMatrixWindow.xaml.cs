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

namespace GUI_Simulation.GraphSimulation
{
    /// <summary>
    /// Interaction logic for ShowMatrixWindow.xaml
    /// </summary>
    public partial class ShowMatrixWindow : Window, INotifyPropertyChanged
    {
        #region Constrator
        public ShowMatrixWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }
        #endregion Constrator

        #region Public Methods
        public void Load(string matris)
        {
            Data = matris;
        }

        #endregion Public Methods

        #region Fields
        private string _data;

        #endregion Fields

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
                propertyChanged();
            }
        }

        #endregion Properties

        #region INotifyPropertyChanged Implementataion
        public event PropertyChangedEventHandler PropertyChanged;
        private void propertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion INotifyPropertyChanged Implementataion
    }
}
