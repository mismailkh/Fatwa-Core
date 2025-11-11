using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace FATWA_DOMAIN.Models.CaseManagment
{
    //< History Author = 'Hassan Abbas' Date = '2022-09-29' Version = "1.0" Branch = "master" >
    //      -> Operating Sector Type Table for G2G Portal
    //      -> Values should also be synced with OperatingSectorTypeEnum
    //</History>
    [Table("CMS_OPERATING_SECTOR_TYPE_G2G_LKP")]
    public class OperatingSectorType : TransactionalBaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Code { get; set; }
        public string Name_En { get; set; }
        public string Name_Ar { get; set; }
        public bool IsActive { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        public int? ParentId { get; set; }
        public OperatingSectorType Parent { get; set; }
        public int? ModuleId { get; set; }
        public int BuildingId { get; set; }
        public int FloorId { get; set; }
        public bool IsOnlyViceHosApprovalRequired { get; set; }
        public bool IsViceHosResponsibleForAllLawyers { get; set; }
        public int? G2GBRSiteID { get; set; }
        [NotMapped]
        public IEnumerable<string> RoleIds { get; set; }
    }
}
