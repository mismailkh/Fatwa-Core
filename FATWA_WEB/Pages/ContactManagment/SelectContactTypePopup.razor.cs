using FATWA_DOMAIN.Models.Contact;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;

namespace FATWA_WEB.Pages.ContactManagment
{
    public partial class SelectContactTypePopup : ComponentBase
    {
        #region Constructor
        public SelectContactTypePopup()
        {
            CntContacts = new CntContact { ContactId = Guid.NewGuid() };
            CntContactTypeList = new List<CntContactType>();
        }
		#endregion

		#region Parameter
		[Parameter]
		public Guid? FileId { get; set; }
		[Parameter]
		public int? FileModule { get; set; }
		#endregion

		#region Variables
		public CntContact CntContacts { get; set; }
        public List<CntContactType> CntContactTypeList { get; set; }

        #endregion

        #region Component Events

        protected override async Task OnInitializedAsync()
        {
            try
            {
                spinnerService.Show();

                await PopulateDropdowns();

                spinnerService.Hide();
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region Populate Dropdowns records
        protected async Task PopulateDropdowns()
        {
            try
            {
                await GetContactType();
            }
            catch (Exception)
            {
                throw;
            }

        }

        private async Task GetContactType()
        {
            var response = await lookupService.GetContactType();
            if (response.IsSuccessStatusCode)
            {
                CntContactTypeList = (List<CntContactType>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }

        #endregion

        #region Form submit
        protected async Task Form0Submit(CntContact args)
        {
            try
            {
                if (args.ContactTypeId != 0)
                {
                    spinnerService.Show();
                    dataCommunicationService.cntContact = args;
                    if (FileId != null && FileModule != null)
                    {
                        CntContactFileLink ObjFile = new CntContactFileLink();
                        ObjFile.ContactId = args.ContactId;
                        ObjFile.ReferenceId = (Guid)FileId;
                        ObjFile.ContactLinkId = 1;
                        ObjFile.ModuleId = (int)FileModule;
                        ObjFile.CreatedBy = loginState.UserDetail.Email;
                        ObjFile.CreatedDate = DateTime.Now;

                        dataCommunicationService.cntContact.CntContactRequestList.Add(ObjFile);
                    }
                    //dataCommunicationService.caseRequest.AuthorityLetter = uploadedAttachment;
                    dialogService.Close();
                    navigationManager.NavigateTo("/contact-add");
                    spinnerService.Hide();
                }
                else
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Please_Select_Contact_Type"),
                        Summary = translationState.Translate("Error"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                }
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region Form closed
        protected async Task CloseDialog(MouseEventArgs args)
        {
            dialogService.Close(null);
        }
        #endregion

    }
}
