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
    #region MADM2

    /// <summary>
    /// The MADM2 class.
    /// </summary>
    public class MADM2 : NInpc
    {
        #region Internal Variables

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
