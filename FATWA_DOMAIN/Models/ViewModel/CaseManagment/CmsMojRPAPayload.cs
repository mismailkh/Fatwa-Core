using FATWA_DOMAIN.Models.BaseModels;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.Notifications.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.CaseManagment
{
    //< History Author = 'Hassan Abbas' Date = '2024-02-01' Version = "1.0" Branch = "master" > Cms Moj Request Payload </History>
    public class CmsMojRPAPayloadVM
    {    //to begenerated in RPA Guid.NewGuid() and be passed same in child objects
        [Required]
        public Guid CaseId { get; set; }
        [Required]
        public string CANNumber { get; set; }
        [Required]
        public string CaseNumber { get; set; }
        [Required]
        public DateTime CaseDate { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string CourtCode { get; set; }
        [Required]
        public string ChamberCode { get; set; }
        [Required]
        public string ChamberNumber { get; set; }
        //Currently missing - also check with Govt Entities data in Core System
        //Lookup table -> CMS_GOVERNMENT_ENTITY_G2G_LKP
        [Required]
        public int GovtEntityId { get; set; }
        public int CourtId { get; set; }
        public int ChamberId { get; set; }
        public int ChamberNumberId { get; set; }
        //Currently missing
        public List<CasePartyRPA> CaseParties { get; set; } = new List<CasePartyRPA>();
        public List<HearingRPA> Hearings { get; set; } = new List<HearingRPA>();
        public List<ExecutionRPA> Executions { get; set; } = new List<ExecutionRPA>();
        public List<AnnouncementRPA> Announcements { get; set; } = new List<AnnouncementRPA>();
        [NotMapped]
        public NotificationParameter NotificationParameter { get; set; } = new NotificationParameter();

    }


    //< History Author = 'Hassan Abbas' Date = '2024-10-31' Version = "1.0" Branch = "master" > Cms Moj Case Data Sync View Model </History>
    public class CmsMojRPACaseData
    {
        [NotMapped]
        public CaseRequest CaseRequest { get; set; }
        [NotMapped]
        public CaseFile CaseFile { get; set; }
        [NotMapped]
        public CmsCaseFileStatusHistory FileStatusHistory { get; set; }
        [NotMapped]
        public List<GovernmentEntityRepresentative> GovernmentEntityRepresentatives { get; set; } = new List<GovernmentEntityRepresentative>();
        [NotMapped]
        public List<CasePartyLink> CasePartyLinks { get; set; } = new List<CasePartyLink>();
        [NotMapped]
        public CmsRegisteredCase RegisteredCase { get; set; }
        [NotMapped]
        public List<Hearing> Hearings { get; set; } = new List<Hearing>();
        [NotMapped]
        public List<OutcomeHearing> OutcomeHearings { get; set; } = new List<OutcomeHearing>();
        [NotMapped]
        public List<JudgementType> JudgementTypes { get; set; } = new List<JudgementType>();
        [NotMapped]
        public List<JudgementCategory> JudgementCategories { get; set; } = new List<JudgementCategory>();
        [NotMapped]
        public List<Judgement> Judgements { get; set; } = new List<Judgement>();
        [NotMapped]
        public List<ExecutionPartyLink> ExecutionPartyLinks { get; set; } = new List<ExecutionPartyLink>();
        [NotMapped]
        public List<CmsJudgmentExecution> CmsJudgmentExecutions { get; set; } = new List<CmsJudgmentExecution>();
        [NotMapped]
        public List<ExecutionAnouncement> ExecutionAnouncements { get; set; } = new List<ExecutionAnouncement>();
        [NotMapped]
        public List<CaseAnnouncement> CaseAnnouncements { get; set; } = new List<CaseAnnouncement>();
        [NotMapped]
        public CmsRegisteredCaseStatusHistory CaseStatusHistory { get; set; }
    }

    public class HearingRPA
    {
        //to be generated in RPA Guid.NewGuid()
        [Required]
        public Guid HearingId { get; set; }
        [Required]
        public DateTime HearingDate { get; set; }
        public DateTime? NextHearingDate { get; set; }
        //lookup table -> CMS_HEARING_STATUS_G2G_LKP
        [Required]
        public int StatusId { get; set; }
        public List<OutcomeHearingRPA> OutcomeHearing { get; set; } = new List<OutcomeHearingRPA>();
    }
    public class OutcomeHearingRPA
    {
        //to be generated in RPA Guid.NewGuid() and be passed same in child objects
        [Required]
        public Guid OutcomeId { get; set; }
        //passed from parent(Hearing)
        [Required]
        public DateTime HearingDate { get; set; }
        //Court Code + Court Decision concatinated
        public string? Remarks { get; set; }
        public List<JudgementRPA> Judgements { get; set; } = new List<JudgementRPA>();
    }
    public class JudgementRPA
    {
        //to be generated in RPA Guid.NewGuid() and be passed same in child objects
        [Required]
        public Guid JudgementId { get; set; }
        //passed from parent(Hearing)
        [Required]
        public DateTime HearingDate { get; set; }
        [Required]
        public DateTime JudgementDate { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public string Category { get; set; }
        //Lookup table -> CMS_JUDGEMENT_STATUS_G2G_LKP
        [Required]
        public int StatusId { get; set; }
        //Always being returned NULL in RPA
        [Required]
        public Double Amount { get; set; }
        //Currently Missing
        [Required]
        public Double AmountCollected { get; set; }
        //Judgement Statement
        public string? Remarks { get; set; }
        //Passed true by RPA if Latest Judgement
        [Required]
        public bool IsFinal { get; set; }
    }
    public class CasePartyRPA
    {
        //to be generated in RPA Guid.NewGuid() and be passed same in child objects
        [Required]
        public Guid Id { get; set; }//
                                    //the current extracted value in RPA as PartyTypeId, the PartyType in the Core system is considered as Party Category i.e. (Defendent, Plaintiff))
                                    //Lookup table -> CMS_CASE_PARTY_CATEGORY_G2G_LKP
        [Required]
        public int CategoryId { get; set; }//
                                           //the current values for TypeId in core system are (Individual, Company, Govt Entity)
                                           //Lookup table -> CMS_CASE_PARTY_TYPE_G2G_LKP
        [Required]
        public int TypeId { get; set; }
        public string? Name { get; set; }
        //Govt Entity Id if Party Type is GE, this value is also used as a relation between GE and Representative
        //lOOKUP TABLE ->CMS_GOVERNMENT_ENTITY_G2G_LKP
        public int? GovtEntityId { get; set; }
        //Representative Code in Core System, will be unique for an induvidual representative but will be same if one representative exist as a party in another case as well
        public string? RepresentativeNumber { get; set; }
        //Currently Missing
        public string? RepresentativeNameEn { get; set; }
        //Currently Missing
        public string? RepresentativeNameAr { get; set; }
    }

    public class ExecutionRPA
    {
        //to be generated in RPA Guid.NewGuid() and be passed same in child objects
        [Required]
        public Guid ExecutionId { get; set; }
        public string? ExecutorNumber { get; set; }
        public string? ExecutionFileNumber { get; set; }
        //Currently the Status Name is extracted by RPA but if not possible to send the Id, will set it Closed by default in core system
        //Lookup table -> CMS_EXECUTION_STATUS_G2G_LKP
        [Required]
        public int FileStatusId { get; set; }
        public string? Remarks { get; set; }
        public DateTime? FileOpeningDate { get; set; }
        //the RPA will send the relevent Amount data and not for the repeated rows
        public decimal? Amount { get; set; }
        //the RPA will send the relevent Amount data and not for the repeated rows
        public decimal? PaidAmount { get; set; }
        //in RPA the name is extracted but it should be the PartyId from parent(Party)
        public Guid? PayerId { get; set; }
        //If Payer Name not matched with the existing parties, send the PayerId = null and payer name and it will stored in new table
        public string? MismatchedPayerName { get; set; }
        //in RPA the name is extracted but it should be the PartyId from parent(Party)
        public Guid? ReceiverId { get; set; }
        //If Reciever Name not matched with the existing parties, send the ReceiverId = null and reciever name and it will stored in new table
        public string? MismatchedRecieverName { get; set; }
        public List<ExecutionAnnouncementProcedureRPA> ExecutionAnouncements { get; set; } = new List<ExecutionAnnouncementProcedureRPA>();
    }

    //Will be New Entity in Core System
    public class ExecutionAnnouncementProcedureRPA
    {
        [Required]
        public Guid Id { get; set; }
        //No Lookup exist in Core System, data will need to be shared
        [Required]
        public int? AnouncementStatusId { get; set; }
        //No Lookup exist in Core System, data will need to be shared
        [Required]
        public int? AnouncementTypeId { get; set; }
        public string? PersonToBeanounced { get; set; }
        public DateTime? ProcedureDate { get; set; }
    }

    //Will be New Entity in Core System
    public class AnnouncementRPA
    {
        [Required]
        public Guid? PartyId { get; set; }
        [Required]
        public int? AnouncementNumber { get; set; }
        //No Lookup exist in Core System, data will need to be shared
        public string? AnouncementType { get; set; }
        public string? PersonToBeanounced { get; set; }
        public DateTime? HearingDate { get; set; }
        //No Lookup exist in Core System, data will need to be shared
        public string? DistributionStatus { get; set; }
        public DateTime? AnouncementMakingDate { get; set; }
        public DateTime? AnouncementGoOutDate { get; set; }
        public DateTime? ActualAnouncementDate { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public string? AnouncementResult { get; set; }
        public string? Reason { get; set; }
    }

    public class CaseDocumentsRPA
    {
        //passed from parent(CaseId)
        public Guid CaseId { get; set; }
        //Lookup already exists in Core system FT_DMS DB and the value should be relevent to Case Management Module which is (ModuleId:5)
        public int AttachmentTypeId { get; set; }
        //Just the name will be passed not the path if the document will be uploaded
        public string FileName { get; set; }
        public DateTime DocumentDate { get; set; }
    }
}
