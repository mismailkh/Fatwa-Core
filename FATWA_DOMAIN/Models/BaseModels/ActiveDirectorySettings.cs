namespace FATWA_DOMAIN.Models.BaseModels
{
    public class ActiveDirectorySettings
    {
        public string ServerIPAddress { get; set; }
        public string Container { get; set; }
        public string UserCreationContainer { get; set; }
        public string DomainName { get; set; }
        public string OrganizationalUnit { get; set; }
        public string MachineAccountName { get; set; }
        public string MachineAccountPassword { get; set; }
    }
}
