#region Using

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
using System.Windows.Navigation;
using System.Windows.Shapes;

using NLib;
using NLib.Services;

using PPRP.Models;

#endregion

namespace PPRP.Controls.Elements
{
    /// <summary>
    /// Interaction logic for EducationIdFieldControl.xaml
    /// </summary>
    public partial class EducationIdFieldControl : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public EducationIdFieldControl()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private MPerson _item = null;

        #endregion

        #region Loaded/Unloaded

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            _item = null;
        }

        #endregion

        #region Private Methods

        private void LoadComboBox()
        {
           cb.ItemsSource = MEducation.Gets().Value();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Setup.
        /// </summary>
        /// <param name="value">The edit item instance.</param>
        public void Setup(MPerson value)
        {
            LoadComboBox();

            _item = value;
            if (null != _item)
            {

            }
            DataContext = _item;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets Current Item.
        /// </summary>
        public MPerson Item { get { return _item; } }

        #endregion
    }
}
