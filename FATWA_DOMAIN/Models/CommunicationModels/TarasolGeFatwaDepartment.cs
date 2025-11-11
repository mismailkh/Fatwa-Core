using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FATWA_DOMAIN.Models.CommunicationModels
{
    [Table("TARASOL_GE_FATWA_DEPARTMENTS")]

    public class TarasolGeFatwaDepartment 
    {
        public int Id { get; set; } 
        public int G2GSiteId { get; set; } 
        public int G2GBrSiteId { get; set; } 
        public string? G2GBrSiteNameEn {  get; set; }   
        public string? G2GBrSiteNameAr {  get; set; }   

    }


}
