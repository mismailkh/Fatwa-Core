using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.ServiceRequestModels.LeaveAndAttendanceRequestModels
{
    //[Table("LA_REASON_LKP")] // OSS DB
    public class Reason
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReasonId { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
    }
}
