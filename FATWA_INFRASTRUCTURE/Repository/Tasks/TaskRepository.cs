using AutoMapper;
using FATWA_DOMAIN.Interfaces.Tasks;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.Consultation;
using FATWA_DOMAIN.Models.Dms;
using FATWA_DOMAIN.Models.Notifications;
using FATWA_DOMAIN.Models.TaskModels;
using FATWA_DOMAIN.Models.ViewModel.DmsVMs;
using FATWA_DOMAIN.Models.ViewModel.MobileAppVMs;
using FATWA_DOMAIN.Models.ViewModel.TaskVMs;
using FATWA_INFRASTRUCTURE.Database;
using FATWA_INFRASTRUCTURE.Repository.NotificationRepo;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.Consultation.ConsultationEnums;
using static FATWA_DOMAIN.Enums.DmsEnums;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using static FATWA_DOMAIN.Enums.TaskEnums;
using System.Reflection.Emit;
using Itenso.TimePeriod;
using Azure;
using System.Linq;
using MsgReader.Outlook;
using Microsoft.Data.SqlClient.DataClassification;
using FATWA_DOMAIN.Models.ViewModel;
using static Org.BouncyCastle.Math.EC.ECCurve;
using System.ComponentModel;

namespace FATWA_INFRASTRUCTURE.Repository.Tasks
{
    public class TaskRepository : ITask
    {
        private readonly IConfiguration _config;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        private readonly DatabaseContext _dbContext;
        private readonly DmsDbContext _dmsDbContext;
        private UserTodoList? _UserTodoList = new UserTodoList();
        private readonly RoleRepository _roleRepo;
        private readonly NotificationRepository _notificationRepo;

        SaveTaskVM saveTask = new SaveTaskVM()
        {
            Task = new UserTask(),
            TaskActions = new List<TaskAction>(),
            DeletedTaskActionIds = new List<Guid>(),
        };
        public TaskRepository(DatabaseContext dbContext, IConfiguration configuration, IServiceScopeFactory serviceScopeFactory, DmsDbContext dmsDbContext)
        {
            _dbContext = dbContext;
            _config = configuration;
            _serviceScopeFactory = serviceScopeFactory;
            _dmsDbContext = dmsDbContext;
            using var scope = _serviceScopeFactory.CreateScope();
            _roleRepo = scope.ServiceProvider.GetRequiredService<RoleRepository>();
            _notificationRepo = scope.ServiceProvider.GetRequiredService<NotificationRepository>();
        }

        #region GET

        public async Task<List<TaskVM>> GetTasksList(AdvanceSearchTaskVM advanceSearchVM)
        {
            try
            {
                string fromDate = advanceSearchVM.FromDate != null ? Convert.ToDateTime(advanceSearchVM.FromDate).ToString("yyyy/MM/dd HH:mm") : null;
                string toDate = advanceSearchVM.ToDate != null ? Convert.ToDateTime(advanceSearchVM.ToDate).ToString("yyyy/MM/dd HH:mm") : null;
                string userId = advanceSearchVM.UserId != null ? advanceSearchVM.UserId : null;
                string assignedBy = advanceSearchVM.AssignedBy != null ? advanceSearchVM.AssignedBy : null;
                string taskName = advanceSearchVM.Name != null ? advanceSearchVM.Name : null;
                string taskDescription = advanceSearchVM.Description != null ? advanceSearchVM.Description : null;
                int? taskStatusId = advanceSearchVM.TaskStatusId != null ? advanceSearchVM.TaskStatusId : null;

                string storedProc = $"exec pTaskList  @FromDate = N'{fromDate}', @ToDate = N'{toDate}', @UserId = N'{userId}', " +
                                    $"@AssignedBy = N'{assignedBy}', @TaskName = N'{taskName}', @TaskDescription = N'{taskDescription}'," +
                                    $"@TaskStatusId=N'{taskStatusId}', @Selectedindex=N'{advanceSearchVM.SelectedIndex}' " +
                                    $",@PageNumber ='{advanceSearchVM.PageNumber}',@PageSize ='{advanceSearchVM.PageSize}' , @ReferenceNumber='{advanceSearchVM.ReferenceNumber}'";
                return await _dbContext.TaskVMs.FromSqlRaw(storedProc).ToListAsync();
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public async Task<SaveTaskVM> GetTaskById(Guid taskId)
        {
            try
            {
                UserTask task = await _dbContext.Tasks.FirstOrDefaultAsync(x => x.TaskId == taskId);
                if (task != null)
                {
                    saveTask.Task = task;

                    var actionItems = await GetTaskActionsByTaskId(taskId);
                    if (actionItems != null)
                    {
                        saveTask.TaskActions = actionItems;
                    }
                }
                return saveTask;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<TaskAction>> GetTaskActionsByTaskId(Guid taskId)
        {
            try
            {
                var actionItems = await _dbContext.TaskActions.Where(x => x.TaskId == taskId).ToListAsync();
                if (actionItems != null)
                {
                    return actionItems;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<TaskDetailVM> GetTaskDetailById(Guid taskId)
        {
            try
            {
                string storedProc = $"exec pTaskDetail @TaskId = N'{taskId}'";
                var result = await _dbContext.TaskDetailVMs.FromSqlRaw(storedProc).ToListAsync();
                if (result is not null)
                {
                    return result.FirstOrDefault();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<TaskDetailVM> GetTaskDetailByReferenceAndUserId(Guid referenceId, string userId)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var _dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            using (_dbContext)
            {
                try
                {
                    string storedProc = $"exec pTaskDetailByReferenceAndUserId @ReferenceId = N'{referenceId}', @UserId = '{userId}'";
                    var result = await _dbContext.TaskDetailVMs.FromSqlRaw(storedProc).ToListAsync();
                    if (result is not null)
                    {
                        return result.FirstOrDefault();
                    }
                    else
                    {
                        return null;
                    }
                }

                catch (Exception)
                {
                    throw;
                }
            }
        }
        public async Task<List<TaskEntityHistoryVM>> GetTaskEntityHistoryByReferenceIdAndSubmodule(Guid referenceId, int submoduleId)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var _dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            using (_dbContext)
            {
                try
                {
                    string storedProc = $"exec pTaskViewEntityHistory @referenceId = N'{referenceId}', @submoduleId = '{submoduleId}'";
                    var result = await _dbContext.TaskEntityHistoryVMs.FromSqlRaw(storedProc).ToListAsync();
                    if (result is not null)
                    {
                        return result;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
        public async Task<UserTask> GetTaskDetailByReferenceAndAssignedToId(Guid referenceId, string userId)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var _dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            using (_dbContext)
            {
                try
                {
                    //string storedProc = $"exec pTaskDetailByReferenceAndUserId @ReferenceId = N'{referenceId}', @UserId = '{userId}'";
                    var result = await _dbContext.Tasks.Where(x => x.ReferenceId == referenceId && x.AssignedTo == userId && x.TaskStatusId == (int)TaskStatusEnum.Pending).FirstOrDefaultAsync();
                    if (result is not null)
                    {
                        return result;
                    }
                    else
                    {
                        return new UserTask();
                    }
                }

                catch (Exception)
                {
                    throw;
                }
            }
        }
        public async Task<List<DraftListVM>> GetDraftList(AdvanceSearchDraftVM advanceSearchVM)
        {
            try
            {
                string FromDate = advanceSearchVM.FromDate != null ? Convert.ToDateTime(advanceSearchVM.FromDate).ToString("yyyy/MM/dd HH:mm") : null;
                string ToDate = advanceSearchVM.ToDate != null ? Convert.ToDateTime(advanceSearchVM.ToDate).ToString("yyyy/MM/dd HH:mm") : null;
                string DraftName = advanceSearchVM.Name != null ? advanceSearchVM.Name : null;
                string FileNumber = advanceSearchVM.FileNumber != null ? advanceSearchVM.FileNumber : null;


                string storedProc = $"exec pDraftListView @FromDate = N'{FromDate}', @ToDate = N'{ToDate}', @Name = N'{DraftName}', " +
                                    $"@FileNumber = N'{FileNumber}' ";
                return await _dbContext.DraftListVMs.FromSqlRaw(storedProc).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        // HISTORY HASSAN IFTIKHAR
        public async Task<TaskDashboardVM> GetTaskDashboard(string item, string UserName)
        {
            try
            {
                TaskDashboardVM _DashBoardVM = new TaskDashboardVM();
                var storedProc = $"exec pTaskDashBoard @UserId = N'{item}'";
                var result = await _dbContext.TaskDashboardVMs.FromSqlRaw(storedProc).ToListAsync();

                var todoListDetail = _dbContext.UserTodoLists.FirstOrDefault(x => ((x.UserId == UserName) && (x.CreatedDate.Date == DateTime.Now.Date)));

                if (result != null && result.Any())
                {
                    _DashBoardVM = result.First();
                }
                if (todoListDetail != null)
                {
                    _DashBoardVM.ToDoItem = todoListDetail.Description;
                }
                return _DashBoardVM;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> GetMaxTaskNumber()
        {
            try
            {
                //var taskNo = _dbContext.Tasks.DefaultIfEmpty().Max(x => x.TaskNumber);
                var taskNo = _dbContext.Tasks.Select(t => t.TaskNumber).DefaultIfEmpty().ToList().Max();
                if (taskNo != null)
                {
                    taskNo = taskNo + 1;
                }
                return taskNo;
            }
            catch (Exception)
            {
                throw;
            }
        }


        #endregion

        #region Save

        public async Task<bool> AddTask(SaveTaskVM task, string action, string entityName, string entityId)
        {
            bool isSaved;
            using var scope = _serviceScopeFactory.CreateScope();
            var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

            using (_DbContext)
            {
                using (var transaction = _DbContext.Database.BeginTransaction())
                {
                    try
                    {
                        if (task.Task.IsSystemGenerated)
                        {
                            isSaved = GenerateUrl(task.Task, action, entityName, entityId);
                            isSaved = GenerateTaskNumber(task.Task, _DbContext);
                        }

                        isSaved = await SaveTask(task.Task, false, _DbContext);
                        //if (!task.Task.IsSystemGenerated)
                        //{
                        //	isSaved = await SaveTaskAttachment(task, _dbContext);
                        //}

                        if (task.TaskActions.Any())
                            isSaved = await SaveTaskActions(task, _DbContext);

                        if (isSaved)
                            transaction.Commit();
                    }
                    catch (Exception)
                    {
                        isSaved = false;
                        transaction.Rollback();
                    }
                }
            }

            return isSaved;
        }

        public async Task<bool> EditTask(SaveTaskVM task, string action, string entityName, string entityId)
        {
            bool isSaved;
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                using (_DbContext)
                {
                    using (var transaction = _DbContext.Database.BeginTransaction())
                    {
                        if (task.Task.IsSystemGenerated)
                        {
                            isSaved = GenerateUrl(task.Task, action, entityName, entityId);
                            isSaved = GenerateTaskNumber(task.Task, _DbContext);
                        }

                        isSaved = await SaveTask(task.Task, true, _DbContext);
                        if (!task.Task.IsSystemGenerated)
                        {
                            isSaved = await SaveTaskAttachment(task, _dmsDbContext);
                        }

                        if (task.TaskActions.Any())
                            isSaved = await SaveTaskActions(task, _DbContext);

                        if (isSaved)
                            transaction.Commit();
                    }
                }

            }
            catch (Exception)
            {
                return false;
                throw;
            }
            return isSaved;
        }

        private async Task<bool> SaveTaskAttachment(SaveTaskVM task, DmsDbContext dmsDbContext)
        {
            bool isSaved = false;
            try
            {
                //remove existing attachments 
                foreach (var deletedAttachementId in task.DeletedAttachementIds)
                {
                    var attachement = await dmsDbContext.UploadedDocuments.FindAsync(deletedAttachementId);

                    string filePath = attachement.StoragePath;
                    string basePath = Path.Combine(Directory.GetCurrentDirectory() + attachement.StoragePath);
                    basePath = basePath.Replace("FATWA_API", "FATWA_WEB");

                    if (File.Exists(basePath))
                    {
                        File.Delete(basePath);
                    }
                    dmsDbContext.UploadedDocuments.Remove(attachement);
                    await dmsDbContext.SaveChangesAsync();
                }

                //add new attachments 
                var attachements = await dmsDbContext.TempAttachements.Where(x => x.Guid == task.Task.TaskId).ToListAsync();
                foreach (var file in attachements)
                {
                    UploadedDocument documentObj = new UploadedDocument();
                    documentObj.Description = "Task file";
                    documentObj.CreatedDateTime = DateTime.Now;
                    documentObj.CreatedBy = task.Task.CreatedBy;
                    documentObj.IsDeleted = false;
                    documentObj.IsActive = true;
                    documentObj.DocumentDate = DateTime.Now;
                    documentObj.FileName = file.FileName;
                    documentObj.StoragePath = file.StoragePath;
                    documentObj.DocType = file.DocType;
                    documentObj.ReferenceGuid = task.Task.TaskId;
                    documentObj.CreatedAt = file.StoragePath;
                    documentObj.AttachmentTypeId = file.AttachmentTypeId;
                    await dmsDbContext.UploadedDocuments.AddAsync(documentObj);
                    await dmsDbContext.SaveChangesAsync();
                    await Task.Delay(200);
                    dmsDbContext.TempAttachements.Remove(file);
                    await dmsDbContext.SaveChangesAsync();
                }
                isSaved = true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return isSaved;
        }

        private bool GenerateUrl(UserTask task, string action, string entityName, string entityId)
        {
            bool isSaved = false;
            try
            {
                //if (action.StartsWith("-transfer-review") && (entityName.StartsWith("ConsultationRequest")))
                //{
                //    if (action != "list" && action != "delete")
                //    {
                //        string sectorTypeId = action.Substring(action.IndexOf("_") + 1);
                //        task.Url = $"consultationrequest-transfer-review/{sectorTypeId}/{entityId}";
                //    }
                //    else
                //    {
                //        task.Url = $"{entityName.ToLower()}-{action}";
                //    }
                //}
                //else if (action.StartsWith("consultationrequest-detail"))
                //{
                //    if (action != "list" && action != "delete")
                //    {
                //        string sectorTypeId = action.Substring(action.IndexOf("_") + 1);
                //        task.Url = $"consultationrequest-detail/{entityId}/{sectorTypeId}";
                //    }
                //    else
                //    {
                //        task.Url = $"{entityName.ToLower()}-{action}";
                //    }
                //}
                //else if (action.StartsWith("transfer-review_") && (entityName.StartsWith("ConsultationFile")))
                //{
                //    if (action != "list" && action != "delete")
                //    {
                //        string sectorTypeId = action.Substring(action.IndexOf("_") + 1);
                //        task.Url = $"consultationfile-transfer-review/{sectorTypeId}/{entityId}";
                //    }


                //    else
                //    {
                //        task.Url = $"{entityName.ToLower()}{"-"}{action.ToLower()}";
                //    }


                //}

                //else if (action.StartsWith("consultationfile-detail_"))
                //{
                //    if (action != "list" && action != "delete")
                //    {
                //        string sectorTypeId = action.Substring(action.IndexOf("_") + 1);
                //        task.Url = $"consultationfile-view/{entityId}/{sectorTypeId}";
                //    }

                //    else
                //    {
                //        task.Url = $"{entityName.ToLower()}{"-"}{action.ToLower()}";
                //    }
                //}

               
                    if (action != "list" && action != "delete")
                    {
                        task.Url = $"{entityName.ToLower()}{"-"}{action.ToLower()}/{entityId}";
                    }


                    else
                    {
                        task.Url = $"{entityName.ToLower()}{"-"}{action.ToLower()}";
                    }
                
                isSaved = true;
            }
            catch
            {
                throw;
            }
            return isSaved;
        }

        private bool GenerateTaskNumber(UserTask task, DatabaseContext dbContext)
        {
            bool isSaved = false;
            try
            {
                //var taskNo = dbContext.Tasks.Max(x => x.TaskNumber);
                var taskNo = dbContext.Tasks.Select(t => t.TaskNumber).DefaultIfEmpty().ToList().Max();
                if (taskNo != null)
                {
                    taskNo = taskNo + 1;

                    task.TaskNumber = taskNo;
                    isSaved = true;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return isSaved;
        }

        private async Task<UserTask> SaveTaskDetails(SaveTaskVM task, string userId)
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                using (_DbContext)
                {
                    if (task.Task.IsSystemGenerated)
                    {
                        task.Task.TaskId = Guid.NewGuid();
                        task.Task.AssignedTo = userId;
                        GenerateUrl(task.Task, task.Action, task.EntityName, task.EntityId);
                        GenerateTaskNumber(task.Task, _DbContext);
                    }
                    await SaveTask(task.Task, false, _DbContext);
                    if (task.TaskActions != null && task.TaskActions.Any())
                        await SaveTaskActions(task, _DbContext);

                    return task.Task;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private async Task<bool> SaveTask(UserTask task, bool isEdit, DatabaseContext dbContext)
        {
            bool isSaved;
            try
            {
                if (!isEdit)
                {
                    dbContext.Tasks.Add(task);
                }
                else
                {
                    dbContext.Tasks.Update(task);
                }
                await dbContext.SaveChangesAsync();
                isSaved = true;
            }
            catch (Exception)
            {
                throw;
            }
            return isSaved;
        }

        #region Task Actions

        public async Task<bool> SaveTaskActions(SaveTaskVM task, DatabaseContext dbContext)
        {
            bool isSaved;
            List<TaskAction> taskActionList = new List<TaskAction>();
            try
            {
                foreach (var action in task.TaskActions)
                {
                    var actionExist = dbContext.TaskActions.FirstOrDefault(x => x.TaskId == task.Task.TaskId && x.ActionName.Contains(action.ActionName));
                    if (actionExist is null)
                    {
                        TaskAction newAction = new TaskAction()
                        {
                            ActionId = Guid.NewGuid(),
                            ActionName = action.ActionName,
                            TaskId = task.Task.TaskId,
                            DueDate = action.DueDate,
                            CreatedBy = task.Task.CreatedBy,
                            CreatedDate = task.Task.CreatedDate,
                            IsDeleted = task.Task.IsDeleted
                        };
                        taskActionList.Add(newAction);
                    }
                }
                dbContext.TaskActions.AddRange(taskActionList);

                //Remove Actions 
                var deleteActions = task.DeletedTaskActionIds;
                await DeleteActions(deleteActions, task.Task.CreatedBy);

                await dbContext.SaveChangesAsync();
                isSaved = true;
            }
            catch
            {
                isSaved = false;
            }
            return isSaved;
        }

        public async Task<bool> DeleteActions(List<Guid> deleteActions, string deleteBy)
        {
            bool isSaved = true;
            try
            {
                if (deleteActions is not null)
                {
                    foreach (var item in deleteActions)
                    {
                        var deleteAction = await _dbContext.TaskActions.FirstOrDefaultAsync(x => x.ActionId == item);
                        if (deleteAction != null)
                        {
                            deleteAction.IsDeleted = true;
                            deleteAction.DeletedDate = DateTime.Now;
                            deleteAction.DeletedBy = deleteBy;

                            _dbContext.TaskActions.Update(deleteAction);
                        }
                    }
                }
            }
            catch (Exception)
            {
                isSaved = false;
                throw;
            }
            return isSaved;

        }

        //<History Author = 'Hassan Abbas' Date='2024-05-01' Version="1.0" Branch="master">Generate Tasks and Notifications for HOS and Vice HOSs based on Sector Configuration </History>
        public async Task AddTaskAndNotificationForHOSAndViceHOSOfSector(SaveTaskVM task, FATWA_DOMAIN.Models.Notifications.Notification notification, int sectorId, bool verifyViceHOSResponsibility, bool includeHOS, int chamberNumberId)
        {
            try
            {
                List<User> hosAndViceHos = await _roleRepo.GetHOSAndViceHOSBySectorId(sectorId, task != null ? task.Task.CreatedBy : "", verifyViceHOSResponsibility, includeHOS, chamberNumberId);
                foreach (var user in hosAndViceHos)
                {
                    UserTask taskDetails = null;
                    if (task != null)
                    {
                        //Send Task
                        taskDetails = await SaveTaskDetails(task, user.Id);
                    }
                    if (notification != null)
                    {
                        //Send Notification
                        var notifObj = new MapperConfiguration(cfg => cfg.CreateMap<Notification, Notification>()).CreateMapper().Map<Notification>(notification);
                        notifObj.NotificationId = Guid.NewGuid();
                        notifObj.EntityId = taskDetails != null ? notification.EntityId + "/" + taskDetails.TaskId : notification.EntityId;
                        notifObj.ReceiverId = user.Id;
                        await _notificationRepo.SendNotification(notifObj, notifObj.EventId, notifObj.Action, notifObj.EntityName, notifObj.EntityId, notifObj.NotificationParameter);
                    }

                }
            }
            catch (Exception)
            {
            }
        }
        #endregion

        // HISTORY HASSAN IFTIKHAR
        public async Task<bool> SaveToDoList(TaskDashboardVM item)
        {
            bool Saved;
            try
            {
                var res = _dbContext.UserTodoLists.Where(u => (u.UserId == item.User) && (u.CreatedDate.Date == DateTime.Now.Date)).FirstOrDefault();
                if (res == null)
                {
                    _UserTodoList.TodoItemId = Guid.NewGuid();
                    _UserTodoList.Description = item.ToDoItem;
                    _UserTodoList.UserId = item.User;
                    _UserTodoList.CreatedBy = item.User;
                    _UserTodoList.CreatedDate = DateTime.Now;
                    _UserTodoList.IsDeleted = false;
                    _dbContext.UserTodoLists.Add(_UserTodoList);
                    await _dbContext.SaveChangesAsync();
                    Saved = true;
                }
                else
                {

                    res.Description = item.ToDoItem;
                    res.UserId = item.User;
                    res.ModifiedBy = item.User;
                    res.ModifiedDate = DateTime.Now;
                    res.IsDeleted = false;
                    res.Description = item.ToDoItem;

                    _dbContext.UserTodoLists.Update(res);
                    await _dbContext.SaveChangesAsync();
                    Saved = true;
                }

            }
            catch (Exception ex)
            {
                Saved = false;
                throw;
            }
            return Saved;
        }

        public async Task<bool> DecisionTask(TaskDetailVM task)
        {
            bool isSaved = true;
            try
            {
                UserTask userTask = _dbContext.Tasks.FirstOrDefault(m => m.TaskId == task.TaskId);
                if (task.IsMultipleTaskUpdateForSameEntity)
                {
                    List<UserTask> userTasks = await _dbContext.Tasks.Where(n => n.ReferenceId == task.ReferenceId && n.SectorId == task.SectorId && n.TaskStatusId == (int)TaskStatusEnum.Pending
                    && (task.SystemGenTypeIdsToComplete.Contains(n.SystemGenTypeId != null ? (int)n.SystemGenTypeId : 0) 
                    || n.SystemGenTypeId == (int)TaskSystemGenTypeEnum.RegisteredCaseToMoj
                    || n.SystemGenTypeId == (int)TaskSystemGenTypeEnum.CaseFileAssignToSector 
                    || n.SystemGenTypeId == (int)TaskSystemGenTypeEnum.CaseRequestTransfer
                    || n.SystemGenTypeId == (int)TaskSystemGenTypeEnum.CaseFileTransfer 
                    || n.SystemGenTypeId == (int)TaskSystemGenTypeEnum.ConsultationRequestTransfer
                    || n.SystemGenTypeId == (int)TaskSystemGenTypeEnum.ConsultationFileAssignToSector
                    || n.SystemGenTypeId == (int)TaskSystemGenTypeEnum.ConsultationFileTransfer
                    || n.SystemGenTypeId == (int)TaskSystemGenTypeEnum.ExecutionRequest 
                    || n.SystemGenTypeId == (int)TaskSystemGenTypeEnum.CaseRequestSendCopy 
                    || n.SystemGenTypeId == (int)TaskSystemGenTypeEnum.CaseFileSendCopy
                    || n.SystemGenTypeId == (int)TaskSystemGenTypeEnum.CaseRequestDraftDocumentReview 
                    || n.SystemGenTypeId == (int)TaskSystemGenTypeEnum.DraftDocumentReview 
                    || n.SystemGenTypeId == (int)TaskSystemGenTypeEnum.ConsultationRequestDraftDocumentReview
                    || n.SystemGenTypeId == (int)TaskSystemGenTypeEnum.ConsultationDraftDocumentReview 
                    || n.SystemGenTypeId == (int)TaskSystemGenTypeEnum.CaseMigratedFromMOJ
                    || n.SystemGenTypeId == (int)TaskSystemGenTypeEnum.CreateCaseRequest
                    || n.SystemGenTypeId == (int)TaskSystemGenTypeEnum.CreateConsultationRequest)).OrderByDescending(x => x.CreatedDate).ToListAsync();
                    foreach (var item in userTasks)
                    {
                        item.TaskStatusId = task.TaskStatusId;
                        item.Url = task.Url;
                        item.Name = task.Name;
                        item.ModifiedBy = task.ModifiedBy;
                        item.ModifiedDate = DateTime.Now;
                        _dbContext.Tasks.Update(item);
                        await _dbContext.SaveChangesAsync();
                        await UpdateUserTaskViewTime(Guid.Parse(task.AssignedTo), task.ReferenceId);
                    }
                }
                if (task.IsFinalJudgementTrue)
                {
                    List<UserTask> userTasks = await _dbContext.Tasks.Where(n => n.ReferenceId == task.ReferenceId && n.SectorId == task.SectorId && n.TaskStatusId == (int)TaskStatusEnum.Pending).OrderByDescending(x => x.CreatedDate).ToListAsync();
                    foreach (var item in userTasks)
                    {
                        item.TaskStatusId = task.TaskStatusId;
                        item.Url = task.Url;
                        item.Name = task.Name;
                        item.ModifiedBy = task.ModifiedBy;
                        item.ModifiedDate = DateTime.Now;
                        _dbContext.Tasks.Update(item);
                        await _dbContext.SaveChangesAsync();
                        await UpdateUserTaskViewTime(Guid.Parse(task.AssignedTo), task.ReferenceId);
                    }
                }
                else
                {
                    if (userTask is not null)
                    {
                        userTask.TaskStatusId = task.TaskStatusId;
                        userTask.Url = task.Url;
                        userTask.Name = task.Name;
                        userTask.ModifiedBy = task.ModifiedBy;
                        userTask.ModifiedDate = DateTime.Now;
                        _dbContext.Tasks.Update(userTask);
                        await _dbContext.SaveChangesAsync();
                        await UpdateUserTaskViewTime(Guid.Parse(task.AssignedTo), task.ReferenceId);
                    }
                }
            }
            catch (Exception)
            {
                isSaved = false;
            }
            return isSaved;
        }
        public async Task<bool> DecisionTaskByStatusAndRefId(TaskDetailVM task)
        {
            bool isSaved = true;
            try
            {
                var userTask = _dbContext.Tasks.Where(m => m.ReferenceId == task.ReferenceId && m.SystemGenTypeId == task.SystemGenTypeId).ToList();
                foreach (var item in userTask)
                {
                    if (userTask is not null)
                    {
                        item.TaskStatusId = task.TaskStatusId;
                        item.Url = task.Url;
                        item.Name = task.Name;
                        item.ModifiedBy = task.ModifiedBy;
                        item.ModifiedDate = DateTime.Now;
                        _dbContext.Tasks.Update(item);

                    }
                }

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                isSaved = false;
            }
            return isSaved;
        }

        #region New Assighnmentss
        public async Task<bool> SaveConsultationAssignment(TaskDetailVM task)
        {
            bool isSaved = false;

            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        isSaved = await AddTempConsultationAssignment(task);
                        // isSaved = await DecisionTask(task);
                        if (isSaved)
                        {
                            transaction.Commit();
                        }
                    }
                    catch (Exception)
                    {
                        isSaved = false;
                        transaction.Rollback();

                    }
                    return isSaved;
                }
            }

        }



        public async Task<bool> AddTempConsultationAssignment(TaskDetailVM task)
        {
            bool isSaved = true;
            try
            {
                ConsultationFile file = await _dbContext.ConsultationFiles.FindAsync(task.ReferenceId);
                if (file != null)
                {
                    file.StatusId = (int)CaseFileStatusEnum.AssignToLawyer;
                    _dbContext.ConsultationFiles.Update(file);
                    await _dbContext.SaveChangesAsync();
                }
                List<TempConsultationAssignment> tempConsultationAssignment = await _dbContext.TempConsultationAssignments.Where(m => m.ReferenceId == task.ReferenceId).ToListAsync();
                foreach (var lawyerAssignment in tempConsultationAssignment)
                {
                    ConsultationAssignment consultationAssignment = new ConsultationAssignment()
                    {
                        Id = Guid.NewGuid(),
                        ReferenceId = lawyerAssignment.ReferenceId,
                        LawyerId = lawyerAssignment.LawyerId,
                        SupervisorId = lawyerAssignment.SupervisorId,
                        IsPrimary = lawyerAssignment.IsPrimary,
                        Remarks = lawyerAssignment.Remarks,
                        CreatedBy = task.ModifiedBy,
                        CreatedDate = DateTime.Now,
                        IsDeleted = false
                    };

                    _dbContext.ConsultationAssignments.Add(consultationAssignment);
                    await _dbContext.SaveChangesAsync();
                    await SaveConsultationAssignmentHistory(lawyerAssignment.ReferenceId, lawyerAssignment.CreatedBy, lawyerAssignment.LawyerId, lawyerAssignment.Remarks);
                    await RemoveTempConsultationAssignement(lawyerAssignment);
                }
                if (tempConsultationAssignment.Any())
                    await SaveConsultationAssignmentHistory(tempConsultationAssignment.FirstOrDefault().ReferenceId, tempConsultationAssignment.FirstOrDefault().CreatedBy, tempConsultationAssignment.FirstOrDefault().SupervisorId, tempConsultationAssignment.FirstOrDefault().Remarks);
            }
            catch
            {
                isSaved = false;
                throw;
            }
            return isSaved;
        }
        public async Task SaveConsultationAssignmentHistory(Guid referenceId, string userId, string assigneeId, string remarks)
        {
            try
            {
                ConsultationAssignmentHistory historyObj = new ConsultationAssignmentHistory();
                historyObj.ReferenceId = referenceId;
                historyObj.AssigneeId = assigneeId;
                historyObj.Remarks = remarks;
                historyObj.CreatedBy = userId;
                historyObj.CreatedDate = DateTime.Now;
                await _dbContext.ConsultationAssignmentHistorys.AddAsync(historyObj);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> RemoveTempConsultationAssignement(TempConsultationAssignment lawyerAssignment)
        {
            bool isSaved = true;
            try
            {
                _dbContext.TempConsultationAssignments.Remove(lawyerAssignment);
                await _dbContext.SaveChangesAsync();
            }
            catch
            {
                isSaved = false;
                throw;
            }
            return isSaved;
        }

        #endregion

        public async Task<bool> SaveCaseAssignment(TaskDetailVM task)
        {
            bool isSaved = false;

            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        isSaved = await AddTempCaseAssignment(task);
                        isSaved = await DecisionTask(task);
                        if (isSaved)
                        {
                            transaction.Commit();
                        }
                    }
                    catch (Exception)
                    {
                        isSaved = false;
                        transaction.Rollback();

                    }
                    return isSaved;
                }
            }

        }

        public async Task<bool> AddTempCaseAssignment(TaskDetailVM task)
        {
            bool isSaved = true;
            try
            {
                List<TempCaseAssignment> tempCaseAssignment = new List<TempCaseAssignment>();
                CaseFile file = await _dbContext.CaseFiles.FindAsync(task.ReferenceId);
                if (file != null)
                {
                    file.StatusId = (int)CaseFileStatusEnum.AssignToLawyer;
                    _dbContext.CaseFiles.Update(file);
                    await _dbContext.SaveChangesAsync();
                    tempCaseAssignment = await _dbContext.TempCaseAssignments.Where(m => m.ReferenceId == task.ReferenceId).ToListAsync();
                    if (tempCaseAssignment.Any())
                    {
                        var sectorId = _dbContext.UserEmploymentInformation.Where(u => u.UserId == tempCaseAssignment.FirstOrDefault().LawyerId).FirstOrDefault().SectorTypeId;
                        List<CmsRegisteredCase> regCases = await _dbContext.CmsRegisteredCases.Where(c => c.FileId == task.ReferenceId && c.SectorTypeId == sectorId).ToListAsync();
                        foreach (var regCase in regCases)
                        {
                            var tempCaseAssignment2 = await _dbContext.TempCaseAssignments.Where(m => m.ReferenceId == regCase.CaseId).ToListAsync();
                            tempCaseAssignment = new List<TempCaseAssignment>(tempCaseAssignment?.Concat(tempCaseAssignment2).ToList());
                        }
                    }
                }
                else
                {
                    CmsRegisteredCase regCase = await _dbContext.CmsRegisteredCases.FindAsync(task.ReferenceId);
                    if (regCase != null)
                    {
                        tempCaseAssignment = await _dbContext.TempCaseAssignments.Where(m => m.ReferenceId == task.ReferenceId || m.ReferenceId == regCase.FileId).ToListAsync();
                    }
                    else
                    {
                        tempCaseAssignment = await _dbContext.TempCaseAssignments.Where(m => m.ReferenceId == task.ReferenceId).ToListAsync();
                    }
                }
                foreach (var lawyerAssignment in tempCaseAssignment)
                {
                    CaseAssignment caseAssignment = new CaseAssignment()
                    {
                        Id = Guid.NewGuid(),
                        ReferenceId = lawyerAssignment.ReferenceId,
                        LawyerId = lawyerAssignment.LawyerId,
                        SupervisorId = lawyerAssignment.SupervisorId,
                        IsPrimary = lawyerAssignment.IsPrimary,
                        Remarks = lawyerAssignment.Remarks,
                        CreatedBy = task.ModifiedBy,
                        CreatedDate = DateTime.Now,
                        IsDeleted = false
                    };
                    _dbContext.CaseFileAssignment.Add(caseAssignment);
                    await _dbContext.SaveChangesAsync();
                    await SaveCaseAssignmentHistory(lawyerAssignment.ReferenceId, lawyerAssignment.CreatedBy, lawyerAssignment.LawyerId, lawyerAssignment.Remarks);
                    await RemoveTempCaseAssignement(lawyerAssignment);
                }
                if (tempCaseAssignment.Any())
                    await SaveCaseAssignmentHistory(tempCaseAssignment.FirstOrDefault().ReferenceId, tempCaseAssignment.FirstOrDefault().CreatedBy, tempCaseAssignment.FirstOrDefault().SupervisorId, tempCaseAssignment.FirstOrDefault().Remarks);
            }
            catch
            {
                isSaved = false;
                throw;
            }
            return isSaved;
        }

        public async Task<bool> RemoveTempCaseAssignement(TempCaseAssignment lawyerAssignment)
        {
            bool isSaved = true;
            try
            {
                _dbContext.TempCaseAssignments.Remove(lawyerAssignment);
                await _dbContext.SaveChangesAsync();
            }
            catch
            {
                isSaved = false;
                throw;
            }
            return isSaved;
        }

        public async Task<bool> RemoveAllTempCaseAssignement(TaskDetailVM task)
        {
            bool isSaved = true;
            try
            {
                CaseFile file = await _dbContext.CaseFiles.FindAsync(task.ReferenceId);
                if (file != null)
                {
                    file.StatusId = (int)CaseFileStatusEnum.RejectedByLawyer;
                    _dbContext.CaseFiles.Update(file);
                    await _dbContext.SaveChangesAsync();
                }
                List<TempCaseAssignment> tempCaseAssignment = await _dbContext.TempCaseAssignments.Where(m => m.ReferenceId == task.ReferenceId).ToListAsync();
                _dbContext.TempCaseAssignments.RemoveRange(tempCaseAssignment);
                await _dbContext.SaveChangesAsync();
            }
            catch
            {
                isSaved = false;
                throw;
            }
            return isSaved;
        }

        //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master">Save Case Assigneement History </History>
        public async Task SaveCaseAssignmentHistory(Guid referenceId, string userId, string assigneeId, string remarks)
        {
            try
            {
                CaseAssignmentHistory historyObj = new CaseAssignmentHistory();
                historyObj.ReferenceId = referenceId;
                historyObj.AssigneeId = assigneeId;
                historyObj.Remarks = remarks;
                historyObj.CreatedBy = userId;
                historyObj.CreatedDate = DateTime.Now;
                await _dbContext.CaseFileAssignmentHistory.AddAsync(historyObj);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        //#region Delete Draft

        //// History HAssan Iftikhar

        //public async Task<bool> SoftDeleteDraft(DraftListVM draft)
        //{
        //    bool isSaved = false;
        //    try
        //    {
        //        DraftTask draftTask = _dbContext.DraftTasks.FirstOrDefault(m => m.DraftId == draft.DraftId);
        //        if (draftTask is not null)
        //        {
        //            draftTask.DeletedDate = DateTime.Now;
        //            draftTask.IsDeleted = true;

        //            _dbContext.DraftTasks.Update(draftTask);
        //            await _dbContext.SaveChangesAsync();

        //            isSaved = true;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        isSaved = false;
        //    }
        //    return isSaved;
        //}

        //#endregion

        #region Reject Reason

        public async Task<bool> RejectReason(RejectReason reject)
        {
            try
            {
                var res = _dbContext.RejectReasons.Where(u => (u.ReferenceId == reject.ReferenceId)).FirstOrDefault();
                if (res == null)
                {
                    reject.CreatedDate = DateTime.Now;

                    _dbContext.RejectReasons.Add(reject);
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
                return false;

            }

            catch (Exception)
            {
                return false;
            }

        }

        #endregion

        #region Get Task Response Status

        public async Task<List<TaskResponseStatus>> GetTaskResponseStatus()
        {
            try
            {
                var task = await _dbContext.TaskResponseStatuses.OrderBy(x => x.TaskResponeStatusId).ToListAsync();
                return task;
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region Task Response

        public async Task<bool> SaveTaskResponseDecision(TaskResponseVM task, bool isEdit)
        {
            bool isSaved = false;

            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        isSaved = await SaveTaskResponse(task.TaskResponse, isEdit, _dbContext);
                        isSaved = await UpdateTaskActions(task.TaskActions, _dbContext);
                        await UpdateTaskStatus((Guid)task.TaskResponse.TaskId, task.TaskResponse.TaskResponeStatusId);

                        //isSaved = await SaveTaskResponseAttachment(task, _dbContext);

                        if (isSaved)
                            transaction.Commit();
                    }
                    catch (Exception)
                    {
                        isSaved = false;
                        transaction.Rollback();
                    }

                }
            }

            return isSaved;
        }

        private async Task<bool> SaveTaskResponse(TaskResponse task, bool isEdit, DatabaseContext dbContext)
        {
            bool isSaved;
            try
            {
                if (!isEdit)
                {
                    dbContext.TaskResponses.Add(task);
                }
                else
                {
                    dbContext.TaskResponses.Update(task);
                }
                await dbContext.SaveChangesAsync();
                isSaved = true;
            }
            catch (Exception)
            {
                isSaved = false;
                throw;
            }
            return isSaved;
        }

        private async Task<bool> UpdateTaskActions(List<TaskAction> taskActions, DatabaseContext dbContext)
        {
            bool isSaved = false;
            try
            {
                foreach (var item in taskActions)
                {
                    var taskAction = await dbContext.TaskActions.FirstOrDefaultAsync(x => x.ActionId == item.ActionId);
                    if (taskAction != null)
                    {
                        taskAction.CompleteDate = item.CompleteDate;

                        dbContext.TaskActions.Update(taskAction);
                        await dbContext.SaveChangesAsync();
                    }
                    isSaved = true;
                }
            }
            catch (Exception)
            {
                isSaved = false;
                throw;
            }
            return isSaved;
        }

        #endregion

        #region Cms Tasks List

        //<History Author = "Hassan Abbas" Date="2023-03-08" Version="1.0" Branch="master">List of Tasks related to Case Management filtered based on screen, task type and submodule</History>
        public async Task<List<TaskVM>> GetCmsTasksList(AdvanceSearchTaskVM advanceSearchVM)
        {
            try
            {
                string fromDate = advanceSearchVM.FromDate != null ? Convert.ToDateTime(advanceSearchVM.FromDate).ToString("yyyy/MM/dd HH:mm") : null;
                string toDate = advanceSearchVM.ToDate != null ? Convert.ToDateTime(advanceSearchVM.ToDate).ToString("yyyy/MM/dd HH:mm") : null;
                string userId = advanceSearchVM.UserId != null ? advanceSearchVM.UserId : null;
                string assignedBy = advanceSearchVM.AssignedBy != null ? advanceSearchVM.AssignedBy : null;
                string taskName = advanceSearchVM.Name != null ? advanceSearchVM.Name : null;
                string taskDescription = advanceSearchVM.Description != null ? advanceSearchVM.Description : null;
                int? taskStatusId = advanceSearchVM.TaskStatusId != null ? advanceSearchVM.TaskStatusId : null;
                int startValue = 0; int endValue = 0;
                int startModule = 0; int endModule = 0;
                string procName = "";

                if (advanceSearchVM.ScreenId == (int)CaseScreensEnum.ListRequest)
                {
                    startValue = (int)TaskSystemGenTypeEnum.CaseRequestTransfer;
                    endValue = (int)TaskSystemGenTypeEnum.CaseRequestDraftDocumentReview;
                    startModule = (int)SubModuleEnum.CaseRequest;
                    endModule = (int)SubModuleEnum.CaseRequest;
                    procName = "pCmsTaskList";
                }
                else if (advanceSearchVM.ScreenId == (int)CaseScreensEnum.ListUnderFilePendingRequests)
                {
                    startValue = (int)TaskSystemGenTypeEnum.CaseFileTransfer;
                    endValue = (int)TaskSystemGenTypeEnum.DraftDocumentReview;
                    startModule = (int)SubModuleEnum.CaseFile;
                    endModule = (int)SubModuleEnum.CaseFile;
                    procName = "pCmsTaskListForUnderFilingScreen";
                }
                else if (advanceSearchVM.ScreenId == (int)CaseScreensEnum.ListUnderFilePendingCaseFiles)
                {
                    startValue = (int)TaskSystemGenTypeEnum.CaseFileAssignToLawyer;
                    endValue = (int)TaskSystemGenTypeEnum.CaseFileAssignToLawyer;
                    startModule = (int)SubModuleEnum.CaseFile;
                    endModule = (int)SubModuleEnum.CaseFile;
                    procName = "pCmsTaskListForUnderFilingScreen";
                }
                else if (advanceSearchVM.ScreenId == (int)CaseScreensEnum.ListCaseFilePendingRequests)
                {
                    startValue = (int)TaskSystemGenTypeEnum.CaseFileTransfer;
                    endValue = (int)TaskSystemGenTypeEnum.ExecutionRequest;
                    startModule = (int)SubModuleEnum.CaseFile;
                    endModule = (int)SubModuleEnum.RegisteredCase;
                    procName = "pCmsTaskListForCaseFilingScreen";
                }
                else if (advanceSearchVM.ScreenId == (int)CaseScreensEnum.ListCaseFilePendingCaseFiles)
                {
                    startValue = (int)TaskSystemGenTypeEnum.CaseFileAssignToLawyer;
                    endValue = (int)TaskSystemGenTypeEnum.RegisteredCaseAssignToLawyer;
                    startModule = (int)SubModuleEnum.CaseFile;
                    endModule = (int)SubModuleEnum.RegisteredCase;
                    procName = "pCmsTaskListForCaseFilingScreen";
                }

                string storedProc = $"exec {procName}  @FromDate = N'{fromDate}', @ToDate = N'{toDate}', @UserId = N'{userId}', " +
                                    $"@AssignedBy = N'{assignedBy}', @TaskName = N'{taskName}', @TaskDescription = N'{taskDescription}'," +
                                    $"@TaskStatusId=N'{taskStatusId}', @StartSubModule='{startModule}', @EndSubModule='{endModule}', @StartValue='{startValue}', @EndValue='{endValue}' ";
                return await _dbContext.TaskVMs.FromSqlRaw(storedProc).ToListAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //<History Author = 'Hassan Abbas' Date='2023-04-18' Version="1.0" Branch="master">Get Tasks related to Cases </History>
        public async Task<List<TaskVM>> GetAllCmsTasks(AdvanceSearchTaskVM advanceSearchVM)
        {
            try
            {
                string fromDate = advanceSearchVM.FromDate != null ? Convert.ToDateTime(advanceSearchVM.FromDate).ToString("yyyy/MM/dd HH:mm") : null;
                string toDate = advanceSearchVM.ToDate != null ? Convert.ToDateTime(advanceSearchVM.ToDate).ToString("yyyy/MM/dd HH:mm") : null;
                string fromHearingDate = advanceSearchVM.FromHearingDate != null ? Convert.ToDateTime(advanceSearchVM.FromHearingDate).ToString("yyyy/MM/dd HH:mm") : null;
                string toHearingDate = advanceSearchVM.ToHearingDate != null ? Convert.ToDateTime(advanceSearchVM.ToHearingDate).ToString("yyyy/MM/dd HH:mm") : null;
                string userId = advanceSearchVM.UserId != null ? advanceSearchVM.UserId : null;
                string assignedBy = advanceSearchVM.AssignedBy != null ? advanceSearchVM.AssignedBy : null;
                string taskName = advanceSearchVM.Name != null ? advanceSearchVM.Name : null;
                string taskDescription = advanceSearchVM.Description != null ? advanceSearchVM.Description : null;
                int? taskStatusId = advanceSearchVM.TaskStatusId != null ? advanceSearchVM.TaskStatusId : null;
                int startValue = 0; int endValue = 0;
                int startModule = 0; int endModule = 0;
                string procName = "";

                startValue = (int)TaskSystemGenTypeEnum.CaseRequestTransfer;
                endValue = (int)TaskSystemGenTypeEnum.FinalJudgementIssued;
                startModule = (int)SubModuleEnum.CaseRequest;
                endModule = (int)SubModuleEnum.DMSReviewDocument;
                procName = "pCmsTaskList";

                string storedProc = $"exec {procName}  @FromDate = N'{fromDate}', @ToDate = N'{toDate}', @UserId = N'{userId}', " +
                                    $"@FromHearingDate = N'{fromHearingDate}', @ToHearingDate = N'{toHearingDate}'," +
                                    $"@AssignedBy = N'{assignedBy}', @TaskName = N'{taskName}', @TaskDescription = N'{taskDescription}'," +
                                    $"@TaskStatusId=N'{taskStatusId}', @StartSubModule='{startModule}', @EndSubModule='{endModule}'" +
                                    $",@StartValue='{startValue}', @EndValue='{endValue}', @IsImportant='{advanceSearchVM.IsImpportant}', " +
                                    $" @GeId='{advanceSearchVM.GovermentEntityId}', @ReferenceNumber='{advanceSearchVM.ReferenceNumber}'"+
                                    $",@PageNumber ='{advanceSearchVM.PageNumber}',@PageSize ='{advanceSearchVM.PageSize}'";
                return await _dbContext.TaskVMs.FromSqlRaw(storedProc).ToListAsync();

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<int> GetCountCmsTasksList(AdvanceSearchTaskVM advanceSearchVM)
        {
            try
            {
                string fromDate = advanceSearchVM.FromDate != null ? Convert.ToDateTime(advanceSearchVM.FromDate).ToString("yyyy/MM/dd HH:mm") : null;
                string toDate = advanceSearchVM.ToDate != null ? Convert.ToDateTime(advanceSearchVM.ToDate).ToString("yyyy/MM/dd HH:mm") : null;
                string userId = advanceSearchVM.UserId != null ? advanceSearchVM.UserId : null;
                string assignedBy = advanceSearchVM.AssignedBy != null ? advanceSearchVM.AssignedBy : null;
                string taskName = advanceSearchVM.Name != null ? advanceSearchVM.Name : null;
                string taskDescription = advanceSearchVM.Description != null ? advanceSearchVM.Description : null;
                int? taskStatusId = advanceSearchVM.TaskStatusId != null ? advanceSearchVM.TaskStatusId : null;
                int startValue = 0; int endValue = 0;
                int startModule = 0; int endModule = 0;
                string procName = "";

                if (advanceSearchVM.ScreenId == (int)CaseScreensEnum.ListRequest)
                {
                    startValue = (int)TaskSystemGenTypeEnum.CaseRequestTransfer;
                    endValue = (int)TaskSystemGenTypeEnum.CaseRequestDraftDocumentReview;
                    startModule = (int)SubModuleEnum.CaseRequest;
                    endModule = (int)SubModuleEnum.CaseRequest;
                    procName = "pCmsTaskList";
                }
                else if (advanceSearchVM.ScreenId == (int)CaseScreensEnum.ListUnderFilePendingRequests)
                {
                    startValue = (int)TaskSystemGenTypeEnum.CaseFileTransfer;
                    endValue = (int)TaskSystemGenTypeEnum.DraftDocumentReview;
                    startModule = (int)SubModuleEnum.CaseFile;
                    endModule = (int)SubModuleEnum.CaseFile;
                    procName = "pCmsTaskListForUnderFilingScreen";
                }
                else if (advanceSearchVM.ScreenId == (int)CaseScreensEnum.ListUnderFilePendingCaseFiles)
                {
                    startValue = (int)TaskSystemGenTypeEnum.CaseFileAssignToLawyer;
                    endValue = (int)TaskSystemGenTypeEnum.CaseFileAssignToLawyer;
                    startModule = (int)SubModuleEnum.CaseFile;
                    endModule = (int)SubModuleEnum.CaseFile;
                    procName = "pCmsTaskListForUnderFilingScreen";
                }
                else if (advanceSearchVM.ScreenId == (int)CaseScreensEnum.ListCaseFilePendingRequests)
                {
                    startValue = (int)TaskSystemGenTypeEnum.CaseFileTransfer;
                    endValue = (int)TaskSystemGenTypeEnum.ExecutionRequest;
                    startModule = (int)SubModuleEnum.CaseFile;
                    endModule = (int)SubModuleEnum.RegisteredCase;
                    procName = "pCmsTaskListForCaseFilingScreen";
                }
                else if (advanceSearchVM.ScreenId == (int)CaseScreensEnum.ListCaseFilePendingCaseFiles)
                {
                    startValue = (int)TaskSystemGenTypeEnum.CaseFileAssignToLawyer;
                    endValue = (int)TaskSystemGenTypeEnum.RegisteredCaseAssignToLawyer;
                    startModule = (int)SubModuleEnum.CaseFile;
                    endModule = (int)SubModuleEnum.RegisteredCase;
                    procName = "pCmsTaskListForCaseFilingScreen";
                }

                string storedProc = $"exec {procName}  @FromDate = N'{fromDate}', @ToDate = N'{toDate}', @UserId = N'{userId}', " +
                                    $"@AssignedBy = N'{assignedBy}', @TaskName = N'{taskName}', @TaskDescription = N'{taskDescription}'," +
                                    $"@TaskStatusId=N'{taskStatusId}', @StartSubModule='{startModule}', @EndSubModule='{endModule}', @StartValue='{startValue}', @EndValue='{endValue}' ";
                var tasks = await _dbContext.TaskVMs.FromSqlRaw(storedProc).ToListAsync();
                return tasks.Any() ? tasks.Count : 0;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region Consultation Tasks List

        public async Task<List<TaskVM>> GetComsTasksList(AdvanceSearchTaskVM advanceSearchVM)
        {
            try
            {
                string fromDate = advanceSearchVM.FromDate != null ? Convert.ToDateTime(advanceSearchVM.FromDate).ToString("yyyy/MM/dd HH:mm") : null;
                string toDate = advanceSearchVM.ToDate != null ? Convert.ToDateTime(advanceSearchVM.ToDate).ToString("yyyy/MM/dd HH:mm") : null;
                string userId = advanceSearchVM.UserId != null ? advanceSearchVM.UserId : null;
                string assignedBy = advanceSearchVM.AssignedBy != null ? advanceSearchVM.AssignedBy : null;
                string taskName = advanceSearchVM.Name != null ? advanceSearchVM.Name : null;
                string taskDescription = advanceSearchVM.Description != null ? advanceSearchVM.Description : null;
                int? taskStatusId = advanceSearchVM.TaskStatusId != null ? advanceSearchVM.TaskStatusId : null;
                int startValue = 0; int endValue = 0;
                int startModule = 0; int endModule = 0;
                string procName = "";

                if (advanceSearchVM.ScreenId == (int)ConsultationScreensEnum.ListConsultationRequest)
                {
                    startValue = (int)TaskSystemGenTypeEnum.ConsultationRequestTransfer;
                    endValue = (int)TaskSystemGenTypeEnum.ConsultationRequestDraftDocumentReview;
                    startModule = (int)SubModuleEnum.ConsultationRequest;
                    endModule = (int)SubModuleEnum.ConsultationRequest;
                    procName = "pComsTaskList";
                }
                else if (advanceSearchVM.ScreenId == (int)ConsultationScreensEnum.ListConsultationRequestPendingtransferRequests)
                {
                    startValue = (int)TaskSystemGenTypeEnum.ConsultationFileTransfer;
                    endValue = (int)TaskSystemGenTypeEnum.ConsultationDraftDocumentReview;
                    startModule = (int)SubModuleEnum.ConsultationFile;
                    endModule = (int)SubModuleEnum.ConsultationFile;
                    procName = "pComsTaskListForUnderfilingScreen";
                }
                else if (advanceSearchVM.ScreenId == (int)ConsultationScreensEnum.ListConsultationFilePendingAssignFilesRequest)
                {
                    startValue = (int)TaskSystemGenTypeEnum.ConsultationFileAssignToLawyer;
                    endValue = (int)TaskSystemGenTypeEnum.ConsultationRequestAssignToLawyer;
                    startModule = (int)SubModuleEnum.ConsultationRequest;
                    endModule = (int)SubModuleEnum.ConsultationFile;
                    procName = "pComsTaskListForUnderfilingScreen";
                }


                string storedProc = $"exec {procName}  @FromDate = N'{fromDate}', @ToDate = N'{toDate}', @UserId = N'{userId}', " +
                                    $"@AssignedBy = N'{assignedBy}', @TaskName = N'{taskName}', @TaskDescription = N'{taskDescription}'," +
                                    $"@TaskStatusId=N'{taskStatusId}', @StartSubModule='{startModule}', @EndSubModule='{endModule}', @StartValue='{startValue}', @EndValue='{endValue}' ";
                return await _dbContext.TaskVMs.FromSqlRaw(storedProc).ToListAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<int> GetCountComsTasksList(AdvanceSearchTaskVM advanceSearchVM)
        {
            try
            {
                string fromDate = advanceSearchVM.FromDate != null ? Convert.ToDateTime(advanceSearchVM.FromDate).ToString("yyyy/MM/dd HH:mm") : null;
                string toDate = advanceSearchVM.ToDate != null ? Convert.ToDateTime(advanceSearchVM.ToDate).ToString("yyyy/MM/dd HH:mm") : null;
                string userId = advanceSearchVM.UserId != null ? advanceSearchVM.UserId : null;
                string assignedBy = advanceSearchVM.AssignedBy != null ? advanceSearchVM.AssignedBy : null;
                string taskName = advanceSearchVM.Name != null ? advanceSearchVM.Name : null;
                string taskDescription = advanceSearchVM.Description != null ? advanceSearchVM.Description : null;
                int? taskStatusId = advanceSearchVM.TaskStatusId != null ? advanceSearchVM.TaskStatusId : null;
                int startValue = 0; int endValue = 0;
                int startModule = 0; int endModule = 0;
                string procName = "";

                if (advanceSearchVM.ScreenId == (int)ConsultationScreensEnum.ListConsultationRequest)
                {
                    startValue = (int)TaskSystemGenTypeEnum.ConsultationRequestTransfer;
                    endValue = (int)TaskSystemGenTypeEnum.ConsultationRequestDraftDocumentReview;
                    startModule = (int)SubModuleEnum.ConsultationRequest;
                    endModule = (int)SubModuleEnum.ConsultationRequest;
                    procName = "pComsTaskList";
                }
                else if (advanceSearchVM.ScreenId == (int)ConsultationScreensEnum.ListConsultationRequestPendingtransferRequests)
                {
                    startValue = (int)TaskSystemGenTypeEnum.ConsultationFileTransfer;
                    endValue = (int)TaskSystemGenTypeEnum.ConsultationDraftDocumentReview;
                    startModule = (int)SubModuleEnum.ConsultationFile;
                    endModule = (int)SubModuleEnum.ConsultationFile;
                    procName = "pComsTaskListForUnderfilingScreen";
                }
                else if (advanceSearchVM.ScreenId == (int)ConsultationScreensEnum.ListConsultationFilePendingAssignFilesRequest)
                {
                    startValue = (int)TaskSystemGenTypeEnum.ConsultationFileAssignToLawyer;
                    endValue = (int)TaskSystemGenTypeEnum.ConsultationRequestAssignToLawyer;
                    startModule = (int)SubModuleEnum.ConsultationRequest;
                    endModule = (int)SubModuleEnum.ConsultationFile;
                    procName = "pComsTaskListForUnderfilingScreen";
                }
                string storedProc = $"exec {procName}  @FromDate = N'{fromDate}', @ToDate = N'{toDate}', @UserId = N'{userId}', " +
                                    $"@AssignedBy = N'{assignedBy}', @TaskName = N'{taskName}', @TaskDescription = N'{taskDescription}'," +
                                    $"@TaskStatusId=N'{taskStatusId}', @StartSubModule='{startModule}', @EndSubModule='{endModule}', @StartValue='{startValue}', @EndValue='{endValue}' ";
                var tasks = await _dbContext.TaskVMs.FromSqlRaw(storedProc).ToListAsync();
                return tasks.Any() ? tasks.Count : 0;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Get All COnsultation Task List 
        public async Task<List<TaskVM>> GetAllComsTasks(AdvanceSearchTaskVM advanceSearchVM)
        {
            try
            {
                string fromDate = advanceSearchVM.FromDate != null ? Convert.ToDateTime(advanceSearchVM.FromDate).ToString("yyyy/MM/dd HH:mm") : null;
                string toDate = advanceSearchVM.ToDate != null ? Convert.ToDateTime(advanceSearchVM.ToDate).ToString("yyyy/MM/dd HH:mm") : null;
                string userId = advanceSearchVM.UserId != null ? advanceSearchVM.UserId : null;
                string assignedBy = advanceSearchVM.AssignedBy != null ? advanceSearchVM.AssignedBy : null;
                string taskName = advanceSearchVM.Name != null ? advanceSearchVM.Name : null;
                string taskDescription = advanceSearchVM.Description != null ? advanceSearchVM.Description : null;
                int? taskStatusId = advanceSearchVM.TaskStatusId != null ? advanceSearchVM.TaskStatusId : null;
                int startValue = 0; int endValue = 0;
                int startModule = 0; int endModule = 0;
                string procName = "";

                startValue = (int)TaskSystemGenTypeEnum.ConsultationRequestTransfer;
                endValue = (int)TaskSystemGenTypeEnum.CaseMigratedFromMOJ;
                startModule = (int)SubModuleEnum.ConsultationRequest;
                endModule = (int)SubModuleEnum.ConsultationFile;
                procName = "pComsTaskList";

                string storedProc = $"exec {procName}  @FromDate = N'{fromDate}', @ToDate = N'{toDate}', @UserId = N'{userId}', " +
                                    $"@AssignedBy = N'{assignedBy}', @TaskName = N'{taskName}', @TaskDescription = N'{taskDescription}'," +
                                    $"@TaskStatusId=N'{taskStatusId}', @StartSubModule='{startModule}', @EndSubModule='{endModule}', @StartValue='{startValue}', @EndValue='{endValue}' , @IsImportant='{advanceSearchVM.IsImpportant}', @ReferenceNumber='{advanceSearchVM.ReferenceNumber}', @GeId='{advanceSearchVM.GovermentEntityId}'" +
                                    $",@PageNumber ='{advanceSearchVM.PageNumber}',@PageSize ='{advanceSearchVM.PageSize}'";
                return await _dbContext.TaskVMs.FromSqlRaw(storedProc).ToListAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Update Task Status
        public async Task<bool> UpdateTaskStatus(Guid TaskId, int StatusId)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var _dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            using (_dbContext)
            {
                bool isSaved = true;
                try
                {
                    UserTask task = await _dbContext.Tasks.FindAsync(TaskId);
                    if (task != null)
                    {
                        task.TaskStatusId = StatusId;
                        _dbContext.Tasks.Update(task);
                        await _dbContext.SaveChangesAsync();
                    }
                }
                catch
                {
                    isSaved = false;
                    throw;
                }
                return isSaved;
            }
        }
        #endregion

        public async Task<List<UserTask>> GetTaskListByFileId(Guid referenceId)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var _dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            using (_dbContext)
            {
                try
                {
                    //string storedProc = $"exec pTaskDetailByReferenceAndUserId @ReferenceId = N'{referenceId}', @UserId = '{userId}'";
                    var result = await _dbContext.Tasks.Where(x => x.ReferenceId == referenceId).ToListAsync();
                    if (result is not null)
                    {
                        return result;
                    }
                    else
                    {
                        return new List<UserTask>();
                    }
                }

                catch (Exception)
                {
                    throw;
                }
            }
        }

        #region Get All Case/Consultation Task List 
        public async Task<List<TaskVM>> GetAllCMSComsTasks(AdvanceSearchTaskVM advanceSearchVM)
        {
            try
            {
                string fromDate = advanceSearchVM.FromDate != null ? Convert.ToDateTime(advanceSearchVM.FromDate).ToString("yyyy/MM/dd HH:mm") : null;
                string toDate = advanceSearchVM.ToDate != null ? Convert.ToDateTime(advanceSearchVM.ToDate).ToString("yyyy/MM/dd HH:mm") : null;
                string userId = advanceSearchVM.UserId != null ? advanceSearchVM.UserId : null;
                string assignedBy = advanceSearchVM.AssignedBy != null ? advanceSearchVM.AssignedBy : null;
                string taskName = advanceSearchVM.Name != null ? advanceSearchVM.Name : null;
                string taskDescription = advanceSearchVM.Description != null ? advanceSearchVM.Description : null;
                int? taskStatusId = advanceSearchVM.TaskStatusId != null ? advanceSearchVM.TaskStatusId : null;
                string procName = "";
                string SystemGenTypeIds = (int)TaskSystemGenTypeEnum.CreateCaseRequest + "," +
                                           (int)TaskSystemGenTypeEnum.CaseRequestTransfer + "," +
                                           (int)TaskSystemGenTypeEnum.CreateConsultationRequest + "," +
                                           (int)TaskSystemGenTypeEnum.ConsultationRequestTransfer + "," +
                                           (int)TaskSystemGenTypeEnum.ConsultationFileTransfer + "," +
                                           (int)TaskSystemGenTypeEnum.Meeting + "," +
                                           (int)TaskSystemGenTypeEnum.CaseFileTransfer;
                procName = "pCMS_COMSTaskList";

                string storedProc = $"exec {procName}  @FromDate = N'{fromDate}', @ToDate = N'{toDate}', @UserId = N'{userId}', " +
                                    $"@AssignedBy = N'{assignedBy}', @TaskName = N'{taskName}', @TaskDescription = N'{taskDescription}'," +
                                    $"@TaskStatusId=N'{taskStatusId}', @SystemGenTypeIds='{SystemGenTypeIds}', @IsImportant='{advanceSearchVM.IsImpportant}', @GeId='{advanceSearchVM.GovermentEntityId}' " +
                                    $",@PageNumber ='{advanceSearchVM.PageNumber}',@PageSize ='{advanceSearchVM.PageSize}'";
                return await _dbContext.TaskVMs.FromSqlRaw(storedProc).ToListAsync();

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Delete Task On Reject File
        public async Task<bool> DeleteTask(Guid TaskId)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var _dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            using (_dbContext)
            {
                bool isSaved = true;
                try
                {
                    UserTask task = await _dbContext.Tasks.FindAsync(TaskId);
                    if (task != null)
                    {
                        task.TaskStatusId = (int)TaskStatusEnum.Done;
                        task.IsDeleted = true;
                        _dbContext.Tasks.Update(task);
                        await _dbContext.SaveChangesAsync();
                    }
                }
                catch
                {
                    isSaved = false;
                    throw;
                }
                return isSaved;
            }
        }
        #endregion

        #region Complete All Pending Task 
        public async Task CompleteAllPendingTasks(Guid RequestId, string User)
        {
            var tasks = await _dbContext.Tasks.Where(x => x.ReferenceId == RequestId && x.TaskStatusId == (int)TaskStatusEnum.Pending).ToListAsync();
            var fileId = _dbContext.CaseFiles.Where(x => x.RequestId == RequestId).Select(x => x.FileId).FirstOrDefault();
            if (fileId != Guid.Empty)
            {
                tasks.AddRange(_dbContext.Tasks.Where(x => x.ReferenceId == fileId && x.TaskStatusId == (int)TaskStatusEnum.Pending).ToList());
            }
            tasks = tasks.Select(x =>
            {
                x.TaskStatusId = (int)TaskStatusEnum.Done;
                x.ModifiedBy = User;
                x.ModifiedDate = DateTime.Now;

                return x;
            }).ToList();
            _dbContext.Tasks.UpdateRange(tasks);
            await _dbContext.SaveChangesAsync();
        }
        #endregion


        public async Task<UserTask> GetTaskDetailBySystemGeneratedId(Guid referenceId, string userId, int SystemGeneratedTaskId)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var _dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            using (_dbContext)
            {
                try
                {
                    var result = await _dbContext.Tasks.Where(x => x.ReferenceId == referenceId && x.AssignedTo == userId && x.TaskStatusId == (int)TaskStatusEnum.Pending && x.SystemGenTypeId == SystemGeneratedTaskId).FirstOrDefaultAsync();
                    if (result is not null)
                    {
                        return result;
                    }
                    else
                    {
                        return new UserTask();
                    }
                }

                catch (Exception)
                {
                    throw;
                }
            }
        }
        public async Task<bool> CompleteAssignTask(Guid referenceId)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var _dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            using (_dbContext)
            {
                bool isSaved = true;
                try
                {

                    var result = await _dbContext.Tasks.Where(x => x.ReferenceId == referenceId && x.TaskStatusId == (int)TaskStatusEnum.Pending && x.SystemGenTypeId == (int)TaskSystemGenTypeEnum.ConsultationRequestAssignToLawyer).FirstOrDefaultAsync();
                    if (result is not null)
                    {

                        result.TaskStatusId = (int)TaskStatusEnum.Done;
                        result.ModifiedDate = DateTime.Now;
                        _dbContext.Tasks.Update(result);
                        await _dbContext.SaveChangesAsync();
                    }

                }

                catch (Exception)
                {
                    isSaved = false;
                    throw;
                }
                return isSaved;
            }
        }
        public async Task<UserTask> GetTaskDetailByFileId(Guid FileId)
        {
            try
            {
                var result = _dbContext.Tasks.Where(x => x.ReferenceId == FileId);

                if (result is not null)
                {
                    return result.FirstOrDefault();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<bool> ApproveTaskByReferenceId(Guid refernceId, string User, bool IsViceHos)
        {
            bool isSaved = true;
            try
            {
                UserTask userTask = new();

                if (IsViceHos)
                {
                    userTask = _dbContext.Tasks.FirstOrDefault(m => (m.ReferenceId == refernceId) && ((m.SystemGenTypeId == (int)TaskSystemGenTypeEnum.Meeting)) && (m.TaskStatusId != (int)TaskStatusEnum.Done));
                }
                else
                {
                    userTask = _dbContext.Tasks.FirstOrDefault(m => (m.ReferenceId == refernceId) && ((m.SystemGenTypeId == (int)TaskSystemGenTypeEnum.MeetingSendToHos)) && (m.TaskStatusId != (int)TaskStatusEnum.Done));

                }
                if (userTask is not null)
                {
                    userTask.TaskStatusId = (int)TaskStatusEnum.Done;
                    userTask.Url = $"meeting-view/{refernceId}/{true}";
                    userTask.ModifiedBy = User;
                    userTask.ModifiedDate = DateTime.Now;

                    _dbContext.Tasks.Update(userTask);
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    userTask = new UserTask();
                }
            }
            catch (Exception)
            {
                isSaved = false;
            }
            return isSaved;
        }
        public async Task<bool> UpdateUserTaskViewTime(Guid? AssignedTo, Guid? referenceId)
        {
            bool isSaved = true;
            try
            {
                List<UserTaskView> userTaskViewList = new List<UserTaskView>();

                if (referenceId.HasValue)
                {
                    userTaskViewList = await _dbContext.UserTaskViews
                                                       .Where(x => x.ReferenceId == referenceId && x.UserId == AssignedTo)
                                                       .ToListAsync();

                    if (userTaskViewList.Count == 1)
                    {
                        isSaved = await RemoveUserTaskView(userTaskViewList.First());
                    }
                }
            }
            catch (Exception ex)
            {
                isSaved = false;
            }

            return isSaved;
        }
        public async Task<List<TaskListMobileAppVM>> GetAllTasksListForMobileApp(AdvanceSearchTaskMobileAppVM advanceSearchVM)
        {
            try
            {
                string fromDate = advanceSearchVM.FromDate != null ? Convert.ToDateTime(advanceSearchVM.FromDate).ToString("yyyy/MM/dd HH:mm") : null;
                string toDate = advanceSearchVM.ToDate != null ? Convert.ToDateTime(advanceSearchVM.ToDate).ToString("yyyy/MM/dd HH:mm") : null;
                string fromHearingDate = advanceSearchVM.FromHearingDate != null ? Convert.ToDateTime(advanceSearchVM.FromHearingDate).ToString("yyyy/MM/dd HH:mm") : null;
                string toHearingDate = advanceSearchVM.ToHearingDate != null ? Convert.ToDateTime(advanceSearchVM.ToHearingDate).ToString("yyyy/MM/dd HH:mm") : null;
                string userId = advanceSearchVM.UserId != null ? advanceSearchVM.UserId : null;
                string assignedBy = advanceSearchVM.AssignedBy != null ? advanceSearchVM.AssignedBy : null;
                string taskName = advanceSearchVM.Name != null ? advanceSearchVM.Name : null;
                string taskDescription = advanceSearchVM.Description != null ? advanceSearchVM.Description : null;
                int? taskStatusId = advanceSearchVM.TaskStatusId != null ? advanceSearchVM.TaskStatusId : null;
                int startValue = 0; int endValue = 0;
                int startModule = 0; int endModule = 0;
                string procName = "";
                startValue = (int)TaskSystemGenTypeEnum.CaseRequestTransfer;
                endValue = (int)TaskSystemGenTypeEnum.CaseMigratedFromMOJ;
                startModule = (int)SubModuleEnum.CaseRequest;
                endModule = (int)SubModuleEnum.DMSReviewDocument;
                procName = "pTasksListForMobileApplication";
                if (advanceSearchVM.TaskTypeId == (int)MobileTaskTypeEnum.CaseManagement)
                {
                    startValue = (int)TaskSystemGenTypeEnum.CaseRequestTransfer;
                    endValue = (int)TaskSystemGenTypeEnum.CaseMigratedFromMOJ;
                    startModule = (int)SubModuleEnum.CaseRequest;
                    endModule = (int)SubModuleEnum.DMSReviewDocument;
                }
                else if (advanceSearchVM.TaskTypeId == (int)MobileTaskTypeEnum.Consultation)
                {
                    startValue = (int)TaskSystemGenTypeEnum.ConsultationRequestTransfer;
                    endValue = (int)TaskSystemGenTypeEnum.CaseMigratedFromMOJ;
                    startModule = (int)SubModuleEnum.ConsultationRequest;
                    endModule = (int)SubModuleEnum.ConsultationFile;
                }
                else if (advanceSearchVM.TaskTypeId == (int)MobileTaskTypeEnum.Generic)
                {
                    startValue = 0;
                    endValue = 0;
                    startModule = 0;
                    endModule = 0;
                }
                string storedProc = $"exec {procName}  @FromDate = N'{fromDate}', @ToDate = N'{toDate}', @UserId = N'{userId}', " +
                                       $"@FromHearingDate = N'{fromHearingDate}', @ToHearingDate = N'{toHearingDate}'," +
                                    $"@AssignedBy = N'{assignedBy}', @TaskName = N'{taskName}', @TaskDescription = N'{taskDescription}'," +
                                    $"@TaskStatusId=N'{taskStatusId}', @StartSubModule='{startModule}', @EndSubModule='{endModule}', @StartValue='{startValue}', @EndValue='{endValue}', @IsImportant='{advanceSearchVM.IsImpportant}', @GeId='{advanceSearchVM.GovermentEntityId}', @ReferenceNumber='{advanceSearchVM.ReferenceNumber}',@TaskTypeId='{advanceSearchVM.TaskTypeId}'";
                return await _dbContext.TaskListMobileAppVM.FromSqlRaw(storedProc).ToListAsync();

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<dynamic> GetTasksDetailsForMobileApp(string taskId, string cultureType)
        {
            try
            {
                string procName = string.Empty;
                string storedProc = string.Empty;
                var task = await _dbContext.Tasks.FirstOrDefaultAsync(x => x.TaskId == Guid.Parse(taskId));
                if (task != null)
                {
                    if (task.SystemGenTypeId == (int)TaskSystemGenTypeEnum.CaseRequestTransfer || task.SystemGenTypeId == (int)TaskSystemGenTypeEnum.CaseRequestSendCopy ||
                        task.SystemGenTypeId == (int)TaskSystemGenTypeEnum.CreateCaseRequest)
                    {
                        procName = "pMobileAppCaseRequestDetailsById";
                        storedProc = $"exec {procName} @CaseRequestId='{task.ReferenceId}',@CultureType='{cultureType}'";
                        var data = (await _dbContext.CMSCOMSRequestDetailVM.FromSqlRaw(storedProc).ToListAsync()).FirstOrDefault();
                        if (data != null)
                        {
                            var transformedData = await TransformData(data, null);
                            return transformedData;
                        }
                    }
                    else if (task.SystemGenTypeId == (int)TaskSystemGenTypeEnum.ConsultationRequestTransfer || task.SystemGenTypeId == (int)TaskSystemGenTypeEnum.CreateConsultationRequest)
                    {
                        procName = "pMobileAppConsultationRequestDetailsById";
                        storedProc = $"exec {procName} @consultationRequestId='{task.ReferenceId}',@CultureType='{cultureType}'";
                        var data = (await _dbContext.CMSCOMSRequestDetailVM.FromSqlRaw(storedProc).ToListAsync()).FirstOrDefault();
                        if (data != null)
                        {
                            var transformedData = await TransformData(data, null);
                            return transformedData;
                        }
                    }
                    else if (task.SystemGenTypeId == (int)TaskSystemGenTypeEnum.CaseFileAssignToLawyer || task.SystemGenTypeId == (int)TaskSystemGenTypeEnum.CaseFileTransfer ||
                        task.SystemGenTypeId == (int)TaskSystemGenTypeEnum.CaseFileSendCopy || task.SystemGenTypeId == (int)TaskSystemGenTypeEnum.CaseFileAssignToSector || task.SystemGenTypeId == (int)TaskSystemGenTypeEnum.CaseFileRejectTransfer)
                    {
                        procName = "pMobileAppCmsCaseFileDetailsById";
                        storedProc = $"exec {procName} @casefileId='{task.ReferenceId}',@CultureType='{cultureType}'";
                        var data = (await _dbContext.CMSCOMSFileDetailVM.FromSqlRaw(storedProc).ToListAsync()).FirstOrDefault();
                        if (data != null)
                        {
                            var transformedData = await TransformData(data, null);
                            return transformedData;
                        }
                    }
                    else if (task.SystemGenTypeId == (int)TaskSystemGenTypeEnum.ConsultationFileAssignToLawyer || task.SystemGenTypeId == (int)TaskSystemGenTypeEnum.ConsultationRequestAssignToLawyer ||
                        task.SystemGenTypeId == (int)TaskSystemGenTypeEnum.ConsultationFileTransfer || task.SystemGenTypeId == (int)TaskSystemGenTypeEnum.ConsultationFileAssignToSector || task.SystemGenTypeId == (int)TaskSystemGenTypeEnum.ConsultationFileRejectTransfer)
                    {
                        procName = "pMobileAppConsultationFileDetailsById";
                        storedProc = $"exec {procName} @consultationFileId='{task.ReferenceId}',@CultureType='{cultureType}'";
                        var data = (await _dbContext.CMSCOMSFileDetailVM.FromSqlRaw(storedProc).ToListAsync()).FirstOrDefault();
                        if (data != null)
                        {
                            var transformedData = await TransformData(data, null);
                            return transformedData;
                        }
                    }
                    else if (task.SystemGenTypeId == (int)TaskSystemGenTypeEnum.RegisteredCaseAssignToLawyer || task.SystemGenTypeId == (int)TaskSystemGenTypeEnum.CaseMigratedFromMOJ ||
                        task.SystemGenTypeId == (int)TaskSystemGenTypeEnum.RegisteredCaseToMoj)
                    {
                        procName = "pMobileAppCmsRegisteredCaseDetailsById";
                        storedProc = $"exec {procName} @caseId='{task.ReferenceId}',@CultureType='{cultureType}'";
                        var data = (await _dbContext.MOJRegisteredCaseDetailVM.FromSqlRaw(storedProc).ToListAsync()).FirstOrDefault();
                        if (data != null)
                        {
                            var transformedData = await TransformData(data, null);
                            return transformedData;
                        }
                    }
                    else if (task.SystemGenTypeId == (int)TaskSystemGenTypeEnum.CaseRequestDraftDocumentReview || task.SystemGenTypeId == (int)TaskSystemGenTypeEnum.DraftDocumentReview ||
                        task.SystemGenTypeId == (int)TaskSystemGenTypeEnum.ConsultationRequestDraftDocumentReview || task.SystemGenTypeId == (int)TaskSystemGenTypeEnum.ConsultationDraftDocumentReview)
                    {
                        var taskUrl = task.Url.Trim().Split('/').ToArray();
                        if (taskUrl.Length > 2)
                        {
                            string draftId = taskUrl[1].ToString();
                            string versionId = taskUrl[2].ToString();
                            procName = "pMobileAppDraftDocumentDetailsById";
                            storedProc = $"exec {procName} @draftDocumentId='{draftId}',@versionId='{versionId}',@CultureType='{cultureType}'";
                            var data = (await _dbContext.MobileAppDraftDocumentDetailVM.FromSqlRaw(storedProc).ToListAsync()).FirstOrDefault();
                            if (data != null)
                            {
                                var transformedData = await TransformData(data, null);
                                return transformedData;
                            }
                        }
                    }
                    else if (task.SystemGenTypeId == (int)TaskSystemGenTypeEnum.Meeting || task.SystemGenTypeId == (int)TaskSystemGenTypeEnum.MeetingSendToHos)
                    {
                        procName = "pMobileAppMeetingsDetailsById";
                        storedProc = $"exec {procName} @MeetingId='{task.ReferenceId}',@CultureType='{cultureType}'";
                        var data = (await _dbContext.MobileAppMeetingDetailVM.FromSqlRaw(storedProc).ToListAsync()).FirstOrDefault();
                        if (data != null)
                        {
                            var transformedData = await TransformData(data, null);
                            return transformedData;
                        }
                    }
                    else if (task.SystemGenTypeId == (int)TaskSystemGenTypeEnum.WithdrawCaseRequest || task.SystemGenTypeId == (int)TaskSystemGenTypeEnum.WithdrawConsultationRequest)
                    {
                        procName = "pMobileAppWithdrawRequestDetailsById";
                        storedProc = $"exec {procName} @WithdrawRequestId='{task.ReferenceId}',@CultureType='{cultureType}'";
                        var data = (await _dbContext.WithdrawCMSCOMSRequestDetailVM.FromSqlRaw(storedProc).ToListAsync()).FirstOrDefault();
                        if (data != null)
                        {
                            var transformedData = await TransformData(data, null);
                            return transformedData;
                        }
                    }
                    else if (task.SystemGenTypeId == (int)TaskSystemGenTypeEnum.ExecutionRequest)
                    {
                        procName = "pMobileAppExecutionRequestDetailsById";
                        storedProc = $"exec {procName} @executionRequestId='{task.ReferenceId}',@CultureType='{cultureType}'";
                        var data = (await _dbContext.MobileAppExecutionDetailVM.FromSqlRaw(storedProc).ToListAsync()).FirstOrDefault();
                        if (data != null)
                        {
                            var transformedData = await TransformData(data, null);
                            return transformedData;
                        }
                    }
                    else if (task.SystemGenTypeId == (int)TaskSystemGenTypeEnum.Communication)
                    {
                        if (!string.IsNullOrEmpty(task.Url))
                        {
                            var taskUrl = task.Url.Trim().Split('/').ToArray();
                            if (taskUrl[0].Equals("request-need-more-detail"))
                            {
                                string ReferenceId = taskUrl[1].ToString();
                                string CommunicationId = taskUrl[2].ToString();
                                string SubModuleId = taskUrl[3].ToString();
                                string ByActivity = taskUrl[4].ToString();
                                procName = "pMobileAppCommunicationDetailsbyId";
                                storedProc = $"exec {procName} @ReferenceId='{ReferenceId}',@CommunicationType='{ByActivity}',@CommunicationId='{CommunicationId}',@SubModuleId='{SubModuleId}',@CultureType='{cultureType}'";
                                var data = (await _dbContext.CMSCOMSCommunicationDetailVM.FromSqlRaw(storedProc).ToListAsync()).FirstOrDefault();
                                if (data != null)
                                {
                                    var transformedData = await TransformData(data, null);
                                    return transformedData;
                                }
                            }
                            else if (taskUrl[0].Equals("meeting-view") || taskUrl[0].Equals("request-meeting-detail") || taskUrl[0].Equals("meeting-detail-request"))
                            {
                                string communicationId = taskUrl[1].ToString();
                                procName = "pMobileAppCommMeetingDetailsById";
                                storedProc = $"exec {procName} @communicationId='{communicationId}',@CultureType='{cultureType}'";
                                var data = (await _dbContext.MobileAppMeetingDetailVM.FromSqlRaw(storedProc).ToListAsync()).FirstOrDefault();
                                if (data != null)
                                {
                                    var transformedData = await TransformData(data, null);
                                    return transformedData;
                                }
                            }
                        }
                    }
                    else if (task.SystemGenTypeId == (int)TaskSystemGenTypeEnum.CaseMergeRequest)
                    {
                        MobileAppMergeCaseRequestDetailVM mobileAppMergeCaseRequestDetailVM = new MobileAppMergeCaseRequestDetailVM();
                        if (!string.IsNullOrEmpty(task.Url))
                        {
                            var taskUrl = task.Url.Trim().Split('/').ToArray();
                            if (taskUrl[0].Equals("mergerequest-view"))
                            {
                                string mergedId = taskUrl[1].ToString();
                                procName = "pMobileAppMergeRequestDetailsById";
                                storedProc = $"exec {procName} @mergedId='{mergedId}'";
                                var result = await _dbContext.MobileAppMergeCaseRequestDetailVM.FromSqlRaw(storedProc).ToListAsync();
                                mobileAppMergeCaseRequestDetailVM = result.FirstOrDefault();
                                string StoredProc = $"exec pMergedCasesByMergeRequestId @mergeRequestId ='{mergedId}' ";
                                var mergedCases = await _dbContext.CmsRegisteredCaseVMs.FromSqlRaw(StoredProc).ToListAsync();
                                mobileAppMergeCaseRequestDetailVM.MergedCANs = mobileAppMergeCaseRequestDetailVM.PrimaryCANNumber;
                                for (int i = 0; i < mergedCases?.Count(); i++)
                                {
                                    var seperator = i + 1 == mergedCases?.Count() ? "" : ", ";
                                    mobileAppMergeCaseRequestDetailVM.MergedCANs += mergedCases[i].CANNumber + seperator;
                                }
                            }
                        }
                        if (mobileAppMergeCaseRequestDetailVM != null)
                        {
                            var transformedData = await TransformData(mobileAppMergeCaseRequestDetailVM, null);
                            return transformedData;
                        }
                    }
                    else if (task.SystemGenTypeId == (int)TaskSystemGenTypeEnum.DMSReviewDocument)
                    {
                        if (!string.IsNullOrEmpty(task.Url))
                        {
                            var taskUrl = task.Url.Trim().Split('/').ToArray();
                            if (taskUrl[0].Equals("document-view"))
                            {
                                string documentId = taskUrl[1].ToString();
                                string versionId = taskUrl[2].ToString();
                                procName = "pMobileAppDMSDocumentDetailsById";
                                storedProc = $"exec {procName} @DocumentId='{documentId}',@VersionId='{versionId}'";
                                var data = (await _dmsDbContext.MobileAppDMSDocumentDetailVM.FromSqlRaw(storedProc).ToListAsync()).FirstOrDefault();
                                if (data != null)
                                {
                                    var transformedData = await TransformData(data, null);
                                    return transformedData;
                                }
                            }
                        }
                    }
                    else if (task.SystemGenTypeId == (int)TaskSystemGenTypeEnum.Hearing)
                    {
                        procName = "pMobileAppHearingDetailsById";
                        storedProc = $"exec {procName} @hearingId='{task.ReferenceId}',@CultureType='{cultureType}'";
                        var data = (await _dbContext.MobileAppHearingDetailVM.FromSqlRaw(storedProc).ToListAsync()).FirstOrDefault();
                        if (data != null)
                        {
                            var transformedData = await TransformData(data, null);
                            return transformedData;
                        }
                    }
                    else if (task.SystemGenTypeId == (int)TaskSystemGenTypeEnum.CaseFileTranferRequestToSector)
                    {
                        procName = "pMobileAppCaseFileTranferRequestDetailById";
                        string StoredProc = $"exec {procName} @ReferenceId = N'{task.ReferenceId}',@CultureType='{cultureType}'";
                        var data = (await _dbContext.MobileAppCaseFileTransferRequestVM.FromSqlRaw(StoredProc).ToListAsync()).FirstOrDefault();
                        if (data != null)
                        {
                            var transformedData = await TransformData(data, null);
                            return transformedData;
                        }
                    }
                    else if (task.SystemGenTypeId == (int)TaskSystemGenTypeEnum.RegisteredCaseTranferRequest)
                    {
                        procName = "pMobileAppCaseRegisteredTranferRequestDetailbyId";
                        string StoredProc = $"exec {procName} @ReferenceId = N'{task.ReferenceId}',@CultureType='{cultureType}'";
                        var data = (await _dbContext.MobileAppCaseTransferRequestVM.FromSqlRaw(StoredProc).ToListAsync()).FirstOrDefault();
                        if (data != null)
                        {
                            var transformedData = await TransformData(data, null);
                            return transformedData;
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> RemoveUserTaskView(UserTaskView userTaskView)
        {
            try
            {
                _dbContext.UserTaskViews.Remove(userTaskView);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> AddOrUpdateUserTaskViewTime(Guid userId, Guid? referenceId)
        {
            try
            {
                var taskType = await _dbContext.Tasks.Where(x => x.ReferenceId == referenceId).Select(x => x.SystemGenTypeId).FirstOrDefaultAsync();
                switch (taskType)
                {
                    case (int)TaskSystemGenTypeEnum.DraftDocumentReview:
                        referenceId = await _dbContext.CmsDraftedTemplate.Where(x => x.Id == referenceId).Select(x => x.ReferenceId).FirstOrDefaultAsync();
                        break;

                    case (int)TaskSystemGenTypeEnum.Meeting:
                        referenceId = await _dbContext.Meetings.Where(x => x.MeetingId == referenceId).Select(x => x.ReferenceGuid).FirstOrDefaultAsync();
                        break;

                    case (int)TaskSystemGenTypeEnum.WithdrawCaseRequest:
                        referenceId = await _dbContext.WithdrawRequests.Where(x => x.Id == referenceId).Select(x => x.CaseRequestId).FirstOrDefaultAsync();
                        break;
                }
                if (referenceId != null)
                {
                    var existingUserTaskView = await _dbContext.UserTaskViews.FirstOrDefaultAsync(x => x.UserId == userId && x.ReferenceId == referenceId);

                    if (existingUserTaskView != null)
                    {
                        existingUserTaskView.LastViewTime = DateTime.Now;
                        _dbContext.UserTaskViews.Update(existingUserTaskView);
                    }
                    else
                    {
                        var newUserTaskView = new UserTaskView
                        {
                            UserId = userId,
                            ReferenceId = referenceId,
                            LastViewTime = DateTime.Now
                        };
                        await _dbContext.UserTaskViews.AddAsync(newUserTaskView);
                    }

                    await _dbContext.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #region Save Task List
        public async Task<bool> AddTaskList(List<SaveTaskVM> taskList)
        {
            bool isSaved = false;
            using var scope = _serviceScopeFactory.CreateScope();
            var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

            using (_DbContext)
            {
                using (var transaction = _DbContext.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var task in taskList)
                        {
                            isSaved = GenerateTaskNumber(task.Task, _DbContext);
                            isSaved = await SaveTask(task.Task, false, _DbContext);
                            if (task.TaskActions.Any())
                                isSaved = await SaveTaskActions(task, _DbContext);
                        }
                        if (isSaved)
                            transaction.Commit();

                    }
                    catch (Exception)
                    {
                        isSaved = false;
                        transaction.Rollback();
                    }
                }
            }

            return isSaved;
        }
        #endregion
        #region Get Task List For OSS
        public async Task<List<FatwaOssTaskVM>> GetTasksListForOSS(AdvanceSearchTaskVM advanceSearchVM)
        {
            try
            {
                string fromDate = advanceSearchVM.FromDate != null ? Convert.ToDateTime(advanceSearchVM.FromDate).ToString("yyyy/MM/dd HH:mm") : null;
                string toDate = advanceSearchVM.ToDate != null ? Convert.ToDateTime(advanceSearchVM.ToDate).ToString("yyyy/MM/dd HH:mm") : null;
                string userId = advanceSearchVM.UserId != null ? advanceSearchVM.UserId : null;
                string assignedBy = advanceSearchVM.AssignedBy != null ? advanceSearchVM.AssignedBy : null;
                string taskName = advanceSearchVM.Name != null ? advanceSearchVM.Name : null;
                string taskDescription = advanceSearchVM.Description != null ? advanceSearchVM.Description : null;
                int? taskStatusId = advanceSearchVM.TaskStatusId != null ? advanceSearchVM.TaskStatusId : null;

                string storedProc = $"exec pOSS_GetTaskListForOSS  @FromDate = N'{fromDate}'," +
                                                                   $"@UserId = N'{userId}', " +
                                                                   $"@ToDate = N'{toDate}', " +
                                                                   $"@TaskName = N'{taskName}'," +
                                                                   $"@AssignedBy = N'{assignedBy}'," +
                                                                   $" @TaskDescription = N'{taskDescription}'," +
                                                                   $"@TaskStatusId=N'{taskStatusId}' ";
                return await _dbContext.FatwaOssTaskVMs.FromSqlRaw(storedProc).ToListAsync();
            }
            catch
            {
                throw;
            }
        }

        #endregion
        #region Get Task Detail By Id For OSS
        public async Task<FatwaOssTaskDetailVM> GetTaskDetailByIdForOSS(Guid taskId)
        {
            try
            {
                string storedProc = $"exec pOSS_TaskDetailForOSS @TaskId = N'{taskId}'";
                var result = await _dbContext.FatwaOssTaskDetailVMs.FromSqlRaw(storedProc).ToListAsync();
                if (result is not null)
                {
                    return result.FirstOrDefault();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #region Get System Generated Task List FOr OSS
        public async Task<List<FatwaOssTaskVM>> GetOSSSystemGeneratedTasks(AdvanceSearchTaskVM advanceSearchVM)
        {
            try
            {
                string fromDate = advanceSearchVM.FromDate != null ? Convert.ToDateTime(advanceSearchVM.FromDate).ToString("yyyy/MM/dd HH:mm") : null;
                string toDate = advanceSearchVM.ToDate != null ? Convert.ToDateTime(advanceSearchVM.ToDate).ToString("yyyy/MM/dd HH:mm") : null;
                string fromHearingDate = advanceSearchVM.FromHearingDate != null ? Convert.ToDateTime(advanceSearchVM.FromHearingDate).ToString("yyyy/MM/dd HH:mm") : null;
                string toHearingDate = advanceSearchVM.ToHearingDate != null ? Convert.ToDateTime(advanceSearchVM.ToHearingDate).ToString("yyyy/MM/dd HH:mm") : null;
                string userId = advanceSearchVM.UserId != null ? advanceSearchVM.UserId : null;
                string assignedBy = advanceSearchVM.AssignedBy != null ? advanceSearchVM.AssignedBy : null;
                string taskName = advanceSearchVM.Name != null ? advanceSearchVM.Name : null;
                string taskDescription = advanceSearchVM.Description != null ? advanceSearchVM.Description : null;
                int? taskStatusId = advanceSearchVM.TaskStatusId != null ? advanceSearchVM.TaskStatusId : null;
                int startValue = 0; int endValue = 0;
                int startModule = 0; int endModule = 0;
                string procName = "";

                startValue = (int)TaskSystemGenTypeEnum.CaseRequestTransfer;
                endValue = (int)TaskSystemGenTypeEnum.InventoryManagement;
                startModule = (int)SubModuleEnum.OrganizingCommittee;
                endModule = (int)SubModuleEnum.LeaveAndAttendance;

                procName = "pOSS_GetSystemGeneratedTaskListForOSS";

                string storedProc = $"exec {procName} " +
                                    $"@ToDate = N'{toDate}'," +
                                    $"@UserId = N'{userId}'," +
                                    $"@EndValue='{endValue}'," +
                                    $"@FromDate = N'{fromDate}'," +
                                    $"@TaskName = N'{taskName}'," +
                                    $"@StartValue='{startValue}'," +
                                    $"@EndSubModule='{endModule}'," +
                                    $"@AssignedBy = N'{assignedBy}'," +
                                    $"@TaskStatusId=N'{taskStatusId}'," +
                                    $"@StartSubModule='{startModule}'," +
                                    $"@ToHearingDate = N'{toHearingDate}'," +
                                    $"@FromHearingDate = N'{fromHearingDate}'," +
                                    $"@TaskDescription = N'{taskDescription}'," +
                                    $"@GeId='{advanceSearchVM.GovermentEntityId}'," +
                                    $"@IsImportant='{advanceSearchVM.IsImpportant}'," +
                                    $"@ReferenceNumber='{advanceSearchVM.ReferenceNumber}'";
                return await _dbContext.FatwaOssTaskVMs.FromSqlRaw(storedProc).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
        public async Task<List<TaskResponseStatusVM>> GetTaskResponseByTasktId(Guid Id)
        {
            try
            {
                string storedProc = $"exec pGetTaskResponse @TaskId='{Id}'";
                var result = await _dbContext.TaskResponseStatusVMs.FromSqlRaw(storedProc).ToListAsync();
                return result;

            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task UpdateTaskForSignature(DsSigningRequestTaskLog taskForDS)
        {
            try
            {
                _dmsDbContext.DsSigningRequestTaskLogs.Update(taskForDS);
                await _dmsDbContext.SaveChangesAsync();
                if (taskForDS.StatusId != (int)SigningTaskStatusEnum.Failed)
                {
                    var result = await _dmsDbContext.UploadedDocuments.Where(x => x.UploadedDocumentId == taskForDS.DocumentId).FirstOrDefaultAsync();
                    if (result is not null)
                    {
                        result.StatusId = taskForDS.StatusId;
                        _dmsDbContext.UploadedDocuments.Update(result);
                        await _dmsDbContext.SaveChangesAsync();
                    }
                    // For Notification
                    taskForDS.NotificationParameter.Type = await _dmsDbContext.AttachmentType.Where(x => x.AttachmentTypeId == result.AttachmentTypeId).Select(x => x.Type_En + "/" + x.Type_Ar).FirstOrDefaultAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<DsSigningRequestTaskLogVM>> GetAllTasksForSignature(int DocumentId)
        {
            try
            {
                string StoredProc = $"exec pDSDocumentTaskStatus @DocumentId = '{DocumentId}'";
                var result = await _dmsDbContext.DsSigningRequestTaskLogVM.FromSqlRaw(StoredProc).AsNoTracking().ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #region Mobile Application Data Transform
        public async Task<object> TransformData(object data, string? referenceId)
        {
            var dataType = data.GetType();
            var labels = dataType.GetProperties()
            .Select(p => new
            {
                Label = p.GetCustomAttributes(typeof(DisplayNameAttribute), true)
                .Cast<DisplayNameAttribute>()
                .FirstOrDefault()?.DisplayName ?? p.Name,
                Value = p.GetValue(data)?.ToString(),
                Type = GetPropertyType(p.PropertyType)
            }).Cast<object>().ToList();
            var propertiesToSplit = new[] { "Priority", "Is_Confidential", "Court_level" };
            var tags = new List<object>();
            foreach (var property in propertiesToSplit)
            {
                var tagsList = labels.Where(x => string.Equals(((dynamic)x).Label, property, StringComparison.OrdinalIgnoreCase)).ToList();
                if (tagsList.Any())
                {
                    labels.Remove(tagsList.FirstOrDefault());
                    tags.AddRange(tagsList);
                }
            }
            if (!string.IsNullOrEmpty(referenceId))
            {
                var attachments = await GetUploadDocumentsList(Guid.Parse(referenceId));
                return new { labels, tags, attachments };
            }
            return new { labels, tags };
        }
        public static string GetPropertyType(Type propertyType)
        {
            if (propertyType == typeof(DateTime?) || propertyType == typeof(DateTime))
                return "datetime";
            else if (propertyType == typeof(decimal?) || propertyType == typeof(decimal) ||
                     propertyType == typeof(double?) || propertyType == typeof(double) ||
                     propertyType == typeof(float?) || propertyType == typeof(float))
                return "amount";
            else if (propertyType == typeof(bool?) || propertyType == typeof(bool))
                return "boolean";
            else
                return propertyType.Name.ToLower();
        }
        public async Task<List<MobileAppUploadDocumentsVM>> GetUploadDocumentsList(Guid? ReferenceId)
        {
            try
            {
                string StoredProcDms = $"exec pMobileAppUploadDocumentsList @ReferenceId = N'{ReferenceId}'";
                var attachments = await _dmsDbContext.MobileAppUploadDocumentsVM.FromSqlRaw(StoredProcDms).ToListAsync();
                if (attachments.Any())
                {
                    foreach (var item in attachments)
                    {
                        var physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + item.FileUrl).Replace(@"\\", @"\");
#if !DEBUG
						physicalPath = physicalPath.Replace("\\wwwroot\\Attachments\\", "\\");
#endif
                        item.FileUrl = physicalPath;
                    }
                    return attachments;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        public async Task CreateTaskForSignature(DsSigningRequestTaskLog taskForDS)
        {
            try
            {
                await _dmsDbContext.DsSigningRequestTaskLogs.AddAsync(taskForDS);
                await _dmsDbContext.SaveChangesAsync();

                var result = await _dmsDbContext.UploadedDocuments.Where(x => x.UploadedDocumentId == taskForDS.DocumentId).FirstOrDefaultAsync();
                if (result is not null)
                {
                    result.StatusId = taskForDS.StatusId;
                    _dmsDbContext.UploadedDocuments.Update(result);
                    await _dmsDbContext.SaveChangesAsync();
                }
                // For Notification
                taskForDS.NotificationParameter.Type = await _dmsDbContext.AttachmentType.Where(x => x.AttachmentTypeId == result.AttachmentTypeId).Select(x => x.Type_En + "/" + x.Type_Ar).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<DsSigningRequestTaskLog> GetTaskForSignature(Guid SigningTaskId)
        {
            try
            {
                var result = await _dmsDbContext.DsSigningRequestTaskLogs.Where(x => x.SigningTaskId == SigningTaskId).FirstOrDefaultAsync();
                if (result is not null)
                {
                    var Name = await _dbContext.UserPersonalInformation.Where(x => x.UserId == result.SenderId).FirstOrDefaultAsync();
                    result.SenderName_En = string.Join(" ", new[] { Name.FirstName_En, Name.SecondName_En, Name.LastName_En }.Where(n => !string.IsNullOrEmpty(n)));
                    result.SenderName_Ar = string.Join(" ", new[] { Name.FirstName_Ar, Name.SecondName_Ar, Name.LastName_Ar }.Where(n => !string.IsNullOrEmpty(n)));
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

}

