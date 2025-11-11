using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.DmsVMs
{
    public class DocumentListAdvanceSearchVM : GridPagination
    {
        public string? Filename { get; set; }
        public DateTime? CreatedFrom { get; set; }
        public DateTime? CreatedTo { get; set; }
        public int? AttachmentTypeId { get; set; }
        public bool isFavourite { get; set; }
        public string? UserId { get; set; }
        public string? RoleId { get; set; }
       public int? SectorTypeId { get; set; }
        public string? CreatedBy { get; set; }


    }
}
