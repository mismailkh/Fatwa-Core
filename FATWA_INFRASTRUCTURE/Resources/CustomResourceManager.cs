using System.Globalization;
using System.Reflection;
using System.Resources;
namespace FATWA_INFRASTRUCTURE.Resources
{
    public class CustomResourceManager : ResourceManager
    {
        public CustomResourceManager(Type resourceSource) : base(resourceSource)
        {
        }

        public CustomResourceManager(string baseName, Assembly assembly)
            : base(baseName, assembly)
        {
        }

        public CustomResourceManager(string baseName, Assembly assembly, Type usingResourceSet)
            : base(baseName, assembly, usingResourceSet)
        {
        }

        public override string GetString(string name)
        {
            // your business logic
            return this.GetString(name);
        }

        public string GetString(string name, string lang)
        {
            // your business logic
            CultureInfo culture = new CultureInfo(lang);
            return this.GetString(name,culture);
        }
    }
}
