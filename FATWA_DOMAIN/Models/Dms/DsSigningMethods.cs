using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace FATWA_DOMAIN.Models.Dms
{
    [Table("DS_SIGNING_METHODS")]
    public class DsSigningMethods
    {
        [Key]
        public int MethodId { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public string SignatureProfileName { get; set; }
        public bool IsActive { get; set; }
    }
}
