using FATWA_WEB.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace FATWA_WEB.Services
{
    public partial class AdvancedSearchsService
    {
        private readonly ProtectedBrowserStorage BrowserStorage;
        private readonly IConfiguration _config;
        private readonly NavigationManager navigationManager;
        private readonly LoginState loginState;

        public AdvancedSearchsService(ProtectedLocalStorage _browserStorage, IConfiguration config, NavigationManager _navigationManager, LoginState _loginstate)
        {
            BrowserStorage = _browserStorage;
            _config = config;
            navigationManager = _navigationManager;
            loginState = _loginstate;

        }
    }
}
