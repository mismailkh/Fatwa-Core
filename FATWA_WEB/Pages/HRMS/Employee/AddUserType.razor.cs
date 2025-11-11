using DocumentFormat.OpenXml.Drawing;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_WEB.Pages.CaseManagment.Shared;
using Microsoft.AspNetCore.Components;
using Radzen;
using System;
using System.Collections.Generic;
using System.Linq;
using static FATWA_DOMAIN.Enums.UserEnum;
using System.Reflection.Metadata;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using Radzen.Blazor;
using FATWA_DOMAIN.Models.CaseManagment;
using static FATWA_GENERAL.Helper.Response;
using Group = FATWA_DOMAIN.Models.AdminModels.UserManagement.Group;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components.Web;
using static FATWA_WEB.Pages.CreateUser;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Enums;

namespace FATWA_WEB.Pages.HRMS.Employee
{
    public partial class AddUserType
    {
        UserEnum UserEnum = new UserEnum();
        IEnumerable<EmployeeType> employeeTypes { get; set; }
        int employeetypeId;


        protected override async void OnInitialized()
        {
            await GetEmployeeType();
        }
        public async Task GetEmployeeType()
        {
            var response = await userService.GetEmployeeType();
            if (response.IsSuccessStatusCode)
            {
                employeeTypes = (IEnumerable<EmployeeType>)response.ResultData;
                employeeTypes = employeeTypes.Where(x => x.Id < 3).ToList();
            }
            else
            {
                /*  await invalidRequestHandlerService.ReturnBadRequestNotification(response);*/
            }
            StateHasChanged();
        }
        public async Task SubmitChanges()
        {
            if (true)
            {
                spinnerService.Show();

                dialogService.Close(employeetypeId);
                spinnerService.Hide();
            }
            else
            {
                dialogService.Close();
            }
        }

        protected void ButtonCancelClick(MouseEventArgs args)
        {
            dialogService.Close(null);
        }
        public UserEnum userEnum;
    }
}
