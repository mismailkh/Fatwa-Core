namespace FATWA_WEB.Services
{
    public class DraftHandlerService
    {
        public Func<bool, Task<bool>>? SaveDraftForCase { get; set; }
        public Func<bool, Task<bool>>? SaveDraftForConsultation { get; set; }
    }
}
