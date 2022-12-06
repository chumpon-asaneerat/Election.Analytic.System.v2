//#define CHECK_EXISTS

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
    public delegate void ProcessADM(int retionNo, int maxRegion, int provinceNo, int maxProvince, 
        int districtNo, int maxDistrict, int subDistrictNo, int maxSubDistrict);

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
            // Only supports ShapeType.Polygon.

            if (null == shape) return;

            var ADM0_PCODE = shape.GetMetadata("ADM0_PCODE");
            var ADM0_EN = shape.GetMetadata("ADM0_EN");

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
                            ShapeMapDbService.Instance.Db.BeginTransaction();

#if CHECK_EXISTS
                            var admPart = LADM0Part.Get(row.ADM0Code, recordId, iPart).Value();
                            if (null == admPart) admPart = new LADM0Part();
#else
                            var admPart = new LADM0Part();
#endif

                            admPart.ADM0Code = row.ADM0Code;
                            admPart.RecordId = recordId;
                            admPart.PartId = iPart;
                            admPart.PointCount = part.Length;
                            // Save part
                            LADM0Part.Save(admPart);

                            int iCnt = 1;
                            int maxPts = part.Length;

                            foreach (PointD point in part)
                            {
#if CHECK_EXISTS
                                var admPoint = LADM0Point.Get(row.ADM0Code, recordId, iPart, iCnt).Value();
                                if (null == admPoint) admPoint = new LADM0Point();
#else
                                var admPoint = new LADM0Point();
#endif

                                admPoint.ADM0Code = row.ADM0Code;
                                admPoint.RecordId = recordId;
                                admPoint.PartId = iPart;
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

                            ShapeMapDbService.Instance.Db.Commit();

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

            // Set Bound Rect
            row.LF = boundRect.Left;
            row.TP = boundRect.Top;
            row.RT = boundRect.Right;
            row.BT = boundRect.Bottom;

            // Save General Information.
            LADM0.Save(row);

            #endregion
        }

        private void ImportADM1(Shape shape, int shapeCount, ProcessShape action)
        {
            // Only supports ShapeType.Polygon.

            if (null == shape) return;

            var ADM0_PCODE = shape.GetMetadata("ADM0_PCODE");
            var ADM1_PCODE = shape.GetMetadata("ADM1_PCODE");
            var ADM1_EN = shape.GetMetadata("ADM1_EN");

            var row = new LADM1();
            // Set Code
            row.ADM0Code = ADM0_PCODE;
            row.ADM1Code = ADM1_PCODE;
            row.ProvinceNameEN = ADM1_EN;

            var recordId = shape.RecordNumber; // shape number
            var boundRect = new Models.RectangleD();

            #region Extract and convert shape part's points

            // cast shape based on the type
            switch (shape.Type)
            {
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
                            ShapeMapDbService.Instance.Db.BeginTransaction();

#if CHECK_EXISTS
                            var admPart = LADM1Part.Get(row.ADM0Code, row.ADM1Code, recordId, iPart).Value();
                            if (null == admPart) admPart = new LADM1Part();
#else
                            var admPart = new LADM1Part();
#endif

                            admPart.ADM0Code = row.ADM0Code;
                            admPart.ADM1Code = row.ADM1Code;
                            admPart.RecordId = recordId;
                            admPart.PartId = iPart;
                            admPart.PointCount = part.Length;
                            // Save part
                            LADM1Part.Save(admPart);

                            int iCnt = 1;
                            int maxPts = part.Length;

                            foreach (PointD point in part)
                            {
#if CHECK_EXISTS
                                var admPoint = LADM1Point.Get(row.ADM0Code, row.ADM1Code, recordId, iPart, iCnt).Value();
                                if (null == admPoint) admPoint = new LADM1Point();
#else
                                var admPoint = new LADM1Point();
#endif

                                admPoint.ADM0Code = row.ADM0Code;
                                admPoint.ADM1Code = row.ADM1Code;
                                admPoint.RecordId = recordId;
                                admPoint.PartId = iPart;
                                admPoint.PointId = iCnt;
                                admPoint.X = point.X;
                                admPoint.Y = point.Y;
                                // Save point
                                LADM1Point.Save(admPoint);

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

                            ShapeMapDbService.Instance.Db.Commit();

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

            // Set Bound Rect
            row.LF = boundRect.Left;
            row.TP = boundRect.Top;
            row.RT = boundRect.Right;
            row.BT = boundRect.Bottom;
            // Save General Information.
            LADM1.Save(row);

            #endregion

            ShapeMapDbService.Instance.Db.Commit();
        }

        private void ImportADM2(Shape shape, int shapeCount, ProcessShape action)
        {
            // Only supports ShapeType.Polygon.

            if (null == shape) return;

            var ADM0_PCODE = shape.GetMetadata("ADM0_PCODE");
            var ADM1_PCODE = shape.GetMetadata("ADM1_PCODE");
            var ADM2_PCODE = shape.GetMetadata("ADM2_PCODE");
            var ADM2_EN = shape.GetMetadata("ADM2_EN");

            var row = new LADM2();
            // Set Code
            row.ADM0Code = ADM0_PCODE;
            row.ADM1Code = ADM1_PCODE;
            row.ADM2Code = ADM2_PCODE;
            row.DistrictNameEN = ADM2_EN;

            var recordId = shape.RecordNumber; // shape number
            var boundRect = new Models.RectangleD();

            // cast shape based on the type
            switch (shape.Type)
            {
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
                            ShapeMapDbService.Instance.Db.BeginTransaction();

#if CHECK_EXISTS
                            var admPart = LADM2Part.Get(row.ADM0Code, row.ADM1Code, row.ADM2Code, recordId, iPart).Value();
                            if (null == admPart) admPart = new LADM2Part();
#else
                            var admPart = new LADM2Part();
#endif

                            admPart.ADM0Code = row.ADM0Code;
                            admPart.ADM1Code = row.ADM1Code;
                            admPart.ADM2Code = row.ADM2Code;
                            admPart.RecordId = recordId;
                            admPart.PartId = iPart;
                            admPart.PointCount = part.Length;
                            // Save part
                            LADM2Part.Save(admPart);

                            int iCnt = 1;
                            int maxPts = part.Length;

                            foreach (PointD point in part)
                            {
#if CHECK_EXISTS
                                var admPoint = LADM2Point.Get(row.ADM0Code, row.ADM1Code, row.ADM2Code, recordId, iPart, iCnt).Value();
                                if (null == admPoint) admPoint = new LADM2Point();
#else
                                var admPoint = new LADM2Point();
#endif

                                admPoint.ADM0Code = row.ADM0Code;
                                admPoint.ADM1Code = row.ADM1Code;
                                admPoint.ADM2Code = row.ADM2Code;
                                admPoint.RecordId = recordId;
                                admPoint.PartId = iPart;
                                admPoint.PointId = iCnt;
                                admPoint.X = point.X;
                                admPoint.Y = point.Y;
                                // Save point
                                LADM2Point.Save(admPoint);

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

                            ShapeMapDbService.Instance.Db.Commit();

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
            // Only supports ShapeType.Polygon.

            if (null == shape) return;

            var ADM0_PCODE = shape.GetMetadata("ADM0_PCODE");
            var ADM1_PCODE = shape.GetMetadata("ADM1_PCODE");
            var ADM2_PCODE = shape.GetMetadata("ADM2_PCODE");
            var ADM3_PCODE = shape.GetMetadata("ADM3_PCODE");
            var ADM3_EN = shape.GetMetadata("ADM3_EN");

            var row = new LADM3();
            // Set Code
            row.ADM0Code = ADM0_PCODE;
            row.ADM1Code = ADM1_PCODE;
            row.ADM2Code = ADM2_PCODE;
            row.ADM3Code = ADM3_PCODE;
            row.SubdistrictNameEN = ADM3_EN;

            var recordId = shape.RecordNumber; // shape number
            var boundRect = new Models.RectangleD();

            // cast shape based on the type
            switch (shape.Type)
            {
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
                            ShapeMapDbService.Instance.Db.BeginTransaction();
#if CHECK_EXISTS
                            var admPart = LADM3Part.Get(row.ADM0Code, row.ADM1Code, row.ADM2Code, row.ADM3Code, recordId, iPart).Value();
                            if (null == admPart) admPart = new LADM3Part();
#else
                            var admPart = new LADM3Part();
#endif
                            admPart.ADM0Code = row.ADM0Code;
                            admPart.ADM1Code = row.ADM1Code;
                            admPart.ADM2Code = row.ADM2Code;
                            admPart.ADM3Code = row.ADM3Code;
                            admPart.RecordId = recordId;
                            admPart.PartId = iPart;
                            admPart.PointCount = part.Length;
                            // Save part
                            LADM3Part.Save(admPart);

                            int iCnt = 1;
                            int maxPts = part.Length;
                            foreach (PointD point in part)
                            {
#if CHECK_EXISTS
                                var admPoint = LADM3Point.Get(row.ADM0Code, row.ADM1Code, row.ADM2Code, row.ADM3Code, recordId, iPart, iCnt).Value();
                                if (null == admPoint) admPoint = new LADM3Point();
#else
                                var admPoint = new LADM3Point();
#endif
                                admPoint.ADM0Code = row.ADM0Code;
                                admPoint.ADM1Code = row.ADM1Code;
                                admPoint.ADM2Code = row.ADM2Code;
                                admPoint.ADM3Code = row.ADM3Code;
                                admPoint.RecordId = recordId;
                                admPoint.PartId = iPart;
                                admPoint.PointId = iCnt;
                                admPoint.X = point.X;
                                admPoint.Y = point.Y;
                                // Save point
                                LADM3Point.Save(admPoint);

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

                            ShapeMapDbService.Instance.Db.Commit();

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

            // Set Bound Rect
            row.LF = boundRect.Left;
            row.TP = boundRect.Top;
            row.RT = boundRect.Right;
            row.BT = boundRect.Bottom;
            // Save General Information.
            LADM3.Save(row);

            ShapeMapDbService.Instance.Db.Commit();
        }

        public void ImportMappings(ProcessADM action)
        {
            if (null == ShapeMapDbService.Instance.Db)
                return;

            MethodBase med = MethodBase.GetCurrentMethod();

            ShapeMapDbService.Instance.Db.BeginTransaction();
            try
            {
                var regions = MRegion.Gets().Value();
                if (null != regions)
                {
                    int iRegion = 1;
                    int iRegionCnt = regions.Count;
                    regions.ForEach(region =>
                    {
                        LPAK.Import(region);
                        var provinces = MProvince.Gets(region.RegionId).Value();
                        if (null != provinces)
                        {
                            int iProvince = 1;
                            int iProvinceCnt = provinces.Count;
                            provinces.ForEach(province => 
                            {
                                LProvince.Import(province);
                                var districts = MDistrict.Gets(region.RegionId, province.ADM1Code).Value();
                                if (null != districts)
                                {
                                    int iDistrict = 1;
                                    int iDistrictCnt = districts.Count;
                                    districts.ForEach(district => 
                                    {
                                        LDistrict.Import(district);
                                        var subdistricts = MSubdistrict.Gets(region.RegionId, 
                                            province.ADM1Code, district.ADM2Code).Value();
                                        if (null != subdistricts)
                                        {
                                            int iSubdistrict = 1;
                                            int iSubdistrictCnt = subdistricts.Count;
                                            subdistricts.ForEach(subdistrict => 
                                            {
                                                LSubdistrict.Import(subdistrict);
                                                if (null != action)
                                                {
                                                    action(iRegion, iRegionCnt, 
                                                        iProvince, iProvinceCnt, 
                                                        iDistrict, iDistrictCnt, 
                                                        iSubdistrict, iSubdistrictCnt);
                                                }
                                            });
                                        }
                                    });
                                }
                            });
                        }
                    });
                }
                ShapeMapDbService.Instance.Db.Commit();
            }
            catch (Exception ex)
            {
                med.Err(ex);

                ShapeMapDbService.Instance.Db.Rollback();
            }
        }
    }
}
