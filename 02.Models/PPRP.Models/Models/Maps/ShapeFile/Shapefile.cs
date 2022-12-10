/* ------------------------------------------------------------------------
 * (c)copyright 2009-2019 Robert Ellison and contributors - https://github.com/abfo/shapefile
 * Provided under the ms-PL license, see LICENSE.txt
 * ------------------------------------------------------------------------ */

#region Using

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Drawing;

#endregion

namespace PPRP.Models.ShapeFiles
{
    #region EndianBitConverter classes

    #region ProvidedOrder (Enum)

    /// <summary>
    /// The order of bytes provided to EndianBitConverter
    /// </summary>
    public enum ProvidedOrder
    {
        /// <summary>Value is stored as big-endian</summary>
        Big,
        /// <summary>Value is stored as little-endian</summary>
        Little
    }

    #endregion

    #region EndianBitConverter

    /// <summary>
    /// BitConverter methods that allow a different source byte order (only a subset of BitConverter)
    /// </summary>
    public static class EndianBitConverter
    {
        /// <summary>
        /// Returns an integer from four bytes of a byte array
        /// </summary>
        /// <param name="value">bytes to convert</param>
        /// <param name="startIndex">start index in value</param>
        /// <param name="order">byte order of value</param>
        /// <returns>the integer</returns>
        /// <exception cref="ArgumentNullException">Thrown if value is null</exception>
        /// <exception cref="ArgumentException">Thrown if startIndex is invalid</exception>
        public static int ToInt32(byte[] value, int startIndex, ProvidedOrder order)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            if ((startIndex + sizeof(int)) > value.Length)
            {
                throw new ArgumentException("startIndex invalid (not enough space in value to extract an integer", "startIndex");
            }

            if (BitConverter.IsLittleEndian && (order == ProvidedOrder.Big))
            {
                byte[] toConvert = new byte[sizeof(int)];
                Array.Copy(value, startIndex, toConvert, 0, sizeof(int));
                Array.Reverse(toConvert);
                return BitConverter.ToInt32(toConvert, 0);
            }
            else
            {
                return BitConverter.ToInt32(value, startIndex);
            }
        }

        /// <summary>
        /// Returns a double from eight bytes of a byte array
        /// </summary>
        /// <param name="value">bytes to convert</param>
        /// <param name="startIndex">start index in value</param>
        /// <param name="order">byte order of value</param>
        /// <returns>the double</returns>
        /// <exception cref="ArgumentNullException">Thrown if value is null</exception>
        /// <exception cref="ArgumentException">Thrown if startIndex is invalid</exception>
        public static double ToDouble(byte[] value, int startIndex, ProvidedOrder order)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            if ((startIndex + sizeof(double)) > value.Length)
            {
                throw new ArgumentException("startIndex invalid (not enough space in value to extract a double", "startIndex");
            }

            if (BitConverter.IsLittleEndian && (order == ProvidedOrder.Big))
            {
                byte[] toConvert = new byte[sizeof(double)];
                Array.Copy(value, startIndex, toConvert, 0, sizeof(double));
                Array.Reverse(toConvert);
                return BitConverter.ToDouble(toConvert, 0);
            }
            else
            {
                return BitConverter.ToDouble(value, startIndex);
            }
        }
    }

    #endregion

    #endregion

    #region FileFormat classes

    #region ShapeType enum

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

    #region ShapeFactory

    /// <summary>
    /// Static factory class to create shape objects from a shape record
    /// </summary>
    static class ShapeFactory
    {
        #region Static Methods

        /// <summary>
        /// Creates a Shape object (or derived object) from a shape record
        /// </summary>
        /// <param name="shapeData">The shape record as a byte array</param>
        /// <param name="metadata">Metadata associated with this shape (optional)</param>
        /// <param name="dataRecord">IDataRecord associated with the metadata</param>
        /// <returns>A Shape, or derived class</returns>
        /// <exception cref="ArgumentNullException">Thrown if shapeData or metadata are null</exception>
        /// <exception cref="ArgumentException">Thrown if shapeData is less than 12 bytes long</exception>
        /// <exception cref="InvalidOperationException">Thrown if an error occurs parsing shapeData</exception>
        public static Shape ParseShape(byte[] shapeData, StringDictionary metadata, IDataRecord dataRecord)
        {
            if (shapeData == null)
            {
                throw new ArgumentNullException("shapeData");
            }

            if (shapeData.Length < 12)
            {
                throw new ArgumentException("shapeData must be at least 12 bytes long");
            }

            // shape data contains a header (shape number and content length)
            // the first field in each shape is the shape type

            // Position  Field           Value                   Type        Order
            // Byte 0    Record          Number Record Number    Integer     Big
            // Byte 4    Content Length  Content Length          Integer     Big

            // Position  Field       Value                   Type        Number      Order
            // Byte 0    Shape Type  Shape Type              Integer     1           Little

            int recordNumber = EndianBitConverter.ToInt32(shapeData, 0, ProvidedOrder.Big);
            int contentLengthInWords = EndianBitConverter.ToInt32(shapeData, 4, ProvidedOrder.Big);
            ShapeType shapeType = (ShapeType)EndianBitConverter.ToInt32(shapeData, 8, ProvidedOrder.Little);

            // test that we have the expected amount of data - need to take the 8 byte header into account
            if (shapeData.Length != (contentLengthInWords * 2) + 8)
            {
                throw new InvalidOperationException("Shape data length does not match shape header length");
            }

            Shape shape = null;

            switch (shapeType)
            {
                case ShapeType.Null:
                    shape = new Shape(shapeType, recordNumber, metadata, dataRecord);
                    break;
                case ShapeType.Point:
                    shape = new ShapePoint(recordNumber, metadata, dataRecord, shapeData);
                    break;
                case ShapeType.MultiPoint:
                    shape = new ShapeMultiPoint(recordNumber, metadata, dataRecord, shapeData);
                    break;
                case ShapeType.PolyLine:
                    shape = new ShapePolyLine(recordNumber, metadata, dataRecord, shapeData);
                    break;
                case ShapeType.PolyLineM:
                    shape = new ShapePolyLineM(recordNumber, metadata, dataRecord, shapeData);
                    break;
                case ShapeType.Polygon:
                    shape = new ShapePolygon(recordNumber, metadata, dataRecord, shapeData);
                    break;
                default:
                    throw new NotImplementedException(string.Format("Shapetype {0} is not implemented", shapeType));
            }

            return shape;
        }

        #endregion
    }

    #endregion

    #region ShapeFileEnumerator (private class)

    class ShapeFileEnumerator : IEnumerator<Shape>
    {
        #region Internal Variables

        private OleDbCommand _dbCommand;
        private OleDbDataReader _dbReader;
        private int _currentIndex = -1;
        private bool _rawMetadataOnly;
        private FileStream _mainStream;
        private FileStream _indexStream;
        private int _count;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="dbConnection">The connection string.</param>
        /// <param name="selectString">The select string.</param>
        /// <param name="rawMetadataOnly">True for read metada only.</param>
        /// <param name="mainStream">The shp main file stream.</param>
        /// <param name="indexStream">The shp index file stream.</param>
        /// <param name="count"></param>
        public ShapeFileEnumerator(OleDbConnection dbConnection
            , string selectString
            , bool rawMetadataOnly
            , FileStream mainStream
            , FileStream indexStream
            , int count)
        {
            _rawMetadataOnly = rawMetadataOnly;
            _mainStream = mainStream;
            _indexStream = indexStream;
            _count = count;
            _dbCommand = new OleDbCommand(selectString, dbConnection);
            _dbReader = _dbCommand.ExecuteReader();
        }

        #endregion

        #region IEnumerator<Shape> Members

        /// <summary>
        /// Gets the current shape in the collection
        /// </summary>
        public Shape Current
        {
            get
            {
                // get the metadata
                StringDictionary metadata = null;
                if (!_rawMetadataOnly)
                {
                    metadata = new StringDictionary();
                    for (int i = 0; i < _dbReader.FieldCount; i++)
                    {
                        metadata.Add(_dbReader.GetName(i),
                            _dbReader.GetValue(i).ToString());
                    }
                }

                // get the index record
                byte[] indexHeaderBytes = new byte[8];
                _indexStream.Seek(Header.HeaderLength + _currentIndex * 8, SeekOrigin.Begin);
                _indexStream.Read(indexHeaderBytes, 0, indexHeaderBytes.Length);
                int contentOffsetInWords = EndianBitConverter.ToInt32(indexHeaderBytes, 0, ProvidedOrder.Big);
                int contentLengthInWords = EndianBitConverter.ToInt32(indexHeaderBytes, 4, ProvidedOrder.Big);

                // get the data chunk from the main file - need to factor in 8 byte record header
                int bytesToRead = (contentLengthInWords * 2) + 8;
                byte[] shapeData = new byte[bytesToRead];
                _mainStream.Seek(contentOffsetInWords * 2, SeekOrigin.Begin);
                _mainStream.Read(shapeData, 0, bytesToRead);

                return ShapeFactory.ParseShape(shapeData, metadata, _dbReader);
            }
        }

        #endregion

        #region IEnumerator Members

        /// <summary>
        /// Gets the current item in the collection
        /// </summary>
        object System.Collections.IEnumerator.Current { get { return this.Current; } }

        /// <summary>
        /// Dispose.
        /// </summary>
        public void Dispose()
        {
            _dbReader.Close();
            _dbCommand.Dispose();
        }

        /// <summary>
        /// Move to the next item in the collection (returns false if at the end)
        /// </summary>
        /// <returns>false if there are no more items in the collection</returns>
        public bool MoveNext()
        {
            if (_currentIndex++ < (_count - 1))
            {
                // try to read the next database record
                if (!_dbReader.Read())
                {
                    throw new InvalidOperationException("Metadata database does not contain a record for the next shape");
                }
                return true;
            }
            else
            {
                // reached the last shape
                return false;
            }
        }
        /// <summary>
        /// Reset the enumerator
        /// </summary>
        public void Reset()
        {
            _dbReader.Close();
            _dbReader = _dbCommand.ExecuteReader();
            _currentIndex = -1;
        }

        #endregion
    }

    #endregion

    #endregion

    #region Shapefile

    /// <summary>
    /// Provides a readonly IEnumerable interface to an ERSI Shapefile.
    /// NOTE - has not been designed to be thread safe
    /// </summary>
    /// <remarks>
    /// See the ESRI Shapefile specification at http://www.esri.com/library/whitepapers/pdfs/shapefile.pdf
    /// </remarks>
    public class Shapefile : IDisposable, IEnumerable<Shape>
    {
        #region Static Fields

        /// <summary>
        /// Jet connection string template
        /// </summary>
        public const string ConnectionStringTemplateJet = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=dBase IV";
        /// <summary>
        /// ACE connection string template
        /// </summary>
        public const string ConnectionStringTemplateAce = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=dBase IV";

        #endregion

        #region Internal Variables

        private const string DbSelectStringTemplate = "SELECT * FROM [{0}]";
        private const string MainPathExtension = "shp";
        private const string IndexPathExtension = "shx";
        private const string DbasePathExtension = "dbf";

        private bool _disposed;
        private bool _opened;
        private bool _rawMetadataOnly;
        private int _count;
        private RectangleD _boundingBox;
        private ShapeType _type;
        private string _shapefileMainPath;
        private string _shapefileIndexPath;
        private string _shapefileDbasePath;
        private string _shapefileTempDbasePath;
        private FileStream _mainStream;
        private FileStream _indexStream;
        private Header _mainHeader;
        private Header _indexHeader;
        private OleDbConnection _dbConnection;
        private string _connectionStringTemplate;
        private string _selectString;

        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// Create a new Shapefile object.
        /// </summary>
        public Shapefile()
            : this(null, ConnectionStringTemplateJet) { }
        /// <summary>
        /// Create a new Shapefile object and open a Shapefile. Note that three files are required - 
        /// the main file (.shp), the index file (.shx) and the dBASE table (.dbf). The three files 
        /// must all have the same filename (i.e. shapes.shp, shapes.shx and shapes.dbf). Set path
        /// to any one of these three files to open the Shapefile.
        /// </summary>
        /// <param name="path">Path to the .shp, .shx or .dbf file for this Shapefile</param>
        /// <exception cref="ObjectDisposedException">Thrown if the Shapefile has been disposed</exception>
        /// <exception cref="ArgumentException">Thrown if the path parameter is empty</exception>
        /// <exception cref="FileNotFoundException">Thrown if one of the three required files is not found</exception>
        public Shapefile(string path)
            : this(path, ConnectionStringTemplateJet) { }
        /// <summary>
        /// Create a new Shapefile object and open a Shapefile. Note that three files are required - 
        /// the main file (.shp), the index file (.shx) and the dBASE table (.dbf). The three files 
        /// must all have the same filename (i.e. shapes.shp, shapes.shx and shapes.dbf). Set path
        /// to any one of these three files to open the Shapefile.
        /// </summary>
        /// <param name="path">Path to the .shp, .shx or .dbf file for this Shapefile</param>
        /// <param name="connectionStringTemplate">Connection string template - use Shapefile.ConnectionStringTemplateJet
        /// (the default), Shapefile.ConnectionStringTemplateAce or your own dBASE connection string</param>
        /// <exception cref="ObjectDisposedException">Thrown if the Shapefile has been disposed</exception>
        /// <exception cref="ArgumentNullException">Thrown if the connectionStringTemplate parameter is null</exception>
        /// <exception cref="ArgumentException">Thrown if the path parameter is empty</exception>
        /// <exception cref="FileNotFoundException">Thrown if one of the three required files is not found</exception>
        public Shapefile(string path, string connectionStringTemplate)
        {
            if (connectionStringTemplate == null)
            {
                throw new ArgumentNullException("connectionStringTemplate");
            }

            ConnectionStringTemplate = connectionStringTemplate;

            if (path != null)
            {
                Open(path);
            }
        }
        /// <summary>
        /// Destructor
        /// </summary>
        ~Shapefile()
        {
            Dispose(false);
        }

        #endregion

        #region Private Methods

        private void OpenDb()
        {
            // The drivers for DBF files throw an exception if the filename 
            // is longer than 8 characters - in this case create a temp file
            // for the DB
            string safeDbasePath = _shapefileDbasePath;
            if (Path.GetFileNameWithoutExtension(safeDbasePath).Length > 8)
            {
                // create/delete temp file (we just want a safe path)
                string initialTempFile = Path.GetTempFileName();
                try
                {
                    File.Delete(initialTempFile);
                }
                catch { }

                // set the correct extension
                _shapefileTempDbasePath = Path.ChangeExtension(initialTempFile, DbasePathExtension);

                // copy over the DB
                File.Copy(_shapefileDbasePath, _shapefileTempDbasePath, true);
                safeDbasePath = _shapefileTempDbasePath;
            }

            string connectionString = string.Format(ConnectionStringTemplate,
                Path.GetDirectoryName(safeDbasePath));
            _selectString = string.Format(DbSelectStringTemplate,
                Path.GetFileNameWithoutExtension(safeDbasePath));

            _dbConnection = new OleDbConnection(connectionString);
            _dbConnection.Open();

        }
        private void CloseDb()
        {

            if (_dbConnection != null)
            {
                _dbConnection.Close();
                _dbConnection = null;
            }

            if (_shapefileTempDbasePath != null)
            {
                if (File.Exists(_shapefileTempDbasePath))
                {
                    try
                    {
                        File.Delete(_shapefileTempDbasePath);
                    }
                    catch { }
                }

                _shapefileTempDbasePath = null;
            }
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Dispose the Shapefile and free all resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private void Dispose(bool canDisposeManagedResources)
        {
            if (!_disposed)
            {
                if (canDisposeManagedResources)
                {
                    if (_mainStream != null)
                    {
                        _mainStream.Close();
                        _mainStream = null;
                    }

                    if (_indexStream != null)
                    {
                        _indexStream.Close();
                        _indexStream = null;
                    }

                    CloseDb();
                }

                _disposed = true;
                _opened = false;
            }
        }
        /// <summary>
        /// Get the IEnumerator for this Shapefile
        /// </summary>
        /// <returns>IEnumerator</returns>
        public IEnumerator<Shape> GetEnumerator()
        {
            return new ShapeFileEnumerator(_dbConnection, _selectString, _rawMetadataOnly, _mainStream,
                                          _indexStream, _count);
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Create a new Shapefile object and open a Shapefile. Note that three files are required - 
        /// the main file (.shp), the index file (.shx) and the dBASE table (.dbf). The three files 
        /// must all have the same filename (i.e. shapes.shp, shapes.shx and shapes.dbf). Set path
        /// to any one of these three files to open the Shapefile.
        /// </summary>
        /// <param name="path">Path to the .shp, .shx or .dbf file for this Shapefile</param>
        /// <exception cref="ObjectDisposedException">Thrown if the Shapefile has been disposed</exception>
        /// <exception cref="ArgumentNullException">Thrown if the path parameter is null</exception>
        /// <exception cref="ArgumentException">Thrown if the path parameter is empty</exception>
        /// <exception cref="FileNotFoundException">Thrown if one of the three required files is not found</exception>
        /// <exception cref="InvalidOperationException">Thrown if an error occurs parsing file headers</exception>
        public void Open(string path)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException("Shapefile");
            }

            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            if (path.Length <= 0)
            {
                throw new ArgumentException("path parameter is empty", "path");
            }

            _shapefileMainPath = Path.ChangeExtension(path, MainPathExtension);
            _shapefileIndexPath = Path.ChangeExtension(path, IndexPathExtension);
            _shapefileDbasePath = Path.ChangeExtension(path, DbasePathExtension);

            if (!File.Exists(_shapefileMainPath))
            {
                throw new FileNotFoundException("Shapefile main file not found", _shapefileMainPath);
            }
            if (!File.Exists(_shapefileIndexPath))
            {
                throw new FileNotFoundException("Shapefile index file not found", _shapefileIndexPath);
            }
            if (!File.Exists(_shapefileDbasePath))
            {
                throw new FileNotFoundException("Shapefile dBase file not found", _shapefileDbasePath);
            }

            _mainStream = File.Open(_shapefileMainPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            _indexStream = File.Open(_shapefileIndexPath, FileMode.Open, FileAccess.Read, FileShare.Read);

            if (_mainStream.Length < Header.HeaderLength)
            {
                throw new InvalidOperationException("Shapefile main file does not contain a valid header");
            }

            if (_indexStream.Length < Header.HeaderLength)
            {
                throw new InvalidOperationException("Shapefile index file does not contain a valid header");
            }

            // read in and parse the headers
            byte[] headerBytes = new byte[Header.HeaderLength];
            _mainStream.Read(headerBytes, 0, Header.HeaderLength);
            _mainHeader = new Header(headerBytes);
            _indexStream.Read(headerBytes, 0, Header.HeaderLength);
            _indexHeader = new Header(headerBytes);

            // set properties from the main header
            _type = _mainHeader.ShapeType;
            _boundingBox = new RectangleD(_mainHeader.XMin, _mainHeader.YMin, _mainHeader.XMax, _mainHeader.YMax);

            // index header length is in 16-bit words, including the header - number of 
            // shapes is the number of records (each 4 workds long) after subtracting the header bytes
            _count = (_indexHeader.FileLength - (Header.HeaderLength / 2)) / 4;

            // open the metadata database
            OpenDb();

            _opened = true;
        }
        /// <summary>
        /// Close the Shapefile. Equivalent to calling Dispose().
        /// </summary>
        public void Close()
        {
            Dispose();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the connection string template - use Shapefile.ConnectionStringTemplateJet
        /// (the default), Shapefile.ConnectionStringTemplateAce or your own dBASE connection string
        /// </summary>
        public string ConnectionStringTemplate
        {
            get { return _connectionStringTemplate; }
            set { _connectionStringTemplate = value; }
        }
        /// <summary>
        /// If true then only the IDataRecord (DataRecord) property is available to access metadata for each shape.
        /// If flase (the default) then metadata is also parsed into a string dictionary (use GetMetadataNames() and
        /// GetMetadata() to access)
        /// </summary>
        public bool RawMetadataOnly
        {
            get { return _rawMetadataOnly; }
            set { _rawMetadataOnly = value; }
        }
        /// <summary>Gets the number of shapes in the Shapefile</summary>
        public int Count
        {
            get
            {
                if (_disposed) throw new ObjectDisposedException("Shapefile");
                if (!_opened) throw new InvalidOperationException("Shapefile not open.");

                return _count;
            }
        }
        /// <summary>Gets the bounding box for the Shapefile</summary>
        public RectangleD BoundingBox
        {
            get
            {
                if (_disposed) throw new ObjectDisposedException("Shapefile");
                if (!_opened) throw new InvalidOperationException("Shapefile not open.");

                return _boundingBox;
            }

        }
        /// <summary>Gets the ShapeType of the Shapefile</summary>
        public ShapeType Type
        {
            get
            {
                if (_disposed) throw new ObjectDisposedException("Shapefile");
                if (!_opened) throw new InvalidOperationException("Shapefile not open.");
                return _type;
            }
        }

        #endregion
    }

    #endregion
}
