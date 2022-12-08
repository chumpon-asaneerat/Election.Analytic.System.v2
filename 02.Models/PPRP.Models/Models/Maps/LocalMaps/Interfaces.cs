#region Using

using System.Collections.Generic;

#endregion

namespace PPRP.Models
{
    #region Interfaces

    public interface IBound
    {
        double LF { get; set; }
        double TP { get; set; }
        double RT { get; set; }
        double BT { get; set; }
        double WD { get; set; }
        double HT { get; set; }
        double CX { get; set; }
        double CY { get; set; }
    }

    public interface IADM : IBound
    {
        string ADMCode { get; }
        string Name { get; }

        List<IADMPart> GetADMParts();
    }

    public interface IADMPart
    {
        int RecordId { get; set; }
        int PartId { get; set; }

        List<IADMPoint> GetADMPoints();
    }

    public interface IADMPoint
    {
        int RecordId { get; set; }
        int PartId { get; set; }
        int PointId { get; set; }
        double X { get; set; }
        double Y { get; set; }
    }

    #endregion
}
