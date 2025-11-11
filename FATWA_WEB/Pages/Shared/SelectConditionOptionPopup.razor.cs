using FATWA_DOMAIN.Enums;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;
using static FATWA_GENERAL.Helper.Response;
using Radzen;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using FATWA_DOMAIN.Models.CaseManagment;
using Microsoft.AspNetCore.Components.Web;
using FATWA_WEB.Extensions;
using static FATWA_DOMAIN.Enums.CommunicationEnums;
using System.Runtime.CompilerServices;
using FATWA_DOMAIN.Models.ViewModel;

namespace FATWA_WEB.Pages.Shared
{
    //<History Author = 'Hassan Abbas' Date='2022-07-22' Version="1.0" Branch="master"> Select types for draft document</History>
    public partial class SelectConditionOptionPopup : ComponentBase
    {
        #region Parameter

        [Parameter]
        public dynamic Options { get; set; }
         [Parameter]
        public bool isActivity { get; set; }

        
        #endregion
        #region  Variable Declaration
        public List<WorkflowConditionsOptionVM> conditionoptions { get { return Options; } set { Options = value; } }
        public List<WorkflowActivityOptionVM> activityOptions { get { return Options; } set { Options = value; } }
        public int SelectedOptionId { get; set; } = 0;


        #endregion
        protected async Task Form0Submit()
        {
            try
            {
                if(SelectedOptionId > 0)
                {
                    if (await dialogService.Confirm(translationState.Translate("Option_Select_Confirm"), translationState.Translate("Confirm"), new ConfirmOptions()
                    {
                        OkButtonText = translationState.Translate("OK"),
                        CancelButtonText = translationState.Translate("Cancel")
                    }) == true)
                    {
                        dialogService.Close(SelectedOptionId);
                    }
                }
                else
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Fill_Required_Fields"),
                        //Summary = $"" + translationState.Translate("Error"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                }
            }
            catch(Exception ex)
            {

            }
        }
        protected async Task CloseDialog(MouseEventArgs args)
        {
            dialogService.Close(null);
        }

    }
}
