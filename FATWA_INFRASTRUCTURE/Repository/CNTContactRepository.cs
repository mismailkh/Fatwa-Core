using FATWA_INFRASTRUCTURE.Database;
using Microsoft.EntityFrameworkCore;
using FATWA_DOMAIN.Interfaces.ContactManagment;
using FATWA_DOMAIN.Models.ViewModel.ContactManagmentVMs;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.Consultation;
using FATWA_DOMAIN.Models.Contact;

namespace FATWA_INFRASTRUCTURE.Repository
{
    public class CNTContactRepository : ICNTContact
    {
        #region Varriables
        private readonly DatabaseContext _dbContext;
        private List<ContactListVM> _contactListVM;
        private List<ContactCaseConsultationListVM> _contactCaseList;
        private List<ContactCaseConsultationListVM> _contactConsultationList;
        private List<ContactCaseConsultationRequestListVM> _contactConsultationRequestList;
        private List<ContactCaseConsultationRequestListVM> _contactCaseRequestList;
        private ContactDetailVM _contactDetailVM;
        #endregion

        #region Constructor
        public CNTContactRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        #endregion

        #region Get Case Request list
        public async Task<List<ContactListVM>> GetContactList(ContactAdvanceSearchVM advanceSearchVM)
        {
            try
            {
                if (_contactListVM == null)
                {
                    string createdFrom = advanceSearchVM.CreatedFrom != null ? Convert.ToDateTime(advanceSearchVM.CreatedFrom).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                    string createdTo = advanceSearchVM.CreatedTo != null ? Convert.ToDateTime(advanceSearchVM.CreatedTo).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                    string StoredProc = $"exec pCntContactList @Name ='{advanceSearchVM.Name}' , @ContactTypeId='{advanceSearchVM.ContactTypeId}' , @createdFrom='{createdFrom}' , @createdTo='{createdTo}', @PageNumber='{advanceSearchVM.PageNumber}', @PageSize='{advanceSearchVM.PageSize}'";
                    _contactListVM = await _dbContext.ContactListVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
                return _contactListVM;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get Contact Detail By Id
        public async Task<ContactDetailVM> GetContactDetailById(Guid contactId)
        {
            try
            {
                string StoredProc = $"exec pContactDetailById @contactId ='{contactId}'";

                var res = await _dbContext.ContactDetailVMs.FromSqlRaw(StoredProc).ToListAsync();
                if (res != null)
                {
                    _contactDetailVM = res.FirstOrDefault();

                }
                return _contactDetailVM;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Delete
        public async Task<bool> DeleteContact(ContactListVM contact)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var cntContact = await _dbContext.Contacts.FindAsync(contact.ContactId);
                        if (cntContact != null)
                        {
                            cntContact.IsDeleted = true;
                            cntContact.DeletedBy = contact.DeletedBy;
                            cntContact.DeletedDate = DateTime.Now;
                            _dbContext.Contacts.Update(cntContact);
                            await _dbContext.SaveChangesAsync();
                        }
                        var resultContactLinkFiles = await _dbContext.CntContactFileLinks.Where(x => x.ContactId == contact.ContactId).ToListAsync();
                        if (resultContactLinkFiles.Count() > 0)
                        {
                            foreach (var item in resultContactLinkFiles)
                            {
                                item.IsDeleted = true;
                                item.DeletedBy = contact.DeletedBy;
                                item.DeletedDate = DateTime.Now;
                                _dbContext.CntContactFileLinks.Update(item);
                            }
                            await _dbContext.SaveChangesAsync();
                        }
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }
        #endregion

        public async Task<List<ContactCaseConsultationListVM>> GetCaseListByContactId(Guid contactId)
        {
            try
              {
                if (_contactCaseList == null)
                {
                    string StoredProc = $"exec pCntContactCasetList @ContactId ='{contactId}' ";
                    _contactCaseList = await _dbContext.ContactCaseConsultationListVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
                return _contactCaseList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        } 
        public async Task<List<ContactCaseConsultationListVM>> GetConsultationListByContactId(Guid contactId)
        {
            try
            {
                if (_contactConsultationList == null)
                {
                    string StoredProc = $"exec pCntContactConsultationList @ContactId ='{contactId}' ";
                    _contactConsultationList = await _dbContext.ContactCaseConsultationListVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
                return _contactConsultationList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<ContactCaseConsultationRequestListVM>> GetConsultationRequestListByContactId(Guid contactId)
        {
            try
            {
                if (_contactConsultationRequestList == null)
                {
                    string StoredProc = $"exec pCntContactConsultationRequestList @ContactId ='{contactId}' ";
                    _contactConsultationRequestList = await _dbContext.ContactCaseConsultationRequestListVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
                return _contactConsultationRequestList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<ContactCaseConsultationRequestListVM>> GetCaseRequestListByContactId(Guid contactId)
        {
            try
            {
                if (_contactCaseRequestList == null)
                {
                    string StoredProc = $"exec pCntContactCaseRequestList @ContactId ='{contactId}' ";
                    _contactCaseRequestList = await _dbContext.ContactCaseConsultationRequestListVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
                return _contactCaseRequestList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #region Get Case File List
        public async Task<List<CaseFile>> GetCaseFileList()
        {
            try
            {
                var result = await _dbContext.CaseFiles.OrderBy(u => u.CreatedDate).ToListAsync();
                if (result.Count() != 0)
                {
                    return result;
                }
                return new List<CaseFile>();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<ConsultationFile>> GetConsultationFileList()
        {
            try
            {
                var result = await _dbContext.ConsultationFiles.OrderBy(u => u.CreatedDate).ToListAsync();
                if (result.Count() != 0)
                {
                    return result;
                }
                return new List<ConsultationFile>();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<CaseRequest>> GetCaseRequestList()
        {
            try
            {
                var result = await _dbContext.CaseRequests.OrderBy(u => u.CreatedDate).ToListAsync();
                if (result.Count() != 0)
                {
                    return result;
                }
                return new List<CaseRequest>();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<ConsultationRequest>> GetConsultationRequestList()
        {
            try
            {
                var result = await _dbContext.ConsultationRequests.OrderBy(u => u.CreatedDate).ToListAsync();
                if (result.Count() != 0)
                {
                    return result;
                }
                return new List<ConsultationRequest>();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Create Contact
        public async Task<bool> CreateContact(CntContact args)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        await _dbContext.AddAsync(args);
                        await _dbContext.SaveChangesAsync();
                        if (args.CntContactRequestList.Count() != 0)
                        {
                            await AddContactLinkFileRequest(args.CntContactRequestList, _dbContext);
                        }
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            } 
        }
        private async Task AddContactLinkFileRequest(List<CntContactFileLink> cntContactRequestList, DatabaseContext dbContext)
        {
			dbContext.CntContactFileLinks.RemoveRange(dbContext.CntContactFileLinks.Where(p => p.ContactId == cntContactRequestList.Select(x => x.ContactId).FirstOrDefault()));
			//var result = await dbContext.CntContactRequests.Where(x => x.ReferenceId == cntContactRequestList.Select(x => x.ReferenceId).FirstOrDefault()).ToListAsync();
   //         if (result.Count > 0) 
   //         {
			//	foreach (var item in result)
			//	{
			//		dbContext.CntContactRequests.Remove(item);
			//	}
			//	await dbContext.SaveChangesAsync();
			//}

			foreach (var item in cntContactRequestList)
            {
                await dbContext.CntContactFileLinks.AddAsync(item);
            }
            await dbContext.SaveChangesAsync();
        }


        #endregion

        #region Check Email, phone and civilid
		public async Task<bool> CheckEmailExists(Guid contactId, string email)
		{
			try
			{
                var result = await _dbContext.CntContacts.Where(u => u.Email == email && u.IsDeleted == false && u.ContactId != contactId).FirstOrDefaultAsync();
				if (result != null)
				{
					return true;
				}
				return false;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
		public async Task<bool> CheckPhoneNumberExists(Guid contactId, string phoneNumber)
		{
			try
			{
				var result = await _dbContext.CntContacts.Where(u => u.PhoneNumber == phoneNumber && u.IsDeleted == false && u.ContactId != contactId).FirstOrDefaultAsync();
				if (result != null)
				{
					return true;
				}
				return false;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
		public async Task<bool> CheckCivilIdExists(Guid contactId, string civilId)
		{
			try
			{
				var result = await _dbContext.CntContacts.Where(u => u.CivilId == civilId && u.IsDeleted == false && u.ContactId != contactId).FirstOrDefaultAsync();
				if (result != null)
				{
					return true;
				}
				return false;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
        #endregion

        #region Get contact details by using contactid
        public async Task<CntContact> GetContactDetailByUsingContactId(Guid contactId)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var task = await _dbContext.CntContacts.Where(x => x.ContactId == contactId).FirstOrDefaultAsync();
                        if (task != null)
                        {
                            
                            var resultTags = await GetContactRequestDetails(task.ContactId, _dbContext);
                            if (resultTags.Count() != 0)
                            {
                                task.CntContactRequestList = resultTags;
                            }
                            transaction.Commit();
                            return task;
                        }
                        return new CntContact();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return new CntContact();
                    }
                }
            }
        }

        private async Task<List<CntContactFileLink>> GetContactRequestDetails(Guid contactId, DatabaseContext dbContext)
        {
            try
            {
                var result = await dbContext.CntContactFileLinks.Where(x => x.ContactId == contactId).ToListAsync();
                if (result.Count() != 0)
                {
                    return result;
                }
                return new List<CntContactFileLink>();
            }
            catch (Exception)
            {
                throw;
            }
        }
		#endregion

		#region Update Contact
		public async Task<bool> UpdateContact(CntContact args)
		{
			using (_dbContext)
			{
				using (var transaction = _dbContext.Database.BeginTransaction())
				{
					try
					{
                        var result = await _dbContext.CntContacts.Where(x => x.ContactId == args.ContactId).FirstOrDefaultAsync();
                        if (result != null)
                        {
                            result.ContactTypeId = args.ContactTypeId;
							result.JobRoleId = args.JobRoleId;
							result.SectorId = args.SectorId;
							result.DepartmentId = args.DepartmentId;
							result.FirstName = args.FirstName;
							result.SecondName = args.SecondName;
							result.LastName = args.LastName;
							result.CivilId = args.CivilId;
							result.DOB = args.DOB;
							result.PhoneNumber = args.PhoneNumber;
							result.Email = args.Email;
							result.Notes = args.Notes;
							result.CreatedBy = args.CreatedBy;
							result.CreatedDate = args.CreatedDate;
							result.ModifiedBy = args.ModifiedBy;
							result.ModifiedDate = args.ModifiedDate;
							result.DeletedBy = args.DeletedBy;
							result.DeletedDate = args.DeletedDate;
							result.IsDeleted = args.IsDeleted;

							_dbContext.Entry(result).State = EntityState.Modified;
							await _dbContext.SaveChangesAsync();
						}
                        else
                        {
							await _dbContext.AddAsync(args);
							await _dbContext.SaveChangesAsync();
						}
						
						if (args.CntContactRequestList.Count() != 0)
						{
							await AddContactLinkFileRequest(args.CntContactRequestList, _dbContext);
						}
						transaction.Commit();
						return true;
					}
					catch (Exception)
					{
						transaction.Rollback();
						return false;
					}
				}
			}
		}
		#endregion
	}
}