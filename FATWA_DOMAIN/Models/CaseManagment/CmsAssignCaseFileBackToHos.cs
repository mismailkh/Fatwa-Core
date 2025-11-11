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
    //< History Author = 'Ijaz Ahmad' Date = '2023-04-17' Version = "1.0" Branch = "master">Assigned Case File Back TO HOS</History>
    [Table("CMS_ASSIGN_CASEFILE_BACKTO_HOS")]
    public class CmsAssignCaseFileBackToHos : TransactionalBaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public Guid ReferenceId { get; set; }
        public string Remarks { get; set; }
        [NotMapped]
        public int? SectorTypeId { get; set; }
        [NotMapped]
        public string TaskUserId { get; set; }
        [NotMapped]
        public Guid TaskId { get; set; }
        [NotMapped]
        public NotificationParameter NotificationParameter { get; set; } = new NotificationParameter();

    }
}
