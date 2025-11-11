using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.BaseModels
{
    //< History Author = 'Hassan Abbas' Date = '2022-09-28' Version = "1.0" Branch = "master" >Created Base Model for transactional columns</History>
    public class TransactionalBaseModel
    {
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
