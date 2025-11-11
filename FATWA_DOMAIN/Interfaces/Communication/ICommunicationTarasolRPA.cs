using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.CommunicationModels;
using FATWA_DOMAIN.Models.ViewModel.CommunicationVMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Interfaces.Communication
{
    public interface ICommunicationTarasolRPA
    {
        Task<SendCommunicationVM> AddCommunicationData(CommunicationTarasolRPAPayload communication);
        Task AddFaultyCommunicationData(CommunicationTarasolRpaPayload payload);
        Task<List<CommunicationTarasolMarkedCorrespondencesVM>> GetCorrespondences();
        Task<SendCommunicationVM> SendTarassolCommunication(SendCommunicationVM communication);
        Task<GovernmentEntity> GetGovernmentEntitiyById(int Id);
        Task<GEDepartments> GetGEDepartmentById(int Id);
        Task<TarasolRPAPayloadWithDocuments> GetRPAFaultyMessages();
        Task<int> GetGovernmentEntitiyByReferenceAndSubmoduleId(Guid referenceId, int subModuleId);
        Task<FATWA_DOMAIN.Models.CommunicationModels.Communication> GetCommunicationByCommunicationId(Guid CommunicationId);


    }
}
