using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.Lms
{
    public class StockTakingAdvancedSearchVM:TransactionalBaseModel
    {
        public Guid Id { get; set; }
        public DateTime StockTakingDate { get; set; }
        public int? StatusId { get; set; }
        public int? TotalBooks { get; set; }
        public string? Note { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]

        public DateTime? FromDate { get; set; } = null;
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]

        public DateTime? ToDate { get; set; } = null;
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]

        public DateTime? StockTakingFromDate { get; set; } = null;
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]

        public DateTime? StockTakingToDate { get; set; } = null;
        public int? PageSize { get; set; } 
        public int? PageNumber { get; set; } = 1;
        public bool isDataSorted { get; set; }
        public bool isGridLoaded { get; set; }
        public bool isPageSizeChangeOnFirstLastPage { get; set; }
    }
}
