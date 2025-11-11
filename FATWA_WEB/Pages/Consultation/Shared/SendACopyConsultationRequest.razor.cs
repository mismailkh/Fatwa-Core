using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.Consultation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_WEB.Pages.Consultation.Shared
{
    public partial class SendACopyConsultationRequest : ComponentBase
    {


        #region Parameters

        [Parameter]
        public Guid ReferenceId { get; set; }
        [Parameter]
        public dynamic SendACopyType { get; set; }
        [Parameter]
        public dynamic SectorTypeId { get; set; }

        #endregion

        #region Variables

        protected List<OperatingSectorType> SectorTypes { get; set; } = new List<OperatingSectorType>();
        public ComsApprovalTracking approvalTracking { get; set; } = new ComsApprovalTracking { StatusId = (int)ApprovalStatusEnum.Pending, CreatedDate = DateTime.Now };
        public ConsultationRequest consultationRequest { get; set; } = new ConsultationRequest();
        protected string typeValidationMsgSector = "";
        protected string typeValidationMsgReason = "";
        int SectorTypeIdNew  = 0;

        #endregion

        #region Component Load

        protected override async Task OnInitializedAsync()
        {
            
            await PopulateSectorTypes();
        }

        #endregion

        #region Populate Dropdown Events

        //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master"> Populate Sector Types </History>
        protected async Task PopulateSectorTypes()
        {
            var res = await lookupService.GetOperatingSectorTypes();
            if (res.IsSuccessStatusCode)
            {
                SectorTypes = (List<OperatingSectorType>)res.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(res);
            }
            StateHasChanged();
        }

        #endregion

        #region Button Events
        public async void FormSubmit()
        {
             SectorTypeIdNew = Convert.ToInt32(SectorTypeId); 
            if (approvalTracking.SectorTo > 0 && !String.IsNullOrEmpty(approvalTracking.Remarks))
            {
                if (await dialogService.Confirm(translationState.Translate("Sure_Send_Copy"), translationState.Translate("Confirm"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
                {

                    ApiCallResponse response = null;
                    approvalTracking.SectorFrom = (int)loginState.UserDetail.SectorTypeId;
                    approvalTracking.ReferenceId = ReferenceId;
                    approvalTracking.CreatedBy = loginState.Username;
                    approvalTracking.ProcessTypeId = (int)ApprovalProcessTypeEnum.SendaCopy;

                    spinnerService.Show();

                    response = await comsSharedService.AddSendACopyConsultationTask(approvalTracking, SendACopyType, SectorTypeIdNew);

                    if (response.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Copy_Sent"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        dialogService.Close();
                        dialogService.Close(1);
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }
                    spinnerService.Hide();
                }
            }
            else
            {
                typeValidationMsgSector = approvalTracking.SectorTo > 0 ? "" : @translationState.Translate("Required_Field");
                typeValidationMsgReason = !String.IsNullOrEmpty(approvalTracking.Remarks) ? "" : @translationState.Translate("Required_Field");
            }



        }
        protected void ButtonCancelClick(MouseEventArgs args)
        {
            dialogService.Close(null);
        }

        #endregion

    }
}

