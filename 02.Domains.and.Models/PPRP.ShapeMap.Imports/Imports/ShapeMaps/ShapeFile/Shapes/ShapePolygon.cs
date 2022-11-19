/* ------------------------------------------------------------------------
 * (c)copyright 2009-2019 Robert Ellison and contributors - https://github.com/abfo/shapefile
 * Provided under the ms-PL license, see LICENSE.txt
 * ------------------------------------------------------------------------ */

#region Using

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Data;

#endregion

namespace PPRP.Imports.ShapeFiles
{
    #region ShapePolygon

    /// <summary>
    /// A Shapefile Polygon Shape
    /// </summary>
    public class ShapePolygon : Shape
    {
        #region Internal Variables

        private RectangleD _boundingBox;
        private List<PointD[]> _parts;

        #endregion

        #region Constructor

        /// <summary>
        /// A Shapefile Polygon Shape
        /// </summary>
        /// <param name="recordNumber">The record number in the Shapefile</param>
        /// <param name="metadata">Metadata about the shape</param>
        /// <param name="dataRecord">IDataRecord associated with the metadata</param>
        /// <param name="shapeData">The shape record as a byte array</param>
        /// <exception cref="ArgumentNullException">Thrown if shapeData is null</exception>
        /// <exception cref="InvalidOperationException">Thrown if an error occurs parsing shapeData</exception>
        protected internal ShapePolygon(int recordNumber, StringDictionary metadata, IDataRecord dataRecord, byte[] shapeData)
            : base(ShapeType.Polygon, recordNumber, metadata, dataRecord)
        {
            ParsePolyLineOrPolygon(shapeData, out _boundingBox, out _parts);
        }

        #endregion

        #region Public Properties

        /// <summary>Gets the bounding box</summary>
        public RectangleD BoundingBox { get { return _boundingBox; } }
        /// <summary>
        /// Gets a list of parts (segments) for the PolyLine. Each part
        /// is an array of double precision points
        /// </summary>
        public List<PointD[]> Parts { get { return _parts; } }

        #endregion
    }

    #endregion
}
