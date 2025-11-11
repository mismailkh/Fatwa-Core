using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.AdminModels.UserManagement
{
    //< History Author = 'Ammaar Naveed' Date = '2024-04-24' Version = "1.0" Branch = "master" >Created model for Employee Leave Delegation Information.</History>
    [Table("EP_EMPLOYEE_LEAVE_DELEGATION_INFORMATION")]
    public class EmployeeLeaveDelegationInformation : TransactionalBaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? DelegatedUserId { get; set; }
    }
}
