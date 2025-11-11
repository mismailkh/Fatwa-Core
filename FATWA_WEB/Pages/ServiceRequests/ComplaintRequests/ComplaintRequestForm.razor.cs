using DocumentFormat.OpenXml.Spreadsheet;
using FATWA_DOMAIN.Models.ServiceRequestModels;
using FATWA_DOMAIN.Models.ServiceRequestModels.ComplaintRequestModels;
using FATWA_DOMAIN.Models.ViewModel.DmsVMs;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using Syncfusion.Blazor.RichTextEditor;
using static FATWA_DOMAIN.Enums.ServiceRequests.ServiceRequestEnums;

namespace FATWA_WEB.Pages.ServiceRequests.ComplaintRequests
{
    public partial class ComplaintRequestForm : ComponentBase
    {
        #region Paramters
        [Parameter]
        public dynamic RequestTypeId { get; set; }  
        [Parameter]
        public dynamic SectorId { get; set; }
        [Parameter]
        public dynamic ServiceRequestId { get; set; }
        #endregion

        #region Variables
        ComplaintRequest ComplaintRequest;
        IEnumerable<SRPriority> Priorities;
        IEnumerable<ComplaintTypes> ComplaintTypes;
        ComplaintRequestDetailVM ComplaintRequestDetailVM { get; set; }
        public List<int> DeletedAttachementIds { get; set; } = new List<int>();
        public string descriptionValidationMessage { get; set; } = "";
        protected string RequestNumber = null!;
        protected bool isSaved;

        #endregion

        #region Constructor
        public ComplaintRequestForm()
        {
            ComplaintRequest = new ComplaintRequest { Id = Guid.NewGuid(), IssueStartDate = DateTime.Now.Date };
            Priorities = new List<SRPriority>();
            ComplaintTypes = new List<ComplaintTypes>();
            ComplaintRequestDetailVM = new ComplaintRequestDetailVM();
        }
        #endregion

        #region SyncFusion Rich Text Editor
        private List<ToolbarItemModel> Tools = new List<ToolbarItemModel>()
   {
   new ToolbarItemModel() { Command = ToolbarCommand.Formats },
   new ToolbarItemModel() { Command = ToolbarCommand.Separator },
   new ToolbarItemModel() { Command = ToolbarCommand.Bold },
   new ToolbarItemModel() { Command = ToolbarCommand.Italic },
   new ToolbarItemModel() { Command = ToolbarCommand.Underline },
   new ToolbarItemModel() { Command = ToolbarCommand.Separator },
   new ToolbarItemModel() { Command = ToolbarCommand.FontColor },
   new ToolbarItemModel() { Command = ToolbarCommand.Separator },
   new ToolbarItemModel() { Command = ToolbarCommand.NumberFormatList },
   new ToolbarItemModel() { Command = ToolbarCommand.BulletFormatList },
   new ToolbarItemModel() { Command = ToolbarCommand.Separator },
   new ToolbarItemModel() { Command = ToolbarCommand.CreateLink },
   new ToolbarItemModel() { Command = ToolbarCommand.Image },
   new ToolbarItemModel() { Command = ToolbarCommand.CreateTable },
   new ToolbarItemModel() { Command = ToolbarCommand.InsertCode },
   new ToolbarItemModel() { Command = ToolbarCommand.SourceCode },
   new ToolbarItemModel() { Command = ToolbarCommand.Separator },
   new ToolbarItemModel() { Command = ToolbarCommand.Undo },
   new ToolbarItemModel() { Command = ToolbarCommand.Redo }
   };
        private List<TableToolbarItemModel> TableQuickToolbarItems = new List<TableToolbarItemModel>()
   {
   new TableToolbarItemModel() { Command = TableToolbarCommand.TableHeader },
   new TableToolbarItemModel() { Command = TableToolbarCommand.TableRows },
   new TableToolbarItemModel() { Command = TableToolbarCommand.TableColumns },
   new TableToolbarItemModel() { Command = TableToolbarCommand.TableCell },
   new TableToolbarItemModel() { Command = TableToolbarCommand.HorizontalSeparator },
   new TableToolbarItemModel() { Command = TableToolbarCommand.TableRemove },
   new TableToolbarItemModel() { Command = TableToolbarCommand.BackgroundColor },
   new TableToolbarItemModel() { Command = TableToolbarCommand.TableCellVerticalAlign },
   new TableToolbarItemModel() { Command = TableToolbarCommand.Styles }
   };
        private List<ImageToolbarItemModel> ImageTools = new List<ImageToolbarItemModel>()
   {
   new ImageToolbarItemModel() { Command = ImageToolbarCommand.Replace },
   new ImageToolbarItemModel() { Command = ImageToolbarCommand.Align },
   new ImageToolbarItemModel() { Command = ImageToolbarCommand.Caption },
   new ImageToolbarItemModel() { Command = ImageToolbarCommand.Remove },
   new ImageToolbarItemModel() { Command = ImageToolbarCommand.HorizontalSeparator },
   new ImageToolbarItemModel() { Command = ImageToolbarCommand.InsertLink },
   new ImageToolbarItemModel() { Command = ImageToolbarCommand.Display },
   new ImageToolbarItemModel() { Command = ImageToolbarCommand.AltText },
   new ImageToolbarItemModel() { Command = ImageToolbarCommand.Dimension }
   };
        private List<string> allowedTypes = new List<string> { ".png", ".jpg", ".jpeg" };
        #endregion

        #region On Load Component 
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await Load();
            spinnerService.Hide();
        }

        #endregion

        #region DropDowns
        private async Task GetPriorties()
        {
            var response = await complaintRequestService.GetPriorities();
            if (response.IsSuccessStatusCode)
            {
                Priorities = (IEnumerable<SRPriority>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }

        private async Task GetComplaintTypes()
        {
            var response = await complaintRequestService.GetComplaintTypes();
            if (response.IsSuccessStatusCode)
            {
                ComplaintTypes = (IEnumerable<ComplaintTypes>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        #endregion

        #region Load
        protected async Task Load()
        {
            if (ServiceRequestId is null)
                await GetLatestServiceRequestNumber();
            else
                await GetComplaintRequestDetailById();
            await PopulateDropDowns();
        }
        #endregion

        #region Functions

        private async Task PopulateDropDowns()
        {
            await GetComplaintTypes();
            await GetPriorties();
        }

        private async Task GetLatestServiceRequestNumber()
        {
            var response = await serviceRequestSharedService.GetLatestServiceRequestNumber(Convert.ToInt32(RequestTypeId));
            if (response.IsSuccessStatusCode)
            {
                RequestNumber = (string)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }

        protected async Task SaveTempAttachementToUploadedDocument()
        {
            try
            {
                var docResponse = await fileUploadService.SaveTempAttachementToUploadedDocument(new FileUploadVM()
                {
                    RequestIds = new List<Guid> { ComplaintRequest.Id },
                    CreatedBy = loginState.Username,
                    FilePath = _config.GetValue<string>("dms_file_path"),
                    DeletedAttachementIds = null
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
            catch (Exception)
            {
                return;
                throw;
            }
        }
        private async Task GetComplaintRequestDetailById()
        {
            var response = await complaintRequestService.GetComplaintRequestDetailById(Guid.Parse(ServiceRequestId));

            if (response.IsSuccessStatusCode)
            {
                ComplaintRequestDetailVM = (ComplaintRequestDetailVM)response.ResultData;
                ComplaintRequest.Subject = ComplaintRequestDetailVM.Subject;
                ComplaintRequest.Description = ComplaintRequestDetailVM.Description;
                ComplaintRequest.PriorityId = ComplaintRequestDetailVM.PriorityId;
                ComplaintRequest.TypeId = ComplaintRequestDetailVM.ComplaintTypeId;
                ComplaintRequest.Id = ComplaintRequestDetailVM.Id;
                ComplaintRequest.ServiceRequestId = ComplaintRequestDetailVM.ServiceRequestId;
                RequestNumber = ComplaintRequestDetailVM.ServiceRequestNumber;
                ComplaintRequest.IssueStartDate = ComplaintRequestDetailVM.IssueStartDate;
                ComplaintRequest.OtherType = ComplaintRequestDetailVM.OtherComplaintType;
            }
            else
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
        }

        private async Task AddComplaintRequest(int statusId)
        {
            ComplaintRequest.ServiceRequest = new ServiceRequest
            {
                ServiceRequestNumber = RequestNumber,
                ServiceRequestId = Guid.NewGuid(),
                ServiceRequestTypeId = Convert.ToInt32(RequestTypeId),
                ServiceRequestStatusId = statusId,
                CreatedBy = loginState.UserDetail.UserName,
                CreatedDate = DateTime.Now
            };

            ComplaintRequest.ServiceRequestId = ComplaintRequest.ServiceRequest.ServiceRequestId;
            ComplaintRequest.ReceiverId = "436e82d2-70d8-455c-a643-7909b8689667";
            var response = await complaintRequestService.AddComplaintRequest(ComplaintRequest);
            if (response.IsSuccessStatusCode)
            {
                if (ComplaintRequest.IsSubmit)
                    await SaveTempAttachementToUploadedDocument();
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Success,
                    Detail = translationState.Translate("Complaint_Request_Submitted_Successfully"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                StateHasChanged();
                navigationManager.NavigateTo("servicerequest-list");
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }

        private async Task UpdateComplaintRequest()
        {
            var response = await complaintRequestService.UpdateComplaintRequest(ComplaintRequest);
            if (response.IsSuccessStatusCode)
            {
                if (ComplaintRequest.IsSubmit)
                    await SaveTempAttachementToUploadedDocument();
                var message = ComplaintRequest.IsSubmit ? "Complaint_Request_Submitted_Successfully" : "Complaint_Request_Draft_Saved_Successfully";
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Success,
                    Detail = translationState.Translate(message),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                StateHasChanged();
                navigationManager.NavigateTo("servicerequest-list");
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        #endregion

        #region Form Submit
        protected async Task Form0Submit()
        {
            try
            {
                bool ? dialogResponse = await dialogService.Confirm(
                            translationState.Translate("Are_You_Sure_You_Want_to_Submit_Complaint_Request"),
                            translationState.Translate("Confirm"),
                            new ConfirmOptions()
                            {
                                OkButtonText = @translationState.Translate("Yes"),
                                CancelButtonText = @translationState.Translate("No")
                            });
                if (dialogResponse == true)
                {
                    ComplaintRequest.IsSubmit = true;
                    spinnerService.Show();
                    if (ServiceRequestId is null)
                    {
                        await AddComplaintRequest((int)ServiceRequestStatusEnum.Submitted);
                    }
                    else
                    {
                        await UpdateComplaintRequest();
                    }
                    spinnerService.Hide();

                    navigationManager.NavigateTo("servicerequest-list");
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
                navigationManager.NavigateTo("servicerequest-list");
            }
        }
        #endregion

        #region Save As Draft
        protected async Task SaveAsDraft()
        {
            try
            {
                bool? dialogResponse = await dialogService.Confirm(
                            translationState.Translate("Are_You_Sure_You_Want_to_Save_Complaint_Request_Draft"),
                            translationState.Translate("Confirm"),
                            new ConfirmOptions()
                            {
                                OkButtonText = @translationState.Translate("Yes"),
                                CancelButtonText = @translationState.Translate("No")
                            });
                if (dialogResponse == true)
                {
                    spinnerService.Show();
                    if (ServiceRequestId == null)
                    {
                        await AddComplaintRequest((int)ServiceRequestStatusEnum.Draft);
                    }
                    else
                        await UpdateComplaintRequest();

                    spinnerService.Hide();

                    navigationManager.NavigateTo("servicerequest-list");
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
    }
}
