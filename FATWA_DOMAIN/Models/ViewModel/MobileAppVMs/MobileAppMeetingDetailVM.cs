using FATWA_DOMAIN.Models.Notifications.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.MobileAppVMs
{
    public class MobileAppMeetingDetailVM
    {
        [DisplayName("FileNumber")]
        public string? FileNumber { get; set; }
        [DisplayName("Date")]
        public DateTime? Date { get; set; }
        [DisplayName("Description")]
        public string? Description { get; set; }
        [DisplayName("Subject")]
        public string? Subject { get; set; }
        [DisplayName("Agenda")]
        public string? Agenda { get; set; }
        [DisplayName("Location")]
        public string? Location { get; set; }
        [DisplayName("StartTime")]
        public DateTime? StartTime { get; set; }
        [DisplayName("EndTime")]
        public DateTime? EndTime { get; set; }
        [DisplayName("Type")]
        public string? MeetingType { get; set; }
        [DisplayName("Link")]
        public string? MeetingLink { get; set; }
        [DisplayName("Status")]
        public string? MeetingStatus { get; set; }
    }
}
