using FATWA_DOMAIN.Models;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;

namespace FATWA_WEB.Pages.Lms
{
    public partial class AddLiteratureParentIndex : ComponentBase
    {
        public AddLiteratureParentIndex()
        {
        }
        #region variable declaration
        [Parameter]
        public int ParentIndexId { get; set; } = 0;
        public string OldParentIndexNumber { get; set; } = null;
        public IReadOnlyDictionary<string, dynamic> Attributes { get; set; }
        public LmsLiteratureParentIndex lmsLiteratureParentIndexLoad { get; set; } = new LmsLiteratureParentIndex();
        #endregion
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }

        LmsLiteratureParentIndex _lmsliteratureParentIndex;
        protected LmsLiteratureParentIndex lmsliteratureParentIndex
        {
            get
            {
                return _lmsliteratureParentIndex;
            }
            set
            {
                if (!object.Equals(_lmsliteratureParentIndex, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "lmsliteratureParentIndex", NewValue = value, OldValue = _lmsliteratureParentIndex };
                    _lmsliteratureParentIndex = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        public LmsLiteratureParentIndex parentIndexDetails { get; private set; }

        protected override async Task OnInitializedAsync()
        {
            await Load();
        }
        protected async Task Load()
        {
            spinnerService.Show();
            if (ParentIndexId == 0)
            {
                lmsliteratureParentIndex = new LmsLiteratureParentIndex() { };
            }
            else
            {
                lmsliteratureParentIndex = await lmsliteratureParentIndexServices.GetLiteratureParentIndexDetailById(ParentIndexId);
                if (lmsliteratureParentIndex != null)
                {
                    OldParentIndexNumber = lmsliteratureParentIndex.ParentIndexNumber;
                }
            }
            spinnerService.Hide();
        }

        protected async Task FormSubmit(LmsLiteratureParentIndex args)
        {
            try
            {
                if (ParentIndexId == 0)
                {
                    // check if parent number is already saved
                    var result = await lmsliteratureParentIndexServices.CheckLiteratureParentIndexByUsingParentNumber(args.ParentIndexNumber);
                    if (!result)
                    {
                        var response = await lmsliteratureParentIndexServices.CreateLmsLiteratureParentIndex(lmsliteratureParentIndex);
                        if (response.IsSuccessStatusCode)
                        {
                            lmsliteratureParentIndex = (LmsLiteratureParentIndex)response.ResultData;
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("Literature_Parent_Index_Create_Success"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                            StateHasChanged();
                        }
                        else
                        {
                            await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                        }
                        dialogService.Close(lmsliteratureParentIndex);

                    }
                    else
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Info,
                            Detail = translationState.Translate("Parent_Index_number_is_already_registered_Please_enter_another_number"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                    }
                }
                else
                {
                    // first check if parent index number and name's already saved in table.
                    var registeredIndexNumber = await lmsliteratureParentIndexServices.CheckLiteratureParentIndexByUsingParentIndexNumber(args);
                    if (!registeredIndexNumber)
                    {
                        if (OldParentIndexNumber == args.ParentIndexNumber)
                        {
                            // if only parent index name's are change.
                            parentIndexDetails = await lmsliteratureParentIndexServices.GetLmsLiteratureParentIndexDetailByNumber(OldParentIndexNumber);
                            if (parentIndexDetails != null)
                            {
                                parentIndexDetails.ParentIndexId = parentIndexDetails.ParentIndexId;
                                parentIndexDetails.Name_En = args.Name_En;
                                parentIndexDetails.Name_Ar = args.Name_Ar;
                                parentIndexDetails.ParentIndexNumber = args.ParentIndexNumber;
                                parentIndexDetails.CreatedBy = parentIndexDetails.CreatedBy;
                                parentIndexDetails.CreatedDate = parentIndexDetails.CreatedDate;
                                parentIndexDetails.DeletedBy = parentIndexDetails.DeletedBy;
                                parentIndexDetails.DeletedDate = parentIndexDetails.DeletedDate;
                                parentIndexDetails.IsDeleted = parentIndexDetails.IsDeleted;
                                var response = await lmsliteratureParentIndexServices.UpdateLmsLiteratureParentIndex(parentIndexDetails.ParentIndexId, parentIndexDetails);
                                if (response.IsSuccessStatusCode)
                                {
                                    lmsliteratureParentIndex = (LmsLiteratureParentIndex)response.ResultData;
                                    notificationService.Notify(new NotificationMessage()
                                    {
                                        Severity = NotificationSeverity.Success,
                                        Detail = translationState.Translate("Changes_saved_successfully"),
                                        //Summary = $"????!", 
                                        Style = "position: fixed !important; left: 0; margin: auto; "
                                    });
                                    StateHasChanged();
                                }
                                else
                                {
                                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                                }
                                dialogService.Close(lmsliteratureParentIndex);


                            }
                        }
                        else
                        {
                            // again check if parent number is already saved
                            var result = await lmsliteratureParentIndexServices.CheckLiteratureParentIndexByUsingParentNumber(args.ParentIndexNumber);
                            if (!result)
                            {
                                //check if only parent number are change
                                var Result = await lmsliteratureParentIndexServices.UpdateLmsLiteratureParentIndex(lmsliteratureParentIndex.ParentIndexId, lmsliteratureParentIndex);
                                dialogService.Close(lmsliteratureParentIndex);
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
                                    Detail = translationState.Translate("Parent_Index_number_is_already_registered_Please_enter_another_number"),
                                    Style = "position: fixed !important; left: 0; margin: auto; "
                                });
                            }
                        }
                    }
                    else
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Info,
                            Detail = translationState.Translate("Parent_Index_number_is_already_registered_Please_enter_another_number"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                    }
                }
            }
            catch (Exception)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Could_not_create_a_new_index"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }

        protected async Task ButtonCancelClick(MouseEventArgs args)
        {
            dialogService.Close(null);
        }
    }
}
