#region Using

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
    /// Interaction logic for MPDC2566EditorWindow.xaml
    /// </summary>
    public partial class MPDC2566EditorWindow : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public MPDC2566EditorWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private bool _addNew = false;
        private MPDC _item = null;

        #endregion

        #region Button Handlers

        private void cmdChangeIamge_Click(object sender, RoutedEventArgs e)
        {
            ChangeImage();
        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            Save();
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        #endregion

        #region Private Methods

        private void ChangeImage()
        {

        }

        private void Save()
        {

        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Setup.
        /// </summary>
        /// <param name="value">The edit item instance.</param>
        public void Setup(MPDC value, bool addNew = false)
        {
            _item = value;

            // set callback.
            if (null != _item)
            {
                // keep original name to detect changed.
                //_item.WhenPartyNameChanged(_item.Prefix, _item.FirstName, _item.LastName, CheckPersonName);
            }

            _addNew = addNew;

            DataContext = _item;
            //ed.Setup(_item);
        }

        #endregion


        #region Public Properties

        /// <summary>
        /// Gets current item.
        /// </summary>
        public MPDC Item { get { return _item; } }
        /// <summary>
        /// Checks is add new mode.
        /// </summary>
        public bool IsAddNew { get { return _addNew; } }

        #endregion
    }
}
