/****** Object:  StoredProcedure [dbo].[pLiteratureSelAll]    Script Date: 10/3/2022 3:07:48 PM ******/
DROP PROCEDURE [dbo].[pLiteratureSelAll]
GO

/****** Object:  StoredProcedure [dbo].[pLiteratureSelAll]    Script Date: 10/3/2022 3:07:48 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
   
CREATE Procedure [dbo].[pLiteratureSelAll]                 
AS             
Select distinct ld.LiteratureId       
     , ld.ISBN        
     , ld.[Name] as LiteratureName        
     , lt.Name_En as LiteratureType        
     , la.FirstName_En as AuthorName        
     , li.IndexNumber as IndexNumber        
     , lp.[Date] as PurchaseDate        
     , ld.DeletedBy as DeletedBy          
 , CASE        
   WHEN ld.IsBorrowable = 0 THEN 'UnAvailable'      
   --WHEN ld.CopyCount = (Select Count(*) as Borrowable From LMS_LITERATURE_BARCODE where LiteratureId = ld.LiteratureId and Active = 1  and IsBorrowed = 1 and IsDeleted = 0) THEN 'UnAvailable'        
       
   WHEN lbd.BorrowApprovalStatus = 16 THEN (Select CONCAT('Borrowed: ', Count(BorrowApprovalStatus)) AS Result  From LMS_LITERATURE_BORROW_DETAILS where BorrowApprovalStatus = 16)         
   WHEN lbd.BorrowApprovalStatus = 1 THEN (Select CONCAT('Reserved: ', Count(BorrowApprovalStatus)) AS Result  From LMS_LITERATURE_BORROW_DETAILS where BorrowApprovalStatus = 1)          
  ELSE  CONCAT('Borrowable: ', CONVERT(nvarchar,  (Select Count(*) as Borrowable From LMS_LITERATURE_BARCODE where LiteratureId = ld.LiteratureId and Active = 1  and IsBorrowed = 0 and IsDeleted = 0) ))          
 END AS BookStatus        
 from LMS_LITERATURE_DETAILS ld        
 LEFT JOIN LMS_LITERATURE_BORROW_DETAILS lbd ON ld.LiteratureId = lbd.LiteratureId         
 LEFT JOIN LMS_LITERATURE_TYPE lt ON ld.TypeId = lt.TypeId        
 LEFT JOIN LMS_LITERATURE_DETAILS_LMS_LITERATURE_AUTHOR lda ON ld.LiteratureId = lda.LiteratureId         
 LEFT JOIN LMS_LITERATURE_AUTHOR la ON lda.AuthorId = la.AuthorId          
 LEFT JOIN LMS_LITERATURE_INDEX li ON ld.IndexId = li.IndexId        
 LEFT JOIN LMS_LITERATURE_PURCHASE lp ON ld.LiteratureId = lp.LiteratureId        
 Where ld.IsDeleted != 1         
 ORDER BY ld.LiteratureId desc          
 SET ROWCOUNT 0            
 SET NOCOUNT OFF            
 RETURN 
GO
 
GO
/****** Object:  StoredProcedure [dbo].[pLiteratureSelBySearchTerm]    Script Date: 10/3/2022 3:10:26 PM ******/
SET ANSI_NULLS ON
GO 
SET QUOTED_IDENTIFIER ON
GO 
-- [dbo].[pLiteratureSelBySearchTerm] '', 'en-US'    
-- [dbo].[pLiteratureSelBySearchTerm] '', 'ar-KW'
if exists (select * from sysobjects where id = object_id('[pLiteratureSelBySearchTerm]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pBellNotifications]
GO
CREATE PROCEDURE [dbo].[pBellNotifications]   
	@UserId nvarchar(50) = NULL    
AS        
BEGIN         
	SELECT DISTINCT nf.NotificationId 
					, nfu.[NotificationMessage]     
					, nl.[URL] as [Url]
					, nf.CreatedDate as CreationDate   
	FROM NOTIF_NOTIFICATION_USER nfu
	INNER JOIN NOTIF_NOTIFICATION nf on nf.NotificationId = nfu.NotificationId
	LEFT JOIN NOTIF_NOTIFICATION_TEMPLATE nft on nft.TemplateId = nf.NotificationTemplateId  
	INNER JOIN NOTIF_NOTIFICATION_LINK nl ON nl.LinkId = nf.NotificationLinkId       
	WHERE nf.IsDeleted = 0
	AND nfu.NotificationStatusId = 2 -- Unread
	AND nf.NotificationCommunicationMethodId = 4 --'Browser'   
	AND (nf.ReceiverId = @UserId OR @UserId IS NULL OR @UserId = '')  
	ORDER BY nf.CreatedDate Desc   
END 
GO
 
GO
/****** Object:  StoredProcedure [dbo].[pBellNotifications]    Script Date: 21/07/2022 9:51:24 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- pBellNotifications '436e82d2-70d8-455c-a643-7909b8689667'     

if exists (select * from sysobjects where id = object_id('[dbo].[pBellNotifications]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pBellNotifications]
GO
CREATE PROCEDURE [dbo].[pBellNotifications]   
	@UserId nvarchar(50) = NULL    
AS        
BEGIN         
	SELECT DISTINCT nf.NotificationId
					, nft.SubjectAr
					, nft.SubjectEn
					, nfu.[NotificationMessage]     
					, nl.[URL] as [Url]
					, nf.CreatedDate as CreationDate   
	FROM NOTIF_NOTIFICATION_USER nfu
	INNER JOIN NOTIF_NOTIFICATION nf on nf.NotificationId = nfu.NotificationId
	INNER JOIN NOTIF_NOTIFICATION_TEMPLATE nft on nft.TemplateId = nf.NotificationTemplateId  
	INNER JOIN NOTIF_NOTIFICATION_LINK nl ON nl.LinkId = nf.NotificationLinkId       
	WHERE nf.IsDeleted = 0
	AND nfu.NotificationStatusId = 2 -- Unread
	AND nf.NotificationCommunicationMethodId = 4 --'Browser'   
	AND (nf.ReceiverId = @UserId OR @UserId IS NULL OR @UserId = '')  
	ORDER BY nf.CreatedDate Desc   
END 
GO
PRINT 'Created [pBellNotifications]'
GO

----- 

GO
/****** Object:  StoredProcedure [dbo].[pNotificationDetailView]    Script Date: 21/07/2022 9:51:24 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO 

-- [dbo].[pNotificationdetailView] '20f43e62-cdfc-411f-bb76-0d6988340567', '436e82d2-70d8-455c-a643-7909b8689667'        
-- [dbo].[pNotificationdetailView] '3fa85f64-5717-4562-b3fc-2c963f66afe6', '436e82d2-70d8-455c-a643-7909b8689667'        
if exists (select * from sysobjects where id = object_id('[dbo].[pNotificationDetailView]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pNotificationdetailView]
GO     
CREATE PROCEDURE [dbo].[pNotificationdetailView]          
(                      
	@NotificationId uniqueidentifier = NULL,       
	@UserId [nvarchar](150) = NULL        
)        
AS          
BEGIN        
	 SELECT nf.NotificationId
	 , ne.[NameEn] as EventNameEn     
	 , ne.[NameAr] as EventNameAr    
	 , nu.[NotificationMessage]        
	 , nl.[Url] as [Url]    
	 , nl.isDeleted as LinkIsDeleted         
	 , nf.CreatedDate        
	 , uu.FirstName_Ar + ' ' + uu.LastName_Ar as CreatedByAr        
	 , uu.FirstName_En + ' ' + uu.LastName_En as CreatedByEn        
	 , ne.[NameAr] + N' إلى عن على ' + md.ModuleNameAr as Title_Ar        
	 , ne.[NameEn] + ' For ' + md.ModuleNameEn as Title_En         
	 , nt.SubjectAr as SubjectAr        
	 , nt.SubjectEn as SubjectEn        
	 , nt.Body as Body        
	 , nc.Color as Color  
	 , md.ModuleNameEn as ModuleName_En  
	 , md.ModuleNameAr as ModuleName_Ar  
	 FROM NOTIF_NOTIFICATION nf          
	 LEFT JOIN NOTIF_NOTIFICATION_TEMPLATE nt ON nt.TemplateId = nf.NotificationTemplateId        
	 INNER JOIN NOTIF_NOTIFICATION_EVENT ne ON ne.EventId = nf.NotificationEventId        
	 INNER JOIN NOTIF_NOTIFICATION_LINK nl ON nl.LinkId=nf.NotificationLinkId        
	 INNER JOIN NOTIF_NOTIFICATION_CATEGORY nc ON nc.CategoryId = nf.NotificationCategoryId         
	 INNER JOIN NOTIF_NOTIFICATION_USER nu ON nu.NotificationId = nf.NotificationId         
	 INNER JOIN UMS_USER uu ON uu.Id = nu.UserId        
	 INNER JOIN MODULE md ON md.ModuleId = nf.ModuleId     
	 WHERE nf.IsDeleted != 1           
	 AND (nf.NotificationId = @NotificationId OR @NotificationId IS NULL)            
	 --AND(nf.ReceiverId = @UserId OR @UserId IS NULL)        
	 --AND (uu.Id = @UserId OR @UserId IS NULL OR @UserId = '')                                
END 
GO
PRINT 'Created [pNotificationDetailView]'
GO

-----

GO
/****** Object:  StoredProcedure [dbo].[pNotificationList]    Script Date: 21/07/2022 9:51:24 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- [dbo].[pNotificationList] '3FA85F64-5717-4562-B3FC-2C963F66AFE6', '436e82d2-70d8-455c-a643-7909b8689667'   
if exists (select * from sysobjects where id = object_id('[dbo].[pNotificationList]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pNotificationList]
GO  
Create PROCEDURE [dbo].[pNotificationList]            
	@UserId nvarchar(50) = NULL       
AS            
BEGIN              
 Select DISTINCT nf.NotificationId      
	, nf.DueDate       
	, nf.CreatedBy      
	, nf.CreatedDate   
	, nu.ReadDate    
	, nf.DeletedBy      
	, nf.DeletedDate      
	, nf.ReceiverId      
	, tr.Value_Ar as SubjectAr          
	, tr.Value_En as SubjectEn     
	, nfl.[Url] as NotificationLink
	, ne.[NameEn] as EventNameEn       
	, ne.[NameAr] as EventNameAr         
	, nc.[NameEn] as CatNameEn      
	, nc.[NameAr] as CatNameAr  
	, nf.SenderId    
	, md.ModuleNameAr  
	, md.ModuleNameEn  
	, nu.NotificationStatusId as NotificationStatus  
	FROM NOTIF_NOTIFICATION nf      
	INNER JOIN NOTIF_NOTIFICATION_EVENT ne ON ne.EventId = nf.NotificationEventId         
	INNER JOIN NOTIF_NOTIFICATION_LINK nfl ON nfl.LinkId = nf.NotificationLinkId
	INNER JOIN NOTIF_NOTIFICATION_CATEGORY nc ON nc.CategoryId = nf.NotificationCategoryId          
	INNER JOIN NOTIF_NOTIFICATION_USER nu ON nu.NotificationId = nf.NotificationId  
	INNER JOIN tTranslation tr ON tr.TranslationKey = nu.NotificationMessage 
	INNER JOIN MODULE md on md.ModuleId = nf.ModuleId  
	WHERE nf.IsDeleted = 0            
	AND (nu.NotificationStatusId =  1 OR nu.NotificationStatusId =  2 ) -- Read AND Unread      
	AND nf.NotificationCommunicationMethodId = 4 --'Browser'       
	AND nf.ReceiverId = @UserId        
	ORDER BY nu.NotificationStatusId Desc, nf.CreatedDate Desc
END   
GO
PRINT 'Created [pNotificationList]'
GO

-- Nadia Gull
/****** Object:  StoredProcedure [dbo].[pLegislationListFiltered]   ******/
GO

/****** Object:  StoredProcedure [dbo].[pLegislationListFiltered]    Script Date: 28/11/2022 11:38:40 am ******/
DROP PROCEDURE [dbo].[pLegislationListFiltered]
GO

/****** Object:  StoredProcedure [dbo].[pLegislationListFiltered]    Script Date: 28/11/2022 11:38:40 am ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE    PROCEDURE [dbo].[pLegislationListFiltered]    
(  
@legislation_Number NVARCHAR(1000) = null,  
@legislation_Type int = null,  
@introduction NVARCHAR(1000) = null,  
@legislation_Title NVARCHAR(1000) = null,  
@legislation_Status int = null,  
@start_From datetime=null,  
@end_To datetime=null  
)  
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
WHERE ll.IsDeleted = 0    
AND (ll.Legislation_Number = @legislation_Number OR @legislation_Number IS NULL OR @legislation_Number ='')  
AND (ll.Legislation_Type = @legislation_Type OR @legislation_Type IS NULL OR @legislation_Type = '')  
AND (ll.Introduction = @introduction OR @introduction IS NULL OR @introduction ='')  
AND (ll.LegislationTitle = @legislation_Title OR @legislation_Title IS NULL OR @legislation_Title ='')  
AND (ll.Legislation_Status = @legislation_Status OR @legislation_Status IS NULL OR @legislation_Status = '')  
AND (CAST(ll.StartDate as date)>=@start_From OR @start_From IS NULL OR @start_From='')  
AND (CAST(ll.StartDate as date)<=@end_To OR @end_To IS NULL OR @end_To='')  
ORDER BY ll.AddedDate DESC           
SET ROWCOUNT 0            
SET NOCOUNT OFF            
RETURN             
END
GO
PRINT 'pLegislationListFiltered'
GO
-- Nadia Gull
/****** Object:  StoredProcedure [dbo].[pLiteratureListFiltered]  ******/
if exists (select * from sysobjects where id = object_id('[dbo].[pLiteratureListFiltered]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pLiteratureListFiltered]
GO
/****** Object:  StoredProcedure [dbo].[pLegislationListFiltered]   ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[pLiteratureListFiltered] 
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
AS     
Select distinct ld.LiteratureId    
     , lt.Name_En as LiteratureType_En
	 , lt.Name_Ar as LiteratureType_Ar
     , ld.ISBN  
     , ld.ClassificationId  
     , ld.Name as LiteratureName  
     , la.FirstName_Ar AuthorName  
     , li.IndexNumber  
     , lida.DivisionNumber
     , lida.AisleNumber  
 , CASE  
   WHEN ld.IsBorrowable = 0 THEN 'UnAvailable'  
   WHEN lbd.BorrowApprovalStatus = 16 THEN (Select CONCAT('Borrowed: ', Count(BorrowApprovalStatus)) AS Result  From LMS_LITERATURE_BORROW_DETAILS where BorrowApprovalStatus = 16)   
   WHEN lbd.BorrowApprovalStatus = 1 THEN (Select CONCAT('Reserved: ', Count(BorrowApprovalStatus)) AS Result  From LMS_LITERATURE_BORROW_DETAILS where BorrowApprovalStatus = 1)    
  ELSE  CONCAT('Borrowable: ', CONVERT(nvarchar, (ld.CopyCount - ld.NumberOfBorrowedBooks)))    
 END AS BookStatus 
, CAST(lp.[Date] as date) as PurchaseDate 
, '' as DeletedBy  
 from LMS_LITERATURE_DETAILS ld   
 LEFT JOIN LMS_LITERATURE_TYPE lt ON ld.TypeId = lt.TypeId   
 LEFT JOIN LMS_LITERATURE_DETAILS_LMS_LITERATURE_AUTHOR lda ON ld.LiteratureId = lda.LiteratureId   
 LEFT JOIN LMS_LITERATURE_AUTHOR la ON lda.AuthorId = la.AuthorId     
 LEFT JOIN LMS_LITERATURE_INDEX li ON ld.IndexId = li.IndexId   
 LEFT JOIN LMS_LITERATURE_INDEX_DIVISION_AISLE lida ON ld.DivisionAisleId = lida.DivisionAisleId   
 LEFT JOIN LMS_LITERATURE_PURCHASE lp ON ld.LiteratureId = lp.LiteratureId   
 LEFT JOIN LMS_LITERATURE_BARCODE lb ON ld.LiteratureId = lb.LiteratureId   
 LEFT JOIN LMS_LITERATURE_BORROW_DETAILS lbd ON ld.LiteratureId = lbd.LiteratureId   
 Where ld.IsDeleted != 1   
 AND (ld.[Name] like '%' + @bookName + '%' OR @bookName IS NULL OR @bookName='')   
 AND (ld.ClassificationId = @classificationId OR @classificationId IS NULL OR @classificationId='0')   
 AND (la.FirstName_Ar LIKE '%' + @authorName + '%' OR @authorName IS NULL OR @authorName='')   
 AND (li.Name_Ar LIKE '%' + @indexName + '%' OR @indexName IS NULL OR @indexName='')   
 AND (li.IndexNumber = @indexNumber OR @indexNumber IS NULL OR @indexNumber='')   
 AND (lida.DivisionNumber = @divisionNumber OR @divisionNumber IS NULL OR @divisionNumber='')   
 AND (lida.AisleNumber = @aisleNumber OR @aisleNumber IS NULL OR @aisleNumber='')   
 AND (lb.BarCodeNumber = @barcode OR @barcode IS NULL OR @barcode='')   
 AND (ld.Characters = @character OR @character IS NULL OR @character='')   
 AND (lp.Price = @price OR @price IS NULL OR @price='')   
 AND ( (CAST(lp.Date AS DATE) = @purchaseDate) OR @purchaseDate IS NULL OR @purchaseDate='')   
 AND ( (CAST(ld.CreatedDate AS DATE) >= @From) Or @From Is Null OR @From='')   
 AND ( (CAST(ld.CreatedDate AS DATE) <= @To) OR @To Is Null OR @To='')   
 ORDER BY ld.LiteratureId desc  
SET ROWCOUNT 0    
SET NOCOUNT OFF    
 RETURN
GO
  PRINT 'pLiteratureListFiltered'
GO

GO

/****** Object:  StoredProcedure [dbo].[pLegalLegislationDecision]    Script Date: 28/11/2022 11:43:31 am ******/
DROP PROCEDURE [dbo].[pLegalLegislationDecision]
GO

/****** Object:  StoredProcedure [dbo].[pLegalLegislationDecision]    Script Date: 28/11/2022 11:43:31 am ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE       PROCEDURE [dbo].[pLegalLegislationDecision]    
@LegislationId uniqueidentifier = NULL  
AS    
Begin    
Select distinct ll.LegislationId    
  , lt.Name_Ar as Legislation_Type_Ar  
  , lt.Name_En as Legislation_Type_En  
  , ll.Legislation_Number  
  , ll.IssueDate  
  , ll.Introduction  
  , ll.LegislationTitle as Legislation_TitleEn   
  , ll.IssueDate_Hijri  
  , ll.Legislation_Comment  
  ,ll.AddedBy  
  , ls.Name_Ar as Legislation_Status_Ar   
  , ls.Name_En as Legislation_Status_En  
  , ls.Id as StatusId  
    , ls.Name_Ar as LegislationFlowStatusAr   
  , ls.Name_En as LegislationFlowStatusEn  
  , ls.Id as FlowStatusId  

from LEGAL_LEGISLATION ll   
LEFT JOIN LEGAL_LEGISLATION_TYPE lt ON ll.Legislation_Type = lt.Id            
LEFT JOIN LEGAL_LEGISLATION_STATUS ls ON ll.Legislation_Status = ls.Id  
LEFT JOIN LEGAL_LEGISLATION_FLOW_STATUS fs ON ll.Legislation_Flow_Status = ls.Id    

WHERE ll.LegislationId=@LegislationId  
END  
GO



GO

/****** Object:  StoredProcedure [dbo].[pLLRelationAdvanceSearch]    Script Date: 28/11/2022 11:35:05 am ******/
DROP PROCEDURE [dbo].[pLLRelationAdvanceSearch]
GO

/****** Object:  StoredProcedure [dbo].[pLLRelationAdvanceSearch]    Script Date: 28/11/2022 11:35:05 am ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- [dbo].[pLLRelationAdvanceSearch] null, null, null, '2022/09/12 00:00', null, 0        
CREATE   Procedure [dbo].[pLLRelationAdvanceSearch]                       
	@LegislationType int = NULL                 
  , @LegislationNumber NVARCHAR(150)= NUll                  
  , @LegislationYear int = NULL  
AS                           
 Select ll.LegislationId as LegislationId 
	, ll.LegislationTitle as LegislationTitleEn
	, ll.Legislation_Number as Legislation_Number
	, YEAR(ll.IssueDate) as Legislation_Year
	, ll.Legislation_Status as Legislation_Status
	, ll.IssueDate as LegislationIssueDate
	, llt.Id as Legislation_Type
	, llt.Name_En as Legislation_Type_Name_En                      
	, llt.Name_Ar as Legislation_Type_Name_Ar
	, lls.Name_En as LegislationStatusNameEn
	, lls.Name_Ar as LegislationStatusNameAr
  from LEGAL_LEGISLATION ll                      
  LEFT JOIN LEGAL_LEGISLATION_TYPE llt ON ll.Legislation_Type = llt.Id
  LEFT JOIN LEGAL_LEGISLATION_STATUS lls ON ll.Legislation_Status = lls.Id
  Where ll.IsDeleted != 1                      
  AND (ll.Legislation_Type = @LegislationType or @LegislationType is Null OR @LegislationType = '')  
  AND (ll.Legislation_Number like '%' + @LegislationNumber + '%' or @LegislationNumber is Null OR @LegislationNumber like '')
  AND ((YEAR(ll.IssueDate) = @LegislationYear) Or @LegislationYear Is Null OR @LegislationYear = '') 
   
  ORDER BY ll.AddedDate desc
 SET ROWCOUNT 0                          
 SET NOCOUNT OFF                          
 RETURN   
GO

 -- Nadia Gull --
/****** Object:  StoredProcedure [dbo].[pLegislationListFiltered]   ******/
if exists (select * from sysobjects where id = object_id('[dbo].[pUserBorrowLiteraturesByUserId]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pUserBorrowLiteraturesByUserId]

/****** Object:  StoredProcedure [dbo].[pUserBorrowLiteraturesByUserId]   ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[pUserBorrowLiteraturesByUserId]  
(
@searchTerm nvarchar(50) = NULL
)
AS
Begin  
Select distinct la.BorrowId
		, lld.Name as LiteratureName
		, llb.BarCodeNumber 
		, la.IssueDate 
from LMS_LITERATURE_BORROW_DETAILS la
LEFT JOIN LMS_LITERATURE_DETAILS lld ON la.LiteratureId = lld.LiteratureId
LEFT JOIN LMS_LITERATURE_BARCODE llb ON la.BarcodeId = llb.BarcodeId
WHERE la.IsDeleted = 0  
AND (la.UserId = @searchTerm)
AND (la.ReturnDate IS NULL)
Order By la.IssueDate        
SET ROWCOUNT 0          
SET NOCOUNT OFF          
RETURN           
END 
GO
PRINT 'pUserBorrowLiteraturesByUserId'

 -- Nadia Gull --

 /****** Object:  StoredProcedure [dbo].[pPrincipleListFiltered]    Script Date: 11/17/2022 4:48:49 PM ******/
DROP PROCEDURE [dbo].[pPrincipleListFiltered]
GO

/****** Object:  StoredProcedure [dbo].[pPrincipleListFiltered]    Script Date: 11/17/2022 4:48:49 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


Create    PROCEDURE [dbo].[pPrincipleListFiltered]      
(    
@Principle_Number NVARCHAR(1000) = null,    
@Principle_Type int = null,    
@introduction NVARCHAR(1000) = null,    
@PrincipleTitle  NVARCHAR(1000) = null,    
@Principle_Status int = null,    
@start_From datetime=null,    
@end_To datetime=null,    
@BaseNumber NVARCHAR(1000)=null,
@AddedBy NVARCHAR(1000)=null
)    
AS      
Begin      
Select distinct ll.PrincipleId      
  , ll.Principle_Type     
  , ll.Principle_Number     
  , ll.Introduction        
  , ll.IssueDate    
  , YEAR(ll.IssueDate) as Principle_Year  
  , ll.IssueDate_Hijri        
  , ll.PrincipleTitle      
  , ll.Principle_Status     
  , ll.Principle_Flow_Status    
  , ll.StartDate        
  , ll.AddedBy      
  , ll.AddedDate As CreatedDate     
  , ll.ModifiedBy        
  , ll.ModifiedDate    
  ,ll.BaseNumber  
  , llt.Name_Ar As Principle_Type_Ar        
  , llt.Name_En As Principle_Type_En         
  , lls.Name_Ar As Principle_Status_Ar     
  , lls.Name_En As Principle_Status_En    
  , llfs.Name_Ar As Principle_Flow_Status_Ar    
  , llfs.Name_En As Principle_Flow_Status_En    
    
from LEGAL_Principle ll              
LEFT JOIN LEGAL_Principle_TYPE llt ON ll.Principle_Type = llt.Id    
LEFT JOIN LEGAL_Principle_FLOW_STATUS llfs ON ll.Principle_Flow_Status = llfs.Id     
LEFT JOIN LEGAL_Principle_STATUS lls ON ll.Principle_Status = lls.Id      
WHERE ll.IsDeleted = 0      
AND (ll.Principle_Number = @Principle_Number OR @Principle_Number IS NULL OR @Principle_Number ='')    
AND (ll.Principle_Type = @Principle_Type OR @Principle_Type IS NULL OR @Principle_Type = '')  
AND (ll.BaseNumber=@BaseNumber OR @BaseNumber IS NULL OR @BaseNumber='')
AND(ll.AddedBy=@AddedBy OR @AddedBy IS NULL OR @AddedBy='')  
AND (ll.PrincipleTitle = @PrincipleTitle  OR ll.PrincipleTitle = @PrincipleTitle  OR @PrincipleTitle  IS NULL OR @PrincipleTitle  ='')    
AND (ll.Principle_Status = @Principle_Status OR @Principle_Status IS NULL OR @Principle_Status = '')    
AND (CAST(ll.StartDate as date)>=@start_From OR @start_From IS NULL OR @start_From='')    
  
ORDER BY ll.AddedDate DESC             
SET ROWCOUNT 0              
SET NOCOUNT OFF              
RETURN               
END  
GO




/****** Object:  StoredProcedure [dbo].[pGetLegalPrincipleDecisionbyPrincipleId]    Script Date: 11/28/2022 6:50:46 PM ******/
DROP PROCEDURE [dbo].[pGetLegalPrincipleDecisionbyPrincipleId]
GO

/****** Object:  StoredProcedure [dbo].[pGetLegalPrincipleDecisionbyPrincipleId]    Script Date: 11/28/2022 6:50:46 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE  PROCEDURE [dbo].[pGetLegalPrincipleDecisionbyPrincipleId]      
@PrincipleId uniqueidentifier = NULL    
AS      
Begin      
Select distinct ll.PrincipleId      
  , lt.Name_Ar as Principle_Type_Ar    
  , lt.Name_En as Principle_Type_En    
  , ll.Principle_Number    
  , ll.IssueDate    
  , ll.Introduction    
  , ll.PrincipleTitle as Principle_TitleEn     
  , ll.IssueDate_Hijri    
  , ll.Principle_Comment    
  ,ll.AddedBy    
  , ls.Name_Ar as Principle_Status_Ar     
  , ls.Name_En as Principle_Status_En    
  , ls.Id as StatusId   
  ,fs.Name_En as PrincipleFlowStatusEn
  ,fs.Name_Ar as PrincipleFlowStatusAr
  ,fs.Id as FlowStatusId


from LEGAL_Principle ll     
LEFT JOIN LEGAL_Principle_TYPE lt ON ll.Principle_Type = lt.Id              
LEFT JOIN Legal_Principle_Status ls ON ll.Principle_Status = ls.Id      
LEFT JOIN LEGAL_PRINCIPLE_FLOW_STATUS fs ON ll.Principle_Flow_Status = fs.Id      

WHERE ll.PrincipleId=@PrincipleId    
END    
GO






GO

/****** Object:  StoredProcedure [dbo].[plegislationDetailSelbyId]    Script Date: 28/11/2022 11:37:05 am ******/
DROP PROCEDURE [dbo].[plegislationDetailSelbyId]
GO

/****** Object:  StoredProcedure [dbo].[plegislationDetailSelbyId]    Script Date: 28/11/2022 11:37:05 am ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure [dbo].[plegislationDetailSelbyId]                                                     
    @LegislationId uniqueidentifier = Null                                        
                  
AS              
select distinct          
lg.LegislationId,  
llt.Name_Ar LegislationtypeAr,
llt.Name_En LegislationtypeEn,
lg.Introduction,        
lg.Legislation_Number,        
lg.IssueDate,        
lg.IssueDate_Hijri,        
lg.LegislationTitle, 
lgs.Name_En as StatusNameEn,  
lgs.Name_Ar as StatusNameAr,  
lg.StartDate,    
lg.CanceledDate    
from LEGAL_LEGISLATION lg   
left join LEGAL_LEGISLATION_STATUS lgs  
on lg.Legislation_Status = lgs.Id  
left join LEGAL_LEGISLATION_TYPE llt
on llt.Id =lg.Legislation_Type
  where lg.LegislationId =  @LegislationId  OR @LegislationId IS NULL OR @LegislationId='00000000-0000-0000-0000-000000000000'      
  and IsDeleted!=1      
  ORDER BY lg.LegislationId desc                                        
SET ROWCOUNT 0                                            
SET NOCOUNT OFF                                            
RETURN 
GO

GO

/****** Object:  StoredProcedure [dbo].[plegClauseSectionsSelbySourceId]    Script Date: 28/11/2022 11:40:43 am ******/
DROP PROCEDURE [dbo].[plegClauseSectionsSelbySourceId]
GO

/****** Object:  StoredProcedure [dbo].[plegClauseSectionsSelbySourceId]    Script Date: 28/11/2022 11:40:43 am ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE    Procedure [dbo].[plegClauseSectionsSelbySourceId]                                                       
    @LegislationId uniqueidentifier = Null                                         
                    
AS                
select distinct ll.LegislationId,       
la.ClauseId,      
la.Clause_Name,        
la.Clause_Content,         
ls.SectionTitle,  
las.Name_Ar as ClauseStatusAr,  
las.Name_En as ClauseStatusEn,  
la.[Start_Date]        
from LEGAL_CLAUSE la        
 right join LEGAL_LEGISLATION ll on           
    la.LegislationId= ll.LegislationId            
  left join LEGAL_SECTION ls on           
    la.SectionId= ls.SectionId   
 left join LEGAL_ARTICLE_STATUS las  
 on la.Clause_Status =las.Id  
  where la.LegislationId =@LegislationId  OR @LegislationId IS NULL OR @LegislationId='00000000-0000-0000-0000-000000000000'                
  ORDER BY ll.LegislationId desc                                          
SET ROWCOUNT 0                                              
SET NOCOUNT OFF 
GO

GO

/****** Object:  StoredProcedure [dbo].[plegArticalSectionsSelbySourceId]    Script Date: 28/11/2022 11:42:14 am ******/
DROP PROCEDURE [dbo].[plegArticalSectionsSelbySourceId]
GO

/****** Object:  StoredProcedure [dbo].[plegArticalSectionsSelbySourceId]    Script Date: 28/11/2022 11:42:14 am ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE      Procedure [dbo].[plegArticalSectionsSelbySourceId]                                                     
    @LegislationId uniqueidentifier = Null                                       
                  
AS              
select distinct ll.LegislationId,     
la.ArticleId,    
la.Article_Name,       
la.Article_Text,      
la.Article_Title,      
ls.SectionTitle,
las.Name_Ar as ArticalStatusAr,
las.Name_En as ArticalStatusEn,
la.[Start_Date]      
from LEGAL_ARTICLE la      
 right join LEGAL_LEGISLATION ll on         
    la.LegislationId= ll.LegislationId          
  left join LEGAL_SECTION ls on         
    la.SectionId= ls.SectionId 
	left join LEGAL_ARTICLE_STATUS las
	on la.Article_Status =las.Id
  where la.LegislationId =@LegislationId  OR @LegislationId IS NULL OR @LegislationId='00000000-0000-0000-0000-000000000000'              
  ORDER BY ll.LegislationId desc                                        
SET ROWCOUNT 0                                            
SET NOCOUNT OFF 
GO

  
  
CREATE or Alter      PROCEDURE [dbo].[pLegalLegislationDecision]      
@LegislationId uniqueidentifier = NULL    
AS      
Begin      
Select distinct ll.LegislationId      
  , lt.Name_Ar as Legislation_Type_Ar    
  , lt.Name_En as Legislation_Type_En    
  , ll.Legislation_Number    
  , ll.IssueDate    
  , ll.Introduction    
  , ll.LegislationTitle as Legislation_TitleEn     
  , ll.IssueDate_Hijri    
  , ll.Legislation_Comment    
  ,ll.AddedBy    
  , ls.Name_Ar as Legislation_Status_Ar     
  , ls.Name_En as Legislation_Status_En    
  , ls.Id as StatusId    
  , fs.Name_Ar as LegislationFlowStatusAr     
  , fs.Name_En as LegislationFlowStatusEn    
  , fs.Id as FlowStatusId    
from LEGAL_LEGISLATION ll     
LEFT JOIN LEGAL_LEGISLATION_TYPE lt ON ll.Legislation_Type = lt.Id              
LEFT JOIN LEGAL_LEGISLATION_STATUS ls ON ll.Legislation_Status = ls.Id      
LEFT JOIN LEGAL_LEGISLATION_FLOW_STATUS fs ON ll.Legislation_Flow_Status = ls.Id      
  
WHERE ll.LegislationId=@LegislationId    
END    




--pPrincipleListForHierarchy
GO

/****** Object:  StoredProcedure [dbo].[pPrincipleListForHierarchy]    Script Date: 13/12/2022 3:24:56 pm ******/
DROP PROCEDURE [dbo].[pPrincipleListForHierarchy]
GO

/****** Object:  StoredProcedure [dbo].[pPrincipleListForHierarchy]    Script Date: 13/12/2022 3:24:56 pm ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE  PROCEDURE [dbo].[pPrincipleListForHierarchy]                 
AS          
Begin          
Select distinct LP.PrincipleId          
  , LP.Principle_Type         
  , LP.Principle_Number         
  , LP.Introduction            
  , LP.IssueDate        
  , YEAR(LP.IssueDate) as Principle_Year      
  , LP.IssueDate_Hijri            
  , LP.PrincipleTitle          
  , LP.Principle_Status         
  , LP.Principle_Flow_Status        
  , LP.StartDate            
  , LP.AddedBy          
  , LP.AddedDate As CreatedDate         
  , LP.ModifiedBy            
  , LP.ModifiedDate  
  , LPT.Name_Ar As Principle_Type_Ar            
  , LPT.Name_En As Principle_Type_En             
  , LPS.Name_Ar As Principle_Status_Ar         
  , LPS.Name_En As Principle_Status_En        
  , LPFS.Name_Ar As Principle_Flow_Status_Ar        
  , LPFS.Name_En As Principle_Flow_Status_En     
  , LPC.CategoryName As Category_Name     
  , LPSC.SubCategoryName As Sub_Category_Name
  , LPCM.CategoryId AS CategoryId  
  , LPSCM.SubCategoryId AS SubCategoryId  
from LEGAL_Principle LP                  
LEFT JOIN LEGAL_Principle_TYPE LPT ON LP.Principle_Type = LPT.Id        
LEFT JOIN LEGAL_Principle_FLOW_STATUS LPFS ON LP.Principle_Flow_Status = LPFS.Id         
LEFT JOIN LEGAL_Principle_STATUS LPS ON LP.Principle_Status = LPS.Id
INNER JOIN LEGAL_PRINCIPLE_LEGAL_CATEGORY LPCM ON LP.PrincipleId = LPCM.PrincipleId 
LEFT JOIN LEGAL_PRINCIPLE_CATEGORY_FTW_LKP LPC ON LPCM.CategoryId = LPC.Id 
INNER JOIN LEGAL_PRINCIPLE_LEGAL_SUB_CATEGORY LPSCM ON LP.PrincipleId = LPSCM.PrincipleId AND LPCM.CategoryId = LPSCM.CategoryId
LEFT JOIN LEGAL_PRINCIPLE_SUB_CATEGORY_FTW_LKP LPSC ON LPSCM.SubCategoryId = LPSC.Id
WHERE LP.IsDeleted = 0          
ORDER BY LP.AddedDate DESC                 
SET ROWCOUNT 0                  
SET NOCOUNT OFF                  
RETURN                   
END       
GO  

      
	
--pPrincipleListForReview
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pPrincipleListForReview]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pPrincipleListForReview]
GO

CREATE    PROCEDURE [dbo].[pPrincipleListForReview]        
(      
@Principle_Number NVARCHAR(1000) = null,      
@Principle_Type int = null,      
@introduction NVARCHAR(1000) = null,      
@PrincipleTitle  NVARCHAR(1000) = null,      
@start_From datetime=null,      
@end_To datetime=null,      
@BaseNumber NVARCHAR(1000)=null,  
@AddedBy NVARCHAR(1000)=null  
)      
AS        
Begin        
Select distinct ll.PrincipleId        
  , ll.Principle_Type       
  , ll.Principle_Number       
  , ll.Introduction          
  , ll.IssueDate      
  , YEAR(ll.IssueDate) as Principle_Year    
  , ll.IssueDate_Hijri          
  , ll.PrincipleTitle        
  , ll.Principle_Status       
  , ll.Principle_Flow_Status      
  , ll.StartDate          
  , ll.AddedBy        
  , ll.AddedDate As CreatedDate       
  , ll.ModifiedBy          
  , ll.ModifiedDate      
  ,ll.BaseNumber    
  , llt.Name_Ar As Principle_Type_Ar          
  , llt.Name_En As Principle_Type_En           
  , lls.Name_Ar As Principle_Status_Ar       
  , lls.Name_En As Principle_Status_En      
  , llfs.Name_Ar As Principle_Flow_Status_Ar      
  , llfs.Name_En As Principle_Flow_Status_En      
      
from LEGAL_Principle ll                
LEFT JOIN LEGAL_Principle_TYPE llt ON ll.Principle_Type = llt.Id      
LEFT JOIN LEGAL_Principle_FLOW_STATUS llfs ON ll.Principle_Flow_Status = llfs.Id       
LEFT JOIN LEGAL_Principle_STATUS lls ON ll.Principle_Status = lls.Id        
WHERE ll.IsDeleted = 0   
AND ll.Principle_Flow_Status = 2
AND (ll.Principle_Number = @Principle_Number OR @Principle_Number IS NULL OR @Principle_Number ='')      
AND (ll.Principle_Type = @Principle_Type OR @Principle_Type IS NULL OR @Principle_Type = '')    
AND (ll.BaseNumber=@BaseNumber OR @BaseNumber IS NULL OR @BaseNumber='')  
AND(ll.AddedBy=@AddedBy OR @AddedBy IS NULL OR @AddedBy='')    
AND (ll.PrincipleTitle = @PrincipleTitle  OR ll.PrincipleTitle = @PrincipleTitle  OR @PrincipleTitle  IS NULL OR @PrincipleTitle  ='')      
AND (CAST(ll.StartDate as date)>=@start_From OR @start_From IS NULL OR @start_From='')      
    
ORDER BY ll.AddedDate DESC               
SET ROWCOUNT 0                
SET NOCOUNT OFF                
RETURN                 
END    

    
	
--pPrincipleListForPublish
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pPrincipleListForPublish]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pPrincipleListForPublish]
GO

CREATE    PROCEDURE [dbo].[pPrincipleListForPublish]        
(      
@Principle_Number NVARCHAR(1000) = null,      
@Principle_Type int = null,      
@introduction NVARCHAR(1000) = null,      
@PrincipleTitle  NVARCHAR(1000) = null,      
@start_From datetime=null,      
@end_To datetime=null,      
@BaseNumber NVARCHAR(1000)=null,  
@AddedBy NVARCHAR(1000)=null  
)      
AS        
Begin        
Select distinct ll.PrincipleId        
  , ll.Principle_Type       
  , ll.Principle_Number       
  , ll.Introduction          
  , ll.IssueDate      
  , YEAR(ll.IssueDate) as Principle_Year    
  , ll.IssueDate_Hijri          
  , ll.PrincipleTitle        
  , ll.Principle_Status       
  , ll.Principle_Flow_Status      
  , ll.StartDate          
  , ll.AddedBy        
  , ll.AddedDate As CreatedDate       
  , ll.ModifiedBy          
  , ll.ModifiedDate      
  ,ll.BaseNumber    
  , llt.Name_Ar As Principle_Type_Ar          
  , llt.Name_En As Principle_Type_En           
  , lls.Name_Ar As Principle_Status_Ar       
  , lls.Name_En As Principle_Status_En      
  , llfs.Name_Ar As Principle_Flow_Status_Ar      
  , llfs.Name_En As Principle_Flow_Status_En      
      
from LEGAL_Principle ll                
LEFT JOIN LEGAL_Principle_TYPE llt ON ll.Principle_Type = llt.Id      
LEFT JOIN LEGAL_Principle_FLOW_STATUS llfs ON ll.Principle_Flow_Status = llfs.Id       
LEFT JOIN LEGAL_Principle_STATUS lls ON ll.Principle_Status = lls.Id        
WHERE ll.IsDeleted = 0   
AND (ll.Principle_Flow_Status = 64)
AND (ll.Principle_Number = @Principle_Number OR @Principle_Number IS NULL OR @Principle_Number ='')      
AND (ll.Principle_Type = @Principle_Type OR @Principle_Type IS NULL OR @Principle_Type = '')    
AND (ll.BaseNumber=@BaseNumber OR @BaseNumber IS NULL OR @BaseNumber='')  
AND(ll.AddedBy=@AddedBy OR @AddedBy IS NULL OR @AddedBy='')    
AND (ll.PrincipleTitle = @PrincipleTitle  OR ll.PrincipleTitle = @PrincipleTitle  OR @PrincipleTitle  IS NULL OR @PrincipleTitle  ='')      
AND (CAST(ll.StartDate as date)>=@start_From OR @start_From IS NULL OR @start_From='')      
    
ORDER BY ll.AddedDate DESC               
SET ROWCOUNT 0                
SET NOCOUNT OFF                
RETURN                 
END    

---------------------------------------------------------
/****** Object:  StoredProcedure [dbo].[pGetLegalPrincipleDecisionbyPrincipleId]    Script Date: 12/6/2022 11:59:38 PM ******/
DROP PROCEDURE [dbo].[pGetLegalPrincipleDecisionbyPrincipleId]
GO

/****** Object:  StoredProcedure [dbo].[pGetLegalPrincipleDecisionbyPrincipleId]    Script Date: 12/6/2022 11:59:38 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE  PROCEDURE [dbo].[pGetLegalPrincipleDecisionbyPrincipleId]      
@PrincipleId uniqueidentifier = NULL    
AS      
Begin      
Select distinct ll.PrincipleId      
  , lt.Name_Ar as Principle_Type_Ar    
  , lt.Name_En as Principle_Type_En    
  , ll.Principle_Number    
  , ll.IssueDate    
  , ll.Introduction    
  , ll.PrincipleTitle as Principle_TitleEn     
  , ll.IssueDate_Hijri    
  , ll.Principle_Comment    
  ,ll.AddedBy    
  , ls.Name_Ar as Principle_Status_Ar     
  , ls.Name_En as Principle_Status_En    
  , ls.Id as StatusId   
  ,fs.Name_En as PrincipleFlowStatusEn
  ,fs.Name_Ar as PrincipleFlowStatusAr
  ,fs.Id as FlowStatusId


from LEGAL_Principle ll     
LEFT JOIN LEGAL_Principle_TYPE lt ON ll.Principle_Type = lt.Id              
LEFT JOIN Legal_Principle_Status ls ON ll.Principle_Status = ls.Id      
LEFT JOIN LEGAL_PRINCIPLE_FLOW_STATUS fs ON ll.Principle_Flow_Status = fs.Id      

WHERE ll.PrincipleId=@PrincipleId    
END    
GO

------------------------------------------------------------------------------------------------------------------
-------------------------------------------------------------------------------------------------------------------
                                                -- NEW PROCEDURE BY ZAEEM--
------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------

  
--pLegislationListForPublish-------------------------------------------------------------------------------
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pLegislationListForPublish]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pLegislationListForPublish]
GO

  CREATE or alter  Procedure [dbo].[pLegislationListForPublish]       
AS                  
BEGIN                  
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
  , ll.LegislationTitle As LegislationTitleEn  
from LEGAL_LEGISLATION ll                
LEFT JOIN LEGAL_LEGISLATION_TYPE llt ON ll.Legislation_Type = llt.Id      
LEFT JOIN LEGAL_LEGISLATION_FLOW_STATUS llfs ON ll.Legislation_Flow_Status = llfs.Id       
LEFT JOIN LEGAL_LEGISLATION_STATUS lls ON ll.Legislation_Status = lls.Id        
WHERE ll.IsDeleted = 0    
AND    
llfs.Id = '64'   
ORDER BY ll.Legislation_Number DESC                 
SET ROWCOUNT 0                  
SET NOCOUNT OFF                  
RETURN                   
END 

---------------------pPrincipleListFiltered--------------------------------------------------------------------------
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pPrincipleListFiltered]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pPrincipleListFiltered]
GO
CREATE or Alter   PROCEDURE [dbo].[pPrincipleListFiltered]        
(      
@Principle_Number NVARCHAR(1000) = null,      
@Principle_Type int = null,      
@introduction NVARCHAR(1000) = null,      
@PrincipleTitle  NVARCHAR(1000) = null,   
@Principle_Flow_Status int = null,   
@Principle_Status int = null,      
@start_From datetime=null,      
@end_To datetime=null,      
@AddedBy NVARCHAR(1000)=null  
)      
AS        
Begin        
Select distinct ll.PrincipleId        
  , ll.Principle_Type       
  , ll.Principle_Number       
  , ll.Introduction          
  , ll.IssueDate      
  , YEAR(ll.IssueDate) as Principle_Year    
  , ll.IssueDate_Hijri          
  , ll.PrincipleTitle        
  , ll.Principle_Status       
  , ll.Principle_Flow_Status      
  , ll.StartDate          
  , ll.AddedBy        
  , ll.AddedDate As CreatedDate       
  , ll.ModifiedBy          
  , ll.ModifiedDate   
  , llt.Name_Ar As Principle_Type_Ar          
  , llt.Name_En As Principle_Type_En           
  , lls.Name_Ar As Principle_Status_Ar       
  , lls.Name_En As Principle_Status_En      
  , llfs.Name_Ar As Principle_Flow_Status_Ar      
  , llfs.Name_En As Principle_Flow_Status_En      
      
from LEGAL_Principle ll                
LEFT JOIN LEGAL_Principle_TYPE llt ON ll.Principle_Type = llt.Id      
LEFT JOIN LEGAL_Principle_FLOW_STATUS llfs ON ll.Principle_Flow_Status = llfs.Id       
LEFT JOIN LEGAL_Principle_STATUS lls ON ll.Principle_Status = lls.Id        
WHERE ll.IsDeleted = 0        
AND (ll.Principle_Number = @Principle_Number OR @Principle_Number IS NULL OR @Principle_Number ='')      
AND (ll.Principle_Type = @Principle_Type OR @Principle_Type IS NULL OR @Principle_Type = '')    
AND(ll.AddedBy=@AddedBy OR @AddedBy IS NULL OR @AddedBy='')    
AND (ll.PrincipleTitle = @PrincipleTitle  OR ll.PrincipleTitle = @PrincipleTitle  OR @PrincipleTitle  IS NULL OR @PrincipleTitle  ='')      
AND (ll.Principle_Status = @Principle_Status OR @Principle_Status IS NULL OR @Principle_Status = '')      
AND (ll.Principle_Flow_Status = @Principle_Flow_Status OR @Principle_Flow_Status IS NULL OR @Principle_Flow_Status = '')      

AND (CAST(ll.StartDate as date)>=@start_From OR @start_From IS NULL OR @start_From='')      
    
ORDER BY ll.AddedDate DESC               
SET ROWCOUNT 0                
SET NOCOUNT OFF                
RETURN                 
END    
  ----------------------------[pPrincipleListForPublish]-----------------------------------------------------------

  IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pPrincipleListForPublish]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pPrincipleListForPublish]
GO
  CREATE Or Alter   PROCEDURE [dbo].[pPrincipleListForPublish]          
(        
@Principle_Number NVARCHAR(1000) = null,        
@Principle_Type int = null,        
@introduction NVARCHAR(1000) = null,        
@PrincipleTitle  NVARCHAR(1000) = null,        
@start_From datetime=null,        
@end_To datetime=null,        
@AddedBy NVARCHAR(1000)=null
)        
AS          
Begin          
Select distinct ll.PrincipleId          
  , ll.Principle_Type         
  , ll.Principle_Number         
  , ll.Introduction            
  , ll.IssueDate        
  , YEAR(ll.IssueDate) as Principle_Year      
  , ll.IssueDate_Hijri            
  , ll.PrincipleTitle          
  , ll.Principle_Status         
  , ll.Principle_Flow_Status        
  , ll.StartDate            
  , ll.AddedBy          
  , ll.AddedDate As CreatedDate         
  , ll.ModifiedBy            
  , ll.ModifiedDate        
  , llt.Name_Ar As Principle_Type_Ar            
  , llt.Name_En As Principle_Type_En             
  , lls.Name_Ar As Principle_Status_Ar         
  , lls.Name_En As Principle_Status_En        
  , llfs.Name_Ar As Principle_Flow_Status_Ar        
  , llfs.Name_En As Principle_Flow_Status_En        
        
from LEGAL_Principle ll                  
LEFT JOIN LEGAL_Principle_TYPE llt ON ll.Principle_Type = llt.Id        
LEFT JOIN LEGAL_Principle_FLOW_STATUS llfs ON ll.Principle_Flow_Status = llfs.Id         
LEFT JOIN LEGAL_Principle_STATUS lls ON ll.Principle_Status = lls.Id          
WHERE ll.IsDeleted = 0     
AND (ll.Principle_Flow_Status = 4 OR ll.Principle_Flow_Status = 64)  
AND (ll.Principle_Number = @Principle_Number OR @Principle_Number IS NULL OR @Principle_Number ='')        
AND (ll.Principle_Type = @Principle_Type OR @Principle_Type IS NULL OR @Principle_Type = '')      
AND(ll.AddedBy=@AddedBy OR @AddedBy IS NULL OR @AddedBy='')
AND (ll.PrincipleTitle = @PrincipleTitle  OR ll.PrincipleTitle = @PrincipleTitle  OR @PrincipleTitle  IS NULL OR @PrincipleTitle  ='')        
AND (CAST(ll.StartDate as date)>=@start_From OR @start_From IS NULL OR @start_From='')        
      
ORDER BY ll.AddedDate DESC                 
SET ROWCOUNT 0                  
SET NOCOUNT OFF                  
RETURN                   
END  

------------------------------------[pPrincipleListForReview]---------------------------------------------------
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pPrincipleListForReview]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pPrincipleListForReview]
GO
CREATE  or Alter  PROCEDURE [dbo].[pPrincipleListForReview]          
(        
@Principle_Number NVARCHAR(1000) = null,        
@Principle_Type int = null,        
@introduction NVARCHAR(1000) = null,        
@PrincipleTitle  NVARCHAR(1000) = null,        
@start_From datetime=null,        
@end_To datetime=null,        
@AddedBy NVARCHAR(1000)=null
)        
AS          
Begin          
Select distinct ll.PrincipleId          
  , ll.Principle_Type         
  , ll.Principle_Number         
  , ll.Introduction            
  , ll.IssueDate        
  , YEAR(ll.IssueDate) as Principle_Year      
  , ll.IssueDate_Hijri            
  , ll.PrincipleTitle          
  , ll.Principle_Status         
  , ll.Principle_Flow_Status        
  , ll.StartDate            
  , ll.AddedBy          
  , ll.AddedDate As CreatedDate         
  , ll.ModifiedBy            
  , ll.ModifiedDate        
       
  , llt.Name_Ar As Principle_Type_Ar            
  , llt.Name_En As Principle_Type_En             
  , lls.Name_Ar As Principle_Status_Ar         
  , lls.Name_En As Principle_Status_En        
  , llfs.Name_Ar As Principle_Flow_Status_Ar        
  , llfs.Name_En As Principle_Flow_Status_En        
        
from LEGAL_Principle ll                  
LEFT JOIN LEGAL_Principle_TYPE llt ON ll.Principle_Type = llt.Id        
LEFT JOIN LEGAL_Principle_FLOW_STATUS llfs ON ll.Principle_Flow_Status = llfs.Id         
LEFT JOIN LEGAL_Principle_STATUS lls ON ll.Principle_Status = lls.Id          
WHERE ll.IsDeleted = 0     
AND ll.Principle_Flow_Status = 2  
AND (ll.Principle_Number = @Principle_Number OR @Principle_Number IS NULL OR @Principle_Number ='')        
AND (ll.Principle_Type = @Principle_Type OR @Principle_Type IS NULL OR @Principle_Type = '') 
AND(ll.AddedBy=@AddedBy OR @AddedBy IS NULL OR @AddedBy='')      
AND (ll.PrincipleTitle = @PrincipleTitle  OR ll.PrincipleTitle = @PrincipleTitle  OR @PrincipleTitle  IS NULL OR @PrincipleTitle  ='')        
AND (CAST(ll.StartDate as date)>=@start_From OR @start_From IS NULL OR @start_From='')        
      
ORDER BY ll.AddedDate DESC                 
SET ROWCOUNT 0                  
SET NOCOUNT OFF                  
RETURN                   
END 

----------------------------------------------pLegalPricnipleDetailesbyId------------------------------------------------------------
/****** Object:  StoredProcedure [dbo].[pLegalPricnipleDetailesbyId]    Script Date: 12/8/2022 4:12:48 PM ******/
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pLegalPricnipleDetailesbyId]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pLegalPricnipleDetailesbyId]
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER    Procedure [dbo].[pLegalPricnipleDetailesbyId]                                                       
    @PrincipleId uniqueidentifier = Null                                          
                
AS                
select distinct            
lg.PrincipleId,    
llt.Name_Ar PrincipletypeAr,  
llt.Name_En PrincipletypeEn,  
lg.Introduction,          
lg.Principle_Number,          
lg.IssueDate,          
lg.IssueDate_Hijri,          
lg.PrincipleTitle,    
lgs.Name_En as FlowStatusNameEn,    
lgs.Name_Ar as FlowStatusNameAr,    
lg.StartDate,      
lg.CanceledDate,  
lg.Summary,  
lg.Conclusion,  
lg.Notes,  
lg.Principle_Comment  
from LEGAL_Principle lg     
left join LEGAL_PRINCIPLE_FLOW_STATUS lgs    
on lg.Principle_Flow_Status = lgs.Id    
left join LEGAL_Principle_TYPE llt  
on llt.Id =lg.Principle_Type  
left join LEGAL_PRINCIPLE_ARTICLE la on lg.PrincipleId=la.PrincipleId  
left join LEGAL_PRINCIPLE_PUBLICATION_SOURCE lp on lg.PrincipleId=lp.PrincipleId  
  where lg.PrincipleId =  @PrincipleId  OR @PrincipleId IS NULL OR @PrincipleId='00000000-0000-0000-0000-000000000000'        
  and IsDeleted!=1        
  ORDER BY lg.PrincipleId desc                                          
SET ROWCOUNT 0                                              
SET NOCOUNT OFF                                              
RETURN   
GO
-----------------------------------------------[pLPRelationAdvanceSearch]---------------------------------
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pLPRelationAdvanceSearch]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pLPRelationAdvanceSearch]
	GO
CREATE   or ALTER Procedure [dbo].[pLPRelationAdvanceSearch]                           
    @PrincipleType int = NULL                     
  , @PrincipleNumber NVARCHAR(150)= NUll                      
  , @PrincipleYear int = NULL      
    
AS                               
 Select ll.PrincipleId as PrincipleId     
 , ll.PrincipleTitle as PrincipleTitle    
 , ll.Principle_Number as Principle_Number 
 , YEAR(ll.IssueDate) as Principle_Year    
 , ll.Principle_Status as Principle_Status    
 , llt.Id as Principle_Type    
 , llt.Name_En as Principle_Type_En                          
 , llt.Name_Ar as Principle_Type_Ar    
 , lls.Name_En as Principle_Status_En    
 , lls.Name_Ar as Principle_Status_Ar    
  from LEGAL_PRINCIPLE ll                          
  LEFT JOIN LEGAL_PRINCIPLE_TYPE llt ON ll.Principle_Type = llt.Id    
  LEFT JOIN LEGAL_PRINCIPLE_STATUS lls ON ll.Principle_Status = lls.Id    
  Where ll.IsDeleted != 1                          
  AND (ll.Principle_Type = @PrincipleType or @PrincipleType is Null OR @PrincipleType = '')      
  AND (ll.Principle_Number like '%' + @PrincipleNumber + '%' or @PrincipleNumber is Null OR @PrincipleNumber like '')    
  AND ((YEAR(ll.IssueDate) = @PrincipleYear) Or @PrincipleYear Is Null OR @PrincipleYear = '')     
          
    
       
  ORDER BY ll.AddedDate desc    
 SET ROWCOUNT 0                              
 SET NOCOUNT OFF                              
 RETURN 

 --------------------------------------------------[pLegalPricnipleDetailesbyId]--------------------------------------
 IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pLegalPricnipleDetailesbyId]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pLegalPricnipleDetailesbyId]
	GO
 CREATE    Procedure [dbo].[pLegalPricnipleDetailesbyId]                                                         
    @PrincipleId uniqueidentifier = Null                                            
                  
AS                  
select distinct              
lg.PrincipleId,      
llt.Name_Ar PrincipletypeAr,    
llt.Name_En PrincipletypeEn,    
lg.Introduction,            
lg.Principle_Number,            
lg.IssueDate,            
lg.IssueDate_Hijri,            
lg.PrincipleTitle,      
lgs.Name_En as FlowStatusNameEn,      
lgs.Name_Ar as FlowStatusNameAr,      
lg.StartDate,        
lg.CanceledDate,    
lg.BaseNumber,    
lg.Summary,    
lg.Conclusion,    
lg.Notes,    
lg.Principle_Comment    
from LEGAL_Principle lg       
left join LEGAL_PRINCIPLE_FLOW_STATUS lgs      
on lg.Principle_Flow_Status = lgs.Id      
left join LEGAL_Principle_TYPE llt    
on llt.Id =lg.Principle_Type    
left join LEGAL_PRINCIPLE_ARTICLE la on lg.PrincipleId=la.PrincipleId    
left join LEGAL_PRINCIPLE_PUBLICATION_SOURCE lp on lg.PrincipleId=lp.PrincipleId    
  where lg.PrincipleId =  @PrincipleId  OR @PrincipleId IS NULL OR @PrincipleId='00000000-0000-0000-0000-000000000000'          
  and IsDeleted!=1          
  ORDER BY lg.PrincipleId desc                                            
SET ROWCOUNT 0                                                
SET NOCOUNT OFF                                                
RETURN 
------------------------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------------------------
-------------------------------------------------------------------  pDeletePrincipleList         -----------------------------------
-----------------------------------------------------------------------------------------------------------------------
/****** Object:  StoredProcedure [dbo].[pDeletePrincipleList]    Script Date: 12/12/2022 7:52:24 PM ******/
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pDeletePrincipleList]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
DROP PROCEDURE [dbo].[pDeletePrincipleList]
GO

/****** Object:  StoredProcedure [dbo].[pDeletePrincipleList]    Script Date: 12/12/2022 7:52:24 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE      PROCEDURE [dbo].[pDeletePrincipleList]          
        
AS          
Begin          
Select distinct ll.PrincipleId          
  , ll.Principle_Type         
  , ll.Principle_Number         
  , ll.Introduction            
  , ll.IssueDate        
  , YEAR(ll.IssueDate) as Principle_Year      
  , ll.IssueDate_Hijri            
  , ll.PrincipleTitle          
  , ll.Principle_Status         
  , ll.Principle_Flow_Status        
  , ll.StartDate            
  , ll.AddedBy          
  , ll.DeletedDate As CreatedDate         
  , ll.ModifiedBy            
  , ll.ModifiedDate   
  , llt.Name_Ar As Principle_Type_Ar            
  , llt.Name_En As Principle_Type_En             
  , lls.Name_Ar As Principle_Status_Ar         
  , lls.Name_En As Principle_Status_En        
  , llfs.Name_Ar As Principle_Flow_Status_Ar        
  , llfs.Name_En As Principle_Flow_Status_En        
        
from LEGAL_Principle ll                  
LEFT JOIN LEGAL_Principle_TYPE llt ON ll.Principle_Type = llt.Id        
LEFT JOIN LEGAL_Principle_FLOW_STATUS llfs ON ll.Principle_Flow_Status = llfs.Id         
LEFT JOIN LEGAL_Principle_STATUS lls ON ll.Principle_Status = lls.Id          
WHERE ll.IsDeleted = 1          

ORDER BY ll.DeletedDate DESC                 
SET ROWCOUNT 0                  
SET NOCOUNT OFF                  
RETURN                   
END      
GO

---------------------------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------------------------
------------------------------------------[pPrincipleListFilteredByStatus]         ----------------------------------------
---------------------------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[pPrincipleListFilteredByStatus]    Script Date: 12/12/2022 7:43:26 PM ******/
IF EXISTS (select * from sysobjects where id = object_id('[dbo].[pPrincipleListFilteredByStatus]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)

DROP PROCEDURE [dbo].[pPrincipleListFilteredByStatus]
GO

/****** Object:  StoredProcedure [dbo].[pPrincipleListFilteredByStatus]    Script Date: 12/12/2022 7:43:26 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE   Procedure [dbo].[pPrincipleListFilteredByStatus]    
(      
@status int      
)      
AS                  
BEGIN                  
Select distinct ll.PrincipleId        
  , ll.Principle_Type         
  , ll.Principle_Number         
  , ll.Introduction            
  , ll.IssueDate        
  , YEAR(ll.IssueDate) as Principle_Year      
  , ll.IssueDate_Hijri            
  , ll.PrincipleTitle          
  , ll.Principle_Status         
  , ll.Principle_Flow_Status        
  , ll.StartDate            
  , ll.AddedBy          
  , ll.AddedDate As CreatedDate         
  , ll.ModifiedBy            
  , ll.ModifiedDate   
  , llt.Name_Ar As Principle_Type_Ar            
  , llt.Name_En As Principle_Type_En             
  , lls.Name_Ar As Principle_Status_Ar         
  , lls.Name_En As Principle_Status_En        
  , llfs.Name_Ar As Principle_Flow_Status_Ar        
  , llfs.Name_En As Principle_Flow_Status_En     
from LEGAL_PRINCIPLE ll                
LEFT JOIN LEGAL_Principle_TYPE llt ON ll.Principle_Type = llt.Id        
LEFT JOIN LEGAL_Principle_FLOW_STATUS llfs ON ll.Principle_Flow_Status = llfs.Id         
LEFT JOIN LEGAL_Principle_STATUS lls ON ll.Principle_Status = lls.Id          
WHERE ll.IsDeleted = 0    
AND    
llfs.Id=@status    
ORDER BY ll.Principle_Number DESC                 
SET ROWCOUNT 0                  
SET NOCOUNT OFF                  
RETURN                   
END 
GO
---------------------------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------  pPrincipleListFiltered        ---------------------------------------------------
---------------------------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------------------------
USE [FATWA_DB_DEV]
GO

/****** Object:  StoredProcedure [dbo].[pPrincipleListFiltered]    Script Date: 12/13/2022 12:40:29 PM ******/
DROP PROCEDURE [dbo].[pPrincipleListFiltered]
GO

/****** Object:  StoredProcedure [dbo].[pPrincipleListFiltered]    Script Date: 12/13/2022 12:40:29 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE      PROCEDURE [dbo].[pPrincipleListFiltered]          
(        
@Principle_Number NVARCHAR(1000) = null,        
@Principle_Type int = null,        
@introduction NVARCHAR(1000) = null,        
@PrincipleTitle  NVARCHAR(1000) = null,     
@Principle_Flow_Status int = null,     
@Principle_Status int = null,        
@start_From datetime=null,        
@end_To datetime=null,        
@AddedBy NVARCHAR(1000)=null    
)        
AS          
Begin          
Select distinct ll.PrincipleId          
  , ll.Principle_Type         
  , ll.Principle_Number         
  , ll.Introduction            
  , ll.IssueDate        
  , YEAR(ll.IssueDate) as Principle_Year      
  , ll.IssueDate_Hijri            
  , ll.PrincipleTitle          
  , ll.Principle_Status         
  , ll.Principle_Flow_Status        
  , ll.StartDate            
  , ll.AddedBy          
  , ll.AddedDate As CreatedDate         
  , ll.ModifiedBy            
  , ll.ModifiedDate   
  , llt.Name_Ar As Principle_Type_Ar            
  , llt.Name_En As Principle_Type_En             
  , lls.Name_Ar As Principle_Status_Ar         
  , lls.Name_En As Principle_Status_En        
  , llfs.Name_Ar As Principle_Flow_Status_Ar        
  , llfs.Name_En As Principle_Flow_Status_En        
        
from LEGAL_Principle ll                  
LEFT JOIN LEGAL_Principle_TYPE llt ON ll.Principle_Type = llt.Id        
LEFT JOIN LEGAL_Principle_FLOW_STATUS llfs ON ll.Principle_Flow_Status = llfs.Id         
LEFT JOIN LEGAL_Principle_STATUS lls ON ll.Principle_Status = lls.Id          
WHERE ll.IsDeleted = 0          
AND (ll.Principle_Number = @Principle_Number OR @Principle_Number IS NULL OR @Principle_Number ='')        
AND (ll.Principle_Type = @Principle_Type OR @Principle_Type IS NULL OR @Principle_Type = '')      
AND(ll.AddedBy=@AddedBy OR @AddedBy IS NULL OR @AddedBy='')      
AND (ll.PrincipleTitle = @PrincipleTitle  OR ll.PrincipleTitle = @PrincipleTitle  OR @PrincipleTitle  IS NULL OR @PrincipleTitle  ='')        
AND (ll.Principle_Status = @Principle_Status OR @Principle_Status IS NULL OR @Principle_Status = '')        
AND (ll.Principle_Flow_Status = @Principle_Flow_Status OR @Principle_Flow_Status IS NULL OR @Principle_Flow_Status = '')        
  
AND (CAST(ll.AddedDate as date)>=@start_From OR @start_From IS NULL OR @start_From='')        
AND (CAST(ll.AddedDate as date)<=@end_To OR @end_To IS NULL OR @end_To='')    
      
      
ORDER BY ll.AddedDate DESC                 
SET ROWCOUNT 0                  
SET NOCOUNT OFF                  
RETURN                   
END      
GO



  
