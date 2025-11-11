//<History Author = 'Hassan Abbas' Date='2022-07-21' Version="1.0" Branch="master"> Service created for sidemenu upto multiple levels with Permissions/Claims</History>

namespace FATWA_ADMIN.Services.General
{
    //<History Author = 'Hassan Abbas' Date='2022-07-21' Version="1.0" Branch="master"> Sidemenu class/Claims</History>
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
        public bool Expanded { get; set; }
        public IEnumerable<SideMenu> Children { get; set; }
        public IEnumerable<string> Tags { get; set; }
    }

    //<History Author = 'Hassan Abbas' Date='2022-07-21' Version="1.0" Branch="master"> Sidemenu service returning menu array with claims, paths and attributes</History>
    public class SideMenuService
    {
        private readonly SideMenu[] allSideMenus = new[] {
        new SideMenu()
        {
            Name = "Home",
            Path = "/index",
            Icon = "&#xe871",
            Claim = "Admin.Permissions.Menu.Dashboard"
        },
        new SideMenu
        {
            Name = "Users_Management",
            Icon = "people_alt",
            Claim = "Admin.Permissions.Menu.UMS",
            Children = new [] {
                new SideMenu()
                {
                    Name = "Web_Systems",
                    Updated = false,
                    New = false,
                    Path = "/websystem-list",
                    Icon = "panorama_fish_eye",
                    Claim = "Admin.Permissions.Menu.UMS",
                    Tags = new [] { "pager", "paging" }
                },
                new SideMenu()
                {
                    Name = "Group_Access_Types",
                    Updated = false,
                    New = false,
                    Path = "/grouptype-list",
                    Icon = "panorama_fish_eye",
                    Claim = "Admin.Permissions.Menu.UMS",
                    Tags = new [] { "pager", "paging" }

                },
                new SideMenu()
                {
                    Name = "Groups",
                    Updated = false,
                    New = false,
                    Path = "/groups",
                    Icon = "panorama_fish_eye",
                    Claim = "Admin.Permissions.Menu.UMS",
                    Tags = new [] { "dataview", "supervisorsAndManagersGrid", "table" }
                },
                new SideMenu()
                {
                    Name = "Roles",
                    Updated = false,
                    New = false,
                    Path = "/roles",
                    Icon = "panorama_fish_eye",
                    Claim = "Admin.Permissions.Menu.UMS",
                    Tags = new [] { "dataview", "supervisorsAndManagersGrid", "table" }
                },
                new SideMenu()
                {
                    Name = "Claims",
                    Path = "/claims",
                    Icon = "panorama_fish_eye",
                    Claim = "Admin.Permissions.Menu.UMS"
                },
                new SideMenu()
                {
                    Name = "User_Role_And_Digital_Signature",
                    Updated = false,
                    New = false,
                    Path = "/employees-list",
                    Icon = "panorama_fish_eye",
                    Claim = "Admin.Permissions.Menu.UMS",
                    Tags = new [] { "pager", "paging" }
                },
            }
        },
        new SideMenu()
        {
            Name = "Notifications_Event",
            Icon = "notifications",
            Claim = "Admin.Permissions.Menu.Notfication",
            Path = "/notification-eventlist",
        },
        new SideMenu()
        {
            Name = "UMS_Configuration",
            Icon = "manage_accounts",
            Claim = "Admin.Permissions.Menu.UMS.Configuration",
            Children = new []
            {
                new SideMenu()
                {
                    Name = "Dynamic_Lookups",
                    Path = "/UMS-lookups",
                    Icon = "panorama_fish_eye",
                    Claim = "Admin.Permissions.Menu.UMS.Configuration"
                },
                new SideMenu()
                {
                    Name = "Fixed_Lookups",
                        Path = "/UMS-Enums",
                        Icon="panorama_fish_eye",
                     Claim = "Admin.Permissions.Menu.UMS.Configuration",
                        Tags = new [] { "dataview", "supervisorsAndManagersGrid", "table" }
                },
            }
        },
        new SideMenu()
        {
            Name = "System_Settings",
            Path = "/system-setting",
            Icon = "settings",
            Claim = "Admin.Permissions.Users.SystemConfiguration"
        },
        new SideMenu()
        {
            Name = "LLS_Configuration",
            Icon = "book",
            Claim = "Admin.Permissions.Menu.LLS.Configuration",
            Children = new [] {
                new SideMenu()
                {
                    Name = "Library_Configuration",
                    Path = "/llc-lookups",
                    Claim = "Admin.Permissions.Menu.LLS.Configuration",
                    Icon ="panorama_fish_eye",
                },
                new SideMenu()
                {
                    Name = "Legislation_Configuration",
                    Path = "/lls-lookups",
                    Claim = "Admin.Permissions.Menu.LLS.Configuration",
                    Icon ="panorama_fish_eye",

                    Tags = new [] { "dataview", "supervisorsAndManagersGrid", "table" }
                },
                new SideMenu()
                {
                    Name = "Legal_Principle_Configuration",
                    Path = "/lps-lookups",
                    Icon="panorama_fish_eye",
                    Claim = "Admin.Permissions.Menu.LLS.Configuration",
                    Tags = new [] { "dataview", "supervisorsAndManagersGrid", "table" }
                },
            }

        },
        new SideMenu()
        {
            Name = "Case_Configuration",
            Icon = "gavel",
            Claim = "Admin.Permissions.Menu.CMS.Configuration",
            Children = new [] {
                new SideMenu()
                {
                    Name = "Dynamic_Lookups",
                    Path = "/ge-lookups",
                    Icon="panorama_fish_eye",
                    Claim = "Admin.Permissions.Menu.CMS.Configuration",
                    Tags = new [] { "pager", "paging" }

                },
                new SideMenu()
                {
                    Name = "Fixed_Lookups",
                    Path = "/enums-lists",
                    Icon="panorama_fish_eye",
                    Claim = "Admin.Permissions.Menu.CMS.Configuration",
                    Tags = new [] { "dataview", "supervisorsAndManagersGrid", "table" }
                },
                    new SideMenu()
                {
                        Name = "Case_Request_Number_Pattern",
                        Path = "/CaseRequesNumberPattern",
                        Claim = "Admin.Permissions.Menu.CMS.Configuration",
                        Icon="panorama_fish_eye",
                        Tags = new [] { "dataview", "supervisorsAndManagersGrid", "table" }
                    },
                new SideMenu()
                    {
                        Name = "Case_File_Number_Pattern",
                        Path = "/CaseFileNumberPattern",
                        Icon="panorama_fish_eye",
                        Claim = "Admin.Permissions.Menu.CMS.Configuration",
                        Tags = new [] { "dataview", "supervisorsAndManagersGrid", "table" }
                    },
                new SideMenu()
                    {
                    Name = "Assign_Supervisor_And_Manager",
                    Path = "/assign-supervisor-manager",
                    Icon = "panorama_fish_eye",
                    Claim = "Admin.Permissions.Menu.CMS.Configuration"
                    },
            }
        },
        new SideMenu()
        {
            Name = "Consultation_Configuration",
            Icon = "gavel",
            Claim = "Admin.Permissions.Menu.COMS.Configuration",
            Children = new [] {
                new SideMenu()
                {
                    Name = "Dynamic_Lookups",
                     Path = "/coms-lookups",
                    Claim = "Admin.Permissions.Menu.COMS.Configuration",
                    Icon="panorama_fish_eye",
                    Tags = new [] { "pager", "paging" }

                },
                new SideMenu()
                {
                    Name = "Consultation_Request_Number_Pattern",
                    Path = "/ConsultationRequestNumberPattern",
                    Claim = "Admin.Permissions.Menu.COMS.Configuration",
                    Icon="panorama_fish_eye",
                    Tags = new [] { "dataview", "supervisorsAndManagersGrid", "table" }

                },
                new SideMenu()
                {
                    Name = "Consultation_File_Number_Pattern",
                    Path = "/ConsultationFileNumberPattern",
                    Claim = "Admin.Permissions.Menu.COMS.Configuration",
                    Icon="panorama_fish_eye",
                    Tags = new [] { "dataview", "supervisorsAndManagersGrid", "table" }

                },

            }
        },
        new SideMenu()
        {
            Name = "DMS_Configuration",
            Path = "DMS-Enums",
            Icon = "assignment",
            Claim = "Admin.Permissions.Menu.DMS.Configuration",
            Tags = new [] { "dataview", "supervisorsAndManagersGrid", "table" }
        },
        new SideMenu()
        {
            Name = "Common_Configuration",
            Icon = "settings_suggest",
            Claim = "Admin.Permissions.Menu.Common.Configuration",
            Children = new [] {
            new SideMenu()
            {
                Name = "Inbox_Number_Pattern",
                Path = "/InboxNumberPattern",
                Icon="panorama_fish_eye",
                Claim = "Admin.Permissions.Menu.Common.Configuration"
            },
            new SideMenu()
            {
                Name = "Outbox_Number_Pattern",
                Path = "/OutboxNumberPattern",
                Icon="panorama_fish_eye",
                Claim = "Admin.Permissions.Menu.Common.Configuration"
            },
            new SideMenu()
            {
                Name = "Common_Lookups",
                Path = "/CommonEnums-lists",
                Icon="panorama_fish_eye",
                Claim = "Admin.Permissions.Menu.Common.Configuration"
            },
            }
        },
        new SideMenu()
        {
            Name = "Translations",
            Path = "/translations",
            Icon = "translate",
            Claim = "Admin.Permissions.Users.Translations"
        },
        new SideMenu
        {
            Name = "Background_Services",
            Icon = "settings",
            Claim = "Admin.Permissions.Menu.BackgroundServices",
            Children = new [] {
                    new SideMenu()
                    {
                        Name = "SLA_Intervals",
                        Path = "/time-interval",
                        Claim = "Admin.Permissions.Menu.BackgroundServices",
                        Icon="panorama_fish_eye"
                    },

                    new SideMenu()
                    {
                        Name = "Execution_Logs",
                        Path = "/workerservice-executiondetail-view",
                        Claim = "Admin.Permissions.Menu.UMS",
                        Icon="panorama_fish_eye"
                    },

                    new SideMenu()
                    {
                        Name = "Public_Holidays",
                        Path = "/public-holidays-view",
                        Claim = "Admin.Permissions.Menu.BackgroundServices",
                        Icon="panorama_fish_eye"
                    },
            }
        },
        new SideMenu
        {
            Name = "Automation_Monitoring_Interface",
            Icon = "dvr",
            Claim = "Admin.Permissions.Menu.AutoMonInterface",
            Children = new [] {
                    new SideMenu()
                    {
                        Name = "Process_List",
                        Path = "/process-list",
                        Claim = "Admin.Permissions.Menu.AutoMonInterface",
                        Icon="panorama_fish_eye"
                    },
                    new SideMenu()
                    {
                        Name = "Case_Data_Extraction",
                        Path = "/casedata_extraction",
                        Claim = "Admin.Permissions.Menu.AutoMonInterface",
                        Icon="panorama_fish_eye"
                    }
            }
        },
        new SideMenu()
            {
                Name = "Logs",
                Icon = "sync_alt",
                Claim = "Admin.Permissions.Menu.Logs",
                Children = new []
                {
                    new SideMenu()
                    {
                        Name = "ErrorLogs",
                        Path = "/errorlogs",
                        Icon = "panorama_fish_eye",
                        Claim = "Admin.Permissions.Menu.Logs",
                    },
                    new SideMenu()
                    {
                        Name = "Operation_logs",
                        Path = "/processlogs",
                        Icon = "panorama_fish_eye",
                        Claim = "Admin.Permissions.Menu.Logs",
                    },
                }
            },
        new SideMenu
      {
          Name ="Service_Request_Flow_Setup",
          Icon ="view_timeline",
          Claim = "Admin.Permissions.Menu.CaseData.Extraction",
          Path = "/service-request-flow-list"
      },
        new SideMenu
        {
            Name = "Bug_Reporting",
            Icon = "bug_report",
            Claim = "Admin.Permissions.Menu.BugReporting",
            Children = new [] {
                    new SideMenu()
                    {
                        Name = "List_Bug_Ticket",
                        Path = "/list-bugticket",
                        Claim = "Admin.Permissions.Menu.BugReporting",
                        Icon="panorama_fish_eye"
                    },
                    new SideMenu()
                    {
                        Name = "Reported_Bugs",
                        Path = "/list-reportedbug",
                        Claim = "Admin.Permissions.Menu.BugReporting",
                        Icon="panorama_fish_eye"
                    },

                    new SideMenu()
                    {
                        Name = "Crash_Report",
                        Path = "/list-crashreport",
                        Claim = "Admin.Permissions.Menu.BugReporting",
                        Icon="panorama_fish_eye"
                    },
                new SideMenu()
                    {
                        Name = "Bug_Type_Lookups",
                        Path = "/list-bugtype",
                        Claim = "Admin.Permissions.Menu.BugReporting",
                        Icon="panorama_fish_eye"
                    },
            }
        },
        new SideMenu()
            {
                Name = "Document_Management",
                Icon = "storage",
                Claim = "Admin.Permissions.Menu.DMS.Management",
                Children = new []
                {
                    new SideMenu()
                    {
                        Name = "Template_List",
                        Path = "/template-list",
                        Icon = "panorama_fish_eye",
                        Claim = "Admin.Permissions.Menu.DMS.Management"
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
    }
}