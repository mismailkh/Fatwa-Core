using FATWA_DOMAIN.Models.ViewModel.CMSCOMSRequestNumberVMs;

namespace FATWA_DOMAIN.Interfaces.PatternNumber
{
    public interface ICMSCOMSInboxOutboxRequestPatternNumber
    {
        Task<NumberPatternResult> GenerateNumberPattern(int govtEntityId, int NumberTypeId, dynamic dbContext = null);
    }
}
