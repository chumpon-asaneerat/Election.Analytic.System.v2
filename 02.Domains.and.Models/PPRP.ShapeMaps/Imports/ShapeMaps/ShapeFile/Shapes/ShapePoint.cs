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
    #region ShapePoint

    /// <summary>
    /// A Shapefile Point Shape
    /// </summary>
    public class ShapePoint : Shape
    {
        #region Constructor

        /// <summary>
        /// A Shapefile Point Shape
        /// </summary>
        /// <param name="recordNumber">The record number in the Shapefile</param>
        /// <param name="metadata">Metadata about the shape</param>
        /// <param name="dataRecord">IDataRecord associated with the metadata</param>
        /// <param name="shapeData">The shape record as a byte array</param>
        /// <exception cref="ArgumentNullException">Thrown if shapeData is null</exception>
        /// <exception cref="InvalidOperationException">Thrown if an error occurs parsing shapeData</exception>
        protected internal ShapePoint(int recordNumber, StringDictionary metadata, IDataRecord dataRecord, byte[] shapeData)
            : base(ShapeType.Point, recordNumber, metadata, dataRecord)
        {
            // metadata is validated by the base class
            if (shapeData == null)
            {
                throw new ArgumentNullException("shapeData");
            }

            // Note, shapeData includes an 8 byte header so positions below are +8
            // Position     Field       Value   Type        Number  Order
            // Byte 0       Shape Type  1       Integer     1       Little
            // Byte 4       X           X       Double      1       Little
            // Byte 12      Y           Y       Double      1       Little

            // validation - shapedata should be 8 + 4 + 8 + 8 = 28 bytes long
            if (shapeData.Length != 28)
            {
                throw new InvalidOperationException("Invalid shape data");
            }

            Point = new PointD(EndianBitConverter.ToDouble(shapeData, 12, ProvidedOrder.Little),
                EndianBitConverter.ToDouble(shapeData, 20, ProvidedOrder.Little));
        }

        #endregion

        #region Public Properties

        /// <summary>Gets the point</summary>
        public PointD Point { get; }

        #endregion
    }

    #endregion
}
