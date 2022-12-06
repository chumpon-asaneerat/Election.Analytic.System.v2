#region Using

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
        private string _GeoGroup = null;
        private string _GeoSubGroup = null;

        private string _ADM1Code = null;
        private string _ADM2Code = null;
        private string _ADM3Code = null;

        private string _ProvinceId = null;
        private string _ProvinceNameTH = null;
        private string _ProvinceNameEN = null;

        private string _DistrictId = null;
        private string _DistrictNameTH = null;
        private string _DistrictNameEN = null;

        private string _SubdistrictId = null;
        private string _SubdistrictNameTH = null;
        private string _SubdistrictNameEN = null;

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
        [ExcelColumn("ภาค", 1)]
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
        /// Gets or sets Geo Group.
        /// </summary>
        public string GeoGroup
        {
            get { return _GeoGroup; }
            set
            {
                if (_GeoGroup != value)
                {
                    _GeoGroup = value;
                    // Raise Event
                    Raise(() => GeoGroup);
                }
            }
        }
        /// <summary>
        /// Gets or sets Geo SubGroup.
        /// </summary>
        public string GeoSubGroup
        {
            get { return _GeoSubGroup; }
            set
            {
                if (_GeoSubGroup != value)
                {
                    _GeoSubGroup = value;
                    // Raise Event
                    Raise(() => GeoSubGroup);
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
        [ExcelColumn("รหัสจังหวัด", 2)]
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
        [ExcelColumn("ชื่อจังหวัด", 3)]
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
        [ExcelColumn("รหัสอำเภอ", 4)]
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
        [ExcelColumn("ชื่ออำเภอ", 5)]
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
        [ExcelColumn("รหัสตำบล", 6)]
        public string SubdistrictId
        {
            get { return _SubdistrictId; }
            set
            {
                if (_SubdistrictId != value)
                {
                    _SubdistrictId = value;
                    Raise(() => SubdistrictId);
                }
            }
        }
        /// <summary>
        /// Gets or sets Sub District Name (TH).
        /// </summary>
        [ExcelColumn("ชื่อตำบล", 7)]
        public string SubdistrictNameTH
        {
            get { return _SubdistrictNameTH; }
            set
            {
                if (_SubdistrictNameTH != value)
                {
                    _SubdistrictNameTH = value;
                    Raise(() => SubdistrictNameTH);
                }
            }
        }
        /// <summary>
        /// Gets or sets Sub District Name (EN).
        /// </summary>
        public string SubdistrictNameEN
        {
            get { return _SubdistrictNameEN; }
            set
            {
                if (_SubdistrictNameEN != value)
                {
                    _SubdistrictNameEN = value;
                    Raise(() => SubdistrictNameEN);
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
            p.Add("@SubdistrictId", value.SubdistrictId);
            p.Add("@ProvinceNameTH", value.ProvinceNameTH);
            p.Add("@DistrictNameTH", value.DistrictNameTH);
            p.Add("@SubdistrictNameTH", value.SubdistrictNameTH);

            p.Add("@errNum", dbType: DbType.Int32, direction: ParameterDirection.Output);
            p.Add("@errMsg", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

            try
            {
                cnn.Execute("ImportADMPak", p, commandType: CommandType.StoredProcedure);
                ret.Success();
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
        /// <summary>
        /// Gets.
        /// </summary>
        /// <param name="regionId"></param>
        /// <param name="adm1Code"></param>
        /// <param name="adm2Code"></param>
        /// <param name="adm3Code"></param>
        /// <returns></returns>
        public static NDbResult<List<MADMPak>> Gets(
            string regionId = null,
            string adm1Code = null, string adm2Code = null, string adm3Code = null)
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
            p.Add("@ADM1Code", adm1Code);
            p.Add("@ADM2Code", adm2Code);
            p.Add("@ADM3Code", adm3Code);

            try
            {
                var data = cnn.Query<MADMPak>("GetMADMPaks", p,
                    commandType: CommandType.StoredProcedure).ToList();
                rets.Success(data);
            }
            catch (Exception ex)
            {
                med.Err(ex);
                // Set error number/message
                rets.ErrNum = 9999;
                rets.ErrMsg = ex.Message;
            }

            if (null == rets.data)
            {
                // create empty list.
                rets.data = new List<MADMPak>();
            }

            return rets;
        }

        #endregion
    }

    #endregion
}
