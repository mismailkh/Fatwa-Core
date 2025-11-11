using FATWA_DOMAIN.Models.BaseModels;

namespace FATWA_DOMAIN.Models.ViewModel.ContactManagmentVMs
{

    public partial class ContactListVM : GridMetadata
    {
        public Guid? ContactId { get; set; }
        public string? Name { get; set; }
		public string? JobRoleId { get; set; }
		public string? JobRoleEn { get; set; } 
        public string? JobRoleAr { get; set; } 
        //public string? DepartmentEn { get; set; } 
        //public string? DepartmentAr { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? DeletedBy { get; set; }
        public int? ContactTypeId { get; set; }
		public string? ContactTypeNameEn { get; set; }
		public string? ContactTypeNameAr { get; set; }
		public int? Designation { get; set; }
		public string? DesignationNameEn { get; set; }
		public string? DesignationNameAr { get; set; }
		public int? WorkPlace { get; set; }
		public string? WorkplaceNameEn { get; set; }
		public string? WorkplaceNameAr { get; set; }

	}

    public partial class ContactFileLinkVM
    {
        public Guid? ContactId { get; set; }
        public string? Name { get; set; }
        public string? JobRoleId { get; set; }
        public string? JobRoleEn { get; set; }
        public string? JobRoleAr { get; set; }
        //public string? DepartmentEn { get; set; } 
        //public string? DepartmentAr { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? DeletedBy { get; set; }
        public int? ContactTypeId { get; set; }
        public string? ContactTypeNameEn { get; set; }
        public string? ContactTypeNameAr { get; set; }
        public int? Designation { get; set; }
        public string? DesignationNameEn { get; set; }
        public string? DesignationNameAr { get; set; }
        public int? WorkPlace { get; set; }
        public string? WorkplaceNameEn { get; set; }
        public string? WorkplaceNameAr { get; set; }
        public Guid? ReferenceId { get; set; }
        public int? Id { get; set; }
        public int? ContactLinkId { get; set; }
        public int? ModuleId { get; set; }

    }
}
