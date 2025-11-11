using FATWA_DOMAIN.Models.AdminModels.CaseManagment;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.Contact;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.Meet;
using System.Collections.ObjectModel;
using FATWA_DOMAIN.Models.ViewModel.CommunicationVMs;
using FATWA_DOMAIN.Models.ViewModel.MojRollsVM;
using FATWA_DOMAIN.Models.ViewModel.LLSLegalPrinciple;
using FATWA_DOMAIN.Models.ViewModel.DmsVMs;
using FATWA_DOMAIN.Models.ViewModel.OrganizingCommitteeVMs;
using FATWA_DOMAIN.Models.Consultation;

namespace FATWA_WEB.Data
{
    //<History Author = 'Hassan Abbas' Date='2022-10-06' Version="1.0" Branch="master"> Added State to pass or transfer data between components</History>
    public class DataCommunicationState
    {
        public CaseRequest caseRequest { get; set; }
        public ConsultationRequest consultationRequest { get; set; }
        public CmsCaseFileDetailVM caseFile { get; set; } = new CmsCaseFileDetailVM();
        public List<CmsRegisteredCaseVM> registeredCases { get; set; }
        public CmsRegisteredCaseDetailVM registeredCase { get; set; }
        public DraftEntityDataVM draftEntityData { get; set; } = new DraftEntityDataVM();
        public SaveMeetingVM saveMeetingVM { get; set; } = new SaveMeetingVM();
        public ObservableCollection<TempAttachementVM> TempFiles { get; set; }
        public CntContact cntContact { get; set; }
        public SendCommunicationVM sendCommunicationVM { get; set; }
        public OutcomeHearing outcomeHearing { get; set; }
        public MOJRollsRequestListVM hearingRollRequest { get; set; }
        public LLSLegalPrincipleDocumentVM selectedJudmentDocumentsList { get; set; } = new LLSLegalPrincipleDocumentVM();
        public LLSLegalPrinciplLegalAdviceDocumentVM selectedLegalAdviceDocument { get; set; } = new LLSLegalPrinciplLegalAdviceDocumentVM();
        public LLSLegalPrincipleKuwaitAlYoumDocuments selectedKuwaitAlYawmDocument { get; set; } = new LLSLegalPrincipleKuwaitAlYoumDocuments();
        public LLSLegalPrinciplOtherDocumentVM selectedOtherDocument { get; set; } = new LLSLegalPrinciplOtherDocumentVM();
        public SaveCommitteeVM saveCommitteeVM { get; set; } = new SaveCommitteeVM();

        public int? selectedLegalPrincipleDocumentTab { get; set; }
    }
}
