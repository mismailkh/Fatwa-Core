using FATWA_DOMAIN.Models.BaseModels;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//< History Author = ijaz Ahmad' Date = '2022-09-28' Version = "1.0" Branch = "master" >Save & Close Case Model</History>

namespace FATWA_DOMAIN.Models.CaseManagment
{
    [Table("CMS_CLOSE_CASE")]
    public class CmsSaveCloseCaseFile : TransactionalBaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public Guid CaseId { get; set; }
        public int? ResponseTypeId { get; set; }
        public DateTime? RequestDate { get; set; } = DateTime.Now;
        public DateTime? DueDate { get; set; } = DateTime.Now;
        public string? Reason { get; set; }
        public string? Other { get; set; }  
       public bool IsUrgent { get; set; }

        #region Foreign Keys
        public int? EntityId { get; set; }
        public int? PriorityId { get; set; }
        public int? FrequencyId { get; set; }

        #endregion

    }
}
