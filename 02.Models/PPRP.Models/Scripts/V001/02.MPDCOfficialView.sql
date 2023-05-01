/****** Object:  View [dbo].[MPDCOfficialView]    Script Date: 11/26/2022 1:56:52 PM ******/
CREATE VIEW [dbo].[MPDCOfficialView]
AS
    SELECT ROW_NUMBER() OVER
		   (
			   ORDER BY ThaiYear, ProvinceNameTH, PollingUnitNo, VoteCount DESC, SortOrder ASC
		   ) AS RowNo
		 , ROW_NUMBER() OVER
		   (
		       PARTITION BY ThaiYear, ProvinceNameTH, PollingUnitNo
			   ORDER BY ThaiYear, ProvinceNameTH, PollingUnitNo, VoteCount DESC, SortOrder ASC
		   ) AS RankNo
         , A.ThaiYear
         , A.ADM1Code
         , B.ProvinceId
         , B.ProvinceNameTH
         , B.ProvinceNameEN
         , B.RegionId
         , B.RegionName
         , B.GeoGroup
         , B.GeoSubgroup
	     , A.PollingUnitNo
	     , A.RevoteNo
	     , A.SortOrder
	     , A.PartyId
         , C.PartyName
         , C.[Data] AS PartyImageData
	     , A.PersonId
         , D.Prefix
         , D.FirstName
         , D.LastName
         , LTRIM(RTRIM(
            LTRIM(RTRIM(D.Prefix)) + ' ' + 
            LTRIM(RTRIM(D.FirstName))+ ' ' + 
            LTRIM(RTRIM(D.LastName))
           )) AS FullName
         , D.DOB
         , D.GenderId
         , D.EducationId
         , D.OccupationId
         , D.[Remark] AS PersonRemark
         , D.[Data] AS PersonImageData
	     , A.VoteCount
	  FROM MPDCOfficial A 
        LEFT OUTER JOIN MProvinceView B ON B.ADM1Code = A.ADM1Code
        LEFT OUTER JOIN MParty C ON C.PartyId = A.PartyId
        LEFT OUTER JOIN MPerson D ON D.PersonId = A.PersonId
;
