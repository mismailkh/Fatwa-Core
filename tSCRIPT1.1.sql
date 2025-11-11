
/******  
<Object Scope='Public'> LEGAL_LEGISLATION 
<Copyright> Digital Processing System </Copyright>  
<Author Company='Business Analytics'> Umer Zaman </Author>  
<Created> 2022-10-05 </Created>  
<History Author='' Date=''>  </History>  
</Object>  
******/ 
-- [dbo].[LEGAL_LEGISLATION] 

GO

/****** Object:  Table [dbo].[LEGAL_LEGISLATION]    Script Date: 12/10/2022 3:46:20 pm ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_LEGISLATION]') AND type in (N'U'))
	DROP TABLE [dbo].[LEGAL_LEGISLATION]
GO

/****** Object:  Table [dbo].[LEGAL_LEGISLATION]    Script Date: 12/10/2022 3:46:20 pm ******/
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
	[IssueDate_Hijri] [datetime] NOT NULL,
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
) ON [PRIMARY]
GO

/******  
<Object Scope='Public'> LEGAL_LEGISLATION_LEGAL_TAG 
<Copyright> Digital Processing System </Copyright>  
<Author Company='Business Analytics'> Umer Zaman </Author>  
<Created> 2022-10-26 </Created>  
<History Author='' Date=''>  </History>  
</Object>  
******/ 
-- [dbo].[LEGAL_LEGISLATION_LEGAL_TAG] 

GO

/****** Object:  Table [dbo].[LEGAL_LEGISLATION_LEGAL_TAG]    Script Date: 12/10/2022 3:46:20 pm ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_LEGISLATION_LEGAL_TAG]') AND type in (N'U'))
DROP TABLE [dbo].[LEGAL_LEGISLATION_LEGAL_TAG]
GO

/****** Object:  Table [dbo].[LEGAL_LEGISLATION_LEGAL_TAG]    Script Date: 12/10/2022 3:46:20 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LEGAL_LEGISLATION_LEGAL_TAG](
	[Id] [int] IDENTITY (1,1) NOT NULL,
	[LegislationId] [uniqueidentifier] NOT NULL,
	[TagId] [int] NOT NULL,
 CONSTRAINT [PK_LEGAL_LEGISLATION_LEGAL_TAG] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/******  
<Object Scope='Public'> LEGAL_LEGISLATION_SIGNATURE 
<Copyright> Digital Processing System </Copyright>  
<Author Company='Business Analytics'> Umer Zaman </Author>  
<Created> 2022-10-05 </Created>  
<History Author='' Date=''>  </History>  
</Object>  
******/ 
-- [dbo].[LEGAL_LEGISLATION_SIGNATURE] 

GO

/****** Object:  Table [dbo].[LEGAL_LEGISLATION_SIGNATURE]    Script Date: 18/08/2022 2:24:57 pm ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_LEGISLATION_SIGNATURE]') AND type in (N'U'))
	DROP TABLE [dbo].[LEGAL_LEGISLATION_SIGNATURE]
GO

/****** Object:  Table [dbo].[LEGAL_LEGISLATION_SIGNATURE]    Script Date: 05/10/2022 2:24:57 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LEGAL_LEGISLATION_SIGNATURE](
	[SignatureId] [int] IDENTITY (1,1) NOT NULL,
	[LegislationId] [uniqueidentifier] NOT NULL,
	[Full_Name] [nvarchar](300) NOT NULL,
	[Job_Title] [nvarchar](300) NOT NULL,
	
 CONSTRAINT [PK_LEGAL_LEGISLATION_SIGNATURE] PRIMARY KEY CLUSTERED 
(
	[SignatureId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/******  
<Object Scope='Public'> LEGAL_PUBLICATION_SOURCE_NAME 
<Copyright> Digital Processing System </Copyright>  
<Author Company='Business Analytics'> Umer Zaman </Author>  
<Created> 2022-10-05 </Created>  
<History Author='' Date=''>  </History>  
</Object>  
******/ 
-- [dbo].[LEGAL_PUBLICATION_SOURCE_NAME] 

GO

/****** Object:  Table [dbo].LEGAL_PUBLICATION_SOURCE_NAME]    Script Date: 18/08/2022 2:24:57 pm ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_PUBLICATION_SOURCE_NAME]') AND type in (N'U'))
	DROP TABLE [dbo].[LEGAL_PUBLICATION_SOURCE_NAME]
GO

/****** Object:  Table [dbo].[LEGAL_PUBLICATION_SOURCE_NAME]    Script Date: 05/10/2022 2:24:57 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LEGAL_PUBLICATION_SOURCE_NAME](
	[PublicationNameId] [int] IDENTITY (1,1) NOT NULL,
	[Name_En] [nvarchar](500) NOT NULL,
	[Name_Ar] [nvarchar](500) NOT NULL,
	
 CONSTRAINT [PK_LEGAL_PUBLICATION_SOURCE_NAME] PRIMARY KEY CLUSTERED 
(
	[PublicationNameId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/******  
<Object Scope='Public'> LEGAL_PUBLICATION_SOURCE 
<Copyright> Digital Processing System </Copyright>  
<Author Company='Business Analytics'> Umer Zaman </Author>  
<Created> 2022-10-05 </Created>  
<History Author='' Date=''>  </History>  
</Object>  
******/ 
-- [dbo].[LEGAL_PUBLICATION_SOURCE] 

GO

/****** Object:  Table [dbo].LEGAL_PUBLICATION_SOURCE]    Script Date: 18/08/2022 2:24:57 pm ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_PUBLICATION_SOURCE]') AND type in (N'U'))
	DROP TABLE [dbo].[LEGAL_PUBLICATION_SOURCE]
GO

/****** Object:  Table [dbo].[LEGAL_PUBLICATION_SOURCE]    Script Date: 05/10/2022 2:24:57 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LEGAL_PUBLICATION_SOURCE](
	[SourceId] [int] IDENTITY (1,1) NOT NULL,
	[PublicationNameId] [int] NOT NULL,
	[LegislationId] [uniqueidentifier] NOT NULL,
	[Issue_Number] [nvarchar](100) NOT NULL,
	[PublicationDate] [datetime] NOT NULL,
	[PublicationDate_Hijri] [datetime] NOT NULL,
	[Page_Start] [int] NOT NULL,
	[Page_End] [int] NOT NULL,
	
 CONSTRAINT [PK_LEGAL_PUBLICATION_SOURCE] PRIMARY KEY CLUSTERED 
(
	[SourceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/******  
<Object Scope='Public'> LEGAL_ARTICLE 
<Copyright> Digital Processing System </Copyright>  
<Author Company='Business Analytics'> Umer Zaman </Author>  
<Created> 2022-10-05 </Created>  
<History Author='' Date=''>  </History>  
</Object>  
******/ 
-- [dbo].[LEGAL_ARTICLE] 

GO

/****** Object:  Table [dbo].[LEGAL_ARTICLE]    Script Date: 18/08/2022 2:24:57 pm ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_ARTICLE]') AND type in (N'U'))
	DROP TABLE [dbo].[LEGAL_ARTICLE]
GO

/****** Object:  Table [dbo].[LEGAL_ARTICLE]    Script Date: 05/10/2022 2:24:57 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LEGAL_ARTICLE](
	[ArticleId] [uniqueidentifier] NOT NULL,
	[LegislationId] [uniqueidentifier] NOT NULL,
	[SectionId] [uniqueidentifier] NULL,
	[Article_Name] [nvarchar](1000) NOT NULL,
	[Article_Title] [nvarchar](1000) NOT NULL,
	[Start_Date] [datetime] NULL,
	[Article_Status] [int] NULL,
	[End_Date] [datetime] NULL,
	[Article_Text] [nvarchar](max) NOT NULL,
	[Article_Explanatory_Note] [nvarchar](max) NULL,
	[NextArticleId] [uniqueidentifier] NULL,
	[ArticleOrder] [datetime] NULL,
	
 CONSTRAINT [PK_LEGAL_ARTICLE] PRIMARY KEY CLUSTERED 
(
	[ArticleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/******  
<Object Scope='Public'> LEGAL_CLAUSE 
<Copyright> Digital Processing System </Copyright>  
<Author Company='Business Analytics'> Umer Zaman </Author>  
<Created> 2022-10-04 </Created>  
<History Author='' Date=''>  </History>  
</Object>  
******/ 
-- [dbo].[LEGAL_CLAUSE] 

GO

/****** Object:  Table [dbo].[LEGAL_CLAUSE]    Script Date: 18/08/2022 2:24:57 pm ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_CLAUSE]') AND type in (N'U'))
	DROP TABLE [dbo].[LEGAL_CLAUSE]
GO

/****** Object:  Table [dbo].[LEGAL_CLAUSE]    Script Date: 05/10/2022 2:24:57 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LEGAL_CLAUSE](
	[ClauseId] [uniqueidentifier] NOT NULL,
	[LegislationId] [uniqueidentifier] NOT NULL,
	[SectionId] [uniqueidentifier] NULL,
	[Clause_Name] [nvarchar](1000) NOT NULL,
	[Clause_Status] [int] NULL,
	[Start_Date] [datetime] NULL,
	[End_Date] [datetime] NULL,
	[Clause_Content] [nvarchar](max) NOT NULL,
	[ClauseOrder] [datetime] NULL,
	
 CONSTRAINT [PK_LEGAL_CLAUSE] PRIMARY KEY CLUSTERED 
(
	[ClauseId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/******  
<Object Scope='Public'> LEGAL_ARTICLE_CHILD 
<Copyright> Digital Processing System </Copyright>  
<Author Company='Business Analytics'> Umer Zaman </Author>  
<Created> 2022-10-05 </Created>  
<History Author='' Date=''>  </History>  
</Object>  
******/ 
-- [dbo].[LEGAL_ARTICLE_CHILD] 

GO

/****** Object:  Table [dbo].[LEGAL_ARTICLE_CHILD]    Script Date: 18/08/2022 2:24:57 pm ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_ARTICLE_CHILD]') AND type in (N'U'))
	DROP TABLE [dbo].[LEGAL_ARTICLE_CHILD]
GO

/****** Object:  Table [dbo].[LEGAL_ARTICLE_CHILD]    Script Date: 05/10/2022 2:24:57 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LEGAL_ARTICLE_CHILD](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ParentId] [uniqueidentifier] NOT NULL,
	[ArticleChildId] [uniqueidentifier] NOT NULL,
	
 CONSTRAINT [PK_LEGAL_ARTICLE_CHILD] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/******  
<Object Scope='Public'> LEGAL_SECTION 
<Copyright> Digital Processing System </Copyright>  
<Author Company='Business Analytics'> Umer Zaman </Author>  
<Created> 2022-10-05 </Created>  
<History Author='' Date=''>  </History>  
</Object>  
******/ 
-- [dbo].[LEGAL_SECTION] 

GO

/****** Object:  Table [dbo].[LEGAL_SECTION]    Script Date: 18/08/2022 2:24:57 pm ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_SECTION]') AND type in (N'U'))
	DROP TABLE [dbo].[LEGAL_SECTION]
GO

/****** Object:  Table [dbo].[LEGAL_SECTION]    Script Date: 05/10/2022 2:24:57 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LEGAL_SECTION](
	[SectionId] [uniqueidentifier] NOT NULL,
	[LegislationId] [uniqueidentifier] NOT NULL,
	[Section_Parent_Id] [uniqueidentifier] NULL,
	[Section_Number] [int] NOT NULL,
	[ParentId] [int] NULL,
	[SectionTitle] [nvarchar](500) NOT NULL,
	[SectionParentTitle] [nvarchar](500) NULL,
	[HasChildren] [bit] NOT NULL,
	
	
 CONSTRAINT [PK_LEGAL_SECTION] PRIMARY KEY CLUSTERED 
(
	[SectionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/******  
<Object Scope='Public'> LEGAL_EXPLANATORY_NOTE 
<Copyright> Digital Processing System </Copyright>  
<Author Company='Business Analytics'> Umer Zaman </Author>  
<Created> 2022-10-05 </Created>  
<History Author='' Date=''>  </History>  
</Object>  
******/ 
-- [dbo].[LEGAL_EXPLANATORY_NOTE] 

GO

/****** Object:  Table [dbo].[LEGAL_EXPLANATORY_NOTE]    Script Date: 18/08/2022 2:24:57 pm ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_EXPLANATORY_NOTE]') AND type in (N'U'))
	DROP TABLE [dbo].[LEGAL_EXPLANATORY_NOTE]
GO

/****** Object:  Table [dbo].[LEGAL_EXPLANATORY_NOTE]    Script Date: 05/10/2022 2:24:57 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LEGAL_EXPLANATORY_NOTE](
	[ExplanatoryNoteId] [uniqueidentifier] NOT NULL,
	[LegislationId] [uniqueidentifier] NOT NULL,
	[ExplanatoryNote_Body] [nvarchar](max) NULL,
	
	
 CONSTRAINT [PK_LEGAL_EXPLANATORY_NOTE] PRIMARY KEY CLUSTERED 
(
	[ExplanatoryNoteId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/******  
<Object Scope='Public'> LEGAL_NOTE 
<Copyright> Digital Processing System </Copyright>  
<Author Company='Business Analytics'> Umer Zaman </Author>  
<Created> 2022-10-05 </Created>  
<History Author='' Date=''>  </History>  
</Object>  
******/ 
-- [dbo].[LEGAL_NOTE] 

GO

/****** Object:  Table [dbo].[LEGAL_NOTE]    Script Date: 18/08/2022 2:24:57 pm ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_NOTE]') AND type in (N'U'))
	DROP TABLE [dbo].[LEGAL_NOTE]
GO

/****** Object:  Table [dbo].[LEGAL_EXPLANATORY_NOTE]    Script Date: 05/10/2022 2:24:57 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LEGAL_NOTE](
	[NoteId] [int] IDENTITY (1,1) NOT NULL,
	[ParentId] [uniqueidentifier] NOT NULL,
	[Note_Text] [nvarchar](max) NULL,
	[Note_Location] [nvarchar](max) NOT NULL,
	[Note_Date] [datetime] NOT NULL,
	
	
 CONSTRAINT [PK_LEGAL_NOTE] PRIMARY KEY CLUSTERED 
(
	[NoteId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[LEGAL_LEGISLATION_FLOW_STATUS]    Script Date: 05/10/2022 4:59:38 pm ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_LEGISLATION_FLOW_STATUS]') AND type in (N'U'))
	DROP TABLE [dbo].[LEGAL_LEGISLATION_FLOW_STATUS]
GO

/****** Object:  Table [dbo].[LEGAL_LEGISLATION_FLOW_STATUS]    Script Date: 05/10/2022 4:59:38 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LEGAL_LEGISLATION_FLOW_STATUS](
	[Id] [int] NOT NULL,
	[Name_En] [nvarchar](250) NOT NULL,
	[Name_Ar] [nvarchar](250) NOT NULL
) ON [PRIMARY]
GO
GO

/****** Object:  Table [dbo].[LEGAL_LEGISLATION_STATUS]    Script Date: 05/10/2022 4:59:38 pm ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_LEGISLATION_STATUS]') AND type in (N'U'))
	DROP TABLE [dbo].[LEGAL_LEGISLATION_STATUS]
GO

/****** Object:  Table [dbo].[LEGAL_LEGISLATION_STATUS]    Script Date: 05/10/2022 4:59:38 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LEGAL_LEGISLATION_STATUS](
	[Id] [int] NOT NULL,
	[Name_En] [nvarchar](250) NOT NULL,
	[Name_Ar] [nvarchar](250) NOT NULL
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[LEGAL_ARTICLE_STATUS]    Script Date: 05/10/2022 5:00:04 pm ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_ARTICLE_STATUS]') AND type in (N'U'))
	DROP TABLE [dbo].[LEGAL_ARTICLE_STATUS]
GO

/****** Object:  Table [dbo].[LEGAL_ARTICLE_STATUS]    Script Date: 05/10/2022 5:00:04 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LEGAL_ARTICLE_STATUS](
	[Id] [int] NOT NULL,
	[Name_En] [nvarchar](50) NOT NULL,
	[Name_Ar] [nvarchar](50) NOT NULL
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[LEGAL_ARTICLE_SOURCE]    Script Date: 05/10/2022 5:00:39 pm ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_ARTICLE_SOURCE]') AND type in (N'U'))
DROP TABLE [dbo].[LEGAL_ARTICLE_SOURCE]
GO

/****** Object:  Table [dbo].[LEGAL_ARTICLE_SOURCE]    Script Date: 05/10/2022 5:00:39 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LEGAL_ARTICLE_SOURCE](
	[Id] [int] NOT NULL,
	[Name_En] [nvarchar](50) NOT NULL,
	[Name_Ar] [nvarchar](50) NOT NULL
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[LEGAL_LEGISLATION_TYPE]    Script Date: 05/10/2022 5:01:29 pm ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_LEGISLATION_TYPE]') AND type in (N'U'))
	DROP TABLE [dbo].[LEGAL_LEGISLATION_TYPE]
GO

/****** Object:  Table [dbo].[LEGAL_LEGISLATION_TYPE]    Script Date: 05/10/2022 5:01:29 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LEGAL_LEGISLATION_TYPE](
	[Id] [int] NOT NULL,
	[Name_En] [nvarchar](50) NOT NULL,
	[Name_Ar] [nvarchar](50) NOT NULL
) ON [PRIMARY]
GO
 
GO

ALTER TABLE [dbo].[NOTIF_NOTIFICATION] DROP CONSTRAINT [FK_NOTIF_NOTIFICATION_NOTIF_NOTIFICATION_TYPE]
GO

ALTER TABLE [dbo].[NOTIF_NOTIFICATION] DROP CONSTRAINT [FK_NOTIF_NOTIFICATION_NOTIF_NOTIFICATION_TEMPLATE]
GO

ALTER TABLE [dbo].[NOTIF_NOTIFICATION] DROP CONSTRAINT [FK_NOTIF_NOTIFICATION_NOTIF_NOTIFICATION_RECEIVER_TYPE]
GO

ALTER TABLE [dbo].[NOTIF_NOTIFICATION] DROP CONSTRAINT [FK_NOTIF_NOTIFICATION_NOTIF_NOTIFICATION_LINK]
GO

ALTER TABLE [dbo].[NOTIF_NOTIFICATION] DROP CONSTRAINT [FK_NOTIF_NOTIFICATION_NOTIF_NOTIFICATION_EVENT]
GO

ALTER TABLE [dbo].[NOTIF_NOTIFICATION] DROP CONSTRAINT [FK_NOTIF_NOTIFICATION_NOTIF_NOTIFICATION_COMMUNICATION_METHOD]
GO

ALTER TABLE [dbo].[NOTIF_NOTIFICATION] DROP CONSTRAINT [FK_NOTIF_NOTIFICATION_NOTIF_NOTIFICATION_CATEGORY]
GO

ALTER TABLE [dbo].[NOTIF_NOTIFICATION] DROP CONSTRAINT [FK_NOTIF_NOTIFICATION_MODULE]
GO

/****** Object:  Table [dbo].[NOTIF_NOTIFICATION]    Script Date: 10/10/2022 4:02:42 pm ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[NOTIF_NOTIFICATION]') AND type in (N'U'))
	DROP TABLE [dbo].[NOTIF_NOTIFICATION]
GO

/****** Object:  Table [dbo].[NOTIF_NOTIFICATION]    Script Date: 10/10/2022 4:02:42 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[NOTIF_NOTIFICATION](
	[NotificationId] [uniqueidentifier] NOT NULL,
	[DueDate] [datetime] NOT NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](100) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](100) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted] [bit] NULL,
	[SenderId] [nvarchar](450) NULL,
	[ReceiverId] [nvarchar](450) NULL,
	[ReceiverTypeId] [int] NULL,
	[ModuleId] [int] NOT NULL,
	[NotificationCommunicationMethodId] [int] NOT NULL,
	[NotificationTypeId] [int] NOT NULL,
	[NotificationTemplateId] [uniqueidentifier] NULL,
	[NotificationEventId] [int] NOT NULL,
	[NotificationCategoryId] [int] NOT NULL,
	[NotificationLinkId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_NOTIF_NOTIFICATION] PRIMARY KEY CLUSTERED 
(
	[NotificationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[NOTIF_NOTIFICATION]  WITH CHECK ADD  CONSTRAINT [FK_NOTIF_NOTIFICATION_MODULE] FOREIGN KEY([ModuleId])
REFERENCES [dbo].[MODULE] ([ModuleId])
GO

ALTER TABLE [dbo].[NOTIF_NOTIFICATION] CHECK CONSTRAINT [FK_NOTIF_NOTIFICATION_MODULE]
GO

ALTER TABLE [dbo].[NOTIF_NOTIFICATION]  WITH CHECK ADD  CONSTRAINT [FK_NOTIF_NOTIFICATION_NOTIF_NOTIFICATION_CATEGORY] FOREIGN KEY([NotificationCategoryId])
REFERENCES [dbo].[NOTIF_NOTIFICATION_CATEGORY] ([CategoryId])
GO

ALTER TABLE [dbo].[NOTIF_NOTIFICATION] CHECK CONSTRAINT [FK_NOTIF_NOTIFICATION_NOTIF_NOTIFICATION_CATEGORY]
GO

ALTER TABLE [dbo].[NOTIF_NOTIFICATION]  WITH CHECK ADD  CONSTRAINT [FK_NOTIF_NOTIFICATION_NOTIF_NOTIFICATION_COMMUNICATION_METHOD] FOREIGN KEY([NotificationCommunicationMethodId])
REFERENCES [dbo].[NOTIF_NOTIFICATION_COMMUNICATION_METHOD] ([CommunicationId])
GO

ALTER TABLE [dbo].[NOTIF_NOTIFICATION] CHECK CONSTRAINT [FK_NOTIF_NOTIFICATION_NOTIF_NOTIFICATION_COMMUNICATION_METHOD]
GO

ALTER TABLE [dbo].[NOTIF_NOTIFICATION]  WITH CHECK ADD  CONSTRAINT [FK_NOTIF_NOTIFICATION_NOTIF_NOTIFICATION_EVENT] FOREIGN KEY([NotificationEventId])
REFERENCES [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId])
GO

ALTER TABLE [dbo].[NOTIF_NOTIFICATION] CHECK CONSTRAINT [FK_NOTIF_NOTIFICATION_NOTIF_NOTIFICATION_EVENT]
GO

ALTER TABLE [dbo].[NOTIF_NOTIFICATION]  WITH CHECK ADD  CONSTRAINT [FK_NOTIF_NOTIFICATION_NOTIF_NOTIFICATION_LINK] FOREIGN KEY([NotificationLinkId])
REFERENCES [dbo].[NOTIF_NOTIFICATION_LINK] ([LinkId])
GO

ALTER TABLE [dbo].[NOTIF_NOTIFICATION] CHECK CONSTRAINT [FK_NOTIF_NOTIFICATION_NOTIF_NOTIFICATION_LINK]
GO

ALTER TABLE [dbo].[NOTIF_NOTIFICATION]  WITH CHECK ADD  CONSTRAINT [FK_NOTIF_NOTIFICATION_NOTIF_NOTIFICATION_RECEIVER_TYPE] FOREIGN KEY([ReceiverTypeId])
REFERENCES [dbo].[NOTIF_NOTIFICATION_RECEIVER_TYPE] ([TypeId])
GO

ALTER TABLE [dbo].[NOTIF_NOTIFICATION] CHECK CONSTRAINT [FK_NOTIF_NOTIFICATION_NOTIF_NOTIFICATION_RECEIVER_TYPE]
GO

ALTER TABLE [dbo].[NOTIF_NOTIFICATION]  WITH CHECK ADD  CONSTRAINT [FK_NOTIF_NOTIFICATION_NOTIF_NOTIFICATION_TEMPLATE] FOREIGN KEY([NotificationTemplateId])
REFERENCES [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId])
GO

ALTER TABLE [dbo].[NOTIF_NOTIFICATION] CHECK CONSTRAINT [FK_NOTIF_NOTIFICATION_NOTIF_NOTIFICATION_TEMPLATE]
GO

ALTER TABLE [dbo].[NOTIF_NOTIFICATION]  WITH CHECK ADD  CONSTRAINT [FK_NOTIF_NOTIFICATION_NOTIF_NOTIFICATION_TYPE] FOREIGN KEY([NotificationTypeId])
REFERENCES [dbo].[NOTIF_NOTIFICATION_TYPE] ([TypeId])
GO

ALTER TABLE [dbo].[NOTIF_NOTIFICATION] CHECK CONSTRAINT [FK_NOTIF_NOTIFICATION_NOTIF_NOTIFICATION_TYPE]
GO


 
GO

/****** Object:  Table [dbo].[NOTIF_NOTIFICATION_CATEGORY]    Script Date: 10/10/2022 4:05:17 pm ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[NOTIF_NOTIFICATION_CATEGORY]') AND type in (N'U'))
	DROP TABLE [dbo].[NOTIF_NOTIFICATION_CATEGORY]
GO

/****** Object:  Table [dbo].[NOTIF_NOTIFICATION_CATEGORY]    Script Date: 10/10/2022 4:05:17 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[NOTIF_NOTIFICATION_CATEGORY](
	[CategoryId] [int] NOT NULL,
	[NameEn] [nvarchar](100) NULL,
	[NameAr] [nvarchar](100) NULL,
	[DescriptionEn] [nvarchar](1000) NULL,
	[DescriptionAr] [nvarchar](1000) NULL,
	[Color] [nvarchar](50) NOT NULL,
	[Label] [nchar](100) NOT NULL,
 CONSTRAINT [PK_NOTIF_NOTIFICATION_CATEGORY] PRIMARY KEY CLUSTERED 
(
	[CategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


 
GO

/****** Object:  Table [dbo].[NOTIF_NOTIFICATION_COMMUNICATION_METHOD]    Script Date: 10/10/2022 4:05:43 pm ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[NOTIF_NOTIFICATION_COMMUNICATION_METHOD]') AND type in (N'U'))
	DROP TABLE [dbo].[NOTIF_NOTIFICATION_COMMUNICATION_METHOD]
GO

/****** Object:  Table [dbo].[NOTIF_NOTIFICATION_COMMUNICATION_METHOD]    Script Date: 10/10/2022 4:05:43 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[NOTIF_NOTIFICATION_COMMUNICATION_METHOD](
	[CommunicationId] [int] NOT NULL,
	[NameEn] [nvarchar](100) NULL,
	[NameAr] [nvarchar](100) NULL,
	[DescriptionEn] [nvarchar](1000) NULL,
	[DescriptionAr] [nvarchar](1000) NULL,
 CONSTRAINT [PK_NOTIF_NOTIFICATION_COMMUNICATION_METHOD] PRIMARY KEY CLUSTERED 
(
	[CommunicationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


 
GO

/****** Object:  Table [dbo].[NOTIF_NOTIFICATION_EVENT]    Script Date: 10/10/2022 4:06:13 pm ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[NOTIF_NOTIFICATION_EVENT]') AND type in (N'U'))
	DROP TABLE [dbo].[NOTIF_NOTIFICATION_EVENT]
GO

/****** Object:  Table [dbo].[NOTIF_NOTIFICATION_EVENT]    Script Date: 10/10/2022 4:06:13 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[NOTIF_NOTIFICATION_EVENT](
	[EventId] [int] NOT NULL,
	[NameEn] [nvarchar](50) NULL,
	[NameAr] [nvarchar](50) NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_NOTIF_NOTIFICATION_EVENT] PRIMARY KEY CLUSTERED 
(
	[EventId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


 
GO

/****** Object:  Table [dbo].[NOTIF_NOTIFICATION_LINK]    Script Date: 10/10/2022 4:06:37 pm ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[NOTIF_NOTIFICATION_LINK]') AND type in (N'U'))
	DROP TABLE [dbo].[NOTIF_NOTIFICATION_LINK]
GO

/****** Object:  Table [dbo].[NOTIF_NOTIFICATION_LINK]    Script Date: 10/10/2022 4:06:37 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[NOTIF_NOTIFICATION_LINK](
	[LinkId] [uniqueidentifier] NOT NULL,
	[Url] [nvarchar](250) NULL,
	[CreatedAt] [nvarchar](100) NOT NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[DeletedAt] [nvarchar](100) NULL,
	[DeletedBy] [nvarchar](100) NULL,
	[DeletedDate] [datetime] NULL,
	[isDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_NOTIF_NOTIFICATION_LINK] PRIMARY KEY CLUSTERED 
(
	[LinkId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


 
GO

/****** Object:  Table [dbo].[NOTIF_NOTIFICATION_RECEIVER_TYPE]    Script Date: 10/10/2022 4:06:54 pm ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[NOTIF_NOTIFICATION_RECEIVER_TYPE]') AND type in (N'U'))
	DROP TABLE [dbo].[NOTIF_NOTIFICATION_RECEIVER_TYPE]
GO

/****** Object:  Table [dbo].[NOTIF_NOTIFICATION_RECEIVER_TYPE]    Script Date: 10/10/2022 4:06:54 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[NOTIF_NOTIFICATION_RECEIVER_TYPE](
	[TypeId] [int] NOT NULL,
	[Name] [nvarchar](50) NULL,
 CONSTRAINT [PK_NOTIF_NOTIFICATION_RECEIVER_TYPE] PRIMARY KEY CLUSTERED 
(
	[TypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


 
GO

/****** Object:  Table [dbo].[NOTIF_NOTIFICATION_STATUS]    Script Date: 10/10/2022 4:07:15 pm ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[NOTIF_NOTIFICATION_STATUS]') AND type in (N'U'))
	DROP TABLE [dbo].[NOTIF_NOTIFICATION_STATUS]
GO

/****** Object:  Table [dbo].[NOTIF_NOTIFICATION_STATUS]    Script Date: 10/10/2022 4:07:15 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[NOTIF_NOTIFICATION_STATUS](
	[StatusId] [int] NOT NULL,
	[NameEn] [nvarchar](100) NULL,
	[NameAr] [nvarchar](100) NULL,
 CONSTRAINT [PK_NOTIF_NOTIFICATION_STATUS] PRIMARY KEY CLUSTERED 
(
	[StatusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO



 
GO

/****** Object:  Table [dbo].[NOTIF_NOTIFICATION_TEMPLATE]    Script Date: 10/10/2022 4:07:30 pm ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[NOTIF_NOTIFICATION_TEMPLATE]') AND type in (N'U'))
	DROP TABLE [dbo].[NOTIF_NOTIFICATION_TEMPLATE]
GO

/****** Object:  Table [dbo].[NOTIF_NOTIFICATION_TEMPLATE]    Script Date: 10/10/2022 4:07:30 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[NOTIF_NOTIFICATION_TEMPLATE](
	[TemplateId] [uniqueidentifier] NOT NULL,
	[NameEn] [nvarchar](100) NULL,
	[NameAr] [nvarchar](100) NULL,
	[SubjectEn] [nvarchar](150) NULL,
	[SubjectAr] [nvarchar](150) NULL,
	[Body] [nvarchar](1000) NULL,
	[Footer] [nvarchar](1000) NULL,
	[IsActive] [bit] NULL,
	[URL] [nvarchar](100) NULL,
	[LogoImagePath] [nvarchar](150) NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](100) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](100) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK_NOTIF_NOTIFICATION_TEMPLATE] PRIMARY KEY CLUSTERED 
(
	[TemplateId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


 
GO

/****** Object:  Table [dbo].[NOTIF_NOTIFICATION_TYPE]    Script Date: 10/10/2022 4:07:47 pm ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[NOTIF_NOTIFICATION_TYPE]') AND type in (N'U'))
	DROP TABLE [dbo].[NOTIF_NOTIFICATION_TYPE]
GO

/****** Object:  Table [dbo].[NOTIF_NOTIFICATION_TYPE]    Script Date: 10/10/2022 4:07:47 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[NOTIF_NOTIFICATION_TYPE](
	[TypeId] [int] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_NOTIF_NOTIFICATION_TYPE] PRIMARY KEY CLUSTERED 
(
	[TypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


 
GO

ALTER TABLE [dbo].[NOTIF_NOTIFICATION_USER] DROP CONSTRAINT [FK_NOTIF_NOTIFICATION_USER_UMS_USER]
GO

ALTER TABLE [dbo].[NOTIF_NOTIFICATION_USER] DROP CONSTRAINT [FK_NOTIF_NOTIFICATION_USER_NOTIF_NOTIFICATION_STATUS]
GO

ALTER TABLE [dbo].[NOTIF_NOTIFICATION_USER] DROP CONSTRAINT [FK_NOTIF_NOTIFICATION_USER_NOTIF_NOTIFICATION]
GO

/****** Object:  Table [dbo].[NOTIF_NOTIFICATION_USER]    Script Date: 10/10/2022 4:08:04 pm ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[NOTIF_NOTIFICATION_USER]') AND type in (N'U'))
	DROP TABLE [dbo].[NOTIF_NOTIFICATION_USER]
GO

/****** Object:  Table [dbo].[NOTIF_NOTIFICATION_USER]    Script Date: 10/10/2022 4:08:04 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[NOTIF_NOTIFICATION_USER](
	[Id] [uniqueidentifier] NOT NULL,
	[NotificationId] [uniqueidentifier] NOT NULL,
	[UserId] [nvarchar](450) NULL,
	[NotificationStatusId] [int] NULL,
	[ReadDate] [datetime] NULL,
	[NotificationMessage] [nvarchar](500) NULL,
 CONSTRAINT [PK_NOTIF_NOTIFICATION_USER] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[NOTIF_NOTIFICATION_USER]  WITH CHECK ADD  CONSTRAINT [FK_NOTIF_NOTIFICATION_USER_NOTIF_NOTIFICATION] FOREIGN KEY([NotificationId])
REFERENCES [dbo].[NOTIF_NOTIFICATION] ([NotificationId])
GO

ALTER TABLE [dbo].[NOTIF_NOTIFICATION_USER] CHECK CONSTRAINT [FK_NOTIF_NOTIFICATION_USER_NOTIF_NOTIFICATION]
GO

ALTER TABLE [dbo].[NOTIF_NOTIFICATION_USER]  WITH CHECK ADD  CONSTRAINT [FK_NOTIF_NOTIFICATION_USER_NOTIF_NOTIFICATION_STATUS] FOREIGN KEY([NotificationStatusId])
REFERENCES [dbo].[NOTIF_NOTIFICATION_STATUS] ([StatusId])
GO

ALTER TABLE [dbo].[NOTIF_NOTIFICATION_USER] CHECK CONSTRAINT [FK_NOTIF_NOTIFICATION_USER_NOTIF_NOTIFICATION_STATUS]
GO

ALTER TABLE [dbo].[NOTIF_NOTIFICATION_USER]  WITH CHECK ADD  CONSTRAINT [FK_NOTIF_NOTIFICATION_USER_UMS_USER] FOREIGN KEY([UserId])
REFERENCES [dbo].[UMS_USER] ([Id])
GO

ALTER TABLE [dbo].[NOTIF_NOTIFICATION_USER] CHECK CONSTRAINT [FK_NOTIF_NOTIFICATION_USER_UMS_USER]
GO


GO

GO

/****** Object:  Table [dbo].[LEGAL_LEGISLATION_TAG]    Script Date: 13/10/2022 12:21:49 am ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_LEGISLATION_TAG]') AND type in (N'U'))
	DROP TABLE [dbo].[LEGAL_LEGISLATION_TAG]
GO

/****** Object:  Table [dbo].[LEGAL_LEGISLATION_TAG]    Script Date: 13/10/2022 12:21:49 am ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LEGAL_LEGISLATION_TAG](
	[TagId] [int] IDENTITY(1,1) NOT NULL,
	[TagName] [nvarchar](500) NOT NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](100) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](100) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_LEGAL_LEGISLATION_TAG] PRIMARY KEY CLUSTERED 
(
	[TagId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


GO

/****** Object:  Table [dbo].[LEGAL_LEGISLATION_REFERENCE]    Script Date: 13/10/2022 12:21:49 am ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_LEGISLATION_REFERENCE]') AND type in (N'U'))
	DROP TABLE [dbo].[LEGAL_LEGISLATION_REFERENCE]
GO

/****** Object:  Table [dbo].[LEGAL_LEGISLATION_REFERENCE]    Script Date: 13/10/2022 12:21:49 am ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LEGAL_LEGISLATION_REFERENCE](

	[ReferenceId] [int] IDENTITY(1,1) NOT NULL,
	[Reference_Parent_Id] [uniqueidentifier] NOT NULL,
	[Legislation_Link_Id] [uniqueidentifier] NOT NULL,
	[Legislation_Link] [nvarchar](500) NOT NULL,
 CONSTRAINT [PK_LEGAL_LEGISLATION_REFERENCE] PRIMARY KEY CLUSTERED 
(
	[ReferenceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


/******  
<Object Scope='Public'> LEGAL_TEMPLATE
<Copyright> Digital Processing System </Copyright>  
<Author Company='Business Analytics'> Umer Zaman </Author>  
<Created> 2022-10-28 </Created>  
<History Author='' Date=''>  </History>  
</Object>  
******/ 
-- [dbo].[LEGAL_TEMPLATE] 

GO

/****** Object:  Table [dbo].[LEGAL_TEMPLATE]    Script Date: 12/10/2022 3:46:20 pm ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_TEMPLATE]') AND type in (N'U'))
DROP TABLE [dbo].[LEGAL_TEMPLATE]
GO

/****** Object:  Table [dbo].[LEGAL_TEMPLATE]    Script Date: 12/10/2022 3:46:20 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LEGAL_TEMPLATE](
	[TemplateId] [uniqueidentifier] NOT NULL,
	[Template_Name] [nvarchar] (1000) NOT NULL,
	[Legislation_Type] [int] NOT NULL
 CONSTRAINT [PK_LEGAL_TEMPLATE] PRIMARY KEY CLUSTERED 
(
	[TemplateId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/******  
<Object Scope='Public'> LEGAL_TEMPLATE_SETTING
<Copyright> Digital Processing System </Copyright>  
<Author Company='Business Analytics'> Umer Zaman </Author>  
<Created> 2022-10-28 </Created>  
<History Author='' Date=''>  </History>  
</Object>  
******/ 
-- [dbo].[LEGAL_TEMPLATE_SETTING] 

GO

/****** Object:  Table [dbo].[LEGAL_TEMPLATE_SETTING]    Script Date: 12/10/2022 3:46:20 pm ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_TEMPLATE_SETTING]') AND type in (N'U'))
DROP TABLE [dbo].[LEGAL_TEMPLATE_SETTING]
GO

/****** Object:  Table [dbo].[LEGAL_TEMPLATE_SETTING]    Script Date: 12/10/2022 3:46:20 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LEGAL_TEMPLATE_SETTING](
	[TemplateSettingId] [int] NOT NULL,
	[Template_Heading] [nvarchar] (250) NOT NULL,
	[Template_Value] [nvarchar] (500) NOT NULL
 CONSTRAINT [PK_LEGAL_TEMPLATE_SETTING] PRIMARY KEY CLUSTERED 
(
	[TemplateSettingId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/******  
<Object Scope='Public'> LEGAL_LEGISLATION_LEGAL_TEMPLATE
<Copyright> Digital Processing System </Copyright>  
<Author Company='Business Analytics'> Umer Zaman </Author>  
<Created> 2022-10-28 </Created>  
<History Author='' Date=''>  </History>  
</Object>  
******/ 
-- [dbo].[LEGAL_LEGISLATION_LEGAL_TEMPLATE] 

GO

/****** Object:  Table [dbo].[LEGAL_LEGISLATION_LEGAL_TEMPLATE]    Script Date: 12/10/2022 3:46:20 pm ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_LEGISLATION_LEGAL_TEMPLATE]') AND type in (N'U'))
DROP TABLE [dbo].[LEGAL_LEGISLATION_LEGAL_TEMPLATE]
GO

/****** Object:  Table [dbo].[LEGAL_LEGISLATION_LEGAL_TEMPLATE]    Script Date: 12/10/2022 3:46:20 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LEGAL_LEGISLATION_LEGAL_TEMPLATE](
	[Id] [int] IDENTITY (1,1) NOT NULL,
	[LegislationId] [uniqueidentifier] NOT NULL,
	[TemplateId] [uniqueidentifier] NOT NULL,
	[TemplateSettingId] [int] NOT NULL
 CONSTRAINT [PK_LEGAL_LEGISLATION_LEGAL_TEMPLATE] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

--------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------

ALTER TABLE [dbo].[LEGAL_PRINCIPLE] DROP CONSTRAINT [FK_LEGAL_PRINCIPLE_SUB_CATEGORY]
GO

ALTER TABLE [dbo].[LEGAL_PRINCIPLE] DROP CONSTRAINT [FK_LEGAL_PRINCIPLE_CATEGORY]
GO

/****** Object:  Table [dbo].[LEGAL_PRINCIPLE]    Script Date: 12/1/2022 2:49:27 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_PRINCIPLE]') AND type in (N'U'))
DROP TABLE [dbo].[LEGAL_PRINCIPLE]
GO

/****** Object:  Table [dbo].[LEGAL_PRINCIPLE]    Script Date: 12/1/2022 2:49:27 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LEGAL_PRINCIPLE](
	[PrincipleId] [uniqueidentifier] NOT NULL,
	[Principle_Type] [int] NOT NULL,
	[Principle_Number] [int] NOT NULL,
	[IssueDate] [datetime] NOT NULL,
	[IssueDate_Hijri] [datetime] NOT NULL,
	[BaseNumber] [int] NULL,
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

ALTER TABLE [dbo].[LEGAL_PRINCIPLE]  WITH CHECK ADD  CONSTRAINT [FK_LEGAL_PRINCIPLE_CATEGORY] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[LEGAL_PRINCIPLE_CATEGORY_FTW_LKP] ([Id])
GO

ALTER TABLE [dbo].[LEGAL_PRINCIPLE] CHECK CONSTRAINT [FK_LEGAL_PRINCIPLE_CATEGORY]
GO

ALTER TABLE [dbo].[LEGAL_PRINCIPLE]  WITH CHECK ADD  CONSTRAINT [FK_LEGAL_PRINCIPLE_SUB_CATEGORY] FOREIGN KEY([SubCategoryId])
REFERENCES [dbo].[LEGAL_PRINCIPLE_SUB_CATEGORY_FTW_LKP] ([Id])
GO

ALTER TABLE [dbo].[LEGAL_PRINCIPLE] CHECK CONSTRAINT [FK_LEGAL_PRINCIPLE_SUB_CATEGORY]
GO
----------------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------



GO
/****** Object:  Table [dbo].[LEGAL_PRINCIPLE_ARTICLE]    Script Date: 14/11/2022 2:55:19 pm ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_PRINCIPLE_ARTICLE]') AND type in (N'U'))
DROP TABLE [dbo].[dbo].[LEGAL_PRINCIPLE_ARTICLE]
GO
/******  
<Object Scope='Public'> LEGAL_PRINCIPLE
<Copyright> Digital Processing System </Copyright>  
<Author Company='Business Analytics'> Ijaz Ahmad </Author>  
<Created> 2022-11-14 </Created>  
<History Author='' Date=''>  </History>  
</Object>  
******/ 
-- [dbo].[LEGAL_PRINCIPLE_ARTICLE] 
/****** Object:  Table [dbo].[LEGAL_PRINCIPLE_ARTICLE]    Script Date: 14/11/2022 3:05:16 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LEGAL_PRINCIPLE_ARTICLE](
	[ArticleId] [uniqueidentifier] NOT NULL,
	[PrincipleId] [uniqueidentifier] NOT NULL,
	[Article_Name_En] [nvarchar](500) NULL,
	[Article_Name_Ar] [nvarchar](500) NULL,
	[ArticleTitle] [nvarchar](500) NULL,
	[Start_Date] [datetime] NULL,
	[Article_Status] [int] NULL,
	[End_Date] [datetime] NULL,
	[Article_Text_En] [nvarchar](max) NULL,
	[Article_Text_Ar] [nvarchar](max) NULL,
	[NextArticleId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_LEGAL_PRINCIPLE_ARTICLE] PRIMARY KEY CLUSTERED 
(
	[ArticleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LEGAL_PRINCIPLE_ARTICLE_CHILD]    Script Date: 14/11/2022 2:55:19 pm ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_PRINCIPLE_ARTICLE_CHILD]') AND type in (N'U'))
DROP TABLE [dbo].[dbo].[LEGAL_PRINCIPLE_ARTICLE_CHILD]
GO
/******  
<Object Scope='Public'> LEGAL_PRINCIPLE_ARTICLE_CHILD
<Copyright> Digital Processing System </Copyright>  
<Author Company='Business Analytics'> Ijaz Ahmad </Author>  
<Created> 2022-11-14 </Created>  
<History Author='' Date=''>  </History>  
</Object>  
******/ 
-- [dbo].[LEGAL_PRINCIPLE_ARTICLE_CHILD]
/****** Object:  Table [dbo].[LEGAL_PRINCIPLE_ARTICLE_CHILD]    Script Date: 14/11/2022 3:06:53 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LEGAL_PRINCIPLE_ARTICLE_CHILD](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ParentId] [uniqueidentifier] NOT NULL,
	[ArticleChildId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_LEGAL_PRINCIPLE_ARTICLE_CHILD] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LEGAL_PRINCIPLE_ARTICLE_SOURCE]    Script Date: 14/11/2022 2:55:19 pm ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_PRINCIPLE_ARTICLE_SOURCE]') AND type in (N'U'))
DROP TABLE [dbo].[dbo].[LEGAL_PRINCIPLE_ARTICLE_SOURCE]
GO
/******  
<Object Scope='Public'> LEGAL_PRINCIPLE_ARTICLE_SOURCE
<Copyright> Digital Processing System </Copyright>  
<Author Company='Business Analytics'> Ijaz Ahmad </Author>  
<Created> 2022-11-14 </Created>  
<History Author='' Date=''>  </History>  
</Object>  
******/ 
-- [dbo].[LEGAL_PRINCIPLE_ARTICLE_SOURCE]
/****** Object:  Table [dbo].[LEGAL_PRINCIPLE_ARTICLE_SOURCE]    Script Date: 14/11/2022 3:09:17 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LEGAL_PRINCIPLE_ARTICLE_SOURCE](
	[Id] [int] NOT NULL,
	[Name_En] [nvarchar](50) NOT NULL,
	[Name_Ar] [nvarchar](50) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LEGAL_PRINCIPLE_CONCLUSION]    Script Date: 14/11/2022 2:55:19 pm ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_PRINCIPLE_CONCLUSION]') AND type in (N'U'))
DROP TABLE [dbo].[dbo].[LEGAL_PRINCIPLE_CONCLUSION]
GO
/******  
<Object Scope='Public'> LEGAL_PRINCIPLE_CONCLUSION
<Copyright> Digital Processing System </Copyright>  
<Author Company='Business Analytics'> Ijaz Ahmad </Author>  
<Created> 2022-11-14 </Created>  
<History Author='' Date=''>  </History>  
</Object>  
******/ 
-- [dbo].[LEGAL_PRINCIPLE_CONCLUSION]
/****** Object:  Table [dbo].[LEGAL_PRINCIPLE_CONCLUSION]    Script Date: 14/11/2022 3:11:54 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LEGAL_PRINCIPLE_CONCLUSION](
	[ConclusionId] [int] IDENTITY(1,1) NOT NULL,
	[PrincipleId] [uniqueidentifier] NOT NULL,
	[Body_En] [nvarchar](max) NULL,
	[Body_Ar] [nvarchar](max) NULL,
 CONSTRAINT [PK_LEGAL_PRINCIPLE_CONCLUSION] PRIMARY KEY CLUSTERED 
(
	[ConclusionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LEGAL_PRINCIPLE_FLOW_STATUS]    Script Date: 14/11/2022 2:55:19 pm ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_PRINCIPLE_FLOW_STATUS]') AND type in (N'U'))
DROP TABLE [dbo].[dbo].[LEGAL_PRINCIPLE_FLOW_STATUS]
GO
/******  
<Object Scope='Public'> LEGAL_PRINCIPLE_FLOW_STATUS
<Copyright> Digital Processing System </Copyright>  
<Author Company='Business Analytics'> Ijaz Ahmad </Author>  
<Created> 2022-11-14 </Created>  
<History Author='' Date=''>  </History>  
</Object>  
******/ 
-- [dbo].[LEGAL_PRINCIPLE_FLOW_STATUS]
/****** Object:  Table [dbo].[LEGAL_PRINCIPLE_FLOW_STATUS]    Script Date: 14/11/2022 3:13:47 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LEGAL_PRINCIPLE_FLOW_STATUS](
	[Id] [int] NOT NULL,
	[Name_En] [nvarchar](250) NOT NULL,
	[Name_Ar] [nvarchar](250) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LEGAL_PRINCIPLE_NOTE]    Script Date: 14/11/2022 2:55:19 pm ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_PRINCIPLE_NOTE]') AND type in (N'U'))
DROP TABLE [dbo].[dbo].[LEGAL_PRINCIPLE_NOTE]
GO
/******  
<Object Scope='Public'> LEGAL_PRINCIPLE_NOTE
<Copyright> Digital Processing System </Copyright>  
<Author Company='Business Analytics'> Ijaz Ahmad </Author>  
<Created> 2022-11-14 </Created>  
<History Author='' Date=''>  </History>  
</Object>  
******/ 
-- [dbo].[LEGAL_PRINCIPLE_NOTE]
/****** Object:  Table [dbo].[LEGAL_PRINCIPLE_NOTE]    Script Date: 14/11/2022 3:16:30 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LEGAL_PRINCIPLE_NOTE](
	[NoteId] [int] IDENTITY(1,1) NOT NULL,
	[ParentId] [uniqueidentifier] NOT NULL,
	[Note_Text_En] [nvarchar](max) NULL,
	[Note_Text_Ar] [nvarchar](max) NULL,
	[Note_Location] [nvarchar](max) NOT NULL,
	[Note_Date] [datetime] NOT NULL,
 CONSTRAINT [PK_LEGAL_PRINCIPLE_NOTE] PRIMARY KEY CLUSTERED 
(
	[NoteId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LEGAL_PRINCIPLE_PUBLICATION_SOURCE]    Script Date: 14/11/2022 2:55:19 pm ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_PRINCIPLE_PUBLICATION_SOURCE]') AND type in (N'U'))
DROP TABLE [dbo].[dbo].[LEGAL_PRINCIPLE_PUBLICATION_SOURCE]
GO
/******  
<Object Scope='Public'> LEGAL_PRINCIPLE_PUBLICATION_SOURCE
<Copyright> Digital Processing System </Copyright>  
<Author Company='Business Analytics'> Ijaz Ahmad </Author>  
<Created> 2022-11-14 </Created>  
<History Author='' Date=''>  </History>  
</Object>  
******/ 
-- [dbo].[LEGAL_PRINCIPLE_PUBLICATION_SOURCE]
/****** Object:  Table [dbo].[LEGAL_PRINCIPLE_PUBLICATION_SOURCE]    Script Date: 14/11/2022 3:17:54 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LEGAL_PRINCIPLE_PUBLICATION_SOURCE](
	[SourceId] [int] IDENTITY(1,1) NOT NULL,
	[PublicationNameId] [int] NOT NULL,
	[PrincipleId] [uniqueidentifier] NOT NULL,
	[Issue_Number] [nvarchar](100) NOT NULL,
	[PublicationDate] [datetime] NOT NULL,
	[PublicationDate_Hijri] [datetime] NOT NULL,
	[Page_Start] [int] NOT NULL,
	[Page_End] [int] NOT NULL,
 CONSTRAINT [PK_LEGAL_PRINCIPLE_PUBLICATION_SOURCE] PRIMARY KEY CLUSTERED 
(
	[SourceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LEGAL_PRINCIPLE_PUBLICATION_SOURCE_NAME]    Script Date: 14/11/2022 2:55:19 pm ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_PRINCIPLE_PUBLICATION_SOURCE_NAME]') AND type in (N'U'))
DROP TABLE [dbo].[dbo].[LEGAL_PRINCIPLE_PUBLICATION_SOURCE_NAME]
GO
/******  
<Object Scope='Public'> LEGAL_PRINCIPLE_PUBLICATION_SOURCE_NAME
<Copyright> Digital Processing System </Copyright>  
<Author Company='Business Analytics'> Ijaz Ahmad </Author>  
<Created> 2022-11-14 </Created>  
<History Author='' Date=''>  </History>  
</Object>  
******/ 
-- [dbo].[LEGAL_PRINCIPLE_PUBLICATION_SOURCE_NAME]
/****** Object:  Table [dbo].[LEGAL_PRINCIPLE_PUBLICATION_SOURCE_NAME]    Script Date: 14/11/2022 3:20:47 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LEGAL_PRINCIPLE_PUBLICATION_SOURCE_NAME](
	[PublicationNameId] [int] IDENTITY(1,1) NOT NULL,
	[Name_En] [nvarchar](500) NOT NULL,
	[Name_Ar] [nvarchar](500) NOT NULL,
 CONSTRAINT [PK_LEGAL_PRINCIPLE_PUBLICATION_SOURCE_NAME] PRIMARY KEY CLUSTERED 
(
	[PublicationNameId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LEGAL_PRINCIPLE_REFERENCE]    Script Date: 14/11/2022 6:22:01 pm ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_PRINCIPLE_REFERENCE]') AND type in (N'U'))
DROP TABLE [dbo].[LEGAL_PRINCIPLE_REFERENCE]
GO

/****** Object:  Table [dbo].[LEGAL_PRINCIPLE_REFERENCE]    Script Date: 14/11/2022 6:22:01 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LEGAL_PRINCIPLE_REFERENCE](
	[ReferenceId] [int] IDENTITY(1,1) NOT NULL,
	[Reference_Parent_Id] [uniqueidentifier] NOT NULL,
	[Principle_Link_Id] [uniqueidentifier] NOT NULL,
	[Principle_Link] [nvarchar](500) NOT NULL,
	[LegalPrincipleReferenceTypeId] [int] NULL,
 CONSTRAINT [PK_LEGAL_PRINCIPLE_REFERENCE] PRIMARY KEY CLUSTERED 
(
	[ReferenceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[LEGAL_PRINCIPLE_STATUS]    Script Date: 14/11/2022 2:55:19 pm ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_PRINCIPLE_STATUS]') AND type in (N'U'))
DROP TABLE [dbo].[dbo].[LEGAL_PRINCIPLE_STATUS]
GO
/******  
<Object Scope='Public'> LEGAL_PRINCIPLE_STATUS
<Copyright> Digital Processing System </Copyright>  
<Author Company='Business Analytics'> Ijaz Ahmad </Author>  
<Created> 2022-11-14 </Created>  
<History Author='' Date=''>  </History>  
</Object>  
******/ 
-- [dbo].LEGAL_PRINCIPLE_STATUS]
/****** Object:  Table [dbo].[LEGAL_PRINCIPLE_STATUS]    Script Date: 14/11/2022 3:24:40 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LEGAL_PRINCIPLE_STATUS](
	[Id] [int] NOT NULL,
	[Name_En] [nvarchar](250) NOT NULL,
	[Name_Ar] [nvarchar](250) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LEGAL_PRINCIPLE_SUMMARY]    Script Date: 14/11/2022 2:55:19 pm ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_PRINCIPLE_SUMMARY]') AND type in (N'U'))
DROP TABLE [dbo].[dbo].[LEGAL_PRINCIPLE_SUMMARY]
GO
/******  
<Object Scope='Public'> LEGAL_PRINCIPLE_SUMMARY
<Copyright> Digital Processing System </Copyright>  
<Author Company='Business Analytics'> Ijaz Ahmad </Author>  
<Created> 2022-11-14 </Created>  
<History Author='' Date=''>  </History>  
</Object>  
******/ 
-- [dbo].LEGAL_PRINCIPLE_SUMMARY]
/****** Object:  Table [dbo].[LEGAL_PRINCIPLE_SUMMARY]    Script Date: 14/11/2022 3:26:20 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LEGAL_PRINCIPLE_SUMMARY](
	[SummaryId] [int] IDENTITY(1,1) NOT NULL,
	[ParentId] [uniqueidentifier] NOT NULL,
	[Summary_Text_En] [nvarchar](max) NULL,
	[Summary_Text_Ar] [nvarchar](max) NULL,
	[Summary_Location] [nvarchar](max) NOT NULL,
	[Summary_Date] [datetime] NOT NULL,
 CONSTRAINT [PK_LEGAL_PRINCIPLE_SUMMARY] PRIMARY KEY CLUSTERED 
(
	[SummaryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


/****** Object:  Table [dbo].[LEGAL_PRINCIPLE_LEGAL_TAG]    Script Date: 11/15/2022 6:20:47 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_PRINCIPLE_LEGAL_TAG]') AND type in (N'U'))
DROP TABLE [dbo].[LEGAL_PRINCIPLE_LEGAL_TAG]
GO

/****** Object:  Table [dbo].[LEGAL_PRINCIPLE_LEGAL_TAG]    Script Date: 11/15/2022 6:20:47 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LEGAL_PRINCIPLE_LEGAL_TAG](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PrincipleId] [uniqueidentifier] NOT NULL,
	[TagId] [int] NOT NULL,
 CONSTRAINT [PK_LEGAL_PRINCIPLE_LEGAL_TAG] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LEGAL_PRINCIPLE_TAG]    Script Date: 11/15/2022 6:23:35 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_PRINCIPLE_TAG]') AND type in (N'U'))
DROP TABLE [dbo].[LEGAL_PRINCIPLE_TAG]
GO

/****** Object:  Table [dbo].[LEGAL_PRINCIPLE_TAG]    Script Date: 11/15/2022 6:23:35 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LEGAL_PRINCIPLE_TAG](
	[TagId] [int] IDENTITY(1,1) NOT NULL,
	[Name_En] [nvarchar](200) NOT NULL,
	[Name_Ar] [nvarchar](200) NOT NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](100) NULL,
	[ModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](100) NULL,
	[DeletedDate] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_LEGAL_PRINCIPLE_TAG] PRIMARY KEY CLUSTERED 
(
	[TagId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO



/****** Object:  Table [dbo].[LEGAL_PRINCIPLE_TYPE]    Script Date: 14/11/2022 2:55:19 pm ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_PRINCIPLE_TYPE]') AND type in (N'U'))
DROP TABLE [dbo].[dbo].[LEGAL_PRINCIPLE_TYPE]
GO
/******  
<Object Scope='Public'> LEGAL_PRINCIPLE_TYPE
<Copyright> Digital Processing System </Copyright>  
<Author Company='Business Analytics'> Ijaz Ahmad </Author>  
<Created> 2022-11-14 </Created>  
<History Author='' Date=''>  </History>  
</Object>  
******/ 
-- [dbo].LEGAL_PRINCIPLE_TYPE]
/****** Object:  Table [dbo].[LEGAL_PRINCIPLE_TYPE]    Script Date: 14/11/2022 3:29:46 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LEGAL_PRINCIPLE_TYPE](
	[Id] [int] NOT NULL,
	[Name_En] [nvarchar](50) NOT NULL,
	[Name_Ar] [nvarchar](50) NOT NULL
) ON [PRIMARY]

/****** Object:  Table [dbo].[LEGAL_PRINCIPLE_REFERENCE_TYPE]    Script Date: 11/17/2022 10:12:52 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_PRINCIPLE_REFERENCE_Type]') AND type in (N'U'))
DROP TABLE [dbo].[LEGAL_PRINCIPLE_REFERENCE_TYPE]
GO

/****** Object:  Table [dbo].[LEGAL_PRINCIPLE_REFERENCE_TYPE]    Script Date: 11/17/2022 10:12:52 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LEGAL_PRINCIPLE_REFERENCE_TYPE](
	[ReferenceTypeId] [int]  NOT NULL,
	[Name_En] [nvarchar](max) NULL,
	[Name_Ar] [nvarchar](max) NULL,
 CONSTRAINT [PK_LEGAL_PRINCIPLE_REFERENCE_TYPE] PRIMARY KEY CLUSTERED 
(
	[ReferenceTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO



/****** Object:  Table [dbo].[LEGAL_PRINCIPLE_CATEGORY_FTW_LKP]    Script Date: 12/1/2022 2:30:07 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_PRINCIPLE_CATEGORY_FTW_LKP]') AND type in (N'U'))
DROP TABLE [dbo].[LEGAL_PRINCIPLE_CATEGORY_FTW_LKP]
GO

/****** Object:  Table [dbo].[LEGAL_PRINCIPLE_CATEGORY_FTW_LKP]    Script Date: 12/1/2022 2:30:07 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LEGAL_PRINCIPLE_CATEGORY_FTW_LKP](
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
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

---------------------------------------------------------------

/****** Object:  Table [dbo].[LEGAL_PRINCIPLE_SUB_CATEGORY_FTW_LKP]    Script Date: 12/1/2022 2:30:07 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_PRINCIPLE_SUB_CATEGORY_FTW_LKP]') AND type in (N'U'))
DROP TABLE [dbo].[LEGAL_PRINCIPLE_SUB_CATEGORY_FTW_LKP]
GO

/****** Object:  Table [dbo].[LEGAL_PRINCIPLE_SUB_CATEGORY_FTW_LKP]    Script Date: 12/1/2022 2:30:07 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LEGAL_PRINCIPLE_SUB_CATEGORY_FTW_LKP](
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
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

---------------------------------------------[LEGAL_PRINCIPLE]---------------------------------------------------------------
ALTER TABLE [dbo].[LEGAL_PRINCIPLE] DROP CONSTRAINT [FK_LEGAL_PRINCIPLE_SUB_CATEGORY]
GO

ALTER TABLE [dbo].[LEGAL_PRINCIPLE] DROP CONSTRAINT [FK_LEGAL_PRINCIPLE_CATEGORY]
GO

/****** Object:  Table [dbo].[LEGAL_PRINCIPLE]    Script Date: 12/8/2022 4:03:48 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_PRINCIPLE]') AND type in (N'U'))
DROP TABLE [dbo].[LEGAL_PRINCIPLE]
GO

/****** Object:  Table [dbo].[LEGAL_PRINCIPLE]    Script Date: 12/8/2022 4:03:48 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LEGAL_PRINCIPLE](
	[PrincipleId] [uniqueidentifier] NOT NULL,
	[Principle_Type] [int] NOT NULL,
	[Principle_Number] [int] NOT NULL,
	[IssueDate] [datetime] NOT NULL,
	[IssueDate_Hijri] [datetime] NOT NULL,
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

ALTER TABLE [dbo].[LEGAL_PRINCIPLE]  WITH CHECK ADD  CONSTRAINT [FK_LEGAL_PRINCIPLE_CATEGORY] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[LEGAL_PRINCIPLE_CATEGORY_FTW_LKP] ([Id])
GO

ALTER TABLE [dbo].[LEGAL_PRINCIPLE] CHECK CONSTRAINT [FK_LEGAL_PRINCIPLE_CATEGORY]
GO

ALTER TABLE [dbo].[LEGAL_PRINCIPLE]  WITH CHECK ADD  CONSTRAINT [FK_LEGAL_PRINCIPLE_SUB_CATEGORY] FOREIGN KEY([SubCategoryId])
REFERENCES [dbo].[LEGAL_PRINCIPLE_SUB_CATEGORY_FTW_LKP] ([Id])
GO

ALTER TABLE [dbo].[LEGAL_PRINCIPLE] CHECK CONSTRAINT [FK_LEGAL_PRINCIPLE_SUB_CATEGORY]
GO
-----------------------------------LEGAL_PRINCIPLE_CATEGORY_FTW_LKP-------------------------------------------------
--------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------

/****** Object:  Table [dbo].[LEGAL_PRINCIPLE_CATEGORY_FTW_LKP]    Script Date: 12/9/2022 3:33:52 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_PRINCIPLE_CATEGORY_FTW_LKP]') AND type in (N'U'))
DROP TABLE [dbo].[LEGAL_PRINCIPLE_CATEGORY_FTW_LKP]
GO

/****** Object:  Table [dbo].[LEGAL_PRINCIPLE_CATEGORY_FTW_LKP]    Script Date: 12/9/2022 3:33:52 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

 Create TABLE [dbo].[LEGAL_PRINCIPLE_CATEGORY_FTW_LKP](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CategoryName] [nvarchar](500) NOT NULL,
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

-------------------------------------------------------------------------------------------------------------------
-------------------------------------------------------------------------------------------------------------------
-----------------------------------------------------LEGAL_PRINCIPLE_SUB_CATEGORY_FTW_LKP-------------------------
-------------------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------------------
USE [FATWA_DB_DEV]
GO

/****** Object:  Table [dbo].[LEGAL_PRINCIPLE_SUB_CATEGORY_FTW_LKP]    Script Date: 12/9/2022 3:41:08 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_PRINCIPLE_SUB_CATEGORY_FTW_LKP]') AND type in (N'U'))
DROP TABLE [dbo].[LEGAL_PRINCIPLE_SUB_CATEGORY_FTW_LKP]
GO

/****** Object:  Table [dbo].[LEGAL_PRINCIPLE_SUB_CATEGORY_FTW_LKP]    Script Date: 12/9/2022 3:41:08 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LEGAL_PRINCIPLE_SUB_CATEGORY_FTW_LKP](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SubCategoryName] [nvarchar](500) NOT NULL,
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

-------------------------------------------------------------------------------------------------------------
-------------------------------------------------------------------------------------------------------------
------------------------------------------LEGAL_PRINCIPLE_LEGAL_CATEGORY-------------------------------------
--------------------------------------------------------------------------------------------------------------
/****** Object:  Table [dbo].[LEGAL_PRINCIPLE_LEGAL_CATEGORY]    Script Date: 12/9/2022 5:36:13 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_PRINCIPLE_LEGAL_CATEGORY]') AND type in (N'U'))
DROP TABLE [dbo].[LEGAL_PRINCIPLE_LEGAL_CATEGORY]
GO

/****** Object:  Table [dbo].[LEGAL_PRINCIPLE_LEGAL_CATEGORY]    Script Date: 12/9/2022 5:36:13 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LEGAL_PRINCIPLE_LEGAL_CATEGORY](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PrincipleId] [uniqueidentifier] NOT NULL,
	[CategoryId] [int] NOT NULL,
 CONSTRAINT [PK_LEGAL_PRINCIPLE_LEGAL_CATEGORY] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
-------------------------------------------------------------------------------------------------------------
-------------------------------------------------------------------------------------------------------------
------------------------------------------LEGAL_PRINCIPLE_LEGAL_SUB_CATEGORY-------------------------------------
--------------------------------------------------------------------------------------------------------------
/****** Object:  Table [dbo].[LEGAL_PRINCIPLE_LEGAL_SUB_CATEGORY]    Script Date: 12/9/2022 5:37:33 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_PRINCIPLE_LEGAL_SUB_CATEGORY]') AND type in (N'U'))
DROP TABLE [dbo].[LEGAL_PRINCIPLE_LEGAL_SUB_CATEGORY]
GO

/****** Object:  Table [dbo].[LEGAL_PRINCIPLE_LEGAL_SUB_CATEGORY]    Script Date: 12/9/2022 5:37:33 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LEGAL_PRINCIPLE_LEGAL_SUB_CATEGORY](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PrincipleId] [uniqueidentifier] NOT NULL,
	[CategoryId] [int] NOT NULL,
	[SubCategoryId] [int] NOT NULL,
 CONSTRAINT [PK_LEGAL_PRINCIPLE_LEGAL_SUB_CATEGORY] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

----------------------------------------------------------------------------
-----------------------------------------------------------------------------
--------------------------------------------------------------------------------
USE [FATWA_DB_DEV]
GO

ALTER TABLE [dbo].[LEGAL_PRINCIPLE] DROP CONSTRAINT [FK_LEGAL_PRINCIPLE_SUB_CATEGORY]
GO

ALTER TABLE [dbo].[LEGAL_PRINCIPLE] DROP CONSTRAINT [FK_LEGAL_PRINCIPLE_CATEGORY]
GO


/****** Object:  Table [dbo].[LEGAL_PRINCIPLE]    Script Date: 12/9/2022 7:30:05 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_PRINCIPLE]') AND type in (N'U'))
DROP TABLE [dbo].[LEGAL_PRINCIPLE]
GO

/****** Object:  Table [dbo].[LEGAL_PRINCIPLE]    Script Date: 12/9/2022 7:30:05 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LEGAL_PRINCIPLE](
	[PrincipleId] [uniqueidentifier] NOT NULL,
	[Principle_Type] [int] NOT NULL,
	[Principle_Number] [int] NOT NULL,
	[IssueDate] [datetime] NOT NULL,
	[IssueDate_Hijri] [datetime] NOT NULL,
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
 CONSTRAINT [PK_LEGAL_PRINCIPLE] PRIMARY KEY CLUSTERED 
(
	[PrincipleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


















