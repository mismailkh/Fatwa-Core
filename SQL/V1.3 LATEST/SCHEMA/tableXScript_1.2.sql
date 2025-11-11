IF COLUMNPROPERTY(OBJECT_ID('dbo.ATTACHMENT_TYPE'), 'IsMandatory', 'ColumnId') IS NULL
BEGIN 
ALTER TABLE ATTACHMENT_TYPE
	ADD IsMandatory bit NOT NULL default 0
	Print('ATTACHMENT_TYPE.IsMandatory Added')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.TEMP_ATTACHEMENTS'), 'OtherAttachmentType', 'ColumnId') IS NULL
BEGIN 
ALTER TABLE TEMP_ATTACHEMENTS
	 ADD OtherAttachmentType NVARCHAR(500)
	Print('TEMP_ATTACHEMENTS.OtherAttachmentType Added')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.TEMP_ATTACHEMENTS'), 'Description', 'ColumnId') IS NULL
BEGIN 
ALTER TABLE TEMP_ATTACHEMENTS
	 ADD Description NVARCHAR(MAX)
	Print('TEMP_ATTACHEMENTS.Description Added')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.TEMP_ATTACHEMENTS'), 'ReferenceNo', 'ColumnId') IS NULL
BEGIN 
ALTER TABLE TEMP_ATTACHEMENTS
	 ADD ReferenceNo NVARCHAR(50)
	Print('TEMP_ATTACHEMENTS.ReferenceNo Added')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.TEMP_ATTACHEMENTS'), 'ReferenceDate', 'ColumnId') IS NULL
BEGIN 
ALTER TABLE TEMP_ATTACHEMENTS
	 ADD ReferenceDate Date
	Print('TEMP_ATTACHEMENTS.ReferenceDate Added')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.TEMP_ATTACHEMENTS'), 'DocumentDate', 'ColumnId') IS NULL
BEGIN 
ALTER TABLE TEMP_ATTACHEMENTS
	 ADD DocumentDate Date
	Print('TEMP_ATTACHEMENTS.DocumentDate Added')
END

---Rename  UploadedDocument 
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LMS_LITERATURE_UPLOADED_DOCUMENT]') AND type in (N'U')) 
	EXEC sp_rename 'dbo.LMS_LITERATURE_UPLOADED_DOCUMENT', 'UPLOADED_DOCUMENT'  
	Print '[dbo].[LMS_LITERATURE_UPLOADED_DOCUMENT] Renamed Successfully' 
GO  

	
--UPLOADED_DOCUMENT
IF COLUMNPROPERTY(OBJECT_ID('dbo.UPLOADED_DOCUMENT'), 'Description_En', 'ColumnId') IS NOT NULL
BEGIN 
ALTER TABLE UPLOADED_DOCUMENT DROP COLUMN Description_En
	Print('UPLOADED_DOCUMENT.Description_En Dropped')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.UPLOADED_DOCUMENT'), 'ReferenceNo', 'ColumnId') IS NULL
BEGIN 
ALTER TABLE UPLOADED_DOCUMENT
	 ADD ReferenceNo NVARCHAR(50)
	Print('UPLOADED_DOCUMENT.ReferenceNo Added')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.UPLOADED_DOCUMENT'), 'ReferenceDate', 'ColumnId') IS NULL
BEGIN 
ALTER TABLE UPLOADED_DOCUMENT
	 ADD ReferenceDate Date
	Print('UPLOADED_DOCUMENT.ReferenceDate Added')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.UPLOADED_DOCUMENT'), 'OtherAttachmentType', 'ColumnId') IS NULL
BEGIN 
ALTER TABLE UPLOADED_DOCUMENT
	 ADD OtherAttachmentType NVARCHAR(500)
	Print('UPLOADED_DOCUMENT.OtherAttachmentType Added')
END

 
IF COLUMNPROPERTY(OBJECT_ID('dbo.UPLOADED_DOCUMENT'), 'Description_Ar', 'ColumnId') IS NOT NULL
BEGIN
EXEC sp_rename 'UPLOADED_DOCUMENT.Description_Ar', 'Description', 'COLUMN' 
    Print '[dbo].[UPLOADED_DOCUMENT.Description_Ar] Renamed Successfully'
END
GO

IF COLUMNPROPERTY(OBJECT_ID('dbo.UPLOADED_DOCUMENT'), 'DocDateTime', 'ColumnId') IS NOT NULL
BEGIN
EXEC sp_rename 'UPLOADED_DOCUMENT.DocDateTime', 'DocumentDate', 'COLUMN' 
    Print '[dbo].[UPLOADED_DOCUMENT.DocDateTime] Renamed Successfully'
END
GO

IF COLUMNPROPERTY(OBJECT_ID('dbo.UPLOADED_DOCUMENT'), 'DocumentDate', 'ColumnId') IS NOT NULL
BEGIN
	ALTER TABLE UPLOADED_DOCUMENT ALTER COLUMN DocumentDate Date
    Print '[dbo].[UPLOADED_DOCUMENT.DocumentDate] Altered Successfully'
END
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_REQUEST_RESPONSE]') AND type in (N'U')) 
	DROP TABLE CMS_REQUEST_RESPONSE 
	Print '[dbo].[CMS_REQUEST_RESPONSE] Dropped Successfully' 
GO  

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_CASE_REQUESTS]') AND type in (N'U')) 
	DROP TABLE CMS_CASE_REQUESTS 
	Print '[dbo].[CMS_CASE_REQUESTS] Dropped Successfully' 
GO  

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_RESPONSE_REASON]') AND type in (N'U')) 
	DROP TABLE CMS_RESPONSE_REASON 
	Print '[dbo].[CMS_RESPONSE_REASON] Dropped Successfully' 
GO  

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_RESPONSE_TYPE]') AND type in (N'U')) 
	DROP TABLE CMS_RESPONSE_TYPE 
	Print '[dbo].[CMS_RESPONSE_TYPE] Dropped Successfully' 
GO  

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_GOVERNMENT_ENTITY]') AND type in (N'U')) 
	DROP TABLE CMS_GOVERNMENT_ENTITY 
	Print '[dbo].[CMS_GOVERNMENT_ENTITY] Dropped Successfully' 
GO  


------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------
--<History Author = 'Hassan Abbas' Date='2022-10-26' Version="1.0" Branch="master"> ABOVE SCRIPT EXECUTED ON FATWA_DB_DEV</History>
------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------


/****** Object:  Table [dbo].[CMS_CASE_PARTIES_LINK]    Script Date: 26/10/2022 9:33:54 am ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_CASE_PARTIES_LINK]') AND type in (N'U'))
DROP TABLE CMS_CASE_PARTIES_LINK
GO

/****** Object:  Table [dbo].[CMS_CASE_PARTIES_CATEGORY]    Script Date: 26/10/2022 9:33:54 am ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_CASE_PARTIES_CATEGORY]') AND type in (N'U'))
DROP TABLE CMS_CASE_PARTIES_CATEGORY
GO

--CMS_CASE_REQUEST

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_CASE_REQUEST'), 'ReceivedBy', 'ColumnId') IS NULL
BEGIN 
ALTER TABLE CMS_CASE_REQUEST
	ADD ReceivedBy nvarchar(256)
	Print('CMS_CASE_REQUEST.ReceivedBy Added')
END

--CMS_CASE_REQUEST

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_CASE_REQUEST'), 'ReceivedDate', 'ColumnId') IS NULL
BEGIN 
ALTER TABLE CMS_CASE_REQUEST
	ADD ReceivedDate Datetime
	Print('CMS_CASE_REQUEST.ReceivedDate Added')
END

--CMS_CASE_REQUEST

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_CASE_REQUEST'), 'ReviewedBy', 'ColumnId') IS NULL
BEGIN 
ALTER TABLE CMS_CASE_REQUEST
	ADD ReviewedBy nvarchar(256)
	Print('CMS_CASE_REQUEST.ReviewedBy Added')
END

--CMS_CASE_REQUEST

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_CASE_REQUEST'), 'ReviewedDate', 'ColumnId') IS NULL
BEGIN 
ALTER TABLE CMS_CASE_REQUEST
	ADD ReviewedDate Datetime
	Print('CMS_CASE_REQUEST.ReviewedDate Added')
END

--CMS_CASE_REQUEST

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_CASE_REQUEST'), 'ApprovedBy', 'ColumnId') IS NULL
BEGIN 
ALTER TABLE CMS_CASE_REQUEST
	ADD ApprovedBy nvarchar(256)
	Print('CMS_CASE_REQUEST.ApprovedBy Added')
END

--CMS_CASE_REQUEST

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_CASE_REQUEST'), 'TaskStatusId', 'ColumnId') IS NULL
BEGIN 
ALTER TABLE CMS_CASE_REQUEST
	ADD ApprovedDate int
	Print('CMS_CASE_REQUEST.TaskStatusId Added')
END

--CMS_CASE_REQUEST

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_CASE_REQUEST'), 'ApprovedDate', 'ColumnId') IS NULL
BEGIN 
ALTER TABLE CMS_CASE_REQUEST
	ADD ApprovedDate Datetime
	Print('CMS_CASE_REQUEST.ApprovedDate Added')
END

--CMS_CASE_FILE

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_CASE_FILE'), 'FileName', 'ColumnId') IS NULL
BEGIN 
ALTER TABLE CMS_CASE_FILE
	ADD FileName nvarchar(1000) NOT NULL DEFAULT ''
	Print('CMS_CASE_FILE.FileName Added')
END


ALTER TABLE CMS_CASE_PARTY_LINK DROP CONSTRAINT [CMS_CASE_PARTY_LINK_MINISTRY]


---Rename  CasePartyLink.MinistryId
IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_CASE_PARTY_LINK'), 'MinistryId', 'ColumnId') IS NOT NULL
EXEC sp_rename 'CMS_CASE_PARTY_LINK.MinistryId', 'EntityId', 'COLUMN' 
    Print '[dbo].[CMS_CASE_PARTY_LINK.EntityId] Renamed Successfully'
GO  

ALTER TABLE CMS_CASE_PARTY_LINK ADD CONSTRAINT CMS_CASE_PARTY_LINK_GOVT_ENTITY FOREIGN KEY(EntityId)
REFERENCES CMS_GOVERNMENT_ENTITY_G2G_LKP (EntityId)


---Rename  CasePartyLink.RequestId
IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_CASE_PARTY_LINK'), 'RequestId', 'ColumnId') IS NOT NULL
EXEC sp_rename 'CMS_CASE_PARTY_LINK.RequestId', 'ReferenceGuid', 'COLUMN' 
    Print '[dbo].[CMS_CASE_PARTY_LINK.RequestId] Renamed Successfully'
GO  


--CMS_CASE_PARTY_LINK

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_CASE_PARTY_LINK'), 'PACINumber', 'ColumnId') IS NULL
BEGIN 
ALTER TABLE CMS_CASE_PARTY_LINK
	ADD PACINumber NVARCHAR(1000)
	Print('CMS_CASE_PARTY_LINK.PACINumber Added')
END

--CMS_CASE_PARTY_LINK

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_CASE_PARTY_LINK'), 'Address', 'ColumnId') IS NULL
BEGIN 
ALTER TABLE CMS_CASE_PARTY_LINK
	ADD Address NVARCHAR(MAX)
	Print('CMS_CASE_PARTY_LINK.Address Added')
END

--CMS_CASE_PARTY_LINK

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_CASE_PARTY_LINK'), 'CompanyCivilId', 'ColumnId') IS NULL
BEGIN 
ALTER TABLE CMS_CASE_PARTY_LINK
	ADD CompanyCivilId NVARCHAR(1000)
	Print('CMS_CASE_PARTY_LINK.CompanyCivilId Added')
END

--CMS_CASE_PARTY_LINK

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_CASE_PARTY_LINK'), 'MOCIFileNumber', 'ColumnId') IS NULL
BEGIN 
ALTER TABLE CMS_CASE_PARTY_LINK
	ADD MOCIFileNumber NVARCHAR(1000)
	Print('CMS_CASE_PARTY_LINK.MOCIFileNumber Added')
END

--CMS_CASE_PARTY_LINK

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_CASE_PARTY_LINK'), 'LicenseNumber', 'ColumnId') IS NULL
BEGIN 
ALTER TABLE CMS_CASE_PARTY_LINK
	ADD LicenseNumber NVARCHAR(1000)
	Print('CMS_CASE_PARTY_LINK.LicenseNumber Added')
END

--CMS_CASE_PARTY_LINK

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_CASE_PARTY_LINK'), 'MembershipNumber', 'ColumnId') IS NULL
BEGIN 
ALTER TABLE CMS_CASE_PARTY_LINK
	ADD MembershipNumber NVARCHAR(1000)
	Print('CMS_CASE_PARTY_LINK.MembershipNumber Added')
END


IF COLUMNPROPERTY(OBJECT_ID('dbo.ATTACHMENT_TYPE'), 'IsOfficialLetter', 'ColumnId') IS NULL
BEGIN 
ALTER TABLE ATTACHMENT_TYPE
	 ADD IsOfficialLetter bit NOT NULL DEFAULT 0
	Print('TEMP_ATTACHEMENTS.IsOfficialLetter Added')
END


------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------
--<History Author = 'Hassan Abbas' Date='2022-11-03' Version="1.0" Branch="master"> ABOVE SCRIPT EXECUTED ON FATWA_DB_DEV</History>
------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------

--CMS_TEMPLATE
IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_TEMPLATE'), 'AttachmentTypeId', 'ColumnId') IS NULL
BEGIN 
	ALTER TABLE CMS_TEMPLATE ADD AttachmentTypeId INT
	Print('CMS_TEMPLATE.AttachmentTypeId Added')
END


IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_CASE_REQUEST_LAWYER_ASSIGNMENT'), 'Reason', 'ColumnId') IS NOT NULL
BEGIN
EXEC sp_rename 'CMS_CASE_REQUEST_LAWYER_ASSIGNMENT.Reason', 'Remarks', 'COLUMN' 
    Print '[dbo].[CMS_CASE_REQUEST_LAWYER_ASSIGNMENT.Reason] Renamed Successfully'
END
GO


IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_CASE_FILE'), 'FileNumber', 'ColumnId') IS NOT NULL
BEGIN
ALTER TABLE CMS_CASE_FILE ALTER COLUMN FileNumber INT NOT NULL
    Print '[dbo].[CMS_CASE_FILE.FileNumber] Altered Successfully'
END
GO


---Rename  CMS_CASE_REQUEST_LAWYER_ASSIGNMENT 
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_CASE_REQUEST_LAWYER_ASSIGNMENT]') AND type in (N'U')) 
	EXEC sp_rename 'dbo.CMS_CASE_REQUEST_LAWYER_ASSIGNMENT', 'CMS_CASE_FILE_ASSIGNMENT'  
	Print '[dbo].[CMS_CASE_REQUEST_LAWYER_ASSIGNMENT] Renamed Successfully' 
GO  


IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_CASE_FILE_ASSIGNMENT'), 'RequestId', 'ColumnId') IS NOT NULL
BEGIN
EXEC sp_rename 'CMS_CASE_FILE_ASSIGNMENT.RequestId', 'FileId', 'COLUMN' 
    Print '[dbo].[CMS_CASE_FILE_ASSIGNMENT.Description_Ar] Renamed Successfully'
END
GO


IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_CASE_FILE_ASSIGNMENT_HISTORY'), 'AssignorId', 'ColumnId') IS NOT NULL
BEGIN
EXEC sp_rename 'CMS_CASE_FILE_ASSIGNMENT_HISTORY.AssignorId', 'Remarks', 'COLUMN' 
    Print '[dbo].[CMS_CASE_FILE_ASSIGNMENT_HISTORY.AssignorId] Renamed Successfully'
END
GO


IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_CASE_FILE_ASSIGNMENT_HISTORY'), 'Remarks', 'ColumnId') IS NOT NULL
BEGIN
ALTER TABLE CMS_CASE_FILE_ASSIGNMENT_HISTORY ALTER COLUMN Remarks NVARCHAR(MAX)
    Print '[dbo].[CMS_CASE_FILE_ASSIGNMENT_HISTORY.Remarks] Altered Successfully'
END
GO


ALTER TABLE CMS_CASE_FILE_ASSIGNMENT DROP CONSTRAINT CMS_LAWYER_ASSIGN_CASE_RQST

delete from CMS_CASE_FILE_ASSIGNMENT

ALTER TABLE CMS_CASE_FILE_ASSIGNMENT ADD CONSTRAINT CMS_ASSIGNMENT_CASE_FILE FOREIGN KEY(FileId)
REFERENCES CMS_CASE_FILE(FileId)


IF COLUMNPROPERTY(OBJECT_ID('dbo.PARAMETER'), 'IsAutoPopulated', 'ColumnId') IS NULL
BEGIN 
ALTER TABLE PARAMETER ADD IsAutoPopulated bit NOT NULL default 0
	Print('PARAMETER.IsAutoPopulated Added')
END





------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------
--<History Author = 'Hassan Abbas' Date='2022-11-21' Version="1.0" Branch="master"> ABOVE SCRIPT EXECUTED ON FATWA_DB_DEV</History>
------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------


IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_DRAFTED_TEMPLATE_SECTION'), 'SequenceNumber', 'ColumnId') IS NULL
BEGIN 
ALTER TABLE CMS_DRAFTED_TEMPLATE_SECTION
	 ADD SequenceNumber int NULL
	Print('CMS_DRAFTED_TEMPLATE_SECTION.SequenceNumber Added')
END


------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------
--<History Author = 'Hassan Abbas' Date='2022-11-25' Version="1.0" Branch="master"> ABOVE SCRIPT EXECUTED ON FATWA_DB_DEV</History>
------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------


---Rename  CMS_CASE_FILE_ASSIGNMENT.FileId
IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_CASE_FILE_ASSIGNMENT'), 'FileId', 'ColumnId') IS NOT NULL
EXEC sp_rename 'CMS_CASE_FILE_ASSIGNMENT.FileId', 'ReferenceId', 'COLUMN' 
    Print '[dbo].[CMS_CASE_FILE_ASSIGNMENT.FileId] Renamed Successfully'
GO  
---Rename  CMS_CASE_FILE_ASSIGNMENT_HISTORY.FileId
IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_CASE_FILE_ASSIGNMENT_HISTORY'), 'FileId', 'ColumnId') IS NOT NULL
EXEC sp_rename 'CMS_CASE_FILE_ASSIGNMENT_HISTORY.FileId', 'ReferenceId', 'COLUMN' 
    Print '[dbo].[CMS_CASE_FILE_ASSIGNMENT_HISTORY.FileId] Renamed Successfully'
GO  

---Rename  CMS_CASE_FILE_ASSIGNMENT 
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_CASE_FILE_ASSIGNMENT]') AND type in (N'U')) 
	EXEC sp_rename 'dbo.CMS_CASE_FILE_ASSIGNMENT', 'CMS_CASE_ASSIGNMENT'  
	Print '[dbo].[CMS_CASE_FILE_ASSIGNMENT] Renamed Successfully' 
GO  
---Rename  CMS_CASE_FILE_ASSIGNMENT_HISTORY 
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_CASE_FILE_ASSIGNMENT_HISTORY]') AND type in (N'U')) 
	EXEC sp_rename 'dbo.CMS_CASE_FILE_ASSIGNMENT_HISTORY', 'CMS_CASE_ASSIGNMENT_HISTORY'  
	Print '[dbo].[CMS_CASE_FILE_ASSIGNMENT_HISTORY] Renamed Successfully' 
GO  


ALTER TABLE MODULE_ACTIVITY_PARAMETERS ADD CONSTRAINT AK_ACTIVITY_PARAMETER UNIQUE(ActivityId,ParameterId)
ALTER TABLE WORKFLOW_ACTIVITY_PARAMETERS ADD CONSTRAINT AK_WORKFLOW_ACTIVITY_PARAMETER UNIQUE(WorkflowActivityId,ParameterId)


IF COLUMNPROPERTY(OBJECT_ID('dbo.UPLOADED_DOCUMENT'), 'Description', 'ColumnId') IS NOT NULL
BEGIN
	ALTER TABLE UPLOADED_DOCUMENT ALTER COLUMN Description NVARCHAR(500) NULL
    Print '[dbo].[UPLOADED_DOCUMENT.Description] Altered Successfully'
END
GO


IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_REGISTERED_CASE'), 'IsPrimary', 'ColumnId') IS NULL
BEGIN 
ALTER TABLE CMS_REGISTERED_CASE
	 ADD IsPrimary bit NULL
	Print('CMS_REGISTERED_CASE.IsPrimary Added')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_REGISTERED_CASE'), 'IsDissolved', 'ColumnId') IS NULL
BEGIN 
ALTER TABLE CMS_REGISTERED_CASE
	 ADD IsDissolved bit NULL
	Print('CMS_REGISTERED_CASE.IsDissolved Added')
END
GO

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_REGISTERED_CASE'), 'CaseAmount', 'ColumnId') IS  NOT NULL
BEGIN    
ALTER TABLE CMS_REGISTERED_CASE 
	ALTER COLUMN CaseAmount decimal(10,2)    
	Print('CMS_REGISTERED_CASE.CaseAmount Altered')
END
GO

ALTER TABLE CMS_CASE_ASSIGNMENT DROP CONSTRAINT CMS_ASSIGNMENT_CASE_FILE

------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------
--<History Author = 'Hassan Abbas' Date='2022-12-05' Version="1.0" Branch="master"> ABOVE SCRIPT EXECUTED ON FATWA_DB_DEV</History>
------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_CASE_FILE'), 'ShortNumber', 'ColumnId') IS NULL
BEGIN 
ALTER TABLE CMS_CASE_FILE ADD ShortNumber INT NOT NULL DEFAULT 0
	Print('CMS_CASE_FILE.ShortNumberShort Added')
END
GO

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_CASE_FILE'), 'FileNumber', 'ColumnId') IS NOT NULL
BEGIN
	ALTER TABLE CMS_CASE_FILE ALTER COLUMN FileNumber NVARCHAR(1000) NOT NULL
    Print '[dbo].[CMS_CASE_FILE.FileNumber] Altered Successfully'
END
GO


IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_CASE_REQUEST'), 'IsLinked', 'ColumnId') IS NULL
BEGIN 
	ALTER TABLE CMS_CASE_REQUEST ADD IsLinked bit NOT NULL DEFAULT 0
	Print('CMS_CASE_REQUEST.IsLinked Added')
END
GO


IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_CASE_REQUEST'), 'IsPrimary', 'ColumnId') IS NULL
BEGIN 
	ALTER TABLE CMS_CASE_REQUEST ADD IsPrimary bit NOT NULL DEFAULT 0
	Print('CMS_CASE_REQUEST.IsPrimary Added')
END
GO


IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_CASE_FILE'), 'IsLinked', 'ColumnId') IS NULL
BEGIN 
	ALTER TABLE CMS_CASE_FILE ADD IsLinked bit NOT NULL DEFAULT 0
	Print('CMS_CASE_FILE.IsLinked Added')
END
GO


IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_CASE_FILE'), 'IsPrimary', 'ColumnId') IS NULL
BEGIN 
	ALTER TABLE CMS_CASE_FILE ADD IsPrimary bit NOT NULL DEFAULT 0
	Print('CMS_CASE_FILE.IsPrimary Added')
END
GO


UPDATE TEMP_ATTACHEMENTS SET AttachmentTypeId = '7' WHERE AttachmentTypeId = '22'
UPDATE UPLOADED_DOCUMENT SET AttachmentTypeId = '7' WHERE AttachmentTypeId = '22'
DELETE FROM ATTACHMENT_TYPE WHERE ATTACHMENTtypeid = '22'
ALTER TABLE ATTACHMENT_TYPE ADD CONSTRAINT UN_TYPE_EN UNIQUE(Type_En)
ALTER TABLE ATTACHMENT_TYPE ADD CONSTRAINT UN_TYPE_AR UNIQUE(Type_Ar)


IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_REGISTERED_CASE'), 'OldCANNumber', 'ColumnId') IS NULL
BEGIN 
	ALTER TABLE CMS_REGISTERED_CASE ADD OldCANNumber NVARCHAR(1000)
	Print('CMS_REGISTERED_CASE.OldCANNumber Added')
END
GO



IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_CASE_REQUEST'), 'RequestTypeId', 'ColumnId') IS NULL
BEGIN 
ALTER TABLE CMS_CASE_REQUEST
	ADD RequestTypeId INT NOT NULL DEFAULT 1
	Print('CMS_CASE_REQUEST.RequestTypeId Added')
END



ALTER TABLE CMS_CASE_REQUEST ADD CONSTRAINT CMS_CASE_REQUEST_REQUEST_TYPE FOREIGN KEY(RequestTypeId)
REFERENCES CMS_REQUEST_TYPE_G2G_LKP (Id)


IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_SUBTYPE_G2G_LKP'), 'CreatedBy', 'ColumnId') IS NOT NULL
BEGIN 
ALTER TABLE CMS_SUBTYPE_G2G_LKP DROP COLUMN CreatedBy
	Print('CMS_SUBTYPE_G2G_LKP.CreatedBy Dropped')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_SUBTYPE_G2G_LKP'), 'ModifiedBy', 'ColumnId') IS NOT NULL
BEGIN 
ALTER TABLE CMS_SUBTYPE_G2G_LKP DROP COLUMN ModifiedBy
	Print('CMS_SUBTYPE_G2G_LKP.ModifiedBy Dropped')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_SUBTYPE_G2G_LKP'), 'DeletedBy', 'ColumnId') IS NOT NULL
BEGIN 
ALTER TABLE CMS_SUBTYPE_G2G_LKP DROP COLUMN DeletedBy
	Print('CMS_SUBTYPE_G2G_LKP.DeletedBy Dropped')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_SUBTYPE_G2G_LKP'), 'CreatedDate', 'ColumnId') IS NOT NULL
BEGIN 
ALTER TABLE CMS_SUBTYPE_G2G_LKP DROP COLUMN CreatedDate
	Print('CMS_SUBTYPE_G2G_LKP.CreatedDate Dropped')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_SUBTYPE_G2G_LKP'), 'ModifiedDate', 'ColumnId') IS NOT NULL
BEGIN 
ALTER TABLE CMS_SUBTYPE_G2G_LKP DROP COLUMN ModifiedDate
	Print('CMS_SUBTYPE_G2G_LKP.ModifiedDate Dropped')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_SUBTYPE_G2G_LKP'), 'DeletedDate', 'ColumnId') IS NOT NULL
BEGIN 
ALTER TABLE CMS_SUBTYPE_G2G_LKP DROP COLUMN DeletedDate
	Print('CMS_SUBTYPE_G2G_LKP.DeletedDate Dropped')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_SUBTYPE_G2G_LKP'), 'IsDeleted', 'ColumnId') IS NOT NULL
BEGIN 
ALTER TABLE CMS_SUBTYPE_G2G_LKP DROP COLUMN IsDeleted
	Print('CMS_SUBTYPE_G2G_LKP.IsDeleted Dropped')
END


IF COLUMNPROPERTY(OBJECT_ID('dbo.[CMS_OPERATING_SECTOR_TYPE_G2G_LKP]'), 'CreatedBy', 'ColumnId') IS NOT NULL
BEGIN 
ALTER TABLE [CMS_OPERATING_SECTOR_TYPE_G2G_LKP] DROP COLUMN CreatedBy
	Print('[CMS_OPERATING_SECTOR_TYPE_G2G_LKP].CreatedBy Dropped')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.[CMS_OPERATING_SECTOR_TYPE_G2G_LKP]'), 'ModifiedBy', 'ColumnId') IS NOT NULL
BEGIN 
ALTER TABLE [CMS_OPERATING_SECTOR_TYPE_G2G_LKP] DROP COLUMN ModifiedBy
	Print('[CMS_OPERATING_SECTOR_TYPE_G2G_LKP].ModifiedBy Dropped')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.[CMS_OPERATING_SECTOR_TYPE_G2G_LKP]'), 'DeletedBy', 'ColumnId') IS NOT NULL
BEGIN 
ALTER TABLE [CMS_OPERATING_SECTOR_TYPE_G2G_LKP] DROP COLUMN DeletedBy
	Print('[CMS_OPERATING_SECTOR_TYPE_G2G_LKP].DeletedBy Dropped')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.[CMS_OPERATING_SECTOR_TYPE_G2G_LKP]'), 'CreatedDate', 'ColumnId') IS NOT NULL
BEGIN 
ALTER TABLE [CMS_OPERATING_SECTOR_TYPE_G2G_LKP] DROP COLUMN CreatedDate
	Print('[CMS_OPERATING_SECTOR_TYPE_G2G_LKP].CreatedDate Dropped')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.[CMS_OPERATING_SECTOR_TYPE_G2G_LKP]'), 'ModifiedDate', 'ColumnId') IS NOT NULL
BEGIN 
ALTER TABLE [CMS_OPERATING_SECTOR_TYPE_G2G_LKP] DROP COLUMN ModifiedDate
	Print('[CMS_OPERATING_SECTOR_TYPE_G2G_LKP].ModifiedDate Dropped')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.[CMS_OPERATING_SECTOR_TYPE_G2G_LKP]'), 'DeletedDate', 'ColumnId') IS NOT NULL
BEGIN 
ALTER TABLE [CMS_OPERATING_SECTOR_TYPE_G2G_LKP] DROP COLUMN DeletedDate
	Print('[CMS_OPERATING_SECTOR_TYPE_G2G_LKP].DeletedDate Dropped')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.[CMS_OPERATING_SECTOR_TYPE_G2G_LKP]'), 'IsDeleted', 'ColumnId') IS NOT NULL
BEGIN 
ALTER TABLE [CMS_OPERATING_SECTOR_TYPE_G2G_LKP] DROP COLUMN IsDeleted
	Print('[CMS_OPERATING_SECTOR_TYPE_G2G_LKP].IsDeleted Dropped')
END


IF COLUMNPROPERTY(OBJECT_ID('dbo.[CMS_CASE_PARTY_TYPE_G2G_LKP]'), 'CreatedBy', 'ColumnId') IS NOT NULL
BEGIN 
ALTER TABLE [CMS_CASE_PARTY_TYPE_G2G_LKP] DROP COLUMN CreatedBy
	Print('[CMS_CASE_PARTY_TYPE_G2G_LKP].CreatedBy Dropped')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.[CMS_CASE_PARTY_TYPE_G2G_LKP]'), 'ModifiedBy', 'ColumnId') IS NOT NULL
BEGIN 
ALTER TABLE [CMS_CASE_PARTY_TYPE_G2G_LKP] DROP COLUMN ModifiedBy
	Print('[CMS_CASE_PARTY_TYPE_G2G_LKP].ModifiedBy Dropped')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.[CMS_CASE_PARTY_TYPE_G2G_LKP]'), 'DeletedBy', 'ColumnId') IS NOT NULL
BEGIN 
ALTER TABLE [CMS_CASE_PARTY_TYPE_G2G_LKP] DROP COLUMN DeletedBy
	Print('[CMS_CASE_PARTY_TYPE_G2G_LKP].DeletedBy Dropped')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.[CMS_CASE_PARTY_TYPE_G2G_LKP]'), 'CreatedDate', 'ColumnId') IS NOT NULL
BEGIN 
ALTER TABLE [CMS_CASE_PARTY_TYPE_G2G_LKP] DROP COLUMN CreatedDate
	Print('[CMS_CASE_PARTY_TYPE_G2G_LKP].CreatedDate Dropped')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.[CMS_CASE_PARTY_TYPE_G2G_LKP]'), 'ModifiedDate', 'ColumnId') IS NOT NULL
BEGIN 
ALTER TABLE [CMS_CASE_PARTY_TYPE_G2G_LKP] DROP COLUMN ModifiedDate
	Print('[CMS_CASE_PARTY_TYPE_G2G_LKP].ModifiedDate Dropped')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.[CMS_CASE_PARTY_TYPE_G2G_LKP]'), 'DeletedDate', 'ColumnId') IS NOT NULL
BEGIN 
ALTER TABLE [CMS_CASE_PARTY_TYPE_G2G_LKP] DROP COLUMN DeletedDate
	Print('[CMS_CASE_PARTY_TYPE_G2G_LKP].DeletedDate Dropped')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.[CMS_CASE_PARTY_TYPE_G2G_LKP]'), 'IsDeleted', 'ColumnId') IS NOT NULL
BEGIN 
ALTER TABLE [CMS_CASE_PARTY_TYPE_G2G_LKP] DROP COLUMN IsDeleted
	Print('[CMS_CASE_PARTY_TYPE_G2G_LKP].IsDeleted Dropped')
END


IF COLUMNPROPERTY(OBJECT_ID('dbo.[CMS_CASE_PARTY_CATEGORY_G2G_LKP]'), 'CreatedBy', 'ColumnId') IS NOT NULL
BEGIN 
ALTER TABLE [CMS_CASE_PARTY_CATEGORY_G2G_LKP] DROP COLUMN CreatedBy
	Print('[CMS_CASE_PARTY_CATEGORY_G2G_LKP].CreatedBy Dropped')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.[CMS_CASE_PARTY_CATEGORY_G2G_LKP]'), 'ModifiedBy', 'ColumnId') IS NOT NULL
BEGIN 
ALTER TABLE [CMS_CASE_PARTY_CATEGORY_G2G_LKP] DROP COLUMN ModifiedBy
	Print('[CMS_CASE_PARTY_CATEGORY_G2G_LKP].ModifiedBy Dropped')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.[CMS_CASE_PARTY_CATEGORY_G2G_LKP]'), 'DeletedBy', 'ColumnId') IS NOT NULL
BEGIN 
ALTER TABLE [CMS_CASE_PARTY_CATEGORY_G2G_LKP] DROP COLUMN DeletedBy
	Print('[CMS_CASE_PARTY_CATEGORY_G2G_LKP].DeletedBy Dropped')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.[CMS_CASE_PARTY_CATEGORY_G2G_LKP]'), 'CreatedDate', 'ColumnId') IS NOT NULL
BEGIN 
ALTER TABLE [CMS_CASE_PARTY_CATEGORY_G2G_LKP] DROP COLUMN CreatedDate
	Print('[CMS_CASE_PARTY_CATEGORY_G2G_LKP].CreatedDate Dropped')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.[CMS_CASE_PARTY_CATEGORY_G2G_LKP]'), 'ModifiedDate', 'ColumnId') IS NOT NULL
BEGIN 
ALTER TABLE [CMS_CASE_PARTY_CATEGORY_G2G_LKP] DROP COLUMN ModifiedDate
	Print('[CMS_CASE_PARTY_CATEGORY_G2G_LKP].ModifiedDate Dropped')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.[CMS_CASE_PARTY_CATEGORY_G2G_LKP]'), 'DeletedDate', 'ColumnId') IS NOT NULL
BEGIN 
ALTER TABLE [CMS_CASE_PARTY_CATEGORY_G2G_LKP] DROP COLUMN DeletedDate
	Print('[CMS_CASE_PARTY_CATEGORY_G2G_LKP].DeletedDate Dropped')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.[CMS_CASE_PARTY_CATEGORY_G2G_LKP]'), 'IsDeleted', 'ColumnId') IS NOT NULL
BEGIN 
ALTER TABLE [CMS_CASE_PARTY_CATEGORY_G2G_LKP] DROP COLUMN IsDeleted
	Print('[CMS_CASE_PARTY_CATEGORY_G2G_LKP].IsDeleted Dropped')
END

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_MINISTRY_G2G_LKP]') AND type in (N'U')) 
	DROP TABLE CMS_MINISTRY_G2G_LKP 
	Print '[dbo].[CMS_MINISTRY_G2G_LKP] Dropped Successfully' 
GO  

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_SUBTYPE_G2G_LKP'), 'SectorTypeId', 'ColumnId') IS NOT NULL
BEGIN
EXEC sp_rename 'CMS_SUBTYPE_G2G_LKP.SectorTypeId', 'RequestTypeId', 'COLUMN' 
    Print '[dbo].[CMS_SUBTYPE_G2G_LKP.SectorTypeId] Renamed Successfully'
END
GO

-- Add SectorTypeI Column in UMS_USER 
ALTER TABLE UMS_USER 
ADD SectorTypeId int  null
	 CONSTRAINT [UMS_USER_SECTOR_TYPE] FOREIGN KEY([SectorTypeId]) REFERENCES [dbo].[CMS_OPERATING_SECTOR_TYPE_G2G_LKP] ([Id])
	Print('UMS_USER.SectorTypeId Added')

	
IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_CASE_FILE'), 'TransferStatusId', 'ColumnId') IS NULL
BEGIN 
ALTER TABLE CMS_CASE_FILE ADD TransferStatusId INT
	Print('CMS_CASE_FILE.TransferStatusId Added')
END

---Rename  CMS_CASE_REQUEST.TaskStatusId
IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_CASE_REQUEST'), 'TaskStatusId', 'ColumnId') IS NOT NULL
EXEC sp_rename 'CMS_CASE_REQUEST.TaskStatusId', 'TransferStatusId', 'COLUMN' 
    Print '[dbo].[CMS_CASE_REQUEST.TaskStatusId] Renamed Successfully'
GO  


ALTER TABLE UMS_USER DROP CONSTRAINT UMS_USER_SECTOR_TYPE


IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_DRAFTED_TEMPLATE'), 'DraftEntityType', 'ColumnId') IS NULL
BEGIN 
ALTER TABLE CMS_DRAFTED_TEMPLATE ADD DraftEntityType INT
	Print('CMS_DRAFTED_TEMPLATE.DraftEntityType Added')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_DRAFTED_TEMPLATE'), 'Payload', 'ColumnId') IS NULL
BEGIN 
ALTER TABLE CMS_DRAFTED_TEMPLATE ADD Payload NVARCHAR(MAX)
	Print('CMS_DRAFTED_TEMPLATE.Payload Added')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_MOJ_REGISTRATION_REQUEST'), 'SectorTypeId', 'ColumnId') IS NULL
BEGIN 
ALTER TABLE CMS_MOJ_REGISTRATION_REQUEST ADD SectorTypeId INT
	Print('CMS_MOJ_REGISTRATION_REQUEST.SectorTypeId Added')
END
-----------------------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------------------
-------------------------------------------------Consultation Start---------------------------------------------------
-----------------------------------------------------------------------------------------------------------------------------


IF COLUMNPROPERTY(OBJECT_ID('dbo.COMS_CONSULTATION_REQUEST'), 'TransferStatusId', 'ColumnId') IS NULL
BEGIN
ALTER TABLE COMS_CONSULTATION_REQUEST    
ADD TransferStatusId INT   
Print('COMS_CONSULTATION_REQUEST.TransferStatusId Added')
END
---------------------------
IF COLUMNPROPERTY(OBJECT_ID('dbo.COMS_CONSULTATION_REQUEST'), 'ReceivedBy', 'ColumnId') IS NULL
BEGIN
ALTER TABLE COMS_CONSULTATION_REQUEST    
ADD ReceivedBy nvarchar(256)    
Print('COMS_CONSULTATION_REQUEST.ReceivedBy Added')
END
----------------------------
IF COLUMNPROPERTY(OBJECT_ID('dbo.COMS_CONSULTATION_REQUEST'), 'ReceivedDate', 'ColumnId') IS NULL
BEGIN
ALTER TABLE COMS_CONSULTATION_REQUEST    
ADD ReceivedDate datetime    
Print('COMS_CONSULTATION_REQUEST.ReceivedDate Added')
END
---------------------------
IF COLUMNPROPERTY(OBJECT_ID('dbo.COMS_CONSULTATION_REQUEST'), 'ReviewedBy', 'ColumnId') IS NULL
BEGIN
ALTER TABLE COMS_CONSULTATION_REQUEST    
ADD ReviewedBy nvarchar(256)    
Print('COMS_CONSULTATION_REQUEST.ReviewedBy Added')
END
---------------------------

IF COLUMNPROPERTY(OBJECT_ID('dbo.COMS_CONSULTATION_REQUEST'), 'ReviewedDate', 'ColumnId') IS NULL
BEGIN
ALTER TABLE COMS_CONSULTATION_REQUEST    
ADD ReviewedDate datetime    
Print('COMS_CONSULTATION_REQUEST.ReviewedDate Added')
END
---------------------------
IF COLUMNPROPERTY(OBJECT_ID('dbo.COMS_CONSULTATION_REQUEST'), 'ApprovedBy', 'ColumnId') IS NULL
BEGIN
ALTER TABLE COMS_CONSULTATION_REQUEST    
ADD ApprovedBy nvarchar(256)    
Print('COMS_CONSULTATION_REQUEST.ApprovedBy Added')
END
----------------------------
IF COLUMNPROPERTY(OBJECT_ID('dbo.COMS_CONSULTATION_REQUEST'), 'ApprovedDate', 'ColumnId') IS NULL
BEGIN
ALTER TABLE COMS_CONSULTATION_REQUEST    
ADD ApprovedDate datetime    
Print('COMS_CONSULTATION_REQUEST.ApprovedDate Added')
END
----------------------------
IF COLUMNPROPERTY(OBJECT_ID('dbo.COMS_CONSULTATION_REQUEST'), 'SectorTypeId', 'ColumnId') IS NULL
BEGIN
ALTER TABLE COMS_CONSULTATION_REQUEST    
ADD SectorTypeId INT    
Print('COMS_CONSULTATION_REQUEST.SectorTypeId Added')
END

-----------------------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------------------
-------------------------------------------------Consultation End----------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------------------

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_DRAFTED_TEMPLATE'), 'SectorTypeId', 'ColumnId') IS NULL
BEGIN 
ALTER TABLE CMS_DRAFTED_TEMPLATE ADD SectorTypeId INT
	Print('CMS_DRAFTED_TEMPLATE.SectorTypeId Added')
END
----------------------------------
IF COLUMNPROPERTY(OBJECT_ID('dbo.LMS_LITERATURE_BORROW_APPROVAL_STATUS'), 'Name_Ar', 'ColumnId') IS NULL
BEGIN 
ALTER TABLE LMS_LITERATURE_BORROW_APPROVAL_STATUS
	 ADD Name_Ar NVARCHAR(500)
	Print('LMS_LITERATURE_BORROW_APPROVAL_STATUS.Name_Ar Added')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.LEGAL_TEMPLATE_SETTING'), 'Template_Value_Ar', 'ColumnId') IS NULL
BEGIN 
ALTER TABLE LEGAL_TEMPLATE_SETTING
	 ADD Template_Value_Ar NVARCHAR(500)
	Print('LEGAL_TEMPLATE_SETTING.Template_Value_Ar Added')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.LEGAL_TEMPLATE_SETTING'), 'Template_Heading_Ar', 'ColumnId') IS NULL
BEGIN 
ALTER TABLE LEGAL_TEMPLATE_SETTING
	 ADD Template_Heading_Ar NVARCHAR(500)
	Print('LEGAL_TEMPLATE_SETTING.Template_Heading_Ar Added')
END