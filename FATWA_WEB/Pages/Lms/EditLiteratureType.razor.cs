using FATWA_DOMAIN.Models;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;

namespace FATWA_WEB.Pages.Lms
{
    public partial class EditLiteratureType : ComponentBase
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
        public dynamic TypeId { get; set; }

        LmsLiteratureType _lmsliteraturetype;
        protected LmsLiteratureType lmsliteraturetype
        {
            get
            {
                return _lmsliteraturetype;
            }
            set
            {
                if (!object.Equals(_lmsliteraturetype, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "lmsliteraturetype", NewValue = value, OldValue = _lmsliteraturetype };
                    _lmsliteraturetype = value;
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
            var getLmsLiteratureTypeByTypeIdResult = await lmsLiteratureTypeService.GetLmsLiteratureTypeById(TypeId);
            lmsliteraturetype = getLmsLiteratureTypeByTypeIdResult;
            spinnerService.Hide();
        }

        protected async Task Form0Submit(LmsLiteratureType args)
        {
            try
            {
                var response = await lmsLiteratureTypeService.UpdateLmsLiteratureType(TypeId, lmsliteraturetype);
                if (response.IsSuccessStatusCode)
                {
                    lmsliteraturetype = (LmsLiteratureType)response.ResultData;
                    if (lmsliteraturetype != null)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Changes_saved_successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        dialogService.Close(lmsliteraturetype);
                        await Load();
                    }
                    StateHasChanged();
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
                dialogService.Close(lmsliteraturetype);
            }
            catch (Exception)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Literature_type_could_not_be_updated"),
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
