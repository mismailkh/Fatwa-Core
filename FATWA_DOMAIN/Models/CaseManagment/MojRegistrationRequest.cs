using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.CaseManagment
{
    //< History Author = 'Hassan Abbas' Date = '2022-11-15' Version = "1.0" Branch = "master" >Model for Moj Registration Request</History>
    [Table("CMS_MOJ_REGISTRATION_REQUEST")]
    public class MojRegistrationRequest : TransactionalBaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public Guid FileId { get; set; }
        public string? MessengerId { get; set; }
        public bool IsRegistered { get; set; } = false;
        public int SectorTypeId { get; set; }
        public int DocumentId { get; set; }
    }
}
