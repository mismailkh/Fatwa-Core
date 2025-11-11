using FATWA_DOMAIN.Models.BaseModels;
using FATWA_DOMAIN.Models.Notifications.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.CaseManagment
{
    [Table("CMS_CASE_FILE_TRANSFER_REQUEST")]
    public class CmsCaseFileTranferRequest : TransactionalBaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public string RequestNo { get; set; }
        public int SectorFrom { get; set; }
        public int SectorTo { get; set; }
        public string? Description { get; set; }
        public int StatusId { get; set; }
        [NotMapped]
        public NotificationParameter NotificationParameter { get; set; } = new NotificationParameter();
    }
}
