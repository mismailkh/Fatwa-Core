using FATWA_DOMAIN.Interfaces.BugReporting;
using FATWA_DOMAIN.Models.BugReporting;
using FATWA_DOMAIN.Models.ViewModel.BugReportingVMs;
using FATWA_INFRASTRUCTURE.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using static FATWA_DOMAIN.Enums.BugReporting.BugReportingEnum;
//< History Author = 'Muhammad Zaeem' Date = '2024-04-28' Version = "1.0" Branch = "master" Bug Reporting Repo</History>

namespace FATWA_INFRASTRUCTURE.Repository.BugReporting
{
    public class BugReportingRepository : IBugReporting
    {
        #region Variable Declaration
        private readonly DatabaseContext _dbContext;
        private readonly DmsDbContext _dmsDbContext;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly AccountRepository _accountRepository;
        #endregion

        #region Constructor
        public BugReportingRepository(DatabaseContext dbContext, IServiceScopeFactory serviceScopeFactory, DmsDbContext dmsDbContext)
        {
            _dbContext = dbContext;
            _serviceScopeFactory = serviceScopeFactory;
            using var scope = _serviceScopeFactory.CreateScope();
            _dmsDbContext = dmsDbContext;
            _accountRepository = scope.ServiceProvider.GetRequiredService<AccountRepository>();

        }
        #endregion

        #region Get LookUps Data
        public async Task<List<BugApplication>> GetAllApplications()
        {
            try
            {
                var result = await _dbContext.BugApplicatoins.OrderBy(a => a.Id).ToListAsync();
                if (result != null)
                {
                    result = Thread.CurrentThread.CurrentUICulture.Name == "en-US" ? result.OrderBy(x => x.Name_Ar).ToList() : result.OrderBy(x => x.Name_Ar).ToList();
                    return result;
                }
                else
                {
                    return new List<BugApplication>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<BugModule>> GetBugModules()
        {
            try
            {
                var result = await _dbContext.BugModules.OrderBy(a => a.Id).ToListAsync();
                if (result != null)
                {
                    result = Thread.CurrentThread.CurrentUICulture.Name == "en-US" ? result.OrderBy(x => x.Name_En).ToList() : result.OrderBy(x => x.Name_Ar).ToList();
                    return result;
                }
                else
                {
                    return new List<BugModule>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<BugIssueType>> GetIssueTypes()
        {
            try
            {
                var result = await _dbContext.BugIssueTypes.OrderBy(a => a.Id).Where(x => x.IsDeleted != true).ToListAsync();
                if (result != null)
                {
                    result = Thread.CurrentThread.CurrentUICulture.Name == "en-US" ? result.OrderBy(x => x.Type_En).ToList() : result.OrderBy(x => x.Type_Ar).ToList();
                    return result;
                }
                else
                {
                    return new List<BugIssueType>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<BugStatus>> GetBugStatuses()
        {
            try
            {
                var result = await _dbContext.BugStatuses.OrderBy(a => a.Id).ToListAsync();
                if (result != null)
                {
                    result = Thread.CurrentThread.CurrentUICulture.Name == "en-US" ? result.OrderBy(x => x.Value_En).ToList() : result.OrderBy(x => x.Value_Ar).ToList();
                    return result;
                }
                else
                {
                    return new List<BugStatus>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<BugSeverity>> GetSeverities()
        {
            try
            {
                var result = await _dbContext.BugSeverities.OrderBy(a => a.Id).ToListAsync();
                if (result != null)
                {
                    result = Thread.CurrentThread.CurrentUICulture.Name == "en-US" ? result.OrderBy(x => x.Value_En).ToList() : result.OrderBy(x => x.Value_Ar).ToList();
                    return result;
                }
                else
                {
                    return new List<BugSeverity>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get Auto Generated BugTicketId
        public async Task<BugTicket> GetAutoGeneratedId()
        {
            try
            {
                BugTicket bugTicket = new BugTicket();
                if (_dbContext.BugTickets.Any())
                {
                    var shortNumber = await _dbContext.BugTickets.Select(x => x.ShortNumber).MaxAsync() + 1;
                    bugTicket.TicketId = "TIFT-" + (shortNumber).ToString().PadLeft(6, '0');
                    bugTicket.ShortNumber = shortNumber;
                }
                else
                {
                    bugTicket.TicketId = "TIFT-000001";
                    bugTicket.ShortNumber = 1;
                }
                return bugTicket;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        #endregion

        #region Create Bug Ticket
        public async Task CreateBugTicket(BugTicket ticket)
        {
            try
            {
                using (_dbContext)
                {
                    using (var transaction = _dbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            _dbContext.BugTickets.Add(ticket);
                            await _dbContext.SaveChangesAsync();
                            transaction.Commit();
                            await SaveBugStatusHistory(null, ticket.Id, ticket.StatusId, ticket.StatusId == (int)BugStatusEnum.Draft ? (int)TicketEvenEnum.TicketDrafted : (int)TicketEvenEnum.TicketRaised, ticket.CreatedBy, "");
                            ticket.NotificationParameter.ReferenceNumber = ticket.TicketId;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw new Exception(ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Update Bug Ticket
        public async Task UpdateBugTicket(BugTicket ticket)
        {
            try
            {
                using (_dbContext)
                {
                    using (var transaction = _dbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            _dbContext.BugTickets.Update(ticket);
                            await _dbContext.SaveChangesAsync();
                            if (ticket != null && ticket.StatusId != (int)BugStatusEnum.Draft)
                            {
                                await SaveBugStatusHistory(null, ticket.Id, ticket.StatusId, (int)TicketEvenEnum.TicketRaised, ticket.ModifiedBy, "");
                            }
                            transaction.Commit();

                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        #endregion

        #region Get Bug Ticket By Id 
        public async Task<BugTicket> GetBugTicketById(Guid TicketId)
        {
            try
            {
                var result = await _dbContext.BugTickets.Where(x => x.Id == TicketId).FirstOrDefaultAsync();
                if (result != null)
                {
                    return result;
                }
                else
                {
                    return new BugTicket();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }

        }
        #endregion

        #region Get Bug Ticket Detail By Id
        public async Task<TicketDetailVM> GetBugTicketDetail(Guid id)
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var dbContextScope = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                using (dbContextScope)
                {
                    string storedProc = $"exec pGetTicketDetail @TicketId ='{id}'";
                    var result = await dbContextScope.TicketDetailVM.FromSqlRaw(storedProc).ToListAsync();
                    if (result.Count() > 0)
                    {
                        return result.FirstOrDefault();
                    }
                    else
                    {
                        return new TicketDetailVM();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Get All Tickets List
        public async Task<List<TicketListVM>> GetTickets(AdvanceSearchTicketListVM advanceSearchTicketList)
        {
            try
            {
                string fromDate = advanceSearchTicketList.CreatedDateFrom != null ? Convert.ToDateTime(advanceSearchTicketList.CreatedDateFrom).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                string toDate = advanceSearchTicketList.CreatedDateTo != null ? Convert.ToDateTime(advanceSearchTicketList.CreatedDateTo).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                string StoredProc = $"exec pGetTicketList @TicketId  ='{advanceSearchTicketList.TicketId}' , @ApplicationId='{advanceSearchTicketList.ApplicationId}' " +
                    $", @ModuleId='{advanceSearchTicketList.ModuleId}', @StatusId='{advanceSearchTicketList.StatusId}',@Subject=N'{advanceSearchTicketList.Subject}' " +
                    $", @CreatedDateFrom='{fromDate}' , @CreatedDateTo='{toDate}', @UserId='{advanceSearchTicketList.UserId}', @IssueTypeId='{advanceSearchTicketList.IssueTypeId}'" +
                    $", @PriorityId='{advanceSearchTicketList.PriorityId}',@SeverityId='{advanceSearchTicketList.SeverityId}'" +
                    $",@PageNumber ='{advanceSearchTicketList.PageNumber}',@PageSize ='{advanceSearchTicketList.PageSize}'";
                return await _dbContext.TicketListVM.FromSqlRaw(StoredProc).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Get Bug Ticket Status History
        public async Task<List<TicketStatusHistoryVM>> GetTicketStatusHistory(Guid id)
        {
            try
            {
                var StoredProc = $"exec pGetBugTicketStatusHistory @TicketId='{id}'";
                var result = await _dbContext.BugTicketStatusHistoriesVM.FromSqlRaw(StoredProc).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Save Bug Status History
        public async Task SaveBugStatusHistory(Guid? HistoryId, Guid ReferenceId, int? StatusId, int? EventId, string UserName, string? Remarks = "")
        {
            try
            {
                BugTicketStatusHistory statusHistory = new BugTicketStatusHistory();
                statusHistory.HistoryId = HistoryId != null ? (Guid)HistoryId : Guid.NewGuid();
                statusHistory.ReferenceId = ReferenceId;
                statusHistory.StatusId = StatusId;
                statusHistory.CreatedDate = DateTime.Now;
                statusHistory.CreatedBy = UserName;
                statusHistory.Remarks = Remarks;
                statusHistory.EventId = EventId;
                _dbContext.BugTicketStatusHistories.Add(statusHistory);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get Auto Generated Bug Number
        public async Task<ReportedBug> GetAutoGeneratedBugNumber()
        {
            try
            {
                ReportedBug bugTicket = new ReportedBug();
                if (_dbContext.ReportedBugs.Any())
                {
                    var shortNumber = await _dbContext.ReportedBugs.Select(x => x.ShortNumber).MaxAsync() + 1;
                    bugTicket.PrimaryBugId = "PI-" + (shortNumber).ToString().PadLeft(6, '0');
                    bugTicket.ShortNumber = shortNumber;
                }
                else
                {
                    bugTicket.PrimaryBugId = "PI-000001";
                    bugTicket.ShortNumber = 1;
                }
                return bugTicket;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        #endregion

        #region Get Reported Bug By Id 
        public async Task<ReportedBug> GetReportedBugById(Guid BugId)
        {
            try
            {
                var result = await _dbContext.ReportedBugs.Where(x => x.Id == BugId).FirstOrDefaultAsync();
                if (result != null)
                {
                    return result;
                }
                else
                {
                    return new ReportedBug();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }

        }
        #endregion

        #region Create Bug 
        public async Task CreateBug(ReportedBug bug)
        {
            try
            {
                using (_dbContext)
                {
                    using (var transaction = _dbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            _dbContext.ReportedBugs.Add(bug);
                            await _dbContext.SaveChangesAsync();
                            transaction.Commit();
                            await SaveBugStatusHistory(null, bug.Id, bug.StatusId, (int)TicketEvenEnum.BugReported, bug.CreatedBy, "");

                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw new Exception(ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Update Bug 
        public async Task UpdateBug(ReportedBug bug)
        {
            try
            {
                using (_dbContext)
                {
                    using (var transaction = _dbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            _dbContext.ReportedBugs.Update(bug);
                            await _dbContext.SaveChangesAsync();
                            transaction.Commit();
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        #endregion

        #region Get All Reported Bug List
        public async Task<List<ReportedBugListVM>> GetAllReportedBug(AdvanceSearchBugListVM advanceSearchBugList)
        {
            try
            {
                string fromDate = advanceSearchBugList.CreatedDateFrom != null ? Convert.ToDateTime(advanceSearchBugList.CreatedDateFrom).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                string toDate = advanceSearchBugList.CreatedDateTo != null ? Convert.ToDateTime(advanceSearchBugList.CreatedDateTo).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                string StoredProc = $"exec pGetReportedBugList @PrimaryBugId  ='{advanceSearchBugList.PrimaryBugId}' , @ApplicationId='{advanceSearchBugList.ApplicationId}' , @StatusId='{advanceSearchBugList.StatusId}', @ModuleId='{advanceSearchBugList.ModuleId}', @IssueTypeId='{advanceSearchBugList.IssueTypeId}', @Subject='{advanceSearchBugList.Subject}', @CreatedDateFrom='{fromDate}' , @CreatedDateTo='{toDate}', @PageNumber='{advanceSearchBugList.PageNumber}', @PageSize='{advanceSearchBugList.PageSize}'";
                return await _dbContext.ReportedBugListVMs.FromSqlRaw(StoredProc).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Get Reported Bug Detail By Id
        public async Task<ReportedBugDetailVM> GetReportedBugDetail(Guid Id)
        {
            try
            {
                string storedProc = $"exec pGetReportedBugDetail @BugId ='{Id}'";
                var result = await _dbContext.ReportedBugDetailVMs.FromSqlRaw(storedProc).ToListAsync();
                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Get Crash Report List
        public async Task<List<CrashReportListVM>> GetCrashReportList(AdvanceSearchBugListVM advanceSearchBugList)
        {
            try
            {
                string fromDate = advanceSearchBugList.CreatedDateFrom != null ? Convert.ToDateTime(advanceSearchBugList.CreatedDateFrom).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                string toDate = advanceSearchBugList.CreatedDateTo != null ? Convert.ToDateTime(advanceSearchBugList.CreatedDateTo).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                string StoredProc = $"exec pGetCrashReportList @PrimaryBugId  ='{advanceSearchBugList.PrimaryBugId}' , @ApplicationId='{advanceSearchBugList.ApplicationId}',@ModuleId='{advanceSearchBugList.ModuleId}' , @CreatedDateFrom='{fromDate}' , @CreatedDateTo='{toDate}'";
                return await _dbContext.CrashReportListVMs.FromSqlRaw(StoredProc).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Get Bug Type List
        public async Task<List<BugIssueTypeListVM>> GetBugIssueList()
        {
            try
            {
                string StoredProc = $"exec pGetBugIssueTypeList";
                return await _dbContext.BugIssueTypeListVMs.FromSqlRaw(StoredProc).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Remove The bug Type
        public async Task RemoveBugType(BugIssueTypeListVM issueType)

        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var bugType = await _dbContext.BugIssueTypes.FindAsync(issueType.Id);
                        bugType.IsDeleted = true;
                        bugType.DeletedDate = DateTime.Now;
                        bugType.DeletedBy = issueType.DeletedBy;
                        _dbContext.BugIssueTypes.Update(bugType);
                        await _dbContext.SaveChangesAsync();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception(ex.Message);
                    }
                }
            }
        }

        #endregion

        #region Add The bug Type
        public async Task SaveBugType(BugIssueType issueType)

        {
            try
            {
                using (_dbContext)
                {
                    using (var transaction = _dbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            _dbContext.BugIssueTypes.Add(issueType);
                            await _dbContext.SaveChangesAsync();
                            transaction.Commit();
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Add/Update Comment FeedBack
        public async Task<dynamic> AddCommentFeedBack(BugCommentFeedBack bugTicketComment)
        {
            try
            {
                dynamic entityObject = null;
                _dbContext.BugTicketComments.Add(bugTicketComment);
                await _dbContext.SaveChangesAsync();
                var ticket = await _dbContext.BugTickets.FindAsync(bugTicketComment.ReferenceId);
                if (ticket != null)
                {
                    entityObject = ticket;
                    bugTicketComment.NotificationParameter.ReferenceNumber = ticket.TicketId;
                    bugTicketComment.CommentFeedbackFrom = (int)CommentFeedbackFromTypeEnum.Ticket;
                    if (bugTicketComment.RemarkType == (int)RemarksTypeEnum.Comment)
                    {
                        await SaveBugStatusHistory(null, bugTicketComment.ReferenceId, ticket.StatusId, bugTicketComment.ParentCommentId == null || bugTicketComment.ParentCommentId == Guid.Empty ? (int)TicketEvenEnum.CommentAdded : (int)TicketEvenEnum.CommentReplyAdded, bugTicketComment.CreatedBy, "");
                    }
                    if (bugTicketComment.RemarkType == (int)RemarksTypeEnum.Feedback)
                    {
                        await SaveBugStatusHistory(null, bugTicketComment.ReferenceId, ticket.StatusId, (int)TicketEvenEnum.FeedbackAdded, bugTicketComment.CreatedBy, "");
                    }
                }
                else
                {
                    var bug = await _dbContext.ReportedBugs.FindAsync(bugTicketComment.ReferenceId);
                    if (bug != null)
                    {
                        entityObject = bug;
                        bugTicketComment.NotificationParameter.ReferenceNumber = bug.PrimaryBugId;
                        bugTicketComment.CommentFeedbackFrom = (int)CommentFeedbackFromTypeEnum.Bug;
                    }
                }
                return entityObject;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task UpdateCommentFeedBack(BugCommentFeedBack bugTicketComment)
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var savedComment = await _dbContext.BugTicketComments.FindAsync(bugTicketComment.Id);
                    savedComment.Comment = bugTicketComment.Comment;
                    savedComment.ModifiedBy = bugTicketComment.ModifiedBy;
                    savedComment.ModifiedDate = bugTicketComment.ModifiedDate;
                    _dbContext.BugTicketComments.Update(savedComment);
                    await SaveBugStatusHistory(null, bugTicketComment.ReferenceId, bugTicketComment.TicketStatusId, (int)TicketEvenEnum.CommentEdited, bugTicketComment.ModifiedBy, "");

                    await _dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
        #endregion

        #region Get Comments FeedBack By Id
        public async Task<List<BugTicketCommentVM>> GetBugTicketCommentFeedBack(Guid Id, int RemarksType)
        {
            try
            {
                var StoreProc = $"exec pGetBugTicketCommentFeedBack @ReferenceId='{Id}',@RemarksType='{RemarksType}'";
                var result = await _dbContext.BugTicketCommentsVM.FromSqlRaw(StoreProc).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Delete Bug Ticket Comments
        public async Task DeleteBugTicketComment(BugTicketCommentVM bugTicketCommentVM)
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var bugTicketComment = await _dbContext.BugTicketComments.Where(c => c.Id == bugTicketCommentVM.Id).FirstOrDefaultAsync();
                    if (bugTicketComment != null)
                    {
                        bugTicketComment.IsDeleted = bugTicketCommentVM.IsDeleted;
                        _dbContext.BugTicketComments.Update(bugTicketComment);

                        await SaveBugStatusHistory(null, bugTicketComment.ReferenceId, bugTicketCommentVM.TicketStatusId, (int)TicketEvenEnum.CommentDeleted, bugTicketCommentVM.DeletedBy, "");

                        _dbContext.SaveChanges();
                        await transaction.CommitAsync();
                    }
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
        #endregion

        #region Assign UnAssign Issue Type User
        public async Task AssigningTypesToUser(BugUserTypeAssignment assigningType)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var existingAssignments = await _dbContext.AssigningTypeUsers.Where(x => x.BugTypeId == assigningType.BugTypeId).ToListAsync();
                        var selectedUserIds = new HashSet<string>(assigningType.selectedUserIds ?? Enumerable.Empty<string>());
                        var selectedGroupIds = new HashSet<Guid>(assigningType.selectedGroupIds ?? Enumerable.Empty<Guid>());

                        var existingUserAssignments = existingAssignments.Where(x => !string.IsNullOrEmpty(x.UserId)).ToList();
                        var existingGroupAssignments = existingAssignments.Where(x => x.GroupId.HasValue).ToList();

                        var usersToRemove = existingUserAssignments.Where(x => !selectedUserIds.Contains(x.UserId)).ToList();
                        var groupsToRemove = existingGroupAssignments.Where(x => !selectedGroupIds.Contains((Guid)x.GroupId))
                            .ToList();
                        var newUserAssignments = selectedUserIds.Where(id => !existingUserAssignments.Any(x => x.UserId == id))
                            .Select(id => new BugUserTypeAssignment
                            {
                                Id = Guid.NewGuid(),
                                BugTypeId = assigningType.BugTypeId,
                                UserId = id,
                                GroupId = null,
                                CreatedBy = assigningType.CreatedBy,
                                CreatedDate = DateTime.Now,
                            }).ToList();
                        var newGroupAssignments = selectedGroupIds.Where(id => !existingGroupAssignments.Any(x => x.GroupId == id))
                            .Select(id => new BugUserTypeAssignment
                            {
                                Id = Guid.NewGuid(),
                                BugTypeId = assigningType.BugTypeId,
                                UserId = null,
                                GroupId = id,
                                CreatedBy = assigningType.CreatedBy,
                                CreatedDate = DateTime.Now,
                            }).ToList();
                        var entitiesToRemove = usersToRemove.Concat(groupsToRemove).ToList();
                        var entitiesToAdd = newUserAssignments.Concat(newGroupAssignments).ToList();
                        if (entitiesToRemove.Any())
                        {
                            _dbContext.AssigningTypeUsers.RemoveRange(entitiesToRemove);
                        }
                        if (entitiesToAdd.Any())
                        {
                            _dbContext.AssigningTypeUsers.AddRange(entitiesToAdd);
                        }
                        if (entitiesToRemove.Any() || entitiesToAdd.Any())
                        {
                            await _dbContext.SaveChangesAsync();
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                        transaction.Rollback();
                    }
                }
            }
        }
        public async Task UnAssigningTypesToUser(BugUserTypeAssignment assigningType)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var userIdsToUnassign = new HashSet<string>(assigningType.selectedUserIds ?? Enumerable.Empty<string>());
                        var groupIdsToUnassign = new HashSet<Guid>(assigningType.selectedGroupIds ?? Enumerable.Empty<Guid>());
                        var existingAssignments = await _dbContext.AssigningTypeUsers
                            .Where(x => x.BugTypeId == assigningType.BugTypeId)
                            .ToListAsync();
                        var assignmentsToRemove = existingAssignments.Where(x => (!string.IsNullOrEmpty(x.UserId) && userIdsToUnassign.Contains(x.UserId)) ||
                                        (x.GroupId.HasValue && groupIdsToUnassign.Contains((Guid)x.GroupId))).ToList();
                        if (assignmentsToRemove.Any())
                        {
                            _dbContext.AssigningTypeUsers.RemoveRange(assignmentsToRemove);
                            await _dbContext.SaveChangesAsync();
                        }
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                        transaction.Rollback();
                    }
                }
            }
        }
        #endregion

        #region Update Ticket Status
        public async Task UpdateTicketStatus(DecisionStatusVM decisionStatus)
        {
            try
            {
                using (_dbContext)
                {
                    using (var transaction = _dbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            int EventId = 0;
                            var ticket = await _dbContext.BugTickets.FindAsync(decisionStatus.ReferenceId);
                            ticket.StatusId = decisionStatus.StatusId;
                            decisionStatus.EntityCreator = ticket.CreatedBy;
                            decisionStatus.NotificationParameter.ReferenceNumber = ticket.TicketId;
                            decisionStatus.AssignedUser = ticket.AssignTo;
                            decisionStatus.GroupId = ticket.GroupId;
                            if (decisionStatus.StatusId == (int)BugStatusEnum.Rejected)
                            {
                                ticket.AssignTo = null;
                                ticket.GroupId = null;
                                EventId = (int)TicketEvenEnum.TicketRejected;
                            }
                            if (decisionStatus.StatusId == (int)BugStatusEnum.Resolved)
                            {
                                ticket.ResolvedBy = decisionStatus.UserName;
                                ticket.ResolutionDate = DateTime.Now;
                                EventId = (int)TicketEvenEnum.ResolutionAdded;
                            }
                            if (decisionStatus.StatusId == (int)BugStatusEnum.InProgress)
                            {
                                var user = _accountRepository.GetUserByUserEmail(decisionStatus.UserName);
                                ticket.AssignTo = user.Id;
                                ticket.GroupId = null;
                                EventId = (int)TicketEvenEnum.TicketAccepted;
                            }
                            if (decisionStatus.StatusId == (int)BugStatusEnum.Closed)
                            {
                                EventId = (int)TicketEvenEnum.TicketClosed;
                            }
                            if (decisionStatus.StatusId == (int)BugStatusEnum.Reopened)
                            {
                                EventId = (int)TicketEvenEnum.TicketReOpened;
                            }

                            _dbContext.BugTickets.Update(ticket);
                            await _dbContext.SaveChangesAsync();
                            if (decisionStatus.StatusId == (int)BugStatusEnum.InProgress)
                            {
                                if (ticket.BugId != null)
                                {
                                    await UpdateBugStatus((Guid)ticket.BugId, (int)decisionStatus.StatusId, decisionStatus.UserName, decisionStatus.Reason);
                                    await SaveBugStatusHistory(null, (Guid)ticket.BugId, decisionStatus.StatusId, null, decisionStatus.UserName, decisionStatus.Reason);

                                }
                            }
                            await SaveBugStatusHistory(null, decisionStatus.ReferenceId, decisionStatus.StatusId, EventId, decisionStatus.UserName, decisionStatus.Reason);
                            if (decisionStatus.StatusId == (int)BugStatusEnum.Reopened || decisionStatus.StatusId == (int)BugStatusEnum.Rejected || decisionStatus.StatusId == (int)BugStatusEnum.Resolved)
                            {
                                BugCommentFeedBack reason = new BugCommentFeedBack();
                                reason.Id = (Guid)decisionStatus.CommentId;
                                reason.Comment = decisionStatus.Reason;
                                reason.ReferenceId = decisionStatus.ReferenceId;
                                reason.CreatedBy = decisionStatus.UserName;
                                reason.CreatedDate = DateTime.Now;
                                reason.RemarkType = decisionStatus.StatusId == (int)BugStatusEnum.Reopened ? (int)RemarksTypeEnum.Reason : decisionStatus.StatusId == (int)BugStatusEnum.Rejected ? (int)RemarksTypeEnum.RejectReason : (int)RemarksTypeEnum.Resolution;
                                await AddCommentFeedBack(reason);

                            }
                            transaction.Commit();

                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw new Exception(ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Update Bug Status
        public async Task UpdateBugStatus(Guid BugId, int StatusId, string UserName, string? Remarks = "")
        {
            try
            {

                var bug = await _dbContext.ReportedBugs.FindAsync(BugId);
                if (bug.StatusId != (int)BugStatusEnum.Resolved)
                {

                    bug.StatusId = StatusId;
                    _dbContext.ReportedBugs.Update(bug);
                    await _dbContext.SaveChangesAsync();
                    await SaveBugStatusHistory(null, BugId, StatusId, null, UserName, Remarks);
                }
                if (StatusId == (int)BugStatusEnum.Resolved)
                {
                    BugCommentFeedBack reason = new BugCommentFeedBack();
                    reason.Id = Guid.NewGuid();
                    reason.Comment = Remarks;
                    reason.ReferenceId = BugId;
                    reason.CreatedBy = UserName;
                    reason.CreatedDate = DateTime.Now;
                    reason.RemarkType = (int)RemarksTypeEnum.Resolution;
                    await AddCommentFeedBack(reason);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get Modules By Application Id
        public async Task<List<BugModule>> GetModulesByApplicationId(int ApplicationId)
        {
            try
            {
                var StoreProc = $"exec pBugModuleOnChangeApplication @ApplicationId ='{ApplicationId}'";
                var result = await _dbContext.BugModules.FromSqlRaw(StoreProc).ToListAsync();
                if (result != null)
                {
                    return result;
                }
                else
                {
                    return new List<BugModule>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Assigning Types Module
        public async Task AssigningTypesModule(BugModuleTypeAssignment assigningType)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        // Create procedure to get existing assigments on the basis of type and application
                        var StoreProc = $"exec pGetExistingModuleTypeAssignment @BugTypeId ='{assigningType.BugTypeId}', @ApplicationId ='{assigningType.ApplicationId}'";
                        var existingAssignments = await _dbContext.AssigningTypeModule.FromSqlRaw(StoreProc).ToListAsync();
                        var selectedModuleIds = new HashSet<int>(assigningType.SelectedModule ?? Enumerable.Empty<int>());
                        var modulesToRemove = existingAssignments.Where(x => !selectedModuleIds.Contains(x.ModuleId)).ToList();
                        var typeAssignment = await _dbContext.AssigningTypeModule.Where(x => x.BugTypeId == assigningType.BugTypeId).ToListAsync();
                        var newModuleAssignments = selectedModuleIds.Where(id => !existingAssignments.Any(x => x.ModuleId == id))
                                                    .Select(id => new BugModuleTypeAssignment
                                                    {
                                                        Id = Guid.NewGuid(),
                                                        BugTypeId = assigningType.BugTypeId,
                                                        ModuleId = id,
                                                        PriorityId = assigningType.PriorityId,
                                                        SeverityId = assigningType.SeverityId,
                                                        ApplicationId = assigningType.ApplicationId,
                                                        CreatedBy = assigningType.CreatedBy,
                                                        CreatedDate = DateTime.Now,
                                                    }).ToList();

                        if (modulesToRemove.Any())
                        {
                            _dbContext.AssigningTypeModule.RemoveRange(modulesToRemove);
                        }
                        if (newModuleAssignments.Any())
                        {
                            _dbContext.AssigningTypeModule.AddRange(newModuleAssignments);
                        }
                        if (modulesToRemove.Any() || newModuleAssignments.Any())
                        {
                            await _dbContext.SaveChangesAsync();
                        }
                        if (typeAssignment.Any())
                        {
                            typeAssignment.ForEach(x => { x.PriorityId = assigningType.PriorityId; x.SeverityId = assigningType.SeverityId; });
                            await _dbContext.SaveChangesAsync();
                        }
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception(ex.Message);
                    }
                }
            }
        }

        #endregion

        #region Get Assign Type Modules
        public async Task<List<BugModuleTypeAssignment>> GetAssignTypeModules(int BugId)
        {
            try
            {
                var result = await _dbContext.AssigningTypeModule.Where(x => x.BugTypeId == BugId).ToListAsync();
                if (result != null)
                {
                    return result;
                }
                else
                {
                    return new List<BugModuleTypeAssignment>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region UnAssigng types Module
        public async Task UnAssigningTypesModule(BugModuleTypeAssignment assigningType)
        {
            try
            {
                var result = await _dbContext.AssigningTypeModule.Where(x => x.BugTypeId == assigningType.BugTypeId).ToListAsync();

                if (result != null)
                {
                    foreach (var item in assigningType.SelectedModule)
                    {
                        var selected = _dbContext.AssigningTypeModule.Where(x => x.ModuleId == item);
                        _dbContext.AssigningTypeModule.RemoveRange(selected);
                        _dbContext.SaveChanges();
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get Assigned Group By Type Id
        public async Task<List<BugUserTypeAssignment>> GetGroupByTypeId(int TypeId)
        {
            try
            {
                var result = await _dbContext.AssigningTypeUsers.Where(x => x.BugTypeId == TypeId && x.GroupId != null).ToListAsync();
                if (result != null)
                {
                    return result;

                }
                else
                {
                    return new List<BugUserTypeAssignment>();
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<List<ReportedBug>> GetBugList()
        {
            try
            {
                var result = await _dbContext.ReportedBugs.ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Ticket Tagging And Assignment
        public async Task TicketTaggingAndAssignment(TicketAssignmentVM ticketAssignment)
        {
            try
            {
                using (_dbContext)
                {
                    using (var transaction = _dbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            var selectedTicket = _dbContext.BugTickets.Where(x => x.Id == ticketAssignment.TicketId).FirstOrDefault();
                            if (selectedTicket != null)
                            {
                                ticketAssignment.NotificationParameter.ReferenceNumber = selectedTicket.TicketId;
                                selectedTicket.BugId = ticketAssignment.BugId == Guid.Empty ? null : ticketAssignment.BugId;
                                selectedTicket.ApplicationId = ticketAssignment.ApplicationId;
                                selectedTicket.ModuleId = ticketAssignment.ModuleId;
                                selectedTicket.IssueTypeId = ticketAssignment.IssueTypeId;
                                selectedTicket.PriorityId = ticketAssignment.PriorityId;
                                selectedTicket.SeverityId = ticketAssignment.SeverityId;
                                selectedTicket.GroupId = ticketAssignment.GroupId;
                                selectedTicket.AssignTo = ticketAssignment.UserId;
                                selectedTicket.StatusId = (int)BugStatusEnum.Assigned;
                                _dbContext.BugTickets.Update(selectedTicket);
                                await _dbContext.SaveChangesAsync();
                                var HistoryId = Guid.NewGuid();
                                await SaveBugStatusHistory(HistoryId, (Guid)ticketAssignment.TicketId, (int)BugStatusEnum.Assigned, (int)TicketEvenEnum.TicketAssigned, ticketAssignment.UserName, ticketAssignment.Remarks);
                                await SaveTicketAssignmentHistory(HistoryId, ticketAssignment.UserId, ticketAssignment.GroupId, ticketAssignment.AssignmentTypeId, ticketAssignment.UserName);
                            }
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw new Exception(ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        #endregion

        #region Update All Selected Ticket Status
        public async Task UpdateAllSelectedTicketStatus(IList<TicketListVM> selectedTicketList, string UserName)
        {
            try
            {
                using (_dbContext)
                {
                    using (var transaction = _dbContext.Database.BeginTransaction())
                    {
                        foreach (var selectedticket in selectedTicketList)
                        {
                            var ticket = await _dbContext.BugTickets.FindAsync(selectedticket.Id);
                            ticket.StatusId = (int)BugStatusEnum.Closed;
                            _dbContext.BugTickets.Update(ticket);
                            await _dbContext.SaveChangesAsync();
                            await SaveBugStatusHistory(null, (Guid)ticket.Id, (int)BugStatusEnum.Closed, (int)TicketEvenEnum.TicketClosed, UserName, "");
                        }
                        transaction.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Save Ticket Assignment History
        public async Task SaveTicketAssignmentHistory(Guid HistoryId, string? UserId, Guid? GroupId, int AssignmentType, string UserName)
        {
            try
            {
                TicketAssignmentHistory assignmentHistory = new TicketAssignmentHistory();
                assignmentHistory.Id = Guid.NewGuid();
                assignmentHistory.HistoryId = HistoryId;
                assignmentHistory.UserId = UserId;
                assignmentHistory.GroupId = GroupId;
                assignmentHistory.AssignmentType = AssignmentType;
                assignmentHistory.CreatedDate = DateTime.Now;
                assignmentHistory.CreatedBy = UserName;
                _dbContext.TicketAssignmentHistories.Add(assignmentHistory);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
        #region Get Ticekt Comment By id
        public async Task<BugCommentFeedBack> GetBugTicketCommentById(Guid? CommentId)
        {
            try
            {
                return await _dbContext.BugTicketComments.Where(x => x.Id == CommentId).FirstOrDefaultAsync() ?? new BugCommentFeedBack();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get Bug List For Tagging
        public async Task<List<ReportedBugListVM>> GetBugListForTagging()
        {
            try
            {
                string StoredProc = $"exec pGetBugListForTagging";
                return await _dbContext.ReportedBugListVMs.FromSqlRaw(StoredProc).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion
        #region Get Tickets For Bug Detail
        public async Task<List<TicketListVM>> GetTicketsListByBugId(Guid bugId)
        {
            try
            {
                string StoredProc = $"exec pGetTicketsForBugDetail @BugId  ='{bugId}'";
                return await _dbContext.TicketListVM.FromSqlRaw(StoredProc).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion
    }

}



