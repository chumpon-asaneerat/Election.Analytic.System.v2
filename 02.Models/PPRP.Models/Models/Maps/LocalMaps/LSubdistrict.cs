#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

using NLib;
using NLib.Design;
using NLib.Reflection;

using SQLite;
using SQLiteNetExtensions.Attributes;
using SQLiteNetExtensions.Extensions;
// required for JsonIgnore attribute.
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

#endregion

namespace PPRP.Models
{
    #region LSubdistrict

    /// <summary>
    /// The LSubdistrict class
    /// </summary>
    public class LSubdistrict
    {
        #region Public Properties

        /// <summary>
        /// Gets or set Id.
        /// </summary>
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets RegionId.
        /// </summary>
        [MaxLength(20)]
        public string RegionId { get; set; }
        /// <summary>
        /// Gets or sets ADM1Code.
        /// </summary>
        [MaxLength(20)]
        public string ADM1Code { get; set; }
        /// <summary>
        /// Gets or sets ADM2Code.
        /// </summary>
        [MaxLength(20)]
        public string ADM2Code { get; set; }
        /// <summary>
        /// Gets or sets ADM3Code.
        /// </summary>
        [MaxLength(20)]
        public string ADM3Code { get; set; }
        /// <summary>
        /// Gets or sets SubdistrictName.
        /// </summary>
        [MaxLength(200)]
        public string SubdistrictName { get; set; }

        #endregion

        #region Static Methods

        #endregion
    }

    #endregion
}
