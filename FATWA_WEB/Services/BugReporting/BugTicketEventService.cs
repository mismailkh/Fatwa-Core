namespace FATWA_WEB.Services.BugReporting
{
    public class BugTicketEventService
    {
        public event Action? OnBugTicketAdded;

        public void BugTicketAdded()
        {
            OnBugTicketAdded?.Invoke();
        }
    }
}
