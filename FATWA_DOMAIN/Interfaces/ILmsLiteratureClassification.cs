using FATWA_DOMAIN.Models;

namespace FATWA_DOMAIN.Interfaces
{
    public interface ILmsLiteratureClassification
    {
        Task<List<LmsLiteratureClassification>> GetLmsLiteratureClassifications();
        List<LmsLiteratureClassification> GetLmsLiteratureClassificationsSync();
        Task CreateLmsLiteratureClassification(LmsLiteratureClassification LmsLiteratureClassification);
        Task<LmsLiteratureClassification> GetLiteratureClassificationDetailById(int Id);
        Task UpdateLmsLiteratureClassification(LmsLiteratureClassification LmsLiteratureClassification);
        Task<int> DeleteLmsLiteratureClassification(int Id);
    }
}
