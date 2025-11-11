using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;

namespace FATWA_DOMAIN.Interfaces
{
    //<History Author = 'Muhammad Zaeem' Date='2022-09-29' Version="1.0" Branch="master"> Interface of UMS CLAIMS</History>

    public interface IClaims
    {
        Task<List<ClaimUms>> GetClaimUms();
        List<ClaimUms> GetClaimUmsSync();
        Task<ClaimUms> GetClaimsById(int Id);

        Task CreateClaims(ClaimUms ClaimUms);

        Task UpdateClaims(ClaimUms ClaimUms);
        Task DeleteClaims(int Id);

    }
}
