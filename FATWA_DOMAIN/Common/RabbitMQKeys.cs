using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Common
{
    public static class RabbitMQKeys
    {
        public const string MojCaseDataSyncKey = "routingkeys.MojCaseDataSyncKey";
        public const string MojAssignCaseFileToSectorKey = "routingkeys.MojAssignCaseFileToSectorKey";
        public const string GERepresentativeFATWAKey = "routingkeys.GERepresentativeFATWAKey";
        public const string CreateCaseFileKey = "routingkeys.createCaseFile";
        public const string UpdateRequestHistory = "routingkeys.UpdateRequestHistory";
        public const string RequestStatusKey = "routingkeys.FatwaRequestStatusKey";
        public const string CopyRequestKey = "routingkeys.CopyRequestKey";
        public const string CopyFileKey = "routingkeys.CopyFileKey";
        public const string HistoryKey = "routingkeys.FatwaHistoryKey";
        public const string RegisteredCaseKey = "routingkeys.FatwaRegisteredCaseKey";
        public const string HearingKey = "routingkeys.FatwaHearingKey";
        public const string OutcomeHearingKey = "routingkeys.FatwaOutcomeHearingKey";
        public const string JudgementKey = "routingkeys.FatwaJudgementKey";
        public const string JudgementExecutionKey = "routingkeys.FatwaJudgementExecutionKey";
        public const string SubCaseRegister = "routingkeys.FatwaSubCaseRegister";
        public const string CommunicationKey = "routingkeys.FatwaCommunicationKey";
        public const string ComsWithdrawRequestStatusKey = "routingkeys.FatwaComsWithdrawRequestStatusKey";
        public const string CreateConsultationFileKey = "routingkeys.createConsultationFile";
        public const string UpdateComsRequestHistory = "routingkeys.UpdateComsRequestHistory";
        public const string MeetingScheduleKey = "routingkeys.FatwaMeetingScheduletKey";
        public const string MeetingStatusUpdateKey = "routingkeys.FatwaMeetingStatusUpdateKey";
        public const string SubmitMOMMeeting = "routingkeys.SubmitMOMMeeting";
        public const string RequestForMeetingFromG2GKey = "routingkeys.RequestForMeetingFromG2GKey";
        public const string CreateCaseRequestFromFatwa = "routingkeys.CreateCaseRequestFromFatwa";

        // For Lookups

        public const string GovEntityPatternKey = "routingkeys.GovEntityPatternKey";
        public const string GovEntityPatternDeleteKey = "routingkeys.GovEntityPatternDeleteKey";
        public const string GovEntityPatternEditKey = "routingkeys.GovEntityPatternEditKey";
        public const string GovEntityKey = "routingkeys.GovEntityKey";
        public const string ChamberKey = "routingkeys.ChamberKey";
        public const string CourtNameKey = "routingkeys.CourtNameKey";
        public const string UpdateCourtNameKey = "routingkeys.UpdateCourtNameKey";
        public const string DeleteCourtNameKey = "routingkeys.DeleteCourtNameKey";
        public const string UpdateChamberKey = "routingkeys.UpdateChamberKey";
        public const string DeleteChamberKey = "routingkeys.DeleteChamberKey";
        public const string ActiveGovEntityKey = "routingkeys.ActiveGovEntityKey";
        public const string ActiveChamberKey = "routingkeys.ActiveChamberKey";
        public const string ActiveCourtsKey = "routingkeys.ActiveCourtsKey";
        public const string ActiveDepartmentsKey = "routingkeys.ActiveDepartmentsKey";
        public const string UpdateGovEntityKey = "routingkeys.UpdateGovEntityKey";
        public const string DeleteGovEntityKey = "routingkeys.DeleteGovEntityKey";

        public const string UpdateDepartmentKey = "routingkeys.UpdateDepartmentKey";
        public const string SaveDepartmentKey = "routingkeys.SaveDepartmentKey";
        public const string DeleteGovEntityDepartmentKey = "routingkeys.DeleteGovEntityDepartmentKey";

        public const string UpdateRequesttypeKey = "routingkeys.UpdateRequesttypeKey";
        public const string SaveSubTypeKey = "routingkeys.UpdateRequesttypeKey";
        public const string UpdateSubtypeKey = "routingkeys.UpdateSubtypeKey";

        public const string DeleteSubTypeKey = "routingkeys.DeleteSubTypeKey";
        public const string ActiveSubTypeKey = "routingkeys.ActiveSubTypeKey";
        public const string SaveSubTypesKey = "routingkeys.SaveSubTypesKey";
        public const string SaveConsultationLegislationFileTypeKey = "routingkeys.SaveConsultationLegislationFileTypeKey";

        public const string UpdateConsultationLegislationFileTypeKey = "routingkeys.UpdateConsultationLegislationFileTypeKey";
        public const string DeleteConsultationLegislationFileTypeKey = "routingkeys.DeleteConsultationLegislationFileTypeKey";
        public const string ActiveConsultationLegislationFileTypeKey = "routingkeys.ActiveConsultationLegislationFileTypeKey";

        public const string SaveComsInternationalArbitrationTypeKey = "routingkeys.SaveComsInternationalArbitrationTypeKey";
        public const string UpdateComsInternationalArbitrationTypeKey = "routingkeys.UpdateComsInternationalArbitrationTypeKey";
        public const string DeleteComsInternationalArbitrationTypeKey = "routingkeys.DeleteComsInternationalArbitrationTypeKey";

        public const string ActiveComsInternationalArbitrationTypeKey = "routingkeys.ActiveComsInternationalArbitrationTypeKey";
        public const string SaveCommunicationTypeKey = "routingkeys.SaveCommunicationTypeKey";
        public const string UpdateCommunicationTypeKey = "routingkeys.UpdateCommunicationTypeKey";

        public const string DeleteCommunicationTypeKey = "routingkeys.DeleteCommunicationTypeKey";
        public const string ActiveCommunicationTypeKey = "routingkeys.ActiveCommunicationTypeKey";
        public const string UpdateCaseFileStatusKey = "routingkeys.UpdateCaseFileStatusKey";

        public const string GetGEAttendee = "routingkeys.GetGEAttendee";
        public const string SaveGEDepartmentKey = "routingkeys.SaveGEDepartmentKey";
        public const string UpdateGEDepartmentKey = "routingkeys.UpdateGEDepartmentKey";
        public const string ActiveGEDepartmentsKey = "routingkeys.ActiveGEDepartmentsKey";
        public const string UpdateCaseFileStatusEnumKey = "routingkeys.UpdateCaseFileStatusEnumKey";
        public const string SaveChamberNumberKey = "routingkeys.SaveChamberNumberKey";
        public const string UpdateChamberNumberKey = "routingkeys.UpdateChamberNumberKey";
        public const string DeleteChamberNumberKey = "routingkeys.DeleteChamberNumberKey";
        public const string ActiveChamberNumberKey = "routingkeys.ActiveChamberNumberKey";
        // Notification Template handling > G2G  
        public const string CreateNotificationTemplate = "routingkeys.CreateNotificationTemplate";
        public const string UpdateNotificationEventTemplate = "routingkeys.UpdateNotificationEventTemplate";

        // RPA Tarasol
        public const string TarasolPayloadKey = "TarasolPayloadKey";
        public const string TarasolDocKey = "TarasolDocKey";

        //Government Entity Representative Kyes
        public const string SaveGovernmentEntityRepresentativeKey = "routingkeys.SaveGovernmentEntityRepresentativeKey";
        public const string UpdateGovernmentEntityRepresentativeKey = "routingkeys.UpdateGovernmentEntityRepresentativeKey";
        public const string DeleteGovernmentEntityRepresentativeKey = "routingkeys.DeleteGovernmentEntityRepresentativeKey";
        public const string ActiveGovernmentEntityRepresentativeKey = "routingkeys.ActiveGovernmentEntityRepresentativeKey";
        

        //Bug Reporting
        public const string AssignBugTypeToModuleKey = "routingkeys.assignBugTypeToModuleKey";
        public const string AddG2GFeedbackCommentKey = "routingkeys.addG2GFeedbackCommentKey";
        public const string UpdateG2GFeedbackCommentKey = "routingkeys.updateG2GFeedbackCommentKey";
        public const string DeleteG2GFeedbackCommentKey = "routingkeys.deleteG2GFeedbackCommentKey";
        public const string UpdateG2GTicketStatusKey = "routingkeys.updateG2GTicketStatusKey";
        public const string TicketTaggingAndAssignmentKey = "routingkeys.ticketTaggingAndAssignmentKey";
    }
}
