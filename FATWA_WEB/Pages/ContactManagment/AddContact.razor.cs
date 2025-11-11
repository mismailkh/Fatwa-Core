using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.Consultation;
using FATWA_DOMAIN.Models.Contact;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.ContactManagmentVMs;
using FATWA_DOMAIN.Models.ViewModel.DmsVMs;
using FATWA_DOMAIN.Models.WorkflowModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using static FATWA_DOMAIN.Enums.Contact.ContactManagementEnum;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_WEB.Pages.ContactManagment
{
    public partial class AddContact : ComponentBase
    {
        #region Constructor
        public AddContact()
        {
            CntContactJobRoleList = new List<Role>();
            CntContactTypeList = new List<CntContactType>();
            OperatingSectorTypeList = new List<OperatingSectorType>();
            DepartmentList = new List<Department>();
            CntContacts = new CntContact();
            InternalContact = false;
            ExternalContact = false;
            MandatoryTempFiles = new ObservableCollection<TempAttachementVM>();
            ShowLinkRequestValues = false;
            ShowLinkFileValues = false;
            ModuleList = new List<Module>();
            CaseFileList = new List<CaseFile>();
            ConsultationFileList = new List<ConsultationFile>();
            CaseRequestList = new List<CaseRequest>();
            ConsultationRequestList = new List<ConsultationRequest>();
            CntContactRequests = new CntContactFileLink();
            ShowConsultationFileValue = false;
            ShowCaseFileValue = false;
            FileJoins = new List<FileJoin>();
            RequestJoins = new List<RequestJoin>();
            DropdownErrorShow = false;
            DropdownErrorShow2 = false;
            FileRequestGridListShow = new List<FileRequestGridList>();
            CompanyList = new List<Company>();
            DesignationList = new List<Designation>();
        }
        #endregion

        #region Parameter
        [Parameter]
        public dynamic ContactId { get; set; }
        #endregion

        #region Variables Declarations
        public CntContact CntContacts { get; set; }
        public List<Role> CntContactJobRoleList { get; set; }
        public List<CntContactType> CntContactTypeList { get; set; }
        public List<OperatingSectorType> OperatingSectorTypeList { get; set; }
        public List<Department> DepartmentList { get; set; }
        public DateTime Max = new DateTime(DateTime.Now.Date.Ticks);
        public DateTime Minimum = new DateTime(1950, 1, 1);
        public bool InternalContact { get; set; }
        public bool ExternalContact { get; set; }
        public bool IsContactExist { get; set; } = false;
        public ObservableCollection<TempAttachementVM> MandatoryTempFiles { get; set; }
        public bool ShowLinkRequestValues { get; set; }
        public bool ShowLinkFileValues { get; set; }
        public List<Module> ModuleList { get; set; }
        public List<CaseFile> CaseFileList { get; set; }
        public List<ConsultationFile> ConsultationFileList { get; set; }
        public List<CaseRequest> CaseRequestList { get; set; }
        public List<ConsultationRequest> ConsultationRequestList { get; set; }
        public CntContactFileLink CntContactRequests { get; set; }
        public bool ShowConsultationFileValue { get; set; }
        public bool ShowCaseFileValue { get; set; }
        public List<FileJoin> FileJoins { get; set; }
        public List<RequestJoin> RequestJoins { get; set; }
        public bool DropdownErrorShow { get; set; }
        public bool DropdownErrorShow2 { get; set; }
        public RadzenDataGrid<FileRequestGridList>? SignatureGridRef = new RadzenDataGrid<FileRequestGridList>();
        public List<FileRequestGridList> FileRequestGridListShow { get; set; }
        protected TempAttachementVM uploadedAttachment;
        public bool ShowEmailErrorMessage { get; set; } = false;
        public List<Company> CompanyList { get; set; }
        public List<Designation> DesignationList { get; set; }
        public RadzenDropDown<int?> CompanyDropDown { get; set; }
        public RadzenDropDown<int?> DesignationDropDown { get; set; }
        public string REGEX_EMAIL_ADDRESS = "^[_A-Za-z0-9-\\+]+(\\.[_A-Za-z0-9-]+)*@[A-Za-z0-9-]+(\\.[A-Za-z0-9]+)*(\\.[A-Za-z]{2,})$";
        public bool IsEmailExist { get; set; } = false;


        #endregion

        #region Component OnInitialized
        protected override async Task OnInitializedAsync()
        {
            try
            {
                spinnerService.Show();
                await PopulateDropdowns();
                if (ContactId == null) // add new contact
                {
                    if (dataCommunicationService?.cntContact != null)
                    {
                        CntContacts.ContactId = dataCommunicationService.cntContact.ContactId;
                        CntContacts.ContactTypeId = dataCommunicationService.cntContact.ContactTypeId;
                        CntContacts.DOB = DateTime.Now;
                    }
                    else
                    {
                        navigationManager.NavigateTo("contact-list");
                    }

                }
                else 
                {
                    CntContacts.ContactId = Guid.Parse(ContactId);
                    var responseContact = await CntContactService.GetContactDetailByUsingContactId(Guid.Parse(ContactId));
                    if (responseContact.IsSuccessStatusCode)
                    {
                        CntContacts = (CntContact)responseContact.ResultData;
                    }
                }
                await SelectedContactType(CntContacts.ContactTypeId);
                StateHasChanged();
                spinnerService.Hide();
            }
            catch (Exception ex)
            {

            }
        }

		//private async Task FillContactRequestGridDetails(List<CntContactRequest> cntContactRequestList)
		//{
		//	try
		//	{
		//		foreach (var itemContact in cntContactRequestList)
		//		{
		//			await AddLinkFile(itemContact.ContactLinkId);
		//			FileRequestGridList fillFileRequest = new FileRequestGridList();
		//			if (itemContact.ContactLinkId == (int)ContactLinkType.File)
		//			{
		//				var resultFile = FileJoins.Where(x => x.FileId == itemContact.ReferenceId).FirstOrDefault();
		//				if (resultFile != null)
		//				{
		//					fillFileRequest.ContactId = CntContacts.ContactId;
		//					fillFileRequest.FileRequestId = resultFile.FileId;
		//					fillFileRequest.FileRequestNumber = resultFile.FileNumber;
		//					if (resultFile.ContactModuleId == (int)WorkflowModuleEnum.CaseManagement)
		//					{
		//						fillFileRequest.FileRequestName = CaseFileList.Where(x => x.FileId == resultFile.FileId).Select(x => x.FileName).FirstOrDefault();
		//					}
		//					else if (resultFile.ContactModuleId == (int)WorkflowModuleEnum.COMSConsultationManagement)
		//					{
		//						fillFileRequest.FileRequestName = ConsultationFileList.Where(x => x.FileId == resultFile.FileId).Select(x => x.FileName).FirstOrDefault();
		//					}
		//					FileRequestGridListShow.Add(fillFileRequest);
		//					fillFileRequest = new FileRequestGridList();
		//				}
		//			}
		//			else if (itemContact.ContactLinkId == (int)ContactLinkType.Request)
		//			{
		//				var resultRequest = RequestJoins.Where(x => x.RequestId == itemContact.ReferenceId).FirstOrDefault();
		//				if (resultRequest != null)
		//				{
		//					fillFileRequest.ContactId = CntContacts.ContactId;
		//					fillFileRequest.FileRequestId = resultRequest.RequestId;
		//					fillFileRequest.FileRequestNumber = resultRequest.FileNumber;
		//					if (resultRequest.ContactModuleId == (int)WorkflowModuleEnum.CaseManagement)
		//					{
		//						fillFileRequest.FileRequestName = CaseRequestList.Where(x => x.RequestId == resultRequest.RequestId).Select(x => x.Subject).FirstOrDefault();
		//					}
		//					else if (resultRequest.ContactModuleId == (int)WorkflowModuleEnum.COMSConsultationManagement)
		//					{
		//						fillFileRequest.FileRequestName = ConsultationRequestList.Where(x => x.ConsultationRequestId == resultRequest.RequestId).Select(x => x.Subject).FirstOrDefault();
		//					}
		//					FileRequestGridListShow.Add(fillFileRequest);
		//					fillFileRequest = new FileRequestGridList();
		//				}
		//			}
		//		}
		//		StateHasChanged();
		//		if (CntContacts.CntContactRequestList.Count() != 0)
		//		{
		//			await SignatureGridRef.Reload();
		//			StateHasChanged();
		//		}
		//	}
		//	catch (Exception)
		//	{
		//		throw;
		//	}
		//}
		#endregion

        #region Populate Dropdowns records
        protected async Task PopulateDropdowns()
        {
            try
            {
                await GetContactJobRole();
                await GetContactType();
                await GetOperatingSectorTypes();
                await GetDepartments();
                await GetModuleList();
                await GetCaseFileList();
                await GetConsultationFileList();
                await GetCaseRequestList();
                await GetConsultationRequestList();
                await GetCompanyList();
                await GetDesignationList();
            }
            catch (Exception)
            {
                throw;
            }

        }

        private async Task GetContactJobRole()
        {
            var response = await lookupService.GetContactJobRole();
            if (response.IsSuccessStatusCode)
            {
                CntContactJobRoleList = (List<Role>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
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
        private async Task GetOperatingSectorTypes()
        {
            var response = await lookupService.GetOperatingSectorTypes();
            if (response.IsSuccessStatusCode)
            {
                OperatingSectorTypeList = (List<OperatingSectorType>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }

        }
        private async Task GetDepartments()
        {
            var response = await lookupService.GetDepartments();
            if (response.IsSuccessStatusCode)
            {
                DepartmentList = (List<Department>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }

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
        private async Task GetCompanyList()
        {
            var response = await lookupService.GetCompanyList();
            if (response.IsSuccessStatusCode)
            {
                CompanyList = (List<Company>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }

        }

        private async Task GetDesignationList()
        {
            var response = await lookupService.GetDesignationList();
            if (response.IsSuccessStatusCode)
            {
                DesignationList = (List<Designation>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }

        }

        #endregion

        #region Radzen form submit button event and Cancel event
        protected async Task Form0Submit(CntContact args)
        {
            try
            {

                await CheckEmailExists();
                await CheckPhoneNumberExists();
                if (IsEmailExist || IsContactExist)
                {
                    await JSRuntime.InvokeVoidAsync("ScrollCaseRequestToTop");
                    return;
                }

                if (ContactId == null)
                {
                    var attachments = await fileUploadService.GetTempAttachements(CntContacts.ContactId);

                    if (attachments.Any())
                    {
                        CntContacts.MandatoryTempFiles = attachments;
                    }
                }

                if (await dialogService.Confirm(translationState.Translate("Consultation_Submit"), translationState.Translate("Confirm"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
                {
                    spinnerService.Show();
                    ApiCallResponse response;
                    //send create/update request
                    if (ContactId == null)
                    {
                        if (dataCommunicationService?.cntContact.CntContactRequestList.Count() != 0)
                        {
                            CntContacts.CntContactRequestList = dataCommunicationService?.cntContact.CntContactRequestList;

                        }
                        response = await CntContactService.CreateContact(CntContacts);

                        if (response.IsSuccessStatusCode)
                        {
                            // Save Attachment of Consultation Request
                            List<Guid> requestIds = new List<Guid>();

                            requestIds.Add(CntContacts.ContactId);


                            var docResponse = await fileUploadService.SaveTempAttachementToUploadedDocument(new FileUploadVM()
                            {
                                RequestIds = requestIds,
                                CreatedBy = CntContacts.CreatedBy,
                                FilePath = _config.GetValue<string>("dms_file_path"),
                                DeletedAttachementIds = CntContacts.DeletedAttachementIds
                            });

                            if (!docResponse.IsSuccessStatusCode)
                            {
                                notificationService.Notify(new NotificationMessage()
                                {
                                    Severity = NotificationSeverity.Error,
                                    Detail = translationState.Translate("Attachment_Save_Failed"),
                                    Style = "position: fixed !important; left: 0; margin: auto; "
                                });
                                return;
                            }
                        }
                    }
                    else
                    {
                        response = await CntContactService.UpdateContact(CntContacts);
                        List<Guid> requestIdList = new List<Guid>();
                        requestIdList.Add(CntContacts.ContactId);
                        var docResponse = await fileUploadService.SaveTempAttachementToUploadedDocument(new FileUploadVM()
                        {
                            RequestIds = requestIdList,
                            CreatedBy = CntContacts.CreatedBy,
                            FilePath = _config.GetValue<string>("dms_file_path"),
                            DeletedAttachementIds = CntContacts.DeletedAttachementIds
                        });

                        if (!docResponse.IsSuccessStatusCode)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Error,
                                Detail = translationState.Translate("Attachment_Save_Failed"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                            return;
                        }
                    }
                    spinnerService.Hide();
                    if (response.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Consultation_Submit_Successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        if (CntContacts.CntContactRequestList.Count() != 0)
                        {
                            if (CntContacts.CntContactRequestList.Select(x => x.ModuleId).FirstOrDefault() == (int)WorkflowModuleEnum.COMSConsultationManagement)
                            {
                                Guid FileId = CntContacts.CntContactRequestList.Select(x => x.ReferenceId).FirstOrDefault();
                                navigationManager.NavigateTo("consultationfile-view/" + FileId + "/" + loginState.UserDetail.SectorTypeId);
                            }
                            else if (CntContacts.CntContactRequestList.Select(x => x.ModuleId).FirstOrDefault() == (int)WorkflowModuleEnum.CaseManagement)
                            {
                                Guid FileId = CntContacts.CntContactRequestList.Select(x => x.ReferenceId).FirstOrDefault();
                                navigationManager.NavigateTo("casefile-view/" + FileId);
                            }
                            {

                            }
                        }
                        else
                        {
                            await JSRuntime.InvokeVoidAsync("history.back");
                        }
                        CntContacts = new CntContact();
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }
                }
            }
            catch (Exception ex)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
        protected async Task CancelForm()
        {
            if (await dialogService.Confirm(translationState.Translate("Sure_Cancel"), translationState.Translate("Confirm"), new ConfirmOptions()
            {
                OkButtonText = translationState.Translate("OK"),
                CancelButtonText = translationState.Translate("Cancel")
            }) == true)
            {
                await JSRuntime.InvokeVoidAsync("history.back");
            }
        }
        protected async Task SubmitClicked()
        {
            if (CntContacts.ContactTypeId <= 0 || string.IsNullOrEmpty(CntContacts.FirstName) || string.IsNullOrEmpty(CntContacts.LastName) || string.IsNullOrEmpty(CntContacts.PhoneNumber))
            {
                await JSRuntime.InvokeVoidAsync("ScrollCaseRequestToTop");
            }
        }
        #endregion

        #region Contact type on change event
        protected async Task SelectedContactType(int args)
        {
            if (args != 0)
            {
                if (args == (int)ContactTypeEnum.Internal)
                {
                    ExternalContact = false;
                    InternalContact = true;
                }
                else if (args == (int)ContactTypeEnum.External)
                {
                    InternalContact = false;
                    ExternalContact = true;
                }
            }
            else
            {
                navigationManager.NavigateTo("contact-list");
            }
        }
        #endregion

        #region Redirect Function

        private void GoBackDashboard()
        {
            navigationManager.NavigateTo("/dashboard");
        }

        private void GoBackHomeScreen()
        {
            navigationManager.NavigateTo("/index");
        }
        protected async Task RedirectBack()
        {
            await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
        }
        #endregion

        #region Add link File, Request, Transaction Events
        //protected async Task AddLinkFile(int selectedType)
        //{
        //         if (selectedType == (int)ContactLinkType.File)
        //         {
        //	FileJoins = new List<FileJoin>();
        //	if (CaseFileList.Count() != 0)
        //	{
        //		foreach (var item in CaseFileList)
        //		{
        //			FileJoin obj = new FileJoin();
        //			obj.FileId = item.FileId;
        //			obj.FileNumber = item.FileNumber;
        //			obj.ContactLinkId = (int)ContactLinkType.File;
        //			obj.ContactModuleId = (int)WorkflowModuleEnum.CaseManagement;
        //			FileJoins.Add(obj);
        //		}
        //	}
        //	if (ConsultationFileList.Count() != 0)
        //	{
        //		foreach (var item in ConsultationFileList)
        //		{
        //			FileJoin obj = new FileJoin();
        //			obj.FileId = item.FileId;
        //			obj.FileNumber = item.FileNumber;
        //			obj.ContactLinkId = (int)ContactLinkType.File;
        //			obj.ContactModuleId = (int)WorkflowModuleEnum.COMSConsultationManagement;
        //			FileJoins.Add(obj);
        //		}
        //	}

        //	//ShowConsultationFileValue = false;
        //	//ShowCaseFileValue = false;
        //	ShowLinkRequestValues = false;
        //	ShowLinkFileValues = true;
        //}
        //else if (selectedType == (int)ContactLinkType.Request)
        //{
        //	RequestJoins = new List<RequestJoin>();
        //	if (CaseRequestList.Count() != 0)
        //	{
        //		foreach (var item in CaseRequestList)
        //		{
        //			RequestJoin obj = new RequestJoin();
        //			obj.RequestId = item.RequestId;
        //			obj.FileNumber = item.FileNumber.ToString();
        //			obj.ContactLinkId = (int)ContactLinkType.Request;
        //			obj.ContactModuleId = (int)WorkflowModuleEnum.CaseManagement;
        //			RequestJoins.Add(obj);
        //		}
        //	}
        //	if (ConsultationRequestList.Count() != 0)
        //	{
        //		foreach (var item in ConsultationRequestList)
        //		{
        //			RequestJoin obj = new RequestJoin();
        //			obj.RequestId = item.ConsultationRequestId;
        //			obj.FileNumber = item.FileNumber;
        //			obj.ContactLinkId = (int)ContactLinkType.Request;
        //			obj.ContactModuleId = (int)WorkflowModuleEnum.COMSConsultationManagement;
        //			RequestJoins.Add(obj);
        //		}
        //	}
        //	ShowLinkFileValues = false;
        //	ShowLinkRequestValues = true;
        //}
        //	var result = await dialogService.OpenAsync<SelectContactLinkPopup>(translationState.Translate("Add_Contact_Link_Document"),
        //		new Dictionary<string, object>()
        //			{
        //				{"SelectedContactLinkType", selectedType },
        //                      {"CntContacts", CntContacts }
        //                  },
        //	   new DialogOptions() { Width = "30% !important", CloseDialogOnOverlayClick = true });
        //	var LinkedFilesResult = (List<CntContactRequest>)result;
        //	if (LinkedFilesResult.Count() != 0)
        //	{
        //		//foreach (var item in LinkedFilesResult)
        //		//{
        //		//	if(item.ReferenceId != CntContacts.CntContactRequestList.Select(x -> x.))
        //		//	CntContacts.CntContactRequestList.Add(item);
        //		//}
        //	}
        //}

        #endregion

        #region Add Contact File Request
        //protected async Task AddContactFileRequest(int file)
        //{
        //	if (CntContactRequests.ReferenceId != Guid.Empty)
        //	{
        //		CntContactRequest ObjContactRequest = new CntContactRequest();
        //		FileRequestGridList ObjFileRequest = new FileRequestGridList();
        //		DropdownErrorShow = false;
        //		DropdownErrorShow2 = false;
        //		if (file == (int)ContactLinkType.File)
        //		{
        //			var resultFile = FileJoins.Where(x => x.FileId == CntContactRequests.ReferenceId).FirstOrDefault();
        //			if (resultFile != null)
        //			{
        //				ObjContactRequest.ContactId = CntContacts.ContactId;
        //				ObjContactRequest.ReferenceId = resultFile.FileId;
        //				ObjContactRequest.ContactLinkId = resultFile.ContactLinkId;
        //				ObjContactRequest.ModuleId = resultFile.ContactModuleId;
        //				ObjContactRequest.CreatedBy = await BrowserStorage.GetItemAsync<string>("User");
        //				ObjContactRequest.CreatedDate = DateTime.Now;
        //				ObjContactRequest.IsDeleted = false;
        //				CntContacts.CntContactRequestList.Add(ObjContactRequest);
        //				ObjContactRequest = new CntContactRequest();
        //				// fill for grid list
        //				ObjFileRequest.ContactId = CntContacts.ContactId;
        //				ObjFileRequest.FileRequestId = resultFile.FileId;
        //				ObjFileRequest.FileRequestNumber = resultFile.FileNumber;
        //				if (resultFile.ContactModuleId == (int)WorkflowModuleEnum.CaseManagement)
        //				{
        //					ObjFileRequest.FileRequestName = CaseFileList.Where(x => x.FileId == resultFile.FileId).Select(x => x.FileName).FirstOrDefault();
        //				}
        //				else if(resultFile.ContactModuleId == (int)WorkflowModuleEnum.COMSConsultationManagement)
        //				{
        //					ObjFileRequest.FileRequestName = ConsultationFileList.Where(x => x.FileId == resultFile.FileId).Select(x => x.FileName).FirstOrDefault();
        //				}
        //				FileRequestGridListShow.Add(ObjFileRequest);
        //				ObjFileRequest = new FileRequestGridList();
        //                  }
        //		}
        //		else if (file == (int)ContactLinkType.Request)
        //		{
        //			var resultRequest = RequestJoins.Where(x => x.RequestId == CntContactRequests.ReferenceId).FirstOrDefault();
        //			if (resultRequest != null)
        //			{
        //				ObjContactRequest.ContactId = CntContacts.ContactId;
        //				ObjContactRequest.ReferenceId = resultRequest.RequestId;
        //				ObjContactRequest.ContactLinkId = resultRequest.ContactLinkId;
        //				ObjContactRequest.ModuleId = resultRequest.ContactModuleId;
        //				ObjContactRequest.CreatedBy = await BrowserStorage.GetItemAsync<string>("User");
        //				ObjContactRequest.CreatedDate = DateTime.Now;
        //				ObjContactRequest.IsDeleted = false;
        //				CntContacts.CntContactRequestList.Add(ObjContactRequest);
        //				ObjContactRequest = new CntContactRequest();
        //				// fill for grid list
        //				ObjFileRequest.ContactId = CntContacts.ContactId;
        //				ObjFileRequest.FileRequestId = resultRequest.RequestId;
        //				ObjFileRequest.FileRequestNumber = resultRequest.FileNumber;
        //				if (resultRequest.ContactModuleId == (int)WorkflowModuleEnum.CaseManagement)
        //				{
        //					ObjFileRequest.FileRequestName = CaseRequestList.Where(x => x.RequestId == resultRequest.RequestId).Select(x => x.Subject).FirstOrDefault();
        //				}
        //				else if (resultRequest.ContactModuleId == (int)WorkflowModuleEnum.COMSConsultationManagement)
        //				{
        //					ObjFileRequest.FileRequestName = ConsultationRequestList.Where(x => x.ConsultationRequestId == resultRequest.RequestId).Select(x => x.Subject).FirstOrDefault();
        //				}
        //				FileRequestGridListShow.Add(ObjFileRequest);
        //				ObjFileRequest = new FileRequestGridList();
        //                  }
        //		}
        //		CntContactRequests.ReferenceId = Guid.Empty;
        //              StateHasChanged();
        //              if (CntContacts.CntContactRequestList.Count() != 0)
        //              {
        //                  await SignatureGridRef.Reload();
        //			StateHasChanged();
        //              }
        //	}
        //	else
        //	{
        //		notificationService.Notify(new NotificationMessage()
        //		{
        //			Severity = NotificationSeverity.Error,
        //			Detail = translationState.Translate("Fill_Required_Fields"),
        //			Style = "position: fixed !important; left: 0; margin: auto; "
        //		});
        //		DropdownErrorShow = true;
        //		DropdownErrorShow2 = true;
        //	}
        //}
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

        #region Check Email, phone and civilid
        protected async Task CheckEmailExists()
        {
            var response = await CntContactService.CheckEmailExists(CntContacts.ContactId, CntContacts.Email);
            if (response.IsSuccessStatusCode)
            {
                IsEmailExist = (bool)response.ResultData;
                if (IsEmailExist)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Email_Already_Exist"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                }
            }
            else { await invalidRequestHandlerService.ReturnBadRequestNotification(response); }
        }
        protected async Task CheckPhoneNumberExists()
        {
            var response = await CntContactService.CheckPhoneNumberExists(CntContacts.ContactId, CntContacts.PhoneNumber);
            if (response.IsSuccessStatusCode)
            {
                var resultEmail = (bool)response.ResultData;
                if (resultEmail)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("PhoneNumber_Already_Exist"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    IsContactExist = true;
                }
            }
        }
        protected async void CheckCivilIdExists()
        {
            var response = await CntContactService.CheckCivilIdExists(CntContacts.ContactId, CntContacts.CivilId);
            if (response.IsSuccessStatusCode)
            {
                var resultEmail = (bool)response.ResultData;
                if (resultEmail)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("CivilId_Already_Exist"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    CntContacts.CivilId = string.Empty;
                }
            }
        }

        public static bool isValidEmail(string inputEmail)
        {
            string strRegex = @"^[_A-Za-z0-9-\\+]+(\\.[_A-Za-z0-9-]+)*@[A-Za-z0-9-]+(\\.[A-Za-z0-9]+)*(\\.[A-Za-z]{2,})$";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(inputEmail))
                return (true);
            else
                return (false);
        }
        #endregion

        #region Add new Contact Workplace
        protected async Task AddWorkPlaceButtonClick(MouseEventArgs args)
        {
            try
            {
                var dialogResult = await dialogService.OpenAsync<AddCompany>(
                translationState.Translate("Add_Company"),
                null,
                new DialogOptions() { CloseDialogOnOverlayClick = true });
                await Task.Delay(100);
                var resultTags = (Company)dialogResult;
                if (resultTags != null)
                {
                    await GetCompanyList();
                    CompanyDropDown.Reset();
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Add new Designation
        protected async Task AddDesignationButtonClick(MouseEventArgs args)
        {
            try
            {
                var dialogResult = await dialogService.OpenAsync<AddDesignation>(
                translationState.Translate("Add_Designation"),
                null,
                new DialogOptions() { CloseDialogOnOverlayClick = true });
                await Task.Delay(100);
                var resultTags = (Designation)dialogResult;
                if (resultTags != null)
                {
                    await GetDesignationList();
                    DesignationDropDown.Reset();
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
                    Name = CntContacts.FirstName,
                    JobRoleId = CntContacts.JobRoleId,
                    PhoneNumber = CntContacts.PhoneNumber
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
