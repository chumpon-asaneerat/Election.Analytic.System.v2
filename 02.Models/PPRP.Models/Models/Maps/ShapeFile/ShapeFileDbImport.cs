#region Using

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

using NLib;
using PPRP.Models;
using PPRP.Models.ShapeFiles;

#endregion

namespace PPRP.Services
{
    public delegate void ProcessShape(int shapeNo, int maxShape, int partNo, int maxPart, int pointNo, int maxPoint);

    /// <summary>
    /// JsonMapFiles Extension Methods class.
    /// </summary>
    public class ShapeFileDbImport
    {
        public void Import(Shapefile shapefile, ProcessShape action)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            if (null == shapefile)
                return;

            foreach (Shape shape in shapefile)
            {
                var ADM0_PCODE = shape.GetMetadata("ADM0_PCODE");
                var ADM1_PCODE = shape.GetMetadata("ADM1_PCODE");
                var ADM2_PCODE = shape.GetMetadata("ADM2_PCODE");
                var ADM3_PCODE = shape.GetMetadata("ADM3_PCODE");

                if (null != ADM3_PCODE)
                {
                    ImportADM3(shape, shapefile.Count, action);
                }
                else if (null != ADM2_PCODE)
                {
                    ImportADM2(shape, shapefile.Count, action);
                }
                else if (null != ADM1_PCODE)
                {
                    ImportADM1(shape, shapefile.Count, action);
                }
                else if (null != ADM0_PCODE)
                {
                    ImportADM0(shape, shapefile.Count, action);
                }
                else
                {
                    Console.WriteLine("Invalid.");
                }
            }
        }

        private void ImportADM0(Shape shape, int shapeCount, ProcessShape action)
        {
            // TODO: Remove supports ShapeType.Point, Handle close window during import.

            if (null == shape) return;
            var ADM0_PCODE = shape.GetMetadata("ADM0_PCODE");
            var ADM0_EN = shape.GetMetadata("ADM0_EN");

            ShapeMapDbService.Instance.Db.BeginTransaction();

            var row = new LADM0();
            // Set Code
            row.ADM0Code = ADM0_PCODE;
            row.CountryNameEN = ADM0_EN;

            var recordId = shape.RecordNumber; // shape number
            var boundRect = new Models.RectangleD();

            #region Extract and convert shape part's points

            // cast shape based on the type
            switch (shape.Type)
            {
                case ShapeType.Point:
                    // a point is just a single x/y point
                    ShapePoint shapePoint = shape as ShapePoint;
                    {
                        var admPart = LADM0Part.Get(row.ADM0Code, recordId).Value();
                        if (null == admPart) admPart = new LADM0Part();
                        admPart.ADM0Code = row.ADM0Code;
                        admPart.RecordId = recordId;
                        admPart.PointCount = 1;

                        LADM0Part.Save(admPart);

                        // Set Bound Rect
                        boundRect.Left = shapePoint.Point.X;
                        boundRect.Top = shapePoint.Point.Y;
                        boundRect.Right = shapePoint.Point.X;
                        boundRect.Bottom = shapePoint.Point.Y;

                        if (null != action)
                        {
                            action(shape.RecordNumber, shapeCount, 1, 1, 1, 1);
                        }
                    }
                    break;
                case ShapeType.Polygon:
                    // a polygon contains one or more parts - each part is a list of points which
                    // are clockwise for boundaries and anti-clockwise for holes 
                    // see http://www.esri.com/library/whitepapers/pdfs/shapefile.pdf
                    ShapePolygon shapePolygon = shape as ShapePolygon;
                    {
                        int iPart = 1;
                        int maxPart = shapePolygon.Parts.Count;
                        foreach (PointD[] part in shapePolygon.Parts)
                        {
                            var admPart = LADM0Part.Get(row.ADM0Code, recordId).Value();
                            if (null == admPart) admPart = new LADM0Part();
                            admPart.ADM0Code = row.ADM0Code;
                            admPart.RecordId = recordId;
                            admPart.PointCount = part.Length;
                            // Save part
                            LADM0Part.Save(admPart);

                            int iCnt = 1;
                            int maxPts = part.Length;
                            foreach (PointD point in part)
                            {
                                var admPoint = LADM0Point.Get(row.ADM0Code, recordId, iCnt).Value();
                                if (null == admPoint) admPoint = new LADM0Point();
                                admPoint.ADM0Code = row.ADM0Code;
                                admPoint.RecordId = recordId;
                                admPoint.PointId = iCnt;
                                admPoint.X = point.X;
                                admPoint.Y = point.Y;
                                // Save point
                                LADM0Point.Save(admPoint);

                                // Set Bound Rect
                                if (boundRect.Left == 0 || point.X < boundRect.Left) boundRect.Left = point.X;
                                if (boundRect.Right == 0 || point.X > boundRect.Right) boundRect.Right = point.X;
                                if (boundRect.Top == 0 || point.Y < boundRect.Top) boundRect.Top = point.Y;
                                if (boundRect.Bottom == 0 || point.Y > boundRect.Bottom) boundRect.Bottom = point.Y;

                                if (null != action)
                                {
                                    action(shape.RecordNumber, shapeCount, iPart, maxPart, iCnt, maxPts);
                                }

                                iCnt++;
                            }

                            iPart++;
                        }
                    }
                    break;
                default:
                    // other not supports.
                    if (null != action)
                    {
                        action(shape.RecordNumber, shapeCount, 0, 0, 0, 0);
                    }
                    break;
            }
            // append to shape list.
            //file.Shapes.Add(jshape);

            // Set Bound Rect
            row.LF = boundRect.Left;
            row.TP = boundRect.Top;
            row.RT = boundRect.Right;
            row.BT = boundRect.Bottom;

            // Save General Information.
            LADM0.Save(row);

            ShapeMapDbService.Instance.Db.Commit();

            #endregion
        }

        private void ImportADM1(Shape shape, int shapeCount, ProcessShape action)
        {
            if (null == shape) return;
            var ADM0_PCODE = shape.GetMetadata("ADM0_PCODE");
            var ADM1_PCODE = shape.GetMetadata("ADM1_PCODE");
            var ADM1_EN = shape.GetMetadata("ADM1_EN");

            ShapeMapDbService.Instance.Db.BeginTransaction();

            var row = new LADM1();
            // Set Code
            row.ADM0Code = ADM0_PCODE;
            row.ADM1Code = ADM1_PCODE;
            row.ProvinceNameEN = ADM1_EN;

            var recordId = shape.RecordNumber; // shape number
            var boundRect = new Models.RectangleD();


            // Set Bound Rect
            row.LF = boundRect.Left;
            row.TP = boundRect.Top;
            row.RT = boundRect.Right;
            row.BT = boundRect.Bottom;
            // Save General Information.
            LADM1.Save(row);

            ShapeMapDbService.Instance.Db.Commit();
        }

        private void ImportADM2(Shape shape, int shapeCount, ProcessShape action)
        {
            if (null == shape) return;
            var ADM0_PCODE = shape.GetMetadata("ADM0_PCODE");
            var ADM1_PCODE = shape.GetMetadata("ADM1_PCODE");
            var ADM2_PCODE = shape.GetMetadata("ADM2_PCODE");
            var ADM2_EN = shape.GetMetadata("ADM2_EN");

            ShapeMapDbService.Instance.Db.BeginTransaction();

            var row = new LADM2();
            // Set Code
            row.ADM0Code = ADM0_PCODE;
            row.ADM1Code = ADM1_PCODE;
            row.ADM2Code = ADM2_PCODE;
            row.DistrictNameEN = ADM2_EN;

            var recordId = shape.RecordNumber; // shape number
            var boundRect = new Models.RectangleD();


            // Set Bound Rect
            row.LF = boundRect.Left;
            row.TP = boundRect.Top;
            row.RT = boundRect.Right;
            row.BT = boundRect.Bottom;
            // Save General Information.
            LADM2.Save(row);

            ShapeMapDbService.Instance.Db.Commit();
        }

        private void ImportADM3(Shape shape, int shapeCount, ProcessShape action)
        {
            if (null == shape) return;
            var ADM0_PCODE = shape.GetMetadata("ADM0_PCODE");
            var ADM1_PCODE = shape.GetMetadata("ADM1_PCODE");
            var ADM2_PCODE = shape.GetMetadata("ADM2_PCODE");
            var ADM3_PCODE = shape.GetMetadata("ADM3_PCODE");
            var ADM3_EN = shape.GetMetadata("ADM3_EN");

            ShapeMapDbService.Instance.Db.BeginTransaction();

            var row = new LADM3();
            // Set Code
            row.ADM0Code = ADM0_PCODE;
            row.ADM1Code = ADM1_PCODE;
            row.ADM2Code = ADM2_PCODE;
            row.ADM3Code = ADM3_PCODE;
            row.SubdistrictNameEN = ADM3_EN;

            var recordId = shape.RecordNumber; // shape number
            var boundRect = new Models.RectangleD();


            // Set Bound Rect
            row.LF = boundRect.Left;
            row.TP = boundRect.Top;
            row.RT = boundRect.Right;
            row.BT = boundRect.Bottom;
            // Save General Information.
            LADM3.Save(row);

            ShapeMapDbService.Instance.Db.Commit();
        }
    }
}
