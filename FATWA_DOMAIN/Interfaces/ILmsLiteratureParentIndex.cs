using FATWA_DOMAIN.Models;

namespace FATWA_DOMAIN.Interfaces
{
    //<!-- <History Author = 'Umer Zaman' Date='2022-07-06' Version="1.0" Branch="master">extract interface</History> -->
    public interface ILmsLiteratureParentIndex
    {
        Task CreateLmsLiteratureParentIndex(LmsLiteratureParentIndex lmsLiteratureParentIndex);
        Task SoftDeleteLiteratureParentIndex(LmsLiteratureParentIndex UniqueLiteratureParentIndex);
        Task<LmsLiteratureParentIndex> GetLmsLiteratureParentIndexDetailByNumber(string parentIndexNumber);
        Task<LmsLiteratureParentIndex> GetLiteratureParentIndexDetailById(int parentIndexId);
        bool CheckLiteratureParentIndexByUsingParentIndexNumber(string parentIndexNumber, string name_En, string name_Ar);
        bool CheckLiteratureParentIndexByUsingParentNumber(string parentIndexNumber);
        Task<List<LmsLiteratureParentIndex>> GetLmsLiteratureParentIndexs();
        List<LmsLiteratureParentIndex> GetLmsLiteratureParentIndexSync();
        Task SoftDeleteLmsLiteratureParentIndex(LmsLiteratureParentIndex lmsLiteratureParentIndex);
        Task UpdateLmsLiteratureParentIndex(LmsLiteratureParentIndex lmsLiteratureParentIndex);
        Task<string> GetUserIdByName(string lmsLiteratureParentIndex);
    }
}
