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

namespace PPRP.Domains
{
    #region AreaMenuItem

    public class AreaMenuItem
    {
        public const string PAK = "PAK";
        public const string PROVINCE = "PROVINCE";
        public const string POLLUNIT = "POLLUNIT";

        public virtual string ItemType { get; set; }
        public virtual string DisplayText { get; set; }
        public virtual string PakUnitText { get; set; }
        public virtual string ProvinceText { get; set; }
    }

    #endregion

    #region PakMenuItem

    public class PakMenuItem : AreaMenuItem
    {
        #region Internal Variables

        private List<ProvinceMenuItem> _Items = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public PakMenuItem() : base()
        {

        }

        #endregion

        #region Public Properties

        public override string ItemType 
        {
            get { return PAK; }
            set { }
        }

        public override string DisplayText
        {
            get { return RegionName;  }
            set { }
        }

        public override string ProvinceText 
        {
            get { return string.Empty;  }
            set { }
        }
        public override string PakUnitText 
        {
            get 
            {
                int unitCnt = 0;
                if (null != Provinces)
                {
                    Provinces.ForEach(province => 
                    {
                        unitCnt += province.UnitCount;
                    });
                }
                return string.Format("({0} เขต)", unitCnt);

            }
            set { }
        }

        public string RegionId { get; set; }
        public string RegionName { get; set; }

        public List<ProvinceMenuItem> Provinces
        {
            get 
            {
                lock (typeof(PakMenuItem))
                {
                    if (null == _Items)
                    {
                        _Items = ProvinceMenuItem.Gets(RegionId).Value;
                    }
                }
                return _Items;
            }
            set { }
        }

        #endregion

        #region Static Methods

        public static NDbResult<List<PakMenuItem>> Gets()
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            NDbResult<List<PakMenuItem>> rets = new NDbResult<List<PakMenuItem>>();

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

            try
            {
                var items = cnn.Query<PakMenuItem>("GetRegionMenuItems", p,
                    commandType: CommandType.StoredProcedure);
                rets.Value = (null != items) ? items.ToList() : new List<PakMenuItem>();
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
                rets.Value = new List<PakMenuItem>();
            }

            return rets;
        }

        #endregion
    }

    #endregion

    #region ProvinceMenuItem

    public class ProvinceMenuItem : AreaMenuItem
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public ProvinceMenuItem() : base()
        {

        }

        #endregion

        #region Public Properties

        public override string ItemType
        {
            get { return PROVINCE; }
            set { }
        }

        public override string DisplayText
        {
            get { return string.Format("{0} - {1} เขต", ProvinceNameTH, UnitCount); }
            set { }
        }

        public override string ProvinceText
        {
            get { return string.Format("{0}", ProvinceNameTH); }
            set { }
        }

        public string RegionId { get; set; }
        public string ProvinceId { get; set; }
        public string ProvinceNameTH { get; set; }

        public int UnitCount { get; set; }

        #endregion

        #region Static Methods

        public static NDbResult<List<ProvinceMenuItem>> Gets(string regionId)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            NDbResult<List<ProvinceMenuItem>> rets = new NDbResult<List<ProvinceMenuItem>>();

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

            try
            {
                var items = cnn.Query<ProvinceMenuItem>("GetProvinceMenuItems", p,
                    commandType: CommandType.StoredProcedure);
                rets.Value = (null != items) ? items.ToList() : new List<ProvinceMenuItem>();
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
                rets.Value = new List<ProvinceMenuItem>();
            }

            return rets;
        }

        #endregion
    }

    #endregion

    #region PollingUnitMenuItem

    public class PollingUnitMenuItem : AreaMenuItem
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public PollingUnitMenuItem() : base()
        {

        }

        #endregion

        #region Public Properties

        public override string ItemType
        {
            get { return POLLUNIT; }
            set { }
        }

        public override string DisplayText
        {
            get 
            { 
                return string.Format("{0} เขต {1}", ProvinceNameTH, PollingUnitNo);
            }
            set { }
        }
        public override string ProvinceText
        {
            get { return string.Format("{0}", ProvinceNameTH); }
            set { }
        }

        public string DisplayMenu
        {
            get
            {
                if (ThaiYear == 2566)
                {
                    return string.Format("เขต {0} ({1})", PollingUnitNo, ThaiYear);
                }
                else
                {
                    return string.Format("เขต {0}", PollingUnitNo);
                }
            }
            set { }
        }

        public int ThaiYear { get; set; }
        public string RegionId { get; set; }
        public string ProvinceId { get; set; }
        public string ProvinceNameTH { get; set; }
        public int PollingUnitNo { get; set; }

        #endregion

        #region Static Methods

        public static NDbResult<List<PollingUnitMenuItem>> Gets(string regionId, string provinceId)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            NDbResult<List<PollingUnitMenuItem>> rets = new NDbResult<List<PollingUnitMenuItem>>();

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
            p.Add("@ProvinceId", provinceId);

            try
            {
                var items = cnn.Query<PollingUnitMenuItem>("GetPollingUnitMenuItems", p,
                    commandType: CommandType.StoredProcedure);
                rets.Value = (null != items) ? items.ToList() : new List<PollingUnitMenuItem>();
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
                rets.Value = new List<PollingUnitMenuItem>();
            }

            return rets;
        }

        #endregion
    }

    #endregion
}
