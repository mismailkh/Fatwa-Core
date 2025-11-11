using FATWA_DOMAIN.Models;

namespace FATWA_DOMAIN.Interfaces
{
    //<History Author = 'Umer Zaman' Date='2022-03-15' Version="1.0" Branch="own"> Extract Interface from parent class & add functions</History>
    public interface ILiteratureTypes
    {
        Task CreateLmsLiteratureType(LmsLiteratureType lmsLiteratureType);
        Task<int> DeleteLmsLiteratureType(int id);
        Task<LmsLiteratureType> GetLiteratureTypeDetails(int id);
        Task<List<LmsLiteratureType>> GetLmsLiteratureTypes();
        List<LmsLiteratureType> GetLmsLiteratureTypesSync();
        Task UpdateLmsLiteratureType(LmsLiteratureType lmsLiterature);
    }
}
