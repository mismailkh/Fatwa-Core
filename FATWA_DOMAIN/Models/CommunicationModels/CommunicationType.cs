using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.CommunicationModels
{
    [Table("COMM_COMMUNICATION_TYPE")]
    public partial class CommunicationType :TransactionalBaseModel
    {
        [Key]
        public int CommunicationTypeId { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsActive { get; set; }

    }
}
