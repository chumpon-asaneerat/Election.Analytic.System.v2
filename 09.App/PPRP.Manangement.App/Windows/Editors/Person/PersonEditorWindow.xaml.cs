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

        private MPerson GetByName(string firstName, string lastName)
        {
            return MPerson.Get(firstName, lastName).Value();
        }

        private void CheckPartyName()
        {
            if (null == _item) return;
            var existItem = GetByName(_item.FirstName, _item.LastName);
            if (null != existItem)
            {
                var win = PPRPWindows.Windows.MessageBoxOKCancel;
                string msg = string.Empty;
                msg += string.Format("'{0}' มีอยู่ในระบบฐานข้อมูลอยู่แล้ว", _item.FullName) + Environment.NewLine;
                msg += "ต้องการเรียกข้อมูลที่มีอยู่ขึ้นมาแก้ไขหรือไม่ ?";

                win.Setup(msg, "PPRP");
                if (win.ShowDialog() == true)
                {
                    // load exist data with same mode
                    Setup(existItem, _addNew);
                }
                else
                {
                    _item.FirstName = _item.FirstNameOri; // restore back
                    _item.LastName = _item.LastNameOri; // restore back
                }
            }
        }

        private bool AllowSave()
        {
            bool ret = false;
            if (null != _item)
            {
                var existItem = GetByName(_item.FirstName, _item.LastName);
                if (null != existItem && existItem.PersonId != _item.PersonId)
                    ret = false;
                else ret = true;
            }
            return ret;
        }

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
                Console.WriteLine("Save Item.");
            }
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

            DataContext = _item;
            ed.Setup(_item);
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
