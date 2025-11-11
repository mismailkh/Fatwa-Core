using FATWA_ADMIN.Services.General;
using FATWA_DOMAIN.Enums.Common;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.Lookups;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using Radzen;
using Radzen.Blazor;
using System.Linq.Dynamic.Core;
using System.Text.RegularExpressions;
using static FATWA_GENERAL.Helper.Response;
using Court = FATWA_DOMAIN.Models.CaseManagment.Court;
using Group = FATWA_DOMAIN.Models.AdminModels.UserManagement.Group;

namespace FATWA_ADMIN.Pages.LookupsManagment
{
    public partial class CaseFileNumberAdd : ComponentBase
    {

        #region Parameter
        [Parameter]
        public int SelectedPatternTypeId { get; set; }
        [Parameter]
        public bool IsDefault { get; set; }
        public int Selectedvalue { get; set; }
        [Parameter]
        public dynamic Id { get; set; }
        [Parameter]
        public int updateCount { get; set; }
        [Parameter(CaptureUnmatchedValues = true)]
        public IReadOnlyDictionary<string, dynamic> Attributes { get; set; }
        #endregion

        #region Variable Declaration
        List<string> sequenceParts = new List<string>();
        protected User? selectedUser = null;
        protected Group? selectedUserGroup = null;
        public bool TextboxEnable { get; set; }
        public bool SequenceResultEnable { get; set; } = false;
        public int TextboxYear { get; set; }
        public bool VisibleCaseFileNumber { get; set; }
        public bool VisibleCaseRequestNumber { get; set; }
        public bool VisibleConsaltationRequestNumber { get; set; }
        public bool TextboxStaticTextPattern { get; set; } = true;
        public bool TextboxStaticTextPatternValidation { get; set; }
        public bool TextboxSequanceNumber { get; set; }
        public bool DisableDay { get; set; }
        public bool DisableDayvalidation { get; set; } = true;
        public bool DisableGovtEntityvalidation { get; set; } = true;
        public bool DisableMonth { get; set; }
        public bool DisableMonthvalidation { get; set; } = true;
        public bool DisableStaticTextPatternValidation { get; set; } = true;
        public bool DisableYear { get; set; }
        public bool DisableYearValidation { get; set; } = true;
        public bool GovernmentEntityValidation { get; set; } = false;
        public bool DisableStaticTextPattern { get; set; }
        public string TypId { get; set; }

        string selectedValue = string.Empty;
        string selectedDay = string.Empty;
        string selectedCatory = string.Empty;
        public int? selectedDaySequence { get; set; } = 0;
        string selectedMonth = string.Empty;
        int? selectedMonthSequence = 0;
        string selectedYear = string.Empty;
        int? selectedYearSequence = 0;
        string selectedStaticTextPattern = string.Empty;
        int? selectedStaticTextPatternSequence = 0;
        int? selectedSequanceNumber = 0;

        public bool showSelectAllOption = true;
        bool isGovtEntityExist = false;
        public string concatenatedUserNames;
        protected int count { get; set; }
        private bool showWarning = false;
        bool IsGEPatternValidator = false;
        private Timer debouncer;
        private const int debouncerDelay = 500;
        Regex rgx = new Regex("[^a-zA-Z?-?]");

        public List<string> optionsList;
        public List<string> optionsDayList;
        public List<string> optionsNumberTypeList;
        public List<string> optionsMonList;

        public List<int> optionsYearList;
        public List<int> optionsSequenceList;

        RadzenDropDown<int> DropDownDayValueResetToZero = new RadzenDropDown<int>();
        RadzenDropDown<int> DropDownMonthValueResetToZero = new RadzenDropDown<int>();
        RadzenDropDown<int> DropDownYearValueResetToZero = new RadzenDropDown<int>();
        RadzenDropDown<int> DropDownCharaterStringValueResetToZero = new RadzenDropDown<int>(); // late on remove
        RadzenDropDown<int> DropDownStaticTextPatternValueResetToZero = new RadzenDropDown<int>();
        RadzenDropDown<int> DropDownSequenceNumberValueResetToZero = new RadzenDropDown<int>();
        RadzenDropDown<string> govEntityDropdown;

        public List<Group> UserGroups { get; set; } = new List<Group>();
        public List<GovernmentEntity> GovernmentEntity { get; set; } = new List<GovernmentEntity>();
        public List<CmsGovtEntityNumPattern> CmsGovtEntityNumPattern { get; set; } = new List<CmsGovtEntityNumPattern>();
        public List<CmsComsNumPattern> cmsComsNumPatterns { get; set; } = new List<CmsComsNumPattern>();
        public IList<Group> SelectUsersGroup;
        public CmsComsNumPatternVM cmsComsNumPatternVM { get; set; } = new CmsComsNumPatternVM();
        public List<GovernmentEntitiesPatternVM> cmsComsNumPatternVMdetail { get; set; } = new List<GovernmentEntitiesPatternVM>();
        public GovernmentEntitiesUsersVM GovernmentEntitiesUser { get; set; }
        public List<Court> CourtNames { get; set; } = new List<Court>();
        public List<Group> getUserGroupDetails = null;
        public List<string> SelectedUserNames { get; set; } = new List<string>();
        public List<CmsComsNumPatternType> getCmsComsNumPatternTypedetails = null;
        protected RadzenDataGrid<CmsComsNumPatternHistoryVM>? grid1 = new RadzenDataGrid<CmsComsNumPatternHistoryVM>();

        CmsComsNumPatternType _cmsComsNumPatterntype;

        #endregion

        #region Functions
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }
        protected override async Task OnInitializedAsync()
        {
            if ((SelectedPatternTypeId == (int)CmsComsNumPatternTypeEnum.CaseRequestNumber || SelectedPatternTypeId == (int)CmsComsNumPatternTypeEnum.ConsultationRequestNumber))
                await GetAllGEUserList();
            await GetCmsComsNumberPatterntypes();
            await GetYearList();
            await GetSequenceList();
            await GetMonthList();
            await GetDayList();
            await Load();

            if (Id == null)
            {

            }
            else
            {
                await CmsHistorydetail();
            }

        }

        #endregion

        #region Number Pattern History Lists
        IEnumerable<CmsComsNumPatternHistoryVM> CmsComsNumPatternHistoryDetail;
        IEnumerable<CmsComsNumPatternHistoryVM> _FilteredCmsComsNumPatternHistoryDetail;
        protected IEnumerable<CmsComsNumPatternHistoryVM> FilteredCmsComsNumPatternHistoryDetail
        {
            get
            {
                return _FilteredCmsComsNumPatternHistoryDetail;
            }
            set
            {
                if (!object.Equals(_FilteredCmsComsNumPatternHistoryDetail, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "FilteredCmsComsNumPatternHistoryDetail", NewValue = value, OldValue = _FilteredCmsComsNumPatternHistoryDetail };
                    _FilteredCmsComsNumPatternHistoryDetail = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }
        public IEnumerable<int> IgnoredGovernamentEntityIds { get; set; } = new List<int>();

        string _search;
        protected string search
        {
            get
            {
                return _search;
            }
            set
            {
                if (!object.Equals(_search, value))
                {
                    var args = new FATWA_ADMIN.Services.General.PropertyChangedEventArgs() { Name = "search", NewValue = value, OldValue = _search };
                    _search = value;

                    Reload();
                }
            }
        }
        protected CmsComsNumPatternType cmsComsNumPatterntype
        {
            get
            {
                return _cmsComsNumPatterntype;
            }
            set
            {
                if (!object.Equals(_cmsComsNumPatterntype, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "cmsComsNumPatterntype", NewValue = value, OldValue = _cmsComsNumPatterntype };
                    _cmsComsNumPatterntype = value;
                    OnPropertyChanged(args);
                }
            }
        }


        CmsComsNumPattern _cmsComsNumPattern;
        protected CmsComsNumPattern cmsComsNumPattern
        {
            get
            {
                return _cmsComsNumPattern;
            }
            set
            {
                if (!object.Equals(_cmsComsNumPattern, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "cmsComsNumPattern", NewValue = value, OldValue = _cmsComsNumPattern };
                    _cmsComsNumPattern = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        List<CmsComsNumPatternGroups> _cmsComsNumPatternGroups;
        protected List<CmsComsNumPatternGroups> cmsComsNumPatternGroups
        {
            get
            {
                return _cmsComsNumPatternGroups;
            }
            set
            {
                if (!object.Equals(_cmsComsNumPatternGroups, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "cmsComsNumPattern", NewValue = value, OldValue = _cmsComsNumPatternGroups };
                    _cmsComsNumPatternGroups = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        CmsComsNumPatternHistory _cmsComsNumPatternHistory;
        protected CmsComsNumPatternHistory cmsComsNumPatternHistory
        {
            get
            {
                return _cmsComsNumPatternHistory;
            }
            set
            {
                if (!object.Equals(_cmsComsNumPatternHistory, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "cmsComsNumPattern", NewValue = value, OldValue = _cmsComsNumPatternHistory };
                    _cmsComsNumPatternHistory = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }
        #endregion

        #region populate Court Name 
        protected async Task GetAllGEUserList()
        {
            var response = await lookupService.GetAllUserGroupsList();
            if (response.IsSuccessStatusCode)
            {
                GovernmentEntity = (List<GovernmentEntity>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        protected async Task GetCmsComsNumberPatterntypes()
        {
            var response = await lookupService.GetCmsComsNumberPatterntype();
            if (response.IsSuccessStatusCode)
            {
                getCmsComsNumPatternTypedetails = (List<CmsComsNumPatternType>)response.ResultData;

            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }

        #endregion

        #region Add, Edit and Save Number Pattern
        ApiCallResponse response = new ApiCallResponse();
        protected async Task Load()
        {
            if (Id == null) // add form
            {
                spinnerService.Show();
                cmsComsNumPattern = new CmsComsNumPattern()
                {
                    Id = Guid.NewGuid(),
                    PatternTypId = SelectedPatternTypeId,
                };
                spinnerService.Hide();
            }
            else  // Edit form
            {
                spinnerService.Show();
                //cmsComsNumPatternHistory.PatternTypId = SelectedPatternTypeId;
                response = await lookupService.GetCmsComsNumberPatternHistory(Id);
                if (response.IsSuccessStatusCode)
                {
                    CmsComsNumPatternHistory history = (CmsComsNumPatternHistory)response.ResultData;
                    CmsComsNumPattern pattern = new CmsComsNumPattern
                    {
                        PatternTypId = history.PatternTypId,
                        Day = history.Day,
                        D_Order = history.D_Order,
                        Month = history.Month,
                        M_Order = history.M_Order,
                        Year = history.Year,
                        Y_Order = history.Y_Order,
                        CreatedDate = history.CreatedDate,
                        CreatedBy = history.CreatedBy,
                        StaticTextPattern = history.StaticTextPattern,
                        STP_Order = history.STP_Order,
                        CmsGovtEntityNumPatternGroup = history.CmsGovtEntityNumPatternGroup,
                        usersGroup = history.usersGroup,
                        GroupIds = history.GroupIds,
                        UserName = history.UserName,
                        SequanceResult = history.SequanceResult,
                        SequanceNumber = history.SequanceNumber,
                        SN_Order = history.SN_Order,
                        ResetYearly = history.ResetYearly,
                        SequanceFormatResult = history.SequanceFormatResult,
                        Id = (Guid)history.PatternId,
                        IsDefault = history.IsDefault,
                    };

                    // Now 'pattern' contains the values from 'history'
                    cmsComsNumPattern = pattern;
                    SelectedPatternTypeId = (int)cmsComsNumPattern.PatternTypId;

                    selectedYearSequence = cmsComsNumPattern.Y_Order;
                    selectedMonthSequence = cmsComsNumPattern.M_Order;
                    selectedDaySequence = cmsComsNumPattern.D_Order;
                    selectedSequanceNumber = cmsComsNumPattern.SN_Order;

                    selectedStaticTextPattern = cmsComsNumPattern.StaticTextPattern;
                    selectedMonth = cmsComsNumPattern.Month;
                    selectedYear = cmsComsNumPattern.Year;
                    selectedDay = cmsComsNumPattern.Day;
                    selectedStaticTextPatternSequence = cmsComsNumPattern.STP_Order;
                    //cmsComsNumPattern.GovtEntityNumPatternId = cmsComsNumPattern.GovtEntityNumPatternId;

                    if (cmsComsNumPattern.STP_Order == null)
                    {
                        DisableStaticTextPattern = true;
                        selectedStaticTextPatternSequence = 0;
                        DisableStaticTextPatternValidation = false;
                    }
                    else
                    {
                        DisableStaticTextPattern = false;
                        DisableStaticTextPatternValidation = false;
                    }
                    if (cmsComsNumPattern.Month == "")
                    {
                        DisableMonth = true;
                        selectedMonthSequence = 0;
                        DisableMonthvalidation = false;
                    }
                    else
                    {
                        DisableMonth = false;
                        DisableMonthvalidation = false;
                    }
                    if (cmsComsNumPattern.Year == "")
                    {
                        DisableYear = true;
                        selectedYearSequence = 0;
                        DisableYearValidation = false;
                    }
                    else
                    {
                        DisableYear = false;
                        DisableYearValidation = false;
                    }
                    if (cmsComsNumPattern.Day == "")
                    {
                        DisableDay = true;
                        selectedDaySequence = 0;
                        DisableDayvalidation = false;
                    }
                    else
                    {
                        DisableDay = false;
                        DisableDayvalidation = false;
                    }

                    if (cmsComsNumPattern.StaticTextPattern == "")
                    {
                        DisableStaticTextPattern = true;
                        selectedStaticTextPatternSequence = 0;
                        DisableStaticTextPatternValidation = false;
                    }
                    else
                    {
                        DisableStaticTextPattern = false;
                        DisableStaticTextPatternValidation = false;
                    }

                    if (SelectedPatternTypeId == (int)CmsComsNumPatternTypeEnum.CaseRequestNumber || SelectedPatternTypeId == (int)CmsComsNumPatternTypeEnum.ConsultationRequestNumber)
                    {
                        var Result = await lookupService.GetAllAGEUserListPatternAttached(Id, SelectedPatternTypeId, IsDefault);
                        if (Result.IsSuccessStatusCode)
                        {
                            cmsComsNumPatternVMdetail = (List<GovernmentEntitiesPatternVM>)Result.ResultData;
                            IgnoredGovernamentEntityIds = cmsComsNumPatternVMdetail.Select(vm => vm.EntityId);
                            if (string.IsNullOrEmpty(history.UpdatedGovtEntities))
                            {
                                cmsComsNumPattern.GovernamentEntityIds = cmsComsNumPatternVMdetail.Select(vm => vm.EntityId);
                            }
                            else
                            {
                                cmsComsNumPattern.GovernamentEntityIds = history.UpdatedGovtEntities?.Split(',').Select(int.Parse).ToList();
                            }
                        }
                        else
                        {
                            await invalidRequestHandlerService.ReturnBadRequestNotification(Result);
                        }
                    }
                    spinnerService.Hide();

                    var result = await lookupService.GetCmsComsNumberPatternHistoryForEditing(Id);
                    if (result.IsSuccessStatusCode)
                    {
                        var res = (List<CmsComsNumPatternHistory>)result.ResultData;
                        if (res.Any() && res.First().CreatedDate.Date == DateTime.Today)
                        {
                            bool? userConfirmed = await dialogService.Confirm(
                                translationState.Translate("Update_Number_Pattern_At_MidNight"),
                                translationState.Translate("Confirm"),
                                new ConfirmOptions()
                                {
                                    OkButtonText = @translationState.Translate("OK"),
                                    CancelButtonText = @translationState.Translate("Cancel")
                                }
                                );
                            if (userConfirmed != true && userConfirmed != null)
                                dialogService.Close(true);
                            else
                                return;
                        }
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(result);
                    }
                }
                else
                {
                    response = await lookupService.GetCmsComsNumberPatterntype(Id);
                    if (response.IsSuccessStatusCode)
                    {
                        cmsComsNumPattern = (CmsComsNumPattern)response.ResultData;

                        selectedYearSequence = cmsComsNumPattern.Y_Order;
                        selectedMonthSequence = cmsComsNumPattern.M_Order;
                        selectedDaySequence = cmsComsNumPattern.D_Order;
                        selectedSequanceNumber = cmsComsNumPattern.SN_Order;
                        selectedMonth = cmsComsNumPattern.Month;
                        selectedYear = cmsComsNumPattern.Year;
                        selectedDay = cmsComsNumPattern.Day;

                        SelectedPatternTypeId = (int)cmsComsNumPattern.PatternTypId;

                        //cmsComsNumPattern.GovtEntityNumPatternId = cmsComsNumPattern.GovtEntityNumPatternId;
                        selectedStaticTextPatternSequence = cmsComsNumPattern.STP_Order;

                        if (cmsComsNumPattern.STP_Order == null)
                        {
                            DisableStaticTextPattern = true;
                            selectedStaticTextPatternSequence = 0;
                            DisableStaticTextPatternValidation = false;
                        }
                        else
                        {
                            DisableStaticTextPattern = false;
                            DisableStaticTextPatternValidation = false;
                        }
                        if (cmsComsNumPattern.Month == "")
                        {
                            DisableMonth = true;
                            selectedMonthSequence = 0;
                            DisableMonthvalidation = false;
                        }
                        else
                        {
                            DisableMonth = false;
                            DisableMonthvalidation = false;
                        }
                        if (cmsComsNumPattern.Year == "")
                        {
                            DisableYear = true;
                            selectedYearSequence = 0;
                            DisableYearValidation = false;
                        }
                        else
                        {
                            DisableYear = false;
                            DisableYearValidation = false;
                        }
                        if (cmsComsNumPattern.Day == "")
                        {
                            DisableDay = true;
                            selectedDaySequence = 0;
                            DisableDayvalidation = false;
                        }
                        else
                        {
                            DisableDay = false;
                            DisableDayvalidation = false;
                        }
                        if (SelectedPatternTypeId == (int)CmsComsNumPatternTypeEnum.CaseRequestNumber || SelectedPatternTypeId == (int)CmsComsNumPatternTypeEnum.ConsultationRequestNumber)
                        {
                            var Result = await lookupService.GetAllAGEUserListPatternAttached(Id, SelectedPatternTypeId, IsDefault);
                            if (Result.IsSuccessStatusCode)
                            {
                                cmsComsNumPatternVMdetail = (List<GovernmentEntitiesPatternVM>)Result.ResultData;
                                IgnoredGovernamentEntityIds = cmsComsNumPattern.GovernamentEntityIds = cmsComsNumPatternVMdetail.Select(vm => vm.EntityId);
                            }
                            else
                            {
                                await invalidRequestHandlerService.ReturnBadRequestNotification(Result);
                            }
                        }
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }
                    spinnerService.Hide();

                }
            }

        }

        protected async Task SaveChanges(CmsComsNumPattern args)
        {
            try
            {
                if (cmsComsNumPattern.GovernamentEntityIds is null || !cmsComsNumPattern.GovernamentEntityIds.Any()
                    && (SelectedPatternTypeId == (int)CmsComsNumPatternTypeEnum.CaseRequestNumber || SelectedPatternTypeId == (int)CmsComsNumPatternTypeEnum.ConsultationRequestNumber)
                    )
                {
                    GovernmentEntityValidation = true;
                    return;
                }
                if (IsGEPatternValidator)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Government_Entity_Already_Exist"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    return;
                }

                if ((string.IsNullOrEmpty(cmsComsNumPattern.StaticTextPattern) || cmsComsNumPattern.StaticTextPattern.Length > 4) && !DisableStaticTextPattern)
                {
                    DisableStaticTextPatternValidation = true;
                    cmsComsNumPattern.StaticTextPattern = string.Empty;
                    DisableStaticTextPattern = false;
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Invalid_Static_Text_Value"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    return;
                }
                bool? dialogResponse = await dialogService.Confirm(
                                translationState.Translate("Sure_Submit"),
                                translationState.Translate("Confirm"),
                                new ConfirmOptions()
                                {
                                    OkButtonText = translationState.Translate("OK"),
                                    CancelButtonText = translationState.Translate("Cancel")
                                });
                if (dialogResponse == true)
                {
                    if (Id == null)
                    {
                        spinnerService.Show();
                        if (IsGEPatternValidator)
                        {
                            return;
                        }
                        else if (cmsComsNumPattern.GovernamentEntityIds.Any())
                        {
                            var response = await lookupService.SaveCMSCOMSPattrenNumber(cmsComsNumPattern);
                            if (response.IsSuccessStatusCode)
                            {
                                notificationService.Notify(new NotificationMessage()
                                {
                                    Severity = NotificationSeverity.Success,
                                    Detail = translationState.Translate("Pattern_Added_Successfully"),
                                    Style = "position: fixed !important; left: 0; margin: auto; "
                                });
                            }
                            else
                            {
                                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                            }
                        }
                        else
                        {
                            GovernmentEntityValidation = true;
                        }
                    }
                    else
                    {
                        // Maintain a record of government entities that have been updated, but not those which were already selected
                        string UpdatedGovtEntities = IgnoredGovernamentEntityIds.OrderBy(x => x).SequenceEqual(args.GovernamentEntityIds.OrderBy(x => x)) ? null : string.Join(",", args.GovernamentEntityIds);
                        CmsComsNumPatternHistory PatternHistory = new CmsComsNumPatternHistory
                        {
                            Id = Guid.NewGuid(),
                            PatternId = args.Id,
                            PatternTypId = args.PatternTypId,
                            SequanceFormatResult = args.SequanceFormatResult,
                            SequanceNumber = args.SequanceNumber,
                            StaticTextPattern = args.StaticTextPattern,
                            STP_Order = args.STP_Order,
                            SN_Order = args.SN_Order,
                            Y_Order = args.Y_Order,
                            M_Order = args.M_Order,
                            D_Order = args.D_Order,
                            Year = args.Year,
                            Month = args.Month,
                            Day = args.Day,
                            GovernamentEntityIds = args.GovernamentEntityIds,
                            CmsGovtEntityNumPatternGroup = args.CmsGovtEntityNumPatternGroup,
                            GroupIds = args.GroupIds,
                            SequanceResult = args.SequanceResult,
                            ResetYearly = args.ResetYearly,
                            IsDefault = args.IsDefault,
                            UpdatedGovtEntities = UpdatedGovtEntities
                        };


                        if (await dialogService.Confirm(
                                translationState.Translate("Record_Not_Reflected_Immediately"),
                                translationState.Translate("Confirm"),
                                new ConfirmOptions()
                                {
                                    OkButtonText = translationState.Translate("OK"),
                                    CancelButtonText = translationState.Translate("Cancel")
                                }) == true)
                        {
                            spinnerService.Show();
                            var patternhistoryresult = await lookupService.UpdateCaseFileNumberPattrenHistory(PatternHistory);
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("Pattern_Updated_Successfully"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                        }
                    }

                    dialogService.Close(true);
                    StateHasChanged();
                    spinnerService.Hide();
                }
            }
            catch (Exception)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = Id == null ? translationState.Translate("Could_not_create_a_new_Pattern") : translationState.Translate("Pattern_could_not_be_updated"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                spinnerService.Hide();
            }
        }

        #endregion

        #region Sequence Dropdown change event
        protected async Task OnDaySequenceDropDownValueChanged(int args)
        {
            if (!string.IsNullOrEmpty(cmsComsNumPattern.Day))
            {
                if (args != selectedSequanceNumber &&
                    args != selectedStaticTextPatternSequence &&
                    args != selectedYearSequence &&
                    args != selectedMonthSequence)
                {
                    cmsComsNumPattern.D_Order = args;
                }
                else
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("This_Sequence_Number_Already_Selected"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    selectedDaySequence = 0;
                    DropDownDayValueResetToZero.Reset();
                }
            }
            else
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Please_Select_Pattern_Day"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                selectedDaySequence = 0;
                DropDownDayValueResetToZero.Reset();
            }
            // cmsComsNumPattern.SequanceResult = cmsComsNumPattern.Year +""+ cmsComsNumPattern.SequanceResult;
        }

        protected async Task OnMonthSequenceDropDownValueChanged(int args)
        {
            if (!string.IsNullOrEmpty(cmsComsNumPattern.Month))
            {
                if (args >= 1 && args <= 5)
                {
                    if (args != selectedSequanceNumber &&
                        args != selectedStaticTextPatternSequence &&
                        args != selectedYearSequence &&
                        args != selectedDaySequence)
                    {
                        cmsComsNumPattern.M_Order = args;
                    }
                    else
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Error,
                            Detail = translationState.Translate("This_Sequence_Number_Already_Selected"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        selectedMonthSequence = 0;
                        DropDownMonthValueResetToZero.Reset();
                    }
                }
            }
            else
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Please_Select_Month_Pattern_First"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                selectedMonthSequence = 0;
                DropDownMonthValueResetToZero.Reset();
            }

            // cmsComsNumPattern.SequanceResult = cmsComsNumPattern.Year +""+ cmsComsNumPattern.SequanceResult;
        }

        protected async Task OnYearSequenceDropDownValueChanged(int args)
        {
            if (!string.IsNullOrEmpty(cmsComsNumPattern.Year))
            {
                if (args != selectedSequanceNumber &&
                    args != selectedStaticTextPatternSequence &&
                    args != selectedMonthSequence &&
                    args != selectedDaySequence)
                {
                    cmsComsNumPattern.Y_Order = args;
                }
                else
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("This_Sequence_Number_Already_Selected"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    selectedYearSequence = 0;
                    DropDownYearValueResetToZero.Reset();
                }
            }
            else
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Please_Select_Year_Pattern_First"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                selectedYearSequence = 0;
                DropDownYearValueResetToZero.Reset();
            }
            // cmsComsNumPattern.SequanceResult = cmsComsNumPattern.Year +""+ cmsComsNumPattern.SequanceResult;
        }

        protected async Task OnCharaterStringSequanceDropDownValueChanged(int args)
        {
            if (!string.IsNullOrEmpty(cmsComsNumPattern.StaticTextPattern))
            {
                if (args >= 1 && args <= 5)
                {
                    if (args != selectedSequanceNumber &&
                        args != selectedMonthSequence &&
                        args != selectedYearSequence &&
                        args != selectedDaySequence)
                    {
                        cmsComsNumPattern.STP_Order = args;
                    }
                    else
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Error,
                            Detail = translationState.Translate("This_Sequence_Number_Already_Selected"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        selectedStaticTextPatternSequence = 0;
                        DropDownCharaterStringValueResetToZero.Reset();
                    }
                }
                else
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("This_Sequence_Number_Already_Selected"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    selectedStaticTextPatternSequence = 0;
                    DropDownCharaterStringValueResetToZero.Reset();
                }
            }
            else
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Please_Select_Character_String_Pattern_First"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                selectedStaticTextPatternSequence = 0;
                DropDownCharaterStringValueResetToZero.Reset();
            }
        }
        protected async Task OnSequanceNumberDropDownValueChanged(int args)
        {
            if (!string.IsNullOrEmpty(cmsComsNumPattern.SequanceNumber))
            {
                if (args >= 1 && args <= 5)
                {
                    if (args != selectedYearSequence &&
                        args != selectedStaticTextPatternSequence &&
                        args != selectedMonthSequence &&
                        args != selectedDaySequence)
                    {
                        cmsComsNumPattern.SN_Order = args;
                    }
                    else
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Error,
                            Detail = translationState.Translate("This_Sequence_Number_Already_Selected"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        selectedSequanceNumber = 0;
                        DropDownSequenceNumberValueResetToZero.Reset();
                    }
                }
                else
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("This_Sequence_Number_Already_Selected"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    selectedSequanceNumber = 0;
                    DropDownSequenceNumberValueResetToZero.Reset();
                }
            }
            else
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Please_Select_Sequence_Number_Pattern_First"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                selectedSequanceNumber = 0;
                DropDownSequenceNumberValueResetToZero.Reset();
            }
        }
        #endregion

        #region Static DDl
        protected async Task GetDayList()
        {
            try
            {
                // Initialize the list to store the formatted month values
                optionsDayList = new List<string>
        {
            "DD"
        };
            }
            catch (Exception)
            {

                throw;
            }


        }
        protected async Task GetMonthList()
        {
            try
            {
                // Initialize the list to store the formatted month values
                optionsMonList = new List<string>
        {
            "MM",
            "MMM",
            "MMMM"
        };
            }
            catch (Exception)
            {

                throw;
            }


        }
        protected async Task GetNumberTypeList()
        {
            try
            {
                // Initialize the list to store the formatted month values
                optionsNumberTypeList = new List<string>
        {
            "CaseRequestNumber",
            "CaseFileNumber",
            "ConsaltationRequestNumber"
        };
            }
            catch (Exception)
            {

                throw;
            }


        }
        protected async Task GetYearList()
        {
            try
            {
                optionsList = new List<string>
        {
            "YY",
            "YYYY"
        };
            }
            catch (Exception)
            {

                throw;
            }


        }
        protected async Task GetSequenceList()
        {
            try
            {
                int Start = 1;
                int end = 5;

                if (end < Start)
                {
                    int temp = end;
                    end = Start;
                    Start = temp;
                }
                int count = end - Start + 1;
                optionsSequenceList = Enumerable.Range(Start, count).OrderByDescending(x => x).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Checkbox On Change Event
        protected async Task OnIsDisableDayValueChanged(bool value)
        {
            if (DisableDay == false)
            {
                DisableDay = true;
                DisableDayvalidation = false;
                selectedDay = string.Empty;
                selectedDaySequence = 0;
                cmsComsNumPattern.Day = string.Empty;
                if (!string.IsNullOrEmpty(cmsComsNumPattern.SequanceResult))
                {
                    // Use a regular expression to remove "Month" from the string
                    string pattern = "DD";
                    string modifiedValue = Regex.Replace(cmsComsNumPattern.SequanceResult, pattern, string.Empty);

                    cmsComsNumPattern.SequanceResult = modifiedValue;
                }
            }
            else
            {
                DisableDay = false;
                DisableDayvalidation = true;
                selectedDay = string.Empty;
                selectedDaySequence = 0;
                cmsComsNumPattern.Day = string.Empty;
            }
            // cmsComsNumPattern.SequanceResult = selectedSequanceNumber.ToString();
            // cmsComsNumPattern.SequanceResult = cmsComsNumPattern.Year +""+ cmsComsNumPattern.SequanceResult;
        }
        protected async Task OnIsDisableMonthValueChanged(bool value)
        {
            if (DisableMonth == false)
            {
                DisableMonth = true;
                DisableMonthvalidation = false;

                selectedMonth = string.Empty;
                selectedMonthSequence = 0;
                cmsComsNumPattern.Month = string.Empty;
                if (!string.IsNullOrEmpty(cmsComsNumPattern.SequanceResult))
                {
                    // Use a regular expression to remove "Month" from the string
                    string pattern = "MMMM|MM";
                    string modifiedValue = Regex.Replace(cmsComsNumPattern.SequanceResult, pattern, string.Empty);
                    //pattern = "M";
                    //modifiedValue = Regex.Replace(modifiedValue, pattern, string.Empty);

                    cmsComsNumPattern.SequanceResult = modifiedValue;
                }
            }
            else
            {
                DisableMonth = false;
                DisableMonthvalidation = true;
                selectedMonth = string.Empty;
                selectedMonthSequence = 0;
                cmsComsNumPattern.Month = string.Empty;
            }
            // cmsComsNumPattern.SequanceResult = selectedSequanceNumber.ToString();
            // cmsComsNumPattern.SequanceResult = cmsComsNumPattern.Year +""+ cmsComsNumPattern.SequanceResult;
        }
        protected async Task OnIsDisableYearValueChanged(bool value)
        {
            if (DisableYear == false)
            {
                //visiableIsYearIncrement = false;
                DisableYear = true;
                DisableYearValidation = false;
                selectedYear = string.Empty;
                selectedYearSequence = 0;

                cmsComsNumPattern.Year = string.Empty;
                if (!string.IsNullOrEmpty(cmsComsNumPattern.SequanceResult))
                {
                    // Use a regular expression to remove "YYYY" from the string
                    string pattern = "YYYY|YY";
                    string modifiedValue = Regex.Replace(cmsComsNumPattern.SequanceResult, pattern, string.Empty);

                    cmsComsNumPattern.SequanceResult = modifiedValue;
                }
            }
            else
            {
                //visiableIsYearIncrement = true;
                DisableYear = false;
                DisableYearValidation = true;
                selectedYear = string.Empty;
                selectedYearSequence = 0;
                cmsComsNumPattern.Year = string.Empty;
            }
            // cmsComsNumPattern.SequanceResult = selectedSequanceNumber.ToString();
            // cmsComsNumPattern.SequanceResult = cmsComsNumPattern.Year +""+ cmsComsNumPattern.SequanceResult;
        }
        protected async Task OnIsCharaterStringDisableValueChanged(bool value)
        {
            if (DisableStaticTextPattern == false)
            {
                DisableStaticTextPattern = true;
                DisableStaticTextPatternValidation = false;
                cmsComsNumPattern.StaticTextPattern = string.Empty;
                selectedStaticTextPatternSequence = 0;
                if (!string.IsNullOrEmpty(cmsComsNumPattern.SequanceResult))
                {
                    // Use a regular expression to remove "YYYY" from the string
                    string pattern = @"(?<![YMD])[^YMD\d]|(?<=D)(?![D\d])[A-Za-z]";
                    string modifiedValue = Regex.Replace(cmsComsNumPattern.SequanceResult, pattern, string.Empty);

                    cmsComsNumPattern.SequanceResult = modifiedValue;
                }
            }
            else
            {
                DisableStaticTextPattern = false;
                DisableStaticTextPatternValidation = true;
                selectedStaticTextPatternSequence = 0;

            }
            // cmsComsNumPattern.SequanceResult = selectedSequanceNumber.ToString();
            // cmsComsNumPattern.SequanceResult = cmsComsNumPattern.Year +""+ cmsComsNumPattern.SequanceResult;
        }
        #endregion

        #region Change and API Functions
        protected async Task OnYearDropDownValueChanged(string value)
        {
            cmsComsNumPattern.Year = selectedYear;
        }

        protected async Task OnDayDropDownValueChanged(string value)
        {
            cmsComsNumPattern.Day = selectedDay;
        }
        protected async Task OnNumberTypeDropDownValueChanged(int PatternTypeId)
        {
            if (PatternTypeId == (int)CmsComsNumPatternTypeEnum.CaseRequestNumber)
            {
                VisibleCaseRequestNumber = true;
                //VisibleCaseFileNumber = false;
            }
            else if (PatternTypeId == (int)CmsComsNumPatternTypeEnum.ConsultationRequestNumber)
                //{
                //	VisibleCaseFileNumber = false;
                VisibleConsaltationRequestNumber = true;
        }


        protected async Task OnMonthDownValueChanged(string value)
        {
            cmsComsNumPattern.Month = selectedMonth;
        }
        protected async void OnUserChange(object GroupId, string name)
        {
            if (getUserGroupDetails != null)
            {
                selectedUserGroup = getUserGroupDetails.FirstOrDefault(c => c.Name_En == (string)name);
                if (selectedUser != null)
                {
                    await InvokeAsync(StateHasChanged);
                }


            }
        }
        protected void OnInput(ChangeEventArgs e)
        {
            string input = e.Value?.ToString();

            // Remove any non-zero digits from the input
            string zerosOnly = new string('0', input.Length);

            // Update the binding value with the restricted input
            cmsComsNumPattern.SequanceNumber = zerosOnly;
        }
        protected async Task OnIsYearUpdateValueChanged(bool value)
        {

            cmsComsNumPattern.ResetYearly = value;


            // cmsComsNumPattern.SequanceResult = selectedSequanceNumber.ToString();
            // cmsComsNumPattern.SequanceResult = cmsComsNumPattern.Year +""+ cmsComsNumPattern.SequanceResult;
        }
        protected void OnInputStaticText(ChangeEventArgs e)
        {
            cmsComsNumPattern.StaticTextPattern = e.Value.ToString().ToUpper();
            if (cmsComsNumPattern.StaticTextPattern.Length > 4 || string.IsNullOrEmpty(cmsComsNumPattern.StaticTextPattern))
            {
                cmsComsNumPattern.StaticTextPattern = string.Empty;
                DisableStaticTextPatternValidation = true;
            }
        }
        protected async Task Button2Click(MouseEventArgs args)
        {
            dialogService.Close(false);
        }
        protected async Task PreviewClick(MouseEventArgs args)
        {
            List<string> sequenceParts = new List<string>();
            SequenceResultEnable = true;

            if (DisableStaticTextPattern == true)
                cmsComsNumPattern.STP_Order = 0;
            if (DisableDay == true)
                cmsComsNumPattern.D_Order = 0;
            if (DisableMonth == true)
                cmsComsNumPattern.M_Order = 0;
            if (DisableYear == true)
                cmsComsNumPattern.Y_Order = 0;

            if (cmsComsNumPattern.Y_Order != null && cmsComsNumPattern.SN_Order != null && cmsComsNumPattern.STP_Order != null && cmsComsNumPattern.M_Order != null && cmsComsNumPattern.D_Order != null)
            {
                SequenceResultEnable = true;
            }

            for (int i = 1; i <= 5; i++)
            {
                if (cmsComsNumPattern.Y_Order == i && cmsComsNumPattern.SN_Order != i && cmsComsNumPattern.STP_Order != i && cmsComsNumPattern.D_Order != i && cmsComsNumPattern.M_Order != i)
                {
                    sequenceParts.Add(cmsComsNumPattern.Year);
                }
                else if (cmsComsNumPattern.SN_Order == i && cmsComsNumPattern.Y_Order != i && cmsComsNumPattern.STP_Order != i && cmsComsNumPattern.D_Order != i && cmsComsNumPattern.M_Order != i)
                {
                    sequenceParts.Add(cmsComsNumPattern.SequanceNumber);
                }
                else if (cmsComsNumPattern.STP_Order == i && cmsComsNumPattern.Y_Order != i && cmsComsNumPattern.SN_Order != i && cmsComsNumPattern.D_Order != i && cmsComsNumPattern.M_Order != i)
                {
                    sequenceParts.Add(cmsComsNumPattern.StaticTextPattern);
                }
                else if (cmsComsNumPattern.D_Order == i && cmsComsNumPattern.Y_Order != i && cmsComsNumPattern.SN_Order != i && cmsComsNumPattern.STP_Order != i && cmsComsNumPattern.M_Order != i)
                {
                    sequenceParts.Add(cmsComsNumPattern.Day);
                }
                else if (cmsComsNumPattern.M_Order == i && cmsComsNumPattern.Y_Order != i && cmsComsNumPattern.SN_Order != i && cmsComsNumPattern.STP_Order != i && cmsComsNumPattern.D_Order != i)
                {
                    sequenceParts.Add(cmsComsNumPattern.Month);
                }
            }
            cmsComsNumPattern.SequanceResult = string.Join("", sequenceParts);
            cmsComsNumPattern.SequanceFormatResult = string.Join("/-/", sequenceParts);
            StateHasChanged();
        }

        protected async Task CmsHistorydetail()
        {
            ApiCallResponse result = await lookupService.GetCmsComNumPatternHistoryDetail(Id);
            if (result.IsSuccessStatusCode)
            {

                CmsComsNumPatternHistoryDetail = (IEnumerable<CmsComsNumPatternHistoryVM>)result.ResultData;
                FilteredCmsComsNumPatternHistoryDetail = (IEnumerable<CmsComsNumPatternHistoryVM>)result.ResultData;
                count = FilteredCmsComsNumPatternHistoryDetail.Count();
                await InvokeAsync(StateHasChanged);
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(result);
            }
        }
        protected async Task OnSearchInput(string value)
        {
            try
            {
                debouncer?.Dispose();
                debouncer = new Timer(async (e) =>
                {
                    search = string.IsNullOrEmpty(value) ? "" : value.TrimStart().TrimEnd().ToLower();
                    FilteredCmsComsNumPatternHistoryDetail = await gridSearchExtension.Filter(CmsComsNumPatternHistoryDetail, new Query()
                    {
                        Filter = $@"i => (i.StaticTextPattern != null && i.StaticTextPattern.ToLower().Contains(@0))",
                        FilterParameters = new object[] { search.ToLower() }
                    });  await InvokeAsync(StateHasChanged);
                }, null, debouncerDelay, Timeout.Infinite);
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
        protected async void PatternValidator(object args)
        {
            if (IsDefault)
                return;
            if (cmsComsNumPattern.GovernamentEntityIds != null && cmsComsNumPattern.GovernamentEntityIds.Any())
            {
                var entityIdsCopy = new List<int>(cmsComsNumPattern.GovernamentEntityIds);
                entityIdsCopy.RemoveAll(id => IgnoredGovernamentEntityIds.Contains(id));
                var response = await lookupService.CheckPatternAlreadyAttachedGovtid(entityIdsCopy, SelectedPatternTypeId);
                if (response.IsSuccessStatusCode)
                {
                    if ((bool)response.ResultData)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Error,
                            Detail = translationState.Translate("Government_Entity_Already_Exist"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                    }
                    IsGEPatternValidator = (bool)response.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
        }
        #endregion
    }
}

