using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.Meet;
using FATWA_DOMAIN.Models.TaskModels;
using FATWA_DOMAIN.Models.ViewModel.TaskVMs;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.Dms;
using FATWA_DOMAIN.Models.ViewModel.DmsVMs;

namespace FATWA_DOMAIN.Interfaces.Tasks
{
    public interface ITask
	{
		#region Get

		Task<List<TaskVM>> GetTasksList(AdvanceSearchTaskVM advanceSearchVM);
		Task<List<TaskVM>> GetCmsTasksList(AdvanceSearchTaskVM advanceSearchVM);
		Task<List<TaskVM>> GetAllCmsTasks(AdvanceSearchTaskVM advanceSearchVM);
		Task<int> GetCountCmsTasksList(AdvanceSearchTaskVM advanceSearchVM);
        Task<int> GetCountComsTasksList(AdvanceSearchTaskVM advanceSearchVM);
        Task<List<TaskListMobileAppVM>> GetAllTasksListForMobileApp(AdvanceSearchTaskMobileAppVM advanceSearchVM);
        Task<dynamic> GetTasksDetailsForMobileApp(string taskId, string cultureType);
        Task<SaveTaskVM> GetTaskById(Guid taskId);
		Task<TaskDetailVM> GetTaskDetailById(Guid taskId);
		Task<TaskDetailVM> GetTaskDetailByReferenceAndUserId(Guid referenceId, string userId);
		Task<TaskDashboardVM> GetTaskDashboard(string item, string UserName);
		Task<List<DraftListVM>> GetDraftList(AdvanceSearchDraftVM advanceSearchVM);
        Task<int> GetMaxTaskNumber();
        Task<List<TaskResponseStatus>> GetTaskResponseStatus();
        Task<List<TaskAction>> GetTaskActionsByTaskId(Guid taskId);
        Task<List<TaskVM>> GetComsTasksList(AdvanceSearchTaskVM advanceSearchVM);
        Task<List<TaskVM>> GetAllComsTasks(AdvanceSearchTaskVM advanceSearchVM);
        Task<UserTask> GetTaskDetailByReferenceAndAssignedToId(Guid referenceId, string userId);
        Task<List<UserTask>> GetTaskListByFileId(Guid referenceId);
        Task<List<TaskVM>> GetAllCMSComsTasks(AdvanceSearchTaskVM advanceSearchVM);
        Task<UserTask> GetTaskDetailBySystemGeneratedId(Guid referenceId, string userId , int SystemGeneratedTaskId);
		Task<List<FatwaOssTaskVM>> GetTasksListForOSS(AdvanceSearchTaskVM advanceSearchVM);
        Task<FatwaOssTaskDetailVM> GetTaskDetailByIdForOSS(Guid taskId);
        Task<List<FatwaOssTaskVM>> GetOSSSystemGeneratedTasks(AdvanceSearchTaskVM advanceSearchVM);

        #endregion

        #region Save

        Task<bool> AddTask(SaveTaskVM task, string action, string entityName, string entityId);
        Task<bool> EditTask(SaveTaskVM task, string action, string entityName, string entityId);
		Task<bool> SaveToDoList(TaskDashboardVM item);
		Task<bool> SaveTaskResponseDecision(TaskResponseVM task, bool isEdit);
		Task<bool> DecisionTask(TaskDetailVM task);
		Task<bool> DecisionTaskByStatusAndRefId(TaskDetailVM task);
		Task<bool> SaveCaseAssignment(TaskDetailVM task);
		Task<bool> SaveConsultationAssignment(TaskDetailVM task);
        Task<bool> RemoveAllTempCaseAssignement(TaskDetailVM task);
        Task<bool> UpdateTaskStatus(Guid TaskId , int StatusId);
        Task AddTaskAndNotificationForHOSAndViceHOSOfSector(SaveTaskVM? task, FATWA_DOMAIN.Models.Notifications.Notification? notification, int sectorId, bool verifyViceHOSResponsibility, bool includeHOS, int chamberNumberId);

        #endregion

        //#region Delete 
        //Task<bool> SoftDeleteDraft(DraftListVM draft);

        //#endregion
        #region Reject Reason
        Task<bool> RejectReason(RejectReason reject);
        #endregion
        Task CompleteAllPendingTasks(Guid RequestId , string User);
        Task<bool> CompleteAssignTask(Guid FileId);
        Task<UserTask> GetTaskDetailByFileId(Guid FileId);
        Task<bool> ApproveTaskByReferenceId(Guid refernceId, string User, bool IsViceHos);
        Task<bool> AddOrUpdateUserTaskViewTime(Guid userId,Guid? referenceId);

        Task<List<TaskEntityHistoryVM>> GetTaskEntityHistoryByReferenceIdAndSubmodule(Guid referenceId, int submoduleId);
        Task<bool> AddTaskList(List<SaveTaskVM> taskList);

        Task CreateTaskForSignature(DsSigningRequestTaskLog taskForDS);
        Task<DsSigningRequestTaskLog> GetTaskForSignature(Guid SigningTaskId);
        Task UpdateTaskForSignature(DsSigningRequestTaskLog taskForDS);
        Task<List<DsSigningRequestTaskLogVM>> GetAllTasksForSignature(int DocumentId);
        Task<object> TransformData(object data, string? referenceId);
        Task<List<TaskResponseStatusVM>> GetTaskResponseByTasktId(Guid Id);
    }
}
