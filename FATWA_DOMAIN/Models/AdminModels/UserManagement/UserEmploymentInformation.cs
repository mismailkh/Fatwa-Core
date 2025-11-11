using FATWA_DOMAIN.Models.CaseManagment;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.AdminModels.UserManagement
{
    [Table("EP_EMPLOYMENT_INFORMATION")]
    public class UserEmploymentInformation
    {
        [Key]
        public string UserId { get; set; }
        public string EmployeeId { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public int EmployeeTypeId { get; set; }
        public int? GradeId { get; set; }
        public int DesignationId { get; set; }
        [NotMapped]
        public int DepartmentId { get; set; }
        public int? SectorTypeId { get; set; }
        public string? SupervisorId { get; set; }
        [NotMapped]
        public UserPersonalInformation? Supervisor { get; set; }
        public string? ManagerId { get; set; }
        [NotMapped]
        public UserPersonalInformation? Manager { get; set; }
        public int? CompanyId { get; set; }
        public int WorkingTimeId { get; set; } // (Full Time, Part Time)
        public int? EmployeeStatusId { get; set; } // For Internal(Active, Resigned, Terminated) For External(Active, InActive)
        [DisplayName("Resigned/Termination/InActiveDate")]
        public DateTime? ResignedTerminationdDate { get; set; }
        public string? ResignationTerminationReason { get; set; }
        public string? FingerPrintId { get; set; }
        public int? ContractTypeId { get; set; }
        public bool IsDefaultCorrespondenceReceiver { get; set; }
        public string? DelegatedUserId { get; set; }
        [NotMapped]
        public string? DelegatedBy { get; set; }
        public ContractType ContractType { get; set; }
        public EmployeeType EmployeeType { get; set; }
        public Grade Grade { get; set; }
        public Designation Designation { get; set; }
        public Company Company { get; set; }
        public User User { get; set; }
        public OperatingSectorType SectorType { get; set; }
        public EmployeeStatus EmployeeStatus { get; set; }
        public EmployeeWorkingTime WorkingTime { get; set; }         
    }
    public class UserEmploymentInformationVM
    {
        public string UserId { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public int EmployeeTypeId { get; set; }
        public int? GradeId { get; set; }
        public int DesignationId { get; set; }
        [NotMapped]
        public int DepartmentId { get; set; }
        public int? SectorTypeId { get; set; }
        public string? SupervisorId { get; set; }
        [NotMapped]
        public UserPersonalInformation? Supervisor { get; set; }
        public string? ManagerId { get; set; }
        [NotMapped]
        public UserPersonalInformation? Manager { get; set; }
        public int? CompanyId { get; set; }
        public int WorkingTimeId { get; set; } // (Full Time, Part Time)
        public int EmployeeStatusId { get; set; } // For Internal(Active, Resigned, Terminated) For External(Active, InActive)
        [DisplayName("Resigned/Termination/InActiveDate")]
        public DateTime? ResignedTerminationdDate { get; set; }
        public string? ResignationTerminationReason { get; set; }
    }
}
