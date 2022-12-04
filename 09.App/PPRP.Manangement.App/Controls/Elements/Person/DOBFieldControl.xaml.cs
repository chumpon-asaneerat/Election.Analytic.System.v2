#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Markup;
using System.Windows.Navigation;
using System.Windows.Shapes;

using NLib;
using NLib.Services;

using PPRP.Models;

#endregion

namespace PPRP.Controls.Elements
{
    /// <summary>
    /// Interaction logic for DOBFieldControl.xaml
    /// </summary>
    public partial class DOBFieldControl : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public DOBFieldControl()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        //private CultureInfo culture = new CultureInfo("th-TH") { DateTimeFormat = { Calendar = new ThaiBuddhistCalendar() } };
        private CultureInfo culture = new CultureInfo("th-TH");
        private XmlLanguage language = XmlLanguage.GetLanguage("th-TH");

        private MPerson _item = null;

        #endregion

        #region Loaded/Unloaded

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            dtDOB.CultureInfo = culture;
            dtDOB.Language = language;
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            _item = null;
        }

        #endregion

        #region DateTimePicker ValueChanged Handler 

        private void dtDOB_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (null != _item)
            {
                _item.DOB = dtDOB.Value;
            }
        }

        #endregion

        #region Private Methods

        private void Reset()
        {
            if (null != _item)
            {
                dtDOB.Value = _item.DOB;
            }
            else
            {
                dtDOB.Value = new DateTime?();
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Setup.
        /// </summary>
        /// <param name="value">The edit item instance.</param>
        public void Setup(MPerson value)
        {
            _item = value;
            if (null != _item)
            {
                Reset();
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
