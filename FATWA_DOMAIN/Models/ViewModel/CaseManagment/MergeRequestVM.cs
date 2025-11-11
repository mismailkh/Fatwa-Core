using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.CaseManagment
{
    //<History Author = 'Hassan Abbas' Date='2022-11-29' Version="1.0" Branch="master"> Merge Cases Request View Model</History>
    public class MergeRequestVM
    {
        [Key]
        public Guid Id { get; set; }
        public Guid PrimaryId { get; set; }
        public int StatusId { get; set; }
        public string Reason { get; set; }
        public bool IsMergeTypeCase { get; set; }
        public DateTime CreatedDate { get; set; }
        public string PrimaryCaseNumber { get; set; }
        public string PrimaryCANNumber { get; set; }
    }
}
