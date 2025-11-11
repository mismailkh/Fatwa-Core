using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;

namespace FATWA_WEB.Pages.Lms
{
    public partial class AddLiteratureIndex : ComponentBase
    {
        public AddLiteratureIndex()
        {
        }
        #region Parameters
        [Parameter]
        public dynamic ParentIndexId { get; set; }
        [Parameter]
        public dynamic ParentIndexNumber { get; set; }

        protected string ParrentIndexNameAr { get; set; }
        protected string ParrentIndexNameEn { get; set; }
        #endregion

        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }

        #region Service Injections
    
         
          

        #endregion

        LmsLiteratureIndex _lmsliteratureIndex;
        protected LmsLiteratureIndex lmsliteratureIndex
        {
            get
            {
                return _lmsliteratureIndex;
            }
            set
            {
                if (!object.Equals(_lmsliteratureIndex, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "lmsliteratureIndex", NewValue = value, OldValue = _lmsliteratureIndex };
                    _lmsliteratureIndex = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        public LmsLiteratureParentIndexVM RegisteredIndexDetails { get; set; } = new LmsLiteratureParentIndexVM();

        protected override async Task OnInitializedAsync()
        {
            await Load();
        }
        protected async Task Load()
        {
            spinnerService.Show();

            // LmsLiteratureParentIndex ParrentIndex = await FatwaLmsParentLitIndexApi.GetLiteratureParentIndexDetailById(ParentIndexId);
            //ParrentIndexNameAr=ParrentIndex.Name_Ar; 
            //ParrentIndexNameEn=ParrentIndex.Name_En;
            var result = await lmsLiteratureIndexService.GetLiteratureIndexByIndexIdAndNumber(int.Parse(ParentIndexId), Convert.ToString(ParentIndexNumber));
            if (result.IsSuccessStatusCode)
            {
                RegisteredIndexDetails = (LmsLiteratureParentIndexVM)result.ResultData;
            }
            else
            {
                RegisteredIndexDetails = new LmsLiteratureParentIndexVM();
            }
            lmsliteratureIndex = new LmsLiteratureIndex() { };
            spinnerService.Hide();
        }

        protected async Task Form0Submit(LmsLiteratureIndex args)
        {
            try
            {
                var registeredIndexNumber = await lmsLiteratureIndexService.GetLmsLiteratureIndexesIdByIndexNumber(args.IndexNumber);
                if (registeredIndexNumber.Count() == 0)
                {
                    lmsliteratureIndex.ParentId = int.Parse(ParentIndexId);
                    lmsliteratureIndex.IndexParentNumber = Convert.ToString(ParentIndexNumber);
                    var response = await lmsLiteratureIndexService.CreateLmsLiteratureIndex(lmsliteratureIndex);
                    if (response.IsSuccessStatusCode)
                    {
                        lmsliteratureIndex = (LmsLiteratureIndex)response.ResultData;
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Literature_Index_Create_Success"),

                            //Summary = $"????!", 
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        StateHasChanged();
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }
                    dialogService.Close(lmsliteratureIndex);
                    await Load();

                    //await Task.Delay(200);
                    //await Load();
                }
                else
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Info,
                        Detail = translationState.Translate("Index_number_is_already_registered_Please_enter_another_number"),
                        //Summary = $"??????!", 
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    registeredIndexNumber = null;
                }
            }
            catch (Exception)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Contact_Administrator"),
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
