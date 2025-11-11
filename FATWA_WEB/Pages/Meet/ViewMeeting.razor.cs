using FATWA_DOMAIN.Common;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.MeetModels;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using FATWA_DOMAIN.Models.ViewModel.CommunicationVMs;
using FATWA_DOMAIN.Models.ViewModel.Meet;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using static FATWA_DOMAIN.Enums.CommunicationEnums;
using static FATWA_DOMAIN.Enums.MeetingEnums;

namespace FATWA_WEB.Pages.Meet
{
    public partial class ViewMeeting : ComponentBase
    {

        #region Constructor
        public ViewMeeting()
        {
            ShowAttandenceCheckBoxs = false;
            UserVMDetail = new FatwaAttendeeVM();
            CheckBoxValue = new List<bool>();
            selectedLegislationAttandee = new List<FatwaAttendeeVM>();
            selectedGEAttandee = new List<MeetingAttendeeVM>();

        }
        public SaveMomVM meetingMomVM = new SaveMomVM()
        {
            Meeting = new Meeting(),
            MeetingMom = new MeetingMom(),
            GeAttendee = new List<MeetingAttendeeVM>(),
            LegislationAttendee = new List<FatwaAttendeeVM>()
        };
        #endregion

        #region Parameters

        [Parameter]
        public dynamic CommunicationId { get; set; }
        [Parameter]
        public dynamic CommunicationTypeId { get; set; }
        [Parameter]
        public dynamic ReferenceId { get; set; }
        [Parameter]
        public dynamic MeetingId { get; set; }
        [Parameter]
        public dynamic IsView { get; set; }
        [Parameter]
        public dynamic SubModuleId { get; set; }


        #endregion

        #region Variables Declarations

        protected RadzenDropDown<int> ddlMeetingTypes;
        protected RadzenDropDown<Guid?> ddlFileNumber;
        protected RadzenDropDown<string> ddlLegislationAttendees;
        protected RadzenDropDown<int> ddlGovtEntities;
        protected RadzenDropDown<int> ddlDepartments;
        protected RadzenSteps steps;
        protected RadzenDataGrid<FatwaAttendeeVM> LegislationAttendeeGrid;
        protected RadzenDataGrid<MeetingAttendeeVM> GeAttendeeGrid;

        int selectedIndex = 0;
        bool isSaveAllowed = true;

        private DateTime selectedDate = DateTime.Now.Date;
        protected bool isOnline = false;

        SaveMeetingVM saveMeeting = new SaveMeetingVM()
        {
            Meeting = new Meeting(),
            GeAttendee = new List<MeetingAttendeeVM>(),
            DeletedGeAttendeeIds = new List<Guid>(),
            LegislationAttendee = new List<FatwaAttendeeVM>(),
            DeletedLegislationAttendeeIds = new List<Guid>(),
            MeetingMom = new MeetingMom()
        };
        protected List<MeetingType> MeetingTypes = new List<MeetingType>();

        protected string MeetingTypeEn;

        protected string MeetingTypeAr;

        protected string FileNumber;
        protected List<GovernmentEntity> GovtEntities = new List<GovernmentEntity>();
        protected List<Department> Departments = new List<Department>();

        //DDL
        protected List<FatwaAttendeeVM> LegislationAttendees = new List<FatwaAttendeeVM>();

        //Grid
        protected List<FatwaAttendeeVM> GetLegislationAttendees = new List<FatwaAttendeeVM>();
        protected List<MeetingAttendeeVM> GetGeAttendees = new List<MeetingAttendeeVM>();

        protected User LegislationAttendee = new User();
        protected MeetingAttendeeVM GeAttendee = new MeetingAttendeeVM();
        protected MeetingMom meetingMom = new MeetingMom();
        protected CommunicationSendMessageVM communicationSendMessage = new CommunicationSendMessageVM();
        protected bool isEdit = false;
        int atttendeeLegislationSerialNo = 0;
        int atttendeeGeSerialNo = 0;
        public bool ShowAttandenceCheckBoxs { get; set; }
        protected FatwaAttendeeVM UserVMDetail { get; set; }
        public IEnumerable<bool> CheckBoxValue { get; set; }

        public IList<FatwaAttendeeVM> selectedLegislationAttandee { get; set; }
        public IList<MeetingAttendeeVM> selectedGEAttandee { get; set; }
        public bool allowRowSelectOnRowClick = true;


        bool IsConsultationUser = false;

        #endregion

        #region Request Response Need More Detail View  Load Properties Load

        CommunicationMeetingDetailVM _communicationMeetingDetailVM;
        protected CommunicationMeetingDetailVM communicationMeetingDetailVM
        {
            get
            {
                return _communicationMeetingDetailVM;
            }
            set
            {
                if (!object.Equals(_communicationMeetingDetailVM, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "communicationMeetingDetailVM", NewValue = value, OldValue = _communicationMeetingDetailVM };
                    _communicationMeetingDetailVM = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }


        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {

        }
        #endregion

        #region On Load

        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();

            //Meeting 
            if (loginState.UserDetail.RoleId == SystemRoles.CaseRoles.FirstOrDefault())
            {
                IsConsultationUser = false;
            }
            else if (loginState.UserDetail.RoleId == SystemRoles.ConsultationRoles.FirstOrDefault())
            {
                IsConsultationUser = true;
            }
            if (MeetingId == null)
            {
                if (bool.Parse(IsView) == true)
                {
                    if (int.Parse(CommunicationTypeId) == (int)CommunicationTypeEnum.MeetingScheduled)
                    {
                        await GetMeetingIdCommunitationbyIds();
                        var response = await meetingService.GetMeetingById((Guid)communicationMeetingDetailVM.MeetingId);
                        if (response.IsSuccessStatusCode)
                        {
                            saveMeeting = (SaveMeetingVM)response.ResultData;
                            isOnline = saveMeeting.Meeting.IsOnline;
                            await GetReferenceNumber((Guid)saveMeeting.Meeting.ReferenceGuid, saveMeeting.Meeting.SubModulId);
                            GetLegislationAttendees = saveMeeting.LegislationAttendee;
                            atttendeeLegislationSerialNo = saveMeeting.LegislationAttendee.Count();

                            GetGeAttendees = saveMeeting.GeAttendee;
                            atttendeeGeSerialNo = saveMeeting.GeAttendee.Count();

                            Reload();
                        }
                    }
                    else
                    {
                        var response = await communicationService.GetCommunicationDetailCommunicationId(Guid.Parse(CommunicationId));
                        if (response.IsSuccessStatusCode)
                        {
                            var communication = (CommunicationVM)response.ResultData;
                            if (communication is not null)
                            {
                                if (communication.CommunicationMeeting.IsReplyForMeetingRequest)
                                {
                                    var responseMeeting = await communicationService.GetMeetingDetailByUsingCommunicationId(Guid.Parse(CommunicationId));
                                    if (responseMeeting.IsSuccessStatusCode)
                                    {
                                        saveMeeting.Meeting = (Meeting)responseMeeting.ResultData;
                                        saveMeeting.Meeting.SectorTypeId = loginState.UserDetail.SectorTypeId;
                                        saveMeeting.Meeting.SentBy = communication.Communication.CreatedBy;
                                        saveMeeting.Meeting.ReceivedBy = loginState.Username;
                                        saveMeeting.CommunicationId = Guid.Parse(CommunicationId);
                                    }
                                }
                                else
                                {
                                    saveMeeting.CommunicationId = Guid.Parse(CommunicationId);
                                    saveMeeting.Meeting.CommunicationId = Guid.Parse(CommunicationId);
                                    saveMeeting.Meeting.Subject = communication.CommunicationMeeting.Subject;
                                    saveMeeting.Meeting.Description = communication.CommunicationMeeting.Description;
                                    saveMeeting.Meeting.Agenda = communication.CommunicationMeeting.Agenda;
                                    saveMeeting.Meeting.Date = communication.CommunicationMeeting.Date;
                                    saveMeeting.Meeting.StartTime = communication.CommunicationMeeting.StartTime;
                                    saveMeeting.Meeting.EndTime = communication.CommunicationMeeting.EndTime;
                                    saveMeeting.Meeting.IsOnline = communication.CommunicationMeeting.IsOnline;
                                    saveMeeting.Meeting.MeetingLink = communication.CommunicationMeeting.MeetingLink;
                                    saveMeeting.Meeting.RequirePassword = communication.CommunicationMeeting.RequirePassword;
                                    saveMeeting.Meeting.Location = communication.CommunicationMeeting.Location;
                                    saveMeeting.Meeting.MeetingPassword = communication.CommunicationMeeting.MeetingPassword;
                                    saveMeeting.Meeting.MeetingStatusEn = communication.CommunicationMeeting.MeetingStatusEn;
                                    saveMeeting.Meeting.MeetingStatusAr = communication.CommunicationMeeting.MeetingStatusAr;
                                    saveMeeting.Meeting.MeetingTypeId = (int)MeetingTypeEnum.External;
                                    saveMeeting.Meeting.MeetingStatusId = (int)MeetingStatusEnum.OnHold;
                                    saveMeeting.Meeting.SubModulId = Convert.ToInt32(SubModuleId);
                                    saveMeeting.Meeting.ReferenceGuid = Guid.Parse(ReferenceId);
                                    saveMeeting.Meeting.CreatedBy = communication.Communication.CreatedBy;
                                    saveMeeting.Meeting.CreatedDate = communication.Communication.CreatedDate;
                                    saveMeeting.Meeting.IsDeleted = communication.Communication.IsDeleted;
                                    saveMeeting.Meeting.SectorTypeId = loginState.UserDetail.SectorTypeId;
                                    saveMeeting.Meeting.SentBy = communication.Communication.CreatedBy;
                                    saveMeeting.Meeting.ReceivedBy = loginState.Username;
                                    saveMeeting.Meeting.Note = communication.CommunicationMeeting.Note;
                                    saveMeeting.Meeting.IsReplyForMeetingRequest = communication.CommunicationMeeting.IsReplyForMeetingRequest;

                                    //saveMeeting.Meeting.CommunicationId = Guid.Parse(CommunicationId);
                                    //saveMeeting.Meeting.Subject = communication.CommunicationMeeting.Subject;
                                    //saveMeeting.Meeting.Description = communication.CommunicationMeeting.Description;
                                    //saveMeeting.Meeting.Agenda = communication.CommunicationMeeting.Agenda;
                                    //saveMeeting.Meeting.Date = communication.CommunicationMeeting.Date;
                                    //saveMeeting.Meeting.StartTime = communication.CommunicationMeeting.StartTime;
                                    //saveMeeting.Meeting.EndTime = communication.CommunicationMeeting.EndTime;
                                    //saveMeeting.Meeting.MeetingTypeId = (int)MeetingTypeEnum.External;
                                    //saveMeeting.Meeting.ReferenceGuid = Guid.Parse(ReferenceId);
                                    //saveMeeting.Meeting.MeetingStatusId = communication.CommunicationMeeting.StatusId;
                                    //saveMeeting.Meeting.Note = communication.CommunicationMeeting.Note;
                                    //saveMeeting.Meeting.IsReplyForMeetingRequest = communication.CommunicationMeeting.IsReplyForMeetingRequest;
                                    await GetReferenceNumber(Guid.Parse(ReferenceId), int.Parse(SubModuleId));
                                }
                                if (communication.MeetingAttendee.Any())
                                {
                                    GetGeAttendees = communication.MeetingAttendee;
                                    atttendeeGeSerialNo = communication.MeetingAttendee.Count();
                                }
                                if (communication.FatwaAttendee.Any())
                                {
                                    GetLegislationAttendees = communication.FatwaAttendee.Where(x => x.Id != null).ToList();
                                }
                            }
                            Reload();
                        }
                    }
                    isEdit = false;
                } else
                {
                    saveMeeting.Meeting.MeetingId = Guid.NewGuid();
                    saveMeeting.Meeting.StartTime = DateTime.Now.Date;
                    saveMeeting.Meeting.EndTime = DateTime.Now.Date;
                    saveMeeting.MeetingMom.MeetingId = saveMeeting.Meeting.MeetingId;
                }
            }
            else
            {
                if (CommunicationId == null)
                {
                    if (bool.Parse(IsView) == true)
                    {
                        var responsedetail = await meetingService.GetMeetingById(Guid.Parse(MeetingId));
                        if (responsedetail.IsSuccessStatusCode)
                        {
                            saveMeeting = (SaveMeetingVM)responsedetail.ResultData;
                            isOnline = saveMeeting.Meeting.IsOnline;
                            await GetReferenceNumber((Guid)saveMeeting.Meeting.ReferenceGuid, saveMeeting.Meeting.SubModulId);
                            GetLegislationAttendees = saveMeeting.LegislationAttendee;
                            atttendeeLegislationSerialNo = saveMeeting.LegislationAttendee.Count();

                            GetGeAttendees = saveMeeting.GeAttendee;
                            atttendeeGeSerialNo = saveMeeting.GeAttendee.Count();

                            Reload();
                        }
                        isEdit = false;
                    }
                    else
                    {
                        var response = await meetingService.GetMeetingById(Guid.Parse(MeetingId));
                        if (response.IsSuccessStatusCode)
                        {
                            saveMeeting = (SaveMeetingVM)response.ResultData;
                            isOnline = saveMeeting.Meeting.IsOnline;
                            await GetReferenceNumber((Guid)saveMeeting.Meeting.ReferenceGuid, saveMeeting.Meeting.SubModulId);

                            GetLegislationAttendees = saveMeeting.LegislationAttendee;
                            atttendeeLegislationSerialNo = saveMeeting.LegislationAttendee.Count();

                            GetGeAttendees = saveMeeting.GeAttendee;
                            atttendeeGeSerialNo = saveMeeting.GeAttendee.Count();

                            Reload();
                        }
                        isEdit = true;
                    }
                }
                else
                {
                    //await GetMeetingIdCommunitationbyIds();
                    //var response = await meetingService.GetMeetingById((Guid)communicationMeetingDetailVM.MeetingId);
                    //if (response.IsSuccessStatusCode)
                    //{
                    //    saveMeeting = (SaveMeetingVM)response.ResultData;

                    //    GetLegislationAttendees = saveMeeting.LegislationAttendee;
                    //    atttendeeLegislationSerialNo = saveMeeting.LegislationAttendee.Count();

                    //    GetGeAttendees = saveMeeting.GeAttendee;
                    //    atttendeeGeSerialNo = saveMeeting.GeAttendee.Count();

                    //    Reload();
                    //}
                    //isEdit = true;
                    await RetrieveMeetingDetails();
                }


               
                saveMeeting.Meeting.MeetingId = Guid.Parse(MeetingId);
                saveMeeting.MeetingMom.MeetingId = saveMeeting.Meeting.MeetingId;
                await PopulateMOM();
                await PopulateMeetingDetail();
            }
            await PopulateDropdowns();
            spinnerService.Hide();
        }

        protected async Task GetMeetingIdCommunitationbyIds()
        {
            try
            {
                spinnerService.Show();
                string Commid = CommunicationId.ToString();
                var response = await communicationService.GetMeetingIdCommunitationbyId(Commid, int.Parse(CommunicationTypeId));
                if (response.IsSuccessStatusCode)
                {
                    communicationMeetingDetailVM = (CommunicationMeetingDetailVM)response.ResultData;
                }
                await InvokeAsync(StateHasChanged);
                spinnerService.Hide();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Redirect Function
        private async Task PreviousPageUrl()
        {
            await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
        }
        private void GoBackDashboard()
        {
            navigationManager.NavigateTo("/dashboard");
        }
        private void GoBackHomeScreen()
        {
            navigationManager.NavigateTo("/index");
        }
        protected async Task RedirectBack()
        {
            await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
        }
        #endregion

        #region Form Submit click
        protected async Task ViewFormSubmit(SaveMeetingVM sendMeeting)
        {
            bool? dialogResponse = false;
            string dialogMessage = translationState.Translate("Communication_Send_Response");
            string successMessage = translationState.Translate("Send_Response_Success");
            if (selectedLegislationAttandee.Count() != 0 || selectedGEAttandee.Count() != 0)
            {
                sendMeeting.LegislationAttandeeSelected = (List<UserVM>)selectedLegislationAttandee;
                sendMeeting.GEAttandeeSelected = (List<MeetingAttendeeVM>)selectedGEAttandee;
                dialogResponse = await dialogService.Confirm(
                        dialogMessage,
                        translationState.Translate("Confirm"),
                        new ConfirmOptions()
                        {
                            OkButtonText = translationState.Translate("OK"),
                            CancelButtonText = translationState.Translate("Cancel")
                        });

                if (dialogResponse == true)
                {
                    var response = await meetingService.SaveLegislationAttandee(sendMeeting);
                    if (response.IsSuccessStatusCode)
                    {
                        var resultAttandence = (bool)response.ResultData;
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Attandee_Save_Successful"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        dialogService.Close(resultAttandence);
                    }
                }
            }
            else
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Please_MarkAttendance_Atleast_One_Attendee"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                return;
            }

        }
        #endregion

        #region Remote Dropdown Data and Dropdown Change Events

        protected async Task PopulateDropdowns()
        {
            await PopulateMeetingTypes();
            await PopulateGovtEntities();
            //await PopulateDepartments();
            await PopulateLegislationAttendees();
        }

        protected async Task PopulateMeetingTypes()
        {
            var response = await lookupService.GetMeetingTypes();
            if (response.IsSuccessStatusCode)
            {
                MeetingTypes = (List<MeetingType>)response.ResultData;
                if (MeetingTypes.Count() != 0)
                {
                    MeetingTypeEn = MeetingTypes.Where(x => x.MeetingTypeId == saveMeeting.Meeting.MeetingTypeId).Select(x => x.NameEn).FirstOrDefault();
                    MeetingTypeAr = MeetingTypes.Where(x => x.MeetingTypeId == saveMeeting.Meeting.MeetingTypeId).Select(x => x.NameAr).FirstOrDefault();
                }

            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        protected async Task PopulateMOM()
        {
            var response = await meetingService.GetMeetingMOMByMeetingId(Guid.Parse(MeetingId));
            if(response.IsSuccessStatusCode)
            {
                meetingMom =  (MeetingMom)response.ResultData;
                saveMeeting.MeetingMom.MeetingMomId=meetingMom.MeetingMomId;
            }
        }

        public async Task PopulateMeetingDetail()
        {
            try
            {
                var response = await meetingService.GetMeetingDetailById(Guid.Parse(MeetingId));
                if (response.IsSuccessStatusCode)
                {
                    meetingMomVM = (SaveMomVM)response.ResultData; 
                    if (meetingMomVM.LegislationAttendee.Any())
                    {
                        GetLegislationAttendees = meetingMomVM.LegislationAttendee;
                        selectedLegislationAttandee = meetingMomVM.LegislationAttendee.Where(x => x.IsPresent == true).ToList();
                        atttendeeLegislationSerialNo = meetingMomVM.LegislationAttendee.Count();
                    }

                    if (meetingMomVM.GeAttendee.Any())
                    {
                        GetGeAttendees = meetingMomVM.GeAttendee;
                        atttendeeGeSerialNo = meetingMomVM.GeAttendee.Count();
                        selectedGEAttandee = meetingMomVM.GeAttendee.Where(x => x.IsPresent == true).ToList(); ;
                    }
                    meetingMomVM.MeetingMom.Content = meetingMomVM.MeetingMom.Content;

                    Reload();
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        protected async Task GetReferenceNumber(Guid ReferenceId, int SubModulId)
        {
            var response = await lookupService.GetReferenceNumber(ReferenceId, SubModulId);
            if (response.IsSuccessStatusCode)
            {
                FileNumber = (string)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }

        protected async Task PopulateGovtEntities()
        {
            var response = await lookupService.GetGovernmentEntities();
            if (response.IsSuccessStatusCode)
            {
                GovtEntities = (List<GovernmentEntity>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        } 
        protected async Task PopulateLegislationAttendees()
        {
            var response = await lookupService.GetAttendeeUser();
            if (response.IsSuccessStatusCode)
            {
                LegislationAttendees = (List<FatwaAttendeeVM>)response.ResultData;
                LegislationAttendees.OrderBy(x => x.FirstNameEnglish);
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }

        #endregion

        #region Functions

        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        protected bool isPassReq = false;
        void IsPasswordRequired(bool value)
        {
            isPassReq = value;
        }
        #endregion 

        #region Cancel button click
        protected void BtnCancel(MouseEventArgs args)
        {
            navigationManager.NavigateTo("/usertask-list");

        }
        #endregion

        //#region Add Take Attendance
        //protected async Task AddTakeAttendance()
        //{
        //    ShowAttandenceCheckBoxs = true;
        //}
        //#endregion


        private async Task RetrieveMeetingDetails()
        {
            await GetMeetingIdCommunitationbyIds();
            var response = await meetingService.GetMeetingById((Guid)communicationMeetingDetailVM.MeetingId);
            if (response.IsSuccessStatusCode)
            {
                saveMeeting = (SaveMeetingVM)response.ResultData;

                GetLegislationAttendees = saveMeeting.LegislationAttendee;
                atttendeeLegislationSerialNo = saveMeeting.LegislationAttendee.Count();

                GetGeAttendees = saveMeeting.GeAttendee;
                atttendeeGeSerialNo = saveMeeting.GeAttendee.Count();

                Reload();
            }
            isEdit = true;
        }
    }
}
