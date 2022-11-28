/* ------------------------------------------------------------------------
 * (c)copyright 2009-2019 Robert Ellison and contributors - https://github.com/abfo/shapefile
 * Provided under the ms-PL license, see LICENSE.txt
 * ------------------------------------------------------------------------ */

#region Using

using System;

#endregion

namespace PPRP.Models.ShapeFiles
{
    #region PointD Struct

    /// <summary>
    /// A simple double precision point
    /// </summary>
    public struct PointD
    {
        #region Public Fields

        /// <summary>Gets or sets the X value</summary>
        public double X;
        /// <summary>Gets or sets the Y value</summary>
        public double Y;

        #endregion

        #region Constructor

        /// <summary>
        /// A simple double precision point
        /// </summary>
        /// <param name="x">X value</param>
        /// <param name="y">Y value</param>
        public PointD(double x, double y)
        {
            X = x;
            Y = y;
        }

        #endregion
    }

    #endregion
}
