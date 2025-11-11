using FATWA_DOMAIN.Models;

namespace FATWA_DOMAIN.Interfaces
{
    //<!-- <History Author = 'Umer Zaman' Date='2022-07-08' Version="1.0" Branch="master">update interface</History> -->
    public interface ILmsLiteratureIndexDivisionAisle
    {
        Task<List<LmsLiteratureIndexDivisionAisle>> GetLmsLiteratureIndexDivisions();
        List<LmsLiteratureIndexDivisionAisle> GetLmsLiteratureIndexDivisionsSync();
        Task<LmsLiteratureIndexDivisionAisle> GetLiteratureIndexDivisionDetail(int divisionAisleId);
        Task<List<LmsLiteratureIndexDivisionAisle>> GetLmsLiteratureDivisionDetailsByUsingIndexId(int indexId);
        Task<List<LmsLiteratureIndexDivisionAisle>> GetLmsLiteratureDivisionDetailsByUsingIndexIdForViewPage(int indexId);
        Task<List<LmsLiteratureIndexDivisionAisle>> GetDivisionDetailsByUsingIndexAndDivisionId(int divisionAisleId, int indexId);
        Task<List<LmsLiterature>> GetLmsLiteratureDivisionDetailByUsingDivisionId(int divisionAisleId);
        Task CreateLmsLiteratureIndexDivision(LmsLiteratureIndexDivisionAisle lmsLiteratureIndexDivision);
        Task UpdateLmsLiteratureIndexDivision(LmsLiteratureIndexDivisionAisle lmsLiteratureIndexDivision);
        Task DeleteLmsLiteratureIndexDivisionAisle(int divisionAisleId);
        Task<List<LmsLiteratureIndexDivisionAisle>> GetLmsLiteratureAisleNumberDetailsByUsingIndexAndDivisionNumber(int indexId, string divisionNumber);
        //List<LmsLiterature> CheckLmsLiteratureDivisionAisleIdAssociatedWithLiterature(int divisionAisleId);
    }
}
