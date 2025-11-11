using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.ViewModel.PACIVM
{

    public class PACIRequestDataVM
    {
        public Guid id { get; set; }
        public Guid RequestId { get; set; }
        public string? EntryId { get; set; }
        public string? CivilId { get; set; }
        public string Name { get; set; }
        public string? Gender { get; set; }
        public string? Nationality { get; set; }
        public string? BirthDate { get; set; }
        public string? NationalityResidencyNumber { get; set; }
        public string? Area { get; set; }
        public string? Block { get; set; }
        public string? Avenue { get; set; }
        public string? PhoneNumber { get; set; }
        public string? StreetName { get; set; }
        public string? BuildingNumber { get; set; }
        public string? HousingType { get; set; }
        public string? Floor { get; set; }
        public string? HousingNumber { get; set; }
        public string? AddressAutomatedNumber { get; set; }
        public string? Branchedfrom { get; set; }
        public string? BuildingName { get; set; }
        public string? OtherData { get; set; }
        public string? PassportNumber { get; set; }
        public string? Country { get; set; }
        public string? IndividualType { get; set; }
    }
}
