using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.AdminVM.Lookups
{
    public class CourtDetailVM : TransactionalBaseModel
    {
        [Key]
        public int Id { get; set; }
        public int TypeId { get; set; }
        public string Name_En { get; set; }
        public string Name_Ar { get; set; }
        public string District { get; set; }
        public string Location { get; set; }
        public string Address { get; set; }
        public string CourtTypeEn { get; set; }
        public string CourtTypeAr { get; set; }
        public string CourtCode { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string? Description { get; set; }
        public string? UserFullNameEn { get; set; }
        public string? UserFullNameAr { get; set; }

    }
}
