using FATWA_DOMAIN.Models.BugReporting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.BugReportingVMs
{
    public class BugTicketCommentVM
    {
        public Guid Id { get; set; }
        public Guid ReferenceId { get; set; }
        public string? BugId { get; set; }
        public string? CommenterNameEn { get; set; }
        public string? CommenterNameAr { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? Comment { get; set; }
        public bool IsDeleted { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public Guid? ParentCommentId { get; set; }
        public string? CreatedBy { get; set; }
        public int? Rating { get; set; }
        [NotMapped]
        public bool FromFatwa { get; set; } = false;
        [NotMapped]
        public int TicketStatusId { get; set; }
    }
}
