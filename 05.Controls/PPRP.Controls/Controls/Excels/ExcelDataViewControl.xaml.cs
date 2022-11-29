#region Using

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;

using NLib;
using NLib.Reflection;
using NLib.Services;
using PPRP.Models;

#endregion

namespace PPRP.Controls.Excels
{
    /// <summary>
    /// Interaction logic for ExcelDataViewControl.xaml
    /// </summary>
    public partial class ExcelDataViewControl : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public ExcelDataViewControl()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private NExcelWorksheet _sheet = null;

        #endregion

        #region Loaded/Unloaded

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region Public Methods

        public void Setup<T>(NExcelWorksheet sheet)
            where T: class, new()
        {
            _sheet = sheet;
            LoadItems<T>();
        }

        private void LoadItems<T>()
            where T : class, new()
        {
            var items = _sheet.LoadItems<T>();
            var maps = _sheet.Mappings;

            if (null == maps || maps.Count <= 0)
            {
                // Clear exist data
                this.lvMapPreview.ItemsSource = null;
                // Clear exist columns
                this.lvMapGridView.Columns.Clear();
                return;
            }

            // Do work while the dispatcher processing is disabled.
            using (Dispatcher.DisableProcessing())
            {
                // Clear exist data
                this.lvMapPreview.ItemsSource = null;
                // Clear exist columns
                this.lvMapGridView.Columns.Clear();

                // Build new columns
                GridViewColumn col;
                foreach (var map in maps)
                {
                    col = new GridViewColumn();
                    col.Header = map.DisplayText;
                    col.Width = 120;
                    col.DisplayMemberBinding = new Binding(map.PropertyName);

                    this.lvMapGridView.Columns.Add(col);
                }

                // set new ItemsSource
                this.lvMapPreview.ItemsSource = items;
            }
        }

        #endregion
    }
}
