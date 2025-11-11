/*<History Author='Nabeel ur Rehman' Date='26-01-2023'> Example of Create Table </History>*/
/******  
<Object Scope='Public'> LDS_DOCUMENT_REFERENCE  
<Copyright> Digital Processing System </Copyright>  
<Author Company='Business Analytics'> Nabeel ur Rehman </Author>  
<Created> 2022-06-15 </Created>  
<History Author='' Date=''>  </History>  
</Object>  
******/ 
-- [dbo].[LDS_DOCUMENT_REFERENCE] 


/****** Object:  Table [dbo].[LDS_DOCUMENT_REFERENCE]    Script Date: 01/07/2022 11:31:56 am ******/
--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LDS_DOCUMENT_REFERENCE]') AND type in (N'U'))
--DROP TABLE [dbo].[LDS_DOCUMENT_REFERENCE]
--GO

--/****** Object:  Table [dbo].[LDS_DOCUMENT_REFERENCE]    Script Date: 01/07/2022 11:31:56 am ******/
--SET ANSI_NULLS ON
--GO

--SET QUOTED_IDENTIFIER ON
--GO

--CREATE TABLE [dbo].[LDS_DOCUMENT_REFERENCE](
--	[ReferenceId] [uniqueidentifier] NOT NULL,
--	[MainDocumentId] [uniqueidentifier] NOT NULL,
--	[Reference_Document_Masked_Content] [nvarchar](max) NOT NULL,
--	[CreatedBy] [nvarchar](100) NOT NULL,
--	[CreatedDate] [datetime] NOT NULL,
--	[ModifiedBy] [nvarchar](100) NULL,
--	[ModifiedDate] [datetime] NULL,
--	[DeletedBy] [nvarchar](100) NULL,
--	[DeletedDate] [datetime] NULL,
--	[IsDeleted] [bit] NOT NULL,
--	[IsNewlyCreated] [bit] NOT NULL,
--	[IsPdf] [bit] NOT NULL,
-- CONSTRAINT [PK_LDS_DOCUMENT_REFERENCE] PRIMARY KEY CLUSTERED 
--(
--	[ReferenceId] ASC
--)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
--) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
--GO


/****** Object:  Table [dbo].[CMS_DRAFTED_TEMPLATE_REASON]    Script Date: 01/07/2022 11:31:56 am ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_DRAFTED_TEMPLATE_REASON]') AND type in (N'U'))
DROP TABLE [dbo].[CMS_DRAFTED_TEMPLATE_REASON]
GO

/****** Object:  Table [dbo].[CMS_DRAFTED_TEMPLATE_REASON]    Script Date: 01/07/2022 11:31:56 am ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CMS_DRAFTED_TEMPLATE_REASON](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DraftedTemplateId] [uniqueidentifier] NOT NULL,
	[VersionNumber] [float] NOT NULL,
	[Reason] [nvarchar](max) NOT NULL,
	[StatusId] [int] NOT NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](100) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](100) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL
 CONSTRAINT [PK_CMS_DRAFTED_TEMPLATE_REASON] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
--GO

-----------------------------------------[COMS_DRAFTED_TEMPLATE]
ALTER TABLE [dbo].[COMS_DRAFTED_TEMPLATE] DROP CONSTRAINT [COMS_DRAFTED_TEMPLATE_TEMPLATE]
GO

ALTER TABLE [dbo].[COMS_DRAFTED_TEMPLATE] DROP CONSTRAINT [COMS_DRAFTED_TEMPLATE_STATUS]
GO

ALTER TABLE [dbo].[COMS_DRAFTED_TEMPLATE] DROP CONSTRAINT [COMS_DRAFTED_TEMPLATE_ATTACHMENT_TYPE]
GO

/****** Object:  Table [dbo].[LMS_LITERATURE_DETAILS_LMS_LITERATURE_AUTHOR]    Script Date: 21/02/2023 10:06:35 pm ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LMS_LITERATURE_DETAILS_LMS_LITERATURE_AUTHOR]') AND type in (N'U'))
DROP TABLE [dbo].[LMS_LITERATURE_DETAILS_LMS_LITERATURE_AUTHOR]
GO

/****** Object:  Table [dbo].[LMS_LITERATURE_DETAILS_LMS_LITERATURE_AUTHOR]    Script Date: 21/02/2023 10:06:35 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LMS_LITERATURE_DETAILS_LMS_LITERATURE_AUTHOR](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LiteratureId] [int] NOT NULL,
	[AuthorId] [int] NOT NULL,
 CONSTRAINT [PK_LMS_LITERATURE_DETAILS_LMS_LITERATURE_AUTHOR] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[COMS_DRAFTED_TEMPLATE]    Script Date: 2/1/2023 5:29:24 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[COMS_DRAFTED_TEMPLATE]') AND type in (N'U'))
DROP TABLE [dbo].[COMS_DRAFTED_TEMPLATE]
GO

/****** Object:  Table [dbo].[COMS_DRAFTED_TEMPLATE]    Script Date: 2/1/2023 5:29:24 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[COMS_DRAFTED_TEMPLATE](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](1000) NOT NULL,
	[DraftNumber] [int] NOT NULL,
	[VersionNumber] [float] NOT NULL,
	[ReferenceId] [uniqueidentifier] NOT NULL,
	[TemplateId] [int] NOT NULL,
	[AttachmentTypeId] [int] NOT NULL,
	[StatusId] [int] NOT NULL,
	[ReviewerUserId] [nvarchar](256) NULL,
	[ReviewerRoleId] [nvarchar](256) NULL,
	[Description] [nvarchar](max) NULL,
	[CreatedBy] [nvarchar](256) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](256) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](256) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
	[DraftEntityType] [int] NULL,
	[Payload] [nvarchar](max) NULL,
	[SectorTypeId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[COMS_DRAFTED_TEMPLATE]  WITH CHECK ADD  CONSTRAINT [COMS_DRAFTED_TEMPLATE_ATTACHMENT_TYPE] FOREIGN KEY([AttachmentTypeId])
REFERENCES [dbo].[ATTACHMENT_TYPE] ([AttachmentTypeId])
GO

ALTER TABLE [dbo].[COMS_DRAFTED_TEMPLATE] CHECK CONSTRAINT [COMS_DRAFTED_TEMPLATE_ATTACHMENT_TYPE]
GO

ALTER TABLE [dbo].[COMS_DRAFTED_TEMPLATE]  WITH CHECK ADD  CONSTRAINT [COMS_DRAFTED_TEMPLATE_STATUS] FOREIGN KEY([StatusId])
REFERENCES [dbo].[CMS_DRAFT_DOCUMENT_STATUS] ([Id])
GO

ALTER TABLE [dbo].[COMS_DRAFTED_TEMPLATE] CHECK CONSTRAINT [COMS_DRAFTED_TEMPLATE_STATUS]
GO

ALTER TABLE [dbo].[COMS_DRAFTED_TEMPLATE]  WITH CHECK ADD  CONSTRAINT [COMS_DRAFTED_TEMPLATE_TEMPLATE] FOREIGN KEY([TemplateId])
REFERENCES [dbo].[CMS_TEMPLATE] ([Id])
GO

ALTER TABLE [dbo].[COMS_DRAFTED_TEMPLATE] CHECK CONSTRAINT [COMS_DRAFTED_TEMPLATE_TEMPLATE]
GO

--------------------------------------------[COMS_DRAFTED_TEMPLATE_SECTION]
ALTER TABLE [dbo].[COMS_DRAFTED_TEMPLATE_SECTION] DROP CONSTRAINT [COMS_DRAFTED_TEMPLATE_SECTION_SCTN]
GO

ALTER TABLE [dbo].[COMS_DRAFTED_TEMPLATE_SECTION] DROP CONSTRAINT [COMS_DRAFTED_TEMPLATE_SECTION_DRAFT_TEMP]
GO

/****** Object:  Table [dbo].[COMS_DRAFTED_TEMPLATE_SECTION]    Script Date: 2/1/2023 5:31:16 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[COMS_DRAFTED_TEMPLATE_SECTION]') AND type in (N'U'))
DROP TABLE [dbo].[COMS_DRAFTED_TEMPLATE_SECTION]
GO

/****** Object:  Table [dbo].[COMS_DRAFTED_TEMPLATE_SECTION]    Script Date: 2/1/2023 5:31:16 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[COMS_DRAFTED_TEMPLATE_SECTION](
	[Id] [uniqueidentifier] NOT NULL,
	[DraftedTemplateId] [uniqueidentifier] NOT NULL,
	[SectionId] [int] NOT NULL,
	[AdditionalName] [nvarchar](1000) NULL,
	[SequenceNumber] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[COMS_DRAFTED_TEMPLATE_SECTION]  WITH CHECK ADD  CONSTRAINT [COMS_DRAFTED_TEMPLATE_SECTION_DRAFT_TEMP] FOREIGN KEY([DraftedTemplateId])
REFERENCES [dbo].[COMS_DRAFTED_TEMPLATE] ([Id])
GO

ALTER TABLE [dbo].[COMS_DRAFTED_TEMPLATE_SECTION] CHECK CONSTRAINT [COMS_DRAFTED_TEMPLATE_SECTION_DRAFT_TEMP]
GO

ALTER TABLE [dbo].[COMS_DRAFTED_TEMPLATE_SECTION]  WITH CHECK ADD  CONSTRAINT [COMS_DRAFTED_TEMPLATE_SECTION_SCTN] FOREIGN KEY([SectionId])
REFERENCES [dbo].[CMS_SECTION] ([Id])
GO

ALTER TABLE [dbo].[COMS_DRAFTED_TEMPLATE_SECTION] CHECK CONSTRAINT [COMS_DRAFTED_TEMPLATE_SECTION_SCTN]
GO

-----------------------------------[COMS_DRAFTED_TEMPLATE_SECTION_PARAMETER]
ALTER TABLE [dbo].[COMS_DRAFTED_TEMPLATE_SECTION_PARAMETER] DROP CONSTRAINT [COMS_DRFT_TEMP_SCTN_PRMTR_PRMTR]
GO

ALTER TABLE [dbo].[COMS_DRAFTED_TEMPLATE_SECTION_PARAMETER] DROP CONSTRAINT [COMS_DRFT_TEMP_SCTN_PRMTR_DRFT_TEMP_SCTN]
GO

/****** Object:  Table [dbo].[COMS_DRAFTED_TEMPLATE_SECTION_PARAMETER]    Script Date: 2/1/2023 5:34:40 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[COMS_DRAFTED_TEMPLATE_SECTION_PARAMETER]') AND type in (N'U'))
DROP TABLE [dbo].[COMS_DRAFTED_TEMPLATE_SECTION_PARAMETER]
GO

/****** Object:  Table [dbo].[COMS_DRAFTED_TEMPLATE_SECTION_PARAMETER]    Script Date: 2/1/2023 5:34:40 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[COMS_DRAFTED_TEMPLATE_SECTION_PARAMETER](
	[Id] [uniqueidentifier] NOT NULL,
	[DraftedTemplateSectionId] [uniqueidentifier] NOT NULL,
	[ParameterId] [int] NOT NULL,
	[Value] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[COMS_DRAFTED_TEMPLATE_SECTION_PARAMETER]  WITH CHECK ADD  CONSTRAINT [COMS_DRFT_TEMP_SCTN_PRMTR_DRFT_TEMP_SCTN] FOREIGN KEY([DraftedTemplateSectionId])
REFERENCES [dbo].[COMS_DRAFTED_TEMPLATE_SECTION] ([Id])
GO

ALTER TABLE [dbo].[COMS_DRAFTED_TEMPLATE_SECTION_PARAMETER] CHECK CONSTRAINT [COMS_DRFT_TEMP_SCTN_PRMTR_DRFT_TEMP_SCTN]
GO

ALTER TABLE [dbo].[COMS_DRAFTED_TEMPLATE_SECTION_PARAMETER]  WITH CHECK ADD  CONSTRAINT [COMS_DRFT_TEMP_SCTN_PRMTR_PRMTR] FOREIGN KEY([ParameterId])
REFERENCES [dbo].[PARAMETER] ([ParameterId])
GO

ALTER TABLE [dbo].[COMS_DRAFTED_TEMPLATE_SECTION_PARAMETER] CHECK CONSTRAINT [COMS_DRFT_TEMP_SCTN_PRMTR_PRMTR]
GO

---------------------------------------COMS_DRAFTED_TEMPLATE_REASON
/****** Object:  Table [dbo].[COMS_DRAFTED_TEMPLATE_REASON]    Script Date: 2/2/2023 10:34:20 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[COMS_DRAFTED_TEMPLATE_REASON]') AND type in (N'U'))
DROP TABLE [dbo].[COMS_DRAFTED_TEMPLATE_REASON]
GO

/****** Object:  Table [dbo].[COMS_DRAFTED_TEMPLATE_REASON]    Script Date: 2/2/2023 10:34:20 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[COMS_DRAFTED_TEMPLATE_REASON](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DraftedTemplateId] [uniqueidentifier] NOT NULL,
	[VersionNumber] [float] NOT NULL,
	[Reason] [nvarchar](max) NOT NULL,
	[StatusId] [int] NOT NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](100) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](100) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_COMS_DRAFTED_TEMPLATE_REASON] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO



ALTER TABLE [dbo].[LMS_LITERATURE_DETAILS] DROP CONSTRAINT LMS_LITERATURE_DETAILS_LMS_LITERATURE_AUTHOR_LIT_DET

ALTER TABLE [dbo].[LMS_LITERATURE_DETAILS] DROP CONSTRAINT LMS_LITERATURE_DETAILS_LITERATURE_TYPE




/****** Object:  Table [dbo].[COMM_COMMUNICATION_SOURCE]    Script Date: 2/2/2023 10:34:20 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[COMM_COMMUNICATION_SOURCE]') AND type in (N'U'))
DROP TABLE [dbo].[COMM_COMMUNICATION_SOURCE]
GO

CREATE TABLE COMM_COMMUNICATION_SOURCE
(
Id INT PRIMARY KEY,
NameEn NVARCHAR(500),
NameAr NVARCHAR(500)
)

------------------------COMS_WITHDRAW_REQUEST
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[COMS_WITHDRAW_REQUEST]') AND type in (N'U'))
DROP TABLE [dbo].[COMS_WITHDRAW_REQUEST]
GO

/****** Object:  Table [dbo].[COMS_WITHDRAW_REQUEST]    Script Date: 2/20/2023 4:40:23 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[COMS_WITHDRAW_REQUEST](
	[Id] [uniqueidentifier] NOT NULL,
	[ConsultationRequestId] [uniqueidentifier] NOT NULL,
	[RequestNumber] [int] NOT NULL,
	[RequestDate] [datetime] NOT NULL,
	[Reason] [nvarchar](max) NULL,
	[RequestStatusId] [int] NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](100) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](100) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_COMS_WITHDRAW_REQUEST] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
--------------------------------------

/****** Object:  Table [dbo].[EMPLOYEE_PORTAL]    Script Date: 21/02/2023 5:51:43 pm ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EMPLOYEE_PORTAL]') AND type in (N'U'))
DROP TABLE [dbo].[EMPLOYEE_PORTAL]
GO

/****** Object:  Table [dbo].[EMPLOYEE_PORTAL]    Script Date: 21/02/2023 5:51:43 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EMPLOYEE_PORTAL](
	[Id] [nvarchar](450) NOT NULL,
	[UserName] [nvarchar](256) NULL,
	[Email] [nvarchar](256) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[FirstName_En] [nvarchar](150) NULL,
	[FirstName_Ar] [nvarchar](150) NULL,
	[SecondName_En] [nvarchar](150) NULL,
	[SecondName_Ar] [nvarchar](150) NULL,
	[LastName_En] [nvarchar](150) NULL,
	[LastName_Ar] [nvarchar](150) NULL,
	[AlternatePhoneNumber] [nvarchar](150) NULL,
	[DateOfBirth] [datetime] NULL,
	[Address] [nvarchar](256) NULL,
	[DateOfJoining] [datetime] NULL,
	[GroupId] [uniqueidentifier] NULL,
	[NationalityId] [int] NULL,
	[GenderId] [int] NULL,
	[UserTypeId] [int] NULL,
	[GradeId] [int] NULL,
	[DepartmentId] [int] NULL,
	[DesignationId] [int] NULL,
	[ManagerId] [nvarchar](150) NULL,
	[IsActive] [bit] NULL,
	[AllowAccess] [bit] NULL,
	[IsLocked] [bit] NULL,
	[AbsentFrom] [datetime] NULL,
	[AbsentTo] [datetime] NULL,
	[ReplacementId] [nvarchar](150) NULL,
	[ProfilePicReferenceGuid] [uniqueidentifier] NULL,
	[CreatedBy] [nvarchar](150) NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedBy] [nvarchar](150) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](150) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted] [bit] NULL,
	[SectorTypeId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


-------------------------------
/****** Object:  Table [dbo].[COMS_TEMPLATE_SECTION_HEAD]    Script Date: 02/22/2023 11:31:56 am ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[COMS_TEMPLATE_SECTION_HEAD]') AND type in (N'U'))
DROP TABLE [dbo].[COMS_TEMPLATE_SECTION_HEAD]
GO

/****** Object:  Table [dbo].[COMS_TEMPLATE_SECTION_HEAD]    Script Date: 01/07/2022 11:31:56 am ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[COMS_TEMPLATE_SECTION_HEAD](
	[SectionHeadId] [int] NOT NULL,
	[Name_En] [nvarchar](500) NOT NULL,
	[Name_Ar] [nvarchar](500) NOT NULL
 CONSTRAINT [PK_COMS_TEMPLATE_SECTION_HEAD] PRIMARY KEY CLUSTERED 
(
	[SectionHeadId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

---------------------[COMS_WITHDRAW_REQUEST_REASON]
/****** Object:  Table [dbo].[COMS_WITHDRAW_REQUEST_REASON]    Script Date: 2/24/2023 6:42:57 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[COMS_WITHDRAW_REQUEST_REASON]') AND type in (N'U'))
DROP TABLE [dbo].[COMS_WITHDRAW_REQUEST_REASON]
GO

/****** Object:  Table [dbo].[COMS_WITHDRAW_REQUEST_REASON]    Script Date: 2/24/2023 6:42:57 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[COMS_WITHDRAW_REQUEST_REASON](
	[Id] [uniqueidentifier] NOT NULL,
	[WithdrawRequestId] [uniqueidentifier] NOT NULL,
	[ConsultationRequestId] [uniqueidentifier] NOT NULL,
	[RequestDate] [datetime] NOT NULL,
	[Reason] [nvarchar](max) NULL,
	[RequestStatusId] [int] NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](100) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](100) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_COMS_WITHDRAW_REQUEST_REASON] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


/****** Object:  Table [dbo].[CMS_GOVERNMENT_ENTITY_REPRESENTATIVES]    Script Date: 2/1/2023 5:29:24 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_GOVERNMENT_ENTITY_REPRESENTATIVES]') AND type in (N'U'))
DROP TABLE [dbo].[CMS_GOVERNMENT_ENTITY_REPRESENTATIVES]
GO

CREATE TABLE CMS_GOVERNMENT_ENTITY_REPRESENTATIVES
(
Id INT PRIMARY KEY IDENTITY(1,1),
NameEn NVARCHAR(1000),
NameAr NVARCHAR(1000),
GovtEntityId INT,
[CreatedBy] [nvarchar](100) NOT NULL,
[CreatedDate] [datetime] NOT NULL,
[ModifiedBy] [nvarchar](100) NULL,
[ModifiedDate] [datetime] NULL,
[DeletedBy] [nvarchar](100) NULL,
[DeletedDate] [datetime] NULL,
[IsDeleted] [bit] NOT NULL,
)

ALTER TABLE CMS_GOVERNMENT_ENTITY_REPRESENTATIVES ADD CONSTRAINT CMS_GOVERNMENT_ENTITY_REPRESENTATIVES_FK FOREIGN KEY (GovtEntityId)
REFERENCES CMS_GOVERNMENT_ENTITY_G2G_LKP(EntityId)


/****** Object:  Table [dbo].[CMS_LAWYER_SUPERVISOR] ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_LAWYER_SUPERVISOR]') AND type in (N'U'))
DROP TABLE [dbo].[CMS_LAWYER_SUPERVISOR]
GO

CREATE TABLE CMS_LAWYER_SUPERVISOR
(
Id UNIQUEIDENTIFIER PRIMARY KEY,
LawyerId NVARCHAR(450) NOT NULL,
SupervisorId NVARCHAR(450) NOT NULL,
[CreatedBy] [nvarchar](100) NOT NULL,
[CreatedDate] [datetime] NOT NULL,
[ModifiedBy] [nvarchar](100) NULL,
[ModifiedDate] [datetime] NULL,
[DeletedBy] [nvarchar](100) NULL,
[DeletedDate] [datetime] NULL,
[IsDeleted] [bit] NOT NULL
)

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[COMS_CONSULTATION_INTERNATIONAL_ARBITRATION_TYPE_FTW_LKP]') AND type in (N'U'))
--DROP TABLE [dbo].[COMS_CONSULTATION_INTERNATIONAL_ARBITRATION_TYPE_FTW_LKP]
--GO

--CREATE TABLE COMS_CONSULTATION_INTERNATIONAL_ARBITRATION_TYPE_FTW_LKP
--(
--Id INT PRIMARY KEY,
--NameEn NVARCHAR(500),
--NameAr NVARCHAR(500)
--)


-----------  Consultation (06-03-2023) ---------------
GO

/****** Object:  Table [dbo].[COMS_TEMPLATE_SECTION]    Script Date: 06/03/2023 5:43:30 pm ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[COMS_TEMPLATE_SECTION]') AND type in (N'U'))
DROP TABLE [dbo].[COMS_TEMPLATE_SECTION]
GO

/****** Object:  Table [dbo].[COMS_TEMPLATE_SECTION]    Script Date: 06/03/2023 5:43:30 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[COMS_TEMPLATE_SECTION](
	[TemplateSectionId] [int] NOT NULL,
	[Name] [nvarchar](500) NOT NULL,
	[Content_En] [nvarchar](max) NULL,
	[Content_Ar] [nvarchar](max) NULL,
	[Name_Ar] [nvarchar](1000) NULL,
	[SectionHeadId] [int] NOT NULL,
 CONSTRAINT [PK_COMS_TEMPLATE_SECTION] PRIMARY KEY CLUSTERED 
(
	[TemplateSectionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[COMS_TEMPLATE_SECTION] ADD  DEFAULT ((0)) FOR [SectionHeadId]
GO

---------------------------

GO

/****** Object:  Table [dbo].[COMS_CONSULTATION_INTERNATIONAL_ARBITRATION_TYPE_FTW_LKP]    Script Date: 06/03/2023 5:43:30 pm ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[COMS_CONSULTATION_INTERNATIONAL_ARBITRATION_TYPE_FTW_LKP]') AND type in (N'U'))
DROP TABLE [dbo].[COMS_CONSULTATION_INTERNATIONAL_ARBITRATION_TYPE_FTW_LKP]
GO

/****** Object:  Table [dbo].[COMS_CONSULTATION_INTERNATIONAL_ARBITRATION_TYPE_FTW_LKP]    Script Date: 06/03/2023 5:43:30 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[COMS_CONSULTATION_INTERNATIONAL_ARBITRATION_TYPE_G2G_LKP] (
 [Id] INT NOT NULL,
 [NameEn] NVARCHAR (500) NULL,
 [NameAr] NVARCHAR (500) NULL,
 CONSTRAINT [PK_COMS_CONSULTATION_INTERNATIONAL_ARBITRATION_TYPE_G2G_LKP] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
---------------------CMS_CASE_DECISION_TYPE_G2G_LKP
/****** Object:  Table [dbo].[CMS_CASE_DECISION_TYPE_G2G_LKP]    Script Date: 2/2/2023 10:34:20 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_CASE_DECISION_TYPE_G2G_LKP]') AND type in (N'U'))
DROP TABLE [dbo].[CMS_CASE_DECISION_TYPE_G2G_LKP]
GO

 

CREATE TABLE CMS_CASE_DECISION_TYPE_G2G_LKP
(
Id INT PRIMARY KEY,
NameEn NVARCHAR(500),
NameAr NVARCHAR(500)
)
--------------------CMS_CASE_DECISION
/****** Object:  Table [dbo].[CMS_CASE_DECISION]    Script Date: 01/07/2022 11:31:56 am ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_CASE_DECISION]') AND type in (N'U'))
DROP TABLE [dbo].[CMS_CASE_DECISION]
GO

/****** Object:  Table [dbo].[CMS_CASE_DECISION]    Script Date: 01/07/2022 11:31:56 am ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CMS_CASE_DECISION](
	[Id] [uniqueidentifier] NOT NULL,
	[DecisionTypeId] [int] NOT NULL,
	[CaseId] [uniqueidentifier] NOT NULL,
	[Title] [nvarchar](500)NULL,
	[Description] [nvarchar](max)NULL,
	[ReferenceNo][nvarchar](500)NULL,
	[ReferenceDate][datetime]NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](100) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](100) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted][bit] NOT NULL,
	
 CONSTRAINT [PK_CMS_CASE_DECISION] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

GO


/****** Object:  Table [dbo].[CMS_PRE_COURT_TYPE_G2G_LKP]    Script Date: 2/1/2023 5:29:24 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_PRE_COURT_TYPE_G2G_LKP]') AND type in (N'U'))
DROP TABLE [dbo].[CMS_PRE_COURT_TYPE_G2G_LKP]
GO

CREATE TABLE CMS_PRE_COURT_TYPE_G2G_LKP
(
Id INT PRIMARY KEY IDENTITY(1,1),
Name_En NVARCHAR(500),
Name_Ar NVARCHAR(500)
)

INSERT INTO CMS_PRE_COURT_TYPE_G2G_LKP VALUES('Tort Claims','Tort Claims')
INSERT INTO CMS_PRE_COURT_TYPE_G2G_LKP VALUES('Property','Property')


/****** Object:  Table [dbo].[CMS_MOJ_EXECUTION_REQUEST]    Script Date: 4/6/2023 5:25:45 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_MOJ_EXECUTION_REQUEST]') AND type in (N'U'))
DROP TABLE [dbo].[CMS_MOJ_EXECUTION_REQUEST]
GO

/****** Object:  Table [dbo].[CMS_MOJ_EXECUTION_REQUEST]    Script Date: 4/6/2023 5:25:45 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CMS_MOJ_EXECUTION_REQUEST](
	[Id] [uniqueidentifier] NOT NULL,
	[CaseId] [uniqueidentifier] NULL,
	[StatusId] [int] NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](100) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](100) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
	[Remarks] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO




/****** Object:  Table [dbo].[CNT_CONTACT]    Script Date: 4/5/2023 11:21:02 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CNT_CONTACT]') AND type in (N'U'))
DROP TABLE [dbo].[CNT_CONTACT]
GO

/****** Object:  Table [dbo].[CNT_CONTACT]    Script Date: 4/5/2023 11:21:02 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CNT_CONTACT](
	[ContactId] [uniqueidentifier] NOT NULL,
	[ContactTypeId] [int] NOT NULL,
	[JobRoleId] [int] NOT NULL,
	[SectorId] [int] NOT NULL,
	[DepartmentId] [int] NOT NULL,
	[FirstName] [nvarchar](max) NOT NULL,
	[SecondName] [nvarchar](max) NULL,
	[LastName] [nvarchar](max) NOT NULL,
	[CivilId] [nvarchar](20) NOT NULL,
	[DOB] [datetime] NOT NULL,
	[PhoneNumber] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](256) NOT NULL,
	[Notes] [nvarchar](max) NULL,
	[CreatedBy] [nvarchar](150) NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedBy] [nvarchar](150) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](150) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK_CNT_CONTACT] PRIMARY KEY CLUSTERED 
(
	[ContactId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[CNT_CONTACT_JOB_ROLE_LKP]    Script Date: 4/5/2023 11:21:25 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CNT_CONTACT_JOB_ROLE_LKP]') AND type in (N'U'))
DROP TABLE [dbo].[CNT_CONTACT_JOB_ROLE_LKP]
GO

/****** Object:  Table [dbo].[CNT_CONTACT_JOB_ROLE_LKP]    Script Date: 4/5/2023 11:21:25 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CNT_CONTACT_JOB_ROLE_LKP](
	[RoleId] [int] NOT NULL,
	[NameEn] [nvarchar](100) NULL,
	[NameAr] [nvarchar](100) NULL,
 CONSTRAINT [PK_CNT_CONTACT_JOB_ROLE] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO



/****** Object:  Table [dbo].[CNT_CONTACT_REQUEST]    Script Date: 01/07/2022 11:31:56 am ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CNT_CONTACT_REQUEST]') AND type in (N'U'))
DROP TABLE [dbo].[CNT_CONTACT_REQUEST]
GO

/****** Object:  Table [dbo].[CNT_CONTACT_REQUEST]    Script Date: 01/07/2022 11:31:56 am ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CNT_CONTACT_REQUEST](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ContactId] [uniqueidentifier] NOT NULL,
	[ReferenceId] [uniqueidentifier] NOT NULL,
	[ContactLinkId] [int] NOT NULL,
	[ModuleId] [int] NOT NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](100) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](100) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL
 CONSTRAINT [PK_CNT_CONTACT_REQUEST] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[CNT_CONTACT_LINK_LKP]    Script Date: 01/07/2022 11:31:56 am ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CNT_CONTACT_LINK_LKP]') AND type in (N'U'))
DROP TABLE [dbo].[CNT_CONTACT_LINK_LKP]
GO

/****** Object:  Table [dbo].[CNT_CONTACT_LINK_LKP]    Script Date: 01/07/2022 11:31:56 am ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CNT_CONTACT_LINK_LKP](
 [ContactLinkId] INT NOT NULL,
 [NameEn] NVARCHAR (500) NULL,
 [NameAr] NVARCHAR (500) NULL,
 CONSTRAINT [PK_CNT_CONTACT_LINK_LKP] PRIMARY KEY CLUSTERED 
(
	[ContactLinkId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[CNT_CONTACT_TYPE_LKP]    Script Date: 4/6/2023 5:22:44 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CNT_CONTACT_TYPE_LKP]') AND type in (N'U'))
DROP TABLE [dbo].[CNT_CONTACT_TYPE_LKP]
GO

/****** Object:  Table [dbo].[CNT_CONTACT_TYPE_LKP]    Script Date: 4/6/2023 5:22:44 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CNT_CONTACT_TYPE_LKP](
	[TypeId] [int] NOT NULL,
	[NameEn] [nvarchar](100) NULL,
	[NameAr] [nvarchar](100) NULL,
 CONSTRAINT [PK_CNT_CONTACT_TYPE] PRIMARY KEY CLUSTERED 
(
	[TypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO




---------------------------------[CMS_DECISION_REQUEST_ASSIGNEE]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_DECISION_REQUEST_ASSIGNEE]') AND type in (N'U'))
DROP TABLE [dbo].[CMS_DECISION_REQUEST_ASSIGNEE]
GO

/****** Object:  Table [dbo].[CMS_DECISION_REQUEST_ASSIGNEE]    Script Date: 2/1/2023 5:31:16 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CMS_DECISION_REQUEST_ASSIGNEE](
	[Id] [uniqueidentifier] NOT NULL,
	[DecisionId] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[CreatedBy] [nvarchar](256) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](256) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](256) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

----------Assign Back to Hos-------------
/****** Object:  Table [dbo].[CMS_ASSIGN_CASEFILE_BACKTO_HOS]    Script Date: 17/04/2023 9:57:47 am ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CMS_ASSIGN_CASEFILE_BACKTO_HOS](
	[Id] [uniqueidentifier] NOT NULL,
	[ReferenceId] [uniqueidentifier] NOT NULL,
	[Remarks] [nvarchar](max) NOT NULL,
	[CreatedBy] [nvarchar](256) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](256) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](256) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
-----------------------------------------[CMS_CASE_FILE]
ALTER TABLE CMS_CASE_FILE 
ADD IsAssignedBack Bit DEFAULT 0;

	----------------Inventory Management Tables----------------

	/****** Object:  Table [dbo].[INV_SERVICE_REQUEST]    Script Date: 03/05/2023 5:06:07 pm ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[INV_SERVICE_REQUEST]') AND type in (N'U'))
DROP TABLE [dbo].[INV_SERVICE_REQUEST]
GO

/****** Object:  Table [dbo].[INV_SERVICE_REQUEST]    Script Date: 03/05/2023 5:06:07 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INV_SERVICE_REQUEST](
	[RequestId] [uniqueidentifier] NOT NULL,
	[RequestStatusId] [int] NOT NULL,
	[RequestNumber] [int] NOT NULL,
	[RequestDate] [datetime] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[SpecialInstruction] [nvarchar](max) NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](100) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](100) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL
PRIMARY KEY CLUSTERED 
(
	[RequestId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


--------------END OF INV SERVICE REQUEST TABLE-----------


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[INV_SERVICE_REQUEST_ITEM]') AND type in (N'U'))
DROP TABLE [dbo].[INV_SERVICE_REQUEST_ITEM]
GO

/****** Object:  Table [dbo].[INV_SERVICE_REQUEST_ITEM]    Script Date: 03/05/2023 5:06:07 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INV_SERVICE_REQUEST_ITEM](
	[RequestItemId] [uniqueidentifier] NOT NULL,
	[RequestId] [uniqueidentifier] NOT NULL,
	[ItemStatusId] [int] NOT NULL,
	[InventoryDepartmentId] [int] NOT NULL,
	[ItemCategoryId] [int] NOT NULL,
	[ItemNameId] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[PendingQuantity] [int]  NULL,
PRIMARY KEY CLUSTERED 
(
	[RequestItemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

-------------INV_ITEM_CATEGORY--TABLE------
/****** Object:  Table [dbo].[INV_SERVICE_CATEGORY_LKP]    Script Date: 03/05/2023 5:22:39 pm ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[INV_SERVICE_CATEGORY_LKP]') AND type in (N'U'))
DROP TABLE [dbo].[INV_SERVICE_CATEGORY_LKP]
GO

/****** Object:  Table [dbo].[INV_SERVICE_CATEGORY_LKP]    Script Date: 03/05/2023 5:22:39 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INV_ITEM_CATEGORY](
	[ItemCategoryId] [int] IDENTITY (1,1) NOT NULL,
	[InventoryDepartmentId] [int] NOT NULL,
	[NameEn] [nvarchar](500) NOT NULL,
	[NameAr] [nvarchar](500) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ItemCategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

--------------END OF INV ITEM CATEGORY TABLE-----------

-------------INV_ITEM_NAME--TABLE------
/****** Object:  Table [dbo].[INV_ITEM_NAME]    Script Date: 03/05/2023 5:23:30 pm ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[INV_ITEM_NAME]') AND type in (N'U'))
DROP TABLE [dbo].[INV_ITEM_NAME]
GO

/****** Object:  Table [dbo].[INV_ITEM_NAME]    Script Date: 03/05/2023 5:23:30 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INV_ITEM_NAME](
	[ItemNameId] [int] IDENTITY (1,1) NOT NULL,
	[ItemCategoryId] [int] NOT NULL,
	[ItemCode] [nvarchar](100) NOT NULL,
	[NameEn] [nvarchar](500) NOT NULL,
	[NameAr] [nvarchar](500) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ItemNameId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

--------------END OF INV ITEM NAME TABLE-----------


----------INV REQUEST STATUS----------
/****** Object:  Table [dbo].[INV_SERVICE_STATUS_LKP]    Script Date: 03/05/2023 4:08:36 pm ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[INV_SERVICE_STATUS_LKP]') AND type in (N'U'))
DROP TABLE [dbo].[INV_SERVICE_STATUS_LKP]
GO

/****** Object:  Table [dbo].[INV_SERVICE_STATUS_LKP]    Script Date: 03/05/2023 4:08:36 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INV_REQUEST_STATUS_LKP](
	[Id] [int] NOT NULL,
	[NameEn] [nvarchar](100) NOT NULL,
	[NameAr] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
---------------END OF REQUEST STATUS -----------

-------Inv Inventory Department---------

/****** Object:  Table [dbo].[INV_SERVICE_DEPARTMENT_LKP]    Script Date: 03/05/2023 4:51:23 pm ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[INV_SERVICE_DEPARTMENT_LKP]') AND type in (N'U'))
DROP TABLE [dbo].[INV_SERVICE_DEPARTMENT_LKP]
GO

/****** Object:  Table [dbo].[INV_SERVICE_DEPARTMENT_LKP]    Script Date: 03/05/2023 4:51:23 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INV_INVENTORY_DEPARTMENT_LKP](
	[InventoryDepartmentId] [int] NOT NULL,
	[NameEn] [nvarchar](500) NOT NULL,
	[NameAr] [nvarchar](500) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[InventoryDepartmentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


-----End of Inventory Department -----------

--- Add Column of LawyerId in CMS_HEARING Table--

ALTER TABLE CMS_HEARING
ADD  LawyerId nvarchar(256) null
-----------------------------------------------DMS SCRIPTS START-----------------------------------------------------------------------------------
-----------------------------------------------DMS SCRIPTS START---------------------------------------------------------------------------------------
-----------------------------------------------DMS SCRIPTS START------------------------------------------------------------------------------------

-------------------DMS_FAVOURITE_DOCUMENT
/****** Object:  Table [dbo].[DMS_FAVOURITE_DOCUMENT]    Script Date: 6/9/2023 5:41:54 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DMS_USER_FAVOURITE_DOCUMENT]') AND type in (N'U'))
DROP TABLE [dbo].[DMS_USER_FAVOURITE_DOCUMENT]
GO

/****** Object:  Table [dbo].[DMS_USER_FAVOURITE_DOCUMENT]   Script Date: 6/9/2023 5:41:54 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DMS_USER_FAVOURITE_DOCUMENT](
	[UserId] [nvarchar](450) NOT NULL,
	[DocumentId] [int] NOT NULL
) ON [PRIMARY]
GO

-------------------DMS_SHARED_DOCUMENT
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DMS_SHARED_DOCUMENT]') AND type in (N'U'))
DROP TABLE [dbo].[DMS_SHARED_DOCUMENT]
GO

/****** Object:  Table [dbo].[DMS_SHARED_DOCUMENT]   Script Date: 6/9/2023 5:41:54 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE  TABLE [dbo].[DMS_SHARED_DOCUMENT](
[Id] uniqueidentifier NOT NULL,
[DocumentId] int NOT NULL,
[SenderId] nvarchar(450) NOT NULL,
[RecieverId] nvarchar(450) NOT NULL,
[Notes] nvarchar(max) NULL,
[CreatedDate]  datetime NOT NULL,
[CreatedBy] nvarchar(256) NOT NULL,
[ModifiedBy] nvarchar(256) NULL,
[ModifiedDate] datetime NULL,
[IsDeleted] bit NOT NULL,
[DeletedBy] nvarchar(256) NULL,
[DeletedDate] datetime NULL,	
CONSTRAINT [PK_DMS_SHARED_DOCUMENT] PRIMARY KEY CLUSTERED
(
[Id] ASC
)
WITH (PAD_INDEX= OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF,ALLOW_ROW_LOCKS = ON ,ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
---------------------DMS_FILE_TYPES_LKP(DMS_DB)
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DMS_FILE_TYPES_LKP]') AND type in (N'U'))
DROP TABLE [dbo].[DMS_FILE_TYPES_LKP]
GO

CREATE TABLE [dbo].[DMS_FILE_TYPES_LKP](
[Id] int NOT NULL,
[Name_En] nvarchar(100) NOT NULL,
[Name_Ar] nvarchar(100) NOT NULL,
PRIMARY KEY CLUSTERED
(
[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) 
)ON [PRIMARY]
GO
--------------------------Create USer favourite table again
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DMS_USER_FAVOURITE_DOCUMENT]') AND type in (N'U'))
DROP TABLE [dbo].[DMS_USER_FAVOURITE_DOCUMENT]
GO

/****** Object:  Table [dbo].[DMS_USER_FAVOURITE_DOCUMENT]   Script Date: 6/9/2023 5:41:54 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DMS_USER_FAVOURITE_DOCUMENT](
	[Id] uniqueidentifier NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[DocumentId] [int]  NULL,
	[AddedDocumentVersionId] uniqueidentifier NULL
CONSTRAINT [PK_DMS_FAVOURITE_DOCUMENT] PRIMARY KEY CLUSTERED
(
[Id] ASC
)
WITH (PAD_INDEX= OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF,ALLOW_ROW_LOCKS = ON ,ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] 
GO
-----------------------------------------------DMS SCRIPTS END----------------------------------------------------------------------------------------
-----------------------------------------------DMS SCRIPTS END----------------------------------------------------------------------------------------
-----------------------------------------------DMS SCRIPTS END----------------------------------------------------------------------------------------

------------------------LEGAL_LEGISLATION_ARTICLE_EFFECT_HISTORY
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_LEGISLATION_ARTICLE_EFFECT_HISTORY]') AND type in (N'U'))
DROP TABLE [dbo].[LEGAL_LEGISLATION_ARTICLE_EFFECT_HISTORY]
GO

/****** Object:  Table [dbo].[LEGAL_LEGISLATION_ARTICLE_EFFECT_HISTORY]    Script Date: 2/20/2023 4:40:23 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LEGAL_LEGISLATION_ARTICLE_EFFECT_HISTORY](
	[Id] [uniqueidentifier] NOT NULL,
	[LegislationId] [uniqueidentifier] NOT NULL,
	[ArticleId] [uniqueidentifier] NOT NULL,
	[ArticleStatus] [int] NOT NULL,
	[Note] [nvarchar](max) NOT NULL
 CONSTRAINT [PK_LEGAL_LEGISLATION_ARTICLE_EFFECT_HISTORY] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


/****** Object:  Table [dbo].[CMS_TEMPLATE_SECTION_PARAMETER]    Script Date: 27/08/2023 1:26:28 pm ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_TEMPLATE_SECTION_PARAMETER]') AND type in (N'U'))
DROP TABLE [dbo].[CMS_TEMPLATE_SECTION_PARAMETER]
GO

/****** Object:  Table [dbo].[CMS_TEMPLATE_SECTION_PARAMETER]    Script Date: 27/08/2023 1:26:28 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CMS_TEMPLATE_SECTION_PARAMETER](
	[Id] [uniqueidentifier] NOT NULL,
	[TemplateSectionId] [uniqueidentifier] NOT NULL,
	[ParameterId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
--------------------------------
CREATE TABLE WORKFLOW_SUBMODULE
(
Id INT PRIMARY KEY,
Name_En NVARCHAR (500),
Name_Ar NVARCHAR (500),
ModuleId int,
IsActive bit
)


-------------- 9-4-2023 --------------
CREATE TABLE [dbo].[CMS_TEMPLATE_PARAMETER](
	[ParameterId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NULL,
	[PKey] [nvarchar](255) NULL,
	[Mandatory] [bit] NULL,
	[IsAutoPopulated] [bit] NOT NULL,
	[ModuleId][int] NOT NULL,
	[IsActive][bit] Null,
PRIMARY KEY CLUSTERED 
(
	[ParameterId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[CMS_TEMPLATE_PARAMETER] ADD  DEFAULT ((0)) FOR [IsAutoPopulated]
GO
-- DROP CONSTRAINT -- 
ALTER TABLE CMS_DRAFTED_TEMPLATE_SECTION_PARAMETER
DROP CONSTRAINT CMS_DRFT_TEMP_SCTN_PRMTR_PRMTR;
-- ADD CONSTRAINT -- 
ALTER TABLE [dbo].[CMS_DRAFTED_TEMPLATE_SECTION_PARAMETER]  WITH CHECK 
ADD  CONSTRAINT [CMS_DRFT_TEMP_SCTN_PRMTR_PRMTR] FOREIGN KEY([ParameterId])
REFERENCES [dbo].[CMS_TEMPLATE_PARAMETER] ([ParameterId])
-- DROP CONSTRAINT -- 
ALTER TABLE COMS_DRAFTED_TEMPLATE_SECTION_PARAMETER
DROP CONSTRAINT COMS_DRFT_TEMP_SCTN_PRMTR_PRMTR;
-- ADD CONSTRAINT -- 
ALTER TABLE [dbo].[COMS_DRAFTED_TEMPLATE_SECTION_PARAMETER]  WITH CHECK 
ADD  CONSTRAINT [COMS_DRFT_TEMP_SCTN_PRMTR_PRMETER] FOREIGN KEY([ParameterId])
REFERENCES [dbo].[CMS_TEMPLATE_PARAMETER] ([ParameterId])

----- 9-4-2023 -----
----------------------WORKFLOW_SUBMODULE_TRIGGER-----------
CREATE TABLE WORKFLOW_SUBMODULE_TRIGGER
(
Id INT Primary Key,
WorkflowSubModuleId INT NOT NULL ,
ModuleTriggerId INT FOREIGN KEY REFERENCES MODULE_TRIGGER(ModuleTriggerId)
)
----------------------
CREATE TABLE WORKFLOW_SUBMODULE_ACTIVITY
(
Id INT Primary Key,
WorkflowSubModuleId INT NOT NULL,
SubModuleActivityId INT FOREIGN KEY REFERENCES MODULE_ACTIVITY(ActivityId)
)
-------------------
CREATE TABLE WORKFLOW_SUBMODULE_CONDITION
(
Id INT IDENTITY(1,1) PRIMARY KEY,
WorkflowSubModuleId INT NOT NULL,
SubModuleConditionId INT FOREIGN KEY REFERENCES MODULE_CONDITION(ModuleConditionId)
)

----- 07-09-2023


/****** Object:  Table [dbo].[LMS_LITERATURE_BORROW_RETURN_APPROVAL_STATUS]    Script Date: 01/07/2022 11:31:56 am ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LMS_LITERATURE_BORROW_RETURN_APPROVAL_STATUS]') AND type in (N'U'))
DROP TABLE [dbo].[LMS_LITERATURE_BORROW_RETURN_APPROVAL_STATUS]
GO

/****** Object:  Table [dbo].[LMS_LITERATURE_BORROW_RETURN_APPROVAL_STATUS]    Script Date: 01/07/2022 11:31:56 am ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LMS_LITERATURE_BORROW_RETURN_APPROVAL_STATUS](
	[Id] [int] NOT NULL,
	[NameEn] [nvarchar](500) NOT NULL,
	[NameAr] [nvarchar](500) NOT NULL
 CONSTRAINT [PK_LMS_LITERATURE_BORROW_RETURN_APPROVAL_STATUS] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


---- Case / Consultation request 
IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_CASE_REQUEST'), 'SequenceFormatResult', 'ColumnId') IS NULL
BEGIN 
ALTER TABLE CMS_CASE_REQUEST
	 ADD SequenceFormatResult NVARCHAR(256)
	Print('CMS_CASE_REQUEST.SequenceFormatResult Added')
END

IF COLUMNPROPERTY(OBJECT_ID('dbo.CMS_COMS_NUM_PATTERN'), 'IsModified', 'ColumnId') IS NULL
BEGIN 
ALTER TABLE CMS_COMS_NUM_PATTERN
	 ADD IsModified bit NOT NULL DEFAULT 0
	Print('CMS_COMS_NUM_PATTERN.IsModified Added')
END
--------- consultation-File
ALTER TABLE COMS_CONSULTATION_File 
ADD IsAssignedBack Bit DEFAULT 0;
-------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_DRAFT_EXPERT_OPINION]') AND type in (N'U'))
DROP TABLE [dbo].[CMS_DRAFT_EXPERT_OPINION]
GO

/****** Object:  Table [dbo].[CMS_DRAFT_EXPERT_OPINION]    Script Date: 9/24/2023 3:08:16 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

--------------NUM PATTERN HISTORY TABLE-------------

/****** Object:  Table [dbo].[CMS_COMS_NUM_PATTERN_HISTORY]    Script Date: 20/10/2023 8:56:45 pm ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_COMS_NUM_PATTERN_HISTORY]') AND type in (N'U'))
DROP TABLE [dbo].[CMS_COMS_NUM_PATTERN_HISTORY]
GO

/****** Object:  Table [dbo].[CMS_COMS_NUM_PATTERN_HISTORY]    Script Date: 20/10/2023 8:56:45 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CMS_COMS_NUM_PATTERN_HISTORY](
	[id] [uniqueidentifier] NOT NULL,
	[PatternTypId] [int] NOT NULL,
	[PatternId] [uniqueidentifier] NOT NULL,
	[Day] [nvarchar](50) NULL,
	[D_order] [int] NULL,
	[Month] [nvarchar](50) NULL,
	[M_order] [int] NULL,
	[Year] [nvarchar](50) NULL,
	[Y_order] [int] NULL,
	[CharaterString] [nvarchar](150) NOT NULL,
	[CS_order] [int] NULL,
	[SequanceNumber] [nvarchar](max) NOT NULL,
	[SN_order] [int] NULL,
	[UserId] [uniqueidentifier] NULL,
	[SequanceResult] [nvarchar](max) NOT NULL,
	[IsYearUpdate] [bit] NOT NULL,
	[CreatedBy] [nvarchar](256) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](256) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](256) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
	[SequanceFormatResult] [nvarchar](256) NULL,
	 
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[CMS_COMS_NUM_PATTERN_HISTORY]  WITH CHECK ADD  CONSTRAINT [FK_CMS_COMS_NUM_PATTERN_HISTORY] FOREIGN KEY([PatternId])
REFERENCES [dbo].[CMS_COMS_NUM_PATTERN] ([id])
GO

ALTER TABLE [dbo].[CMS_COMS_NUM_PATTERN_HISTORY] CHECK CONSTRAINT [FK_CMS_COMS_NUM_PATTERN_HISTORY]
GO



ALTER TABLE [dbo].[CMS_COMS_NUM_PATTERN_HISTORY]
WITH CHECK ADD CONSTRAINT [FK_CMS_COMS_NUM_PATTERN_HISTORY]
FOREIGN KEY ([PatternId])
REFERENCES [dbo].[CMS_COMS_NUM_PATTERN] ([Id]);
/****** Object:  Table [dbo].[COMS_CONSULTATION_LEGISLATION_FILE_TYPE_FTW_LKP]    Script Date: 22/10/2023 9:14:29 pm ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[COMS_CONSULTATION_LEGISLATION_FILE_TYPE_G2G_LKP]') AND type in (N'U'))
DROP TABLE [dbo].[COMS_CONSULTATION_LEGISLATION_FILE_TYPE_G2G_LKP]
GO

/****** Object:  Table [dbo].[COMS_CONSULTATION_LEGISLATION_FILE_TYPE_FTW_LKP]    Script Date: 22/10/2023 9:14:29 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[COMS_CONSULTATION_LEGISLATION_FILE_TYPE_G2G_LKP](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name_En] [nvarchar](500) NOT NULL,
	[Name_Ar] [nvarchar](500) NOT NULL,
	[CreatedBy] [nvarchar](256) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](256) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](256) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_COMS_CONSULTATION_LEGISLATION_FILE_TYPE_G2G_LKP] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO



/****** Object:  Table [dbo].[COMS_CONSULTATION_INTERNATIONAL_ARBITRATION_TYPE_FTW_LKP]    Script Date: 23/10/2023 12:09:55 am ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[COMS_CONSULTATION_INTERNATIONAL_ARBITRATION_TYPE_FTW_LKP]') AND type in (N'U'))
DROP TABLE [dbo].[COMS_CONSULTATION_INTERNATIONAL_ARBITRATION_TYPE_G2G_LKP]
GO

/****** Object:  Table [dbo].[COMS_CONSULTATION_INTERNATIONAL_ARBITRATION_TYPE_FTW_LKP]    Script Date: 23/10/2023 12:09:55 am ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[COMS_CONSULTATION_INTERNATIONAL_ARBITRATION_TYPE_G2G_LKP](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[NameEn] [nvarchar](500) NULL,
	[NameAr] [nvarchar](500) NULL,
	[CreatedBy] [nvarchar](256) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](256) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](256) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_COMS_CONSULTATION_INTERNATIONAL_ARBITRATION_TYPE_G2G_LKP] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[COMS_CONSULTATION_INTERNATIONAL_ARBITRATION_TYPE_G2G_LKP] ADD  DEFAULT ('fatwaadmin@gmail.com') FOR [CreatedBy]
GO

ALTER TABLE [dbo].[COMS_CONSULTATION_INTERNATIONAL_ARBITRATION_TYPE_G2G_LKP] ADD  DEFAULT ('2023-08-13 17:01:00.000') FOR [CreatedDate]
GO

ALTER TABLE [dbo].[COMS_CONSULTATION_INTERNATIONAL_ARBITRATION_TYPE_G2G_LKP] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO

ALTER TABLE [dbo].[COMS_CONSULTATION_INTERNATIONAL_ARBITRATION_TYPE_G2G_LKP] ADD  DEFAULT ((1)) FOR [IsActive]
GO
------------Consultation File Number Column 
alter table  COMS_CONSULTATION_FILE
add  ComsFileNumberFormat nvarchar(500)


alter table COMS_CONSULTATION_FILE 
add IsAssignedBack bit null 


 Alter table COMM_COMMUNICATION 
add InboxNumberFormat nvarchar(500)

Alter table COMM_COMMUNICATION 
add OutBoxNumberFormat nvarchar(500)


----- Contact Mgt

/****** Object:  Table [dbo].[CNT_CONTACT]    Script Date: 4/5/2023 11:21:02 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CNT_CONTACT]') AND type in (N'U'))
DROP TABLE [dbo].[CNT_CONTACT]
GO

/****** Object:  Table [dbo].[CNT_CONTACT]    Script Date: 4/5/2023 11:21:02 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CNT_CONTACT](
	[ContactId] [uniqueidentifier] NOT NULL,
	[ContactTypeId] [int] NOT NULL,
	[JobRoleId] [int] NULL,
	[SectorId] [int] NULL,
	[DepartmentId] [int] NULL,
	[FirstName] [nvarchar](max) NOT NULL,
	[SecondName] [nvarchar](max) NULL,
	[LastName] [nvarchar](max) NOT NULL,
	[CivilId] [nvarchar](20) NULL,
	[DOB] [datetime] NULL,
	[PhoneNumber] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](256) NULL,
	[Notes] [nvarchar](max) NULL,
	[WorkPlace] [int] NULL,
	[Designation] [int] NULL,
	[CreatedBy] [nvarchar](150) NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedBy] [nvarchar](150) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](150) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK_CNT_CONTACT] PRIMARY KEY CLUSTERED 
(
	[ContactId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

--Added By Jahanzaib Ayub Khan 1/11/2023
ALTER TABLE MEET_MEETING_MOM
ADD MOMStatusId INT, Content NVARCHAR(MAX);

GO

USE [FATWA_DB_DEV]
GO

/****** Object:  Table [dbo].[MEET_MEETING_MOM_ATTENDEE_DECISION]    Script Date: 11/17/2023 12:29:47 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MEET_MEETING_MOM_ATTENDEE_DECISION](
	[MomAttendeeDecisionId] [uniqueidentifier] NOT NULL,
	[MeetingMomId] [uniqueidentifier] NOT NULL,
	[MeetingId] [uniqueidentifier] NOT NULL,
	[MeetingAttendeeId] [uniqueidentifier] NOT NULL,
	[AttendeeStatusId] [int] NOT NULL,
	[Comment] [nvarchar](max) NULL,
 CONSTRAINT [PK_MEET_MEETING_MOM_ATTENDEE_DECISION] PRIMARY KEY CLUSTERED 
(
	[MomAttendeeDecisionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[MEET_MEETING_MOM_ATTENDEE_DECISION]  WITH CHECK ADD  CONSTRAINT [FK_MEET_MEETING_MOM_ATTENDEE_DECISION_MEET_MEETING] FOREIGN KEY([MeetingId])
REFERENCES [dbo].[MEET_MEETING] ([MeetingId])
GO

ALTER TABLE [dbo].[MEET_MEETING_MOM_ATTENDEE_DECISION] CHECK CONSTRAINT [FK_MEET_MEETING_MOM_ATTENDEE_DECISION_MEET_MEETING]
GO

ALTER TABLE [dbo].[MEET_MEETING_MOM_ATTENDEE_DECISION]  WITH CHECK ADD  CONSTRAINT [FK_MEET_MEETING_MOM_ATTENDEE_DECISION_MEET_MEETING_ATTENDEE] FOREIGN KEY([MeetingAttendeeId])
REFERENCES [dbo].[MEET_MEETING_ATTENDEE] ([MeetingAttendeeId])
GO

ALTER TABLE [dbo].[MEET_MEETING_MOM_ATTENDEE_DECISION] CHECK CONSTRAINT [FK_MEET_MEETING_MOM_ATTENDEE_DECISION_MEET_MEETING_ATTENDEE]
GO

ALTER TABLE [dbo].[MEET_MEETING_MOM_ATTENDEE_DECISION]  WITH CHECK ADD  CONSTRAINT [FK_MEET_MEETING_MOM_ATTENDEE_DECISION_MEET_MEETING_ATTENDEE_STATUS] FOREIGN KEY([AttendeeStatusId])
REFERENCES [dbo].[MEET_MEETING_ATTENDEE_STATUS] ([Id])
GO

ALTER TABLE [dbo].[MEET_MEETING_MOM_ATTENDEE_DECISION] CHECK CONSTRAINT [FK_MEET_MEETING_MOM_ATTENDEE_DECISION_MEET_MEETING_ATTENDEE_STATUS]
GO

ALTER TABLE [dbo].[MEET_MEETING_MOM_ATTENDEE_DECISION]  WITH CHECK ADD  CONSTRAINT [FK_MEET_MEETING_MOM_ATTENDEE_DECISION_MEET_MEETING_MOM] FOREIGN KEY([MeetingMomId])
REFERENCES [dbo].[MEET_MEETING_MOM] ([MeetingMomId])
GO

ALTER TABLE [dbo].[MEET_MEETING_MOM_ATTENDEE_DECISION] CHECK CONSTRAINT [FK_MEET_MEETING_MOM_ATTENDEE_DECISION_MEET_MEETING_MOM]
GO

------------------------13/11/2023 link target
 ALTER TABLE LINK_TARGET
ALTER COLUMN ReferenceId unilueidentifier null;

------------------------------------------------------------------------------------------------------------------


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_DOCUMENT_PORTFOLIO]') AND type in (N'U'))
DROP TABLE [dbo].[CMS_DOCUMENT_PORTFOLIO]
GO


CREATE TABLE CMS_DOCUMENT_PORTFOLIO
(
Id UNIQUEIDENTIFIER PRIMARY KEY,
Name NVARCHAR(256),
HearingId UNIQUEIDENTIFIER,
AttachmentTypeId INT,
StoragePath NVARCHAR(MAX),
[CreatedBy] [nvarchar](256) NOT NULL,
[CreatedDate] [datetime] NOT NULL,
[ModifiedBy] [nvarchar](256) NULL,
[ModifiedDate] [datetime] NULL,
[DeletedBy] [nvarchar](256) NULL,
[DeletedDate] [datetime] NULL,
[IsDeleted] [bit] NOT NULL
)

------------------------------------------------------------------------------------------------------------------

/****** Object:  Table [dbo].[CMS_JUDGEMENT_CATEGORY_G2G_LKP]    Script Date: 29/11/2023 11:33:32 am ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_JUDGEMENT_CATEGORY_G2G_LKP]') AND type in (N'U'))
DROP TABLE [dbo].[CMS_JUDGEMENT_CATEGORY_G2G_LKP]
GO

/****** Object:  Table [dbo].[CMS_JUDGEMENT_CATEGORY_G2G_LKP]    Script Date: 29/11/2023 11:33:32 am ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CMS_JUDGEMENT_CATEGORY_G2G_LKP](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[NameEn] [nvarchar](100) NOT NULL,
	[NameAr] [nvarchar](100) NOT NULL,
	[IsActive] [bit] NULL,
	[CreatedBy] [nvarchar](256) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](256) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](256) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

----------------------------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  Table [dbo].[CMS_JUDGEMENT_STATUS_G2G_LKP]    Script Date: 29/11/2023 11:33:32 am ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_JUDGEMENT_STATUS_G2G_LKP]') AND type in (N'U'))
DROP TABLE [dbo].CMS_JUDGEMENT_STATUS_G2G_LKP
GO

/****** Object:  Table [dbo].[CMS_JUDGEMENT_STATUS_G2G_LKP]    Script Date: 29/11/2023 11:33:32 am ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].CMS_JUDGEMENT_STATUS_G2G_LKP(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[NameEn] [nvarchar](100) NOT NULL,
	[NameAr] [nvarchar](100) NOT NULL,
	[IsActive] [bit] NULL,
	[CreatedBy] [nvarchar](256) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](256) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](256) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


----------------------------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  Table [dbo].[CMS_EXECUTION_FILE_LEVEL_G2G_LKP]    Script Date: 29/11/2023 11:33:32 am ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_EXECUTION_FILE_LEVEL_G2G_LKP]') AND type in (N'U'))
DROP TABLE [dbo].CMS_EXECUTION_FILE_LEVEL_G2G_LKP
GO

/****** Object:  Table [dbo].[CMS_EXECUTION_FILE_LEVEL_G2G_LKP]    Script Date: 29/11/2023 11:33:32 am ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].CMS_EXECUTION_FILE_LEVEL_G2G_LKP(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[NameEn] [nvarchar](100) NOT NULL,
	[NameAr] [nvarchar](100) NOT NULL,
	[IsActive] [bit] NULL,
	[CreatedBy] [nvarchar](256) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](256) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](256) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO



--ADD TABLE CMS_CHAMBER_NUMBER_G2G_LK 29/11/23

CREATE TABLE CMS_CHAMBER_NUMBER_G2G_LKP(
Id INT PRIMARY KEY IDENTITY(1,1),
Number NVARCHAR(50) NOT NULL,
Code NVARCHAR(50) NOT NULL,
ChamberId INT NOT NULL,
IsActive BIT NOT NULL,
CreatedBy NVARCHAR(256) NOT NULL,
CreatedDate DATETIME NOT NULL,
ModifiedBy NVARCHAR(256),
ModifiedDate DATETIME,
DeletedBy NVARCHAR(256),
DeletedDate DATETIME,
IsDeleted BIT NOT NULL
)

ALTER TABLE CMS_CHAMBER_NUMBER_G2G_LKP
ADD CONSTRAINT  FK_CHAMBER_NUMBER_CHAMBER
FOREIGN KEY (ChamberId)
REFERENCES CMS_CHAMBER_G2G_LKP(Id)


----------------------------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  Table [dbo].[CMS_LAWYER_CHAMBER_NUMBER_ASSIGNMENT]    Script Date: 29/11/2023 4:13:59 pm ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_LAWYER_CHAMBER_NUMBER_ASSIGNMENT]') AND type in (N'U'))
DROP TABLE [dbo].[CMS_LAWYER_CHAMBER_NUMBER_ASSIGNMENT]
GO

/****** Object:  Table [dbo].[CMS_LAWYER_CHAMBER_NUMBER_ASSIGNMENT]    Script Date: 29/11/2023 4:13:59 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CMS_LAWYER_CHAMBER_NUMBER_ASSIGNMENT](
	[Id] [uniqueidentifier] NOT NULL,
	[LawyerId] [nvarchar](256) NOT NULL,
	[ChamberNumberId] [int] NOT NULL,
	[CreatedBy] [nvarchar](256) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](256) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](256) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
-----------------
/****** Object:  Table [dbo].[CMS_RESGISTERED_CASE_TRANSFER_HISTORY]    Script Date: 06/12/2023 5:39:28 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CMS_RESGISTERED_CASE_TRANSFER_HISTORY](
	[Id] [uniqueidentifier] NOT NULL,
	[ChamberFromId] [int] NOT NULL,
	[ChamberToId] [int] NOT NULL,
	[ChamberNumberFromId] [int] NOT NULL,
	[ChamberNumberToId] [int] NOT NULL,
	[OutcomeId] [uniqueidentifier] NOT NULL,
	[CreatedBy] [nvarchar](256) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](256) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](256) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[CMS_RESGISTERED_CASE_TRANSFER_HISTORY]  WITH CHECK ADD  CONSTRAINT [FK_CMS_RESGISTERED_CASE_TRANSFER_HISTORY_OUTCOME_HEARING] FOREIGN KEY([OutcomeId])
REFERENCES [dbo].[CMS_OUTCOME_HEARING] ([Id])
GO
ALTER TABLE [dbo].[CMS_RESGISTERED_CASE_TRANSFER_HISTORY] CHECK CONSTRAINT [FK_CMS_RESGISTERED_CASE_TRANSFER_HISTORY_OUTCOME_HEARING]
GO
----------------------------------------------------------------------------------------------------------------------------------------------------------
--EXECUTION STATUS LKP   5/12/23
CREATE TABLE [dbo].CMS_EXECUTION_STATUS_G2G_LKP(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[NameEn] [nvarchar](100) NOT NULL,
	[NameAr] [nvarchar](100) NOT NULL,
	[IsActive] [bit] NULL,
	[CreatedBy] [nvarchar](256) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](256) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](256) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
-- Add column for Government Entity Representative 
ALTER TABLE CMS_GOVERNMENT_ENTITY_REPRESENTATIVES
ADD    [Representative_Designation_EN] nvarchar(500) NULL,
		[Representative_Designation_AR] nvarchar(500) NULL,
 		[IsActive] bit NOT NULL DEFAULT '1',
		[RepresentativeCode] nvarchar(20) NULL;

		-----Add column in government tabel 

		Alter table CMS_GOVERNMENT_ENTITY_G2G_LKP
		add GECode Nvarchar(20) null 
		---Drop column from Court -Number 
		ALTER TABLE CMS_COURT_G2G_LKP
        DROP COLUMN Number;
		---Drop column from Government Entity Table
		alter table CMS_GOVERNMENT_ENTITY_G2G_LKP
        drop column Description,
        column DesignationofRepresentative;





CREATE TABLE [dbo].[CMS_DRAFT_EXPERT_OPINION](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Opinion] [nvarchar](max) NOT NULL,
	[DraftedTemplateVersionId] [uniqueidentifier] NOT NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_EXPERT_OPINION] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MODULE_CONDITION_OPTIONS]') AND type in (N'U'))
DROP TABLE [dbo].[MODULE_CONDITION_OPTIONS]
GO

/****** Object:  Table [dbo].[MODULE_CONDITION_OPTIONS]    Script Date: 10/6/2023 11:33:30 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
CREATE  TABLE [DBO].[MODULE_CONDITION_OPTIONS]
(
[ModuleOptionId] [int] NOT NULL,
[Name] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED
(
[ModuleOptionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] 
GO
------------------------------[WORKFLOW_CONDITION_OPTIONS]
/****** Object:  Table [dbo].[WORKFLOW_CONDITION_OPTIONS]    Script Date: 10/6/2023 12:38:49 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[WORKFLOW_CONDITION_OPTIONS]') AND type in (N'U'))
DROP TABLE [dbo].[WORKFLOW_CONDITION_OPTIONS]
GO

/****** Object:  Table [dbo].[WORKFLOW_CONDITION_OPTIONS]   Script Date: 10/6/2023 12:38:49 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WORKFLOW_CONDITION_OPTIONS]
(
[WorkflowOptionId] [int] IDENTITY(1,1) NOT NULL,
[ModuleOptionId] [int] NULL,
[WorkflowConditionId] [int] NULL,
[TrueCaseFlowControlId] [int] NULL,
[TrueCaseActivityNo] [int] NULL,
[CreatedBy] [nvarchar](500) NOT NULL,
[CreatedDate] [datetime] NOT NULL,
[ModifiedBy] [nvarchar](256) NULL,
[ModifiedDate] [datetime] NULL,
[DeletedBy] [datetime] NULL,
[DeletedDate] [datetime] NULL,
[IsDeleted] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
[WorkflowOptionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
--------------------------- WORKFLOW OPTIONS IMPLEMENTATION
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[WF_WORKFLOW_OPTIONS]') AND type in (N'U'))
DROP TABLE [dbo].[WF_WORKFLOW_OPTIONS]
GO

/****** Object:  Table [dbo].[WF_WORKFLOW_OPTIONS]   Script Date: 10/6/2023 12:38:49 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
Create Table [dbo].[WF_WORKFLOW_OPTIONS]
(
WorkflowOptionId INT Identity(1,1) NOT NULL,
ModuleOptionId INT  NULL,
WorkflowActivityId INT  NULL,
TrueCaseFlowControlId INT  NULL,
TrueCaseActivityNo INT NULL,
[CreatedBy] [nvarchar](500) NOT NULL,
[CreatedDate] [datetime] NOT NULL,
[ModifiedBy] [nvarchar](256) NULL,
[ModifiedDate] [datetime] NULL,
[DeletedBy] [datetime] NULL,
[DeletedDate] [datetime] NULL,
[IsDeleted] [bit] NOT NULL, PRIMARY KEY CLUSTERED 
(
[WorkflowOptionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
--------------------------- WORKFLOW OPTIONS IMPLEMENTATION
----------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_DRAFT_DOCUMENT_VERSION_STATUS]') AND type in (N'U'))
DROP TABLE [dbo].[CMS_DRAFT_DOCUMENT_VERSION_STATUS]
GO
CREATE TABLE CMS_DRAFT_DOCUMENT_VERSION_STATUS
(
Id INT Primary Key,
NameEn nvarchar (200) NOT NULL ,
NameAr nvarchar (200) NOT NULL 
)
-----------
-----------------------------------------------------------
--------------------------------------------------------------------
-------------------------------------------------------------------------- Send Copy Case Request Workflow(10-11-2023)

ALTER TABLE [dbo].[CMS_COPY_HISTORY] DROP CONSTRAINT [DF__CMS_COPY___Submo__3D690CCA]
GO

/****** Object:  Table [dbo].[CMS_COPY_HISTORY]    Script Date: 11/10/2023 4:13:23 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_COPY_HISTORY]') AND type in (N'U'))
DROP TABLE [dbo].[CMS_COPY_HISTORY]
GO

/****** Object:  Table [dbo].[CMS_COPY_HISTORY]    Script Date: 11/10/2023 4:13:23 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CMS_COPY_HISTORY](
	[CopyHistoryId] [uniqueidentifier] NOT NULL,
	[ReferenceId] [uniqueidentifier] NOT NULL,
	[StatusId] [int] NOT NULL,
	[SectorFrom] [int] NOT NULL,
	[SectorTo] [int] NOT NULL,
	[Reason] [nvarchar](max) NULL,
	[CreatedBy] [nvarchar](256) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](256) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](256) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
	[SubmoduleId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[CopyHistoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[CMS_COPY_HISTORY] ADD  DEFAULT ((0)) FOR [SubmoduleId]
GO


-----------------------------------------------------------
--------------------------------------------------------------------
-------------------------------------------------------------------------- Send Copy Case Request Workflow(10-11-2023)


-------------------update Workflow Condition Name 20/12/23-------------
UPDATE WF_CONDITION_PR_LKP SET Name = 'If_Legislation_Document_Status_is_Draft' WHERE ModuleConditionId = 1
UPDATE WF_CONDITION_PR_LKP SET Name = 'If_Legislation_Document_Status_is_InReview' WHERE ModuleConditionId = 2
UPDATE WF_CONDITION_PR_LKP SET Name = 'If_Legislation_Document_Status_is_Approve' WHERE ModuleConditionId = 4
UPDATE WF_CONDITION_PR_LKP SET Name = 'If_Legislation_Document_Status_is_Reject' WHERE ModuleConditionId = 8
UPDATE WF_CONDITION_PR_LKP SET Name = 'If_Legislation_Document_Status_is_NeedToModify' WHERE ModuleConditionId = 16
UPDATE WF_CONDITION_PR_LKP SET Name = 'If_Legislation_Document_Status_is_SendaComment' WHERE ModuleConditionId = 32
UPDATE WF_CONDITION_PR_LKP SET Name = 'If_Legislation_Document_Status_is_Publish' WHERE ModuleConditionId = 64
UPDATE WF_CONDITION_PR_LKP SET Name = 'If_Legislation_Document_Status_is_Unpublish' WHERE ModuleConditionId = 128
UPDATE WF_CONDITION_PR_LKP SET Name = 'If_Principle_Status_is_Draft' WHERE ModuleConditionId = 129
UPDATE WF_CONDITION_PR_LKP SET Name = 'If_Principle_Status_is_InReview' WHERE ModuleConditionId = 130
UPDATE WF_CONDITION_PR_LKP SET Name = 'If_Principle_Status_is_Approve' WHERE ModuleConditionId = 131
UPDATE WF_CONDITION_PR_LKP SET Name = 'If_Principle_Status_is_Reject' WHERE ModuleConditionId = 132
UPDATE WF_CONDITION_PR_LKP SET Name = 'If_Principle_Status_is_NeedToModify' WHERE ModuleConditionId = 133
UPDATE WF_CONDITION_PR_LKP SET Name = 'If_Principle_Status_is_SendaComment' WHERE ModuleConditionId = 134
UPDATE WF_CONDITION_PR_LKP SET Name = 'If_Principle_Status_is_Publish' WHERE ModuleConditionId = 135
UPDATE WF_CONDITION_PR_LKP SET Name = 'If_Principle_Status_is_Unpublish' WHERE ModuleConditionId = 136
UPDATE WF_CONDITION_PR_LKP SET Name = 'If_Document_Status_is_Draft' WHERE ModuleConditionId = 149
UPDATE WF_CONDITION_PR_LKP SET Name = 'If_Document_Status_is_InReview' WHERE ModuleConditionId = 150
UPDATE WF_CONDITION_PR_LKP SET Name = 'If_Document_Status_is_Reject' WHERE ModuleConditionId = 151
UPDATE WF_CONDITION_PR_LKP SET Name = 'If_Document_Status_is_Approve' WHERE ModuleConditionId = 152
UPDATE WF_CONDITION_PR_LKP SET Name = 'If_Document_Status_is_Publish' WHERE ModuleConditionId = 153
UPDATE WF_CONDITION_PR_LKP SET Name = 'If_Draft_Document_is_Submitted_By_Lawyer' WHERE ModuleConditionId = 160
UPDATE WF_CONDITION_PR_LKP SET Name = 'If_Draft_Document_is_Submitted_By_Supervisor' WHERE ModuleConditionId = 161
UPDATE WF_CONDITION_PR_LKP SET Name = 'If_Draft_Document_is_Submitted_By_HOs' WHERE ModuleConditionId = 162
UPDATE WF_CONDITION_PR_LKP SET Name = 'If_Draft_Document_Status_is_InReview' WHERE ModuleConditionId = 163
UPDATE WF_CONDITION_PR_LKP SET Name = 'If_Draft_Document_Status_is_Draft' WHERE ModuleConditionId = 164
UPDATE WF_CONDITION_PR_LKP SET Name = 'If_Draft_Document_Status_is_Reject' WHERE ModuleConditionId = 165
UPDATE WF_CONDITION_PR_LKP SET Name = 'If_Draft_Document_Status_is_Approve' WHERE ModuleConditionId = 166
UPDATE WF_CONDITION_PR_LKP SET Name = 'If_Draft_Document_Status_is_Published' WHERE ModuleConditionId = 167
UPDATE WF_CONDITION_PR_LKP SET Name = 'If_Draft_Document_is_Submitted_By_Lawyer' WHERE ModuleConditionId = 168
UPDATE WF_CONDITION_PR_LKP SET Name = 'If_Draft_Document_is_Submitted_By_Supervisor' WHERE ModuleConditionId = 169
UPDATE WF_CONDITION_PR_LKP SET Name = 'If_Draft_Document_is_Submitted_By_HOs' WHERE ModuleConditionId = 170
-------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_OUTCOME_PARTY_HISTORY]') AND type in (N'U'))
DROP TABLE [dbo].[CMS_OUTCOME_PARTY_HISTORY]
GO

/****** Object:  Table [dbo].[CMS_OUTCOME_PARTY_HISTORY]   Script Date: 01/07/2022 11:31:56 am ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CMS_OUTCOME_PARTY_HISTORY]
(
Id UniqueIdentifier NOT NULL,
OutcomeId UniqueIdentifier NOT NULL,
CasePartyLinkId UniqueIdentifier NOT NULL,
ActionId INT NOT NULL,
[CreatedBy] [nvarchar](100) NOT NULL,
[CreatedDate] [datetime] NOT NULL,
[ModifiedBy] [nvarchar](100) NULL,
[ModifiedDate] [datetime] NULL,
[DeletedBy] [nvarchar](100) NULL,
[DeletedDate] [datetime] NULL,
[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_OUTCOME_PARTY_HISTORY] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] 
GO

-----------------------01/01/2024

IF COLUMNPROPERTY(OBJECT_ID('dbo.COMM_COMMUNICATION_ATTENDEES'), 'IsPresent', 'ColumnId') IS NULL
BEGIN
ALTER TABLE COMM_COMMUNICATION_ATTENDEES
ADD IsPresent bit null;
Print('COMM_COMMUNICATION_ATTENDEES.IsPresent Added')
END 

IF COLUMNPROPERTY(OBJECT_ID('dbo.COMM_COMMUNICATION_ATTENDEES'), 'AttendeeUserId', 'ColumnId') IS NULL
BEGIN
ALTER TABLE COMM_COMMUNICATION_ATTENDEES
ADD AttendeeUserId nvarchar(max)   NULL;
Print('COMM_COMMUNICATION_ATTENDEES.AttendeeUserId Added')
END
--------------------------------------------------------
--------------------------------------------------------
/****** Object:  Table [dbo].[KAY_PUBLICATION_STG]    Script Date: 1/4/2024 1:25:04 PM ******/

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[KAY_PUBLICATION_STG]') AND type in (N'U'))
DROP TABLE [dbo].[KAY_PUBLICATION_STG]
GO

/****** Object:  Table [dbo].[CMS_OUTCOME_PARTY_HISTORY]   Script Date: 01/07/2022 11:31:56 am ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KAY_PUBLICATION_STG](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EditionNumber] [nvarchar](250) NULL,
	[EditionType] [nvarchar](250) NULL,
	[PublicationDate] [datetime] NULL,
	[DocumentTitle] [nvarchar](500) NULL,
	[FileTitle] [nvarchar](250) NULL,
	[StoragePath] [nvarchar](500) NULL,
	[ReferenceGuid] [uniqueidentifier] NULL,
	[StartPage] [int] NULL,
	[EndPage] [int] NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedBy] [nvarchar](256) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedBy] [nvarchar](256) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](256) NULL,
	[DeletedDate] [datetime] NULL,
 CONSTRAINT [PK_KAY_PUBLICATION] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

-------------- AUDIT PASSWORD HISTORY FOR UMS_USER-------- 12-JAN-2024--------------


GO
/****** Object:  Table [dbo].[UMS_USER_PASSWORD_HISTORY]    Script Date: 12/01/2024 3:57:52 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UMS_USER_PASSWORD_HISTORY](
	[HistoryId] [uniqueidentifier] NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [nvarchar](150) NULL,
PRIMARY KEY CLUSTERED 
(
	[HistoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[UMS_USER_PASSWORD_HISTORY]  WITH CHECK ADD  CONSTRAINT [FK_USERID] FOREIGN KEY([UserId])
REFERENCES [dbo].[UMS_USER] ([Id])
GO
ALTER TABLE [dbo].[UMS_USER_PASSWORD_HISTORY] CHECK CONSTRAINT [FK_USERID]
GO


----------18-01-2024---------
alter table EP_EMPLOYMENT_INFORMATION 
drop constraint UQ_EmployeeId

alter table EP_EMPLOYMENT_INFORMATION 
add constraint  UQ_EmployeeId unique (EmployeeId,EmployeeTypeId)


---------------------------------Muhammad Ali-------------------25-01-2024---------------------------------
/****** Object:  Table [dbo].[CMS_GOVERNMENT_ENTITY_NUM_PATTERN]    Script Date: 19/01/2024 6:20:06 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CMS_GOVERNMENT_ENTITY_NUM_PATTERN](
	[Id] [uniqueidentifier] NOT NULL,
	[GovtEntityId] [int] NULL,
	[CMSRequestPatternId] [uniqueidentifier] NULL,
	[COMSRequestPatternId] [uniqueidentifier] NULL,
	[CreatedBy] [nvarchar](256) NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedBy] [nvarchar](256) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](256) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted] [bit] NULL,
	[IsActive] [bit] NULL,
 CONSTRAINT [PK_CMS_GOVERNMENT_ENTITY_NUM_PATTERN] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[CMS_GOVERNMENT_ENTITY_NUM_PATTERN]  WITH CHECK ADD  CONSTRAINT [FK_CMS_GOVERNMENT_ENTITY_NUM_PATTERN_CMS_GOVERNMENT_ENTITY_G2G_LKP] FOREIGN KEY([GovtEntityId])
REFERENCES [dbo].[CMS_GOVERNMENT_ENTITY_G2G_LKP] ([EntityId])
GO
ALTER TABLE [dbo].[CMS_GOVERNMENT_ENTITY_NUM_PATTERN] CHECK CONSTRAINT [FK_CMS_GOVERNMENT_ENTITY_NUM_PATTERN_CMS_GOVERNMENT_ENTITY_G2G_LKP]
GO

-------------------------------------------------

ALTER TABLE CMS_GOVERNMENT_ENTITY_G2G_LKP
DROP COLUMN CMSRequestPatternId, COMSRequestPatternId;

-------------------------------------------------------------End--------------------------------------
--------------------------------------------------------------Worker Service Table Script-------------------------------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[WS_COMM_COMMUNICATION_TYPES]') AND type in (N'U'))
DROP TABLE [dbo].[WS_COMM_COMMUNICATION_TYPES]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WS_COMM_COMMUNICATION_TYPES](
	[CommunicationTypeId] [int] NOT NULL,
	[NameAr] [nvarchar](150) NULL,
	[NameEn] [nvarchar](150) NULL,
	[CreatedBy] [nvarchar](256) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](256) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](256) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_WS_COMM_COMMUNICATION_TYPES] PRIMARY KEY CLUSTERED 
(
	[CommunicationTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
-------------------------------------------------------------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[WS_CMS_COMS_REMINDER_HISTORY]') AND type in (N'U'))
DROP TABLE [dbo].[WS_CMS_COMS_REMINDER_HISTORY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WS_CMS_COMS_REMINDER_HISTORY](
    [Id] [uniqueidentifier] NOT NULL,
    [CmsComsReminderId] [int] NULL,
    [CmsComsReminderTypeId] [int] NOT NULL,
    [SLAInterval] [int] NULL,
    [FirstReminderDuration] [int] NULL,
    [SecondReminderDuration] [int] NULL,
    [ThirdReminderDuration] [int] NULL,
    [CommunicationTypeId] [int] NULL,
    [ExecutionTime] [datetime] NULL,
    [DraftTemplateVersionStatusId] [int] NULL,
    [CmsCaseFileStatusId] [int] NULL,
    [IsNotification] [bit] NULL,
    [IsTask] [bit] NULL,
    [StatusId] [int] NULL,
    [CreatedBy] [nvarchar](256) NOT NULL,
    [CreatedDate] [datetime] NOT NULL,
    [ModifiedBy] [nvarchar](256) NULL,
    [ModifiedDate] [datetime] NULL,
    [DeletedBy] [nvarchar](256) NULL,
    [DeletedDate] [datetime] NULL,
    [IsDeleted] [bit] NOT NULL,
    [IsActive] [bit] NOT NULL,
    [CouttypeId] [int] NULL,
CONSTRAINT [PK_CMS_COMS_REMINDER_HISTORY] PRIMARY KEY CLUSTERED
(
    [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
--------------------------------------------------------------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[WS_CMS_COMS_REMINDER_HISTORY_STATUS]') AND type in (N'U'))
DROP TABLE [dbo].[WS_CMS_COMS_REMINDER_HISTORY_STATUS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WS_CMS_COMS_REMINDER_HISTORY_STATUS](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[NameEn] [varchar](256) NOT NULL,
	[NameAr] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_CMS_COMS_REMINDER_HISTORY_STATUS] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
-------------------------------------------------------------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[WS_CMS_COMS_REMINDER]') AND type in (N'U'))
DROP TABLE [dbo].[WS_CMS_COMS_REMINDER]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WS_CMS_COMS_REMINDER](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CmsComsReminderTypeId] [int] NOT NULL,
	[FirstReminderDuration] [int] NULL,
	[SecondReminderDuration] [int] NULL,
	[ThirdReminderDuration] [int] NULL,
	[CommunicationTypeId] [int] NULL,
	[DraftTemplateVersionStatusId] [int] NULL,
	[CmsCaseFileStatusId] [int] NULL,
	[IsNotification] [bit] NULL,
	[IsTask] [bit] NULL,
	[CreatedBy] [nvarchar](256) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](256) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](256) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CouttypeId] [int] NULL,
	[SLAInterval] [int] NULL,
	[ExecutionTime] [datetime] NULL,
 CONSTRAINT [PK_CMS_COMS_REMINDER] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
--------------------------------------------------------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[WS_EXECUTION_STATUS_LKP]') AND type in (N'U'))
DROP TABLE [dbo].[WS_EXECUTION_STATUS_LKP]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WS_EXECUTION_STATUS_LKP](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ExecutionStatusEn] [varchar](50) NOT NULL,
	[ExecutionStatusAr] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_WS_EXECUTION_STATUS] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
--------------------------------------------------------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[WS_PROCESS_TYPE_LKP]') AND type in (N'U'))
DROP TABLE [dbo].[WS_PROCESS_TYPE_LKP]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WS_PROCESS_TYPE_LKP](
	[Id] [int] NOT NULL,
	[ProcessTypeEn] [varchar](256) NOT NULL,
	[ProcessTypeAr] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_WS_PROCESS_TYPE] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
------------------------------------------------------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[WS_WORKERSERVICES_LKP]') AND type in (N'U'))
DROP TABLE [dbo].[WS_WORKERSERVICES_LKP]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WS_WORKERSERVICES_LKP](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[WorkerServiceEn] [varchar](100) NOT NULL,
	[WorkerServiceAr] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_WS_WorkerService_LKP] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
----------------------------------------------------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[WS_REMINDER_ERRORLOG]') AND type in (N'U'))
DROP TABLE [dbo].[WS_REMINDER_ERRORLOG]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WS_REMINDER_ERRORLOG](
	[Id] [uniqueidentifier] NOT NULL,
	[WorkerServiceId] [int] NULL,
	[ProcessLogId] [int] NULL,
	[ReceiverId] [nvarchar](256) NULL,
	[Sender] [nvarchar](256) NULL,
	[ProcessTypeId] [int] NULL,
	[ReminderId] [int] NULL,
	[ReminderTypeId] [int] NULL,
	[CommunicationTypeId] [int] NULL,
	[DraftTemplateVersionStatusId] [int] NULL,
	[CmsCaseFileStatusId] [int] NULL,
	[Message] [nvarchar](256) NULL,
	[IsNotification] [bit] NULL,
	[IsTask] [bit] NULL,
	[CreatedBy] [nvarchar](256) NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedBy] [nvarchar](256) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](256) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted] [bit] NULL,
	[IsActive] [bit] NULL,
	[IsFirstReminder] [bit] NULL,
	[IsSecondReminder] [bit] NULL,
	[IsThirdReminder] [bit] NULL,
	[ReferenceId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_WS_REMINDER_ERRORLOG] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[WS_REMINDER_ERRORLOG]  WITH CHECK ADD  CONSTRAINT [FK_WS_REMINDER_ERRORLOG_CMS_CASE_FILE_STATUS_G2G_LKP] FOREIGN KEY([CmsCaseFileStatusId])
REFERENCES [dbo].[CMS_CASE_FILE_STATUS_G2G_LKP] ([Id])
GO
ALTER TABLE [dbo].[WS_REMINDER_ERRORLOG] CHECK CONSTRAINT [FK_WS_REMINDER_ERRORLOG_CMS_CASE_FILE_STATUS_G2G_LKP]
GO
ALTER TABLE [dbo].[WS_REMINDER_ERRORLOG]  WITH CHECK ADD  CONSTRAINT [FK_WS_REMINDER_ERRORLOG_CMS_COMS_REMINDER_TYPE] FOREIGN KEY([ReminderTypeId])
REFERENCES [dbo].[CMS_COMS_REMINDER_TYPE] ([Id])
GO
ALTER TABLE [dbo].[WS_REMINDER_ERRORLOG] CHECK CONSTRAINT [FK_WS_REMINDER_ERRORLOG_CMS_COMS_REMINDER_TYPE]
GO
ALTER TABLE [dbo].[WS_REMINDER_ERRORLOG]  WITH CHECK ADD  CONSTRAINT [FK_WS_REMINDER_ERRORLOG_CMS_DRAFT_DOCUMENT_STATUS] FOREIGN KEY([DraftTemplateVersionStatusId])
REFERENCES [dbo].[CMS_DRAFT_DOCUMENT_STATUS] ([Id])
GO
ALTER TABLE [dbo].[WS_REMINDER_ERRORLOG] CHECK CONSTRAINT [FK_WS_REMINDER_ERRORLOG_CMS_DRAFT_DOCUMENT_STATUS]
GO
ALTER TABLE [dbo].[WS_REMINDER_ERRORLOG]  WITH CHECK ADD  CONSTRAINT [FK_WS_REMINDER_ERRORLOG_COMM_COMMUNICATION_TYPE] FOREIGN KEY([CommunicationTypeId])
REFERENCES [dbo].[COMM_COMMUNICATION_TYPE] ([CommunicationTypeId])
GO
ALTER TABLE [dbo].[WS_REMINDER_ERRORLOG] CHECK CONSTRAINT [FK_WS_REMINDER_ERRORLOG_COMM_COMMUNICATION_TYPE]
GO
ALTER TABLE [dbo].[WS_REMINDER_ERRORLOG]  WITH CHECK ADD  CONSTRAINT [FK_WS_REMINDER_ERRORLOG_WS_CMS_COMS_REMINDER] FOREIGN KEY([ReminderId])
REFERENCES [dbo].[WS_CMS_COMS_REMINDER] ([ID])
GO
ALTER TABLE [dbo].[WS_REMINDER_ERRORLOG] CHECK CONSTRAINT [FK_WS_REMINDER_ERRORLOG_WS_CMS_COMS_REMINDER]
GO
ALTER TABLE [dbo].[WS_REMINDER_ERRORLOG]  WITH CHECK ADD  CONSTRAINT [FK_WS_REMINDER_ERRORLOG_WS_PROCESS_TYPE_LKP] FOREIGN KEY([ProcessTypeId])
REFERENCES [dbo].[WS_PROCESS_TYPE_LKP] ([Id])
GO
ALTER TABLE [dbo].[WS_REMINDER_ERRORLOG] CHECK CONSTRAINT [FK_WS_REMINDER_ERRORLOG_WS_PROCESS_TYPE_LKP]
GO
ALTER TABLE [dbo].[WS_REMINDER_ERRORLOG]  WITH CHECK ADD  CONSTRAINT [FK_WS_REMINDER_ERRORLOG_WS_WORKERSERVICES_LKP] FOREIGN KEY([WorkerServiceId])
REFERENCES [dbo].[WS_WORKERSERVICES_LKP] ([Id])
GO
ALTER TABLE [dbo].[WS_REMINDER_ERRORLOG] CHECK CONSTRAINT [FK_WS_REMINDER_ERRORLOG_WS_WORKERSERVICES_LKP]
GO
-------------------------------------------------------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[WS_REMINDER_PROCESSLOG]') AND type in (N'U'))
DROP TABLE [dbo].[WS_REMINDER_PROCESSLOG]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WS_REMINDER_PROCESSLOG](
	[Id] [uniqueidentifier] NOT NULL,
	[WorkerServiceId] [int] NULL,
	[ReceiverId] [nvarchar](256) NULL,
	[Sender] [nvarchar](256) NULL,
	[ProcessTypeId] [int] NULL,
	[ReminderId] [int] NULL,
	[ReminderTypeId] [int] NULL,
	[CommunicationTypeId] [int] NULL,
	[DraftTemplateVersionStatusId] [int] NULL,
	[CmsCaseFileStatusId] [int] NULL,
	[Description] [nvarchar](256) NULL,
	[IsNotification] [bit] NULL,
	[IsTask] [bit] NULL,
	[CreatedBy] [nvarchar](256) NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedBy] [nvarchar](256) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](256) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted] [bit] NULL,
	[IsActive] [bit] NULL,
	[IsFirstReminder] [bit] NULL,
	[IsSecondReminder] [bit] NULL,
	[IsThirdReminder] [bit] NULL,
	[ReferenceId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_WS_REMINDER_PROCESSLOG] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[WS_REMINDER_PROCESSLOG]  WITH CHECK ADD  CONSTRAINT [FK_WS_REMINDER_PROCESSLOG_CMS_CASE_FILE_STATUS_G2G_LKP] FOREIGN KEY([CmsCaseFileStatusId])
REFERENCES [dbo].[CMS_CASE_FILE_STATUS_G2G_LKP] ([Id])
GO
ALTER TABLE [dbo].[WS_REMINDER_PROCESSLOG] CHECK CONSTRAINT [FK_WS_REMINDER_PROCESSLOG_CMS_CASE_FILE_STATUS_G2G_LKP]
GO
ALTER TABLE [dbo].[WS_REMINDER_PROCESSLOG]  WITH CHECK ADD  CONSTRAINT [FK_WS_REMINDER_PROCESSLOG_CMS_COMS_REMINDER_TYPE] FOREIGN KEY([ReminderTypeId])
REFERENCES [dbo].[CMS_COMS_REMINDER_TYPE] ([Id])
GO
ALTER TABLE [dbo].[WS_REMINDER_PROCESSLOG] CHECK CONSTRAINT [FK_WS_REMINDER_PROCESSLOG_CMS_COMS_REMINDER_TYPE]
GO
ALTER TABLE [dbo].[WS_REMINDER_PROCESSLOG]  WITH CHECK ADD  CONSTRAINT [FK_WS_REMINDER_PROCESSLOG_CMS_DRAFT_DOCUMENT_STATUS] FOREIGN KEY([DraftTemplateVersionStatusId])
REFERENCES [dbo].[CMS_DRAFT_DOCUMENT_STATUS] ([Id])
GO
ALTER TABLE [dbo].[WS_REMINDER_PROCESSLOG] CHECK CONSTRAINT [FK_WS_REMINDER_PROCESSLOG_CMS_DRAFT_DOCUMENT_STATUS]
GO
ALTER TABLE [dbo].[WS_REMINDER_PROCESSLOG]  WITH CHECK ADD  CONSTRAINT [FK_WS_REMINDER_PROCESSLOG_COMM_COMMUNICATION_TYPE] FOREIGN KEY([CommunicationTypeId])
REFERENCES [dbo].[COMM_COMMUNICATION_TYPE] ([CommunicationTypeId])
GO
ALTER TABLE [dbo].[WS_REMINDER_PROCESSLOG] CHECK CONSTRAINT [FK_WS_REMINDER_PROCESSLOG_COMM_COMMUNICATION_TYPE]
GO
ALTER TABLE [dbo].[WS_REMINDER_PROCESSLOG]  WITH CHECK ADD  CONSTRAINT [FK_WS_REMINDER_PROCESSLOG_WS_CMS_COMS_REMINDER] FOREIGN KEY([ReminderId])
REFERENCES [dbo].[WS_CMS_COMS_REMINDER] ([ID])
GO
ALTER TABLE [dbo].[WS_REMINDER_PROCESSLOG] CHECK CONSTRAINT [FK_WS_REMINDER_PROCESSLOG_WS_CMS_COMS_REMINDER]
GO
ALTER TABLE [dbo].[WS_REMINDER_PROCESSLOG]  WITH CHECK ADD  CONSTRAINT [FK_WS_REMINDER_PROCESSLOG_WS_PROCESS_TYPE_LKP] FOREIGN KEY([ProcessTypeId])
REFERENCES [dbo].[WS_PROCESS_TYPE_LKP] ([Id])
GO
ALTER TABLE [dbo].[WS_REMINDER_PROCESSLOG] CHECK CONSTRAINT [FK_WS_REMINDER_PROCESSLOG_WS_PROCESS_TYPE_LKP]
GO
ALTER TABLE [dbo].[WS_REMINDER_PROCESSLOG]  WITH CHECK ADD  CONSTRAINT [FK_WS_REMINDER_PROCESSLOG_WS_WORKERSERVICES_LKP] FOREIGN KEY([WorkerServiceId])
REFERENCES [dbo].[WS_WORKERSERVICES_LKP] ([Id])
GO
ALTER TABLE [dbo].[WS_REMINDER_PROCESSLOG] CHECK CONSTRAINT [FK_WS_REMINDER_PROCESSLOG_WS_WORKERSERVICES_LKP]
GO
----------------------------------------------------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[WS_PUBLIC_HOLIDAYS]') AND type in (N'U'))
DROP TABLE [dbo].[WS_PUBLIC_HOLIDAYS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WS_PUBLIC_HOLIDAYS](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [HolidayDate] [datetime] NOT NULL,
    [Description] [nvarchar](500) NOT NULL,
    [HolidayEnumValue] [int] NOT NULL,
    [IsActive] [bit] NOT NULL,
    [CreatedBy] [nvarchar](250) NOT NULL,
    [CreatedDate] [datetime] NOT NULL,
    [ModifiedBy] [nvarchar](250) NULL,
    [ModifiedDate] [datetime] NULL,
    [DeletedBy] [nvarchar](250) NULL,
    [DeletedDate] [datetime] NULL,
    [IsDeleted] [bit] NOT NULL,
CONSTRAINT [PK_WS_Public_Holidays] PRIMARY KEY CLUSTERED
(
    [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
--------------------------------------------------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[WS_WORKERSERVICE_EXECUTION]') AND type in (N'U'))
DROP TABLE [dbo].[WS_WORKERSERVICE_EXECUTION]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WS_WORKERSERVICE_EXECUTION](
	[Id] [uniqueidentifier] NOT NULL,
	[StartDateTime] [datetime] NULL,
	[EndDateTime] [datetime] NULL,
	[ExecutionStatusId] [int] NULL,
	[ReAttemptCount] [int] NULL,
	[ExecutionDetails] [nvarchar](256) NULL,
	[WorkerServiceId] [int] NOT NULL,
 CONSTRAINT [PK_WS_WORKERSERVICE_EXECUTION] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[WS_WORKERSERVICE_EXECUTION]  WITH CHECK ADD  CONSTRAINT [FK_WS_WORKERSERVICE_EXECUTION_WS_WORKERSERVICES_LKP] FOREIGN KEY([WorkerServiceId])
REFERENCES [dbo].[WS_WORKERSERVICES_LKP] ([Id])
GO
ALTER TABLE [dbo].[WS_WORKERSERVICE_EXECUTION] CHECK CONSTRAINT [FK_WS_WORKERSERVICE_EXECUTION_WS_WORKERSERVICES_LKP]
GO
-------------------------------------------------------------Worker Service Table Script End----------------------------------------------------------------
----------------------------------------- 26-01-2024 ----------------------------------------
CREATE TABLE EP_CONTACT_TYPE(
	Id int PRIMARY KEY identity(1,1),
	Name_En nvarchar(150) NULL,
	Name_Ar nvarchar(150) NULL,
	IsActive bit null,
	CreatedBy nvarchar(100) not null,
	CreatedDate datetime not null,
	ModifiedBy nvarchar(100) null,
	ModifiedDate datetime null,
	DeletedBy nvarchar(100) null,
	DeletedDate datetime null,
	IsDeleted bit null default 0
)
GO
CREATE TABLE EP_CONTACT_INFORMATION(
	Id int PRIMARY KEY identity(1,1),
	UserId nvarchar(450)FOREIGN KEY REFERENCES EP_PERSONAL_INFORMATION(UserId),
	ContectTypeId int FOREIGN KEY REFERENCES EP_CONTACT_TYPE(Id),
	IsPrimary bit null,
	IsActive bit null,
	CreatedBy nvarchar(100) not null,
	CreatedDate datetime not null,
	ModifiedBy nvarchar(100) null,
	ModifiedDate datetime null,
	DeletedBy nvarchar(100) null,
	DeletedDate datetime null,
	IsDeleted bit null default 0
)
GO

CREATE TABLE EP_GRADE_TYPE(
	Id int PRIMARY KEY identity(1,1),
	Name_En nvarchar(150) NULL,
	Name_Ar nvarchar(150) NULL,
	CreatedBy nvarchar(100) not null,
	CreatedDate datetime not null,
	ModifiedBy nvarchar(100) null,
	ModifiedDate datetime null,
	DeletedBy nvarchar(100) null,
	DeletedDate datetime null,
	IsDeleted bit null default 0
)
GO
CREATE TABLE EP_CONTRACT_TYPE(
	Id int PRIMARY KEY identity(1,1),
	Name_En nvarchar(150) NULL,
	Name_Ar nvarchar(150) NULL,
	CreatedBy nvarchar(100) not null,
	CreatedDate datetime not null,
	ModifiedBy nvarchar(100) null,
	ModifiedDate datetime null,
	DeletedBy nvarchar(100) null,
	DeletedDate datetime null,
	IsDeleted bit null default 0
)
GO

----------------------------------------- 26-01-2024 ----------------------------------------

-------Ammaar Naveed-----26/01/2024
ALTER TABLE EP_CONTACT_TYPE
ALTER COLUMN Name_Ar NVARCHAR(50)
------------------------------------

-------Ammaar Naveed-------30/01/2024
ALTER TABLE EP_CONTACT_INFORMATION  
DROP CONSTRAINT PK__EP_CONTA__3214EC07261D417F;   
GO  
ALTER TABLE EP_CONTACT_INFORMATION
DROP COLUMN Id
ALTER TABLE EP_CONTACT_INFORMATION
ADD Id UNIQUEIDENTIFIER NOT NULL
ALTER TABLE EP_CONTACT_INFORMATION ADD PRIMARY KEY (Id)
-------------------------------------------------------
----Ihsaan Abbas
-----Alter Script for two column 
ALTER TABLE CMS_CHAMBER_G2G_LKP
DROP CONSTRAINT DF__CMS_CHAMB__Addre__7E22B05D; 

---alter table allow null
ALTER TABLE CMS_CHAMBER_G2G_LKP
ALTER COLUMN Address NVARCHAR(255) NULL;

----allow null to number column 
ALTER TABLE CMS_CHAMBER_G2G_LKP
ALTER COLUMN Number NVARCHAR(255) NULL;


--------Ammaar Naveed-----11/02/2024----UMS_USER_ROLES----Transaction Based Implementation
ALTER TABLE UMS_USER_ROLES
ADD CreatedBy NVARCHAR(150) NULL DEFAULT 'hremployee@fatwa.gov.kw'

ALTER TABLE UMS_USER_ROLES
ADD CreatedDate DATETIME NULL DEFAULT '0000-00-00 00:00:00.000'

ALTER TABLE UMS_USER_ROLES
ADD ModifiedBy NVARCHAR(150) NULL

ALTER TABLE UMS_USER_ROLES
ADD ModifiedDate DATETIME NULL

ALTER TABLE UMS_USER_ROLES
ADD DeletedBy NVARCHAR(150) NULL

ALTER TABLE UMS_USER_ROLES
ADD DeletedDate DATETIME NULL

ALTER TABLE UMS_USER_ROLES
ADD IsDeleted BIT NULL DEFAULT 0
-------------------------------------------------------------------------------------------

--------Ammaar Naveed-----11/02/2024----UMS_USER_ROLES----Creation of Composite Key
ALTER TABLE UMS_USER_ROLES 
ADD CONSTRAINT PK_Composite_RoleId_UserId 
PRIMARY KEY (UserId, RoleId);
-----------------------------------------------------------------------------------

--------Ammaar Naveed-----11/02/2024----UMS_USER_GROUP----Transaction Based Implementation
ALTER TABLE UMS_USER_GROUP
ADD CreatedBy NVARCHAR(150) NULL DEFAULT 'hremployee@fatwa.gov.kw'

ALTER TABLE UMS_USER_GROUP
ADD CreatedDate DATETIME NULL DEFAULT '0000-00-00 00:00:00.000'

ALTER TABLE UMS_USER_GROUP
ADD ModifiedBy NVARCHAR(150) NULL

ALTER TABLE UMS_USER_GROUP
ADD ModifiedDate DATETIME NULL

ALTER TABLE UMS_USER_GROUP
ADD DeletedBy NVARCHAR(150) NULL

ALTER TABLE UMS_USER_GROUP
ADD DeletedDate DATETIME NULL

ALTER TABLE UMS_USER_GROUP
ADD IsDeleted BIT NULL DEFAULT 0
-------------------------------------------------------------------------------------------

CREATE TABLE CMS_EXECUTION_ANOUNCEMENT
(
Id uniqueidentifier NOT NULL PRIMARY KEY,
ExecutionId uniqueidentifier NOT NULL,
AnouncementStatusId INT NOT NULL,
AnouncementTypeId INT NOT NULL,
PersonToBeanounced NVARCHAR(500) NULL,
ProcedureDate DATETIME NULL,
[CreatedBy] [nvarchar](256) NOT NULL,
[CreatedDate] [datetime] NOT NULL,
[ModifiedBy] [nvarchar](256) NULL,
[ModifiedDate] [datetime] NULL,
[DeletedBy] [nvarchar](256) NULL,
[DeletedDate] [datetime] NULL,
[IsDeleted] [bit] NOT NULL,
)


ALTER TABLE [dbo].CMS_EXECUTION_ANOUNCEMENT  WITH CHECK ADD  CONSTRAINT CMS_EXECUTION_ANOUNCEMENT_EXECUTION FOREIGN KEY(ExecutionId)
REFERENCES [dbo].CMS_EXECUTION (Id)
GO

-------------------------------------------------------------------------------------------

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_EXECUTION_PARTY_LINK]') AND type in (N'U'))
DROP TABLE [dbo].[CMS_EXECUTION_PARTY_LINK]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CMS_EXECUTION_PARTY_LINK](
    [Id] uniqueidentifier PRIMARY KEY,
    Name NVARCHAR(500) NOT NULL,
    [CreatedBy] [nvarchar](256) NOT NULL,
    [CreatedDate] [datetime] NOT NULL,
    [ModifiedBy] [nvarchar](256) NULL,
    [ModifiedDate] [datetime] NULL,
    [DeletedBy] [nvarchar](256) NULL,
    [DeletedDate] [datetime] NULL,
    [IsDeleted] [bit] NOT NULL
)




IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_MOJ_RPA_PAYLOAD]') AND type in (N'U'))
DROP TABLE [dbo].[CMS_MOJ_RPA_PAYLOAD]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CMS_MOJ_RPA_PAYLOAD](
    [Id] uniqueidentifier PRIMARY KEY,
    Payload NVARCHAR(MAX),
    [CreatedBy] [nvarchar](256) NOT NULL,
    [CreatedDate] [datetime] NOT NULL,
    [ModifiedBy] [nvarchar](256) NULL,
    [ModifiedDate] [datetime] NULL,
    [DeletedBy] [nvarchar](256) NULL,
    [DeletedDate] [datetime] NULL,
    [IsDeleted] [bit] NOT NULL
)


---Ammaar Naveed---22/02/2024-----Employee Floor and Building Implementation
ALTER TABLE CMS_OPERATING_SECTOR_TYPE_G2G_LKP
DROP COLUMN FloorNumber

-----Table_Floor
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FLOOR_FTW_LKP]') AND type in (N'U'))
DROP TABLE [dbo].[dbo].[FLOOR_FTW_LKP]
GO
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FLOOR_FTW_LKP](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name_En] [varchar](50) NOT NULL,
	[Name_Ar] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

---Table_Building
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BUILDING_FTW_LKP]') AND type in (N'U'))
DROP TABLE [dbo].[dbo].[BUILDING_FTW_LKP]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BUILDING_FTW_LKP](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name_En] [varchar](50) NOT NULL,
	[Name_Ar] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

--Table_EP_Floor_Building
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EP_FLOOR_BUILDING]') AND type in (N'U'))
DROP TABLE [dbo].[dbo].[EP_FLOOR_BUILDING]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EP_FLOOR_BUILDING](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FloorId] [int] NOT NULL,
	[BuildingId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[EP_FLOOR_BUILDING]  WITH CHECK ADD FOREIGN KEY([BuildingId])
REFERENCES [dbo].[BUILDING_FTW_LKP] ([Id])
GO

ALTER TABLE [dbo].[EP_FLOOR_BUILDING]  WITH CHECK ADD FOREIGN KEY([FloorId])
REFERENCES [dbo].[FLOOR_FTW_LKP] ([Id])
GO

--Alter EP_Employement Information for Employee Placement Implementation
ALTER TABLE EP_EMPLOYMENT_INFORMATION
ADD EmployeePlacementId INT NULL
ALTER TABLE EP_EMPLOYMENT_INFORMATION
ADD FOREIGN KEY(EmployeePlacementId) REFERENCES EP_FLOOR_BUILDING(Id);



IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_CHAMBER_OPERATING_SECTOR]') AND type in (N'U'))
DROP TABLE [dbo].[CMS_CHAMBER_OPERATING_SECTOR]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE CMS_CHAMBER_OPERATING_SECTOR
(
	Id INT IDENTITY(1,1) PRIMARY KEY,
	ChamberId INT NOT NULL,
	SectorTypeId INT NOT NULL,
    [CreatedBy] [nvarchar](256) NOT NULL,
    [CreatedDate] [datetime] NOT NULL,
    [ModifiedBy] [nvarchar](256) NULL,
    [ModifiedDate] [datetime] NULL,
    [DeletedBy] [nvarchar](256) NULL,
    [DeletedDate] [datetime] NULL,
    [IsDeleted] [bit] NOT NULL
)

ALTER TABLE [dbo].[CMS_CHAMBER_OPERATING_SECTOR]  WITH CHECK ADD  CONSTRAINT [FK_CMS_CHAMBER_OPERATING_SECTOR_CHAMBER] FOREIGN KEY([ChamberId])
REFERENCES [dbo].[CMS_CHAMBER_G2G_LKP] ([Id])
GO

ALTER TABLE [dbo].[CMS_CHAMBER_OPERATING_SECTOR]  WITH CHECK ADD  CONSTRAINT [FK_CMS_CHAMBER_OPERATING_SECTOR_SECTOR] FOREIGN KEY([SectorTypeId])
REFERENCES [dbo].[CMS_OPERATING_SECTOR_TYPE_G2G_LKP] ([Id])
GO

GO

/****** Object:  Table [dbo].[MOJ_IMAGE_DOCUMENT]    Script Date: 21/02/2024 11:53:48 am ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].MOJ_IMAGE_DOCUMENT]') AND type in (N'U'))
DROP TABLE [dbo].[MOJ_IMAGE_DOCUMENT]
GO

/****** Object:  Table [dbo].[MOJ_IMAGE_DOCUMENT]    Script Date: 27/02/2024 2:11:05 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MOJ_IMAGE_DOCUMENT](
	[Id] [uniqueidentifier] NOT NULL,
	[CANNumber] [nvarchar](1000) NULL,
	[CaseNumber] [nvarchar](1000) NULL,
	[AttachmentTypeId] [int] NOT NULL,
	[DocumentDate] [date] NULL,
	[FileName] [nvarchar](500) NOT NULL,
	[StoragePath] [nvarchar](max) NOT NULL,
	[CreatedBy] [nvarchar](256) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedBy] [nvarchar](256) NULL,
	[ModifiedDate] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
	[DeletedBy] [nvarchar](256) NULL,
	[DeletedDate] [datetime] NULL,
 CONSTRAINT [PK_MOJ_IMAGE_DOCUMENT] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
-----Ihsaan Abbas ----Court Chamber ,Chamber Chamber Nubmer --------03-Mar-2024---
---Drop column Script 
ALTER TABLE CMS_CHAMBER_NUMBER_G2G_LKP
DROP CONSTRAINT FK_CHAMBER_NUMBER_CHAMBER

ALTER TABLE CMS_CHAMBER_NUMBER_G2G_LKP 
DROP COLUMN chamberId;

--Allow Null

ALTER TABLE CMS_CHAMBER_NUMBER_G2G_LKP
ALTER COLUMN Code NVARCHAR(255) NULL;

-- Step 1: Drop the constraint
ALTER TABLE CMS_CHAMBER_G2G_LKP
DROP CONSTRAINT CMS_CHAMBER_COURT;

-- Step 2: Drop the column
ALTER TABLE CMS_CHAMBER_G2G_LKP
DROP COLUMN CourtId;

-----Table Script 
----Court Chamber 
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_COURT_CHAMBER]') AND type in (N'U'))
DROP TABLE [dbo].[CMS_COURT_CHAMBER]
GO

/****** Object:  Table [dbo].[CMS_COURT_CHAMBER]    Script Date: 12/02/2024 4:14:50 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CMS_COURT_CHAMBER](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CourtId] [int] NOT NULL,
	[ChamberId] [int] NOT NULL,
	[CreatedBy] [nvarchar](256) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](256) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](256) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[CMS_COURT_CHAMBER]  WITH CHECK ADD  CONSTRAINT [FK_CMS_COURT_CHAMBER] FOREIGN KEY([ChamberId])
REFERENCES [dbo].[CMS_CHAMBER_G2G_LKP] ([Id])
GO

ALTER TABLE [dbo].[CMS_COURT_CHAMBER] CHECK CONSTRAINT [FK_CMS_COURT_CHAMBER]
GO

ALTER TABLE [dbo].[CMS_COURT_CHAMBER]  WITH CHECK ADD  CONSTRAINT [FK_CMS_COURT_CHAMBERS] FOREIGN KEY([CourtId])
REFERENCES [dbo].[CMS_COURT_G2G_LKP] ([Id])
GO

ALTER TABLE [dbo].[CMS_COURT_CHAMBER] CHECK CONSTRAINT [FK_CMS_COURT_CHAMBERS]
GO

-----Chamber Chamber Number 
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_CHAMBER_CHAMBER_NUMBER]') AND type in (N'U'))
DROP TABLE [dbo].[CMS_CHAMBER_CHAMBER_NUMBER]
GO

/****** Object:  Table [dbo].[CMS_CHAMBER_CHAMBER_NUMBER]    Script Date: 12/02/2024 4:25:34 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CMS_CHAMBER_CHAMBER_NUMBER](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ChamberId] [int] NOT NULL,
	[ChamberNumberId] [int] NOT NULL,
	[CreatedBy] [nvarchar](256) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](256) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](256) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[CMS_CHAMBER_CHAMBER_NUMBER]  WITH CHECK ADD  CONSTRAINT [FK_CMS_CHAMBER_CHAMBER_NUMBER] FOREIGN KEY([ChamberNumberId])
REFERENCES [dbo].[CMS_CHAMBER_NUMBER_G2G_LKP] ([Id])
GO

ALTER TABLE [dbo].[CMS_CHAMBER_CHAMBER_NUMBER] CHECK CONSTRAINT [FK_CMS_CHAMBER_CHAMBER_NUMBER]
GO

ALTER TABLE [dbo].[CMS_CHAMBER_CHAMBER_NUMBER]  WITH CHECK ADD  CONSTRAINT [FK_CMS_CHAMBER_CHAMBER_NUMBERS] FOREIGN KEY([ChamberId])
REFERENCES [dbo].[CMS_CHAMBER_G2G_LKP] ([Id])
GO

ALTER TABLE [dbo].[CMS_CHAMBER_CHAMBER_NUMBER] CHECK CONSTRAINT [FK_CMS_CHAMBER_CHAMBER_NUMBERS]
GO

-----CMS Hearing Days 
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_HEARING_DAY]') AND type in (N'U'))
DROP TABLE [dbo].[CMS_HEARING_DAY]
GO
GO
/****** Object:  Table [dbo].[CMS_HEARING_DAY]    Script Date: 27/02/2024 4:50:22 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CMS_HEARING_DAY](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[NameEn] [varchar](50) NOT NULL,
	[NameAr] [nvarchar](50) NOT NULL,
	[CreatedBy] [nvarchar](256) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](256) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](256) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
CONSTRAINT [PK_CMS_HEARING_DAY] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

-------Chamber Chamber Number Hearing 

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_CHAMBER_NUMBER_HEARING]') AND type in (N'U'))
DROP TABLE [dbo].[CMS_CHAMBER_NUMBER_HEARING]
GO
GO
/****** Object:  Table [dbo].[CMS_CHAMBER_NUMBER_HEARING]    Script Date: 27/02/2024 4:40:18 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CMS_CHAMBER_NUMBER_HEARING](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[HearingDayId] [int] NOT NULL,
	[ChamberNumberId] [int] NOT NULL,
	[CreatedBy] [nvarchar](256) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](256) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](256) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[CMS_CHAMBER_CHAMBER_NUMBER]  WITH NOCHECK ADD  CONSTRAINT [FK_CMS_CHAMBER_CHAMBER_NUMBERS] FOREIGN KEY([ChamberNumberId])
REFERENCES [dbo].[CMS_CHAMBER_NUMBER_G2G_LKP] ([Id])
GO
ALTER TABLE [dbo].[CMS_CHAMBER_CHAMBER_NUMBER] CHECK CONSTRAINT [FK_CMS_CHAMBER_CHAMBER_NUMBER]
GO
ALTER TABLE [dbo].[CMS_CHAMBER_CHAMBER_NUMBER]  WITH NOCHECK ADD  CONSTRAINT [FK_CMS_CHAMBER_CHAMBER_NUMBERS] FOREIGN KEY([ChamberId])
REFERENCES [dbo].[CMS_CHAMBER_G2G_LKP] ([Id])
GO
ALTER TABLE [dbo].[CMS_CHAMBER_CHAMBER_NUMBER] CHECK CONSTRAINT [FK_CMS_CHAMBER_CHAMBER_NUMBERS]
GO
ALTER TABLE [dbo].[CMS_CHAMBER_NUMBER_HEARING]  WITH CHECK ADD  CONSTRAINT [FK_CMS_CHAMBER_NUMBER_HEARING_CMS_CHAMBER_NUMBER_G2G_LKP] FOREIGN KEY([ChamberNumberId])
REFERENCES [dbo].[CMS_CHAMBER_NUMBER_G2G_LKP] ([Id])
GO
ALTER TABLE [dbo].[CMS_CHAMBER_NUMBER_HEARING] CHECK CONSTRAINT [FK_CMS_CHAMBER_NUMBER_HEARING_CMS_CHAMBER_NUMBER_G2G_LKP]
GO


--Ammaar Naveed----05/03/2024-------Removal of EmployeePlacement information and transfer of building/floor to FATWA Admin Sector Types

--Dropp constraint -> This constraint name is random, please cross check your constraint name and drop accordingly.
ALTER TABLE EP_EMPLOYMENT_INFORMATION
DROP CONSTRAINT FK__EP_EMPLOY__Emplo__5CCCA98A;

ALTER TABLE EP_EMPLOYMENT_INFORMATION
DROP COLUMN EmployeePlacementId

--Adding constraint and BuildingId in SectorType
ALTER TABLE CMS_OPERATING_SECTOR_TYPE_G2G_LKP
ADD BuildingId INT NOT NULL DEFAULT 1
ALTER TABLE CMS_OPERATING_SECTOR_TYPE_G2G_LKP
ADD CONSTRAINT FK_SEC_BUILDING
FOREIGN KEY (BuildingId) REFERENCES BUILDING_FTW_LKP(Id)

--Adding constraint and FloorId in SectorType
ALTER TABLE CMS_OPERATING_SECTOR_TYPE_G2G_LKP
ADD FloorId INT NOT NULL DEFAULT 2
ALTER TABLE CMS_OPERATING_SECTOR_TYPE_G2G_LKP
ADD CONSTRAINT FK_SEC_FLOOR
FOREIGN KEY (FloorId) REFERENCES FLOOR_FTW_LKP(Id)

--Drop relational table for floor and building
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EP_FLOOR_BUILDING]') AND type in (N'U'))
DROP TABLE EP_FLOOR_BUILDING
GO
/*<History Author='IHSAAN ABBAS' Date='11-03-2024'> CMS_BANK GOVT ENTITY TABLE  </History>*/

/****** Object:  Table [dbo].[CMS_BANK_G2G_LKP]    Script Date: 11/03/2024 3:57:38 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CMS_BANK_G2G_LKP](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name_En] [varchar](50) NOT NULL,
	[Name_Ar] [nvarchar](50) NOT NULL,
	[CreatedBy] [nvarchar](256) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](256) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](256) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_CMS_BANK_G2G_LKP] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
------

/****** Object:  Table [dbo].[CMS_BANK_GOVERNMENT_ENTITY_G2G_LKP]    Script Date: 11/03/2024 4:28:58 pm ******/

SET QUOTED_IDENTIFIER ON
GO 
CREATE TABLE [dbo].[CMS_BANK_GOVERNMENT_ENTITY_G2G_LKP](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BankId] [int] NOT NULL,
	[GovtEntityId] [int] NOT NULL,
	IBAN VARCHAR(34),
	[CreatedBy] [nvarchar](256) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](256) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](256) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[CMS_BANK_GOVERNMENT_ENTITY_G2G_LKP]  WITH CHECK ADD  CONSTRAINT [FK_CMS_BANK_G2G_LKP] FOREIGN KEY([BankId])
REFERENCES [dbo].[CMS_BANK_G2G_LKP] ([Id])
GO

ALTER TABLE [dbo].[CMS_BANK_GOVERNMENT_ENTITY_G2G_LKP] CHECK CONSTRAINT [FK_CMS_BANK_G2G_LKP]
GO

ALTER TABLE [dbo].[CMS_BANK_GOVERNMENT_ENTITY_G2G_LKP]  WITH CHECK ADD  CONSTRAINT [FK_CMS_GOVERNMENT_ENTITY_G2G_LKP] FOREIGN KEY([GovtEntityId])
REFERENCES [dbo].[CMS_GOVERNMENT_ENTITY_G2G_LKP] ([EntityId])
GO

ALTER TABLE [dbo].[CMS_BANK_GOVERNMENT_ENTITY_G2G_LKP] CHECK CONSTRAINT [FK_CMS_GOVERNMENT_ENTITY_G2G_LKP]
GO
GO
--------------------------End IHSAAN ABBAS -------------------------

CREATE TABLE CMS_CASE_ANNOUNCEMENT
(
	Id UNIQUEIDENTIFIER PRIMARY KEY,
	CaseId UNIQUEIDENTIFIER NOT NULL,
	PartyId UNIQUEIDENTIFIER NOT NULL,
	AnouncementNumber NVARCHAR(20),
	AnouncementType NVARCHAR(500),
	PersonToBeanounced NVARCHAR(500),
	HearingDate datetime,
	DistributionStatus NVARCHAR(500),
	AnouncementMakingDate datetime,
	AnouncementGoOutDate datetime,
	ActualAnouncementDate datetime,
	LastUpdateDate datetime,
	AnouncementResult NVARCHAR(Max),
	Reason NVARCHAR(Max),
	[CreatedBy] [nvarchar](256) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](256) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](256) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
)
GO
--------------------------------------------

--Ammaar Naveed----10/03/2024-------Updating/Syncing ModuleEnums and implementation of ModuleId in database table
ALTER TABLE UMS_CLAIM 
ADD ModuleId INT NULL

--Ammaar Naveed----13/03/2024-------Making building and floor column in sector type as NOT NULL
ALTER TABLE CMS_OPERATING_SECTOR_TYPE_G2G_LKP DROP CONSTRAINT FK_SEC_BUILDING
ALTER TABLE CMS_OPERATING_SECTOR_TYPE_G2G_LKP DROP CONSTRAINT FK_SEC_FLOOR
ALTER TABLE CMS_OPERATING_SECTOR_TYPE_G2G_LKP DROP COLUMN BuildingId 
ALTER TABLE CMS_OPERATING_SECTOR_TYPE_G2G_LKP DROP COLUMN FloorId 

ALTER TABLE CMS_OPERATING_SECTOR_TYPE_G2G_LKP
ADD BuildingId INT NOT NULL DEFAULT 1
ALTER TABLE CMS_OPERATING_SECTOR_TYPE_G2G_LKP
ADD CONSTRAINT FK_SEC_BUILDING
FOREIGN KEY(BuildingId) REFERENCES BUILDING_FTW_LKP(Id)

ALTER TABLE CMS_OPERATING_SECTOR_TYPE_G2G_LKP
ADD FloorId INT NOT NULL DEFAULT 1
ALTER TABLE CMS_OPERATING_SECTOR_TYPE_G2G_LKP
ADD CONSTRAINT FK_SEC_FLOOR
FOREIGN KEY(FloorId) REFERENCES FLOOR_FTW_LKP(Id)

--Ammaar Naveed--14/03/2024--Removing LastLoggedIn column from UMS user for revised implementation.
ALTER TABLE UMS_USER
DROP COLUMN LastLoggedIn
--Ammaar Naveed--14/03/2024--Table to track user activity.
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UMS_USER_ACTIVITY]') AND type in (N'U'))
DROP TABLE [dbo].[UMS_USER_ACTIVITY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[UMS_USER_ACTIVITY](
	[ActivityId] [uniqueidentifier] NOT NULL,
	[UserId] [nvarchar](450) NULL,
	[UserName] [nvarchar](256) NOT NULL,
	[ComputerName] [nvarchar](100) NOT NULL,
	[IPAddress] [nvarchar](500) NOT NULL,
	[IsLoggedIn] [bit] NULL,
	[LoginDateTime] [datetime] NULL,
	[IsLoggedOut] [bit] NULL,
	[LogoutDateTime] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[ActivityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[UMS_USER_ACTIVITY] ADD  DEFAULT ('192.168.200.140') FOR [IPAddress]
GO

ALTER TABLE [dbo].[UMS_USER_ACTIVITY]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[UMS_USER] ([Id])
GO
--Ammaar Naveed--14/03/2024--Alteraion in table to record exceptions.
ALTER TABLE UMS_USER_ACTIVITY
ADD ExceptionMessage NVARCHAR(500) NULL
ALTER TABLE UMS_USER_ACTIVITY
ALTER COLUMN UserName NVARCHAR(256) NULL
/*<History Author='Ihsaan Abbas' Date='24-03-2024'> UMS lookup Alter Script Add Metadata </History>*/
----Alter Script
Alter table  EP_NATIONALITY
add [CreatedBy] [nvarchar](256) default 'fatwaadmin@gmail.com' NOT NULL,
	[CreatedDate] [datetime] default '2023-08-13 17:01:00.000'NOT NULL,
	[ModifiedBy] [nvarchar](256) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](256) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted] [bit]  Default 0 NOT NULL,
	[IsActive] [bit] default 1 NOT NULL

 
 ----EP_GRADE
 Alter table  EP_GRADE
add [CreatedBy] [nvarchar](256) default 'fatwaadmin@gmail.com' NOT NULL,
	[CreatedDate] [datetime] default '2023-08-13 17:01:00.000'NOT NULL,
	[ModifiedBy] [nvarchar](256) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](256) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted] [bit]  Default 0 NOT NULL,
	[IsActive] [bit] default 1 NOT NULL

	----- EP Gender
 Alter table  EP_Gender
add [CreatedBy] [nvarchar](256) default 'fatwaadmin@gmail.com' NOT NULL,
	[CreatedDate] [datetime] default '2023-08-13 17:01:00.000'NOT NULL,
	[ModifiedBy] [nvarchar](256) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](256) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted] [bit]  Default 0 NOT NULL,
	[IsActive] [bit] default 1 NOT NULL
	------------------End---------------------
-------------- 26-3-2024 --------------------

Drop table NOTIF_NOTIFICATION_TYPE;
Drop table NOTIF_NOTIFICATION_TEMPLATE;
Drop table NOTIF_NOTIFICATION_CATEGORY;
Drop table NOTIF_NOTIFICATION_USER;
Drop table NOTIF_NOTIFICATION_LINK;
Drop table NOTIF_NOTIFICATION;


CREATE TABLE NOTIF_NOTIFICATION_STATUS_LKP (
StatusId int primary key,
NameEn nvarchar(100),
NameAr nvarchar(100),
);
insert into NOTIF_NOTIFICATION_STATUS_LKP
select * from NOTIF_NOTIFICATION_STATUS;
Drop table NOTIF_NOTIFICATION_STATUS;

CREATE TABLE NOTIF_NOTIFICATION_CHANNEL_LKP (
ChannelId int primary key,
NameEn nvarchar(100),
NameAr nvarchar(100),
);
insert into NOTIF_NOTIFICATION_CHANNEL_LKP
select CommunicationId, NameEn, NameAr from NOTIF_NOTIFICATION_COMMUNICATION_METHOD;
Drop table NOTIF_NOTIFICATION_COMMUNICATION_METHOD;

Create table NOTIF_NOTIFICATION_RECEIVER_TYPE_LKP (
ReceiverTypeId int primary key,
ReceiverTypeName nvarchar(100)
);
insert into NOTIF_NOTIFICATION_RECEIVER_TYPE_LKP
select * from FATWA_DB_DEV.dbo.NOTIF_NOTIFICATION_RECEIVER_TYPE;
Drop table NOTIF_NOTIFICATION_RECEIVER_TYPE;

Create table NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP (
PlaceHolderId int primary key,
PlaceHolderName nvarchar(100),
EventId int foreign key references NOTIF_NOTIFICATION_EVENT(EventId)
);

CREATE TABLE NOTIF_NOTIFICATION_TEMPLATE (
	TemplateId uniqueidentifier NOT NULL primary key,
	EventId int foreign key references NOTIF_NOTIFICATION_EVENT (EventId),
	ChannelId int foreign key references NOTIF_NOTIFICATION_CHANNEL_LKP (ChannelId),
	NameEn nvarchar(100) NULL,
	NameAr nvarchar(100) NULL,
	SubjectEn nvarchar(150) NULL,
	SubjectAr nvarchar(150) NULL,
	BodyEn nvarchar(1000) NULL,
	BodyAr nvarchar(1000) NULL,
	Footer nvarchar(1000) NULL,
	CreatedBy nvarchar(100) NOT NULL,
	CreatedDate datetime NOT NULL,
	ModifiedBy nvarchar(100) NULL,
	ModifiedDate datetime NULL,
	DeletedBy nvarchar(100) NULL,
	DeletedDate datetime NULL,
	IsDeleted bit NULL,
	constraint Event_Channel_UQK Unique(EventId,ChannelId) 
);

CREATE TABLE NOTIF_NOTIFICATION (
	NotificationId uniqueidentifier NOT NULL,
	DueDate datetime,
	ReadDate datetime,
	SenderId nvarchar(450) NULL,
	ReceiverId nvarchar(450) NULL,
	NotificationURL nvarchar(450) NULL,
	ModuleId int NOT NULL,
	NotificationMessageEn nvarchar(max),
	NotificationMessageAr nvarchar(max),
	NotificationStatusId int foreign key references NOTIF_NOTIFICATION_STATUS_LKP (StatusId),
	NotificationTemplateId uniqueidentifier foreign key references NOTIF_NOTIFICATION_TEMPLATE (TemplateId),
	CreatedBy nvarchar(100) not null ,
	CreatedDate datetime not null ,
	ModifiedBy nvarchar(100) null,
	ModifiedDate datetime null,
	DeletedBy nvarchar(100) null,
	DeletedDate datetime null,
	IsDeleted bit null default 0
);
GO


ALTER TABLE CMS_TEMPLATE 
ADD IsTimeStamp bit 
DEFAULT 0 NOT NULL;
---------------
CREATE TABLE CMS_DRAFT_STAMP
(
[Id] INT IDENTITY(1,1) PRIMARY KEY,
[Content] NVARCHAR(MAX),
[CreatedBy] [nvarchar](256) NOT NULL,
[CreatedDate] [datetime] NOT NULL,
[ModifiedBy] [nvarchar](256) NULL,
[ModifiedDate] [datetime] NULL,
[DeletedBy] [nvarchar](256) NULL,
[DeletedDate] [datetime] NULL,
[IsDeleted] [bit] NOT NULL,
)
-----------
--Muhammad Ismail--30/03/2024--Aletering the following table Columns datatype( Hijri datetime to datetime2 ) .
-- Table LEGAL_LEGISLATION, LEGAL_PUBLICATION_SOURCE, and LEGAL_PRINCIPLE

GO

/****** Object:  Table [dbo].[LEGAL_PUBLICATION_SOURCE]    Script Date: 30/03/2024 3:13:18 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LEGAL_PUBLICATION_SOURCE](
	[SourceId] [int] IDENTITY(1,1) NOT NULL,
	[PublicationNameId] [int] NOT NULL,
	[LegislationId] [uniqueidentifier] NOT NULL,
	[Issue_Number] [nvarchar](100) NOT NULL,
	[PublicationDate] [datetime] NOT NULL,
	[PublicationDate_Hijri] [datetime2](7) NULL,
	[Page_Start] [int] NOT NULL,
	[Page_End] [int] NOT NULL,
 CONSTRAINT [PK_LEGAL_PUBLICATION_SOURCE] PRIMARY KEY CLUSTERED 
(
	[SourceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

-----------------------------------------------

GO

/****** Object:  Table [dbo].[LEGAL_LEGISLATION]    Script Date: 30/03/2024 3:10:58 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LEGAL_LEGISLATION](
	[LegislationId] [uniqueidentifier] NOT NULL,
	[Legislation_Type] [int] NOT NULL,
	[Legislation_Number] [nvarchar](100) NULL,
	[Introduction] [nvarchar](max) NULL,
	[IssueDate] [datetime] NULL,
	[IssueDate_Hijri] [datetime2](7) NULL,
	[LegislationTitle] [nvarchar](max) NULL,
	[Legislation_Comment] [nvarchar](max) NULL,
	[LegislationYear] [nvarchar](100) NULL,
	[LegislationRemark] [nvarchar](max) NULL,
	[Legislation_Status] [int] NOT NULL,
	[Legislation_Flow_Status] [int] NOT NULL,
	[StartDate] [datetime] NULL,
	[AddedBy] [nvarchar](100) NOT NULL,
	[AddedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](100) NULL,
	[ModifiedDate] [datetime] NULL,
	[CanceledBy] [nvarchar](100) NULL,
	[CanceledDate] [datetime] NULL,
	[DeletedBy] [nvarchar](100) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
	[UserId] [nvarchar](450) NULL,
	[RoleId] [nvarchar](450) NULL,
 CONSTRAINT [PK_LEGAL_LEGISLATION] PRIMARY KEY CLUSTERED 
(
	[LegislationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

-------------------------------------------------------

GO

/****** Object:  Table [dbo].[LEGAL_PRINCIPLE]    Script Date: 30/03/2024 3:06:33 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LEGAL_PRINCIPLE](
	[PrincipleId] [uniqueidentifier] NOT NULL,
	[Principle_Type] [int] NOT NULL,
	[Principle_Number] [int] NOT NULL,
	[IssueDate] [datetime] NOT NULL,
	[IssueDate_Hijri] [datetime2](7) NULL,
	[Introduction] [nvarchar](max) NULL,
	[Summary] [nvarchar](max) NULL,
	[Conclusion] [nvarchar](max) NULL,
	[Notes] [nvarchar](max) NULL,
	[PrincipleTitle] [nvarchar](500) NULL,
	[Principle_Comment] [nvarchar](max) NULL,
	[Principle_Status] [int] NULL,
	[Principle_Flow_Status] [int] NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[AddedBy] [nvarchar](100) NOT NULL,
	[AddedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](100) NULL,
	[ModifiedDate] [datetime] NULL,
	[CanceledBy] [nvarchar](100) NULL,
	[CanceledDate] [datetime] NULL,
	[DeletedBy] [nvarchar](100) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
	[UserId] [nvarchar](450) NULL,
	[RoleId] [nvarchar](450) NULL,
	[CategoryId] [int] NULL,
	[SubCategoryId] [int] NULL,
 CONSTRAINT [PK_LEGAL_PRINCIPLE] PRIMARY KEY CLUSTERED 
(
	[PrincipleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
-------------- 31 -03-2024-------
 CREATE TABLE COMM_STOP_EXECUTION_REJECTION_REASON(Id UNIQUEIDENTIFIER not null, CommunicationId UNIQUEIDENTIFIER not null, Reason NVARCHAR(MAX) )



CREATE TABLE CMS_DRAFTED_TEMPLATE_VERSION_LOGS
(
[Id] uniqueidentifier NOT NULL PRIMARY KEY,
[VersionId] uniqueidentifier NOT NULL,
[UserId] uniqueidentifier NOT NULL,
[ActionId] int NOT NULL,
[CreatedBy] [nvarchar](256) NULL,
[CreatedDate] [datetime] NOT NULL,
[ModifiedBy] [nvarchar](256) NULL,
[ModifiedDate] [datetime] NULL,
[DeletedBy] [nvarchar](256) NULL,
[DeletedDate] [datetime] NULL,
[IsDeleted] [bit] NOT NULL,
)
--------
CREATE TABLE CMS_DRAFT_ACTION_ENUM
(
[ActionId] int NOT NULL PRIMARY KEY,
[NameEn] [nvarchar](256) NULL,
[NameAr] [nvarchar](256) NULL
)
---------- 
ALTER TABLE EP_DEPARTMENT
ADD Borrow_Return_Day_Duration INT Not Null Default 14  ;
------------------------------------
----------

------------------------------------------Inventory Management Tables----------------------------------------------------

GO
CREATE TABLE [dbo].[INV_SERVICE_REQUEST_TYPE_OSS_LKP](
	[Id] [int] NOT NULL,
	[NameEn] [nvarchar](500) NOT NULL,
	[NameAr] [nchar](500) NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_INV_SERVICE_REQUEST_TYPE_OSS_LKP] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
-----------------------------------------Inventory Management Tables End---------------------------------------------------
ALTER TABLE CMS_OPERATING_SECTOR_TYPE_G2G_LKP 
ADD IsOnlyViceHosApprovalRequired BIT NOT NULL DEFAULT 0;

ALTER TABLE CMS_OPERATING_SECTOR_TYPE_G2G_LKP 
ADD IsViceHosResponsibleForAllLawyers BIT NOT NULL DEFAULT 0;
--------------
-----------------------------------------Inventory Management Tables End---------------------------------------------------

CREATE TABLE CMS_HEARING_ROLL_OUTCOME_DRAFT_PAYLOAD
(
Id UNIQUEIDENTIFIER PRIMARY KEY NOT NULL,
HearingRollId INT NOT NULL,
Payload NVARCHAR(MAX) NOT NULL,
[CreatedBy] [nvarchar](256) NOT NULL,
[CreatedDate] [datetime] NOT NULL,
[ModifiedBy] [nvarchar](256) NULL,
[ModifiedDate] [datetime] NULL,
[DeletedBy] [nvarchar](256) NULL,
[DeletedDate] [datetime] NULL,
[IsDeleted] [bit] NOT NULL
)


/****** Object:  Table [dbo].[COMM_Communication_Tarasol_Rpa_Payload]    Script Date: 16/04/2024 8:05:50 pm ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[COMM_Communication_Tarasol_Rpa_Payload]') AND type in (N'U'))
DROP TABLE [dbo].[COMM_Communication_Tarasol_Rpa_Payload]
GO

/****** Object:  Table [dbo].[COMM_Communication_Tarasol_Rpa_Payload]    Script Date: 16/04/2024 8:05:50 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[COMM_Communication_Tarasol_Rpa_Payload](
	[Id] [uniqueidentifier] NOT NULL,
	[Payload] [nvarchar](max) NULL,
	[CreatedBy] [nvarchar](256) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](256) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](256) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO 
/*<History Author='Ihsaan Abbas' Date='18-04-2024'> Alter Table script of LMS_LITERATURE_AUTHOR Meta data add</History>*/  
Alter table  LMS_LITERATURE_AUTHOR
add [CreatedBy] [nvarchar](256) Default 'fatwaadmin@gmail.com' NOT NULL,
	[CreatedDate] [datetime] Default '2023-08-13 17:01:00.000'NOT NULL,
	[ModifiedBy] [nvarchar](256) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](256) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted] [bit]  Default 0 NOT NULL
	----------------End------------------
GO

------------- Legal Principle Table Scripts 2024-04-21 ---------------

/*<History Author='Umer Zaman' Date='16-04-2024'> LLS Legal Principle new model </History>*/
/******  
<Object Scope='Public'> LLS_LEGAL_PRINCIPLE  
<Copyright> Digital Processing System </Copyright>  
<Author Company='Business Analytics'> Umer Zaman </Author>  
<Created> 16-04-2024 </Created>
</Object>  
******/

/****** Object:  Table [dbo].[LLS_LEGAL_PRINCIPLE]    Script Date: 16/04/2024 11:31:56 am ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LLS_LEGAL_PRINCIPLE]') AND type in (N'U'))
DROP TABLE [dbo].[LLS_LEGAL_PRINCIPLE]
GO

/****** Object:  Table [dbo].[LLS_LEGAL_PRINCIPLE]    Script Date: 16/04/2024 11:31:56 am ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LLS_LEGAL_PRINCIPLE](
	[PrincipleId] [uniqueidentifier] NOT NULL,
	[PrincipleNumber] [int] NOT NULL,
	[PrincipleSourceDocumentTypeId] [int] NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NULL,
	[PrincipleContent] [nvarchar](max) NOT NULL,
	[PageNumber] [int] NOT NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](100) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](100) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL
 CONSTRAINT [PK_LLS_LEGAL_PRINCIPLE] PRIMARY KEY CLUSTERED 
(
	[PrincipleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

----------------------

/*<History Author='Umer Zaman' Date='16-04-2024'> LLS Legal Principle new model </History>*/
/******  
<Object Scope='Public'> LLS_LEGAL_PRINCIPLE_SOURCE_DOCUMENT_REFERENCE  
<Copyright> Digital Processing System </Copyright>  
<Author Company='Business Analytics'> Umer Zaman </Author>  
<Created> 16-04-2024 </Created>
</Object>  
******/

/****** Object:  Table [dbo].[LLS_LEGAL_PRINCIPLE_SOURCE_DOCUMENT_REFERENCE]    Script Date: 16/04/2024 11:31:56 am ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LLS_LEGAL_PRINCIPLE_SOURCE_DOCUMENT_REFERENCE]') AND type in (N'U'))
DROP TABLE [dbo].[LLS_LEGAL_PRINCIPLE_SOURCE_DOCUMENT_REFERENCE]
GO

/****** Object:  Table [dbo].[LLS_LEGAL_PRINCIPLE_SOURCE_DOCUMENT_REFERENCE]    Script Date: 16/04/2024 11:31:56 am ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LLS_LEGAL_PRINCIPLE_SOURCE_DOCUMENT_REFERENCE](
	[ReferenceId] [int] IDENTITY(1,1) NOT NULL,
	[PrincipleId] [uniqueidentifier] NOT NULL,
	[UploadedDocumentId] [int] NOT NULL,
	[IsMaskJudgment] [bit] NOT NULL,
	[MaskedJudgmentId] [int] NOT NULL
 CONSTRAINT [PK_LLS_LEGAL_PRINCIPLE_SOURCE_DOCUMENT_REFERENCE] PRIMARY KEY CLUSTERED 
(
	[ReferenceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
)
GO

-------------
/*<History Author='Umer Zaman' Date='16-04-2024'> LLS Legal Principle new model </History>*/
/******  
<Object Scope='Public'> LLS_LEGAL_PRINCIPLE_CATEGORY_FTW_LKP  
<Copyright> Digital Processing System </Copyright>  
<Author Company='Business Analytics'> Umer Zaman </Author>  
<Created> 16-04-2024 </Created>
</Object>  
******/

/****** Object:  Table [dbo].[LLS_LEGAL_PRINCIPLE_CATEGORY_FTW_LKP]    Script Date: 16/04/2024 11:31:56 am ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LLS_LEGAL_PRINCIPLE_CATEGORY_FTW_LKP]') AND type in (N'U'))
DROP TABLE [dbo].[LLS_LEGAL_PRINCIPLE_CATEGORY_FTW_LKP]
GO

/****** Object:  Table [dbo].[LLS_LEGAL_PRINCIPLE_CATEGORY_FTW_LKP]    Script Date: 16/04/2024 11:31:56 am ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LLS_LEGAL_PRINCIPLE_CATEGORY_FTW_LKP](
	[CategoryId] [int] IDENTITY(1,1) NOT NULL,
	[ParentId] [int] NULL,
	[Name] [nvarchar] (500) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](256) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](256) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](256) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_LLS_LEGAL_PRINCIPLE_CATEGORY_FTW_LKP] PRIMARY KEY CLUSTERED 
(
	[CategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
)
GO

---------

/*<History Author='Umer Zaman' Date='16-04-2024'> LLS Legal Principle new model </History>*/
/******  
<Object Scope='Public'> LLS_LEGAL_PRINCIPLE_LLS_LEGAL_PRINCIPLE_CATEGORY  
<Copyright> Digital Processing System </Copyright>  
<Author Company='Business Analytics'> Umer Zaman </Author>  
<Created> 16-04-2024 </Created>
</Object>  
******/

/****** Object:  Table [dbo].[LLS_LEGAL_PRINCIPLE_LLS_LEGAL_PRINCIPLE_CATEGORY]    Script Date: 16/04/2024 11:31:56 am ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LLS_LEGAL_PRINCIPLE_LLS_LEGAL_PRINCIPLE_CATEGORY]') AND type in (N'U'))
DROP TABLE [dbo].[LLS_LEGAL_PRINCIPLE_LLS_LEGAL_PRINCIPLE_CATEGORY]
GO

/****** Object:  Table [dbo].[LLS_LEGAL_PRINCIPLE_LLS_LEGAL_PRINCIPLE_CATEGORY]    Script Date: 16/04/2024 11:31:56 am ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LLS_LEGAL_PRINCIPLE_LLS_LEGAL_PRINCIPLE_CATEGORY](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PrincipleId] [uniqueidentifier] NOT NULL,
	[CategoryId] [int] NOT NULL
 CONSTRAINT [PK_LLS_LEGAL_PRINCIPLE_LLS_LEGAL_PRINCIPLE_CATEGORY] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
)
GO

-----

/*<History Author='Umer Zaman' Date='16-04-2024'> LLS Legal Principle new model </History>*/
/******  
<Object Scope='Public'> LLS_LEGAL_PRINCIPLE_REFERENCE_PRINCIPLE  
<Copyright> Digital Processing System </Copyright>  
<Author Company='Business Analytics'> Umer Zaman </Author>  
<Created> 16-04-2024 </Created>
</Object>  
******/

/****** Object:  Table [dbo].[LLS_LEGAL_PRINCIPLE_REFERENCE_PRINCIPLE]    Script Date: 16/04/2024 11:31:56 am ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LLS_LEGAL_PRINCIPLE_REFERENCE_PRINCIPLE]') AND type in (N'U'))
DROP TABLE [dbo].[LLS_LEGAL_PRINCIPLE_REFERENCE_PRINCIPLE]
GO

/****** Object:  Table [dbo].[LLS_LEGAL_PRINCIPLE_REFERENCE_PRINCIPLE]    Script Date: 16/04/2024 11:31:56 am ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LLS_LEGAL_PRINCIPLE_REFERENCE_PRINCIPLE](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PrincipleId] [uniqueidentifier] NOT NULL,
	[ReferencePrincipleId] [uniqueidentifier] NOT NULL,
	[PageNumber] [int] NOT NULL
 CONSTRAINT [PK_LLS_LEGAL_PRINCIPLE_REFERENCE_PRINCIPLE] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
)
GO

------------- Legal Principle Table Scripts 2024-04-21 ---------------

---------23-04-2024-------
CREATE or alter  procedure [dbo].[pBellNotifications]          
(           
 @UserId nvarchar(50) = NULL  ,      
 @notificationStatusId  int = NULL            
)        
AS                  
BEGIN                   
 SELECT DISTINCT nf.NotificationId           
     , nf.NotificationMessageEn         
	 , nf.NotificationMessageAr         
     , nf.NotificationURL as Url        
     , nf.CreatedDate as CreationDate
	 , nne.NameEn EventNameEn
	 , nne.NameAr EventNameAr
 FROM NOTIF_NOTIFICATION nf         
 INNER JOIN NOTIF_NOTIFICATION_TEMPLATE nft on nft.TemplateId = nf.NotificationTemplateId    
 INNER JOIN NOTIF_NOTIFICATION_EVENT nne on nft.EventId= nne.EventId
 WHERE nf.IsDeleted = 0          
 AND nft.ChannelId = 4 --'Browser'             
 AND (nf.ReceiverId = @UserId OR @UserId IS NULL OR @UserId = '')            
 AND (nf.NotificationStatusId = @notificationStatusId OR @notificationStatusId IS NULL OR @notificationStatusId = '')          
 ORDER BY nf.CreatedDate Desc             
END 

/*<History Author='Arshad khan' Date='25-04-2024'> </History>*/
------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
----------------------------------------------               BUG REPORTING  START            ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------



/****** Object:  Table [dbo].[BUG_APPLICATION_G2G_LKP]    Script Date: 25/04/2024 6:25:48 pm ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BUG_APPLICATION_G2G_LKP]') AND type in (N'U'))
DROP TABLE [dbo].[BUG_APPLICATION_G2G_LKP]
GO

/****** Object:  Table [dbo].[BUG_APPLICATION_G2G_LKP]    Script Date: 25/04/2024 6:25:48 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BUG_APPLICATION_G2G_LKP](
	[Id] [int] NOT NULL,
	[Name_En] [nvarchar](100) NOT NULL,
	[Name_Ar] [nvarchar](100) NOT NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](100) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](100) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK__BUG_APPL__3214EC07076F2E25] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO




/****** Object:  Table [dbo].[BUG_ISSUE_TYPE_G2G_LKP]    Script Date: 25/04/2024 6:45:53 pm ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BUG_ISSUE_TYPE_G2G_LKP]') AND type in (N'U'))
DROP TABLE [dbo].[BUG_ISSUE_TYPE_G2G_LKP]
GO

/****** Object:  Table [dbo].[BUG_ISSUE_TYPE_G2G_LKP]    Script Date: 25/04/2024 6:45:53 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BUG_ISSUE_TYPE_G2G_LKP](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Type_En] [nvarchar](100) NOT NULL,
	[Type_Ar] [nvarchar](100) NOT NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](100) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](100) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK__BUG_ISSU__3214EC076D73E482] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO



/****** Object:  Table [dbo].[BUG_MODULE_G2G_LKP]    Script Date: 25/04/2024 6:46:25 pm ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BUG_MODULE_G2G_LKP]') AND type in (N'U'))
DROP TABLE [dbo].[BUG_MODULE_G2G_LKP]
GO

/****** Object:  Table [dbo].[BUG_MODULE_G2G_LKP]    Script Date: 25/04/2024 6:46:25 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BUG_MODULE_G2G_LKP](
	[Id] [int] NOT NULL,
	[Name_En] [nvarchar](100) NOT NULL,
	[Name_Ar] [nvarchar](100) NOT NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](100) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](100) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK__BUG_MODU__3214EC07F9DAC7F6] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO




/****** Object:  Table [dbo].[BUG_SEVERITY_G2G_LKP]    Script Date: 25/04/2024 6:46:46 pm ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BUG_SEVERITY_G2G_LKP]') AND type in (N'U'))
DROP TABLE [dbo].[BUG_SEVERITY_G2G_LKP]
GO

/****** Object:  Table [dbo].[BUG_SEVERITY_G2G_LKP]    Script Date: 25/04/2024 6:46:46 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BUG_SEVERITY_G2G_LKP](
	[Id] [int] NOT NULL,
	[Value_En] [nvarchar](100) NOT NULL,
	[Value_Ar] [nvarchar](100) NOT NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](100) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](100) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK__BUG_SEVE__3214EC07607C4C33] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO




/****** Object:  Table [dbo].[BUG_STATUS_G2G_LKP]    Script Date: 25/04/2024 6:47:05 pm ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BUG_STATUS_G2G_LKP]') AND type in (N'U'))
DROP TABLE [dbo].[BUG_STATUS_G2G_LKP]
GO

/****** Object:  Table [dbo].[BUG_STATUS_G2G_LKP]    Script Date: 25/04/2024 6:47:05 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BUG_STATUS_G2G_LKP](
	[Id] [int] NOT NULL,
	[Value_En] [nvarchar](100) NOT NULL,
	[Value_Ar] [nvarchar](100) NOT NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](100) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](100) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK__BUG_STAT__3214EC07D8705EE7] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

--------------------------------------- BUG Reported
/****** Object:  Table [dbo].[BUG_REPORTED]    Script Date: 01/May/24 12:29:10 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BUG_REPORTED]') AND type in (N'U'))
DROP TABLE [dbo].[BUG_REPORTED]
GO

/****** Object:  Table [dbo].[BUG_REPORTED]    Script Date: 01/May/24 12:29:10 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BUG_REPORTED](
	[Id] [uniqueidentifier] NOT NULL,
	[PrimaryBugId] [nvarchar](256) NOT NULL,
	[ApplicationId] [int] NULL,
	[ModuleId] [int] NULL,
	[ScreenReference] [nvarchar](max) NULL,
	[TypeId] [int] NULL,
	[Description] [nvarchar](250) NULL,
	[ShortNumber] [int] NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](100) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](100) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_BUG_REPORTED] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


--------------------------------------------------BUG_TICKET

/****** Object:  Table [dbo].[BUG_TICKET]    Script Date: 01/May/24 1:46:21 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BUG_TICKET]') AND type in (N'U'))
DROP TABLE [dbo].[BUG_TICKET]
GO

/****** Object:  Table [dbo].[BUG_TICKET]    Script Date: 01/May/24 1:46:21 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BUG_TICKET](
	[Id] [uniqueidentifier] NOT NULL,
	[TicketId] [nvarchar](256) NULL,
	[BugId] [uniqueidentifier] NULL,
	[ApplicationId] [int] NULL,
	[ModuleId] [int] NULL,
	[IssueTypeId] [int] NULL,
	[Description] [nvarchar](250) NULL,
	[SeverityId] [int] NULL,
	[StatusId] [int] NULL,
	[AssignTo] [nvarchar](100) NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](100) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](100) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
	[ResolutionDate] [datetime] NULL,
	[ReportedBy] [nvarchar](100) NULL,
	[ShortNumber] [int] NULL,
	[PriorityId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[BUG_TICKET]  WITH CHECK ADD  CONSTRAINT [FK_Application] FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[BUG_APPLICATION_G2G_LKP] ([Id])
GO

ALTER TABLE [dbo].[BUG_TICKET] CHECK CONSTRAINT [FK_Application]
GO

ALTER TABLE [dbo].[BUG_TICKET]  WITH CHECK ADD  CONSTRAINT [FK_Bug_Reported] FOREIGN KEY([BugId])
REFERENCES [dbo].[BUG_REPORTED] ([Id])
GO

ALTER TABLE [dbo].[BUG_TICKET] CHECK CONSTRAINT [FK_Bug_Reported]
GO

ALTER TABLE [dbo].[BUG_TICKET]  WITH CHECK ADD  CONSTRAINT [FK_BugStatus] FOREIGN KEY([StatusId])
REFERENCES [dbo].[BUG_STATUS_G2G_LKP] ([Id])
GO

ALTER TABLE [dbo].[BUG_TICKET] CHECK CONSTRAINT [FK_BugStatus]
GO

ALTER TABLE [dbo].[BUG_TICKET]  WITH CHECK ADD  CONSTRAINT [FK_Module] FOREIGN KEY([ModuleId])
REFERENCES [dbo].[BUG_MODULE_G2G_LKP] ([Id])
GO

ALTER TABLE [dbo].[BUG_TICKET] CHECK CONSTRAINT [FK_Module]
GO

ALTER TABLE [dbo].[BUG_TICKET]  WITH CHECK ADD  CONSTRAINT [Fk_Priority] FOREIGN KEY([PriorityId])
REFERENCES [dbo].[CMS_PRIORITY_G2G_LKP] ([Id])
GO

ALTER TABLE [dbo].[BUG_TICKET] CHECK CONSTRAINT [Fk_Priority]
GO

ALTER TABLE [dbo].[BUG_TICKET]  WITH CHECK ADD  CONSTRAINT [FK_Severity] FOREIGN KEY([SeverityId])
REFERENCES [dbo].[BUG_SEVERITY_G2G_LKP] ([Id])
GO

ALTER TABLE [dbo].[BUG_TICKET] CHECK CONSTRAINT [FK_Severity]
GO

-------------------------------------------------- BUG_TICKET_STATUS_HISTORY
GO
/****** Object:  Table [dbo].[BUG_TICKET_STATUS_HISTORY]    Script Date: 01/May/24 1:35:08 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BUG_TICKET_STATUS_HISTORY]') AND type in (N'U'))
DROP TABLE [dbo].[BUG_TICKET_STATUS_HISTORY]
GO

/****** Object:  Table [dbo].[BUG_TICKET_STATUS_HISTORY]    Script Date: 01/May/24 1:35:08 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BUG_TICKET_STATUS_HISTORY](
	[HistoryId] [uniqueidentifier] NOT NULL,
	[TicketId] [uniqueidentifier] NULL,
	[StatusId] [int] NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[Remarks] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[HistoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[BUG_TICKET_STATUS_HISTORY]  WITH CHECK ADD  CONSTRAINT [Fk_Status] FOREIGN KEY([StatusId])
REFERENCES [dbo].[BUG_STATUS_G2G_LKP] ([Id])
GO

ALTER TABLE [dbo].[BUG_TICKET_STATUS_HISTORY] CHECK CONSTRAINT [Fk_Status]
GO

ALTER TABLE [dbo].[BUG_TICKET_STATUS_HISTORY]  WITH CHECK ADD  CONSTRAINT [Fk_Ticket] FOREIGN KEY([TicketId])
REFERENCES [dbo].[BUG_TICKET] ([Id])
GO

ALTER TABLE [dbo].[BUG_TICKET_STATUS_HISTORY] CHECK CONSTRAINT [Fk_Ticket]
GO
--------------------------------BUG_COMMENT_FEEDBACK
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BUG_COMMENT_FEEDBACK]') AND type in (N'U'))
DROP TABLE [dbo].[BUG_COMMENT_FEEDBACK]
GO

/****** Object:  Table [dbo].[BUG_COMMENT_FEEDBACK]    Script Date: 07/05/2024 11:20:43 am ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BUG_COMMENT_FEEDBACK](
	[Id] [uniqueidentifier] NOT NULL,
	[ReferenceId] [uniqueidentifier] NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[Comment] [nvarchar](max) NULL,
	[DeletedBy] [nvarchar](100) NULL,
	[DeletedDate] [nvarchar](100) NULL,
	[IsDeleted] [bit] NOT NULL,
	[RemarkType] [int] NULL,
 CONSTRAINT [PK__BUG_COMM__3214EC07F22119A5] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

----------------------------Assign_Bug_Type_User 07/05/24
/****** Object:  Table [dbo].[BUG_ASSIGN_TYPE_USER]    Script Date: 08/05/2024 4:39:49 pm ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BUG_ASSIGN_TYPE_USER]') AND type in (N'U'))
DROP TABLE [dbo].[BUG_ASSIGN_TYPE_USER]
GO

/****** Object:  Table [dbo].[BUG_ASSIGN_TYPE_USER]    Script Date: 08/05/2024 4:39:49 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BUG_ASSIGN_TYPE_USER](
	[Id]  [uniqueidentifier]  NOT NULL,
	[BugTypeId] [int] NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[GroupId] [uniqueidentifier] NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](100) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](100) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL
 CONSTRAINT [PK__BUG_ASSIGN_TYPE_USER__3214EC07F22119A5] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
---------------------------------------12/05/24-----------BUG_ASSIGN_TYPE_MODULE
CREATE TABLE BUG_ASSIGN_TYPE_MODULE
(
Id uniqueidentifier primary key not null,
ApplicationId int not null,
ModuleId int not null,
BugTypeId int not null,
CreatedBy nvarchar(100) not null,
CreatedDate datetime not null,
ModifiedBy nvarchar(100) null,
ModifiedDate datetime null,
DeletedBy nvarchar null,
DeletedDate datetime null,
IsDeleted bit not null
)
--------------------------------BUG_ASSIGN_MODULE_APPLICATION
CREATE TABLE BUG_ASSIGN_MODULE_APPLICATION
(
Id uniqueidentifier primary key not null,
ApplicationId int not null,
ModuleId int not null,
CreatedBy nvarchar(100) not null,
CreatedDate datetime not null,
ModifiedBy nvarchar(100) null,
ModifiedDate datetime null,
DeletedBy nvarchar(100) null,
DeletedDate datetime null,
IsDeleted bit not null
)
---------------------
GO
/****** Object:  Table [dbo].[BUG_TICKET_ASSIGNMENT_HISTORY]    Script Date: 01/May/24 1:35:08 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BUG_TICKET_ASSIGNMENT_HISTORY]') AND type in (N'U'))
DROP TABLE [dbo].[BUG_TICKET_ASSIGNMENT_HISTORY]
GO

/****** Object:  Table [dbo].[BUG_TICKET_ASSIGNMENT_HISTORY]    Script Date: 01/May/24 1:35:08 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BUG_TICKET_ASSIGNMENT_HISTORY](
	[Id] [uniqueidentifier] NOT NULL,
	[HistoryId] [uniqueidentifier] NOT NULL,
	[UserId] [nvarchar](100) NULL,
	[GroupId] [uniqueidentifier] NULL,
    [AssignmentType] [int] NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] 
GO
-------------------------

/****** Object:  Table [dbo].[BUG_EVENT_G2G_LKP]   Script Date: 06/Jun/24 6:36:32 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BUG_EVENT_G2G_LKP]') AND type in (N'U'))
DROP TABLE [dbo].[BUG_EVENT_G2G_LKP]
GO

/****** Object:  Table [dbo].[BUG_EVENT_G2G_LKP]    Script Date: 06/Jun/24 6:36:32 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BUG_EVENT_G2G_LKP](
	[Id] [int] NOT NULL,
	[Name_En] [nvarchar](100) NOT NULL,
	[Name_Ar] [nvarchar](100) NOT NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](100) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](100) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK__BUG_EVENT__3214EC07D8705EE7] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
----------------------------------------------               BUG REPORTING  End            ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------










/*<History Author='Ihsaan Abbas' Date='25-04-2024'> Add column in CMS_GOVERNMENT_ENTITY_G2G_LKP </History>*/
----Add column in CMS_GOVERNMENT_ENTITY_G2G_LKP
Alter Table CMS_GOVERNMENT_ENTITY_G2G_LKP Add 
G2GSiteID  int Null


Alter Table CMS_GOVERNMENT_ENTITY_G2G_LKP Add 
G2GSiteCode  int   Null 
 
 ------Add column in CMS_GOVERNMENT_ENTITY_DEPARTMENT_G2G_LKP
Alter Table CMS_GOVERNMENT_ENTITY_DEPARTMENT_G2G_LKP Add
G2GBRSiteID int Null

Alter Table CMS_GOVERNMENT_ENTITY_DEPARTMENT_G2G_LKP Add 
DefaultReceiver Bit Default 0 not null

-----Add column in CMS_OPERATING_SECTOR_TYPE_G2G_LKP
Alter Table CMS_OPERATING_SECTOR_TYPE_G2G_LKP Add 
G2GBRSiteID int Null 
----------------------------End----------------

/*<History Author='Ihsaan Abbas' Date='29-04-2024'> Add table CMS_SECTOR_TYPE_GE_DEPARTMENT</History*/  
 GO 
/****** Object:  Table [dbo].[CMS_SECTOR_TYPE_GE_DEPARTMENT]    Script Date: 29/04/2024 11:00:21 am ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CMS_SECTOR_TYPE_GE_DEPARTMENT](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SectorTypeId] [int] NOT NULL,
	[DepartmentId] [int] NOT NULL,
	[CreatedBy] [nvarchar](256) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](256) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](256) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[CMS_SECTOR_TYPE_GE_DEPARTMENT]  WITH CHECK ADD  CONSTRAINT [FK_CMS_SECTOR_TYPE_GE_DEPARTMENT] FOREIGN KEY([DepartmentId])
REFERENCES [dbo].[CMS_GOVERNMENT_ENTITY_DEPARTMENT_G2G_LKP] ([Id])
GO

ALTER TABLE [dbo].[CMS_SECTOR_TYPE_GE_DEPARTMENT] CHECK CONSTRAINT [FK_CMS_SECTOR_TYPE_GE_DEPARTMENT]
GO

------ 30-4-2024 -----------

create table EmailConfiguration(
Id int primary key,
ApplicationId int not null,
ToEmail varchar(150) not null,
FromEmail varchar(150) not null,
EmailSubject varchar(500) not null,
EmailBody varchar(max) not null,
SmtpHost varchar(50) not null,
SmtpPort varchar(10) not null,
SmtpUser varchar(100) not null,
SmtpPass varchar(100) not null,
CreatedBy nvarchar(100) not null,
CreatedDate datetime not null,
ModifiedBy nvarchar(100) null,
ModifiedDate datetime null,
DeletedBy nvarchar(100) null,
DeletedDate datetime null,
IsDeleted bit null default 0
);

------ 30-4-2024 -----------
 

------------- Legal Principle Table Scripts 2024-04-21 ---------------

/*<History Author='Ammaar Naveed' Date='30-04-2024'> Created table for employee leave delegation information. </History>*/ 

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EP_EMPLOYEE_LEAVE_DELEGATION_INFORMATION]') AND type in (N'U'))
DROP TABLE [dbo].[EP_EMPLOYEE_LEAVE_DELEGATION_INFORMATION]
GO
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EP_EMPLOYEE_LEAVE_DELEGATION_INFORMATION](
	[Id] [uniqueidentifier] NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[FromDate] [date] NULL,
	[ToDate] [date] NULL,
	[DelegatedUserId] [nvarchar](450) NOT NULL,
	[CreatedBy] [nvarchar](256) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](256) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](256) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_EP_EMPLOYEE_LEAVE_DELEGATION_INFORMATION_Id] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[EP_EMPLOYEE_LEAVE_DELEGATION_INFORMATION]  WITH CHECK ADD  CONSTRAINT [FK_EP_EMPLOYEE_LEAVE_DELEGATION_INFORMATION_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[UMS_USER] ([Id])
GO

ALTER TABLE [dbo].[EP_EMPLOYEE_LEAVE_DELEGATION_INFORMATION] CHECK CONSTRAINT [FK_EP_EMPLOYEE_LEAVE_DELEGATION_INFORMATION_UserId]
GO
--------------------------------- Table-> Employee Leave Delegation Information---------------------------------------------------


--------------------------------- Table-> Employee Leave Delegation Information---------------------------------------------------

/*<History Author='Ihsaan Abbas' Date='05-05-2024'> Created table for pattern of literature . </History>*/
/****** Object:  Table [dbo].[LITERATURE_DEWEY_NUMBER_PATTERN_TYPE]    Script Date: 06/05/2024 4:24:17 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LITERATURE_DEWEY_NUMBER_PATTERN_TYPE](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PatternNameEn] [nvarchar](200) NOT NULL,
	[PatternNameAr] [nvarchar](200) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


/****** Object:  Table [dbo].[LITERATURE_DEWEY_NUMBER_PATTERN]    Script Date: 06/05/2024 4:23:51 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

Create  TABLE [dbo].[LITERATURE_DEWEY_NUMBER_PATTERN](
	[Id] [uniqueidentifier] NOT NULL,
	[PatternTypId] [int] NOT NULL,
	[SeriesNumber] [nvarchar](150) NOT NULL,
	[DigitSequenceNumber]  [nvarchar](256) NOT NULL,
	[SequenceResult] [nvarchar](max)  NULL,
	[SequenceFormatResult] [nvarchar](256) NULL,
	[CreatedBy] [nvarchar](256) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](256) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](256) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
	[IsActive] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[LITERATURE_DEWEY_NUMBER_PATTERN]  WITH CHECK ADD  CONSTRAINT [FK_LITERATURE_DEWEY_NUMBER_PATTERN] FOREIGN KEY([PatternTypId])
REFERENCES [dbo].[LITERATURE_DEWEY_NUMBER_PATTERN_TYPE] ([Id])
GO

ALTER TABLE [dbo].[LITERATURE_DEWEY_NUMBER_PATTERN] CHECK CONSTRAINT [FK_LITERATURE_DEWEY_NUMBER_PATTERN]
GO

---------------------------------------15/5/24--------------------------
DROP TABLE LLS_LEGAL_PRINCIPLE
DROP TABLE LLS_LEGAL_PRINCIPLE_LLS_LEGAL_PRINCIPLE_CATEGORY
DROP TABLE LLS_LEGAL_PRINCIPLE_REFERENCE_PRINCIPLE
DROP TABLE LLS_LEGAL_PRINCIPLE_SOURCE_DOCUMENT_REFERENCE

GO
CREATE TABLE [dbo].[LLS_LEGAL_PRINCIPLE](
	[PrincipleId] [uniqueidentifier] NOT NULL,
	[PrincipleNumber] [int] NOT NULL,
	[FlowStatus] [int] NOT NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](100) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](100) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
	[OriginalSourceDocumentId] [int] NOT NULL,
	[UserId] [nvarchar](50) NULL,
	[RoleId] [nvarchar](50) NULL,
	[Principle_Comment] [nvarchar](max) NULL,
 CONSTRAINT [PK_LLS_LEGAL_PRINCIPLE] PRIMARY KEY CLUSTERED 
(
	[PrincipleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[LLS_LEGAL_PRINCIPLE_CATEGORY_FTW_LKP](
	[CategoryId] [int] IDENTITY(1,1) NOT NULL,
	[ParentId] [int] NULL,
	[Name] [nvarchar](500) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](256) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](256) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](256) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_LLS_LEGAL_PRINCIPLE_CATEGORY_FTW_LKP] PRIMARY KEY CLUSTERED 
(
	[CategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
CREATE TABLE [dbo].[LLS_LEGAL_PRINCIPLE_CONTENT](
	[PrincipleContentId] [uniqueidentifier] NOT NULL,
	[PrincipleId] [uniqueidentifier] NOT NULL,
	[PrincipleContent] [nvarchar](max) NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NULL,
 CONSTRAINT [PK_LLS_LEGAL_PRINCIPLE_PRINCIPLECONTENT] PRIMARY KEY CLUSTERED 
(
	[PrincipleContentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
CREATE TABLE [dbo].[LLS_LEGAL_PRINCIPLE_CONTENT_CATEGORY](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PrincipleContentId] [uniqueidentifier] NOT NULL,
	[CategoryId] [int] NOT NULL,
 CONSTRAINT [PK_LLS_LEGAL_PRINCIPLE_LLS_LEGAL_PRINCIPLE_CATEGORY] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[LLS_LEGAL_PRINCIPLE_CONTENT_SOURCE_DOCUMENT_REFERENCE](
	[ReferenceId] [int] IDENTITY(1,1) NOT NULL,
	[PrincipleContentId] [uniqueidentifier] NOT NULL,
	[PageNumber] [int] NOT NULL,
	[OriginalSourceDocId] [int] NOT NULL,
	[CopySourceDocId] [int] NOT NULL,
	[IsMaskedJudgment] [bit] NOT NULL,
 CONSTRAINT [PK_LLS_LEGAL_PRINCIPLE_SOURCE_DOCUMENT_REFERENCE] PRIMARY KEY CLUSTERED 
(
	[ReferenceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
------------------------------------------------------------------------

/*<History Author='Ammaar Naveed' Date='19-05-2024'>Made ModuleId column required</History>*/
ALTER TABLE UMS_CLAIM
ALTER COLUMN ModuleId INT NOT NULL;


------------20/05/2024
GO

/****** Object:  Table [dbo].[CMS_CASE_USER_IMPORTANT]    Script Date: 20/05/2024 11:35:45 am ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CMS_CASE_USER_IMPORTANT](
	[Id] [uniqueidentifier] NOT NULL,
	[ReferenceId] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](100) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](100) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_CMS_CASE_USER_IMPORTANT] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


------------20/05/2024

--------------Muhammad Ali----------- 21 May 2024---------------
ALTER TABLE CMS_CHAMBER_NUMBER_HEARING
ADD HearingsRollDays INT NOT NULL DEFAULT 0,
JudgmentsRollDays INT NOT NULL DEFAULT 0;
------------------END-------------------------------------------

/*<History Author='Ammaar Naveed' Date='21-05-2024'>IsMojExtracted provision</History>*/
ALTER TABLE ATTACHMENT_TYPE
ADD IsMojExtracted BIT NOT NULL DEFAULT 0;


----------26/05/2024
/****** Object:  Table [dbo].[COMM_COMMUNICATION_Recipient]    Script Date: 26/05/2024 11:17:06 am ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[COMM_COMMUNICATION_Recipient](
	[Id] [uniqueidentifier] NOT NULL,
	[CommunicationId] [uniqueidentifier] NOT NULL,
	[RecipientId] [uniqueidentifier] NOT NULL,
	[CreatedBy] [nvarchar](100) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](100) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](100) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_COMM_COMMUNICATION_Recipient] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[COMM_COMMUNICATION_HISTORY]    Script Date: 26/05/2024 11:16:31 am ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[COMM_COMMUNICATION_HISTORY](
	[Id] [uniqueidentifier] NOT NULL,
	[ReferenceId] [uniqueidentifier] NOT NULL,
	[SentBy] [uniqueidentifier] NOT NULL,
	[SentTo] [uniqueidentifier] NOT NULL,
	[ActionId] [int] NULL,
	[CreatedBy] [nvarchar](100) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](100) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](100) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
	[Reason] [nvarchar](max) NULL,
 CONSTRAINT [PK_COMM_COMMUNICATION_HISTORY] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


/****** Object:  Table [dbo].[TARASOL_GE_FATWA_DEPARTMENTS]    Script Date: 26/05/2024 11:18:47 am ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TARASOL_GE_FATWA_DEPARTMENTS](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[G2GSiteId] [int] NOT NULL,
	[G2GBrSiteId] [int] NOT NULL,
	[G2GBrSiteNameEn] [nvarchar](max) NULL,
	[G2GBrSiteNameAr] [nvarchar](max) NULL,
 CONSTRAINT [PK_TARASOL_GE_DEPARTMENTS] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

 -------------Muhammad Ali--------ChamberNumberHearing Table  update--------02 June 2024---------
 ALTER TABLE CMS_CHAMBER_NUMBER_HEARING
ADD CourtId INT, ChamberId INT;

ALTER TABLE CMS_CHAMBER_NUMBER_HEARING
ADD CONSTRAINT FK_CMS_CHAMBER_NUMBER_HEARING_CMS_COURT_G2G_LKP
FOREIGN KEY (CourtId) REFERENCES CMS_COURT_G2G_LKP(Id);

ALTER TABLE CMS_CHAMBER_NUMBER_HEARING
ADD CONSTRAINT FK_CMS_CHAMBER_NUMBER_HEARING_CMS_CHAMBER_G2G_LKP
FOREIGN KEY (ChamberId) REFERENCES CMS_CHAMBER_G2G_LKP(Id);

---------------------End----------------------------------------


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LLS_Legal_PRINCIPLE_COMMENTS]') AND type in (N'U'))
DROP TABLE [dbo].[LLS_Legal_PRINCIPLE_COMMENTS]
GO

/****** Object:  Table [dbo].[LLS_Legal_PRINCIPLE_COMMENTS]    Script Date: 01/07/2022 11:31:56 am ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE LLS_Legal_PRINCIPLE_COMMENTS (
    CommentId uniqueidentifier not null ,
	PrincipleId uniqueidentifier not null,
    Comment nvarchar(255) not null,
    Reason nvarchar(255) null,
    Status varchar(255) not null,
    Createdby varchar(255) not null,
	CreatedDate datetime not null
	PRIMARY KEY (CommentId) 
);

------------------END-------------------------------------------


-----------------------------------------------------------
CREATE TABLE [dbo].[UMS_USER_DEVICE_TOKEN](
	[Id] [uniqueidentifier] NOT NULL,
	[DeviceToken] [nvarchar](max) NOT NULL,
	[UserId] [nvarchar](256) NULL,
	[CreatedBy] [nvarchar](256) NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedBy] [nvarchar](256) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](256) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
-----------
ALTER TABLE UMS_USER_DEVICE_TOKEN
ADD ChannelId INT NOT NULL DEFAULT 0
-----------
CREATE TABLE [dbo].[UMS_USER_APP_VERSION](
	[Id] INT NOT NULL,
	[ChannelId] INT NOT NULL,
	[VersionCode] nvarchar(255) NOT NULL
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
----------




CREATE TABLE CMS_GOVERNMENT_ENTITY_AND_DEPARTMENTS_SYNC_LOG
(
Id INT PRIMARY KEY IDENTITY(1,1),
Message NVARCHAR(1000),
CreatedBy NVARCHAR(256) NOT NULL,
CreatedDate DATETIME NOT NULL,
ModifiedBy NVARCHAR(256),
ModifiedDate DATETIME,
DeletedBy NVARCHAR(256),
DeletedDate DATETIME,
IsDeleted BIT NOT NULL
)

/****** Object:  Table [dbo].[TARASOL_GE_FATWA_DEPARTMENTS]    Script Date: 01/07/2022 11:31:56 am ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TARASOL_GE_FATWA_DEPARTMENTS]') AND type in (N'U'))
DROP TABLE [dbo].[TARASOL_GE_FATWA_DEPARTMENTS]
GO

/****** Object:  Table [dbo].[USER_TASK_VIEW]    Script Date: 05/06/2024 12:40:48 pm ******/

CREATE TABLE [dbo].[USER_TASK_VIEW](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[LastViewTime] [datetime] NOT NULL,
	[ReferenceId] [uniqueidentifier] NULL,
 CONSTRAINT [PK__USER_TAS__3214EC07A5992D2E] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
 --------------------Muhammad Ali--------Court Address Nullable--------13 June 2024----
 ALTER TABLE CMS_COURT_G2G_LKP
ALTER COLUMN Address VARCHAR(500) NULL;
-----------------------------------------End--------------------------------------

----------------Muhammad Ali---------Number Pattern----------------13 June 2024--------
ALTER TABLE CMS_COMS_NUM_PATTERN
DROP COLUMN GovtEntityNumPatternId;

ALTER TABLE CMS_COMS_NUM_PATTERN
DROP COLUMN UserId;

ALTER TABLE CMS_COMS_NUM_PATTERN_HISTORY
DROP COLUMN GovtEntityNumPatternId;

ALTER TABLE CMS_COMS_NUM_PATTERN_HISTORY
DROP COLUMN UserId;
-------------------------------------End


/*<History Author='Ammaar Naveed' Date='23-06-2024'>Recreated designations lookup table to handle enum values seamlessly.</History>*/
ALTER TABLE EP_EMPLOYMENT_INFORMATION
DROP CONSTRAINT FK__EP_EMPLOY__Desig__32F66B4F

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EP_DESIGNATION]') AND type in (N'U'))
DROP TABLE [dbo].[EP_DESIGNATION]
GO
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EP_DESIGNATION](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name_En] [nvarchar](150) NULL,
	[Name_Ar] [nvarchar](150) NULL,
	[CreatedBy] [nvarchar](256) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](256) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](256) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
	[IsSystemGenerated] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
--Recrated foreign key contraint. Proceed to insert values first in EP_Designation(from tInsert file) then run this script.
ALTER TABLE EP_EMPLOYMENT_INFORMATION
ADD CONSTRAINT FK_Employee_DesignationId
FOREIGN KEY (DesignationId) REFERENCES EP_DESIGNATION(Id);

----------------Muhammad Ali---------26 June 2024-------------DeptProfession and ShiftId added along with the new table CMS_CHAMBER_SHIFT_G2G_LKP------------
/****** Object:  Table [dbo].[CMS_CHAMBER_SHIFT_G2G_LKP]    Script Date: 25/06/2024  ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].CMS_CHAMBER_SHIFT_G2G_LKP(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[NameEn] [nvarchar](100) NOT NULL,
	[NameAr] [nvarchar](100) NOT NULL,
	[IsActive] [bit] NULL,
	[CreatedBy] [nvarchar](256) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](256) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](256) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
--------------------------------
ALTER TABLE CMS_GOVERNMENT_ENTITY_DEPARTMENT_G2G_LKP
ADD DeptProfession nvarchar(256) DEFAULT NULL;

ALTER TABLE CMS_CHAMBER_NUMBER_G2G_LKP
ADD ShiftId INT DEFAULT 1 NULL;

UPDATE CMS_CHAMBER_NUMBER_G2G_LKP
SET ShiftId = 1
WHERE ShiftId IS NULL;
-----------------------------End----------------

/*<History Author='Ammaar Naveed' Date='04-07-2024'>Added manager Id column</History>*/
ALTER TABLE CMS_LAWYER_SUPERVISOR
ADD ManagerId NVARCHAR(450) NULL
-----------------------------End----------------
--------- 08 july 2024--
go

IF OBJECT_ID('pCmsComsGetGovernmentEntitiesPattern', 'P') IS NOT NULL
    DROP PROCEDURE pCmsComsGetGovernmentEntitiesPattern;

IF OBJECT_ID('vw_GetGovernmentEntitiesPattern', 'V') IS NOT NULL
    DROP VIEW vw_GetGovernmentEntitiesPattern;

	-- 09 july 
IF NOT EXISTS (
  SELECT 1
  FROM INFORMATION_SCHEMA.COLUMNS
  WHERE TABLE_NAME = 'CMS_COMS_NUM_PATTERN_HISTORY'
  AND COLUMN_NAME = 'UpdatedGovtEntities'
)
BEGIN
  ALTER TABLE CMS_COMS_NUM_PATTERN_HISTORY
  ADD UpdatedGovtEntities varchar(512);
END;

------18 july-2024
EXEC sp_rename 'WS_PUBLIC_HOLIDAYS.holidaydate',  'FromDate', 'COLUMN';
ALTER TABLE WS_PUBLIC_HOLIDAYS
ADD ToDate date ;

-----
--NOTE > Not for FATWA_DB_DEV && FT_FATWA
truncate table WS_PUBLIC_HOLIDAYS
---------
ALTER TABLE CMS_CASE_FILE_SECTOR_ASSIGNMENT
ADD IsReadOnly bit NOT NULL DEFAULT(0)
--------


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EP_WORKINGHOURS_LKP]') AND type in (N'U'))
DROP TABLE [dbo].[EP_WORKINGHOURS_LKP]
GO

/****** Object:  Table [dbo].[EP_WORKINGHOURS_LKP]    Script Date: 30/07/2024 3:47:42 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EP_WORKINGHOURS_LKP](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[NameEn] [varchar](255) NOT NULL,
	[NameAr] [nvarchar](255) NOT NULL,
	[StartTime] [time](7) NOT NULL,
	[EndTime] [time](7) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[EP_WORKINGHOURS_LKP] ON 
GO
INSERT [dbo].[EP_WORKINGHOURS_LKP] ([Id], [NameEn], [NameAr], [StartTime], [EndTime]) VALUES (1, N'Half Time', N'Half Time', CAST(N'07:30:00' AS Time), CAST(N'13:30:00' AS Time))
GO
INSERT [dbo].[EP_WORKINGHOURS_LKP] ([Id], [NameEn], [NameAr], [StartTime], [EndTime]) VALUES (2, N'Full Time', N'Full Time', CAST(N'07:30:00' AS Time), CAST(N'15:30:00' AS Time))
GO
INSERT [dbo].[EP_WORKINGHOURS_LKP] ([Id], [NameEn], [NameAr], [StartTime], [EndTime]) VALUES (3, N'Special Hour', N'Speical Hour', CAST(N'07:30:00' AS Time), CAST(N'13:30:00' AS Time))
GO
SET IDENTITY_INSERT [dbo].[EP_WORKINGHOURS_LKP] OFF
GO
--------
ALTER TABLE CMS_DRAFTED_TEMPLATE_VERSION_LOGS
ADD ReviewerUserId nvarchar(256)
-------
--------

/*<History Author='Ammaar Naveed' Date='30-07-2024'>Added default receiver column</History>*/
ALTER TABLE EP_EMPLOYMENT_INFORMATION
ADD IsDefaultCorrespondenceReceiver BIT NOT NULL DEFAULT 0

---------------------------------------4/8/24
CREATE TABLE [dbo].[EP_WEEKDAYS_SETTINGS_LKP](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[NameEn] [varchar](50) NOT NULL,
	[NameAr] [nvarchar](50) NOT NULL,
	[IsWeekend] [bit] NOT NULL,
	[IsRestDay] [bit] NOT NULL,
 CONSTRAINT [PK_EP_WEEKDAYS_SETTINGS] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
-------
ALTER TABLE CMS_CASE_FILE_STATUS_HISTORY
ADD SectorTo int NOT NULL DEFAULT(0)
-------

-----------  8-8-2024 ---------------

create table RMQ_UNPUBLISH_MESSAGES (
Id uniqueidentifier ,
RoutingKey nvarchar(500),
RQMessages nvarchar(max),
Re_AttemptCount int,
IsPublished bit null default 0,
CreatedBy nvarchar(100) null,
CreatedDate datetime null,
ModifiedBy nvarchar(100) null,
ModifiedDate datetime null,
DeletedBy nvarchar(100) null,
DeletedDate datetime null,
IsDeleted bit null default 0
)

-----------  8-8-2024 ---------------
truncate table WS_PUBLIC_HOLIDAYS


--------------------------------------- FK Reference Changed FROM TSK_TASK_RESPONSE_STATUS to TSK_TASK_STATUS
ALTER TABLE [dbo].[TSK_TASK_RESPONSE] DROP CONSTRAINT [FK_TSK_TASK_RESPONSE_STATUS]
-----------  8-8-2024 ---------------
-------

/*<History Author='Ammaar Naveed' Date='11-08-2024'>Added transactional columns for UMS_USER_CLAIMS</History>*/
ALTER TABLE UMS_USER_CLAIMS
ADD CreatedBy NVARCHAR(150) NOT NULL DEFAULT 'fatwaadmin@gmail.com'

ALTER TABLE UMS_USER_CLAIMS
ADD CreatedDate DATETIME NOT NULL DEFAULT GETDATE()

ALTER TABLE UMS_USER_CLAIMS
ADD ModifiedBy NVARCHAR(150) NULL

ALTER TABLE UMS_USER_CLAIMS
ADD ModifiedDate DATETIME NULL

ALTER TABLE UMS_USER_CLAIMS
ADD DeletedBy NVARCHAR(150) NULL

ALTER TABLE UMS_USER_CLAIMS
ADD DeletedDate DATETIME NULL

ALTER TABLE UMS_USER_CLAIMS
ADD IsDeleted BIT NOT NULL

/*<History Author='Ammaar Naveed' Date='13-08-2024'>Added ModuleId column for data filtration</History>*/
ALTER TABLE UMS_USER_CLAIMS
ADD ModuleId BIGINT NULL
----Case File Transfer Request------
GO

/****** Object:  Table [dbo].[CMS_CASE_FILE_TRANSFER_REQUEST]    Script Date: 22/08/2024 6:24:40 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CMS_CASE_FILE_TRANSFER_REQUEST](
	[Id] [uniqueidentifier] NOT NULL,
	[SectorFrom] [int] NOT NULL,
	[SectorTo] [int] NOT NULL,
	[Description] [nvarchar](500) NULL,
	[StatusId] [int] NOT NULL,
	[CreatedBy] [nvarchar](256) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](256) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](256) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL
) ON [PRIMARY]
GO
----------
GO

/****** Object:  Table [dbo].[CMS_CASE_FILE_TRANSFER_REQUEST_ENUM]    Script Date: 22/08/2024 6:25:28 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CMS_CASE_FILE_TRANSFER_REQUEST_ENUM](
	[ActionId] [int] NOT NULL,
	[NameEn] [nvarchar](256) NOT NULL,
	[NameAr] [nvarchar](256) NOT NULL
) ON [PRIMARY]
GO
-------
GO

/****** Object:  Table [dbo].[CMS_CASE_FILE_TRANSFER_REQUEST_STATUS]    Script Date: 25/08/2024 6:08:14 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CMS_CASE_FILE_TRANSFER_REQUEST_STATUS](
	[Id] [int] NOT NULL,
	[NameEn] [nvarchar](256) NOT NULL,
	[NameAr] [nvarchar](256) NOT NULL
) ON [PRIMARY]
GO
---------

---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------- LMS STOCKTAKING START -------------------------------------------------------------
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LMS_LITERATURE_STOCKTAKING]') AND type in (N'U'))
DROP TABLE [dbo].[LMS_LITERATURE_STOCKTAKING]
GO
/****** Object:  Table [dbo].[LMS_LITERATURE_STOCKTAKING]    Script Date: 29/008/2024 04:07:56 am ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [DBO].[LMS_LITERATURE_STOCKTAKING]
(
Id UNIQUEIDENTIFIER PRIMARY KEY NOT NULL,
StockTakingDate DATETIME NOT NULL,
StatusId INT NOT NULL,
TotalBooks INT NOT NULL,
StockTakingPerformerId NVARCHAR(450) NOT NULL,
Note NVARCHAR(500) NULL,
CreatedBy NVARCHAR(256) NOT NULL,
CreatedDate DATETIME NOT NULL,
ModifiedBy NVARCHAR(256) NULL,
ModifiedDate DATETIME NULL,
DeletedBy NVARCHAR(256) NULL,
DeletedDate DATETIME NULL,
IsDeleted BIT NULL,
)
-----------------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LMS_LITERATURE_STOCKTAKING_REPORT]') AND type in (N'U'))
DROP TABLE [dbo].[LMS_LITERATURE_STOCKTAKING_REPORT]
GO

/****** Object:  Table [dbo].[LMS_LITERATURE_STOCKTAKING_REPORT]    Script Date: 29/008/2024 04:07:56 am ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [DBO].[LMS_LITERATURE_STOCKTAKING_REPORT]
(
Id UNIQUEIDENTIFIER PRIMARY KEY NOT NULL,
BarcodeId INT NOT NULL,
StockTakingId UNIQUEIDENTIFIER NOT NULL,
IsBorrowed BIT NULL,
Excess INT NULL,
Shortage INT NULL,
)
----------------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LMS_BARCODE_STOCKTAKING_REMARKS]') AND type in (N'U'))
DROP TABLE [dbo].[LMS_BARCODE_STOCKTAKING_REMARKS]
GO
/****** Object:  Table [dbo].[LMS_BARCODE_STOCKTAKING_REMARKS]    Script Date: 29/008/2024 04:07:56 am ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [DBO].[LMS_BARCODE_STOCKTAKING_REMARKS]
(
Id UNIQUEIDENTIFIER PRIMARY KEY NOT NULL,
BarcodeId INT NOT NULL,
StockTakingId UNIQUEIDENTIFIER NOT NULL,
Remarks NVARCHAR(500) NULL
)
----------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LMS_STOCKTAKING_STATUS_LKP]') AND type in (N'U'))
DROP TABLE [dbo].[LMS_STOCKTAKING_STATUS_LKP]
GO
/****** Object:  Table [dbo].[LMS_STOCKTAKING_STATUS_LKP]   Script Date: 29/008/2024 04:07:56 am ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [DBO].[LMS_STOCKTAKING_STATUS_LKP]
(
Id INT PRIMARY KEY NOT NULL,
NameEn NVARCHAR(100) NOT NULL,
NameAr NVARCHAR(100) NOT NULL,
CreatedBy NVARCHAR(256) NOT NULL,
CreatedDate DATETIME NOT NULL,
ModifiedBy NVARCHAR(256) NULL,
ModifiedDate DATETIME NULL,
DeletedBy NVARCHAR(256) NULL,
DeletedDate DATETIME NULL,
IsDeleted BIT NULL,
)
----------------------------------
ALTER TABLE LMS_LITERATURE_BARCODE 
ALTER COLUMN RFIDValue NVARCHAR(200) NULL

---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------- LMS STOCKTAKING END ---------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-------Registered Case Transfer Request------
GO

/****** Object:  Table [dbo].[CMS_RESGISTERED_CASE_TRANSFER_REQUEST]    Script Date: 03/09/2024 2:26:53 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CMS_RESGISTERED_CASE_TRANSFER_REQUEST](
	[Id] [uniqueidentifier] NOT NULL,
	[OutcomeId] [uniqueidentifier] NOT NULL,
	[ChamberFromId] [int] NOT NULL,
	[ChamberToId] [int] NOT NULL,
	[ChamberNumberFromId] [int] NOT NULL,
	[ChamberNumberToId] [int] NOT NULL,
	[Remarks] [nvarchar](500) NULL,
	[StatusId] [int] NOT NULL,
	[CreatedBy] [nvarchar](256) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](256) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](256) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL
) ON [PRIMARY]
GO
----------
GO

/****** Object:  Table [dbo].[CMS_RESGISTERED_CASE_TRANSFER_REQUEST_STATUS]    Script Date: 03/09/2024 2:27:56 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CMS_RESGISTERED_CASE_TRANSFER_REQUEST_STATUS](
	[Id] [int] NOT NULL,
	[NameEn] [nvarchar](256) NOT NULL,
	[NameAr] [nvarchar](256) NOT NULL
) ON [PRIMARY]
GO
------------


------09-09-2024--

IF NOT EXISTS (
    SELECT 1
    FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'UMS_USER'
    AND COLUMN_NAME = 'AllowDigitalSign'
)
BEGIN
    ALTER TABLE UMS_USER
    ADD AllowDigitalSign BIT NOT NULL DEFAULT 1;
END



---------10-09-2024


IF NOT EXISTS (
    SELECT 1
    FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'UMS_USER'
    AND COLUMN_NAME = 'HasSignatureImage'
)
BEGIN
    ALTER TABLE ums_user
    ADD HasSignatureImage BIT not NULL DEFAULT 0;
END

IF  EXISTS (
    SELECT 1
    FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'UMS_USER'
    AND COLUMN_NAME = 'SignatureImage'
)
BEGIN
    ALTER TABLE ums_user
    drop column SignatureImage ;
END
------------
----------------------------- 05- sep-024

CREATE TABLE [dbo].[CMS_OPERATING_SECTOR_TYPES_G2G_LKP_UMS_ROLE](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [nvarchar](150) NOT NULL,
	[SectorId] [int] NOT NULL,
 CONSTRAINT [PK_CMS_OPERATING_SECTOR_TYPES_G2G_LKP_UMS_ROLE] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[CMS_OPERATING_SECTOR_TYPES_G2G_LKP_UMS_ROLE]  WITH CHECK ADD  CONSTRAINT [FK_CMS_OPERATING_SECTOR_TYPES_G2G_LKP_UMS_ROLE_CMS_OPERATING_SECTOR_TYPES_G2G_LKP_UMS_ROLE] FOREIGN KEY([Id])
REFERENCES [dbo].[CMS_OPERATING_SECTOR_TYPES_G2G_LKP_UMS_ROLE] ([Id])
GO

ALTER TABLE [dbo].[CMS_OPERATING_SECTOR_TYPES_G2G_LKP_UMS_ROLE] CHECK CONSTRAINT [FK_CMS_OPERATING_SECTOR_TYPES_G2G_LKP_UMS_ROLE_CMS_OPERATING_SECTOR_TYPES_G2G_LKP_UMS_ROLE]
GO


--------17 sep 2024

/****** Object:  Table [dbo].[LMS_BORROW_LITERATURE_HISTORY]    Script Date: 17/09/2024 11:53:57 am ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LMS_BORROW_LITERATURE_HISTORY](
	[Id] [uniqueidentifier] NOT NULL,
	[UserId] [nvarchar](max) NOT NULL,
	[EventId] [nvarchar](max) NOT NULL,
	[LiteratureId] [int] NOT NULL,
	[BorrowId] [int] NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](100) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](100) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO



/****** Object:  Table [dbo].[LMS_BORROW_LITERATURE_Event_LKP]    Script Date: 17/09/2024 11:55:14 am ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LMS_BORROW_LITERATURE_Event_LKP](
	[Id] [int] NOT NULL,
	[NameEn] [nvarchar](max) NULL,
	[NameAr] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

---------19/09/2024
ALTER TABLE LMS_BORROW_LITERATURE_HISTORY

ALTER COLUMN CreatedBy NVARCHAR(256) NOT NULL;


ALTER TABLE LMS_BORROW_LITERATURE_HISTORY

ALTER COLUMN DeletedBy NVARCHAR(256) NULL;


ALTER TABLE LMS_BORROW_LITERATURE_HISTORY

ALTER COLUMN ModifiedBy NVARCHAR(256) NULL;

EXEC sp_rename 'LMS_BORROW_LITERATURE_Event_LKP', 'LMS_BORROW_LITERATURE_EVENT_LKP';




--------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LMS_STOCKTAKING_PERFORMER]') AND type in (N'U'))
DROP TABLE [dbo].[LMS_STOCKTAKING_PERFORMER]
GO
CREATE TABLE [DBO].[LMS_STOCKTAKING_PERFORMER]
(
Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
StockTakingId UNIQUEIDENTIFIER NOT NULL,
UserId NVARCHAR(450) NOT NULL,
CreatedBy NVARCHAR(256) NOT NULL,
CreatedDate DATETIME NOT NULL,
ModifiedBy NVARCHAR(256)  NULL,
ModifiedDate DATETIME  NULL,
DeletedBy NVARCHAR(256)  NULL,
DeletedDate DATETIME  NULL,
IsDeleted BIT NOT NULL,
)
--------
update NOTIF_NOTIFICATION_TEMPLATE set NameEn =N'Reject To Accept Assign File', NameAr=N'Reject To Accept Assign File',
SubjectEn=N'Reject To Accept Assign File',SubjectAr=N'Reject To Accept Assign File',
BodyEn=N'Assign File For Transfer to Sector has been Rejected send by #Sender Name#  on #Created Date#',
BodyAr=N'Assign File For Transfer to Sector has been Rejected send by #Sender Name#  on #Created Date#'
where TemplateId='AAA78021-5AC3-472E-B4F3-A61879311F58' and EventId=68
------------

/*<History Author='Ammaar Naveed' Date='01-10-2024'>Added DelegatedUserId column in EP</History>*/
ALTER TABLE EP_EMPLOYMENT_INFORMATION
ADD DelegatedUserId NVARCHAR(256) NULL

/*<History Author='Ammaar Naveed' Date='02-10-2024'>Added DelegatedBy column in EP for delegation history</History>*/
ALTER TABLE EP_EMPLOYMENT_INFORMATION
ADD DelegatedBy NVARCHAR(256) NULL

-----------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LMS_STOCKTAKING_HISTORY]') AND type in (N'U'))
DROP TABLE [dbo].[LMS_STOCKTAKING_HISTORY]
GO
CREATE TABLE [DBO].[LMS_STOCKTAKING_HISTORY]
(
Id UNIQUEIDENTIFIER PRIMARY KEY NOT NULL,
StockTakingId UNIQUEIDENTIFIER NOT NULL,
EventId INT NOT NULL,
CreatedBy NVARCHAR(256) NOT NULL,
CreatedDate DATETIME NOT NULL,
ModifiedBy NVARCHAR(256)  NULL,
ModifiedDate DATETIME  NULL,
DeletedBy NVARCHAR(256)  NULL,
DeletedDate DATETIME  NULL,
IsDeleted BIT NOT NULL,

)
-----------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LMS_STOCKTAKING_EVENT_LKP]') AND type in (N'U'))
DROP TABLE [dbo].[LMS_STOCKTAKING_EVENT_LKP]
GO
CREATE TABLE [DBO].[LMS_STOCKTAKING_EVENT_LKP]
(
Id INT PRIMARY KEY NOT NULL,
NameEn NVARCHAR(100) NOT NULL,
NameAr NVARCHAR(100) NOT NULL,
CreatedBy NVARCHAR(256) NOT NULL,
CreatedDate DATETIME NOT NULL,
ModifiedBy NVARCHAR(256)  NULL,
ModifiedDate DATETIME  NULL,
DeletedBy NVARCHAR(256)  NULL,
DeletedDate DATETIME  NULL,
IsDeleted BIT NOT NULL,

)

------
ALTER TABLE CMS_RESGISTERED_CASE_TRANSFER_REQUEST
ADD PRIMARY KEY (Id);
---------
---------------- 06/10/2024------

ALTER TABLE NOTIF_NOTIFICATION_EVENT
ALTER COLUMN NameEn nvarchar(255);
ALTER TABLE NOTIF_NOTIFICATION_EVENT
ALTER COLUMN NameAr nvarchar(255);

----------------------------------------
GO

/****** Object:  Table [dbo].[SR_FINAL_APPROVAL]    Script Date: 08/10/2024 12:55:32 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SR_FINAL_APPROVAL](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ServiceRequestTypeId] [int] NOT NULL,
	[NoOfApprovals] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](512) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](512) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [nchar](10) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_SR_FINAL_APPROVAL] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
---------------------------------------------
GO

/****** Object:  Table [dbo].[SR_FINAL_APPROVAL_ACTIVITIES]    Script Date: 08/10/2024 12:56:25 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SR_FINAL_APPROVAL_ACTIVITIES](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FinalApprovalId] [int] NOT NULL,
	[ApprovalSequenceNo] [int] NULL,
	[RoleId] [nvarchar](250) NULL,
	[SectorTypeId] [int] NOT NULL,
	[DepartmentId] [int] NOT NULL,
	[VersionId] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](512) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](512) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [nchar](10) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_SR_FINAL_APPROVAL_ACTIVITIES] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[SR_FINAL_APPROVAL_ACTIVITIES] ADD  DEFAULT ((1)) FOR [DepartmentId]
GO

ALTER TABLE [dbo].[SR_FINAL_APPROVAL_ACTIVITIES] ADD  DEFAULT ((1)) FOR [VersionId]
GO

ALTER TABLE [dbo].[SR_FINAL_APPROVAL_ACTIVITIES] ADD  DEFAULT ((1)) FOR [IsActive]
GO

ALTER TABLE [dbo].[SR_FINAL_APPROVAL_ACTIVITIES] ADD  DEFAULT ('') FOR [CreatedBy]
GO

ALTER TABLE [dbo].[SR_FINAL_APPROVAL_ACTIVITIES] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO

ALTER TABLE [dbo].[SR_FINAL_APPROVAL_ACTIVITIES] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO




-------------------13 / 10/ 2024 --------------

UPDATE CMS_CASE_REQUEST SET PatternSequenceResult = sub.SequanceResult, RequestNumberFormat = sub.SequanceFormatResult
FROM (SELECT TOP 1 SequanceResult, SequanceFormatResult
      FROM CMS_COMS_NUM_PATTERN
      WHERE PatternTypId = 1 AND IsDefault = 1) AS sub;


 UPDATE COMS_CONSULTATION_REQUEST   SET PatternSequenceResult = sub.SequanceResult, RequestNumberFormat = sub.SequanceFormatResult
FROM (SELECT TOP 1 SequanceResult, SequanceFormatResult
      FROM CMS_COMS_NUM_PATTERN
      WHERE PatternTypId = 2 AND IsDefault = 1) AS sub;


UPDATE CMS_CASE_FILE SET PatternSequenceResult = sub.SequanceResult, CaseFileNumberFormat = sub.SequanceFormatResult
FROM (SELECT TOP 1 SequanceResult, SequanceFormatResult
      FROM CMS_COMS_NUM_PATTERN
      WHERE PatternTypId = 3 AND IsDefault = 1) AS sub;


UPDATE COMS_CONSULTATION_FILE SET PatternSequenceResult = sub.SequanceResult, ComsFileNumberFormat = sub.SequanceFormatResult
FROM (SELECT TOP 1 SequanceResult, SequanceFormatResult
      FROM CMS_COMS_NUM_PATTERN
      WHERE PatternTypId = 4 AND IsDefault = 1) AS sub;

UPDATE COMM_COMMUNICATION SET PatternSequenceResult = sub.SequanceResult, InboxNumberFormat = sub.SequanceFormatResult
FROM (SELECT TOP 1 SequanceResult, SequanceFormatResult
      FROM CMS_COMS_NUM_PATTERN
      WHERE PatternTypId = 5 AND IsDefault = 1) AS sub where InboxNumber is not null

UPDATE COMM_COMMUNICATION SET PatternSequenceResult = sub.SequanceResult, OutBoxNumberFormat = sub.SequanceFormatResult
FROM (SELECT TOP 1 SequanceResult, SequanceFormatResult
      FROM CMS_COMS_NUM_PATTERN
      WHERE PatternTypId = 6 AND IsDefault = 1) AS sub where OutboxNumber is not null


IF NOT EXISTS(SELECT * FROM sys.columns 
              WHERE Name = N'PatternSequenceResult' 
              AND Object_ID = Object_ID(N'CMS_CASE_REQUEST'))
BEGIN
    ALTER TABLE CMS_CASE_REQUEST 
    ADD PatternSequenceResult nvarchar(256) NOT NULL DEFAULT 'Default';
END

IF NOT EXISTS(SELECT * FROM sys.columns 
              WHERE Name = N'RequestNumberFormat' 
              AND Object_ID = Object_ID(N'COMS_CONSULTATION_REQUEST'))
BEGIN
    ALTER TABLE COMS_CONSULTATION_REQUEST 
    ADD RequestNumberFormat nvarchar(256) NOT NULL DEFAULT 'Default';
END

IF NOT EXISTS(SELECT * FROM sys.columns 
              WHERE Name = N'PatternSequenceResult' 
              AND Object_ID = Object_ID(N'COMS_CONSULTATION_REQUEST'))
BEGIN
    ALTER TABLE COMS_CONSULTATION_REQUEST 
    ADD PatternSequenceResult nvarchar(256) NOT NULL DEFAULT 'Default';
END


If NOT EXISTS(SELECT * FROM sys.columns 
              WHERE Name = N'PatternSequenceResult' 
              AND Object_ID = Object_ID(N'CMS_CASE_FILE'))
BEGIN
    ALTER TABLE CMS_CASE_FILE 
    ADD PatternSequenceResult nvarchar(256) NOT NULL DEFAULT 'Default';
END

IF NOT EXISTS(SELECT * FROM sys.columns 
              WHERE Name = N'PatternSequenceResult' 
              AND Object_ID = Object_ID(N'COMS_CONSULTATION_FILE'))
BEGIN
    ALTER TABLE COMS_CONSULTATION_FILE 
    ADD PatternSequenceResult nvarchar(256) NOT NULL DEFAULT 'Default';
END

IF NOT EXISTS(SELECT * FROM sys.columns 
              WHERE Name = N'PatternSequenceResult' 
              AND Object_ID = Object_ID(N'COMM_COMMUNICATION'))
BEGIN
    ALTER TABLE COMM_COMMUNICATION 
    ADD PatternSequenceResult nvarchar(256) NOT NULL DEFAULT 'Default';
END





-------------------
--- below queries Already Run on FT_Fatwa 

ALTER TABLE CMS_GOVERNMENT_ENTITY_NUM_PATTERN 
drop COLUMN CreatedBy 
ALTER TABLE CMS_GOVERNMENT_ENTITY_NUM_PATTERN 
drop COLUMN CreatedDate 
ALTER TABLE CMS_GOVERNMENT_ENTITY_NUM_PATTERN 
drop COLUMN ModifiedBy 
ALTER TABLE CMS_GOVERNMENT_ENTITY_NUM_PATTERN 
drop COLUMN ModifiedDate 
ALTER TABLE CMS_GOVERNMENT_ENTITY_NUM_PATTERN 
drop COLUMN DeletedBy 
ALTER TABLE CMS_GOVERNMENT_ENTITY_NUM_PATTERN 
drop COLUMN DeletedDate 
ALTER TABLE CMS_GOVERNMENT_ENTITY_NUM_PATTERN 
drop COLUMN IsDeleted 
ALTER TABLE CMS_GOVERNMENT_ENTITY_NUM_PATTERN 
drop COLUMN isactive 

ALTER TABLE CMS_GOVERNMENT_ENTITY_NUM_PATTERN 
ADD  CreatedBy nvarchar(256)  NOT NULL Default 'System';

ALTER TABLE CMS_GOVERNMENT_ENTITY_NUM_PATTERN 
ADD  CreatedDate datetime NOT NULL Default getdate()

ALTER TABLE CMS_GOVERNMENT_ENTITY_NUM_PATTERN 
ADD  ModifiedBy nvarchar(256)  NULL;

ALTER TABLE CMS_GOVERNMENT_ENTITY_NUM_PATTERN 
ADD  ModifiedDate datetime NULL;

ALTER TABLE CMS_GOVERNMENT_ENTITY_NUM_PATTERN 
ADD  DeletedBy nvarchar(256)  NULL;

ALTER TABLE CMS_GOVERNMENT_ENTITY_NUM_PATTERN 
ADD  DeletedDate datetime NULL;

ALTER TABLE CMS_GOVERNMENT_ENTITY_NUM_PATTERN 
ADD  IsDeleted bit not NULL Default 0;




------------16-10-2024

ALTER TABLE COMM_COMMUNICATION
ALTER COLUMN PatternSequenceResult nvarchar(256) NULL;
---------
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WF_WORKFLOW_TRIGGER_CONDITION_OPTIONS](
	[WorkflowOptionId] [int] IDENTITY(1,1) NOT NULL,
	[ModuleOptionId] [int] NULL,
	[TriggerConditionId] [int] NULL,
	[TrueCaseFlowControlId] [int] NULL,
	[TrueCaseActivityNo] [int] NULL,
	[CreatedBy] [nvarchar](256) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](256) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](256) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[WorkflowOptionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
------------
ALTER TABLE WF_WORKFLOW_TRIGGER_CONDITION 
ADD IsOption BIT NOT NULL DEFAULT(0)
----------
ALTER TABLE WF_OPTIONS_PR_LKP 
ADD IsTriggerSpecific BIT NOT NULL DEFAULT(0)
----------

-------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TASK_DECISION_REMINDER]') AND type in (N'U'))
DROP TABLE [dbo].[TASK_DECISION_REMINDER]
GO
CREATE TABLE TASK_DECISION_REMINDER
(
Id UNIQUEIDENTIFIER PRIMARY KEY NOT NULL,
TaskId UNIQUEIDENTIFIER NOT NULL,
IsReminderSent BIT NOT NULL,
CreatedDate DATETIME NOT NULL
)




/****** Object:  Table [dbo].[SR_FINAL_APPROVAL]    Script Date: 08/10/2024 12:55:32 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SR_FINAL_APPROVAL](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ServiceRequestTypeId] [int] NOT NULL,
	[NoOfApprovals] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](512) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](512) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [nchar](10) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_SR_FINAL_APPROVAL] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
---------------------------------------------
GO

/****** Object:  Table [dbo].[SR_FINAL_APPROVAL_ACTIVITIES]    Script Date: 08/10/2024 12:56:25 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SR_FINAL_APPROVAL_ACTIVITIES](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FinalApprovalId] [int] NOT NULL,
	[ApprovalSequenceNo] [int] NULL,
	[RoleId] [nvarchar](250) NULL,
	[SectorTypeId] [int] NOT NULL,
	[DepartmentId] [int] NOT NULL,
	[VersionId] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedBy] [nvarchar](512) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](512) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [nchar](10) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_SR_FINAL_APPROVAL_ACTIVITIES] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[SR_FINAL_APPROVAL_ACTIVITIES] ADD  DEFAULT ((1)) FOR [DepartmentId]
GO

ALTER TABLE [dbo].[SR_FINAL_APPROVAL_ACTIVITIES] ADD  DEFAULT ((1)) FOR [VersionId]
GO

ALTER TABLE [dbo].[SR_FINAL_APPROVAL_ACTIVITIES] ADD  DEFAULT ((1)) FOR [IsActive]
GO

ALTER TABLE [dbo].[SR_FINAL_APPROVAL_ACTIVITIES] ADD  DEFAULT ('') FOR [CreatedBy]
GO

ALTER TABLE [dbo].[SR_FINAL_APPROVAL_ACTIVITIES] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO

ALTER TABLE [dbo].[SR_FINAL_APPROVAL_ACTIVITIES] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO

-----
IF COLUMNPROPERTY(OBJECT_ID('dbo.EP_GRADE_TYPE'), 'DepartmentId', 'ColumnId') IS NULL
BEGIN
    ALTER TABLE EP_GRADE_TYPE  ADD DepartmentId  INT;
	ALTER TABLE EP_GRADE_TYPE  ADD CONSTRAINT FK_Department FOREIGN KEY (DepartmentId) REFERENCES EP_DEPARTMENT(Id);
END