/****** Object:  StoredProcedure [dbo].[ImportMPDCOfficial]    Script Date: 11/26/2022 3:17:28 PM ******/
-- =============================================
-- Author: Chumpon Asaneerat
-- Description:	ImportMPDCOfficial
-- [== History ==]
-- <2022-09-29> :
--	- Stored Procedure Created.
--
-- [== Example ==]
-- 
-- DECLARE @errNum int
-- DECLARE @errMsg nvarchar(max)
-- DECLARE @ProvinceName nvarchar(200)
-- DECLARE @PollingUnitNo int
-- DECLARE @PartyName nvarchar(200)
-- DECLARE @FullName nvarchar(max)
-- DECLARE @SortOrder int
-- DECLARE @VoteCount int
-- DECLARE @RevoteNo int
-- 
-- SET @ProvinceName = N'เชียงใหม่'
-- SET @PollingUnitNo = 1
-- SET @PartyName = N'เพื่อไทย'
-- SET @FullName = N'นางสาว ทัศนีย์ บูรณุปกรณ์'
-- SET @SortOrder = 1
-- SET @VoteCount = 0
-- SET @RevoteNo = 0
-- 
-- EXEC ImportMPDCOfficial 2566
--                         , @ProvinceName, @PollingUnitNo
-- 						   , @PartyName, @FullName
--                         , @SortOrder
-- 						   , @VoteCount
-- 						   , @RevoteNo
-- 						   , @errNum out, @errMsg out
-- SELECT @errNum as ErrNum, @errMsg as ErrMsg
-- 
-- -- =============================================
CREATE PROCEDURE [dbo].[ImportMPDCOfficial] (
  @ThaiYear int    
, @ProvinceNameTH nvarchar(200)
, @PollingUnitNo int
, @PartyName nvarchar(200)
, @FullName nvarchar(MAX)
, @SortOrder int
, @VoteCount int = 0
, @RevoteNo int = 0
, @errNum as int = 0 out
, @errMsg as nvarchar(MAX) = N'' out)
AS
BEGIN
DECLARE @ADM1Code nvarchar(20)
DECLARE @GenderId int
DECLARE @PartyId int
DECLARE @PersonId int
DECLARE @Prefix nvarchar(MAX) = null
DECLARE @FirstName nvarchar(MAX) = null
DECLARE @LastName nvarchar(MAX) = null
	BEGIN TRY
		IF (   @ThaiYear IS NULL 
            OR @ProvinceNameTH IS NULL 
            OR @PollingUnitNo IS NULL 
            OR @RevoteNo IS NULL
            OR @PartyName IS NULL
            OR @FullName IS NULL
           )
		BEGIN
			SET @errNum = 100;
			SET @errMsg = 'Some parameter(s) is null';
			RETURN
		END

		SELECT @ADM1Code = ADM1Code 
		  FROM MProvince
		 WHERE UPPER(LTRIM(RTRIM(ProvinceNameTH))) = UPPER(LTRIM(RTRIM(@ProvinceNameTH)))

        -- Call Save to get PartyId
        EXEC SaveMParty @PartyName, @PartyId out, @errNum out, @errMsg out

        IF (@errNum <> 0)
        BEGIN
            RETURN
        END

		IF (@ADM1Code IS NULL OR @PartyId IS NULL)
		BEGIN
			SET @errNum = 101;
			SET @errMsg = 'ADM1Code or PartyId is null';
			RETURN
		END

        EXEC Parse_FullName @FullName, @Prefix out, @FirstName out, @LastName out

		IF (@FirstName IS NULL OR @LastName IS NULL)
		BEGIN
			SET @errNum = 102;
			SET @errMsg = 'Cannot Parse FullName.';
			RETURN
		END

        IF (@Prefix IS NOT NULL)
        BEGIN
            SELECT @GenderId = dbo.GetGenderFromTitle(@Prefix)
        END

        -- Call Save to get PersonId
        EXEC SaveMPerson @Prefix, @FirstName, @LastName
                       , NULL -- DOB
                       , @GenderId -- GenderId
                       , NULL -- EducationId
                       , NULL -- OccupationId
                       , NULL -- Remark
                       , @PersonId out -- PersonId
                       , @errNum out, @errMsg out

        IF (@errNum <> 0)
        BEGIN
            RETURN
        END

		IF (@PersonId IS NULL)
		BEGIN
			SET @errNum = 103;
			SET @errMsg = 'Cannot find PersonId.';
			RETURN
		END

        IF (EXISTS(
              SELECT * 
			    FROM MPDCOfficial
			   WHERE ThaiYear = @ThaiYear
                 AND UPPER(LTRIM(RTRIM(ADM1Code))) = UPPER(LTRIM(RTRIM(@ADM1Code)))
                 AND PollingUnitNo = @PollingUnitNo
                 AND RevoteNo = @RevoteNo
                 AND PartyId = @PartyId
           ))
		BEGIN
			  UPDATE MPDCOfficial
			     SET  PersonId = @PersonId
                    , SortOrder = @SortOrder
                    , VoteCount = COALESCE(@VoteCount, VoteCount)
			   WHERE ThaiYear = @ThaiYear
                 AND UPPER(LTRIM(RTRIM(ADM1Code))) = UPPER(LTRIM(RTRIM(@ADM1Code)))
                 AND PollingUnitNo = @PollingUnitNo
                 AND RevoteNo = @RevoteNo
                 AND PartyId = @PartyId
		END
        ELSE
        BEGIN
            INSERT INTO MPDCOfficial
            (
                  ThaiYear
                , ADM1Code
                , PollingUnitNo
                , PartyId
                , PersonId
                , SortOrder
                , RevoteNo
                , VoteCount
            )
            VALUES
            (
                  @ThaiYear
                , LTRIM(RTRIM(@ADM1Code))
                , @PollingUnitNo
                , @PartyId
                , @PersonId
                , @SortOrder
                , @RevoteNo
                , @VoteCount
            )
        END
		-- Update Error Status/Message
		SET @errNum = 0;
		SET @errMsg = 'Success';
	END TRY
	BEGIN CATCH
		SET @errNum = ERROR_NUMBER();
		SET @errMsg = ERROR_MESSAGE();
	END CATCH
END;
