namespace FATWA_DOMAIN.Models.ServiceRequestModels
{
    public class ServiceRequestType
    {
        public int Id { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public int? SectorTypeId { get; set; }
        public bool IsActive { get; set; }
    }
}
