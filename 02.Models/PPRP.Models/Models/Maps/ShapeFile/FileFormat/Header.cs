/* ------------------------------------------------------------------------
 * (c)copyright 2009-2019 Robert Ellison and contributors - https://github.com/abfo/shapefile
 * Provided under the ms-PL license, see LICENSE.txt
 * ------------------------------------------------------------------------ */

#region Using

using System;

#endregion

namespace PPRP.Models.ShapeFiles
{
    #region ShapeType

    /// <summary>
    /// The ShapeType of a shape in a Shapefile
    /// </summary>
    public enum ShapeType
    {
        /// <summary>Null Shape</summary>
        Null = 0,
        /// <summary>Point Shape</summary>
        Point = 1,
        /// <summary>PolyLine Shape</summary>
        PolyLine = 3,
        /// <summary>Polygon Shape</summary>
        Polygon = 5,
        /// <summary>MultiPoint Shape</summary>
        MultiPoint = 8,
        /// <summary>PointZ Shape</summary>
        PointZ = 11,
        /// <summary>PolyLineZ Shape</summary>
        PolyLineZ = 13,
        /// <summary>PolygonZ Shape</summary>
        PolygonZ = 15,
        /// <summary>MultiPointZ Shape</summary>
        MultiPointZ = 18,
        /// <summary>PointM Shape</summary>
        PointM = 21,
        /// <summary>PolyLineM Shape</summary>
        PolyLineM = 23,
        /// <summary>PolygonM Shape</summary>
        PolygonM = 25,
        /// <summary>MultiPointM Shape</summary>
        MultiPointM = 28,
        /// <summary>MultiPatch Shape</summary>
        MultiPatch = 31
    }

    #endregion

    #region Header

    /// <summary>
    /// The header data for a Shapefile main file or Index file
    /// </summary>
    class Header
    {
        #region Static Const and Variables

        /// <summary>The length of a Shapefile header in bytes</summary>
        public const int HeaderLength = 100;

        /// <summary>The Expected File Code is 9994</summary>
        private const int ExpectedFileCode = 9994;
        /// <summary>The Expected File Version is 1000</summary>
        private const int ExpectedVersion = 1000;

        #endregion

        #region Constructor

        /// <summary>
        /// The header data for a Shapefile main file or Index file
        /// </summary>
        /// <param name="headerBytes">The first 100 bytes of the Shapefile main file or Index file</param>
        /// <exception cref="ArgumentNullException">Thrown if headerBytes is null</exception>
        /// <exception cref="InvalidOperationException">Thrown if an error occurs parsing the header</exception>
        public Header(byte[] headerBytes)
        {
            if (headerBytes == null)
            {
                throw new ArgumentNullException("headerBytes");
            }

            if (headerBytes.Length != HeaderLength)
            {
                throw new InvalidOperationException(string.Format("headerBytes must be {0} bytes long",
                    HeaderLength));
            }

            // Position  Field           Value       Type        Order
            // Byte 0    File Code       9994        Integer     Big
            // Byte 4    Unused          0           Integer     Big
            // Byte 8    Unused          0           Integer     Big
            // Byte 12   Unused          0           Integer     Big
            // Byte 16   Unused          0           Integer     Big
            // Byte 20   Unused          0           Integer     Big
            // Byte 24   File Length     File Length Integer     Big
            // Byte 28   Version         1000        Integer     Little
            // Byte 32   Shape Type      Shape Type  Integer     Little
            // Byte 36   Bounding Box    Xmin        Double      Little
            // Byte 44   Bounding Box    Ymin        Double      Little
            // Byte 52   Bounding Box    Xmax        Double      Little
            // Byte 60   Bounding Box    Ymax        Double      Little
            // Byte 68*  Bounding Box    Zmin        Double      Little
            // Byte 76*  Bounding Box    Zmax        Double      Little
            // Byte 84*  Bounding Box    Mmin        Double      Little
            // Byte 92*  Bounding Box    Mmax        Double      Little

            FileCode = EndianBitConverter.ToInt32(headerBytes, 0, ProvidedOrder.Big);
            if (FileCode != ExpectedFileCode)
            {
                throw new InvalidOperationException(string.Format("Header File code is {0}, expected {1}",
                    FileCode,
                    ExpectedFileCode));
            }

            Version = EndianBitConverter.ToInt32(headerBytes, 28, ProvidedOrder.Little);
            if (Version != ExpectedVersion)
            {
                throw new InvalidOperationException(string.Format("Header version is {0}, expected {1}",
                    Version,
                    ExpectedVersion));
            }

            FileLength = EndianBitConverter.ToInt32(headerBytes, 24, ProvidedOrder.Big);
            ShapeType = (ShapeType)EndianBitConverter.ToInt32(headerBytes, 32, ProvidedOrder.Little);
            XMin = EndianBitConverter.ToDouble(headerBytes, 36, ProvidedOrder.Little);
            YMin = EndianBitConverter.ToDouble(headerBytes, 44, ProvidedOrder.Little);
            XMax = EndianBitConverter.ToDouble(headerBytes, 52, ProvidedOrder.Little);
            YMax = EndianBitConverter.ToDouble(headerBytes, 60, ProvidedOrder.Little);
            ZMin = EndianBitConverter.ToDouble(headerBytes, 68, ProvidedOrder.Little);
            ZMax = EndianBitConverter.ToDouble(headerBytes, 76, ProvidedOrder.Little);
            MMin = EndianBitConverter.ToDouble(headerBytes, 84, ProvidedOrder.Little);
            MMax = EndianBitConverter.ToDouble(headerBytes, 92, ProvidedOrder.Little);
        }

        #endregion

        #region Public Properties

        /// <summary>Gets the FileCode</summary>
        public int FileCode { get; }
        /// <summary>Gets the file length, in 16-bit words, including the header bytes</summary>
        public int FileLength { get; }
        /// <summary>Gets the file version</summary>
        public int Version { get; }
        /// <summary>Gets the ShapeType contained in this Shapefile</summary>
        public ShapeType ShapeType { get; }
        /// <summary>Gets min x for the bounding box</summary>
        public double XMin { get; }
        /// <summary>Gets min y for the bounding box</summary>
        public double YMin { get; }
        /// <summary>Gets max x for the bounding box</summary>
        public double XMax { get; }
        /// <summary>Gets max y for the bounding box</summary>
        public double YMax { get; }
        /// <summary>Gets min z for the bounding box (0 if unused)</summary>
        public double ZMin { get; }
        /// <summary>Gets max z for the bounding box (0 if unused)</summary>
        public double ZMax { get; }
        /// <summary>Gets min m for the bounding box (0 if unused)</summary>
        public double MMin { get; }
        /// <summary>Gets max m for the bounding box (0 if unused)</summary>
        public double MMax { get; }

        #endregion
    }

    #endregion
}
