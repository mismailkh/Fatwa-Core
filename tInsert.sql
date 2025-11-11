
/*<History Author='Nabeel ur Rehman' Date='26-01-2023'> Example of Adding Data in Table </History>*/
--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Designation]') AND type in (N'U'))
--	INSERT [dbo].[Designation] ([Id], [Name_En], [Name_Ar]) VALUES (1, N'QA', N'سؤال وجواب عربي')
--	INSERT [dbo].[Designation] ([Id], [Name_En], [Name_Ar]) VALUES (2, N'Dev', N'ديف ديزاين عربي')
--GO


/*<History Author='Nabeel ur Rehman' Date='26-01-2023'> Example of Update Data in Table </History>*/
--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LPS_PRINCIPLE_STATUS]') AND type in (N'U'))
--	Update [LPS_PRINCIPLE_STATUS] Set [Name] = 'In Review' where StatusId = 2
--	Update [LPS_PRINCIPLE_STATUS] Set [Name] = 'Need To Modify' where StatusId = 16
--	Update [LPS_PRINCIPLE_STATUS] Set [Name] = 'Send A Comment' where StatusId = 32
--	Update [LPS_PRINCIPLE_STATUS] Set [Name] = 'Unpublished' where StatusId = 128
--	PRINT('Updated')
--GO  
/*<History Author='Nabeel ur Rehman' Date='26-01-2023'> Example of Update Data in Permission Table </History>*/
--IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Permissions.Submenu.LPS.Principles.Approval')
--	INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Title_Ar],[Module],[SubModule],[ClaimType],[ClaimValue])
--		VALUES ('Approve Reject Principles Submenu','Approve Reject Principles Submenu', 'LPS', 'Principles', 'Permission', 'Permissions.Submenu.LPS.Principles.Approval')
--GO 

IF((SELECT COUNT(*) FROM CMS_CASE_FILE_EVENT_G2G_LKP WHERE Id = '256') <= 0)
SET IDENTITY_INSERT CMS_CASE_FILE_EVENT_G2G_LKP ON
INSERT INTO CMS_CASE_FILE_EVENT_G2G_LKP (Id,Name_En,Name_Ar)VALUES(256,'AssignToLawyer','AssignToLawyer')
SET IDENTITY_INSERT CMS_CASE_FILE_EVENT_G2G_LKP OFF
GO

--Added By Noman Entry Added into CMS_CASE_REQUEST_EVENT_G2G_LKP FATWA DB Side
IF((SELECT COUNT(*) FROM CMS_CASE_REQUEST_EVENT_G2G_LKP WHERE Id = '12') <= 0)
SET IDENTITY_INSERT CMS_CASE_REQUEST_EVENT_G2G_LKP ON
INSERT [dbo].CMS_CASE_REQUEST_EVENT_G2G_LKP ([Id], Name_En, [Name_Ar]) VALUES (12, N'Withdraw', N'Withdraw')
IF((SELECT COUNT(*) FROM CMS_CASE_REQUEST_EVENT_G2G_LKP WHERE Id = '13') <= 0)
SET IDENTITY_INSERT CMS_CASE_REQUEST_EVENT_G2G_LKP ON
INSERT [dbo].CMS_CASE_REQUEST_EVENT_G2G_LKP ([Id], Name_En, [Name_Ar]) VALUES (13, N'Withdraw Rejected', N'Withdraw Rejected')
------
IF((SELECT COUNT(*) FROM MODULE_CONDITION WHERE Id = '149') <= 0)
INSERT [dbo].[MODULE_CONDITION] ([ModuleConditionId], [Name], [MKey], [ValueToCompare]) VALUES (149, N'If Coms Draft Status is RejectedBySupervisor', N'ComsDraftStatusRejectedBySupervisor', 256)
IF((SELECT COUNT(*) FROM MODULE_CONDITION WHERE Id = '150') <= 0)
INSERT [dbo].[MODULE_CONDITION] ([ModuleConditionId], [Name], [MKey], [ValueToCompare]) VALUES (150, N'If Coms Draft Status is RejectedByHOS', N'ComsDraftStatusRejectedByHOS', 512)


SET IDENTITY_INSERT [dbo].[LMS_LITERATURE_TAG] ON 
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (1, N'001', N'Control Number (NR)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (2, N'003', N'Control Number Identifier (NR)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (3, N'005', N'Date & Time of Latest Transaction (NR)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (4, N'006', N'Fixed Length Data Elements – AMC (R)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (5, N'007', N'Physical Description Fixed Field (R)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (6, N'008', N'Fixed Length Data Elements –GI (NR)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (7, N'013', N'Patent Control Number (R)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (8, N'015', N'National Bibliography Agency Control Number (R)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (9, N'020', N'ISBN (R)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (10, N'022', N'ISSN (R)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (11, N'027', N'Standard Technical Report Number(R)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (12, N'028', N'Publisher Number (R)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (14, N'030', N'CODEN Designation (R)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (15, N'033', N'Date/ Time Place of Event (R)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (16, N'034', N'Coded Cartographic Mathematical Data (R)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (18, N'036', N'Original Study Number for Computer Data files (NR)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (19, N'040', N'Cataloguing Source (NR)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (20, N'041', N'Language Code (NR)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (21, N'043', N'Geographic Area Code (NR)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (22, N'045', N'Time Period of Content (NR)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (23, N'080', N'UDC Number (R)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (24, N'082', N'DDC Number (R)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (25, N'084', N'Other Classification Number(R)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (26, N'086', N'Government Document Call Number (R)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (27, N'088', N'Report Number (R)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (28, N'100', N'Main Entry – Personal Name(NR)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (29, N'110', N'Main Entry - CorporateName(NR)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (30, N'111', N'Main Entry – Meeting Name (NR)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (31, N'130', N'Main Entry – Uniform Title(NR)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (32, N'222', N'Key Title (R)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (33, N'240', N'Uniform Title (NR)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (34, N'245', N'Title Statement (NR)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (35, N'246', N'Varying form of Title (R)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (36, N'247', N'Former Title or Title Variations (R)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (37, N'250', N'Edition Statement (NR)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (38, N'254', N'Musical Presentation Statement (NR)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (39, N'255', N'Cartographic Mathematical Data (R)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (40, N'256', N'Computer File Characteristics (NR)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (41, N'257', N'Country of Producing Entity for Archival Films (NR)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (42, N'260', N'Publication, Distribution, etc(Imprint) (NR)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (43, N'270', N'Address (R)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (44, N'300', N'Physical Description (R)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (45, N'306', N'Playing Time (NR)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (46, N'307', N'Hours, Etc. (R)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (47, N'310', N'Current Publication Frequency (NR)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (48, N'321', N'Former Publication Frequency(R)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (49, N'340', N'Physical Medium (R)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (50, N'351', N'Organization and Arrangement of Materials (R)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (51, N'362', N'Dates of Publication and/or Volume Designation (R)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (52, N'440', N'Series Statement/Added Entry – Title(R)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (53, N'490', N'Series Statement (R)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (54, N'500', N'General Note (R)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (55, N'501', N'With Note (R)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (56, N'502', N'Dissertation Note (R)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (57, N'504', N'Bibliography, Etc. Note (R)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (58, N'505', N'Formatted Contents Note (R)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (59, N'508', N'Creation/Production Credits Note (NR)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (60, N'513', N'Type of Report and Period Covered Note (R)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (61, N'515', N'Numbering Peculiarities Note (R)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (62, N'516', N'Type of Computer File or Data Note (R)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (63, N'520', N'Summary, Etc. (R)', 1)

GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (64, N'525', N'Supplement Note (R)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (65, N'530', N'Additional Physical Form Available Note (R)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (66, N'534', N'Original Version Note (R)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (67, N'536', N'Funding Information Note (R)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (69, N'538', N'System Details Note (R)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (70, N'546', N'Language Note (R)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (71, N'550', N'Issuing Body Note (R)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (72, N'586', N'Awards Note (R)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (73, N'590', N'Local Notes (R)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (74, N'600', N'Subject Added Entry – Personal Name (R)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (75, N'610', N'Subject Added Entry – Corporate Name (R)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (76, N'611', N'Subject Added Entry – Meeting Name (R)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (77, N'630', N'Subject Added Entry – Uniform Title (R)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (78, N'650', N'Subject Added Entry – Topical Term (R)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (79, N'651', N'Subject Added Entry – Geographical Name (R)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (80, N'653', N'Index Term – Uncontrolled (R)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (81, N'700', N'Added Entry – Personal Name (R)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (82, N'710', N'Added Entry – Corporate Name (R)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (83, N'711', N'Added Entry – Meeting Name (R)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (84, N'730', N'Added Entry – Uniform Title (R)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (86, N'740', N'Added Entry – Uncontrolled Related/Analytical title (R)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (87, N'777', N'Issued With Entry (R)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (88, N'780', N'Preceding Entry (R)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (89, N'785', N'Succeeding Entry (R)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (90, N'800', N'Series Added Entry – Personal Name (R)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (91, N'810', N'Series Added Entry – Corporate Name (R)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (92, N'811', N'Series Added Entry – Meeting Name (R)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (93, N'830', N'Series Added Entry - Uniform Title (R)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (94, N'850', N'Holding Institution (R)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (95, N'852', N'Location (R)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (96, N'856', N'Electronic Location and Access(R)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (97, N'886', N'Foreign MARC Information Field (R)', 1)
GO
INSERT [dbo].[LMS_LITERATURE_TAG] ([Id], [TagNo], [Description], [Active]) VALUES (98, N'016', N'National Bibliography Number (NR)', 1)
GO
SET IDENTITY_INSERT [dbo].[LMS_LITERATURE_TAG] OFF
GO

SET IDENTITY_INSERT [dbo].[CMS_COURT_G2G_LKP] ON
INSERT INTO CMS_COURT_G2G_LKP(Id,Number,Name_En,Name_Ar,District,Location,TypeId,IsActive,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,DeletedBy,DeletedDate,IsDeleted)
VALUES(5,'23WER2334','Appeal Court','Appeal Court','KWT','Kuwait',3,1,'fatwaadmin@gmail.com','2022-10-11 17:49:54.350',NULL,NULL,NULL,NULL,1)
GO

IF((SELECT COUNT(*) FROM CMS_COURT_G2G_LKP WHERE Id = 5) > 0)
DELETE FROM CMS_COURT_G2G_LKP WHERE Id = 5

IF((SELECT COUNT(*) FROM CMS_COURT_G2G_LKP WHERE Id = 5) <= 0)
SET IDENTITY_INSERT CMS_COURT_G2G_LKP ON
INSERT INTO CMS_COURT_G2G_LKP(Id,Number,Name_En,Name_Ar,District,Location,TypeId,IsActive,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,DeletedBy,DeletedDate,IsDeleted)
VALUES(5,'23WER2334','Appeal Court','Appeal Court','KWT','Kuwait',4,1,'fatwaadmin@gmail.com','2022-10-11 17:49:54.350',NULL,NULL,NULL,NULL,1)
SET IDENTITY_INSERT CMS_COURT_G2G_LKP OFF
GO


IF((SELECT COUNT(*) FROM CMS_SECTION WHERE NameEn = 'No Template Section') > 0)
UPDATE CMS_SECTION SET NameAr = N'لا يوجد قسم قالب' WHERE NameEn = 'No Template Section'
GO
IF((SELECT COUNT(*) FROM CMS_SECTION WHERE NameEn = 'Additional Section') > 0)
UPDATE CMS_SECTION SET NameAr = N'قسم إضافي' WHERE NameEn = 'Additional Section'
GO
IF((SELECT COUNT(*) FROM CMS_SECTION WHERE NameEn = 'Opening Statement') > 0)
UPDATE CMS_SECTION SET NameAr = N'البيان الافتتاحي' WHERE NameEn = 'Opening Statement'
GO
IF((SELECT COUNT(*) FROM CMS_SECTION WHERE NameEn = 'Body') > 0)
UPDATE CMS_SECTION SET NameAr = N'نص الوثيقة' WHERE NameEn = 'Body'
GO
IF((SELECT COUNT(*) FROM CMS_SECTION WHERE NameEn = 'Closing Statement') > 0)
UPDATE CMS_SECTION SET NameAr = N'كلمة الختام' WHERE NameEn = 'Closing Statement'
GO
--------------------------------------CMS_CASE_REQUEST_STATUS_G2G_LKP
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_CASE_REQUEST_STATUS_G2G_LKP]') AND type in (N'U'))
update CMS_CASE_REQUEST_STATUS_G2G_LKP set Name_Ar = N'يرسم'
where Name_Ar = N'Draft' 
update CMS_CASE_REQUEST_STATUS_G2G_LKP set Name_Ar = N'مُقَدَّم'
where Name_Ar = N'Submitted' 
update CMS_CASE_REQUEST_STATUS_G2G_LKP set Name_Ar = N'أعيد تقديمها'
where Name_Ar = N'Resubmitted' 
update CMS_CASE_REQUEST_STATUS_G2G_LKP set Name_Ar = N'تم تحويله إلى ملف'
where Name_Ar = N'Converted To File' 
update CMS_CASE_REQUEST_STATUS_G2G_LKP set Name_Ar = N'مرفوض'
where Name_Ar = N'Rejected' 
update CMS_CASE_REQUEST_STATUS_G2G_LKP set Name_Ar = N'طلب سحب'
where Name_Ar = N'Withdraw Requested' 
update CMS_CASE_REQUEST_STATUS_G2G_LKP set Name_Ar = N'تم السحب بواسطة GE'
where Name_Ar = N'Withdrawn By GE' 
update CMS_CASE_REQUEST_STATUS_G2G_LKP set Name_Ar = N'أرشيف'
where Name_Ar = N'Archive'
update CMS_CASE_REQUEST_STATUS_G2G_LKP set Name_Ar = N'مسجلة بوزارة العدل'
where Name_Ar = N'Registered In MOJ' 
update CMS_CASE_REQUEST_STATUS_G2G_LKP set Name_Ar = N'مخصص للقطاع الإقليمي'
where Name_Ar = N'Assigned To Regional Sector' 
update CMS_CASE_REQUEST_STATUS_G2G_LKP set Name_Ar = N'في انتظار استجابة GE'
where Name_Ar = N'Pending For GE Response' 
PRINT('Updated')
GO  
IF((SELECT COUNT(*) FROM LMS_BOOK_STATUS WHERE Id = 1) = 0)
INSERT [dbo].[LMS_BOOK_STATUS] ([StatusId], [Name], [Name_Ar]) VALUES (1, N'Borrowable', N'قابل للاستعارة')
GO
INSERT [dbo].[LMS_BOOK_STATUS] ([StatusId], [Name], [Name_Ar]) VALUES (2, N'Reserved', N'محجوز')
GO
INSERT [dbo].[LMS_BOOK_STATUS] ([StatusId], [Name], [Name_Ar]) VALUES (4, N'Borrowed', N'اقترضت، استعارت')
GO
INSERT [dbo].[LMS_BOOK_STATUS] ([StatusId], [Name], [Name_Ar]) VALUES (8, N'UnAvailable', N'غير متوفره')
GO

---------------------------------------------CMS_PRIORITY_G2G_LKP
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_PRIORITY_G2G_LKP]') AND type in (N'U'))
update CMS_PRIORITY_G2G_LKP set 
Name_Ar = N'قليل' where
Name_Ar = N'Low'
update CMS_PRIORITY_G2G_LKP set 
Name_Ar = N'واسطة' where
Name_Ar = N'Medium'
update CMS_PRIORITY_G2G_LKP set 
Name_Ar = N'عالي' where
Name_Ar = N'High'
PRINT('Updated')
GO 
--------------------------------------------CMS_COURT_TYPE_G2G_LKP
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_COURT_TYPE_G2G_LKP]') AND type in (N'U'))
update CMS_COURT_TYPE_G2G_LKP set 
Name_Ar = N'إقليمي' where
Name_Ar = N'Regional'
update CMS_COURT_TYPE_G2G_LKP set 
Name_Ar = N'أعلى فائق' where
Name_Ar = N'Supreme'
update CMS_COURT_TYPE_G2G_LKP set 
Name_Ar = N'جاذبية' where
Name_Ar = N'Appeal'
---------------------------------------CMS_CASE_PARTY_TYPE_G2G_LKP
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_CASE_PARTY_TYPE_G2G_LKP]') AND type in (N'U'))

update CMS_CASE_PARTY_TYPE_G2G_LKP set 
Name_Ar = N'فردي' where
Name_Ar = N'Individual'
update CMS_CASE_PARTY_TYPE_G2G_LKP set 
Name_Ar = N'شركة' where
Name_Ar = N'Company'
update CMS_CASE_PARTY_TYPE_G2G_LKP set 
Name_Ar = N'جهة حكومية' where
Name_Ar = N'Government Entity'
-----------------------------------------CMS_EXECUTION
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_EXECUTION]') AND type in (N'U'))

update CMS_EXECUTION set CaseId = 'F8B0310B-13A6-4DCD-9062-944D9E87378E'
where Id = '74E80CCF-BD97-4CD6-F4A5-08DB03774CE2'
update CMS_EXECUTION set CaseId = 'F8B0310B-13A6-4DCD-9062-944D9E87378E'
where Id = '35783DF8-757A-49CD-F4A6-08DB03774CE2'
update CMS_EXECUTION set CaseId = 'F8B0310B-13A6-4DCD-9062-944D9E87378E'
where Id = '72CB95DF-D4BF-46D4-9400-08DB03798A0B'
update CMS_EXECUTION set CaseId = 'F8B0310B-13A6-4DCD-9062-944D9E87378E'
where Id = 'C09A1AB6-C73A-49B0-BA2F-08DB03738CAD'
update CMS_EXECUTION set CaseId = '893712ed-a86b-468e-a319-143f7d6ca043'
where Id = 'FB726FB9-92F0-4346-F271-08DB03847D3C'
update CMS_EXECUTION set CaseId = '893712ed-a86b-468e-a319-143f7d6ca043'
where Id = '82E0C148-4F35-4530-5B65-08DB03867F4D'
update CMS_EXECUTION set CaseId = '893712ed-a86b-468e-a319-143f7d6ca043'
where Id = 'E5CE0B56-D14C-4E90-C4A6-08DB038709A2'
update CMS_EXECUTION set CaseId = '13efd6dc-54c8-405d-a222-3f2f2242e7dc'
where Id = 'E5CE0B56-D14C-4E90-C4A6-08DB038709A2'
update CMS_EXECUTION set CaseId = 'ce2dfd15-caef-4352-87f3-a130e792ca33'
where Id = '2FF13DD5-05A2-44AA-5927-08DB05F5DAA4'
GO


IF((SELECT COUNT(*) FROM ATTACHMENT_TYPE WHERE AttachmentTypeId = 18) > 0)
UPDATE ATTACHMENT_TYPE SET IsMandatory = 0 WHERE AttachmentTypeId = 18
GO
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_LEGISLATION_FLOW_STATUS]') AND type in (N'U'))
	INSERT [dbo].[LEGAL_LEGISLATION_FLOW_STATUS] ([Id], [Name_En], [Name_Ar]) VALUES (512, N'Need Modification', N'بحاجة الى تعديل')
GO
IF((SELECT COUNT(*) FROM ATTACHMENT_TYPE WHERE AttachmentTypeId = 18) > 0)
	UPDATE ATTACHMENT_TYPE SET IsMandatory = 0 WHERE AttachmentTypeId = 18
	GO

IF((SELECT COUNT(*) FROM COMM_COMMUNICATION_SOURCE WHERE Id = 1) = 0)
INSERT INTO COMM_COMMUNICATION_SOURCE (Id, NameEn, NameAr) VALUES (1,'FATWA',N'FATWA')
GO
IF((SELECT COUNT(*) FROM COMM_COMMUNICATION_SOURCE WHERE Id = 2) = 0)
INSERT INTO COMM_COMMUNICATION_SOURCE (Id, NameEn, NameAr) VALUES (2,'G2G',N'G2G')
GO
IF((SELECT COUNT(*) FROM COMM_COMMUNICATION_SOURCE WHERE Id = 4) = 0)
INSERT INTO COMM_COMMUNICATION_SOURCE (Id, NameEn, NameAr) VALUES (4,'Tarasul',N'Tarasul')
GO
IF((SELECT COUNT(*) FROM COMM_COMMUNICATION_SOURCE WHERE Id = 8) = 0)
INSERT INTO COMM_COMMUNICATION_SOURCE (Id, NameEn, NameAr) VALUES (8,'Legal Announcement',N'Legal Announcement')
GO

IF ((SELECT COUNT(*) FROM [CMS_OPERATING_SECTOR_TYPE_G2G_LKP] WHERE Id = 134217728) = 0)
BEGIN
	SET IDENTITY_INSERT [CMS_OPERATING_SECTOR_TYPE_G2G_LKP] ON
	INSERT [dbo].[CMS_OPERATING_SECTOR_TYPE_G2G_LKP]([Id], [Code], [Name_En], [Name_Ar], [IsActive])  VALUES (134217728,'EXE','Execution','Execution',1)
	SET IDENTITY_INSERT [CMS_OPERATING_SECTOR_TYPE_G2G_LKP] OFF
END
Go


Go
INSERT [dbo].[ATTACHMENT_TYPE] ([AttachmentTypeId], [Type_Ar], [Type_En], [ModuleId], [IsMandatory],[IsOfficialLetter]) VALUES (32, N'Main Authority Letter', N'Main Authority Letter', 10, 1,1)
Go
INSERT [dbo].[ATTACHMENT_TYPE] ([AttachmentTypeId], [Type_Ar], [Type_En], [ModuleId], [IsMandatory],[IsOfficialLetter]) VALUES (33, N'Main Defendent Civil ID', N'Main Defendent Civil ID', 10, 1,0)

----------------------[CMS_OPERATING_SECTOR_TYPE_G2G_LKP]
IF((SELECT COUNT(*) FROM CMS_OPERATING_SECTOR_TYPE_G2G_LKP WHERE Id between 512 and 33554432) > 0)
delete from CMS_OPERATING_SECTOR_TYPE_G2G_LKP where Id between 512 and 33554432

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_OPERATING_SECTOR_TYPE_G2G_LKP]') AND type in (N'U'))

IF ((SELECT COUNT(*) FROM [CMS_OPERATING_SECTOR_TYPE_G2G_LKP] WHERE Id = 512) = 0)
BEGIN
	SET IDENTITY_INSERT [CMS_OPERATING_SECTOR_TYPE_G2G_LKP] ON
	INSERT [dbo].[CMS_OPERATING_SECTOR_TYPE_G2G_LKP]([Id], [Code], [Name_En], [Name_Ar], [IsActive])  VALUES (512,'LAC','Legal Advice','Legal Advice',1)
	SET IDENTITY_INSERT [CMS_OPERATING_SECTOR_TYPE_G2G_LKP] OFF
END
Go
IF ((SELECT COUNT(*) FROM [CMS_OPERATING_SECTOR_TYPE_G2G_LKP] WHERE Id = 1024) = 0)
BEGIN
	SET IDENTITY_INSERT [CMS_OPERATING_SECTOR_TYPE_G2G_LKP] ON
	INSERT [dbo].[CMS_OPERATING_SECTOR_TYPE_G2G_LKP]([Id], [Code], [Name_En], [Name_Ar], [IsActive])  VALUES (1024,'LEG','Legislations','Legislations',1)
	SET IDENTITY_INSERT [CMS_OPERATING_SECTOR_TYPE_G2G_LKP] OFF
END
Go
IF ((SELECT COUNT(*) FROM [CMS_OPERATING_SECTOR_TYPE_G2G_LKP] WHERE Id = 2048) = 0)
BEGIN
	SET IDENTITY_INSERT [CMS_OPERATING_SECTOR_TYPE_G2G_LKP] ON
	INSERT [dbo].[CMS_OPERATING_SECTOR_TYPE_G2G_LKP]([Id], [Code], [Name_En], [Name_Ar], [IsActive])  VALUES (2048,'ADC','Administrative Complaints','Administrative Complaints',1)
	SET IDENTITY_INSERT [CMS_OPERATING_SECTOR_TYPE_G2G_LKP] OFF
END
Go
IF ((SELECT COUNT(*) FROM [CMS_OPERATING_SECTOR_TYPE_G2G_LKP] WHERE Id = 4096) = 0)
BEGIN
	SET IDENTITY_INSERT [CMS_OPERATING_SECTOR_TYPE_G2G_LKP] ON
	INSERT [dbo].[CMS_OPERATING_SECTOR_TYPE_G2G_LKP]([Id], [Code], [Name_En], [Name_Ar], [IsActive])  VALUES (4096,'CTT','Contracts ','Contracts ',1)
	SET IDENTITY_INSERT [CMS_OPERATING_SECTOR_TYPE_G2G_LKP] OFF
END
Go
IF ((SELECT COUNT(*) FROM [CMS_OPERATING_SECTOR_TYPE_G2G_LKP] WHERE Id = 8192) = 0)
BEGIN
	SET IDENTITY_INSERT [CMS_OPERATING_SECTOR_TYPE_G2G_LKP] ON
	INSERT [dbo].[CMS_OPERATING_SECTOR_TYPE_G2G_LKP]([Id], [Code], [Name_En], [Name_Ar], [IsActive])  VALUES (8192,'IAB','International Arbitration ','International Arbitration ',1)
	SET IDENTITY_INSERT [CMS_OPERATING_SECTOR_TYPE_G2G_LKP] OFF
END
Go
IF ((SELECT COUNT(*) FROM [CMS_OPERATING_SECTOR_TYPE_G2G_LKP] WHERE Id = 16384) = 0)
BEGIN
	SET IDENTITY_INSERT [CMS_OPERATING_SECTOR_TYPE_G2G_LKP] ON
	INSERT [dbo].[CMS_OPERATING_SECTOR_TYPE_G2G_LKP]([Id], [Code], [Name_En], [Name_Ar], [IsActive])  VALUES (16384,'POS','Private Operational Sector  ','Private Operational Sector  ',1)
	SET IDENTITY_INSERT [CMS_OPERATING_SECTOR_TYPE_G2G_LKP] OFF
END
Go
IF ((SELECT COUNT(*) FROM [CMS_OPERATING_SECTOR_TYPE_G2G_LKP] WHERE Id = 32768) = 0)
BEGIN
	SET IDENTITY_INSERT [CMS_OPERATING_SECTOR_TYPE_G2G_LKP] ON
	INSERT [dbo].[CMS_OPERATING_SECTOR_TYPE_G2G_LKP]([Id], [Code], [Name_En], [Name_Ar], [IsActive])  VALUES (32768,'PBS','Public Operational Sector  ','Public Operational Sector  ',1)
	SET IDENTITY_INSERT [CMS_OPERATING_SECTOR_TYPE_G2G_LKP] OFF
END
Go
--------------------CMS_WITHDRAW_REQUEST_STATUS_G2G_LKP
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_WITHDRAW_REQUEST_STATUS_G2G_LKP]') AND type in (N'U'))
Update CMS_WITHDRAW_REQUEST_STATUS_G2G_LKP set Name_En ='Withdraw  Request' where Name_En ='Withdraw Case Request'
Update CMS_WITHDRAW_REQUEST_STATUS_G2G_LKP set  Name_Ar ='Withdraw  Request' where Name_Ar ='Withdraw Case Request'



IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UMS_USER_TYPE]') AND type in (N'U'))
UPDATE UMS_USER_TYPE
SET Name_En = 'Internal Employee', Name_Ar= 'Internal Employee'
WHERE Id = 1;
 
------ CONSULTATION

UPDATE UMS_USER_TYPE
SET Name_En = 'External Employee', Name_Ar= 'External Employee'
WHERE Id = 2;

UPDATE UMS_USER_TYPE
SET Name_En = 'GE User', Name_Ar= 'GE User'
WHERE Id = 4;

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UMS_USER_TYPE]') AND type in (N'U'))
delete from UMS_USER_TYPE where Id= 8
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
----------------------------------------------------------------Consultation Workflow--------------------------------------------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

--------------------------MODULE_TRIGGER 
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MODULE_TRIGGER]') AND type in (N'U'))
SET IDENTITY_INSERT MODULE_TRIGGER ON
INSERT INTO MODULE_TRIGGER (ModuleTriggerId,Name,ModuleId) VALUES (5,'User Submits Consultation Draft',10)
SET IDENTITY_INSERT MODULE_TRIGGER OFF
--------------------------MODULE_ACTIVITY
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MODULE_ACTIVITY]') AND type in (N'U'))
INSERT INTO MODULE_ACTIVITY (Name,Class,Method,ModuleId,CategoryId,AKey) VALUES ('SUPCOMS-Review Draft Document','WorkflowImplementationService','Coms_ReviewDraftDocument','10','2','ComsReviewDraftDocument')
INSERT INTO MODULE_ACTIVITY (Name,Class,Method,ModuleId,CategoryId,AKey) VALUES ('HOSCOMS-Review Draft Document','WorkflowImplementationService','Coms_ReviewDraftDocumentHOS','10','2','ComsReviewDraftDocumentHOS')
--------------------------PARAMETER
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PARAMETER]') AND type in (N'U'))
INSERT INTO PARAMETER VALUES('User','ComsReviewDraftDocumentUser',0,0)
INSERT INTO PARAMETER VALUES('User Role','ComsReviewDraftDocumentUserRole',0,0)
INSERT INTO PARAMETER VALUES('User','ComsReviewDraftDocumentHosUser',0,0)
INSERT INTO PARAMETER VALUES('User Role','ComsReviewDraftDocumentHosUserRole',0,0)
--------------------------MODULE_ACTIVITY_PARAMETERS
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MODULE_ACTIVITY_PARAMETERS]') AND type in (N'U'))
INSERT INTO MODULE_ACTIVITY_PARAMETERS VALUES ('48','10')
INSERT INTO MODULE_ACTIVITY_PARAMETERS VALUES ('49','10')
INSERT INTO MODULE_ACTIVITY_PARAMETERS VALUES ('50','11')
INSERT INTO MODULE_ACTIVITY_PARAMETERS VALUES ('51','11')
-----------------------MODULE_CONDITION
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MODULE_CONDITION]') AND type in (N'U'))
INSERT INTO MODULE_CONDITION VALUES ('142','10','If Coms Draft Status is InReview','ComsDraftStatusInReview','1')
INSERT INTO MODULE_CONDITION VALUES ('143','10','If Coms Draft Status is NeedModification','ComsDraftStatusNeedModification','2')
INSERT INTO MODULE_CONDITION VALUES ('144','10','If Coms Draft Status is Reject','ComsDraftStatusReject','4')
INSERT INTO MODULE_CONDITION VALUES ('145','10','If Coms Draft Status is ApproveBySupervisor','ComsDraftStatusApproveBySupervisor','8')
INSERT INTO MODULE_CONDITION VALUES ('146','10','If Coms Draft Status is ApproveByHOS','ComsDraftStatusApproveByHOS','16')
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
----------------------------------------------------------------Consultation Workflow--------------------------------------------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[COMS_TEMPLATE_SECTION_HEAD]') AND type in (N'U'))
BEGIN
	INSERT INTO COMS_TEMPLATE_SECTION_HEAD VALUES (1, 'Title', N'عنوان')
	INSERT INTO COMS_TEMPLATE_SECTION_HEAD VALUES (2, 'Introduction', N'مقدمة')
	INSERT INTO COMS_TEMPLATE_SECTION_HEAD VALUES (4, 'Party 1', N'الطرف 1')
	INSERT INTO COMS_TEMPLATE_SECTION_HEAD VALUES (8, 'Party 2', N'الطرف 2')
	INSERT INTO COMS_TEMPLATE_SECTION_HEAD VALUES (16, 'Article', N'شرط')
	INSERT INTO COMS_TEMPLATE_SECTION_HEAD VALUES (32, 'Locked Article', N'مقفل شرط')
END
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[COMS_TEMPLATE_SECTION]') AND type in (N'U'))
BEGIN
	UPDATE COMS_TEMPLATE_SECTION SET Name_Ar = N'عنوان' WHERE TemplateSectionId = 1
	UPDATE COMS_TEMPLATE_SECTION SET Name_Ar = N'مقدمة' WHERE TemplateSectionId = 2
	UPDATE COMS_TEMPLATE_SECTION SET Name_Ar = N'الطرف 1' WHERE TemplateSectionId = 4
	UPDATE COMS_TEMPLATE_SECTION SET Name_Ar = N'الطرف 2' WHERE TemplateSectionId = 8
	UPDATE COMS_TEMPLATE_SECTION SET Name_Ar = N'شرط' WHERE TemplateSectionId = 16
END
GO
------------------------------------LINK_TARGET_TYPE
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LINK_TARGET_TYPE]') AND type in (N'U'))
BEGIN
INSERT INTO LINK_TARGET_TYPE VALUES ('64','ConsultationFile')
END
GO


IF((SELECT COUNT(*) FROM SYSTEM_SETTING) > 0)
BEGIN
UPDATE SYSTEM_SETTING SET Grid_Pagination = 10
END


IF((SELECT COUNT(*) FROM PARAMETER  where ParameterId = 33) > 0)
BEGIN
UPDATE PARAMETER SET IsAutoPopulated = 0 where ParameterId = 33
END
----------------------[CMS_CASE_FILE_STATUS_G2G_LKP]
SET IDENTITY_INSERT [dbo].[CMS_CASE_FILE_STATUS_G2G_LKP]ON 

INSERT [dbo].[CMS_CASE_FILE_STATUS_G2G_LKP] ([Id], [Name_En], [Name_Ar]) VALUES (2048, N'Assigned To Contract Sector', N'Assigned To Contract Sector')
INSERT [dbo].[CMS_CASE_FILE_STATUS_G2G_LKP] ([Id], [Name_En], [Name_Ar]) VALUES (4096, N'Assigned To LegalAdvice Sector', N'Assigned To LegalAdvice Sector Sector')
INSERT [dbo].[CMS_CASE_FILE_STATUS_G2G_LKP] ([Id], [Name_En], [Name_Ar]) VALUES (8192, N'Assigned To Legislation Sector', N'Assigned To Legislation Sector')
INSERT [dbo].[CMS_CASE_FILE_STATUS_G2G_LKP] ([Id], [Name_En], [Name_Ar]) VALUES (16384, N'Assigned To InternationalArbitration Sector', N'Assigned To InternationalArbitration Sector')
INSERT [dbo].[CMS_CASE_FILE_STATUS_G2G_LKP] ([Id], [Name_En], [Name_Ar]) VALUES (32768, N'Assigned To AdministrativeComplaints Sector', N'Assigned To AdministrativeComplaints Sector')
SET IDENTITY_INSERT [dbo].[CMS_CASE_FILE_STATUS_G2G_LKP] OFF 
----------------------------------[CMS_CASE_REQUEST_STATUS_G2G_LKP]
SET IDENTITY_INSERT [dbo].[CMS_CASE_REQUEST_STATUS_G2G_LKP] ON 

INSERT [dbo].[CMS_CASE_REQUEST_STATUS_G2G_LKP] ([Id], [Name_En], [Name_Ar]) VALUES (2048, N'Assigned To Contract Sector', N'Assigned To Contract Sector')
INSERT [dbo].[CMS_CASE_REQUEST_STATUS_G2G_LKP] ([Id], [Name_En], [Name_Ar]) VALUES (4096, N'Assigned To LegalAdvice Sector', N'Assigned To LegalAdvice Sector Sector')
INSERT [dbo].[CMS_CASE_REQUEST_STATUS_G2G_LKP] ([Id], [Name_En], [Name_Ar]) VALUES (8192, N'Assigned To Legislation Sector', N'Assigned To Legislation Sector')
INSERT [dbo].[CMS_CASE_REQUEST_STATUS_G2G_LKP] ([Id], [Name_En], [Name_Ar]) VALUES (16384, N'Assigned To InternationalArbitration Sector', N'Assigned To InternationalArbitration Sector')
INSERT [dbo].[CMS_CASE_REQUEST_STATUS_G2G_LKP] ([Id], [Name_En], [Name_Ar]) VALUES (32768, N'Assigned To AdministrativeComplaints Sector', N'Assigned To AdministrativeComplaints Sector')
SET IDENTITY_INSERT [dbo].[CMS_CASE_REQUEST_STATUS_G2G_LKP] OFF 
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[COMS_CONSULTATION_INTERNATIONAL_ARBITRATION_TYPE_G2G_LKP]') AND type in (N'U'))
BEGIN
	INSERT INTO COMS_CONSULTATION_INTERNATIONAL_ARBITRATION_TYPE_G2G_LKP VALUES (1, 'International Arbitration', N'International Arbitration')
	INSERT INTO COMS_CONSULTATION_INTERNATIONAL_ARBITRATION_TYPE_G2G_LKP VALUES (2, 'Local Arbitration', N'Local Arbitration')
	INSERT INTO COMS_CONSULTATION_INTERNATIONAL_ARBITRATION_TYPE_G2G_LKP VALUES (4, 'International Disputes', N'International Disputes')
END
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[COMS_CONSULTATION_Legislation_FILE_TYPE_FTW_LKP]') AND type in (N'U'))
BEGIN
	UPDATE COMS_CONSULTATION_Legislation_FILE_TYPE_FTW_LKP SET Name_En = 'Issuance of new laws', Name_Ar = 'Issuance of new laws' WHERE Id = 1
	UPDATE COMS_CONSULTATION_Legislation_FILE_TYPE_FTW_LKP SET Name_En = 'Proposed Laws', Name_Ar = 'Proposed Laws' WHERE Id = 2
	UPDATE COMS_CONSULTATION_Legislation_FILE_TYPE_FTW_LKP SET Name_En = 'Agreements and Regulation', Name_Ar = 'Agreements and Regulation' WHERE Id = 4
END
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_SUBTYPE_G2G_LKP]') AND type in (N'U'))
BEGIN
	INSERT INTO CMS_SUBTYPE_G2G_LKP VALUES (4, 'LA', 'Housing Allowance', N'Housing Allowance',1)
	INSERT INTO CMS_SUBTYPE_G2G_LKP VALUES (4, 'LA', 'Employees', N'Employees',1)
	INSERT INTO CMS_SUBTYPE_G2G_LKP VALUES (4, 'LA', 'Ministries', N'Ministries',1)
	INSERT INTO CMS_SUBTYPE_G2G_LKP VALUES (4, 'LA', 'Judgements', N'Judgements',1)
	INSERT INTO CMS_SUBTYPE_G2G_LKP VALUES (4, 'LA', 'Authorities', N'Authorities',1)
	INSERT INTO CMS_SUBTYPE_G2G_LKP VALUES (4, 'LA', 'Investigation', N'Investigation',1)
	INSERT INTO CMS_SUBTYPE_G2G_LKP VALUES (4, 'LA', 'Housing Welfare', N'Housing Welfare',1)
	INSERT INTO CMS_SUBTYPE_G2G_LKP VALUES (4, 'LA', 'Sub-Classification', N'Sub-Classification',1)
END
GO

--------------  Consultation (06-03-2023) -----------------

GO
INSERT [dbo].[COMS_TEMPLATE_SECTION] ([TemplateSectionId], [Name], [Content_En], [Content_Ar], [Name_Ar], [SectionHeadId]) VALUES (1, N'Title', N'null', N'null', N'عنوان', 1)
GO
INSERT [dbo].[COMS_TEMPLATE_SECTION] ([TemplateSectionId], [Name], [Content_En], [Content_Ar], [Name_Ar], [SectionHeadId]) VALUES (2, N'Introduction', N'It is on the day: ........, corresponding to: ........., of the month: ……, in the year: ………… AD , It was completed
conclusion of the aforementioned contract.
between', N'أنه في يوم : ........ ، الموافق : ......... ، من شهر : ....... ، عام : ........... م ، تم
إبرام العقد المشار إليه .
بين', N'مقدمة', 2)
GO
INSERT [dbo].[COMS_TEMPLATE_SECTION] ([TemplateSectionId], [Name], [Content_En], [Content_Ar], [Name_Ar], [SectionHeadId]) VALUES (4, N'Party1', N'
1- #key1# in the State of Kuwait, represented by Mr. #key2# In his capacity as: 
#key3# And his address: #key4# 
It is called (the first party) 
And between', N'١- #key1# بدولة الكويت ويمثلها السيد #key2#
بصفته : #key3#
وعنوانه : #key4#
ويسمى (الطرف الأول)
وبين', N'الطرف 1', 4)
GO
INSERT [dbo].[COMS_TEMPLATE_SECTION] ([TemplateSectionId], [Name], [Content_En], [Content_Ar], [Name_Ar], [SectionHeadId]) VALUES (8, N'Party2', N'
2- Mr. / Messrs. #key1# represented by Mr.  #key2#
As: #key3#
And its address: District: #key4#, Plot: #key5#, Street: #key6#
Building/Voucher: #key7# Office: #key8# Postal Address: Kuwait #key9#
P.O.Box: #key10# Postal Code: #key11# Phone Number: #key12#
Fax number: #key13# E-mail #key14#
He/she is called (the second party)', N'٢- السيد/ السادة #key1# ويمثله السيد/ #key2#
بصفته : #key3#
وعنوانه : منطقة : #key4# قطعة : #key5# شارع : #key6#
المبنى/ القسيمة : #key7# المكتب : #key8# العنوان البريدي: الكويت #key9#
ص.ب : #key10# الرمز البريدي: #key11# رقم الهاتف : #key12#
رقم الفاكس : #key13# البريد الالكتروني #key14#
ويسمى/ ويسمون (الطرف الثاني)', N'الطرف 2', 8)
GO
INSERT [dbo].[COMS_TEMPLATE_SECTION] ([TemplateSectionId], [Name], [Content_En], [Content_Ar], [Name_Ar], [SectionHeadId]) VALUES (16, N'Article', N'null', N'null', N'شرط', 16)
GO
INSERT [dbo].[COMS_TEMPLATE_SECTION] ([TemplateSectionId], [Name], [Content_En], [Content_Ar], [Name_Ar], [SectionHeadId]) VALUES (32, N'Contract documents', N'The previous preamble, conditions document, technical specifications, and all attached appendices are considered an integral part of the contract and offer submitted by the second party, and all correspondences exchanged between the two parties. From this and complementary to it. The contract and completed', N'يعتـبر التمهيـد السـابق ومسـتند الشـروط والمواصـفات الفنيـة وكافـة الملاحـق المرفقـة العقـد والعـرض ا لا يتجـزأً المقـدم مـن الطـرف الثـاني وكافـة المكاتبـات المتبادلـة بـين الطـرفين جـزءمن هذا ا ومكمًلا له.ً العقد ومتمم', N'مستندات العقد', 32)
GO
INSERT [dbo].[COMS_TEMPLATE_SECTION] ([TemplateSectionId], [Name], [Content_En], [Content_Ar], [Name_Ar], [SectionHeadId]) VALUES (64, N'scope of works', N'The second party is obligated to supply (devices / machines / equipment) according to the terms subject of the contract and in accordance with the technical specifications stipulated in the contract documents referred to above.', N'يلتـزم الطـرف الثـاني بتوريـد (الأجهـزة / الآلات / المعـدات) للشـروط محـل العقـد طبقـا والمواصفات الفنية المنصوص عليها في مســتندات العقـد المشار إليها أعــلاه. ', N'نطاق الأعمال ', 32)
GO

---------------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[COMS_CONSULTATION_INTERNATIONAL_ARBITRATION_TYPE_G2G_LKP]') AND type in (N'U'))
BEGIN
	INSERT INTO COMS_CONSULTATION_INTERNATIONAL_ARBITRATION_TYPE_G2G_LKP VALUES (1, 'International Arbitration', N'International Arbitration')
	INSERT INTO COMS_CONSULTATION_INTERNATIONAL_ARBITRATION_TYPE_G2G_LKP VALUES (2, 'Local Arbitration', N'Local Arbitration')
	INSERT INTO COMS_CONSULTATION_INTERNATIONAL_ARBITRATION_TYPE_G2G_LKP VALUES (4, 'International Disputes', N'International Disputes')
END
GO

IF ((SELECT COUNT(*) FROM [COMM_COMMUNICATION_TYPE] WHERE CommunicationTypeId = 32) = 0)
BEGIN
	INSERT [dbo].[COMM_COMMUNICATION_TYPE]([CommunicationTypeId], [NameAr], [NameEn])  VALUES (32,N'ConsultationRequest','ConsultationRequest')
END
Go

IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Permissions.Dashboard.COMS.View')
	INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted])
		VALUES ('COMS','Fatwa Dashboard', 'COMS', 'Permission', 'Permissions.Dashboard.COMS.View','COMS',0)
GO 


IF((SELECT COUNT(*) FROM UMS_ROLE_CLAIMS  WHERE RoleId = '1dbe8947-fa41-4f1c-a150-fe272e27b06c' AND ClaimValue = 'Permissions.Dashboard.CMS.View') > 0)
BEGIN
UPDATE UMS_ROLE_CLAIMS SET ClaimValue = 'Permissions.Dashboard.COMS.View' WHERE RoleId = '1dbe8947-fa41-4f1c-a150-fe272e27b06c' AND ClaimValue = 'Permissions.Dashboard.CMS.View'
END
ELSE
BEGIN
INSERT INTO UMS_ROLE_CLAIMS VALUES ('1dbe8947-fa41-4f1c-a150-fe272e27b06c', 'Permission', 'Permissions.Dashboard.COMS.View')
END

IF((SELECT COUNT(*) FROM UMS_ROLE_CLAIMS  WHERE RoleId = 'ec11b80f-2429-44d0-a5e1-1e144752e579' AND ClaimValue = 'Permissions.Dashboard.CMS.View') > 0)
BEGIN
Update UMS_ROLE_CLAIMS SET ClaimValue = 'Permissions.Dashboard.COMS.View' WHERE RoleId = 'ec11b80f-2429-44d0-a5e1-1e144752e579' AND ClaimValue = 'Permissions.Dashboard.CMS.View'
END
ELSE
BEGIN
INSERT INTO UMS_ROLE_CLAIMS VALUES ('ec11b80f-2429-44d0-a5e1-1e144752e579', 'Permission', 'Permissions.Dashboard.COMS.View')
END

IF((SELECT COUNT(*) FROM UMS_ROLE_CLAIMS  WHERE RoleId = '35bae56b-6523-477a-a8ca-bbf6fa2d4647' AND ClaimValue = 'Permissions.Dashboard.CMS.View') > 0)
BEGIN
UPDATE UMS_ROLE_CLAIMS SET ClaimValue = 'Permissions.Dashboard.COMS.View' WHERE RoleId = '35bae56b-6523-477a-a8ca-bbf6fa2d4647' AND ClaimValue = 'Permissions.Dashboard.CMS.View'
END
ELSE
BEGIN
INSERT INTO UMS_ROLE_CLAIMS VALUES ('35bae56b-6523-477a-a8ca-bbf6fa2d4647', 'Permission', 'Permissions.Dashboard.COMS.View')
END

IF((SELECT COUNT(*) FROM UMS_ROLE_CLAIMS  WHERE RoleId = 'e1e17355-216f-463b-918e-e4d898e01457' AND ClaimValue = 'Permissions.Dashboard.CMS.View') > 0)
BEGIN
UPDATE UMS_ROLE_CLAIMS SET ClaimValue = 'Permissions.Dashboard.COMS.View' WHERE RoleId = 'e1e17355-216f-463b-918e-e4d898e01457' AND ClaimValue = 'Permissions.Dashboard.CMS.View'
END
ELSE
BEGIN
INSERT INTO UMS_ROLE_CLAIMS values ('e1e17355-216f-463b-918e-e4d898e01457', 'Permission', 'Permissions.Dashboard.COMS.View')
END
IF((SELECT COUNT(*) FROM TSK_TASK_TYPE  WHERE TypeId = 8) = 0)
BEGIN
INSERT INTO TSK_TASK_TYPE values (8, 'Transfer', N'Transfer')
END

---------------------

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[COMS_TEMPLATE_SECTION]') AND type in (N'U'))
BEGIN
	UPDATE COMS_TEMPLATE_SECTION SET SectionName_En = 'Contract Section', SectionName_Ar = N'Contract Section' WHERE TemplateSectionId = 32
	UPDATE COMS_TEMPLATE_SECTION SET SectionName_En = 'Scope Section', SectionName_Ar = N'Scope Section' WHERE TemplateSectionId = 64
END
GO

GO
SET IDENTITY_INSERT [dbo].[CMS_CASE_REQUEST_EVENT_FTW_LKP] ON 
GO
INSERT [dbo].[CMS_CASE_REQUEST_EVENT_FTW_LKP] ([Id], [Name_En], [Name_Ar]) VALUES (1, N'Created', N'Created')
GO
INSERT [dbo].[CMS_CASE_REQUEST_EVENT_FTW_LKP] ([Id], [Name_En], [Name_Ar]) VALUES (2, N'Edited', N'Edited')
GO
INSERT [dbo].[CMS_CASE_REQUEST_EVENT_FTW_LKP] ([Id], [Name_En], [Name_Ar]) VALUES (3, N'Withdrawn', N'Withdrawn')
GO
INSERT [dbo].[CMS_CASE_REQUEST_EVENT_FTW_LKP] ([Id], [Name_En], [Name_Ar]) VALUES (4, N'Transfer', N'Transfer')
GO
INSERT [dbo].[CMS_CASE_REQUEST_EVENT_FTW_LKP] ([Id], [Name_En], [Name_Ar]) VALUES (5, N'AssignToLawyer', N'AssignToLawyer')
GO
INSERT [dbo].[CMS_CASE_REQUEST_EVENT_FTW_LKP] ([Id], [Name_En], [Name_Ar]) VALUES (6, N'SentCopy', N'SentCopy')
GO
INSERT [dbo].[CMS_CASE_REQUEST_EVENT_FTW_LKP] ([Id], [Name_En], [Name_Ar]) VALUES (7, N'ReceivedCopy', N'ReceivedCopy')
GO
INSERT [dbo].[CMS_CASE_REQUEST_EVENT_FTW_LKP] ([Id], [Name_En], [Name_Ar]) VALUES (8, N'Linked', N'Linked')
GO
INSERT [dbo].[CMS_CASE_REQUEST_EVENT_FTW_LKP] ([Id], [Name_En], [Name_Ar]) VALUES (9, N'RegisteredInMOJ', N'RegisteredInMOJ')
GO
INSERT [dbo].[CMS_CASE_REQUEST_EVENT_FTW_LKP] ([Id], [Name_En], [Name_Ar]) VALUES (10, N'Need More Info', N'Need More Info')
GO
INSERT [dbo].[CMS_CASE_REQUEST_EVENT_FTW_LKP] ([Id], [Name_En], [Name_Ar]) VALUES (11, N'Send Communication', N'Send Communication')
Go
SET IDENTITY_INSERT [dbo].[CMS_CASE_REQUEST_EVENT_FTW_LKP] OFF
GO
GO


 IF((SELECT COUNT(*) FROM UMS_CLAIM WHERE ClaimValue = 'Permissions.CMS.DraftDocument.CreateButton') = 0)
 BEGIN
 INSERT INTO UMS_CLAIM VALUES ('Create Button Draft Document','CMS','Draft Document', 'Permission', 'Permissions.CMS.DraftDocument.CreateButton','Create Button Draft Document',0)
 END

 
 IF((SELECT COUNT(*) FROM UMS_ROLE_CLAIMS WHERE ClaimValue = 'Permissions.CMS.DraftDocument.CreateButton' AND RoleId ='8b6cfa36-914a-4430-9feb-627e11715113') = 0)
 BEGIN
 INSERT INTO UMS_ROLE_CLAIMS VALUES ('8b6cfa36-914a-4430-9feb-627e11715113', 'Permission', 'Permissions.CMS.DraftDocument.CreateButton')
 END

IF((SELECT COUNT(*) FROM CMS_TEMPLATE WHERE Id = 12 AND NameEn ='Contract Review Request' AND AttachmentTypeId = 27) > 0)
BEGIN
UPDATE CMS_TEMPLATE SET Content = '<!DOCTYPE html>
<html>
<head>
<meta charset="utf-8">
<meta name="viewport" content="width=device-width, initial-scale=1.0">
<title></title>
</head>
<body style="margin: 132px 108px 0px 87px; color: rgb(0, 0, 0); background-color: rgb(255, 255, 255);">
<p style="margin:0cm;font-size:16px;text-align:center;background:#F3F3F3;border:none;padding:0cm;">
<strong><span style="font-size:28px;color:black;">#ComsTempTitle#</span></strong></p>
<p style="margin:0cm;font-size:16px;margin-top:15.0pt;text-align:left;">#ComsTempIntroduction#</p>
<p style="margin:0cm;font-size:16px;margin-top:15.0pt;text-align:left;">#ComsTempParty#</p>
<p style="margin:0cm;font-size:16px;margin-top:15.0pt;text-align:left;">#ComsTempArticle#</p>
</body>
</html>' WHERE Id = 12 AND NameEn ='Contract Review Request' AND AttachmentTypeId = 27
 END



   IF((SELECT COUNT(*) FROM CMS_SECTION WHERE Id = 6) = 0)
 BEGIN
 INSERT INTO CMS_SECTION VALUES (6, 'Contract Title', N'عنوان العقد')
 END
   IF((SELECT COUNT(*) FROM CMS_SECTION WHERE Id = 7) = 0)
 BEGIN
 INSERT INTO CMS_SECTION VALUES (7, 'Contract Introduction', N'مقدمة العقد')
 END
    IF((SELECT COUNT(*) FROM CMS_SECTION WHERE Id = 8) = 0)
 BEGIN
 INSERT INTO CMS_SECTION VALUES (8, 'Contract Party', N'طرف العقد')
 END
 IF((SELECT COUNT(*) FROM CMS_SECTION WHERE Id = 9) = 0)
 BEGIN
 INSERT INTO CMS_SECTION VALUES (9, 'Contract Article', N'مادة العقد')
 END

  IF((SELECT COUNT(*) FROM CMS_TEMPLATE_SECTION WHERE TemplateId = 12) > 0)
 BEGIN
 DELETE FROM CMS_TEMPLATE_SECTION where TemplateId = 12
 END

 IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_TEMPLATE_SECTION]') AND type in (N'U'))
BEGIN
INSERT INTO CMS_TEMPLATE_SECTION VALUES (NewID(), 12, 6)
INSERT INTO CMS_TEMPLATE_SECTION VALUES (NewID(), 12, 7)
INSERT INTO CMS_TEMPLATE_SECTION VALUES (NewID(), 12, 8)
INSERT INTO CMS_TEMPLATE_SECTION VALUES (NewID(), 12, 9)
END

 IF((SELECT COUNT(*) FROM PARAMETER WHERE ParameterId = 52 AND PKey ='ComsTempTitle') = 0)
 BEGIN
 INSERT INTO PARAMETER VALUES ('Contract Title', 'ComsTempTitle', 0, 1)
 END

 IF((SELECT COUNT(*) FROM PARAMETER WHERE ParameterId = 53 AND PKey ='ComsTempIntroduction') = 0)
 BEGIN
 INSERT INTO PARAMETER VALUES ('Contract Introduction', 'ComsTempIntroduction', 0, 1)
 END

  IF((SELECT COUNT(*) FROM PARAMETER WHERE ParameterId = 54 AND PKey ='ComsTempParty') = 0)
 BEGIN
 INSERT INTO PARAMETER VALUES ('Contract Party', 'ComsTempParty', 0, 1)
 END

  IF((SELECT COUNT(*) FROM PARAMETER WHERE ParameterId = 55 AND PKey ='ComsTempArticle') = 0)
 BEGIN
 INSERT INTO PARAMETER VALUES ('Contract Article', 'ComsTempArticle', 0, 1)
 END

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_TEMPLATE_SECTION_PARAMETER]') AND type in (N'U'))
BEGIN
INSERT INTO CMS_TEMPLATE_SECTION_PARAMETER VALUES ('9CC5D917-D921-438F-96AB-32918F2FE41B', 52)
INSERT INTO CMS_TEMPLATE_SECTION_PARAMETER VALUES ('D44F04F4-F347-4B66-B63F-7CDF5793B58C', 53)
INSERT INTO CMS_TEMPLATE_SECTION_PARAMETER VALUES ('9022F375-6C6C-4F57-BF91-28BDFB1DC81A', 54)
INSERT INTO CMS_TEMPLATE_SECTION_PARAMETER VALUES ('A14E092F-1A27-4861-83F1-F753BE47FD0A', 55)
END

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UMS_CLAIM]') AND type in (N'U'))
SET IDENTITY_INSERT [dbo].[UMS_CLAIM] ON 
GO
INSERT [dbo].[UMS_CLAIM] ([Id], [Title_En], [Module], [SubModule], [ClaimType], [ClaimValue], [Title_Ar], [IsDeleted]) VALUES (4306, 'Assign Consultation File to Lawyer', 'COMS', 'Consultation File', 'Permission', 'Permissions.COMS.ConsultationFile.AssignToLawyer',N'Assign Consultation File to Lawyer', 0)
INSERT [dbo].[UMS_CLAIM] ([Id], [Title_En], [Module], [SubModule], [ClaimType], [ClaimValue], [Title_Ar], [IsDeleted]) VALUES (4307, 'Trasnfer Consultation File ', 'COMS', 'Consultation File', 'Permission', 'Permissions.COMS.ConsultationFile.Transfer',N'Trasnfer Consultation File', 0)
INSERT [dbo].[UMS_CLAIM] ([Id], [Title_En], [Module], [SubModule], [ClaimType], [ClaimValue], [Title_Ar], [IsDeleted]) VALUES (4308, 'Create Button Consultation Draft Document', 'COMS', 'Draft Document', 'Permission', 'Permissions.COMS.DraftDocument.CreateButton',N'Create Button Consultation Draft Document', 0)
INSERT [dbo].[UMS_CLAIM] ([Id], [Title_En], [Module], [SubModule], [ClaimType], [ClaimValue], [Title_Ar], [IsDeleted]) VALUES (4309, 'Request More Info for Consultation File ', 'COMS', 'Consultation File', 'Permission', 'Permissions.COMS.ConsultationFile.RequestMoreInfo',N'Request More Info for Consultation File', 0)

GO
SET IDENTITY_INSERT [dbo].[UMS_CLAIM] OFf 
GO
--------------------------UMS_ROLE_CLAIMS
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UMS_ROLE_CLAIMS]') AND type in (N'U'))

INSERT [dbo].[UMS_ROLE_CLAIMS] ([RoleId], [ClaimType], [ClaimValue]) VALUES ('1dbe8947-fa41-4f1c-a150-fe272e27b06c', N'Permission', N'Permissions.COMS.ConsultationFile.AssignToLawyer')
INSERT [dbo].[UMS_ROLE_CLAIMS] ([RoleId], [ClaimType], [ClaimValue]) VALUES ('1dbe8947-fa41-4f1c-a150-fe272e27b06c', N'Permission', N'Permissions.COMS.ConsultationFile.Transfer')
INSERT [dbo].[UMS_ROLE_CLAIMS] ([RoleId], [ClaimType], [ClaimValue]) VALUES ('35bae56b-6523-477a-a8ca-bbf6fa2d4647', N'Permission', N'Permissions.COMS.DraftDocument.CreateButton')
INSERT [dbo].[UMS_ROLE_CLAIMS] ([RoleId], [ClaimType], [ClaimValue]) VALUES ('35bae56b-6523-477a-a8ca-bbf6fa2d4647', N'Permission', N'Permissions.COMS.ConsultationFile.RequestMoreInfo')
GO




UPDATE CMS_TEMPLATE SET Content = N'<!DOCTYPE html>
<html>

<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title></title>
</head>

<body style="margin: 132px 108px 0px 87px; color: rgb(0, 0, 0); background-color: rgb(255, 255, 255);">
    <p dir="RTL" style="margin:0cm;text-align:right;font-size:16px;font-family:''Times New Roman'',serif;"><span style="font-size:21px;font-family:"Calibri",sans-serif;color:black;">&nbsp;</span></p>
    <p dir="RTL" style="margin:0cm;text-align:right;font-size:16px;font-family:''Times New Roman'',serif;"><span style="font-size:19px;font-family:"Calibri",sans-serif;color:black;">الكويت في :&nbsp;#CmsTempOutboxDate#</span></p>
    <p dir="RTL" style="margin:0cm;text-align:right;font-size:16px;font-family:''Times New Roman'',serif;"><span style="font-size:19px;font-family:"Calibri",sans-serif;color:black;">مرجع رقــم :&nbsp;#CmsTempOutboxNumber#</span></p>
    <p dir="RTL" style="margin:0cm;text-align:right;font-size:16px;font-family:''Times New Roman'',serif;"><span style="font-size:21px;font-family:"Calibri",sans-serif;color:black;">&nbsp;</span></p>
    <p dir="RTL" style="margin:0cm;text-align:right;font-size:16px;font-family:''Times New Roman'',serif;"><span style="font-size:21px;font-family:"Calibri",sans-serif;color:black;">&nbsp;</span></p>
    <p dir="RTL" style="margin:0cm;text-align:center;font-size:16px;font-family:''Times New Roman'',serif;background:#BFBFBF;"><strong><span style="font-size:27px;font-family:"Calibri",sans-serif;color:black;">إخطار بالحكم</span></strong></p>
    <p dir="RTL" style="margin:0cm;text-align:right;font-size:16px;font-family:''Times New Roman'',serif;"><span dir="LTR" style="font-size:21px;font-family:"Calibri",sans-serif;color:black;">&nbsp;</span></p>
    <p dir="RTL" style="margin:0cm;text-align:right;font-size:16px;font-family:''Times New Roman'',serif;margin-top:12.0pt;"><strong><span style="font-size:21px;font-family:"Calibri",sans-serif;color:black;">حضرة الفاضل :#CmsTempName#</span></strong><strong><span dir="LTR" style="font-size:21px;font-family:"Calibri",sans-serif;color:black;">&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;</span></strong><strong><span style="font-size:21px;font-family:"Calibri",sans-serif;color:black;">المحترم</span></strong><span style="font-size:21px;font-family:"Calibri",sans-serif;color:black;">&nbsp;</span></p>
    <p dir="RTL" style="margin:0cm;text-align:right;font-size:16px;font-family:''Times New Roman'',serif;margin-top:12.0pt;"><span style="font-size:21px;font-family:"Calibri",sans-serif;color:black;">تحية طيبة وبعد،،،</span></p>
    <p dir="RTL" style="margin:0cm;text-align:right;font-size:16px;font-family:''Times New Roman'',serif;margin-top:12.0pt;"><span dir="LTR" style="color:black;">&nbsp;</span></p>
    <p dir="RTL" style="margin:0cm;text-align:right;font-size:16px;font-family:''Times New Roman'',serif;margin-top:12.0pt;"><span style="font-size:21px;font-family:"Calibri",sans-serif;color:black;">بشأن الدعوى رقم #CmsTempCaseNumber# &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; والمقامة:#CmsTempCourtName#</span></p>
    <p dir="RTL" style="margin:0cm;text-align:right;font-size:16px;font-family:''Times New Roman'',serif;margin-top:12.0pt;"><span style="font-size:21px;font-family:"Calibri",sans-serif;color:black;">مــــــــــن :&nbsp;#CmsTempPlaintiffName#</span></p>
    <p dir="RTL" style="margin:0cm;text-align:right;font-size:16px;font-family:''Times New Roman'',serif;margin-top:12.0pt;"><span style="font-size:21px;font-family:"Calibri",sans-serif;color:black;background:white;">ضــــد</span><span dir="LTR" style="font-size:21px;font-family:"Calibri",sans-serif;color:black;background:white;">&nbsp;</span><span style="font-size:21px;font-family:"Calibri",sans-serif;color:black;">:&nbsp;#CmsTempDefendantName#</span></p>
    <p dir="RTL" style="margin:0cm;text-align:right;font-size:16px;font-family:''Times New Roman'',serif;margin-top:12.0pt;"><span style="font-size:21px;font-family:"Calibri",sans-serif;color:black;">والمنظورة أمام الدائرة الإدارية / #CmsTempChamberName#&nbsp;</span></p>
    <p dir="RTL" style="margin:0cm;text-align:right;font-size:16px;font-family:''Times New Roman'',serif;margin-top:12.0pt;text-indent:14.8pt;"><span style="font-size:21px;font-family:"Calibri",sans-serif;color:black;">نفيدكم بأن الدعوى قدم صدر الحكم فيها بجلسة #CmsTempHearingDate# &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; بالحكم المرفق صورته.</span></p>
    <p dir="RTL" style="margin:0cm;text-align:center;font-size:16px;font-family:''Times New Roman'',serif;margin-top:12.0pt;"><span style="font-size:21px;font-family:"Calibri",sans-serif;color:black;">مع أطيب التمنيات ،،،</span></p>
    <p dir="RTL" style="margin:0cm;text-align:center;font-size:16px;font-family:''Times New Roman'',serif;"><span style="color:black;">&nbsp;</span></p>
    <div align="left" dir="ltr" style="margin:0cm;text-align:right;font-size:16px;font-family:''Times New Roman'',serif;">
        <table cellspacing="3" style="border: none;width:338.55pt;">
            <tbody>
                <tr>
                    <td style="padding:.75pt .75pt .75pt .75pt;"><br></td>
                </tr>
            </tbody>
        </table>
    </div>
    <p dir="LTR" style="margin:0cm;text-align:left;font-size:16px;font-family:''Times New Roman'',serif;"><span dir="RTL" style="font-size:21px;font-family:"Calibri",sans-serif;color:black;">رئيس قطاع القضاء الاداري الكلي</span><span style="font-size:21px;font-family:"Calibri",sans-serif;color:black;">&nbsp;&nbsp;</span></p>
    <p dir="RTL" style="margin:0cm;text-align:right;font-size:16px;font-family:''Times New Roman'',serif;"><span style="font-size:21px;font-family:"Calibri",sans-serif;color:black;">العضو المختص :&nbsp;#CmsTempLawywerName#</span></p>
    <p dir="RTL" style="margin:0cm;text-align:right;font-size:16px;font-family:''Times New Roman'',serif;"><span style="font-size:21px;font-family:"Calibri",sans-serif;color:black;">المرفقات : &nbsp;#CmsTempAttachments#</span></p>
    <p dir="RTL" style="margin:0cm;text-align:right;font-size:16px;font-family:''Times New Roman'',serif;">&nbsp;</p>
    <p dir="RTL" style="margin:0cm;text-align:right;font-size:16px;font-family:''Times New Roman'',serif;">&nbsp;</p>
</body>

</html>' WHERE ID = '4'


IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Permissions.Menu.InboxOutbox')
	INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted])
		VALUES ('Inbox Outbox List','Communication', 'InboxOutbox', 'Permission', 'Permissions.Menu.InboxOutbox','Inbox Outbox List',0)
GO 

IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Permissions.CMS.DocumentPortfolio.Create')
	INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted])
		VALUES ('Create Document Portfolio','CMS', 'Document Portfolio', 'Permission', 'Permissions.CMS.DocumentPortfolio.Create','Create Document Portfolio',0)
GO 

IF NOT Exists(SELECT 1 FROM UMS_ROLE_CLAIMS  WHERE RoleId = '8b6cfa36-914a-4430-9feb-627e11715113' AND ClaimValue = 'Permissions.Menu.InboxOutbox')
BEGIN
INSERT INTO UMS_ROLE_CLAIMS VALUES ('8b6cfa36-914a-4430-9feb-627e11715113', 'Permission', 'Permissions.Menu.InboxOutbox')
END

IF NOT Exists(SELECT 1 FROM UMS_ROLE_CLAIMS  WHERE RoleId = '93e5374b-cbd9-494e-92d4-d9d7d44c2c39' AND ClaimValue = 'Permissions.Menu.InboxOutbox')
BEGIN
INSERT INTO UMS_ROLE_CLAIMS VALUES ('93e5374b-cbd9-494e-92d4-d9d7d44c2c39', 'Permission', 'Permissions.Menu.InboxOutbox')
END

IF NOT Exists(SELECT 1 FROM UMS_ROLE_CLAIMS  WHERE RoleId = 'f2c87c20-3a38-4a20-b238-ec643ebd0df9' AND ClaimValue = 'Permissions.Menu.InboxOutbox')
BEGIN
INSERT INTO UMS_ROLE_CLAIMS VALUES ('f2c87c20-3a38-4a20-b238-ec643ebd0df9', 'Permission', 'Permissions.Menu.InboxOutbox')
END

IF NOT Exists(SELECT 1 FROM UMS_ROLE_CLAIMS  WHERE RoleId = '1dbe8947-fa41-4f1c-a150-fe272e27b06c' AND ClaimValue = 'Permissions.Menu.InboxOutbox')
BEGIN
INSERT INTO UMS_ROLE_CLAIMS VALUES ('1dbe8947-fa41-4f1c-a150-fe272e27b06c', 'Permission', 'Permissions.Menu.InboxOutbox')
END

IF NOT Exists(SELECT 1 FROM UMS_ROLE_CLAIMS  WHERE RoleId = '35bae56b-6523-477a-a8ca-bbf6fa2d4647' AND ClaimValue = 'Permissions.Menu.InboxOutbox')
BEGIN
INSERT INTO UMS_ROLE_CLAIMS VALUES ('35bae56b-6523-477a-a8ca-bbf6fa2d4647', 'Permission', 'Permissions.Menu.InboxOutbox')
END

IF NOT Exists(SELECT 1 FROM UMS_ROLE_CLAIMS  WHERE RoleId = 'ec11b80f-2429-44d0-a5e1-1e144752e579' AND ClaimValue = 'Permissions.Menu.InboxOutbox')
BEGIN
INSERT INTO UMS_ROLE_CLAIMS VALUES ('ec11b80f-2429-44d0-a5e1-1e144752e579', 'Permission', 'Permissions.Menu.InboxOutbox')
END


IF NOT Exists(SELECT 1 FROM UMS_ROLE_CLAIMS  WHERE RoleId = '8b6cfa36-914a-4430-9feb-627e11715113' AND ClaimValue = 'Permissions.CMS.DocumentPortfolio.Create')
BEGIN
INSERT INTO UMS_ROLE_CLAIMS VALUES ('8b6cfa36-914a-4430-9feb-627e11715113', 'Permission', 'Permissions.CMS.DocumentPortfolio.Create')
END

insert into ATTACHMENT_TYPE values(35,'WithdrawRequest','WithdrawRequest',5,1,1)
insert into COMM_COMMUNICATION_TYPE values(128,'Withdraw Requested','Withdraw Requested')
insert into LINK_TARGET_TYPE values(128,'Withdraw Request')

-----------------------------------------------------
SET IDENTITY_INSERT CMS_CASE_FILE_STATUS_G2G_LKP on

 INSERT INTO CMS_CASE_FILE_STATUS_G2G_LKP (id,Name_En,Name_Ar)
 VALUES (65536,'Rejected','Rejected');
   SET IDENTITY_INSERT CMS_CASE_FILE_STATUS_G2G_LKP off
-----------------------------------------------------
UPDATE ATTACHMENT_TYPE SET IsMandatory = 0 WHERE ModuleId = 5 AND AttachmentTypeId != 9


IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Permissions.CMS.DocumentPortfolio.MojList')
	INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted])
		VALUES ('Moj List Document Portfolio','CMS', 'Document Portfolio', 'Permission', 'Permissions.CMS.DocumentPortfolio.MojList','Moj List Document Portfolio',0)
GO 

IF NOT Exists(SELECT 1 FROM UMS_ROLE_CLAIMS  WHERE RoleId = '4eae855f-500f-4912-90fc-fe399fcb6fea' AND ClaimValue = 'Permissions.CMS.DocumentPortfolio.MojList')
BEGIN
INSERT INTO UMS_ROLE_CLAIMS VALUES ('4eae855f-500f-4912-90fc-fe399fcb6fea', 'Permission', 'Permissions.CMS.DocumentPortfolio.MojList')
END


IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Permissions.CMS.DocumentPortfolio.RequestDetail')
	INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted])
		VALUES ('Detail Of Document Portfolio Request','CMS', 'Document Portfolio', 'Permission', 'Permissions.CMS.DocumentPortfolio.RequestDetail','Detail Of Document Portfolio Request',0)
GO 

IF NOT Exists(SELECT 1 FROM UMS_ROLE_CLAIMS  WHERE RoleId = '4eae855f-500f-4912-90fc-fe399fcb6fea' AND ClaimValue = 'Permissions.CMS.DocumentPortfolio.RequestDetail')
BEGIN
INSERT INTO UMS_ROLE_CLAIMS VALUES ('4eae855f-500f-4912-90fc-fe399fcb6fea', 'Permission', 'Permissions.CMS.DocumentPortfolio.RequestDetail')
END

UPDATE CMS_TEMPLATE SET Content = N'<!DOCTYPE html>  <html>    <head>      <meta charset="utf-8">      <meta name="viewport" content="width=device-width, initial-scale=1.0">      <title></title>  </head>    <body style="margin: 132px 108px 0px 87px; color: rgb(0, 0, 0); background-color: rgb(255, 255, 255);">  <p dir="RTL" style="margin:0cm;font-size:16px;font-family:''Times New Roman'',serif;text-align:justify;"><strong><span dir="LTR" style="font-size:13px;font-family:''Calibri'',sans-serif;">&nbsp;</span></strong></p>  <p dir="RTL" style="margin:0cm;font-size:16px;font-family:''Times New Roman'',serif;text-align:justify;"><strong><span dir="LTR" style="font-size:13px;font-family:''Calibri'',sans-serif;">&nbsp;</span></strong></p>  <p dir="RTL" style="margin:0cm;font-size:16px;font-family:''Times New Roman'',serif;text-align:justify;"><strong><span dir="LTR" style="font-size:13px;font-family:''Calibri'',sans-serif;">&nbsp;</span></strong></p>  <p dir="RTL" style="margin:0cm;font-size:16px;font-family:''Times New Roman'',serif;text-align:justify;"><strong><span style="font-size:19px;font-family:''Calibri'',sans-serif;">الكويت في :&nbsp;#CmsTempOutboxDate#</span></strong></p>  <p dir="RTL" style="margin:0cm;font-size:16px;font-family:''Times New Roman'',serif;text-align:right;"><strong><span style="font-size:19px;font-family:''Calibri'',sans-serif;">مرجع رقم :&nbsp;#CmsTempOutboxNumber#</span></strong><strong><span style="font-size:19px;font-family:''Calibri'',sans-serif;">&nbsp;</span></strong></p>  <h2 dir="RTL" style="margin:0cm;text-align:center;font-size:19px;font-family:''Simplified Arabic'',serif;"><span style="font-family:''Calibri'',sans-serif;">&nbsp;</span></h2>  <div style="margin:0cm;font-size:16px;font-family:''Times New Roman'',serif;border:solid windowtext 1.0pt;padding:1.0pt 4.0pt 1.0pt 4.0pt;background:#F3F3F3;">      <p dir="RTL" style="margin:0cm;font-size:16px;font-family:''Times New Roman'',serif;text-align:center;background:#F3F3F3;border:none;padding:0cm;"><strong><span style="font-size:28px;font-family:''Calibri'',sans-serif;color:black;">إخطار بإقامة دعوى&nbsp;</span></strong></p>  </div>  <p dir="RTL" style="margin:0cm;font-size:16px;font-family:''Times New Roman'',serif;text-align:center;"><strong><span style="font-size:11px;font-family:''Calibri'',sans-serif;">&nbsp;</span></strong></p>  <p dir="RTL" style="margin:0cm;font-size:16px;font-family:''Times New Roman'',serif;margin-bottom:12.0pt;text-align:justify;"><strong><span style="font-size:24px;font-family:''Calibri'',sans-serif;">&nbsp;</span></strong></p>  <p dir="RTL" style="margin:0cm;font-size:16px;font-family:''Times New Roman'',serif;margin-bottom:12.0pt;text-align:justify;"><strong><span style="font-size:24px;font-family:''Calibri'',sans-serif;">حضرة</span></strong><strong><span style="font-size:24px;font-family:''Calibri'',sans-serif;">&nbsp;</span></strong><strong><span style="font-size:24px;font-family:''Calibri'',sans-serif;">الفاضل/ #CmsTempName#</span></strong><strong><span style="font-size:24px;font-family:''Calibri'',sans-serif;">&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;</span></strong><strong><span style="font-size:24px;font-family:''Calibri'',sans-serif;">المحتـرم</span></strong></p>  <p dir="RTL" style="margin:0cm;font-size:16px;font-family:''Times New Roman'',serif;margin-bottom:12.0pt;text-align:justify;"><strong><span style="font-size:21px;font-family:''Calibri'',sans-serif;">تحية طيبة وبعد ،،،</span></strong></p>  <p dir="RTL" style="margin:0cm;font-size:16px;font-family:''Times New Roman'',serif;text-align:justify;"><strong><span style="font-size:21px;font-family:''Calibri'',sans-serif;">&nbsp;</span></strong></p>  <p dir="RTL" style="margin:0cm;font-size:16px;font-family:''Times New Roman'',serif;text-align:justify;"><strong><span style="font-size:21px;font-family:''Calibri'',sans-serif;">مُرفق طيه صورة من صحيفة الدعوى رقم&nbsp;#CmsTempCaseNumber#</span></strong><strong><span style="font-size:21px;font-family:''Calibri'',sans-serif;">&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;المقامة</span></strong><strong><span style="font-size:21px;font-family:''Calibri'',sans-serif;">&nbsp;:&nbsp;#CmsTempCourtName#</span></strong></p>  <p dir="RTL" style="margin:0cm;font-size:16px;font-family:''Times New Roman'',serif;text-align:justify;text-indent:8.15pt;"><strong><span style="font-size:21px;font-family:''Calibri'',sans-serif;">&nbsp;</span></strong></p>  <p dir="RTL" style="margin:0cm;font-size:16px;font-family:''Times New Roman'',serif;margin-top:12.0pt;text-align:justify;text-indent:8.15pt;"><strong><span style="font-size:21px;font-family:''Calibri'',sans-serif;">مــــن :&nbsp;#CmsTempPlaintiffName#</span></strong></p>  <p dir="RTL" style="margin:0cm;font-size:16px;font-family:''Times New Roman'',serif;margin-top:12.0pt;text-align:justify;text-indent:8.15pt;"><strong><span style="font-size:21px;font-family:''Calibri'',sans-serif;">ضــــد:&nbsp;#CmsTempDefendantName#</span></strong></p>  <p dir="RTL" style="margin:0cm;font-size:16px;font-family:''Times New Roman'',serif;margin-top:12.0pt;text-align:justify;text-indent:8.15pt;"><strong><span style="font-size:21px;font-family:''Calibri'',sans-serif;">الدائرة الإدارية /&nbsp;#CmsTempChamberName#</span></strong><strong><span style="font-size:21px;font-family:''Calibri'',sans-serif;">&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;والمحدد لنظرها جلسة:&nbsp;#CmsTempHearingDate#</span></strong></p>  <p dir="RTL" style="margin:0cm;font-size:16px;font-family:''Times New Roman'',serif;text-align:justify;text-indent:8.15pt;"><strong><span style="font-size:21px;font-family:''Calibri'',sans-serif;">&nbsp;</span></strong></p>  <p dir="RTL" style="margin:0cm;font-size:16px;font-family:''Times New Roman'',serif;text-align:right;text-indent:14.8pt;"><strong><span style="font-size:21px;font-family:''Calibri'',sans-serif;">يرجى العلم والإحاطة واتخاذ ما يلزم بشأن تمثيل &nbsp;#CmsTempGovernmentEntityName# &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;في هذه الدعوى وإبداء ما يلزم من دفاع فيها بمعرفة الإدارة القانونية لديكم.</span></strong></p>  <p dir="RTL" style="margin:0cm;font-size:16px;font-family:''Times New Roman'',serif;text-align:center;"><strong><span style="font-size:21px;font-family:''Calibri'',sans-serif;">&nbsp;</span></strong></p>  <p dir="RTL" style="margin:0cm;font-size:16px;font-family:''Times New Roman'',serif;text-align:center;"><strong><span style="font-size:21px;font-family:''Calibri'',sans-serif;">مـــع أطيـــب التمنيـــات ،،،</span></strong></p>  <p dir="RTL" style="margin:0cm;font-size:16px;font-family:''Times New Roman'',serif;text-align:center;"><strong><span style="font-family:''Calibri'',sans-serif;">&nbsp;</span></strong></p>  <p dir="RTL" style="margin:0cm;font-size:16px;font-family:''Times New Roman'',serif;text-align:center;"><strong><span style="font-family:''Calibri'',sans-serif;">&nbsp;</span></strong></p>  <p dir="RTL" style="margin: 0cm;font-size:16px;font-family: ''Times New Roman'', serif;text-align: right;"><strong><span style="font-size:27px;font-family:''Calibri'',sans-serif;">&nbsp; &nbsp;رئيس قطاع قضايا الإداري الكلي</span></strong><strong><span style="font-size:27px;font-family:''Calibri'',sans-serif;">&nbsp;</span></strong></p>  <p dir="RTL" style="margin:0cm;font-size:16px;font-family:''Times New Roman'',serif;text-align:justify;"><strong><span style="font-size:18px;font-family:''Calibri'',sans-serif;">&nbsp;</span></strong></p>  <p dir="RTL" style="margin:0cm;font-size:16px;font-family:''Times New Roman'',serif;text-align:justify;"><strong><span style="font-size:18px;font-family:''Calibri'',sans-serif;">&nbsp;</span></strong></p>  <p dir="RTL" style="margin:0cm;font-size:16px;font-family:''Times New Roman'',serif;text-align:justify;"><strong><span style="font-size:18px;font-family:''Calibri'',sans-serif;">&nbsp;</span></strong></p>  <p dir="RTL" style="margin:0cm;font-size:16px;font-family:''Times New Roman'',serif;text-align:justify;"><strong><span style="font-size:11px;font-family:''Calibri'',sans-serif;">&nbsp;</span></strong></p>  <p dir="RTL" style="margin:0cm;font-size:16px;font-family:''Times New Roman'',serif;text-align:justify;"><strong><span style="font-family:''Calibri'',sans-serif;">&nbsp;</span></strong></p>  <p dir="RTL" style="margin:0cm;font-size:16px;font-family:''Times New Roman'',serif;text-align:justify;"><strong><span style="font-family:''Calibri'',sans-serif;">&nbsp;</span></strong></p>  <p dir="RTL" style="margin:0cm;font-size:16px;font-family:''Times New Roman'',serif;text-align:justify;"><strong><span style="font-family:''Calibri'',sans-serif;">&nbsp;</span></strong></p>  <p dir="RTL" style="margin:0cm;font-size:16px;font-family:''Times New Roman'',serif;text-align:justify;"><strong><span style="font-family:''Calibri'',sans-serif;">&nbsp;</span></strong></p>  <p dir="RTL" style="margin:0cm;font-size:16px;font-family:''Times New Roman'',serif;text-align:justify;"><strong><span style="font-family:''Calibri'',sans-serif;">العضو المختص :&nbsp;#CmsTempLawywerName#</span></strong><strong><span style="font-family:''Calibri'',sans-serif;">&nbsp;</span></strong></p>  <p dir="RTL" style="margin:0cm;font-size:16px;font-family:''Times New Roman'',serif;text-align:justify;"><strong><span style="font-family:''Calibri'',sans-serif;">المرفقات &nbsp;: #CmsTempAttachments#</span></strong></p>  <p dir="RTL" style="margin:0cm;font-size:16px;font-family:''Times New Roman'',serif;text-align:justify;"><strong><span style="font-family:''Calibri'',sans-serif;">&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;</span></strong></p></body></html>' WHERE ID = '3'
----------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_CASE_DECISION_TYPE_FTW_LKP]') AND type in (N'U'))
BEGIN
INSERT INTO CMS_CASE_DECISION_TYPE_G2G_LKP VALUES (1,'Interpretation Of Judgment','Interpretation Of Judgement')
INSERT INTO CMS_CASE_DECISION_TYPE_G2G_LKP VALUES (2,'Invalidity Of Judgment Execution','Invalidity Of Judgement Execution')
INSERT INTO CMS_CASE_DECISION_TYPE_G2G_LKP VALUES (4,'Stop Judgment Execution','Stop Judgement Execution')	
INSERT INTO CMS_CASE_DECISION_TYPE_G2G_LKP VALUES (8,'Regeneration Of Judgment Execution','Regeneration Of Judgment Execution')	
END

UPDATE CMS_COURT_TYPE_G2G_LKP SET IsDeleted = 0

IF NOT EXISTS(SELECT 1 FROM CMS_COURT_TYPE_G2G_LKP WHERE Id = 8)
BEGIN
SET IDENTITY_INSERT CMS_COURT_TYPE_G2G_LKP ON
INSERT INTO CMS_COURT_TYPE_G2G_LKP (Id,Name_En,Name_Ar,IsActive,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,DeletedBy,DeletedDate,IsDeleted)
VALUES(8,'Partial & Uregent','Partial & Urgent',1,'fatwaadmin@gmail.com','2022-10-11 17:49:54.350',NULL,NULL,NULL,NULL,0)
SET IDENTITY_INSERT CMS_COURT_TYPE_G2G_LKP OFF
END


UPDATE CMS_SUBTYPE_G2G_LKP SET Name_En = 'Lawsuit', Name_Ar = 'Lawsuit' WHERE Id = 5
UPDATE CMS_SUBTYPE_G2G_LKP SET Name_En = 'Complain Against Decision From Pr', Name_Ar = 'Complain Against Decision From Pr' WHERE Id = 6
UPDATE CMS_SUBTYPE_G2G_LKP SET Name_En = 'Perform Order Request', Name_Ar = 'Perform Order Request' WHERE Id = 7
UPDATE CMS_SUBTYPE_G2G_LKP SET Name_En = 'Order On Petition Request', Name_Ar = 'Order On Petition Request' WHERE Id = 8
UPDATE CMS_SUBTYPE_G2G_LKP SET RequestTypeId = 0 WHERE Id = 9


--DMS DB

ALTER TABLE ATTACHMENT_TYPE ADD SubTypeId INT DEFAULT 0

INSERT INTO ATTACHMENT_TYPE (AttachmentTypeId,Type_Ar,Type_En,ModuleId,IsMandatory,IsOfficialLetter,SubTypeId) VALUES(35,'Complaint','Complaint',5,1,0,6)
INSERT INTO ATTACHMENT_TYPE (AttachmentTypeId,Type_Ar,Type_En,ModuleId,IsMandatory,IsOfficialLetter,SubTypeId) VALUES(36,'Decision','Decision',5,1,0,6)
INSERT INTO ATTACHMENT_TYPE (AttachmentTypeId,Type_Ar,Type_En,ModuleId,IsMandatory,IsOfficialLetter,SubTypeId) VALUES(37,'Anouncement of Porcecution For GE','Anouncement of Porcecution For GE',5,1,0,6)
INSERT INTO ATTACHMENT_TYPE (AttachmentTypeId,Type_Ar,Type_En,ModuleId,IsMandatory,IsOfficialLetter,SubTypeId) VALUES(38,'Anouncement of Porcecution For Council of Ministers','Anouncement of Porcecution For Council of Ministers',5,1,0,6)

INSERT INTO ATTACHMENT_TYPE (AttachmentTypeId,Type_Ar,Type_En,ModuleId,IsMandatory,IsOfficialLetter,SubTypeId) VALUES(35,'Complaint','Complaint',5,1,0,6)
INSERT INTO ATTACHMENT_TYPE (AttachmentTypeId,Type_Ar,Type_En,ModuleId,IsMandatory,IsOfficialLetter,SubTypeId) VALUES(36,'Decision','Decision',5,1,0,6)
INSERT INTO ATTACHMENT_TYPE (AttachmentTypeId,Type_Ar,Type_En,ModuleId,IsMandatory,IsOfficialLetter,SubTypeId) VALUES(37,'Anouncement of Porcecution For GE','Anouncement of Porcecution For GE',5,1,0,6)
INSERT INTO ATTACHMENT_TYPE (AttachmentTypeId,Type_Ar,Type_En,ModuleId,IsMandatory,IsOfficialLetter,SubTypeId) VALUES(38,'Anouncement of Porcecution For Council of Ministers','Anouncement of Porcecution For Council of Ministers',5,1,0,6)
INSERT INTO ATTACHMENT_TYPE (AttachmentTypeId,Type_Ar,Type_En,ModuleId,IsMandatory,IsOfficialLetter,SubTypeId) VALUES(39,'Anouncement of Porcecution For Council of Ministers','Anouncement of Porcecution For Council of Ministers',5,1,0,7)
INSERT INTO ATTACHMENT_TYPE (AttachmentTypeId,Type_Ar,Type_En,ModuleId,IsMandatory,IsOfficialLetter,SubTypeId) VALUES(40,'Anouncement of Porcecution For Council of Ministers','Anouncement of Porcecution For Council of Ministers',5,1,0,8)


IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Permissions.CMS.MOJ.ExecutionList')
	INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted])
		VALUES ('List Of Execution Requests','CMS', 'MOJ', 'Permission', 'Permissions.CMS.MOJ.ExecutionList','List Of Execution Requests',0)
GO 


IF NOT Exists(SELECT 1 from UMS_ROLE_CLAIMS  WHERE RoleId = '4eae855f-500f-4912-90fc-fe399fcb6fea' AND ClaimValue = 'Permissions.CMS.MOJ.ExecutionList')
	INSERT INTO UMS_ROLE_CLAIMS VALUES ('4eae855f-500f-4912-90fc-fe399fcb6fea', 'Permission', 'Permissions.CMS.MOJ.ExecutionList')
GO 


IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Permissions.CMS.MOJ.ExecutionView')
	INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted])
		VALUES ('Detail Of Execution Request','CMS', 'MOJ', 'Permission', 'Permissions.CMS.MOJ.ExecutionView','Detail Of Execution Request',0)
GO 


IF NOT Exists(SELECT 1 from UMS_ROLE_CLAIMS  WHERE RoleId = '4eae855f-500f-4912-90fc-fe399fcb6fea' AND ClaimValue = 'Permissions.CMS.MOJ.ExecutionView')
	INSERT INTO UMS_ROLE_CLAIMS VALUES ('4eae855f-500f-4912-90fc-fe399fcb6fea', 'Permission', 'Permissions.CMS.MOJ.ExecutionView')
GO 

----------------- Contact Management Start ---------
IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Permissions.Menu.Contact')
	INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted])
		VALUES ('Contact Management Menu','Contact Management', 'Menu', 'Permission', 'Permissions.Menu.Contact','Contact Management Menu',0)
GO 

IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Permissions.Submenu.Contact.ContactList')
	INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted])
		VALUES ('Contact Management List','Contact Management', 'Contact List', 'Permission', 'Permissions.Submenu.Contact.ContactList','Contact Management List',0)
GO

IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Permissions.Contact.AddContact.Create')
	INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted])
		VALUES ('Contact Management Create','Contact Management', 'Contact Create', 'Permission', 'Permissions.Contact.AddContact.Create','Contact Management Create',0)
GO
IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Permissions.Contact.AddContact.Edit')
	INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted])
		VALUES ('Contact Management Edit','Contact Management', 'Contact Edit', 'Permission', 'Permissions.Contact.AddContact.Edit','Contact Management Edit',0)
GO

IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Permissions.Contact.DeleteContact.Delete')
	INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted])
		VALUES ('Contact Management Delete','Contact Management', 'Contact Delete', 'Permission', 'Permissions.Contact.DeleteContact.Delete','Contact Management Delete',0)
GO

IF((SELECT COUNT(*) FROM UMS_ROLE_CLAIMS  WHERE RoleId = 'd6b3075c-3f70-4b44-baa4-1fdc599a6bb2' AND ClaimValue = 'Permissions.Menu.Contact') = 0)
BEGIN
INSERT INTO UMS_ROLE_CLAIMS VALUES ('d6b3075c-3f70-4b44-baa4-1fdc599a6bb2', 'Permission', 'Permissions.Menu.Contact')
END

IF((SELECT COUNT(*) FROM UMS_ROLE_CLAIMS  WHERE RoleId = 'd6b3075c-3f70-4b44-baa4-1fdc599a6bb2' AND ClaimValue = 'Permissions.Submenu.Contact.ContactList') = 0)
BEGIN
INSERT INTO UMS_ROLE_CLAIMS VALUES ('d6b3075c-3f70-4b44-baa4-1fdc599a6bb2', 'Permission', 'Permissions.Submenu.Contact.ContactList')
END

IF((SELECT COUNT(*) FROM UMS_ROLE_CLAIMS  WHERE RoleId = 'd6b3075c-3f70-4b44-baa4-1fdc599a6bb2' AND ClaimValue = 'Permissions.Contact.AddContact.Create') = 0)
BEGIN
INSERT INTO UMS_ROLE_CLAIMS VALUES ('d6b3075c-3f70-4b44-baa4-1fdc599a6bb2', 'Permission', 'Permissions.Contact.AddContact.Create')
END

IF((SELECT COUNT(*) FROM UMS_ROLE_CLAIMS  WHERE RoleId = 'd6b3075c-3f70-4b44-baa4-1fdc599a6bb2' AND ClaimValue = 'Permissions.Contact.AddContact.Edit') = 0)
BEGIN
INSERT INTO UMS_ROLE_CLAIMS VALUES ('d6b3075c-3f70-4b44-baa4-1fdc599a6bb2', 'Permission', 'Permissions.Contact.AddContact.Edit')
END

IF((SELECT COUNT(*) FROM UMS_ROLE_CLAIMS  WHERE RoleId = 'd6b3075c-3f70-4b44-baa4-1fdc599a6bb2' AND ClaimValue = 'Permissions.Contact.DeleteContact.Delete') = 0)
BEGIN
INSERT INTO UMS_ROLE_CLAIMS VALUES ('d6b3075c-3f70-4b44-baa4-1fdc599a6bb2', 'Permission', 'Permissions.Contact.DeleteContact.Delete')
END

IF((SELECT COUNT(*) FROM MODULE  WHERE ModuleId = 11) = 0)
BEGIN
SET IDENTITY_INSERT [dbo].[MODULE] ON 
INSERT [dbo].[MODULE] ([ModuleId], [ModuleNameEn], [ModuleNameAr]) VALUES (11, 'CNT Contact Management', N'CNT Contact Management')
SET IDENTITY_INSERT [dbo].[MODULE] OFF
END

IF((SELECT COUNT(*) FROM ATTACHMENT_TYPE WHERE AttachmentTypeId = 41) = 0)
INSERT INTO ATTACHMENT_TYPE VALUES(41, N'مستند جهة الاتصال', 'Contact Document', 11, 1, 0, NULL)
GO

---------- Contact Management End -------------
GO 

update CMS_CASE_DECISION_TYPE_G2G_LKP set NameAr = N'تفسير الحكم' where Id = 1
update CMS_CASE_DECISION_TYPE_G2G_LKP set NameAr = N'بطلان الحكم' where Id = 2
update CMS_CASE_DECISION_TYPE_G2G_LKP set NameAr = N'وقف تنفيذ الحكم' where Id = 4
update CMS_CASE_DECISION_TYPE_G2G_LKP set NameAr = N'Regeneration Of Judgment Execution' where Id = 8

-----------------------------DMS_DB
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ATTACHMENT_TYPE]') AND type in (N'U'))
BEGIN
INSERT INTO ATTACHMENT_TYPE VALUES (42,'Opinion Document','Opinion Document',5,0,0,NULL)
END
------------------------UMS_CLAIM
IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Permissions.CMS.Case.DecisionList')
	INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted])
		VALUES ('Decision List','CMS', 'Registered Case', 'Permission', 'Permissions.CMS.Case.DecisionList','Decision List',0)
GO 
------------------------UMS_ROLE_CLAIMS

IF((SELECT COUNT(*) FROM UMS_ROLE_CLAIMS WHERE ClaimValue = 'Permissions.CMS.Case.DecisionList' AND RoleId ='93e5374b-cbd9-494e-92d4-d9d7d44c2c39') = 0)
 BEGIN
 INSERT INTO UMS_ROLE_CLAIMS VALUES ('93e5374b-cbd9-494e-92d4-d9d7d44c2c39', 'Permission', 'Permissions.CMS.Case.DecisionList')
 END
 ---------------------------UMS_USER_CLAIMS(FATWA_DB)
 IF((SELECT COUNT(*) FROM UMS_USER_CLAIMS WHERE ClaimValue = 'Permissions.CMS.Case.DecisionList' AND UserId ='10a0c422-5730-492b-8b8a-722b6e5674ba') = 0)

SET IDENTITY_INSERT [dbo].[UMS_USER_CLAIMS] ON 
GO
INSERT [dbo].[UMS_USER_CLAIMS] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (4755, N'10a0c422-5730-492b-8b8a-722b6e5674ba', N'Permission', N'Permissions.CMS.Case.DecisionList')
GO
SET IDENTITY_INSERT [dbo].[UMS_USER_CLAIMS] OFF 
-------------------------------UMS_USER_CLAIMS(FATWA_DB)
 IF((SELECT COUNT(*) FROM UMS_USER_CLAIMS WHERE ClaimValue = 'Permissions.CMS.Case.DecisionList' AND UserId ='a6a2b11f-f830-4fa1-a834-3e06ae44b748') = 0)

SET IDENTITY_INSERT [dbo].[UMS_USER_CLAIMS] ON 
GO
INSERT [dbo].[UMS_USER_CLAIMS] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (4756, N'a6a2b11f-f830-4fa1-a834-3e06ae44b748', N'Permission', N'Permissions.CMS.Case.DecisionList')
GO
SET IDENTITY_INSERT [dbo].[UMS_USER_CLAIMS] OFF 


IF NOT Exists(SELECT 1 from UMS_ROLE_CLAIMS  WHERE RoleId = '93e5374b-cbd9-494e-92d4-d9d7d44c2c39' AND ClaimValue = 'Permissions.CMS.MOJ.ExecutionView')
	INSERT INTO UMS_ROLE_CLAIMS VALUES ('93e5374b-cbd9-494e-92d4-d9d7d44c2c39', 'Permission', 'Permissions.CMS.MOJ.ExecutionView')
GO 

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LINK_TARGET_TYPE]') AND type in (N'U'))
BEGIN
INSERT INTO LINK_TARGET_TYPE VALUES ('256','Registered Case')
END

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[COMS_CONSULTATION_PARTY_TYPE_G2G_LKP]') AND type in (N'U'))
BEGIN
update COMS_CONSULTATION_PARTY_TYPE_G2G_LKP set Name_En = 'First Party' where Id = 1
update COMS_CONSULTATION_PARTY_TYPE_G2G_LKP set Name_Ar = N'الطرف الأول' where Id = 1
update COMS_CONSULTATION_PARTY_TYPE_G2G_LKP set Name_En = 'Second Party' where Id = 2
update COMS_CONSULTATION_PARTY_TYPE_G2G_LKP set Name_Ar = N'الطرف الثاني' where Id = 2
END
----CMS----
IF((SELECT COUNT(*) FROM CMS_CASE_FILE_EVENT_G2G_LKP WHERE Id = '512') <= 0)
SET IDENTITY_INSERT CMS_CASE_FILE_EVENT_G2G_LKP ON
INSERT INTO CMS_CASE_FILE_EVENT_G2G_LKP (Id,Name_En,Name_Ar)VALUES(512,'IsAssignedBack','IsAssignedBack')
SET IDENTITY_INSERT CMS_CASE_FILE_EVENT_G2G_LKP OFF
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[COMM_COMMUNICATION_TYPE]') AND type in (N'U'))

INSERT [dbo].[COMM_COMMUNICATION_TYPE] ([CommunicationTypeId], [NameAr], [NameEn]) VALUES (256, N'طلب العقود ', N'Contract Request')
INSERT [dbo].[COMM_COMMUNICATION_TYPE] ([CommunicationTypeId], [NameAr], [NameEn]) VALUES (512, N'طلب التشريع ', N'Legislation Request')
INSERT [dbo].[COMM_COMMUNICATION_TYPE] ([CommunicationTypeId], [NameAr], [NameEn]) VALUES (1024, N'طلب الفتوى', N'Legal Advice Request')
INSERT [dbo].[COMM_COMMUNICATION_TYPE] ([CommunicationTypeId], [NameAr], [NameEn]) VALUES (2048, N'طلب التظلم الإداري ', N'Administrative Complaint Request')
INSERT [dbo].[COMM_COMMUNICATION_TYPE] ([CommunicationTypeId], [NameAr], [NameEn]) VALUES (4096, N'طلب التحكيم الدولي ', N'International Arbitration Request')
GO
-------------------COMS_CONSULTATION_ARTICLE_STATUS
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[COMS_CONSULTATION_ARTICLE_STATUS]') AND type in (N'U'))

update COMS_CONSULTATION_ARTICLE_STATUS set Name_Ar=N'الحالة' where Id = 1
update COMS_CONSULTATION_ARTICLE_STATUS set Name_Ar=N'جديد' where Id = 2
update COMS_CONSULTATION_ARTICLE_STATUS set Name_Ar=N'يمكن تعديله' where Id = 4
update COMS_CONSULTATION_ARTICLE_STATUS set Name_Ar=N'لا يمكن تعديله' where Id = 8
GO


IF((SELECT COUNT(*) FROM ATTACHMENT_TYPE WHERE AttachmentTypeId = 44) = 0)
INSERT INTO ATTACHMENT_TYPE VALUES(44, N'Presentation Notes', 'Presentation Notes', 5, 0, 0, 0)
GO

IF((SELECT COUNT(*) FROM ATTACHMENT_TYPE WHERE AttachmentTypeId = 45) = 0)
INSERT INTO ATTACHMENT_TYPE VALUES(45, N'Open Pleading Request', 'Open Pleading Request', 5, 0, 0, 0)
GO

IF((SELECT COUNT(*) FROM ATTACHMENT_TYPE WHERE AttachmentTypeId = 46) = 0)
INSERT INTO ATTACHMENT_TYPE VALUES(46, N'Initial Judgement Notification', 'Initial Judgement Notification', 5, 0, 0, 0)
GO

IF((SELECT COUNT(*) FROM ATTACHMENT_TYPE WHERE AttachmentTypeId = 47) = 0)
INSERT INTO ATTACHMENT_TYPE VALUES(47, N'General Update Notification', 'General Update Notification', 5, 0, 0, 0)
GO

IF((SELECT COUNT(*) FROM ATTACHMENT_TYPE WHERE AttachmentTypeId = 48) = 0)
INSERT INTO ATTACHMENT_TYPE VALUES(48, N'Case Registered Notification', 'Case Registered Notification', 5, 0, 0, 0)
GO

IF((SELECT COUNT(*) FROM ATTACHMENT_TYPE WHERE AttachmentTypeId = 49) = 0)
INSERT INTO ATTACHMENT_TYPE VALUES(49, N'Additional Information Notification', 'Additional Information Notification', 5, 0, 0, 0)
GO

IF((SELECT COUNT(*) FROM ATTACHMENT_TYPE WHERE AttachmentTypeId = 50) = 0)
INSERT INTO ATTACHMENT_TYPE VALUES(50, N'Saving File Notification', 'Saving File Notification', 5, 0, 0, 0)
GO

IF((SELECT COUNT(*) FROM ATTACHMENT_TYPE WHERE AttachmentTypeId = 51) = 0)
INSERT INTO ATTACHMENT_TYPE VALUES(51, N'Additional Information Reminder Notification', 'Additional Information Reminder Notification', 5, 0, 0, 0)
GO

IF((SELECT COUNT(*) FROM ATTACHMENT_TYPE WHERE AttachmentTypeId = 52) = 0)
INSERT INTO ATTACHMENT_TYPE VALUES(52, N'Case Closing Document', 'Case Closing Document', 5, 0, 0, 0)
GO

IF((SELECT COUNT(*) FROM ATTACHMENT_TYPE WHERE AttachmentTypeId = 53) = 0)
INSERT INTO ATTACHMENT_TYPE VALUES(53, N'Final Judgement Notification', 'Final Judgement Notification', 5, 0, 0, 0)
GO

IF((SELECT COUNT(*) FROM ATTACHMENT_TYPE WHERE AttachmentTypeId = 54) = 0)
INSERT INTO ATTACHMENT_TYPE VALUES(54, N'Hearing Document', 'Hearing Document', 5, 0, 0, 0)
GO

IF((SELECT COUNT(*) FROM ATTACHMENT_TYPE WHERE AttachmentTypeId = 55) = 0)
INSERT INTO ATTACHMENT_TYPE VALUES(55, N'Defense Document', 'Defense Document', 5, 0, 0, 0)
GO

IF((SELECT COUNT(*) FROM ATTACHMENT_TYPE WHERE AttachmentTypeId = 56) = 0)
INSERT INTO ATTACHMENT_TYPE VALUES(56, N'Execution Additional Information Notification', 'Execution Additional Information Notification', 5, 0, 0, 0)
GO

IF((SELECT COUNT(*) FROM ATTACHMENT_TYPE WHERE AttachmentTypeId = 57) = 0)
INSERT INTO ATTACHMENT_TYPE VALUES(57, N'Execution File Opened', 'Execution File Opened', 5, 0, 0, 0)
GO

IF((SELECT COUNT(*) FROM ATTACHMENT_TYPE WHERE AttachmentTypeId = 58) = 0)
INSERT INTO ATTACHMENT_TYPE VALUES(58, N'Postpone Hearing Document', 'Postpone Hearing Document', 5, 0, 0, 0)
GO

----CMS_CHAMBER_G2G_LKP---
update CMS_CHAMBER_G2G_LKP set Name_Ar = N'الدائرة الأولى'
where Name_Ar = N'Chamber 1' 
update CMS_CHAMBER_G2G_LKP set Name_Ar = N'الدائرة الثانية'
where Name_Ar = N'Chamber 2' 
update CMS_CHAMBER_G2G_LKP set Name_Ar = N'الدائرة الثالثة'
where Name_Ar = N'Chamber 3' 
update CMS_CHAMBER_G2G_LKP set Name_Ar = N'الدائرة الرابعة'
where Name_Ar = N'Chamber 4' 

--CMS_COURT_G2G_LKP---
update CMS_COURT_G2G_LKP set Name_Ar = N'المحكمة الجزئية المستعجلة'
where Name_Ar = N'Partial Urgent Court'

----CMS_COURT_TYPE_G2G_LKP---
update CMS_COURT_TYPE_G2G_LKP set Name_Ar = N'جزئي مستعجل'
where Name_Ar = N'Partial & Urgent'

---CMS_PRE_COURT_TYPE_G2G_LKP---
Update CMS_PRE_COURT_TYPE_G2G_LKP Set Name_Ar=N'الملكية' 
where Name_Ar=N'Property'


IF((SELECT COUNT(*) FROM CMS_REGISTERED_CASE_EVENT_G2G_LKP WHERE Id = 7) = 0)
SET IDENTITY_INSERT CMS_REGISTERED_CASE_EVENT_G2G_LKP ON
INSERT INTO CMS_REGISTERED_CASE_EVENT_G2G_LKP (Id, Name_En, Name_Ar) VALUES (7,'Closed',N'Closed')
SET IDENTITY_INSERT CMS_REGISTERED_CASE_EVENT_G2G_LKP OFF
GO

-----------MODULE
update MODULE set ModuleNameAr = N'نظام إدارة ' where ModuleId =10

--------------------------[UMS_GROUP_CLAIMS]
SET IDENTITY_INSERT [dbo].[UMS_GROUP_CLAIMS] ON 
GO
INSERT [dbo].[UMS_GROUP_CLAIMS] ([Id], [GroupId], [ClaimType], [ClaimValue]) VALUES (502, N'93C1C1D4-73FE-4944-8F3E-8B9B983646F8', N'Permission', N'Permissions.Comm.Communication.Detail')
GO
INSERT [dbo].[UMS_GROUP_CLAIMS] ([Id], [GroupId], [ClaimType], [ClaimValue]) VALUES (503, N'D4974198-4951-4C0F-8848-A2FC148E1022', N'Permission', N'Permissions.Comm.Communication.Detail')
GO
INSERT [dbo].[UMS_GROUP_CLAIMS] ([Id], [GroupId], [ClaimType], [ClaimValue]) VALUES (504, N'85D6848C-3BF1-403E-9CF6-B749D197ED15', N'Permission', N'Permissions.Comm.Communication.Detail')
GO
INSERT [dbo].[UMS_GROUP_CLAIMS] ([Id], [GroupId], [ClaimType], [ClaimValue]) VALUES (505, N'E5E9AFCE-3123-48E5-A0CC-EAEC678AA179', N'Permission', N'Permissions.Comm.Communication.Detail')
GO
SET IDENTITY_INSERT [dbo].[UMS_GROUP_CLAIMS] OFF 
IF((SELECT COUNT(*) FROM UMS_USER_TYPE WHERE Id = 8) = 0)
INSERT INTO UMS_USER_TYPE VALUES (8,'GS',N'GS')
GO
IF((SELECT COUNT(*) FROM UMS_USER_TYPE WHERE Id = 8) = 0)
INSERT INTO UMS_USER_TYPE VALUES (16,'IT',N'IT')
GO
INSERT [dbo].[UMS_USER_FLOOR_LKP]  VALUES (1, N'Floor 1 ', N'Floor 1')
INSERT [dbo].[UMS_USER_FLOOR_LKP]  VALUES (2, N'Floor 2 ', N'Floor 2')
INSERT [dbo].[UMS_USER_FLOOR_LKP]  VALUES (4, N'Floor 3 ', N'Floor 3')


UPDATE ATTACHMENT_TYPE SET Type_Ar = N'اخطار بما تم' WHERE AttachmentTypeId = 47
UPDATE ATTACHMENT_TYPE SET Type_Ar = N'اخطار باغلاق القضية' WHERE AttachmentTypeId = 52
UPDATE ATTACHMENT_TYPE SET Type_Ar = N'اخطار بفتح ملف التنفيذ', Type_En = 'File Execution Notification' WHERE AttachmentTypeId = 56

-------------------- Inventory Claim

IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Permissions.Menu.INV')
	INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar], [IsDeleted])
		VALUES ('Inventory Menu', 'Inventory Management', 'Menu', 'Permission', 'Permissions.Menu.INV', 'Inventory', 0)
GO

IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Permissions.Submenu.INV.Inventory.List')
	INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar], [IsDeleted])
		VALUES ('Inventory Submenu', 'Inventory Management', 'Request List', 'Permission', 'Permissions.Submenu.INV.Inventory.List', 'Inventory', 0)
GO

IF Exists(SELECT 1 from UMS_GROUP_CLAIMS where Id = 509)
	DELETE FROM UMS_GROUP_CLAIMS WHERE Id = 509
GO

IF Exists(SELECT 1 from UMS_GROUP_CLAIMS where Id = 512)
	DELETE FROM UMS_GROUP_CLAIMS WHERE Id = 512
GO

IF NOT Exists(SELECT 1 from UMS_GROUP_CLAIMS where GroupId = '036B1CA4-DD4E-4B02-A639-EFDB4A9E7147' and ClaimValue = 'Permissions.Dashboard.INV.View')
	INSERT INTO [dbo].[UMS_GROUP_CLAIMS] ([GroupId],[ClaimType],[ClaimValue])
		VALUES ('036B1CA4-DD4E-4B02-A639-EFDB4A9E7147', 'Permission', 'Permissions.Dashboard.INV.View')
GO

IF NOT Exists(SELECT 1 from UMS_GROUP_CLAIMS where GroupId = '036B1CA4-DD4E-4B02-A639-EFDB4A9E7147' and ClaimValue = 'Permissions.Menu.INV')
	INSERT INTO [dbo].[UMS_GROUP_CLAIMS] ([GroupId],[ClaimType],[ClaimValue])
		VALUES ('036B1CA4-DD4E-4B02-A639-EFDB4A9E7147', 'Permission', 'Permissions.Menu.INV')
GO

IF NOT Exists(SELECT 1 from UMS_GROUP_CLAIMS where GroupId = '036B1CA4-DD4E-4B02-A639-EFDB4A9E7147' and ClaimValue = 'Permissions.Submenu.INV.Inventory.List')
	INSERT INTO [dbo].[UMS_GROUP_CLAIMS] ([GroupId],[ClaimType],[ClaimValue])
		VALUES ('036B1CA4-DD4E-4B02-A639-EFDB4A9E7147', 'Permission', 'Permissions.Submenu.INV.Inventory.List')
GO

IF NOT Exists(SELECT 1 from UMS_GROUP_CLAIMS where GroupId = '949ED508-5CBD-4A07-B319-7508A68BA117' and ClaimValue = 'Permissions.Menu.Notfication')
	INSERT INTO [dbo].[UMS_GROUP_CLAIMS] ([GroupId],[ClaimType],[ClaimValue])
		VALUES ('949ED508-5CBD-4A07-B319-7508A68BA117', 'Permission', 'Permissions.Menu.Notfication')
GO

IF NOT Exists(SELECT 1 from UMS_GROUP_CLAIMS where GroupId = '949ED508-5CBD-4A07-B319-7508A68BA117' and ClaimValue = 'Permissions.Submenu.Notfication.NotificationList')
	INSERT INTO [dbo].[UMS_GROUP_CLAIMS] ([GroupId],[ClaimType],[ClaimValue])
		VALUES ('949ED508-5CBD-4A07-B319-7508A68BA117', 'Permission', 'Permissions.Submenu.Notfication.NotificationList')
GO
IF NOT Exists(SELECT 1 from UMS_GROUP_CLAIMS where GroupId = '949ED508-5CBD-4A07-B319-7508A68BA117' and ClaimValue = 'Permissions.Menu.AuditLogs')
	INSERT INTO [dbo].[UMS_GROUP_CLAIMS] ([GroupId],[ClaimType],[ClaimValue])
		VALUES ('949ED508-5CBD-4A07-B319-7508A68BA117', 'Permission', 'Permissions.Menu.AuditLogs')
GO
IF NOT Exists(SELECT 1 from UMS_GROUP_CLAIMS where GroupId = '949ED508-5CBD-4A07-B319-7508A68BA117' and ClaimValue = 'Permissions.Submenu.AuditLogs.ErrorLogs')
	INSERT INTO [dbo].[UMS_GROUP_CLAIMS] ([GroupId],[ClaimType],[ClaimValue])
		VALUES ('949ED508-5CBD-4A07-B319-7508A68BA117', 'Permission', 'Permissions.Submenu.AuditLogs.ErrorLogs')
GO
IF NOT Exists(SELECT 1 from UMS_GROUP_CLAIMS where GroupId = '949ED508-5CBD-4A07-B319-7508A68BA117' and ClaimValue = 'Permissions.Submenu.AuditLogs.ProcessLogs')
	INSERT INTO [dbo].[UMS_GROUP_CLAIMS] ([GroupId],[ClaimType],[ClaimValue])
		VALUES ('949ED508-5CBD-4A07-B319-7508A68BA117', 'Permission', 'Permissions.Submenu.AuditLogs.ProcessLogs')
GO

-------------

IF NOT Exists(SELECT 1 from UMS_GROUP_CLAIMS where GroupId = '036B1CA4-DD4E-4B02-A639-EFDB4A9E7147' and ClaimValue = 'Permissions.Menu.Notfication')
	INSERT INTO [dbo].[UMS_GROUP_CLAIMS] ([GroupId],[ClaimType],[ClaimValue])
		VALUES ('036B1CA4-DD4E-4B02-A639-EFDB4A9E7147', 'Permission', 'Permissions.Menu.Notfication')
GO

IF NOT Exists(SELECT 1 from UMS_GROUP_CLAIMS where GroupId = '036B1CA4-DD4E-4B02-A639-EFDB4A9E7147' and ClaimValue = 'Permissions.Submenu.Notfication.NotificationList')
	INSERT INTO [dbo].[UMS_GROUP_CLAIMS] ([GroupId],[ClaimType],[ClaimValue])
		VALUES ('036B1CA4-DD4E-4B02-A639-EFDB4A9E7147', 'Permission', 'Permissions.Submenu.Notfication.NotificationList')
GO
IF NOT Exists(SELECT 1 from UMS_GROUP_CLAIMS where GroupId = '036B1CA4-DD4E-4B02-A639-EFDB4A9E7147' and ClaimValue = 'Permissions.Menu.AuditLogs')
	INSERT INTO [dbo].[UMS_GROUP_CLAIMS] ([GroupId],[ClaimType],[ClaimValue])
		VALUES ('036B1CA4-DD4E-4B02-A639-EFDB4A9E7147', 'Permission', 'Permissions.Menu.AuditLogs')
GO
IF NOT Exists(SELECT 1 from UMS_GROUP_CLAIMS where GroupId = '036B1CA4-DD4E-4B02-A639-EFDB4A9E7147' and ClaimValue = 'Permissions.Submenu.AuditLogs.ErrorLogs')
	INSERT INTO [dbo].[UMS_GROUP_CLAIMS] ([GroupId],[ClaimType],[ClaimValue])
		VALUES ('036B1CA4-DD4E-4B02-A639-EFDB4A9E7147', 'Permission', 'Permissions.Submenu.AuditLogs.ErrorLogs')
GO
IF NOT Exists(SELECT 1 from UMS_GROUP_CLAIMS where GroupId = '036B1CA4-DD4E-4B02-A639-EFDB4A9E7147' and ClaimValue = 'Permissions.Submenu.AuditLogs.ProcessLogs')
	INSERT INTO [dbo].[UMS_GROUP_CLAIMS] ([GroupId],[ClaimType],[ClaimValue])
		VALUES ('036B1CA4-DD4E-4B02-A639-EFDB4A9E7147', 'Permission', 'Permissions.Submenu.AuditLogs.ProcessLogs')
GO
-----------------

IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Permissions.INV.Inventory.RequestItemDetailView')
	INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar], [IsDeleted])
		VALUES ('Inventory Request Item Detail', 'Inventory Management', 'Request Item Detail', 'Permission', 'Permissions.INV.Inventory.RequestItemDetailView', 'Inventory', 0)
GO

-------------------

IF NOT Exists(SELECT 1 from UMS_GROUP_CLAIMS where GroupId = '036B1CA4-DD4E-4B02-A639-EFDB4A9E7147' and ClaimValue = 'Permissions.INV.Inventory.RequestItemDetailView')
	INSERT INTO [dbo].[UMS_GROUP_CLAIMS] ([GroupId],[ClaimType],[ClaimValue])
		VALUES ('036B1CA4-DD4E-4B02-A639-EFDB4A9E7147', 'Permission', 'Permissions.INV.Inventory.RequestItemDetailView')
GO

IF NOT Exists(SELECT 1 from UMS_GROUP_CLAIMS where GroupId = '949ED508-5CBD-4A07-B319-7508A68BA117' and ClaimValue = 'Permissions.INV.Inventory.RequestItemDetailView')
	INSERT INTO [dbo].[UMS_GROUP_CLAIMS] ([GroupId],[ClaimType],[ClaimValue])
		VALUES ('949ED508-5CBD-4A07-B319-7508A68BA117', 'Permission', 'Permissions.INV.Inventory.RequestItemDetailView')
GO
--------------------------[UMS_GROUP_CLAIMS]
SET IDENTITY_INSERT [dbo].[UMS_GROUP_CLAIMS] ON 
GO
INSERT [dbo].[UMS_GROUP_CLAIMS] ([Id], [GroupId], [ClaimType], [ClaimValue]) VALUES (860, N'071C4BA0-CC3F-43DC-B2CB-6FC2BEA6D831', N'Permission', N'Permissions.Menu.Document')
GO																							
INSERT [dbo].[UMS_GROUP_CLAIMS] ([Id], [GroupId], [ClaimType], [ClaimValue]) VALUES (861, N'071C4BA0-CC3F-43DC-B2CB-6FC2BEA6D831', N'Permission', N'Permissions.Submenu.Document.DocumentList')
GO
SET IDENTITY_INSERT [dbo].[UMS_GROUP_CLAIMS] OFF 
----------------------[MODULE](FATWA_DB)
SET IDENTITY_INSERT [dbo].[MODULE] ON 
GO
INSERT [dbo].[MODULE]([ModuleId],[ModuleNameEn],[ModuleNameAr]) VALUES(12, 'Document Management' , N'Document Management')
SET IDENTITY_INSERT [dbo].[MODULE] OFF 
GO
------------------------DMS_FILE_TYPES_LKP(DMS_DB)
INSERT [dbo].[DMS_FILE_TYPES_LKP]([Id],[Name_En],[Name_Ar]) VALUES (2,'.pdf',N'.pdf')
INSERT [dbo].[DMS_FILE_TYPES_LKP]([Id],[Name_En],[Name_Ar]) VALUES (4,'.jpg',N'.jpg')
INSERT [dbo].[DMS_FILE_TYPES_LKP]([Id],[Name_En],[Name_Ar]) VALUES (8,'.jpeg',N'.jpeg')
INSERT [dbo].[DMS_FILE_TYPES_LKP]([Id],[Name_En],[Name_Ar]) VALUES (16,'.png',N'.png')
INSERT [dbo].[DMS_FILE_TYPES_LKP]([Id],[Name_En],[Name_Ar]) VALUES (32,'.msg',N'.msg')
-------------------UMS_GROUP_CLAIMS (FATWA_DB)
SET IDENTITY_INSERT [dbo].[UMS_GROUP_CLAIMS] ON 
GO
INSERT [dbo].[UMS_GROUP_CLAIMS]([Id],[GroupId],[ClaimType],[ClaimValue]) VALUES(918, '4A6C0AA1-D091-4C01-B6F3-4CDD3780A06A' , 'Permission','Permissions.Dashboard.DMS.View')
INSERT [dbo].[UMS_GROUP_CLAIMS]([Id],[GroupId],[ClaimType],[ClaimValue]) VALUES(919, '49744B64-2399-4097-94FD-326D6FCE2626' , 'Permission','Permissions.Dashboard.DMS.View')
INSERT [dbo].[UMS_GROUP_CLAIMS]([Id],[GroupId],[ClaimType],[ClaimValue]) VALUES(920, '8EA8BEA4-B490-48E2-9776-8BA2B3EED060' , 'Permission','Permissions.Dashboard.DMS.View')
INSERT [dbo].[UMS_GROUP_CLAIMS]([Id],[GroupId],[ClaimType],[ClaimValue]) VALUES(921, '071C4BA0-CC3F-43DC-B2CB-6FC2BEA6D831' , 'Permission','Permissions.Dashboard.DMS.View')
INSERT [dbo].[UMS_GROUP_CLAIMS]([Id],[GroupId],[ClaimType],[ClaimValue]) VALUES(922, '93C1C1D4-73FE-4944-8F3E-8B9B983646F8' , 'Permission','Permissions.Dashboard.DMS.View')
INSERT [dbo].[UMS_GROUP_CLAIMS]([Id],[GroupId],[ClaimType],[ClaimValue]) VALUES(923, '9D33079E-1183-4CCE-BF5F-9CEF1605D3AB' , 'Permission','Permissions.Dashboard.DMS.View')
INSERT [dbo].[UMS_GROUP_CLAIMS]([Id],[GroupId],[ClaimType],[ClaimValue]) VALUES(924, '85D6848C-3BF1-403E-9CF6-B749D197ED15' , 'Permission','Permissions.Dashboard.DMS.View')
INSERT [dbo].[UMS_GROUP_CLAIMS]([Id],[GroupId],[ClaimType],[ClaimValue]) VALUES(925, 'E5E9AFCE-3123-48E5-A0CC-EAEC678AA179' , 'Permission','Permissions.Dashboard.DMS.View')
INSERT [dbo].[UMS_GROUP_CLAIMS]([Id],[GroupId],[ClaimType],[ClaimValue]) VALUES(926, '4A6C0AA1-D091-4C01-B6F3-4CDD3780A06A' , 'Permission','Permissions.Menu.Document')
INSERT [dbo].[UMS_GROUP_CLAIMS]([Id],[GroupId],[ClaimType],[ClaimValue]) VALUES(927, '49744B64-2399-4097-94FD-326D6FCE2626' , 'Permission','Permissions.Menu.Document')
INSERT [dbo].[UMS_GROUP_CLAIMS]([Id],[GroupId],[ClaimType],[ClaimValue]) VALUES(928, '8EA8BEA4-B490-48E2-9776-8BA2B3EED060' , 'Permission','Permissions.Menu.Document')
INSERT [dbo].[UMS_GROUP_CLAIMS]([Id],[GroupId],[ClaimType],[ClaimValue]) VALUES(929, '93C1C1D4-73FE-4944-8F3E-8B9B983646F8' , 'Permission','Permissions.Menu.Document')
INSERT [dbo].[UMS_GROUP_CLAIMS]([Id],[GroupId],[ClaimType],[ClaimValue]) VALUES(930, '9D33079E-1183-4CCE-BF5F-9CEF1605D3AB' , 'Permission','Permissions.Menu.Document')
INSERT [dbo].[UMS_GROUP_CLAIMS]([Id],[GroupId],[ClaimType],[ClaimValue]) VALUES(931, '85D6848C-3BF1-403E-9CF6-B749D197ED15' , 'Permission','Permissions.Menu.Document')
INSERT [dbo].[UMS_GROUP_CLAIMS]([Id],[GroupId],[ClaimType],[ClaimValue]) VALUES(932, 'E5E9AFCE-3123-48E5-A0CC-EAEC678AA179' , 'Permission','Permissions.Menu.Document')

INSERT [dbo].[UMS_GROUP_CLAIMS]([Id],[GroupId],[ClaimType],[ClaimValue]) VALUES(933, '4A6C0AA1-D091-4C01-B6F3-4CDD3780A06A' , 'Permission','Permissions.Submenu.Document.DocumentList')
INSERT [dbo].[UMS_GROUP_CLAIMS]([Id],[GroupId],[ClaimType],[ClaimValue]) VALUES(934, '49744B64-2399-4097-94FD-326D6FCE2626' , 'Permission','Permissions.Submenu.Document.DocumentList')
INSERT [dbo].[UMS_GROUP_CLAIMS]([Id],[GroupId],[ClaimType],[ClaimValue]) VALUES(935, '8EA8BEA4-B490-48E2-9776-8BA2B3EED060' , 'Permission','Permissions.Submenu.Document.DocumentList')
INSERT [dbo].[UMS_GROUP_CLAIMS]([Id],[GroupId],[ClaimType],[ClaimValue]) VALUES(936, '93C1C1D4-73FE-4944-8F3E-8B9B983646F8' , 'Permission','Permissions.Submenu.Document.DocumentList')
INSERT [dbo].[UMS_GROUP_CLAIMS]([Id],[GroupId],[ClaimType],[ClaimValue]) VALUES(937, '9D33079E-1183-4CCE-BF5F-9CEF1605D3AB' , 'Permission','Permissions.Submenu.Document.DocumentList')
INSERT [dbo].[UMS_GROUP_CLAIMS]([Id],[GroupId],[ClaimType],[ClaimValue]) VALUES(938, '85D6848C-3BF1-403E-9CF6-B749D197ED15' , 'Permission','Permissions.Submenu.Document.DocumentList')
INSERT [dbo].[UMS_GROUP_CLAIMS]([Id],[GroupId],[ClaimType],[ClaimValue]) VALUES(939, 'E5E9AFCE-3123-48E5-A0CC-EAEC678AA179' , 'Permission','Permissions.Submenu.Document.DocumentList')


SET IDENTITY_INSERT [dbo].[UMS_GROUP_CLAIMS] OFF 
GO
GO

---------------

IF((SELECT COUNT(*) FROM LEGAL_TEMPLATE_SETTING WHERE TemplateSettingId = 128) = 1)
update LEGAL_TEMPLATE_SETTING set Template_Heading_Ar = N'مواد التشريع', Template_Value_Ar = N'مواد مع أقسام' where TemplateSettingId = 128
GO
IF((SELECT COUNT(*) FROM LEGAL_TEMPLATE_SETTING WHERE TemplateSettingId = 256) = 1)
update LEGAL_TEMPLATE_SETTING set Template_Heading_Ar = N'مواد التشريع', Template_Value_Ar = N'مواد بدون أقسام' where TemplateSettingId = 256
GO
IF((SELECT COUNT(*) FROM LEGAL_TEMPLATE_SETTING WHERE TemplateSettingId = 512) = 1)
update LEGAL_TEMPLATE_SETTING set Template_Heading_Ar = N'مواد التشريع', Template_Value_Ar = N'بنود مع أقسام' where TemplateSettingId = 512
GO
IF((SELECT COUNT(*) FROM LEGAL_TEMPLATE_SETTING WHERE TemplateSettingId = 1024) = 1)
update LEGAL_TEMPLATE_SETTING set Template_Heading_Ar = N'مواد التشريع', Template_Value_Ar = N'بنود بدون أقسام' where TemplateSettingId = 1024
GO
IF((SELECT COUNT(*) FROM LEGAL_TEMPLATE_SETTING WHERE TemplateSettingId = 2048) = 1)
update LEGAL_TEMPLATE_SETTING set Template_Heading_Ar = N'المذكرة الايضاحية', Template_Value_Ar = N'المذكرة الايضاحية' where TemplateSettingId = 2048
GO
IF((SELECT COUNT(*) FROM LEGAL_TEMPLATE_SETTING WHERE TemplateSettingId = 4096) = 1)
update LEGAL_TEMPLATE_SETTING set Template_Heading_Ar = N'ملاحظات', Template_Value_Ar = N'ملاحظات' where TemplateSettingId = 4096
GO

----------------------

IF NOT Exists(SELECT 1 from UMS_GROUP_CLAIMS where ClaimValue = 'Permissions.Submenu.LDS.Legislation.List')
	INSERT INTO [dbo].[UMS_GROUP_CLAIMS] ([GroupId],[ClaimType],[ClaimValue])
		VALUES ('071C4BA0-CC3F-43DC-B2CB-6FC2BEA6D831', 'Permission', 'Permissions.Submenu.LDS.Legislation.List')
GO

IF NOT Exists(SELECT 1 from UMS_GROUP_CLAIMS where ClaimValue = 'Permissions.Submenu.LDS.Documents.DocumentReview')
	INSERT INTO [dbo].[UMS_GROUP_CLAIMS] ([GroupId],[ClaimType],[ClaimValue])
		VALUES ('071C4BA0-CC3F-43DC-B2CB-6FC2BEA6D831', 'Permission', 'Permissions.Submenu.LDS.Documents.DocumentReview')
GO

IF NOT Exists(SELECT 1 from UMS_GROUP_CLAIMS where ClaimValue = 'Permissions.Submenu.LDS.Documents.DocumentApproval')
	INSERT INTO [dbo].[UMS_GROUP_CLAIMS] ([GroupId],[ClaimType],[ClaimValue])
		VALUES ('071C4BA0-CC3F-43DC-B2CB-6FC2BEA6D831', 'Permission', 'Permissions.Submenu.LDS.Documents.DocumentApproval')
GO

IF NOT Exists(SELECT 1 from UMS_GROUP_CLAIMS where ClaimValue = 'Permissions.Submenu.LDS.Documents.PublishUnpublish')
	INSERT INTO [dbo].[UMS_GROUP_CLAIMS] ([GroupId],[ClaimType],[ClaimValue])
		VALUES ('071C4BA0-CC3F-43DC-B2CB-6FC2BEA6D831', 'Permission', 'Permissions.Submenu.LDS.Documents.PublishUnpublish')
GO

IF NOT Exists(SELECT 1 from UMS_GROUP_CLAIMS where ClaimValue = 'Permissions.Submenu.LDS.Documents.List.Delete')
	INSERT INTO [dbo].[UMS_GROUP_CLAIMS] ([GroupId],[ClaimType],[ClaimValue])
		VALUES ('071C4BA0-CC3F-43DC-B2CB-6FC2BEA6D831', 'Permission', 'Permissions.Submenu.LDS.Documents.List.Delete')
GO

---------------------------

IF NOT Exists(SELECT 1 from UMS_USER_CLAIMS where UserId = '0226a7d2-1d08-4348-b94c-20ae662fff19' AND ClaimValue = 'Permissions.Submenu.LDS.Legislation.List')
	INSERT INTO [dbo].[UMS_USER_CLAIMS] ([UserId],[ClaimType], [ClaimValue])
		VALUES ('0226a7d2-1d08-4348-b94c-20ae662fff19', 'Permission', 'Permissions.Submenu.LDS.Legislation.List')
GO
IF NOT Exists(SELECT 1 from UMS_USER_CLAIMS where UserId = '0226a7d2-1d08-4348-b94c-20ae662fff19' AND ClaimValue = 'Permissions.Submenu.LDS.Documents.DocumentReview')
	INSERT INTO [dbo].[UMS_USER_CLAIMS] ([UserId],[ClaimType], [ClaimValue])
		VALUES ('0226a7d2-1d08-4348-b94c-20ae662fff19', 'Permission', 'Permissions.Submenu.LDS.Documents.DocumentReview')
GO
IF NOT Exists(SELECT 1 from UMS_USER_CLAIMS where UserId = '0226a7d2-1d08-4348-b94c-20ae662fff19' AND ClaimValue = 'Permissions.Submenu.LDS.Documents.DocumentApproval')
	INSERT INTO [dbo].[UMS_USER_CLAIMS] ([UserId],[ClaimType], [ClaimValue])
		VALUES ('0226a7d2-1d08-4348-b94c-20ae662fff19', 'Permission', 'Permissions.Submenu.LDS.Documents.DocumentApproval')
GO
IF NOT Exists(SELECT 1 from UMS_USER_CLAIMS where UserId = '0226a7d2-1d08-4348-b94c-20ae662fff19' AND ClaimValue = 'Permissions.Submenu.LDS.Documents.PublishUnpublish')
	INSERT INTO [dbo].[UMS_USER_CLAIMS] ([UserId],[ClaimType], [ClaimValue])
		VALUES ('0226a7d2-1d08-4348-b94c-20ae662fff19', 'Permission', 'Permissions.Submenu.LDS.Documents.PublishUnpublish')
GO
IF NOT Exists(SELECT 1 from UMS_USER_CLAIMS where UserId = '0226a7d2-1d08-4348-b94c-20ae662fff19' AND ClaimValue = 'Permissions.Submenu.LDS.Documents.List.Delete')
	INSERT INTO [dbo].[UMS_USER_CLAIMS] ([UserId],[ClaimType], [ClaimValue])
		VALUES ('0226a7d2-1d08-4348-b94c-20ae662fff19', 'Permission', 'Permissions.Submenu.LDS.Documents.List.Delete')
GO
IF NOT Exists(SELECT 1 from UMS_USER_CLAIMS where UserId = '0226a7d2-1d08-4348-b94c-20ae662fff19' AND ClaimValue = 'Permissions.Menu.Notfication')
	INSERT INTO [dbo].[UMS_USER_CLAIMS] ([UserId],[ClaimType], [ClaimValue])
		VALUES ('0226a7d2-1d08-4348-b94c-20ae662fff19', 'Permission', 'Permissions.Menu.Notfication')
GO
IF NOT Exists(SELECT 1 from UMS_USER_CLAIMS where UserId = '0226a7d2-1d08-4348-b94c-20ae662fff19' AND ClaimValue = 'Permissions.Submenu.Notfication.NotificationList')
	INSERT INTO [dbo].[UMS_USER_CLAIMS] ([UserId],[ClaimType], [ClaimValue])
		VALUES ('0226a7d2-1d08-4348-b94c-20ae662fff19', 'Permission', 'Permissions.Submenu.Notfication.NotificationList')
GO
IF NOT Exists(SELECT 1 from UMS_USER_CLAIMS where UserId = '0226a7d2-1d08-4348-b94c-20ae662fff19' AND ClaimValue = 'Permissions.Menu.AuditLogs')
	INSERT INTO [dbo].[UMS_USER_CLAIMS] ([UserId],[ClaimType], [ClaimValue])
		VALUES ('0226a7d2-1d08-4348-b94c-20ae662fff19', 'Permission', 'Permissions.Menu.AuditLogs')
GO
IF NOT Exists(SELECT 1 from UMS_USER_CLAIMS where UserId = '0226a7d2-1d08-4348-b94c-20ae662fff19' AND ClaimValue = 'Permissions.Submenu.AuditLogs.ErrorLogs')
	INSERT INTO [dbo].[UMS_USER_CLAIMS] ([UserId],[ClaimType], [ClaimValue])
		VALUES ('0226a7d2-1d08-4348-b94c-20ae662fff19', 'Permission', 'Permissions.Submenu.AuditLogs.ErrorLogs')
GO
IF NOT Exists(SELECT 1 from UMS_USER_CLAIMS where UserId = '0226a7d2-1d08-4348-b94c-20ae662fff19' AND ClaimValue = 'Permissions.Submenu.AuditLogs.ProcessLogs')
	INSERT INTO [dbo].[UMS_USER_CLAIMS] ([UserId],[ClaimType], [ClaimValue])
		VALUES ('0226a7d2-1d08-4348-b94c-20ae662fff19', 'Permission', 'Permissions.Submenu.AuditLogs.ProcessLogs')
GO
IF NOT Exists(SELECT 1 from UMS_USER_CLAIMS where UserId = '0226a7d2-1d08-4348-b94c-20ae662fff19' AND ClaimValue = 'Permissions.Submenu.LDS.Documents.List')
	INSERT INTO [dbo].[UMS_USER_CLAIMS] ([UserId],[ClaimType], [ClaimValue])
		VALUES ('0226a7d2-1d08-4348-b94c-20ae662fff19', 'Permission', 'Permissions.Submenu.LDS.Documents.List')
GO
IF NOT Exists(SELECT 1 from UMS_USER_CLAIMS where UserId = '0226a7d2-1d08-4348-b94c-20ae662fff19' AND ClaimValue = 'Permissions.Submenu.LDS.Documents.List.Button.View')
	INSERT INTO [dbo].[UMS_USER_CLAIMS] ([UserId],[ClaimType], [ClaimValue])
		VALUES ('0226a7d2-1d08-4348-b94c-20ae662fff19', 'Permission', 'Permissions.Submenu.LDS.Documents.List.Button.View')
GO
IF NOT Exists(SELECT 1 from UMS_USER_CLAIMS where UserId = '0226a7d2-1d08-4348-b94c-20ae662fff19' AND ClaimValue = 'Permissions.LDS.Documents.Create')
	INSERT INTO [dbo].[UMS_USER_CLAIMS] ([UserId],[ClaimType], [ClaimValue])
		VALUES ('0226a7d2-1d08-4348-b94c-20ae662fff19', 'Permission', 'Permissions.LDS.Documents.Create')
GO
IF NOT Exists(SELECT 1 from UMS_USER_CLAIMS where UserId = '0226a7d2-1d08-4348-b94c-20ae662fff19' AND ClaimValue = 'Permissions.LDS.Documents.Edit')
	INSERT INTO [dbo].[UMS_USER_CLAIMS] ([UserId],[ClaimType], [ClaimValue])
		VALUES ('0226a7d2-1d08-4348-b94c-20ae662fff19', 'Permission', 'Permissions.LDS.Documents.Edit')
GO
IF NOT Exists(SELECT 1 from UMS_USER_CLAIMS where UserId = '0226a7d2-1d08-4348-b94c-20ae662fff19' AND ClaimValue = 'Permissions.LDS.Legislation.Add')
	INSERT INTO [dbo].[UMS_USER_CLAIMS] ([UserId],[ClaimType], [ClaimValue])
		VALUES ('0226a7d2-1d08-4348-b94c-20ae662fff19', 'Permission', 'Permissions.LDS.Legislation.Add')
GO
IF NOT Exists(SELECT 1 from UMS_USER_CLAIMS where UserId = '0226a7d2-1d08-4348-b94c-20ae662fff19' AND ClaimValue = 'Permissions.LDS.Documents.View')
	INSERT INTO [dbo].[UMS_USER_CLAIMS] ([UserId],[ClaimType], [ClaimValue])
		VALUES ('0226a7d2-1d08-4348-b94c-20ae662fff19', 'Permission', 'Permissions.LDS.Documents.View')
GO
IF NOT Exists(SELECT 1 from UMS_USER_CLAIMS where UserId = '0226a7d2-1d08-4348-b94c-20ae662fff19' AND ClaimValue = 'Permissions.LDS.Documents.DocumentApproveReject')
	INSERT INTO [dbo].[UMS_USER_CLAIMS] ([UserId],[ClaimType], [ClaimValue])
		VALUES ('0226a7d2-1d08-4348-b94c-20ae662fff19', 'Permission', 'Permissions.LDS.Documents.DocumentApproveReject')
GO


--DMS DATABASE SCRIPTS
IF((SELECT COUNT(*) FROM ATTACHMENT_TYPE WHERE AttachmentTypeId = 70) = 0)
INSERT INTO ATTACHMENT_TYPE VALUES(70, N'External Source Document', 'External Source Document', 2, 0, 0, NULL, 0)
GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LMS_BOOK_STATUS]') AND type in (N'U'))
INSERT INTO [dbo].[LMS_BOOK_STATUS] ([StatusId]   ,[Name] ,[Name_Ar])VALUES(16 ,'Save as Draft',N'حفظ كمسودة')
GO
-----------  START  Legal notification claims assigned to HOS , LAWYER AND SUPERVISORS of Consultation

IF NOT Exists(SELECT 1 from UMS_GROUP_CLAIMS where ClaimValue = 'Permissions.Comm.NeedMoreInfo.List' AND GroupId = 'E5E9AFCE-3123-48E5-A0CC-EAEC678AA179')
	INSERT INTO [dbo].[UMS_GROUP_CLAIMS]
		VALUES ('E5E9AFCE-3123-48E5-A0CC-EAEC678AA179','Permission','Permissions.Comm.NeedMoreInfo.List')
GO
IF NOT Exists(SELECT 1 from UMS_GROUP_CLAIMS where ClaimValue = 'Permissions.Comm.NeedMoreInfo.List' AND GroupId = '85D6848C-3BF1-403E-9CF6-B749D197ED15')
	INSERT INTO [dbo].[UMS_GROUP_CLAIMS]
		VALUES ('85D6848C-3BF1-403E-9CF6-B749D197ED15','Permission','Permissions.Comm.NeedMoreInfo.List')
GO
IF NOT Exists(SELECT 1 from UMS_GROUP_CLAIMS where ClaimValue = 'Permissions.Comm.NeedMoreInfo.List' AND GroupId = '93C1C1D4-73FE-4944-8F3E-8B9B983646F8')
	INSERT INTO [dbo].[UMS_GROUP_CLAIMS]
		VALUES ('93C1C1D4-73FE-4944-8F3E-8B9B983646F8','Permission','Permissions.Comm.NeedMoreInfo.List')
GO

---------------   END  Legal notification claims assigned to HOS , LAWYER AND SUPERVISORS of Consultation

--------- LMS Admin role update and role assigned to Library admin user

IF((SELECT COUNT(*) FROM UMS_ROLE WHERE id = 'abe81828-560a-4efa-8bf0-a5f02738bcf6') = 1)
UPDATE UMS_ROLE SET Name = 'LMSAdmin', NormalizedName = 'LMSAdmin' WHERE id = 'abe81828-560a-4efa-8bf0-a5f02738bcf6'
GO
-------------
IF((SELECT COUNT(*) FROM UMS_USER_ROLES WHERE UserId = '9779af5f-e7a4-4ebf-9af0-52beb251b4eb' and RoleId = 'abe81828-560a-4efa-8bf0-a5f02738bcf6') <= 0)
INSERT INTO UMS_USER_ROLES (UserId, RoleId) VALUES ('9779af5f-e7a4-4ebf-9af0-52beb251b4eb','abe81828-560a-4efa-8bf0-a5f02738bcf6')
GO



INSERT [dbo].[CMS_TEMPLATE_SECTION_PARAMETER] ([Id], [TemplateSectionId], [ParameterId]) VALUES (NewId(), N'5a0ebb36-c1bc-4aba-bb49-3062f7d6ebce', 13)
INSERT [dbo].[CMS_TEMPLATE_SECTION_PARAMETER] ([Id], [TemplateSectionId], [ParameterId]) VALUES (NewId(), N'cb1e9584-939b-4a24-8fd7-722b378502ac', 13)
INSERT [dbo].[CMS_TEMPLATE_SECTION_PARAMETER] ([Id], [TemplateSectionId], [ParameterId]) VALUES (NewId(), N'5a0ebb36-c1bc-4aba-bb49-3062f7d6ebce', 15)
INSERT [dbo].[CMS_TEMPLATE_SECTION_PARAMETER] ([Id], [TemplateSectionId], [ParameterId]) VALUES (NewId(), N'cb1e9584-939b-4a24-8fd7-722b378502ac', 14)
INSERT [dbo].[CMS_TEMPLATE_SECTION_PARAMETER] ([Id], [TemplateSectionId], [ParameterId]) VALUES (NewId(), N'cb1e9584-939b-4a24-8fd7-722b378502ac', 44)
INSERT [dbo].[CMS_TEMPLATE_SECTION_PARAMETER] ([Id], [TemplateSectionId], [ParameterId]) VALUES (NewId(), N'5a0ebb36-c1bc-4aba-bb49-3062f7d6ebce', 19)
INSERT [dbo].[CMS_TEMPLATE_SECTION_PARAMETER] ([Id], [TemplateSectionId], [ParameterId]) VALUES (NewId(), N'cb1e9584-939b-4a24-8fd7-722b378502ac', 45)
INSERT [dbo].[CMS_TEMPLATE_SECTION_PARAMETER] ([Id], [TemplateSectionId], [ParameterId]) VALUES (NewId(), N'5a0ebb36-c1bc-4aba-bb49-3062f7d6ebce', 21)
INSERT [dbo].[CMS_TEMPLATE_SECTION_PARAMETER] ([Id], [TemplateSectionId], [ParameterId]) VALUES (NewId(), N'49cadf04-e310-409f-b524-db58059c1b09', 22)
INSERT [dbo].[CMS_TEMPLATE_SECTION_PARAMETER] ([Id], [TemplateSectionId], [ParameterId]) VALUES (NewId(), N'9674d11f-ec34-4dbc-9f78-f7fd041faf15', 34)
INSERT [dbo].[CMS_TEMPLATE_SECTION_PARAMETER] ([Id], [TemplateSectionId], [ParameterId]) VALUES (NewId(), N'c7b4bf63-483f-406e-afe9-f214481b93d3', 32)
INSERT [dbo].[CMS_TEMPLATE_SECTION_PARAMETER] ([Id], [TemplateSectionId], [ParameterId]) VALUES (NewId(), N'5a0ebb36-c1bc-4aba-bb49-3062f7d6ebce', 17)
INSERT [dbo].[CMS_TEMPLATE_SECTION_PARAMETER] ([Id], [TemplateSectionId], [ParameterId]) VALUES (NewId(), N'5a0ebb36-c1bc-4aba-bb49-3062f7d6ebce', 16)
INSERT [dbo].[CMS_TEMPLATE_SECTION_PARAMETER] ([Id], [TemplateSectionId], [ParameterId]) VALUES (NewId(), N'7a4c663d-95e6-4fad-9298-efc43194df86', 40)
INSERT [dbo].[CMS_TEMPLATE_SECTION_PARAMETER] ([Id], [TemplateSectionId], [ParameterId]) VALUES (NewId(), N'5a0ebb36-c1bc-4aba-bb49-3062f7d6ebce', 14)
INSERT [dbo].[CMS_TEMPLATE_SECTION_PARAMETER] ([Id], [TemplateSectionId], [ParameterId]) VALUES (NewId(), N'5a0ebb36-c1bc-4aba-bb49-3062f7d6ebce', 31)
INSERT [dbo].[CMS_TEMPLATE_SECTION_PARAMETER] ([Id], [TemplateSectionId], [ParameterId]) VALUES (NewId(), N'5a0ebb36-c1bc-4aba-bb49-3062f7d6ebce', 18)
INSERT [dbo].[CMS_TEMPLATE_SECTION_PARAMETER] ([Id], [TemplateSectionId], [ParameterId]) VALUES (NewId(), N'5a0ebb36-c1bc-4aba-bb49-3062f7d6ebce', 20)
INSERT [dbo].[CMS_TEMPLATE_SECTION_PARAMETER] ([Id], [TemplateSectionId], [ParameterId]) VALUES (NewId(), N'9674d11f-ec34-4dbc-9f78-f7fd041faf15', 22)
INSERT [dbo].[CMS_TEMPLATE_SECTION_PARAMETER] ([Id], [TemplateSectionId], [ParameterId]) VALUES (NewId(), N'5a0ebb36-c1bc-4aba-bb49-3062f7d6ebce', 24)
INSERT [dbo].[CMS_TEMPLATE_SECTION_PARAMETER] ([Id], [TemplateSectionId], [ParameterId]) VALUES (NewId(), N'7a4c663d-95e6-4fad-9298-efc43194df86', 39)
INSERT [dbo].[CMS_TEMPLATE_SECTION_PARAMETER] ([Id], [TemplateSectionId], [ParameterId]) VALUES (NewId(), N'7a4c663d-95e6-4fad-9298-efc43194df86', 41)
INSERT [dbo].[CMS_TEMPLATE_SECTION_PARAMETER] ([Id], [TemplateSectionId], [ParameterId]) VALUES (NewId(), N'e9cb4eb9-0479-4144-8fc9-187bce522660', 42)
INSERT [dbo].[CMS_TEMPLATE_SECTION_PARAMETER] ([Id], [TemplateSectionId], [ParameterId]) VALUES (NewId(), N'e9cb4eb9-0479-4144-8fc9-187bce522660', 44)
INSERT [dbo].[CMS_TEMPLATE_SECTION_PARAMETER] ([Id], [TemplateSectionId], [ParameterId]) VALUES (NewId(), N'0fcef313-7c2e-4510-a3e5-fa98946d01ea', 40)
INSERT [dbo].[CMS_TEMPLATE_SECTION_PARAMETER] ([Id], [TemplateSectionId], [ParameterId]) VALUES (NewId(), N'0fcef313-7c2e-4510-a3e5-fa98946d01ea', 47)
INSERT [dbo].[CMS_TEMPLATE_SECTION_PARAMETER] ([Id], [TemplateSectionId], [ParameterId]) VALUES (NewId(), N'9cc5d917-d921-438f-96ab-32918f2fe41b', 52)
INSERT [dbo].[CMS_TEMPLATE_SECTION_PARAMETER] ([Id], [TemplateSectionId], [ParameterId]) VALUES (NewId(), N'd44f04f4-f347-4b66-b63f-7cdf5793b58c', 53)
INSERT [dbo].[CMS_TEMPLATE_SECTION_PARAMETER] ([Id], [TemplateSectionId], [ParameterId]) VALUES (NewId(), N'e9cb4eb9-0479-4144-8fc9-187bce522660', 43)
INSERT [dbo].[CMS_TEMPLATE_SECTION_PARAMETER] ([Id], [TemplateSectionId], [ParameterId]) VALUES (NewId(), N'e9cb4eb9-0479-4144-8fc9-187bce522660', 45)
INSERT [dbo].[CMS_TEMPLATE_SECTION_PARAMETER] ([Id], [TemplateSectionId], [ParameterId]) VALUES (NewId(), N'0fcef313-7c2e-4510-a3e5-fa98946d01ea', 46)
INSERT [dbo].[CMS_TEMPLATE_SECTION_PARAMETER] ([Id], [TemplateSectionId], [ParameterId]) VALUES (NewId(), N'9022f375-6c6c-4f57-bf91-28bdfb1dc81a', 54)
INSERT [dbo].[CMS_TEMPLATE_SECTION_PARAMETER] ([Id], [TemplateSectionId], [ParameterId]) VALUES (NewId(), N'a14e092f-1a27-4861-83f1-f753be47fd0a', 55)
GO

IF NOT Exists(SELECT 1 from UMS_GROUP_CLAIMS where GroupId = '85D6848C-3BF1-403E-9CF6-B749D197ED15' and ClaimValue = 'Permissions.COMS.ConsultationFile.RequestMoreInfo')
	INSERT INTO [dbo].[UMS_GROUP_CLAIMS] ([GroupId],[ClaimType],[ClaimValue])
		VALUES ('85D6848C-3BF1-403E-9CF6-B749D197ED15', 'Permission', 'Permissions.COMS.ConsultationFile.RequestMoreInfo')
GO
---------------------------WORKFLOW_SUBMODULE-----------------

INSERT [dbo].[WORKFLOW_SUBMODULE] ([Id], [Name_En], [Name_Ar],[ModuleId],[IsActive]) VALUES (1,N'Administrative', N'Administrative',5,1)
INSERT [dbo].[WORKFLOW_SUBMODULE] ([Id], [Name_En], [Name_Ar],[ModuleId],[IsActive]) VALUES (2,N'Civil/Commercial', N'Civil/Commercial',5,1)

INSERT [dbo].[WORKFLOW_SUBMODULE] ([Id], [Name_En], [Name_Ar],[ModuleId],[IsActive]) VALUES (3,N'Contracts', N'Contracts',10,1)
INSERT [dbo].[WORKFLOW_SUBMODULE] ([Id], [Name_En], [Name_Ar],[ModuleId],[IsActive]) VALUES (4,N'Administrative Complaints', N'Administrative Complaints',10,1)
INSERT [dbo].[WORKFLOW_SUBMODULE] ([Id], [Name_En], [Name_Ar],[ModuleId],[IsActive]) VALUES (5,N'Legal Advice', N'Legal Advice',10,1)
INSERT [dbo].[WORKFLOW_SUBMODULE] ([Id], [Name_En], [Name_Ar],[ModuleId],[IsActive]) VALUES (6,N'Legislations', N'Legislations',10,1)
INSERT [dbo].[WORKFLOW_SUBMODULE] ([Id], [Name_En], [Name_Ar],[ModuleId],[IsActive]) VALUES (7,N'International Arbitration', N'International Arbitration',10,1)

INSERT [dbo].[WORKFLOW_SUBMODULE] ([Id], [Name_En], [Name_Ar],[ModuleId],[IsActive]) VALUES (8,N'LMS', N'LMS',3,1)
INSERT [dbo].[WORKFLOW_SUBMODULE] ([Id], [Name_En], [Name_Ar],[ModuleId],[IsActive]) VALUES (9,N'Legislations', N'Legislations',3,1)
INSERT [dbo].[WORKFLOW_SUBMODULE] ([Id], [Name_En], [Name_Ar],[ModuleId],[IsActive]) VALUES (10,N'Legal Principles', N'Legal Principles',3,1)
-----------------------------------------------

----- 9-4-2023 -----
insert into CMS_TEMPLATE_PARAMETER
( Name, PKey, Mandatory, IsAutoPopulated, moduleID, isActive)
select Name, PKey, Mandatory, IsAutoPopulated, 5 as moduleID, 1 as isActive from PARAMETER
where pKey like 'CmsTemp%';

insert into CMS_TEMPLATE_PARAMETER
( Name, PKey, Mandatory, IsAutoPopulated, moduleID, isActive)
select Name, PKey, Mandatory, IsAutoPopulated, 10 as moduleID, 1 as isActive from PARAMETER
where pKey like 'ComsTemp%';

delete from PARAMETER
where pKey like 'CmsTemp%' or pKey like 'ComsTemp%';

delete from COMS_DRAFTED_TEMPLATE_SECTION_PARAMETER

----- 9-4-2023 -----

--------------------------------Template List Claims 

IF NOT Exists(SELECT 1 from UMS_GROUP_CLAIMS where ClaimValue = 'Permissions.Submenu.Document.TemplateList' AND GroupId = '4A6C0AA1-D091-4C01-B6F3-4CDD3780A06A')
	INSERT INTO [dbo].[UMS_GROUP_CLAIMS]
		VALUES ('4A6C0AA1-D091-4C01-B6F3-4CDD3780A06A','Permission','Permissions.Submenu.Document.TemplateList')
GO
IF NOT Exists(SELECT 1 from UMS_GROUP_CLAIMS where ClaimValue = 'Permissions.Submenu.Document.TemplateList' AND GroupId = '49744B64-2399-4097-94FD-326D6FCE2626')
	INSERT INTO [dbo].[UMS_GROUP_CLAIMS]
		VALUES ('49744B64-2399-4097-94FD-326D6FCE2626','Permission','Permissions.Submenu.Document.TemplateList')
GO
IF NOT Exists(SELECT 1 from UMS_GROUP_CLAIMS where ClaimValue = 'Permissions.Submenu.Document.TemplateList' AND GroupId = '8EA8BEA4-B490-48E2-9776-8BA2B3EED060')
	INSERT INTO [dbo].[UMS_GROUP_CLAIMS]
		VALUES ('8EA8BEA4-B490-48E2-9776-8BA2B3EED060','Permission','Permissions.Submenu.Document.TemplateList')
GO
IF NOT Exists(SELECT 1 from UMS_GROUP_CLAIMS where ClaimValue = 'Permissions.Submenu.Document.TemplateList' AND GroupId = '93C1C1D4-73FE-4944-8F3E-8B9B983646F8')
	INSERT INTO [dbo].[UMS_GROUP_CLAIMS]
		VALUES ('93C1C1D4-73FE-4944-8F3E-8B9B983646F8','Permission','Permissions.Submenu.Document.TemplateList')
GO
IF NOT Exists(SELECT 1 from UMS_GROUP_CLAIMS where ClaimValue = 'Permissions.Submenu.Document.TemplateList' AND GroupId = '9D33079E-1183-4CCE-BF5F-9CEF1605D3AB')
	INSERT INTO [dbo].[UMS_GROUP_CLAIMS]
		VALUES ('9D33079E-1183-4CCE-BF5F-9CEF1605D3AB','Permission','Permissions.Submenu.Document.TemplateList')
GO
IF NOT Exists(SELECT 1 from UMS_GROUP_CLAIMS where ClaimValue = 'Permissions.Submenu.Document.TemplateList' AND GroupId = '85D6848C-3BF1-403E-9CF6-B749D197ED15')
	INSERT INTO [dbo].[UMS_GROUP_CLAIMS]
		VALUES ('85D6848C-3BF1-403E-9CF6-B749D197ED15','Permission','Permissions.Submenu.Document.TemplateList')
GO
IF NOT Exists(SELECT 1 from UMS_GROUP_CLAIMS where ClaimValue = 'Permissions.Submenu.Document.TemplateList' AND GroupId = '071C4BA0-CC3F-43DC-B2CB-6FC2BEA6D831')
	INSERT INTO [dbo].[UMS_GROUP_CLAIMS]
		VALUES ('071C4BA0-CC3F-43DC-B2CB-6FC2BEA6D831','Permission','Permissions.Submenu.Document.TemplateList')
GO
IF NOT Exists(SELECT 1 from UMS_GROUP_CLAIMS where ClaimValue = 'Permissions.Submenu.Document.TemplateList' AND GroupId = 'E5E9AFCE-3123-48E5-A0CC-EAEC678AA179')
	INSERT INTO [dbo].[UMS_GROUP_CLAIMS]
		VALUES ('E5E9AFCE-3123-48E5-A0CC-EAEC678AA179','Permission','Permissions.Submenu.Document.TemplateList')
GO
--------------------WORKFLOW_SUBMODULE_TRIGGER---------
INSERT [dbo].WORKFLOW_SUBMODULE_TRIGGER ([Id],[WorkflowSubModuleId],[ModuleTriggerId]) VALUES (1,9,1)
INSERT [dbo].WORKFLOW_SUBMODULE_TRIGGER ([Id],[WorkflowSubModuleId],[ModuleTriggerId]) VALUES (2,10,3)
INSERT [dbo].WORKFLOW_SUBMODULE_TRIGGER ([Id],[WorkflowSubModuleId],[ModuleTriggerId]) VALUES (3,1,4)
INSERT [dbo].WORKFLOW_SUBMODULE_TRIGGER ([Id],[WorkflowSubModuleId],[ModuleTriggerId]) VALUES (4,2,4)
INSERT [dbo].WORKFLOW_SUBMODULE_TRIGGER ([Id],[WorkflowSubModuleId],[ModuleTriggerId]) VALUES (5,3,5)
INSERT [dbo].WORKFLOW_SUBMODULE_TRIGGER ([Id],[WorkflowSubModuleId],[ModuleTriggerId]) VALUES (6,4,5)
INSERT [dbo].WORKFLOW_SUBMODULE_TRIGGER ([Id],[WorkflowSubModuleId],[ModuleTriggerId]) VALUES (7,5,5)
INSERT [dbo].WORKFLOW_SUBMODULE_TRIGGER ([Id],[WorkflowSubModuleId],[ModuleTriggerId]) VALUES (8,6,5)
INSERT [dbo].WORKFLOW_SUBMODULE_TRIGGER ([Id],[WorkflowSubModuleId],[ModuleTriggerId]) VALUES (9,7,5)
INSERT [dbo].WORKFLOW_SUBMODULE_TRIGGER ([Id],[WorkflowSubModuleId],[ModuleTriggerId]) VALUES (10,9,6)
INSERT [dbo].WORKFLOW_SUBMODULE_TRIGGER ([Id],[WorkflowSubModuleId],[ModuleTriggerId]) VALUES (11,10,6)
INSERT [dbo].WORKFLOW_SUBMODULE_TRIGGER ([Id],[WorkflowSubModuleId],[ModuleTriggerId]) VALUES (12,1,6)
INSERT [dbo].WORKFLOW_SUBMODULE_TRIGGER ([Id],[WorkflowSubModuleId],[ModuleTriggerId]) VALUES (13,2,6)
------------------------


---- 07-09-2023

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LMS_LITERATURE_BORROW_RETURN_APPROVAL_STATUS]') AND type in (N'U'))
	INSERT [dbo].[LMS_LITERATURE_BORROW_RETURN_APPROVAL_STATUS] ([Id], [NameEn], [NameAr]) VALUES (1, N'Default', N'تقصير')
	INSERT [dbo].[LMS_LITERATURE_BORROW_RETURN_APPROVAL_STATUS] ([Id], [NameEn], [NameAr]) VALUES (2, N'Returned', N'عاد')
	INSERT [dbo].[LMS_LITERATURE_BORROW_RETURN_APPROVAL_STATUS] ([Id], [NameEn], [NameAr]) VALUES (4, N'Rejected', N'رفض')
	INSERT [dbo].[LMS_LITERATURE_BORROW_RETURN_APPROVAL_STATUS] ([Id], [NameEn], [NameAr]) VALUES (8, N'Pending For Approval', N'في انتظار لموافقة')
GO

----- 19-10-2023

TRUNCATE TABLE CNT_CONTACT_TYPE_LKP

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CNT_CONTACT_TYPE_LKP]') AND type in (N'U'))
	INSERT [dbo].[CNT_CONTACT_TYPE_LKP] ([TypeId], [NameEn], [NameAr]) VALUES (1, 'Internal', N'داخلي')
	INSERT [dbo].[CNT_CONTACT_TYPE_LKP] ([TypeId], [NameEn], [NameAr]) VALUES (2, 'External', N'خارجي')
GO

GO
----------
begin tran
update tTranslation set Value_Ar=N'File Minimum Size (Bytes)',Value_En='File Minimum Size (Bytes)'
where TranslationId=3029
commit
--------------
begin tran
update tTranslation set Value_Ar=N'File Maximum Size (Bytes)',Value_En='File Maximum Size (Bytes)'
where TranslationId=3030
commit
GO
----------
begin tran
update tTranslation set Value_Ar=N'File Minimum Size (Bytes)',Value_En='File Minimum Size (Bytes)'
where TranslationId=3029
commit
--------------
begin tran
update tTranslation set Value_Ar=N'File Maximum Size (Bytes)',Value_En='File Maximum Size (Bytes)'
where TranslationId=3030
commit


UPDATE CMS_HEARING_STATUS_G2G_LKP SET NameEn = 'Hearing Cancelled', NameAr = N'تم إلغاء الجلسة' where Id = 2


------------------------------------------------------------------------------------------------------------------

SET IDENTITY_INSERT CMS_REGISTERED_CASE_EVENT_G2G_LKP ON
INSERT INTO CMS_REGISTERED_CASE_EVENT_G2G_LKP (Id, Name_En, Name_Ar) VALUES (8,'Hearing Scheduled','Hearing Scheduled')
INSERT INTO CMS_REGISTERED_CASE_EVENT_G2G_LKP (Id, Name_En, Name_Ar) VALUES (9,'Hearing Attended','Hearing Attended')
INSERT INTO CMS_REGISTERED_CASE_EVENT_G2G_LKP (Id, Name_En, Name_Ar) VALUES (10,'Hearing Cancelled','Hearing Cancelled')
INSERT INTO CMS_REGISTERED_CASE_EVENT_G2G_LKP (Id, Name_En, Name_Ar) VALUES (11,'Outcome Added','Outcome Added')
INSERT INTO CMS_REGISTERED_CASE_EVENT_G2G_LKP (Id, Name_En, Name_Ar) VALUES (12,'Judgement Added','Judgement Added')
INSERT INTO CMS_REGISTERED_CASE_EVENT_G2G_LKP (Id, Name_En, Name_Ar) VALUES (13,'Execution Request Sent','Execution Request Sent')
INSERT INTO CMS_REGISTERED_CASE_EVENT_G2G_LKP (Id, Name_En, Name_Ar) VALUES (14,'Execution Added','Execution Added')
INSERT INTO CMS_REGISTERED_CASE_EVENT_G2G_LKP (Id, Name_En, Name_Ar) VALUES (15,'Final Judgement Added','Final Judgement Added')
SET IDENTITY_INSERT CMS_REGISTERED_CASE_EVENT_G2G_LKP OFF


------------------------------------------------------------------------------------------------------------------


INSERT INTO CMS_JUDGEMENT_STATUS_G2G_LKP VALUES ('Against the state (all applications were rejected)',N'ضد الدولة (تم رفض كل الطلبات)',1,'fatwaadmin@gmail.com','2023-08-05 23:49:23.627',NULL,NULL,NULL,NULL,0)
INSERT INTO CMS_JUDGEMENT_STATUS_G2G_LKP VALUES ('In favor of the state',N'لصالح الدولة',1,'fatwaadmin@gmail.com','2023-08-05 23:49:23.627',NULL,NULL,NULL,NULL,0)
INSERT INTO CMS_JUDGEMENT_STATUS_G2G_LKP VALUES ('By virtue of part of the requests',N'بحكم جزء من الطلبات',1,'fatwaadmin@gmail.com','2023-08-05 23:49:23.627',NULL,NULL,NULL,NULL,0)
INSERT INTO CMS_JUDGEMENT_STATUS_G2G_LKP VALUES ('Judgment in confrontation',N'بحكم في المواجهة',1,'fatwaadmin@gmail.com','2023-08-05 23:49:23.627',NULL,NULL,NULL,NULL,0)
INSERT INTO CMS_JUDGEMENT_STATUS_G2G_LKP VALUES ('By virtue of lack of jurisdiction',N'بحكم عدم الاختصاص ولائي',1,'fatwaadmin@gmail.com','2023-08-05 23:49:23.627',NULL,NULL,NULL,NULL,0)
INSERT INTO CMS_JUDGEMENT_STATUS_G2G_LKP VALUES ('By virtue of the lack of special jurisdiction',N'بحكم عدم الاختصاص نوعي',1,'fatwaadmin@gmail.com','2023-08-05 23:49:23.627',NULL,NULL,NULL,NULL,0)
INSERT INTO CMS_JUDGEMENT_STATUS_G2G_LKP VALUES ('By virtue of my lack of jurisdiction',N'بحكم عدم الاختصاص قيمي',1,'fatwaadmin@gmail.com','2023-08-05 23:49:23.627',NULL,NULL,NULL,NULL,0)


------------------------------------------------------------------------------------------------------------------

 INSERT INTO UMS_GROUP_CLAIMS VALUES ((SELECT GroupId FROM UMS_GROUP WHERE Name_En = LOWER('hos')), 'Permission', 'Permissions.Submenu.CMS.LawyerChamberAssignment')
 -------------
 insert into tTranslation ([Url],[TranslationKey],[TranslationType],[Value_Ar],[Value_En],[ReferenceScreen],[CreateDate],[UpdateDate])
values ('Script','TransferHistory_Details',0,N'Transfer History Details',N'Transfer History Details','Case Management',GetDate(),GetDate())

insert into tTranslation ([Url],[TranslationKey],[TranslationType],[Value_Ar],[Value_En],[ReferenceScreen],[CreateDate],[UpdateDate])
values ('Script','Transfer_Date',0,N'Transfer Date',N'Transfer Date','Case Management',GetDate(),GetDate())

insert into tTranslation ([Url],[TranslationKey],[TranslationType],[Value_Ar],[Value_En],[ReferenceScreen],[CreateDate],[UpdateDate])
values ('Script','Chamber_Name_From',0,N'Chamber Name From',N'Chamber Name From','Case Management',GetDate(),GetDate())

insert into tTranslation ([Url],[TranslationKey],[TranslationType],[Value_Ar],[Value_En],[ReferenceScreen],[CreateDate],[UpdateDate])
values ('Script','Chamber_Name_To',0,N'Chamber Name To',N'Chamber Name To','Case Management',GetDate(),GetDate())

insert into tTranslation ([Url],[TranslationKey],[TranslationType],[Value_Ar],[Value_En],[ReferenceScreen],[CreateDate],[UpdateDate])
values ('Script','Chamber_Number_From',0,N'Chamber Number From',N'Chamber Number From','Case Management',GetDate(),GetDate())

insert into tTranslation ([Url],[TranslationKey],[TranslationType],[Value_Ar],[Value_En],[ReferenceScreen],[CreateDate],[UpdateDate])
values ('Script','Chamber_Number_To',0,N'Chamber Number To',N'Chamber Number To','Case Management',GetDate(),GetDate())

insert into tTranslation ([Url],[TranslationKey],[TranslationType],[Value_Ar],[Value_En],[ReferenceScreen],[CreateDate],[UpdateDate])
values ('Script','Transfered_By',0,N'Transfered By',N'Transfered By','Case Management',GetDate(),GetDate())

insert into tTranslation ([Url],[TranslationKey],[TranslationType],[Value_Ar],[Value_En],[ReferenceScreen],[CreateDate],[UpdateDate])
values ('Script','Case_History_Transfer_History',0,N'Case History & Transfer History',N'Case History & Transfer History','Case Management',GetDate(),GetDate())
--------------
GO
--------------------------------
SET IDENTITY_INSERT MODULE_TRIGGER ON
INSERT [dbo].MODULE_TRIGGER ([ModuleTriggerId],[Name]) VALUES (6,N'User Review DMS Document')
----------------------------
SET IDENTITY_INSERT MODULE_ACTIVITY ON
INSERT [dbo].MODULE_ACTIVITY ([ActivityId],[Name],[Class],[Method],[CategoryId],[AKey]) VALUES (12,N'Review DMS Document','','Lps_ReviewDMSDocument',2,'LpsDMSReview')

SET IDENTITY_INSERT MODULE_ACTIVITY ON
INSERT [dbo].MODULE_ACTIVITY ([ActivityId],[Name],[Class],[Method],[CategoryId],[AKey]) VALUES (17,N'Publish DMS Document','WorkflowImplementationService','Lps_PublishDMSDocument',2,'LpsDMSPublish')
--------------------------
INSERT [dbo].WORKFLOW_SUBMODULE_ACTIVITY ([WorkflowSubModuleId],[SubModuleActivityId]) VALUES (9,12)
INSERT [dbo].WORKFLOW_SUBMODULE_ACTIVITY ([WorkflowSubModuleId],[SubModuleActivityId]) VALUES (10,12)
INSERT [dbo].WORKFLOW_SUBMODULE_ACTIVITY ([WorkflowSubModuleId],[SubModuleActivityId]) VALUES (1,12)
INSERT [dbo].WORKFLOW_SUBMODULE_ACTIVITY ([WorkflowSubModuleId],[SubModuleActivityId]) VALUES (2,12)
INSERT [dbo].WORKFLOW_SUBMODULE_ACTIVITY ([WorkflowSubModuleId],[SubModuleActivityId]) VALUES (9,17)
INSERT [dbo].WORKFLOW_SUBMODULE_ACTIVITY ([WorkflowSubModuleId],[SubModuleActivityId]) VALUES (10,17)
INSERT [dbo].WORKFLOW_SUBMODULE_ACTIVITY ([WorkflowSubModuleId],[SubModuleActivityId]) VALUES (1,17)
INSERT [dbo].WORKFLOW_SUBMODULE_ACTIVITY ([WorkflowSubModuleId],[SubModuleActivityId]) VALUES (2,17)
----------------------------
INSERT [dbo].[PARAMETER] ([Name], [PKey],[Mandatory],[IsAutoPopulated]) VALUES (N'User', N'LpsDocumentReview_User',0,0)
INSERT [dbo].[PARAMETER] ([Name], [PKey],[Mandatory],[IsAutoPopulated]) VALUES (N'User Role', N'LpsDocumentReview_UserRole',0,0)
SET IDENTITY_INSERT [PARAMETER] ON
INSERT [dbo].[PARAMETER] ([ParameterId],[Name], [PKey],[Mandatory],[IsAutoPopulated]) VALUES (30,N'User', N'LpsDocumentPublish_User',0,0)
SET IDENTITY_INSERT [PARAMETER] ON
INSERT [dbo].[PARAMETER] ([ParameterId],[Name], [PKey],[Mandatory],[IsAutoPopulated]) VALUES (31,N'User Role', N'LpsDocumentPublish_UserRole',0,0)
---------------
INSERT [dbo].[MODULE_ACTIVITY_PARAMETERS] ([ParameterId],[ActivityId]) VALUES (56,12)
INSERT [dbo].[MODULE_ACTIVITY_PARAMETERS] ([ParameterId],[ActivityId]) VALUES (57,12)
INSERT [dbo].[MODULE_ACTIVITY_PARAMETERS] ([ParameterId],[ActivityId]) VALUES (30,17)
INSERT [dbo].[MODULE_ACTIVITY_PARAMETERS] ([ParameterId],[ActivityId]) VALUES (31,17)
------------------
begin tran
update MODULE_CONDITION set Name='If Legislation Document Status is Draft'
where ModuleConditionId=1
begin tran
update MODULE_CONDITION set Name='If Legislation Document Status is InReview'
where ModuleConditionId=2	
begin tran
update MODULE_CONDITION set Name='If Legislation Document Status is Approve'
where ModuleConditionId=4		
begin tran
update MODULE_CONDITION set Name='If Legislation Document Status is Reject'
where ModuleConditionId=8	
begin tran
update MODULE_CONDITION set Name='If Legislation Document Status is NeedToModify'
where ModuleConditionId=16	
begin tran
update MODULE_CONDITION set Name='If Legislation Document Status is SendaComment'
where ModuleConditionId=32	
begin tran
update MODULE_CONDITION set Name='If Legislation Document Status is Publish'
where ModuleConditionId=64	
begin tran
update MODULE_CONDITION set Name='If Legislation Document Status is Unpublish'
where ModuleConditionId=128
begin tran
update MODULE_CONDITION set MKey=N'LpsDmsStatusPublish'
where ModuleConditionId=153

begin tran
update MODULE_CONDITION set Name='If Document Status is Reject', MKey=N'LpsDmsStatusReject'
where ModuleConditionId=151

begin tran
update MODULE_CONDITION set Name='If Document Status is Approve', MKey=N'LpsDmsStatusApprove'
where ModuleConditionId=152

begin tran
update MODULE_CONDITION set MKey=N'LpsDmsStatusPublish'
where ModuleConditionId=153
------------------------------
INSERT [dbo].WORKFLOW_SUBMODULE_CONDITION ([WorkflowSubModuleId],[SubModuleConditionId]) VALUES (9,149)
INSERT [dbo].WORKFLOW_SUBMODULE_CONDITION ([WorkflowSubModuleId],[SubModuleConditionId]) VALUES (9,150)
INSERT [dbo].WORKFLOW_SUBMODULE_CONDITION ([WorkflowSubModuleId],[SubModuleConditionId]) VALUES (9,151)
INSERT [dbo].WORKFLOW_SUBMODULE_CONDITION ([WorkflowSubModuleId],[SubModuleConditionId]) VALUES (9,152)
INSERT [dbo].WORKFLOW_SUBMODULE_CONDITION ([WorkflowSubModuleId],[SubModuleConditionId]) VALUES (9,153)
---
INSERT [dbo].WORKFLOW_SUBMODULE_CONDITION ([WorkflowSubModuleId],[SubModuleConditionId]) VALUES (10,149)
INSERT [dbo].WORKFLOW_SUBMODULE_CONDITION ([WorkflowSubModuleId],[SubModuleConditionId]) VALUES (10,150)
INSERT [dbo].WORKFLOW_SUBMODULE_CONDITION ([WorkflowSubModuleId],[SubModuleConditionId]) VALUES (10,151)
INSERT [dbo].WORKFLOW_SUBMODULE_CONDITION ([WorkflowSubModuleId],[SubModuleConditionId]) VALUES (10,152)
INSERT [dbo].WORKFLOW_SUBMODULE_CONDITION ([WorkflowSubModuleId],[SubModuleConditionId]) VALUES (10,153)
---
INSERT [dbo].WORKFLOW_SUBMODULE_CONDITION ([WorkflowSubModuleId],[SubModuleConditionId]) VALUES (1,149)
INSERT [dbo].WORKFLOW_SUBMODULE_CONDITION ([WorkflowSubModuleId],[SubModuleConditionId]) VALUES (1,150)
INSERT [dbo].WORKFLOW_SUBMODULE_CONDITION ([WorkflowSubModuleId],[SubModuleConditionId]) VALUES (1,151)
INSERT [dbo].WORKFLOW_SUBMODULE_CONDITION ([WorkflowSubModuleId],[SubModuleConditionId]) VALUES (1,152)
INSERT [dbo].WORKFLOW_SUBMODULE_CONDITION ([WorkflowSubModuleId],[SubModuleConditionId]) VALUES (1,153)
---
INSERT [dbo].WORKFLOW_SUBMODULE_CONDITION ([WorkflowSubModuleId],[SubModuleConditionId]) VALUES (2,149)
INSERT [dbo].WORKFLOW_SUBMODULE_CONDITION ([WorkflowSubModuleId],[SubModuleConditionId]) VALUES (2,150)
INSERT [dbo].WORKFLOW_SUBMODULE_CONDITION ([WorkflowSubModuleId],[SubModuleConditionId]) VALUES (2,151)
INSERT [dbo].WORKFLOW_SUBMODULE_CONDITION ([WorkflowSubModuleId],[SubModuleConditionId]) VALUES (2,152)
INSERT [dbo].WORKFLOW_SUBMODULE_CONDITION ([WorkflowSubModuleId],[SubModuleConditionId]) VALUES (2,153)
----------------------
INSERT INTO [dbo].[UMS_USER_CLAIMS] (UserId , ClaimType , ClaimValue) VALUES ('e75cba83-d6cd-4117-887f-eb6421082d42' , 'Permission' ,'Permissions.CMS.DraftDocument.View')
INSERT INTO [dbo].[UMS_USER_CLAIMS] (UserId , ClaimType , ClaimValue) VALUES ('c604e117-9cca-4e88-9b99-f59faef3f509' , 'Permission' ,'Permissions.CMS.DraftDocument.View')
-------------
IF((SELECT COUNT(*) FROM CMS_DRAFT_DOCUMENT_STATUS WHERE Id = '1024') <= 0)
INSERT [DBO].[CMS_DRAFT_DOCUMENT_STATUS]([Id] , [NameEn] ,[NameAr]) VALUES (1024 , 'ApproveByGS' , N'ApproveByGS') 
IF((SELECT COUNT(*) FROM CMS_DRAFT_DOCUMENT_STATUS WHERE Id = '2048') <= 0)
INSERT [DBO].[CMS_DRAFT_DOCUMENT_STATUS]([Id] , [NameEn] ,[NameAr]) VALUES (2048 , 'RejectedByGS' , N'RejectedByGS') 
IF((SELECT COUNT(*) FROM CMS_DRAFT_DOCUMENT_STATUS WHERE Id = '4096') <= 0)
INSERT [DBO].[CMS_DRAFT_DOCUMENT_STATUS]([Id] , [NameEn] ,[NameAr]) VALUES (4096 , 'ApprovedByPOO' , N'ApprovedByPOO') 
IF((SELECT COUNT(*) FROM CMS_DRAFT_DOCUMENT_STATUS WHERE Id = '8196') <= 0)
INSERT [DBO].[CMS_DRAFT_DOCUMENT_STATUS]([Id] , [NameEn] ,[NameAr]) VALUES (8196 , 'RejectedByPOO' , N'RejectedByPOO') 
IF((SELECT COUNT(*) FROM CMS_DRAFT_DOCUMENT_STATUS WHERE Id = '16384') <= 0)
INSERT [DBO].[CMS_DRAFT_DOCUMENT_STATUS]([Id] , [NameEn] ,[NameAr]) VALUES (16384 , 'ApproveByLawyer' , N'ApproveByLawyer') 
IF((SELECT COUNT(*) FROM CMS_DRAFT_DOCUMENT_STATUS WHERE Id = '32768') <= 0)
INSERT [DBO].[CMS_DRAFT_DOCUMENT_STATUS]([Id] , [NameEn] ,[NameAr]) VALUES (32768 , 'RejectedByLawyer' , N'RejectedByLawyer') 
------------------
IF((SELECT COUNT(*) FROM MODULE_CONDITION WHERE ModuleConditionId = '154') <= 0)
INSERT [dbo].[MODULE_CONDITION] ([ModuleConditionId], [Name], [MKey], [ValueToCompare]) VALUES (154, N'If Draft Status is ApproveByGS', N'CmsDraftStatusApproveByGS', 1024)
IF((SELECT COUNT(*) FROM MODULE_CONDITION WHERE ModuleConditionId = '155') <= 0)
INSERT [dbo].[MODULE_CONDITION] ([ModuleConditionId], [Name], [MKey], [ValueToCompare]) VALUES (155, N'If Draft Status is RejectedByGS', N'CmsDraftStatusRejectedByGS', 2048)
IF((SELECT COUNT(*) FROM MODULE_CONDITION WHERE ModuleConditionId = '156') <= 0)
INSERT [dbo].[MODULE_CONDITION] ([ModuleConditionId], [Name], [MKey], [ValueToCompare]) VALUES (156, N'If Draft Status is ApprovedByPOO', N'CmsDraftStatusApprovedByPOO', 4096)
IF((SELECT COUNT(*) FROM MODULE_CONDITION WHERE ModuleConditionId = '157') <= 0)
INSERT [dbo].[MODULE_CONDITION] ([ModuleConditionId], [Name], [MKey], [ValueToCompare]) VALUES (157, N'If Draft Status is RejectedByPOO', N'CmsDraftStatusRejectedByPOO', 8196)
IF((SELECT COUNT(*) FROM MODULE_CONDITION WHERE ModuleConditionId = '158') <= 0)
INSERT [dbo].[MODULE_CONDITION] ([ModuleConditionId], [Name], [MKey], [ValueToCompare]) VALUES (158, N'If Draft Status is ApproveByLawyer', N'CmsDraftStatusApproveByLawyer', 16384)
IF((SELECT COUNT(*) FROM MODULE_CONDITION WHERE ModuleConditionId = '159') <= 0)
INSERT [dbo].[MODULE_CONDITION] ([ModuleConditionId], [Name], [MKey], [ValueToCompare]) VALUES (159, N'If Draft Status is RejectedByLawyer', N'CmsDraftStatusRejectedByLawyer', 32768)
----------------
INSERT [dbo].WORKFLOW_SUBMODULE_CONDITION ([WorkflowSubModuleId],[SubModuleConditionId]) VALUES (1,154)
INSERT [dbo].WORKFLOW_SUBMODULE_CONDITION ([WorkflowSubModuleId],[SubModuleConditionId]) VALUES (1,155)
INSERT [dbo].WORKFLOW_SUBMODULE_CONDITION ([WorkflowSubModuleId],[SubModuleConditionId]) VALUES (1,156)
INSERT [dbo].WORKFLOW_SUBMODULE_CONDITION ([WorkflowSubModuleId],[SubModuleConditionId]) VALUES (1,157)
INSERT [dbo].WORKFLOW_SUBMODULE_CONDITION ([WorkflowSubModuleId],[SubModuleConditionId]) VALUES (1,158)
INSERT [dbo].WORKFLOW_SUBMODULE_CONDITION ([WorkflowSubModuleId],[SubModuleConditionId]) VALUES (1,159)
INSERT [dbo].WORKFLOW_SUBMODULE_CONDITION ([WorkflowSubModuleId],[SubModuleConditionId]) VALUES (2,154)
INSERT [dbo].WORKFLOW_SUBMODULE_CONDITION ([WorkflowSubModuleId],[SubModuleConditionId]) VALUES (2,155)
INSERT [dbo].WORKFLOW_SUBMODULE_CONDITION ([WorkflowSubModuleId],[SubModuleConditionId]) VALUES (2,156)
INSERT [dbo].WORKFLOW_SUBMODULE_CONDITION ([WorkflowSubModuleId],[SubModuleConditionId]) VALUES (2,157)
INSERT [dbo].WORKFLOW_SUBMODULE_CONDITION ([WorkflowSubModuleId],[SubModuleConditionId]) VALUES (2,158)
INSERT [dbo].WORKFLOW_SUBMODULE_CONDITION ([WorkflowSubModuleId],[SubModuleConditionId]) VALUES (2,159)
----------------
INSERT INTO MODULE_ACTIVITY (Name,Class,Method,CategoryId,AKey) 
VALUES ('Lawyer-ReviewDraft','WorkflowImplementationService','Cms_ReviewDraftDocumentLawyer','2','CmsReviewDraftDocumentLawyer')
----------------
SET IDENTITY_INSERT PARAMETER ON
INSERT INTO PARAMETER([ParameterId],[Name],[PKey],[Mandatory],[IsAutoPopulated]) VALUES(28,'User','CmsReviewDraftDocumentLawyer_User',0,0)
INSERT INTO PARAMETER([ParameterId],[Name],[PKey],[Mandatory],[IsAutoPopulated]) VALUES(29,'User Role','CmsReviewDraftDocumentLawyer_UserRole',0,0)
SET IDENTITY_INSERT PARAMETER OFF
-----------------
INSERT INTO MODULE_ACTIVITY_PARAMETERS VALUES ('28','16')
INSERT INTO MODULE_ACTIVITY_PARAMETERS VALUES ('29','16')
------------------

INSERT INTO WORKFLOW_SUBMODULE_ACTIVITY VALUES (1 , 16)
INSERT INTO WORKFLOW_SUBMODULE_ACTIVITY VALUES (2 , 16)
-----------------
INSERT INTO MODULE_CONDITION_OPTIONS ([ModuleOptionId] , [Name]) VALUES (1 , 'SendToHos')
INSERT INTO MODULE_CONDITION_OPTIONS ([ModuleOptionId] , [Name]) VALUES (2 , 'SendToSup')
INSERT INTO MODULE_CONDITION_OPTIONS ([ModuleOptionId] , [Name]) VALUES (4 , 'SendToLawyer')
INSERT INTO MODULE_CONDITION_OPTIONS ([ModuleOptionId] , [Name]) VALUES (8 , 'SendToGS')
INSERT INTO MODULE_CONDITION_OPTIONS ([ModuleOptionId] , [Name]) VALUES (16 , 'SendToPOO')
INSERT [dbo].WORKFLOW_SUBMODULE_CONDITION ([WorkflowSubModuleId],[SubModuleConditionId]) VALUES (2,153)
----------------
INSERT [dbo].Submodule ([id],[Name_En],[Name_Ar]) VALUES (128,N'DMSReviewDocument',N'DMSReviewDocument')
INSERT [dbo].UMS_USER_CLAIMS ([UserId],[ClaimType],[ClaimValue]) VALUES ('74686d50-809a-4fef-a517-4156b1334d22','Permission','Permissions.DMS.Document.View')
------------
ALTER TABLE MODULE_ACTIVITY
ADD IsEndofFlow bit;

begin tran
update MODULE_ACTIVITY set IsEndofFlow=1 where ActivityId=17
-----------------------------
INSERT [dbo].tTranslation ([Url],[TranslationKey],[TranslationType],[Value_Ar],[Value_En],[ReferenceScreen],[CreateDate],[UpdateDate])
VALUES ('Script',N'Choose_Execution_Branch',0,N'Choose Execution Branch','Choose Execution Branch','Create Workflow Page',GETDATE(),GETDATE())
---------------
ALTER TABLE WORKFLOW_ATTACHMENT_TYPE
ADD IsActiveFlow bit;
update MODULE_ACTIVITY set IsEndofFlow=1 where ActivityId=17
--------------------------------
----------------------------------
------------------------------------Transfer Request WORKFLOW(10-23-2023)
SET IDENTITY_INSERT MODULE_TRIGGER ON
INSERT INTO MODULE_TRIGGER ([ModuleTriggerId] ,[Name]) VALUES(7,'User Transfer Request')
SET IDENTITY_INSERT MODULE_TRIGGER Off
--------------------------------
INSERT INTO WORKFLOW_SUBMODULE_TRIGGER ([Id],[WorkflowSubModuleId] ,[ModuleTriggerId]) VALUES(14,1,7)
INSERT INTO WORKFLOW_SUBMODULE_TRIGGER ([Id],[WorkflowSubModuleId] ,[ModuleTriggerId]) VALUES(15,2,7)
----------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MODULE_ACTIVITY]') AND type in (N'U'))
INSERT INTO MODULE_ACTIVITY (Name,Class,Method,CategoryId,AKey) VALUES ('Transfer-Request','WorkflowImplementationService','Cms_RequestTransferToSector','2','CmsRequestTransferToSector')
-----------------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PARAMETER]') AND type in (N'U'))
SET IDENTITY_INSERT PARAMETER ON
INSERT INTO PARAMETER([ParameterId],[Name],[PKey],[Mandatory],[IsAutoPopulated]) VALUES(32,'User','CmsTransferRequestUser',0,0)
INSERT INTO PARAMETER([ParameterId],[Name],[PKey],[Mandatory],[IsAutoPopulated]) VALUES(33,'User Role','CmsTransferRequestUserRole',0,0)
SET IDENTITY_INSERT PARAMETER OFF
-----------------------------------------
INSERT [dbo].WORKFLOW_SUBMODULE_ACTIVITY ([WorkflowSubModuleId],[SubModuleActivityId]) VALUES (1,18)
INSERT [dbo].WORKFLOW_SUBMODULE_ACTIVITY ([WorkflowSubModuleId],[SubModuleActivityId]) VALUES (2,18)
INSERT [dbo].WORKFLOW_SUBMODULE_ACTIVITY ([WorkflowSubModuleId],[SubModuleActivityId]) VALUES (3,18)
INSERT [dbo].WORKFLOW_SUBMODULE_ACTIVITY ([WorkflowSubModuleId],[SubModuleActivityId]) VALUES (4,18)
INSERT [dbo].WORKFLOW_SUBMODULE_ACTIVITY ([WorkflowSubModuleId],[SubModuleActivityId]) VALUES (5,18)
INSERT [dbo].WORKFLOW_SUBMODULE_ACTIVITY ([WorkflowSubModuleId],[SubModuleActivityId]) VALUES (6,18)
INSERT [dbo].WORKFLOW_SUBMODULE_ACTIVITY ([WorkflowSubModuleId],[SubModuleActivityId]) VALUES (7,18)
---------------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MODULE_ACTIVITY_PARAMETERS]') AND type in (N'U'))
INSERT INTO MODULE_ACTIVITY_PARAMETERS VALUES ('32','18')
INSERT INTO MODULE_ACTIVITY_PARAMETERS VALUES ('33','18')
--------------------------------------
IF((SELECT COUNT(*) FROM MODULE_CONDITION WHERE ModuleConditionId = '160') <= 0)
INSERT [dbo].[MODULE_CONDITION] ([ModuleConditionId], [Name], [MKey], [ValueToCompare]) VALUES (160, N'If Transfer Status is Pending', N'CmsTransferPending', 1)
IF((SELECT COUNT(*) FROM MODULE_CONDITION WHERE ModuleConditionId = '161') <= 0)
INSERT [dbo].[MODULE_CONDITION] ([ModuleConditionId], [Name], [MKey], [ValueToCompare]) VALUES (161, N'If Transfer Status is Approved', N'CmsTransferApproved', 2)
IF((SELECT COUNT(*) FROM MODULE_CONDITION WHERE ModuleConditionId = '162') <= 0)
INSERT [dbo].[MODULE_CONDITION] ([ModuleConditionId], [Name], [MKey], [ValueToCompare]) VALUES (162, N'If Transfer Status is Rejected', N'CmsTransferRejected', 4)
----------------------------------
INSERT [dbo].WORKFLOW_SUBMODULE_CONDITION ([WorkflowSubModuleId],[SubModuleConditionId]) VALUES (1,160)
INSERT [dbo].WORKFLOW_SUBMODULE_CONDITION ([WorkflowSubModuleId],[SubModuleConditionId]) VALUES (1,161)
INSERT [dbo].WORKFLOW_SUBMODULE_CONDITION ([WorkflowSubModuleId],[SubModuleConditionId]) VALUES (1,162)


INSERT [dbo].WORKFLOW_SUBMODULE_CONDITION ([WorkflowSubModuleId],[SubModuleConditionId]) VALUES (2,160)
INSERT [dbo].WORKFLOW_SUBMODULE_CONDITION ([WorkflowSubModuleId],[SubModuleConditionId]) VALUES (2,161)
INSERT [dbo].WORKFLOW_SUBMODULE_CONDITION ([WorkflowSubModuleId],[SubModuleConditionId]) VALUES (2,162)

INSERT [dbo].WORKFLOW_SUBMODULE_CONDITION ([WorkflowSubModuleId],[SubModuleConditionId]) VALUES (3,160)
INSERT [dbo].WORKFLOW_SUBMODULE_CONDITION ([WorkflowSubModuleId],[SubModuleConditionId]) VALUES (3,161)
INSERT [dbo].WORKFLOW_SUBMODULE_CONDITION ([WorkflowSubModuleId],[SubModuleConditionId]) VALUES (3,162)

INSERT [dbo].WORKFLOW_SUBMODULE_CONDITION ([WorkflowSubModuleId],[SubModuleConditionId]) VALUES (4,160)
INSERT [dbo].WORKFLOW_SUBMODULE_CONDITION ([WorkflowSubModuleId],[SubModuleConditionId]) VALUES (4,161)
INSERT [dbo].WORKFLOW_SUBMODULE_CONDITION ([WorkflowSubModuleId],[SubModuleConditionId]) VALUES (4,162)

INSERT [dbo].WORKFLOW_SUBMODULE_CONDITION ([WorkflowSubModuleId],[SubModuleConditionId]) VALUES (5,160)
INSERT [dbo].WORKFLOW_SUBMODULE_CONDITION ([WorkflowSubModuleId],[SubModuleConditionId]) VALUES (5,161)
INSERT [dbo].WORKFLOW_SUBMODULE_CONDITION ([WorkflowSubModuleId],[SubModuleConditionId]) VALUES (5,162)

INSERT [dbo].WORKFLOW_SUBMODULE_CONDITION ([WorkflowSubModuleId],[SubModuleConditionId]) VALUES (6,160)
INSERT [dbo].WORKFLOW_SUBMODULE_CONDITION ([WorkflowSubModuleId],[SubModuleConditionId]) VALUES (6,161)
INSERT [dbo].WORKFLOW_SUBMODULE_CONDITION ([WorkflowSubModuleId],[SubModuleConditionId]) VALUES (6,162)

INSERT [dbo].WORKFLOW_SUBMODULE_CONDITION ([WorkflowSubModuleId],[SubModuleConditionId]) VALUES (7,160)
INSERT [dbo].WORKFLOW_SUBMODULE_CONDITION ([WorkflowSubModuleId],[SubModuleConditionId]) VALUES (7,161)
INSERT [dbo].WORKFLOW_SUBMODULE_CONDITION ([WorkflowSubModuleId],[SubModuleConditionId]) VALUES (7,162)
-----------------------------------------

------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MODULE_ACTIVITY]') AND type in (N'U'))
INSERT INTO MODULE_ACTIVITY (Name,Class,Method,CategoryId,AKey) VALUES ('Transfer Back To Initiator','WorkflowImplementationService','Cms_RequestTransferToInitiator','2','CmsRequestTransferToIntiator')
-----------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PARAMETER]') AND type in (N'U'))
INSERT INTO PARAMETER VALUES('User','CmsTransferToInitiatorRequestUser',0,0)
INSERT INTO PARAMETER VALUES('User Role','CmsTransferToInitiatorRequestUserRole',0,0)
----------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MODULE_ACTIVITY_PARAMETERS]') AND type in (N'U'))
INSERT INTO MODULE_ACTIVITY_PARAMETERS VALUES ('34','19')
INSERT INTO MODULE_ACTIVITY_PARAMETERS VALUES ('35','19')
------------------------
INSERT [dbo].WORKFLOW_SUBMODULE_ACTIVITY ([WorkflowSubModuleId],[SubModuleActivityId]) VALUES (1,19)
INSERT [dbo].WORKFLOW_SUBMODULE_ACTIVITY ([WorkflowSubModuleId],[SubModuleActivityId]) VALUES (2,19)
INSERT [dbo].WORKFLOW_SUBMODULE_ACTIVITY ([WorkflowSubModuleId],[SubModuleActivityId]) VALUES (3,19)
INSERT [dbo].WORKFLOW_SUBMODULE_ACTIVITY ([WorkflowSubModuleId],[SubModuleActivityId]) VALUES (4,19)
INSERT [dbo].WORKFLOW_SUBMODULE_ACTIVITY ([WorkflowSubModuleId],[SubModuleActivityId]) VALUES (5,19)
INSERT [dbo].WORKFLOW_SUBMODULE_ACTIVITY ([WorkflowSubModuleId],[SubModuleActivityId]) VALUES (6,19)
INSERT [dbo].WORKFLOW_SUBMODULE_ACTIVITY ([WorkflowSubModuleId],[SubModuleActivityId]) VALUES (7,19)
------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MODULE_ACTIVITY]') AND type in (N'U'))
INSERT INTO MODULE_ACTIVITY (Name,Class,Method,CategoryId,AKey,IsEndofFlow) VALUES ('TransferTo-GS','WorkflowImplementationService','Cms_TransferToGS','2','CmsTransferToGS',0)
------------------------
INSERT [dbo].WORKFLOW_SUBMODULE_ACTIVITY ([WorkflowSubModuleId],[SubModuleActivityId]) VALUES (1,20)
INSERT [dbo].WORKFLOW_SUBMODULE_ACTIVITY ([WorkflowSubModuleId],[SubModuleActivityId]) VALUES (2,20)
INSERT [dbo].WORKFLOW_SUBMODULE_ACTIVITY ([WorkflowSubModuleId],[SubModuleActivityId]) VALUES (3,20)
INSERT [dbo].WORKFLOW_SUBMODULE_ACTIVITY ([WorkflowSubModuleId],[SubModuleActivityId]) VALUES (4,20)
INSERT [dbo].WORKFLOW_SUBMODULE_ACTIVITY ([WorkflowSubModuleId],[SubModuleActivityId]) VALUES (5,20)
INSERT [dbo].WORKFLOW_SUBMODULE_ACTIVITY ([WorkflowSubModuleId],[SubModuleActivityId]) VALUES (6,20)
INSERT [dbo].WORKFLOW_SUBMODULE_ACTIVITY ([WorkflowSubModuleId],[SubModuleActivityId]) VALUES (7,20)
-------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PARAMETER]') AND type in (N'U'))
INSERT INTO PARAMETER VALUES('User','CmsTransferToGSUser',0,0)
INSERT INTO PARAMETER VALUES('User Role','CmsTransferToGSUserRole',0,0)
-----------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MODULE_ACTIVITY_PARAMETERS]') AND type in (N'U'))
INSERT INTO MODULE_ACTIVITY_PARAMETERS VALUES ('36','20')
INSERT INTO MODULE_ACTIVITY_PARAMETERS VALUES ('37','20')
-----------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MODULE_ACTIVITY]') AND type in (N'U'))
INSERT INTO MODULE_ACTIVITY (Name,Class,Method,CategoryId,AKey,IsEndofFlow) VALUES ('TransferTo-POO','WorkflowImplementationService','Cms_TransferToPOO','2','CmsTransferToPOO',0)
------------------------
INSERT [dbo].WORKFLOW_SUBMODULE_ACTIVITY ([WorkflowSubModuleId],[SubModuleActivityId]) VALUES (1,21)
INSERT [dbo].WORKFLOW_SUBMODULE_ACTIVITY ([WorkflowSubModuleId],[SubModuleActivityId]) VALUES (2,21)
INSERT [dbo].WORKFLOW_SUBMODULE_ACTIVITY ([WorkflowSubModuleId],[SubModuleActivityId]) VALUES (3,21)
INSERT [dbo].WORKFLOW_SUBMODULE_ACTIVITY ([WorkflowSubModuleId],[SubModuleActivityId]) VALUES (4,21)
INSERT [dbo].WORKFLOW_SUBMODULE_ACTIVITY ([WorkflowSubModuleId],[SubModuleActivityId]) VALUES (5,21)
INSERT [dbo].WORKFLOW_SUBMODULE_ACTIVITY ([WorkflowSubModuleId],[SubModuleActivityId]) VALUES (6,21)
INSERT [dbo].WORKFLOW_SUBMODULE_ACTIVITY ([WorkflowSubModuleId],[SubModuleActivityId]) VALUES (7,21)
-------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PARAMETER]') AND type in (N'U'))
INSERT INTO PARAMETER VALUES('User','CmsTransferToPOOUser',0,0)
INSERT INTO PARAMETER VALUES('User Role','CmsTransferToPOOUserRole',0,0)
----------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MODULE_ACTIVITY_PARAMETERS]') AND type in (N'U'))
INSERT INTO MODULE_ACTIVITY_PARAMETERS VALUES ('38','21')
INSERT INTO MODULE_ACTIVITY_PARAMETERS VALUES ('39','21')
---------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MODULE_ACTIVITY]') AND type in (N'U'))
INSERT INTO MODULE_ACTIVITY (Name,Class,Method,CategoryId,AKey,IsEndofFlow) VALUES ('TransferTo-POS','WorkflowImplementationService','Cms_TransferToPOS','2','CmsTransferToPOS',0)
------------------------
INSERT [dbo].WORKFLOW_SUBMODULE_ACTIVITY ([WorkflowSubModuleId],[SubModuleActivityId]) VALUES (1,22)
INSERT [dbo].WORKFLOW_SUBMODULE_ACTIVITY ([WorkflowSubModuleId],[SubModuleActivityId]) VALUES (2,22)
INSERT [dbo].WORKFLOW_SUBMODULE_ACTIVITY ([WorkflowSubModuleId],[SubModuleActivityId]) VALUES (3,22)
INSERT [dbo].WORKFLOW_SUBMODULE_ACTIVITY ([WorkflowSubModuleId],[SubModuleActivityId]) VALUES (4,22)
INSERT [dbo].WORKFLOW_SUBMODULE_ACTIVITY ([WorkflowSubModuleId],[SubModuleActivityId]) VALUES (5,22)
INSERT [dbo].WORKFLOW_SUBMODULE_ACTIVITY ([WorkflowSubModuleId],[SubModuleActivityId]) VALUES (6,22)
INSERT [dbo].WORKFLOW_SUBMODULE_ACTIVITY ([WorkflowSubModuleId],[SubModuleActivityId]) VALUES (7,22)
-------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PARAMETER]') AND type in (N'U'))
INSERT INTO PARAMETER VALUES('User','CmsTransferToPOSUser',0,0)
INSERT INTO PARAMETER VALUES('User Role','CmsTransferToPOSUserRole',0,0)
----------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MODULE_ACTIVITY_PARAMETERS]') AND type in (N'U'))
INSERT INTO MODULE_ACTIVITY_PARAMETERS VALUES ('40','22')
INSERT INTO MODULE_ACTIVITY_PARAMETERS VALUES ('41','22')
-----------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MODULE_ACTIVITY]') AND type in (N'U'))
INSERT INTO MODULE_ACTIVITY (Name,Class,Method,CategoryId,AKey,IsEndofFlow) VALUES ('TransferTo-FP','WorkflowImplementationService','Cms_TransferToFP','2','CmsTransferToFP',0)
------------------------
INSERT [dbo].WORKFLOW_SUBMODULE_ACTIVITY ([WorkflowSubModuleId],[SubModuleActivityId]) VALUES (1,23)
INSERT [dbo].WORKFLOW_SUBMODULE_ACTIVITY ([WorkflowSubModuleId],[SubModuleActivityId]) VALUES (2,23)
INSERT [dbo].WORKFLOW_SUBMODULE_ACTIVITY ([WorkflowSubModuleId],[SubModuleActivityId]) VALUES (3,23)
INSERT [dbo].WORKFLOW_SUBMODULE_ACTIVITY ([WorkflowSubModuleId],[SubModuleActivityId]) VALUES (4,23)
INSERT [dbo].WORKFLOW_SUBMODULE_ACTIVITY ([WorkflowSubModuleId],[SubModuleActivityId]) VALUES (5,23)
INSERT [dbo].WORKFLOW_SUBMODULE_ACTIVITY ([WorkflowSubModuleId],[SubModuleActivityId]) VALUES (6,23)
INSERT [dbo].WORKFLOW_SUBMODULE_ACTIVITY ([WorkflowSubModuleId],[SubModuleActivityId]) VALUES (7,23)
-------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PARAMETER]') AND type in (N'U'))
INSERT INTO PARAMETER VALUES('User','CmsTransferToFPUser',0,0)
INSERT INTO PARAMETER VALUES('User Role','CmsTransferToFPUserRole',0,0)
----------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MODULE_ACTIVITY_PARAMETERS]') AND type in (N'U'))
INSERT INTO MODULE_ACTIVITY_PARAMETERS VALUES ('42','23')
INSERT INTO MODULE_ACTIVITY_PARAMETERS VALUES ('43','23')
----------------------------------
------------------------------------Transfer Request WORKFLOW(10-23-2023)

------------------------------------------------------------Transfer File WORKFLOW(10-27-2023)
---------------------------------------------------------
------------------------------------------------
SET IDENTITY_INSERT MODULE_TRIGGER ON
INSERT INTO MODULE_TRIGGER ([ModuleTriggerId] ,[Name]) VALUES(8,'User Transfer File')
SET IDENTITY_INSERT MODULE_TRIGGER Off
---------------------------------------
INSERT INTO WORKFLOW_SUBMODULE_TRIGGER ([Id],[WorkflowSubModuleId] ,[ModuleTriggerId]) VALUES(16,1,8)
INSERT INTO WORKFLOW_SUBMODULE_TRIGGER ([Id],[WorkflowSubModuleId] ,[ModuleTriggerId]) VALUES(17,2,8)



---------------------------------------------
----------------------------------------------------
--------------------------------------------------------------Transfer File WORKFLOW(10-27-2023)


------------------------------------------------------------ Consultation Transfer  WORKFLOW(10-27-2023)
---------------------------------------------------------
------------------------------------------------
SET IDENTITY_INSERT MODULE_TRIGGER ON
INSERT INTO MODULE_TRIGGER ([ModuleTriggerId] ,[Name]) VALUES(9,'User Transfer Consultation Request')
SET IDENTITY_INSERT MODULE_TRIGGER Off 
-----------------------------------------------
INSERT INTO WORKFLOW_SUBMODULE_TRIGGER ([Id],[WorkflowSubModuleId] ,[ModuleTriggerId]) VALUES(18,3,9)
INSERT INTO WORKFLOW_SUBMODULE_TRIGGER ([Id],[WorkflowSubModuleId] ,[ModuleTriggerId]) VALUES(19,4,9)
INSERT INTO WORKFLOW_SUBMODULE_TRIGGER ([Id],[WorkflowSubModuleId] ,[ModuleTriggerId]) VALUES(20,5,9)
INSERT INTO WORKFLOW_SUBMODULE_TRIGGER ([Id],[WorkflowSubModuleId] ,[ModuleTriggerId]) VALUES(21,6,9)
INSERT INTO WORKFLOW_SUBMODULE_TRIGGER ([Id],[WorkflowSubModuleId] ,[ModuleTriggerId]) VALUES(22,7,9)

--------------------------------------------
SET IDENTITY_INSERT MODULE_TRIGGER ON
INSERT INTO MODULE_TRIGGER ([ModuleTriggerId] ,[Name]) VALUES(10,'User Transfer Consultation File')
SET IDENTITY_INSERT MODULE_TRIGGER Off
-------------------------------------------
INSERT INTO WORKFLOW_SUBMODULE_TRIGGER ([Id],[WorkflowSubModuleId] ,[ModuleTriggerId]) VALUES(23,3,10)
INSERT INTO WORKFLOW_SUBMODULE_TRIGGER ([Id],[WorkflowSubModuleId] ,[ModuleTriggerId]) VALUES(24,4,10)
INSERT INTO WORKFLOW_SUBMODULE_TRIGGER ([Id],[WorkflowSubModuleId] ,[ModuleTriggerId]) VALUES(25,5,10)
INSERT INTO WORKFLOW_SUBMODULE_TRIGGER ([Id],[WorkflowSubModuleId] ,[ModuleTriggerId]) VALUES(26,6,10)
INSERT INTO WORKFLOW_SUBMODULE_TRIGGER ([Id],[WorkflowSubModuleId] ,[ModuleTriggerId]) VALUES(27,7,10)

---------------------------------------------
----------------------------------------------------
--------------------------------------------------------------Consultation Transfer (10-27-2023)
-----------------------------------
-------------------------
INSERT INTO WF_TRIGGER_OPTIONS_PR ([OptionId],[TriggerId]) VALUES (1,4)
INSERT INTO WF_TRIGGER_OPTIONS_PR ([OptionId],[TriggerId]) VALUES (2,4)
INSERT INTO WF_TRIGGER_OPTIONS_PR ([OptionId],[TriggerId]) VALUES (4,4)
INSERT INTO WF_TRIGGER_OPTIONS_PR ([OptionId],[TriggerId]) VALUES (8,4)
INSERT INTO WF_TRIGGER_OPTIONS_PR ([OptionId],[TriggerId]) VALUES (16,4)
------------------------------------------
--------------------------------------------Transfer Option Workflow
IF((SELECT COUNT(*) FROM WF_WORKFLOW_BRANCH_PR_LKP WHERE BranchId = 1) = 1)
UPDATE WF_WORKFLOW_BRANCH_PR_LKP SET Name = 'OptionalBranch' where BranchId = 1
GO
------------------------------------
INSERT INTO WF_OPTIONS_PR_LKP (ModuleOptionId , Name) VALUES(32,'TransferToInitiator')
INSERT INTO WF_OPTIONS_PR_LKP (ModuleOptionId , Name) VALUES(64,'ApproveAndWork')
INSERT INTO WF_OPTIONS_PR_LKP (ModuleOptionId , Name) VALUES(128,'TransferToInitiatorAndEndflow')
INSERT INTO WF_OPTIONS_PR_LKP (ModuleOptionId , Name) VALUES(256,'TransferToRecieverAndEndflow')
--------------------------------------
INSERT INTO WF_TRIGGER_OPTIONS_PR (TriggerId , OptionId) VALUES(7,8)
INSERT INTO WF_TRIGGER_OPTIONS_PR (TriggerId , OptionId) VALUES(7,16)
INSERT INTO WF_TRIGGER_OPTIONS_PR (TriggerId , OptionId) VALUES(7,32)
INSERT INTO WF_TRIGGER_OPTIONS_PR (TriggerId , OptionId) VALUES(7,64)
INSERT INTO WF_TRIGGER_OPTIONS_PR (TriggerId , OptionId) VALUES(7,128)
INSERT INTO WF_TRIGGER_OPTIONS_PR (TriggerId , OptionId) VALUES(7,256)
---------------------------------
INSERT INTO WF_TRIGGER_OPTIONS_PR (TriggerId , OptionId) VALUES(8,8)	
INSERT INTO WF_TRIGGER_OPTIONS_PR (TriggerId , OptionId) VALUES(8,16)
INSERT INTO WF_TRIGGER_OPTIONS_PR (TriggerId , OptionId) VALUES(8,32)
INSERT INTO WF_TRIGGER_OPTIONS_PR (TriggerId , OptionId) VALUES(8,64)
INSERT INTO WF_TRIGGER_OPTIONS_PR (TriggerId , OptionId) VALUES(8,128)
INSERT INTO WF_TRIGGER_OPTIONS_PR (TriggerId , OptionId) VALUES(8,256)
----------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[WF_ACTIVITY_PR_LKP]') AND type in (N'U'))
INSERT INTO WF_ACTIVITY_PR_LKP (Name,Class,Method,CategoryId,AKey,IsEndofFlow) VALUES ('Transfer To Initiator And Endflow','WorkflowImplementationService','Cms_TransferToInitiatorAndEndflow','2','CmsTransferToInitiatorAndEndflow',1)
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[WF_ACTIVITY_PR_LKP]') AND type in (N'U'))
INSERT INTO WF_ACTIVITY_PR_LKP (Name,Class,Method,CategoryId,AKey,IsEndofFlow) VALUES ('Transfer To Reciever And Endflow','WorkflowImplementationService','Cms_TransferToRecieverAndEndflow','2','CmsTransferToRecieverAndEndflow',1)
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[WF_ACTIVITY_PR_LKP]') AND type in (N'U'))
INSERT INTO WF_ACTIVITY_PR_LKP (Name,Class,Method,CategoryId,AKey,IsEndofFlow) VALUES ('Approve And Work','WorkflowImplementationService','Cms_ApproveAndWork','2','CmsApproveAndWork',1)

-----------------------------------------

INSERT INTO WF_PARAMETER_PR_LKP (Name,PKey,Mandatory,IsAutoPopulated) VALUES ('User', 'CmsTransferToInitiatorAndEndflowUser',0,0)
INSERT INTO WF_PARAMETER_PR_LKP (Name,PKey,Mandatory,IsAutoPopulated) VALUES ('User', 'CmsTransferToRecieverAndEndflowUser',0,0)
INSERT INTO WF_PARAMETER_PR_LKP (Name,PKey,Mandatory,IsAutoPopulated) VALUES ('User', 'CmsApproveTransferAndWorkUser',0,0)
------------------------------------------

INSERT INTO WF_ACTIVITY_PARAMETERS_PR (ParameterId , ActivityId) VALUES(44,24)
INSERT INTO WF_ACTIVITY_PARAMETERS_PR (ParameterId , ActivityId) VALUES(45,25)
INSERT INTO WF_ACTIVITY_PARAMETERS_PR (ParameterId , ActivityId) VALUES(46,26)
--------------------------------------------

INSERT INTO WF_TRIGGER_ACTIVITY_PR(TriggerId , ActivityId) VALUES(7,24)
INSERT INTO WF_TRIGGER_ACTIVITY_PR(TriggerId , ActivityId) VALUES(7,25)
INSERT INTO WF_TRIGGER_ACTIVITY_PR(TriggerId , ActivityId) VALUES(7,26)

INSERT INTO WF_TRIGGER_ACTIVITY_PR(TriggerId , ActivityId) VALUES(8,24)
INSERT INTO WF_TRIGGER_ACTIVITY_PR(TriggerId , ActivityId) VALUES(8,25)
INSERT INTO WF_TRIGGER_ACTIVITY_PR(TriggerId , ActivityId) VALUES(8,26)
------------------------------

Insert INTO WF_SUBMODULE_TRIGGER_PR (Id, WorkflowSubModuleId , ModuleTriggerId) VALUES (28,1,8)
Insert INTO WF_SUBMODULE_TRIGGER_PR (Id, WorkflowSubModuleId , ModuleTriggerId) VALUES (29,2,8)
----------------------------

INSERT INTO WF_TRIGGER_ACTIVITY_PR (TriggerId , ActivityId) VALUES (9,24)
INSERT INTO WF_TRIGGER_ACTIVITY_PR (TriggerId , ActivityId) VALUES (9,25)
INSERT INTO WF_TRIGGER_ACTIVITY_PR (TriggerId , ActivityId) VALUES (9,26)


INSERT INTO WF_TRIGGER_ACTIVITY_PR (TriggerId , ActivityId) VALUES (10,24)
INSERT INTO WF_TRIGGER_ACTIVITY_PR (TriggerId , ActivityId) VALUES (10,25)
INSERT INTO WF_TRIGGER_ACTIVITY_PR (TriggerId , ActivityId) VALUES (10,26)
----------------------------
INSERT INTO WF_TRIGGER_OPTIONS_PR (TriggerId , OptionId) VALUES(9,8)	
INSERT INTO WF_TRIGGER_OPTIONS_PR (TriggerId , OptionId) VALUES(9,16)
INSERT INTO WF_TRIGGER_OPTIONS_PR (TriggerId , OptionId) VALUES(9,32)
INSERT INTO WF_TRIGGER_OPTIONS_PR (TriggerId , OptionId) VALUES(9,64)
INSERT INTO WF_TRIGGER_OPTIONS_PR (TriggerId , OptionId) VALUES(9,128)
INSERT INTO WF_TRIGGER_OPTIONS_PR (TriggerId , OptionId) VALUES(9,256)

INSERT INTO WF_TRIGGER_OPTIONS_PR (TriggerId , OptionId) VALUES(10,8)	
INSERT INTO WF_TRIGGER_OPTIONS_PR (TriggerId , OptionId) VALUES(10,16)
INSERT INTO WF_TRIGGER_OPTIONS_PR (TriggerId , OptionId) VALUES(10,32)
INSERT INTO WF_TRIGGER_OPTIONS_PR (TriggerId , OptionId) VALUES(10,64)
INSERT INTO WF_TRIGGER_OPTIONS_PR (TriggerId , OptionId) VALUES(10,128)
INSERT INTO WF_TRIGGER_OPTIONS_PR (TriggerId , OptionId) VALUES(10,256)
--------------------------------------------Transfer Option Workflow


-------------------------------------------Create Workflow-----------------------------------------

Alter Table MODULE_TRIGGER Add Name_Ar NVarchar(500)
--done for all 7 triggers
UPDATE WF_TRIGGER_PR_LKP SET Name = 'User_Submits_Document'  WHERE ModuleTriggerId = 1
UPDATE WF_TRIGGER_PR_LKP SET Name = 'User_Unpublished_Document'  WHERE ModuleTriggerId = 2
UPDATE WF_TRIGGER_PR_LKP SET Name = 'User_Submits_Principle'  WHERE ModuleTriggerId = 3
UPDATE WF_TRIGGER_PR_LKP SET Name = 'User_Submits_Case_Draft'  WHERE ModuleTriggerId = 4
UPDATE WF_TRIGGER_PR_LKP SET Name = 'User_Submits_Consultation_Draft'  WHERE ModuleTriggerId = 5
UPDATE WF_TRIGGER_PR_LKP SET Name = 'User_Submits_DMS_Document'  WHERE ModuleTriggerId = 6
UPDATE WF_TRIGGER_PR_LKP SET Name = 'User_Transfer_Request'  WHERE ModuleTriggerId = 7
UPDATE WF_TRIGGER_PR_LKP SET Name = 'Transfer_Case_File'  WHERE ModuleTriggerId = 8
UPDATE WF_TRIGGER_PR_LKP SET Name = 'Transfer_Consultation_Request'  WHERE ModuleTriggerId = 9
UPDATE WF_TRIGGER_PR_LKP SET Name = 'Transfer_Consultation_File'  WHERE ModuleTriggerId = 10

UPDATE WF_ACTIVITY_PR_LKP SET Name = 'Review_Document' WHERE ActivityId = 1
UPDATE WF_ACTIVITY_PR_LKP SET Name = 'Publish_Unpublish_Document' WHERE ActivityId = 2
UPDATE WF_ACTIVITY_PR_LKP SET Name = 'Send_Email' WHERE ActivityId = 5
UPDATE WF_ACTIVITY_PR_LKP SET Name = 'Review_Principle' WHERE ActivityId = 6
UPDATE WF_ACTIVITY_PR_LKP SET Name = 'Publish_Unpublish_Principle' WHERE ActivityId = 7
UPDATE WF_ACTIVITY_PR_LKP SET Name = 'SUPV_Review_Draft_Document' WHERE ActivityId = 8
UPDATE WF_ACTIVITY_PR_LKP SET Name = 'HOS_Review_Draft_Document' WHERE ActivityId = 9
UPDATE WF_ACTIVITY_PR_LKP SET Name = 'SUPCOMS_Review_Draft_Document' WHERE ActivityId = 10
UPDATE WF_ACTIVITY_PR_LKP SET Name = 'HOSCOMS_Review_Draft_Document' WHERE ActivityId = 11
UPDATE WF_ACTIVITY_PR_LKP SET Name = 'Review_DMS_Document' WHERE ActivityId = 12
UPDATE WF_ACTIVITY_PR_LKP SET Name = 'MOJ_Send_To_Messenger' WHERE ActivityId = 13
UPDATE WF_ACTIVITY_PR_LKP SET Name = 'GS_Review_Draft' WHERE ActivityId = 14
UPDATE WF_ACTIVITY_PR_LKP SET Name = 'POO_Review_Draft' WHERE ActivityId = 15
UPDATE WF_ACTIVITY_PR_LKP SET Name = 'Lawyer_Review_Draft' WHERE ActivityId = 16
UPDATE WF_ACTIVITY_PR_LKP SET Name = 'Publish_DMS_Document' WHERE ActivityId = 17
UPDATE WF_ACTIVITY_PR_LKP SET Name = 'Transfer_To_Sector' WHERE ActivityId = 18
UPDATE WF_ACTIVITY_PR_LKP SET Name = 'Transfer_Back_To_Initiator' WHERE ActivityId = 19
UPDATE WF_ACTIVITY_PR_LKP SET Name = 'Send_To_GS' WHERE ActivityId = 20
UPDATE WF_ACTIVITY_PR_LKP SET Name = 'Send_To_POO' WHERE ActivityId = 21
UPDATE WF_ACTIVITY_PR_LKP SET Name = 'Send_To_POS' WHERE ActivityId = 22
UPDATE WF_ACTIVITY_PR_LKP SET Name = 'Send_To_FP' WHERE ActivityId = 23
UPDATE WF_ACTIVITY_PR_LKP SET Name = 'Transfer_To_Initiator_And_Endflow' WHERE ActivityId = 24
UPDATE WF_ACTIVITY_PR_LKP SET Name = 'Transfer_To_Reciever_And_Endflow' WHERE ActivityId = 25
UPDATE WF_ACTIVITY_PR_LKP SET Name = 'Approve_And_Work' WHERE ActivityId = 26
UPDATE WF_ACTIVITY_PR_LKP SET Name = 'Lawyer_Draft_Modification' WHERE ActivityId = 27
UPDATE WF_ACTIVITY_PR_LKP SET Name = 'Publish_Draft_and_End_Flow' WHERE ActivityId = 28
UPDATE WF_ACTIVITY_PR_LKP SET Name = 'Publish_Draft_Send_to_G2G_and_End_Flow' WHERE ActivityId = 29

UPDATE WF_OPTIONS_PR_LKP SET Name = 'SendToHOS' WHERE ModuleOptionId = 1
UPDATE WF_OPTIONS_PR_LKP SET Name = 'SendToSupervisor' WHERE ModuleOptionId = 2

--Update in DMS Database
UPDATE ATTACHMENT_TYPE SET Type_Ar = (N'طلب سحب الطلب') WHERE AttachmentTypeId = 32 and Type_En = 'Withdraw Request'
UPDATE ATTACHMENT_TYPE SET Type_Ar = (N'حافظة المستندات') WHERE AttachmentTypeId = 33 and Type_En = 'Document Portfolio'
UPDATE ATTACHMENT_TYPE SET Type_Ar = (N'مستند قانوني') WHERE AttachmentTypeId = 34 and Type_En = 'Legal Document'
UPDATE ATTACHMENT_TYPE SET Type_Ar = (N'الشكوى') WHERE AttachmentTypeId = 35 and Type_En = 'Complaint'
UPDATE ATTACHMENT_TYPE SET Type_Ar = (N'القرار') WHERE AttachmentTypeId = 36 and Type_En = 'Decision'
UPDATE ATTACHMENT_TYPE SET Type_Ar = (N'إعلان مقاضاة من قبل الجهة الحكومية') WHERE AttachmentTypeId = 37 and Type_En = 'Anouncement of Porcecution For GE'
UPDATE ATTACHMENT_TYPE SET Type_Ar = (N'إعلان مقاضاة من قبل مجلس الوزاراء') WHERE AttachmentTypeId = 38 and Type_En = 'Anouncement of Porcecution For Council of Ministers'
UPDATE ATTACHMENT_TYPE SET Type_Ar = (N'إعلان مقاضاة من قبل مجلس الوزاراء') WHERE AttachmentTypeId = 39 and Type_En = 'Anouncement of Porcecution For Council of Ministers'
UPDATE ATTACHMENT_TYPE SET Type_Ar = (N'إعلان مقاضاة من قبل مجلس الوزاراء') WHERE AttachmentTypeId = 40 and Type_En = 'Anouncement of Porcecution For Council of Ministers'
UPDATE ATTACHMENT_TYPE SET Type_Ar = (N'مذكرة عرض') WHERE AttachmentTypeId = 44 and Type_En = 'Presentation Notes'
UPDATE ATTACHMENT_TYPE SET Type_Ar = (N'اخطار بحكم تمهيدي') WHERE AttachmentTypeId = 46 and Type_En = 'Initial Judgement Notification'
UPDATE ATTACHMENT_TYPE SET Type_Ar = (N'اخطار بما تم') WHERE AttachmentTypeId = 47 and Type_En = 'General Update Notification'
UPDATE ATTACHMENT_TYPE SET Type_Ar = (N'اخطار بإقامة دعوى') WHERE AttachmentTypeId = 48 and Type_En = 'Case Registered Notification'
UPDATE ATTACHMENT_TYPE SET Type_Ar = (N'اخطار بحفظ الملف') WHERE AttachmentTypeId = 50 and Type_En = 'Saving File Notification'
UPDATE ATTACHMENT_TYPE SET Type_Ar = (N'اخطار بالحكم') WHERE AttachmentTypeId = 53 and Type_En = 'Final Judgement Notification'
UPDATE ATTACHMENT_TYPE SET Type_Ar = (N'مستندات جلسة المحكمة') WHERE AttachmentTypeId = 54 and Type_En = 'Hearing Document'
UPDATE ATTACHMENT_TYPE SET Type_Ar = (N'مستندات تأجيل جلسة المحكمة') WHERE AttachmentTypeId = 58 and Type_En = 'Postpone Hearing Document'

ALTER TABLE WORKFLOW_ATTACHMENT_TYPE
ADD IsActiveFlow bit;
---------------------
INSERT [dbo].[UMS_GROUP] ([GroupId], [Name_En], [Name_Ar],[Description_En],[Description_Ar],[CreatedBy],[CreatedDate],[ModifiedBy],[ModifiedDate],[DeletedBy],DeletedDate,[IsDeleted])
VALUES ('35448264-4a25-4edb-8a14-322283a59bd6','Workflow_Author','Workflow_Author','Workflow_Author','Workflow_Author','superadmin@gmail.com',GETDATE(),'superadmin@gmail.com',GETDATE(),'','',0)
----------For author
INSERT INTO [dbo].[UMS_GROUP_CLAIMS] ([GroupId],[ClaimType],[ClaimValue])
VALUES ('35448264-4a25-4edb-8a14-322283a59bd6', 'Permission', 'Permissions.WF.Workflow.Create')
----------For Admin
INSERT INTO [dbo].[UMS_GROUP_CLAIMS] ([GroupId],[ClaimType],[ClaimValue])
VALUES ('071C4BA0-CC3F-43DC-B2CB-6FC2BEA6D831', 'Permission', 'Permissions.WF.Workflow.List')

INSERT INTO [dbo].[UMS_GROUP_CLAIMS] ([GroupId],[ClaimType],[ClaimValue])
VALUES ('071C4BA0-CC3F-43DC-B2CB-6FC2BEA6D831', 'Permission', 'Permissions.WF.Workflow.Create')

INSERT INTO [dbo].[UMS_GROUP_CLAIMS] ([GroupId],[ClaimType],[ClaimValue])
VALUES ('071C4BA0-CC3F-43DC-B2CB-6FC2BEA6D831', 'Permission', 'Permissions.WF.Workflow.Detail')

INSERT INTO [dbo].[UMS_GROUP_CLAIMS] ([GroupId],[ClaimType],[ClaimValue])
VALUES ('071C4BA0-CC3F-43DC-B2CB-6FC2BEA6D831', 'Permission', 'Permissions.WF.WorkflowInstance.List')
-----------------RelationShip-------
insert into UMS_GROUP_USER values ('071C4BA0-CC3F-43DC-B2CB-6FC2BEA6D831','09cbfcc1-be3f-48b4-8c0d-c88caa28759d')
insert into UMS_GROUP_USER values ('35448264-4A25-4EDB-8A14-322283A59BD6','32d06279-5996-4394-a723-d7c00e0f1645')
--------For Author
INSERT INTO [dbo].[UMS_GROUP_CLAIMS] ([GroupId],[ClaimType],[ClaimValue])
VALUES ('35448264-4A25-4EDB-8A14-322283A59BD6', 'Permission', 'Permissions.Workflow.Edit')

INSERT INTO [dbo].[UMS_GROUP_CLAIMS] ([GroupId],[ClaimType],[ClaimValue])
VALUES ('35448264-4A25-4EDB-8A14-322283A59BD6', 'Permission', 'Permissions.Workflow.Detail')

INSERT INTO [dbo].[UMS_GROUP_CLAIMS] ([GroupId],[ClaimType],[ClaimValue])
VALUES ('35448264-4A25-4EDB-8A14-322283A59BD6', 'Permission', 'Permissions.WF.Workflow.List')
--------For Admin
INSERT INTO [dbo].[UMS_GROUP_CLAIMS] ([GroupId],[ClaimType],[ClaimValue])
VALUES ('071C4BA0-CC3F-43DC-B2CB-6FC2BEA6D831', 'Permission', 'Permissions.Workflow.Detail')

INSERT INTO [dbo].[UMS_GROUP_CLAIMS] ([GroupId],[ClaimType],[ClaimValue])
VALUES ('071C4BA0-CC3F-43DC-B2CB-6FC2BEA6D831', 'Permission', 'Permissions.Workflow.Active')

INSERT INTO [dbo].[UMS_GROUP_CLAIMS] ([GroupId],[ClaimType],[ClaimValue])
VALUES ('071C4BA0-CC3F-43DC-B2CB-6FC2BEA6D831', 'Permission', 'Permissions.Workflow.Publish')

INSERT INTO [dbo].[UMS_GROUP_CLAIMS] ([GroupId],[ClaimType],[ClaimValue])
VALUES ('071C4BA0-CC3F-43DC-B2CB-6FC2BEA6D831', 'Permission', 'Permissions.Workflow.Delete')
-------------
SET IDENTITY_INSERT WF_ACTIVITY_PR_LKP ON
INSERT [dbo].WF_ACTIVITY_PR_LKP([ActivityId],[Name],[Class],[Method],[CategoryId],[AKey],[IsEndofFlow]) VALUES (27,N'Lawyer Draft Modification','WorkflowImplementationService','Lawyer_ModifyDraft',2,'LawyerDraftModification',0)
SET IDENTITY_INSERT WF_ACTIVITY_PR_LKP OFF
-------------------
INSERT INTO WF_TRIGGER_ACTIVITY_PR VALUES (4,27)
--------------------
SET IDENTITY_INSERT WF_PARAMETER_PR_LKP ON
INSERT [dbo].WF_PARAMETER_PR_LKP([ParameterId],[Name], [PKey],[Mandatory],[IsAutoPopulated]) VALUES (47,N'User', N'LawModifyDraftDocument_User',0,0)
SET IDENTITY_INSERT WF_PARAMETER_PR_LKP OFF
------------------------
SET IDENTITY_INSERT WF_PARAMETER_PR_LKP ON
INSERT [dbo].WF_PARAMETER_PR_LKP([ParameterId],[Name], [PKey],[Mandatory],[IsAutoPopulated]) VALUES (48,N'User Role', N'LawModifyDraftDocument_UserRole',0,0)
SET IDENTITY_INSERT WF_PARAMETER_PR_LKP OFF
-------------------
INSERT [dbo].WF_ACTIVITY_PARAMETERS_PR([ParameterId],[ActivityId]) VALUES (47,27)
INSERT [dbo].WF_ACTIVITY_PARAMETERS_PR ([ParameterId],[ActivityId]) VALUES (48,27)
-----------------
SET IDENTITY_INSERT WF_ACTIVITY_PR_LKP ON
INSERT [dbo].WF_ACTIVITY_PR_LKP([ActivityId],[Name],[Class],[Method],[CategoryId],[AKey],[IsEndofFlow]) VALUES (28,N'Publish Draft and End Flow','WorkflowImplementationService','PublishDraftand_EndFlow',2,'PublishDraftandEndFlow',1)
SET IDENTITY_INSERT WF_ACTIVITY_PR_LKP OFF
-------------------
INSERT INTO WF_TRIGGER_ACTIVITY_PR VALUES (4,28)
--------------------
SET IDENTITY_INSERT WF_PARAMETER_PR_LKP ON
INSERT [dbo].WF_PARAMETER_PR_LKP([ParameterId],[Name], [PKey],[Mandatory],[IsAutoPopulated]) VALUES (49,N'User', N'PublishDraftandEndFlow_User',0,0)
SET IDENTITY_INSERT WF_PARAMETER_PR_LKP OFF
------------------------
SET IDENTITY_INSERT WF_PARAMETER_PR_LKP ON
INSERT [dbo].WF_PARAMETER_PR_LKP([ParameterId],[Name], [PKey],[Mandatory],[IsAutoPopulated]) VALUES (50,N'User Role', N'PublishDraftandEndFlow_UserRole',0,0)
SET IDENTITY_INSERT WF_PARAMETER_PR_LKP OFF
-------------------
INSERT [dbo].WF_ACTIVITY_PARAMETERS_PR([ParameterId],[ActivityId]) VALUES (49,28)
INSERT [dbo].WF_ACTIVITY_PARAMETERS_PR ([ParameterId],[ActivityId]) VALUES (50,28)
-----------------
SET IDENTITY_INSERT WF_ACTIVITY_PR_LKP ON
INSERT [dbo].WF_ACTIVITY_PR_LKP([ActivityId],[Name],[Class],[Method],[CategoryId],[AKey],[IsEndofFlow]) VALUES (29,N'Publish Draft, Send to G2G and End Flow','WorkflowImplementationService','PublishDraftSendtoG2Gand_EndFlow',2,'PublishDraftSendtoG2GandEndFlow',1)
SET IDENTITY_INSERT WF_ACTIVITY_PR_LKP OFF
-------------------
INSERT INTO WF_TRIGGER_ACTIVITY_PR VALUES (4,29)
--------------------
SET IDENTITY_INSERT WF_PARAMETER_PR_LKP ON
INSERT [dbo].WF_PARAMETER_PR_LKP([ParameterId],[Name], [PKey],[Mandatory],[IsAutoPopulated]) VALUES (51,N'User', N'PublishDraftSendtoG2GandEndFlow_User',0,0)
SET IDENTITY_INSERT WF_PARAMETER_PR_LKP OFF
------------------------
SET IDENTITY_INSERT WF_PARAMETER_PR_LKP ON
INSERT [dbo].WF_PARAMETER_PR_LKP([ParameterId],[Name], [PKey],[Mandatory],[IsAutoPopulated]) VALUES (52,N'User Role', N'PublishDraftSendtoG2GandEndFlow_UserRole',0,0)
SET IDENTITY_INSERT WF_PARAMETER_PR_LKP OFF
-------------------
INSERT [dbo].WF_ACTIVITY_PARAMETERS_PR([ParameterId],[ActivityId]) VALUES (51,29)
INSERT [dbo].WF_ACTIVITY_PARAMETERS_PR ([ParameterId],[ActivityId]) VALUES (52,29)
-----------
IF((SELECT COUNT(*) FROM WF_CONDITION_PR_LKP WHERE ModuleConditionId = '163') <= 0)
INSERT [dbo].[WF_CONDITION_PR_LKP] ([ModuleConditionId], [Name], [MKey], [ValueToCompare]) VALUES (163, N'If Draft Document Status is InReview', N'CmsDraftDocumentStatusInReview', 1)
IF((SELECT COUNT(*) FROM WF_CONDITION_PR_LKP WHERE ModuleConditionId = '164') <= 0)
INSERT [dbo].[WF_CONDITION_PR_LKP] ([ModuleConditionId], [Name], [MKey], [ValueToCompare]) VALUES (164, N'If Draft Document Status is Draft', N'CmsDraftDocumentStatusDraft', 2)
IF((SELECT COUNT(*) FROM WF_CONDITION_PR_LKP WHERE ModuleConditionId = '165') <= 0)
INSERT [dbo].[WF_CONDITION_PR_LKP] ([ModuleConditionId], [Name], [MKey], [ValueToCompare]) VALUES (165, N'If Draft Document Status is Reject', N'CmsDraftDocumentStatusReject', 4)
IF((SELECT COUNT(*) FROM WF_CONDITION_PR_LKP WHERE ModuleConditionId = '166') <= 0)
INSERT [dbo].[WF_CONDITION_PR_LKP] ([ModuleConditionId], [Name], [MKey], [ValueToCompare]) VALUES (166, N'If Draft Document Status is Approve', N'CmsDraftDocumentStatusApprove', 8)
IF((SELECT COUNT(*) FROM WF_CONDITION_PR_LKP WHERE ModuleConditionId = '167') <= 0)
INSERT [dbo].[WF_CONDITION_PR_LKP] ([ModuleConditionId], [Name], [MKey], [ValueToCompare]) VALUES (167, N'If Draft Document Status is Published', N'CmsDraftDocumentStatusPublished', 16)
---------
INSERT [dbo].[WF_TRIGGER_CONDITION_PR] ([TriggerId], [ConditionId]) VALUES (4, 163)
INSERT [dbo].[WF_TRIGGER_CONDITION_PR] ([TriggerId], [ConditionId]) VALUES (4, 164)
INSERT [dbo].[WF_TRIGGER_CONDITION_PR] ([TriggerId], [ConditionId]) VALUES (4, 165)
INSERT [dbo].[WF_TRIGGER_CONDITION_PR] ([TriggerId], [ConditionId]) VALUES (4, 166)
INSERT [dbo].[WF_TRIGGER_CONDITION_PR] ([TriggerId], [ConditionId]) VALUES (4, 167)
-------------
INSERT [dbo].CMS_DRAFT_DOCUMENT_VERSION_STATUS ([Id],[NameEn],[NameAr]) VALUES (1,N'InReview',N'InReview')
INSERT [dbo].CMS_DRAFT_DOCUMENT_VERSION_STATUS ([Id],[NameEn],[NameAr]) VALUES (2,N'Draft',N'Draft')
INSERT [dbo].CMS_DRAFT_DOCUMENT_VERSION_STATUS ([Id],[NameEn],[NameAr]) VALUES (4,N'Reject',N'Reject')
INSERT [dbo].CMS_DRAFT_DOCUMENT_VERSION_STATUS ([Id],[NameEn],[NameAr]) VALUES (8,N'Approve',N'Approve')
INSERT [dbo].CMS_DRAFT_DOCUMENT_VERSION_STATUS ([Id],[NameEn],[NameAr]) VALUES (16,N'Published',N'Published')
-----------
INSERT [dbo].ATTACHMENT_TYPE ([AttachmentTypeId],[Type_Ar],[Type_En],[ModuleId],[IsMandatory],[IsOfficialLetter],[SubTypeId],[IsGePortalType],[IsOpinion]) VALUES (85,N'الاشعار القانوني',N'Coms Legal Notification',10,0,0,'',0,0)
---------
begin tran
update ATTACHMENT_TYPE set ModuleId=5
where AttachmentTypeId=29
commit
-----------
-----------------------------------------------------------
--------------------------------------------------------------------
-------------------------------------------------------------------------- Send Copy Case Request Workflow(10-11-2023)

SET IDENTITY_INSERT WF_TRIGGER_PR_LKP ON
INSERT INTO WF_TRIGGER_PR_LKP ([ModuleTriggerId] ,[Name],[IsConditional],[IsOptional]) VALUES(13,'Send_Copy_Case_Request',0,0)
SET IDENTITY_INSERT WF_TRIGGER_PR_LKP OFF
----------------------

Insert INTO WF_SUBMODULE_TRIGGER_PR (Id, WorkflowSubModuleId , ModuleTriggerId) VALUES (30,1,13)
Insert INTO WF_SUBMODULE_TRIGGER_PR (Id, WorkflowSubModuleId , ModuleTriggerId) VALUES (31,2,13)

----------------------
INSERT INTO WF_OPTIONS_PR_LKP (ModuleOptionId , Name) VALUES(512,'SendCopyToInitiator')	
INSERT INTO WF_OPTIONS_PR_LKP (ModuleOptionId , Name) VALUES(1024,'SendCopyToGS')	
INSERT INTO WF_OPTIONS_PR_LKP (ModuleOptionId , Name) VALUES(2048,'ApproveCopyAndWork')	
INSERT INTO WF_OPTIONS_PR_LKP (ModuleOptionId , Name) VALUES(4096,'SendCopyToInitiatorAndEndflow')	
INSERT INTO WF_OPTIONS_PR_LKP (ModuleOptionId , Name) VALUES(8192,'SendCopyToRecieverAndEndflow')	
---------------------

INSERT INTO WF_TRIGGER_OPTIONS_PR (TriggerId , OptionId) VALUES(13,512)	
INSERT INTO WF_TRIGGER_OPTIONS_PR (TriggerId , OptionId) VALUES(13,1024)	
INSERT INTO WF_TRIGGER_OPTIONS_PR (TriggerId , OptionId) VALUES(13,2048)	
INSERT INTO WF_TRIGGER_OPTIONS_PR (TriggerId , OptionId) VALUES(13,4096)	
INSERT INTO WF_TRIGGER_OPTIONS_PR (TriggerId , OptionId) VALUES(13,8192)	
---------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[WF_ACTIVITY_PR_LKP]') AND type in (N'U'))
INSERT INTO WF_ACTIVITY_PR_LKP (Name,Class,Method,CategoryId,AKey,IsEndofFlow) VALUES ('Send_Copy_To_Initiator','WorkflowImplementationService','Cms_SendCopyToInitiator','2','CmsSendCopyToInitiator',0)
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[WF_ACTIVITY_PR_LKP]') AND type in (N'U'))
INSERT INTO WF_ACTIVITY_PR_LKP (Name,Class,Method,CategoryId,AKey,IsEndofFlow) VALUES ('Send_Copy_To_GS','WorkflowImplementationService','Cms_SendCopyToGS','2','CmsSendCopyToGS',0)
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[WF_ACTIVITY_PR_LKP]') AND type in (N'U'))
INSERT INTO WF_ACTIVITY_PR_LKP (Name,Class,Method,CategoryId,AKey,IsEndofFlow) VALUES ('Approve_Copy_And_Work','WorkflowImplementationService','Cms_ApproveCopyAndWork','2','CmsApproveCopyAndWork',1)
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[WF_ACTIVITY_PR_LKP]') AND type in (N'U'))
INSERT INTO WF_ACTIVITY_PR_LKP (Name,Class,Method,CategoryId,AKey,IsEndofFlow) VALUES ('Send_Copy_To_Initiator_And_Endflow','WorkflowImplementationService','Cms_SendCopyToInitiatorAndEndflow','2','CmsSendCopyToInitiatorAndEndflow',1)
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[WF_ACTIVITY_PR_LKP]') AND type in (N'U'))
INSERT INTO WF_ACTIVITY_PR_LKP (Name,Class,Method,CategoryId,AKey,IsEndofFlow) VALUES ('Send_Copy_To_Reciever_And_Endflow','WorkflowImplementationService','Cms_SendCopyToRecieverAndEndflow','2','CmsSendCopyToRecieverAndEndflow',1)
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[WF_ACTIVITY_PR_LKP]') AND type in (N'U'))
INSERT INTO WF_ACTIVITY_PR_LKP (Name,Class,Method,CategoryId,AKey,IsEndofFlow) VALUES ('Send_Copy_To_Sector','WorkflowImplementationService','Cms_SendCopyToSector','2','CmsSendCopyToSector',0)

---------------------

INSERT INTO WF_PARAMETER_PR_LKP (Name,PKey,Mandatory,IsAutoPopulated) VALUES ('User', 'CmsSendCopyToInitiatorUser',0,0)
INSERT INTO WF_PARAMETER_PR_LKP (Name,PKey,Mandatory,IsAutoPopulated) VALUES ('User', 'CmsSendCopyToGSUser',0,0)
INSERT INTO WF_PARAMETER_PR_LKP (Name,PKey,Mandatory,IsAutoPopulated) VALUES ('User', 'CmsApproveCopyAndWorkUser',0,0)
INSERT INTO WF_PARAMETER_PR_LKP (Name,PKey,Mandatory,IsAutoPopulated) VALUES ('User', 'CmsSendCopyToInitiatorAndEndflowUser',0,0)
INSERT INTO WF_PARAMETER_PR_LKP (Name,PKey,Mandatory,IsAutoPopulated) VALUES ('User', 'CmsSendCopyToRecieverAndEndflowUser',0,0)
INSERT INTO WF_PARAMETER_PR_LKP (Name,PKey,Mandatory,IsAutoPopulated) VALUES ('User', 'CmsSendCopyUser',0,0)

----------------------

INSERT INTO WF_ACTIVITY_PARAMETERS_PR (ParameterId , ActivityId) VALUES(53,30)
INSERT INTO WF_ACTIVITY_PARAMETERS_PR (ParameterId , ActivityId) VALUES(54,31)
INSERT INTO WF_ACTIVITY_PARAMETERS_PR (ParameterId , ActivityId) VALUES(55,32)
INSERT INTO WF_ACTIVITY_PARAMETERS_PR (ParameterId , ActivityId) VALUES(56,33)
INSERT INTO WF_ACTIVITY_PARAMETERS_PR (ParameterId , ActivityId) VALUES(57,34)
INSERT INTO WF_ACTIVITY_PARAMETERS_PR (ParameterId , ActivityId) VALUES(58,35)
-----------------------

INSERT [dbo].WORKFLOW_SUBMODULE_ACTIVITY ([WorkflowSubModuleId],[SubModuleActivityId]) VALUES (1,30)
INSERT [dbo].WORKFLOW_SUBMODULE_ACTIVITY ([WorkflowSubModuleId],[SubModuleActivityId]) VALUES (1,31)
INSERT [dbo].WORKFLOW_SUBMODULE_ACTIVITY ([WorkflowSubModuleId],[SubModuleActivityId]) VALUES (1,32)
INSERT [dbo].WORKFLOW_SUBMODULE_ACTIVITY ([WorkflowSubModuleId],[SubModuleActivityId]) VALUES (1,33)
INSERT [dbo].WORKFLOW_SUBMODULE_ACTIVITY ([WorkflowSubModuleId],[SubModuleActivityId]) VALUES (1,34)
INSERT [dbo].WORKFLOW_SUBMODULE_ACTIVITY ([WorkflowSubModuleId],[SubModuleActivityId]) VALUES (1,35)



INSERT [dbo].WORKFLOW_SUBMODULE_ACTIVITY ([WorkflowSubModuleId],[SubModuleActivityId]) VALUES (2,30)
INSERT [dbo].WORKFLOW_SUBMODULE_ACTIVITY ([WorkflowSubModuleId],[SubModuleActivityId]) VALUES (2,31)
INSERT [dbo].WORKFLOW_SUBMODULE_ACTIVITY ([WorkflowSubModuleId],[SubModuleActivityId]) VALUES (2,32)
INSERT [dbo].WORKFLOW_SUBMODULE_ACTIVITY ([WorkflowSubModuleId],[SubModuleActivityId]) VALUES (2,33)
INSERT [dbo].WORKFLOW_SUBMODULE_ACTIVITY ([WorkflowSubModuleId],[SubModuleActivityId]) VALUES (2,34)
INSERT [dbo].WORKFLOW_SUBMODULE_ACTIVITY ([WorkflowSubModuleId],[SubModuleActivityId]) VALUES (2,35)


----------------------------------

INSERT INTO WF_TRIGGER_ACTIVITY_PR(TriggerId , ActivityId) VALUES(13,30)
INSERT INTO WF_TRIGGER_ACTIVITY_PR(TriggerId , ActivityId) VALUES(13,31)
INSERT INTO WF_TRIGGER_ACTIVITY_PR(TriggerId , ActivityId) VALUES(13,32)
INSERT INTO WF_TRIGGER_ACTIVITY_PR(TriggerId , ActivityId) VALUES(13,33)
INSERT INTO WF_TRIGGER_ACTIVITY_PR(TriggerId , ActivityId) VALUES(13,34)
INSERT INTO WF_TRIGGER_ACTIVITY_PR(TriggerId , ActivityId) VALUES(13,35)

----------------------------------

-----------------------------------------------------------
--------------------------------------------------------------------
-------------------------------------------------------------------------- Send Copy Case Request Workflow(10-11-2023)


-----------------------------------------------------------
--------------------------------------------------------------------
-------------------------------------------------------------------------- Send Copy Case File Workflow(13-11-2023)
SET IDENTITY_INSERT WF_TRIGGER_PR_LKP ON
INSERT INTO WF_TRIGGER_PR_LKP ([ModuleTriggerId] ,[Name],[IsConditional],[IsOptional]) VALUES(14,'Send_Copy_Case_File',0,0)
SET IDENTITY_INSERT WF_TRIGGER_PR_LKP OFF
-------------------------
INSERT INTO WF_TRIGGER_ACTIVITY_PR(TriggerId , ActivityId) VALUES(14,30)
INSERT INTO WF_TRIGGER_ACTIVITY_PR(TriggerId , ActivityId) VALUES(14,31)
INSERT INTO WF_TRIGGER_ACTIVITY_PR(TriggerId , ActivityId) VALUES(14,32)
INSERT INTO WF_TRIGGER_ACTIVITY_PR(TriggerId , ActivityId) VALUES(14,33)
INSERT INTO WF_TRIGGER_ACTIVITY_PR(TriggerId , ActivityId) VALUES(14,34)
INSERT INTO WF_TRIGGER_ACTIVITY_PR(TriggerId , ActivityId) VALUES(14,35)
----------------------

INSERT INTO WF_TRIGGER_OPTIONS_PR (TriggerId , OptionId) VALUES(14,512)	
INSERT INTO WF_TRIGGER_OPTIONS_PR (TriggerId , OptionId) VALUES(14,1024)	
INSERT INTO WF_TRIGGER_OPTIONS_PR (TriggerId , OptionId) VALUES(14,2048)	
INSERT INTO WF_TRIGGER_OPTIONS_PR (TriggerId , OptionId) VALUES(14,4096)	
INSERT INTO WF_TRIGGER_OPTIONS_PR (TriggerId , OptionId) VALUES(14,8192)	
---------------------
Insert INTO WF_SUBMODULE_TRIGGER_PR (Id, WorkflowSubModuleId , ModuleTriggerId) VALUES (36,1,14)
Insert INTO WF_SUBMODULE_TRIGGER_PR (Id, WorkflowSubModuleId , ModuleTriggerId) VALUES (37,2,14)


-----------------------------------------------------------
--------------------------------------------------------------------
----------------
SET IDENTITY_INSERT WF_ACTIVITY_PR_LKP ON
INSERT [dbo].WF_ACTIVITY_PR_LKP([ActivityId],[Name],[Class],[Method],[CategoryId],[AKey],[IsEndofFlow]) VALUES (37,N'Initiator Document Modification','WorkflowImplementationService','InitiatorDocument_Modification',2,'InitiatorDocumentModification',0)
SET IDENTITY_INSERT WF_ACTIVITY_PR_LKP OFF
-------------------
SET IDENTITY_INSERT WF_TRIGGER_ACTIVITY_PR ON
INSERT INTO WF_TRIGGER_ACTIVITY_PR([Id],[TriggerId],[ActivityId]) VALUES (73,6,37)
SET IDENTITY_INSERT WF_TRIGGER_ACTIVITY_PR OFF
--------------------
SET IDENTITY_INSERT WF_PARAMETER_PR_LKP ON
INSERT [dbo].WF_PARAMETER_PR_LKP([ParameterId],[Name], [PKey],[Mandatory],[IsAutoPopulated]) VALUES (60,N'User', N'InitiatorDocumentModification_User',0,0)
SET IDENTITY_INSERT WF_PARAMETER_PR_LKP OFF
------------------------
SET IDENTITY_INSERT WF_PARAMETER_PR_LKP ON
INSERT [dbo].WF_PARAMETER_PR_LKP([ParameterId],[Name], [PKey],[Mandatory],[IsAutoPopulated]) VALUES (61,N'User Role', N'InitiatorDocumentModification_UserRole',0,0)
SET IDENTITY_INSERT WF_PARAMETER_PR_LKP OFF
-------------------
INSERT [dbo].WF_ACTIVITY_PARAMETERS_PR([ParameterId],[ActivityId]) VALUES (60,37)
INSERT [dbo].WF_ACTIVITY_PARAMETERS_PR ([ParameterId],[ActivityId]) VALUES (61,37)
-----------
begin tran 
update WF_ACTIVITY_PR_LKP set Name=N'Initiator_Document_Modification' where ActivityId=37
commit
-----------
begin tran 
update WF_ACTIVITY_PR_LKP set Name=N'Publish_DMS_Document_EndFlow' where ActivityId=17
commit
----------
Update WF_SUBMODULE_PR_LKP Set Name_En ='LegalLegislations', Name_Ar = 'LegalLegislations'  where Id = 9
--------------------------------------------------------Trigger Options 16/11/23

INSERT INTO WF_OPTIONS_PR_LKP (ModuleOptionId, Name) VALUES ((65536*2),'Send To FP')

SET IDENTITY_INSERT WF_TRIGGER_OPTIONS_PR ON
INSERT INTO WF_TRIGGER_OPTIONS_PR ([Id],[OptionId],[TriggerId]) VALUES (46,131072,7)
INSERT INTO WF_TRIGGER_OPTIONS_PR ([Id],[OptionId],[TriggerId]) VALUES (47,131072,8)
INSERT INTO WF_TRIGGER_OPTIONS_PR ([Id],[OptionId],[TriggerId]) VALUES (48,131072,9)
INSERT INTO WF_TRIGGER_OPTIONS_PR ([Id],[OptionId],[TriggerId]) VALUES (49,131072,10)
SET IDENTITY_INSERT WF_TRIGGER_OPTIONS_PR OFF 
--------------------------------------------------------------------- Send Copy Case File Workflow(13-11-2023)
--------------------------------------------------------Trigger Options 16/11/23

INSERT INTO WF_OPTIONS_PR_LKP (ModuleOptionId, Name) VALUES ((65536*2),'Send To FP')

SET IDENTITY_INSERT WF_TRIGGER_OPTIONS_PR ON
INSERT INTO WF_TRIGGER_OPTIONS_PR ([Id],[OptionId],[TriggerId]) VALUES (46,131072,7)
INSERT INTO WF_TRIGGER_OPTIONS_PR ([Id],[OptionId],[TriggerId]) VALUES (47,131072,8)
INSERT INTO WF_TRIGGER_OPTIONS_PR ([Id],[OptionId],[TriggerId]) VALUES (48,131072,9)
INSERT INTO WF_TRIGGER_OPTIONS_PR ([Id],[OptionId],[TriggerId]) VALUES (49,131072,10)
SET IDENTITY_INSERT WF_TRIGGER_OPTIONS_PR OFF
-----------
SET IDENTITY_INSERT WF_ACTIVITY_PR_LKP ON
INSERT [dbo].WF_ACTIVITY_PR_LKP([ActivityId],[Name],[Class],[Method],[CategoryId],[AKey],[IsEndofFlow]) VALUES (37,N'Initiator Document Modification','WorkflowImplementationService','InitiatorDocument_Modification',2,'InitiatorDocumentModification',0)
SET IDENTITY_INSERT WF_ACTIVITY_PR_LKP OFF
-------------------
SET IDENTITY_INSERT WF_TRIGGER_ACTIVITY_PR ON
INSERT INTO WF_TRIGGER_ACTIVITY_PR([Id],[TriggerId],[ActivityId]) VALUES (73,6,37)
SET IDENTITY_INSERT WF_TRIGGER_ACTIVITY_PR OFF
--------------------
SET IDENTITY_INSERT WF_PARAMETER_PR_LKP ON
INSERT [dbo].WF_PARAMETER_PR_LKP([ParameterId],[Name], [PKey],[Mandatory],[IsAutoPopulated]) VALUES (60,N'User', N'InitiatorDocumentModification_User',0,0)
SET IDENTITY_INSERT WF_PARAMETER_PR_LKP OFF
------------------------
SET IDENTITY_INSERT WF_PARAMETER_PR_LKP ON
INSERT [dbo].WF_PARAMETER_PR_LKP([ParameterId],[Name], [PKey],[Mandatory],[IsAutoPopulated]) VALUES (61,N'User Role', N'InitiatorDocumentModification_UserRole',0,0)
SET IDENTITY_INSERT WF_PARAMETER_PR_LKP OFF
-------------------
INSERT [dbo].WF_ACTIVITY_PARAMETERS_PR([ParameterId],[ActivityId]) VALUES (60,37)
INSERT [dbo].WF_ACTIVITY_PARAMETERS_PR ([ParameterId],[ActivityId]) VALUES (61,37)
-----------
begin tran 
update WF_ACTIVITY_PR_LKP set Name=N'Initiator_Document_Modification' where ActivityId=37
commit
-----------
begin tran 
update WF_ACTIVITY_PR_LKP set Name=N'Publish_DMS_Document_EndFlow' where ActivityId=17
commit
----------Consultation Draft------

SET IDENTITY_INSERT WF_TRIGGER_ACTIVITY_PR ON
INSERT INTO WF_TRIGGER_ACTIVITY_PR([Id],[TriggerId],[ActivityId]) VALUES (74,5,8)
SET IDENTITY_INSERT WF_TRIGGER_ACTIVITY_PR OFF

SET IDENTITY_INSERT WF_TRIGGER_ACTIVITY_PR ON
INSERT INTO WF_TRIGGER_ACTIVITY_PR([Id],[TriggerId],[ActivityId]) VALUES (75,5,9)
SET IDENTITY_INSERT WF_TRIGGER_ACTIVITY_PR OFF

SET IDENTITY_INSERT WF_TRIGGER_ACTIVITY_PR ON
INSERT INTO WF_TRIGGER_ACTIVITY_PR([Id],[TriggerId],[ActivityId]) VALUES (76,5,15)
SET IDENTITY_INSERT WF_TRIGGER_ACTIVITY_PR OFF

SET IDENTITY_INSERT WF_TRIGGER_ACTIVITY_PR ON
INSERT INTO WF_TRIGGER_ACTIVITY_PR([Id],[TriggerId],[ActivityId]) VALUES (77,5,16)
SET IDENTITY_INSERT WF_TRIGGER_ACTIVITY_PR OFF

SET IDENTITY_INSERT WF_TRIGGER_ACTIVITY_PR ON
INSERT INTO WF_TRIGGER_ACTIVITY_PR([Id],[TriggerId],[ActivityId]) VALUES (78,5,28)
SET IDENTITY_INSERT WF_TRIGGER_ACTIVITY_PR OFF

SET IDENTITY_INSERT WF_TRIGGER_ACTIVITY_PR ON
INSERT INTO WF_TRIGGER_ACTIVITY_PR([Id],[TriggerId],[ActivityId]) VALUES (79,5,29)
SET IDENTITY_INSERT WF_TRIGGER_ACTIVITY_PR OFF

SET IDENTITY_INSERT WF_TRIGGER_ACTIVITY_PR ON
INSERT INTO WF_TRIGGER_ACTIVITY_PR([Id],[TriggerId],[ActivityId]) VALUES (80,5,27)
SET IDENTITY_INSERT WF_TRIGGER_ACTIVITY_PR OFF
-------------
delete from WF_TRIGGER_ACTIVITY_PR where id=10
delete from WF_TRIGGER_ACTIVITY_PR where id=11
---------------------------
----------
Update WF_SUBMODULE_PR_LKP Set Name_En ='LegalLegislations', Name_Ar ='LegalLegislations' where Id = 9

--------------------------------------------------------Trigger Options 16/11/23

INSERT INTO WF_OPTIONS_PR_LKP (ModuleOptionId, Name) VALUES ((65536*2),'Send To FP')

SET IDENTITY_INSERT WF_TRIGGER_OPTIONS_PR ON
INSERT INTO WF_TRIGGER_OPTIONS_PR ([Id],[OptionId],[TriggerId]) VALUES (46,131072,7)
INSERT INTO WF_TRIGGER_OPTIONS_PR ([Id],[OptionId],[TriggerId]) VALUES (47,131072,8)
INSERT INTO WF_TRIGGER_OPTIONS_PR ([Id],[OptionId],[TriggerId]) VALUES (48,131072,9)
INSERT INTO WF_TRIGGER_OPTIONS_PR ([Id],[OptionId],[TriggerId]) VALUES (49,131072,10)
SET IDENTITY_INSERT WF_TRIGGER_OPTIONS_PR OFF
-------------------
INSERT [dbo].[WF_TRIGGER_CONDITION_PR] ([TriggerId], [ConditionId]) VALUES (5, 163)
INSERT [dbo].[WF_TRIGGER_CONDITION_PR] ([TriggerId], [ConditionId]) VALUES (5, 164)
INSERT [dbo].[WF_TRIGGER_CONDITION_PR] ([TriggerId], [ConditionId]) VALUES (5, 165)
INSERT [dbo].[WF_TRIGGER_CONDITION_PR] ([TriggerId], [ConditionId]) VALUES (5, 166)
INSERT [dbo].[WF_TRIGGER_CONDITION_PR] ([TriggerId], [ConditionId]) VALUES (5, 167)
------------
INSERT INTO [dbo].[UMS_GROUP_CLAIMS] ([GroupId],[ClaimType],[ClaimValue])
VALUES ('E5E9AFCE-3123-48E5-A0CC-EAEC678AA179', 'Permission', 'Permissions.CMS.DraftDocument.Create')

INSERT INTO [dbo].[UMS_GROUP_CLAIMS] ([GroupId],[ClaimType],[ClaimValue])
VALUES ('E5E9AFCE-3123-48E5-A0CC-EAEC678AA179', 'Permission', 'Permissions.CMS.DraftDocument.Create')

INSERT INTO [dbo].[UMS_GROUP_CLAIMS] ([GroupId],[ClaimType],[ClaimValue])
VALUES ('93C1C1D4-73FE-4944-8F3E-8B9B983646F8', 'Permission', 'Permissions.CMS.DraftDocument.View')

INSERT INTO [dbo].[UMS_GROUP_CLAIMS] ([GroupId],[ClaimType],[ClaimValue])
VALUES ('E5E9AFCE-3123-48E5-A0CC-EAEC678AA179', 'Permission', 'Permissions.CMS.DraftDocument.View')

INSERT INTO [dbo].[UMS_GROUP_CLAIMS] ([GroupId],[ClaimType],[ClaimValue])
VALUES ('85D6848C-3BF1-403E-9CF6-B749D197ED15', 'Permission', 'Permissions.CMS.DraftDocument.View')
------------------------Workflow Edit
INSERT INTO UMS_GROUP_CLAIMS (GroupId , ClaimType ,ClaimValue) VALUES('BC55DCCF-B4AB-4203-BBC1-97EF566FF621','Permission','Permissions.Workflow.Edit')
-----------------------SLA Action Parameter
UPDATE SLA_ACTION_PARAMETERS set ParameterId = 8 where ActionId = 1
insert into tTranslation ([Url],[TranslationKey],[TranslationType],[Value_Ar],[Value_En],[ReferenceScreen],[CreateDate],[UpdateDate])
values ('Script','Case_History_Transfer_History',0,N'Case History & Transfer History',N'Case History & Transfer History','Case Management',GetDate(),GetDate())
--------------

/*<History Author='Umer Zaman' Date='24-12-2023'> Meeting Script start </History>*/
INSERT INTO MEET_MEETING_STATUS (MeetingStatusId, NameEn, NameAr) VALUES (1024, N'On Hold', N'في الانتظار')

-------------

INSERT INTO MEET_MEETING_STATUS VALUES (2048, 'Approved', N'موافق عليه')



/*<History Author='Umer Zaman' Date='24-12-2023'> Meeting Script end </History>*/
INSERT INTO UMS_GROUP_CLAIMS VALUES ((SELECT GroupId FROM UMS_GROUP WHERE Name_En = LOWER('hos')), 'Permission', 'Permissions.Submenu.CMS.LawyerChamberAssignment')
INSERT INTO UMS_GROUP_CLAIMS VALUES ((SELECT GroupId FROM UMS_GROUP WHERE Name_En = LOWER('hos')), 'Permission', 'Permissions.Submenu.CMS.LawyerChamberAssignment.List')
UPDATE SLA_ACTION_PARAMETERS set ParameterId = 8 where ActionId = 1
----------
INSERT INTO [dbo].[UMS_GROUP_CLAIMS] ([GroupId],[ClaimType],[ClaimValue])
VALUES ('49744B64-2399-4097-94FD-326D6FCE2626', 'Permission', 'Permissions.DMS.Document.View')
-------------
INSERT [dbo].tTranslation ([Url],[TranslationKey],[TranslationType],[Value_Ar],[Value_En],[ReferenceScreen],[CreateDate],[UpdateDate])
VALUES ('Script',N'Published_DMS_Document',0,N'Published DMS Document','Published DMS Document','Workflow',GETDATE(),GETDATE())

INSERT [dbo].tTranslation ([Url],[TranslationKey],[TranslationType],[Value_Ar],[Value_En],[ReferenceScreen],[CreateDate],[UpdateDate])
VALUES ('Script',N'Published_Draft_Document',0,N'Published Draft Document','Published Draft Document','Workflow',GETDATE(),GETDATE())
-------------
INSERT INTO [dbo].[UMS_GROUP_CLAIMS] ([GroupId],[ClaimType],[ClaimValue])
VALUES ('49744B64-2399-4097-94FD-326D6FCE2626', 'Permission', 'Permissions.DMS.Document.Add')
---------------
--------------------------------------Workflow options table updated 14/12/23 
UPDATE WF_OPTIONS_PR_LKP SET Name = N'SendToFP' WHERE ModuleOptionId = 131072

INSERT INTO [dbo].[WF_OPTIONS_PR_LKP]([ModuleOptionId],[Name])
     VALUES ((2*131072),N'SendToPOS')

INSERT INTO [dbo].[UMS_USER_CLAIMS] ([UserId],[ClaimType],[ClaimValue])
VALUES ('e75cba83-d6cd-4117-887f-eb6421082d42', 'Permission', 'Permissions.CMS.CaseFile.View')
INSERT INTO [dbo].[UMS_USER_CLAIMS] ([UserId],[ClaimType],[ClaimValue])
VALUES ('c604e117-9cca-4e88-9b99-f59faef3f509', 'Permission', 'Permissions.CMS.CaseFile.View')
INSERT INTO [dbo].[UMS_USER_CLAIMS] ([UserId],[ClaimType],[ClaimValue])
VALUES ('c604e117-9cca-4e88-9b99-f59faef3f509', 'Permission', 'Permissions.CMS.DraftDocument.View')
INSERT INTO [dbo].[UMS_USER_CLAIMS] ([UserId],[ClaimType],[ClaimValue])
VALUES ('c604e117-9cca-4e88-9b99-f59faef3f509', 'Permission', 'Permissions.CMS.DraftDocument.Create')

INSERT INTO [dbo].[WF_TRIGGER_OPTIONS_PR] ([TriggerId],[OptionId])
VALUES (9,262144)
INSERT INTO [dbo].[WF_TRIGGER_OPTIONS_PR] ([TriggerId],[OptionId])
VALUES (10,262144)
INSERT INTO [dbo].[WF_TRIGGER_OPTIONS_PR] ([TriggerId],[OptionId])
VALUES (7,262144)
INSERT INTO [dbo].[WF_TRIGGER_OPTIONS_PR] ([TriggerId],[OptionId])
VALUES (8,262144)

INSERT INTO [dbo].[WF_OPTIONS_PR_LKP]([ModuleOptionId],[Name])
     VALUES ((2*262144),N'TransferToSector')

INSERT INTO [dbo].[WF_TRIGGER_OPTIONS_PR] ([TriggerId],[OptionId])
VALUES (9,524288)
INSERT INTO [dbo].[WF_TRIGGER_OPTIONS_PR] ([TriggerId],[OptionId])
VALUES (10,524288)
INSERT INTO [dbo].[WF_TRIGGER_OPTIONS_PR] ([TriggerId],[OptionId])
VALUES (7,524288)
INSERT INTO [dbo].[WF_TRIGGER_OPTIONS_PR] ([TriggerId],[OptionId])
VALUES (8,524288)
INSERT INTO [dbo].[WF_TRIGGER_OPTIONS_PR] ([TriggerId],[OptionId])
VALUES (11,524288)
INSERT INTO [dbo].[WF_TRIGGER_OPTIONS_PR] ([TriggerId],[OptionId])
VALUES (12,524288)

INSERT INTO [dbo].[WF_SUBMODULE_TRIGGER_PR] ([Id],[WorkflowSubModuleId],[ModuleTriggerId])
VALUES (42,5, 13)
INSERT INTO [dbo].[WF_SUBMODULE_TRIGGER_PR] ([Id],[WorkflowSubModuleId],[ModuleTriggerId])
VALUES (43,5, 14)
INSERT INTO [dbo].[WF_SUBMODULE_TRIGGER_PR] ([Id],[WorkflowSubModuleId],[ModuleTriggerId])
VALUES (44,6, 13)
INSERT INTO [dbo].[WF_SUBMODULE_TRIGGER_PR] ([Id],[WorkflowSubModuleId],[ModuleTriggerId])
VALUES (45,6, 14)
INSERT INTO [dbo].[WF_SUBMODULE_TRIGGER_PR] ([Id],[WorkflowSubModuleId],[ModuleTriggerId])
VALUES (46,7, 13)
INSERT INTO [dbo].[WF_SUBMODULE_TRIGGER_PR] ([Id],[WorkflowSubModuleId],[ModuleTriggerId])
VALUES (47,7, 14)

DELETE FROM WF_TRIGGER_ACTIVITY_PR WHERE TriggerId = 9 AND ActivityId = 20
DELETE FROM WF_TRIGGER_ACTIVITY_PR WHERE TriggerId = 10 AND ActivityId = 20
DELETE FROM WF_TRIGGER_ACTIVITY_PR WHERE TriggerId = 13 AND ActivityId = 20
DELETE FROM WF_TRIGGER_ACTIVITY_PR WHERE TriggerId = 14 AND ActivityId = 20

DELETE FROM WF_TRIGGER_OPTIONS_PR WHERE TriggerId = 9 AND OptionId = 8
DELETE FROM WF_TRIGGER_OPTIONS_PR WHERE TriggerId = 10 AND OptionId = 8
DELETE FROM WF_TRIGGER_OPTIONS_PR WHERE TriggerId = 13 AND OptionId = 8
DELETE FROM WF_TRIGGER_OPTIONS_PR WHERE TriggerId = 14 AND OptionId = 8

INSERT INTO [dbo].[WF_TRIGGER_ACTIVITY_PR] ([TriggerId],[ActivityId])
VALUES (13 ,18)
INSERT INTO [dbo].[WF_TRIGGER_ACTIVITY_PR] ([TriggerId],[ActivityId])
VALUES (13 ,24)
INSERT INTO [dbo].[WF_TRIGGER_ACTIVITY_PR] ([TriggerId],[ActivityId])
VALUES (13 ,23)
INSERT INTO [dbo].[WF_TRIGGER_ACTIVITY_PR] ([TriggerId],[ActivityId])
VALUES (13 ,36)


INSERT INTO [dbo].[WF_TRIGGER_OPTIONS_PR]([TriggerId],[OptionId])
VALUES (13,524288)
INSERT INTO [dbo].[WF_TRIGGER_OPTIONS_PR]([TriggerId],[OptionId])
VALUES (13,65536)
INSERT INTO [dbo].[WF_TRIGGER_OPTIONS_PR]([TriggerId],[OptionId])
VALUES (13,32768)
INSERT INTO [dbo].[WF_TRIGGER_OPTIONS_PR]([TriggerId],[OptionId])
VALUES (13,16384)
---------------
UPDATE SLA_ACTION_PARAMETERS set ParameterId = 8 where ActionId = 1

------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

---------------------------------------------------Workflow Modifications In Activities

----------------------------------------------------
----------------------------------------------------12-13-2023
-----------------------------------------------------
-----------------------------UMS_ROLE
INSERT INTO UMS_ROLE (Id,Name,NormalizedName,ConcurrencyStamp,Description_En,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,DeletedBy,DeletedDate,IsDeleted,Description_Ar,NameAr)
VALUES('9A203FC5-F85D-423D-B57C-2AC9DDDAEE57' ,'LPS Editor','LPS EDITOR','3627d697-6b94-4348-9f8d-1a9aa0c7307c','This user has the rights to add, update, delete, review, publish, unpublish principles. This user is having most of the permissions except users and roles management.','superadmin@gmail.com','2022-09-20 14:52:59.263',NULL,NULL,NULL,NULL,0,N'This user has the rights to add, update, delete, review, publish, unpublish documents. This user is having most of the permissions except users and roles management.',NULL)
------------------------------UMS_USER_ROLES
INSERT INTO UMS_USER_ROLES (UserId , RoleId) VALUES ('c6d85bfc-d217-402a-b335-aab4c5ec1181' ,'9A203FC5-F85D-423D-B57C-2AC9DDDAEE57')

-----------------------------------------UMS_USER_ROLES
------------Legislation Roles
INSERT INTO UMS_USER_ROLES (UserId , RoleId) VALUES ('0226a7d2-1d08-4348-b94c-20ae662fff19' ,'cccb3715-0e55-4a1c-a7e2-c12fd20d7a4a')
INSERT INTO UMS_USER_ROLES (UserId , RoleId) VALUES ('3ef075b8-4433-40af-a294-7316afa3252a' ,'467af2c7-7bfa-4cf7-bbe8-8198a3340497')
INSERT INTO UMS_USER_ROLES (UserId , RoleId) VALUES ('4e3ff8c9-2bca-40b3-839c-a3b8a3af217e' ,'cccb3715-0e55-4a1c-a7e2-c12fd20d7a4a')

------------Principle Roles

INSERT INTO UMS_USER_ROLES (UserId , RoleId) VALUES ('1dece812-6b0c-4848-a199-fde1158db48d' ,'a484bb1b-0423-4e12-a3c6-f347a0236d85')
INSERT INTO UMS_USER_ROLES (UserId , RoleId) VALUES ('3ef075b8-4433-40af-a294-7316afa3252a' ,'9A203FC5-F85D-423D-B57C-2AC9DDDAEE57')
INSERT INTO UMS_USER_ROLES (UserId , RoleId) VALUES ('60406c1a-da9d-4122-b55c-b5e907ab69bf' ,'fca2fe9b-aba5-416d-bb49-e81819fe563b')

------------------Principle Contributor Role
INSERT INTO UMS_ROLE (Id,Name,NormalizedName,ConcurrencyStamp,Description_En,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,DeletedBy,DeletedDate,IsDeleted,Description_Ar,NameAr)
VALUES('B6FE35A6-6A6B-44FD-A70A-B65C88B23DF1' ,'LPS Contributor','LPS Contributor','3627d697-6b94-4348-9f8d-1a9aa0c7307c','This user has the rights to add, update, delete, review, publish, unpublish principles. This user is having most of the permissions except users and roles management.','superadmin@gmail.com','2022-09-20 14:52:59.263',NULL,NULL,NULL,NULL,0,N'This user has the rights to add, update, delete, review, publish, unpublish documents. This user is having most of the permissions except users and roles management.',NULL)

--------------------Legislation Contributor Role
INSERT INTO UMS_ROLE (Id,Name,NormalizedName,ConcurrencyStamp,Description_En,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,DeletedBy,DeletedDate,IsDeleted,Description_Ar,NameAr)
VALUES('279E1469-599D-4D56-944A-DA3E6A55148F' ,'LDS Contributor','LDS Contributor','3627d697-6b94-4348-9f8d-1a9aa0c7307c','This user has the rights to add, update, delete, review, publish, unpublish legislation. This user is having most of the permissions except users and roles management.','superadmin@gmail.com','2022-09-20 14:52:59.263',NULL,NULL,NULL,NULL,0,N'This user has the rights to add, update, delete, review, publish, unpublish documents. This user is having most of the permissions except users and roles management.',NULL)

-------------------UMS_USER_ROLES
INSERT INTO UMS_USER_ROLES (UserId , RoleId) VALUES ('9f8c7796-bc30-4a6c-a4ac-70bf63a813e1' ,'279E1469-599D-4D56-944A-DA3E6A55148F')
INSERT INTO UMS_USER_ROLES (UserId , RoleId) VALUES ('4875d9d6-3c51-4191-aaf6-9f1231c51be8' ,'B6FE35A6-6A6B-44FD-A70A-B65C88B23DF1')
----------------------WF_ACTIVITY_PARAMETERS_PR

Delete WF_ACTIVITY_PARAMETERS_PR where ParameterId = 1 and ActivityId = 1
Delete WF_ACTIVITY_PARAMETERS_PR where ParameterId = 3 and ActivityId = 2
Delete WF_ACTIVITY_PARAMETERS_PR where ParameterId = 8 and ActivityId = 6
Delete WF_ACTIVITY_PARAMETERS_PR where ParameterId = 10 and ActivityId = 7

----------------------------------------------------
----------------------------------------------------12-14-2023
-----------------------------------------------------
------------------------WF_SUBMODULE_TRIGGER_PR

INSERT INTO WF_SUBMODULE_TRIGGER_PR (Id,WorkflowSubModuleId , ModuleTriggerId) VALUES (38,1,15)
INSERT INTO WF_SUBMODULE_TRIGGER_PR (Id,WorkflowSubModuleId , ModuleTriggerId) VALUES (39,1,16)
INSERT INTO WF_SUBMODULE_TRIGGER_PR (Id,WorkflowSubModuleId , ModuleTriggerId) VALUES (40,2,15)
INSERT INTO WF_SUBMODULE_TRIGGER_PR (Id,WorkflowSubModuleId , ModuleTriggerId) VALUES (41,2,16)
-----------------------WF_SUBMODULE_TRIGGER_PR
Update WF_SUBMODULE_TRIGGER_PR SET WorkflowSubModuleId = 3 where  WorkflowSubModuleId = 1 And ModuleTriggerId = 13
Update WF_SUBMODULE_TRIGGER_PR SET WorkflowSubModuleId = 4 where  WorkflowSubModuleId = 2 And ModuleTriggerId = 13
Update WF_SUBMODULE_TRIGGER_PR SET WorkflowSubModuleId = 3 where  WorkflowSubModuleId = 1 And ModuleTriggerId = 14
Update WF_SUBMODULE_TRIGGER_PR SET WorkflowSubModuleId = 4 where  WorkflowSubModuleId = 2 And ModuleTriggerId = 14
-----------------------WF_SUBMODULE_TRIGGER_PR
Insert INTO WF_SUBMODULE_TRIGGER_PR (Id, WorkflowSubModuleId , ModuleTriggerId) VALUES (42,5,13)
Insert INTO WF_SUBMODULE_TRIGGER_PR (Id, WorkflowSubModuleId , ModuleTriggerId) VALUES (43,5,14)

Insert INTO WF_SUBMODULE_TRIGGER_PR (Id, WorkflowSubModuleId , ModuleTriggerId) VALUES (44,6,13)
Insert INTO WF_SUBMODULE_TRIGGER_PR (Id, WorkflowSubModuleId , ModuleTriggerId) VALUES (45,6,14)

Insert INTO WF_SUBMODULE_TRIGGER_PR (Id, WorkflowSubModuleId , ModuleTriggerId) VALUES (46,7,13)
Insert INTO WF_SUBMODULE_TRIGGER_PR (Id, WorkflowSubModuleId , ModuleTriggerId) VALUES (47,7,14)

----------------------WF_SUBMODULE_PR_LKP
Delete WF_SUBMODULE_PR_LKP Where Id = 8 

----------------------WF_TRIGGER_PR_LKP

Update WF_TRIGGER_PR_LKP Set IsOptional = 1 where ModuleTriggerId =13
Update WF_TRIGGER_PR_LKP Set IsOptional = 1 where ModuleTriggerId =14
Update WF_TRIGGER_PR_LKP Set IsOptional = 1 where ModuleTriggerId =15
Update WF_TRIGGER_PR_LKP Set IsOptional = 1 where ModuleTriggerId =16


------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
---------------------------------------------------Workflow Modifications In Activities



-----------------
ALTER TABLE COMM_COMMUNICATION_MEETING
ADD Location nvarchar(150) null;

ALTER TABLE COMM_COMMUNICATION_MEETING
ADD MeetingLink nvarchar(150) null;

ALTER TABLE COMM_COMMUNICATION_MEETING
ADD MeetingPassword nvarchar(150) null;

ALTER TABLE COMM_COMMUNICATION_MEETING
ADD IsOnline bit null;

ALTER TABLE COMM_COMMUNICATION_MEETING
ADD RequirePassword bit null;


 ALTER TABLE LINK_TARGET
ALTER COLUMN ReferenceId uniqueidentifier null;

INSERT INTO MEET_MEETING_STATUS (MeetingStatusId, NameEn, NameAr)
VALUES (512, 'Save As Draft', N'حفظ كمسودة')

-----------------------------19/12/23 
INSERT INTO [dbo].[WF_TRIGGER_OPTIONS_PR]([TriggerId],[OptionId])
VALUES (14,524288)
INSERT INTO [dbo].[WF_TRIGGER_OPTIONS_PR]([TriggerId],[OptionId])
VALUES (14,65536)
INSERT INTO [dbo].[WF_TRIGGER_OPTIONS_PR]([TriggerId],[OptionId])
VALUES (14,32768)
INSERT INTO [dbo].[WF_TRIGGER_OPTIONS_PR]([TriggerId],[OptionId])
VALUES (14,16384)

INSERT INTO [dbo].[WF_TRIGGER_ACTIVITY_PR] ([TriggerId],[ActivityId])
VALUES (14 ,18)
INSERT INTO [dbo].[WF_TRIGGER_ACTIVITY_PR] ([TriggerId],[ActivityId])
VALUES (14 ,24)
INSERT INTO [dbo].[WF_TRIGGER_ACTIVITY_PR] ([TriggerId],[ActivityId])
VALUES (14 ,23)
INSERT INTO [dbo].[WF_TRIGGER_ACTIVITY_PR] ([TriggerId],[ActivityId])
VALUES (14 ,36)


---------------------


SET IDENTITY_INSERT WF_TRIGGER_PR_LKP ON
INSERT INTO WF_TRIGGER_PR_LKP (ModuleTriggerId,Name,IsConditional,IsOptional)
values (17,'Transfer_Confidential_Case_Request_Private_Office',0,1)
SET IDENTITY_INSERT WF_TRIGGER_PR_LKP OFF

SET IDENTITY_INSERT WF_TRIGGER_PR_LKP ON
INSERT INTO WF_TRIGGER_PR_LKP (ModuleTriggerId,Name,IsConditional,IsOptional)
values (18,'Transfer_Confidential_Consultation_Request_Private_Office',0,1)
SET IDENTITY_INSERT WF_TRIGGER_PR_LKP OFF

SET IDENTITY_INSERT WF_ACTIVITY_PR_LKP ON
INSERT [dbo].WF_ACTIVITY_PR_LKP([ActivityId],[Name],[Class],[Method],[CategoryId],[AKey],[IsEndofFlow]) VALUES (38,N'Transfer_To_Respective_Sector_And_Endflow','WorkflowImplementationService','Cms_TransferToRespectiveSectorAndEndflow',2,'TransferToRespectiveSectorAndEndflow',1)
SET IDENTITY_INSERT WF_ACTIVITY_PR_LKP OFF



INSERT INTO WF_OPTIONS_PR_LKP (ModuleOptionId , Name) VALUES(524288*2,'RejectAndTransferToRespectiveSectorToEndFlow')

SET IDENTITY_INSERT WF_PARAMETER_PR_LKP ON
INSERT INTO WF_PARAMETER_PR_LKP (ParameterId,Name,PKey,Mandatory,IsAutoPopulated)
VALUES (62,'User','CmsTransferToRespectiveSectorAndEndflowUser',0,0)
SET IDENTITY_INSERT WF_PARAMETER_PR_LKP OFF


INSERT INTO WF_ACTIVITY_PARAMETERS_PR VALUES (62,38)


INSERT INTO WF_TRIGGER_ACTIVITY_PR VALUES (17,23)
INSERT INTO WF_TRIGGER_ACTIVITY_PR VALUES (17,36)
INSERT INTO WF_TRIGGER_ACTIVITY_PR VALUES (17,38)

INSERT INTO WF_TRIGGER_ACTIVITY_PR VALUES (18,23)
INSERT INTO WF_TRIGGER_ACTIVITY_PR VALUES (18,36)
INSERT INTO WF_TRIGGER_ACTIVITY_PR VALUES (18,38)


INSERT INTO WF_TRIGGER_OPTIONS_PR VALUES (17,65536)
INSERT INTO WF_TRIGGER_OPTIONS_PR VALUES (17,1048576)


INSERT INTO WF_TRIGGER_OPTIONS_PR VALUES (18,65536)
INSERT INTO WF_TRIGGER_OPTIONS_PR VALUES (18,1048576)


INSERT INTO WF_SUBMODULE_TRIGGER_PR VALUES (48,1,17)
INSERT INTO WF_SUBMODULE_TRIGGER_PR VALUES (49,2,17)

INSERT INTO WF_SUBMODULE_TRIGGER_PR VALUES (50,3,18)

INSERT INTO WF_SUBMODULE_TRIGGER_PR VALUES (51,4,18)

INSERT INTO WF_SUBMODULE_TRIGGER_PR VALUES (52,5,18)

INSERT INTO WF_SUBMODULE_TRIGGER_PR VALUES (53,6,18)

INSERT INTO WF_SUBMODULE_TRIGGER_PR VALUES (54,7,18)

UPDATE WF_ACTIVITY_PR_LKP SET Name = 'Transfer_To_PO_But_Send_TO_FP_For_Decision' , Method = 'Cms_TransferToPOButSendTOFPForDecision', AKey = 'CmsTransferToPOButSendTOFPForDecision' where ActivityId = 23

UPDATE WF_OPTIONS_PR_LKP SET Name = 'TransferToPOButSendTOFPForDecision' WHERE ModuleOptionId = '131072'

-----------------------20/12/2023
UPDATE MEET_MEETING_ATTENDEE_STATUS
SET NameEn = 'Accept', NameAr = N'Accept'
WHERE id=4;
-----------------------Dms_File_type(1/2/2024)
UPDATE DMS_FILE_TYPES_LKP SET IsActive = 1 
-----------------

Update DMS_FILE_TYPES_LKP  SET Type = '.pdf' where Id = 2
Update DMS_FILE_TYPES_LKP  SET Type ='.jpg' where Id = 4
Update DMS_FILE_TYPES_LKP  SET Type ='.jpeg' where Id = 8
Update DMS_FILE_TYPES_LKP  SET Type ='.png' where Id = 16
Update DMS_FILE_TYPES_LKP  SET Type ='.msg' where Id = 32


------------
begin tran
update WF_PARAMETER_PR_LKP set PKey='DMSDocumentReview_User' where ParameterId=22
commit

begin tran
update WF_PARAMETER_PR_LKP set PKey='DMSDocumentReview_UserRole' where ParameterId=23
commit

begin tran
update WF_PARAMETER_PR_LKP set PKey='DMSDocumentPublish_User' where ParameterId=30
commit

begin tran
update WF_PARAMETER_PR_LKP set PKey='DMSDocumentPublish_UserRole' where ParameterId=31
commit

begin tran 
update WF_ACTIVITY_PARAMETERS_PR set ParameterId=23 where ActivityId=12
commit
----------------
insert into tTranslation ([Url],[TranslationKey],[TranslationType],[Value_Ar],[Value_En],[ReferenceScreen],[CreateDate],[UpdateDate])
values ('Script','Remarks_for_Rejection',0,N'Remarks for Rejection',N'Remarks for Rejection','Draft Document Detail',GetDate(),GetDate())
--------------
Insert INTO WF_SUBMODULE_TRIGGER_PR (Id, WorkflowSubModuleId , ModuleTriggerId) VALUES (55,3,6)
Insert INTO WF_SUBMODULE_TRIGGER_PR (Id, WorkflowSubModuleId , ModuleTriggerId) VALUES (56,4,6)
Insert INTO WF_SUBMODULE_TRIGGER_PR (Id, WorkflowSubModuleId , ModuleTriggerId) VALUES (57,5,6)
Insert INTO WF_SUBMODULE_TRIGGER_PR (Id, WorkflowSubModuleId , ModuleTriggerId) VALUES (58,6,6)
Insert INTO WF_SUBMODULE_TRIGGER_PR (Id, WorkflowSubModuleId , ModuleTriggerId) VALUES (59,7,6)
--------------
INSERT INTO [dbo].[UMS_GROUP_CLAIMS] ([GroupId],[ClaimType],[ClaimValue])
VALUES ('E5E9AFCE-3123-48E5-A0CC-EAEC678AA179', 'Permission', 'Permissions.Submenu.Document.DocumentList')

INSERT INTO [dbo].[UMS_GROUP_CLAIMS] ([GroupId],[ClaimType],[ClaimValue])
VALUES ('93C1C1D4-73FE-4944-8F3E-8B9B983646F8', 'Permission', 'Permissions.Submenu.Document.DocumentList')

INSERT INTO [dbo].[UMS_GROUP_CLAIMS] ([GroupId],[ClaimType],[ClaimValue])
VALUES ('85D6848C-3BF1-403E-9CF6-B749D197ED15', 'Permission', 'Permissions.Submenu.Document.DocumentList')
------------------
INSERT INTO [dbo].[UMS_GROUP_CLAIMS] ([GroupId],[ClaimType],[ClaimValue])
VALUES ('E5E9AFCE-3123-48E5-A0CC-EAEC678AA179', 'Permission', 'Permissions.DMS.Document.Add')

INSERT INTO [dbo].[UMS_GROUP_CLAIMS] ([GroupId],[ClaimType],[ClaimValue])
VALUES ('93C1C1D4-73FE-4944-8F3E-8B9B983646F8', 'Permission', 'Permissions.DMS.Document.Add')

INSERT INTO [dbo].[UMS_GROUP_CLAIMS] ([GroupId],[ClaimType],[ClaimValue])
VALUES ('85D6848C-3BF1-403E-9CF6-B749D197ED15', 'Permission', 'Permissions.DMS.Document.Add')
--------------
INSERT INTO [dbo].[UMS_GROUP_CLAIMS] ([GroupId],[ClaimType],[ClaimValue])
VALUES ('E5E9AFCE-3123-48E5-A0CC-EAEC678AA179', 'Permission', 'Permissions.DMS.Document.View')

INSERT INTO [dbo].[UMS_GROUP_CLAIMS] ([GroupId],[ClaimType],[ClaimValue])
VALUES ('93C1C1D4-73FE-4944-8F3E-8B9B983646F8', 'Permission', 'Permissions.DMS.Document.View')

INSERT INTO [dbo].[UMS_GROUP_CLAIMS] ([GroupId],[ClaimType],[ClaimValue])
VALUES ('85D6848C-3BF1-403E-9CF6-B749D197ED15', 'Permission', 'Permissions.DMS.Document.View')
----------
INSERT INTO [dbo].[UMS_GROUP_CLAIMS] ([GroupId],[ClaimType],[ClaimValue])
VALUES ('49744B64-2399-4097-94FD-326D6FCE2626', 'Permission', 'Permissions.Submenu.Document.DocumentList')
------------
INSERT INTO [dbo].[UMS_USER_CLAIMS] ([UserId],[ClaimType],[ClaimValue])
VALUES ('0226a7d2-1d08-4348-b94c-20ae662fff19', 'Permission', 'Permissions.Submenu.Document.DocumentList')

INSERT INTO [dbo].[UMS_USER_CLAIMS] ([UserId],[ClaimType],[ClaimValue])
VALUES ('0226a7d2-1d08-4348-b94c-20ae662fff19', 'Permission', 'Permissions.DMS.Document.Add')

INSERT INTO [dbo].[UMS_USER_CLAIMS] ([UserId],[ClaimType],[ClaimValue])
VALUES ('0226a7d2-1d08-4348-b94c-20ae662fff19', 'Permission', 'Permissions.DMS.Document.View')
----------
INSERT INTO [dbo].[UMS_USER_CLAIMS] ([UserId],[ClaimType],[ClaimValue])
VALUES ('3ef075b8-4433-40af-a294-7316afa3252a', 'Permission', 'Permissions.Submenu.Document.DocumentList')

INSERT INTO [dbo].[UMS_USER_CLAIMS] ([UserId],[ClaimType],[ClaimValue])
VALUES ('3ef075b8-4433-40af-a294-7316afa3252a', 'Permission', 'Permissions.DMS.Document.Add')

INSERT INTO [dbo].[UMS_USER_CLAIMS] ([UserId],[ClaimType],[ClaimValue])
VALUES ('3ef075b8-4433-40af-a294-7316afa3252a', 'Permission', 'Permissions.DMS.Document.View')
------------
INSERT INTO [dbo].[UMS_USER_CLAIMS] ([UserId],[ClaimType],[ClaimValue])
VALUES ('4e3ff8c9-2bca-40b3-839c-a3b8a3af217e', 'Permission', 'Permissions.Submenu.Document.DocumentList')

INSERT INTO [dbo].[UMS_USER_CLAIMS] ([UserId],[ClaimType],[ClaimValue])
VALUES ('4e3ff8c9-2bca-40b3-839c-a3b8a3af217e', 'Permission', 'Permissions.DMS.Document.Add')

INSERT INTO [dbo].[UMS_USER_CLAIMS] ([UserId],[ClaimType],[ClaimValue])
VALUES ('4e3ff8c9-2bca-40b3-839c-a3b8a3af217e', 'Permission', 'Permissions.DMS.Document.View')
---------
INSERT INTO [dbo].[UMS_USER_CLAIMS] ([UserId],[ClaimType],[ClaimValue])
VALUES ('1dece812-6b0c-4848-a199-fde1158db48d', 'Permission', 'Permissions.Submenu.Document.DocumentList')

----------Ammaar Naveed 10/01/2024--------
INSERT INTO EP_STATUS(Name_En,Name_Ar)
VALUES ('Suspended','Suspended')

INSERT INTO [dbo].[UMS_USER_CLAIMS] ([UserId],[ClaimType],[ClaimValue])
VALUES ('1dece812-6b0c-4848-a199-fde1158db48d', 'Permission', 'Permissions.DMS.Document.Add')

INSERT INTO [dbo].[UMS_USER_CLAIMS] ([UserId],[ClaimType],[ClaimValue])
VALUES ('1dece812-6b0c-4848-a199-fde1158db48d', 'Permission', 'Permissions.DMS.Document.View')
------------
INSERT INTO [dbo].[UMS_USER_CLAIMS] ([UserId],[ClaimType],[ClaimValue])
VALUES ('c6d85bfc-d217-402a-b335-aab4c5ec1181', 'Permission', 'Permissions.Submenu.Document.DocumentList')

INSERT INTO [dbo].[UMS_USER_CLAIMS] ([UserId],[ClaimType],[ClaimValue])
VALUES ('c6d85bfc-d217-402a-b335-aab4c5ec1181', 'Permission', 'Permissions.DMS.Document.Add')

INSERT INTO [dbo].[UMS_USER_CLAIMS] ([UserId],[ClaimType],[ClaimValue])
VALUES ('c6d85bfc-d217-402a-b335-aab4c5ec1181', 'Permission', 'Permissions.DMS.Document.View')
-------
INSERT INTO [dbo].[UMS_USER_CLAIMS] ([UserId],[ClaimType],[ClaimValue])
VALUES ('60406c1a-da9d-4122-b55c-b5e907ab69bf', 'Permission', 'Permissions.Submenu.Document.DocumentList')

INSERT INTO [dbo].[UMS_USER_CLAIMS] ([UserId],[ClaimType],[ClaimValue])
VALUES ('60406c1a-da9d-4122-b55c-b5e907ab69bf', 'Permission', 'Permissions.DMS.Document.Add')

INSERT INTO [dbo].[UMS_USER_CLAIMS] ([UserId],[ClaimType],[ClaimValue])
VALUES ('60406c1a-da9d-4122-b55c-b5e907ab69bf', 'Permission', 'Permissions.DMS.Document.View')
------
insert into tTranslation ([Url],[TranslationKey],[TranslationType],[Value_Ar],[Value_En],[ReferenceScreen],[CreateDate],[UpdateDate])
values ('Script','Link_Document',0,N'Link Document',N'Link Document','Detail Document',GetDate(),GetDate())
-------
insert into tTranslation ([Url],[TranslationKey],[TranslationType],[Value_Ar],[Value_En],[ReferenceScreen],[CreateDate],[UpdateDate])
values ('Script','Document_Linked_Successfully',0,N'Document Linked Successfully',N'Document Linked Successfully','Link Document',GetDate(),GetDate())
---------

------------------Add New Attachment Type for LdsExternal Source -------------------
IF((SELECT COUNT(*) FROM ATTACHMENT_TYPE WHERE AttachmentTypeId = 95) > 0)
INSERT [dbo].[ATTACHMENT_TYPE] ([AttachmentTypeId], [Type_Ar], [Type_En], [ModuleId], [IsMandatory],[IsOfficialLetter]) VALUES (95, N'LdsExternalSource', N'LdsExternalSource', 1, 0,0)
GO
GO
------------
insert into tTranslation ([Url],[TranslationKey],[TranslationType],[Value_Ar],[Value_En],[ReferenceScreen],[CreateDate],[UpdateDate])
values ('Script','Take_a_Decision',0,N'Take a Decision',N'اتخاذ القرار','ReviewAssignmentCaseFile',GetDate(),GetDate())
--------
insert into tTranslation ([Url],[TranslationKey],[TranslationType],[Value_Ar],[Value_En],[ReferenceScreen],[CreateDate],[UpdateDate])
values ('Script','Review_DMS_Document_Notify',0,N'A Document has been sent for Review #entity#.',N'A Document has been sent for Review #entity#.','DocumentView',GetDate(),GetDate())
------------
--begin tran
--update tTranslation set Value_Ar=N'A Draft has been sent for Review #entity#.'
--where TranslationId=9832
--commit

--begin tran
--update tTranslation set Value_En=N'A Draft has been sent for Review #entity#.'
--where TranslationId=9832
--commit

--begin tran
--update tTranslation set Value_Ar=N'Draft has been Published now. #entity#.'
--where TranslationId=12561
--commit

--begin tran
--update tTranslation set Value_En=N'Draft has been Published now. #entity#.'
--where TranslationId=12561
--commit

--begin tran
--update tTranslation set Value_Ar=N'Document has been Published now. #entity#.'
--where TranslationId=12560
--commit

--begin tran
--update tTranslation set Value_En=N'Document has been Published now. #entity#.'
--where TranslationId=12560
--commit
----------------


----------------------------------------------------- QA Issues Working (1/8/2024)

Update CMS_CASE_PARTY_TYPE_G2G_LKP set Name_En = 'Government Entity' , Name_Ar = N'الجهة الحكومية' Where Id = 3
Update CMS_CASE_PARTY_TYPE_G2G_LKP set Name_Ar = N'فردي' Where Id = 1
Update CMS_CASE_PARTY_TYPE_G2G_LKP set Name_Ar = N'الشركة' Where Id = 2
Update tTranslation Set Value_En = 'Government Entity' where TranslationId =9862
Update CMS_CASE_FILE_STATUS_G2G_LKP Set Name_En = 'Rejected By Lawyer' , Name_Ar = N'Rejected By Lawyer' Where Id = 512
---------
INSERT [dbo].CMS_DRAFT_DOCUMENT_VERSION_STATUS ([Id],[NameEn],[NameAr]) VALUES (32,N'SendToMOJ',N'SendToMOJ')
INSERT [dbo].CMS_DRAFT_DOCUMENT_VERSION_STATUS ([Id],[NameEn],[NameAr]) VALUES (64,N'RegisteredInMOJ',N'RegisteredInMOJ')
----------

--------------------------------------------------[COMM_COMMUNICATION_SOURCE]

GO
INSERT [dbo].[COMM_COMMUNICATION_SOURCE] ([Id], [NameEn], [NameAr]) VALUES (1, N'FATWA', N'FATWA')
GO
INSERT [dbo].[COMM_COMMUNICATION_SOURCE] ([Id], [NameEn], [NameAr]) VALUES (2, N'G2G', N'G2G')
GO
INSERT [dbo].[COMM_COMMUNICATION_SOURCE] ([Id], [NameEn], [NameAr]) VALUES (4, N'Tarasul', N'Tarasul')
GO
INSERT [dbo].[COMM_COMMUNICATION_SOURCE] ([Id], [NameEn], [NameAr]) VALUES (8, N'Legal Announcement', N'Legal Announcement')
GO
---------------------------
SET IDENTITY_INSERT CMS_REGISTERED_CASE_STATUS_G2G_LKP ON
INSERT INTO CMS_REGISTERED_CASE_STATUS_G2G_LKP (Id, Name_En , Name_Ar,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,DeletedBy,DeletedDate,IsDeleted,IsActive)
VALUES(16,'Pending For GE Response',N'Pending For GE Response','fatwaadmin@gmail.com','2023-08-13 17:01:00.000',NULL,NULL,NULL,NULL,0,1)
SET IDENTITY_INSERT CMS_REGISTERED_CASE_STATUS_G2G_LKP OFF

--------Ammaar Naveed----17/01/2024
UPDATE EP_TYPE SET Name_En = 'Internal Employee' WHERE Id=1
UPDATE EP_TYPE SET Name_Ar = N'الموظف الداخلي' WHERE Id=1
UPDATE EP_TYPE SET Name_En = 'External Employee' WHERE Id=2
UPDATE EP_TYPE SET Name_Ar = N'الموظف الخارجي' WHERE Id=2
SET IDENTITY_INSERT CMS_REGISTERED_CASE_STATUS_G2G_LKP OFF
-----kay UMS_CLAIM______

INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Title_Ar],[Module],[SubModule],[ClaimType],[ClaimValue],[IsDeleted])
VALUES ('Main Document Menu','Main Document Menu', 'DMS', 'Menu', 'Permission', 'Permissions.DMS.Menu.Document',0)

INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Title_Ar],[Module],[SubModule],[ClaimType],[ClaimValue],[IsDeleted])
VALUES ('KAY Publications List','KAY Publications List', 'DMS', 'KAY', 'Permission', 'Permissions.DMS.KAY.DocumentList',0)
SET IDENTITY_INSERT CMS_REGISTERED_CASE_STATUS_G2G_LKP OFF

-----------------------------
SET IDENTITY_INSERT UMS_GROUP_CLAIMS ON
Insert into UMS_GROUP_CLAIMS (Id,GroupId,ClaimType,ClaimValue) VALUES(8115,'071C4BA0-CC3F-43DC-B2CB-6FC2BEA6D831','Permission','Permissions.Dashboard.Workflow.View')
SET IDENTITY_INSERT UMS_GROUP_CLAIMS OFF
SET IDENTITY_INSERT UMS_GROUP_CLAIMS ON
Insert into UMS_GROUP_CLAIMS (Id,GroupId,ClaimType,ClaimValue) VALUES(8116,'071C4BA0-CC3F-43DC-B2CB-6FC2BEA6D831','Permission','Permissions.WF.Workflow.List')
SET IDENTITY_INSERT UMS_GROUP_CLAIMS OFF
SET IDENTITY_INSERT UMS_GROUP_CLAIMS ON
Insert into UMS_GROUP_CLAIMS (Id,GroupId,ClaimType,ClaimValue) VALUES(8117,'071C4BA0-CC3F-43DC-B2CB-6FC2BEA6D831','Permission','Permissions.WF.WorkflowInstance.List')
SET IDENTITY_INSERT UMS_GROUP_CLAIMS OFF
SET IDENTITY_INSERT UMS_GROUP_CLAIMS ON
Insert into UMS_GROUP_CLAIMS (Id,GroupId,ClaimType,ClaimValue) VALUES(8118,'071C4BA0-CC3F-43DC-B2CB-6FC2BEA6D831','Permission','Permissions.WF.Workflow.Create')
SET IDENTITY_INSERT UMS_GROUP_CLAIMS OFF
SET IDENTITY_INSERT UMS_GROUP_CLAIMS ON
Insert into UMS_GROUP_CLAIMS (Id,GroupId,ClaimType,ClaimValue) VALUES(8119,'071C4BA0-CC3F-43DC-B2CB-6FC2BEA6D831','Permission','Permissions.WF.Workflow.Detail')
SET IDENTITY_INSERT UMS_GROUP_CLAIMS OFF
-----------------------------

Insert into UMS_GROUP_CLAIMS (GroupId,ClaimType,ClaimValue) VALUES('071C4BA0-CC3F-43DC-B2CB-6FC2BEA6D831','Permission','Permissions.WF.Workflow.Clone')


INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Title_Ar],[Module],[SubModule],[ClaimType],[ClaimValue],[IsDeleted])
VALUES ('WF','WF', 'Fatwa Dashboard', 'WF', 'Permission', 'Permissions.Dashboard.Workflow.View',0)

INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Title_Ar],[Module],[SubModule],[ClaimType],[ClaimValue],[IsDeleted])
VALUES ('List of Workflows','List of Workflows', 'Workflow Management', 'Workflow', 'Permission', 'Permissions.WF.Workflow.List',0)

INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Title_Ar],[Module],[SubModule],[ClaimType],[ClaimValue],[IsDeleted])
VALUES ('List of Workflow Instances','List of Workflows Instances', 'Workflow Management', 'Workflow', 'Permission', 'Permissions.WF.WorkflowInstance.List',0)


INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Title_Ar],[Module],[SubModule],[ClaimType],[ClaimValue],[IsDeleted])
VALUES ('Create Workflow','Create Workflow', 'Workflow Management', 'Workflow', 'Permission', 'Permissions.WF.Workflow.Create',0)


INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Title_Ar],[Module],[SubModule],[ClaimType],[ClaimValue],[IsDeleted])
VALUES ('Publish Workflow','Publish Workflow', 'Workflow Management', 'Workflow', 'Permission', 'Permissions.WF.Workflow.Publish',0)


INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Title_Ar],[Module],[SubModule],[ClaimType],[ClaimValue],[IsDeleted])
VALUES ('Activate Workflow','Activate Workflow', 'Workflow Management', 'Workflow', 'Permission', 'Permissions.WF.Workflow.Active',0)

INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Title_Ar],[Module],[SubModule],[ClaimType],[ClaimValue],[IsDeleted])
VALUES ('Clone Workflow','Clone Workflow', 'Workflow Management', 'Workflow', 'Permission', 'Permissions.WF.Workflow.Clone',0)

INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Title_Ar],[Module],[SubModule],[ClaimType],[ClaimValue],[IsDeleted])
VALUES ('Detail Workflow','Detail Workflow', 'Workflow Management', 'Workflow', 'Permission', 'Permissions.WF.Workflow.Detail',0)

------Ammaar Naveed-----25/01/2024---------UMS_CLAIMS Titles, Module Names and SubModules are transformed into keys and respective arabic values.
--Update Queries (Module Names)
Update UMS_CLAIM SET Module='User_Management' WHERE Module='User Management'
Update UMS_CLAIM SET Module='Legal_Principles_Management_System' WHERE Module='LPS'
Update UMS_CLAIM SET Module='Library_Management_System' WHERE Module='LMS'
Update UMS_CLAIM SET Module='Inventory_Management' WHERE Module='Inventory Management'
Update UMS_CLAIM SET Module='Legal_Library_System' WHERE Module='LLS'
Update UMS_CLAIM SET Module='Legislation_Management_System' WHERE Module='Legislation'
Update UMS_CLAIM SET Module='FATWA_Dashboard' WHERE Module='Fatwa Dashboard'
Update UMS_CLAIM SET Module='Employee_Management' WHERE Module='Employee Management'
Update UMS_CLAIM SET Module='Document_Management_System' WHERE Module='DMS'
Update UMS_CLAIM SET Module='Contact_Management' WHERE Module='Contact Management'
Update UMS_CLAIM SET Module='Consultation_Management_System' WHERE Module='COMS'
Update UMS_CLAIM SET Module='Case_Consultation_Management_System' WHERE Module='CMS/COMS'
Update UMS_CLAIM SET Module='Case_Management_System' WHERE Module='CMS'
Update UMS_CLAIM SET Module='Audit_Logs' WHERE Module='Audit Logs'
Update UMS_CLAIM SET Module='User_Management' WHERE Module='User Management'

--Update Queries (Sub Module Names)
Update UMS_CLAIM SET SubModule='LPS_Principle_Tag' WHERE Submodule='LPS Principle Tag'
Update UMS_CLAIM SET SubModule='Masked_Principles' WHERE Submodule='Masked Principles'
Update UMS_CLAIM SET SubModule='Requests_List' WHERE Submodule='Requests List'
Update UMS_CLAIM SET SubModule='Request_Item_Detail' WHERE Submodule='Request Item Detail'
Update UMS_CLAIM SET SubModule='Inventory' WHERE Submodule='INV'
Update UMS_CLAIM SET SubModule='Literature_Index' WHERE Submodule='Literature Index'
Update UMS_CLAIM SET SubModule='Literature_Borrow_Detail' WHERE Submodule='Literature Borrow Detail'
Update UMS_CLAIM SET SubModule='Masked_Document' WHERE Submodule='Masked Document'
Update UMS_CLAIM SET SubModule='Case_Management_System' WHERE Submodule='CMS'
Update UMS_CLAIM SET SubModule='Legal_Library_System' WHERE Submodule='LLS'
Update UMS_CLAIM SET SubModule='Consultation_Management_System' WHERE Submodule='COMS'
Update UMS_CLAIM SET SubModule='Case_Consultation_Management_System' WHERE Submodule='CMS/COMS'
Update UMS_CLAIM SET SubModule='Employee_Management' WHERE Submodule='EM'
Update UMS_CLAIM SET SubModule='Workflow' WHERE Submodule='WF'
Update UMS_CLAIM SET SubModule='Contact_List' WHERE Submodule='Contact List'
Update UMS_CLAIM SET SubModule='Create_Contact' WHERE Submodule='Contact Create'
Update UMS_CLAIM SET SubModule='Edit_Contact' WHERE Submodule='Contact Edit'
Update UMS_CLAIM SET SubModule='Delete_Contact' WHERE Submodule='Contact Delete'
Update UMS_CLAIM SET SubModule='Kuwait_Al_Yawm' WHERE Submodule='KAY'
Update UMS_CLAIM SET SubModule='Consultation_Request' WHERE Submodule='Consultation Request'
Update UMS_CLAIM SET SubModule='Consultation_File' WHERE Submodule='Consultation File'
Update UMS_CLAIM SET SubModule='Draft_Document' WHERE Submodule='Draft Document'
Update UMS_CLAIM SET SubModule='Inbox_Outbox' WHERE Submodule='InboxOutbox'
Update UMS_CLAIM SET SubModule='Case_Request' WHERE Submodule='Case Request'
Update UMS_CLAIM SET SubModule='Case_File' WHERE Submodule='Case File'
Update UMS_CLAIM SET SubModule='Registered_Case' WHERE Submodule='Registered Case'
Update UMS_CLAIM SET SubModule='Case_Draft' WHERE Submodule='Case Draft'
Update UMS_CLAIM SET SubModule='Ministry_Of_Judgement' WHERE Submodule='MOJ'
Update UMS_CLAIM SET SubModule='Document_Portfolio' WHERE Submodule='Document Portfolio'
Update UMS_CLAIM SET SubModule='Error_Logs' WHERE Submodule='Error Logs'
Update UMS_CLAIM SET SubModule='Process_Logs' WHERE Submodule='Process Logs'



------------------------------------------------------------------------------------------
---------
INSERT INTO [dbo].[UMS_GROUP_CLAIMS] ([GroupId],[ClaimType],[ClaimValue])
VALUES ('071C4BA0-CC3F-43DC-B2CB-6FC2BEA6D831', 'Permission', 'Permissions.Submenu.CMS.LawyerChamberAssignment.List')

INSERT INTO [dbo].[UMS_GROUP_CLAIMS] ([GroupId],[ClaimType],[ClaimValue])
VALUES ('071C4BA0-CC3F-43DC-B2CB-6FC2BEA6D831', 'Permission', 'Permissions.Submenu.CMS.LawyerChamberAssignment')
----------
  SET IDENTITY_INSERT [dbo].[UMS_USER_CLAIMS] ON 
Set IDENTITY_INSERT [dbo].[UMS_USER_CLAIMS] ON 
	INSERT [dbo].[UMS_USER_CLAIMS] ([Id], [UserId], [ClaimType], [ClaimValue]) 
	VALUES (5180, N'c25bdebd-cfe3-4dc0-9e2c-102f370319e8', N'Permission', N'Permissions.Menu.Timelog')
	SET IDENTITY_INSERT [dbo].[UMS_USER_CLAIMS] off
INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Title_Ar],[Module],[SubModule],[ClaimType],[ClaimValue] ,[IsDeleted])
	VALUES ('Time log Menu','Time log Menu', 'Timelog', 'Timelog', 'Permission', 'Permissions.Menu.Timelog', 0)

	INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Title_Ar],[Module],[SubModule],[ClaimType],[ClaimValue] ,[IsDeleted])
	VALUES ('List of Time log','List of Time log', 'Timelog', 'Timelog', 'Permission', 'Permissions.Submenu.Timelog.TimelogList', 0)
------------------------------------------------------------------------Worker Service--------------------------------------------------------------------Insert Script
DBCC CHECKIDENT ('FATWA_DB_DEV.dbo.WS_CMS_COMS_REMINDER', RESEED, 1);
USE [FATWA_DB_DEV]
GO
SET IDENTITY_INSERT [dbo].[WS_CMS_COMS_REMINDER] ON 
INSERT [dbo].[WS_CMS_COMS_REMINDER] ([ID], [CmsComsReminderTypeId], [FirstReminderDuration], [SecondReminderDuration], [ThirdReminderDuration], [CommunicationTypeId], [DraftTemplateVersionStatusId], [CmsCaseFileStatusId], [IsNotification], [IsTask], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive], [CouttypeId], [SLAInterval], [ExecutionTime]) VALUES (1, 3, 15, 10, 5, 2, NULL, NULL, 1, 0, N'fatwaadmin@gmail.com', CAST(N'2024-01-24T10:46:59.307' AS DateTime), N'fatwaadmin@gmail.com', CAST(N'2024-02-13T16:52:24.507' AS DateTime), NULL, NULL, 0, 1, NULL, 25, CAST(N'2024-02-13T00:30:00.000' AS DateTime))
INSERT [dbo].[WS_CMS_COMS_REMINDER] ([ID], [CmsComsReminderTypeId], [FirstReminderDuration], [SecondReminderDuration], [ThirdReminderDuration], [CommunicationTypeId], [DraftTemplateVersionStatusId], [CmsCaseFileStatusId], [IsNotification], [IsTask], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive], [CouttypeId], [SLAInterval], [ExecutionTime]) VALUES (2, 3, 15, 10, 5, 64, NULL, NULL, 1, 0, N'fatwaadmin@gmail.com', CAST(N'2024-01-18T16:15:57.753' AS DateTime), N'fatwaadmin@gmail.com', CAST(N'2024-02-13T16:52:42.950' AS DateTime), NULL, NULL, 0, 1, NULL, 25, CAST(N'2024-02-13T00:30:00.000' AS DateTime))
INSERT [dbo].[WS_CMS_COMS_REMINDER] ([ID], [CmsComsReminderTypeId], [FirstReminderDuration], [SecondReminderDuration], [ThirdReminderDuration], [CommunicationTypeId], [DraftTemplateVersionStatusId], [CmsCaseFileStatusId], [IsNotification], [IsTask], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive], [CouttypeId], [SLAInterval], [ExecutionTime]) VALUES (3, 3, 15, 10, 5, 4, NULL, NULL, 1, 0, N'fatwaadmin@gmail.com', CAST(N'2024-01-18T18:36:39.093' AS DateTime), N'fatwaadmin@gmail.com', CAST(N'2024-02-13T16:52:35.667' AS DateTime), NULL, NULL, 0, 1, NULL, 25, CAST(N'2024-02-13T00:30:00.000' AS DateTime))
INSERT [dbo].[WS_CMS_COMS_REMINDER] ([ID], [CmsComsReminderTypeId], [FirstReminderDuration], [SecondReminderDuration], [ThirdReminderDuration], [CommunicationTypeId], [DraftTemplateVersionStatusId], [CmsCaseFileStatusId], [IsNotification], [IsTask], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive], [CouttypeId], [SLAInterval], [ExecutionTime]) VALUES (6, 3, 15, 10, 5, 8, NULL, NULL, 1, 0, N'fatwaadmin@gmail.com', CAST(N'2024-01-26T12:43:31.603' AS DateTime), N'fatwaadmin@gmail.com', CAST(N'2024-02-13T16:51:56.983' AS DateTime), NULL, NULL, 0, 1, NULL, 25, CAST(N'2024-02-13T00:30:00.000' AS DateTime))
INSERT [dbo].[WS_CMS_COMS_REMINDER] ([ID], [CmsComsReminderTypeId], [FirstReminderDuration], [SecondReminderDuration], [ThirdReminderDuration], [CommunicationTypeId], [DraftTemplateVersionStatusId], [CmsCaseFileStatusId], [IsNotification], [IsTask], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive], [CouttypeId], [SLAInterval], [ExecutionTime]) VALUES (7, 3, 15, 10, 5, 16, NULL, NULL, 1, 0, N'fatwaadmin@gmail.com', CAST(N'2024-02-02T16:10:08.063' AS DateTime), N'fatwaadmin@gmail.com', CAST(N'2024-02-13T16:51:35.157' AS DateTime), NULL, NULL, 0, 1, NULL, 25, CAST(N'2024-02-13T00:30:00.000' AS DateTime))
INSERT [dbo].[WS_CMS_COMS_REMINDER] ([ID], [CmsComsReminderTypeId], [FirstReminderDuration], [SecondReminderDuration], [ThirdReminderDuration], [CommunicationTypeId], [DraftTemplateVersionStatusId], [CmsCaseFileStatusId], [IsNotification], [IsTask], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive], [CouttypeId], [SLAInterval], [ExecutionTime]) VALUES (8, 3, 15, 10, 5, 32, NULL, NULL, 1, 0, N'fatwaadmin@gmail.com', CAST(N'2024-02-02T16:10:30.097' AS DateTime), N'fatwaadmin@gmail.com', CAST(N'2024-02-13T16:51:22.120' AS DateTime), NULL, NULL, 0, 1, NULL, 25, CAST(N'2024-02-13T00:30:00.000' AS DateTime))
INSERT [dbo].[WS_CMS_COMS_REMINDER] ([ID], [CmsComsReminderTypeId], [FirstReminderDuration], [SecondReminderDuration], [ThirdReminderDuration], [CommunicationTypeId], [DraftTemplateVersionStatusId], [CmsCaseFileStatusId], [IsNotification], [IsTask], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive], [CouttypeId], [SLAInterval], [ExecutionTime]) VALUES (9, 3, 15, 10, 5, 128, NULL, NULL, 1, 0, N'fatwaadmin@gmail.com', CAST(N'2024-02-02T16:10:45.597' AS DateTime), N'fatwaadmin@gmail.com', CAST(N'2024-02-13T16:51:10.497' AS DateTime), NULL, NULL, 0, 1, NULL, 25, CAST(N'2024-02-13T00:30:00.000' AS DateTime))
INSERT [dbo].[WS_CMS_COMS_REMINDER] ([ID], [CmsComsReminderTypeId], [FirstReminderDuration], [SecondReminderDuration], [ThirdReminderDuration], [CommunicationTypeId], [DraftTemplateVersionStatusId], [CmsCaseFileStatusId], [IsNotification], [IsTask], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive], [CouttypeId], [SLAInterval], [ExecutionTime]) VALUES (10, 3, 15, 10, 5, 512, NULL, NULL, 1, 0, N'fatwaadmin@gmail.com', CAST(N'2024-02-02T16:11:10.443' AS DateTime), N'fatwaadmin@gmail.com', CAST(N'2024-02-13T16:50:54.620' AS DateTime), NULL, NULL, 0, 1, NULL, 25, CAST(N'2024-02-13T00:30:00.000' AS DateTime))
INSERT [dbo].[WS_CMS_COMS_REMINDER] ([ID], [CmsComsReminderTypeId], [FirstReminderDuration], [SecondReminderDuration], [ThirdReminderDuration], [CommunicationTypeId], [DraftTemplateVersionStatusId], [CmsCaseFileStatusId], [IsNotification], [IsTask], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive], [CouttypeId], [SLAInterval], [ExecutionTime]) VALUES (11, 3, 15, 10, 5, 256, NULL, NULL, 1, 0, N'fatwaadmin@gmail.com', CAST(N'2024-02-02T16:11:25.877' AS DateTime), N'fatwaadmin@gmail.com', CAST(N'2024-02-13T16:50:14.653' AS DateTime), NULL, NULL, 0, 1, NULL, 25, CAST(N'2024-02-12T00:30:00.000' AS DateTime))
INSERT [dbo].[WS_CMS_COMS_REMINDER] ([ID], [CmsComsReminderTypeId], [FirstReminderDuration], [SecondReminderDuration], [ThirdReminderDuration], [CommunicationTypeId], [DraftTemplateVersionStatusId], [CmsCaseFileStatusId], [IsNotification], [IsTask], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive], [CouttypeId], [SLAInterval], [ExecutionTime]) VALUES (12, 11, 15, 10, 5, NULL, NULL, NULL, 1, 0, N'fatwaadmin@gmail.com', CAST(N'2024-01-12T11:35:36.250' AS DateTime), N'fatwaadmin@gmail.com', CAST(N'2024-02-13T16:58:59.937' AS DateTime), NULL, NULL, 0, 1, NULL, 25, CAST(N'2024-02-13T02:45:00.000' AS DateTime))
INSERT [dbo].[WS_CMS_COMS_REMINDER] ([ID], [CmsComsReminderTypeId], [FirstReminderDuration], [SecondReminderDuration], [ThirdReminderDuration], [CommunicationTypeId], [DraftTemplateVersionStatusId], [CmsCaseFileStatusId], [IsNotification], [IsTask], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive], [CouttypeId], [SLAInterval], [ExecutionTime]) VALUES (4, 1, 15, 10, 5, NULL, NULL, NULL, 1, 0, N'fatwaadmin@gmail.com', CAST(N'2024-01-24T12:30:41.377' AS DateTime), N'fatwaadmin@gmail.com', CAST(N'2024-02-13T10:25:38.650' AS DateTime), NULL, NULL, 0, 1, NULL, 25, CAST(N'2024-02-13T00:15:00.000' AS DateTime))
INSERT [dbo].[WS_CMS_COMS_REMINDER] ([ID], [CmsComsReminderTypeId], [FirstReminderDuration], [SecondReminderDuration], [ThirdReminderDuration], [CommunicationTypeId], [DraftTemplateVersionStatusId], [CmsCaseFileStatusId], [IsNotification], [IsTask], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive], [CouttypeId], [SLAInterval], [ExecutionTime]) VALUES (13, 9, 15, 10, 5, NULL, NULL, NULL, 1, 0, N'fatwaadmin@gmail.com', CAST(N'2024-01-16T17:30:31.930' AS DateTime), N'fatwaadmin@gmail.com', CAST(N'2024-02-13T16:58:46.990' AS DateTime), NULL, NULL, 0, 1, NULL, 25, CAST(N'2024-02-13T02:30:00.000' AS DateTime))
INSERT [dbo].[WS_CMS_COMS_REMINDER] ([ID], [CmsComsReminderTypeId], [FirstReminderDuration], [SecondReminderDuration], [ThirdReminderDuration], [CommunicationTypeId], [DraftTemplateVersionStatusId], [CmsCaseFileStatusId], [IsNotification], [IsTask], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive], [CouttypeId], [SLAInterval], [ExecutionTime]) VALUES (14, 10, 15, 10, 5, NULL, NULL, NULL, 1, 0, N'fatwaadmin@gmail.com', CAST(N'2024-01-17T12:33:00.260' AS DateTime), N'fatwaadmin@gmail.com', CAST(N'2024-02-13T16:58:37.113' AS DateTime), NULL, NULL, 0, 1, NULL, 25, CAST(N'2024-02-13T02:15:00.000' AS DateTime))
INSERT [dbo].[WS_CMS_COMS_REMINDER] ([ID], [CmsComsReminderTypeId], [FirstReminderDuration], [SecondReminderDuration], [ThirdReminderDuration], [CommunicationTypeId], [DraftTemplateVersionStatusId], [CmsCaseFileStatusId], [IsNotification], [IsTask], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive], [CouttypeId], [SLAInterval], [ExecutionTime]) VALUES (15, 13, 15, 10, 5, NULL, NULL, NULL, 1, 0, N'fatwaadmin@gmail.com', CAST(N'2024-01-19T16:18:55.450' AS DateTime), N'fatwaadmin@gmail.com', CAST(N'2024-02-13T16:58:25.437' AS DateTime), NULL, NULL, 0, 1, NULL, 25, CAST(N'2024-02-13T02:00:00.000' AS DateTime))
INSERT [dbo].[WS_CMS_COMS_REMINDER] ([ID], [CmsComsReminderTypeId], [FirstReminderDuration], [SecondReminderDuration], [ThirdReminderDuration], [CommunicationTypeId], [DraftTemplateVersionStatusId], [CmsCaseFileStatusId], [IsNotification], [IsTask], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive], [CouttypeId], [SLAInterval], [ExecutionTime]) VALUES (16, 5, 15, 10, 5, NULL, NULL, NULL, 1, 0, N'fatwaadmin@gmail.com', CAST(N'2024-02-02T15:28:39.927' AS DateTime), N'fatwaadmin@gmail.com', CAST(N'2024-02-13T16:58:12.763' AS DateTime), NULL, NULL, 0, 1, NULL, 25, CAST(N'2024-02-13T01:45:00.000' AS DateTime))
INSERT [dbo].[WS_CMS_COMS_REMINDER] ([ID], [CmsComsReminderTypeId], [FirstReminderDuration], [SecondReminderDuration], [ThirdReminderDuration], [CommunicationTypeId], [DraftTemplateVersionStatusId], [CmsCaseFileStatusId], [IsNotification], [IsTask], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive], [CouttypeId], [SLAInterval], [ExecutionTime]) VALUES (17, 6, 15, 10, 5, NULL, NULL, NULL, 1, 0, N'fatwaadmin@gmail.com', CAST(N'2024-02-02T15:32:49.673' AS DateTime), N'fatwaadmin@gmail.com', CAST(N'2024-02-13T16:58:00.923' AS DateTime), NULL, NULL, 0, 1, NULL, 25, CAST(N'2024-02-13T01:30:00.000' AS DateTime))
INSERT [dbo].[WS_CMS_COMS_REMINDER] ([ID], [CmsComsReminderTypeId], [FirstReminderDuration], [SecondReminderDuration], [ThirdReminderDuration], [CommunicationTypeId], [DraftTemplateVersionStatusId], [CmsCaseFileStatusId], [IsNotification], [IsTask], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive], [CouttypeId], [SLAInterval], [ExecutionTime]) VALUES (18, 7, 15, 10, 5, NULL, NULL, NULL, 1, 0, N'fatwaadmin@gmail.com', CAST(N'2024-02-02T16:09:18.780' AS DateTime), N'fatwaadmin@gmail.com', CAST(N'2024-02-13T16:57:45.097' AS DateTime), NULL, NULL, 0, 1, NULL, 25, CAST(N'2024-02-13T01:15:00.000' AS DateTime))
INSERT [dbo].[WS_CMS_COMS_REMINDER] ([ID], [CmsComsReminderTypeId], [FirstReminderDuration], [SecondReminderDuration], [ThirdReminderDuration], [CommunicationTypeId], [DraftTemplateVersionStatusId], [CmsCaseFileStatusId], [IsNotification], [IsTask], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive], [CouttypeId], [SLAInterval], [ExecutionTime]) VALUES (19, 8, 15, 10, 5, NULL, NULL, NULL, 1, 0, N'fatwaadmin@gmail.com', CAST(N'2024-02-02T16:09:30.293' AS DateTime), N'fatwaadmin@gmail.com', CAST(N'2024-02-13T16:56:41.330' AS DateTime), NULL, NULL, 0, 1, NULL, 25, CAST(N'2024-02-13T01:00:00.000' AS DateTime))
INSERT [dbo].[WS_CMS_COMS_REMINDER] ([ID], [CmsComsReminderTypeId], [FirstReminderDuration], [SecondReminderDuration], [ThirdReminderDuration], [CommunicationTypeId], [DraftTemplateVersionStatusId], [CmsCaseFileStatusId], [IsNotification], [IsTask], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive], [CouttypeId], [SLAInterval], [ExecutionTime]) VALUES (20, 12, 15, 10, 5, NULL, NULL, NULL, 1, 0, N'fatwaadmin@gmail.com', CAST(N'2024-02-02T16:09:43.947' AS DateTime), N'fatwaadmin@gmail.com', CAST(N'2024-02-13T16:55:48.260' AS DateTime), NULL, NULL, 0, 1, NULL, 25, CAST(N'2024-02-13T00:45:00.000' AS DateTime))
SET IDENTITY_INSERT [dbo].[WS_CMS_COMS_REMINDER] OFF
GO
--------------------------------------------------------------------------------
INSERT [dbo].[WS_CMS_COMS_REMINDER_HISTORY_STATUS] ([Id], [NameEn], [NameAr]) VALUES (1, N'Added', N'تمت الاضافة')
INSERT [dbo].[WS_CMS_COMS_REMINDER_HISTORY_STATUS] ([Id], [NameEn], [NameAr]) VALUES (2, N'Updated', N'تم التحديث')
SET IDENTITY_INSERT [dbo].[WS_CMS_COMS_REMINDER_HISTORY_STATUS] OFF

INSERT [dbo].[WS_COMM_COMMUNICATION_TYPES] ([CommunicationTypeId], [NameAr], [NameEn], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (1, N'Request For More Information', N'Request For More Information', N'fatwaadmin@gmail.com', CAST(N'2023-08-13T17:01:00.000' AS DateTime), N'fatwaadmin@gmail.com', CAST(N'2024-01-10T19:02:07.763' AS DateTime), NULL, NULL, 0, 1)
INSERT [dbo].[WS_COMM_COMMUNICATION_TYPES] ([CommunicationTypeId], [NameAr], [NameEn], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (2, N'Meeting Scheduled', N'Meeting Scheduled', N'fatwaadmin@gmail.com', CAST(N'2023-08-13T17:01:00.000' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
INSERT [dbo].[WS_COMM_COMMUNICATION_TYPES] ([CommunicationTypeId], [NameAr], [NameEn], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (4, N'Request For More Information Reminder', N'Request For More Information Reminder', N'fatwaadmin@gmail.com', CAST(N'2023-08-13T17:01:00.000' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
INSERT [dbo].[WS_COMM_COMMUNICATION_TYPES] ([CommunicationTypeId], [NameAr], [NameEn], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (8, N'Save And Close File', N'Save And Close File', N'fatwaadmin@gmail.com', CAST(N'2023-08-13T17:01:00.000' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
INSERT [dbo].[WS_COMM_COMMUNICATION_TYPES] ([CommunicationTypeId], [NameAr], [NameEn], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (16, N'Case Registered', N'Case Registered', N'fatwaadmin@gmail.com', CAST(N'2023-08-13T17:01:00.000' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
INSERT [dbo].[WS_COMM_COMMUNICATION_TYPES] ([CommunicationTypeId], [NameAr], [NameEn], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (32, N'Case Registered', N'Case Registered', N'fatwaadmin@gmail.com', CAST(N'2023-08-13T17:01:00.000' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
INSERT [dbo].[WS_COMM_COMMUNICATION_TYPES] ([CommunicationTypeId], [NameAr], [NameEn], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (64, N'General Update', N'General Update', N'fatwaadmin@gmail.com', CAST(N'2023-08-13T17:01:00.000' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
INSERT [dbo].[WS_COMM_COMMUNICATION_TYPES] ([CommunicationTypeId], [NameAr], [NameEn], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (128, N'Incoming Report', N'Judgement', N'fatwaadmin@gmail.com', CAST(N'2023-08-13T17:01:00.000' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
INSERT [dbo].[WS_COMM_COMMUNICATION_TYPES] ([CommunicationTypeId], [NameAr], [NameEn], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (256, N'Case File Execution', N'Case File Execution', N'fatwaadmin@gmail.com', CAST(N'2023-08-13T17:01:00.000' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
INSERT [dbo].[WS_COMM_COMMUNICATION_TYPES] ([CommunicationTypeId], [NameAr], [NameEn], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (512, N'Reject Save And Close File', N'Reject Save And Close File', N'fatwaadmin@gmail.com', CAST(N'2023-08-13T17:01:00.000' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)

SET IDENTITY_INSERT [dbo].[WS_EXECUTION_STATUS_LKP] ON 
INSERT [dbo].[WS_EXECUTION_STATUS_LKP] ([Id], [ExecutionStatusEn], [ExecutionStatusAr]) VALUES (1, N'Successfull', N'Successfull')
INSERT [dbo].[WS_EXECUTION_STATUS_LKP] ([Id], [ExecutionStatusEn], [ExecutionStatusAr]) VALUES (2, N'Failed', N'Failed')
INSERT [dbo].[WS_EXECUTION_STATUS_LKP] ([Id], [ExecutionStatusEn], [ExecutionStatusAr]) VALUES (3, N'Exception', N'Exception')
INSERT [dbo].[WS_EXECUTION_STATUS_LKP] ([Id], [ExecutionStatusEn], [ExecutionStatusAr]) VALUES (4, N'Reattempt', N'Reattempt')
SET IDENTITY_INSERT [dbo].[WS_EXECUTION_STATUS_LKP] OFF

SET IDENTITY_INSERT [dbo].[WS_WORKERSERVICES_LKP] ON 
INSERT [dbo].[WS_WORKERSERVICES_LKP] ([Id], [WorkerServiceEn], [WorkerServiceAr]) VALUES (1, N'Define the period to Appeal case file', N'Reminder For Lawyer')
INSERT [dbo].[WS_WORKERSERVICES_LKP] ([Id], [WorkerServiceEn], [WorkerServiceAr]) VALUES (2, N'Communication Response Reminder ', N'Communication Response Reminder ')
INSERT [dbo].[WS_WORKERSERVICES_LKP] ([Id], [WorkerServiceEn], [WorkerServiceAr]) VALUES (3, N'Define the period to Supreme case file', N'Define the period to Supreme case file')
INSERT [dbo].[WS_WORKERSERVICES_LKP] ([Id], [WorkerServiceEn], [WorkerServiceAr]) VALUES (4, N'Define the period to Request for Additional Information', N'Define the period to Request for Additional Information')
INSERT [dbo].[WS_WORKERSERVICES_LKP] ([Id], [WorkerServiceEn], [WorkerServiceAr]) VALUES (5, N'Define the period for reminder request for addition information', N'Define the period for reminder request for addition information')
INSERT [dbo].[WS_WORKERSERVICES_LKP] ([Id], [WorkerServiceEn], [WorkerServiceAr]) VALUES (6, N'Define the period to auto save the file', N'Define the period to auto save the file')
INSERT [dbo].[WS_WORKERSERVICES_LKP] ([Id], [WorkerServiceEn], [WorkerServiceAr]) VALUES (7, N'Define the period to register a case at MOJ', N'Define the period to register a case at MOJ')
INSERT [dbo].[WS_WORKERSERVICES_LKP] ([Id], [WorkerServiceEn], [WorkerServiceAr]) VALUES (8, N'Define the period to complete the claim statement', N'Define the period to complete the claim statement')
INSERT [dbo].[WS_WORKERSERVICES_LKP] ([Id], [WorkerServiceEn], [WorkerServiceAr]) VALUES (9, N'Define the period to reply on the operation consultant', N'Define the period to reply on the operation consultant')
INSERT [dbo].[WS_WORKERSERVICES_LKP] ([Id], [WorkerServiceEn], [WorkerServiceAr]) VALUES (10, N'Define the period to prepare defense letter', N'Define the period to prepare defense letter')
INSERT [dbo].[WS_WORKERSERVICES_LKP] ([Id], [WorkerServiceEn], [WorkerServiceAr]) VALUES (11, N'Draft Modification Reminder', N'Draft Modification Reminder')
INSERT [dbo].[WS_WORKERSERVICES_LKP] ([Id], [WorkerServiceEn], [WorkerServiceAr]) VALUES (12, N'Data Migration on Number Pattern', N'Data Migration on Number Pattern')
SET IDENTITY_INSERT [dbo].[WS_WORKERSERVICES_LKP] OFF

GO
INSERT [dbo].[WS_COMM_COMMUNICATION_TYPES] ([CommunicationTypeId], [NameAr], [NameEn], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (8, N'Request For More Information', N'Request For More Information', N'fatwaadmin@gmail.com', CAST(N'2023-08-13T17:01:00.000' AS DateTime), N'fatwaadmin@gmail.com', CAST(N'2024-01-10T19:02:07.763' AS DateTime), NULL, NULL, 0, 1)
INSERT [dbo].[WS_COMM_COMMUNICATION_TYPES] ([CommunicationTypeId], [NameAr], [NameEn], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (64, N'Meeting Scheduled', N'Meeting Scheduled', N'fatwaadmin@gmail.com', CAST(N'2023-08-13T17:01:00.000' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
INSERT [dbo].[WS_COMM_COMMUNICATION_TYPES] ([CommunicationTypeId], [NameAr], [NameEn], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (8192, N'Request For More Information Reminder', N'Request For More Information Reminder', N'fatwaadmin@gmail.com', CAST(N'2023-08-13T17:01:00.000' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
INSERT [dbo].[WS_COMM_COMMUNICATION_TYPES] ([CommunicationTypeId], [NameAr], [NameEn], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (16384, N'Save And Close File', N'Save And Close File', N'fatwaadmin@gmail.com', CAST(N'2023-08-13T17:01:00.000' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
INSERT [dbo].[WS_COMM_COMMUNICATION_TYPES] ([CommunicationTypeId], [NameAr], [NameEn], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (32768, N'Case Registered', N'Case Registered', N'fatwaadmin@gmail.com', CAST(N'2023-08-13T17:01:00.000' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
INSERT [dbo].[WS_COMM_COMMUNICATION_TYPES] ([CommunicationTypeId], [NameAr], [NameEn], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (65536, N'Case File Execution', N'Case File Execution', N'fatwaadmin@gmail.com', CAST(N'2023-08-13T17:01:00.000' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
INSERT [dbo].[WS_COMM_COMMUNICATION_TYPES] ([CommunicationTypeId], [NameAr], [NameEn], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (131072, N'Judgement', N'Judgement', N'fatwaadmin@gmail.com', CAST(N'2023-08-13T17:01:00.000' AS DateTime), NULL, NULL, NULL, NULL, 0, 1) 
INSERT [dbo].[WS_COMM_COMMUNICATION_TYPES] ([CommunicationTypeId], [NameAr], [NameEn], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (262144, N'General Update', N'General Update', N'fatwaadmin@gmail.com', CAST(N'2023-08-13T17:01:00.000' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
INSERT [dbo].[WS_COMM_COMMUNICATION_TYPES] ([CommunicationTypeId], [NameAr], [NameEn], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (1048576, N'Incoming Report', N'Judgement', N'fatwaadmin@gmail.com', CAST(N'2023-08-13T17:01:00.000' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
INSERT [dbo].[WS_COMM_COMMUNICATION_TYPES] ([CommunicationTypeId], [NameAr], [NameEn], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (1073741824, N'Reject Save And Close File', N'Reject Save And Close File', N'fatwaadmin@gmail.com', CAST(N'2023-08-13T17:01:00.000' AS DateTime), NULL, NULL, NULL, NULL, 0, 1) 

INSERT INTO CMS_COMS_REMINDER_TYPE (IntervalNameEn, IntervalNameAr) 
VALUES ('Draft Modification Reminder', N'تذكير بالتعديل المسودة');

UPDATE [dbo].[WS_CMS_COMS_REMINDER]
SET [CreatedBy] = N'FatwaAdmin@uat.fatwa.gov.kw';

UPDATE [dbo].[WS_COMM_COMMUNICATION_TYPES]
SET [CreatedBy] = N'FatwaAdmin@uat.fatwa.gov.kw';

UPDATE [dbo].[WS_COMM_COMMUNICATION_TYPES]
SET [ModifiedBy] = N'FatwaAdmin@uat.fatwa.gov.kw' where CommunicationTypeId='1';
------------------------------------------------------------------------Worker Service Ends--------------------------------------------------------------------Insert Script

IF NOT Exists(SELECT 1 from UMS_GROUP_CLAIMS where GroupId = '071C4BA0-CC3F-43DC-B2CB-6FC2BEA6D831' and ClaimValue = 'Permissions.WF.Workflow.Publish')
	INSERT INTO [dbo].[UMS_GROUP_CLAIMS] ([GroupId],[ClaimType],[ClaimValue])
		VALUES ('071C4BA0-CC3F-43DC-B2CB-6FC2BEA6D831', 'Permission', 'Permissions.WF.Workflow.Publish')
GO
IF NOT Exists(SELECT 1 from UMS_GROUP_CLAIMS where GroupId = '071C4BA0-CC3F-43DC-B2CB-6FC2BEA6D831' and ClaimValue = 'Permissions.WF.Workflow.Active')
	INSERT INTO [dbo].[UMS_GROUP_CLAIMS] ([GroupId],[ClaimType],[ClaimValue])
		VALUES ('071C4BA0-CC3F-43DC-B2CB-6FC2BEA6D831', 'Permission', 'Permissions.WF.Workflow.Active')
GO

-----------------------------
Update WF_ACTIVITY_PR_LKP Set Method = 'Cms_TransferToPOButSendToFPForDecision' , AKey ='CmsTransferToPOButSendToFPForDecision' where ActivityId = 23


------Ammaar Naveed-----26/01/2024---------
UPDATE EP_CONTACT_TYPE SET Name_Ar=N'الصفحة الرئيسية' WHERE Id=1
------Ammaar Naveed-----29/01/2024---------
UPDATE EP_CONTACT_TYPE SET Name_Ar=N'العمل' WHERE Id=2
UPDATE EP_CONTACT_TYPE SET Name_Ar=N'الهاتف النقال' WHERE Id=3
UPDATE EP_CONTACT_TYPE SET Name_Ar=N'أخرى' WHERE Id=4
Update WF_ACTIVITY_PR_LKP Set Method = 'Cms_TransferToPOButSendToFPForDecision' , AKey ='CmsTransferToPOButSendToFPForDecision' where ActivityId = 23



----------------------------------------- 26-01-2024 ----------------------------------------

insert into EP_CONTACT_TYPE values (1,'Home','Home',1,'SYSTEM',GETDATE(),null,null,null,null,0)
insert into EP_CONTACT_TYPE values (2,'Work','Work',1,'SYSTEM',GETDATE(),null,null,null,null,0)
insert into EP_CONTACT_TYPE values (3,'Mobile','Mobile',1,'SYSTEM',GETDATE(),null,null,null,null,0)
insert into EP_CONTACT_TYPE values (4,'Other','Other',1,'SYSTEM',GETDATE(),null,null,null,null,0)
GO
insert into EP_GRADE_TYPE values ('Gradetype1','Gradetype1','System',GETDATE(),null,null,null,null,0)
GO
update EP_GRADE set GradeTypeId = 1
GO
insert into EP_CONTRACT_TYPE values ('Contrcttype1','Contrcttype1','System',GETDATE(),null,null,null,null,0)
GO
update EP_EMPLOYMENT_INFORMATION set ContractTypeId = 1
GO
---------02/02/2024	
 IF((SELECT COUNT(*) FROM UMS_CLAIM WHERE ClaimValue = 'Permissions.COMS.File.View') = 0)
 BEGIN
 INSERT INTO UMS_CLAIM VALUES ('Consultation File View','Consultation_Management_System','Consultation_File', 'Permission', 'Permissions.COMS.File.View','Consultation File View',0)
 END
 GO
  IF((SELECT COUNT(*) FROM UMS_CLAIM WHERE ClaimValue = 'Permissions.COMS.Request.View') = 0)
 BEGIN
 INSERT INTO UMS_CLAIM VALUES ('Consultation Request View','Consultation_Management_System','Consultation_Request', 'Permission', 'Permissions.COMS.Request.View','Consultation Request View',0)
 END
 GO
  INSERT INTO UMS_GROUP_CLAIMS VALUES ('93C1C1D4-73FE-4944-8F3E-8B9B983646F8', 'Permission', 'Permissions.COMS.Request.View')
  INSERT INTO UMS_GROUP_CLAIMS VALUES ('85D6848C-3BF1-403E-9CF6-B749D197ED15', 'Permission', 'Permissions.COMS.Request.View')
  INSERT INTO UMS_GROUP_CLAIMS VALUES ('E5E9AFCE-3123-48E5-A0CC-EAEC678AA179', 'Permission', 'Permissions.COMS.Request.View')
  INSERT INTO UMS_GROUP_CLAIMS VALUES ('93C1C1D4-73FE-4944-8F3E-8B9B983646F8', 'Permission', 'Permissions.COMS.File.View')
  INSERT INTO UMS_GROUP_CLAIMS VALUES ('85D6848C-3BF1-403E-9CF6-B749D197ED15', 'Permission', 'Permissions.COMS.File.View')
  INSERT INTO UMS_GROUP_CLAIMS VALUES ('E5E9AFCE-3123-48E5-A0CC-EAEC678AA179', 'Permission', 'Permissions.COMS.File.View')
  ------
  INSERT INTO UMS_GROUP_CLAIMS VALUES ('9590F5FC-7570-4CE8-B3BA-4FF57C5D6414', 'Permission', 'Permissions.COMS.Request.View')
  -------

GO

----------------------CMS_OPERATING_SECTOR_TYPE_G2G_LKP
Update CMS_OPERATING_SECTOR_TYPE_G2G_LKP set Name_Ar = N'مشرف قطاعات المدني/تجاري' where Id = 11
--------------------

Update COMS_CONSULTATION_Legislation_FILE_TYPE_FTW_LKP set Name_Ar = N'مشاريع القوانين' where Id = 1
Update COMS_CONSULTATION_Legislation_FILE_TYPE_FTW_LKP set Name_Ar = N'اقتراحات القوانين' where Id = 2
Update COMS_CONSULTATION_Legislation_FILE_TYPE_FTW_LKP set Name_Ar = N'الاتفاقيات واللوائح' where Id = 4

-------------------

Update CMS_CASE_REQUEST_STATUS_G2G_LKP set Name_Ar = N'تم التعيين لقطاع الفتاوى' where Id = 4096
Update CMS_CASE_REQUEST_STATUS_G2G_LKP set Name_Ar = N'تم التعيين للقطاع التحكيم الدولي' where Id = 16384
Update CMS_CASE_REQUEST_STATUS_G2G_LKP set Name_Ar = N'تم التعيين للقطاع التظلمات الإدارية' where Id = 32768
------------------
Update CMS_REQUEST_TYPE_G2G_LKP set Name_Ar = N'قطاعات المدني/تجاري' where Id = 2
--------------
Update CMS_SUBTYPE_G2G_LKP set Name_Ar = N'التظلم من قرار النيابة' where Id = 6
-----------------
------------------------------------- 1/12/2024
Update Submodule Set Name_En = 'Case Request' where Id = 1
Update Submodule Set Name_En = 'Case File' where Id = 2
Update Submodule Set Name_En = 'Registered Case' where Id = 4
Update Submodule Set Name_En = 'Consultation Request' where Id = 8
Update Submodule Set Name_En = 'Consultation File' where Id = 64
----------------------------------------------

Update CMS_Response_Type set Name_Ar = N'المستند النهائي' where Id = 1024
Update CMS_Response_Type set Name_Ar = N'ورود تقرير ' where Id = 512
Update CMS_Response_Type set Name_Ar = N'حكم الاستجواب  ' where Id = 256
Update CMS_Response_Type set Name_Ar = N'بما تم   ' where Id = 128
--------------------

Update EP_PERSONAL_INFORMATION Set LastName_Ar =N'موظف' where UserId = '50371be9-0b0b-4a5d-b7e7-73ebd9730286'
Update EP_PERSONAL_INFORMATION Set SecondName_Ar =N'موظف' where UserId = '50371be9-0b0b-4a5d-b7e7-73ebd9730286'
Update EP_PERSONAL_INFORMATION Set FirstName_Ar =N'الموارد البشرية' where UserId = '50371be9-0b0b-4a5d-b7e7-73ebd9730286'

-----Ammaar Naveed------13/02/2024---------Updating claim titles against claim values.
--Updating Permission Titles (En)
Update UMS_CLAIM SET Title_En='List of Workflow Instances' WHERE ClaimValue='Permissions.WF.WorkflowInstance.List'
UPDATE UMS_CLAIM SET Title_En='Case & Consultation Management System (Menu)' WHERE ClaimValue='Permissions.Menu.CMS/COMS'
UPDATE UMS_CLAIM SET Title_En='Add Minutes of Meeting' WHERE ClaimValue='Permissions.Submenu.Meeting.MeetingList.AddMOM'
UPDATE UMS_CLAIM SET Title_En='Legal Principles Management System (Menu)' WHERE ClaimValue='Permissions.Menu.LPS'
UPDATE UMS_CLAIM SET Title_En='Library Management System (Menu)' WHERE ClaimValue='Permissions.Menu.LMS'
UPDATE UMS_CLAIM SET Title_En='Approved Return Book' WHERE ClaimValue='Permissions.Submenu.LMS.LiteratureReturnDetail.Approval'
UPDATE UMS_CLAIM SET Title_En='Legislation Management System (Menu)' WHERE ClaimValue='Permissions.Menu.LDS'
UPDATE UMS_CLAIM SET Title_En='Case Management System' WHERE ClaimValue='Permissions.Dashboard.CMS.View'
UPDATE UMS_CLAIM SET Title_En='Legal Library System' WHERE ClaimValue='Permissions.Dashboard.LLS.View'
UPDATE UMS_CLAIM SET Title_En='Consultation Management System' WHERE ClaimValue='Permissions.Dashboard.COMS.View'
UPDATE UMS_CLAIM SET Title_En='Case/Consultation Management System' WHERE ClaimValue='Permissions.Dashboard.CMS/COMS.View'
UPDATE UMS_CLAIM SET Title_En='Employee Management System' WHERE ClaimValue='Permissions.Dashboard.EM.View'
UPDATE UMS_CLAIM SET Title_En='Workflow' WHERE ClaimValue='Permissions.Dashboard.Workflow.View'
UPDATE UMS_CLAIM SET Title_En='Add Employee' WHERE ClaimValue='Permissions.EM.Employee.Create'
UPDATE UMS_CLAIM SET Title_En='Create New Contacts' WHERE ClaimValue='Permissions.Contact.AddContact.Create'
UPDATE UMS_CLAIM SET Title_En='Edit Contacts' WHERE ClaimValue='Permissions.Contact.AddContact.Edit'
UPDATE UMS_CLAIM SET Title_En='Delete Contacts' WHERE ClaimValue='Permissions.Contact.DeleteContact.Delete'
UPDATE UMS_CLAIM SET Title_En='Kuwait Al-Yawm Publications List' WHERE ClaimValue='Permissions.DMS.KAY.DocumentList'
UPDATE UMS_CLAIM SET Title_En='List of Workflows' WHERE ClaimValue='Permissions.WF.Workflow.List'
UPDATE UMS_CLAIM SET Title_En='Legal Principle Details View' WHERE ClaimValue='Permissions.Lps_Latest.Principles.DetailsView'
UPDATE UMS_CLAIM SET Title_En='Add LPS Principle Tag' WHERE ClaimValue='Permissions.Submenu.LPS.Principles.Tags.Add'
UPDATE UMS_CLAIM SET Title_En='Edit LPS Principle Tag' WHERE ClaimValue='Permissions.Submenu.LPS.Principles.Tags.Edit'
UPDATE UMS_CLAIM SET Title_En='LPS Principle Tag' WHERE ClaimValue='Permissions.Submenu.LPS.Principles.Tags'
UPDATE UMS_CLAIM SET Title_En='List of Index Divisions' WHERE ClaimValue='Permissions.LMS.LiteratureIndexDivison.List'
UPDATE UMS_CLAIM SET Title_En='Create Index Divisions' WHERE ClaimValue='Permissions.LMS.LiteratureIndexDivision.Create'

--Updating Permission Titles (Ar)
--Workflow
UPDATE UMS_CLAIM SET Title_Ar=N'آلية العمل' WHERE ClaimValue='Permissions.Dashboard.Workflow.View'
UPDATE UMS_CLAIM SET Title_Ar=N'استنساخ آلية العمل' WHERE ClaimValue='Permissions.WF.Workflow.Clone'
UPDATE UMS_CLAIM SET Title_Ar=N'تفاصيل آلية العمل' WHERE ClaimValue='Permissions.WF.Workflow.Detail'
UPDATE UMS_CLAIM SET Title_Ar=N'تنشيط آلية العمل' WHERE ClaimValue='Permissions.WF.Workflow.Active'
UPDATE UMS_CLAIM SET Title_Ar=N'نشر آلية العمل' WHERE ClaimValue='Permissions.WF.Workflow.Publish'
UPDATE UMS_CLAIM SET Title_Ar=N'إنشاء آلية العمل' WHERE ClaimValue='Permissions.WF.Workflow.Create'
UPDATE UMS_CLAIM SET Title_Ar=N'قائمة نماذج آلية العمل' WHERE ClaimValue='Permissions.WF.WorkflowInstance.List'
UPDATE UMS_CLAIM SET Title_Ar=N'قائمة آلية العمل' WHERE ClaimValue='Permissions.WF.Workflow.List'
--User Management
UPDATE UMS_CLAIM SET Title_Ar=N'ترجمة' WHERE ClaimValue='Admin.Permissions.Users.Translations'
UPDATE UMS_CLAIM SET Title_Ar=N'اعدادات النظام' WHERE ClaimValue='Admin.Permissions.Users.SystemSetting'
UPDATE UMS_CLAIM SET Title_Ar=N'عرض إعدادات النظام' WHERE ClaimValue='Admin.Permissions.Users.SystemConfiguration.View'
UPDATE UMS_CLAIM SET Title_Ar=N'حذف إعدادات النظام' WHERE ClaimValue='Admin.Permissions.Users.SystemConfiguration.Delete'
UPDATE UMS_CLAIM SET Title_Ar=N'تعديل إعدادات النظام' WHERE ClaimValue='Admin.Permissions.Users.SystemConfiguration.Edit'
UPDATE UMS_CLAIM SET Title_Ar=N'إنشاء إعدادات النظام' WHERE ClaimValue='Admin.Permissions.Users.SystemConfiguration.Create'
UPDATE UMS_CLAIM SET Title_Ar=N'إعدادات النظام' WHERE ClaimValue='Admin.Permissions.Users.SystemConfiguration'
UPDATE UMS_CLAIM SET Title_Ar=N'حذف الدور' WHERE ClaimValue='Admin.Permissions.Roles.Delete'
UPDATE UMS_CLAIM SET Title_Ar=N'نقل المستخدم' WHERE ClaimValue='Admin.Permissions.TransferUser'
UPDATE UMS_CLAIM SET Title_Ar=N'حفظ نقل المستخدم' WHERE ClaimValue='Admin.Permissions.TransferUser.Save'
UPDATE UMS_CLAIM SET Title_Ar=N'أدوار القائمة الفرعية' WHERE ClaimValue='Admin.Permissions.Submenu.UMS.Roles'
UPDATE UMS_CLAIM SET Title_Ar=N'مجموعات القائمة الفرعية' WHERE ClaimValue='Admin.Permissions.Submenu.UMS.Groups'
UPDATE UMS_CLAIM SET Title_Ar=N'قائمة المستخدمين الفرعية' WHERE ClaimValue='Admin.Permissions.Submenu.UMS.Users'
UPDATE UMS_CLAIM SET Title_Ar=N'قائمة إدارة المستخدم' WHERE ClaimValue='Admin.Permissions.Menu.UMS'
UPDATE UMS_CLAIM SET Title_Ar=N'حذف الدور' WHERE ClaimValue='Admin.Permissions.Roles.Delete'
UPDATE UMS_CLAIM SET Title_Ar=N'أدوار القائمة الفرعية' WHERE ClaimValue='Admin.Permissions.Submenu.UMS.Roles'
UPDATE UMS_CLAIM SET Title_Ar=N'قائمة إدارة المستخدم' WHERE ClaimValue='Admin.Permissions.Menu.UMS'
UPDATE UMS_CLAIM SET Title_Ar=N'مجموعات القائمة الفرعية' WHERE ClaimValue='Admin.Permissions.Submenu.UMS.Groups'
--Time log
UPDATE UMS_CLAIM SET Title_Ar=N'قائمة سجل الوقت' WHERE ClaimValue='Permissions.Submenu.Timelog.TimelogList'
UPDATE UMS_CLAIM SET Title_Ar=N'قائمة سجل الوقت' WHERE ClaimValue='Permissions.Menu.Timelog'
UPDATE UMS_CLAIM SET Title_Ar=N'قائمة المسودة' WHERE ClaimValue='Permissions.Submenu.Task.DraftList'
UPDATE UMS_CLAIM SET Title_Ar=N'قائمة المهام' WHERE ClaimValue='Permissions.Submenu.Task.TaskList'
UPDATE UMS_CLAIM SET Title_Ar=N'لوحة معلومات المهام' WHERE ClaimValue='Permissions.Submenu.Task.Taskdashboard'
UPDATE UMS_CLAIM SET Title_Ar=N'قائمة المهام' WHERE ClaimValue='Permissions.Menu.Task'
UPDATE UMS_CLAIM SET Title_Ar=N'قائمة لوحة معلومات مسؤول النظام' WHERE ClaimValue='Admin.Permissions.Menu.Dashboard'
UPDATE UMS_CLAIM SET Title_Ar=N'نظام إدارة القضايا والاستشاري القائمة' WHERE ClaimValue='Permissions.Menu.CMS/COMS'
UPDATE UMS_CLAIM SET Title_Ar=N'عرض تفاصيل الاشعار' WHERE ClaimValue='Permission.Notification.Notification.Detail'
UPDATE UMS_CLAIM SET Title_Ar=N'قائمة الاشعارات' WHERE ClaimValue='Permissions.Submenu.Notfication.NotificationList'
UPDATE UMS_CLAIM SET Title_Ar=N'قائمة الاشعارات' WHERE ClaimValue='Permissions.Menu.Notfication'
--Meeting
UPDATE UMS_CLAIM SET Title_Ar=N'قرار الاجتماع' WHERE ClaimValue='Permissions.Submenu.Meeting.MeetingList.MeetingDecision'
UPDATE UMS_CLAIM SET Title_Ar=N'إضافة محضر الاجتماع' WHERE ClaimValue='Permissions.Submenu.Meeting.MeetingList.AddMOM'
UPDATE UMS_CLAIM SET Title_Ar=N'قائمة الاجتماع' WHERE ClaimValue='Permissions.Submenu.Meeting.MeetingList'
UPDATE UMS_CLAIM SET Title_Ar=N'قائمة الاجتماع' WHERE ClaimValue='Permissions.Menu.Meeting'
--Principles
UPDATE UMS_CLAIM SET Title_Ar=N'المبادئ المعتمدة القائمة الفرعية' WHERE ClaimValue='Permissions.Submenu.LPS.Principles.PrincipleApproval'
UPDATE UMS_CLAIM SET Title_Ar=N'حذف المبدأ القانوني' WHERE ClaimValue='Permissions.Submenu.LPS.Principles.List.Delete'
UPDATE UMS_CLAIM SET Title_Ar=N'عرض تفاصيل المبدأ القانوني' WHERE ClaimValue='Permissions.Submenu.LPS.Principles.Detailsview'
UPDATE UMS_CLAIM SET Title_Ar=N'عرض تفاصيل المبدأ القانوني' WHERE ClaimValue='Permissions.Lps_Latest.Principles.DetailsView'
UPDATE UMS_CLAIM SET Title_Ar=N'زر سجل التعليقات للمبدأ القانوني' WHERE ClaimValue='Permissions.Submenu.LPS.Principles.List.Button.ViewCommenthistory'
UPDATE UMS_CLAIM SET Title_Ar=N'تعديل وسم المبدأ القانوني' WHERE ClaimValue='Permissions.Submenu.LPS.Principles.Tags.Edit'
UPDATE UMS_CLAIM SET Title_Ar=N'إضافة وسم المبدأ القانوني' WHERE ClaimValue='Permissions.Submenu.LPS.Principles.Tags.Add'
UPDATE UMS_CLAIM SET Title_Ar=N'وسم المبدأ القانوني' WHERE ClaimValue='Permissions.Submenu.LPS.Principles.Tags'
UPDATE UMS_CLAIM SET Title_Ar=N'قائمة الوسوم الرئيسية' WHERE ClaimValue='Permissions.Submenu.LPS.Principles.Tags.List'
UPDATE UMS_CLAIM SET Title_Ar=N'زر تفاصيل المبدأ' WHERE ClaimValue='Permissions.Submenu.LPS.Principles.List.Button.Detail'
UPDATE UMS_CLAIM SET Title_Ar=N'زر تاريخ إصدار المبدأ' WHERE ClaimValue='Permissions.Submenu.LPS.Principles.List.Button.VersionHistory'
UPDATE UMS_CLAIM SET Title_Ar=N'زر حذف المبدأ' WHERE ClaimValue='Permissions.LPS.Principles.Delete'
UPDATE UMS_CLAIM SET Title_Ar=N'زر تعديل المبدأ' WHERE ClaimValue='Permissions.Submenu.LPS.Principles.List.Button.Edit'
UPDATE UMS_CLAIM SET Title_Ar=N'زر نشر المبدأ' WHERE ClaimValue='Permissions.Submenu.LPS.Principles.List.Button.Publish'
UPDATE UMS_CLAIM SET Title_Ar=N'عرض المبدأ القانوني المظلل' WHERE ClaimValue='Permissions.Submenu.LPS.PrinciplesMasked.View'
UPDATE UMS_CLAIM SET Title_Ar=N'إنشاء مبدأ قانوني مظلل' WHERE ClaimValue='Permissions.Submenu.LPS.PrinciplesMasked.Create'
UPDATE UMS_CLAIM SET Title_Ar=N'مبادئ مظللة للقائمة الفرعية' WHERE ClaimValue='Permissions.Submenu.LPS.Principles.Masked'
UPDATE UMS_CLAIM SET Title_Ar=N'نشر / إلغاء نشر المبادئ القانونية' WHERE ClaimValue='Permissions.LPS.Principles.PublishUnpublish'
UPDATE UMS_CLAIM SET Title_Ar=N'موافقة / رفض القائمة الفرعية للمبادئ' WHERE ClaimValue='Permissions.Submenu.LPS.Principles.Approval'
UPDATE UMS_CLAIM SET Title_Ar=N'تعديل المبدأ' WHERE ClaimValue='Permissions.LPS.Principles.Edit'
UPDATE UMS_CLAIM SET Title_Ar=N'إنشاء مبدأ ' WHERE ClaimValue='Permissions.LPS.Principles.Create'
UPDATE UMS_CLAIM SET Title_Ar=N'مراجعة المبادئ القانونية' WHERE ClaimValue='Permissions.LPS.Principles.Review'
UPDATE UMS_CLAIM SET Title_Ar=N'عرض تفاصيل تاريخ الإصدار' WHERE ClaimValue='Permissions.LPS.PrincipleVersionHistory.View'
UPDATE UMS_CLAIM SET Title_Ar=N'تاريخ الإصدار' WHERE ClaimValue='Permissions.LPS.PrincipleVersionHistory'
UPDATE UMS_CLAIM SET Title_Ar=N'مبادئ قائمة الهيكيلية للقائمة الفرعية' WHERE ClaimValue='Permissions.Submenu.LPS.PrinciplesHierarchy.List'
UPDATE UMS_CLAIM SET Title_Ar=N'قائمة المبادئ للقائمة الفرعية' WHERE ClaimValue='Permissions.Submenu.LPS.Principles.List'
UPDATE UMS_CLAIM SET Title_Ar=N'نظام إدارة المبادئ القانونية القائمة' WHERE ClaimValue='Permissions.Menu.LPS'
--Inventory
UPDATE UMS_CLAIM SET Title_Ar=N'تفاصيل طلب طلب المخازن' WHERE ClaimValue='Permissions.INV.Inventory.RequestItemDetailView'
UPDATE UMS_CLAIM SET Title_Ar=N'القائمة الفرعية المخازن' WHERE ClaimValue='Permissions.Submenu.INV.Inventory.List'
UPDATE UMS_CLAIM SET Title_Ar=N'قائمة المخازن' WHERE ClaimValue='Permissions.Menu.INV'
UPDATE UMS_CLAIM SET Title_Ar=N'المخازن' WHERE ClaimValue='Permissions.Dashboard.INV.View'
--LLS
UPDATE UMS_CLAIM SET Title_Ar=N'عرض تفاصيل التشريع القانوني' WHERE ClaimValue='Permissions.Submenu.LLS.Legislation.DetailView'
--Literature Management System
UPDATE UMS_CLAIM SET Title_Ar=N'قائمة أقسام الفهرس' WHERE ClaimValue='Permissions.LMS.LiteratureIndexDivison.List'
UPDATE UMS_CLAIM SET Title_Ar=N'إنشاء الفهرس الأساسي' WHERE ClaimValue='Permissions.LMS.LiteratureParentIndex.Create'
UPDATE UMS_CLAIM SET Title_Ar=N'إنشاء قسم الفهرس ' WHERE ClaimValue='Permissions.LMS.LiteratureIndexDivision.Create'
UPDATE UMS_CLAIM SET Title_Ar=N'القائمة الفرعية فهرس الكتاب' WHERE ClaimValue='Permissions.Submenu.LMS.LiteratureIndex'
UPDATE UMS_CLAIM SET Title_Ar=N'القائمة الفرعية الفهرس الرئيسي للكتاب' WHERE ClaimValue='Permissions.Submenu.LMS.LiteratureParentIndex'
UPDATE UMS_CLAIM SET Title_Ar=N'قائمة فهرس الكتاب' WHERE ClaimValue='Permissions.LMS.LiteratureIndex.List'
UPDATE UMS_CLAIM SET Title_Ar=N'عرض فهرس الكتاب' WHERE ClaimValue='Permissions.LMS.LiteratureIndex.View'
UPDATE UMS_CLAIM SET Title_Ar=N'حذف فهرس الكتاب' WHERE ClaimValue='Permissions.LMS.LiteratureIndex.Delete'
UPDATE UMS_CLAIM SET Title_Ar=N'تعديل فهرس الكتاب' WHERE ClaimValue='Permissions.LMS.LiteratureIndex.Edit'
UPDATE UMS_CLAIM SET Title_Ar=N'إنشاء فهرس الكتاب' WHERE ClaimValue='Permissions.LMS.LiteratureIndex.Create'
UPDATE UMS_CLAIM SET Title_Ar=N'القائمة الفرعية للكتاب' WHERE ClaimValue='Permissions.Submenu.LMS.Literatures'
UPDATE UMS_CLAIM SET Title_Ar=N'عرض الكتاب' WHERE ClaimValue='Permissions.LMS.Literatures.View'
UPDATE UMS_CLAIM SET Title_Ar=N'حذف الكتاب' WHERE ClaimValue='Permissions.LMS.Literatures.Delete'
UPDATE UMS_CLAIM SET Title_Ar=N'تعديل الكتاب' WHERE ClaimValue='Permissions.LMS.Literatures.Edit'
UPDATE UMS_CLAIM SET Title_Ar=N'إنشاء الكتاب' WHERE ClaimValue='Permissions.LMS.Literatures.Create'
UPDATE UMS_CLAIM SET Title_Ar=N'موافقة تمديد الاستعارة' WHERE ClaimValue='Permissions.LMS.LiteratureBorrowDetail.ExtensionApproval'
UPDATE UMS_CLAIM SET Title_Ar=N'طلبات الاستعارة الكتاب قائمة فرعية' WHERE ClaimValue='Permissions.Submenu.LMS.LiteratureBorrowDetail.Approval'
UPDATE UMS_CLAIM SET Title_Ar=N'القائمة الفرعية لتمديد الكتاب' WHERE ClaimValue='Permissions.Submenu.LMS.LiteratureBorrowDetail.Extension.ApproveReject'
UPDATE UMS_CLAIM SET Title_Ar=N'القائمة الفرعية لتفاصيل الكتاب' WHERE ClaimValue='Permissions.Submenu.LMS.LiteratureBorrowDetail'
UPDATE UMS_CLAIM SET Title_Ar=N'حالة الموافقة على طلب استعارة الكتاب' WHERE ClaimValue='Permissions.LMS.LiteratureBorrowDetail.BorrowApprovalStatusDiv'
UPDATE UMS_CLAIM SET Title_Ar=N'موافقة/رفض طلب تمديد استعارة الكتاب' WHERE ClaimValue='Permissions.LMS.LiteratureBorrowDetail.Extension.ApproveReject'
UPDATE UMS_CLAIM SET Title_Ar=N'عرض طلب استعارة الكتاب' WHERE ClaimValue='Permissions.LMS.LiteratureBorrowDetail.View'
UPDATE UMS_CLAIM SET Title_Ar=N'حذف طلب استعارة الكتاب' WHERE ClaimValue='Permissions.LMS.LiteratureBorrowDetail.Delete'
UPDATE UMS_CLAIM SET Title_Ar=N'تعديل طلب استعارة الكتاب' WHERE ClaimValue='Permissions.LMS.LiteratureBorrowDetail.Edit'
UPDATE UMS_CLAIM SET Title_Ar=N'إنشاء طلب استعارة الكتاب' WHERE ClaimValue='Permissions.LMS.LiteratureBorrowDetail.Create'
UPDATE UMS_CLAIM SET Title_Ar=N'الكتاب المسترجع المعتمد' WHERE ClaimValue='Permissions.Submenu.LMS.LiteratureReturnDetail.Approval'
UPDATE UMS_CLAIM SET Title_Ar=N'نظام إدارة التشريع القائمة' WHERE ClaimValue='Permissions.Menu.LDS'
--05/02/2024
UPDATE UMS_CLAIM SET Title_Ar=N'تغيير حالة الموظفين' WHERE ClaimValue='Permissions.EM.Employee.Deactivate'
UPDATE UMS_CLAIM SET Title_Ar=N'تعديل بيانات الموظفين' WHERE ClaimValue='Permissions.EM.Employee.Edit'
UPDATE UMS_CLAIM SET Title_Ar=N'عرض تفاصيل الموظف' WHERE ClaimValue='Permissions.EM.Employee.View'
UPDATE UMS_CLAIM SET Title_Ar=N'إضافة موظفين' WHERE ClaimValue='Permissions.EM.Employee.Create'
UPDATE UMS_CLAIM SET Title_Ar=N'قائمة الموظفين' WHERE ClaimValue='Permissions.EM.Employee.List'
UPDATE UMS_CLAIM SET Title_Ar=N'القائمة الفرعية للموظفين' WHERE ClaimValue='Permissions.Submenu.Employee.EmployeeList'
UPDATE UMS_CLAIM SET Title_Ar=N'قائمة الموظفين' WHERE ClaimValue='Permissions.Menu.Employee'
UPDATE UMS_CLAIM SET Title_Ar=N'قائمة المستندات الرئيسية' WHERE ClaimValue='Permissions.DMS.Menu.Document'
UPDATE UMS_CLAIM SET Title_Ar=N'قائمة مستندات الكويت اليوم' WHERE ClaimValue='Permissions.DMS.KAY.DocumentList'
UPDATE UMS_CLAIM SET Title_Ar=N'حذف العقود' WHERE ClaimValue='Permissions.Contact.DeleteContact.Delete'
UPDATE UMS_CLAIM SET Title_Ar=N'تعديل العقود' WHERE ClaimValue='Permissions.Contact.AddContact.Edit'
UPDATE UMS_CLAIM SET Title_Ar=N'إنشاء عقود' WHERE ClaimValue='Permissions.Contact.AddContact.Create'
UPDATE UMS_CLAIM SET Title_Ar=N'قائمة إدارة جهات الاتصال' WHERE ClaimValue='Permissions.Menu.Contact'
UPDATE UMS_CLAIM SET Title_Ar=N'إنشاء مسودة مستند' WHERE ClaimValue='Permissions.CMS.DraftDocument.Create'
UPDATE UMS_CLAIM SET Title_Ar=N'تحويل ملف الاستشاري' WHERE ClaimValue='Permissions.COMS.ConsultationFile.Transfer'
UPDATE UMS_CLAIM SET Title_Ar=N'قائمة ملفات الاستشاري' WHERE ClaimValue='Permissions.Submenu.COMS.ConsultationFile.List'
UPDATE UMS_CLAIM SET Title_Ar=N'التظلمات الإدارية' WHERE ClaimValue='Permissions.Menu.COMS.AdministrativeComplaints'
UPDATE UMS_CLAIM SET Title_Ar=N'التشريع' WHERE ClaimValue='Permissions.Menu.COMS.Legislations'
UPDATE UMS_CLAIM SET Title_Ar=N'التحكيم الدولي' WHERE ClaimValue='Permissions.Menu.COMS.InternationalArbitration'
UPDATE UMS_CLAIM SET Title_Ar=N'الفتوى' WHERE ClaimValue='Permissions.Menu.COMS.LegalAdvice'
UPDATE UMS_CLAIM SET Title_Ar=N'العقود' WHERE ClaimValue='Permissions.Menu.COMS.ContractReview'
UPDATE UMS_CLAIM SET Title_Ar=N'قائمة طلبات الاستشاري' WHERE ClaimValue='Permissions.Submenu.COMS.Request.List'
UPDATE UMS_CLAIM SET Title_Ar=N'نظام إدارة الاستشاري' WHERE ClaimValue='Permissions.Dashboard.COMS.View'
UPDATE UMS_CLAIM SET Title_Ar=N'نظام إدارة ملفات القضايا/الاستشاري' WHERE ClaimValue='Permissions.Dashboard.CMS/COMS.View'
UPDATE UMS_CLAIM SET Title_Ar=N'تفاصيل التواصل' WHERE ClaimValue='Permissions.Comm.Communication.Detail'
UPDATE UMS_CLAIM SET Title_Ar=N'قائمة الوارد والصادر' WHERE ClaimValue='Permissions.Menu.InboxOutbox'
UPDATE UMS_CLAIM SET Title_Ar=N'قائمة طلبات رفع قضية/الاستشاري' WHERE ClaimValue='Permissions.Submenu.CMS/COMS.Request.List'
UPDATE UMS_CLAIM SET Title_Ar=N'إضافة طلب تنفيذ' WHERE ClaimValue='Permissions.CMS.RegisteredCase.AddExecutionRequest'
UPDATE UMS_CLAIM SET Title_Ar=N'قائمة التنفيذ' WHERE ClaimValue='Permissions.CMS.Case.MojExecutionList'
UPDATE UMS_CLAIM SET Title_Ar=N'قائمة القرارات' WHERE ClaimValue='Permissions.CMS.Case.DecisionList'
UPDATE UMS_CLAIM SET Title_Ar=N'تفاصيل طلب التنفيذ' WHERE ClaimValue='Permissions.CMS.MOJ.ExecutionView'
UPDATE UMS_CLAIM SET Title_Ar=N'قائمة طلبات التنفيذ' WHERE ClaimValue='Permissions.CMS.MOJ.ExecutionList'
UPDATE UMS_CLAIM SET Title_Ar=N'تفاصيل طلب حافظة المستندات' WHERE ClaimValue='Permissions.CMS.DocumentPortfolio.RequestDetail'
UPDATE UMS_CLAIM SET Title_Ar=N'قائمة حافظة المستندات الخاصة بوزارة العدل' WHERE ClaimValue='Permissions.CMS.DocumentPortfolio.MojList'
UPDATE UMS_CLAIM SET Title_Ar=N'إنشاء حافظة مستندات' WHERE ClaimValue='Permissions.CMS.DocumentPortfolio.Create'
UPDATE UMS_CLAIM SET Title_Ar=N'إنشاء مسودة مستند' WHERE ClaimValue='Permissions.CMS.DraftDocument.Create'
UPDATE UMS_CLAIM SET Title_Ar=N'عرض المهمة للقضية تم تعيينها' WHERE ClaimValue='Permissions.CMS.RegisteredCase.ReviewCaseTask'
UPDATE UMS_CLAIM SET Title_Ar=N'عرض المهمة لملف القضية تم تعيينه' WHERE ClaimValue='Permissions.CMS.CaseFile.ReviewCaseFileTask'
UPDATE UMS_CLAIM SET Title_Ar=N'قائمة تسجيل القضايا في وزارة العدل' WHERE ClaimValue='Permissions.Submenu.CMS.MOJ.RegistrationList'
UPDATE UMS_CLAIM SET Title_Ar=N'تعيين طلب رفع قضية لعضو' WHERE ClaimValue='Permissions.CMS.CaseRequest.AssignToLawyer'
UPDATE UMS_CLAIM SET Title_Ar=N'إرسال نسخة من طلب رفع قضية' WHERE ClaimValue='Permissions.CMS.CaseRequest.SendCopy'
UPDATE UMS_CLAIM SET Title_Ar=N'طلب اجتماع طلب رفع قضية' WHERE ClaimValue='Permissions.CMS.CaseFile.MeetingRequest'
UPDATE UMS_CLAIM SET Title_Ar=N'تحويل طلب رفع قضية' WHERE ClaimValue='Permissions.CMS.CaseRequest.Transfer'
UPDATE UMS_CLAIM SET Title_Ar=N'تعيين ملف القضية لعضو' WHERE ClaimValue='Permissions.CMS.CaseFile.AssignToLawyer'
UPDATE UMS_CLAIM SET Title_Ar=N'إرسال نسخة من ملف القضية' WHERE ClaimValue='Permissions.CMS.CaseFile.SendCopy'
UPDATE UMS_CLAIM SET Title_Ar=N'إحالة الملف لقطاع' WHERE ClaimValue='Permissions.CMS.CaseFile.AsignToSector'
UPDATE UMS_CLAIM SET Title_Ar=N'مسودة المستندات' WHERE ClaimValue='Permissions.CMS.DraftDocument.List'
UPDATE UMS_CLAIM SET Title_Ar=N'إحالة ملفات القضايا' WHERE ClaimValue='Permissions.CMS.CaseFile.Transfer'
UPDATE UMS_CLAIM SET Title_Ar=N'قائمة المستندات التي تم طلبها' WHERE ClaimValue='Permissions.CMS.RegisteredCase.RequestedDocuments'
UPDATE UMS_CLAIM SET Title_Ar=N'ضم القضايا' WHERE ClaimValue='Permissions.CMS.RegisteredCase.MergeCases'
UPDATE UMS_CLAIM SET Title_Ar=N'إضافة قضية فرعية' WHERE ClaimValue='Permissions.CMS.CaseRequest.AddSubCaseRequest'
UPDATE UMS_CLAIM SET Title_Ar=N'إنشاء طلب تسجيل قضية' WHERE ClaimValue='Permissions.CMS.RegisteredCase.Create'
UPDATE UMS_CLAIM SET Title_Ar=N'عرض تفاصيل القضية' WHERE ClaimValue='Permissions.CMS.RegisteredCase.View'
UPDATE UMS_CLAIM SET Title_Ar=N'تسجيل القضية في وزارة العدل' WHERE ClaimValue='Permissions.CMS.CaseFile.RegistertoMOJ'
UPDATE UMS_CLAIM SET Title_Ar=N'إنشاء مسودة مستند' WHERE ClaimValue='Permissions.CMS.DraftDocument.Create'
UPDATE UMS_CLAIM SET Title_Ar=N'مسودة المستندات' WHERE ClaimValue='Permissions.CMS.CaseDraft.DraftDocument'
UPDATE UMS_CLAIM SET Title_Ar=N'قائمة مسودة المستندات' WHERE ClaimValue='Permissions.CMS.DraftDocument.List'
UPDATE UMS_CLAIM SET Title_Ar=N'عرض تفاصيل مسودة المستند' WHERE ClaimValue='Permissions.CMS.DraftDocument.View'
UPDATE UMS_CLAIM SET Title_Ar=N'عرض تفاصيل ملف القضية' WHERE ClaimValue='Permissions.CMS.CaseFile.View'
UPDATE UMS_CLAIM SET Title_Ar=N'قائمة ملفات القضايا' WHERE ClaimValue='Permissions.Submenu.CMS.CaseFile.List'
UPDATE UMS_CLAIM SET Title_Ar=N'قائمة نظام إدارة القضايا' WHERE ClaimValue='Permissions.Menu.CMS'
UPDATE UMS_CLAIM SET Title_Ar=N'قائمة طلبات رفع قضية' WHERE ClaimValue='Permissions.Submenu.CMS.CaseRequest.List'
UPDATE UMS_CLAIM SET Title_Ar=N'عرض تفاصيل طلب رفع قضية' WHERE ClaimValue='Permissions.CMS.CaseRequest.View'
UPDATE UMS_CLAIM SET Title_Ar=N'القائمة الفرعية لسجل العمليات' WHERE ClaimValue='Permissions.Submenu.AuditLogs.ProcessLogs'
UPDATE UMS_CLAIM SET Title_Ar=N'القائمة الفرعية لسجل الأخطاء' WHERE ClaimValue='Permissions.Submenu.AuditLogs.ErrorLogs'
UPDATE UMS_CLAIM SET Title_Ar=N'قائمة سجل التدقيق' WHERE ClaimValue='Permissions.Menu.AuditLogs'

Update UMS_CLAIM SET SubModule='Communication_Details' WHERE ClaimValue='Permissions.Comm.NeedMoreInfo.List'
Update UMS_CLAIM SET SubModule='Communication_Details' WHERE ClaimValue='Permissions.Comm.NeedMoreInfo.Request'
Update UMS_CLAIM SET SubModule='Communication_Details' WHERE ClaimValue='Permissions.Comm.Communication.Detail'
-------------15/02/2024   Time Tracking Permissions to POS 


insert into UMS_GROUP_CLAIMS values ('9D33079E-1183-4CCE-BF5F-9CEF1605D3AB','Permission','Permissions.Menu.Timelog')

insert into UMS_GROUP_CLAIMS values ('9D33079E-1183-4CCE-BF5F-9CEF1605D3AB','Permission','Permissions.Submenu.Timelog.TimelogList')

insert into UMS_GROUP_CLAIMS values ('D1EFA56E-8E3E-46EC-BFF8-BEF63C8906FC','Permission','Permissions.Menu.Timelog')

insert into UMS_GROUP_CLAIMS values ('D1EFA56E-8E3E-46EC-BFF8-BEF63C8906FC','Permission','Permissions.Submenu.Timelog.TimelogList')

insert into UMS_GROUP_CLAIMS values ('FE25993C-E93A-4801-B412-CDFDFD7A8B73','Permission','Permissions.Menu.Timelog')

insert into UMS_GROUP_CLAIMS values ('FE25993C-E93A-4801-B412-CDFDFD7A8B73','Permission','Permissions.Submenu.Timelog.TimelogList')


IF((SELECT COUNT(*) FROM UMS_CLAIM WHERE ClaimValue = 'Permissions.Dashboard.OnDemandPortal.View') = 0)
BEGIN
	INSERT INTO UMS_CLAIM VALUES ('On Demand Request Portal Card','FATWA_Dashboard','On_Demand_Request_Portal', 'Permission', 'Permissions.Dashboard.OnDemandPortal.View','On Demand Request Portal Card',0)
END
GO

IF((SELECT COUNT(*) FROM UMS_CLAIM WHERE ClaimValue = 'Permissions.Menu.ODRP') = 0)
BEGIN
	INSERT INTO UMS_CLAIM VALUES ('On Demand Requests Menu','FATWA_Dashboard','On_Demand_Request_Portal', 'Permission', 'Permissions.Menu.ODRP','On Demand Requests Menu',0)
END
GO

IF((SELECT COUNT(*) FROM UMS_CLAIM WHERE ClaimValue = 'Permissions.ODRP.MojRolls.List') = 0)
BEGIN
	INSERT INTO UMS_CLAIM VALUES ('Moj Rolls List','On_Demand_Request_Portal','MOJ_Rolls', 'Permission', 'Permissions.ODRP.MojRolls.List','Moj Rolls List',0)
END
GO

IF((SELECT COUNT(*) FROM UMS_CLAIM WHERE ClaimValue = 'Permissions.ODRP.PACI.List') = 0)
BEGIN
	INSERT INTO UMS_CLAIM VALUES ('PACI List','On_Demand_Request_Portal','PACI_Address_Query', 'Permission', 'Permissions.ODRP.PACI.List','PACI List',0)
END
GO

IF((SELECT COUNT(*) FROM UMS_CLAIM WHERE ClaimValue = 'Permissions.ODRP.MojStatistics.List') = 0)
BEGIN
	INSERT INTO UMS_CLAIM VALUES ('Moj Statistics List','On_Demand_Request_Portal','Moj_Statistics', 'Permission', 'Permissions.ODRP.MojStatistics.List','Moj Statistics List',0)
END
GO

IF((SELECT COUNT(*) FROM UMS_CLAIM WHERE ClaimValue = 'Permissions.ODRP.MojStatistics.View') = 0)
BEGIN
	INSERT INTO UMS_CLAIM VALUES ('Moj Statistics Detail','On_Demand_Request_Portal','Moj_Statistics', 'Permission', 'Permissions.ODRP.MojStatistics.View','Moj Statistics Detail',0)
END
GO

IF((SELECT COUNT(*) FROM UMS_CLAIM WHERE ClaimValue = 'Permissions.ODRP.MojStatisticsCaseStudy.List') = 0)
BEGIN
	INSERT INTO UMS_CLAIM VALUES ('Moj Statistics Case Study List','On_Demand_Request_Portal','Moj_Statistics_Case_Study', 'Permission', 'Permissions.ODRP.MojStatisticsCaseStudy.List','Moj Statistics Case Study List',0)
END
GO

IF((SELECT COUNT(*) FROM UMS_CLAIM WHERE ClaimValue = 'Permissions.ODRP.MojStatisticsCaseStudy.View') = 0)
BEGIN
	INSERT INTO UMS_CLAIM VALUES ('Moj Statistics Case Study Detail','On_Demand_Request_Portal','Moj_Statistics_Case_Study', 'Permission', 'Permissions.ODRP.MojStatisticsCaseStudy.View','Moj Statistics Case Study Detail',0)
END
GO
IF((SELECT COUNT(*) FROM UMS_CLAIM WHERE ClaimValue = 'Permissions.ODRP.MojRolls.CustomList') = 0)
BEGIN
	INSERT INTO UMS_CLAIM VALUES ('Moj Custom Rolls List','On_Demand_Request_Portal','MOJ_Rolls', 'Permission', 'Permissions.ODRP.MojRolls.CustomList','Moj Custom Rolls List',0)
END
GO

INSERT INTO UMS_GROUP_CLAIMS VALUES ('071C4BA0-CC3F-43DC-B2CB-6FC2BEA6D831', 'Permission', 'Permissions.Dashboard.OnDemandPortal.View')
INSERT INTO UMS_GROUP_CLAIMS VALUES ('071C4BA0-CC3F-43DC-B2CB-6FC2BEA6D831', 'Permission', 'Permissions.Menu.ODRP')
INSERT INTO UMS_GROUP_CLAIMS VALUES ('071C4BA0-CC3F-43DC-B2CB-6FC2BEA6D831', 'Permission', 'Permissions.ODRP.MojRolls.List')
INSERT INTO UMS_GROUP_CLAIMS VALUES ('071C4BA0-CC3F-43DC-B2CB-6FC2BEA6D831', 'Permission', 'Permissions.ODRP.PACI.List')
INSERT INTO UMS_GROUP_CLAIMS VALUES ('071C4BA0-CC3F-43DC-B2CB-6FC2BEA6D831', 'Permission', 'Permissions.ODRP.MojStatistics.List')
INSERT INTO UMS_GROUP_CLAIMS VALUES ('071C4BA0-CC3F-43DC-B2CB-6FC2BEA6D831', 'Permission', 'Permissions.ODRP.MojStatistics.View')
INSERT INTO UMS_GROUP_CLAIMS VALUES ('071C4BA0-CC3F-43DC-B2CB-6FC2BEA6D831', 'Permission', 'Permissions.ODRP.MojStatisticsCaseStudy.List')
INSERT INTO UMS_GROUP_CLAIMS VALUES ('071C4BA0-CC3F-43DC-B2CB-6FC2BEA6D831', 'Permission', 'Permissions.ODRP.MojStatisticsCaseStudy.View')
INSERT INTO UMS_GROUP_CLAIMS VALUES ('071C4BA0-CC3F-43DC-B2CB-6FC2BEA6D831', 'Permission', 'Permissions.ODRP.MojRolls.CustomList')

--------------- DMS ------------------
insert into ATTACHMENT_TYPE (AttachmentTypeId,Type_Ar,Type_En,ModuleId,IsMandatory,IsOfficialLetter,SubTypeId,IsGePortalType,IsOpinion,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,DeletedBy,DeletedDate,IsDeleted,IsActive,IsSystemDefine,Description)
values (90,'Order On Petition Notes','Order On Petition Notes',5,0,0,null,0,0,'fatwaadmin@gmail.com','2023-08-13 17:01:00.000',null,null,null,null,0,1,1,null)
insert into ATTACHMENT_TYPE (AttachmentTypeId,Type_Ar,Type_En,ModuleId,IsMandatory,IsOfficialLetter,SubTypeId,IsGePortalType,IsOpinion,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,DeletedBy,DeletedDate,IsDeleted,IsActive,IsSystemDefine,Description)
values (91,'Perform Order Notes','Perform Order Notes',5,0,0,null,0,0,'fatwaadmin@gmail.com','2023-08-13 17:01:00.000',null,null,null,null,0,1,1,null)
insert into ATTACHMENT_TYPE (AttachmentTypeId,Type_Ar,Type_En,ModuleId,IsMandatory,IsOfficialLetter,SubTypeId,IsGePortalType,IsOpinion,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,DeletedBy,DeletedDate,IsDeleted,IsActive,IsSystemDefine,Description)
values (92,'Bug','Bug',13,0,0,null,1,0,'fatwaadmin@gmail.com','2023-08-13 17:01:00.000',null,null,null,null,0,1,1,null)
insert into ATTACHMENT_TYPE (AttachmentTypeId,Type_Ar,Type_En,ModuleId,IsMandatory,IsOfficialLetter,SubTypeId,IsGePortalType,IsOpinion,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,DeletedBy,DeletedDate,IsDeleted,IsActive,IsSystemDefine,Description)
values (93,'Order On Petition Court Decision','Order On Petition Court Decision',5,0,0,null,0,0,'fatwaadmin@gmail.com','2023-08-13 17:01:00.000',null,null,null,null,0,1,1,null)
insert into ATTACHMENT_TYPE (AttachmentTypeId,Type_Ar,Type_En,ModuleId,IsMandatory,IsOfficialLetter,SubTypeId,IsGePortalType,IsOpinion,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,DeletedBy,DeletedDate,IsDeleted,IsActive,IsSystemDefine,Description)
values (94,'Perform Order Court Decision','Perform Order Court Decision',5,0,0,null,0,0,'fatwaadmin@gmail.com','2023-08-13 17:01:00.000',null,null,null,null,0,1,1,null)

-----------------------19/2/24------------------
SET IDENTITY_INSERT [dbo].CMS_OPERATING_SECTOR_TYPE_G2G_LKP ON 
INSERT INTO CMS_OPERATING_SECTOR_TYPE_G2G_LKP
(id,CODE, Name_En, Name_Ar, IsActive, DepartmentId, ModuleId)
VALUES(23,'GS','General Services' ,N'خدمات عامة',1, 2, 8)
INSERT INTO CMS_OPERATING_SECTOR_TYPE_G2G_LKP
(id,CODE, Name_En, Name_Ar, IsActive, DepartmentId, ModuleId)
VALUES(24,'IT','Information Technology' ,N'تكنولوجيا المعلومات',1, 2, 8)
SET IDENTITY_INSERT [dbo].CMS_OPERATING_SECTOR_TYPE_G2G_LKP OFF 

--------Moj Image Document List Claim------------
IF((SELECT COUNT(*) FROM UMS_CLAIM WHERE ClaimValue = 'Permissions.DMS.MojImage.DocumentList') = 0)
BEGIN
	INSERT INTO UMS_CLAIM VALUES ('Moj Images Document List','Document_Management_System','MOJ_Images_Document', 'Permission', 'Permissions.DMS.MojImage.DocumentList','Moj Images Document List',0)
END
GO
INSERT INTO UMS_GROUP_CLAIMS VALUES ('071C4BA0-CC3F-43DC-B2CB-6FC2BEA6D831', 'Permission', 'Permissions.DMS.MojImage.DocumentList')






IF((SELECT COUNT(*) FROM UMS_CLAIM WHERE ClaimValue = 'Permissions.ODRP.KAY.List') = 0)
BEGIN
	INSERT INTO UMS_CLAIM VALUES ('Kuwait Alyum Publications List','On_Demand_Request_Portal','Kuwait_AlYawm', 'Permission', 'Permissions.ODRP.KAY.List','Kuwait Alyum Publications List',0)
END
GO
INSERT INTO UMS_GROUP_CLAIMS VALUES ('071C4BA0-CC3F-43DC-B2CB-6FC2BEA6D831', 'Permission', 'Permissions.ODRP.KAY.List')

-----------22-02-2024------------
SET IDENTITY_INSERT CMS_OPERATING_SECTOR_TYPE_G2G_LKP Off
INSERT INTO CMS_OPERATING_SECTOR_TYPE_G2G_LKP VALUES ('COMSGS','Consultation General Supervisor','Consultation General Supervisor',1,1,20,'fatwaadmin@gmail.com',GETDATE(),NULL,NULL,NULL,NULL,0,NULL);
INSERT INTO CMS_OPERATING_SECTOR_TYPE_G2G_LKP VALUES ('SD','Services Department','Services Department',1,2,23,'fatwaadmin@gmail.com',GETDATE(),NULL,NULL,NULL,NULL,0,8);
INSERT INTO CMS_OPERATING_SECTOR_TYPE_G2G_LKP VALUES ('MEAD','Maintenance AND Engineering Affair Department','Services Department',1,2,23,'fatwaadmin@gmail.com',GETDATE(),NULL,NULL,NULL,NULL,0,8);
INSERT INTO CMS_OPERATING_SECTOR_TYPE_G2G_LKP VALUES ('PRD','Public Record Department','Public Record Department',1,2,26,'fatwaadmin@gmail.com',GETDATE(),NULL,NULL,NULL,NULL,0,8);
INSERT INTO CMS_OPERATING_SECTOR_TYPE_G2G_LKP VALUES ('TCS','Technical Support Controller','Technical Support Controller',1,2,24,'fatwaadmin@gmail.com',GETDATE(),NULL,NULL,NULL,NULL,0,8);
INSERT INTO CMS_OPERATING_SECTOR_TYPE_G2G_LKP VALUES ('SDC','System Development Controller','System Development Controller',1,2,24,'fatwaadmin@gmail.com',GETDATE(),NULL,NULL,NULL,NULL,0,8);
INSERT INTO CMS_OPERATING_SECTOR_TYPE_G2G_LKP VALUES ('OC','Operation Controller','Operation Controller',1,2,24,'fatwaadmin@gmail.com',GETDATE(),NULL,NULL,NULL,NULL,0,8);
INSERT INTO CMS_OPERATING_SECTOR_TYPE_G2G_LKP VALUES ('DD','Database Department','Database Department',1,2,29,'fatwaadmin@gmail.com',GETDATE(),NULL,NULL,NULL,NULL,0,8);
INSERT INTO CMS_OPERATING_SECTOR_TYPE_G2G_LKP VALUES ('TDD','Technical Development Dapartment','echnical Development Dapartment',1,2,29,'fatwaadmin@gmail.com',GETDATE(),NULL,NULL,NULL,NULL,0,8);
INSERT INTO CMS_OPERATING_SECTOR_TYPE_G2G_LKP VALUES ('PPD','Project Planning Department','Project Planning Department',1,2,30,'fatwaadmin@gmail.com',GETDATE(),NULL,NULL,NULL,NULL,0,8);
INSERT INTO CMS_OPERATING_SECTOR_TYPE_G2G_LKP VALUES ('PEFD','Project Execution And Follow Up Dapartment','Project Execution And Follow Up Dapartment',1,2,30,'fatwaadmin@gmail.com',GETDATE(),NULL,NULL,NULL,NULL,0,8);
INSERT INTO CMS_OPERATING_SECTOR_TYPE_G2G_LKP VALUES ('OEPD','Operation Executaion & Planning Department','Operation Executaion & Planning Department',1,2,31,'fatwaadmin@gmail.com',GETDATE(),NULL,NULL,NULL,NULL,0,8);
INSERT INTO CMS_OPERATING_SECTOR_TYPE_G2G_LKP VALUES ('OCDPD','Operation Controll And Document Processing Deaprtment','Operation Controll And Document Processing Deaprtment',1,2,31,'fatwaadmin@gmail.com',GETDATE(),NULL,NULL,NULL,NULL,0,8);
UPDATE CMS_OPERATING_SECTOR_TYPE_G2G_LKP SET ParentId=21 WHERE Name_En IN ('General Services','Information Technology');
UPDATE CMS_OPERATING_SECTOR_TYPE_G2G_LKP SET ParentId=20 WHERE Name_En IN ('Administrative General Supervisor','Civil/Commercial General Supervisor','Consultation General Supervisor','Assistant Undersecretary for Financial and Administrative Affairs','Private Operational Sector','Public Operational Sector');
UPDATE CMS_OPERATING_SECTOR_TYPE_G2G_LKP SET ParentId=23 WHERE Name_En IN ('Administrative Complaints','Contracts','Legislations','Legal Advice','International Arbitration ');
UPDATE CMS_OPERATING_SECTOR_TYPE_G2G_LKP SET ParentId=11 WHERE Name_En IN ('Civil Commercial Under Filing Cases','Civil Commercial Regional Cases','Civil Commercial Appeal Cases','Civil Commercial Supreme Cases','Civil Commercial Partial/Urgent Sector');
UPDATE CMS_OPERATING_SECTOR_TYPE_G2G_LKP SET ParentId=10 WHERE Name_En IN (' Administrative Under Filing Cases','Administrative Regional Cases test','Administrative Appeal Cases','Administrative Supreme Cases');
INSERT INTO UMS_GROUP_CLAIMS VALUES ('071C4BA0-CC3F-43DC-B2CB-6FC2BEA6D831', 'Permission', 'Permissions.ODRP.KAY.List')


-------------------------------------------UMS_ROLE 2/22/2024
Update UMS_ROLE Set NameAr = N'رئيس قطاع الاستشاري' where Id ='1dbe8947-fa41-4f1c-a150-fe272e27b06c'
Update UMS_ROLE Set NameAr = N'مدخل التشريعات' where Id ='279E1469-599D-4D56-944A-DA3E6A55148F'
Update UMS_ROLE Set NameAr = N'عضو قطاع الاستشاري' where Id ='35bae56b-6523-477a-a8ca-bbf6fa2d4647'
Update UMS_ROLE Set NameAr = N'محرر التشريعات' where Id ='467af2c7-7bfa-4cf7-bbe8-8198a3340497'
Update UMS_ROLE Set NameAr = N'مندوب اطلاع' where Id ='4eae855f-500f-4912-90fc-fe399fcb6fea'
Update UMS_ROLE Set NameAr = N'عضو قطاع القضايا' where Id ='8b6cfa36-914a-4430-9feb-627e11715113'
Update UMS_ROLE Set NameAr = N'رئيس قطاع القضايا' where Id ='93e5374b-cbd9-494e-92d4-d9d7d44c2c39'
Update UMS_ROLE Set NameAr = N'مسؤول نظام التشريع' where Id ='9578924a-127b-4e33-b15a-5643ecadaf7c'
Update UMS_ROLE Set NameAr = N'محرر المبادئ القانونية' where Id ='9A203FC5-F85D-423D-B57C-2AC9DDDAEE57'
Update UMS_ROLE Set NameAr = N'مراجع المبادئ القانونية' where Id ='a484bb1b-0423-4e12-a3c6-f347a0236d85'
Update UMS_ROLE Set NameAr = N'مسؤول نظام المكتبة' where Id ='abe81828-560a-4efa-8bf0-a5f02738bcf6'
Update UMS_ROLE Set NameAr = N'مدخل المبادئ القانونية' where Id ='B6FE35A6-6A6B-44FD-A70A-B65C88B23DF1'
Update UMS_ROLE Set NameAr = N'مراجع التشريعات' where Id ='cccb3715-0e55-4a1c-a7e2-c12fd20d7a4a'
Update UMS_ROLE Set NameAr = N'مستخدم التشريعات' where Id ='cd8013fc-6d54-45cb-ab92-73286cec48d3'
Update UMS_ROLE Set NameAr = N'مسؤول النظام' where Id ='d6b3075c-3f70-4b44-baa4-1fdc599a6bb2'
Update UMS_ROLE Set NameAr = N'رئيس قطاع المكتب الفني العام' where Id ='e1e17355-216f-463b-918e-e4d898e01457'
Update UMS_ROLE Set NameAr = N'مشرف فني قطاع الاستشاري' where Id ='ec11b80f-2429-44d0-a5e1-1e144752e579'
Update UMS_ROLE Set NameAr = N'مشرف فني قطاع القضايا' where Id ='f2c87c20-3a38-4a20-b238-ec643ebd0df9'
Update UMS_ROLE Set NameAr = N'مستخدم المبادئ القانونية' where Id ='fca2fe9b-aba5-416d-bb49-e81819fe563b'


---------------29/02/2024
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EP_EMPLOYMENT_INFORMATION]') AND type in (N'U'))
Update  EP_EMPLOYMENT_INFORMATION
set SectorTypeId = 16 
where UserId = '96b970bd-94bc-4eb4-9f6a-1f8a6d8b950a'


IF((SELECT COUNT(*) FROM UMS_CLAIM WHERE ClaimValue = 'Permissions.Submenu.CMS.CaseFile.UnassignedList') = 0)
BEGIN
	INSERT INTO UMS_CLAIM VALUES ('Unassigned Case Files List Migrated From MOJ','CMS','Case_Files', 'Permission', 'Permissions.Submenu.CMS.CaseFile.UnassignedList','Unassigned Case Files List Migrated From MOJ',0)
END
GO
INSERT INTO UMS_GROUP_CLAIMS VALUES ('071C4BA0-CC3F-43DC-B2CB-6FC2BEA6D831', 'Permission', 'Permissions.Submenu.CMS.CaseFile.UnassignedList')


IF((SELECT COUNT(*) FROM UMS_CLAIM WHERE ClaimValue = 'Permissions.CMS.RegisteredCase.AddSubCase') = 0)
BEGIN
	INSERT INTO UMS_CLAIM VALUES ('Create Sub Case','Case_Management_System','Registered_Case', 'Permission', 'Permissions.CMS.RegisteredCase.AddSubCase','Create Sub Case',0)
END
GO


INSERT INTO CMS_COURT_CHAMBER (ChamberId, CourtId, CreatedBy, CreatedDate, ModifiedBy, ModifiedDate,DeletedBy,DeletedDate,IsDeleted)
SELECT Id, CourtId, 'System Generated', '2024-02-22 15:57:08.043', NULL, NULL, NULL, NULL, 0
FROM CMS_CHAMBER_G2G_LKP;


INSERT INTO CMS_CHAMBER_CHAMBER_NUMBER (ChamberNumberId, ChamberId, CreatedBy, CreatedDate, ModifiedBy, ModifiedDate,DeletedBy,DeletedDate,IsDeleted)
SELECT Id, ChamberId, 'System Generated', '2024-02-22 15:57:08.043', NULL, NULL, NULL, NULL, 0
FROM CMS_CHAMBER_NUMBER_G2G_LKP;

INSERT INTO UMS_GROUP_CLAIMS VALUES ('071C4BA0-CC3F-43DC-B2CB-6FC2BEA6D831', 'Permission', 'Permissions.DMS.Template.Add')
INSERT INTO UMS_GROUP_CLAIMS VALUES ('071C4BA0-CC3F-43DC-B2CB-6FC2BEA6D831', 'Permission', 'Permissions.DMS.Template.List')

-----------------------------Module 4/3/24-----------------------------------
INSERT INTO [dbo].[MODULE]([ModuleNameEn],[ModuleNameAr])
VALUES('NumberPatternWorkerService',N'NumberPatternWorkerService')
INSERT INTO [dbo].[MODULE]([ModuleNameEn],[ModuleNameAr])
VALUES('InventoryManagement',N'InventoryManagement')
INSERT INTO UMS_GROUP_CLAIMS VALUES ('071C4BA0-CC3F-43DC-B2CB-6FC2BEA6D831', 'Permission', 'Permissions.DMS.Template.List')

-------------------------------------
Update CMS_COURT_TYPE_G2G_LKP Set Name_En = 'Appeal' , Name_Ar =N'جاذبية' where Id = 2
Update CMS_COURT_TYPE_G2G_LKP Set Name_En = 'Supreme' , Name_Ar =N'أعلى فائق' where Id = 4
---------------------------------------
Update CMS_COURT_G2G_LKP set TypeId = null where TypeId = 2
Update CMS_COURT_G2G_LKP set TypeId = 2 where TypeId = 4
Update CMS_COURT_G2G_LKP set TypeId = 4 where TypeId IS NULL
------------------------------------------
Update CMS_CASE_REQUEST set CourtTypeId = null where CourtTypeId = 2  AND StatusId = 2
Update CMS_CASE_REQUEST set CourtTypeId = 2 where CourtTypeId = 4 AND StatusId = 2
Update CMS_CASE_REQUEST set CourtTypeId = 4 where CourtTypeId IS NULL AND StatusId = 2


INSERT INTO UMS_GROUP_CLAIMS VALUES ('071C4BA0-CC3F-43DC-B2CB-6FC2BEA6D831', 'Permission', 'Permissions.DMS.Template.List')

--Ammaar Naveed---07/03/2024--Implementation for Sector Type Lookup History
--Register Lookup History Table
INSERT INTO LOOKUPS_TABLES(TableName, IsActive, TablesEnumvalues)
VALUES('CMS_OPERATING_SECTOR_TYPE_G2G_LKP', 1, 15 )


IF((SELECT COUNT(*) FROM CMS_CASE_FILE_EVENT_G2G_LKP WHERE Id = '32768') <= 0)
SET IDENTITY_INSERT CMS_CASE_FILE_EVENT_G2G_LKP ON
INSERT INTO CMS_CASE_FILE_EVENT_G2G_LKP (Id,Name_En,Name_Ar)VALUES(32768,'MigratedFromMOJ','MigratedFromMOJ')
SET IDENTITY_INSERT CMS_CASE_FILE_EVENT_G2G_LKP OFF
GO


IF((SELECT COUNT(*) FROM CMS_REGISTERED_CASE_EVENT_G2G_LKP WHERE Id = 16) = 0)
SET IDENTITY_INSERT CMS_REGISTERED_CASE_EVENT_G2G_LKP ON
INSERT INTO CMS_REGISTERED_CASE_EVENT_G2G_LKP (Id, Name_En, Name_Ar) VALUES (16,'MigratedFromMOJ',N'MigratedFromMOJ')
SET IDENTITY_INSERT CMS_REGISTERED_CASE_EVENT_G2G_LKP OFF
GO

/*<History Author='Ihsaan Abbas' Date='11-03-2024'>CMS_BANK_G2G_LKP </History>*/
INSERT INTO [dbo].[CMS_BANK_G2G_LKP] ([Name_En], [Name_Ar], [CreatedBy], [CreatedDate], [IsDeleted]) 
VALUES ('National Bank of Pakistan', N'البنك الوطني لباكستان', 'fatwaadmin@gmail.com', GETDATE(), 0);

INSERT INTO [dbo].[CMS_BANK_G2G_LKP] ([Name_En], [Name_Ar], [CreatedBy], [CreatedDate], [IsDeleted]) 
VALUES ('United Bank Limited', N'متحدہ بینک محدود', 'fatwaadmin@gmail.com', GETDATE(), 0);

INSERT INTO [dbo].[CMS_BANK_G2G_LKP] ([Name_En], [Name_Ar], [CreatedBy], [CreatedDate], [IsDeleted]) 
VALUES ('Habib Bank Limited', N'حبیب بینک لمیٹڈ', 'fatwaadmin@gmail.com', GETDATE(), 0);

INSERT INTO [dbo].[CMS_BANK_G2G_LKP] ([Name_En], [Name_Ar], [CreatedBy], [CreatedDate], [IsDeleted]) 
VALUES ('MCB Bank Limited', N'ام سی بی بینک لمیٹڈ', 'fatwaadmin@gmail.com', GETDATE(), 0);

INSERT INTO [dbo].[CMS_BANK_G2G_LKP] ([Name_En], [Name_Ar], [CreatedBy], [CreatedDate], [IsDeleted]) 
VALUES ('Allied Bank Limited', N'متحدہ بینک لمیٹڈ', 'fatwaadmin@gmail.com', GETDATE(), 0);

INSERT INTO [dbo].[CMS_BANK_G2G_LKP] ([Name_En], [Name_Ar], [CreatedBy], [CreatedDate], [IsDeleted]) 
VALUES ('Askari Bank Limited', N'اسکاری بینک لمیٹڈ', 'fatwaadmin@gmail.com', GETDATE(), 0);

INSERT INTO [dbo].[CMS_BANK_G2G_LKP] ([Name_En], [Name_Ar], [CreatedBy], [CreatedDate], [IsDeleted]) 
VALUES ('Bank Alfalah Limited', N'بینک الفلاح لمیٹڈ', 'fatwaadmin@gmail.com', GETDATE(), 0);

INSERT INTO [dbo].[CMS_BANK_G2G_LKP] ([Name_En], [Name_Ar], [CreatedBy], [CreatedDate], [IsDeleted]) 
VALUES ('Faysal Bank Limited', N'فیصل بینک لمیٹڈ', 'fatwaadmin@gmail.com', GETDATE(), 0);



--Ammaar Naveed--10/03/2024--Updating ModuleId to sync with Enums
UPDATE UMS_CLAIM SET ModuleId = 1 WHERE Module='Legal_Library_System'
UPDATE UMS_CLAIM SET ModuleId = 2 WHERE Module='Case_Management_System'
UPDATE UMS_CLAIM SET ModuleId = 4 WHERE Module='Consultation_Management_System'
UPDATE UMS_CLAIM SET ModuleId = 8 WHERE Module='Inventory_Management'
UPDATE UMS_CLAIM SET ModuleId = 16 WHERE Module='Document_Management_System'
UPDATE UMS_CLAIM SET ModuleId = 32 WHERE Module='Workflow_Management'
UPDATE UMS_CLAIM SET ModuleId = 64 WHERE Module='Employee_Management'
UPDATE UMS_CLAIM SET ModuleId = 128 WHERE Module='Case_Consultation_Management_System'
UPDATE UMS_CLAIM SET ModuleId = 256 WHERE Module='On_Demand_Request_Portal'
UPDATE UMS_CLAIM SET ModuleId = 512 WHERE Module='Library_Management_System'
UPDATE UMS_CLAIM SET ModuleId = 1024 WHERE Module='Legislation_Management_System'
UPDATE UMS_CLAIM SET ModuleId = 2048 WHERE Module='Legal_Principles_Management_System'
UPDATE UMS_CLAIM SET ModuleId = 4096 WHERE Module='Contact_Management'
UPDATE UMS_CLAIM SET ModuleId = 8192 WHERE Module='User_Management'
UPDATE UMS_CLAIM SET ModuleId = 16384 WHERE Module='Meeting'
UPDATE UMS_CLAIM SET ModuleId = 32768 WHERE Module='Communication'
UPDATE UMS_CLAIM SET ModuleId = 65536 WHERE Module='Notification'
UPDATE UMS_CLAIM SET ModuleId = 131072 WHERE Module='Task'
UPDATE UMS_CLAIM SET ModuleId = 262144 WHERE Module='Audit_Logs'
UPDATE UMS_CLAIM SET ModuleId = 524288 WHERE Module='Timelog'
UPDATE UMS_CLAIM SET ModuleId = 1048576 WHERE Module='FATWA_Dashboard'
UPDATE UMS_CLAIM SET ModuleId = 2097152 WHERE Module='Sidemenu'
--Ammaar Naveed--11/03/2024--Updating Module Name
UPDATE UMS_CLAIM SET Module='Case_Management_System' WHERE ClaimValue='Permissions.Submenu.CMS.CaseFile.UnassignedList'
--Ammaar Naveed--12/03/2024--Updating Module for Legislation Detail View
UPDATE UMS_CLAIM SET Module='Legislation_Management_System' WHERE ClaimValue='Permissions.Submenu.LLS.Legislation.DetailView'
UPDATE UMS_CLAIM SET ClaimValue='Permissions.Submenu.LDS.Legislation.DetailView' WHERE ClaimValue='Permissions.Submenu.LLS.Legislation.DetailView'
UPDATE UMS_CLAIM SET ModuleId=1024 WHERE ClaimValue='Permissions.Submenu.LDS.Legislation.DetailView'
GO


/*<History Author='Umer Zaman' Date='05-03-2024'> Script for OSS </History>*/

IF NOT Exists(SELECT 1 from UMS_ROLE where Name = 'StoreKeeper')
	INSERT INTO [dbo].[UMS_ROLE] ([Id],[Name],[NormalizedName],[ConcurrencyStamp],[Description_En],[CreatedBy],[CreatedDate],[ModifiedBy],[ModifiedDate],[DeletedBy],[DeletedDate],[IsDeleted],[Description_Ar],[NameAr])
		VALUES (NewID(),'StoreKeeper','STOREKEEPER',NewID(),'StoreKeeper','fatwaadmin@gmail.com',GETDATE(),NULL,NULL,NULL,NULL,0,'StoreKeeper',N'امين المخزن')
GO

IF NOT Exists(SELECT 1 from UMS_ROLE where Name = 'Custodian')
	INSERT INTO [dbo].[UMS_ROLE] ([Id],[Name],[NormalizedName],[ConcurrencyStamp],[Description_En],[CreatedBy],[CreatedDate],[ModifiedBy],[ModifiedDate],[DeletedBy],[DeletedDate],[IsDeleted],[Description_Ar],[NameAr])
		VALUES (NewID(),'Custodian','CUSTODIAN',NewID(),'Custodian','fatwaadmin@gmail.com',GETDATE(),NULL,NULL,NULL,NULL,0,'Custodian',N'وصي')
GO

IF NOT Exists(SELECT 1 from EP_DESIGNATION where Name_En = 'StoreKeeper')
	INSERT INTO [dbo].[EP_DESIGNATION] ([Name_En],[Name_Ar],[CreatedBy],[CreatedDate],[ModifiedBy],[ModifiedDate],[DeletedBy],[DeletedDate],[IsDeleted])
		VALUES ('StoreKeeper',N'امين المخزن','fatwaadmin@gmail.com',GETDATE(),NULL,NULL,NULL,NULL,0)
GO

IF NOT Exists(SELECT 1 from EP_DESIGNATION where Name_En = 'Custodian')
INSERT INTO [dbo].[EP_DESIGNATION] ([Name_En],[Name_Ar],[CreatedBy],[CreatedDate],[ModifiedBy],[ModifiedDate],[DeletedBy],[DeletedDate],[IsDeleted])
		VALUES ('Custodian',N'وصي','fatwaadmin@gmail.com',GETDATE(),NULL,NULL,NULL,NULL,0)
GO
IF NOT Exists(SELECT 1 from UMS_ROLE where Name = 'Procurement')
	INSERT INTO [dbo].[UMS_ROLE] ([Id],[Name],[NormalizedName],[ConcurrencyStamp],[Description_En],[CreatedBy],[CreatedDate],[ModifiedBy],[ModifiedDate],[DeletedBy],[DeletedDate],[IsDeleted],[Description_Ar],[NameAr])
		VALUES (NewID(),'Procurement','PROCUREMENT',NewID(),'Procurement','fatwaadmin@gmail.com',GETDATE(),NULL,NULL,NULL,NULL,0,'Procurement',N'مشتريات')
GO

IF NOT Exists(SELECT 1 from EP_DESIGNATION where Name_En = 'Procurement')
	INSERT INTO [dbo].[EP_DESIGNATION] ([Name_En],[Name_Ar],[CreatedBy],[CreatedDate],[ModifiedBy],[ModifiedDate],[DeletedBy],[DeletedDate],[IsDeleted])
		VALUES ('Procurement',N'مشتريات','fatwaadmin@gmail.com',GETDATE(),NULL,NULL,NULL,NULL,0)
GO
IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Permissions.INV.Inventory.ServiceRequest.Create')
INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted],[ModuleId])
		VALUES ('Create Service Request','Inventory_Management','Inventory','Permission','Permissions.INV.Inventory.ServiceRequest.Create', 'Create Service Request', 0, 8)
GO
IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Permissions.INV.Inventory.ServiceRequest.Edit')
INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted],[ModuleId])
		VALUES ('Edit Service Request','Inventory_Management','Inventory','Permission','Permissions.INV.Inventory.ServiceRequest.Edit', 'Edit Service Request', 0, 8)
GO
/*<History Author='Umer Zaman' Date='05-03-2024'> Script for OSS </History>*/
UPDATE UMS_CLAIM SET ModuleId=1024 WHERE ClaimValue='Permissions.Submenu.LDS.Legislation.DetailView'

------ 03-25-2024 start --------
UPDATE tTranslation SET Value_En = 'Please fill all the required fields.', Value_Ar = N'يرجى ملء جميع الحقول المطلوبة' WHERE TranslationKey = 'Fill_Required_Fields'
------ 03-25-2024 end --------

----------------- 26-3-2024 --------------

GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId]) VALUES (1, N'New Request', N'طلب جديد', N'System Generated', CAST(N'2022-09-06T12:27:53.330' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId]) VALUES (2, N'Assign to Lawyer', N'Assign to Lawyer', N'System Generated', CAST(N'2022-09-06T12:27:53.330' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId]) VALUES (3, N'Share Document', N'Share Document', N'System Generated', CAST(N'2022-09-06T12:27:53.400' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId]) VALUES (4, N'Legal Notification Response', N'Legal Notification Response', N'System Generated', CAST(N'2022-09-06T12:27:43.330' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId]) VALUES (5, N'Case Registered', N'Case Registered', N'System Generated', CAST(N'2022-09-06T12:27:43.330' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId]) VALUES (6, N'Open a File', N'فتح ملف', N'System Generated', CAST(N'2022-09-06T12:27:43.330' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId]) VALUES (7, N'Create Execution Request', N'Create Execution Request', N'System Generated', CAST(N'2022-09-06T12:27:43.330' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId]) VALUES (8, N'Send Execution Request To MOJ Execution', N'Send Execution Request To MOJ Execution', N'System Generated', CAST(N'2022-09-06T00:00:00.000' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId]) VALUES (9, N'Approve Execution Request', N'Approve Execution Request', N'System Generated', CAST(N'2022-09-02T00:00:00.000' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId]) VALUES (10, N'Reject Execution Request', N'Reject Execution Request', N'System Generated', CAST(N'2024-03-19T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId]) VALUES (12, N'Send A Copy Review', N'Send A Copy Review', N'System Generated', CAST(N'2024-03-19T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId]) VALUES (13, N'Send A Copy Approved', N'Send A Copy Approved', N'System Generated', CAST(N'2024-03-19T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId]) VALUES (14, N'Transfer Of Sector', N'Transfer Of Sector', N'System Generated', CAST(N'2024-03-19T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId]) VALUES (15, N'Assign Back To Hos', N'Assign Back To Hos', N'System Generated', CAST(N'2024-03-19T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId]) VALUES (16, N'Request For Meeting', N'Request For Meeting', N'System Generated', CAST(N'2024-03-20T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId]) VALUES (17, N'Create Merge Request', N'Create Merge Request', N'System Generated', CAST(N'2024-03-20T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId]) VALUES (18, N'Approve Merge Request', N'Approve Merge Request', N'System Generated', CAST(N'2024-03-20T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId]) VALUES (19, N'New Consultation Request', N'New Consultation Request', N'System Generated', CAST(N'2024-03-20T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId]) VALUES (20, N'Add Judgement', N'Add Judgement', N'System Generated', CAST(N'2024-03-20T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId]) VALUES (21, N'Attendee Decision For Meeting', N'Attendee Decision For Meeting', N'System Generated', CAST(N'2024-03-21T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId]) VALUES (22, N'Add Judgment Execution', N'Add Judgment Execution', N'System Generated', CAST(N'2024-03-21T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId]) VALUES (23, N'Modify Legislation/Principle Draft Document', N'Modify Legislation/Principle Draft Document', N'System Generated', CAST(N'2024-03-21T16:39:16.277' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId]) VALUES (24, N'Modify Draft', N'Modify Draft', N'System Generated', CAST(N'2024-03-23T14:54:04.243' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId]) VALUES (25, N'Review Draft', N'Review Draft', N'System Generated', CAST(N'2024-03-23T14:54:04.243' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId]) VALUES (26, N'Published Draft', N'Published Draft', N'System Generated', CAST(N'2024-03-23T14:54:04.243' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId]) VALUES (27, N'Modify Document', N'Modify Document', N'System Generated', CAST(N'2024-03-23T14:54:04.247' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId]) VALUES (28, N'Review Document', N'Review Document', N'System Generated', CAST(N'2024-03-23T14:54:04.247' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId]) VALUES (29, N'Published Document', N'Published Document', N'System Generated', CAST(N'2024-03-23T14:54:04.247' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId]) VALUES (30, N'Add Contact', N'Add Contact', N'System Generated', CAST(N'2024-03-23T15:16:07.310' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId]) VALUES (31, N'Send To MOJ', N'Send To MOJ', N'System Generated', CAST(N'2024-03-23T15:29:34.570' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId]) VALUES (32, N'Case Data Pushed From RPA', N'Case Data Pushed From RPA', N'System Generated', CAST(N'2024-03-23T16:54:27.510' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId]) VALUES (33, N'Delete Attendee From Meeeting', N'Delete Attendee From Meeeting', N'System Generated', CAST(N'2024-03-24T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId]) VALUES (34, N' Attendee Accept Meeeting Invite', N'Attendee Accept Meeeting Invite', N'System Generated', CAST(N'2024-03-24T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId]) VALUES (35, N' Attendee Reject Meeeting Invite', N'Attendee Reject Meeeting Invite', N'System Generated', CAST(N'2024-03-24T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId]) VALUES (36, N'Meeeting Decision of HOS For Approval', N'Meeeting Decision of HOS For Approval', N'System Generated', CAST(N'2024-03-24T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId]) VALUES (37, N'Save Legal Legislation', N'Save Legal Legislation', N'System Generated', CAST(N'2024-03-24T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId]) VALUES (38, N'Update Legal Legislation', N'Update Legal Legislation', N'System Generated', CAST(N'2024-03-24T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId]) VALUES (39, N'Soft Delete Legal Legislation', N'Soft Delete Legal Legislation', N'System Generated', CAST(N'2024-03-24T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId]) VALUES (40, N' Add Meeting Success', N'Add Meeting Success', N'System Generated', CAST(N'2024-03-25T10:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId]) VALUES (41, N' Edit Meeting Success', N'Edit Meeting Success', N'System Generated', CAST(N'2024-03-25T10:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId]) VALUES (42, N'Revoke Delete Legal Legislation', N'Revoke Delete Legal Legislation', N'System Generated', CAST(N'2024-03-25T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId]) VALUES (43, N' Add MOM Of Meeting', N'Add MOM Of Meeting', N'System Generated', CAST(N'2024-03-26T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId]) VALUES (44, N' Edit MOM Of Meeting', N'Edit MOM Of Meeting', N'System Generated', CAST(N'2024-03-24T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId]) VALUES (49, N' GE Reject Meeeting Invite', N'GE Reject Meeeting Invite', N'System Generated', CAST(N'2024-03-24T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId]) VALUES (50, N' GE Accept Meeeting Invite', N'GE Accept Meeeting Invite', N'System Generated', CAST(N'2024-03-24T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId]) VALUES (51, N' MOM Created Successfully', N'MOM Created Successfully', N'System Generated', CAST(N'2024-03-24T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId]) VALUES (52, N'Assig/Unassigned Cases to Sector', N'Assig/Unassigned Cases to Sector', N'System Generated', CAST(N'2024-03-26T19:25:23.337' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (1, N'#Entity#', NULL)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (2, N'#Reference Number#', 2)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (5, N'#Sender Name#', NULL)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (6, N'#Receiver Name#', NULL)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (7, N'#Created Date#', NULL)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (8, N'#Document Name#', 3)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (9, N'#Request Number#', 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (10, N'#Reference Number#', 14)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (11, N'#Sector From#', 14)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (21, N'#Sector To#', 14)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (26, N'#File Number#', 15)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (27, N'#Case Number#', 8)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (28, N'#Case Number#', 7)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (29, N'#Case Number#', 9)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (30, N'#Case Number#', 10)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (32, N'#Case Number#', 17)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (33, N'#File Number#', 17)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (34, N'#Request Type#', 19)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (35, N'#Request Number#', 19)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (36, N'#Case Number#', 20)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (37, N'#Case Number#', 22)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (38, N'#Type#', 23)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (39, N'#Status#', 23)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (40, N'#Type#', 31)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (41, N'#Case Number#', 32)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (42, N'#File Number#', 32)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (43, N'#Legislation Number#', 37)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (44, N'#Legislation Number#', 38)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (45, N'#Type#', 25)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (46, N'#Type#', 24)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (47, N'#Type#', 26)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (48, N'#Legislation Number#', 39)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (49, N'#Case Number#', 5)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (50, N'#File Number#', 5)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (51, N'#Legislation Number#', 42)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (52, N'#Type#', 42)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (53, N'#Type#', 37)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (54, N'#Type#', 38)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (55, N'#Type#', 39)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (56, N'#File Number#', 20)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (57, N'#Sector From#', 9)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (58, N'#Sector From#', 8)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (59, N'#Sector To#', 52)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'4a12fc1a-0dd0-4a08-8c0b-07a5ff4cad68', 12, 4, N'Send A Copy Review', N'Send A Copy Review', N'Send A Copy Review', N'Send A Copy Review', N'A Copy of #Entity#, with #Entity# Number #Refrence Number# has been send to #Sector To# from #Sector From# on #Created Date# Date by #Sender Name# for review.', N'A Copy of #Entity#, with #Entity# Number #Refrence Number# has been send to #Sector To# from #Sector From# on #Created Date# Date by #Sender Name# for review.', NULL, N'System Generated', CAST(N'2024-03-21T16:18:12.853' AS DateTime), NULL, NULL, NULL, NULL, 0)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'17ea1229-3f34-4333-81b6-0c29d21c4afb', 25, 4, N'Review Draft', N'Review Draft', N'Review Draft', N'Review Draft', N'Following Darft of type #Type# has been send to #Receiver Name# to review send by #Sender Name#', N'Following Darft of type #Type# has been send to #Receiver Name# to review send by #Sender Name#', NULL, N'System Generated', CAST(N'2024-03-24T16:44:12.697' AS DateTime), NULL, NULL, NULL, NULL, 0)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'414adc17-e20e-4889-9d11-0ee4ba52ed5e', 36, 4, N' Meeting Decision of HOS For Approval', N' Meeting Decision of HOS For Approval', N' Meeting Decision of HOS For Approval', N' Meeting Decision of HOS For Approval', N'#Entity# is pending for your approval Sent by #Sender Name# on #Created Date#', N'#Entity# is pending for your approval Sent by #Sender Name# on #Created Date#', NULL, N'fatwaadmin@gmail.com', CAST(N'2024-03-24T12:45:52.087' AS DateTime), NULL, NULL, NULL, NULL, 0)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'6d434354-9a15-4029-9418-1680331685db', 34, 4, N'Attendee Accept Meeting Invite', N'Attendee Accept Meeting Invite', N'Attendee Accept Meeting Invite', N'Attendee Accept Meeting Invite', N'Attendee #Sender Name# Has Accept Your 
#Entity# Invite, on #Created Date#', N'Attendee #Sender Name# Has Accept Your 
#Entity# Invite, on #Created Date#', NULL, N'fatwaadmin@gmail.com', CAST(N'2024-03-24T11:34:05.560' AS DateTime), N'fatwaadmin@gmail.com', CAST(N'2024-03-24T15:33:48.027' AS DateTime), NULL, NULL, 0)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'82ecc9dd-a845-459c-8a6c-205b16248259', 37, 4, N'Save Legal Legislation', N'Save Legal Legislation', N'Save Legal Legislationt Successfully', N'Save Legal Legislation Successfully', N'Following #Entity# added with Legislation type #Type# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', N'Following #Entity# added with Legislation type #Type# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', NULL, N'System Generated', CAST(N'2024-03-21T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'9e76ce4e-4cdd-49ad-acc0-2772f2344479', 3, 4, N'Share Document', N'Share Document', N'User Share Document', N'User Share Document', N'Following #Entity#, #Document Name# share with #Receiver Name# user by #Sender Name#.', NULL, NULL, N'System Generated', CAST(N'2024-03-13T15:20:19.030' AS DateTime), NULL, NULL, NULL, NULL, 0)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'dddb9793-dc89-4f24-ab77-311dd288b0b4', 2, 4, N'Assign to Lawyer', N'Assign to Lawyer', N'File/Case Assign to Lawyer', N'File/Case Assign to Lawyer', N'The #Entity#  #Reference Number# has been assigned to #Receiver Name# by #Sender Name# on the date #Created Date#', N'تم تعيين #Entity# رقم #Reference Number# الى #Receiver Name# من قبل #Sender Name# بتاريخ #Created Date#', NULL, N'System Generated', CAST(N'2024-03-12T19:06:43.297' AS DateTime), N'fatwaadmin@gmail.com', CAST(N'2024-03-26T16:59:11.157' AS DateTime), NULL, NULL, 0)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'b53bc393-1908-4a35-a105-40c2e8514610', 15, 4, N'Assign Back To Hos', N'Assign Back To Hos', N'Assign Back To Hos Successfully', N'Assign Back To Hos Successfully', N'Following #Entity#, with #Entity# number #File Number# has been assigned back to #Receiver Name# on #Created Date# Date by lawyer #Sender Name#.', N'Following #Entity#, with #Entity# number #File Number# has been assigned back to #Receiver Name# on #Created Date# Date by lawyer #Sender Name#.', NULL, N'System Generated', CAST(N'2024-03-14T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'f7afb7aa-120d-4b3b-80da-439b18eb86a5', 1, 4, N'Request Created', N'Request Created', N'Request Created Successfully updated', N'Request Created Successfully', N'Following #Entity#, with request number #Request Number# has been assigned to #Receiver Name# on #Created Date# Date .', NULL, NULL, N'System Generated', CAST(N'2024-03-14T11:41:43.297' AS DateTime), N'fatwaadmin@gmail.com', CAST(N'2024-03-19T12:03:48.670' AS DateTime), NULL, NULL, 0)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'68322bf2-4873-4b11-a5ea-4b095af547bd', 26, 4, N'Published Draft', N'Published Draft', N'Published Draft', N'Modify Draft', N'Following Darft of type #Type# has been Published by #Sender Name#', N'Following Darft of type #Type# has been Published by #Sender Name#', NULL, N'System Generated', CAST(N'2024-03-24T16:49:33.913' AS DateTime), NULL, NULL, NULL, NULL, 0)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'62a254e8-2124-41eb-9aaa-54254f823519', 20, 4, N'Add Judgement', N'Add Judgement', N'Judgement Added Successfully', N'Judgement Added Successfully', N'Judgement is add on case number #Case Number# against file number #File Number# by #Sender Name# on #Created Date# Date.', N'Judgement is add on case number #Case Number# against file number #File Number# by #Sender Name# on #Created Date# Date.', NULL, N'System Generated', CAST(N'2024-03-21T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'e6c58878-61b4-48b6-ac63-5514dfd1d540', 33, 4, N'Delete Attendee From Meeting', N' Delete Attendee From Meeting', N' Delete Attendee From Meeting', N' Delete Attendee From Meeting', N'You have been deleted from #Entity# by #Sender Name# on #Created Date#', N'You have been deleted from #Entity# by #Sender Name# on #Created Date#', NULL, N'fatwaadmin@gmail.com', CAST(N'2024-03-24T15:33:18.747' AS DateTime), NULL, NULL, NULL, NULL, 0)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'dc3d8171-1308-463e-9702-566edaa17283', 17, 4, N'Create Merge Request', N'Create Merge Request', N'Create Merge Request Successfully', N'Create Merge Request Successfully', N'Following #Entity#, with Case number #Case Number#  created from #File Number# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', NULL, NULL, N'System Generated', CAST(N'2024-03-20T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'8b53ad90-c091-4621-b2e8-672ed1ea45a5', 21, 4, N'Attendee Decision For Meeting', N'Attendee Decision For Meeting', N'Attendee Decision For Meeting', N'Attendee Decision For Meeting', N'Following #Entity#, is pending for your descision created on #Created Date# sent by#Sender Name#.', N'Following #Entity#, is pending for your descision created on #Created Date# sent by#Sender Name#.', NULL, N'System Generated', CAST(N'2024-03-21T11:41:43.297' AS DateTime), N'fatwaadmin@gmail.com', CAST(N'2024-03-24T11:14:23.503' AS DateTime), NULL, NULL, 0)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'50105901-79ae-4b77-9ce1-6bede9cf4fe3', 13, 4, N'Send A Copy Approved', N'Send A Copy Approved', N'Send A Copy Approved', N'Send A Copy Approved', N'A Copy of #Entity#, with #Entity# Number #Refrence Number# has been approved received from #Sector From# sector on #Created Date# Date send by #Sender Name#.', N'A Copy of #Entity#, with #Entity# Number #Refrence Number# has been approved received from #Sector From# sector on #Created Date# Date send by #Sender Name#.', NULL, N'System Generated', CAST(N'2024-03-21T16:29:50.487' AS DateTime), NULL, NULL, NULL, NULL, 0)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'c121e56d-857b-4b55-a8d5-7054ecb87791', 39, 4, N'Soft Delete Legal Legislation', N'Soft Delete Legal Legislation', N'Soft Delete Legal Legislation Successfully', N'Soft Delete Legal Legislation Successfully', N'Following #Entity# soft deleted with Legislation type #Type# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', N'Following #Entity# soft deleted with Legislation type #Type# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', NULL, N'System Generated', CAST(N'2024-03-21T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'c0327b70-9c1e-41b6-a5bb-70d8a0e30912', 8, 4, N'Request Sent For Execution', N'Request Sent For Execution', N'Request Sent For Execution', N'Request Sent For Execution', N'You received execution request from #Sector From# on case number #Case Number# by #Sender Name# on #Created Date# Date to review.', N'You received execution request from #Sector From# on case number #Case Number# by #Sender Name# on #Created Date# Date to review.', NULL, N'System Generated', CAST(N'2024-03-19T17:25:44.147' AS DateTime), NULL, NULL, NULL, NULL, 0)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'469bd75d-f10b-4457-a8db-7ac3d0692230', 32, 4, N'Case Data Pushed From RPA', N'Case Data Pushed From RPA', N'Case Data Pushed From RPA', N'Case Data Pushed From RPA', N'A new case registered, number #Case Number# and has been sent to #Receiver Name# on the date #Created Date#.', N'تم تسجيل قضية جديدة رقم #Case Number# وارسلت الى #Receiver Name# بتاريخ #Created Date#', NULL, N'fatwaadmin@gmail.com', CAST(N'2024-03-26T17:09:33.300' AS DateTime), NULL, NULL, NULL, NULL, 0)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'f3f2f8b3-f74a-4439-ba38-7ca105da0919', 9, 4, N'Approve Execution Request', N'Approve Execution Request', N'Approve Execution Request', N'Approve Execution Request', N'You received execution request from #Sector From# on case number #Case Number# by #Sender Name# on #Created Date# Date to review.', N'You received execution request from #Sector From# on case number #Case Number# by #Sender Name# on #Created Date# Date to review.', NULL, N'System Generated', CAST(N'2024-03-19T23:30:47.300' AS DateTime), NULL, NULL, NULL, NULL, 0)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'92cd1f23-5e2e-4587-9f98-7cd92fa7d364', 5, 4, N'Case Registered', N'Case Registered', N'Case Registered', N'Case Registered', N'Following Case, with Case Number #Case Number# has been registered against File Number #File Number# by #Sender Name# on #Created Date# Date for #Receiver Name#', N'Following Case, with Case Number #Case Number# has been registered against File Number #File Number# by #Sender Name# on #Created Date# Date for #Receiver Name#', NULL, N'System Generated', CAST(N'2024-03-24T20:01:18.220' AS DateTime), NULL, NULL, NULL, NULL, 0)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'e0f53f84-1ceb-4aea-8155-afa7bb26d2eb', 42, 4, N'Revoke Delete Legal Legislation', N'Revoke Delete Legal Legislation', N'Revoke Delete Legal Legislation Successfully', N'Revoke Delete Legal Legislation Successfully', N'Following #Entity# revoke with Legislation type #Type# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', N'Following #Entity# revoke with Legislation type #Type# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', NULL, N'System Generated', CAST(N'2024-03-25T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'387a7c13-87ea-4604-b6ee-bb6fc4d28655', 7, 4, N'Create Execution Request', N'Create Execution Request', N'Create Execution Request Successfully', N'Create Execution Request Successfully', N'Execution Request is add on case number #Case Number# by #Sender Name# on #Created Date# Date.', N'Execution Request is add on case number #Case Number# by #Sender Name# on #Created Date# Date.', NULL, N'System Generated', CAST(N'2024-03-21T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'bb38d542-2349-4c70-8973-bc5a9252f8d6', 10, 4, N'Reject Execution Request', N'Reject Execution Request', N'Reject Execution Request', N'Reject Execution Request', N'Execution Request for the Case, with case number #Case Number# has been rejected by #Receiver Name# on #Created Date# Date Send by #Sender Name#.', N'Execution Request for the Case, with case number #Case Number# has been rejected by #Receiver Name# on #Created Date# Date Send by #Sender Name#.', NULL, N'System Generated', CAST(N'2024-03-19T23:30:47.303' AS DateTime), NULL, NULL, NULL, NULL, 0)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'07989300-2042-4256-b9ed-bc7741b9db1f', 31, 4, N'Send To MOJ', N'Send To MOJ', N'Send To MOJ', N'Send To MOJ', N'Document of Type #Type# has been sent to MOJ for case registration, By sender #Sender Name#.', N'Document of Type #Type# has been sent to MOJ for case registration, By sender #Sender Name#.', NULL, N'System Generated', CAST(N'2024-03-23T16:02:00.333' AS DateTime), NULL, NULL, NULL, NULL, 0)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'498d49be-92b6-4b04-a941-d2ef0338e8e8', 22, 4, N'Add Judgment Execution', N'Add Judgment Execution', N'Add Judgment Execution Successfully', N'Add Judgment Execution', N'Following #Entity#, with Case number #Case Number# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', N'Following #Entity#, with Case number #Case Number# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', NULL, N'System Generated', CAST(N'2024-03-21T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'e32d8354-ae1f-49e9-b134-d94a2f30796e', 19, 4, N'Consultation Request Created', N'Consultation Request Created', N'Consultation Request Created Successfully', N'Consultation Request Created Successfully', N'Following #Entity#, type #Request Type# with request number #Request Number# has been assigned to #Receiver Name# on #Created Date# Date .', N'Following #Entity#, type #Request Type# with request number #Request Number# has been assigned to #Receiver Name# on #Created Date# Date .', NULL, N'System Generated', CAST(N'2024-03-20T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'e9188734-6cda-4fdf-9fe9-db21dff2d7b7', 30, 4, N'Add Contact', N'Add Contact', N'Add Contact', N'Add Contact', N'New contact is successfully added in the system.', N'تم إضافة جهة اتصال جديدة بنجاح', NULL, N'System Generated', CAST(N'2024-03-23T15:19:47.467' AS DateTime), NULL, NULL, NULL, NULL, 0)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'f474efb0-2b32-4d71-80f1-dc9bf8503014', 23, 4, N'Modify Legislation Draft Document', N'Modify Legislation Draft Document', N'Modify Legislation Draft Document', N'Modify Legislation Draft Document', N'#Entity# of type #Type# has been #Status# ,send by #Sender Name#.', N'#Entity# of type #Type# has been #Status# ,send by #Sender Name#.', NULL, N'System Generated', CAST(N'2024-03-21T16:46:52.167' AS DateTime), N'fatwaadmin@gmail.com', CAST(N'2024-03-26T15:40:50.557' AS DateTime), NULL, NULL, 0)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'0f31770a-a581-4205-a7de-e298f8ea0e88', 35, 4, N' Attendee Reject Meeting Invite', N' Attendee Reject Meeting Invite', N' Attendee Reject Meeting Invite', N' Attendee Reject Meeting Invite', N'Attendee #Sender Name# Has Reject Your 
#Entity# Invite, on #Created Date#', N'Attendee #Sender Name# Has Reject Your 
#Entity# Invite, on #Created Date#', NULL, N'fatwaadmin@gmail.com', CAST(N'2024-03-24T15:34:37.873' AS DateTime), NULL, NULL, NULL, NULL, 0)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'00abe322-836c-4922-947a-eeca54631c21', 24, 4, N'Modify Draft', N'Modify Draft', N'Modify Draft', N'Modify Draft', N'Following Darft of type #Type# has been send to #Receiver Name# to Modify send by #Sender Name#', N'Following Darft of type #Type# has been send to #Receiver Name# to Modify send by #Sender Name#', NULL, N'System Generated', CAST(N'2024-03-24T16:46:13.890' AS DateTime), NULL, NULL, NULL, NULL, 0)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'1a773ba2-97c5-41e3-adce-eecb423af676', 14, 4, N'Transfer Of Sector', N'Transfer Of Sector', N'Transfer Of Sector Successfully', N'Transfer Of Sector Successfully', N'Following #Entity#, with #Entity# Number #Reference Number# has been transfer from #Sector From# to #Sector To# on #Created Date# Date by #Sender Name#.', N'Following #Entity#, with #Entity# Number #Reference Number# has been transfer from #Sector From# to #Sector To# on #Created Date# Date by #Sender Name#.', NULL, N'System Generated', CAST(N'2024-03-14T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'be414ea0-343f-4054-8907-f317d4d78356', 40, 4, N'Add Meeting Success', N'Add Meeting Success', N'Add Meeting Success', N'Add Meeting Success', N'#Entity# has been created successfully, on #Created Date#', N'#Entity# has been created successfully, on #Created Date#', NULL, N'fatwaadmin@gmail.com', CAST(N'2024-03-25T10:26:38.033' AS DateTime), NULL, NULL, NULL, NULL, 0)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'7093b956-e88f-4358-a8ed-f716fba13c09', 52, 4, N'Assig/Unassigned Cases to Sector', N'Assig/Unassigned Cases to Sector', N'Assig/Unassigned Cases to Sector', N'Assig/Unassigned Cases to Sector', N'The Case has been assigned to #Sector To# by #Sender Name# on the date #Created Date#', N'تم تعيين ملفات القضايا لقطاع #Sector To# من قبل #Sender Name# بتاريخ #Created Date#', NULL, N'fatwaadmin@gmail.com', CAST(N'2024-03-26T19:30:20.720' AS DateTime), N'fatwaadmin@gmail.com', CAST(N'2024-03-26T19:30:50.547' AS DateTime), NULL, NULL, 0)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'a28b8463-73e4-496d-9a51-fc31f319b220', 38, 4, N'Update Legal Legislation', N'Update Legal Legislation', N'Update Legal Legislation Successfully', N'Update Legal Legislation Successfully', N'Following #Entity# updated with Legislation type #Type# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', N'Following #Entity# updated with Legislation type #Type# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', NULL, N'System Generated', CAST(N'2024-03-21T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0)
GO
------ 03-25-2024 end --------

 INSERT INTO NOTIF_NOTIFICATION_TEMPLATE (TemplateId, EventId, ChannelId, NameEn, NameAr ,SubjectEn, SubjectAr, BodyEn, CreatedBy , CreatedDate , IsDeleted )
VALUES (NewID(),14, 4, 'Transfer Of Sector' , 'Transfer Of Sector', 'Transfer Of Sector Successfully','Transfer Of Sector Successfully', 'Following #Entity#, with request number #Request Number# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', 'System Generated' ,'2024-03-14 11:41:43.297',0);

----27-03-2024---------------------------------------- Notification---------
----------------------------------------------------------------------------
----------------------- insert NOTIF_NOTIFICATION_EVENT---------------------
----------------------------------------------------------------------------


INSERT INTO NOTIF_NOTIFICATION_EVENT ( EventId, NameEn, NameAr , CreatedBy , CreatedDate, ReceiverTypeId , IsDeleted)
VALUES (16,  'Request For Meeting' , 'Request For Meeting',  'System Generated' ,'2024-03-20 11:41:43.297', 1 , 0);

INSERT INTO NOTIF_NOTIFICATION_EVENT ( EventId, NameEn, NameAr , CreatedBy , CreatedDate, ReceiverTypeId , IsDeleted)
VALUES (17,  'Create Merge Request' , 'Create Merge Request',  'System Generated' ,'2024-03-20 11:41:43.297', 1 , 0);

INSERT INTO NOTIF_NOTIFICATION_EVENT ( EventId, NameEn, NameAr , CreatedBy , CreatedDate, ReceiverTypeId , IsDeleted)
VALUES (18,  'Approve Merge Request' , 'Approve Merge Request',  'System Generated' ,'2024-03-20 11:41:43.297', 1 , 0);


INSERT INTO NOTIF_NOTIFICATION_EVENT ( EventId, NameEn, NameAr , CreatedBy , CreatedDate, ReceiverTypeId , IsDeleted)
VALUES (19,  'New Consultation Request' , 'New Consultation Request',  'System Generated' ,'2024-03-20 11:41:43.297', 1 , 0);

INSERT INTO NOTIF_NOTIFICATION_EVENT ( EventId, NameEn, NameAr , CreatedBy , CreatedDate, ReceiverTypeId , IsDeleted)
VALUES (20,  'Add Judgement' , 'Add Judgement',  'System Generated' ,'2024-03-20 11:41:43.297', 1 , 0);

INSERT INTO NOTIF_NOTIFICATION_EVENT ( EventId, NameEn, NameAr , CreatedBy , CreatedDate, ReceiverTypeId , IsDeleted)
VALUES (22,  'Add Judgment Execution' , 'Add Judgment Execution',  'System Generated' ,'2024-03-21 11:41:43.297', 1 , 0);

INSERT INTO NOTIF_NOTIFICATION_EVENT ( EventId, NameEn, NameAr , CreatedBy , CreatedDate, ReceiverTypeId , IsDeleted)
VALUES (37,  'Save Legal Legislation' , 'Save Legal Legislation',  'System Generated' ,'2024-03-24 11:41:43.297', 1 , 0);

INSERT INTO NOTIF_NOTIFICATION_EVENT ( EventId, NameEn, NameAr , CreatedBy , CreatedDate, ReceiverTypeId , IsDeleted)
VALUES (38,  'Update Legal Legislation' , 'Update Legal Legislation',  'System Generated' ,'2024-03-24 11:41:43.297', 1 , 0);

INSERT INTO NOTIF_NOTIFICATION_EVENT ( EventId, NameEn, NameAr , CreatedBy , CreatedDate, ReceiverTypeId , IsDeleted)
VALUES (39,  'Soft Delete Legal Legislation' , 'Soft Delete Legal Legislation',  'System Generated' ,'2024-03-24 11:41:43.297', 1 , 0);

INSERT INTO NOTIF_NOTIFICATION_EVENT ( EventId, NameEn, NameAr , CreatedBy , CreatedDate, ReceiverTypeId , IsDeleted)
VALUES (42,  'Revoke Delete Legal Legislation' , 'Revoke Delete Legal Legislation',  'System Generated' , '2024-03-25 11:41:43.297', 1 , 0);

INSERT INTO NOTIF_NOTIFICATION_EVENT ( EventId, NameEn, NameAr , CreatedBy , CreatedDate, ReceiverTypeId , IsDeleted)
VALUES (45,  'Save Legal Principle' , 'Save Legal Principle',  'System Generated' , '2024-03-26 11:41:43.297', 1 , 0);

INSERT INTO NOTIF_NOTIFICATION_EVENT ( EventId, NameEn, NameAr , CreatedBy , CreatedDate, ReceiverTypeId , IsDeleted)
VALUES (46,  'Update Legal Principle' , 'Update Legal Principle',  'System Generated' , '2024-03-26 11:41:43.297', 1 , 0);

INSERT INTO NOTIF_NOTIFICATION_EVENT ( EventId, NameEn, NameAr , CreatedBy , CreatedDate, ReceiverTypeId , IsDeleted)
VALUES (47,  'Soft Delete Legal Principle' , 'Soft Delete Legal Principle',  'System Generated' , '2024-03-26 11:41:43.297', 1 , 0);

INSERT INTO NOTIF_NOTIFICATION_EVENT ( EventId, NameEn, NameAr , CreatedBy , CreatedDate, ReceiverTypeId , IsDeleted)
VALUES (48,  'Revoke Delete Legal Principle' , 'Revoke Delete Legal Principle',  'System Generated' , '2024-03-26 11:41:43.297', 1 , 0);

----------------------------------------------------------------------------
-----------------------Insert NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP-----
----------------------------------------------------------------------------

Insert into NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP values (18,'#Entity#',14)
Insert into NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP values (19,'#Request Number#',14)
Insert into NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP values (20,'#File Number#',14)
Insert into NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP values (21,'#Receiver Name#',14)
Insert into NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP values (22,'#Sender Name#',14)
Insert into NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP values (23,'#Created Date#',14)
Insert into NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP values (24,'#Entity#',15)
Insert into NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP values (25,'#Request Number#',15)
Insert into NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP values (26,'#File Number#',15)
Insert into NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP values (27,'#Receiver Name#',15)
Insert into NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP values (28,'#Sender Name#',15)
Insert into NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP values (29,'#Created Date#',15)
Insert into NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP values (32,'#Case Number#',17)
Insert into NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP values (33,'#File Number#',17)
Insert into NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP values (34,'#Consultation Request Type#', 19)
Insert into NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP values (35,'#Request Number#', 19)
Insert into NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP values (37,'#Case Number#', 22)
 Insert into NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP values (43,'#Legislation Number#', 37)
 Insert into NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP values (44,'#Legislation Number#', 38)
 Insert into NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP values (48,'#Legislation Number#', 39)
 Insert into NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP values (51,'#Legislation Number#', 42)
 Insert into NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP values (52,'#Type#', 42)
 Insert into NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP values (53,'#Type#', 37)
 Insert into NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP values (54,'#Type#', 38)
 Insert into NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP values (55,'#Type#', 39)
 Insert into NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP values (59,'#Type#', 45) 
 Insert into NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP values (60,'#Type#', 46) 
 Insert into NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP values (61,'#Type#', 47) 
 Insert into NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP values (62,'#Type#', 48) 
 -----------------

----------------------------------------------------------------------------------------------
-----------------------UPDATE NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP-------------------------------------


UPDATE NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP
SET PlaceHolderName='#Request Type#'
WHERE PlaceHolderId = 34;


----------------------------------------------------------------------------
----------------------- INSERT NOTIF_NOTIFICATION_TEMPLATE-------------------
----------------------------------------------------------------------------

 INSERT INTO NOTIF_NOTIFICATION_TEMPLATE (TemplateId, EventId, ChannelId, NameEn, NameAr ,SubjectEn, SubjectAr, BodyEn, CreatedBy , CreatedDate , IsDeleted )
VALUES (NewID(),17, 4, 'Create Merge Request' , 'Create Merge Request', 'Create Merge Request Successfully','Create Merge Request Successfully',
'Following #Entity#, with Case number #Case Number#  created from #File Number# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', 
'System Generated' ,'2024-03-20 11:41:43.297',0);

INSERT INTO NOTIF_NOTIFICATION_TEMPLATE (TemplateId, EventId, ChannelId, NameEn, NameAr ,SubjectEn, SubjectAr, BodyEn, BodyAr,CreatedBy , CreatedDate , IsDeleted )
VALUES (NewID(),19, 4, 'Consultation Request Created' , 'Consultation Request Created', 'Consultation Request Created Successfully','Consultation Request Created Successfully',
'Following #Entity#, type #Request Type# with request number #Request Number# has been assigned to #Receiver Name# on #Created Date# Date .', 
'Following #Entity#, type #Request Type# with request number #Request Number# has been assigned to #Receiver Name# on #Created Date# Date .', 'System Generated' ,'2024-03-20 11:41:43.297',0);

INSERT INTO NOTIF_NOTIFICATION_TEMPLATE (TemplateId, EventId, ChannelId, NameEn, NameAr ,SubjectEn, SubjectAr, BodyEn, BodyAr,CreatedBy , CreatedDate , IsDeleted )
VALUES (NewID(),20, 4, 'Add Judgement' , 'Add Judgement', 'Judgement Added Successfully',
'Judgement Added Successfully',
'Following #Entity#, with Case number #Case Number# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', 
'Following #Entity#, with Case number #Case Number# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', 
'System Generated' ,'2024-03-21 11:41:43.297',0);

INSERT INTO NOTIF_NOTIFICATION_TEMPLATE (TemplateId, EventId, ChannelId, NameEn, NameAr ,SubjectEn, SubjectAr, BodyEn, BodyAr,CreatedBy , CreatedDate , IsDeleted )
VALUES (NewID(),22, 4, 'Add Judgment Execution' , 'Add Judgment Execution', 'Add Judgment Execution Successfully',
'Add Judgment Execution',
'Following #Entity#, with Case number #Case Number# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', 
'Following #Entity#, with Case number #Case Number# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', 
'System Generated' ,'2024-03-21 11:41:43.297',0);

 INSERT INTO NOTIF_NOTIFICATION_TEMPLATE (TemplateId, EventId, ChannelId, NameEn, NameAr ,SubjectEn, SubjectAr, BodyEn, BodyAr,CreatedBy , CreatedDate , IsDeleted )
VALUES (NewID(),7, 4, 'Create Execution Request' , 'Create Execution Request', 'Create Execution Request Successfully',
'Create Execution Request Successfully',
'Following #Entity#, with Case number #Case Number# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', 
'Following #Entity#, with Case number #Case Number# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', 
'System Generated' ,'2024-03-21 11:41:43.297',0);

 INSERT INTO NOTIF_NOTIFICATION_TEMPLATE (TemplateId, EventId, ChannelId, NameEn, NameAr ,SubjectEn, SubjectAr, BodyEn, BodyAr,CreatedBy , CreatedDate , IsDeleted )
VALUES (NewID(),37, 4, 'Save Legal Legislation' , 'Save Legal Legislation', 'Save Legal Legislationt Successfully',
'Save Legal Legislation Successfully',
'Following #Entity#, with Case number #Legislation Number# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', 
'Following #Entity#, with Case number #Legislation Number# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', 
'System Generated' ,'2024-03-21 11:41:43.297',0);

 INSERT INTO NOTIF_NOTIFICATION_TEMPLATE (TemplateId, EventId, ChannelId, NameEn, NameAr ,SubjectEn, SubjectAr, BodyEn, BodyAr,CreatedBy , CreatedDate , IsDeleted )
VALUES (NewID(),38, 4, 'Update Legal Legislation' , 'Update Legal Legislation', 'Update Legal Legislation Successfully',
'Update Legal Legislation Successfully',
'Following #Entity#, with Legislation number #Legislation Number# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', 
'Following #Entity#, with Legislation number #Legislation Number# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', 
'System Generated' ,'2024-03-21 11:41:43.297',0);


 INSERT INTO NOTIF_NOTIFICATION_TEMPLATE (TemplateId, EventId, ChannelId, NameEn, NameAr ,SubjectEn, SubjectAr, BodyEn, BodyAr,CreatedBy , CreatedDate , IsDeleted )
VALUES (NewID(),39, 4, 'Soft Delete Legal Legislation' , 'Soft Delete Legal Legislation', 'Soft Delete Legal Legislation Successfully',
'Soft Delete Legal Legislation Successfully',
'Following #Entity#, with Legislation number #Legislation Number# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', 
'Following #Entity#, with Legislation number #Legislation Number# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', 
'System Generated' ,'2024-03-21 11:41:43.297',0);


 INSERT INTO NOTIF_NOTIFICATION_TEMPLATE (TemplateId, EventId, ChannelId, NameEn, NameAr ,SubjectEn, SubjectAr, BodyEn, BodyAr,CreatedBy , CreatedDate , IsDeleted )
VALUES (NewID(),42, 4, 'Revoke Delete Legal Legislation' , 'Revoke Delete Legal Legislation', 'Revoke Delete Legal Legislation Successfully',
'Revoke Delete Legal Legislation Successfully',
'Following #Entity# revoke with Legislation number #Legislation Number# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', 
'Following #Entity# revoke with Legislation number #Legislation Number# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', 
'System Generated' ,'2024-03-25 11:41:43.297',0);

 INSERT INTO NOTIF_NOTIFICATION_TEMPLATE (TemplateId, EventId, ChannelId, NameEn, NameAr ,SubjectEn, SubjectAr, BodyEn, BodyAr,CreatedBy , CreatedDate , IsDeleted )
VALUES (NewID(),45, 4, 'Save Legal Principle' , 'Save Legal Principle', 'Save Legal Principle Successfully', 'Save Legal Principle Successfully',
'Following #Entity# added with principle type #Type# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', 
'Following #Entity# added with principle type #Type# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', 
'System Generated' ,'2024-03-26 11:41:43.297',0);

 INSERT INTO NOTIF_NOTIFICATION_TEMPLATE (TemplateId, EventId, ChannelId, NameEn, NameAr ,SubjectEn, SubjectAr, BodyEn, BodyAr,CreatedBy , CreatedDate , IsDeleted )
VALUES (NewID(),46, 4, 'Update Legal Principle' , 'Update Legal Principle', 'Update Legal Principle Successfully','Update Legal Principle Successfully',
'Following #Entity# updated with principle type #Type# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', 
'Following #Entity# updated with principle type #Type# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', 
'System Generated' ,'2024-03-26 11:41:43.297',0);

 INSERT INTO NOTIF_NOTIFICATION_TEMPLATE (TemplateId, EventId, ChannelId, NameEn, NameAr ,SubjectEn, SubjectAr, BodyEn, BodyAr,CreatedBy , CreatedDate , IsDeleted )
VALUES (NewID(),47, 4, 'Soft Delete Legal Principle' , 'Soft Delete Legal Principle', 'Soft Delete Legal Principle Successfully',
'Soft Delete Legal Principle Successfully',
'Following #Entity# soft deleted with principle type #Type# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', 
'Following #Entity# soft deleted with principle type #Type# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', 
'System Generated' ,'2024-03-26 11:41:43.297',0);

 INSERT INTO NOTIF_NOTIFICATION_TEMPLATE (TemplateId, EventId, ChannelId, NameEn, NameAr ,SubjectEn, SubjectAr, BodyEn, BodyAr,CreatedBy , CreatedDate , IsDeleted )
VALUES (NewID(),48, 4, 'Revoke Delete Legal Principle' , 'Revoke Delete Legal Principle', 'Revoke Delete Legal Principle Successfully','Revoke Delete Legal Principle Successfully',
'Following #Entity# revoke with principle type #Type# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', 
'Following #Entity# revoke with principle type #Type# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', 
'System Generated' ,'2024-03-26 11:41:43.297',0);

----------------------------------------------------------------------------------------------
-----------------------UPDATE NOTIF_NOTIFICATION_TEMPLATE-------------------------------------

UPDATE NOTIF_NOTIFICATION_TEMPLATE
SET BodyEn= 'Following #Entity# updated with Legislation number #Legislation Number# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.'
, BodyAr= 'Following #Entity# updated with Legislation number #Legislation Number# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.'
WHERE TemplateId='A28B8463-73E4-496D-9A51-FC31F319B220';


UPDATE NOTIF_NOTIFICATION_TEMPLATE
SET BodyEn= 'Following #Entity# saved with Legislation number #Legislation Number# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.'
, BodyAr= 'Following #Entity# saved with Legislation number #Legislation Number# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.'
WHERE TemplateId='82ECC9DD-A845-459C-8A6C-205B16248259';

UPDATE NOTIF_NOTIFICATION_TEMPLATE
SET BodyEn= 'Following #Entity# Deleted with Legislation number #Legislation Number# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.'
, BodyAr= 'Following #Entity# Deleted with Legislation number #Legislation Number# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.'
WHERE TemplateId='C121E56D-857B-4B55-A8D5-7054ECB87791';


 UPDATE NOTIF_NOTIFICATION_TEMPLATE
SET BodyEn= 'Following #Entity# soft deleted with Legislation type #Type# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.'
, BodyAr= 'Following #Entity# soft deleted with Legislation type #Type# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.'
WHERE TemplateId='C121E56D-857B-4B55-A8D5-7054ECB87791';


UPDATE NOTIF_NOTIFICATION_TEMPLATE
SET BodyEn= 'Following #Entity# revoke with Legislation type #Type# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.'
, BodyAr= 'Following #Entity# revoke with Legislation type #Type# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.'
WHERE TemplateId='E0F53F84-1CEB-4AEA-8155-AFA7BB26D2EB';

UPDATE NOTIF_NOTIFICATION_TEMPLATE
SET BodyEn= 'Following #Entity# updated with Legislation type #Type# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.'
, BodyAr= 'Following #Entity# updated with Legislation type #Type# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.'
WHERE TemplateId='A28B8463-73E4-496D-9A51-FC31F319B220';

UPDATE NOTIF_NOTIFICATION_TEMPLATE
SET BodyEn= 'Following #Entity# added with Legislation type #Type# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.'
, BodyAr= 'Following #Entity# added with Legislation type #Type# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.'
WHERE TemplateId='82ECC9DD-A845-459C-8A6C-205B16248259';
----------------------------------------------------------------------------
--------------------------End Notification----------------------------------
----------------------------------------------------------------------------

--Ammaar Naveed--26/03/2024--Claim for upcoming hearings submenu permission.
INSERT INTO UMS_CLAIM (Title_En, Title_Ar, Module, SubModule, ClaimType, ClaimValue, ModuleId, IsDeleted)
VALUES('Upcoming Hearings List',N'Upcoming Hearings List','Case_Management_System','Registered_Case','Permission','Permissions.Submenu.CMS.UpcomingHearingsList',2,0)

-----------------------------------------------------------start-----------------------------------------------------------
----------------------------------------------------------------------------
----------------------- insert NOTIF_NOTIFICATION_EVENT---------------------
----------------------------------------------------------------------------
 INSERT INTO NOTIF_NOTIFICATION_EVENT ( EventId, NameEn, NameAr , CreatedBy , CreatedDate, ReceiverTypeId , IsDeleted)
VALUES (48,  'Revoke Delete Legal Principle' , 'Revoke Delete Legal Principle',  'System Generated' , '2024-03-26 11:41:43.297', 1 , 0);

INSERT INTO NOTIF_NOTIFICATION_EVENT ( EventId, NameEn, NameAr , CreatedBy , CreatedDate, ReceiverTypeId , IsDeleted)
VALUES (45,  'Save Legal Principle' , 'Save Legal Principle',  'System Generated' , '2024-03-26 11:41:43.297', 1 , 0);

INSERT INTO NOTIF_NOTIFICATION_EVENT ( EventId, NameEn, NameAr , CreatedBy , CreatedDate, ReceiverTypeId , IsDeleted)
VALUES (46,  'Update Legal Principle' , 'Update Legal Principle',  'System Generated' , '2024-03-26 11:41:43.297', 1 , 0);

----------------------------------------------------------------------------
-----------------------Insert NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP-----
----------------------------------------------------------------------------

 Insert into NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP values (76,'#Type#', 45)   

 
----------------------------------------------------------------------------
----------------------- INSERT NOTIF_NOTIFICATION_TEMPLATE-------------------
----------------------------------------------------------------------------

  INSERT INTO NOTIF_NOTIFICATION_TEMPLATE (TemplateId, EventId, ChannelId, NameEn, NameAr ,SubjectEn, SubjectAr, BodyEn, BodyAr,CreatedBy , CreatedDate , IsDeleted )
VALUES (NewID(),45, 4, 'Save Legal Principle' , 'Save Legal Principle', 'Save Legal Principle Successfully', 'Save Legal Principle Successfully',
'Following #Entity# added with principle type #Type# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', 
'Following #Entity# added with principle type #Type# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', 
'System Generated' ,'2024-03-26 11:41:43.297',0);

 INSERT INTO NOTIF_NOTIFICATION_TEMPLATE (TemplateId, EventId, ChannelId, NameEn, NameAr ,SubjectEn, SubjectAr, BodyEn, BodyAr,CreatedBy , CreatedDate , IsDeleted )
VALUES (NewID(),46, 4, 'Update Legal Principle' , 'Update Legal Principle', 'Update Legal Principle Successfully','Update Legal Principle Successfully',
'Following #Entity# updated with principle type #Type# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', 
'Following #Entity# updated with principle type #Type# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', 
'System Generated' ,'2024-03-26 11:41:43.297',0);

 INSERT INTO NOTIF_NOTIFICATION_TEMPLATE (TemplateId, EventId, ChannelId, NameEn, NameAr ,SubjectEn, SubjectAr, BodyEn, BodyAr,CreatedBy , CreatedDate , IsDeleted )
VALUES (NewID(),47, 4, 'Soft Delete Legal Principle' , 'Soft Delete Legal Principle', 'Soft Delete Legal Principle Successfully',
'Soft Delete Legal Principle Successfully',
'Following #Entity# soft deleted with principle type #Type# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', 
'Following #Entity# soft deleted with principle type #Type# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', 
'System Generated' ,'2024-03-26 11:41:43.297',0);


-----------------------------------------------------------end-----------------------------------------------------------
/*<History Author='Ihsaan Abbas' Date='31-03-2024'> Notification Template insert of Worker Services</History>*/ 
----Reminder to complete claim statement 
insert into NOTIF_NOTIFICATION_EVENT values (53,'Reminder To Complete Claim Statement','Reminder To Complete Claim Statement','System Generated',GETDATE(),null,null,null,null,0,1,null);
insert into NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP values (77,'#File Number#',53)
insert into NOTIF_NOTIFICATION_TEMPLATE values (NEWID(),53,4,'Reminder To Complete Claim Statement','Reminder To Complete Claim Statement','Reminder To Complete Claim Statement','Reminder To Complete Claim Statement',
'No claim statement document is created against Case File #File Number# by System.',
'No claim statement document is created against Case File #File Number# by System..',
null,
'System Generated',GETDATE(),null,null,null,null,0) 

-----Assign to MOJ  
insert into NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP values (78,'#File Number#',54) 
insert into NOTIF_NOTIFICATION_TEMPLATE values (NEWID(),54,4,'Assign To MOJ Reminder','Assign To MOJ Reminder','Assign To MOJ Reminder','Assign To MOJ Reminder',
'Case File #File Number# by System not yet registered in MOJ.',
'Case File #File Number# by System not yet registered in MOJ.',
null,
'System Generated',GETDATE(),null,null,null,null,0)
---------------------
 --------Defence Letter reminder service  
insert into NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP values (83,'#File Number#',56) 
insert into NOTIF_NOTIFICATION_TEMPLATE values (NEWID(),56,4,'WS NOTIF Defense Letter Prepration Reminder','WS NOTIF Defense Letter Prepration Reminder','WS NOTIF Defense Letter Prepration Reminder','WS NOTIF Defense Letter Prepration Reminder',
'No Defence Document is created against Case #File Number#.',
'No Defence Document is created against Case #File Number#.',
null,
'System Generated',GETDATE(),null,null,null,null,0)
--------------
-------Draft Modification reminder    
insert into NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP values (84,'#File Number#',57) 
insert into NOTIF_NOTIFICATION_TEMPLATE values (NEWID(),57,4,'Draft Modification Not Completed','Draft Modification Not Completed','Draft Modification Not Completed','Draft Modification Not Completed',
'Draft Modification Not Completed against Case #File Number#.',
'Draft Modification Not Completed against Case #File Number#.',
null,
'System Generated',GETDATE(),null,null,null,null,0)
-------------- 
------Request for Additional info reminder service  
 insert into NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP values (85,'#File Number#',61) 
insert into NOTIF_NOTIFICATION_TEMPLATE values (NEWID(),61,4,'WS NOTIF No Additional Information Or Claim Statement Document Created','WS NOTIF No Additional Information Or Claim Statement Document Created','WS NOTIF No Additional Information Or Claim Statement Document Created','WS NOTIF No Additional Information Or Claim Statement Document Created',
'No additional information or claim statement document is created yet Case #File Number#.',
'No additional information or claim statement document is created yet Case #File Number#.',
null,
'System Generated',GETDATE(),null,null,null,null,0) 

--------------------
 ------------Review_Draft_Reminder_ 
  
  insert into NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP values (86,'#File Number#',62) 
insert into NOTIF_NOTIFICATION_TEMPLATE values (NEWID(),62,4,'Review Draft Reminder','Review Draft Reminder','Review Draft Reminder','Review Draft Reminder',
'Review Draft Reminder against Case #File Number#.',
'Review Draft Reminder against Case #File Number#.',
null,
'System Generated',GETDATE(),null,null,null,null,0)
----------------------------
------------ Communication Response reminder  
insert into NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP values (87,'#Entity#',55) 
insert into NOTIF_NOTIFICATION_TEMPLATE values (NEWID(),55,4,'Communication Response Not Completed','Communication Response Not Completed','Communication Response Not Completed','Communication Response Not Completed',
'Communication Response Not Completed against Case #Entity#.',
'Communication Response Not Completed against Case #Entity#.',
null,
'System Generated',GETDATE(),null,null,null,null,0)

 -------------------------------------END------------Ihsaan ---Abbas ------------------
 -----31-03-2024------
insert into COMM_COMMUNICATION_TYPE values (1073741826,'Stop Execution Of Judgment','Stop Execution Of Judgment','fatwaadmin@gmail.com',GETDATE(),null,null,null,null,0,1)
insert into COMM_COMMUNICATION_TYPE values (1073741827,'Stopping Execution Of Judgment','Stopping Execution Of Judgment','fatwaadmin@gmail.com',GETDATE(),null,null,null,null,0,1)
set IDENTITY_INSERT CMS_CASE_FILE_STATUS_G2G_LKP on
insert into CMS_CASE_FILE_STATUS_G2G_LKP (Id,Name_En,Name_Ar,IsActive,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,DeletedBy,DeletedDate,IsDeleted)
values(524288,'Assigned To Partial/Urgent Sector','Assigned To Partial/Urgent Sector',1,'fatwaadmin@gmail.com',GETDATE(),null,null,null,null,0)
set IDENTITY_INSERT CMS_CASE_FILE_STATUS_G2G_LKP off
--------------- Start NOTIFICATION EVENT PLACE HOLDER ----------------------------------

INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP ( PlaceHolderId, PlaceHolderName, EventId)
VALUES (63,  '#File Number#' , 21);
INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP ( PlaceHolderId, PlaceHolderName, EventId)
VALUES (64,  '#File Number#' , '51'); 

 INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP ( PlaceHolderId, PlaceHolderName, EventId)
VALUES (65,  '#File Number#' , '50');


INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP ( PlaceHolderId, PlaceHolderName, EventId)
VALUES (66,  '#File Number#' , '49');

INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP ( PlaceHolderId, PlaceHolderName, EventId)
VALUES (67,  '#File Number#' , '44');

INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP ( PlaceHolderId, PlaceHolderName, EventId)
VALUES (68,  '#File Number#' , '43');

INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP ( PlaceHolderId, PlaceHolderName, EventId)
VALUES (69,  '#File Number#' , '41');

INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP ( PlaceHolderId, PlaceHolderName, EventId)
VALUES (70,  '#File Number#' , '40');

INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP ( PlaceHolderId, PlaceHolderName, EventId)
VALUES (71,  '#File Number#' , '36');

INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP ( PlaceHolderId, PlaceHolderName, EventId)
VALUES (72,  '#File Number#' , '35');

INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP ( PlaceHolderId, PlaceHolderName, EventId)
VALUES (73,  '#File Number#' , '34');

INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP ( PlaceHolderId, PlaceHolderName, EventId)
VALUES (74,  '#File Number#' , '33');

INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP ( PlaceHolderId, PlaceHolderName, EventId)
VALUES (75,  '#File Number#' , '31');


---------------  Start NOTIFICATION EVENT PLACE HOLDER----------------------------------

-----------------START NOTIFICATTION EVENT --------------------------
INSERT INTO NOTIF_NOTIFICATION_EVENT ( EventId, NameEn, NameAr , CreatedBy , CreatedDate, ReceiverTypeId , IsDeleted)
VALUES (63,  'Delete Parent Index Literature' , 'Delete Parent Index Literature',  'System Generated' ,'2024-03-31 11:41:43.297', 1 , 0);
INSERT INTO NOTIF_NOTIFICATION_EVENT ( EventId, NameEn, NameAr , CreatedBy , CreatedDate, ReceiverTypeId , IsDeleted)
VALUES (64,  'Add Literature' , 'Add Literature',  'System Generated' ,'2024-03-31 11:41:43.297', 1 , 0);

-----------------END NOTIFICATTION EVENT --------------------------
INSERT [dbo].[CMS_DRAFT_ACTION_ENUM]  VALUES (1, N'Created', N'أنشأ')
INSERT [dbo].[CMS_DRAFT_ACTION_ENUM]  VALUES (2, N'Edited', N'معدل')
INSERT [dbo].[CMS_DRAFT_ACTION_ENUM]  VALUES (4, N'Submitted', N'الطلبات المرسلة')

DELETE FROM NOTIF_NOTIFICATION_CHANNEL_LKP WHERE ChannelId = 1
UPDATE NOTIF_NOTIFICATION_CHANNEL_LKP SET NameEn = 'Mobile', NameAr = N'صفحة الويب' WHERE ChannelId = 2
UPDATE NOTIF_NOTIFICATION_CHANNEL_LKP SET NameEn = 'Browser', NameAr = N'الهاتف النقال' WHERE ChannelId = 4

--------------UpComing Hearing Rolls List Claim  ---------
INSERT INTO UMS_CLAIM (Title_En, Title_Ar, Module, SubModule, ClaimType, ClaimValue, ModuleId, IsDeleted)
VALUES('Upcoming Hearing Rolls List',N'Upcoming Hearing Rolls List','Case_Management_System','Registered_Case','Permission','Permissions.Submenu.CMS.UpcomingHearingRollsList',2,0)
--------------Assign Hearing Rolls List Claim  ---------
INSERT INTO UMS_CLAIM (Title_En, Title_Ar, Module, SubModule, ClaimType, ClaimValue, ModuleId, IsDeleted)
VALUES('Assign Hearing Rolls',N'Assign Hearing Rolls','Case_Management_System','Registered_Case','Permission','Permissions.Submenu.CMS.AssignHearingRolls',2,0)


INSERT INTO UMS_CLAIM (Title_En, Title_Ar, Module, SubModule, ClaimType, ClaimValue, ModuleId, IsDeleted)
VALUES('Templates List',N'Templates List','Document_Management_System','Draft_Template','Permission','Permissions.DMS.Template.List',16,0)

INSERT INTO UMS_CLAIM (Title_En, Title_Ar, Module, SubModule, ClaimType, ClaimValue, ModuleId, IsDeleted)
VALUES('Add Template',N'Add Template','Document_Management_System','Draft_Template','Permission','Permissions.DMS.Template.Add',16,0)

INSERT INTO UMS_CLAIM (Title_En, Title_Ar, Module, SubModule, ClaimType, ClaimValue, ModuleId, IsDeleted)
VALUES('DMS Portal Document Management Menu',N'DMS Portal Document Management Menu','Sidemenu','Draft_Template','Menu','Permissions.DMS.Menu.Document',2097152,0)
--------------

------------------------------------------------Inventory Management Table DATA------------------------------------------------------
GO
INSERT [dbo].[INV_SERVICE_REQUEST_TYPE_OSS_LKP] ([Id], [NameEn], [NameAr], [IsActive]) VALUES (1, N'RequestToChangeResidence', N'طلب تغيير الإقامة                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   ', 1)
GO
INSERT [dbo].[INV_SERVICE_REQUEST_TYPE_OSS_LKP] ([Id], [NameEn], [NameAr], [IsActive]) VALUES (2, N'Request For Maintenance And Repair Of Residence', N'طلب صيانة وإصلاح السكن                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              ', 1)
GO
INSERT [dbo].[INV_SERVICE_REQUEST_TYPE_OSS_LKP] ([Id], [NameEn], [NameAr], [IsActive]) VALUES (4, N'Issue Residence Clearance', N'إصدار تصريح الإقامة                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 ', 1)
GO
INSERT [dbo].[INV_SERVICE_REQUEST_TYPE_OSS_LKP] ([Id], [NameEn], [NameAr], [IsActive]) VALUES (8, N'Registration Of Apartments Complaints', N'تسجيل شكاوى الشقق                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   ', 1)
GO
INSERT [dbo].[INV_SERVICE_REQUEST_TYPE_OSS_LKP] ([Id], [NameEn], [NameAr], [IsActive]) VALUES (16, N'Insert Consultant Residential Details', N'أدخل بيانات سكن الاستشاري                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           ', 1)
GO
INSERT [dbo].[INV_SERVICE_REQUEST_TYPE_OSS_LKP] ([Id], [NameEn], [NameAr], [IsActive]) VALUES (32, N'Assign Employees To Follow Up Server Status', N'تعيين الموظفين لمتابعة حالة الخادم                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  ', 1)
GO
INSERT [dbo].[INV_SERVICE_REQUEST_TYPE_OSS_LKP] ([Id], [NameEn], [NameAr], [IsActive]) VALUES (64, N'Request To Issue Any GS Item', N'طلب إصدار أي مادة GS                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                ', 1)
GO
INSERT [dbo].[INV_SERVICE_REQUEST_TYPE_OSS_LKP] ([Id], [NameEn], [NameAr], [IsActive]) VALUES (128, N'Request To Issue Any IT Item', N'طلب إصدار أي مادة IT                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                ', 1)
GO
INSERT [dbo].[INV_SERVICE_REQUEST_TYPE_OSS_LKP] ([Id], [NameEn], [NameAr], [IsActive]) VALUES (256, N'Request To Return Any GS Item', N'طلب إرجاع أي منتج من فئة GS                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         ', 1)
GO
INSERT [dbo].[INV_SERVICE_REQUEST_TYPE_OSS_LKP] ([Id], [NameEn], [NameAr], [IsActive]) VALUES (512, N'Request To Return Any IT Item', N'طلب إرجاع أي منتج من فئة IT                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         ', 1)
GO
INSERT [dbo].[INV_SERVICE_REQUEST_TYPE_OSS_LKP] ([Id], [NameEn], [NameAr], [IsActive]) VALUES (1024, N'Request To Detach The Asset From Employee', N'طلب فصل الأصل عن الموظف                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             ', 1)
GO



------------------------------------------------Inventory Management Table DATA END------------------------------------------------------
/*<History Author='Umer Zaman' Date='04-04-2024'> Script start </History>*/

UPDATE LMS_BOOK_STATUS SET Name_Ar = N'مستعار' WHERE StatusId = 2 AND Name = 'Reserved'

/*<History Author='Umer Zaman' Date='04-04-2024'> Script end </History>*/
--------------
--Ammaar Naveed--04/04/2024--Claim value update for hearings list
UPDATE UMS_CLAIM SET ClaimValue='Permissions.Submenu.CMS.HearingsList' WHERE ClaimValue='Permissions.Submenu.CMS.UpcomingHearingsList'
UPDATE UMS_CLAIM SET Title_En='Current & Previous Hearings List' WHERE ClaimValue='Permissions.Submenu.CMS.HearingsList' 
UPDATE UMS_CLAIM SET Title_Ar='Current & Previous Hearings List' WHERE ClaimValue='Permissions.Submenu.CMS.HearingsList' 


--Fatwa script 07/04/2024
---notification place holder
  Insert into NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP values (94,'#Name#', 70) 
  Insert into NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP values (95,'#Name#', 71) 
  Insert into NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP values (96,'#Name#', 72) 
  Insert into NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP values (97,'#Name#', 73) 
  Insert into NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP values (98,'#Name#', 74) 
  Insert into NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP values (99,'#Name#', 75) 
  Insert into NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP values (100,'#Name#', 76) 
  Insert into NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP values (101,'#Name#', 77) 
  Insert into NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP values (102,'#Name#', 78) 


 --- insert NOTIF_NOTIFICATION_EVENT
INSERT INTO NOTIF_NOTIFICATION_EVENT ( EventId, NameEn, NameAr , CreatedBy , CreatedDate, ReceiverTypeId , IsDeleted)
VALUES (70,  'Create Lms Literature Borrow Detail' , 'Create Lms Literature Borrow Detail',  'System Generated' ,'2024-04-04 11:41:43.297', 1 , 0);
INSERT INTO NOTIF_NOTIFICATION_EVENT ( EventId, NameEn, NameAr , CreatedBy , CreatedDate, ReceiverTypeId , IsDeleted)
VALUES (71,  'Received Lms Literature Borrow Request' , 'Received Lms Literature Borrow Request',  'System Generated' ,'2024-04-04 11:41:43.297', 1 , 0);

 --- insert NOTIF_NOTIFICATION_EVENT 
INSERT INTO NOTIF_NOTIFICATION_EVENT ( EventId, NameEn, NameAr , CreatedBy , CreatedDate, ReceiverTypeId , IsDeleted)
VALUES (72,  'Lms Literature Borrow Request For Return' , 'Lms Literature Borrow Request For Return',  'System Generated' ,'2024-04-04 11:41:43.297', 1 , 0);

 --- insert NOTIF_NOTIFICATION_EVENT 
INSERT INTO NOTIF_NOTIFICATION_EVENT ( EventId, NameEn, NameAr , CreatedBy , CreatedDate, ReceiverTypeId , IsDeleted)
VALUES (73,  'Update Lms Literature Retun' , 'Update Lms Literature Retun',  'System Generated' ,'2024-04-04 11:41:43.297', 1 , 0);

 --- insert NOTIF_NOTIFICATION_EVENT 
INSERT INTO NOTIF_NOTIFICATION_EVENT ( EventId, NameEn, NameAr , CreatedBy , CreatedDate, ReceiverTypeId , IsDeleted)
VALUES (74,  'Update Literature Borrow Approval Status' , 'Update Literature Borrow Approval Status',  'System Generated' ,'2024-04-04 11:41:43.297', 1 , 0);
 --- insert NOTIF_NOTIFICATION_EVENT 
INSERT INTO NOTIF_NOTIFICATION_EVENT ( EventId, NameEn, NameAr , CreatedBy , CreatedDate, ReceiverTypeId , IsDeleted)
VALUES (75,  'Update Literature Borrow Reject Status' , 'Update Literature Borrow Reject Status',  'System Generated' ,'2024-04-04 11:41:43.297', 1 , 0);

INSERT INTO NOTIF_NOTIFICATION_EVENT ( EventId, NameEn, NameAr , CreatedBy , CreatedDate, ReceiverTypeId , IsDeleted)
VALUES (76,  'Lms Literature Borrow Request For Approval' , 'Lms Literature Borrow Request For Approval',  'System Generated' ,'2024-04-04 11:41:43.297', 1 , 0);

INSERT INTO NOTIF_NOTIFICATION_EVENT ( EventId, NameEn, NameAr , CreatedBy , CreatedDate, ReceiverTypeId , IsDeleted)
VALUES (77,  'Lms Literature Borrow Request For Rejection' , 'Lms Literature Borrow Request For Rejection',  'System Generated' ,'2024-04-04 11:41:43.297', 1 , 0);

INSERT INTO NOTIF_NOTIFICATION_EVENT ( EventId, NameEn, NameAr , CreatedBy , CreatedDate, ReceiverTypeId , IsDeleted)
VALUES (78,  'Lms Literature Borrow Request For Extension' , 'Lms Literature Borrow Request For Extension',  'System Generated' ,'2024-04-04 11:41:43.297', 1 , 0);

--- INSERT NOTIF_NOTIFICATION_TEMPLATE
 INSERT INTO NOTIF_NOTIFICATION_TEMPLATE (TemplateId, EventId, ChannelId, NameEn, NameAr ,SubjectEn, SubjectAr, BodyEn, BodyAr,CreatedBy , CreatedDate , IsDeleted )
VALUES (NewID(),70, 4, 'Create Lms Literature Borrow Detail' , 'Create Lms Literature Borrow Detail', 'Create Lms Literature Borrow Detail Successfully','Create Lms Literature Borrow Detail Successfully',
'Borrow request has been created for Literature, #Name# by #Sender Name# on #Created Date# for #Receiver Name#', 
'Borrow request has been created for Literature, #Name# by #Sender Name# on #Created Date# for #Receiver Name#', 
'System Generated' ,'2024-04-04 11:41:43.297',0);

 INSERT INTO NOTIF_NOTIFICATION_TEMPLATE (TemplateId, EventId, ChannelId, NameEn, NameAr ,SubjectEn, SubjectAr, BodyEn, BodyAr,CreatedBy , CreatedDate , IsDeleted )
VALUES (NewID(),71, 4, 'Received Lms Literature Borrow Request' , 'Received Lms Literature Borrow Request', 'Received Lms Literature Borrow Request Successfully','Received Lms Literature Borrow Request Successfully',
'Borrow request has been Received for Literature, #Name# by #Sender Name# on #Created Date# for #Receiver Name#', 
'Borrow request has been Received for Literature, #Name# by #Sender Name# on #Created Date# for #Receiver Name#', 
'System Generated' ,'2024-04-04 11:41:43.297',0);

--- INSERT NOTIF_NOTIFICATION_TEMPLATE 

 INSERT INTO NOTIF_NOTIFICATION_TEMPLATE (TemplateId, EventId, ChannelId, NameEn, NameAr ,SubjectEn, SubjectAr, BodyEn, BodyAr,CreatedBy , CreatedDate , IsDeleted )
VALUES (NewID(),74, 4, 'Update Literature Borrow Approval Status' , 'Update Literature Borrow Approval Status', 'Update Literature Borrow Approval Status','Update Lms Literature Retun Successfully',
'LMS approve borrow extension request has been created for Literature, #Name# by #Sender Name# on #Created Date# for #Receiver Name#.', 
'LMS approve borrow extension request has been created for Literature, #Name# by #Sender Name# on #Created Date# for #Receiver Name#.', 
'System Generated' ,'2024-04-04 11:41:43.297',0);

--- INSERT NOTIF_NOTIFICATION_TEMPLATE 

 INSERT INTO NOTIF_NOTIFICATION_TEMPLATE (TemplateId, EventId, ChannelId, NameEn, NameAr ,SubjectEn, SubjectAr, BodyEn, BodyAr,CreatedBy , CreatedDate , IsDeleted )
VALUES (NewID(),73, 4, 'Update Lms Literature Retun' , 'Update Lms Literature Retun', 'Update Lms Literature Retun Successfully','Update Lms Literature Retun Successfully',
'Lms Literature Retun has been updated for Literature, #Name# by #Sender Name# on #Created Date# for #Receiver Name#', 
'Lms Literature Retun has been updated for Literature, #Name# by #Sender Name# on #Created Date# for #Receiver Name#', 
'System Generated' ,'2024-04-04 11:41:43.297',0);

--- INSERT NOTIF_NOTIFICATION_TEMPLATE 

 INSERT INTO NOTIF_NOTIFICATION_TEMPLATE (TemplateId, EventId, ChannelId, NameEn, NameAr ,SubjectEn, SubjectAr, BodyEn, BodyAr,CreatedBy , CreatedDate , IsDeleted )
VALUES (NewID(),72, 4, 'Update Lms Literature Borrow Request' , 'Update Lms Literature Borrow Request', 'Update Lms Literature Borrow Request Successfully','Update Lms Literature Borrow Request Successfully',
'LMS return borrow request has been created for Literature, #Name# by #Sender Name# on #Created Date# for #Receiver Name#.', 
'LMS return borrow request has been created for Literature, #Name# by #Sender Name# on #Created Date# for #Receiver Name#.', 
'System Generated' ,'2024-04-04 11:41:43.297',0);

--- INSERT NOTIF_NOTIFICATION_TEMPLATE 

 INSERT INTO NOTIF_NOTIFICATION_TEMPLATE (TemplateId, EventId, ChannelId, NameEn, NameAr ,SubjectEn, SubjectAr, BodyEn, BodyAr,CreatedBy , CreatedDate , IsDeleted )
VALUES (NewID(),75, 4, 'Update Literature Borrow Reject Status' , 'Update Literature Borrow Reject Status', 'Update Literature Borrow Reject Status Successfully','Update Literature Borrow Reject Status Successfully',
'LMS reject borrow extension request has been created for Literature, #Name# by #Sender Name# on #Created Date# for #Receiver Name#.', 
'LMS reject borrow extension request has been created for Literature, #Name# by #Sender Name# on #Created Date# for #Receiver Name#.', 
'System Generated' ,'2024-04-04 11:41:43.297',0);


 INSERT INTO NOTIF_NOTIFICATION_TEMPLATE (TemplateId, EventId, ChannelId, NameEn, NameAr ,SubjectEn, SubjectAr, BodyEn, BodyAr,CreatedBy , CreatedDate , IsDeleted )
VALUES (NewID(),76, 4, 'Lms Literature Borrow Request For Approval' , 'Lms Literature Borrow Request For Approval', 'Lms Literature Borrow Request For Approval Successfully','Lms Literature Borrow Request For Approval Successfully',
'LMS approve borrow request has been created for Literature, #Name# by #Sender Name# on #Created Date# for #Receiver Name#.', 
'LMS approve borrow request has been created for Literature, #Name# by #Sender Name# on #Created Date# for #Receiver Name#.', 
'System Generated' ,'2024-04-04 11:41:43.297',0);

 INSERT INTO NOTIF_NOTIFICATION_TEMPLATE (TemplateId, EventId, ChannelId, NameEn, NameAr ,SubjectEn, SubjectAr, BodyEn, BodyAr,CreatedBy , CreatedDate , IsDeleted )
VALUES (NewID(),77, 4, 'Lms Literature Borrow Request For Rejection' , 'Lms Literature Borrow Request For Rejection', 'Lms Literature Borrow Request For Rejection Successfully','Lms Literature Borrow Request For Rejection Successfully',
'LMS reject borrow request has been created for Literature, #Name# by #Sender Name# on #Created Date# for #Receiver Name#.', 
'LMS reject borrow request request has been created for Literature, #Name# by #Sender Name# on #Created Date# for #Receiver Name#.', 
'System Generated' ,'2024-04-04 11:41:43.297',0);

 INSERT INTO NOTIF_NOTIFICATION_TEMPLATE (TemplateId, EventId, ChannelId, NameEn, NameAr ,SubjectEn, SubjectAr, BodyEn, BodyAr,CreatedBy , CreatedDate , IsDeleted )
VALUES (NewID(),78, 4, 'Lms Literature Borrow Request For Extension' , 'Lms Literature Borrow Request For Extension', 'Lms Literature Borrow Request For Extension Successfully','Lms Literature Borrow Request For Extension Successfully',
'LMS extension borrow request has been created for Literature, #Name# by #Sender Name# on #Created Date# for #Receiver Name#.', 
'LMS extension borrow request has been created for Literature, #Name# by #Sender Name# on #Created Date# for #Receiver Name#.', 
'System Generated' ,'2024-04-04 11:41:43.297',0); 


IF NOT Exists(SELECT 1 from UMS_GROUP_CLAIMS where GroupId = 'C01BDC00-8307-4E03-8E5C-7239F69A6A05' and ClaimValue = 'Permissions.Submenu.CMS.UpcomingHearingRollsList')
	INSERT INTO [dbo].[UMS_GROUP_CLAIMS] ([GroupId],[ClaimType],[ClaimValue])
		VALUES ('C01BDC00-8307-4E03-8E5C-7239F69A6A05', 'Permission', 'Permissions.Submenu.CMS.UpcomingHearingRollsList')
GO

IF NOT Exists(SELECT 1 from UMS_GROUP_CLAIMS where GroupId = 'C01BDC00-8307-4E03-8E5C-7239F69A6A05' and ClaimValue = 'Permissions.Submenu.CMS.HearingsList')
	INSERT INTO [dbo].[UMS_GROUP_CLAIMS] ([GroupId],[ClaimType],[ClaimValue])
		VALUES ('C01BDC00-8307-4E03-8E5C-7239F69A6A05', 'Permission', 'Permissions.Submenu.CMS.HearingsList')
GO

INSERT INTO UMS_CLAIM (Title_En, Title_Ar, Module, SubModule, ClaimType, ClaimValue, ModuleId, IsDeleted)
VALUES('Create Hearing Rolls',N'Create Hearing Rolls','Case_Management_System','Registered_Case','Permission','Permissions.Submenu.CMS.HearingRolls.Add',2,0)


IF NOT Exists(SELECT 1 from UMS_GROUP_CLAIMS where GroupId = 'C01BDC00-8307-4E03-8E5C-7239F69A6A05' and ClaimValue = 'Permissions.Submenu.CMS.HearingRolls.Add')
	INSERT INTO [dbo].[UMS_GROUP_CLAIMS] ([GroupId],[ClaimType],[ClaimValue])
		VALUES ('C01BDC00-8307-4E03-8E5C-7239F69A6A05', 'Permission', 'Permissions.Submenu.CMS.HearingRolls.Add')
GO
------Step 1
SET IDENTITY_INSERT WF_ACTIVITY_PR_LKP ON
INSERT [dbo].WF_ACTIVITY_PR_LKP([ActivityId],[Name],[Class],[Method],[CategoryId],[AKey],[IsEndofFlow]) 
VALUES (39,N'Vice_HOS_Review_Draft_Document','WorkflowImplementationService','Cms_ReviewDraftDocumentViceHOS',2,'CmsReviewDraftDocumentViceHOS',0)
SET IDENTITY_INSERT WF_ACTIVITY_PR_LKP OFF
-------------------step 2
INSERT INTO WF_TRIGGER_ACTIVITY_PR VALUES (4,39)
INSERT INTO WF_TRIGGER_ACTIVITY_PR VALUES (5,39)
-------------------- Step 3
SET IDENTITY_INSERT WF_PARAMETER_PR_LKP ON
INSERT [dbo].WF_PARAMETER_PR_LKP([ParameterId],[Name], [PKey],[Mandatory],[IsAutoPopulated])
VALUES (63,N'User', N'CmsReviewDraftDocumentViceHos_User',0,0)
SET IDENTITY_INSERT WF_PARAMETER_PR_LKP OFF

SET IDENTITY_INSERT WF_PARAMETER_PR_LKP ON
INSERT [dbo].WF_PARAMETER_PR_LKP([ParameterId],[Name], [PKey],[Mandatory],[IsAutoPopulated])
VALUES (64,N'User Role', N'CmsReviewDraftDocumentViceHos_UserRole',0,0)
SET IDENTITY_INSERT WF_PARAMETER_PR_LKP OFF
------------------- Step 4
INSERT [dbo].WF_ACTIVITY_PARAMETERS_PR([ParameterId],[ActivityId]) VALUES (63,39)
INSERT [dbo].WF_ACTIVITY_PARAMETERS_PR ([ParameterId],[ActivityId]) VALUES (64,39)
-----------
GO


IF((SELECT COUNT(*) FROM UMS_CLAIM WHERE ClaimValue = 'Permissions.CMS.HearingRolls.AddOutcome') = 0)
BEGIN
	INSERT INTO UMS_CLAIM (Title_En, Title_Ar, Module, SubModule, ClaimType, ClaimValue, ModuleId, IsDeleted)
	VALUES('Add Hearing Roll Outcome',N'Add Hearing Roll Outcome','Case_Management_System','Registered_Case','Permission','Permissions.CMS.HearingRolls.AddOutcome',2,0)
END
GO


IF NOT Exists(SELECT 1 from UMS_GROUP_CLAIMS where GroupId = 'C01BDC00-8307-4E03-8E5C-7239F69A6A05' and ClaimValue = 'Permissions.CMS.HearingRolls.AddOutcome')
	INSERT INTO [dbo].[UMS_GROUP_CLAIMS] ([GroupId],[ClaimType],[ClaimValue])
		VALUES ('C01BDC00-8307-4E03-8E5C-7239F69A6A05', 'Permission', 'Permissions.CMS.HearingRolls.AddOutcome')
GO
 INSERT INTO COMM_COMMUNICATION_TYPE VALUES (1073741828,N'التراسل الإلكتروني الحكومي','G2G Tarasol Correspondence','fatwaadmin@gmail.com','2024-04-18 13:01:00.000',NULL,NULL,NULL,NULL,0,1)

/*<History Author='Ammaar Naveed' Date='21-04-2024'> Employee Manageemnt/UMS_CLAIM Start </History>*/
UPDATE EP_DESIGNATION SET IsDeleted=0

INSERT INTO UMS_CLAIM(Title_En, Title_Ar, Module, SubModule, ClaimType, ClaimValue, IsDeleted, ModuleId)
VALUES
('Claims Submenu','Claims Submenu','User_Management','Claims','Permission','Admin.Permissions.Submenu.UMS.Claims',0,8192),
('Group Access Type Submenu','Group Access Type Submenu','User_Management','Group_Access_Type','Permission','Admin.Permissions.Submenu.GroupAccessType',0,8192),
('Websystems Submenu','Websystems Submenu','User_Management','Web_System','Permission','Admin.Permissions.Submenu.Websystem',0,8192)

DELETE FROM UMS_CLAIM WHERE ClaimValue='Admin.Permissions.Menu.Dashboard'
DELETE FROM UMS_CLAIM WHERE ClaimValue='Admin.Permissions.Users.Translations'

INSERT INTO UMS_CLAIM(Title_En, Title_Ar, Module, SubModule, ClaimType, ClaimValue, IsDeleted, ModuleId)
VALUES
('Side Menu','Side Menu','Sidemenu','FATWA_Administration_Portal','Permission','Admin.Permissions.Menu.Dashboard',0,2097152),
('Add Translations',N'إضافة ترجمة','User_Management','Translations','Permission','Admin.Permissions.Users.Translations',0,8192)

--Updating Permissions
UPDATE UMS_CLAIM SET IsDeleted=1 WHERE ClaimValue='Permissions.Lps_Latest.Principles.DetailsView'
UPDATE UMS_CLAIM SET Title_En='Legal Principles Detail View' WHERE ClaimValue='Permissions.Submenu.LPS.Principles.Detailsview'
UPDATE UMS_CLAIM SET IsDeleted=1 WHERE ClaimValue='Permissions.Submenu.LDS.Documents.List.Button.View'
UPDATE UMS_CLAIM SET IsDeleted=1 WHERE ClaimValue='Permissions.Submenu.LPS.Principles.List.Button.Publish'
UPDATE UMS_CLAIM SET IsDeleted=1 WHERE ClaimValue='Permissions.Submenu.LPS.Principles.List.Button.Edit'
UPDATE UMS_CLAIM SET Title_En='List of Deleted Principles' WHERE ClaimValue='Permissions.Submenu.LPS.Principles.List.Delete'
UPDATE UMS_CLAIM SET Title_En='Delete Principle (Action Button)' WHERE ClaimValue='Permissions.LPS.Principles.Delete'
UPDATE UMS_CLAIM SET IsDeleted=1 WHERE ClaimValue='Permissions.LPS.PrincipleVersionHistory'
UPDATE UMS_CLAIM SET IsDeleted=1 WHERE ClaimValue='Permissions.LPS.PrincipleVersionHistory.View'
UPDATE UMS_CLAIM SET IsDeleted=1 WHERE ClaimValue='Permissions.Submenu.LPS.Principles.List.Button.VersionHistory'
UPDATE UMS_CLAIM SET IsDeleted=1 WHERE ClaimValue='Permissions.Submenu.LPS.Principles.Tags'
UPDATE UMS_CLAIM SET IsDeleted=1 WHERE ClaimValue='Permissions.Submenu.LPS.Principles.Tags.Add'
UPDATE UMS_CLAIM SET IsDeleted=1 WHERE ClaimValue='Permissions.Submenu.LPS.Principles.Tags.Edit'
UPDATE UMS_CLAIM SET IsDeleted=1 WHERE ClaimValue='Permissions.Submenu.LPS.Principles.List.Button.Detail'
UPDATE UMS_CLAIM SET IsDeleted=1 WHERE ClaimValue='Permissions.Submenu.LPS.Principles.List.Button.ViewCommenthistory'
UPDATE UMS_CLAIM SET Title_En='Legal Principle Detail View' WHERE ClaimValue='Permissions.Lps_Latest.Principles.DetailsView'
UPDATE UMS_CLAIM SET Title_En='Legal Principle Details View' WHERE ClaimValue='Permissions.Submenu.LPS.Principles.Detailsview'
UPDATE UMS_CLAIM SET Title_En='Approval Status Decision (Borrow Request)' WHERE ClaimValue='Permissions.LMS.LiteratureBorrowDetail.BorrowApprovalStatusDiv'
UPDATE UMS_CLAIM SET IsDeleted=1 WHERE ClaimValue='Admin.Permissions.TransferUser.Save'
UPDATE UMS_CLAIM SET IsDeleted=1 WHERE ClaimValue='Admin.Permissions.Submenu.UMS.Users'
UPDATE UMS_CLAIM SET IsDeleted=1 WHERE ClaimValue='Admin.Permissions.TransferUser'
UPDATE UMS_CLAIM SET IsDeleted=1 WHERE ClaimValue='Admin.Permissions.Users.SystemConfiguration.Edit'
UPDATE UMS_CLAIM SET IsDeleted=1 WHERE ClaimValue='Admin.Permissions.Users.SystemConfiguration.Create'
UPDATE UMS_CLAIM SET Title_En='Create System Setting' WHERE ClaimValue='Admin.Permissions.Users.SystemConfiguration'
UPDATE UMS_CLAIM SET IsDeleted=1 WHERE ClaimValue='Admin.Permissions.Users.SystemConfiguration.Delete'
UPDATE UMS_CLAIM SET IsDeleted=1 WHERE ClaimValue='Admin.Permissions.Users.SystemSetting'
UPDATE UMS_CLAIM SET IsDeleted=1 WHERE ClaimValue='Admin.Permissions.Users.SystemConfiguration.View'
UPDATE UMS_CLAIM SET IsDeleted=1 WHERE ClaimValue='Admin.Permissions.Users.SystemConfiguration.Delete'
UPDATE UMS_CLAIM SET IsDeleted=1 WHERE ClaimValue='Admin.Permissions.Roles.Delete'
UPDATE UMS_CLAIM SET Title_En='List Employees' WHERE ClaimValue='Permissions.EM.Employee.List'
UPDATE UMS_CLAIM SET Module='FATWA_Dashboard' WHERE ClaimValue='Permissions.Dashboard.INV.View'
UPDATE UMS_CLAIM SET SubModule='Inventory_Management' WHERE ClaimValue='Permissions.Dashboard.INV.View'
UPDATE UMS_CLAIM SET ModuleId=1048576 WHERE ClaimValue='Permissions.Dashboard.INV.View'
UPDATE UMS_CLAIM SET Title_En='Inventory Management System' WHERE ClaimValue='Permissions.Dashboard.INV.View'
UPDATE UMS_CLAIM SET Title_Ar=N'نظام إدارة المخازن' WHERE ClaimValue='Permissions.Dashboard.INV.View'
UPDATE UMS_CLAIM SET Module='On_Demand_Request_Portal' WHERE ClaimValue='Permissions.Menu.ODRP'
UPDATE UMS_CLAIM SET SubModule='Menu' WHERE ClaimValue='Permissions.Menu.ODRP'
UPDATE UMS_CLAIM SET ModuleId=256 WHERE ClaimValue='Permissions.Menu.ODRP'
UPDATE UMS_CLAIM SET Title_En='On Demand Request Portal (Menu)' WHERE ClaimValue='Permissions.Menu.ODRP'
UPDATE UMS_CLAIM SET Title_Ar=N'(بوابة طلب معلومات من الجهة الحكومية (القائمة' WHERE ClaimValue='Permissions.Menu.ODRP'
UPDATE UMS_CLAIM SET SubModule='FATWA_Web_Portal' WHERE ClaimValue='Permissions.Menu.Dashboard'
UPDATE UMS_CLAIM SET Title_En='Main Menu Screen' WHERE ClaimValue='Permissions.Menu.Dashboard'
UPDATE UMS_CLAIM SET Title_Ar='Main Menu Screen' WHERE ClaimValue='Permissions.Menu.Dashboard'
UPDATE UMS_CLAIM SET ModuleId=2097152 WHERE ClaimValue='Permissions.Menu.Dashboard'
UPDATE UMS_CLAIM SET ModuleId=128 WHERE ClaimValue='Permissions.Menu.CMS/COMS'
UPDATE UMS_CLAIM SET Module='Case_Consultation_Management_System' WHERE ClaimValue='Permissions.Menu.CMS/COMS'
UPDATE UMS_CLAIM SET SubModule='FATWA_Admin_Portal' WHERE ClaimValue='Admin.Permissions.Menu.Dashboard'
UPDATE UMS_CLAIM SET Title_En='Add Translations' WHERE ClaimValue='Admin.Permissions.Users.Translations'
UPDATE UMS_CLAIM SET Title_Ar=N'إضافة ترجمة' WHERE ClaimValue='Admin.Permissions.Users.Translations'
UPDATE UMS_CLAIM SET Title_En='Web Systems Submenu' WHERE ClaimValue='Admin.Permissions.Submenu.Websystem'
UPDATE UMS_CLAIM SET SubModule='System_Settings' WHERE ClaimValue='Admin.Permissions.Users.SystemConfiguration'
UPDATE UMS_CLAIM SET Title_En='MOJ Execution Requests List' WHERE ClaimValue='Permissions.CMS.MOJ.ExecutionList'
UPDATE UMS_CLAIM SET Title_En='MOJ Execution Request Details' WHERE ClaimValue='Permissions.CMS.MOJ.ExecutionView'
UPDATE UMS_CLAIM SET Title_En='Draft Document (Create Button)' WHERE ClaimValue='Permissions.CMS.DraftDocument.CreateButton'
UPDATE UMS_CLAIM SET SubModule='Menu' WHERE ClaimValue='Permissions.Menu.CMS'
UPDATE UMS_CLAIM SET IsDeleted=1 WHERE ClaimValue='Admin.Permissions.Roles.Delete'
UPDATE UMS_CLAIM SET IsDeleted=1 WHERE ClaimValue='Permissions.CMS.CaseDraft.DraftDocument'
UPDATE UMS_CLAIM SET ModuleId=1048576 WHERE ClaimValue='Permissions.Menu.Dashboard'
UPDATE UMS_CLAIM SET SubModule='Main_Menu_Screen' WHERE ClaimValue='Permissions.Menu.Dashboard'
UPDATE UMS_CLAIM SET Title_En='Main Menu Screen (FATWA Web Portal)' WHERE ClaimValue='Permissions.Menu.Dashboard'
UPDATE UMS_CLAIM SET Module='FATWA_Dashboard' WHERE ClaimValue='Permissions.Menu.Dashboard'
UPDATE UMS_CLAIM SET Module='Side_Menu' WHERE ClaimValue='Admin.Permissions.Menu.Dashboard'
UPDATE UMS_CLAIM SET SubModule='Menu' WHERE ClaimValue='Permissions.Menu.COMS'
UPDATE UMS_CLAIM SET Title_En='Consultation Management (Menu)' WHERE ClaimValue='Permissions.Menu.COMS'
UPDATE UMS_CLAIM SET Title_En='Consultation Draft Document (Create Button)' WHERE ClaimValue='Permissions.COMS.DraftDocument.CreateButton'
UPDATE UMS_CLAIM SET Title_En='Document Management System (Menu)' WHERE ClaimValue='Permissions.DMS.Menu.Document'
UPDATE UMS_CLAIM SET Title_En='On Demand Request Portal' WHERE ClaimValue='Permissions.Dashboard.OnDemandPortal.View'
UPDATE UMS_CLAIM SET IsDeleted=1 WHERE ClaimValue='Permissions.CMS.CaseRequest.RequestMoreInfo'
UPDATE UMS_CLAIM SET IsDeleted=1 WHERE ClaimValue='Permissions.CMS.CaseRequest.NeedMoreInfo'
UPDATE UMS_CLAIM SET SubModule='Menu' WHERE ClaimValue='Permissions.Menu.Employee'
UPDATE UMS_CLAIM SET SubModule='Menu' WHERE ClaimValue='Permissions.Submenu.Employee.EmployeeList'
UPDATE UMS_CLAIM SET Title_En='Employee Management (Menu)' WHERE ClaimValue='Permissions.Menu.Employee'
UPDATE UMS_CLAIM SET Title_En='List Employees' WHERE ClaimValue='Permissions.EM.Employee.List'
UPDATE UMS_CLAIM SET Title_En='List Employees (Sub Menu)' WHERE ClaimValue='Permissions.Submenu.Employee.EmployeeList'
UPDATE UMS_CLAIM SET Title_En='Inventory Management (Menu)' WHERE ClaimValue='Permissions.Menu.INV'
/*<History Author='Ammaar Naveed' Date='21-04-2024'> Employee Manageemnt/UMS_CLAIM End </History>*/

/*<History Author='Ihsaan Abbas' Date='24-04-2024'> Notifications Start </History>*/

-----------------------NOTIF_NOTIFICATION_EVENT-------------------
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (1, N'New Case Request', N'New Case Request', N'System Generated', CAST(N'2022-09-06T12:27:53.330' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (2, N'Assign to Lawyer', N'Assign to Lawyer', N'System Generated', CAST(N'2022-09-06T12:27:53.330' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (3, N'Share Document', N'Share Document', N'System Generated', CAST(N'2022-09-06T12:27:53.400' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (4, N'Legal Notification Response', N'Legal Notification Response', N'System Generated', CAST(N'2022-09-06T12:27:43.330' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (5, N'Case Registered', N'Case Registered', N'System Generated', CAST(N'2022-09-06T12:27:43.330' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (6, N'Open File', N'Open File', N'System Generated', CAST(N'2022-09-06T12:27:43.330' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (7, N'Create Execution Request', N'Create Execution Request', N'System Generated', CAST(N'2022-09-06T12:27:43.330' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (8, N'Send Execution Request to MOJ', N'Send Execution Request to MOJ', N'System Generated', CAST(N'2022-09-06T00:00:00.000' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (9, N'Approve Execution Request', N'Approve Execution Request', N'System Generated', CAST(N'2022-09-02T00:00:00.000' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (10, N'Reject Execution Request', N'Reject Execution Request', N'System Generated', CAST(N'2024-03-19T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (12, N'Send a Copy', N'Send a Copy', N'System Generated', CAST(N'2024-03-19T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (13, N'Send a Copy Approved', N'Send a Copy Approved', N'System Generated', CAST(N'2024-03-19T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (14, N'Transfer of Sector', N'Transfer of Sector', N'System Generated', CAST(N'2024-03-19T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (15, N'Assign Back to HOS', N'Assign Back To Hos', N'System Generated', CAST(N'2024-03-19T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (16, N'Request for Meeting', N'Request for Meeting', N'System Generated', CAST(N'2024-03-20T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (17, N'Create Merge Request', N'Create Merge Request', N'System Generated', CAST(N'2024-03-20T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (18, N'Approve Merge Request', N'Approve Merge Request', N'System Generated', CAST(N'2024-03-20T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (19, N'New Consultation Request', N'New Consultation Request', N'System Generated', CAST(N'2024-03-20T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (20, N'Add Judgement', N'Add Judgement', N'System Generated', CAST(N'2024-03-20T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (21, N'Attendee Decision to Attend the Meeting', N'Attendee Decision to Attend the Meeting', N'System Generated', CAST(N'2024-03-21T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (22, N'Submit Judgment Execution', N'Submit Judgment Execution', N'System Generated', CAST(N'2024-03-21T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (23, N'Modify Legislation/Legal Principle Draft Document', N'Modify Legislation/Legal Principle Draft Document', N'System Generated', CAST(N'2024-03-21T16:39:16.277' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (24, N'Modify Draft', N'Modify Draft', N'System Generated', CAST(N'2024-03-23T14:54:04.243' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (25, N'Review Draft', N'Review Draft', N'System Generated', CAST(N'2024-03-23T14:54:04.243' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (26, N'Published Draft', N'Published Draft', N'System Generated', CAST(N'2024-03-23T14:54:04.243' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (27, N'Modify Document', N'Modify Document', N'System Generated', CAST(N'2024-03-23T14:54:04.247' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (28, N'Review Document', N'Review Document', N'System Generated', CAST(N'2024-03-23T14:54:04.247' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (29, N'Published Document', N'Published Document', N'System Generated', CAST(N'2024-03-23T14:54:04.247' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (30, N'Add Contact', N'Add Contact', N'System Generated', CAST(N'2024-03-23T15:16:07.310' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (31, N'Send to MOJ', N'Send to MOJ', N'System Generated', CAST(N'2024-03-23T15:29:34.570' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (32, N'Case Data Pushed from MOJ', N'Case Data Pushed from MOJ', N'System Generated', CAST(N'2024-03-23T16:54:27.510' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (33, N'Remove Attendee from Meeting', N'Remove Attendee from Meeting', N'System Generated', CAST(N'2024-03-24T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (34, N'Attendee Accept Meeting Invite', N'Attendee Accept Meeting Invite', N'System Generated', CAST(N'2024-03-24T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (35, N'Attendee Reject Meeting Invite', N'Attendee Reject Meeting Invite', N'System Generated', CAST(N'2024-03-24T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (36, N'HOS Decision to Approve the Meeting', N'HOS Decision to Approve the Meeting', N'System Generated', CAST(N'2024-03-24T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (37, N'Save Legislation', N'Save Legislation', N'System Generated', CAST(N'2024-03-24T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (38, N'Update Legislation', N'Update Legislation', N'System Generated', CAST(N'2024-03-24T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (39, N'Soft Delete Legislation', N'Soft Delete Legislation', N'System Generated', CAST(N'2024-03-24T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (40, N'Add Meeting Success', N'Add Meeting Success', N'System Generated', CAST(N'2024-03-25T10:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (41, N'Edit Meeting Success', N'Edit Meeting Success', N'System Generated', CAST(N'2024-03-25T10:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (42, N'Revoke Deleted Legislation', N'Revoke Deleted Legislation', N'System Generated', CAST(N'2024-03-25T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (43, N'Add Minutes of Meeting', N'Add Minutes of Meeting', N'System Generated', CAST(N'2024-03-26T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (44, N'Edit Minutes of Meeting', N'Edit Minutes of Meeting', N'System Generated', CAST(N'2024-03-24T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (45, N'Save Legal Principle', N'Save Legal Principle', N'System Generated', CAST(N'2024-03-26T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (46, N'Update Legal Principle', N'Update Legal Principle', N'System Generated', CAST(N'2024-03-26T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (47, N'Soft Delete Legal Principle', N'Soft Delete Legal Principle', N'System Generated', CAST(N'2024-03-26T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (48, N'Revoke Delete Legal Principle', N'Revoke Delete Legal Principle', N'System Generated', CAST(N'2024-03-26T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (49, N'GE Reject Meeting Invite', N'GE Reject Meeting Invite', N'System Generated', CAST(N'2024-03-24T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (50, N'GE Accept Meeting Invite', N'GE Accept Meeting Invite', N'System Generated', CAST(N'2024-03-24T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (51, N'Minutes of Meeting Created Successfully', N'Minutes of Meeting Created Successfully', N'System Generated', CAST(N'2024-03-24T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (52, N'Assign/Unassign Cases to Sector', N'Assign/Unassign Cases to Sector', N'System Generated', CAST(N'2024-03-26T19:25:23.337' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (53, N'Reminder To Complete Claim Statement', N'Reminder To Complete Claim Statement', N'System Generated', CAST(N'2024-03-27T12:49:44.420' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (54, N'Assign To MOJ Reminder', N'Assign To MOJ Reminder', N'System Generated', CAST(N'2024-03-27T14:04:57.163' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (55, N'Communication Response Reminder', N'Communication Response Reminder', N'System Generated', CAST(N'2024-03-27T14:04:57.167' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (56, N'Defense Letter Reminder', N'Defense Letter Reminder', N'System Generated', CAST(N'2024-03-27T14:04:57.167' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (57, N'Draft Modification Reminder', N'Draft Modification Reminder', N'System Generated', CAST(N'2024-03-27T14:04:57.167' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (58, N'HOS Appeal Reminder', N'HOS Appeal Reminder', N'System Generated', CAST(N'2024-03-27T14:04:57.167' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (59, N'HOS Regional Reminder', N'HOS Regional Reminder', N'System Generated', CAST(N'2024-03-27T14:04:57.167' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (60, N'HOS Supreme Reminder', N'HOS Supreme Reminder', N'System Generated', CAST(N'2024-03-27T14:04:57.170' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (61, N'Request For Additional Info Reminder', N'Request For Additional Info Reminder', N'System Generated', CAST(N'2024-03-27T14:04:57.170' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (62, N'Review Draft Reminder', N'Review Draft Reminder', N'System Generated', CAST(N'2024-03-27T14:04:57.170' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (63, N'Delete Parent Book Index', N'Delete Parent Book Index', N'System Generated', CAST(N'2024-03-31T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (64, N'Add Book', N'Add Book', N'System Generated', CAST(N'2024-03-31T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (65, N'Delete Literature', N'Delete Literature', N'System Generated', CAST(N'2024-03-31T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (66, N'Assign to Sector', N'Assign to Sector', N'System Generated', CAST(N'2024-03-31T21:03:25.550' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (67, N'Hearing Data Pushed From RPA', N'Hearing Data Pushed From RPA', N'System Generated', CAST(N'2024-04-02T11:39:51.423' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (68, N'Reject To Accept Assign File', N'Reject To Accept Assign File', N'System Generated', CAST(N'2024-04-02T11:39:51.423' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (69, N'Add Sub Case', N'Add Sub Case', N'System Generated', CAST(N'2024-04-02T13:08:24.147' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (70, N'Create Book Borrowing Request', N'إنشاء طلب إعارة كتاب', N'System Generated', CAST(N'2024-04-04T11:41:43.297' AS DateTime), N'fatwaadmin@gmail.com', CAST(N'2024-04-21T16:03:01.650' AS DateTime), NULL, NULL, 0, 1, NULL, N'New Book Borrowing Request', N'طلب استعارة كتاب جديد', 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (71, N'Received Book Borrowing Request', N'Received Book Borrowing Request', N'System Generated', CAST(N'2024-04-04T11:41:43.297' AS DateTime), N'fatwaadmin@gmail.com', CAST(N'2024-04-18T18:16:18.780' AS DateTime), NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (72, N'Request Return Borrowed Book', N'Request Return Borrowed Book', N'System Generated', CAST(N'2024-04-04T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (73, N'Update Request Return Borrowed Book', N'Update Request Return Borrowed Book', N'System Generated', CAST(N'2024-04-04T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (74, N'Approve Book Borrowing Request', N'Approve Book Borrowing Request', N'System Generated', CAST(N'2024-04-04T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (75, N'Reject Book Borrowing Request', N'Reject Book Borrowing Request', N'System Generated', CAST(N'2024-04-04T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (76, N'Book Borrowing Request Approved', N'Book Borrowing Request Approved', N'System Generated', CAST(N'2024-04-04T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (77, N'Book Borrowing Request Rejected', N'Book Borrowing Request Rejected', N'System Generated', CAST(N'2024-04-04T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (78, N'Extend Book Borrowing Period', N'Extend Book Borrowing Period', N'System Generated', CAST(N'2024-04-04T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO

-------------------------NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP-------------------------

INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (1, N'#Entity#', NULL)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (2, N'#Reference Number#', 2)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (5, N'#Sender Name#', NULL)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (6, N'#Receiver Name#', NULL)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (7, N'#Created Date#', NULL)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (8, N'#Document Name#', 3)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (9, N'#Request Number#', 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (10, N'#Reference Number#', 14)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (11, N'#Sector From#', 14)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (21, N'#Sector To#', 14)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (26, N'#File Number#', 15)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (27, N'#Case Number#', 8)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (28, N'#Case Number#', 7)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (29, N'#Case Number#', 9)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (30, N'#Case Number#', 10)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (32, N'#Case Number#', 17)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (33, N'#Primary Case Number#', 17)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (34, N'#Request Type#', 19)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (35, N'#Request Number#', 19)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (36, N'#Case Number#', 20)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (37, N'#Case Number#', 22)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (38, N'#Type#', 23)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (39, N'#Status#', 23)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (40, N'#Type#', 31)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (41, N'#Case Number#', 32)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (42, N'#File Number#', 32)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (43, N'#Legislation Number#', 37)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (44, N'#Legislation Number#', 38)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (45, N'#Type#', 25)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (46, N'#Type#', 24)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (47, N'#Type#', 26)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (48, N'#Legislation Number#', 39)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (49, N'#Case Number#', 5)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (50, N'#File Number#', 5)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (51, N'#Legislation Number#', 42)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (52, N'#Type#', 42)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (53, N'#Type#', 37)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (54, N'#Type#', 38)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (55, N'#Type#', 39)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (56, N'#File Number#', 20)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (57, N'#Sector From#', 9)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (58, N'#Sector From#', 8)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (59, N'#Sector To#', 52)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (60, N'#Type#', 46)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (61, N'#Type#', 47)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (62, N'#Type#', 48)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (63, N'#File Number#', 21)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (64, N'#File Number#', 51)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (65, N'#File Number#', 50)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (66, N'#File Number#', 49)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (67, N'#File Number#', 44)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (68, N'#File Number#', 43)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (69, N'#File Number#', 41)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (70, N'#File Number#', 40)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (71, N'#File Number#', 36)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (72, N'#File Number#', 35)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (73, N'#File Number#', 34)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (74, N'#File Number#', 33)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (75, N'#File Number#', 31)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (76, N'#Type#', 45)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (77, N'#File Number#', 53)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (78, N'#File Number#', 54)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (79, N'#Reference Number#', 4)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (80, N'#Reference Number#', 16)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (81, N'#Sector From#', 12)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (82, N'#Sector To#', 12)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (83, N'#File Number#', 56)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (84, N'#File Number#', 57)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (85, N'#File Number#', 61)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (86, N'#File Number#', 62)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (87, N'#Reference Number#', 66)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (88, N'#Sector From#', 66)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (89, N'#Sector To#', 66)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (90, N'#Reference Number#', 22)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (91, N'#File Number#', 68)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (92, N'#Case Number#', 69)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (93, N'#CAN Number#', 69)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (94, N'#Name#', 70)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (95, N'#Name#', 71)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (96, N'#Name#', 72)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (97, N'#Name#', 73)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (98, N'#Name#', 74)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (99, N'#Name#', 75)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (100, N'#Name#', 76)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (101, N'#Name#', 77)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (102, N'#Name#', 78)
GO
--------------------------NOTIF_NOTIFICATION_CHANNEL_LKP---------------

INSERT [dbo].[NOTIF_NOTIFICATION_CHANNEL_LKP] ([ChannelId], [NameEn], [NameAr]) VALUES (1, N'Email', N'البريد الالكتروني')
GO
INSERT [dbo].[NOTIF_NOTIFICATION_CHANNEL_LKP] ([ChannelId], [NameEn], [NameAr]) VALUES (2, N'Mobile', N'Mobile')
GO
INSERT [dbo].[NOTIF_NOTIFICATION_CHANNEL_LKP] ([ChannelId], [NameEn], [NameAr]) VALUES (4, N'Browser', N'Browser Arbi')
GO

 ------------------------- NOTIF_NOTIFICATION_TEMPLATE--------------

INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'4a12fc1a-0dd0-4a08-8c0b-07a5ff4cad68', 12, 4, N'Send A Copy Review', N'Send A Copy Review', N'Send A Copy Review', N'Send A Copy Review', N'A Copy of #Entity#, with #Entity# Number #Refrence Number# has been send to #Sector To# from #Sector From# on #Created Date# Date by #Sender Name# for review.', N'A Copy of #Entity#, with #Entity# Number #Refrence Number# has been send to #Sector To# from #Sector From# on #Created Date# Date by #Sender Name# for review.', NULL, N'System Generated', CAST(N'2024-03-21T16:18:12.853' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'c4bb0d17-12bc-4078-98de-09d90d4380be', 69, 4, N'Add Sub Case', N'Add Sub Case', N'Add Sub Case', N'Add Sub Case', N'Sub case has been added, #Case Number# on Can Number #CAN Number#.', N'Sub case has been added, #Case Number# on Can Number #CAN Number#.', NULL, N'System Generated', CAST(N'2024-04-02T13:26:13.823' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'99830c71-4155-4204-846e-0b73eba6835f', 76, 4, N'Lms Literature Borrow Request For Approval', N'Lms Literature Borrow Request For Approval', N'Lms Literature Borrow Request For Approval Successfully', N'Lms Literature Borrow Request For Approval Successfully', N'LMS approve borrow request has been created for Literature, #Name# by #Sender Name# on #Created Date# for #Receiver Name#.', N'LMS approve borrow request has been created for Literature, #Name# by #Sender Name# on #Created Date# for #Receiver Name#.', NULL, N'System Generated', CAST(N'2024-04-04T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'17ea1229-3f34-4333-81b6-0c29d21c4afb', 25, 4, N'Review Draft', N'Review Draft', N'Review Draft', N'Review Draft', N'The draft document #Type# has been sent to #Receiver Name# to be reviewed by #Sender Name# on the date #Created Date#', N'مسودة المستند #Type# تم ارسالها الى #Receiver Name# للمراجعة من قبل #Sender Name# بتاريخ #Created Date#', NULL, N'System Generated', CAST(N'2024-03-24T16:44:12.697' AS DateTime), N'FatwaAdmin@uat.fatwa.v.kw', CAST(N'2024-03-27T12:48:14.983' AS DateTime), NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'414adc17-e20e-4889-9d11-0ee4ba52ed5e', 36, 4, N' Meeting Decision of HOS For Approval', N' Meeting Decision of HOS For Approval', N' Meeting Decision of HOS For Approval', N' Meeting Decision of HOS For Approval', N'#Entity# is pending for your approval Sent by #Sender Name# on #Created Date#', N'#Entity# is pending for your approval Sent by #Sender Name# on #Created Date#', NULL, N'fatwaadmin@gmail.com', CAST(N'2024-03-24T12:45:52.087' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'6d434354-9a15-4029-9418-1680331685db', 34, 4, N'Attendee Accept Meeting Invite', N'Attendee Accept Meeting Invite', N'Attendee Accept Meeting Invite', N'Attendee Accept Meeting Invite', N'Attendee #Sender Name# Has Accept Your 
#Entity# Invite, on #Created Date#', N'Attendee #Sender Name# Has Accept Your 
#Entity# Invite, on #Created Date#', NULL, N'fatwaadmin@gmail.com', CAST(N'2024-03-24T11:34:05.560' AS DateTime), N'fatwaadmin@gmail.com', CAST(N'2024-03-24T15:33:48.027' AS DateTime), NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'06baa003-3f62-4049-b7c3-16e27c424f7b', 47, 4, N'Soft Delete Legal Principle', N'Soft Delete Legal Principle', N'Soft Delete Legal Principle Successfully', N'Soft Delete Legal Principle Successfully', N'Following #Entity# soft deleted with principle type #Type# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', N'Following #Entity# soft deleted with principle type #Type# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', NULL, N'System Generated', CAST(N'2024-03-26T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'7fb88bb4-873c-472e-ad7d-189495c79ec4', 75, 4, N'Update Literature Borrow Reject Status', N'Update Literature Borrow Reject Status', N'Update Literature Borrow Reject Status Successfully', N'Update Literature Borrow Reject Status Successfully', N'LMS reject borrow extension request has been created for Literature, #Name# by #Sender Name# on #Created Date# for #Receiver Name#.', N'LMS reject borrow extension request has been created for Literature, #Name# by #Sender Name# on #Created Date# for #Receiver Name#.', NULL, N'System Generated', CAST(N'2024-04-04T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'82ecc9dd-a845-459c-8a6c-205b16248259', 37, 4, N'Save Legal Legislation', N'Save Legal Legislation', N'Save Legal Legislationt Successfully', N'Save Legal Legislation Successfully', N'Following #Entity# added with Legislation type #Type# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', N'Following #Entity# added with Legislation type #Type# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', NULL, N'System Generated', CAST(N'2024-03-21T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'2f7bb222-0507-4338-a983-250e230b197b', 44, 4, N'Edit MOM Of Meeting', N'Edit MOM Of Meeting', N'Edit MOM Of Meeting', N'Edit MOM Of Meeting', N'A Minutes Of Meeting is modified by #Sender Name#  number #File Number# on the date #Created Date#', N'رقم تم تعديل محضر الاجتماع  #File Number# من قبل #Sender Name# بتاريخ #Created Date#', NULL, N'fatwaadmin@gmail.com', CAST(N'2024-03-27T00:01:44.997' AS DateTime), N'fatwaadmin@gmail.com', CAST(N'2024-03-27T00:05:34.540' AS DateTime), NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'9e76ce4e-4cdd-49ad-acc0-2772f2344479', 3, 4, N'Share Document', N'Share Document', N'User Share Document', N'User Share Document', N'Following #Entity#, #Document Name# share with #Receiver Name# user by #Sender Name#.', NULL, NULL, N'System Generated', CAST(N'2024-03-13T15:20:19.030' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'c5c3d4eb-9d67-452a-9bb8-29dfa24e0c51', 73, 4, N'Update Lms Literature Retun', N'Update Lms Literature Retun', N'Update Lms Literature Retun Successfully', N'Update Lms Literature Retun Successfully', N'Lms Literature Retun has been updated for Literature, #Name# by #Sender Name# on #Created Date# for #Receiver Name#', N'Lms Literature Retun has been updated for Literature, #Name# by #Sender Name# on #Created Date# for #Receiver Name#', NULL, N'System Generated', CAST(N'2024-04-04T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'3f71e0f5-cc49-4808-82c5-2bc6782c8d69', 46, 4, N'Update Legal Principle', N'Update Legal Principle', N'Update Legal Principle Successfully', N'Update Legal Principle Successfully', N'Following #Entity# updated with principle type #Type# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', N'Following #Entity# updated with principle type #Type# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', NULL, N'System Generated', CAST(N'2024-03-26T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'dddb9793-dc89-4f24-ab77-311dd288b0b4', 2, 4, N'Assign to Lawyer', N'Assign to Lawyer', N'File/Case Assign to Lawyer', N'File/Case Assign to Lawyer', N'The #Entity#  #Reference Number# has been assigned to #Receiver Name# by #Sender Name# on the date #Created Date#', N'تم تعيين #Entity# رقم #Reference Number# الى #Receiver Name# من قبل #Sender Name# بتاريخ #Created Date#', NULL, N'System Generated', CAST(N'2024-03-12T19:06:43.297' AS DateTime), N'fatwaadmin@gmail.com', CAST(N'2024-03-26T16:59:11.157' AS DateTime), NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'fb212623-849f-4c6e-b9e3-31d932385332', 53, 4, N'Reminder To Complete Claim Statement', N'Reminder To Complete Claim Statement', N'Reminder To Complete Claim Statement', N'Reminder To Complete Claim Statement', N'No claim statement document is created against Case File #File Number#.', N'No claim statement document is created against Case File #File Number#.', NULL, N'System Generated', CAST(N'2024-03-27T12:57:05.770' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'bb8b5bdc-ff46-494b-b343-39892eea7391', 43, 4, N'Add MOM Of Meeting', N'Add MOM Of Meeting', N'Add MOM Of Meeting', N'Add MOM Of Meeting', N'A Minutes of meeting is saved number #File Number# by #Sender Name# on the date #Created Date#', N'رقم تم حفظ محضر الاجتماع   #File Number# من قبل #Sender Name# بتاريخ #Created Date#', NULL, N'fatwaadmin@gmail.com', CAST(N'2024-03-26T23:54:23.610' AS DateTime), N'fatwaadmin@gmail.com', CAST(N'2024-03-27T00:06:13.907' AS DateTime), NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'136c15f8-7f19-4b48-96f3-40124177dbc3', 70, 2, N'test', N'test', NULL, NULL, N'test', N'test', NULL, N'fatwaadmin@gmail.com', CAST(N'2024-04-15T12:06:12.197' AS DateTime), NULL, NULL, NULL, NULL, 0, 0)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'b53bc393-1908-4a35-a105-40c2e8514610', 15, 4, N'Assign Back To Hos', N'Assign Back To Hos', N'Assign Back To Hos Successfully', N'Assign Back To Hos Successfully', N'Following #Entity#, with #Entity# number #File Number# has been assigned back to #Receiver Name# on #Created Date# Date by lawyer #Sender Name#.', N'Following #Entity#, with #Entity# number #File Number# has been assigned back to #Receiver Name# on #Created Date# Date by lawyer #Sender Name#.', NULL, N'System Generated', CAST(N'2024-03-14T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'f7afb7aa-120d-4b3b-80da-439b18eb86a5', 1, 4, N'Request Created', N'Request Created', N'Request Created Successfully updated', N'Request Created Successfully', N'Following #Entity#, with request number #Request Number# has been assigned to #Receiver Name# on #Created Date# Date .', NULL, NULL, N'System Generated', CAST(N'2024-03-14T11:41:43.297' AS DateTime), N'fatwaadmin@gmail.com', CAST(N'2024-03-19T12:03:48.670' AS DateTime), NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'ce2fe61d-a923-42ab-81b0-454a296dafa9', 48, 4, N'Revoke Delete Legal Principle', N'Revoke Delete Legal Principle', N'Revoke Delete Legal Principle Successfully', N'Revoke Delete Legal Principle Successfully', N'Following #Entity# revoke with principle type #Type# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', N'Following #Entity# revoke with principle type #Type# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', NULL, N'System Generated', CAST(N'2024-03-26T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'68322bf2-4873-4b11-a5ea-4b095af547bd', 26, 4, N'Published Draft', N'Published Draft', N'Published Draft', N'Modify Draft', N'Following Darft of type #Type# has been Published by #Sender Name#', N'Following Darft of type #Type# has been Published by #Sender Name#', NULL, N'System Generated', CAST(N'2024-03-24T16:49:33.913' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'62a254e8-2124-41eb-9aaa-54254f823519', 20, 4, N'Add Judgement', N'Add Judgement', N'Judgement Added Successfully', N'Judgement Added Successfully', N'Judgement is add on case number #Case Number# against file number #File Number# by #Sender Name# on #Created Date# Date.', N'Judgement is add on case number #Case Number# against file number #File Number# by #Sender Name# on #Created Date# Date.', NULL, N'System Generated', CAST(N'2024-03-21T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'e6c58878-61b4-48b6-ac63-5514dfd1d540', 33, 4, N'Delete Attendee From Meeting', N' Delete Attendee From Meeting', N' Delete Attendee From Meeting', N' Delete Attendee From Meeting', N'You have been deleted from #Entity# by #Sender Name# on #Created Date#', N'You have been deleted from #Entity# by #Sender Name# on #Created Date#', NULL, N'fatwaadmin@gmail.com', CAST(N'2024-03-24T15:33:18.747' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'dc3d8171-1308-463e-9702-566edaa17283', 17, 4, N'Create Merge Request', N'Create Merge Request', N'Create Merge Request Successfully', N'Create Merge Request Successfully', N'Following #Entity#, with Case number #Case Number#  created from #File Number# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', NULL, NULL, N'System Generated', CAST(N'2024-03-20T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'0d310a15-b2da-429c-a49b-62361f56d056', 16, 4, N'Request For Meeting', N'Request For Meeting', N'Request For Meeting', N'Request For Meeting', N'A meeting requesthas been received from #Sender Name# on #Entity# number, #Reference Number#.', N'A meeting requesthas been received from #Sender Name# on #Entity# number, #Reference Number#.', NULL, N'System Generated', CAST(N'2024-03-28T13:13:42.707' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'8b53ad90-c091-4621-b2e8-672ed1ea45a5', 21, 4, N'Attendee Decision For Meeting', N'Attendee Decision For Meeting', N'Attendee Decision For Meeting', N'Attendee Decision For Meeting', N'Following #Entity#, is pending for your descision created on #Created Date# sent by#Sender Name#.', N'Following #Entity#, is pending for your descision created on #Created Date# sent by#Sender Name#.', NULL, N'System Generated', CAST(N'2024-03-21T11:41:43.297' AS DateTime), N'fatwaadmin@gmail.com', CAST(N'2024-03-24T11:14:23.503' AS DateTime), NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'1880c8ba-7c68-476a-8044-692a3b191422', 72, 4, N'Update Lms Literature Borrow Request', N'Update Lms Literature Borrow Request', N'Update Lms Literature Borrow Request Successfully', N'Update Lms Literature Borrow Request Successfully', N'LMS return borrow request has been created for Literature, #Name# by #Sender Name# on #Created Date# for #Receiver Name#.', N'LMS return borrow request has been created for Literature, #Name# by #Sender Name# on #Created Date# for #Receiver Name#.', NULL, N'System Generated', CAST(N'2024-04-04T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'50105901-79ae-4b77-9ce1-6bede9cf4fe3', 13, 4, N'Send A Copy Approved', N'Send A Copy Approved', N'Send A Copy Approved', N'Send A Copy Approved', N'A Copy of #Entity#, with #Entity# Number #Refrence Number# has been approved received from #Sector From# sector on #Created Date# Date send by #Sender Name#.', N'A Copy of #Entity#, with #Entity# Number #Refrence Number# has been approved received from #Sector From# sector on #Created Date# Date send by #Sender Name#.', NULL, N'System Generated', CAST(N'2024-03-21T16:29:50.487' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'30046dc7-f0b8-45aa-844a-6fc18bdc1736', 71, 4, N'Received Lms Literature Borrow Request', N'Received Lms Literature Borrow Request', N'Received Lms Literature Borrow Request Successfully', N'Received Lms Literature Borrow Request Successfully', N'Borrow request has been Received for Literature, #Name# by #Sender Name# on #Created Date# for #Receiver Name#', N'Borrow request has been Received for Literature, #Name# by #Sender Name# on #Created Date# for #Receiver Name#', NULL, N'System Generated', CAST(N'2024-04-04T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'c121e56d-857b-4b55-a8d5-7054ecb87791', 39, 4, N'Soft Delete Legal Legislation', N'Soft Delete Legal Legislation', N'Soft Delete Legal Legislation Successfully', N'Soft Delete Legal Legislation Successfully', N'Following #Entity# soft deleted with Legislation type #Type# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', N'Following #Entity# soft deleted with Legislation type #Type# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', NULL, N'System Generated', CAST(N'2024-03-21T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'c0327b70-9c1e-41b6-a5bb-70d8a0e30912', 8, 4, N'Request Sent For Execution', N'Request Sent For Execution', N'Request Sent For Execution', N'Request Sent For Execution', N'You received execution request from #Sector From# on case number #Case Number# by #Sender Name# on #Created Date# Date to review.', N'You received execution request from #Sector From# on case number #Case Number# by #Sender Name# on #Created Date# Date to review.', NULL, N'System Generated', CAST(N'2024-03-19T17:25:44.147' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'469bd75d-f10b-4457-a8db-7ac3d0692230', 32, 4, N'Case Data Pushed From RPA', N'Case Data Pushed From RPA', N'Case Data Pushed From RPA', N'Case Data Pushed From RPA', N'A new case registered, number #Case Number# and has been sent to #Receiver Name# on the date #Created Date#.', N'تم تسجيل قضية جديدة رقم #Case Number# وارسلت الى #Receiver Name# بتاريخ #Created Date#', NULL, N'fatwaadmin@gmail.com', CAST(N'2024-03-26T17:09:33.300' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'f3f2f8b3-f74a-4439-ba38-7ca105da0919', 9, 4, N'Approve Execution Request', N'Approve Execution Request', N'Approve Execution Request', N'Approve Execution Request', N'You received execution request from #Sector From# on case number #Case Number# by #Sender Name# on #Created Date# Date to review.', N'You received execution request from #Sector From# on case number #Case Number# by #Sender Name# on #Created Date# Date to review.', NULL, N'System Generated', CAST(N'2024-03-19T23:30:47.300' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'92cd1f23-5e2e-4587-9f98-7cd92fa7d364', 5, 4, N'Case Registered', N'Case Registered', N'Case Registered', N'Case Registered', N'Following Case, with Case Number #Case Number# has been registered against File Number #File Number# by #Sender Name# on #Created Date# Date for #Receiver Name#', N'Following Case, with Case Number #Case Number# has been registered against File Number #File Number# by #Sender Name# on #Created Date# Date for #Receiver Name#', NULL, N'System Generated', CAST(N'2024-03-24T20:01:18.220' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'263ad40d-ef17-450e-bec2-823351186473', 62, 4, N'Review Draft Reminder', N'Review Draft Reminder', N'Review Draft Reminder', N'Review Draft Reminder', N'Review Draft Reminder against Case #File Number#.', N'Review Draft Reminder against Case #File Number#.', NULL, N'System Generated', CAST(N'2024-03-31T12:24:12.587' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'9c16f5f0-5de3-4d69-bc09-8259f1673a2c', 4, 4, N'Legal Notification Response', N'Legal Notification Response', N'Legal Notification Response', N'Legal Notification Response', N'A Legal notification has been received from #Sender Name# on #Entity# number, #Reference Number#.', N'A Legal notification has been received from #Sender Name# on #Entity# number, #Reference Number#.', NULL, N'fatwaadmin@gmail.com', CAST(N'2024-03-27T13:20:20.313' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'9b539b8a-f726-492a-b425-9afd624efa9e', 45, 4, N'Save Legal Principle', N'Save Legal Principle', N'Save Legal Principle Successfully', N'Save Legal Principle Successfully', N'Following #Entity# added with principle type #Type# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', N'Following #Entity# added with principle type #Type# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', NULL, N'System Generated', CAST(N'2024-03-26T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'be2bd815-6156-47f5-93f9-9b8865a17cea', 51, 4, N'MOM Created Successfully', N'MOM Created Successfully', N'MOM Created Successfully', N'MOM Created Successfully', N'A Minutes Of Meeting is sent by #Sender Name# number #File Number# on the date #Created Date#', N'رقم تم ارسال محضر الاجتماع  #File Number# من قبل #Sender Name# بتاريخ #Created Date#', NULL, N'fatwaadmin@gmail.com', CAST(N'2024-03-27T00:08:28.997' AS DateTime), N'fatwaadmin@gmail.com', CAST(N'2024-03-27T01:32:52.290' AS DateTime), NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'aaa78021-5ac3-472e-b4f3-a61879311f58', 68, 4, N'Approve Execution Request', N'Approve Execution Request', N'Approve Execution Request', N'Approve Execution Request', N'#Sender Name# reject to accept the file, #File Number# send by #Receiver Name#.', N'#Sender Name# reject to accept the file, #File Number# send by #Receiver Name#.', NULL, N'System Generated', CAST(N'2024-04-02T11:53:56.423' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'f97ec53b-b595-4b2f-9c76-a90365038cb4', 55, 4, N'Communication Response Not Completed', N'Communication Response Not Completed', N'Communication Response Not Completed', N'Communication Response Not Completed', N'Communication Response Not Completed against Case #Entity#.', N'Communication Response Not Completed against Case #Entity#.', NULL, N'System Generated', CAST(N'2024-03-31T13:07:23.073' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'e7935d6e-4dea-40c4-88a8-ad84c872b727', 18, 4, N'Approve Merge Request', N'Approve Merge Request', N'Approve Merge Request', N'Approve Merge Request', N'Request to merge cases has been approved', N'Request to merge cases has been approved', NULL, N'System Generated', CAST(N'2024-04-01T12:42:23.683' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'e0f53f84-1ceb-4aea-8155-afa7bb26d2eb', 42, 4, N'Revoke Delete Legal Legislation', N'Revoke Delete Legal Legislation', N'Revoke Delete Legal Legislation Successfully', N'Revoke Delete Legal Legislation Successfully', N'Following #Entity# revoke with Legislation type #Type# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', N'Following #Entity# revoke with Legislation type #Type# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', NULL, N'System Generated', CAST(N'2024-03-25T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'98f49257-e3ba-4819-9d7d-b4447ef253b2', 70, 4, N'Create Lms Literature Borrow Detail', N'Create Lms Literature Borrow Detail', N'Create Lms Literature Borrow Detail Successfully', N'Create Lms Literature Borrow Detail Successfully', N'Borrow request has been created for Literature, #Name# by #Sender Name# on #Created Date# for #Receiver Name#', N'Borrow request has been created for Literature, #Name# by #Sender Name# on #Created Date# for #Receiver Name#', NULL, N'System Generated', CAST(N'2024-04-04T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'387a7c13-87ea-4604-b6ee-bb6fc4d28655', 7, 4, N'Create Execution Request', N'Create Execution Request', N'Create Execution Request Successfully', N'Create Execution Request Successfully', N'Execution Request is add on case number #Case Number# by #Sender Name# on #Created Date# Date.', N'Execution Request is add on case number #Case Number# by #Sender Name# on #Created Date# Date.', NULL, N'System Generated', CAST(N'2024-03-21T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'bb38d542-2349-4c70-8973-bc5a9252f8d6', 10, 4, N'Reject Execution Request', N'Reject Execution Request', N'Reject Execution Request', N'Reject Execution Request', N'Execution Request for the Case, with case number #Case Number# has been rejected by #Receiver Name# on #Created Date# Date Send by #Sender Name#.', N'Execution Request for the Case, with case number #Case Number# has been rejected by #Receiver Name# on #Created Date# Date Send by #Sender Name#.', NULL, N'System Generated', CAST(N'2024-03-19T23:30:47.303' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'07989300-2042-4256-b9ed-bc7741b9db1f', 31, 4, N'Send To MOJ', N'Send To MOJ', N'Send To MOJ', N'Send To MOJ', N'Document of Type #Type# has been sent to MOJ for case registration, By sender #Sender Name#.', N'Document of Type #Type# has been sent to MOJ for case registration, By sender #Sender Name#.', NULL, N'System Generated', CAST(N'2024-03-23T16:02:00.333' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'3cb64213-6de3-4ff6-a1c5-c88de421fa9d', 54, 4, N'Assign To MOJ Reminder', N'Assign To MOJ Reminder', N'Assign To MOJ Reminder', N'Assign To MOJ Reminder', N'Case File #File Number# by System not yet registered in MOJ.', N'Case File #File Number# by System not yet registered in MOJ.', NULL, N'System Generated', CAST(N'2024-03-27T15:56:43.550' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'04331e0b-167a-42b6-beb5-d209a32f5eb8', 77, 4, N'Lms Literature Borrow Request For Rejection', N'Lms Literature Borrow Request For Rejection', N'Lms Literature Borrow Request For Rejection Successfully', N'Lms Literature Borrow Request For Rejection Successfully', N'LMS reject borrow request has been created for Literature, #Name# by #Sender Name# on #Created Date# for #Receiver Name#.', N'LMS reject borrow request request has been created for Literature, #Name# by #Sender Name# on #Created Date# for #Receiver Name#.', NULL, N'System Generated', CAST(N'2024-04-04T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'498d49be-92b6-4b04-a941-d2ef0338e8e8', 22, 4, N'Add Judgment Execution', N'Add Judgment Execution', N'Add Judgment Execution Successfully', N'Add Judgment Execution', N'Following #Entity#, with Case number #Case Number# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', N'Following #Entity#, with Case number #Case Number# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', NULL, N'System Generated', CAST(N'2024-03-21T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'e32d8354-ae1f-49e9-b134-d94a2f30796e', 19, 4, N'Consultation Request Created', N'Consultation Request Created', N'Consultation Request Created Successfully', N'Consultation Request Created Successfully', N'Following #Entity#, type #Request Type# with request number #Request Number# has been assigned to #Receiver Name# on #Created Date# Date .', N'Following #Entity#, type #Request Type# with request number #Request Number# has been assigned to #Receiver Name# on #Created Date# Date .', NULL, N'System Generated', CAST(N'2024-03-20T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'e9188734-6cda-4fdf-9fe9-db21dff2d7b7', 30, 4, N'Add Contact', N'Add Contact', N'Add Contact', N'Add Contact', N'New contact is successfully added in the system.', N'تم إضافة جهة اتصال جديدة بنجاح', NULL, N'System Generated', CAST(N'2024-03-23T15:19:47.467' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'99be75a7-ff3d-4443-97c6-db24d99fe30b', 61, 4, N'WS NOTIF No Additional Information Or Claim Statement Document Created', N'WS NOTIF No Additional Information Or Claim Statement Document Created', N'WS NOTIF No Additional Information Or Claim Statement Document Created', N'WS NOTIF No Additional Information Or Claim Statement Document Created', N'No additional information or claim statement document is created yet Case #File Number#.', N'No additional information or claim statement document is created yet Case #File Number#.', NULL, N'System Generated', CAST(N'2024-03-31T11:35:53.350' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'754df145-e6ee-4eaf-999b-db27f2854879', 66, 4, N'Assign to Sector', N'Assign to Sector', N'Assign to Sector', N'Assign to Sector', N'Following #Entity#, with #Entity# Number #Reference Number# has been assigned to #Sector To# from #Sector From# on #Created Date# Date by #Sender Name#.', N'Following #Entity#, with #Entity# Number #Reference Number# has been assigned to #Sector To# from #Sector From# on #Created Date# Date by #Sender Name#.', NULL, N'System Generated', CAST(N'2024-03-31T21:12:24.713' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'0cf53bd7-0230-48fe-94df-dc00e2e07e33', 56, 4, N'WS NOTIF Defense Letter Prepration Reminder', N'WS NOTIF Defense Letter Prepration Reminder', N'WS NOTIF Defense Letter Prepration Reminder', N'WS NOTIF Defense Letter Prepration Reminder', N'No Defence Document is created against Case #File Number#.', N'No Defence Document is created against Case #File Number#.', NULL, N'System Generated', CAST(N'2024-03-31T10:17:53.910' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'f474efb0-2b32-4d71-80f1-dc9bf8503014', 23, 4, N'Modify Legislation Draft Document', N'Modify Legislation Draft Document', N'Modify Legislation Draft Document', N'Modify Legislation Draft Document', N'#Entity# of type #Type# has been #Status# ,send by #Sender Name#.', N'#Entity# of type #Type# has been #Status# ,send by #Sender Name#.', NULL, N'System Generated', CAST(N'2024-03-21T16:46:52.167' AS DateTime), N'fatwaadmin@gmail.com', CAST(N'2024-03-26T15:40:50.557' AS DateTime), NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'73e03bb0-2515-40f6-a49b-ddc853b8471d', 57, 4, N'Draft Modification Not Completed', N'Draft Modification Not Completed', N'Draft Modification Not Completed', N'Draft Modification Not Completed', N'Draft Modification Not Completed against Case #File Number#.', N'Draft Modification Not Completed against Case #File Number#.', NULL, N'System Generated', CAST(N'2024-03-31T11:07:03.650' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'0f31770a-a581-4205-a7de-e298f8ea0e88', 35, 4, N' Attendee Reject Meeting Invite', N' Attendee Reject Meeting Invite', N' Attendee Reject Meeting Invite', N' Attendee Reject Meeting Invite', N'Attendee #Sender Name# Has Reject Your 
#Entity# Invite, on #Created Date#', N'Attendee #Sender Name# Has Reject Your 
#Entity# Invite, on #Created Date#', NULL, N'fatwaadmin@gmail.com', CAST(N'2024-03-24T15:34:37.873' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'157f6af7-2150-457c-8c6d-e9357828aa7b', 49, 4, N'GE Reject Meeting Invite', N'GE Reject Meeting Invite', N'GE Reject Meeting Invite', N'GE Reject Meeting Invite', N'A Meeting is rejected by #Sender Name# number #File Number# on the date #Created Date#', N'A Meeting is rejected by #Sender Name# number #File Number# on the date #Created Date#', NULL, N'fatwaadmin@gmail.com', CAST(N'2024-03-26T23:35:53.957' AS DateTime), N'fatwaadmin@gmail.com', CAST(N'2024-03-26T23:40:44.690' AS DateTime), NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'00abe322-836c-4922-947a-eeca54631c21', 24, 4, N'Modify Draft', N'Modify Draft', N'Modify Draft', N'Modify Draft', N'Following Darft of type #Type# has been send to #Receiver Name# to Modify send by #Sender Name#', N'Following Darft of type #Type# has been send to #Receiver Name# to Modify send by #Sender Name#', NULL, N'System Generated', CAST(N'2024-03-24T16:46:13.890' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'1a773ba2-97c5-41e3-adce-eecb423af676', 14, 4, N'Transfer Of Sector', N'Transfer Of Sector', N'Transfer Of Sector Successfully', N'Transfer Of Sector Successfully', N'Following #Entity#, with #Entity# Number #Reference Number# has been transfer from #Sector From# to #Sector To# on #Created Date# Date by #Sender Name#.', N'Following #Entity#, with #Entity# Number #Reference Number# has been transfer from #Sector From# to #Sector To# on #Created Date# Date by #Sender Name#.', NULL, N'System Generated', CAST(N'2024-03-14T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'be414ea0-343f-4054-8907-f317d4d78356', 40, 4, N'Add Meeting Success', N'Add Meeting Success', N'Add Meeting Success', N'Add Meeting Success', N'#Entity# has been created successfully, on #Created Date#', N'#Entity# has been created successfully, on #Created Date#', NULL, N'fatwaadmin@gmail.com', CAST(N'2024-03-25T10:26:38.033' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'c52c3789-d9de-4e44-bd84-f6d379e5d3fb', 41, 4, N'Edit Meeting Success', N'Edit Meeting Success', N'Edit Meeting Success', N'Edit Meeting Success', N'A Meeting is modified number #Request Number# by #Sender Name# on the date #Created Date#', N'رقم تم تعديل مقابلة #Request Number# من قبل #Sender Name# بتاريخ #Created Date#', NULL, N'fatwaadmin@gmail.com', CAST(N'2024-03-26T23:49:20.657' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'7093b956-e88f-4358-a8ed-f716fba13c09', 52, 4, N'Assig/Unassigned Cases to Sector', N'Assig/Unassigned Cases to Sector', N'Assig/Unassigned Cases to Sector', N'Assig/Unassigned Cases to Sector', N'The Case has been assigned to #Sector To# by #Sender Name# on the date #Created Date#', N'تم تعيين ملفات القضايا لقطاع #Sector To# من قبل #Sender Name# بتاريخ #Created Date#', NULL, N'fatwaadmin@gmail.com', CAST(N'2024-03-26T19:30:20.720' AS DateTime), N'fatwaadmin@gmail.com', CAST(N'2024-03-26T19:30:50.547' AS DateTime), NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'db99e1d1-1790-4f05-a0e3-f8cacfddace0', 78, 4, N'Lms Literature Borrow Request For Extension', N'Lms Literature Borrow Request For Extension', N'Lms Literature Borrow Request For Extension Successfully', N'Lms Literature Borrow Request For Extension Successfully', N'LMS extension borrow request has been created for Literature, #Name# by #Sender Name# on #Created Date# for #Receiver Name#.', N'LMS extension borrow request has been created for Literature, #Name# by #Sender Name# on #Created Date# for #Receiver Name#.', NULL, N'System Generated', CAST(N'2024-04-04T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'6ec890af-c730-4f86-a295-fb9bb6668fcc', 74, 4, N'Update Literature Borrow Approval Status', N'Update Literature Borrow Approval Status', N'Update Literature Borrow Approval Status', N'Update Lms Literature Retun Successfully', N'LMS approve borrow extension request has been created for Literature, #Name# by #Sender Name# on #Created Date# for #Receiver Name#.', N'LMS approve borrow extension request has been created for Literature, #Name# by #Sender Name# on #Created Date# for #Receiver Name#.', NULL, N'System Generated', CAST(N'2024-04-04T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'a28b8463-73e4-496d-9a51-fc31f319b220', 38, 4, N'Update Legal Legislation', N'Update Legal Legislation', N'Update Legal Legislation Successfully', N'Update Legal Legislation Successfully', N'Following #Entity# updated with Legislation type #Type# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', N'Following #Entity# updated with Legislation type #Type# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', NULL, N'System Generated', CAST(N'2024-03-21T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'f07284c2-0a1e-4c39-936f-fefdf1daba49', 50, 4, N'GE Accept Meeting Invite', N'GE Accept Meeting Invite', N'GE Accept Meeting Invite', N'GE Accept Meeting Invite', N'A Meeting is Accepted by #Sender Name# number #File Number# on the date #Created Date#', N'A Meeting is Accepted by #Sender Name# number #File Number# on the date #Created Date#', NULL, N'fatwaadmin@gmail.com', CAST(N'2024-03-26T23:44:31.963' AS DateTime), N'fatwaadmin@gmail.com', CAST(N'2024-03-27T15:46:13.807' AS DateTime), NULL, NULL, 0, 1)
GO

/*<History Author='Ihsaan Abbas' Date='24-04-2024'> Notification  End </History>*/

/*<History Author='Ihsaan Abbas' Date='24-04-2024'>CMS_BANK_G2G_LKP  Start </History>*/
INSERT INTO [dbo].[CMS_BANK_G2G_LKP] ([Name_En], [Name_Ar], [CreatedBy], [CreatedDate], [IsDeleted]) 
VALUES ('National Bank of Pakistan', N'البنك الوطني لباكستان', 'fatwaadmin@gmail.com', GETDATE(), 0);

INSERT INTO [dbo].[CMS_BANK_G2G_LKP] ([Name_En], [Name_Ar], [CreatedBy], [CreatedDate], [IsDeleted]) 
VALUES ('United Bank Limited', N'متحدہ بینک محدود', 'fatwaadmin@gmail.com', GETDATE(), 0);

INSERT INTO [dbo].[CMS_BANK_G2G_LKP] ([Name_En], [Name_Ar], [CreatedBy], [CreatedDate], [IsDeleted]) 
VALUES ('Habib Bank Limited', N'حبیب بینک لمیٹڈ', 'fatwaadmin@gmail.com', GETDATE(), 0);

INSERT INTO [dbo].[CMS_BANK_G2G_LKP] ([Name_En], [Name_Ar], [CreatedBy], [CreatedDate], [IsDeleted]) 
VALUES ('MCB Bank Limited', N'ام سی بی بینک لمیٹڈ', 'fatwaadmin@gmail.com', GETDATE(), 0);

INSERT INTO [dbo].[CMS_BANK_G2G_LKP] ([Name_En], [Name_Ar], [CreatedBy], [CreatedDate], [IsDeleted]) 
VALUES ('Allied Bank Limited', N'متحدہ بینک لمیٹڈ', 'fatwaadmin@gmail.com', GETDATE(), 0);

INSERT INTO [dbo].[CMS_BANK_G2G_LKP] ([Name_En], [Name_Ar], [CreatedBy], [CreatedDate], [IsDeleted]) 
VALUES ('Askari Bank Limited', N'اسکاری بینک لمیٹڈ', 'fatwaadmin@gmail.com', GETDATE(), 0);

INSERT INTO [dbo].[CMS_BANK_G2G_LKP] ([Name_En], [Name_Ar], [CreatedBy], [CreatedDate], [IsDeleted]) 
VALUES ('Bank Alfalah Limited', N'بینک الفلاح لمیٹڈ', 'fatwaadmin@gmail.com', GETDATE(), 0);

INSERT INTO [dbo].[CMS_BANK_G2G_LKP] ([Name_En], [Name_Ar], [CreatedBy], [CreatedDate], [IsDeleted]) 
VALUES ('Faysal Bank Limited', N'فیصل بینک لمیٹڈ', 'fatwaadmin@gmail.com', GETDATE(), 0);

/*<History Author='Ihsaan Abbas' Date='24-04-2024'>CMS_BANK_G2G_LKP  End </History>*/


------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
----------------------------------------------               BUG REPORTING  START            ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

/*<History Author='Arshad khan' Date='25-04-2024'>BUG_APPLICATION_G2G_LKP  End </History>*/

/*<History Author='Arshad khan' Date='25-04-2024'>BUG_APPLICATION_G2G_LKP  End </History>*/
INSERT [dbo].[BUG_APPLICATION_G2G_LKP] ([Id], [Name_En], [Name_Ar], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES(1, N'G2G Portal', N'G2G Portal', N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)

INSERT [dbo].[BUG_APPLICATION_G2G_LKP] ([Id], [Name_En], [Name_Ar], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) 
VALUES (2, N'Internal Portal', N'Internal Portal', N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)

INSERT [dbo].[BUG_APPLICATION_G2G_LKP] ([Id], [Name_En], [Name_Ar], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES (4, N'Task Management', N'Task Management', N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)

INSERT [dbo].[BUG_APPLICATION_G2G_LKP] ([Id], [Name_En], [Name_Ar], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) 
VALUES (8, N'Website', N'Website', N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)

INSERT [dbo].[BUG_APPLICATION_G2G_LKP] ([Id], [Name_En], [Name_Ar], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES (16, N'User Management', N'User Management', N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)

INSERT [dbo].[BUG_APPLICATION_G2G_LKP] ([Id], [Name_En], [Name_Ar], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES (32, 'Fatwa Portal', N'Fatwa Portal', 'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)

/*<History Author='Arshad khan' Date='25-04-2024'>BUG_ISSUE_TYPE_G2G_LKP  End </History>*/

SET IDENTITY_INSERT BUG_ISSUE_TYPE_G2G_LKP ON
INSERT [dbo].[BUG_ISSUE_TYPE_G2G_LKP] ([Id], [Type_En], [Type_Ar], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES (1, N'System Crashing', N'System Crashing', N'fatwaadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)

INSERT [dbo].[BUG_ISSUE_TYPE_G2G_LKP] ([Id], [Type_En], [Type_Ar], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES (2, N'Data Not Loading', N'Data Not Loading', N'fatwaadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)

INSERT [dbo].[BUG_ISSUE_TYPE_G2G_LKP] ([Id], [Type_En], [Type_Ar], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES (3, N'Data Not Found', N'Data Not Found', N'fatwaadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)

INSERT [dbo].[BUG_ISSUE_TYPE_G2G_LKP] ([Id], [Type_En], [Type_Ar], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES (4, N'Unable To Login', N'Unable To Login', N'fatwaadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)

INSERT [dbo].[BUG_ISSUE_TYPE_G2G_LKP] ([Id], [Type_En], [Type_Ar], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) 
VALUES (5, N'Button Not Working', N'Button Not Working', N'fatwaadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)

INSERT [dbo].[BUG_ISSUE_TYPE_G2G_LKP] ([Id], [Type_En], [Type_Ar], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES (6, N'Status Not Changing', N'Status Not Changing', N'fatwaadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)

INSERT [dbo].[BUG_ISSUE_TYPE_G2G_LKP] ([Id], [Type_En], [Type_Ar], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES (7, N'Other', N'Other', N'fatwaadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)
SET IDENTITY_INSERT BUG_ISSUE_TYPE_G2G_LKP OFF


/*<History Author='Arshad khan' Date='25-04-2024'>BUG_MODULE_G2G_LKP  End </History>*/
INSERT [dbo].[BUG_MODULE_G2G_LKP] ([Id], [Name_En], [Name_Ar], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) 
VALUES (1, N'Hiring', N'Hiring', N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)

INSERT [dbo].[BUG_MODULE_G2G_LKP] ([Id], [Name_En], [Name_Ar], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES (2, N'Leaves and Attendence', N'Leaves and Attendence', N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)

INSERT [dbo].[BUG_MODULE_G2G_LKP] ([Id], [Name_En], [Name_Ar], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES (4, N'Vendors and Contracts', N'Vendors and Contracts', N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)

INSERT [dbo].[BUG_MODULE_G2G_LKP] ([Id], [Name_En], [Name_Ar], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES (8, 'Case Management System', N'Case Management System', N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)

INSERT [dbo].[BUG_MODULE_G2G_LKP] ([Id], [Name_En], [Name_Ar], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES (16, 'User Management System', N'User Management System', N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)


INSERT [dbo].[BUG_MODULE_G2G_LKP] ([Id], [Name_En], [Name_Ar], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES (32, 'Consultation Management', N'Consultation Management', N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)

INSERT [dbo].[BUG_MODULE_G2G_LKP] ([Id], [Name_En], [Name_Ar], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES (64, 'DMS', N'DMS', N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)

INSERT [dbo].[BUG_MODULE_G2G_LKP] ([Id], [Name_En], [Name_Ar], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES (128, 'WorkFlow', N'WorkFlow', N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)

INSERT [dbo].[BUG_MODULE_G2G_LKP] ([Id], [Name_En], [Name_Ar], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES (256, 'Library', N'Library', N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)


/*<History Author='Arshad khan' Date='25-04-2024'>BUG_SEVERITY_G2G_LKP  End </History>*/
INSERT [dbo].[BUG_SEVERITY_G2G_LKP] ([Id], [Value_En], [Value_Ar], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES (1, N'Major', N'Major', N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)

INSERT [dbo].[BUG_SEVERITY_G2G_LKP] ([Id], [Value_En], [Value_Ar], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES (2, N'Minor', N'Minor', N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)

INSERT [dbo].[BUG_SEVERITY_G2G_LKP] ([Id], [Value_En], [Value_Ar], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES (4, N'Critical', N'Critical', N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)

INSERT [dbo].[BUG_SEVERITY_G2G_LKP] ([Id], [Value_En], [Value_Ar], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) 
VALUES (8, N'Show Stopper', N'Show Stopper', N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)


/*<History Author='Arshad khan' Date='25-04-2024'>BUG_STATUS_G2G_LKP  End </History>*/
INSERT [dbo].[BUG_STATUS_G2G_LKP] ([Id], [Value_En], [Value_Ar], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) 
VALUES (1, N'New', N'New', N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)

INSERT [dbo].[BUG_STATUS_G2G_LKP] ([Id], [Value_En], [Value_Ar], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) 
VALUES (2, N'In Progress', N'In Progress', N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)

INSERT [dbo].[BUG_STATUS_G2G_LKP] ([Id], [Value_En], [Value_Ar], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES (4, N'Assigned', N'Assigned', N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)

INSERT [dbo].[BUG_STATUS_G2G_LKP] ([Id], [Value_En], [Value_Ar], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) 
VALUES (8, N'Draft', N'Draft', N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)

INSERT [dbo].[BUG_STATUS_G2G_LKP] ([Id], [Value_En], [Value_Ar], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES (16, N'Resolved', N'Resolved', N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)

INSERT [dbo].[BUG_STATUS_G2G_LKP] ([Id], [Value_En], [Value_Ar], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES (32, N'Verified', N'Verified', N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)

INSERT [dbo].[BUG_STATUS_G2G_LKP] ([Id], [Value_En], [Value_Ar], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES (64, N'Reopened', N'Reopened', N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)

INSERT [dbo].[BUG_STATUS_G2G_LKP] ([Id], [Value_En], [Value_Ar], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES (128, N'Cancelled', N'Cancelled', N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)

INSERT [dbo].[BUG_STATUS_G2G_LKP] ([Id], [Value_En], [Value_Ar], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) 
VALUES (254, N'Closed', N'Closed', N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)
-----------------------------------------Notification FOR Add Bug Ticket
INSERT INTO NOTIF_NOTIFICATION_EVENT ( EventId, NameEn, NameAr , CreatedBy , CreatedDate, ReceiverTypeId , IsDeleted,IsActive)
VALUES (83,  'Bug Ticket Added' , 'Bug Ticket Added',  'System Generated' ,'2024-04-04 11:41:43.297', 1 , 0,1);
INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP ([PlaceHolderId],[PlaceHolderName],[EventId]) VALUES (104,'#Reference Number#' ,83)
--------------------------------

------------------------------------

Update BUG_ISSUE_TYPE_G2G_LKP set Id = 3 where Type_En = 'Data Not Found'
Update BUG_ISSUE_TYPE_G2G_LKP set Id = 4 where Type_En = 'Unable To Login'
Update BUG_ISSUE_TYPE_G2G_LKP set Id = 5 where Type_En = 'Button Not Working'
Update BUG_ISSUE_TYPE_G2G_LKP set Id = 6 where Type_En = 'Status Not Changing'
Update BUG_ISSUE_TYPE_G2G_LKP set Id = 7 where Type_En = 'Other'
------------------------------------
  INSERT INTO UMS_GROUP_CLAIMS VALUES ('071C4BA0-CC3F-43DC-B2CB-6FC2BEA6D831', 'Permission', 'Admin.Permissions.Menu.BUG')

  /*<History Author='Arshad khan' Date='12-05-2024'>BUG_STATUS_G2G_LKP  End </History>*/
INSERT [dbo].[BUG_ASSIGN_MODULE_APPLICATION]([ApplicationId], [ModuleId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES (32, 1, N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)

INSERT [dbo].[BUG_ASSIGN_MODULE_APPLICATION]([ApplicationId], [ModuleId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES (32, 2, N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)

INSERT [dbo].[BUG_ASSIGN_MODULE_APPLICATION]([ApplicationId], [ModuleId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES (32, 4, N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)

INSERT [dbo].[BUG_ASSIGN_MODULE_APPLICATION]([ApplicationId], [ModuleId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES (32, 8, N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)

INSERT [dbo].[BUG_ASSIGN_MODULE_APPLICATION]([ApplicationId], [ModuleId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES (32, 16, N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)

INSERT [dbo].[BUG_ASSIGN_MODULE_APPLICATION]([ApplicationId], [ModuleId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES (32, 32, N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)

INSERT [dbo].[BUG_ASSIGN_MODULE_APPLICATION]([ApplicationId], [ModuleId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES (32, 64, N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)

INSERT [dbo].[BUG_ASSIGN_MODULE_APPLICATION]([ApplicationId], [ModuleId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES (32, 128, N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)

INSERT [dbo].[BUG_ASSIGN_MODULE_APPLICATION]([ApplicationId], [ModuleId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES (32, 256, N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[BUG_ASSIGN_MODULE_APPLICATION]([ApplicationId], [ModuleId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES (1, 512, N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)



  INSERT INTO UMS_GROUP_CLAIMS VALUES ('071C4BA0-CC3F-43DC-B2CB-6FC2BEA6D831', 'Permission', 'Permissions.Reported.Bug.Add')
  INSERT INTO UMS_GROUP_CLAIMS VALUES ('071C4BA0-CC3F-43DC-B2CB-6FC2BEA6D831', 'Permission', 'Permissions.Bug.Ticket.Add')
  INSERT INTO UMS_GROUP_CLAIMS VALUES ('071C4BA0-CC3F-43DC-B2CB-6FC2BEA6D831', 'Permission', 'Permissions.Bug.Type.Add')
  INSERT INTO UMS_GROUP_CLAIMS VALUES ('071C4BA0-CC3F-43DC-B2CB-6FC2BEA6D831', 'Permission', 'Permissions.Comment.FeedBack.Add')
  INSERT INTO UMS_GROUP_CLAIMS VALUES ('071C4BA0-CC3F-43DC-B2CB-6FC2BEA6D831', 'Permission', 'Permissions.Assign.Type.User')
  INSERT INTO UMS_GROUP_CLAIMS VALUES ('071C4BA0-CC3F-43DC-B2CB-6FC2BEA6D831', 'Permission', 'Permissions.Assign.Type.Module')
  INSERT INTO UMS_GROUP_CLAIMS VALUES ('071C4BA0-CC3F-43DC-B2CB-6FC2BEA6D831', 'Permission', 'Permissions.Detail.Bug.Ticket')
  INSERT INTO UMS_GROUP_CLAIMS VALUES ('071C4BA0-CC3F-43DC-B2CB-6FC2BEA6D831', 'Permission', 'Permissions.Detail.Reported.Bug')
  INSERT INTO UMS_GROUP_CLAIMS VALUES ('071C4BA0-CC3F-43DC-B2CB-6FC2BEA6D831', 'Permission', 'Permissions.List.Bug.Ticket')
  INSERT INTO UMS_GROUP_CLAIMS VALUES ('071C4BA0-CC3F-43DC-B2CB-6FC2BEA6D831', 'Permission', 'Permissions.List.Bug.Type')
  INSERT INTO UMS_GROUP_CLAIMS VALUES ('071C4BA0-CC3F-43DC-B2CB-6FC2BEA6D831', 'Permission', 'Permissions.List.Crash.Report')
  INSERT INTO UMS_GROUP_CLAIMS VALUES ('071C4BA0-CC3F-43DC-B2CB-6FC2BEA6D831', 'Permission', 'Permissions.List.Reported.Bug')

  ---------------------------------------20/05/24
  
Update BUG_APPLICATION_G2G_LKP set Name_En='Fatwa Portal' ,Name_Ar='Fatwa Portal' where Id=1
Update BUG_APPLICATION_G2G_LKP set Name_En='Fatwa Admin Portal' ,Name_Ar='Fatwa Admin Portal' where Id=2
Update BUG_APPLICATION_G2G_LKP set Name_En='Document Management System' ,Name_Ar='Document Management System' where Id=4
Update BUG_APPLICATION_G2G_LKP set Name_En='G2G Portal' ,Name_Ar='G2G Portal' where Id=8
Update BUG_APPLICATION_G2G_LKP set Name_En='G2G Admin Portal' ,Name_Ar='G2G Admin Portal' where Id=16
Update BUG_APPLICATION_G2G_LKP set Name_En='Operations Support System' ,Name_Ar='Operations Support System' where Id=32




Update BUG_MODULE_G2G_LKP set Name_En='Case Management' ,Name_Ar='Case Management' where Id=1
Update BUG_MODULE_G2G_LKP set Name_En='Consultation Management' ,Name_Ar='Consultation Management' where Id=2
Update BUG_MODULE_G2G_LKP set Name_En='Contact Management' ,Name_Ar='Contact Management' where Id=4
Update BUG_MODULE_G2G_LKP set Name_En='Meeting Management' ,Name_Ar='Meeting Management' where Id=8
Update BUG_MODULE_G2G_LKP set Name_En='Task Management' ,Name_Ar='Task Management' where Id=16
Update BUG_MODULE_G2G_LKP set Name_En='WorkFlow' ,Name_Ar='WorkFlow' where Id=32
Update BUG_MODULE_G2G_LKP set Name_En='Time Tracking' ,Name_Ar='Time Tracking' where Id=64
Update BUG_MODULE_G2G_LKP set Name_En='Audit & History Tracking' ,Name_Ar='Audit & History Tracking' where Id=128
Update BUG_MODULE_G2G_LKP set Name_En='Universal Search' ,Name_Ar='Universal Search' where Id=256
Update BUG_MODULE_G2G_LKP set Name_En='User Management' ,Name_Ar='User Management' where Id=512
INSERT [dbo].[BUG_MODULE_G2G_LKP] ([Id], [Name_En], [Name_Ar], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES (1024, 'Legal Library System', N'Legal Library System', N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)


INSERT [dbo].[BUG_MODULE_G2G_LKP] ([Id], [Name_En], [Name_Ar], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES (2048, 'Admin Lookups', N'Admin Lookups', N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)

INSERT [dbo].[BUG_MODULE_G2G_LKP] ([Id], [Name_En], [Name_Ar], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES (4096, 'Bug Reporting', N'Bug Reporting', N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)

INSERT [dbo].[BUG_MODULE_G2G_LKP] ([Id], [Name_En], [Name_Ar], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES (8192, 'Decision Support', N'Decision Support', N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)

INSERT [dbo].[BUG_MODULE_G2G_LKP] ([Id], [Name_En], [Name_Ar], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES (16384, 'Reporting', N'Reporting', N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)

INSERT [dbo].[BUG_MODULE_G2G_LKP] ([Id], [Name_En], [Name_Ar], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES (32768, 'Document Management', N'Document Management', N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)

INSERT [dbo].[BUG_MODULE_G2G_LKP] ([Id], [Name_En], [Name_Ar], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES (65536, 'Human Resource', N'Human Resource', N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)

INSERT [dbo].[BUG_MODULE_G2G_LKP] ([Id], [Name_En], [Name_Ar], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES (131072, 'Finance', N'Finance', N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)

INSERT [dbo].[BUG_MODULE_G2G_LKP] ([Id], [Name_En], [Name_Ar], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES (262144, 'Information Technology', N'Information Technology', N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)

INSERT [dbo].[BUG_MODULE_G2G_LKP] ([Id], [Name_En], [Name_Ar], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES (524288, 'General Services', N'General Services', N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)

INSERT [dbo].[BUG_MODULE_G2G_LKP] ([Id], [Name_En], [Name_Ar], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES (1048576, 'Audit & Inspection', N'Audit & Inspection', N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)

INSERT [dbo].[BUG_MODULE_G2G_LKP] ([Id], [Name_En], [Name_Ar], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES (2097152, 'Legal Affairs', N'Legal Affairs', N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)

INSERT [dbo].[BUG_MODULE_G2G_LKP] ([Id], [Name_En], [Name_Ar], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES (4194304, 'Planning & Training', N'Planning & Training', N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)

INSERT [dbo].[BUG_MODULE_G2G_LKP] ([Id], [Name_En], [Name_Ar], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES (8388608, 'Organizing Committee', N'Organizing Committee', N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)

INSERT [dbo].[BUG_MODULE_G2G_LKP] ([Id], [Name_En], [Name_Ar], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES (16777216, 'Announcement & Memos', N'Announcement & Memos', N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)

INSERT [dbo].[BUG_MODULE_G2G_LKP] ([Id], [Name_En], [Name_Ar], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES (33554432, 'Vendors & Contracts', N'Vendors & Contracts', N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)

INSERT [dbo].[BUG_MODULE_G2G_LKP] ([Id], [Name_En], [Name_Ar], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES (67108864, 'Evaluation', N'Evaluation', N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)

INSERT [dbo].[BUG_MODULE_G2G_LKP] ([Id], [Name_En], [Name_Ar], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES (134217728, 'Leaves & Attendance', N'Leaves & Attendance', N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)


Update BUG_MODULE_APPLICATION set ApplicationId=1 ,ModuleId=1 where Id=1
Update BUG_MODULE_APPLICATION set ApplicationId=1 ,ModuleId=2 where Id=2
Update BUG_MODULE_APPLICATION set ApplicationId=1 ,ModuleId=4 where Id=3
Update BUG_MODULE_APPLICATION set ApplicationId=1 ,ModuleId=8 where Id=4
Update BUG_MODULE_APPLICATION set ApplicationId=1 ,ModuleId=16 where Id=5
Update BUG_MODULE_APPLICATION set ApplicationId=1 ,ModuleId=32 where Id=6
Update BUG_MODULE_APPLICATION set ApplicationId=1 ,ModuleId=64 where Id=7
Update BUG_MODULE_APPLICATION set ApplicationId=1 ,ModuleId=128 where Id=8
Update BUG_MODULE_APPLICATION set ApplicationId=1 ,ModuleId=256 where Id=9
Update BUG_MODULE_APPLICATION set ApplicationId=1 ,ModuleId=512 where Id=10
INSERT [dbo].[BUG_MODULE_APPLICATION]([ApplicationId], [ModuleId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES (1, 1024, N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[BUG_MODULE_APPLICATION]([ApplicationId], [ModuleId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES (2, 2048, N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[BUG_MODULE_APPLICATION]([ApplicationId], [ModuleId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES (2, 4096, N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[BUG_MODULE_APPLICATION]([ApplicationId], [ModuleId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES (2, 8192, N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[BUG_MODULE_APPLICATION]([ApplicationId], [ModuleId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES (2, 16384, N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)

INSERT [dbo].[BUG_MODULE_APPLICATION]([ApplicationId], [ModuleId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES (4, 32768, N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)

INSERT [dbo].[BUG_MODULE_APPLICATION]([ApplicationId], [ModuleId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES (8, 1, N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)

INSERT [dbo].[BUG_MODULE_APPLICATION]([ApplicationId], [ModuleId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES (8, 2, N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)

INSERT [dbo].[BUG_MODULE_APPLICATION]([ApplicationId], [ModuleId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES (8, 8, N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)

INSERT [dbo].[BUG_MODULE_APPLICATION]([ApplicationId], [ModuleId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES (8, 32768, N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[BUG_MODULE_APPLICATION]([ApplicationId], [ModuleId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES (16, 2048, N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[BUG_MODULE_APPLICATION]([ApplicationId], [ModuleId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES (16, 512, N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)


INSERT [dbo].[BUG_MODULE_APPLICATION]([ApplicationId], [ModuleId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES (32, 65536, N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)

INSERT [dbo].[BUG_MODULE_APPLICATION]([ApplicationId], [ModuleId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES (32, 131072, N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)

INSERT [dbo].[BUG_MODULE_APPLICATION]([ApplicationId], [ModuleId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES (32, 262144, N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)

INSERT [dbo].[BUG_MODULE_APPLICATION]([ApplicationId], [ModuleId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES (32, 524288, N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)

INSERT [dbo].[BUG_MODULE_APPLICATION]([ApplicationId], [ModuleId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES (32, 1048576, N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)

INSERT [dbo].[BUG_MODULE_APPLICATION]([ApplicationId], [ModuleId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES (32, 2097152, N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)

INSERT [dbo].[BUG_MODULE_APPLICATION]([ApplicationId], [ModuleId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES (32, 4194304, N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)

INSERT [dbo].[BUG_MODULE_APPLICATION]([ApplicationId], [ModuleId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES (32, 8388608, N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)

INSERT [dbo].[BUG_MODULE_APPLICATION]([ApplicationId], [ModuleId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES (32, 16777216, N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)

INSERT [dbo].[BUG_MODULE_APPLICATION]([ApplicationId], [ModuleId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES (32, 33554432, N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)

INSERT [dbo].[BUG_MODULE_APPLICATION]([ApplicationId], [ModuleId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES (32, 67108864, N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)

INSERT [dbo].[BUG_MODULE_APPLICATION]([ApplicationId], [ModuleId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted])
VALUES (32, 134217728, N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)

---------------------------------------
--------------- Main Menu
IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Permissions.Menu.BugReporting')
	INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted],[ModuleId])
		VALUES ('Side Menu Item','Bug_Reporting', 'Menu', 'Permission', 'Permissions.Menu.BugReporting','Side Menu Item',0,8388608)
GO 
---------------------- Submenu + List
IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Permissions.SubMenu.Bug.ReportedBug.List')
	INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted],[ModuleId])
		VALUES ('Reported Bug List','Bug_Reporting', 'SubMenu', 'Permission', 'Permissions.SubMenu.Bug.ReportedBug.List','Reported Bug List',0,8388608)
GO 
IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Permissions.SubMenu.Ticket.List')
	INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted],[ModuleId])
		VALUES ('Tickets List','Bug_Reporting', 'SubMenu', 'Permission', 'Permissions.SubMenu.Ticket.List','Tickets List',0,8388608)
GO 

IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Permissions.SubMenu.CrashReport.List')
	INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted],[ModuleId])
		VALUES ('Crash Report List','Bug_Reporting', 'SubMenu', 'Permission', 'Permissions.SubMenu.CrashReport.List','Crash Report List',0,8388608)
GO 

IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Permissions.SubMenu.IssueType.List')
	INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted],[ModuleId])
		VALUES ('Issue Type List','Bug_Reporting', 'SubMenu', 'Permission', 'Permissions.SubMenu.IssueType.List','Issue Type List',0,8388608)
GO 
----------------------
-------------------- Add Forms

IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Permissions.Bug.Ticket.Add')
	INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted],[ModuleId])
		VALUES ('Raise A Ticket','Bug_Reporting', 'Ticket', 'Permission', 'Permissions.Bug.Ticket.Add','Raise A Ticket',0,8388608)
GO 

IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Permissions.Bug.ReportedBug.Add')
	INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted],[ModuleId])
		VALUES ('Report A Bug','Bug_Reporting', 'Bug', 'Permission', 'Permissions.Bug.ReportedBug.Add','Report A Bug',0,8388608)
GO 
IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Permissions.Bug.Type.Add')
	INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted],[ModuleId])
		VALUES ('Add Bug Type','Bug_Reporting', 'Bug', 'Permission', 'Permissions.Bug.Type.Add','Add Bug Type',0,8388608)
GO 

-------------------- Detail Screens

IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Permissions.Bug.Ticket.Detail')
	INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted],[ModuleId])
		VALUES ('View Ticket Detail','Bug_Reporting', 'Ticket', 'Permission', 'Permissions.Bug.Ticket.Detail','View Ticket Detail',0,8388608)
GO 

IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Permissions.Bug.ReportedBug.Detail')
	INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted],[ModuleId])
		VALUES ('View Bug Detail','Bug_Reporting', 'Bug', 'Permission', 'Permissions.Bug.ReportedBug.Detail','View Bug Detail',0,8388608)
GO 
-------------------------- ROLE BUG REPORTING ADMIN
INSERT INTO UMS_ROLE (Id,Name,NormalizedName,ConcurrencyStamp,Description_En,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,DeletedBy,DeletedDate,IsDeleted,Description_Ar,NameAr)
VALUES('B69EF02F-96EF-4192-A01F-4C20A0F6809D' ,'Bug Reporting Admin','BUG REPORTING ADMIN','1C36C73A-A82A-42A0-8BE7-3BC1A92256A2','This user has the rights to add, update, view bug reporting module.','fatwaadmin@gmail.com','2024-05-29 14:59:59.263',NULL,NULL,NULL,NULL,0,N'This user has the rights to add, update, view bug reporting module.','Bug Reporting Admin')
-----------------

INSERT INTO NOTIF_NOTIFICATION_EVENT ( EventId, NameEn, NameAr , CreatedBy , CreatedDate, ReceiverTypeId , IsDeleted,IsActive)
VALUES (88,  'Comment Added' , 'Comment Added',  'System Generated' ,'2024-04-04 11:41:43.297', 1 , 0,1);
-------------------
INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP ([PlaceHolderId],[PlaceHolderName],[EventId]) VALUES (105,'#Reference Number#' ,88)
-------------------


INSERT INTO NOTIF_NOTIFICATION_EVENT ( EventId, NameEn, NameAr , CreatedBy , CreatedDate, ReceiverTypeId , IsDeleted,IsActive)
VALUES (89,  'Feedback Added' , 'Feedback Added',  'System Generated' ,'2024-04-04 11:41:43.297', 1 , 0,1);
--------------------
INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP ([PlaceHolderId],[PlaceHolderName],[EventId]) VALUES (106,'#Reference Number#' ,89)
----------------------
INSERT INTO UMS_ROLE (Id,Name,NormalizedName,ConcurrencyStamp,Description_En,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,DeletedBy,DeletedDate,IsDeleted,Description_Ar,NameAr)
VALUES('B66BEBA6-675C-436C-8514-5DFEE4690E0A' ,'IT Support','IT SUPPORT','C84D83AD-550E-4AFE-968E-A866C43CD67E','This user has the rights to view , update status, add comment ,add feedback and add resolution on ticket.','fatwaadmin@gmail.com','2024-05-29 14:59:59.263',NULL,NULL,NULL,NULL,0,N'This user has the rights to view , update status, add comment ,add feedback and add resolution on ticket.','IT Support')
-----------------------

INSERT INTO NOTIF_NOTIFICATION_EVENT ( EventId, NameEn, NameAr , CreatedBy , CreatedDate, ReceiverTypeId , IsDeleted,IsActive)
VALUES (90,  'Assign Ticket' , 'Assign Ticket',  'System Generated' ,'2024-04-04 11:41:43.297', 1 , 0,1);
--------------------
INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP ([PlaceHolderId],[PlaceHolderName],[EventId]) VALUES (107,'#Reference Number#' ,90)
-----------------------------
Update BUG_STATUS_G2G_LKP Set Value_En = 'Draft' , Value_Ar ='Draft' where Id = 2
Update BUG_STATUS_G2G_LKP Set Value_En = 'Assigned' , Value_Ar ='Assigned' where Id = 4
Update BUG_STATUS_G2G_LKP Set Value_En = 'Closed' , Value_Ar ='Closed' where Id = 8
Update BUG_STATUS_G2G_LKP Set Value_En = 'Resolved' , Value_Ar ='Resolved' where Id = 16
Update BUG_STATUS_G2G_LKP Set Value_En = 'InProgress' , Value_Ar ='InProgress' where Id = 32
Update BUG_STATUS_G2G_LKP Set Value_En = 'Reopened' , Value_Ar ='Reopened' where Id = 64
Update BUG_STATUS_G2G_LKP Set Value_En = 'Cancelled' , Value_Ar ='Cancelled' where Id = 128
Update BUG_STATUS_G2G_LKP Set Id = 256 where Id = 254
Update BUG_STATUS_G2G_LKP Set Value_En = 'Verified' , Value_Ar ='Verified' where Id = 256
-------------------------------
INSERT INTO  BUG_STATUS_G2G_LKP([Id],[Value_En],[Value_Ar],[CreatedBy],[CreatedDate],[ModifiedBy],[ModifiedDate],[DeletedBy],[DeletedDate],[IsDeleted]) 
VALUES(512, 'Rejected' , N'Rejected' ,'superadmin@gmail.com','2022-08-02 11:14:25.280',NULL,NULL,NULL,NULL,0)
-----------------------
INSERT INTO NOTIF_NOTIFICATION_EVENT ( EventId, NameEn, NameAr , CreatedBy , CreatedDate, ReceiverTypeId , IsDeleted,IsActive)
VALUES (91,  'Reject Ticket' , 'Reject Ticket',  'System Generated' ,'2024-04-04 11:41:43.297', 1 , 0,1);
INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP ([PlaceHolderId],[PlaceHolderName],[EventId]) VALUES (108,'#Reference Number#' ,91)
--------------------
INSERT INTO NOTIF_NOTIFICATION_EVENT ( EventId, NameEn, NameAr , CreatedBy , CreatedDate, ReceiverTypeId , IsDeleted,IsActive)
VALUES (92,  'Resolve Ticket' , 'Resolve Ticket',  'System Generated' ,'2024-04-04 11:41:43.297', 1 , 0,1);
INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP ([PlaceHolderId],[PlaceHolderName],[EventId]) VALUES (109,'#Reference Number#' ,92)
--------------------
INSERT INTO NOTIF_NOTIFICATION_EVENT ( EventId, NameEn, NameAr , CreatedBy , CreatedDate, ReceiverTypeId , IsDeleted,IsActive)
VALUES (93,  'ReOpen Ticket' , 'ReOpen Ticket',  'System Generated' ,'2024-04-04 11:41:43.297', 1 , 0,1);
INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP ([PlaceHolderId],[PlaceHolderName],[EventId]) VALUES (110,'#Reference Number#' ,93)
-------------------------
IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Permissions.Bug.Ticket.AddFeedBack')
	INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted],[ModuleId])
		VALUES ('Add Ticket FeedBack','Bug_Reporting', 'Ticket', 'Permission', 'Permissions.Bug.Ticket.AddFeedBack','Add Ticket FeedBack',0,8388608)
GO 
----------------
INSERT INTO NOTIF_NOTIFICATION_EVENT ( EventId, NameEn, NameAr , CreatedBy , CreatedDate, ReceiverTypeId , IsDeleted,IsActive)
VALUES (94,  'Close Ticket' , 'Close Ticket',  'System Generated' ,'2024-04-04 11:41:43.297', 1 , 0,1);
INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP ([PlaceHolderId],[PlaceHolderName],[EventId]) VALUES (111,'#Reference Number#' ,94)
----------------------
INSERT [dbo].[BUG_EVENT_G2G_LKP] ([Id], [Name_En], [Name_Ar], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) 
VALUES (1, N'Ticket Drafted', N'Ticket Drafted', N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)

INSERT [dbo].[BUG_EVENT_G2G_LKP] ([Id], [Name_En], [Name_Ar], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) 
VALUES (2, N'Ticket Raised', N'Ticket Raised', N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)

INSERT [dbo].[BUG_EVENT_G2G_LKP] ([Id], [Name_En], [Name_Ar], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) 
VALUES (4, N'Ticket Assigned', N'Ticket Assigned', N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[BUG_EVENT_G2G_LKP] ([Id], [Name_En], [Name_Ar], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) 
VALUES (8, N'Ticket Accepted', N'Ticket Accepted', N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[BUG_EVENT_G2G_LKP] ([Id], [Name_En], [Name_Ar], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) 
VALUES (16, N'Ticket Rejected', N'Ticket Rejected', N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[BUG_EVENT_G2G_LKP] ([Id], [Name_En], [Name_Ar], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) 
VALUES (32, N'Resolution Added', N'Resolution Added', N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[BUG_EVENT_G2G_LKP] ([Id], [Name_En], [Name_Ar], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) 
VALUES (64, N'Ticket Closed', N'Ticket Closed', N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[BUG_EVENT_G2G_LKP] ([Id], [Name_En], [Name_Ar], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) 
VALUES (128, N'Ticket ReOpened', N'Ticket ReOpened', N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[BUG_EVENT_G2G_LKP] ([Id], [Name_En], [Name_Ar], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) 
VALUES (256, N'Comment Added', N'Comment Added', N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[BUG_EVENT_G2G_LKP] ([Id], [Name_En], [Name_Ar], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) 
VALUES (512, N'Feedback Added', N'Feedback Added', N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[BUG_EVENT_G2G_LKP] ([Id], [Name_En], [Name_Ar], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) 
VALUES (1024, N'Bug Reported', N'Bug Reported', N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[BUG_EVENT_G2G_LKP] ([Id], [Name_En], [Name_Ar], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) 
VALUES (2048, N'Comment Reply Added', N'Comment Reply Added', N'superadmin@gmail.com', CAST(N'2022-08-02T11:14:25.280' AS DateTime), NULL, NULL, NULL, NULL, 0)
--------------------
IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Permissions.Bug.Ticket.Close')
	INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted],[ModuleId])
		VALUES ('Close Ticket','Bug_Reporting', 'Ticket', 'Permission', 'Permissions.Bug.Ticket.Close','Close Ticket',0,8388608)
GO 
-------------------


Update BUG_STATUS_G2G_LKP Set Value_En = 'New' , Value_Ar =N'جديد' where Id = 1
Update BUG_STATUS_G2G_LKP Set Value_En = 'Draft' , Value_Ar =N'مسودة' where Id = 2
Update BUG_STATUS_G2G_LKP Set Value_En = 'Assigned' , Value_Ar =N'تم التعيين' where Id = 4
Update BUG_STATUS_G2G_LKP Set Value_En = 'Closed' , Value_Ar =N'أغلقت' where Id = 8
Update BUG_STATUS_G2G_LKP Set Value_En = 'Resolved' , Value_Ar =N'تم الحل' where Id = 16
Update BUG_STATUS_G2G_LKP Set Value_En = 'InProgress' , Value_Ar =N'قيد التقدم' where Id = 32
Update BUG_STATUS_G2G_LKP Set Value_En = 'Reopened' , Value_Ar =N'إعادة فتحه' where Id = 64
Update BUG_STATUS_G2G_LKP Set Value_En = 'Cancelled' , Value_Ar =N'ألغيت' where Id = 128
Update BUG_STATUS_G2G_LKP Set Value_En = 'Verified' , Value_Ar =N'تم التحقق' where Id = 256
Update BUG_STATUS_G2G_LKP Set Value_En = 'Rejected' , Value_Ar =N'ألغيت' where Id = 512
----------------------


Update BUG_SEVERITY_G2G_LKP Set Value_En = 'Major' , Value_Ar =N'أساسي' where Id = 1
Update BUG_SEVERITY_G2G_LKP Set Value_En = 'Minor' , Value_Ar =N'غير خطير' where Id = 2
Update BUG_SEVERITY_G2G_LKP Set Value_En = 'Critical' , Value_Ar =N'شديد الأهمية' where Id = 4
Update BUG_SEVERITY_G2G_LKP Set Value_En = 'Show Stopper' , Value_Ar =N'مسبب تعطيل' where Id = 8
-----------------
------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
----------------------------------------------               BUG REPORTING     END           ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
/*<History Author='Ihsaan Abbas' Date='24-04-2024'>CMS_BANK_G2G_LKP  End </History>*/ 
  /*<History Author='Ihsaan Abbas' Date='24-04-2024'> Notifications Start </History>*/
---Before delete first run the below script of Nocheck 
ALTER TABLE dbo.NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP 
NOCHECK CONSTRAINT FK__NOTIF_NOT__Event__5E7FE7D2;


ALTER TABLE dbo.NOTIF_NOTIFICATION_TEMPLATE 
NOCHECK CONSTRAINT FK__NOTIF_NOT__Event__625078B6;

ALTER TABLE dbo.NOTIF_NOTIFICATION_TEMPLATE 
NOCHECK CONSTRAINT FK__NOTIF_NOT__Chann__63449CEF;

 
 delete from NOTIF_NOTIFICATION_EVENT 
 delete from NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP
 delete from  NOTIF_NOTIFICATION_CHANNEL_LKP
 delete from NOTIF_NOTIFICATION_TEMPLATE
  ----Run below script 


-----------------------NOTIF_NOTIFICATION_EVENT-------------------
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (1, N'New Case Request', N'New Case Request', N'System Generated', CAST(N'2022-09-06T12:27:53.330' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (2, N'Assign to Lawyer', N'Assign to Lawyer', N'System Generated', CAST(N'2022-09-06T12:27:53.330' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (3, N'Share Document', N'Share Document', N'System Generated', CAST(N'2022-09-06T12:27:53.400' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (4, N'Legal Notification Response', N'Legal Notification Response', N'System Generated', CAST(N'2022-09-06T12:27:43.330' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (5, N'Case Registered', N'Case Registered', N'System Generated', CAST(N'2022-09-06T12:27:43.330' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (6, N'Open File', N'Open File', N'System Generated', CAST(N'2022-09-06T12:27:43.330' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (7, N'Create Execution Request', N'Create Execution Request', N'System Generated', CAST(N'2022-09-06T12:27:43.330' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (8, N'Send Execution Request to MOJ', N'Send Execution Request to MOJ', N'System Generated', CAST(N'2022-09-06T00:00:00.000' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (9, N'Approve Execution Request', N'Approve Execution Request', N'System Generated', CAST(N'2022-09-02T00:00:00.000' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (10, N'Reject Execution Request', N'Reject Execution Request', N'System Generated', CAST(N'2024-03-19T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (12, N'Send a Copy', N'Send a Copy', N'System Generated', CAST(N'2024-03-19T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (13, N'Send a Copy Approved', N'Send a Copy Approved', N'System Generated', CAST(N'2024-03-19T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (14, N'Transfer of Sector', N'Transfer of Sector', N'System Generated', CAST(N'2024-03-19T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (15, N'Assign Back to HOS', N'Assign Back To Hos', N'System Generated', CAST(N'2024-03-19T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (16, N'Request for Meeting', N'Request for Meeting', N'System Generated', CAST(N'2024-03-20T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (17, N'Create Merge Request', N'Create Merge Request', N'System Generated', CAST(N'2024-03-20T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (18, N'Approve Merge Request', N'Approve Merge Request', N'System Generated', CAST(N'2024-03-20T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (19, N'New Consultation Request', N'New Consultation Request', N'System Generated', CAST(N'2024-03-20T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (20, N'Add Judgement', N'Add Judgement', N'System Generated', CAST(N'2024-03-20T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (21, N'Attendee Decision to Attend the Meeting', N'Attendee Decision to Attend the Meeting', N'System Generated', CAST(N'2024-03-21T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (22, N'Submit Judgment Execution', N'Submit Judgment Execution', N'System Generated', CAST(N'2024-03-21T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (23, N'Modify Legislation/Legal Principle Draft Document', N'Modify Legislation/Legal Principle Draft Document', N'System Generated', CAST(N'2024-03-21T16:39:16.277' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (24, N'Modify Draft', N'Modify Draft', N'System Generated', CAST(N'2024-03-23T14:54:04.243' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (25, N'Review Draft', N'Review Draft', N'System Generated', CAST(N'2024-03-23T14:54:04.243' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (26, N'Published Draft', N'Published Draft', N'System Generated', CAST(N'2024-03-23T14:54:04.243' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (27, N'Modify Document', N'Modify Document', N'System Generated', CAST(N'2024-03-23T14:54:04.247' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (28, N'Review Document', N'Review Document', N'System Generated', CAST(N'2024-03-23T14:54:04.247' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (29, N'Published Document', N'Published Document', N'System Generated', CAST(N'2024-03-23T14:54:04.247' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (30, N'Add Contact', N'Add Contact', N'System Generated', CAST(N'2024-03-23T15:16:07.310' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (31, N'Send to MOJ', N'Send to MOJ', N'System Generated', CAST(N'2024-03-23T15:29:34.570' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (32, N'Case Data Pushed from MOJ', N'Case Data Pushed from MOJ', N'System Generated', CAST(N'2024-03-23T16:54:27.510' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (33, N'Remove Attendee from Meeting', N'Remove Attendee from Meeting', N'System Generated', CAST(N'2024-03-24T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (34, N'Attendee Accept Meeting Invite', N'Attendee Accept Meeting Invite', N'System Generated', CAST(N'2024-03-24T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (35, N'Attendee Reject Meeting Invite', N'Attendee Reject Meeting Invite', N'System Generated', CAST(N'2024-03-24T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (36, N'HOS Decision to Approve the Meeting', N'HOS Decision to Approve the Meeting', N'System Generated', CAST(N'2024-03-24T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (37, N'Save Legislation', N'Save Legislation', N'System Generated', CAST(N'2024-03-24T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (38, N'Update Legislation', N'Update Legislation', N'System Generated', CAST(N'2024-03-24T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (39, N'Soft Delete Legislation', N'Soft Delete Legislation', N'System Generated', CAST(N'2024-03-24T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (40, N'Add Meeting Success', N'Add Meeting Success', N'System Generated', CAST(N'2024-03-25T10:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (41, N'Edit Meeting Success', N'Edit Meeting Success', N'System Generated', CAST(N'2024-03-25T10:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (42, N'Revoke Deleted Legislation', N'Revoke Deleted Legislation', N'System Generated', CAST(N'2024-03-25T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (43, N'Add Minutes of Meeting', N'Add Minutes of Meeting', N'System Generated', CAST(N'2024-03-26T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (44, N'Edit Minutes of Meeting', N'Edit Minutes of Meeting', N'System Generated', CAST(N'2024-03-24T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (45, N'Save Legal Principle', N'Save Legal Principle', N'System Generated', CAST(N'2024-03-26T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (46, N'Update Legal Principle', N'Update Legal Principle', N'System Generated', CAST(N'2024-03-26T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (47, N'Soft Delete Legal Principle', N'Soft Delete Legal Principle', N'System Generated', CAST(N'2024-03-26T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (48, N'Revoke Delete Legal Principle', N'Revoke Delete Legal Principle', N'System Generated', CAST(N'2024-03-26T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (49, N'GE Reject Meeting Invite', N'GE Reject Meeting Invite', N'System Generated', CAST(N'2024-03-24T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (50, N'GE Accept Meeting Invite', N'GE Accept Meeting Invite', N'System Generated', CAST(N'2024-03-24T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (51, N'Minutes of Meeting Created Successfully', N'Minutes of Meeting Created Successfully', N'System Generated', CAST(N'2024-03-24T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (52, N'Assign/Unassign Cases to Sector', N'Assign/Unassign Cases to Sector', N'System Generated', CAST(N'2024-03-26T19:25:23.337' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (53, N'Reminder To Complete Claim Statement', N'Reminder To Complete Claim Statement', N'System Generated', CAST(N'2024-03-27T12:49:44.420' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (54, N'Assign To MOJ Reminder', N'Assign To MOJ Reminder', N'System Generated', CAST(N'2024-03-27T14:04:57.163' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (55, N'Communication Response Reminder', N'Communication Response Reminder', N'System Generated', CAST(N'2024-03-27T14:04:57.167' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (56, N'Defense Letter Reminder', N'Defense Letter Reminder', N'System Generated', CAST(N'2024-03-27T14:04:57.167' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (57, N'Draft Modification Reminder', N'Draft Modification Reminder', N'System Generated', CAST(N'2024-03-27T14:04:57.167' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (58, N'HOS Appeal Reminder', N'HOS Appeal Reminder', N'System Generated', CAST(N'2024-03-27T14:04:57.167' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (59, N'HOS Regional Reminder', N'HOS Regional Reminder', N'System Generated', CAST(N'2024-03-27T14:04:57.167' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (60, N'HOS Supreme Reminder', N'HOS Supreme Reminder', N'System Generated', CAST(N'2024-03-27T14:04:57.170' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (61, N'Request For Additional Info Reminder', N'Request For Additional Info Reminder', N'System Generated', CAST(N'2024-03-27T14:04:57.170' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (62, N'Review Draft Reminder', N'Review Draft Reminder', N'System Generated', CAST(N'2024-03-27T14:04:57.170' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (63, N'Delete Parent Book Index', N'Delete Parent Book Index', N'System Generated', CAST(N'2024-03-31T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (64, N'Add Book', N'Add Book', N'System Generated', CAST(N'2024-03-31T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (65, N'Delete Literature', N'Delete Literature', N'System Generated', CAST(N'2024-03-31T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (66, N'Assign to Sector', N'Assign to Sector', N'System Generated', CAST(N'2024-03-31T21:03:25.550' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (67, N'Hearing Data Pushed From RPA', N'Hearing Data Pushed From RPA', N'System Generated', CAST(N'2024-04-02T11:39:51.423' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (68, N'Reject To Accept Assign File', N'Reject To Accept Assign File', N'System Generated', CAST(N'2024-04-02T11:39:51.423' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (69, N'Add Sub Case', N'Add Sub Case', N'System Generated', CAST(N'2024-04-02T13:08:24.147' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (70, N'Create Book Borrowing Request', N'إنشاء طلب إعارة كتاب', N'System Generated', CAST(N'2024-04-04T11:41:43.297' AS DateTime), N'fatwaadmin@gmail.com', CAST(N'2024-04-21T16:03:01.650' AS DateTime), NULL, NULL, 0, 1, NULL, N'New Book Borrowing Request', N'طلب استعارة كتاب جديد', 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (71, N'Received Book Borrowing Request', N'Received Book Borrowing Request', N'System Generated', CAST(N'2024-04-04T11:41:43.297' AS DateTime), N'fatwaadmin@gmail.com', CAST(N'2024-04-18T18:16:18.780' AS DateTime), NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (72, N'Request Return Borrowed Book', N'Request Return Borrowed Book', N'System Generated', CAST(N'2024-04-04T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (73, N'Update Request Return Borrowed Book', N'Update Request Return Borrowed Book', N'System Generated', CAST(N'2024-04-04T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (74, N'Approve Book Borrowing Request', N'Approve Book Borrowing Request', N'System Generated', CAST(N'2024-04-04T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (75, N'Reject Book Borrowing Request', N'Reject Book Borrowing Request', N'System Generated', CAST(N'2024-04-04T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (76, N'Book Borrowing Request Approved', N'Book Borrowing Request Approved', N'System Generated', CAST(N'2024-04-04T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (77, N'Book Borrowing Request Rejected', N'Book Borrowing Request Rejected', N'System Generated', CAST(N'2024-04-04T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) VALUES (78, N'Extend Book Borrowing Period', N'Extend Book Borrowing Period', N'System Generated', CAST(N'2024-04-04T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
GO

-------------------------NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP-------------------------

INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (1, N'#Entity#', NULL)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (2, N'#Reference Number#', 2)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (5, N'#Sender Name#', NULL)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (6, N'#Receiver Name#', NULL)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (7, N'#Created Date#', NULL)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (8, N'#Document Name#', 3)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (9, N'#Request Number#', 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (10, N'#Reference Number#', 14)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (11, N'#Sector From#', 14)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (21, N'#Sector To#', 14)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (26, N'#File Number#', 15)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (27, N'#Case Number#', 8)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (28, N'#Case Number#', 7)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (29, N'#Case Number#', 9)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (30, N'#Case Number#', 10)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (32, N'#Case Number#', 17)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (33, N'#Primary Case Number#', 17)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (34, N'#Request Type#', 19)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (35, N'#Request Number#', 19)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (36, N'#Case Number#', 20)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (37, N'#Case Number#', 22)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (38, N'#Type#', 23)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (39, N'#Status#', 23)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (40, N'#Type#', 31)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (41, N'#Case Number#', 32)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (42, N'#File Number#', 32)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (43, N'#Legislation Number#', 37)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (44, N'#Legislation Number#', 38)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (45, N'#Type#', 25)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (46, N'#Type#', 24)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (47, N'#Type#', 26)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (48, N'#Legislation Number#', 39)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (49, N'#Case Number#', 5)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (50, N'#File Number#', 5)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (51, N'#Legislation Number#', 42)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (52, N'#Type#', 42)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (53, N'#Type#', 37)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (54, N'#Type#', 38)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (55, N'#Type#', 39)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (56, N'#File Number#', 20)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (57, N'#Sector From#', 9)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (58, N'#Sector From#', 8)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (59, N'#Sector To#', 52)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (60, N'#Type#', 46)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (61, N'#Type#', 47)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (62, N'#Type#', 48)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (63, N'#File Number#', 21)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (64, N'#File Number#', 51)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (65, N'#File Number#', 50)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (66, N'#File Number#', 49)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (67, N'#File Number#', 44)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (68, N'#File Number#', 43)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (69, N'#File Number#', 41)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (70, N'#File Number#', 40)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (71, N'#File Number#', 36)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (72, N'#File Number#', 35)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (73, N'#File Number#', 34)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (74, N'#File Number#', 33)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (75, N'#File Number#', 31)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (76, N'#Type#', 45)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (77, N'#File Number#', 53)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (78, N'#File Number#', 54)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (79, N'#Reference Number#', 4)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (80, N'#Reference Number#', 16)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (81, N'#Sector From#', 12)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (82, N'#Sector To#', 12)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (83, N'#File Number#', 56)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (84, N'#File Number#', 57)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (85, N'#File Number#', 61)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (86, N'#File Number#', 62)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (87, N'#Reference Number#', 66)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (88, N'#Sector From#', 66)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (89, N'#Sector To#', 66)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (90, N'#Reference Number#', 22)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (91, N'#File Number#', 68)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (92, N'#Case Number#', 69)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (93, N'#CAN Number#', 69)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (94, N'#Name#', 70)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (95, N'#Name#', 71)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (96, N'#Name#', 72)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (97, N'#Name#', 73)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (98, N'#Name#', 74)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (99, N'#Name#', 75)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (100, N'#Name#', 76)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (101, N'#Name#', 77)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (102, N'#Name#', 78)
GO
--------------------------NOTIF_NOTIFICATION_CHANNEL_LKP---------------

INSERT [dbo].[NOTIF_NOTIFICATION_CHANNEL_LKP] ([ChannelId], [NameEn], [NameAr]) VALUES (1, N'Email', N'البريد الالكتروني')
GO
INSERT [dbo].[NOTIF_NOTIFICATION_CHANNEL_LKP] ([ChannelId], [NameEn], [NameAr]) VALUES (2, N'Mobile', N'Mobile')
GO
INSERT [dbo].[NOTIF_NOTIFICATION_CHANNEL_LKP] ([ChannelId], [NameEn], [NameAr]) VALUES (4, N'Browser', N'Browser Arbi')
GO

 ------------------------- NOTIF_NOTIFICATION_TEMPLATE--------------

INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'4a12fc1a-0dd0-4a08-8c0b-07a5ff4cad68', 12, 4, N'Send A Copy Review', N'Send A Copy Review', N'Send A Copy Review', N'Send A Copy Review', N'A Copy of #Entity#, with #Entity# Number #Refrence Number# has been send to #Sector To# from #Sector From# on #Created Date# Date by #Sender Name# for review.', N'A Copy of #Entity#, with #Entity# Number #Refrence Number# has been send to #Sector To# from #Sector From# on #Created Date# Date by #Sender Name# for review.', NULL, N'System Generated', CAST(N'2024-03-21T16:18:12.853' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'c4bb0d17-12bc-4078-98de-09d90d4380be', 69, 4, N'Add Sub Case', N'Add Sub Case', N'Add Sub Case', N'Add Sub Case', N'Sub case has been added, #Case Number# on Can Number #CAN Number#.', N'Sub case has been added, #Case Number# on Can Number #CAN Number#.', NULL, N'System Generated', CAST(N'2024-04-02T13:26:13.823' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'99830c71-4155-4204-846e-0b73eba6835f', 76, 4, N'Lms Literature Borrow Request For Approval', N'Lms Literature Borrow Request For Approval', N'Lms Literature Borrow Request For Approval Successfully', N'Lms Literature Borrow Request For Approval Successfully', N'LMS approve borrow request has been created for Literature, #Name# by #Sender Name# on #Created Date# for #Receiver Name#.', N'LMS approve borrow request has been created for Literature, #Name# by #Sender Name# on #Created Date# for #Receiver Name#.', NULL, N'System Generated', CAST(N'2024-04-04T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'17ea1229-3f34-4333-81b6-0c29d21c4afb', 25, 4, N'Review Draft', N'Review Draft', N'Review Draft', N'Review Draft', N'The draft document #Type# has been sent to #Receiver Name# to be reviewed by #Sender Name# on the date #Created Date#', N'مسودة المستند #Type# تم ارسالها الى #Receiver Name# للمراجعة من قبل #Sender Name# بتاريخ #Created Date#', NULL, N'System Generated', CAST(N'2024-03-24T16:44:12.697' AS DateTime), N'FatwaAdmin@uat.fatwa.v.kw', CAST(N'2024-03-27T12:48:14.983' AS DateTime), NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'414adc17-e20e-4889-9d11-0ee4ba52ed5e', 36, 4, N' Meeting Decision of HOS For Approval', N' Meeting Decision of HOS For Approval', N' Meeting Decision of HOS For Approval', N' Meeting Decision of HOS For Approval', N'#Entity# is pending for your approval Sent by #Sender Name# on #Created Date#', N'#Entity# is pending for your approval Sent by #Sender Name# on #Created Date#', NULL, N'fatwaadmin@gmail.com', CAST(N'2024-03-24T12:45:52.087' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'6d434354-9a15-4029-9418-1680331685db', 34, 4, N'Attendee Accept Meeting Invite', N'Attendee Accept Meeting Invite', N'Attendee Accept Meeting Invite', N'Attendee Accept Meeting Invite', N'Attendee #Sender Name# Has Accept Your 
#Entity# Invite, on #Created Date#', N'Attendee #Sender Name# Has Accept Your 
#Entity# Invite, on #Created Date#', NULL, N'fatwaadmin@gmail.com', CAST(N'2024-03-24T11:34:05.560' AS DateTime), N'fatwaadmin@gmail.com', CAST(N'2024-03-24T15:33:48.027' AS DateTime), NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'06baa003-3f62-4049-b7c3-16e27c424f7b', 47, 4, N'Soft Delete Legal Principle', N'Soft Delete Legal Principle', N'Soft Delete Legal Principle Successfully', N'Soft Delete Legal Principle Successfully', N'Following #Entity# soft deleted with principle type #Type# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', N'Following #Entity# soft deleted with principle type #Type# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', NULL, N'System Generated', CAST(N'2024-03-26T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'7fb88bb4-873c-472e-ad7d-189495c79ec4', 75, 4, N'Update Literature Borrow Reject Status', N'Update Literature Borrow Reject Status', N'Update Literature Borrow Reject Status Successfully', N'Update Literature Borrow Reject Status Successfully', N'LMS reject borrow extension request has been created for Literature, #Name# by #Sender Name# on #Created Date# for #Receiver Name#.', N'LMS reject borrow extension request has been created for Literature, #Name# by #Sender Name# on #Created Date# for #Receiver Name#.', NULL, N'System Generated', CAST(N'2024-04-04T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'82ecc9dd-a845-459c-8a6c-205b16248259', 37, 4, N'Save Legal Legislation', N'Save Legal Legislation', N'Save Legal Legislationt Successfully', N'Save Legal Legislation Successfully', N'Following #Entity# added with Legislation type #Type# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', N'Following #Entity# added with Legislation type #Type# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', NULL, N'System Generated', CAST(N'2024-03-21T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'2f7bb222-0507-4338-a983-250e230b197b', 44, 4, N'Edit MOM Of Meeting', N'Edit MOM Of Meeting', N'Edit MOM Of Meeting', N'Edit MOM Of Meeting', N'A Minutes Of Meeting is modified by #Sender Name#  number #File Number# on the date #Created Date#', N'رقم تم تعديل محضر الاجتماع  #File Number# من قبل #Sender Name# بتاريخ #Created Date#', NULL, N'fatwaadmin@gmail.com', CAST(N'2024-03-27T00:01:44.997' AS DateTime), N'fatwaadmin@gmail.com', CAST(N'2024-03-27T00:05:34.540' AS DateTime), NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'9e76ce4e-4cdd-49ad-acc0-2772f2344479', 3, 4, N'Share Document', N'Share Document', N'User Share Document', N'User Share Document', N'Following #Entity#, #Document Name# share with #Receiver Name# user by #Sender Name#.', NULL, NULL, N'System Generated', CAST(N'2024-03-13T15:20:19.030' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'c5c3d4eb-9d67-452a-9bb8-29dfa24e0c51', 73, 4, N'Update Lms Literature Retun', N'Update Lms Literature Retun', N'Update Lms Literature Retun Successfully', N'Update Lms Literature Retun Successfully', N'Lms Literature Retun has been updated for Literature, #Name# by #Sender Name# on #Created Date# for #Receiver Name#', N'Lms Literature Retun has been updated for Literature, #Name# by #Sender Name# on #Created Date# for #Receiver Name#', NULL, N'System Generated', CAST(N'2024-04-04T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'3f71e0f5-cc49-4808-82c5-2bc6782c8d69', 46, 4, N'Update Legal Principle', N'Update Legal Principle', N'Update Legal Principle Successfully', N'Update Legal Principle Successfully', N'Following #Entity# updated with principle type #Type# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', N'Following #Entity# updated with principle type #Type# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', NULL, N'System Generated', CAST(N'2024-03-26T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'dddb9793-dc89-4f24-ab77-311dd288b0b4', 2, 4, N'Assign to Lawyer', N'Assign to Lawyer', N'File/Case Assign to Lawyer', N'File/Case Assign to Lawyer', N'The #Entity#  #Reference Number# has been assigned to #Receiver Name# by #Sender Name# on the date #Created Date#', N'تم تعيين #Entity# رقم #Reference Number# الى #Receiver Name# من قبل #Sender Name# بتاريخ #Created Date#', NULL, N'System Generated', CAST(N'2024-03-12T19:06:43.297' AS DateTime), N'fatwaadmin@gmail.com', CAST(N'2024-03-26T16:59:11.157' AS DateTime), NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'fb212623-849f-4c6e-b9e3-31d932385332', 53, 4, N'Reminder To Complete Claim Statement', N'Reminder To Complete Claim Statement', N'Reminder To Complete Claim Statement', N'Reminder To Complete Claim Statement', N'No claim statement document is created against Case File #File Number#.', N'No claim statement document is created against Case File #File Number#.', NULL, N'System Generated', CAST(N'2024-03-27T12:57:05.770' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'bb8b5bdc-ff46-494b-b343-39892eea7391', 43, 4, N'Add MOM Of Meeting', N'Add MOM Of Meeting', N'Add MOM Of Meeting', N'Add MOM Of Meeting', N'A Minutes of meeting is saved number #File Number# by #Sender Name# on the date #Created Date#', N'رقم تم حفظ محضر الاجتماع   #File Number# من قبل #Sender Name# بتاريخ #Created Date#', NULL, N'fatwaadmin@gmail.com', CAST(N'2024-03-26T23:54:23.610' AS DateTime), N'fatwaadmin@gmail.com', CAST(N'2024-03-27T00:06:13.907' AS DateTime), NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'136c15f8-7f19-4b48-96f3-40124177dbc3', 70, 2, N'test', N'test', NULL, NULL, N'test', N'test', NULL, N'fatwaadmin@gmail.com', CAST(N'2024-04-15T12:06:12.197' AS DateTime), NULL, NULL, NULL, NULL, 0, 0)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'b53bc393-1908-4a35-a105-40c2e8514610', 15, 4, N'Assign Back To Hos', N'Assign Back To Hos', N'Assign Back To Hos Successfully', N'Assign Back To Hos Successfully', N'Following #Entity#, with #Entity# number #File Number# has been assigned back to #Receiver Name# on #Created Date# Date by lawyer #Sender Name#.', N'Following #Entity#, with #Entity# number #File Number# has been assigned back to #Receiver Name# on #Created Date# Date by lawyer #Sender Name#.', NULL, N'System Generated', CAST(N'2024-03-14T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'f7afb7aa-120d-4b3b-80da-439b18eb86a5', 1, 4, N'Request Created', N'Request Created', N'Request Created Successfully updated', N'Request Created Successfully', N'Following #Entity#, with request number #Request Number# has been assigned to #Receiver Name# on #Created Date# Date .', NULL, NULL, N'System Generated', CAST(N'2024-03-14T11:41:43.297' AS DateTime), N'fatwaadmin@gmail.com', CAST(N'2024-03-19T12:03:48.670' AS DateTime), NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'ce2fe61d-a923-42ab-81b0-454a296dafa9', 48, 4, N'Revoke Delete Legal Principle', N'Revoke Delete Legal Principle', N'Revoke Delete Legal Principle Successfully', N'Revoke Delete Legal Principle Successfully', N'Following #Entity# revoke with principle type #Type# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', N'Following #Entity# revoke with principle type #Type# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', NULL, N'System Generated', CAST(N'2024-03-26T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'68322bf2-4873-4b11-a5ea-4b095af547bd', 26, 4, N'Published Draft', N'Published Draft', N'Published Draft', N'Modify Draft', N'Following Darft of type #Type# has been Published by #Sender Name#', N'Following Darft of type #Type# has been Published by #Sender Name#', NULL, N'System Generated', CAST(N'2024-03-24T16:49:33.913' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'62a254e8-2124-41eb-9aaa-54254f823519', 20, 4, N'Add Judgement', N'Add Judgement', N'Judgement Added Successfully', N'Judgement Added Successfully', N'Judgement is add on case number #Case Number# against file number #File Number# by #Sender Name# on #Created Date# Date.', N'Judgement is add on case number #Case Number# against file number #File Number# by #Sender Name# on #Created Date# Date.', NULL, N'System Generated', CAST(N'2024-03-21T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'e6c58878-61b4-48b6-ac63-5514dfd1d540', 33, 4, N'Delete Attendee From Meeting', N' Delete Attendee From Meeting', N' Delete Attendee From Meeting', N' Delete Attendee From Meeting', N'You have been deleted from #Entity# by #Sender Name# on #Created Date#', N'You have been deleted from #Entity# by #Sender Name# on #Created Date#', NULL, N'fatwaadmin@gmail.com', CAST(N'2024-03-24T15:33:18.747' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'dc3d8171-1308-463e-9702-566edaa17283', 17, 4, N'Create Merge Request', N'Create Merge Request', N'Create Merge Request Successfully', N'Create Merge Request Successfully', N'Following #Entity#, with Case number #Case Number#  created from #File Number# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', NULL, NULL, N'System Generated', CAST(N'2024-03-20T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'0d310a15-b2da-429c-a49b-62361f56d056', 16, 4, N'Request For Meeting', N'Request For Meeting', N'Request For Meeting', N'Request For Meeting', N'A meeting requesthas been received from #Sender Name# on #Entity# number, #Reference Number#.', N'A meeting requesthas been received from #Sender Name# on #Entity# number, #Reference Number#.', NULL, N'System Generated', CAST(N'2024-03-28T13:13:42.707' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'8b53ad90-c091-4621-b2e8-672ed1ea45a5', 21, 4, N'Attendee Decision For Meeting', N'Attendee Decision For Meeting', N'Attendee Decision For Meeting', N'Attendee Decision For Meeting', N'Following #Entity#, is pending for your descision created on #Created Date# sent by#Sender Name#.', N'Following #Entity#, is pending for your descision created on #Created Date# sent by#Sender Name#.', NULL, N'System Generated', CAST(N'2024-03-21T11:41:43.297' AS DateTime), N'fatwaadmin@gmail.com', CAST(N'2024-03-24T11:14:23.503' AS DateTime), NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'1880c8ba-7c68-476a-8044-692a3b191422', 72, 4, N'Update Lms Literature Borrow Request', N'Update Lms Literature Borrow Request', N'Update Lms Literature Borrow Request Successfully', N'Update Lms Literature Borrow Request Successfully', N'LMS return borrow request has been created for Literature, #Name# by #Sender Name# on #Created Date# for #Receiver Name#.', N'LMS return borrow request has been created for Literature, #Name# by #Sender Name# on #Created Date# for #Receiver Name#.', NULL, N'System Generated', CAST(N'2024-04-04T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'50105901-79ae-4b77-9ce1-6bede9cf4fe3', 13, 4, N'Send A Copy Approved', N'Send A Copy Approved', N'Send A Copy Approved', N'Send A Copy Approved', N'A Copy of #Entity#, with #Entity# Number #Refrence Number# has been approved received from #Sector From# sector on #Created Date# Date send by #Sender Name#.', N'A Copy of #Entity#, with #Entity# Number #Refrence Number# has been approved received from #Sector From# sector on #Created Date# Date send by #Sender Name#.', NULL, N'System Generated', CAST(N'2024-03-21T16:29:50.487' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'30046dc7-f0b8-45aa-844a-6fc18bdc1736', 71, 4, N'Received Lms Literature Borrow Request', N'Received Lms Literature Borrow Request', N'Received Lms Literature Borrow Request Successfully', N'Received Lms Literature Borrow Request Successfully', N'Borrow request has been Received for Literature, #Name# by #Sender Name# on #Created Date# for #Receiver Name#', N'Borrow request has been Received for Literature, #Name# by #Sender Name# on #Created Date# for #Receiver Name#', NULL, N'System Generated', CAST(N'2024-04-04T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'c121e56d-857b-4b55-a8d5-7054ecb87791', 39, 4, N'Soft Delete Legal Legislation', N'Soft Delete Legal Legislation', N'Soft Delete Legal Legislation Successfully', N'Soft Delete Legal Legislation Successfully', N'Following #Entity# soft deleted with Legislation type #Type# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', N'Following #Entity# soft deleted with Legislation type #Type# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', NULL, N'System Generated', CAST(N'2024-03-21T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'c0327b70-9c1e-41b6-a5bb-70d8a0e30912', 8, 4, N'Request Sent For Execution', N'Request Sent For Execution', N'Request Sent For Execution', N'Request Sent For Execution', N'You received execution request from #Sector From# on case number #Case Number# by #Sender Name# on #Created Date# Date to review.', N'You received execution request from #Sector From# on case number #Case Number# by #Sender Name# on #Created Date# Date to review.', NULL, N'System Generated', CAST(N'2024-03-19T17:25:44.147' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'469bd75d-f10b-4457-a8db-7ac3d0692230', 32, 4, N'Case Data Pushed From RPA', N'Case Data Pushed From RPA', N'Case Data Pushed From RPA', N'Case Data Pushed From RPA', N'A new case registered, number #Case Number# and has been sent to #Receiver Name# on the date #Created Date#.', N'تم تسجيل قضية جديدة رقم #Case Number# وارسلت الى #Receiver Name# بتاريخ #Created Date#', NULL, N'fatwaadmin@gmail.com', CAST(N'2024-03-26T17:09:33.300' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'f3f2f8b3-f74a-4439-ba38-7ca105da0919', 9, 4, N'Approve Execution Request', N'Approve Execution Request', N'Approve Execution Request', N'Approve Execution Request', N'You received execution request from #Sector From# on case number #Case Number# by #Sender Name# on #Created Date# Date to review.', N'You received execution request from #Sector From# on case number #Case Number# by #Sender Name# on #Created Date# Date to review.', NULL, N'System Generated', CAST(N'2024-03-19T23:30:47.300' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'92cd1f23-5e2e-4587-9f98-7cd92fa7d364', 5, 4, N'Case Registered', N'Case Registered', N'Case Registered', N'Case Registered', N'Following Case, with Case Number #Case Number# has been registered against File Number #File Number# by #Sender Name# on #Created Date# Date for #Receiver Name#', N'Following Case, with Case Number #Case Number# has been registered against File Number #File Number# by #Sender Name# on #Created Date# Date for #Receiver Name#', NULL, N'System Generated', CAST(N'2024-03-24T20:01:18.220' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'263ad40d-ef17-450e-bec2-823351186473', 62, 4, N'Review Draft Reminder', N'Review Draft Reminder', N'Review Draft Reminder', N'Review Draft Reminder', N'Review Draft Reminder against Case #File Number#.', N'Review Draft Reminder against Case #File Number#.', NULL, N'System Generated', CAST(N'2024-03-31T12:24:12.587' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'9c16f5f0-5de3-4d69-bc09-8259f1673a2c', 4, 4, N'Legal Notification Response', N'Legal Notification Response', N'Legal Notification Response', N'Legal Notification Response', N'A Legal notification has been received from #Sender Name# on #Entity# number, #Reference Number#.', N'A Legal notification has been received from #Sender Name# on #Entity# number, #Reference Number#.', NULL, N'fatwaadmin@gmail.com', CAST(N'2024-03-27T13:20:20.313' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'9b539b8a-f726-492a-b425-9afd624efa9e', 45, 4, N'Save Legal Principle', N'Save Legal Principle', N'Save Legal Principle Successfully', N'Save Legal Principle Successfully', N'Following #Entity# added with principle type #Type# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', N'Following #Entity# added with principle type #Type# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', NULL, N'System Generated', CAST(N'2024-03-26T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'be2bd815-6156-47f5-93f9-9b8865a17cea', 51, 4, N'MOM Created Successfully', N'MOM Created Successfully', N'MOM Created Successfully', N'MOM Created Successfully', N'A Minutes Of Meeting is sent by #Sender Name# number #File Number# on the date #Created Date#', N'رقم تم ارسال محضر الاجتماع  #File Number# من قبل #Sender Name# بتاريخ #Created Date#', NULL, N'fatwaadmin@gmail.com', CAST(N'2024-03-27T00:08:28.997' AS DateTime), N'fatwaadmin@gmail.com', CAST(N'2024-03-27T01:32:52.290' AS DateTime), NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'aaa78021-5ac3-472e-b4f3-a61879311f58', 68, 4, N'Approve Execution Request', N'Approve Execution Request', N'Approve Execution Request', N'Approve Execution Request', N'#Sender Name# reject to accept the file, #File Number# send by #Receiver Name#.', N'#Sender Name# reject to accept the file, #File Number# send by #Receiver Name#.', NULL, N'System Generated', CAST(N'2024-04-02T11:53:56.423' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'f97ec53b-b595-4b2f-9c76-a90365038cb4', 55, 4, N'Communication Response Not Completed', N'Communication Response Not Completed', N'Communication Response Not Completed', N'Communication Response Not Completed', N'Communication Response Not Completed against Case #Entity#.', N'Communication Response Not Completed against Case #Entity#.', NULL, N'System Generated', CAST(N'2024-03-31T13:07:23.073' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'e7935d6e-4dea-40c4-88a8-ad84c872b727', 18, 4, N'Approve Merge Request', N'Approve Merge Request', N'Approve Merge Request', N'Approve Merge Request', N'Request to merge cases has been approved', N'Request to merge cases has been approved', NULL, N'System Generated', CAST(N'2024-04-01T12:42:23.683' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'e0f53f84-1ceb-4aea-8155-afa7bb26d2eb', 42, 4, N'Revoke Delete Legal Legislation', N'Revoke Delete Legal Legislation', N'Revoke Delete Legal Legislation Successfully', N'Revoke Delete Legal Legislation Successfully', N'Following #Entity# revoke with Legislation type #Type# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', N'Following #Entity# revoke with Legislation type #Type# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', NULL, N'System Generated', CAST(N'2024-03-25T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'98f49257-e3ba-4819-9d7d-b4447ef253b2', 70, 4, N'Create Lms Literature Borrow Detail', N'Create Lms Literature Borrow Detail', N'Create Lms Literature Borrow Detail Successfully', N'Create Lms Literature Borrow Detail Successfully', N'Borrow request has been created for Literature, #Name# by #Sender Name# on #Created Date# for #Receiver Name#', N'Borrow request has been created for Literature, #Name# by #Sender Name# on #Created Date# for #Receiver Name#', NULL, N'System Generated', CAST(N'2024-04-04T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'387a7c13-87ea-4604-b6ee-bb6fc4d28655', 7, 4, N'Create Execution Request', N'Create Execution Request', N'Create Execution Request Successfully', N'Create Execution Request Successfully', N'Execution Request is add on case number #Case Number# by #Sender Name# on #Created Date# Date.', N'Execution Request is add on case number #Case Number# by #Sender Name# on #Created Date# Date.', NULL, N'System Generated', CAST(N'2024-03-21T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'bb38d542-2349-4c70-8973-bc5a9252f8d6', 10, 4, N'Reject Execution Request', N'Reject Execution Request', N'Reject Execution Request', N'Reject Execution Request', N'Execution Request for the Case, with case number #Case Number# has been rejected by #Receiver Name# on #Created Date# Date Send by #Sender Name#.', N'Execution Request for the Case, with case number #Case Number# has been rejected by #Receiver Name# on #Created Date# Date Send by #Sender Name#.', NULL, N'System Generated', CAST(N'2024-03-19T23:30:47.303' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'07989300-2042-4256-b9ed-bc7741b9db1f', 31, 4, N'Send To MOJ', N'Send To MOJ', N'Send To MOJ', N'Send To MOJ', N'Document of Type #Type# has been sent to MOJ for case registration, By sender #Sender Name#.', N'Document of Type #Type# has been sent to MOJ for case registration, By sender #Sender Name#.', NULL, N'System Generated', CAST(N'2024-03-23T16:02:00.333' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'3cb64213-6de3-4ff6-a1c5-c88de421fa9d', 54, 4, N'Assign To MOJ Reminder', N'Assign To MOJ Reminder', N'Assign To MOJ Reminder', N'Assign To MOJ Reminder', N'Case File #File Number# by System not yet registered in MOJ.', N'Case File #File Number# by System not yet registered in MOJ.', NULL, N'System Generated', CAST(N'2024-03-27T15:56:43.550' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'04331e0b-167a-42b6-beb5-d209a32f5eb8', 77, 4, N'Lms Literature Borrow Request For Rejection', N'Lms Literature Borrow Request For Rejection', N'Lms Literature Borrow Request For Rejection Successfully', N'Lms Literature Borrow Request For Rejection Successfully', N'LMS reject borrow request has been created for Literature, #Name# by #Sender Name# on #Created Date# for #Receiver Name#.', N'LMS reject borrow request request has been created for Literature, #Name# by #Sender Name# on #Created Date# for #Receiver Name#.', NULL, N'System Generated', CAST(N'2024-04-04T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'498d49be-92b6-4b04-a941-d2ef0338e8e8', 22, 4, N'Add Judgment Execution', N'Add Judgment Execution', N'Add Judgment Execution Successfully', N'Add Judgment Execution', N'Following #Entity#, with Case number #Case Number# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', N'Following #Entity#, with Case number #Case Number# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', NULL, N'System Generated', CAST(N'2024-03-21T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'e32d8354-ae1f-49e9-b134-d94a2f30796e', 19, 4, N'Consultation Request Created', N'Consultation Request Created', N'Consultation Request Created Successfully', N'Consultation Request Created Successfully', N'Following #Entity#, type #Request Type# with request number #Request Number# has been assigned to #Receiver Name# on #Created Date# Date .', N'Following #Entity#, type #Request Type# with request number #Request Number# has been assigned to #Receiver Name# on #Created Date# Date .', NULL, N'System Generated', CAST(N'2024-03-20T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'e9188734-6cda-4fdf-9fe9-db21dff2d7b7', 30, 4, N'Add Contact', N'Add Contact', N'Add Contact', N'Add Contact', N'New contact is successfully added in the system.', N'تم إضافة جهة اتصال جديدة بنجاح', NULL, N'System Generated', CAST(N'2024-03-23T15:19:47.467' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'99be75a7-ff3d-4443-97c6-db24d99fe30b', 61, 4, N'WS NOTIF No Additional Information Or Claim Statement Document Created', N'WS NOTIF No Additional Information Or Claim Statement Document Created', N'WS NOTIF No Additional Information Or Claim Statement Document Created', N'WS NOTIF No Additional Information Or Claim Statement Document Created', N'No additional information or claim statement document is created yet Case #File Number#.', N'No additional information or claim statement document is created yet Case #File Number#.', NULL, N'System Generated', CAST(N'2024-03-31T11:35:53.350' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'754df145-e6ee-4eaf-999b-db27f2854879', 66, 4, N'Assign to Sector', N'Assign to Sector', N'Assign to Sector', N'Assign to Sector', N'Following #Entity#, with #Entity# Number #Reference Number# has been assigned to #Sector To# from #Sector From# on #Created Date# Date by #Sender Name#.', N'Following #Entity#, with #Entity# Number #Reference Number# has been assigned to #Sector To# from #Sector From# on #Created Date# Date by #Sender Name#.', NULL, N'System Generated', CAST(N'2024-03-31T21:12:24.713' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'0cf53bd7-0230-48fe-94df-dc00e2e07e33', 56, 4, N'WS NOTIF Defense Letter Prepration Reminder', N'WS NOTIF Defense Letter Prepration Reminder', N'WS NOTIF Defense Letter Prepration Reminder', N'WS NOTIF Defense Letter Prepration Reminder', N'No Defence Document is created against Case #File Number#.', N'No Defence Document is created against Case #File Number#.', NULL, N'System Generated', CAST(N'2024-03-31T10:17:53.910' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'f474efb0-2b32-4d71-80f1-dc9bf8503014', 23, 4, N'Modify Legislation Draft Document', N'Modify Legislation Draft Document', N'Modify Legislation Draft Document', N'Modify Legislation Draft Document', N'#Entity# of type #Type# has been #Status# ,send by #Sender Name#.', N'#Entity# of type #Type# has been #Status# ,send by #Sender Name#.', NULL, N'System Generated', CAST(N'2024-03-21T16:46:52.167' AS DateTime), N'fatwaadmin@gmail.com', CAST(N'2024-03-26T15:40:50.557' AS DateTime), NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'73e03bb0-2515-40f6-a49b-ddc853b8471d', 57, 4, N'Draft Modification Not Completed', N'Draft Modification Not Completed', N'Draft Modification Not Completed', N'Draft Modification Not Completed', N'Draft Modification Not Completed against Case #File Number#.', N'Draft Modification Not Completed against Case #File Number#.', NULL, N'System Generated', CAST(N'2024-03-31T11:07:03.650' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'0f31770a-a581-4205-a7de-e298f8ea0e88', 35, 4, N' Attendee Reject Meeting Invite', N' Attendee Reject Meeting Invite', N' Attendee Reject Meeting Invite', N' Attendee Reject Meeting Invite', N'Attendee #Sender Name# Has Reject Your 
#Entity# Invite, on #Created Date#', N'Attendee #Sender Name# Has Reject Your 
#Entity# Invite, on #Created Date#', NULL, N'fatwaadmin@gmail.com', CAST(N'2024-03-24T15:34:37.873' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'157f6af7-2150-457c-8c6d-e9357828aa7b', 49, 4, N'GE Reject Meeting Invite', N'GE Reject Meeting Invite', N'GE Reject Meeting Invite', N'GE Reject Meeting Invite', N'A Meeting is rejected by #Sender Name# number #File Number# on the date #Created Date#', N'A Meeting is rejected by #Sender Name# number #File Number# on the date #Created Date#', NULL, N'fatwaadmin@gmail.com', CAST(N'2024-03-26T23:35:53.957' AS DateTime), N'fatwaadmin@gmail.com', CAST(N'2024-03-26T23:40:44.690' AS DateTime), NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'00abe322-836c-4922-947a-eeca54631c21', 24, 4, N'Modify Draft', N'Modify Draft', N'Modify Draft', N'Modify Draft', N'Following Darft of type #Type# has been send to #Receiver Name# to Modify send by #Sender Name#', N'Following Darft of type #Type# has been send to #Receiver Name# to Modify send by #Sender Name#', NULL, N'System Generated', CAST(N'2024-03-24T16:46:13.890' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'1a773ba2-97c5-41e3-adce-eecb423af676', 14, 4, N'Transfer Of Sector', N'Transfer Of Sector', N'Transfer Of Sector Successfully', N'Transfer Of Sector Successfully', N'Following #Entity#, with #Entity# Number #Reference Number# has been transfer from #Sector From# to #Sector To# on #Created Date# Date by #Sender Name#.', N'Following #Entity#, with #Entity# Number #Reference Number# has been transfer from #Sector From# to #Sector To# on #Created Date# Date by #Sender Name#.', NULL, N'System Generated', CAST(N'2024-03-14T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'be414ea0-343f-4054-8907-f317d4d78356', 40, 4, N'Add Meeting Success', N'Add Meeting Success', N'Add Meeting Success', N'Add Meeting Success', N'#Entity# has been created successfully, on #Created Date#', N'#Entity# has been created successfully, on #Created Date#', NULL, N'fatwaadmin@gmail.com', CAST(N'2024-03-25T10:26:38.033' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'c52c3789-d9de-4e44-bd84-f6d379e5d3fb', 41, 4, N'Edit Meeting Success', N'Edit Meeting Success', N'Edit Meeting Success', N'Edit Meeting Success', N'A Meeting is modified number #Request Number# by #Sender Name# on the date #Created Date#', N'رقم تم تعديل مقابلة #Request Number# من قبل #Sender Name# بتاريخ #Created Date#', NULL, N'fatwaadmin@gmail.com', CAST(N'2024-03-26T23:49:20.657' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'7093b956-e88f-4358-a8ed-f716fba13c09', 52, 4, N'Assig/Unassigned Cases to Sector', N'Assig/Unassigned Cases to Sector', N'Assig/Unassigned Cases to Sector', N'Assig/Unassigned Cases to Sector', N'The Case has been assigned to #Sector To# by #Sender Name# on the date #Created Date#', N'تم تعيين ملفات القضايا لقطاع #Sector To# من قبل #Sender Name# بتاريخ #Created Date#', NULL, N'fatwaadmin@gmail.com', CAST(N'2024-03-26T19:30:20.720' AS DateTime), N'fatwaadmin@gmail.com', CAST(N'2024-03-26T19:30:50.547' AS DateTime), NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'db99e1d1-1790-4f05-a0e3-f8cacfddace0', 78, 4, N'Lms Literature Borrow Request For Extension', N'Lms Literature Borrow Request For Extension', N'Lms Literature Borrow Request For Extension Successfully', N'Lms Literature Borrow Request For Extension Successfully', N'LMS extension borrow request has been created for Literature, #Name# by #Sender Name# on #Created Date# for #Receiver Name#.', N'LMS extension borrow request has been created for Literature, #Name# by #Sender Name# on #Created Date# for #Receiver Name#.', NULL, N'System Generated', CAST(N'2024-04-04T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'6ec890af-c730-4f86-a295-fb9bb6668fcc', 74, 4, N'Update Literature Borrow Approval Status', N'Update Literature Borrow Approval Status', N'Update Literature Borrow Approval Status', N'Update Lms Literature Retun Successfully', N'LMS approve borrow extension request has been created for Literature, #Name# by #Sender Name# on #Created Date# for #Receiver Name#.', N'LMS approve borrow extension request has been created for Literature, #Name# by #Sender Name# on #Created Date# for #Receiver Name#.', NULL, N'System Generated', CAST(N'2024-04-04T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'a28b8463-73e4-496d-9a51-fc31f319b220', 38, 4, N'Update Legal Legislation', N'Update Legal Legislation', N'Update Legal Legislation Successfully', N'Update Legal Legislation Successfully', N'Following #Entity# updated with Legislation type #Type# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', N'Following #Entity# updated with Legislation type #Type# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', NULL, N'System Generated', CAST(N'2024-03-21T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1)
GO
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) VALUES (N'f07284c2-0a1e-4c39-936f-fefdf1daba49', 50, 4, N'GE Accept Meeting Invite', N'GE Accept Meeting Invite', N'GE Accept Meeting Invite', N'GE Accept Meeting Invite', N'A Meeting is Accepted by #Sender Name# number #File Number# on the date #Created Date#', N'A Meeting is Accepted by #Sender Name# number #File Number# on the date #Created Date#', NULL, N'fatwaadmin@gmail.com', CAST(N'2024-03-26T23:44:31.963' AS DateTime), N'fatwaadmin@gmail.com', CAST(N'2024-03-27T15:46:13.807' AS DateTime), NULL, NULL, 0, 1)
GO


  ---After delete and insert data then again run alter table Check constraints script 
  ALTER TABLE dbo.NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP 
CHECK CONSTRAINT FK__NOTIF_NOT__Event__5E7FE7D2;


ALTER TABLE dbo.NOTIF_NOTIFICATION_TEMPLATE 
CHECK CONSTRAINT FK__NOTIF_NOT__Event__625078B6;

ALTER TABLE dbo.NOTIF_NOTIFICATION_TEMPLATE 
CHECK CONSTRAINT FK__NOTIF_NOT__Chann__63449CEF;
  /*<History Author='Ihsaan Abbas' Date='24-04-2024'> Notifications End  </History>*/
  
  /*<History Author='Ihsaan Abbas' Date='28-04-2024'> System Configuration  Start  </History>*/
  GO
INSERT [dbo].[SYSTEM_CONFIGURATION] ([ConfigurationId], [GroupId], [Password_Period], [Session_Timeout_Period], [Wrong_Password_Attempts], [Invalid_Login_Attempts], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'89b7f868-189d-4736-af1c-01db2b9bbee8', N'071c4ba0-cc3f-43dc-b2cb-6fc2bea6d831', CAST(N'2023-12-08T11:15:57.000' AS DateTime), 10, 3, 3, N'superadmin@gmail.com', CAST(N'2023-11-10T11:17:08.813' AS DateTime), NULL, NULL, NULL, NULL, 0)
GO
INSERT [dbo].[SYSTEM_CONFIGURATION] ([ConfigurationId], [GroupId], [Password_Period], [Session_Timeout_Period], [Wrong_Password_Attempts], [Invalid_Login_Attempts], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'c7f82b95-436d-4331-90b5-f02ccf5145fb', N'071c4ba0-cc3f-43dc-b2cb-6fc2bea6d831', CAST(N'2023-12-09T11:17:27.000' AS DateTime), 10, 5, 3, N'superadmin@gmail.com', CAST(N'2023-11-10T11:18:41.487' AS DateTime), NULL, NULL, NULL, NULL, 0)
GO
 /*<History Author='Ihsaan Abbas' Date='28-04-2024'> System Configuration  End  </History>*/
 

   -- 29-04-2024 ----
   IF NOT Exists(SELECT 1 from LMS_LITERATURE_BORROW_APPROVAL_STATUS where DecisionId = 64)
	INSERT INTO [dbo].[LMS_LITERATURE_BORROW_APPROVAL_STATUS] ([DecisionId],[Name],[Name_Ar])
		VALUES (64, N'Pending For Extension', N'في انتظار التمديد')
GO 

IF NOT Exists(SELECT 1 from LMS_LITERATURE_BORROW_APPROVAL_STATUS where DecisionId = 128)
	INSERT INTO [dbo].[LMS_LITERATURE_BORROW_APPROVAL_STATUS] ([DecisionId],[Name],[Name_Ar])
		VALUES (128, N'Extended', N'ممتد')
GO 

IF NOT Exists(SELECT 1 from LMS_LITERATURE_BORROW_APPROVAL_STATUS where DecisionId = 256)
	INSERT INTO [dbo].[LMS_LITERATURE_BORROW_APPROVAL_STATUS] ([DecisionId],[Name],[Name_Ar])
		VALUES (265, N'Extension Rejected', N'التمديد مرفوض')
GO
    -- 29-04-2024 ----

------ 30-4-2024 -----------
insert into EmailConfiguration values(1,1,'','','','','smtp.gmail.com','587','','','System Generated',GETDATE(),null,null,null,null,0)
insert into EmailConfiguration values(2,2,'','','','','smtp.gmail.com','587','','','System Generated',GETDATE(),null,null,null,null,0)
insert into EmailConfiguration values(3,4,'','','Test Message From Tarasol Comsumer','','smtp.mail.yahoo.com','465','','','System Generated',GETDATE(),null,null,null,null,0)

update NOTIF_NOTIFICATION_EVENT set NameEn = 'Review Principle', NameAr = 'Review Principle' where EventId = 23

insert into NOTIF_NOTIFICATION_EVENT values (82,'Review Legislation','Review Legislation','System Generated',GETDATE(),null,null,null,null,0,1,null,null,null,1)

------ 30-4-2024 -----------



---- 01-05-2024 ----

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LEGAL_TEMPLATE]') AND type in (N'U'))
	Update [LEGAL_TEMPLATE] Set IsDefault = 1 where Template_Name = 'New Template'
	PRINT('Updated')
GO

---- 01-05-2024 ----

  ----------------04/may/2024
  INSERT INTO MEET_MEETING_STATUS VALUES (4096,'Approved By Vice HOS','Approved Bu Vice HOS')

INSERT INTO MEET_MEETING_STATUS VALUES (8192,'Rejected By Vice HOS','Approved Bu Vice HOS')

----------04/MAY/2024


--fatwa Script 06/05/2024
 INSERT INTO NOTIF_NOTIFICATION_TEMPLATE (TemplateId, EventId, ChannelId, NameEn, NameAr ,SubjectEn, SubjectAr, BodyEn, BodyAr,CreatedBy , CreatedDate , IsDeleted )
VALUES (NewID(),64, 4, 'Add Lms Literature' , 'Add Lms Literature', 'Lms Added Literature Successfully','Lms Added Literature Successfully',
'Literature has been Added, #Name# by #Sender Name# on #Created Date# for #Receiver Name#', 
'Literature has been Added, #Name# by #Sender Name# on #Created Date# for #Receiver Name#', 
'System Generated' ,'2024-04-04 11:41:43.297',0);


Insert into NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP values (103,'#Name#', 64) 


 INSERT INTO NOTIF_NOTIFICATION_TEMPLATE (TemplateId, EventId, ChannelId, NameEn, NameAr ,SubjectEn, SubjectAr, BodyEn, BodyAr,CreatedBy , CreatedDate , IsDeleted )
VALUES (NewID(),38, 4, 'Legislation Need to Modified' , 'Legislation Need to Modified', 'Legislation Need to Modified','Legislation Need to Modified',
'Legislation need to mofied with Legislation number #Legislation Number# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', 
'Legislation need to mofied with Legislation number #Legislation Number# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.', 
'System Generated' ,'2024-04-04 11:41:43.297',0);
--fatwa Script 06/05/2024


----------04/MAY/2024
insert into LITERATURE_DEWEY_NUMBER_PATTERN_TYPE ([PatternNameEn],[PatternNameAr]) values ('Literature Dewey Number Pattern',N'Literature Dewey Number Pattern')
insert into LITERATURE_DEWEY_NUMBER_PATTERN_TYPE ([PatternNameEn],[PatternNameAr]) values ('Others',N'Others')


INSERT [dbo].[UMS_ROLE] ([Id], [Name], [NormalizedName], [ConcurrencyStamp], [Description_En], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [Description_Ar], [NameAr]) VALUES (N'3a07eb32-db29-47a6-8252-900e4d10182c', N'VICE HOS', N'VICE HOS', N'9daaff5d-2ef0-4a9e-8fe5-40d9c18b77ac', N'This user is head of one sector and is responsible to review and assign the requests, sign the official documents being sent from that sector of FATWA to G2G. This user is also responsible to assign and approve drafts and files created by Lawyers. ', N'superadmin@gmail.com', CAST(N'2022-11-21T15:43:25.503' AS DateTime), N'fatwaadmin@gmail.com', CAST(N'2023-02-03T12:48:34.737' AS DateTime), NULL, NULL, 0, N'This user is head of one sector and is responsible to review and assign the requests, sign the official documents being sent from that sector of FATWA to G2G. This user is also responsible to assign and approve drafts and files created by Lawyers. ', N'VICE HOS')
GO
--Ammaar Naveed--13/05/2024--Found missing data for workflow module.
UPDATE UMS_CLAIM SET Module='Workflow_Management' WHERE Module='Workflow Management'
UPDATE UMS_CLAIM SET ModuleId=32 WHERE Module='Workflow_Management'

-------------------------14/05/2024---------Muhammad Ali---------
update CMS_COMS_NUM_PATTERN set IsDefault=1 where id='A650D14C-315A-4291-EB9C-08DBD5EDD3CD'
update CMS_COMS_NUM_PATTERN set IsDefault=1 where id='65050113-F26E-4CE9-914B-A835D350A772'
update CMS_COMS_NUM_PATTERN_HISTORY set IsDefault=1 where PatternId='A650D14C-315A-4291-EB9C-08DBD5EDD3CD'
update CMS_COMS_NUM_PATTERN_HISTORY set IsDefault=1 where PatternId='65050113-F26E-4CE9-914B-A835D350A772'
update CMS_COMS_NUM_PATTERN_HISTORY set IsDefault = 1 where PatternId='4D06D7AA-DB7B-425F-50E5-08DBD3912B3A'
----------------------END-----------------------

--Ammaar Naveed--19/05/2024--Sector Users CMS Permission
INSERT INTO UMS_CLAIM(Title_En, Title_Ar, Module, SubModule, ClaimType, ClaimValue, IsDeleted, ModuleId)
VALUES('Sector Users List',N'قائمة مستخدمي القطاع','Case_Management_System','Users_List','Permission','Permissions.CMS.SectorUsersList',0,2)
--Missing ModuleId
UPDATE UMS_CLAIM SET ModuleId=2 WHERE ClaimValue='Permissions.Submenu.CMS.CaseFile.UnassignedList'


------------------------------------------------------------------------------------------
----------------20/05/2024

update tTranslation 
set Value_En = 'Legislation Issue Date (Hijri)'    , Value_Ar =N'تاريخ الإصدار (هجري)'
where TranslationId= '5160'

update tTranslation 
set Value_En = 'Must Return Before'    , Value_Ar =N'يجب إعادته قبل'
where TranslationId= '9786'

----------------20/05/2024
---------------------------------------------------------------------------------------

--------23-05-2024---------
insert into module values ('Vendor Contract Management','Vendor Contract Management')
---------------------------------------------------------------------------------------

IF NOT Exists(SELECT 1 from CMS_TEMPLATE_PARAMETER where ParameterId = 35)
SET IDENTITY_INSERT CMS_TEMPLATE_PARAMETER ON
	INSERT INTO [dbo].CMS_TEMPLATE_PARAMETER (ParameterId,[Name],PKey,Mandatory,IsAutoPopulated,ModuleId,IsActive)
		VALUES (35, N'Chamber Number', N'CmsTempChamberNumber',0,1,5,1)
SET IDENTITY_INSERT CMS_TEMPLATE_PARAMETER OFF
GO
--Notification Event
insert into NOTIF_NOTIFICATION_EVENT values(84,	'Correspondence Forward To Lawyer','Correspondence Forward To Lawyer',	'System Generated',	'2024-05-26 11:41:43.297',NULL,NULL,NULL,NULL,0,1	,NULL,NULL,NULL,1)
insert into NOTIF_NOTIFICATION_EVENT values(85,	'Correspondence Send Back To HOS','Correspondence Send Back To HOS',	'System Generated',	'2024-05-26 11:41:43.297',NULL,NULL,NULL,NULL,0,1	,NULL,NULL,NULL,1)
insert into NOTIF_NOTIFICATION_EVENT values(86,	'Correspondence Forward To Sector','Correspondence Forward To Sector',	'System Generated',	'2024-05-26 11:41:43.297',NULL,NULL,NULL,NULL,0,1	,NULL,NULL,NULL,1)
insert into NOTIF_NOTIFICATION_EVENT values(87,	'Receive From Tarassol','Receive From Tarassol',	'System Generated',	'2024-05-26 11:41:43.297',NULL,NULL,NULL,NULL,0,1	,NULL,NULL,NULL,1)
-------
INSERT INTO UMS_USER_APP_VERSION ([Id],[ChannelId],[VersionCode])VALUES(1,1,1.0)
INSERT INTO UMS_USER_APP_VERSION ([Id],[ChannelId],[VersionCode])VALUES(2,2,1.1)
INSERT INTO UMS_USER_APP_VERSION ([Id],[ChannelId],[VersionCode])VALUES(3,3,1.2)
INSERT INTO UMS_USER_APP_VERSION ([Id],[ChannelId],[VersionCode])VALUES(4,4,1.10)
---------

 Insert into NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP values (112,'#Principle Number#', 45)
  Insert into NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP values (113,'#Principle Number#', 46)

   update NOTIF_NOTIFICATION_TEMPLATE 
   set BodyEn = 'Legal Principle Document need to mofied with Principle Number #Principle Number# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.' ,
       BodyAr= N'Legal Principle Document need to mofied with Principle Number #Principle Number# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.'
   where TemplateId ='3F71E0F5-CC49-4808-82C5-2BC6782C8D69'

    update NOTIF_NOTIFICATION_TEMPLATE 
   set BodyEn = 'Legal Principle Document has been created with Principle Number #Principle Number# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.' ,
       BodyAr= N'Legal Principle Document has been created with Principle Number #Principle Number# has been assigned to #Receiver Name# on #Created Date# Date by #Sender Name#.'
   where TemplateId ='9B539B8A-F726-492A-B425-9AFD624EFA9E'
---------


delete from CMS_GOVERNMENT_ENTITY_G2G_LKP where EntityId = 94
delete from CMS_GOVERNMENT_ENTITY_DEPARTMENT_G2G_LKP where EntityId = 94

-- vendor and contract

INSERT INTO UMS_CLAIM(Title_En, Module, SubModule, ClaimType, ClaimValue, Title_Ar, IsDeleted, ModuleId)
VALUES
('List Of Car Fuel Record (SubMenu)','Vendor_Contract','Car_Fuel_Record','Permission','Permissions.Submenu.VendorContract.CarFuelRecord.List','قائمة سجل وقود السيارة',0,4194304),
('Add Car Fuel Record (SubMenu)','Vendor_Contract','Car_Fuel_Record','Permission','Permissions.Submenu.VendorContract.CarFuelRecord.Add','إضافة سجل وقود السيارة',0,8192)

-------------------------23/6/24-------------------------
   INSERT INTO MODULE VALUES ('Service Request',N'طلب خدمة')
---------


------ 25 - june - 2024
update NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP set PlaceHolderName =N'#Case Number#' where EventId=56
update NOTIF_NOTIFICATION_TEMPLATE set BodyEn='No defense or opinion letter has been issued for Case #Case Number#',BodyAr =N'لم يتم إصدار خطاب دفاع أو رأي لقضية #Case Number#' where EventId=56
-- check the greatest number as I already run this query in fatwa_db_dev and ft_fatwa
IF NOT EXISTS (
    SELECT 1 
    FROM [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] 
    WHERE PlaceHolderId = 106
)
BEGIN
    INSERT INTO [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId])
    VALUES ('106', N'#Duration#', '58');
END



--Ammaar Naveed--23/06/2024--EP Designation Lookup recreated
INSERT INTO EP_DESIGNATION
VALUES
( 'Head of Sector', N'رئيس القطاع', 'fatwaadmin@gmail.com', GETUTCDATE(), NULL, NULL, NULL, NULL, 0, 1),
( 'Vice HOS', N'نائب رئيس القطاع', 'fatwaadmin@gmail.com', GETUTCDATE(), NULL, NULL, NULL, NULL, 0, 1),
( 'Lawyer', N'محامي', 'fatwaadmin@gmail.com', GETUTCDATE(), NULL, NULL, NULL, NULL, 0, 1),
( 'Supervisor', N'مشرف', 'fatwaadmin@gmail.com', GETUTCDATE(), NULL, NULL, NULL, NULL, 0, 1),
( 'Human Resource Manager', N'مدير الموارد البشرية', 'fatwaadmin@gmail.com', GETUTCDATE(), NULL, NULL, NULL, NULL, 0, 0),
( 'Messenger', N'مراسل', 'fatwaadmin@gmail.com', GETUTCDATE(), NULL, NULL, NULL, NULL, 0, 0),
( 'Store Keeper', N'أمين المخزن', 'fatwaadmin@gmail.com', GETUTCDATE(), NULL, NULL, NULL, NULL, 0, 0),
( 'Custodian', N'حارس', 'fatwaadmin@gmail.com', GETUTCDATE(), NULL, NULL, NULL, NULL, 0, 0),
( 'Procurement', N'المشتريات', 'fatwaadmin@gmail.com', GETUTCDATE(), NULL, NULL, NULL, NULL, 0, 0),
( 'Test Designation', N'Test Designation', 'fatwaadmin@gmail.com', GETUTCDATE(), NULL, NULL, NULL, NULL, 0, 0);

--Ammaar Naveed--24/06/2024--Employees List replication to FATWA Admin Portal
INSERT INTO UMS_CLAIM(Title_En, Title_Ar, Module, SubModule, ClaimType, ClaimValue, IsDeleted, ModuleId)
VALUES(
'Employees List', N'قائمة الموظفين', 'User_Management','Assign_User_Role','Permission','Admin.Permissions.Submenu.UMS.EmployeesList',0,8192
);

---------------Muhammad Ali----------26 June 2024 ------------ Morning Evening Data insertion-----------------

INSERT INTO [dbo].[CMS_CHAMBER_SHIFT_G2G_LKP] ([NameEn], [NameAr], [IsActive], [CreatedBy], [CreatedDate], [IsDeleted]) 
VALUES ('Morning', N'صباح', 1, 'System', GETDATE(), 0);

INSERT INTO [dbo].[CMS_CHAMBER_SHIFT_G2G_LKP] ([NameEn], [NameAr], [IsActive], [CreatedBy], [CreatedDate], [IsDeleted]) 
VALUES ('Evening', N'مساء', 1, 'System', GETDATE(), 0);

--------------------End---------------
-- vendor and contract
IF ((SELECT COUNT(*) FROM [UMS_CLAIM] WHERE Title_En = 'List Of Car Fuel Record (SubMenu') = 0)
BEGIN
INSERT INTO UMS_CLAIM(Title_En, Module, SubModule, ClaimType, ClaimValue, Title_Ar, IsDeleted, ModuleId)
VALUES
('List Of Car Fuel Record (SubMenu)','Vendor_Contract','Car_Fuel_Record','Permission','Permissions.Submenu.VendorContract.CarFuelRecord.List','قائمة سجل وقود السيارة',0,4194304)
END
ELSE
BEGIN
 PRINT 'Record already exists'
 END

 IF ((SELECT COUNT(*) FROM [UMS_CLAIM] WHERE Title_En = 'Add Car Fuel Record (SubMenu)') = 0)
BEGIN
    INSERT INTO UMS_CLAIM
    VALUES ('Add Car Fuel Record (SubMenu)','Vendor_Contract','Car_Fuel_Record','Permission','Permissions.Submenu.VendorContract.CarFuelRecord.Add','إضافة سجل وقود السيارة',0,8192)
END
ELSE
BEGIN
    PRINT 'Record already exists'
END


IF ((SELECT COUNT(*) FROM [UMS_CLAIM] WHERE Title_En = 'List Car Plate Number (SubMenu)') = 0)
BEGIN
    INSERT INTO UMS_CLAIM
    VALUES ('List Car Plate Number (SubMenu)','Vendor_Contract','Car_Plate_Number','Permission','Permissions.Submenu.VendorContract.CarPlateNumber.List',N'قائمة رقم لوحة السيارة',0,4194304)
END
ELSE
BEGIN
    PRINT 'Record already exists'
END
IF ((SELECT COUNT(*) FROM [UMS_CLAIM] WHERE Title_En = 'List Car Mileage Record (SubMenu)') = 0)
BEGIN
    insert into UMS_CLAIM values ('List Car Mileage Record (SubMenu)','Vendor_Contract','Car_Mileage_Record','Permission','Permissions.Submenu.VendorContract.CarMileageRecord.List',N'قائمة سجل الأميال السيارة',0,4194304)
END
ELSE
BEGIN
    PRINT 'Record already exists'
END

--Ammaar Naveed--02/07/2024--FATWA Admin Group UMS permissions
INSERT INTO UMS_GROUP_CLAIMS (GroupId, ClaimType, ClaimValue)
VALUES
    ('071C4BA0-CC3F-43DC-B2CB-6FC2BEA6D831', 'Permission', 'Admin.Permissions.Submenu.UMS.EmployeesList'),
    ('071C4BA0-CC3F-43DC-B2CB-6FC2BEA6D831', 'Permission', 'Admin.Permissions.Submenu.UMS.Claims'),
    ('071C4BA0-CC3F-43DC-B2CB-6FC2BEA6D831', 'Permission', 'Admin.Permissions.Submenu.Websystem'),
    ('071C4BA0-CC3F-43DC-B2CB-6FC2BEA6D831', 'Permission', 'Admin.Permissions.Submenu.UMS.Roles');


--------------------------------------------3/7/24

SET IDENTITY_INSERT [CMS_OPERATING_SECTOR_TYPE_G2G_LKP] ON
INSERT INTO CMS_OPERATING_SECTOR_TYPE_G2G_LKP 
(ID,Code, Name_En, Name_Ar, IsActive, DepartmentId, ParentId,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,DeletedBy,DeletedDate,IsDeleted,ModuleId)
VALUES (51, 'AAD','Administrative Affairs Department','Administrative Affairs Department',1,2,20,'fatwaadmin@gmail.com',GETDATE(),NULL,NULL,NULL,NULL,0,NULL);

INSERT INTO CMS_OPERATING_SECTOR_TYPE_G2G_LKP 
(ID,Code, Name_En, Name_Ar, IsActive, DepartmentId, ParentId,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,DeletedBy,DeletedDate,IsDeleted,ModuleId)
VALUES (52,'EAD','Employment Affairs Department','Employment Affairs Department',1,2,51,'fatwaadmin@gmail.com',GETDATE(),NULL,NULL,NULL,NULL,0,NULL);

INSERT INTO CMS_OPERATING_SECTOR_TYPE_G2G_LKP 
(ID,Code, Name_En, Name_Ar, IsActive, DepartmentId, ParentId,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,DeletedBy,DeletedDate,IsDeleted,ModuleId)
VALUES (53,'ASD','Administrative Services Department','Employment Affairs Department',1,2,51,'fatwaadmin@gmail.com',GETDATE(),NULL,NULL,NULL,NULL,0,NULL);

INSERT INTO CMS_OPERATING_SECTOR_TYPE_G2G_LKP 
(ID, Code, Name_En, Name_Ar, IsActive, DepartmentId, ParentId,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,DeletedBy,DeletedDate,IsDeleted,ModuleId)
VALUES (54,'LDD','Leave And Duty Department','Leave And Duty Department',1,2,51,'fatwaadmin@gmail.com',GETDATE(),NULL,NULL,NULL,NULL,0,NULL);
SET IDENTITY_INSERT [CMS_OPERATING_SECTOR_TYPE_G2G_LKP] OFF

-------------------------------------end
--------------------Automation Monitoring Interface---------------

SET IDENTITY_INSERT [dbo].[QUEUE_ITEM_STATUS] ON 
GO
INSERT [dbo].[QUEUE_ITEM_STATUS] ([Id], [StatusNameEn], [StatusAr], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (1, N'Pending', N'Pending', N'fatwat.admin', CAST(N'2024-02-06T00:00:00.000' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[QUEUE_ITEM_STATUS] ([Id], [StatusNameEn], [StatusAr], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (2,N'Completed', N'Completed', N'fatea.admin', CAST(N'2024-02-06T00:00:00.000' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[QUEUE_ITEM_STATUS] ([Id], [StatusNameEn], [StatusAr], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (3, N'Locked', N'Locked', N'locked', CAST(N'2024-02-06T00:00:00.000' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[QUEUE_ITEM_STATUS] ( [Id],[StatusNameEn], [StatusAr], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (4, N'Exception', N'Exeception', N'Fatwa.admin', CAST(N'2024-02-06T00:00:00.000' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[QUEUE_ITEM_STATUS] ([Id], [StatusNameEn], [StatusAr], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (5, N'Reattampt Exception', N'Reattampt Exception', N'fatwa.admin', CAST(N'2024-07-04T00:00:00.000' AS DateTime), N'fatwa', CAST(N'2024-07-04T00:00:00.000' AS DateTime))
GO
INSERT [dbo].[QUEUE_ITEM_STATUS] ([Id], [StatusNameEn], [StatusAr], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (6, N'Running', N'test', N'string', CAST(N'2024-07-04T00:00:00.000' AS DateTime), N'f', CAST(N'2024-07-04T00:00:00.000' AS DateTime))
GO
INSERT [dbo].[QUEUE_ITEM_STATUS] ([Id], [StatusNameEn], [StatusAr], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (7, N'Stop', N'Stop', N'fatwa.admin', CAST(N'2024-07-02T00:00:00.000' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[QUEUE_ITEM_STATUS] ([Id], [StatusNameEn], [StatusAr], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (8, N'Reinstated', N'Reinstated', N'fatwa.admin', CAST(N'2024-07-02T00:00:00.000' AS DateTime), NULL, NULL)
GO
GO
INSERT [dbo].[QUEUE_ITEM_STATUS] ([Id], [StatusNameEn], [StatusAr], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (9, N'Closed', N'Closed', N'fatwa.admin', CAST(N'2024-07-02T00:00:00.000' AS DateTime), NULL, NULL)
GO
GO
INSERT [dbo].[QUEUE_ITEM_STATUS] ([Id], [StatusNameEn], [StatusAr], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn]) VALUES (10, N'Disabled', N'Disabled', N'fatwa.admin', CAST(N'2024-07-02T00:00:00.000' AS DateTime), NULL, NULL)
GO
GO
SET IDENTITY_INSERT [dbo].[QUEUE_ITEM_STATUS] OFF
GO

---Automation Monitoring Interface----------
IF((SELECT COUNT(*) FROM UMS_CLAIM WHERE ClaimValue = 'Permissions.Menu.AutoMonInterface') = 0)
BEGIN
	INSERT INTO UMS_CLAIM VALUES ('Automation Monitoring Interface Menu','Automation_Monitoring_Interface','Menu', 'Permission', 'Permissions.Menu.AutoMonInterface','Automation Monitoring Interface Menu',0,0)
END
GO

IF((SELECT COUNT(*) FROM UMS_CLAIM WHERE ClaimValue = 'Permissions.AutoMonInterface.Process.List') = 0)
BEGIN
	INSERT INTO UMS_CLAIM VALUES ('Process List','Automation_Monitoring_Interface','AutoMonInterface', 'Permission', 'Permissions.AutoMonInterface.Process.List','Process List',0,0)
END

IF((SELECT COUNT(*) FROM UMS_CLAIM WHERE ClaimValue = 'Permissions.AutoMonInterface.Queue.List') = 0)
BEGIN
	INSERT INTO UMS_CLAIM VALUES ('Queue List','Automation_Monitoring_Interface','AutoMonInterface', 'Permission', 'Permissions.AutoMonInterface.Queue.List','Queue List',0,0)
END

INSERT INTO UMS_GROUP_CLAIMS VALUES ('071C4BA0-CC3F-43DC-B2CB-6FC2BEA6D831', 'Permission', 'Permissions.Menu.AutoMonInterface')
INSERT INTO UMS_GROUP_CLAIMS VALUES ('071C4BA0-CC3F-43DC-B2CB-6FC2BEA6D831', 'Permission', 'Permissions.AutoMonInterface.Queue.List')
INSERT INTO UMS_GROUP_CLAIMS VALUES ('071C4BA0-CC3F-43DC-B2CB-6FC2BEA6D831', 'Permission', 'Permissions.AutoMonInterface.Process.List')
---end---------
-------------------------------------end

SET IDENTITY_INSERT  MODULE ON
INSERT INTO MODULE (ModuleId , ModuleNameEn , ModuleNameAr) VALUES (19,'Organizing Committee',N'Organizing Committee')
SET IDENTITY_INSERT  MODULE OFF

-------------------14/07/2024
SET IDENTITY_INSERT  MODULE ON
INSERT INTO MODULE (ModuleId , ModuleNameEn , ModuleNameAr) VALUES (20,'Leave And Attendance',N'الاجازة والحضور')
SET IDENTITY_INSERT  MODULE OFF
----------------------------------------------
-----------------------------
----------------------------Committee Notification Event
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId]) VALUES (95, N'Committee Added', N'Committee Added', N'System Generated', CAST(N'2024-03-25T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL)
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (129, N'#Reference Number#', 95)
----------
INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP VALUES (117,'#Correspondence Number#',87)
INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP VALUES (118,'#Correspondence Number#',86)
INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP VALUES (119,'#Correspondence Number#',84)
INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP VALUES (120,'#Correspondence Number#',85)
INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP VALUES (121,'#GE Name#',87)



INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP VALUES (123,'#Draft Number#',25)
INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP VALUES (124,'#Draft Number#',26)
INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP VALUES (125,'#Draft Number#',24)

INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP VALUES (126,'#Draft Name#',25)
INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP VALUES (127,'#Draft Name#',26)
INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP VALUES (128,'#Draft Name#',24)


-----------------------------------22/7/24
IF NOT Exists(SELECT 1 from UMS_ROLE where name = 'Manager')
	INSERT INTO [dbo].[UMS_ROLE] ([Id],[Name],[NormalizedName],[ConcurrencyStamp],[Description_En],[CreatedBy],[CreatedDate],[ModifiedBy],[ModifiedDate],[DeletedBy],[DeletedDate],[IsDeleted],[Description_Ar],[NameAr])
		VALUES ('D047A9CA-29E3-4987-BC4B-A0C911086B63','Manager','MANAGER','FDA6FD59-6DF0-4AEB-8662-E323EAE7266A','Manager','fatwaadmin@gmail.com',GETDATE(),NULL,NULL,NULL,NULL,0,N'مدير',N'مدير')
GO
------------------------------------------------------------------------------------------------------------------




-------------

INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId]) VALUES (96, N'Add Committee Member', N'Add Committee Member', N'System Generated', CAST(N'2024-03-25T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL)
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (130, N'#Reference Number#', 96)

-------------
IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Permissions.Menu.OrganizingCommittee')
	INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted],[ModuleId])
		VALUES ('Organizing Committee Menu','Organizing_Committee', 'Menu', 'Permission', 'Permissions.Menu.OrganizingCommittee','Organizing Committee Menu',0,16777216)
GO 
IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Permissions.Submenu.OrganizingCommittee.CommitteeList')
	INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted],[ModuleId])
		VALUES ('Committee List','Organizing_Committee', 'SubMenu', 'Permission', 'Permissions.Submenu.OrganizingCommittee.CommitteeList','Committee List',0,16777216)
GO 

-------------------

INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId]) VALUES (103, N'Dissolve Committee', N'Dissolve Committee', N'System Generated', CAST(N'2024-03-25T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL)
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (132, N'#Reference Number#', 103)
--------------------------------


INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId]) VALUES (104, N'Update Member Access', N'Update Member Access', N'System Generated', CAST(N'2024-03-25T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL)
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (133, N'#Reference Number#', 104)

----------
----------25-07-2024

update NOTIF_NOTIFICATION_TEMPLATE set EventId=59, BodyEn='Reminder: No additional information or claim statement document has been created for Case #File Number# yet', BodyAr='Reminder: No additional information or claim statement document has been created for Case #File Number# yet.' where EventId=61
update NOTIF_NOTIFICATION_EVENT set NameEn='Request For Additional Info',NameAr='Request For Additional Info' where EventId=59
update NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP set EventId=59 where EventId=61


insert into NOTIF_NOTIFICATION_TEMPLATE values (newid(),61,4,'Additional Information Reminder','Additional Information Reminder','Reminder for GE related to Additional Information','Reminder for GE related to Additional Information','No response to the legal notification has been received yet','No response to the legal notification has been received yet',NULL,'System Generated',GETDATE(),NULL,NULL,NULL,NULL,0,1)
------
update COMM_COMMUNICATION_TYPE set NameAr='Initial Judgement',NameEn='Initial Judgement' where CommunicationTypeId=131072
--------------
insert into COMM_COMMUNICATION_TYPE ([CommunicationTypeId],[NameAr],[NameEn],[CreatedBy],[CreatedDate],[ModifiedBy],[ModifiedDate],[DeletedBy],[DeletedDate],[IsDeleted],[IsActive])
values(1073741829,'Final Judgement','Final Judgement','fatwaadmin@gmail.com',GETDATE(),null,null,null,null,0,1)
----------
insert into CMS_DRAFT_ACTION_ENUM ([ActionId],[NameEn],[NameAr]) values (8,'Approved',N'?????')
insert into CMS_DRAFT_ACTION_ENUM ([ActionId],[NameEn],[NameAr]) values (16,'Rejected',N'???')
insert into CMS_DRAFT_ACTION_ENUM ([ActionId],[NameEn],[NameAr]) values (32,'EditedAndSubmitted',N'EditedAndSubmitted')

IF((SELECT COUNT(*) FROM CMS_CASE_FILE_EVENT_G2G_LKP WHERE Id = '65536') <= 0)
SET IDENTITY_INSERT CMS_CASE_FILE_EVENT_G2G_LKP ON
INSERT INTO CMS_CASE_FILE_EVENT_G2G_LKP (Id,Name_En,Name_Ar)VALUES(65536,'AcceptedByLawyer','AcceptedByLawyer')
SET IDENTITY_INSERT CMS_CASE_FILE_EVENT_G2G_LKP OFF
GO

--------------------------------------31/7/24-----------------------------

IF((SELECT COUNT(*) FROM UMS_CLAIM WHERE ClaimValue = 'Permissions.INV.Inventory.ServiceRequest.Create') > 0)
Update UMS_CLAIM SET Module='Service_Request',SubModule = 'Service_Request', ClaimValue='Permissions.SR.ServiceRequest.Create', ModuleId ='33554432' 
WHERE ClaimValue = 'Permissions.INV.Inventory.ServiceRequest.Create'

ELSE IF((SELECT COUNT(*) FROM UMS_CLAIM WHERE ClaimValue = 'Permissions.SR.ServiceRequest.Create') = 0)
INSERT INTO UMS_CLAIM VALUES ('Create Service Request', 'Service_Request', 'Service_Request', 'Permission','Permissions.SR.ServiceRequest.Create',N'Create Service Request', 0,'33554432')

ELSE
PRINT('Record Already Updated')
---------
IF((SELECT COUNT(*) FROM UMS_CLAIM WHERE ClaimValue = 'Permissions.INV.Inventory.ServiceRequest.Detail') > 0)
Update UMS_CLAIM SET Module='Service_Request',SubModule = 'Service_Request', ClaimValue='Permissions.SR.ServiceRequest.Detail', ModuleId ='33554432' 
WHERE ClaimValue = 'Permissions.INV.Inventory.ServiceRequest.Detail'

ELSE IF((SELECT COUNT(*) FROM UMS_CLAIM WHERE ClaimValue = 'Permissions.SR.ServiceRequest.Detail') = 0)
INSERT INTO UMS_CLAIM VALUES ('Service Request Detail', 'Service_Request', 'Service_Request', 'Permission','Permissions.SR.ServiceRequest.Detail',N'Service Request Detail', 0,'33554432')

ELSE
PRINT('Record Already Updated')
----------
IF((SELECT COUNT(*) FROM UMS_CLAIM WHERE ClaimValue = 'Permissions.INV.Inventory.ServiceRequest.Edit') > 0)
Update UMS_CLAIM SET Module='Service_Request',SubModule = 'Service_Request', ClaimValue='Permissions.SR.ServiceRequest.Edit', ModuleId ='33554432' 
WHERE ClaimValue = 'Permissions.INV.Inventory.ServiceRequest.Edit'

ELSE IF((SELECT COUNT(*) FROM UMS_CLAIM WHERE ClaimValue = 'Permissions.SR.ServiceRequest.Edit') = 0)
INSERT INTO UMS_CLAIM VALUES ('Edit Service Request', 'Service_Request', 'Service_Request', 'Permission','Permissions.SR.ServiceRequest.Edit',N'Edit Service Request', 0,'33554432')

ELSE
PRINT('Record Already Updated')
-----------------------
IF((SELECT COUNT(*) FROM UMS_CLAIM WHERE ClaimValue = 'Permissions.Submenu.INV.Inventory.ServiceRequests.List') > 0)
Update UMS_CLAIM SET Module='Service_Request',SubModule = 'Service_Request', ClaimValue='Permissions.Submenu.SR.ServiceRequest.List', ModuleId ='33554432' 
WHERE ClaimValue = 'Permissions.Submenu.INV.Inventory.ServiceRequests.List'

ELSE IF((SELECT COUNT(*) FROM UMS_CLAIM WHERE ClaimValue = 'Permissions.Submenu.SR.ServiceRequest.List') = 0)
INSERT INTO UMS_CLAIM VALUES ('Service Requests List', 'Service_Request', 'Service_Request', 'Permission','Permissions.Submenu.SR.ServiceRequest.List',N'Service Requests List', 0,'33554432')

ELSE
PRINT('Record Already Updated')
------------------------------------

IF((SELECT COUNT(*) FROM UMS_CLAIM WHERE ClaimValue = 'Permissions.INV.Inventory.ServiceRequestItems.Issue') > 0)
Update UMS_CLAIM SET Module='Service_Request',SubModule = 'Service_Request', ClaimValue='Permissions.SR.ServiceRequest.ServiceRequestItems.Issue', ModuleId ='33554432' 
WHERE ClaimValue = 'Permissions.INV.Inventory.ServiceRequestItems.Issue'

ELSE IF((SELECT COUNT(*) FROM UMS_CLAIM WHERE ClaimValue = 'Permissions.SR.ServiceRequest.ServiceRequestItems.Issue') = 0)
INSERT INTO UMS_CLAIM VALUES ('Issue Service Request Items', 'Service_Request', 'Service_Request', 'Permission','Permissions.SR.ServiceRequest.ServiceRequestItems.Issue',N'Issue Service Request Items', 0,'33554432')

ELSE
PRINT('Record Already Updated')

-----------------
IF((SELECT COUNT(*) FROM UMS_CLAIM WHERE ClaimValue = 'Permissions.Menu.SR.ServiceRequest') = 0)
INSERT INTO UMS_CLAIM VALUES ('Service Request (Menu)', 'Service_Request', 'Service_Request', 'Permission','Permissions.Menu.SR.ServiceRequest',N'قائمة طلبات الخدمة', 0,'33554432')
ELSE PRINT ('Record Already Exist')
----------------
IF((SELECT COUNT(*) FROM UMS_CLAIM WHERE ClaimValue = 'Permissions.Dashboard.SR.View') = 0)
INSERT INTO UMS_CLAIM VALUES ('Service Request', 'FATWA_Dashboard', 'Service_Request', 'Permission','Permissions.Dashboard.SR.View',N'قائمة طلبات', 0,'33554432')
ELSE PRINT ('Record Already Exist')
---------------------------END-------------------------------------------------------------------
------------ 29 july 024
GO
SET IDENTITY_INSERT [dbo].[EP_WORKINGHOURS_LKP] ON 
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[EP_WORKINGHOURS_LKP] WHERE [Id] = 1)
BEGIN
    INSERT [dbo].[EP_WORKINGHOURS_LKP] ([Id], [NameEn], [NameAr], [StartTime], [EndTime])
    VALUES (1, N'Half Time', N'Half Time', CAST(N'07:30:00' AS Time), CAST(N'13:30:00' AS Time))
END
GO	

IF NOT EXISTS (SELECT 1 FROM [dbo].[EP_WORKINGHOURS_LKP] WHERE [Id] = 2)
BEGIN
    INSERT [dbo].[EP_WORKINGHOURS_LKP] ([Id], [NameEn], [NameAr], [StartTime], [EndTime])
    VALUES (2, N'Full Time', N'Full Time', CAST(N'07:30:00' AS Time), CAST(N'15:30:00' AS Time))
END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[EP_WORKINGHOURS_LKP] WHERE [Id] = 3)
BEGIN
    INSERT [dbo].[EP_WORKINGHOURS_LKP] ([Id], [NameEn], [NameAr], [StartTime], [EndTime])
    VALUES (3, N'Special Hour', N'Speical Hour', CAST(N'07:30:00' AS Time), CAST(N'13:30:00' AS Time))
END
GO

SET IDENTITY_INSERT [dbo].[EP_WORKINGHOURS_LKP] OFF
GO
update CMS_DRAFT_ACTION_ENUM set NameEn='CreatedAndDraft' where ActionId=1
update CMS_DRAFT_ACTION_ENUM set NameEn='EditedAndDraft' where ActionId=2
-------------
Insert into CMS_DRAFT_ACTION_ENUM ([ActionId],[NameEn],[NameAr]) Values(64,'Published',N'Published')
--------

-----------AutoMonInterface------
IF((SELECT COUNT(*) FROM UMS_CLAIM WHERE ClaimValue = 'Permission.AutoMonInterface.CaseData.Extraction') = 0)
BEGIN
	INSERT INTO UMS_CLAIM VALUES ('Case Data Extraction','Automation_Monitoring_Interface','AutoMonInterface', 'Permission', 'Permissions.AutoMonInterface.CaseData.Extraction','Case Data Extraction',0,0)
END

INSERT INTO UMS_GROUP_CLAIMS VALUES ('071C4BA0-CC3F-43DC-B2CB-6FC2BEA6D831', 'Permission', 'Permissions.AutoMonInterface.CaseData.Extraction')

---------------------------8/4/24------------

GO
SET IDENTITY_INSERT [dbo].[EP_WEEKDAYS_SETTINGS_LKP] ON 
GO
INSERT [dbo].[EP_WEEKDAYS_SETTINGS_LKP] ([Id], [NameEn], [NameAr], [IsWeekend], [IsRestDay]) VALUES (1, N'Monday', N'الاثنين', 0, 0)
GO
INSERT [dbo].[EP_WEEKDAYS_SETTINGS_LKP] ([Id], [NameEn], [NameAr], [IsWeekend], [IsRestDay]) VALUES (2, N'Tuesday', N'يوم الثلاثاء', 0, 0)
GO
INSERT [dbo].[EP_WEEKDAYS_SETTINGS_LKP] ([Id], [NameEn], [NameAr], [IsWeekend], [IsRestDay]) VALUES (3, N'Wednesday', N'الأربعاء', 0, 0)
GO
INSERT [dbo].[EP_WEEKDAYS_SETTINGS_LKP] ([Id], [NameEn], [NameAr], [IsWeekend], [IsRestDay]) VALUES (4, N'Thursday', N'يوم الخميس', 0, 0)
GO
INSERT [dbo].[EP_WEEKDAYS_SETTINGS_LKP] ([Id], [NameEn], [NameAr], [IsWeekend], [IsRestDay]) VALUES (5, N'Friday', N'جمعة', 1, 0)
GO
INSERT [dbo].[EP_WEEKDAYS_SETTINGS_LKP] ([Id], [NameEn], [NameAr], [IsWeekend], [IsRestDay]) VALUES (6, N'Saturday', N'السبت', 0, 1)
GO
INSERT [dbo].[EP_WEEKDAYS_SETTINGS_LKP] ([Id], [NameEn], [NameAr], [IsWeekend], [IsRestDay]) VALUES (7, N'Sunday', N'الأحد', 0, 0)
GO
SET IDENTITY_INSERT [dbo].[EP_WEEKDAYS_SETTINGS_LKP] OFF
GO
------------------------------------------------------------------
----------------------------------------------


                                              --09/07/2024-----


Insert into NOTIF_NOTIFICATION_EVENT (EventId,NameEn,NameAr,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,DeletedBy,DeletedDate,IsDeleted,ReceiverTypeId,ReceiverTypeRefId,DescriptionEn,DescriptionAr,IsActive) values
                                      (102,'Add Member Task','Add Member Task','System Generated','2024-03-25 11:41:43.297',NULL,NULL,NULL,NULL,0,1,NULL,NULL,NULL,1)

INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP (PlaceHolderId,PlaceHolderName,EventId) VALUES (131,'#Reference Number#',102)
                                              --09/07/2024-----

-------------------1/08/24
update ATTACHMENT_TYPE set  IsMandatory=1 where AttachmentTypeId=117
---------------------------------

--Ammaar Naveed--06/08/2024--Digital Signature Permissions
INSERT INTO UMS_CLAIM VALUES ('Sign Document', 'Digital_Signature', 'Action', 'Permission','Permissions.DS.DigitalSignature',N'توقيع المستند', 0,'67108864')
INSERT INTO UMS_CLAIM VALUES ('Sign by Civil ID', 'Digital_Signature', 'Digital Signature', 'Permission','Permissions.DS.DigitalSignature.LocalSigning',N'طريق البطاقة المدنية التوقيع عن', 0,'67108864')
INSERT INTO UMS_CLAIM VALUES ('Sign by Mobile Auth', 'Digital_Signature', 'Digital Signature', 'Permission','Permissions.DS.DigitalSignature.RemoteSigning',N'الهاتف التوقيع عن طريق', 0,'67108864')
INSERT INTO UMS_CLAIM VALUES ('Sign by KMID', 'Digital_Signature', 'Digital Signature', 'Permission','Permissions.DS.DigitalSignature.ExternalSigning',N'التوقيع عن طريق هويتي', 0,'67108864')


------------------------- 07-aug-024 -----------------

IF NOT EXISTS (SELECT 1 FROM [dbo].[NOTIF_NOTIFICATION_EVENT] WHERE EventId = 111)
BEGIN
    INSERT INTO [dbo].[NOTIF_NOTIFICATION_EVENT] 
        ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) 
    VALUES 
        (111, N'Leave Request submitted', N'Leave Request submitted', N'System Generated', CAST(N'2024-08-05T14:30:13.453' AS DateTime), N'fatwaadmin@gmail.com', CAST(N'2024-08-05T15:22:38.183' AS DateTime), NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
END
IF NOT EXISTS (SELECT 1 FROM [dbo].[NOTIF_NOTIFICATION_EVENT] WHERE EventId = 112)
BEGIN
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) 
  VALUES
(112, N'Leave Request Rejected', N'Leave Request Rejected', N'System Generated', CAST(N'2024-08-05T14:30:13.453' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
  END

IF NOT EXISTS (SELECT 1 FROM [dbo].[NOTIF_NOTIFICATION_EVENT] WHERE EventId = 113)
BEGIN
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) 
VALUES (113, N'Reduce Working Hours Request submited', N'Reduce Working Hours Request submited', N'System Generated', CAST(N'2024-08-05T14:30:13.453' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[NOTIF_NOTIFICATION_EVENT] WHERE EventId = 114)
BEGIN
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) 
VALUES (114, N'Reduce Working Hours Request submited', N'Reduce Working Hours Request submited', N'System Generated', CAST(N'2024-08-05T14:30:13.453' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
END
IF NOT EXISTS (SELECT 1 FROM [dbo].[NOTIF_NOTIFICATION_EVENT] WHERE EventId = 115)
BEGIN
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive])
VALUES (115, N'Reduce Working Hours Request Rejected', N'Reduce Working Hours Request Rejected', N'System Generated', CAST(N'2024-08-05T14:30:13.453' AS DateTime), N'fatwaadmin@gmail.com', CAST(N'2024-08-05T15:26:55.853' AS DateTime), NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
END
IF NOT EXISTS (SELECT 1 FROM [dbo].[NOTIF_NOTIFICATION_EVENT] WHERE [EventId] = 115)
BEGIN
    INSERT INTO [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) 
    VALUES (115, N'FingerPrint Exemption Request submitted', N'FingerPrint Exemption Request submitted', N'System Generated', CAST(N'2024-08-05T14:30:13.453' AS DateTime), N'fatwaadmin@gmail.com', CAST(N'2024-08-05T15:27:49.587' AS DateTime), NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[NOTIF_NOTIFICATION_EVENT] WHERE [EventId] = 116)
BEGIN
    INSERT INTO [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) 
    VALUES (116, N'FingerPrint Exemption Request Rejected', N'FingerPrint Exemption Request Rejected', N'System Generated', CAST(N'2024-08-05T14:30:13.453' AS DateTime), N'fatwaadmin@gmail.com', CAST(N'2024-08-05T15:27:32.183' AS DateTime), NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[NOTIF_NOTIFICATION_EVENT] WHERE [EventId] = 117)
BEGIN
    INSERT INTO [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) 
    VALUES (117, N'Permission Request submitted', N'Permission Request submitted', N'System Generated', CAST(N'2024-08-05T14:30:13.453' AS DateTime), N'fatwaadmin@gmail.com', CAST(N'2024-08-05T15:28:07.403' AS DateTime), NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[NOTIF_NOTIFICATION_EVENT] WHERE [EventId] = 118)
BEGIN
    INSERT INTO [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) 
    VALUES (118, N'Permission Request Rejected', N'Permission Request Rejected', N'System Generated', CAST(N'2024-08-05T14:30:13.453' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[NOTIF_NOTIFICATION_EVENT] WHERE [EventId] = 119)
BEGIN
    INSERT INTO [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) 
    VALUES (119, N'Medical Appointment Request submitted', N'Medical Appointment Request submitted', N'System Generated', CAST(N'2024-08-05T14:30:13.453' AS DateTime), N'fatwaadmin@gmail.com', CAST(N'2024-08-05T15:29:59.350' AS DateTime), NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[NOTIF_NOTIFICATION_EVENT] WHERE [EventId] = 120)
BEGIN
    INSERT INTO [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) 
    VALUES (120, N'Medical Appoinment Request Rejected', N'Medical Appoinment Request Rejected', N'System Generated', CAST(N'2024-08-05T14:30:13.453' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[NOTIF_NOTIFICATION_EVENT] WHERE [EventId] = 121)
BEGIN
    INSERT INTO [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) 
    VALUES (121, N'Leave Request Approved ', N'Leave Request Approved', N'System Generated', CAST(N'2024-08-05T18:21:02.753' AS DateTime), N'fatwaadmin@gmail.com', CAST(N'2024-08-06T10:03:23.467' AS DateTime), NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[NOTIF_NOTIFICATION_EVENT] WHERE [EventId] = 122)
BEGIN
    INSERT INTO [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) 
    VALUES (122, N'Medical Appointment Request Need Modification', N'Medical Appointment Request Need Modification', N'System Generated', CAST(N'2024-08-05T18:21:02.753' AS DateTime), N'fatwaadmin@gmail.com', CAST(N'2024-08-06T10:03:36.520' AS DateTime), NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[NOTIF_NOTIFICATION_EVENT] WHERE [EventId] = 123)
BEGIN
    INSERT INTO [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) 
    VALUES (123, N'Leave Request Need Modification', N'Leave Request Need Modification', N'System Generated', CAST(N'2024-08-05T18:21:02.753' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
END
GO

------------------ 07-aug-024 --------------------------


IF NOT EXISTS (SELECT 1 FROM [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] WHERE [PlaceHolderId] = 136)
BEGIN
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (136, N'#Service Request Number#', 111)
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] WHERE [PlaceHolderId] = 138)
BEGIN
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (138, N'#Service Request Number#', 121)
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] WHERE [PlaceHolderId] = 140)
BEGIN
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (140, N'#Service Request Number#', 112)
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] WHERE [PlaceHolderId] = 141)
BEGIN
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (141, N'#Service Request Number#', 123)
END


-------------------------------8/8/24
GO
IF ((SELECT COUNT(*) FROM [CMS_OPERATING_SECTOR_TYPE_G2G_LKP] WHERE Id = 51) = 0)
BEGIN
	SET IDENTITY_INSERT [CMS_OPERATING_SECTOR_TYPE_G2G_LKP] ON
INSERT INTO CMS_OPERATING_SECTOR_TYPE_G2G_LKP 
(ID,Code, Name_En, Name_Ar, IsActive, DepartmentId, ParentId,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,DeletedBy,DeletedDate,IsDeleted,ModuleId)
VALUES (51, 'AAD','Administrative Affairs Department','Administrative Affairs Department',1,2,21,'fatwaadmin@gmail.com',GETDATE(),NULL,NULL,NULL,NULL,0,NULL);
SET IDENTITY_INSERT [CMS_OPERATING_SECTOR_TYPE_G2G_LKP] OFF
END
Update CMS_OPERATING_SECTOR_TYPE_G2G_LKP SET ParentId = 21  WHERE Id = 51
PRINT 'Record UPDATED'
Go

-------------- 08-August-024 -------------------
IF NOT EXISTS (Select 1 From [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] WHERE [PlaceHolderId] = 142)
BEGIN
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (142, '#Start Date#', 109)
END

IF NOT EXISTS (Select 1 From [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] WHERE [PlaceHolderId] = 143)
BEGIN
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (143, '#End Date#', 109)
END


---------------------------------
---06-08-2024
set IDENTITY_INSERT WS_WORKERSERVICES_LKP on
INSERT INTO WS_WORKERSERVICES_LKP (Id,WorkerServiceEn,WorkerServiceAr)VALUES (9,'RabbitMQUnpublishedMessagesService','RabbitMQUnpublishedMessagesService')
set IDENTITY_INSERT WS_WORKERSERVICES_LKP off
--------------------------------
INSERT INTO Submodule (Id , Name_En , Name_Ar) VALUES(128, 'DMS Review Document' ,'DMS Review Document')
INSERT INTO Submodule (Id , Name_En , Name_Ar) VALUES(256, 'Organizing Committee' ,'Organizing Committee')

----------------------------
INSERT INTO MEET_MEETING_STATUS (MeetingStatusId , NameEn, NameAr) VALUES (16384 , 'Requested By Organizer', N'Requested By Organizer')
INSERT INTO MEET_MEETING_STATUS (MeetingStatusId , NameEn, NameAr) VALUES (32768 , 'Rejected', N'Rejected')


-----------------------------
Update NOTIF_NOTIFICATION_EVENT Set NameEn= 'Committee Added' , NameAr = N'Committee Added' where EventId = 95
UPDATE NOTIF_NOTIFICATION_EVENT set NameEn = 'Comment Added' , NameAr = N'Comment Added' where EventId = 88

----------------------------

UPDATE NOTIF_NOTIFICATION_TEMPLATE set BodyAr = N'تم إنشاء اللجنة رقم #Reference Number# من قبل #Sender Name# بتاريخ #Created Date# وتم إضافة #Receiver Name# كعضو في اللجنة' , BodyEn ='The committe No. #Reference Number# has been created by #Sender Name# on #Created Date# and the #Receiver Name# has been added as a member' where EventId = 95
UPDATE NOTIF_NOTIFICATION_TEMPLATE set BodyAr = N'تم إضافة #Receiver Name# كعضو في اللجنة رقم #Reference Number# من قبل #Sender Name# بتاريخ #Created Date#',BodyEn ='#Receiver Name# has been added as the member to the committee No. #Reference Number# by #Sender Name# on #Created Date#' where EventId = 96
UPDATE NOTIF_NOTIFICATION_TEMPLATE set BodyAr = N'تم تعيين المهمة رقم #Reference Number# لـ#Receiver Name# من قبل #Sender Name# بتاريخ #Created Date#' , BodyEn = 'The task No. #Reference Number# has been assigned to #Receiver Name# by #Sender Name# on #Created Date#' where EventId = 102
UPDATE NOTIF_NOTIFICATION_TEMPLATE set BodyAr = N'تم إغلاق اللجنة رقم #Reference Number# من قبل #Sender Name# بتاريخ #Created Date#' , BodyEn ='The committe No. #Reference Number# has been dissolved by #Sender Name# on #Created Date#' where EventId = 103
UPDATE NOTIF_NOTIFICATION_TEMPLATE set BodyAr = N'تم تعديل الصلاحية لـ#Receiver Name# في اللجنة رقم #Reference Number# من قبل #Sender Name# بتاريخ #Created Date#',BodyEn='The access of #Receiver Name# in the committee No. #Reference Number# has been updated by  #Sender Name# on #Created Date#' where EventId = 104

---------------------------
Update NOTIF_NOTIFICATION_TEMPLATE set BodyEn = 'A Meeting is modified number #File Number# by #Sender Name# on the date #Created Date#' , BodyAr = N'رقم تم تعديل مقابلة #File Number# من قبل #Sender Name# بتاريخ #Created Date#'  WHERE EventId = 41



set IDENTITY_INSERT WS_WORKERSERVICES_LKP off
--------------------------------
INSERT INTO Submodule (Id , Name_En , Name_Ar) VALUES(128, 'DMS Review Document' ,'DMS Review Document')
INSERT INTO Submodule (Id , Name_En , Name_Ar) VALUES(256, 'Organizing Committee' ,'Organizing Committee')
------- 12-08-024
if Not Exists (select id from Submodule where id = 512 ) 
insert Submodule VALUES (512, 'Leave and Attendance', N'الاجازة والحضور')
else
  print 'data already exist';
set IDENTITY_INSERT WS_WORKERSERVICES_LKP off

----- LLS start 13-08-2024
-------------


UPDATE NOTIF_NOTIFICATION_EVENT SET NameEn = N'Update Literature Borrow Approval Status', NameAr = N'Update Literature Borrow Approval Status' WHERE EventId = 74;

UPDATE NOTIF_NOTIFICATION_EVENT SET NameEn = N'Update Literature Borrow Reject Status', NameAr = N'Update Literature Borrow Reject Status' WHERE EventId = 75;

UPDATE NOTIF_NOTIFICATION_EVENT SET NameEn = N'Lms Literature Borrow Request For Approval', NameAr = N'Lms Literature Borrow Request For Approval' WHERE EventId = 76;

UPDATE NOTIF_NOTIFICATION_EVENT SET NameEn = N'Lms Literature Borrow Request For Rejection', NameAr = N'Lms Literature Borrow Request For Rejection' WHERE EventId = 77;


---------------------

UPDATE NOTIF_NOTIFICATION_TEMPLATE SET BodyEn = N'LMS borrow request has been approved for Literature, #Name# by #Sender Name# on #Created Date# for #Receiver Name#.', BodyAr = N'LMS borrow request has been approved for Literature, #Name# by #Sender Name# on #Created Date# for #Receiver Name#.' WHERE EventId = 76;

UPDATE NOTIF_NOTIFICATION_TEMPLATE SET BodyEn = N'LMS borrow request has been rejected for Literature, #Name# by #Sender Name# on #Created Date# for #Receiver Name#.', BodyAr = N'LMS borrow request has been rejected for Literature, #Name# by #Sender Name# on #Created Date# for #Receiver Name#.' WHERE EventId = 77;

------------------


INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (144, N'#Name#', 124)

INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (145, N'#Name#', 125)

INSERT INTO NOTIF_NOTIFICATION_EVENT ( EventId, NameEn, NameAr , CreatedBy , CreatedDate, ReceiverTypeId , IsDeleted)
VALUES (125,  'Lms Literature Borrow Request For Extended' , 'Lms Literature Borrow Request For Extended',  'System Generated' ,'2024-03-20 11:41:43.297', 1 , 0);

INSERT INTO NOTIF_NOTIFICATION_EVENT ( EventId, NameEn, NameAr , CreatedBy , CreatedDate, ReceiverTypeId , IsDeleted)
VALUES (124,  'Lms Literature Borrow Request Returned' , 'Lms Literature Borrow Request Returned',  'System Generated' ,'2024-03-20 11:41:43.297', 1 , 0);


 INSERT INTO NOTIF_NOTIFICATION_TEMPLATE (TemplateId, EventId, ChannelId, NameEn, NameAr ,SubjectEn, SubjectAr, BodyEn, BodyAr,CreatedBy , CreatedDate , IsDeleted )
VALUES (NewID(),124, 4, 'Lms Literature Borrow Request Returned' , 'Lms Literature Borrow Request Returned', 'Extended Return Successfully','Extended Return Successfully',
'LMS Literature borrowed book return request has been returned for Literature #Name# by #Sender Name# on #Created Date# for #Receiver Name#.', 
'LMS Literature borrowed book return request has been returned for Literature #Name# by #Sender Name# on #Created Date# for #Receiver Name#.', 
'System Generated' ,'2024-04-04 11:41:43.297',0);


 INSERT INTO NOTIF_NOTIFICATION_TEMPLATE (TemplateId, EventId, ChannelId, NameEn, NameAr ,SubjectEn, SubjectAr, BodyEn, BodyAr,CreatedBy , CreatedDate , IsDeleted )
VALUES (NewID(),125, 4, 'Lms Literature Borrow Request Extended' , 'Lms Literature Borrow Request Extended', 'Returned Successfully','Returned Successfully',
'LMS Literature borrowed book extension request has been extended for Literature #Name# by #Sender Name# on #Created Date# for #Receiver Name#.', 
'LMS Literature borrowed book extension request has been extended for Literature #Name# by #Sender Name# on #Created Date# for #Receiver Name#.', 
'System Generated' ,'2024-04-04 11:41:43.297',0);

-----------

EXEC [dbo].pInsTranslation 'CLC_Library',N'المركز الثقافي القانوني','CLC','Literature',1

-------
----- LLS end 13-08-2024
---------------------- 13-08-024 ----------------
 
--  IF NOT EXISTS (Select * from NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP where PlaceHolderId = 146)
--insert NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP Values (146, '#Service Request Number#', 113)
--else
-- print 'Record alrady exist'


-- IF NOT EXISTS (Select * from NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP where PlaceHolderId = 147)
--insert NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP Values (147, '#Permission Date#', 114)
--else
-- print 'Record alrady exist'
--  IF NOT EXISTS (Select * from NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP where PlaceHolderId = 148)
--insert NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP Values (148, '#Start Time#', 114)
--else
-- print 'Record alrady exist'
--  IF NOT EXISTS (Select * from NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP where PlaceHolderId = 149)
--insert NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP Values (149, '#End Time#', 114)
--else
-- print 'Record alrady exist'
 ------------------------- 

  update NOTIF_NOTIFICATION_EVENT set NameEn = 'Service Request Resubmitted', 
 NameAr = N'Service Request Resubmitted', ModifiedBy ='fatwadmin@gmail.com', ModifiedDate = CURRENT_TIMESTAMP  where EventId = 113

 update NOTIF_NOTIFICATION_EVENT set NameEn = 'Service Permission Request Handover To User', 
 NameAr = N'Service Permission Request Handover To User', ModifiedBy ='fatwadmin@gmail.com', ModifiedDate = CURRENT_TIMESTAMP where EventId = 114


 ----  LLS start 13-08-2024

 IF Exists(SELECT 1 from LMS_LITERATURE_BORROW_RETURN_APPROVAL_STATUS where id = 8) 

update LMS_LITERATURE_BORROW_RETURN_APPROVAL_STATUS set NameEn ='Pending for Return Book Approval' , NameAr =N'في انتظار الموافقة على كتاب العودة' where id = 8

GO  
 
IF Exists(SELECT 1 from LMS_LITERATURE_BORROW_APPROVAL_STATUS where DecisionId = 64) 

update LMS_LITERATURE_BORROW_APPROVAL_STATUS set Name ='Pending for Extension Approval' , Name_Ar =N'في انتظار الموافقة على التمديد' where DecisionId = 64

GO

IF Exists(SELECT 1 from LMS_LITERATURE_BORROW_APPROVAL_STATUS where DecisionId = 4) 
update LMS_LITERATURE_BORROW_APPROVAL_STATUS set Name ='Borrowing Period Expired' , Name_Ar =N'انتهت فترة الاقتراض' where DecisionId = 4
GO

IF NOT Exists(SELECT 1 from LMS_LITERATURE_BORROW_APPROVAL_STATUS where DecisionId = 512) 
INSERT INTO LMS_LITERATURE_BORROW_APPROVAL_STATUS (DecisionId, Name, Name_Ar) VALUES (512, 'Extending Period Expired', N'انتهت فترة التمديد')
GO
 
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (152, N'#Name#', 127)

INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (153, N'#Name#', 128)

INSERT INTO NOTIF_NOTIFICATION_EVENT ( EventId, NameEn, NameAr , CreatedBy , CreatedDate, ReceiverTypeId , IsDeleted)

VALUES (127,  'Lms Literature Borrow Extension Request Approved' , 'Lms Literature Borrow Extension Request Approved',  'System Generated' ,'2024-03-20 11:41:43.297', 1 , 0);

INSERT INTO NOTIF_NOTIFICATION_EVENT ( EventId, NameEn, NameAr , CreatedBy , CreatedDate, ReceiverTypeId , IsDeleted)

VALUES (128,  'Lms Literature Borrow Extension Request Rejected' , 'Lms Literature Borrow Extension Request Rejected',  'System Generated' ,'2024-03-20 11:41:43.297', 1 , 0);


INSERT INTO NOTIF_NOTIFICATION_TEMPLATE (TemplateId, EventId, ChannelId, NameEn, NameAr ,SubjectEn, SubjectAr, BodyEn, BodyAr,CreatedBy , CreatedDate , IsDeleted )

VALUES (NewID(),127, 4, 'Literature Borrow Extension Request Approved' , 'Literature Borrow Extension Request Approved', 'Extension Request Approved Successfully','Extension Request Approved Successfully',

'LMS Literature borrowed book extension has been approved for Literature #Name# by #Sender Name# on #Created Date# for #Receiver Name#.', 

'LMS Literature borrowed book extension has been epproved for Literature #Name# by #Sender Name# on #Created Date# for #Receiver Name#.', 

'System Generated' ,'2024-04-04 11:41:43.297',0); 

INSERT INTO NOTIF_NOTIFICATION_TEMPLATE (TemplateId, EventId, ChannelId, NameEn, NameAr ,SubjectEn, SubjectAr, BodyEn, BodyAr,CreatedBy , CreatedDate , IsDeleted )

VALUES (NewID(),128, 4, 'Literature Borrow Extension Request Rejected' , 'Literature Borrow Extension Request Rejected', 'Extension Request Rejected Successfully','Extension Request Rejected Successfully',

'LMS Literature borrowed book extension request has been rejected for Literature #Name# by #Sender Name# on #Created Date# for #Receiver Name#.', 

'LMS Literature borrowed book extension request has been rejected for Literature #Name# by #Sender Name# on #Created Date# for #Receiver Name#.', 

'System Generated' ,'2024-04-04 11:41:43.297',0);
 


 ----- LLS end 13-08-2024
 -------------------------------------------------------------------------
  IF  Exists(SELECT 1 from CMS_CASE_FILE_EVENT_G2G_LKP where Id = 128 )
 UPDATE CMS_CASE_FILE_EVENT_G2G_LKP SET Name_En ='Copy Received', Name_Ar = N'تم استلام نسخة' WHERE Id = 128
GO

IF  Exists(SELECT 1 from CMS_CASE_FILE_EVENT_G2G_LKP where Id = 256 )
 UPDATE CMS_CASE_FILE_EVENT_G2G_LKP SET Name_En ='File Assigned to Lawyer', Name_Ar = N'تم تعيين الملف للعضو' WHERE Id = 256
GO

IF  Exists(SELECT 1 from CMS_CASE_FILE_EVENT_G2G_LKP where Id = 512 )
 UPDATE CMS_CASE_FILE_EVENT_G2G_LKP SET Name_En ='Is Returned Back', Name_Ar = N'تم إعادة الملف' WHERE Id = 512
GO

IF  Exists(SELECT 1 from CMS_CASE_FILE_EVENT_G2G_LKP where Id = 32768 )
 UPDATE CMS_CASE_FILE_EVENT_G2G_LKP SET Name_En ='Case File Created', Name_Ar = N'تم إنشاء ملف القضية' WHERE Id = 32768
GO

IF  Exists(SELECT 1 from CMS_CASE_FILE_EVENT_G2G_LKP where Id = 65536 )
 UPDATE CMS_CASE_FILE_EVENT_G2G_LKP SET Name_En ='File Accepted', Name_Ar = N'يتم العمل على الملف' WHERE Id = 65536
GO

IF  Exists(SELECT 1 from CMS_CASE_FILE_EVENT_G2G_LKP where Id = 16384 )
 UPDATE CMS_CASE_FILE_EVENT_G2G_LKP SET Name_En ='Saved and Closed', Name_Ar = N'تم حفظه واغلاقه' WHERE Id = 16384
GO
 -------------------------------------------------------------------------


delete from NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP where EventId in(53,59)
insert into NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP values ((select max(PlaceHolderId)+1 from NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP),'#Type#',53)
insert into NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP values ((select max(PlaceHolderId)+1 from NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP),'#Reference Number#',53)
insert into NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP values ((select max(PlaceHolderId)+1 from NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP),'#Type#',59)
insert into NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP values ((select max(PlaceHolderId)+1 from NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP),'#Reference Number#',59)

update NOTIF_NOTIFICATION_TEMPLATE set BodyEn = 'No #Type# is created against #Entity# number #Reference Number#.',
BodyAr = 'No #Type# is created against #Entity# number #Reference Number#.' where EventId = 53

update NOTIF_NOTIFICATION_TEMPLATE set BodyEn = 'Reminder: No additional information or #Type# has been created against #Entity# number #Reference Number# yet',
BodyAr = 'Reminder: No additional information or #Type# has been created against #Entity# number #Reference Number# yet' where EventId = 59

------------------------


-----------------------------
Update NOTIF_NOTIFICATION_EVENT Set NameEn= 'Committee Added' , NameAr = N'Committee Added' where EventId = 95
UPDATE NOTIF_NOTIFICATION_EVENT set NameEn = 'Comment Added' , NameAr = N'Comment Added' where EventId = 88

----------------------------

UPDATE NOTIF_NOTIFICATION_TEMPLATE set BodyAr = N'تم إنشاء اللجنة رقم #Reference Number# من قبل #Sender Name# بتاريخ #Created Date# وتم إضافة #Receiver Name# كعضو في اللجنة' , BodyEn ='The committe No. #Reference Number# has been created by #Sender Name# on #Created Date# and the #Receiver Name# has been added as a member' where EventId = 95
UPDATE NOTIF_NOTIFICATION_TEMPLATE set BodyAr = N'تم إضافة #Receiver Name# كعضو في اللجنة رقم #Reference Number# من قبل #Sender Name# بتاريخ #Created Date#',BodyEn ='#Receiver Name# has been added as the member to the committee No. #Reference Number# by #Sender Name# on #Created Date#' where EventId = 96
UPDATE NOTIF_NOTIFICATION_TEMPLATE set BodyAr = N'تم تعيين المهمة رقم #Reference Number# لـ#Receiver Name# من قبل #Sender Name# بتاريخ #Created Date#' , BodyEn = 'The task No. #Reference Number# has been assigned to #Receiver Name# by #Sender Name# on #Created Date#' where EventId = 102
UPDATE NOTIF_NOTIFICATION_TEMPLATE set BodyAr = N'تم إغلاق اللجنة رقم #Reference Number# من قبل #Sender Name# بتاريخ #Created Date#' , BodyEn ='The committe No. #Reference Number# has been dissolved by #Sender Name# on #Created Date#' where EventId = 103
UPDATE NOTIF_NOTIFICATION_TEMPLATE set BodyAr = N'تم تعديل الصلاحية لـ#Receiver Name# في اللجنة رقم #Reference Number# من قبل #Sender Name# بتاريخ #Created Date#',BodyEn='The access of #Receiver Name# in the committee No. #Reference Number# has been updated by  #Sender Name# on #Created Date#' where EventId = 104

---------------------------
Update NOTIF_NOTIFICATION_TEMPLATE set BodyEn = 'A Meeting is modified number #File Number# by #Sender Name# on the date #Created Date#' , BodyAr = N'رقم تم تعديل مقابلة #File Number# من قبل #Sender Name# بتاريخ #Created Date#'  WHERE EventId = 41


---------------------------
--------------------------------
INSERT INTO Submodule (Id , Name_En , Name_Ar) VALUES(128, 'DMS Review Document' ,'DMS Review Document')
INSERT INTO Submodule (Id , Name_En , Name_Ar) VALUES(256, 'Organizing Committee' ,'Organizing Committee')

----------------------------
INSERT INTO MEET_MEETING_STATUS (MeetingStatusId , NameEn, NameAr) VALUES (16384 , 'Requested By Organizer', N'Requested By Organizer')
INSERT INTO MEET_MEETING_STATUS (MeetingStatusId , NameEn, NameAr) VALUES (32768 , 'Rejected', N'Rejected')
-------Case File Transfer Request------
INSERT INTO UMS_GROUP_CLAIMS ([GroupId],[ClaimType],[ClaimValue])
VALUES ('4A6C0AA1-D091-4C01-B6F3-4CDD3780A06A', 'Permission', 'Permissions.CMS.CaseFile.TransferRequest.List')
-----
INSERT INTO UMS_GROUP_CLAIMS ([GroupId],[ClaimType],[ClaimValue])
VALUES ('4A6C0AA1-D091-4C01-B6F3-4CDD3780A06A', 'Permission', 'Permissions.CMS.CaseFile.TransferRequest.Add')
--------
INSERT INTO UMS_GROUP_CLAIMS ([GroupId],[ClaimType],[ClaimValue])
VALUES ('4A6C0AA1-D091-4C01-B6F3-4CDD3780A06A', 'Permission', 'Permissions.CMS.CaseFile.TransferRequest.View')
--------------
INSERT INTO UMS_GROUP_CLAIMS ([GroupId],[ClaimType],[ClaimValue])
VALUES ('49744B64-2399-4097-94FD-326D6FCE2626', 'Permission', 'Permissions.CMS.CaseFile.TransferRequest.List')
------
INSERT INTO UMS_GROUP_CLAIMS ([GroupId],[ClaimType],[ClaimValue])
VALUES ('49744B64-2399-4097-94FD-326D6FCE2626', 'Permission', 'Permissions.CMS.CaseFile.TransferRequest.Add')
------
INSERT INTO UMS_GROUP_CLAIMS ([GroupId],[ClaimType],[ClaimValue])
VALUES ('49744B64-2399-4097-94FD-326D6FCE2626', 'Permission', 'Permissions.CMS.CaseFile.TransferRequest.View')
-------
INSERT INTO CMS_CASE_FILE_TRANSFER_REQUEST_ENUM ([ActionId],[NameEn],[NameAr]) VALUES(1,'Approved',N'معتمد')
-------
INSERT INTO CMS_CASE_FILE_TRANSFER_REQUEST_ENUM ([ActionId],[NameEn],[NameAr]) VALUES(2,'Rejected',N'رفض')
-------
INSERT INTO CMS_CASE_FILE_TRANSFER_REQUEST_ENUM ([ActionId],[NameEn],[NameAr]) VALUES(3,'Submitted',N'الطلبات المرسلة')
-------
INSERT INTO [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) 
VALUES (126, N'Request For Transfer To Sector', N'Request For Transfer To Sector', N'System Generated', GETDATE(), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
-------
INSERT INTO [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) 
VALUES (126, N'Request For Transfer To Sector', N'Request For Transfer To Sector', N'System Generated', GETDATE(), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
-------
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) 
VALUES (NEWID(), 126, 4, N'Request for Transfer to Sector', N'Request for Transfer to Sector', N'Request for Transfer to Sector Successfully', N'Request for Transfer to Sector Successfully', N'Following #Entity#, Request has been transfer from #Sector From# to #Sector To# on #Created Date# Date by #Sender Name#.', N'Following #Entity#, Request has been transfer from #Sector From# to #Sector To# on #Created Date# Date by #Sender Name#.', NULL, N'fatwaadmin@gmail.com', GETDATE(), N'fatwaadmin@gmail.com', NULL, NULL, NULL, 0, 1)
-------
INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP ([PlaceHolderId],[PlaceHolderName],[EventId]) VALUES (150,'#Sector From#',126)
-------
INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP ([PlaceHolderId],[PlaceHolderName],[EventId]) VALUES (151,'#Sector To#',126)
-------
INSERT INTO [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) 
VALUES (129, N'Reject To Accept Transfer Request', N'Reject To Accept Transfer Request', N'System Generated', GETDATE(), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
 -------
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) 
VALUES (NEWID(), 129, 4, N'Reject To Accept Transfer Request', N'Reject To Accept Transfer Request', N'Request for Transfer is Rejected', N'Request for Transfer is Rejected', N'Request For Transfer Sector has been Rejected send by #Sender Name#  on #Created Date#', N'Request For Transfer Sector has been Rejected send by #Sender Name#  on #Created Date#', NULL, N'fatwaadmin@gmail.com', GETDATE(), N'fatwaadmin@gmail.com', NULL, NULL, NULL, 0, 1)
-------
INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP ([PlaceHolderId],[PlaceHolderName],[EventId]) VALUES (154,'#Sender Name#',129)
-------
INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP ([PlaceHolderId],[PlaceHolderName],[EventId]) VALUES (155,'#File Number#',129)
-------
INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP ([PlaceHolderId],[PlaceHolderName],[EventId]) VALUES (156,'#Receiver Name#',129)
-------
INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP ([PlaceHolderId],[PlaceHolderName],[EventId]) VALUES (157,'#Sector To#',129)
-------
INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP ([PlaceHolderId],[PlaceHolderName],[EventId]) VALUES (158,'#Created Date#',129)
-------
INSERT INTO [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) 
VALUES (130, N'Approved Request For Transfer', N'Approved Request For Transfer', N'System Generated', GETDATE(), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
-------
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) 
VALUES (NEWID(), 130, 4, N'Approved Request For Transfer', N'Approved Request For Transfer', N'Request for Transfer is Approved', N'Request for Transfer is Approved', N'Request For Transfer Sector has been Approved send by #Sender Name#  on #Created Date#', N'Request For Transfer Sector has been Approved send by #Sender Name#  on #Created Date#', NULL, N'fatwaadmin@gmail.com', GETDATE(), N'fatwaadmin@gmail.com', NULL, NULL, NULL, 0, 1)
-------
INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP ([PlaceHolderId],[PlaceHolderName],[EventId]) VALUES (163,'#Sender Name#',130)
-------
INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP ([PlaceHolderId],[PlaceHolderName],[EventId]) VALUES (164,'#Receiver Name#',130)
-------
INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP ([PlaceHolderId],[PlaceHolderName],[EventId]) VALUES (165,'#Sector To#',130)
-------
INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP ([PlaceHolderId],[PlaceHolderName],[EventId]) VALUES (166,'#Created Date#',130)

---------------- 23-Aug-024 ---------

IF NOT EXISTS (Select * from NOTIF_NOTIFICATION_TEMPLATE where EventId = 111)
INSERT INTO NOTIF_NOTIFICATION_TEMPLATE VALUES (NewID(),111, 4, 'Leave Request submitted' , N'Leave Request submitted',null, null,
'#Entity#  #Service Request Number# has been created by #Sender Name#  on #Created Date# ', 
'#Entity#  #Service Request Number# has been created by #Sender Name#  on #Created Date# ', null,
'System Generated' ,CURRENT_TIMESTAMP,null, null, null, null,0, 1);
else
 print 'Record alrady exist'
 

IF NOT EXISTS (Select * from NOTIF_NOTIFICATION_TEMPLATE where EventId = 113)
INSERT INTO NOTIF_NOTIFICATION_TEMPLATE VALUES (NewID(),113, 4, 'Service Request Resubmitted' , N'Service Request Resubmitted',null, null,
'#Entity# #Service Request Number# is Resubmitted by #Sender Name# on #Created Date#', 
'#Entity# #Service Request Number# is Resubmitted by #Sender Name# on #Created Date#', null,
'System Generated' ,CURRENT_TIMESTAMP,null, null, null, null,0, 1);
else
 print 'Record alrady exist'

IF NOT EXISTS (Select * from NOTIF_NOTIFICATION_TEMPLATE where EventId = 121)
INSERT INTO NOTIF_NOTIFICATION_TEMPLATE VALUES (NewID(),121, 4, 'Leave Request Approved' , N'Leave Request Approved ',null, null,
'#Entity# #Service Request Number# has been Approved by #Sender Name#  on #Created Date# ', 
'#Entity# #Service Request Number# has been Approved by #Sender Name#  on #Created Date# ', null,
'System Generated' ,CURRENT_TIMESTAMP,null, null, null, null,0, 1);
else
 print 'Record alrady exist'



IF NOT EXISTS (Select * from NOTIF_NOTIFICATION_TEMPLATE where EventId = 123)
INSERT INTO NOTIF_NOTIFICATION_TEMPLATE VALUES (NewID(),123, 4, 'Leave Request Need Modification' , N'Leave Request Need Modification',null, null,
'#Sender Name# has requested a modification to #Entity# #Service Request Number#  on  #Created Date#', 
'#Sender Name# has requested a modification to #Entity# #Service Request Number#  on  #Created Date#', null,
'System Generated' ,CURRENT_TIMESTAMP,null, null, null, null,0, 1);
else
 print 'Record alrady exist'


IF NOT EXISTS (Select * from NOTIF_NOTIFICATION_TEMPLATE where EventId = 112)
INSERT INTO NOTIF_NOTIFICATION_TEMPLATE VALUES (NewID(),112, 4, 'Leave Request Rejected' , N'Leave Request Rejected',null, null,
' #Entity# #Service Request Number# Rejected by #Sender Name#  on #Created Date#', 
' #Entity# #Service Request Number# Rejected by #Sender Name#  on #Created Date#', null,
'System Generated' ,CURRENT_TIMESTAMP,null, null, null, null,0, 1);
else
 print 'Record alrady exist'

 

IF NOT EXISTS (Select * from NOTIF_NOTIFICATION_TEMPLATE where EventId = 114)
INSERT INTO NOTIF_NOTIFICATION_TEMPLATE VALUES (NewID(),114, 4, 'Service Permission Request Handover To User' , N'Service Permission Request Handover To User',null, null,
'#Sender Name# will be on #Entity# on #Permission Date#. For the specified period from #Start Time# to #End Time# , their duties and responsibilities are handed over to you.', 
'#Sender Name# will be on #Entity# on #Permission Date#. For the specified period from #Start Time# to #End Time# , their duties and responsibilities are handed over to you.', null,
'System Generated' ,CURRENT_TIMESTAMP,null, null, null, null,0, 1);
else
 print 'Record alrady exist'

------------

 IF NOT EXISTS (Select * from NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP where PlaceHolderId = 154)
insert NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP Values (154, '#Service Request Number#', 113)
else
 print 'Record alrady exist'
 
 IF NOT EXISTS (Select * from NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP where PlaceHolderId = 155)
insert NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP Values (155, '#Permission Date#', 114)
else
 print 'Record alrady exist'

 IF NOT EXISTS (Select * from NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP where PlaceHolderId = 156)
insert NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP Values (156, '#Start Time#', 114)
else
 print 'Record alrady exist'

  IF NOT EXISTS (Select * from NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP where PlaceHolderId = 157)
insert NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP Values (157, '#End Time#', 114)
else
 print 'Record alrady exist'

 -----------------------------23/8/24-------------------------


IF Exists(SELECT 1 from MODULE where ModuleNameEn = 'InventoryManagement')
BEGIN
DELETE FROM MODULE WHERE ModuleNameEn = 'InventoryManagement'
END
SET IDENTITY_INSERT  MODULE ON
INSERT INTO MODULE (ModuleId , ModuleNameEn , ModuleNameAr) VALUES (16,'Inventory Management',N'إدارة المخزون')
SET IDENTITY_INSERT  MODULE OFF

IF Exists(SELECT 1 from MODULE where ModuleNameEn = 'Vendor Contract Management')
BEGIN
DELETE FROM MODULE WHERE ModuleNameEn = 'Vendor Contract Management'
END
SET IDENTITY_INSERT  MODULE ON
INSERT INTO MODULE (ModuleId , ModuleNameEn , ModuleNameAr) VALUES (17,'Vendor Contract Management',N'إدارة عقود البائعين')
SET IDENTITY_INSERT  MODULE OFF

IF Exists(SELECT 1 from MODULE where ModuleNameEn = 'Service Request')
BEGIN
DELETE FROM MODULE WHERE ModuleNameEn = 'Service Request'
END
SET IDENTITY_INSERT  MODULE ON
INSERT INTO MODULE (ModuleId , ModuleNameEn , ModuleNameAr) VALUES (18,'Service Request',N'طلب خدمة')
SET IDENTITY_INSERT  MODULE OFF

-------------------------------------------------------------------

------------------------------------24/8/24

insert into NOTIF_NOTIFICATION_EVENT VALUES (131, 'Decision Form Re-Uploaded' , 'Decision Form Re-Uploaded', 'System Generated', CURRENT_TIMESTAMP, null, null,
null, null, 0, 1, null, null, null,1)
 
IF NOT EXISTS (Select * from NOTIF_NOTIFICATION_TEMPLATE where EventId = 131)
INSERT INTO NOTIF_NOTIFICATION_TEMPLATE VALUES (NewID(),131, 4, 'Decision Form Re-Uploaded' , N'Decision Form Re-Uploaded',null, null,
'#Entity#  #Service Request Number# Decision Form Re-Uploaded by #Sender Name# on #Created Date#', 
'#Entity#  #Service Request Number# Decision Form Re-Uploaded by #Sender Name# on #Created Date#', null,
'System Generated' ,CURRENT_TIMESTAMP,null, null, null, null,0, 1);
else
print 'Record alrady exist';
 

IF NOT EXISTS (Select * from NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP where PlaceHolderId = 158)
insert into NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP VALUES(158,'#Service Request Number#', 131)
else
print 'Record alrady exist'

----------------------------------end------------------------------------
---------------- 25-Aug-024 ----------
IF NOT EXISTS (Select * from UMS_ROLE where Id = '3C2C927A-F7DE-4A84-884E-C694780FB8D8')
insert into UMS_ROLE VALUES('3C2C927A-F7DE-4A84-884E-C694780FB8D8', 'Leave And Duty Employee', 'LEAVE AND DUTY EMPLOYEE', '92C72DB3-E452-4153-961F-628AB2772BF5', 'Leave And Duty Employee', 'superadmin@gmail.com',
CURRENT_TIMESTAMP, null, null, null, null, 0, 'Leave And Duty Employee', N'موظف الإجازة والواجب')
else
print 'Record alrady exist';


INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP ([PlaceHolderId],[PlaceHolderName],[EventId]) VALUES (166,'#Created Date#',130)
--------
INSERT INTO UMS_CLAIM ([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted],[ModuleId])
VALUES('Case File Transfer Request List','Case_Management_System','Case_File','Permission','Permissions.CMS.CaseFile.TransferRequest.List','Case File Transfer Request',0,2)
-----
INSERT INTO UMS_CLAIM ([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted],[ModuleId])
VALUES('Case File Transfer Request Add','Case_Management_System','Case_File','Permission','Permissions.CMS.CaseFile.TransferRequest.Add','Case File Transfer Request',0,2)
--------
INSERT INTO UMS_CLAIM ([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted],[ModuleId])
VALUES('Case File Transfer Request View','Case_Management_System','Case_File','Permission','Permissions.CMS.CaseFile.TransferRequest.View','Case File Transfer Request',0,2)
---------

--------------- 26-8-2024 -----------------------
update NOTIF_NOTIFICATION_EVENT set NameEn = 'Claim Statement & Defense Letter Reminder' , NameAr = 'Claim Statement & Defense Letter Reminder' where EventId = 53
update NOTIF_NOTIFICATION_EVENT set NameEn = 'Opinion Notes Reminder' , NameAr = 'Opinion Notes Reminder' where EventId = 56
update NOTIF_NOTIFICATION_EVENT set NameEn = 'Case Close Reminder' , NameAr = 'Case Close Reminder' where EventId = 58
update NOTIF_NOTIFICATION_EVENT set NameEn = 'Legal Notification Reminder' , NameAr = 'Legal Notification Reminder' where EventId = 59
update NOTIF_NOTIFICATION_EVENT set NameEn = 'Legal Notification Response Reminder' , NameAr = 'Legal Notification Response Reminder' where EventId = 61

delete from NOTIF_NOTIFICATION_TEMPLATE where EventId in (55,60)
delete from NOTIF_NOTIFICATION_EVENT where EventId in (55,60)

--------------- 26-8-2024 -----------------------


---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------- LMS STOCKTAKING START -------------------------------------------------------------
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

------------------------------
INSERT INTO LMS_STOCKTAKING_STATUS_LKP(Id , NameEn , NameAr,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,DeletedBy,DeletedDate,IsDeleted) VALUES(1, 'New',N'New','fatwaadmin@gmail.com',GETDATE(),null,null,null,null,0)
INSERT INTO LMS_STOCKTAKING_STATUS_LKP(Id , NameEn , NameAr,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,DeletedBy,DeletedDate,IsDeleted) VALUES(2, 'In Progress',N'In Progress','fatwaadmin@gmail.com',GETDATE(),null,null,null,null,0)
INSERT INTO LMS_STOCKTAKING_STATUS_LKP(Id , NameEn , NameAr,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,DeletedBy,DeletedDate,IsDeleted) VALUES(4, 'Completed',N'Completed','fatwaadmin@gmail.com',GETDATE(),null,null,null,null,0)


---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------- LMS STOCKTAKING End -------------------------------------------------------------
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

----02-sep-2024
update WS_COMM_COMMUNICATION_TYPES set NameAr=N'قيد المراجعة' where CommunicationTypeId=1 --Inreview
update WS_COMM_COMMUNICATION_TYPES set NameAr=N'تم الرفض' where CommunicationTypeId=4-- rejected
update WS_COMM_COMMUNICATION_TYPES set NameAr=N'اعتمد' where CommunicationTypeId=8 --Approve

update WS_COMM_COMMUNICATION_TYPES set NameAr=N'الكلية' where CommunicationTypeId=16 -- Regional
update WS_COMM_COMMUNICATION_TYPES set NameAr=N'الإستئناف' where CommunicationTypeId=32 -- Appeal
update WS_COMM_COMMUNICATION_TYPES set NameAr=N'التمييز' where CommunicationTypeId=64 -- Supreme
--------Registered Case Transfer Request-------
INSERT INTO CMS_RESGISTERED_CASE_TRANSFER_REQUEST_STATUS ([Id],[NameEn],[NameAr]) VALUES(1,'Approved',N'معتمد')
-------
INSERT INTO CMS_RESGISTERED_CASE_TRANSFER_REQUEST_STATUS ([Id],[NameEn],[NameAr]) VALUES(2,'Rejected',N'رفض')
-------
INSERT INTO CMS_RESGISTERED_CASE_TRANSFER_REQUEST_STATUS ([Id],[NameEn],[NameAr]) VALUES(3,'Submitted',N'الطلبات المرسلة')
---------
INSERT INTO [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) 
VALUES (132, N'Registered Case Transfer Request', N'Registered Case Transfer Request', N'System Generated', GETDATE(), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)

INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) 
VALUES (NEWID(), 132, 4, N'Registered Case Transfer Request', N'Registered Case Transfer Request', N'Registered Case Request Transfer Successfully', N'Registered Case Request Transfer Successfully', N'Resgistered Case Transfer Request has been sent by #Sender Name# on #Created Date#', N'Resgistered Case Transfer Request has been sent by #Sender Name# on #Created Date#', NULL, N'fatwaadmin@gmail.com', GETDATE(), N'fatwaadmin@gmail.com', NULL, NULL, NULL, 0, 1)

INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP ([PlaceHolderId],[PlaceHolderName],[EventId]) VALUES (167,'#Sender Name#',132)

INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP ([PlaceHolderId],[PlaceHolderName],[EventId]) VALUES (168,'#Created Date#',132)
----------
INSERT INTO [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) 
VALUES (135, N'Reject To Accept Case Transfer Request', N'Reject To Accept Case Transfer Request', N'System Generated', GETDATE(), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)

INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) 
VALUES (NEWID(), 135, 4, N'Reject To Accept Case Transfer Request', N'Reject To Accept Case Transfer Request', N'Case Request for Transfer is Rejected', N'Case Request for Transfer is Rejected', N'Registered Case Request For Transfer Sector has been Rejected send by #Sender Name#  on #Created Date#', N'Registered Case Request For Transfer Sector has been Rejected send by #Sender Name#  on #Created Date#', NULL, N'fatwaadmin@gmail.com', GETDATE(), N'fatwaadmin@gmail.com', NULL, NULL, NULL, 0, 1)
-----------
INSERT INTO [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) 
VALUES (136, N'Approved Case Transfer Request', N'Approved Case Transfer Request', N'System Generated', GETDATE(), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)

INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) 
VALUES (NEWID(), 136, 4, N'Approved Case Transfer Request', N'Approved Case Transfer Request', N'Case Request for Transfer is Approved', N'Case Request for Transfer is Approved', N'Registered Case Request For Transfer Sector has been Approved send by #Sender Name#  on #Created Date#', N'Registered Case Request For Transfer Sector has been Approved send by #Sender Name#  on #Created Date#', NULL, N'fatwaadmin@gmail.com', GETDATE(), N'fatwaadmin@gmail.com', NULL, NULL, NULL, 0, 1)
-------
INSERT INTO UMS_CLAIM ([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted],[ModuleId])
VALUES('Registered Case Transfer Request View','Case_Management_System','Registered_Case','Permission','Permissions.CMS.RegisteredCase.TransferRequest.View','Registered Case Transfer Request View',0,2)
--------
INSERT INTO UMS_GROUP_CLAIMS ([GroupId],[ClaimType],[ClaimValue])
VALUES ('49744B64-2399-4097-94FD-326D6FCE2626', 'Permission', 'Permissions.CMS.RegisteredCase.TransferRequest.View')
-----
INSERT INTO UMS_GROUP_CLAIMS ([GroupId],[ClaimType],[ClaimValue])
VALUES ('4A6C0AA1-D091-4C01-B6F3-4CDD3780A06A', 'Permission', 'Permissions.CMS.RegisteredCase.TransferRequest.View')
--------

-----------------03/September/2024---------

update NOTIF_NOTIFICATION_TEMPLATE set 
NameEn='Claim Statement and Defense Note Reminder',
NameAr= N'Claim Statement and Defense Note Reminder',
SubjectEn='Claim Statement & Defense Note Reminder',
SubjectAr='Claim Statement & Defense Note Reminder',
BodyEn=N'Reminder:#Type# has not yet been created for #Entity# number #Reference Number#',
BodyAr=N'Reminder:#Type# has not yet been created for #Entity# number #Reference Number#'
where EventId=53

update NOTIF_NOTIFICATION_TEMPLATE set 
NameEn='Register a Case in MOJ Reminder',
NameAr= N'Register a Case in MOJ Reminder',
SubjectEn='Register a Case in MOJ Reminder',
SubjectAr=N'Register a Case in MOJ Reminder',
BodyEn=N'Reminder: The case for file number #File Number# is not yet registered in MOJ',
BodyAr=N'Reminder: The case for file number #File Number# is not yet registered in MOJ'
where EventId=54

update NOTIF_NOTIFICATION_TEMPLATE set 
NameEn='Opinion Note Reminder',
NameAr= N'Opinion Note Reminder',
SubjectEn='Opinion Note Reminder',
SubjectAr=N'Opinion Note Reminder',
BodyEn=N'REMINDER: Opinion Note has not yet been created for case number #Case Number#',
BodyAr=N'REMINDER: Opinion Note has not yet been created for case number #Case Number#'
where EventId=56

update NOTIF_NOTIFICATION_TEMPLATE set 
NameEn='Draft Modification Reminder',
NameAr= N'Draft Modification Reminder',
SubjectEn='Modify draft after rejection',
SubjectAr=N'Modify draft after rejection', 
BodyEn=N'Reminder: Need to modify the draft document #Type# that sent back by #Sender Name# on #Created Date# for #Entity# #Reference Number#',
BodyAr=N'Reminder: Need to modify the draft document #Type# that sent back by #Sender Name# on #Created Date# for #Entity# #Reference Number#'
where EventId=57

update NOTIF_NOTIFICATION_TEMPLATE set 
NameEn='Case Close Reminder',
NameAr= N'Case Close Reminder',
SubjectEn='Notifying to HOS and Vice HOS about Case Closing',
SubjectAr=N'Notifying to HOS and Vice HOS about Case Closing', 
BodyEn=N'Reminder: #Duration# day(s) left to close the case number #Case Number#',
BodyAr=N'Reminder: #Duration# day(s) left to close the case number #Case Number#'
where EventId=58

update NOTIF_NOTIFICATION_TEMPLATE set 
NameEn='Legal Notification Reminder',
NameAr= N'Legal Notification Reminder',
SubjectEn='No Additional Information has been created',
SubjectAr=N'No Additional Information has been created', 
BodyEn=N'Reminder: #Type# Legal Notification has not yet been submitted for #Entity# #Reference Number#',
BodyAr=N'Reminder: #Type# Legal Notification has not yet been submitted for #Entity# #Reference Number#'
where EventId=59

update NOTIF_NOTIFICATION_TEMPLATE set 
NameEn='Legal Notification Response Reminder',
NameAr= N'Legal Notification Response Reminder',
SubjectEn='Legal Notification Response Reminder',
SubjectAr=N'Legal Notification Response Reminder', 
BodyEn=N'Reminder: Need to respond to the legal notification submitted by "#GE Name#" for case number #Case Number#',
BodyAr=N'Reminder: Need to respond to the legal notification submitted by "#GE Name#" for case number #Case Number#'
where EventId=61

update NOTIF_NOTIFICATION_TEMPLATE set 
NameEn='Review Draft Reminder',
NameAr= N'Review Draft Reminder',
SubjectEn='Review Draft Reminder',
SubjectAr=N'Review Draft Reminder', 
BodyEn=N'Reminder: Need to review the draft document #Type# submitted by #Sender Name# on #Created Date# for case number #Reference Number#',
BodyAr=N'Reminder: Need to review the draft document #Type# submitted by #Sender Name# on #Created Date# for case number #Reference Number#'
where EventId=62


delete from NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP where EventId=58

INSERT INTO NOTIF_NOTIFICATION_EVENT ( EventId, NameEn, NameAr , CreatedBy , CreatedDate, ReceiverTypeId , IsDeleted)
VALUES (137,  'Case Close reminder for Appeal/Supreme' , 'Case Close reminder for Appeal/Supreme',  'System Generated' ,GETDATE(), 1 , 0);

INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr],
[SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) 
VALUES (NEWID(),137, 4, N'Case Close reminder for Appeal/Supreme', N'Case Close reminder for Appeal/Supreme',
N'Notifying to HOS and Vice HOS about Case Closing', N'Notifying to HOS and Vice HOS about Case Closing',
N'Reminder: #Duration# day(s) left for #Sector To# on case number #Case Number#', 
N'Reminder: #Duration# day(s) left for #Sector To# on case number #Case Number#', NULL, N'System Generated', GETDATE(), NULL, NULL, NULL, NULL, 0)

IF NOT EXISTS (
    SELECT 1
    FROM NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP
    WHERE PlaceHolderName = N'#Case Number#' AND EventId = 58
)
BEGIN
    INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP (PlaceHolderId, PlaceHolderName, EventId) 
    VALUES (((SELECT Max(PlaceHolderId) FROM NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP )+1), N'#Case Number#', 58)
END
IF NOT EXISTS (
    SELECT 1
    FROM NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP
    WHERE PlaceHolderName = N'#Duration#' AND EventId = 58
)
BEGIN
    INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP (PlaceHolderId, PlaceHolderName, EventId) 
    VALUES (((SELECT Max(PlaceHolderId) FROM NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP )+1), N'#Duration#', 58)
END
IF NOT EXISTS (
    SELECT 1
    FROM NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP
    WHERE PlaceHolderName = N'#Entity#' AND EventId = 59
)
BEGIN
    INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP (PlaceHolderId, PlaceHolderName, EventId) 
    VALUES (((SELECT Max(PlaceHolderId) FROM NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP )+1), N'#Entity#', 59)
END

IF NOT EXISTS (
    SELECT 1
    FROM NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP
    WHERE PlaceHolderName = N'#Type#' AND EventId = 59
)
BEGIN
    INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP (PlaceHolderId, PlaceHolderName, EventId) 
    VALUES (((SELECT Max(PlaceHolderId) FROM NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP )+1), N'#Type#', 59)
END
IF NOT EXISTS (
    SELECT 1
    FROM NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP
    WHERE PlaceHolderName = N'#Reference Number#' AND EventId = 59
)
BEGIN
    INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP (PlaceHolderId, PlaceHolderName, EventId) 
    VALUES (((SELECT Max(PlaceHolderId) FROM NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP )+1), N'#Reference Number#', 59)
END

IF NOT EXISTS (
    SELECT 1
    FROM NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP
    WHERE PlaceHolderName = N'#Type#' AND EventId = 62
)
BEGIN
    INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP (PlaceHolderId, PlaceHolderName, EventId) 
    VALUES (((SELECT Max(PlaceHolderId) FROM NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP )+1), N'#Type#', 62)
END
IF NOT EXISTS (
    SELECT 1
    FROM NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP
    WHERE PlaceHolderName = N'#Reference Number#' AND EventId = 62
)
BEGIN
    INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP (PlaceHolderId, PlaceHolderName, EventId) 
    VALUES (((SELECT Max(PlaceHolderId) FROM NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP )+1), N'#Reference Number#', 62)
END
IF NOT EXISTS (
    SELECT 1
    FROM NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP
    WHERE PlaceHolderName = N'#Sender Name#' AND EventId = 62
)
BEGIN
    INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP (PlaceHolderId, PlaceHolderName, EventId) 
    VALUES (((SELECT Max(PlaceHolderId) FROM NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP )+1), N'#Sender Name#', 62)
END
IF NOT EXISTS (
    SELECT 1
    FROM NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP
    WHERE PlaceHolderName = N'#Created Date#' AND EventId = 62
)
BEGIN
    INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP (PlaceHolderId, PlaceHolderName, EventId) 
    VALUES (((SELECT Max(PlaceHolderId) FROM NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP )+1), N'#Created Date#', 62)
END

IF NOT EXISTS (
    SELECT 1
    FROM NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP
    WHERE PlaceHolderName = N'#Type#' AND EventId = 57
)
BEGIN
    INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP (PlaceHolderId, PlaceHolderName, EventId) 
    VALUES (((SELECT Max(PlaceHolderId) FROM NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP )+1), N'#Type#', 57)
END

IF NOT EXISTS (
    SELECT 1
    FROM NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP
    WHERE PlaceHolderName = N'#Entity#' AND EventId = 57
)
BEGIN
    INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP (PlaceHolderId, PlaceHolderName, EventId) 
    VALUES (((SELECT Max(PlaceHolderId) FROM NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP )+1), N'#Entity#', 57)
END
IF NOT EXISTS (
    SELECT 1
    FROM NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP
    WHERE PlaceHolderName = N'#Reference Number#' AND EventId = 57
)
BEGIN
    INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP (PlaceHolderId, PlaceHolderName, EventId) 
    VALUES (((SELECT Max(PlaceHolderId) FROM NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP )+1), N'#Reference Number#', 57)
END

IF NOT EXISTS (
    SELECT 1
    FROM NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP
    WHERE PlaceHolderName = N'#Reference Number#' AND EventId = 53
)
BEGIN
    INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP (PlaceHolderId, PlaceHolderName, EventId) 
    VALUES (((SELECT Max(PlaceHolderId) FROM NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP )+1), N'#Reference Number#', 53)
END

IF NOT EXISTS (
    SELECT 1
    FROM NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP
    WHERE PlaceHolderName = N'#Entity#' AND EventId = 53
)
BEGIN
    INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP (PlaceHolderId, PlaceHolderName, EventId) 
    VALUES (((SELECT Max(PlaceHolderId) FROM NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP )+1), N'#Entity#', 53)
END
--
IF NOT EXISTS (
    SELECT 1
    FROM NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP
    WHERE PlaceHolderName = N'#Case Number#' AND EventId = 137
)
BEGIN
    INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP (PlaceHolderId, PlaceHolderName, EventId) 
    VALUES (((SELECT Max(PlaceHolderId) FROM NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP )+1), N'#Case Number#', 137)
END
IF NOT EXISTS (
    SELECT 1
    FROM NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP
    WHERE PlaceHolderName = N'#Duration#' AND EventId = 137
)
BEGIN
    INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP (PlaceHolderId, PlaceHolderName, EventId) 
    VALUES (((SELECT Max(PlaceHolderId) FROM NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP )+1), N'#Duration#', 137)
END

IF NOT EXISTS (
    SELECT 1
    FROM NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP
    WHERE PlaceHolderName = N'#Sector To#' AND EventId = 137
)
BEGIN
    INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP (PlaceHolderId, PlaceHolderName, EventId) 
    VALUES (((SELECT Max(PlaceHolderId) FROM NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP )+1), N'#Sector To#', 137)
END


IF NOT EXISTS (
    SELECT 1
    FROM NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP
    WHERE PlaceHolderName = N'#Case Number#' AND EventId = 61
)
BEGIN
    INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP (PlaceHolderId, PlaceHolderName, EventId) 
    VALUES (((SELECT Max(PlaceHolderId) FROM NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP )+1), N'#Case Number#', 61)
END
IF NOT EXISTS (
    SELECT 1
    FROM NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP
    WHERE PlaceHolderName = N'GE Name#' AND EventId = 61
)
BEGIN
    INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP (PlaceHolderId, PlaceHolderName, EventId) 
    VALUES (((SELECT Max(PlaceHolderId) FROM NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP )+1), N'GE Name#', 61)
END
--------------- 26-8-2024 -----------------------

------- author Arshad khan (29/08/24)
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LMS_LITERATURE_BARCODE]') AND type in (N'U'))
	Update [LMS_LITERATURE_BARCODE] Set [RFIDValue] = 0
GO
----

---------------------
INSERT INTO UMS_CLAIM ([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted],[ModuleId])
VALUES('Stock Taking Report List','Library_Management_System','Literatures','Permission','Permissions.Submenu.LMS.StockTaking.List','Stock Taking Report List',0,512)

----------------
 INSERT INTO UMS_USER_CLAIMS VALUES('9779af5f-e7a4-4ebf-9af0-52beb251b4eb','Permission','Permissions.Submenu.LMS.StockTaking.List','fatwaadmin@gmail.com','2024-09-04 12:11:18.160',null,null,null,null,0,null)
--------

  
IF EXISTS (SELECT ModuleConditionId FROM WF_CONDITION_PR_LKP WHERE ModuleConditionId = 161 ) 
BEGIN
	UPDATE WF_CONDITION_PR_LKP SET Name = 'If_Draft_Document_is_Submitted_By_ViceHOS' where ModuleConditionId = 161
    PRINT 'Record Updated';
END
else
BEGIN
    PRINT 'Record Does Not Exist';
END

IF NOT EXISTS (SELECT Id FROM WF_TRIGGER_CONDITION_PR WHERE ConditionId = 161 AND TriggerId = 4) 
BEGIN
	INSERT INTO WF_TRIGGER_CONDITION_PR VALUES(4,161)
    PRINT 'Record Inserted';
END


SET IDENTITY_INSERT WF_ACTIVITY_PR_LKP ON
INSERT [dbo].WF_ACTIVITY_PR_LKP([ActivityId],[Name],[Class],[Method],[CategoryId],[AKey],[IsEndofFlow]) 
VALUES (40,N'Sign_Draft_Publish_Draft_And_End_Flow','WorkflowImplementationService','SignDraftPublishDraftand_EndFlow',2,'SignDraftPublishDraftandEndFlow',1)
SET IDENTITY_INSERT WF_ACTIVITY_PR_LKP OFF


IF NOT EXISTS (SELECT Id FROM WF_TRIGGER_ACTIVITY_PR WHERE TriggerId = 4 AND ActivityId = 40) 
BEGIN
	INSERT INTO WF_TRIGGER_ACTIVITY_PR VALUES (4,40)
    PRINT 'Record Inserted';
END


SET IDENTITY_INSERT WF_PARAMETER_PR_LKP ON
INSERT [dbo].WF_PARAMETER_PR_LKP([ParameterId],[Name], [PKey],[Mandatory],[IsAutoPopulated])
VALUES (65,N'User', N'SignDraftPublishDraftandEndFlow_User',0,0)
SET IDENTITY_INSERT WF_PARAMETER_PR_LKP OFF

SET IDENTITY_INSERT WF_PARAMETER_PR_LKP ON
INSERT [dbo].WF_PARAMETER_PR_LKP([ParameterId],[Name], [PKey],[Mandatory],[IsAutoPopulated])
VALUES (66,N'User Role', N'SignDraftPublishDraftandEndFlow_UserRole',0,0)
SET IDENTITY_INSERT WF_PARAMETER_PR_LKP OFF



IF NOT EXISTS (SELECT * FROM WF_ACTIVITY_PARAMETERS_PR WHERE [ParameterId] = 65 AND [ActivityId] = 40) 
BEGIN
	INSERT [dbo].WF_ACTIVITY_PARAMETERS_PR([ParameterId],[ActivityId]) VALUES (65,40)
    PRINT 'Record Inserted';
END


UPDATE WF_ACTIVITY_PR_LKP SET Name = 'Sign_Draft_Publish_Draft_Send_To_G2G_And_End_Flow', Method = 'SignDraftPublishDraftSendtoG2Gand_EndFlow', AKey = 'SignDraftPublishDraftSendtoG2GandEndFlow' where ActivityId = 29
UPDATE WF_PARAMETER_PR_LKP SET [PKey] = 'SignDraftPublishDraftSendtoG2GandEndFlow_User' WHERE  ParameterId = 51
UPDATE WF_PARAMETER_PR_LKP SET [PKey] = 'SignDraftPublishDraftSendtoG2GandEndFlow_UserRole' WHERE  ParameterId = 52


IF NOT Exists(SELECT 1 from CMS_TEMPLATE_PARAMETER where ParameterId = 36)
BEGIN
SET IDENTITY_INSERT CMS_TEMPLATE_PARAMETER ON
	INSERT INTO [dbo].CMS_TEMPLATE_PARAMETER (ParameterId,[Name],PKey,Mandatory,IsAutoPopulated,ModuleId,IsActive)
		VALUES (36, N'Representative Name', N'CmsTempRepresentativeName',0,1,5,1)
SET IDENTITY_INSERT CMS_TEMPLATE_PARAMETER OFF
END
GO


------------- 10/09/2024---------------

IF  Exists(SELECT 1 from NOTIF_NOTIFICATION_TEMPLATE where TemplateId='07989300-2042-4256-B9ED-BC7741B9DB1F' )
	update NOTIF_NOTIFICATION_TEMPLATE 
	set BodyEn ='#Type# Document for file number #File Number# has been sent to MOJ messenger for case registration by #Sender Name# on #Created Date#', 
	BodyAr =N'تم إرسال مستند #Type# إلى مندوب اطلاع المحاكم للملف رقم #File Number# لتسجيل القضية من قبل #Sender Name# بتاريخ #Created Date#'
	where TemplateId='07989300-2042-4256-B9ED-BC7741B9DB1F'
GO

IF  Exists(SELECT 1 from NOTIF_NOTIFICATION_TEMPLATE where TemplateId='3CB64213-6DE3-4FF6-A1C5-C88DE421FA9D')
	update NOTIF_NOTIFICATION_TEMPLATE 
	set BodyEn ='Reminder: The case has not been registered for file number #File Number# at MOJ' , 
	BodyAr =N'تنبيه: لم يتم تسجيل القضية للملف رقم #File Number# في وزارة العدل'
	where TemplateId='3CB64213-6DE3-4FF6-A1C5-C88DE421FA9D'
GO
------------- 10/09/2024---------------


-----------17 sep 2024

Insert into LMS_BORROW_LITERATURE_Event_LKP VALUES(1,'Borrowed',N'Borrowed' )

Insert into LMS_BORROW_LITERATURE_Event_LKP VALUES(2,'Borrow Request Rejected',N'Borrow Request Rejected' )
Insert into LMS_BORROW_LITERATURE_Event_LKP VALUES(4,'Borrowing Period Expired',N'Borrowing Period Expired ' )
Insert into LMS_BORROW_LITERATURE_Event_LKP VALUES(8,'Borrow Request Pending For Approval',N'Borrow Request Pending For Approval' )
Insert into LMS_BORROW_LITERATURE_Event_LKP VALUES(16,'Borrowed',N'Borrowed')
Insert into LMS_BORROW_LITERATURE_Event_LKP VALUES(32,'Return',N'Return' )
Insert into LMS_BORROW_LITERATURE_Event_LKP VALUES(64,'Pending For Extension Approval',N'Pending For Extension Approval' )
Insert into LMS_BORROW_LITERATURE_Event_LKP VALUES(128,'Extended',N'Borrow Request Approve' )
Insert into LMS_BORROW_LITERATURE_Event_LKP VALUES(256,'Extension Rejected',N'Extension Rejected' )

Insert into LMS_BORROW_LITERATURE_Event_LKP VALUES(512,'Extending Period Expired',N'Extending Period Expired' )

Insert into LMS_BORROW_LITERATURE_Event_LKP VALUES(1024,'Literature Request Return Reject',N'Literature Request Return Reject' )

Insert into LMS_BORROW_LITERATURE_Event_LKP VALUES(2048,'Pending For Return Book Approval',N'Pending For Return Book Approval' )

ALTER TABLE LMS_LITERATURE_DETAILS
ADD Description nvarchar(max) null;


ALTER TABLE LMS_LITERATURE_DETAILS
DROP COLUMN Description;

IF((SELECT COUNT(*) FROM UMS_CLAIM WHERE ClaimValue = 'Permissions.Submenu.LMS.Literatures.ReturnBorrowExtend') = 0)
BEGIN
	INSERT INTO UMS_CLAIM (Title_En, Title_Ar, Module, SubModule, ClaimType, ClaimValue, ModuleId, IsDeleted)
	VALUES('Borrow Return And Extend the Book',N'Borrow Return And Extend the Book','Library_Management_System','Literatures','Permission','Permissions.Submenu.LMS.Literatures.ReturnBorrowExtend',512,0)
END

-- Assign the group which will have this permission of doing these actions
INSERT INTO UMS_GROUP_CLAIMS VALUES ('071C4BA0-CC3F-43DC-B2CB-6FC2BEA6D831','Permission','Permissions.Submenu.LMS.Literatures.ReturnBorrowExtend')

update CMS_TEMPLATE set Content = '<p style="font-family: Arial; font-size: 13px"><br></p>' where id = 1


---------------------------26/09/24------------------
INSERT INTO UMS_CLAIM (Title_En , Module , SubModule , ClaimType , ClaimValue ,Title_Ar ,IsDeleted , ModuleId)
VALUES('LMS Literature Add StockTaking','Library_Management_System','Literatures','Permission' ,'Permissions.LMS.Literatures.AddStockTaking','LMS Literature Add StockTaking',0,512)

INSERT INTO UMS_CLAIM (Title_En , Module , SubModule , ClaimType , ClaimValue ,Title_Ar ,IsDeleted , ModuleId)
VALUES('LMS Literature List StockTaking','Library_Management_System','Literatures','Permission' ,'Permissions.LMS.Literatures.ListStockTaking','LMS Literature List StockTaking',0,512)

INSERT INTO UMS_CLAIM (Title_En , Module , SubModule , ClaimType , ClaimValue ,Title_Ar ,IsDeleted , ModuleId)
VALUES('LMS Literature View StockTaking','Library_Management_System','Literatures','Permission' ,'Permissions.LMS.Literatures.ViewStockTaking','LMS Literature View StockTaking',0,512)

-- <History Author = 'Ammaar Naveed' Date='2024-09-17' Version="1.0" Branch="master"> Viewable literature list sidemenu permissions</History>
INSERT INTO UMS_CLAIM (Title_En, Title_Ar, Module, SubModule, ClaimType, ClaimValue, IsDeleted, ModuleId)
VALUES('Viewable Literature List', N'قائمة الأدبيات المتاحة', 'Library_Management_System', 'Literatures', 'Permission', 'Permissions.Submenu.LMS.ViewableLiteratureList',0,512 )

-----26/09/2024 Hasan
update NOTIF_NOTIFICATION_EVENT set NameEn = 'Books Borrowing Period Extended',NameAr = N'تمديد فترة استعارة الكتب' where EventId = 125

update NOTIF_NOTIFICATION_EVENT set NameEn = 'Borrowed Books Returned' ,NameAr = N'ارجاع الكتب المستعارة'where EventId = 124

update NOTIF_NOTIFICATION_EVENT set NameEn = 'Books Borrowed' ,NameAr = N'استعارة الكتب' where EventId = 70

---29 SEP
 INSERT INTO LMS_BOOK_STATUS VALUES (32,'Not Borrowable',N'Not Borrowable')
update NOTIF_NOTIFICATION_EVENT set NameEn = 'Books Borrowed' ,NameAr = N'استعارة الكتب' where EventId = 70


----------------
-------------Description Update in Lookups History

UPDATE LOOKUPS_HISTORY set Description = (SELECT Description from LMS_LITERATURE_TAG where Id = LookupsId) 
where LookupsTableId = 12 
AND LookupsId 
IN(
Select Id from LMS_LITERATURE_TAG)AND '2024-09-29 13:02:53.220' BETWEEN StartDate AND ISNULL(EndDate , '2024-09-29 13:02:53.220') AND Description IS NULL
-------------Tag No Update in Lookups History
UPDATE LOOKUPS_HISTORY set TagNo = (SELECT TagNo from LMS_LITERATURE_TAG where Id = LookupsId) 
where LookupsTableId = 12 
AND LookupsId 
IN(
Select Id from LMS_LITERATURE_TAG)AND '2024-09-29 13:02:53.220' BETWEEN StartDate AND ISNULL(EndDate , '2024-09-29 13:02:53.220') AND TagNo IS NULL
 INSERT INTO LMS_BOOK_STATUS VALUES (32,'Not Borrowable',N'Not Borrowable')


 ----------------------------------------------------

 
 UPDATE  LMS_STOCKTAKING_STATUS_LKP SET NameAr=N'جديد' WHERE Id=1
 
 UPDATE  LMS_STOCKTAKING_STATUS_LKP SET NameAr=N'قيد العمل' WHERE Id=2
 
 UPDATE  LMS_STOCKTAKING_STATUS_LKP SET NameAr=N'مكتمل' WHERE Id=4

 update LMS_BORROW_LITERATURE_EVENT_LKP set NameEn ='Borrowed & Delivered ', NameAr = N'استعارة وتسليم الكتاب تم' where Id =1


update LMS_BORROW_LITERATURE_EVENT_LKP set NameEn ='Returned', NameAr = N'تم ارجاع الكتاب' where Id =32

update LMS_BORROW_LITERATURE_EVENT_LKP set NameEn ='Borrowing Period Extended', NameAr = N' تم تمديد فترة الاستعارة' where Id = 128




IF NOT EXISTS(SELECT 1 FROM [CMS_OPERATING_SECTOR_TYPE_G2G_LKP] WHERE Id = 55)
BEGIN
	SET IDENTITY_INSERT [CMS_OPERATING_SECTOR_TYPE_G2G_LKP] ON
INSERT INTO CMS_OPERATING_SECTOR_TYPE_G2G_LKP 
(ID,Code, Name_En, Name_Ar, IsActive, DepartmentId, ParentId,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,DeletedBy,DeletedDate,IsDeleted,ModuleId,BuildingId,FloorId,IsOnlyViceHosApprovalRequired,IsViceHosResponsibleForAllLawyers,G2GBRSiteID)
VALUES (55, 'LCC','Legal Cultural Center',N'المركز الثقافي القانوني',1,2,20,'fatwaadmin@gmail.com',GETDATE(),NULL,NULL,NULL,NULL,0,NULL,1,1,0,0,NULL);
SET IDENTITY_INSERT [CMS_OPERATING_SECTOR_TYPE_G2G_LKP] OFF
END
Go


-----------02/10/2024
IF NOT EXISTS (SELECT 1 FROM NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP WHERE PlaceHolderName = N'#Reference Number#' AND EventId = 26)
BEGIN
    INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP (placeholderid, PlaceHolderName, EventId)
    VALUES ((SELECT MAX(placeholderid) FROM NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP) + 1, N'#Reference Number#', 26);
END;
Go

-- <History Author = 'Ammaar Naveed' Date='2024-10-02' Version="1.0" Branch="master">Sector users list in permissions grid</History>
INSERT INTO UMS_CLAIM(Title_En, Module, SubModule, ClaimType, ClaimValue, Title_Ar, IsDeleted, ModuleId)
VALUES('Sector Users', 'Consultation_Management_System', 'Users_List', 'Permission', 'Permissions.COMS.SectorUsersList', N'قائمة مستخدمي القطاع', 0, 4)

-- <History Author = 'Ammaar Naveed' Date='2024-10-03' Version="1.0" Branch="master">Create draft document for consultation (permission)</History>
INSERT INTO UMS_CLAIM(Title_En, Title_Ar, Module, SubModule, ClaimType, ClaimValue, IsDeleted, ModuleId)
VALUES('Create Consultation Draft Document', N'إنشاء مسودة مستند', 'Consultation_Management_System', 'Draft_Document','Permission','Permissions.COMS.DraftDocument.Create',0,4)
Go
------------------------
INSERT INTO LMS_STOCKTAKING_EVENT_LKP (Id , NameEn , NameAr,CreatedBy,CreatedDate,IsDeleted) VALUES(1,'In Progress',N'في تَقَدم','fatwaadmin@gmail.com',GetDate(),0)
INSERT INTO LMS_STOCKTAKING_EVENT_LKP (Id , NameEn , NameAr,CreatedBy,CreatedDate,IsDeleted) VALUES(2,'Completed',N'مكتمل','fatwaadmin@gmail.com',GetDate(),0)
INSERT INTO LMS_STOCKTAKING_EVENT_LKP (Id , NameEn , NameAr,CreatedBy,CreatedDate,IsDeleted) VALUES(4,'Deleted',N'تم الحذف','fatwaadmin@gmail.com',GetDate(),0)



------------06/10/2024--------

IF NOT EXISTS ( SELECT 1 FROM NOTIF_NOTIFICATION_EVENT WHERE EventId = 140)
BEGIN
    INSERT INTO NOTIF_NOTIFICATION_EVENT (EventId, NameEn, NameAr, CreatedBy, CreatedDate, ReceiverTypeId, IsDeleted)
    VALUES (140, 'Opinion Notes Reminder For Manager', 'Opinion Notes Reminder For Manager', 'System Generated', GETDATE(), 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM NOTIF_NOTIFICATION_TEMPLATE WHERE EventId = 140 AND ChannelId = 4)
BEGIN
    INSERT INTO NOTIF_NOTIFICATION_TEMPLATE (TemplateId, EventId, ChannelId, NameEn, NameAr, SubjectEn, SubjectAr, BodyEn,BodyAr, CreatedBy, CreatedDate, IsDeleted)
    VALUES (NewID(), 140, 4, 'Opinion Notes Reminder For Manager', 'Opinion Notes Reminder For Manager', 'Opinion Notes Reminder For Manager', 'Opinion Notes Reminder For Manager', 'Alert: Opinion Note has not yet been created for case number #Case Number# By #Sender Name#','Alert: Opinion Note has not yet been created for case number #Case Number# By #Sender Name#', 'System Generated', GETDATE(), 0);
END;
IF NOT EXISTS (SELECT 1 FROM NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP WHERE PlaceHolderName = N'#Case Number#' AND EventId = 140)
BEGIN
    INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP (PlaceHolderId, PlaceHolderName, EventId)
    VALUES ((SELECT MAX(PlaceHolderId) FROM NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP) + 1, N'#Case Number#', 140);
END;


IF NOT EXISTS ( SELECT 1 FROM NOTIF_NOTIFICATION_EVENT WHERE EventId = 141)
BEGIN
    INSERT INTO NOTIF_NOTIFICATION_EVENT (EventId, NameEn, NameAr, CreatedBy, CreatedDate, ReceiverTypeId, IsDeleted)
    VALUES (141, 'ClaimStatement & Defense Letter Reminder For Manager', 'ClaimStatement & Defense Letter Reminder For Manager', 'System Generated', GETDATE(), 1, 0);
END;


IF NOT EXISTS (SELECT 1 FROM NOTIF_NOTIFICATION_TEMPLATE WHERE EventId = 141 AND ChannelId = 4)
BEGIN
    INSERT INTO NOTIF_NOTIFICATION_TEMPLATE (TemplateId, EventId, ChannelId, NameEn, NameAr, SubjectEn, SubjectAr, BodyEn,BodyAr, CreatedBy, CreatedDate, IsDeleted)
    VALUES (NewID(), 141, 4, 'ClaimStatement & Defense Letter Reminder For Manager', 'ClaimStatement & Defense Letter Reminder For Manager', 'ClaimStatement & Defense Letter Reminder For Manager', 'ClaimStatement & Defense Letter Reminder For Manager', 'Alert: #Type# has not yet been created for #Entity# number #Reference Number# By #Sender Name#','Alert: #Type# has not yet been created for #Entity# number #Reference Number# By #Sender Name#', 'System Generated', GETDATE(), 0);
END;

IF NOT EXISTS (SELECT 1 FROM NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP WHERE PlaceHolderName = N'#Reference Number#' AND EventId = 141)
BEGIN
    INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP (PlaceHolderId, PlaceHolderName, EventId)
    VALUES ((SELECT MAX(PlaceHolderId) FROM NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP) + 1, N'#Reference Number#', 141);
END;
IF NOT EXISTS (SELECT 1 FROM NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP WHERE PlaceHolderName = N'#Type#' AND EventId = 141)
BEGIN
    INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP (PlaceHolderId, PlaceHolderName, EventId)
    VALUES ((SELECT MAX(PlaceHolderId) FROM NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP) + 1, N'#Type#', 141);
END;

IF NOT EXISTS ( SELECT 1 FROM NOTIF_NOTIFICATION_EVENT WHERE EventId = 142)
BEGIN
    INSERT INTO NOTIF_NOTIFICATION_EVENT (EventId, NameEn, NameAr, CreatedBy, CreatedDate, ReceiverTypeId, IsDeleted)
    VALUES (142, 'Legal Notification Response Reminder For Manager', 'Legal Notification Response Reminder For Manager', 'System Generated', GETDATE(), 1, 0);
END;


IF NOT EXISTS (SELECT 1 FROM NOTIF_NOTIFICATION_TEMPLATE WHERE EventId = 142 AND ChannelId = 4)
BEGIN
    INSERT INTO NOTIF_NOTIFICATION_TEMPLATE (TemplateId, EventId, ChannelId, NameEn, NameAr, SubjectEn, SubjectAr, BodyEn,BodyAr, CreatedBy, CreatedDate, IsDeleted)
    VALUES (NewID(), 142, 4, 'Legal Notification Response Reminder For Manager', 'Legal Notification Response Reminder For Manager', 'Legal Notification Response Reminder For Manager', 'Legal Notification Response Reminder For Manager', 'Alert: #Sender Name# has not yet responded to the legal notification submitted by #GE Name# for case number #Case Number#','Alert: #Sender Name# has not yet responded to the legal notification submitted by #GE Name#  for case number #Case Number#', 'System Generated', GETDATE(), 0);
END;

IF NOT EXISTS (SELECT 1 FROM NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP WHERE PlaceHolderName = N'GE Name#' AND EventId = 142)
BEGIN
    INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP (PlaceHolderId, PlaceHolderName, EventId)
    VALUES ((SELECT MAX(PlaceHolderId) FROM NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP) + 1, N'GE Name#', 142);
END;
IF NOT EXISTS (SELECT 1 FROM NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP WHERE PlaceHolderName = N'#Case Number#' AND EventId = 142)
BEGIN
    INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP (PlaceHolderId, PlaceHolderName, EventId)
    VALUES ((SELECT MAX(PlaceHolderId) FROM NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP) + 1, N'#Case Number#', 142);
END;

IF NOT EXISTS ( SELECT 1 FROM NOTIF_NOTIFICATION_EVENT WHERE EventId = 143)
BEGIN
    INSERT INTO NOTIF_NOTIFICATION_EVENT (EventId, NameEn, NameAr, CreatedBy, CreatedDate, ReceiverTypeId, IsDeleted)
    VALUES (143, 'Legal Notification Reminder For Manager', 'Legal Notification Reminder For Manager', 'System Generated', GETDATE(), 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM NOTIF_NOTIFICATION_TEMPLATE WHERE EventId = 143 AND ChannelId = 4)
BEGIN
    INSERT INTO NOTIF_NOTIFICATION_TEMPLATE (TemplateId, EventId, ChannelId, NameEn, NameAr, SubjectEn, SubjectAr, BodyEn,BodyAr, CreatedBy, CreatedDate, IsDeleted)
    VALUES (NewID(), 143, 4, 'Legal Notification Reminder For Manager', 'Legal Notification Reminder For Manager', 'LLegal Notification Reminder For Manager', 'Legal Notification Reminder For Manager', 'Alert! #Type# Legal Notification has not yet been submitted for #Entity# #Reference Number# by #Sender Name# ','Alert! #Type# Legal Notification has not yet been submitted for #Entity# #Reference Number# by #Sender Name#', 'System Generated', GETDATE(), 0);
END;
SELECT * FROM NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP where EventId=59

IF NOT EXISTS (SELECT 1 FROM NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP WHERE PlaceHolderName = N'#Type#' AND EventId = 143)
BEGIN
    INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP (PlaceHolderId, PlaceHolderName, EventId)
    VALUES ((SELECT MAX(PlaceHolderId) FROM NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP) + 1, N'#Type#', 143);
END;
IF NOT EXISTS (SELECT 1 FROM NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP WHERE PlaceHolderName = N'#Reference Number#' AND EventId = 143)
BEGIN
    INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP (PlaceHolderId, PlaceHolderName, EventId)
    VALUES ((SELECT MAX(PlaceHolderId) FROM NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP) + 1, N'#Reference Number#', 143);
END; 

IF NOT EXISTS ( SELECT 1 FROM NOTIF_NOTIFICATION_EVENT WHERE EventId = 144)
BEGIN
    INSERT INTO NOTIF_NOTIFICATION_EVENT (EventId, NameEn, NameAr, CreatedBy, CreatedDate, ReceiverTypeId, IsDeleted)
    VALUES (144, 'Review Draft Reminder For Manager', 'Review Draft Reminder For Manager', 'System Generated', GETDATE(), 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM NOTIF_NOTIFICATION_TEMPLATE WHERE EventId = 144 AND ChannelId = 4)
BEGIN
    INSERT INTO NOTIF_NOTIFICATION_TEMPLATE (TemplateId, EventId, ChannelId, NameEn, NameAr, SubjectEn, SubjectAr, BodyEn,BodyAr, CreatedBy, CreatedDate, IsDeleted)
    VALUES (NewID(), 144, 4, 'Review Draft Reminder For Manager', 'Review Draft Reminder For Manager', 'Review Draft Reminder For Manager', 'Review Draft Reminder For Manager', 'Alert! #Reviewer Name# Need to review the draft document #Type# submitted by #Sender Name# on #Created Date# for case number #Reference Number#','Alert! #Reviewer Name# Need to review the draft document #Type# submitted by #Sender Name# on #Created Date# for case number #Reference Number#', 'System Generated', GETDATE(), 0);
END;

IF NOT EXISTS (SELECT 1 FROM NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP WHERE PlaceHolderName = N'#Type#' AND EventId = 144)
BEGIN
    INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP (PlaceHolderId, PlaceHolderName, EventId)
    VALUES ((SELECT MAX(PlaceHolderId) FROM NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP) + 1, N'#Type#', 144);
END;
IF NOT EXISTS (SELECT 1 FROM NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP WHERE PlaceHolderName = N'#Reference Number#' AND EventId = 144)
BEGIN
    INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP (PlaceHolderId, PlaceHolderName, EventId)
    VALUES ((SELECT MAX(PlaceHolderId) FROM NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP) + 1, N'#Reference Number#', 144);
END;
IF NOT EXISTS (SELECT 1 FROM NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP WHERE PlaceHolderName = N'#Reviewer Name#' AND EventId = 144)
BEGIN
    INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP (PlaceHolderId, PlaceHolderName, EventId)
    VALUES ((SELECT MAX(PlaceHolderId) FROM NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP) + 1, N'#Reviewer Name#', 144);
END;

IF NOT EXISTS ( SELECT 1 FROM NOTIF_NOTIFICATION_EVENT WHERE EventId = 145)
BEGIN
    INSERT INTO NOTIF_NOTIFICATION_EVENT (EventId, NameEn, NameAr, CreatedBy, CreatedDate, ReceiverTypeId, IsDeleted)
    VALUES (145, 'Draft Modification Reminder For Manager', 'Draft Modification Reminder For Manager', 'System Generated', GETDATE(), 1, 0);
END;


IF NOT EXISTS (SELECT 1 FROM NOTIF_NOTIFICATION_TEMPLATE WHERE EventId = 145 AND ChannelId = 4)
BEGIN
    INSERT INTO NOTIF_NOTIFICATION_TEMPLATE (TemplateId, EventId, ChannelId, NameEn, NameAr, SubjectEn, SubjectAr, BodyEn,BodyAr, CreatedBy, CreatedDate, IsDeleted)
    VALUES (NewID(), 145, 4, 'Draft Modification Reminder For Manager', 'Draft Modification Reminder For Manager', 'Draft Modification Reminder For Manager', 'Draft Modification Reminder For Manager', 'Alert! #Reviewer Name# Need to Modify the draft document #Type# submitted by #Sender Name# on #Created Date# for #Entity# number #Reference Number#','Alert! #Reviewer Name# Need to Modify the draft document #Type# submitted by #Sender Name# on #Created Date# for #Entity# number #Reference Number#', 'System Generated', GETDATE(), 0);
END;

IF NOT EXISTS (SELECT 1 FROM NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP WHERE PlaceHolderName = N'#Type#' AND EventId = 145)
BEGIN
    INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP (PlaceHolderId, PlaceHolderName, EventId)
    VALUES ((SELECT MAX(PlaceHolderId) FROM NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP) + 1, N'#Type#', 145);
END;
IF NOT EXISTS (SELECT 1 FROM NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP WHERE PlaceHolderName = N'#Reference Number#' AND EventId = 145)
BEGIN
    INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP (PlaceHolderId, PlaceHolderName, EventId)
    VALUES ((SELECT MAX(PlaceHolderId) FROM NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP) + 1, N'#Reference Number#', 145);
END;
IF NOT EXISTS (SELECT 1 FROM NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP WHERE PlaceHolderName = N'#Reviewer Name#' AND EventId = 145)
BEGIN
    INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP (PlaceHolderId, PlaceHolderName, EventId)
    VALUES ((SELECT MAX(PlaceHolderId) FROM NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP) + 1, N'#Reviewer Name#', 145);
END;



IF NOT EXISTS ( SELECT 1 FROM NOTIF_NOTIFICATION_EVENT WHERE EventId = 146)
BEGIN
    INSERT INTO NOTIF_NOTIFICATION_EVENT (EventId, NameEn, NameAr, CreatedBy, CreatedDate, ReceiverTypeId, IsDeleted)
    VALUES (146, 'Assign To MOJ Reminder For Manager', 'Assign To MOJ Reminder For Manager', 'System Generated', GETDATE(), 1, 0);
END;


IF NOT EXISTS (SELECT 1 FROM NOTIF_NOTIFICATION_TEMPLATE WHERE EventId = 146 AND ChannelId = 4)
BEGIN
    INSERT INTO NOTIF_NOTIFICATION_TEMPLATE (TemplateId, EventId, ChannelId, NameEn, NameAr, SubjectEn, SubjectAr, BodyEn,BodyAr, CreatedBy, CreatedDate, IsDeleted)
    VALUES (NewID(), 146, 4, 'Assign To MOJ Reminder For Manager', 'Assign To MOJ Reminder For Manager', 'Assign To MOJ Reminder For Manager', 'Assign To MOJ Reminder For Manager', 'Alert! The case has not been registered for file number #File Number# at MOJ by #Sender Name#','Alert! The case has not been registered for file number #File Number# at MOJ', 'System Generated', GETDATE(), 0);
END;

IF NOT EXISTS (SELECT 1 FROM NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP WHERE PlaceHolderName = N'#File Number#' AND EventId = 146)
BEGIN
    INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP (PlaceHolderId, PlaceHolderName, EventId)
    VALUES ((SELECT MAX(PlaceHolderId) FROM NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP) + 1, N'#File Number#', 146);
END;
IF NOT EXISTS (SELECT 1 FROM NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP WHERE PlaceHolderName = N'#Reviewer Name#' AND EventId = 146)
BEGIN
    INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP (PlaceHolderId, PlaceHolderName, EventId)
    VALUES ((SELECT MAX(PlaceHolderId) FROM NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP) + 1, N'#Reviewer Name#', 146);
END;

--- Event for Adding Store AND items ( OSS Inventory Module ) ---------------------

IF NOT EXISTS(SELECT 1 From NOTIF_NOTIFICATION_EVENT where EventId = 138)
BEGIN 
insert into NOTIF_NOTIFICATION_EVENT VALUES(138, 'Add Store', N'سجل لكل', 'System Generated', CURRENT_TIMESTAMP, null, null, null, null, 0, 1, null, null,null,1);
END

IF NOT EXISTS(Select 1 From NOTIF_NOTIFICATION_TEMPLATE where EventId = 138)
BEGIN
INSERT INTO NOTIF_NOTIFICATION_TEMPLATE VALUES (NEWID(), 138, 4, 'Add Store', N'سجل لكل','Add Store', N'سجل لكل',
'A new store is created, and you are selected as a #Store Incharge# on #Created Date#',
'A new store is created, and you are selected as a #Store Incharge# on #Created Date#', null, 'fatwaadmin@gmail.com', CURRENT_TIMESTAMP, null, null, null, null,
0, 1)
END;

IF NOT EXISTS(SELECT 1 FROM NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP where EventId = 138)
BEGIN
insert NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP VALUES (185,'#Store Incharge#', 138);
END;


IF NOT EXISTS(SELECT 1 From NOTIF_NOTIFICATION_EVENT where EventId = 139)
BEGIN 
insert into NOTIF_NOTIFICATION_EVENT VALUES(139, 'Add Items', N'إضافة عناصر', 'System Generated', CURRENT_TIMESTAMP, null, null, null, null, 0, 1, null, null,null,1);
END
IF NOT EXISTS(SELECT 1 From NOTIF_NOTIFICATION_TEMPLATE where EventId = 139)
BEGIN 
INSERT INTO NOTIF_NOTIFICATION_TEMPLATE VALUES (NEWID(), 139, 4, 'Add Items', N'إضافة عناصر','Add Items', N'إضافة عناصر',
'New items have been added to the #Store Incharge# store  on #Created Date#. Please check the inventory',
'New items have been added to the #Store Incharge# store  on #Created Date#. Please check the inventory', null, 'fatwaadmin@gmail.com', CURRENT_TIMESTAMP, null, null, null, null,
0, 1)
END

IF NOT EXISTS(SELECT 1 From NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP where EventId = 139)
BEGIN 
insert NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP VALUES (186,'#Store Incharge#', 139);
END

update ttranslation set value_En = 'Store_Already_Exist_In_Selected_Building' , Value_Ar = N'المتجر موجود بالفعل في المبنى المحدد'
where TranslationId = 14441 and TranslationKey = 'Store_Already_Exist'

-- Notification Event against 100 already exists before
-- Event Template ---- Date : 07-Oct-024
IF NOT EXISTS (Select 1 from NOTIF_NOTIFICATION_TEMPLATE where EventId = 100)
BEGIN
INSERT INTO NOTIF_NOTIFICATION_TEMPLATE VALUES (NEWID(), 100, 4, 'Decision Service Request', N'Decision Service Request',
  'Decision Service Request', N'Decision Service Request', 'Item Order Number #Reference Number# are #Document Name# by #Name# #Sender Name# On #Created Date#',
  'Item Order Number #Reference Number# are #Document Name# by #Name# #Sender Name# On #Created Date#', null, 'System Generated', CURRENT_TIMESTAMP, null, null,
	null, null, 0, 1)
	END;
-------- Event Place Holders
IF NOT EXISTS (Select 1 From NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP where EventId = 100)
BEGIN
INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP VALUES (204,'#Reference Number#',100)
INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP VALUES (205,'#Name#',100)
INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP VALUES (206,'#Document Name#',100)
END----------------------------------------------------

update CMS_TEMPLATE set Content = '<p><br></p>' where id = 1

update CMS_TEMPLATE set Content = '<p style="font-family: Sultan; font-size: 13px"><br></p>' where id = 1


------------------14-Oct-024-----------------------


IF EXISTS(Select 1 from NOTIF_NOTIFICATION_EVENT where EventId = 106)
BEGIN
update NOTIF_NOTIFICATION_EVENT set NameEn = 'Service Request Approved', NameAr=N'Service Request Approved' where EventId = 106
END
ELSE
BEGIN
   insert into NOTIF_NOTIFICATION_EVENT VALUES(106, 'Service Request Approved', N'Service Request Approved By HOS','System Generated', 
											   CURRENT_TIMESTAMP, null, null, null, null, 0, 1, null, null, null, 1)
 END;

 
IF EXISTS(Select 1 from NOTIF_NOTIFICATION_TEMPLATE where EventId = 106)
BEGIN
update NOTIF_NOTIFICATION_TEMPLATE set NameEn = 'Service Request Approved', NameAr = N'Service Request Approved',
 SubjectEn = 'Service Request Approved', SubjectAr = N'Service Request Approved',
BodyEn= '#Entity# #Request Number# Approved by #Sender Name# on #CreatedDate#', 
BodyAr= N'#Entity# #Request Number# Approved by #Sender Name# on #CreatedDate#' where EventId = 106
END
ELSE
BEGIN
   insert into NOTIF_NOTIFICATION_TEMPLATE VALUES(NEWID(), 106, 4,'Service Request Approved', N'Service Request Approved',
   'Service Request Approved', N'Service Request Approved', '#Entity# #Request Number# Approved by #Sender Name# on #CreatedDate#', 
    N'#Entity# #Request Number# Approved by #Sender Name# on #CreatedDate#', null, 'System Generated', CURRENT_TIMESTAMP, null, null, null, null,
	0, 1)
 END;

IF NOT EXISTS(Select 1 from NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP where EventId = 106)
BEGIN
 DECLARE @PlaceHolderId INT 
 SET @PlaceHolderId = (Select MAX(PlaceHolderId) from NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP)
 SET @PlaceHolderId = @PlaceHolderId + 1
INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP VALUES (@PlaceHolderId, '#Request Number#', 106)
END
 
------------------Transfer items Event-----------------------
  

IF NOT EXISTS(Select 1 from NOTIF_NOTIFICATION_EVENT where EventId = 147)
BEGIN
 INSERT INTO NOTIF_NOTIFICATION_EVENT VALUES(147, 'Transfer Items', N'Transfer Items','System Generated',  CURRENT_TIMESTAMP, null, null, null, null, 0, 1, null, null, null, 1)
END;
 
IF NOT EXISTS(Select 1 from NOTIF_NOTIFICATION_TEMPLATE where EventId = 147)
BEGIN
INSERT INTO NOTIF_NOTIFICATION_TEMPLATE VALUES(NEWID(), 147, 4,'Transfer Items', N'Transfer Items',
   'Transfer Items', N'Transfer Items', 'The #Sender Name# of #Store Incharge# requests to transfer items to #Name#. Please confirm approval or rejection.', 
    N'The #Sender Name# of #Store Incharge# requests to transfer items to #Name#. Please confirm approval or rejection.', null, 'System Generated', CURRENT_TIMESTAMP, null, null, null, null,
	0, 1)
END;

IF NOT EXISTS(Select 1 from NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP where EventId = 147)
BEGIN
 DECLARE @PlaceHolderId INT 
 SET @PlaceHolderId = (Select MAX(PlaceHolderId) from NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP)
 SET @PlaceHolderId = @PlaceHolderId + 1
INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP VALUES (@PlaceHolderId, '#Store Incharge#', 147)
SET @PlaceHolderId = @PlaceHolderId + 1
INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP VALUES (@PlaceHolderId, '#Name#', 147)
END

IF NOT EXISTS(Select 1 from NOTIF_NOTIFICATION_EVENT where EventId = 148)
BEGIN
 INSERT INTO NOTIF_NOTIFICATION_EVENT VALUES(148, 'Approve Transfer Items', N'Approve Transfer Items','System Generated',  CURRENT_TIMESTAMP, null, null, null, null, 0, 1, null, null, null, 1)
END;
 
IF NOT EXISTS(Select 1 from NOTIF_NOTIFICATION_TEMPLATE where EventId = 148)
BEGIN
INSERT INTO NOTIF_NOTIFICATION_TEMPLATE VALUES(NEWID(), 148, 4,'Approve Transfer Items', N'Approve Transfer Items',
   'Approve Transfer Items', N'Approve Transfer Items', '#Sender Name# Approve Transfer Items request On #Created Date#.', 
    N'#Sender Name# Approve Transfer Items request On #Created Date#.', null, 'System Generated', CURRENT_TIMESTAMP, null, null, null, null,
	0, 1)
END;
IF NOT EXISTS(Select 1 from NOTIF_NOTIFICATION_EVENT where EventId = 149)
BEGIN
 INSERT INTO NOTIF_NOTIFICATION_EVENT VALUES(149, 'Reject Transfer Items', N'Reject Transfer Items','System Generated',  CURRENT_TIMESTAMP, null, null, null, null, 0, 1, null, null, null, 1)
END;
 
IF NOT EXISTS(Select 1 from NOTIF_NOTIFICATION_TEMPLATE where EventId = 149)
BEGIN
INSERT INTO NOTIF_NOTIFICATION_TEMPLATE VALUES(NEWID(), 149, 4,'Reject Transfer Items', N'Reject Transfer Items',
   'Reject Transfer Items', N'Reject Transfer Items', '#Sender Name# Reject Transfer Items Request On #Created Date#.', 
    N'#Sender Name# Reject Transfer Items Request On #Created Date#.', null, 'System Generated', CURRENT_TIMESTAMP, null, null, null, null,
	0, 1)
END;
-------------------------------------




IF NOT EXISTS(SELECT 1 From [WF_CONDITION_PR_LKP] where MKey = 'CmsDraftDocumentStatusApproveAndViceHOSApprovalEnough')
BEGIN 
INSERT [dbo].[WF_CONDITION_PR_LKP] ([ModuleConditionId], [Name], [MKey], [ValueToCompare],IsTriggerSpecific) VALUES (171, N'If_Draft_Document_Status_is_Approve_And_Vice_HOS_Approval_Enough', N'CmsDraftDocumentStatusApproveAndViceHOSApprovalEnough', 8, 0)
END


INSERT [dbo].[WF_TRIGGER_CONDITION_PR] ([TriggerId], [ConditionId]) VALUES (4, 171)
INSERT [dbo].[WF_TRIGGER_CONDITION_PR] ([TriggerId], [ConditionId]) VALUES (5, 171)



IF NOT EXISTS(SELECT 1 From [WF_CONDITION_PR_LKP] where MKey = 'CmsSubmittedByViceHOSAndViceHOSApprovalEnough')
BEGIN 
INSERT [dbo].[WF_CONDITION_PR_LKP] ([ModuleConditionId], [Name], [MKey], [ValueToCompare],IsTriggerSpecific) VALUES (172, N'If_Draft_Document_is_Submitted_By_ViceHOS_And_Vice_HOS_Approval_Enough', N'CmsSubmittedByViceHOSAndViceHOSApprovalEnough', '3a07eb32-db29-47a6-8252-900e4d10182c', 1)
END

INSERT [dbo].[WF_TRIGGER_CONDITION_PR] ([TriggerId], [ConditionId]) VALUES (4, 172)
INSERT [dbo].[WF_TRIGGER_CONDITION_PR] ([TriggerId], [ConditionId]) VALUES (5, 172)



IF EXISTS (SELECT ModuleConditionId FROM WF_CONDITION_PR_LKP WHERE ModuleConditionId = 161 ) 
BEGIN
	UPDATE WF_CONDITION_PR_LKP SET MKey = 'CmsSubmittedByViceHOS', ValueToCompare = '3a07eb32-db29-47a6-8252-900e4d10182c' where ModuleConditionId = 161
    PRINT 'Record Updated';
END
else
BEGIN
    PRINT 'Record Does Not Exist';
END



IF EXISTS (SELECT ModuleConditionId FROM WF_CONDITION_PR_LKP WHERE ModuleConditionId = 169 ) 
BEGIN
	UPDATE WF_CONDITION_PR_LKP SET Name = 'If_Draft_Document_is_Submitted_By_ViceHOS', MKey = 'ComsSubmittedByViceHOS', ValueToCompare = '3a07eb32-db29-47a6-8252-900e4d10182c' where ModuleConditionId = 169
    PRINT 'Record Updated';
END
else
BEGIN
    PRINT 'Record Does Not Exist';
END
------------
-----------
INSERT INTO WF_OPTIONS_PR_LKP (ModuleOptionId , Name) VALUES(2097152,'SubmitToSupervisor')
INSERT INTO WF_OPTIONS_PR_LKP (ModuleOptionId , Name) VALUES(4194304,'SubmitToHOS')
INSERT INTO WF_OPTIONS_PR_LKP (ModuleOptionId , Name) VALUES(8388608,'SubmitToViceHOS')
----------
INSERT INTO WF_TRIGGER_OPTIONS_PR ([TriggerId],[OptionId]) VALUES (4,2097152)
INSERT INTO WF_TRIGGER_OPTIONS_PR ([TriggerId],[OptionId]) VALUES (4,4194304)
INSERT INTO WF_TRIGGER_OPTIONS_PR ([TriggerId],[OptionId]) VALUES (4,8388608)
--------
INSERT INTO WF_TRIGGER_OPTIONS_PR ([TriggerId],[OptionId]) VALUES (5,2097152)
INSERT INTO WF_TRIGGER_OPTIONS_PR ([TriggerId],[OptionId]) VALUES (5,4194304)
INSERT INTO WF_TRIGGER_OPTIONS_PR ([TriggerId],[OptionId]) VALUES (5,8388608)
----------
insert into WF_TRIGGER_CONDITION_PR ([TriggerId],[ConditionId]) values ('5','169')
insert into UMS_GROUP_CLAIMS (GroupId,ClaimType,ClaimValue) values ('49744B64-2399-4097-94FD-326D6FCE2626','Permission','Permissions.CMS.DraftDocument.CreateButton')
----------




update CMS_REGISTERED_CASE_EVENT_G2G_LKP set Name_Ar = N'تم تحويل القضية لدائرة أخرى', 
Name_En = 'Case Transferred to Another Chmaber'
where id = 4
-------
INSERT INTO [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId], [DescriptionEn], [DescriptionAr], [IsActive]) 
VALUES (150, N'Final Judgement Issued', N'Final Judgement Issued', N'System Generated', GETDATE(), NULL, NULL, NULL, NULL, 0, 1, NULL, NULL, NULL, 1)
-------
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) 
VALUES (NEWID(), 150, 4, N'Final Judgement Issued', N'Final Judgement Issued', N'Final Judgement Issued Successfully', N'Final Judgement Issued Successfully', N'Following #Entity#, Final Judgement Issued to #Sector To# on #Created Date# Date by #Sender Name#.', N'Following #Entity#, Final Judgement Issued to #Sector To# on #Created Date# Date by #Sender Name#.', NULL, N'fatwaadmin@gmail.com', GETDATE(), N'fatwaadmin@gmail.com', NULL, NULL, NULL, 0, 1)
-------
INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP ([PlaceHolderId],[PlaceHolderName],[EventId]) VALUES (210,'#Created Date#',150)
INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP ([PlaceHolderId],[PlaceHolderName],[EventId]) VALUES (211,'#Sender Name#',150)
-------
update CMS_TEMPLATE set Content = '<p style="font-family: Sultan; font-size: 13px"><br></p>' where id = 1


------------

Update CMS_REQUEST_TYPE_G2G_LKP set Name_Ar = N' التشريعات' where Id = 8
Update CMS_REQUEST_TYPE_G2G_LKP set Name_Ar = N'تظلمات إدارية' where Id = 16
Update CMS_REQUEST_TYPE_G2G_LKP set Name_Ar = N'التحكيم الدولي' where Id = 64
-------------------------------------
-------------------------------------27-Oct-024
IF NOT EXISTS(Select 1 from NOTIF_NOTIFICATION_EVENT where EventId = 151)
BEGIN
 INSERT INTO NOTIF_NOTIFICATION_EVENT VALUES(151, 'Return Inventory', N'Return Inventory','System Generated',  CURRENT_TIMESTAMP, null, null, null, null, 0, 1, null, null, null, 1)
END;

IF NOT EXISTS(Select 1 from NOTIF_NOTIFICATION_TEMPLATE where EventId = 151)
BEGIN
INSERT INTO NOTIF_NOTIFICATION_TEMPLATE VALUES(NEWID(), 151, 4,'Return Inventory', N'Return Inventory',
   'Return Inventory', N'Return Inventory', '#Entity# #Service Request Number# has been #Name# by #Sender Name# On #Created Date#.', 
    N'#Entity# #Service Request Number# has been #Name# by #Sender Name# On #Created Date#.', null, 'System Generated', CURRENT_TIMESTAMP, null, null, null, null,
	0, 1)
END;
IF NOT EXISTS(Select 1 from NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP where EventId = 151)
BEGIN
 DECLARE @PlaceHolderId INT 
 SET @PlaceHolderId = (Select MAX(PlaceHolderId) from NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP)
 SET @PlaceHolderId = @PlaceHolderId + 1
INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP VALUES (@PlaceHolderId, '#Service Request Number#', 151)
SET @PlaceHolderId = @PlaceHolderId + 1
INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP VALUES (@PlaceHolderId, '#Name#', 151)
END

--------------------------------- End




INSERT INTO CMS_CASE_FILE_TRANSFER_REQUEST_STATUS (Id,NameEn,NameAr) VALUES (1,'Approved',N'معتمد')
INSERT INTO CMS_CASE_FILE_TRANSFER_REQUEST_STATUS (Id,NameEn,NameAr) VALUES (2,'Rejected',N'رفض')
INSERT INTO CMS_CASE_FILE_TRANSFER_REQUEST_STATUS (Id,NameEn,NameAr) VALUES (3,'Submitted',N'الطلبات المرسلة')


---------------------------------------
IF NOT EXISTS(Select 1 from NOTIF_NOTIFICATION_EVENT where EventId = 165)
BEGIN
 INSERT INTO NOTIF_NOTIFICATION_EVENT VALUES(165, 'Pending Task Reminder For Manager', N'Pending Task Reminder For Manager','System Generated',  CURRENT_TIMESTAMP, null, null, null, null, 0, 1, null, null, null, 1)
END;

----------------------------------

INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP ([PlaceHolderId],[PlaceHolderName],[EventId]) VALUES (227,'#Created Date#',165)
INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP ([PlaceHolderId],[PlaceHolderName],[EventId]) VALUES (228,'#Assignee NameEn#',165)
INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP ([PlaceHolderId],[PlaceHolderName],[EventId]) VALUES (229,'#Assignee NameAr#',165)
INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP ([PlaceHolderId],[PlaceHolderName],[EventId]) VALUES (230,'#Assignor NameEn#',165)
INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP ([PlaceHolderId],[PlaceHolderName],[EventId]) VALUES (231,'#Assignor NameAr#',165)
INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP ([PlaceHolderId],[PlaceHolderName],[EventId]) VALUES (232,'#Reference Number#',165)
INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP ([PlaceHolderId],[PlaceHolderName],[EventId]) VALUES (233,'#Entity#',165)
INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP ([PlaceHolderId],[PlaceHolderName],[EventId]) VALUES (234,'#Subject#',165)


-------------------------------
INSERT [dbo].[NOTIF_NOTIFICATION_TEMPLATE] ([TemplateId], [EventId], [ChannelId], [NameEn], [NameAr], [SubjectEn], [SubjectAr], [BodyEn], [BodyAr], [Footer], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [IsActive]) 
VALUES (NEWID(), 165, 4, N'Pending Task Reminder For Manager', N'Pending Task Reminder For Manager', N'Task Decision is pending', N'Task Decision is pending', N'Reminder: #Assignee NameEn# has not taken any action on task for #Entity# #Reference Number# which is assigned by #Assignor NameEn# on #Created Date#', N'Reminder: #Assignee NameAr# has not taken any action on task for #Entity# #Reference Number# which is assigned by #Assignor NameAr# on #Created Date#', NULL, N'fatwaadmin@gmail.com', GETDATE(), N'fatwaadmin@gmail.com', NULL, NULL, NULL, 0, 1)
--------------------------

UPDATE NOTIF_NOTIFICATION_TEMPLATE set 
BodyEn = 'Reminder: #Assignee NameEn# has not taken any action on task #Subject# for #Entity# #Reference Number# which is assigned by #Assignor NameEn# on #Created Date#' where TemplateId = 'E53A11C2-30F6-4605-9940-B7866D1143D8'
UPDATE NOTIF_NOTIFICATION_TEMPLATE set 
BodyAr = 'Reminder: #Assignee NameAr# has not taken any action on task #Subject# for #Entity# #Reference Number# which is assigned by #Assignor NameAr# on #Created Date#' where TemplateId = 'E53A11C2-30F6-4605-9940-B7866D1143D8'


----------------------------

Update NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP Set PlaceHolderName = '#SubjectEn#' where PlaceHolderId = 234
INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP VALUES(235 , '#SubjectAr#' , 165)

----------------------
UPDATE NOTIF_NOTIFICATION_TEMPLATE set 
BodyEn = 'Reminder: #Assignee NameEn# has not taken any action on task #SubjectEn# for #Entity# #Reference Number# which is assigned by #Assignor NameEn# on #Created Date#' where TemplateId = 'E53A11C2-30F6-4605-9940-B7866D1143D8'
UPDATE NOTIF_NOTIFICATION_TEMPLATE set 
BodyAr = 'Reminder: #Assignee NameAr# has not taken any action on task #SubjectAr# for #Entity# #Reference Number# which is assigned by #Assignor NameAr# on #Created Date#' where TemplateId = 'E53A11C2-30F6-4605-9940-B7866D1143D8'


UPDATE SYSTEM_SETTING SET File_Maximum_Size = '26214400'

-----Archiving Claims
INSERT INTO [dbo].[UMS_CLAIM]
           ([Title_En]
           ,[Module]
           ,[SubModule]
           ,[ClaimType]
           ,[ClaimValue]
           ,[Title_Ar]
           ,[IsDeleted]
           ,[ModuleId])
     VALUES
           ('Archived Cases List','Archived_Cases','Archived_Cases_List','Permission','Permissions.ARC.ArchivedCases.List',N'لائحة الحالات المؤرشفة',0,2)
UPDATE SYSTEM_SETTING SET File_Maximum_Size = '26214400'




SET IDENTITY_INSERT [dbo].[CMS_OPERATING_SECTOR_TYPE_G2G_LKP] ON 
GO
INSERT [dbo].[CMS_OPERATING_SECTOR_TYPE_G2G_LKP] ([Id], [Code], [Name_En], [Name_Ar], [IsActive], [DepartmentId], [ParentId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ModuleId], [BuildingId], [FloorId], [IsOnlyViceHosApprovalRequired], [IsViceHosResponsibleForAllLawyers], [G2GBRSiteID]) VALUES (38, N'FAD', N'Financial Affairs Department', N'Financial Affairs Department', 1, 2, 21, N'fatwaadmin@gmail.com', CAST(N'2024-03-26T15:53:13.920' AS DateTime), NULL, NULL, NULL, NULL, 0, NULL, 1, 1, 0, 0, NULL)
GO
INSERT [dbo].[CMS_OPERATING_SECTOR_TYPE_G2G_LKP] ([Id], [Code], [Name_En], [Name_Ar], [IsActive], [DepartmentId], [ParentId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ModuleId], [BuildingId], [FloorId], [IsOnlyViceHosApprovalRequired], [IsViceHosResponsibleForAllLawyers], [G2GBRSiteID]) VALUES (39, N'AUFDA', N'Assistant Undersecretay of Finance Administrative Affairs', N'Assistant Undersecretay of Finance Administrative Affairs', 1, 2, 21, N'fatwaadmin@gmail.com', CAST(N'2024-03-26T16:21:36.270' AS DateTime), NULL, NULL, NULL, NULL, 0, NULL, 1, 1, 0, 0, NULL)
GO
INSERT [dbo].[CMS_OPERATING_SECTOR_TYPE_G2G_LKP] ([Id], [Code], [Name_En], [Name_Ar], [IsActive], [DepartmentId], [ParentId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ModuleId], [BuildingId], [FloorId], [IsOnlyViceHosApprovalRequired], [IsViceHosResponsibleForAllLawyers], [G2GBRSiteID]) VALUES (40, N'PWC', N'Purchasing And Warehoused Controller', N'Purchasing And Warehoused Controller', 1, 2, 38, N'fatwaadmin@gmail.com', CAST(N'2024-03-26T16:25:29.830' AS DateTime), NULL, NULL, NULL, NULL, 0, NULL, 1, 1, 0, 0, NULL)
GO
INSERT [dbo].[CMS_OPERATING_SECTOR_TYPE_G2G_LKP] ([Id], [Code], [Name_En], [Name_Ar], [IsActive], [DepartmentId], [ParentId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ModuleId], [BuildingId], [FloorId], [IsOnlyViceHosApprovalRequired], [IsViceHosResponsibleForAllLawyers], [G2GBRSiteID]) VALUES (41, N'TCD', N'Tenders and Contract Department', N'Tenders and Contract Department', 1, 2, 39, N'fatwaadmin@gmail.com', CAST(N'2024-03-26T16:25:54.933' AS DateTime), NULL, NULL, NULL, NULL, 0, NULL, 1, 1, 0, 0, NULL)
GO
INSERT [dbo].[CMS_OPERATING_SECTOR_TYPE_G2G_LKP] ([Id], [Code], [Name_En], [Name_Ar], [IsActive], [DepartmentId], [ParentId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ModuleId], [BuildingId], [FloorId], [IsOnlyViceHosApprovalRequired], [IsViceHosResponsibleForAllLawyers], [G2GBRSiteID]) VALUES (42, N'PC', N'Purchasing Department', N'Purchasing Department', 1, 2, 39, N'fatwaadmin@gmail.com', CAST(N'2024-03-26T16:25:54.937' AS DateTime), NULL, NULL, NULL, NULL, 0, NULL, 1, 1, 0, 0, NULL)
GO
INSERT [dbo].[CMS_OPERATING_SECTOR_TYPE_G2G_LKP] ([Id], [Code], [Name_En], [Name_Ar], [IsActive], [DepartmentId], [ParentId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ModuleId], [BuildingId], [FloorId], [IsOnlyViceHosApprovalRequired], [IsViceHosResponsibleForAllLawyers], [G2GBRSiteID]) VALUES (43, N'WD', N'Warehouses Department', N'Warehouses Department', 1, 2, 39, N'fatwaadmin@gmail.com', CAST(N'2024-03-26T16:25:54.937' AS DateTime), NULL, NULL, NULL, NULL, 0, NULL, 1, 1, 0, 0, NULL)
GO
INSERT [dbo].[CMS_OPERATING_SECTOR_TYPE_G2G_LKP] ([Id], [Code], [Name_En], [Name_Ar], [IsActive], [DepartmentId], [ParentId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ModuleId], [BuildingId], [FloorId], [IsOnlyViceHosApprovalRequired], [IsViceHosResponsibleForAllLawyers], [G2GBRSiteID]) VALUES (44, N'AC', N'Accounting Controller', N'Accounting Controller', 1, 2, 38, N'fatwaadmin@gmail.com', CAST(N'2024-03-26T16:29:06.013' AS DateTime), NULL, NULL, NULL, NULL, 0, NULL, 1, 1, 0, 0, NULL)
GO
INSERT [dbo].[CMS_OPERATING_SECTOR_TYPE_G2G_LKP] ([Id], [Code], [Name_En], [Name_Ar], [IsActive], [DepartmentId], [ParentId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ModuleId], [BuildingId], [FloorId], [IsOnlyViceHosApprovalRequired], [IsViceHosResponsibleForAllLawyers], [G2GBRSiteID]) VALUES (45, N'BD', N'Budgeting Department', N'Budgeting Department', 1, 2, 44, N'fatwaadmin@gmail.com', CAST(N'2024-03-26T16:32:48.370' AS DateTime), NULL, NULL, NULL, NULL, 0, NULL, 1, 1, 0, 0, NULL)
GO
INSERT [dbo].[CMS_OPERATING_SECTOR_TYPE_G2G_LKP] ([Id], [Code], [Name_En], [Name_Ar], [IsActive], [DepartmentId], [ParentId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ModuleId], [BuildingId], [FloorId], [IsOnlyViceHosApprovalRequired], [IsViceHosResponsibleForAllLawyers], [G2GBRSiteID]) VALUES (46, N'AD', N'Accounting Department', N'Accounting Department', 1, 2, 44, N'fatwaadmin@gmail.com', CAST(N'2024-03-26T16:32:48.370' AS DateTime), NULL, NULL, NULL, NULL, 0, NULL, 1, 1, 0, 0, NULL)
GO
INSERT [dbo].[CMS_OPERATING_SECTOR_TYPE_G2G_LKP] ([Id], [Code], [Name_En], [Name_Ar], [IsActive], [DepartmentId], [ParentId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ModuleId], [BuildingId], [FloorId], [IsOnlyViceHosApprovalRequired], [IsViceHosResponsibleForAllLawyers], [G2GBRSiteID]) VALUES (47, N'PD', N'Payroll Department', N'Payroll Department', 1, 2, 44, N'fatwaadmin@gmail.com', CAST(N'2024-03-26T16:32:48.370' AS DateTime), NULL, NULL, NULL, NULL, 0, NULL, 1, 1, 0, 0, NULL)
GO
INSERT [dbo].[CMS_OPERATING_SECTOR_TYPE_G2G_LKP] ([Id], [Code], [Name_En], [Name_Ar], [IsActive], [DepartmentId], [ParentId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ModuleId], [BuildingId], [FloorId], [IsOnlyViceHosApprovalRequired], [IsViceHosResponsibleForAllLawyers], [G2GBRSiteID]) VALUES (48, N'WACD', N'Warehouses Accounting and Custody Department', N'Warehouses Accounting and Custody Departmen', 1, 2, 44, N'fatwaadmin@gmail.com', CAST(N'2024-03-26T16:32:48.370' AS DateTime), NULL, NULL, NULL, NULL, 0, NULL, 1, 1, 0, 0, NULL)
GO
INSERT [dbo].[CMS_OPERATING_SECTOR_TYPE_G2G_LKP] ([Id], [Code], [Name_En], [Name_Ar], [IsActive], [DepartmentId], [ParentId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ModuleId], [BuildingId], [FloorId], [IsOnlyViceHosApprovalRequired], [IsViceHosResponsibleForAllLawyers], [G2GBRSiteID]) VALUES (49, N'AAD', N'Assets Accounting Department', N'Assets Accounting Department', 1, 2, 44, N'fatwaadmin@gmail.com', CAST(N'2024-03-26T16:32:48.373' AS DateTime), NULL, NULL, NULL, NULL, 0, NULL, 1, 1, 0, 0, NULL)
GO
INSERT [dbo].[CMS_OPERATING_SECTOR_TYPE_G2G_LKP] ([Id], [Code], [Name_En], [Name_Ar], [IsActive], [DepartmentId], [ParentId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ModuleId], [BuildingId], [FloorId], [IsOnlyViceHosApprovalRequired], [IsViceHosResponsibleForAllLawyers], [G2GBRSiteID]) VALUES (50, N'IAD', N'Internal Audit Department', N'Internal Audit Department', 1, 2, 38, N'fatwaadmin@gmail.com', CAST(N'2024-03-26T16:36:46.520' AS DateTime), NULL, NULL, NULL, NULL, 0, NULL, 1, 1, 0, 0, NULL)
GO
SET IDENTITY_INSERT [dbo].[CMS_OPERATING_SECTOR_TYPE_G2G_LKP] OFF
GO
delete from CMS_GOVERNMENT_ENTITY_DEPARTMENT_G2G_LKP where EntityId = 94

-----------------------
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId]) VALUES (187, N'Comment Reply Added', N'Comment Reply Added', N'System Generated', CAST(N'2024-03-24T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL)
INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP (PlaceHolderId ,PlaceHolderName , EventId) VALUES (267,'#Reference Number#' , 187)
-------
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId]) VALUES (188, N'G2G Comment Added', N'G2G Comment Added', N'System Generated', CAST(N'2024-03-24T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL)
INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP (PlaceHolderId ,PlaceHolderName , EventId) VALUES (268,'#Reference Number#' , 188)

-------
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId]) VALUES (189, N'G2G Comment Reply Added', N'G2G Comment Reply Added', N'System Generated', CAST(N'2024-03-24T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL)
INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP (PlaceHolderId ,PlaceHolderName , EventId) VALUES (269,'#Reference Number#' , 189)
---------
Update NOTIF_NOTIFICATION_EVENT set NameEn = 'Feedback Added' , NameAr = 'Feedback Added' where EventId = 89

-----------------------------

INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId]) VALUES (190, N'FTW Comment Reply Added', N'FTW Comment Reply Added', N'System Generated', CAST(N'2024-03-24T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL)
INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP (PlaceHolderId ,PlaceHolderName , EventId) VALUES (270,'#Reference Number#' , 190)
----------------------------
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId]) VALUES (191, N'FTW Comment Added', N'FTW Comment Added', N'System Generated', CAST(N'2024-03-24T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL)
INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP (PlaceHolderId ,PlaceHolderName , EventId) VALUES (271,'#Reference Number#' , 191)
---------------------------
 INSERT INTO NOTIF_NOTIFICATION_TEMPLATE (TemplateId, EventId, ChannelId, NameEn, NameAr ,SubjectEn, SubjectAr, BodyEn, BodyAr,CreatedBy , CreatedDate , IsDeleted )
VALUES (NewID(),187, 4, 'Reply sent successfully' , N'Reply sent successfully',  'Reply sent successfully' , N'Reply sent successfully',
'#Sender Name# has replied to the  #Receiver Name#''s comment on ticket #Reference Number# on #Created Date#', 
N'#Sender Name# has replied to the  #Receiver Name#''s comment on ticket #Reference Number# on #Created Date#', 
'fatwaadmin@gmail.com' ,'2024-04-04 11:41:43.297',0);
----------
 INSERT INTO NOTIF_NOTIFICATION_TEMPLATE (TemplateId, EventId, ChannelId, NameEn, NameAr ,SubjectEn, SubjectAr, BodyEn, BodyAr,CreatedBy , CreatedDate , IsDeleted )
VALUES (NewID(),188, 4, 'Comment Added Successfully' , N'Comment Added Successfully',  'Comment Added Successfully' , N'Comment Added Successfully',
'Government entity has added a comment on ticket #Reference Number# on #Created Date#', 
N'Government entity has added a comment on ticket #Reference Number# on #Created Date#', 
'fatwaadmin@gmail.com' ,'2024-04-04 11:41:43.297',0);
------------
 INSERT INTO NOTIF_NOTIFICATION_TEMPLATE (TemplateId, EventId, ChannelId, NameEn, NameAr ,SubjectEn, SubjectAr, BodyEn, BodyAr,CreatedBy , CreatedDate , IsDeleted )
VALUES (NewID(),189, 4, 'Comment Reply Added Successfully' , N'Comment Reply Added Successfully',  'Comment Reply Added Successfully' , N'Comment Reply Added Successfully',
'Government entity has added a reply to  #Receiver Name#''s comment on ticket #Reference Number# on #Created Date#', 
N'Government entity has added a reply to  #Receiver Name#''s comment on ticket #Reference Number# on #Created Date#', 
'fatwaadmin@gmail.com' ,'2024-04-04 11:41:43.297',0);
-----------
INSERT INTO NOTIF_NOTIFICATION_TEMPLATE (TemplateId, EventId, ChannelId, NameEn, NameAr ,SubjectEn, SubjectAr, BodyEn, BodyAr,CreatedBy , CreatedDate , IsDeleted )
VALUES (NewID(),190, 4, 'Comment Reply Added Successfully' , N'Comment Reply Added Successfully',  'Comment Reply Added Successfully' , N'Comment Reply Added Successfully',
'IT Support Team has added a reply to  #Receiver Name#''s comment on ticket #Reference Number# on #Created Date#', 
N'IT Support Team has added a reply to  #Receiver Name#''s comment on ticket #Reference Number# on #Created Date#', 
'fatwaadmin@gmail.com' ,'2024-04-04 11:41:43.297',0);
-----------
INSERT INTO NOTIF_NOTIFICATION_TEMPLATE (TemplateId, EventId, ChannelId, NameEn, NameAr ,SubjectEn, SubjectAr, BodyEn, BodyAr,CreatedBy , CreatedDate , IsDeleted )
VALUES (NewID(),191, 4, 'Comment Added Successfully' , N'Comment Added Successfully',  'Comment Added Successfully' , N'Comment Added Successfully',
'IT Support Team has added a comment on ticket #Reference Number# on #Created Date#', 
N'IT Support Team has added a comment on ticket #Reference Number# on #Created Date#', 
'fatwaadmin@gmail.com' ,'2024-04-04 11:41:43.297',0);
------------------

UPDATE BUG_EVENT_G2G_LKP Set Name_Ar = N'تمت إنشاء مسودة للتذكرة' where Id = 1
UPDATE BUG_EVENT_G2G_LKP Set Name_Ar = N'تم إضافة التذكرة' where Id = 2
UPDATE BUG_EVENT_G2G_LKP Set Name_Ar = N'تم تعيين التذكرة ' where Id = 4
UPDATE BUG_EVENT_G2G_LKP Set Name_Ar = N'تم قبول التذكرة' where Id = 8
UPDATE BUG_EVENT_G2G_LKP Set Name_Ar = N'تم رفض التذكرة' where Id = 16
UPDATE BUG_EVENT_G2G_LKP Set Name_Ar = N'تمت إضافة القرار' where Id = 32
UPDATE BUG_EVENT_G2G_LKP Set Name_Ar = N'التذكرة مغلقة' where Id = 64
UPDATE BUG_EVENT_G2G_LKP Set Name_Ar = N'أعيد فتح التذكرة' where Id = 128
UPDATE BUG_EVENT_G2G_LKP Set Name_Ar = N'تم إضافة التعليق' where Id = 256
UPDATE BUG_EVENT_G2G_LKP Set Name_Ar = N'تمت إضافة الملاحظات' where Id = 512
UPDATE BUG_EVENT_G2G_LKP Set Name_Ar = N'تم الإبلاغ عن الخطأ' where Id = 1024
UPDATE BUG_EVENT_G2G_LKP Set Name_Ar = N'تمت إضافة الرد على التعليق' where Id = 2048

-----------------------

Update BUG_MODULE_G2G_LKP Set Name_Ar = N'نظام إدارة ملفات القضايا' where Id = 1
Update BUG_MODULE_G2G_LKP Set Name_Ar = N'إدارة الاستشاري' where Id = 2
Update BUG_MODULE_G2G_LKP Set Name_Ar = N'إدارة جهات الاتصال' where Id = 4
Update BUG_MODULE_G2G_LKP Set Name_Ar = N'إدارة الاجتماعات' where Id = 8
Update BUG_MODULE_G2G_LKP Set Name_Ar = N'ادارة المهام' where Id = 16
Update BUG_MODULE_G2G_LKP Set Name_Ar = N'آليات العمل' where Id = 32
Update BUG_MODULE_G2G_LKP Set Name_Ar = N'تتبع الوقت' where Id = 64
Update BUG_MODULE_G2G_LKP Set Name_Ar = N'التدقيق وتتبع التاريخ' where Id = 128
Update BUG_MODULE_G2G_LKP Set Name_Ar = N'بحث عام' where Id = 256
Update BUG_MODULE_G2G_LKP Set Name_Ar = N'إدارة المستخدم' where Id = 512
Update BUG_MODULE_G2G_LKP Set Name_Ar = N'بحث عام' where Id = 1024
Update BUG_MODULE_G2G_LKP Set Name_Ar = N'عمليات بحث المشرف' where Id = 2048
Update BUG_MODULE_G2G_LKP Set Name_Ar = N'الإبلاغ عن الأخطاء' where Id = 4096
Update BUG_MODULE_G2G_LKP Set Name_Ar = N'دعم القرار' where Id = 8192
Update BUG_MODULE_G2G_LKP Set Name_Ar = N'إعداد التقارير' where Id = 16384
Update BUG_MODULE_G2G_LKP Set Name_Ar = N'إدارة المستندات' where Id = 32768
Update BUG_MODULE_G2G_LKP Set Name_Ar = N'الموارد البشرية' where Id = 65536
Update BUG_MODULE_G2G_LKP Set Name_Ar = N'المالية' where Id =131072
Update BUG_MODULE_G2G_LKP Set Name_Ar = N'تكنولوجيا المعلومات' where Id =262144
Update BUG_MODULE_G2G_LKP Set Name_Ar = N'خدمات عامة' where Id =524288
Update BUG_MODULE_G2G_LKP Set Name_Ar = N'التدقيق والتفتيش' where Id =1048576
Update BUG_MODULE_G2G_LKP Set Name_Ar = N'الشؤون القانونية' where Id =2097152
Update BUG_MODULE_G2G_LKP Set Name_Ar = N'التخطيط والتدريب' where Id =4194304
Update BUG_MODULE_G2G_LKP Set Name_Ar = N'اللجنة المنظمة' where Id =8388608
Update BUG_MODULE_G2G_LKP Set Name_Ar = N'إعلان ومذكرات' where Id =16777216
Update BUG_MODULE_G2G_LKP Set Name_Ar = N'البائعين والعقود' where Id =33554432
Update BUG_MODULE_G2G_LKP Set Name_Ar = N'تقييم' where Id =67108864
Update BUG_MODULE_G2G_LKP Set Name_Ar = N'الحضور والانصراف' where Id =134217728

------


Update NOTIF_NOTIFICATION_TEMPLATE Set BodyEn = 'Assign Ticket #Reference Number# on #Created Date#'
,BodyAr = 'Assign Ticket #Reference Number# on #Created Date#' where EventId = 90


Update NOTIF_NOTIFICATION_TEMPLATE Set BodyEn = 'Reject Ticket #Reference Number# on #Created Date#'
,BodyAr = 'Reject Ticket #Reference Number# on #Created Date#' where EventId = 91
Update NOTIF_NOTIFICATION_TEMPLATE Set BodyEn = 'Resolve Ticket  #Reference Number# on #Created Date#'
,BodyAr = 'Resolve Ticket  #Reference Number# on #Created Date#' where EventId = 92

Update NOTIF_NOTIFICATION_TEMPLATE Set BodyEn = 'ReOpen Ticket #Reference Number# on #Created Date#'
,BodyAr = 'ReOpen Ticket #Reference Number# on #Created Date#' where EventId = 93

Update NOTIF_NOTIFICATION_TEMPLATE Set BodyEn = 'Close Ticket  #Reference Number# on #Created Date#'
,BodyAr = 'Close Ticket  #Reference Number# on #Created Date#' where EventId = 94


---------------------Notiifcation Templates
Update NOTIF_NOTIFICATION_TEMPLATE Set
BodyEn = 'Bug Ticket #Reference Number# has been raised by #Sender Name# on #Created Date#'
,BodyAr = 'Bug Ticket #Reference Number# has been raised by #Sender Name# on #Created Date#' where EventId = 83
------------------------------------
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] 
([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId]) 
VALUES (192, N'G2G Bug Ticket Added', N'G2G Bug Ticket Added', N'System Generated', CAST(N'2022-09-06T12:27:53.330' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL)


INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (272, N'#Reference Number#', 192)
--------------------------
INSERT INTO NOTIF_NOTIFICATION_TEMPLATE (TemplateId, EventId, ChannelId, NameEn, NameAr, SubjectEn, SubjectAr, BodyEn,BodyAr, CreatedBy, CreatedDate, IsDeleted)
VALUES (NewID(), 192, 4, 'Bug Ticket Added From G2G ', 'Bug Ticket Added From G2G', 'Bug Ticket Added From G2G', 'Bug Ticket Added From G2G', 'Bug Ticket #Reference Number# has been raised by Government Entity on #Created Date#',N'Bug Ticket #Reference Number# has been raised by Government Entity on #Created Date#', 'System Generated', GETDATE(), 0);

------------------
Update NOTIF_NOTIFICATION_TEMPLATE Set
BodyEn = '#Sender Name# has added a comment on Ticket #Reference Number# on #Created Date#'
,BodyAr = '#Sender Name# has added a comment on Ticket #Reference Number# on #Created Date#' where EventId = 88
---------------

Update NOTIF_NOTIFICATION_TEMPLATE Set
BodyEn = 'Feedback has been added on the Ticket #Reference Number# by #Sender Name# on #Created Date#'
,BodyAr = 'Feedback has been added on the Ticket #Reference Number# by #Sender Name# on #Created Date#' where EventId = 89
-----------------------
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] 
([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId]) 
VALUES (193, N'G2G Feedback Added', N'G2G Feedback Added', N'System Generated', CAST(N'2022-09-06T12:27:53.330' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL)


INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (273, N'#Reference Number#', 193)
--------------------------
INSERT INTO NOTIF_NOTIFICATION_TEMPLATE (TemplateId, EventId, ChannelId, NameEn, NameAr, SubjectEn, SubjectAr, BodyEn,BodyAr, CreatedBy, CreatedDate, IsDeleted)
VALUES (NewID(), 193, 4, 'G2G Feedback Added', 'G2G Feedback Added', 'G2G Feedback Added', 'G2G Feedback Added', 'Feedback has been added on the Ticket #Reference Number# by Government Entity on #Created Date#',N'Feedback has been added on the Ticket #Reference Number# by Government Entity on #Created Date#', 'System Generated', GETDATE(), 0);

------------------------
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] 
([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId]) 
VALUES (194, N'FTW Feedback Added', N'FTW Feedback Added', N'System Generated', CAST(N'2022-09-06T12:27:53.330' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL)


INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (274, N'#Reference Number#', 194)
--------------------------
INSERT INTO NOTIF_NOTIFICATION_TEMPLATE (TemplateId, EventId, ChannelId, NameEn, NameAr, SubjectEn, SubjectAr, BodyEn,BodyAr, CreatedBy, CreatedDate, IsDeleted)
VALUES (NewID(), 194, 4, 'FTW Feedback Added', 'FTW Feedback Added', 'FTW Feedback Added', 'FTW Feedback Added', 'Feedback has been added on the Ticket #Reference Number# by IT Support Team on #Created Date#',N'Feedback has been added on the Ticket #Reference Number# by IT Support Team on #Created Date#', 'System Generated', GETDATE(), 0);

------------------

Update NOTIF_NOTIFICATION_TEMPLATE Set
BodyEn = 'Ticket #Reference Number# has been assigned to #Receiver Name# by #Sender Name# on #Created Date#'
,BodyAr = 'Ticket #Reference Number# has been assigned to #Receiver Name# by #Sender Name# on #Created Date#' where EventId = 90
----------

Update NOTIF_NOTIFICATION_TEMPLATE Set
BodyEn = 'Ticket #Reference Number# has been rejected by #Sender Name# on #Created Date#'
,BodyAr = 'Ticket #Reference Number# has been rejected by #Sender Name# on #Created Date#' where EventId = 91
-----------------------
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] 
([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId]) 
VALUES (195, N'FTW Reject Ticket', N'FTW Reject Ticket', N'System Generated', CAST(N'2022-09-06T12:27:53.330' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL)


INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (275, N'#Reference Number#', 195)
--------------------------
INSERT INTO NOTIF_NOTIFICATION_TEMPLATE (TemplateId, EventId, ChannelId, NameEn, NameAr, SubjectEn, SubjectAr, BodyEn,BodyAr, CreatedBy, CreatedDate, IsDeleted)
VALUES (NewID(), 195, 4, 'FTW Reject Ticket', 'FTW Reject Ticket', 'FTW Reject Ticket', 'FTW Reject Ticket', 'Ticket #Reference Number# has been rejected by IT Support Team on #Created Date#',N'Ticket #Reference Number# has been rejected by IT Support Team on #Created Date#', 'System Generated', GETDATE(), 0);

----------------------------
Update NOTIF_NOTIFICATION_TEMPLATE Set
BodyEn = 'Ticket #Reference Number# has been resolved by #Sender Name# on #Created Date#'
,BodyAr = 'Ticket #Reference Number# has been resolved by #Sender Name# on #Created Date#' where EventId = 92
----------------------------
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] 
([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId]) 
VALUES (196, N'FTW Resolve Ticket', N'FTW Resolve Ticket', N'System Generated', CAST(N'2022-09-06T12:27:53.330' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL)


INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (276, N'#Reference Number#', 196)
INSERT INTO NOTIF_NOTIFICATION_TEMPLATE (TemplateId, EventId, ChannelId, NameEn, NameAr, SubjectEn, SubjectAr, BodyEn,BodyAr, CreatedBy, CreatedDate, IsDeleted)
VALUES (NewID(), 196, 4, 'FTW Resolve Ticket', 'FTW Resolve Ticket', 'FTW Resolve Ticket', 'FTW Resolve Ticket', 'Ticket #Reference Number# has been resolved by IT Support Team on #Created Date#',N'Ticket #Reference Number# has been resolved by IT Support Team on #Created Date#', 'System Generated', GETDATE(), 0);
---------------

Update NOTIF_NOTIFICATION_TEMPLATE Set
BodyEn = 'Ticket #Reference Number# has been reopened by #Sender Name# on #Created Date#'
,BodyAr = 'Ticket #Reference Number# has been reopened by #Sender Name# on #Created Date#' where EventId = 93
-------------------------------
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] 
([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId]) 
VALUES (197, N'G2G ReOpen Ticket', N'G2G ReOpen Ticket', N'System Generated', CAST(N'2022-09-06T12:27:53.330' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL)


INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (277, N'#Reference Number#', 197)
INSERT INTO NOTIF_NOTIFICATION_TEMPLATE (TemplateId, EventId, ChannelId, NameEn, NameAr, SubjectEn, SubjectAr, BodyEn,BodyAr, CreatedBy, CreatedDate, IsDeleted)
VALUES (NewID(), 197, 4, 'G2G ReOpen Ticket', 'G2G ReOpen Ticket', 'G2G ReOpen Ticket', 'G2G ReOpen Ticket', 'Ticket #Reference Number# has been reopened by Government Entity on #Created Date#',N'Ticket #Reference Number# has been reopened by Government Entity on #Created Date#', 'System Generated', GETDATE(), 0);
----------------------------
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] 
([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId]) 
VALUES (198, N'G2G Close Ticket', N'G2G Close Ticket', N'System Generated', CAST(N'2022-09-06T12:27:53.330' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL)


INSERT [dbo].[NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP] ([PlaceHolderId], [PlaceHolderName], [EventId]) VALUES (278, N'#Reference Number#', 198)
INSERT INTO NOTIF_NOTIFICATION_TEMPLATE (TemplateId, EventId, ChannelId, NameEn, NameAr, SubjectEn, SubjectAr, BodyEn,BodyAr, CreatedBy, CreatedDate, IsDeleted)
VALUES (NewID(), 198, 4, 'G2G Close Ticket', 'G2G Close Ticket', 'G2G Close Ticket', 'G2G Close Ticket', 'Ticket #Reference Number# has been closed by Government Entity on #Created Date#',N'Ticket #Reference Number# has been closed by Government Entity on #Created Date#', 'System Generated', GETDATE(), 0);
----------------------

Update NOTIF_NOTIFICATION_TEMPLATE Set
BodyEn = 'Ticket #Reference Number# has been closed by #Sender Name# on #Created Date#'
,BodyAr = 'Ticket #Reference Number# has been closed by #Sender Name# on #Created Date#' where EventId = 94

-------------24/12/2024

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_CASE_PARTY_CATEGORY_G2G_LKP]') AND type in (N'U'))
update CMS_CASE_PARTY_CATEGORY_G2G_LKP set Name_Ar = N'المدعي عليه'
where id = 1
GO  
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_CASE_PARTY_CATEGORY_G2G_LKP]') AND type in (N'U'))
update CMS_CASE_PARTY_CATEGORY_G2G_LKP set Name_Ar = N'المدعي'
where id = 2
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_CASE_PARTY_CATEGORY_G2G_LKP]') AND type in (N'U'))
update CMS_CASE_PARTY_CATEGORY_G2G_LKP set  Name_Ar = N'طرف ثالث'
where id = 3
GO 
-----------------------------
UPDATE NOTIF_NOTIFICATION_TEMPLATE Set 
BodyEn = 'Alert! #Assignee NameEn# has not taken any action on task #SubjectEn# for #Entity# #Reference Number# which is assigned by #Assignor NameEn# on #Created Date#'
, BodyAr = 'تنبيه! #Assignee NameAr# لم يتخذ أي إجراء على المهمة #SubjectAr# الخاصة بـ #Entity# #Reference Number# والتي تم تعيينها من قبل #Assignor NameAr# بتاريخ #Created Date#'
where EventId = 165

---------------For Bug Reporting Notification-----------
INSERT INTO NOTIF_NOTIFICATION_TEMPLATE VALUES (NewId(), 88 , 4 , 'Ticket Comment Added Successfully' ,'Ticket Comment Added Successfully','Ticket Comment Added Successfully','Ticket Comment Added Successfully' ,'#Sender Name# has added a comment on Ticket #Reference Number# on #Created Date#','#Sender Name# has added a comment on Ticket #Reference Number# on #Created Date#',null , 'fatwaadmin@gmail.com' ,GetDate(),null , null ,null,null , 0 ,1)
INSERT INTO NOTIF_NOTIFICATION_TEMPLATE VALUES (NewId(), 89 , 4 , 'Ticket Feedback Added Successfully' ,'Ticket Feedback Added Successfully','Ticket Feedback Added Successfully','Ticket Feedback Added Successfully' ,'Feedback has been added on the Ticket #Reference Number# by #Sender Name# on #Created Date#','Feedback has been added on the Ticket #Reference Number# by #Sender Name# on #Created Date#',null , 'fatwaadmin@gmail.com' ,GetDate(),null , null ,null,null , 0 ,1)
INSERT INTO NOTIF_NOTIFICATION_TEMPLATE VALUES (NewId(), 90 , 4 , 'Assign Ticket' ,'Assign Ticket','Assign Ticket','Assign Ticket' ,'Ticket #Reference Number# has been assigned to #Receiver Name# by #Sender Name# on #Created Date#','Ticket #Reference Number# has been assigned to #Receiver Name# by #Sender Name# on #Created Date#',null , 'fatwaadmin@gmail.com' ,GetDate(),null , null ,null,null , 0 ,1)
INSERT INTO NOTIF_NOTIFICATION_TEMPLATE VALUES (NewId(), 91 , 4 , 'Reject Ticket' ,'Reject Ticket','Reject Ticket','Reject Ticket' ,'Ticket #Reference Number# has been rejected by #Sender Name# on #Created Date#','Ticket #Reference Number# has been rejected by #Sender Name# on #Created Date#',null , 'fatwaadmin@gmail.com' ,GetDate(),null , null ,null,null , 0 ,1)
INSERT INTO NOTIF_NOTIFICATION_TEMPLATE VALUES (NewId(), 92 , 4 , 'Resolve Ticket ' ,'Resolve Ticket ','Resolve Ticket ','Resolve Ticket ' ,'Ticket #Reference Number# has been resolved by #Sender Name# on #Created Date#','Ticket #Reference Number# has been resolved by #Sender Name# on #Created Date#',null , 'fatwaadmin@gmail.com' ,GetDate(),null , null ,null,null , 0 ,1)
INSERT INTO NOTIF_NOTIFICATION_TEMPLATE VALUES (NewId(), 93 , 4 , 'ReOpen Ticket' ,'ReOpen Ticket','ReOpen Ticket','ReOpen Ticket' ,'Ticket #Reference Number# has been reopened by #Sender Name# on #Created Date#','Ticket #Reference Number# has been reopened by #Sender Name# on #Created Date#',null , 'fatwaadmin@gmail.com' ,GetDate(),null , null ,null,null , 0 ,1)
INSERT INTO NOTIF_NOTIFICATION_TEMPLATE VALUES (NewId(), 94 , 4 , 'Close Ticket' ,'Close Ticket','Close Ticket','Close Ticket' ,'Ticket #Reference Number# has been closed by #Sender Name# on #Created Date#','Ticket #Reference Number# has been closed by #Sender Name# on #Created Date#',null , 'fatwaadmin@gmail.com' ,GetDate(),null , null ,null,null , 0 ,1)
--------------------------------------------------------

-------------------------------------------------31/12/24    BUG_APPLICATION_G2G_LKP
IF((SELECT COUNT(*) FROM BUG_APPLICATION_G2G_LKP WHERE Id=1) > 0)
UPDATE BUG_APPLICATION_G2G_LKP SET Name_Ar = N'بوابة الفتوى' WHERE Id=1
GO
IF((SELECT COUNT(*) FROM BUG_APPLICATION_G2G_LKP WHERE Id=2) > 0)
UPDATE BUG_APPLICATION_G2G_LKP SET Name_Ar = N'بوابة إدارة الفتوى' WHERE Id=2
GO
IF((SELECT COUNT(*) FROM BUG_APPLICATION_G2G_LKP WHERE Id=4) > 0)
UPDATE BUG_APPLICATION_G2G_LKP SET Name_Ar = N'نظام إدارة الوثائق' WHERE Id=4
GO
IF((SELECT COUNT(*) FROM BUG_APPLICATION_G2G_LKP WHERE Id=8) > 0)
UPDATE BUG_APPLICATION_G2G_LKP SET Name_Ar = N'بوابة الحكومة إلى الحكومة' WHERE Id=8
GO
IF((SELECT COUNT(*) FROM BUG_APPLICATION_G2G_LKP WHERE Id=16) > 0)
UPDATE BUG_APPLICATION_G2G_LKP SET Name_Ar = N'بوابة إدارة الحكومة إلى الحكومة' WHERE Id=16
GO
IF((SELECT COUNT(*) FROM BUG_APPLICATION_G2G_LKP WHERE Id=32) > 0)
UPDATE BUG_APPLICATION_G2G_LKP SET Name_Ar = N'نظام دعم العمليات' WHERE Id=32
GO
 
--------------------------
IF((SELECT COUNT(*) FROM BUG_EVENT_G2G_LKP WHERE Id = 4096) > 0)
UPDATE BUG_EVENT_G2G_LKP SET Name_Ar = N'تم حذف التعليق' WHERE Id = 4096
GO
IF((SELECT COUNT(*) FROM BUG_EVENT_G2G_LKP WHERE Id =8192) > 0)
UPDATE BUG_EVENT_G2G_LKP SET Name_Ar = N'تم تحرير التعليق' WHERE Id =8192
GO
--------------
---------------------------------------
----------------------------05/jan/2025


INSERT INTO BUG_EVENT_G2G_LKP (Id, Name_En, Name_Ar, CreatedBy, CreatedDate, ModifiedBy, ModifiedDate, DeletedBy, DeletedDate, IsDeleted)
SELECT 4096, 'Comment Deleted', 'Comment Deleted', 'superadmin@gmail.com', GETDATE(), NULL, NULL, NULL, NULL, 0
WHERE NOT EXISTS (SELECT 1 FROM BUG_EVENT_G2G_LKP WHERE Id = 4096);

INSERT INTO BUG_EVENT_G2G_LKP (Id, Name_En, Name_Ar, CreatedBy, CreatedDate, ModifiedBy, ModifiedDate, DeletedBy, DeletedDate, IsDeleted)
SELECT 8192, 'Comment Edited', 'Comment Edited', 'superadmin@gmail.com', GETDATE(), NULL, NULL, NULL, NULL, 0
WHERE NOT EXISTS (SELECT 1 FROM BUG_EVENT_G2G_LKP WHERE Id = 8192);
--------------------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BUG_TICKET]') AND type in (N'U'))
BEGIN
UPDATE BUG_TICKET
SET Subject = LEFT(Subject, 150)
WHERE LEN(Subject) > 150;
END
IF COLUMNPROPERTY(OBJECT_ID('dbo.BUG_TICKET'), 'Subject', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE BUG_TICKET ALTER COLUMN Subject NVARCHAR(512);
END
-------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BUG_REPORTED]') AND type in (N'U'))
BEGIN
UPDATE BUG_REPORTED
SET Subject = LEFT(Subject, 150)
WHERE LEN(Subject) > 150;
END
IF COLUMNPROPERTY(OBJECT_ID('dbo.BUG_REPORTED'), 'Subject', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE BUG_REPORTED ALTER COLUMN Subject NVARCHAR(512);
END
--------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BUG_REPORTED]') AND type in (N'U'))
BEGIN
UPDATE BUG_REPORTED
SET ScreenReference = LEFT(ScreenReference, 30)
WHERE LEN(ScreenReference) > 30;
END
IF COLUMNPROPERTY(OBJECT_ID('dbo.BUG_REPORTED'), 'ScreenReference', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE BUG_REPORTED ALTER COLUMN ScreenReference NVARCHAR(50);
END
---------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BUG_TICKET_STATUS_HISTORY]') AND type in (N'U'))
BEGIN
UPDATE BUG_TICKET_STATUS_HISTORY
SET Remarks = LEFT(Remarks, 1000)
WHERE LEN(Remarks) > 1000;
END
IF COLUMNPROPERTY(OBJECT_ID('dbo.BUG_TICKET_STATUS_HISTORY'), 'Remarks', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE BUG_TICKET_STATUS_HISTORY ALTER COLUMN Remarks NVARCHAR(1024);
END
-----------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BUG_ISSUE_TYPE_G2G_LKP]') AND type in (N'U'))
BEGIN
UPDATE BUG_ISSUE_TYPE_G2G_LKP
SET Type_En = LEFT(Type_En, 90)
WHERE LEN(Type_En) > 90;
END
IF COLUMNPROPERTY(OBJECT_ID('dbo.BUG_ISSUE_TYPE_G2G_LKP'), 'Type_En', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE BUG_ISSUE_TYPE_G2G_LKP ALTER COLUMN Type_En NVARCHAR(150);
END
------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BUG_ISSUE_TYPE_G2G_LKP]') AND type in (N'U'))
BEGIN
UPDATE BUG_ISSUE_TYPE_G2G_LKP
SET Type_Ar = LEFT(Type_Ar, 90)
WHERE LEN(Type_Ar) > 90;
END
IF COLUMNPROPERTY(OBJECT_ID('dbo.BUG_ISSUE_TYPE_G2G_LKP'), 'Type_Ar', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE BUG_ISSUE_TYPE_G2G_LKP ALTER COLUMN Type_Ar NVARCHAR(150);
END
---------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BUG_ISSUE_TYPE_G2G_LKP]') AND type in (N'U'))
BEGIN
UPDATE BUG_ISSUE_TYPE_G2G_LKP
SET Description = LEFT(Description, 1000)
WHERE LEN(Description) > 1000;
END
IF COLUMNPROPERTY(OBJECT_ID('dbo.BUG_ISSUE_TYPE_G2G_LKP'), 'Description', 'ColumnId') IS NOT NULL
BEGIN 
	ALTER TABLE BUG_ISSUE_TYPE_G2G_LKP ALTER COLUMN Description NVARCHAR(1024);
END


-----Admin portal Menu/Page Claims

delete from UMS_CLAIM where  ClaimValue='Admin.Permissions.Menu.UMS'
delete from UMS_CLAIM where  ClaimValue='Permissions.AutoMonInterface.Process.List'
delete from UMS_CLAIM where  ClaimValue='Permissions.AutoMonInterface.CaseData.Extraction'
delete from UMS_CLAIM where  ClaimValue='Permissions.Menu.AutoMonInterface'
delete from UMS_CLAIM where  ClaimValue='Admin.Permissions.Users.Translations'
delete from UMS_CLAIM where  ClaimValue='Admin.Permissions.Submenu.WebSystem'
delete from UMS_CLAIM where  ClaimValue='Admin.Permissions.Submenu.GroupAccessType'
delete from UMS_CLAIM where  ClaimValue='Admin.Permissions.Submenu.UMS.Groups'
delete from UMS_CLAIM where  ClaimValue='Admin.Permissions.Submenu.UMS.Roles'
delete from UMS_CLAIM where  ClaimValue='Admin.Permissions.Submenu.UMS.Claims'
delete from UMS_CLAIM where  ClaimValue='Admin.Permissions.Submenu.UMS.EmployeesList'

update UMS_GROUP_CLAIMS set ClaimValue='Admin.Permissions.Menu.AutoMonInterface' where ClaimValue='Permissions.Menu.AutoMonInterface'
update UMS_USER_CLAIMS set ClaimValue='Admin.Permissions.Menu.AutoMonInterface' where ClaimValue='Permissions.Menu.AutoMonInterface'
update UMS_GROUP_CLAIMS set ClaimValue='Admin.Permissions.Menu.UMS' where ClaimValue in ('Admin.Permissions.Submenu.WebSystem','Admin.Permissions.Submenu.GroupAccessType','Admin.Permissions.Submenu.UMS.Groups','Admin.Permissions.Submenu.UMS.EmployeesList','Admin.Permissions.Submenu.UMS.Claims','Admin.Permissions.Submenu.UMS.Roles')
update UMS_USER_CLAIMS set ClaimValue='Admin.Permissions.Menu.UMS' where ClaimValue in ('Admin.Permissions.Submenu.WebSystem','Admin.Permissions.Submenu.GroupAccessType','Admin.Permissions.Submenu.UMS.Groups','Admin.Permissions.Submenu.UMS.EmployeesList','Admin.Permissions.Submenu.UMS.Claims','Admin.Permissions.Submenu.UMS.Roles')
delete from UMS_GROUP_CLAIMS where ClaimValue in ('Permissions.Menu.AutoMonInterface','Permissions.AutoMonInterface.Process.List','Permissions.AutoMonInterface.CaseData.Extraction')
delete from UMS_USER_CLAIMS  where ClaimValue in ('Permissions.Menu.AutoMonInterface','Permissions.AutoMonInterface.Process.List','Permissions.AutoMonInterface.CaseData.Extraction')
delete from UMS_GROUP_CLAIMS where  ClaimValue in ('Admin.Permissions.Submenu.WebSystem','Admin.Permissions.Submenu.GroupAccessType','Admin.Permissions.Submenu.UMS.Groups','Admin.Permissions.Submenu.UMS.EmployeesList','Admin.Permissions.Submenu.UMS.Claims','Admin.Permissions.Submenu.UMS.Roles')
delete from UMS_USER_CLAIMS  where  ClaimValue in ('Admin.Permissions.Submenu.WebSystem','Admin.Permissions.Submenu.GroupAccessType','Admin.Permissions.Submenu.UMS.Groups','Admin.Permissions.Submenu.UMS.EmployeesList','Admin.Permissions.Submenu.UMS.Claims','Admin.Permissions.Submenu.UMS.Roles')


GO 
IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Admin.Permissions.Menu.UMS')
	INSERT INTO [dbo].[UMS_CLAIM] 
	([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted],[ModuleId])
		VALUES 
('User Management','FATWA_ADMINISTRATION_PORTAL', 'Menu', 'Permission', 'Admin.Permissions.Menu.UMS',N'بوابة إدارة نظام الفتوى',0,1073741824)

IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Admin.Permissions.Menu.Notfication')
	INSERT INTO [dbo].[UMS_CLAIM] 
	([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted],[ModuleId])
		VALUES 
('Notifications Event','FATWA_ADMINISTRATION_PORTAL', 'Menu', 'Permission', 'Admin.Permissions.Menu.Notfication',N'Notifications Event',0,1073741824)

IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Admin.Permissions.Menu.UMS.Configuration')
	INSERT INTO [dbo].[UMS_CLAIM] 
	([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted],[ModuleId])
		VALUES 
('UMS_Configuration','FATWA_ADMINISTRATION_PORTAL', 'Menu', 'Permission', 'Admin.Permissions.Menu.UMS.Configuration',N'UMS_Configuration',0,1073741824)

IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Admin.Permissions.Users.SystemConfiguration')
	INSERT INTO [dbo].[UMS_CLAIM] 
	([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted],[ModuleId])
		VALUES 
('Create System Setting','FATWA_ADMINISTRATION_PORTAL', 'Menu', 'Permission', 'Admin.Permissions.Users.SystemConfiguration',N'إعدادات النظام',0,1073741824)

IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Admin.Permissions.Menu.LLS.Configuration')
	INSERT INTO [dbo].[UMS_CLAIM] 
	([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted],[ModuleId])
		VALUES 
('LLS Configuration','FATWA_ADMINISTRATION_PORTAL', 'Menu', 'Permission', 'Admin.Permissions.Menu.LLS.Configuration',N'إعدادات نظام إدارة المكتبات',0,1073741824)

IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Admin.Permissions.Menu.CMS.Configuration')
	INSERT INTO [dbo].[UMS_CLAIM] 
	([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted],[ModuleId])
		VALUES 
('Case Configuration','FATWA_ADMINISTRATION_PORTAL', 'Menu', 'Permission', 'Admin.Permissions.Menu.CMS.Configuration',N'إعدادات نظام القضايا',0,1073741824)

IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Admin.Permissions.Menu.COMS.Configuration')
	INSERT INTO [dbo].[UMS_CLAIM] 
	([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted],[ModuleId])
		VALUES 
('Consultation Configuration','FATWA_ADMINISTRATION_PORTAL', 'Menu', 'Permission', 'Admin.Permissions.Menu.COMS.Configuration',N'إعدادات نظام الاستشاري',0,1073741824)

IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Admin.Permissions.Menu.DMS.Configuration')
	INSERT INTO [dbo].[UMS_CLAIM] 
	([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted],[ModuleId])
		VALUES 
('DMS Configuration','FATWA_ADMINISTRATION_PORTAL', 'Menu', 'Permission', 'Admin.Permissions.Menu.DMS.Configuration',N'إعدادات نظام إدارة المستندات',0,1073741824)

IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Admin.Permissions.Menu.Common.Configuration')
	INSERT INTO [dbo].[UMS_CLAIM] 
	([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted],[ModuleId])
		VALUES 
('Common Configuration','FATWA_ADMINISTRATION_PORTAL', 'Menu', 'Permission', 'Admin.Permissions.Menu.Common.Configuration',N'الإعدادات العامة',0,1073741824)

IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Admin.Permissions.Users.Translations')
	INSERT INTO [dbo].[UMS_CLAIM] 
	([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted],[ModuleId])
		VALUES 
('Translations','FATWA_ADMINISTRATION_PORTAL', 'Menu', 'Permission', 'Admin.Permissions.Users.Translations',N'الترجمات',0,1073741824)

IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Admin.Permissions.Menu.BackgroundServices')
	INSERT INTO [dbo].[UMS_CLAIM] 
	([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted],[ModuleId])
		VALUES 
('Background Services','FATWA_ADMINISTRATION_PORTAL', 'Menu', 'Permission', 'Admin.Permissions.Menu.BackgroundServices',N'خدمات النظام',0,1073741824)

IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Admin.Permissions.Menu.AutoMonInterface')
	INSERT INTO [dbo].[UMS_CLAIM] 
	([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted],[ModuleId])
		VALUES 
('Monitoring Interface','FATWA_ADMINISTRATION_PORTAL', 'Menu', 'Permission', 'Admin.Permissions.Menu.AutoMonInterface',N'Monitoring Interface',0,1073741824)
 
IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Admin.Permissions.Menu.Logs')
	INSERT INTO [dbo].[UMS_CLAIM] 
	([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted],[ModuleId])
		VALUES 
('Logs','FATWA_ADMINISTRATION_PORTAL', 'Menu', 'Permission', 'Admin.Permissions.Menu.Logs',N'السجلات',0,1073741824)

IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Admin.Permissions.Menu.CaseData.Extraction')
	INSERT INTO [dbo].[UMS_CLAIM] 
	([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted],[ModuleId])
		VALUES 
('Service Request Flow Setup','FATWA_ADMINISTRATION_PORTAL', 'Menu', 'Permission', 'Admin.Permissions.Menu.CaseData.Extraction',N'إعداد تدفق طلب الخدمة',0,1073741824)

IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Admin.Permissions.Menu.DMS.Management')
	INSERT INTO [dbo].[UMS_CLAIM] 
	([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted],[ModuleId])
		VALUES 
('Document Management','FATWA_ADMINISTRATION_PORTAL', 'Menu', 'Permission', 'Admin.Permissions.Menu.DMS.Management',N'إدارة المستندات',0,1073741824)

IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Admin.Permissions.Menu.BugReporting')
	INSERT INTO [dbo].[UMS_CLAIM] 
	([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted],[ModuleId])
		VALUES 
('Bug Reporting','FATWA_ADMINISTRATION_PORTAL', 'Menu', 'Permission', 'Admin.Permissions.Menu.BugReporting',N'الإبلاغ عن الأخطاء',0,1073741824)
-------------------------------------
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId]) VALUES (202, N'FTW Mention User Notification', N'FTW Mention User Notification', N'System Generated', CAST(N'2024-03-24T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL)
INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP (PlaceHolderId ,PlaceHolderName , EventId) VALUES (281,'#Reference Number#' , 202)

------------
INSERT INTO NOTIF_NOTIFICATION_TEMPLATE (TemplateId, EventId, ChannelId, NameEn, NameAr ,SubjectEn, SubjectAr, BodyEn, BodyAr,CreatedBy , CreatedDate , IsDeleted )
VALUES (NewID(),202, 4, 'User Mentioned Sucessfully' , N'User Mentioned Sucessfully',  'User Mentioned Sucessfully' , N'User Mentioned Sucessfully',
'IT Support Team has mentioned #Receiver Name#''s in a comment on ticket #Reference Number# on #Created Date#', 
N'IT Support Team has mentioned #Receiver Name#''s in a comment on ticket #Reference Number# on #Created Date#', 
'fatwaadmin@gmail.com' ,'2024-04-04 11:41:43.297',0);
----------------------------------
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId]) VALUES (203, N'Mention User Notification', N'Mention User Notification', N'System Generated', CAST(N'2024-03-24T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL)
INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP (PlaceHolderId ,PlaceHolderName , EventId) VALUES (282,'#Reference Number#' , 203)

------------
INSERT INTO NOTIF_NOTIFICATION_TEMPLATE (TemplateId, EventId, ChannelId, NameEn, NameAr ,SubjectEn, SubjectAr, BodyEn, BodyAr,CreatedBy , CreatedDate , IsDeleted )
VALUES (NewID(),203, 4, 'User Mentioned Sucessfully' , N'User Mentioned Sucessfully',  'User Mentioned Sucessfully' , N'User Mentioned Sucessfully',
'#Sender Name# has mentioned #Receiver Name#''s in a comment on ticket #Reference Number# on #Created Date#', 
N'#Sender Name# has mentioned #Receiver Name#''s in a comment on ticket #Reference Number# on #Created Date#', 
'fatwaadmin@gmail.com' ,'2024-04-04 11:41:43.297',0);

----------------------------------------
INSERT [dbo].[NOTIF_NOTIFICATION_EVENT] ([EventId], [NameEn], [NameAr], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted], [ReceiverTypeId], [ReceiverTypeRefId]) VALUES (204, N'G2G Mention User Notification', N'G2G Mention User Notification', N'System Generated', CAST(N'2024-03-24T11:41:43.297' AS DateTime), NULL, NULL, NULL, NULL, 0, 1, NULL)
INSERT INTO NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP (PlaceHolderId ,PlaceHolderName , EventId) VALUES (283,'#Reference Number#' , 204)

------------
INSERT INTO NOTIF_NOTIFICATION_TEMPLATE (TemplateId, EventId, ChannelId, NameEn, NameAr ,SubjectEn, SubjectAr, BodyEn, BodyAr,CreatedBy , CreatedDate , IsDeleted )
VALUES (NewID(),204, 4, 'User Mentioned Sucessfully' , N'User Mentioned Sucessfully',  'User Mentioned Sucessfully' , N'User Mentioned Sucessfully',
'GE User has mentioned #Receiver Name#''s in a comment on ticket #Reference Number# on #Created Date#', 
N'GE User has mentioned #Receiver Name#''s in a comment on ticket #Reference Number# on #Created Date#', 
'fatwaadmin@gmail.com' ,'2024-04-04 11:41:43.297',0);
-----
GO
IF((SELECT COUNT(*) FROM UMS_CLAIM WHERE ClaimValue = 'Permissions.Submenu.Document.DocumentList') = 0)
BEGIN
INSERT INTO UMS_CLAIM VALUES ('Document Management System','Document_Management_System','Document_List', 'Permission', 'Permissions.Submenu.Document.DocumentList','Document_List',0,16)
END
-------------------------------
Update LOOKUPS_HISTORY Set DescriptionAr = Description where LookupsTableId = 12 AND DescriptionAr IS NULL


UPDATE NOTIF_NOTIFICATION_TEMPLATE SET BodyEn = 'Meeting regarding #Entity# sent by #Sender Name# on #Created Date# is pending for your decision'



---- 21-04-2025----


update UMS_CLAIM set Title_Ar=N'قائمة سجل وقود السيارة' where Title_En='List Of Car Fuel Record (SubMenu)' and ModuleId=4194304
update UMS_CLAIM set Title_Ar=N'إضافة سجل وقود السيارة' where Title_En='Add Car Fuel Record (SubMenu)' and ModuleId=8192


UPDATE CMS_OPERATING_SECTOR_TYPE_G2G_LKP SET Name_En = 'Administrative Underfiling Sector' WHERE Id = 1
UPDATE CMS_OPERATING_SECTOR_TYPE_G2G_LKP SET Name_En = 'Civil\Commercial Underfiling Sector' WHERE Id = 5

insert into NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP values ((select max(PlaceHolderId)+1 from NOTIF_NOTIFICATION_EVENT_PLACEHOLDERS_LKP),'#Sector To#',150)
UPDATE NOTIF_NOTIFICATION_TEMPLATE SET BodyEn = 'Meeting regarding #Entity# sent by #Sender Name# on #Created Date# is pending for your decision' where EventId=21

UPDATE BUG_ISSUE_TYPE_G2G_LKP SET Type_Ar = N'لم يتم تحميل البيانات' where Id = 2


-----------------------------------
Update Submodule Set Name_Ar = N'طلبات رفع قضية' where Id = 1
Update Submodule Set Name_Ar = N'ملف القضية' where Id = 2
Update Submodule Set Name_Ar = N'القضايا المسجلة' where Id = 4
Update Submodule Set Name_Ar = N'طلب الاستشاري' where Id = 8
Update Submodule Set Name_Ar = N'ملف الاستشاري' where Id = 64
Update Submodule set Name_Ar = N'مراجعة وثيقة إدارة الوثائق' where Id = 128
Update Submodule set Name_Ar = N'اللجنة المنظمة' where Id = 256
Update Submodule set Name_Ar = N'الاجازة والحضور' where Id = 512

