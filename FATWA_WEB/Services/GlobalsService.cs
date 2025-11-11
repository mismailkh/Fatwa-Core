namespace FATWA_WEB.Services
{
    //<History Author = 'Aqeel Altaf' Date='2022-03-14' Version="1.0" Branch="master">Global service that will use throught out in the application</History>
    public partial class GlobalsService
    {

    }

    public class PropertyChangedEventArgs
    {
        public string Name { get; set; }
        public object NewValue { get; set; }
        public object OldValue { get; set; }
        public bool IsGlobal { get; set; }
    }
}
