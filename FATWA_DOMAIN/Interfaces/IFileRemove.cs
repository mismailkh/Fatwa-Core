namespace FATWA_DOMAIN.Interfaces
{
    public interface IFileRemove<T> where T : class
    {
        Task<List<T>> GetAttachements(object id);
        Task DeleteAttachement(object id);
    }
}
