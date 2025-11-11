using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;

namespace FATWA_WEB.Pages.HRMS.Employee
{
    //<History Author='Ammaar Naveed' Date='30-01-2024'>Employee Contact Information -> Multiple Contacts</History>//
    public partial class AddContact : ComponentBase
    {
        [Parameter] public string UserId { get; set; }
        [Parameter] public Guid Id { get; set; }
        [Parameter] public string ContactNumber { get; set; }
        [Parameter] public int ContactType { get; set; }
        [Parameter] public bool IsPrimary { get; set; }

        UserContactInformation UserContactInformation = new UserContactInformation();
        ContactType EmployeeContactTypes = new ContactType();
        AddEmployeeVM EmployeeVM = new AddEmployeeVM();

        protected IEnumerable<ContactType> ContactTypes { get; set; }

        protected override async void OnInitialized()
        {
            await GetContactTypes();
            if (ContactType != null && ContactNumber != null)
            {
                UserContactInformation.ContactTypeId = ContactType;
                UserContactInformation.ContactNumber = ContactNumber;
                UserContactInformation.IsPrimary = IsPrimary;   
            }
            StateHasChanged();
        }

        protected async Task GetContactTypes()
        {
            var response = await userService.GetContactTypes();
            if (response.IsSuccessStatusCode)

            {
                ContactTypes = (IEnumerable<ContactType>)response.ResultData;
            }
            else
            {
                //await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }

        public async Task SubmitChnages()
        {
            if (UserContactInformation.ContactNumber != null && UserContactInformation.ContactTypeId != null)
            {
                spinnerService.Show();
                if (UserContactInformation.Id == Guid.Empty)
                {
                    UserContactInformation.Id = Guid.NewGuid();
                }
                if (UserContactInformation.ContactTypeId != null)
                {
                    UserContactInformation.ContactType = ContactTypes.Where(x => x.Id == UserContactInformation.ContactTypeId).FirstOrDefault();
                }
                dialogService.Close(UserContactInformation);
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
    }
}
