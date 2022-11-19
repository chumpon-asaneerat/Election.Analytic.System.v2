#region Using

using System.Collections.Generic;
using Newtonsoft.Json;
using PPRP;

#endregion

namespace PPRP.Models.Maps
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
}
