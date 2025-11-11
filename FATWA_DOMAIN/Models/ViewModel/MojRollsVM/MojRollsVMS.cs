
using System.ComponentModel.DataAnnotations.Schema;
namespace FATWA_DOMAIN.Models.ViewModel.MojRollsVM
{
  
   
    public partial class MOJRollsChamberVM
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public int? CourtId { get; set; }
    }

  
    public partial class MOJRollsChamberTypeCode
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public int ChamberId { get; set; }
    }

    public partial class MOJRollsCourts
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string Name_En { get; set; }
        public string? Name_Ar { get; set; }
        public string? District { get; set; }
        public string? Location { get; set; }
        public int? TypeId { get; set; }
        public bool? IsActive { get; set; }
        public string? CourtCode { get; set; }
        public string? Address { get; set; }
        public string? Description { get; set; }
    }

  
    public partial class MOJRollsRequestStatusHistory
    {
        public int Id { get; set; }
        public int? Request_Id { get; set; }
        public int? PreviousStatusId_from { get; set; }
        public int? NewStatusId_To { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? CreatedBy { get; set; }
        public string? Comments { get; set; }
        [NotMapped]
        public DateTime? SessionDate { get; set; }



    }
 
    public partial class MOJRollsRequest
    {
        public int Id { get; set; }
        public DateTime? SessionDate { get; set; }
        public int? RollId_LookUp { get; set; }
        public int? CourtType_LookUp { get; set; }
        public int? ChamberType_LookUp { get; set; }
        public int? ChamberTypeCode_LookUp { get; set; }
        public DateTime? RequestedDateTime { get; set; }
        public string? RequestedBy { get; set; }
        public string? FilePath { get; set; }
        public int? RequestStatus_LookUp { get; set; }
        public int? DocumentId { get; set; }
        public bool IsAssigned { get; set; }
        public string? LawyerId { get; set; }
        public bool HasOutCome { get; set; }
        public bool IsFatwaManual {  get; set; }

    }
   
    public partial class MOJRollsVM
    {
        public int Id { get; set; }
        public int? CourtTypeId_LookUp { get; set; }
        public string? Description { get; set; }
        public string? FunctionKey { get; set; }
    }
    //MOJRollsRequestDetailsList
    public class MOJRollsRequestDetailsList
    {
        public int? Id { get; set; }


        public DateTime? SessionDate { get; set; }
        public int? RollId_LookUp { get; set; }
        public string? RollId_LookUp_Value { get; set; }
        public int? CourtType_LookUp { get; set; }
        public string? CourtType_LookUp_Value { get; set; }
        public int? ChamberType_LookUp { get; set; }
        public string? ChamberType_LookUp_Value { get; set; }
        public int? ChamberTypeCode_LookUp { get; set; }
        public string? ChamberTypeCode_LookUp_Value { get; set; }
        public DateTime? RequestedDateTime { get; set; }
        public string? RequestedBy { get; set; }
        public string? FilePath { get; set; }
        public int? RequestStatus_LookUp { get; set; }
        public string? RequestStatus_LookUp_Value { get; set; }
        public string? ExceptionDetails { get; set; }
        public string? NoFound { get; set; }
        public int? DocumentId { get; set; }
       
    }
    public class MOJRollsChamberNumberVM
    {
        public int Id { get; set; }
        public string Number { get; set; }
    }

    public class AssignHearingRollToLawyerVM
    {
        public string LawyerId { get; set; } = string.Empty;
        public DateTime HearingDate { get; set;}
        public int ChamberNumberid { get; set;}
    }
}
