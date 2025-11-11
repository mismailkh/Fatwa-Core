using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.MeetModels;
using FATWA_DOMAIN.Models.ViewModel.CommunicationVMs;
using FATWA_DOMAIN.Models.ViewModel.Meet;

namespace FATWA_DOMAIN.Interfaces.Meet
{
    public interface IMeeting
    {
        Task<List<MeetingVM>> GetMeetingsList(string userName, int PageSize, int PageNumber);
        Task<List<MeetingVM>> GetMeetingsForMobileApp(string userName, int channelId);
        Task<SaveMeetingVM> GetMeetingById(Guid meetingId);
        Task<MeetingDecisionVM> GetMeetingDecisionDetailById(Guid meetingId, int sectorId);
        Task<bool> SaveMom(SaveMomVM meetingMom);
        Task<bool> EditMom(SaveMomVM meetingMom);
        Task<MeetingCommunicationVM> AddMeeting(SaveMeetingVM meetingVM);
		//Task<bool> EditMeeting(SaveMeetingVM meetingVM); 
		Task<MeetingCommunicationVM> UpdateMeetingDecision(MeetingDecisionVM meetingVM);
        Task<bool> SaveLegislationAttandee(SaveMeetingVM meetingVM);
        Task<bool> UpdateMeetingStatus(MeetingDecisionVM meetingVM);
        Task<AttendeeDecisionVM> GetMeetingAttendeeDecisionById(Guid meetingId, string userId, bool isMomAttendeeDecision);
         Task<bool> UpdateMeetingAttendeeDecision(AttendeeDecisionVM decision, string userId, bool isMomAttendeeDecision);
        Task<MeetingCommunicationVM> EditsMeeting(SaveMeetingVM meetingVM);
        Task<MeetingMom> GetMeetingMOMByMeetingId(Guid meetingId);
        Task<List<MomAttendeeDecisionVM>> PopulateMOMAttendeesDecisionDetails(Guid meetingMomId, Guid meetingId);

        Task<SaveMomVM> GetMeetingDetailById(Guid meetingId);
        Task<MeetingStatusVM> EditMeetingStatus(MeetingStatusVM meeting);

        Task<List<MeetingAttendee>> GetAttendeeDetails(Guid meetingId );

        Task<bool> SubmitMom(SaveMomVM meetingMom);
        Task<List<GEDepartments>> GetDepartmentsByGeId(int GeId);
        Task<MeetingDecisionVM> GetMeetingDetailsById(Guid Meeting);
        Task<bool> CheckDraftExixt(Guid MeetingId);
         Task<MeetingCommunicationVM> TakeRequestForMeetingDecisionFromFatwa(SaveMeetingVM meetingVM);
        Task<bool> UpdateMeetingMOMAttendeeDecision(MomAttendeeDecision item);
        Task<User> GetMOMCreatedByIdByUsingMOMId(Guid meetingMomId);
        Task<bool> GetAttendeeStatusesByMeetingId(Guid MeetingId);
        Task<AttendeeDecisionVM> GetNotificationParameters(AttendeeDecisionVM decision);
        Task<bool> CheckViceHosApproval(int sectorTypeId);
    }
}
    