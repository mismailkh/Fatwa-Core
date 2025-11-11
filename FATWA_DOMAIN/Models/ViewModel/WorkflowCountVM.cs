using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel
{
    public class WorkflowCountVM
    {
        public int Draft { get; set; }
        public int InReview { get; set; }
        public int Published { get; set; }
        public int Active { get; set; }
        public int Suspended { get; set; }
        public int Inactive { get; set; }
        public int Deleted { get; set; }
    }
    public class WorkflowInstanceCountVM
    {
        public int InProgress { get; set; }
        public int Failed { get; set; }
        public int Expired { get; set; }
        public int Success { get; set; }
        public int Cancel { get; set; }
        public int Total { get; set; }
    }
}
