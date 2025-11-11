using FATWA_DOMAIN.Models.AdminModels.CaseManagment;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;

namespace DMS_WEB.Data
{
    //<History Author = 'Hassan Abbas' Date='2022-10-06' Version="1.0" Branch="master"> Added State to pass or transfer data between components</History>
    public class DataCommunicationState
    {
        public CaseRequest caseRequest { get; set; }
        public CmsCaseFileDetailVM caseFile { get; set; }
        public List<CmsRegisteredCaseVM> registeredCases { get; set; }
        public DraftEntityDataVM draftEntityData { get; set; } = new DraftEntityDataVM();
    }
}
