/* V1 Script for Habitat Management

Search for the index value to jump to that section of the script.

Index						Search
Tables						1TABLES
Database Valued Functions   2DVF 
Indexes						3INDEXES
Views						4VIEWS
Functions					5FUNCTIONS
Stored Procs				6SP
Triggers					7TRIGGERS
Language independent data	8DATA
SP database version			9VERSION

*/
-------------------------------------------------------------------------------------------------------------------------------
/* 1TABLES */
-------------------------------------------------------------------------------------------------------------------------------

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PermitFormScreenDesignTemplate]') AND TYPE in (N'U'))
BEGIN

    CREATE TABLE [dbo].[PermitFormScreenDesignTemplate](
	    [FormID] [int] IDENTITY(1,1) NOT NULL,
	    [Design] [nvarchar](20) NOT NULL,
	    [Description] [nvarchar](60) NOT NULL,
	    [Active] [bit] NOT NULL DEFAULT ((1)),
	    [CreatedDateTime] [datetime] NOT NULL DEFAULT (getdate()),
	    [LastUpdatedDateTime] [datetime] NOT NULL DEFAULT (getdate()) ,
	    [CreatedBy] [char](10) NOT NULL,
	    [UpdatedBy] [char](10) NOT NULL,
    PRIMARY KEY CLUSTERED 
    (
	    [FormID] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY]

END
GO



IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PermitFormScreenDesignTemplateDetail]') AND TYPE in (N'U'))
BEGIN

CREATE TABLE [dbo].PermitFormScreenDesignTemplateDetail(
	[FormID] [int] NOT NULL,
	[Field] [int] NOT NULL,
	[FieldName] [nvarchar](max) NULL,
	[FieldType] [int] NOT NULL,
	[Section] [nvarchar](20) NOT NULL,
	[Sequence] [int] NOT NULL DEFAULT ((0)),
 CONSTRAINT [PK_PermitFormScreenDesignTemplateDetail] PRIMARY KEY CLUSTERED 
(
	[FormID] ASC,
	[Field] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TemplateFormFieldData]') AND TYPE in (N'U'))
BEGIN

CREATE TABLE [dbo].TemplateFormFieldData(
	[FormID] [int] NOT NULL,
	[Field] [int] NOT NULL,
	[FieldValue] [nvarchar](max) NULL
)
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DigitalSignature]') AND TYPE in (N'U'))
BEGIN

CREATE TABLE [dbo].[DigitalSignature](
	[SignatureID] [int] NOT NULL,
	[UserID] [char](40) NOT NULL,
	[Blob] [image] NULL,
	[DigitalSignatoryTypeSurrogate] [int] NULL,
    [CreationDateTime] [datetime] NOT NULL,
	[LastUpdatedDate] [datetime] NOT NULL DEFAULT (getdate()),
PRIMARY KEY CLUSTERED 
(
	[SignatureID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TemplateFormSection]') AND TYPE in (N'U'))
BEGIN

CREATE TABLE [dbo].TemplateFormSection(
	[FormID] [int] NOT NULL,
	[Section] [nvarchar](20) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[Sequence] [int] NULL
)
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TableFieldTypeMaster]') AND TYPE in (N'U'))
BEGIN

    CREATE TABLE [dbo].[TableFieldTypeMaster](
	    [Id] [int] IDENTITY(1,1) NOT NULL,
	    [Field] [int] NOT NULL,
	    [ColumnName] [nvarchar](max) NULL,
	    [RowCount] [int] NULL,
	    [ColumnType] [int] NULL
		)
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TableFieldTypeMasterData]') AND TYPE in (N'U'))
BEGIN

    CREATE TABLE [dbo].[TableFieldTypeMasterData](
	    [Id] [int] IDENTITY(1,1) NOT NULL,
	    [TableFieldTypeMasterId] [int] NOT NULL,
	    [RowColumnValue] [nvarchar](max) NULL
		)
END
GO


-------------------------------------------------------------------------------------------------------------------------------
/* 2DVF */
-------------------------------------------------------------------------------------------------------------------------------

-------------------------------------------------------------------------------------------------------------------------------
/* 3INDEXES */
-------------------------------------------------------------------------------------------------------------------------------

-------------------------------------------------------------------------------------------------------------------------------
/* 4VIEWS */
-------------------------------------------------------------------------------------------------------------------------------

-------------------------------------------------------------------------------------------------------------------------------
/* 5FUNCTIONS */
-------------------------------------------------------------------------------------------------------------------------------

-------------------------------------------------------------------------------------------------------------------------------
/* 6SP */
-------------------------------------------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[usp_PermitFormScreenDesignTemplateDetail_Update]    Script Date: 08-04-2021 15:51:54 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_PermitFormScreenDesignTemplateDetail_Update]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_PermitFormScreenDesignTemplateDetail_Update]
GO
/****** Object:  StoredProcedure [dbo].[usp_PermitFormScreenDesignTemplateDetail_FetchAll]    Script Date: 08-04-2021 15:51:54 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_PermitFormScreenDesignTemplateDetail_FetchAll]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_PermitFormScreenDesignTemplateDetail_FetchAll]
GO
/****** Object:  StoredProcedure [dbo].[usp_PermitFormScreenDesignTemplateDetail_Fetch]    Script Date: 08-04-2021 15:51:54 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_PermitFormScreenDesignTemplateDetail_Fetch]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_PermitFormScreenDesignTemplateDetail_Fetch]
GO
/****** Object:  StoredProcedure [dbo].[usp_PermitFormScreenDesignTemplateDetail_Add]    Script Date: 08-04-2021 15:51:54 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_PermitFormScreenDesignTemplateDetail_Add]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_PermitFormScreenDesignTemplateDetail_Add]
GO
/****** Object:  StoredProcedure [dbo].[usp_PermitFormScreenDesignTemplate_Update]    Script Date: 08-04-2021 15:51:54 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_PermitFormScreenDesignTemplate_Update]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_PermitFormScreenDesignTemplate_Update]
GO
/****** Object:  StoredProcedure [dbo].[usp_PermitFormScreenDesignTemplate_BlockFetch]    Script Date: 08-04-2021 15:51:54 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_PermitFormScreenDesignTemplate_BlockFetch]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_PermitFormScreenDesignTemplate_BlockFetch]
GO
/****** Object:  StoredProcedure [dbo].[usp_PermitFormScreenDesignTemplate_Add]    Script Date: 08-04-2021 15:51:54 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_PermitFormScreenDesignTemplate_Add]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_PermitFormScreenDesignTemplate_Add]
GO
/****** Object:  StoredProcedure [dbo].[usp_PermitFormScreenDesignTemplate_Fetch]    Script Date: 08-04-2021 15:51:54 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_PermitFormScreenDesignTemplate_Fetch]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_PermitFormScreenDesignTemplate_Fetch]
GO
/****** Object:  StoredProcedure [dbo].[usp_PermitFormScreenDesignTemplateDetail_Delete]    Script Date: 12-04-2021 15:51:54 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_PermitFormScreenDesignTemplateDetail_Delete]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_PermitFormScreenDesignTemplateDetail_Delete]
GO
/****** Object:  StoredProcedure [dbo].[usp_PermitFormScreenDesignTemplateDetail_Delete]    Script Date: 12-04-2021 15:51:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE dbo.[usp_PermitFormScreenDesignTemplateDetail_Delete]
(
	@FormID INT,  
	@Field INT
)
AS
BEGIN
	DELETE FROM PermitFormScreenDesignTemplateDetail WHERE FormID = @FormID AND Field = @Field;
END
GO
/****** Object:  StoredProcedure [dbo].[usp_PermitFormScreenDesignTemplate_Fetch]    Script Date: 08-04-2021 15:51:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_PermitFormScreenDesignTemplate_Fetch]    
(    
 @FormID INT    
)   
AS     
BEGIN    
SET NOCOUNT ON;     
   SELECT t.* FROM PermitFormScreenDesignTemplate AS t  WHERE t.FormID = @FormID  
END  
GO
/****** Object:  StoredProcedure [dbo].[usp_PermitFormScreenDesignTemplate_Add]    Script Date: 08-04-2021 15:51:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
CREATE PROCEDURE [dbo].[usp_PermitFormScreenDesignTemplate_Add]    
(     
    @Design NVARCHAR(max),    
    @Description NVARCHAR(max),    
    @Active BIT,  
    @CreatedDateTime Datetime,    
    @LastUpdatedDateTime Datetime,    
    @CreatedBy CHAR(10),    
    @UpdatedBy CHAR(10),    
    @ErrorOccured BIT OUTPUT,    
    @FormID INT OUTPUT    
)
AS  
BEGIN  
BEGIN TRY    
 BEGIN TRANSACTION trans    
    
  INSERT INTO [PermitFormScreenDesignTemplate]    
  (    
   [Design],    
   [Description],    
   [Active],  
   [CreatedDateTime],    
   [LastUpdatedDateTime],    
   [CreatedBy],    
   [UpdatedBy]    
  )    
  VALUES   
  (    
    @Design,    
    @Description,    
    @Active,  
    @CreatedDateTime,    
    @LastUpdatedDateTime,    
    @CreatedBy,    
    @UpdatedBy    
  )
    
 DECLARE @identity INT    
 SELECT @identity = Scope_Identity();  
   
 COMMIT Transaction trans    
    
 SET @ErrorOccured = 1;    
 SET @FormID = @identity    

END TRY    
BEGIN CATCH    
 IF (@@TRANCOUNT > 0)  
 BEGIN  
  ROLLBACK    
 END  
  
 SET @ErrorOccured = 0;    
 DECLARE @ErrorMessage nvarchar(4000);        
 DECLARE @ErrorSeverity int;    
 DECLARE @ErrorState int;    
    
 SELECT    
 @ErrorMessage = ERROR_MESSAGE(),    
 @ErrorSeverity = ERROR_SEVERITY(),    
 @ErrorState = ERROR_STATE();    
    
 RAISERROR (@ErrorMessage, -- Message text.      
 @ErrorSeverity, -- Severity.      
 @ErrorState -- State.      
 );    
    
END CATCH  
END
GO
/****** Object:  StoredProcedure [dbo].[usp_PermitFormScreenDesignTemplate_BlockFetch]    Script Date: 08-04-2021 15:51:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 CREATE PROCEDURE [dbo].[usp_PermitFormScreenDesignTemplate_BlockFetch]      
 @SearchForm VARCHAR (256) = NULL,              
 @PageIndex INT = 1,      
 @PageSize INT = 10,
 @RecordCount INT OUTPUT  
AS       
BEGIN     
 IF(@PageIndex IS NULL)    
 BEGIN    
  SET @PageIndex = 1       
 END    

 SELECT @RecordCount = COUNT(*) FROM PermitFormScreenDesignTemplate  
 Where ([Design] LIKE '%' + @SearchForm + '%' OR [Description] LIKE '%' + @SearchForm + '%')  
      
 SELECT t.*
 FROM PermitFormScreenDesignTemplate AS t    
 Where (t.[Design] LIKE '%' + @SearchForm + '%' OR t.[Description] LIKE '%' + @SearchForm + '%')   
 ORDER BY [Design]             
 OFFSET @PageSize * (@PageIndex - 1) ROWS            
 FETCH NEXT @PageSize ROWS ONLY;  

END    
GO
/****** Object:  StoredProcedure [dbo].[usp_PermitFormScreenDesignTemplate_Update]    Script Date: 08-04-2021 15:51:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
CREATE PROCEDURE [dbo].[usp_PermitFormScreenDesignTemplate_Update]    
(    
    @FormID INT,    
    @Design NVARCHAR(max),    
    @Description NVARCHAR(max),    
    @Active BIT,  
    @CreatedDateTime Datetime,    
    @LastUpdatedDateTime Datetime,    
    @CreatedBy CHAR(10),    
    @UpdatedBy CHAR(10),    
    @ErrorOccured BIT OUTPUT    
)    
AS  
BEGIN  
 BEGIN TRY  
  BEGIN TRANSACTION trans    
    
  UPDATE [PermitFormScreenDesignTemplate]    
  SET [Design] = @Design,    
  [Description] = @Description,    
  [Active] = @Active,  
  [LastUpdatedDateTime] = @LastUpdatedDateTime,    
  [UpdatedBy] = @UpdatedBy    
  WHERE FormID = @FormID    
       
  COMMIT Transaction trans    
    
  SET @ErrorOccured = 1;   
 END TRY    
 BEGIN CATCH    
  IF (@@TRANCOUNT > 0)  
  BEGIN  
   ROLLBACK    
  END  
  
  SET @ErrorOccured = 0;    
    
  DECLARE @ErrorMessage nvarchar(4000);    
  DECLARE @ErrorSeverity int;       
  DECLARE @ErrorState int;    
    
  SELECT    
  @ErrorMessage = ERROR_MESSAGE(),    
  @ErrorSeverity = ERROR_SEVERITY(),    
  @ErrorState = ERROR_STATE();    
    
  RAISERROR (@ErrorMessage, -- Message text.      
  @ErrorSeverity, -- Severity.      
  @ErrorState -- State.      
  );    
    
 END CATCH  
END  
GO
/****** Object:  StoredProcedure [dbo].[usp_PermitFormScreenDesignTemplateDetail_Add]    Script Date: 08-04-2021 15:51:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
CREATE PROCEDURE [dbo].[usp_PermitFormScreenDesignTemplateDetail_Add]    
(    
    @FormID INT,    
    @FieldName nvarchar(max),    
    @FieldType INT,     
    @Section nvarchar(20),    
    @Sequence INT,
    @Field INT OUT   
)
AS    
BEGIN    

 --DECLARE @Field BIGINT;  
 SELECT @Field = ISNULL(MAX(Field),0) + 1 FROM PermitFormScreenDesignTemplateDetail WHERE FormID = @FormID

 IF(@Sequence = 0)
 BEGIN
     SELECT @Sequence = ISNULL(MAX([Sequence]),0) + 1 FROM PermitFormScreenDesignTemplateDetail WHERE FormID = @FormID AND [Section] = @Section;
 END

     INSERT INTO PermitFormScreenDesignTemplateDetail    
        (FormID,    
        [Field],    
        FieldName,    
        FieldType,     
        [Section],    
        [Sequence]
	  )     
      VALUES (  
		@FormID,    
		@Field,    
		@FieldName,    
		@FieldType,      
		@Section,    
		@Sequence    
		  )    
END    
GO
/****** Object:  StoredProcedure [dbo].[usp_PermitFormScreenDesignTemplateDetail_Fetch]    Script Date: 08-04-2021 15:51:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
CREATE PROCEDURE [dbo].[usp_PermitFormScreenDesignTemplateDetail_Fetch]    
(    
	@FormID INT,   
	@Field INT   
)   
AS     
BEGIN 
	SELECT t.[FormID], t.[Field], t.[FieldName], t.[FieldType], t.[Sequence], t1.[Section], 
	t1.[Description] as SectionDescription, t1.[Sequence] as SectionSequence  
	FROM PermitFormScreenDesignTemplateDetail t JOIN TemplateFormSection t1 
	ON t.Section = t1.Section AND t.[FormID] = t1.[FormID]
    WHERE t1.[FormID] = @FormID AND [Field] = @Field ORDER BY t1.[Sequence], t.[Sequence]  
END  
GO
/****** Object:  StoredProcedure [dbo].[usp_PermitFormScreenDesignTemplateDetail_FetchAll]    Script Date: 08-04-2021 15:51:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
CREATE PROCEDURE [dbo].[usp_PermitFormScreenDesignTemplateDetail_FetchAll]  
(  
	@FormID INT  
)   
AS  
BEGIN  
SET NOCOUNT ON;  
   
	SELECT t.[FormID], t.[Field], t.[FieldName], t.[FieldType], t.[Sequence], t1.[Section], 
	t1.[Description] as SectionDescription, t1.[Sequence] as SectionSequence 
	FROM PermitFormScreenDesignTemplateDetail t RIGHT JOIN TemplateFormSection t1 
	ON t.Section = t1.Section AND t.[FormID] = t1.[FormID] 
    WHERE t1.[FormID] = @FormID AND (ISNULL(t1.[Description],'') != '' OR [Field] IS NOT NULL) ORDER BY t1.[Sequence], t.[Sequence]

END  
GO
/****** Object:  StoredProcedure [dbo].[usp_PermitFormScreenDesignTemplateDetail_Update]    Script Date: 08-04-2021 15:51:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
CREATE PROCEDURE  [dbo].[usp_PermitFormScreenDesignTemplateDetail_Update]    
(    
    @FormID INT,    
    @Field INT,      
    @FieldName nvarchar(max),      
    @FieldType INT,       
    @Section nvarchar(20),        
    @Sequence INT    
)    
AS    
BEGIN    
 DECLARE @PrevSection nvarchar(20),@PrevSequence int;    
 DECLARE @CheckListPrevSection nvarchar(20);
 
 SELECT @CheckListPrevSection = Section FROM PermitFormScreenDesignTemplateDetail
  WHERE FormID = @FormID AND FieldType = 9 AND Field = @Field

 IF NOT EXISTS(SELECT 1 FROM PermitFormScreenDesignTemplateDetail WHERE FormID = @FormID AND Field = @Field     
    AND Section = @Section AND [Sequence] = @Sequence)    
 BEGIN    
  SELECT @PrevSection = Section, @PrevSequence = [Sequence] FROM PermitFormScreenDesignTemplateDetail     
  WHERE FormID = @FormID AND Field = @Field;    
    
  IF(@PrevSection != @Section)    
  BEGIN    
   UPDATE PermitFormScreenDesignTemplateDetail SET     
    [Sequence] = [Sequence] + 1    
    WHERE PermitFormScreenDesignTemplateDetail.FormID = @FormID AND Section = @Section    
    AND [Sequence] >= @Sequence    
    
   UPDATE PermitFormScreenDesignTemplateDetail      
   SET FieldName = @FieldName,      
   FieldType = @FieldType,          
   [Section] = @Section,      
   [Sequence] = @Sequence    
   WHERE FormID = @FormID AND [Field] = @Field    
    
   ;WITH templateField    
   AS    
   (    
    SELECT ROW_NUMBER() OVER(ORDER BY [Sequence]) AS RowNumber, FormID,[Section],[Field] FROM PermitFormScreenDesignTemplateDetail    
    WHERE FormID = @FormID AND Section = @PrevSection    
   )    
    
   UPDATE PermitFormScreenDesignTemplateDetail SET PermitFormScreenDesignTemplateDetail.[Sequence] = templateField.RowNumber    
   FROM PermitFormScreenDesignTemplateDetail INNER JOIN templateField ON     
   PermitFormScreenDesignTemplateDetail.FormID = templateField.FormID     
   AND PermitFormScreenDesignTemplateDetail.Section = templateField.Section    
   AND PermitFormScreenDesignTemplateDetail.[Field] = templateField.[Field];    
  END    
  ELSE    
  BEGIN    
   IF (@PrevSequence > @Sequence) --Increment seq no      
   BEGIN    
    UPDATE PermitFormScreenDesignTemplateDetail SET     
    PermitFormScreenDesignTemplateDetail.[Sequence] = PermitFormScreenDesignTemplateDetail.[Sequence] + 1    
    WHERE PermitFormScreenDesignTemplateDetail.FormID = @FormID AND Section = @Section    
    AND [Sequence] >= @Sequence AND [Sequence] < @PrevSequence    
   END    
   ELSE IF (@PrevSequence < @Sequence) --decrement seq no     
   BEGIN    
    UPDATE PermitFormScreenDesignTemplateDetail SET     
    PermitFormScreenDesignTemplateDetail.[Sequence] = PermitFormScreenDesignTemplateDetail.[Sequence] - 1    
    WHERE PermitFormScreenDesignTemplateDetail.FormID = @FormID AND Section = @Section    
    AND [Sequence] > @PrevSequence AND [Sequence] <= @Sequence    
   END    
    
   UPDATE PermitFormScreenDesignTemplateDetail      
   SET FieldName = @FieldName,      
   FieldType = @FieldType,        
   [Section] = @Section,      
   [Sequence] = @Sequence    
   WHERE FormID = @FormID AND [Field] = @Field    
  END    
 END    
 ELSE    
 BEGIN    
  UPDATE PermitFormScreenDesignTemplateDetail      
  SET FieldName = @FieldName,      
   FieldType = @FieldType,        
  [Section] = @Section,      
  [Sequence] = @Sequence     
  WHERE FormID = @FormID AND [Field] = @Field    
 END    

  -- Check List Type
  IF EXISTS (SELECT 1 FROM PermitFormScreenDesignTemplateDetail WHERE FormID = @FormID AND FieldType = 9 AND Field = @Field)
  BEGIN

    UPDATE PermitFormScreenDesignTemplateDetail
	  SET Section = @Section, [Sequence] = @Sequence
	WHERE FormID = @FormID AND FieldType = 9 AND Section = @CheckListPrevSection

  END
END    
GO


/****** Object:  StoredProcedure [dbo].[usp_DigitalSignature_Fetch]    Script Date: 08-04-2021 15:51:54 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_DigitalSignature_Fetch]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_DigitalSignature_Fetch]
GO
CREATE PROCEDURE [dbo].[usp_DigitalSignature_Fetch]      
  @SignatureID INT  
AS  
BEGIN  
SET NOCOUNT ON;  
   
SELECT DigitalSignature.* FROM DigitalSignature  WHERE SignatureID = @SignatureID   

END  
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_DigitalSignature_Add]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_DigitalSignature_Add]
GO

CREATE PROCEDURE [dbo].[usp_DigitalSignature_Add]  
@SignatureID INT,  
@UserID char(40),  
@CreationDateTime datetime,     
@Blob image,  
@DigitalSignatoryTypeSurrogate int,  
@Surrogate INT OUT   
  
AS  
IF @SignatureID = 0   
BEGIN  
SELECT @SignatureID =  COALESCE(MAX(SignatureID),0) + 1  FROM  DigitalSignature   
END  
 ELSE  
BEGIN  
  SET @SignatureID = @SignatureID  
END  
  
INSERT INTO [dbo].DigitalSignature  
([SignatureID],  
[UserID],  
[Blob],  
 DigitalSignatoryTypeSurrogate,
 [CreationDateTime]
  )  
  
VALUES (  
@SignatureID,  
@UserID,  
@Blob,  
@DigitalSignatoryTypeSurrogate,
@CreationDateTime)  
  
SELECT @Surrogate = @SignatureID  
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_DigitalSignature_Update]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_DigitalSignature_Update]
GO
CREATE PROCEDURE [dbo].[usp_DigitalSignature_Update] 
  @SignatureID INT  
 ,@UserID CHAR(40)
 ,@Blob IMAGE  
 ,@DigitalSignatoryTypeSurrogate INT  
 ,@CreationDateTime datetime
 ,@LastUpdatedDate datetime  
AS  
UPDATE [dbo].DigitalSignature  
SET [UserID] = @UserID 
 ,[Blob] = @Blob  
 ,DigitalSignatoryTypeSurrogate = @DigitalSignatoryTypeSurrogate  
 ,LastUpdatedDate = @LastUpdatedDate
 ,CreationDateTime = @CreationDateTime
WHERE SignatureID = @SignatureID  
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_TemplateFormFieldData_FetchAll]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].usp_TemplateFormFieldData_FetchAll
GO
CREATE PROCEDURE [dbo].usp_TemplateFormFieldData_FetchAll      
(      
 @FormID INT      
)     
AS       
BEGIN      
SET NOCOUNT ON;       
   SELECT t.* FROM TemplateFormFieldData AS t  WHERE t.FormID = @FormID    
END    
GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_TemplateFormSection_FetchAll]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].usp_TemplateFormSection_FetchAll
GO
CREATE PROCEDURE [dbo].usp_TemplateFormSection_FetchAll        
(        
 @FormID INT        
)       
AS         
BEGIN        
SET NOCOUNT ON;         
   SELECT t.* FROM TemplateFormSection AS t  WHERE t.FormID = @FormID ORDER BY [Sequence]     
END      
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_TemplateFormSection_Fetch]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_TemplateFormSection_Fetch]
GO
CREATE PROCEDURE [dbo].[usp_TemplateFormSection_Fetch]        
 @FormID [int] NULL,
 @Section [nvarchar](20) NULL  
AS    
BEGIN    
SET NOCOUNT ON;        
   SELECT t.* FROM TemplateFormSection AS t  WHERE t.FormID = @FormID AND t.Section = @Section ORDER BY [Sequence]
END    
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_TemplateFormSection_Add]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_TemplateFormSection_Add]
GO
CREATE PROCEDURE [dbo].[usp_TemplateFormSection_Add]    
	@FormID [int] NULL,
	@Section [nvarchar](20) NULL,
	@Description [nvarchar](max) NULL,
	@Sequence int NULL  
AS    
BEGIN

	IF(@Sequence = 0)
	BEGIN
		SELECT @Sequence = ISNULL(MAX([Sequence]),0) + 1 FROM TemplateFormSection WHERE FormID = @FormID;
	END

	INSERT INTO [dbo].TemplateFormSection    
	(
		[FormID],    
		[Section],    
		[Description],    
		[Sequence]
	)      
	VALUES (    
		@FormID,    
		@Section,    
		@Description,    
		@Sequence
	)  
END
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_TemplateFormSection_Update]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_TemplateFormSection_Update]
GO
CREATE PROCEDURE [dbo].[usp_TemplateFormSection_Update]    
    @FormID [int] NULL,
    @Section [nvarchar](20) NULL,
    @Description [nvarchar](max) NULL,
    @Sequence int NULL  
AS    
BEGIN

    Update [dbo].TemplateFormSection    
    SET [Description] = @Description,    
    [Sequence] = @Sequence
    WHERE FormID = @FormID AND Section = @Section

END
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_TemplateFormSection_Delete]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_TemplateFormSection_Delete]
GO
CREATE PROCEDURE [dbo].[usp_TemplateFormSection_Delete]  
    @FormID [int] NULL,
    @Section [nvarchar](20) NULL  
AS  
BEGIN

    DELETE FROM TemplateFormSection WHERE FormID = @FormID AND Section = @Section

END  
GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_TemplateFormFieldData_Update]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_TemplateFormFieldData_Update]
GO
CREATE PROCEDURE [dbo].[usp_TemplateFormFieldData_Update]      
(      
    @FormID INT,      
    @Field INT,      
    @FieldValue NVARCHAR(max),        
    @ErrorOccured BIT OUTPUT      
)      
AS    
BEGIN    
 BEGIN TRY    
  BEGIN TRANSACTION trans      
      
  IF EXISTS (select 1 from TemplateFormFieldData WHERE FormID = @FormID AND Field = @Field)
  BEGIN

  UPDATE TemplateFormFieldData      
  SET FieldValue = @FieldValue
  WHERE FormID = @FormID AND Field = @Field    

  END
  ELSE BEGIN

   INSERT INTO TemplateFormFieldData (FormID, Field, FieldValue)      
     SELECT @FormID, @Field, @FieldValue

  END


  COMMIT Transaction trans      
      
  SET @ErrorOccured = 1;     
 END TRY      
 BEGIN CATCH      
  IF (@@TRANCOUNT > 0)    
  BEGIN    
   ROLLBACK      
  END    
    
  SET @ErrorOccured = 0;      
      
  DECLARE @ErrorMessage nvarchar(4000);      
  DECLARE @ErrorSeverity int;         
  DECLARE @ErrorState int;      
      
  SELECT      
  @ErrorMessage = ERROR_MESSAGE(),      
  @ErrorSeverity = ERROR_SEVERITY(),      
  @ErrorState = ERROR_STATE();      
      
  RAISERROR (@ErrorMessage, -- Message text.        
  @ErrorSeverity, -- Severity.        
  @ErrorState -- State.        
  );      
      
 END CATCH    
END    
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_PermitFormScreenDesignTemplateDetail_BlockFetch]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_PermitFormScreenDesignTemplateDetail_BlockFetch]
GO
CREATE PROCEDURE [dbo].[usp_PermitFormScreenDesignTemplateDetail_BlockFetch]        
 @FormID INT,                    
 @PageIndex INT = 1,        
 @PageSize INT = 10,  
 @RecordCount INT OUTPUT    
AS         
BEGIN       
 IF(@PageIndex IS NULL)      
 BEGIN      
  SET @PageIndex = 1         
 END      
  
 SELECT @RecordCount = COUNT(*)  FROM PermitFormScreenDesignTemplateDetail t JOIN TemplateFormSection t1   
 ON t.Section = t1.Section AND t.[FormID] = t1.[FormID]
 WHERE t1.[FormID] = @FormID AND (ISNULL(t1.[Description],'') != '' OR [Field] IS NOT NULL)
        
 SELECT t.[FormID], t.[Field], t.[FieldName], t.[FieldType], t.[Sequence], t1.[Section], 
	t1.[Description] as SectionDescription, t1.[Sequence] as SectionSequence   
 FROM PermitFormScreenDesignTemplateDetail t JOIN TemplateFormSection t1   
 ON t.Section = t1.Section AND t.[FormID] = t1.[FormID]
 WHERE t1.[FormID] = @FormID AND (ISNULL(t1.[Description],'') != '' OR [Field] IS NOT NULL)
 ORDER BY t1.[Sequence], t.[Sequence]           
 OFFSET @PageSize * (@PageIndex - 1) ROWS FETCH NEXT @PageSize ROWS ONLY;    

END      
GO



/****** Object:  StoredProcedure [dbo].[usp_PermitFormScreenDesignTemplateDetail_Fetch]    Script Date: 08-04-2021 15:51:54 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_TableFieldTypeMaster_Fetch]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_TableFieldTypeMaster_Fetch]
GO
CREATE PROCEDURE [dbo].[usp_TableFieldTypeMaster_Fetch]    
(    
 @Id INT,   
 @Field INT   
)   
AS     
BEGIN 
	SELECT * FROM [TableFieldTypeMaster] t WHERE t.[id] = @Id AND [Field] = @Field
END  
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_TableFieldTypeMaster_Add]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_TableFieldTypeMaster_Add]
GO
CREATE PROCEDURE [dbo].[usp_TableFieldTypeMaster_Add]  
@Field [int],
@ColumnName [nvarchar](max) = NULL,
@RowCount [int] = NULL,
@ColumnType [int] = NULL 
  
AS   
  
INSERT INTO [dbo].TableFieldTypeMaster  
(Field,  
ColumnName,  
[RowCount],  
[ColumnType]
)  
VALUES (  
@Field,  
@ColumnName,  
@RowCount,  
@ColumnType)  
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_TableFieldTypeMaster_FetchAll]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].usp_TableFieldTypeMaster_FetchAll
GO
CREATE PROCEDURE [dbo].usp_TableFieldTypeMaster_FetchAll      
(      
 @Field INT      
)     
AS       
BEGIN      
SET NOCOUNT ON;       
   SELECT t.* FROM TableFieldTypeMaster AS t  WHERE t.Field = @Field    
END    
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_TableFieldTypeMaster_Delete]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_TableFieldTypeMaster_Delete]
GO
CREATE PROCEDURE [dbo].[usp_TableFieldTypeMaster_Delete]  
  @Field [int] = NULL 
AS  
BEGIN
    DELETE FROM TableFieldTypeMaster WHERE Field = @Field
END  
GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_TableFieldTypeMasterData_Add]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_TableFieldTypeMasterData_Add]
GO
CREATE PROCEDURE [dbo].[usp_TableFieldTypeMasterData_Add]  
@Id [int] = NULL,
@TableFieldTypeMasterId [int] = NULL,
@RowColumnValue [nvarchar](max) = NULL
  
AS   
  
INSERT INTO [dbo].TableFieldTypeMasterData  
(
Id,  
TableFieldTypeMasterId,  
RowColumnValue
)  
VALUES (  
@Id,  
@TableFieldTypeMasterId,  
@RowColumnValue
)  
  
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_TableFieldTypeMasterData_Delete]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_TableFieldTypeMasterData_Delete]
GO
CREATE PROCEDURE [dbo].[usp_TableFieldTypeMasterData_Delete]  
   @TableFieldTypeMasterId [int] = NULL 
AS  
BEGIN
    DELETE FROM TableFieldTypeMasterData WHERE TableFieldTypeMasterId = @TableFieldTypeMasterId
END  
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_TableFieldTypeMasterData_FetchAll]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].usp_TableFieldTypeMasterData_FetchAll
GO
CREATE PROCEDURE [dbo].usp_TableFieldTypeMasterData_FetchAll      
(      
 @Field INT      
)     
AS       
BEGIN      
SET NOCOUNT ON;       
   SELECT t.* FROM TableFieldTypeMasterData AS t  WHERE t.TableFieldTypeMasterId IN (select Id from TableFieldTypeMaster where @Field = @Field)   
END    
GO

-------------------------------------------------------------------------------------------------------------------------------
/* 7TRIGGERS */
-------------------------------------------------------------------------------------------------------------------------------

-------------------------------------------------------------------------------------------------------------------------------
/* 8DATA */
-------------------------------------------------------------------------------------------------------------------------------

-------------------------------------------------------------------------------------------------------------------------------
/* 9VERSION */
-------------------------------------------------------------------------------------------------------------------------------

----------------------------------------------------------------------------------------------------------------------------------

GO