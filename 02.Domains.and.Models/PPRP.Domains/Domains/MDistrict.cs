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

namespace PPRP.Domains
{
    /// <summary>
    /// The MDistrict class.
    /// </summary>
    public class MDistrict
    {
        #region Public Properties

        public string DistrictId { get; set; }
        public string DistrictNameTH { get; set; }
        public string DistrictNameEN { get; set; }
        public string ADM2Code { get; set; }
        public decimal DistrictAreaM2 { get; set; }
        public Guid DistrictContentId { get; set; }

        public string ProvinceId { get; set; }
        public string ProvinceNameTH { get; set; }
        public string ProvinceNameEN { get; set; }
        public string ADM1Code { get; set; }

        public string RegionId { get; set; }
        public string RegionName { get; set; }
        public string GeoGroup { get; set; }
        public string GeoSubGroup { get; set; }


        #endregion

        #region Static Methods

        public static NDbResult<List<MDistrict>> Gets(
            string districtId = null, string districtNameTH = null,
            string provinceId = null, string provinceNameTH = null,
            string regionId = null, string regionName = null,
            string geoGroup = null, string geoSubGroup = null)
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

            p.Add("@DistrictId", districtId, dbType: DbType.String, direction: ParameterDirection.Input);
            p.Add("@DistrictNameTH", districtNameTH, dbType: DbType.String, direction: ParameterDirection.Input);
            p.Add("@ProvinceId", provinceId, dbType: DbType.String, direction: ParameterDirection.Input);
            p.Add("@ProvinceNameTH", provinceNameTH, dbType: DbType.String, direction: ParameterDirection.Input);
            p.Add("@RegionId", regionId, dbType: DbType.String, direction: ParameterDirection.Input);
            p.Add("@RegionName", regionName, dbType: DbType.String, direction: ParameterDirection.Input);
            p.Add("@GeoGroup", geoGroup, dbType: DbType.String, direction: ParameterDirection.Input);
            p.Add("@GeoSubGroup", geoSubGroup, dbType: DbType.String, direction: ParameterDirection.Input);

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

        public static NDbResult Save(MDistrict value)
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
            p.Add("@DistrictId", value.DistrictId);
            p.Add("@RegionId", value.RegionId);
            p.Add("@ProvinceId", value.ProvinceId);
            p.Add("@DistrictNameTH", value.DistrictNameTH);
            p.Add("@DistrictNameEN", value.DistrictNameEN);
            p.Add("@ADM2Code", value.ADM2Code);

            p.Add("@errNum", dbType: DbType.Int32, direction: ParameterDirection.Output);
            p.Add("@errMsg", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

            try
            {
                cnn.Execute("SaveMDistrict", p, commandType: CommandType.StoredProcedure);
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

        public static NDbResult ImportADM2(MDistrict value)
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
            p.Add("@ProvinceNameTH", value.ProvinceNameTH);
            p.Add("@ProvinceNameEN", value.ProvinceNameEN);
            p.Add("@DistrictNameTH", value.DistrictNameTH);
            p.Add("@DistrictNameEN", value.DistrictNameEN);
            p.Add("@ADM2Code", value.ADM2Code);
            p.Add("@AreaM2", value.DistrictAreaM2);

            p.Add("@errNum", dbType: DbType.Int32, direction: ParameterDirection.Output);
            p.Add("@errMsg", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

            try
            {
                cnn.Execute("ImportMDistrictADM2", p, commandType: CommandType.StoredProcedure);
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
}
