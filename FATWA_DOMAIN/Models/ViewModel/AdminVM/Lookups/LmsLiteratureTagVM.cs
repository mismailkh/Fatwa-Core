using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.AdminVM.Lookups
{
    public class LmsLiteratureTagVM : TransactionalBaseModel
    {
        [Key]
        public int Id { get; set; }
        public string TagNo { get; set; }
        public string Description { get; set; }
        public string Description_Ar { get; set; }
        public bool Active { get; set; }
        public bool IsActive { get; set; }
        public string? UserFullNameEn { get; set; }
        public string? UserFullNameAr { get; set; }

    }
}
