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
    #region MADMPak

    /// <summary>
    /// The MADMPak class
    /// </summary>
    public class MADMPak : NInpc
    {
        #region Internal Variables

        private string _RegionId = null;
        private string _RegionName = null;

        private string _ADM1Code = null;
        private string _ADM2Code = null;
        private string _ADM3Code = null;

        private string _ProvinceId = null;
        private string _ProvinceNameTH = null;
        private string _ProvinceNameEN = null;

        private string _DistrictId = null;
        private string _DistrictNameTH = null;
        private string _DistrictNameEN = null;

        private string _SubDistrictId = null;
        private string _SubDistrictNameTH = null;
        private string _SubDistrictNameEN = null;

        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public MADMPak() : base()
        {

        }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~MADMPak()
        {

        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets Region Id.
        /// </summary>
        public string RegionId
        {
            get { return _RegionId; }
            set
            {
                if (_RegionId != value)
                {
                    _RegionId = value;
                    // Raise Event
                    Raise(() => RegionId);
                }
            }
        }
        /// <summary>
        /// Gets or sets Region Name.
        /// </summary>
        [ExcelColumn("ภาค")]
        public string RegionName
        {
            get { return _RegionName; }
            set
            {
                if (_RegionName != value)
                {
                    _RegionName = value;
                    // Raise Event
                    Raise(() => RegionName);
                }
            }
        }

        /// <summary>
        /// Gets or sets ADM1Code.
        /// </summary>
        public string ADM1Code
        {
            get { return _ADM1Code; }
            set
            {
                if (_ADM1Code != value)
                {
                    _ADM1Code = value;
                    Raise(() => ADM1Code);
                }
            }
        }
        /// <summary>
        /// Gets or sets ADM2Code.
        /// </summary>
        public string ADM2Code
        {
            get { return _ADM2Code; }
            set
            {
                if (_ADM2Code != value)
                {
                    _ADM2Code = value;
                    Raise(() => ADM2Code);
                }
            }
        }
        /// <summary>
        /// Gets or sets ADM3Code.
        /// </summary>
        public string ADM3Code
        {
            get { return _ADM3Code; }
            set
            {
                if (_ADM3Code != value)
                {
                    _ADM3Code = value;
                    Raise(() => ADM3Code);
                }
            }
        }

        /// <summary>
        /// Gets or sets Province Id.
        /// </summary>
        [ExcelColumn("รหัสจังหวัด")]
        public string ProvinceId
        {
            get { return _ProvinceId; }
            set
            {
                if (_ProvinceId != value)
                {
                    _ProvinceId = value;
                    Raise(() => ProvinceId);
                }
            }
        }
        /// <summary>
        /// Gets or sets Province Name (TH).
        /// </summary>
        [ExcelColumn("จังหวัด")]
        public string ProvinceNameTH
        {
            get { return _ProvinceNameTH; }
            set
            {
                if (_ProvinceNameTH != value)
                {
                    _ProvinceNameTH = value;
                    Raise(() => ProvinceNameTH);
                }
            }
        }
        /// <summary>
        /// Gets or sets Province Name (EN).
        /// </summary>
        public string ProvinceNameEN
        {
            get { return _ProvinceNameEN; }
            set
            {
                if (_ProvinceNameEN != value)
                {
                    _ProvinceNameEN = value;
                    Raise(() => ProvinceNameEN);
                }
            }
        }

        /// <summary>
        /// Gets or sets Sub District Id.
        /// </summary>
        [ExcelColumn("รหัสอำเภอ")]
        public string DistrictId
        {
            get { return _DistrictId; }
            set
            {
                if (_DistrictId != value)
                {
                    _DistrictId = value;
                    Raise(() => DistrictId);
                }
            }
        }
        /// <summary>
        /// Gets or sets District Name (TH).
        /// </summary>
        [ExcelColumn("อำเภอ")]
        public string DistrictNameTH
        {
            get { return _DistrictNameTH; }
            set
            {
                if (_DistrictNameTH != value)
                {
                    _DistrictNameTH = value;
                    Raise(() => DistrictNameTH);
                }
            }
        }
        /// <summary>
        /// Gets or sets District Name (EN).
        /// </summary>
        public string DistrictNameEN
        {
            get { return _DistrictNameEN; }
            set
            {
                if (_DistrictNameEN != value)
                {
                    _DistrictNameEN = value;
                    Raise(() => DistrictNameEN);
                }
            }
        }

        /// <summary>
        /// Gets or sets Sub District Id.
        /// </summary>
        [ExcelColumn("รหัสตำบล")]
        public string SubDistrictId
        {
            get { return _SubDistrictId; }
            set
            {
                if (_SubDistrictId != value)
                {
                    _SubDistrictId = value;
                    Raise(() => SubDistrictId);
                }
            }
        }
        /// <summary>
        /// Gets or sets Sub District Name (TH).
        /// </summary>
        [ExcelColumn("ตำบล")]
        public string SubDistrictNameTH
        {
            get { return _SubDistrictNameTH; }
            set
            {
                if (_SubDistrictNameTH != value)
                {
                    _SubDistrictNameTH = value;
                    Raise(() => SubDistrictNameTH);
                }
            }
        }
        /// <summary>
        /// Gets or sets Sub District Name (EN).
        /// </summary>
        public string SubDistrictNameEN
        {
            get { return _SubDistrictNameEN; }
            set
            {
                if (_SubDistrictNameEN != value)
                {
                    _SubDistrictNameEN = value;
                    Raise(() => SubDistrictNameEN);
                }
            }
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Import ADM Pak.
        /// </summary>
        /// <param name="value">The MADMPak value.</param>
        /// <returns>Returns NDbResult instance.</returns>
        public static NDbResult Import(MADMPak value)
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

            if (null == value)
            {
                string msg = "Value is null.";
                med.Err(msg);
                // Set error number/message
                ret.ErrNum = 8000;
                ret.ErrMsg = msg;

                return ret;
            }

            var p = new DynamicParameters();
            p.Add("@RegionName", value.RegionName);
            p.Add("@ProvinceId", value.ProvinceId);
            p.Add("@DistrictId", value.DistrictId);
            p.Add("@SubDistrictId", value.SubDistrictId);
            p.Add("@ProvinceNameTH", value.ProvinceNameTH);
            p.Add("@DistrictNameTH", value.DistrictNameTH);
            p.Add("@SubDistrictNameTH", value.SubDistrictNameTH);

            p.Add("@errNum", dbType: DbType.Int32, direction: ParameterDirection.Output);
            p.Add("@errMsg", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

            try
            {
                cnn.Execute("ImportADMPak", p, commandType: CommandType.StoredProcedure);
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

        public static NDbResult<List<MADMPak>> Gets(string regionId = null,
            string provinceNameTH = null, string districtNameTH = null, string subdistrictNameTH = null)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            NDbResult<List<MADMPak>> rets = new NDbResult<List<MADMPak>>();

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

            p.Add("@RegionId", regionId);
            p.Add("@ProvinceNameTH", provinceNameTH);
            p.Add("@DistrictNameTH", districtNameTH);
            p.Add("@SubdistrictNameTH", subdistrictNameTH);

            try
            {
                rets.Value = cnn.Query<MADMPak>("GetMADMPaks", p,
                    commandType: CommandType.StoredProcedure).ToList();
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
                rets.Value = new List<MADMPak>();
            }

            return rets;
        }

        #endregion
    }

    #endregion
}
