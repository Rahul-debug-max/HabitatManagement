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

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FormDesignTemplate]') AND type in (N'U'))
AND EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PermitFormScreenDesignTemplate]') AND type in (N'U'))
BEGIN
    EXEC sp_RENAME 'PermitFormScreenDesignTemplate' , 'FormDesignTemplate'
END									
GO
									
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FormDesignTemplateDetail]') AND type in (N'U'))
AND EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PermitFormScreenDesignTemplateDetail]') AND type in (N'U'))
BEGIN
    EXEC sp_RENAME 'PermitFormScreenDesignTemplateDetail' , 'FormDesignTemplateDetail'
END								
GO			

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FormDesignTemplate]') AND TYPE in (N'U'))
BEGIN

    CREATE TABLE [dbo].[FormDesignTemplate](
	    [FormID] [int] IDENTITY(1,1) NOT NULL,
	    [Design] [nvarchar](20) NOT NULL,
	    [Description] [nvarchar](60) NOT NULL,
	    [Active] [bit] NOT NULL DEFAULT ((1)),
	    [CreatedDateTime] [datetime] NOT NULL DEFAULT (getdate()),
	    [LastUpdatedDateTime] [datetime] NULL,
	    [CreatedBy] [char](10) NOT NULL,
	    [UpdatedBy] [char](10) NULL,
    PRIMARY KEY CLUSTERED 
    (
	    [FormID] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY]

END
GO



IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FormDesignTemplateDetail]') AND TYPE in (N'U'))
BEGIN

CREATE TABLE [dbo].FormDesignTemplateDetail(
	[FormID] [int] NOT NULL,
	[Field] [int] NOT NULL,
	[FieldName] [nvarchar](max) NULL,
	[FieldType] [int] NOT NULL,
	[Section] [nvarchar](20) NOT NULL,
	[Sequence] [int] NOT NULL DEFAULT ((0)),
 CONSTRAINT [PK_FormDesignTemplateDetail] PRIMARY KEY CLUSTERED 
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
    [ReferenceNumber] [int] NOT NULL,
	[FormID] [int] NOT NULL,
	[Field] [int] NOT NULL,
	[FieldValue] [nvarchar](max) NULL,
    [CreationDate] DATETIME DEFAULT(GETDATE()) NOT NULL
)
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TemplateFormFieldData]') AND TYPE in (N'U'))
BEGIN
	IF (COL_LENGTH('[dbo].[TemplateFormFieldData]','Surrogate') IS NOT NULL)	
	BEGIN
		ALTER TABLE [dbo].[TemplateFormFieldData]
		drop column Surrogate
	END

	IF (COL_LENGTH('[dbo].[TemplateFormFieldData]','ReferenceNumber') IS NULL)	
	BEGIN
		ALTER TABLE [dbo].[TemplateFormFieldData]
		ADD ReferenceNumber [int] DEFAULT(0) NOT NULL
	END

	IF (COL_LENGTH('[dbo].[TemplateFormFieldData]','CreationDate') IS NULL)	
	BEGIN
		ALTER TABLE [dbo].[TemplateFormFieldData]
		ADD [CreationDate] DATETIME DEFAULT(GETDATE()) NOT NULL
	END
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DigitalSignature]') AND TYPE in (N'U'))
BEGIN

CREATE TABLE [dbo].[DigitalSignature](
	[SignatureID] [int] NOT NULL,
	[UserID] [char](40) NOT NULL,
	[Blob] [image] NULL,
	[DigitalSignatoryTypeSurrogate] [int] NULL,
    [CreationDateTime] DATETIME DEFAULT(GETDATE()) NOT NULL,
	[LastUpdatedDate] DATETIME NULL,
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
	[BackgroundColor] [nvarchar](20) NULL,
	[TextColor] [nvarchar](20) NULL,
	[Sequence] [int] NULL
)
END
ELSE
BEGIN
	IF (COL_LENGTH('[dbo].[TemplateFormSection]','BackgroundColor') IS NULL)	
	BEGIN
		ALTER TABLE [dbo].[TemplateFormSection]
		ADD [BackgroundColor] [nvarchar](20) NULL
	END
	IF (COL_LENGTH('[dbo].[TemplateFormSection]','TextColor') IS NULL)	
	BEGIN
		ALTER TABLE [dbo].[TemplateFormSection]
		ADD [TextColor] [nvarchar](20) NULL
	END

	IF COL_LENGTH('[dbo].[TemplateFormSection]','BackgroundColor') IS NOT NULL 
	AND (SELECT DATA_TYPE from INFORMATION_SCHEMA.COLUMNS  where  TABLE_NAME = 'TemplateFormSection' and COLUMN_NAME = 'BackgroundColor') = 'int'
	BEGIN
		ALTER TABLE [dbo].[TemplateFormSection]
		ALTER COLUMN [BackgroundColor] [nvarchar](20) NULL
	END
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

						
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Client]') AND TYPE in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Client](
	    [ID] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	    [Name] [nvarchar](250) NOT NULL
	)
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Project]') AND TYPE in (N'U'))
BEGIN

    CREATE TABLE [dbo].[Project](
	    [ID] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
		[ClientID] INT NULL,
	    [Project] [nvarchar](60) NULL,
		[ProjectName] [nvarchar](256) NOT NULL,
	    [Description] [nvarchar](500) NOT NULL,
	    [Manager] [nvarchar](60) NULL,
		[SiteAddress] [nvarchar](256) NULL,
		[SitePostcode] [nvarchar](60) NULL,
	    [CreatedDateTime] [datetime] NOT NULL DEFAULT (GETDATE()),
	    [LastUpdatedDateTime] [datetime] NULL,
	    [CreatedBy] [char](10) NULL,
	    [UpdatedBy] [char](10) NULL   
    )
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ProjectForm]') AND TYPE in (N'U'))
BEGIN

    CREATE TABLE [dbo].[ProjectForm](
	    [ProjectId] [int] NOT NULL,
	    [FormId] [int] NOT NULL
    PRIMARY KEY CLUSTERED 
    (
	    [ProjectId] ASC,
		[FormId] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY]

END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SubmittedForm]') AND TYPE in (N'U'))
BEGIN

    CREATE TABLE [dbo].[SubmittedForm](
	    [ReferenceNumber] [int] IDENTITY(1,1) NOT NULL,
	    [ProjectId] [int] NOT NULL,
	    [FormId] [int] NOT NULL,
		[Status] [int] NOT NULL,
	    [CreatedDateTime] [datetime] NOT NULL DEFAULT (getdate()),
	    [LastUpdatedDateTime] [datetime] NULL,
	    [CreatedBy] [char](10) NULL,
	    [UpdatedBy] [char](10) NULL,
    PRIMARY KEY CLUSTERED 
    (
	    [ReferenceNumber] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY]
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
IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Split]') AND xtype IN (N'FN', N'IF', N'TF'))
DROP FUNCTION [dbo].[Split]
GO

CREATE FUNCTION [dbo].[Split] 
(
	@Input nvarchar(max),
	@Character char(1)
)
RETURNS @Output TABLE (
  Item nvarchar(1000)
)
AS
BEGIN
	DECLARE @StartIndex int,
			@EndIndex int

	SET @StartIndex = 1
	IF SUBSTRING(@Input, LEN(@Input) - 1, LEN(@Input)) <> @Character
	BEGIN
		SET @Input = @Input + @Character
	END

	WHILE CHARINDEX(@Character, @Input) > 0
	BEGIN
		SET @EndIndex = CHARINDEX(@Character, @Input)

		INSERT INTO @Output (Item)
			SELECT
			SUBSTRING(@Input, @StartIndex, @EndIndex - 1)

		SET @Input = SUBSTRING(@Input, @EndIndex + 1, LEN(@Input))
	END
	
	RETURN
END
GO

-------------------------------------------------------------------------------------------------------------------------------
/* 6SP */
-------------------------------------------------------------------------------------------------------------------------------

---------------------- Start of Drop Extra Sproc from data base ----------------------
/****** Object:  StoredProcedure [dbo].[usp_TemplateFormFieldData_BlockFetch_ByForm]    Script Date: 06-05-2021 15:51:54 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_TemplateFormFieldData_BlockFetch_ByForm]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_TemplateFormFieldData_BlockFetch_ByForm]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_TemplateFormFieldData_Fetch_MaxSurrogate]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].usp_TemplateFormFieldData_Fetch_MaxSurrogate
GO
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
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_PermitFormScreenDesignTemplateDetail_BlockFetch]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_PermitFormScreenDesignTemplateDetail_BlockFetch]
GO		

---------------------- End of Drop Extra Sproc from data base ----------------------


/****** Object:  StoredProcedure [dbo].[usp_FormDesignTemplateDetail_Update]    Script Date: 08-04-2021 15:51:54 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_FormDesignTemplateDetail_Update]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_FormDesignTemplateDetail_Update]
GO
/****** Object:  StoredProcedure [dbo].[usp_FormDesignTemplateDetail_FetchAll]    Script Date: 08-04-2021 15:51:54 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_FormDesignTemplateDetail_FetchAll]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_FormDesignTemplateDetail_FetchAll]
GO
/****** Object:  StoredProcedure [dbo].[usp_FormDesignTemplateDetail_Fetch]    Script Date: 08-04-2021 15:51:54 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_FormDesignTemplateDetail_Fetch]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_FormDesignTemplateDetail_Fetch]
GO
/****** Object:  StoredProcedure [dbo].[usp_FormDesignTemplateDetail_Add]    Script Date: 08-04-2021 15:51:54 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_FormDesignTemplateDetail_Add]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_FormDesignTemplateDetail_Add]
GO
/****** Object:  StoredProcedure [dbo].[usp_FormDesignTemplate_Update]    Script Date: 08-04-2021 15:51:54 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_FormDesignTemplate_Update]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_FormDesignTemplate_Update]
GO
/****** Object:  StoredProcedure [dbo].[usp_FormDesignTemplate_BlockFetch]    Script Date: 08-04-2021 15:51:54 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_FormDesignTemplate_BlockFetch]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_FormDesignTemplate_BlockFetch]
GO
/****** Object:  StoredProcedure [dbo].[usp_FormDesignTemplate_Add]    Script Date: 08-04-2021 15:51:54 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_FormDesignTemplate_Add]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_FormDesignTemplate_Add]
GO
/****** Object:  StoredProcedure [dbo].[usp_FormDesignTemplate_Fetch]    Script Date: 08-04-2021 15:51:54 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_FormDesignTemplate_Fetch]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_FormDesignTemplate_Fetch]
GO
/****** Object:  StoredProcedure [dbo].[usp_FormDesignTemplateDetail_Delete]    Script Date: 12-04-2021 15:51:54 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_FormDesignTemplateDetail_Delete]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_FormDesignTemplateDetail_Delete]
GO
/****** Object:  StoredProcedure [dbo].[usp_FormDesignTemplateDetail_Delete]    Script Date: 12-04-2021 15:51:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE dbo.[usp_FormDesignTemplateDetail_Delete]
(
	@FormID INT,  
	@Field INT
)
AS
BEGIN
	DELETE FROM FormDesignTemplateDetail WHERE FormID = @FormID AND Field = @Field;
END
GO
/****** Object:  StoredProcedure [dbo].[usp_FormDesignTemplate_Fetch]    Script Date: 08-04-2021 15:51:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_FormDesignTemplate_Fetch]    
(    
 @FormID INT    
)   
AS     
BEGIN    
SET NOCOUNT ON;     
   SELECT t.* FROM FormDesignTemplate AS t  WHERE t.FormID = @FormID  
END  
GO
/****** Object:  StoredProcedure [dbo].[usp_FormDesignTemplate_Add]    Script Date: 08-04-2021 15:51:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
CREATE PROCEDURE [dbo].[usp_FormDesignTemplate_Add]    
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
    
  INSERT INTO FormDesignTemplate    
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
/****** Object:  StoredProcedure [dbo].[usp_FormDesignTemplate_BlockFetch]    Script Date: 08-04-2021 15:51:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_FormDesignTemplate_BlockFetch]      
 @SearchForm VARCHAR (256) = NULL,              
 @PageIndex INT = 1,      
 @PageSize INT = 10,
 @ProjectId INT = null,
 @RecordCount INT OUTPUT  
AS       
BEGIN     
 IF(@PageIndex IS NULL)    
 BEGIN    
  SET @PageIndex = 1       
 END    

 IF(@ProjectId IS NULL)    
 BEGIN

 SELECT @RecordCount = COUNT(*) FROM FormDesignTemplate  
 Where ([Design] LIKE '%' + @SearchForm + '%' OR [Description] LIKE '%' + @SearchForm + '%')  
      
 SELECT t.*
 FROM FormDesignTemplate AS t    
 Where (t.[Design] LIKE '%' + @SearchForm + '%' OR t.[Description] LIKE '%' + @SearchForm + '%')   
 ORDER BY [Design]             
 OFFSET @PageSize * (@PageIndex - 1) ROWS FETCH NEXT @PageSize ROWS ONLY;  

 END
 ELSE BEGIN

 SELECT @RecordCount = COUNT(*) FROM FormDesignTemplate 
 INNER JOIN ProjectForm ON FormDesignTemplate.FormID = ProjectForm.FormId
 Where ([Design] LIKE '%' + @SearchForm + '%' OR [Description] LIKE '%' + @SearchForm + '%')  
 AND ProjectForm.ProjectId = @ProjectId
 
 SELECT t.*
 FROM FormDesignTemplate AS t    
 INNER JOIN ProjectForm ON t.FormID = ProjectForm.FormId
  Where (t.[Design] LIKE '%' + @SearchForm + '%' OR t.[Description] LIKE '%' + @SearchForm + '%')   
  AND ProjectForm.ProjectId = @ProjectId
  ORDER BY [Design]             
  OFFSET @PageSize * (@PageIndex - 1) ROWS FETCH NEXT @PageSize ROWS ONLY;  

 END
END    
GO

/****** Object:  StoredProcedure [dbo].[usp_FormDesignTemplate_Update]    Script Date: 08-04-2021 15:51:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
CREATE PROCEDURE [dbo].[usp_FormDesignTemplate_Update]    
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
    
  UPDATE FormDesignTemplate    
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
/****** Object:  StoredProcedure [dbo].[usp_FormDesignTemplateDetail_Add]    Script Date: 08-04-2021 15:51:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
CREATE PROCEDURE [dbo].[usp_FormDesignTemplateDetail_Add]    
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
 SELECT @Field = ISNULL(MAX(Field),0) + 1 FROM FormDesignTemplateDetail WHERE FormID = @FormID

 IF(@Sequence = 0)
 BEGIN
     SELECT @Sequence = ISNULL(MAX([Sequence]),0) + 1 FROM FormDesignTemplateDetail WHERE FormID = @FormID AND [Section] = @Section;
 END

     INSERT INTO FormDesignTemplateDetail    
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
/****** Object:  StoredProcedure [dbo].[usp_FormDesignTemplateDetail_Fetch]    Script Date: 08-04-2021 15:51:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
CREATE PROCEDURE [dbo].[usp_FormDesignTemplateDetail_Fetch]    
(    
	@FormID INT,   
	@Field INT   
)   
AS     
BEGIN 
	SELECT t.[FormID], t.[Field], t.[FieldName], t.[FieldType], t.[Sequence], t1.[Section], 
	t1.[Description] as SectionDescription, t1.[Sequence] as SectionSequence, t1.BackgroundColor, t1.TextColor   
	FROM FormDesignTemplateDetail t JOIN TemplateFormSection t1 
	ON t.Section = t1.Section AND t.[FormID] = t1.[FormID]
    WHERE t1.[FormID] = @FormID AND [Field] = @Field ORDER BY t1.[Sequence], t.[Sequence]  
END  
GO
/****** Object:  StoredProcedure [dbo].[usp_FormDesignTemplateDetail_FetchAll]    Script Date: 08-04-2021 15:51:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
CREATE PROCEDURE [dbo].[usp_FormDesignTemplateDetail_FetchAll]  
(  
	@FormID INT  
)   
AS  
BEGIN  
SET NOCOUNT ON;  
   
	SELECT t.[FormID], t.[Field], t.[FieldName], t.[FieldType], t.[Sequence], t1.[Section], 
	t1.[Description] as SectionDescription, t1.[Sequence] as SectionSequence , t1.BackgroundColor, t1.TextColor 
	FROM FormDesignTemplateDetail t RIGHT JOIN TemplateFormSection t1 
	ON t.Section = t1.Section AND t.[FormID] = t1.[FormID] 
    WHERE t1.[FormID] = @FormID AND (ISNULL(t1.[Description],'') != '' OR [Field] IS NOT NULL) ORDER BY t1.[Sequence], t.[Sequence]

END  
GO
/****** Object:  StoredProcedure [dbo].[usp_FormDesignTemplateDetail_Update]    Script Date: 08-04-2021 15:51:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
CREATE PROCEDURE  [dbo].[usp_FormDesignTemplateDetail_Update]    
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
 
 SELECT @CheckListPrevSection = Section FROM FormDesignTemplateDetail
  WHERE FormID = @FormID AND FieldType = 9 AND Field = @Field

 IF NOT EXISTS(SELECT 1 FROM FormDesignTemplateDetail WHERE FormID = @FormID AND Field = @Field     
    AND Section = @Section AND [Sequence] = @Sequence)    
 BEGIN    
  SELECT @PrevSection = Section, @PrevSequence = [Sequence] FROM FormDesignTemplateDetail     
  WHERE FormID = @FormID AND Field = @Field;    
    
  IF(@PrevSection != @Section)    
  BEGIN    
   UPDATE FormDesignTemplateDetail SET     
    [Sequence] = [Sequence] + 1    
    WHERE FormDesignTemplateDetail.FormID = @FormID AND Section = @Section    
    AND [Sequence] >= @Sequence    
    
   UPDATE FormDesignTemplateDetail      
   SET FieldName = @FieldName,      
   FieldType = @FieldType,          
   [Section] = @Section,      
   [Sequence] = @Sequence    
   WHERE FormID = @FormID AND [Field] = @Field    
    
   ;WITH templateField    
   AS    
   (    
    SELECT ROW_NUMBER() OVER(ORDER BY [Sequence]) AS RowNumber, FormID,[Section],[Field] FROM FormDesignTemplateDetail    
    WHERE FormID = @FormID AND Section = @PrevSection    
   )    
    
   UPDATE FormDesignTemplateDetail SET FormDesignTemplateDetail.[Sequence] = templateField.RowNumber    
   FROM FormDesignTemplateDetail INNER JOIN templateField ON     
   FormDesignTemplateDetail.FormID = templateField.FormID     
   AND FormDesignTemplateDetail.Section = templateField.Section    
   AND FormDesignTemplateDetail.[Field] = templateField.[Field];    
  END    
  ELSE    
  BEGIN    
   IF (@PrevSequence > @Sequence) --Increment seq no      
   BEGIN    
    UPDATE FormDesignTemplateDetail SET     
    FormDesignTemplateDetail.[Sequence] = FormDesignTemplateDetail.[Sequence] + 1    
    WHERE FormDesignTemplateDetail.FormID = @FormID AND Section = @Section    
    AND [Sequence] >= @Sequence AND [Sequence] < @PrevSequence    
   END    
   ELSE IF (@PrevSequence < @Sequence) --decrement seq no     
   BEGIN    
    UPDATE FormDesignTemplateDetail SET     
    FormDesignTemplateDetail.[Sequence] = FormDesignTemplateDetail.[Sequence] - 1    
    WHERE FormDesignTemplateDetail.FormID = @FormID AND Section = @Section    
    AND [Sequence] > @PrevSequence AND [Sequence] <= @Sequence    
   END    
    
   UPDATE FormDesignTemplateDetail      
   SET FieldName = @FieldName,      
   FieldType = @FieldType,        
   [Section] = @Section,      
   [Sequence] = @Sequence    
   WHERE FormID = @FormID AND [Field] = @Field    
  END    
 END    
 ELSE    
 BEGIN    
  UPDATE FormDesignTemplateDetail      
  SET FieldName = @FieldName,      
   FieldType = @FieldType,        
  [Section] = @Section,      
  [Sequence] = @Sequence     
  WHERE FormID = @FormID AND [Field] = @Field    
 END    

  -- Check List Type
  IF EXISTS (SELECT 1 FROM FormDesignTemplateDetail WHERE FormID = @FormID AND FieldType = 9 AND Field = @Field)
  BEGIN

    UPDATE FormDesignTemplateDetail
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
	@ReferenceNumber int,
	@FormID INT      
)     
AS       
BEGIN
SET NOCOUNT ON;       
	SELECT t.* FROM TemplateFormFieldData AS t  WHERE t.FormID = @FormID AND t.ReferenceNumber = @ReferenceNumber 
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
	@BackgroundColor [nvarchar](20) NULL,
	@TextColor [nvarchar](20) NULL,
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
		[BackgroundColor],
		[TextColor],
		[Sequence]
	)      
	VALUES (    
		@FormID,    
		@Section,
		@Description,
		@BackgroundColor,
		@TextColor,
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
	@BackgroundColor [nvarchar](20) NULL,
	@TextColor [nvarchar](20) NULL,
    @Sequence int NULL  
AS    
BEGIN

    Update [dbo].TemplateFormSection    
    SET [Description] = @Description,    
    [Sequence] = @Sequence,
	[BackgroundColor] = @BackgroundColor,
	[TextColor] = @TextColor
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
	@ReferenceNumber INT,
    @FormID INT,      
    @Field INT,      
    @FieldValue NVARCHAR(max),
    @ErrorOccured BIT OUTPUT      
)      
AS    
BEGIN    
	BEGIN TRY    
	BEGIN TRANSACTION trans      
      
	IF EXISTS(SELECT 1 FROM TemplateFormFieldData WHERE FormID = @FormID AND ReferenceNumber = @ReferenceNumber AND Field = @Field)
	BEGIN
		UPDATE TemplateFormFieldData      
		SET FieldValue = @FieldValue
		WHERE FormID = @FormID AND Field = @Field AND ReferenceNumber = @ReferenceNumber
	END
	ELSE 
	BEGIN
		INSERT INTO TemplateFormFieldData (ReferenceNumber,FormID, Field, FieldValue)      
		SELECT @ReferenceNumber, @FormID, @Field, @FieldValue
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

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_FormDesignTemplateDetail_BlockFetch]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_FormDesignTemplateDetail_BlockFetch]
GO
CREATE PROCEDURE [dbo].[usp_FormDesignTemplateDetail_BlockFetch]        
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
  
 SELECT @RecordCount = COUNT(*)  FROM FormDesignTemplateDetail t JOIN TemplateFormSection t1   
 ON t.Section = t1.Section AND t.[FormID] = t1.[FormID]
 WHERE t1.[FormID] = @FormID AND (ISNULL(t1.[Description],'') != '' OR [Field] IS NOT NULL)
        
 SELECT t.[FormID], t.[Field], t.[FieldName], t.[FieldType], t.[Sequence], t1.[Section], 
	t1.[Description] as SectionDescription, t1.[Sequence] as SectionSequence, t1.BackgroundColor, t1.TextColor     
 FROM FormDesignTemplateDetail t JOIN TemplateFormSection t1   
 ON t.Section = t1.Section AND t.[FormID] = t1.[FormID]
 WHERE t1.[FormID] = @FormID AND (ISNULL(t1.[Description],'') != '' OR [Field] IS NOT NULL)
 ORDER BY t1.[Sequence], t.[Sequence]           
 OFFSET @PageSize * (@PageIndex - 1) ROWS FETCH NEXT @PageSize ROWS ONLY;    

END      
GO



/****** Object:  StoredProcedure [dbo].[usp_TableFieldTypeMaster_Fetch]    Script Date: 08-04-2021 15:51:54 ******/
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

-- Project & Project Form

/****** Object:  StoredProcedure [dbo].[usp_Project_Add]    Script Date: 08-04-2021 15:51:54 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_Project_Add]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_Project_Add]
GO
/****** Object:  StoredProcedure [dbo].[usp_Project_Add]    Script Date: 08-04-2021 15:51:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_Project_Add]    
(    
     @Project nvarchar(60),
	 @ClientID INT,
	 @ProjectName nvarchar(256),
	 @Description nvarchar(500),
	 @Manager nvarchar(60),
	 @SiteAddress nvarchar(256),
	 @SitePostcode nvarchar(60),
	 @CreatedDateTime datetime,
	 @LastUpdatedDateTime datetime,
	 @CreatedBy char(10),
	 @UpdatedBy char(10),
	 @ErrorOccured BIT OUTPUT,
	 @ProjectID INT OUTPUT  
)
AS    
BEGIN    

BEGIN TRY    
 BEGIN TRANSACTION trans    

     INSERT INTO Project    
        (Project,
		 ProjectName,
         [Description],
		 ClientID,
         Manager,   
		 SiteAddress,
		 SitePostcode,
         CreatedDateTime,     
         LastUpdatedDateTime,    
         CreatedBy,
		 UpdatedBy
	  )     
      VALUES (  
		@Project,
		@ProjectName,
		@Description,
		@ClientID,
		@Manager,
		@SiteAddress,
		@SitePostcode,
		@CreatedDateTime,      
		@LastUpdatedDateTime,    
		@CreatedBy,
		@UpdatedBy
		  )   
		  
 DECLARE @identity INT    
 SELECT @identity = Scope_Identity(); 

COMMIT Transaction trans    
    
 SET @ErrorOccured = 1;     
 SET @ProjectID = @identity 
 
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


/****** Object:  StoredProcedure [dbo].[usp_Project_Update]    Script Date: 08-04-2021 15:51:54 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_Project_Update]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_Project_Update]
GO
/****** Object:  StoredProcedure [dbo].[usp_Project_Update]    Script Date: 08-04-2021 15:51:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_Project_Update]    
(    
     @ProjectId int,
	 @Project nvarchar(60),
	 @ClientID INT,
	 @ProjectName nvarchar(256),
	 @Description nvarchar(500),
	 @Manager nvarchar(60),
	 @SiteAddress nvarchar(256),
	 @SitePostcode nvarchar(60),	 
	 @LastUpdatedDateTime datetime,	
	 @UpdatedBy char(10),
	 @ErrorOccured BIT OUTPUT   
)
AS    
BEGIN    

BEGIN TRY    
 BEGIN TRANSACTION trans    

     Update Project    
       SET Project = @Project,
		   ClientID = @ClientID,
		   ProjectName = @ProjectName,
	       [Description] = @Description,    
           Manager = @Manager,    
		   SiteAddress = @SiteAddress,
		   SitePostcode = @SitePostcode,
           LastUpdatedDateTime = @LastUpdatedDateTime,    
		   UpdatedBy = @UpdatedBy
       Where ID = @ProjectId

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


/****** Object:  StoredProcedure [dbo].[usp_Project_Fetch]    Script Date: 08-04-2021 15:51:54 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_Project_Fetch]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_Project_Fetch]
GO
CREATE PROCEDURE [dbo].[usp_Project_Fetch]    
(    
	@ProjectId int
)   
AS     
BEGIN 
	SELect * from Project where ID = @ProjectId
END  
GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_Project_BlockFetch]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_Project_BlockFetch]
GO
CREATE PROCEDURE [dbo].[usp_Project_BlockFetch]
(
	@PageIndex INT = 1,        
	@PageSize INT = 10,  
	@RecordCount INT OUTPUT   
)
AS         
BEGIN
SET NOCOUNT ON;
	IF(@PageIndex IS NULL)      
	BEGIN      
		SET @PageIndex = 1         
	END      
  
	SELECT @RecordCount = COUNT(*) FROM Project
        
	SELECT t.*,t1.[Name] as ClientName FROM Project t LEFT JOIN Client t1 ON t.ClientID = t1.ID
	ORDER BY Project          
	OFFSET @PageSize * (@PageIndex - 1) ROWS FETCH NEXT @PageSize ROWS ONLY;

END 
GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_Project_Delete]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_Project_Delete]
GO
CREATE PROCEDURE [dbo].[usp_Project_Delete]  
  @ProjectId int  
AS  
BEGIN
 
    DELETE FROM ProjectForm WHERE ProjectId = @ProjectId
    DELETE FROM Project WHERE ID = @ProjectId
END  
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_ProjectForm_BlockFetch]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_ProjectForm_BlockFetch]
GO
CREATE PROCEDURE [dbo].[usp_ProjectForm_BlockFetch]        
 @ProjectId int,                
 @PageIndex INT = 1,        
 @PageSize INT = 10,  
 @RecordCount INT OUTPUT    
AS         
BEGIN       
 IF(@PageIndex IS NULL)      
 BEGIN      
  SET @PageIndex = 1         
 END      
  
 SELECT @RecordCount = COUNT(*) FROM ProjectForm WHERE ProjectId  = @ProjectId
        
 SELECT * FROM ProjectForm WHERE ProjectId = @ProjectId
 ORDER BY ProjectId, FormId          
 OFFSET @PageSize * (@PageIndex - 1) ROWS FETCH NEXT @PageSize ROWS ONLY;    

END      
GO

/****** Object:  StoredProcedure [dbo].[usp_ProjectForm_Save]    Script Date: 08-04-2021 15:51:54 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_ProjectForm_Save]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_ProjectForm_Save]
GO
/****** Object:  StoredProcedure [dbo].[usp_ProjectForm_Save]    Script Date: 08-04-2021 15:51:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_ProjectForm_Save]    
(    
     @ProjectId int,
	 @FormIds nvarchar(max)
)
AS    
BEGIN    
SET NOCOUNT ON;

	IF EXISTS(SELECT 1 FROM ProjectForm WHERE ProjectId = @ProjectId)
	BEGIN
		DELETE FROM ProjectForm WHERE ProjectId = @ProjectId;
	END   

	INSERT INTO ProjectForm    
	SELECT @ProjectId,Item FROM Split(@FormIds,',');
 
END    
GO



/****** Object:  StoredProcedure [dbo].[usp_SubmittedForm_Add]    Script Date: 08-04-2021 15:51:54 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_SubmittedForm_Add]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_SubmittedForm_Add]
GO
/****** Object:  StoredProcedure [dbo].[usp_SubmittedForm_Add]    Script Date: 08-04-2021 15:51:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_SubmittedForm_Add]    
(    
     @ProjectId int,
	 @FormId int,
	 @Status int,
	 @CreatedDateTime datetime,
	 @LastUpdatedDateTime datetime,
	 @CreatedBy char(10),
	 @UpdatedBy char(10),
	 @ErrorOccured BIT OUTPUT,
	 @ReferenceNumber INT OUTPUT  
)
AS    
BEGIN    

BEGIN TRY    
 BEGIN TRANSACTION trans    

     INSERT INTO SubmittedForm    
        (ProjectId,    
         FormId,    
         [Status],    
         CreatedDateTime,     
         LastUpdatedDateTime,    
         CreatedBy,
		 UpdatedBy
	  )     
      VALUES (  
		@ProjectId,    
		@FormId,    
		@Status,    
		@CreatedDateTime,      
		@LastUpdatedDateTime,    
		@CreatedBy,
		@UpdatedBy
		  )   
		  
 DECLARE @identity INT    
 SELECT @identity = Scope_Identity(); 

COMMIT Transaction trans    
    
 SET @ErrorOccured = 1;     
 SET @ReferenceNumber = @identity 
 
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

/****** Object:  StoredProcedure [dbo].[usp_SubmittedForm_Update]    Script Date: 08-04-2021 15:51:54 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_SubmittedForm_Update]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_SubmittedForm_Update]
GO
/****** Object:  StoredProcedure [dbo].[usp_SubmittedForm_Update]    Script Date: 08-04-2021 15:51:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_SubmittedForm_Update]    
(    
     @ReferenceNumber int,
	 @ProjectId int,
	 @FormId int,
	 @Status int,
	 @LastUpdatedDateTime datetime,
	 @UpdatedBy char(10),
	 @ErrorOccured BIT OUTPUT
)
AS    
BEGIN    

BEGIN TRY    
 BEGIN TRANSACTION trans    

     Update SubmittedForm    
       set ProjectId = @ProjectId,
	       FormId = @FormId,    
           [Status] = @Status,    
           LastUpdatedDateTime = @LastUpdatedDateTime,    
		   UpdatedBy = @UpdatedBy
       Where ReferenceNumber = @ReferenceNumber

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


/****** Object:  StoredProcedure [dbo].[usp_SubmittedForm_Fetch]    Script Date: 08-04-2021 15:51:54 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_SubmittedForm_Fetch]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_SubmittedForm_Fetch]
GO
CREATE PROCEDURE [dbo].[usp_SubmittedForm_Fetch]    
(    
	@ReferenceNumber int
)   
AS     
BEGIN 
	SELECT * FROM SubmittedForm WHERE ReferenceNumber = @ReferenceNumber
END  
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_SubmittedForm_BlockFetch]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_SubmittedForm_BlockFetch]
GO
CREATE PROCEDURE usp_SubmittedForm_BlockFetch
(
	@FormID INT,
	@ProjectID INT,
	@PageIndex INT = 1,      
	@PageSize INT = 10,
	@RecordCount INT OUTPUT  
)
AS
BEGIN
	IF(@PageIndex IS NULL)    
	BEGIN    
		SET @PageIndex = 1       
	END

	SELECT @RecordCount = COUNT(*) FROM SubmittedForm 
	WHERE ProjectId = @ProjectID AND (@FormID = 0 OR FormID = @FormID)

	SELECT t.*,t1.Design, t1.[Description] as DesignDescription FROM SubmittedForm t JOIN  
	[dbo].FormDesignTemplate t1 ON t.FormID = t1.FormID 
	WHERE ProjectId = @ProjectID AND (@FormID = 0 OR t.FormID = @FormID) ORDER BY FormId, ReferenceNumber        
	OFFSET @PageSize * (@PageIndex - 1) ROWS            
	FETCH NEXT @PageSize ROWS ONLY;  
END      
GO

/****** Object:  StoredProcedure [dbo].[usp_Client_BlockFetch]    Script Date: 11-05-2021 15:51:54 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_Client_BlockFetch]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_Client_BlockFetch]
GO

/****** Object:  StoredProcedure [dbo].[usp_Client_Fetch]    Script Date: 11-05-2021 15:51:54 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_Client_Fetch]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_Client_Fetch]
GO

CREATE PROCEDURE [dbo].[usp_Client_BlockFetch]
(
	@PageIndex INT = 1,        
	@PageSize INT = 10,  
	@RecordCount INT OUTPUT   
)
AS
BEGIN
SET NOCOUNT ON;
	IF(@PageIndex IS NULL)      
	BEGIN      
		SET @PageIndex = 1         
	END      
  
	SELECT @RecordCount = COUNT(*) FROM Client
        
	SELECT * FROM Client
	ORDER BY [ID]          
	OFFSET @PageSize * (@PageIndex - 1) ROWS FETCH NEXT @PageSize ROWS ONLY;
END
GO



CREATE PROCEDURE [dbo].[usp_Client_Fetch]    
(    
	@ID int
)   
AS     
BEGIN 
	SELECT * from Client where ID = @ID
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