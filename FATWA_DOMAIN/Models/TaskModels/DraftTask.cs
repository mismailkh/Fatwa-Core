using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FATWA_DOMAIN.Models.TaskModels
{
    [Table("TSK_Draft")]
    public class DraftTask : TransactionalBaseModel
    {
        [Key]
        public Guid DraftId { get; set; }  
        public int DraftNumber { get; set; }
        public int Status { get; set; }
        public string Name { get; set; }
        public string FileNumber { get; set; }
        #region foriegn Keys
        public int StatementType { get; set; }
        public Guid TaskId { get; set; }
        #endregion

    }
}

