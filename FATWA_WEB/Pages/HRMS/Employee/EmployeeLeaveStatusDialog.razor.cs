using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using Radzen;
using Radzen.Blazor;

namespace FATWA_WEB.Pages.HRMS.Employee
{
    //<History Author = 'Ammaar Naveed' Date='2024-04-29' Version="1.0" Branch="master">Leave check response dialog</History>
    public partial class EmployeeLeaveStatusDialog : ComponentBase
    {
       
        protected override async void OnInitialized()
        {
        }
        private async Task ButtonCloseClick()
        {
            dialogService.Close();
        }
    }
}
