using SharedObject;
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

namespace GUI
{
    /// <summary>
    /// Interaction logic for ItemBarUserControl.xaml
    /// </summary>
    public partial class ItemBarUserControl : UserControl, INotifyPropertyChanged
    {
        #region Ctor
        public ItemBarUserControl()
        {
            InitializeComponent();
            DataContext = this;
        }

        #endregion Ctor

        #region Fields

        private string _fieldName;
        private string _actionName;
        private AdleItemBase _item;

        #endregion Fields

        #region Event Definitions
        public event PropertyChangedEventHandler PropertyChanged;
        public delegate void executeItemHandler(object sender, ExecuteItemEventArgs e);
        public event executeItemHandler executeItem;

        #endregion Event Definitions

        #region Private Methods
        private void onPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        #endregion  Private Methods

        #region Properties
        public string FieldName
        {
            get
            {
                return _fieldName;
            }

            set
            {
                _fieldName = value;
                onPropertyChanged();
            }
        }

        public string ActionName
        {
            get
            {
                return _actionName;
            }

            set
            {
                _actionName = value;
                onPropertyChanged();
            }
        }

        #endregion Properties

        #region Events
        private void btnExecuteAction_Click(object sender, RoutedEventArgs e)
        {
            if (_item == null || string.IsNullOrEmpty(ActionName)) return;

            executeItem?.Invoke(this, new ExecuteItemEventArgs(_item, ActionName));
        }

        #endregion Events

        #region Public Methods
        public void SetAction(string actionName, AdleItemBase item = null)
        {
            btnExecuteAction.Visibility = lblFieldName.Visibility = item == null ? Visibility.Collapsed : Visibility.Visible;
            _item = item;
            ActionName = actionName;

            if (item != null) FieldName = item.Name;

            double thick = item == null ? 0.0 : 1.0;
            this.BorderThickness = new Thickness(thick, thick, thick, thick);
            this.Background = item == null ? null : new SolidColorBrush(Colors.WhiteSmoke);
        }

        #endregion  Public Methods
    }

    public class ExecuteItemEventArgs : RoutedEventArgs
    {
        public ExecuteItemEventArgs(AdleItemBase item, string actionName)
        {
            Item = item;
            ActionName = actionName;
        }
        public AdleItemBase Item { get; set; }
        public string ActionName { get; set; }
    }
}
