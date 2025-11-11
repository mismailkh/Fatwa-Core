using Blazored.LocalStorage;

namespace FATWA_WEB.Services.ServiceRequestService.HiringManagement
{
    public class HiringRequestService
    {
        #region variables declaration
        private readonly IConfiguration _config;
        private readonly ILocalStorageService browserStorage;

        #endregion

        #region Constructor
        public HiringRequestService(IConfiguration config, ILocalStorageService _browserStorage)
        {
            _config = config;
            browserStorage = _browserStorage;
        }
        #endregion
    }
}
