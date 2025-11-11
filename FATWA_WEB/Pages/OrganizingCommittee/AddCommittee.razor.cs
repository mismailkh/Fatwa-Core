using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.OrganizingCommittee;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using FATWA_DOMAIN.Models.ViewModel.DmsVMs;
using FATWA_DOMAIN.Models.ViewModel.GeneralVMs;
using FATWA_DOMAIN.Models.ViewModel.OrganizingCommitteeVMs;
using FATWA_DOMAIN.Models.ViewModel.TaskVMs;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using Syncfusion.Blazor.RichTextEditor;
using System.Collections.ObjectModel;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using static FATWA_DOMAIN.Enums.OrganizingCommittee.OrganizingCommitteeEnum;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_WEB.Pages.OrganizingCommittee
{
    public partial class AddCommittee : ComponentBase
    {
        #region Inject
        [Inject]
        public ProcessLogService processLogService { get; set; }
        [Inject]
        public ErrorLogService errorLogService { get; set; }
        #endregion

        #region Parameter
        [Parameter]
        public dynamic CommitteeId { get; set; }
        #endregion

        #region Variable Declaration
        public SaveCommitteeVM saveCommittee { get; set; } = new SaveCommitteeVM();
        public string requestDateValidationMsg { get; set; } = " ";
        public string descriptionValidationMessage { get; set; } = " ";
        public string taskValidationMessage { get; set; } = " ";
        public string roleValidationMessage { get; set; } = " ";
        public string memberValidationMessage { get; set; } = " ";
        public string userValidationMessage { get; set; } = " ";
        public string loginroleValidationMessage { get; set; } = " ";
        public DateTime Minimum = new DateTime(2014, 1, 1);
        public IEnumerable<CommitteeUserDataVM> usersData { get; set; } = new List<CommitteeUserDataVM>();
        public IEnumerable<CommitteeRoles> committeeRoles { get; set; } = new List<CommitteeRoles>();
        public List<OperatingSectorType> operatingSectorTypes { get; set; } = new List<OperatingSectorType>();
        public CommitteeMembersVMs committeeMembersVm { get; set; } = new CommitteeMembersVMs();
        public TempCommitteeTaskVm committeeTasksVm { get; set; } = new TempCommitteeTaskVm();
        public List<SaveTaskVM> addedTaskList { get; set; } = new List<SaveTaskVM>();
        public List<string> ReceiverIds { get; set; } = new List<string>();
        public List<FATWA_DOMAIN.Models.Notifications.Notification> sendNotifications { get; set; } = new List<FATWA_DOMAIN.Models.Notifications.Notification>();
        public RadzenDataGrid<TempCommitteeTaskVm> committeeTaskGrid { get; set; } = new RadzenDataGrid<TempCommitteeTaskVm>();
        public RadzenDataGrid<CommitteeMembersVMs> committeeMembersGrid { get; set; } = new RadzenDataGrid<CommitteeMembersVMs>();
        public RadzenDataGrid<CommitteeUserDataVM> usersDataGrid { get; set; } = new RadzenDataGrid<CommitteeUserDataVM>();
        public RadzenDropDown<string?> addedMemberDropDown { get; set; } = new RadzenDropDown<string?>();
        public List<TempCommitteeTaskVm> tempCommitteeTasksList = new List<TempCommitteeTaskVm>();
        public List<CommitteeMembersVMs> CommitteeMembers { get; set; } = new List<CommitteeMembersVMs>();
        public int SectorTypeId { get; set; }
        protected bool Keywords = false;
        public int isSelectedRole { get; set; }
        public TempAttachementVM uploadedAttachment;
        public ObservableCollection<TempAttachementVM> committeeAttachment { get; set; } = new ObservableCollection<TempAttachementVM>();
        public ObservableCollection<TempAttachementVM> committeeAttachment2 { get; set; } = new ObservableCollection<TempAttachementVM>();
        #endregion

        #region SyncFusion Rich Text Editor
        private List<ToolbarItemModel> Tools = new List<ToolbarItemModel>()
        {
        new ToolbarItemModel() { Command = ToolbarCommand.Formats },
        new ToolbarItemModel() { Command = ToolbarCommand.Separator },
        new ToolbarItemModel() { Command = ToolbarCommand.Bold },
        new ToolbarItemModel() { Command = ToolbarCommand.Italic },
        new ToolbarItemModel() { Command = ToolbarCommand.Underline },
        new ToolbarItemModel() { Command = ToolbarCommand.Separator },
        new ToolbarItemModel() { Command = ToolbarCommand.FontColor },
        new ToolbarItemModel() { Command = ToolbarCommand.Separator },
        new ToolbarItemModel() { Command = ToolbarCommand.NumberFormatList },
        new ToolbarItemModel() { Command = ToolbarCommand.BulletFormatList },
        new ToolbarItemModel() { Command = ToolbarCommand.Separator },
        new ToolbarItemModel() { Command = ToolbarCommand.CreateLink },
        new ToolbarItemModel() { Command = ToolbarCommand.Image },
        new ToolbarItemModel() { Command = ToolbarCommand.CreateTable },
        new ToolbarItemModel() { Command = ToolbarCommand.InsertCode },
        new ToolbarItemModel() { Command = ToolbarCommand.SourceCode },
        new ToolbarItemModel() { Command = ToolbarCommand.Separator },
        new ToolbarItemModel() { Command = ToolbarCommand.Undo },
        new ToolbarItemModel() { Command = ToolbarCommand.Redo }
        };
        private List<TableToolbarItemModel> TableQuickToolbarItems = new List<TableToolbarItemModel>()
        {
        new TableToolbarItemModel() { Command = TableToolbarCommand.TableHeader },
        new TableToolbarItemModel() { Command = TableToolbarCommand.TableRows },
        new TableToolbarItemModel() { Command = TableToolbarCommand.TableColumns },
        new TableToolbarItemModel() { Command = TableToolbarCommand.TableCell },
        new TableToolbarItemModel() { Command = TableToolbarCommand.HorizontalSeparator },
        new TableToolbarItemModel() { Command = TableToolbarCommand.TableRemove },
        new TableToolbarItemModel() { Command = TableToolbarCommand.BackgroundColor },
        new TableToolbarItemModel() { Command = TableToolbarCommand.TableCellVerticalAlign },
        new TableToolbarItemModel() { Command = TableToolbarCommand.Styles }
        };
        private List<ImageToolbarItemModel> ImageTools = new List<ImageToolbarItemModel>()
        {
        new ImageToolbarItemModel() { Command = ImageToolbarCommand.Replace },
        new ImageToolbarItemModel() { Command = ImageToolbarCommand.Align },
        new ImageToolbarItemModel() { Command = ImageToolbarCommand.Caption },
        new ImageToolbarItemModel() { Command = ImageToolbarCommand.Remove },
        new ImageToolbarItemModel() { Command = ImageToolbarCommand.HorizontalSeparator },
        new ImageToolbarItemModel() { Command = ImageToolbarCommand.InsertLink },
        new ImageToolbarItemModel() { Command = ImageToolbarCommand.Display },
        new ImageToolbarItemModel() { Command = ImageToolbarCommand.AltText },
        new ImageToolbarItemModel() { Command = ImageToolbarCommand.Dimension }
        };
        private List<string> allowedTypes = new List<string> { ".png", ".jpg", ".jpeg" };
        #endregion

        #region Componont Load
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await Load();
            spinnerService.Hide();
        }
        protected async Task Load()
        {
            await PopulateCommitteeRoles();
            await PopulateSectorTypesByDepartmentId();

            if (CommitteeId != null)
            {
                await GetCommitteeById();
                await GetCommitteeMemeberByCommitteeId();
                await GetTempCommitteeTasks();
            }
            else
            {
                saveCommittee.committee.Id = Guid.NewGuid();
                saveCommittee.committee.CommencementDate = DateTime.Now;
                saveCommittee.committee.ExpectedEndDate = DateTime.Now;
                saveCommittee.committee.CircularIssueDate = DateTime.Now;
                await PopulateCommitteeNumber();
            }

        }
        #endregion

        #region On Change Sector Type
        protected async Task OnChangeSectorType()
        {
            try
            {
                if (SectorTypeId > 0)
                {
                    await GetUsersBySectorTypeId(SectorTypeId);
                }
                else
                {
                    usersData = new List<CommitteeUserDataVM>();
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

        #region Add Creator in member list
        protected async Task AddCreatorInMemberList(int roleId)
        {
            try
            {
                CommitteeUserDataVM loginUser = new CommitteeUserDataVM();
                var response = await userService.GetUsersBySectorTypeId((int)loginState.UserDetail.SectorTypeId);
                if (response.IsSuccessStatusCode)
                {
                    var Users = (IEnumerable<CommitteeUserDataVM>)response.ResultData;
                    loginUser = Users.Where(x => x.UserId == loginState.UserDetail.UserId).FirstOrDefault();
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }

                var existingMember = saveCommittee.committeeMembers.Where(x => x.MemberId == loginState.UserDetail.UserId).FirstOrDefault();
                if (saveCommittee.committeeMembers.Exists(x => x.CommitteeRoleId == (int)CommitteeRoleEnum.CommitteeHead) && roleId == (int)CommitteeRoleEnum.CommitteeHead)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Commiittee_Head_Already_Exist"),
                        Style = "position: fixed !important; left: 0; margin: auto; ",
                        Duration = 1000
                    });

                    isSelectedRole = existingMember?.CommitteeRoleId == (int)CommitteeRoleEnum.Organizer ? (int)CommitteeRoleEnum.Organizer : 0;

                }
                else if (existingMember != null)
                {
                    existingMember.RoleNameEn = committeeRoles.Where(x => x.Id == roleId).Select(x => x.NameEn).FirstOrDefault();
                    existingMember.RoleNameAr = committeeRoles.Where(x => x.Id == roleId).Select(x => x.NameAr).FirstOrDefault();
                    existingMember.CommitteeRoleId = roleId;
                    isSelectedRole = roleId;
                }
                else
                {
                    CommitteeMembersVMs loginMemberVm = new CommitteeMembersVMs
                    {
                        MemeberNameEn = loginState.UserDetail.FullNameEn,
                        MemeberNameAr = loginState.UserDetail.FullNameAr,
                        RoleNameEn = committeeRoles.Where(x => x.Id == roleId).Select(x => x.NameEn).FirstOrDefault(),
                        RoleNameAr = committeeRoles.Where(x => x.Id == roleId).Select(x => x.NameAr).FirstOrDefault(),
                        CommitteeRoleId = roleId,
                        MemberId = loginState.UserDetail.UserId,
                        CommitteeId = saveCommittee.committee.Id,
                        IsReadOnly = false,
                        SectorTypeEn = loginUser?.SectorTypeEn,
                        SectorTypeAr = loginUser?.SectorTypeAr,
                        TotalCommittee = loginUser?.TotalCommittee,
                        TotalTasks = loginUser?.TotalTasks,
                        LastActivityDate = loginUser?.LastActivityDate
                    };
                    saveCommittee.committeeMembers.Add(loginMemberVm);
                    CommitteeMembers.Add(loginMemberVm);
                }

                await committeeMembersGrid.Reload();
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

        #region Populate Data 
        protected async Task PopulateCommitteeNumber()
        {
            try
            {
                var response = await organizingCommitteeService.GetAutoGeneratedCommitteeNumber();
                if (response.IsSuccessStatusCode)
                {
                    var result = (Committee)response.ResultData;
                    saveCommittee.committee.CommitteeNumber = result.CommitteeNumber;
                    saveCommittee.committee.ShortNumber = result.ShortNumber;

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
        protected async Task GetUsersBySectorTypeId(int SectorTypeId)
        {
            try
            {
                var response = await userService.GetUsersBySectorTypeId(SectorTypeId);
                if (response.IsSuccessStatusCode)
                {
                    usersData = (IEnumerable<CommitteeUserDataVM>)response.ResultData;
                    usersData = usersData.Where(x => x.UserId != loginState.UserDetail.UserId).ToList();
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
        protected async Task PopulateCommitteeRoles()
        {
            try
            {
                var response = await lookupService.GetCommitteeRoles();
                if (response.IsSuccessStatusCode)
                {
                    committeeRoles = (IEnumerable<CommitteeRoles>)response.ResultData;
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
        protected async Task PopulateSectorTypesByDepartmentId()
        {
            try
            {
                var response = await lookupService.GetOperatingSectorsByDepartmentId(loginState.UserDetail.DepartmentId);
                if (response.IsSuccessStatusCode)
                {
                    operatingSectorTypes = (List<OperatingSectorType>)response.ResultData;
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
        protected async Task GetTempCommitteeTasks()
        {
            try
            {
                var response = await organizingCommitteeService.GetTempCommitteeTasks(Guid.Parse(CommitteeId));
                if (response.IsSuccessStatusCode)
                {
                    saveCommittee.committeeTasks = (List<TempCommitteeTaskVm>)response.ResultData;
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
        protected async Task GetCommitteeById()
        {
            try
            {
                var response = await organizingCommitteeService.GetCommitteeById(Guid.Parse(CommitteeId));
                if (response.IsSuccessStatusCode)
                {
                    saveCommittee.committee = (Committee)response.ResultData;
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
        protected async Task GetCommitteeMemeberByCommitteeId()
        {
            try
            {
                var response = await organizingCommitteeService.GetCommitteeMembers(Guid.Parse(CommitteeId));
                if (response.IsSuccessStatusCode)
                {
                    //CommitteeMembers = (List<CommitteeMembersVMs>)response.ResultData;
                    saveCommittee.committeeMembers = (List<CommitteeMembersVMs>)response.ResultData;
                    var loginuser = saveCommittee.committeeMembers.Where(x => x.MemberId == loginState.UserDetail.UserId).FirstOrDefault();
                    isSelectedRole = loginuser?.CommitteeRoleId ?? 0;
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

        #region Add Members
        protected async Task AddMember()
        {
            try
            {
                if (saveCommittee.committeeMembers.Exists(x => x.CommitteeRoleId == (int)CommitteeRoleEnum.CommitteeHead) && committeeMembersVm.CommitteeRoleId == (int)CommitteeRoleEnum.CommitteeHead)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Team_Leader_Already_Exist"),
                        Style = "position: fixed !important; left: 0; margin: auto; ",
                        Duration = 1000
                    });
                }
                else
                {
                    if (string.IsNullOrEmpty(committeeMembersVm.MemberId) || committeeMembersVm.CommitteeRoleId == 0)
                    {
                        userValidationMessage = string.IsNullOrEmpty(committeeMembersVm.MemberId) ? translationState.Translate("Required_Field") : "";
                        roleValidationMessage = committeeMembersVm.CommitteeRoleId == 0 ? translationState.Translate("Required_Field") : "";
                    }
                    else
                    {
                        committeeMembersVm.MemeberNameEn = usersData.Where(x => x.UserId == committeeMembersVm.MemberId).Select(x => x.FullName_En).FirstOrDefault();
                        committeeMembersVm.MemeberNameAr = usersData.Where(x => x.UserId == committeeMembersVm.MemberId).Select(x => x.FullName_Ar).FirstOrDefault();
                        committeeMembersVm.RoleNameEn = committeeRoles.Where(x => x.Id == committeeMembersVm.CommitteeRoleId).Select(x => x.NameEn).FirstOrDefault();
                        committeeMembersVm.RoleNameAr = committeeRoles.Where(x => x.Id == committeeMembersVm.CommitteeRoleId).Select(x => x.NameAr).FirstOrDefault();
                        committeeMembersVm.CommitteeId = saveCommittee.committee.Id;
                        committeeMembersVm.IsReadOnly = false;
                        committeeMembersVm.SectorTypeEn = usersData.Where(x => x.UserId == committeeMembersVm.MemberId).Select(x => x.SectorTypeEn).FirstOrDefault();
                        committeeMembersVm.SectorTypeAr = usersData.Where(x => x.UserId == committeeMembersVm.MemberId).Select(x => x.SectorTypeAr).FirstOrDefault();
                        committeeMembersVm.TotalCommittee = usersData.Where(x => x.UserId == committeeMembersVm.MemberId).Select(x => x.TotalCommittee).FirstOrDefault();
                        committeeMembersVm.TotalTasks = usersData.Where(x => x.UserId == committeeMembersVm.MemberId).Select(x => x.TotalTasks).FirstOrDefault();
                        committeeMembersVm.LastActivityDate = usersData.Where(x => x.UserId == committeeMembersVm.MemberId).Select(x => x.LastActivityDate).FirstOrDefault();
                        if (committeeMembersVm.MemberId == loginState.UserDetail.UserId)
                        {
                            saveCommittee.committeeMembers.Where(x => x.MemberId == loginState.UserDetail.UserId).ToList().ForEach(x => { x.CommitteeRoleId = committeeMembersVm.CommitteeRoleId; x.RoleNameEn = committeeMembersVm.RoleNameEn; x.RoleNameAr = committeeMembersVm.RoleNameAr; });
                            CommitteeMembers.Where(x => x.MemberId == loginState.UserDetail.UserId).ToList().ForEach(x => { x.CommitteeRoleId = committeeMembersVm.CommitteeRoleId; x.RoleNameEn = committeeMembersVm.RoleNameEn; x.RoleNameAr = committeeMembersVm.RoleNameAr; }); ;

                        }
                        else
                        {
                            saveCommittee.committeeMembers.Add(committeeMembersVm);
                            CommitteeMembers.Add(committeeMembersVm);
                        }
                        await committeeMembersGrid.Reload();
                        committeeMembersVm = new CommitteeMembersVMs();
                    }
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

        #region Add Members Tasks
        protected async Task AddMemberTasks()
        {
            try
            {
                if (string.IsNullOrEmpty(committeeTasksVm.MemberId) || string.IsNullOrEmpty(committeeTasksVm.TaskName))
                {
                    memberValidationMessage = string.IsNullOrEmpty(committeeTasksVm.MemberId) ? translationState.Translate("Required_Field") : "";
                    taskValidationMessage = string.IsNullOrEmpty(committeeTasksVm.TaskName) ? translationState.Translate("Required_Field") : "";
                }
                else
                {
                    committeeTasksVm.MemeberNameEn = saveCommittee.committeeMembers.Where(x => x.MemberId == committeeTasksVm.MemberId).Select(x => x.MemeberNameEn).FirstOrDefault();
                    committeeTasksVm.MemeberNameAr = saveCommittee.committeeMembers.Where(x => x.MemberId == committeeTasksVm.MemberId).Select(x => x.MemeberNameAr).FirstOrDefault();
                    committeeTasksVm.CommitteeId = saveCommittee.committee.Id;
                    saveCommittee.committeeTasks.Add(committeeTasksVm);
                    await committeeTaskGrid.Reload();
                    committeeTasksVm = new TempCommitteeTaskVm();
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

        #region Delete members
        protected async Task DeleteMember(CommitteeMembersVMs args)
        {
            try
            {
                if (saveCommittee.committeeMembers.Contains(args))
                {
                    saveCommittee.committeeMembers.Remove(args);
                }
                else
                {
                    saveCommittee.deletedMembers.Add(args);
                }
                CommitteeMembers.Remove(args);
                var tasksToDelete = saveCommittee.committeeTasks.Where(x => x.MemberId == args.MemberId).ToList();
                foreach (var task in tasksToDelete)
                {
                    saveCommittee.committeeTasks.Remove(task);
                    await committeeTaskGrid.Reload();

                }
                await committeeMembersGrid.Reload();
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

        #region Delete members Tasks
        protected async Task DeleteMemberTasks(TempCommitteeTaskVm args)
        {
            try
            {
                if (saveCommittee.committeeTasks.Contains(args))
                {
                    saveCommittee.committeeTasks.Remove(args);
                }
                saveCommittee.committeeTasks.Remove(args);
                await committeeTaskGrid.Reload();

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

        #region Save As Draft 
        protected async Task SaveAsDraft()
        {
            try
            {
                if (saveCommittee.committee.CommencementDate > saveCommittee.committee.ExpectedEndDate)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("ExpectedEndDate_NotGreater_CommencementDate"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    Keywords = true;
                    return;
                }
                bool? dialogResponse = await dialogService.Confirm(
                   translationState.Translate("Sure_Save_Draft"),
                   translationState.Translate("Confirm"),
                   new ConfirmOptions()
                   {
                       OkButtonText = @translationState.Translate("OK"),
                       CancelButtonText = @translationState.Translate("Cancel")
                   });
                if (dialogResponse == true)
                {
                    saveCommittee.committee.StatusId = (int)CommitteeStatusEnum.Draft;
                    saveCommittee.LoginUserId = loginState.UserDetail.UserId;
                    saveCommittee.SectorTypeId = loginState.UserDetail.SectorTypeId;
                    ApiCallResponse response;
                    if (CommitteeId == null)
                    {
                        response = await organizingCommitteeService.SaveCommittee(saveCommittee);

                    }
                    else
                    {

                        response = await organizingCommitteeService.UpdateCommittee(saveCommittee);
                    }
                    if (response.IsSuccessStatusCode)
                    {
                        var apiResponse = (ApiReturnTaskNotifAuditLogVM)response.ResultData;
                        if (apiResponse.processLog != null)
                        {
                            await processLogService.CreateProcessLog(apiResponse.processLog);
                        }
                        await SaveTempAttachementToUploadedDocument();
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Committee_Successfully_Added_As_Draft"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        StateHasChanged();
                        await RedirectBack();
                    }
                    else
                    {
                        var errorLog = (ErrorLog)response.ResultData;
                        if (errorLog != null)
                        {
                            await errorLogService.CreateErrorLog(errorLog);

                        }
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }
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

        #region Form Submit
        protected async Task FormSubmit()
        {
            try
            {
                committeeAttachment = await fileUploadService.GetTempAttachements(saveCommittee.committee.Id);
                committeeAttachment2 = await fileUploadService.GetUploadedAttachements(false, 0, saveCommittee.committee.Id);
                committeeAttachment = new ObservableCollection<TempAttachementVM>(committeeAttachment?.Concat(committeeAttachment2).ToList());

                if (committeeAttachment.Where(x => x.AttachmentTypeId == (int)AttachmentTypeEnum.FatwaCircular).Any())
                {
                    if (saveCommittee.committeeMembers.Count > 0)
                    {
                        if (saveCommittee.committeeMembers.Any(member => member.CommitteeRoleId == (int)CommitteeRoleEnum.CommitteeHead))
                        {
                            bool? dialogResponse = await dialogService.Confirm(
                        translationState.Translate("Sure_Submit"),
                        translationState.Translate("Confirm"),
                        new ConfirmOptions()
                        {
                            OkButtonText = @translationState.Translate("OK"),
                            CancelButtonText = @translationState.Translate("Cancel")
                        });
                            if (dialogResponse == true)
                            {
                                saveCommittee.committee.StatusId = (int)CommitteeStatusEnum.Created;
                                saveCommittee.LoginUserId = loginState.UserDetail.UserId;
                                saveCommittee.SectorTypeId = loginState.UserDetail.SectorTypeId;
                                saveCommittee.AdditionalTempFiles = dataCommunicationService.saveCommitteeVM.AdditionalTempFiles;
                                saveCommittee.MandatoryTempFiles = dataCommunicationService.saveCommitteeVM.MandatoryTempFiles;
                                ApiCallResponse response;
                                if (CommitteeId == null)
                                {
                                    response = await organizingCommitteeService.SaveCommittee(saveCommittee);

                                }
                                else
                                {
                                    response = await organizingCommitteeService.UpdateCommittee(saveCommittee);
                                }
                                if (response.IsSuccessStatusCode)
                                {
                                    var apiResponse = (ApiReturnTaskNotifAuditLogVM)response.ResultData;
                                    if (apiResponse.addedTaskList.Count() > 0)
                                    {
                                        await taskService.AddTaskList(apiResponse.addedTaskList);
                                    }
                                    if (apiResponse.sendNotifications.Count > 0)
                                    {
                                        await notificationDetailService.SendNotification(apiResponse.sendNotifications);
                                    }
                                    if (apiResponse.processLog != null)
                                    {
                                        await processLogService.CreateProcessLog(apiResponse.processLog);
                                    }
                                    await SaveTempAttachementToUploadedDocument();
                                    notificationService.Notify(new NotificationMessage()
                                    {
                                        Severity = NotificationSeverity.Success,
                                        Detail = translationState.Translate("Committee_Successfully_Added"),
                                        Style = "position: fixed !important; left: 0; margin: auto; "
                                    });
                                    StateHasChanged();
                                    await RedirectBack();
                                }
                                else
                                {
                                    var errorLog = (ErrorLog)response.ResultData;
                                    if (errorLog != null)
                                    {
                                        await errorLogService.CreateErrorLog(errorLog);

                                    }
                                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                                }
                            }
                        }
                        else
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Error,
                                Detail = translationState.Translate("Add_AtLeast_One_Committee_Head"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                        }
                    }
                    else
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Error,
                            Detail = translationState.Translate("Add_Atleast_One_Member"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                    }
                }
                else
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Required_Document_Committee"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    return;
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

        #region On Change Assignment Option
        protected async Task OnChangeOption(int roleId)
        {
            await AddCreatorInMemberList(roleId);
            await committeeMembersGrid.Reload();
        }
        #endregion
        #region Save Temp attachment to Uploaded Document
        protected async Task SaveTempAttachementToUploadedDocument()
        {
            try
            {
                var docResponse = await fileUploadService.SaveTempAttachementToUploadedDocument(new FileUploadVM()
                {
                    RequestIds = new List<Guid> { saveCommittee.committee.Id },
                    CreatedBy = loginState.Username,
                    FilePath = _config.GetValue<string>("dms_file_path"),
                    DeletedAttachementIds = saveCommittee.committee.DeletedAttachementIds
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
        #endregion

        #region Redirect and Dialog Events

        protected async void ButtonCancelClick(MouseEventArgs args)
        {
            await RedirectBack();
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

        #region On Change Commencement Date 
        protected async Task OnChangeCommencementDate()
        {
            try
            {
                if (saveCommittee.committee.CommencementDate > saveCommittee.committee.ExpectedEndDate)
                {
                    saveCommittee.committee.ExpectedEndDate = saveCommittee.committee.CommencementDate;
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
    
    }
}
