#region Using

using System;
using System.Windows;
using System.Windows.Controls;
using NLib;

#endregion

namespace PPRP.Controls
{
    /// <summary>
    /// Interaction logic for PagingControl.xaml
    /// </summary>
    public partial class PagingControl : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public PagingControl()
        {
            InitializeComponent();
        }

        #endregion

        #region Button Handlers

        private void cmdFirst_Click(object sender, RoutedEventArgs e)
        {
            PageNo = 1;
            UpdateUI();
            RaisePagingChanged();
        }

        private void cmdPrev_Click(object sender, RoutedEventArgs e)
        {
            --PageNo;
            if (PageNo < 1) PageNo = 1;
            UpdateUI();
            RaisePagingChanged();
        }

        private void cmdNext_Click(object sender, RoutedEventArgs e)
        {
            ++PageNo;
            if (PageNo > MaxPage) PageNo = MaxPage;
            UpdateUI();
            RaisePagingChanged();
        }

        private void cmdLast_Click(object sender, RoutedEventArgs e)
        {
            this.PageNo = MaxPage;
            UpdateUI();
            RaisePagingChanged();
        }

        #endregion

        #region Private Methods

        private void UpdateUI()
        {
            cmdFirst.IsEnabled = (PageNo > 1);
            cmdPrev.IsEnabled = (PageNo > 1);
            cmdNext.IsEnabled = (PageNo < MaxPage);
            cmdLast.IsEnabled = (PageNo < MaxPage);
        }

        private void RaisePagingChanged()
        {
            PagingChanged.Call(this, EventArgs.Empty);
        }

        #endregion

        #region Public Methods

        public void Setup(int pageNo, int maxPage)
        {
            PageNo = pageNo;
            MaxPage = maxPage;
            if (MaxPage == 0)
            {
                txtCurrentPage.Text = string.Format("หน้าที่ {0}/{1}", 0, MaxPage);
            }
            else
            {
                txtCurrentPage.Text = string.Format("หน้าที่ {0}/{1}", PageNo, MaxPage);
            }

            UpdateUI();
        }

        #endregion

        #region Public Properties

        public int PageNo { get; private set; }
        public int MaxPage { get; private set; }

        #endregion

        #region Public Events

        public event EventHandler PagingChanged;

        #endregion
    }
}
