using FATWA_DOMAIN.Models.BaseModels;
using FATWA_DOMAIN.Models.Notifications.ViewModel;
using FATWA_DOMAIN.Models.ViewModel;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.Contact
{
    [Table("CNT_CONTACT")]
    public class CntContact : TransactionalBaseModel
    {
        [Key]
        public Guid ContactId { get; set; }
        public int ContactTypeId { get; set; }
        public string? JobRoleId { get; set; }
        public int? SectorId { get; set; }  
        public int? DepartmentId { get; set; }  // will remove // same as SectorId
		public string FirstName { get; set; }  
        public string? SecondName { get; set; }  
        public string LastName { get; set; }   
        public string? CivilId { get; set; } // will remove
        public DateTime? DOB { get; set; } // will remove
        public string PhoneNumber { get; set; }        
        public string? Email { get; set; }
        public string? Notes { get; set; }
        public int? WorkPlace { get; set; }
        public int? Designation { get; set; } // same as JobRoleId

		[NotMapped]
		public ObservableCollection<TempAttachementVM>? MandatoryTempFiles { get; set; } = new ObservableCollection<TempAttachementVM>();
		[NotMapped]
		public List<int>? DeletedAttachementIds { get; set; } = new List<int>();
        [NotMapped]
        public List<CntContactFileLink>? CntContactRequestList { get; set; } = new List<CntContactFileLink>();
        [NotMapped]
        public NotificationParameter NotificationParameter { get; set; } = new NotificationParameter();

    }
}
