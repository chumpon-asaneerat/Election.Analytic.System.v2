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
using System.Windows.Controls.Primitives;
using System.Windows.Forms;

#endregion

namespace PPRP.Models
{
    public class PatchHistory
    {
        #region Public Properties

        public int PatchId { get; set; }
        public string Description { get; set; }

        #endregion

        #region Static Methods (private)

        private static void CreatePatchTable()
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            IDbConnection cnn = DbServer.Instance.Db;
            if (null == cnn || !DbServer.Instance.Connected)
            {
                string msg = "Connection is null or cannot connect to database server.";
                med.Err(msg);
            }
            try
            {
                // Note: cannot use GO in script.
                string cmd = string.Empty;
                cmd += "CREATE TABLE PatchHistory " + Environment.NewLine;
                cmd += "(" + Environment.NewLine;
                cmd += "  PatchId int NOT NULL, " + Environment.NewLine;
                cmd += "  [Description] nvarchar(200) NOT NULL, " + Environment.NewLine;
                cmd += "  CONSTRAINT PK_PatchHistory PRIMARY KEY (PatchId ASC) " + Environment.NewLine;
                cmd += "); " + Environment.NewLine;
                cmd += Environment.NewLine;

                cnn.ExecuteScalar(cmd);
            }
            catch (Exception ex)
            {
                med.Err(ex);
            }
        }

        private static void CheckPatchTable()
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            IDbConnection cnn = DbServer.Instance.Db;
            if (null == cnn || !DbServer.Instance.Connected)
            {
                string msg = "Connection is null or cannot connect to database server.";
                med.Err(msg);
            }
            try
            {
                string cmd = string.Empty;
                cmd += "SELECT COUNT(*) AS CNT " + Environment.NewLine;
                cmd += "  FROM INFORMATION_SCHEMA.TABLES " + Environment.NewLine;
                cmd += " WHERE TABLE_NAME = 'PatchHistory' " + Environment.NewLine;
                cmd += "   AND TABLE_SCHEMA = 'dbo' " + Environment.NewLine;

                int cnt = cnn.Query<int>(cmd).First();
                if (cnt == 0)
                {
                    // Create Table
                    CreatePatchTable();
                }
            }
            catch (Exception ex)
            {
                med.Err(ex);
            }
        }

        private static NDbResult<PatchHistory> Get(int patchId) 
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            NDbResult<PatchHistory> ret = new NDbResult<PatchHistory>();

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
            p.Add("@PatchId", patchId);

            try
            {
                string cmd = string.Empty;
                cmd += "SELECT PatchId, [Description] " + Environment.NewLine;
                cmd += "  FROM PatchHistory " + Environment.NewLine;
                cmd += " WHERE PatchId = @PatchId " + Environment.NewLine;

                var items = cnn.Query<PatchHistory>(cmd, p);
                var data = (null != items) ? items.FirstOrDefault() : null;
                ret.Success(data);
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

        #region Static Methods (Update Script by Version)

        private static void UpdateScriptV1()
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            IDbConnection cnn = DbServer.Instance.Db;
            if (null == cnn || !DbServer.Instance.Connected)
            {
                string msg = "Connection is null or cannot connect to database server.";
                med.Err(msg);
            }

            int id = 1;
            var patch = Get(id).Value();
            if (null != patch) return; // already apply patch.

            try
            {
                string[] resourceNames = new string[]
                {
                    @"PPRP.Scripts.V001.01.MPDCOfficial.sql",
                    @"PPRP.Scripts.V001.02.MPDCOfficialView.sql",
                    @"PPRP.Scripts.V001.03.ImportMPDCOfficial.sql",
                    @"PPRP.Scripts.V001.04.GetMPDCOfficials.sql",
                    @"PPRP.Scripts.V001.05.GetMPDCOfficialByFullName.sql",
                    @"PPRP.Scripts.V001.06.GetMPDCOfficialTopVoteSummaries.sql",
                    @"PPRP.Scripts.V001.07.InitMTitleData.sql",
                };

                foreach (string resourceName in resourceNames)
                {
                    string script = PPRPScriptManager.GetScript(resourceName);
                    if (!string.IsNullOrEmpty(script))
                    {
                        cnn.ExecuteScalar(script);
                    }
                }

                // Update version
                var p = new DynamicParameters();
                p.Add("@PatchId", id);
                p.Add("@description", "Add MPDCOfficial supports");

                cnn.Execute("INSERT INTO PatchHistory(PatchId, [Description]) VALUES(@patchId, @description);", p);
            }
            catch (Exception ex)
            {
                med.Err(ex);
            }
        }

        private static void UpdateScriptV2()
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            IDbConnection cnn = DbServer.Instance.Db;
            if (null == cnn || !DbServer.Instance.Connected)
            {
                string msg = "Connection is null or cannot connect to database server.";
                med.Err(msg);
            }

            int id = 2;
            var patch = Get(id).Value();
            if (null != patch) return; // already apply patch.

            try
            {
                string[] resourceNames = new string[]
                {
                    @"PPRP.Scripts.V001.01.UpdateMPDCOfficialVoteCount.sql",
                    @"PPRP.Scripts.V001.02.GetMPDCOfficialTopVoteSummaries.sql"
                };

                foreach (string resourceName in resourceNames)
                {
                    string script = PPRPScriptManager.GetScript(resourceName);
                    if (!string.IsNullOrEmpty(script))
                    {
                        cnn.ExecuteScalar(script);
                    }
                }

                // Update version
                var p = new DynamicParameters();
                p.Add("@PatchId", id);
                p.Add("@description", "Supports Edit MPDC Official Vote Count");

                cnn.Execute("INSERT INTO PatchHistory(PatchId, [Description]) VALUES(@patchId, @description);", p);
            }
            catch (Exception ex)
            {
                med.Err(ex);
            }
        }

        private static void UpdateScripts()
        {
            UpdateScriptV1();
            UpdateScriptV2();
        }

        #endregion

        #region Static Methods (public)

        public static void ApplyPatch()
        {
            CheckPatchTable();
            UpdateScripts();
        }

        #endregion
    }
}
