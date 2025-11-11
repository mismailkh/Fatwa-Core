using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.CommunicationModels
{

    [Table("COMM_COMMUNICATION_RESPONSE_GOVERNMENT_ENTITY")]
    public partial class CommunicationResponseGovtEntity
    {
        [Key]
        public int ResponseGovernmentEntityId { get; set; }
        //ForeignKey from COMM_COMMUNICATION_RESPONSE 
        public Guid CommunicationResponseId { get; set; }
        //ForeignKey from CMS_GOVERNMENT_ENTITY_G2G_LKP
        public int EntityId { get; set; }
    }
}
