using FATWA_DOMAIN.Models.ViewModel.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;

namespace FATWA_WEB.Pages.Shared
{
    //< History Author = 'Umer Zaman' Date = '2023-01-27' Version = "1.0" Branch = "master" >Delete added section under Blank Template Section</History>
    public partial class DeleteBlankTemplateSection : ComponentBase
    {
        #region Parameter

        [Parameter]
        public dynamic Sections { get; set; }
        public List<CaseTemplateSectionsVM> TemplateSections { get { return (List<CaseTemplateSectionsVM>)Sections; } set { Sections = value; } }

        #endregion

        #region Variables
        protected CaseTemplateSectionsVM templateSection { get; set; } = new CaseTemplateSectionsVM(); 
        protected List<object> SectionPositionOptions { get; set; } = new List<object>();
        public class SectionPositionTemp
        {
            public BlankTemplateSectionPositionEnum SectionPositionEnumValue { get; set; }
            public string SectionPositionEnumName { get; set; }
        }

        protected string positionValidationMsg { get; set; }
        protected string sectionValidationMsg { get; set; }
        public Guid SelectedSection { get; set; }

        #endregion

        #region Component Events

        protected override async Task OnInitializedAsync()
        {
            try
            {
                spinnerService.Show();
                SelectedSection = Guid.Empty;
                spinnerService.Hide();
            }
            catch (Exception ex)
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

        protected async Task Form0Submit(CaseTemplateSectionsVM args)
        {
            try
            {
                if(templateSection.Id == null)
                {
                    sectionValidationMsg = templateSection.Id == null ? translationState.Translate("Required_Field") : "" ;
                    return;
                }
                if (await dialogService.Confirm(translationState.Translate("Sure_Delete_Section"), translationState.Translate("Submit"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
                {
                    spinnerService.Show();
                    dialogService.Close(templateSection);
                    spinnerService.Hide();
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

        protected async Task CloseDialog(MouseEventArgs args)
        {
            dialogService.Close(null);
        }

        #endregion

    }
}
