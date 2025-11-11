using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models
{
	//<History Author = 'Umer Zaman' Date='2022-07-11' Version="1.0" Branch="own">Create class model & add properties</History>  
	[Table("LEGAL_LEGISLATION_ARTICLE_EFFECT_HISTORY", Schema = "dbo")]
	public class LegalLegislationArticleEffectHistory
	{
		[Key]
		public Guid Id { get; set; }
		public Guid LegislationId { get; set; }
		public Guid ArticleId { get; set; }
		public int ArticleStatus { get; set; }
		public string Note { get; set; }
		public DateTime CreatedDate { get; set; }
	}
}
