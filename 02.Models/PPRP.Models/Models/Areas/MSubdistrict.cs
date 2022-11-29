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

        private string _SubDistrictNameEN = null;
        private string _SubDistrictNameTH = null;

        private string _DistrictNameEN = null;
        private string _DistrictNameTH = null;

        private string _ProvinceNameEN = null;
        private string _ProvinceNameTH = null;

        private decimal _SubDistrictAreaM2 = decimal.Zero;

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
        /// Gets or sets Sub District Name (EN).
        /// </summary>
        [ExcelColumn("ADM3_EN")]
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
        /// <summary>
        /// Gets or sets Sub District Name (TH).
        /// </summary>
        [ExcelColumn("ADM3_TH")]
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
        /// Gets or sets Sub District Area M2.
        /// </summary>
        [ExcelColumn("AREA_M2")]
        public decimal SubDistrictAreaM2
        {
            get { return _SubDistrictAreaM2; }
            set
            {
                if (_SubDistrictAreaM2 != value)
                {
                    _SubDistrictAreaM2 = value;
                    // Raise Event
                    Raise(() => SubDistrictAreaM2);
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
        public static NDbResult ImportADM3(MADM3 value)
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
            p.Add("@SubDistrictNameTH", value.SubDistrictNameTH);
            p.Add("@SubDistrictNameEN", value.SubDistrictNameEN);
            p.Add("@DistrictNameTH", value.DistrictNameTH);
            p.Add("@DistrictNameEN", value.DistrictNameEN);
            p.Add("@ProvinceNameTH", value.ProvinceNameTH);
            p.Add("@ProvinceNameEN", value.ProvinceNameEN);
            p.Add("@AreaM2", value.SubDistrictAreaM2);

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
    public class MSubdistrict : MADM3
    {
        #region Internal Variables

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

        #endregion

        #region Static Methods

        #endregion
    }

    #endregion
}
