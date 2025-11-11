using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel
{
    public class AdvanceSearchCmsCaseRequestVM : GridPagination
    {
        public string? RequestNumber { get; set; }
        public int? StatusId { get; set; }
        public string? Subject { get; set; }
        public int? SectorTypeId { get; set; }
        public DateTime? RequestFrom { get; set; }
        public DateTime? RequestTo { get; set; }
        public bool ShowUndefinedRequest { get; set; }
        public int? GovEntityId { get; set; }
        public string? CreatedBy { get; set; }
    }
}
