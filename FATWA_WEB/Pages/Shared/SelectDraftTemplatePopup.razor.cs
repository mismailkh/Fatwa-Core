using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;

namespace FATWA_WEB.Pages.Shared
{
    //<History Author = 'Hassan Abbas' Date='2022-07-22' Version="1.0" Branch="master"> Select types for draft document</History>
    public partial class SelectDraftTemplatePopup : ComponentBase
    {
        #region Parameter

        [Parameter]
        public string ReferenceId { get; set; }

        [Parameter]
        public dynamic DraftEntityType { get; set; }

        [Parameter]
        public string? Payload { get; set; }
        [Parameter]
        public int? Document_Type { get; set; }
        [Parameter]
        public List<AttachmentType>? DocumentTypes { get; set; }
        [Parameter]
        public int? ResponseTypeId { get; set; }
        [Parameter]
        public dynamic? TaskId { get; set; }
        [Parameter]
        public int ModuleId { get; set; } 

        #endregion

        #region Variables

        CasePartyLinkVM casePartyLink = new CasePartyLinkVM { Id = Guid.NewGuid() };
        protected int AttachmentTypeId { get; set; }
        protected int TemplateId { get; set; }
        protected int radioValue { get; set; }
        protected CaseTemplate CaseTemplate { get; set; } = new CaseTemplate();
        protected List<AttachmentType> AttachmentTypes { get; set; } = new List<AttachmentType>();
        protected List<CaseTemplate> CaseTemplates { get; set; } = new List<CaseTemplate>();

        #endregion

        #region Radio Button Change
        protected async Task OnChange(int value)
        {
        }

        protected async Task CloseDialog(MouseEventArgs args)
        {
            dialogService.Close(null);
        }

        #endregion

        #region Component Load

        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await PopulateAttachmentTypes();
            spinnerService.Hide(); 
        }

        #endregion

        #region Dropdown Data and Change Events

        //<History Author = 'Hassan Abbas' Date='2022-09-30' Version="1.0" Branch="master">Populate Subtypes data</History>
        protected async Task PopulateAttachmentTypes()
        {
            var response = await lookupService.GetAttachmentTypes(Document_Type != null && Document_Type > 0 ? 0 : ModuleId); // for getting all AttachmentTypes if Attachment type auto selected
            if (response.IsSuccessStatusCode)
            {
                AttachmentTypes = (List<AttachmentType>)response.ResultData;
                AttachmentTypes= AttachmentTypes.Where(x=>x.IsMojExtracted==false).ToList();
                if ((Document_Type == null || Document_Type <= 0) && ModuleId == (int)WorkflowModuleEnum.CaseManagement)
                {
                    if(DocumentTypes != null)
                    AttachmentTypes = AttachmentTypes.Where(d => DocumentTypes.Any(t => t.AttachmentTypeId == d.AttachmentTypeId)).ToList();
                }
                else if ((Document_Type == null || Document_Type <= 0) && ModuleId == (int)WorkflowModuleEnum.COMSConsultationManagement)
                {
                    AttachmentTypes = AttachmentTypes;
                }
                else if (Document_Type != null && Document_Type > 0)
                {
                    AttachmentTypeId = (int)Document_Type;
                    await OnTypeChange(AttachmentTypeId);
                }
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }

        //<History Author = 'Hassan Abbas' Date='2022-09-30' Version="1.0" Branch="master">Populate Case Templates</History>
        protected async Task PopulateTemplates()
        {
            var response = await lookupService.GetCaseTemplates(AttachmentTypeId);
            if (response.IsSuccessStatusCode)
            {
                CaseTemplates = (List<CaseTemplate>)response.ResultData;
                //if(ResponseTypeId == (int)ResponseTypeEnum.RequestForMoreInformation)
                //{
                //    CaseTemplates = CaseTemplates.Where(t => t.Id == (int)CaseTemplateEnum.AdditionalInformationNotification).ToList();
                //} 
                //else if(ResponseTypeId == (int)ResponseTypeEnum.RequestForMoreInformationReminder)
                //{
                //    CaseTemplates = CaseTemplates.Where(t => t.Id == (int)CaseTemplateEnum.AdditionalInformationReminderNotification).ToList();
                //}
                //else if (ResponseTypeId == (int)ResponseTypeEnum.SaveAndCloseFile)
                //{
                //    CaseTemplates = CaseTemplates.Where(t => t.Id == (int)CaseTemplateEnum.SavingFileNotification).ToList();
                //}
                //else if (ResponseTypeId == (int)ResponseTypeEnum.CaseRegistered)
                //{
                //    CaseTemplates = CaseTemplates.Where(t => t.Id == (int)CaseTemplateEnum.CaseRegisteredNotification).ToList();
                //}
                //else if (ResponseTypeId == (int)ResponseTypeEnum.CaseFileExecution)
                //{
                //    CaseTemplates = CaseTemplates.Where(t => t.Id == (int)CaseTemplateEnum.FileExecutionNotification).ToList();
                //}
                //else if (ResponseTypeId == (int)ResponseTypeEnum.InitialJudgement)
                //{
                //    CaseTemplates = CaseTemplates.Where(t => t.Id == (int)CaseTemplateEnum.InitialJudgemenet).ToList();
                //}
                //else if (ResponseTypeId == (int)ResponseTypeEnum.FinalJudgement)
                //{
                //    CaseTemplates = CaseTemplates.Where(t => t.Id == (int)CaseTemplateEnum.Judgement).ToList();
                //}
                //else if (ResponseTypeId == (int)ResponseTypeEnum.GeneralUpdate)
                //{
                //    CaseTemplates = CaseTemplates.Where(t => t.Id == (int)CaseTemplateEnum.GeneralUpdateNotification).ToList();
                //}
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        //<History Author = 'Hassan Abbas' Date='2022-09-30' Version="1.0" Branch="master">Hide Template dropdown if no Type selected</History>
        protected async Task OnTypeChange(object args)
        {
            try
            {
                if ((int)args == 0)
                {
					radioValue = 0;
				} 
                else
                {
                    radioValue = radioValue == 0 ? 1 : radioValue;
                    await PopulateTemplates();
                }

                StateHasChanged();
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region Submit
        protected async Task FormInvalidSubmit()
        {
            try
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Fill_Required_Fields"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                return;
            }
            catch (Exception)
            {
                throw;
            }
        }

            //<History Author = 'Hassan Abbas' Date='2022-07-22' Version="1.0" Branch="master"> Submit selected draft types</History>
        protected async void Form0Submit(CaseTemplate args)
        {
            try
            {
                dataCommunicationService.draftEntityData.Payload = Payload;
                dataCommunicationService.draftEntityData.DraftEntityType = (int)DraftEntityType;
                dialogService.Close(null);


                if (radioValue == 1)
                    if (TaskId == null)
                        navigationManager.NavigateTo("/create-filedraft/" + ReferenceId + "/" + AttachmentTypeId + "/" + (int)CaseTemplateEnum.NoTemplate + "/" + ModuleId);
                    else
                        navigationManager.NavigateTo("/create-filedraft/" + ReferenceId + "/" + AttachmentTypeId + "/" + (int)CaseTemplateEnum.NoTemplate + "/" + TaskId + "/" + ModuleId) ;
                else
                    if (TaskId == null)
                        navigationManager.NavigateTo("/create-filedraft/" + ReferenceId + "/" + AttachmentTypeId + "/" + TemplateId + "/" + ModuleId);
                    else
                        navigationManager.NavigateTo("/create-filedraft/" + ReferenceId + "/" + AttachmentTypeId + "/" + TemplateId + "/" + TaskId + "/" + ModuleId);
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

    }
}
