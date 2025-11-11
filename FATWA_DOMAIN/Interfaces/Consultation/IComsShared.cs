using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.Consultation;
using FATWA_DOMAIN.Models.ViewModel.ConsultationVMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Interfaces.Consultation
{
    //<!-- <History Author = 'Muhammad Zaeem' Date='2023-1-9' Version="1.0" Branch="master">Shared Interface For Case Management</History> -->
    public interface IComsShared
    {
        Task SaveApprovalTrackingProcess(CmsApprovalTracking approvalTracking);
        Task<Guid?> AssignConsultationToLawyer(ConsultationAssignment consultationfileAssignment);
        Task ApproveTransferComsSector(dynamic Item, int TransferConsultationType);
        Task RejectTransferComsSector(dynamic Item, int TransferConsultationType);
        Task<CmsApprovalTracking> GetApprovalTrackingProcess(Guid referenceId, int sectorTypeId, int processTypeId);
        Task ApproveSendACopy(dynamic Item, int TransferCaseType);
        Task RejectSendACopy(dynamic Item, int TransferCaseType);
        Task<ConsultationFileHistory> ApproveConsultationFile(ConsultationFileDetailVM Item);

        Task<ConsultationAssignment> GetConsultationAssigmentByLawyerIdAndFileId(Guid FileId, string UserId);
        Task AssignConsultationBackToHos(CmsAssignCaseFileBackToHos caseAssignment);
        Task<CmsAssignCaseFileBackToHos> GetSendBackToHosByReferenceId(Guid ReferenceId, string LawyerId);


    }
}
