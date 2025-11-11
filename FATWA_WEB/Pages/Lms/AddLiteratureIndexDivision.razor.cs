using FATWA_DOMAIN.Models;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;

namespace FATWA_WEB.Pages.Lms
{
    public partial class AddLiteratureIndexDivision : ComponentBase
    {
        public AddLiteratureIndexDivision()
        {
            getDivisionRecordsByUsingIndexId = new List<LmsLiteratureIndexDivisionAisle>();
            registeredDivisionNumber = new List<LmsLiteratureIndexDivisionAisle>();
            DivisionAisleNumberCheck = false;
        }

        #region Variables declaration
        private List<LmsLiteratureIndexDivisionAisle> getDivisionRecordsByUsingIndexId;

        private List<LmsLiteratureIndexDivisionAisle> registeredDivisionNumber;

        private bool DivisionAisleNumberCheck;

        [Parameter]
        public dynamic IndexId { get; set; }

        [Parameter(CaptureUnmatchedValues = true)]
        public IReadOnlyDictionary<string, dynamic> Attributes { get; set; }

        LmsLiteratureIndexDivisionAisle _lmsliteratureIndexDivision;
        protected LmsLiteratureIndexDivisionAisle lmsliteratureIndexDivision
        {
            get
            {
                return _lmsliteratureIndexDivision;
            }
            set
            {
                if (!object.Equals(_lmsliteratureIndexDivision, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "lmsliteratureIndexDivision", NewValue = value, OldValue = _lmsliteratureIndexDivision };
                    _lmsliteratureIndexDivision = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        #endregion



        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }

        #region Service Injections
         
          
         
          
         
         
         
          

         
          

         
        protected LmsLiteratureIndexDivisionService lmsIndexDivisionServices { get; set; }

         
          

        #endregion

        #region Initialization
        protected override async Task OnInitializedAsync()
        {
            await Load();
        }
        protected async System.Threading.Tasks.Task Load()
        {
            spinnerService.Show();
            lmsliteratureIndexDivision = new LmsLiteratureIndexDivisionAisle() { };
            spinnerService.Hide();
        }
        #endregion

        #region Create
        LmsLiteratureIndexDivisionAisle lmsLiteratureIndexDivisionAisle;
        protected async Task Form0Submit(LmsLiteratureIndexDivisionAisle args)
        {
            try
            {
                // get all divisions & aisle records by using index id to check if division & aisle number is already created.
                getDivisionRecordsByUsingIndexId = await lmsIndexDivisionServices.GetLmsLiteratureDivisionDetailsByUsingIndexId(Convert.ToInt32(IndexId));
                if (getDivisionRecordsByUsingIndexId.Count() == 0)
                {
                    args.IndexId = Convert.ToInt32(IndexId);
                    args.DivisionCreationDate = DateTime.Now;
                    args.IsDeleted = false;

                    var response = await lmsIndexDivisionServices.CreateLmsLiteratureIndexDivision(args);
                    if (response.IsSuccessStatusCode)
                    {
                        lmsLiteratureIndexDivisionAisle = (LmsLiteratureIndexDivisionAisle)response.ResultData;
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Section_created_successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        StateHasChanged();
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }
                    dialogService.Close(lmsliteratureIndexDivision);


                }
                else
                {
                    foreach (var item in getDivisionRecordsByUsingIndexId)
                    {
                        if (item.DivisionNumber == args.DivisionNumber && item.AisleNumber == args.AisleNumber)
                        {
                            DivisionAisleNumberCheck = true;
                        }
                    }
                    if (DivisionAisleNumberCheck == false)
                    {
                        args.IndexId = Convert.ToInt32(IndexId);
                        args.DivisionCreationDate = DateTime.Now;
                        args.IsDeleted = false;

                        var fatwaDbCreateLmsLiteratureIndexResult = await lmsIndexDivisionServices.CreateLmsLiteratureIndexDivision(args);
                        dialogService.Close(lmsliteratureIndexDivision);

                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Section_created_successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                    }
                    else
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Info,
                            Detail = translationState.Translate("Section_and_lane_number_is_already_saved_please_enter_another_number"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        DivisionAisleNumberCheck = false;
                    }
                }
            }
            catch (Exception)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Could_not_create_a_new_literature"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
        #endregion

        protected async Task Button2Click(MouseEventArgs args)
        {
            dialogService.Close(null);
        }
    }
}
