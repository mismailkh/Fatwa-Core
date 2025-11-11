using System.ComponentModel.DataAnnotations;

namespace FATWA_DOMAIN.Models.ServiceRequestModels
{
    public class SRPriority
    {
        public int Id { get; set; }
        public string NameEn { get; set; } = null!;
        public string NameAr { get; set; } = null!;
    }
}
