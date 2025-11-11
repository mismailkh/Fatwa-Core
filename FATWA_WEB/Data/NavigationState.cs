namespace FATWA_WEB.Data
{
    //<History Author = 'Hassan Abbas' Date='2022-12-05' Version="1.0" Branch="master"> Navigation State to keep Navigation Urls and data</History>
    public class NavigationState
    {
        public List<string> RedirectionUrls { get; set; } = new List<string>();
        public string ReturnUrl { get; set; }
        public dynamic Id { get; set; }
    }
}
