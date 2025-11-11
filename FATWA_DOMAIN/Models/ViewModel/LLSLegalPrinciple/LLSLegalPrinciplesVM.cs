using FATWA_DOMAIN.Models.BaseModels;
using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.ViewModel.LLSLegalPrinciple
{
    public class LLSLegalPrinciplesVM
    {
        [Key]
        public Guid PrincipleId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string PrincipleContent { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedByEn { get; set; }
        public string CreatedByAr { get; set; }
		public int? PrincipleSourceDocumentTypeId { get; set; }
		[NotMapped]
		public string? DeletedBy { get; set; }
	}
	public class LLSLegalPrinciplReferenceVM
	{
		[Key]
		public Guid PrincipleId { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public string? PrincipleContent { get; set; }
		public DateTime CreatedDate { get; set; }
		public string CreatedByEn { get; set; }
		public string CreatedByAr { get; set; }
		public int PageNumber { get; set; }
	}

    public class LLSLegalPrincipleAdvanceSearchVM : GridPagination
    {
        public int? FlowStatusId { get; set; }
        public int? PrincipleNumber { get; set; }   
        public string? UserId { get; set; }
        public int? flowStatusAS { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public bool IsPublishUnPublish { get; set; }
    }
}
