using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.CaseManagment
{
    public class CmsDraftTemplateVersionLogVM
    {
        [Key]
        public Guid Id { get; set; }
        public string ActionNameEn { get; set; }
        public string ActionNameAr { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string UserNameEn { get; set; }
        public string UserNameAr { get; set; }
        public string ReviewerUserNameEn { get; set; }
        public string ReviewerUserNameAr { get; set; }
    }
}
