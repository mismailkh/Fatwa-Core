using FATWA_DOMAIN.Models.TaskModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace FATWA_DOMAIN.Models.ViewModel.MobileAppVMs
{
    public class MobileAppCaseFileTransferRequestVM
    {
        [Key]
        public Guid Id { get; set; }
        [DisplayName("Sector_From")]
        public string? SectorFromName { get; set; }
        [DisplayName("Sector_To")]
        public string? SectorToName { get; set; }
        [DisplayName("Status")]
        public string? StatusName { get; set; }
        [DisplayName("Request_Date")]
        public DateTime? CreatedDate { get; set; }
        [DisplayName("Created_By")]
        public string? CreatedBy { get; set; }
        [DisplayName("Modified_Date")]
        public DateTime? ModifiedDate { get; set; }
        [DisplayName("Description")]
        public string? Description { get; set; }
        public int? StatusId { get; set; }
    }
}
