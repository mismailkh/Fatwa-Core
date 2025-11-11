using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel
{
    public class LmsLiteratureParentIndexVM
    {
        [Key]
        public int? ParentIndexId
        {
            get;
            set;
        }
        public string? IndexParentNumber
        {
            get;
            set;
        }
        public string? Parent_Name_En
        {
            get;
            set;
        }
        public string? Parent_Name_Ar
        {
            get;
            set;
        }
    }
}
