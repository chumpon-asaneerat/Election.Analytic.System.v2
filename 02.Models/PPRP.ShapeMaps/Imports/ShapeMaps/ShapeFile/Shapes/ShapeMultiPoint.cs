﻿/* ------------------------------------------------------------------------
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
    #region ShapeMultiPoint

    /// <summary>
    /// A Shapefile MultiPoint Shape
    /// </summary>
    public class ShapeMultiPoint : Shape
    {
        #region Constructor

        /// <summary>
        /// A Shapefile MultiPoint Shape
        /// </summary>
        /// <param name="recordNumber">The record number in the Shapefile</param>
        /// <param name="metadata">Metadata about the shape</param>
        /// <param name="dataRecord">IDataRecord associated with the metadata</param>
        /// <param name="shapeData">The shape record as a byte array</param>
        /// <exception cref="ArgumentNullException">Thrown if shapeData is null</exception>
        /// <exception cref="InvalidOperationException">Thrown if an error occurs parsing shapeData</exception>
        protected internal ShapeMultiPoint(int recordNumber, StringDictionary metadata
            , IDataRecord dataRecord, byte[] shapeData)
            : base(ShapeType.MultiPoint, recordNumber, metadata, dataRecord)
        {
            // metadata is validated by the base class
            if (shapeData == null)
            {
                throw new ArgumentNullException("shapeData");
            }

            // Note, shapeData includes an 8 byte header so positions below are +8
            // Position     Field       Value       Type        Number      Order
            // Byte 0       Shape Type  8           Integer     1           Little
            // Byte 4       Box         Box         Double      4           Little
            // Byte 36      NumPoints   Num Points  Integer     1           Little
            // Byte 40      Points      Points      Point       NumPoints   Little

            // validation step 1 - must have at least 8 + 4 + (4*8) + 4 bytes = 48
            if (shapeData.Length < 48)
            {
                throw new InvalidOperationException("Invalid shape data");
            }

            // extract bounding box and points
            BoundingBox = ParseBoundingBox(shapeData, 12, ProvidedOrder.Little);
            int numPoints = EndianBitConverter.ToInt32(shapeData, 44, ProvidedOrder.Little);

            // validation step 2 - we're expecting 16 * numPoints + 48 bytes total
            if (shapeData.Length != 48 + (16 * numPoints))
            {
                throw new InvalidOperationException("Invalid shape data");
            }

            // now extract the points
            Points = new PointD[numPoints];
            for (int pointNum = 0; pointNum < numPoints; pointNum++)
            {
                Points[pointNum] = new PointD(EndianBitConverter.ToDouble(shapeData, 48 + (16 * pointNum), ProvidedOrder.Little),
                    EndianBitConverter.ToDouble(shapeData, 56 + (16 * pointNum), ProvidedOrder.Little));
            }

        }

        #endregion

        #region Public Properties

        /// <summary>Gets the bounding box</summary>
        public RectangleD BoundingBox { get; }
        /// <summary>Gets the array of points</summary>
        public PointD[] Points { get; }

        #endregion
    }

    #endregion
}
