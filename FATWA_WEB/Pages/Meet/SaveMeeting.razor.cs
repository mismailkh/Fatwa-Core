
using FATWA_DOMAIN.Common;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.Consultation;
using FATWA_DOMAIN.Models.MeetModels;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.DmsVMs;
using FATWA_DOMAIN.Models.ViewModel.Meet;
using FATWA_DOMAIN.Models.ViewModel.TaskVMs;
using FATWA_GENERAL.Helper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using static FATWA_DOMAIN.Enums.MeetingEnums;
using static FATWA_DOMAIN.Enums.TaskEnums;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_WEB.Pages.Meet
{
    public partial class SaveMeeting : ComponentBase
    {
        #region Parameters

        [Parameter]
        public dynamic MeetingId { get; set; }
        [Parameter]
        public dynamic CommunicationId { get; set; }
        [Parameter]
        public dynamic ReferenceId { get; set; }
        [Parameter]
        public dynamic SubModuleId { get; set; }
        [Parameter]
        public dynamic SectorTypeId { get; set; }
        [Parameter]
        public dynamic ReceivedBy { get; set; }
        [Parameter]
        public string? TaskId { get; set; }
        [Parameter]
        public dynamic IsTask { get; set; }
        #endregion

        #region Variables Declarations
        public bool Istask { get { return Convert.ToBoolean(IsTask); } set { IsTask = value; } }

        protected RadzenDropDown<int> ddlMeetingTypes;
        protected RadzenDropDown<int> ddlSubModule;
        protected RadzenDropDown<Guid?> ddlFileNumber;
        protected RadzenDropDown<string> ddlLegislationAttendees;
        protected RadzenDropDown<int> ddlGovtEntities;
        protected RadzenDropDown<int?> ddlDepartments;
        protected RadzenSteps steps;
        protected RadzenDataGrid<FatwaAttendeeVM> LegislationAttendeeGrid = new RadzenDataGrid<FatwaAttendeeVM>();
        protected RadzenDataGrid<MeetingAttendeeVM> GeAttendeeGrid = new RadzenDataGrid<MeetingAttendeeVM>();
        public List<Guid> RefId { get; set; } = new List<Guid>();
        public bool AllApproved = false;
        public bool IsNewUser = false;
        public bool IsHOS = false;
        public bool IsVice = false;

        int selectedIndex = 0;
        bool isSaveAllowed = true;


        SaveMeetingVM saveMeeting = new SaveMeetingVM()
        {
            Meeting = new Meeting()
            {
                Date = DateTime.Today
            },

            GeAttendee = new List<MeetingAttendeeVM>(),
            DeletedGeAttendeeIds = new List<Guid>(),
            LegislationAttendee = new List<FatwaAttendeeVM>(),
            DeletedLegislationAttendeeIds = new List<Guid>(),
            MeetingMom = new MeetingMom()
        };
        protected List<MeetingType> MeetingTypes = new List<MeetingType>();
        protected List<SubModule> SubModules = new List<SubModule>();
        protected string FileNumber;
        protected TaskDetailVM fileTask { get; set; }
        //protected List<string> FileNumbers = new List<string>();
        //Dictionary<Guid?, string> FileNumbers { get; set; } = new Dictionary<Guid?, string>();

        protected List<ReferenceNumberVM> FilerefNumbers = new List<ReferenceNumberVM>();
        protected List<ConsultationFile> ConsultaitonFileNumbers = new List<ConsultationFile>();
        protected List<GovernmentEntity> GovtEntities = new List<GovernmentEntity>();
        protected List<GEDepartments> Departments = new List<GEDepartments>();
        protected string govtEntityId;

        //DDL
        protected List<FatwaAttendeeVM> LegislationAttendees = new List<FatwaAttendeeVM>();

        //Grid
        protected List<FatwaAttendeeVM> GetLegislationAttendees = new List<FatwaAttendeeVM>();
        protected List<MeetingAttendeeVM>? GetGeAttendees = new List<MeetingAttendeeVM>();
        //protected int GetLegislationAttendeescount { get; set; }
        protected FATWA_DOMAIN.Models.AdminModels.UserManagement.User LegislationAttendee = new FATWA_DOMAIN.Models.AdminModels.UserManagement.User();
        protected MeetingAttendeeVM GeAttendee = new MeetingAttendeeVM();
        protected bool isEdit = false;
        int? atttendeeLegislationSerialNo = 0;
        int atttendeeGeSerialNo = 1;
        protected bool isOnline = false;
        bool isUploaded = false;
        bool isSaveAsDraft;
        bool IsSendToHOS;
        //public bool IsCreateMeeting;
        bool isView = false;
        bool IsConsultationUser = false;
        List<string> validFiles { get; set; } = new List<string>() { ".pdf" };
        public List<MeetingAttendeeStatus> attendeeStatus { get; set; } = new List<MeetingAttendeeStatus>();
        public bool? Temp = false;
        public bool HasRendered = false;
        public bool Ischange = false;
        public string? NewRecievedBy { get; set; } = null;
        protected TaskDetailVM taskDetailVM { get; set; } = new TaskDetailVM();

        protected List<GovernmentEntityRepresentative> GeRepresentatives = new List<GovernmentEntityRepresentative>();
        #endregion
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                HasRendered = true;
            }
        }
        protected async void OnReferenceChange(object args)
        {

            if (saveMeeting.Meeting.ReferenceGuid != null)
            {
                if (saveMeeting.Meeting.SubModulId == (int)SubModuleEnum.OrganizingCommittee)
                {
                    var response = await userService.GetCommitteeMembersByReferenceId((Guid)args);
                    if(response.IsSuccessStatusCode)
                    {
                        LegislationAttendees = (List<FatwaAttendeeVM>)response.ResultData;
                    }
                    GetLegislationAttendees.Clear();
                    await LegislationAttendeeGrid.Reload();
                    atttendeeLegislationSerialNo = 0;
                    Ischange = true;
                    StateHasChanged();
                }
                else
                {
                    await GetGeAttendeeByReferenceId((Guid)saveMeeting.Meeting.ReferenceGuid);
                    Ischange = true;
                    StateHasChanged();
                }
            }
            else
            {
                if (saveMeeting.Meeting.SubModulId == (int)SubModuleEnum.OrganizingCommittee)
                {
                    GetLegislationAttendees.Clear();
                    LegislationAttendees.Clear();
                    await LegislationAttendeeGrid.Reload();
                    atttendeeLegislationSerialNo = 0;
                }
                else
                {
                    GetLegislationAttendees.Clear();
                    await LegislationAttendeeGrid.Reload();
                    atttendeeLegislationSerialNo = 0;
                    await PopulateLegislationAttendees();
                }
            }
        }

        #region On Load

        protected override async Task OnInitializedAsync()
        {
            if (loginState.UserRoles.Any(r => SystemRoles.HOS.Contains(r.RoleId)) || loginState.UserRoles.Any(r => SystemRoles.ComsHOS.Contains(r.RoleId)) || loginState.UserRoles.Any(r => SystemRoles.ViceHOS.Contains(r.RoleId)))
            {
                IsHOS = true;
            }
            if (loginState.UserRoles.Any(r => SystemRoles.ViceHOS.Contains(r.RoleId)))
            {
                IsVice = true;

            }
            if (loginState.UserRoles.Any(r => SystemRoles.CaseRoles.Contains(r.RoleId)))
            {
                IsConsultationUser = false;
            }
            else if (loginState.UserRoles.Any(r => SystemRoles.ConsultationRoles.Contains(r.RoleId)))
            {
                IsConsultationUser = true;
            }
            saveMeeting.Meeting.SectorTypeId = loginState.UserDetail.SectorTypeId;
            spinnerService.Show();

            await PopulateDropdowns();

            //Meeting 

            if (MeetingId == null)
            {
                saveMeeting.Meeting.MeetingId = Guid.NewGuid();
                saveMeeting.MeetingMom.MeetingId = saveMeeting.Meeting.MeetingId;
                if (ReferenceId != null && SubModuleId != null)
                {
                    await GetTaskDetailByReferenceAndUserId();
                    await GetReferenceNumber(Guid.Parse(ReferenceId), Convert.ToInt32(SubModuleId));
                    await GetGovtEnityByReferencId(Guid.Parse(ReferenceId), Convert.ToInt32(SubModuleId));
                    saveMeeting.Meeting.ReferenceGuid = Guid.Parse(ReferenceId);
                    saveMeeting.Meeting.SubModulId = Convert.ToInt32(SubModuleId);
                    NewRecievedBy = System.Net.WebUtility.UrlDecode(ReceivedBy.Replace("%999", "."));
                    if (saveMeeting.Meeting.SubModulId == (int)SubModuleEnum.OrganizingCommittee)
                    {
                        saveMeeting.Meeting.MeetingTypeId = (int)MeetingTypeEnum.Internal;
                        await GetReferenceNumberBySubModuleId(saveMeeting.Meeting.SubModulId);
                        var response = await userService.GetCommitteeMembersByReferenceId(Guid.Parse(ReferenceId));
                        if (response.IsSuccessStatusCode)
                        {
                            var consultationFileAssignmentHistoryVMs = (List<FatwaAttendeeVM>)response.ResultData;

                            foreach (var person in consultationFileAssignmentHistoryVMs)
                            {
                                var AttendeeLawyer = LegislationAttendees.FirstOrDefault(x => person.Id == x.Id);
                                if (AttendeeLawyer is not null)
                                {

                                    AttendeeLawyer.AttendeeStatusId = (int)MeetingAttendeeStatusEnum.New;
                                    AttendeeLawyer.AttendeeStatusEn = attendeeStatus.Where(x => x.Id == (int)MeetingAttendeeStatusEnum.New).Select(y => y.NameEn).FirstOrDefault();
                                    AttendeeLawyer.AttendeeStatusAr = attendeeStatus.Where(x => x.Id == (int)MeetingAttendeeStatusEnum.New).Select(y => y.NameAr).FirstOrDefault();

                                    AttendeeLawyer.SerialNo = ++atttendeeLegislationSerialNo;
                                    GetLegislationAttendees.Add(AttendeeLawyer);

                                }
                            }
                        }

                        await GetCommitteeMembersByCommitteeId((Guid)saveMeeting.Meeting.ReferenceGuid, GetLegislationAttendees.Select(x => x.Id).ToList());
                    }
                    else if (Convert.ToInt32(SubModuleId) == (int)SubModuleEnum.CaseFile)
                    {
                        var res = await cmsCaseFileService.GetCaseAssigment(Guid.Parse(ReferenceId));
                        if (res.IsSuccessStatusCode)
                        {
                            var caseFileAssignees = (List<CaseAssignment>)res.ResultData;
                            // having error when multiple lawyer has the file and same supervisor is appearing multiple time so used hashset
                            var addedSupervisorIds = new HashSet<string>();

                            foreach (var person in caseFileAssignees)
                            {
                                var AttendeeLawyer = LegislationAttendees.FirstOrDefault(x => person.LawyerId == x.Id);
                                if (AttendeeLawyer is not null)
                                {
                                    AttendeeLawyer.AttendeeStatusId = (int)MeetingAttendeeStatusEnum.New;
                                    AttendeeLawyer.AttendeeStatusEn = attendeeStatus.Where(x => x.Id == (int)MeetingAttendeeStatusEnum.New).Select(y => y.NameEn).FirstOrDefault();
                                    AttendeeLawyer.AttendeeStatusAr = attendeeStatus.Where(x => x.Id == (int)MeetingAttendeeStatusEnum.New).Select(y => y.NameAr).FirstOrDefault();

                                    AttendeeLawyer.SerialNo = ++atttendeeLegislationSerialNo;
                                    GetLegislationAttendees.Add(AttendeeLawyer);
                                }

                                var AttendeeSupervisor = LegislationAttendees.FirstOrDefault(x => person.SupervisorId == x.Id);
                                if (AttendeeSupervisor is not null)
                                {
                                    if (addedSupervisorIds.Add(AttendeeSupervisor.Id))
                                    {
                                        AttendeeSupervisor.AttendeeStatusId = (int)MeetingAttendeeStatusEnum.New;
                                        AttendeeSupervisor.AttendeeStatusEn = attendeeStatus.Where(x => x.Id == (int)MeetingAttendeeStatusEnum.New).Select(y => y.NameEn).FirstOrDefault();
                                        AttendeeSupervisor.AttendeeStatusAr = attendeeStatus.Where(x => x.Id == (int)MeetingAttendeeStatusEnum.New).Select(y => y.NameAr).FirstOrDefault();

                                        AttendeeSupervisor.SerialNo = ++atttendeeLegislationSerialNo;
                                        GetLegislationAttendees.Add(AttendeeSupervisor);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        var response = await consultationFileService.GetConsultationAssigment(Guid.Parse(ReferenceId));
                        if (response.IsSuccessStatusCode)
                        {
                            var consultationFileAssignmentHistoryVMs = (List<ConsultationAssignment>)response.ResultData;

                            foreach (var person in consultationFileAssignmentHistoryVMs)
                            {
                                var AttendeeLawyer = LegislationAttendees.FirstOrDefault(x => person.LawyerId == x.Id);
                                if (AttendeeLawyer is not null)
                                {

                                    AttendeeLawyer.AttendeeStatusId = (int)MeetingAttendeeStatusEnum.New;
                                    AttendeeLawyer.AttendeeStatusEn = attendeeStatus.Where(x => x.Id == (int)MeetingAttendeeStatusEnum.New).Select(y => y.NameEn).FirstOrDefault();
                                    AttendeeLawyer.AttendeeStatusAr = attendeeStatus.Where(x => x.Id == (int)MeetingAttendeeStatusEnum.New).Select(y => y.NameAr).FirstOrDefault();

                                    AttendeeLawyer.SerialNo = ++atttendeeLegislationSerialNo;
                                    GetLegislationAttendees.Add(AttendeeLawyer);

                                }
                                var AttendeeSupervisor = LegislationAttendees.FirstOrDefault(x => person.SupervisorId == x.Id);
                                if (AttendeeSupervisor is not null)
                                {

                                    AttendeeSupervisor.AttendeeStatusId = (int)MeetingAttendeeStatusEnum.New;
                                    AttendeeSupervisor.AttendeeStatusEn = attendeeStatus.Where(x => x.Id == (int)MeetingAttendeeStatusEnum.New).Select(y => y.NameEn).FirstOrDefault();
                                    AttendeeSupervisor.AttendeeStatusAr = attendeeStatus.Where(x => x.Id == (int)MeetingAttendeeStatusEnum.New).Select(y => y.NameAr).FirstOrDefault();

                                    AttendeeSupervisor.SerialNo = ++atttendeeLegislationSerialNo;
                                    GetLegislationAttendees.Add(AttendeeSupervisor);

                                }
                            }
                            //await PopulateLegislationAttendees(GetLegislationAttendees.Select(x => x.Id).ToList());
                        }
                    }
                    if (saveMeeting.Meeting.SubModulId != (int)SubModuleEnum.OrganizingCommittee)
                    {
                        await PopulateLegislationAttendees(GetLegislationAttendees.Select(x => x.Id).ToList());
                    }

                }
                OnChangeDate();
            }
            else
            {
                var response = await meetingService.GetMeetingById(Guid.Parse(MeetingId));
                if (response.IsSuccessStatusCode)
                {
                    saveMeeting = (SaveMeetingVM)response.ResultData;
                    isOnline = saveMeeting.Meeting.IsOnline;
                    if (saveMeeting.Meeting.ReferenceGuid != null)
                    {
                        await GetReferenceNumber((Guid)saveMeeting.Meeting.ReferenceGuid, saveMeeting.Meeting.SubModulId);
                        await CheckViceHosApproval((int)loginState.UserDetail.SectorTypeId);
                        await GetGovtEnityByReferencId((Guid)saveMeeting.Meeting.ReferenceGuid, saveMeeting.Meeting.SubModulId);
                        Ischange = true;
                        isEdit = true;
                    }
                    if (saveMeeting.Meeting.SubModulId != null)
                    {
                        GetReferenceNumberBySubModuleId(saveMeeting.Meeting.SubModulId);
                    }

                    GetLegislationAttendees = saveMeeting.LegislationAttendee;
                    atttendeeLegislationSerialNo = saveMeeting.LegislationAttendee.Count();
                    GetGeAttendees = saveMeeting.GeAttendee;
                    atttendeeGeSerialNo = saveMeeting.GeAttendee.Count();
                    var resu = GetLegislationAttendees.Select(x => x.Id).ToList();
                    if(saveMeeting.Meeting.SubModulId == (int)SubModuleEnum.OrganizingCommittee)
                    {
                        await GetCommitteeMembersByCommitteeId((Guid)saveMeeting.Meeting.ReferenceGuid, GetLegislationAttendees.Select(x => x.Id).ToList());
                    }
                    else
                    {
                        await PopulateLegislationAttendees(GetLegislationAttendees.Select(x => x.Id).ToList());
                    }
                    await PopulateGovtEntities(GetGeAttendees.Select(x => x.GovernmentEntityId).ToList());
                    if (GetLegislationAttendees.Count != 0)
                    {
                        AllApproved = GetLegislationAttendees.ToList().All(x => x.AttendeeStatusId != (int)MeetingAttendeeStatusEnum.NotResponded && x.AttendeeStatusId != (int)MeetingAttendeeStatusEnum.New && x.AttendeeStatusId != (int)MeetingAttendeeStatusEnum.Pending);
                    }
                    //if(saveMeeting.Meeting.MeetingStatusId != (int)MeetingStatusEnum.SaveAsDraft)
                    //{
                    //    Ischange = true;
                    //    isEdit = true;
                    //}
                    Reload();
                    //StateHasChanged();  
                }
            }
            if (ReferenceId != null)
            {
                Ischange = true;
                await GetGeAttendeeByReferenceId(Guid.Parse(ReferenceId));

            }
            else if (saveMeeting.Meeting.ReferenceGuid == null)
            {
                Ischange = false;
            }
            spinnerService.Hide();
        }

        #endregion

        #region Remote Dropdown Data and Dropdown Change Events

        //<History Author = 'Hassan Abbas' Date='2022-09-30' Version="1.0" Branch="master">Populate Subtypes data</History>
        protected async Task PopulateDropdowns()
        {
            await PopulateMeetingTypes();
            // await GetReferenceNumber(Guid.Parse(ReferenceId), Convert.ToInt32(SubModuleId));
            //await PopulateFileNumber();
            await PopulateGovtEntities();
            //  await PopulateDepartments();
            await PopulateLegislationAttendees();
            await PopulateSubModuleData();
            await PopulateMeetingAttendeeStatus();
            await PopulateTaskDetails();
            if (saveMeeting.Meeting.ReferenceGuid != null)
            {
                await GetCommitteeMembersByCommitteeId((Guid)saveMeeting.Meeting.ReferenceGuid, new List<string>());

            }

        }
        protected async Task GetGovtEnityByReferencId(Guid ReferenceId, int SubModuleId)
        {
            var response = await cmsSharedService.GetGovtEnityByReferencId(ReferenceId, SubModuleId);
            if (response.IsSuccessStatusCode)
            {
                govtEntityId = (string)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        protected async Task PopulateMeetingTypes()
        {
            var response = await lookupService.GetMeetingTypes();
            if (response.IsSuccessStatusCode)
            {
                MeetingTypes = (List<MeetingType>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }


        protected async Task GetReferenceNumber(Guid ReferenceId, int SubModuleId)
        {
            var response = await lookupService.GetReferenceNumber(ReferenceId, SubModuleId);
            if (response.IsSuccessStatusCode)
            {
                FileNumber = (string)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        protected async Task CheckViceHosApproval(int sectorTypeId)
        {
            var response = await meetingService.CheckViceHosApproval(sectorTypeId);
            if (response.IsSuccessStatusCode)
            {
                saveMeeting.OnlyViceHosApproval = (bool)response.ResultData;

            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }

        protected async Task GetTaskDetailByReferenceAndUserId()
        {

            var taskResponse = await taskService.GetTaskDetailByReferenceAndUserId(Guid.Parse(ReferenceId), loginState.UserDetail.UserId);
            if (taskResponse.IsSuccessStatusCode)
            {
                fileTask = (TaskDetailVM)taskResponse.ResultData;

            }
        }
        protected async Task PopulateTaskDetails()
        {
            if (TaskId != null)
            {
                var getTaskDetail = await taskService.GetTaskDetailById(Guid.Parse(TaskId));
                if (getTaskDetail.IsSuccessStatusCode)
                {
                    taskDetailVM = (TaskDetailVM)getTaskDetail.ResultData;
                }
                else
                {
                    taskDetailVM = new TaskDetailVM();
                }
            }
        }

        protected async Task PopulateGovtEntities(List<int> attendeeIds = null)
        {
            var response = await lookupService.GetGovernmentEntities();
            if (response.IsSuccessStatusCode)
            {
                attendeeIds = attendeeIds ?? new List<int>();
                if (attendeeIds.IsNullOrEmpty())
                {
                    GovtEntities = (List<GovernmentEntity>)response.ResultData;
                }
                //GovtEntities = GovtEntities.Where(x => !attendeeIds.Contains(x.EntityId)).ToList();
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }

        protected async Task PopulateDepartments()
        {
            var response = await lookupService.GetDepartments();
            if (response.IsSuccessStatusCode)
            {
                Departments = (List<GEDepartments>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }

        protected async Task PopulateLegislationAttendees(List<string> attendeeIds = null)
        {
            var response = await lookupService.GetAttendeeUser();
            if (response.IsSuccessStatusCode)
            {
                attendeeIds = attendeeIds ?? new List<string>();
                var user = await BrowserStorage.GetItemAsync<UserDetailVM>("UserDetail");
                LegislationAttendees = (List<FatwaAttendeeVM>)response.ResultData;
                var existingAttendeeIds = new HashSet<string>(GetLegislationAttendees.Select(x => x.Id));
                LegislationAttendees = LegislationAttendees.Where(x => x.SectorTypeId == user.SectorTypeId ||
                                           x.Email == "hospublicoperational@fatwa.com" ||
                                           x.Email == "fatwapresidentoffice@fatwa.com")
                                           .Where(x => !attendeeIds.Contains(x.Id) && !existingAttendeeIds.Contains(x.Id))
                                           .OrderBy(x => x.FirstNameEnglish).ToList();

            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }

        protected async Task GetReferenceNumberBySubModuleId(int SubModuleId)
        {
            FilerefNumbers = new List<ReferenceNumberVM>();
            int sectorId = (int)loginState.UserDetail.SectorTypeId;
            var response = await lookupService.GetReferenceNumberBySubmoduleId(SubModuleId, sectorId);
            if (response.IsSuccessStatusCode)
            {
                FilerefNumbers = (List<ReferenceNumberVM>)response.ResultData;
                if (saveMeeting.Meeting.SubModulId == (int)SubModuleEnum.OrganizingCommittee && ReferenceId!=null)
                {
                    FileNumber = FilerefNumbers.Where(x => x.ReferenceId == Guid.Parse(ReferenceId)).Select(x => x.ReferenceNumber).FirstOrDefault();

                }
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        protected async Task PopulateSubModuleData()
        {
            var response = await lookupService.GetSubmodule();
            if (response.IsSuccessStatusCode)
            {
                SubModules = (List<SubModule>)response.ResultData;
                if (IsConsultationUser)
                {
                    SubModules = SubModules.Where(x => x.Id == (int)SubModuleEnum.ConsultationFile || x.Id==(int)SubModuleEnum.OrganizingCommittee).ToList();
                }
                else
                {
                    SubModules = SubModules.Where(x => x.Id == (int)SubModuleEnum.CaseFile
                    || x.Id == (int)SubModuleEnum.RegisteredCase || x.Id == (int)SubModuleEnum.OrganizingCommittee).ToList();
                }
            }



            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }

        protected async Task GetCommitteeMembersByCommitteeId(Guid ReferenceId, List<string> attendeeIds = null)
        {
            try
            {
                var response = await userService.GetCommitteeMembersByReferenceId(ReferenceId);
                if (response.IsSuccessStatusCode)
                {
                    LegislationAttendees = (List<FatwaAttendeeVM>)response.ResultData;
                    attendeeIds = attendeeIds ?? new List<string>();
                    var user = await BrowserStorage.GetItemAsync<UserDetailVM>("UserDetail");
                    var existingAttendeeIds = new HashSet<string>(GetLegislationAttendees.Select(x => x.Id));
                    LegislationAttendees = LegislationAttendees.Where(x => !attendeeIds.Contains(x.Id) && !existingAttendeeIds.Contains(x.Id))
                                          .OrderBy(x => x.FirstNameEnglish).ToList();
                    //if (ddlMeetingAttendees != null) { ddlMeetingAttendees.Reset(); }
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
                    Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
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

        #region Legislation GRID

        protected async Task AddLegislationAttendee()
        {
            if (LegislationAttendee.Id != null)
            {
                var newAttendee = LegislationAttendees.FirstOrDefault(x => x.Id == LegislationAttendee.Id);
                if (newAttendee is not null)
                {
                    newAttendee.AttendeeStatusId = (int)MeetingAttendeeStatusEnum.New;
                    newAttendee.AttendeeStatusEn = attendeeStatus.Where(x => x.Id == (int)MeetingAttendeeStatusEnum.New).Select(y => y.NameEn).FirstOrDefault();
                    newAttendee.AttendeeStatusAr = attendeeStatus.Where(x => x.Id == (int)MeetingAttendeeStatusEnum.New).Select(y => y.NameAr).FirstOrDefault();
                    newAttendee.SerialNo = ++atttendeeLegislationSerialNo;
                    GetLegislationAttendees.Add(newAttendee);
                    AllApproved = GetLegislationAttendees.ToList().All(x => x.AttendeeStatusId != (int)MeetingAttendeeStatusEnum.NotResponded && x.AttendeeStatusId != (int)MeetingAttendeeStatusEnum.New && x.AttendeeStatusId != (int)MeetingAttendeeStatusEnum.Pending);
                    if (saveMeeting.Meeting.SubModulId == (int)SubModuleEnum.OrganizingCommittee)
                    {
                        await GetCommitteeMembersByCommitteeId((Guid)saveMeeting.Meeting.ReferenceGuid, new List<string>());
                    }
                    else
                    {
                        await PopulateLegislationAttendees(new List<string> { newAttendee.Id });
                    }
                    // remove selected attendee from attendee dropdown
                    var resultAttendee = LegislationAttendees.Where(x => x.Id == newAttendee.Id).FirstOrDefault();
                    if (resultAttendee != null)
                    {
                        LegislationAttendees.Remove(resultAttendee);
                    }
                    if (saveMeeting.Meeting.MeetingStatusId == (int)MeetingStatusEnum.SaveAsDraft)
                    {
                        saveMeeting.DeletedLegislationAttendeeIds.Remove(Guid.Parse(newAttendee.Id));
                    }
                }
                LegislationAttendeeGrid.Reset();
                ddlLegislationAttendees.Reset();
                await LegislationAttendeeGrid.Reload();
            }
            Reload();
        }

        protected async Task DeleteLegislationAttendee(FatwaAttendeeVM attendee)
        {
            saveMeeting.DeletedLegislationAttendeeIds.Add(Guid.Parse(attendee.Id));
            atttendeeLegislationSerialNo = atttendeeLegislationSerialNo - 1;

            GetLegislationAttendees.Remove(attendee);
            // for rest Grid Serial Number
            int count = GetLegislationAttendees.Count;
            for (int i = 0; i < count; i++)
            {
                GetLegislationAttendees[i].SerialNo = i + 1;
            }
            //end
            if (saveMeeting.Meeting.SubModulId == (int)SubModuleEnum.OrganizingCommittee)
            {
                await GetCommitteeMembersByCommitteeId((Guid)saveMeeting.Meeting.ReferenceGuid, new List<string>());
            }
            else
            {
                await PopulateLegislationAttendees();
            }
            LegislationAttendeeGrid.Reset();
            AllApproved = GetLegislationAttendees.ToList().All(x => x.AttendeeStatusId != (int)MeetingAttendeeStatusEnum.NotResponded && x.AttendeeStatusId != (int)MeetingAttendeeStatusEnum.New && x.AttendeeStatusId != (int)MeetingAttendeeStatusEnum.Pending);

            await LegislationAttendeeGrid.Reload();
        }

        #endregion

        #region GE GRID

        protected async Task AddGeAttendee()
        {
            if (GeAttendee.GovernmentEntityId != 0 && GeAttendee.DepartmentId != 0 && GeAttendee.RepresentativeId != null)
            {
                GeAttendeeGrid.Reset();

                MeetingAttendeeVM newAttendee = new MeetingAttendeeVM();

                var govt = GovtEntities.FirstOrDefault(x => x.EntityId == GeAttendee.GovernmentEntityId);
                if (govt is not null)
                {
                    newAttendee.GovernmentEntityId = govt.EntityId;
                    newAttendee.GovernmentEntityNameEn = govt.Name_En;
                    newAttendee.GovernmentEntityNameAr = govt.Name_Ar;
                }

                var dept = Departments.FirstOrDefault(x => x.Id == GeAttendee.DepartmentId);
                if (dept is not null)
                {
                    newAttendee.DepartmentId = dept.Id;
                    newAttendee.DepartmentNameEn = dept.Name_En;
                    newAttendee.DepartmentNameAr = dept.Name_Ar;
                }



                var rep = GeRepresentatives.FirstOrDefault(x => x.Id == GeAttendee.RepresentativeId);
                if (rep is not null)
                {
                    newAttendee.RepresentativeId = rep.Id;
                    newAttendee.RepresentativeNameEn = rep.NameEn;
                    newAttendee.RepresentativeNameAr = rep.NameAr;
                }
                newAttendee.SerialNo = ++atttendeeGeSerialNo;




                GetGeAttendees.Add(newAttendee);
                await PopulateGovtEntities(new List<int> { (int)newAttendee.GovernmentEntityId });
                GeAttendee.RepresentativeNameEn = null;
                GeAttendee.RepresentativeNameAr = null;

                ddlGovtEntities.Reset();
                ddlDepartments.Reset();

                await GeAttendeeGrid.Reload();
            }
            Reload();
        }

        protected async Task DeleteGeAttendee(MeetingAttendeeVM attendee)
        {
            saveMeeting.DeletedGeAttendeeIds.Add((Guid)attendee.MeetingAttendeeId);
            atttendeeGeSerialNo = atttendeeGeSerialNo--;

            GetGeAttendees.Remove(attendee);
            await PopulateGovtEntities();
            GeAttendeeGrid.Reset();
            await GeAttendeeGrid.Reload();
            Reload();
        }

        #endregion

        #region Task Status update
        protected async Task TaskStatusUpdate(Guid MeetingId)
        {
            if (CommunicationId != null)
            {
                //when the Meeting is done, it ll navigatge to meeting-view page
                fileTask.TaskStatusId = (int)TaskStatusEnum.Done;

                if (!string.IsNullOrEmpty(fileTask.Url))
                {
                    fileTask.Url = fileTask.Url.StartsWith("meeting-add") ? $"meeting-view/{MeetingId}/true" : fileTask.Url;
                }
                var taskResponse = await taskService.DecisionTask(fileTask);
                if (!taskResponse.IsSuccessStatusCode)
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(taskResponse);
                }
            }
        }
        protected async Task UpdateTaskDetail()
        {
            taskDetailVM.TaskStatusId = (int)TaskStatusEnum.Done;
            var taskResponse = await taskService.DecisionTask(taskDetailVM);
            if (!taskResponse.IsSuccessStatusCode)
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(taskResponse);
            }
        }
        #endregion

        protected async Task SaveAsDraft()
        {
            isSaveAsDraft = true;

            await FormSubmit(saveMeeting);
            isSaveAsDraft = false;
        }

        protected async Task SendToHOS()
        {
            IsSendToHOS = true;
            await FormSubmit(saveMeeting);
        }
        protected async Task CreateMeeting()
        {
            saveMeeting.IsCreateMeeting = true;

            await FormSubmit(saveMeeting);
        }

        protected async Task InvalidFormSubmit()
        {
            await JsInterop.InvokeVoidAsync("ScrollPageToTop");
        }
        protected async Task FormSubmit(SaveMeetingVM saveMeeting)
        {
            try
            {
                DateTime startTi = saveMeeting.Meeting.StartTime;
                TimeSpan time = startTi.TimeOfDay;
                saveMeeting.Meeting.StartTime = saveMeeting.Meeting.Date + time;
                DateTime endTimeTi = saveMeeting.Meeting.EndTime;
                TimeSpan end = endTimeTi.TimeOfDay;
                saveMeeting.Meeting.EndTime = saveMeeting.Meeting.Date + end;
                bool? dialogResponse = false;
                string dialogMessage = string.Empty;
                dialogMessage = translationState.Translate("Sure_Submit");
                if (isSaveAsDraft)
                {
                    saveMeeting.isSaveAsDraft = true;
                    dialogMessage = translationState.Translate("Sure_SaveAsDarft");
                }
                if (IsSendToHOS)
                {
                    saveMeeting.Meeting.IsSendToHOS = true;
                    dialogMessage = translationState.Translate("Sure_Send_For_Approval");
                }
                if ((bool)saveMeeting.IsCreateMeeting)
                {
                    if ((loginState.UserRoles.Any(r => SystemRoles.ViceHOS.Contains(r.RoleId))) && (saveMeeting.OnlyViceHosApproval != true))
                    {
                        saveMeeting.Meeting.IsApproved = false;
                        saveMeeting.Meeting.MeetingStatusId = (int)MeetingStatusEnum.ApprovedByViceHos;
                    }
                    else
                    {
                        saveMeeting.Meeting.IsApproved = true;
                    }
                    if (saveMeeting.Meeting.MeetingTypeId == (int)MeetingTypeEnum.Internal)
                    {
                        if ((saveMeeting.OnlyViceHosApproval == true) || (loginState.UserRoles.Any(r => SystemRoles.HOS.Contains(r.RoleId))) || (loginState.UserRoles.Any(r => SystemRoles.ComsHOS.Contains(r.RoleId))) || saveMeeting.Meeting.SubModulId == (int)SubModuleEnum.OrganizingCommittee)

                        {
                            saveMeeting.Meeting.MeetingStatusId = (int)MeetingStatusEnum.Scheduled;
                        }
                    }
                    else
                    {
                        saveMeeting.Meeting.MeetingStatusId = (int)MeetingStatusEnum.ApprovedByHOS;
                    }
                    dialogMessage = translationState.Translate("Sure_Create_Meeting");
                }
                string successMessage = translationState.Translate("SaveMeeting_Success");

                saveMeeting.AdditionalTempFiles = dataCommunicationService.saveMeetingVM.AdditionalTempFiles;
                saveMeeting.MandatoryTempFiles = dataCommunicationService.saveMeetingVM.MandatoryTempFiles;
                //saveMeeting.GEAttandeeSelected = dataCommunicationService.saveMeetingVM.GEAttandeeSelected;

                if (saveMeeting.Meeting.MeetingTypeId == (int)MeetingTypeEnum.External || saveMeeting.Meeting.MeetingTypeId == (int)MeetingTypeEnum.Internal)
                {
                    //Legislation Attendee 
                    //Check if the Atleast one Attendee is added
                    if (GetLegislationAttendees.Count == 0 && !saveMeeting.isSaveAsDraft)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Error,
                            Detail = translationState.Translate("Please_Select_Atleast_One_Attendee"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        return;
                    }
                    else
                    {
                        if (saveMeeting.LegislationAttendee.Count() != 0)
                        {
                            saveMeeting.LegislationAttendee = new List<FatwaAttendeeVM>();
                        }
                        saveMeeting.LegislationAttendee = GetLegislationAttendees;
                    }
                }
                if (saveMeeting.Meeting.MeetingTypeId == (int)MeetingTypeEnum.External)
                {
                    //Ge Attendee 
                    // only Add GE User in Case Files in Consultation Files We Dont Add GE Attendee 
                    //Check if the Atleast one Attendee is added
                    if (GetGeAttendees.Count == 0 && !saveMeeting.isSaveAsDraft)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Error,
                            Detail = translationState.Translate("Please_Select_Atleast_One_Attendee"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        return;
                    }
                    else
                    {
                        saveMeeting.GeAttendee = GetGeAttendees;
                    }

                    //if ((saveMeeting.MandatoryTempFiles.Count == 0) && !saveMeeting.isSaveAsDraft)
                    //{
                    //    notificationService.Notify(new NotificationMessage()
                    //    {
                    //        Severity = NotificationSeverity.Error,
                    //        Detail = translationState.Translate("Must_Attach_Mandatory_Documents"),
                    //        Style = "position: fixed !important; left: 0; margin: auto; "
                    //    });
                    //    return;
                    //}
                }

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
                    spinnerService.Show();
                    if (saveMeeting.Meeting.IsApproved)
                    {
                        saveMeeting.LoggedInUser = loginState.UserDetail.UserId;
                    }
                    else if (!isSaveAsDraft && (bool)saveMeeting.IsCreateMeeting != true)

                    {
                        saveMeeting.Meeting.IsApproved = false;
                        saveMeeting.Meeting.MeetingStatusId = (int)MeetingStatusEnum.OnHold;
                        saveMeeting.Meeting.GovtEntityId = Convert.ToInt32(govtEntityId);
                    }
                    //}

                    saveMeeting.Meeting.SectorTypeId = loginState.UserDetail.SectorTypeId;
                    saveMeeting.Meeting.SentBy = loginState.Username;
                    saveMeeting.Meeting.ReceivedBy = NewRecievedBy;
                    ApiCallResponse response = null;
                    if (!isEdit && saveMeeting.Meeting.MeetingStatusId != (int)MeetingStatusEnum.SaveAsDraft)
                    {

                        var userIdToCheck = loginState.UserDetail.UserId;

                        if (saveMeeting.LegislationAttendee.Any(attendee => attendee.Id == userIdToCheck))
                        {
                            var attendeeToUpdate = saveMeeting.LegislationAttendee.First(attendee => attendee.Id == userIdToCheck);
                            attendeeToUpdate.AttendeeStatusId = (int)MeetingAttendeeStatusEnum.Accept;
                        }

                        response = await meetingService.AddMeeting(saveMeeting);
                        await SaveTempAttachementToUploadedDocument();
                        //await CopyAttachmentsFromSourceToDestination(saveMeeting);

                        //await SaveTempAttachementToUploadedDocument();
                        if (response.IsSuccessStatusCode)
                        {
                            await TaskStatusUpdate(saveMeeting.Meeting.MeetingId);
                            if (TaskId != null && taskDetailVM.TaskStatusId != (int)TaskStatusEnum.Done)
                                await UpdateTaskDetail();
                        }
                    }
                    else
                    {
                        var userIdToCheck = loginState.UserDetail.UserId;

                        if (saveMeeting.LegislationAttendee.Any(attendee => attendee.Id == userIdToCheck))
                        {
                            var attendeeToUpdate = saveMeeting.LegislationAttendee.First(attendee => attendee.Id == userIdToCheck);
                            attendeeToUpdate.AttendeeStatusId = (int)MeetingAttendeeStatusEnum.Accept;
                        }
                        response = await meetingService.EditMeeting(saveMeeting);
                        await SaveTempAttachementToUploadedDocument();
                    }
                    if (response.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = successMessage,
                            Style = "position: fixed !important; left: 0; margin: auto;"
                        });
                        if ((bool)saveMeeting.IsCreateMeeting)
                        {
                            var taskResponse = await taskService.ApproveTaskByReferenceId(Guid.Parse(MeetingId), IsVice);
                            if (!taskResponse.IsSuccessStatusCode)
                            {
                                await invalidRequestHandlerService.ReturnBadRequestNotification(taskResponse);
                            }

                        }
                        if ((bool)saveMeeting.IsCreateMeeting)
                        {
                            navigationManager.NavigateTo("/meeting-list");
                        }
                        else
                        {
                            await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
                        }
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }
                    spinnerService.Hide();
                }
                else
                {
                    saveMeeting.isSaveAsDraft = false;
                }
            }
            catch (Exception)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }

        protected async Task BtnCancel(MouseEventArgs args)
        {
            if (await dialogService.Confirm(translationState.Translate("Sure_Cancel"), translationState.Translate("Confirm"), new ConfirmOptions()
            {
                OkButtonText = translationState.Translate("OK"),
                CancelButtonText = translationState.Translate("Cancel")
            }) == true)
            {
                await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
            }
        }

        protected void OnStartTimeChange(object theUserInput)
        {
            saveMeeting.Meeting.EndTime = saveMeeting.Meeting.StartTime.AddMinutes(5);
        }
        protected void OnEndTimeChange(object theUserInput)
        {
            if(saveMeeting.Meeting.EndTime <= saveMeeting.Meeting.StartTime)
            {
                saveMeeting.Meeting.EndTime = saveMeeting.Meeting.StartTime.AddMinutes(5);
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Start_Time_Should_Less_Than_End_Time"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
        protected void OnChangeDate()
        {
            if (saveMeeting.Meeting.Date > DateTime.Now.Date)
            {
                saveMeeting.Meeting.StartTime = DateTime.Now.Date.AddHours(09);
                saveMeeting.Meeting.EndTime = saveMeeting.Meeting.StartTime.AddHours(01);
            }
            else if (saveMeeting.Meeting.Date == DateTime.Now.Date)
            {
                saveMeeting.Meeting.StartTime = DateTime.Now.AddMinutes(60);
                saveMeeting.Meeting.EndTime = saveMeeting.Meeting.StartTime.AddHours(01);


            }
            Reload();
        }
        #endregion

        #region on Change Event
        protected async Task OnGEChange(object args)
        {
            try
            {
                if (args != null)
                {
                    var result = await meetingService.GetDepartmentsByGeId((int)args);
                    if (result.IsSuccessStatusCode)
                    {
                        Departments = (List<GEDepartments>)result.ResultData;
                    }
                    PopulateGeRepresentatives(GeAttendee.GovernmentEntityId);

                }
            }
            catch (Exception ex)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                    Summary = translationState.Translate("Error"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
        protected async Task PopulateGeRepresentatives(int GeId)
        {

            var govtEntityResponse = await lookupService.GetGeRepresentatives(GeId);
            if (govtEntityResponse.IsSuccessStatusCode)
            {
                GeRepresentatives = (List<GovernmentEntityRepresentative>)govtEntityResponse.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(govtEntityResponse);
            }
            StateHasChanged();
        }
        void OnChange(bool value)
        {
            if (value)
            {
                isOnline = true;
            }
            else
            {
                isOnline = false;
            }
            Console.WriteLine($"{value}");
        }

        public async Task GetGeAttendeeByReferenceId(Guid ReferenceId)
        {
            try
            {
                if (ReferenceId != null)
                {
                    var result = await meetingService.GetGeAttendeeByReferenceId(ReferenceId);
                    if (result.IsSuccessStatusCode)
                    {
                        GetGeAttendees = (List<MeetingAttendeeVM>)result.ResultData;
                        List<MeetingAttendeeVM> Obj = new List<MeetingAttendeeVM>();
                        foreach (var item in GetGeAttendees)
                        {
                            item.IsGEUser = true;
                            Obj.Add(item);
                        }
                        GetGeAttendees = Obj;
                        // GeAttendee = (List<MeetingAttendeeVM>)result.ResultData;
                        Ischange = true;
                        if (GeAttendeeGrid.Count > 0)
                        {
                            await GeAttendeeGrid.Reload();

                        }
                        StateHasChanged();
                    }
                    //PopulateGeRepresentatives(GeAttendee.GovernmentEntityId);

                }
            }
            catch (Exception ex)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                    Summary = translationState.Translate("Error"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }

        }
        //public async Task GetGEAttendeeDetails()
        //{
        //    try
        //    {
        //        var result = await meetingService.GetGEAttendeeDetails(new MeetingAttendeeVM());
        //        if (result.IsSuccessStatusCode)
        //        {
        //            GeAttendee = (MeetingAttendeeVM)result.ResultData;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        notificationService.Notify(new NotificationMessage()
        //        {
        //            Severity = NotificationSeverity.Error,
        //            Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
        //            Summary = translationState.Translate("Error"),
        //            Style = "position: fixed !important; left: 0; margin: auto; "
        //        });
        //    }

        //}


        #endregion

        #region Redirect Function
        private void GoBackDashboard()
        {
            navigationManager.NavigateTo("/dashboard");
        }
        private void GoBackHomeScreen()
        {
            navigationManager.NavigateTo("/index");
        }

        private async Task PreviousPageUrl()
        {
            await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
        }
        protected async Task RedirectBack()
        {
            await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
        }
        #endregion
        protected async Task OnSubModuleChange(object args)
        {
            try
            {
                if (args != null)
                {

                    if (saveMeeting.Meeting.SubModulId == (int)SubModuleEnum.OrganizingCommittee)
                    {
                        GetLegislationAttendees.Clear();
                        LegislationAttendees = null;
                        saveMeeting.Meeting.MeetingTypeId = (int)MeetingTypeEnum.Internal;
                        await GetReferenceNumberBySubModuleId(saveMeeting.Meeting.SubModulId);
                        await LegislationAttendeeGrid.Reload();
                        StateHasChanged();
                    }
                    else
                    {
                        GetLegislationAttendees.Clear();
                        saveMeeting.Meeting.MeetingTypeId = -1;
                        await PopulateLegislationAttendees();
                        await GetReferenceNumberBySubModuleId(saveMeeting.Meeting.SubModulId);
                        StateHasChanged();
                    }
                    Ischange = false;

                }
            }
            catch (Exception ex)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                    Summary = translationState.Translate("Error"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
        async void OnChangeTab(int index)
        {
            translationState.TranslateGridFilterLabels(LegislationAttendeeGrid);
            translationState.TranslateGridFilterLabels(GeAttendeeGrid);
            if (index == 2)
            {
                if (saveMeeting.Meeting.MeetingStatusId == 0 && (dataCommunicationService.saveMeetingVM.MandatoryTempFiles.Count == 0 || dataCommunicationService.saveMeetingVM.MandatoryTempFiles.Select(x => x.FileName).FirstOrDefault() == null))
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("RequiredDocument"), 
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    await steps.PrevStep();
                }
                if (saveMeeting.Meeting.MeetingStatusId == 512 && saveMeeting.DeletedAttachementIds.Count > 0 || dataCommunicationService.saveMeetingVM.MandatoryTempFiles.Count == 0)
                {
                    TempAttachementVM tempAttachement = new TempAttachementVM();
                    var getAttachments = await fileUploadService.GetUploadedAttachements(false, 0, saveMeeting.Meeting.MeetingId);
                    if (getAttachments.Count > 0)
                    {
                        foreach (int attachmentId in saveMeeting.DeletedAttachementIds)
                        {
                            tempAttachement = getAttachments.Where(x => x.AttachmentTypeId == 73 && x.UploadedDocumentId == attachmentId).FirstOrDefault();
                            if (tempAttachement != null)
                            {
                                break;
                            }
                        }
                    }
                    if (tempAttachement != null || dataCommunicationService.saveMeetingVM.MandatoryTempFiles.Count == 0)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Error,
                            Detail = translationState.Translate("RequiredDocument"), 
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        await steps.PrevStep();
                    }
                }
            }
        }

        protected async Task CopyAttachmentsFromSourceToDestination(SaveMeetingVM item)
        {
            try
            {
                var docResponse = await fileUploadService.CopyAttachmentsFromSourceToDestination(new List<CopyAttachmentVM>
                {
                    new CopyAttachmentVM()
                   {
                       SourceId = item.Meeting.MeetingId,
                       DestinationId = (Guid)item.Meeting.ReferenceGuid,
                       CreatedBy = item.Meeting.CreatedBy
                   }
                });
                if (!docResponse.IsSuccessStatusCode)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Attachment_Save_Failed"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    return;
                }
            }
            catch (Exception)
            {
                return;
                throw;
            }
        }
        protected async Task SaveTempAttachementToUploadedDocument()
        {
            try
            {
                List<Guid> requestIds = new List<Guid>
                {

                    saveMeeting.Meeting.MeetingId

            };

                var docResponse = await fileUploadService.SaveTempAttachementToUploadedDocument(new FileUploadVM()
                {
                    RequestIds = requestIds,
                    CreatedBy = saveMeeting.Meeting.CreatedBy,
                    FilePath = _config.GetValue<string>("dms_file_path"),
                    DeletedAttachementIds = saveMeeting.DeletedAttachementIds
                });

                if (!docResponse.IsSuccessStatusCode)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Attachment_Save_Failed"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    return;
                }
            }
            catch (Exception)
            {
                return;
                throw;
            }
        }
        protected async Task PopulateMeetingAttendeeStatus()
        {
            var response = await lookupService.GetAttendeeMeetingStatus();
            if (response.IsSuccessStatusCode)
            {
                attendeeStatus = (List<MeetingAttendeeStatus>)response.ResultData;

            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
    }
}
