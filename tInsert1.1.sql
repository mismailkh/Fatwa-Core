 
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[NOTIF_NOTIFICATION_CATEGORY]') AND type in (N'U'))
	INSERT [dbo].[NOTIF_NOTIFICATION_CATEGORY] ([CategoryId], [NameEn], [NameAr], [DescriptionEn], [DescriptionAr], [Color], [Label]) VALUES (1, N'Urgent', N'Urgent Arbi', N'Urgent Desc', N'Urgent Desc Arbi', N'Red', N'URGENT                                                                                              ')
	INSERT [dbo].[NOTIF_NOTIFICATION_CATEGORY] ([CategoryId], [NameEn], [NameAr], [DescriptionEn], [DescriptionAr], [Color], [Label]) VALUES (2, N'Normal', N'Normal Arbi', N'Normal Desc', N'Normal Desc Arbi', N'Green', N'NORMAL                                                                                              ')
	INSERT [dbo].[NOTIF_NOTIFICATION_CATEGORY] ([CategoryId], [NameEn], [NameAr], [DescriptionEn], [DescriptionAr], [Color], [Label]) VALUES (4, N'Important', N'Important Arbi', N'Important Desc', N'Important Desc Arbi', N'Blue', N'IMPORTANT                                                                                           ')
	INSERT [dbo].[NOTIF_NOTIFICATION_CATEGORY] ([CategoryId], [NameEn], [NameAr], [DescriptionEn], [DescriptionAr], [Color], [Label]) VALUES (8, N'Do Not Reply', N'Do Not Reply Arbi', N'Do Not Reply Desc', N'Do Not Reply Desc Arbi', N'Grey', N'DO NOT REPLY                                                                                        ')
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[NOTIF_NOTIFICATION_COMMUNICATION_METHOD]') AND type in (N'U'))
	INSERT [dbo].[NOTIF_NOTIFICATION_COMMUNICATION_METHOD] ([CommunicationId], [NameEn], [NameAr], [DescriptionEn], [DescriptionAr]) VALUES (1, N'Email', N'Email Arbi', N'Send Via Email', N'Send Via Email Arbi')
	INSERT [dbo].[NOTIF_NOTIFICATION_COMMUNICATION_METHOD] ([CommunicationId], [NameEn], [NameAr], [DescriptionEn], [DescriptionAr]) VALUES (2, N'SMS', N'SMS Arbi', N'Send Via SMS', N'Send Via SMS Arbi')
	INSERT [dbo].[NOTIF_NOTIFICATION_COMMUNICATION_METHOD] ([CommunicationId], [NameEn], [NameAr], [DescriptionEn], [DescriptionAr]) VALUES (4, N'Browser', N'Browser Arbi', N'Send Via Browser', N'Send Via Browser Arbi')
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[NOTIF_NOTIFICATION_EVENT]') AND type in (N'U'))
	INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate]) VALUES (1, N'New Request', N'New Request Arbi', N'hassan1@gmail.com', CAST(N'2022-09-06T12:27:53.330' AS DateTime))
	INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate]) VALUES (2, N'Submit Request', N'Submit Request Arbi', N'aqeeltest123@gmail.com', CAST(N'2022-09-06T12:27:53.330' AS DateTime))
	INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate]) VALUES (4, N'Change of Status', N'Change of Status Arbi', N'aqeeltest123@gmail.com', CAST(N'2022-09-06T12:27:53.400' AS DateTime))
	INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate]) VALUES (8, N'Receiving a Response', N'Receiving a Response Arbi', N'zain123@gmail.com', CAST(N'2022-09-06T12:27:43.330' AS DateTime))
	INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate]) VALUES (16, N'Register a Case', N'Register a Case Arbi', N'shazim@gmail.com', CAST(N'2022-09-06T12:27:43.330' AS DateTime))
	INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate]) VALUES (32, N'Open a File', N'Open a File Arbi', N'hashim@gmail.com', CAST(N'2022-09-06T12:27:43.330' AS DateTime))
	INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate]) VALUES (64, N'Announcement', N'Announcement Arbi', N'anny@gmail.com', CAST(N'2022-09-06T12:27:43.330' AS DateTime))
	INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate]) VALUES (128, N'Delete Request', N'Delete Request Arbi', N'zain@gmail.com', CAST(N'2022-09-06T00:00:00.000' AS DateTime))
	INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate]) VALUES (256, N'Return Request', N'Return Request Arbi', N'zain@gmail.com', CAST(N'2022-09-02T00:00:00.000' AS DateTime))
GO
 
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[NOTIF_NOTIFICATION_RECEIVER_TYPE]') AND type in (N'U'))
	INSERT [dbo].[NOTIF_NOTIFICATION_RECEIVER_TYPE] ([TypeId], [Name]) VALUES (1, N'User')
	INSERT [dbo].[NOTIF_NOTIFICATION_RECEIVER_TYPE] ([TypeId], [Name]) VALUES (2, N'Role')
	INSERT [dbo].[NOTIF_NOTIFICATION_RECEIVER_TYPE] ([TypeId], [Name]) VALUES (4, N'Group')
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[NOTIF_NOTIFICATION_STATUS]') AND type in (N'U'))
	INSERT [dbo].[NOTIF_NOTIFICATION_STATUS] ([StatusId], [NameEn], [NameAr]) VALUES (1, N'Read', N'Arbi Read')
	INSERT [dbo].[NOTIF_NOTIFICATION_STATUS] ([StatusId], [NameEn], [NameAr]) VALUES (2, N'Unread', N'Arbi Unread')
	INSERT [dbo].[NOTIF_NOTIFICATION_STATUS] ([StatusId], [NameEn], [NameAr]) VALUES (4, N'Sent', N'Arbi Sent')
	INSERT [dbo].[NOTIF_NOTIFICATION_STATUS] ([StatusId], [NameEn], [NameAr]) VALUES (8, N'Received', N'Arbi Received')
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[NOTIF_NOTIFICATION_TEMPLATE]') AND type in (N'U'))
	INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [Body], [Footer], [IsActive], [URL], [LogoImagePath], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'3fa85f64-5717-4562-b3fc-2c963f66afa6', N'Save Literature', N'حفظ الأدب', N'Literature Saved Successfully', N'حفظ الأدب بنجاح', N'Email Body', N'Email Footer', 0, N'www.email.com', N'Email', N'zain@gmail.com', CAST(N'2022-09-27T06:01:21.730' AS DateTime), NULL, NULL, NULL, NULL, 0)
	INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [Body], [Footer], [IsActive], [URL], [LogoImagePath], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'3fa85f64-5717-4562-b3fc-2c963f66afa7', N'Borrowed Literature', N'الأدب المستعير', N'Literature Borrowed Successfully', N'استعار الأدب بنجاح', N'Sms Body', N'Sms Footer', 0, N'www.sms.com', N'Sms', N'hassan@gmail.com', CAST(N'2022-09-27T06:01:21.730' AS DateTime), NULL, NULL, NULL, NULL, 0)
	INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [Body], [Footer], [IsActive], [URL], [LogoImagePath], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'3fa85f64-5717-4562-b3fc-2c963f66bfa7', N'Borrow Request', N'طلب استعارة', N'Borrow Request for literature', N'طلب استعارة الأدب', N'Email Body', N'Email Footer', 0, N'www.email.com', N'Email', N'hassan@gmail.com', CAST(N'2022-09-27T06:01:21.730' AS DateTime), NULL, NULL, NULL, NULL, 0)
	INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [Body], [Footer], [IsActive], [URL], [LogoImagePath], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'3fa85f64-5717-4562-b3fc-2c969f66bfa7', N'Borrow Request Approved', N'تمت الموافقة على طلب الاقتراض', N'Literature Borrow Request Approved', N'الموافقة على طلب استعارة المؤلفات', N'Email Body', N'Email Footer', 0, N'www.email.com', N'Email', N'hassan@gmail.com', CAST(N'2022-09-27T07:02:22.223' AS DateTime), N'null', NULL, NULL, NULL, 0)
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[NOTIF_NOTIFICATION_TYPE]') AND type in (N'U'))
	INSERT [dbo].[NOTIF_NOTIFICATION_TYPE] ([TypeId], [Name]) VALUES (1, N'Synchronous')
	INSERT [dbo].[NOTIF_NOTIFICATION_TYPE] ([TypeId], [Name]) VALUES (2, N'Asynchronous')
GO
 
IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Permissions.Submenu.LDS.Legislation.List')
	INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Title_Ar],[Module],[SubModule],[ClaimType],[ClaimValue],[IsDeleted])
		VALUES ('List of Legal Legislation','List of Legal Legislation', 'LDS', 'Legislation', 'Permission', 'Permissions.Submenu.LDS.Legislation.List',0)
GO  
IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Permissions.Submenu.LDS.Legislation.List')
    INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Title_Ar],[Module],[SubModule],[ClaimType],[ClaimValue])
        VALUES ('List Of Legal Legislation','List Of Legal Legislation', 'LDS', 'Legislation', 'Permission', 'Permissions.Submenu.LDS.Legislation.List')
GO 

IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Permissions.LDS.Legislation.Decision')
    INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Title_Ar],[Module],[SubModule],[ClaimType],[ClaimValue],[IsDeleted])
        VALUES ('Decision Legal Legislation',N'Decision Legal Legislation', 'LDS', 'Legislation', 'Permission', 'Permissions.LDS.Legislation.Decision',0)
GO

IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Permissions.LDS.Legislation.Add')
    INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Title_Ar],[Module],[SubModule],[ClaimType],[ClaimValue],[IsDeleted])
        VALUES ('Add Legal Legislation',N'Add Legal Legislation', 'LDS', 'Legislation', 'Permission', 'Permissions.LDS.Legislation.Add',0)
GO

IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Permissions.LDS.Legislation.Edit')
    INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Title_Ar],[Module],[SubModule],[ClaimType],[ClaimValue],[IsDeleted])
        VALUES ('Edit Legal Legislation',N'Edit Legal Legislation', 'LDS', 'Legislation', 'Permission', 'Permissions.LDS.Legislation.Edit',0)
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_TEMPLATE_SETTING]') AND type in (N'U'))
INSERT [dbo].[LEGAL_TEMPLATE_SETTING] ([TemplateSettingId], [Template_Heading], [Template_Value]) VALUES (1, 'Main Title', 'Legislation Number')
INSERT [dbo].[LEGAL_TEMPLATE_SETTING] ([TemplateSettingId], [Template_Heading], [Template_Value]) VALUES (2, 'Main Title', 'Legislation Issue Date')
INSERT [dbo].[LEGAL_TEMPLATE_SETTING] ([TemplateSettingId], [Template_Heading], [Template_Value]) VALUES (4, 'Main Title', 'Legislation Start Date')
INSERT [dbo].[LEGAL_TEMPLATE_SETTING] ([TemplateSettingId], [Template_Heading], [Template_Value]) VALUES (8, 'Main Title', 'Legislation Subject')
INSERT [dbo].[LEGAL_TEMPLATE_SETTING] ([TemplateSettingId], [Template_Heading], [Template_Value]) VALUES (16, 'Introduction', 'Introduction with relation')
INSERT [dbo].[LEGAL_TEMPLATE_SETTING] ([TemplateSettingId], [Template_Heading], [Template_Value]) VALUES (32, 'Introduction', 'Introduction without relation')
INSERT [dbo].[LEGAL_TEMPLATE_SETTING] ([TemplateSettingId], [Template_Heading], [Template_Value]) VALUES (64, 'Publication', 'Publication details')
INSERT [dbo].[LEGAL_TEMPLATE_SETTING] ([TemplateSettingId], [Template_Heading], [Template_Value]) VALUES (128, 'Legislation Body', 'Articles with sections')
INSERT [dbo].[LEGAL_TEMPLATE_SETTING] ([TemplateSettingId], [Template_Heading], [Template_Value]) VALUES (256, 'Legislation Body', 'Articles without sections')
INSERT [dbo].[LEGAL_TEMPLATE_SETTING] ([TemplateSettingId], [Template_Heading], [Template_Value]) VALUES (512, 'Legislation Body', 'Clauses with sections')
INSERT [dbo].[LEGAL_TEMPLATE_SETTING] ([TemplateSettingId], [Template_Heading], [Template_Value]) VALUES (1024, 'Legislation Body', 'Clauses without sections')
INSERT [dbo].[LEGAL_TEMPLATE_SETTING] ([TemplateSettingId], [Template_Heading], [Template_Value]) VALUES (2048, 'Explanatory Note', 'Explanatory Note')
INSERT [dbo].[LEGAL_TEMPLATE_SETTING] ([TemplateSettingId], [Template_Heading], [Template_Value]) VALUES (4096, 'Note', 'Note')
GO
---------------------------------------------------------------------------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_PRINCIPLE_STATUS]') AND type in (N'U'))
INSERT [dbo].[LEGAL_PRINCIPLE_STATUS] ([Id], [Name_En], [Name_Ar]) VALUES (1, N'Active', N'نشيط')
INSERT [dbo].[LEGAL_PRINCIPLE_STATUS] ([Id], [Name_En], [Name_Ar]) VALUES (2, N'Modified', N'المعدل')
INSERT [dbo].[LEGAL_PRINCIPLE_STATUS] ([Id], [Name_En], [Name_Ar]) VALUES (4, N'Expired', N'منتهي الصلاحية')
GO
---------------------------------------------------------------------------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_PRINCIPLE_FLOW_STATUS]') AND type in (N'U'))

INSERT [dbo].[LEGAL_PRINCIPLE_FLOW_STATUS] ([Id], [Name_En], [Name_Ar]) VALUES (1, N'PartiallyCompleted', N'أنجزت جزئيا')
INSERT [dbo].[LEGAL_PRINCIPLE_FLOW_STATUS] ([Id], [Name_En], [Name_Ar]) VALUES (2, N'InReview', N'في مراجعة')
INSERT [dbo].[LEGAL_PRINCIPLE_FLOW_STATUS] ([Id], [Name_En], [Name_Ar]) VALUES (4, N'Approved', N'اعتماد')
INSERT [dbo].[LEGAL_PRINCIPLE_FLOW_STATUS] ([Id], [Name_En], [Name_Ar]) VALUES (8, N'Rejected', N'رفض')
INSERT [dbo].[LEGAL_PRINCIPLE_FLOW_STATUS] ([Id], [Name_En], [Name_Ar]) VALUES (16, N'Need To Modify', N'تحتاج إلى تعديل')
INSERT [dbo].[LEGAL_PRINCIPLE_FLOW_STATUS] ([Id], [Name_En], [Name_Ar]) VALUES (32, N'Send A Comment', N'أرسل تعليقًا')
INSERT [dbo].[LEGAL_PRINCIPLE_FLOW_STATUS] ([Id], [Name_En], [Name_Ar]) VALUES (64, N'Published', N'ينشر')
INSERT [dbo].[LEGAL_PRINCIPLE_FLOW_STATUS] ([Id], [Name_En], [Name_Ar]) VALUES (128, N'Unpublished', N'غير منشورة')

GO
-----------------------------------------------------------------------------------------------

-------------------------------------------------------------------------------
insert into UMS_CLAIM values('Approved Principles Submenu','LPS','Principles','Permission','Permissions.Submenu.LPS.Principles.PrincipleApproval','abc',0)
insert into UMS_ROLE_CLAIMS values('d6b3075c-3f70-4b44-baa4-1fdc599a6bb2','Permission','Permissions.Submenu.LPS.Principles.PrincipleApproval')
insert into UMS_CLAIM values('Delete Legal Principle','LPS','Principles','Permission','Permissions.Submenu.LPS.Principles.List.Delete','abc',0)
insert into UMS_ROLE_CLAIMS values('d6b3075c-3f70-4b44-baa4-1fdc599a6bb2','Permission','Permissions.Submenu.LPS.Principles.List.Delete')

------------------------------------------------------------------------------
-------------------------------------------------------------------------------
----------------------------------------------------------------------------------
INSERT INTO CMS_WITHDRAW_REQUEST_STATUS_FTW_LKP
VALUES (2, 'Withdrawn by GE', 'Withdrawn by GE');
INSERT INTO CMS_WITHDRAW_REQUEST_STATUS_FTW_LKP
VALUES (4, 'Rejected', 'Rejected');


SET IDENTITY_INSERT CMS_WITHDRAW_REQUEST_STATUS_G2G_LKP on
INSERT INTO CMS_WITHDRAW_REQUEST_STATUS_G2G_LKP
VALUES (2, 'Withdrawn by GE', 'Withdrawn by GE');
INSERT INTO CMS_WITHDRAW_REQUEST_STATUS_G2G_LKP
VALUES (4, 'Rejected', 'Rejected');