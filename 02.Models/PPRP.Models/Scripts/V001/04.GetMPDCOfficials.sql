-- =============================================
-- Author: Chumpon Asaneerat
-- Description:	GetMPDCOfficials
-- [== History ==]
-- <2022-09-29> :
--	- Stored Procedure Created.
-- <2022-10-09> :
--	- Add PartyNamne parameter.
--	- Add FullNamne parameter.
--
-- [== Example ==]
--
-- =============================================
CREATE PROCEDURE [dbo].[GetMPDCOfficials]
(
  @ThaiYear int
, @RegionId nvarchar(20) = NULL
, @RegionName nvarchar(200) = NULL
, @ProvinceNameTH nvarchar(100) = NULL
, @PartyName nvarchar(200) = NULL
, @FullName nvarchar(MAX) = NULL
)
AS
BEGIN
    ;WITH MPDCOFF
    AS
    -- Define the Vote Summary by Province and PollingUnit query.
    (
        SELECT RowNo
		     , RankNo
			 , ThaiYear
			 , ADM1Code 
			 , PollingUnitNo
			 , SortOrder
			 , RevoteNo
			 , VoteCount
			 , PartyId
			 , PartyName
			 --, PartyImageData
			 , PersonId
			 , Prefix
			 , FirstName
			 , LastName
			 , FullName
			 --, PersonImageData
			 , DOB
			 , GenderId
			 , EducationId
			 , OccupationId
			 , PersonRemark
			 , ProvinceId 
			 , ProvinceNameTH
			 , ProvinceNameEN 
			 , RegionId
			 , RegionName
			 , GeoGroup
			 , GeoSubgroup
          FROM MPDCOfficialView
         WHERE ThaiYear = @ThaiYear
		   AND UPPER(LTRIM(RTRIM(RegionId))) LIKE '%' + UPPER(LTRIM(RTRIM(COALESCE(@RegionId, RegionId)))) + '%'
		   AND UPPER(LTRIM(RTRIM(RegionName))) LIKE '%' + UPPER(LTRIM(RTRIM(COALESCE(@RegionName, RegionName)))) + '%'
		   AND UPPER(LTRIM(RTRIM(ProvinceNameTH))) LIKE '%' + UPPER(LTRIM(RTRIM(COALESCE(@ProvinceNameTH, ProvinceNameTH)))) + '%'
		   AND UPPER(LTRIM(RTRIM(PartyName))) LIKE '%' + UPPER(LTRIM(RTRIM(COALESCE(@PartyName, PartyName)))) + '%'
		   AND (
                    FullName IS NULL
                 OR UPPER(LTRIM(RTRIM(FullName))) LIKE '%' + UPPER(LTRIM(RTRIM(COALESCE(@FullName, FullName)))) + '%'
               )
    )
    SELECT * 
      FROM MPDCOFF 
     ORDER BY ProvinceNameTH, PollingUnitNo, VoteCount DESC, SortOrder

END;
