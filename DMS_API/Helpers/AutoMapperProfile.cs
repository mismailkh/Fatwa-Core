using AutoMapper;
using DSPExternalAuthenticationService;
using FATWA_DOMAIN.Models.DigitalSignature;

namespace DMS_API.Helpers
{
	public class AutoMapperProfile : Profile
	{
		public AutoMapperProfile()
		{
			CreateMap<AuthenticateRequestVM, AuthenticateRequest>();
        }
	}
}