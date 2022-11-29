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
    #region MADM1

    /// <summary>
    /// The MADM1 class.
    /// </summary>
    public class MADM1 : NInpc
    {
        #region Internal Variables

        private string _ADM1Code = null;

        private string _ProvinceNameEN = null;
        private string _ProvinceNameTH = null;
        private decimal _ProvinceAreaM2 = decimal.Zero;

        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public MADM1() : base()
        {

        }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~MADM1()
        {

        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets ADM1 Code.
        /// </summary>
        [ExcelColumn("ADM1_CODE")]
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
        /// Gets or sets Province Name (EN).
        /// </summary>
        [ExcelColumn("ADM1_EN")]
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
        [ExcelColumn("ADM1_TH")]
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
        /// Gets or sets Province Area M2.
        /// </summary>
        [ExcelColumn("AREA_M2")]
        public decimal ProvinceAreaM2
        {
            get { return _ProvinceAreaM2; }
            set
            {
                if (_ProvinceAreaM2 != value)
                {
                    _ProvinceAreaM2 = value;
                    // Raise Event
                    Raise(() => ProvinceAreaM2);
                }
            }
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Import ADM1.
        /// </summary>
        /// <param name="value">The ADM1 value.</param>
        /// <returns>Returns NDbResult instance.</returns>
        public static NDbResult ImportADM1(MADM1 value)
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
            p.Add("@ADM1Code", value.ADM1Code);
            p.Add("@ProvinceNameTH", value.ProvinceNameTH);
            p.Add("@ProvinceNameEN", value.ProvinceNameEN);
            p.Add("@AreaM2", value.ProvinceAreaM2);

            p.Add("@errNum", dbType: DbType.Int32, direction: ParameterDirection.Output);
            p.Add("@errMsg", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

            try
            {
                cnn.Execute("ImportADM1", p, commandType: CommandType.StoredProcedure);
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

    #region MProvince

    /// <summary>
    /// The MProvince class.
    /// </summary>
    public class MProvince : MADM1
    {
        #region Internal Variables

        private int? _ProvinceId = new int?();

        private int _RegionId = 0;
        private string _RegionName = null;
        private string _GeoGroup = null;
        private string _GeoSubGroup = null;

        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public MProvince() : base()
        {

        }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~MProvince()
        {

        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets ProvinceId.
        /// </summary>
        public int? ProvinceId
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
        /// Gets or sets RegionId.
        /// </summary>
        public int RegionId
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

        #region Static Methods

        /// <summary>
        /// Gets.
        /// </summary>
        /// <param name="adm1code">The ADM1 Code.</param>
        /// <param name="provinceNameTH">The province name (th).</param>
        /// <param name="regionId">The region id.</param>
        /// <param name="regionName">The region name.</param>
        /// <param name="geoGroup">The geo group.</param>
        /// <param name="geoSubGroup">The geo subgroup.</param>
        /// <returns>Returns list of MProvince instance.</returns>
        public static NDbResult<List<MProvince>> Gets(
            string adm1code = null, string provinceNameTH = null,
            string regionId = null, string regionName = null,
            string geoGroup = null, string geoSubGroup = null)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            NDbResult<List<MProvince>> rets = new NDbResult<List<MProvince>>();

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
            p.Add("@ADM1Code", adm1code);
            p.Add("@ProvinceNameTH", provinceNameTH);
            p.Add("@RegionId", regionId);
            p.Add("@RegionName", regionName);
            p.Add("@GeoGroup", geoGroup);
            p.Add("@GeoSubGroup", geoSubGroup);

            try
            {
                rets.Value = cnn.Query<MProvince>("GetMProvinces", p,
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
                rets.Value = new List<MProvince>();
            }

            return rets;
        }
        /*
        /// <summary>
        /// Save.
        /// </summary>
        /// <param name="value">The MProvince insance.</param>
        /// <returns>Returns NDbResult instance.</returns>
        public static NDbResult Save(MProvince value)
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
            p.Add("@ADM1Code", value.ADM1Code);
            p.Add("@ProvinceId", value.ProvinceId);
            p.Add("@RegionId", value.RegionId);
            p.Add("@ProvinceNameTH", value.ProvinceNameTH);
            p.Add("@ProvinceNameEN", value.ProvinceNameEN);

            p.Add("@errNum", dbType: DbType.Int32, direction: ParameterDirection.Output);
            p.Add("@errMsg", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

            try
            {
                cnn.Execute("SaveMProvince", p, commandType: CommandType.StoredProcedure);
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
        */

        #endregion
    }

    #endregion
}
