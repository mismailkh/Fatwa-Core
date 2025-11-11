--Example of adding column in Table 

/*<History Author='Nabeel ur Rehman' Date='26-01-2023'> Example of adding column in Table </History>*/  
GO
--IF COLUMNPROPERTY(OBJECT_ID('dbo.LDS_DOCUMENT'), 'IsAllowedToModify', 'ColumnId') IS NULL
--BEGIN 
--ALTER TABLE LDS_DOCUMENT
--	ADD IsAllowedToModify BIT NULL
--	Print('LDS_DOCUMENT.IsAllowedToModify Added')
--END
--GO
/*<History Author='Nabeel ur Rehman' Date='26-01-2023'> Example of Re-Name Column  in Table </History>*/
--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LDS_DOCUMENT]') AND type in (N'U'))  
--    EXEC sp_rename 'LDS_DOCUMENT.Title_En', 'Title', 'COLUMN'  
--    Print '[dbo].[LDS_DOCUMENT.Title_En] Renamed Successfully'

--	EXEC sp_rename 'LDS_DOCUMENT.Description_En', 'Description', 'COLUMN'  
--   Print '[dbo].[LDS_DOCUMENT.Description_En] Renamed Successfully'
--GO

-- Nadia Gull


IF COLUMNPROPERTY(OBJECT_ID('dbo.LMS_LITERATURE_AUTHOR'), 'CreatedBy', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE LMS_LITERATURE_AUTHOR
	drop column CreatedBy 
	Print('LMS_LITERATURE_AUTHOR.CreatedBy Dropped')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.LMS_LITERATURE_AUTHOR'), 'CreatedBy', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE LMS_LITERATURE_AUTHOR
	drop column CreateDate
	Print('LMS_LITERATURE_AUTHOR.CreateDate Dropped')
END


IF COLUMNPROPERTY(OBJECT_ID('dbo.LMS_LITERATURE_AUTHOR'), 'ModifiedBy', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE LMS_LITERATURE_AUTHOR
	drop column ModifiedBy
	Print('LMS_LITERATURE_AUTHOR.CreateDate Dropped')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.LMS_LITERATURE_AUTHOR'), 'ModifiedBy', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE LMS_LITERATURE_AUTHOR
	drop column ModifiedBy
	Print('LMS_LITERATURE_AUTHOR.ModifiedBy Dropped')
END


IF COLUMNPROPERTY(OBJECT_ID('dbo.LMS_LITERATURE_AUTHOR'), 'DeletedDate', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE LMS_LITERATURE_AUTHOR
	drop column DeletedDate
	Print('LMS_LITERATURE_AUTHOR.DeletedDate Dropped')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.LMS_LITERATURE_AUTHOR'), 'IsDeleted', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE LMS_LITERATURE_AUTHOR
	drop column IsDeleted
	Print('LMS_LITERATURE_AUTHOR.IsDeleted Dropped')
END


IF COLUMNPROPERTY(OBJECT_ID('dbo.LMS_LITERATURE_AUTHOR'), 'CreatedDate', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE LMS_LITERATURE_AUTHOR
	drop column CreatedDate
	Print('LMS_LITERATURE_AUTHOR.CreatedDate Dropped')
END
IF COLUMNPROPERTY(OBJECT_ID('dbo.LMS_LITERATURE_AUTHOR'), 'ModifiedDate', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE LMS_LITERATURE_AUTHOR
	drop column ModifiedDate
	Print('LMS_LITERATURE_AUTHOR.ModifiedDate Dropped')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.LMS_LITERATURE_AUTHOR'), 'DeletedBy', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE LMS_LITERATURE_AUTHOR
	drop column DeletedBy
	Print('LMS_LITERATURE_AUTHOR.DeletedBy Dropped')
END


IF COLUMNPROPERTY(OBJECT_ID('dbo.COMM_COMMUNICATION_MEETING'), 'StartTime', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE COMM_COMMUNICATION_MEETING
	drop column StartTime
	Print('COMM_COMMUNICATION_MEETING.StartTime Dropped')
END
IF COLUMNPROPERTY(OBJECT_ID('dbo.COMM_COMMUNICATION_MEETING'), 'EndTime', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE COMM_COMMUNICATION_MEETING
	drop column EndTime
	Print('COMM_COMMUNICATION_MEETING.EndTime Dropped')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.COMM_COMMUNICATION_MEETING'), 'StartTime', 'ColumnId') IS NULL
BEGIN
	ALTER TABLE COMM_COMMUNICATION_MEETING
	Add StartTime datetime not null default CURRENT_TIMESTAMP;
	Print('COMM_COMMUNICATION_MEETING.StartTime Added')
END
IF COLUMNPROPERTY(OBJECT_ID('dbo.COMM_COMMUNICATION_MEETING'), 'EndTime', 'ColumnId') IS NULL
BEGIN
	ALTER TABLE COMM_COMMUNICATION_MEETING
	Add EndTime datetime not null  default CURRENT_TIMESTAMP;
	Print('COMM_COMMUNICATION_MEETING.EndTime Added')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.MEET_MEETING'), 'FileId', 'ColumnId') IS NOT NULL
BEGIN
	EXEC sp_rename 'MEET_MEETING.FileId', 'ReferenceGuid', 'COLUMN';
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.MEET_MEETING'), 'IsOnline', 'ColumnId') IS NULL
BEGIN
	ALTER TABLE MEET_MEETING
	Add IsOnline bit null default 0;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.COMM_COMMUNICATION'), 'ShortNumber', 'ColumnId') IS NULL
BEGIN
	ALTER TABLE COMM_COMMUNICATION
	Add ShortNumber int not null default 1;  
	Print('COMM_COMMUNICATION.ShortNumber Added')
END

-- Nadia Gull
IF COLUMNPROPERTY(OBJECT_ID('dbo.LMS_LITERATURE_DETAILS'), 'EditionNumber ', 'ColumnId') IS NULL
BEGIN
	ALTER TABLE LMS_LITERATURE_DETAILS
	Add EditionNumber nvarchar(50) not null default 'x';
	Print('LMS_LITERATURE_DETAILS.EditionNumber Added')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.LMS_BOOK_STATUS'), 'Name_Ar ', 'ColumnId') IS NULL
BEGIN
	ALTER TABLE LMS_BOOK_STATUS
	Add Name_Ar nvarchar(50)  null;
	Print('LMS_BOOK_STATUS.Name_Ar Added')
END
 

 IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_EXECUTION'), 'CaseId', 'ColumnId') IS NULL
BEGIN
ALTER TABLE CMS_EXECUTION
ADD CaseId UNIQUEIDENTIFIER   NULL;
Print('CMS_EXECUTION.CaseId Added')
END


/*<History Author='ijaz Ahmad' Date='07-02-2023'> Alter the Following Table Column </History>*/  


IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_CASE_REQUEST'), 'ClaimAmount', 'ColumnId') IS NOT NULL
BEGIN
ALTER TABLE CMS_CASE_REQUEST
ALTER COLUMN ClaimAmount decimal(25,2) Null
END
IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_REGISTERED_CASE'), 'CaseAmount', 'ColumnId') IS NOT NULL
BEGIN
ALTER TABLE CMS_REGISTERED_CASE
ALTER COLUMN CaseAmount decimal(25,2) Null
END
IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_EXECUTION'), 'FileBalance', 'ColumnId') IS NOT NULL
BEGIN
ALTER TABLE CMS_EXECUTION
ALTER COLUMN FileBalance decimal(25,2) Null
END
IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_JUDGEMENT'), 'Amount', 'ColumnId') IS NOT NULL
BEGIN
ALTER TABLE CMS_JUDGEMENT
ALTER COLUMN Amount decimal(25,2) Null
END
IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_EXECUTION'), 'FileBalance', 'ColumnId') IS NOT NULL
BEGIN
ALTER TABLE CMS_EXECUTION
ALTER COLUMN FileBalance decimal(25,2) Null
END
IF COLUMNPROPERTY(OBJECT_ID('dbo.COMM_COMMUNICATION'), 'SectorTypeId', 'ColumnId') IS NULL
BEGIN
ALTER TABLE COMM_COMMUNICATION
ADD SectorTypeId INT;
Print('COMM_COMMUNICATION.SectorTypeId Added')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.COMM_COMMUNICATION'), 'SourceId', 'ColumnId') IS NULL
BEGIN
ALTER TABLE COMM_COMMUNICATION
ADD SourceId INT;
Print('COMM_COMMUNICATION.SourceId Added')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.COMM_COMMUNICATION'), 'GovtEntityId', 'ColumnId') IS NULL
BEGIN
ALTER TABLE COMM_COMMUNICATION
ADD GovtEntityId INT;
Print('COMM_COMMUNICATION.GovtEntityId Added')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.COMM_COMMUNICATION'), 'ReceivedBy', 'ColumnId') IS NULL
BEGIN
ALTER TABLE COMM_COMMUNICATION
ADD ReceivedBy NVARCHAR(256);
Print('COMM_COMMUNICATION.ReceivedBy Added')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.COMM_COMMUNICATION'), 'SentBy', 'ColumnId') IS NULL
BEGIN
ALTER TABLE COMM_COMMUNICATION
ADD SentBy NVARCHAR(256);
Print('COMM_COMMUNICATION.SentBy Added')
END


---------------- consultation request model -----------------------

IF COLUMNPROPERTY(OBJECT_ID('dbo.COMS_CONSULTATION_REQUEST'), 'RequestSubTypeId', 'ColumnId') IS NOT NULL
BEGIN
ALTER TABLE COMS_CONSULTATION_REQUEST
ALTER COLUMN RequestSubTypeId int NULL
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.COMS_CONSULTATION_REQUEST'), 'Subject', 'ColumnId') IS NOT NULL
BEGIN
ALTER TABLE COMS_CONSULTATION_REQUEST
ALTER COLUMN Subject nvarchar(500) NULL
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.COMS_CONSULTATION_REQUEST'), 'CompetentAuthorityId', 'ColumnId') IS NULL
BEGIN
ALTER TABLE COMS_CONSULTATION_REQUEST
ADD CompetentAuthorityId int NULL;
Print('COMS_CONSULTATION_REQUEST.CompetentAuthorityId Added')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.COMS_CONSULTATION_REQUEST'), 'ComplainantName', 'ColumnId') IS NULL
BEGIN
ALTER TABLE COMS_CONSULTATION_REQUEST
ADD ComplainantName nvarchar(1000) NULL;
Print('COMS_CONSULTATION_REQUEST.ComplainantName Added')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.COMS_CONSULTATION_REQUEST'), 'ComplaintAgainst', 'ColumnId') IS NULL
BEGIN
ALTER TABLE COMS_CONSULTATION_REQUEST
ADD ComplaintAgainst nvarchar(1000) NULL;
Print('COMS_CONSULTATION_REQUEST.ComplaintAgainst Added')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.COMS_CONSULTATION_REQUEST'), 'ComplainantDecisionNumber', 'ColumnId') IS NULL
BEGIN
ALTER TABLE COMS_CONSULTATION_REQUEST
ADD ComplainantDecisionNumber nvarchar(500) NULL;
Print('COMS_CONSULTATION_REQUEST.ComplainantDecisionNumber Added')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.COMS_CONSULTATION_REQUEST'), 'LegislationFileTypeId', 'ColumnId') IS NULL
BEGIN
ALTER TABLE COMS_CONSULTATION_REQUEST
ADD LegislationFileTypeId int NULL;
Print('COMS_CONSULTATION_REQUEST.LegislationFileTypeId Added')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.COMS_CONSULTATION_REQUEST'), 'CompetentAuthorityOpinionWithNote', 'ColumnId') IS NULL
BEGIN
ALTER TABLE COMS_CONSULTATION_REQUEST
ADD CompetentAuthorityOpinionWithNote nvarchar(max) NULL;
Print('COMS_CONSULTATION_REQUEST.CompetentAuthorityOpinionWithNote Added')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.COMS_CONSULTATION_REQUEST'), 'Introduction', 'ColumnId') IS NOT NULL
BEGIN
ALTER TABLE COMS_CONSULTATION_REQUEST
ALTER COLUMN Introduction nvarchar(max) NULL
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.COMS_CONSULTATION_REQUEST'), 'OfficialLetterOutboxNumber', 'ColumnId') IS NULL
BEGIN
ALTER TABLE COMS_CONSULTATION_REQUEST
ALTER COLUMN OfficialLetterOutboxNumber nvarchar(500) NULL
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.COMS_CONSULTATION_REQUEST'), 'FatwaInboxNumber', 'ColumnId') IS NULL
BEGIN
ALTER TABLE COMS_CONSULTATION_REQUEST
ALTER COLUMN FatwaInboxNumber nvarchar(500) NULL
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.COMS_CONSULTATION_REQUEST'), 'TemplateId', 'ColumnId') IS NULL
BEGIN
ALTER TABLE COMS_CONSULTATION_REQUEST
ADD TemplateId int NULL;
Print('COMS_CONSULTATION_REQUEST.TemplateId Added')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.COMS_CONSULTATION_PARTY'), 'Email', 'ColumnId') IS NOT NULL
BEGIN
ALTER TABLE COMS_CONSULTATION_PARTY
ALTER COLUMN Email nvarchar(500) NULL
END

--------------------------- CONSULTATION

IF COLUMNPROPERTY(OBJECT_ID('dbo.COMS_TEMPLATE_SECTION'), 'Name_Ar ', 'ColumnId') IS NULL
BEGIN
	ALTER TABLE COMS_TEMPLATE_SECTION
	Add Name_Ar nvarchar(1000) NULL 
	Print('COMS_TEMPLATE_SECTION.Name_Ar')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.COMS_TEMPLATE_SECTION'), 'SectionHeadId', 'ColumnId') IS NOT NULL
BEGIN 
ALTER TABLE COMS_TEMPLATE_SECTION
	 ADD SectionHeadId int NOT NULL DEFAULT 0
	Print('COMS_TEMPLATE_SECTION.SectionHeadId Added')
END

 IF COLUMNPROPERTY(OBJECT_ID('dbo.LMS_LITERATURE_BORROW_DETAILS'), 'ApplyReturnDate', 'ColumnId') IS NULL
BEGIN
	ALTER TABLE LMS_LITERATURE_BORROW_DETAILS
	Add ApplyReturnDate datetime  null  ;
	Print('LMS_LITERATURE_BORROW_DETAILS.ApplyReturnDate Added')
END


-------------------

IF COLUMNPROPERTY(OBJECT_ID('dbo.COMS_WITHDRAW_REQUEST'), 'CreatedBy', 'ColumnId') IS NOT NULL
BEGIN
ALTER TABLE COMS_WITHDRAW_REQUEST
ALTER COLUMN CreatedBy nvarchar(100) NULL
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.COMS_WITHDRAW_REQUEST'), 'CreatedDate', 'ColumnId') IS NOT NULL
BEGIN
ALTER TABLE COMS_WITHDRAW_REQUEST
ALTER COLUMN CreatedDate datetime NULL
END

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[COMS_WITHDRAW_REQUEST]') AND type in (N'U')) 
	
	EXEC sp_rename 'COMS_WITHDRAW_REQUEST.CreatedBy', 'RequestedBy', 'COLUMN';
	Print '[dbo].[COMS_WITHDRAW_REQUEST.CreatedBy] Renamed Successfully'

	EXEC sp_rename 'COMS_WITHDRAW_REQUEST.CreatedDate', 'RequestedDate', 'COLUMN';
	Print '[dbo].[COMS_WITHDRAW_REQUEST.CreatedDate] Renamed Successfully'

	EXEC sp_rename 'COMS_WITHDRAW_REQUEST.ModifiedBy', 'ApprovedBy', 'COLUMN';
	Print '[dbo].[COMS_WITHDRAW_REQUEST.ModifiedBy] Renamed Successfully'

	EXEC sp_rename 'COMS_WITHDRAW_REQUEST.ModifiedDate', 'ApprovedDate', 'COLUMN'  
	Print '[dbo].[COMS_WITHDRAW_REQUEST.ModifiedDate] Renamed Successfully'

    EXEC sp_rename 'COMS_WITHDRAW_REQUEST.DeletedBy', 'RejectedBy', 'COLUMN'  
    Print '[dbo].[COMS_WITHDRAW_REQUEST.DeletedBy] Renamed Successfully'

	EXEC sp_rename 'COMS_WITHDRAW_REQUEST.DeletedDate', 'RejectedDate', 'COLUMN'  
    Print '[dbo].[COMS_WITHDRAW_REQUEST.DeletedDate] Renamed Successfully'
GO

IF COLUMNPROPERTY(OBJECT_ID('dbo.COMS_WITHDRAW_REQUEST'), 'RequestNumber', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE COMS_WITHDRAW_REQUEST
	drop column RequestNumber 
	Print('COMS_WITHDRAW_REQUEST.RequestNumber Dropped')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.COMS_WITHDRAW_REQUEST'), 'RequestDate', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE COMS_WITHDRAW_REQUEST
	drop column RequestDate 
	Print('COMS_WITHDRAW_REQUEST.RequestDate Dropped')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.COMS_CONSULTATION_REQUEST'), 'InternationalArbitrationTypeId', 'ColumnId') IS NULL
BEGIN 
ALTER TABLE COMS_CONSULTATION_REQUEST
	 ADD InternationalArbitrationTypeId int NULL
	Print('COMS_CONSULTATION_REQUEST.InternationalArbitrationTypeId Added')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.COMS_CONSULTATION_REQUEST'), 'HighPriorityReason', 'ColumnId') IS NULL
BEGIN 
ALTER TABLE COMS_CONSULTATION_REQUEST
	 ADD HighPriorityReason nvarchar(max) NULL
	Print('COMS_CONSULTATION_REQUEST.HighPriorityReason Added')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.COMS_CONSULTATION_REQUEST'), 'CSCSubmissionDate', 'ColumnId') IS NULL
BEGIN 
ALTER TABLE COMS_CONSULTATION_REQUEST
	 ADD CSCSubmissionDate datetime NULL
	Print('COMS_CONSULTATION_REQUEST.CSCSubmissionDate Added')
END


-----------------

IF COLUMNPROPERTY(OBJECT_ID('dbo.COMS_TEMPLATE_SECTION'), 'SectionName_En', 'ColumnId') IS NULL
BEGIN
ALTER TABLE COMS_TEMPLATE_SECTION
ADD SectionName_En NVARCHAR(500);
Print('COMS_TEMPLATE_SECTION.SectionName_En Added')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.COMS_TEMPLATE_SECTION'), 'SectionName_Ar', 'ColumnId') IS NULL
BEGIN
ALTER TABLE COMS_TEMPLATE_SECTION
ADD SectionName_Ar NVARCHAR(500);
Print('COMS_TEMPLATE_SECTION.SectionName_Ar Added')
END



IF COLUMNPROPERTY(OBJECT_ID('dbo.TSK_TASK'), 'SubModuleId', 'ColumnId') IS NULL
BEGIN
	ALTER TABLE TSK_TASK
	Add SubModuleId INT
	Print('TSK_TASK.SubModuleId Added')
END


IF COLUMNPROPERTY(OBJECT_ID('dbo.TSK_TASK'), 'SystemGenTypeId', 'ColumnId') IS NULL
BEGIN
	ALTER TABLE TSK_TASK
	Add SystemGenTypeId INT
	Print('TSK_TASK.SystemGenTypeId Added')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.COMS_WITHDRAW_REQUEST'), 'FatwaReason', 'ColumnId') IS NULL
BEGIN
ALTER TABLE COMS_WITHDRAW_REQUEST
ADD FatwaReason NVARCHAR(max);
Print('COMS_WITHDRAW_REQUEST.FatwaReason Added')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.COMS_WITHDRAW_REQUEST'), 'RequestPreviousStateStatusId', 'ColumnId') IS NULL
BEGIN
ALTER TABLE COMS_WITHDRAW_REQUEST
ADD RequestPreviousStateStatusId int;
Print('COMS_WITHDRAW_REQUEST.RequestPreviousStateStatusId Added')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.COMS_WITHDRAW_REQUEST'), 'FilePreviousStateStatusId', 'ColumnId') IS NULL
BEGIN
ALTER TABLE COMS_WITHDRAW_REQUEST
ADD FilePreviousStateStatusId int;
Print('COMS_WITHDRAW_REQUEST.FilePreviousStateStatusId Added')
END


DELETE FROM CMS_REGISTERED_CASE_STATUS_HISTORY
Go

WITH cte AS (
    SELECT 
        CaseNumber, 
        ROW_NUMBER() OVER (
            PARTITION BY 
                CaseNumber
            ORDER BY 
                CaseNumber
        ) row_num
     FROM 
        CMS_REGISTERED_CASE
)
DELETE FROM cte
WHERE row_num > 1;
Go

ALTER TABLE CMS_REGISTERED_CASE ADD CONSTRAINT UC_CaseNumber UNIQUE(CaseNumber)

/****** Object:  Table [dbo].[CMS_REQUEST_FOR_DOCUMENT]    Script Date: 24/03/2023 5:27:28 pm ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_REQUEST_FOR_DOCUMENT]') AND type in (N'U'))
DROP TABLE [dbo].[CMS_REQUEST_FOR_DOCUMENT]
GO

/****** Object:  Table [dbo].[CMS_REQUEST_FOR_DOCUMENT]    Script Date: 24/03/2023 5:27:28 pm ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[CMS_REQUEST_FOR_DOCUMENT]') AND type in (N'U'))
DROP TABLE [CMS_REQUEST_FOR_DOCUMENT]
GO

/****** Object:  Table [CMS_DOCUMENT_PORTFOLIO_REQUEST]    Script Date: 24/03/2023 5:27:28 pm ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[CMS_DOCUMENT_PORTFOLIO_REQUEST]') AND type in (N'U'))
DROP TABLE [CMS_DOCUMENT_PORTFOLIO_REQUEST]
GO

CREATE TABLE CMS_DOCUMENT_PORTFOLIO_REQUEST(
	[Id] [uniqueidentifier] NOT NULL,
	[AttachmentTypeId] [int] NOT NULL,
	[CaseId] [uniqueidentifier] NOT NULL,
	[HearingDate] [datetime] NOT NULL,
	[CreatedBy] [nvarchar](256) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](256) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](256) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
	[IsAddressed] [bit] NULL,
	[SectorTypeId] [int] NULL,
	[RequiredDocuments] [nvarchar](max) NULL)

ALTER TABLE CMS_DOCUMENT_PORTFOLIO_REQUEST  ADD CONSTRAINT [CMS_DOCUMENT_PORTFOLIO_REQUEST_CASE] FOREIGN KEY([CaseId])
REFERENCES [CMS_REGISTERED_CASE] ([CaseId])
GO

ALTER TABLE CNT_CONTACT ALTER COLUMN PhoneNumber nvarchar(100)
ALTER TABLE CNT_CONTACT ALTER COLUMN FirstName nvarchar(max)
ALTER TABLE CNT_CONTACT ALTER COLUMN SecondName nvarchar(max)
ALTER TABLE CNT_CONTACT ALTER COLUMN LastName nvarchar(max)
ALTER TABLE CNT_CONTACT ALTER COLUMN Notes nvarchar(max)
-------------Drop Constraint From CMS_DRAFTED_TEMPLATE (FATWA_DB)
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[CMS_DRAFTED_TEMPLATE]') AND type in (N'U'))
ALTER TABLE [dbo].[CMS_DRAFTED_TEMPLATE] DROP CONSTRAINT IF EXISTS [CMS_DRAFTED_TEMPLATE_ATTACHMENT_TYPE]

----------------------Add coulmn CMS_CASE_DECISION
IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_CASE_DECISION'), 'isExecutionLost', 'ColumnId') IS NULL
BEGIN
ALTER TABLE CMS_CASE_DECISION
ADD isExecutionLost BIT NULL
Print('CMS_CASE_DECISION.isExecutionLost Added')
END
IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_CASE_DECISION'), 'StatusId', 'ColumnId') IS NULL
BEGIN
ALTER TABLE CMS_CASE_DECISION
ADD StatusId INT  NOT NULL DEFAULT 0;
Print('CMS_CASE_DECISION.StatusId Added')
END


ALTER TABLE CMS_MOJ_EXECUTION_REQUEST ADD Remarks NVARCHAR(MAX)


/****** Object:  Table [CMS_MOJ_EXECUTION_REQUEST_ASSIGNEE]    Script Date: 24/03/2023 5:27:28 pm ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[CMS_MOJ_EXECUTION_REQUEST_ASSIGNEE]') AND type in (N'U'))
DROP TABLE CMS_MOJ_EXECUTION_REQUEST_ASSIGNEE
GO

CREATE TABLE CMS_MOJ_EXECUTION_REQUEST_ASSIGNEE
(
Id UNIQUEIDENTIFIER,
RequestId UNIQUEIDENTIFIER,
UserId NVARCHAR(256),
[CreatedBy] [nvarchar](256) NOT NULL,
[CreatedDate] [datetime] NOT NULL,
[ModifiedBy] [nvarchar](256) NULL,
[ModifiedDate] [datetime] NULL,
[DeletedBy] [nvarchar](256) NULL,
[DeletedDate] [datetime] NULL,
[IsDeleted] [bit] NOT NULL,
)


------- Attachment Type -----
IF COLUMNPROPERTY(OBJECT_ID('dbo.ATTACHMENT_TYPE'), 'RequestTypeId', 'ColumnId') IS NULL
BEGIN
ALTER TABLE ATTACHMENT_TYPE
ADD RequestTypeId INT NULL;
Print('ATTACHMENT_TYPE.RequestTypeId Added')
END


IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_DRAFTED_TEMPLATE'), 'Content', 'ColumnId') IS NULL
BEGIN
ALTER TABLE CMS_DRAFTED_TEMPLATE ADD Content NVARCHAR(MAX)
Print('CMS_DRAFTED_TEMPLATE.Content Added')
END

--------------------------- Script Date: 18/04/2023 ------------------------------ 
IF COLUMNPROPERTY(OBJECT_ID('dbo.UPLOADED_DOCUMENT'), 'IsMOJRegistered', 'ColumnId') IS NULL
BEGIN
ALTER TABLE UPLOADED_DOCUMENT
Add IsMOJRegistered bit default 0;
update UPLOADED_DOCUMENT set IsMOJRegistered = 0
Print('UPLOADED_DOCUMENT.IsMOJRegistered Added')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.TEMP_ATTACHEMENTS'), 'IsMOJRegistered', 'ColumnId') IS NULL
BEGIN
ALTER TABLE TEMP_ATTACHEMENTS
Add IsMOJRegistered bit default 0;
update TEMP_ATTACHEMENTS set IsMOJRegistered = 0
Print('TEMP_ATTACHEMENTS.IsMOJRegistered Added')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_MOJ_REGISTRATION_REQUEST'), 'DocumentId', 'ColumnId') IS NULL
BEGIN
ALTER TABLE CMS_MOJ_REGISTRATION_REQUEST
Add DocumentId int default 0;
Print('CMS_MOJ_REGISTRATION_REQUEST.DocumentId Added')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_REGISTERED_CASE'), 'RequestTypeId', 'ColumnId') IS NULL
BEGIN
Alter Table CMS_REGISTERED_CASE
Add RequestTypeId int null;
ALTER TABLE CMS_REGISTERED_CASE
ADD FOREIGN KEY (RequestTypeId) REFERENCES [CMS_REQUEST_TYPE_G2G_LKP](Id);
Print('CMS_REGISTERED_CASE.RequestTypeId Added')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_REGISTERED_CASE'), 'FloorNumber', 'ColumnId') IS NULL
BEGIN
Alter Table CMS_REGISTERED_CASE
Add FloorNumber nvarchar(max) not null default ''
Print('CMS_REGISTERED_CASE.FloorNumber Added')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_REGISTERED_CASE'), 'RoomNumber', 'ColumnId') IS NULL
BEGIN
Alter Table CMS_REGISTERED_CASE
Add RoomNumber nvarchar(max) not null default ''
Print('CMS_REGISTERED_CASE.RoomNumber Added')
END


IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_REGISTERED_CASE'), 'AnnouncementNumber', 'ColumnId') IS NULL
BEGIN
Alter Table CMS_REGISTERED_CASE
Add AnnouncementNumber nvarchar(max) not null default ''
Print('CMS_REGISTERED_CASE.AnnouncementNumber Added')
END

ALTER TABLE CMS_CASE_REQUEST ADD Pledge bit default 0


------- Attachment Type -----
IF COLUMNPROPERTY(OBJECT_ID('dbo.ATTACHMENT_TYPE'), 'IsGePortalType', 'ColumnId') IS NULL
BEGIN
ALTER TABLE ATTACHMENT_TYPE
ADD IsGePortalType bit NOT NULL DEFAULT 0;
Print('ATTACHMENT_TYPE.IsGePortalType Added')
END
------ Store Item Name
IF COLUMNPROPERTY(OBJECT_ID('dbo.INV_ITEM_NAME'), 'IsViewable', 'ColumnId') IS NULL
BEGIN
ALTER TABLE	INV_ITEM_NAME
ADD [IsViewable] BIT NOT NULL  DEFAULT 0
Print('INV_ITEM_NAME.IsViewable Added')
END
ALTER TABLE	INV_ITEM_NAME
ADD [Quantity] INT NOT NULL DEFAULT 0
ALTER TABLE	INV_ITEM_NAME
ADD [Unit] INT  NULL
ALTER TABLE	INV_ITEM_NAME
ADD [QuantityPerUnit] INT  NULL
ALTER TABLE	INV_ITEM_NAME
ADD [BarCode] nvarchar(max) NOT  NULL
ALTER TABLE	INV_ITEM_NAME
ADD StoreId UNIQUEIDENTIFIER  NULL
ALTER TABLE	INV_ITEM_NAME
ADD [VendorId] UNIQUEIDENTIFIER   NULL

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[UMS_USER_FLOOR_LKP]') AND type in (N'U'))
DROP TABLE [UMS_USER_FLOOR_LKP]
GO

CREATE TABLE UMS_USER_FLOOR_LKP (
[Id] INT NOT NULL,
[NameEn] nvarchar(max) NULL,
[NameAr] nvarchar(max) NULL
)

IF COLUMNPROPERTY(OBJECT_ID('dbo.COMS_DRAFTED_TEMPLATE'), 'Content', 'ColumnId') IS NULL
BEGIN
ALTER TABLE COMS_DRAFTED_TEMPLATE ADD Content NVARCHAR(MAX)
Print('COMS_DRAFTED_TEMPLATE.Content Added')
END
-----------------Add Column SYSTEM_SETTING(FATWA_DB)
IF COLUMNPROPERTY(OBJECT_ID('dbo.SYSTEM_SETTING'), 'FileTypes', 'ColumnId') IS  NULL
BEGIN
	ALTER TABLE SYSTEM_SETTING
	ADD  FileTypes NVARCHAR(max)  ;
	Print('SYSTEM_SETTING.FileTypes Added')
END
---------------------UPLOADED_DOCUMENT (DMS_DB)

IF COLUMNPROPERTY(OBJECT_ID('dbo.Uploaded_DOCUMENT'), 'IsConfidential','ColumnId') IS NULL
BEGIN
ALTER TABLE UPLOADED_DOCUMENT 
ADD  IsConfidential BIT default 0;
update UPLOADED_DOCUMENT set IsConfidential = 0
Print('UPLOADED_DOCUMENT.IsConfidential Added')
END


-----------

ALTER TABLE LEGAL_LEGISLATION ALTER COLUMN IssueDate_Hijri Datetime NULL

-------------- 14/08/2023---------

IF COLUMNPROPERTY(OBJECT_ID('dbo.TEMP_ATTACHEMENTS'), 'IsMaskedAttachment', 'ColumnId') IS NULL
BEGIN
ALTER TABLE TEMP_ATTACHEMENTS
Add IsMaskedAttachment bit default 0;
END
IF COLUMNPROPERTY(OBJECT_ID('dbo.UPLOADED_DOCUMENT'), 'IsMaskedAttachment', 'ColumnId') IS NOT NULL
BEGIN
update TEMP_ATTACHEMENTS set IsMaskedAttachment = 0
Print('TEMP_ATTACHEMENTS.IsMaskedAttachment Added')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.UPLOADED_DOCUMENT'), 'IsMaskedAttachment', 'ColumnId') IS NULL
BEGIN
ALTER TABLE UPLOADED_DOCUMENT
Add IsMaskedAttachment bit default 0;
END
IF COLUMNPROPERTY(OBJECT_ID('dbo.UPLOADED_DOCUMENT'), 'IsMaskedAttachment', 'ColumnId') IS NOT NULL
BEGIN
update UPLOADED_DOCUMENT set IsMaskedAttachment = 0
Print('UPLOADED_DOCUMENT.IsMaskedAttachment Added')
END

------------------------
----------------WORKFLOW_TRIGGER----------------
IF COLUMNPROPERTY(OBJECT_ID('dbo.WORKFLOW_TRIGGER'), 'AttachmentTypeId', 'ColumnId') IS NULL
BEGIN
ALTER TABLE WORKFLOW_TRIGGER
ADD AttachmentTypeId int;
Print('WORKFLOW_TRIGGER.AttachmentTypeId Added')
END
----------------------------------
IF COLUMNPROPERTY(OBJECT_ID('dbo.WORKFLOW_TRIGGER'), 'RequestTypeId', 'ColumnId') IS NULL
BEGIN
ALTER TABLE WORKFLOW_TRIGGER
ADD RequestTypeId int;
Print('WORKFLOW_TRIGGER.RequestTypeId Added')
END
---------------------------------------------
IF COLUMNPROPERTY (OBJECT_ID('dbo.WORKFLOW_ACTIVITY'),'IsTask','ColumnId' )IS  NULL
BEGIN
ALTER TABLE WORKFLOW_ACTIVITY
ADD IsTask BIT  NULL DEFAULT 0;
Print('WORKFLOW_ACTIVITY.IsTask Added')
END
GO
UPDATE WORKFLOW_ACTIVITY SET IsTask =0;

-----------------------------------------------------------------
IF COLUMNPROPERTY(OBJECT_ID('dbo.WORKFLOW_CONDITION'), 'IsLawyerTask', 'ColumnId') IS NULL
BEGIN 
ALTER TABLE WORKFLOW_CONDITION
	ADD IsLawyerTask BIT NULL
	Print('WORKFLOW_CONDITION.IsLawyerTask Added')
END
GO
UPDATE WORKFLOW_CONDITION set IsLawyerTask = 0;
GO
-------------------------------------------
ALTER TABLE [dbo].[MODULE_CONDITION] DROP CONSTRAINT [MODULE_CONDITIONS_MODULE]
IF COLUMNPROPERTY(OBJECT_ID('dbo.MODULE_CONDITION'), 'ModuleId', 'ColumnId') IS NOT NULL
ALTER TABLE MODULE_CONDITION
DROP COLUMN ModuleId;
---------------------------------
IF COLUMNPROPERTY(OBJECT_ID('dbo.WORKFLOW_ACTIVITY'), 'IsNotification', 'ColumnId') IS NULL
BEGIN
ALTER TABLE WORKFLOW_ACTIVITY
Add  IsNotification BIT  NULL DEFAULT 0
END
GO
update WORKFLOW_ACTIVITY set IsNotification = 0

------------------------ 07-09-2023
IF COLUMNPROPERTY(OBJECT_ID('dbo.LMS_LITERATURE_BORROW_DETAILS'), 'BorrowReturnApprovalStatus', 'ColumnId') IS NOT NULL
BEGIN
	ALTER TABLE LMS_LITERATURE_BORROW_DETAILS
	Add BorrowReturnApprovalStatus INT NOT NULL DEFAULT 1;
	Print('LMS_LITERATURE_BORROW_DETAILS.BorrowReturnApprovalStatus Added')
END

--Add column in CMS_REGISTERED_CASE 29/11/23

ALTER TABLE CMS_REGISTERED_CASE
ADD ChamberNumberId INT NOT NULL FOREIGN KEY(ChamberNumberId)
REFERENCES CMS_CHAMBER_NUMBER_G2G_LKP(Id)


IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_JUDGEMENT'), 'CategoryId', 'ColumnId') IS NOT NULL
BEGIN
	ALTER TABLE CMS_JUDGEMENT
	ADD CategoryId INT;
	Print('CMS_JUDGEMENT.CategoryId Added')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_JUDGEMENT'), 'StatusId', 'ColumnId') IS NOT NULL
BEGIN
	ALTER TABLE CMS_JUDGEMENT
	ADD StatusId INT;
	Print('CMS_JUDGEMENT.StatusId Added')
END


IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_JUDGEMENT'), 'AmountCollected', 'ColumnId') IS NOT NULL
BEGIN
	ALTER TABLE CMS_JUDGEMENT
	ADD AmountCollected decimal(25,2);
	Print('CMS_JUDGEMENT.AmountCollected Added')
END


IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_JUDGEMENT'), 'Remarks', 'ColumnId') IS NOT NULL
BEGIN
	ALTER TABLE CMS_JUDGEMENT
	ADD Remarks NVARCHAR(MAX);
	Print('CMS_JUDGEMENT.Remarks Added')
END
------------------------10/9/2023
IF COLUMNPROPERTY(OBJECT_ID('dbo.WORKFLOW_CONDITION'), 'IsOption', 'ColumnId') IS NULL
BEGIN
ALTER TABLE WORKFLOW_CONDITION
Add  IsOption BIT  NULL DEFAULT 0
END
GO
update WORKFLOW_CONDITION set IsOption = 0
-----------------------10/17/2023
IF COLUMNPROPERTY(OBJECT_ID('DBO.ATTACHMENT_TYPE') , 'IsOpinion' , 'ColumnId') IS NULL
BEGIN
ALTER TABLE ATTACHMENT_TYPE
ADD IsOpinion BIT  NULL Default 0;
END
GO
update ATTACHMENT_TYPE set IsOpinion = 0
-----------------------------------

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_JUDGEMENT'), 'OpenExecutionFile', 'ColumnId') IS NOT NULL
BEGIN
	ALTER TABLE CMS_JUDGEMENT
	ADD OpenExecutionFile bit;
	Print('CMS_JUDGEMENT.OpenExecutionFile Added')
END
IF COLUMNPROPERTY(OBJECT_ID('dbo.COMS_CONSULTATION_FILE'), 'IsAssignedBack', 'ColumnId') IS NULL
BEGIN 
ALTER TABLE COMS_CONSULTATION_FILE
	ADD IsAssignedBack BIT NULL
	Print('COMS_CONSULTATION_FILE.IsAssignedBack Added')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_JUDGEMENT'), 'ExecutionFileLevelId', 'ColumnId') IS NOT NULL
BEGIN
	ALTER TABLE CMS_JUDGEMENT
	ADD ExecutionFileLevelId int;
	Print('CMS_JUDGEMENT.ExecutionFileLevelId Added')
END

/*<History Author='Umer Zaman' Date='24-12-2023'> Meeting Script start </History>*/
IF COLUMNPROPERTY(OBJECT_ID('dbo.MEET_MEETING'), 'Note ', 'ColumnId') IS NULL
BEGIN
	ALTER TABLE MEET_MEETING
	Add Note nvarchar(max) NULL;
	Print('MEET_MEETING.Note Added')
END
-----------

IF COLUMNPROPERTY(OBJECT_ID('dbo.COMM_COMMUNICATION_MEETING'), 'Note ', 'ColumnId') IS NULL
BEGIN
	ALTER TABLE COMM_COMMUNICATION_MEETING
	Add Note nvarchar(max) NULL;
	Print('COMM_COMMUNICATION_MEETING.Note Added')
END

----------------

IF COLUMNPROPERTY(OBJECT_ID('dbo.MEET_MEETING'), 'IsReplyForMeetingRequest', 'ColumnId') IS NULL
BEGIN 
ALTER TABLE MEET_MEETING
	ADD IsReplyForMeetingRequest BIT NOT NULL DEFAULT 0
	Print('MEET_MEETING.IsReplyForMeetingRequest Added')
END
------------

IF COLUMNPROPERTY(OBJECT_ID('dbo.MEET_MEETING'), 'CommunicationId', 'ColumnId') IS NULL
BEGIN 
ALTER TABLE MEET_MEETING
	ADD CommunicationId UNIQUEIDENTIFIER NULL
	Print('MEET_MEETING.CommunicationId Added')
END

---------------

IF COLUMNPROPERTY(OBJECT_ID('dbo.COMM_COMMUNICATION_MEETING'), 'IsReplyForMeetingRequest', 'ColumnId') IS NULL
BEGIN 
ALTER TABLE COMM_COMMUNICATION_MEETING
	ADD IsReplyForMeetingRequest BIT NOT NULL DEFAULT 0
	Print('COMM_COMMUNICATION_MEETING.IsReplyForMeetingRequest Added')
END
/*<History Author='Umer Zaman' Date='24-12-2023'> Meeting Script start </History>*/
------------------------
IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_TRANSFER_HISTORY'), 'ApprovalTrackingId', 'ColumnId') IS NULL
BEGIN 
ALTER TABLE CMS_TRANSFER_HISTORY
	ADD ApprovalTrackingId uniqueidentifier NULL
	Print('CMS_TRANSFER_HISTORY.ApprovalTrackingId Added')
END
--------------------------
IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_COPY_HISTORY'), 'ApprovalTrackingId', 'ColumnId') IS NULL
BEGIN 
ALTER TABLE CMS_COPY_HISTORY
	ADD ApprovalTrackingId uniqueidentifier NULL
	Print('CMS_COPY_HISTORY.ApprovalTrackingId Added')
END
-----------------------------
----------------------------ApprovalTrackingStatus
Update ApprovalTrackingStatus Set Id = 3 where Id = 4
Update ApprovalTrackingStatus Set Id = 4 where Id = 8
Update ApprovalTrackingStatus Set Id = 5 where Id = 16
Update ApprovalTrackingStatus Set Id = 6 where Id = 32
Update ApprovalTrackingStatus Set Id = 7 where Id = 64
Update ApprovalTrackingStatus Set Id = 8 where Id = 128
Update ApprovalTrackingStatus Set Id = 9 where Id = 256
Update ApprovalTrackingStatus Set Id = 10 where Id = 131072
Update ApprovalTrackingStatus Set Id = 11 where Id = 262144
Update ApprovalTrackingStatus Set Id = 12 where Id = 134217728
Update ApprovalTrackingStatus Set Id = 13 where Id = 512
Update ApprovalTrackingStatus Set Id = 14 where Id = 1024
Update ApprovalTrackingStatus Set Id = 15 where Id = 2048
Update ApprovalTrackingStatus Set Id = 16 where Id = 4096
Update ApprovalTrackingStatus Set Id = 17 where Id = 8192
Update ApprovalTrackingStatus Set Id = 18 where Id = 16384
Update ApprovalTrackingStatus Set Id = 19 where Id = 32768
Update ApprovalTrackingStatus Set Id = 20 where Id = 65536
Update ApprovalTrackingStatus Set Id = 21 where Id = 268435456
Update ApprovalTrackingStatus Set Id = 22 where Id = 536870912
Update ApprovalTrackingStatus Set Id = 23 where Id = 1073741824
---------------------------------------------------
INSERT INTO ApprovalTrackingStatus (Id , NameEn , NameAr) VALUES (24, 'Pending',N'Pending')


------------------------20-12-2023
IF COLUMNPROPERTY(OBJECT_ID('dbo.LOOKUPS_HISTORY'), 'IsActive', 'ColumnId') IS NULL
BEGIN 
ALTER TABLE LOOKUPS_HISTORY
ADD IsActive bit NULL DEFAULT 0
	Print('LOOKUPS_HISTORY.IsActive Added')
END

-------29/12/2023------
GO
EXEC sp_rename 'COMM_COMMUNICATION_ATTENDEES.[RepresentativeName]', 'RepresentativeNameEn', 'COLUMN';
EXEC sp_rename 'MEET_MEETING_ATTENDEE.[RepresentativeName]', 'RepresentativeNameEn', 'COLUMN';
IF COLUMNPROPERTY(OBJECT_ID('dbo.MEET_MEETING_ATTENDEE'), 'RepresentativeId', 'ColumnId') IS NULL
BEGIN 
ALTER TABLE MEET_MEETING_ATTENDEE
ADD  RepresentativeId UNIQUEIDENTIFIER NULL
	Print('MEET_MEETING_ATTENDEE.RepresentativeId Added')
END
IF COLUMNPROPERTY(OBJECT_ID('dbo.MEET_MEETING_ATTENDEE'), 'RepresentativeNameAr', 'ColumnId') IS NULL
BEGIN 
ALTER TABLE MEET_MEETING_ATTENDEE
ADD  RepresentativeNameAr NVARCHAR(MAX) NULL
	Print('MEET_MEETING_ATTENDEE.RepresentativeNameAr Added')
END
IF COLUMNPROPERTY(OBJECT_ID('dbo.MEET_MEETING_ATTENDEE'), 'RepresentativeNameEN', 'ColumnId') IS NULL
BEGIN 
ALTER TABLE MEET_MEETING_ATTENDEE
ALTER COLUMN  RepresentativeNameEN NVARCHAR(MAX) NULL
Print('MEET_MEETING_ATTENDEE.RepresentativeNameEN Added')
END
IF COLUMNPROPERTY(OBJECT_ID('dbo.COMM_COMMUNICATION_ATTENDEES'), 'RepresentativeNameEN', 'ColumnId') IS NULL
BEGIN 
ALTER TABLE COMM_COMMUNICATION_ATTENDEES
ALTER COLUMN  RepresentativeNameEN NVARCHAR(MAX) NULL
Print('COMM_COMMUNICATION_ATTENDEES.RepresentativeNameEN Added')
END
IF COLUMNPROPERTY(OBJECT_ID('dbo.COMM_COMMUNICATION_ATTENDEES'), 'RepresentativeNameAr', 'ColumnId') IS NULL
BEGIN 
ALTER TABLE COMM_COMMUNICATION_ATTENDEES
ADD  RepresentativeNameAr NVARCHAR(MAX) NULL
Print('COMM_COMMUNICATION_ATTENDEES.RepresentativeNameAr Added')
END
IF COLUMNPROPERTY(OBJECT_ID('dbo.COMM_COMMUNICATION_ATTENDEES'), 'RepresentativeId', 'ColumnId') IS NULL
BEGIN 
ALTER TABLE COMM_COMMUNICATION_ATTENDEES
ADD RepresentativeId UNIQUEIDENTIFIER NULL
Print('COMM_COMMUNICATION_ATTENDEES.RepresentativeId Added')
END

--Change in UMS_User 
IF COLUMNPROPERTY(OBJECT_ID('dbo.UMS_USER'), 'ADUserName', 'ColumnId') IS NULL
BEGIN
ALTER TABLE UMS_USER
ADD ADUserName NVARCHAR(50) NULL;
Print('UMS_USER.ADUserName Added')
END

/*<History Author='Umer Zaman' Date='02-01-2024'> Meeting Script start </History>*/

IF COLUMNPROPERTY(OBJECT_ID('dbo.MEET_MEETING_ATTENDEE'), 'MOMAttendeeStatus', 'ColumnId') IS NULL
BEGIN 
	ALTER TABLE MEET_MEETING_ATTENDEE
	drop column MOMAttendeeStatus
	Print('MEET_MEETING_ATTENDEE.MOMAttendeeStatus Dropped')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.MEET_MEETING_ATTENDEE'), 'MOMRejectionReason', 'ColumnId') IS NULL
BEGIN 
	ALTER TABLE MEET_MEETING_ATTENDEE
	drop column MOMRejectionReason
	Print('MEET_MEETING_ATTENDEE.MOMRejectionReason Dropped')
END
/*<History Author='Umer Zaman' Date='02-01-2024'> Meeting Script end </History>*/
-----------------------------------------DMS_FILE_TYPES_LKP
IF COLUMNPROPERTY(OBJECT_ID('dbo.DMS_FILE_TYPES_LKP'),'Type','ColumnId') IS NULL
BEGIN
ALTER TABLE DMS_FILE_TYPES_LKP
ADD Type NVARCHAR(100) NULL
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.DMS_FILE_TYPES_LKP'),'IsActive','ColumnId') IS NULL
BEGIN
ALTER TABLE DMS_FILE_TYPES_LKP
ADD IsActive BIT NULL
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.DMS_FILE_TYPES_LKP'),'CreatedBy','ColumnId') IS  NULL
BEGIN
ALTER TABLE DMS_FILE_TYPES_LKP
ADD CreatedBy NVARCHAR(250) NOT NULL Default 'superadmin@gmail.com'
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.DMS_FILE_TYPES_LKP'),'CreatedDate','ColumnId') IS  NULL
BEGIN
ALTER TABLE DMS_FILE_TYPES_LKP
ADD CreatedDate DATETIME not null default CURRENT_TIMESTAMP;
END
IF COLUMNPROPERTY(OBJECT_ID('dbo.DMS_FILE_TYPES_LKP'),'ModifiedBy','ColumnId') IS NULL
BEGIN
ALTER TABLE DMS_FILE_TYPES_LKP
ADD ModifiedBy NVARCHAR(250) NULL
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.DMS_FILE_TYPES_LKP'),'ModifiedDate','ColumnId') IS  NULL
BEGIN
ALTER TABLE DMS_FILE_TYPES_LKP
ADD ModifiedDate DATETIME  NULL
END
IF COLUMNPROPERTY(OBJECT_ID('dbo.DMS_FILE_TYPES_LKP'),'DeletedBy','ColumnId') IS NULL
BEGIN
ALTER TABLE DMS_FILE_TYPES_LKP
ADD DeletedBy NVARCHAR(250) NULL
END
IF COLUMNPROPERTY(OBJECT_ID('dbo.DMS_FILE_TYPES_LKP'),'DeletedDate','ColumnId') IS  NULL
BEGIN
ALTER TABLE DMS_FILE_TYPES_LKP
ADD DeletedDate DATETIME  NULL
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.DMS_FILE_TYPES_LKP'),'IsDeleted','ColumnId') IS NULL
BEGIN
ALTER TABLE DMS_FILE_TYPES_LKP
ADD IsDeleted BIT NULL
END


IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_EXECUTION'), 'Amount', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE CMS_EXECUTION
	ALTER COLUMN  Amount DECIMAL(25,2)
	Print('CMS_EXECUTION.Amount Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_EXECUTION'), 'PaidAmount', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE CMS_EXECUTION
	ALTER COLUMN  PaidAmount DECIMAL(25,2)
	Print('CMS_EXECUTION.PaidAmount Altered')
END

-------Change in UMS_User   Author='Attique Rehman' Date='08-01-2024'>
IF COLUMNPROPERTY(OBJECT_ID('dbo.UMS_USER'), 'IsPasswordReset', 'ColumnId') IS NULL
BEGIN 
	ALTER TABLE UMS_USER
	ADD IsPasswordReset BIT DEFAULT 1 not NULL;
	Print('UMS_USER.IsPasswordReset Added')
END
------------ 08/01/2024-----
update NOTIF_NOTIFICATION_EVENT set NameAr = N'طلب جديد'  where EventId =1

update NOTIF_NOTIFICATION_EVENT set NameAr = N'تقديم الطلب'  where EventId =2
update NOTIF_NOTIFICATION_EVENT set NameAr = N'تغيير الحالة'  where EventId =4
update NOTIF_NOTIFICATION_EVENT set NameAr = N'تلقي الرد'  where EventId =8
update NOTIF_NOTIFICATION_EVENT set NameAr = N'تسجيل قضية'  where EventId =16
update NOTIF_NOTIFICATION_EVENT set NameAr = N'فتح ملف'  where EventId =32
update NOTIF_NOTIFICATION_EVENT set NameAr = N'إعلان'  where EventId =64
update NOTIF_NOTIFICATION_EVENT set NameAr = N'حذف طلب'  where EventId =128
update NOTIF_NOTIFICATION_EVENT set NameAr = N'طلب ارجاع'  where EventId = 256

update NOTIF_NOTIFICATION_CATEGORY set NameAr = N'عاجل'  where CategoryId = 1
update NOTIF_NOTIFICATION_CATEGORY set NameAr = N'طبيعي'  where CategoryId = 2
update NOTIF_NOTIFICATION_CATEGORY set NameAr = N'مهم'  where CategoryId = 4
update NOTIF_NOTIFICATION_CATEGORY set NameAr = N'يرجى عدم الرد'  where CategoryId = 8

---------------- 11-jan-2024---------------

WITH CTE AS (
    SELECT EmployeeId,
           ROW_NUMBER() OVER (PARTITION BY employeeid ORDER BY (SELECT NULL)) AS RowNum
    FROM EP_EMPLOYMENT_INFORMATION
)
UPDATE CTE
SET employeeid = employeeid + '1' + CAST(RowNum AS NVARCHAR(10))
WHERE RowNum > 1;

ALTER TABLE EP_EMPLOYMENT_INFORMATION
ADD CONSTRAINT UQ_employeeid UNIQUE (EmployeeId);

------Ammaar Naveed-----11/01/2024-------
IF COLUMNPROPERTY(OBJECT_ID('dbo.EP_EDUCATIONAL_LEVEL'), 'UniversityAddress', 'ColumnId') IS NULL
BEGIN 
	ALTER TABLE EP_EDUCATIONAL_LEVEL
	ADD UniversityAddress NVARCHAR(MAX)
END
-----------

------Ammaar Naveed-----15/01/2024-------
IF COLUMNPROPERTY(OBJECT_ID('dbo.EP_EMPlOYMENT_INFORMATION'), 'FingerPrintId', 'ColumnId') IS NULL
BEGIN
ALTER TABLE EP_EMPlOYMENT_INFORMATION
ADD FingerPrintId VARCHAR(10) NULL
END

-------24-JAN-2024----------
UPDATE UMS_CLAIM set Title_Ar=Title_En;
update UMS_CLAIM set Module='Legislation' where Module='LDS'
UPDATE UMS_CLAIM set IsDeleted=1 where SubModule = 'Literature Type' OR SubModule = 'Literature Classification';



----------------------------------------- 26-01-2024 ----------------------------------------
alter table EP_GRADE
add GradeTypeId int FOREIGN KEY REFERENCES EP_GRADE_TYPE(Id)
Go
alter table EP_EMPLOYMENT_INFORMATION
add ContractTypeId int FOREIGN KEY REFERENCES EP_CONTRACT_TYPE(Id)
GO

----------------------------------------- 26-01-2024 ----------------------------------------


-------26-01-2024
ALTER TABLE MEET_MEETING
ADD IsSendToHOS BIT NULL DEFAULT 0


--------------20-jan-2024----------

ALTER TABLE EP_EMPLOYMENT_INFORMATION
ADD  EmployeeId VARCHAR(30) not null default 1 ;

WITH CTE AS (
    SELECT EmployeeId,
           ROW_NUMBER() OVER (PARTITION BY employeeid ORDER BY (SELECT NULL)) AS RowNum
    FROM EP_EMPLOYMENT_INFORMATION
)
UPDATE CTE
SET employeeid = employeeid + '1' + CAST(RowNum AS NVARCHAR(10))
WHERE RowNum > 1;

alter table EP_EMPLOYMENT_INFORMATION 
add constraint  UQ_EmployeeId unique (EmployeeId,EmployeeTypeId)

UPDATE EP_EMPLOYMENT_INFORMATION
SET GradeId =1 WHERE GradeId IS NULL

ALTER TABLE EP_ADDRESS
ALTER COLUMN Address NVARCHAR(MAX) NOT NULL;

---------------- 2-2-2024 ------------------------
select * from ums_role

update ums_role set NameAr = Name

---------------- 2-2-2024 ------------------------

------------19/2/24------------------- Add ModuleId Column in CMS_OPERATING_SECTOR_TYPE_G2G_LKP
ALTER TABLE [FATWA_DB_DEV].dbo.CMS_OPERATING_SECTOR_TYPE_G2G_LKP
ADD ModuleId INT NULL

UPDATE UMS_USER_ROLES SET IsDeleted = 0 where IsDeleted IS NULL
UPDATE UMS_USER_GROUP SET IsDeleted = 0 where IsDeleted IS NULL
UPDATE UMS_USER_ROLES SET CreatedDate = CURRENT_TIMESTAMP where CreatedDate IS NULL
UPDATE UMS_USER_GROUP SET CreatedDate = CURRENT_TIMESTAMP where CreatedDate IS NULL

ALTER TABLE UMS_USER_ROLES
ALTER COLUMN CreatedDate Datetime NOT NULL

ALTER TABLE UMS_USER_GROUP
ALTER COLUMN CreatedDate Datetime NOT NULL

ALTER TABLE UMS_USER_ROLES
ALTER COLUMN IsDeleted BIT NOT NULL

ALTER TABLE UMS_USER_GROUP
ALTER COLUMN IsDeleted BIT NOT NULL


ALTER TABLE CMS_CASE_REQUEST
ALTER COLUMN RequestTypeId INT NULL

ALTER TABLE UMS_USER 
ALTER COLUMN AllowAccess BIT NOT NULL
update ums_user set AllowAccess = 1 where AllowAccess IS NULL

----Ammaar Naveed---29/02/2024----Revision in implementation for employee floor/building.

--Dropping constraint and column from EP_EMPLOYMENT_INFORMATION
SELECT * FROM EP_EMPLOYMENT_INFORMATION
ALTER TABLE EP_EMPLOYMENT_INFORMATION
DROP CONSTRAINT FK__EP_EMPLOY__Emplo__5CCCA98A;

ALTER TABLE EP_EMPLOYMENT_INFORMATION
DROP COLUMN EmployeePlacementId

--Adding constraint and employee placement id in sector type table
ALTER TABLE CMS_OPERATING_SECTOR_TYPE_G2G_LKP
ADD PlacementId INT NULL
ALTER TABLE CMS_OPERATING_SECTOR_TYPE_G2G_LKP
ADD CONSTRAINT FK_EMP_BUILDING_FLOOR
FOREIGN KEY (PlacementId) REFERENCES EP_FLOOR_BUILDING(Id)

--------------------------------------------------------------------------------

ALTER TABLE UMS_USER 
ALTER COLUMN AllowAccess BIT NOT NULL


ALTER TABLE CMS_CASE_REQUEST 
ALTER COLUMN ClaimAmount decimal(25,3)
		 
ALTER TABLE CMS_EXECUTION 
ALTER COLUMN FileBalance decimal(25,3)
		 
ALTER TABLE CMS_EXECUTION 
ALTER COLUMN PaidAmount decimal(25,3)
		 
ALTER TABLE CMS_EXECUTION 
ALTER COLUMN Amount decimal(25,3)
		 
ALTER TABLE CMS_JUDGEMENT 
ALTER COLUMN Amount decimal(25,3)
		 
ALTER TABLE CMS_JUDGEMENT 
ALTER COLUMN AmountCollected decimal(25,3)
		 
ALTER TABLE CMS_REGISTERED_CASE 
ALTER COLUMN CaseAmount decimal(25,3)
----------------------------------------------
IF COLUMNPROPERTY(OBJECT_ID('dbo.COMM_COMMUNICATION_RESPONSE'), 'PartyEntityId', 'ColumnId') IS NULL
BEGIN 
ALTER TABLE COMM_COMMUNICATION_RESPONSE
	ADD PartyEntityId int NULL
	Print('COMM_COMMUNICATION_RESPONSE.PartyEntityId Added')
END
GO

---------------------------------------DROP JUDGEMENT TYPE CONSTRAINT
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[CMS_JUDGEMENT]') AND type in (N'U'))
ALTER TABLE [dbo].[CMS_JUDGEMENT] DROP CONSTRAINT IF EXISTS  [CMS_JUDGEMENT_TYPE]



-------------- 26-3-2024 --------------

delete from NOTIF_NOTIFICATION_EVENT;

Alter table NOTIF_NOTIFICATION_EVENT
Add ModifiedBy nvarchar(100) null,
ModifiedDate datetime null,
DeletedBy nvarchar(100) null,
DeletedDate datetime null,
IsDeleted bit null default 0,
ReceiverTypeId int foreign key references NOTIF_NOTIFICATION_RECEIVER_TYPE_LKP (ReceiverTypeId),
ReceiverTypeRefId uniqueidentifier;


 UPDATE EP_DESIGNATION SET IsDeleted = 0
  Alter table  EP_DESIGNATION ALTER COLUMN [IsDeleted] [bit]  NOT NULL


  /*<History Author='Umer Zaman' Date='04-04-2024'> Script start </History>*/

ALTER TABLE [dbo].[UMS_USER] ADD  CONSTRAINT [DF_UMS_USERAllowA_0FF747D5]  DEFAULT ((1)) FOR [AllowAccess]
GO

IF COLUMNPROPERTY(OBJECT_ID('dbo.LMS_LITERATURE_DETAILS'), 'ISBN', 'ColumnId') IS NOT NULL
BEGIN
ALTER TABLE LMS_LITERATURE_DETAILS
ALTER COLUMN ISBN nvarchar(80) NULL
END
  
  IF COLUMNPROPERTY(OBJECT_ID('dbo.LMS_LITERATURE_BORROW_DETAILS'), 'ApprovalDate', 'ColumnId') IS NULL
BEGIN
	ALTER TABLE LMS_LITERATURE_BORROW_DETAILS
	Add ApprovalDate DATETIME NULL;
	Print('LMS_LITERATURE_BORROW_DETAILS.ApprovalDate Added')
END
/*<History Author='Umer Zaman' Date='04-04-2024'> Script end </History>*/

-------------- 14-4-2024 ---------------

Alter table NOTIF_NOTIFICATION_EVENT
Add DescriptionEn nvarchar(max) null,
DescriptionAr nvarchar(max) null,
IsActive bit null default 1

update NOTIF_NOTIFICATION_EVENT set IsActive = 1

Alter table NOTIF_NOTIFICATION_TEMPLATE
Add IsActive bit null default 1
update NOTIF_NOTIFICATION_TEMPLATE set IsActive = 1

-------------- 14-4-2024 ---------------

-----------21/04/2024----------

  IF COLUMNPROPERTY(OBJECT_ID('dbo.COMM_COMMUNICATION'), 'DepartmentId', 'ColumnId') IS NULL
BEGIN
ALTER TABLE COMM_COMMUNICATION ADD DepartmentId int NULL
Print('COMM_COMMUNICATION.DepartmentId Added')
END

------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
----------------------------------------------               BUG REPORTING  START            ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

---------------------------------
IF COLUMNPROPERTY(OBJECT_ID('dbo.BUG_TICKET_STATUS_HISTORY'), 'Remarks ', 'ColumnId') IS NULL
BEGIN
	ALTER TABLE BUG_TICKET_STATUS_HISTORY
	Add Remarks nvarchar(max)  null;
	Print('BUG_TICKET_STATUS_HISTORY.Remarks Added')
END
---------------------------
---------------------------
IF COLUMNPROPERTY(OBJECT_ID('dbo.BUG_REPORTED'), 'StatusId', 'ColumnId') IS NULL
BEGIN
ALTER TABLE BUG_REPORTED
ADD StatusId int NULL;
Print('BUG_REPORTED.StatusId Added')
END
-------------------------
IF COLUMNPROPERTY(OBJECT_ID('dbo.BUG_REPORTED'), 'ResolutionDate', 'ColumnId') IS NULL
BEGIN
ALTER TABLE BUG_REPORTED
ADD ResolutionDate DateTime NULL;
Print('BUG_REPORTED.ResolutionDate Added')
END
--------------------------------
IF COLUMNPROPERTY(OBJECT_ID('dbo.BUG_COMMENT'), 'RemarkType', 'ColumnId') IS NULL
BEGIN
ALTER TABLE BUG_COMMENT
ADD RemarkType INT NULL;
Print('BUG_COMMENT.RemarkType Added')
END

---------------------------------------
exec sp_rename 'BUG_COMMENT', 'BUG_COMMENT_FEEDBACK';
---------------------------------------
IF COLUMNPROPERTY(OBJECT_ID('dbo.BUG_ISSUE_TYPE_G2G_LKP'), 'IsSystemGenerated', 'ColumnId') IS NULL
BEGIN
ALTER TABLE BUG_ISSUE_TYPE_G2G_LKP
ADD IsSystemGenerated BIT NULL ;
Print('BUG_ISSUE_TYPE_G2G_LKP.IsSystemGenerated Added')
END
Update BUG_ISSUE_TYPE_G2G_LKP set IsSystemGenerated = 1
--------------------------------------------
EXEC sp_rename 'BUG_TICKET_STATUS_HISTORY.TicketId', 'ReferenceId', 'COLUMN';

------------------------------BUG_ASSIGN_TYPE_MODULE
IF COLUMNPROPERTY(OBJECT_ID('dbo.BUG_ASSIGN_TYPE_MODULE'), 'PriorityId', 'ColumnId') IS NULL
BEGIN
ALTER TABLE BUG_ASSIGN_TYPE_MODULE
ADD PriorityId INT NULL ;
Print('BUG_ASSIGN_TYPE_MODULE.PriorityId Added')
END
----------------------------------
IF COLUMNPROPERTY(OBJECT_ID('dbo.BUG_ASSIGN_TYPE_MODULE'), 'SeverityId', 'ColumnId') IS NULL
BEGIN
ALTER TABLE BUG_ASSIGN_TYPE_MODULE
ADD SeverityId INT NULL ;
Print('BUG_ASSIGN_TYPE_MODULE.SeverityId Added')
END
----------------------------------BUG_ASSIGN_TYPE_USER
IF COLUMNPROPERTY(OBJECT_ID('dbo.BUG_ASSIGN_TYPE_USER'), 'UserId', 'ColumnId') IS NULL
BEGIN
ALTER TABLE BUG_ASSIGN_TYPE_USER
ALTER COLUMN UserId nvarchar(900) NULL;
Print('BUG_ASSIGN_TYPE_USER.UserId updated')
END
-----------------------------------RENAME--BUG_ASSIGN_MODULE_APPLICATION
EXEC sp_rename 'BUG_ASSIGN_MODULE_APPLICATION', 'BUG_MODULE_APPLICATION';
----------------------------------

EXEC sp_rename 'BUG_ASSIGN_TYPE_MODULE', 'BUG_TYPE_MODULE_ASSIGNMENT';
-----------------------------------
EXEC sp_rename 'BUG_ASSIGN_TYPE_USER', 'BUG_TYPE_USER_ASSIGNMENT';
----------------------------------

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[[BUG_TICKET_STATUS_HISTORY]]') AND type in (N'U'))
ALTER TABLE [dbo].[BUG_TICKET_STATUS_HISTORY] DROP CONSTRAINT IF EXISTS [Fk_Ticket]
-------------------------------
 IF COLUMNPROPERTY(OBJECT_ID('dbo.BUG_TICKET'), 'GroupId', 'ColumnId') IS NULL
BEGIN
ALTER TABLE BUG_TICKET
ADD GroupId UNIQUEIDENTIFIER   NULL;
Print('BUG_TICKET.GroupId Added')
END

-----------------------------------------
 IF COLUMNPROPERTY(OBJECT_ID('dbo.BUG_COMMENT_FEEDBACK'), 'ModifiedBy', 'ColumnId') IS NULL
BEGIN
ALTER TABLE BUG_COMMENT_FEEDBACK
ADD ModifiedBy NVARCHAR(100) NULL;
Print('BUG_COMMENT_FEEDBACK.ModifiedBy Added')
END
---------------------------------------------------
 IF COLUMNPROPERTY(OBJECT_ID('dbo.BUG_COMMENT_FEEDBACK'), 'ModifiedDate', 'ColumnId') IS NULL
BEGIN
ALTER TABLE BUG_COMMENT_FEEDBACK
ADD ModifiedDate DateTime NULL;
Print('BUG_COMMENT_FEEDBACK.ModifiedDate Added')
END
--------------------------------------------------
ALTER TABLE BUG_TICKET
ALTER COLUMN Description NVARCHAR(MAX) NULL;
Print('BUG_TICKET.Description Added')
--------------------------------
ALTER TABLE BUG_REPORTED
ALTER COLUMN Description NVARCHAR(MAX)   NULL;
Print('BUG_REPORTED.Description Added')
---------------------------------
 IF COLUMNPROPERTY(OBJECT_ID('dbo.BUG_COMMENT_FEEDBACK'), 'ParentCommentId', 'ColumnId') IS NULL
BEGIN
ALTER TABLE BUG_COMMENT_FEEDBACK
ADD ParentCommentId UNIQUEIDENTIFIER NULL;
Print('BUG_COMMENT_FEEDBACK.ParentCommentId Added')
END
-------------------------
  IF COLUMNPROPERTY(OBJECT_ID('dbo.BUG_COMMENT_FEEDBACK'), 'Rating', 'ColumnId') IS NULL
BEGIN
ALTER TABLE BUG_COMMENT_FEEDBACK
ADD Rating INT NULL;
Print('BUG_COMMENT_FEEDBACK.Rating Added')
END
--------------------
IF COLUMNPROPERTY(OBJECT_ID('dbo.BUG_REPORTED'), 'Subject', 'ColumnId') IS NULL
BEGIN
ALTER TABLE BUG_REPORTED
ADD Subject NVARCHAR(100) NULL;
Print('BUG_REPORTED.Subject Added')
END
-----------------
IF COLUMNPROPERTY(OBJECT_ID('dbo.BUG_TICKET'), 'ResolvedBy', 'ColumnId') IS NULL
BEGIN
ALTER TABLE BUG_TICKET
ADD ResolvedBy NVARCHAR(100) NULL;
Print('BUG_TICKET.ResolvedBy Added')
END
----------------
IF COLUMNPROPERTY(OBJECT_ID('dbo.BUG_ISSUE_TYPE_G2G_LKP'), 'Description', 'ColumnId') IS NULL
BEGIN
ALTER TABLE BUG_ISSUE_TYPE_G2G_LKP
ADD Description NVARCHAR(500) NULL ;
Print('BUG_ISSUE_TYPE_G2G_LKP.Description Added')
END
--------------------BUG_TICKET
alter table BUG_TICKET
add  Subject nvarchar(100)
------------

IF COLUMNPROPERTY(OBJECT_ID('dbo.BUG_TICKET_STATUS_HISTORY'), 'EventId', 'ColumnId') IS NULL
BEGIN
ALTER TABLE BUG_TICKET_STATUS_HISTORY
ADD EventId int NULL ;
Print('BUG_TICKET_STATUS_HISTORY.EventId Added')
END
----------------
IF COLUMNPROPERTY(OBJECT_ID('dbo.BUG_TICKET'), 'PortalId', 'ColumnId') IS NULL
BEGIN
ALTER TABLE BUG_TICKET
ADD PortalId int NULL ;
Print('BUG_TICKET.PortalId Added')
END
-----------------
IF COLUMNPROPERTY(OBJECT_ID('dbo.BUG_TYPE_MODULE_ASSIGNMENT'), 'ApplicationId', 'ColumnId') IS NULL
BEGIN
ALTER TABLE BUG_TYPE_MODULE_ASSIGNMENT
ADD ApplicationId int NULL ;
Print('BUG_TYPE_MODULE_ASSIGNMENT.ApplicationId Added')
END
------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
----------------------------------------------               BUG REPORTING  END            ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-------------------------------------------------------------------BUG REPORTING 30/04/2024


---- 01-05-2024 ----

IF COLUMNPROPERTY(OBJECT_ID('dbo.LEGAL_TEMPLATE'), 'IsDefault', 'ColumnId') IS NULL
BEGIN
	ALTER TABLE LEGAL_TEMPLATE
	Add IsDefault BIT NOT NULL default 0;
	Print('LEGAL_TEMPLATE.IsDefault Added')
END

---- 01-05-2024 ----

------- 6-5-2024 ------------

alter table COMM_Communication_Tarasol_Rpa_Payload
add isSucceeded bit default 0,
CommunicationPayload bit default 0,
CommunicationDocumentPayload bit default 0

update COMM_Communication_Tarasol_Rpa_Payload set isSucceeded = 0
update COMM_Communication_Tarasol_Rpa_Payload set CommunicationPayload = 0
update COMM_Communication_Tarasol_Rpa_Payload set CommunicationDocumentPayload = 0


alter table ums_user
add SignatureImage varchar(max)

update ums_user set SignatureImage =
'\wwwroot\Attachments\UserManagment\Signatures\dpskw.png'

------- 6-5-2024 ------------
---- 01-05-2024 ----


---- 07-05-2024 start ----

IF COLUMNPROPERTY(OBJECT_ID('dbo.LLS_LEGAL_PRINCIPLE'), 'FlowStatus', 'ColumnId') IS NULL
BEGIN 
ALTER TABLE LLS_LEGAL_PRINCIPLE
	ADD FlowStatus INT NOT NULL DEFAULT 0
	Print('LLS_LEGAL_PRINCIPLE.FlowStatus Added')
END

---- 07-05-2024 end ----
------- 6-5-2024 ------------

------- 8-5-2024 ------------
alter table COMM_Communication_Tarasol_Rpa_Payload
add CorrespondenceId varchar(150)
------- 8-5-2024 ------------

--- LLS Legal Principle 16-05-2024

IF COLUMNPROPERTY(OBJECT_ID('dbo.LLS_LEGAL_PRINCIPLE'), 'FlowStatus', 'ColumnId') IS NULL
BEGIN 
ALTER TABLE LLS_LEGAL_PRINCIPLE
	ADD FlowStatus INT NOT NULL DEFAULT 0
	Print('LLS_LEGAL_PRINCIPLE.FlowStatus Added')
END

-----------


EXEC [dbo].pInsTranslation 'Principle_Category_Section',N'قسم فئة المبدأ','Principle Category Section','Legal Principle Add Form Page',1

----

IF COLUMNPROPERTY(OBJECT_ID('dbo.LLS_LEGAL_PRINCIPLE'), 'PrincipleSourceDocumentTypeId', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE LLS_LEGAL_PRINCIPLE
	drop column PrincipleSourceDocumentTypeId 
	Print('LLS_LEGAL_PRINCIPLE.PrincipleSourceDocumentTypeId Dropped')
END

----

IF COLUMNPROPERTY(OBJECT_ID('dbo.LLS_LEGAL_PRINCIPLE'), 'OriginalSourceDocumentId', 'ColumnId') IS NULL
BEGIN
	ALTER TABLE LLS_LEGAL_PRINCIPLE
	Add OriginalSourceDocumentId int not null default 0;  
	Print('LLS_LEGAL_PRINCIPLE.OriginalSourceDocumentId Added')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.LLS_LEGAL_PRINCIPLE'), 'UserId', 'ColumnId') IS NULL
BEGIN
	ALTER TABLE LLS_LEGAL_PRINCIPLE
	Add UserId nvarchar(50) NULL;  
	Print('LLS_LEGAL_PRINCIPLE.UserId Added')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.LLS_LEGAL_PRINCIPLE'), 'RoleId', 'ColumnId') IS NULL
BEGIN
	ALTER TABLE LLS_LEGAL_PRINCIPLE
	Add RoleId nvarchar(50) NULL;  
	Print('LLS_LEGAL_PRINCIPLE.RoleId Added')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.LLS_LEGAL_PRINCIPLE'), 'Principle_Comment', 'ColumnId') IS NULL
BEGIN
	ALTER TABLE LLS_LEGAL_PRINCIPLE
	Add Principle_Comment nvarchar(max) NULL;  
	Print('LLS_LEGAL_PRINCIPLE.Principle_Comment Added')
END

--- LLS Legal Principle 16-05-2024

--- Literature 20-05-2024 start
IF COLUMNPROPERTY(OBJECT_ID('dbo.LMS_LITERATURE_PURCHASE'), 'Price', 'ColumnId') IS NOT NULL
BEGIN
ALTER TABLE LMS_LITERATURE_PURCHASE
ALTER COLUMN Price decimal(25,2)
END
--- Literature 20-05-2024 end

IF COLUMNPROPERTY(OBJECT_ID('dbo.LEGAL_PUBLICATION_SOURCE'), 'Issue_Number', 'ColumnId') IS NULL
BEGIN
	 ALTER TABLE LEGAL_PUBLICATION_SOURCE
	drop column Issue_Number;  
	Print('LEGAL_PUBLICATION_SOURCE.Issue_Number Dropped')
END

 IF COLUMNPROPERTY(OBJECT_ID('dbo.LEGAL_PUBLICATION_SOURCE'), 'Issue_Number', 'ColumnId') IS NULL
BEGIN
	 ALTER TABLE LEGAL_PUBLICATION_SOURCE
	add Issue_Number int not null default 0;   
	Print('LEGAL_PUBLICATION_SOURCE.Issue_Number Added')
END


---- LLS Legal Principle 26-05-2024 start

IF COLUMNPROPERTY(OBJECT_ID('dbo.LLS_LEGAL_PRINCIPLE_CONTENT'),'CreatedBy','ColumnId') IS  NULL
BEGIN
ALTER TABLE LLS_LEGAL_PRINCIPLE_CONTENT
ADD CreatedBy NVARCHAR(250) NOT NULL Default 'fatwaadmin@gmail.com'
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.LLS_LEGAL_PRINCIPLE_CONTENT'),'CreatedDate','ColumnId') IS  NULL
BEGIN
ALTER TABLE LLS_LEGAL_PRINCIPLE_CONTENT
ADD CreatedDate DATETIME not null default CURRENT_TIMESTAMP;
END
IF COLUMNPROPERTY(OBJECT_ID('dbo.LLS_LEGAL_PRINCIPLE_CONTENT'),'ModifiedBy','ColumnId') IS NULL
BEGIN
ALTER TABLE LLS_LEGAL_PRINCIPLE_CONTENT
ADD ModifiedBy NVARCHAR(250) NULL
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.LLS_LEGAL_PRINCIPLE_CONTENT'),'ModifiedDate','ColumnId') IS  NULL
BEGIN
ALTER TABLE LLS_LEGAL_PRINCIPLE_CONTENT
ADD ModifiedDate DATETIME  NULL
END
IF COLUMNPROPERTY(OBJECT_ID('dbo.LLS_LEGAL_PRINCIPLE_CONTENT'),'DeletedBy','ColumnId') IS NULL
BEGIN
ALTER TABLE LLS_LEGAL_PRINCIPLE_CONTENT
ADD DeletedBy NVARCHAR(250) NULL
END
IF COLUMNPROPERTY(OBJECT_ID('dbo.LLS_LEGAL_PRINCIPLE_CONTENT'),'DeletedDate','ColumnId') IS  NULL
BEGIN
ALTER TABLE LLS_LEGAL_PRINCIPLE_CONTENT
ADD DeletedDate DATETIME NULL
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.LLS_LEGAL_PRINCIPLE_CONTENT'),'IsDeleted','ColumnId') IS NULL
BEGIN
ALTER TABLE LLS_LEGAL_PRINCIPLE_CONTENT
ADD IsDeleted BIT NOT NULL DEFAULT 0
END
---- LLS Legal Principle 26-05-2024 end



 IF COLUMNPROPERTY(OBJECT_ID('dbo.COMM_COMMUNICATION'), 'ReturnCorrespondence', 'ColumnId') IS NULL
BEGIN
	 ALTER TABLE COMM_COMMUNICATION
	add ReturnCorrespondence BIT not null default 0;   
	Print('COMM_COMMUNICATION.ReturnCorrespondence Added')
END

 IF COLUMNPROPERTY(OBJECT_ID('dbo.COMM_COMMUNICATION'), 'Archive', 'ColumnId') IS NULL
BEGIN
	 ALTER TABLE COMM_COMMUNICATION
	add Archive BIT not null default 0;   
	Print('COMM_COMMUNICATION.Archive Added')
END



--- LLS Legal Principle 30-05-2024
IF COLUMNPROPERTY(OBJECT_ID('dbo.LLS_LEGAL_PRINCIPLE_CONTENT_SOURCE_DOCUMENT_REFERENCE'),'IsDeleted','ColumnId') IS NULL
BEGIN
ALTER TABLE LLS_LEGAL_PRINCIPLE_CONTENT_SOURCE_DOCUMENT_REFERENCE
ADD IsDeleted BIT NOT NULL DEFAULT 0
END

---- LLS Legal Principle 30-05-2024

----------- LLS Literature Lookups Date : 27-05-2024------------

IF COLUMNPROPERTY(OBJECT_ID('dbo.LITERATURE_DEWEY_NUMBER_PATTERN'), 'SeperatorPattern', 'ColumnId') IS NULL
BEGIN
	 ALTER TABLE LITERATURE_DEWEY_NUMBER_PATTERN
	add SeperatorPattern NVARCHAR(50) not null default '/';   
	Print('LITERATURE_DEWEY_NUMBER_PATTERN.SeperatorPattern Added')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.LITERATURE_DEWEY_NUMBER_PATTERN'), 'SeriesSequenceNumber', 'ColumnId') IS NULL
BEGIN
	 ALTER TABLE LITERATURE_DEWEY_NUMBER_PATTERN
	add SeriesSequenceNumber varchar(50) not null default 00;   
	Print('LITERATURE_DEWEY_NUMBER_PATTERN.SeriesSequenceNumber Added')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.LITERATURE_DEWEY_NUMBER_PATTERN'), 'CheracterSeriesOrder', 'ColumnId') IS NULL
BEGIN
	 ALTER TABLE LITERATURE_DEWEY_NUMBER_PATTERN
	add CheracterSeriesOrder int not null default 0;   
	Print('LITERATURE_DEWEY_NUMBER_PATTERN.CheracterSeriesOrder Added')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.LITERATURE_DEWEY_NUMBER_PATTERN'), 'DigitSequnceOrder', 'ColumnId') IS NULL
BEGIN
	 ALTER TABLE LITERATURE_DEWEY_NUMBER_PATTERN
	add DigitSequnceOrder int not null default 1;   
	Print('LITERATURE_DEWEY_NUMBER_PATTERN.DigitSequnceOrder Added')
END

----------- LLS Literature  Date : 02-06-2024------------

IF COLUMNPROPERTY(OBJECT_ID('dbo.LMS_LITERATURE_DETAILS'), 'LiteratureUniqueId', 'ColumnId') IS NULL
BEGIN
	 ALTER TABLE LMS_LITERATURE_DETAILS 
	add LiteratureUniqueId UNIQUEIDENTIFIER  not null default NEWID();   
	Print('LMS_LITERATURE_DETAILS.LiteratureUniqueId Added')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.LMS_LITERATURE_DETAILS'), 'SeriesSequenceNumber', 'ColumnId') IS NULL
BEGIN
	 ALTER TABLE LMS_LITERATURE_DETAILS 
	add SeriesSequenceNumber varchar(50) not null default '01';   
	Print('LMS_LITERATURE_DETAILS.SeriesSequenceNumber Added')
END
-------------------Muhammad Ali ----------CMS_CHAMBER_G2G_LKP ----------Drop contraint and column-------27-06-2024------ 
--Step 1: Drop the constraint From CMS_CHAMBER_G2G_LKP
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[CMS_CHAMBER_G2G_LKP]') AND type in (N'U'))
ALTER TABLE [dbo].[CMS_CHAMBER_G2G_LKP] DROP CONSTRAINT IF EXISTS [CMS_CHAMBER_COURT]

-- Step 2: Drop the column
IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_CHAMBER_G2G_LKP'), 'CourtId', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE CMS_CHAMBER_G2G_LKP
	drop column CourtId 
	Print('CMS_CHAMBER_G2G_LKP.CourtId Dropped')
END
------------End------------
--------------------------------Muhammad Ali---------History Lookup at Fatwa------------Add Columns ---------08-07-2024-----
IF COLUMNPROPERTY(OBJECT_ID('dbo.LOOKUPS_HISTORY'), 'TagNo', 'ColumnId') IS NULL
BEGIN
	ALTER TABLE LOOKUPS_HISTORY
	Add TagNo nvarchar(255) null;
	Print('LOOKUPS_HISTORY.TagNo Added')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.LOOKUPS_HISTORY'), 'Description', 'ColumnId') IS NULL
BEGIN
	ALTER TABLE LOOKUPS_HISTORY
	Add [Description] nvarchar(255) null;
	Print('LOOKUPS_HISTORY.Description Added')
END
----------End--------------

-------------------CMS_LAWYER_CHAMBER_NUMBER_ASSIGNMENT---

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_LAWYER_CHAMBER_NUMBER_ASSIGNMENT'), 'CourtId', 'ColumnId') IS NOT NULL
BEGIN
ALter table CMS_LAWYER_CHAMBER_NUMBER_ASSIGNMENT
Add CourtId int Not NUll default 0
Print('CMS_LAWYER_CHAMBER_NUMBER_ASSIGNMENT.CourtId Added')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_LAWYER_CHAMBER_NUMBER_ASSIGNMENT'), 'ChamberId', 'ColumnId') IS NOT NULL
BEGIN
ALter table CMS_LAWYER_CHAMBER_NUMBER_ASSIGNMENT
Add ChamberId int Not NUll default 0 
Print('CMS_LAWYER_CHAMBER_NUMBER_ASSIGNMENT.ChamberId Added')
END
---------end--------------
------------End------------


ALTER TABLE TSK_TODO_LIST ALTER COLUMN Description NVARCHAR(MAX)



ALTER TABLE [dbo].[CMS_CASE_REQUEST] DROP CONSTRAINT [CMS_CASE_REQUEST_DEPARTMENT]
GO

ALTER TABLE [dbo].[CMS_CASE_REQUEST]  WITH CHECK ADD  CONSTRAINT [CMS_CASE_REQUEST_DEPARTMENT] FOREIGN KEY([DepartmentId])
REFERENCES [dbo].[CMS_GOVERNMENT_ENTITY_DEPARTMENT_G2G_LKP] ([Id])
GO

--- LLS start 14-08-2024
IF COLUMNPROPERTY(OBJECT_ID('dbo.LMS_LITERATURE_AUTHOR'), 'Address_En', 'ColumnId') IS NOT NULL
BEGIN
ALTER TABLE LMS_LITERATURE_AUTHOR
ALTER COLUMN Address_En nvarchar(max) NULL
Print('LMS_LITERATURE_AUTHOR.Address_En Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.LMS_LITERATURE_AUTHOR'), 'Address_Ar', 'ColumnId') IS NOT NULL
BEGIN
ALTER TABLE LMS_LITERATURE_AUTHOR
ALTER COLUMN Address_Ar nvarchar(max) NULL
Print('LMS_LITERATURE_AUTHOR.Address_Ar Altered')
END
----- LLS end 14-08-2024

--- author Arshad khan (29/08/24)
IF COLUMNPROPERTY(OBJECT_ID('dbo.LMS_LITERATURE_BARCODE'), 'RFIDValue', 'ColumnId') IS NULL
BEGIN 
ALTER TABLE LMS_LITERATURE_BARCODE
	ADD RFIDValue INT DEFAULT 0
	Print('LMS_LITERATURE_BARCODE.RFIDValue Added')
END
-----
----- LLS end 14-08-2024





 ALTER TABLE LMS_LITERATURE_DETAILS ALTER COLUMN Subject_En NVARCHAR(MAX)
 ALTER TABLE LMS_LITERATURE_DETAILS ALTER COLUMN Subject_Ar NVARCHAR(MAX)


 ALTER TABLE LEGAL_LEGISLATION 
ALTER COLUMN [IssueDate_Hijri] [datetime2](7) NULL


ALTER TABLE LEGAL_PUBLICATION_SOURCE 
ALTER COLUMN PublicationDate_Hijri [datetime2](7) NULL


ALTER TABLE legal_principle 
ALTER COLUMN [IssueDate_Hijri] [datetime2](7) NULL

ALTER TABLE LEGAL_PRINCIPLE_PUBLICATION_SOURCE 
ALTER COLUMN PublicationDate_Hijri [datetime2](7) NULL
----- LLS end 14-08-2024


---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------- LMS STOCKTAKING START -----------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


IF COLUMNPROPERTY(OBJECT_ID('dbo.LMS_BARCODE_STOCKTAKING_REMARKS'),'CreatedBy','ColumnId') IS  NULL
BEGIN
ALTER TABLE LMS_BARCODE_STOCKTAKING_REMARKS
ADD CreatedBy NVARCHAR(250) NOT NULL Default 'fatwaadmin@gmail.com'
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.LMS_BARCODE_STOCKTAKING_REMARKS'),'CreatedDate','ColumnId') IS  NULL
BEGIN
ALTER TABLE LMS_BARCODE_STOCKTAKING_REMARKS
ADD CreatedDate DATETIME not null default CURRENT_TIMESTAMP;
END
IF COLUMNPROPERTY(OBJECT_ID('dbo.LMS_BARCODE_STOCKTAKING_REMARKS'),'ModifiedBy','ColumnId') IS NULL
BEGIN
ALTER TABLE LMS_BARCODE_STOCKTAKING_REMARKS
ADD ModifiedBy NVARCHAR(250) NULL
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.LMS_BARCODE_STOCKTAKING_REMARKS'),'ModifiedDate','ColumnId') IS  NULL
BEGIN
ALTER TABLE LMS_BARCODE_STOCKTAKING_REMARKS
ADD ModifiedDate DATETIME  NULL
END
IF COLUMNPROPERTY(OBJECT_ID('dbo.LMS_BARCODE_STOCKTAKING_REMARKS'),'DeletedBy','ColumnId') IS NULL
BEGIN
ALTER TABLE LMS_BARCODE_STOCKTAKING_REMARKS
ADD DeletedBy NVARCHAR(250) NULL
END
IF COLUMNPROPERTY(OBJECT_ID('dbo.LMS_BARCODE_STOCKTAKING_REMARKS'),'DeletedDate','ColumnId') IS  NULL
BEGIN
ALTER TABLE LMS_BARCODE_STOCKTAKING_REMARKS
ADD DeletedDate DATETIME NULL
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.LMS_BARCODE_STOCKTAKING_REMARKS'),'IsDeleted','ColumnId') IS NULL
BEGIN
ALTER TABLE LMS_BARCODE_STOCKTAKING_REMARKS
ADD IsDeleted BIT NOT NULL DEFAULT 0
END
-----------------------------
IF COLUMNPROPERTY(OBJECT_ID('DBO.LMS_LITERATURE_STOCKTAKING'),'ReportNumber' , 'ColumnId') IS  NULL
BEGIN
ALTER TABLE LMS_LITERATURE_STOCKTAKING
ADD  ReportNumber NVARCHAR(256) NOT NULL DEFAULT ''
PRINT('LMS_LITERATURE_STOCKTAKING.ReportNumber Added' )
END
----------------------
IF COLUMNPROPERTY(OBJECT_ID('DBO.LMS_LITERATURE_STOCKTAKING'),'ShortNumber' , 'ColumnId') IS  NULL
BEGIN
ALTER TABLE LMS_LITERATURE_STOCKTAKING
ADD  ShortNumber INT NULL
PRINT('LMS_LITERATURE_STOCKTAKING.ShortNumber Added' )
END
---------------------
IF COLUMNPROPERTY(OBJECT_ID('dbo.LMS_LITERATURE_STOCKTAKING'),'StockTakingPerformerId' ,'ColumnId') IS NOT NULL
BEGIN
ALTER TABLE LMS_LITERATURE_STOCKTAKING
DROP COLUMN StockTakingPerformerId
PRINT('LMS_LITERATURE_STOCKTAKING.StockTakingPerformerId has been dropped')
END
--------------------
IF COLUMNPROPERTY (OBJECT_ID('DBO.LMS_LITERATURE_STOCKTAKING') , 'Note' , 'ColumnId') IS NOT NULL
BEGIN
ALTER TABLE LMS_LITERATURE_STOCKTAKING 
ALTER COLUMN Note NVARCHAR(max) NULL
PRINT('LMS_LITERATURE_STOCKTAKING.Note altered')
END
--------------------
IF COLUMNPROPERTY (OBJECT_ID('DBO.LMS_BARCODE_STOCKTAKING_REMARKS') , 'Remarks' , 'ColumnId') IS NOT NULL
BEGIN
ALTER TABLE LMS_BARCODE_STOCKTAKING_REMARKS 
ALTER COLUMN Remarks NVARCHAR(max) NULL
PRINT('LMS_BARCODE_STOCKTAKING_REMARKS.Remarks altered')
END
GO
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------- LMS STOCKTAKING END -----------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
--------------- 2-10-2024 --------------------------

CREATE or alter    PROCEDURE [dbo].[pTaskViewEntityHistory]                                               
(                                                            
 @referenceId uniqueidentifier = NULL   ,                              
 @submoduleId int = NULL                              
)                                           
AS                                
IF @submoduleId = 1                              
BEGIN                              
Select DISTINCT * from (                              
Select tk.ReferenceId,                              
CASE WHEN ccrs.CreatedBy = 'MOJ RPA' THEN ccre.Name_En + ' By '+'MOJ'+  ISNULL(epi.FirstName_En, '') + ' ' + ISNULL(epi.LastName_En, '')           
 ELSE ccre.Name_En + ' By ' +  ISNULL(epi.FirstName_En, '') + ' ' + ISNULL(epi.LastName_En, '') End AS ActionEn                  
,CASE WHEN ccrs.CreatedBy = 'MOJ RPA' THEN ccre.Name_Ar +  N' من قبل '+'MOJ'+  ISNULL(epi.FirstName_Ar, '') + ' ' + ISNULL(epi.LastName_Ar, '')           
 ELSE ccre.Name_Ar +  N' من قبل ' +  ISNULL(epi.FirstName_Ar, '') + ' ' + ISNULL(epi.LastName_Ar, '') End  AS ActionAr                
,ccrs.CreatedDate                              
,ISNULL(epi.FirstName_En, '') + ' ' + ISNULL(epi.LastName_En, '') AS UserNameEn                              
,ISNULL(epi.FirstName_Ar, '') + ' ' + ISNULL(epi.LastName_Ar, '') AS UserNameAr                        
,ccr.RequestNumber as ReferenceNumber                      
,Null as CANNumber                      
from TSK_TASK tk                               
LEFT JOIN CMS_WITHDRAW_REQUEST wr on wr.Id = tk.ReferenceId                                                    
LEFT JOIN MEET_MEETING mm on mm.MeetingId = tk.ReferenceId                                                 
LEFT JOIN CMS_CASE_REQUEST ccr on ccr.RequestId = tk.ReferenceId OR ccr.RequestId = wr.CaseRequestId    or ccr.RequestId = mm.ReferenceGuid                                 
INNER JOIN CMS_CASE_REQUEST_STATUS_HISTORY ccrs ON ccrs.RequestId = ccr.RequestId                              
LEFT JOIN CMS_CASE_REQUEST_EVENT_G2G_LKP ccre ON ccre.Id = ccrs.EventId                              
LEFT JOIN UMS_USER uu ON uu.Email = ccrs.CreatedBy                              
LEFT JOIN EP_PERSONAL_INFORMATION epi ON epi.UserId = uu.Id                              
UNION ALL                              
Select tk.ReferenceId,                              
ats.NameEn AS ActionEn                              
,ats.NameAr  AS ActionAr                              
,cth.CreatedDate                              
,ISNULL(epi.FirstName_En, '') + ' ' + ISNULL(epi.LastName_En, '') AS UserNameEn                              
,ISNULL(epi.FirstName_Ar, '') + ' ' + ISNULL(epi.LastName_Ar, '') AS UserNameAr                         
,ccr.RequestNumber as ReferenceNumber                      
,Null as CANNumber                      
from TSK_TASK tk                               
LEFT JOIN CMS_WITHDRAW_REQUEST wr on wr.Id = tk.ReferenceId                                                    
LEFT JOIN MEET_MEETING mm on mm.MeetingId = tk.ReferenceId                                                 
LEFT JOIN CMS_CASE_REQUEST ccr on ccr.RequestId = tk.ReferenceId OR ccr.RequestId = wr.CaseRequestId or ccr.RequestId = mm.ReferenceGuid                                 
INNER JOIN CMS_TRANSFER_HISTORY cth ON cth.ReferenceId = ccr.RequestId                              
INNER JOIN ApprovalTrackingStatus ats ON ats.Id = cth.StatusId                              
LEFT JOIN UMS_USER uu ON uu.Email = cth.CreatedBy                              
LEFT JOIN EP_PERSONAL_INFORMATION epi ON epi.UserId = uu.Id                              
where Tk.ReferenceId = @referenceId    
UNION ALL   
Select TK.ReferenceId,                              
ats.Type_En + ' ' + 'Signed' + ' By ' + ISNULL(epic.FirstName_En, '') + ' ' + ISNULL(epic.LastName_En, '') AS ActionEn                              
,ats.Type_Ar + ' ' + 'Signed' + N' من قبل ' +  ISNULL(epic.FirstName_Ar, '') + ' ' + ISNULL(epic.LastName_Ar, '')  AS ActionAr                              
,dsrl.CreatedDate                              
,ISNULL(epic.FirstName_En, '') + ' ' + ISNULL(epic.LastName_En, '') AS UserNameEn                              
,ISNULL(epic.FirstName_Ar, '') + ' ' + ISNULL(epic.LastName_Ar, '') AS UserNameAr                         
,ccf.FileNumber as ReferenceNumber                      
,null as CANNumber                      
from TSK_TASK tk                               
LEFT JOIN DMS_DB_DEV.dbo.UPLOADED_DOCUMENT ud on ud.ReferenceGuid = tk.ReferenceId                              
LEFT JOIN DMS_DB_DEV.dbo.DS_SIGNING_REQUEST_LOG dsrl ON dsrl.DocumentId = ud.UploadedDocumentId                             
LEFT JOIN CMS_CASE_FILE ccf on ccf.FileId = tk.ReferenceId                                
LEFT JOIN DMS_DB_DEV.dbo.ATTACHMENT_TYPE ats on ats.AttachmentTypeId = ud.AttachmentTypeId    
LEFT JOIN EP_PERSONAL_INFORMATION epic on epic.CivilId = dsrl.CivilId  
where dsrl.RequestStatus= 'Approved'  
)CaseRequestView                               
WHERE CaseRequestView.ReferenceId =@referenceId                              
order by CaseRequestView.CreatedDate DESC                              
END                              
                              
ELSE IF @submoduleId = 2                              
BEGIN                              
Select DISTINCT * from (                              
Select tk.ReferenceId,                              
CASE WHEN ccfs.CreatedBy = 'MOJ RPA' THEN ccfe.Name_En + ' By '+'MOJ'+  ISNULL(epi.FirstName_En, '') + ' ' + ISNULL(epi.LastName_En, '')           
 ELSE ccfe.Name_En + ' By ' +  ISNULL(epi.FirstName_En, '') + ' ' + ISNULL(epi.LastName_En, '') End  AS ActionEn                 
,CASE WHEN ccfs.CreatedBy = 'MOJ RPA' THEN ccfe.Name_Ar +  N' من قبل '+'MOJ'+  ISNULL(epi.FirstName_Ar, '') + ' ' + ISNULL(epi.LastName_Ar, '')           
 ELSE ccfe.Name_Ar +  N' من قبل ' +  ISNULL(epi.FirstName_Ar, '') + ' ' + ISNULL(epi.LastName_Ar, '') End AS ActionAr                 
,ccfs.CreatedDate                              
,CASE WHEN ccfs.CreatedBy = 'MOJ RPA' THEN 'MOJ' ELSE ISNULL(epi.FirstName_En, '') + ' ' + ISNULL(epi.LastName_En, '') END AS UserNameEn                              
,CASE WHEN ccfs.CreatedBy = 'MOJ RPA' THEN 'MOJ' ELSE ISNULL(epi.FirstName_Ar, '') + ' ' + ISNULL(epi.LastName_Ar, '') END AS UserNameAr                        
,ccf.FileNumber as ReferenceNumber                      
,null as  CANNumber                      
from  TSK_TASK tk                              
 LEFT JOIN MEET_MEETING mm on mm.MeetingId = tk.ReferenceId                                                 
 LEFT JOIN CMS_DRAFTED_TEMPLATE cdt on cdt.Id = tk.ReferenceId                               
 LEFT JOIN CMS_CASE_FILE ccf on ccf.FileId = tk.ReferenceId OR ccf.FileId = cdt.ReferenceId or ccf.FileId = mm.ReferenceGuid                                                                                
INNER JOIN CMS_CASE_FILE_STATUS_HISTORY ccfs ON ccfs.FileId = ccf.FileId                              
LEFT JOIN CMS_CASE_FILE_EVENT_G2G_LKP ccfe ON ccfe.Id = ccfs.EventId                              
LEFT JOIN UMS_USER uu ON uu.Email = ccfs.CreatedBy                              
LEFT JOIN EP_PERSONAL_INFORMATION epi ON epi.UserId = uu.Id                       
UNION ALL                              
Select tk.ReferenceId,                              
ats.NameEn AS ActionEn                              
,ats.NameAr  AS ActionAr                              
,cth.CreatedDate                              
,ISNULL(epic.FirstName_En, '') + ' ' + ISNULL(epic.LastName_En, '') AS UserNameEn                              
,ISNULL(epic.FirstName_Ar, '') + ' ' + ISNULL(epic.LastName_Ar, '') AS UserNameAr                        
,ccf.FileNumber as ReferenceNumber                      
,Null as CANNumber                      
from TSK_TASK tk                             
 LEFT JOIN MEET_MEETING mm on mm.MeetingId = tk.ReferenceId                                                 
 LEFT JOIN CMS_DRAFTED_TEMPLATE cdt on cdt.Id = tk.ReferenceId                               
 LEFT JOIN CMS_CASE_FILE ccf on ccf.FileId = tk.ReferenceId OR ccf.FileId = cdt.ReferenceId or ccf.FileId = mm.ReferenceGuid                               
INNER JOIN CMS_TRANSFER_HISTORY cth ON cth.ReferenceId = ccf.FileId                              
INNER JOIN ApprovalTrackingStatus ats ON ats.Id = cth.StatusId                              
LEFT JOIN CMS_OPERATING_SECTOR_TYPE_G2G_LKP cost ON cost.Id = cth.SectorFrom                              
LEFT JOIN UMS_USER uu ON uu.Email = cth.CreatedBy                              
LEFT JOIN EP_PERSONAL_INFORMATION epic ON epic.UserId = uu.Id                              
UNION ALL                              
Select TK.ReferenceId,                              
ats.Type_En + ' ' + cdae.NameEn + ' By ' + ISNULL(epic.FirstName_En, '') + ' ' + ISNULL(epic.LastName_En, '') AS ActionEn                              
,ats.Type_Ar + ' ' + cdae.NameAr + N' من قبل ' +  ISNULL(epic.FirstName_Ar, '') + ' ' + ISNULL(epic.LastName_Ar, '')  AS ActionAr                              
,cdtvl.CreatedDate                              
,ISNULL(epic.FirstName_En, '') + ' ' + ISNULL(epic.LastName_En, '') AS UserNameEn                              
,ISNULL(epic.FirstName_Ar, '') + ' ' + ISNULL(epic.LastName_Ar, '') AS UserNameAr                         
,ccf.FileNumber as ReferenceNumber                      
,null as CANNumber                      
from TSK_TASK tk                               
 LEFT JOIN MEET_MEETING mm on mm.MeetingId = tk.ReferenceId                              
LEFT JOIN CMS_DRAFTED_TEMPLATE cdt ON cdt.Id = tk.ReferenceId OR cdt.ReferenceId = tk.ReferenceId                             
LEFT JOIN CMS_CASE_FILE ccf on ccf.FileId = tk.ReferenceId OR ccf.FileId = cdt.ReferenceId or ccf.FileId = mm.ReferenceGuid                               
INNER JOIN CMS_DRAFTED_TEMPLATE_VERSIONS cdtv ON cdtv.DraftedTemplateId = cdt.Id            
LEFT JOIN CMS_DRAFTED_TEMPLATE_VERSION_LOGS cdtvl ON cdtv.VersionId=cdtvl.VersionId            
LEFT JOIN CMS_DRAFT_ACTION_ENUM cdae ON cdae.ActionId = cdtvl.ActionId                       
LEFT JOIN DMS_DB_DEV.dbo.ATTACHMENT_TYPE ats on ats.AttachmentTypeId =cdt.AttachmentTypeId                               
LEFT JOIN UMS_USER uuc ON uuc.Email = cdtvl.CreatedBy                                     
LEFT JOIN EP_PERSONAL_INFORMATION epic on epic.UserId = uuc.Id   
UNION ALL   
Select TK.ReferenceId,                              
ats.Type_En + ' ' + 'Signed' + ' By ' + ISNULL(epic.FirstName_En, '') + ' ' + ISNULL(epic.LastName_En, '') AS ActionEn                              
,ats.Type_Ar + ' ' + 'Signed' + N' من قبل ' +  ISNULL(epic.FirstName_Ar, '') + ' ' + ISNULL(epic.LastName_Ar, '')  AS ActionAr                              
,dsrl.CreatedDate                              
,ISNULL(epic.FirstName_En, '') + ' ' + ISNULL(epic.LastName_En, '') AS UserNameEn                              
,ISNULL(epic.FirstName_Ar, '') + ' ' + ISNULL(epic.LastName_Ar, '') AS UserNameAr                         
,ccf.FileNumber as ReferenceNumber                      
,null as CANNumber                      
from TSK_TASK tk                               
LEFT JOIN DMS_DB_DEV.dbo.UPLOADED_DOCUMENT ud on ud.ReferenceGuid = tk.ReferenceId                              
LEFT JOIN DMS_DB_DEV.dbo.DS_SIGNING_REQUEST_LOG dsrl ON dsrl.DocumentId = ud.UploadedDocumentId                             
LEFT JOIN CMS_CASE_FILE ccf on ccf.FileId = tk.ReferenceId                                
LEFT JOIN DMS_DB_DEV.dbo.ATTACHMENT_TYPE ats on ats.AttachmentTypeId = ud.AttachmentTypeId    
LEFT JOIN EP_PERSONAL_INFORMATION epic on epic.CivilId = dsrl.CivilId  
where dsrl.RequestStatus= 'Approved'  
) CaseFileView                               
WHERE CaseFileView.ReferenceId =@referenceId                             
order by CaseFileView.CreatedDate DESC                              
END                              
                               
ELSE IF @submoduleId = 4                              
BEGIN                              
Select DISTINCT * from (                              
Select tk.ReferenceId,                              
CASE WHEN crcs.CreatedBy = 'MOJ RPA' THEN crce.Name_En + ' By '+'MOJ'+  ISNULL(epi.FirstName_En, '') + ' ' + ISNULL(epi.LastName_En, '')           
 ELSE crce.Name_En + ' By '+  ISNULL(epi.FirstName_En, '') + ' ' + ISNULL(epi.LastName_En, '') END AS ActionEn                   
,CASE WHEN crcs.CreatedBy = 'MOJ RPA' THEN crce.Name_Ar +  N' من قبل '+'MOJ'+  ISNULL(epi.FirstName_Ar, '') + ' ' + ISNULL(epi.LastName_Ar, '')           
 ELSE crce.Name_Ar +  N' من قبل ' +  ISNULL(epi.FirstName_Ar, '') + ' ' + ISNULL(epi.LastName_Ar, '') END  AS ActionAr                              
,crcs.CreatedDate                              
,CASE WHEN crcs.CreatedBy = 'MOJ RPA' THEN 'MOJ' ELSE ISNULL(epi.FirstName_En, '') + ' ' + ISNULL(epi.LastName_En, '') END AS UserNameEn                              
,CASE WHEN crcs.CreatedBy = 'MOJ RPA' THEN 'MOJ' ELSE ISNULL(epi.FirstName_Ar, '') + ' ' + ISNULL(epi.LastName_Ar, '') END AS UserNameAr                        
,crc.CaseNumber as ReferenceNumber                      
,crc.CANNumber as CANNumber                      
from TSK_TASK tk                               
 LEFT JOIN CMS_DRAFTED_TEMPLATE cdt on cdt.Id = tk.ReferenceId                               
LEFT JOIN CMS_MOJ_EXECUTION_REQUEST cmer on cmer.Id = tk.ReferenceId                                                                                                 
LEFT JOIN CMS_DOCUMENT_PORTFOLIO_REQUEST cmpr on cmpr.Id = tk.ReferenceId                                
LEFT JOIN  CMS_REGISTERED_CASE crc ON crc.CaseId = tk.ReferenceId OR crc.CaseId = cmer.CaseId OR crc.CaseId = cmpr.CaseId OR crc.CaseId = cdt.ReferenceId                                             
INNER JOIN CMS_REGISTERED_CASE_STATUS_HISTORY crcs ON crcs.CaseId = crc.CaseId                              
LEFT JOIN CMS_REGISTERED_CASE_EVENT_G2G_LKP crce ON crce.Id = crcs.EventId                              
LEFT JOIN UMS_USER uu ON uu.Email = crcs.CreatedBy                              
LEFT JOIN EP_PERSONAL_INFORMATION epi ON epi.UserId = uu.Id                              
UNION ALL                              
Select CH.CaseId As ReferenceId                              
,' Transfer from ' + ISNULL(CCF.Name_En, '') + ' to ' + ISNULL(CCT.Name_En, '') AS ActionEn                              
,ISNULL(CCT.Name_Ar, '') + N' إلى ' + ISNULL(CCF.Name_Ar, '') + N' نقل من'  AS ActionAr                              
,CTH.CreatedDate                              
,ISNULL(epi.FirstName_En, '') + ' ' + ISNULL(epi.LastName_En, '') AS UserNameEn                              
,ISNULL(epi.FirstName_Ar, '') + ' ' + ISNULL(epi.LastName_Ar, '') AS UserNameAr                         
,crc.CaseNumber as ReferenceNumber                      
,crc.CANNumber as CANNumber                      
FROM CMS_RESGISTERED_CASE_TRANSFER_HISTORY CTH                                              
INNER JOIN CMS_CHAMBER_G2G_LKP CCF ON CCF.Id = CTH.ChamberFromId                                        
INNER JOIN CMS_CHAMBER_G2G_LKP CCT ON CCT.Id = CTH.ChamberToId                                  
LEFT JOIN CMS_CHAMBER_NUMBER_G2G_LKP CCNF ON CCNF.Id = CTH.ChamberNumberFromId                                  
LEFT JOIN CMS_CHAMBER_NUMBER_G2G_LKP CCNT ON CCNT.Id = CTH.ChamberNumberToId                                  
LEFT JOIN CMS_OUTCOME_HEARING COH ON COH.Id=CTH.OutcomeId                                  
LEFT JOIN CMS_HEARING CH ON CH.Id=COH.HearingId                                  
LEFT JOIN UMS_USER us on us.UserName=CTH.CreatedBy                                    
LEFT JOIN EP_PERSONAL_INFORMATION epi ON epi.UserId = us.Id                         
LEFT JOIN CMS_REGISTERED_CASE crc ON crc.CaseId=CH.CaseId                        
UNION ALL                              
Select Tk.ReferenceId,                              
ats.Type_En + ' ' + cdae.NameEn + ' By ' + ISNULL(epic.FirstName_En, '') + ' ' + ISNULL(epic.LastName_En, '') AS ActionEn                              
,ats.Type_Ar + ' ' + cdae.NameAr + N' من قبل ' + ISNULL(epic.FirstName_Ar, '') + ' ' +  ISNULL(epic.LastName_Ar, '')  AS ActionAr                              
,cdtvl.CreatedDate                              
,ISNULL(epic.FirstName_En, '') + ' ' + ISNULL(epic.LastName_En, '') AS UserNameEn                              
,ISNULL(epic.FirstName_Ar, '') + ' ' + ISNULL(epic.LastName_Ar, '') AS UserNameAr                        
,crc.CaseNumber as ReferenceNumber                      
,crc.CANNumber as CANNumber                      
from TSK_TASK tk                               
LEFT JOIN CMS_DRAFTED_TEMPLATE cdt ON cdt.Id = tk.ReferenceId OR cdt.ReferenceId = tk.ReferenceId                              
LEFT JOIN CMS_MOJ_EXECUTION_REQUEST cmer on cmer.Id = tk.ReferenceId                                                                                             
LEFT JOIN CMS_DOCUMENT_PORTFOLIO_REQUEST cmpr on cmpr.Id = tk.ReferenceId                                     
LEFT JOIN  CMS_REGISTERED_CASE crc ON crc.CaseId = tk.ReferenceId OR crc.CaseId = cmer.CaseId OR crc.CaseId = cmpr.CaseId OR crc.CaseId = cdt.ReferenceId                                     
INNER JOIN CMS_DRAFTED_TEMPLATE_VERSIONS cdtv ON cdtv.DraftedTemplateId = cdt.Id                              
LEFT JOIN CMS_DRAFTED_TEMPLATE_VERSION_LOGS cdtvl ON cdtv.VersionId=cdtvl.VersionId            
LEFT JOIN CMS_DRAFT_ACTION_ENUM cdae ON cdae.ActionId = cdtvl.ActionId                               
LEFT JOIN DMS_DB_DEV.dbo.ATTACHMENT_TYPE ats on ats.AttachmentTypeId =cdt.AttachmentTypeId                               
LEFT JOIN UMS_USER uuc ON uuc.Email = cdtvl.CreatedBy                                     
LEFT JOIN EP_PERSONAL_INFORMATION epic on epic.UserId = uuc.Id    
UNION ALL   
Select TK.ReferenceId,                              
ats.Type_En + ' ' + 'Signed' + ' By ' + ISNULL(epic.FirstName_En, '') + ' ' + ISNULL(epic.LastName_En, '') AS ActionEn                              
,ats.Type_Ar + ' ' + 'Signed' + N' من قبل ' +  ISNULL(epic.FirstName_Ar, '') + ' ' + ISNULL(epic.LastName_Ar, '')  AS ActionAr                              
,dsrl.CreatedDate                              
,ISNULL(epic.FirstName_En, '') + ' ' + ISNULL(epic.LastName_En, '') AS UserNameEn                              
,ISNULL(epic.FirstName_Ar, '') + ' ' + ISNULL(epic.LastName_Ar, '') AS UserNameAr                         
,ccf.FileNumber as ReferenceNumber                      
,null as CANNumber                      
from TSK_TASK tk                               
LEFT JOIN DMS_DB_DEV.dbo.UPLOADED_DOCUMENT ud on ud.ReferenceGuid = tk.ReferenceId                              
LEFT JOIN DMS_DB_DEV.dbo.DS_SIGNING_REQUEST_LOG dsrl ON dsrl.DocumentId = ud.UploadedDocumentId                             
LEFT JOIN CMS_CASE_FILE ccf on ccf.FileId = tk.ReferenceId                                
LEFT JOIN DMS_DB_DEV.dbo.ATTACHMENT_TYPE ats on ats.AttachmentTypeId = ud.AttachmentTypeId    
LEFT JOIN EP_PERSONAL_INFORMATION epic on epic.CivilId = dsrl.CivilId  
where dsrl.RequestStatus= 'Approved'  
) RegisteredCaseView                               
WHERE RegisteredCaseView.ReferenceId =@referenceId                              
order by RegisteredCaseView.CreatedDate DESC                              
END                              
                              
ELSE                               
IF @submoduleId = 8                              
BEGIN                              
Select DISTINCT * from (                              
Select tk.ReferenceId,                              
CASE WHEN ccrs.CreatedBy = 'MOJ RPA' THEN ccre.Name_En + ' By '+'MOJ'+  ISNULL(epi.FirstName_En, '') + ' ' + ISNULL(epi.LastName_En, '')           
 ELSE ccre.Name_En + ' By ' +  ISNULL(epi.FirstName_En, '') + ' ' + ISNULL(epi.LastName_En, '') End  AS ActionEn                 
,CASE WHEN ccrs.CreatedBy = 'MOJ RPA' THEN ccre.Name_Ar +  N' من قبل '+'MOJ'+  ISNULL(epi.FirstName_Ar, '') + ' ' + ISNULL(epi.LastName_Ar, '')           
 ELSE ccre.Name_Ar +  N' من قبل ' +  ISNULL(epi.FirstName_Ar, '') + ' ' + ISNULL(epi.LastName_Ar, '') END AS ActionAr               
,ccrs.CreatedDate                              
,CASE WHEN ccrs.CreatedBy = 'MOJ RPA' THEN 'MOJ' ELSE ISNULL(epi.FirstName_En, '') + ' ' + ISNULL(epi.LastName_En, '') END AS UserNameEn                        
,CASE WHEN ccrs.CreatedBy = 'MOJ RPA' THEN 'MOJ' ELSE ISNULL(epi.FirstName_Ar, '') + ' ' + ISNULL(epi.LastName_Ar, '') END AS UserNameAr                        
,ccr.RequestNumber as ReferenceNumber                      
,Null as CANNumber                      
from TSK_TASK tk                               
LEFT JOIN COMS_WITHDRAW_REQUEST wr on wr.Id=tk.ReferenceId                                          
LEFT JOIN MEET_MEETING mm on mm.MeetingId = tk.ReferenceId                                
LEFT JOIN COMS_CONSULTATION_REQUEST ccr on ccr.ConsultationRequestId = tk.ReferenceId OR ccr.ConsultationRequestId = wr.ConsultationRequestId     or ccr.ConsultationRequestId = mm.ReferenceGuid                                             
INNER JOIN COMS_CONSULTATION_REQUEST_STATUS_HISTORY ccrs ON ccrs.ConsultationRequestId = ccr.ConsultationRequestId                              
LEFT JOIN CMS_CASE_REQUEST_EVENT_G2G_LKP ccre ON ccre.Id = ccrs.EventId                              
LEFT JOIN UMS_USER uu ON uu.Email = ccrs.CreatedBy                              
LEFT JOIN EP_PERSONAL_INFORMATION epi ON epi.UserId = uu.Id                              
UNION ALL                              
Select tk.ReferenceId,                              
ats.NameEn AS ActionEn                              
,ats.NameAr  AS ActionAr                              
,cth.CreatedDate                              
,ISNULL(epi.FirstName_En, '') + ' ' + ISNULL(epi.LastName_En, '') AS UserNameEn                              
,ISNULL(epi.FirstName_Ar, '') + ' ' + ISNULL(epi.LastName_Ar, '') AS UserNameAr                        
,ccr.RequestNumber as ReferenceNumber                      
,Null as CANNumber                      
from TSK_TASK tk                               
LEFT JOIN COMS_WITHDRAW_REQUEST wr on wr.Id=tk.ReferenceId                                          
LEFT JOIN MEET_MEETING mm on mm.MeetingId = tk.ReferenceId                                
LEFT JOIN COMS_CONSULTATION_REQUEST ccr on ccr.ConsultationRequestId = tk.ReferenceId OR ccr.ConsultationRequestId = wr.ConsultationRequestId     or ccr.ConsultationRequestId = mm.ReferenceGuid                              
INNER JOIN CMS_TRANSFER_HISTORY cth ON cth.ReferenceId = ccr.ConsultationRequestId                              
INNER JOIN ApprovalTrackingStatus ats ON ats.Id = cth.StatusId                              
LEFT JOIN UMS_USER uu ON uu.Email = cth.CreatedBy                              
LEFT JOIN EP_PERSONAL_INFORMATION epi ON epi.UserId = uu.Id                              
)ConsultationRequestView                               
WHERE ConsultationRequestView.ReferenceId =@referenceId                              
order by ConsultationRequestView.CreatedDate DESC                              
END                              
ELSE                               
IF @submoduleId = 64                              
BEGIN                              
Select DISTINCT * from (                              
Select tk.ReferenceId,                              
CASE WHEN ccrs.CreatedBy = 'MOJ RPA' THEN ccre.Name_En + ' By '+'MOJ'+  ISNULL(epi.FirstName_En, '') + ' ' + ISNULL(epi.LastName_En, '')           
 ELSE ccre.Name_En + ' By ' +  ISNULL(epi.FirstName_En, '') + ' ' + ISNULL(epi.LastName_En, '') END AS ActionEn                  
,CASE WHEN ccrs.CreatedBy = 'MOJ RPA' THEN ccre.Name_Ar +  N' من قبل '+'MOJ'+  ISNULL(epi.FirstName_Ar, '') + ' ' + ISNULL(epi.LastName_Ar, '')           
 ELSE ccre.Name_Ar +  N' من قبل ' +   ISNULL(epi.FirstName_Ar, '') + ' ' + ISNULL(epi.LastName_Ar, '') END  AS ActionAr                   
,ccrs.CreatedDate                              
,CASE WHEN ccrs.CreatedBy = 'MOJ RPA' THEN 'MOJ' ELSE ISNULL(epi.FirstName_En, '') + ' ' + ISNULL(epi.LastName_En, '') END AS UserNameEn                              
,CASE WHEN ccrs.CreatedBy = 'MOJ RPA' THEN 'MOJ' ELSE ISNULL(epi.FirstName_Ar, '') + ' ' + ISNULL(epi.LastName_Ar, '') END AS UserNameAr                        
,ccf.FileNumber as ReferenceNumber                      
,Null as CANNumber                      
from TSK_TASK tk                               
LEFT JOIN MEET_MEETING mm on mm.MeetingId = tk.ReferenceId                                
LEFT JOIN CMS_DRAFTED_TEMPLATE cdt on cdt.Id = tk.ReferenceId                                                   
LEFT JOIN COMS_CONSULTATION_FILE ccf on ccf.FileId = tk.ReferenceId   OR ccf.FileId = cdt.ReferenceId   or ccf.FileId = mm.ReferenceGuid                                 
INNER JOIN COMS_CONSULTATION_FILE_STATUS_HISTORY ccrs ON ccrs.FileId = ccf.FileId                              
LEFT JOIN CMS_CASE_FILE_EVENT_G2G_LKP ccre ON ccre.Id = ccrs.EventId                              
LEFT JOIN UMS_USER uu ON uu.Email = ccrs.CreatedBy                              
LEFT JOIN EP_PERSONAL_INFORMATION epi ON epi.UserId = uu.Id       
UNION ALL    
Select tk.ReferenceId,                              
    ats.Type_En + ' (' + ISNULL(crt.Name_En, '') + ') ' + cdae.NameEn + ' ' + ' By ' + ISNULL(epic.FirstName_En, '') + ' ' + ISNULL(epic.LastName_En, '') AS ActionEn,                             
    ats.Type_Ar + ' (' + ISNULL(crt.Name_Ar, '') + ') ' + cdae.NameAr + N' ' + N' من قبل ' + ISNULL(epic.FirstName_Ar, '') + ' ' + ISNULL(epic.LastName_Ar, '') AS ActionAr,                              
    cdtvl.CreatedDate,                              
    ISNULL(epic.FirstName_En, '') + ' ' + ISNULL(epic.LastName_En, '') AS UserNameEn,                              
    ISNULL(epic.FirstName_Ar, '') + ' ' + ISNULL(epic.LastName_Ar, '') AS UserNameAr,                         
    ccf.FileNumber as ReferenceNumber,                       
    null as CANNumber                       
from TSK_TASK tk                               
LEFT JOIN MEET_MEETING mm on mm.MeetingId = tk.ReferenceId                               
LEFT JOIN CMS_DRAFTED_TEMPLATE cdt ON cdt.Id = tk.ReferenceId OR cdt.ReferenceId = tk.ReferenceId                            
LEFT JOIN COMS_CONSULTATION_FILE ccf on ccf.FileId = tk.ReferenceId OR ccf.FileId = cdt.ReferenceId or ccf.FileId = mm.ReferenceGuid                                 
INNER JOIN CMS_DRAFTED_TEMPLATE_VERSIONS cdtv ON cdtv.DraftedTemplateId = cdt.Id                               
LEFT JOIN CMS_DRAFTED_TEMPLATE_VERSION_LOGS cdtvl ON cdtv.VersionId = cdtvl.VersionId            
LEFT JOIN CMS_DRAFT_ACTION_ENUM cdae ON cdae.ActionId = cdtvl.ActionId                          
LEFT JOIN DMS_DB_DEV.dbo.ATTACHMENT_TYPE ats on ats.AttachmentTypeId = cdt.AttachmentTypeId                               
LEFT JOIN UMS_USER uuc ON uuc.Email = cdtvl.CreatedBy                                    
LEFT JOIN EP_PERSONAL_INFORMATION epic on epic.UserId = uuc.Id    
LEFT JOIN LINK_TARGET lt ON lt.ReferenceId = @referenceId -- Keep this JOIN if you always expect to have a referenceId    
LEFT JOIN COMM_COMMUNICATION_TARGET_LINK cctl ON cctl.TargetLinkId = lt.TargetLinkId    
LEFT JOIN COMM_COMMUNICATION_RESPONSE ccr ON ccr.CommunicationId = cctl.CommunicationId    
LEFT JOIN CMS_Response_Type crt ON crt.Id = ccr.ResponseTypeId    
    
--UNION ALL                              
--Select tk.ReferenceId,                              
--ats.Type_En + ' ' + cdae.NameEn + ' By ' + ISNULL(epic.FirstName_En, '') + ' ' + ISNULL(epic.LastName_En, '') AS ActionEn                              
--,ats.Type_Ar + ' ' + cdae.NameAr + N' من قبل ' +  ISNULL(epic.FirstName_Ar, '') + ' ' + ISNULL(epic.LastName_Ar, '')   AS ActionAr                              
--,cdtvl.CreatedDate                              
--,ISNULL(epic.FirstName_En, '') + ' ' + ISNULL(epic.LastName_En, '') AS UserNameEn                              
--,ISNULL(epic.FirstName_Ar, '') + ' ' + ISNULL(epic.LastName_Ar, '') AS UserNameAr                         
--,ccf.FileNumber as ReferenceNumber                       
--,null as CANNumber                      
--from TSK_TASK tk                               
--LEFT JOIN MEET_MEETING mm on mm.MeetingId = tk.ReferenceId                 
--LEFT JOIN CMS_DRAFTED_TEMPLATE cdt ON cdt.Id = tk.ReferenceId OR cdt.ReferenceId = tk.ReferenceId                            
--LEFT JOIN COMS_CONSULTATION_FILE ccf on ccf.FileId = tk.ReferenceId   OR ccf.FileId = cdt.ReferenceId   or ccf.FileId = mm.ReferenceGuid                                 
--INNER JOIN CMS_DRAFTED_TEMPLATE_VERSIONS cdtv ON cdtv.DraftedTemplateId = cdt.Id                              
--LEFT JOIN CMS_DRAFTED_TEMPLATE_VERSION_LOGS cdtvl ON cdtv.VersionId=cdtvl.VersionId            
--LEFT JOIN CMS_DRAFT_ACTION_ENUM cdae ON cdae.ActionId = cdtvl.ActionId                          
--LEFT JOIN DMS_DB_DEV.dbo.ATTACHMENT_TYPE ats on ats.AttachmentTypeId =cdt.AttachmentTypeId                               
--LEFT JOIN UMS_USER uuc ON uuc.Email = cdtvl.CreatedBy                                     
--LEFT JOIN EP_PERSONAL_INFORMATION epic on epic.UserId = uuc.Id      
UNION ALL                              
Select tk.ReferenceId,                              
ats.NameEn AS ActionEn                              
,ats.NameAr  AS ActionAr                              
,cth.CreatedDate                              
,ISNULL(epi.FirstName_En, '') + ' ' + ISNULL(epi.LastName_En, '') AS UserNameEn                              
,ISNULL(epi.FirstName_Ar, '') + ' ' + ISNULL(epi.LastName_Ar, '') AS UserNameAr                        
,ccf.FileNumber as ReferenceNumber                      
,Null as CANNumber                      
from TSK_TASK tk                               
LEFT JOIN MEET_MEETING mm on mm.MeetingId = tk.ReferenceId                                
LEFT JOIN CMS_DRAFTED_TEMPLATE cdt on cdt.Id = tk.ReferenceId                                                   
LEFT JOIN COMS_CONSULTATION_FILE ccf on ccf.FileId = tk.ReferenceId OR ccf.FileId = cdt.ReferenceId   or ccf.FileId = mm.ReferenceGuid                                 
INNER JOIN CMS_TRANSFER_HISTORY cth ON cth.ReferenceId = ccf.FileId                              
INNER JOIN ApprovalTrackingStatus ats ON ats.Id = cth.StatusId                              
LEFT JOIN UMS_USER uu ON uu.Email = cth.CreatedBy                              
LEFT JOIN EP_PERSONAL_INFORMATION epi ON epi.UserId = uu.Id                              
)ConsultationFileView                               
WHERE ConsultationFileView.ReferenceId =@referenceId                              
order by ConsultationFileView.CreatedDate DESC                              
END 

--------------- 2-10-2024 --------------------------


ALTER TABLE CMS_RESGISTERED_CASE_TRANSFER_REQUEST
ADD PRIMARY KEY (Id);




IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_CASE_FILE_TRANSFER_REQUEST'), 'RequestNo', 'ColumnId') IS NULL
BEGIN
	ALTER TABLE CMS_CASE_FILE_TRANSFER_REQUEST
	Add RequestNo NVARCHAR(256) NULL;
END



GO
WITH OrderedRecords AS (
    SELECT 
        ID, 
        ROW_NUMBER() OVER (ORDER BY CreatedDate ASC) AS SeqNum
    FROM CMS_CASE_FILE_TRANSFER_REQUEST
)
UPDATE CMS_CASE_FILE_TRANSFER_REQUEST
SET RequestNo = OrderedRecords.SeqNum
FROM CMS_CASE_FILE_TRANSFER_REQUEST
JOIN OrderedRecords
ON CMS_CASE_FILE_TRANSFER_REQUEST.Id = OrderedRecords.Id;


IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_CASE_FILE_TRANSFER_REQUEST'), 'RequestNo', 'ColumnId') IS NOT NULL
BEGIN
	ALTER TABLE CMS_CASE_FILE_TRANSFER_REQUEST
	ALTER COLUMN RequestNo NVARCHAR(256) NOT NULL;
END




IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_CASE_REQUEST'), 'AssignedBy ', 'ColumnId') IS NULL
BEGIN
	ALTER TABLE CMS_CASE_REQUEST
	Add AssignedBy NVARCHAR(256) NULL;
	Print('CMS_CASE_REQUEST.AssignedBy Added')
END


IF COLUMNPROPERTY(OBJECT_ID('dbo.COMS_CONSULTATION_REQUEST'), 'AssignedBy ', 'ColumnId') IS NULL
BEGIN
	ALTER TABLE COMS_CONSULTATION_REQUEST
	Add AssignedBy NVARCHAR(256) NULL;
	Print('COMS_CONSULTATION_REQUEST.AssignedBy Added')
END



ALTER TABLE CMS_CASE_FILE_TRANSFER_REQUEST_STATUS
ADD PRIMARY KEY (Id);





;WITH CTE AS (
SELECT WorkflowId, nAME, ROW_NUMBER() OVER (PARTITION BY Name ORDER BY WorkflowId) AS RowNum FROM WF_WORKFLOW)
UPDATE WF_WORKFLOW SET Name = CTE.Name + '_' + CAST(RowNum AS NVARCHAR) FROM WF_WORKFLOW WF
JOIN CTE ON WF.WorkflowId = CTE.WorkflowId
WHERE CTE.RowNum > 1;


ALTER TABLE WF_WORKFLOW 
ADD UNIQUE (Name)

-------------------31-Dec-2024----------Muhammad Ali---------Validation/Limits DB Level-----------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EP_DEPARTMENT]') AND type in (N'U'))
BEGIN
update EP_DEPARTMENT set Name_En = LEFT(Name_En, 150) where LEN(Name_En) > 150;
update EP_DEPARTMENT set Name_Ar = LEFT(Name_Ar, 150) where LEN(Name_Ar) > 150;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.EP_DEPARTMENT'), 'Name_En', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE EP_DEPARTMENT ALTER COLUMN Name_En NVARCHAR(150);
	Print('EP_DEPARTMENT.Name_En Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.EP_DEPARTMENT'), 'Name_Ar', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE EP_DEPARTMENT ALTER COLUMN Name_Ar NVARCHAR(150);
	Print('EP_DEPARTMENT.Name_Ar Altered')
END
---------------------------------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EP_NATIONALITY]') AND type in (N'U'))
BEGIN
update EP_NATIONALITY set Name_En = LEFT(Name_En, 150) where LEN(Name_En) > 150;
update EP_NATIONALITY set Name_Ar = LEFT(Name_Ar, 150) where LEN(Name_Ar) > 150;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.EP_NATIONALITY'), 'Name_En', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE EP_NATIONALITY ALTER COLUMN Name_En NVARCHAR(150);
	Print('EP_NATIONALITY.Name_En Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.EP_NATIONALITY'), 'Name_Ar', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE EP_NATIONALITY ALTER COLUMN Name_Ar NVARCHAR(150);
	Print('EP_NATIONALITY.Name_Ar Altered')
END
---------------------------------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EP_GRADE]') AND type in (N'U'))
BEGIN
update EP_GRADE set Name_En = LEFT(Name_En, 150) where LEN(Name_En) > 150;
update EP_GRADE set Name_Ar = LEFT(Name_Ar, 150) where LEN(Name_Ar) > 150;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.EP_GRADE'), 'Name_En', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE EP_GRADE ALTER COLUMN Name_En NVARCHAR(150);
	Print('EP_GRADE.Name_En Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.EP_GRADE'), 'Name_Ar', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE EP_GRADE ALTER COLUMN Name_Ar NVARCHAR(150);
	Print('EP_GRADE.Name_Ar Altered')
END
---------------------------------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EP_GRADE_TYPE]') AND type in (N'U'))
BEGIN
update EP_GRADE_TYPE set Name_En = LEFT(Name_En, 150) where LEN(Name_En) > 150;
update EP_GRADE_TYPE set Name_Ar = LEFT(Name_Ar, 150) where LEN(Name_Ar) > 150;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.EP_GRADE_TYPE'), 'Name_En', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE EP_GRADE_TYPE ALTER COLUMN Name_En NVARCHAR(150);
	Print('EP_GRADE_TYPE.Name_En Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.EP_GRADE_TYPE'), 'Name_Ar', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE EP_GRADE_TYPE ALTER COLUMN Name_Ar NVARCHAR(150);
	Print('EP_GRADE_TYPE.Name_Ar Altered')
END
---------------------------------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EP_DESIGNATION]') AND type in (N'U'))
BEGIN
update EP_DESIGNATION set Name_En = LEFT(Name_En, 150) where LEN(Name_En) > 150;
update EP_DESIGNATION set Name_Ar = LEFT(Name_Ar, 150) where LEN(Name_Ar) > 150;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.EP_DESIGNATION'), 'Name_En', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE EP_DESIGNATION ALTER COLUMN Name_En NVARCHAR(150);
	Print('EP_DESIGNATION.Name_En Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.EP_DESIGNATION'), 'Name_Ar', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE EP_DESIGNATION ALTER COLUMN Name_Ar NVARCHAR(150);
	Print('EP_DESIGNATION.Name_Ar Altered')
END
---------------------------------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EP_CONTRACT_TYPE]') AND type in (N'U'))
BEGIN
update EP_CONTRACT_TYPE set Name_En = LEFT(Name_En, 150) where LEN(Name_En) > 150;
update EP_CONTRACT_TYPE set Name_Ar = LEFT(Name_Ar, 150) where LEN(Name_Ar) > 150;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.EP_CONTRACT_TYPE'), 'Name_En', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE EP_CONTRACT_TYPE ALTER COLUMN Name_En NVARCHAR(150);
	Print('EP_CONTRACT_TYPE.Name_En Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.EP_CONTRACT_TYPE'), 'Name_Ar', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE EP_CONTRACT_TYPE ALTER COLUMN Name_Ar NVARCHAR(150);
	Print('EP_CONTRACT_TYPE.Name_Ar Altered')
END
---------------------------------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EP_Gender]') AND type in (N'U'))
BEGIN
update EP_Gender set Name_En = LEFT(Name_En, 150) where LEN(Name_En) > 150;
update EP_Gender set Name_Ar = LEFT(Name_Ar, 150) where LEN(Name_Ar) > 150;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.EP_Gender'), 'Name_En', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE EP_Gender ALTER COLUMN Name_En NVARCHAR(150);
	Print('EP_Gender.Name_En Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.EP_Gender'), 'Name_Ar', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE EP_Gender ALTER COLUMN Name_Ar NVARCHAR(150);
	Print('EP_Gender.Name_Ar Altered')
END
---------------------------------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_OPERATING_SECTOR_TYPE_G2G_LKP]') AND type in (N'U'))
BEGIN
update CMS_OPERATING_SECTOR_TYPE_G2G_LKP set Name_En = LEFT(Name_En, 150) where LEN(Name_En) > 150;
update CMS_OPERATING_SECTOR_TYPE_G2G_LKP set Name_Ar = LEFT(Name_Ar, 150) where LEN(Name_Ar) > 150;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_OPERATING_SECTOR_TYPE_G2G_LKP'), 'Name_En', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE CMS_OPERATING_SECTOR_TYPE_G2G_LKP ALTER COLUMN Name_En NVARCHAR(150);
	Print('CMS_OPERATING_SECTOR_TYPE_G2G_LKP.Name_En Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_OPERATING_SECTOR_TYPE_G2G_LKP'), 'Name_Ar', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE CMS_OPERATING_SECTOR_TYPE_G2G_LKP ALTER COLUMN Name_Ar NVARCHAR(150);
	Print('CMS_OPERATING_SECTOR_TYPE_G2G_LKP.Name_Ar Altered')
END
---------------------------------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LMS_LITERATURE_TAG]') AND type in (N'U'))
BEGIN
update LMS_LITERATURE_TAG set TagNo = LEFT(TagNo, 150) where LEN(TagNo) > 150;
update LMS_LITERATURE_TAG set [Description] = LEFT([Description], 1024) where LEN([Description]) > 1024;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.LMS_LITERATURE_TAG'), 'TagNo', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE LMS_LITERATURE_TAG ALTER COLUMN TagNo NVARCHAR(150);
	Print('LMS_LITERATURE_TAG.TagNo Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.LMS_LITERATURE_TAG'), 'Description', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE LMS_LITERATURE_TAG ALTER COLUMN [Description] NVARCHAR(1024);
	Print('LMS_LITERATURE_TAG.Description Altered')
END
---------------------------------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LMS_LITERATURE_AUTHOR]') AND type in (N'U'))
BEGIN
update LMS_LITERATURE_AUTHOR set FullName_En = LEFT(FullName_En, 150) where LEN(FullName_En) > 150;
update LMS_LITERATURE_AUTHOR set FullName_Ar = LEFT(FullName_Ar, 150) where LEN(FullName_Ar) > 150;
update LMS_LITERATURE_AUTHOR set Address_En = LEFT(Address_En, 150) where LEN(Address_En) > 150;
update LMS_LITERATURE_AUTHOR set Address_Ar	= LEFT(Address_Ar, 150) where LEN(Address_Ar) > 150;
update LMS_LITERATURE_AUTHOR set FirstName_En = LEFT(FirstName_En, 50) where LEN(FirstName_En) > 50;
update LMS_LITERATURE_AUTHOR set FirstName_Ar = LEFT(FirstName_Ar, 50) where LEN(FirstName_Ar) > 50;
update LMS_LITERATURE_AUTHOR set SecondName_En  = LEFT(SecondName_En, 50) where LEN(SecondName_En) > 50;
update LMS_LITERATURE_AUTHOR set SecondName_Ar  = LEFT(SecondName_Ar, 50) where LEN(SecondName_Ar) > 50;
update LMS_LITERATURE_AUTHOR set ThirdName_En	= LEFT(ThirdName_En, 50) where LEN(ThirdName_En) > 50;
update LMS_LITERATURE_AUTHOR set ThirdName_Ar   = LEFT(ThirdName_Ar, 50) where LEN(ThirdName_Ar) > 50;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.LMS_LITERATURE_AUTHOR'), 'FullName_En', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE LMS_LITERATURE_AUTHOR ALTER COLUMN FullName_En NVARCHAR(150);
	Print('LMS_LITERATURE_AUTHOR.FullName_En Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.LMS_LITERATURE_AUTHOR'), 'FullName_Ar', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE LMS_LITERATURE_AUTHOR ALTER COLUMN FullName_Ar NVARCHAR(150);
	Print('LMS_LITERATURE_AUTHOR.FullName_Ar Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.LMS_LITERATURE_AUTHOR'), 'Address_En', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE LMS_LITERATURE_AUTHOR ALTER COLUMN Address_En NVARCHAR(150);
	Print('LMS_LITERATURE_AUTHOR.Address_En Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.LMS_LITERATURE_AUTHOR'), 'Address_Ar', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE LMS_LITERATURE_AUTHOR ALTER COLUMN Address_Ar NVARCHAR(150);
	Print('LMS_LITERATURE_AUTHOR.Address_Ar Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.LMS_LITERATURE_AUTHOR'), 'FirstName_En', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE LMS_LITERATURE_AUTHOR ALTER COLUMN FirstName_En NVARCHAR(150);
	Print('LMS_LITERATURE_AUTHOR.FirstName_En Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.LMS_LITERATURE_AUTHOR'), 'FirstName_Ar', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE LMS_LITERATURE_AUTHOR ALTER COLUMN FirstName_Ar NVARCHAR(150);
	Print('LMS_LITERATURE_AUTHOR.FirstName_Ar Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.LMS_LITERATURE_AUTHOR'), 'SecondName_En', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE LMS_LITERATURE_AUTHOR ALTER COLUMN SecondName_En NVARCHAR(150);
	Print('LMS_LITERATURE_AUTHOR.SecondName_En Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.LMS_LITERATURE_AUTHOR'), 'SecondName_Ar', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE LMS_LITERATURE_AUTHOR ALTER COLUMN SecondName_Ar NVARCHAR(150);
	Print('LMS_LITERATURE_AUTHOR.SecondName_Ar Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.LMS_LITERATURE_AUTHOR'), 'ThirdName_En', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE LMS_LITERATURE_AUTHOR ALTER COLUMN ThirdName_En NVARCHAR(150);
	Print('LMS_LITERATURE_AUTHOR.ThirdName_En Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.LMS_LITERATURE_AUTHOR'), 'ThirdName_Ar', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE LMS_LITERATURE_AUTHOR ALTER COLUMN ThirdName_Ar NVARCHAR(150);
	Print('LMS_LITERATURE_AUTHOR.ThirdName_Ar Altered')
END
---------------------------------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_PUBLICATION_SOURCE_NAME]') AND type in (N'U'))
BEGIN
update LEGAL_PUBLICATION_SOURCE_NAME set Name_En = LEFT(Name_En, 150) where LEN(Name_En) > 150;
update LEGAL_PUBLICATION_SOURCE_NAME set Name_Ar = LEFT(Name_Ar, 150) where LEN(Name_Ar) > 150;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.EP_DEPARTMENT'), 'Name_En', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE LEGAL_PUBLICATION_SOURCE_NAME ALTER COLUMN Name_En NVARCHAR(150);
	Print('LEGAL_PUBLICATION_SOURCE_NAME.Name_En Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.LEGAL_PUBLICATION_SOURCE_NAME'), 'Name_Ar', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE LEGAL_PUBLICATION_SOURCE_NAME ALTER COLUMN Name_Ar NVARCHAR(150);
	Print('LEGAL_PUBLICATION_SOURCE_NAME.Name_Ar Altered')
END
---------------------------------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_LEGISLATION_TYPE]') AND type in (N'U'))
BEGIN
update LEGAL_LEGISLATION_TYPE set Name_En = LEFT(Name_En, 150) where LEN(Name_En) > 150;
update LEGAL_LEGISLATION_TYPE set Name_Ar = LEFT(Name_Ar, 150) where LEN(Name_Ar) > 150;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.LEGAL_LEGISLATION_TYPE'), 'Name_En', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE LEGAL_LEGISLATION_TYPE ALTER COLUMN Name_En NVARCHAR(150);
	Print('LEGAL_LEGISLATION_TYPE.Name_En Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.LEGAL_LEGISLATION_TYPE'), 'Name_Ar', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE LEGAL_LEGISLATION_TYPE ALTER COLUMN Name_Ar NVARCHAR(150);
	Print('LEGAL_LEGISLATION_TYPE.Name_Ar Altered')
END
---------------------------------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_PRINCIPLE_TYPE]') AND type in (N'U'))
BEGIN
update LEGAL_PRINCIPLE_TYPE set Name_En = LEFT(Name_En, 150) where LEN(Name_En) > 150;
update LEGAL_PRINCIPLE_TYPE set Name_Ar = LEFT(Name_Ar, 150) where LEN(Name_Ar) > 150;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.LEGAL_PRINCIPLE_TYPE'), 'Name_En', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE LEGAL_PRINCIPLE_TYPE ALTER COLUMN Name_En NVARCHAR(150);
	Print('LEGAL_PRINCIPLE_TYPE.Name_En Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.LEGAL_PRINCIPLE_TYPE'), 'Name_Ar', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE LEGAL_PRINCIPLE_TYPE ALTER COLUMN Name_Ar NVARCHAR(150);
	Print('LEGAL_PRINCIPLE_TYPE.Name_Ar Altered')
END
---------------------------------------------------------
--Step 1: Below script to check the constraint 
SELECT name 
FROM sys.default_constraints 
WHERE parent_object_id = OBJECT_ID('CMS_COURT_G2G_LKP') 
  AND parent_column_id = COLUMNPROPERTY(OBJECT_ID('CMS_COURT_G2G_LKP'), 'Address', 'ColumnId');

--Step 2: Drop the constraint
ALTER TABLE CMS_COURT_G2G_LKP DROP CONSTRAINT DF__CMS_COURT__Addre__247D636F;

--Step 3: Run the below scirpt
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_COURT_G2G_LKP]') AND type in (N'U'))
BEGIN
update CMS_COURT_G2G_LKP set Name_En = LEFT(Name_En, 150) where LEN(Name_En) > 150;
update CMS_COURT_G2G_LKP set Name_Ar = LEFT(Name_Ar, 150) where LEN(Name_Ar) > 150;
update CMS_COURT_G2G_LKP set [Address] = LEFT([Address], 150) where LEN([Address]) > 150;
update CMS_COURT_G2G_LKP set District = LEFT(District, 150) where LEN(District) > 150;
update CMS_COURT_G2G_LKP set [Location] = LEFT([Location], 150) where LEN([Location]) > 150;
update CMS_COURT_G2G_LKP set [Description] = LEFT([Description], 1024) where LEN([Description]) > 1024;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_COURT_G2G_LKP'), 'Name_En', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE CMS_COURT_G2G_LKP ALTER COLUMN Name_En NVARCHAR(150);
	Print('CMS_COURT_G2G_LKP.Name_En Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_COURT_G2G_LKP'), 'Name_Ar', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE CMS_COURT_G2G_LKP ALTER COLUMN Name_Ar NVARCHAR(150);
	Print('CMS_COURT_G2G_LKP.Name_Ar Altered')
END
--the below creating issue
--Msg 5074, Level 16, State 1, Line 336
--The object 'DF__CMS_COURT__Addre__247D636F' is dependent on column 'Address'.
--Msg 4922, Level 16, State 9, Line 336
--ALTER TABLE ALTER COLUMN Address failed because one or more objects access this column.
IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_COURT_G2G_LKP'), 'Address', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE CMS_COURT_G2G_LKP ALTER COLUMN [Address] NVARCHAR(150);
	Print('CMS_COURT_G2G_LKP.Address Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_COURT_G2G_LKP'), 'District', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE CMS_COURT_G2G_LKP ALTER COLUMN District NVARCHAR(150);
	Print('CMS_COURT_G2G_LKP.District Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_COURT_G2G_LKP'), 'Location', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE CMS_COURT_G2G_LKP ALTER COLUMN [Location] NVARCHAR(150);
	Print('CMS_COURT_G2G_LKP.Location Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_COURT_G2G_LKP'), 'Description', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE CMS_COURT_G2G_LKP ALTER COLUMN [Description] NVARCHAR(1024);
	Print('CMS_COURT_G2G_LKP.Description Altered')
END

--step 4: add the constraint again:
ALTER TABLE CMS_COURT_G2G_LKP 
ADD CONSTRAINT DF__CMS_COURT__Addre__247D636F DEFAULT 'DefaultValue' FOR Address;
---------------------------------------------------------
--Step 1: Below script to check the constraint 
SELECT name 
FROM sys.default_constraints 
WHERE parent_object_id = OBJECT_ID('CMS_CHAMBER_G2G_LKP') 
  AND parent_column_id = COLUMNPROPERTY(OBJECT_ID('CMS_CHAMBER_G2G_LKP'), 'ChamberCode', 'ColumnId');

--Step 2: Drop the constraint
ALTER TABLE CMS_CHAMBER_G2G_LKP DROP CONSTRAINT DF__CMS_CHAMB__Chamb__20ACD28B;

--Step 3: Run the below scirpt
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_CHAMBER_G2G_LKP]') AND type in (N'U'))
BEGIN
update CMS_CHAMBER_G2G_LKP set Name_En = LEFT(Name_En, 150) where LEN(Name_En) > 150;
update CMS_CHAMBER_G2G_LKP set Name_Ar = LEFT(Name_Ar, 150) where LEN(Name_Ar) > 150;
update CMS_CHAMBER_G2G_LKP set ChamberCode = LEFT(ChamberCode, 50) where LEN(ChamberCode) > 50;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_CHAMBER_G2G_LKP'), 'Name_En', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE CMS_CHAMBER_G2G_LKP ALTER COLUMN Name_En NVARCHAR(150);
	Print('CMS_CHAMBER_G2G_LKP.Name_En Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_CHAMBER_G2G_LKP'), 'Name_Ar', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE CMS_CHAMBER_G2G_LKP ALTER COLUMN Name_Ar NVARCHAR(150);
	Print('CMS_CHAMBER_G2G_LKP.Name_Ar Altered')
END
--the below creating issue
--Msg 5074, Level 16, State 1, Line 377
--The object 'DF__CMS_CHAMB__Chamb__20ACD28B' is dependent on column 'ChamberCode'.
--Msg 4922, Level 16, State 9, Line 377
--ALTER TABLE ALTER COLUMN ChamberCode failed because one or more objects access this column.
IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_CHAMBER_G2G_LKP'), 'ChamberCode', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE CMS_CHAMBER_G2G_LKP ALTER COLUMN ChamberCode NVARCHAR(150);
	Print('CMS_CHAMBER_G2G_LKP.ChamberCode Altered')
END

--Step 4: Run the below scirpt
ALTER TABLE CMS_CHAMBER_G2G_LKP 
ADD CONSTRAINT DF__CMS_CHAMB__Chamb__20ACD28B DEFAULT 'DefaultValue' FOR ChamberCode;
---------------------------------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_CHAMBER_NUMBER_G2G_LKP]') AND type in (N'U'))
BEGIN
update CMS_CHAMBER_NUMBER_G2G_LKP set Number = LEFT(Number, 50) where LEN(Number) > 50;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_CHAMBER_NUMBER_G2G_LKP'), 'Number', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE CMS_CHAMBER_NUMBER_G2G_LKP ALTER COLUMN Number NVARCHAR(50);
	Print('CMS_CHAMBER_NUMBER_G2G_LKP.Number Altered')
END
---------------------------------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_REQUEST_TYPE_G2G_LKP]') AND type in (N'U'))
BEGIN
update CMS_REQUEST_TYPE_G2G_LKP set Name_En = LEFT(Name_En, 150) where LEN(Name_En) > 150;
update CMS_REQUEST_TYPE_G2G_LKP set Name_Ar = LEFT(Name_Ar, 150) where LEN(Name_Ar) > 150;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_REQUEST_TYPE_G2G_LKP'), 'Name_En', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE CMS_REQUEST_TYPE_G2G_LKP ALTER COLUMN Name_En NVARCHAR(150);
	Print('CMS_REQUEST_TYPE_G2G_LKP.Name_En Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_REQUEST_TYPE_G2G_LKP'), 'Name_Ar', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE CMS_REQUEST_TYPE_G2G_LKP ALTER COLUMN Name_Ar NVARCHAR(150);
	Print('CMS_REQUEST_TYPE_G2G_LKP.Name_Ar Altered')
END
---------------------------------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_SUBTYPE_G2G_LKP]') AND type in (N'U'))
BEGIN
update CMS_SUBTYPE_G2G_LKP set Name_En = LEFT(Name_En, 150) where LEN(Name_En) > 150;
update CMS_SUBTYPE_G2G_LKP set Name_Ar = LEFT(Name_Ar, 150) where LEN(Name_Ar) > 150;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_SUBTYPE_G2G_LKP'), 'Name_En', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE CMS_SUBTYPE_G2G_LKP ALTER COLUMN Name_En NVARCHAR(150);
	Print('CMS_SUBTYPE_G2G_LKP.Name_En Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_SUBTYPE_G2G_LKP'), 'Name_Ar', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE CMS_SUBTYPE_G2G_LKP ALTER COLUMN Name_Ar NVARCHAR(150);
	Print('CMS_SUBTYPE_G2G_LKP.Name_Ar Altered')
END
---------------------------------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[COMM_COMMUNICATION_TYPE]') AND type in (N'U'))
BEGIN
update COMM_COMMUNICATION_TYPE set NameEn = LEFT(NameEn, 150) where LEN(NameEn) > 150;
update COMM_COMMUNICATION_TYPE set NameAr = LEFT(NameAr, 150) where LEN(NameAr) > 150;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.COMM_COMMUNICATION_TYPE'), 'NameEn', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE COMM_COMMUNICATION_TYPE ALTER COLUMN NameEn NVARCHAR(150);
	Print('COMM_COMMUNICATION_TYPE.NameEn Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.COMM_COMMUNICATION_TYPE'), 'NameAr', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE COMM_COMMUNICATION_TYPE ALTER COLUMN NameAr NVARCHAR(150);
	Print('COMM_COMMUNICATION_TYPE.NameAr Altered')
END
---------------------------------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TSK_TASK_TYPE]') AND type in (N'U'))
BEGIN
update TSK_TASK_TYPE set NameEn = LEFT(NameEn, 150) where LEN(NameEn) > 150;
update TSK_TASK_TYPE set NameAr = LEFT(NameAr, 150) where LEN(NameAr) > 150;
update TSK_TASK_TYPE set [Description] = LEFT([Description], 1024) where  LEN([Description]) > 1024;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.TSK_TASK_TYPE'), 'NameEn', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE TSK_TASK_TYPE ALTER COLUMN NameEn NVARCHAR(150);
	Print('TSK_TASK_TYPE.NameEn Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.TSK_TASK_TYPE'), 'NameAr', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE TSK_TASK_TYPE ALTER COLUMN NameAr NVARCHAR(150);
	Print('TSK_TASK_TYPE.NameAr Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.TSK_TASK_TYPE'), 'Description', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE TSK_TASK_TYPE ALTER COLUMN [Description] NVARCHAR(150);
	Print('TSK_TASK_TYPE.[Description] Altered')
END
---------------------------------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_CASE_FILE_STATUS_G2G_LKP]') AND type in (N'U'))
BEGIN
update CMS_CASE_FILE_STATUS_G2G_LKP set Name_En = LEFT(Name_En, 150) where LEN(Name_En) > 150;
update CMS_CASE_FILE_STATUS_G2G_LKP set Name_Ar = LEFT(Name_Ar, 150) where LEN(Name_Ar) > 150;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_CASE_FILE_STATUS_G2G_LKP'), 'Name_En', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE CMS_CASE_FILE_STATUS_G2G_LKP ALTER COLUMN Name_En NVARCHAR(150);
	Print('CMS_CASE_FILE_STATUS_G2G_LKP.Name_En Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_CASE_FILE_STATUS_G2G_LKP'), 'Name_Ar', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE CMS_CASE_FILE_STATUS_G2G_LKP ALTER COLUMN Name_Ar NVARCHAR(150);
	Print('CMS_CASE_FILE_STATUS_G2G_LKP.Name_Ar Altered')
END
---------------------------------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_CASE_REQUEST_STATUS_G2G_LKP]') AND type in (N'U'))
BEGIN
update CMS_CASE_REQUEST_STATUS_G2G_LKP set Name_En = LEFT(Name_En, 150) where LEN(Name_En) > 150;
update CMS_CASE_REQUEST_STATUS_G2G_LKP set Name_Ar = LEFT(Name_Ar, 150) where LEN(Name_Ar) > 150;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_CASE_REQUEST_STATUS_G2G_LKP'), 'Name_En', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE CMS_CASE_REQUEST_STATUS_G2G_LKP ALTER COLUMN Name_En NVARCHAR(150);
	Print('CMS_CASE_REQUEST_STATUS_G2G_LKP.Name_En Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_CASE_REQUEST_STATUS_G2G_LKP'), 'Name_Ar', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE CMS_CASE_REQUEST_STATUS_G2G_LKP ALTER COLUMN Name_Ar NVARCHAR(150);
	Print('CMS_CASE_REQUEST_STATUS_G2G_LKP.Name_Ar Altered')
END
---------------------------------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_REGISTERED_CASE_STATUS_G2G_LKP]') AND type in (N'U'))
BEGIN
update CMS_REGISTERED_CASE_STATUS_G2G_LKP set Name_En = LEFT(Name_En, 150) where LEN(Name_En) > 150;
update CMS_REGISTERED_CASE_STATUS_G2G_LKP set Name_Ar = LEFT(Name_Ar, 150) where LEN(Name_Ar) > 150;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_REGISTERED_CASE_STATUS_G2G_LKP'), 'Name_En', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE CMS_REGISTERED_CASE_STATUS_G2G_LKP ALTER COLUMN Name_En NVARCHAR(150);
	Print('CMS_REGISTERED_CASE_STATUS_G2G_LKP.Name_En Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_REGISTERED_CASE_STATUS_G2G_LKP'), 'Name_Ar', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE CMS_REGISTERED_CASE_STATUS_G2G_LKP ALTER COLUMN Name_Ar NVARCHAR(150);
	Print('CMS_REGISTERED_CASE_STATUS_G2G_LKP.Name_Ar Altered')
END
---------------------------------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_COMS_NUM_PATTERN]') AND type in (N'U'))
BEGIN
update CMS_COMS_NUM_PATTERN set StaticTextPattern = LEFT(StaticTextPattern, 512) where LEN(StaticTextPattern) > 512;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_COMS_NUM_PATTERN'), 'StaticTextPattern', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE CMS_COMS_NUM_PATTERN ALTER COLUMN StaticTextPattern NVARCHAR(512);
	Print('CMS_COMS_NUM_PATTERN.StaticTextPattern Altered')
END
---------------------------------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_REQUEST_TYPE_G2G_LKP]') AND type in (N'U'))
BEGIN
update CMS_REQUEST_TYPE_G2G_LKP set Name_En = LEFT(Name_En, 150) where LEN(Name_En) > 150;
update CMS_REQUEST_TYPE_G2G_LKP set Name_Ar = LEFT(Name_Ar, 150) where LEN(Name_Ar) > 150;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_REQUEST_TYPE_G2G_LKP'), 'Name_En', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE CMS_REQUEST_TYPE_G2G_LKP ALTER COLUMN Name_En NVARCHAR(150);
	Print('CMS_REQUEST_TYPE_G2G_LKP.Name_En Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_REQUEST_TYPE_G2G_LKP'), 'Name_Ar', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE CMS_REQUEST_TYPE_G2G_LKP ALTER COLUMN Name_Ar NVARCHAR(150);
	Print('CMS_REQUEST_TYPE_G2G_LKP.Name_Ar Altered')
END
---------------------------------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ATTACHMENT_TYPE]') AND type in (N'U'))
BEGIN
update ATTACHMENT_TYPE set Type_En = LEFT(Type_En, 150) where LEN(Type_En) > 150;
update ATTACHMENT_TYPE set Type_Ar = LEFT(Type_Ar, 150) where LEN(Type_Ar) > 150;
update ATTACHMENT_TYPE set [Description] = LEFT([Description], 1024) where LEN([Description]) > 1024;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.ATTACHMENT_TYPE'), 'Type_En', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE ATTACHMENT_TYPE ALTER COLUMN Type_En NVARCHAR(150);
	Print('ATTACHMENT_TYPE.Type_En Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.ATTACHMENT_TYPE'), 'Type_Ar', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE ATTACHMENT_TYPE ALTER COLUMN Type_Ar NVARCHAR(150);
	Print('ATTACHMENT_TYPE.Type_Ar Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.ATTACHMENT_TYPE'), 'Description', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE ATTACHMENT_TYPE ALTER COLUMN [Description] NVARCHAR(1024);
	Print('ATTACHMENT_TYPE.Description Altered')
END
---------------------------------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_GOVERNMENT_ENTITY_DEPARTMENT_G2G_LKP]') AND type in (N'U'))
BEGIN
update CMS_GOVERNMENT_ENTITY_DEPARTMENT_G2G_LKP set DeptProfession = LEFT(DeptProfession, 150) where LEN(DeptProfession) > 150;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_GOVERNMENT_ENTITY_DEPARTMENT_G2G_LKP'), 'DeptProfession', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE CMS_GOVERNMENT_ENTITY_DEPARTMENT_G2G_LKP ALTER COLUMN DeptProfession NVARCHAR(150);
	Print('CMS_GOVERNMENT_ENTITY_DEPARTMENT_G2G_LKP.StaticTextPattern Altered')
END
---------------------------------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_GOVERNMENT_ENTITY_REPRESENTATIVES]') AND type in (N'U'))
BEGIN
update CMS_GOVERNMENT_ENTITY_REPRESENTATIVES set NameEn = LEFT(NameEn, 150) where LEN(NameEn) > 150;
update CMS_GOVERNMENT_ENTITY_REPRESENTATIVES set NameAr = LEFT(NameAr, 150) where LEN(NameAr) > 150;
update CMS_GOVERNMENT_ENTITY_REPRESENTATIVES set Representative_Designation_EN = LEFT(Representative_Designation_EN, 150) where LEN(Representative_Designation_EN) > 150;
update CMS_GOVERNMENT_ENTITY_REPRESENTATIVES set Representative_Designation_AR = LEFT(Representative_Designation_AR, 150) where LEN(Representative_Designation_AR) > 150;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_GOVERNMENT_ENTITY_REPRESENTATIVES'), 'NameEn', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE CMS_GOVERNMENT_ENTITY_REPRESENTATIVES ALTER COLUMN NameEn NVARCHAR(150);
	Print('CMS_GOVERNMENT_ENTITY_REPRESENTATIVES.NameEn Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_GOVERNMENT_ENTITY_REPRESENTATIVES'), 'NameAr', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE CMS_GOVERNMENT_ENTITY_REPRESENTATIVES ALTER COLUMN NameAr NVARCHAR(150);
	Print('CMS_GOVERNMENT_ENTITY_REPRESENTATIVES.NameAr Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_GOVERNMENT_ENTITY_REPRESENTATIVES'), 'Representative_Designation_EN', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE CMS_GOVERNMENT_ENTITY_REPRESENTATIVES ALTER COLUMN Representative_Designation_EN NVARCHAR(150);
	Print('CMS_GOVERNMENT_ENTITY_REPRESENTATIVES.Representative_Designation_EN Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_GOVERNMENT_ENTITY_REPRESENTATIVES'), 'Representative_Designation_AR', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE CMS_GOVERNMENT_ENTITY_REPRESENTATIVES ALTER COLUMN Representative_Designation_AR NVARCHAR(150);
	Print('CMS_GOVERNMENT_ENTITY_REPRESENTATIVES.Representative_Designation_AR Altered')
END
---------------------------------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tTranslation]') AND type in (N'U'))
BEGIN
update tTranslation set Value_En = LEFT(Value_En, 512) where LEN(Value_En) > 512;
update tTranslation set Value_Ar = LEFT(Value_Ar, 512) where LEN(Value_Ar) > 512;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.tTranslation'), 'Value_En', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE tTranslation ALTER COLUMN Value_En NVARCHAR(512);
	Print('tTranslation.Value_En Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.tTranslation'), 'Value_Ar', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE tTranslation ALTER COLUMN Value_Ar NVARCHAR(512);
	Print('tTranslation.Value_Ar Altered')
END
---------------------------------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UMS_GROUP]') AND type in (N'U'))
BEGIN
update UMS_GROUP set Name_En = LEFT(Name_En, 150) where LEN(Name_En) > 150;
update UMS_GROUP set Name_Ar = LEFT(Name_Ar, 150) where LEN(Name_Ar) > 150;
update UMS_GROUP set Description_En = LEFT(Description_En, 512) where LEN(Description_En) > 512;
update UMS_GROUP set Description_Ar = LEFT(Description_Ar, 512) where LEN(Description_Ar) > 512;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.UMS_GROUP'), 'Name_En', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE UMS_GROUP ALTER COLUMN Name_En NVARCHAR(150);
	Print('UMS_GROUP.Name_En Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.UMS_GROUP'), 'Name_Ar', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE UMS_GROUP ALTER COLUMN Name_Ar NVARCHAR(150);
	Print('UMS_GROUP.Name_Ar Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.UMS_GROUP'), 'Description_En', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE UMS_GROUP ALTER COLUMN Description_En NVARCHAR(512);
	Print('UMS_GROUP.Description_En Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.UMS_GROUP'), 'Description_Ar', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE UMS_GROUP ALTER COLUMN Description_Ar NVARCHAR(512);
	Print('UMS_GROUP.Description_Ar Altered')
END
---------------------------------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UMS_ROLE]') AND type in (N'U'))
BEGIN
update UMS_ROLE set [Name] = LEFT([Name], 150) where LEN([Name]) > 150;
update UMS_ROLE set NameAr = LEFT(NameAr, 150) where LEN(NameAr) > 150;
update UMS_ROLE set Description_En = LEFT(Description_En, 512) where LEN(Description_En) > 512;
update UMS_ROLE set Description_Ar = LEFT(Description_Ar, 512) where LEN(Description_Ar) > 512;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.UMS_GROUP'), 'Name', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE UMS_GROUP ALTER COLUMN [Name] NVARCHAR(150);
	Print('UMS_GROUP.Name Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.UMS_GROUP'), 'NameAr', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE UMS_GROUP ALTER COLUMN NameAr NVARCHAR(150);
	Print('UMS_GROUP.NameAr Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.UMS_GROUP'), 'Description_En', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE UMS_GROUP ALTER COLUMN Description_En NVARCHAR(512);
	Print('UMS_GROUP.Description_En Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.UMS_GROUP'), 'Description_Ar', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE UMS_GROUP ALTER COLUMN Description_Ar NVARCHAR(512);
	Print('UMS_GROUP.Description_Ar Altered')
END
---------------------------------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UMS_CLAIM]') AND type in (N'U'))
BEGIN
update UMS_CLAIM set Title_En = LEFT(Title_En, 512) where LEN(Title_En) > 512;
update UMS_CLAIM set Title_Ar = LEFT(Title_Ar, 512) where LEN(Title_Ar) > 512;
update UMS_CLAIM set Module = LEFT(Module, 512) where LEN(Module) > 512;
update UMS_CLAIM set SubModule = LEFT(Module, 512) where LEN(Module) > 512;
update UMS_CLAIM set ClaimType = LEFT(ClaimType, 512) where LEN(ClaimType) > 512;
update UMS_CLAIM set ClaimValue = LEFT(ClaimValue, 512) where LEN(ClaimValue) > 512;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.UMS_CLAIM'), 'Title_En', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE UMS_CLAIM ALTER COLUMN Title_En NVARCHAR(512);
	Print('UMS_CLAIM.Title_En Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.UMS_CLAIM'), 'Title_Ar', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE UMS_CLAIM ALTER COLUMN Title_Ar NVARCHAR(512);
	Print('UMS_CLAIM.Title_Ar Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.UMS_CLAIM'), 'Module', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE UMS_CLAIM ALTER COLUMN Module NVARCHAR(512);
	Print('UMS_CLAIM.Module Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.UMS_CLAIM'), 'SubModule', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE UMS_CLAIM ALTER COLUMN SubModule NVARCHAR(512);
	Print('UMS_CLAIM.SubModule Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.UMS_CLAIM'), 'ClaimType', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE UMS_CLAIM ALTER COLUMN ClaimType NVARCHAR(512);
	Print('UMS_CLAIM.ClaimType Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.UMS_CLAIM'), 'ClaimValue', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE UMS_CLAIM ALTER COLUMN ClaimValue NVARCHAR(512);
	Print('UMS_CLAIM.ClaimValue Altered')
END
---------------------------------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[NOTIF_NOTIFICATION_TEMPLATE]') AND type in (N'U'))
BEGIN
update NOTIF_NOTIFICATION_TEMPLATE set NameEn = LEFT(NameEn, 150) where LEN(NameEn) > 150;
update NOTIF_NOTIFICATION_TEMPLATE set NameAr = LEFT(NameAr, 150) where LEN(NameAr) > 150;
update NOTIF_NOTIFICATION_TEMPLATE set BodyEn = LEFT(BodyEn, 512) where LEN(BodyEn) > 512;
update NOTIF_NOTIFICATION_TEMPLATE set BodyAr = LEFT(BodyAr, 512) where LEN(BodyAr) > 512;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.UMS_GROUP'), 'NameEn', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE UMS_GROUP ALTER COLUMN NameEn NVARCHAR(150);
	Print('UMS_GROUP.NameEn Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.UMS_GROUP'), 'NameAr', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE UMS_GROUP ALTER COLUMN NameAr NVARCHAR(150);
	Print('UMS_GROUP.NameAr Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.UMS_GROUP'), 'BodyEn', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE UMS_GROUP ALTER COLUMN BodyEn NVARCHAR(512);
	Print('UMS_GROUP.BodyEn Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.UMS_GROUP'), 'BodyAr', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE UMS_GROUP ALTER COLUMN BodyAr NVARCHAR(512);
	Print('UMS_GROUP.BodyAr Altered')
END
------------------------------End-----------------Muhammad Ali-------------------

------ Remove Old Legal Document System table and constraints from Fatwa Portal Start 19-02-2025 ----------

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[CMS_DRAFTED_TEMPLATE]') AND type in (N'U'))
ALTER TABLE [dbo].[LDS_DOCUMENT_LDS_DOCUMENT_REFERENCE] DROP CONSTRAINT IF EXISTS [LDS_DOCUMENT_LDS_DOCUMENT_REFERENCE_RDID]

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[CMS_DRAFTED_TEMPLATE]') AND type in (N'U'))
ALTER TABLE [dbo].[LDS_DOCUMENT_MASKED] DROP CONSTRAINT IF EXISTS [LDS_DOCUMENT_MASKED_DOCUMENT]

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[CMS_DRAFTED_TEMPLATE]') AND type in (N'U'))
ALTER TABLE [dbo].[LDS_DOCUMENT_VERSION_HISTORY] DROP CONSTRAINT IF EXISTS [LDS_DOCUMENT_VERSION_HISTORY_LDS_DOCUMENT]

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LDS_DOCUMENT]') AND type in (N'U'))
DROP TABLE [dbo].[LDS_DOCUMENT]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LDS_DOCUMENT_CATALOG]') AND type in (N'U'))
DROP TABLE [dbo].[LDS_DOCUMENT_CATALOG]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LDS_DOCUMENT_REFERENCE]') AND type in (N'U'))
DROP TABLE [dbo].[LDS_DOCUMENT_REFERENCE]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LDS_DOCUMENT_LDS_DOCUMENT_REFERENCE]') AND type in (N'U'))
DROP TABLE [dbo].[LDS_DOCUMENT_LDS_DOCUMENT_REFERENCE]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LDS_DOCUMENT_MASKED]') AND type in (N'U'))
DROP TABLE [dbo].[LDS_DOCUMENT_MASKED]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LDS_DOCUMENT_SUBJECT]') AND type in (N'U'))
DROP TABLE [dbo].[LDS_DOCUMENT_SUBJECT]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LDS_DOCUMENT_TYPE]') AND type in (N'U'))
DROP TABLE [dbo].[LDS_DOCUMENT_TYPE]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LDS_DOCUMENT_VERSION_HISTORY]') AND type in (N'U'))
DROP TABLE [dbo].[LDS_DOCUMENT_VERSION_HISTORY]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LDS_DOCUMENT_REQUEST]') AND type in (N'U'))
DROP TABLE [dbo].[LDS_DOCUMENT_REQUEST]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LDS_DOCUMENT_REQUEST_HIST]') AND type in (N'U'))
DROP TABLE [dbo].[LDS_DOCUMENT_REQUEST_HIST]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LDS_DOCUMENT_REQUEST_STATUS]') AND type in (N'U'))
DROP TABLE [dbo].[LDS_DOCUMENT_REQUEST_STATUS]
GO

------ Remove Old Legal Document System table and constraints from Fatwa Portal End ----------

------ Remove first and second version of Legal Principle Syatem tables and constraints from Fatwa Portal Start 20-02-2025 -------

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[LPS_PRINCIPLE]') AND type in (N'U'))
ALTER TABLE [dbo].[LPS_PRINCIPLE] DROP CONSTRAINT IF EXISTS [LPS_PRINCIPLE_LPS_PRINCIPLE_CATEGORY]

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LPS_PRINCIPLE_CATEGORY]') AND type in (N'U'))
DROP TABLE [dbo].[LPS_PRINCIPLE_CATEGORY]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[LPS_PRINCIPLE_LPS_PRINCIPLE_REFERENCE]') AND type in (N'U'))
ALTER TABLE [dbo].[LPS_PRINCIPLE_LPS_PRINCIPLE_REFERENCE] DROP CONSTRAINT IF EXISTS [LPS_PRINCIPLE_LPS_PRINCIPLE_REFERENCE_PRID]

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[LPS_PRINCIPLE_LPS_PRINCIPLE_TAG]') AND type in (N'U'))
ALTER TABLE [dbo].[LPS_PRINCIPLE_LPS_PRINCIPLE_TAG] DROP CONSTRAINT IF EXISTS [LPS_PRINCIPLE_LPS_PRINCIPLE_TAG_PTID]

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[LPS_PRINCIPLE_MASKED]') AND type in (N'U'))
ALTER TABLE [dbo].[LPS_PRINCIPLE_MASKED] DROP CONSTRAINT IF EXISTS [LPS_PRINCIPLE_MASKED_PRINCIPLE]

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[LPS_PRINCIPLE_VERSION_HISTORY]') AND type in (N'U'))
ALTER TABLE [dbo].[LPS_PRINCIPLE_VERSION_HISTORY] DROP CONSTRAINT IF EXISTS [LPS_PRINCIPLE_VERSION_HISTORY_LPS_PRINCIPLE]

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LPS_PRINCIPLE]') AND type in (N'U'))
DROP TABLE [dbo].[LPS_PRINCIPLE]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LPS_PRINCIPLE_LPS_PRINCIPLE_REFERENCE]') AND type in (N'U'))
DROP TABLE [dbo].[LPS_PRINCIPLE_LPS_PRINCIPLE_REFERENCE]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LPS_PRINCIPLE_LPS_PRINCIPLE_TAG]') AND type in (N'U'))
DROP TABLE [dbo].[LPS_PRINCIPLE_LPS_PRINCIPLE_TAG]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LPS_PRINCIPLE_MASKED]') AND type in (N'U'))
DROP TABLE [dbo].[LPS_PRINCIPLE_MASKED]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LPS_PRINCIPLE_REFERENCE]') AND type in (N'U'))
DROP TABLE [dbo].[LPS_PRINCIPLE_REFERENCE]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LPS_PRINCIPLE_VERSION_HISTORY]') AND type in (N'U'))
DROP TABLE [dbo].[LPS_PRINCIPLE_VERSION_HISTORY]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LPS_PRINCIPLEHIERARCHY]') AND type in (N'U'))
DROP TABLE [dbo].[LPS_PRINCIPLEHIERARCHY]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LPS_PRINCIPLE_TAG]') AND type in (N'U'))
DROP TABLE [dbo].[LPS_PRINCIPLE_TAG]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LDS_DOCUMENT_APPROVAL_STATUS]') AND type in (N'U'))
DROP TABLE [dbo].[LDS_DOCUMENT_APPROVAL_STATUS]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LPS_PRINCIPLE_STATUS]') AND type in (N'U'))
DROP TABLE [dbo].[LPS_PRINCIPLE_STATUS]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LPS_DOCUMENT_VERSION_HISTORY]') AND type in (N'U'))
DROP TABLE [dbo].[LPS_DOCUMENT_VERSION_HISTORY]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[UFC_UNDER_FILING_CASE]') AND type in (N'U'))
ALTER TABLE [dbo].[UFC_UNDER_FILING_CASE] DROP CONSTRAINT IF EXISTS [UFC_UNDER_FILING_CASE_LPS_PRINCIPLE_REFERENCE_TYPE]

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LPS_PRINCIPLE_REFERENCE_TYPE]') AND type in (N'U'))
DROP TABLE [dbo].[LPS_PRINCIPLE_REFERENCE_TYPE]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UFC_UNDER_FILING_CASE]') AND type in (N'U'))
DROP TABLE [dbo].[UFC_UNDER_FILING_CASE]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_PRINCIPLE_REFERENCE]') AND type in (N'U'))
DROP TABLE [dbo].[LEGAL_PRINCIPLE_REFERENCE]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_PRINCIPLE_TAG]') AND type in (N'U'))
DROP TABLE [dbo].[LEGAL_PRINCIPLE_TAG]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_PRINCIPLE_CATEGORY_FTW_LKP]') AND type in (N'U'))
DROP TABLE [dbo].[LEGAL_PRINCIPLE_CATEGORY_FTW_LKP]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_PRINCIPLE_LEGAL_SUB_CATEGORY]') AND type in (N'U'))
DROP TABLE [dbo].[LEGAL_PRINCIPLE_LEGAL_SUB_CATEGORY]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_PRINCIPLE_SUB_CATEGORY_FTW_LKP]') AND type in (N'U'))
DROP TABLE [dbo].[LEGAL_PRINCIPLE_SUB_CATEGORY_FTW_LKP]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_PRINCIPLE_STATUS]') AND type in (N'U'))
DROP TABLE [dbo].[LEGAL_PRINCIPLE_STATUS]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_PRINCIPLE_ARTICLE]') AND type in (N'U'))
DROP TABLE [dbo].[LEGAL_PRINCIPLE_ARTICLE]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_PRINCIPLE]') AND type in (N'U'))
DROP TABLE [dbo].[LEGAL_PRINCIPLE]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_PRINCIPLE_ARTICLE_CHILD]') AND type in (N'U'))
DROP TABLE [dbo].[LEGAL_PRINCIPLE_ARTICLE_CHILD]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_PRINCIPLE_ARTICLE_SOURCE]') AND type in (N'U'))
DROP TABLE [dbo].[LEGAL_PRINCIPLE_ARTICLE_SOURCE]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_PRINCIPLE_CONCLUSION]') AND type in (N'U'))
DROP TABLE [dbo].[LEGAL_PRINCIPLE_CONCLUSION]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_PRINCIPLE_LEGAL_CATEGORY]') AND type in (N'U'))
DROP TABLE [dbo].[LEGAL_PRINCIPLE_LEGAL_CATEGORY]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_PRINCIPLE_LEGAL_TAG]') AND type in (N'U'))
DROP TABLE [dbo].[LEGAL_PRINCIPLE_LEGAL_TAG]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_PRINCIPLE_NOTE]') AND type in (N'U'))
DROP TABLE [dbo].[LEGAL_PRINCIPLE_NOTE]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_PRINCIPLE_PUBLICATION_SOURCE]') AND type in (N'U'))
DROP TABLE [dbo].[LEGAL_PRINCIPLE_PUBLICATION_SOURCE]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_PRINCIPLE_PUBLICATION_SOURCE_NAME]') AND type in (N'U'))
DROP TABLE [dbo].[LEGAL_PRINCIPLE_PUBLICATION_SOURCE_NAME]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_PRINCIPLE_REFERENCE_Type]') AND type in (N'U'))
DROP TABLE [dbo].[LEGAL_PRINCIPLE_REFERENCE_Type]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_PRINCIPLE_SUMMARY]') AND type in (N'U'))
DROP TABLE [dbo].[LEGAL_PRINCIPLE_SUMMARY]
GO

------ Remove first and second version of Legal Principle Syatem tables and constraints from Fatwa Portal End 20-02-2025 -------


---------------------------LMS_LITERATURE_TAG
IF COLUMNPROPERTY(OBJECT_ID('dbo.LMS_LITERATURE_TAG'), 'Description_Ar', 'ColumnId') IS NULL
BEGIN
ALTER TABLE LMS_LITERATURE_TAG 
ADD Description_Ar NVARCHAR(1024) NOT NULL DEFAULT ''
Print('LMS_LITERATURE_TAG.Description_Ar Added')
END

-----------------------------LOOKUPS_HISTORY
IF COLUMNPROPERTY(OBJECT_ID('dbo.LOOKUPS_HISTORY'), 'DescriptionAr', 'ColumnId') IS NULL
BEGIN
ALTER TABLE LOOKUPS_HISTORY 
ADD DescriptionAr NVARCHAR(1024) NULL
Print('LOOKUPS_HISTORY.DescriptionAr Added')
END

----- Literature 04-28-2025
IF COLUMNPROPERTY(OBJECT_ID('dbo.LMS_LITERATURE_DETAILS'), 'Publisher', 'ColumnId') IS NOT NULL
BEGIN
ALTER TABLE LMS_LITERATURE_DETAILS
ALTER COLUMN Publisher nvarchar(1024) NULL
END

-----------------------------------------------------------FATWA Validations (Shayan 05/01/25)---------------------------------------------------------------------------------------------
EXEC [dbo].pInsTranslation 'Max_Hundred_Characters',N'Maximum 100 characters are allowed.','Maximum 100 characters are allowed.','G2G Case Management',1

EXEC [dbo].pInsTranslation 'Max_Three_Hundred_Characters',N'Maximum 300 characters are allowed.','Maximum 300 characters are allowed.','G2G Case Management',1

EXEC [dbo].pInsTranslation 'Max_Ninety_Characters',N'Maximum 90 characters are allowed.','Maximum 90 characters are allowed.','G2G Case Management',1

EXEC [dbo].pInsTranslation 'Max_Fifty_Characters',N'Maximum 50 characters are allowed.','Maximum 50 characters are allowed.','G2G Case Management',1


---------------------------------------------------------Save Case Request-----------------------------------------------------------------------------------------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_CASE_REQUEST]') AND type in (N'U'))
BEGIN 
	UPDATE CMS_CASE_REQUEST
	SET Subject = LEFT(Subject, 150)
	WHERE LEN(Subject) > 150;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_CASE_REQUEST'), 'Subject', 'ColumnId') IS NOT NULL
BEGIN
	ALTER TABLE CMS_CASE_REQUEST
	ALTER COLUMN Subject NVARCHAR(512);
END

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_CASE_REQUEST]') AND type in (N'U'))
BEGIN 
	UPDATE CMS_CASE_REQUEST
	SET CaseRequirements = LEFT(CaseRequirements, 1000)
	WHERE LEN(CaseRequirements) > 1000;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_CASE_REQUEST'), 'CaseRequirements', 'ColumnId') IS NOT NULL
BEGIN
	ALTER TABLE CMS_CASE_REQUEST
	ALTER COLUMN CaseRequirements NVARCHAR(1024);
END

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_CASE_REQUEST]') AND type in (N'U'))
BEGIN 
	UPDATE CMS_CASE_REQUEST
	SET Remarks = LEFT(Remarks, 1000)
	WHERE LEN(Remarks) > 1000;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_CASE_REQUEST'), 'Remarks', 'ColumnId') IS NOT NULL
BEGIN
	ALTER TABLE CMS_CASE_REQUEST
	ALTER COLUMN Remarks NVARCHAR(1024);
END
-------------------------------------------------------------Case Party Link---------------------------------------------------------------------------------------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_CASE_PARTY_LINK]') AND type in (N'U'))
BEGIN 
	UPDATE CMS_CASE_PARTY_LINK
	SET Name = LEFT(Name, 90)
	WHERE LEN(Name) > 90;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_CASE_PARTY_LINK'), 'Name', 'ColumnId') IS NOT NULL
BEGIN
	ALTER TABLE CMS_CASE_PARTY_LINK
	ALTER COLUMN Name NVARCHAR(150);
END

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_CASE_PARTY_LINK]') AND type in (N'U'))
BEGIN 
	UPDATE CMS_CASE_PARTY_LINK
	SET CivilId= LEFT(CivilId, 12)
	WHERE LEN(CivilId) > 12;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_CASE_PARTY_LINK'), 'CivilId', 'ColumnId') IS NOT NULL
BEGIN
	ALTER TABLE CMS_CASE_PARTY_LINK
	ALTER COLUMN CivilId NVARCHAR(20);
END

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_CASE_PARTY_LINK]') AND type in (N'U'))
BEGIN 
	UPDATE CMS_CASE_PARTY_LINK
	SET CRN = LEFT(CRN, 30)
	WHERE LEN(CRN) > 30;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_CASE_PARTY_LINK'), 'CRN', 'ColumnId') IS NOT NULL
BEGIN
	ALTER TABLE CMS_CASE_PARTY_LINK
	ALTER COLUMN CRN NVARCHAR(50);
END

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_CASE_PARTY_LINK]') AND type in (N'U'))
BEGIN 
	UPDATE CMS_CASE_PARTY_LINK
	SET PACINumber = LEFT(PACINumber, 12)
	WHERE LEN(PACINumber) > 12;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_CASE_PARTY_LINK'), 'PACINumber', 'ColumnId') IS NOT NULL
BEGIN
	ALTER TABLE CMS_CASE_PARTY_LINK
	ALTER COLUMN PACINumber NVARCHAR(20);
END

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_CASE_PARTY_LINK]') AND type in (N'U'))
BEGIN 
	UPDATE CMS_CASE_PARTY_LINK
	SET CompanyCivilId = LEFT(CompanyCivilId, 12)
	WHERE LEN(CompanyCivilId) > 12;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_CASE_PARTY_LINK'), 'CompanyCivilId', 'ColumnId') IS NOT NULL
BEGIN
	ALTER TABLE CMS_CASE_PARTY_LINK
	ALTER COLUMN CompanyCivilId NVARCHAR(20);
END

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_CASE_PARTY_LINK]') AND type in (N'U'))
BEGIN
	UPDATE CMS_CASE_PARTY_LINK
	SET MOCIFileNumber = LEFT(MOCIFileNumber, 12)
	WHERE LEN(MOCIFileNumber) > 12;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_CASE_PARTY_LINK'), 'MOCIFileNumber', 'ColumnId') IS NOT NULL
BEGIN
	ALTER TABLE CMS_CASE_PARTY_LINK
	ALTER COLUMN MOCIFileNumber NVARCHAR(20);
END

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_CASE_PARTY_LINK]') AND type in (N'U'))
BEGIN
	UPDATE CMS_CASE_PARTY_LINK
	SET LicenseNumber = LEFT(LicenseNumber, 12)
	WHERE LEN(LicenseNumber) > 12;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_CASE_PARTY_LINK'), 'LicenseNumber', 'ColumnId') IS NOT NULL
BEGIN
	ALTER TABLE CMS_CASE_PARTY_LINK
	ALTER COLUMN LicenseNumber NVARCHAR(20);
END

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_CASE_PARTY_LINK]') AND type in (N'U'))
BEGIN
	UPDATE CMS_CASE_PARTY_LINK
	SET MembershipNumber = LEFT(MembershipNumber, 12)
	WHERE LEN(MembershipNumber) > 12;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_CASE_PARTY_LINK'), 'MembershipNumber', 'ColumnId') IS NOT NULL
BEGIN
	ALTER TABLE CMS_CASE_PARTY_LINK
	ALTER COLUMN MembershipNumber NVARCHAR(20);
END

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_CASE_PARTY_LINK]') AND type in (N'U'))
BEGIN
	UPDATE CMS_CASE_PARTY_LINK
	SET Address = LEFT(Address, 150)
	WHERE LEN(Address) > 150;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_CASE_PARTY_LINK'), 'Address', 'ColumnId') IS NOT NULL
BEGIN
	ALTER TABLE CMS_CASE_PARTY_LINK
	ALTER COLUMN Address NVARCHAR(512);
END
---------------------------------------------------------Send Communication (Send Message)-------------------------------------------------------------------------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[COMM_COMMUNICATION]') AND type in (N'U'))
BEGIN
	UPDATE COMM_COMMUNICATION
	SET Title = LEFT(Title, 100)
	WHERE LEN(Title) >100
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.COMM_COMMUNICATION'), 'Title', 'ColumnId') IS NOT NULL
BEGIN
	ALTER TABLE COMM_COMMUNICATION
	ALTER COLUMN Title NVARCHAR(150);
END

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[COMM_COMMUNICATION]') AND type in (N'U'))
BEGIN
	UPDATE COMM_COMMUNICATION
	SET Description = LEFT(Description, 1000)
	WHERE LEN(Description) >1000
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.COMM_COMMUNICATION'), 'Description', 'ColumnId') IS NOT NULL
BEGIN
	ALTER TABLE COMM_COMMUNICATION
	ALTER COLUMN Description NVARCHAR(1024);
END
---------------------------------------------------------Send Communication (Request for Meeting)-------------------------------------------------------------------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[COMM_COMMUNICATION_MEETING]') AND type in (N'U'))
BEGIN
	UPDATE COMM_COMMUNICATION_MEETING
	SET Description = LEFT(Description, 1000)
	WHERE LEN(Description) > 1000;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.COMM_COMMUNICATION_MEETING'), 'Description', 'ColumnId') IS NOT NULL
BEGIN
	ALTER TABLE COMM_COMMUNICATION_MEETING
	ALTER COLUMN Description NVARCHAR(1024);
END

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[COMM_COMMUNICATION_MEETING]') AND type in (N'U'))
BEGIN
	UPDATE COMM_COMMUNICATION_MEETING
	SET Subject = LEFT(Subject, 100)
	WHERE LEN(Subject) > 100;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.COMM_COMMUNICATION_MEETING'), 'Subject', 'ColumnId') IS NOT NULL
BEGIN
	ALTER TABLE COMM_COMMUNICATION_MEETING
	ALTER COLUMN Subject NVARCHAR(150);
END

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[COMM_COMMUNICATION_MEETING]') AND type in (N'U'))
BEGIN
	UPDATE COMM_COMMUNICATION_MEETING
	SET Agenda = LEFT(Agenda, 1000)
	WHERE LEN(Agenda) > 1000;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.COMM_COMMUNICATION_MEETING'), 'Agenda', 'ColumnId') IS NOT NULL
BEGIN
	ALTER TABLE COMM_COMMUNICATION_MEETING
	ALTER COLUMN Agenda NVARCHAR(1024);
END

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[COMM_COMMUNICATION_MEETING]') AND type in (N'U'))
BEGIN
	UPDATE COMM_COMMUNICATION_MEETING
	SET MeetingLink = LEFT(MeetingLink, 100)
	WHERE LEN(MeetingLink) > 100;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.COMM_COMMUNICATION_MEETING'), 'MeetingLink', 'ColumnId') IS NOT NULL
BEGIN
	ALTER TABLE COMM_COMMUNICATION_MEETING
	ALTER COLUMN MeetingLink NVARCHAR(150);
END

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[COMM_COMMUNICATION_MEETING]') AND type in (N'U'))
BEGIN
	UPDATE COMM_COMMUNICATION_MEETING
	SET Location = LEFT(Location, 150)
	WHERE LEN(Location) > 150;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.COMM_COMMUNICATION_MEETING'), 'Location', 'ColumnId') IS NOT NULL
BEGIN
	ALTER TABLE COMM_COMMUNICATION_MEETING
	ALTER COLUMN Location NVARCHAR(512);
END

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[COMM_COMMUNICATION_MEETING]') AND type in (N'U'))
BEGIN
	UPDATE COMM_COMMUNICATION_MEETING
	SET Note = LEFT(Note, 1000)
	WHERE LEN(Note) > 1000;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.COMM_COMMUNICATION_MEETING'), 'Note', 'ColumnId') IS NOT NULL
BEGIN
	ALTER TABLE COMM_COMMUNICATION_MEETING
	ALTER COLUMN Note NVARCHAR(1024);
END
---------------------------------------------------------Upload Document Table--------------------------------------------------------------------------------------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UPLOADED_DOCUMENT]') AND type in (N'U'))
BEGIN
	UPDATE UPLOADED_DOCUMENT
	SET Description = LEFT(Description, 1000)
	WHERE LEN(Description) > 1000;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.UPLOADED_DOCUMENT'), 'Description', 'ColumnId') IS NOT NULL
BEGIN
	ALTER TABLE DMS_DB_DEV.dbo.UPLOADED_DOCUMENT
	ALTER COLUMN Description NVARCHAR(1024);
END

---------------------------------------------------------Meet Meeting Table-----------------------------------------------------------------------------------------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MEET_MEETING]') AND type in (N'U'))
BEGIN
	UPDATE MEET_MEETING
	SET Description = LEFT(Description, 1000)
	WHERE LEN(Description) > 1000;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.MEET_MEETING'), 'Description', 'ColumnId') IS NOT NULL
BEGIN
	ALTER TABLE MEET_MEETING
	ALTER COLUMN Description NVARCHAR(1024);
END

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MEET_MEETING]') AND type in (N'U'))
BEGIN
	UPDATE MEET_MEETING
	SET Subject= LEFT(Subject, 100)
	WHERE LEN(Subject) > 100;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.MEET_MEETING'), 'Subject', 'ColumnId') IS NOT NULL
BEGIN
	ALTER TABLE MEET_MEETING
	ALTER COLUMN Subject NVARCHAR(150);
END

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MEET_MEETING]') AND type in (N'U'))
BEGIN
	UPDATE MEET_MEETING
	SET Agenda = LEFT(Agenda, 1000)
	WHERE LEN(Agenda) > 1000;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.MEET_MEETING'), 'Agenda', 'ColumnId') IS NOT NULL
BEGIN
	ALTER TABLE MEET_MEETING
	ALTER COLUMN Agenda NVARCHAR(1024);
END

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MEET_MEETING]') AND type in (N'U'))
BEGIN
	UPDATE MEET_MEETING
	SET MeetingLink = LEFT(MeetingLink, 100)
	WHERE LEN(MeetingLink) > 100;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.MEET_MEETING'), 'MeetingLink', 'ColumnId') IS NOT NULL
BEGIN
	ALTER TABLE MEET_MEETING
	ALTER COLUMN MeetingLink NVARCHAR(150);
END

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MEET_MEETING]') AND type in (N'U'))
BEGIN
	UPDATE MEET_MEETING
	SET Location = LEFT(Location, 150)
	WHERE LEN(Location) > 150;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.MEET_MEETING'), 'Location', 'ColumnId') IS NOT NULL
BEGIN
	ALTER TABLE MEET_MEETING
	ALTER COLUMN Location NVARCHAR(512);
END

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MEET_MEETING]') AND type in (N'U'))
BEGIN
	UPDATE MEET_MEETING
	SET Note = LEFT(Note, 1000)
	WHERE LEN(Note) > 1000;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.MEET_MEETING'), 'Note', 'ColumnId') IS NOT NULL
BEGIN
	ALTER TABLE MEET_MEETING
	ALTER COLUMN Note NVARCHAR(1024);
END

--------------------------------------------------------CMS Case Assignment------------------------------------------------------------------------------------------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_CASE_ASSIGNMENT]') AND type in (N'U'))
BEGIN
	UPDATE CMS_CASE_ASSIGNMENT
	SET Remarks = LEFT(Remarks, 1000)
	WHERE LEN(Remarks) > 1000;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_CASE_ASSIGNMENT'), 'Remarks', 'ColumnId') IS NOT NULL
BEGIN
	ALTER TABLE CMS_CASE_ASSIGNMENT
	ALTER COLUMN Remarks NVARCHAR(1024);
END

--------------------------------------------------------CMS Registered Case------------------------------------------------------------------------------------------------------------------

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_REGISTERED_CASE]') AND type in (N'U'))
BEGIN
	UPDATE CMS_REGISTERED_CASE
	SET FloorNumber = LEFT(FloorNumber, 2)
	WHERE LEN(FloorNumber) > 2;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_REGISTERED_CASE'), 'FloorNumber', 'ColumnId') IS NOT NULL
BEGIN
	--ALTER TABLE CMS_REGISTERED_CASE
	--DROP CONSTRAINT DF__CMS_REGIS__Floor__33BFA6FF;

	ALTER TABLE CMS_REGISTERED_CASE
	ALTER COLUMN FloorNumber NVARCHAR(20) NOT NULL;

	--ALTER TABLE CMS_REGISTERED_CASE
	--ADD CONSTRAINT DF__CMS_REGIS__Floor__33BFA6FF DEFAULT '' FOR FloorNumber;
END

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_REGISTERED_CASE]') AND type in (N'U'))
BEGIN
	UPDATE CMS_REGISTERED_CASE
	SET RoomNumber = LEFT(RoomNumber, 4)
	WHERE LEN(RoomNumber) > 4;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_REGISTERED_CASE'), 'RoomNumber', 'ColumnId') IS NOT NULL
BEGIN

	--ALTER TABLE CMS_REGISTERED_CASE
	--DROP CONSTRAINT DF__CMS_REGIS__RoomN__34B3CB38;

	ALTER TABLE CMS_REGISTERED_CASE
	ALTER COLUMN RoomNumber NVARCHAR(20) NOT NULL;

	--ALTER TABLE CMS_REGISTERED_CASE
	--ADD CONSTRAINT DF__CMS_REGIS__RoomN__34B3CB38 DEFAULT '' FOR RoomNumber;
END

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_REGISTERED_CASE]') AND type in (N'U'))
BEGIN
	UPDATE CMS_REGISTERED_CASE
	SET AnnouncementNumber = LEFT(AnnouncementNumber, 2)
	WHERE LEN(AnnouncementNumber) > 2;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_REGISTERED_CASE'), 'AnnouncementNumber', 'ColumnId') IS NOT NULL
BEGIN
	--ALTER TABLE CMS_REGISTERED_CASE
	--DROP CONSTRAINT DF__CMS_REGIS__Annou__35A7EF71;

	ALTER TABLE CMS_REGISTERED_CASE
	ALTER COLUMN AnnouncementNumber NVARCHAR(20) NOT NULL;

	--ALTER TABLE CMS_REGISTERED_CASE
	--ADD CONSTRAINT DF__CMS_REGIS__Annou__35A7EF71 DEFAULT '' FOR AnnouncementNumber;
END

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_REGISTERED_CASE]') AND type in (N'U'))
BEGIN
	UPDATE CMS_REGISTERED_CASE
	SET CANNumber = LEFT(CANNumber, 9)
	WHERE LEN(CANNumber) > 9;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_REGISTERED_CASE'), 'CANNumber', 'ColumnId') IS NOT NULL
BEGIN
	ALTER TABLE CMS_REGISTERED_CASE
	ALTER COLUMN CANNumber NVARCHAR(20) NOT NULL;
END

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_REGISTERED_CASE]') AND type in (N'U'))
BEGIN
	UPDATE CMS_REGISTERED_CASE
	SET CaseNumber = LEFT(CaseNumber, 8)
	WHERE LEN(CaseNumber) > 8;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_REGISTERED_CASE'), 'CaseNumber', 'ColumnId') IS NOT NULL
BEGIN
	--ALTER TABLE CMS_REGISTERED_CASE
	--DROP CONSTRAINT UC_CaseNumber

	ALTER TABLE CMS_REGISTERED_CASE
	ALTER COLUMN CaseNumber NVARCHAR(20) NOT NULL;

	--ALTER TABLE CMS_REGISTERED_CASE
	--ADD CONSTRAINT UC_CaseNumber UNIQUE(CaseNumber);
END

--------------------------------------------------------Legal Library (Add Legislation)-------------------------------------------------------------------------------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_LEGISLATION]') AND type in (N'U'))
BEGIN
	UPDATE LEGAL_LEGISLATION
	SET Legislation_Number = LEFT(Legislation_Number, 50)
	WHERE LEN(Legislation_Number) > 50;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.LEGAL_LEGISLATION'), 'Legislation_Number', 'ColumnId') IS NOT NULL
BEGIN
	ALTER TABLE LEGAL_LEGISLATION
	ALTER COLUMN Legislation_Number NVARCHAR(150);
END

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_LEGISLATION]') AND type in (N'U'))
BEGIN
	UPDATE LEGAL_LEGISLATION
	SET LegislationTitle = LEFT(LegislationTitle, 300)
	WHERE LEN(LegislationTitle) > 300;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.LEGAL_LEGISLATION'), 'LegislationTitle', 'ColumnId') IS NOT NULL
BEGIN
	ALTER TABLE LEGAL_LEGISLATION
	ALTER COLUMN LegislationTitle NVARCHAR(512);
END


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_LEGISLATION_TAG]') AND type in (N'U'))
BEGIN
	UPDATE LEGAL_LEGISLATION_TAG
	SET TagName = LEFT(TagName, 90)
	WHERE LEN(TagName) > 90;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.LEGAL_LEGISLATION_TAG'), 'TagName', 'ColumnId') IS NOT NULL
BEGIN
	ALTER TABLE LEGAL_LEGISLATION_TAG
	ALTER COLUMN TagName NVARCHAR(150) NOT NULL;
END

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_LEGISLATION_SIGNATURE]') AND type in (N'U'))
BEGIN
	UPDATE LEGAL_LEGISLATION_SIGNATURE
	SET Full_Name = LEFT(Full_Name, 90)
	WHERE LEN(Full_Name) > 90;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.LEGAL_LEGISLATION_SIGNATURE'), 'Full_Name', 'ColumnId') IS NOT NULL
BEGIN
	ALTER TABLE LEGAL_LEGISLATION_SIGNATURE
	ALTER COLUMN Full_Name NVARCHAR(150) NOT NULL;
END

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_LEGISLATION_SIGNATURE]') AND type in (N'U'))
BEGIN
	UPDATE LEGAL_LEGISLATION_SIGNATURE
	SET Job_Title = LEFT(Job_Title, 50)
	WHERE LEN(Job_Title) > 50;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.LEGAL_LEGISLATION_SIGNATURE'), 'Job_Title', 'ColumnId') IS NOT NULL
BEGIN
	ALTER TABLE LEGAL_LEGISLATION_SIGNATURE
	ALTER COLUMN Job_Title NVARCHAR(150) NOT NULL;
END

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_ARTICLE]') AND type in (N'U'))
BEGIN
	UPDATE LEGAL_ARTICLE
	SET Article_Name = LEFT(Article_Name, 300)
	WHERE LEN(Article_Name) > 300;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.LEGAL_ARTICLE'), 'Article_Name', 'ColumnId') IS NOT NULL
BEGIN
	ALTER TABLE LEGAL_ARTICLE
	ALTER COLUMN Article_Name NVARCHAR(512) NOT NULL;
END

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_ARTICLE]') AND type in (N'U'))
BEGIN
	UPDATE LEGAL_ARTICLE
	SET Article_Title = LEFT(Article_Title, 300)
	WHERE LEN(Article_Title) > 300;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.LEGAL_ARTICLE'), 'Article_Title', 'ColumnId') IS NOT NULL
BEGIN
	ALTER TABLE LEGAL_ARTICLE
	ALTER COLUMN Article_Title NVARCHAR(512) NOT NULL;
END

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_SECTION]') AND type in (N'U'))
BEGIN
	UPDATE LEGAL_SECTION
	SET SectionTitle = LEFT(SectionTitle, 1000)
	WHERE LEN(SectionTitle) > 1000;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.LEGAL_SECTION'), 'SectionTitle', 'ColumnId') IS NOT NULL
BEGIN
	ALTER TABLE LEGAL_SECTION
	ALTER COLUMN SectionTitle NVARCHAR(1024) NOT NULL;
END

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_SECTION]') AND type in (N'U'))
BEGIN
	UPDATE LEGAL_SECTION
	SET SectionParentTitle = LEFT(SectionParentTitle, 1000)
	WHERE LEN(SectionParentTitle) > 1000;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.LEGAL_SECTION'), 'SectionParentTitle', 'ColumnId') IS NOT NULL
BEGIN
	ALTER TABLE LEGAL_SECTION
	ALTER COLUMN SectionParentTitle NVARCHAR(1024);
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.LEGAL_TEMPLATE'), 'Template_Name', 'ColumnId') IS NOT NULL
BEGIN
	ALTER TABLE LEGAL_TEMPLATE
	ALTER COLUMN Template_Name NVARCHAR(1024) NOT NULL;
END
----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
---------VALIDATION UPDATIONS 30/12/2024----------------------


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[COMS_CONSULTATION_REQUEST]') AND type in (N'U'))
BEGIN
UPDATE COMS_CONSULTATION_REQUEST
SET Subject = LEFT(Subject, 50)
WHERE LEN(Subject) > 50;
 END
 IF COLUMNPROPERTY(OBJECT_ID('dbo.COMS_CONSULTATION_REQUEST'), 'Subject', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE COMS_CONSULTATION_REQUEST ALTER COLUMN Subject NVARCHAR(150);
END


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[COMS_CONSULTATION_REQUEST]') AND type in (N'U'))
BEGIN
UPDATE COMS_CONSULTATION_REQUEST
SET GEOpinion = LEFT(GEOpinion, 300)
WHERE LEN(GEOpinion) > 300;
 END
 IF COLUMNPROPERTY(OBJECT_ID('dbo.COMS_CONSULTATION_REQUEST'), 'GEOpinion', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE COMS_CONSULTATION_REQUEST ALTER COLUMN GEOpinion NVARCHAR(512);
END

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[COMS_CONSULTATION_REQUEST]') AND type in (N'U'))
BEGIN
UPDATE COMS_CONSULTATION_REQUEST
SET Remarks = LEFT(Remarks, 300)
WHERE LEN(Remarks) > 300;
 END
 IF COLUMNPROPERTY(OBJECT_ID('dbo.COMS_CONSULTATION_REQUEST'), 'Remarks', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE COMS_CONSULTATION_REQUEST ALTER COLUMN Remarks NVARCHAR(512);
END

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[COMS_CONSULTATION_REQUEST]') AND type in (N'U'))
BEGIN
UPDATE COMS_CONSULTATION_REQUEST
SET CompetentAuthorityOpinionWithNote = LEFT(CompetentAuthorityOpinionWithNote, 300)
WHERE LEN(CompetentAuthorityOpinionWithNote) > 300;
 END
 IF COLUMNPROPERTY(OBJECT_ID('dbo.COMS_CONSULTATION_REQUEST'), 'CompetentAuthorityOpinionWithNote', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE COMS_CONSULTATION_REQUEST ALTER COLUMN CompetentAuthorityOpinionWithNote NVARCHAR(512);
END

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[COMS_CONSULTATION_REQUEST]') AND type in (N'U'))
BEGIN
UPDATE COMS_CONSULTATION_REQUEST
SET ComplainantDecisionNumber = LEFT(ComplainantDecisionNumber, 30)
WHERE LEN(ComplainantDecisionNumber) > 30;
 END
 IF COLUMNPROPERTY(OBJECT_ID('dbo.COMS_CONSULTATION_REQUEST'), 'ComplainantDecisionNumber', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE COMS_CONSULTATION_REQUEST ALTER COLUMN ComplainantDecisionNumber NVARCHAR(50);
END

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[COMS_CONSULTATION_REQUEST]') AND type in (N'U'))
BEGIN
UPDATE COMS_CONSULTATION_REQUEST
SET ComplaintAgainst = LEFT(ComplaintAgainst, 90)
WHERE LEN(ComplaintAgainst) > 90;
 END
 IF COLUMNPROPERTY(OBJECT_ID('dbo.COMS_CONSULTATION_REQUEST'), 'ComplaintAgainst', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE COMS_CONSULTATION_REQUEST ALTER COLUMN ComplaintAgainst NVARCHAR(150);
END


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[COMS_CONSULTATION_REQUEST]') AND type in (N'U'))
BEGIN
UPDATE COMS_CONSULTATION_REQUEST
SET ComplainantName = LEFT(ComplainantName, 90)
WHERE LEN(ComplainantName) > 90;
 END
 IF COLUMNPROPERTY(OBJECT_ID('dbo.COMS_CONSULTATION_REQUEST'), 'ComplainantName', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE COMS_CONSULTATION_REQUEST ALTER COLUMN ComplainantName NVARCHAR(150);
END

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[COMS_CONSULTATION_REQUEST]') AND type in (N'U'))
BEGIN
UPDATE COMS_CONSULTATION_REQUEST
SET HighPriorityReason = LEFT(HighPriorityReason, 1000)
WHERE LEN(HighPriorityReason) > 1000;
 END
 IF COLUMNPROPERTY(OBJECT_ID('dbo.COMS_CONSULTATION_REQUEST'), 'HighPriorityReason', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE COMS_CONSULTATION_REQUEST ALTER COLUMN HighPriorityReason NVARCHAR(1024);
END


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[COMS_CONSULTATION_ASSIGNMENT]') AND type in (N'U'))
BEGIN
UPDATE COMS_CONSULTATION_ASSIGNMENT
SET Remarks = LEFT(Remarks, 1000)
WHERE LEN(Remarks) > 1000;
 END
 IF COLUMNPROPERTY(OBJECT_ID('dbo.COMS_CONSULTATION_ASSIGNMENT'), 'HighPriorityReason', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE COMS_CONSULTATION_REQUEST ALTER COLUMN Remarks NVARCHAR(1024);
END

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rejection]') AND type in (N'U'))
BEGIN
UPDATE Rejection
SET Reason = LEFT(Reason, 1000)
WHERE LEN(Reason) > 1000;
 END
 IF COLUMNPROPERTY(OBJECT_ID('dbo.Rejection'), 'Rejection', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE Rejection ALTER COLUMN Reason NVARCHAR(1024);
END


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_APPROVAL_TRACKING]') AND type in (N'U'))
BEGIN
UPDATE CMS_APPROVAL_TRACKING
SET Remarks = LEFT(Remarks, 1000)
WHERE LEN(Remarks) > 1000;
 END
 IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_APPROVAL_TRACKING'), 'Remarks', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE CMS_APPROVAL_TRACKING ALTER COLUMN Remarks NVARCHAR(1024);
END

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[COMS_CONSULTATION_ASSIGNMENT]') AND type in (N'U'))
BEGIN
UPDATE COMS_CONSULTATION_ASSIGNMENT
SET Remarks = LEFT(Remarks, 1000)
WHERE LEN(Remarks) > 1000;
 END
 IF COLUMNPROPERTY(OBJECT_ID('dbo.COMS_CONSULTATION_ASSIGNMENT'), 'Remarks', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE CMS_APPROVAL_TRACKING ALTER COLUMN Remarks NVARCHAR(1024);
END


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LMS_LITERATURE_DETAILS]') AND type in (N'U'))
BEGIN
UPDATE LMS_LITERATURE_DETAILS
SET Name = LEFT(Name, 90)
WHERE LEN(Name) > 90;
 END
 IF COLUMNPROPERTY(OBJECT_ID('dbo.LMS_LITERATURE_DETAILS'), 'Name', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE LMS_LITERATURE_DETAILS ALTER COLUMN Name NVARCHAR(150) NOT NULL;
END


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LMS_LITERATURE_DETAILS]') AND type in (N'U'))
BEGIN
UPDATE LMS_LITERATURE_DETAILS
SET ISBN = LEFT(ISBN, 15)
WHERE LEN(ISBN) > 15;
 END
 IF COLUMNPROPERTY(OBJECT_ID('dbo.LMS_LITERATURE_DETAILS'), 'ISBN', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE LMS_LITERATURE_DETAILS ALTER COLUMN ISBN NVARCHAR(50);
END


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LMS_LITERATURE_DETAILS]') AND type in (N'U'))
BEGIN
UPDATE LMS_LITERATURE_DETAILS
SET Publisher = LEFT(Publisher, 1000)
WHERE LEN(Publisher) > 1000;
 END
 IF COLUMNPROPERTY(OBJECT_ID('dbo.LMS_LITERATURE_DETAILS'), 'Publisher', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE LMS_LITERATURE_DETAILS ALTER COLUMN Publisher NVARCHAR(1024) NOT NULL;
END


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LMS_LITERATURE_AUTHOR]') AND type in (N'U'))
BEGIN
UPDATE LMS_LITERATURE_AUTHOR
SET Address_En = LEFT(Address_En, 150)
WHERE LEN(Address_En) > 150;
 END
 IF COLUMNPROPERTY(OBJECT_ID('dbo.LMS_LITERATURE_AUTHOR'), 'Address_En', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE LMS_LITERATURE_AUTHOR ALTER COLUMN Address_En NVARCHAR(512);
END

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LMS_LITERATURE_AUTHOR]') AND type in (N'U'))
BEGIN
UPDATE LMS_LITERATURE_AUTHOR
SET Address_Ar = LEFT(Address_Ar, 150)
WHERE LEN(Address_Ar) > 150;
 END
 IF COLUMNPROPERTY(OBJECT_ID('dbo.LMS_LITERATURE_AUTHOR'), 'Address_Ar', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE LMS_LITERATURE_AUTHOR ALTER COLUMN Address_Ar NVARCHAR(512);
END


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LMS_LITERATURE_PURCHASE]') AND type in (N'U'))
BEGIN
UPDATE LMS_LITERATURE_PURCHASE
SET Location = LEFT(Location, 150)
WHERE LEN(Location) > 150;
 END
 IF COLUMNPROPERTY(OBJECT_ID('dbo.LMS_LITERATURE_PURCHASE'), 'Location', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE LMS_LITERATURE_PURCHASE ALTER COLUMN Location NVARCHAR(512) NOT NULL;
END
---------Muhammad Ali------------------09-Jan-2025------------------Validation/Limits DB Level-----------
--IMPORTANT: Below all tables are contains the not null set in the database
--Step 1: Below script to check the constraint 
SELECT name 
FROM sys.default_constraints 
WHERE parent_object_id = OBJECT_ID('CMS_COURT_G2G_LKP') 
  AND parent_column_id = COLUMNPROPERTY(OBJECT_ID('CMS_COURT_G2G_LKP'), 'Address', 'ColumnId');

--Step 2: Drop the constraint
ALTER TABLE CMS_COURT_G2G_LKP DROP CONSTRAINT DF__CMS_COURT__Addre__247D636F;

--Step 3: Run the below scirpt
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_COURT_G2G_LKP]') AND type in (N'U'))
BEGIN
update CMS_COURT_G2G_LKP set Name_En = LEFT(Name_En, 150) where LEN(Name_En) > 150;
update CMS_COURT_G2G_LKP set Name_Ar = LEFT(Name_Ar, 150) where LEN(Name_Ar) > 150;
update CMS_COURT_G2G_LKP set [Address] = LEFT([Address], 150) where LEN([Address]) > 150;
update CMS_COURT_G2G_LKP set District = LEFT(District, 150) where LEN(District) > 150;
update CMS_COURT_G2G_LKP set [Location] = LEFT([Location], 150) where LEN([Location]) > 150;
update CMS_COURT_G2G_LKP set [Description] = LEFT([Description], 1024) where LEN([Description]) > 1024;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_COURT_G2G_LKP'), 'Name_En', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE CMS_COURT_G2G_LKP ALTER COLUMN Name_En NVARCHAR(150) NOT NULL;
	Print('CMS_COURT_G2G_LKP.Name_En Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_COURT_G2G_LKP'), 'Name_Ar', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE CMS_COURT_G2G_LKP ALTER COLUMN Name_Ar NVARCHAR(150) NOT NULL;
	Print('CMS_COURT_G2G_LKP.Name_Ar Altered')
END
--the below creating issue
--Msg 5074, Level 16, State 1, Line 336
--The object 'DF__CMS_COURT__Addre__247D636F' is dependent on column 'Address'.
--Msg 4922, Level 16, State 9, Line 336
--ALTER TABLE ALTER COLUMN Address failed because one or more objects access this column.
IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_COURT_G2G_LKP'), 'Address', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE CMS_COURT_G2G_LKP ALTER COLUMN [Address] NVARCHAR(150);
	Print('CMS_COURT_G2G_LKP.Address Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_COURT_G2G_LKP'), 'District', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE CMS_COURT_G2G_LKP ALTER COLUMN District NVARCHAR(150);
	Print('CMS_COURT_G2G_LKP.District Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_COURT_G2G_LKP'), 'Location', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE CMS_COURT_G2G_LKP ALTER COLUMN [Location] NVARCHAR(150);
	Print('CMS_COURT_G2G_LKP.Location Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_COURT_G2G_LKP'), 'Description', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE CMS_COURT_G2G_LKP ALTER COLUMN [Description] NVARCHAR(1024);
	Print('CMS_COURT_G2G_LKP.Description Altered')
END

--step 4: add the constraint again:
ALTER TABLE CMS_COURT_G2G_LKP 
ADD CONSTRAINT DF__CMS_COURT__Addre__247D636F DEFAULT 'DefaultValue' FOR Address;
---------------------------------------------------------
--Step 1: Below script to check the constraint 
SELECT name 
FROM sys.default_constraints 
WHERE parent_object_id = OBJECT_ID('CMS_CHAMBER_G2G_LKP') 
  AND parent_column_id = COLUMNPROPERTY(OBJECT_ID('CMS_CHAMBER_G2G_LKP'), 'ChamberCode', 'ColumnId');

--Step 2: Drop the constraint
ALTER TABLE CMS_CHAMBER_G2G_LKP DROP CONSTRAINT DF__CMS_CHAMB__Chamb__20ACD28B;

--Step 3: Run the below scirpt
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_CHAMBER_G2G_LKP]') AND type in (N'U'))
BEGIN
update CMS_CHAMBER_G2G_LKP set Name_En = LEFT(Name_En, 150) where LEN(Name_En) > 150;
update CMS_CHAMBER_G2G_LKP set Name_Ar = LEFT(Name_Ar, 150) where LEN(Name_Ar) > 150;
update CMS_CHAMBER_G2G_LKP set ChamberCode = LEFT(ChamberCode, 50) where LEN(ChamberCode) > 50;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_CHAMBER_G2G_LKP'), 'Name_En', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE CMS_CHAMBER_G2G_LKP ALTER COLUMN Name_En NVARCHAR(150) NOT NULL;
	Print('CMS_CHAMBER_G2G_LKP.Name_En Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_CHAMBER_G2G_LKP'), 'Name_Ar', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE CMS_CHAMBER_G2G_LKP ALTER COLUMN Name_Ar NVARCHAR(150) NOT NULL;
	Print('CMS_CHAMBER_G2G_LKP.Name_Ar Altered')
END
--the below creating issue
--Msg 5074, Level 16, State 1, Line 377
--The object 'DF__CMS_CHAMB__Chamb__20ACD28B' is dependent on column 'ChamberCode'.
--Msg 4922, Level 16, State 9, Line 377
--ALTER TABLE ALTER COLUMN ChamberCode failed because one or more objects access this column.
IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_CHAMBER_G2G_LKP'), 'ChamberCode', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE CMS_CHAMBER_G2G_LKP ALTER COLUMN ChamberCode NVARCHAR(150) NOT NULL;
	Print('CMS_CHAMBER_G2G_LKP.ChamberCode Altered')
END

--Step 4: Run the below scirpt
ALTER TABLE CMS_CHAMBER_G2G_LKP 
ADD CONSTRAINT DF__CMS_CHAMB__Chamb__20ACD28B DEFAULT 'DefaultValue' FOR ChamberCode;
---------------------------------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_CHAMBER_NUMBER_G2G_LKP]') AND type in (N'U'))
BEGIN
update CMS_CHAMBER_NUMBER_G2G_LKP set Number = LEFT(Number, 50) where LEN(Number) > 50;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_CHAMBER_NUMBER_G2G_LKP'), 'Number', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE CMS_CHAMBER_NUMBER_G2G_LKP ALTER COLUMN Number NVARCHAR(50) NOT NULL;
	Print('CMS_CHAMBER_NUMBER_G2G_LKP.Number Altered')
END
---------------------------------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_REQUEST_TYPE_G2G_LKP]') AND type in (N'U'))
BEGIN
update CMS_REQUEST_TYPE_G2G_LKP set Name_En = LEFT(Name_En, 150) where LEN(Name_En) > 150;
update CMS_REQUEST_TYPE_G2G_LKP set Name_Ar = LEFT(Name_Ar, 150) where LEN(Name_Ar) > 150;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_REQUEST_TYPE_G2G_LKP'), 'Name_En', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE CMS_REQUEST_TYPE_G2G_LKP ALTER COLUMN Name_En NVARCHAR(150) NOT NULL;
	Print('CMS_REQUEST_TYPE_G2G_LKP.Name_En Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_REQUEST_TYPE_G2G_LKP'), 'Name_Ar', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE CMS_REQUEST_TYPE_G2G_LKP ALTER COLUMN Name_Ar NVARCHAR(150) NOT NULL;
	Print('CMS_REQUEST_TYPE_G2G_LKP.Name_Ar Altered')
END
---------------------------------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_SUBTYPE_G2G_LKP]') AND type in (N'U'))
BEGIN
update CMS_SUBTYPE_G2G_LKP set Name_En = LEFT(Name_En, 150) where LEN(Name_En) > 150;
update CMS_SUBTYPE_G2G_LKP set Name_Ar = LEFT(Name_Ar, 150) where LEN(Name_Ar) > 150;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_SUBTYPE_G2G_LKP'), 'Name_En', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE CMS_SUBTYPE_G2G_LKP ALTER COLUMN Name_En NVARCHAR(150) NOT NULL;
	Print('CMS_SUBTYPE_G2G_LKP.Name_En Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_SUBTYPE_G2G_LKP'), 'Name_Ar', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE CMS_SUBTYPE_G2G_LKP ALTER COLUMN Name_Ar NVARCHAR(150) NOT NULL;
	Print('CMS_SUBTYPE_G2G_LKP.Name_Ar Altered')
END
---------------------------------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_CASE_FILE_STATUS_G2G_LKP]') AND type in (N'U'))
BEGIN
update CMS_CASE_FILE_STATUS_G2G_LKP set Name_En = LEFT(Name_En, 150) where LEN(Name_En) > 150;
update CMS_CASE_FILE_STATUS_G2G_LKP set Name_Ar = LEFT(Name_Ar, 150) where LEN(Name_Ar) > 150;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_CASE_FILE_STATUS_G2G_LKP'), 'Name_En', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE CMS_CASE_FILE_STATUS_G2G_LKP ALTER COLUMN Name_En NVARCHAR(150) NOT NULL;
	Print('CMS_CASE_FILE_STATUS_G2G_LKP.Name_En Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_CASE_FILE_STATUS_G2G_LKP'), 'Name_Ar', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE CMS_CASE_FILE_STATUS_G2G_LKP ALTER COLUMN Name_Ar NVARCHAR(150) NOT NULL;
	Print('CMS_CASE_FILE_STATUS_G2G_LKP.Name_Ar Altered')
END
---------------------------------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_CASE_REQUEST_STATUS_G2G_LKP]') AND type in (N'U'))
BEGIN
update CMS_CASE_REQUEST_STATUS_G2G_LKP set Name_En = LEFT(Name_En, 150) where LEN(Name_En) > 150;
update CMS_CASE_REQUEST_STATUS_G2G_LKP set Name_Ar = LEFT(Name_Ar, 150) where LEN(Name_Ar) > 150;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_CASE_REQUEST_STATUS_G2G_LKP'), 'Name_En', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE CMS_CASE_REQUEST_STATUS_G2G_LKP ALTER COLUMN Name_En NVARCHAR(150) NOT NULL;
	Print('CMS_CASE_REQUEST_STATUS_G2G_LKP.Name_En Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_CASE_REQUEST_STATUS_G2G_LKP'), 'Name_Ar', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE CMS_CASE_REQUEST_STATUS_G2G_LKP ALTER COLUMN Name_Ar NVARCHAR(150) NOT NULL;
	Print('CMS_CASE_REQUEST_STATUS_G2G_LKP.Name_Ar Altered')
END
---------------------------------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_REGISTERED_CASE_STATUS_G2G_LKP]') AND type in (N'U'))
BEGIN
update CMS_REGISTERED_CASE_STATUS_G2G_LKP set Name_En = LEFT(Name_En, 150) where LEN(Name_En) > 150;
update CMS_REGISTERED_CASE_STATUS_G2G_LKP set Name_Ar = LEFT(Name_Ar, 150) where LEN(Name_Ar) > 150;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_REGISTERED_CASE_STATUS_G2G_LKP'), 'Name_En', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE CMS_REGISTERED_CASE_STATUS_G2G_LKP ALTER COLUMN Name_En NVARCHAR(150) NOT NULL;
	Print('CMS_REGISTERED_CASE_STATUS_G2G_LKP.Name_En Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_REGISTERED_CASE_STATUS_G2G_LKP'), 'Name_Ar', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE CMS_REGISTERED_CASE_STATUS_G2G_LKP ALTER COLUMN Name_Ar NVARCHAR(150) NOT NULL;
	Print('CMS_REGISTERED_CASE_STATUS_G2G_LKP.Name_Ar Altered')
END
---------------------------------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_COMS_NUM_PATTERN]') AND type in (N'U'))
BEGIN
update CMS_COMS_NUM_PATTERN set StaticTextPattern = LEFT(StaticTextPattern, 512) where LEN(StaticTextPattern) > 512;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_COMS_NUM_PATTERN'), 'StaticTextPattern', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE CMS_COMS_NUM_PATTERN ALTER COLUMN StaticTextPattern NVARCHAR(512) NOT NULL;
	Print('CMS_COMS_NUM_PATTERN.StaticTextPattern Altered')
END
---------------------------------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_REQUEST_TYPE_G2G_LKP]') AND type in (N'U'))
BEGIN
update CMS_REQUEST_TYPE_G2G_LKP set Name_En = LEFT(Name_En, 150) where LEN(Name_En) > 150;
update CMS_REQUEST_TYPE_G2G_LKP set Name_Ar = LEFT(Name_Ar, 150) where LEN(Name_Ar) > 150;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_REQUEST_TYPE_G2G_LKP'), 'Name_En', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE CMS_REQUEST_TYPE_G2G_LKP ALTER COLUMN Name_En NVARCHAR(150) NOT NULL;
	Print('CMS_REQUEST_TYPE_G2G_LKP.Name_En Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_REQUEST_TYPE_G2G_LKP'), 'Name_Ar', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE CMS_REQUEST_TYPE_G2G_LKP ALTER COLUMN Name_Ar NVARCHAR(150) NOT NULL;
	Print('CMS_REQUEST_TYPE_G2G_LKP.Name_Ar Altered')
END
---------------------------------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ATTACHMENT_TYPE]') AND type in (N'U'))
BEGIN
update ATTACHMENT_TYPE set Type_En = LEFT(Type_En, 150) where LEN(Type_En) > 150;
update ATTACHMENT_TYPE set Type_Ar = LEFT(Type_Ar, 150) where LEN(Type_Ar) > 150;
update ATTACHMENT_TYPE set [Description] = LEFT([Description], 1024) where LEN([Description]) > 1024;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.ATTACHMENT_TYPE'), 'Type_En', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE ATTACHMENT_TYPE ALTER COLUMN Type_En NVARCHAR(150) NOT NULL;
	Print('ATTACHMENT_TYPE.Type_En Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.ATTACHMENT_TYPE'), 'Type_Ar', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE ATTACHMENT_TYPE ALTER COLUMN Type_Ar NVARCHAR(150) NOT NULL;
	Print('ATTACHMENT_TYPE.Type_Ar Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.ATTACHMENT_TYPE'), 'Description', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE ATTACHMENT_TYPE ALTER COLUMN [Description] NVARCHAR(1024);
	Print('ATTACHMENT_TYPE.Description Altered')
END
---------------------------------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tTranslation]') AND type in (N'U'))
BEGIN
update tTranslation set Value_En = LEFT(Value_En, 512) where LEN(Value_En) > 512;
update tTranslation set Value_Ar = LEFT(Value_Ar, 512) where LEN(Value_Ar) > 512;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.tTranslation'), 'Value_En', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE tTranslation ALTER COLUMN Value_En NVARCHAR(512);
	Print('tTranslation.Value_En Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.tTranslation'), 'Value_Ar', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE tTranslation ALTER COLUMN Value_Ar NVARCHAR(512) NOT NULL;
	Print('tTranslation.Value_Ar Altered')
END
---------------------------------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UMS_GROUP]') AND type in (N'U'))
BEGIN
update UMS_GROUP set Name_En = LEFT(Name_En, 150) where LEN(Name_En) > 150;
update UMS_GROUP set Name_Ar = LEFT(Name_Ar, 150) where LEN(Name_Ar) > 150;
update UMS_GROUP set Description_En = LEFT(Description_En, 512) where LEN(Description_En) > 512;
update UMS_GROUP set Description_Ar = LEFT(Description_Ar, 512) where LEN(Description_Ar) > 512;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.UMS_GROUP'), 'Name_En', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE UMS_GROUP ALTER COLUMN Name_En NVARCHAR(150) NOT NULL;
	Print('UMS_GROUP.Name_En Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.UMS_GROUP'), 'Name_Ar', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE UMS_GROUP ALTER COLUMN Name_Ar NVARCHAR(150) NOT NULL;
	Print('UMS_GROUP.Name_Ar Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.UMS_GROUP'), 'Description_En', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE UMS_GROUP ALTER COLUMN Description_En NVARCHAR(512) NOT NULL;
	Print('UMS_GROUP.Description_En Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.UMS_GROUP'), 'Description_Ar', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE UMS_GROUP ALTER COLUMN Description_Ar NVARCHAR(512) NOT NULL;
	Print('UMS_GROUP.Description_Ar Altered')
END
---------------------------------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UMS_CLAIM]') AND type in (N'U'))
BEGIN
update UMS_CLAIM set Title_En = LEFT(Title_En, 512) where LEN(Title_En) > 512;
update UMS_CLAIM set Title_Ar = LEFT(Title_Ar, 512) where LEN(Title_Ar) > 512;
update UMS_CLAIM set Module = LEFT(Module, 512) where LEN(Module) > 512;
update UMS_CLAIM set SubModule = LEFT(SubModule, 512) where LEN(SubModule) > 512;
update UMS_CLAIM set ClaimType = LEFT(ClaimType, 512) where LEN(ClaimType) > 512;
update UMS_CLAIM set ClaimValue = LEFT(ClaimValue, 512) where LEN(ClaimValue) > 512;
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.UMS_CLAIM'), 'Title_En', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE UMS_CLAIM ALTER COLUMN Title_En NVARCHAR(512) NOT NULL;
	Print('UMS_CLAIM.Title_En Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.UMS_CLAIM'), 'Title_Ar', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE UMS_CLAIM ALTER COLUMN Title_Ar NVARCHAR(512) NOT NULL;
	Print('UMS_CLAIM.Title_Ar Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.UMS_CLAIM'), 'Module', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE UMS_CLAIM ALTER COLUMN Module NVARCHAR(512) NOT NULL;
	Print('UMS_CLAIM.Module Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.UMS_CLAIM'), 'SubModule', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE UMS_CLAIM ALTER COLUMN SubModule NVARCHAR(512) NOT NULL;
	Print('UMS_CLAIM.SubModule Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.UMS_CLAIM'), 'ClaimType', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE UMS_CLAIM ALTER COLUMN ClaimType NVARCHAR(512) NOT NULL;
	Print('UMS_CLAIM.ClaimType Altered')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.UMS_CLAIM'), 'ClaimValue', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE UMS_CLAIM ALTER COLUMN ClaimValue NVARCHAR(512) NOT NULL;
	Print('UMS_CLAIM.ClaimValue Altered')
END
----------Muhammad Ali--------------------End----------------------------------


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[WF_WORKFLOW]') AND type in (N'U'))
BEGIN
UPDATE WF_WORKFLOW
SET Name = LEFT(Name, 90)
WHERE LEN(Name) > 90;
 END

 SELECT 
    OBJECT_NAME(object_id) AS TableName,
    name AS ConstraintName
FROM 
    sys.key_constraints
WHERE 
    OBJECT_NAME(parent_object_id) = 'WF_WORKFLOW';

ALTER TABLE WF_WORKFLOW DROP CONSTRAINT UQ__WF_WORKF__737584F6D33359C4;

 IF COLUMNPROPERTY(OBJECT_ID('dbo.WF_WORKFLOW'), 'Name', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE WF_WORKFLOW ALTER COLUMN [Name] NVARCHAR(150);
END
ALTER TABLE WF_WORKFLOW ADD CONSTRAINT UQ__WF_WORKF__737584F6D33359C4 UNIQUE ([Name]);



IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[WF_WORKFLOW]') AND type in (N'U'))
BEGIN
UPDATE WF_WORKFLOW
SET Description = LEFT(Description, 1000)
WHERE LEN(Description) > 1000;
 END
 IF COLUMNPROPERTY(OBJECT_ID('dbo.WF_WORKFLOW'), 'Description', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE WF_WORKFLOW ALTER COLUMN [Description] NVARCHAR(1024);
END

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[NOTIF_NOTIFICATION_EVENT]') AND type in (N'U'))
BEGIN
UPDATE NOTIF_NOTIFICATION_EVENT
SET NameEn = LEFT(NameEn, 90)
WHERE LEN(NameEn) > 90
 END
 IF COLUMNPROPERTY(OBJECT_ID('dbo.NOTIF_NOTIFICATION_EVENT'), 'NameEn', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE NOTIF_NOTIFICATION_EVENT ALTER COLUMN NameEn NVARCHAR(150);
END

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[NOTIF_NOTIFICATION_EVENT]') AND type in (N'U'))
BEGIN
UPDATE NOTIF_NOTIFICATION_EVENT
SET NameAr = LEFT(NameAr, 90)
WHERE LEN(NameAr) > 90
 END
 IF COLUMNPROPERTY(OBJECT_ID('dbo.NOTIF_NOTIFICATION_EVENT'), 'NameAr', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE NOTIF_NOTIFICATION_EVENT ALTER COLUMN NameAr NVARCHAR(150);
END



IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[NOTIF_NOTIFICATION_EVENT]') AND type in (N'U'))
BEGIN
UPDATE NOTIF_NOTIFICATION_EVENT
SET DescriptionEn = LEFT(DescriptionEn, 300)
WHERE LEN(DescriptionEn) > 300
 END
 IF COLUMNPROPERTY(OBJECT_ID('dbo.NOTIF_NOTIFICATION_EVENT'), 'DescriptionEn', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE NOTIF_NOTIFICATION_EVENT ALTER COLUMN DescriptionEn NVARCHAR(512);
END


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[NOTIF_NOTIFICATION_EVENT]') AND type in (N'U'))
BEGIN
UPDATE NOTIF_NOTIFICATION_EVENT
SET DescriptionAr = LEFT(DescriptionAr, 300)
WHERE LEN(DescriptionAr) > 300
 END
 IF COLUMNPROPERTY(OBJECT_ID('dbo.NOTIF_NOTIFICATION_EVENT'), 'DescriptionAr', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE NOTIF_NOTIFICATION_EVENT ALTER COLUMN DescriptionAr NVARCHAR(512);
END


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_HEARING]') AND type in (N'U'))
BEGIN
UPDATE CMS_HEARING
SET Description = LEFT(Description, 1000)
WHERE LEN(Description) > 1000
 END
 IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_HEARING'), 'Description', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE CMS_HEARING ALTER COLUMN Description NVARCHAR(1024);
END

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_OUTCOME_HEARING]') AND type in (N'U'))
BEGIN
UPDATE CMS_OUTCOME_HEARING
SET Remarks = LEFT(Remarks, 1000)
WHERE LEN(Remarks) > 1000
 END
 IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_OUTCOME_HEARING'), 'Remarks', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE CMS_OUTCOME_HEARING ALTER COLUMN Remarks NVARCHAR(1024);
END

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_JUDGEMENT]') AND type in (N'U'))
BEGIN
UPDATE CMS_JUDGEMENT
SET Remarks = LEFT(Remarks, 1000)
WHERE LEN(Remarks) > 1000
 END
 IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_JUDGEMENT'), 'Remarks', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE CMS_JUDGEMENT ALTER COLUMN Remarks NVARCHAR(1024);
END

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_MOJ_EXECUTION_REQUEST]') AND type in (N'U'))
BEGIN
UPDATE CMS_MOJ_EXECUTION_REQUEST
SET Remarks = LEFT(Remarks, 1000)
WHERE LEN(Remarks) > 1000
 END
 IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_MOJ_EXECUTION_REQUEST'), 'Remarks', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE CMS_MOJ_EXECUTION_REQUEST ALTER COLUMN Remarks NVARCHAR(1024);
END

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_EXECUTION]') AND type in (N'U'))
BEGIN
UPDATE CMS_EXECUTION
SET ExecutionFileNumber = LEFT(ExecutionFileNumber, 100)
WHERE LEN(ExecutionFileNumber) > 100
 END
 IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_EXECUTION'), 'ExecutionFileNumber', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE CMS_EXECUTION ALTER COLUMN ExecutionFileNumber NVARCHAR(150);
END



IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TSK_TASK]') AND type in (N'U'))
BEGIN
UPDATE TSK_TASK
SET Name = LEFT(Name, 55)
WHERE LEN(Name) > 55
 END
 IF COLUMNPROPERTY(OBJECT_ID('dbo.TSK_TASK'), 'Name', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE TSK_TASK ALTER COLUMN Name NVARCHAR(150);
END

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TSK_TASK]') AND type in (N'U'))
BEGIN
UPDATE TSK_TASK
SET Url = LEFT(Url, 1000)
WHERE LEN(Url) > 1000
 END
 IF COLUMNPROPERTY(OBJECT_ID('dbo.TSK_TASK'), 'Url', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE TSK_TASK ALTER COLUMN Url NVARCHAR(1024);
END

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TSK_TASK]') AND type in (N'U'))
BEGIN
UPDATE TSK_TASK
SET Description = LEFT(Description, 1000)
WHERE LEN(Description) > 1000
 END
 IF COLUMNPROPERTY(OBJECT_ID('dbo.TSK_TASK'), 'Description', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE TSK_TASK ALTER COLUMN Description NVARCHAR(1024);
END


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TSK_TASK_ACTION]') AND type in (N'U'))
BEGIN
UPDATE TSK_TASK_ACTION
SET ActionName = LEFT(ActionName, 150)
WHERE LEN(ActionName) > 150
 END
 IF COLUMNPROPERTY(OBJECT_ID('dbo.TSK_TASK_ACTION'), 'ActionName', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE TSK_TASK_ACTION ALTER COLUMN ActionName NVARCHAR(512);
END