using FATWA_DOMAIN.Enums;
using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.Lms;
using FATWA_DOMAIN.Models.Notifications;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using FATWA_DOMAIN.Models.ViewModel.Lms;
using FATWA_INFRASTRUCTURE.Database;
using Itenso.TimePeriod;
using Microsoft.EntityFrameworkCore;
using System;
using static FATWA_DOMAIN.Enums.LiteratureEnum;
using static FATWA_DOMAIN.Enums.UserEnum;

namespace FATWA_INFRASTRUCTURE.Repository
{
    public class LmsLiteratureBorrowDetailRepository : ILmsLiteratureBorrowDetail
    {
        private readonly DatabaseContext _dbContext;
        private List<BorrowDetailVM> _LmsLiteratureBorrowDetailVM;
        private List<ReturnDetailVM> _LmsLiteratureReturnDetailVM;
        private UserAndLiteratureVM _userAndLiteratureVM = new();
        private LmsLiteratureBorrowHistory lmsHistory = new();
        private BorrowedLiteratureVM _borrowedLiteratureVM = new();
        private List<BorrowedLiteratureVM> _listBorrowedLiteratureVM = new();
        private LmsLiteratureBorrowDetail _lmsLiteratureBorrowDetails = new();

        #region Literature Borrow Detail

        public LmsLiteratureBorrowDetailRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<BorrowDetailVM>> GetLmsLiteratureBorrowDetails(UserDetailVM? loggedUser)
        {
            try
            {
                if (_LmsLiteratureBorrowDetailVM == null)
                {
                    //Conditions to fetch all records for Super Admin else for loggedIn user 
                    string storedProc = "exec pBorrowDetailSelAll";
					storedProc = storedProc.PadRight(storedProc.Length + 1, ' ');

					if (loggedUser != null && (loggedUser.RoleName == Roles.FatwaAdmin.GetDisplayName() || loggedUser.RoleName == Roles.SuperAdmin.GetDisplayName() || loggedUser.RoleName == Roles.LMSAdmin.GetDisplayName()))
                        storedProc += "@userId = null";
                    else
                        storedProc += "@userId = '" + loggedUser.UserId + "'";
                    _LmsLiteratureBorrowDetailVM = await _dbContext.LmsLiteratureBorrowDetailsVM.FromSqlRaw(storedProc).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return _LmsLiteratureBorrowDetailVM;
        }

        public async Task<List<ReturnDetailVM>> GetLmsLiteratureReturnDetails(UserDetailVM? loggedUser)
        {
            try
            {
                if (_LmsLiteratureReturnDetailVM == null)
                {
                    //Conditions to fetch all records for Super Admin else for loggedIn user 
                    string storedProc = "exec pReturnDetailSelAll";

                    if (loggedUser != null && (loggedUser.RoleName == Roles.FatwaAdmin.GetDisplayName() || loggedUser.RoleName == Roles.SuperAdmin.GetDisplayName() || loggedUser.RoleName == Roles.LMSAdmin.GetDisplayName()))
                    {
                        storedProc += " @userId = null";
                    }
                    else
                    {
                        storedProc += " @userId = '" + loggedUser.UserId + "'";

                    }
                    _LmsLiteratureReturnDetailVM = await _dbContext.LmsLiteratureReturnDetailsVM.FromSqlRaw(storedProc).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return _LmsLiteratureReturnDetailVM;
        }

        //public async Task<List<ReturnDetailVM>> GetLmsLiteratureReturnDetails(UserDetailVM? loggedUser)
        //{
        //	try
        //	{
        //		if (_LmsLiteratureReturnDetailVM == null)
        //		{
        //			//Conditions to fetch all records for Super Admin else for loggedIn user 
        //			string storedProc = "exec pReturnDetailSelAll";

        //			if (loggedUser != null && (loggedUser.RoleName == Roles.FatwaAdmin.GetDisplayName() || loggedUser.RoleName == Roles.SuperAdmin.GetDisplayName()))
        //				storedProc += "@userId = null";
        //			else
        //				storedProc += "@userId = '" + loggedUser.UserId + "'";
        //			_LmsLiteratureReturnDetailVM = await _dbContext.LmsLiteratureReturnDetailsVM.FromSqlRaw(storedProc).ToListAsync();
        //		}
        //	}
        //	catch (Exception ex)
        //	{
        //		throw new Exception(ex.Message);
        //	}
        //	return _LmsLiteratureReturnDetailVM;
        //}

        public async Task<LmsLiteratureBorrowDetail> GetLmsLiteratureBorrowDetailById(int Id)
        {
            try
            {
                LmsLiteratureBorrowDetail? entity = await _dbContext.LmsLiteratureBorrowDetails.FindAsync(Id);
                if (entity != null)
                {
                    var barCodeResult = await GetBarcodeNumberDetailByUsingBarcodeId(entity.BarcodeId, _dbContext);
                    if (barCodeResult != null)
                    {
                        entity.BarCodeNumber = barCodeResult.BarCodeNumber;
                    }
                    var result = await GetUserBorrowReturnDayDurationDetailByUsingUserId(entity.UserId, _dbContext);
                    entity.BorrowReturnDayDuration = result;
					return entity;

                }
                else
                {
                    throw new ArgumentNullException();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

		private async Task<int> GetUserBorrowReturnDayDurationDetailByUsingUserId(string userId, DatabaseContext dbContext)
		{
            try
            {
                var duration = await (from eei in dbContext.UserEmploymentInformation
										  join cost in dbContext.OperatingSectorType on eei.SectorTypeId equals cost.Id
										  join ed in dbContext.Departments on cost.DepartmentId equals ed.Id
										  where eei.UserId == userId
								        select ed.Borrow_Return_Day_Duration).Distinct().FirstOrDefaultAsync();

				return duration;
            }
            catch (Exception)
            {
				throw new NotImplementedException();
			}
		}

		private async Task<LmsLiteratureBarcode> GetBarcodeNumberDetailByUsingBarcodeId(int barcodeId, DatabaseContext dbContext)
        {
            try
            {
                var result = await dbContext.LmsLiteratureBarcodes.Where(x => x.BarcodeId == barcodeId).FirstOrDefaultAsync();
                if (result != null)
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

        public async Task<int?> CreateLmsLiteratureBorrowDetail(LmsLiteratureBorrowDetail borrowDetail)
        {
            try
            {
                using (_dbContext)
                {
                    using (var transaction = _dbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            UserDetailVM? userDetail = (from usr in _dbContext.Users
                                                        join usrRole in _dbContext.UserRoles
                                                        on usr.Id equals usrRole.UserId
                                                        join role in _dbContext.Roles
                                                        on usrRole.RoleId equals role.Id
                                                        where usr.Id == borrowDetail.UserId
                                                        select new UserDetailVM
                                                        {
                                                            UserId = usr.Id,
                                                            UserName = usr.UserName,
                                                            RoleId = usrRole.RoleId,
                                                            SecurityStamp = usr.SecurityStamp,
                                                            RoleName = role.Name
                                                        }).FirstOrDefault();

                            if (userDetail != null && (userDetail.RoleName == Roles.SuperAdmin.GetDisplayName() || userDetail.RoleName == Roles.FatwaAdmin.GetDisplayName() || userDetail.RoleName == Roles.LMSAdmin.GetDisplayName()))
                                borrowDetail.BorrowApprovalStatus = (int)BorrowApprovalStatus.Approved;
                            else
                                borrowDetail.BorrowApprovalStatus = (int)BorrowApprovalStatus.PendingForApproval;

                            await _dbContext.LmsLiteratureBorrowDetails.AddAsync(borrowDetail);
                            await _dbContext.SaveChangesAsync();
                            await UpdateLiteratureBarcodeRecord(borrowDetail.BarcodeId, borrowDetail.BorrowApprovalStatus, _dbContext);

                            if (borrowDetail.BorrowId != 0)
                            {
                                //To Update Eligible Count in User tbl
                                User? user = _dbContext.Users.FirstOrDefault(x => x.Id == borrowDetail.UserId);
                                if (user != null)
                                {
                                    user.EligibleCount += 1;
                                    _dbContext.Entry(user).State = EntityState.Modified;
                                    await _dbContext.SaveChangesAsync();
                                }

                                //To Update Number Of Borrowed Books in Literature tbl 
                                LmsLiterature? literature = _dbContext.LmsLiteratures.FirstOrDefault(x => x.LiteratureId == borrowDetail.LiteratureId);
                                if (literature != null)
                                {
                                    literature.NumberOfBorrowedBooks += 1;
                                    _dbContext.Entry(literature).State = EntityState.Modified;
                                    await _dbContext.SaveChangesAsync();
                                }
                                lmsHistory.BorrowId = borrowDetail.BorrowId;
                                lmsHistory.LiteratureId = borrowDetail.LiteratureId;
                                lmsHistory.UserId = borrowDetail.UserId;
                                lmsHistory.CreatedDate = borrowDetail.CreatedDate;
                                lmsHistory.CreatedBy = borrowDetail.CreatedBy;
                                if (userDetail != null && (userDetail.RoleName == Roles.SuperAdmin.GetDisplayName() || userDetail.RoleName == Roles.FatwaAdmin.GetDisplayName() || userDetail.RoleName == Roles.LMSAdmin.GetDisplayName()))
                                    lmsHistory.EventId = (int)BorrowedLiteratureEvent.BorrowRequestApproved;
                                else
                                    lmsHistory.EventId = (int)BorrowedLiteratureEvent.BorrowRequestPendingForApproval;

                                await AddBorrowedLiteratureHistory(lmsHistory, _dbContext);

                            }

                            transaction.Commit();
                            //For Notification 
                            borrowDetail.NotificationParameter.Name = borrowDetail.LiteratureName;
                            return borrowDetail.BorrowId;


                        }
                        catch (Exception)
                        {
                            transaction.Rollback();

                            return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task UpdateLiteratureBarcodeRecord(int barCodeId, int borrowApprovalStatus, DatabaseContext dbContext)
        {
            try
            {
                var result = await dbContext.LmsLiteratureBarcodes.Where(x => x.BarcodeId == barCodeId).FirstOrDefaultAsync();
                if (result != null)
                {
                    if (borrowApprovalStatus == (int)BorrowApprovalStatus.Rejected)
                    {
                        result.IsBorrowed = false;
                    }
                    else
                    {
                        result.IsBorrowed = true;
                    }
                    dbContext.Entry(result).State = EntityState.Modified;
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateLmsLiteratureBorrowDetail(LmsLiteratureBorrowDetail borrowDetail)
        {
            try
            {
                using (_dbContext)
                {
                    using (var transaction = _dbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            _dbContext.Entry(borrowDetail).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            // when user submit new borrow request and library admin will reject then run this scenario
                            if (borrowDetail.ReturnDate == null && borrowDetail.BorrowApprovalStatus == (int)BorrowApprovalStatus.Rejected)
                            {
                                await UpdateLiteratureBarcodeRecord(borrowDetail.BarcodeId, borrowDetail.BorrowApprovalStatus, _dbContext);
                                //To Update Eligible Count in User tbl
                                User? user = _dbContext.Users.FirstOrDefault(x => x.Id == borrowDetail.UserId);
                                if (user != null)
                                {
                                    user.EligibleCount -= 1;
                                    _dbContext.Entry(user).State = EntityState.Modified;
                                    await _dbContext.SaveChangesAsync();
                                }

                                //To Update Number Of Borrowed Books in Literature tbl 
                                LmsLiterature? literature = _dbContext.LmsLiteratures.FirstOrDefault(x => x.LiteratureId == borrowDetail.LiteratureId);
                                if (literature != null)
                                {
                                    literature.NumberOfBorrowedBooks -= 1;
                                    _dbContext.Entry(literature).State = EntityState.Modified;
                                    await _dbContext.SaveChangesAsync();
                                }
                            }
                            // when user submit return borrowed literature request and library admin will approve then run this scenario
                            else if (borrowDetail.ReturnDate != null && borrowDetail.BorrowReturnApprovalStatus == (int)BorrowReturnApprovalStatus.Returned)
                            {
                                //To Update Eligible Count in User tbl
                                User? user = _dbContext.Users.FirstOrDefault(x => x.Id == borrowDetail.UserId);
                                if (user != null)
                                {
                                    user.EligibleCount -= 1;
                                    _dbContext.Entry(user).State = EntityState.Modified;
                                    await _dbContext.SaveChangesAsync();
                                }

                                //To Update Number Of Borrowed Books in Literature tbl 
                                LmsLiterature? literature = _dbContext.LmsLiteratures.FirstOrDefault(x => x.LiteratureId == borrowDetail.LiteratureId);
                                if (literature != null)
                                {
                                    literature.NumberOfBorrowedBooks -= 1;
                                    _dbContext.Entry(literature).State = EntityState.Modified;
                                    await _dbContext.SaveChangesAsync();
                                }
                                // update barcodenumber Isborrowed column.
                                var barCodeResult = _dbContext.LmsLiteratureBarcodes.FirstOrDefault(x => x.BarcodeId == borrowDetail.BarcodeId);
                                if (barCodeResult != null)
                                {
                                    barCodeResult.IsBorrowed = false;
                                    _dbContext.Entry(barCodeResult).State = EntityState.Modified;
                                    await _dbContext.SaveChangesAsync();
                                }
                            }
                            // when user submit return borrowed literature request and library admin will reject then run this scenario
                            else if (borrowDetail.ReturnDate != null && borrowDetail.BorrowReturnApprovalStatus == (int)BorrowReturnApprovalStatus.Rejected)
                            {
                                //borrowDetail.ApplyReturnDate = null;
                                borrowDetail.ReturnDate = null;
                                borrowDetail.BorrowReturnApprovalStatus = (int)BorrowReturnApprovalStatus.Rejected;
                                _dbContext.Entry(borrowDetail).State = EntityState.Modified;
                                await _dbContext.SaveChangesAsync();
                            }
                            transaction.Commit();
                            borrowDetail.NotificationParameter.Name = borrowDetail.LiteratureName;
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

        public async Task UpdateLmsLiteratureRetunDetail(BorrowDetailVM borrowDetail)
        {
            try
            {
                using (_dbContext)
                {
                    using (var transaction = _dbContext.Database.BeginTransaction())
                    {
                        try
                        {

                            //Update return date and borrow return approval status columns in borrow request table
                            LmsLiteratureBorrowDetail? lmsLiteratureBorrowDetail = await _dbContext.LmsLiteratureBorrowDetails.FirstOrDefaultAsync(x => x.BorrowId == borrowDetail.BorrowId);
                            if (lmsLiteratureBorrowDetail != null)
                            {
                                if (borrowDetail.ExtensionApprovalStatus == (int)BorrowApprovalStatus.PendingForExtensionApproval && borrowDetail.Extended) // Extension applied
                                {
                                    lmsLiteratureBorrowDetail.Extended = borrowDetail.Extended;
                                    lmsLiteratureBorrowDetail.ExtensionApprovalStatus = (int)borrowDetail.ExtensionApprovalStatus;
									lmsLiteratureBorrowDetail.ExtendDueDate = borrowDetail.ExtendDueDate;
                                    lmsHistory.BorrowId = borrowDetail.BorrowId;
                                    lmsHistory.LiteratureId = (int)borrowDetail.LiteratureId;
                                    lmsHistory.UserId = borrowDetail.UserId;
                                    lmsHistory.CreatedBy = borrowDetail.LoggedInUser;
                                    lmsHistory.CreatedDate = DateTime.Now;
                                    lmsHistory.EventId = (int)BorrowedLiteratureEvent.PendingForExtensionApproval;
                                    await AddBorrowedLiteratureHistory(lmsHistory, _dbContext);
                                }
                                else
                                {
									lmsLiteratureBorrowDetail.ApplyReturnDate = borrowDetail.ApplyReturnDate;
									lmsLiteratureBorrowDetail.BorrowReturnApprovalStatus = (int)borrowDetail.DecisionId;
                                    lmsHistory.BorrowId = borrowDetail.BorrowId;
                                    lmsHistory.LiteratureId = (int)borrowDetail.LiteratureId;
                                    lmsHistory.UserId = borrowDetail.UserId;
                                    lmsHistory.CreatedBy = borrowDetail.LoggedInUser;
                                    lmsHistory.CreatedDate = DateTime.Now;
                                    lmsHistory.EventId = (int)BorrowedLiteratureEvent.PendingForReturnBookApproval;
                                    await AddBorrowedLiteratureHistory(lmsHistory, _dbContext);
                                }
                                _dbContext.Entry(lmsLiteratureBorrowDetail).State = EntityState.Modified;
                                await _dbContext.SaveChangesAsync();
                            }
                            transaction.Commit();
                            borrowDetail.NotificationParameter.Name = lmsLiteratureBorrowDetail.LiteratureName;
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

        public async Task<bool> DeleteLiteratureBorrow(BorrowDetailVM literatureBorrow)
        {
            bool isSaved = false;
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        LmsLiteratureBorrowDetail? literatureBorrowDetail = await _dbContext.LmsLiteratureBorrowDetails.FindAsync(literatureBorrow.BorrowId);
                        if (literatureBorrowDetail != null)
                        {
                            isSaved = await RemoveLiteratureBorrow(literatureBorrowDetail, literatureBorrow, _dbContext);

                            isSaved = await UpdateUserBorrowEligibleCount(literatureBorrowDetail, _dbContext);

                            isSaved = await UpdateLiteratureNumberOfBorrowedBook(literatureBorrowDetail, _dbContext);
							await UpdateLiteratureBarcodeRecord(literatureBorrowDetail.BarcodeId, literatureBorrowDetail.BorrowApprovalStatus, _dbContext);
							if (isSaved == true)
                                transaction.Commit();
                        }
                    }
                    catch
                    {
                        isSaved = false;
                        transaction.Rollback();
                    }
                }
            }
            return isSaved;
        }

        protected async Task<bool> RemoveLiteratureBorrow(LmsLiteratureBorrowDetail _LmsLiteratureBorrowDetail, BorrowDetailVM literatureBorrow, DatabaseContext _dbContext)
        {
            bool isSaved;
            try
            {
                if (_LmsLiteratureBorrowDetail != null)
                {
                    _LmsLiteratureBorrowDetail.BorrowApprovalStatus = (int)BorrowApprovalStatus.Rejected;
                    _LmsLiteratureBorrowDetail.DeletedBy = literatureBorrow.DeletedBy;
                    _LmsLiteratureBorrowDetail.DeletedDate = DateTime.Now;
                    _LmsLiteratureBorrowDetail.IsDeleted = true;

                    _dbContext.Entry(_LmsLiteratureBorrowDetail).State = EntityState.Modified;
                    await _dbContext.SaveChangesAsync();
                }
                isSaved = true;
            }
            catch
            {
                isSaved = false;
            }
            return isSaved;
        }

        protected async Task<bool> UpdateUserBorrowEligibleCount(LmsLiteratureBorrowDetail _LmsLiteratureBorrowDetail, DatabaseContext _dbContext)
        {
            bool isSaved;
            try
            {
                var user = _dbContext.Users.FirstOrDefault(x => x.Id == _LmsLiteratureBorrowDetail.UserId);
                if (user != null)
                {
                    user.EligibleCount -= 1;
                    _dbContext.Entry(user).State = EntityState.Modified;
                    await _dbContext.SaveChangesAsync();
                }

                isSaved = true;
            }
            catch
            {
                isSaved = false;
            }
            return isSaved;
        }

        protected async Task<bool> UpdateLiteratureNumberOfBorrowedBook(LmsLiteratureBorrowDetail _LmsLiteratureBorrowDetail, DatabaseContext _dbContext)
        {
            bool isSaved;
            try
            {
                var literatureResult = _dbContext.LmsLiteratures.FirstOrDefault(x => x.LiteratureId == _LmsLiteratureBorrowDetail.LiteratureId);
                if (literatureResult != null)
                {
                    literatureResult.NumberOfBorrowedBooks -= 1;
                    _dbContext.Entry(literatureResult).State = EntityState.Modified;
                    await _dbContext.SaveChangesAsync();
                }

                isSaved = true;
            }
            catch
            {
                isSaved = false;
            }
            return isSaved;
        }

        protected async Task<bool> UpdateLiteratureBarcodeIsBorrowed(LmsLiteratureBorrowDetail _LmsLiteratureBorrowDetail, DatabaseContext _dbContext)
        {
            bool isSaved;
            try
            {
                var barCodeResult = _dbContext.LmsLiteratureBarcodes.FirstOrDefault(x => x.BarcodeId == _LmsLiteratureBorrowDetail.BarcodeId);
                if (barCodeResult != null)
                {
                    barCodeResult.IsBorrowed = false;
                    _dbContext.Entry(barCodeResult).State = EntityState.Modified;
                    await _dbContext.SaveChangesAsync();
                }

                isSaved = true;
            }
            catch
            {
                isSaved = false;
            }
            return isSaved;
        }

        #endregion

        #region Literature Extension Borrow Approval

        public async Task<List<BorrowDetailVM>> GetLmsLiteratureBorrowExtensionApprovals()
        {
            try
            {
                string storedProc = "exec pBorroweBorrowExtensionApprovalSelAll ";
                return await _dbContext.LmsLiteratureBorrowDetailsVM.FromSqlRaw(storedProc).ToListAsync();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public async Task<IEnumerable<LiteratureBorrowApprovalType>> GetLiteratureBorrowApprovalTypes()
        {
            try
            {
                IEnumerable<LiteratureBorrowApprovalType> reponse = await _dbContext.LiteratureBorrowApprovalTypes.ToListAsync();
                if (reponse != null)
                {
                    return reponse;
                }
                else
                {

                    throw new ArgumentNullException();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateLiteratureBorrowApprovalStatus(LmsLiteratureBorrowDetail LmsLiteratureBorrowDetail)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        if (LmsLiteratureBorrowDetail.ExtensionApprovalStatus == (int)FATWA_GENERAL.Helper.Enum.BorrowExtensionApprovalStatus.Approve)
                        {
                            LmsLiteratureBorrowDetail.ExtensionApprovalStatus = (int)BorrowApprovalStatus.Extended;
							LmsLiteratureBorrowDetail.ExtendDueDate = DateTime.Now.AddDays(7);
						}
                        else if (LmsLiteratureBorrowDetail.ExtensionApprovalStatus == (int)FATWA_GENERAL.Helper.Enum.BorrowExtensionApprovalStatus.Reject)
                        {
							LmsLiteratureBorrowDetail.ExtensionApprovalStatus = (int)BorrowApprovalStatus.ExtensionRejected;
						}
                        _dbContext.Entry(LmsLiteratureBorrowDetail).State = EntityState.Modified;
                        await _dbContext.SaveChangesAsync();

                        transaction.Commit();
                        LmsLiteratureBorrowDetail.NotificationParameter.Name = LmsLiteratureBorrowDetail.LiteratureName;
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

        #region Literature Borrow Approval

        public async Task<List<BorrowDetailVM>> GetLiteratureBorrowApprovals()
        {
            try
            {
                if (_LmsLiteratureBorrowDetailVM == null)
                {
                    string storedProc = "exec pLiteratureBorrowApprovalSelAll ";
                    _LmsLiteratureBorrowDetailVM = await _dbContext.LmsLiteratureBorrowDetailsVM.FromSqlRaw(storedProc).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return _LmsLiteratureBorrowDetailVM;

        }

        #endregion

        #region Get Borrow Approval Status Details
        public async Task<List<LiteratureBorrowApprovalType>> GetBorrowApprovalStatusDetails()
        {
            try
            {
                List<LiteratureBorrowApprovalType> result = new List<LiteratureBorrowApprovalType>();
                result = await _dbContext.LiteratureBorrowApprovalTypes.ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
        public async Task<UserAndLiteratureVM> GetBorrowedLiteratureAndUserDetailByUserIdAndCivilId(string? userId,string? civilId)
        {
            try
            {
                _userAndLiteratureVM.UserDetail = await GetUserDetailByCivilIdOrUserId(civilId, userId);
                _userAndLiteratureVM.Literature = await GetBorrowedLiteratureByCivilIdOrUserId(civilId, userId);
                if (_userAndLiteratureVM != null)
                {

                    return _userAndLiteratureVM;
                }
                else
                {
                    return _userAndLiteratureVM = new();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public async Task<List<UserDetailsVM>> GetUserDetailByCivilIdOrUserId(string? civilId, string? userId)
        {
            try
            {
                string procName = string.Empty;
                string storedProc = string.Empty;
                procName = "pGetUserDetailByUserIdAndCivilId";
                storedProc = $"exec {procName} @civilId='{civilId}',@userId='{userId}'";

                var res =  await _dbContext.UserDetailsVMs.FromSqlRaw(storedProc).ToListAsync();
                if(res.Count()>0)
                {
                    return res;
                }
                else
                {
                    return _userAndLiteratureVM.UserDetail = new();


                }
            }
            catch (Exception ex)

            {
                throw new Exception(ex.Message);
            }

        }
        public async Task<List<BorrowedLiteratureVM>> GetBorrowedLiteratureByCivilIdOrUserId(string? civilId, string? userId)
        {
            try
            {
                string procName = string.Empty;
                string storedProc = string.Empty;
                procName = "pGetBorrowedLiteratureByCivilIdOrUserId";
                storedProc = $"exec {procName} @civilId='{civilId}',@userId='{userId}'";
                return await _dbContext.BorrowedLiteratureVMs.FromSqlRaw(storedProc).ToListAsync();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);    
            }

        }

        public async Task AddBorrowedLiteratureHistory(LmsLiteratureBorrowHistory history, DatabaseContext dbContext)
        {
            history.Id = Guid.NewGuid();
            dbContext.LmsLiteratureBorrowHistory.Add(history);   
            await _dbContext.SaveChangesAsync();    
        }

        public async Task<BorrowedLiteratureVM> GetLiteratureByBarcode(string BarCode)
        {
            try
            {
                string procName = string.Empty;
                string storedProc = string.Empty;
                procName = "pLiteratureSelByBarcode";
                storedProc = $"exec {procName} @BarCode='{BarCode}'";
                var res = await _dbContext.BorrowedLiteratureVMs.FromSqlRaw(storedProc).ToListAsync();
                if (res.Count > 0)
                {
                    _borrowedLiteratureVM = res.FirstOrDefault();
                    return _borrowedLiteratureVM;
                }
                else
                {

                    return _borrowedLiteratureVM;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task CreateLmsLiteratureBorrow(LmsLiteratureBorrowDetail borrowDetail, DatabaseContext _dbContext)
        {
            try
            {
                
                        
                                borrowDetail.BorrowApprovalStatus = (int)BorrowApprovalStatus.Approved;

                            await _dbContext.LmsLiteratureBorrowDetails.AddAsync(borrowDetail);
                            await _dbContext.SaveChangesAsync();
                            await UpdateLiteratureBarcodeRecord(borrowDetail.BarcodeId, borrowDetail.BorrowApprovalStatus, _dbContext);

                            if (borrowDetail.BorrowId != 0)
                            {
                                //To Update Eligible Count in User tbl
                                User? user = _dbContext.Users.FirstOrDefault(x => x.Id == borrowDetail.UserId);
                                if (user != null)
                                {
                                    user.EligibleCount += 1;
                                    _dbContext.Entry(user).State = EntityState.Modified;
                                    await _dbContext.SaveChangesAsync();
                                }

                                //To Update Number Of Borrowed Books in Literature tbl 
                                LmsLiterature? literature = _dbContext.LmsLiteratures.FirstOrDefault(x => x.LiteratureId == borrowDetail.LiteratureId);
                                if (literature != null)
                                {
                                    literature.NumberOfBorrowedBooks += 1;
                                    _dbContext.Entry(literature).State = EntityState.Modified;
                                    await _dbContext.SaveChangesAsync();
                                }
                                lmsHistory.BorrowId = borrowDetail.BorrowId;
                                lmsHistory.LiteratureId = borrowDetail.LiteratureId;
                                lmsHistory.UserId = borrowDetail.UserId;
                                lmsHistory.CreatedDate = borrowDetail.CreatedDate;
                                lmsHistory.CreatedBy = borrowDetail.CreatedBy;
                                    lmsHistory.EventId = (int)BorrowedLiteratureEvent.BorrowRequestApproved;
                                
                                await AddBorrowedLiteratureHistory(lmsHistory, _dbContext);

                            }

                          
                            //For Notification 

                        }
                        
                       
                    
               
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<UserBorrowedHistoryVM>> GetUserBorrowHistoryByUserId(string UserId)
        {
            try
            {
                string procName = string.Empty;
                string storedProc = string.Empty;
                procName = "exec pGetUserBorrowingHistory";
                storedProc = $"{procName} @userId='{UserId}'";
                return await _dbContext.UserBorrowedHistoryVMs.FromSqlRaw(storedProc).ToListAsync();
               
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateLiteratureReturnExtendDetail(BorrowedLiteratureVM borrowDetail)
        {
            try
            {
                using (_dbContext)
                {
                    using (var transaction = _dbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            
                                if (borrowDetail.IsNew)
                                {
                                    _lmsLiteratureBorrowDetails.BarCodeNumber = borrowDetail.Barcode;
                                    _lmsLiteratureBorrowDetails.BarcodeId = (int)borrowDetail.BarcodeId;
                                    _lmsLiteratureBorrowDetails.IssueDate = (DateTime)borrowDetail.BarrowedDate;
                                    _lmsLiteratureBorrowDetails.DueDate = (DateTime)borrowDetail.DueDate;
                                    _lmsLiteratureBorrowDetails.BorrowReturnApprovalStatus = (int)BorrowReturnApprovalStatus.Default;
                                    _lmsLiteratureBorrowDetails.CreatedBy = borrowDetail.LoggedInUser;
                                    _lmsLiteratureBorrowDetails.UserId = borrowDetail.BorrowerUserId;
                                    _lmsLiteratureBorrowDetails.CreatedDate = DateTime.Now;
                                    _lmsLiteratureBorrowDetails.LiteratureId = (int)borrowDetail.LiteratureId;
                                    await CreateLmsLiteratureBorrow(_lmsLiteratureBorrowDetails, _dbContext);

                                }
                                else
                                {
                                    LmsLiteratureBorrowDetail? lmsLiteratureBorrowDetail = await _dbContext.LmsLiteratureBorrowDetails.FirstOrDefaultAsync(x => x.BorrowId == borrowDetail.BorrowId);
                                    if (lmsLiteratureBorrowDetail != null)
                                    {
                                        if (borrowDetail.ExtensionApprovalStatus == (int)BorrowApprovalStatus.Extended && (bool)borrowDetail.Extended) // Extension applied
                                        {
                                            lmsLiteratureBorrowDetail.Extended = (bool)borrowDetail.Extended;
                                            lmsLiteratureBorrowDetail.ExtensionApprovalStatus = (int)borrowDetail.ExtensionApprovalStatus;
                                            lmsLiteratureBorrowDetail.ExtendDueDate = borrowDetail.ExtendDueDate;
                                            lmsHistory.BorrowId = borrowDetail.BorrowId;
                                            lmsHistory.LiteratureId = (int)borrowDetail.LiteratureId;
                                            lmsHistory.UserId = borrowDetail.BorrowerUserId;
                                            lmsHistory.CreatedBy = borrowDetail.LoggedInUser;
                                            lmsHistory.CreatedDate = DateTime.Now;
                                            lmsHistory.EventId = (int)BorrowedLiteratureEvent.Extended;
                                            await AddBorrowedLiteratureHistory(lmsHistory, _dbContext);

                                        }

                                        else
                                        {
                                            lmsLiteratureBorrowDetail.ReturnDate = borrowDetail.ReturnDate;
                                            lmsLiteratureBorrowDetail.BorrowReturnApprovalStatus = (int)borrowDetail.DecisionId;
                                            lmsHistory.BorrowId = borrowDetail.BorrowId;
                                            lmsHistory.LiteratureId = (int)borrowDetail.LiteratureId;
                                            lmsHistory.UserId = borrowDetail.BorrowerUserId;
                                            lmsHistory.CreatedBy = borrowDetail.LoggedInUser;
                                            lmsHistory.CreatedDate = DateTime.Now;
                                            lmsHistory.EventId = (int)BorrowedLiteratureEvent.Returned;
                                            await AddBorrowedLiteratureHistory(lmsHistory, _dbContext);
                                        //To Update Eligible Count in User tbl
                                        User? user = _dbContext.Users.FirstOrDefault(x => x.Id == borrowDetail.BorrowerUserId);
                                        if (user != null)
                                        {
                                            user.EligibleCount -= 1;
                                            _dbContext.Entry(user).State = EntityState.Modified;
                                            await _dbContext.SaveChangesAsync();
                                        }

                                        //To Update Number Of Borrowed Books in Literature tbl 
                                        LmsLiterature? literature = _dbContext.LmsLiteratures.FirstOrDefault(x => x.LiteratureId == borrowDetail.LiteratureId);
                                        if (literature != null)
                                        {
                                            literature.NumberOfBorrowedBooks -= 1;
                                            _dbContext.Entry(literature).State = EntityState.Modified;
                                            await _dbContext.SaveChangesAsync();
                                        }
                                        // update barcodenumber Isborrowed column.
                                        var barCodeResult = _dbContext.LmsLiteratureBarcodes.FirstOrDefault(x => x.BarcodeId == borrowDetail.BarcodeId);
                                        if (barCodeResult != null)
                                        {
                                            barCodeResult.IsBorrowed = false;
                                            _dbContext.Entry(barCodeResult).State = EntityState.Modified;
                                            await _dbContext.SaveChangesAsync();
                                        }

                                    }
                                    _dbContext.Entry(lmsLiteratureBorrowDetail).State = EntityState.Modified;
                                        await _dbContext.SaveChangesAsync();
                                    }
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
        private List<AllLmsUserDetailVM> _usersList;
        public async Task<List<AllLmsUserDetailVM>> GetAllLmsUserList()
        {
            try
            {
                if (_usersList == null)
                {
                    string StoredProc = "exec pLmsGetAllUser";
                    _usersList = await _dbContext.AllLmsUserDetailVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return _usersList;
        }


        public async Task<List<BorrowedLiteratureVM>> GetLmsBorrowLiteraturesAdvanceSearch(LiteratureAdvancedSearchVM advancedSearch)
        {
            try
            {
                string purchaseDateKeyword = advancedSearch.PurchaseDateKeyword != null ? Convert.ToDateTime(advancedSearch.PurchaseDateKeyword).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                string fromDate = advancedSearch.FromDate != null ? Convert.ToDateTime(advancedSearch.FromDate).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                string toDate = advancedSearch.ToDate != null ? Convert.ToDateTime(advancedSearch.ToDate).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                string StoredProc = "";
                switch (advancedSearch.EnumSearchValue)
                {
                    case LiteratureAdvancedSearchVM.AdvancedSearchDropDownEnum.Book_Name:
                        {
                            StoredProc = $"exec pLiteratureBorrowListFiltered @bookName = N'{advancedSearch.KeywordsType}', @From = '{fromDate}', @To = '{toDate}', @classificationId = {advancedSearch.ClassificationId}";
                            break;
                        }
                    case LiteratureAdvancedSearchVM.AdvancedSearchDropDownEnum.Book_Index:
                        {
                            StoredProc = $"exec pLiteratureBorrowListFiltered @indexId = '{advancedSearch.GenericsIntergerKeyword}', @From = '{fromDate}', @To = '{toDate}', @classificationId = {advancedSearch.ClassificationId}";
                            break;
                        }

                    case LiteratureAdvancedSearchVM.AdvancedSearchDropDownEnum.Barcode:
                        {
                            StoredProc = $"exec pLiteratureBorrowListFiltered @barcode = '{advancedSearch.KeywordsType}', @From = '{fromDate}', @To = '{toDate}', @classificationId = {advancedSearch.ClassificationId}";
                            break;
                        }

                    case LiteratureAdvancedSearchVM.AdvancedSearchDropDownEnum.Sticker:
                        {
                            StoredProc = $"exec pLiteratureBorrowListFiltered @character82 = '{advancedSearch.KeywordsType}', @From = '{fromDate}', @To = '{toDate}', @classificationId = {advancedSearch.ClassificationId}";
                            break;
                        }

                    case LiteratureAdvancedSearchVM.AdvancedSearchDropDownEnum.Author_Name:
                        {

                            StoredProc = $"exec pLiteratureBorrowListFiltered @authorId = '{advancedSearch.GenericsIntergerKeyword}', @From = '{fromDate}', @To = '{toDate}', @classificationId = {advancedSearch.ClassificationId}";
                            break;
                        }

                    default:
                        if (fromDate == null && toDate == null && advancedSearch.ClassificationId != 0)
                        {
                            StoredProc = $"exec pLiteratureBorrowListFiltered @classificationId = {advancedSearch.ClassificationId}";
                            break;
                        }
                        else if (fromDate != null && toDate != null)
                        {
                            StoredProc = $"exec pLiteratureBorrowListFiltered @From = '{fromDate}', @To = '{toDate}', @classificationId = {advancedSearch.ClassificationId}";
                            break;
                        }
                        StoredProc = $"exec pLiteratureBorrowListFiltered";
                        break;
                }
                if (_listBorrowedLiteratureVM.Count()<1)
                {
                    _listBorrowedLiteratureVM = await _dbContext.BorrowedLiteratureVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _listBorrowedLiteratureVM;
        }


    }
}
