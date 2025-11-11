-- [dbo].[pUploadedDocuments] 'a7c87b4f-0cd0-4cc1-95c4-3d3d1fa1d0b9'                 
CREATE OR ALTER Procedure [dbo].[pUploadedDocuments]               
(                  
  @referenceGuid uniqueidentifier = NULL,      
  @literatureId int = NULL         
)                  
AS                            
begin                  
Select LUD.*        
  , ATP.Type_Ar        
  , ATP.Type_En        
from UPLOADED_DOCUMENT LUD                            
INNER JOIN ATTACHMENT_TYPE ATP ON ATP.AttachmentTypeId = LUD.AttachmentTypeId          
Where LUD.IsDeleted != 1                           
AND (LUD.ReferenceGuid = @referenceGuid OR @referenceGuid IS NULL)                          
AND (LUD.LiteratureId = @literatureId OR @literatureId IS NULL OR @literatureId = '0')                                               
SET ROWCOUNT 0                            
SET NOCOUNT OFF                            
RETURN                   
end   



/****** Object:  StoredProcedure [dbo].[pDocumentApprovalListSelAll]    Script Date: 26/10/2022 5:20:19 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER Procedure [dbo].[pDocumentApprovalListSelAll]        
AS        
BEGIN        
Select distinct  ld.DocumentId  
       , ld.Title  
       , ld.CatalogId  
       , ld.Comment  
       , ld.AppliedDate  
       , ld.Content  
       , ld.CreatedBy  
       , ld.Number        
       , ld.[Description]  
       , ld.EntityId  
       , ld.IssuedBy  
       , ld.IssueDate  
       , ld.ModifiedBy  
       , ld.ModifiedDate  
       , ld.[Status]  
       , ld.Reason  
       , ld.Reference    
       , ge.[Name_En] as Govt_Entity_En  
       , ge.[Name_Ar] as Govt_Entity_Ar    
       , ldsub.[Name_En] as SubjectName_En  
       , ldsub.[Name_Ar] as SubjectName_Ar    
       , ld.CreatedDate as CreatedDate  
       , ld.AppliedDate as PublishedDate  
       , ld.IsAllowedToModify  
       , ld.ReadOnlyContent  
	   , ld.DocumentReviewerName AS DocumentReviewerName  
	   , ld.DocumentReviewDate AS DocumentReviewDate  
       , us.UserName as Issuer        
       , dc.Catalog_Name_Ar as Catalog_Ar  
       , dc.Catalog_Name_En as Catalog_En        
       , ldt.Name_Ar as Type_Ar, ldt.Name_En As Type_En        
       , lds.[Name] As StatusName  
       , ans.UserName as UserName  
       , anr.[Name] as RoleName  
from LDS_DOCUMENT ld        
LEFT JOIN LDS_DOCUMENT_CATALOG dc ON ld.CatalogId = dc.CatalogId        
LEFT JOIN LDS_DOCUMENT_TYPE ldt ON ld.TypeId = ldt.TypeId        
LEFT JOIN LDS_DOCUMENT_APPROVAL_STATUS lds ON lds.DecisionId=ld.Status        
LEFT JOIN UMS_USER us ON ld.IssuedBy = us.Id    
LEFT JOIN CMS_GOVERNMENT_ENTITY_G2G_LKP ge ON ld.EntityId = ge.EntityId    
LEFT JOIN LDS_DOCUMENT_SUBJECT ldsub ON ld.EntityId = ldsub.SubjectId  
LEFT JOIN UMS_USER ans ON ld.UserId = ans.Id  
LEFT JOIN UMS_ROLE anr ON ld.RoleId = anr.Id    
WHERE ld.IsDeleted = 0      
AND lds.DecisionId = 2  
ORDER BY ld.Number DESC    
SET ROWCOUNT 0        
SET NOCOUNT OFF        
RETURN         
END    
GO



/****** Object:  StoredProcedure [dbo].[pDocumentListFiltered]    Script Date: 26/10/2022 5:21:05 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER Procedure [dbo].[pDocumentListFiltered]            
AS            
BEGIN            
Select distinct ld.DocumentId      
  , ld.Title   
  , ld.CatalogId      
  , ld.Comment      
  , ld.AppliedDate      
  , ld.Content      
  , ld.CreatedBy      
  , ld.Number            
  , ld.[Description]       
  , ld.EntityId      
  , ld.IssuedBy      
  , ld.IssueDate      
  , ld.ModifiedBy      
  , ld.ModifiedDate      
  , ld.Reason, ld.Reference        
  , ld.IsAllowedToModify      
  , ld.ReadOnlyContent        
  , ge.[Name_En] as Govt_Entity_En      
  , ge.[Name_Ar] as Govt_Entity_Ar        
  , ldsub.[Name_En] as SubjectName_En      
  , ldsub.[Name_Ar] as SubjectName_Ar        
  , ld.CreatedDate as CreatedDate      
  , ld.AppliedDate as PublishedDate            
  , us.UserName as Issuer            
  , dc.Catalog_Name_Ar as Catalog_Ar      
  , dc.Catalog_Name_En as Catalog_En            
  , ldt.Name_Ar as Type_Ar      
  , ldt.Name_En As Type_En         
  , ld.[Status]          
  , lds.[Name] As StatusName      
  , ans.UserName AS UserName    
  , ld.DocumentReviewerName AS DocumentReviewerName    
  , ld.DocumentReviewDate AS DocumentReviewDate    
  , anr.[Name] AS RoleName      
from LDS_DOCUMENT ld            
LEFT JOIN LDS_DOCUMENT_CATALOG dc ON ld.CatalogId = dc.CatalogId            
LEFT JOIN LDS_DOCUMENT_TYPE ldt ON ld.TypeId = ldt.TypeId            
LEFT JOIN LDS_DOCUMENT_APPROVAL_STATUS lds ON lds.DecisionId=ld.Status            
LEFT JOIN UMS_USER us ON ld.IssuedBy = us.Id        
LEFT JOIN CMS_GOVERNMENT_ENTITY_G2G_LKP ge ON ld.EntityId = ge.EntityId        
LEFT JOIN LDS_DOCUMENT_SUBJECT ldsub ON ld.EntityId = ldsub.SubjectId      
LEFT JOIN UMS_USER ans ON ld.CreatedBy = ans.Email    
LEFT JOIN UMS_ROLE anr ON ld.RoleId = anr.Id      
WHERE ld.IsDeleted = 0    
AND dc.Catalog_Name_En !='Default'    
ORDER BY ld.Number DESC           
SET ROWCOUNT 0            
SET NOCOUNT OFF            
RETURN             
END 
GO


/****** Object:  StoredProcedure [dbo].[pDocumentPublishUnPublishListFiltered]    Script Date: 26/10/2022 5:21:48 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER Procedure [dbo].[pDocumentPublishUnPublishListFiltered]          
AS          
BEGIN          
  Select distinct ld.DocumentId  
    , ld.Title  
    , ld.CatalogId  
    , ld.[Status]  
    , ld.Comment  
    , ld.AppliedDate  
    , ld.Content
	, ld.CreatedBy          
    , ld.Number  
    , ld.[Description] 
    , ld.EntityId  
    , ld.IssuedBy  
    , ld.IssueDate  
    , ld.ModifiedBy  
    , ld.ModifiedDate  
    , ld.Reason          
    , ld.Reference  
    , ld.IsAllowedToModify  
    , ld.DocumentReviewerName AS DocumentReviewerName  
    , ld.DocumentReviewDate AS DocumentReviewDate  
    , ld.ReadOnlyContent    
    , ldss.[Name_En] as SubjectName_En  
    , ldss.[Name_Ar] as SubjectName_Ar    
    , ge.[Name_En] as Govt_Entity_En  
    , ge.[Name_Ar] as Govt_Entity_Ar    
    , ld.CreatedDate as CreatedDate  
    , ld.AppliedDate as PublishedDate          
    , us.UserName as Issuer          
    , dc.Catalog_Name_Ar as Catalog_Ar  
    , dc.Catalog_Name_En as Catalog_En          
    , ldt.Name_Ar as Type_Ar  
    , ldt.Name_En As Type_En          
    , lds.[Name] As StatusName  
    , ans.UserName as UserName  
    , anr.[Name] as RoleName  
      
  from LDS_DOCUMENT ld         
  LEFT JOIN LDS_DOCUMENT_SUBJECT ldss ON ld.SubjectId = ldss.SubjectId     
  LEFT JOIN CMS_GOVERNMENT_ENTITY_G2G_LKP ge ON ld.EntityId = ge.EntityId     
  LEFT JOIN LDS_DOCUMENT_CATALOG dc ON ld.CatalogId = dc.CatalogId          
  LEFT JOIN LDS_DOCUMENT_TYPE ldt ON ld.TypeId = ldt.TypeId          
  LEFT JOIN LDS_DOCUMENT_APPROVAL_STATUS lds ON lds.DecisionId=ld.[Status]          
  LEFT JOIN UMS_USER us ON ld.IssuedBy = us.Id  
  LEFT JOIN UMS_USER ans ON ld.UserId = ans.Id  
  LEFT JOIN UMS_ROLE anr ON ld.RoleId = anr.Id          
  WHERE ld.[Status] ='64'or ld.[Status]='128' or ld.[Status]='4'  
  ORDER BY ld.Number DESC  
SET ROWCOUNT 0          
SET NOCOUNT OFF          
RETURN           
END    
GO

-- [dbo].[pGetDraftCaseRequestList] '3FA85F64-5717-4562-B3FC-2C963F66AFE6', '436e82d2-70d8-455c-a643-7909b8689667'   
if exists (select * from sysobjects where id = object_id('[dbo].[pGetDraftCaseRequestList]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pNotificationList]
GO  

create or  alter    PROC [dbo].[pGetDraftCaseRequestList]      
(     
@ReferenceId  uniqueIdentifier   
)    
AS      
BEGIN      
select cdt.*,      
at.Type_En AS TypeEn,      
at.Type_Ar AS TypeAr,      
sdds.NameEn AS StatusEn,      
sdds.NameAr AS StatusAr      
from CMS_DRAFTED_TEMPLATE cdt   
LEFT JOIN CMS_REGISTERED_CASE crc on cdt.ReferenceId = crc.CaseId
LEFT JOIN ATTACHMENT_TYPE at ON at.AttachmentTypeId = cdt.AttachmentTypeId      
LEFT JOIN CMS_DRAFT_DOCUMENT_STATUS sdds ON sdds.Id = cdt.StatusId      
WHERE cdt.IsDeleted != 1  
AND  crc.CaseId=@ReferenceId OR @ReferenceId IS NULL OR @ReferenceId='00000000-0000-0000-0000-000000000000'    
ORDER BY cdt.DraftNumber desc      
END   
GO
PRINT 'Created [pGetDraftCaseRequestList]'
GO
/****** Object:  StoredProcedure [dbo].[pDocumentSelAdvanceSearch]    Script Date: 26/10/2022 5:22:18 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

  
    
    
CREATE OR ALTER   Procedure [dbo].[pDocumentSelAdvanceSearch]                   
(              
  @catalogue uniqueidentifier = NULL                        
, @documentType int = NULL                        
, @documentTitle NVARCHAR(100) = NULL                        
, @documentNumber int = NULL                        
, @issueDate datetime = Null                        
, @documentIssuer NVARCHAR(50) = NULL                        
, @status int = NULL        
, @startDate datetime = Null                        
, @endDate datetime = Null          
)              
AS                        
begin              
Select distinct ld.DocumentId    
            , ld.IsAllowedToModify    
   , ld.ReadOnlyContent    
   , ld.Title   
   , ld.CatalogId    
   , ld.Comment    
   , ld.AppliedDate    
   , ld.Content, ld.CreatedBy, ld.Number    
   , ld.[Description]   
   , ld.EntityId    
   , ld.IssuedBy    
   , ld.IssueDate    
   , ld.ModifiedBy     
   , ld.ModifiedDate    
   , ld.Reason    
   , ld.Reference    
   , ld.CreatedDate as CreatedDate    
   , ld.AppliedDate as PublishedDate    
   , us.UserName as Issuer     
   , dc.Catalog_Name_Ar as Catalog_Ar    
   , dc.Catalog_Name_En as Catalog_En    
   , ldt.Name_Ar as Type_Ar    
   , ldt.Name_En As Type_En    
   , lds.[Name] As StatusName     
   , ld.[Status]     
   , lSub.Name_En as SubjectName_En     
   , lSub.[Name_Ar] as SubjectName_Ar     
   , ge.[Name_En] as Govt_Entity_En     
   , ge.[Name_Ar] as Govt_Entity_Ar    
   , us.UserName as UserName    
   , anr.[Name] as RoleName
   , ld.DocumentReviewerName
   , ld.DocumentReviewDate
from LDS_DOCUMENT ld                        
LEFT JOIN LDS_DOCUMENT_CATALOG dc ON ld.CatalogId = dc.CatalogId                        
LEFT JOIN LDS_DOCUMENT_TYPE ldt ON ld.TypeId = ldt.TypeId                        
LEFT JOIN LDS_DOCUMENT_APPROVAL_STATUS lds ON lds.DecisionId = ld.[Status]                        
LEFT JOIN UMS_USER us ON ld.IssuedBy = us.Id                 
LEFT JOIN CMS_GOVERNMENT_ENTITY_G2G_LKP ge ON ld.EntityId = ge.EntityId          
LEFT JOIN LDS_DOCUMENT_SUBJECT lSub ON ld.SubjectId = lSub.SubjectId     
LEFT JOIN UMS_ROLE anr ON ld.RoleId = anr.Id                      
Where ld.IsDeleted != 1                        
AND (dc.CatalogId = @catalogue OR @catalogue IS NULL OR @catalogue='00000000-0000-0000-0000-000000000000')                        
AND (us.Id = @documentIssuer OR @documentIssuer IS NULL OR @documentIssuer = '')                        
AND (ldt.TypeId = @documentType OR @documentType IS NULL OR @documentType = '0')                        
AND (ld.Title LIKE '%'+ @documentTitle +'%' OR @documentTitle IS NULL OR @documentTitle = '')                        
AND (ld.Number = @documentNumber OR @documentNumber IS NULL OR @documentNumber = '0')                        
AND (cast(ld.IssueDate As date) = @issueDate OR @issueDate IS NULL OR @issueDate = '')          
AND ( (CAST(ld.CreatedDate AS DATE) >= @startDate) Or @startDate Is Null OR @startDate = '')          
AND ( (CAST(ld.CreatedDate AS DATE) <= @endDate) OR @endDate Is Null OR @endDate = '')                        
AND (lds.DecisionId = @status OR @status IS NULL OR @status = '0')    
ORDER BY ld.Number DESC          
SET ROWCOUNT 0                        
SET NOCOUNT OFF                        
RETURN               
end           
GO


/****** Object:  StoredProcedure [dbo].[pDocumentSelAdvanceSearchForMask]    Script Date: 26/10/2022 5:22:44 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

  
CREATE OR ALTER Procedure [dbo].[pDocumentSelAdvanceSearchForMask]                     
(                
	@catalogue uniqueidentifier = NULL, 
	@documentType int = NULL, 
	@documentTitle NVARCHAR(100) = NULL, 
	@documentNumber int = NULL, 
	@issueDate datetime = Null, 
	@documentIssuer NVARCHAR(50) = NULL, 
	@status int = NULL, 
	@startDate datetime = Null, 
	@endDate datetime = Null            
)                
AS                          
begin                
Select distinct ld.DocumentId    
     , ld.IsAllowedToModify    
     , ld.ReadOnlyContent    
     , ld.Title       
     , ld.[Status]    
     , ld.CatalogId    
     , ld.Comment    
     , ld.AppliedDate    
     , ld.Content    
     , ld.CreatedBy    
     , ld.Number    
     , ld.[Description] 
	 , ld.DocumentReviewDate  
	 , ld.DocumentReviewerName  
     , ld.EntityId    
     , ld.IssuedBy, ld.IssueDate     
     , ld.ModifiedBy    
     , ld.ModifiedDate    
     , ld.Reason    
     , ld.Reference    
     , ld.CreatedDate as CreatedDate    
     , ld.AppliedDate as PublishedDate     
     , us.UserName as Issuer         
     , dc.Catalog_Name_Ar as Catalog_Ar    
     , dc.Catalog_Name_En as Catalog_En    
     , ldt.Name_Ar as Type_Ar    
     , ldt.Name_En As Type_En    
     , lds.Name As StatusName                  
     , lSub.Name_En as SubjectName_En          
     , lSub.[Name_Ar] as SubjectName_Ar          
     , ge.[Name_En] as Govt_Entity_En          
     , ge.[Name_Ar] as Govt_Entity_Ar    
     , ans.UserName as UserName    
     , anr.[Name] as RoleName          
from LDS_DOCUMENT ld                          
LEFT JOIN LDS_DOCUMENT_CATALOG dc ON ld.CatalogId = dc.CatalogId                          
LEFT JOIN LDS_DOCUMENT_TYPE ldt ON ld.TypeId = ldt.TypeId                          
LEFT JOIN LDS_DOCUMENT_APPROVAL_STATUS lds ON lds.DecisionId=ld.Status                          
LEFT JOIN UMS_USER us ON ld.IssuedBy = us.Id                   
LEFT JOIN CMS_GOVERNMENT_ENTITY_G2G_LKP ge ON ld.EntityId = ge.EntityId            
LEFT JOIN LDS_DOCUMENT_SUBJECT lSub ON ld.SubjectId = lSub.SubjectId    
LEFT JOIN UMS_USER ans ON ld.UserId = ans.Id    
LEFT JOIN UMS_ROLE anr ON ld.RoleId = anr.Id          
                          
Where ld.IsDeleted != 1 AND lds.DecisionId = '64'                      
AND (dc.CatalogId = @catalogue OR @catalogue IS NULL)                          
AND (us.Id = @documentIssuer OR @documentIssuer IS NULL OR @documentIssuer='')                          
AND (ldt.TypeId = @documentType OR @documentType IS NULL OR @documentType='0')                          
AND (ld.Title LIKE '%'+@documentTitle+'%' OR @documentTitle IS NULL OR @documentTitle='')                          
AND (ld.Number = @documentNumber OR @documentNumber IS NULL OR @documentNumber='0')                          
AND (cast(ld.IssueDate As date) = @issueDate OR @issueDate IS NULL OR @issueDate='')            
AND ((ld.CreatedDate >= @startDate) Or @startDate Is Null OR @startDate='')            
AND ((ld.CreatedDate < @endDate) OR @endDate Is Null OR @endDate='')                          
AND (lds.DecisionId = @status OR @status IS NULL OR @status='0')    
ORDER BY ld.Number DESC         
SET ROWCOUNT 0                          
SET NOCOUNT OFF                          
RETURN                 
end          
GO


/****** Object:  StoredProcedure [dbo].[pDocumentViewDetails]    Script Date: 26/10/2022 5:23:11 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER Procedure [dbo].[pDocumentViewDetails]       
      @documentId uniqueidentifier = NULL      
AS           
Select distinct ld.*    
        , lds.[Name_En] as SubjectName_En    
        , lds.[Name_Ar] as SubjectName_Ar    
        , ldc.[Catalog_Name_En] as Catalog_En    
     , ldc.[Catalog_Name_Ar] as Catalog_Ar    
     , ldt.[Name_En] as Type_En    
     , ldt.[Name_Ar] as Type_Ar    
     , ge.[Name_En] as Govt_Entity_En    
     , ge.[Name_Ar] as Govt_Entity_Ar    
        , anu.[UserName] as Issuer    
     , ld.[AppliedDate] as PublishedDate    
     , ldas.[Name] As StatusName  
     , ans.UserName as UserName  
     , anr.Name as RoleName    
    
 from LDS_DOCUMENT ld      
 LEFT JOIN LDS_DOCUMENT_SUBJECT lds       
  ON ld.SubjectId = lds.SubjectId    
  LEFT JOIN LDS_DOCUMENT_CATALOG ldc    
  ON ld.CatalogId = ldc.CatalogId    
 LEFT JOIN LDS_DOCUMENT_TYPE ldt       
  ON ld.TypeId = ldt.TypeId      
 LEFT JOIN CMS_GOVERNMENT_ENTITY_G2G_LKP ge       
  ON ld.EntityId = ge.EntityId      
 LEFT JOIN UMS_USER anu       
  ON ld.IssuedBy = anu.Id    
 LEFT JOIN LDS_DOCUMENT_APPROVAL_STATUS ldas        
 ON ldas.DecisionId=ld.Status  
LEFT JOIN UMS_USER ans  
ON ld.UserId = ans.Id  
LEFT JOIN UMS_ROLE anr  
ON ld.RoleId = anr.Id     
 Where      
 ld.IsDeleted = 0    
 AND (ld.DocumentId = @documentId OR @documentId IS NULL OR @documentId='00000000-0000-0000-0000-000000000000')      
      
 SET ROWCOUNT 0          
 SET NOCOUNT OFF          
 RETURN   
GO

if exists (select * from sysobjects where id = object_id('[dbo].[pCaseRequestViewDetail]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pCaseRequestViewDetail]
GO

/****** Object:  StoredProcedure [dbo].[pCaseRequestViewDetail]    Script Date: 10/26/2022 11:57:26 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER PROCEDURE [dbo].[pCaseRequestViewDetail]                      
(                                  
  @RequestId uniqueidentifier = NULL                   
)                 
AS                  
BEGIN        
   Select cc.RequestId              
   , cc.RequestNumber              
   , cc.RequestDate              
   , cc.StatusId              
   , cs.Name_En as StatusName_En              
   , cs.Name_Ar as StatusName_Ar              
   , cc.GovtEntityId              
   , ge.Name_En as EntityName_En              
   , ge.Name_Ar as EntityName_Ar              
   , cc.DepartmentId              
   , dp.Name_En as DepartmentName_En              
   , dp.Name_Ar as DepartmentName_Ar               
   , cc.RequestTypeId                             
   , rt.Name_En as RequestType_Name_En      
   , rt.Name_Ar as RequestType_Name_Ar               
   , cc.SectorTypeId              
   , os.Name_En as SectorName_En              
   , os.Name_Ar as SectorName_Ar              
   , cc.PriorityId              
   , pc.Name_En as PriorityName_En              
   , pc.Name_Ar as PriorityName_Ar              
   , cc.SubTypeId              
   , st.Name_En as SubType_En              
   , st.Name_Ar as SubType_Ar              
   , cc.ClaimAmount              
   , cc.[Subject]             
   , cc.IsConfidential              
   , cc.Remarks              
   , cc.CaseRequirements              
   , cc.ReferenceNo              
   , cc.ReferenceDate              
   , cc.CreatedBy              
   , cc.CreatedDate              
   , cc.ModifiedBy              
   , cc.ModifiedDate              
   , cc.DeletedBy              
   , cc.DeletedDate              
   , cc.IsDeleted              
   , cc.ReviewedDate            
   , cc.ReceivedDate            
   , cc.ApprovedDate           
   , CourtTypeId As CourtTypeId        
   , cctgl.Name_En As Court_Type_Name_En          
   , cctgl.Name_Ar As Court_Type_Name_Ar          
   , ISNULL(uurc.FirstName_En,'') + ' ' + ISNULL(uurc.SecondName_En,'') + ' ' + ISNULL(uurc.LastName_En,'') AS ReceiverNameEn            
   , ISNULL(uurc.FirstName_Ar,'') + ' ' + ISNULL(uurc.SecondName_Ar,'') + ' ' + ISNULL(uurc.LastName_Ar,'') AS ReceiverNameAr            
   , ISNULL(uurv.FirstName_En,'') + ' ' + ISNULL(uurc.SecondName_En,'') + ' ' + ISNULL(uurv.LastName_En,'') AS ReviewerNameEn            
   , ISNULL(uurv.FirstName_Ar,'') + ' ' + ISNULL(uurv.SecondName_Ar,'') + ' ' + ISNULL(uurv.LastName_Ar,'') AS ReviewerNameAr            
   , ISNULL(uuap.FirstName_En,'') + ' ' + ISNULL(uuap.SecondName_En,'') + ' ' + ISNULL(uuap.LastName_En,'') AS ApproverNameEn            
   , ISNULL(uuap.FirstName_Ar,'') + ' ' + ISNULL(uuap.SecondName_Ar,'') + ' ' + ISNULL(uuap.LastName_Ar,'') AS ApproverNameAr            
   , ISNULL(geus.FirstName_En,'') + ' ' + ISNULL(geus.SecondName_En,'') + ' ' + ISNULL(geus.LastName_En,'') AS GEUserNameEn            
   , ISNULL(geus.FirstName_Ar,'') + ' ' + ISNULL(geus.SecondName_Ar,'') + ' ' + ISNULL(geus.LastName_Ar,'') AS GEUserNameAr     
   , cc.TransferStatusId  
   FROM CMS_CASE_REQUEST cc              
   LEFT JOIN CMS_CASE_REQUEST_STATUS_G2G_LKP cs ON cs.Id = cc.StatusId              
   LEFT JOIN CMS_GOVERNMENT_ENTITY_G2G_LKP ge ON ge.EntityId = cc.GovtEntityId              
   LEFT JOIN Department dp ON dp.Id = cc.DepartmentId              
   LEFT JOIN CMS_OPERATING_SECTOR_TYPE_G2G_LKP os ON os.Id = cc.SectorTypeId                     
   LEFT JOIN CMS_REQUEST_TYPE_G2G_LKP rt on rt.Id = cc.RequestTypeId      
   LEFT JOIN CMS_PRIORITY_G2G_LKP pc ON pc.Id = cc.PriorityId              
   LEFT JOIN CMS_SUBTYPE_G2G_LKP st ON st.Id = cc.SubTypeId            
   LEFT JOIN UMS_USER uurc ON uurc.UserName = cc.ReceivedBy            
   LEFT JOIN UMS_USER uurv ON uurv.UserName = cc.ReviewedBy            
   LEFT JOIN UMS_USER uuap ON uuap.UserName = cc.ApprovedBy            
   LEFT JOIN UMS_USER geus ON geus.Email = cc.CreatedBy            
   LEFT JOIN CMS_COURT_TYPE_G2G_LKP cctgl ON cc.CourtTypeId = cctgl.Id         
   Where cc.IsDeleted = 0 
   AND (cc.RequestId = @RequestId OR @RequestId IS NULL OR @RequestId='00000000-0000-0000-0000-000000000000')                    
END     

/****** Object:  StoredProcedure [dbo].[pCasePartyViewDetail]    Script Date: 10/26/2022 4:26:49 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[pCasePartyViewDetail]        
(                    
  @Id uniqueidentifier = NULL     
     
)   
AS    
BEGIN     
---
   Select cpl.Id
   , cpl.CategoryId
   , cpc.Name_En as CategoryName_En
   , cpc.Name_Ar as CategoryName_Ar
   , cpl.TypeId
   , cpt.Name_En as  TypeName_En
   , cpt.Name_Ar  as  TypeName_Ar
   , cpl.MinistryId
   , cm.Name_En  as  MinistryName_En
   , cm.Name_Ar as MinistryName_Ar
   , cpl.Name
   , cpl.CivilId
   , cpl.RequestId
   , cpl.Representative
   , cpl.CRN
   , cpl.CreatedBy
   , cpl.CreatedDate
   , cpl.DeletedBy
   , cpl.DeletedDate
   , cpl.IsDeleted
   , cpl.ModifiedBy
   , cpl.ModifiedDate
   
  
  

   from CMS_CASE_PARTY_LINK cpl
    LEFT JOIN CMS_CASE_PARTY_CATEGORY_G2G_LKP cpc ON cpc.Id = cpl.CategoryId
	LEFT JOIN CMS_CASE_PARTY_TYPE_G2G_LKP cpt ON cpt.Id = cpl.TypeId
	LEFT JOIN  CMS_MINISTRY_G2G_LKP cm ON cm.Id = cpl.MinistryId 
 Where      
 cpl.IsDeleted = 0    
 AND (cpl.RequestId = @Id OR @Id IS NULL OR @Id='00000000-0000-0000-0000-000000000000')      
  END 
   
GO

/****** Object:  StoredProcedure [dbo].[pGetCaseRequestStatusHistory]    Script Date: 10/26/2022 4:27:31 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER PROCEDURE [dbo].[pGetCaseRequestStatusHistory]          
(                      
  @RequestId uniqueidentifier = NULL,
  @HistoryId uniqueidentifier = NULL      
)     
AS      
BEGIN   
	Select crs.HistoryId 
	, crs.RequestId
	, cre.Name_En as EventEn  
	, cre.Name_Ar as EventAr  
	, crs.EventId  
	, cwr.Name_En as StatusEn  
	, cwr.Name_Ar as StatusAr  
	, crs.StatusId  
	, crs.Remarks  
	, crs.CreatedBy  
	, crs.CreatedDate  
	, crs.ModifiedBy  
	, crs.ModifiedDate  
	, crs.DeletedBy  
	, crs.DeletedDate  
	, crs.IsDeleted 
	FROM CMS_CASE_REQUEST_STATUS_HISTORY crs  
	LEFT JOIN CMS_CASE_REQUEST_EVENT_G2G_LKP cre ON cre.Id = crs.EventId  
	LEFT JOIN CMS_CASE_REQUEST_STATUS_G2G_LKP cwr ON cwr.Id = crs.StatusId  
	Where crs.IsDeleted = 0      
	AND (crs.RequestId = @RequestId OR @RequestId IS NULL OR @RequestId='00000000-0000-0000-0000-000000000000') 
	AND (crs.HistoryId = @HistoryId OR @HistoryId IS NULL OR @HistoryId='00000000-0000-0000-0000-000000000000') 	
END
   
GO



------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------
--<History Author = 'Hassan Abbas' Date='2022-10-26' Version="1.0" Branch="master"> ABOVE SCRIPT EXECUTED ON FATWA_DB_DEV</History>
------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------



--- pCmsCaseRequestList
if exists (select * from sysobjects where id = object_id('[dbo].[pCmsCaseRequestList]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pCmsCaseRequestList]
GO


CREATE OR ALTER  PROCEDURE [dbo].[pCmsCaseRequestList]
(  
@requestNumber int = null,  
@statusId int =null,  
@subject NVARCHAR(1000) =null,  
@sectorTypeId int=null,  
@requestFrom datetime=null,  
@requestTo datetime=null  
)  
AS    
Begin    
Select distinct ccr.*,    
    
--Department    
cd.Name_En as Department_Name_En,    
cd.Name_Ar as Department_Name_Ar,    
--GovtEntity    
cg.Name_En as GovermentEntity_Name_En,    
cg.Name_Ar as GovermentEntity_Name_Ar,    
--SectorType    
co.Name_En as SectorType_Name_En,    
co.Name_Ar as SectorType_Name_Ar,    
--Subtype    
cs.Name_En as Subtype_Name_En,    
cs.Name_Ar as Subtype_Name_Ar,    
--Priorty    
cp.Name_En as Priority_Name_En,    
cp.Name_Ar as Priority_Name_Ar,    
--Status    
ccrs.Name_En as Status_Name_En,    
ccrs.Name_Ar as Status_Name_Ar    
    
FROM CMS_CASE_REQUEST ccr    
    
left join Department cd on  ccr.DepartmentId = cd.Id    
left join CMS_GOVERNMENT_ENTITY_G2G_LKP cg on  ccr.GovtEntityId = cg.EntityId    
left join CMS_OPERATING_SECTOR_TYPE_G2G_LKP co on ccr.SectorTypeId= co.Id    
left join CMS_SUBTYPE_G2G_LKP cs on ccr.SubTypeId = cs.Id    
left join CMS_PRIORITY_G2G_LKP cp on ccr.PriorityId = cp.Id    
left join CMS_CASE_REQUEST_STATUS_G2G_LKP ccrs on ccr.StatusId = ccrs.Id    
WHERE ccr.IsDeleted != 1  
AND (ccr.RequestNumber=@requestNumber OR @requestNumber IS NULL OR @requestNumber='0')  
AND (ccr.StatusId=@statusId OR @statusId IS NULL OR @statusId='0')  
AND (ccr.Subject=@subject OR @subject IS NULL OR @subject='')  
AND(ccr.SectorTypeId=@sectorTypeId OR @sectorTypeId IS NULL OR @sectorTypeId='0')  
AND(CAST(ccr.RequestDate as date)>=@requestFrom OR @requestFrom IS NULL OR @requestFrom='')  
AND(CAST(ccr.RequestDate as date)<=@requestTo OR @requestTo IS NULL OR @requestTo='')  
   
ORDER BY ccr.RequestNumber desc
END    
   
GO


--- pCaseRequestSelAll
if exists (select * from sysobjects where id = object_id('[dbo].[pCaseRequestSelAll]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pCaseRequestSelAll]
GO


--- pCmsCaseFileList
if exists (select * from sysobjects where id = object_id('[dbo].[pCmsCaseFileList]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pCmsCaseFileList]
GO

CREATE OR ALTER  PROCEDURE [dbo].[pCmsCaseFileList]
(
@fileNumber NVARCHAR(1000) =null,
@statusId int = null,
@createdFrom datetime = null,
@createdTo datetime = null,
@modifiedFrom datetime = null,
@modifiedTo datetime = null
)
As

Select distinct  ccf.*,
--Case request
ccr.RequestNumber, 
ccr.RequestDate, 
ccr.Subject,

--Status    
ccfs.Name_En as StatusNameEn,    
ccfs.Name_Ar as StatusNameAr,    

-- Government Entity
cge.Name_En as GovernmentEntityNameEn,
cge.Name_Ar as GovernmentEntityNameAr,
-- Priority
cpg.Name_En as PriorityNameEn,
cpg.Name_Ar as PriorityNameAr,

-- Operating Sector
ost.Name_En as OperatingSectorNameEn,
ost.Name_Ar as OperatingSectorNameAr

from CMS_CASE_FILE ccf
left join CMS_CASE_FILE_STATUS_G2G_LKP ccfs on ccf.StatusId= ccfs.Id
left join CMS_CASE_REQUEST ccr on ccf.RequestId = ccr.RequestId 
left join CMS_GOVERNMENT_ENTITY_G2G_LKP cge on cge.EntityId=ccr.GovtEntityId
left join CMS_PRIORITY_G2G_LKP cpg on cpg.Id=ccr.PriorityId
left join CMS_OPERATING_SECTOR_TYPE_G2G_LKP  ost on ost.Id=ccr.SectorTypeId
WHERE ccr.IsDeleted != 1  
AND (ccf.FileNumber=@fileNumber OR @fileNumber IS NULL OR @fileNumber='')  
AND (ccf.StatusId=@statusId OR @statusId IS NULL OR @statusId='0')  
AND(CAST(ccf.CreatedDate as date)>=@createdFrom OR @createdFrom IS NULL OR @createdFrom='')  
AND(CAST(ccf.CreatedDate as date)<=@createdTo OR @createdTo IS NULL OR @createdTo='')  
AND(CAST(ccf.ModifiedDate as date)>=@modifiedFrom OR @modifiedFrom IS NULL OR @modifiedFrom='')  
AND(CAST(ccf.ModifiedDate as date)<=@modifiedTo OR @modifiedTo IS NULL OR @modifiedTo='')  

GO

------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------
--<History Author = 'Hassan Abbas' Date='2022-11-01' Version="1.0" Branch="master"> ABOVE SCRIPT EXECUTED ON FATWA_DB_DEV</History>
------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------
  
--- pCmsCaseFileDetailById
if exists (select * from sysobjects where id = object_id('[dbo].[pCmsCaseFileDetailById]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pCmsCaseFileDetailById]
GO
  
CREATE OR ALTER  PROCEDURE [dbo].[pCmsCaseFileDetailById]  
(  
@fileId uniqueidentifier  
)  
As  
  
Select distinct  ccf.*,  
  
--Status      
ccfs.Name_En as StatusNameEn,      
ccfs.Name_Ar as StatusNameAr      
      
from CMS_CASE_FILE ccf  
left join CMS_CASE_FILE_STATUS_G2G_LKP ccfs on ccf.StatusId= ccfs.Id  
left join CMS_CASE_REQUEST ccr on ccf.RequestId = ccr.RequestId   
WHERE ccr.IsDeleted != 1    
and (ccf.FileId = @fileId OR @fileId IS NULL OR @fileId = '00000000-0000-0000-0000-000000000000')     




--- pCaseRequestViewDetail
if exists (select * from sysobjects where id = object_id('[dbo].[pCaseRequestViewDetail]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pCaseRequestViewDetail]
GO

CREATE OR ALTER PROCEDURE [dbo].[pCaseRequestViewDetail]          
(                      
  @RequestId uniqueidentifier = NULL       
       
)     
AS      
BEGIN       
  
   Select cc.RequestId  
   , cc.RequestNumber  
   , cc.RequestDate  
   , cc.StatusId  
   , cs.Name_En as StatusName_En  
   , cs.Name_Ar as StatusName_Ar  
   , cc.GovtEntityId  
   , ge.Name_En as EntityName_En  
   , ge.Name_Ar as EntityName_Ar  
   , cc.DepartmentId  
   , dp.Name_En as DepartmentName_En  
   , dp.Name_Ar as DepartmentName_Ar  
   , cc.SectorTypeId  
   , os.Name_En as SectorName_En  
   , os.Name_Ar as SectorName_Ar  
   , cc.PriorityId  
   , pc.Name_En as PriorityName_En  
   , pc.Name_Ar as PriorityName_Ar  
   , cc.SubTypeId  
   , st.Name_En as SubType_En  
   , st.Name_Ar as SubType_Ar  
   , cc.ClaimAmount  
   , cc.Subject  
   , cc.IsConfidential  
   , cc.Remarks  
   , cc.CaseRequirements  
   , cc.ReferenceNo  
   , cc.ReferenceDate  
   , cc.CreatedBy  
   , cc.CreatedDate  
   , cc.ModifiedBy  
   , cc.ModifiedDate  
   , cc.DeletedBy  
   , cc.DeletedDate  
   , cc.IsDeleted  
   , cc.ReviewedDate
   , cc.ReceivedDate
   , cc.ApprovedDate
   , ISNULL(uurc.FirstName_En,'') + ' ' + ISNULL(uurc.SecondName_En,'') + ' ' + ISNULL(uurc.LastName_En,'') AS ReceiverNameEn
   , ISNULL(uurc.FirstName_Ar,'') + ' ' + ISNULL(uurc.SecondName_Ar,'') + ' ' + ISNULL(uurc.LastName_Ar,'') AS ReceiverNameAr
   , ISNULL(uurv.FirstName_En,'') + ' ' + ISNULL(uurc.SecondName_En,'') + ' ' + ISNULL(uurv.LastName_En,'') AS ReviewerNameEn
   , ISNULL(uurv.FirstName_Ar,'') + ' ' + ISNULL(uurv.SecondName_Ar,'') + ' ' + ISNULL(uurv.LastName_Ar,'') AS ReviewerNameAr
   , ISNULL(uuap.FirstName_En,'') + ' ' + ISNULL(uuap.SecondName_En,'') + ' ' + ISNULL(uuap.LastName_En,'') AS ApproverNameEn
   , ISNULL(uuap.FirstName_Ar,'') + ' ' + ISNULL(uuap.SecondName_Ar,'') + ' ' + ISNULL(uuap.LastName_Ar,'') AS ApproverNameAr
   , ISNULL(geus.FirstName_En,'') + ' ' + ISNULL(geus.SecondName_En,'') + ' ' + ISNULL(geus.LastName_En,'') AS GEUserNameEn
   , ISNULL(geus.FirstName_Ar,'') + ' ' + ISNULL(geus.SecondName_Ar,'') + ' ' + ISNULL(geus.LastName_Ar,'') AS GEUserNameAr
   from CMS_CASE_REQUEST cc  
   LEFT JOIN CMS_CASE_REQUEST_STATUS_G2G_LKP cs ON cs.Id = cc.StatusId  
   LEFT JOIN CMS_GOVERNMENT_ENTITY_G2G_LKP ge ON ge.EntityId = cc.GovtEntityId  
   LEFT JOIN Department dp ON dp.Id = cc.DepartmentId  
   LEFT JOIN CMS_OPERATING_SECTOR_TYPE_G2G_LKP os ON os.Id = cc.SectorTypeId  
   LEFT JOIN CMS_PRIORITY_G2G_LKP pc ON pc.Id = cc.PriorityId  
   LEFT JOIN CMS_SUBTYPE_G2G_LKP st ON st.Id = cc.SubTypeId
   LEFT JOIN UMS_USER uurc ON uurc.UserName = cc.ReceivedBy
   LEFT JOIN UMS_USER uurv ON uurv.UserName = cc.ReviewedBy
   LEFT JOIN UMS_USER uuap ON uuap.UserName = cc.ApprovedBy
   LEFT JOIN UMS_USER geus ON geus.Email = cc.CreatedBy
 Where        
 cc.IsDeleted = 0      
 AND (cc.RequestId = @RequestId OR @RequestId IS NULL OR @RequestId='00000000-0000-0000-0000-000000000000')        
  END

  	 
--- pCasePartyViewDetail
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pCasePartyViewDetail]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pCasePartyViewDetail]
GO

CREATE  OR ALTER PROCEDURE [dbo].[pCasePartyViewDetail]            
(                        
  @Id uniqueidentifier = NULL  
)       
AS        
BEGIN         
   Select cpl.Id    
   , cpl.CategoryId    
   , cpc.Name_En as CategoryName_En    
   , cpc.Name_Ar as CategoryName_Ar    
   , cpl.TypeId    
   , cpt.Name_En as  TypeName_En    
   , cpt.Name_Ar  as  TypeName_Ar    
   , cpl.EntityId    
   , ge.Name_En as GovtEntity_En    
   , ge.Name_Ar as GovtEntity_Ar    
   , cpl.Name    
   , cpl.CivilId    
   , cpl.ReferenceGuid    
   , cpl.Representative    
   , cpl.CRN    
   , cpl.CreatedBy    
   , cpl.CreatedDate    
   , cpl.DeletedBy    
   , cpl.DeletedDate    
   , cpl.IsDeleted    
   , cpl.ModifiedBy    
   , cpl.ModifiedDate   
   , cpl.PACINumber
   , cpl.Address
   , cpl.CompanyCivilId
   , MOCIFileNumber
   , LicenseNumber
   , MembershipNumber
   from CMS_CASE_PARTY_LINK cpl    
 LEFT JOIN CMS_CASE_PARTY_CATEGORY_G2G_LKP cpc ON cpc.Id = cpl.CategoryId    
 LEFT JOIN CMS_CASE_PARTY_TYPE_G2G_LKP cpt ON cpt.Id = cpl.TypeId    
 LEFT JOIN  CMS_GOVERNMENT_ENTITY_G2G_LKP ge ON ge.EntityId = cpl.EntityId    
 Where          
 cpl.IsDeleted = 0        
 AND (cpl.ReferenceGuid = @Id OR @Id IS NULL OR @Id='00000000-0000-0000-0000-000000000000')          
 END 

 
------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------
--<History Author = 'Hassan Abbas' Date='2022-11-03' Version="1.0" Branch="master"> ABOVE SCRIPT EXECUTED ON FATWA_DB_DEV</History>
------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------


--pCmsCaseRequestLawyerList

if exists (select * from sysobjects where id = object_id('[dbo].[pCmsCaseRequestLawyerList]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pCmsCaseRequestLawyerList]
GO
CREATE OR ALTER procedure [dbo].[pCmsCaseRequestLawyerList]


as

select distinct crl.* ,

---Lawyer
 CONCAT(uu.FirstName_En, ' ',uu.SecondName_En, ' ',uu.LastName_En) LawyerNameEn,
 CONCAT(uu.FirstName_Ar, ' ',uu.SecondName_Ar, ' ',uu.LastName_Ar) LawyerNameAr,
 ---Supervisor
 CONCAT(u.FirstName_En, ' ',u.SecondName_En, ' ',u.LastName_En) SupervisorNameEn,
 CONCAT(u.FirstName_Ar, ' ',u.SecondName_Ar, ' ',u.LastName_Ar) SupervisorNameAr

from CMS_CASE_REQUEST_LAWYER_ASSIGNMENT crl
left join CMS_CASE_REQUEST ccr on crl.RequestId=ccr.RequestId
left join UMS_USER uu on crl.LawyerId=uu.Id
left join UMS_USER u on crl.SupervisorId=u.Id
GO


--pGetCaseFileStatusHistory
if exists (select * from sysobjects where id = object_id('[dbo].[pGetCaseFileStatusHistory]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pGetCaseFileStatusHistory]
GO

CREATE OR ALTER    PROCEDURE [dbo].[pGetCaseFileStatusHistory]        
(                    
  @FileId uniqueidentifier = NULL     
     
)   
AS    
BEGIN     

   Select distinct ccf.*,
  --Event
    cfe.Name_En as EventNameEn,
    cfe.Name_Ar as EventNameAr,
    --Status
   cfs.Name_En as StatusNameEn,
   cfs.Name_Ar as StatusNameAr

   from CMS_CASE_FILE_STATUS_HISTORY ccf
    LEFT JOIN CMS_CASE_FILE_EVENT_G2G_LKP cfe ON ccf.EventId= cfe.Id
	LEFT JOIN CMS_CASE_FILE_STATUS_G2G_LKP cfs ON ccf.StatusId = cfs.Id
 Where      
 ccf.IsDeleted = 0    
 AND (ccf.FileId = @FileId OR @FileId IS NULL OR @FileId='00000000-0000-0000-0000-000000000000')      
  END 
   
GO


/****** Object:  StoredProcedure [dbo].[pCmsCaseRequestList]    Script Date: 11/7/2022 3:51:13 PM ******/

IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pCmsCaseRequestList]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pCmsCaseRequestList]
GO

  CREATE  or alter    PROCEDURE [dbo].[pCmsCaseRequestList]        
(          
@requestNumber int = null,          
@statusId int =null,          
@subject NVARCHAR(1000) =null,          
@sectorTypeId int=null,          
@requestFrom datetime=null,          
@requestTo datetime=null          
)          
AS            
Begin            
Select distinct ccr.*,            
            
--Department            
cd.Name_En as Department_Name_En,            
cd.Name_Ar as Department_Name_Ar,            
--GovtEntity            
cg.Name_En as GovermentEntity_Name_En,            
cg.Name_Ar as GovermentEntity_Name_Ar,            
--SectorType            
co.Name_En as SectorType_Name_En,            
co.Name_Ar as SectorType_Name_Ar,            
--Subtype            
cs.Name_En as Subtype_Name_En,            
cs.Name_Ar as Subtype_Name_Ar,            
--Priorty            
cp.Name_En as Priority_Name_En,            
cp.Name_Ar as Priority_Name_Ar,            
--Status            
ccrs.Name_En as Status_Name_En,            
ccrs.Name_Ar as Status_Name_Ar,       
--Court type   
cctgl.Id as CourtTypeId,    
cctgl.Name_En as Court_Type_Name_En,    
cctgl.Name_Ar as Court_Type_Name_Ar            
FROM CMS_CASE_REQUEST ccr            
            
left join Department cd on  ccr.DepartmentId = cd.Id            
left join CMS_GOVERNMENT_ENTITY_G2G_LKP cg on  ccr.GovtEntityId = cg.EntityId            
left join CMS_OPERATING_SECTOR_TYPE_G2G_LKP co on ccr.SectorTypeId= co.Id            
left join CMS_SUBTYPE_G2G_LKP cs on ccr.SubTypeId = cs.Id            
left join CMS_PRIORITY_G2G_LKP cp on ccr.PriorityId = cp.Id            
left join CMS_CASE_REQUEST_STATUS_G2G_LKP ccrs on ccr.StatusId = ccrs.Id     
left join CMS_COURT_TYPE_G2G_LKP cctgl on ccr.CourtTypeId = cctgl.Id     
WHERE ccr.IsDeleted != 1          
AND (ccr.RequestNumber = @requestNumber OR @requestNumber IS NULL OR @requestNumber='0')          
AND (ccr.StatusId = @statusId OR @statusId IS NULL OR @statusId='0')          
AND (ccr.Subject = @subject OR @subject IS NULL OR @subject = '')          
AND (ccr.SectorTypeId = @sectorTypeId OR @sectorTypeId IS NULL OR @sectorTypeId='0')          
AND (CAST(ccr.RequestDate as date) >= @requestFrom OR @requestFrom IS NULL OR @requestFrom='')         
AND (CAST(ccr.RequestDate as date) <= @requestTo OR @requestTo IS NULL OR @requestTo='')          
    
ORDER BY ccr.RequestNumber desc        
END 
 
GO

------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------
--<History Author = 'Hassan Abbas' Date='2022-11-03' Version="1.0" Branch="master"> ABOVE SCRIPT EXECUTED ON FATWA_DB_DEV</History>
------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------


/****** Object:  StoredProcedure [dbo].[pCmsCaseFileList]    Script Date: 11/7/2022 4:46:42 PM ******/
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pCmsCaseRequestList]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pCmsCaseRequestList]
GO


CREATE OR ALTER  PROCEDURE [dbo].[pCmsCaseFileList]
(
@fileNumber NVARCHAR(1000) =null,
@statusId int = null,
@createdFrom datetime = null,
@createdTo datetime = null,
@modifiedFrom datetime = null,
@modifiedTo datetime = null
)
As

Select distinct  ccf.*,
--Case request
ccr.RequestNumber, 
ccr.RequestDate, 
ccr.Subject,

--Status    
ccfs.Name_En as StatusNameEn,    
ccfs.Name_Ar as StatusNameAr,    

-- Government Entity
cge.Name_En as GovernmentEntityNameEn,
cge.Name_Ar as GovernmentEntityNameAr,
-- Priority
cpg.Name_En as PriorityNameEn,
cpg.Name_Ar as PriorityNameAr,

-- Operating Sector
ost.Name_En as OperatingSectorNameEn,
ost.Name_Ar as OperatingSectorNameAr

from CMS_CASE_FILE ccf
left join CMS_CASE_FILE_STATUS_G2G_LKP ccfs on ccf.StatusId= ccfs.Id
left join CMS_CASE_REQUEST ccr on ccf.RequestId = ccr.RequestId 
left join CMS_GOVERNMENT_ENTITY_G2G_LKP cge on cge.EntityId=ccr.GovtEntityId
left join CMS_PRIORITY_G2G_LKP cpg on cpg.Id=ccr.PriorityId
left join CMS_OPERATING_SECTOR_TYPE_G2G_LKP  ost on ost.Id=ccr.SectorTypeId
WHERE ccr.IsDeleted != 1  
AND (ccf.FileNumber=@fileNumber OR @fileNumber IS NULL OR @fileNumber='')  
AND (ccf.StatusId=@statusId OR @statusId IS NULL OR @statusId='0')  
AND(CAST(ccf.CreatedDate as date)>=@createdFrom OR @createdFrom IS NULL OR @createdFrom='')  
AND(CAST(ccf.CreatedDate as date)<=@createdTo OR @createdTo IS NULL OR @createdTo='')  
AND(CAST(ccf.ModifiedDate as date)>=@modifiedFrom OR @modifiedFrom IS NULL OR @modifiedFrom='')  
AND(CAST(ccf.ModifiedDate as date)<=@modifiedTo OR @modifiedTo IS NULL OR @modifiedTo='') 
AND ccr.StatusId<>3

GO
 
-- pMeetingList  
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pMeetingList]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pMeetingList]
GO

CREATE OR ALTER PROCEDURE [dbo].[pMeetingList]    
 @UserName nvarchar(150) = NULL        

AS                      
BEGIN                       
 SELECT CAST( row_number() OVER (ORDER BY mt.CreatedDate) as int) as SerialNo,           
  mt.MeetingId,            
  mt.[Subject],               
  mty.NameEn as [TypeEn],               
  mty.NameAr as [TypeAr],               
  mt.[Location],               
  mt.CreatedBy,          
  mts.MeetingStatusId,          
  mts.NameEn as [StatusEn],               
  mts.NameAr as [StatusAr],               
  CAST (cf.[FileNumber] as int) as [FileNumber],          
  mt.[Date] as [DateTime]              
 FROM MEET_MEETING mt              
 LEFT JOIN CMS_CASE_FILE cf on cf.FileId = mt.FileId      
 INNER JOIN MEET_MEETING_TYPE mty on mty.MeetingTypeId = mt.MeetingTypeId              
 INNER JOIN MEET_MEETING_STATUS mts on mts.MeetingStatusId = mt.MeetingStatusId 
 INNER JOIN MEET_MEETING_ATTENDEE ma on ma.MeetingId = mt.MeetingId
 LEFT JOIN UMS_USER usr on usr.FirstName_En = ma.RepresentativeName 
 WHERE mt.IsDeleted = 0  
 AND usr.UserName like '%'+ @UserName +'%' OR @UserName IS NULL    
 
 Group By mt.MeetingId,            
		mt.[Subject],               
		mty.NameEn,               
		mty.NameAr,               
		mt.[Location],               
		mt.CreatedBy,          
		mts.MeetingStatusId,          
		mts.NameEn,               
		mts.NameAr,               
		CAST (cf.[FileNumber] as int),          
		mt.[Date],
		mt.CreatedDate
 ORDER BY mt.CreatedDate DESC   
 
END 
GO

-- pMeetingAttendeeByMeetingId  
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pMeetingAttendeeByMeetingId]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pMeetingAttendeeByMeetingId]
GO

CREATE OR ALTER PROCEDURE [dbo].[pMeetingAttendeeByMeetingId]   
	@MeetingId nvarchar(50) = NULL,
	@MeetingAttendeeTypeId int = NULL
AS        
BEGIN         
	Select  CAST( row_number() OVER (ORDER BY CreatedDate) as int) as SerialNo, 
			mta.RepresentativeName, 
			ISNULL( mta.GovernmentEntityId, 0) as GovernmentEntityId,
			ISNULL( ge.Name_En, '---') as GovernmentEntityNameEn,
			ISNULL( ge.Name_Ar, '---') as GovernmentEntityNameAr,
			mta.DepartmentId,
			dpt.Name_En as DepartmentNameEn,
			dpt.Name_Ar as DepartmentNameAr,
			mta.MeetingId,
			mta.MeetingAttendeeId 

	from MEET_MEETING_ATTENDEE mta
	Left Join CMS_GOVERNMENT_ENTITY_G2G_LKP ge on ge.EntityId = mta.GovernmentEntityId
	Left Join Department dpt on dpt.Id = mta.DepartmentId

	where mta.IsDeleted = 0
	and (mta.MeetingId = @MeetingId OR @MeetingId IS NULL) 
	and (mta.MeetingAttendeeTypeId = @MeetingAttendeeTypeId OR @MeetingAttendeeTypeId IS NULL )

	ORDER BY mta.CreatedDate  
END 
GO
 
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pCmsCaseRequestLawyerList]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pCmsCaseRequestLawyerList]
GO
  
CREATE OR ALTER procedure [dbo].[pCmsCaseRequestLawyerList] 
AS 
    select distinct crl.* ,

    ---Lawyer
    CONCAT(uu.FirstName_En, ' ',uu.SecondName_En, ' ',uu.LastName_En) LawyerNameEn,
    CONCAT(uu.FirstName_Ar, ' ',uu.SecondName_Ar, ' ',uu.LastName_Ar) LawyerNameAr,
    ---Supervisor
    CONCAT(u.FirstName_En, ' ',u.SecondName_En, ' ',u.LastName_En) SupervisorNameEn,
    CONCAT(u.FirstName_Ar, ' ',u.SecondName_Ar, ' ',u.LastName_Ar) SupervisorNameAr

    from CMS_CASE_REQUEST_LAWYER_ASSIGNMENT crl
    left join CMS_CASE_REQUEST ccr on crl.RequestId=ccr.RequestId
    left join UMS_USER uu on crl.LawyerId=uu.Id
    left join UMS_USER u on crl.SupervisorId=u.Id
GO

------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------
--<History Author = '???????' Date='???????' Version="1.0" Branch="master"> ?????????????</History>
------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------

--- pCaseTemplateParametersList
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pCaseTemplateParametersList]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].pCaseTemplateParametersList
GO
CREATE OR ALTER PROC pCaseTemplateParametersList
(
@templateId INT
)
AS 
BEGIN
SELECT CTP.TemplateId,
CTP.SectionId,
P.ParameterId,
P.Name,
P.PKey
FROM CMS_TEMPLATE_PARAMETER CTP
LEFT JOIN CMS_TEMPLATE CT ON CT.Id = CTP.TemplateId
LEFT JOIN PARAMETER P ON P.ParameterId = CTP.ParameterId
WHERE CTP.TemplateId = @templateId
END


--- pCmsCaseRequestLawyerList
if exists (select * from sysobjects where id = object_id('[dbo].[pCmsCaseRequestLawyerList]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pCmsCaseRequestLawyerList]
GO

--- pCmsCaseFileAssignmentHistory
if exists (select * from sysobjects where id = object_id('[dbo].[pCmsCaseFileAssignmentHistory]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pCmsCaseFileAssignmentHistory]
GO

CREATE PROC [dbo].[pCmsCaseFileAssignmentHistory]  
(
@fileId uniqueidentifier
)
AS  
BEGIN
SELECT distinct crl.* ,  
  
---Lawyer  
 CONCAT(uu.FirstName_En, ' ',uu.SecondName_En, ' ',uu.LastName_En) LawyerNameEn,  
 CONCAT(uu.FirstName_Ar, ' ',uu.SecondName_Ar, ' ',uu.LastName_Ar) LawyerNameAr,  
 ---Supervisor  
 CONCAT(u.FirstName_En, ' ',u.SecondName_En, ' ',u.LastName_En) SupervisorNameEn,  
 CONCAT(u.FirstName_Ar, ' ',u.SecondName_Ar, ' ',u.LastName_Ar) SupervisorNameAr,  
 ---Assignor  
 CONCAT(crtb.FirstName_En, ' ',crtb.SecondName_En, ' ',crtb.LastName_En) AssignorNameEn,  
 CONCAT(crtb.FirstName_Ar, ' ',crtb.SecondName_Ar, ' ',crtb.LastName_Ar) AssignorNameAr  
  
from CMS_CASE_REQUEST_LAWYER_ASSIGNMENT crl  
left join CMS_CASE_REQUEST ccr on crl.RequestId=ccr.RequestId  
left join UMS_USER uu on crl.LawyerId=uu.Id  
left join UMS_USER u on crl.SupervisorId=u.Id  
left join UMS_USER crtb on crl.CreatedBy=crtb.Id 
--WHERE crl.RequestId = '' 
END


--- pGetCaseFileStatusHistory
if exists (select * from sysobjects where id = object_id('[dbo].[pGetCaseFileStatusHistory]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pGetCaseFileStatusHistory]
GO
   
--- pCaseFileStatusHistory
if exists (select * from sysobjects where id = object_id('[dbo].[pCaseFileStatusHistory]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pCaseFileStatusHistory]
GO

-- [dbo].[pCaseFileStatusHistory] '1CA5234B-3ACA-4B34-7308-08DAF45F8C6C','A3068B85-4E59-484C-85FA-465E1EE1530C'
CREATE OR ALTER PROCEDURE [dbo].[pCaseFileStatusHistory]             
(                          
  @fileId uniqueidentifier = NULL,
  @HistoryId uniqueidentifier = NULL
)         
AS          
BEGIN     
	Select distinct ccf.*,      
	--Event      
	cfe.Name_En as EventNameEn,      
	cfe.Name_Ar as EventNameAr,      
	--Status      
	cfs.Name_En as StatusNameEn,      
	cfs.Name_Ar as StatusNameAr,      
	--Username      
	CONCAT(us.FirstName_En, ' ',us.SecondName_En, ' ',us.LastName_En) UserNameEn,      
	CONCAT(us.FirstName_Ar, ' ',us.SecondName_Ar, ' ',us.LastName_Ar) UserNameAr      
	FROM CMS_CASE_FILE_STATUS_HISTORY ccf      
	LEFT JOIN CMS_CASE_FILE_EVENT_G2G_LKP cfe ON ccf.EventId= cfe.Id      
	LEFT JOIN CMS_CASE_FILE_STATUS_G2G_LKP cfs ON ccf.StatusId = cfs.Id      
	LEFT JOIN UMS_USER us ON ccf.CreatedBy = us.Email    
	Where ccf.IsDeleted = 0          
	AND (ccf.FileId = @FileId OR @FileId IS NULL OR @FileId='00000000-0000-0000-0000-000000000000') 
	AND (ccf.HistoryId = @HistoryId OR @HistoryId IS NULL OR @HistoryId='00000000-0000-0000-0000-000000000000')      
END      
   
--- pCmsCaseFileAssignmentHistory
if exists (select * from sysobjects where id = object_id('[dbo].[pCmsCaseFileAssignmentHistory]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pCmsCaseFileAssignmentHistory]
GO

CREATE PROC [dbo].[pCmsCaseFileAssignmentHistory]  
(
@fileId uniqueidentifier
)
AS  
BEGIN
SELECT distinct cfl.* ,  
 ---Assignor 
 CONCAT(ar.FirstName_En, ' ',ar.SecondName_En, ' ',ar.LastName_En) AssignorNameEn,  
 CONCAT(ar.FirstName_Ar, ' ',ar.SecondName_Ar, ' ',ar.LastName_Ar) AssignorNameAr,  
 ---Assignee 
 CONCAT(ae.FirstName_En, ' ',ae.SecondName_En, ' ',ae.LastName_En) AssigneeNameEn,  
 CONCAT(ae.FirstName_Ar, ' ',ae.SecondName_Ar, ' ',ae.LastName_Ar) AssigneeNameAr  
  
from CMS_CASE_FILE_ASSIGNMENT_HISTORY cfl  
left join CMS_CASE_FILE ccf on cfl.FileId = ccf.FileId  
left join UMS_USER ar on cfl.CreatedBy = ar.Email  
left join UMS_USER ae on cfl.AssigneeId = ae.Id
WHERE cfl.FileId = @fileId
END


--- pCmsCaseFileAssigneeList
if exists (select * from sysobjects where id = object_id('[dbo].[pCmsCaseFileAssigneeList]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pCmsCaseFileAssigneeList]
GO
  
CREATE PROC [dbo].[pCmsCaseFileAssigneeList]    
(  
@fileId uniqueidentifier  
)  
AS    
BEGIN  
SELECT distinct cfa.* ,    
 ---Assignor   
 CONCAT(law.FirstName_En, ' ',law.SecondName_En, ' ',law.LastName_En) LawyerNameEn,    
 CONCAT(law.FirstName_Ar, ' ',law.SecondName_Ar, ' ',law.LastName_Ar) LawyerNameAr,    
 ---Assignee   
 CONCAT(sup.FirstName_En, ' ',sup.SecondName_En, ' ',sup.LastName_En) SupervisorNameEn,    
 CONCAT(sup.FirstName_Ar, ' ',sup.SecondName_Ar, ' ',sup.LastName_Ar) SupervisorNameAr    
    
from CMS_CASE_FILE_ASSIGNMENT cfa    
left join CMS_CASE_FILE ccf on cfa.FileId=ccf.FileId    
left join UMS_USER law on cfa.LawyerId = law.Id    
left join UMS_USER sup on cfa.SupervisorId = sup.Id  
WHERE ccf.FileId = @fileId  
END


--- pCaseTemplateParametersList
if exists (select * from sysobjects where id = object_id('[dbo].[pCaseTemplateParametersList]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pCaseTemplateParametersList]
GO

--- pCaseTemplateSectionParametersList
if exists (select * from sysobjects where id = object_id('[dbo].[pCaseTemplateSectionParametersList]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pCaseTemplateSectionParametersList]
GO

CREATE PROC pCaseTemplateSectionParametersList  
(  
@templateSectionId uniqueidentifier  
)  
AS   
BEGIN  
SELECT CTS.TemplateId,  
CTS.SectionId,  
P.ParameterId,  
P.Name,  
P.PKey,
P.IsAutoPopulated
FROM CMS_TEMPLATE_SECTION_PARAMETER CTSP  
LEFT JOIN CMS_TEMPLATE_SECTION CTS ON CTS.Id = CTSP.TemplateSectionId  
LEFT JOIN CMS_TEMPLATE CT ON CT.Id = CTS.TemplateId  
LEFT JOIN PARAMETER P ON P.ParameterId = CTSP.ParameterId
WHERE CTSP.TemplateSectionId = @templateSectionId  
END


--- pCaseTemplateSectionsList
if exists (select * from sysobjects where id = object_id('[dbo].[pCaseTemplateSectionsList]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pCaseTemplateSectionsList]
GO

CREATE PROC pCaseTemplateSectionsList  
(  
@templateId INT  
)  
AS   
BEGIN  
SELECT CTS.Id,  
CTS.TemplateId,  
CTS.SectionId,  
CS.NameEn AS SectionNameEn,   
CS.NameAr AS SectionNameAr
FROM CMS_TEMPLATE_SECTION CTS 
LEFT JOIN CMS_SECTION CS ON CS.Id = CTS.SectionId  
WHERE CTS.TemplateId = @templateId  
END




/****** Object:  StoredProcedure [dbo].[pCommuncationListByCaseRequest]    Script Date: 11/18/2022 4:07:27 PM ******/
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pCommuncationListByCaseRequest]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pCommuncationListByCaseRequest]
    GO
/****** Object:  StoredProcedure [dbo].[pCommuncationListByCaseRequest]    Script Date: 11/21/2022 10:12:49 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- [dbo].[pCommuncationListByCaseRequest] '04fa66d1-80ad-4287-89ed-04c4f84be848'          
CREATE OR ALTER PROCEDURE [dbo].[pCommuncationListByCaseRequest]               
(              
 @RequestId nvarchar(150) = NULL            
)              
AS                  
BEGIN                 
   Select DISTINCT cl.CommunicationId    
  , cl.CommunicationTypeId    
  , ct.NameEn AS Activity_En              
  , ct.NameAr AS Activity_Ar              
  , cl.CreatedBy As CreatedBy              
  , cl.CreatedDate As [CreatedDate]              
  , cl.Title As Remarks               
  , cl.InboxNumber as GeReferenceNo      
  , cl.InboxDate as GeReferenceDate       
  , cl.OutboxNumber as FatwaReferenceNo            
  , cl.OutboxDate as FatwaReferenceDate             
  , crt.NameAr as CorrespondenceTypeAr      
  , crt.NameEn as CorrespondenceTypeEn             
   FROM COMM_COMMUNICATION cl             
   INNER JOIN COMM_COMMUNICATION_CORRESPONDENCE_TYPE crt on crt.CorrespondenceTypeId = cl.CorrespondenceTypeId      
   INNER JOIN COMM_COMMUNICATION_TYPE ct ON ct.CommunicationTypeId = cl.CommunicationTypeId                
   INNER JOIN COMM_COMMUNICATION_TARGET_LINK tl ON tl.communicationId = cl.CommunicationId          
   INNER JOIN LINK_TARGET lt ON lt.TargetLinkId = tl.TargetLinkId                  
   WHERE lt.ReferenceId = @RequestId       
   --AND cl.CommunicationTypeId = 2 -- Request for Meeting    
   Order by cl.CreatedDate desc                            
END 

GO

/****** Object:  StoredProcedure [dbo].[pCommuncationListByCaseFile]    Script Date: 11/21/2022 10:11:55 AM ******/
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pCommuncationListByCaseFile]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pCommuncationListByCaseFile]
    GO

/****** Object:  StoredProcedure [dbo].[pCommuncationListByCaseFile]    Script Date: 11/21/2022 10:11:55 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


     
-- [dbo].[pCommuncationListByCaseFile] '362171a6-3248-4fab-c902-08dac095919a'        
CREATE PROCEDURE [dbo].[pCommuncationListByCaseFile]           
(          
 @CaseFileId nvarchar(256) = NULL        
)          
AS              
BEGIN             
   Select DISTINCT   
      ct.NameEn AS Activity_En          
    , ct.NameAr AS Activity_Ar          
    , cl.CreatedBy As CreatedBy          
    , cl.CreatedDate As [CreatedDate]          
    , cl.Title As Remarks           
    , cl.InboxNumber as GeReferenceNo  
    , cl.InboxDate as GeReferenceDate   
    , cl.OutboxNumber as FatwaReferenceNo        
    , cl.OutboxDate as FatwaReferenceDate         
    , crt.NameAr as CorrespondenceTypeAr  
    , crt.NameEn as CorrespondenceTypeEn       
	, CAST( cf.FileNumber as int) as FileNo      
	, crc.CANNumber as CanNo      
	, crc.CaseNumber as CasNo       
   FROM COMM_COMMUNICATION cl    
   INNER JOIN COMM_COMMUNICATION_CORRESPONDENCE_TYPE crt on crt.CorrespondenceTypeId = cl.CorrespondenceTypeId  
   INNER JOIN COMM_COMMUNICATION_TYPE ct ON ct.CommunicationTypeId = cl.CommunicationTypeId            
   INNER JOIN COMM_COMMUNICATION_TARGET_LINK tl ON tl.CommunicationId = cl.CommunicationId        
   INNER JOIN LINK_TARGET lt ON lt.TargetLinkId = tl.TargetLinkId      
   INNER JOIN CMS_CASE_REQUEST cr on cr.RequestId = lt.ReferenceId    
   INNER JOIN CMS_CASE_FILE cf on cf.RequestId = cr.RequestId    
   INNER JOIN CMS_REGISTERED_CASE crc on crc.FileId = cf.FileId     
	WHERE cf.FileId = @CaseFileId   
	Order by cl.CreatedDate desc  
                         
END   
GO

/****** Object:  StoredProcedure [dbo].[pCommuncationListByCaseRequest]    Script Date: 11/18/2022 4:07:27 PM ******/
DROP PROCEDURE [dbo].[pProcessLogSelAdvanceSearch]     
GO 
    
 -- [dbo].[pCommuncationListByCaseRequest] '8cbb9443-e9ee-4ed9-b8ba-b25deea67c61'      
CREATE Procedure [dbo].[pProcessLogSelAdvanceSearch]                             
    @ProcessLogId uniqueidentifier = Null                         
   , @Task NVARCHAR(150)= Null    
   , @Process NVARCHAR(150)= Null    
   , @StartDate datetime = Null                      
   , @EndDate Datetime = Null                         
   , @ComputerName NVARCHAR(150) = Null                            
   , @UserName NVARCHAR(150)= Null        
AS                                 
	Select distinct *     
	from LOG_PROCESSLOG PL                     
	Where(PL.Task = @Task or @Task is NULL OR @Task = '')                            
	AND(PL.Process = @Process or @Process is NULL OR @Process = '' )               
	AND ( (CAST(PL.StartDate AS DATE) >= @StartDate ) Or @StartDate Is Null OR @StartDate = '')      
	AND ( (CAST(PL.StartDate AS DATE) <= @EndDate ) Or @EndDate Is Null OR @EndDate = '')      
	AND (PL.Computer = @ComputerName or @ComputerName is NULL OR @ComputerName = '')      
	AND (PL.UserName = @UserName or @UserName is NULL OR @UserName = '')      
	ORDER BY PL.StartDate desc                              
 SET ROWCOUNT 0                                
 SET NOCOUNT OFF                                
 RETURN   
GO

------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------
--<History Author = 'Hassan Abbas' Date='2022-11-21' Version="1.0" Branch="master"> ABOVE SCRIPT EXECUTED ON FATWA_DB_DEV</History>
------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------


/****** Object:  StoredProcedure [dbo].[pCaseDraftDocumentsList]    Script Date: 11/21/2022 10:11:55 AM ******/
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pCaseDraftDocumentsList]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pCaseDraftDocumentsList]
    GO

CREATE OR ALTER   PROC [dbo].[pCaseDraftDocumentsList]    
(  
@userName nvarchar(256)  ,
@DraftNumber nvarchar(1000)=null,
@DocumentType int =null,
@start_From datetime=null,    
@end_To datetime=null  
)  
AS    
BEGIN    
select cdt.*,    
at.Type_En AS TypeEn,    
at.Type_Ar AS TypeAr,    
sdds.NameEn AS StatusEn,    
sdds.NameAr AS StatusAr    
from CMS_DRAFTED_TEMPLATE cdt    
LEFT JOIN ATTACHMENT_TYPE at ON at.AttachmentTypeId = cdt.AttachmentTypeId    
LEFT JOIN CMS_DRAFT_DOCUMENT_STATUS sdds ON sdds.Id = cdt.StatusId    
WHERE cdt.IsDeleted != 1
AND (cdt.CreatedBy = @userName
OR cdt.ReviewerUserId =(SELECT Id from UMS_USER WHERE UserName = @userName)
OR cdt.ReviewerRoleId IN ( SELECT RoleId FROM UMS_USER_ROLES WHERE UserId = (SELECT Id from UMS_USER WHERE UserName = @userName)))  
AND (cdt.AttachmentTypeId = @DocumentType OR @DocumentType IS NULL OR @DocumentType = '')  
AND (cdt.DraftNumber=@DraftNumber OR @DraftNumber IS NULL OR @DraftNumber='')
AND (CAST(cdt.CreatedDate as date)>=CAST(@start_From as date) OR @start_From IS NULL OR @start_From='')    
AND (CAST(cdt.CreatedDate as date)<=CAST(@end_To as date) OR @end_To IS NULL OR @end_To='')
ORDER BY cdt.DraftNumber desc    
END   

  
/****** Object:  StoredProcedure [dbo].[pCaseDraftDocumentDetailById]    Script Date: 11/21/2022 10:11:55 AM ******/
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pCaseDraftDocumentDetailById]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pCaseDraftDocumentDetailById]
GO

CREATE OR ALTER PROC pCaseDraftDocumentDetailById
(
@draftId uniqueidentifier
)
AS
BEGIN
select cdt.*,
at.Type_En AS TypeEn,
at.Type_Ar AS TypeAr,
sdds.NameEn AS StatusEn,
sdds.NameAr AS StatusAr,
CT.NameEn AS TemplateNameEn,
CT.NameAr AS TemplateNameAr,
CT.Content AS Content
from CMS_DRAFTED_TEMPLATE cdt
LEFT JOIN ATTACHMENT_TYPE at ON at.AttachmentTypeId = cdt.AttachmentTypeId
LEFT JOIN CMS_DRAFT_DOCUMENT_STATUS sdds ON sdds.Id = cdt.StatusId
LEFT JOIN CMS_TEMPLATE CT ON CT.Id = cdt.TemplateId
WHERE cdt.Id = @draftId
END


/****** Object:  StoredProcedure [dbo].[pCaseDraftDocumentSectionsList]    Script Date: 11/21/2022 10:11:55 AM ******/
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pCaseDraftDocumentSectionsList]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pCaseDraftDocumentSectionsList]
    GO

CREATE PROC pCaseDraftDocumentSectionsList    
(    
@draftedTemplateId uniqueidentifier    
)    
AS     
BEGIN    
SELECT CDTS.Id,    
CDT.TemplateId,    
CDTS.SectionId,    
CS.NameEn AS SectionNameEn,     
CS.NameAr AS SectionNameAr  
FROM CMS_DRAFTED_TEMPLATE_SECTION CDTS   
LEFT JOIN CMS_SECTION CS ON CS.Id = CDTS.SectionId  
LEFT JOIN CMS_DRAFTED_TEMPLATE CDT ON CDT.Id = CDTS.DraftedTemplateId   
WHERE CDTS.DraftedTemplateId = @draftedTemplateId    
END  


/****** Object:  StoredProcedure [dbo].[pCaseDraftDocumentSectionParametersList]    Script Date: 11/21/2022 10:11:55 AM ******/
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pCaseDraftDocumentSectionParametersList]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pCaseDraftDocumentSectionParametersList]
    GO

CREATE PROC pCaseDraftDocumentSectionParametersList    
(    
@draftedTemplateSectionId uniqueidentifier    
)    
AS     
BEGIN    
SELECT CDT.TemplateId,    
CDTS.SectionId,    
P.ParameterId,    
P.Name,    
P.PKey,  
P.IsAutoPopulated,
CDTSP.Value
FROM CMS_DRAFTED_TEMPLATE_SECTION_PARAMETER CDTSP    
LEFT JOIN CMS_DRAFTED_TEMPLATE_SECTION CDTS ON CDTS.Id = CDTSP.DraftedTemplateSectionId  
LEFT JOIN CMS_DRAFTED_TEMPLATE CDT ON CDT.Id = CDTS.DraftedTemplateId    
LEFT JOIN PARAMETER P ON P.ParameterId = CDTSP.ParameterId  
WHERE CDTSP.DraftedTemplateSectionId = @draftedTemplateSectionId   
END  
  

  
/****** Object:  StoredProcedure [dbo].[pCaseTemplateSectionParametersList]    Script Date: 11/21/2022 10:11:55 AM ******/
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pCaseTemplateSectionParametersList]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pCaseTemplateSectionParametersList]
GO

CREATE PROC pCaseTemplateSectionParametersList    
(    
@templateSectionId uniqueidentifier    
)    
AS     
BEGIN    
SELECT CTS.TemplateId,    
CTS.SectionId,    
P.ParameterId,    
P.Name,    
P.PKey,  
P.IsAutoPopulated,
'' AS Value
FROM CMS_TEMPLATE_SECTION_PARAMETER CTSP    
LEFT JOIN CMS_TEMPLATE_SECTION CTS ON CTS.Id = CTSP.TemplateSectionId    
LEFT JOIN CMS_TEMPLATE CT ON CT.Id = CTS.TemplateId    
LEFT JOIN PARAMETER P ON P.ParameterId = CTSP.ParameterId  
WHERE CTSP.TemplateSectionId = @templateSectionId    
END  


/****** Object:  StoredProcedure [dbo].[pWorkflowInstancesDocumentList]    Script Date: 11/21/2022 10:11:55 AM ******/
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pWorkflowInstancesDocumentList]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].pWorkflowInstancesDocumentList
GO

CREATE  OR ALTER  PROC [dbo].[pWorkflowInstancesDocumentList]        
AS        
BEGIN        
SELECT WI.InstanceId        
     , WI.ReferenceId      
  , WI.WorkflowId        
  , WI.WorkflowActivityId        
  , WIS.Name AS Status        
  , W.Name AS WorkflowName        
  , MA.Name AS ActivityName        
  , CASE WHEN LD.Title IS NOT NULL THEN LD.Title
	WHEN LP.Title IS NOT NULL THEN LP.Title
	ELSE CDT.Name END AS Title         
FROM WORKFLOW_INSTANCE WI        
LEFT JOIN WORKFLOW_INSTANCE_STATUS WIS        
ON WIS.StatusId = WI.StatusId        
LEFT JOIN LDS_DOCUMENT LD        
ON LD.DocumentId = WI.ReferenceId        
LEFT JOIN WORKFLOW W        
ON W.WorkflowId = WI.WorkflowId        
LEFT JOIN WORKFLOW_ACTIVITY WA        
ON WA.WorkflowActivityId = WI.WorkflowActivityId        
LEFT JOIN MODULE_ACTIVITY MA        
ON WA.ActivityId = MA.ActivityId  
LEFT JOIN LPS_PRINCIPLE LP  
ON LP.PrincipleId = WI.ReferenceId  
LEFT JOIN CMS_DRAFTED_TEMPLATE CDT  
ON CDT.Id = WI.ReferenceId  
ORDER BY WI.InstanceId DESC      
END 


/****** Object:  StoredProcedure [dbo].[pCmsCaseFileList]    Script Date: 11/21/2022 10:11:55 AM ******/
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pCmsCaseFileList]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pCmsCaseFileList]
GO

  
CREATE  OR ALTER  PROCEDURE [dbo].[pCmsCaseFileList]  
(  
@fileNumber NVARCHAR(1000) =null,  
@statusId int = null,  
@createdFrom datetime = null,  
@createdTo datetime = null,  
@modifiedFrom datetime = null,  
@modifiedTo datetime = null  
)  
As  
  
Select distinct  ccf.*,  
--Case request  
ccr.RequestNumber,   
ccr.RequestDate,   
ccr.Subject,  
  
--Status      
ccfs.Name_En as StatusNameEn,      
ccfs.Name_Ar as StatusNameAr,      
  
-- Government Entity  
cge.Name_En as GovernmentEntityNameEn,  
cge.Name_Ar as GovernmentEntityNameAr,  
-- Priority  
cpg.Name_En as PriorityNameEn,  
cpg.Name_Ar as PriorityNameAr,  
  
-- Operating Sector  
ost.Name_En as OperatingSectorNameEn,  
ost.Name_Ar as OperatingSectorNameAr  
  
from CMS_CASE_FILE ccf  
left join CMS_CASE_FILE_STATUS_G2G_LKP ccfs on ccf.StatusId= ccfs.Id  
left join CMS_CASE_REQUEST ccr on ccf.RequestId = ccr.RequestId   
left join CMS_GOVERNMENT_ENTITY_G2G_LKP cge on cge.EntityId=ccr.GovtEntityId  
left join CMS_PRIORITY_G2G_LKP cpg on cpg.Id=ccr.PriorityId  
left join CMS_OPERATING_SECTOR_TYPE_G2G_LKP  ost on ost.Id=ccr.SectorTypeId  
WHERE ccr.IsDeleted != 1    
AND (ccf.FileNumber=@fileNumber OR @fileNumber IS NULL OR @fileNumber='')    
AND (ccf.StatusId=@statusId OR @statusId IS NULL OR @statusId='0')    
AND(CAST(ccf.CreatedDate as date)>=@createdFrom OR @createdFrom IS NULL OR @createdFrom='')    
AND(CAST(ccf.CreatedDate as date)<=@createdTo OR @createdTo IS NULL OR @createdTo='')    
AND(CAST(ccf.ModifiedDate as date)>=@modifiedFrom OR @modifiedFrom IS NULL OR @modifiedFrom='')    
AND(CAST(ccf.ModifiedDate as date)<=@modifiedTo OR @modifiedTo IS NULL OR @modifiedTo='')    
ORDER BY ccf.FileNumber DESC


/****** Object:  StoredProcedure [dbo].[pCaseDraftDocumentSectionParametersList]    Script Date: 11/21/2022 10:11:55 AM ******/
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pCaseDraftDocumentSectionParametersList]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pCaseDraftDocumentSectionParametersList]
GO

  
CREATE or ALTER PROC pCaseDraftDocumentSectionParametersList      
(      
@draftedTemplateSectionId uniqueidentifier      
)      
AS       
BEGIN      
SELECT CDTSP.Id,
CDT.TemplateId,      
CDTS.SectionId,      
P.ParameterId,      
P.Name,      
P.PKey,    
P.IsAutoPopulated,  
CDTSP.Value  
FROM CMS_DRAFTED_TEMPLATE_SECTION_PARAMETER CDTSP      
LEFT JOIN CMS_DRAFTED_TEMPLATE_SECTION CDTS ON CDTS.Id = CDTSP.DraftedTemplateSectionId    
LEFT JOIN CMS_DRAFTED_TEMPLATE CDT ON CDT.Id = CDTS.DraftedTemplateId      
LEFT JOIN PARAMETER P ON P.ParameterId = CDTSP.ParameterId    
WHERE CDTSP.DraftedTemplateSectionId = @draftedTemplateSectionId     
END    


/****** Object:  StoredProcedure [dbo].[pCaseTemplateSectionParametersList]    Script Date: 11/21/2022 10:11:55 AM ******/
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pCaseTemplateSectionParametersList]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pCaseTemplateSectionParametersList]
GO

    
CREATE OR ALTER PROC pCaseTemplateSectionParametersList      
(      
@templateSectionId uniqueidentifier      
)      
AS       
BEGIN      
SELECT NewId() AS Id,
CTS.TemplateId,      
CTS.SectionId,      
P.ParameterId,      
P.Name,      
P.PKey,    
P.IsAutoPopulated,  
'' AS Value  
FROM CMS_TEMPLATE_SECTION_PARAMETER CTSP      
LEFT JOIN CMS_TEMPLATE_SECTION CTS ON CTS.Id = CTSP.TemplateSectionId      
LEFT JOIN CMS_TEMPLATE CT ON CT.Id = CTS.TemplateId      
LEFT JOIN PARAMETER P ON P.ParameterId = CTSP.ParameterId    
WHERE CTSP.TemplateSectionId = @templateSectionId      
END 



------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------
--<History Author = 'Hassan Abbas' Date='2022-11-25' Version="1.0" Branch="master"> ABOVE SCRIPT EXECUTED ON FATWA_DB_DEV</History>
------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------
 

/****** Object:  StoredProcedure [dbo].[pCmsCaseFileAssignmentHistory]    Script Date: 11/21/2022 10:11:55 AM ******/
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pCmsCaseFileAssignmentHistory]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pCmsCaseFileAssignmentHistory]
GO

         
/****** Object:  StoredProcedure [dbo].[pCmsCaseAssigneeList]    Script Date: 11/21/2022 10:11:55 AM ******/
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pCmsCaseAssigneeList]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pCmsCaseAssigneeList]
GO

CREATE PROC [dbo].[pCmsCaseAssigneeList]      
(    
@referenceId uniqueidentifier    
)    
AS      
BEGIN    
SELECT distinct cfa.* ,      
 ---Assignor     
 CONCAT(law.FirstName_En, ' ',law.SecondName_En, ' ',law.LastName_En) LawyerNameEn,      
 CONCAT(law.FirstName_Ar, ' ',law.SecondName_Ar, ' ',law.LastName_Ar) LawyerNameAr,      
 ---Assignee     
 CONCAT(sup.FirstName_En, ' ',sup.SecondName_En, ' ',sup.LastName_En) SupervisorNameEn,      
 CONCAT(sup.FirstName_Ar, ' ',sup.SecondName_Ar, ' ',sup.LastName_Ar) SupervisorNameAr      
      
from CMS_CASE_ASSIGNMENT cfa      
left join CMS_CASE_FILE ccf on cfa.ReferenceId = ccf.FileId      
left join UMS_USER law on cfa.LawyerId = law.Id      
left join UMS_USER sup on cfa.SupervisorId = sup.Id    
WHERE cfa.ReferenceId = @referenceId    
END  
  
      
/****** Object:  StoredProcedure [dbo].[pCmsCaseAssignmentHistory]    Script Date: 11/21/2022 10:11:55 AM ******/
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pCmsCaseAssignmentHistory]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pCmsCaseAssignmentHistory]
GO

  
CREATE PROC [dbo].[pCmsCaseAssignmentHistory]    
(  
@referenceId uniqueidentifier  
)  
AS    
BEGIN  
SELECT distinct cfl.* ,    
 ---Assignor   
 CONCAT(ar.FirstName_En, ' ',ar.SecondName_En, ' ',ar.LastName_En) AssignorNameEn,    
 CONCAT(ar.FirstName_Ar, ' ',ar.SecondName_Ar, ' ',ar.LastName_Ar) AssignorNameAr,    
 ---Assignee   
 CONCAT(ae.FirstName_En, ' ',ae.SecondName_En, ' ',ae.LastName_En) AssigneeNameEn,    
 CONCAT(ae.FirstName_Ar, ' ',ae.SecondName_Ar, ' ',ae.LastName_Ar) AssigneeNameAr    
    
from CMS_CASE_ASSIGNMENT_HISTORY cfl    
left join CMS_CASE_FILE ccf on cfl.ReferenceId = ccf.FileId    
left join UMS_USER ar on cfl.CreatedBy = ar.Email    
left join UMS_USER ae on cfl.AssigneeId = ae.Id  
WHERE cfl.ReferenceId = @referenceId  
END
------------------------------------------------------------------------------------------------------------------------------------



/****** Object:  StoredProcedure [dbo].[pTaskList]    Script Date: 11/21/2022 10:11:55 AM ******/
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pTaskList]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pTaskList]
GO

-- pTaskList NULL, NULL, 'fatwaadmin@gmail.com', 'zain@gmail.com', 'edit', 'do'    
CREATE OR ALTER PROCEDURE [dbo].[pTaskList]                   
(                
  @FromDate datetime = NULL,                  
  @ToDate datetime = NULL,              
  @UserId nvarchar(150) = NULL,              
  @AssignedBy nvarchar(150) = NULL,              
  @TaskName nvarchar(150) = NULL,              
  @TaskDescription nvarchar(150) = NULL,             
  @TaskStatusId int = NULL             
)              
AS                            
BEGIN                             
 SELECT CAST(tk.TaskNumber as nvarchar(150)) as TaskNo                 
   , tk.TaskId              
   , tk.[Name]              
   , tk.[Description]              
   , tk.[Date]              
   , md.ModuleNameEn              
   , md.ModuleNameAr              
   , tk.TaskStatusId            
   , tks.NameEn as StatusNameEn              
   , tks.NameAr as StatusNameAr              
   , usr.FirstName_En + ' ' + usr.LastName_En as AssignedBy              
   , '' as ModifiedBy  
   , tk.[Url]  
 FROM TSK_TASK tk              
 INNER JOIN MODULE md on md.ModuleId = tk.ModuleId              
 INNER JOIN TSK_TASK_STATUS tks on tks.TaskStatusId = tk.TaskStatusId              
 INNER JOIN UMS_USER usr on usr.Id = tk.AssignedBy             
 WHERE tk.IsDeleted = 0               
 AND (tk.AssignedTo = @UserId OR @UserId IS NULL OR @UserId = '')                 
 AND(CAST(tk.[Date] as date) >= @FromDate OR @FromDate IS NULL OR @FromDate = '')                    
 AND(CAST(tk.[Date] as date) <= @ToDate OR @ToDate IS NULL OR @ToDate = '')                
 AND (tk.AssignedBy = @AssignedBy OR @AssignedBy IS NULL OR @AssignedBy = '')                
 AND (tk.[Name] like '%' + @TaskName + '%' OR @TaskName IS NULL OR @TaskName = '')                 
 AND (tk.[Description] like '%' + @TaskDescription + '%' OR @TaskDescription IS NULL OR @TaskDescription = '')                 
 AND (tk.TaskStatusId = @TaskStatusId OR @TaskStatusId IS NULL OR @TaskStatusId = '')                 
 ORDER BY tk.CreatedDate DESC                     
END 
 
/****** Object:  StoredProcedure [dbo].[pRegisterCaseFileList]    Script Date: 29/11/2022 1:33:28 pm ******/
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pRegisterCaseFileList]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pRegisterCaseFileList]
GO

CREATE OR ALTER   PROCEDURE [dbo].[pRegisterCaseFileList]  
(  
@fileNumber NVARCHAR(1000) =null,  
@statusId int = null,  
@createdFrom datetime = null,  
@createdTo datetime = null,  
@modifiedFrom datetime = null,  
@modifiedTo datetime = null  
)  
As  
  
Select distinct  ccf.*,  
--Case request  
ccr.RequestNumber,   
ccr.RequestDate,   
ccr.Subject,  
  
--Status      
ccfs.Name_En as StatusNameEn,      
ccfs.Name_Ar as StatusNameAr,      
  
-- Government Entity  
cge.Name_En as GovernmentEntityNameEn,  
cge.Name_Ar as GovernmentEntityNameAr,  
-- Priority  
cpg.Name_En as PriorityNameEn,  
cpg.Name_Ar as PriorityNameAr,  
  
-- Operating Sector  
ost.Name_En as OperatingSectorNameEn,  
ost.Name_Ar as OperatingSectorNameAr  
  
from CMS_CASE_FILE ccf  
left join CMS_CASE_FILE_STATUS_G2G_LKP ccfs on ccf.StatusId= ccfs.Id  
left join CMS_CASE_REQUEST ccr on ccf.RequestId = ccr.RequestId   
left join CMS_GOVERNMENT_ENTITY_G2G_LKP cge on cge.EntityId=ccr.GovtEntityId  
left join CMS_PRIORITY_G2G_LKP cpg on cpg.Id=ccr.PriorityId  
left join CMS_OPERATING_SECTOR_TYPE_G2G_LKP  ost on ost.Id=ccr.SectorTypeId  
WHERE ccr.IsDeleted != 1    
AND (ccf.FileNumber=@fileNumber OR @fileNumber IS NULL OR @fileNumber='')    
AND (ccf.StatusId=@statusId OR @statusId IS NULL OR @statusId='0')    
AND(CAST(ccf.CreatedDate as date)>=@createdFrom OR @createdFrom IS NULL OR @createdFrom='')    
AND(CAST(ccf.CreatedDate as date)<=@createdTo OR @createdTo IS NULL OR @createdTo='')    
AND(CAST(ccf.ModifiedDate as date)>=@modifiedFrom OR @modifiedFrom IS NULL OR @modifiedFrom='')    
AND(CAST(ccf.ModifiedDate as date)<=@modifiedTo OR @modifiedTo IS NULL OR @modifiedTo='')    
ORDER BY ccf.FileNumber DESC
GO



/****** Object:  StoredProcedure [dbo].[pCmsRegisteredCasesListByFileId]    Script Date: 29/11/2022 1:33:28 pm ******/
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pCmsRegisteredCasesListByFileId]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pCmsRegisteredCasesListByFileId]
GO



CREATE OR ALTER   PROCEDURE [dbo].[pCmsRegisteredCasesListByFileId]   
(  
@fileId uniqueidentifier  
)  
AS    
Begin    
Select distinct crc.*,    
--GovtEntity    
ge.Name_En as GovtEntityNameEn,    
ge.Name_Ar as GovtEntityNameAr,      
--Status    
crcs.Name_En as StatusNameEn,    
crcs.Name_Ar as StatusNameAr,    
--CourtType    
crtp.Name_En as CourtTypeNameEn,    
crtp.Name_Ar as CourtTypeNameAr, 
--Court    
crt.Name_En as CourtNameEn,    
crt.Name_Ar as CourtNameAr,    
crt.Number as CourtNumber,    
crt.District as CourtDistrict,   
crt.Location as CourtLocation,     
--Chamber    
chm.Name_En as ChamberNameEn,    
chm.Name_Ar as ChamberNameAr,     
chm.Number as ChamberNumber,      
chm.Address as ChamberAddress, 
--File
ccf.FileNumber AS FileNumber
    
FROM CMS_REGISTERED_CASE crc    
    
LEFT JOIN CMS_COURT_G2G_LKP crt on  crt.Id = crc.CourtId    
LEFT JOIN CMS_CHAMBER_G2G_LKP chm ON chm.Id = crc.ChamberId
LEFT JOIN CMS_COURT_TYPE_G2G_LKP crtp ON crtp.Id = crt.TypeId
LEFT JOIN CMS_GOVERNMENT_ENTITY_G2G_LKP ge ON ge.EntityId = crc.GovtEntityId
LEFT JOIN CMS_REGISTERED_CASE_STATUS_G2G_LKP crcs ON crcs.Id = crc.StatusId
LEFT JOIN CMS_CASE_FILE ccf ON ccf.FileId = crc.FileId
WHERE crc.IsDeleted != 1  
and (crc.FileId = @fileId OR @fileId IS NULL OR @fileId = '00000000-0000-0000-0000-000000000000')         
ORDER BY crc.CreatedDate desc
END    
   
GO
     




IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pTaskDashBoard]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pTaskDashBoard]
GO
                    
CREATE PROCEDURE [dbo].[pTaskDashBoard]                    
(                                
  @UserId nvarchar(150) = NULL          
)               
AS                
BEGIN                 
	SELECT StatusNameEn, rcount       
	 INTO #tmpTasksData       
	 FROM (      
         
	   SELECT tts.NameEn as StatusNameEn,     
	   CASE      
		WHEN tt.TaskStatusId = 1 THEN (Select Count(TaskStatusId) AS Result  From TSK_TASK where TaskStatusId = 1 AND IsDeleted = 0 AND AssignedTo = @UserId )          
		WHEN tt.TaskStatusId = 2 THEN (Select Count(TaskStatusId) AS Result  From TSK_TASK where TaskStatusId = 2 AND IsDeleted = 0 AND AssignedTo = @UserId )         
		WHEN tt.TaskStatusId = 4 THEN (Select Count(TaskStatusId) AS Result  From TSK_TASK where TaskStatusId = 4 AND IsDeleted = 0 AND AssignedTo = @UserId )             
		WHEN tt.TaskStatusId = 8 THEN (Select Count(TaskStatusId) AS Result  From TSK_TASK where TaskStatusId = 8 AND IsDeleted = 0 AND AssignedTo = @UserId )            
		WHEN tt.TaskStatusId = 16 THEN (Select Count(TaskStatusId) AS Result  From TSK_TASK where TaskStatusId = 16 AND IsDeleted = 0 AND AssignedTo = @UserId )          
	 END AS rcount        
	 FROM TSK_TASK tt            
	 LEFT JOIN TSK_TASK_STATUS tts ON tts.TaskStatusId = tt.TaskStatusId           
	 LEFT JOIN UMS_USER ums ON ums.Id = tt.AssignedTo        
	 LEFT JOIN TSK_TODO_LIST ttl ON ttl.TodoItemId = ums.Id        
	 Where tt.IsDeleted = 0               
	 GROUP BY tt.TaskStatusId, tt.AssignedTo, ttl.[Description], tts.NameEn, tts.NameAr      
	   ) AS A      
      
	SELECT * FROM #tmpTasksData TMP       
	PIVOT (AVG(rcount) FOR StatusNameEn IN (Pending, Approved, Rejected, InProgress, Done)) AS PivotTable;      
         
	DROP TABLE #tmpTasksData      
      
END 


--pWorkflowInstancesDocumentList
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pWorkflowInstancesDocumentList]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pWorkflowInstancesDocumentList]
GO

CREATE  OR ALTER   PROC [dbo].[pWorkflowInstancesDocumentList]          
AS          
BEGIN          
SELECT WI.InstanceId          
     , WI.ReferenceId        
  , WI.WorkflowId          
  , WI.WorkflowActivityId          
  , WIS.Name AS Status          
  , W.Name AS WorkflowName          
  , MA.Name AS ActivityName          
  , CASE WHEN LD.LegislationTitle IS NOT NULL THEN LD.LegislationTitle  
 WHEN LP.PrincipleTitle IS NOT NULL THEN LP.PrincipleTitle  
 ELSE CDT.Name END AS Title           
FROM WORKFLOW_INSTANCE WI          
LEFT JOIN WORKFLOW_INSTANCE_STATUS WIS          
ON WIS.StatusId = WI.StatusId          
LEFT JOIN LEGAL_LEGISLATION LD          
ON LD.LegislationId = WI.ReferenceId          
LEFT JOIN WORKFLOW W          
ON W.WorkflowId = WI.WorkflowId          
LEFT JOIN WORKFLOW_ACTIVITY WA          
ON WA.WorkflowActivityId = WI.WorkflowActivityId          
LEFT JOIN MODULE_ACTIVITY MA          
ON WA.ActivityId = MA.ActivityId    
LEFT JOIN LEGAL_PRINCIPLE LP    
ON LP.PrincipleId = WI.ReferenceId    
LEFT JOIN CMS_DRAFTED_TEMPLATE CDT    
ON CDT.Id = WI.ReferenceId    
ORDER BY WI.InstanceId DESC        
END 

--pMergeListForApproval
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pMergeListForApproval]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pMergeListForApproval]
GO

CREATE OR ALTER PROCEDURE pMergeListForApproval
AS
BEGIN
SELECT CMR.Reason,
CMR.Id,
CMR.PrimaryId,
CMR.StatusId,
CMR.IsMergeTypeCase,
CRC.CaseNumber AS PrimaryCaseNumber,
CRC.CANNumber AS PrimaryCANNumber,
CRC.CreatedDate
FROM CMS_MERGE_REQUEST CMR
LEFT JOIN CMS_REGISTERED_CASE CRC ON CRC.CaseId = CMR.PrimaryId
WHERE CMR.StatusId = '2'
END

  
--pMergeRequestDetailById
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pMergeRequestDetailById]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pMergeRequestDetailById]
GO

CREATE OR ALTER PROCEDURE pMergeRequestDetailById
(
@id UNIQUEIDENTIFIER
)
AS  
BEGIN  
SELECT CMR.Reason,  
CMR.Id,  
CMR.PrimaryId,  
CMR.StatusId,  
CMR.IsMergeTypeCase,  
CRC.CaseNumber AS PrimaryCaseNumber,  
CRC.CANNumber AS PrimaryCANNumber,  
CRC.CreatedDate  
FROM CMS_MERGE_REQUEST CMR  
LEFT JOIN CMS_REGISTERED_CASE CRC ON CRC.CaseId = CMR.PrimaryId  
WHERE CMR.Id = @id
END


--pMergedCasesByMergeRequestId
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pMergedCasesByMergeRequestId]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pMergedCasesByMergeRequestId]
GO

CREATE  OR ALTER PROCEDURE [dbo].[pMergedCasesByMergeRequestId]           
(          
@mergeRequestId uniqueidentifier null        
)          
AS            
Begin            
Select distinct crc.*,            
--GovtEntity            
ge.Name_En as GovtEntityNameEn,            
ge.Name_Ar as GovtEntityNameAr,              
--Status            
crcs.Name_En as StatusNameEn,            
crcs.Name_Ar as StatusNameAr,            
--CourtType            
crtp.Name_En as CourtTypeNameEn,            
crtp.Name_Ar as CourtTypeNameAr,         
--Court            
crt.Name_En as CourtNameEn,            
crt.Name_Ar as CourtNameAr,            
crt.Number as CourtNumber,            
crt.District as CourtDistrict,           
crt.Location as CourtLocation,             
--Chamber            
chm.Name_En as ChamberNameEn,            
chm.Name_Ar as ChamberNameAr,             
chm.Number as ChamberNumber,              
chm.Address as ChamberAddress,         
--File        
ccf.FileNumber AS FileNumber,      
CCF.FileName As FileName      
FROM CMS_REGISTERED_CASE crc            
LEFT JOIN CMS_MERGE_REQUEST_SECONDARIES cmrs on cmrs.SecondaryId = crc.CaseId 
LEFT JOIN CMS_MERGE_REQUEST cmr ON cmr.Id = cmrs.MergeRequestId
LEFT JOIN CMS_COURT_G2G_LKP crt on  crt.Id = crc.CourtId            
LEFT JOIN CMS_CHAMBER_G2G_LKP chm ON chm.Id = crc.ChamberId        
LEFT JOIN CMS_COURT_TYPE_G2G_LKP crtp ON crtp.Id = crt.TypeId        
LEFT JOIN CMS_GOVERNMENT_ENTITY_G2G_LKP ge ON ge.EntityId = crc.GovtEntityId        
LEFT JOIN CMS_REGISTERED_CASE_STATUS_G2G_LKP crcs ON crcs.Id = crc.StatusId        
LEFT JOIN CMS_CASE_FILE ccf ON ccf.FileId = crc.FileId        
WHERE crc.IsDeleted != 1 AND cmrs.MergeRequestId = @mergeRequestId      
    
ORDER BY crc.CreatedDate desc        
END 


--pHearingListByCase
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pHearingListByCase]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pHearingListByCase]
GO
  
    
CREATE PROCEDURE pHearingListByCase    
(    
@caseId uniqueidentifier    
)    
AS    
BEGIN    
SELECT CH.*,    
CHGL.NameEn AS StatusEn,    
CHGL.NameAr AS StatusAr    
FROM CMS_HEARING CH    
LEFT JOIN CMS_HEARING_STATUS_G2G_LKP CHGL ON CHGL.Id = CH.StatusId    
WHERE CH.CaseId = @caseId    
ORDER BY CH.CreatedDate DESC  
END


--pOutcomeHearingListByCase
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pOutcomeHearingListByCase]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pOutcomeHearingListByCase]
GO
  
    
CREATE PROCEDURE pOutcomeHearingListByCase    
(    
@caseId uniqueidentifier    
)    
AS    
BEGIN    
SELECT COH.*,  
---Lawyer
 CONCAT(UU.FirstName_En, ' ',UU.SecondName_En, ' ',UU.LastName_En) LawyerNameEn,
 CONCAT(UU.FirstName_Ar, ' ',UU.SecondName_Ar, ' ',UU.LastName_Ar) LawyerNameAr
FROM CMS_OUTCOME_HEARING COH    
LEFT JOIN UMS_USER UU ON UU.Id = COH.LawyerId    
LEFT JOIN CMS_HEARING CH ON CH.Id = COH.HearingId
WHERE CH.CaseId = @caseId    
ORDER BY COH.CreatedDate DESC  
END


--pCaseDraftDocumentsbyFileAndAttachmentTypeId
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pCaseDraftDocumentsbyFileAndAttachmentTypeId]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pCaseDraftDocumentsbyFileAndAttachmentTypeId]
GO

CREATE PROC pCaseDraftDocumentsbyFileAndAttachmentTypeId  
(  
@fileId UNIQUEIDENTIFIER,
@attachmentTypeId INT
)  
AS    
BEGIN    
select cdt.*,    
at.Type_En AS TypeEn,    
at.Type_Ar AS TypeAr,    
sdds.NameEn AS StatusEn,    
sdds.NameAr AS StatusAr    
from CMS_DRAFTED_TEMPLATE cdt    
LEFT JOIN ATTACHMENT_TYPE at ON at.AttachmentTypeId = cdt.AttachmentTypeId    
LEFT JOIN CMS_DRAFT_DOCUMENT_STATUS sdds ON sdds.Id = cdt.StatusId    
WHERE cdt.IsDeleted != 1 AND cdt.ReferenceId = @fileId AND cdt.AttachmentTypeId = @attachmentTypeId
ORDER BY cdt.DraftNumber desc    
END 


--pMojRegistrationRequests
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pMojRegistrationRequests]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pMojRegistrationRequests]
GO

CREATE PROC pMojRegistrationRequests 
AS    
BEGIN    
select cmrr.*,    
ccf.FileName AS FileName,    
ccf.FileNumber AS FileNumber    
from CMS_MOJ_REGISTRATION_REQUEST cmrr    
LEFT JOIN CMS_CASE_FILE ccf ON ccf.FileId = cmrr.FileId    
WHERE cmrr.IsDeleted != 1 AND cmrr.IsRegistered != 1
ORDER BY cmrr.CreatedDate desc    
END 


--pJudgementsByCaseId
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pJudgementsByCaseId]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pJudgementsByCaseId]
GO
  
    
CREATE PROCEDURE pJudgementsByCaseId    
(    
@caseId uniqueidentifier    
)    
AS    
BEGIN    
SELECT CJ.*,  
---Type
CJT.NameEn AS TypeEn,
CJT.NameAr AS TypeAr
FROM CMS_JUDGEMENT CJ    
LEFT JOIN CMS_JUDGEMENT_TYPE_G2G_LKP CJT ON CJT.Id = CJ.TypeId    
WHERE CJ.CaseId = @caseId    
ORDER BY CJ.CreatedDate DESC  
END

  
/****** Object:  StoredProcedure [dbo].[pRegisteredCaseStatusHistory]    Script Date: 29/11/2022 1:33:28 pm ******/
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pRegisteredCaseStatusHistory]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pRegisteredCaseStatusHistory]
GO

CREATE PROCEDURE [dbo].[pRegisteredCaseStatusHistory]           
(                        
  @caseId uniqueidentifier = NULL         
)       
AS        
BEGIN         
    
   Select distinct CRCSH.*,    
  --Event    
    CRCE.Name_En as EventEn,    
    CRCE.Name_Ar as EventAr,    
    --Status    
   CRCS.Name_En as StatusEn,    
   CRCS.Name_Ar as StatusAr,    
    --Username    
 CONCAT(us.FirstName_En, ' ',us.SecondName_En, ' ',us.LastName_En) UserNameEn,    
 CONCAT(us.FirstName_Ar, ' ',us.SecondName_Ar, ' ',us.LastName_Ar) UserNameAr    
    
   from CMS_REGISTERED_CASE_STATUS_HISTORY CRCSH   
    LEFT JOIN CMS_REGISTERED_CASE_EVENT_G2G_LKP CRCE ON CRCE.Id= CRCSH.EventId    
 LEFT JOIN CMS_REGISTERED_CASE_STATUS_G2G_LKP CRCS ON CRCS.Id = CRCSH.StatusId    
 LEFT JOIN UMS_USER us ON CRCSH.CreatedBy = us.Email  
 Where          
 CRCSH.IsDeleted = 0        
 AND (CRCSH.CaseId = @caseId OR @caseId IS NULL OR @caseId='00000000-0000-0000-0000-000000000000')          
  END     

    
 --pSubCasesByCaseId
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pSubCasesByCaseId]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pSubCasesByCaseId]
GO   
    
        
CREATE   PROCEDURE [dbo].[pSubCasesByCaseId]             
(            
@caseId uniqueidentifier null          
)            
AS              
Begin              
Select distinct crc.*,              
--GovtEntity              
ge.Name_En as GovtEntityNameEn,              
ge.Name_Ar as GovtEntityNameAr,                
--Status              
crcs.Name_En as StatusNameEn,              
crcs.Name_Ar as StatusNameAr,              
--CourtType              
crtp.Name_En as CourtTypeNameEn,              
crtp.Name_Ar as CourtTypeNameAr,           
--Court              
crt.Name_En as CourtNameEn,              
crt.Name_Ar as CourtNameAr,              
crt.Number as CourtNumber,              
crt.District as CourtDistrict,             
crt.Location as CourtLocation,               
--Chamber              
chm.Name_En as ChamberNameEn,              
chm.Name_Ar as ChamberNameAr,               
chm.Number as ChamberNumber,                
chm.Address as ChamberAddress,           
--File        s  
ccf.FileNumber AS FileNumber,        
CCF.FileName As FileName        
              
FROM CMS_REGISTERED_CASE crc              
LEFT JOIN CMS_REGISTERED_CASE_SUB_CASE crsc on  crsc.CaseId = crc.CaseId             
LEFT JOIN CMS_COURT_G2G_LKP crt on  crt.Id = crc.CourtId              
LEFT JOIN CMS_CHAMBER_G2G_LKP chm ON chm.Id = crc.ChamberId          
LEFT JOIN CMS_COURT_TYPE_G2G_LKP crtp ON crtp.Id = crt.TypeId          
LEFT JOIN CMS_GOVERNMENT_ENTITY_G2G_LKP ge ON ge.EntityId = crc.GovtEntityId          
LEFT JOIN CMS_REGISTERED_CASE_STATUS_G2G_LKP crcs ON crcs.Id = crc.StatusId          
LEFT JOIN CMS_CASE_FILE ccf ON ccf.FileId = crc.FileId          
WHERE crc.IsDeleted != 1            
and (crsc.CaseId  = @caseId OR @caseId IS NULL OR @caseId = '00000000-0000-0000-0000-000000000000')        
      
ORDER BY crc.CreatedDate desc          
END 


 --pDeleteLegislationList
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pDeleteLegislationList]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pDeleteLegislationList]
GO
CREATE    PROCEDURE [dbo].[pDeleteLegislationList]        
     
AS        
Begin        
Select distinct ll.LegislationId        
  , ll.Legislation_Type       
  , ll.Legislation_Number       
  , ll.Introduction          
  , ll.IssueDate          
  , ll.IssueDate_Hijri          
  , ll.LegislationTitle     
  , ll.Legislation_Status       
  , ll.Legislation_Flow_Status      
  , ll.StartDate          
  , ll.AddedBy        
  , ll.AddedDate As CreatedDate       
  , ll.ModifiedBy          
  , ll.ModifiedDate              
  , llt.Name_Ar As Legislation_Type_Ar          
  , llt.Name_En As Legislation_Type_En           
  , lls.Name_Ar As Legislation_Status_Ar       
  , lls.Name_En As Legislation_Status_En      
  , llfs.Name_Ar As Legislation_Flow_Status_Ar      
  , llfs.Name_En As Legislation_Flow_Status_En      
from LEGAL_LEGISLATION ll                
LEFT JOIN LEGAL_LEGISLATION_TYPE llt ON ll.Legislation_Type = llt.Id      
LEFT JOIN LEGAL_LEGISLATION_FLOW_STATUS llfs ON ll.Legislation_Flow_Status = llfs.Id       
LEFT JOIN LEGAL_LEGISLATION_STATUS lls ON ll.Legislation_Status = lls.Id        
WHERE ll.IsDeleted = 1  
  
ORDER BY ll.AddedDate DESC               
SET ROWCOUNT 0                
SET NOCOUNT OFF                
RETURN                 
END 

--pTaskDetail
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pTaskDetail]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].pTaskDetail
GO
CREATE OR ALTER PROCEDURE [dbo].[pTaskDetail]                   
(                  
 @TaskId nvarchar(150) = NULL           
)              
AS                            
BEGIN                             
 SELECT tk.TaskId,          
  tk.TaskNumber,          
  tk.[Date] as TaskDate, 
  tk.TypeId,
  tkt.NameEn As TypeEn,          
  tkt.NameAr As TypeAr,            
  tkst.NameEn As SubTypeEn,          
  tkst.NameAr As SubTypeAr,            
  pr.Name_En As PriorityEn,          
  pr.Name_Ar As PriorityAr,           
  tk.DueDate,          
  tk.[Name],          
  tk.[Description],          
  cf.FileNumber,          
  sec.Name_Ar as SectorAr,          
  sec.Name_En as SectorEn,           
  dep.Name_Ar as DepartmentAr,          
  dep.Name_En as DepartmentEn,   
  tk.ModuleId,
  md.ModuleNameAr as ModuleAr,          
  md.ModuleNameEn as ModuleEn,           
  rl.[Name] as [Role],    
  tk.AssignedBy,          
  tk.AssignedTo,          
  tk.[Url],          
  '' as ModifiedBy,          
  tk.TaskStatusId,  
  ts.NameEn as TaskStatusEn,   
  ts.NameAr as TaskStatusAr,  
  r.Reason,
  tk.ReferenceId,
  usr.FirstName_En+' '+usr.LastName_En As AssignedTo  
 FROM TSK_TASK tk            
 INNER JOIN TSK_TASK_TYPE tkt on tkt.TypeId = tk.TypeId          
 LEFT JOIN TSK_TASK_SUB_TYPE tkst on tkst.SubTypeId = tk.SubTypeId          
 LEFT JOIN CMS_PRIORITY_G2G_LKP pr on pr.Id = tk.PriorityId           
 INNER JOIN CMS_OPERATING_SECTOR_TYPE_G2G_LKP sec on sec.Id = tk.SectorId          
 INNER JOIN Department dep on dep.Id = tk.DepartmentId          
 INNER JOIN MODULE md on md.ModuleId = tk.ModuleId          
 LEFT JOIN CMS_CASE_FILE cf on cf.FileId = tk.ReferenceId          
 INNER JOIN UMS_ROLE rl on rl.Id = tk.RoleId      
 INNER JOIN TSK_TASK_STATUS ts on ts.TaskStatusId = tk.TaskStatusId  
 LEFT JOIN Rejection r on r.ReferenceId = tk.TaskId 
 INNER JOIN UMS_USER usr on usr.Id = tk.AssignedTo 
 WHERE tk.IsDeleted = 0             
 AND (tk.TaskId = @TaskId OR @TaskId IS NULL OR @TaskId = '')           
 ORDER BY tk.CreatedDate DESC                     
END

--pMergedCasesByCaseId
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pDeleteLegislationList]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pMergedCasesByCaseId]
GO

CREATE   PROCEDURE [dbo].[pMergedCasesByCaseId]                 
(                
@caseId uniqueidentifier null              
)                
AS                  
Begin    
Select distinct crc.*,                  
--GovtEntity                  
ge.Name_En as GovtEntityNameEn,                  
ge.Name_Ar as GovtEntityNameAr,                    
--Status                  
crcs.Name_En as StatusNameEn,                  
crcs.Name_Ar as StatusNameAr,                  
--CourtType                  
crtp.Name_En as CourtTypeNameEn,                  
crtp.Name_Ar as CourtTypeNameAr,               
--Court                  
crt.Name_En as CourtNameEn,                  
crt.Name_Ar as CourtNameAr,                  
crt.Number as CourtNumber,                  
crt.District as CourtDistrict,                 
crt.Location as CourtLocation,                   
--Chamber                  
chm.Name_En as ChamberNameEn,                  
chm.Name_Ar as ChamberNameAr,                   
chm.Number as ChamberNumber,                    
chm.Address as ChamberAddress,               
--File        s      
ccf.FileNumber AS FileNumber,            
CCF.FileName As FileName            
                  
FROM CMS_REGISTERED_CASE crc                  
LEFT JOIN CMS_REGISTERED_CASE_MERGED_CASE crcmc on  crcmc.PrimaryCaseId = crc.CaseId                 
LEFT JOIN CMS_COURT_G2G_LKP crt on  crt.Id = crc.CourtId                  
LEFT JOIN CMS_CHAMBER_G2G_LKP chm ON chm.Id = crc.ChamberId             
LEFT JOIN CMS_COURT_TYPE_G2G_LKP crtp ON crtp.Id = crt.TypeId              
LEFT JOIN CMS_GOVERNMENT_ENTITY_G2G_LKP ge ON ge.EntityId = crc.GovtEntityId              
LEFT JOIN CMS_REGISTERED_CASE_STATUS_G2G_LKP crcs ON crcs.Id = crc.StatusId              
LEFT JOIN CMS_CASE_FILE ccf ON ccf.FileId = crc.FileId              
WHERE crc.IsDeleted != 1               
and (crcmc.PrimaryCaseId = @caseId OR @caseId IS NULL OR @caseId = '00000000-0000-0000-0000-000000000000')            
          
ORDER BY crc.CreatedDate desc              
                   
END     



/****** Object:  StoredProcedure [dbo].[pRegisteredCaseStatusHistory]    Script Date: 29/11/2022 1:33:28 pm ******/
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pRegisteredCaseStatusHistory]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pRegisteredCaseStatusHistory]
GO

CREATE PROCEDURE [dbo].[pRegisteredCaseStatusHistory]           
(                        
  @caseId uniqueidentifier = NULL         
)       
AS        
BEGIN         
    
   Select distinct CRCSH.*,    
  --Event    
    CRCE.Name_En as EventEn,    
    CRCE.Name_Ar as EventAr,    
    --Status    
   CRCS.Name_En as StatusEn,    
   CRCS.Name_Ar as StatusAr,    
    --Username    
 CONCAT(us.FirstName_En, ' ',us.SecondName_En, ' ',us.LastName_En) UserNameEn,    
 CONCAT(us.FirstName_Ar, ' ',us.SecondName_Ar, ' ',us.LastName_Ar) UserNameAr    
    
   from CMS_REGISTERED_CASE_STATUS_HISTORY CRCSH   
    LEFT JOIN CMS_REGISTERED_CASE_EVENT_G2G_LKP CRCE ON CRCE.Id= CRCSH.EventId    
 LEFT JOIN CMS_REGISTERED_CASE_STATUS_G2G_LKP CRCS ON CRCS.Id = CRCSH.StatusId    
 LEFT JOIN UMS_USER us ON CRCSH.CreatedBy = us.Email  
 Where          
 CRCSH.IsDeleted = 0        
 AND (CRCSH.CaseId = @caseId OR @caseId IS NULL OR @caseId='00000000-0000-0000-0000-000000000000')          
  END     

  /****** Object:  StoredProcedure [dbo].[pMergedCasesByCaseId]    Script Date: 29/11/2022 1:33:28 pm ******/
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pMergedCasesByCaseId]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pMergedCasesByCaseId]
GO

CREATE PROCEDURE [dbo].[pMergedCasesByCaseId]                   
(                  
@caseId uniqueidentifier null                
)                  
AS                    
Begin      
Select distinct crc.*,                    
--GovtEntity                    
ge.Name_En as GovtEntityNameEn,                    
ge.Name_Ar as GovtEntityNameAr,                      
--Status                    
crcs.Name_En as StatusNameEn,                    
crcs.Name_Ar as StatusNameAr,                    
--CourtType                    
crtp.Name_En as CourtTypeNameEn,                    
crtp.Name_Ar as CourtTypeNameAr,                 
--Court                    
crt.Name_En as CourtNameEn,                    
crt.Name_Ar as CourtNameAr,                    
crt.Number as CourtNumber,                    
crt.District as CourtDistrict,                   
crt.Location as CourtLocation,                     
--Chamber                    
chm.Name_En as ChamberNameEn,                    
chm.Name_Ar as ChamberNameAr,                     
chm.Number as ChamberNumber,                      
chm.Address as ChamberAddress,                 
--File        s        
ccf.FileNumber AS FileNumber,              
CCF.FileName As FileName              
                    
FROM CMS_REGISTERED_CASE crc                    
LEFT JOIN CMS_REGISTERED_CASE_MERGED_CASE crcmc on  crcmc.MergedCaseId = crc.CaseId                   
LEFT JOIN CMS_COURT_G2G_LKP crt on  crt.Id = crc.CourtId                    
LEFT JOIN CMS_CHAMBER_G2G_LKP chm ON chm.Id = crc.ChamberId               
LEFT JOIN CMS_COURT_TYPE_G2G_LKP crtp ON crtp.Id = crt.TypeId                
LEFT JOIN CMS_GOVERNMENT_ENTITY_G2G_LKP ge ON ge.EntityId = crc.GovtEntityId                
LEFT JOIN CMS_REGISTERED_CASE_STATUS_G2G_LKP crcs ON crcs.Id = crc.StatusId                
LEFT JOIN CMS_CASE_FILE ccf ON ccf.FileId = crc.FileId                
WHERE crc.IsDeleted != 1                 
and (crcmc.PrimaryCaseId = @caseId OR @caseId IS NULL OR @caseId = '00000000-0000-0000-0000-000000000000')              
            
ORDER BY crc.CreatedDate desc                
                     
END 

   
--- pCmsCaseFileList
if exists (select * from sysobjects where id = object_id('[dbo].[pCmsCaseFileList]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pCmsCaseFileList]
GO

CREATE OR ALTER  PROCEDURE [dbo].[pCmsCaseFileList]    
(    
@fileNumber NVARCHAR(1000) =null,    
@statusId int = null,    
@createdFrom datetime = null,    
@createdTo datetime = null,    
@modifiedFrom datetime = null,    
@modifiedTo datetime = null    
)    
As    
    
Select distinct  ccf.*,    
--Case request    
ccr.RequestNumber,     
ccr.RequestDate,     
ccr.Subject,    
    
--Status        
ccfs.Name_En as StatusNameEn,        
ccfs.Name_Ar as StatusNameAr,        
    
-- Government Entity    
cge.Name_En as GovernmentEntityNameEn,    
cge.Name_Ar as GovernmentEntityNameAr,    
-- Priority    
cpg.Name_En as PriorityNameEn,    
cpg.Name_Ar as PriorityNameAr,    
    
-- Operating Sector    
ost.Name_En as OperatingSectorNameEn,    
ost.Name_Ar as OperatingSectorNameAr    
    
from CMS_CASE_FILE ccf    
left join CMS_CASE_FILE_STATUS_G2G_LKP ccfs on ccf.StatusId= ccfs.Id    
left join CMS_CASE_REQUEST ccr on ccf.RequestId = ccr.RequestId     
left join CMS_GOVERNMENT_ENTITY_G2G_LKP cge on cge.EntityId=ccr.GovtEntityId    
left join CMS_PRIORITY_G2G_LKP cpg on cpg.Id=ccr.PriorityId    
left join CMS_OPERATING_SECTOR_TYPE_G2G_LKP  ost on ost.Id=ccr.SectorTypeId    
WHERE ccr.IsDeleted != 1      
AND (ccf.FileNumber=@fileNumber OR @fileNumber IS NULL OR @fileNumber='')      
AND (ccf.StatusId=@statusId OR @statusId IS NULL OR @statusId='0')      
AND(CAST(ccf.CreatedDate as date)>=@createdFrom OR @createdFrom IS NULL OR @createdFrom='')      
AND(CAST(ccf.CreatedDate as date)<=@createdTo OR @createdTo IS NULL OR @createdTo='')      
AND(CAST(ccf.ModifiedDate as date)>=@modifiedFrom OR @modifiedFrom IS NULL OR @modifiedFrom='')      
AND(CAST(ccf.ModifiedDate as date)<=@modifiedTo OR @modifiedTo IS NULL OR @modifiedTo='')      
ORDER BY ccf.CreatedDate DESC

  

   
--- pCmsCaseFileList
if exists (select * from sysobjects where id = object_id('[dbo].[pCmsCaseFileList]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pCmsCaseFileList]
GO

CREATE OR ALTER  PROCEDURE [dbo].[pCmsCaseFileList]    
(    
@fileNumber NVARCHAR(1000) =null,    
@statusId int = null,    
@createdFrom datetime = null,    
@createdTo datetime = null,    
@modifiedFrom datetime = null,    
@modifiedTo datetime = null    
)    
As    
    
Select distinct  ccf.*,    
--Case request    
ccr.RequestNumber,     
ccr.RequestDate,     
ccr.Subject,    
    
--Status        
ccfs.Name_En as StatusNameEn,        
ccfs.Name_Ar as StatusNameAr,        
    
-- Government Entity    
cge.Name_En as GovernmentEntityNameEn,    
cge.Name_Ar as GovernmentEntityNameAr,    
-- Priority    
cpg.Name_En as PriorityNameEn,    
cpg.Name_Ar as PriorityNameAr,    
    
-- Operating Sector    
ost.Name_En as OperatingSectorNameEn,    
ost.Name_Ar as OperatingSectorNameAr    
    
from CMS_CASE_FILE ccf    
left join CMS_CASE_FILE_STATUS_G2G_LKP ccfs on ccf.StatusId= ccfs.Id    
left join CMS_CASE_REQUEST ccr on ccf.RequestId = ccr.RequestId     
left join CMS_GOVERNMENT_ENTITY_G2G_LKP cge on cge.EntityId=ccr.GovtEntityId    
left join CMS_PRIORITY_G2G_LKP cpg on cpg.Id=ccr.PriorityId    
left join CMS_OPERATING_SECTOR_TYPE_G2G_LKP  ost on ost.Id=ccr.SectorTypeId    
WHERE ccr.IsDeleted != 1      
AND (ccf.FileNumber=@fileNumber OR @fileNumber IS NULL OR @fileNumber='')      
AND (ccf.StatusId=@statusId OR @statusId IS NULL OR @statusId='0')      
AND(CAST(ccf.CreatedDate as date)>=@createdFrom OR @createdFrom IS NULL OR @createdFrom='')      
AND(CAST(ccf.CreatedDate as date)<=@createdTo OR @createdTo IS NULL OR @createdTo='')      
AND(CAST(ccf.ModifiedDate as date)>=@modifiedFrom OR @modifiedFrom IS NULL OR @modifiedFrom='')      
AND(CAST(ccf.ModifiedDate as date)<=@modifiedTo OR @modifiedTo IS NULL OR @modifiedTo='')      
ORDER BY ccf.CreatedDate DESC

  
--- pRegisterCaseFileList
if exists (select * from sysobjects where id = object_id('[dbo].[pRegisterCaseFileList]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pRegisterCaseFileList]
GO

CREATE OR ALTER PROCEDURE [dbo].[pRegisterCaseFileList]    
(    
@fileNumber NVARCHAR(1000) =null,    
@statusId int = null,    
@createdFrom datetime = null,    
@createdTo datetime = null,    
@modifiedFrom datetime = null,    
@modifiedTo datetime = null    
)    
As    
    
Select distinct  ccf.*,    
--Case request    
ccr.RequestNumber,     
ccr.RequestDate,     
ccr.Subject,    
    
--Status        
ccfs.Name_En as StatusNameEn,        
ccfs.Name_Ar as StatusNameAr,        
    
-- Government Entity    
cge.Name_En as GovernmentEntityNameEn,    
cge.Name_Ar as GovernmentEntityNameAr,    
-- Priority    
cpg.Name_En as PriorityNameEn,    
cpg.Name_Ar as PriorityNameAr,    
    
-- Operating Sector    
ost.Name_En as OperatingSectorNameEn,    
ost.Name_Ar as OperatingSectorNameAr    
    
from CMS_CASE_FILE ccf    
left join CMS_CASE_FILE_STATUS_G2G_LKP ccfs on ccf.StatusId= ccfs.Id    
left join CMS_CASE_REQUEST ccr on ccf.RequestId = ccr.RequestId     
left join CMS_GOVERNMENT_ENTITY_G2G_LKP cge on cge.EntityId=ccr.GovtEntityId    
left join CMS_PRIORITY_G2G_LKP cpg on cpg.Id=ccr.PriorityId    
left join CMS_OPERATING_SECTOR_TYPE_G2G_LKP  ost on ost.Id=ccr.SectorTypeId    
WHERE ccr.IsDeleted != 1      
AND (ccf.FileNumber=@fileNumber OR @fileNumber IS NULL OR @fileNumber='')      
AND (ccf.StatusId=@statusId OR @statusId IS NULL OR @statusId='0')      
AND(CAST(ccf.CreatedDate as date)>=@createdFrom OR @createdFrom IS NULL OR @createdFrom='')      
AND(CAST(ccf.CreatedDate as date)<=@createdTo OR @createdTo IS NULL OR @createdTo='')      
AND(CAST(ccf.ModifiedDate as date)>=@modifiedFrom OR @modifiedFrom IS NULL OR @modifiedFrom='')      
AND(CAST(ccf.ModifiedDate as date)<=@modifiedTo OR @modifiedTo IS NULL OR @modifiedTo='')      
ORDER BY ccf.CreatedDate DESC 



--pCmsRequestedDocuments
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pCmsRequestedDocuments]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pCmsRequestedDocuments]
GO

Create  PROCEDURE [dbo].[pCmsRequestedDocuments]                 
      
AS                  
Begin    
Select distinct crc.*,                    
--GovtEntity                    
ge.Name_En as GovtEntityNameEn,                    
ge.Name_Ar as GovtEntityNameAr,                      
--Status                    
crcs.Name_En as StatusNameEn,                    
crcs.Name_Ar as StatusNameAr,                    
--CourtType                    
crtp.Name_En as CourtTypeNameEn,                    
crtp.Name_Ar as CourtTypeNameAr,                 
--Court                    
crt.Name_En as CourtNameEn,                    
crt.Name_Ar as CourtNameAr,                    
crt.Number as CourtNumber,                    
crt.District as CourtDistrict,                   
crt.Location as CourtLocation,                     
--Chamber                    
chm.Name_En as ChamberNameEn,                    
chm.Name_Ar as ChamberNameAr,                    
chm.Number as ChamberNumber,                      
chm.Address as ChamberAddress,                 
--File        s        
ccf.FileNumber AS FileNumber,              
CCF.FileName As FileName,  
-- Document Type       
att.Type_Ar as DocumentTypeEn,  
att.Type_Ar as DocumentTypeAr,  
att.AttachmentTypeId,  
--Request Document   
crfd.HearingDate  
FROM CMS_REGISTERED_CASE crc                    
INNER JOIN CMS_REQUEST_FOR_DOCUMENT crfd on  crfd.CaseId = crc.CaseId            
LEFT JOIN ATTACHMENT_TYPE att on att.AttachmentTypeId=crfd.AttachmentTypeId  
LEFT JOIN CMS_COURT_G2G_LKP crt on  crt.Id = crc.CourtId                    
LEFT JOIN CMS_CHAMBER_G2G_LKP chm ON chm.Id = crc.ChamberId               
LEFT JOIN CMS_COURT_TYPE_G2G_LKP crtp ON crtp.Id = crt.TypeId                
LEFT JOIN CMS_GOVERNMENT_ENTITY_G2G_LKP ge ON ge.EntityId = crc.GovtEntityId                
LEFT JOIN CMS_REGISTERED_CASE_STATUS_G2G_LKP crcs ON crcs.Id = crc.StatusId                
LEFT JOIN CMS_CASE_FILE ccf ON ccf.FileId = crc.FileId                
WHERE crc.IsDeleted != 1                
ORDER BY crc.CreatedDate desc   
          
                   
END    



------------------------------------------------------------------------------------------------------------------------------
-------------------------------------------------------- Consultation Start --------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------



------------------------------------------------------------------------------------------------------------------------------
-------------------------------------------------------- Consultation End ----------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------

  
--pLinkedRequestsByPrimaryRequestId
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pLinkedRequestsByPrimaryRequestId]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pLinkedRequestsByPrimaryRequestId]
GO  

CREATE   PROCEDURE [dbo].[pLinkedRequestsByPrimaryRequestId]      
(        
@requestId UNIQUEIDENTIFIER     
)        
AS          
Begin          
Select distinct ccr.*,          
          
--Department          
cd.Name_En as Department_Name_En,          
cd.Name_Ar as Department_Name_Ar,          
--GovtEntity          
cg.Name_En as GovermentEntity_Name_En,          
cg.Name_Ar as GovermentEntity_Name_Ar,          
--SectorType          
co.Name_En as SectorType_Name_En,          
co.Name_Ar as SectorType_Name_Ar,          
--Subtype          
cs.Name_En as Subtype_Name_En,          
cs.Name_Ar as Subtype_Name_Ar,          
--Priorty          
cp.Name_En as Priority_Name_En,          
cp.Name_Ar as Priority_Name_Ar,          
--Status          
ccrs.Name_En as Status_Name_En,          
ccrs.Name_Ar as Status_Name_Ar,     
--Court type            
cctgl.Name_En as Court_Type_Name_En,        
cctgl.Name_Ar as Court_Type_Name_Ar            
          
FROM CMS_CASE_REQUEST_LINKED_REQUEST ccrlr          
          
INNER JOIN CMS_CASE_REQUEST ccr on ccrlr.LinkedRequestId = ccr.RequestId  
left join Department cd on  ccr.DepartmentId = cd.Id          
left join CMS_GOVERNMENT_ENTITY_G2G_LKP cg on  ccr.GovtEntityId = cg.EntityId          
left join CMS_OPERATING_SECTOR_TYPE_G2G_LKP co on ccr.SectorTypeId= co.Id          
left join CMS_SUBTYPE_G2G_LKP cs on ccr.SubTypeId = cs.Id          
left join CMS_PRIORITY_G2G_LKP cp on ccr.PriorityId = cp.Id          
left join CMS_CASE_REQUEST_STATUS_G2G_LKP ccrs on ccr.StatusId = ccrs.Id     
left join CMS_COURT_TYPE_G2G_LKP cctgl on ccr.CourtTypeId = cctgl.Id                  
WHERE ccr.IsDeleted != 1 AND ccr.IsLinked = 1 AND ccrlr.PrimaryRequestId = @requestId   
         
ORDER BY ccr.CreatedDate desc      
END         
  

--pRegisterdRequestDetailbyRequestId
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pRegisterdRequestDetailbyRequestId]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pRegisterdRequestDetailbyRequestId]
GO  

--pRegisteredRequestSelAll
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pRegisteredRequestSelAll]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pRegisteredRequestSelAll]
GO  

--pRegisterdRequestResponseDetailbyRequestId
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pRegisterdRequestResponseDetailbyRequestId]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pRegisterdRequestResponseDetailbyRequestId]
GO  



--pSubCasesByCaseId
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pSubCasesByCaseId]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pSubCasesByCaseId]
GO  

        
CREATE OR ALTER  PROCEDURE [dbo].[pSubCasesByCaseId]             
(            
@caseId uniqueidentifier null          
)            
AS              
Begin              
Select distinct crc.*,              
--GovtEntity              
ge.Name_En as GovtEntityNameEn,              
ge.Name_Ar as GovtEntityNameAr,                
--Status              
crcs.Name_En as StatusNameEn,              
crcs.Name_Ar as StatusNameAr,              
--CourtType              
crtp.Name_En as CourtTypeNameEn,              
crtp.Name_Ar as CourtTypeNameAr,           
--Court              
crt.Name_En as CourtNameEn,              
crt.Name_Ar as CourtNameAr,              
crt.Number as CourtNumber,              
crt.District as CourtDistrict,             
crt.Location as CourtLocation,               
--Chamber              
chm.Name_En as ChamberNameEn,              
chm.Name_Ar as ChamberNameAr,               
chm.Number as ChamberNumber,                
chm.Address as ChamberAddress,           
--File        s  
ccf.FileNumber AS FileNumber,        
CCF.FileName As FileName        
              
FROM CMS_REGISTERED_CASE crc              
LEFT JOIN CMS_REGISTERED_CASE_SUB_CASE crsc on  crsc.SubCaseId = crc.CaseId             
LEFT JOIN CMS_COURT_G2G_LKP crt on  crt.Id = crc.CourtId              
LEFT JOIN CMS_CHAMBER_G2G_LKP chm ON chm.Id = crc.ChamberId          
LEFT JOIN CMS_COURT_TYPE_G2G_LKP crtp ON crtp.Id = crt.TypeId          
LEFT JOIN CMS_GOVERNMENT_ENTITY_G2G_LKP ge ON ge.EntityId = crc.GovtEntityId          
LEFT JOIN CMS_REGISTERED_CASE_STATUS_G2G_LKP crcs ON crcs.Id = crc.StatusId          
LEFT JOIN CMS_CASE_FILE ccf ON ccf.FileId = crc.FileId          
WHERE crc.IsDeleted != 1            
and (crsc.CaseId  = @caseId OR @caseId IS NULL OR @caseId = '00000000-0000-0000-0000-000000000000')        
      
ORDER BY crc.CreatedDate desc          
END              

    
    
    
--pCmsRegisteredCasesListByFileId
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pCmsRegisteredCasesListByFileId]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pCmsRegisteredCasesListByFileId]
GO  

CREATE PROCEDURE [dbo].[pCmsRegisteredCasesListByFileId]         
(        
@fileId uniqueidentifier =null     
)        
AS          
Begin          
Select distinct crc.*,          
--GovtEntity          
ge.Name_En as GovtEntityNameEn,          
ge.Name_Ar as GovtEntityNameAr,            
--Status          
crcs.Name_En as StatusNameEn,          
crcs.Name_Ar as StatusNameAr,          
--CourtType          
crtp.Name_En as CourtTypeNameEn,          
crtp.Name_Ar as CourtTypeNameAr,       
--Court          
crt.Name_En as CourtNameEn,          
crt.Name_Ar as CourtNameAr,          
crt.Number as CourtNumber,          
crt.District as CourtDistrict,         
crt.Location as CourtLocation,           
--Chamber          
chm.Name_En as ChamberNameEn,          
chm.Name_Ar as ChamberNameAr,           
chm.Number as ChamberNumber,            
chm.Address as ChamberAddress,       
--File      
ccf.FileNumber AS FileNumber      
          
FROM CMS_REGISTERED_CASE crc          
          
LEFT JOIN CMS_COURT_G2G_LKP crt on  crt.Id = crc.CourtId          
LEFT JOIN CMS_CHAMBER_G2G_LKP chm ON chm.Id = crc.ChamberId      
LEFT JOIN CMS_COURT_TYPE_G2G_LKP crtp ON crtp.Id = crt.TypeId      
LEFT JOIN CMS_GOVERNMENT_ENTITY_G2G_LKP ge ON ge.EntityId = crc.GovtEntityId      
LEFT JOIN CMS_REGISTERED_CASE_STATUS_G2G_LKP crcs ON crcs.Id = crc.StatusId      
LEFT JOIN CMS_CASE_FILE ccf ON ccf.FileId = crc.FileId      
WHERE crc.IsDeleted != 1 AND (crc.IsSubCase = 0 or crc.IsSubCase IS NULL)
and (crc.FileId = @fileId OR @fileId IS NULL OR @fileId = '00000000-0000-0000-0000-000000000000')               
ORDER BY crc.CreatedDate desc      
END   


/****** Object:  StoredProcedure [dbo].[pCmsCaseRequestList]    Script Date: 29/11/2022 1:33:28 pm ******/
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pCmsCaseRequestList]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pCmsCaseRequestList]
GO

CREATE PROCEDURE [dbo].[pCmsCaseRequestList]            
(              
@requestNumber int = null,              
@statusId int =null,              
@subject NVARCHAR(1000) =null,              
@sectorTypeId int=null,              
@requestFrom datetime=null,              
@requestTo datetime=null              
)              
AS                
Begin                
Select distinct ccr.*,                
                
--Department                
cd.Name_En as Department_Name_En,                
cd.Name_Ar as Department_Name_Ar,                
--GovtEntity                
cg.Name_En as GovermentEntity_Name_En,                
cg.Name_Ar as GovermentEntity_Name_Ar,                
--RequestType                
rt.Name_En as RequestType_Name_En,                
rt.Name_Ar as RequestType_Name_Ar,                 
--SectorType                
co.Name_En as SectorType_Name_En,                
co.Name_Ar as SectorType_Name_Ar,                
--Subtype                
cs.Name_En as Subtype_Name_En,                
cs.Name_Ar as Subtype_Name_Ar,                
--Priorty                
cp.Name_En as Priority_Name_En,                
cp.Name_Ar as Priority_Name_Ar,                
--Status                
ccrs.Name_En as Status_Name_En,                
ccrs.Name_Ar as Status_Name_Ar,           
--Court type            
cctgl.Name_En as Court_Type_Name_En,        
cctgl.Name_Ar as Court_Type_Name_Ar                
FROM CMS_CASE_REQUEST ccr                
                
left join Department cd on  ccr.DepartmentId = cd.Id                
left join CMS_GOVERNMENT_ENTITY_G2G_LKP cg on  ccr.GovtEntityId = cg.EntityId                
left join CMS_OPERATING_SECTOR_TYPE_G2G_LKP co on ccr.SectorTypeId= co.Id                 
left join CMS_REQUEST_TYPE_G2G_LKP rt on ccr.RequestTypeId= rt.Id                   
left join CMS_SUBTYPE_G2G_LKP cs on ccr.SubTypeId = cs.Id                
left join CMS_PRIORITY_G2G_LKP cp on ccr.PriorityId = cp.Id                
left join CMS_CASE_REQUEST_STATUS_G2G_LKP ccrs on ccr.StatusId = ccrs.Id         
left join CMS_COURT_TYPE_G2G_LKP cctgl on ccr.CourtTypeId = cctgl.Id         
WHERE ccr.IsDeleted != 1              
AND (ccr.RequestNumber=@requestNumber OR @requestNumber IS NULL OR @requestNumber='0')              
AND (ccr.StatusId=@statusId OR @statusId IS NULL OR @statusId='0')              
AND (ccr.Subject=@subject OR @subject IS NULL OR @subject='')              
AND(ccr.SectorTypeId=@sectorTypeId OR @sectorTypeId IS NULL OR @sectorTypeId='0')              
AND (CAST(ccr.RequestDate as date)>=@requestFrom OR @requestFrom IS NULL OR @requestFrom='')             
AND(CAST(ccr.RequestDate as date)<=@requestTo OR @requestTo IS NULL OR @requestTo='')              
        
ORDER BY ccr.RequestNumber desc            
END  

  
/****** Object:  StoredProcedure [dbo].[pLinkedRequestsByPrimaryRequestId]    Script Date: 29/11/2022 1:33:28 pm ******/
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pLinkedRequestsByPrimaryRequestId]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pLinkedRequestsByPrimaryRequestId]
GO

CREATE   PROCEDURE [dbo].[pLinkedRequestsByPrimaryRequestId]        
(          
@requestId UNIQUEIDENTIFIER       
)          
AS            
Begin            
Select distinct ccr.*,            
            
--Department            
cd.Name_En as Department_Name_En,            
cd.Name_Ar as Department_Name_Ar,            
--GovtEntity            
cg.Name_En as GovermentEntity_Name_En,            
cg.Name_Ar as GovermentEntity_Name_Ar,               
--RequestType                
rt.Name_En as RequestType_Name_En,                
rt.Name_Ar as RequestType_Name_Ar,              
--SectorType            
co.Name_En as SectorType_Name_En,            
co.Name_Ar as SectorType_Name_Ar,            
--Subtype            
cs.Name_En as Subtype_Name_En,            
cs.Name_Ar as Subtype_Name_Ar,            
--Priorty            
cp.Name_En as Priority_Name_En,            
cp.Name_Ar as Priority_Name_Ar,            
--Status            
ccrs.Name_En as Status_Name_En,            
ccrs.Name_Ar as Status_Name_Ar,       
--Court type              
cctgl.Name_En as Court_Type_Name_En,          
cctgl.Name_Ar as Court_Type_Name_Ar              
            
FROM CMS_CASE_REQUEST_LINKED_REQUEST ccrlr            
            
INNER JOIN CMS_CASE_REQUEST ccr on ccrlr.LinkedRequestId = ccr.RequestId    
left join Department cd on  ccr.DepartmentId = cd.Id            
left join CMS_GOVERNMENT_ENTITY_G2G_LKP cg on  ccr.GovtEntityId = cg.EntityId            
left join CMS_OPERATING_SECTOR_TYPE_G2G_LKP co on ccr.SectorTypeId= co.Id                    
left join CMS_REQUEST_TYPE_G2G_LKP rt on ccr.RequestTypeId= rt.Id               
left join CMS_SUBTYPE_G2G_LKP cs on ccr.SubTypeId = cs.Id            
left join CMS_PRIORITY_G2G_LKP cp on ccr.PriorityId = cp.Id            
left join CMS_CASE_REQUEST_STATUS_G2G_LKP ccrs on ccr.StatusId = ccrs.Id       
left join CMS_COURT_TYPE_G2G_LKP cctgl on ccr.CourtTypeId = cctgl.Id                    
WHERE ccr.IsDeleted != 1 AND ccr.IsLinked = 1 AND ccrlr.PrimaryRequestId = @requestId     
           
ORDER BY ccr.CreatedDate desc        
END           
    

    
/****** Object:  StoredProcedure [dbo].[pCaseRequestViewDetail]    Script Date: 29/11/2022 1:33:28 pm ******/
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pCaseRequestViewDetail]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pCaseRequestViewDetail]
GO

CREATE PROCEDURE [dbo].[pCaseRequestViewDetail]                
(                            
  @RequestId uniqueidentifier = NULL             
)           
AS            
BEGIN             
        
   Select cc.RequestId        
   , cc.RequestNumber        
   , cc.RequestDate        
   , cc.StatusId        
   , cs.Name_En as StatusName_En        
   , cs.Name_Ar as StatusName_Ar        
   , cc.GovtEntityId        
   , ge.Name_En as EntityName_En        
   , ge.Name_Ar as EntityName_Ar        
   , cc.DepartmentId        
   , dp.Name_En as DepartmentName_En        
   , dp.Name_Ar as DepartmentName_Ar         
   , cc.RequestTypeId                       
   , rt.Name_En as RequestType_Name_En
   , rt.Name_Ar as RequestType_Name_Ar         
   , cc.SectorTypeId        
   , os.Name_En as SectorName_En        
   , os.Name_Ar as SectorName_Ar        
   , cc.PriorityId        
   , pc.Name_En as PriorityName_En        
   , pc.Name_Ar as PriorityName_Ar        
   , cc.SubTypeId        
   , st.Name_En as SubType_En        
   , st.Name_Ar as SubType_Ar        
   , cc.ClaimAmount        
   , cc.Subject        
   , cc.IsConfidential        
   , cc.Remarks        
   , cc.CaseRequirements        
   , cc.ReferenceNo        
   , cc.ReferenceDate        
   , cc.CreatedBy        
   , cc.CreatedDate        
   , cc.ModifiedBy        
   , cc.ModifiedDate        
   , cc.DeletedBy        
   , cc.DeletedDate        
   , cc.IsDeleted        
   , cc.ReviewedDate      
   , cc.ReceivedDate      
   , cc.ApprovedDate     
   ,CourtTypeId As CourtTypeId  
   ,cctgl.Name_En As Court_Type_Name_En    
   ,cctgl.Name_Ar As Court_Type_Name_Ar    
   , ISNULL(uurc.FirstName_En,'') + ' ' + ISNULL(uurc.SecondName_En,'') + ' ' + ISNULL(uurc.LastName_En,'') AS ReceiverNameEn      
   , ISNULL(uurc.FirstName_Ar,'') + ' ' + ISNULL(uurc.SecondName_Ar,'') + ' ' + ISNULL(uurc.LastName_Ar,'') AS ReceiverNameAr      
   , ISNULL(uurv.FirstName_En,'') + ' ' + ISNULL(uurc.SecondName_En,'') + ' ' + ISNULL(uurv.LastName_En,'') AS ReviewerNameEn      
   , ISNULL(uurv.FirstName_Ar,'') + ' ' + ISNULL(uurv.SecondName_Ar,'') + ' ' + ISNULL(uurv.LastName_Ar,'') AS ReviewerNameAr      
   , ISNULL(uuap.FirstName_En,'') + ' ' + ISNULL(uuap.SecondName_En,'') + ' ' + ISNULL(uuap.LastName_En,'') AS ApproverNameEn      
   , ISNULL(uuap.FirstName_Ar,'') + ' ' + ISNULL(uuap.SecondName_Ar,'') + ' ' + ISNULL(uuap.LastName_Ar,'') AS ApproverNameAr      
   , ISNULL(geus.FirstName_En,'') + ' ' + ISNULL(geus.SecondName_En,'') + ' ' + ISNULL(geus.LastName_En,'') AS GEUserNameEn      
   , ISNULL(geus.FirstName_Ar,'') + ' ' + ISNULL(geus.SecondName_Ar,'') + ' ' + ISNULL(geus.LastName_Ar,'') AS GEUserNameAr      
   from CMS_CASE_REQUEST cc        
   LEFT JOIN CMS_CASE_REQUEST_STATUS_G2G_LKP cs ON cs.Id = cc.StatusId        
   LEFT JOIN CMS_GOVERNMENT_ENTITY_G2G_LKP ge ON ge.EntityId = cc.GovtEntityId        
   LEFT JOIN Department dp ON dp.Id = cc.DepartmentId        
   LEFT JOIN CMS_OPERATING_SECTOR_TYPE_G2G_LKP os ON os.Id = cc.SectorTypeId               
   LEFT JOIN CMS_REQUEST_TYPE_G2G_LKP rt on rt.Id = cc.RequestTypeId
   LEFT JOIN CMS_PRIORITY_G2G_LKP pc ON pc.Id = cc.PriorityId        
   LEFT JOIN CMS_SUBTYPE_G2G_LKP st ON st.Id = cc.SubTypeId      
   LEFT JOIN UMS_USER uurc ON uurc.UserName = cc.ReceivedBy      
   LEFT JOIN UMS_USER uurv ON uurv.UserName = cc.ReviewedBy      
   LEFT JOIN UMS_USER uuap ON uuap.UserName = cc.ApprovedBy      
   LEFT JOIN UMS_USER geus ON geus.Email = cc.CreatedBy      
   LEFT JOIN CMS_COURT_TYPE_G2G_LKP cctgl ON cc.CourtTypeId = cctgl.Id    
 Where              
 cc.IsDeleted = 0            
 AND (cc.RequestId = @RequestId OR @RequestId IS NULL OR @RequestId='00000000-0000-0000-0000-000000000000')              
  END   

      
/****** Object:  StoredProcedure [dbo].[pCaseRequestViewDetail]    Script Date: 29/11/2022 1:33:28 pm ******/
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pCaseRequestViewDetail]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pCaseRequestViewDetail]
GO

CREATE PROCEDURE [dbo].[pCaseRequestViewDetail]                    
(                                
  @RequestId uniqueidentifier = NULL                 
)               
AS                
BEGIN                 
            
   Select cc.RequestId            
   , cc.RequestNumber            
   , cc.RequestDate            
   , cc.StatusId            
   , cs.Name_En as StatusName_En            
   , cs.Name_Ar as StatusName_Ar            
   , cc.GovtEntityId            
   , ge.Name_En as EntityName_En            
   , ge.Name_Ar as EntityName_Ar            
   , cc.DepartmentId            
   , dp.Name_En as DepartmentName_En            
   , dp.Name_Ar as DepartmentName_Ar             
   , cc.RequestTypeId                           
   , rt.Name_En as RequestType_Name_En    
   , rt.Name_Ar as RequestType_Name_Ar             
   , cc.SectorTypeId            
   , os.Name_En as SectorName_En            
   , os.Name_Ar as SectorName_Ar            
   , cc.PriorityId            
   , pc.Name_En as PriorityName_En            
   , pc.Name_Ar as PriorityName_Ar            
   , cc.SubTypeId            
   , st.Name_En as SubType_En            
   , st.Name_Ar as SubType_Ar            
   , cc.ClaimAmount            
   , cc.Subject            
   , cc.IsConfidential            
   , cc.Remarks            
   , cc.CaseRequirements            
   , cc.ReferenceNo            
   , cc.ReferenceDate            
   , cc.CreatedBy            
   , cc.CreatedDate            
   , cc.ModifiedBy            
   , cc.ModifiedDate            
   , cc.DeletedBy            
   , cc.DeletedDate            
   , cc.IsDeleted            
   , cc.ReviewedDate          
   , cc.ReceivedDate          
   , cc.ApprovedDate         
   ,CourtTypeId As CourtTypeId      
   ,cctgl.Name_En As Court_Type_Name_En        
   ,cctgl.Name_Ar As Court_Type_Name_Ar        
   , ISNULL(uurc.FirstName_En,'') + ' ' + ISNULL(uurc.SecondName_En,'') + ' ' + ISNULL(uurc.LastName_En,'') AS ReceiverNameEn          
   , ISNULL(uurc.FirstName_Ar,'') + ' ' + ISNULL(uurc.SecondName_Ar,'') + ' ' + ISNULL(uurc.LastName_Ar,'') AS ReceiverNameAr          
   , ISNULL(uurv.FirstName_En,'') + ' ' + ISNULL(uurc.SecondName_En,'') + ' ' + ISNULL(uurv.LastName_En,'') AS ReviewerNameEn          
   , ISNULL(uurv.FirstName_Ar,'') + ' ' + ISNULL(uurv.SecondName_Ar,'') + ' ' + ISNULL(uurv.LastName_Ar,'') AS ReviewerNameAr          
   , ISNULL(uuap.FirstName_En,'') + ' ' + ISNULL(uuap.SecondName_En,'') + ' ' + ISNULL(uuap.LastName_En,'') AS ApproverNameEn          
   , ISNULL(uuap.FirstName_Ar,'') + ' ' + ISNULL(uuap.SecondName_Ar,'') + ' ' + ISNULL(uuap.LastName_Ar,'') AS ApproverNameAr          
   , ISNULL(geus.FirstName_En,'') + ' ' + ISNULL(geus.SecondName_En,'') + ' ' + ISNULL(geus.LastName_En,'') AS GEUserNameEn          
   , ISNULL(geus.FirstName_Ar,'') + ' ' + ISNULL(geus.SecondName_Ar,'') + ' ' + ISNULL(geus.LastName_Ar,'') AS GEUserNameAr   
   , cc.TransferStatusId
   from CMS_CASE_REQUEST cc            
   LEFT JOIN CMS_CASE_REQUEST_STATUS_G2G_LKP cs ON cs.Id = cc.StatusId            
   LEFT JOIN CMS_GOVERNMENT_ENTITY_G2G_LKP ge ON ge.EntityId = cc.GovtEntityId            
   LEFT JOIN Department dp ON dp.Id = cc.DepartmentId            
   LEFT JOIN CMS_OPERATING_SECTOR_TYPE_G2G_LKP os ON os.Id = cc.SectorTypeId                   
   LEFT JOIN CMS_REQUEST_TYPE_G2G_LKP rt on rt.Id = cc.RequestTypeId    
   LEFT JOIN CMS_PRIORITY_G2G_LKP pc ON pc.Id = cc.PriorityId            
   LEFT JOIN CMS_SUBTYPE_G2G_LKP st ON st.Id = cc.SubTypeId          
   LEFT JOIN UMS_USER uurc ON uurc.UserName = cc.ReceivedBy          
   LEFT JOIN UMS_USER uurv ON uurv.UserName = cc.ReviewedBy          
   LEFT JOIN UMS_USER uuap ON uuap.UserName = cc.ApprovedBy          
   LEFT JOIN UMS_USER geus ON geus.Email = cc.CreatedBy          
   LEFT JOIN CMS_COURT_TYPE_G2G_LKP cctgl ON cc.CourtTypeId = cctgl.Id      
 Where                  
 cc.IsDeleted = 0                
 AND (cc.RequestId = @RequestId OR @RequestId IS NULL OR @RequestId='00000000-0000-0000-0000-000000000000')                  
  END   
  

  
/****** Object:  StoredProcedure [dbo].[pCmsCaseRequestList]    Script Date: 29/11/2022 1:33:28 pm ******/
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pCmsCaseRequestList]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pCmsCaseRequestList]
GO

  
CREATE OR ALTER PROCEDURE [dbo].[pCmsCaseRequestList]              
(                
@requestNumber int = null,                
@statusId int =null,                
@subject NVARCHAR(1000) =null,                
@sectorTypeId int=null,                
@requestFrom datetime=null,                
@requestTo datetime=null,
@showUndefinedRequests bit = null
)                
AS                  
Begin                  
Select distinct ccr.*,                  
                  
--Department                  
cd.Name_En as Department_Name_En,                  
cd.Name_Ar as Department_Name_Ar,                  
--GovtEntity                  
cg.Name_En as GovermentEntity_Name_En,                  
cg.Name_Ar as GovermentEntity_Name_Ar,                  
--RequestType                  
rt.Name_En as RequestType_Name_En,                  
rt.Name_Ar as RequestType_Name_Ar,                   
--SectorType                  
co.Name_En as SectorType_Name_En,                  
co.Name_Ar as SectorType_Name_Ar,                  
--Subtype                  
cs.Name_En as Subtype_Name_En,                  
cs.Name_Ar as Subtype_Name_Ar,                  
--Priorty                  
cp.Name_En as Priority_Name_En,                  
cp.Name_Ar as Priority_Name_Ar,                  
--Status                  
ccrs.Name_En as Status_Name_En,                  
ccrs.Name_Ar as Status_Name_Ar,             
--Court type              
cctgl.Name_En as Court_Type_Name_En,          
cctgl.Name_Ar as Court_Type_Name_Ar                  
FROM CMS_CASE_REQUEST ccr                  
                  
left join Department cd on  ccr.DepartmentId = cd.Id                  
left join CMS_GOVERNMENT_ENTITY_G2G_LKP cg on  ccr.GovtEntityId = cg.EntityId                  
left join CMS_OPERATING_SECTOR_TYPE_G2G_LKP co on ccr.SectorTypeId= co.Id                   
left join CMS_REQUEST_TYPE_G2G_LKP rt on ccr.RequestTypeId= rt.Id                     
left join CMS_SUBTYPE_G2G_LKP cs on ccr.SubTypeId = cs.Id                  
left join CMS_PRIORITY_G2G_LKP cp on ccr.PriorityId = cp.Id                  
left join CMS_CASE_REQUEST_STATUS_G2G_LKP ccrs on ccr.StatusId = ccrs.Id           
left join CMS_COURT_TYPE_G2G_LKP cctgl on ccr.CourtTypeId = cctgl.Id           
WHERE ccr.IsDeleted != 1 AND CCr.IsLinked != 1               
AND (ccr.RequestNumber=@requestNumber OR @requestNumber IS NULL OR @requestNumber='0')                
AND (ccr.StatusId=@statusId OR @statusId IS NULL OR @statusId='0')                
AND (ccr.Subject=@subject OR @subject IS NULL OR @subject='')                
AND(ccr.SectorTypeId=@sectorTypeId OR @sectorTypeId IS NULL OR @sectorTypeId='0')                
AND (CAST(ccr.RequestDate as date)>=@requestFrom OR @requestFrom IS NULL OR @requestFrom='')               
AND(CAST(ccr.RequestDate as date)<=@requestTo OR @requestTo IS NULL OR @requestTo='')
AND ((ccr.TransferStatusId = CASE WHEN @showUndefinedRequests = 0 THEN 2 ELSE 4 END) OR (@showUndefinedRequests = 0 AND ccr.TransferStatusId is null) )
ORDER BY ccr.RequestNumber desc              
END    


/****** Object:  StoredProcedure [dbo].[pGetHOSBSectorId]    Script Date: 29/11/2022 1:33:28 pm ******/
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pGetHOSBSectorId]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pGetHOSBSectorId]
GO
CREATE PROCEDURE pGetHOSBSectorId
(
@sectorTypeId INT
)
AS
BEGIN
select UU.* from UMS_USER UU
LEFT JOIN UMS_USER_ROLES UUR ON UUR.UserId = UU.Id
LEFT JOIN UMS_ROLE UR ON UR.Id = UUr.RoleId
WHERE UR.Id = '93e5374b-cbd9-494e-92d4-d9d7d44c2c39' AND UU.SectorTypeId = @sectorTypeId
END

  
/****** Object:  StoredProcedure [dbo].[pCmsCaseFileList]    Script Date: 29/11/2022 1:33:28 pm ******/
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pCmsCaseFileList]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pCmsCaseFileList]
GO

CREATE    PROCEDURE [dbo].[pCmsCaseFileList]      
(      
@fileNumber NVARCHAR(1000) =null,      
@statusId int = null,      
@createdFrom datetime = null,      
@createdTo datetime = null,      
@modifiedFrom datetime = null,      
@modifiedTo datetime = null,
@sectorTypeId int
)      
As      
      
Select distinct  ccf.*,      
--Case request      
ccr.RequestNumber,       
ccr.RequestDate,       
ccr.Subject,      
      
--Status          
ccfs.Name_En as StatusNameEn,          
ccfs.Name_Ar as StatusNameAr,          
      
-- Government Entity      
cge.Name_En as GovernmentEntityNameEn,      
cge.Name_Ar as GovernmentEntityNameAr,      
-- Priority      
cpg.Name_En as PriorityNameEn,      
cpg.Name_Ar as PriorityNameAr,      
      
-- Operating Sector      
ost.Name_En as RequestTypeNameEn,      
ost.Name_Ar as RequestTypeNameAr      
      
from CMS_CASE_FILE ccf      
left join CMS_CASE_FILE_STATUS_G2G_LKP ccfs on ccf.StatusId= ccfs.Id      
left join CMS_CASE_REQUEST ccr on ccf.RequestId = ccr.RequestId       
left join CMS_GOVERNMENT_ENTITY_G2G_LKP cge on cge.EntityId=ccr.GovtEntityId      
left join CMS_PRIORITY_G2G_LKP cpg on cpg.Id = ccr.PriorityId      
left join CMS_REQUEST_TYPE_G2G_LKP ost on ost.Id=ccr.RequestTypeId
WHERE ccf.IsDeleted != 1 AND ccf.IsLinked != 1       
AND (ccf.FileNumber=@fileNumber OR @fileNumber IS NULL OR @fileNumber='')      
AND ((ccf.StatusId=@statusId OR @statusId IS NULL OR @statusId='0') AND ccf.StatusId <= 64 AND ccf.StatusId != 8)        
AND(CAST(ccf.CreatedDate as date)>=@createdFrom OR @createdFrom IS NULL OR @createdFrom='')        
AND(CAST(ccf.CreatedDate as date)<=@createdTo OR @createdTo IS NULL OR @createdTo='')        
AND(CAST(ccf.ModifiedDate as date)>=@modifiedFrom OR @modifiedFrom IS NULL OR @modifiedFrom='')        
AND(CAST(ccf.ModifiedDate as date)<=@modifiedTo OR @modifiedTo IS NULL OR @modifiedTo='') 
AND((SELECT count(*) from CMS_CASE_FILE_SECTOR_ASSIGNMENT WHERE SectorTypeId = @sectorTypeId AND FileId = ccf.FileId AND IsDeleted != 1) > 0)
ORDER BY ccf.CreatedDate DESC  



  
/****** Object:  StoredProcedure [dbo].[pRegisterCaseFileList]    Script Date: 29/11/2022 1:33:28 pm ******/
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pRegisterCaseFileList]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pRegisterCaseFileList]
GO

CREATE   PROCEDURE [dbo].[pRegisterCaseFileList]      
(      
@fileNumber NVARCHAR(1000) =null,      
@statusId int = null,      
@createdFrom datetime = null,      
@createdTo datetime = null,      
@modifiedFrom datetime = null,      
@modifiedTo datetime = null,
@sectorTypeId int    
)      
As      
      
Select distinct  ccf.*,      
--Case request      
ccr.RequestNumber,       
ccr.RequestDate,       
ccr.Subject,      
      
--Status          
ccfs.Name_En as StatusNameEn,          
ccfs.Name_Ar as StatusNameAr,          
      
-- Government Entity      
cge.Name_En as GovernmentEntityNameEn,      
cge.Name_Ar as GovernmentEntityNameAr,      
-- Priority      
cpg.Name_En as PriorityNameEn,      
cpg.Name_Ar as PriorityNameAr,      
      
-- Operating Sector      
ost.Name_En as OperatingSectorNameEn,      
ost.Name_Ar as OperatingSectorNameAr      
      
from CMS_CASE_FILE ccf      
left join CMS_CASE_FILE_STATUS_G2G_LKP ccfs on ccf.StatusId= ccfs.Id      
left join CMS_CASE_REQUEST ccr on ccf.RequestId = ccr.RequestId       
left join CMS_GOVERNMENT_ENTITY_G2G_LKP cge on cge.EntityId=ccr.GovtEntityId      
left join CMS_PRIORITY_G2G_LKP cpg on cpg.Id=ccr.PriorityId      
left join CMS_OPERATING_SECTOR_TYPE_G2G_LKP  ost on ost.Id=ccr.SectorTypeId      
WHERE ccr.IsDeleted != 1        AND ccr.IsLinked != 1 
AND (ccf.FileNumber=@fileNumber OR @fileNumber IS NULL OR @fileNumber='')        
AND ((ccf.StatusId=@statusId OR @statusId IS NULL OR @statusId='0') AND ccf.StatusId > 64 AND ccf.StatusId != 8)   
AND(CAST(ccf.CreatedDate as date)>=@createdFrom OR @createdFrom IS NULL OR @createdFrom='')        
AND(CAST(ccf.CreatedDate as date)<=@createdTo OR @createdTo IS NULL OR @createdTo='')        
AND(CAST(ccf.ModifiedDate as date)>=@modifiedFrom OR @modifiedFrom IS NULL OR @modifiedFrom='')        
AND(CAST(ccf.ModifiedDate as date)<=@modifiedTo OR @modifiedTo IS NULL OR @modifiedTo='')   
AND((SELECT count(*) from CMS_CASE_FILE_SECTOR_ASSIGNMENT WHERE SectorTypeId = @sectorTypeId AND FileId = ccf.FileId AND IsDeleted != 1) > 0)
ORDER BY ccf.CreatedDate DESC   


/****** Object:  StoredProcedure [dbo].[pOfficialDocuments]    Script Date: 29/11/2022 1:33:28 pm ******/
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pOfficialDocuments]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pOfficialDocuments]
GO
          
CREATE   Procedure [dbo].[pOfficialDocuments]                 
(                    
  @referenceGuid uniqueidentifier = NULL       
)                    
AS                              
begin                    
Select LUD.*          
  , ATP.Type_Ar          
  , ATP.Type_En          
from UPLOADED_DOCUMENT LUD                              
INNER JOIN ATTACHMENT_TYPE ATP ON ATP.AttachmentTypeId = LUD.AttachmentTypeId            
Where LUD.IsDeleted != 1 AND ATP.IsOfficialLetter = 1                            
AND (LUD.ReferenceGuid = @referenceGuid OR @referenceGuid IS NULL)                            
SET ROWCOUNT 0                              
SET NOCOUNT OFF                              
RETURN                     
end     


/****** Object:  StoredProcedure [dbo].[pSchedulingCourtVisitListById]    Script Date: 29/11/2022 1:33:28 pm ******/
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pSchedulingCourtVisitListById]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pSchedulingCourtVisitListById]
GO

CREATE PROCEDURE  [dbo].[pSchedulingCourtVisitListById]  
(
@HearingId  uniqueidentifier  
)  
AS  
SELECT distinct ccv.*,
CONCAT(uu.FirstName_En,'',uu.SecondName_En,'',uu.LastName_En)as lawyerNameEn ,  
CONCAT(uu.FirstName_Ar,'',uu.SecondName_Ar,'',uu.LastName_Ar)as lawyerNameAr  
FROM CMS_COURT_VISIT ccv  
left join UMS_USER uu on uu.Id=ccv.LawyerId  
where ccv.HearingId = @HearingId  

/****** Object:  StoredProcedure [dbo].[pUserListBySector]    Script Date: 29/11/2022 1:33:28 pm ******/
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pUserListBySector]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pUserListBySector]
GO

CREATE   Procedure [dbo].[pUserListBySector]      
(
@sectorTypeId INT
)
AS          
 Select us.Id      
      , us.FirstName_En AS FirstNameEnglish      
      , us.FirstName_Ar AS FirstNameArabic      
   , us.DepartmentId As DepartmentId      
      , dep.Name_Ar AS DepartmentArabic      
      , dep.Name_En AS DepartmentEnglish      
      , ut.Name_Ar  AS UserTypeArabic      
      , ut.Name_En  AS UserTypeEnglish      
      , us.PhoneNumber AS MobileNumber      
      , us.UserName      
      , us.Email      
  from UMS_USER us      
    INNER JOIN Department dep ON us.DepartmentId = dep.Id      
    LEFT JOIN UMS_USER_TYPE ut ON us.UserTypeId = ut.Id      
 WHERE us.IsDeleted=0   And us.SectorTypeId = @sectorTypeId   
    ORDER BY us.CreatedDate  desc    
 RETURN 

 ------------------------------------------------------------------------------------------------------------------------------
-------------------------------------------------------- Consultation Start --------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------
-----------------------------pComsConsultationRequestList
/****** Object:  StoredProcedure [dbo].[pComsConsultationRequestList]    Script Date: 1/4/2023 4:20:09 PM ******/
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pComsConsultationRequestList]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)

DROP PROCEDURE [dbo].[pComsConsultationRequestList]
GO

/****** Object:  StoredProcedure [dbo].[pComsConsultationRequestList]    Script Date: 1/4/2023 4:20:09 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE       PROCEDURE [dbo].[pComsConsultationRequestList]    
(  
@requestNumber NVARCHAR(500) = null,  
@statusId int =null,  
@subject NVARCHAR(1000) =null,  
@requestFrom datetime=null,  
@requestTo datetime=null,  
@requestTypeId int = null  
)  
AS    
Begin    
Select distinct  ccr.ConsultationRequestId,  
 ccr.RequestNumber,  
 ccr.Subject,  
 ccr.OfficialLetterOutboxNumber,  
 ccr.FatwaInboxNumber,  
 ccr.RequestDate,  
--Status  
ccr.RequestStatusId,  
ccrs.Name_En as Status_Name_En,    
ccrs.Name_Ar as Status_Name_Ar,  
--RequestType  
ccr.RequestTypeId,  
coct.Name_En as RequestType_Name_En,  
coct.Name_Ar as RequestType_Name_Ar  
  
  
    
FROM COMS_CONSULTATION_REQUEST ccr    
left join CMS_CASE_REQUEST_STATUS_G2G_LKP ccrs on ccr.RequestStatusId = ccrs.Id    
left join CMS_REQUEST_TYPE_G2G_LKP coct on ccr.RequestTypeId = coct.Id  
WHERE ccr.IsDeleted != 1  
AND (ccr.RequestTypeId=@RequestTypeId)   
AND (ccr.RequestNumber LIKE '%' + @requestNumber + '%' OR @requestNumber IS NULL OR @requestNumber='')  
AND (ccr.Subject LIKE '%' + @subject + '%' OR @subject IS NULL OR @subject='')     
AND (ccr.RequestStatusId=@statusId OR @statusId IS NULL OR @statusId='')  
AND(CAST(ccr.RequestDate as date)>=@requestFrom OR @requestFrom IS NULL OR @requestFrom='')  
AND (CAST(ccr.RequestDate as date)<=@requestTo OR @requestTo IS NULL OR @requestTo='')    
   
ORDER BY ccr.RequestDate desc  
END    
GO
----------------------------------------------------------pComsConsultationRequestList
/****** Object:  StoredProcedure [dbo].[[pConsultationDetailById]]    Script Date: 1/4/2023 4:20:09 PM ******/
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pConsultationDetailById]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)

DROP PROCEDURE [dbo].[pConsultationDetailById]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE        PROCEDURE [dbo].[pConsultationDetailById]          
(          
@consultationId uniqueidentifier          
)          
As          
 BEGIN         
Select distinct  ccr.RequestNumber         
,ccr.ConsultationRequestId        
        , ccr.GEOpinion     
  , ccr.Introduction    
       , ccr.Remarks         
       , ccr. ContractAmount75000KD      
    , ccr.IsConfidential    
       , ccr. ReferenceNo         
       , ccr.  RequestDate         
         , ccr.ReferenceDate         
         , ccr.RequestTitle         
         , ccr.[Description]             
         , ccr.Subject         
        , pr.Name_En AS [priorityEn]          
      , pr.Name_Ar AS [priorityAr]          
      , ge.Name_En AS [gvtEntityEn]          
      , ge.Name_Ar AS [gvtEntityAr]          
      , cdf.Name_En AS [departmentEn]          
      , cdf.Name_Ar AS [departmentAr]          
      , sfl.Name_En AS [contractSubTypeEn]          
      , sfl.Name_Ar AS [contractSubTypeAr]          
      , crs.Name_En AS [requestStatusEn]          
      , crs.Name_Ar AS [requestStatusAr]       
   , rtf.Name_En AS [RequestTypeEn]      
   , rtf.Name_Ar AS [RequestTypeAr]      
             
from COMS_CONSULTATION_REQUEST ccr          
LEFT JOIN PRIORITY pr ON ccr.PriorityId = pr.Id          
LEFT JOIN CMS_GOVERNMENT_ENTITY_FTW_LKP ge ON ccr.GovtEntityId = ge.EntityId          
LEFT JOIN Department cdf ON ccr.DepartmentId = cdf.Id        
LEFT JOIN CMS_CASE_REQUEST_STATUS_G2G_LKP crs ON crs.Id = ccr.RequestStatusId         
LEFT JOIN CMS_REQUEST_TYPE_G2G_LKP rtf ON rtf.Id = ccr.RequestTypeId      
LEFT JOIN CMS_SUBTYPE_G2G_LKP sfl ON sfl.RequestTypeId = rtf.Id AND sfl.Id = ccr.RequestSubTypeId   
WHERE ccr.IsDeleted != 1            
and (ccr.ConsultationRequestId = @consultationId OR @consultationId IS NULL OR @consultationId = '00000000-0000-0000-0000-000000000000') 
END
GO
-------------------------------------------------------pCommuncationListByConsultationRequest

IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pCommuncationListByConsultationRequest]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)

/****** Object:  StoredProcedure [dbo].[pCommuncationListByConsultationRequest]    Script Date: 1/5/2023 6:21:15 PM ******/
DROP PROCEDURE [dbo].[pCommuncationListByConsultationRequest]
GO

/****** Object:  StoredProcedure [dbo].[pCommuncationListByConsultationRequest]    Script Date: 1/5/2023 6:21:15 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[pCommuncationListByConsultationRequest]               
(              
 @RequestId nvarchar(150) = NULL            
)              
AS                  
BEGIN                 
   Select DISTINCT       
  cl.CommunicationId    
  , cl.CommunicationTypeId    
  , ct.NameEn AS Activity_En              
  , ct.NameAr AS Activity_Ar              
  , cl.CreatedBy As CreatedBy              
  , cl.CreatedDate As [CreatedDate]              
  , cl.Title As Remarks               
  , cl.InboxNumber as GeReferenceNo      
  , cl.InboxDate as GeReferenceDate       
  , cl.OutboxNumber as FatwaReferenceNo            
  , cl.OutboxDate as FatwaReferenceDate             
  , crt.NameAr as CorrespondenceTypeAr      
  , crt.NameEn as CorrespondenceTypeEn             
   FROM COMM_COMMUNICATION cl             
   INNER JOIN COMM_COMMUNICATION_CORRESPONDENCE_TYPE crt on crt.CorrespondenceTypeId = cl.CorrespondenceTypeId      
   INNER JOIN COMM_COMMUNICATION_TYPE ct ON ct.CommunicationTypeId = cl.CommunicationTypeId                
   INNER JOIN COMM_COMMUNICATION_TARGET_LINK tl ON tl.communicationId = cl.CommunicationId          
   INNER JOIN LINK_TARGET lt ON lt.TargetLinkId = tl.TargetLinkId                  
   WHERE lt.ReferenceId = @RequestId       
   --AND cl.CommunicationTypeId = 2 -- Request for Meeting    
   Order by cl.CreatedDate desc      
                             
END 
GO

-------------------------------------------pGetConsultationRequestStatusHistory
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pGetConsultationRequestStatusHistory]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)

/****** Object:  StoredProcedure [dbo].[pGetConsultationRequestStatusHistory]    Script Date: 1/6/2023 4:18:13 PM ******/
DROP PROCEDURE [dbo].[pGetConsultationRequestStatusHistory]
GO

/****** Object:  StoredProcedure [dbo].[pGetConsultationRequestStatusHistory]    Script Date: 1/6/2023 4:18:13 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE  PROCEDURE [dbo].[pGetConsultationRequestStatusHistory]        
(                    
  @ConsultationRequestId uniqueidentifier = NULL     
     
)   
AS    
BEGIN     

   Select crs.HistoryId
  
   , cre.Name_En as EventEn
   , cre.Name_Ar as EventAr
   , crs.EventId
   , cwr.Name_En as StatusEn
   , cwr.Name_Ar as StatusAr
   , crs.StatusId
   , crs.Remarks
   , crs.CreatedBy
   , crs.CreatedDate
   , crs.ModifiedBy
   , crs.ModifiedDate
   , crs.DeletedBy
   , crs.DeletedDate
   , crs.IsDeleted
  

   from COMS_CONSULTATION_REQUEST_STATUS_HISTORY crs
    LEFT JOIN CMS_CASE_REQUEST_EVENT_G2G_LKP cre ON cre.Id = crs.EventId
	LEFT JOIN CMS_CASE_REQUEST_STATUS_G2G_LKP cwr ON cwr.Id = crs.StatusId
 Where      
 crs.IsDeleted = 0    
 AND (crs.ConsultationRequestId = @ConsultationRequestId OR @ConsultationRequestId IS NULL OR @ConsultationRequestId='00000000-0000-0000-0000-000000000000')      
  END 
   
GO



/****** Object:  StoredProcedure [dbo].[pConsultationDetailById]    Script Date: 1/9/2023 5:32:52 PM ******/
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pConsultationDetailById]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)

DROP PROCEDURE [dbo].[pConsultationDetailById]
GO

/****** Object:  StoredProcedure [dbo].[pConsultationDetailById]    Script Date: 1/9/2023 5:32:52 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE        PROCEDURE [dbo].[pConsultationDetailById]          
(          
@consultationId uniqueidentifier          
)          
As          
 BEGIN         
Select distinct  ccr.RequestNumber         
,ccr.ConsultationRequestId        
        , ccr.GEOpinion     
  , ccr.Introduction    
       , ccr.Remarks         
       , ccr. ContractAmount75000KD      
    , ccr.IsConfidential    
       , ccr. ReferenceNo         
       , ccr.  RequestDate         
         , ccr.ReferenceDate         
         , ccr.RequestTitle         
         , ccr.[Description]             
         , ccr.Subject         
        , pr.Name_En AS [priorityEn]          
      , pr.Name_Ar AS [priorityAr]          
      , ge.Name_En AS [gvtEntityEn]          
      , ge.Name_Ar AS [gvtEntityAr]          
      , cdf.Name_En AS [departmentEn]          
      , cdf.Name_Ar AS [departmentAr]          
      , sfl.Name_En AS [contractSubTypeEn]          
      , sfl.Name_Ar AS [contractSubTypeAr] 
	  ,ccr.RequestStatusId
      , crs.Name_En AS [requestStatusEn]          
      , crs.Name_Ar AS [requestStatusAr]       
   , rtf.Name_En AS [RequestTypeEn]      
   , rtf.Name_Ar AS [RequestTypeAr]  
   ,ccr.TransferStatusId
             
from COMS_CONSULTATION_REQUEST ccr          
LEFT JOIN PRIORITY pr ON ccr.PriorityId = pr.Id          
LEFT JOIN CMS_GOVERNMENT_ENTITY_FTW_LKP ge ON ccr.GovtEntityId = ge.EntityId          
LEFT JOIN Department cdf ON ccr.DepartmentId = cdf.Id        
LEFT JOIN CMS_CASE_REQUEST_STATUS_G2G_LKP crs ON crs.Id = ccr.RequestStatusId         
LEFT JOIN CMS_REQUEST_TYPE_G2G_LKP rtf ON rtf.Id = ccr.RequestTypeId      
LEFT JOIN CMS_SUBTYPE_G2G_LKP sfl ON sfl.RequestTypeId = rtf.Id AND sfl.Id = ccr.RequestSubTypeId   
WHERE ccr.IsDeleted != 1            
and (ccr.ConsultationRequestId = @consultationId OR @consultationId IS NULL OR @consultationId = '00000000-0000-0000-0000-000000000000') 
END
GO






------------------------------------------------------------------------------------------------------------------------------
-------------------------------------------------------- Consultation End --------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------
 RETURN 

 
/****** Object:  StoredProcedure [dbo].[pCmsCaseFileList]    Script Date: 29/11/2022 1:33:28 pm ******/
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pCmsCaseFileList]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pCmsCaseFileList]
GO

CREATE    PROCEDURE [dbo].[pCmsCaseFileList]      
(      
@fileNumber NVARCHAR(1000) =null,      
@statusId int = null,      
@createdFrom datetime = null,      
@createdTo datetime = null,      
@modifiedFrom datetime = null,      
@modifiedTo datetime = null,
@sectorTypeId int,
@userId NVARCHAR(500)
)      
As      
      
Select distinct  ccf.*,      
--Case request      
ccr.RequestNumber,       
ccr.RequestDate,       
ccr.Subject,      
      
--Status          
ccfs.Name_En as StatusNameEn,          
ccfs.Name_Ar as StatusNameAr,          
      
-- Government Entity      
cge.Name_En as GovernmentEntityNameEn,      
cge.Name_Ar as GovernmentEntityNameAr,      
-- Priority      
cpg.Name_En as PriorityNameEn,      
cpg.Name_Ar as PriorityNameAr,      
      
-- Operating Sector      
ost.Name_En as RequestTypeNameEn,      
ost.Name_Ar as RequestTypeNameAr      
      
from CMS_CASE_FILE ccf      
left join CMS_CASE_FILE_STATUS_G2G_LKP ccfs on ccf.StatusId= ccfs.Id      
left join CMS_CASE_REQUEST ccr on ccf.RequestId = ccr.RequestId       
left join CMS_GOVERNMENT_ENTITY_G2G_LKP cge on cge.EntityId=ccr.GovtEntityId      
left join CMS_PRIORITY_G2G_LKP cpg on cpg.Id = ccr.PriorityId      
left join CMS_REQUEST_TYPE_G2G_LKP ost on ost.Id=ccr.RequestTypeId
WHERE ccf.IsDeleted != 1 AND ccf.IsLinked != 1       
AND (ccf.FileNumber=@fileNumber OR @fileNumber IS NULL OR @fileNumber='')      
AND ((ccf.StatusId=@statusId OR @statusId IS NULL OR @statusId='0') AND ccf.StatusId <= 64 AND ccf.StatusId != 8)        
AND(CAST(ccf.CreatedDate as date)>=@createdFrom OR @createdFrom IS NULL OR @createdFrom='')        
AND(CAST(ccf.CreatedDate as date)<=@createdTo OR @createdTo IS NULL OR @createdTo='')        
AND(CAST(ccf.ModifiedDate as date)>=@modifiedFrom OR @modifiedFrom IS NULL OR @modifiedFrom='')        
AND(CAST(ccf.ModifiedDate as date)<=@modifiedTo OR @modifiedTo IS NULL OR @modifiedTo='') 
AND((SELECT count(*) from CMS_CASE_FILE_SECTOR_ASSIGNMENT WHERE SectorTypeId = @sectorTypeId AND FileId = ccf.FileId AND IsDeleted != 1) > 0)
AND(((SELECT COUNT(*) FROM UMS_USER_ROLES WHERE RoleId = '93e5374b-cbd9-494e-92d4-d9d7d44c2c39' AND UserId = @userId) > 0) OR ((SELECT COUNT(*) FROM CMS_CASE_ASSIGNMENT WHERE (LawyerId = @userId OR SupervisorId = @userId) AND ReferenceId = ccf.FileId AND IsDeleted = 0) > 0) )
ORDER BY ccf.CreatedDate DESC  


/****** Object:  StoredProcedure [dbo].[pRegisterCaseFileList]    Script Date: 29/11/2022 1:33:28 pm ******/
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pRegisterCaseFileList]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pRegisterCaseFileList]
GO

CREATE   PROCEDURE [dbo].[pRegisterCaseFileList]      
(      
@fileNumber NVARCHAR(1000) =null,      
@statusId int = null,      
@createdFrom datetime = null,      
@createdTo datetime = null,      
@modifiedFrom datetime = null,      
@modifiedTo datetime = null,
@sectorTypeId int,
@userId NVARCHAR(500)    
)      
As      
      
Select distinct  ccf.*,      
--Case request      
ccr.RequestNumber,       
ccr.RequestDate,       
ccr.Subject,      
      
--Status          
ccfs.Name_En as StatusNameEn,          
ccfs.Name_Ar as StatusNameAr,          
      
-- Government Entity      
cge.Name_En as GovernmentEntityNameEn,      
cge.Name_Ar as GovernmentEntityNameAr,      
-- Priority      
cpg.Name_En as PriorityNameEn,      
cpg.Name_Ar as PriorityNameAr,      
      
-- Operating Sector      
ost.Name_En as OperatingSectorNameEn,      
ost.Name_Ar as OperatingSectorNameAr      
      
from CMS_CASE_FILE ccf      
left join CMS_CASE_FILE_STATUS_G2G_LKP ccfs on ccf.StatusId= ccfs.Id      
left join CMS_CASE_REQUEST ccr on ccf.RequestId = ccr.RequestId       
left join CMS_GOVERNMENT_ENTITY_G2G_LKP cge on cge.EntityId=ccr.GovtEntityId      
left join CMS_PRIORITY_G2G_LKP cpg on cpg.Id=ccr.PriorityId      
left join CMS_OPERATING_SECTOR_TYPE_G2G_LKP  ost on ost.Id=ccr.SectorTypeId      
WHERE ccr.IsDeleted != 1        AND ccr.IsLinked != 1 
AND (ccf.FileNumber=@fileNumber OR @fileNumber IS NULL OR @fileNumber='')        
AND ((ccf.StatusId=@statusId OR @statusId IS NULL OR @statusId='0') AND ccf.StatusId > 64 AND ccf.StatusId != 8)   
AND(CAST(ccf.CreatedDate as date)>=@createdFrom OR @createdFrom IS NULL OR @createdFrom='')        
AND(CAST(ccf.CreatedDate as date)<=@createdTo OR @createdTo IS NULL OR @createdTo='')        
AND(CAST(ccf.ModifiedDate as date)>=@modifiedFrom OR @modifiedFrom IS NULL OR @modifiedFrom='')        
AND(CAST(ccf.ModifiedDate as date)<=@modifiedTo OR @modifiedTo IS NULL OR @modifiedTo='')   
AND((SELECT count(*) from CMS_CASE_FILE_SECTOR_ASSIGNMENT WHERE SectorTypeId = @sectorTypeId AND FileId = ccf.FileId AND IsDeleted != 1) > 0)
AND(((SELECT COUNT(*) FROM UMS_USER_ROLES WHERE RoleId = '93e5374b-cbd9-494e-92d4-d9d7d44c2c39' AND UserId = @userId) > 0) OR ((SELECT COUNT(*) FROM CMS_CASE_ASSIGNMENT WHERE (LawyerId = @userId OR SupervisorId = @userId) AND ReferenceId = ccf.FileId AND IsDeleted = 0) > 0) )
ORDER BY ccf.CreatedDate DESC 


/****** Object:  StoredProcedure [dbo].[pGetDraftCaseRequestList]    Script Date: 29/11/2022 1:33:28 pm ******/
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pGetDraftCaseRequestList]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pGetDraftCaseRequestList]
GO

CREATE PROC [dbo].[pGetDraftCaseRequestList]        
(       
@ReferenceId  uniqueIdentifier     
)      
AS        
BEGIN        
select cdt.*,        
at.Type_En AS TypeEn,        
at.Type_Ar AS TypeAr,        
sdds.NameEn AS StatusEn,        
sdds.NameAr AS StatusAr        
from CMS_DRAFTED_TEMPLATE cdt     
LEFT JOIN CMS_REGISTERED_CASE crc on cdt.ReferenceId = crc.CaseId  
LEFT JOIN ATTACHMENT_TYPE at ON at.AttachmentTypeId = cdt.AttachmentTypeId        
LEFT JOIN CMS_DRAFT_DOCUMENT_STATUS sdds ON sdds.Id = cdt.StatusId        
WHERE cdt.IsDeleted != 1    
AND  cdt.ReferenceId = @ReferenceId     
ORDER BY cdt.DraftNumber desc        
END     


--pMojRegistrationRequests
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pMojRegistrationRequests]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pMojRegistrationRequests]
GO

CREATE PROC pMojRegistrationRequests 
(
@sectorTypeId INT
)
AS    
BEGIN    
select cmrr.*,    
ccf.FileName AS FileName,    
ccf.FileNumber AS FileNumber    
from CMS_MOJ_REGISTRATION_REQUEST cmrr    
LEFT JOIN CMS_CASE_FILE ccf ON ccf.FileId = cmrr.FileId    
WHERE cmrr.IsDeleted != 1 AND cmrr.IsRegistered != 1 AND cmrr.SectorTypeId = @sectorTypeId
ORDER BY cmrr.CreatedDate desc    
END 

  
--pCmsRegisteredCaseDetailById
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pCmsRegisteredCaseDetailById]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pCmsRegisteredCaseDetailById]
GO
  
CREATE PROCEDURE [pCmsRegisteredCaseDetailById]         
(        
	@caseId uniqueidentifier null,  
	@userId NVARCHAR(256)  
)        
AS          
Begin          
Select distinct crc.*,          
--GovtEntity          
ge.Name_En as GovtEntityNameEn,          
ge.Name_Ar as GovtEntityNameAr,            
--Status          
crcs.Name_En as StatusNameEn,          
crcs.Name_Ar as StatusNameAr,          
--CourtType          
crtp.Id as CourtTypeId,  
crtp.Name_En as CourtTypeNameEn,          
crtp.Name_Ar as CourtTypeNameAr,       
--Court          
crt.Name_En as CourtNameEn,          
crt.Name_Ar as CourtNameAr,          
crt.Number as CourtNumber,          
crt.District as CourtDistrict,         
crt.Location as CourtLocation,           
--Chamber          
chm.Name_En as ChamberNameEn,          
chm.Name_Ar as ChamberNameAr,           
chm.Number as ChamberNumber,            
chm.Address as ChamberAddress,       
--File      
ccf.FileNumber AS FileNumber,    
CCF.FileName As FileName,  
CAST(CASE WHEN cca.Id IS NOT NULL THEN 1 ELSE 0 END AS BIT) AS IsAssigned  
          
FROM CMS_REGISTERED_CASE crc          
          
LEFT JOIN CMS_COURT_G2G_LKP crt on  crt.Id = crc.CourtId          
LEFT JOIN CMS_CHAMBER_G2G_LKP chm ON chm.Id = crc.ChamberId      
LEFT JOIN CMS_COURT_TYPE_G2G_LKP crtp ON crtp.Id = crt.TypeId      
LEFT JOIN CMS_GOVERNMENT_ENTITY_G2G_LKP ge ON ge.EntityId = crc.GovtEntityId      
LEFT JOIN CMS_REGISTERED_CASE_STATUS_G2G_LKP crcs ON crcs.Id = crc.StatusId      
LEFT JOIN CMS_CASE_FILE ccf ON ccf.FileId = crc.FileId  
LEFT JOIN CMS_CASE_ASSIGNMENT cca ON cca.LawyerId = (SELECT Id from UMS_USER where Email = @userId) AND cca.ReferenceId = ccf.FileId --crc.CaseId   old
WHERE crc.IsDeleted != 1 AND crc.CaseId = @caseId              
ORDER BY crc.CreatedDate desc      
END        

  
--pLinkedFilesByPrimaryFileId
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pLinkedFilesByPrimaryFileId]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pLinkedFilesByPrimaryFileId]
GO
  
CREATE    PROCEDURE [dbo].[pLinkedFilesByPrimaryFileId]        
(        
@fileId UNIQUEIDENTIFIER   
)        
As        
        
Select distinct  ccf.*,        
--Case request        
ccr.RequestNumber,         
ccr.RequestDate,         
ccr.Subject,        
        
--Status            
ccfs.Name_En as StatusNameEn,            
ccfs.Name_Ar as StatusNameAr,            
        
-- Government Entity        
cge.Name_En as GovernmentEntityNameEn,        
cge.Name_Ar as GovernmentEntityNameAr,        
-- Priority        
cpg.Name_En as PriorityNameEn,        
cpg.Name_Ar as PriorityNameAr,        
        
-- Operating Sector        
ost.Name_En as RequestTypeNameEn,        
ost.Name_Ar as RequestTypeNameAr        
        
from CMS_CASE_FILE_LINKED_FILE ccflf
left join CMS_CASE_FILE ccf on ccf.FileId = ccflf.LinkedFileId    
left join CMS_CASE_FILE_STATUS_G2G_LKP ccfs on ccf.StatusId= ccfs.Id      
left join CMS_CASE_REQUEST ccr on ccf.RequestId = ccr.RequestId         
left join CMS_GOVERNMENT_ENTITY_G2G_LKP cge on cge.EntityId=ccr.GovtEntityId        
left join CMS_PRIORITY_G2G_LKP cpg on cpg.Id = ccr.PriorityId        
left join CMS_REQUEST_TYPE_G2G_LKP ost on ost.Id=ccr.RequestTypeId  
WHERE ccf.IsDeleted != 1 AND ccf.IsLinked = 1 AND ccflf.PrimaryFileId = @fileId
ORDER BY ccf.CreatedDate DESC    


--pCmsRegisteredCasesListByFileId
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pCmsRegisteredCasesListByFileId]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pCmsRegisteredCasesListByFileId]
GO
  
CREATE PROCEDURE [dbo].[pCmsRegisteredCasesListByFileId]           
(          
@fileId uniqueidentifier =null       
)          
AS            
Begin            
Select distinct crc.*,            
--GovtEntity            
ge.Name_En as GovtEntityNameEn,            
ge.Name_Ar as GovtEntityNameAr,              
--Status            
crcs.Name_En as StatusNameEn,            
crcs.Name_Ar as StatusNameAr,            
--CourtType            
crtp.Name_En as CourtTypeNameEn,            
crtp.Name_Ar as CourtTypeNameAr,         
--Court            
crt.Name_En as CourtNameEn,            
crt.Name_Ar as CourtNameAr,            
crt.Number as CourtNumber,            
crt.District as CourtDistrict,           
crt.Location as CourtLocation,             
--Chamber            
chm.Name_En as ChamberNameEn,            
chm.Name_Ar as ChamberNameAr,             
chm.Number as ChamberNumber,              
chm.Address as ChamberAddress,         
--File        
ccf.FileNumber AS FileNumber        
            
FROM CMS_REGISTERED_CASE crc            
            
LEFT JOIN CMS_COURT_G2G_LKP crt on  crt.Id = crc.CourtId            
LEFT JOIN CMS_CHAMBER_G2G_LKP chm ON chm.Id = crc.ChamberId        
LEFT JOIN CMS_COURT_TYPE_G2G_LKP crtp ON crtp.Id = crt.TypeId        
LEFT JOIN CMS_GOVERNMENT_ENTITY_G2G_LKP ge ON ge.EntityId = crc.GovtEntityId        
LEFT JOIN CMS_REGISTERED_CASE_STATUS_G2G_LKP crcs ON crcs.Id = crc.StatusId        
LEFT JOIN CMS_CASE_FILE ccf ON ccf.FileId = crc.FileId        
WHERE crc.IsDeleted != 1 AND (crc.IsSubCase = 0 or crc.IsSubCase IS NULL)  AND (crc.IsDissolved = 0 OR crc.IsDissolved IS NULL)
and (crc.FileId = @fileId OR @fileId IS NULL OR @fileId = '00000000-0000-0000-0000-000000000000')                 
ORDER BY crc.CreatedDate desc        
END 


--pCmsRequestWithDrawListByRequestId
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pCmsRequestWithDrawListByRequestId]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pCmsRequestWithDrawListByRequestId]
GO
CREATE PROCEDURE [dbo].[pCmsRequestWithDrawListByRequestId]                
(     
@requestId uniqueidentifier=null,
@requestNumber int = null,                  
@statusId int =null,                  
@subject NVARCHAR(1000) =null,                  
@sectorTypeId int=null,                  
@requestFrom datetime=null,                  
@requestTo datetime=null,  
@showUndefinedRequests bit = null  
)                  
AS                    
Begin                    
Select distinct ccr.*,                    
                    
--Department                    
cd.Name_En as Department_Name_En,                    
cd.Name_Ar as Department_Name_Ar,                    
--GovtEntity                    
cg.Name_En as GovermentEntity_Name_En,                    
cg.Name_Ar as GovermentEntity_Name_Ar,                    
--RequestType                    
rt.Name_En as RequestType_Name_En,                    
rt.Name_Ar as RequestType_Name_Ar,                     
--SectorType                    
co.Name_En as SectorType_Name_En,                    
co.Name_Ar as SectorType_Name_Ar,                    
--Subtype                    
cs.Name_En as Subtype_Name_En,                    
cs.Name_Ar as Subtype_Name_Ar,                    
--Priorty                    
cp.Name_En as Priority_Name_En,                    
cp.Name_Ar as Priority_Name_Ar,                    
--Status                    
cwrs.Name_En as Status_Name_En,
cwrs.Name_Ar as Status_Name_Ar,
cctgl.Name_En as Court_Type_Name_En,            
cctgl.Name_Ar as Court_Type_Name_Ar ,
--withCaseRequest
cwr.Reason

FROM CMS_CASE_REQUEST ccr                    
                    
LEFT JOIN Department cd on  ccr.DepartmentId = cd.Id                    
LEFT JOIN CMS_GOVERNMENT_ENTITY_G2G_LKP cg on  ccr.GovtEntityId = cg.EntityId                    
LEFT JOIN CMS_OPERATING_SECTOR_TYPE_G2G_LKP co on ccr.SectorTypeId= co.Id                     
LEFT JOIN CMS_REQUEST_TYPE_G2G_LKP rt on ccr.RequestTypeId= rt.Id                       
LEFT JOIN CMS_SUBTYPE_G2G_LKP cs on ccr.SubTypeId = cs.Id                    
LEFT JOIN CMS_PRIORITY_G2G_LKP cp on ccr.PriorityId = cp.Id                    
LEFT JOIN CMS_CASE_REQUEST_STATUS_G2G_LKP ccrs on ccr.StatusId = ccrs.Id             
LEFT JOIN CMS_COURT_TYPE_G2G_LKP cctgl on ccr.CourtTypeId = cctgl.Id     
LEFT JOIN CMS_WITHDRAW_REQUEST cwr ON cwr.CaseRequestId=ccr.RequestId
LEFT JOIN CMS_WITHDRAW_REQUEST_STATUS_FTW_LKP cwrs ON cwr.StatusId=cwrs.Id
 
WHERE   ccr.IsDeleted != 1         
AND (ccr.RequestNumber=@requestNumber OR @requestNumber IS NULL OR @requestNumber='0')                  
AND (ccr.StatusId=@statusId OR @statusId IS NULL OR @statusId='0')                  
AND (ccr.Subject=@subject OR @subject IS NULL OR @subject='')                  
AND(ccr.SectorTypeId=@sectorTypeId OR @sectorTypeId IS NULL OR @sectorTypeId='0')                    
And(cwr.CaseRequestId=@requestId OR @requestId Is NULL )
ORDER BY ccr.RequestNumber desc                
END    
 
--pCaseByPrimaryLawyerId
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pCaseByPrimaryLawyerId]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pCaseByPrimaryLawyerId]
GO
  
CREATE PROCEDURE [dbo].[pCaseByPrimaryLawyerId]             
(            
	@PrimaryLawyerId nvarchar(150) null          
)            
AS              
Begin              
	Select distinct crc.*,              
	--GovtEntity              
	ge.Name_En as GovtEntityNameEn,              
	ge.Name_Ar as GovtEntityNameAr,                
	--Status              
	crcs.Name_En as StatusNameEn,              
	crcs.Name_Ar as StatusNameAr,              
	--CourtType              
	crtp.Name_En as CourtTypeNameEn,              
	crtp.Name_Ar as CourtTypeNameAr,           
	--Court              
	crt.Name_En as CourtNameEn,              
	crt.Name_Ar as CourtNameAr,              
	crt.Number as CourtNumber,              
	crt.District as CourtDistrict,             
	crt.Location as CourtLocation,               
	--Chamber              
	chm.Name_En as ChamberNameEn,              
	chm.Name_Ar as ChamberNameAr,               
	chm.Number as ChamberNumber,                
	chm.Address as ChamberAddress,           
	--File          
	ccf.FileNumber AS FileNumber,        
	CCF.[FileName] As FileName        
	FROM CMS_REGISTERED_CASE crc              
	LEFT JOIN CMS_MERGE_REQUEST_SECONDARIES cmrs on cmrs.SecondaryId = crc.CaseId   
	LEFT JOIN CMS_MERGE_REQUEST cmr ON cmr.Id = cmrs.MergeRequestId  
	LEFT JOIN CMS_COURT_G2G_LKP crt on  crt.Id = crc.CourtId              
	LEFT JOIN CMS_CHAMBER_G2G_LKP chm ON chm.Id = crc.ChamberId          
	LEFT JOIN CMS_COURT_TYPE_G2G_LKP crtp ON crtp.Id = crt.TypeId          
	LEFT JOIN CMS_GOVERNMENT_ENTITY_G2G_LKP ge ON ge.EntityId = crc.GovtEntityId          
	LEFT JOIN CMS_REGISTERED_CASE_STATUS_G2G_LKP crcs ON crcs.Id = crc.StatusId          
	LEFT JOIN CMS_CASE_FILE ccf ON ccf.FileId = crc.FileId          
	LEFT JOIN CMS_CASE_ASSIGNMENT ca on ca.ReferenceId = crc.CaseId 
	WHERE crc.IsDeleted != 1 
	AND ca.LawyerId = @PrimaryLawyerId   
	ORDER BY crc.CreatedDate desc          
END 

/****** Object:  StoredProcedure [dbo].[pMeetingDecision]   Script Date: 11/21/2022 10:11:55 AM ******/
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pMeetingDecision]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pMeetingDecision]
GO
   
CREATE OR ALTER PROCEDURE [dbo].[pMeetingDecision]       
 @MeetingId nvarchar(150) = NULL      
AS                
BEGIN                 
 SELECT mt.MeetingId,  
  mt.[Subject],        
  mt.[Location],   
  mt.[Description],  
  mt.[Agenda],   
  mt.Comment,  
  CAST (cf.[FileNumber] as int) as [FileNumber], 
  mt.[Date],  
  mt.StartTime,  
  mt.EndTime,  
  mt.MeetingLink,   
  mty.NameAr as [MeetingTypeAr],     
  mty.NameEn as [MeetingTypeEn],     
  mt.MeetingStatusId,  
  mts.NameAr as [MeetingStatusAr],  
  mts.NameEn as [MeetingStatusEn],  
  '' as ModifiedBy  
  FROM MEET_MEETING mt        
  INNER JOIN MEET_MEETING_TYPE mty on mty.MeetingTypeId = mt.MeetingTypeId        
  INNER JOIN MEET_MEETING_STATUS mts on mts.MeetingStatusId = mt.MeetingStatusId   
  LEFT JOIN CMS_CASE_FILE cf on cf.FileId = mt.FileId    
  WHERE mt.IsDeleted = 0        
  AND mt.MeetingId = @MeetingId OR @MeetingId IS NULL  
  ORDER BY mt.CreatedDate DESC         
END 




/****** Object:  StoredProcedure [dbo].[pGetAllExecutionList]    Script Date: 12/01/2023 2:38:56 pm ******/
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pGetAllExecutionList]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pGetAllExecutionList]
GO

CREATE OR ALTER PROCEDURE [dbo].[pGetAllExecutionList]
AS
select ce.*,
ccf.Name_En as FileStatusEn,
ccf.Name_Ar as FileStatusAr



from CMS_EXECUTION ce
left join CMS_CASE_FILE_STATUS_G2G_LKP ccf on ce.FileStatusId=ccf.Id
GO
-----------------------------------pMeetingList
/****** Object:  StoredProcedure [dbo].[pMeetingList]    Script Date: 1/16/2023 5:39:49 PM ******/
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pMeetingList]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)

DROP PROCEDURE [dbo].[pMeetingList]
GO

/****** Object:  StoredProcedure [dbo].[pMeetingList]    Script Date: 1/16/2023 5:39:49 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- pMeetingList  
-- pMeetingList 'ldsreviewer2@fatwa.com'          
  
CREATE    PROCEDURE [dbo].[pMeetingList]      
 @UserName nvarchar(150) = NULL          
  
AS                        
BEGIN
SELECT CAST( row_number() OVER (ORDER BY MD.CreatedDate) as int) as SerialNo, 
MD.* FROM (
 SELECT  
 mt.CreatedDate,--CAST( row_number() OVER (ORDER BY mt.CreatedDate) as int) as SerialNo,             
  mt.MeetingId,              
  mt.[Subject],                 
  mty.NameEn as [TypeEn],                 
  mty.NameAr as [TypeAr],                 
  mt.[Location],                 
  mt.CreatedBy,            
  mts.MeetingStatusId,            
  mts.NameEn as [StatusEn],                 
  mts.NameAr as [StatusAr],    
  --CAST (cf.[FileNumber] as int) as [FileNumber],
  CASE WHEN ccf.FileNumber IS NULL THEN cf.FileNumber END as [FileNumber],       
  mt.[Date] as [DateTime]                
 FROM MEET_MEETING mt                
 LEFT JOIN CMS_CASE_FILE cf on cf.FileId = mt.FileId  
 LEFT JOIN COMS_CONSULTATION_FILE ccf on ccf.FileId = mt.FileId  
 INNER JOIN MEET_MEETING_TYPE mty on mty.MeetingTypeId = mt.MeetingTypeId                
 INNER JOIN MEET_MEETING_STATUS mts on mts.MeetingStatusId = mt.MeetingStatusId   
 INNER JOIN MEET_MEETING_ATTENDEE ma on ma.MeetingId = mt.MeetingId  
 LEFT JOIN UMS_USER usr on usr.FirstName_En = ma.RepresentativeName   
 WHERE mt.IsDeleted = 0    
 AND usr.UserName like '%'+ @UserName +'%' OR @UserName IS NULL      
   ) MD
 Group By MD.MeetingId,              
  MD.[Subject],                 
  MD.[TypeEn],                 
  MD.[TypeAr],                 
  MD.[Location],                 
  MD.CreatedBy,            
  MD.MeetingStatusId,            
  MD.[StatusEn],                 
  MD.[StatusAr],                 
  MD.[FileNumber],
  --CAST (MD.[FileNmber] as int), 
  --CAST (ccf.[FileNumber] as int), 
  MD.[DateTime],  
  MD.CreatedDate  
 ORDER BY MD.CreatedDate DESC     
   
END 
GO
-----------------------------------[pConsultationDetailById]
/****** Object:  StoredProcedure [dbo].[pConsultationDetailById]    Script Date: 1/16/2023 5:55:27 PM ******/
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pConsultationDetailById]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)

DROP PROCEDURE [dbo].[pConsultationDetailById]
GO

/****** Object:  StoredProcedure [dbo].[pConsultationDetailById]    Script Date: 1/16/2023 5:55:27 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE        PROCEDURE [dbo].[pConsultationDetailById]          
(          
@consultationId uniqueidentifier          
)          
As          
 BEGIN         
Select distinct  ccr.RequestNumber         
,ccr.ConsultationRequestId        
        , ccr.GEOpinion     
  , ccr.Introduction    
       , ccr.Remarks         
       , ccr. ContractAmount75000KD      
    , ccr.IsConfidential    
       , ccr. ReferenceNo         
       , ccr.  RequestDate         
         , ccr.ReferenceDate         
         , ccr.RequestTitle         
         , ccr.[Description]             
         , ccr.Subject         
        , pr.Name_En AS [priorityEn]          
      , pr.Name_Ar AS [priorityAr]          
      , ge.Name_En AS [gvtEntityEn]          
      , ge.Name_Ar AS [gvtEntityAr]          
      , cdf.Name_En AS [departmentEn]          
      , cdf.Name_Ar AS [departmentAr]          
      , sfl.Name_En AS [contractSubTypeEn]          
      , sfl.Name_Ar AS [contractSubTypeAr] 
	  ,ccr.RequestStatusId
      , crs.Name_En AS [requestStatusEn]          
      , crs.Name_Ar AS [requestStatusAr]   
	  ,ccr.RequestTypeId
   , rtf.Name_En AS [RequestTypeEn]      
   , rtf.Name_Ar AS [RequestTypeAr]  
    , ccr.SectorTypeId            
   , os.Name_En as SectorName_En            
   , os.Name_Ar as SectorName_Ar   
   ,ccr.TransferStatusId
             
from COMS_CONSULTATION_REQUEST ccr          
LEFT JOIN PRIORITY pr ON ccr.PriorityId = pr.Id          
LEFT JOIN CMS_GOVERNMENT_ENTITY_G2G_LKP ge ON ccr.GovtEntityId = ge.EntityId          
LEFT JOIN Department cdf ON ccr.DepartmentId = cdf.Id        
LEFT JOIN CMS_CASE_REQUEST_STATUS_G2G_LKP crs ON crs.Id = ccr.RequestStatusId         
LEFT JOIN CMS_REQUEST_TYPE_G2G_LKP rtf ON rtf.Id = ccr.RequestTypeId      
LEFT JOIN CMS_SUBTYPE_G2G_LKP sfl ON sfl.RequestTypeId = rtf.Id AND sfl.Id = ccr.RequestSubTypeId  
LEFT JOIN CMS_OPERATING_SECTOR_TYPE_G2G_LKP os ON os.Id = ccr.SectorTypeId                   

WHERE ccr.IsDeleted != 1            
and (ccr.ConsultationRequestId = @consultationId OR @consultationId IS NULL OR @consultationId = '00000000-0000-0000-0000-000000000000') 
END
GO
-----------------------------pConsultationPartyList
/****** Object:  StoredProcedure [dbo].[pConsultationPartyList]    Script Date: 1/17/2023 3:25:05 PM ******/
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pConsultationPartyList]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)

DROP PROCEDURE [dbo].[pConsultationPartyList]
GO

/****** Object:  StoredProcedure [dbo].[pConsultationPartyList]    Script Date: 1/17/2023 3:25:05 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE     PROCEDURE [dbo].[pConsultationPartyList]    
(    
@ConsultationId uniqueidentifier    
)    
As    
 BEGIN   
Select distinct  ccp.RepresentativeName    
    , ccp.ConsultationRequestId    
    , ccp.CivilID_CRN    
    , ccp.Designation   
    , cpt.Name_Ar AS[PartyTypeAr]    
    , cpt.Name_En AS[PartyTypeEn]    
    , ccp.IsDeleted    
from COMS_CONSULTATION_PARTY ccp    
LEFT JOIN COMS_CONSULTATION_PARTY_TYPE_G2G_LKP cpt ON cpt.Id = ccp.PartyTypeId    
WHERE ccp.IsDeleted != 1      
and (ccp.ConsultationRequestId = @ConsultationId OR @ConsultationId IS NULL OR @ConsultationId = '00000000-0000-0000-0000-000000000000')       
 END
GO
-----------------------pComsConsultationRequestList
/****** Object:  StoredProcedure [dbo].[pComsConsultationRequestList]    Script Date: 1/19/2023 1:04:08 PM ******/
if exists (select * from sysobjects where id = object_id('[dbo].[pComsConsultationRequestList]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)

DROP PROCEDURE [dbo].[pComsConsultationRequestList]
GO

/****** Object:  StoredProcedure [dbo].[pComsConsultationRequestList]    Script Date: 1/19/2023 1:04:08 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE       PROCEDURE [dbo].[pComsConsultationRequestList]    
(  
@requestNumber NVARCHAR(500) = null,  
@statusId int =null,  
@subject NVARCHAR(1000) =null,  
@requestFrom datetime=null,  
@requestTo datetime=null,  
@requestTypeId int = null ,
@sectorTypeId int=null,
@showUndefinedRequests bit = null  
)  
AS    
Begin    
Select distinct  ccr.ConsultationRequestId,  
 ccr.RequestNumber,  
 ccr.Subject,  
 ccr.OfficialLetterOutboxNumber,  
 ccr.FatwaInboxNumber,  
 ccr.RequestDate,  
--Status  
ccr.RequestStatusId,  
ccrs.Name_En as Status_Name_En,    
ccrs.Name_Ar as Status_Name_Ar,  
--RequestType  
ccr.RequestTypeId,  
coct.Name_En as RequestType_Name_En,  
coct.Name_Ar as RequestType_Name_Ar , 
 -------
 ccr.SectorTypeId,
 co.Name_En as SectorType_Name_En,                    
co.Name_Ar as SectorType_Name_Ar 
  
    
FROM COMS_CONSULTATION_REQUEST ccr    
left join CMS_CASE_REQUEST_STATUS_G2G_LKP ccrs on ccr.RequestStatusId = ccrs.Id    
left join CMS_REQUEST_TYPE_G2G_LKP coct on ccr.RequestTypeId = coct.Id  
left join CMS_OPERATING_SECTOR_TYPE_G2G_LKP co on ccr.SectorTypeId= co.Id                     

WHERE ccr.IsDeleted != 1  
AND (ccr.RequestTypeId=@RequestTypeId)   
AND (ccr.RequestNumber LIKE '%' + @requestNumber + '%' OR @requestNumber IS NULL OR @requestNumber='')  
AND (ccr.Subject LIKE '%' + @subject + '%' OR @subject IS NULL OR @subject='')     
AND (ccr.RequestStatusId=@statusId OR @statusId IS NULL OR @statusId='')  
AND(CAST(ccr.RequestDate as date)>=@requestFrom OR @requestFrom IS NULL OR @requestFrom='')  
AND (CAST(ccr.RequestDate as date)<=@requestTo OR @requestTo IS NULL OR @requestTo='')    
AND(ccr.SectorTypeId=@sectorTypeId OR @sectorTypeId IS NULL OR @sectorTypeId='0')                  
AND ((ccr.TransferStatusId = CASE WHEN @showUndefinedRequests = 0 THEN 2 ELSE 4 END) OR (@showUndefinedRequests = 0 AND ccr.TransferStatusId is null) )  
 
ORDER BY ccr.RequestDate desc  
END    
GO





   /****** Object:  StoredProcedure [dbo].[pAssigncourtToLawyer]    Script Date: 19/01/2023 2:38:56 pm ******/
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pAssigncourtToLawyer]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pAssigncourtToLawyer]
GO

Create   PROCEDURE [dbo].[pAssigncourtToLawyer]                   
(                
        
  @UserId nvarchar(150) = NULL,              
  @LawyerId nvarchar(150) = NULL            
      
)              
AS                            
BEGIN   

select DISTINCT clca.LawyerId,clca.Id,
--user
us.FirstName_En as LawyerFirstNameEn,
us.SecondName_Ar as LawyerFirstNameAr,
us.FirstName_Ar +' ' + us.SecondName_Ar as LawyerFullNameAr,
us.FirstName_En+' ' + us.SecondName_En as LawyerFullNameEn,
--Court Type
crt.Name_Ar as CourtTypeAr,
crt.Name_En as CourtTypeEn,
--Court  
c.Name_Ar as CourtNameAr,
c.Name_En as CourtNameEn,
--Chamber 
ch.Name_Ar as ChamberNameAr,
ch.Name_En as  ChamberNameEn,
ch.Number as ChamberNumber

from CMS_LAWYER_COURT_ASSIGNMENT clca 
LEFT JOIN UMS_USER us on us.Id=clca.LawyerId
LEFT JOIN CMS_COURT_TYPE_FTW_LKP crt on crt.Id=clca.CourtTypeId
LEFT JOIN CMS_COURT_FTW_LKP c on c.Id=clca.CourtId
LEFT JOIN CMS_CHAMBER_FTW_LKP ch on ch.Id=clca.ChamberId
where clca.IsDeleted=0
--And (clca.LawyerId = @UserId OR @UserId IS NULL OR @UserId='00000000-0000-0000-0000-000000000000')   
order by clca.LawyerId
END 

/****** Object:  StoredProcedure [dbo].[pLawyerListBySector]    Script Date: 12/01/2023 2:38:56 pm ******/
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pLawyerListBySector]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pLawyerListBySector]
GO

-- [dbo].[pLawyerListBySector] 2
CREATE OR ALTER Procedure [dbo].[pLawyerListBySector]  
(      
	@sectorTypeId INT      
)      
AS   
BEGIN   

 Select us.Id           
  , us.UserName          
  , us.Email     
  , COUNT(tsk.AssignedTo) as TotalTasks
  , MAX(tsk.CreatedDate) as LastActivityDate
 from UMS_USER us          
 INNER JOIN Department dep ON us.DepartmentId = dep.Id          
 LEFT JOIN UMS_USER_TYPE ut ON us.UserTypeId = ut.Id    
 LEFT JOIN TSK_TASK tsk ON tsk.AssignedTo = us.Id AND tsk.IsDeleted = 0 AND tsk.TaskStatusId = 1 --Pending  
 WHERE us.IsDeleted = 0       
 And us.SectorTypeId = @sectorTypeId      
 GROUP BY us.Id           
  , us.UserName          
  , us.Email   
  , us.CreatedDate  
 ORDER BY us.CreatedDate desc  

 END
 -------------------------------------------------[pBorrowDetailSelAll]
 /****** Object:  StoredProcedure [dbo].[pBorrowDetailSelAll]    Script Date: 1/21/2023 11:37:23 AM ******/
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pBorrowDetailSelAll]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
    DROP PROCEDURE [dbo].[pBorrowDetailSelAll]
GO

/****** Object:  StoredProcedure [dbo].[pBorrowDetailSelAll]    Script Date: 1/21/2023 11:37:23 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure [dbo].[pBorrowDetailSelAll]                
 @userId nvarchar(50) = NULL          
AS                 
 SELECT DISTINCT bd.LiteratureId         
			, ld.[Name] AS LiteratureName      
			, bd.BorrowId            
			, bd.DueDate            
			, bd.ExtendDueDate            
			, bd.Extended            
			, bd.Fine            
			, bd.IssueDate            
			, bd.ReturnDate            
			, bd.UserId            
			, bd.ExtensionApprovalStatus            
			, bd.Comment          
			, bd.CreatedBy            
			, bd.CreatedDate            
			, bd.ModifiedBy            
			, bd.ModifiedDate            
			, bd.DeletedBy  
			, bd.DeletedDate            
			, bd.IsDeleted             
			, ld.ISBN             
			, us.UserName As UserName            
			, us.EligibleCount        
			, lt.Name_En As [TypeName_En]      
			, lt.Name_Ar As [TypeName_Ar]      
			, CONCAT(us.FirstName_En, ' ', us.LastName_En) AS FullName_En      
			, CONCAT(us.FirstName_Ar, ' ', us.LastName_Ar) AS FullName_Ar      
			, us.PhoneNumber as PhoneNumber           
			, bas.[Name] as  BorrowApprovalStatus    
			,bas.[Name_Ar] as BorrowApprovalStatusAr    
	 FROM LMS_LITERATURE_BORROW_DETAILS bd                
	 INNER JOIN LMS_LITERATURE_DETAILS ld ON bd.LiteratureId = ld.LiteratureId          
	 INNER JOIN LMS_LITERATURE_TYPE lt ON lt.TypeId = ld.TypeId      
	 INNER JOIN LMS_LITERATURE_BORROW_APPROVAL_STATUS bas ON bas.DecisionId = bd.BorrowApprovalStatus              
	 INNER JOIN UMS_USER us ON bd.UserId = us.Id              
	 Where bd.IsDeleted != 1            
	 AND (bd.UserId = @userId OR @userId IS NULL OR @userId='')            
	 ORDER BY bd.BorrowId desc              
   SET ROWCOUNT 0                
   SET NOCOUNT OFF                
 RETURN    
  
GO
-------------------------------pLiteratureBorrowApprovalSelAll

-- [dbo].[pLiteratureBorrowApprovalSelAll]
if exists (select * from sysobjects where id = object_id('[dbo].[pLiteratureBorrowApprovalSelAll]') and OBJECTPROPERTY(id, 'IsProcedure') = 1) 
    DROP PROCEDURE [dbo].[pLiteratureBorrowApprovalSelAll]
GO

/****** Object:  StoredProcedure [dbo].[pLiteratureBorrowApprovalSelAll]    Script Date: 1/21/2023 1:18:58 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure [dbo].[pLiteratureBorrowApprovalSelAll]        
AS         
 SELECT DISTINCT bd.LiteratureId    
		, bd.BorrowId    
		, bd.DueDate    
		, bd.ExtendDueDate    
		, bd.Extended    
		, bd.Fine    
		, bd.IssueDate    
		, bd.ReturnDate    
		, bd.UserId    
		, bd.ExtensionApprovalStatus    
		, bd.Comment    
		, bd.CreatedBy    
		, bas.[Name] as BorrowApprovalStatus    
		, bas.[Name] as BorrowApprovalStatusAr    
		, bd.CreatedDate    
		, bd.ModifiedBy    
		, bd.ModifiedDate    
		, bd.DeletedBy    
		, bd.DeletedDate    
		, bd.IsDeleted    
		, ld.ISBN    
		, lt.Name_En As [TypeName_En]    
		, lt.Name_Ar As [TypeName_Ar]    
		, ld.Name As LiteratureName    
		, us.UserName  As UserName    
		, us.EligibleCount    
		, us.PhoneNumber as PhoneNumber    
		, CONCAT(us.FirstName_En, ' ', us.LastName_En) AS FullName_En    
		, CONCAT(us.FirstName_Ar, ' ', us.LastName_Ar) AS FullName_Ar    
	FROM LMS_LITERATURE_BORROW_DETAILS bd      
	INNER JOIN LMS_LITERATURE_BORROW_APPROVAL_STATUS bas ON bas.DecisionId = bd.BorrowApprovalStatus     
	LEFT JOIN LMS_LITERATURE_DETAILS ld ON bd.LiteratureId=ld.LiteratureId     
	INNER JOIN LMS_LITERATURE_TYPE lt ON lt.TypeId = ld.TypeId    
	INNER JOIN UMS_USER us ON bd.UserId=us.Id      
	WHERE bd.BorrowApprovalStatus = 8 --Pending    
	AND bd.IsDeleted != 1    
  ORDER BY bd.BorrowId desc      
  SET ROWCOUNT 0        
  SET NOCOUNT OFF        
 RETURN    
GO


-----------------------------------------pBorroweBorrowExtensionApprovalSelAll
/****** Object:  StoredProcedure [dbo].[pBorroweBorrowExtensionApprovalSelAll]    Script Date: 1/21/2023 12:42:31 PM ******/
if exists (select * from sysobjects where id = object_id('[dbo].[pBorroweBorrowExtensionApprovalSelAll]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
    DROP PROCEDURE [dbo].[pBorroweBorrowExtensionApprovalSelAll]
GO

/****** Object:  StoredProcedure [dbo].[pBorroweBorrowExtensionApprovalSelAll]    Script Date: 1/21/2023 12:42:31 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure [dbo].[pBorroweBorrowExtensionApprovalSelAll]            
AS             
 SELECT DISTINCT             
		bd.LiteratureId         
	  , bd.BorrowId        
	  , bd.DueDate        
	  , bd.ExtendDueDate        
	  , bd.Extended        
	  , bd.Fine        
	  , bd.IssueDate        
	  , bd.ReturnDate        
	  , bd.UserId        
	  , bs.[Name] as BorrowApprovalStatus   
	  , bs.[Name] as BorrowApprovalStatusAr    
	  , bd.Comment        
	  , bd.CreatedBy        
	  , bd.CreatedDate        
	  , bd.ModifiedBy        
	  , bd.ModifiedDate        
	  , bd.DeletedBy        
	  , bd.DeletedDate        
	  , bd.IsDeleted        
	  , ld.ISBN       
	  , lt.Name_En As [TypeName_En]    
	  , lt.Name_Ar As [TypeName_Ar]    
	  , CONCAT(us.FirstName_En, ' ', us.LastName_En) AS FullName_En    
	  , CONCAT(us.FirstName_Ar, ' ', us.LastName_Ar) AS FullName_Ar    
	  , ld.[Name] As LiteratureName        
	  , us.UserName  As UserName        
	  , us.EligibleCount        
	  , us.PhoneNumber as PhoneNumber        
  FROM LMS_LITERATURE_BORROW_DETAILS bd            
  INNER JOIN LMS_LITERATURE_DETAILS ld ON bd.LiteratureId = ld.LiteratureId      
  INNER JOIN LMS_LITERATURE_TYPE lt ON lt.TypeId = ld.TypeId    
  INNER JOIN LMS_LITERATURE_BORROW_APPROVAL_STATUS bs on bd.BorrowApprovalStatus = bs.DecisionId      
  INNER JOIN UMS_USER us ON bd.UserId=us.Id          
  WHERE bd.IsDeleted = 0 AND bd.Extended = 1       
  ORDER BY bd.BorrowId desc          
  SET ROWCOUNT 0            
  SET NOCOUNT OFF            
 RETURN
GO




