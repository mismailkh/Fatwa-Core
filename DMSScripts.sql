

IF((SELECT COUNT(*) FROM DMS_ADDED_DOCUMENT_STATUS_LKP WHERE Id = 16) = 0)
INSERT INTO DMS_ADDED_DOCUMENT_STATUS_LKP VALUES(16, N'Published', 'Published')
GO


EXEC [dbo].pInsTranslation 'Template_List',N'Template List','Template List','Document Template List',1
EXEC [dbo].pInsTranslation 'Is_Active',N'IsActive','IsActive','Document Template List',1
EXEC [dbo].pInsTranslation 'Must_Select_Module',N'Please Select Module','Please Select Module','DMS',1


INSERT INTO ATTACHMENT_TYPE (AttachmentTypeId, Type_Ar, Type_En,ModuleId,IsMandatory,IsOfficialLetter,SubTypeId,IsGePortalType)
VALUES (73, N'طلب اجتماع ','Request For Meeting', 7, 1, 1, 0, 1)


/*<History Author='Umer Zaman' Date='24-12-2023'> Meeting translation start </History>*/
INSERT INTO ATTACHMENT_TYPE VALUES(89, N'آحرون', 'Others', 7, 0, 0, 0, 1, 0, 'fatwaadmin@gmail.com', GETDATE(), '', '', '', '', 0, 1, 1, '')
/*<History Author='Umer Zaman' Date='24-12-2023'> Meeting translation end </History>*/

/*<History Author='Umer Zaman' Date='22-12-2023'> Meeting translation start </History>*/
UPDATE ATTACHMENT_TYPE SET Type_Ar = N'أخرى', Type_En = 'Other' WHERE AttachmentTypeId = 89
UPDATE ATTACHMENT_TYPE SET Type_Ar = N'محضر وثيقة الإجتماع', Type_En = 'MOM Document', IsMandatory = 1, IsOfficialLetter = 1, SubTypeId = 0 WHERE AttachmentTypeId = 16
UPDATE ATTACHMENT_TYPE SET Type_Ar = N'توقيع محضر وثيقة الاجتماع', Type_En = 'Signed MOM Document', SubTypeId = 0 WHERE AttachmentTypeId = 17
/*<History Author='Umer Zaman' Date='22-12-2023'> Meeting translation end </History>*/

-----DMS LIST Procedure 

IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pAttachmentTypelist]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
  DROP PROCEDURE [dbo].[pAttachmentTypelist]
  Go
   Create Procedure [dbo].[pAttachmentTypelist]  
 AS  
 BEGIN  
 select ATT.* ,
 
  CONCAT(ISNULL(EPI.FirstName_En, ''), ' ', ISNULL(EPI.SecondName_En, ''), ' ', ISNULL(EPI.LastName_En, '')) AS UserFullNameEn,
  CONCAT(ISNULL(EPI.FirstName_Ar, ''), ' ', ISNULL(EPI.SecondName_Ar, ''), ' ', ISNULL(EPI.LastName_Ar, '')) AS UserFullNameAr 
 from DMS_DB_DEV.dbo.ATTACHMENT_TYPE ATT 
  LEFT JOIN FATWA_DB_DEV.dbo.UMS_USER ums ON ums.Email =  ATT.CreatedBy
    LEFT JOIN FATWA_DB_DEV.dbo.EP_PERSONAL_INFORMATION EPI ON EPI.UserId = ums.Id
 where ATT.IsDeleted=0  order by ATT.CreatedDate desc;  
 END
 GO
/*<History Author='ijaz Ahmad' Date='04-01-2024'> Dms Kay Publication DocumentList Procedure </History>*/
CREATE OR ALTER PROCEDURE [dbo].[pDmsGetKayPublicationDocumentList]                          
(                   
@EditionNumber nvarchar(500) =null,     
@EditionType nvarchar(500)=null,    
@DocumentTitle nvarchar(500)=null,    
@CreatedFrom datetime=null,                        
@CreatedTo datetime=null                
                                      
)                        
AS                              
BEGIN       
    
Select     
kp.Id,    
kp.EditionNumber,    
kp.EditionType,    
kp.FileTitle,    
kp.DocumentTitle,    
kp.PublicationDate,    
kp.StoragePath,    
kp.CreatedDate,    
kp.StartPage,    
kp.EndPage    
From KAY_PUBLICATION_STG kp      
where kp.IsDeleted!=1    
AND  (kp.EditionNumber = @EditionNumber OR @EditionNumber IS NULL OR @EditionNumber = '')     
AND  (kp.EditionType like '%' + @EditionType + '%' OR @EditionType IS NULL OR @EditionType = '')     
AND  (kp.DocumentTitle like '%' + @DocumentTitle + '%' OR @DocumentTitle IS NULL OR @DocumentTitle = '')     
AND(CAST(kp.CreatedDate as date)>=@CreatedFrom OR @CreatedFrom IS NULL OR @CreatedFrom='')                        
AND (CAST(kp.CreatedDate as date)<=@CreatedTo OR @CreatedTo IS NULL OR @CreatedTo='')    
ORDER BY kp.CreatedDate DESC                  
END 
GO
------------------Add New Attachment Type for Lds ExternalvSource Documents-------------------
IF((SELECT COUNT(*) FROM ATTACHMENT_TYPE WHERE AttachmentTypeId = 95) > 0)
INSERT [dbo].[ATTACHMENT_TYPE] ([AttachmentTypeId], [Type_Ar], [Type_En], [ModuleId], [IsMandatory],[IsOfficialLetter]) VALUES (95, N'LdsExternalSource', N'LdsExternalSource', 1, 0,0)
GO
GO



IF COLUMNPROPERTY(OBJECT_ID('dbo.KAY_PUBLICATION_STG'), 'PublicationDateHijri', 'ColumnId') IS NULL
BEGIN
    ALTER TABLE KAY_PUBLICATION_STG 
    ADD PublicationDateHijri NVARCHAR(50)
	Print('KAY_PUBLICATION_STG.PublicationDateHijri Added')
END
IF COLUMNPROPERTY(OBJECT_ID('dbo.KAY_PUBLICATION_STG'), 'IsFullEdition', 'ColumnId') IS NULL
BEGIN
    ALTER TABLE KAY_PUBLICATION_STG 
    ADD IsFullEdition bit NOT NULL default 0
	Print('KAY_PUBLICATION_STG.IsFullEdition Added')
END

GO
CREATE  OR ALTER  PROCEDURE [dbo].[pDmsGetKayPublicationDocumentList]                                
(                         
@EditionNumber nvarchar(500) =null,           
@EditionType nvarchar(500)=null,          
@DocumentTitle nvarchar(500)=null,         
@PublicationDateHijri nvarchar(500)=null,          
@PublicationFrom datetime=null,                              
@PublicationTo datetime=null
)                              
AS                      
BEGIN      
Select           
kp.Id,          
kp.EditionNumber,          
kp.EditionType,          
kp.FileTitle,          
kp.DocumentTitle,          
kp.PublicationDate,        
kp.PublicationDateHijri,          
kp.StoragePath,          
kp.CreatedDate,          
kp.StartPage,          
kp.EndPage,
kp.IsFullEdition
From KAY_PUBLICATION_STG kp            
where kp.IsDeleted!=1                 
AND (kp.EditionNumber like '%' + @EditionNumber + '%' OR @EditionNumber IS NULL OR @EditionNumber = '')      
AND (kp.EditionType like '%' + @EditionType + '%' OR @EditionType IS NULL OR @EditionType = '')           
AND (kp.DocumentTitle like '%' + @DocumentTitle + '%' OR @DocumentTitle IS NULL OR @DocumentTitle = '')     
AND (kp.PublicationDateHijri like '%' + @PublicationDateHijri + '%' OR @PublicationDateHijri IS NULL OR @PublicationDateHijri = '')           
AND(CAST(kp.PublicationDate as date)>=@PublicationFrom OR @PublicationFrom IS NULL OR @PublicationFrom='')                              
AND (CAST(kp.PublicationDate as date)<=@PublicationTo OR @PublicationTo IS NULL OR @PublicationTo='')          
ORDER BY kp.PublicationDate DESC                        
END   

GO

------------------pMojImagesDocumentList-------------------

CREATE  OR ALTER PROCEDURE [dbo].[pMojImagesDocumentList]                                
(  
    @CANNumber NVARCHAR(500) = NULL,  
    @AttachmentTypeId int = NULL,  
    @CreatedFrom DATETIME = NULL,  
    @CreatedTo DATETIME = NULL  
)  
AS  
BEGIN  
WITH RankedRecords AS (  
SELECT  
md.CANNumber,  
md.Id,  
md.CaseNumber,  
md.DocumentDate,  
aty.Type_En as AttachmentTypeEn,  
aty.Type_Ar as AttachmentTypeAr ,  
md.FileName,  
md.StoragePath,  
md.CreatedDate,  
lh.NameEn AS Name_En,  
lh.NameAr AS Name_Ar,  
ROW_NUMBER() OVER (PARTITION BY md.CANNumber ORDER BY md.CaseNumber DESC) AS RowNum  
FROM MOJ_IMAGE_DOCUMENT md  
LEFT JOIN [FATWA_DB_DEV].[dbo].[CMS_REGISTERED_CASE] crc ON md.CaseNumber = crc.CaseNumber  
LEFT JOIN ATTACHMENT_TYPE aty ON md.AttachmentTypeId=aty.AttachmentTypeId  
LEFT JOIN [FATWA_DB_DEV].[dbo].[LOOKUPS_HISTORY] lh ON crc.CourtId = lh.LookupsId AND crc.CreatedDate BETWEEN lh.StartDate AND ISNULL(lh.EndDate, crc.CreatedDate)  
AND lh.LookupsTableId = (SELECT TablesEnumvalues FROM [FATWA_DB_DEV].[dbo].[LOOKUPS_TABLES] WHERE TableName = 'CMS_COURT_G2G_LKP')  
WHERE md.IsDeleted != 1  
AND (md.CANNumber = @CANNumber OR @CANNumber IS NULL OR @CANNumber = '')    
AND (md.AttachmentTypeId = @AttachmentTypeId OR @AttachmentTypeId IS NULL OR @AttachmentTypeId = '')   
AND (CAST(md.DocumentDate AS DATE) >= @CreatedFrom OR @CreatedFrom IS NULL OR @CreatedFrom = '')  
AND (CAST(md.DocumentDate AS DATE) <= @CreatedTo OR @CreatedTo IS NULL OR @CreatedTo = '')  
    )       
SELECT  
CANNumber,  
Id,  
CaseNumber,  
DocumentDate,  
AttachmentTypeEn,  
AttachmentTypeAr,  
FileName,  
StoragePath,  
CreatedDate,  
Name_En,  
Name_Ar  
FROM RankedRecords  
WHERE RowNum = 1  
ORDER BY CreatedDate DESC;   
END  
Go
------------------pGetMojImagesDocumentListbyCaseNumber-------------------

CREATE OR ALTER  PROCEDURE [dbo].[pGetMojImagesDocumentListbyCaseNumber]                                  
(    
     
    @CaseNumber NVARCHAR(500) = NULL    
      
)    
AS    
BEGIN    
   
SELECT    
md.CANNumber,    
md.Id,    
md.CaseNumber,    
md.DocumentDate,    
aty.Type_En as AttachmentTypeEn,  
aty.Type_Ar as AttachmentTypeAr ,  
md.FileName,    
md.StoragePath,    
md.CreatedDate,    
lh.NameEn AS Name_En,    
lh.NameAr AS Name_Ar  
FROM MOJ_IMAGE_DOCUMENT md    
LEFT JOIN [FATWA_DB_DEV].[dbo].[CMS_REGISTERED_CASE] crc ON md.CaseNumber = crc.CaseNumber   
LEFT JOIN ATTACHMENT_TYPE aty ON md.AttachmentTypeId=aty.AttachmentTypeId  
LEFT JOIN [FATWA_DB_DEV].[dbo].LOOKUPS_HISTORY lh ON crc.CourtId = lh.LookupsId  AND crc.CreatedDate BETWEEN lh.StartDate AND ISNULL(lh.EndDate, crc.CreatedDate)    
AND lh.LookupsTableId = (SELECT TablesEnumvalues FROM [FATWA_DB_DEV].[dbo].[LOOKUPS_TABLES] WHERE TableName = 'CMS_COURT_G2G_LKP')        
WHERE md.IsDeleted != 1   AND (md.CaseNumber = @CaseNumber OR @CaseNumber IS NULL OR @CaseNumber = '')    
ORDER BY CreatedDate DESC;     
END 



GO
CREATE   OR ALTER   Procedure [dbo].[pUploadedDocuments]                                       
(                                          
  @referenceGuid uniqueidentifier = NULL,                              
  @literatureId int = NULL                                 
)                                          
AS                                                    
begin                                          
 Select LUD.*           
   , isnull(cc.PreCommunicationId , '00000000-0000-0000-0000-000000000000') as PreCommunicationId          
   , ATP.Type_Ar                                
   , ATP.Type_En                    
   , LUD.ReferenceGuid as [ReferenceId]        
   , (select count(*) from FATWA_DB_DEV.dbo.COMM_COMMUNICATION fcc where fcc.PreCommunicationId <> '00000000-0000-0000-0000-000000000000'         
      AND PreCommunicationId = LUD.CommunicationGuid        
  ) as ChildCount,
  CASE WHEN LUD.CreatedBy = 'MOJ RPA' THEN 'MOJ' ELSE 'FATWA' END AS DocumentSource
 from UPLOADED_DOCUMENT LUD                                                    
 INNER JOIN ATTACHMENT_TYPE ATP ON ATP.AttachmentTypeId = LUD.AttachmentTypeId          
 LEFT JOIN FATWA_DB_DEV.dbo.COMM_COMMUNICATION cc on LUD.CommunicationGuid = cc.CommunicationId          
 Where LUD.IsDeleted != 1                                                 
 AND LUD.ReferenceGuid = @referenceGuid OR LUD.CommunicationGuid = @referenceGuid OR LUD.LiteratureId = @literatureId    
 --AND LUD.ReferenceGuid = @referenceGuid OR @referenceGuid IS NULL OR LUD.CommunicationGuid = @referenceGuid                      
 --AND (LUD.LiteratureId = @literatureId )--OR @literatureId IS NULL OR @literatureId = '0')       
 AND LUD.IsMaskedAttachment != 1      
 ORDER BY LUD.CreatedDateTime DESC                
END  

GO
CREATE OR ALTER Procedure [dbo].[pOfficialDocuments]                       
(                          
  @referenceGuid uniqueidentifier = NULL             
)                          
AS                                    
begin                          
 Select LUD.*                
   , ATP.Type_Ar                
   , ATP.Type_En
   , CASE WHEN LUD.CreatedBy = 'MOJ RPA' THEN 'MOJ' ELSE 'FATWA' END AS DocumentSource                
 from UPLOADED_DOCUMENT LUD                                    
 INNER JOIN ATTACHMENT_TYPE ATP ON ATP.AttachmentTypeId = LUD.AttachmentTypeId                  
 Where LUD.IsDeleted != 1 AND ATP.IsOfficialLetter = 1                                  
 AND (LUD.ReferenceGuid = @referenceGuid OR @referenceGuid IS NULL)                        
end     

GO
CREATE  OR ALTER  Procedure [dbo].[pTempAttachments]                                   
(                              
 @referenceGuid uniqueidentifier = NULL                     
)                              
AS                                        
begin                              
 Select TAT.*        
   , isnull(cc.PreCommunicationId , '00000000-0000-0000-0000-000000000000') as PreCommunicationId        
   , TAT.UploadedDate AS DocDateTime                    
   , ATP.Type_Ar                    
   , ATP.Type_En             
   , (select count(*) from FATWA_DB_DEV.dbo.COMM_COMMUNICATION fcc where fcc.PreCommunicationId <> '00000000-0000-0000-0000-000000000000'       
      AND PreCommunicationId = TAT.CommunicationGuid      
  ) as ChildCount,
  CASE WHEN TAT.UploadedBy = 'MOJ RPA' THEN 'MOJ' ELSE 'FATWA' END AS DocumentSource      
 from TEMP_ATTACHEMENTS TAT                                        
 LEFT JOIN ATTACHMENT_TYPE ATP ON ATP.AttachmentTypeId = TAT.AttachmentTypeId          
 LEFT JOIN FATWA_DB_DEV.dbo.COMM_COMMUNICATION cc on TAT.CommunicationGuid = cc.CommunicationId        
 Where (TAT.Guid = @referenceGuid OR @referenceGuid IS NULL OR TAT.CommunicationGuid = @referenceGuid)    
 AND (TAT.IsMaskedAttachment != 1 OR TAT.IsMaskedAttachment IS NULL)  
 ORDER BY TAT.UploadedDate DESC     
END   


GO
CREATE   OR ALTER   Procedure [dbo].[pUploadedDocuments]                                         
(                                            
  @referenceGuid uniqueidentifier = NULL,                                
  @literatureId int = NULL                                   
)                                            
AS                                                      
begin                                            
 Select LUD.*             
   , isnull(cc.PreCommunicationId , '00000000-0000-0000-0000-000000000000') as PreCommunicationId            
   , ATP.Type_Ar                                  
   , ATP.Type_En                      
   , LUD.ReferenceGuid as [ReferenceId]          
   , (select count(*) from FATWA_DB_DEV.dbo.COMM_COMMUNICATION fcc where fcc.PreCommunicationId <> '00000000-0000-0000-0000-000000000000'           
      AND PreCommunicationId = LUD.CommunicationGuid          
  ) as ChildCount,  
  CASE 
	  WHEN LUD.CreatedBy = 'MOJ RPA' THEN 'MOJ' 
	  ELSE ISNULL(epi.FirstName_En, '') + ' ' + ISNULL(epi.LastName_En, '') + ' (' + OST.Name_En + ')'
  END AS DocumentSourceEn,  
  CASE 
	  WHEN LUD.CreatedBy = 'MOJ RPA' THEN N'وزارة العدل' 
	  ELSE ISNULL(epi.FirstName_Ar, '') + ' ' + ISNULL(epi.LastName_Ar, '') + ' (' + OST.Name_Ar + ')'
  END AS DocumentSourceAr
 from UPLOADED_DOCUMENT LUD                                                      
 INNER JOIN ATTACHMENT_TYPE ATP ON ATP.AttachmentTypeId = LUD.AttachmentTypeId            
 LEFT JOIN FATWA_DB_DEV.dbo.COMM_COMMUNICATION cc on LUD.CommunicationGuid = cc.CommunicationId
 LEFT JOIN FATWA_DB_DEV.dbo.UMS_USER UU ON UU.Email = LUD.CreatedBy
 LEFT JOIN FATWA_DB_DEV.dbo.EP_PERSONAL_INFORMATION EPI ON EPI.UserId = UU.Id
 LEFT JOIN FATWA_DB_DEV.dbo.EP_EMPLOYMENT_INFORMATION EEI ON EEI.UserId = UU.Id
 LEFT JOIN FATWA_DB_DEV.dbo.CMS_OPERATING_SECTOR_TYPE_G2G_LKP OST ON OST.Id = EEI.SectorTypeId
 Where LUD.IsDeleted != 1                                                   
 AND LUD.ReferenceGuid = @referenceGuid OR LUD.CommunicationGuid = @referenceGuid OR LUD.LiteratureId = @literatureId      
 --AND LUD.ReferenceGuid = @referenceGuid OR @referenceGuid IS NULL OR LUD.CommunicationGuid = @referenceGuid                        
 --AND (LUD.LiteratureId = @literatureId )--OR @literatureId IS NULL OR @literatureId = '0')         
 AND LUD.IsMaskedAttachment != 1        
 ORDER BY LUD.CreatedDateTime DESC                  
END    

GO
CREATE  OR ALTER  Procedure [dbo].[pOfficialDocuments]                         
(                            
  @referenceGuid uniqueidentifier = NULL               
)                            
AS                                      
begin                            
 Select LUD.*                  
   , ATP.Type_Ar                  
   , ATP.Type_En  
   ,CASE 
	  WHEN LUD.CreatedBy = 'MOJ RPA' THEN 'MOJ' 
	  ELSE ISNULL(epi.FirstName_En, '') + ' ' + ISNULL(epi.LastName_En, '') + ' (' + OST.Name_En + ')'
  END AS DocumentSourceEn
  ,CASE 
	  WHEN LUD.CreatedBy = 'MOJ RPA' THEN N'وزارة العدل' 
	  ELSE ISNULL(epi.FirstName_Ar, '') + ' ' + ISNULL(epi.LastName_Ar, '') + ' (' + OST.Name_Ar + ')'
  END AS DocumentSourceAr                 
 from UPLOADED_DOCUMENT LUD                                      
 INNER JOIN ATTACHMENT_TYPE ATP ON ATP.AttachmentTypeId = LUD.AttachmentTypeId  
 LEFT JOIN FATWA_DB_DEV.dbo.UMS_USER UU ON UU.Email = LUD.CreatedBy
 LEFT JOIN FATWA_DB_DEV.dbo.EP_PERSONAL_INFORMATION EPI ON EPI.UserId = UU.Id
 LEFT JOIN FATWA_DB_DEV.dbo.EP_EMPLOYMENT_INFORMATION EEI ON EEI.UserId = UU.Id
 LEFT JOIN FATWA_DB_DEV.dbo.CMS_OPERATING_SECTOR_TYPE_G2G_LKP OST ON OST.Id = EEI.SectorTypeId                  
 Where LUD.IsDeleted != 1 AND ATP.IsOfficialLetter = 1                                    
 AND (LUD.ReferenceGuid = @referenceGuid OR @referenceGuid IS NULL)                          
end     


GO
CREATE  OR ALTER  Procedure [dbo].[pTempAttachments]                                     
(                                
 @referenceGuid uniqueidentifier = NULL                       
)                                
AS                                          
begin                                
 Select TAT.*          
   , isnull(cc.PreCommunicationId , '00000000-0000-0000-0000-000000000000') as PreCommunicationId          
   , TAT.UploadedDate AS DocDateTime                      
   , ATP.Type_Ar                      
   , ATP.Type_En               
   , (select count(*) from FATWA_DB_DEV.dbo.COMM_COMMUNICATION fcc where fcc.PreCommunicationId <> '00000000-0000-0000-0000-000000000000'         
      AND PreCommunicationId = TAT.CommunicationGuid        
  ) as ChildCount,  
  CASE 
	  WHEN TAT.UploadedBy = 'MOJ RPA' THEN 'MOJ' 
	  ELSE ISNULL(epi.FirstName_En, '') + ' ' + ISNULL(epi.LastName_En, '') + ' (' + OST.Name_En + ')'
  END AS DocumentSourceEn,  
  CASE 
	  WHEN TAT.UploadedBy = 'MOJ RPA' THEN N'وزارة العدل' 
	  ELSE ISNULL(epi.FirstName_Ar, '') + ' ' + ISNULL(epi.LastName_Ar, '') + ' (' + OST.Name_Ar + ')'
  END AS DocumentSourceAr     
 from TEMP_ATTACHEMENTS TAT                                          
 LEFT JOIN ATTACHMENT_TYPE ATP ON ATP.AttachmentTypeId = TAT.AttachmentTypeId            
 LEFT JOIN FATWA_DB_DEV.dbo.COMM_COMMUNICATION cc on TAT.CommunicationGuid = cc.CommunicationId 
 LEFT JOIN FATWA_DB_DEV.dbo.UMS_USER UU ON UU.Email = TAT.UploadedBy
 LEFT JOIN FATWA_DB_DEV.dbo.EP_PERSONAL_INFORMATION EPI ON EPI.UserId = UU.Id
 LEFT JOIN FATWA_DB_DEV.dbo.EP_EMPLOYMENT_INFORMATION EEI ON EEI.UserId = UU.Id
 LEFT JOIN FATWA_DB_DEV.dbo.CMS_OPERATING_SECTOR_TYPE_G2G_LKP OST ON OST.Id = EEI.SectorTypeId         
 Where (TAT.Guid = @referenceGuid OR @referenceGuid IS NULL OR TAT.CommunicationGuid = @referenceGuid)      
 AND (TAT.IsMaskedAttachment != 1 OR TAT.IsMaskedAttachment IS NULL)    
 ORDER BY TAT.UploadedDate DESC       
END     
------31-3-24------
update ATTACHMENT_TYPE 
set Type_Ar='Request for Stop Execution of Judgment', Type_En='Request for Stop Execution of Judgment',
ModuleId=5,SubTypeId=0,IsMandatory=1, CreatedDate=GETDATE() where AttachmentTypeId=96;

delete from ATTACHMENT_TYPE where AttachmentTypeId>97

insert into ATTACHMENT_TYPE values(97,N'محضر اجتماع','Meeting Minutes',8,0,0,0,0,0,'fatwaadmin@gmail.com',GETDATE(),null,null,null,null,0,1,1,Meeting)
insert into ATTACHMENT_TYPE values(98,'Stop Execution of Judgment','Stop Execution of Judgment',5,0,0,0,0,0,'fatwaadmin@gmail.com',GETDATE(),null,null,null,null,0,1,1,null)
insert into ATTACHMENT_TYPE values(99,N'G2G Tarasol Correspondence Document','G2G Tarasol Correspondence Document',5,0,0,0,0,0,'fatwaadmin@gmail.com',GETDATE(),null,null,null,null,0,1,1,null)
insert into ATTACHMENT_TYPE values(100,N'Hearing Roll Document','Hearing Roll Document',5,0,0,0,0,0,'fatwaadmin@gmail.com',GETDATE(),null,null,null,null,0,1,1,null)

------------------------------21/4/24
GO
CREATE   PROCEDURE pLLSLegalPrincipleSourceDocList          
(          
 @FileType INT = NULL,      
 @FromDate NVARCHAR(30)=null,                                            
 @ToDate NVARCHAR(30)=null,      
 @CourtId INT =null,               
 @JudgementTypeId INT = null ,                              
 @ChamberId int=null,                                    
 @ChamberNumberId INT = null,                                    
 @CaseNumber NVARCHAR(2000) = NULL,       
 @CANNumber NVARCHAR(2000) = NULL        
)          
AS          
BEGIN          
SELECT           
ud.UploadedDocumentId,          
cjs.NameEn JudgementTypeEn,          
cjs.NameAr JudgementTypeAr,          
cj.JudgementDate AS JudgementDate,          
crc.CaseNumber,          
crc.CANNumber,          
cc.Name_En ChamberNameEn,          
cc.Name_Ar ChamberNameAr,          
ccn.Number ChamberNumber,          
co.Name_En CourtEn,          
co.Name_Ar CourtAr,        
ud.AttachmentTypeId,        
ud.StoragePath,        
ud.DocType,    
ud.FileName,    
ud.Description,    
ud.FileTitle,  
ud.CreatedDateTime,  
COUNT(lpsd.UploadedDocumentId) NumberOfPrinciple  
FROM DMS_DB_DEV.DBO.UPLOADED_DOCUMENT ud          
JOIN FATWA_DB_DEV.DBO.CMS_JUDGEMENT cj on cj.Id = ud.ReferenceGuid  AND AttachmentTypeId IN (60,61,63)        
LEFT JOIN FATWA_DB_DEV.DBO.CMS_REGISTERED_CASE crc on cj.CaseId = crc.CaseId AND cj.CaseId IS NOT NULL          
LEFT JOIN FATWA_DB_DEV.DBO.CMS_CHAMBER_G2G_LKP cc on cc.Id = crc.ChamberId AND (@ChamberId IS NULL OR @ChamberId = '' OR @ChamberId = cc.id)      
LEFT JOIN FATWA_DB_DEV.DBO.CMS_COURT_G2G_LKP co on co.Id = crc.CourtId AND (@CourtId IS NULL OR @CourtId = '' OR @CourtId = co.Id)          
LEFT JOIN FATWA_DB_DEV.DBO.CMS_CHAMBER_NUMBER_G2G_LKP ccn on ccn.Id = crc.ChamberNumberId AND (@ChamberNumberId IS NULL OR @ChamberNumberId = '' OR @ChamberNumberId = ccn.Id)      
LEFT JOIN FATWA_DB_DEV.DBO.CMS_JUDGEMENT_STATUS_G2G_LKP cjs on cjs.Id = cj.StatusId      
LEFT JOIN FATWA_DB_DEV.DBO.LLS_LEGAL_PRINCIPLE_SOURCE_DOCUMENT_REFERENCE lpsd ON lpsd.UploadedDocumentId = ud.UploadedDocumentId  
  
WHERE           
(@FileType IS NULL OR @FileType = '' OR           
AttachmentTypeId = (          
CASE           
WHEN @FileType = 1 THEN 60           
WHEN @FileType = 2 THEN 61           
WHEN @FileType = 4 THEN 63           
ELSE -1 END))          
AND(CAST(cj.JudgementDate as date)>=@FromDate OR @FromDate IS NULL OR @FromDate='')                                            
AND (CAST(cj.JudgementDate as date)<=@ToDate OR @ToDate IS NULL OR @ToDate='')                                    
AND (crc.CaseNumber=@CaseNumber OR @CaseNumber IS NULL OR @CaseNumber=N'')      
AND (crc.CANNumber=@CANNumber OR @CANNumber IS NULL OR @CANNumber=N'')     
GROUP BY  
ud.UploadedDocumentId,          
cjs.NameEn,          
cjs.NameAr,          
cj.JudgementDate,          
crc.CaseNumber,          
crc.CANNumber,          
cc.Name_En,          
cc.Name_Ar,          
ccn.Number,          
co.Name_En,          
co.Name_Ar,        
ud.AttachmentTypeId,        
ud.StoragePath,        
ud.DocType,    
ud.FileName,    
ud.Description,    
ud.FileTitle,  
ud.CreatedDateTime  
ORDER BY ud.CreatedDateTime desc          
END


INSERT INTO [dbo].[ATTACHMENT_TYPE]
           ([AttachmentTypeId], [Type_Ar], [Type_En], [ModuleId], [IsMandatory], [IsOfficialLetter], [IsGePortalType], [IsOpinion], [CreatedBy], [CreatedDate], [IsDeleted], [IsActive], [IsSystemDefine])
     VALUES
           (101, N'صورة طبق الاصل - محكمة الاستئناف', N'Appeal Judgment - Copy Original',5,0,0,0,0,'fatwaadmin@gmail.com',GETDATE(),0,1,1)
GO

INSERT INTO [dbo].[ATTACHMENT_TYPE]
           ([AttachmentTypeId], [Type_Ar], [Type_En], [ModuleId], [IsMandatory], [IsOfficialLetter], [IsGePortalType], [IsOpinion], [CreatedBy], [CreatedDate], [IsDeleted], [IsActive], [IsSystemDefine])
     VALUES
           (102, N'حكم مشورة', N'Counsel Judgment',5,0,0,0,0,'fatwaadmin@gmail.com',GETDATE(),0,1,1)
GO

INSERT INTO [dbo].[ATTACHMENT_TYPE]
           ([AttachmentTypeId], [Type_Ar], [Type_En], [ModuleId], [IsMandatory], [IsOfficialLetter], [IsGePortalType], [IsOpinion], [CreatedBy], [CreatedDate], [IsDeleted], [IsActive], [IsSystemDefine])
     VALUES
           (103, N'حكم تصحيح خطأ مادي', N'Correction Judgment',5,0,0,0,0,'fatwaadmin@gmail.com',GETDATE(),0,1,1)
GO

INSERT INTO [dbo].[ATTACHMENT_TYPE]
           ([AttachmentTypeId], [Type_Ar], [Type_En], [ModuleId], [IsMandatory], [IsOfficialLetter], [IsGePortalType], [IsOpinion], [CreatedBy], [CreatedDate], [IsDeleted], [IsActive], [IsSystemDefine])
     VALUES
           (104, N'حكم استئناف مستعجل', N'Appeal Urgent Judgment',5,0,0,0,0,'fatwaadmin@gmail.com',GETDATE(),0,1,1)
GO

INSERT INTO [dbo].[ATTACHMENT_TYPE]
           ([AttachmentTypeId], [Type_Ar], [Type_En], [ModuleId], [IsMandatory], [IsOfficialLetter], [IsGePortalType], [IsOpinion], [CreatedBy], [CreatedDate], [IsDeleted], [IsActive], [IsSystemDefine])
     VALUES
           (105, N'منطوق حكم', N'Judgment Statement',5,0,0,0,0,'fatwaadmin@gmail.com',GETDATE(),0,1,1)
GO

INSERT INTO [dbo].[ATTACHMENT_TYPE]
           ([AttachmentTypeId], [Type_Ar], [Type_En], [ModuleId], [IsMandatory], [IsOfficialLetter], [IsGePortalType], [IsOpinion], [CreatedBy], [CreatedDate], [IsDeleted], [IsActive], [IsSystemDefine])
     VALUES
           (106, N'حكم تمهيدي', N'Initial Judgment',5,0,0,0,0,'fatwaadmin@gmail.com',GETDATE(),0,1,1)
GO

INSERT INTO [dbo].[ATTACHMENT_TYPE]
           ([AttachmentTypeId], [Type_Ar], [Type_En], [ModuleId], [IsMandatory], [IsOfficialLetter], [IsGePortalType], [IsOpinion], [CreatedBy], [CreatedDate], [IsDeleted], [IsActive], [IsSystemDefine])
     VALUES
           (107, N'صورة ضوئية - محكمة التمييز', N'Supreme Judgment - Copy',5,0,0,0,0,'fatwaadmin@gmail.com',GETDATE(),0,1,1)
GO

INSERT INTO [dbo].[ATTACHMENT_TYPE]
           ([AttachmentTypeId], [Type_Ar], [Type_En], [ModuleId], [IsMandatory], [IsOfficialLetter], [IsGePortalType], [IsOpinion], [CreatedBy], [CreatedDate], [IsDeleted], [IsActive], [IsSystemDefine])
     VALUES
           (108, N'صورة طبق الاصل - محكمة التمييز', N'Supreme Judgment - Copy Original',5,0,0,0,0,'fatwaadmin@gmail.com',GETDATE(),0,1,1)
GO

INSERT INTO [dbo].[ATTACHMENT_TYPE]
           ([AttachmentTypeId], [Type_Ar], [Type_En], [ModuleId], [IsMandatory], [IsOfficialLetter], [IsGePortalType], [IsOpinion], [CreatedBy], [CreatedDate], [IsDeleted], [IsActive], [IsSystemDefine])
     VALUES
           (109, N'حكم تمييز تمهيدي', N'Supreme Initial Judgment',5,0,0,0,0,'fatwaadmin@gmail.com',GETDATE(),0,1,1)
GO
INSERT INTO [dbo].[ATTACHMENT_TYPE]
           ([AttachmentTypeId], [Type_Ar], [Type_En], [ModuleId], [IsMandatory], [IsOfficialLetter], [IsGePortalType], [IsOpinion], [CreatedBy], [CreatedDate], [IsDeleted], [IsActive], [IsSystemDefine])
     VALUES
           (110, N'حكم المحكمة', N'Court Judgment',5,0,0,0,0,'fatwaadmin@gmail.com',GETDATE(),0,1,1)
GO

INSERT INTO [dbo].[ATTACHMENT_TYPE]
           ([AttachmentTypeId], [Type_Ar], [Type_En], [ModuleId], [IsMandatory], [IsOfficialLetter], [IsGePortalType], [IsOpinion], [CreatedBy], [CreatedDate], [IsDeleted], [IsActive], [IsSystemDefine])
     VALUES
           (111, N'حكم استئناف جزئي', N'Appeal Partial Judgment',5,0,0,0,0,'fatwaadmin@gmail.com',GETDATE(),0,1,1)
GO	

-----------------------------23/4/24
CREATE OR ALTER PROCEDURE pGetLLSlegalPrincipleReferenceDocuments    
(    
 @PrincipleId UNIQUEIDENTIFIER = '00000000-0000-0000-0000-000000000000'    
)    
AS    
BEGIN    
SELECT    
ud.*,  
 null as PreCommunicationId                
, ATP.Type_Ar                                      
, ATP.Type_En                          
 , 0 ChildCount,      
 CONCAT(ISNULL(EPI.FirstName_En, ''), ' ', ISNULL(EPI.SecondName_En, ''), ' ', ISNULL(EPI.LastName_En, '')) AS DocumentSourceEn,        
 CONCAT(ISNULL(EPI.FirstName_Ar, ''), ' ', ISNULL(EPI.SecondName_Ar, ''), ' ', ISNULL(EPI.LastName_Ar, '')) AS DocumentSourceAr   
FROM     
UPLOADED_DOCUMENT ud    
 INNER JOIN ATTACHMENT_TYPE ATP ON ATP.AttachmentTypeId = ud.AttachmentTypeId                
 LEFT JOIN FATWA_DB_DEV.dbo.UMS_USER UU ON UU.Email = ud.CreatedBy    
 LEFT JOIN FATWA_DB_DEV.dbo.EP_PERSONAL_INFORMATION EPI ON EPI.UserId = UU.Id    
 LEFT JOIN FATWA_DB_DEV.dbo.EP_EMPLOYMENT_INFORMATION EEI ON EEI.UserId = UU.Id    
where UploadedDocumentId in (SELECT     
CASE WHEN    
IsMaskJudgment = 1 THEN MaskedJudgmentId    
ELSE UploadedDocumentId    
END UPLOADDOCUMENTiD    
FROM     
FATWA_DB_DEV.dbo.LLS_LEGAL_PRINCIPLE_SOURCE_DOCUMENT_REFERENCE A where PrincipleId = @PrincipleId)    
END
GO
CREATE OR ALTER   PROCEDURE pLLSLegalPrincipleSourceDocList              
(              
 @FileType INT = NULL,          
 @FromDate NVARCHAR(30)=null,                                                
 @ToDate NVARCHAR(30)=null,          
 @CourtId INT =null,                   
 @JudgementTypeId INT = null ,                                  
 @ChamberId int=null,                                        
 @ChamberNumberId INT = null,                                        
 @CaseNumber NVARCHAR(2000) = NULL,           
 @CANNumber NVARCHAR(2000) = NULL            
)              
AS              
BEGIN              
SELECT               
ud.UploadedDocumentId,              
cjs.NameEn JudgementTypeEn,              
cjs.NameAr JudgementTypeAr,              
cj.JudgementDate AS JudgementDate,              
crc.CaseNumber,              
crc.CANNumber,              
cc.Name_En ChamberNameEn,              
cc.Name_Ar ChamberNameAr,              
ccn.Number ChamberNumber,              
co.Name_En CourtEn,              
co.Name_Ar CourtAr,            
ud.AttachmentTypeId,            
ud.StoragePath,            
ud.DocType,        
ud.FileName,        
ud.Description,        
ud.FileTitle,      
ud.CreatedDateTime,  
ct.Id CourtTypeId,  
COUNT(lpsd.UploadedDocumentId) NumberOfPrinciple   
FROM DMS_DB_DEV.DBO.UPLOADED_DOCUMENT ud              
JOIN FATWA_DB_DEV.DBO.CMS_JUDGEMENT cj on cj.Id = ud.ReferenceGuid  AND AttachmentTypeId IN (SELECT AttachmentTypeId from ATTACHMENT_TYPE where ModuleId = 5)            
JOIN FATWA_DB_DEV.DBO.CMS_REGISTERED_CASE crc on cj.CaseId = crc.CaseId AND cj.CaseId IS NOT NULL              
JOIN FATWA_DB_DEV.DBO.CMS_CHAMBER_G2G_LKP cc on cc.Id = crc.ChamberId AND (@ChamberId IS NULL OR @ChamberId = '' OR @ChamberId = cc.id)          
JOIN FATWA_DB_DEV.DBO.CMS_COURT_G2G_LKP co on co.Id = crc.CourtId AND (@CourtId IS NULL OR @CourtId = '' OR @CourtId = co.Id)       
JOIN FATWA_DB_DEV.DBO.CMS_COURT_TYPE_G2G_LKP ct on ct.Id = co.TypeId AND (co.TypeId IN (2, 4))  
JOIN FATWA_DB_DEV.DBO.CMS_CHAMBER_NUMBER_G2G_LKP ccn on ccn.Id = crc.ChamberNumberId AND (@ChamberNumberId IS NULL OR @ChamberNumberId = '' OR @ChamberNumberId = ccn.Id)          
JOIN FATWA_DB_DEV.DBO.CMS_JUDGEMENT_STATUS_G2G_LKP cjs on cjs.Id = cj.StatusId          
LEFT JOIN FATWA_DB_DEV.DBO.LLS_LEGAL_PRINCIPLE_SOURCE_DOCUMENT_REFERENCE lpsd ON lpsd.UploadedDocumentId = ud.UploadedDocumentId      
      
WHERE               
(@FileType IS NULL OR @FileType = '' OR               
AttachmentTypeId = (              
CASE               
WHEN @FileType = 1 THEN 60               
WHEN @FileType = 2 THEN 61               
WHEN @FileType = 4 THEN 63               
ELSE -1 END))              
AND(CAST(cj.JudgementDate as date)>=@FromDate OR @FromDate IS NULL OR @FromDate='')                                                
AND (CAST(cj.JudgementDate as date)<=@ToDate OR @ToDate IS NULL OR @ToDate='')                                        
AND (crc.CaseNumber=@CaseNumber OR @CaseNumber IS NULL OR @CaseNumber=N'')          
AND (crc.CANNumber=@CANNumber OR @CANNumber IS NULL OR @CANNumber=N'')         
GROUP BY      
ud.UploadedDocumentId,              
cjs.NameEn,              
cjs.NameAr,              
cj.JudgementDate,              
crc.CaseNumber,              
crc.CANNumber,              
cc.Name_En,              
cc.Name_Ar,              
ccn.Number,              
co.Name_En,              
co.Name_Ar,            
ud.AttachmentTypeId,            
ud.StoragePath,            
ud.DocType,        
ud.FileName,        
ud.Description,        
ud.FileTitle,      
ud.CreatedDateTime,  
CT.Id  
ORDER BY ud.CreatedDateTime desc              
END

------------------------------------24/4/24
GO
CREATE OR ALTER PROCEDURE pLLSLegalPrincipleSourceDocList                  
(                  
 @FileType INT = NULL,              
 @FromDate NVARCHAR(30)=null,                                                    
 @ToDate NVARCHAR(30)=null,              
 @CourtId INT =null,                       
 @JudgementTypeId INT = null ,                                      
 @ChamberId int=null,                                            
 @ChamberNumberId INT = null,                                            
 @CaseNumber NVARCHAR(2000) = NULL,               
 @CANNumber NVARCHAR(2000) = NULL                
)                  
AS                  
BEGIN    
--Common Data Exclusion    
SELECT ud.UploadedDocumentId,ud.FileName,ud.StoragePath, 0 AS IsJudgment INTO #tmpCaseData     
FROM DMS_DB_DEV.DBO.UPLOADED_DOCUMENT ud                  
JOIN FATWA_DB_DEV.DBO.CMS_REGISTERED_CASE crc on crc.CaseId = ud.ReferenceGuid AND AttachmentTypeId IN (60, 61, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110,111)      
    
SELECT ud.UploadedDocumentId,ud.FileName,ud.StoragePath, 1 AS IsJudgment  INTO #tmpJudgmentData      
FROM DMS_DB_DEV.DBO.UPLOADED_DOCUMENT ud                  
JOIN FATWA_DB_DEV.DBO.CMS_JUDGEMENT crcc on crcc.Id = ud.ReferenceGuid AND AttachmentTypeId IN (60, 61, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110,111)      
    
SELECT DISTINCT CD.UploadedDocumentId INTO #tmpExclude_ID FROM #tmpCaseData CD    
INNER JOIN #tmpJudgmentData JM ON JM.FileName = CD.FileName AND JM.StoragePath = CD.StoragePath    
--END    
SELECT                   
ud.UploadedDocumentId,                  
'-' JudgementTypeEn,                  
'-' JudgementTypeAr,                  
NULL AS JudgementDate,  
0 AS IsJudgement,  
crc.CaseNumber,                  
crc.CANNumber,                  
cc.Name_En ChamberNameEn,                  
cc.Name_Ar ChamberNameAr,                  
ccn.Number ChamberNumber,                  
co.Name_En CourtEn,                  
co.Name_Ar CourtAr,                
ud.AttachmentTypeId,                
ud.StoragePath,                
ud.DocType,            
ud.FileName,            
ud.Description,            
ud.FileTitle,          
ud.CreatedDateTime,      
ct.Id CourtTypeId,      
COUNT(lpsd.UploadedDocumentId) NumberOfPrinciple       
FROM DMS_DB_DEV.DBO.UPLOADED_DOCUMENT ud                  
JOIN FATWA_DB_DEV.DBO.CMS_REGISTERED_CASE crc on crc.CaseId = ud.ReferenceGuid AND AttachmentTypeId IN (60, 61, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110,111)                 
JOIN FATWA_DB_DEV.DBO.CMS_CHAMBER_G2G_LKP cc on cc.Id = crc.ChamberId AND (@ChamberId IS NULL OR @ChamberId = '' OR @ChamberId = cc.id)              
JOIN FATWA_DB_DEV.DBO.CMS_COURT_G2G_LKP co on co.Id = crc.CourtId AND (@CourtId IS NULL OR @CourtId = '' OR @CourtId = co.Id)           
JOIN FATWA_DB_DEV.DBO.CMS_COURT_TYPE_G2G_LKP ct on ct.Id = co.TypeId AND (co.TypeId IN (2, 4))      
JOIN FATWA_DB_DEV.DBO.CMS_CHAMBER_NUMBER_G2G_LKP ccn on ccn.Id = crc.ChamberNumberId AND (@ChamberNumberId IS NULL OR @ChamberNumberId = '' OR @ChamberNumberId = ccn.Id)              
LEFT JOIN FATWA_DB_DEV.DBO.LLS_LEGAL_PRINCIPLE_SOURCE_DOCUMENT_REFERENCE lpsd ON lpsd.UploadedDocumentId = ud.UploadedDocumentId          
          
WHERE ud.UploadedDocumentId NOT IN (SELECT UploadedDocumentId FROM #tmpExclude_ID)                                  
AND(CAST(ud.CreatedDateTime as date)>=@FromDate OR @FromDate IS NULL OR @FromDate='')                                                    
AND (CAST(ud.CreatedDateTime as date)<=@ToDate OR @ToDate IS NULL OR @ToDate='')                                            
AND (crc.CaseNumber=@CaseNumber OR @CaseNumber IS NULL OR @CaseNumber=N'')              
AND (crc.CANNumber=@CANNumber OR @CANNumber IS NULL OR @CANNumber=N'')             
GROUP BY          
ud.UploadedDocumentId,                               
crc.CaseNumber,                  
crc.CANNumber,                  
cc.Name_En,                  
cc.Name_Ar,                  
ccn.Number,                  
co.Name_En,                  
co.Name_Ar,              
ud.AttachmentTypeId,                
ud.StoragePath,                
ud.DocType,            
ud.FileName,            
ud.Description,            
ud.FileTitle,          
ud.CreatedDateTime,      
CT.Id      
   
UNION    
     
SELECT                   
ud.UploadedDocumentId,                  
ISNULL(JTS.NameEn,'-') AS JudgementTypeEn,                  
ISNULL(JTS.NameAr, '-') AS JudgementTypeAr,                  
cj.JudgementDate,   
1 AS IsJudgement,  
CMR.CaseNumber,                  
CMR.CANNumber,                  
cc.Name_En ChamberNameEn,                  
cc.Name_Ar ChamberNameAr,                  
ccn.Number ChamberNumber,                  
co.Name_En CourtEn,                  
co.Name_Ar CourtAr,                
ud.AttachmentTypeId,                
ud.StoragePath,                
ud.DocType,            
ud.FileName,            
ud.Description,            
ud.FileTitle,          
ud.CreatedDateTime,      
ct.Id CourtTypeId,      
COUNT(lpsd.UploadedDocumentId) NumberOfPrinciple       
FROM DMS_DB_DEV.DBO.UPLOADED_DOCUMENT ud                  
JOIN FATWA_DB_DEV.DBO.CMS_JUDGEMENT cj on cj.Id = ud.ReferenceGuid  AND AttachmentTypeId IN (60, 61, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110,111)                
 LEFT JOIN FATWA_DB_DEV.dbo.CMS_REGISTERED_CASE CMR ON CMR.CaseId = cj.CaseId    
 LEFT JOIN FATWA_DB_DEV.dbo.CMS_JUDGEMENT_STATUS_G2G_LKP JTS ON JTS.Id = cj.StatusId    
JOIN FATWA_DB_DEV.DBO.CMS_CHAMBER_G2G_LKP cc on cc.Id = CMR.ChamberId AND (@ChamberId IS NULL OR @ChamberId = '' OR @ChamberId = cc.id)              
JOIN FATWA_DB_DEV.DBO.CMS_COURT_G2G_LKP co on co.Id = CMR.CourtId AND (@CourtId IS NULL OR @CourtId = '' OR @CourtId = co.Id)           
JOIN FATWA_DB_DEV.DBO.CMS_COURT_TYPE_G2G_LKP ct on ct.Id = co.TypeId AND (co.TypeId IN (2, 4))      
JOIN FATWA_DB_DEV.DBO.CMS_CHAMBER_NUMBER_G2G_LKP ccn on ccn.Id = CMR.ChamberNumberId AND (@ChamberNumberId IS NULL OR @ChamberNumberId = '' OR @ChamberNumberId = ccn.Id)              
LEFT JOIN FATWA_DB_DEV.DBO.LLS_LEGAL_PRINCIPLE_SOURCE_DOCUMENT_REFERENCE lpsd ON lpsd.UploadedDocumentId = ud.UploadedDocumentId          
          
WHERE                 
(CAST(ud.CreatedDateTime as date)>=@FromDate OR @FromDate IS NULL OR @FromDate='')                                                    
AND (CAST(ud.CreatedDateTime as date)<=@ToDate OR @ToDate IS NULL OR @ToDate='')                                            
AND (CMR.CaseNumber=@CaseNumber OR @CaseNumber IS NULL OR @CaseNumber=N'')              
AND (CMR.CANNumber=@CANNumber OR @CANNumber IS NULL OR @CANNumber=N'')     
AND (cj.StatusId=@JudgementTypeId OR @JudgementTypeId IS NULL OR @JudgementTypeId=N'')             
  
GROUP BY          
ud.UploadedDocumentId,    
JTS.NameEn,                  
JTS.NameAr,                  
cj.JudgementDate,     
CMR.CaseNumber,                  
CMR.CANNumber,                  
cc.Name_En,                  
cc.Name_Ar,                  
ccn.Number,                  
co.Name_En,                  
co.Name_Ar,                
ud.AttachmentTypeId,                
ud.StoragePath,                
ud.DocType,            
ud.FileName,            
ud.Description,            
ud.FileTitle,          
ud.CreatedDateTime,      
CT.Id      
ORDER BY ud.CreatedDateTime desc      
    
DROP TABLE #tmpCaseData    
DROP TABLE #tmpJudgmentData    
DROP TABLE #tmpExclude_ID    
END

GO
CREATE OR ALTER PROCEDURE pGetLLSlegalPrincipleReferenceDocuments      
(      
 @PrincipleId UNIQUEIDENTIFIER = '00000000-0000-0000-0000-000000000000'      
)      
AS      
BEGIN      
SELECT      
ud.*,    
 null as PreCommunicationId                  
, ATP.Type_Ar                                        
, ATP.Type_En                            
 , 0 ChildCount,        
 CONCAT(ISNULL(EPI.FirstName_En, ''), ' ', ISNULL(EPI.SecondName_En, ''), ' ', ISNULL(EPI.LastName_En, '')) AS DocumentSourceEn,          
 CONCAT(ISNULL(EPI.FirstName_Ar, ''), ' ', ISNULL(EPI.SecondName_Ar, ''), ' ', ISNULL(EPI.LastName_Ar, '')) AS DocumentSourceAr     
FROM       
UPLOADED_DOCUMENT ud      
 INNER JOIN ATTACHMENT_TYPE ATP ON ATP.AttachmentTypeId = ud.AttachmentTypeId                  
 LEFT JOIN FATWA_DB_DEV.dbo.UMS_USER UU ON UU.Email = ud.CreatedBy      
 LEFT JOIN FATWA_DB_DEV.dbo.EP_PERSONAL_INFORMATION EPI ON EPI.UserId = UU.Id      
 LEFT JOIN FATWA_DB_DEV.dbo.EP_EMPLOYMENT_INFORMATION EEI ON EEI.UserId = UU.Id      
where UploadedDocumentId in (SELECT       
CASE WHEN      
IsMaskJudgment = 1 THEN MaskedJudgmentId      
ELSE UploadedDocumentId      
END UPLOADDOCUMENTiD      
FROM       
FATWA_DB_DEV.dbo.LLS_LEGAL_PRINCIPLE_SOURCE_DOCUMENT_REFERENCE A where PrincipleId = @PrincipleId)      
END

-----------------------------------------------
-------- 6-5-2024 -------------------

alter table UPLOADED_DOCUMENT
add SignedDocStoragePath varchar(max)

----------------------------------------8/5/24
GO
CREATE OR ALTER PROCEDURE pLLSLegalPrincipleSourceDocList                  
(                  
 @FileType INT = NULL,              
 @FromDate NVARCHAR(30)=null,                                                    
 @ToDate NVARCHAR(30)=null,              
 @CourtId INT =null,                       
 @JudgementTypeId INT = null ,                                      
 @ChamberId int=null,                                            
 @ChamberNumberId INT = null,                                            
 @CaseNumber NVARCHAR(2000) = NULL,               
 @CANNumber NVARCHAR(2000) = NULL                
)                  
AS                  
BEGIN    
--Common Data Exclusion    
SELECT ud.UploadedDocumentId,ud.FileName,ud.StoragePath, 0 AS IsJudgment INTO #tmpCaseData     
FROM DMS_DB_DEV.DBO.UPLOADED_DOCUMENT ud                  
JOIN FATWA_DB_DEV.DBO.CMS_REGISTERED_CASE crc on crc.CaseId = ud.ReferenceGuid AND AttachmentTypeId IN (60, 61, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110,111)      
    
SELECT ud.UploadedDocumentId,ud.FileName,ud.StoragePath, 1 AS IsJudgment  INTO #tmpJudgmentData      
FROM DMS_DB_DEV.DBO.UPLOADED_DOCUMENT ud                  
JOIN FATWA_DB_DEV.DBO.CMS_JUDGEMENT crcc on crcc.Id = ud.ReferenceGuid AND AttachmentTypeId IN (60, 61, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110,111)      
    
SELECT DISTINCT CD.UploadedDocumentId INTO #tmpExclude_ID FROM #tmpCaseData CD    
INNER JOIN #tmpJudgmentData JM ON JM.FileName = CD.FileName AND JM.StoragePath = CD.StoragePath    
--END    
SELECT                   
ud.UploadedDocumentId,                  
'-' JudgementTypeEn,                  
'-' JudgementTypeAr,                  
NULL AS JudgementDate,  
0 AS IsJudgement,  
crc.CaseNumber,                  
crc.CANNumber,                  
cc.Name_En ChamberNameEn,                  
cc.Name_Ar ChamberNameAr,                  
ccn.Number ChamberNumber,                  
co.Name_En CourtEn,                  
co.Name_Ar CourtAr,                
ud.AttachmentTypeId,                
ud.StoragePath,                
ud.DocType,            
ud.FileName,            
ud.Description,            
ud.FileTitle,          
ud.CreatedDateTime,      
ct.Id CourtTypeId,      
COUNT(lpsd.OriginalSourceDocId) NumberOfPrinciple       
FROM DMS_DB_DEV.DBO.UPLOADED_DOCUMENT ud                  
JOIN FATWA_DB_DEV.DBO.CMS_REGISTERED_CASE crc on crc.CaseId = ud.ReferenceGuid AND AttachmentTypeId IN (60, 61, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110,111)                 
JOIN FATWA_DB_DEV.DBO.CMS_CHAMBER_G2G_LKP cc on cc.Id = crc.ChamberId AND (@ChamberId IS NULL OR @ChamberId = '' OR @ChamberId = cc.id)              
JOIN FATWA_DB_DEV.DBO.CMS_COURT_G2G_LKP co on co.Id = crc.CourtId AND (@CourtId IS NULL OR @CourtId = '' OR @CourtId = co.Id)           
JOIN FATWA_DB_DEV.DBO.CMS_COURT_TYPE_G2G_LKP ct on ct.Id = co.TypeId AND (co.TypeId IN (2, 4))      
JOIN FATWA_DB_DEV.DBO.CMS_CHAMBER_NUMBER_G2G_LKP ccn on ccn.Id = crc.ChamberNumberId AND (@ChamberNumberId IS NULL OR @ChamberNumberId = '' OR @ChamberNumberId = ccn.Id)              
LEFT JOIN FATWA_DB_DEV.DBO.LLS_LEGAL_PRINCIPLE_CONTENT_SOURCE_DOCUMENT_REFERENCE lpsd ON lpsd.OriginalSourceDocId = ud.UploadedDocumentId          
          
WHERE ud.UploadedDocumentId NOT IN (SELECT UploadedDocumentId FROM #tmpExclude_ID)                                  
AND(CAST(ud.CreatedDateTime as date)>=@FromDate OR @FromDate IS NULL OR @FromDate='')                                                    
AND (CAST(ud.CreatedDateTime as date)<=@ToDate OR @ToDate IS NULL OR @ToDate='')                                            
AND (crc.CaseNumber=@CaseNumber OR @CaseNumber IS NULL OR @CaseNumber=N'')              
AND (crc.CANNumber=@CANNumber OR @CANNumber IS NULL OR @CANNumber=N'')             
GROUP BY          
ud.UploadedDocumentId,                               
crc.CaseNumber,                  
crc.CANNumber,                  
cc.Name_En,                  
cc.Name_Ar,                  
ccn.Number,                  
co.Name_En,                  
co.Name_Ar,              
ud.AttachmentTypeId,                
ud.StoragePath,                
ud.DocType,            
ud.FileName,            
ud.Description,            
ud.FileTitle,          
ud.CreatedDateTime,      
CT.Id      
   
UNION    
     
SELECT                   
ud.UploadedDocumentId,                  
ISNULL(JTS.NameEn,'-') AS JudgementTypeEn,                  
ISNULL(JTS.NameAr, '-') AS JudgementTypeAr,                  
cj.JudgementDate,   
1 AS IsJudgement,  
CMR.CaseNumber,                  
CMR.CANNumber,                  
cc.Name_En ChamberNameEn,                  
cc.Name_Ar ChamberNameAr,                  
ccn.Number ChamberNumber,                  
co.Name_En CourtEn,                  
co.Name_Ar CourtAr,                
ud.AttachmentTypeId,                
ud.StoragePath,                
ud.DocType,            
ud.FileName,            
ud.Description,            
ud.FileTitle,          
ud.CreatedDateTime,      
ct.Id CourtTypeId,      
COUNT(lpsd.OriginalSourceDocId) NumberOfPrinciple       
FROM DMS_DB_DEV.DBO.UPLOADED_DOCUMENT ud                  
JOIN FATWA_DB_DEV.DBO.CMS_JUDGEMENT cj on cj.Id = ud.ReferenceGuid  AND AttachmentTypeId IN (60, 61, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110,111)                
 LEFT JOIN FATWA_DB_DEV.dbo.CMS_REGISTERED_CASE CMR ON CMR.CaseId = cj.CaseId    
 LEFT JOIN FATWA_DB_DEV.dbo.CMS_JUDGEMENT_STATUS_G2G_LKP JTS ON JTS.Id = cj.StatusId    
JOIN FATWA_DB_DEV.DBO.CMS_CHAMBER_G2G_LKP cc on cc.Id = CMR.ChamberId AND (@ChamberId IS NULL OR @ChamberId = '' OR @ChamberId = cc.id)              
JOIN FATWA_DB_DEV.DBO.CMS_COURT_G2G_LKP co on co.Id = CMR.CourtId AND (@CourtId IS NULL OR @CourtId = '' OR @CourtId = co.Id)           
JOIN FATWA_DB_DEV.DBO.CMS_COURT_TYPE_G2G_LKP ct on ct.Id = co.TypeId AND (co.TypeId IN (2, 4))      
JOIN FATWA_DB_DEV.DBO.CMS_CHAMBER_NUMBER_G2G_LKP ccn on ccn.Id = CMR.ChamberNumberId AND (@ChamberNumberId IS NULL OR @ChamberNumberId = '' OR @ChamberNumberId = ccn.Id)              
LEFT JOIN FATWA_DB_DEV.DBO.LLS_LEGAL_PRINCIPLE_CONTENT_SOURCE_DOCUMENT_REFERENCE lpsd ON lpsd.OriginalSourceDocId = ud.UploadedDocumentId          
          
WHERE                 
(CAST(ud.CreatedDateTime as date)>=@FromDate OR @FromDate IS NULL OR @FromDate='')                                                    
AND (CAST(ud.CreatedDateTime as date)<=@ToDate OR @ToDate IS NULL OR @ToDate='')                                            
AND (CMR.CaseNumber=@CaseNumber OR @CaseNumber IS NULL OR @CaseNumber=N'')              
AND (CMR.CANNumber=@CANNumber OR @CANNumber IS NULL OR @CANNumber=N'')     
AND (cj.StatusId=@JudgementTypeId OR @JudgementTypeId IS NULL OR @JudgementTypeId=N'')             
  
GROUP BY          
ud.UploadedDocumentId,    
JTS.NameEn,                  
JTS.NameAr,                  
cj.JudgementDate,     
CMR.CaseNumber,                  
CMR.CANNumber,                  
cc.Name_En,                  
cc.Name_Ar,                  
ccn.Number,                  
co.Name_En,                  
co.Name_Ar,                
ud.AttachmentTypeId,                
ud.StoragePath,                
ud.DocType,            
ud.FileName,            
ud.Description,            
ud.FileTitle,          
ud.CreatedDateTime,      
CT.Id      
ORDER BY ud.CreatedDateTime desc      
    
DROP TABLE #tmpCaseData    
DROP TABLE #tmpJudgmentData    
DROP TABLE #tmpExclude_ID    
END
----------------------------------------------------------------- 8-5-2024 -------------------

create table DSP_Request_Log (
LogId int primary key identity(1,1),
ExternalId nvarchar(max),
CivilId nvarchar(max),
DocumentId int,
Status bit,
ErrorCode nvarchar(max)
);

----------------------------------------15/5/24
GO
CREATE OR ALTER PROCEDURE pLLSLegalPrincipleSourceDocList                        
(                        
 @FileType INT = NULL,                    
 @FromDate NVARCHAR(30)=null,                                                          
 @ToDate NVARCHAR(30)=null,                    
 @CourtId INT =null,                             
 @JudgementTypeId INT = null ,                                            
 @ChamberId int=null,                                                  
 @ChamberNumberId INT = null,                                                  
 @CaseNumber NVARCHAR(2000) = NULL,                     
 @CANNumber NVARCHAR(2000) = NULL                      
)                        
AS                        
BEGIN          
--Common Data Exclusion          
SELECT ud.UploadedDocumentId,ud.FileName,ud.StoragePath, 0 AS IsJudgment INTO #tmpCaseData           
FROM DMS_DB_DEV.DBO.UPLOADED_DOCUMENT ud                        
JOIN FATWA_DB_DEV.DBO.CMS_REGISTERED_CASE crc on crc.CaseId = ud.ReferenceGuid AND AttachmentTypeId IN (60, 61, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110,111)            
          
SELECT ud.UploadedDocumentId,ud.FileName,ud.StoragePath, 1 AS IsJudgment  INTO #tmpJudgmentData            
FROM DMS_DB_DEV.DBO.UPLOADED_DOCUMENT ud                        
JOIN FATWA_DB_DEV.DBO.CMS_JUDGEMENT crcc on crcc.Id = ud.ReferenceGuid AND AttachmentTypeId IN (60, 61, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110,111)            
          
SELECT DISTINCT CD.UploadedDocumentId INTO #tmpExclude_ID FROM #tmpCaseData CD          
INNER JOIN #tmpJudgmentData JM ON JM.FileName = CD.FileName AND JM.StoragePath = CD.StoragePath          
--END          
SELECT            
ud.UploadedDocumentId,                        
'-' JudgementTypeEn,                        
'-' JudgementTypeAr,                        
crc.CreatedDate AS JudgementDate,        
0 AS IsJudgement,        
crc.CaseNumber,                        
crc.CANNumber,                        
cc.Name_En ChamberNameEn,                        
cc.Name_Ar ChamberNameAr,                        
ccn.Number ChamberNumber,                        
co.Name_En CourtEn,                        
co.Name_Ar CourtAr,                      
ud.AttachmentTypeId,                      
ud.StoragePath,                      
ud.DocType,                  
ud.FileName,                  
ud.Description,                  
ud.FileTitle,                
ud.CreatedDateTime,            
ct.Id CourtTypeId, 
COUNT(lpsd.OriginalSourceDocumentId) NumberOfPrinciple          
FROM DMS_DB_DEV.DBO.UPLOADED_DOCUMENT ud                        
JOIN FATWA_DB_DEV.DBO.CMS_REGISTERED_CASE crc on crc.CaseId = ud.ReferenceGuid AND AttachmentTypeId IN (60, 61, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110,111)                       
JOIN FATWA_DB_DEV.DBO.CMS_CHAMBER_G2G_LKP cc on cc.Id = crc.ChamberId AND (@ChamberId IS NULL OR @ChamberId = '' OR @ChamberId = cc.id)                    
JOIN FATWA_DB_DEV.DBO.CMS_COURT_G2G_LKP co on co.Id = crc.CourtId AND (@CourtId IS NULL OR @CourtId = '' OR @CourtId = co.Id)                 
JOIN FATWA_DB_DEV.DBO.CMS_COURT_TYPE_G2G_LKP ct on ct.Id = co.TypeId AND (co.TypeId IN (2, 4))            
JOIN FATWA_DB_DEV.DBO.CMS_CHAMBER_NUMBER_G2G_LKP ccn on ccn.Id = crc.ChamberNumberId AND (@ChamberNumberId IS NULL OR @ChamberNumberId = '' OR @ChamberNumberId = ccn.Id)                    
LEFT JOIN FATWA_DB_DEV.DBO.LLS_LEGAL_PRINCIPLE lpsd ON lpsd.OriginalSourceDocumentId = ud.UploadedDocumentId                
                
WHERE ud.UploadedDocumentId NOT IN (SELECT UploadedDocumentId FROM #tmpExclude_ID)                                        
AND(CAST(ud.CreatedDateTime as date)>=@FromDate OR @FromDate IS NULL OR @FromDate='')                                                          
AND (CAST(ud.CreatedDateTime as date)<=@ToDate OR @ToDate IS NULL OR @ToDate='')                                                  
AND (crc.CaseNumber=@CaseNumber OR @CaseNumber IS NULL OR @CaseNumber=N'')         
AND (crc.CANNumber=@CANNumber OR @CANNumber IS NULL OR @CANNumber=N'')                   
GROUP BY                
ud.UploadedDocumentId,                                     
crc.CaseNumber,                        
crc.CANNumber,               
cc.Name_En,                        
cc.Name_Ar,                        
ccn.Number,                        
co.Name_En,                        
co.Name_Ar,                    
ud.AttachmentTypeId,                      
ud.StoragePath,                      
ud.DocType,                  
ud.FileName,                  
ud.Description,                  
ud.FileTitle,                
ud.CreatedDateTime,            
CT.Id,  
crc.CreatedDate  
         
UNION          
           
SELECT             
ud.UploadedDocumentId,                        
ISNULL(JTS.NameEn,'-') AS JudgementTypeEn,                        
ISNULL(JTS.NameAr, '-') AS JudgementTypeAr,                        
cj.JudgementDate,         
1 AS IsJudgement,        
CMR.CaseNumber,                        
CMR.CANNumber,                        
cc.Name_En ChamberNameEn,                        
cc.Name_Ar ChamberNameAr,                        
ccn.Number ChamberNumber,                        
co.Name_En CourtEn,                        
co.Name_Ar CourtAr,                      
ud.AttachmentTypeId,                      
ud.StoragePath,                      
ud.DocType,                  
ud.FileName,                  
ud.Description,                  
ud.FileTitle,                
ud.CreatedDateTime,            
ct.Id CourtTypeId,            
COUNT(lpsd.OriginalSourceDocumentId) NumberOfPrinciple          
FROM DMS_DB_DEV.DBO.UPLOADED_DOCUMENT ud                        
JOIN FATWA_DB_DEV.DBO.CMS_JUDGEMENT cj on cj.Id = ud.ReferenceGuid  AND AttachmentTypeId IN (60, 61, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110,111)                      
 LEFT JOIN FATWA_DB_DEV.dbo.CMS_REGISTERED_CASE CMR ON CMR.CaseId = cj.CaseId          
 LEFT JOIN FATWA_DB_DEV.dbo.CMS_JUDGEMENT_STATUS_G2G_LKP JTS ON JTS.Id = cj.StatusId          
JOIN FATWA_DB_DEV.DBO.CMS_CHAMBER_G2G_LKP cc on cc.Id = CMR.ChamberId AND (@ChamberId IS NULL OR @ChamberId = '' OR @ChamberId = cc.id)                    
JOIN FATWA_DB_DEV.DBO.CMS_COURT_G2G_LKP co on co.Id = CMR.CourtId AND (@CourtId IS NULL OR @CourtId = '' OR @CourtId = co.Id)                 
JOIN FATWA_DB_DEV.DBO.CMS_COURT_TYPE_G2G_LKP ct on ct.Id = co.TypeId AND (co.TypeId IN (2, 4))            
JOIN FATWA_DB_DEV.DBO.CMS_CHAMBER_NUMBER_G2G_LKP ccn on ccn.Id = CMR.ChamberNumberId AND (@ChamberNumberId IS NULL OR @ChamberNumberId = '' OR @ChamberNumberId = ccn.Id)                    
LEFT JOIN FATWA_DB_DEV.DBO.LLS_LEGAL_PRINCIPLE lpsd ON lpsd.OriginalSourceDocumentId = ud.UploadedDocumentId                
                
WHERE                       
(CAST(ud.CreatedDateTime as date)>=@FromDate OR @FromDate IS NULL OR @FromDate='')                                                          
AND (CAST(ud.CreatedDateTime as date)<=@ToDate OR @ToDate IS NULL OR @ToDate='')                                                  
AND (CMR.CaseNumber=@CaseNumber OR @CaseNumber IS NULL OR @CaseNumber=N'')                    
AND (CMR.CANNumber=@CANNumber OR @CANNumber IS NULL OR @CANNumber=N'')           
AND (cj.StatusId=@JudgementTypeId OR @JudgementTypeId IS NULL OR @JudgementTypeId=N'')                   
        
GROUP BY                
ud.UploadedDocumentId,          
JTS.NameEn,                        
JTS.NameAr,                        
cj.JudgementDate,           
CMR.CaseNumber,                        
CMR.CANNumber,                        
cc.Name_En,                        
cc.Name_Ar,                        
ccn.Number,                        
co.Name_En,                        
co.Name_Ar,                      
ud.AttachmentTypeId,                      
ud.StoragePath,                      
ud.DocType,                  
ud.FileName,                  
ud.Description,                  
ud.FileTitle,                
ud.CreatedDateTime,            
CT.Id        
ORDER BY ud.CreatedDateTime desc            
          
DROP TABLE #tmpCaseData          
DROP TABLE #tmpJudgmentData          
DROP TABLE #tmpExclude_ID          
END

GO
CREATE OR ALTER PROCEDURE pLLSLegalPrincipleContentLinkedDocList                         
(       
 @PrincipleContentId UNIQUEIDENTIFIER = NULL,       
 @CopyDocumentId INT = NULL  
)                            
AS                            
BEGIN              
SELECT csd.OriginalSourceDocId, csd.CopySourceDocId, csd.PageNumber INTO #tempContentLinkDocuments         
FROM FATWA_DB_DEV.DBO.LLS_LEGAL_PRINCIPLE_CONTENT_SOURCE_DOCUMENT_REFERENCE csd         
WHERE (csd.PrincipleContentId = @PrincipleContentId OR @PrincipleContentId IS NULL)   
AND (csd.CopySourceDocId = @CopyDocumentId OR @CopyDocumentId IS NULL OR @CopyDocumentId = '')  
--Common Data Exclusion              
SELECT ud.UploadedDocumentId,ud.FileName,ud.StoragePath, 0 AS IsJudgment INTO #tmpCaseData               
FROM DMS_DB_DEV.DBO.UPLOADED_DOCUMENT ud                            
JOIN FATWA_DB_DEV.DBO.CMS_REGISTERED_CASE crc on crc.CaseId = ud.ReferenceGuid AND AttachmentTypeId IN (60, 61, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110,111)        
              
SELECT ud.UploadedDocumentId,ud.FileName,ud.StoragePath, 1 AS IsJudgment  INTO #tmpJudgmentData                
FROM DMS_DB_DEV.DBO.UPLOADED_DOCUMENT ud                            
JOIN FATWA_DB_DEV.DBO.CMS_JUDGEMENT crcc on crcc.Id = ud.ReferenceGuid AND AttachmentTypeId IN (60, 61, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110,111)                
              
SELECT DISTINCT CD.UploadedDocumentId INTO #tmpExclude_ID FROM #tmpCaseData CD              
INNER JOIN #tmpJudgmentData JM ON JM.FileName = CD.FileName AND JM.StoragePath = CD.StoragePath              
--END              
SELECT DISTINCT                           
ud2.UploadedDocumentId,                            
'-' JudgementTypeEn,                            
'-' JudgementTypeAr,                            
crc.CreatedDate AS JudgementDate,            
0 AS IsJudgement,            
crc.CaseNumber,                            
crc.CANNumber,                            
cc.Name_En ChamberNameEn,                            
cc.Name_Ar ChamberNameAr,                            
ccn.Number ChamberNumber,                            
co.Name_En CourtEn,                            
co.Name_Ar CourtAr,                          
ud.AttachmentTypeId,                          
ud2.StoragePath,                          
ud2.DocType,                      
ud2.FileName,                      
ud2.Description,                      
ud2.FileTitle,                    
ud2.CreatedDateTime,                
ct.Id CourtTypeId,          
TCD.PageNumber        
FROM DMS_DB_DEV.DBO.UPLOADED_DOCUMENT ud             
JOIN #tempContentLinkDocuments tcd on tcd.OriginalSourceDocId = ud.UploadedDocumentId        
JOIN DMS_DB_DEV.DBO.UPLOADED_DOCUMENT ud2 on tcd.CopySourceDocId = ud2.UploadedDocumentId        
JOIN FATWA_DB_DEV.DBO.CMS_REGISTERED_CASE crc on crc.CaseId = ud.ReferenceGuid AND ud.AttachmentTypeId IN (60, 61, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110,111)                           
JOIN FATWA_DB_DEV.DBO.CMS_CHAMBER_G2G_LKP cc on cc.Id = crc.ChamberId --AND (@ChamberId IS NULL OR @ChamberId = '' OR @ChamberId = cc.id)                        
JOIN FATWA_DB_DEV.DBO.CMS_COURT_G2G_LKP co on co.Id = crc.CourtId --AND (@CourtId IS NULL OR @CourtId = '' OR @CourtId = co.Id)                     
JOIN FATWA_DB_DEV.DBO.CMS_COURT_TYPE_G2G_LKP ct on ct.Id = co.TypeId AND (co.TypeId IN (2, 4))                
JOIN FATWA_DB_DEV.DBO.CMS_CHAMBER_NUMBER_G2G_LKP ccn on ccn.Id = crc.ChamberNumberId --AND (@ChamberNumberId IS NULL OR @ChamberNumberId = '' OR @ChamberNumberId = ccn.Id)                        
LEFT JOIN FATWA_DB_DEV.DBO.LLS_LEGAL_PRINCIPLE_CONTENT_SOURCE_DOCUMENT_REFERENCE lpsd ON lpsd.OriginalSourceDocId = ud.UploadedDocumentId                    
                    
WHERE        
ud.UploadedDocumentId NOT IN (SELECT UploadedDocumentId FROM #tmpExclude_ID)            
AND ud2.UploadedDocumentId in (select CopySourceDocId from #tempContentLinkDocuments)   
AND (ud2.UploadedDocumentId = tcd.CopySourceDocId OR @CopyDocumentId IS NULL OR @CopyDocumentId = '')  
             
UNION              
               
SELECT DISTINCT                 
ud2.UploadedDocumentId,                            
ISNULL(JTS.NameEn,'-') AS JudgementTypeEn,                            
ISNULL(JTS.NameAr, '-') AS JudgementTypeAr,                            
cj.JudgementDate,             
1 AS IsJudgement,            
CMR.CaseNumber,                            
CMR.CANNumber,                            
cc.Name_En ChamberNameEn,                            
cc.Name_Ar ChamberNameAr,                            
ccn.Number ChamberNumber,                            
co.Name_En CourtEn,                            
co.Name_Ar CourtAr,                          
ud.AttachmentTypeId,                          
ud2.StoragePath,                          
ud2.DocType,                      
ud2.FileName,                      
ud2.Description,                      
ud2.FileTitle,                    
ud2.CreatedDateTime,                
ct.Id CourtTypeId,          
TCD.PageNumber        
FROM DMS_DB_DEV.DBO.UPLOADED_DOCUMENT ud             
JOIN #tempContentLinkDocuments tcd on tcd.OriginalSourceDocId = ud.UploadedDocumentId        
JOIN DMS_DB_DEV.DBO.UPLOADED_DOCUMENT ud2 on tcd.CopySourceDocId = ud2.UploadedDocumentId        
JOIN FATWA_DB_DEV.DBO.CMS_JUDGEMENT cj on cj.Id = ud.ReferenceGuid  AND ud.AttachmentTypeId IN (60, 61, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110,111)                          
 LEFT JOIN FATWA_DB_DEV.dbo.CMS_REGISTERED_CASE CMR ON CMR.CaseId = cj.CaseId              
 LEFT JOIN FATWA_DB_DEV.dbo.CMS_JUDGEMENT_STATUS_G2G_LKP JTS ON JTS.Id = cj.StatusId              
JOIN FATWA_DB_DEV.DBO.CMS_CHAMBER_G2G_LKP cc on cc.Id = CMR.ChamberId 
JOIN FATWA_DB_DEV.DBO.CMS_COURT_G2G_LKP co on co.Id = CMR.CourtId 
JOIN FATWA_DB_DEV.DBO.CMS_COURT_TYPE_G2G_LKP ct on ct.Id = co.TypeId AND (co.TypeId IN (2, 4))                
JOIN FATWA_DB_DEV.DBO.CMS_CHAMBER_NUMBER_G2G_LKP ccn on ccn.Id = CMR.ChamberNumberId 
LEFT JOIN FATWA_DB_DEV.DBO.LLS_LEGAL_PRINCIPLE_CONTENT_SOURCE_DOCUMENT_REFERENCE lpsd ON lpsd.OriginalSourceDocId = ud.UploadedDocumentId                    
WHERE (ud2.UploadedDocumentId = tcd.CopySourceDocId OR @CopyDocumentId IS NULL OR @CopyDocumentId = '')  
ORDER BY ud2.CreatedDateTime desc                
              
DROP TABLE #tmpCaseData              
DROP TABLE #tmpJudgmentData              
DROP TABLE #tmpExclude_ID       
DROP TABLE #tempContentLinkDocuments    
END
GO


--- LLS Legal Principle 26-05-2024 start


GO
CREATE  OR ALTER   PROCEDURE pLLSLegalPrincipleContentLinkedDocList                               
(             
 @PrincipleContentId UNIQUEIDENTIFIER = NULL,             
 @CopyDocumentId INT = NULL        
)                                  
AS                                  
BEGIN                    
SELECT csd.OriginalSourceDocId, csd.CopySourceDocId, csd.PageNumber, csd.ReferenceId INTO #tempContentLinkDocuments               
FROM FATWA_DB_DEV.DBO.LLS_LEGAL_PRINCIPLE_CONTENT_SOURCE_DOCUMENT_REFERENCE csd               
WHERE (csd.PrincipleContentId = @PrincipleContentId OR @PrincipleContentId IS NULL)         
AND (csd.CopySourceDocId = @CopyDocumentId OR @CopyDocumentId IS NULL OR @CopyDocumentId = '')        
--Common Data Exclusion                    
SELECT ud.UploadedDocumentId,ud.FileName,ud.StoragePath, 0 AS IsJudgment INTO #tmpCaseData                     
FROM DMS_DB_DEV.DBO.UPLOADED_DOCUMENT ud                                  
JOIN FATWA_DB_DEV.DBO.CMS_REGISTERED_CASE crc on crc.CaseId = ud.ReferenceGuid AND AttachmentTypeId IN (60, 61, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110,111)              
                    
SELECT ud.UploadedDocumentId,ud.FileName,ud.StoragePath, 1 AS IsJudgment  INTO #tmpJudgmentData                      
FROM DMS_DB_DEV.DBO.UPLOADED_DOCUMENT ud                                  
JOIN FATWA_DB_DEV.DBO.CMS_JUDGEMENT crcc on crcc.Id = ud.ReferenceGuid AND AttachmentTypeId IN (60, 61, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110,111)                      
                    
SELECT DISTINCT CD.UploadedDocumentId INTO #tmpExclude_ID FROM #tmpCaseData CD                    
INNER JOIN #tmpJudgmentData JM ON JM.FileName = CD.FileName AND JM.StoragePath = CD.StoragePath                    
--END                    
SELECT DISTINCT                                 
ud2.UploadedDocumentId,                                  
'-' JudgementTypeEn,                                  
'-' JudgementTypeAr,                                  
crc.CreatedDate AS JudgementDate,                  
0 AS IsJudgement,                  
crc.CaseNumber,                                  
crc.CANNumber,                                  
cc.Name_En ChamberNameEn,                                  
cc.Name_Ar ChamberNameAr,                                  
ccn.Number ChamberNumber,                                  
co.Name_En CourtEn,                                  
co.Name_Ar CourtAr,                                
ud.AttachmentTypeId,                                
ud2.StoragePath,                                
ud2.DocType,                            
ud2.FileName,                            
ud2.Description,                            
ud2.FileTitle,                          
ud2.CreatedDateTime,                      
ct.Id CourtTypeId,                
TCD.PageNumber,  
TCD.ReferenceId  
FROM DMS_DB_DEV.DBO.UPLOADED_DOCUMENT ud                   
JOIN #tempContentLinkDocuments tcd on tcd.OriginalSourceDocId = ud.UploadedDocumentId              
JOIN DMS_DB_DEV.DBO.UPLOADED_DOCUMENT ud2 on tcd.CopySourceDocId = ud2.UploadedDocumentId              
JOIN FATWA_DB_DEV.DBO.CMS_REGISTERED_CASE crc on crc.CaseId = ud.ReferenceGuid AND ud.AttachmentTypeId IN (60, 61, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110,111)                                 
JOIN FATWA_DB_DEV.DBO.CMS_CHAMBER_G2G_LKP cc on cc.Id = crc.ChamberId --AND (@ChamberId IS NULL OR @ChamberId = '' OR @ChamberId = cc.id)                              
JOIN FATWA_DB_DEV.DBO.CMS_COURT_G2G_LKP co on co.Id = crc.CourtId --AND (@CourtId IS NULL OR @CourtId = '' OR @CourtId = co.Id)                           
JOIN FATWA_DB_DEV.DBO.CMS_COURT_TYPE_G2G_LKP ct on ct.Id = co.TypeId AND (co.TypeId IN (2, 4))                      
JOIN FATWA_DB_DEV.DBO.CMS_CHAMBER_NUMBER_G2G_LKP ccn on ccn.Id = crc.ChamberNumberId --AND (@ChamberNumberId IS NULL OR @ChamberNumberId = '' OR @ChamberNumberId = ccn.Id)                              
LEFT JOIN FATWA_DB_DEV.DBO.LLS_LEGAL_PRINCIPLE_CONTENT_SOURCE_DOCUMENT_REFERENCE lpsd ON lpsd.OriginalSourceDocId = ud.UploadedDocumentId                          
                          
WHERE              
ud.UploadedDocumentId NOT IN (SELECT UploadedDocumentId FROM #tmpExclude_ID)                  
AND ud2.UploadedDocumentId in (select CopySourceDocId from #tempContentLinkDocuments)         
AND (ud2.UploadedDocumentId = tcd.CopySourceDocId OR @CopyDocumentId IS NULL OR @CopyDocumentId = '')        
                   
UNION                    
                     
SELECT DISTINCT                       
ud2.UploadedDocumentId,                                  
ISNULL(JTS.NameEn,'-') AS JudgementTypeEn,                                  
ISNULL(JTS.NameAr, '-') AS JudgementTypeAr,                                  
cj.JudgementDate,                   
1 AS IsJudgement,                  
CMR.CaseNumber,                                  
CMR.CANNumber,                                  
cc.Name_En ChamberNameEn,                                  
cc.Name_Ar ChamberNameAr,                                  
ccn.Number ChamberNumber,                                  
co.Name_En CourtEn,                                  
co.Name_Ar CourtAr,                                
ud.AttachmentTypeId,                                
ud2.StoragePath,                                
ud2.DocType,                            
ud2.FileName,                            
ud2.Description,                            
ud2.FileTitle,                          
ud2.CreatedDateTime,                      
ct.Id CourtTypeId,                
TCD.PageNumber,  
TCD.ReferenceId             
FROM DMS_DB_DEV.DBO.UPLOADED_DOCUMENT ud                   
JOIN #tempContentLinkDocuments tcd on tcd.OriginalSourceDocId = ud.UploadedDocumentId              
JOIN DMS_DB_DEV.DBO.UPLOADED_DOCUMENT ud2 on tcd.CopySourceDocId = ud2.UploadedDocumentId              
JOIN FATWA_DB_DEV.DBO.CMS_JUDGEMENT cj on cj.Id = ud.ReferenceGuid  AND ud.AttachmentTypeId IN (60, 61, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110,111)                                
 LEFT JOIN FATWA_DB_DEV.dbo.CMS_REGISTERED_CASE CMR ON CMR.CaseId = cj.CaseId                    
 LEFT JOIN FATWA_DB_DEV.dbo.CMS_JUDGEMENT_STATUS_G2G_LKP JTS ON JTS.Id = cj.StatusId                    
JOIN FATWA_DB_DEV.DBO.CMS_CHAMBER_G2G_LKP cc on cc.Id = CMR.ChamberId       
JOIN FATWA_DB_DEV.DBO.CMS_COURT_G2G_LKP co on co.Id = CMR.CourtId       
JOIN FATWA_DB_DEV.DBO.CMS_COURT_TYPE_G2G_LKP ct on ct.Id = co.TypeId AND (co.TypeId IN (2, 4))                      
JOIN FATWA_DB_DEV.DBO.CMS_CHAMBER_NUMBER_G2G_LKP ccn on ccn.Id = CMR.ChamberNumberId       
LEFT JOIN FATWA_DB_DEV.DBO.LLS_LEGAL_PRINCIPLE_CONTENT_SOURCE_DOCUMENT_REFERENCE lpsd ON lpsd.OriginalSourceDocId = ud.UploadedDocumentId                          
WHERE (ud2.UploadedDocumentId = tcd.CopySourceDocId OR @CopyDocumentId IS NULL OR @CopyDocumentId = '')        
ORDER BY ud2.CreatedDateTime desc                      
                    
DROP TABLE #tmpCaseData                    
DROP TABLE #tmpJudgmentData                    
DROP TABLE #tmpExclude_ID             
DROP TABLE #tempContentLinkDocuments          
END

--- LLS Legal Principle 26-05-2024 end

------------------------------LLS Legal Principle 30/5/24---------Latest all

INSERT INTO [dbo].[ATTACHMENT_TYPE]
           ([AttachmentTypeId],[Type_Ar],[Type_En],[ModuleId],[IsMandatory],[IsOfficialLetter],[SubTypeId],[IsGePortalType],[IsOpinion],[CreatedBy],[CreatedDate],[IsDeleted],[IsActive],[IsSystemDefine],[Description],[IsMojExtracted])
     VALUES (112 ,N'أخرى','Other', 2, 0, 0, NULL, 0, 0,'fatwaadmin@gmail.com',GETDATE(),0,1,1,NULL,0)

GO
CREATE OR ALTER PROCEDURE pLLSKuwaitAlYoumContentLinkedDocuments         
(        
 @PrincipleContentId UNIQUEIDENTIFIER = NULL        
)        
AS            
BEGIN            
SELECT             
ud2.UploadedDocumentId,                                                  
ud2.CreatedDateTime AS CreatedDate,                                  
ud2.AttachmentTypeId,                                    
ud2.StoragePath,                                    
ud2.DocType,                                
ud2.FileName,          
ud2.FileTitle,        
ud2.Description,  
kp.EditionNumber,  
kp.PublicationDateHijri,  
kp.EditionType,  
kp.PublicationDate,  
kp.DocumentTitle,  
pcsd.PageNumber,        
pcsd.ReferenceId,      
pcsd.CopySourceDocId,      
pcsd.OriginalSourceDocId,      
pcsd.IsMaskedJudgment       
FROM KAY_PUBLICATION_STG kp        
JOIN FATWA_DB_DEV.DBO.LLS_LEGAL_PRINCIPLE_CONTENT_SOURCE_DOCUMENT_REFERENCE pcsd on pcsd.OriginalSourceDocId = kp.Id       
JOIN DMS_DB_DEV.DBO.UPLOADED_DOCUMENT ud2 on pcsd.CopySourceDocId = ud2.UploadedDocumentId AND ud2.AttachmentTypeId = 23       
WHERE pcsd.PrincipleContentId = @PrincipleContentId        
AND pcsd.IsDeleted = 0    
END 

GO
CREATE OR ALTER PROCEDURE pLLSKuwaitAlYoumSourceDocuments                                
(       
 @EditionNumber nvarchar(500) =null,                 
 @EditionType nvarchar(500)=null,                
 @DocumentTitle nvarchar(500)=null,               
 @PublicationDateHijri nvarchar(500)=null,                
 @PublicationFrom datetime=null,                                    
 @PublicationTo datetime=null  
)                                    
AS                            
BEGIN            
    
Select                 
kp.Id UploadedDocumentId,                
kp.EditionNumber,                
kp.EditionType,                
kp.FileTitle,                
kp.DocumentTitle,                
kp.PublicationDate,              
kp.PublicationDateHijri,                
kp.StoragePath,                
kp.CreatedDate,     
RIGHT(Kp.StoragePath, CHARINDEX('.', REVERSE(StoragePath)) - 0) AS DocType    
From KAY_PUBLICATION_STG kp                  
where   
KP.IsFullEdition = 0  
AND (kp.EditionNumber like '%' + @EditionNumber + '%' OR @EditionNumber IS NULL OR @EditionNumber = '')            
AND (kp.EditionType like '%' + @EditionType + '%' OR @EditionType IS NULL OR @EditionType = '')                 
AND (kp.DocumentTitle like '%' + @DocumentTitle + '%' OR @DocumentTitle IS NULL OR @DocumentTitle = '')           
AND (kp.PublicationDateHijri like '%' + @PublicationDateHijri + '%' OR @PublicationDateHijri IS NULL OR @PublicationDateHijri = '')                 
AND(CAST(kp.PublicationDate as date)>=@PublicationFrom OR @PublicationFrom IS NULL OR @PublicationFrom='')                                    
AND (CAST(kp.PublicationDate as date)<=@PublicationTo OR @PublicationTo IS NULL OR @PublicationTo='')                
ORDER BY kp.PublicationDate DESC                              
END 

GO
CREATE OR ALTER PROCEDURE pLLSLegalAdviceContentLinkedDocuments       
(      
 @PrincipleContentId UNIQUEIDENTIFIER = NULL      
)      
AS          
BEGIN          
SELECT           
ud2.UploadedDocumentId,                                                
ud2.CreatedDateTime AS CreatedDate,                                
ud2.AttachmentTypeId,                                  
ud2.StoragePath,                                  
ud2.DocType,                              
ud2.FileName,        
ud2.FileTitle,      
ud2.Description,                              
ccf.FileName FinalDocFileName,          
ccf.FileNumber,      
pcsd.PageNumber,      
pcsd.ReferenceId,    
pcsd.CopySourceDocId,    
pcsd.OriginalSourceDocId,    
pcsd.IsMaskedJudgment     
FROM UPLOADED_DOCUMENT ud      
JOIN FATWA_DB_DEV.DBO.LLS_LEGAL_PRINCIPLE_CONTENT_SOURCE_DOCUMENT_REFERENCE pcsd on pcsd.OriginalSourceDocId = ud.UploadedDocumentId AND ud.AttachmentTypeId = 63       
JOIN DMS_DB_DEV.DBO.UPLOADED_DOCUMENT ud2 on pcsd.CopySourceDocId = ud2.UploadedDocumentId AND ud2.AttachmentTypeId = 63       
JOIN FATWA_DB_DEV.DBO.COMS_CONSULTATION_FILE ccf on ccf.FileId = ud.ReferenceGuid         
WHERE pcsd.PrincipleContentId = @PrincipleContentId      
AND pcsd.IsDeleted = 0  
END 

GO
CREATE OR ALTER PROCEDURE pLLSLegalPrincipleContentLinkedDocList    
(                   
 @PrincipleContentId UNIQUEIDENTIFIER = NULL,                   
 @CopyDocumentId INT = NULL              
)                                        
AS                                        
BEGIN                          
SELECT csd.OriginalSourceDocId, csd.CopySourceDocId,csd.IsMaskedJudgment, csd.PageNumber, csd.ReferenceId INTO #tempContentLinkDocuments                
FROM FATWA_DB_DEV.dbo.LLS_LEGAL_PRINCIPLE_CONTENT_SOURCE_DOCUMENT_REFERENCE csd                     
WHERE (csd.PrincipleContentId = @PrincipleContentId OR @PrincipleContentId IS NULL)   
AND csd.IsDeleted = 0  
AND (csd.CopySourceDocId = @CopyDocumentId OR @CopyDocumentId IS NULL OR @CopyDocumentId = '')              
--Common Data Exclusion                          
SELECT ud.UploadedDocumentId,ud.FileName,ud.StoragePath, 0 AS IsJudgment INTO #tmpCaseData                           
FROM UPLOADED_DOCUMENT ud                                        
JOIN FATWA_DB_DEV.dbo.CMS_REGISTERED_CASE crc on crc.CaseId = ud.ReferenceGuid AND AttachmentTypeId IN (60, 61, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110,111)                    
                          
SELECT ud.UploadedDocumentId,ud.FileName,ud.StoragePath, 1 AS IsJudgment  INTO #tmpJudgmentData                            
FROM UPLOADED_DOCUMENT ud                                        
JOIN FATWA_DB_DEV.dbo.CMS_JUDGEMENT crcc on crcc.Id = ud.ReferenceGuid AND AttachmentTypeId IN (60, 61, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110,111)                            
                          
SELECT DISTINCT CD.UploadedDocumentId INTO #tmpExclude_ID FROM #tmpCaseData CD                          
INNER JOIN #tmpJudgmentData JM ON JM.FileName = CD.FileName AND JM.StoragePath = CD.StoragePath                          
--END                          
SELECT DISTINCT                                       
ud2.UploadedDocumentId,                                        
'-' JudgementTypeEn,                                        
'-' JudgementTypeAr,                                        
crc.CreatedDate AS JudgementDate,                        
0 AS IsJudgement,                        
crc.CaseNumber,                                        
crc.CANNumber,                                        
cc.Name_En ChamberNameEn,                                        
cc.Name_Ar ChamberNameAr,                                        
ccn.Number ChamberNumber,                                        
co.Name_En CourtEn,                                        
co.Name_Ar CourtAr,                                      
ud.AttachmentTypeId,                                      
ud2.StoragePath,                                      
ud2.DocType,                                  
ud2.FileName,                                  
ud2.Description,                                  
ud2.FileTitle,                                
ud2.CreatedDateTime,                            
ct.Id CourtTypeId,                      
TCD.PageNumber,        
TCD.ReferenceId,    
tcd.CopySourceDocId,    
tcd.OriginalSourceDocId,    
tcd.IsMaskedJudgment    
FROM UPLOADED_DOCUMENT ud                         
JOIN #tempContentLinkDocuments tcd on tcd.OriginalSourceDocId = ud.UploadedDocumentId                    
JOIN UPLOADED_DOCUMENT ud2 on tcd.CopySourceDocId = ud2.UploadedDocumentId                    
JOIN FATWA_DB_DEV.dbo.CMS_REGISTERED_CASE crc on crc.CaseId = ud.ReferenceGuid AND ud.AttachmentTypeId IN (60, 61, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110,111)                                       
JOIN FATWA_DB_DEV.dbo.CMS_CHAMBER_G2G_LKP cc on cc.Id = crc.ChamberId --AND (@ChamberId IS NULL OR @ChamberId = '' OR @ChamberId = cc.id)                                    
JOIN FATWA_DB_DEV.dbo.CMS_COURT_G2G_LKP co on co.Id = crc.CourtId --AND (@CourtId IS NULL OR @CourtId = '' OR @CourtId = co.Id)                                 
JOIN FATWA_DB_DEV.dbo.CMS_COURT_TYPE_G2G_LKP ct on ct.Id = co.TypeId AND (co.TypeId IN (2, 4))                            
JOIN FATWA_DB_DEV.dbo.CMS_CHAMBER_NUMBER_G2G_LKP ccn on ccn.Id = crc.ChamberNumberId --AND (@ChamberNumberId IS NULL OR @ChamberNumberId = '' OR @ChamberNumberId = ccn.Id)                                    
LEFT JOIN FATWA_DB_DEV.dbo.LLS_LEGAL_PRINCIPLE_CONTENT_SOURCE_DOCUMENT_REFERENCE lpsd ON lpsd.OriginalSourceDocId = ud.UploadedDocumentId                                
                                
WHERE                    
ud.UploadedDocumentId NOT IN (SELECT UploadedDocumentId FROM #tmpExclude_ID)                        
AND ud2.UploadedDocumentId in (select CopySourceDocId from #tempContentLinkDocuments)               
AND (ud2.UploadedDocumentId = tcd.CopySourceDocId OR @CopyDocumentId IS NULL OR @CopyDocumentId = '')              
                         
UNION                          
                           
SELECT DISTINCT                             
ud2.UploadedDocumentId,                                        
ISNULL(JTS.NameEn,'-') AS JudgementTypeEn,                                        
ISNULL(JTS.NameAr, '-') AS JudgementTypeAr,                                        
cj.JudgementDate,                         
1 AS IsJudgement,                        
CMR.CaseNumber,                                        
CMR.CANNumber,                                        
cc.Name_En ChamberNameEn,                                        
cc.Name_Ar ChamberNameAr,                                        
ccn.Number ChamberNumber,                                        
co.Name_En CourtEn,                                        
co.Name_Ar CourtAr,                                      
ud.AttachmentTypeId,                                      
ud2.StoragePath,                                      
ud2.DocType,                                  
ud2.FileName,                                  
ud2.Description,                                  
ud2.FileTitle,                                
ud2.CreatedDateTime,                            
ct.Id CourtTypeId,                      
TCD.PageNumber,        
TCD.ReferenceId,    
tcd.CopySourceDocId,    
tcd.OriginalSourceDocId,    
tcd.IsMaskedJudgment    
FROM UPLOADED_DOCUMENT ud                         
JOIN #tempContentLinkDocuments tcd on tcd.OriginalSourceDocId = ud.UploadedDocumentId                    
JOIN UPLOADED_DOCUMENT ud2 on tcd.CopySourceDocId = ud2.UploadedDocumentId                    
JOIN FATWA_DB_DEV.dbo.CMS_JUDGEMENT cj on cj.Id = ud.ReferenceGuid  AND ud.AttachmentTypeId IN (60, 61, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110,111)                                      
 LEFT JOIN FATWA_DB_DEV.dbo.CMS_REGISTERED_CASE CMR ON CMR.CaseId = cj.CaseId                          
 LEFT JOIN FATWA_DB_DEV.dbo.CMS_JUDGEMENT_STATUS_G2G_LKP JTS ON JTS.Id = cj.StatusId                          
JOIN FATWA_DB_DEV.dbo.CMS_CHAMBER_G2G_LKP cc on cc.Id = CMR.ChamberId             
JOIN FATWA_DB_DEV.dbo.CMS_COURT_G2G_LKP co on co.Id = CMR.CourtId             
JOIN FATWA_DB_DEV.dbo.CMS_COURT_TYPE_G2G_LKP ct on ct.Id = co.TypeId AND (co.TypeId IN (2, 4))                            
JOIN FATWA_DB_DEV.dbo.CMS_CHAMBER_NUMBER_G2G_LKP ccn on ccn.Id = CMR.ChamberNumberId             
LEFT JOIN FATWA_DB_DEV.dbo.LLS_LEGAL_PRINCIPLE_CONTENT_SOURCE_DOCUMENT_REFERENCE lpsd ON lpsd.OriginalSourceDocId = ud.UploadedDocumentId                                
WHERE (ud2.UploadedDocumentId = tcd.CopySourceDocId OR @CopyDocumentId IS NULL OR @CopyDocumentId = '')              
ORDER BY ud2.CreatedDateTime desc                            
                          
DROP TABLE #tmpCaseData                          
DROP TABLE #tmpJudgmentData                          
DROP TABLE #tmpExclude_ID                   
DROP TABLE #tempContentLinkDocuments                
END  

GO
CREATE OR ALTER PROCEDURE pLLSLegalPrincipleLegalAdviceDocuments   
(  
 @FromDate NVARCHAR(30)=null,                                                                
 @ToDate NVARCHAR(30)=null   
)  
AS        
BEGIN        
SELECT         
ud.UploadedDocumentId,                                              
ud.CreatedDateTime AS CreatedDate,                              
ud.AttachmentTypeId,                                
ud.StoragePath,                                
ud.DocType,                            
ud.FileName,                            
ud.Description,                            
ud.CreatedDateTime,                      
ccf.FileName FinalDocFileName,        
ccf.FileNumber,        
COUNT(lp.OriginalSourceDocumentId) NoOfPrinciples        
FROM DMS_DB_DEV.dbo.UPLOADED_DOCUMENT ud        
JOIN FATWA_DB_DEV.DBO.COMS_CONSULTATION_FILE ccf on ccf.FileId = ud.ReferenceGuid AND ud.AttachmentTypeId = 63      
LEFT JOIN FATWA_DB_DEV.DBO.LLS_LEGAL_PRINCIPLE lp on lp.OriginalSourceDocumentId = ud.UploadedDocumentId   
WHERE  
(CAST(ud.CreatedDateTime as date)>=@FromDate OR @FromDate IS NULL OR @FromDate='')                                                                
AND (CAST(ud.CreatedDateTime as date)<=@ToDate OR @ToDate IS NULL OR @ToDate='')    
GROUP BY     
ud.UploadedDocumentId,                                              
ud.CreatedDateTime,                              
ud.AttachmentTypeId,                                
ud.StoragePath,                                
ud.DocType,                            
ud.FileName,                            
ud.Description,                            
ud.CreatedDateTime,                      
ccf.FileName,        
ccf.FileNumber     
order by ud.CreatedDateTime desc     
END    

GO
CREATE OR ALTER PROCEDURE pLLSLegalPrincipleOtherDocuments    
(  
 @FromDate NVARCHAR(30)=null,                                                                
 @ToDate NVARCHAR(30)=null   
)  
AS          
BEGIN          
SELECT      
ud.UploadedDocumentId,           
ud.DocumentDate,      
ud.CreatedDateTime AS CreatedDate,                                
ud.AttachmentTypeId,                                  
ud.StoragePath,                                  
ud.DocType,                              
ud.FileName,                              
ud.Description,                              
ud.OtherAttachmentType,    
COUNT(lp.OriginalSourceDocumentId) NoOfPrinciples          
FROM DMS_DB_DEV.dbo.UPLOADED_DOCUMENT ud    
LEFT JOIN FATWA_DB_DEV.DBO.LLS_LEGAL_PRINCIPLE lp on lp.OriginalSourceDocumentId = ud.UploadedDocumentId     
WHERE AttachmentTypeId = 112     
AND ud.UploadedDocumentId not in (select CopySourceDocId from FATWA_DB_DEV.DBO.LLS_LEGAL_PRINCIPLE_CONTENT_SOURCE_DOCUMENT_REFERENCE)    
AND (CAST(ud.CreatedDateTime as date)>=@FromDate OR @FromDate IS NULL OR @FromDate='')                                                                
AND (CAST(ud.CreatedDateTime as date)<=@ToDate OR @ToDate IS NULL OR @ToDate='')    
GROUP BY     
ud.UploadedDocumentId,           
ud.DocumentDate,      
ud.CreatedDateTime,                                
ud.AttachmentTypeId,                                  
ud.StoragePath,                                  
ud.DocType,                              
ud.FileName,                              
ud.Description,                              
ud.OtherAttachmentType      
order by ud.CreatedDateTime desc        
END

GO
CREATE OR ALTER PROCEDURE pLLSLegalPrincipleSourceDocList                              
(                              
 @FileType INT = NULL,                          
 @FromDate NVARCHAR(30)=null,                                                                
 @ToDate NVARCHAR(30)=null,                          
 @CourtId INT =null,                                   
 @JudgementTypeId INT = null ,                                                  
 @ChamberId int=null,                                                        
 @ChamberNumberId INT = null,                                                        
 @CaseNumber NVARCHAR(2000) = NULL,                           
 @CANNumber NVARCHAR(2000) = NULL                            
)                              
AS                              
BEGIN                
--Common Data Exclusion                
SELECT ud.UploadedDocumentId,ud.FileName,ud.StoragePath, 0 AS IsJudgment INTO #tmpCaseData                 
FROM DMS_DB_DEV.DBO.UPLOADED_DOCUMENT ud                              
JOIN FATWA_DB_DEV.DBO.CMS_REGISTERED_CASE crc on crc.CaseId = ud.ReferenceGuid AND AttachmentTypeId IN (60, 61, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110,111)                  
                
SELECT ud.UploadedDocumentId,ud.FileName,ud.StoragePath, 1 AS IsJudgment  INTO #tmpJudgmentData                  
FROM DMS_DB_DEV.DBO.UPLOADED_DOCUMENT ud                              
JOIN FATWA_DB_DEV.DBO.CMS_JUDGEMENT crcc on crcc.Id = ud.ReferenceGuid AND AttachmentTypeId IN (60, 61, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110,111)                  
                
SELECT DISTINCT CD.UploadedDocumentId INTO #tmpExclude_ID FROM #tmpCaseData CD                
INNER JOIN #tmpJudgmentData JM ON JM.FileName = CD.FileName AND JM.StoragePath = CD.StoragePath                
--END                
SELECT                  
ud.UploadedDocumentId,                              
'-' JudgementTypeEn,                              
'-' JudgementTypeAr,                              
crc.CreatedDate AS JudgementDate,              
0 AS IsJudgement,              
crc.CaseNumber,                              
crc.CANNumber,                              
cc.Name_En ChamberNameEn,                              
cc.Name_Ar ChamberNameAr,                              
ccn.Number ChamberNumber,                              
co.Name_En CourtEn,                              
co.Name_Ar CourtAr,                            
ud.AttachmentTypeId,                            
ud.StoragePath,                            
ud.DocType,                        
ud.FileName,                        
ud.Description,                        
ud.FileTitle,                      
ud.CreatedDateTime,                  
ct.Id CourtTypeId,       
COUNT(lpsd.OriginalSourceDocumentId) NumberOfPrinciple                
FROM DMS_DB_DEV.DBO.UPLOADED_DOCUMENT ud                              
JOIN FATWA_DB_DEV.DBO.CMS_REGISTERED_CASE crc on crc.CaseId = ud.ReferenceGuid AND AttachmentTypeId IN (60, 61, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110,111)                             
JOIN FATWA_DB_DEV.DBO.CMS_CHAMBER_G2G_LKP cc on cc.Id = crc.ChamberId AND (@ChamberId IS NULL OR @ChamberId = '' OR @ChamberId = cc.id)                          
JOIN FATWA_DB_DEV.DBO.CMS_COURT_G2G_LKP co on co.Id = crc.CourtId AND (@CourtId IS NULL OR @CourtId = '' OR @CourtId = co.Id)                       
JOIN FATWA_DB_DEV.DBO.CMS_COURT_TYPE_G2G_LKP ct on ct.Id = co.TypeId AND (co.TypeId IN (2, 4))                  
JOIN FATWA_DB_DEV.DBO.CMS_CHAMBER_NUMBER_G2G_LKP ccn on ccn.Id = crc.ChamberNumberId AND (@ChamberNumberId IS NULL OR @ChamberNumberId = '' OR @ChamberNumberId = ccn.Id)                          
LEFT JOIN FATWA_DB_DEV.DBO.LLS_LEGAL_PRINCIPLE lpsd ON lpsd.OriginalSourceDocumentId = ud.UploadedDocumentId                      
                      
WHERE ud.UploadedDocumentId NOT IN (SELECT UploadedDocumentId FROM #tmpExclude_ID)                                              
AND(CAST(crc.CreatedDate as date)>=@FromDate OR @FromDate IS NULL OR @FromDate='')                                                                
AND (CAST(crc.CreatedDate as date)<=@ToDate OR @ToDate IS NULL OR @ToDate='')          
AND (crc.CaseNumber=@CaseNumber OR @CaseNumber IS NULL OR @CaseNumber=N'')               
AND (crc.CANNumber=@CANNumber OR @CANNumber IS NULL OR @CANNumber=N'')                         
GROUP BY                      
ud.UploadedDocumentId,                                           
crc.CaseNumber,                              
crc.CANNumber,                     
cc.Name_En,                              
cc.Name_Ar,                              
ccn.Number,                              
co.Name_En,                              
co.Name_Ar,                          
ud.AttachmentTypeId,                            
ud.StoragePath,                            
ud.DocType,                        
ud.FileName,                        
ud.Description,                        
ud.FileTitle,                      
ud.CreatedDateTime,                  
CT.Id,        
crc.CreatedDate        
               
UNION                
                 
SELECT                   
ud.UploadedDocumentId,                              
ISNULL(JTS.NameEn,'-') AS JudgementTypeEn,                              
ISNULL(JTS.NameAr, '-') AS JudgementTypeAr,                              
cj.JudgementDate,               
1 AS IsJudgement,              
CMR.CaseNumber,                              
CMR.CANNumber,                              
cc.Name_En ChamberNameEn,                              
cc.Name_Ar ChamberNameAr,                              
ccn.Number ChamberNumber,                              
co.Name_En CourtEn,                              
co.Name_Ar CourtAr,                            
ud.AttachmentTypeId,                            
ud.StoragePath,                            
ud.DocType,                        
ud.FileName,                        
ud.Description,                        
ud.FileTitle,                      
ud.CreatedDateTime,                  
ct.Id CourtTypeId,                  
COUNT(lpsd.OriginalSourceDocumentId) NumberOfPrinciple                
FROM DMS_DB_DEV.DBO.UPLOADED_DOCUMENT ud                              
JOIN FATWA_DB_DEV.DBO.CMS_JUDGEMENT cj on cj.Id = ud.ReferenceGuid  AND AttachmentTypeId IN (60, 61, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110,111)                            
 LEFT JOIN FATWA_DB_DEV.dbo.CMS_REGISTERED_CASE CMR ON CMR.CaseId = cj.CaseId                
 LEFT JOIN FATWA_DB_DEV.dbo.CMS_JUDGEMENT_STATUS_G2G_LKP JTS ON JTS.Id = cj.StatusId                
JOIN FATWA_DB_DEV.DBO.CMS_CHAMBER_G2G_LKP cc on cc.Id = CMR.ChamberId AND (@ChamberId IS NULL OR @ChamberId = '' OR @ChamberId = cc.id)                          
JOIN FATWA_DB_DEV.DBO.CMS_COURT_G2G_LKP co on co.Id = CMR.CourtId AND (@CourtId IS NULL OR @CourtId = '' OR @CourtId = co.Id)                       
JOIN FATWA_DB_DEV.DBO.CMS_COURT_TYPE_G2G_LKP ct on ct.Id = co.TypeId AND (co.TypeId IN (2, 4))                  
JOIN FATWA_DB_DEV.DBO.CMS_CHAMBER_NUMBER_G2G_LKP ccn on ccn.Id = CMR.ChamberNumberId AND (@ChamberNumberId IS NULL OR @ChamberNumberId = '' OR @ChamberNumberId = ccn.Id)                          
LEFT JOIN FATWA_DB_DEV.DBO.LLS_LEGAL_PRINCIPLE lpsd ON lpsd.OriginalSourceDocumentId = ud.UploadedDocumentId                      
                      
WHERE                             
(CAST(ud.CreatedDateTime as date)>=@FromDate OR @FromDate IS NULL OR @FromDate='')                                                                
AND (CAST(ud.CreatedDateTime as date)<=@ToDate OR @ToDate IS NULL OR @ToDate='')                                                        
AND (CMR.CaseNumber=@CaseNumber OR @CaseNumber IS NULL OR @CaseNumber=N'')                          
AND (CMR.CANNumber=@CANNumber OR @CANNumber IS NULL OR @CANNumber=N'')                 
AND (cj.StatusId=@JudgementTypeId OR @JudgementTypeId IS NULL OR @JudgementTypeId=N'')                         
              
GROUP BY                      
ud.UploadedDocumentId,                
JTS.NameEn,                              
JTS.NameAr,                              
cj.JudgementDate,                 
CMR.CaseNumber,                              
CMR.CANNumber,                              
cc.Name_En,                              
cc.Name_Ar,                              
ccn.Number,                              
co.Name_En,                              
co.Name_Ar,                            
ud.AttachmentTypeId,            
ud.StoragePath,                            
ud.DocType,                        
ud.FileName,                        
ud.Description,                        
ud.FileTitle,                      
ud.CreatedDateTime,                  
CT.Id              
ORDER BY ud.CreatedDateTime desc                  
                
DROP TABLE #tmpCaseData                
DROP TABLE #tmpJudgmentData                
DROP TABLE #tmpExclude_ID                
END 

GO
CREATE OR ALTER PROCEDURE pLLSOtherContentLinkedDocuments       
(      
 @PrincipleContentId UNIQUEIDENTIFIER = NULL      
)      
AS          
BEGIN          
SELECT           
ud2.UploadedDocumentId,          
ud.DocumentDate,    
ud2.CreatedDateTime AS CreatedDate,                                
ud2.AttachmentTypeId,         
ud2.OtherAttachmentType,    
ud2.StoragePath,                                  
ud2.DocType,                              
ud2.FileName,        
ud2.FileTitle,      
ud2.Description,     
pcsd.PageNumber,    
pcsd.CopySourceDocId,    
pcsd.OriginalSourceDocId,    
pcsd.IsMaskedJudgment,    
pcsd.ReferenceId      
FROM UPLOADED_DOCUMENT ud      
JOIN FATWA_DB_DEV.DBO.LLS_LEGAL_PRINCIPLE_CONTENT_SOURCE_DOCUMENT_REFERENCE pcsd on pcsd.OriginalSourceDocId = ud.UploadedDocumentId AND ud.AttachmentTypeId = 112       
JOIN DMS_DB_DEV.DBO.UPLOADED_DOCUMENT ud2 on pcsd.CopySourceDocId = ud2.UploadedDocumentId AND ud2.AttachmentTypeId = 112       
WHERE pcsd.PrincipleContentId = @PrincipleContentId   
AND pcsd.IsDeleted = 0  
END   
GO
-------------------------------------------END-------------------------------------------------------
----------------------------------------------23/6/24------------------------------------

INSERT [dbo].[ATTACHMENT_TYPE] ([AttachmentTypeId], [Type_Ar], [Type_En], [ModuleId], [IsMandatory],[IsOfficialLetter]) VALUES (114, N'الشكوى', N'Complaint', 18, 0,0)
-----------------------------END--------------------------------

------------------- 11-6-2024 --------------------------
create table DSP_Athentication_Requests(
AuthenticationLogId int primary key identity(1,1),
RequestId nvarchar(150),
ErrorCode nvarchar(max),
RequestPayload nvarchar(max),
ResponsePayload nvarchar(max),
)

------------------- 11-6-2024 --------------------------
------------------- 25-6-2024 --------------------------

Alter table DSP_Request_Log 
add RequestId varchar(100) ,
RequestStatus varchar(50)
------------------- 25-6-2024 --------------------------


---- Vendor And Contract
go
INSERT INTO [dbo].[ATTACHMENT_TYPE]
           ([AttachmentTypeId], [Type_Ar], [Type_En], [ModuleId], [IsMandatory], [IsOfficialLetter], [IsGePortalType], [IsOpinion], [CreatedBy], [CreatedDate], [IsDeleted], [IsActive], [IsSystemDefine])
     VALUES
           (113, N'عقد البائع', N'Vendor Contract',17,0,0,0,0,'fatwaadmin@gmail.com',GETDATE(),0,1,1)

------------------- 4-7-2024 --------------------------
go
IF NOT EXISTS (
    SELECT * 
    FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_NAME = 'TEMP_ATTACHEMENTS' 
    AND COLUMN_NAME = 'SignedDocStoragePath'
)
BEGIN
    ALTER TABLE TEMP_ATTACHEMENTS
    ADD SignedDocStoragePath varchar(max);
END

IF NOT EXISTS (
    SELECT * 
    FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_NAME = 'UPLOADED_DOCUMENT' 
    AND COLUMN_NAME = 'SignedDocStoragePath'
)
BEGIN
    ALTER TABLE UPLOADED_DOCUMENT
    ADD SignedDocStoragePath varchar(max);
END

alter table DSP_Request_Log
add CreatedBy nvarchar(100) null,
	CreatedDate datetime null,
	ModifiedBy nvarchar(100) null,
	ModifiedDate datetime null,
	DeletedBy nvarchar(100) null,
	DeletedDate datetime null,
	IsDeleted bit null default 0

alter table DSP_Athentication_Requests
add CreatedBy nvarchar(100) null,
	CreatedDate datetime null,
	ModifiedBy nvarchar(100) null,
	ModifiedDate datetime null,
	DeletedBy nvarchar(100) null,
	DeletedDate datetime null,
	IsDeleted bit null default 0


----------------- 4-7-2024 ---------------------


-----------------------ATTACHMENT_TYPE
IF((SELECT COUNT(*) FROM ATTACHMENT_TYPE WHERE AttachmentTypeId = 115) = 0)
INSERT INTO ATTACHMENT_TYPE VALUES(115, N'Committee', 'Committee', 19, 0, 0, 0,0,0,'fatwaadmin@gmail.com','2024-04-22 18:27:06.700',null,null,null,null,0,1,1,null,0)
GO

------------------14/7/24
IF((SELECT COUNT(*) FROM ATTACHMENT_TYPE WHERE AttachmentTypeId = 121) = 0)
INSERT INTO ATTACHMENT_TYPE VALUES(121, N'آخر', 'Other', 17, 0, 0, 0,0,0,'fatwaadmin@gmail.com','2024-04-22 18:27:06.700',null,null,null,null,0,1,1,null,0)
ELSE
PRINT('Record Exist Already')
GO
IF((SELECT COUNT(*) FROM ATTACHMENT_TYPE WHERE AttachmentTypeId = 122) = 0)
INSERT INTO ATTACHMENT_TYPE VALUES(122, N'آخر', 'Other', 20, 0, 0, 0,0,0,'fatwaadmin@gmail.com','2024-04-22 18:27:06.700',null,null,null,null,0,1,1,null,0)
ELSE
PRINT('Record Exist Already')

IF((SELECT COUNT(*) FROM ATTACHMENT_TYPE WHERE AttachmentTypeId = 123) = 0)
INSERT INTO ATTACHMENT_TYPE VALUES(123, N'الحضور النظيف', 'Cleaner Attendance', 17, 0, 0, 0,0,0,'fatwaadmin@gmail.com','2024-04-22 18:27:06.700',null,null,null,null,0,1,1,null,0)
ELSE
PRINT('Record Exist Already')
----------------------------------------------------
INSERT INTO ATTACHMENT_TYPE VALUES(115, N'Committee Agenda', 'Committee Agenda', 19, 0, 0, 0,0,0,'fatwaadmin@gmail.com','2024-04-22 18:27:06.700',null,null,null,null,0,1,1,null,0)
GO
IF((SELECT COUNT(*) FROM ATTACHMENT_TYPE WHERE AttachmentTypeId = 116) = 0)
INSERT INTO ATTACHMENT_TYPE VALUES(116, N'Reports', 'Reports', 19, 0, 0, 0,0,0,'fatwaadmin@gmail.com','2024-04-22 18:27:06.700',null,null,null,null,0,1,1,null,0)
GO
IF((SELECT COUNT(*) FROM ATTACHMENT_TYPE WHERE AttachmentTypeId = 117) = 0)
INSERT INTO ATTACHMENT_TYPE VALUES(117, N'Fatwa Circular', 'Fatwa Circular', 19, 0, 0, 0,0,0,'fatwaadmin@gmail.com','2024-04-22 18:27:06.700',null,null,null,null,0,1,1,null,0)
GO
IF((SELECT COUNT(*) FROM ATTACHMENT_TYPE WHERE AttachmentTypeId = 118) = 0)
INSERT INTO ATTACHMENT_TYPE VALUES(118, N'Meeting Minutes', 'Meeting Minutes', 19, 0, 0, 0,0,0,'fatwaadmin@gmail.com','2024-04-22 18:27:06.700',null,null,null,null,0,1,1,null,0)
GO
IF((SELECT COUNT(*) FROM ATTACHMENT_TYPE WHERE AttachmentTypeId = 119) = 0)
INSERT INTO ATTACHMENT_TYPE VALUES(119, N'Other', 'Other', 19, 0, 0, 0,0,0,'fatwaadmin@gmail.com','2024-04-22 18:27:06.700',null,null,null,null,0,1,1,null,0)
GO

---- LLS 18-07-2024 start
GO
CREATE OR ALTER  PROCEDURE pLLSKuwaitAlYoumSourceDocuments                                
(       
 @EditionNumber nvarchar(500) =null,                 
 @EditionType nvarchar(500)=null,                
 @DocumentTitle nvarchar(500)=null,               
 @PublicationDateHijri nvarchar(500)=null,                
 @PublicationFrom datetime=null,                                    
 @PublicationTo datetime=null  
)                                    
AS                            
BEGIN            
    
Select                 
kp.Id AS UploadedDocumentId,                
kp.EditionNumber,                
kp.EditionType,                
kp.FileTitle,                
kp.DocumentTitle,                
kp.PublicationDate,              
kp.PublicationDateHijri,                
kp.StoragePath,                
kp.CreatedDate,     
RIGHT(Kp.StoragePath, CHARINDEX('.', REVERSE(StoragePath)) - 0) AS DocType,
COUNT(lp.OriginalSourceDocumentId) NoOfPrinciples
From KAY_PUBLICATION_STG kp   
LEFT JOIN FATWA_DB_DEV.DBO.LLS_LEGAL_PRINCIPLE lp on lp.OriginalSourceDocumentId = kp.Id
where   
KP.IsFullEdition = 0  
AND (kp.EditionNumber like '%' + @EditionNumber + '%' OR @EditionNumber IS NULL OR @EditionNumber = '')            
AND (kp.EditionType like '%' + @EditionType + '%' OR @EditionType IS NULL OR @EditionType = '')                 
AND (kp.DocumentTitle like '%' + @DocumentTitle + '%' OR @DocumentTitle IS NULL OR @DocumentTitle = '')           
AND (kp.PublicationDateHijri like '%' + @PublicationDateHijri + '%' OR @PublicationDateHijri IS NULL OR @PublicationDateHijri = '')                 
AND(CAST(kp.PublicationDate as date)>=@PublicationFrom OR @PublicationFrom IS NULL OR @PublicationFrom='')                                    
AND (CAST(kp.PublicationDate as date)<=@PublicationTo OR @PublicationTo IS NULL OR @PublicationTo='')  
GROUP BY
kp.Id,                
kp.EditionNumber,                
kp.EditionType,                
kp.FileTitle,                
kp.DocumentTitle,                
kp.PublicationDate,              
kp.PublicationDateHijri,                
kp.StoragePath,                
kp.CreatedDate    
ORDER BY kp.PublicationDate DESC                              
END 

---- LLS 18-07-2024 end'

--------------------------------
GO
IF((SELECT COUNT(*) FROM ATTACHMENT_TYPE WHERE AttachmentTypeId = 125) = 0)
INSERT INTO ATTACHMENT_TYPE VALUES(125, N'نموذج القرار', 'Decision Form', 20, 0, 0, 0,0,0,'fatwaadmin@gmail.com','2024-04-22 18:27:06.700',null,null,null,null,0,1,1,null,0)
ELSE
PRINT('Record Exist Already')

---------------------------------------------

-------------- 12-8-2024 ---------------

create table DS_SIGNING_REQUEST_LOG (
LogId int primary key identity(1,1),
ExternalId nvarchar(max),
RequestId varchar(100) ,
CivilId nvarchar(max),
DocumentId int,
Status bit,
ErrorCode nvarchar(max),
ErrorMessage nvarchar(max),
RequestStatus varchar(50),
CreatedBy nvarchar(256) NOT null,
CreatedDate datetime NOT null,
ModifiedBy nvarchar(256) null,
ModifiedDate datetime null,
DeletedBy nvarchar(256) null,
DeletedDate datetime null,
IsDeleted bit null default 0
);

-------------- 12-8-2024 ---------------

----------   22 / Auguest / 2024 ------
--------- Table Schema for DigitalSignature LKP---------
create table DS_SIGNING_METHODS (

MethodId int identity primary key,

NameEn nvarchar(100),

NameAr nvarchar(100)

);
 
create table DS_ATTACHMENT_TYPE_SIGNING_METHODS (

Id int identity primary key,

AttachmentTypeId int foreign key references Attachment_type(AttachmentTypeId),

MethodId int foreign key references DS_SIGNING_METHODS(MethodId),

CreatedBy nvarchar(256) NOT null,

CreatedDate datetime NOT null,

ModifiedBy nvarchar(256) null,

ModifiedDate datetime null,

DeletedBy nvarchar(256) null,

DeletedDate datetime null,

IsDeleted bit null default 0

);
 
create table DS_ATTACHMENT_TYPE_DESIGNATION_MAPPING (

Id int identity primary key,

AttachmentTypeId int foreign key references Attachment_type(AttachmentTypeId),

DesignationId int,

CreatedBy nvarchar(256) NOT null,

CreatedDate datetime NOT null,

ModifiedBy nvarchar(256) null,

ModifiedDate datetime null,

DeletedBy nvarchar(256) null,

DeletedDate datetime null,

IsDeleted bit null default 0

);
 
 ----------------------------
 Update  ATTACHMENT_TYPE Set IsMandatory = 1 where AttachmentTypeId = 117
 ---- 26-August-2024---------
 go
 Create or alter Procedure [dbo].[pAttachmentTypelist]
AS
BEGIN
    SELECT 
        ATT.*,
        CONCAT(ISNULL(EPI.FirstName_En, ''), ' ', ISNULL(EPI.SecondName_En, ''), ' ', ISNULL(EPI.LastName_En, '')) AS UserFullNameEn,
        CONCAT(ISNULL(EPI.FirstName_Ar, ''), ' ', ISNULL(EPI.SecondName_Ar, ''), ' ', ISNULL(EPI.LastName_Ar, '')) AS UserFullNameAr,
        STUFF((SELECT ', ' + CAST(EPD.Name_En AS VARCHAR(256))
               FROM DS_ATTACHMENT_TYPE_DESIGNATION_MAPPING DM
			   lEFT jOIN FATWA_DB_DEV.DBO.EP_DESIGNATION EPD ON EPD.Id=DM.DesignationId
               WHERE DM.AttachmentTypeId = ATT.AttachmentTypeId
               FOR XML PATH('')), 1, 1, '') AS DesignationNameEn,
	    STUFF((SELECT ', ' + CAST(EPD.Name_Ar AS NVARCHAR(256))
               FROM DS_ATTACHMENT_TYPE_DESIGNATION_MAPPING DM
			   lEFT jOIN FATWA_DB_DEV.DBO.EP_DESIGNATION EPD ON EPD.Id=DM.DesignationId
               WHERE DM.AttachmentTypeId = ATT.AttachmentTypeId
               FOR XML PATH('')), 1, 1, '') AS DesignationNameAr,
        STUFF((SELECT ', ' + CAST(DSM.NameEn AS VARCHAR(256))
               FROM DS_ATTACHMENT_TYPE_SIGNING_METHODS SM
			   LEFT JOIN DS_SIGNING_METHODS DSM ON DSM.MethodId=SM.MethodId
               WHERE SM.AttachmentTypeId = ATT.AttachmentTypeId
               FOR XML PATH('')), 1, 1, '') AS SigningMethodNameEN,
		STUFF((SELECT ', ' + CAST(DSM.NameAr AS NVARCHAR(256))
               FROM DS_ATTACHMENT_TYPE_SIGNING_METHODS SM
			   LEFT JOIN DS_SIGNING_METHODS DSM ON DSM.MethodId=SM.MethodId
               WHERE SM.AttachmentTypeId = ATT.AttachmentTypeId
               FOR XML PATH('')), 1, 1, '') AS SigningMethodNameAr
    FROM 
        ATTACHMENT_TYPE ATT
    LEFT JOIN 
        FATWA_DB_DEV.dbo.UMS_USER ums ON ums.Email = ATT.CreatedBy
    LEFT JOIN 
        FATWA_DB_DEV.dbo.EP_PERSONAL_INFORMATION EPI ON EPI.UserId = ums.Id
    WHERE 
        ATT.IsDeleted = 0
    ORDER BY 
        ATT.CreatedDate DESC;
END;


ALTER TABLE ATTACHMENT_TYPE
ADD IsDigitallySign BIT NULL default 0;
update ATTACHMENT_TYPE set IsDigitallySign=0;

insert into DS_SIGNING_METHODS values ('Sign By KMID',N'التوقيع عن طريق هويتي')
insert into DS_SIGNING_METHODS values ('Sign By Civil ID',N'طريق البطاقة المدنية التوقيع عن')
insert into DS_SIGNING_METHODS values ('Sign By Mobile Auth',N'الهاتف التوقيع عن طريق')
---- 27-aug-2024
go
CREATE  or alter  PROCEDURE pDSDocumentTaskStatus  
(                      
    @DocumentId INT                      
) 
AS  
BEGIN
SELECT  
dsrtl.*,
dssts.NameEn TaskStatusEn,
dssts.NameAr TaskStatusAr,
CONCAT(ISNULL(EPI.FirstName_En, ''), ' ', ISNULL(EPI.SecondName_En, ''), ' ', ISNULL(EPI.LastName_En, '')) SenderNameEn,  
CONCAT(ISNULL(EPI.FirstName_Ar, ''), ' ', ISNULL(EPI.SecondName_Ar, ''), ' ', ISNULL(EPI.LastName_Ar, '')) SenderNameAr,
CONCAT(ISNULL(epii.FirstName_En, ''), ' ', ISNULL(epii.SecondName_En, ''), ' ', ISNULL(epii.LastName_En, '')) ReceiverNameEn,  
CONCAT(ISNULL(epii.FirstName_Ar, ''), ' ', ISNULL(epii.SecondName_Ar, ''), ' ', ISNULL(epii.LastName_Ar, '')) ReceiverNameAr

FROM DS_SIGNING_REQUEST_TASK_LOG dsrtl  
LEFT JOIN DS_SIGNING_TASK_STATUS dssts on dssts.Id=dsrtl.StatusId
LEFT JOIN FATWA_DB_DEV.dbo.UMS_USER uu on uu.Id = dsrtl.SenderId  
LEFT JOIN FATWA_DB_DEV.dbo.UMS_USER uuu on uuu.Id = dsrtl.ReceiverId  
LEFT JOIN FATWA_DB_DEV.dbo.EP_PERSONAL_INFORMATION epi on epi.UserId = uu.Id  
LEFT JOIN FATWA_DB_DEV.dbo.EP_PERSONAL_INFORMATION epii on epii.UserId = uuu.Id  
where dsrtl.DocumentId=@DocumentId
order by dsrtl.CreatedDate
END

GO

------------------ 28-08-2024 --------------------------------
CREATE OR alter PROCEDURE pGetLLSlegalPrincipleReferenceDocuments      
(      
 @PrincipleId UNIQUEIDENTIFIER = '00000000-0000-0000-0000-000000000000'      
)      
AS      
BEGIN      
SELECT      
ud.*,    
 null as PreCommunicationId                  
, ATP.Type_Ar                                        
, ATP.Type_En
, ATP.IsDigitallySign
, DSS.NameEn as StatusEn
, DSS.NameAr as StatusAr
 , 0 ChildCount,        
 CONCAT(ISNULL(EPI.FirstName_En, ''), ' ', ISNULL(EPI.SecondName_En, ''), ' ', ISNULL(EPI.LastName_En, '')) AS DocumentSourceEn,          
 CONCAT(ISNULL(EPI.FirstName_Ar, ''), ' ', ISNULL(EPI.SecondName_Ar, ''), ' ', ISNULL(EPI.LastName_Ar, '')) AS DocumentSourceAr     
FROM       
UPLOADED_DOCUMENT ud      
 INNER JOIN ATTACHMENT_TYPE ATP ON ATP.AttachmentTypeId = ud.AttachmentTypeId 
 INNER JOIN DS_SIGNING_TASK_STATUS DSS ON DSS.Id = ud.StatusId 
 LEFT JOIN FATWA_DB_DEV.dbo.UMS_USER UU ON UU.Email = ud.CreatedBy      
 LEFT JOIN FATWA_DB_DEV.dbo.EP_PERSONAL_INFORMATION EPI ON EPI.UserId = UU.Id      
 LEFT JOIN FATWA_DB_DEV.dbo.EP_EMPLOYMENT_INFORMATION EEI ON EEI.UserId = UU.Id      
where UploadedDocumentId in (SELECT       
CASE WHEN      
IsMaskJudgment = 1 THEN MaskedJudgmentId      
ELSE UploadedDocumentId      
END UPLOADDOCUMENTiD      
FROM       
FATWA_DB_DEV.dbo.LLS_LEGAL_PRINCIPLE_SOURCE_DOCUMENT_REFERENCE A where PrincipleId = @PrincipleId)      
END

GO

CREATE or alter Procedure [dbo].[pTempAttachments]                                       
(                                  
 @referenceGuid uniqueidentifier = NULL                         
)                                  
AS                                            
begin                                  
 Select TAT.*            
   , isnull(cc.PreCommunicationId , '00000000-0000-0000-0000-000000000000') as PreCommunicationId            
   , TAT.UploadedDate AS DocDateTime                        
   , ATP.Type_Ar                        
   , ATP.Type_En
   , ATP.IsDigitallySign
   , DSS.NameEn as StatusEn
   , DSS.NameAr as StatusAr
   , (select count(*) from FATWA_DB_DEV.dbo.COMM_COMMUNICATION fcc where fcc.PreCommunicationId <> '00000000-0000-0000-0000-000000000000'           
      AND PreCommunicationId = TAT.CommunicationGuid          
  ) as ChildCount,    
  CASE   
   WHEN TAT.UploadedBy = 'MOJ RPA' THEN 'MOJ'   
   ELSE ISNULL(epi.FirstName_En, '') + ' ' + ISNULL(epi.LastName_En, '') + ' (' + OST.Name_En + ')'  
  END AS DocumentSourceEn,    
  CASE   
   WHEN TAT.UploadedBy = 'MOJ RPA' THEN N'وزارة العدل'   
   ELSE ISNULL(epi.FirstName_Ar, '') + ' ' + ISNULL(epi.LastName_Ar, '') + ' (' + OST.Name_Ar + ')'  
  END AS DocumentSourceAr       
 from TEMP_ATTACHEMENTS TAT                                            
 LEFT JOIN ATTACHMENT_TYPE ATP ON ATP.AttachmentTypeId = TAT.AttachmentTypeId
 LEFT JOIN DS_SIGNING_TASK_STATUS DSS ON DSS.Id = TAT.StatusId 
 LEFT JOIN FATWA_DB_DEV.dbo.COMM_COMMUNICATION cc on TAT.CommunicationGuid = cc.CommunicationId   
 LEFT JOIN FATWA_DB_DEV.dbo.UMS_USER UU ON UU.Email = TAT.UploadedBy  
 LEFT JOIN FATWA_DB_DEV.dbo.EP_PERSONAL_INFORMATION EPI ON EPI.UserId = UU.Id  
 LEFT JOIN FATWA_DB_DEV.dbo.EP_EMPLOYMENT_INFORMATION EEI ON EEI.UserId = UU.Id  
 LEFT JOIN FATWA_DB_DEV.dbo.EP_DESIGNATION OST ON OST.Id = EEI.DesignationId           
 Where (TAT.Guid = @referenceGuid OR @referenceGuid IS NULL OR TAT.CommunicationGuid = @referenceGuid)        
 AND (TAT.IsMaskedAttachment != 1 OR TAT.IsMaskedAttachment IS NULL)      
 ORDER BY TAT.UploadedDate DESC         
END   

GO

CREATE or alter Procedure [dbo].[pOfficialDocuments]                           
(                              
  @referenceGuid uniqueidentifier = NULL                 
)                              
AS                                        
begin                              
 Select LUD.*                    
   , ATP.Type_Ar                    
   , ATP.Type_En
   , ATP.IsDigitallySign
   , DSS.NameEn as StatusEn
   , DSS.NameAr as StatusAr
   ,CASE   
   WHEN LUD.CreatedBy = 'MOJ RPA' THEN 'MOJ'   
   ELSE ISNULL(epi.FirstName_En, '') + ' ' + ISNULL(epi.LastName_En, '') + ' (' + OST.Name_En + ')'  
  END AS DocumentSourceEn  
  ,CASE   
   WHEN LUD.CreatedBy = 'MOJ RPA' THEN N'وزارة العدل'   
   ELSE ISNULL(epi.FirstName_Ar, '') + ' ' + ISNULL(epi.LastName_Ar, '') + ' (' + OST.Name_Ar + ')'  
  END AS DocumentSourceAr                   
 from UPLOADED_DOCUMENT LUD                                        
 INNER JOIN ATTACHMENT_TYPE ATP ON ATP.AttachmentTypeId = LUD.AttachmentTypeId
 LEFT JOIN DS_SIGNING_TASK_STATUS DSS ON DSS.Id = LUD.StatusId
 LEFT JOIN FATWA_DB_DEV.dbo.UMS_USER UU ON UU.Email = LUD.CreatedBy  
 LEFT JOIN FATWA_DB_DEV.dbo.EP_PERSONAL_INFORMATION EPI ON EPI.UserId = UU.Id  
 LEFT JOIN FATWA_DB_DEV.dbo.EP_EMPLOYMENT_INFORMATION EEI ON EEI.UserId = UU.Id  
 LEFT JOIN FATWA_DB_DEV.dbo.CMS_OPERATING_SECTOR_TYPE_G2G_LKP OST ON OST.Id = EEI.SectorTypeId                    
 Where LUD.IsDeleted != 1 AND ATP.IsOfficialLetter = 1                                      
 AND (LUD.ReferenceGuid = @referenceGuid OR @referenceGuid IS NULL)                            
end 

GO

CREATE or alter Procedure [dbo].[pUploadedDocuments]                                             
(                                                
  @referenceGuid uniqueidentifier = NULL,                                    
  @literatureId int = NULL                                       
)                                                
AS                                                          
begin                                                
 Select LUD.*                 
   , isnull(cc.PreCommunicationId , '00000000-0000-0000-0000-000000000000') as PreCommunicationId                
   , ATP.Type_Ar                                      
   , ATP.Type_En
   , ATP.IsDigitallySign
   , DSS.NameEn as StatusEn
   , DSS.NameAr as StatusAr
   , LUD.ReferenceGuid as [ReferenceId]              
   , (select count(*) from FATWA_DB_DEV.dbo.COMM_COMMUNICATION fcc where fcc.PreCommunicationId <> '00000000-0000-0000-0000-000000000000'               
      AND PreCommunicationId = LUD.CommunicationGuid              
  ) as ChildCount,      
  CASE     
   WHEN LUD.CreatedBy = 'MOJ RPA' THEN 'MOJ'     
   ELSE ISNULL(epi.FirstName_En, '') + ' ' + ISNULL(epi.LastName_En, '') + ' (' + OST.Name_En + ')'    
  END AS DocumentSourceEn,      
  CASE     
   WHEN LUD.CreatedBy = 'MOJ RPA' THEN N'وزارة العدل'     
   ELSE ISNULL(epi.FirstName_Ar, '') + ' ' + ISNULL(epi.LastName_Ar, '') + ' (' + OST.Name_Ar + ')'    
  END AS DocumentSourceAr    
 from UPLOADED_DOCUMENT LUD                                                          
 INNER JOIN ATTACHMENT_TYPE ATP ON ATP.AttachmentTypeId = LUD.AttachmentTypeId 
 LEFT JOIN DS_SIGNING_TASK_STATUS DSS ON DSS.Id = LUD.StatusId
 LEFT JOIN FATWA_DB_DEV.dbo.COMM_COMMUNICATION cc on LUD.CommunicationGuid = cc.CommunicationId    
 LEFT JOIN FATWA_DB_DEV.dbo.UMS_USER UU ON UU.Email = LUD.CreatedBy    
 LEFT JOIN FATWA_DB_DEV.dbo.EP_PERSONAL_INFORMATION EPI ON EPI.UserId = UU.Id    
 LEFT JOIN FATWA_DB_DEV.dbo.EP_EMPLOYMENT_INFORMATION EEI ON EEI.UserId = UU.Id    
 LEFT JOIN FATWA_DB_DEV.dbo.EP_DESIGNATION OST ON OST.Id = EEI.DesignationId   
 Where LUD.IsDeleted != 1                                                       
 AND LUD.ReferenceGuid = @referenceGuid OR LUD.CommunicationGuid = @referenceGuid OR LUD.LiteratureId = @literatureId          
 --AND LUD.ReferenceGuid = @referenceGuid OR @referenceGuid IS NULL OR LUD.CommunicationGuid = @referenceGuid                            
 --AND (LUD.LiteratureId = @literatureId )--OR @literatureId IS NULL OR @literatureId = '0')             
 AND LUD.IsMaskedAttachment != 1            
 ORDER BY LUD.CreatedDateTime DESC                      
END      

Go

create table DS_SIGNING_TASK_STATUS(
Id int identity primary key,
NameEn nvarchar(100),
NameAr nvarchar(100)
)

create table DS_SIGNING_REQUEST_TASK_LOG(
SigningTaskId uniqueidentifier primary key,
DocumentId int,
ReferanceId nvarchar(450),
SenderId nvarchar(450),
ReceiverId nvarchar(450),
Remarks nvarchar(200),
ModuleId int,
StatusId int foreign key references DS_SIGNING_TASK_STATUS(Id),
RejectionReason nvarchar(450) null,
CreatedBy nvarchar(256) NOT null,
CreatedDate datetime NOT null,
ModifiedBy nvarchar(256) null,
ModifiedDate datetime null,
DeletedBy nvarchar(256) null,
DeletedDate datetime null,
IsDeleted bit null default 0
)


insert into DS_SIGNING_TASK_STATUS values ('Not Signed','Not Signed');
insert into DS_SIGNING_TASK_STATUS values ('Send For Signing','Send For Signing');
insert into DS_SIGNING_TASK_STATUS values ('Rejected','Rejected');
insert into DS_SIGNING_TASK_STATUS values ('Signed','Signed');
insert into DS_SIGNING_TASK_STATUS values ('Failed','Failed');


alter table UPLOADED_DOCUMENT 
add StatusId int null

update UPLOADED_DOCUMENT set StatusId = 1

alter table TEMP_ATTACHEMENTS 
add StatusId int null

update UPLOADED_DOCUMENT set StatusId = 1



EXEC sp_rename 'DS_SIGNING_REQUEST_TASK_LOG.ReferanceId',  'ReferenceId', 'COLUMN';

------------------ 28-08-2024 --------------------------------

------------------ 5-09-2024 --------------------------------

alter table DS_SIGNING_METHODS
add SignatureProfileName nvarchar(250)
update DS_SIGNING_METHODS set SignatureProfileName = 'DPS_Remote_NoImage' where MethodId = 2
update DS_SIGNING_METHODS set SignatureProfileName = 'DPS_MID_NoImage' where MethodId = 3

------------------ 5-09-2024 --------------------------------

------------------ 9-09-2024 --------------------------------


GO
CREATE or alter    PROCEDURE pDSDocumentTaskStatus    
(                        
    @DocumentId INT                        
)   
AS    
BEGIN  
SELECT    
dsrtl.SigningTaskId,
dsrtl.DocumentId,
dsrtl.Remarks,
dsrtl.RejectionReason,
dsrtl.StatusId,
dssts.NameEn TaskStatusEn,  
dssts.NameAr TaskStatusAr,  
CONCAT(ISNULL(EPI.FirstName_En, ''), ' ', ISNULL(EPI.SecondName_En, ''), ' ', ISNULL(EPI.LastName_En, '')) SenderNameEn,    
CONCAT(ISNULL(EPI.FirstName_Ar, ''), ' ', ISNULL(EPI.SecondName_Ar, ''), ' ', ISNULL(EPI.LastName_Ar, '')) SenderNameAr,  
CONCAT(ISNULL(epii.FirstName_En, ''), ' ', ISNULL(epii.SecondName_En, ''), ' ', ISNULL(epii.LastName_En, '')) ReceiverNameEn,    
CONCAT(ISNULL(epii.FirstName_Ar, ''), ' ', ISNULL(epii.SecondName_Ar, ''), ' ', ISNULL(epii.LastName_Ar, '')) ReceiverNameAr ,
dsrtl.ReceiverId,
dsrtl.CreatedDate
  
FROM DS_SIGNING_REQUEST_TASK_LOG dsrtl    
LEFT JOIN DS_SIGNING_TASK_STATUS dssts on dssts.Id=dsrtl.StatusId  
LEFT JOIN FATWA_DB_DEV.dbo.UMS_USER uu on uu.Id = dsrtl.SenderId    
LEFT JOIN FATWA_DB_DEV.dbo.UMS_USER uuu on uuu.Id = dsrtl.ReceiverId    
LEFT JOIN FATWA_DB_DEV.dbo.EP_PERSONAL_INFORMATION epi on epi.UserId = uu.Id    
LEFT JOIN FATWA_DB_DEV.dbo.EP_PERSONAL_INFORMATION epii on epii.UserId = uuu.Id    
where dsrtl.DocumentId=@DocumentId  
Union 
	SELECT    
NEWID() SigningTaskId,
dsrl.DocumentId,
'' Remarks,
'' RejectionReason,
(select ID from DS_SIGNING_TASK_STATUS where Id = 4) StatusId,  
(select NameEn from DS_SIGNING_TASK_STATUS where Id = 4) TaskStatusEn,  
(select NameAr from DS_SIGNING_TASK_STATUS where Id = 4) TaskStatusAr,  
CONCAT(ISNULL(EPI.FirstName_En, ''), ' ', ISNULL(EPI.SecondName_En, ''), ' ', ISNULL(EPI.LastName_En, '')) SenderNameEn,    
CONCAT(ISNULL(EPI.FirstName_Ar, ''), ' ', ISNULL(EPI.SecondName_Ar, ''), ' ', ISNULL(EPI.LastName_Ar, '')) SenderNameAr,  
'' ReceiverNameEn,    
'' ReceiverNameAr,
null ReceiverId,
dsrl.CreatedDate
  
FROM DS_SIGNING_REQUEST_LOG dsrl       
LEFT JOIN FATWA_DB_DEV.dbo.EP_PERSONAL_INFORMATION epi on epi.CivilId = dsrl.CivilId  
where dsrl.DocumentId=@DocumentId and dsrl.RequestStatus= 'Approved'
order by dsrtl.CreatedDate desc
END


------------------ 9-09-2024 --------------------------------
------------------ 10-09-2024 --------------------------------
INSERT INTO [dbo].[ATTACHMENT_TYPE]
           ([AttachmentTypeId],[Type_Ar],[Type_En],[ModuleId],[IsMandatory],[IsOfficialLetter]
		   ,[SubTypeId],[IsGePortalType],[IsOpinion],[CreatedBy],[CreatedDate],[IsDeleted],[IsActive],[IsSystemDefine],[Description],[IsMojExtracted])
     VALUES (124 ,N'Signature Image','Signature Image', 4, 0, 0, NULL, 0, 0,'fatwaadmin@gmail.com',GETDATE(),0,1,1,NULL,0)
-------------------------
INSERT INTO [dbo].[ATTACHMENT_TYPE]
           ([AttachmentTypeId], [Type_Ar], [Type_En], [ModuleId], [IsMandatory], [IsOfficialLetter], [IsGePortalType], [IsOpinion], [CreatedBy], [CreatedDate], [IsDeleted], [IsActive], [IsSystemDefine])
     VALUES
           (125, N'StockTaking Report', N'StockTaking Report',3,0,0,0,0,'fatwaadmin@gmail.com',GETDATE(),0,1,1)
GO	



------------------ 26-09-2024 --------------------------------
ALTER table DS_SIGNING_REQUEST_LOG 
add SigningMethodId int default 0 

ALTER table DS_SIGNING_REQUEST_TASK_LOG 
add SigningMethodId int default 0 

GO

CREATE OR alter    PROCEDURE pDSDocumentTaskStatus            
(                                
    @DocumentId INT                                
)           
AS            
BEGIN          
SELECT            
dsrtl.SigningTaskId,        
dsrtl.DocumentId,        
dsrtl.Remarks,        
dsrtl.RejectionReason,        
dsrtl.StatusId,        
dssts.NameEn TaskStatusEn,          
dssts.NameAr TaskStatusAr,          
CONCAT(ISNULL(EPI.FirstName_En, ''), ' ', ISNULL(EPI.SecondName_En, ''), ' ', ISNULL(EPI.LastName_En, '')) SenderNameEn,            
CONCAT(ISNULL(EPI.FirstName_Ar, ''), ' ', ISNULL(EPI.SecondName_Ar, ''), ' ', ISNULL(EPI.LastName_Ar, '')) SenderNameAr,          
CONCAT(ISNULL(epii.FirstName_En, ''), ' ', ISNULL(epii.SecondName_En, ''), ' ', ISNULL(epii.LastName_En, '')) ReceiverNameEn,            
CONCAT(ISNULL(epii.FirstName_Ar, ''), ' ', ISNULL(epii.SecondName_Ar, ''), ' ', ISNULL(epii.LastName_Ar, '')) ReceiverNameAr ,        
dsrtl.ReceiverId,        
dsrtl.CreatedDate,  
dsrtl.ModifiedDate,
dsm.NameEn SigningMethodEn,  
dsm.NameAr SigningMethodAr  

FROM DS_SIGNING_REQUEST_TASK_LOG dsrtl       
LEFT JOIN DS_SIGNING_TASK_STATUS dssts on dssts.Id=dsrtl.StatusId  
LEFT JOIN DS_SIGNING_METHODS dsm ON dsrtl.SigningMethodId = dsm.MethodId  
LEFT JOIN FATWA_DB_DEV.dbo.UMS_USER uu on uu.Id = dsrtl.SenderId            
LEFT JOIN FATWA_DB_DEV.dbo.UMS_USER uuu on uuu.Id = dsrtl.ReceiverId            
LEFT JOIN FATWA_DB_DEV.dbo.EP_PERSONAL_INFORMATION epi on epi.UserId = uu.Id            
LEFT JOIN FATWA_DB_DEV.dbo.EP_PERSONAL_INFORMATION epii on epii.UserId = uuu.Id 
where dsrtl.DocumentId=@DocumentId 
Union         
 SELECT            
NEWID() SigningTaskId,        
dsrl.DocumentId,        
'' Remarks,        
'' RejectionReason,        
(select ID from DS_SIGNING_TASK_STATUS where Id = 4) StatusId,          
(select NameEn from DS_SIGNING_TASK_STATUS where Id = 4) TaskStatusEn,          
(select NameAr from DS_SIGNING_TASK_STATUS where Id = 4) TaskStatusAr,          
CONCAT(ISNULL(EPI.FirstName_En, ''), ' ', ISNULL(EPI.SecondName_En, ''), ' ', ISNULL(EPI.LastName_En, '')) SenderNameEn,            
CONCAT(ISNULL(EPI.FirstName_Ar, ''), ' ', ISNULL(EPI.SecondName_Ar, ''), ' ', ISNULL(EPI.LastName_Ar, '')) SenderNameAr,          
'' ReceiverNameEn,            
'' ReceiverNameAr,        
null ReceiverId,    
dsrl.CreatedDate,  
dsrl.ModifiedDate,  
dsm.NameEn SigningMethodEn,  
dsm.NameAr SigningMethodAr  
          
FROM DS_SIGNING_REQUEST_LOG dsrl  
LEFT JOIN DS_SIGNING_METHODS dsm ON dsrl.SigningMethodId = dsm.MethodId  
LEFT JOIN FATWA_DB_DEV.dbo.EP_PERSONAL_INFORMATION epi on epi.CivilId = dsrl.CivilId          
where dsrl.DocumentId=@DocumentId and dsrl.RequestStatus= 'Approved'    
and dsrl.CivilId not in     
 (select CivilId from FATWA_DB_DEV.dbo.EP_PERSONAL_INFORMATION     
 where UserId = (select ReceiverId from DS_SIGNING_REQUEST_TASK_LOG where DocumentId = @DocumentId ))    
order by dsrtl.CreatedDate desc        
END 
Go

------------------ 26-09-2024 --------------------------------

------------------ 6-10-2024 --------------------------------

CREATE OR alter    PROCEDURE pDSDocumentTaskStatus            
(                                
    @DocumentId INT                                
)           
AS            
BEGIN          
SELECT            
dsrtl.SigningTaskId,        
dsrtl.DocumentId,        
dsrtl.Remarks,        
dsrtl.RejectionReason,        
dsrtl.StatusId,        
dssts.NameEn TaskStatusEn,          
dssts.NameAr TaskStatusAr,          
CONCAT(ISNULL(EPI.FirstName_En, ''), ' ', ISNULL(EPI.SecondName_En, ''), ' ', ISNULL(EPI.LastName_En, '')) SenderNameEn,            
CONCAT(ISNULL(EPI.FirstName_Ar, ''), ' ', ISNULL(EPI.SecondName_Ar, ''), ' ', ISNULL(EPI.LastName_Ar, '')) SenderNameAr,          
CONCAT(ISNULL(epii.FirstName_En, ''), ' ', ISNULL(epii.SecondName_En, ''), ' ', ISNULL(epii.LastName_En, '')) ReceiverNameEn,            
CONCAT(ISNULL(epii.FirstName_Ar, ''), ' ', ISNULL(epii.SecondName_Ar, ''), ' ', ISNULL(epii.LastName_Ar, '')) ReceiverNameAr ,        
dsrtl.ReceiverId,        
dsrtl.CreatedDate,  
dsrtl.ModifiedDate,
dsm.NameEn SigningMethodEn,  
dsm.NameAr SigningMethodAr  

FROM DS_SIGNING_REQUEST_TASK_LOG dsrtl       
LEFT JOIN DS_SIGNING_TASK_STATUS dssts on dssts.Id=dsrtl.StatusId  
LEFT JOIN DS_SIGNING_METHODS dsm ON dsrtl.SigningMethodId = dsm.MethodId  
LEFT JOIN FATWA_DB_DEV.dbo.UMS_USER uu on uu.Id = dsrtl.SenderId            
LEFT JOIN FATWA_DB_DEV.dbo.UMS_USER uuu on uuu.Id = dsrtl.ReceiverId            
LEFT JOIN FATWA_DB_DEV.dbo.EP_PERSONAL_INFORMATION epi on epi.UserId = uu.Id            
LEFT JOIN FATWA_DB_DEV.dbo.EP_PERSONAL_INFORMATION epii on epii.UserId = uuu.Id 
where dsrtl.DocumentId=@DocumentId 
Union         
 SELECT            
NEWID() SigningTaskId,        
dsrl.DocumentId,        
'' Remarks,        
'' RejectionReason,        
(select ID from DS_SIGNING_TASK_STATUS where Id = 4) StatusId,          
(select NameEn from DS_SIGNING_TASK_STATUS where Id = 4) TaskStatusEn,          
(select NameAr from DS_SIGNING_TASK_STATUS where Id = 4) TaskStatusAr,          
CONCAT(ISNULL(EPI.FirstName_En, ''), ' ', ISNULL(EPI.SecondName_En, ''), ' ', ISNULL(EPI.LastName_En, '')) SenderNameEn,            
CONCAT(ISNULL(EPI.FirstName_Ar, ''), ' ', ISNULL(EPI.SecondName_Ar, ''), ' ', ISNULL(EPI.LastName_Ar, '')) SenderNameAr,          
'' ReceiverNameEn,            
'' ReceiverNameAr,        
null ReceiverId,    
dsrl.CreatedDate,  
dsrl.ModifiedDate,  
dsm.NameEn SigningMethodEn,  
dsm.NameAr SigningMethodAr  
          
FROM DS_SIGNING_REQUEST_LOG dsrl  
LEFT JOIN DS_SIGNING_METHODS dsm ON dsrl.SigningMethodId = dsm.MethodId  
LEFT JOIN FATWA_DB_DEV.dbo.EP_PERSONAL_INFORMATION epi on epi.CivilId = dsrl.CivilId          
where dsrl.DocumentId=@DocumentId and dsrl.RequestStatus= 'Approved'    
and dsrl.CivilId not in     
 (select CivilId from FATWA_DB_DEV.dbo.EP_PERSONAL_INFORMATION     
 where UserId in (select ReceiverId from DS_SIGNING_REQUEST_TASK_LOG where DocumentId = @DocumentId ))    
order by dsrtl.CreatedDate desc        
END  

------------------ 6-10-2024 --------------------------------


UPDATE TEMP_ATTACHEMENTS SET StatusId = 0 where StatusId IS NULL
ALTER TABLE TEMP_ATTACHEMENTS ALTER COLUMN StatusId INT NOT NULL


UPDATE UPLOADED_DOCUMENT SET StatusId = 0 where StatusId IS NULL
ALTER TABLE UPLOADED_DOCUMENT ALTER COLUMN StatusId INT NOT NULL

------16-01-2025----- PReviewDocument realed chnages
go
CREATE or alter   Procedure [dbo].[pTempAttachments]                                         
(                                    
 @referenceGuid uniqueidentifier = NULL    ,
 @attachementId INT = 0
)                                    
AS                                              
begin                                    
 Select TAT.*              
   , isnull(cc.PreCommunicationId , '00000000-0000-0000-0000-000000000000') as PreCommunicationId              
   , TAT.UploadedDate AS DocDateTime                          
   , ATP.Type_Ar                          
   , ATP.Type_En  
   , ATP.IsDigitallySign  
   , DSS.NameEn as StatusEn  
   , DSS.NameAr as StatusAr  
   , (select count(*) from FATWA_DB_DEV.dbo.COMM_COMMUNICATION fcc where fcc.PreCommunicationId <> '00000000-0000-0000-0000-000000000000'             
      AND PreCommunicationId = TAT.CommunicationGuid            
  ) as ChildCount,      
  CASE     
   WHEN TAT.UploadedBy = 'MOJ RPA' THEN 'MOJ'     
   ELSE ISNULL(epi.FirstName_En, '') + ' ' + ISNULL(epi.LastName_En, '') + ' (' + OST.Name_En + ')'    
  END AS DocumentSourceEn,      
  CASE     
   WHEN TAT.UploadedBy = 'MOJ RPA' THEN N'وزارة العدل'     
   ELSE ISNULL(epi.FirstName_Ar, '') + ' ' + ISNULL(epi.LastName_Ar, '') + ' (' + OST.Name_Ar + ')'    
  END AS DocumentSourceAr         
 from TEMP_ATTACHEMENTS TAT                                              
 LEFT JOIN ATTACHMENT_TYPE ATP ON ATP.AttachmentTypeId = TAT.AttachmentTypeId  
 LEFT JOIN DS_SIGNING_TASK_STATUS DSS ON DSS.Id = TAT.StatusId   
 LEFT JOIN FATWA_DB_DEV.dbo.COMM_COMMUNICATION cc on TAT.CommunicationGuid = cc.CommunicationId     
 LEFT JOIN FATWA_DB_DEV.dbo.UMS_USER UU ON UU.Email = TAT.UploadedBy    
 LEFT JOIN FATWA_DB_DEV.dbo.EP_PERSONAL_INFORMATION EPI ON EPI.UserId = UU.Id    
 LEFT JOIN FATWA_DB_DEV.dbo.EP_EMPLOYMENT_INFORMATION EEI ON EEI.UserId = UU.Id    
 LEFT JOIN FATWA_DB_DEV.dbo.EP_DESIGNATION OST ON OST.Id = EEI.DesignationId             
 WHERE (TAT.Guid = @referenceGuid OR @referenceGuid IS NULL OR TAT.CommunicationGuid = @referenceGuid) 
 AND (TAT.AttachementId = @attachementId OR @attachementId = 0)         
 AND (TAT.IsMaskedAttachment != 1 OR TAT.IsMaskedAttachment IS NULL)        
 ORDER BY TAT.UploadedDate DESC           
END 

---------------
go
CREATE  or alter     Procedure [dbo].[pLiteratureViewableListFiltered]                                                           
          
  @bookName NVARCHAR(50) = NULL                                                          
 , @authorName NVARCHAR(90) = NULL                                                          
 , @indexName NVARCHAR(100) = NULL                                                          
 , @indexNumber NVARCHAR(50) = NULL                                                          
 , @divisionNumber NVARCHAR(50) = NULL                                                          
 , @aisleNumber NVARCHAR(50) = NULL                                                          
 , @barcode NVARCHAR(50) = NULL                                                          
 , @character NVARCHAR(50) = NULL                                                          
 , @price int = NULL                                                          
 , @purchaseDate datetime = Null                                                          
 , @From datetime = Null                                                          
 , @To datetime = Null                                                          
 , @classificationId int = Null                                                 
 , @TagNo NVARCHAR(50) = Null                          
 , @authorId INT = NULL                         
 , @indexId INT = NULL                         
 , @character82 NVARCHAR(50) = NULL                                                          
                      
AS                                                               
          
 SELECT DISTINCT ld.LiteratureId                                          
    , ld.Name AS LiteratureName                                         
    , ld.ISBN           
 ,ld.IsViewable        
         
 ,ld.CopyCount        
 , ld.DeweyBookNumber                    
    , ld.DeletedBy                                            
    , null as PurchaseDate                                                    
       ,ld.EditionNumber        
    ,ld.EditionYear        
 , lla.FullName_En AS LiteratureAuthor_En                     
 , lla.FullName_Ar AS LiteratureAuthor_Ar         
 ,li.IndexNumber        
  ,li.Name_En AS IndexName_En               
  ,li.Name_Ar AS IndexName_Ar      
  ,UD.UploadedDocumentId          
  ,UD.StoragePath      
  ,UD.DocType      
  ,UD.AttachmentTypeId      
  ,UD.FileName    
  ,ld.CreatedDate  
  from LMS_LITERATURE_DETAILS ld                                                                          
  LEFT JOIN LMS_LITERATURE_DETAILS_LMS_LITERATURE_AUTHOR lda ON ld.LiteratureId = lda.LiteratureId              
  LEFT JOIN LMS_LITERATURE_AUTHOR lla ON lla.AuthorId = lda.AuthorId            
  LEFT JOIN LMS_LITERATURE_INDEX li ON ld.IndexId = li.IndexId                                                         
  LEFT JOIN LMS_LITERATURE_INDEX_DIVISION_AISLE lida ON ld.DivisionAisleId = lida.DivisionAisleId                                                             
  LEFT JOIN LMS_LITERATURE_PURCHASE lp ON ld.LiteratureId = lp.LiteratureId                                                             
  LEFT JOIN LMS_LITERATURE_BARCODE lb ON ld.LiteratureId = lb.LiteratureId                                                             
-- TAG NUMBER SEARCH                                
  LEFT JOIN LMS_LITERATURE_DETAIL_LITERATURE_TAG ldt ON ld.LiteratureId = ldt.LiteratureId                                            
  LEFT JOIN  LMS_LITERATURE_TAG llt ON ldt.TagId = llt.Id                                            
  --TAG NO END                
   -- Attachements                
  INNER JOIN DMS_DB_DEV.dbo.UPLOADED_DOCUMENT UD ON UD.LiteratureId = ld.LiteratureId AND UD.AttachmentTypeId = 1      
  Where ld.IsDeleted != 1                                  
  AND ld.IsDraft != 1                                       
  AND ld.IsViewable =1        
  AND (lLa.AuthorId = @authorId OR @authorId IS NULL OR @authorId = '0')                        
  AND (li.IndexId = @indexId OR @indexId IS NULL OR @indexId = '0')           
  AND (ld.[Name] like '%' + @bookName + '%' OR @bookName IS NULL OR @bookName='')             
  AND (ld.ClassificationId = @classificationId OR @classificationId IS NULL OR @classificationId='0')                                             
  AND (lLa.FullName_En LIKE '%' + @authorName + '%' OR @authorName IS NULL OR @authorName='')                                                             
  AND (li.Name_Ar LIKE '%' + @indexName + '%' OR @indexName IS NULL OR @indexName='')                                                             
  AND (li.IndexNumber = @indexNumber OR @indexNumber IS NULL OR @indexNumber='')                                                             
  AND (lida.DivisionNumber = @divisionNumber OR @divisionNumber IS NULL OR @divisionNumber='')                                                             
  AND (lida.AisleNumber = @aisleNumber OR @aisleNumber IS NULL OR @aisleNumber='')                                                             
  AND (lb.BarCodeNumber = @barcode OR @barcode IS NULL OR @barcode='')                                                             
  AND (ld.Characters = @character OR @character IS NULL OR @character='')                       
  AND (ld.Characters82 Like '%'+ @character82+'%' OR @character82 IS NULL OR @character82='')                                                             
  AND (lp.Price = @price OR @price IS NULL OR @price='')                                                
  AND (llt.TagNo = @TagNo OR @TagNo IS NULL OR @TagNo='')                                                        
  AND ( (CAST(lp.Date AS DATE) = @purchaseDate) OR @purchaseDate IS NULL OR @purchaseDate='')                                                             
  AND ( (CAST(ld.CreatedDate AS DATE) >= @From) Or @From Is Null OR @From='')                                                             
  AND ( (CAST(ld.CreatedDate AS DATE) <= @To) OR @To Is Null OR @To='')                       
      ORDER BY ld.CreatedDate desc                                                     
 RETURN 

 ---------------------- 04-17-2025
 GO
CREATE OR ALTER  PROCEDURE pDmsGetKayPublicationDocumentList                                          
(                                   
@EditionNumber nvarchar(500) =null,                     
@EditionType nvarchar(500)=null,                    
@DocumentTitle nvarchar(500)=null,                   
@PublicationDateHijri nvarchar(500)=null,                    
@PublicationFrom datetime=null,                                        
@PublicationTo datetime=null,  
@IsFullEdition bit = null,  
@PageSize INT = 10,          
@PageNumber INT = 1,
@FromLegalLegislationForm bit = null
)                                        
AS                                
BEGIN               
WITH PaginatedResults AS        
 (        
  Select                     
  kp.Id,                    
  kp.EditionNumber,                    
  kp.EditionType,                    
  kp.FileTitle,                    
  kp.DocumentTitle,                    
  kp.PublicationDate,                  
  kp.PublicationDateHijri,                    
  kp.StoragePath,                    
  kp.CreatedDate,                    
  kp.StartPage,                    
  kp.EndPage,          
  kp.IsFullEdition          
  From KAY_PUBLICATION_STG kp                      
  where (@FromLegalLegislationForm = 0 AND kp.IsDeleted != 1 OR @FromLegalLegislationForm = 1) AND (kp.IsFullEdition = (CASE WHEN @IsFullEdition = 1 THEN 1 ELSE 0 END) OR kp.IsFullEdition = (CASE WHEN @IsFullEdition = 1 THEN 0 ELSE 0 END))
  AND (kp.EditionNumber like '%' + @EditionNumber + '%' OR @EditionNumber IS NULL OR @EditionNumber = '')                
  AND (kp.EditionType like '%' + @EditionType + '%' OR @EditionType IS NULL OR @EditionType = '')                     
  AND (kp.DocumentTitle like '%' + @DocumentTitle + '%' OR @DocumentTitle IS NULL OR @DocumentTitle = '')               
  AND (kp.PublicationDateHijri like '%' + @PublicationDateHijri + '%' OR @PublicationDateHijri IS NULL OR @PublicationDateHijri = '')                     
  AND(CAST(kp.PublicationDate as date)>=@PublicationFrom OR @PublicationFrom IS NULL OR @PublicationFrom='')                                        
  AND (CAST(kp.PublicationDate as date)<=@PublicationTo OR @PublicationTo IS NULL OR @PublicationTo='') 
 )      
 SELECT *, COUNT(*) OVER() AS TotalCount                  
    FROM PaginatedResults                  
    ORDER BY PublicationDate DESC                  
    OFFSET (@PageNumber - 1) * @PageSize ROWS                  
    FETCH NEXT @PageSize ROWS ONLY;         
END

 ---------------------------------