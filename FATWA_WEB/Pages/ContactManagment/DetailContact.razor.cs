using FATWA_DOMAIN.Models.ViewModel.ContactManagmentVMs;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;

namespace FATWA_WEB.Pages.ContactManagment
{
    public partial class DetailContact : ComponentBase
    {
        #region paramater
        [Parameter]
        public dynamic ContactId { get; set; }
        [Parameter]
        public dynamic File { get; set; }
        [Parameter]
        public dynamic SectorId { get; set; }
        [Parameter]
        public dynamic Module { get; set; }

        #endregion

        #region Variables declaration

        public ContactDetailVM contactDetailVM = new ContactDetailVM();
        protected RadzenDataGrid<ContactCaseConsultationListVM> CaseGrid;
        protected RadzenDataGrid<ContactCaseConsultationRequestListVM> RequestGrid;
        public List<ContactCaseConsultationListVM> contactCaseList = new List<ContactCaseConsultationListVM>();
        public List<ContactCaseConsultationListVM> contactConsultationList = new List<ContactCaseConsultationListVM>();
        public List<ContactCaseConsultationListVM> contactConsultationCaseList = new List<ContactCaseConsultationListVM>();
        public List<ContactCaseConsultationRequestListVM> contactConsultationRequestList = new List<ContactCaseConsultationRequestListVM>();
        public List<ContactCaseConsultationRequestListVM> contactCaseRequestList = new List<ContactCaseConsultationRequestListVM>();
        public List<ContactCaseConsultationRequestListVM> contactCaseConsultationRequestList = new List<ContactCaseConsultationRequestListVM>();
        protected string RedirectURL { get; set; }
        public string TransKeyHeader = string.Empty;
        #endregion

        #region On Load Component
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await SetBreadCrumbRedirection();
            var result = await CntContactService.GetContactDetailById(Guid.Parse(ContactId));
            if (result.IsSuccessStatusCode)
            {
                contactDetailVM = (ContactDetailVM)result.ResultData;
                await PopulateCases();
                await PopulateConsultations();
                await PopulateCaseRequests();
                await PopulateConsultationRequest();
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(result);
            }

            spinnerService.Hide();
        }

        private async Task SetBreadCrumbRedirection()
        {
            try
            {
                int module = Convert.ToInt32(Module);
                if (module == (int)WorkflowModuleEnum.CaseManagement)
                {
                    RedirectURL = "/casefile-view/" + Guid.Parse(File);
                    TransKeyHeader = "View_Details";
                }
                else if (module == (int)WorkflowModuleEnum.CNTContactManagement)
                {
                    RedirectURL = "/contact-list";
                    TransKeyHeader = "Contact_List";
                }
                else if (module == (int)WorkflowModuleEnum.COMSConsultationManagement)
                {
                    RedirectURL = "/consultationfile-view/" + Guid.Parse(File) + "/" + Convert.ToInt32(SectorId);
                    TransKeyHeader = await PopulateTransationKey();
                }
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }

        private async Task<string> PopulateTransationKey()
        {
            try
            {
                int sectorId = Convert.ToInt32(SectorId);
                string pathName = string.Empty;
                if (sectorId == (int)OperatingSectorTypeEnum.Contracts)
                {
                    pathName = "Contracts_File";
                }
                else if (sectorId == (int)OperatingSectorTypeEnum.LegalAdvice)
                {
                    pathName = "Legal_Advice_File";
                }
                else if (sectorId == (int)OperatingSectorTypeEnum.InternationalArbitration)
                {
                    pathName = "International_Arbitration_File";
                }
                else if (sectorId == (int)OperatingSectorTypeEnum.AdministrativeComplaints)
                {
                    pathName = "Subject";
                }
                else if (sectorId == (int)OperatingSectorTypeEnum.Legislations)
                {
                    pathName = "List_Legislations_File";
                }
                return pathName;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Populate Case And Consultation
        protected async Task PopulateCases()
        {
            var result = await CntContactService.GetCaseListByContactId(Guid.Parse(ContactId));
            if (result.IsSuccessStatusCode)
            {
                contactCaseList  = (List<ContactCaseConsultationListVM>)result.ResultData;
                contactConsultationCaseList = contactCaseList;
               
            }
        }
        protected async Task PopulateConsultations()
        {
            var result = await CntContactService.GetConsultationListByContactId(Guid.Parse(ContactId));
            if (result.IsSuccessStatusCode)
            {
                contactConsultationList = (List<ContactCaseConsultationListVM>)result.ResultData;

                if (contactConsultationCaseList is not null)
                {
                    contactConsultationCaseList = new List<ContactCaseConsultationListVM>(contactConsultationCaseList?.Concat(contactConsultationList).ToList());
                }
                else
                {
                    contactConsultationCaseList = contactConsultationList;
                }
            }
        }
        protected async Task PopulateCaseRequests()
        {
            var result = await CntContactService.GetCaseRequestListByContactId(Guid.Parse(ContactId));
            if (result.IsSuccessStatusCode)
            {
                contactCaseRequestList = (List<ContactCaseConsultationRequestListVM>)result.ResultData;
                contactCaseConsultationRequestList = contactCaseRequestList;

            }
        }
        protected async Task PopulateConsultationRequest()
        {
            var result = await CntContactService.GetConsultationRequestListByContactId(Guid.Parse(ContactId));
            if (result.IsSuccessStatusCode)
            {
                contactConsultationRequestList = (List<ContactCaseConsultationRequestListVM>)result.ResultData;

                if (contactCaseConsultationRequestList is not null)
                {
                    contactCaseConsultationRequestList = new List<ContactCaseConsultationRequestListVM>(contactCaseConsultationRequestList?.Concat(contactConsultationRequestList).ToList());
                }
                else
                {
                    contactCaseConsultationRequestList = contactConsultationRequestList;
                }
            }
        }
        #endregion

        #region Redirect Function
        protected async Task RedirectBack()
        {
            await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
        }
        private void GoBackDashboard()
        {
            navigationManager.NavigateTo("/dashboard");
        }
       

        private void GoBackHomeScreen()
        {
            navigationManager.NavigateTo("/index");
        }
        #endregion

        #region Delete existing contact
        protected async Task DeleteContact()
        {
            if (await dialogService.Confirm(translationState.Translate("Sure_Delete_The_Record"), translationState.Translate("Confirm"), new ConfirmOptions()
            {
                OkButtonText = translationState.Translate("OK"),
                CancelButtonText = translationState.Translate("Cancel")
            }) == true)
            {
                ContactListVM Obj = new ContactListVM()
                {
                    ContactId = Guid.Parse(ContactId),
                    Name = contactDetailVM?.FirstName,
                    PhoneNumber = contactDetailVM?.PhoneNumber
                };
                var response = await CntContactService.DeleteContact(Obj);
                if (response.IsSuccessStatusCode)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Success,
                        Detail = translationState.Translate("Deleted_Successfully"),
                        Style = "position: fixed !important; left: 0; margin: auto;"
                    });
                    await JSRuntime.InvokeVoidAsync("history.back");
                }
                else
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Error,
                            Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                    }
                }
            }
        }
        #endregion
    }
}
