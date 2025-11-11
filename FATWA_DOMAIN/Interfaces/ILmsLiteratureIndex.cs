using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.ViewModel;

namespace FATWA_DOMAIN.Interfaces
{
    //<!-- <History Author = 'Umer Zaman' Date='2022-03-25' Version="1.0" Branch="master">update interface</History> -->
    public interface ILmsLiteratureIndex
    {
        Task<List<LmsLiteratureIndex>> GetLmsLiteratureIndexs();
        List<LmsLiteratureIndex> GetLmsLiteratureIndexSync();
        Task CreateLmsLiteratureIndex(LmsLiteratureIndex lmsLiteratureIndex);
        Task<LmsLiteratureIndex> GetLiteratureIndexDetail(int indexId);
        Task UpdateLmsLiteratureIndex(LmsLiteratureIndex lmsLiteratureIndex);
        Task DeleteLmsLiteratureIndex(LmsLiteratureIndex UniqueLiteratureIndex);
        Task<List<LmsLiteratureIndex>> GetLmsLiteratureIndexesIdByIndexNumber(string indexNumber);
        Task<List<LmsLiteratureIndex>> GetLmsLiteratureIndexDetailByUsingNameAndIndexNumber(string indexNumber, string name_en, string name_ar);
        Task<List<LmsLiteratureIndex>> GetLmsLiteratureIndexDetilsByUsingParentIdAndNumber(int parentIndexId, string parentIndexNumber);
        Task<LmsLiteratureIndex> GetLiteratureIndexDetailByUsingIndexId(int indexId);
        Task<List<LmsLiteratureIndexDivisionAisle>> GetLmsLiteratureIndexDivisions(int indexId);
        Task<List<LmsLiteratureIndex>> GetLmsLiteratureIndexNumberDetailsForAddDivision(string indexNumber);
        List<LmsLiterature> CheckLmsLiteratureIndexIdAssociatedWithLiteratures(int indexId);
        Task<LmsLiteratureParentIndexVM> GetLiteratureIndexByIndexIdAndNumber(int indexId, string indexNumber);
    }
}
