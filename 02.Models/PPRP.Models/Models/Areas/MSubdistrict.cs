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
    #region MADM3

    /// <summary>
    /// The MADM3 class.
    /// </summary>
    public class MADM3 : NInpc
    {
        #region Internal Variables

        private string _ADM3Code = null;

        private string _SubdistrictNameEN = null;
        private string _SubdistrictNameTH = null;

        private string _DistrictNameEN = null;
        private string _DistrictNameTH = null;

        private string _ProvinceNameEN = null;
        private string _ProvinceNameTH = null;

        private decimal _SubdistrictAreaM2 = decimal.Zero;

        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public MADM3() : base()
        {

        }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~MADM3()
        {

        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets ADM3 Code.
        /// </summary>
        [ExcelColumn("ADM3_CODE")]
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
        /// Gets or sets Subdistrict Name (EN).
        /// </summary>
        [ExcelColumn("ADM3_EN")]
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
        /// <summary>
        /// Gets or sets Subdistrict Name (TH).
        /// </summary>
        [ExcelColumn("ADM3_TH")]
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
        /// Gets or sets District Name (EN).
        /// </summary>
        [ExcelColumn("ADM2_EN")]
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
        [ExcelColumn("ADM2_TH")]
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
        /// Gets or sets Sub district Area M2.
        /// </summary>
        [ExcelColumn("AREA_M2")]
        public decimal SubdistrictAreaM2
        {
            get { return _SubdistrictAreaM2; }
            set
            {
                if (_SubdistrictAreaM2 != value)
                {
                    _SubdistrictAreaM2 = value;
                    // Raise Event
                    Raise(() => SubdistrictAreaM2);
                }
            }
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Import ADM3.
        /// </summary>
        /// <param name="value">The ADM3 value.</param>
        /// <returns>Returns NDbResult instance.</returns>
        public static NDbResult Import(MADM3 value)
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
            p.Add("@ADM3Code", value.ADM3Code);
            p.Add("@SubDistrictNameTH", value.SubdistrictNameTH);
            p.Add("@SubDistrictNameEN", value.SubdistrictNameEN);
            p.Add("@DistrictNameTH", value.DistrictNameTH);
            p.Add("@DistrictNameEN", value.DistrictNameEN);
            p.Add("@ProvinceNameTH", value.ProvinceNameTH);
            p.Add("@ProvinceNameEN", value.ProvinceNameEN);
            p.Add("@AreaM2", value.SubdistrictAreaM2);

            p.Add("@errNum", dbType: DbType.Int32, direction: ParameterDirection.Output);
            p.Add("@errMsg", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

            try
            {
                cnn.Execute("ImportADM3", p, commandType: CommandType.StoredProcedure);
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

    #region MSubdistrict

    /// <summary>
    /// The MSubdistrict class.
    /// </summary>
    public class MSubdistrict : NInpc
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

        private string _ADM3Code = null;
        private string _SubdistrictId = null;
        private string _SubdistrictNameEN = null;
        private string _SubdistrictNameTH = null;

        private decimal _SubdistrictAreaM2 = decimal.Zero;

        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public MSubdistrict() : base()
        {
            
        }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~MSubdistrict()
        {

        }

        #endregion

        #region Public Properties

        #region Subdistrict

        /// <summary>
        /// Gets or sets ADM3 Code.
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
        /// Gets or sets Subdistrict Id.
        /// </summary>
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
        /// Gets or sets Subdistrict Name (EN).
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
        /// <summary>
        /// Gets or sets Subdistrict Name (TH).
        /// </summary>
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
        /// Gets or sets Sub District Area M2.
        /// </summary>
        public decimal SubdistrictAreaM2
        {
            get { return _SubdistrictAreaM2; }
            set
            {
                if (_SubdistrictAreaM2 != value)
                {
                    _SubdistrictAreaM2 = value;
                    // Raise Event
                    Raise(() => SubdistrictAreaM2);
                }
            }
        }

        #endregion

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
        /// <param name="adm3Code"></param>
        /// <returns></returns>
        public static NDbResult<List<MSubdistrict>> Gets(
            string regionId = null,
            string adm1Code = null, string adm2Code = null, string adm3Code = null )
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            NDbResult<List<MSubdistrict>> rets = new NDbResult<List<MSubdistrict>>();

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
                rets.Value = cnn.Query<MSubdistrict>("GetMSubdistricts", p,
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
                rets.Value = new List<MSubdistrict>();
            }

            return rets;
        }

        #endregion
    }

    #endregion
}
