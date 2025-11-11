//<History Author = 'Hassan Abbas' Date='2022-03-14' Version="1.0" Branch="master"> Service created for sidemenu upto multiple levels with Permissions/Claims</History>
using static FATWA_DOMAIN.Enums.CaseManagementEnums;

namespace DMS_WEB.Services
{
    //<History Author = 'Hassan Abbas' Date='2022-03-14' Version="1.0" Branch="master"> Sidemenu class/Claims</History>
    public class SideMenu
    {
        public bool New { get; set; }
        public bool Updated { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Path { get; set; }
        public string Title { get; set; }
        public string Claim { get; set; }
        public string Description { get; set; }
        public string Class { get; set; }
        public bool Expanded { get; set; }
        public IEnumerable<SideMenu> Children { get; set; }
        public IEnumerable<string> Tags { get; set; }
    }

    //<History Author = 'Hassan Abbas' Date='2022-03-14' Version="1.0" Branch="master"> Sidemenu service returning menu array with claims, paths and attributes</History>
    public class SideMenuService
    {
        private readonly SideMenu[] allSideMenus = new[]
        {
            new SideMenu()
            {
                Name = "Document_Management",
                Icon = "storage",
                Claim = "Permissions.DMS.Menu.Document",
                Children = new []
                {
                    new SideMenu()
                    {
                        Name = "Configure_File_Attributes",
                        Path = "/configure-fileattributes",
                        Icon = "panorama_fish_eye",
                        Claim = "Permissions.DMS.ConfigureFileAttributes"
                    },
                    new SideMenu()
                    {
                        Name = "Template_List",
                        Path = "/template-list",
                        Icon = "panorama_fish_eye",
                        Claim = "Permissions.DMS.Template.List"
                    }
                }
            }
        };

        public IEnumerable<SideMenu> SideMenus
        {
            get
            {
                return allSideMenus;
            }
        }

        public IEnumerable<SideMenu> Filter(string term)
        {
            if (string.IsNullOrEmpty(term))
                return allSideMenus;

            bool contains(string value) => value != null && value.Contains(term, StringComparison.OrdinalIgnoreCase);

            bool filter(SideMenu SideMenu) => contains(SideMenu.Name) || (SideMenu.Tags != null && SideMenu.Tags.Any(contains));

            bool deepFilter(SideMenu SideMenu) => filter(SideMenu) || SideMenu.Children?.Any(filter) == true;

            return SideMenus.Where(category => category.Children?.Any(deepFilter) == true || filter(category))
                           .Select(category => new SideMenu
                           {
                               Name = category.Name,
                               Expanded = true,
                               Children = category.Children?.Where(deepFilter).Select(SideMenu => new SideMenu
                               {
                                   Name = SideMenu.Name,
                                   Path = SideMenu.Path,
                                   Icon = SideMenu.Icon,
                                   Expanded = true,
                                   Children = SideMenu.Children
                               }
                               ).ToArray()
                           }).ToList();
        }

        public SideMenu FindCurrent(Uri uri)
        {
            IEnumerable<SideMenu> Flatten(IEnumerable<SideMenu> e)
            {
                return e.SelectMany(c => c.Children != null ? Flatten(c.Children) : new[] { c });
            }

            return Flatten(SideMenus)
                        .FirstOrDefault(SideMenu => SideMenu.Path == uri.AbsolutePath || $"/{SideMenu.Path}" == uri.AbsolutePath);
        }

        public string TitleFor(SideMenu SideMenu)
        {
            if (SideMenu != null && SideMenu.Name != "Overview")
            {
                return SideMenu.Title ?? $"Blazor {SideMenu.Name} | a free UI component by Radzen";
            }

            return "Free Blazor Components | 60+ controls by Radzen";
        }

        public string DescriptionFor(SideMenu SideMenu)
        {
            return SideMenu?.Description ?? "The Radzen Blazor component library provides more than 50 UI controls for building rich ASP.NET Core web applications.";
        }
    }
}