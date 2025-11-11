using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.Consultation;
using FATWA_DOMAIN.Models.Contact;
using FATWA_DOMAIN.Models.WorkflowModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using static FATWA_DOMAIN.Enums.Contact.ContactManagementEnum;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;

namespace FATWA_WEB.Pages.ContactManagment
{
    public partial class SelectContactLinkPopup : ComponentBase
	{
		#region Constructor
		public SelectContactLinkPopup()
		{
			CntContactRequests = new CntContactFileLink();
			ModuleList = new List<Module>();
			CaseFileList = new List<CaseFile>();
			ConsultationFileList = new List<ConsultationFile>();
			CaseRequestList = new List<CaseRequest>();
			ConsultationRequestList = new List<ConsultationRequest>();
			FileJoins = new List<FileJoin>();
			RequestJoins = new List<RequestJoin>();
            DropdownErrorShow = false;
            FileRequestGridListShow = new List<FileRequestGridList>();
        }
		#endregion

		#region Parameter
		[Parameter]
		public int SelectedContactLinkType { get; set; }
        [Parameter]
        public CntContact CntContacts { get; set; }
        #endregion

        #region Variable declaration
        public CntContactFileLink CntContactRequests { get; set; }
		public List<Module> ModuleList { get; set; }
		public List<CaseFile> CaseFileList { get; set; }
		public List<ConsultationFile> ConsultationFileList { get; set; }
		public List<CaseRequest> CaseRequestList { get; set; }
		public List<ConsultationRequest> ConsultationRequestList { get; set; }
		public List<FileJoin> FileJoins { get; set; }
		public List<RequestJoin> RequestJoins { get; set; }
        public bool DropdownErrorShow { get; set; }
        public List<FileRequestGridList> FileRequestGridListShow { get; set; }
        public RadzenDataGrid<FileRequestGridList>? SignatureGridRef = new RadzenDataGrid<FileRequestGridList>();
		#endregion

		#region Class for File, Request and Grid
		public class FileJoin
		{
			public Guid FileId { get; set; }
			public string FileNumber { get; set; }
			public int ContactLinkId { get; set; }
			public int ContactModuleId { get; set; }
		}
		public class RequestJoin
		{
			public Guid RequestId { get; set; }
			public string RequestNumber { get; set; }
			public int ContactLinkId { get; set; }
			public int ContactModuleId { get; set; }
		}
		public class FileRequestGridList
		{
			public Guid ContactId { get; set; }
			public Guid FileRequestId { get; set; }
			public string FileRequestNumber { get; set; }
			public string FileRequestName { get; set; }
		}
		#endregion

		#region Component OnInitialized
		protected override async Task OnInitializedAsync()
		{
			try
			{
				spinnerService.Show();

				await PopulateDropdowns();
				await ShowDropDownForLinkFiles();

				spinnerService.Hide();
			}
			catch (Exception ex)
			{

			}
		}
		#endregion

		#region Populate Dropdowns Data
		protected async Task PopulateDropdowns()
		{
			await GetModuleList();
			await GetCaseFileList();
			await GetConsultationFileList();
			await GetCaseRequestList();
			await GetConsultationRequestList();
		}

		private async Task GetConsultationRequestList()
		{
			var response = await CntContactService.GetConsultationRequestList();
			if (response.IsSuccessStatusCode)
			{
				ConsultationRequestList = (List<ConsultationRequest>)response.ResultData;
			}
			else
			{
				await invalidRequestHandlerService.ReturnBadRequestNotification(response);
			}
		}

		private async Task GetCaseRequestList()
		{
			var response = await CntContactService.GetCaseRequestList();
			if (response.IsSuccessStatusCode)
			{
				CaseRequestList = (List<CaseRequest>)response.ResultData;
			}
			else
			{
				await invalidRequestHandlerService.ReturnBadRequestNotification(response);
			}
		}

		private async Task GetConsultationFileList()
		{
			var response = await CntContactService.GetConsultationFileList();
			if (response.IsSuccessStatusCode)
			{
				ConsultationFileList = (List<ConsultationFile>)response.ResultData;
			}
			else
			{
				await invalidRequestHandlerService.ReturnBadRequestNotification(response);
			}
		}

		private async Task GetCaseFileList()
		{
			var response = await CntContactService.GetCaseFileList();
			if (response.IsSuccessStatusCode)
			{
				CaseFileList = (List<CaseFile>)response.ResultData;
			}
			else
			{
				await invalidRequestHandlerService.ReturnBadRequestNotification(response);
			}
		}

		private async Task GetModuleList()
		{
			var response = await lookupService.GetModules();
			if (response.IsSuccessStatusCode)
			{
				ModuleList = (List<Module>)response.ResultData;
			}
			else
			{
				await invalidRequestHandlerService.ReturnBadRequestNotification(response);
			}
		}
		#endregion

		#region Show link Files Dropdown
		protected async Task ShowDropDownForLinkFiles()
		{
			if (SelectedContactLinkType == (int)ContactLinkType.File)
			{
				FileJoins = new List<FileJoin>();
				if (CaseFileList.Count() != 0)
				{
					foreach (var item in CaseFileList)
					{
						FileJoin obj = new FileJoin();
						obj.FileId = item.FileId;
						obj.FileNumber = item.FileNumber;
						obj.ContactLinkId = (int)ContactLinkType.File;
						obj.ContactModuleId = (int)WorkflowModuleEnum.CaseManagement;
						FileJoins.Add(obj);
					}
				}
				if (ConsultationFileList.Count() != 0)
				{
					foreach (var item in ConsultationFileList)
					{
						FileJoin obj = new FileJoin();
						obj.FileId = item.FileId;
						obj.FileNumber = item.FileNumber;
						obj.ContactLinkId = (int)ContactLinkType.File;
						obj.ContactModuleId = (int)WorkflowModuleEnum.COMSConsultationManagement;
						FileJoins.Add(obj);
					}
				}
			}
			else if (SelectedContactLinkType == (int)ContactLinkType.Request)
			{
				RequestJoins = new List<RequestJoin>();
				if (CaseRequestList.Count() != 0)
				{
					foreach (var item in CaseRequestList)
					{
						RequestJoin obj = new RequestJoin();
						obj.RequestId = item.RequestId;
						obj.RequestNumber = item.RequestNumber.ToString();
						obj.ContactLinkId = (int)ContactLinkType.Request;
						obj.ContactModuleId = (int)WorkflowModuleEnum.CaseManagement;
						RequestJoins.Add(obj);
					}
				}
				if (ConsultationRequestList.Count() != 0)
				{
					foreach (var item in ConsultationRequestList)
					{
						RequestJoin obj = new RequestJoin();
						obj.RequestId = item.ConsultationRequestId;
						obj.RequestNumber = item.RequestNumber;
						obj.ContactLinkId = (int)ContactLinkType.Request;
						obj.ContactModuleId = (int)WorkflowModuleEnum.COMSConsultationManagement;
						RequestJoins.Add(obj);
					}
				}
			}
		}
        #endregion

        #region Add Contact File Request
        protected async Task AddContactFileRequest(int file)
        {
            if (CntContactRequests.ReferenceId != Guid.Empty)
            {
                CntContactFileLink ObjContactRequest = new CntContactFileLink();
                FileRequestGridList ObjFileRequest = new FileRequestGridList();
                DropdownErrorShow = false;
                if (file == (int)ContactLinkType.File)
                {
                    var resultFile = FileJoins.Where(x => x.FileId == CntContactRequests.ReferenceId).FirstOrDefault();
                    if (resultFile != null)
                    {
                        ObjContactRequest.ContactId = CntContacts.ContactId;
                        ObjContactRequest.ReferenceId = resultFile.FileId;
                        ObjContactRequest.ContactLinkId = resultFile.ContactLinkId;
                        ObjContactRequest.ModuleId = resultFile.ContactModuleId;
                        ObjContactRequest.CreatedBy = await BrowserStorage.GetItemAsync<string>("User");
                        ObjContactRequest.CreatedDate = DateTime.Now;
                        ObjContactRequest.IsDeleted = false;
                        CntContacts.CntContactRequestList.Add(ObjContactRequest);
                        ObjContactRequest = new CntContactFileLink();
                        // fill for grid list
                        ObjFileRequest.ContactId = CntContacts.ContactId;
                        ObjFileRequest.FileRequestId = resultFile.FileId;
                        ObjFileRequest.FileRequestNumber = resultFile.FileNumber;
                        if (resultFile.ContactModuleId == (int)WorkflowModuleEnum.CaseManagement)
                        {
                            ObjFileRequest.FileRequestName = CaseFileList.Where(x => x.FileId == resultFile.FileId).Select(x => x.FileName).FirstOrDefault();
                        }
                        else if (resultFile.ContactModuleId == (int)WorkflowModuleEnum.COMSConsultationManagement)
                        {
                            ObjFileRequest.FileRequestName = ConsultationFileList.Where(x => x.FileId == resultFile.FileId).Select(x => x.FileName).FirstOrDefault();
                        }
                        FileRequestGridListShow.Add(ObjFileRequest);
                        ObjFileRequest = new FileRequestGridList();
                    }
                }
                else if (file == (int)ContactLinkType.Request)
                {
                    var resultRequest = RequestJoins.Where(x => x.RequestId == CntContactRequests.ReferenceId).FirstOrDefault();
                    if (resultRequest != null)
                    {
                        ObjContactRequest.ContactId = CntContacts.ContactId;
                        ObjContactRequest.ReferenceId = resultRequest.RequestId;
                        ObjContactRequest.ContactLinkId = resultRequest.ContactLinkId;
                        ObjContactRequest.ModuleId = resultRequest.ContactModuleId;
                        ObjContactRequest.CreatedBy = await BrowserStorage.GetItemAsync<string>("User");
                        ObjContactRequest.CreatedDate = DateTime.Now;
                        ObjContactRequest.IsDeleted = false;
                        CntContacts.CntContactRequestList.Add(ObjContactRequest);
                        ObjContactRequest = new CntContactFileLink();
                        // fill for grid list
                        ObjFileRequest.ContactId = CntContacts.ContactId;
                        ObjFileRequest.FileRequestId = resultRequest.RequestId;
                        ObjFileRequest.FileRequestNumber = resultRequest.RequestNumber;
                        if (resultRequest.ContactModuleId == (int)WorkflowModuleEnum.CaseManagement)
                        {
                            ObjFileRequest.FileRequestName = CaseRequestList.Where(x => x.RequestId == resultRequest.RequestId).Select(x => x.Subject).FirstOrDefault();
                        }
                        else if (resultRequest.ContactModuleId == (int)WorkflowModuleEnum.COMSConsultationManagement)
                        {
                            ObjFileRequest.FileRequestName = ConsultationRequestList.Where(x => x.ConsultationRequestId == resultRequest.RequestId).Select(x => x.Subject).FirstOrDefault();
                        }
                        FileRequestGridListShow.Add(ObjFileRequest);
                        ObjFileRequest = new FileRequestGridList();
                    }
                }
                CntContactRequests.ReferenceId = Guid.Empty;
                StateHasChanged();
                if (CntContacts.CntContactRequestList.Count() != 0)
                {
                    await SignatureGridRef.Reload();
                    StateHasChanged();
                }
            }
            else
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Fill_Required_Fields"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                DropdownErrorShow = true;
            }
        }
		#endregion

		#region Delete grid data
		protected async Task DeleteGridData(FileRequestGridList args)
		{
			var DeleteObj = args;
			bool? dialogResponse = await dialogService.Confirm(
				translationState.Translate("Delete_File_Request_Grid_Confirm_Message"),
				translationState.Translate("Confirm"),
				new ConfirmOptions()
				{
					OkButtonText = translationState.Translate("OK"),
					CancelButtonText = translationState.Translate("Cancel")
				});
			if (dialogResponse == true)
			{
				// remove from grid list
				FileRequestGridListShow.Remove(DeleteObj);
				var result = CntContacts.CntContactRequestList.Where(x => x.ReferenceId == DeleteObj.FileRequestId).FirstOrDefault();
				if (result != null)
				{
					CntContacts.CntContactRequestList.Remove(result);
				}
				await Task.Delay(200);
				notificationService.Notify(new NotificationMessage()
				{
					Severity = NotificationSeverity.Success,
					Detail = translationState.Translate("Delete_File_Request_Success_Message"),
					Style = "position: fixed !important; left: 0; margin: auto; "
				});
				await Task.Delay(1000);
				await SignatureGridRef.Reload();
			}
		}
		#endregion

		#region Radzen Form Submit
		protected async Task Form0Submit(CntContactFileLink args)
		{
			if (await dialogService.Confirm(translationState.Translate("ContactLink_File_Submit"), translationState.Translate("Confirm"), new ConfirmOptions()
			{
				OkButtonText = translationState.Translate("OK"),
				CancelButtonText = translationState.Translate("Cancel")
			}) == true)
			{
				dialogService.Close(CntContacts.CntContactRequestList);
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
