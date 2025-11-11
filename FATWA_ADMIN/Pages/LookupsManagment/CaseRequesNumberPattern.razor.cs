using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace G2G_WEB.Pages.Consultation
{
    public partial class CaseRequesNumberPattern : ComponentBase
    {
        [Inject]
        protected NavigationManager navigationManager { get; set; }
        #region Redirect Functions
        public void GoBackDashboard()
        {
            navigationManager.NavigateTo("/dashboard");
        }
        public void GoBackHomeScreen()
        {
            navigationManager.NavigateTo("/index");
        }
        #endregion
    }
}
