using System.ComponentModel.DataAnnotations.Schema;


namespace FATWA_DOMAIN.Models.DigitalSignature
{
    public class DSPAuthenticationResponse
    {
        [NotMapped]
        public MIDAuthSignResponse MIDAuthSignResponse { get; set; }
    }

    public class MIDAuthSignResponse
    {
        [NotMapped]
        public RequestDetails RequestDetails { get; set; }
        [NotMapped]
        public ResultDetails ResultDetails { get; set; }
        [NotMapped]
        public PersonalData PersonalData { get; set; }
    }
    public class RequestDetails
    {
        public string RequestID { get; set; }
        public string RequestType { get; set; }
        public string ServiceProviderId { get; set; }
        public string CivilNo { get; set; }
    }
    public class ResultDetails
    {
        public string ResultCode { get; set; }
        public string UserAction { get; set; }
        public string UserCivilNo { get; set; }
        public string UserCertificate { get; set; }
        public string SigningData { get; set; }
        public string SigningDatatype { get; set; }
        public DateTime TransactionDate { get; set; }
    }
    public class PersonalData
    {
        public string CivilID { get; set; }
        public string FullNameAr { get; set; }
        public string FullNameEn { get; set; }
        public DateTime BirthDate { get; set; }
        public string BloodGroup { get; set; }
        public string Photo { get; set; }
        [NotMapped]
        public Address Address { get; set; }
    }


    public class Address
    {
        public string Governerate { get; set; }
        public string Area { get; set; }
        public string PaciBuildingNumber { get; set; }
        public string BuildingType { get; set; }
        public string FloorNumber { get; set; }
        public string BuildingNumber { get; set; }
        public string BlockNumber { get; set; }
        public string UnitNumber { get; set; }
        public string StreetName { get; set; }
        public string UnitType { get; set; }
    }
}
