using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models;
using AutoMapper;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.ViewModel.Shared;
using FATWA_DOMAIN.Models.Consultation;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using FATWA_DOMAIN.Models.ViewModel.LLSLegalPrinciple;
using FATWA_DOMAIN.Models.LLSLegalPrinciple;
using FATWA_DOMAIN.Models.ServiceRequestModels.LeaveAndAttendanceRequestModels;
using FATWA_DOMAIN.Models.ViewModel.Lms;
using FATWA_DOMAIN.Models.Lms;

namespace FATWA_WEB.Helpers
{
	public class AutoMapperProfile : Profile
	{
		public AutoMapperProfile()
		{
			
			CreateMap<Judgement,JudgementVM> ().ReverseMap();
			CreateMap<JudgementVM, CmsJudgementDetailVM> ().ReverseMap();
			CreateMap<LLSLegalPrinciplesRelationVM, LLSLegalPrincipleContentSourceDocumentReference> ().ReverseMap();
			CreateMap<LeaveAttendanceRequestDetailVM, LeaveAttendanceRequest>();
			CreateMap<CmsRegisteredCaseTransferRequestVM, CmsRegisteredCaseTransferRequest>().ReverseMap();
			CreateMap<LmsStockTakingListVM, LmsStockTaking>().ReverseMap();
        }
	}
}