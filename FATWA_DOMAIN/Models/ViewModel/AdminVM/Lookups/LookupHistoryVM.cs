using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.AdminVM.Lookups
{
    public class LookupHistoryVM
    {
        [Key]
        public int Id { get; set; }
        public int ReferenceId { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool? IsDeleted { get; set; }
        public string? Description { get; set; }
    }
}
