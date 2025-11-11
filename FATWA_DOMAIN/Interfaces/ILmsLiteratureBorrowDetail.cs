using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.Lms;

namespace FATWA_DOMAIN.Interfaces
{
    public interface ILmsLiteratureBorrowDetail
    {
        Task<List<BorrowDetailVM>> GetLmsLiteratureBorrowDetails(UserDetailVM? loggedUser);
        Task<List<ReturnDetailVM>> GetLmsLiteratureReturnDetails(UserDetailVM? loggedUser);
        Task<int?> CreateLmsLiteratureBorrowDetail(LmsLiteratureBorrowDetail LmsLiteratureBorrowDetail);
        Task<LmsLiteratureBorrowDetail> GetLmsLiteratureBorrowDetailById(int Id);
        Task UpdateLmsLiteratureBorrowDetail(LmsLiteratureBorrowDetail LmsLiteratureBorrowDetail);
        Task UpdateLmsLiteratureRetunDetail(BorrowDetailVM LmsLiteratureBorrowDetail);
        Task<bool> DeleteLiteratureBorrow(BorrowDetailVM item);
        Task<IEnumerable<LiteratureBorrowApprovalType>> GetLiteratureBorrowApprovalTypes();
        Task UpdateLiteratureBorrowApprovalStatus(LmsLiteratureBorrowDetail LmsLiteratureBorrowDetail);
        Task<List<BorrowDetailVM>> GetLmsLiteratureBorrowExtensionApprovals();
        Task<List<BorrowDetailVM>> GetLiteratureBorrowApprovals();
        Task<List<LiteratureBorrowApprovalType>> GetBorrowApprovalStatusDetails();
        Task<UserAndLiteratureVM> GetBorrowedLiteratureAndUserDetailByUserIdAndCivilId(string? UserId, string? civilId);
        Task UpdateLiteratureReturnExtendDetail(BorrowedLiteratureVM LmsLiteratureBorrowDetail);
        Task<BorrowedLiteratureVM> GetLiteratureByBarcode(string BarCode);
  Task<List<UserBorrowedHistoryVM>> GetUserBorrowHistoryByUserId(string UserId);
        Task<List<AllLmsUserDetailVM>> GetAllLmsUserList();
        Task<List<BorrowedLiteratureVM>> GetLmsBorrowLiteraturesAdvanceSearch(LiteratureAdvancedSearchVM advancedSearch);


    }

}
