using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models;
using AutoMapper;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.ViewModel.Shared;
using FATWA_DOMAIN.Models.Consultation;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;

namespace FATWA_API.Helpers
{
	public class AutoMapperProfile : Profile
	{
		public AutoMapperProfile()
		{
			CreateMap<CaseRequest, UpdateEntityStatusVM>();
			CreateMap<CmsDraftedTemplateSection, CaseTemplateSectionsVM>();
			CreateMap<CmsDraftedTemplateSectionParameter, CaseTemplateParametersVM>();
			CreateMap<CmsDraftedTemplate, CmsDraftedTemplateVM>();
            CreateMap<UpdateEntityHistoryVM, CmsCaseRequestHistory>().ReverseMap();
            CreateMap<UpdateEntityHistoryVM, CmsCaseFileStatusHistory>().ReverseMap();
			CreateMap<UpdateEntityHistoryVM, ComsConsultationRequestHistory>().ReverseMap();
			CreateMap<UserPersonalInformation, UserPersonalInformationVM>().ReverseMap();
			CreateMap<ImportEmployeeTemplate, UserPersonalInformation>().ReverseMap();
			CreateMap<ImportEmployeeTemplate, UserEmploymentInformation>().ReverseMap();
			CreateMap<ImportEmployeeTemplate, UserEmploymentInformation>().ReverseMap();
			CreateMap<CmsRegisteredCase, CmsMojRPAPayloadVM>().ReverseMap();
			CreateMap<CasePartyLink, CasePartyRPA>().ReverseMap();
			CreateMap<Hearing, HearingRPA>().ReverseMap();
			CreateMap<OutcomeHearing, OutcomeHearingRPA>().ReverseMap();
			CreateMap<Judgement, JudgementRPA>().ReverseMap();
			CreateMap<CmsJudgmentExecution, ExecutionRPA>().ReverseMap();
			CreateMap<ExecutionAnouncement, ExecutionAnnouncementProcedureRPA>().ReverseMap();
			CreateMap<CaseAnnouncement, AnnouncementRPA>().ReverseMap();
			CreateMap<Judgement,JudgementVM> ().ReverseMap();
			CreateMap<CasePartyLinkVM, CasePartyLink> ().ReverseMap();
        }
	}
}