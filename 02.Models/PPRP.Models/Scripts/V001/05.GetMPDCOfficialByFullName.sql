/****** Object:  StoredProcedure [dbo].[GetMPDCOfficialByFullName]    Script Date: 12/14/2022 11:54:43 AM ******/
-- =============================================
-- Author: Chumpon Asaneerat
-- Description:	GetMPDCOfficialByFullName
-- [== History ==]
-- <2022-09-29> :
--	- Stored Procedure Created.
--
-- [== Example ==]
--
-- =============================================
CREATE PROCEDURE [dbo].[GetMPDCOfficialByFullName]
(
  @ThaiYear int
, @FullName nvarchar(200) = NULL
)
AS
BEGIN
    ;WITH MPDCOFF_A
    AS
    (
        SELECT ThaiYear, ProvinceNameTH, PollingUnitNo 
          FROM MPDCOfficialView 
         WHERE ThaiYear = @ThaiYear
		   AND UPPER(LTRIM(RTRIM(FullName))) LIKE '%' + UPPER(LTRIM(RTRIM(COALESCE(@FullName, FullName)))) + '%' 
    ),
    MPDCOFF_B
    AS
    -- Find the Vote Summary by Province and PollingUnit query.
    (
        SELECT ROW_NUMBER() OVER(ORDER BY A.VoteCount DESC) AS RowNo
				, A.ThaiYear
				, A.RegionId
				, A.RegionName
				, A.GeoGroup
				, A.GeoSubGroup
                , A.ADM1Code
                , A.ProvinceNameTH
                , A.ProvinceNameEN
                , A.ProvinceId
				, A.PollingUnitNo
				, A.SortOrder
				, A.RevoteNo
				, A.RankNo 
				, A.VoteCount
				, A.PartyId
				, A.PartyName
				, A.PartyImageData
				, A.PersonId
				, A.Prefix
				, A.FirstName
				, A.LastName
				, A.FullName
				, A.PersonImageData
				, A.PersonRemark
				, A.DOB
				, A.GenderId
				--, A.GenderName
				, A.EducationId
				--, A.EducationName
				, A.OccupationId
				--, A.OccupationName
            FROM MPDCOfficialView A JOIN MPDCOFF_A B
            ON (
                    A.ThaiYear = B.ThaiYear
		        AND UPPER(LTRIM(RTRIM(A.ProvinceNameTH))) = UPPER(LTRIM(RTRIM(B.ProvinceNameTH)))
                AND A.PollingUnitNo = B.PollingUnitNo
                )
    )
    SELECT * FROM MPDCOFF_B
        WHERE ThaiYear = @ThaiYear
		  AND UPPER(LTRIM(RTRIM(FullName))) LIKE '%' + UPPER(LTRIM(RTRIM(COALESCE(@FullName, FullName)))) + '%' 
    ORDER BY ProvinceNameTH, PollingUnitNo, VoteCount DESC

END;
