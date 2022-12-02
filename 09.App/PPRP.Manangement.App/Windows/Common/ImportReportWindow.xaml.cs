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
using System.Windows.Shapes;

using PPRP.Models;

#endregion

namespace PPRP.Windows
{
    /// <summary>
    /// Interaction logic for ImportReportWindow.xaml
    /// </summary>
    public partial class ImportReportWindow : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public ImportReportWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Button Handlers

        private void cmdCopy_Click(object sender, RoutedEventArgs e)
        {
            CopyToClipboard();
        }

        private void cmdOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        #endregion

        public void CopyToClipboard()
        {
            List<ImportError> errors = lvItems.ItemsSource as List<ImportError>;
            if (null == errors)
                return;
            StringBuilder sb = new StringBuilder();
            foreach (var item in errors)
            {
                if (null != item)
                {
                    sb.Append(item.RowNo.ToString() + "\t");
                    sb.Append(item.ErrMsg + "\t");
                    string sVal = item.DataString.Replace(Environment.NewLine, ";");
                    sb.Append(sVal + "\t");
                }
                sb.AppendLine();
            }
            Clipboard.SetDataObject(sb.ToString());

        }

        #region Public Methods

        /// <summary>
        /// Setup.
        /// </summary>
        /// <param name="errors">The error results.</param>
        public void Setup(List<ImportError> errors)
        {
            lvItems.ItemsSource = errors;
            if (null != errors)
            {
                txtTotalCount.Text = string.Format("{0:n0}", errors.Count);
            }
            else txtTotalCount.Text = "0";
        }

        #endregion
    }
}
