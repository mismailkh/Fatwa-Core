using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//< History Author = 'Umer Zaman' Date = '2022-08-18' Version = "1.0" Branch = "master" >Create model class to handle case response type</History>

namespace FATWA_DOMAIN.Models.AdminModels.CaseManagment
{
    [Table("CMS_RESPONSE_TYPE")]
    public partial class CaseResponseType
    {
        [Key]
        public int ResponseTypeId { get; set; }
        public string ResponseType_Name { get; set; }


    }
}
