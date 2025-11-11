using FATWA_DOMAIN.Models;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_WEB.Pages.Lms
{
    //<History Author = 'Zain Ul Islam' Date='2022-08-01' Version="1.0" Branch="master"> Edit Lms LiteratureClassification Reverted</History> 
    public partial class EditLiteratureClassification : ComponentBase
    {
        [Parameter(CaptureUnmatchedValues = true)]
        public IReadOnlyDictionary<string, dynamic> Attributes { get; set; }

        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }

        #region Service Injections
          
         
          
        #endregion

        [Parameter]
        public dynamic ClassificationId { get; set; }

        LmsLiteratureClassification _lmsliteratureclassification;
        protected LmsLiteratureClassification lmsliteratureclassification
        {
            get
            {
                return _lmsliteratureclassification;
            }
            set
            {
                if (!object.Equals(_lmsliteratureclassification, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "lmsliteratureclassification", NewValue = value, OldValue = _lmsliteratureclassification };
                    _lmsliteratureclassification = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await Load();
        }
        protected async Task Load()
        {
            spinnerService.Show();
            ApiCallResponse response = await lmsLiteratureClassificationService.GetUniqueLmsLiteratureClassifications(ClassificationId);
            if (response.IsSuccessStatusCode)
            {
                lmsliteratureclassification = (LmsLiteratureClassification)response.ResultData;

            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            spinnerService.Hide();
        }

        protected async Task Form0Submit(LmsLiteratureClassification args)
        {
            try
            {
                var response = await lmsLiteratureClassificationService.UpdateLiteratureClassification(lmsliteratureclassification);
                if (response.IsSuccessStatusCode)
                {
                    lmsliteratureclassification = (LmsLiteratureClassification)response.ResultData;
                    if (lmsliteratureclassification != null)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Changes_saved_successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                    }
                    StateHasChanged();
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
                dialogService.Close(lmsliteratureclassification);


            }
            catch (Exception)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Unable_to_create_new_literature_category"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }

        protected async Task Button2Click(MouseEventArgs args)
        {
            dialogService.Close(null);
        }
    }
}
