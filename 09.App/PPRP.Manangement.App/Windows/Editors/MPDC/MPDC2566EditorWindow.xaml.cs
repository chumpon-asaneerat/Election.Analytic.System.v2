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
        private MPerson _person = null;
        private MParty _defaultParty = new MParty();

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

        #region ComboBox Handlers

        private void cbParties_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbParties.SelectedItem != null && cbParties.SelectedItem is MPartyName)
            {
                var party = cbParties.SelectedItem as MPartyName;
                UpdateParty(party);
            }
        }

        #endregion

        #region Private Methods

        private void LoadProvinces()
        {
            cbProvince.ItemsSource = null;
            var provinces = MProvince.Gets().Value();
            cbProvince.ItemsSource = (null != provinces) ? provinces : new List<MProvince>();
            if (null != provinces)
            {
                cbProvince.SelectedIndex = 0;
            }
        }

        private void LoadParties()
        {
            cbParties.ItemsSource = null;
            var parties = MPartyName.Gets().Value();
            cbParties.ItemsSource = (null != parties) ? parties : new List<MPartyName>();
            if (null != parties)
            {
                parties.Insert(0, new MPartyName { PartyId = new int?(), PartyName = "ไม่มี" });
            }
            if (null != parties)
            {
                cbParties.SelectedIndex = 0;
            }
        }

        private void UpdateParty(MPartyName partyItem)
        {
            if (null != partyItem)
            {
                var party = MParty.Get(partyItem.PartyName).Value();
                if (null != party)
                {
                    imgParty.DataContext = party;
                }
                else
                {
                    imgParty.DataContext = _defaultParty;
                }
            }
            else imgParty.DataContext = _defaultParty;
        }

        private MPerson GetByName(string firstName, string lastName)
        {
            return MPerson.Get(firstName, lastName).Value();
        }

        private void CheckPersonName()
        {
            if (null == _person) return;
            var existItem = GetByName(_person.FirstName, _person.LastName);
            if (null != existItem)
            {
                var win = PPRPWindows.Windows.MessageBoxOKCancel;
                string msg = string.Empty;
                msg += string.Format("'{0} {1}' มีอยู่ในระบบฐานข้อมูลอยู่แล้ว", _person.FirstName, _person.LastName) + Environment.NewLine;
                msg += "ต้องการเรียกข้อมูลที่มีอยู่ขึ้นมาแก้ไขหรือไม่ ?";

                win.Setup(msg, "PPRP");
                if (win.ShowDialog() == true)
                {
                    // load exist data with same mode
                    SetupPerson(existItem, _addNew);
                }
                else
                {
                    _person.Prefix = _person.PrefixOri; // restore back
                    _person.FirstName = _person.FirstNameOri; // restore back
                    _person.LastName = _person.LastNameOri; // restore back
                }
            }
        }

        public void SetupPerson(MPerson value, bool addNew = false)
        {
            _person = value;

            // set callback.
            if (null != _person)
            {
                // keep original name to detect changed.
                _person.WhenPersonNameChanged(_person.Prefix, _person.FirstName, _person.LastName, CheckPersonName);
            }

            _addNew = addNew;

            personEditor.Setup(_person);
        }

        private void ChangeImage()
        {
            if (null == _person)
                return;

            var img = ImageFile.OpenFile();
            if (null == img) return;

            _person.Data = img.Data; // assign data to current item.
        }

        private void Save()
        {
            NDbResult ret;
            
            if (null != _person)
            {
                #region Save Person

                ret = MPerson.Save(_person);
                if (ret.Failed)
                {
                    var mbox = PPRPApp.Windows.MessageBox;
                    mbox.Owner = this;
                    string msg = string.Empty;
                    msg += "ไม่สามารถบันทึกข้อมูลส่วนตัวว่าที่ผู้สมัครปี 2566 ได้" + Environment.NewLine;
                    msg += ret.ErrMsg;
                    mbox.Setup(msg, "PPRP");
                    mbox.ShowDialog();
                    return;
                }

                #endregion

                #region Check Polling Unit No

                string sPollingUnitNo = txtPollingUnitNo.Text;
                if (string.IsNullOrWhiteSpace(sPollingUnitNo))
                {
                    var mbox = PPRPApp.Windows.MessageBox;
                    mbox.Owner = this;
                    string msg = string.Empty;
                    msg += "กรุณาระบุหมายเลขเขต";
                    mbox.Setup(msg, "PPRP");
                    mbox.ShowDialog();

                    txtPollingUnitNo.SelectAll();
                    txtPollingUnitNo.Focus();

                    return;
                }
                else
                {
                    int iNo;
                    if (!int.TryParse(sPollingUnitNo, out iNo))
                    {
                        var mbox = PPRPApp.Windows.MessageBox;
                        mbox.Owner = this;
                        string msg = string.Empty;
                        msg += "หมายเลขเขตต้องเป็นตัวเลขเท่านั้น";
                        mbox.Setup(msg, "PPRP");
                        mbox.ShowDialog();

                        txtPollingUnitNo.SelectAll();
                        txtPollingUnitNo.Focus();

                        return;
                    }
                    else
                    {
                        if (iNo <= 0)
                        {
                            var mbox = PPRPApp.Windows.MessageBox;
                            mbox.Owner = this;
                            string msg = string.Empty;
                            msg += "หมายเลขเขตต้องมากกว่า 0";
                            mbox.Setup(msg, "PPRP");
                            mbox.ShowDialog();

                            txtPollingUnitNo.SelectAll();
                            txtPollingUnitNo.Focus();

                            return;
                        }
                    }
                }

                #endregion

                #region Check Candidate No

                string sCandidateNo = txtCandidateNo.Text;
                if (string.IsNullOrWhiteSpace(sCandidateNo))
                {
                    var mbox = PPRPApp.Windows.MessageBox;
                    mbox.Owner = this;
                    string msg = string.Empty;
                    msg += "กรุณาระบุลำดับที่ผู้สมัคร";
                    mbox.Setup(msg, "PPRP");
                    mbox.ShowDialog();

                    txtCandidateNo.SelectAll();
                    txtCandidateNo.Focus();

                    return;
                }
                else
                {
                    int iNo;
                    if (!int.TryParse(sCandidateNo, out iNo))
                    {
                        var mbox = PPRPApp.Windows.MessageBox;
                        mbox.Owner = this;
                        string msg = string.Empty;
                        msg += "ลำดับที่ผู้สมัครต้องเป็นตัวเลขเท่านั้น";
                        mbox.Setup(msg, "PPRP");
                        mbox.ShowDialog();

                        txtCandidateNo.SelectAll();
                        txtCandidateNo.Focus();

                        return;
                    }
                    else
                    {
                        if (iNo < 0)
                        {
                            var mbox = PPRPApp.Windows.MessageBox;
                            mbox.Owner = this;
                            string msg = string.Empty;
                            msg += "หมายเลขเขตต้องมากกว่าหรือเท่ากับ 0";
                            mbox.Setup(msg, "PPRP");
                            mbox.ShowDialog();

                            txtCandidateNo.SelectAll();
                            txtCandidateNo.Focus();

                            return;
                        }
                    }
                }

                #endregion

                if (null != _item)
                {
                    _item.ThaiYear = 2566; // Force thai year.
                    _item.PersonId = _person.PersonId;

                    ret = MPDC.Save(_item);
                    if (ret.Failed)
                    {
                        var mbox = PPRPApp.Windows.MessageBox;
                        mbox.Owner = this;
                        string msg = string.Empty;
                        msg += "ไม่สามารถบันทึกข้อมูลว่าที่ผู้สมัครปี 2566 ได้" + Environment.NewLine;
                        msg += ret.ErrMsg;
                        mbox.Setup(msg, "PPRP");
                        mbox.ShowDialog();
                        return;
                    }
                    else
                    {
                        var mbox = PPRPApp.Windows.MessageBox;
                        mbox.Owner = this;
                        string msg = string.Empty;
                        msg += "บันทึกข้อมูลว่าที่ผู้สมัครปี 2566 สำเร็จ";
                        mbox.Setup(msg, "PPRP");
                        mbox.ShowDialog();

                        if (!_addNew)
                        {
                            DialogResult = true; // not add new mode so exit.
                        }
                        // in add new mode so clear screen and prepare empty item
                        Setup(null, _addNew);
                    }
                }
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Setup.
        /// </summary>
        /// <param name="value">The edit item instance.</param>
        public void Setup(MPDC value, bool addNew = false)
        {
            _addNew = addNew;

            personEditor.DataContext = null;
            imgParty.DataContext = _defaultParty;

            _item = value;

            if (_addNew && null == _item) _item = new MPDC();

            Dispatcher.BeginInvoke(new Action(() =>
            {
                LoadProvinces();
                LoadParties();

                // set callback.
                if (null != _item)
                {
                    // keep original name to detect changed.
                    //_item.WhenPartyNameChanged(_item.Prefix, _item.FirstName, _item.LastName, CheckPersonName);

                    // load person information.
                    _person = MPerson.Get(_item.PersonId).Value();
                    if (null == _person)
                    {
                        _person = new MPerson(); // no person found so create new one.
                    }

                    SetupPerson(_person, addNew);
                }
                // setup data context
                DataContext = _item;
            }), DispatcherPriority.Render);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets current item.
        /// </summary>
        public MPDC Item { get { return _item; } }
        /// <summary>
        /// Gets current person.
        /// </summary>
        public MPerson Person { get { return _person; } }
        /// <summary>
        /// Checks is add new mode.
        /// </summary>
        public bool IsAddNew { get { return _addNew; } }

        #endregion
    }
}
