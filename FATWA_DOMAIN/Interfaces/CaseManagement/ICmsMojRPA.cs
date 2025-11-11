using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FATWA_DOMAIN.Models.ViewModel.MojRollsVM;

namespace FATWA_DOMAIN.Interfaces.CaseManagement
{
    //<!-- <History Author = 'Hassan Abbas' Date='2024-02-01' Version="1.0" Branch="master">Interface For Cases Data Migration from MOJ</History> -->
    public interface ICmsMojRPA
    {
        Task<CmsMojRPACaseData> AddCaseData(CmsMojRPAPayloadVM executionRequest);
        Task<List<MojUnassignedCaseFileVM>> GetUnassignedMigratedCaseFilesList(AdvanceSearchCmsCaseFileVM advanceSearch);
        Task<AssignMojCaseFileToSectorVM> AssignUnassignedFilesToSector(AssignMojCaseFileToSectorVM sectorAssignment);
        Task<List<CanAndCaseNumber>> AddHearingData(List<CanAndCaseNumber> mojRPAHearingRolls, DateTime HearingDate,int DocumentId);
        Task<List<CanAndCaseNumber>> AddOutcomeHearingData(List<CanAndCaseNumber> mojRPAHearingRolls ,int DocumentId ,DateTime HearingDate,DateTime? NextHearingDate);
        Task AssignHearingRollsToLawyer(AssignHearingRollToLawyerVM hearingRollToLawyerVM);
        Task<List<CmsPrintHearingRollDetailVM>> GetHearingRollDetailForPrintingAndOutcome(CmsHearingRollDetailSearchVM cmsHearingRollDetailSearch);
        Task SaveHearingRollOutcomesAsDraft(CmsHearingRollOutcomeDraftPayload payload);
        Task SaveHearingRollOutcomes(List<CmsPrintHearingRollDetailVM> hearings);
    }
}
