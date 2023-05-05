-- =============================================
-- Author: Chumpon Asaneerat
-- Description:	UpdateMPDCOfficialVoteCount
-- [== History ==]
-- <2022-09-29> :
--	- Stored Procedure Created.
--
-- [== Example ==]
-- 
-- -- =============================================
CREATE PROCEDURE [dbo].[UpdateMPDCOfficialVoteCount] (
  @ThaiYear int    
, @ADM1Code nvarchar(20)
, @PollingUnitNo int
, @RevoteNo int = 0
, @PartyId int
, @PersonId int
, @VoteCount int = 0
, @errNum as int = 0 out
, @errMsg as nvarchar(MAX) = N'' out)
AS
BEGIN
	BEGIN TRY
		IF (   @ThaiYear IS NULL 
            OR @ADM1Code IS NULL 
            OR @PollingUnitNo IS NULL 
            OR @RevoteNo IS NULL
            OR @PartyId IS NULL
            OR @PersonId IS NULL
           )
		BEGIN
			SET @errNum = 100;
			SET @errMsg = 'Some parameter(s) is null';
			RETURN
		END

        UPDATE MPDCOfficial
           SET VoteCount = COALESCE(@VoteCount, VoteCount)
        WHERE ThaiYear = @ThaiYear
          AND UPPER(LTRIM(RTRIM(ADM1Code))) = UPPER(LTRIM(RTRIM(@ADM1Code)))
          AND PollingUnitNo = @PollingUnitNo
          AND RevoteNo = @RevoteNo
          AND PartyId = @PartyId
          AND PersonId = @PersonId

		-- Update Error Status/Message
		SET @errNum = 0;
		SET @errMsg = 'Success';
	END TRY
	BEGIN CATCH
		SET @errNum = ERROR_NUMBER();
		SET @errMsg = ERROR_MESSAGE();
	END CATCH
END;
