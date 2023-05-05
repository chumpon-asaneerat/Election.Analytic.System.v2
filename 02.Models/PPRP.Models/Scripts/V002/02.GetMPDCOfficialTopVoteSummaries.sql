-- =============================================
-- Author: Chumpon Asaneerat
-- Description:	GetMPDCOfficialTopVoteSummaries
-- [== History ==]
-- <2022-09-29> :
--	- Stored Procedure Created.
--
-- [== Example ==]
--
-- =============================================
ALTER PROCEDURE [dbo].[GetMPDCOfficialTopVoteSummaries]
(
  @ThaiYear int
, @PrevThaiYear int
, @ADM1Code nvarchar(20)
, @PollingUnitNo int
, @Top int = 6
)
AS
BEGIN
DECLARE @sqlCommand as nvarchar(MAX);
    SET @sqlCommand = N'
    ;WITH Top6VoteSum66 AS
    (
        SELECT A.ThaiYear
		     , A.ProvinceId
             , A.ADM1Code
             , A.ProvinceNameTH
             , A.PollingUnitNo
             , A.FullName
             , A.PartyName
             , A.PartyId
             , A.PartyImageData
             , A.PersonId
             , A.PersonImageData
             , A.RankNo
             , A.VoteCount
             , A.RevoteNo
             , A.SortOrder
			 , B.ADM1Code AS PrevADM1Code
			 , B.ProvinceNameTH AS PrevProvinceNameTH
			 , B.PollingUnitNo AS PrevPollingUnitNo
			 , B.PartyId AS PrevPartyId
			 , B.PartyName AS PrevPartyName
			 , B.VoteCount AS PrevVoteCount
			 , B.RankNo AS PrevRankNo
          FROM MPDCOfficialView A LEFT OUTER JOIN MPDVoteSummaryView B 
		    ON B.PersonId = A.PersonId AND B.ThaiYear = ' + CONVERT(nvarchar, @PrevThaiYear) + '
		 WHERE A.ThaiYear = ' + CONVERT(nvarchar, @ThaiYear) + '
    )
    SELECT TOP ' + CONVERT(nvarchar, @Top) + ' *
      FROM Top6VoteSum66
     WHERE ThaiYear = ' + CONVERT(nvarchar, @ThaiYear) + '
       AND ADM1Code = N''' + @ADM1Code + '''
       AND PollingUnitNo = ' + CONVERT(nvarchar, @PollingUnitNo) + '
     ORDER BY VoteCount DESC, SortOrder ASC
    ';
    EXECUTE dbo.sp_executesql @sqlCommand
END;