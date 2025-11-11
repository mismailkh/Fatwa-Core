using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//< History Author = 'Muhammad Zaeem' Date = '2023-1-2' Version = "1.0" Branch = "master" >Consultation article model</History>

namespace FATWA_DOMAIN.Models.Consultation
{
    [Table("COMS_CONSULTATION_ARTICLE")]
    public partial class ConsultationArticle
    {
        [Key]
        public Guid ArticleId { get; set; }
        public Guid ConsultationRequestId { get; set; }
        public Guid? SectionId { get; set; } = Guid.Empty;
        public int ArticleNumber { get; set; }
        public string ArticleTitle { get; set; }
        public int ArticleStatusId { get; set; }
        public string ArticleText { get; set; }
        [NotMapped]
        public string? ShowSectionTitle { get; set; }
        [NotMapped]
        public string? ShowSectionTitleAr { get; set; }
        [NotMapped]
        public string? Article_Status_Name_En { get; set; }
        [NotMapped]
        public string? Article_Status_Name_Ar { get; set; }
        [NotMapped]
        public bool IsDetailView { get; set; } = false;
    }
}
