using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;


namespace FATWA_DOMAIN.Interfaces.Common
{
    public interface ICommonRepo
    {
        Task<bool> UpdateEntityStatus(UpdateEntityStatusVM updateEntity);
        Task<List<CmsComsNumPatternVM>> GetAllCaseRequestNumber();
    }
}
