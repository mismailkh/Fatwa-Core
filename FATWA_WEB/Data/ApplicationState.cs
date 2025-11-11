namespace FATWA_WEB.Data
{
    //<History Author = 'Hassan Abbas' Date='2024-31-03' Version="1.0" Branch="master"> Added State class to maintain data across the application for all the connections</History>
    public class ApplicationState
    {
        public List<Tuple<Guid, string, string, string>> LockedVersions { get; set; } = new List<Tuple<Guid, string, string, string>>();
    }
}
