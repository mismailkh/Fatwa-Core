namespace FATWA_DOMAIN.Enums.ServiceRequests
{
    public class ServiceRequestEnums
    {
        public enum ServiceRequestStatusEnum
        {
            Submitted = 1,
            ApprovedbyHOS = 2,
            RejectedbyCustodian = 4,
            InProgress = 8,
            Closed = 16,
            Draft = 32,
            Rejected = 64,
            ReOpened = 128,
            Resolved = 256,
            RejectedByHOS = 512,
            NeedModification = 1024,
            ApprovedByLeaveAndDutyDepartment = 2048,
            RejectedByLeaveAndDutyDepartment = 4096,
            ApprovedByAdministrativeAffairsDepartment = 8192,
            RejectedByAdministrativeAffairsDepartment = 16384,
            Approved = 32768,
            ReSubmitted = 65536,
        }

        public enum ServiceRequestTypeEnum
        {
            RequestToChangeResidence = 1,
            RequestForMaintenanceAndRepairOfResidence = 2,
            IssueResidenceClearance = 4,
            RegistrationOfApartmentsComplaints = 8,
            InsertConsultantResidentialDetails = 16,
            AssignEmployeesToFollowUpServerStatus = 32,
            RequestToIssueAnyGSItem = 64,
            RequestToIssueAnyITItem = 128,
            RequestToReturnAnyGSItem = 256,
            RequestToReturnAnyITItem = 512,
            RequestToDetachTheAssetFromEmployee = 1024,
            SubmitComplaintRequest = 2048,
            SubmitaLeaveRequest = 4096,
            RequestToReduceWorkingHours = 8192,
            RequestForFingerprintExemption = 16384,
            SubmitaRequestforPermission = 32768,
            RequestforAppointmentwithMedicalCouncil = 65536,
        }

        public enum RemarkTypeEnum
        {
            Rejected = 1,
            AddResolution,
            ReOpen,
            NeedModification
        }

        public enum ComplaintTypeEnum
        {
            Other = 5
        }
        public enum ReasonReduceWorkingHoursEnum
        {
            Other = 3
        }

        /// <summary>
        /// there are more in db but need this only if required you can add more
        /// </summary>
        public enum LeaveTypeEnum
        {
            MaternityLeave = 4
        }
    }
}
