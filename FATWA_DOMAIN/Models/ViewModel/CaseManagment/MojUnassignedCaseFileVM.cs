using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FATWA_DOMAIN.Models.Notifications.ViewModel;

namespace FATWA_DOMAIN.Models.ViewModel.CaseManagment
{
    //<History Author = 'Hassan Abbas' Date='2024-02-29' Version="1.0" Branch="master"> VM for Unassigned Case Filesmigrated from MOJ </History>
    public class MojUnassignedCaseFileVM : TransactionalBaseModel
    {
        [Key]
        //public Guid CaseId { get; set; }
        public Guid FileId { get; set; }
        public Guid RequestId { get; set; }
        public int StatusId { get; set; }
        public string FileNumber { get; set; }
        public string? Subject { get; set; }
        public string? FileName { get; set; }
        public DateTime? RequestDate { get; set; }
        public string? RequestNumber { get; set; }
        public string? StatusNameEn { get; set; }
        public string? StatusNameAr { get; set; }
        public string? GovernmentEntityNameEn { get; set; }
        public string? GovernmentEntityNameAr { get; set; }
        public string? PriorityNameEn { get; set; }
        public string? PriorityNameAr { get; set; }
        public bool? IsAssignedBack { get; set; }
        public string? CaseFileNumberFormat { get; set; }
        public int TotalCount { get; set; }
        [NotMapped]
        public IList<CmsRegisteredCaseFileDetailVM> RegisteredCases { get; set; } = new List<CmsRegisteredCaseFileDetailVM>();
    }

    public class AssignMojCaseFileToSectorVM
    {
        public int SectorTypeId { get; set; }
        public int RequestTypeId { get; set; }
        public List<Guid> FileIds { get; set; }
        [NotMapped]
        public NotificationParameter NotificationParameter { get; set; } = new NotificationParameter();
    }
}
