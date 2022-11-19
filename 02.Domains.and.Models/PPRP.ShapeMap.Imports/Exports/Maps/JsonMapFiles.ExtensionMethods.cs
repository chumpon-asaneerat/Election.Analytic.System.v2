#region Using

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

using NLib;
using PPRP;
using PPRP.Imports.ShapeFiles;
using PPRP.Models.Maps;

#endregion

namespace PPRP.Exports.Maps
{
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
}
