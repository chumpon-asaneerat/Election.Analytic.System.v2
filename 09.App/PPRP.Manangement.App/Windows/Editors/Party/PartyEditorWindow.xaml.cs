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
    /// Interaction logic for PartyEditorWindow.xaml
    /// </summary>
    public partial class PartyEditorWindow : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public PartyEditorWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private bool _addNew = false;
        private MParty _item = null;

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
            if (null == _item) 
                return;

            var img = ImageFile.OpenFile();
            if (null == img) return;

            _item.Data = img.Data; // assign data to current item.
        }

        private void Save()
        {
            if (null != _item)
            {
                var ret = MParty.Save(_item);
                if (ret.Ok)
                {
                    var win = PPRPWindows.Windows.MessageBox;
                    win.Setup("บันทึกข้อมูลสำเร็จ", "PPRP");
                    win.ShowDialog();
                }
                else
                {
                    var win = PPRPWindows.Windows.MessageBox;
                    win.Setup("บันทึกข้อมูลไม่สำเร็จ" + Environment.NewLine + ret.ErrMsg, "PPRP");
                    win.ShowDialog();
                    return;
                }
            }
            if (_addNew)
            {
                // in add new mode so clear data and wait for new entry.
            }
            else
            {
                // in edit mode so exit window.
                DialogResult = true;
            }
        }

        private void CheckPartyName()
        {
            if (null == _item) return;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Setup.
        /// </summary>
        /// <param name="value">The edit item instance.</param>
        public void Setup(MParty value, bool addNew = false)
        {
            _item = value;
            // set callback.
            if (null != _item)
            {
                _item.WhenPartyNameChanged(CheckPartyName);
            }

            _addNew = addNew;

            DataContext = _item;
            ed.Setup(_item);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets current item.
        /// </summary>
        public MParty Item { get { return _item; } }
        /// <summary>
        /// Checks is add new mode.
        /// </summary>
        public bool IsAddNew { get { return _addNew; } }

        #endregion
    }
}
