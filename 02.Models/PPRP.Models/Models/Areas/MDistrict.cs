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
    #region MADM2

    /// <summary>
    /// The MADM2 class.
    /// </summary>
    public class MADM2 : NInpc
    {
        #region Internal Variables

        private string _ADM2Code = null;

        private string _DistrictNameEN = null;
        private string _DistrictNameTH = null;

        private string _ProvinceNameEN = null;
        private string _ProvinceNameTH = null;

        private decimal _DistrictAreaM2 = decimal.Zero;

        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public MADM2() : base()
        {

        }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~MADM2()
        {

        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets ADM2 Code.
        /// </summary>
        [ExcelColumn("ADM2_CODE", 5)]
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
        /// Gets or sets District Name (EN).
        /// </summary>
        [ExcelColumn("ADM2_EN", 4)]
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
        /// Gets or sets District Name (TH).
        /// </summary>
        [ExcelColumn("ADM2_TH", 3)]
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
        /// Gets or sets Province Name (EN).
        /// </summary>
        [ExcelColumn("ADM1_EN", 2)]
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
        /// Gets or sets Province Name (TH).
        /// </summary>
        [ExcelColumn("ADM1_TH", 1)]
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
        /// Gets or sets District Area M2.
        /// </summary>
        [ExcelColumn("AREA_M2", 6)]
        public decimal DistrictAreaM2
        {
            get { return _DistrictAreaM2; }
            set
            {
                if (_DistrictAreaM2 != value)
                {
                    _DistrictAreaM2 = value;
                    // Raise Event
                    Raise(() => DistrictAreaM2);
                }
            }
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Import ADM2.
        /// </summary>
        /// <param name="value">The ADM2 value.</param>
        /// <returns>Returns NDbResult instance.</returns>
        public static NDbResult Import(MADM2 value)
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
            p.Add("@ADM2Code", value.ADM2Code);
            p.Add("@DistrictNameTH", value.DistrictNameTH);
            p.Add("@DistrictNameEN", value.DistrictNameEN);
            p.Add("@ProvinceNameTH", value.ProvinceNameTH);
            p.Add("@ProvinceNameEN", value.ProvinceNameEN);
            p.Add("@AreaM2", value.DistrictAreaM2);

            p.Add("@errNum", dbType: DbType.Int32, direction: ParameterDirection.Output);
            p.Add("@errMsg", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

            try
            {
                cnn.Execute("ImportADM2", p, commandType: CommandType.StoredProcedure);
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
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        public static NDbResult<List<MADM2>> Gets()
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            NDbResult<List<MADM2>> rets = new NDbResult<List<MADM2>>();

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

            p.Add("@RegionId", null);
            p.Add("@ADM1Code", null);
            p.Add("@ADM2Code", null);

            try
            {
                rets.Value = cnn.Query<MADM2>("GetMDistricts", p,
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
                rets.Value = new List<MADM2>();
            }

            return rets;
        }

        #endregion
    }

    #endregion

    #region MDistrict

    /// <summary>
    /// The MDistrict class.
    /// </summary>
    public class MDistrict : NInpc
    {
        #region Internal Variables

        private string _RegionId = null;
        private string _RegionName = null;
        private string _GeoGroup = null;
        private string _GeoSubGroup = null;

        private string _ADM1Code = null;
        private string _ProvinceId = null;
        private string _ProvinceNameEN = null;
        private string _ProvinceNameTH = null;

        private string _ADM2Code = null;
        private string _DistrictId = null;
        private string _DistrictNameEN = null;
        private string _DistrictNameTH = null;

        private decimal _DistrictAreaM2 = decimal.Zero;

        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public MDistrict() : base()
        {

        }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~MDistrict()
        {

        }

        #endregion

        #region Public Properties

        #region District

        /// <summary>
        /// Gets or sets ADM2 Code.
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
        /// Gets or sets District Id.
        /// </summary>
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
        /// Gets or sets District Name (TH).
        /// </summary>
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
        /// Gets or sets District Area M2.
        /// </summary>
        public decimal DistrictAreaM2
        {
            get { return _DistrictAreaM2; }
            set
            {
                if (_DistrictAreaM2 != value)
                {
                    _DistrictAreaM2 = value;
                    // Raise Event
                    Raise(() => DistrictAreaM2);
                }
            }
        }

        #endregion

        #region Province

        /// <summary>
        /// Gets or sets ADM1 Code.
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
        /// Gets or sets ProvinceId.
        /// </summary>
        public string ProvinceId
        {
            get { return _ProvinceId; }
            set
            {
                if (_ProvinceId != value)
                {
                    _ProvinceId = value;
                    // Raise Event
                    Raise(() => ProvinceId);
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
        /// Gets or sets Province Name (TH).
        /// </summary>
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

        #endregion

        #region Region

        /// <summary>
        /// Gets or sets RegionId.
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

        #endregion

        #endregion

        #region Static Methods

        /// <summary>
        /// Gets.
        /// </summary>
        /// <param name="regionId"></param>
        /// <param name="adm1Code"></param>
        /// <param name="adm2Code"></param>
        /// <returns></returns>
        public static NDbResult<List<MDistrict>> Gets(
            string regionId = null,
            string adm1Code = null, string adm2Code = null)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            NDbResult<List<MDistrict>> rets = new NDbResult<List<MDistrict>>();

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

            try
            {
                rets.Value = cnn.Query<MDistrict>("GetMDistricts", p,
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
                rets.Value = new List<MDistrict>();
            }

            return rets;
        }

        #endregion
    }

    #endregion
}
