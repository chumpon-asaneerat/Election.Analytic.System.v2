#region Using

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using Newtonsoft.Json;

using NLib;
using NLib.Reflection;
using PPRP.Models.ShapeFiles;

#endregion

namespace PPRP.Models
{
    #region Json Shape classes

    #region MapShapeType

    /// <summary>
    /// The MapShapeType of a shape in a Shapefile
    /// </summary>
    public enum MapShapeType
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

    #region RectangleD

    /// <summary>
    /// RectangleD class.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class RectangleD
    {
        public double Left { get; set; }
        public double Top { get; set; }
        public double Right { get; set; }
        public double Bottom { get; set; }
    }

    #endregion

    #region JsonShapeFile

    /// <summary>
    /// JsonShapeFile Class.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class JsonShapeFile
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public JsonShapeFile() : base()
        {
            this.Bound = new RectangleD();
            this.Shapes = new List<JsonShape>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets Shape Type.
        /// </summary>
        public MapShapeType Type { get; set; }
        /// <summary>
        /// Gets or sets Number of shapes.
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// Gets or sets Bound.
        /// </summary>
        public RectangleD Bound { get; set; }
        /// <summary>
        /// Gets or sets map shapes.
        /// </summary>
        [JsonProperty(ObjectCreationHandling = ObjectCreationHandling.Replace)]
        public List<JsonShape> Shapes { get; set; }

        #endregion
    }

    #endregion

    #region JsonShape

    /// <summary>
    /// JsonShape class.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class JsonShape
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public JsonShape() : base()
        {
            this.Parts = new List<JsonShapePart>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets Record Number.
        /// </summary>
        public int RecordNo { get; set; }
        /// <summary>
        /// Gets or sets Shape Type.
        /// </summary>
        public MapShapeType ShapeType { get; set; }
        /// <summary>
        /// Gets or sets ADM level 0 EN Name
        /// </summary>
        public string ADM0_EN { get; set; }
        /// <summary>
        /// Gets or sets ADM level 0 Code
        /// </summary>
        public string ADM0_PCODE { get; set; }
        /// <summary>
        /// Gets or sets ADM level 1 EN Name
        /// </summary>
        public string ADM1_EN { get; set; }
        /// <summary>
        /// Gets or sets ADM level 1 Code
        /// </summary>
        public string ADM1_PCODE { get; set; }
        /// <summary>
        /// Gets or sets ADM level 2 EN Name
        /// </summary>
        public string ADM2_EN { get; set; }
        /// <summary>
        /// Gets or sets ADM level 2 Code
        /// </summary>
        public string ADM2_PCODE { get; set; }
        /// <summary>
        /// Gets or sets ADM level 3 EN Name
        /// </summary>
        public string ADM3_EN { get; set; }
        /// <summary>
        /// Gets or sets ADM level 3 Code
        /// </summary>
        public string ADM3_PCODE { get; set; }
        /// <summary>
        /// Gets or sets length.
        /// </summary>
        public double Length { get; set; }
        /// <summary>
        /// Gets or sets map area.
        /// </summary>
        public double Area { get; set; }
        /// <summary>
        /// Gets or sets list of shape parts.
        /// </summary>
        [JsonProperty(ObjectCreationHandling = ObjectCreationHandling.Replace)]
        public List<JsonShapePart> Parts { get; set; }

        #endregion
    }

    #endregion

    #region JsonShapePart

    /// <summary>
    /// JsonShapePart class.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class JsonShapePart
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public JsonShapePart() : base()
        {
            Points = null;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets shape type.
        /// </summary>
        public MapShapeType Type { get; set; }
        /// <summary>
        /// Gets or sets Number of geometry figure on current shape parts.
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// Gets or sets shape point array to construct map geometry path.
        /// </summary>
        [JsonProperty(ObjectCreationHandling = ObjectCreationHandling.Replace)]
        public double[,] Points { get; set; }

        #endregion
    }

    #endregion

    #endregion

    #region JsonMapFiles Extension Methods

    /// <summary>
    /// JsonMapFiles Extension Methods class.
    /// </summary>
    public static class JsonMapFiles
    {
        /// <summary>
        /// Exports.
        /// </summary>
        /// <param name="shapefile">The source Shapefile instance.</param>
        /// <param name="outputPath">The target output directory.</param>
        public static void Export(this Shapefile shapefile, string outputPath)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            if (null == shapefile)
                return;
            if (string.IsNullOrWhiteSpace(outputPath))
                return;
            if (!Directory.Exists(outputPath))
            {
                try { Directory.CreateDirectory(outputPath); }
                catch (Exception ex)
                {
                    med.Err(ex);
                }
            }
            if (!Directory.Exists(outputPath))
                return;

            // enumerate all shapes
            foreach (Shape shape in shapefile)
            {
                #region Prepare file per shape record

                // Prepare each shape record file.
                JsonShapeFile file = new JsonShapeFile();
                file.Type = (MapShapeType)shapefile.Type;
                file.Count = shapefile.Count;
                file.Bound.Left = shapefile.BoundingBox.Left;
                file.Bound.Top = shapefile.BoundingBox.Top;
                file.Bound.Right = shapefile.BoundingBox.Right;
                file.Bound.Bottom = shapefile.BoundingBox.Bottom;

                JsonShape jshape = new JsonShape();
                jshape.RecordNo = shape.RecordNumber;
                jshape.ShapeType = (MapShapeType)shape.Type;
                jshape.Area = Convert.ToDouble(shape.GetMetadata("SHAPE_AREA"));
                jshape.Length = Convert.ToDouble(shape.GetMetadata("SHAPE_LENG"));

                jshape.ADM0_EN = shape.GetMetadata("ADM0_EN");
                jshape.ADM0_PCODE = shape.GetMetadata("ADM0_PCODE");
                jshape.ADM1_EN = shape.GetMetadata("ADM1_EN");
                jshape.ADM1_PCODE = shape.GetMetadata("ADM1_PCODE");
                jshape.ADM2_EN = shape.GetMetadata("ADM2_EN");
                jshape.ADM2_PCODE = shape.GetMetadata("ADM2_PCODE");
                jshape.ADM3_EN = shape.GetMetadata("ADM3_EN");
                jshape.ADM3_PCODE = shape.GetMetadata("ADM3_PCODE");

                #endregion

                #region Extract and convert shape part's points

                // cast shape based on the type
                switch (shape.Type)
                {
                    case ShapeType.Point:
                        // a point is just a single x/y point
                        ShapePoint shapePoint = shape as ShapePoint;
                        {
                            // create new part
                            var jPart = new JsonShapePart();
                            jPart.Count = 1;
                            jPart.Type = MapShapeType.Point;
                            jshape.Parts.Add(jPart);

                            // create point.
                            jPart.Points = new double[1, 2];
                            jPart.Points[0, 0] = shapePoint.Point.X;
                            jPart.Points[0, 1] = shapePoint.Point.Y;
                        }
                        break;
                    case ShapeType.Polygon:
                        // a polygon contains one or more parts - each part is a list of points which
                        // are clockwise for boundaries and anti-clockwise for holes 
                        // see http://www.esri.com/library/whitepapers/pdfs/shapefile.pdf
                        ShapePolygon shapePolygon = shape as ShapePolygon;
                        {
                            foreach (PointD[] part in shapePolygon.Parts)
                            {
                                // create new part
                                var jPart = new JsonShapePart();
                                jPart.Count = part.Length;
                                jPart.Type = MapShapeType.Polygon;
                                jshape.Parts.Add(jPart);

                                // create points.
                                int iCnt = 0;
                                jPart.Points = new double[part.Length, 2];
                                foreach (PointD point in part)
                                {
                                    // assign each point.
                                    jPart.Points[iCnt, 0] = point.X;
                                    jPart.Points[iCnt, 1] = point.Y;
                                    iCnt++;
                                }
                            }
                        }
                        break;
                    default:
                        // other not supports.
                        break;
                }
                // append to shape list.
                file.Shapes.Add(jshape);

                #endregion

                #region Generate file per record

                string path = outputPath;

                if (!string.IsNullOrWhiteSpace(jshape.ADM1_EN))
                {
                    path = Path.Combine(path, jshape.ADM1_EN);
                    if (!Directory.Exists(path))
                    {
                        try { Directory.CreateDirectory(path); }
                        catch (Exception ex1)
                        {
                            med.Err(ex1);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("ADM1EN is Empty");
                }
                if (!string.IsNullOrWhiteSpace(jshape.ADM2_EN))
                {
                    path = Path.Combine(path, jshape.ADM2_EN);
                    if (!Directory.Exists(path))
                    {
                        try { Directory.CreateDirectory(path); }
                        catch (Exception ex2)
                        {
                            med.Err(ex2);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("ADM2EN is Empty");
                }
                /*
                if (!string.IsNullOrWhiteSpace(jshape.ADM3_EN))
                {
                    path = Path.Combine(path, jshape.ADM3_EN);
                    if (!Directory.Exists(path))
                    {
                        try { Directory.CreateDirectory(path); }
                        catch (Exception ex3)
                        {
                            med.Err(ex3);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("ADM3EN is Empty");
                }
                */
                string fileName = string.Empty;
                fileName += jshape.ADM0_EN;
                fileName += string.IsNullOrWhiteSpace(jshape.ADM1_EN) ? string.Empty : "." + jshape.ADM1_EN;
                fileName += string.IsNullOrWhiteSpace(jshape.ADM2_EN) ? string.Empty : "." + jshape.ADM2_EN;
                fileName += string.IsNullOrWhiteSpace(jshape.ADM3_EN) ? string.Empty : "." + jshape.ADM3_EN;

                file.SaveToFile(path + "/" + fileName + ".json", true);

                #endregion
            }
        }
    }

    #endregion
}
