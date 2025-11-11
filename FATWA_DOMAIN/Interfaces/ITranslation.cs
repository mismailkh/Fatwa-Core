using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.ViewModel.TranslationMobileAppVMs;

namespace FATWA_DOMAIN.Interfaces
{
    public interface ITranslation
    {
        Task<List<Translation>> GetTranslations();
        Task<TranslationLabelsVM> GetTranslationsByCultureValue(string cultureValue, int channelId, string versionCode);
        List<Translation> GetTranslationsSync();
        Task UpdateTranslation(Translation Translation);
        Task DeleteTranslation(int Id);

    }
}
