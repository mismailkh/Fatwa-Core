-- Nadia Gull
/****** Object:  StoredProcedure [dbo].[pGenerateOutboxInfo]  ******/
if exists (select * from sysobjects where id = object_id('[dbo].[pGenerateOutboxInfo]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pGenerateOutboxInfo]
GO
/****** Object:  StoredProcedure [dbo].[pGenerateOutboxInfo]   ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE pGenerateOutboxInfo (
	  @refID nvarchar(150) = null
	, @outboxNo nvarchar(50) = null
	, @outdate datetime = Null
)
AS BEGIN
DECLARE @comID nvarchar(max)
select distinct @comID = a.CommunicationId from COMM_COMMUNICATION a
join COMM_COMMUNICATION_TARGET_LINK b on a.CommunicationId = b.CommunicationId
join LINK_TARGET c on b.TargetLinkId = c.TargetLinkId
where a.CommunicationTypeId = 8
and c.ReferenceId = @refID;

UPDATE COMM_COMMUNICATION
	set OutboxNumber = @outboxNo,
	OutboxDate = @outdate
	where CommunicationId = @comID;

SELECT CommunicationId, OutboxNumber, OutboxDate from COMM_COMMUNICATION
where CommunicationId = @comID
END
GO
  PRINT 'pGenerateOutboxInfo'
GO
---------------------------------------

-- pCommunicationDetail '436e82d2-70d8-455c-a643-7909b8689667'      
if exists (select * from sysobjects where id = object_id('[dbo].[pCommunicationDetail]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pCommunicationDetail]
GO
CREATE Procedure [dbo].[pCommunicationDetail]                  
 @CommunicationId [nvarchar](450) = NULL                
AS                
BEGIN             
    Select top 1 ccr.RequestDate,     
    ccr.ResponseDate,     
    ccr.ResponseTypeId,     
    ccr.Reason,      
    ccr.Other,  
    ccr.CommunicationId, 
    dc.[FileName],     
    dc.StoragePath  
    FROM COMM_COMMUNICATION_RESPONSE ccr      
    LEFT JOIN UPLOADED_DOCUMENT dc on dc.ReferenceGuid = ccr.CommunicationId      
    Where ccr.IsDeleted = 0       
END  
GO
PRINT 'Created [pCommunicationDetail]'
GO

-- exec pCommSendResponseDetailbyId @CommunicationId = N'8d65b416-f8c0-4fff-b637-1c74ea2b663e', @CommunicationType = N'4'  
if exists (select * from sysobjects where id = object_id('[dbo].[pCommSendResponseDetailbyId]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pCommSendResponseDetailbyId]
GO
CREATE Procedure pCommSendResponseDetailbyId   
	@CommunicationId uniqueidentifier,      
	@CommunicationType varchar(100)       
As     
BEGIN
	Select comm.CommunicationId,      
	comm.Title as CommunicationResponseTitle,      
	comm.[Description],      
	comm.InboxNumber,      
	comm.InboxDate,      
	comm.OutboxNumber,      
	comm.OutboxDate,     
	ccr.RequestDate,     
    ccr.ResponseDate,     
    ccr.ResponseTypeId,     
    ccr.Reason,  
	ccr.IsUrgent,  
    ccr.Other,    
	ccc.NameEn as CorrespondenceTypeEn,      
	ccc.NameAr as CorrespondenceTypeAr,      
	cct.NameEn as Activity_En,      
	cct.NameAr as Activity_Ar      
	FROM COMM_COMMUNICATION comm      
	LEFT JOIN COMM_COMMUNICATION_RESPONSE ccr on comm.CommunicationId = ccr.CommunicationId      
	LEFT JOIN COMM_COMMUNICATION_TYPE cct on cct.CommunicationTypeId = comm.CommunicationTypeId      
	LEFT JOIN COMM_COMMUNICATION_CORRESPONDENCE_TYPE ccc on ccc.CorrespondenceTypeId = comm.CorrespondenceTypeId      
	WHERE comm.IsDeleted = 0   
	AND (comm.CommunicationId = @CommunicationId OR @CommunicationId IS NULL Or @CommunicationId='00000000-0000-0000-0000-000000000000')      
	AND (cct.CommunicationTypeId = @CommunicationType or cct.NameAr=@CommunicationType)      
END
GO
PRINT 'Created [pCommSendResponseDetailbyId]'
GO



-- exec pCommSendResponseDetailbyId @CommunicationId = N'8d65b416-f8c0-4fff-b637-1c74ea2b663e', @CommunicationType = N'4'  
if exists (select * from sysobjects where id = object_id('[dbo].[pCmsCaseRequestList]') and OBJECTPROPERTY(id, 'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[pCmsCaseRequestList]
GO
CREATE PROCEDURE [dbo].[pCmsCaseRequestList]                
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
cctgl.Name_Ar as Court_Type_Name_Ar ,
ccf.FileId
FROM CMS_CASE_REQUEST ccr                    
                    
left join Department cd on  ccr.DepartmentId = cd.Id                    
left join CMS_GOVERNMENT_ENTITY_G2G_LKP cg on  ccr.GovtEntityId = cg.EntityId                    
left join CMS_OPERATING_SECTOR_TYPE_G2G_LKP co on ccr.SectorTypeId= co.Id                     
left join CMS_REQUEST_TYPE_G2G_LKP rt on ccr.RequestTypeId= rt.Id                       
left join CMS_SUBTYPE_G2G_LKP cs on ccr.SubTypeId = cs.Id                    
left join CMS_PRIORITY_G2G_LKP cp on ccr.PriorityId = cp.Id                    
left join CMS_CASE_REQUEST_STATUS_G2G_LKP ccrs on ccr.StatusId = ccrs.Id             
left join CMS_COURT_TYPE_G2G_LKP cctgl on ccr.CourtTypeId = cctgl.Id   
left join CMS_CASE_FILE ccf on ccf.RequestId = ccr.RequestId
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
GO
PRINT 'Created [pCmsCaseRequestList]'
GO