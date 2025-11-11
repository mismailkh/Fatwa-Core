using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.BugReportingVMs
{
    public  class CrashReportListVM
    {
        public Guid? Id { get; set; }
        public Guid? TicketId { get; set; }
        public string? BugNumber { get; set; }   
        public string? ScreenReference { get; set; }   
        public string? Description { get; set; }   
        public string? CreatedBy { get; set; }   
        public DateTime CreatedDate { get; set; }   
        public string? ApplicationEn { get; set; }   
        public string? ApplicationAr { get; set; }  
        public string? ModuleEn { get; set; }   
        public string? ModuleAr { get; set; }   
        public string? TypeEn { get; set; }   
        public string? TypeAr { get; set; }   
        public string? UserNameEn { get; set; }   
        public string? UserNameAr { get; set; }   
        public bool IsTicket { get; set; }   
    }
}
