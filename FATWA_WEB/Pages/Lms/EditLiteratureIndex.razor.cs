using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;

namespace FATWA_WEB.Pages.Lms
{
    public partial class EditLiteratureIndex : ComponentBase
    {
        public EditLiteratureIndex()
        {
            IndexNumberIndexesId = new List<LmsLiteratureIndex>();
            LmsLiteratureIndexEditCounter = 0;
        }

        #region Variable declaration
        [Parameter(CaptureUnmatchedValues = true)]
        public IReadOnlyDictionary<string, dynamic> Attributes { get; set; }

        private string OldIndexNumber { get; set; } = string.Empty;

        private IEnumerable<LmsLiteratureIndex> IndexNumberIndexesId;

        private IEnumerable<LmsLiterature> lmsLiteraturesResult { get; set; }

        private int LmsLiteratureIndexEditCounter;

        private LmsLiteratureIndex ItemIndex { get; set; }
        private LmsLiterature lmsLiteratureCheck { get; set; }
        #endregion

        #region Service Injections
       
         
          

        #endregion

        #region Parameter
        [Parameter]
        public dynamic IndexId { get; set; }

        protected string ParrentIndexNameAr { get; set; }
        protected string ParrentIndexNameEn { get; set; }
        public LmsLiteratureParentIndexVM ParentIndexDetails { get; set; } = new LmsLiteratureParentIndexVM();

        #endregion

        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }

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

        protected override async Task OnInitializedAsync()
        {
            await Load();
        }
        protected async Task Load()
        {

            spinnerService.Show();
            lmsliteratureIndex = await lmsLiteratureIndexServices.GetLmsLiteratureIndexById(Convert.ToInt32(IndexId));

            var result = await lmsLiteratureIndexService.GetLiteratureIndexByIndexIdAndNumber(lmsliteratureIndex.ParentId, lmsliteratureIndex.IndexParentNumber);
            if (result.IsSuccessStatusCode)
            {
                ParentIndexDetails = (LmsLiteratureParentIndexVM)result.ResultData;
            }
            else
            {
                ParentIndexDetails = new LmsLiteratureParentIndexVM();
            }
            OldIndexNumber = lmsliteratureIndex.IndexNumber;
            spinnerService.Hide();
        }

        protected async Task Form0Submit(LmsLiteratureIndex args)
        {
            try
            {
                // first check if name_en, name_ar and index number already saved in table.
                var registeredIndexNumber = await lmsLiteratureIndexServices.GetLmsLiteratureIndexDetailByUsingNameAndIndexNumber(args);
                if (registeredIndexNumber.Count() == 0)
                {
                    if (args.IndexNumber == OldIndexNumber)
                    {
                        // if only index name are change.
                        IndexNumberIndexesId = await lmsLiteratureIndexServices.GetLmsLiteratureIndexesIdByIndexNumber(OldIndexNumber);
                        if (IndexNumberIndexesId != null)
                        {
                            foreach (var item in IndexNumberIndexesId)
                            {
                                lmsliteratureIndex.IndexId = item.IndexId;
                                lmsliteratureIndex.ParentId = item.ParentId;
                                lmsliteratureIndex.IndexParentNumber = item.IndexParentNumber;
                                lmsliteratureIndex.Name_En = args.Name_En;
                                lmsliteratureIndex.Name_Ar = args.Name_Ar;
                                lmsliteratureIndex.IndexNumber = item.IndexNumber;
                                lmsliteratureIndex.IndexCreationDate = item.IndexCreationDate;
                                lmsliteratureIndex.CreatedBy = item.CreatedBy;
                                lmsliteratureIndex.CreatedDate = item.CreatedDate;
                                lmsliteratureIndex.DeletedBy = item.DeletedBy;
                                lmsliteratureIndex.DeletedDate = item.DeletedDate;
                                lmsliteratureIndex.IsDeleted = item.IsDeleted;
                                var response = await lmsLiteratureIndexServices.UpdateLmsLiteratureIndex((int)item.IndexId, lmsliteratureIndex);
                                if (response.IsSuccessStatusCode)
                                {
                                    lmsliteratureIndex = (LmsLiteratureIndex)response.ResultData;
                                    notificationService.Notify(new NotificationMessage()
                                    {
                                        Severity = NotificationSeverity.Success,
                                        Detail = translationState.Translate("Changes_saved_successfully"),
                                        Style = "position: fixed !important; left: 0; margin: auto; "
                                    });
                                    StateHasChanged();
                                }
                                else
                                {
                                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                                }
                            }
                        }

                    }
                    else
                    {
                        //if index number are change then first check if existinng index number(index id) associated with laterature's.
                        IndexNumberIndexesId = await lmsLiteratureIndexServices.GetLmsLiteratureIndexesIdByIndexNumber(OldIndexNumber);
                        if (IndexNumberIndexesId != null)
                        {
                            foreach (var item in IndexNumberIndexesId)
                            {
                                lmsLiteraturesResult = await lmsLiteratureIndexServices.CheckLmsLiteratureIndexIdAssociatedWithLiteratures(item.IndexId);
                                if (lmsLiteraturesResult.Count() > 0)
                                {
                                    LmsLiteratureIndexEditCounter += lmsLiteraturesResult.Count();
                                }
                            }
                        }
                        if (LmsLiteratureIndexEditCounter == 0)
                        {
                            //check if index number already saved in table.
                            IndexNumberIndexesId = await lmsLiteratureIndexServices.GetLmsLiteratureIndexesIdByIndexNumber(args.IndexNumber);
                            if (IndexNumberIndexesId.Count() == 0)
                            {
                                // get saved index number details and bind with new index number.
                                IndexNumberIndexesId = await lmsLiteratureIndexServices.GetLmsLiteratureIndexesIdByIndexNumber(OldIndexNumber);
                                if (IndexNumberIndexesId != null)
                                {
                                    foreach (var item in IndexNumberIndexesId)
                                    {
                                        lmsliteratureIndex.IndexId = item.IndexId;
                                        lmsliteratureIndex.ParentId = item.ParentId;
                                        lmsliteratureIndex.IndexParentNumber = item.IndexParentNumber;
                                        lmsliteratureIndex.Name_En = args.Name_En;
                                        lmsliteratureIndex.Name_Ar = args.Name_Ar;
                                        lmsliteratureIndex.IndexNumber = args.IndexNumber;
                                        lmsliteratureIndex.IndexCreationDate = item.IndexCreationDate;
                                        lmsliteratureIndex.CreatedBy = item.CreatedBy;
                                        lmsliteratureIndex.CreatedDate = item.CreatedDate;
                                        lmsliteratureIndex.DeletedBy = item.DeletedBy;
                                        lmsliteratureIndex.DeletedDate = item.DeletedDate;
                                        lmsliteratureIndex.IsDeleted = item.IsDeleted;

                                        var fatwaDbUpdateLmsLiteratureIndexResult = await lmsLiteratureIndexServices.UpdateLmsLiteratureIndex(item.IndexId, lmsliteratureIndex);
                                    }
                                }
                                notificationService.Notify(new NotificationMessage()
                                {
                                    Severity = NotificationSeverity.Success,
                                    Detail = translationState.Translate("Changes_saved_successfully"),
                                    Style = "position: fixed !important; left: 0; margin: auto; "
                                });
                            }
                            else
                            {
                                notificationService.Notify(new NotificationMessage()
                                {
                                    Severity = NotificationSeverity.Info,
                                    Detail = translationState.Translate("Index_number_is_already_registered_Please_enter_another_number"),
                                    Style = "position: fixed !important; left: 0; margin: auto; "
                                });
                                IndexNumberIndexesId = null;
                            }
                        }
                        else
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Info,
                                Detail = translationState.Translate("This_index_number_is_associated_with") + " " + LmsLiteratureIndexEditCounter + " " + translationState.Translate("Authors_The_connected_index_number_cannot_be_edited"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                            LmsLiteratureIndexEditCounter = 0;
                            lmsLiteraturesResult = null;
                        }
                    }
                    dialogService.Close(lmsliteratureIndex);
                    await Load();
                }
                else
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Info,
                        Detail = translationState.Translate("Index_number_is_already_registered_Please_enter_another_number"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    registeredIndexNumber = null;
                }
            }
            catch (Exception ex)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Contact_Administrator") + " " + ex.Message,
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
