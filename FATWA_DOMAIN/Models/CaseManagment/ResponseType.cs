using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.CaseManagment
{
    [Table("CMS_Response_Type")]
    public class ResponseType
    {
        [Key]   
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } 
        public string Name_En { get; set; } 
        public string Name_Ar { get; set; } 
    }
}
