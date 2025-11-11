using System.DirectoryServices.ActiveDirectory;

namespace FATWA_DOMAIN.Common
{
    public class CommonUtils
    {
        public static string GetDomainFullName(string friendlyName)
        {
            try
            {
                DirectoryContext context = new DirectoryContext(DirectoryContextType.Domain, friendlyName);
                Domain domain = Domain.GetDomain(context);
                return domain.Name;
            }
            catch
            {
                return "";
            }
        }
    }
}
