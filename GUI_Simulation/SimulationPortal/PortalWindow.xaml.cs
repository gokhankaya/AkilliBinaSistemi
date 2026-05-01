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

namespace GUI_Simulation.SimulationPortal
{
    /// <summary>
    /// Interaction logic for PortalWindow.xaml
    /// </summary>
    public partial class PortalWindow : Window
    {
        public PortalWindow()
        {
            InitializeComponent();
            Closed += PortalWindow_Closed;
        }

        #region Events
        private void Window_Closed(object sender, EventArgs e)
        {
            this.Visibility = Visibility.Visible;
        }

        private void PortalWindow_Closed(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void btnSequencePattern_Click(object sender, RoutedEventArgs e)
        {
            showWindow(new SequencePattern.MainWindowSequentialPattern());
        }

        private void btnRulesBased_Click(object sender, RoutedEventArgs e)
        {
            showWindow(new MainWindowForRules());
        }

        private void btnGraphs_Click(object sender, RoutedEventArgs e)
        {
            showWindow(new MainWindowForGraph());
        }

        private void btnAnomality1_Click(object sender, RoutedEventArgs e)
        {
            showWindow(new AnomalyExploration.MainWindow());
        }

        private void btnAnomality2_Click(object sender, RoutedEventArgs e)
        {
            showWindow(new AnomalyExploration.MainWindowWithMultipleGraphs());
        }

        #endregion Events
        #region Methods
        private void showWindow(Window window)
        {
            this.Visibility = Visibility.Hidden;
            window.Show();
            window.Closed += Window_Closed;
        }

        #endregion Methods

        private void btnSequencePatternLCS_Click(object sender, RoutedEventArgs e)
        {
            showWindow(new LCS.mainWindowLCS());
        }
    }
}
