﻿#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

using NLib;
using NLib.Reflection;
using NLib.Services;

using PPRP.Models;

#endregion

namespace PPRP.Windows
{
    /// <summary>
    /// Interaction logic for ImportPersonImageWindow.xaml
    /// </summary>
    public partial class ImportPersonImageWindow : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public ImportPersonImageWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private int iPageNo = 1;
        private int iMaxPage = 1;
        private int iRowsPerPage = 40;

        private ImageFileSource source = null;

        #endregion

        #region Button Handlers

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void cmdFinish_Click(object sender, RoutedEventArgs e)
        {
            if (Import())
            {
                DialogResult = true;
            }
        }

        private void cmdChooseFolder_Click(object sender, RoutedEventArgs e)
        {
            ChooseImageFolder();
        }

        #endregion

        #region Paging Handlers

        private void nav_PagingChanged(object sender, EventArgs e)
        {
            iPageNo = nav.PageNo;
            RefreshList();
        }

        #endregion

        #region Private Methods

        private bool Import()
        {

            return true;
        }

        private void ChooseImageFolder()
        {
            source = new ImageFileSource();
            var ret = source.OpenFolder(this);
            if (ret)
            {
                txtFolderName.Text = source.ImagePath;
            }
            else txtFolderName.Text = string.Empty;

            RefreshList();
        }

        private void RefreshList()
        {
            lvFiles.ItemsSource = null;
            if (null == source)
            {
                // no source files.
                iPageNo = 1;
                iMaxPage = 1;

                nav.Setup(iPageNo, iMaxPage);

                return;
            }

            source.LoadItems(iPageNo, iRowsPerPage);
            var items = source.Items;
            lvFiles.ItemsSource = (null != items) ? items : new List<ImageFile>();

            if (null != items)
            {
                lvFiles.SelectedIndex = 0;
                lvFiles.ScrollIntoView(lvFiles.SelectedItem);
            }

            iPageNo = (null != items) ? source.PageNo : 1;
            iMaxPage = (null != items) ? source.MaxPages : 1;

            nav.Setup(iPageNo, iMaxPage);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Setup
        /// </summary>
        public void Setup()
        {
            iPageNo = 1;
            iMaxPage = 1;

            RefreshList();
        }

        #endregion
    }
}
