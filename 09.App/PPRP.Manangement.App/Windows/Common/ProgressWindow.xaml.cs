#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

#endregion

namespace PPRP.Windows
{
    /// <summary>
    /// Interaction logic for ProgressWindow.xaml
    /// </summary>
    public partial class ProgressWindow : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public ProgressWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Private Methods

        private void UpdateInfo()
        {
            txtInfo.Text = string.Format("นำเข้าข้อมูล รายการที่ {0:n0} จาก {1:n0} รายการ", progress.Value, progress.Maximum);
        }

        #endregion

        #region Public Methods

        public void Setup(int max, string title = "ดำเนินการนำเข้าข้อมูล")
        {
            txtTitle.Text = title;

            progress.Minimum = 0;
            progress.Maximum = max;
            progress.Value = 0;

            UpdateInfo();
        }

        public void Increment()
        {
            Dispatcher.Invoke(() => 
            {
                progress.Value++;
                UpdateInfo();
            }, DispatcherPriority.Background);
        }

        #endregion
    }
}
