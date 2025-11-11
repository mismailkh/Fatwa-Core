using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.Consultation;
using FATWA_DOMAIN.Models.Contact;
using FATWA_DOMAIN.Models.ViewModel.ContactManagmentVMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Interfaces.ContactManagment
{
    public interface ICNTContact
    {
        Task<List<ContactListVM>> GetContactList(ContactAdvanceSearchVM advanceSearchVM);
        Task<ContactDetailVM> GetContactDetailById(Guid contactId);
        Task<bool> DeleteContact(ContactListVM contact);
        Task<List<ContactCaseConsultationListVM>> GetCaseListByContactId(Guid contactId);
        Task<List<ContactCaseConsultationListVM>> GetConsultationListByContactId(Guid contactId);
        Task<List<ContactCaseConsultationRequestListVM>> GetConsultationRequestListByContactId(Guid contactId);
        Task<List<ContactCaseConsultationRequestListVM>> GetCaseRequestListByContactId(Guid contactId);


        Task<List<CaseFile>> GetCaseFileList();
        Task<List<ConsultationFile>> GetConsultationFileList();
        Task<List<CaseRequest>> GetCaseRequestList();
        Task<List<ConsultationRequest>> GetConsultationRequestList();
        Task<bool> CreateContact(CntContact args);
        Task<bool> CheckEmailExists(Guid contactId, string email);
		Task<bool> CheckPhoneNumberExists(Guid contactId, string phoneNumber);
		Task<bool> CheckCivilIdExists(Guid contactId, string civilId);
        Task<CntContact> GetContactDetailByUsingContactId(Guid contactId);
		Task<bool> UpdateContact(CntContact args);



	}
}
