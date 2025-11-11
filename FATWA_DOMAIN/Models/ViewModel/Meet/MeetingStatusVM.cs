using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.Meet
{
    public class MeetingStatusVM
    {
        public Guid MeetingId { get; set; }
        public int MeetingStatusId { get; set; }
        //[NotMapped]
        //public bool? IsHeld { get; set; }


    }
}
