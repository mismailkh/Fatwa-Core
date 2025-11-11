namespace FATWA_DOMAIN.Models.AdminModels.UserManagement
{
    public  class DeactivateEmployeesModel
    {
        public List<EmployeeStatus> employeeStatuses { get; set; }
        public DateTime StatusDate { get; set; }
        public string StatusReason { get; set; }        
        public string DelegatedEmployeId { get; set; }        
    }
}
