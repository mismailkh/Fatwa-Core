using FATWA_DOMAIN.Enums.Common;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.CommunicationModels;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.CommunicationVMs;
using FATWA_DOMAIN.Models.WorkflowModels;
using FATWA_WEB.Pages.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using System.Collections.ObjectModel;
using static FATWA_DOMAIN.Enums.CommunicationEnums;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_WEB.Pages.TarassolCommunication
{
    public partial class CommunicationTarassolAdd : ComponentBase
	{
		#region Paramter
		[Parameter]
		public dynamic? CommunicationId { get; set; }
		[Parameter]
		public dynamic? GovtEntityId { get; set; }
		[Parameter]
		public dynamic? DepartmentId { get; set; }
		#endregion
		#region Varriable

		public List<GovernmentEntity> govtEntity { get; set; } = new();
		public List<GEDepartments> GEDept {get; set; } = new();
		public CommunicationTarassolSendVM communicationTarassolSendVM { get; set; } = new();
		protected SendCommunicationVM sendCommunication = new()
		{
			Communication = new Communication(),
			CommunicationResponse = new CommunicationResponse(),
			CommunicationTargetLink = new CommunicationTargetLink(),
			LinkTarget = new List<FATWA_DOMAIN.Models.CommonModels.LinkTarget>(),
		};

		FATWA_DOMAIN.Models.CommonModels.LinkTarget linkTarget { get; set; }

        protected List<TempAttachementVM> uploadedAttachment { get; set; } = new List<TempAttachementVM>();
        protected RadzenDataGrid<TempAttachementVM>? upload = new RadzenDataGrid<TempAttachementVM>();
        protected bool isUploaded { get; set; } = true;

        protected RadzenDataGrid<TempAttachementVM> gridAttachments { get; set; }
        protected ObservableCollection<TempAttachementVM> attachments { get; set; }

        protected List<AttachmentType> AttachmentTypes { get; set; } = new List<AttachmentType>();
        public bool allowRowSelectOnRowClick = true;

        #endregion
        protected override async Task OnInitializedAsync()
		{
			
            sendCommunication.Communication.CommunicationId = Guid.NewGuid();
			//its for reply purpose when we make first time communication with tarassol we will get all depts and entities
            await poupulateGoveremntEntity(int.Parse(GovtEntityId));
            await poupulateGEDepartment(int.Parse(DepartmentId));

            await PopulateAttachmentTypes();
        }
        protected async Task PopulateAttachmentTypes()
        {
            ApiCallResponse response;
            response = await lookupService.GetAttachmentTypes(0);

            if (response.IsSuccessStatusCode)
            {
                AttachmentTypes = (List<AttachmentType>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        public async Task poupulateGoveremntEntity(int? Id)
		{
			var response = await communicationTarasolService.GetGovernmentEntitiyById(Id);
			if (response.IsSuccessStatusCode)
			{
				var ent = (GovernmentEntity)response.ResultData;

                sendCommunication.Communication.GovtEntityId = ent.EntityId;
                govtEntity.Add(ent);
			}
			else
			{
				await invalidRequestHandlerService.ReturnBadRequestNotification(response);
			}

		}
        public async Task poupulateGEDepartment(int? Id)
		{
			var response = await communicationTarasolService.GetGEDepartmentById(Id);
			if (response.IsSuccessStatusCode)
			{
				var dept = (GEDepartments)response.ResultData;
				sendCommunication.Communication.DepartmentId = dept.Id;
				GEDept.Add(dept);
			}
			else
			{
				await invalidRequestHandlerService.ReturnBadRequestNotification(response);
			}

		}

		protected async Task FormSubmit()
		{
			//Communication
			sendCommunication.Communication.SectorTypeId = (int)loginState.UserDetail.SectorTypeId;
			sendCommunication.Communication.SourceId = (int)CommunicationSourceEnum.FATWA;
			sendCommunication.Communication.SentBy = loginState.Username;
			//sendCommunication.Communication.GovtEntityId = ent.EntityId;
			sendCommunication.Communication.CorrespondenceTypeId = (int)CommunicationCorrespondenceTypeEnum.Outbox;
			sendCommunication.Communication.CreatedBy = loginState.Username;
			sendCommunication.Communication.CreatedDate = DateTime.Now;
			sendCommunication.Communication.IsDeleted = false;
			sendCommunication.Communication.OutboxShortNum = 0;
			sendCommunication.Communication.CommunicationTypeId = (int)CommunicationTypeEnum.SendResponse;
			sendCommunication.Communication.PreCommunicationId = CommunicationId != null ? Guid.Parse(CommunicationId) : Guid.Empty;
			

			//Communication Response 
			sendCommunication.CommunicationResponse.CommunicationResponseId = Guid.NewGuid();
			sendCommunication.CommunicationResponse.CommunicationId = sendCommunication.Communication.CommunicationId;
			sendCommunication.CommunicationResponse.RequestDate = DateTime.Now;// it should be communication date first one
			sendCommunication.CommunicationResponse.ResponseDate = DateTime.Now;
			sendCommunication.CommunicationResponse.CreatedBy = loginState.Username;
			sendCommunication.CommunicationResponse.CreatedDate = DateTime.Now;
			sendCommunication.CommunicationResponse.IsDeleted = false;
			// Bind Govt Entity in reponse Party
   //         var entityIdsList = sendCommunication.CommunicationResponse.EntityIds.ToList();
   //         entityIdsList.Add((int)sendCommunication.Communication.GovtEntityId);
			//sendCommunication.CommunicationResponse.EntityIds = entityIdsList;	

            //CommunicationTargetLink

            sendCommunication.CommunicationTargetLink.CreatedBy = loginState.Username;
			sendCommunication.CommunicationTargetLink.CreatedDate = DateTime.Now;
			sendCommunication.CommunicationTargetLink.IsDeleted = false;
			sendCommunication.CommunicationTargetLink.CommunicationId = sendCommunication.Communication.CommunicationId;
			PopulateLinkTargets();
			var res = await communicationTarasolService.SendTarassolCommunication(sendCommunication);
			if(res.IsSuccessStatusCode)
			{
				var comm = (SendCommunicationVM)res.ResultData;
				if (CommunicationId != null || CommunicationId != Guid.Empty)
				{

                    communicationTarassolSendVM.RSite = govtEntity.Where(x=> x.EntityId == comm.Communication.GovtEntityId).Select(y => y.Name_Ar).FirstOrDefault();

					communicationTarassolSendVM.SSite = "FATWA";
					communicationTarassolSendVM.SenderUser = loginState.Username;
						communicationTarassolSendVM.G2GSubject = comm.Communication.Title;
                    communicationTarassolSendVM.Remarks = comm.Communication.Description;
                    communicationTarassolSendVM.BookNo = null;
                    communicationTarassolSendVM.BookNo = comm.Communication.OutboxNumber;

					// WHEN MOBEEN SEND THE APIs THEN THIS END POINT WILL BE ENABLED
                    //var resp = await communicationTarasolService.SaveTarassolCommunication(communicationTarassolSendVM);
                    //if (!resp.IsSuccessStatusCode)
                    //{
                    //    notificationService.Notify(new NotificationMessage()
                    //    {
                    //        Severity = NotificationSeverity.Success,
                    //        Detail = translationState.Translate("Communication_Sent_Successfully"),
                    //        Style = "position: fixed !important; left: 0; margin: auto; "
                    //    });
                    //    navigationManager.NavigateTo("inboxOutbox-list");
                    //}
                    //else
                    //{
                    //    await invalidRequestHandlerService.ReturnBadRequestNotification(resp);
                    //}

                }
            }


		}
		protected void PopulateLinkTargets()
		{
			linkTarget = new FATWA_DOMAIN.Models.CommonModels.LinkTarget()
			{
				LinkTargetId = new Guid(),
				ReferenceId = sendCommunication.Communication.CommunicationId,
				IsPrimary = false,
				LinkTargetTypeId = (int)LinkTargetTypeEnum.Communication
			};
			sendCommunication.LinkTarget.Add(linkTarget);

			linkTarget = new FATWA_DOMAIN.Models.CommonModels.LinkTarget()
			{
				LinkTargetId = new Guid(),
				ReferenceId = sendCommunication.CommunicationResponse.CommunicationResponseId,
				IsPrimary = true,
				LinkTargetTypeId = (int)LinkTargetTypeEnum.Communication
			};
			sendCommunication.LinkTarget.Add(linkTarget);

		}

		protected void ButtonCancelClick(MouseEventArgs args)
		{
			navigationManager.NavigateTo("index");
		}
		private void GoBackDashboard()
		{
			navigationManager.NavigateTo("/dashboard");
		}
        protected async Task RedirectBack()
        {
            await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
        }
        private void GoBackHomeScreen()
		{
			navigationManager.NavigateTo("/dashboard");
		}

        #region File Upload
        protected async Task AddDocument()
        {
            var result = await dialogService.OpenAsync<FileUpload>(translationState.Translate("Upload_Document"),
                new Dictionary<string, object>()
                {
                        { "ReferenceGuid", Guid.Empty },
                        { "CommunicationId", sendCommunication.Communication.CommunicationId },
                        { "IsViewOnly", false },
                        { "IsUploadPopup", true },
                        { "FileTypes", new List<string>() { ".pdf" } },
                        { "MaxFileSize", systemSettingState.File_Maximum_Size },
                        { "Multiple", true },
                        { "UploadFrom", "CaseManagement" },
                        { "ModuleId", (int)WorkflowModuleEnum.CaseManagement },
                        { "IsCaseRequest", false },

                }
                ,
                new DialogOptions() { Width = "30% !important" });
            uploadedAttachment = uploadedAttachment ?? new List<TempAttachementVM>();
            if (result != null)
                uploadedAttachment.Add((TempAttachementVM)result);
            if (uploadedAttachment.Count != 0)
            {
                isUploaded = true;
            }
            else
            {
                isUploaded = false;
            }
            upload.Reset();
            StateHasChanged();
        }
        protected async Task RemoveDocument(TempAttachementVM file)
        {
            if (await dialogService.Confirm(translationState.Translate("Sure_Remove_File"), translationState.Translate("Remove_File"), new ConfirmOptions()
            {
                OkButtonText = translationState.Translate("OK"),
                CancelButtonText = translationState.Translate("Cancel")
            }) == true)
            {
                var result = false;
                if (file == null)
                    return;

                result = await fileUploadService.RemoveTempAttachement(file?.FileName, file?.UploadedBy, "CaseManagement", "G2G_WEB", file?.AttachmentTypeId);
                if (result)
                {
                    uploadedAttachment = uploadedAttachment ?? new List<TempAttachementVM>();
                    var fileToRemove = uploadedAttachment.Find(f => f.FileName == file.FileName);
                    if (fileToRemove != null)
                    {
                        uploadedAttachment.Remove(fileToRemove);
                        upload.Reset();
                    }

                    if (uploadedAttachment.Count == 0)
                    {
                        isUploaded = false;
                    }
                }
                else
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                        Summary = translationState.Translate("Error"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                }
            }
        }
        #endregion



    }
}
