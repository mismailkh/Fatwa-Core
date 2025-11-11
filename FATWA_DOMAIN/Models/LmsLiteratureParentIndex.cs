using FATWA_DOMAIN.Models.BaseModels;
using FATWA_DOMAIN.Models.Notifications.ViewModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models
{
    //<History Author = 'Umer Zaman' Date='2022-07-06' Version="1.0" Branch="own">add properties</History>

    [Table("LMS_LITERATURE_PARENT_INDEX")]
    public partial class LmsLiteratureParentIndex
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ParentIndexId
        {
            get;
            set;
        }
        public string Name_En
        {
            get;
            set;
        }
        public string Name_Ar
        {
            get;
            set;
        }
        public string ParentIndexNumber
        {
            get;
            set;
        }
        public string CreatedBy
        {
            get;
            set;
        }
        public DateTime CreatedDate
        {
            get;
            set;
        }
        public string? ModifiedBy
        {
            get;
            set;
        }
        public DateTime? ModifiedDate
        {
            get;
            set;
        }
        public string? DeletedBy
        {
            get;
            set;
        }
        public DateTime? DeletedDate
        {
            get;
            set;
        }
        public bool IsDeleted
        {
            get;
            set;
        }
        [NotMapped]
        public NotificationParameter NotificationParameter { get; set; }   = new NotificationParameter();
    }
}
