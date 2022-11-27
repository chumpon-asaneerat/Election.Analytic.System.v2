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
    #region MADM3

    /// <summary>
    /// The MADM3 class.
    /// </summary>
    public class MADM3 : NInpc
    {
        #region Internal Variables

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

        #endregion

        #region Static Methods

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
