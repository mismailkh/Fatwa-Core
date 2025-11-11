using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.Consultation
{
    //< History Author = 'Muhammad Zaeem' Date = '2023-1-12' Version = "1.0" Branch = "master" >Model for Assignment of Sectors to a Case File</History>
    [Table("COMS_CONSULTATION_FILE_SECTOR_ASSIGNMENT")]
    public class ComsConsultationFileSectorAssignment : TransactionalBaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public Guid FileId { get; set; }
        public int SectorTypeId { get; set; }
    }
}
