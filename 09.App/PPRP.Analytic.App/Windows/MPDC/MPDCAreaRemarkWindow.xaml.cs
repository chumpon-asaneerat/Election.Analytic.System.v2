﻿#region Using

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Reflection;

using NLib;
using NLib.Services;

using PPRP.Models;

#endregion

namespace PPRP.Windows
{
    /// <summary>
    /// Interaction logic for MPDCAreaRemarkWindow.xaml
    /// </summary>
    public partial class MPDCAreaRemarkWindow : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public MPDCAreaRemarkWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private PollingUnit _item = null;

        #endregion

        #region Public Methods

        public void Setup(PollingUnit value)
        {
            _item = value;
            this.DataContext = _item;
            if (null == _item)
            {
                txtTitle.Text = string.Format("ข้อมูลพื้นที่ {0} เขต {1}", "-", "-");
            }
            else
            {
                txtTitle.Text = string.Format("ข้อมูลพื้นที่ {0} เขต {1}", _item.ProvinceNameTH, _item.PollingUnitNo);
            }
            this.Title = txtTitle.Text;
        }

        #endregion
    }
}
