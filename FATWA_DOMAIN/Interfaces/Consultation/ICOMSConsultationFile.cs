using FATWA_DOMAIN.Models.Consultation;
using FATWA_DOMAIN.Models.ViewModel.ConsultationVMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Interfaces.Consultation
{
    public interface ICOMSConsultationFile
    {
        Task<List<ConsultationFileListVM>> GetConsultationFileList(AdvanceSearchConsultationCaseFile advanceSearchVM);
        Task<List<ConsultationFileHistoryVM>> GetConslutationFileStatusHistory(Guid FileId);
         Task<List<ConsultationFileHistoryVM>> GetConslutationFileStatusHistoryForList(Guid FileId);
        Task<ConsultationFileDetailVM> GetConsultationFileDetailById(Guid FileId);
        Task<List<ConsultationFileAssignmentVM>> GetConsultationAssigneeList(Guid FileId);
        Task<List<ConsultationFileAssignmentHistoryVM>> GetConsultationAssigmentHistory(Guid referenceId);
        Task<ConsultationFile> GetConsultationFile(Guid fileId);
        Task<ConsultationFile> ConsultationFileDetailWithPartiesAndAttachments(Guid fileId);
        Task<List<ConsultationFileListDmsVM>> GetAllConsultationFileListBySectorTypeId(int sectorTypeId,string userId);
        Task<List<ConsultationAssignment>> GetConsultationAssigment(Guid referenceId);
        Task<List<ConsultationDraftedRequestListVM>> GetDraftedConsultationRequestList(AdvanceSearchConsultationRequestVM advanceSearchConsultationRequestVM);

    }
}
