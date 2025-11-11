using FATWA_DOMAIN.Models;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;

namespace FATWA_WEB.Pages.Lms
{
    public partial class AddLiteratureClassification : ComponentBase
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
            lmsliteratureclassification = new LmsLiteratureClassification() { };
            spinnerService.Hide();
        }

        protected async Task Form0Submit(LmsLiteratureClassification args)
        {
            try
            {
                var response = await lmsLiteratureClassificationService.SubmitLmsLiteratureClassification(lmsliteratureclassification);
                if (response.IsSuccessStatusCode)
                {
                    lmsliteratureclassification = (LmsLiteratureClassification)response.ResultData;
                    if (lmsliteratureclassification != null)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Literature_Classification_Create_Success"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        Task.Delay(200);
                        dialogService.Close(lmsliteratureclassification);
                        Task.Delay(200);
                        await Load();
                    }

                    StateHasChanged();
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }



            }
            catch (Exception)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Sure_Cancel"),
                    //Summary = $"???!",
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
