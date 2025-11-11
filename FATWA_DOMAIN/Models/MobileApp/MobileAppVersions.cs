using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.MobileApp
{
    [Table("UMS_USER_APP_VERSION")]
    public class MobileAppVersions
    {
        [Key]
        public int Id { get; set; }
        public int ChannelId { get; set; }
        public string VersionCode { get; set; }
    }
}
