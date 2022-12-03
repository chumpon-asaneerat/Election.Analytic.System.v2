﻿#region Using

using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Linq;
using System.Reflection;

using System.Windows.Media;

using NLib;

using Dapper;
using Newtonsoft.Json;

#endregion

namespace PPRP.Models
{
    #region  MPerson

    /// <summary>
    /// The MPerson class.
    /// </summary>
    public class MPerson : NInpc
    {
        #region Internal Variables

        private int _PersonId = 0;
        private string _Prefix = null;
        private string _FirstName = null;
        private string _LastName = null;

        private bool _isDefault = true;
        private bool _ImageLoading = false;
        private ImageSource _img = null;

        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public MPerson() : base()
        {

        }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~MPerson()
        {

        }

        #endregion

        #region Private Methods

        private void CheckExist()
        {
            this.DisableNotify();

            this.EnableNotify();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets PersonId.
        /// </summary>
        public int PersonId
        {
            get { return _PersonId; }
            set
            {
                if (_PersonId != value)
                {
                    _PersonId = value;
                    // Raise Event
                    Raise(() => PersonId);
                }
            }
        }
        /// <summary>
        /// Gets or sets Prefix.
        /// </summary>
        public string Prefix
        {
            get { return _Prefix; }
            set
            {
                if (_Prefix != value)
                {
                    _Prefix = value;
                    // Raise Event
                    Raise(() => Prefix);
                    Raise(() => FullName);
                }
            }
        }
        /// <summary>
        /// Gets or sets First Name.
        /// </summary>
        public string FirstName
        {
            get { return _FirstName; }
            set
            {
                if (_FirstName != value)
                {
                    _FirstName = value;
                    // Raise Event
                    Raise(() => FirstName);
                    Raise(() => FullName);
                }
            }
        }
        /// <summary>
        /// Gets or sets Last Name.
        /// </summary>
        public string LastName
        {
            get { return _LastName; }
            set
            {
                if (_LastName != value)
                {
                    _LastName = value;
                    // Raise Event
                    Raise(() => LastName);
                    Raise(() => FullName);
                }
            }
        }
        /// <summary>
        /// Gets Full Name.
        /// </summary>
        public string FullName
        {
            get 
            {
                var ret = string.Empty;
                ret += !string.IsNullOrEmpty(_Prefix) ? _Prefix.Trim() + " " : string.Empty;
                ret += !string.IsNullOrEmpty(_FirstName) ? _FirstName.Trim() + " " : string.Empty;
                ret += !string.IsNullOrEmpty(_LastName) ? _LastName.Trim() : string.Empty;
                return ret.Trim();
            }
            set { }
        }
        /// <summary>
        /// Gets or sets DOB.
        /// </summary>
        public DateTime? DOB { get; set; }
        /// <summary>
        /// Gets or sets GenderId.
        /// </summary>
        public int? GenderId { get; set; }
        /// <summary>
        /// Gets or sets EducationId.
        /// </summary>
        public int? EducationId { get; set; }
        /// <summary>
        /// Gets or sets OccupationId.
        /// </summary>
        public int? OccupationId { get; set; }
        /// <summary>
        /// Gets or sets Remark.
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// Gets or sets Image Data buffers.
        /// </summary>
        public byte[] Data { get; set; }
        /// <summary>
        /// Gets Image.
        /// </summary>
        public ImageSource Image
        {
            get
            {
                if (null == _img && !_ImageLoading)
                {
                    _ImageLoading = true;

                    Defaults.RunInBackground(() =>
                    {
                        ImageSource imgSrc;
                        if (null == Data)
                        {
                            _isDefault = true;
                            imgSrc = Defaults.Person;
                        }
                        else
                        {
                            _isDefault = false;
                            imgSrc = ByteUtils.GetImageSource(Data);
                        }
                        _img = imgSrc;

                        _ImageLoading = false;
                        Raise(() => Image);
                    });
                }
                return _img;
            }
            set { }
        }
        /// <summary>
        /// Checks is default image.
        /// </summary>
        public bool IsDefault { get { return _isDefault; } }

        #endregion

        #region Static Methods

        /// <summary>
        /// Gets
        /// </summary>
        /// <param name="prefix">The prefix filter.</param>
        /// <param name="firstName">The first name filter.</param>
        /// <param name="lastName">The last name filter.</param>
        /// <param name="pageNo">The page number.</param>
        /// <param name="rowPerPage">The rows per page.</param>
        /// <returns></returns>
        public static NDbResult<List<MPerson>> Gets(string prefix, string firstName, string lastName,
            int pageNo = 1, int rowPerPage = 10)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            string sPrefix = prefix;
            if (string.IsNullOrWhiteSpace(prefix))
            {
                sPrefix = null;
            }
            string sFirstName = firstName;
            if (string.IsNullOrWhiteSpace(firstName))
            {
                sFirstName = null;
            }
            string sLastName = lastName;
            if (string.IsNullOrWhiteSpace(lastName))
            {
                sLastName = null;
            }

            NDbResult<List<MPerson>> rets = new NDbResult<List<MPerson>>();

            IDbConnection cnn = DbServer.Instance.Db;
            if (null == cnn || !DbServer.Instance.Connected)
            {
                string msg = "Connection is null or cannot connect to database server.";
                med.Err(msg);
                // Set error number/message
                rets.ErrNum = 8000;
                rets.ErrMsg = msg;

                return rets;
            }

            var p = new DynamicParameters();
            p.Add("@Prefix", sPrefix);
            p.Add("@FirstName", sFirstName);
            p.Add("@LastName", sLastName);

            p.Add("@pageNum", value: pageNo, dbType: DbType.Int32, direction: ParameterDirection.InputOutput);
            p.Add("@rowsPerPage", value: rowPerPage, dbType: DbType.Int32, direction: ParameterDirection.InputOutput);
            p.Add("@totalRecords", value: 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
            p.Add("@maxPage", value: 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

            p.Add("@errNum", dbType: DbType.Int32, direction: ParameterDirection.Output);
            p.Add("@errMsg", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

            try
            {
                var items = cnn.Query<MPerson>("GetMPersons", p,
                    commandType: CommandType.StoredProcedure);
                rets.Value = (null != items) ? items.ToList() : null;

                // Get Paging parameters
                rets.PageNo = p.Get<int>("@pageNum");
                rets.RowsPerPage = p.Get<int>("@rowsPerPage");
                rets.TotalRecords = p.Get<int>("@totalRecords");
                rets.MaxPage = p.Get<int>("@maxPage");
                // Set error number/message
                rets.ErrNum = p.Get<int>("@errNum");
                rets.ErrMsg = p.Get<string>("@errMsg");
            }
            catch (Exception ex)
            {
                med.Err(ex);
                // Set error number/message
                rets.ErrNum = 9999;
                rets.ErrMsg = ex.Message;
            }

            if (null == rets.Value)
            {
                // create empty list.
                rets.Value = new List<MPerson>();
            }

            return rets;
        }
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static NDbResult Delete(MPerson value)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            NDbResult ret = new NDbResult();

            IDbConnection cnn = DbServer.Instance.Db;
            if (null == cnn || !DbServer.Instance.Connected)
            {
                string msg = "Connection is null or cannot connect to database server.";
                med.Err(msg);
                // Set error number/message
                ret.ErrNum = 8000;
                ret.ErrMsg = msg;

                return ret;
            }

            var p = new DynamicParameters();
            p.Add("@PersonId", value.PersonId);

            p.Add("@errNum", dbType: DbType.Int32, direction: ParameterDirection.Output);
            p.Add("@errMsg", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

            try
            {
                cnn.Execute("DeleteMPerson", p, commandType: CommandType.StoredProcedure);
                // Set error number/message
                ret.ErrNum = p.Get<int>("@errNum");
                ret.ErrMsg = p.Get<string>("@errMsg");
            }
            catch (Exception ex)
            {
                med.Err(ex);
                // Set error number/message
                ret.ErrNum = 9999;
                ret.ErrMsg = ex.Message;
            }

            return ret;
        }

        #endregion
    }

    #endregion
}
