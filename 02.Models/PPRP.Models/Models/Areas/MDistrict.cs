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

        private string _ADM1Code = null;
        private string _ADM2Code = null;

        private string _DistrictNameEN = null;
        private string _DistrictNameTH = null;
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
        /// Gets or sets ADM2 Code.
        /// </summary>
        [ExcelColumn("ADM2_CODE")]
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
        /// Gets or sets Sub District Area M2.
        /// </summary>
        [ExcelColumn("AREA_M2")]
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

        #endregion
    }

    #endregion

    #region MDistrict

    /// <summary>
    /// The MDistrict class.
    /// </summary>
    public class MDistrict : MADM2
    {
        #region Internal Variables

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

        #endregion

        #region Static Methods

        #endregion
    }

    #endregion
}
