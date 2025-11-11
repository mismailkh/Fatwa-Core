using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.AdminVM.Lookups
{
    public class CommunicationTypeVM : TransactionalBaseModel
    {
        [Key]
        public int CommunicationTypeId { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UserFullNameEn { get; set; }
        public string UserFullNameAr { get; set; }
    }
}
