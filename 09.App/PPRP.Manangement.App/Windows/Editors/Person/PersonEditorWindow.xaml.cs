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

        private void CheckPersonName()
        {
            if (null == _item) return;
            var existItem = GetByName(_item.FirstName, _item.LastName);
            if (null != existItem)
            {
                var win = PPRPWindows.Windows.MessageBoxOKCancel;
                string msg = string.Empty;
                msg += string.Format("'{0} {1}' มีอยู่ในระบบฐานข้อมูลอยู่แล้ว", _item.FirstName, _item.LastName) + Environment.NewLine;
                msg += "ต้องการเรียกข้อมูลที่มีอยู่ขึ้นมาแก้ไขหรือไม่ ?";

                win.Setup(msg, "PPRP");
                if (win.ShowDialog() == true)
                {
                    // load exist data with same mode
                    Setup(existItem, _addNew);
                }
                else
                {
                    _item.Prefix = _item.PrefixOri; // restore back
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
                if (string.IsNullOrWhiteSpace(_item.FirstName) ||
                    string.IsNullOrWhiteSpace(_item.LastName))
                {
                    var win = PPRPWindows.Windows.MessageBox;
                    string msg = string.Empty;
                    msg += "กรุณาป้อนข้อมูล ชื่อ และนามสกุล ของ ผู้สมัคร/ว่าที่ผู้สมัคร";

                    win.Setup(msg, "PPRP");
                    win.ShowDialog();

                    return;
                }
                if (!AllowSave())
                {
                    var win = PPRPWindows.Windows.MessageBox;
                    string msg = string.Empty;
                    msg += string.Format("'{0} {1}' มีอยู่ในระบบฐานข้อมูลอยู่แล้ว", _item.FirstName, _item.LastName) + Environment.NewLine;
                    msg += "ไม่สามารถบันทึกซ้ำได้ กรุณาตรวจสอบข้อมูลอีกครั้ง";

                    win.Setup(msg, "PPRP");
                    win.ShowDialog();

                    return;
                }
                var ret = MPerson.Save(_item);
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

                if (_addNew)
                {
                    // in add new mode so clear data and wait for new entry.
                    Setup(new MPerson(), _addNew);
                }
                else
                {
                    // in edit mode so exit window.
                    DialogResult = true;
                }
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

            // set callback.
            if (null != _item)
            {
                // keep original name to detect changed.
                _item.WhenPersonNameChanged(_item.Prefix, _item.FirstName, _item.LastName, CheckPersonName);
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
        public MPerson Item { get { return _item; } }
        /// <summary>
        /// Checks is add new mode.
        /// </summary>
        public bool IsAddNew { get { return _addNew; } }

        #endregion
    }
}
