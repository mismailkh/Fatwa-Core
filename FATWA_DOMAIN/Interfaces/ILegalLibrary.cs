using FATWA_DOMAIN.Models.ViewModel;

namespace FATWA_DOMAIN.Interfaces
{
    public interface ILegalLibrary
    {
        Task<LegalLibraryVM> SearchLegalLibrary();
    }
}
