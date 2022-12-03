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
    /// Interaction logic for PersonEditorWindow.xaml
    /// </summary>
    public partial class PersonEditorWindow : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public PersonEditorWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private bool _addNew = false;
        private MPerson _item = null;

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
        public void Setup(MPerson value, bool addNew = false)
        {
            _item = value;
            _addNew = addNew;
            if (null != _item)
            {

            }
            DataContext = _item;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets current item.
        /// </summary>
        public MPerson Item { get { return _item; } }
        /// <summary>
        /// Checks is add new mode.
        /// </summary>
        public bool IsAddNew { get { return _addNew; } }

        #endregion
    }
}
