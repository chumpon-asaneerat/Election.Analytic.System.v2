#region Using

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Documents;
using System.Reflection;

using NLib;
using NLib.Services;

using PPRP.Models;

#endregion

namespace PPRP.Controls
{
    /// <summary>
    /// Interaction logic for ThailandMenuControl.xaml
    /// </summary>
    public partial class ThailandMenuControl : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public ThailandMenuControl()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private Action<AreaMenuItem> _ClickCallBack;

        #endregion

        #region Button Handlers

        private void cmdPak_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as Hyperlink).DataContext as AreaMenuItem;
            if (null != _ClickCallBack) _ClickCallBack(item);
        }

        private void cmdProvince_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as Hyperlink).DataContext as AreaMenuItem;
            if (null != _ClickCallBack) _ClickCallBack(item);
        }

        #endregion

        #region Public Methods

        public void Setup(List<AreaMenuItem> items, Action<AreaMenuItem> clickCallBack)
        {
            this.DataContext = items;
            _ClickCallBack = clickCallBack;
        }

        #endregion
    }
}
