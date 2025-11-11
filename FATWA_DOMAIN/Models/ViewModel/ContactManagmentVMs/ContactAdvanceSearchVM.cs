using FATWA_DOMAIN.Models.BaseModels;

namespace FATWA_DOMAIN.Models.ViewModel.ContactManagmentVMs
{
    public partial class ContactAdvanceSearchVM : GridPagination
    {
        public DateTime? CreatedTo { get; set; }
        public DateTime? CreatedFrom { get; set; }
        public int? ContactTypeId { get; set; }
        public string? Name { get; set; }

    }
}
