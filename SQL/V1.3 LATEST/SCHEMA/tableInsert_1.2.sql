

SET IDENTITY_INSERT [dbo].[CMS_MINISTRY_G2G_LKP] ON 

INSERT [dbo].[CMS_MINISTRY_G2G_LKP] ([Id], [Name_En], [Name_Ar], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (1, N'Education Ministry', N'Education Ministry', 1, N'superadmin@gmail.com', CAST(N'2022-08-30T20:12:46.453' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[CMS_MINISTRY_G2G_LKP] ([Id], [Name_En], [Name_Ar], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (2, N'Foreign Ministry', N'Foreign Ministry', 1, N'superadmin@gmail.com', CAST(N'2022-08-30T20:12:46.453' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[CMS_MINISTRY_G2G_LKP] ([Id], [Name_En], [Name_Ar], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (3, N'Finance Ministry', N'Finance Ministry', 1, N'superadmin@gmail.com', CAST(N'2022-08-30T20:12:46.453' AS DateTime), NULL, NULL, NULL, NULL, 0)
SET IDENTITY_INSERT [dbo].[CMS_MINISTRY_G2G_LKP] OFF
GO

SET IDENTITY_INSERT [dbo].[CMS_PRIORITY_G2G_LKP] ON 

INSERT [dbo].[CMS_PRIORITY_G2G_LKP] ([Id], [Name_En], [Name_Ar]) VALUES (1, N'Low', N'Low')
INSERT [dbo].[CMS_PRIORITY_G2G_LKP] ([Id], [Name_En], [Name_Ar]) VALUES (2, N'Medium', N'Medium')
INSERT [dbo].[CMS_PRIORITY_G2G_LKP] ([Id], [Name_En], [Name_Ar]) VALUES (3, N'High', N'High')
SET IDENTITY_INSERT [dbo].[CMS_PRIORITY_G2G_LKP] OFF
GO

SET IDENTITY_INSERT [dbo].[CMS_COURT_TYPE_G2G_LKP] ON 

INSERT [dbo].[CMS_COURT_TYPE_G2G_LKP] ([Id], [Name_En], [Name_Ar], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (1, N'Regional', N'Regional', 1, N'fatwaadmin@gmail.com', CAST(N'2022-10-11T17:49:54.350' AS DateTime), NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[CMS_COURT_TYPE_G2G_LKP] ([Id], [Name_En], [Name_Ar], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (2, N'Supreme', N'Supreme', 1, N'fatwaadmin@gmail.com', CAST(N'2022-10-11T17:49:54.350' AS DateTime), NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[CMS_COURT_TYPE_G2G_LKP] ([Id], [Name_En], [Name_Ar], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (3, N'Appeal', N'Appeal', 1, N'fatwaadmin@gmail.com', CAST(N'2022-10-11T17:49:54.350' AS DateTime), NULL, NULL, NULL, NULL, 1)
SET IDENTITY_INSERT [dbo].[CMS_COURT_TYPE_G2G_LKP] OFF
GO


SET IDENTITY_INSERT [dbo].[CMS_COURT_G2G_LKP] ON 

INSERT [dbo].[CMS_COURT_G2G_LKP] ([Id], [Number], [Name_En], [Name_Ar], [District], [Location], [TypeId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (1, N'234SDFSD23', N'RWP District Court', N'RWP District Court', N'Rawalpindi', N'Rawalpindi', 1, 1, N'fatwaadmin@gmail.com', CAST(N'2022-10-11T17:49:54.350' AS DateTime), NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[CMS_COURT_G2G_LKP] ([Id], [Number], [Name_En], [Name_Ar], [District], [Location], [TypeId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (2, N'1224S32D23', N'High Court', N'High Court', N'Islamabad', N'Islamabad', 2, 1, N'fatwaadmin@gmail.com', CAST(N'2022-10-11T17:49:54.350' AS DateTime), NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[CMS_COURT_G2G_LKP] ([Id], [Number], [Name_En], [Name_Ar], [District], [Location], [TypeId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (3, N'1224S23423', N'Supereme Court', N'Supereme Court', N'Islamabad', N'Islamabad', 1, 1, N'fatwaadmin@gmail.com', CAST(N'2022-10-11T17:49:54.350' AS DateTime), NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[CMS_COURT_G2G_LKP] ([Id], [Number], [Name_En], [Name_Ar], [District], [Location], [TypeId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (4, N'234SDFSD23', N'JHLM District Court', N'JHLM District Court', N'Jhelum', N'Jhelum', 1, 1, N'fatwaadmin@gmail.com', CAST(N'2022-10-11T17:49:54.350' AS DateTime), NULL, NULL, NULL, NULL, 1)
SET IDENTITY_INSERT [dbo].[CMS_COURT_G2G_LKP] OFF
GO


SET IDENTITY_INSERT [dbo].[CMS_CHAMBER_G2G_LKP] ON 

INSERT [dbo].[CMS_CHAMBER_G2G_LKP] ([Id], [Number], [Name_En], [Name_Ar], [Address], [CourtId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (1, N'KFN23K23', N'Barristers Chamber', N'Barristers Chamber', N'Islamabad', 2, 1, N'fatwaadmin@gmail.com', CAST(N'2022-10-11T17:49:54.350' AS DateTime), NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[CMS_CHAMBER_G2G_LKP] ([Id], [Number], [Name_En], [Name_Ar], [Address], [CourtId], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (2, N'KJSDVN23', N'Judges Chamber', N'Judges Chamber', N'Islamabad', 2, 1, N'fatwaadmin@gmail.com', CAST(N'2022-10-11T17:49:54.350' AS DateTime), NULL, NULL, NULL, NULL, 1)
SET IDENTITY_INSERT [dbo].[CMS_CHAMBER_G2G_LKP] OFF
GO

SET IDENTITY_INSERT [dbo].[CMS_OPERATING_SECTOR_TYPE_G2G_LKP] ON 
INSERT [dbo].[CMS_OPERATING_SECTOR_TYPE_G2G_LKP] ([Id], [Code], [Name_En], [Name_Ar], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (1, N'ADMN', N'Administrative', N'Administrative', 1, N'superadmin@gmail.com', CAST(N'2022-08-30T20:12:46.453' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[CMS_OPERATING_SECTOR_TYPE_G2G_LKP] ([Id], [Code], [Name_En], [Name_Ar], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (2, N'CC', N'Civil/Commercial', N'Civil/Commercial', 1, N'superadmin@gmail.com', CAST(N'2022-08-30T20:12:46.453' AS DateTime), NULL, NULL, NULL, NULL, 0)
SET IDENTITY_INSERT [dbo].[CMS_OPERATING_SECTOR_TYPE_G2G_LKP] OFF
GO

SET IDENTITY_INSERT [dbo].[CMS_SUBTYPE_G2G_LKP] ON 

INSERT [dbo].[CMS_SUBTYPE_G2G_LKP] ([Id], [SectorTypeId], [Code], [Name_En], [Name_Ar], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (1, 1, N'RGNL', N'Regional', N'Regional', 1, N'superadmin@gmail.com', CAST(N'2022-08-30T20:12:46.453' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[CMS_SUBTYPE_G2G_LKP] ([Id], [SectorTypeId], [Code], [Name_En], [Name_Ar], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (2, 1, N'ADMN', N'Administrative', N'Administrative', 1, N'superadmin@gmail.com', CAST(N'2022-08-30T20:12:46.453' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[CMS_SUBTYPE_G2G_LKP] ([Id], [SectorTypeId], [Code], [Name_En], [Name_Ar], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (3, 1, N'SPRM', N'Supreme', N'Supreme', 1, N'superadmin@gmail.com', CAST(N'2022-08-30T20:12:46.453' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[CMS_SUBTYPE_G2G_LKP] ([Id], [SectorTypeId], [Code], [Name_En], [Name_Ar], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (4, 1, N'APPL', N'Appeal', N'Appeal', 1, N'superadmin@gmail.com', CAST(N'2022-08-30T20:12:46.453' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[CMS_SUBTYPE_G2G_LKP] ([Id], [SectorTypeId], [Code], [Name_En], [Name_Ar], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (5, 2, N'RGNL', N'Regional', N'Regional', 1, N'superadmin@gmail.com', CAST(N'2022-08-30T20:12:46.453' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[CMS_SUBTYPE_G2G_LKP] ([Id], [SectorTypeId], [Code], [Name_En], [Name_Ar], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (6, 2, N'ADMN', N'Administrative', N'Administrative', 1, N'superadmin@gmail.com', CAST(N'2022-08-30T20:12:46.453' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[CMS_SUBTYPE_G2G_LKP] ([Id], [SectorTypeId], [Code], [Name_En], [Name_Ar], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (7, 2, N'SPRM', N'Supreme', N'Supreme', 1, N'superadmin@gmail.com', CAST(N'2022-08-30T20:12:46.453' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[CMS_SUBTYPE_G2G_LKP] ([Id], [SectorTypeId], [Code], [Name_En], [Name_Ar], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (8, 2, N'APPL', N'Appeal', N'Appeal', 1, N'superadmin@gmail.com', CAST(N'2022-08-30T20:12:46.453' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[CMS_SUBTYPE_G2G_LKP] ([Id], [SectorTypeId], [Code], [Name_En], [Name_Ar], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (9, 2, N'URGNT', N'Urgent & Partial', N'Urgent & Partial', 1, N'superadmin@gmail.com', CAST(N'2022-08-30T20:12:46.453' AS DateTime), NULL, NULL, NULL, NULL, 0)
SET IDENTITY_INSERT [dbo].[CMS_SUBTYPE_G2G_LKP] OFF
GO

SET IDENTITY_INSERT [dbo].[CMS_CASE_REQUEST_STATUS_G2G_LKP] ON 
INSERT [dbo].[CMS_CASE_REQUEST_STATUS_G2G_LKP] ([Id], [Name_En], [Name_Ar]) VALUES (1, N'New', N'New')
INSERT [dbo].[CMS_CASE_REQUEST_STATUS_G2G_LKP] ([Id], [Name_En], [Name_Ar]) VALUES (2, N'Draft', N'Draft')
INSERT [dbo].[CMS_CASE_REQUEST_STATUS_G2G_LKP] ([Id], [Name_En], [Name_Ar]) VALUES (3, N'Submitted', N'Submitted')
INSERT [dbo].[CMS_CASE_REQUEST_STATUS_G2G_LKP] ([Id], [Name_En], [Name_Ar]) VALUES (4, N'Resubmitted', N'Resubmitted')
INSERT [dbo].[CMS_CASE_REQUEST_STATUS_G2G_LKP] ([Id], [Name_En], [Name_Ar]) VALUES (5, N'Rejected', N'Rejected')
INSERT [dbo].[CMS_CASE_REQUEST_STATUS_G2G_LKP] ([Id], [Name_En], [Name_Ar]) VALUES (6, N'Accepted', N'Accepted')
INSERT [dbo].[CMS_CASE_REQUEST_STATUS_G2G_LKP] ([Id], [Name_En], [Name_Ar]) VALUES (7, N'Withdrawn', N'Withdrawn')
INSERT [dbo].[CMS_CASE_REQUEST_STATUS_G2G_LKP] ([Id], [Name_En], [Name_Ar]) VALUES (8, N'Need More Information', N'Need More Information')
INSERT [dbo].[CMS_CASE_REQUEST_STATUS_G2G_LKP] ([Id], [Name_En], [Name_Ar]) VALUES (9, N'Request to Withdraw', N'Request to Withdraw')
SET IDENTITY_INSERT [dbo].[CMS_CASE_REQUEST_STATUS_G2G_LKP] OFF


INSERT [dbo].[CMS_CASE_REQUEST] ([RequestId], [RequestNumber], [RequestDate], [ClaimAmount], [ReferenceNo], [ReferenceDate], [Remarks], [CaseRequirements], [Subject], [IsConfidential], [GovtEntityId], [DepartmentId], [SectorTypeId], [SubTypeId], [PriorityId], [StatusId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'04fa66d1-80ad-4287-89ed-04c4f84be848', 8, CAST(N'2022-10-11T17:48:38.020' AS DateTime), CAST(0.00 AS Decimal(10, 2)), NULL, NULL, NULL, NULL, NULL, 0, 2, NULL, 1, NULL, NULL, 2, N'fatwaadmin@gmail.com', CAST(N'2022-10-11T17:49:54.350' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[CMS_CASE_REQUEST] ([RequestId], [RequestNumber], [RequestDate], [ClaimAmount], [ReferenceNo], [ReferenceDate], [Remarks], [CaseRequirements], [Subject], [IsConfidential], [GovtEntityId], [DepartmentId], [SectorTypeId], [SubTypeId], [PriorityId], [StatusId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'8fc84a69-c607-41fb-a6b9-189ed913dc42', 7, CAST(N'2022-10-11T17:42:14.437' AS DateTime), CAST(0.00 AS Decimal(10, 2)), NULL, NULL, NULL, NULL, NULL, 0, 3, NULL, 1, NULL, 2, 9, N'fatwaadmin@gmail.com', CAST(N'2022-10-11T17:47:03.237' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[CMS_CASE_REQUEST] ([RequestId], [RequestNumber], [RequestDate], [ClaimAmount], [ReferenceNo], [ReferenceDate], [Remarks], [CaseRequirements], [Subject], [IsConfidential], [GovtEntityId], [DepartmentId], [SectorTypeId], [SubTypeId], [PriorityId], [StatusId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'192745d2-0fa1-4da2-aa6c-20b8ee2c2200', 17, CAST(N'2022-10-14T18:19:06.497' AS DateTime), CAST(0.00 AS Decimal(10, 2)), NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, 1, NULL, NULL, 2, N'fatwaadmin@gmail.com', CAST(N'2022-10-14T18:20:23.650' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[CMS_CASE_REQUEST] ([RequestId], [RequestNumber], [RequestDate], [ClaimAmount], [ReferenceNo], [ReferenceDate], [Remarks], [CaseRequirements], [Subject], [IsConfidential], [GovtEntityId], [DepartmentId], [SectorTypeId], [SubTypeId], [PriorityId], [StatusId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'227362ac-ae19-4073-8ca0-25e4f14246a6', 5, CAST(N'2022-10-10T19:56:11.140' AS DateTime), CAST(0.00 AS Decimal(10, 2)), NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, 1, NULL, NULL, 2, N'fatwaadmin@gmail.com', CAST(N'2022-10-10T19:56:33.193' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[CMS_CASE_REQUEST] ([RequestId], [RequestNumber], [RequestDate], [ClaimAmount], [ReferenceNo], [ReferenceDate], [Remarks], [CaseRequirements], [Subject], [IsConfidential], [GovtEntityId], [DepartmentId], [SectorTypeId], [SubTypeId], [PriorityId], [StatusId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'093940b4-223f-49f0-b56b-3b7d890badf5', 18, CAST(N'2022-10-17T10:37:57.033' AS DateTime), CAST(0.00 AS Decimal(10, 2)), NULL, NULL, NULL, NULL, N'Testing Doc', 0, NULL, NULL, 1, NULL, 2, 9, N'fatwaadmin@gmail.com', CAST(N'2022-10-17T10:40:33.690' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[CMS_CASE_REQUEST] ([RequestId], [RequestNumber], [RequestDate], [ClaimAmount], [ReferenceNo], [ReferenceDate], [Remarks], [CaseRequirements], [Subject], [IsConfidential], [GovtEntityId], [DepartmentId], [SectorTypeId], [SubTypeId], [PriorityId], [StatusId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'2b28f07e-54c8-4280-8aa0-4926af745468', 26, CAST(N'2022-10-19T16:18:17.743' AS DateTime), CAST(0.00 AS Decimal(10, 2)), NULL, NULL, NULL, NULL, N'gygh', 0, NULL, NULL, 2, 8, 2, 7, N'fatwaadmin@gmail.com', CAST(N'2022-10-19T16:20:34.840' AS DateTime), N'fatwaadmin@gmail.com', CAST(N'2022-10-19T16:21:06.937' AS DateTime), NULL, NULL, 0)
INSERT [dbo].[CMS_CASE_REQUEST] ([RequestId], [RequestNumber], [RequestDate], [ClaimAmount], [ReferenceNo], [ReferenceDate], [Remarks], [CaseRequirements], [Subject], [IsConfidential], [GovtEntityId], [DepartmentId], [SectorTypeId], [SubTypeId], [PriorityId], [StatusId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'12401df0-0910-4e19-97de-4e2f786ad67f', 6, CAST(N'2022-10-11T16:07:50.780' AS DateTime), CAST(3.00 AS Decimal(10, 2)), NULL, NULL, NULL, NULL, N'sdfsdf', 0, 2, NULL, 1, NULL, 2, 2, N'fatwaadmin@gmail.com', CAST(N'2022-10-11T16:16:25.087' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[CMS_CASE_REQUEST] ([RequestId], [RequestNumber], [RequestDate], [ClaimAmount], [ReferenceNo], [ReferenceDate], [Remarks], [CaseRequirements], [Subject], [IsConfidential], [GovtEntityId], [DepartmentId], [SectorTypeId], [SubTypeId], [PriorityId], [StatusId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'5fb743bb-a593-4227-b593-58fe1c578427', 9, CAST(N'2022-10-11T17:50:22.560' AS DateTime), CAST(0.00 AS Decimal(10, 2)), NULL, NULL, NULL, NULL, NULL, 0, 1, NULL, 1, NULL, NULL, 2, N'fatwaadmin@gmail.com', CAST(N'2022-10-11T17:51:55.373' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[CMS_CASE_REQUEST] ([RequestId], [RequestNumber], [RequestDate], [ClaimAmount], [ReferenceNo], [ReferenceDate], [Remarks], [CaseRequirements], [Subject], [IsConfidential], [GovtEntityId], [DepartmentId], [SectorTypeId], [SubTypeId], [PriorityId], [StatusId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'fe539b29-b161-40b5-8425-5b6930519a5a', 25, CAST(N'2022-10-19T15:47:43.340' AS DateTime), CAST(0.00 AS Decimal(10, 2)), NULL, NULL, NULL, NULL, N'wew', 0, NULL, NULL, 1, NULL, 2, 7, N'fatwaadmin@gmail.com', CAST(N'2022-10-19T15:49:14.927' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[CMS_CASE_REQUEST] ([RequestId], [RequestNumber], [RequestDate], [ClaimAmount], [ReferenceNo], [ReferenceDate], [Remarks], [CaseRequirements], [Subject], [IsConfidential], [GovtEntityId], [DepartmentId], [SectorTypeId], [SubTypeId], [PriorityId], [StatusId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'f60a1761-74c2-4eab-ac56-82d95da6c0c4', 2, CAST(N'2022-08-30T20:12:46.453' AS DateTime), CAST(200.00 AS Decimal(10, 2)), N'3434', CAST(N'2022-08-30' AS Date), N'This remark is added to test the length of the remarks coulmn in the view detail page of case request hope it will not get out of the table area', N'This requirement is added to test the length of the CaseRequirement table coulmn in the view detail page of case request hope it will not get out of the table area', N'Case Two', 0, 4, 3, 1, 3, 2, 2, N'', CAST(N'2022-08-30T20:12:46.453' AS DateTime), N'fatwaadmin@gmail.com', CAST(N'2022-08-30T20:12:46.453' AS DateTime), N'fatwaadmin@gmail.com', CAST(N'2022-08-30T20:12:46.453' AS DateTime), 0)
INSERT [dbo].[CMS_CASE_REQUEST] ([RequestId], [RequestNumber], [RequestDate], [ClaimAmount], [ReferenceNo], [ReferenceDate], [Remarks], [CaseRequirements], [Subject], [IsConfidential], [GovtEntityId], [DepartmentId], [SectorTypeId], [SubTypeId], [PriorityId], [StatusId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'1c485957-7066-4346-8670-83198107ba5f', 20, CAST(N'2022-10-17T11:14:39.553' AS DateTime), CAST(0.00 AS Decimal(10, 2)), NULL, NULL, NULL, NULL, N'asfsdfds', 0, NULL, NULL, 1, NULL, 2, 9, N'fatwaadmin@gmail.com', CAST(N'2022-10-17T11:15:41.250' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[CMS_CASE_REQUEST] ([RequestId], [RequestNumber], [RequestDate], [ClaimAmount], [ReferenceNo], [ReferenceDate], [Remarks], [CaseRequirements], [Subject], [IsConfidential], [GovtEntityId], [DepartmentId], [SectorTypeId], [SubTypeId], [PriorityId], [StatusId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'c45b770c-c56a-45c4-b1fa-900d4908ff7d', 11, CAST(N'2022-10-11T18:15:06.917' AS DateTime), CAST(0.00 AS Decimal(10, 2)), NULL, NULL, N'asd', N'asdsd', N'sdf', 0, 2, NULL, 1, NULL, 2, 2, N'fatwaadmin@gmail.com', CAST(N'2022-10-11T18:15:29.827' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[CMS_CASE_REQUEST] ([RequestId], [RequestNumber], [RequestDate], [ClaimAmount], [ReferenceNo], [ReferenceDate], [Remarks], [CaseRequirements], [Subject], [IsConfidential], [GovtEntityId], [DepartmentId], [SectorTypeId], [SubTypeId], [PriorityId], [StatusId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'7739107f-5346-437b-9d3a-921a241bd344', 24, CAST(N'2022-10-19T15:46:39.747' AS DateTime), CAST(0.00 AS Decimal(10, 2)), NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, 1, NULL, NULL, 2, N'fatwaadmin@gmail.com', CAST(N'2022-10-19T15:47:37.340' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[CMS_CASE_REQUEST] ([RequestId], [RequestNumber], [RequestDate], [ClaimAmount], [ReferenceNo], [ReferenceDate], [Remarks], [CaseRequirements], [Subject], [IsConfidential], [GovtEntityId], [DepartmentId], [SectorTypeId], [SubTypeId], [PriorityId], [StatusId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'16609de6-ba55-4df2-8ec5-928d32de5703', 12, CAST(N'2022-10-11T18:17:20.870' AS DateTime), CAST(20.00 AS Decimal(10, 2)), NULL, NULL, N'test', N'test', N'test', 0, 2, NULL, 1, NULL, 2, 2, N'fatwaadmin@gmail.com', CAST(N'2022-10-11T18:17:43.603' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[CMS_CASE_REQUEST] ([RequestId], [RequestNumber], [RequestDate], [ClaimAmount], [ReferenceNo], [ReferenceDate], [Remarks], [CaseRequirements], [Subject], [IsConfidential], [GovtEntityId], [DepartmentId], [SectorTypeId], [SubTypeId], [PriorityId], [StatusId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'494b4efc-9f9f-4fd0-972e-a23ed8aee7aa', 22, CAST(N'2022-10-19T13:19:07.957' AS DateTime), CAST(0.00 AS Decimal(10, 2)), NULL, NULL, NULL, NULL, N'doc5', 0, NULL, NULL, 1, NULL, 3, 9, N'fatwaadmin@gmail.com', CAST(N'2022-10-19T13:20:48.633' AS DateTime), N'fatwaadmin@gmail.com', CAST(N'2022-10-19T13:22:22.873' AS DateTime), NULL, NULL, 0)
INSERT [dbo].[CMS_CASE_REQUEST] ([RequestId], [RequestNumber], [RequestDate], [ClaimAmount], [ReferenceNo], [ReferenceDate], [Remarks], [CaseRequirements], [Subject], [IsConfidential], [GovtEntityId], [DepartmentId], [SectorTypeId], [SubTypeId], [PriorityId], [StatusId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'f0a352f2-11bf-4308-8715-ad9d8fb2c75b', 15, CAST(N'2022-10-14T14:08:18.320' AS DateTime), CAST(0.00 AS Decimal(10, 2)), NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, 1, NULL, NULL, 2, N'fatwaadmin@gmail.com', CAST(N'2022-10-14T14:10:58.907' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[CMS_CASE_REQUEST] ([RequestId], [RequestNumber], [RequestDate], [ClaimAmount], [ReferenceNo], [ReferenceDate], [Remarks], [CaseRequirements], [Subject], [IsConfidential], [GovtEntityId], [DepartmentId], [SectorTypeId], [SubTypeId], [PriorityId], [StatusId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'535bead8-4174-4b40-9371-b2fe11531ce7', 3, CAST(N'2022-10-10T19:48:45.273' AS DateTime), CAST(0.00 AS Decimal(10, 2)), NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, 1, NULL, NULL, 2, N'fatwaadmin@gmail.com', CAST(N'2022-10-10T19:49:32.097' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[CMS_CASE_REQUEST] ([RequestId], [RequestNumber], [RequestDate], [ClaimAmount], [ReferenceNo], [ReferenceDate], [Remarks], [CaseRequirements], [Subject], [IsConfidential], [GovtEntityId], [DepartmentId], [SectorTypeId], [SubTypeId], [PriorityId], [StatusId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'be231865-e0bc-4f25-83c8-bd5e449f3019', 16, CAST(N'2022-10-14T17:43:10.880' AS DateTime), CAST(0.00 AS Decimal(10, 2)), NULL, NULL, NULL, NULL, N'wwe', 0, NULL, NULL, 1, NULL, 1, 2, N'fatwaadmin@gmail.com', CAST(N'2022-10-14T17:47:34.257' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[CMS_CASE_REQUEST] ([RequestId], [RequestNumber], [RequestDate], [ClaimAmount], [ReferenceNo], [ReferenceDate], [Remarks], [CaseRequirements], [Subject], [IsConfidential], [GovtEntityId], [DepartmentId], [SectorTypeId], [SubTypeId], [PriorityId], [StatusId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'be351c0f-036d-4446-b404-c5fc8cfb7711', 10, CAST(N'2022-10-11T18:14:12.863' AS DateTime), CAST(0.00 AS Decimal(10, 2)), NULL, NULL, NULL, NULL, NULL, 0, 2, NULL, 1, NULL, NULL, 2, N'fatwaadmin@gmail.com', CAST(N'2022-10-11T18:14:28.677' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[CMS_CASE_REQUEST] ([RequestId], [RequestNumber], [RequestDate], [ClaimAmount], [ReferenceNo], [ReferenceDate], [Remarks], [CaseRequirements], [Subject], [IsConfidential], [GovtEntityId], [DepartmentId], [SectorTypeId], [SubTypeId], [PriorityId], [StatusId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'ca51cba1-982e-43be-b37b-d73f85700ef5', 19, CAST(N'2022-10-17T11:13:58.240' AS DateTime), CAST(0.00 AS Decimal(10, 2)), NULL, NULL, NULL, NULL, N'Testing Doc', 0, 2, NULL, 1, NULL, 1, 2, N'fatwaadmin@gmail.com', CAST(N'2022-10-17T11:14:36.633' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[CMS_CASE_REQUEST] ([RequestId], [RequestNumber], [RequestDate], [ClaimAmount], [ReferenceNo], [ReferenceDate], [Remarks], [CaseRequirements], [Subject], [IsConfidential], [GovtEntityId], [DepartmentId], [SectorTypeId], [SubTypeId], [PriorityId], [StatusId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'9ec8b381-e7af-47f0-91cf-e24c198c21fe', 23, CAST(N'2022-10-19T13:23:03.733' AS DateTime), CAST(0.00 AS Decimal(10, 2)), NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, 1, NULL, NULL, 2, N'fatwaadmin@gmail.com', CAST(N'2022-10-19T13:23:43.330' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[CMS_CASE_REQUEST] ([RequestId], [RequestNumber], [RequestDate], [ClaimAmount], [ReferenceNo], [ReferenceDate], [Remarks], [CaseRequirements], [Subject], [IsConfidential], [GovtEntityId], [DepartmentId], [SectorTypeId], [SubTypeId], [PriorityId], [StatusId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'c39d3bf3-3219-4e1d-af39-ee896d7d1342', 21, CAST(N'2022-10-19T09:23:47.480' AS DateTime), CAST(0.00 AS Decimal(10, 2)), NULL, NULL, N'asdas', N'asfsa', NULL, 0, 2, NULL, 1, NULL, 2, 2, N'fatwaadmin@gmail.com', CAST(N'2022-10-19T09:26:03.953' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[CMS_CASE_REQUEST] ([RequestId], [RequestNumber], [RequestDate], [ClaimAmount], [ReferenceNo], [ReferenceDate], [Remarks], [CaseRequirements], [Subject], [IsConfidential], [GovtEntityId], [DepartmentId], [SectorTypeId], [SubTypeId], [PriorityId], [StatusId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'a3ecc9a6-026b-4aef-8e49-f11ddb6dcc85', 13, CAST(N'2022-10-11T18:25:04.590' AS DateTime), CAST(20.00 AS Decimal(10, 2)), NULL, NULL, N'test', N'test', N'test', 0, 2, NULL, 1, NULL, 2, 2, N'fatwaadmin@gmail.com', CAST(N'2022-10-11T18:26:49.040' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[CMS_CASE_REQUEST] ([RequestId], [RequestNumber], [RequestDate], [ClaimAmount], [ReferenceNo], [ReferenceDate], [Remarks], [CaseRequirements], [Subject], [IsConfidential], [GovtEntityId], [DepartmentId], [SectorTypeId], [SubTypeId], [PriorityId], [StatusId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'68640382-9e90-49e2-b6a1-f24915c5c986', 14, CAST(N'2022-10-11T19:00:30.150' AS DateTime), CAST(20.00 AS Decimal(10, 2)), NULL, NULL, N'testing demo', N'testing demo', N'testing demo', 0, 3, NULL, 1, NULL, 2, 2, N'fatwaadmin@gmail.com', CAST(N'2022-10-11T19:07:37.013' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[CMS_CASE_REQUEST] ([RequestId], [RequestNumber], [RequestDate], [ClaimAmount], [ReferenceNo], [ReferenceDate], [Remarks], [CaseRequirements], [Subject], [IsConfidential], [GovtEntityId], [DepartmentId], [SectorTypeId], [SubTypeId], [PriorityId], [StatusId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'e2a5dcd3-5269-4c1d-a6ec-f97152bb7317', 4, CAST(N'2022-10-10T19:51:15.350' AS DateTime), CAST(0.00 AS Decimal(10, 2)), NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, 1, NULL, NULL, 2, N'fatwaadmin@gmail.com', CAST(N'2022-10-10T19:51:37.993' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[CMS_CASE_REQUEST] ([RequestId], [RequestNumber], [RequestDate], [ClaimAmount], [ReferenceNo], [ReferenceDate], [Remarks], [CaseRequirements], [Subject], [IsConfidential], [GovtEntityId], [DepartmentId], [SectorTypeId], [SubTypeId], [PriorityId], [StatusId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'b93f2375-ae22-4b7a-ac86-fda4418d7e78', 1, CAST(N'2022-08-30T20:12:46.453' AS DateTime), CAST(1000.00 AS Decimal(10, 2)), N'341234', CAST(N'2022-08-30' AS Date), NULL, NULL, N'First Case', 0, 4, 3, 1, 3, 2, 2, N'', CAST(N'2022-08-30T20:12:46.453' AS DateTime), N'fatwaadmin@gmail.com', CAST(N'2022-08-30T20:12:46.453' AS DateTime), N'fatwaadmin@gmail.com', CAST(N'2022-08-30T20:12:46.453' AS DateTime), 0)
GO

SET IDENTITY_INSERT [dbo].[CMS_CASE_FILE_STATUS_G2G_LKP] ON 

INSERT [dbo].[CMS_CASE_FILE_STATUS_G2G_LKP] ([Id], [Name_En], [Name_Ar]) VALUES (1, N'New', N'New')
SET IDENTITY_INSERT [dbo].[CMS_CASE_FILE_STATUS_G2G_LKP] OFF
GO

GO
INSERT [dbo].[CMS_CASE_FILE] ([FileId], [RequestId], [FileNumber], [StatusId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'6746a505-bf51-436a-8188-1d3fb7f0b885', N'04fa66d1-80ad-4287-89ed-04c4f84be848', N'1', 1, N'fatwaadmin@gmail.com', CAST(N'2022-10-11T17:49:54.350' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[CMS_CASE_FILE] ([FileId], [RequestId], [FileNumber], [StatusId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'c4fdbaee-fa30-42c9-9f80-3d2e2322c525', N'192745d2-0fa1-4da2-aa6c-20b8ee2c2200', N'2', 1, N'fatwaadmin@gmail.com', CAST(N'2022-10-11T17:49:54.350' AS DateTime), NULL, NULL, NULL, NULL, 0)
GO

SET IDENTITY_INSERT [dbo].[CMS_CASE_PARTY_CATEGORY_G2G_LKP] ON 

INSERT [dbo].[CMS_CASE_PARTY_CATEGORY_G2G_LKP] ([Id], [Name_En], [Name_Ar], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (1, N'Defendant', N'Defendant', 1, N'superadmin@gmail.com', CAST(N'2022-08-30T20:12:46.453' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[CMS_CASE_PARTY_CATEGORY_G2G_LKP] ([Id], [Name_En], [Name_Ar], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (2, N'Plaintiff', N'Plaintiff', 1, N'superadmin@gmail.com', CAST(N'2022-08-30T20:12:46.453' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[CMS_CASE_PARTY_CATEGORY_G2G_LKP] ([Id], [Name_En], [Name_Ar], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (3, N'ThirdParty', N'ThirdParty', 1, N'superadmin@gmail.com', CAST(N'2022-08-30T20:12:46.453' AS DateTime), NULL, NULL, NULL, NULL, 0)
SET IDENTITY_INSERT [dbo].[CMS_CASE_PARTY_CATEGORY_G2G_LKP] OFF
GO

SET IDENTITY_INSERT [dbo].[CMS_CASE_PARTY_TYPE_G2G_LKP] ON 

INSERT [dbo].[CMS_CASE_PARTY_TYPE_G2G_LKP] ([Id], [Name_En], [Name_Ar], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (1, N'Individual', N'Individual', 1, N'superadmin@gmail.com', CAST(N'2022-08-30T20:12:46.453' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[CMS_CASE_PARTY_TYPE_G2G_LKP] ([Id], [Name_En], [Name_Ar], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (2, N'Company', N'Company', 1, N'superadmin@gmail.com', CAST(N'2022-08-30T20:12:46.453' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[CMS_CASE_PARTY_TYPE_G2G_LKP] ([Id], [Name_En], [Name_Ar], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (3, N'Ministry', N'Ministry', 1, N'superadmin@gmail.com', CAST(N'2022-08-30T20:12:46.453' AS DateTime), NULL, NULL, NULL, NULL, 0)
SET IDENTITY_INSERT [dbo].[CMS_CASE_PARTY_TYPE_G2G_LKP] OFF
GO


INSERT [dbo].[CMS_CASE_PARTY_LINK] ([Id], [RequestId], [CategoryId], [TypeId], [Name], [CivilId], [CRN], [MinistryId], [Representative], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'88116221-265b-48fa-f1d5-08daaacfb123', N'227362ac-ae19-4073-8ca0-25e4f14246a6', 1, 1, N'sdfsd', N'333333333333', NULL, NULL, NULL, N'fatwaadmin@gmail.com', CAST(N'2022-10-10T19:56:51.013' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[CMS_CASE_PARTY_LINK] ([Id], [RequestId], [CategoryId], [TypeId], [Name], [CivilId], [CRN], [MinistryId], [Representative], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'f67fe88c-1ae2-47b9-c7f3-08daab7a1a68', N'12401df0-0910-4e19-97de-4e2f786ad67f', 1, 1, N'test', N'333333333333', NULL, NULL, NULL, N'fatwaadmin@gmail.com', CAST(N'2022-10-11T16:16:52.303' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[CMS_CASE_PARTY_LINK] ([Id], [RequestId], [CategoryId], [TypeId], [Name], [CivilId], [CRN], [MinistryId], [Representative], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'fa51a0ff-def2-40cf-c7f4-08daab7a1a68', N'12401df0-0910-4e19-97de-4e2f786ad67f', 1, 2, N'er', NULL, N'22323232', NULL, NULL, N'fatwaadmin@gmail.com', CAST(N'2022-10-11T16:17:03.183' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[CMS_CASE_PARTY_LINK] ([Id], [RequestId], [CategoryId], [TypeId], [Name], [CivilId], [CRN], [MinistryId], [Representative], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'46fadb55-e87c-4f19-c7f5-08daab7a1a68', N'12401df0-0910-4e19-97de-4e2f786ad67f', 1, 3, NULL, NULL, NULL, 2, N'werwe', N'fatwaadmin@gmail.com', CAST(N'2022-10-11T16:17:03.190' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[CMS_CASE_PARTY_LINK] ([Id], [RequestId], [CategoryId], [TypeId], [Name], [CivilId], [CRN], [MinistryId], [Representative], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'ed44858e-10b1-4b56-ccc2-08daab86b298', N'8fc84a69-c607-41fb-a6b9-189ed913dc42', 1, 2, N'asd', NULL, N'22222222222222222', NULL, NULL, N'fatwaadmin@gmail.com', CAST(N'2022-10-11T17:47:03.667' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[CMS_CASE_PARTY_LINK] ([Id], [RequestId], [CategoryId], [TypeId], [Name], [CivilId], [CRN], [MinistryId], [Representative], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'4b3037cd-2068-442e-ccc3-08daab86b298', N'8fc84a69-c607-41fb-a6b9-189ed913dc42', 1, 3, N'afw', NULL, N'3333333', 1, N'23233', N'fatwaadmin@gmail.com', CAST(N'2022-10-11T17:47:03.743' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[CMS_CASE_PARTY_LINK] ([Id], [RequestId], [CategoryId], [TypeId], [Name], [CivilId], [CRN], [MinistryId], [Representative], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'bf4c48b3-3696-4074-ccc4-08daab86b298', N'04fa66d1-80ad-4287-89ed-04c4f84be848', 1, 3, NULL, NULL, NULL, 1, N'asdas', N'fatwaadmin@gmail.com', CAST(N'2022-10-11T17:49:54.437' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[CMS_CASE_PARTY_LINK] ([Id], [RequestId], [CategoryId], [TypeId], [Name], [CivilId], [CRN], [MinistryId], [Representative], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'95b8c4a8-5de7-4a0c-ccc5-08daab86b298', N'04fa66d1-80ad-4287-89ed-04c4f84be848', 1, 1, N'asd', N'333333333333', NULL, NULL, NULL, N'fatwaadmin@gmail.com', CAST(N'2022-10-11T17:49:54.440' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[CMS_CASE_PARTY_LINK] ([Id], [RequestId], [CategoryId], [TypeId], [Name], [CivilId], [CRN], [MinistryId], [Representative], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'a294edb3-027c-4650-ccc6-08daab86b298', N'04fa66d1-80ad-4287-89ed-04c4f84be848', 1, 2, N'wdaf', NULL, N'333333', NULL, NULL, N'fatwaadmin@gmail.com', CAST(N'2022-10-11T17:49:54.443' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[CMS_CASE_PARTY_LINK] ([Id], [RequestId], [CategoryId], [TypeId], [Name], [CivilId], [CRN], [MinistryId], [Representative], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'9ba850fc-a040-4f1a-ccc7-08daab86b298', N'5fb743bb-a593-4227-b593-58fe1c578427', 1, 2, N'asv', NULL, N'333333', NULL, NULL, N'fatwaadmin@gmail.com', CAST(N'2022-10-11T17:52:01.923' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[CMS_CASE_PARTY_LINK] ([Id], [RequestId], [CategoryId], [TypeId], [Name], [CivilId], [CRN], [MinistryId], [Representative], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'a79973b4-0edd-40aa-ccc8-08daab86b298', N'5fb743bb-a593-4227-b593-58fe1c578427', 1, 3, NULL, NULL, NULL, 2, N'sdvd', N'fatwaadmin@gmail.com', CAST(N'2022-10-11T17:52:01.987' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[CMS_CASE_PARTY_LINK] ([Id], [RequestId], [CategoryId], [TypeId], [Name], [CivilId], [CRN], [MinistryId], [Representative], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'a23d6f0d-957e-4443-fe0c-08daab8c40a6', N'a3ecc9a6-026b-4aef-8e49-f11ddb6dcc85', 1, 1, N'Hassan', N'239482309423', NULL, NULL, NULL, N'fatwaadmin@gmail.com', CAST(N'2022-10-11T18:26:49.477' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[CMS_CASE_PARTY_LINK] ([Id], [RequestId], [CategoryId], [TypeId], [Name], [CivilId], [CRN], [MinistryId], [Representative], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'e4c4360b-46ef-4cb7-fe0d-08daab8c40a6', N'a3ecc9a6-026b-4aef-8e49-f11ddb6dcc85', 1, 2, N'DPS', NULL, N'234233', NULL, NULL, N'fatwaadmin@gmail.com', CAST(N'2022-10-11T18:26:49.567' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[CMS_CASE_PARTY_LINK] ([Id], [RequestId], [CategoryId], [TypeId], [Name], [CivilId], [CRN], [MinistryId], [Representative], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'8560df30-105c-4931-fe0e-08daab8c40a6', N'a3ecc9a6-026b-4aef-8e49-f11ddb6dcc85', 1, 3, NULL, NULL, NULL, 2, N'Hassan', N'fatwaadmin@gmail.com', CAST(N'2022-10-11T18:26:49.570' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[CMS_CASE_PARTY_LINK] ([Id], [RequestId], [CategoryId], [TypeId], [Name], [CivilId], [CRN], [MinistryId], [Representative], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'44f0493f-70ae-41d7-fe0f-08daab8c40a6', N'68640382-9e90-49e2-b6a1-f24915c5c986', 1, 1, N'Hassan', N'232342342343', NULL, NULL, NULL, N'fatwaadmin@gmail.com', CAST(N'2022-10-11T19:07:37.173' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[CMS_CASE_PARTY_LINK] ([Id], [RequestId], [CategoryId], [TypeId], [Name], [CivilId], [CRN], [MinistryId], [Representative], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'47195b22-eb18-4dd0-fe10-08daab8c40a6', N'68640382-9e90-49e2-b6a1-f24915c5c986', 1, 2, N'DPS', NULL, N'23423434342342343434', NULL, NULL, N'fatwaadmin@gmail.com', CAST(N'2022-10-11T19:07:37.177' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[CMS_CASE_PARTY_LINK] ([Id], [RequestId], [CategoryId], [TypeId], [Name], [CivilId], [CRN], [MinistryId], [Representative], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'960dd78f-6006-4c6f-fe11-08daab8c40a6', N'68640382-9e90-49e2-b6a1-f24915c5c986', 1, 3, NULL, NULL, NULL, 2, N'Hassan', N'fatwaadmin@gmail.com', CAST(N'2022-10-11T19:07:37.183' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[CMS_CASE_PARTY_LINK] ([Id], [RequestId], [CategoryId], [TypeId], [Name], [CivilId], [CRN], [MinistryId], [Representative], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'bd77d650-a272-4fb0-1860-08daadc40299', N'f0a352f2-11bf-4308-8715-ad9d8fb2c75b', 1, 2, N'DPS', NULL, N'3423553', NULL, NULL, N'fatwaadmin@gmail.com', CAST(N'2022-10-14T14:10:59.510' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[CMS_CASE_PARTY_LINK] ([Id], [RequestId], [CategoryId], [TypeId], [Name], [CivilId], [CRN], [MinistryId], [Representative], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'25cd7e0e-09a4-4b29-1f2e-08daade6da45', N'192745d2-0fa1-4da2-aa6c-20b8ee2c2200', 1, 2, N'sds', NULL, N'24233334223343343423', NULL, NULL, N'fatwaadmin@gmail.com', CAST(N'2022-10-14T18:20:24.237' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[CMS_CASE_PARTY_LINK] ([Id], [RequestId], [CategoryId], [TypeId], [Name], [CivilId], [CRN], [MinistryId], [Representative], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'479d3fe2-98a2-46ec-5d79-08dab0021cac', N'093940b4-223f-49f0-b56b-3b7d890badf5', 1, 1, N'hassan', N'342423434333', NULL, NULL, NULL, N'fatwaadmin@gmail.com', CAST(N'2022-10-17T10:40:34.380' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[CMS_CASE_PARTY_LINK] ([Id], [RequestId], [CategoryId], [TypeId], [Name], [CivilId], [CRN], [MinistryId], [Representative], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'c8ad2659-b412-4a61-5d7a-08dab0021cac', N'093940b4-223f-49f0-b56b-3b7d890badf5', 1, 2, N'BA', NULL, N'234233', NULL, NULL, N'fatwaadmin@gmail.com', CAST(N'2022-10-17T10:40:34.507' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[CMS_CASE_PARTY_LINK] ([Id], [RequestId], [CategoryId], [TypeId], [Name], [CivilId], [CRN], [MinistryId], [Representative], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'4de4d843-7466-4443-5d7b-08dab0021cac', N'093940b4-223f-49f0-b56b-3b7d890badf5', 1, 3, NULL, NULL, NULL, 2, N'Hassan', N'fatwaadmin@gmail.com', CAST(N'2022-10-17T10:40:34.510' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[CMS_CASE_PARTY_LINK] ([Id], [RequestId], [CategoryId], [TypeId], [Name], [CivilId], [CRN], [MinistryId], [Representative], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'17e41f6d-bb57-4513-8f25-08dab006de1f', N'ca51cba1-982e-43be-b37b-d73f85700ef5', 1, 2, N'Hassan', NULL, N'2342323', NULL, NULL, N'fatwaadmin@gmail.com', CAST(N'2022-10-17T11:14:36.923' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[CMS_CASE_PARTY_LINK] ([Id], [RequestId], [CategoryId], [TypeId], [Name], [CivilId], [CRN], [MinistryId], [Representative], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'effee700-3bce-44aa-8f26-08dab006de1f', N'1c485957-7066-4346-8670-83198107ba5f', 1, 2, N'ew', NULL, N'234223', NULL, NULL, N'fatwaadmin@gmail.com', CAST(N'2022-10-17T11:15:41.357' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[CMS_CASE_PARTY_LINK] ([Id], [RequestId], [CategoryId], [TypeId], [Name], [CivilId], [CRN], [MinistryId], [Representative], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'462a07db-24e6-4a45-08d6-08dab18a092d', N'c39d3bf3-3219-4e1d-af39-ee896d7d1342', 1, 2, N'asd', NULL, N'32334', NULL, NULL, N'fatwaadmin@gmail.com', CAST(N'2022-10-19T09:26:04.387' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[CMS_CASE_PARTY_LINK] ([Id], [RequestId], [CategoryId], [TypeId], [Name], [CivilId], [CRN], [MinistryId], [Representative], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'e4117039-662b-4fec-da62-08dab1aad441', N'494b4efc-9f9f-4fd0-972e-a23ed8aee7aa', 1, 1, N'dfh', N'464564564565', NULL, NULL, NULL, N'fatwaadmin@gmail.com', CAST(N'2022-10-19T13:22:25.460' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[CMS_CASE_PARTY_LINK] ([Id], [RequestId], [CategoryId], [TypeId], [Name], [CivilId], [CRN], [MinistryId], [Representative], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'33f1f318-ba05-43b7-5ac9-08dab1bf90bb', N'fe539b29-b161-40b5-8425-5b6930519a5a', 1, 3, NULL, NULL, NULL, 3, N'23rw', N'fatwaadmin@gmail.com', CAST(N'2022-10-19T15:49:15.140' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[CMS_CASE_PARTY_LINK] ([Id], [RequestId], [CategoryId], [TypeId], [Name], [CivilId], [CRN], [MinistryId], [Representative], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'f1d729d6-dfe7-48f8-f099-08dab1c3f18a', N'2b28f07e-54c8-4280-8aa0-4926af745468', 1, 1, N'fhf', N'574564666564', NULL, NULL, NULL, N'fatwaadmin@gmail.com', CAST(N'2022-10-19T16:21:07.140' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[CMS_CASE_PARTY_LINK] ([Id], [RequestId], [CategoryId], [TypeId], [Name], [CivilId], [CRN], [MinistryId], [Representative], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'59c9c07e-e4f3-42d6-9242-a9dcedc0f056', N'63ff00b9-0581-4849-b6ac-a630486c1707', 1, 1, N'fhf', N'574564666564', NULL, NULL, NULL, N'fatwaadmin@gmail.com', CAST(N'2022-10-19T16:21:07.140' AS DateTime), NULL, NULL, NULL, NULL, 0)
GO


SET IDENTITY_INSERT [dbo].[CMS_CASE_REQUEST_EVENT_G2G_LKP] ON 

INSERT [dbo].[CMS_CASE_REQUEST_EVENT_G2G_LKP] ([Id], [Name_En], [Name_Ar]) VALUES (1, N'Created', N'Created')
INSERT [dbo].[CMS_CASE_REQUEST_EVENT_G2G_LKP] ([Id], [Name_En], [Name_Ar]) VALUES (2, N'Edited', N'Edited')
INSERT [dbo].[CMS_CASE_REQUEST_EVENT_G2G_LKP] ([Id], [Name_En], [Name_Ar]) VALUES (3, N'Withdrawn', N'Withdrawn')
SET IDENTITY_INSERT [dbo].[CMS_CASE_REQUEST_EVENT_G2G_LKP] OFF
GO

GO
INSERT [dbo].[CMS_CASE_REQUEST_STATUS_HISTORY] ([HistoryId], [EventId], [Remarks], [StatusId], [RequestId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'c36de099-6af4-45ea-8fbf-01903155a9c8', 3, NULL, 9, N'8fc84a69-c607-41fb-a6b9-189ed913dc42', N'fatwaadmin@gmail.com', CAST(N'2022-10-11T17:47:03.237' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[CMS_CASE_REQUEST_STATUS_HISTORY] ([HistoryId], [EventId], [Remarks], [StatusId], [RequestId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'3905b11d-7990-49bf-a5ef-aae147d0423f', 3, NULL, 9, N'8fc84a69-c607-41fb-a6b9-189ed913dc42', N'fatwaadmin@gmail.com', CAST(N'2022-10-11T17:47:03.237' AS DateTime), NULL, NULL, NULL, NULL, 0)
GO













SET IDENTITY_INSERT [dbo].[CMS_REGISTERED_CASE_STATUS_G2G_LKP] ON 

INSERT [dbo].[CMS_REGISTERED_CASE_STATUS_G2G_LKP] ([Id], [Name_En], [Name_Ar]) VALUES (1, N'New', N'New')
SET IDENTITY_INSERT [dbo].[CMS_REGISTERED_CASE_STATUS_G2G_LKP] OFF
GO


INSERT [dbo].[CMS_REGISTERED_CASE] ([CaseId], [FileId], [CANNumber], [CaseNumber], [CaseDate], [CourtId], [ChamberId], [StatusId], [IsConfidential], [GovtEntityId], [CaseRequirements], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'63ff00b9-0581-4849-b6ac-a630486c1707', N'6746a505-bf51-436a-8188-1d3fb7f0b885', N'FS23423', N'2022CA234234', CAST(N'2022-10-11' AS Date), 2, 1, 1, 0, 4, NULL, N'fatwaadmin@gmail.com', CAST(N'2022-10-11T17:49:54.350' AS DateTime), NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[CMS_REGISTERED_CASE] ([CaseId], [FileId], [CANNumber], [CaseNumber], [CaseDate], [CourtId], [ChamberId], [StatusId], [IsConfidential], [GovtEntityId], [CaseRequirements], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'45211edd-1c2f-4e5b-907a-db2c90404e47', N'c4fdbaee-fa30-42c9-9f80-3d2e2322c525', N'JKS3902', N'2022CA097238', CAST(N'2022-10-11' AS Date), 2, 2, 1, 0, 8, NULL, N'fatwaadmin@gmail.com', CAST(N'2022-10-11T17:49:54.350' AS DateTime), NULL, NULL, NULL, NULL, 0)
GO
SET IDENTITY_INSERT [dbo].[CMS_REGISTERED_CASE_EVENT_G2G_LKP] ON 

INSERT [dbo].[CMS_REGISTERED_CASE_EVENT_G2G_LKP] ([Id], [Name_En], [Name_Ar]) VALUES (1, N'Created', N'Created')
INSERT [dbo].[CMS_REGISTERED_CASE_EVENT_G2G_LKP] ([Id], [Name_En], [Name_Ar]) VALUES (2, N'Edited', N'Edited')
SET IDENTITY_INSERT [dbo].[CMS_REGISTERED_CASE_EVENT_G2G_LKP] OFF
GO

INSERT [dbo].[CMS_REGISTERED_CASE_STATUS_HISTORY] ([HistoryId], [EventId], [Remarks], [StatusId], [CaseId], [FileId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [DeletedBy], [DeletedDate], [IsDeleted]) VALUES (N'125d4ef4-7b73-4a00-888a-1571c6c05f47', 1, NULL, 1, N'63ff00b9-0581-4849-b6ac-a630486c1707', N'6746a505-bf51-436a-8188-1d3fb7f0b885', N'fatwaadmin@gmail.com', CAST(N'2022-10-11T17:49:54.350' AS DateTime), NULL, NULL, NULL, NULL, 0)
GO


--MODULE
SET IDENTITY_INSERT  MODULE ON
INSERT INTO MODULE (ModuleId,ModuleNameEn,ModuleNameAr) VALUES (6, N'Communication', N'Communication')

SET IDENTITY_INSERT  MODULE OFF


ALTER TABLE UPLOADED_DOCUMENT DROP CONSTRAINT LMS_LITERATURE_UPLOADED_DOCUMENT_ATTACHMENT_TYPE
ALTER TABLE TEMP_ATTACHEMENTS DROP CONSTRAINT TEMP_ATTACHEMENTS_ATTACHMENT_TYPE

DELETE FROM ATTACHMENT_TYPE

SET IDENTITY_INSERT [ATTACHMENT_TYPE] ON
INSERT [dbo].[ATTACHMENT_TYPE] ([AttachmentTypeId], [Type_Ar], [Type_En], [ModuleId], [IsMandatory]) VALUES (1, N'النسخة الرقمية للكتاب', N'Book Digital Copy', 3, 0)
INSERT [dbo].[ATTACHMENT_TYPE] ([AttachmentTypeId], [Type_Ar], [Type_En], [ModuleId], [IsMandatory]) VALUES (2, N'التحكيم الدولي', N'International Arbitration', 1, 0)
INSERT [dbo].[ATTACHMENT_TYPE] ([AttachmentTypeId], [Type_Ar], [Type_En], [ModuleId], [IsMandatory]) VALUES (3, N'ملف فهرس', N'Catalogue File', NULL, 0)
INSERT [dbo].[ATTACHMENT_TYPE] ([AttachmentTypeId], [Type_Ar], [Type_En], [ModuleId], [IsMandatory]) VALUES (4, N'الصوره الشخصيه', N'Profile Picture', 4, 0)
INSERT [dbo].[ATTACHMENT_TYPE] ([AttachmentTypeId], [Type_Ar], [Type_En], [ModuleId], [IsMandatory]) VALUES (5, N'صورة غلاف الكتاب', N'Book Cover Image', 3, 0)
INSERT [dbo].[ATTACHMENT_TYPE] ([AttachmentTypeId], [Type_Ar], [Type_En], [ModuleId], [IsMandatory]) VALUES (6, N'أحكام المحاكم', N'Courts Decisions', 1, 0)
INSERT [dbo].[ATTACHMENT_TYPE] ([AttachmentTypeId], [Type_Ar], [Type_En], [ModuleId], [IsMandatory]) VALUES (7, N'الفتاوى', N'Legal Advice', 1, 0)
INSERT [dbo].[ATTACHMENT_TYPE] ([AttachmentTypeId], [Type_Ar], [Type_En], [ModuleId], [IsMandatory]) VALUES (8, N'التشريع‎', N'Legislation', 1, 0)
INSERT [dbo].[ATTACHMENT_TYPE] ([AttachmentTypeId], [Type_Ar], [Type_En], [ModuleId], [IsMandatory]) VALUES (9, N'Authority Letter', N'Authority Letter', 5, 1)
INSERT [dbo].[ATTACHMENT_TYPE] ([AttachmentTypeId], [Type_Ar], [Type_En], [ModuleId], [IsMandatory]) VALUES (10, N'Defendent Civil ID', N'Defendent Civil ID', 5, 1)
INSERT [dbo].[ATTACHMENT_TYPE] ([AttachmentTypeId], [Type_Ar], [Type_En], [ModuleId], [IsMandatory]) VALUES (11, N'Administrative', N'Administrative', 5, 1)
INSERT [dbo].[ATTACHMENT_TYPE] ([AttachmentTypeId], [Type_Ar], [Type_En], [ModuleId], [IsMandatory]) VALUES (12, N'Other', N'Other', 5, 0)
INSERT [dbo].[ATTACHMENT_TYPE] ([AttachmentTypeId], [Type_Ar], [Type_En], [ModuleId], [IsMandatory]) VALUES (13, N'Communication', N'Communication', 5, 1)
INSERT [dbo].[ATTACHMENT_TYPE] ([AttachmentTypeId], [Type_Ar], [Type_En], [ModuleId], [IsMandatory]) VALUES (25, N'Task', N'Task', 9, 1)
INSERT [dbo].[ATTACHMENT_TYPE] ([AttachmentTypeId], [Type_Ar], [Type_En], [ModuleId], [IsMandatory]) VALUES (26, N'Task Response', N'Task Response', 9, 1)


SET IDENTITY_INSERT [dbo].[ATTACHMENT_TYPE] OFF

ALTER TABLE UPLOADED_DOCUMENT ADD CONSTRAINT UPLOADED_DOCUMENT_ATTACHMENT_TYPE FOREIGN KEY(AttachmentTypeId)
REFERENCES ATTACHMENT_TYPE(AttachmentTypeId)

ALTER TABLE TEMP_ATTACHEMENTS ADD CONSTRAINT TEMP_ATTACHEMENTS_ATTACHMENT_TYPE FOREIGN KEY(AttachmentTypeId)
REFERENCES ATTACHMENT_TYPE(AttachmentTypeId)


INSERT [dbo].[UPLOADED_DOCUMENT] ([LiteratureId], [DocumentDate], [Description], [FileName], [StoragePath], [IsActive], [CreatedBy], [CreatedDateTime], [CreatedAt], [ModifiedBy], [ModifiedDateTime], [ModifiedAt], [DeletedBy], [DeletedDateTime], [IsDeleted], [ReferenceGuid], [DocType], [AttachmentTypeId], [ReferenceNo], [ReferenceDate], [OtherAttachmentType]) VALUES (0, CAST(N'2022-09-25' AS Date), N'null', N'457592948184_202210141408462498.pdf', N'\wwwroot\Attachments\CaseManagement\457592948184_202210141408462498.pdf', 1, N'fatwaadmin@gmail.com', CAST(N'2022-10-14T14:10:59.620' AS DateTime), N'\wwwroot\Attachments\CaseManagement\457592948184_202210141408462498.pdf', NULL, NULL, NULL, NULL, NULL, 0, N'f0a352f2-11bf-4308-8715-ad9d8fb2c75b', N'.pdf', 9, N'23443', CAST(N'2022-09-27' AS Date), N'null')
INSERT [dbo].[UPLOADED_DOCUMENT] ([LiteratureId], [DocumentDate], [Description], [FileName], [StoragePath], [IsActive], [CreatedBy], [CreatedDateTime], [CreatedAt], [ModifiedBy], [ModifiedDateTime], [ModifiedAt], [DeletedBy], [DeletedDateTime], [IsDeleted], [ReferenceGuid], [DocType], [AttachmentTypeId], [ReferenceNo], [ReferenceDate], [OtherAttachmentType]) VALUES (0, CAST(N'2022-09-26' AS Date), N'null', N'643084455922_202210141410276405.pdf', N'\wwwroot\Attachments\User\643084455922_202210141410276405.pdf', 1, N'fatwaadmin@gmail.com', CAST(N'2022-10-14T14:11:00.017' AS DateTime), N'\wwwroot\Attachments\User\643084455922_202210141410276405.pdf', NULL, NULL, NULL, NULL, NULL, 0, N'f0a352f2-11bf-4308-8715-ad9d8fb2c75b', N'.pdf', 10, N'34', CAST(N'2022-10-06' AS Date), N'null')
INSERT [dbo].[UPLOADED_DOCUMENT] ([LiteratureId], [DocumentDate], [Description], [FileName], [StoragePath], [IsActive], [CreatedBy], [CreatedDateTime], [CreatedAt], [ModifiedBy], [ModifiedDateTime], [ModifiedAt], [DeletedBy], [DeletedDateTime], [IsDeleted], [ReferenceGuid], [DocType], [AttachmentTypeId], [ReferenceNo], [ReferenceDate], [OtherAttachmentType]) VALUES (0, CAST(N'2022-09-25' AS Date), N'null', N'643084455922_202210141410550850.pdf', N'\wwwroot\Attachments\User\643084455922_202210141410550850.pdf', 1, N'fatwaadmin@gmail.com', CAST(N'2022-10-14T14:11:00.233' AS DateTime), N'\wwwroot\Attachments\User\643084455922_202210141410550850.pdf', NULL, NULL, NULL, NULL, NULL, 0, N'f0a352f2-11bf-4308-8715-ad9d8fb2c75b', N'.pdf', 11, N'23423', CAST(N'2022-09-26' AS Date), N'null')
INSERT [dbo].[UPLOADED_DOCUMENT] ([LiteratureId], [DocumentDate], [Description], [FileName], [StoragePath], [IsActive], [CreatedBy], [CreatedDateTime], [CreatedAt], [ModifiedBy], [ModifiedDateTime], [ModifiedAt], [DeletedBy], [DeletedDateTime], [IsDeleted], [ReferenceGuid], [DocType], [AttachmentTypeId], [ReferenceNo], [ReferenceDate], [OtherAttachmentType]) VALUES (0, CAST(N'2022-09-26' AS Date), N'null', N'643084455922_202210141743293527.pdf', N'\wwwroot\Attachments\CaseManagement\643084455922_202210141743293527.pdf', 1, N'fatwaadmin@gmail.com', CAST(N'2022-10-14T17:47:34.823' AS DateTime), N'\wwwroot\Attachments\CaseManagement\643084455922_202210141743293527.pdf', NULL, NULL, NULL, NULL, NULL, 0, N'be231865-e0bc-4f25-83c8-bd5e449f3019', N'.pdf', 9, N'23423', CAST(N'2022-09-25' AS Date), N'null')
INSERT [dbo].[UPLOADED_DOCUMENT] ([LiteratureId], [DocumentDate], [Description], [FileName], [StoragePath], [IsActive], [CreatedBy], [CreatedDateTime], [CreatedAt], [ModifiedBy], [ModifiedDateTime], [ModifiedAt], [DeletedBy], [DeletedDateTime], [IsDeleted], [ReferenceGuid], [DocType], [AttachmentTypeId], [ReferenceNo], [ReferenceDate], [OtherAttachmentType]) VALUES (0, CAST(N'2022-09-26' AS Date), N'null', N'643084455922_202210141819205776.pdf', N'\wwwroot\Attachments\CaseManagement\643084455922_202210141819205776.pdf', 1, N'fatwaadmin@gmail.com', CAST(N'2022-10-14T18:20:24.357' AS DateTime), N'\wwwroot\Attachments\CaseManagement\643084455922_202210141819205776.pdf', NULL, NULL, NULL, NULL, NULL, 0, N'192745d2-0fa1-4da2-aa6c-20b8ee2c2200', N'.pdf', 9, N'2343', CAST(N'2022-09-27' AS Date), N'null')
INSERT [dbo].[UPLOADED_DOCUMENT] ([LiteratureId], [DocumentDate], [Description], [FileName], [StoragePath], [IsActive], [CreatedBy], [CreatedDateTime], [CreatedAt], [ModifiedBy], [ModifiedDateTime], [ModifiedAt], [DeletedBy], [DeletedDateTime], [IsDeleted], [ReferenceGuid], [DocType], [AttachmentTypeId], [ReferenceNo], [ReferenceDate], [OtherAttachmentType]) VALUES (0, CAST(N'2022-09-25' AS Date), N'null', N'457592948184_202210141820035167.pdf', N'\wwwroot\Attachments\User\457592948184_202210141820035167.pdf', 1, N'fatwaadmin@gmail.com', CAST(N'2022-10-14T18:20:24.757' AS DateTime), N'\wwwroot\Attachments\User\457592948184_202210141820035167.pdf', NULL, NULL, NULL, NULL, NULL, 0, N'192745d2-0fa1-4da2-aa6c-20b8ee2c2200', N'.pdf', 12, N'234', CAST(N'2022-09-25' AS Date), N'dvsd')
INSERT [dbo].[UPLOADED_DOCUMENT] ([LiteratureId], [DocumentDate], [Description], [FileName], [StoragePath], [IsActive], [CreatedBy], [CreatedDateTime], [CreatedAt], [ModifiedBy], [ModifiedDateTime], [ModifiedAt], [DeletedBy], [DeletedDateTime], [IsDeleted], [ReferenceGuid], [DocType], [AttachmentTypeId], [ReferenceNo], [ReferenceDate], [OtherAttachmentType]) VALUES (0, CAST(N'2022-09-26' AS Date), N'null', N'643084455922_202210171038093483.pdf', N'\wwwroot\Attachments\CaseManagement\643084455922_202210171038093483.pdf', 1, N'fatwaadmin@gmail.com', CAST(N'2022-10-17T10:40:34.533' AS DateTime), N'\wwwroot\Attachments\CaseManagement\643084455922_202210171038093483.pdf', NULL, NULL, NULL, NULL, NULL, 0, N'093940b4-223f-49f0-b56b-3b7d890badf5', N'.pdf', 9, N'23423', CAST(N'2022-10-12' AS Date), N'null')
INSERT [dbo].[UPLOADED_DOCUMENT] ([LiteratureId], [DocumentDate], [Description], [FileName], [StoragePath], [IsActive], [CreatedBy], [CreatedDateTime], [CreatedAt], [ModifiedBy], [ModifiedDateTime], [ModifiedAt], [DeletedBy], [DeletedDateTime], [IsDeleted], [ReferenceGuid], [DocType], [AttachmentTypeId], [ReferenceNo], [ReferenceDate], [OtherAttachmentType]) VALUES (0, CAST(N'2022-10-02' AS Date), N'null', N'643084455922_202210171040179854.pdf', N'\wwwroot\Attachments\User\643084455922_202210171040179854.pdf', 1, N'fatwaadmin@gmail.com', CAST(N'2022-10-17T10:40:34.910' AS DateTime), N'\wwwroot\Attachments\User\643084455922_202210171040179854.pdf', NULL, NULL, NULL, NULL, NULL, 0, N'093940b4-223f-49f0-b56b-3b7d890badf5', N'.pdf', 10, N'24', CAST(N'2022-10-03' AS Date), N'null')
INSERT [dbo].[UPLOADED_DOCUMENT] ([LiteratureId], [DocumentDate], [Description], [FileName], [StoragePath], [IsActive], [CreatedBy], [CreatedDateTime], [CreatedAt], [ModifiedBy], [ModifiedDateTime], [ModifiedAt], [DeletedBy], [DeletedDateTime], [IsDeleted], [ReferenceGuid], [DocType], [AttachmentTypeId], [ReferenceNo], [ReferenceDate], [OtherAttachmentType]) VALUES (0, CAST(N'2022-09-26' AS Date), N'null', N'643084455922_202210171040304262.pdf', N'\wwwroot\Attachments\User\643084455922_202210171040304262.pdf', 1, N'fatwaadmin@gmail.com', CAST(N'2022-10-17T10:40:35.123' AS DateTime), N'\wwwroot\Attachments\User\643084455922_202210171040304262.pdf', NULL, NULL, NULL, NULL, NULL, 0, N'093940b4-223f-49f0-b56b-3b7d890badf5', N'.pdf', 11, N'23423', CAST(N'2022-10-10' AS Date), N'null')
INSERT [dbo].[UPLOADED_DOCUMENT] ([LiteratureId], [DocumentDate], [Description], [FileName], [StoragePath], [IsActive], [CreatedBy], [CreatedDateTime], [CreatedAt], [ModifiedBy], [ModifiedDateTime], [ModifiedAt], [DeletedBy], [DeletedDateTime], [IsDeleted], [ReferenceGuid], [DocType], [AttachmentTypeId], [ReferenceNo], [ReferenceDate], [OtherAttachmentType]) VALUES (0, CAST(N'2022-10-10' AS Date), N'null', N'643084455922_202210171114089649.pdf', N'\wwwroot\Attachments\CaseManagement\643084455922_202210171114089649.pdf', 1, N'fatwaadmin@gmail.com', CAST(N'2022-10-17T11:14:36.983' AS DateTime), N'\wwwroot\Attachments\CaseManagement\643084455922_202210171114089649.pdf', NULL, NULL, NULL, NULL, NULL, 0, N'ca51cba1-982e-43be-b37b-d73f85700ef5', N'.pdf', 9, N'23423', CAST(N'2022-10-04' AS Date), N'null')
INSERT [dbo].[UPLOADED_DOCUMENT] ([LiteratureId], [DocumentDate], [Description], [FileName], [StoragePath], [IsActive], [CreatedBy], [CreatedDateTime], [CreatedAt], [ModifiedBy], [ModifiedDateTime], [ModifiedAt], [DeletedBy], [DeletedDateTime], [IsDeleted], [ReferenceGuid], [DocType], [AttachmentTypeId], [ReferenceNo], [ReferenceDate], [OtherAttachmentType]) VALUES (0, CAST(N'2022-09-26' AS Date), N'null', N'643084455922_202210171114501491.pdf', N'\wwwroot\Attachments\CaseManagement\643084455922_202210171114501491.pdf', 1, N'fatwaadmin@gmail.com', CAST(N'2022-10-17T11:15:41.363' AS DateTime), N'\wwwroot\Attachments\CaseManagement\643084455922_202210171114501491.pdf', NULL, NULL, NULL, NULL, NULL, 0, N'1c485957-7066-4346-8670-83198107ba5f', N'.pdf', 9, N'23', CAST(N'2022-10-04' AS Date), N'null')
INSERT [dbo].[UPLOADED_DOCUMENT] ([LiteratureId], [DocumentDate], [Description], [FileName], [StoragePath], [IsActive], [CreatedBy], [CreatedDateTime], [CreatedAt], [ModifiedBy], [ModifiedDateTime], [ModifiedAt], [DeletedBy], [DeletedDateTime], [IsDeleted], [ReferenceGuid], [DocType], [AttachmentTypeId], [ReferenceNo], [ReferenceDate], [OtherAttachmentType]) VALUES (0, CAST(N'2022-09-26' AS Date), N'null', N'643084455922_202210171115274596.pdf', N'\wwwroot\Attachments\User\643084455922_202210171115274596.pdf', 1, N'fatwaadmin@gmail.com', CAST(N'2022-10-17T11:15:41.567' AS DateTime), N'\wwwroot\Attachments\User\643084455922_202210171115274596.pdf', NULL, NULL, NULL, NULL, NULL, 0, N'1c485957-7066-4346-8670-83198107ba5f', N'.pdf', 10, N'234', CAST(N'2022-10-12' AS Date), N'null')
INSERT [dbo].[UPLOADED_DOCUMENT] ([LiteratureId], [DocumentDate], [Description], [FileName], [StoragePath], [IsActive], [CreatedBy], [CreatedDateTime], [CreatedAt], [ModifiedBy], [ModifiedDateTime], [ModifiedAt], [DeletedBy], [DeletedDateTime], [IsDeleted], [ReferenceGuid], [DocType], [AttachmentTypeId], [ReferenceNo], [ReferenceDate], [OtherAttachmentType]) VALUES (0, CAST(N'2022-10-10' AS Date), N'null', N'643084455922_202210171115382084.pdf', N'\wwwroot\Attachments\User\643084455922_202210171115382084.pdf', 1, N'fatwaadmin@gmail.com', CAST(N'2022-10-17T11:15:41.777' AS DateTime), N'\wwwroot\Attachments\User\643084455922_202210171115382084.pdf', NULL, NULL, NULL, NULL, NULL, 0, N'1c485957-7066-4346-8670-83198107ba5f', N'.pdf', 11, N'23423', CAST(N'2022-10-03' AS Date), N'null')
INSERT [dbo].[UPLOADED_DOCUMENT] ([LiteratureId], [DocumentDate], [Description], [FileName], [StoragePath], [IsActive], [CreatedBy], [CreatedDateTime], [CreatedAt], [ModifiedBy], [ModifiedDateTime], [ModifiedAt], [DeletedBy], [DeletedDateTime], [IsDeleted], [ReferenceGuid], [DocType], [AttachmentTypeId], [ReferenceNo], [ReferenceDate], [OtherAttachmentType]) VALUES (0, CAST(N'2022-09-26' AS Date), N'null', N'643084455922_202210190924067564.pdf', N'\wwwroot\Attachments\CaseManagement\643084455922_202210190924067564.pdf', 1, N'fatwaadmin@gmail.com', CAST(N'2022-10-19T09:26:04.470' AS DateTime), N'\wwwroot\Attachments\CaseManagement\643084455922_202210190924067564.pdf', NULL, NULL, NULL, NULL, NULL, 0, N'c39d3bf3-3219-4e1d-af39-ee896d7d1342', N'.pdf', 9, N'2323', CAST(N'2022-10-03' AS Date), N'null')
INSERT [dbo].[UPLOADED_DOCUMENT] ([LiteratureId], [DocumentDate], [Description], [FileName], [StoragePath], [IsActive], [CreatedBy], [CreatedDateTime], [CreatedAt], [ModifiedBy], [ModifiedDateTime], [ModifiedAt], [DeletedBy], [DeletedDateTime], [IsDeleted], [ReferenceGuid], [DocType], [AttachmentTypeId], [ReferenceNo], [ReferenceDate], [OtherAttachmentType]) VALUES (0, CAST(N'2022-10-03' AS Date), N'null', N'643084455922_202210190925170850.pdf', N'\wwwroot\Attachments\User\643084455922_202210190925170850.pdf', 1, N'fatwaadmin@gmail.com', CAST(N'2022-10-19T09:26:04.763' AS DateTime), N'\wwwroot\Attachments\User\643084455922_202210190925170850.pdf', NULL, NULL, NULL, NULL, NULL, 0, N'c39d3bf3-3219-4e1d-af39-ee896d7d1342', N'.pdf', 10, N'232', CAST(N'2022-10-10' AS Date), N'null')
INSERT [dbo].[UPLOADED_DOCUMENT] ([LiteratureId], [DocumentDate], [Description], [FileName], [StoragePath], [IsActive], [CreatedBy], [CreatedDateTime], [CreatedAt], [ModifiedBy], [ModifiedDateTime], [ModifiedAt], [DeletedBy], [DeletedDateTime], [IsDeleted], [ReferenceGuid], [DocType], [AttachmentTypeId], [ReferenceNo], [ReferenceDate], [OtherAttachmentType]) VALUES (0, CAST(N'2022-09-26' AS Date), N'null', N'457592948184_202210190925313371.pdf', N'\wwwroot\Attachments\User\457592948184_202210190925313371.pdf', 1, N'fatwaadmin@gmail.com', CAST(N'2022-10-19T09:26:04.970' AS DateTime), N'\wwwroot\Attachments\User\457592948184_202210190925313371.pdf', NULL, NULL, NULL, NULL, NULL, 0, N'c39d3bf3-3219-4e1d-af39-ee896d7d1342', N'.pdf', 11, N'42342', CAST(N'2022-09-25' AS Date), N'null')
INSERT [dbo].[UPLOADED_DOCUMENT] ([LiteratureId], [DocumentDate], [Description], [FileName], [StoragePath], [IsActive], [CreatedBy], [CreatedDateTime], [CreatedAt], [ModifiedBy], [ModifiedDateTime], [ModifiedAt], [DeletedBy], [DeletedDateTime], [IsDeleted], [ReferenceGuid], [DocType], [AttachmentTypeId], [ReferenceNo], [ReferenceDate], [OtherAttachmentType]) VALUES (0, CAST(N'2022-09-27' AS Date), N'null', N'457592948184_202210190925595907.pdf', N'\wwwroot\Attachments\User\457592948184_202210190925595907.pdf', 1, N'fatwaadmin@gmail.com', CAST(N'2022-10-19T09:26:05.177' AS DateTime), N'\wwwroot\Attachments\User\457592948184_202210190925595907.pdf', NULL, NULL, NULL, NULL, NULL, 0, N'c39d3bf3-3219-4e1d-af39-ee896d7d1342', N'.pdf', 12, N'34534', CAST(N'2022-09-25' AS Date), N'fhdf')
INSERT [dbo].[UPLOADED_DOCUMENT] ([LiteratureId], [DocumentDate], [Description], [FileName], [StoragePath], [IsActive], [CreatedBy], [CreatedDateTime], [CreatedAt], [ModifiedBy], [ModifiedDateTime], [ModifiedAt], [DeletedBy], [DeletedDateTime], [IsDeleted], [ReferenceGuid], [DocType], [AttachmentTypeId], [ReferenceNo], [ReferenceDate], [OtherAttachmentType]) VALUES (0, CAST(N'2022-09-26' AS Date), N'null', N'643084455922_202210191319204887.pdf', N'\wwwroot\Attachments\CaseManagement\643084455922_202210191319204887.pdf', 1, N'fatwaadmin@gmail.com', CAST(N'2022-10-19T13:20:49.070' AS DateTime), N'\wwwroot\Attachments\CaseManagement\643084455922_202210191319204887.pdf', NULL, NULL, NULL, NULL, NULL, 0, N'494b4efc-9f9f-4fd0-972e-a23ed8aee7aa', N'.pdf', 9, N'4645', CAST(N'2022-10-12' AS Date), N'null')
INSERT [dbo].[UPLOADED_DOCUMENT] ([LiteratureId], [DocumentDate], [Description], [FileName], [StoragePath], [IsActive], [CreatedBy], [CreatedDateTime], [CreatedAt], [ModifiedBy], [ModifiedDateTime], [ModifiedAt], [DeletedBy], [DeletedDateTime], [IsDeleted], [ReferenceGuid], [DocType], [AttachmentTypeId], [ReferenceNo], [ReferenceDate], [OtherAttachmentType]) VALUES (0, CAST(N'2022-09-25' AS Date), N'null', N'643084455922_202210191320182821.pdf', N'\wwwroot\Attachments\User\643084455922_202210191320182821.pdf', 1, N'fatwaadmin@gmail.com', CAST(N'2022-10-19T13:20:49.563' AS DateTime), N'\wwwroot\Attachments\User\643084455922_202210191320182821.pdf', NULL, NULL, NULL, NULL, NULL, 0, N'494b4efc-9f9f-4fd0-972e-a23ed8aee7aa', N'.pdf', 11, N'435', CAST(N'2022-10-17' AS Date), N'null')
INSERT [dbo].[UPLOADED_DOCUMENT] ([LiteratureId], [DocumentDate], [Description], [FileName], [StoragePath], [IsActive], [CreatedBy], [CreatedDateTime], [CreatedAt], [ModifiedBy], [ModifiedDateTime], [ModifiedAt], [DeletedBy], [DeletedDateTime], [IsDeleted], [ReferenceGuid], [DocType], [AttachmentTypeId], [ReferenceNo], [ReferenceDate], [OtherAttachmentType]) VALUES (0, CAST(N'2022-10-02' AS Date), N'null', N'457592948184_202210191321245955.pdf', N'\wwwroot\Attachments\User\457592948184_202210191321245955.pdf', 1, N'fatwaadmin@gmail.com', CAST(N'2022-10-19T13:22:25.513' AS DateTime), N'\wwwroot\Attachments\User\457592948184_202210191321245955.pdf', NULL, NULL, NULL, NULL, NULL, 0, N'494b4efc-9f9f-4fd0-972e-a23ed8aee7aa', N'.pdf', 10, N'564', CAST(N'2022-10-12' AS Date), N'null')
INSERT [dbo].[UPLOADED_DOCUMENT] ([LiteratureId], [DocumentDate], [Description], [FileName], [StoragePath], [IsActive], [CreatedBy], [CreatedDateTime], [CreatedAt], [ModifiedBy], [ModifiedDateTime], [ModifiedAt], [DeletedBy], [DeletedDateTime], [IsDeleted], [ReferenceGuid], [DocType], [AttachmentTypeId], [ReferenceNo], [ReferenceDate], [OtherAttachmentType]) VALUES (0, CAST(N'2022-09-25' AS Date), N'null', N'457592948184_202210191321541308.pdf', N'\wwwroot\Attachments\User\457592948184_202210191321541308.pdf', 1, N'fatwaadmin@gmail.com', CAST(N'2022-10-19T13:22:25.723' AS DateTime), N'\wwwroot\Attachments\User\457592948184_202210191321541308.pdf', NULL, NULL, NULL, NULL, NULL, 0, N'494b4efc-9f9f-4fd0-972e-a23ed8aee7aa', N'.pdf', 12, N'6756', CAST(N'2022-10-18' AS Date), N'jgg')
INSERT [dbo].[UPLOADED_DOCUMENT] ([LiteratureId], [DocumentDate], [Description], [FileName], [StoragePath], [IsActive], [CreatedBy], [CreatedDateTime], [CreatedAt], [ModifiedBy], [ModifiedDateTime], [ModifiedAt], [DeletedBy], [DeletedDateTime], [IsDeleted], [ReferenceGuid], [DocType], [AttachmentTypeId], [ReferenceNo], [ReferenceDate], [OtherAttachmentType]) VALUES (0, CAST(N'2022-10-04' AS Date), N'null', N'457592948184_202210191323167446.pdf', N'\wwwroot\Attachments\CaseManagement\457592948184_202210191323167446.pdf', 1, N'fatwaadmin@gmail.com', CAST(N'2022-10-19T13:23:43.440' AS DateTime), N'\wwwroot\Attachments\CaseManagement\457592948184_202210191323167446.pdf', NULL, NULL, NULL, NULL, NULL, 0, N'9ec8b381-e7af-47f0-91cf-e24c198c21fe', N'.pdf', 9, N'23423', CAST(N'2022-10-11' AS Date), N'null')
INSERT [dbo].[UPLOADED_DOCUMENT] ([LiteratureId], [DocumentDate], [Description], [FileName], [StoragePath], [IsActive], [CreatedBy], [CreatedDateTime], [CreatedAt], [ModifiedBy], [ModifiedDateTime], [ModifiedAt], [DeletedBy], [DeletedDateTime], [IsDeleted], [ReferenceGuid], [DocType], [AttachmentTypeId], [ReferenceNo], [ReferenceDate], [OtherAttachmentType]) VALUES (0, CAST(N'2022-10-10' AS Date), N'null', N'643084455922_202210191323399622.pdf', N'\wwwroot\Attachments\User\643084455922_202210191323399622.pdf', 1, N'fatwaadmin@gmail.com', CAST(N'2022-10-19T13:23:43.647' AS DateTime), N'\wwwroot\Attachments\User\643084455922_202210191323399622.pdf', NULL, NULL, NULL, NULL, NULL, 0, N'9ec8b381-e7af-47f0-91cf-e24c198c21fe', N'.pdf', 10, N'564', CAST(N'2022-10-13' AS Date), N'null')
INSERT [dbo].[UPLOADED_DOCUMENT] ([LiteratureId], [DocumentDate], [Description], [FileName], [StoragePath], [IsActive], [CreatedBy], [CreatedDateTime], [CreatedAt], [ModifiedBy], [ModifiedDateTime], [ModifiedAt], [DeletedBy], [DeletedDateTime], [IsDeleted], [ReferenceGuid], [DocType], [AttachmentTypeId], [ReferenceNo], [ReferenceDate], [OtherAttachmentType]) VALUES (0, CAST(N'2022-09-26' AS Date), N'null', N'457592948184_202210191546558233.pdf', N'\wwwroot\Attachments\CaseManagement\457592948184_202210191546558233.pdf', 1, N'fatwaadmin@gmail.com', CAST(N'2022-10-19T15:47:38.303' AS DateTime), N'\wwwroot\Attachments\CaseManagement\457592948184_202210191546558233.pdf', NULL, NULL, NULL, NULL, NULL, 0, N'7739107f-5346-437b-9d3a-921a241bd344', N'.pdf', 9, N'45645', CAST(N'2022-10-04' AS Date), N'null')
INSERT [dbo].[UPLOADED_DOCUMENT] ([LiteratureId], [DocumentDate], [Description], [FileName], [StoragePath], [IsActive], [CreatedBy], [CreatedDateTime], [CreatedAt], [ModifiedBy], [ModifiedDateTime], [ModifiedAt], [DeletedBy], [DeletedDateTime], [IsDeleted], [ReferenceGuid], [DocType], [AttachmentTypeId], [ReferenceNo], [ReferenceDate], [OtherAttachmentType]) VALUES (0, CAST(N'2022-09-25' AS Date), N'null', N'643084455922_202210191547554474.pdf', N'\wwwroot\Attachments\CaseManagement\643084455922_202210191547554474.pdf', 1, N'fatwaadmin@gmail.com', CAST(N'2022-10-19T15:49:15.267' AS DateTime), N'\wwwroot\Attachments\CaseManagement\643084455922_202210191547554474.pdf', NULL, NULL, NULL, NULL, NULL, 0, N'fe539b29-b161-40b5-8425-5b6930519a5a', N'.pdf', 9, N'3423', CAST(N'2022-10-10' AS Date), N'null')
INSERT [dbo].[UPLOADED_DOCUMENT] ([LiteratureId], [DocumentDate], [Description], [FileName], [StoragePath], [IsActive], [CreatedBy], [CreatedDateTime], [CreatedAt], [ModifiedBy], [ModifiedDateTime], [ModifiedAt], [DeletedBy], [DeletedDateTime], [IsDeleted], [ReferenceGuid], [DocType], [AttachmentTypeId], [ReferenceNo], [ReferenceDate], [OtherAttachmentType]) VALUES (0, CAST(N'2022-09-26' AS Date), N'null', N'643084455922_202210191548272010.pdf', N'\wwwroot\Attachments\User\643084455922_202210191548272010.pdf', 1, N'fatwaadmin@gmail.com', CAST(N'2022-10-19T15:49:15.480' AS DateTime), N'\wwwroot\Attachments\User\643084455922_202210191548272010.pdf', NULL, NULL, NULL, NULL, NULL, 0, N'fe539b29-b161-40b5-8425-5b6930519a5a', N'.pdf', 10, N'23423', CAST(N'2022-10-11' AS Date), N'null')
INSERT [dbo].[UPLOADED_DOCUMENT] ([LiteratureId], [DocumentDate], [Description], [FileName], [StoragePath], [IsActive], [CreatedBy], [CreatedDateTime], [CreatedAt], [ModifiedBy], [ModifiedDateTime], [ModifiedAt], [DeletedBy], [DeletedDateTime], [IsDeleted], [ReferenceGuid], [DocType], [AttachmentTypeId], [ReferenceNo], [ReferenceDate], [OtherAttachmentType]) VALUES (0, CAST(N'2022-10-02' AS Date), N'null', N'643084455922_202210191548386667.pdf', N'\wwwroot\Attachments\User\643084455922_202210191548386667.pdf', 1, N'fatwaadmin@gmail.com', CAST(N'2022-10-19T15:49:15.707' AS DateTime), N'\wwwroot\Attachments\User\643084455922_202210191548386667.pdf', NULL, NULL, NULL, NULL, NULL, 0, N'fe539b29-b161-40b5-8425-5b6930519a5a', N'.pdf', 11, N'234', CAST(N'2022-10-10' AS Date), N'null')
INSERT [dbo].[UPLOADED_DOCUMENT] ([LiteratureId], [DocumentDate], [Description], [FileName], [StoragePath], [IsActive], [CreatedBy], [CreatedDateTime], [CreatedAt], [ModifiedBy], [ModifiedDateTime], [ModifiedAt], [DeletedBy], [DeletedDateTime], [IsDeleted], [ReferenceGuid], [DocType], [AttachmentTypeId], [ReferenceNo], [ReferenceDate], [OtherAttachmentType]) VALUES (0, CAST(N'2022-09-25' AS Date), N'null', N'643084455922_202210191618317364.pdf', N'\wwwroot\Attachments\CaseManagement\643084455922_202210191618317364.pdf', 1, N'fatwaadmin@gmail.com', CAST(N'2022-10-19T16:20:35.683' AS DateTime), N'\wwwroot\Attachments\CaseManagement\643084455922_202210191618317364.pdf', NULL, NULL, NULL, NULL, NULL, 0, N'2b28f07e-54c8-4280-8aa0-4926af745468', N'.pdf', 9, N'45754', CAST(N'2022-10-11' AS Date), N'null')
INSERT [dbo].[UPLOADED_DOCUMENT] ([LiteratureId], [DocumentDate], [Description], [FileName], [StoragePath], [IsActive], [CreatedBy], [CreatedDateTime], [CreatedAt], [ModifiedBy], [ModifiedDateTime], [ModifiedAt], [DeletedBy], [DeletedDateTime], [IsDeleted], [ReferenceGuid], [DocType], [AttachmentTypeId], [ReferenceNo], [ReferenceDate], [OtherAttachmentType]) VALUES (0, CAST(N'2022-09-25' AS Date), N'null', N'643084455922_202210191619316577.pdf', N'\wwwroot\Attachments\User\643084455922_202210191619316577.pdf', 1, N'fatwaadmin@gmail.com', CAST(N'2022-10-19T16:20:36.157' AS DateTime), N'\wwwroot\Attachments\User\643084455922_202210191619316577.pdf', NULL, NULL, NULL, NULL, NULL, 0, N'2b28f07e-54c8-4280-8aa0-4926af745468', N'.pdf', 12, N'5675', CAST(N'2022-10-11' AS Date), N'tewe')
INSERT [dbo].[UPLOADED_DOCUMENT] ([LiteratureId], [DocumentDate], [Description], [FileName], [StoragePath], [IsActive], [CreatedBy], [CreatedDateTime], [CreatedAt], [ModifiedBy], [ModifiedDateTime], [ModifiedAt], [DeletedBy], [DeletedDateTime], [IsDeleted], [ReferenceGuid], [DocType], [AttachmentTypeId], [ReferenceNo], [ReferenceDate], [OtherAttachmentType]) VALUES (0, CAST(N'2022-10-09' AS Date), N'null', N'643084455922_202210191619507466.pdf', N'\wwwroot\Attachments\User\643084455922_202210191619507466.pdf', 1, N'fatwaadmin@gmail.com', CAST(N'2022-10-19T16:20:36.380' AS DateTime), N'\wwwroot\Attachments\User\643084455922_202210191619507466.pdf', NULL, NULL, NULL, NULL, NULL, 0, N'2b28f07e-54c8-4280-8aa0-4926af745468', N'.pdf', 10, N'675', CAST(N'2022-09-27' AS Date), N'null')
INSERT [dbo].[UPLOADED_DOCUMENT] ([LiteratureId], [DocumentDate], [Description], [FileName], [StoragePath], [IsActive], [CreatedBy], [CreatedDateTime], [CreatedAt], [ModifiedBy], [ModifiedDateTime], [ModifiedAt], [DeletedBy], [DeletedDateTime], [IsDeleted], [ReferenceGuid], [DocType], [AttachmentTypeId], [ReferenceNo], [ReferenceDate], [OtherAttachmentType]) VALUES (0, CAST(N'2022-10-02' AS Date), N'null', N'643084455922_202210191620028252.pdf', N'\wwwroot\Attachments\User\643084455922_202210191620028252.pdf', 1, N'fatwaadmin@gmail.com', CAST(N'2022-10-19T16:20:36.613' AS DateTime), N'\wwwroot\Attachments\User\643084455922_202210191620028252.pdf', NULL, NULL, NULL, NULL, NULL, 0, N'2b28f07e-54c8-4280-8aa0-4926af745468', N'.pdf', 11, N'54756', CAST(N'2022-10-10' AS Date), N'null')
INSERT [dbo].[UPLOADED_DOCUMENT] ([LiteratureId], [DocumentDate], [Description], [FileName], [StoragePath], [IsActive], [CreatedBy], [CreatedDateTime], [CreatedAt], [ModifiedBy], [ModifiedDateTime], [ModifiedAt], [DeletedBy], [DeletedDateTime], [IsDeleted], [ReferenceGuid], [DocType], [AttachmentTypeId], [ReferenceNo], [ReferenceDate], [OtherAttachmentType]) VALUES (0, CAST(N'2022-09-26' AS Date), N'wertyu78i', N'maskedDocument (1)_202210191723455801.pdf', N'\wwwroot\Attachments\CaseManagement\maskedDocument (1)_202210191723455801.pdf', 1, N'fatwaadmin@gmail.com', CAST(N'2022-10-19T17:24:09.200' AS DateTime), N'\wwwroot\Attachments\CaseManagement\maskedDocument (1)_202210191723455801.pdf', NULL, NULL, NULL, NULL, NULL, 0, N'd9062c71-cd2b-453c-99f0-19faf538e7bd', N'.pdf', 12, N'1', CAST(N'2022-10-06' AS Date), N'document')
INSERT [dbo].[UPLOADED_DOCUMENT] ([LiteratureId], [DocumentDate], [Description], [FileName], [StoragePath], [IsActive], [CreatedBy], [CreatedDateTime], [CreatedAt], [ModifiedBy], [ModifiedDateTime], [ModifiedAt], [DeletedBy], [DeletedDateTime], [IsDeleted], [ReferenceGuid], [DocType], [AttachmentTypeId], [ReferenceNo], [ReferenceDate], [OtherAttachmentType]) VALUES (0, CAST(N'2022-10-03' AS Date), N'qwertyuiop', N'maskedDocument (1)_202210191737524589.pdf', N'\wwwroot\Attachments\CaseManagement\maskedDocument (1)_202210191737524589.pdf', 1, N'fatwaadmin@gmail.com', CAST(N'2022-10-19T17:38:10.953' AS DateTime), N'\wwwroot\Attachments\CaseManagement\maskedDocument (1)_202210191737524589.pdf', NULL, NULL, NULL, NULL, NULL, 0, N'84f1e4aa-2467-4abb-b782-a23fcb8f0f43', N'.pdf', 12, N'1', CAST(N'2022-10-14' AS Date), N'document')
INSERT [dbo].[UPLOADED_DOCUMENT] ([LiteratureId], [DocumentDate], [Description], [FileName], [StoragePath], [IsActive], [CreatedBy], [CreatedDateTime], [CreatedAt], [ModifiedBy], [ModifiedDateTime], [ModifiedAt], [DeletedBy], [DeletedDateTime], [IsDeleted], [ReferenceGuid], [DocType], [AttachmentTypeId], [ReferenceNo], [ReferenceDate], [OtherAttachmentType]) VALUES (0, CAST(N'2022-10-13' AS Date), N'wertyuiop', N'maskedDocument_202210191751328608.pdf', N'\wwwroot\Attachments\CaseManagement\maskedDocument_202210191751328608.pdf', 1, N'fatwaadmin@gmail.com', CAST(N'2022-10-19T17:51:55.963' AS DateTime), N'\wwwroot\Attachments\CaseManagement\maskedDocument_202210191751328608.pdf', NULL, NULL, NULL, NULL, NULL, 0, N'059abbd2-95b6-4ad5-b965-221e2c649e67', N'.pdf', 12, N'3', CAST(N'2022-10-06' AS Date), N'document')
INSERT [dbo].[UPLOADED_DOCUMENT] ([LiteratureId], [DocumentDate], [Description], [FileName], [StoragePath], [IsActive], [CreatedBy], [CreatedDateTime], [CreatedAt], [ModifiedBy], [ModifiedDateTime], [ModifiedAt], [DeletedBy], [DeletedDateTime], [IsDeleted], [ReferenceGuid], [DocType], [AttachmentTypeId], [ReferenceNo], [ReferenceDate], [OtherAttachmentType]) VALUES (0, CAST(N'2022-10-11' AS Date), N'withdraw case request has been submitted', N'lost-time_202210191759024477.pdf', N'\wwwroot\Attachments\CaseManagement\lost-time_202210191759024477.pdf', 1, N'fatwaadmin@gmail.com', CAST(N'2022-10-19T17:59:08.633' AS DateTime), N'\wwwroot\Attachments\CaseManagement\lost-time_202210191759024477.pdf', NULL, NULL, NULL, NULL, NULL, 0, N'99f31fc2-db96-4c59-9720-8b30adeb342f', N'.pdf', 12, N'5', CAST(N'2022-09-27' AS Date), N'document')
INSERT [dbo].[UPLOADED_DOCUMENT] ([LiteratureId], [DocumentDate], [Description], [FileName], [StoragePath], [IsActive], [CreatedBy], [CreatedDateTime], [CreatedAt], [ModifiedBy], [ModifiedDateTime], [ModifiedAt], [DeletedBy], [DeletedDateTime], [IsDeleted], [ReferenceGuid], [DocType], [AttachmentTypeId], [ReferenceNo], [ReferenceDate], [OtherAttachmentType]) VALUES (0, CAST(N'2022-10-05' AS Date), N'withdraw case document submitted', N'lost-time_202210191802213592.pdf', N'\wwwroot\Attachments\CaseManagement\lost-time_202210191802213592.pdf', 1, N'fatwaadmin@gmail.com', CAST(N'2022-10-19T18:02:27.150' AS DateTime), N'\wwwroot\Attachments\CaseManagement\lost-time_202210191802213592.pdf', NULL, NULL, NULL, NULL, NULL, 0, N'bfdf1d9b-056f-4415-a75c-a59f89798e5e', N'.pdf', 12, N'6', CAST(N'2022-10-11' AS Date), N'document')
INSERT [dbo].[UPLOADED_DOCUMENT] ([LiteratureId], [DocumentDate], [Description], [FileName], [StoragePath], [IsActive], [CreatedBy], [CreatedDateTime], [CreatedAt], [ModifiedBy], [ModifiedDateTime], [ModifiedAt], [DeletedBy], [DeletedDateTime], [IsDeleted], [ReferenceGuid], [DocType], [AttachmentTypeId], [ReferenceNo], [ReferenceDate], [OtherAttachmentType]) VALUES (0, CAST(N'2022-10-05' AS Date), N'qwertyu', N'Ane waa_202210201037002150.pdf', N'\wwwroot\Attachments\CaseManagement\Ane waa_202210201037002150.pdf', 1, N'fatwaadmin@gmail.com', CAST(N'2022-10-20T10:37:06.267' AS DateTime), N'\wwwroot\Attachments\CaseManagement\Ane waa_202210201037002150.pdf', NULL, NULL, NULL, NULL, NULL, 0, N'9c1a9a2d-5e7e-4978-a4cb-33974da26218', N'.pdf', 12, N'7', CAST(N'2022-09-29' AS Date), N'document')
INSERT [dbo].[UPLOADED_DOCUMENT] ([LiteratureId], [DocumentDate], [Description], [FileName], [StoragePath], [IsActive], [CreatedBy], [CreatedDateTime], [CreatedAt], [ModifiedBy], [ModifiedDateTime], [ModifiedAt], [DeletedBy], [DeletedDateTime], [IsDeleted], [ReferenceGuid], [DocType], [AttachmentTypeId], [ReferenceNo], [ReferenceDate], [OtherAttachmentType]) VALUES (0, CAST(N'2022-10-12' AS Date), N'qwertyuijhgbf', N'Ane waa_202210201244031507.pdf', N'\wwwroot\Attachments\CaseManagement\Ane waa_202210201244031507.pdf', 1, N'fatwaadmin@gmail.com', CAST(N'2022-10-20T12:45:13.523' AS DateTime), N'\wwwroot\Attachments\CaseManagement\Ane waa_202210201244031507.pdf', NULL, NULL, NULL, NULL, NULL, 0, N'ca7db008-b3b8-44a3-86c2-bb04bfe9e6e9', N'.pdf', 12, N'7', CAST(N'2022-10-03' AS Date), N'document')
INSERT [dbo].[UPLOADED_DOCUMENT] ([LiteratureId], [DocumentDate], [Description], [FileName], [StoragePath], [IsActive], [CreatedBy], [CreatedDateTime], [CreatedAt], [ModifiedBy], [ModifiedDateTime], [ModifiedAt], [DeletedBy], [DeletedDateTime], [IsDeleted], [ReferenceGuid], [DocType], [AttachmentTypeId], [ReferenceNo], [ReferenceDate], [OtherAttachmentType]) VALUES (0, CAST(N'2022-10-13' AS Date), N'derytuiklk', N'maskedDocument_202210201251405720.pdf', N'\wwwroot\Attachments\CaseManagement\maskedDocument_202210201251405720.pdf', 1, N'fatwaadmin@gmail.com', CAST(N'2022-10-20T12:51:48.950' AS DateTime), N'\wwwroot\Attachments\CaseManagement\maskedDocument_202210201251405720.pdf', NULL, NULL, NULL, NULL, NULL, 0, N'92af788b-6576-4550-b6d9-32fd678631e3', N'.pdf', 12, N'9', CAST(N'2022-10-05' AS Date), N'document')
INSERT [dbo].[UPLOADED_DOCUMENT] ([LiteratureId], [DocumentDate], [Description], [FileName], [StoragePath], [IsActive], [CreatedBy], [CreatedDateTime], [CreatedAt], [ModifiedBy], [ModifiedDateTime], [ModifiedAt], [DeletedBy], [DeletedDateTime], [IsDeleted], [ReferenceGuid], [DocType], [AttachmentTypeId], [ReferenceNo], [ReferenceDate], [OtherAttachmentType]) VALUES (0, CAST(N'2022-10-12' AS Date), N'fytufikjcu', N'Ane waa_202210201525406431.pdf', N'\wwwroot\Attachments\CaseManagement\Ane waa_202210201525406431.pdf', 1, N'fatwaadmin@gmail.com', CAST(N'2022-10-20T15:25:48.877' AS DateTime), N'\wwwroot\Attachments\CaseManagement\Ane waa_202210201525406431.pdf', NULL, NULL, NULL, NULL, NULL, 0, N'8123c2aa-2c3a-4beb-9f92-bf55fae5d2a8', N'.pdf', 12, N'10', CAST(N'2022-10-15' AS Date), N'document')
INSERT [dbo].[UPLOADED_DOCUMENT] ([LiteratureId], [DocumentDate], [Description], [FileName], [StoragePath], [IsActive], [CreatedBy], [CreatedDateTime], [CreatedAt], [ModifiedBy], [ModifiedDateTime], [ModifiedAt], [DeletedBy], [DeletedDateTime], [IsDeleted], [ReferenceGuid], [DocType], [AttachmentTypeId], [ReferenceNo], [ReferenceDate], [OtherAttachmentType]) VALUES (0, CAST(N'2022-10-02' AS Date), N'frytfugkjnb', N'Ane waa_202210201730281843.pdf', N'\wwwroot\Attachments\CaseManagement\Ane waa_202210201730281843.pdf', 1, N'fatwaadmin@gmail.com', CAST(N'2022-10-20T17:30:36.577' AS DateTime), N'\wwwroot\Attachments\CaseManagement\Ane waa_202210201730281843.pdf', NULL, NULL, NULL, NULL, NULL, 0, N'f48d5b6a-2849-40eb-8b2d-33fef357021d', N'.pdf', 12, N'7', CAST(N'2022-09-27' AS Date), N'document')
INSERT [dbo].[UPLOADED_DOCUMENT] ([LiteratureId], [DocumentDate], [Description], [FileName], [StoragePath], [IsActive], [CreatedBy], [CreatedDateTime], [CreatedAt], [ModifiedBy], [ModifiedDateTime], [ModifiedAt], [DeletedBy], [DeletedDateTime], [IsDeleted], [ReferenceGuid], [DocType], [AttachmentTypeId], [ReferenceNo], [ReferenceDate], [OtherAttachmentType]) VALUES (0, CAST(N'2022-10-11' AS Date), N'cftuyk,', N'zzzzzzzzz (2)_202210201747252222.pdf', N'\wwwroot\Attachments\CaseManagement\zzzzzzzzz (2)_202210201747252222.pdf', 1, N'fatwaadmin@gmail.com', CAST(N'2022-10-20T17:47:32.057' AS DateTime), N'\wwwroot\Attachments\CaseManagement\zzzzzzzzz (2)_202210201747252222.pdf', NULL, NULL, NULL, NULL, NULL, 0, N'6de4d717-02cc-4e9f-a8dd-6e82f954b769', N'.pdf', 12, N'88', CAST(N'2022-10-06' AS Date), N'document')
INSERT [dbo].[UPLOADED_DOCUMENT] ([LiteratureId], [DocumentDate], [Description], [FileName], [StoragePath], [IsActive], [CreatedBy], [CreatedDateTime], [CreatedAt], [ModifiedBy], [ModifiedDateTime], [ModifiedAt], [DeletedBy], [DeletedDateTime], [IsDeleted], [ReferenceGuid], [DocType], [AttachmentTypeId], [ReferenceNo], [ReferenceDate], [OtherAttachmentType]) VALUES (0, CAST(N'2022-10-22' AS Date), N'history document', N'Ane waa_202210241005560039.pdf', N'\wwwroot\Attachments\CaseManagement\Ane waa_202210241005560039.pdf', 1, N'fatwaadmin@gmail.com', CAST(N'2022-10-24T10:09:00.403' AS DateTime), N'\wwwroot\Attachments\CaseManagement\Ane waa_202210241005560039.pdf', NULL, NULL, NULL, NULL, NULL, 0, N'a2451314-5357-4acb-89af-1f9ceaeeb64a', N'.pdf', 12, N'13', CAST(N'2022-10-06' AS Date), N'document')
INSERT [dbo].[UPLOADED_DOCUMENT] ([LiteratureId], [DocumentDate], [Description], [FileName], [StoragePath], [IsActive], [CreatedBy], [CreatedDateTime], [CreatedAt], [ModifiedBy], [ModifiedDateTime], [ModifiedAt], [DeletedBy], [DeletedDateTime], [IsDeleted], [ReferenceGuid], [DocType], [AttachmentTypeId], [ReferenceNo], [ReferenceDate], [OtherAttachmentType]) VALUES (0, CAST(N'2022-09-27' AS Date), N'rytuhjhf', N'Ane waa_202210241011407424.pdf', N'\wwwroot\Attachments\CaseManagement\Ane waa_202210241011407424.pdf', 1, N'fatwaadmin@gmail.com', CAST(N'2022-10-24T10:11:44.253' AS DateTime), N'\wwwroot\Attachments\CaseManagement\Ane waa_202210241011407424.pdf', NULL, NULL, NULL, NULL, NULL, 0, N'b4542ef9-95f0-475b-a5e9-ace2a648c296', N'.pdf', 12, N'9', CAST(N'2022-10-12' AS Date), N'document')
INSERT [dbo].[UPLOADED_DOCUMENT] ([LiteratureId], [DocumentDate], [Description], [FileName], [StoragePath], [IsActive], [CreatedBy], [CreatedDateTime], [CreatedAt], [ModifiedBy], [ModifiedDateTime], [ModifiedAt], [DeletedBy], [DeletedDateTime], [IsDeleted], [ReferenceGuid], [DocType], [AttachmentTypeId], [ReferenceNo], [ReferenceDate], [OtherAttachmentType]) VALUES (0, CAST(N'2022-10-24' AS Date), N'null', N'AqeelAltaf_Motivation letterNXT_202210241459385965.pdf', N'\wwwroot\Attachments\CaseManagement\AqeelAltaf_Motivation letterNXT_202210241459385965.pdf', 1, N'fatwaadmin@gmail.com', CAST(N'2022-10-24T14:59:54.087' AS DateTime), N'\wwwroot\Attachments\CaseManagement\AqeelAltaf_Motivation letterNXT_202210241459385965.pdf', NULL, NULL, NULL, NULL, NULL, 0, N'a36e3b85-aff3-4354-9168-68e5c6907070', N'.pdf', 13, N'Ref r1', CAST(N'2022-10-22' AS Date), N'null')
INSERT [dbo].[UPLOADED_DOCUMENT] ([LiteratureId], [DocumentDate], [Description], [FileName], [StoragePath], [IsActive], [CreatedBy], [CreatedDateTime], [CreatedAt], [ModifiedBy], [ModifiedDateTime], [ModifiedAt], [DeletedBy], [DeletedDateTime], [IsDeleted], [ReferenceGuid], [DocType], [AttachmentTypeId], [ReferenceNo], [ReferenceDate], [OtherAttachmentType]) VALUES (0, CAST(N'2022-10-24' AS Date), N'null', N'AqeelAltaf_Motivation letterNXT_202210241504049654.pdf', N'\wwwroot\Attachments\CaseManagement\AqeelAltaf_Motivation letterNXT_202210241504049654.pdf', 1, N'fatwaadmin@gmail.com', CAST(N'2022-10-24T15:04:14.477' AS DateTime), N'\wwwroot\Attachments\CaseManagement\AqeelAltaf_Motivation letterNXT_202210241504049654.pdf', NULL, NULL, NULL, NULL, NULL, 0, N'66244a92-c660-4b1c-806d-8d8d5afbf867', N'.pdf', 13, N'Ref r1', CAST(N'2022-10-07' AS Date), N'null')
INSERT [dbo].[UPLOADED_DOCUMENT] ([LiteratureId], [DocumentDate], [Description], [FileName], [StoragePath], [IsActive], [CreatedBy], [CreatedDateTime], [CreatedAt], [ModifiedBy], [ModifiedDateTime], [ModifiedAt], [DeletedBy], [DeletedDateTime], [IsDeleted], [ReferenceGuid], [DocType], [AttachmentTypeId], [ReferenceNo], [ReferenceDate], [OtherAttachmentType]) VALUES (0, CAST(N'2022-10-24' AS Date), N'null', N'AqeelAltaf_Motivation letterNXT_202210241506561420.pdf', N'\wwwroot\Attachments\CaseManagement\AqeelAltaf_Motivation letterNXT_202210241506561420.pdf', 1, N'fatwaadmin@gmail.com', CAST(N'2022-10-24T15:07:14.023' AS DateTime), N'\wwwroot\Attachments\CaseManagement\AqeelAltaf_Motivation letterNXT_202210241506561420.pdf', NULL, NULL, NULL, NULL, NULL, 0, N'209ecdff-f83c-48ae-bd33-7b7cbf82739a', N'.pdf', 13, N'Ref r2', CAST(N'2022-10-08' AS Date), N'null')
INSERT [dbo].[UPLOADED_DOCUMENT] ([LiteratureId], [DocumentDate], [Description], [FileName], [StoragePath], [IsActive], [CreatedBy], [CreatedDateTime], [CreatedAt], [ModifiedBy], [ModifiedDateTime], [ModifiedAt], [DeletedBy], [DeletedDateTime], [IsDeleted], [ReferenceGuid], [DocType], [AttachmentTypeId], [ReferenceNo], [ReferenceDate], [OtherAttachmentType]) VALUES (0, CAST(N'2022-10-24' AS Date), N'null', N'AqeelAltaf_Motivation letterNXT_202210241532127083.pdf', N'\wwwroot\Attachments\CaseManagement\AqeelAltaf_Motivation letterNXT_202210241532127083.pdf', 1, N'fatwaadmin@gmail.com', CAST(N'2022-10-24T15:32:32.760' AS DateTime), N'\wwwroot\Attachments\CaseManagement\AqeelAltaf_Motivation letterNXT_202210241532127083.pdf', NULL, NULL, NULL, NULL, NULL, 0, N'ae239ed3-bac6-459f-93d7-800f2ee84325', N'.pdf', 13, N'asdas', CAST(N'2022-09-30' AS Date), N'null')
INSERT [dbo].[UPLOADED_DOCUMENT] ([LiteratureId], [DocumentDate], [Description], [FileName], [StoragePath], [IsActive], [CreatedBy], [CreatedDateTime], [CreatedAt], [ModifiedBy], [ModifiedDateTime], [ModifiedAt], [DeletedBy], [DeletedDateTime], [IsDeleted], [ReferenceGuid], [DocType], [AttachmentTypeId], [ReferenceNo], [ReferenceDate], [OtherAttachmentType]) VALUES (0, CAST(N'2022-10-24' AS Date), N'null', N'AqeelAltaf_Motivation letterNXT_202210241532127083.pdf', N'\wwwroot\Attachments\CaseManagement\AqeelAltaf_Motivation letterNXT_202210241532127083.pdf', 1, N'fatwaadmin@gmail.com', CAST(N'2022-10-24T15:32:32.760' AS DateTime), N'\wwwroot\Attachments\CaseManagement\AqeelAltaf_Motivation letterNXT_202210241532127083.pdf', NULL, NULL, NULL, NULL, NULL, 0, N'63ff00b9-0581-4849-b6ac-a630486c1707', N'.pdf', 13, N'asdas', CAST(N'2022-09-30' AS Date), N'null')
INSERT [dbo].[UPLOADED_DOCUMENT] ([LiteratureId], [DocumentDate], [Description], [FileName], [StoragePath], [IsActive], [CreatedBy], [CreatedDateTime], [CreatedAt], [ModifiedBy], [ModifiedDateTime], [ModifiedAt], [DeletedBy], [DeletedDateTime], [IsDeleted], [ReferenceGuid], [DocType], [AttachmentTypeId], [ReferenceNo], [ReferenceDate], [OtherAttachmentType]) VALUES (0, CAST(N'2022-10-24' AS Date), N'null', N'AqeelAltaf_Motivation letterNXT_202210241624214439.pdf', N'\wwwroot\Attachments\CaseManagement\AqeelAltaf_Motivation letterNXT_202210241624214439.pdf', 1, N'fatwaadmin@gmail.com', CAST(N'2022-10-24T16:24:34.343' AS DateTime), N'\wwwroot\Attachments\CaseManagement\AqeelAltaf_Motivation letterNXT_202210241624214439.pdf', NULL, NULL, NULL, NULL, NULL, 0, N'06c843a7-428f-43ed-89c0-5466cae21059', N'.pdf', 13, N'Ref r1', CAST(N'2022-10-13' AS Date), N'null')
INSERT [dbo].[UPLOADED_DOCUMENT] ([LiteratureId], [DocumentDate], [Description], [FileName], [StoragePath], [IsActive], [CreatedBy], [CreatedDateTime], [CreatedAt], [ModifiedBy], [ModifiedDateTime], [ModifiedAt], [DeletedBy], [DeletedDateTime], [IsDeleted], [ReferenceGuid], [DocType], [AttachmentTypeId], [ReferenceNo], [ReferenceDate], [OtherAttachmentType]) VALUES (0, CAST(N'2022-10-24' AS Date), N'null', N'AqeelAltaf_Motivation letterNXT_202210241624565453.pdf', N'\wwwroot\Attachments\CaseManagement\AqeelAltaf_Motivation letterNXT_202210241624565453.pdf', 1, N'fatwaadmin@gmail.com', CAST(N'2022-10-24T16:24:59.237' AS DateTime), N'\wwwroot\Attachments\CaseManagement\AqeelAltaf_Motivation letterNXT_202210241624565453.pdf', NULL, NULL, NULL, NULL, NULL, 0, N'b0e551d7-caf9-4db9-b3b5-c2b5e4f34071', N'.pdf', 13, N'aaaaaaaa', CAST(N'2022-10-15' AS Date), N'null')
INSERT [dbo].[UPLOADED_DOCUMENT] ([LiteratureId], [DocumentDate], [Description], [FileName], [StoragePath], [IsActive], [CreatedBy], [CreatedDateTime], [CreatedAt], [ModifiedBy], [ModifiedDateTime], [ModifiedAt], [DeletedBy], [DeletedDateTime], [IsDeleted], [ReferenceGuid], [DocType], [AttachmentTypeId], [ReferenceNo], [ReferenceDate], [OtherAttachmentType]) VALUES (0, CAST(N'2022-10-25' AS Date), N'null', N'AqeelAltaf_Motivation letterNXT_202210251101295976.pdf', N'\wwwroot\Attachments\CaseManagement\AqeelAltaf_Motivation letterNXT_202210251101295976.pdf', 1, N'fatwaadmin@gmail.com', CAST(N'2022-10-25T11:01:35.873' AS DateTime), N'\wwwroot\Attachments\CaseManagement\AqeelAltaf_Motivation letterNXT_202210251101295976.pdf', NULL, NULL, NULL, NULL, NULL, 0, N'15b348a3-9247-4be0-82f0-8bd83a5873de', N'.pdf', 13, N'asdsad', CAST(N'2022-10-21' AS Date), N'null')
INSERT [dbo].[UPLOADED_DOCUMENT] ([LiteratureId], [DocumentDate], [Description], [FileName], [StoragePath], [IsActive], [CreatedBy], [CreatedDateTime], [CreatedAt], [ModifiedBy], [ModifiedDateTime], [ModifiedAt], [DeletedBy], [DeletedDateTime], [IsDeleted], [ReferenceGuid], [DocType], [AttachmentTypeId], [ReferenceNo], [ReferenceDate], [OtherAttachmentType]) VALUES (0, CAST(N'2022-10-25' AS Date), N'null', N'AqeelAltaf_Motivation letterNXT_202210251114593796.pdf', N'\wwwroot\Attachments\CaseManagement\AqeelAltaf_Motivation letterNXT_202210251114593796.pdf', 1, N'fatwaadmin@gmail.com', CAST(N'2022-10-25T11:15:48.870' AS DateTime), N'\wwwroot\Attachments\CaseManagement\AqeelAltaf_Motivation letterNXT_202210251114593796.pdf', NULL, NULL, NULL, NULL, NULL, 0, N'e247f793-f005-41b0-a9ff-87c4b3291fbc', N'.pdf', 13, N'asdas', CAST(N'2022-10-15' AS Date), N'null')
INSERT [dbo].[UPLOADED_DOCUMENT] ([LiteratureId], [DocumentDate], [Description], [FileName], [StoragePath], [IsActive], [CreatedBy], [CreatedDateTime], [CreatedAt], [ModifiedBy], [ModifiedDateTime], [ModifiedAt], [DeletedBy], [DeletedDateTime], [IsDeleted], [ReferenceGuid], [DocType], [AttachmentTypeId], [ReferenceNo], [ReferenceDate], [OtherAttachmentType]) VALUES (0, CAST(N'2022-10-25' AS Date), N'null', N'AqeelAltaf_Motivation letterNXT_202210251222310338.pdf', N'\wwwroot\Attachments\CaseManagement\AqeelAltaf_Motivation letterNXT_202210251222310338.pdf', 1, N'fatwaadmin@gmail.com', CAST(N'2022-10-25T12:22:33.263' AS DateTime), N'\wwwroot\Attachments\CaseManagement\AqeelAltaf_Motivation letterNXT_202210251222310338.pdf', NULL, NULL, NULL, NULL, NULL, 0, N'ab623fbc-aff8-43f9-91c2-a49ad3eb7059', N'.pdf', 13, N'asdas', CAST(N'2022-10-21' AS Date), N'null')
INSERT [dbo].[UPLOADED_DOCUMENT] ([LiteratureId], [DocumentDate], [Description], [FileName], [StoragePath], [IsActive], [CreatedBy], [CreatedDateTime], [CreatedAt], [ModifiedBy], [ModifiedDateTime], [ModifiedAt], [DeletedBy], [DeletedDateTime], [IsDeleted], [ReferenceGuid], [DocType], [AttachmentTypeId], [ReferenceNo], [ReferenceDate], [OtherAttachmentType]) VALUES (0, CAST(N'2022-10-25' AS Date), N'null', N'AqeelAltaf_Motivation letterNXT_202210251224207549.pdf', N'\wwwroot\Attachments\CaseManagement\AqeelAltaf_Motivation letterNXT_202210251224207549.pdf', 1, N'fatwaadmin@gmail.com', CAST(N'2022-10-25T12:25:46.143' AS DateTime), N'\wwwroot\Attachments\CaseManagement\AqeelAltaf_Motivation letterNXT_202210251224207549.pdf', NULL, NULL, NULL, NULL, NULL, 0, N'aa69ab9a-f8cb-490b-929f-a415f0a45504', N'.pdf', 13, N'Ref r1', CAST(N'2022-10-22' AS Date), N'null')
INSERT [dbo].[UPLOADED_DOCUMENT] ([LiteratureId], [DocumentDate], [Description], [FileName], [StoragePath], [IsActive], [CreatedBy], [CreatedDateTime], [CreatedAt], [ModifiedBy], [ModifiedDateTime], [ModifiedAt], [DeletedBy], [DeletedDateTime], [IsDeleted], [ReferenceGuid], [DocType], [AttachmentTypeId], [ReferenceNo], [ReferenceDate], [OtherAttachmentType]) VALUES (0, CAST(N'2022-10-25' AS Date), N'null', N'AqeelAltaf_Motivation letterNXT_202210251322423106.pdf', N'\wwwroot\Attachments\CaseManagement\AqeelAltaf_Motivation letterNXT_202210251322423106.pdf', 1, N'fatwaadmin@gmail.com', CAST(N'2022-10-25T13:22:45.397' AS DateTime), N'\wwwroot\Attachments\CaseManagement\AqeelAltaf_Motivation letterNXT_202210251322423106.pdf', NULL, NULL, NULL, NULL, NULL, 0, N'f642f9d5-8195-4f81-b88b-b77a4b315354', N'.pdf', 13, N'Ref r1', CAST(N'2022-10-22' AS Date), N'null')
INSERT [dbo].[UPLOADED_DOCUMENT] ([LiteratureId], [DocumentDate], [Description], [FileName], [StoragePath], [IsActive], [CreatedBy], [CreatedDateTime], [CreatedAt], [ModifiedBy], [ModifiedDateTime], [ModifiedAt], [DeletedBy], [DeletedDateTime], [IsDeleted], [ReferenceGuid], [DocType], [AttachmentTypeId], [ReferenceNo], [ReferenceDate], [OtherAttachmentType]) VALUES (0, CAST(N'2022-10-25' AS Date), N'null', N'Network Programming in Dot NET With C Sharp and Visual Basic Dot NET ( PDFDrive )_202210251605211557.pdf', N'\wwwroot\Attachments\CaseManagement\Network Programming in Dot NET With C Sharp and Visual Basic Dot NET ( PDFDrive )_202210251605211557.pdf', 1, N'fatwaadmin@gmail.com', CAST(N'2022-10-25T16:05:25.830' AS DateTime), N'\wwwroot\Attachments\CaseManagement\Network Programming in Dot NET With C Sharp and Visual Basic Dot NET ( PDFDrive )_202210251605211557.pdf', NULL, NULL, NULL, NULL, NULL, 0, N'22fb221e-2aec-4f21-86fa-7db607064dcb', N'.pdf', 13, N'1322342', CAST(N'2022-10-14' AS Date), N'null')
INSERT [dbo].[UPLOADED_DOCUMENT] ([LiteratureId], [DocumentDate], [Description], [FileName], [StoragePath], [IsActive], [CreatedBy], [CreatedDateTime], [CreatedAt], [ModifiedBy], [ModifiedDateTime], [ModifiedAt], [DeletedBy], [DeletedDateTime], [IsDeleted], [ReferenceGuid], [DocType], [AttachmentTypeId], [ReferenceNo], [ReferenceDate], [OtherAttachmentType]) VALUES (0, CAST(N'2022-10-25' AS Date), N'REGT', N'Network Programming in Dot NET With C Sharp and Visual Basic Dot NET ( PDFDrive )_202210251628172497.pdf', N'\wwwroot\Attachments\CaseManagement\Network Programming in Dot NET With C Sharp and Visual Basic Dot NET ( PDFDrive )_202210251628172497.pdf', 1, N'fatwaadmin@gmail.com', CAST(N'2022-10-25T16:28:23.170' AS DateTime), N'\wwwroot\Attachments\CaseManagement\Network Programming in Dot NET With C Sharp and Visual Basic Dot NET ( PDFDrive )_202210251628172497.pdf', NULL, NULL, NULL, NULL, NULL, 0, N'10c9dfb1-5b1b-4f0d-a4d3-de39f8e0f446', N'.pdf', 13, N'555', CAST(N'2022-09-28' AS Date), N'null')
INSERT [dbo].[UPLOADED_DOCUMENT] ([LiteratureId], [DocumentDate], [Description], [FileName], [StoragePath], [IsActive], [CreatedBy], [CreatedDateTime], [CreatedAt], [ModifiedBy], [ModifiedDateTime], [ModifiedAt], [DeletedBy], [DeletedDateTime], [IsDeleted], [ReferenceGuid], [DocType], [AttachmentTypeId], [ReferenceNo], [ReferenceDate], [OtherAttachmentType]) VALUES (0, CAST(N'2022-10-25' AS Date), N'null', N'Network Programming in Dot NET With C Sharp and Visual Basic Dot NET ( PDFDrive )_202210251639067352.pdf', N'\wwwroot\Attachments\CaseManagement\Network Programming in Dot NET With C Sharp and Visual Basic Dot NET ( PDFDrive )_202210251639067352.pdf', 1, N'fatwaadmin@gmail.com', CAST(N'2022-10-25T16:39:09.657' AS DateTime), N'\wwwroot\Attachments\CaseManagement\Network Programming in Dot NET With C Sharp and Visual Basic Dot NET ( PDFDrive )_202210251639067352.pdf', NULL, NULL, NULL, NULL, NULL, 0, N'40228416-3117-47a2-a232-b66109540a6e', N'.pdf', 13, N'Ref ads', CAST(N'2022-10-21' AS Date), N'null')
INSERT [dbo].[UPLOADED_DOCUMENT] ([LiteratureId], [DocumentDate], [Description], [FileName], [StoragePath], [IsActive], [CreatedBy], [CreatedDateTime], [CreatedAt], [ModifiedBy], [ModifiedDateTime], [ModifiedAt], [DeletedBy], [DeletedDateTime], [IsDeleted], [ReferenceGuid], [DocType], [AttachmentTypeId], [ReferenceNo], [ReferenceDate], [OtherAttachmentType]) VALUES (0, CAST(N'2022-10-25' AS Date), N'null', N'Network Programming in Dot NET With C Sharp and Visual Basic Dot NET ( PDFDrive )_202210251644423656.pdf', N'\wwwroot\Attachments\CaseManagement\Network Programming in Dot NET With C Sharp and Visual Basic Dot NET ( PDFDrive )_202210251644423656.pdf', 1, N'fatwaadmin@gmail.com', CAST(N'2022-10-25T16:44:45.270' AS DateTime), N'\wwwroot\Attachments\CaseManagement\Network Programming in Dot NET With C Sharp and Visual Basic Dot NET ( PDFDrive )_202210251644423656.pdf', NULL, NULL, NULL, NULL, NULL, 0, N'4e5e0136-aff8-4d25-a6d4-11fd6505f0f9', N'.pdf', 13, N'hgfhgf', CAST(N'2022-10-21' AS Date), N'null')
INSERT [dbo].[UPLOADED_DOCUMENT] ([LiteratureId], [DocumentDate], [Description], [FileName], [StoragePath], [IsActive], [CreatedBy], [CreatedDateTime], [CreatedAt], [ModifiedBy], [ModifiedDateTime], [ModifiedAt], [DeletedBy], [DeletedDateTime], [IsDeleted], [ReferenceGuid], [DocType], [AttachmentTypeId], [ReferenceNo], [ReferenceDate], [OtherAttachmentType]) VALUES (0, CAST(N'2022-10-25' AS Date), N'dwed', N'Network Programming in Dot NET With C Sharp and Visual Basic Dot NET ( PDFDrive )_202210251651289109.pdf', N'\wwwroot\Attachments\CaseManagement\Network Programming in Dot NET With C Sharp and Visual Basic Dot NET ( PDFDrive )_202210251651289109.pdf', 1, N'fatwaadmin@gmail.com', CAST(N'2022-10-25T16:51:32.127' AS DateTime), N'\wwwroot\Attachments\CaseManagement\Network Programming in Dot NET With C Sharp and Visual Basic Dot NET ( PDFDrive )_202210251651289109.pdf', NULL, NULL, NULL, NULL, NULL, 0, N'90491b8f-ed58-43ff-9109-d47662095ac6', N'.pdf', 13, N'234', CAST(N'2022-10-20' AS Date), N'null')
INSERT [dbo].[UPLOADED_DOCUMENT] ([LiteratureId], [DocumentDate], [Description], [FileName], [StoragePath], [IsActive], [CreatedBy], [CreatedDateTime], [CreatedAt], [ModifiedBy], [ModifiedDateTime], [ModifiedAt], [DeletedBy], [DeletedDateTime], [IsDeleted], [ReferenceGuid], [DocType], [AttachmentTypeId], [ReferenceNo], [ReferenceDate], [OtherAttachmentType]) VALUES (0, CAST(N'2022-10-25' AS Date), N'sws', N'Network Programming in Dot NET With C Sharp and Visual Basic Dot NET ( PDFDrive )_202210251724594901.pdf', N'\wwwroot\Attachments\CaseManagement\Network Programming in Dot NET With C Sharp and Visual Basic Dot NET ( PDFDrive )_202210251724594901.pdf', 1, N'fatwaadmin@gmail.com', CAST(N'2022-10-25T17:25:03.227' AS DateTime), N'\wwwroot\Attachments\CaseManagement\Network Programming in Dot NET With C Sharp and Visual Basic Dot NET ( PDFDrive )_202210251724594901.pdf', NULL, NULL, NULL, NULL, NULL, 0, N'1148ec48-c75b-42c9-bca3-3853d5b8eb77', N'.pdf', 13, N'32', CAST(N'2022-10-13' AS Date), N'null')
INSERT [dbo].[UPLOADED_DOCUMENT] ([LiteratureId], [DocumentDate], [Description], [FileName], [StoragePath], [IsActive], [CreatedBy], [CreatedDateTime], [CreatedAt], [ModifiedBy], [ModifiedDateTime], [ModifiedAt], [DeletedBy], [DeletedDateTime], [IsDeleted], [ReferenceGuid], [DocType], [AttachmentTypeId], [ReferenceNo], [ReferenceDate], [OtherAttachmentType]) VALUES (0, CAST(N'2022-10-25' AS Date), N'as', N'Network Programming in Dot NET With C Sharp and Visual Basic Dot NET ( PDFDrive )_202210251726459209.pdf', N'\wwwroot\Attachments\CaseManagement\Network Programming in Dot NET With C Sharp and Visual Basic Dot NET ( PDFDrive )_202210251726459209.pdf', 1, N'fatwaadmin@gmail.com', CAST(N'2022-10-25T17:26:52.823' AS DateTime), N'\wwwroot\Attachments\CaseManagement\Network Programming in Dot NET With C Sharp and Visual Basic Dot NET ( PDFDrive )_202210251726459209.pdf', NULL, NULL, NULL, NULL, NULL, 0, N'6d5026af-f7ab-493f-97c4-55cbe5903b25', N'.pdf', 13, N'wsd', CAST(N'2022-10-20' AS Date), N'null')
INSERT [dbo].[UPLOADED_DOCUMENT] ([LiteratureId], [DocumentDate], [Description], [FileName], [StoragePath], [IsActive], [CreatedBy], [CreatedDateTime], [CreatedAt], [ModifiedBy], [ModifiedDateTime], [ModifiedAt], [DeletedBy], [DeletedDateTime], [IsDeleted], [ReferenceGuid], [DocType], [AttachmentTypeId], [ReferenceNo], [ReferenceDate], [OtherAttachmentType]) VALUES (0, CAST(N'2022-10-25' AS Date), N'sds', N'Network Programming in Dot NET With C Sharp and Visual Basic Dot NET ( PDFDrive )_202210251734121121.pdf', N'\wwwroot\Attachments\CaseManagement\Network Programming in Dot NET With C Sharp and Visual Basic Dot NET ( PDFDrive )_202210251734121121.pdf', 1, N'fatwaadmin@gmail.com', CAST(N'2022-10-25T17:34:17.510' AS DateTime), N'\wwwroot\Attachments\CaseManagement\Network Programming in Dot NET With C Sharp and Visual Basic Dot NET ( PDFDrive )_202210251734121121.pdf', NULL, NULL, NULL, NULL, NULL, 0, N'67c7a25e-d92a-4865-a1a5-907732aa1033', N'.pdf', 13, N'223e', CAST(N'2022-10-07' AS Date), N'null')
INSERT [dbo].[UPLOADED_DOCUMENT] ([LiteratureId], [DocumentDate], [Description], [FileName], [StoragePath], [IsActive], [CreatedBy], [CreatedDateTime], [CreatedAt], [ModifiedBy], [ModifiedDateTime], [ModifiedAt], [DeletedBy], [DeletedDateTime], [IsDeleted], [ReferenceGuid], [DocType], [AttachmentTypeId], [ReferenceNo], [ReferenceDate], [OtherAttachmentType]) VALUES (0, CAST(N'2022-10-25' AS Date), N'wew', N'Network Programming in Dot NET With C Sharp and Visual Basic Dot NET ( PDFDrive )_202210251735217028.pdf', N'\wwwroot\Attachments\CaseManagement\Network Programming in Dot NET With C Sharp and Visual Basic Dot NET ( PDFDrive )_202210251735217028.pdf', 1, N'fatwaadmin@gmail.com', CAST(N'2022-10-25T17:35:27.147' AS DateTime), N'\wwwroot\Attachments\CaseManagement\Network Programming in Dot NET With C Sharp and Visual Basic Dot NET ( PDFDrive )_202210251735217028.pdf', NULL, NULL, NULL, NULL, NULL, 0, N'ee78cdab-ba41-49d5-b5c5-a9fb911aab67', N'.pdf', 13, N'we', CAST(N'2022-10-01' AS Date), N'null')
INSERT [dbo].[UPLOADED_DOCUMENT] ([LiteratureId], [DocumentDate], [Description], [FileName], [StoragePath], [IsActive], [CreatedBy], [CreatedDateTime], [CreatedAt], [ModifiedBy], [ModifiedDateTime], [ModifiedAt], [DeletedBy], [DeletedDateTime], [IsDeleted], [ReferenceGuid], [DocType], [AttachmentTypeId], [ReferenceNo], [ReferenceDate], [OtherAttachmentType]) VALUES (0, CAST(N'2022-10-25' AS Date), N'null', N'Network Programming in Dot NET With C Sharp and Visual Basic Dot NET ( PDFDrive )_202210251743577214.pdf', N'\wwwroot\Attachments\CaseManagement\Network Programming in Dot NET With C Sharp and Visual Basic Dot NET ( PDFDrive )_202210251743577214.pdf', 1, N'fatwaadmin@gmail.com', CAST(N'2022-10-25T17:44:01.030' AS DateTime), N'\wwwroot\Attachments\CaseManagement\Network Programming in Dot NET With C Sharp and Visual Basic Dot NET ( PDFDrive )_202210251743577214.pdf', NULL, NULL, NULL, NULL, NULL, 0, N'cf26dcdd-2fd3-4a00-bbe7-1f9386d904fd', N'.pdf', 13, N'vbhc', CAST(N'2022-10-21' AS Date), N'null')


SET IDENTITY_INSERT CMS_CASE_REQUEST_EVENT_G2G_LKP ON
INSERT INTO CMS_CASE_REQUEST_EVENT_G2G_LKP
(Id, Name_En, Name_Ar)
VALUES
('4','Transfer','Transfer')

------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------
--<History Author = 'Hassan Abbas' Date='2022-10-26' Version="1.0" Branch="master"> ABOVE SCRIPT EXECUTED ON FATWA_DB_DEV</History>
------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------


UPDATE CMS_CASE_PARTY_TYPE_G2G_LKP SET Name_En = 'GovernmentEntity', Name_Ar = 'GovernmentEntity' WHERE Name_En = 'Ministry'


SET IDENTITY_INSERT ATTACHMENT_TYPE ON

INSERT INTO ATTACHMENT_TYPE 
(AttachmentTypeId, Type_Ar, Type_En, ModuleId,IsMandatory)
VALUES 
('14','CivilId','CivilId',5,0)

INSERT INTO ATTACHMENT_TYPE 
(AttachmentTypeId, Type_Ar, Type_En, ModuleId,IsMandatory)
VALUES 
('15','MOCICertificate','MOCICertificate',5,0)

SET IDENTITY_INSERT ATTACHMENT_TYPE OFF

update ATTACHMENT_TYPE set IsOfficialLetter = '1' where AttachmentTypeId = '9'


------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------
--<History Author = 'Hassan Abbas' Date='2022-11-03' Version="1.0" Branch="master"> ABOVE SCRIPT EXECUTED ON FATWA_DB_DEV</History>
------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------


SET IDENTITY_INSERT CMS_CASE_REQUEST_EVENT_G2G_LKP ON
INSERT INTO CMS_CASE_REQUEST_EVENT_G2G_LKP
(Id, Name_En, Name_Ar)
VALUES
('5','AssignToLawyer','AssignToLawyer')

IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Permissions.Menu.Meeting')
	INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted])
	VALUES ('Meeting Menu', 'Meeting', 'Meeting', 'Permission', 'Permissions.Menu.Meeting', 'Meeting Menu', 0)
Go

IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Permissions.Submenu.Meeting.MeetingList')
	INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted])
	VALUES ('List Of Meeting', 'Meeting', 'Meeting', 'Permission', 'Permissions.Submenu.Meeting.MeetingList', 'List Of Meeting', 0)
Go


SET IDENTITY_INSERT Module ON
INSERT INTO Module (ModuleNameEn, ModuleNameAr)
VALUES ('Meeting', 'Meeting' );

-- [MEET_MEETING_ATTENDEE_TYPE] 
GO
INSERT [dbo].[MEET_MEETING_ATTENDEE_TYPE] ([MeetingAttendeeTypeId], [NameEn]) VALUES (1, N'Legislation Attendee')
INSERT [dbo].[MEET_MEETING_ATTENDEE_TYPE] ([MeetingAttendeeTypeId], [NameEn]) VALUES (2, N'GE Attendee')
GO
-- [MEET_MEETING_STATUS]
INSERT [dbo].[MEET_MEETING_STATUS] ([MeetingStatusId], [NameEn], [NameAr]) VALUES (1, N'RequestedByGE', N'مطلوب من قبل جهة حكومية')
INSERT [dbo].[MEET_MEETING_STATUS] ([MeetingStatusId], [NameEn], [NameAr]) VALUES (2, N'Scheduled', N'المقرر')
INSERT [dbo].[MEET_MEETING_STATUS] ([MeetingStatusId], [NameEn], [NameAr]) VALUES (4, N'ApprovedByHOS', N'معتمد من قبل رئيس القطاعات')
INSERT [dbo].[MEET_MEETING_STATUS] ([MeetingStatusId], [NameEn], [NameAr]) VALUES (8, N'RejectedByHOS', N'رفضه رئيس القطاعات')
INSERT [dbo].[MEET_MEETING_STATUS] ([MeetingStatusId], [NameEn], [NameAr]) VALUES (16, N'PendingForReviewByGE', N'قيد المراجعة من قبل الجهة الحكومية')
INSERT [dbo].[MEET_MEETING_STATUS] ([MeetingStatusId], [NameEn], [NameAr]) VALUES (32, N'ApprovedByGE', N'معتمدة من الجهة الحكومية')
INSERT [dbo].[MEET_MEETING_STATUS] ([MeetingStatusId], [NameEn], [NameAr]) VALUES (64, N'RejectedByGE', N'مرفوضة من قبل الجهة الحكومية')
INSERT [dbo].[MEET_MEETING_STATUS] ([MeetingStatusId], [NameEn], [NameAr]) VALUES (128, N'Held', N'محتجز')
INSERT [dbo].[MEET_MEETING_STATUS] ([MeetingStatusId], [NameEn], [NameAr]) VALUES (256, N'Complete', N'مكتمل')
GO
-- [MEET_MEETING_TYPE] 
INSERT [dbo].[MEET_MEETING_TYPE] ([MeetingTypeId], [NameEn], [NameAr]) VALUES (1, N'Internal', N'داخلي')
INSERT [dbo].[MEET_MEETING_TYPE] ([MeetingTypeId], [NameEn], [NameAr]) VALUES (2, N'External', N'خارجي')
GO

IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Permissions.Submenu.Meeting.MeetingList.MeetingDecision')
	INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted])
	VALUES ('Meeting Decision', 'Meeting', 'Meeting', 'Permission', 'Permissions.Submenu.Meeting.MeetingList.MeetingDecision', 'Meeting Decision', 0)
Go 
 
IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Permissions.Submenu.Meeting.MeetingList.AddMOM')
	INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted])
	VALUES ('Add MOM', 'Meeting', 'Meeting', 'Permission', 'Permissions.Submenu.Meeting.MeetingList.AddMOM', 'Add MOM', 0)
Go 
 
--CMS_TEMPLATE
SET IDENTITY_INSERT CMS_TEMPLATE ON

INSERT INTO CMS_TEMPLATE (Id,NameEn,NameAr,Content,IsActive,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,DeletedBy,DeletedDate,IsDeleted,AttachmentTypeId)
VALUES(1,'No Template',N'No Template','',1,'fatwaadmin@gmail.com','2022-10-11 17:49:54.350',NULL,NULL,NULL,NULL,0,0)

INSERT INTO CMS_TEMPLATE (Id,NameEn,NameAr,Content,IsActive,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,DeletedBy,DeletedDate,IsDeleted,AttachmentTypeId)
VALUES(2,'Blank Template',N'Blank Template','',1,'fatwaadmin@gmail.com','2022-10-11 17:49:54.350',NULL,NULL,NULL,NULL,0,0)

INSERT INTO CMS_TEMPLATE (Id,NameEn,NameAr,Content,IsActive,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,DeletedBy,DeletedDate,IsDeleted,AttachmentTypeId)
VALUES(3,'Filing a Lawsuit Notification',N'إخطار بإقامة دعوى','',1,'fatwaadmin@gmail.com','2022-10-11 17:49:54.350',NULL,NULL,NULL,NULL,0,11)

INSERT INTO CMS_TEMPLATE (Id,NameEn,NameAr,Content,IsActive,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,DeletedBy,DeletedDate,IsDeleted,AttachmentTypeId)
VALUES(4,'Judgement Notification',N'إخطار بالحكم','',1,'fatwaadmin@gmail.com','2022-10-11 17:49:54.350',NULL,NULL,NULL,NULL,0,11)

INSERT INTO CMS_TEMPLATE (Id,NameEn,NameAr,Content,IsActive,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,DeletedBy,DeletedDate,IsDeleted,AttachmentTypeId)
VALUES(5,'Interrogarion Judgement Notification',N'إخطار بحكم استجواب','',1,'fatwaadmin@gmail.com','2022-10-11 17:49:54.350',NULL,NULL,NULL,NULL,0,11)

INSERT INTO CMS_TEMPLATE (Id,NameEn,NameAr,Content,IsActive,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,DeletedBy,DeletedDate,IsDeleted,AttachmentTypeId)
VALUES(6,'Urgency Notification',N'إخطار استعجال','',1,'fatwaadmin@gmail.com','2022-10-11 17:49:54.350',NULL,NULL,NULL,NULL,0,11)

INSERT INTO CMS_TEMPLATE (Id,NameEn,NameAr,Content,IsActive,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,DeletedBy,DeletedDate,IsDeleted,AttachmentTypeId)
VALUES(7,'What Happened Notification',N'إخطار بما تم','',1,'fatwaadmin@gmail.com','2022-10-11 17:49:54.350',NULL,NULL,NULL,NULL,0,11)

INSERT INTO CMS_TEMPLATE (Id,NameEn,NameAr,Content,IsActive,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,DeletedBy,DeletedDate,IsDeleted,AttachmentTypeId)
VALUES(8,'Incoming Report Notification',N'إخطار بورود تقرير','',1,'fatwaadmin@gmail.com','2022-10-11 17:49:54.350',NULL,NULL,NULL,NULL,0,11)

INSERT INTO CMS_TEMPLATE (Id,NameEn,NameAr,Content,IsActive,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,DeletedBy,DeletedDate,IsDeleted,AttachmentTypeId)
VALUES(9,'Transfer Files Request',N'طلب إحالة ملفات','',1,'fatwaadmin@gmail.com','2022-10-11 17:49:54.350',NULL,NULL,NULL,NULL,0,11)

INSERT INTO CMS_TEMPLATE (Id,NameEn,NameAr,Content,IsActive,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,DeletedBy,DeletedDate,IsDeleted,AttachmentTypeId)
VALUES(10,'Request For Additional Information Notification',N'إخطار بطلب معلومات','',1,'fatwaadmin@gmail.com','2022-10-11 17:49:54.350',NULL,NULL,NULL,NULL,0,11)

INSERT INTO CMS_TEMPLATE (Id,NameEn,NameAr,Content,IsActive,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,DeletedBy,DeletedDate,IsDeleted,AttachmentTypeId)
VALUES(11,'Initial Judgemenet Notification',N'إخطار بحكم تمهيدي','',1,'fatwaadmin@gmail.com','2022-10-11 17:49:54.350',NULL,NULL,NULL,NULL,0,11)

SET IDENTITY_INSERT CMS_TEMPLATE OFF

--PARAMETER
SET IDENTITY_INSERT PARAMETER ON
INSERT INTO PARAMETER  (ParameterId,Name,PKey,Mandatory,IsAutoPopulated)
VALUES (13,'Document Date','CmsTempDocumentDate',0,1)

INSERT INTO PARAMETER  (ParameterId,Name,PKey,Mandatory,IsAutoPopulated)
VALUES (14,'Reference Number','CmsTempReferenceNumber',0,1)

INSERT INTO PARAMETER  (ParameterId,Name,PKey,Mandatory,IsAutoPopulated)
VALUES (15,'Name','CmsTempName',0,0)

INSERT INTO PARAMETER  (ParameterId,Name,PKey,Mandatory,IsAutoPopulated)
VALUES (16,'Case Number','CmsTempCaseNumber',0,1)

INSERT INTO PARAMETER  (ParameterId,Name,PKey,Mandatory,IsAutoPopulated)
VALUES (17,'Start Date for Lawsuit','CmsTempStartDateforLawsuit',0,1)

INSERT INTO PARAMETER  (ParameterId,Name,PKey,Mandatory,IsAutoPopulated)
VALUES (18,'Plaintiff Name','CmsTempPlaintiffName',0,1)

INSERT INTO PARAMETER  (ParameterId,Name,PKey,Mandatory,IsAutoPopulated)
VALUES (19,'Defendant Name','CmsTempDefendantName',0,1)

INSERT INTO PARAMETER  (ParameterId,Name,PKey,Mandatory,IsAutoPopulated)
VALUES (20,'Chamber Name','CmsTempChamberName',0,1)

INSERT INTO PARAMETER  (ParameterId,Name,PKey,Mandatory,IsAutoPopulated)
VALUES (21,'Hearing Date','CmsTempHearingDate',0,1)

INSERT INTO PARAMETER  (ParameterId,Name,PKey,Mandatory,IsAutoPopulated)
VALUES (22,'Lawywer Name','CmsTempLawywerName',0,1)

INSERT INTO PARAMETER  (ParameterId,Name,PKey,Mandatory,IsAutoPopulated)
VALUES (23,'Previous Hearing Date (Old)','CmsTempPreviousHearingDate',0,1)

INSERT INTO PARAMETER  (ParameterId,Name,PKey,Mandatory,IsAutoPopulated)
VALUES (24,'Government Entity Name','CmsTempGovernmentEntityName',0,1)

INSERT INTO PARAMETER  (ParameterId,Name,PKey,Mandatory,IsAutoPopulated)
VALUES (25,'Expert Report Number','CmsTempExpertReportNumber',0,1)

INSERT INTO PARAMETER  (ParameterId,Name,PKey,Mandatory,IsAutoPopulated)
VALUES (26,'Expert Report Date','CmsTempExpertReportDate',0,1)

INSERT INTO PARAMETER  (ParameterId,Name,PKey,Mandatory,IsAutoPopulated)
VALUES (27,'Postponed Hearing Date','CmsTempPostponedHearingDate',0,1)

INSERT INTO PARAMETER  (ParameterId,Name,PKey,Mandatory,IsAutoPopulated)
VALUES (28,'Expected Hearing Date','CmsTempExpectedHearingDate',0,1)

INSERT INTO PARAMETER  (ParameterId,Name,PKey,Mandatory,IsAutoPopulated)
VALUES (29,'Email Subject','SendEmail_Subject',0,0)

INSERT INTO PARAMETER  (ParameterId,Name,PKey,Mandatory,IsAutoPopulated)
VALUES (30,'Email Body','SendEmail_Body',0,0)

INSERT INTO PARAMETER  (ParameterId,Name,PKey,Mandatory,IsAutoPopulated)
VALUES (31, 'Court Name', 'CmsTempCourtName', 0, 1)

INSERT INTO PARAMETER  (ParameterId,Name,PKey,Mandatory,IsAutoPopulated)
VALUES (32, 'No Template Content', 'CmsTempNoTemplateContent', 0, 1)

INSERT INTO PARAMETER  (ParameterId,Name,PKey,Mandatory,IsAutoPopulated)
VALUES (33, 'Additional Section Content', 'CmsTempAdditionalSectionContent', 0, 1)

INSERT INTO PARAMETER  (ParameterId,Name,PKey,Mandatory,IsAutoPopulated)
VALUES (34, 'Attachments', 'CmsTempAttachments', 0, 0)

SET IDENTITY_INSERT PARAMETER OFF


--CMS_SECTION
INSERT INTO CMS_SECTION VALUES (1,'No Template Section','No Template Section')
INSERT INTO CMS_SECTION VALUES (2,'Additional Section','Additional Section')
INSERT INTO CMS_SECTION VALUES (3,'Opening Statement','Opening Statement')
INSERT INTO CMS_SECTION VALUES (4,'Body','Body')
INSERT INTO CMS_SECTION VALUES (5,'Closing Statement','Closing Statement')


UPDATE CMS_TEMPLATE SET Content = N'<!DOCTYPE html>
<html>

<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title></title>
</head>

<body style="margin: 132px 108px 0px 87px; color: rgb(0, 0, 0); background-color: rgb(255, 255, 255);">
<p dir="RTL" style="margin:0cm;font-size:16px;font-family:''Times New Roman'',serif;text-align:justify;"><strong><span dir="LTR" style="font-size:13px;font-family:''Calibri'',sans-serif;">&nbsp;</span></strong></p>
<p dir="RTL" style="margin:0cm;font-size:16px;font-family:''Times New Roman'',serif;text-align:justify;"><strong><span dir="LTR" style="font-size:13px;font-family:''Calibri'',sans-serif;">&nbsp;</span></strong></p>
<p dir="RTL" style="margin:0cm;font-size:16px;font-family:''Times New Roman'',serif;text-align:justify;"><strong><span dir="LTR" style="font-size:13px;font-family:''Calibri'',sans-serif;">&nbsp;</span></strong></p>
<p dir="RTL" style="margin:0cm;font-size:16px;font-family:''Times New Roman'',serif;text-align:justify;"><strong><span style="font-size:19px;font-family:''Calibri'',sans-serif;">الكويت في :&nbsp;#CmsTempOutboxDate#</span></strong></p>
<p dir="RTL" style="margin:0cm;font-size:16px;font-family:''Times New Roman'',serif;text-align:right;"><strong><span style="font-size:19px;font-family:''Calibri'',sans-serif;">مرجع رقم :&nbsp;#CmsTempOutboxNumber#</span></strong><strong><span style="font-size:19px;font-family:''Calibri'',sans-serif;">&nbsp;</span></strong></p>
<h2 dir="RTL" style="margin:0cm;text-align:center;font-size:19px;font-family:''Simplified Arabic'',serif;"><span style="font-family:''Calibri'',sans-serif;">&nbsp;</span></h2>
<div style="margin:0cm;font-size:16px;font-family:''Times New Roman'',serif;border:solid windowtext 1.0pt;padding:1.0pt 4.0pt 1.0pt 4.0pt;background:#F3F3F3;">
    <p dir="RTL" style="margin:0cm;font-size:16px;font-family:''Times New Roman'',serif;text-align:center;background:#F3F3F3;border:none;padding:0cm;"><strong><span style="font-size:28px;font-family:''Calibri'',sans-serif;color:black;">إخطار بإقامة دعوى&nbsp;</span></strong></p>
</div>
<p dir="RTL" style="margin:0cm;font-size:16px;font-family:''Times New Roman'',serif;text-align:center;"><strong><span style="font-size:11px;font-family:''Calibri'',sans-serif;">&nbsp;</span></strong></p>
<p dir="RTL" style="margin:0cm;font-size:16px;font-family:''Times New Roman'',serif;margin-bottom:12.0pt;text-align:justify;"><strong><span style="font-size:24px;font-family:''Calibri'',sans-serif;">&nbsp;</span></strong></p>
<p dir="RTL" style="margin:0cm;font-size:16px;font-family:''Times New Roman'',serif;margin-bottom:12.0pt;text-align:justify;"><strong><span style="font-size:24px;font-family:''Calibri'',sans-serif;">حضرة</span></strong><strong><span style="font-size:24px;font-family:''Calibri'',sans-serif;">&nbsp;</span></strong><strong><span style="font-size:24px;font-family:''Calibri'',sans-serif;">الفاضل/ #CmsTempName#</span></strong><strong><span style="font-size:24px;font-family:''Calibri'',sans-serif;">&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;</span></strong><strong><span style="font-size:24px;font-family:''Calibri'',sans-serif;">المحتـرم</span></strong></p>
<p dir="RTL" style="margin:0cm;font-size:16px;font-family:''Times New Roman'',serif;margin-bottom:12.0pt;text-align:justify;"><strong><span style="font-size:21px;font-family:''Calibri'',sans-serif;">تحية طيبة وبعد ،،،</span></strong></p>
<p dir="RTL" style="margin:0cm;font-size:16px;font-family:''Times New Roman'',serif;text-align:justify;"><strong><span style="font-size:21px;font-family:''Calibri'',sans-serif;">&nbsp;</span></strong></p>
<p dir="RTL" style="margin:0cm;font-size:16px;font-family:''Times New Roman'',serif;text-align:justify;"><strong><span style="font-size:21px;font-family:''Calibri'',sans-serif;">مُرفق طيه صورة من صحيفة الدعوى رقم&nbsp;#CmsTempCaseNumber#</span></strong><strong><span style="font-size:21px;font-family:''Calibri'',sans-serif;">&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;المقامة</span></strong><strong><span style="font-size:21px;font-family:''Calibri'',sans-serif;">&nbsp;:&nbsp;#CmsTempStartDateforLawsuit#</span></strong></p>
<p dir="RTL" style="margin:0cm;font-size:16px;font-family:''Times New Roman'',serif;text-align:justify;text-indent:8.15pt;"><strong><span style="font-size:21px;font-family:''Calibri'',sans-serif;">&nbsp;</span></strong></p>
<p dir="RTL" style="margin:0cm;font-size:16px;font-family:''Times New Roman'',serif;margin-top:12.0pt;text-align:justify;text-indent:8.15pt;"><strong><span style="font-size:21px;font-family:''Calibri'',sans-serif;">مــــن :&nbsp;#CmsTempPlaintiffName#</span></strong></p>
<p dir="RTL" style="margin:0cm;font-size:16px;font-family:''Times New Roman'',serif;margin-top:12.0pt;text-align:justify;text-indent:8.15pt;"><strong><span style="font-size:21px;font-family:''Calibri'',sans-serif;">ضــــد:&nbsp;#CmsTempDefendantName#</span></strong></p>
<p dir="RTL" style="margin:0cm;font-size:16px;font-family:''Times New Roman'',serif;margin-top:12.0pt;text-align:justify;text-indent:8.15pt;"><strong><span style="font-size:21px;font-family:''Calibri'',sans-serif;">الدائرة الإدارية /&nbsp;#CmsTempChamberName#</span></strong><strong><span style="font-size:21px;font-family:''Calibri'',sans-serif;">&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;والمحدد لنظرها جلسة:&nbsp;#CmsTempHearingDate#</span></strong></p>
<p dir="RTL" style="margin:0cm;font-size:16px;font-family:''Times New Roman'',serif;text-align:justify;text-indent:8.15pt;"><strong><span style="font-size:21px;font-family:''Calibri'',sans-serif;">&nbsp;</span></strong></p>
<p dir="RTL" style="margin:0cm;font-size:16px;font-family:''Times New Roman'',serif;text-align:right;text-indent:14.8pt;"><strong><span style="font-size:21px;font-family:''Calibri'',sans-serif;">يرجى العلم والإحاطة واتخاذ ما يلزم بشأن تمثيل &nbsp;#CmsTempGovernmentEntityName# &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;في هذه الدعوى وإبداء ما يلزم من دفاع فيها بمعرفة الإدارة القانونية لديكم.</span></strong></p>
<p dir="RTL" style="margin:0cm;font-size:16px;font-family:''Times New Roman'',serif;text-align:center;"><strong><span style="font-size:21px;font-family:''Calibri'',sans-serif;">&nbsp;</span></strong></p>
<p dir="RTL" style="margin:0cm;font-size:16px;font-family:''Times New Roman'',serif;text-align:center;"><strong><span style="font-size:21px;font-family:''Calibri'',sans-serif;">مـــع أطيـــب التمنيـــات ،،،</span></strong></p>
<p dir="RTL" style="margin:0cm;font-size:16px;font-family:''Times New Roman'',serif;text-align:center;"><strong><span style="font-family:''Calibri'',sans-serif;">&nbsp;</span></strong></p>
<p dir="RTL" style="margin:0cm;font-size:16px;font-family:''Times New Roman'',serif;text-align:center;"><strong><span style="font-family:''Calibri'',sans-serif;">&nbsp;</span></strong></p>
<p dir="RTL" style="margin: 0cm;font-size:16px;font-family: ''Times New Roman'', serif;text-align: right;"><strong><span style="font-size:27px;font-family:''Calibri'',sans-serif;">&nbsp; &nbsp;رئيس قطاع قضايا الإداري الكلي</span></strong><strong><span style="font-size:27px;font-family:''Calibri'',sans-serif;">&nbsp;</span></strong></p>
<p dir="RTL" style="margin:0cm;font-size:16px;font-family:''Times New Roman'',serif;text-align:justify;"><strong><span style="font-size:18px;font-family:''Calibri'',sans-serif;">&nbsp;</span></strong></p>
<p dir="RTL" style="margin:0cm;font-size:16px;font-family:''Times New Roman'',serif;text-align:justify;"><strong><span style="font-size:18px;font-family:''Calibri'',sans-serif;">&nbsp;</span></strong></p>
<p dir="RTL" style="margin:0cm;font-size:16px;font-family:''Times New Roman'',serif;text-align:justify;"><strong><span style="font-size:18px;font-family:''Calibri'',sans-serif;">&nbsp;</span></strong></p>
<p dir="RTL" style="margin:0cm;font-size:16px;font-family:''Times New Roman'',serif;text-align:justify;"><strong><span style="font-size:11px;font-family:''Calibri'',sans-serif;">&nbsp;</span></strong></p>
<p dir="RTL" style="margin:0cm;font-size:16px;font-family:''Times New Roman'',serif;text-align:justify;"><strong><span style="font-family:''Calibri'',sans-serif;">&nbsp;</span></strong></p>
<p dir="RTL" style="margin:0cm;font-size:16px;font-family:''Times New Roman'',serif;text-align:justify;"><strong><span style="font-family:''Calibri'',sans-serif;">&nbsp;</span></strong></p>
<p dir="RTL" style="margin:0cm;font-size:16px;font-family:''Times New Roman'',serif;text-align:justify;"><strong><span style="font-family:''Calibri'',sans-serif;">&nbsp;</span></strong></p>
<p dir="RTL" style="margin:0cm;font-size:16px;font-family:''Times New Roman'',serif;text-align:justify;"><strong><span style="font-family:''Calibri'',sans-serif;">&nbsp;</span></strong></p>
<p dir="RTL" style="margin:0cm;font-size:16px;font-family:''Times New Roman'',serif;text-align:justify;"><strong><span style="font-family:''Calibri'',sans-serif;">العضو المختص :&nbsp;#CmsTempLawywerName#</span></strong><strong><span style="font-family:''Calibri'',sans-serif;">&nbsp;</span></strong></p>
<p dir="RTL" style="margin:0cm;font-size:16px;font-family:''Times New Roman'',serif;text-align:justify;"><strong><span style="font-family:''Calibri'',sans-serif;">المرفقات &nbsp;: #CmsTempAttachments#</span></strong></p>
<p dir="RTL" style="margin:0cm;font-size:16px;font-family:''Times New Roman'',serif;text-align:justify;"><strong><span style="font-family:''Calibri'',sans-serif;">&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;</span></strong></p></body></html>'

WHERE Id = '3'


INSERT INTO CMS_TEMPLATE_SECTION values('C7B4BF63-483F-406E-AFE9-F214481B93D3',1,1)
INSERT INTO CMS_TEMPLATE_SECTION (Id,TemplateId, SectionId) VALUES ('5A0EBB36-C1BC-4ABA-BB49-3062F7D6EBCE','3','3')
INSERT INTO CMS_TEMPLATE_SECTION (Id,TemplateId, SectionId) VALUES ('9674D11F-EC34-4DBC-9F78-F7FD041FAF15','3','5')

DELETE FROM CMS_TEMPLATE_SECTION WHERE TemplateId= '2'

INSERT INTO CMS_TEMPLATE_SECTION (Id, TemplateId, SectionId) VALUES ('F36829D4-AF8D-4997-B50D-5F08C0DA4147', 2, 3)
INSERT INTO CMS_TEMPLATE_SECTION (Id, TemplateId, SectionId) VALUES ('E33E441E-750A-41DA-8B16-78EFC2634269', 2, 4)
INSERT INTO CMS_TEMPLATE_SECTION (Id, TemplateId, SectionId) VALUES ('12BD21C9-926F-4169-9149-F718925CA181', 2, 5)

UPDATE PARAMETER SET Name= 'Outbox Date', PKey = 'CmsTempOutboxDate' where ParameterId = '13'
UPDATE PARAMETER SET Name= 'Outbox Number', PKey = 'CmsTempOutboxNumber' where ParameterId = '14'



INSERT INTO CMS_TEMPLATE_SECTION_PARAMETER (TemplateSectionId, ParameterId) VALUES ('5A0EBB36-C1BC-4ABA-BB49-3062F7D6EBCE','13')
INSERT INTO CMS_TEMPLATE_SECTION_PARAMETER (TemplateSectionId, ParameterId) VALUES ('5A0EBB36-C1BC-4ABA-BB49-3062F7D6EBCE','14')
INSERT INTO CMS_TEMPLATE_SECTION_PARAMETER (TemplateSectionId, ParameterId) VALUES ('5A0EBB36-C1BC-4ABA-BB49-3062F7D6EBCE','15')
INSERT INTO CMS_TEMPLATE_SECTION_PARAMETER (TemplateSectionId, ParameterId) VALUES ('5A0EBB36-C1BC-4ABA-BB49-3062F7D6EBCE','31')
INSERT INTO CMS_TEMPLATE_SECTION_PARAMETER (TemplateSectionId, ParameterId) VALUES ('5A0EBB36-C1BC-4ABA-BB49-3062F7D6EBCE','18')
INSERT INTO CMS_TEMPLATE_SECTION_PARAMETER (TemplateSectionId, ParameterId) VALUES ('5A0EBB36-C1BC-4ABA-BB49-3062F7D6EBCE','19')
INSERT INTO CMS_TEMPLATE_SECTION_PARAMETER (TemplateSectionId, ParameterId) VALUES ('5A0EBB36-C1BC-4ABA-BB49-3062F7D6EBCE','20')
INSERT INTO CMS_TEMPLATE_SECTION_PARAMETER (TemplateSectionId, ParameterId) VALUES ('5A0EBB36-C1BC-4ABA-BB49-3062F7D6EBCE','21')
INSERT INTO CMS_TEMPLATE_SECTION_PARAMETER (TemplateSectionId, ParameterId) VALUES ('5A0EBB36-C1BC-4ABA-BB49-3062F7D6EBCE','17')
INSERT INTO CMS_TEMPLATE_SECTION_PARAMETER (TemplateSectionId, ParameterId) VALUES ('5A0EBB36-C1BC-4ABA-BB49-3062F7D6EBCE','16')
INSERT INTO CMS_TEMPLATE_SECTION_PARAMETER (TemplateSectionId, ParameterId) VALUES ('5A0EBB36-C1BC-4ABA-BB49-3062F7D6EBCE','24')
INSERT INTO CMS_TEMPLATE_SECTION_PARAMETER (TemplateSectionId, ParameterId) VALUES ('9674D11F-EC34-4DBC-9F78-F7FD041FAF15','22')
INSERT INTO CMS_TEMPLATE_SECTION_PARAMETER (TemplateSectionId, ParameterId) VALUES ('9674D11F-EC34-4DBC-9F78-F7FD041FAF15','34')
 INSERT INTO CMS_TEMPLATE_SECTION_PARAMETER VALUES('C7B4BF63-483F-406E-AFE9-F214481B93D3',32)



UPDATE CMS_TEMPLATE SET Content = N'#CmsTempNoTemplateContent#'
WHERE Id = '1'


UPDATE MODULE SET ModuleNameEn = 'CMS Case Management', ModuleNameAr = 'CMS Case Management' where ModuleId = '5'

SET IDENTITY_INSERT MODULE_TRIGGER ON
INSERT INTO MODULE_TRIGGER (ModuleTriggerId, Name, ModuleId) VALUES ('4','User Submits Case Draft','5')
SET IDENTITY_INSERT MODULE_TRIGGER OFF

INSERT INTO MODULE_ACTIVITY VALUES('Review Draft Document', 'WorkflowImplementationService', 'Cms_ReviewDraftDocument','5', '2', 'CmsReviewDraftDocument')

INSERT INTO MODULE_ACTIVITY VALUES('HOS-Review Draft Document', 'WorkflowImplementationService', 'Cms_ReviewDraftDocumentHOS','5', '2', 'CmsReviewDraftDocumentHOS')


SET IDENTITY_INSERT PARAMETER ON 
INSERT INTO PARAMETER(ParameterId,Name,PKey,Mandatory,IsAutoPopulated) VALUES ('35','User','CmsReviewDraftDocumentUser',0,0)
INSERT INTO PARAMETER(ParameterId,Name,PKey,Mandatory,IsAutoPopulated) VALUES ('36','User Role','CmsReviewDraftDocumentUserRole',0,0)
SET IDENTITY_INSERT PARAMETER OFF 

SET IDENTITY_INSERT PARAMETER ON 
INSERT INTO PARAMETER(ParameterId,Name,PKey,Mandatory,IsAutoPopulated) VALUES ('37','User','CmsReviewDraftDocumentHosUser',0,0)
INSERT INTO PARAMETER(ParameterId,Name,PKey,Mandatory,IsAutoPopulated) VALUES ('38','User Role','CmsReviewDraftDocumentHosUserRole',0,0)
SET IDENTITY_INSERT PARAMETER OFF 


INSERT INTO MODULE_ACTIVITY_PARAMETERS values('35','8')
INSERT INTO MODULE_ACTIVITY_PARAMETERS values('36','8')

INSERT INTO MODULE_ACTIVITY_PARAMETERS values('37','9')
INSERT INTO MODULE_ACTIVITY_PARAMETERS values('38','9')

INSERT INTO CMS_DRAFT_DOCUMENT_STATUS VALUES(1,'InReview','InReview')
INSERT INTO CMS_DRAFT_DOCUMENT_STATUS VALUES(2,'NeedModification','NeedModification')
INSERT INTO CMS_DRAFT_DOCUMENT_STATUS VALUES(4,'Reject','Reject')
INSERT INTO CMS_DRAFT_DOCUMENT_STATUS VALUES(8,'ApproveBySupervisor','ApproveBySupervisor')
INSERT INTO CMS_DRAFT_DOCUMENT_STATUS VALUES(16,'ApproveByHOS','ApproveByHOS')

INSERT INTO MODULE_CONDITION VALUES(137,5,'If Draft Status is InReview', 'CmsDraftStatusInReview',1)
INSERT INTO MODULE_CONDITION VALUES(138,5,'If Draft Status is NeedModification', 'CmsDraftStatusNeedModification',2)
INSERT INTO MODULE_CONDITION VALUES(139,5,'If Draft Status is Reject', 'CmsDraftStatusReject',4)
INSERT INTO MODULE_CONDITION VALUES(140,5,'If Draft Status is ApproveBySupervisor', 'CmsDraftStatusApproveBySupervisor',8)
INSERT INTO MODULE_CONDITION VALUES(141,5,'If Draft Status is ApproveByHOS', 'CmsDraftStatusApproveByHOS',16)

delete from tTranslation where TranslationKey = 'Required_Field'
EXEC [dbo].pInsTranslation 'Required_Field',N'This is a required field.','This is a required field.','G2G Case Management',1


------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------
--<History Author = 'Hassan Abbas' Date='2022-11-21' Version="1.0" Branch="master"> ABOVE SCRIPT EXECUTED ON FATWA_DB_DEV</History>
------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------


INSERT INTO ATTACHMENT_TYPE VALUES(18,'NOC','NOC',5,1,1)
INSERT [dbo].[ATTACHMENT_TYPE] ([AttachmentTypeId], [Type_Ar], [Type_En], [ModuleId], [IsMandatory]) VALUES (18, N'ClaimStatement', N'ClaimStatement', 5, 1)

------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------
--<History Author = 'Hassan Abbas' Date='2022-11-21' Version="1.0" Branch="master"> ABOVE SCRIPT EXECUTED ON FATWA_DB_DEV</History>
------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------


SET IDENTITY_INSERT MODULE ON

INSERT INTO MODULE (ModuleId, ModuleNameEn, ModuleNameAr) VALUES (8,'MeetingMom','MeetingMom')

SET IDENTITY_INSERT MODULE OFF
------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------
--<History Author = 'Hassan Abbas' Date='2022-11-21' Version="1.0" Branch="master"> ABOVE SCRIPT EXECUTED ON FATWA_DB_DEV</History>
------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------
IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Permissions.Menu.Task')
	INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted])
	VALUES ('Task Menu', 'Task', 'Task', 'Permission', 'Permissions.Menu.Task', 'Task Menu', 0)
Go

IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Permissions.Submenu.Task.Taskdashboard')
	INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted])
	VALUES ('Task Dashboard', 'Task', 'Task', 'Permission', 'Permissions.Submenu.Task.Taskdashboard', 'Task Dashboard', 0)
Go

IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Permissions.Submenu.Task.TaskList')
	INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted])
	VALUES ('Task List', 'Task', 'Task', 'Permission', 'Permissions.Submenu.Task.TaskList', 'Task List', 0)
Go

-- [CMS_REGISTERED_CASE_EVENT_G2G_LKP] 
SET IDENTITY_INSERT [dbo].[CMS_REGISTERED_CASE_EVENT_G2G_LKP] ON 
INSERT INTO[dbo].[CMS_REGISTERED_CASE_EVENT_G2G_LKP] ([Id], [Name_En], [Name_Ar]) VALUES ('3', N'Withdrawn', N'Withdrawn')
INSERT INTO [dbo]. [CMS_REGISTERED_CASE_EVENT_G2G_LKP](Id, Name_En, Name_Ar)VALUES('4','Transfer','Transfer')
INSERT INTO [dbo]. [CMS_REGISTERED_CASE_EVENT_G2G_LKP](Id, Name_En, Name_Ar)VALUES('5','AssignToLawyer','AssignToLawyer')
SET IDENTITY_INSERT [dbo].[CMS_REGISTERED_CASE_EVENT_G2G_LKP] OFF
GO

Go
IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Permissions.Submenu.Task.DraftList')
	INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted])
	VALUES ('Task List', 'Task', 'Task', 'Permission', 'Permissions.Submenu.Task.DraftList', 'Draft List', 0)
Go


INSERT INTO CMS_HEARING_STATUS_G2G_LKP VALUES(1,'Hearing Scheduled','Hearing Scheduled')
INSERT INTO CMS_HEARING_STATUS_G2G_LKP VALUES(2,'Hearing Postponed','Hearing Postponed')
INSERT INTO CMS_HEARING_STATUS_G2G_LKP VALUES(4,'Hearing Attended','Hearing Attended')


SET IDENTITY_INSERT CMS_CASE_REQUEST_EVENT_G2G_LKP ON
INSERT INTO CMS_CASE_REQUEST_EVENT_G2G_LKP (Id,Name_En,Name_Ar) VALUES(6,'SentCopy','SentCopy')
INSERT INTO CMS_CASE_REQUEST_EVENT_G2G_LKP (Id,Name_En,Name_Ar) VALUES(7,'ReceivedCopy','ReceivedCopy')
SET IDENTITY_INSERT CMS_CASE_REQUEST_EVENT_G2G_LKP OFF


UPDATE CMS_REGISTERED_CASE_STATUS_G2G_LKP SET Name_En = 'Open', Name_Ar = 'Open' wherE id = '1'
SET IDENTITY_INSERT CMS_REGISTERED_CASE_STATUS_G2G_LKP ON
INSERT INTO CMS_REGISTERED_CASE_STATUS_G2G_LKP (Id,Name_En,Name_Ar) VALUES(2,'Judgement Received','Judgement Received')
INSERT INTO CMS_REGISTERED_CASE_STATUS_G2G_LKP (Id,Name_En,Name_Ar) VALUES(4,'Hearing Attended','Hearing Attended')
INSERT INTO CMS_REGISTERED_CASE_STATUS_G2G_LKP (Id,Name_En,Name_Ar) VALUES(8,'Closed','Closed')
SET IDENTITY_INSERT CMS_REGISTERED_CASE_STATUS_G2G_LKP OFF


UPDATE CMS_CASE_FILE_STATUS_G2G_LKP SET Name_En = 'Underfiling', Name_Ar = 'Underfiling' wherE id = '1'
SET IDENTITY_INSERT CMS_CASE_FILE_STATUS_G2G_LKP ON
INSERT INTO CMS_CASE_FILE_STATUS_G2G_LKP (Id,Name_En,Name_Ar) VALUES(2,'InProgress','InProgress')
INSERT INTO CMS_CASE_FILE_STATUS_G2G_LKP (Id,Name_En,Name_Ar) VALUES(4,'Sent For Registration','Sent For Registration')
INSERT INTO CMS_CASE_FILE_STATUS_G2G_LKP (Id,Name_En,Name_Ar) VALUES(8,'Registered At MOJ','Registered At MOJ')
SET IDENTITY_INSERT CMS_CASE_FILE_STATUS_G2G_LKP OFF


UPDATE CMS_CASE_FILE_EVENT_G2G_LKP SET Name_En = 'Created', Name_Ar = 'Created' wherE id = '1'
SET IDENTITY_INSERT CMS_CASE_FILE_EVENT_G2G_LKP ON
INSERT INTO CMS_CASE_FILE_EVENT_G2G_LKP (Id,Name_En,Name_Ar) VALUES(2,'Edited','Edited')
INSERT INTO CMS_CASE_FILE_EVENT_G2G_LKP (Id,Name_En,Name_Ar) VALUES(4,'Sent To MOJ Team','Sent To MOJ Team')
INSERT INTO CMS_CASE_FILE_EVENT_G2G_LKP (Id,Name_En,Name_Ar) VALUES(8,'Registered At MOJ','Registered At MOJ')
SET IDENTITY_INSERT CMS_CASE_FILE_EVENT_G2G_LKP OFF


SET IDENTITY_INSERT [CMS_JUDGEMENT_TYPE_G2G_LKP] ON
INSERT INTO [CMS_JUDGEMENT_TYPE_G2G_LKP] ( Id,NameEn,NameAr) VALUES(1,'Against the state (all applications were rejected)','ضد الدولة (تم رفض كل الطلبات)')
INSERT INTO [CMS_JUDGEMENT_TYPE_G2G_LKP] ( Id,NameEn,NameAr) VALUES(2,'In favor of the state',N'لصالح الدولة')
INSERT INTO [CMS_JUDGEMENT_TYPE_G2G_LKP] ( Id,NameEn,NameAr) VALUES(4,'By virtue of part of the requests',N'بحكم جزء من الطلبات')
INSERT INTO [CMS_JUDGEMENT_TYPE_G2G_LKP] ( Id,NameEn,NameAr) VALUES(8,'Judgment in confrontation',N'بحكم في المواجهة')
INSERT INTO [CMS_JUDGEMENT_TYPE_G2G_LKP] ( Id,NameEn,NameAr) VALUES(16,'By virtue of lack of jurisdiction',N'بحكم عدم الاختصاص ولائي')
INSERT INTO [CMS_JUDGEMENT_TYPE_G2G_LKP] ( Id,NameEn,NameAr) VALUES(32,'By virtue of the lack of special jurisdiction',N'بحكم عدم الاختصاص نوعي')
INSERT INTO [CMS_JUDGEMENT_TYPE_G2G_LKP] ( Id,NameEn,NameAr) VALUES(64,'By virtue of my lack of jurisdiction',N'بحكم عدم الاختصاص قيمي')
SET IDENTITY_INSERT [CMS_JUDGEMENT_TYPE_G2G_LKP] OFF
Go

IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Permissions.Submenu.LPS.Principles.Detailsview')
    INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Title_Ar],[Module],[SubModule],[ClaimType],[ClaimValue],[IsDeleted])
        VALUES ('Detailsview Of Legal Principles','Detailsview Of Legal Principles', 'LPS', 'Principles', 'Permission', 'Permissions.Submenu.LPS.Principles.Detailsview',0)
GO 

IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Permissions.CMS.RegisteredCase.RequestedDocuments')
	INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Title_Ar],[Module],[SubModule],[ClaimType],[ClaimValue],[IsDeleted])
		VALUES ('List Requested Documents','List Requested Documents', 'CMS', 'Registered Case', 'Permission', 'Permissions.CMS.RegisteredCase.RequestedDocuments',0)
Go


IF NOT Exists(SELECT 1 from MODULE where ModuleNameEn = 'Task')
	INSERT INTO MODULE (ModuleId, ModuleNameEn, ModuleNameAr) VALUES (9,'Task','Task')

Go

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TSK_TASK_RESPONSE_STATUS]') AND type in (N'U'))
INSERT [dbo].[TSK_TASK_RESPONSE_STATUS] ([TaskResponeStatusId], [NameEn], [NameAr]) VALUES (1, 'In-Progress',N'في تَقَدم')
INSERT [dbo].[TSK_TASK_RESPONSE_STATUS] ([TaskResponeStatusId], [NameEn], [NameAr]) VALUES (2, 'On-Hold',N'في الانتظار')
INSERT [dbo].[TSK_TASK_RESPONSE_STATUS] ([TaskResponeStatusId], [NameEn], [NameAr]) VALUES (4, 'Completed',N'مكتمل')
INSERT [dbo].[TSK_TASK_RESPONSE_STATUS] ([TaskResponeStatusId], [NameEn], [NameAr]) VALUES (8, 'Rejected',N'مرفوض')
Go

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TSK_TASK_STATUS]') AND type in (N'U'))
	INSERT [dbo].[TSK_TASK_STATUS] ([TaskStatusId], [NameEn], [NameAr]) VALUES (1, N'Pending', N'قيد الانتظار')
	INSERT [dbo].[TSK_TASK_STATUS] ([TaskStatusId], [NameEn], [NameAr]) VALUES (2, N'Approved', N'Approved')
	INSERT [dbo].[TSK_TASK_STATUS] ([TaskStatusId], [NameEn], [NameAr]) VALUES (4, N'Rejected', N'Rejected')
	INSERT [dbo].[TSK_TASK_STATUS] ([TaskStatusId], [NameEn], [NameAr]) VALUES (8, N'InProgress', N'في تَقَدم')
	INSERT [dbo].[TSK_TASK_STATUS] ([TaskStatusId], [NameEn], [NameAr]) VALUES (16, N'Done', N'فعله')
Go

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TSK_TASK_SUB_TYPE]') AND type in (N'U'))
	INSERT [dbo].[TSK_TASK_SUB_TYPE] ([SubTypeId], [NameEn], [NameAr]) VALUES (1, N'General Request', N'General Request')
	INSERT [dbo].[TSK_TASK_SUB_TYPE] ([SubTypeId], [NameEn], [NameAr]) VALUES (2, N'Request for Appointment with Medical Council', N'Request for Appointment with Medical Council')
	INSERT [dbo].[TSK_TASK_SUB_TYPE] ([SubTypeId], [NameEn], [NameAr]) VALUES (4, N'Request for Leave ', N'Request for Leave ')
	INSERT [dbo].[TSK_TASK_SUB_TYPE] ([SubTypeId], [NameEn], [NameAr]) VALUES (8, N'Request for Sick Leave', N'Request for Sick Leave')
	INSERT [dbo].[TSK_TASK_SUB_TYPE] ([SubTypeId], [NameEn], [NameAr]) VALUES (16, N'Request for Reducing Working Hours', N'Request for Reducing Working Hours')
	INSERT [dbo].[TSK_TASK_SUB_TYPE] ([SubTypeId], [NameEn], [NameAr]) VALUES (32, N'Request for Fingerprint Exemption', N'Request for Fingerprint Exemption')
	INSERT [dbo].[TSK_TASK_SUB_TYPE] ([SubTypeId], [NameEn], [NameAr]) VALUES (64, N'Submission of Employee Permissions', N'Submission of Employee Permissions')
	INSERT [dbo].[TSK_TASK_SUB_TYPE] ([SubTypeId], [NameEn], [NameAr]) VALUES (128, N'Request for Renewing Residency of Non-Kuwaiti Employees', N'Request for Renewing Residency of Non-Kuwaiti Employees')
	INSERT [dbo].[TSK_TASK_SUB_TYPE] ([SubTypeId], [NameEn], [NameAr]) VALUES (256, N'Request for Issuing benefits for Wife and Children', N'Request for Issuing benefits for Wife and Children')
	INSERT [dbo].[TSK_TASK_SUB_TYPE] ([SubTypeId], [NameEn], [NameAr]) VALUES (512, N'Request for Revoking benefits for Wife and Children', N'Request for Revoking benefits for Wife and Children')
	INSERT [dbo].[TSK_TASK_SUB_TYPE] ([SubTypeId], [NameEn], [NameAr]) VALUES (1024, N'Request for Internal Transfer ', N'NRequest for Internal Transfer ULL')
	INSERT [dbo].[TSK_TASK_SUB_TYPE] ([SubTypeId], [NameEn], [NameAr]) VALUES (2048, N'Request for External Transfer', N'Request for External Transfer')
	INSERT [dbo].[TSK_TASK_SUB_TYPE] ([SubTypeId], [NameEn], [NameAr]) VALUES (4096, N'Request for Special Needs Benefits', N'Request for Special Needs Benefits')
Go

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TSK_TASK_TYPE]') AND type in (N'U'))
	INSERT [dbo].[TSK_TASK_TYPE] ([TypeId], [NameEn], [NameAr]) VALUES (1, N'Task', N'Task Arabic')
	INSERT [dbo].[TSK_TASK_TYPE] ([TypeId], [NameEn], [NameAr]) VALUES (2, N'Request', N'Request Arabic')
	INSERT [dbo].[TSK_TASK_TYPE] ([TypeId], [NameEn], [NameAr]) VALUES (4, N'Assignment', N'Assignment Arabic')
Go


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TSK_TASK_RESPONSE_STATUS') AND type in (N'U'))
INSERT [dbo].[TSK_TASK_RESPONSE_STATUS] ([TaskResponeStatusId], [NameEn], [NameAr]) VALUES (1, 'In-Progress',N'في تَقَدم')
INSERT [dbo].[TSK_TASK_RESPONSE_STATUS] ([TaskResponeStatusId], [NameEn], [NameAr]) VALUES (2, 'On-Hold',N'في الانتظار')
INSERT [dbo].[TSK_TASK_RESPONSE_STATUS] ([TaskResponeStatusId], [NameEn], [NameAr]) VALUES (4, 'Completed',N'مكتمل')
INSERT [dbo].[TSK_TASK_RESPONSE_STATUS] ([TaskResponeStatusId], [NameEn], [NameAr]) VALUES (8, 'Rejected',N'مرفوض')
Go


--CMS_CASE_FILE_STATUS_G2G_LKP

UPDATE CMS_CASE_FILE_STATUS_G2G_LKP SET Name_En = 'Assigned to Lawyer', Name_Ar = 'Assigned to Lawyer'  WHERE Id = '1'
UPDATE CMS_CASE_FILE_STATUS_G2G_LKP SET Name_En = 'Withdraw Requested', Name_Ar = 'Withdraw Requested'  WHERE Id = '2'
UPDATE CMS_CASE_FILE_STATUS_G2G_LKP SET Name_En = 'Withdrawn By GE', Name_Ar = 'Withdrawn By GE'  WHERE Id = '4'
UPDATE CMS_CASE_FILE_STATUS_G2G_LKP SET Name_En = 'Archived', Name_Ar = 'Archived'  WHERE Id = '8'

SET IDENTITY_INSERT CMS_CASE_FILE_STATUS_G2G_LKP ON
INSERT INTO CMS_CASE_FILE_STATUS_G2G_LKP (Id,Name_En,Name_Ar) VALUES(16,'In Progress','In Progress')
INSERT INTO CMS_CASE_FILE_STATUS_G2G_LKP (Id,Name_En,Name_Ar) VALUES(32,'Pending For GE Response','Pending For GE Response')
INSERT INTO CMS_CASE_FILE_STATUS_G2G_LKP (Id,Name_En,Name_Ar) VALUES(64,'Pending For Registration At MOJ','Pending For Registration At MOJ')
INSERT INTO CMS_CASE_FILE_STATUS_G2G_LKP (Id,Name_En,Name_Ar) VALUES(128,'Registered In MOJ','Registered In MOJ')
INSERT INTO CMS_CASE_FILE_STATUS_G2G_LKP (Id,Name_En,Name_Ar) VALUES(256,'Assigned To Regional Sector','Assigned To Regional Sector')
SET IDENTITY_INSERT CMS_CASE_FILE_STATUS_G2G_LKP OFF

--CMS_CASE_REQUEST_STATUS_G2G_LKP


ALTER TABLE CMS_CASE_REQUEST DROP CONSTRAINT CMS_CASE_REQUEST_STATUS
ALTER TABLE CMS_CASE_REQUEST_STATUS_HISTORY DROP CONSTRAINT CMS_CASE_HISTORY_CASE_REQUEST_STATUS

DELETE FROM CMS_CASE_REQUEST_STATUS_G2G_LKP

SET IDENTITY_INSERT CMS_CASE_REQUEST_STATUS_FTW_LKP ON
INSERT INTO CMS_CASE_REQUEST_STATUS_G2G_LKP (Id,Name_En,Name_Ar) VALUES (1,'New','New')
INSERT INTO CMS_CASE_REQUEST_STATUS_G2G_LKP (Id,Name_En,Name_Ar) VALUES (2,'Draft','Draft')
INSERT INTO CMS_CASE_REQUEST_STATUS_G2G_LKP (Id,Name_En,Name_Ar) VALUES (4,'Submitted','Submitted')
INSERT INTO CMS_CASE_REQUEST_STATUS_G2G_LKP (Id,Name_En,Name_Ar) VALUES (8,'Resubmitted','Resubmitted')
INSERT INTO CMS_CASE_REQUEST_STATUS_G2G_LKP (Id,Name_En,Name_Ar) VALUES (16,'Converted To File','Converted To File')
INSERT INTO CMS_CASE_REQUEST_STATUS_G2G_LKP (Id,Name_En,Name_Ar) VALUES (32,'Rejected','Rejected')
INSERT INTO CMS_CASE_REQUEST_STATUS_G2G_LKP (Id,Name_En,Name_Ar) VALUES (64,'Withdraw Requested','Withdraw Requested')
INSERT INTO CMS_CASE_REQUEST_STATUS_G2G_LKP (Id,Name_En,Name_Ar) VALUES (128,'Withdrawn By GE','Withdrawn By GE')
INSERT INTO CMS_CASE_REQUEST_STATUS_G2G_LKP (Id,Name_En,Name_Ar) VALUES (256,'Archive','Archive')
INSERT INTO CMS_CASE_REQUEST_STATUS_G2G_LKP (Id,Name_En,Name_Ar) VALUES (512,'Registered In MOJ','Registered In MOJ')
INSERT INTO CMS_CASE_REQUEST_STATUS_G2G_LKP (Id,Name_En,Name_Ar) VALUES (1024,'Assigned To Regional Sector','Assigned To Regional Sector')
SET IDENTITY_INSERT CMS_CASE_REQUEST_STATUS_FTW_LKP OFF

UPDATE CMS_CASE_REQUEST SET StatusId = '4' WHERE StatusId = '3'
UPDATE CMS_CASE_REQUEST SET StatusId = '16' WHERE StatusId = '6'

ALTER TABLE CMS_CASE_REQUEST ADD CONSTRAINT CMS_CASE_REQUEST_STATUS FOREIGN KEY (StatusId)
REFERENCES CMS_CASE_REQUEST_STATUS_G2G_LKP (Id)

DELETE FROM CMS_CASE_REQUEST_STATUS_HISTORY WHERE StatusId >= 9

UPDATE CMS_CASE_REQUEST_STATUS_HISTORY SET StatusId = '4' WHERE StatusId = '3'
UPDATE CMS_CASE_REQUEST_STATUS_HISTORY SET StatusId = '16' WHERE StatusId = '6'


ALTER TABLE CMS_CASE_REQUEST_STATUS_HISTORY ADD CONSTRAINT CMS_CASE_HISTORY_CASE_REQUEST_STATUS FOREIGN KEY (StatusId)
REFERENCES CMS_CASE_REQUEST_STATUS_G2G_LKP (Id)


SET IDENTITY_INSERT CMS_CASE_REQUEST_EVENT_G2G_LKP ON
INSERT INTO CMS_CASE_REQUEST_EVENT_G2G_LKP (Id,Name_En,Name_Ar) VALUES(8,'Linked','Linked')
SET IDENTITY_INSERT CMS_CASE_REQUEST_EVENT_G2G_LKP OFF

Go

SET IDENTITY_INSERT CMS_REGISTERED_CASE_EVENT_G2G_LKP ON
INSERT INTO CMS_REGISTERED_CASE_EVENT_G2G_LKP (Id,Name_En,Name_Ar) VALUES(6,'Linked','Linked')
SET IDENTITY_INSERT CMS_REGISTERED_CASE_EVENT_G2G_LKP OFF

SET IDENTITY_INSERT CMS_CASE_FILE_EVENT_G2G_LKP ON
INSERT INTO CMS_CASE_FILE_EVENT_G2G_LKP (Id,Name_En,Name_Ar) VALUES(16,'Linked','Linked')
SET IDENTITY_INSERT CMS_CASE_FILE_EVENT_G2G_LKP OFF



SET IDENTITY_INSERT CMS_REQUEST_TYPE_G2G_LKP ON
INSERT INTO CMS_REQUEST_TYPE_G2G_LKP (Id, Code, Name_En, Name_Ar, IsActive) VALUES (1,'ADMN','Administrative','Administrative',1)
INSERT INTO CMS_REQUEST_TYPE_G2G_LKP (Id, Code, Name_En, Name_Ar, IsActive) VALUES (2, 'CC','Civil/Commercial','Civil/Commercial',1)
INSERT INTO CMS_REQUEST_TYPE_G2G_LKP (Id, Code, Name_En, Name_Ar, IsActive) VALUES (4, 'LA','Legal Advice','Legal Advice',1)
INSERT INTO CMS_REQUEST_TYPE_G2G_LKP (Id, Code, Name_En, Name_Ar, IsActive) VALUES (8, 'LEG','Legislations','Legislations',1)
INSERT INTO CMS_REQUEST_TYPE_G2G_LKP (Id, Code, Name_En, Name_Ar, IsActive) VALUES (16, 'AC','Administrative Complaints','Administrative Complaints',1)
INSERT INTO CMS_REQUEST_TYPE_G2G_LKP (Id, Code, Name_En, Name_Ar, IsActive) VALUES (32, 'AC','Contracts','Contracts',1)
INSERT INTO CMS_REQUEST_TYPE_G2G_LKP (Id, Code, Name_En, Name_Ar, IsActive) VALUES (64, 'IA','International Arbitration','International Arbitration',1)
SET IDENTITY_INSERT CMS_REQUEST_TYPE_G2G_LKP OFF


UPDATE CMS_OPERATING_SECTOR_TYPE_G2G_LKP SET Code = 'ADUF', Name_En = 'Administrative Under Filing Cases', Name_Ar = 'Administrative Under Filing Cases', IsActive = 1 WHERE Id = 1
UPDATE CMS_OPERATING_SECTOR_TYPE_G2G_LKP SET Code = 'ADRC', Name_En = 'Administrative Regional Cases', Name_Ar = 'Administrative Regional Cases', IsActive = 1 WHERE Id = 2

SET IDENTITY_INSERT CMS_OPERATING_SECTOR_TYPE_G2G_LKP ON
INSERT INTO CMS_OPERATING_SECTOR_TYPE_G2G_LKP (Id,Code,Name_En,Name_Ar,IsActive) VALUES (4,'ADAC','Administrative Appeal Cases','Administrative Appeal Cases',1)
INSERT INTO CMS_OPERATING_SECTOR_TYPE_G2G_LKP (Id,Code,Name_En,Name_Ar,IsActive) VALUES (8,'ADSC','Administrative Supreme Cases','Administrative Supreme Cases',1)
INSERT INTO CMS_OPERATING_SECTOR_TYPE_G2G_LKP (Id,Code,Name_En,Name_Ar,IsActive) VALUES (16,'CCUF','Civil Commercial Under Filing Cases','Civil Commercial Under Filing Cases',1)
INSERT INTO CMS_OPERATING_SECTOR_TYPE_G2G_LKP (Id,Code,Name_En,Name_Ar,IsActive) VALUES (32,'CCRC','Civil Commercial Regional Cases','Civil Commercial Regional Cases',1)
INSERT INTO CMS_OPERATING_SECTOR_TYPE_G2G_LKP (Id,Code,Name_En,Name_Ar,IsActive) VALUES (64,'CCAC','Civil Commercial Appeal Cases','Civil Commercial Appeal Cases',1)
INSERT INTO CMS_OPERATING_SECTOR_TYPE_G2G_LKP (Id,Code,Name_En,Name_Ar,IsActive) VALUES (128,'CCSC','Civil Commercial Supreme Cases','Civil Commercial Supreme Cases',1)
INSERT INTO CMS_OPERATING_SECTOR_TYPE_G2G_LKP (Id,Code,Name_En,Name_Ar,IsActive) VALUES (256,'CCPU','Civil Commercial Partial/Urgent Cases','Civil Commercial Partial/Urgent Cases',1)
INSERT INTO CMS_OPERATING_SECTOR_TYPE_G2G_LKP (Id,Code,Name_En,Name_Ar,IsActive) VALUES (512,'LL','Legal Advice','Legal Advice',1)
INSERT INTO CMS_OPERATING_SECTOR_TYPE_G2G_LKP (Id,Code,Name_En,Name_Ar,IsActive) VALUES (1024,'LEG','Legislations','Legislations',1)
INSERT INTO CMS_OPERATING_SECTOR_TYPE_G2G_LKP (Id,Code,Name_En,Name_Ar,IsActive) VALUES (2048,'ADC','Administrative Complaints','Administrative Complaints',1)
INSERT INTO CMS_OPERATING_SECTOR_TYPE_G2G_LKP (Id,Code,Name_En,Name_Ar,IsActive) VALUES (4096,'CON','Contracts','Contracts',1)
INSERT INTO CMS_OPERATING_SECTOR_TYPE_G2G_LKP (Id,Code,Name_En,Name_Ar,IsActive) VALUES (8192,'IA','International Arbitration','International Arbitration',1)
SET IDENTITY_INSERT CMS_OPERATING_SECTOR_TYPE_G2G_LKP OFF


IF NOT Exists(SELECT 1 from CMS_CASE_FILE_EVENT_G2G_LKP where Name_En = 'Transfer')
SET IDENTITY_INSERT CMS_CASE_FILE_EVENT_G2G_LKP ON
INSERT INTO CMS_CASE_FILE_EVENT_G2G_LKP (Id,Name_En,Name_Ar) VALUES(32,'Transfer','Transfer')
SET IDENTITY_INSERT CMS_CASE_FILE_EVENT_G2G_LKP OFF
Go


IF NOT Exists(SELECT 1 from CMS_OPERATING_SECTOR_TYPE_G2G_LKP where Name_En = 'Execution')
SET IDENTITY_INSERT CMS_OPERATING_SECTOR_TYPE_G2G_LKP ON
INSERT INTO CMS_OPERATING_SECTOR_TYPE_G2G_LKP (Id,Code,Name_En,Name_Ar,IsActive) VALUES (16384,'EXE','Execution','Execution',1)
SET IDENTITY_INSERT CMS_OPERATING_SECTOR_TYPE_G2G_LKP OFF
Go


IF NOT Exists(SELECT 1 from CMS_CASE_FILE_EVENT_G2G_LKP where Name_En = 'SentCopy')
SET IDENTITY_INSERT CMS_CASE_FILE_EVENT_G2G_LKP ON
INSERT INTO CMS_CASE_FILE_EVENT_G2G_LKP (Id,Name_En,Name_Ar) VALUES(64,'SentCopy','SentCopy')
SET IDENTITY_INSERT CMS_CASE_FILE_EVENT_G2G_LKP OFF
Go

IF NOT Exists(SELECT 1 from CMS_CASE_FILE_EVENT_G2G_LKP where Name_En = 'ReceivedCopy')
SET IDENTITY_INSERT CMS_CASE_FILE_EVENT_G2G_LKP ON
INSERT INTO CMS_CASE_FILE_EVENT_G2G_LKP (Id,Name_En,Name_Ar) VALUES(128,'ReceivedCopy','ReceivedCopy')
SET IDENTITY_INSERT CMS_CASE_FILE_EVENT_G2G_LKP OFF
Go

IF NOT Exists(SELECT 1 from [CMS_CASE_FILE_STATUS_G2G_LKP] where Name_En = 'RejectedByLawyer')
SET IDENTITY_INSERT [dbo].[CMS_CASE_FILE_STATUS_G2G_LKP] ON 
INSERT [dbo].[CMS_CASE_FILE_STATUS_G2G_LKP] ([Id], [Name_En], [Name_Ar]) VALUES (512, N'RejectedByLawyer', N'RejectedByLawyer')
SET IDENTITY_INSERT [dbo].[CMS_CASE_FILE_STATUS_G2G_LKP] OFF
GO

IF NOT Exists(SELECT 1 from CMS_CASE_REQUEST_EVENT_G2G_LKP where Name_En = 'Registered In MOJ')
SET IDENTITY_INSERT CMS_CASE_REQUEST_EVENT_G2G_LKP ON
INSERT INTO CMS_CASE_REQUEST_EVENT_G2G_LKP (Id,Name_En,Name_Ar) VALUES(9,'Registered In MOJ','Registered In MOJ')
SET IDENTITY_INSERT CMS_CASE_REQUEST_EVENT_G2G_LKP OFF
Go


IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Permissions.CMS.CaseFile.Transfer')
	INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted])
	VALUES ('Transfer Case File', 'CMS', 'Case File', 'Permission', 'Permissions.CMS.CaseFile.Transfer', 'Transfer Case File', 0)
Go

IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Permissions.CMS.CaseDraft.DraftDocument')
	INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted])
	VALUES ('Draft Document', 'CMS', 'Case Draft', 'Permission', 'Permissions.CMS.CaseDraft.DraftDocument', 'Draft Document', 0)
Go

IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Permissions.CMS.CaseFile.ViewRequestMoreInfo')
	INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted])
	VALUES ('View Case File Request For More Info', 'CMS', 'Case File', 'Permission', 'Permissions.CMS.CaseFile.ViewRequestMoreInfo', 'View Case File Request For More Info', 0)
Go

IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Permissions.CMS.CaseFile.AsignToSector')
	INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted])
	VALUES ('Assign File To Sector', 'CMS', 'Case File', 'Permission', 'Permissions.CMS.CaseFile.AsignToSector', 'Assign File To Sector', 0)
Go

IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Permissions.CMS.CaseFile.SendCopy')
	INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted])
	VALUES ('Send A Copy of Case File', 'CMS', 'Case File', 'Permission', 'Permissions.CMS.CaseFile.SendCopy', 'Send A Copy of Case File', 0)
Go

IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Permissions.CMS.CaseFile.MeetingRequest')
	INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted])
	VALUES ('Meeting Request For Case File', 'CMS', 'Case File', 'Permission', 'Permissions.CMS.CaseFile.MeetingRequest', 'Meeting Request For Case File', 0)
Go

IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Permissions.CMS.CaseFile.SendToMoj')
	INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted])
	VALUES ('Send File To MOJ', 'CMS', 'Case File', 'Permission', 'Permissions.CMS.CaseFile.SendToMoj', 'Send File To MOJ', 0)
Go

IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Permissions.CMS.CaseFile.AssignToLawyer')
	INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted])
	VALUES ('Assign Case File To Lawyer', 'CMS', 'Case File', 'Permission', 'Permissions.CMS.CaseFile.AssignToLawyer', 'Assign Case File To Lawyer', 0)
Go

IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Permissions.CMS.CaseFile.RequestMoreInfo')
	INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted])
	VALUES ('Request More Info for Case File', 'CMS', 'Case File', 'Permission', 'Permissions.CMS.CaseFile.RequestMoreInfo', 'Request More Info for Case File', 0)
Go

IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Permissions.CMS.CaseRequest.Transfer')
	INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted])
	VALUES ('Transfer Case Request', 'CMS', 'Case Request', 'Permission', 'Permissions.CMS.CaseRequest.Transfer', 'Transfer Case Request', 0)
Go

IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Permissions.CMS.CaseRequest.MeetingRequest')
	INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted])
	VALUES ('Meeting For Case Request', 'CMS', 'Case Request', 'Permission', 'Permissions.CMS.CaseRequest.MeetingRequest', 'Meeting For Case Request', 0)
Go

IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Permissions.CMS.CaseRequest.SendCopy')
	INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted])
	VALUES ('Send A Copy of Case Request', 'CMS', 'Case Request', 'Permission', 'Permissions.CMS.CaseRequest.SendCopy', 'Send A Copy of Case Request', 0)
Go

IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Permissions.CMS.CaseRequest.AssignToLawyer')
	INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted])
	VALUES ('Assign Case Request to Lawyer', 'CMS', 'Case Request', 'Permission', 'Permissions.CMS.CaseRequest.AssignToLawyer', 'Assign Case Request to Lawyer', 0)
Go

IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Permissions.CMS.CaseRequest.RequestMoreInfo')
	INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted])
	VALUES ('Request More Info For Case Request', 'CMS', 'Case Request', 'Permission', 'Permissions.CMS.CaseRequest.RequestMoreInfo', 'Request More Info For Case Request', 0)
Go

IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Permissions.CMS.CaseRequest.ViewRequestMoreInfo')
	INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted])
	VALUES ('View Case Request For More Info', 'CMS', 'Case Request', 'Permission', 'Permissions.CMS.CaseRequest.ViewRequestMoreInfo', 'View Request For More Info', 0)
Go


IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Permissions.Submenu.CMS.MOJ.RegistrationList')
	INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted])
	VALUES ('MOJ Registration List', 'CMS', 'MOJ', 'Permission', 'Permissions.Submenu.CMS.MOJ.RegistrationList', 'MOJ Registration List', 0)
Go

IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Permissions.CMS.MOJ.RegistertoMOJ')
	INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted])
	VALUES ('MOJ Print Document', 'CMS', 'MOJ', 'Permission', 'Permissions.CMS.MOJ.RegistertoMOJ', 'MOJ Print Document', 0)
Go


IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Permissions.CMS.CaseFile.ReviewCaseFileTask')
	INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted])
	VALUES ('Review Case File Assignment Task', 'CMS', 'Case File', 'Permission', 'Permissions.CMS.CaseFile.ReviewCaseFileTask', 'Review Case File Assignment Task', 0)
Go


update CMS_CHAMBER_G2G_LKP set CourtId = 1 where id = 1

IF NOT Exists(SELECT 1 from CMS_HEARING_STATUS_G2G_LKP where NameEn = 'Hearing Added')
INSERT INTO CMS_HEARING_STATUS_G2G_LKP VALUES(8,'Hearing Added','Hearing Added')
Go

IF NOT Exists(SELECT 1 from ATTACHMENT_TYPE where Type_En = 'Execution File')
INSERT INTO ATTACHMENT_TYPE (AttachmentTypeId, Type_Ar, Type_En, ModuleId,IsMandatory)
VALUES ('28','Execution File','Execution File',5,0)
Go
-------------------------------------------------------------------------------------------------------------------------------------------------------------
-------------------------------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------Consultation Start-------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------------------------
-------------COMS_CONSULTATION_CONTRACT_TYPE
SET IDENTITY_INSERT  CMS_SUBTYPE_G2G_LKP ON
INSERT INTO CMS_SUBTYPE_G2G_LKP (Id,RequestTypeId ,Code, Name_En, Name_Ar, IsActive) VALUES (10,'32','AC','Momarasa',N'ممارسة',1)
INSERT INTO CMS_SUBTYPE_G2G_LKP (Id,RequestTypeId ,Code, Name_En, Name_Ar, IsActive) VALUES (11,'32','AC','Tender',N'مناقصة',1)
INSERT INTO CMS_SUBTYPE_G2G_LKP (Id,RequestTypeId ,Code, Name_En, Name_Ar, IsActive) VALUES (12,'32','AC','Bidding',N'مزايدة',1)
INSERT INTO CMS_SUBTYPE_G2G_LKP (Id,RequestTypeId ,Code, Name_En, Name_Ar, IsActive) VALUES (13,'32','AC','Direct Contract',N'تعاقد مباشر',1)
INSERT INTO CMS_SUBTYPE_G2G_LKP (Id,RequestTypeId ,Code, Name_En, Name_Ar, IsActive) VALUES (14,'32','AC','Extension or Renewal',N'تمديد أو تجديد',1)
INSERT INTO CMS_SUBTYPE_G2G_LKP (Id,RequestTypeId ,Code, Name_En, Name_Ar, IsActive) VALUES (15,'32','AC','Design and Build',N'عقود design and build',1)
INSERT INTO CMS_SUBTYPE_G2G_LKP (Id,RequestTypeId ,Code, Name_En, Name_Ar, IsActive) VALUES (16,'32','AC','PPP',N' PPPعقود',1)
SET IDENTITY_INSERT  CMS_SUBTYPE_G2G_LKP OFF
-------------COMS_CONSULTATION_ARTICLE_STATUS
INSERT INTO COMS_CONSULTATION_ARTICLE_STATUS VALUES (1,N'Our Status',N'Our Status')
INSERT INTO COMS_CONSULTATION_ARTICLE_STATUS VALUES (2,N'New',N'New')
INSERT INTO COMS_CONSULTATION_ARTICLE_STATUS VALUES (4,N'Modifiable',N'Modifiable')
INSERT INTO COMS_CONSULTATION_ARTICLE_STATUS VALUES (8,N'Locked',N'Locked')
--------------COMS_CONSULTATION_PARTY_TYPE_FTW_LKP
insert into COMS_CONSULTATION_PARTY_TYPE_G2G_LKP values(1,'Type1','Type1')
insert into COMS_CONSULTATION_PARTY_TYPE_G2G_LKP values(2,'Type2','Type2')
--------------COMS_CONSULTATION_SECTION
insert into COMS_CONSULTATION_SECTION values(NEWID(),'D23B92E9-F032-40B6-938A-48CA7374A69A','00000000-0000-0000-0000-000000000000',1,0,'Section 1','')
--------------COMS_CONSULTATION_ARTICLE
insert into COMS_CONSULTATION_ARTICLE values(NEWID(),'D23B92E9-F032-40B6-938A-48CA7374A69A','361B710E-1B47-47D1-8D62-C1E5FF884100',1,'Article 1',2,'Article has the text')

----COMS_CONSULTATION_REQUEST
 insert into COMS_CONSULTATION_REQUEST 
values(NEWID(),'CO1',64,2,GETDATE(),'Consultation Contract','Consultation Management Menu','introduction of contract',NULL, NULL ,
'Contract','Opinion GE',1,1,1,2,4,'Remarks of contract',1,1,
'2022OT1','2022IN1',NULL,'fatwaadmin@gmail.com',GETDATE(),NULL,NULL,NULL,NULL,0)
-------COMS_CONSULTATION_REQUEST
insert into COMS_CONSULTATION_REQUEST 
values(NEWID(),'CO4',16,2,GETDATE(),'Consultation Contract','Consultation Managementsadasd Menu','introduction of sdascontract',NULL, NULL ,
'Contract','Opinion GE',1,1,1,1,2,'Remarks of contract',1,1,
'2022OT2','2022IN2',NULL,'fatwaadmin@gmail.com',GETDATE(),NULL,NULL,NULL,NULL,0)
-------COMS_CONSULTATION_REQUEST
insert into COMS_CONSULTATION_REQUEST 
values(NEWID(),'CO1',32,2,GETDATE(),'Consultation Contract','Consultation Management Menu','introduction of contract',NULL, NULL ,
'Contract','Opinion GE',1,1,1,2,4,'Remarks of contract',1,1,
'2022OT1','2022IN1',NULL,'fatwaadmin@gmail.com',GETDATE(),NULL,NULL,NULL,NULL,0)
-------COMS_CONSULTATION_REQUEST
insert into COMS_CONSULTATION_REQUEST 
values(NEWID(),'CO4',32,2,GETDATE(),'Consultation Contract','Consultation Managementsadasd Menu','introduction of sdascontract',NULL, NULL ,
'Contract','Opinion GE',1,1,1,1,2,'Remarks of contract',1,1,
'2022OT2','2022IN2',NULL,'fatwaadmin@gmail.com',GETDATE(),NULL,NULL,NULL,NULL,0)
-----------COMS_CONSULTATION_REQUEST
insert into COMS_CONSULTATION_REQUEST 
values(NEWID(),'CO5',16,2,GETDATE(),'Consultation Contract','Consultation Managementsadasd Menu','introduction of sdascontract',NULL, NULL ,
'Contract','Opinion GE',1,1,1,1,2,'Remarks of contract',1,1,
'2022OT2','2022IN2',NULL,'fatwaadmin@gmail.com',GETDATE(),NULL,NULL,NULL,NULL,0,NULL,NULL,NULL,NULL,NULL,NULL,NULL,512)
-----------COMS_CONSULTATION_REQUEST
insert into COMS_CONSULTATION_REQUEST 
values(NEWID(),'CO5',16,2,GETDATE(),'Consultation Contract','Consultation Managementsadasd Menu','introduction of sdascontract',NULL, NULL ,
'Contract','Opinion GE',1,1,1,1,2,'Remarks of contract',1,1,
'2022OT2','2022IN2',NULL,'fatwaadmin@gmail.com',GETDATE(),NULL,NULL,NULL,NULL,0,NULL,NULL,NULL,NULL,NULL,NULL,NULL,131072)
-----------COMS_CONSULTATION_REQUEST
insert into COMS_CONSULTATION_REQUEST 
values(NEWID(),'CO7',16,2,GETDATE(),'Consultation Contract','Consultation Managementsadasd Menu','introduction of sdascontract',NULL, NULL ,
'Contract','Opinion GE',1,1,1,1,2,'Remarks of contract',1,1,
'2022OT2','2022IN2',NULL,'fatwaadmin@gmail.com',GETDATE(),NULL,NULL,NULL,NULL,0,NULL,NULL,NULL,NULL,NULL,NULL,NULL,262144)
-----------COMS_CONSULTATION_REQUEST
insert into COMS_CONSULTATION_REQUEST 
values(NEWID(),'CO6',16,2,GETDATE(),'Consultation Contract','Consultation Managementsadasd Menu','introduction of sdascontract',NULL, NULL ,
'Contract','Opinion GE',1,1,1,1,2,'Remarks of contract',1,1,
'2022OT2','2022IN2',NULL,'fatwaadmin@gmail.com',GETDATE(),NULL,NULL,NULL,NULL,0,NULL,NULL,NULL,NULL,NULL,NULL,NULL,524288)
insert into COMS_CONSULTATION_REQUEST 
values(NEWID(),'CO11',32,2,GETDATE(),'Consultation Contract','Consultation Managementsadasd Menu','introduction of sdascontract',NULL, NULL ,
'Contract','Opinion GE',1,1,1,1,2,'Remarks of contract',1,1,
'2022OT2','2022IN2',NULL,'fatwaadmin@gmail.com',GETDATE(),NULL,NULL,NULL,NULL,0,NULL,NULL,NULL,NULL,NULL,NULL,NULL,512)
-----------COMS_CONSULTATION_REQUEST
insert into COMS_CONSULTATION_REQUEST 
values(NEWID(),'CO10',32,2,GETDATE(),'Consultation Contract','Consultation Managementsadasd Menu','introduction of sdascontract',NULL, NULL ,
'Contract','Opinion GE',1,1,1,1,2,'Remarks of contract',1,1,
'2022OT2','2022IN2',NULL,'fatwaadmin@gmail.com',GETDATE(),NULL,NULL,NULL,NULL,0,NULL,NULL,NULL,NULL,NULL,NULL,NULL,131072)
-----------COMS_CONSULTATION_REQUEST
insert into COMS_CONSULTATION_REQUEST 
values(NEWID(),'CO9',32,2,GETDATE(),'Consultation Contract','Consultation Managementsadasd Menu','introduction of sdascontract',NULL, NULL ,
'Contract','Opinion GE',1,1,1,1,2,'Remarks of contract',1,1,
'2022OT2','2022IN2',NULL,'fatwaadmin@gmail.com',GETDATE(),NULL,NULL,NULL,NULL,0,NULL,NULL,NULL,NULL,NULL,NULL,NULL,262144)
-----------COMS_CONSULTATION_REQUEST
insert into COMS_CONSULTATION_REQUEST 
values(NEWID(),'CO8',32,2,GETDATE(),'Consultation Contract','Consultation Managementsadasd Menu','introduction of sdascontract',NULL, NULL ,
'Contract','Opinion GE',1,1,1,1,2,'Remarks of contract',1,1,
'2022OT2','2022IN2',NULL,'fatwaadmin@gmail.com',GETDATE(),NULL,NULL,NULL,NULL,0,NULL,NULL,NULL,NULL,NULL,NULL,NULL,524288)
insert into COMS_CONSULTATION_REQUEST 
values(NEWID(),'CO12',32,2,GETDATE(),'Consultation Contract','Consultation Managementsadasd Menu','introduction of sdascontract',NULL, NULL ,
'Contract','Opinion GE',1,1,1,1,2,'Remarks of contract',1,1,
'2022OT2','2022IN2',NULL,'fatwaadmin@gmail.com',GETDATE(),NULL,NULL,NULL,NULL,0,NULL,NULL,NULL,NULL,NULL,NULL,NULL,262144)
insert into COMS_CONSULTATION_REQUEST 
values(NEWID(),'CO13',32,2,GETDATE(),'Consultation Contract','Consultation Managementsadasd Menu','introduction of sdascontract',NULL, NULL ,
'Contract','Opinion GE',1,1,1,1,2,'Remarks of contract',1,1,
'2022OT2','2022IN2',NULL,'fatwaadmin@gmail.com',GETDATE(),NULL,NULL,NULL,NULL,0,NULL,NULL,NULL,NULL,NULL,NULL,NULL,262144)
insert into COMS_CONSULTATION_REQUEST 
values(NEWID(),'CO14',32,2,GETDATE(),'Consultation Contract','Consultation Managementsadasd Menu','introduction of sdascontract',NULL, NULL ,
'Contract','Opinion GE',1,1,1,1,2,'Remarks of contract',1,1,
'2022OT2','2022IN2',NULL,'fatwaadmin@gmail.com',GETDATE(),NULL,NULL,NULL,NULL,0,NULL,NULL,NULL,NULL,NULL,NULL,NULL,524288)
insert into COMS_CONSULTATION_REQUEST 
values(NEWID(),'CO15',32,2,GETDATE(),'Consultation Contract','Consultation Managementsadasd Menu','introduction of sdascontract',NULL, NULL ,
'Contract','Opinion GE',1,1,1,1,2,'Remarks of contract',1,1,
'2022OT2','2022IN2',NULL,'fatwaadmin@gmail.com',GETDATE(),NULL,NULL,NULL,NULL,0,NULL,NULL,NULL,NULL,NULL,NULL,NULL,524288)

-------UMS_CLAIMS
insert into UMS_CLAIM values('Consultation Request List','COMS','Consultation Request','Permission','Permissions.Submenu.COMS.Request.List','Consultation Request List',0)
insert into UMS_ROLE_CLAIMS values('d6b3075c-3f70-4b44-baa4-1fdc599a6bb2','Permission','Permissions.Submenu.COMS.Request.List')
insert into UMS_USER_CLAIMS values('436e82d2-70d8-455c-a643-7909b8689667','Permission','Permissions.Submenu.COMS.Request.List')
insert into UMS_ROLE_CLAIMS values('d6b3075c-3f70-4b44-baa4-1fdc599a6bb2','Permission','Permissions.Menu.COMS')
insert into UMS_USER_CLAIMS values('315c5e0e-99be-4bc3-b102-32a1121a695b','Permission','Permissions.Submenu.COMS.Request.List')
insert into UMS_USER_CLAIMS values('fb0a6ea8-cc89-4f37-aef0-24aa6e318fee','Permission','Permissions.Submenu.COMS.Request.List')

insert into UMS_ROLE_CLAIMS values('f2c87c20-3a38-4a20-b238-ec643ebd0df9','Permission','Permissions.Submenu.COMS.Request.List')
insert into UMS_ROLE_CLAIMS values('93e5374b-cbd9-494e-92d4-d9d7d44c2c39','Permission','Permissions.Submenu.COMS.Request.List')
insert into UMS_ROLE_CLAIMS values('d6b3075c-3f70-4b44-baa4-1fdc599a6bb2','Permission','Permissions.Submenu.COMS.Request.List')

-------------------------------------------------------------------------------------------------------------------------------------------------------------
-------------------------------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------Consultation End-------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------------------------


--CMS_CASE_REQUEST_STATUS_G2G_LKP

ALTER TABLE CMS_CASE_REQUEST DROP CONSTRAINT CMS_CASE_REQUEST_STATUS
ALTER TABLE CMS_CASE_REQUEST_STATUS_HISTORY DROP CONSTRAINT CMS_CASE_HISTORY_CASE_REQUEST_STATUS

DELETE FROM CMS_CASE_REQUEST_STATUS_G2G_LKP

SET IDENTITY_INSERT CMS_CASE_REQUEST_STATUS_G2G_LKP ON
INSERT INTO CMS_CASE_REQUEST_STATUS_G2G_LKP (Id,Name_En,Name_Ar) VALUES (1,'Draft','Draft')
INSERT INTO CMS_CASE_REQUEST_STATUS_G2G_LKP (Id,Name_En,Name_Ar) VALUES (2,'Submitted','Submitted')
INSERT INTO CMS_CASE_REQUEST_STATUS_G2G_LKP (Id,Name_En,Name_Ar) VALUES (4,'Resubmitted','Resubmitted')
INSERT INTO CMS_CASE_REQUEST_STATUS_G2G_LKP (Id,Name_En,Name_Ar) VALUES (8,'Converted To File','Converted To File')
INSERT INTO CMS_CASE_REQUEST_STATUS_G2G_LKP (Id,Name_En,Name_Ar) VALUES (16,'Rejected','Rejected')
INSERT INTO CMS_CASE_REQUEST_STATUS_G2G_LKP (Id,Name_En,Name_Ar) VALUES (32,'Withdraw Requested','Withdraw Requested')
INSERT INTO CMS_CASE_REQUEST_STATUS_G2G_LKP (Id,Name_En,Name_Ar) VALUES (64,'Withdrawn By GE','Withdrawn By GE')
INSERT INTO CMS_CASE_REQUEST_STATUS_G2G_LKP (Id,Name_En,Name_Ar) VALUES (128,'Archive','Archive')
INSERT INTO CMS_CASE_REQUEST_STATUS_G2G_LKP (Id,Name_En,Name_Ar) VALUES (256,'Registered In MOJ','Registered In MOJ')
INSERT INTO CMS_CASE_REQUEST_STATUS_G2G_LKP (Id,Name_En,Name_Ar) VALUES (512,'Assigned To Regional Sector','Assigned To Regional Sector')
SET IDENTITY_INSERT CMS_CASE_REQUEST_STATUS_G2G_LKP OFF


UPDATE CMS_CASE_REQUEST SET StatusId = '2' WHERE StatusId = '4'
UPDATE CMS_CASE_REQUEST SET StatusId = '8' WHERE StatusId = '16'
UPDATE CMS_CASE_REQUEST SET StatusId = '256' WHERE StatusId = '512'

ALTER TABLE CMS_CASE_REQUEST ADD CONSTRAINT CMS_CASE_REQUEST_STATUS FOREIGN KEY (StatusId)
REFERENCES CMS_CASE_REQUEST_STATUS_G2G_LKP (Id)

UPDATE CMS_CASE_REQUEST SET StatusId = '1' WHERE StatusId = '2'
UPDATE CMS_CASE_REQUEST SET StatusId = '2' WHERE StatusId = '4'
UPDATE CMS_CASE_REQUEST SET StatusId = '8' WHERE StatusId = '16'
UPDATE CMS_CASE_REQUEST SET StatusId = '256' WHERE StatusId = '512'

ALTER TABLE CMS_CASE_REQUEST_STATUS_HISTORY ADD CONSTRAINT CMS_CASE_HISTORY_CASE_REQUEST_STATUS FOREIGN KEY (StatusId)
REFERENCES CMS_CASE_REQUEST_STATUS_G2G_LKP (Id)

IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Permissions.CMS.RegisteredCase.ReviewCaseTask')
	INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted])
	VALUES ('Review Case Assignment Task', 'CMS', 'Registered Case', 'Permission', 'Permissions.CMS.RegisteredCase.ReviewCaseTask', 'Review Case Assignment Task', 0)
Go


UPDATE MODULE_ACTIVITY SET Name = 'SUPV-Review Draft Document' where Name = 'Review Draft Document'
update WORKFLOW_ACTIVITY_PARAMETERS set Value = '' where WorkflowActivityId = 81 and ParameterId = 35
update WORKFLOW_ACTIVITY_PARAMETERS set Value = '' where WorkflowActivityId = 81 and ParameterId = 36
update WORKFLOW_ACTIVITY_PARAMETERS set Value = '' where WorkflowActivityId = 82 and ParameterId = 37
update WORKFLOW_ACTIVITY_PARAMETERS set Value = '' where WorkflowActivityId = 82 and ParameterId = 38

UPDATE ATTACHMENT_TYPE set Type_En='MOCI Certificate',  Type_Ar='MOCI Certificate' where AttachmentTypeId=15
UPDATE ATTACHMENT_TYPE set Type_En='Claim Statement',  Type_Ar='Claim Statement' where AttachmentTypeId=18


IF NOT Exists(SELECT 1 from CMS_WITHDRAW_REQUEST_STATUS_FTW_LKP where Name_En = 'Withdraw Case Request')
	INSERT INTO [dbo].[CMS_WITHDRAW_REQUEST_STATUS_FTW_LKP] ([Name_En],[Name_Ar]) VALUES('Withdraw Case Request','Withdraw Case Request')
Go
  IF NOT Exists(SELECT 1 from CMS_WITHDRAW_REQUEST_STATUS_G2G_LKP where Name_En = 'Withdraw Case Request')
	INSERT INTO [dbo].[CMS_WITHDRAW_REQUEST_STATUS_G2G_LKP] ([Name_En],[Name_Ar]) VALUES('Request WithDraw','Request WithDraw')
Go
  ------------------------------------------------CMS_OPERATING_SECTOR_TYPE_G2G_LKP

            delete from CMS_OPERATING_SECTOR_TYPE_G2G_LKP where Id ='512'
			delete from CMS_OPERATING_SECTOR_TYPE_G2G_LKP where Id ='1024'
			delete from CMS_OPERATING_SECTOR_TYPE_G2G_LKP where Id ='2048'
			delete from CMS_OPERATING_SECTOR_TYPE_G2G_LKP where Id ='4096'
			delete from CMS_OPERATING_SECTOR_TYPE_G2G_LKP where Id ='8192'
------------------------------------------------CMS_OPERATING_SECTOR_TYPE_G2G_LKP
  SET IDENTITY_INSERT  CMS_OPERATING_SECTOR_TYPE_G2G_LKP ON
INSERT [dbo].[CMS_OPERATING_SECTOR_TYPE_G2G_LKP]([Id], [Code], [Name_En], [Name_Ar], [IsActive])  VALUES (512,'LAHA','Legal Advice Housing Allowance','Legal Advice Housing Allowance',1)
INSERT [dbo].[CMS_OPERATING_SECTOR_TYPE_G2G_LKP]([Id], [Code], [Name_En], [Name_Ar], [IsActive])  VALUES (1024,'LAE','Legal Advice Employee','Legal Advice Employee',1)
INSERT [dbo].[CMS_OPERATING_SECTOR_TYPE_G2G_LKP]([Id], [Code], [Name_En], [Name_Ar], [IsActive])  VALUES (2048,'LAM','Legal Advice Ministry','Legal Advice Ministry',1)
INSERT [dbo].[CMS_OPERATING_SECTOR_TYPE_G2G_LKP]([Id], [Code], [Name_En], [Name_Ar], [IsActive])  VALUES (4096,'LAJ','Legal Advice Judgment','Legal Advice Judgment',1)
INSERT [dbo].[CMS_OPERATING_SECTOR_TYPE_G2G_LKP]([Id], [Code], [Name_En], [Name_Ar], [IsActive])  VALUES (8192,'LAA','Legal Advice Authorities','Legal Advice Authorities',1)
INSERT [dbo].[CMS_OPERATING_SECTOR_TYPE_G2G_LKP]([Id], [Code], [Name_En], [Name_Ar], [IsActive])  VALUES (16384,'LAI','Legal Advice Investigations','Legal Advice Investigations',1)
INSERT [dbo].[CMS_OPERATING_SECTOR_TYPE_G2G_LKP]([Id], [Code], [Name_En], [Name_Ar], [IsActive])  VALUES (32768,'LARC','Legal Advice Residential Care','Legal Advice Residential Care',1)
INSERT [dbo].[CMS_OPERATING_SECTOR_TYPE_G2G_LKP]([Id], [Code], [Name_En], [Name_Ar], [IsActive])  VALUES (65536,'LASC','Legal Advice Sub Classification','Legal Advice Sub Classification',1)
INSERT [dbo].[CMS_OPERATING_SECTOR_TYPE_G2G_LKP]([Id], [Code], [Name_En], [Name_Ar], [IsActive])  VALUES (131072,'CM','Contracts Momarasa','Contracts Momarasa',1)
INSERT [dbo].[CMS_OPERATING_SECTOR_TYPE_G2G_LKP]([Id], [Code], [Name_En], [Name_Ar], [IsActive])  VALUES (262144,'CT','Contracts Tender','Contracts Tender',1)
INSERT [dbo].[CMS_OPERATING_SECTOR_TYPE_G2G_LKP]([Id], [Code], [Name_En], [Name_Ar], [IsActive])  VALUES (524288,'CB','Contracts Bidding','Contracts Bidding',1)
INSERT [dbo].[CMS_OPERATING_SECTOR_TYPE_G2G_LKP]([Id], [Code], [Name_En], [Name_Ar], [IsActive])  VALUES (1048576,'CDC','Contracts Direct Contract','Contracts Direct Contract',1)
INSERT [dbo].[CMS_OPERATING_SECTOR_TYPE_G2G_LKP]([Id], [Code], [Name_En], [Name_Ar], [IsActive])  VALUES (2097152,'CER','Contracts Extension/Renewal','Contracts Extension/Renewal',1)
INSERT [dbo].[CMS_OPERATING_SECTOR_TYPE_G2G_LKP]([Id], [Code], [Name_En], [Name_Ar], [IsActive])  VALUES (4194304,'CDB','Contracts Design/Build','Contracts Design/Build',1)
INSERT [dbo].[CMS_OPERATING_SECTOR_TYPE_G2G_LKP]([Id], [Code], [Name_En], [Name_Ar], [IsActive])  VALUES (8388608,'CPP','Contracts PPP','Contracts PPP',1)
INSERT [dbo].[CMS_OPERATING_SECTOR_TYPE_G2G_LKP]([Id], [Code], [Name_En], [Name_Ar], [IsActive])  VALUES (16777216,'LG','Legislations','Legislations',1)
INSERT [dbo].[CMS_OPERATING_SECTOR_TYPE_G2G_LKP]([Id], [Code], [Name_En], [Name_Ar], [IsActive])  VALUES (33554432,'AC','Administrative Complaints','Administrative Complaints',1)
INSERT [dbo].[CMS_OPERATING_SECTOR_TYPE_G2G_LKP]([Id], [Code], [Name_En], [Name_Ar], [IsActive])  VALUES (16777216,'LG','International Arbitration','International Arbitration',1)
SET IDENTITY_INSERT  CMS_OPERATING_SECTOR_TYPE_G2G_LKP OFF
GO
 ---------------------------------UMS_CLAIM
 Go
IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Permissions.Menu.COMS.LegalAdvice')
INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted])
 VALUES ('Consultation Legal Advice', 'COMS', 'Consultation Request', 'Permission', 'Permissions.Menu.COMS.LegalAdvice', 'abc',0)
Go
                                                     -----------------------
Go
IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Permissions.Menu.COMS.InternationalArbitration')
INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted])
 VALUES ('Consultation International Arbitration', 'COMS', 'Consultation Request', 'Permission', 'Permissions.Menu.COMS.InternationalArbitration', 'abc',0)
Go
													-----------------------
Go
IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Permissions.Menu.COMS.Legislations')
INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted])
 VALUES ('Consultation Legislations', 'COMS', 'Consultation Request', 'Permission', 'Permissions.Menu.COMS.Legislations', 'abc',0)
Go
													-----------------------
Go
IF NOT Exists(SELECT 1 from UMS_CLAIM where ClaimValue = 'Permissions.Menu.COMS.AdministrativeComplaints')
INSERT INTO [dbo].[UMS_CLAIM] ([Title_En],[Module],[SubModule],[ClaimType],[ClaimValue],[Title_Ar],[IsDeleted])
 VALUES ('Consultation Administrative Complaints', 'COMS', 'Consultation Request', 'Permission', 'Permissions.Menu.COMS.AdministrativeComplaints', 'abc',0)
Go
-------------------------------LMS_LITERATURE_BORROW_APPROVAL_STATUS
UPDATE LMS_LITERATURE_BORROW_APPROVAL_STATUS
SET Name_Ar = N'رفض'
WHERE  Name = 'Reject'
UPDATE LMS_LITERATURE_BORROW_APPROVAL_STATUS
SET Name_Ar = N'اعتماد'
WHERE  Name = 'Approved'
UPDATE LMS_LITERATURE_BORROW_APPROVAL_STATUS
SET Name_Ar = N'منتهي الصلاحية'
WHERE  Name = 'Expired'
UPDATE LMS_LITERATURE_BORROW_APPROVAL_STATUS
SET Name_Ar = N'في انتظار لموافقة'
WHERE  Name = 'Pending For Approval'
UPDATE LMS_LITERATURE_BORROW_APPROVAL_STATUS
SET Name_Ar = N'مستعار'
WHERE  Name = 'Borrowed'
UPDATE LMS_LITERATURE_BORROW_APPROVAL_STATUS
SET Name_Ar = N'عاد'
WHERE  Name = 'Returned'
Go
------------------------------------
Go 
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CMS_Response_Type]') AND type in (N'U'))
	INSERT INTO [dbo].[CMS_Response_Type]([Name_En],[Name_Ar])VALUES('Need More Information', N'Need More Information') 
	INSERT INTO [dbo].[CMS_Response_Type]([Name_En],[Name_Ar])VALUES('Save & Close Case File', N' حفظ وإغلاق ملف القضية')
Go

--legal legislation status arabic change 

UPDATE tTranslation
SET Value_Ar = N'نعم', Value_En='Yes'
WHERE TranslationId=80;

UPDATE tTranslation
SET Value_Ar = N'إلغاء', Value_En='Cancel'
WHERE TranslationId=247;

--Translation Update

UPDATE LEGAL_LEGISLATION_STATUS
SET Name_Ar = N'ساري'
WHERE Id=1;

UPDATE LEGAL_LEGISLATION_STATUS
SET Name_Ar = N'ملغي'
WHERE Id=2;
UPDATE LEGAL_LEGISLATION_STATUS
SET Name_Ar = N'معدل'
WHERE Id=4;

UPDATE tTranslation
SET Value_Ar = N'طباعة', Value_En='Print All'
WHERE TranslationId=3045;

------------------- Legal Template Setting Value -----------------------

UPDATE LEGAL_TEMPLATE_SETTING
SET Template_Heading_Ar = N'العنوان الرئيسي'
WHERE  Template_Heading = 'Main Title'
UPDATE LEGAL_TEMPLATE_SETTING
SET Template_Heading_Ar = N'مقدمة'
WHERE  Template_Heading = 'Introduction'
UPDATE LEGAL_TEMPLATE_SETTING
SET Template_Heading_Ar = N'النشر'
WHERE  Template_Heading = 'Publication'
UPDATE LEGAL_TEMPLATE_SETTING
SET Template_Heading_Ar = N'هيئة التشريع'
WHERE  Template_Heading = 'Legislation Body'
UPDATE LEGAL_TEMPLATE_SETTING
SET Template_Heading_Ar = N'ملاحظة توضيحية'
WHERE  Template_Heading = 'Explanatory Note'
UPDATE LEGAL_TEMPLATE_SETTING
SET Template_Heading_Ar = N'ملحوظة'
WHERE  Template_Heading = 'Note'
Go


------------------------------

UPDATE LEGAL_TEMPLATE_SETTING
SET Template_Value_Ar = N'رقم التشريع'
WHERE  Template_Value = 'Legislation Number'
UPDATE LEGAL_TEMPLATE_SETTING
SET Template_Value_Ar = N'تاريخ اصدار التشريع'
WHERE  Template_Value = 'Legislation Issue Date'
UPDATE LEGAL_TEMPLATE_SETTING
SET Template_Value_Ar = N'تاريخ بدء التشريع'
WHERE  Template_Value = 'Legislation Start Date'
UPDATE LEGAL_TEMPLATE_SETTING
SET Template_Value_Ar = N'موضوع التشريع'
WHERE  Template_Value = 'Legislation Subject'
UPDATE LEGAL_TEMPLATE_SETTING
SET Template_Value_Ar = N'مقدمة مع العلاقة'
WHERE  Template_Value = 'Introduction with relation'
UPDATE LEGAL_TEMPLATE_SETTING
SET Template_Value_Ar = N'مقدمة بدون علاقة'
WHERE  Template_Value = 'Introduction without relation'
UPDATE LEGAL_TEMPLATE_SETTING
SET Template_Value_Ar = N'تفاصيل المنشور'
WHERE  Template_Value = 'Publication details'
UPDATE LEGAL_TEMPLATE_SETTING
SET Template_Value_Ar = N'مقالات مع أقسام'
WHERE  Template_Value = 'Articles with sections'
UPDATE LEGAL_TEMPLATE_SETTING
SET Template_Value_Ar = N'مقالات بدون أقسام'
WHERE  Template_Value = 'Articles without sections'
UPDATE LEGAL_TEMPLATE_SETTING
SET Template_Value_Ar = N'بنود مع أقسام'
WHERE  Template_Value = 'Clauses with sections'
UPDATE LEGAL_TEMPLATE_SETTING
SET Template_Value_Ar = N'بنود بدون أقسام'
WHERE  Template_Value = 'Clauses without sections'
UPDATE LEGAL_TEMPLATE_SETTING
SET Template_Value_Ar = N'ملاحظة توضيحية'
WHERE  Template_Value = 'Explanatory Note'
UPDATE LEGAL_TEMPLATE_SETTING
SET Template_Value_Ar = N'ملحوظة'
WHERE  Template_Value = 'Note'
Go