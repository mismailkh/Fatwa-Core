using FATWA_DOMAIN.Models.Notifications;
using FATWA_DOMAIN.Models.ServiceRequestModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Syncfusion.Blazor.RichTextEditor;
using static FATWA_DOMAIN.Enums.ServiceRequests.ServiceRequestEnums;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_WEB.Pages.ServiceRequests.Shared
{
    public partial class RequestDecisionPopUp : ComponentBase
    {
        #region Parameter
        [Parameter]
        public Guid ReferenceId { get; set; }
        [Parameter]
        public int RemarkType { get; set; }
        [Parameter]
        public Guid ServiceRequestId { get; set; }
        [Parameter]
        public int? ServiceRequestStatus { get; set; }
        #endregion

        #region Variable Declaration 
        public ServiceRequestRemarks remarks = new ServiceRequestRemarks();
        public string reasonValidationMessage { get; set; } = "";
        public string StatusValidationMessage { get; set; } = "";
        public List<int>? DeletedAttachementIds { get; set; } = new List<int>();
        public class RemarkTypeEnumTemp
        {
            public int RemarkTypeEnumValue { get; set; }
            public string RemarkTypeEnumName { get; set; }
        }
        public List<RemarkTypeEnumTemp> RequestStatuses { get; set; } = new List<RemarkTypeEnumTemp>();
        public List<Notification> Notifications { get; set; } = new List<Notification>();

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

        #region Load Componenet
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            remarks.ServiceRequestStatus = ServiceRequestStatus;
            await Load();
            spinnerService.Hide();
        }
        protected async Task Load()
        {
            await PopulateStatuses();
        }
        #endregion

        #region Populate 
        protected async Task PopulateStatuses()
        {
            try
            {
                if(RemarkType > 0)
                {
                    switch (RemarkType)
                    {
                        case (int)RemarkTypeEnum.ReOpen:
							RequestStatuses.Add(new RemarkTypeEnumTemp { RemarkTypeEnumName = translationState.Translate(RemarkTypeEnum.ReOpen.ToString()), RemarkTypeEnumValue = RemarkType });
                            break;
                            
                        case (int)RemarkTypeEnum.Rejected:
							RequestStatuses.Add(new RemarkTypeEnumTemp { RemarkTypeEnumName = translationState.Translate(RemarkTypeEnum.Rejected.ToString()), RemarkTypeEnumValue = RemarkType });
							break;
                        
                        case (int)RemarkTypeEnum.NeedModification:
							RequestStatuses.Add(new RemarkTypeEnumTemp { RemarkTypeEnumName = translationState.Translate(RemarkTypeEnum.NeedModification.ToString()), RemarkTypeEnumValue = RemarkType });
							break;
					}
					remarks.TypeId = RemarkType;
				}
                else
                {
                    foreach(var item in Enum.GetValues(typeof(RemarkTypeEnum)))
                    {
						RequestStatuses.Add(new RemarkTypeEnumTemp { RemarkTypeEnumName = translationState.Translate(item.ToString()), RemarkTypeEnumValue = (int)item });
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
        #endregion

        #region Submit
        protected async Task SubmitRemark()
        {
            try
            {
                if (string.IsNullOrEmpty(remarks.RemarkContent) || remarks.TypeId == 0)
                {
                    reasonValidationMessage = String.IsNullOrEmpty(remarks.RemarkContent) ? translationState.Translate("Required_Field") : "";
                    StatusValidationMessage = remarks.TypeId == 0 ? translationState.Translate("Required_Field") : "";
                }
                else
                {
                    bool? dialogResponse = await dialogService.Confirm(
                    translationState.Translate("Sure_Submit"),
                    translationState.Translate("Submit"),
                    new ConfirmOptions()
                    {
                        OkButtonText = translationState.Translate("OK"),
                        CancelButtonText = translationState.Translate("Cancel")
                    });
                    if (dialogResponse != null)
                    {
                        if ((bool)dialogResponse)
                        {
                            remarks.Id = Guid.NewGuid();
                            remarks.CreatedDate = DateTime.Now;
                            remarks.CreatedBy = loginState.Username;
                            remarks.ReferenceId = ReferenceId;
                            remarks.ServiceRequestId = ServiceRequestId;
                            ApiCallResponse response = new ApiCallResponse();

                            response = await serviceRequestSharedService.AddServiceRequestRemarks(remarks);

                            if (response.IsSuccessStatusCode && (bool)response.ResultData!)
                            {
                                var notifDetail = "";
                                switch (remarks.TypeId)
                                {
                                    case (int)RemarkTypeEnum.Rejected:
                                        notifDetail = "Service_Request_Rejected";
                                        break;

                                    case (int)RemarkTypeEnum.AddResolution:
                                        notifDetail = "Service_Request_Resolved_Successfully";
                                        break;

                                    case (int)RemarkTypeEnum.ReOpen:
                                        notifDetail = "Service_Request_ReOpened_Successfully";
                                        break;

                                    case (int)RemarkTypeEnum.NeedModification:
                                        notifDetail = "Need_Modification_Remarks_Submitted_Successfully";
                                        break;
                                }
                                
                                notificationService.Notify(new NotificationMessage()
                                {
                                    Severity = NotificationSeverity.Success,
                                    Detail = translationState.Translate($"{notifDetail}"),
                                    Style = "position: fixed !important; left: 0; margin: auto; "
                                });
                                dialogService.Close(1);
                            }
                            else
                            {
                                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                            }
                        }
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
        #endregion

        #region Redirect and Dialog Events
        protected async void ButtonCancelClick(MouseEventArgs args)
        {
            bool? dialogResponse = await dialogService.Confirm(
                   translationState.Translate("Sure_Cancel"),
                   translationState.Translate("Submit"),
                   new ConfirmOptions()
                   {
                       OkButtonText = translationState.Translate("OK"),
                       CancelButtonText = translationState.Translate("Cancel")
                   });
            if (dialogResponse != null)
            {
                if ((bool)dialogResponse)
                {
                    dialogService.Close();
                }
            }
        }
        #endregion
    }
}
