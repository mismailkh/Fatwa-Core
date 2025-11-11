using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.AdminModels.UserManagement
{
    //<History Author = 'Umer Zaman' Date='2022-07-26' Version="1.0" Branch="master"> Create model for transfering user</History>
    [Table("UMS_TRANSFER_USER", Schema = "dbo")]
    public class TransferUser
    {
        [Key]
        public Guid TransferId { get; set; }
        public string Id { get; set; }
        public int Previous_DepartmentId { get; set; }
        public int Current_DepartmentId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool IsDeleted { get; set; }


        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [NotMapped]
        public DateTime? TransferStartDate { get; set; } = null;

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [NotMapped]
        public DateTime? TransferEndDate { get; set; } = null;
        [NotMapped]
        public DateTime Max = new DateTime(DateTime.Now.Date.Ticks);
        [NotMapped]
        //public DateTime Min = new DateTime(1950, 1, 1);
        public DateTime Min = new DateTime(DateTime.Now.Date.Ticks);
    }
}
