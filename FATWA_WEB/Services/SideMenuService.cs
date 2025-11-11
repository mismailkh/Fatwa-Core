//<History Author = 'Hassan Abbas' Date='2022-03-14' Version="1.0" Branch="master"> Service created for sidemenu upto multiple levels with Permissions/Claims</History>
using FATWA_GENERAL.Helper;
using FATWA_WEB.Data;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.LegalPrinciple.LegalPrincipleEnum;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;
using static FATWA_GENERAL.Helper.Enum;

namespace FATWA_WEB.Services
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
        public int[] ModuleId { get; set; } // Added By AttiqueRehman to filter SideMenu, based on Dashboard Module card
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
                Name = "Tasks",
                Icon = "storage",
                Claim = "Permissions.Menu.Task",
                ModuleId = new int[] { (int)ModuleEnum.CaseManagement,(int)ModuleEnum.ConsultationManagement,(int)ModuleEnum.EmployeeManagement, (int)ModuleEnum.CMSCOMS, (int)ModuleEnum.ServiceRequest},
                Children = new []
                {
                    new SideMenu()
                    {
                        Name = "Tasks_List",
                        Path = "/usertask-list",
                        Icon = "panorama_fish_eye",
                        Claim = "Permissions.Submenu.Task.TaskList",
                        ModuleId = new int[] { (int)ModuleEnum.CaseManagement,(int)ModuleEnum.ConsultationManagement, (int)ModuleEnum.CMSCOMS, (int)ModuleEnum.ServiceRequest, (int)ModuleEnum.EmployeeManagement},
                    },
                    new SideMenu()
                    {
                        Name = "Todo_List",
                        Path = "/usertask-todo-list",
                        Icon = "panorama_fish_eye",
                        Claim = "Permissions.Submenu.Task.Taskdashboard",
                        ModuleId = new int[] { (int)ModuleEnum.CaseManagement,(int)ModuleEnum.ConsultationManagement, (int)ModuleEnum.CMSCOMS, (int)ModuleEnum.ServiceRequest, (int)ModuleEnum.EmployeeManagement},
                    },
                }
            },
            new SideMenu()
            {
                Name = "literatureSideMenu",
                Icon = "auto_stories",
                Claim = "Permissions.Menu.LMS",
                ModuleId = new int[]{(int)ModuleEnum.LegalLibrarySystem },
                Children = new [] {
                    new SideMenu()
                    {
                        Name = "Literature_Index",
                        New = false,
                        Path = "lmsliteratureparentindex-list",
                        Icon = "panorama_fish_eye",
                        Claim = "Permissions.Submenu.LMS.LiteratureParentIndex",
                        Tags = new [] { "pager", "paging" },
                        ModuleId = new int[]{(int)ModuleEnum.LegalLibrarySystem },
                    },
                    new SideMenu()
                    {
                        Name = "Literature",
                        Path = "lmsliterature-list",
                        Icon = "panorama_fish_eye",
                        Claim = "Permissions.Submenu.LMS.Literatures",
                        Tags = new [] { "dataview", "grid", "table" },
                        ModuleId = new int[]{(int)ModuleEnum.LegalLibrarySystem }

                    },
                    new SideMenu()
                    {
                        Name = "Borrow_Return_Books",
                        Path = "borrow-returnbooks",
                        Icon = "panorama_fish_eye",
                        Claim = "Permissions.Submenu.LMS.Literatures.ReturnBorrowExtend",
                        Tags = new [] { "pager", "paging" },
                        ModuleId = new int[]{(int)ModuleEnum.LegalLibrarySystem }
                    },
                    new SideMenu()
                    {
                        Name = "StockTaking_List",
                        Path = "stocktaking-list" ,
                        Icon = "panorama_fish_eye",
                        Claim = "Permissions.Submenu.LMS.StockTaking.List",
                        ModuleId = new int[] { (int)ModuleEnum.LegalLibrarySystem}
                    },
                    new SideMenu()
                    {
                        Name = "Viewable_Literature",
                        Path = "lmsliterature-download",
                        Icon = "panorama_fish_eye",
                        Claim = "Permissions.Submenu.LMS.ViewableLiteratureList",
                        Tags = new [] { "dataview", "grid", "table" },
                        ModuleId = new int[] { (int)ModuleEnum.LegalLibrarySystem }
                    },
                }
            },
            new SideMenu()
            {
                Name = "Legal_Legislation_SideMenu_Heading",
                Icon = "description",
                Claim = "Permissions.Menu.LDS",
                ModuleId = new int[]{(int)ModuleEnum.LegalLibrarySystem },
                Children = new [] {
                    new SideMenu()
                    {
                        Name = "Legislation_Heading",
                        Path = "legallegislation-list",
                        Icon = "panorama_fish_eye",
                        Claim = "Permissions.Submenu.LDS.Legislation.List",
                        Tags = new [] { "dataview", "grid", "table" },
                        ModuleId = new int[]{(int)ModuleEnum.LegalLibrarySystem }
                    },
                    new SideMenu()
                    {
                        Name = "Legal_Legislation_Review",
                        Path = "legallegislation-review",
                        Icon = "panorama_fish_eye",
                        Claim = "Permissions.Submenu.LDS.Documents.DocumentReview",
                        Tags = new [] { "dataview", "grid", "table" },
                        ModuleId = new int[]{(int)ModuleEnum.LegalLibrarySystem }
                    },
                    new SideMenu()
                    {
                        Name = "Legal_Legislation_Approved",
                        Path = "legallegislation-approve",
                        Icon = "panorama_fish_eye",
                        Claim = "Permissions.Submenu.LDS.Documents.DocumentApproval",
                        Tags = new [] { "dataview", "grid", "table" },
                        ModuleId = new int[]{(int)ModuleEnum.LegalLibrarySystem }
                    },
                    new SideMenu()
                    {
                        Name = "Legal_Legislation_Publish_and_UnPublish",
                        Path = "legallegislation-publish-unpublish",
                        Icon = "panorama_fish_eye",
                        Claim = "Permissions.Submenu.LDS.Documents.PublishUnpublish",
                        Tags = new [] { "dataview", "grid", "table" },
                        ModuleId = new int[]{(int)ModuleEnum.LegalLibrarySystem }
                    },
                     new SideMenu()
                    {
                        Name = "Legal_Legislation_Delete",
                        Path = "legallegislation-delete",
                        Icon = "panorama_fish_eye",
                        Claim = "Permissions.Submenu.LDS.Documents.List.Delete",
                        Tags = new [] { "dataview", "grid", "table" },
                        ModuleId = new int[]{(int)ModuleEnum.LegalLibrarySystem }
                    }
                }
            },
            new SideMenu()
            {
                Name = "Legal_Principle_SideMenu_Heading",
                Icon = "gavel",
                Claim = "Permissions.Menu.LPS",
                ModuleId = new int[]{(int)ModuleEnum.LegalLibrarySystem },
                Children = new [] {
                      new SideMenu()
                    {
                        Name = "Legal_Principles_Heading",
                        Path = "llslegalprincipledocuments-list",
                        Icon = "panorama_fish_eye",
                        Claim = "Permissions.Submenu.LPS.Principles.List",
                        Tags = new [] { "dataview", "grid", "table" },
                        ModuleId = new int[]{(int)ModuleEnum.LegalLibrarySystem }
                    },
                    new SideMenu()
                    {
                        Name = "Review_Legal_Principles",
                        Path = $"/legalprinciple-inreview",
                        Icon = "panorama_fish_eye",
                        Claim = "Permissions.LPS.Principles.Review",
                        Tags = new [] { "dataview", "grid", "table" },
                        ModuleId = new int[]{(int)ModuleEnum.LegalLibrarySystem }
                    },
                    new SideMenu()
                    {
                        Name = "Approved_Principles_Submenu",
                        Path = $"/legalprinciple-approved",
                        Icon = "panorama_fish_eye",
                        Claim = "Permissions.Submenu.LPS.Principles.PrincipleApproval",
                        Tags = new [] { "dataview", "grid", "table" },
                        ModuleId = new int[]{(int)ModuleEnum.LegalLibrarySystem }
                    },
                    new SideMenu()
                    {
                        Name = "Publish_Unpublish_Legal_Principles",
                        Path = "legalprinciple-publishUnpublish",
                        Icon = "panorama_fish_eye",
                        Claim = "Permissions.LPS.Principles.PublishUnpublish",
                        Tags = new [] { "dataview", "grid", "table" },
                        ModuleId = new int[]{(int)ModuleEnum.LegalLibrarySystem }
                    },
                    new SideMenu()
                    {
                        Name = "Legal_Principles_Hierarchy",
                        Path = "principle_hierarchy",
                        Icon = "panorama_fish_eye",
                        Claim = "Permissions.Submenu.LPS.PrinciplesHierarchy.List",
                        Tags = new [] { "dataview", "grid", "table" },
                        ModuleId = new int[]{(int)ModuleEnum.LegalLibrarySystem }
                    }
                }
            },
            new SideMenu()
            {
                Name = "Case_Managment",
                Icon = "cases",
                Claim = "Permissions.Menu.CMS",
                Class = "parent-menu",
                ModuleId = new int[] {(int) ModuleEnum.CaseManagement, (int)ModuleEnum.CMSCOMS },
                Children = new [] {
                    new SideMenu()
                    {
                        Name = "Case_Requests",
                        Path = "case-requests",
                        Icon = "panorama_fish_eye",
                        Claim = "Permissions.Submenu.CMS.CaseRequest.List",
                        Tags = new [] { "dataview", "grid", "table" },
                ModuleId = new int[] {(int) ModuleEnum.CaseManagement, (int)ModuleEnum.CMSCOMS },
                    },
                    new SideMenu()
                    {
                        Name = "Under_Filing",
                        Path = "registerd-requests",
                        Icon = "panorama_fish_eye",
                        Claim = "Permissions.Submenu.CMS.CaseFile.List",
                        Tags = new [] { "dataview", "grid", "table" },
                ModuleId = new int[] {(int) ModuleEnum.CaseManagement },
                    },
                    new SideMenu()
                    {
                        Name = "Case_Files",
                        Path = "case-files",
                        Icon = "panorama_fish_eye",
                        Claim = "Permissions.Submenu.CMS.CaseFile.List",
                        Tags = new [] { "dataview", "grid", "table" },
                        ModuleId = new int[] {(int) ModuleEnum.CaseManagement, (int)ModuleEnum.CMSCOMS },
                    },
                    new SideMenu()
                    {
                        Name = "Unassigned_Case_Files",
                        Path = "unassigned-case-files",
                        Icon = "panorama_fish_eye",
                        Claim = "Permissions.Submenu.CMS.CaseFile.UnassignedList",
                        Tags = new [] { "dataview", "grid", "table" },
                        ModuleId = new int[] {(int) ModuleEnum.CaseManagement, (int)ModuleEnum.CMSCOMS },
                    },
                    new SideMenu()
                    {
                        Name = "Moj_Registration_Requests",
                        Path = "moj-registration-requests",
                        Icon = "panorama_fish_eye",
                        Claim = "Permissions.Submenu.CMS.MOJ.RegistrationList",
                        Tags = new [] { "dataview", "grid", "table" },
                        ModuleId = new int[] {(int) ModuleEnum.CaseManagement, (int)ModuleEnum.CMSCOMS }
                    },
                    new SideMenu()
                    {
                        Name = "Moj_Document_Portfolio_Requests",
                        Path = "moj-portfolio-requests",
                        Icon = "panorama_fish_eye",
                        Claim = "Permissions.CMS.DocumentPortfolio.MojList",
                        Tags = new [] { "dataview", "grid", "table" },
                        ModuleId = new int[] {(int) ModuleEnum.CaseManagement }
                    },
                    new SideMenu()
                    {
                        Name = "Moj_Execution_Requests",
                        Path = "execution-requests",
                        Icon = "panorama_fish_eye",
                        Claim = "Permissions.CMS.MOJ.ExecutionList",
                        Tags = new [] { "dataview", "grid", "table" },
                        ModuleId = new int[] {(int) ModuleEnum.CaseManagement, (int)ModuleEnum.CMSCOMS },
                    },
                    new SideMenu()
                    {
                        Name = "Regenrate_Execution_Requests",
                        Path = "decision-requests",
                        Icon = "panorama_fish_eye",
                        Claim = "Permissions.CMS.Case.DecisionList",
                        Tags = new [] { "dataview", "grid", "table" },
                        ModuleId = new int[] {(int) ModuleEnum.CaseManagement , (int)ModuleEnum.CMSCOMS }
                    },
                    new SideMenu()
                    {
                        Name = "Assigned_lawyer_list",
                        Path = "assigned-lawyertocourt-list",
                        Icon = "panorama_fish_eye",
                        Claim = "Permissions.Submenu.CMS.LawyerChamberAssignment.List",
                        Tags = new [] { "dataview", "grid", "table" },
                        ModuleId = new int[] {(int) ModuleEnum.CaseManagement }
                    },
                    new SideMenu()
                    {
                        Name = "Upcoming_And_Current_Hearings",
                        Path = "current-and-previous-hearings",
                        Icon = "panorama_fish_eye",
                        Claim = "Permissions.Submenu.CMS.HearingsList",
                        Tags = new [] { "dataview", "grid", "table" },
                        ModuleId = new int[] {(int) ModuleEnum.CaseManagement }
                    },
                    new SideMenu()
                    {
                        Name = "Upcoming_Hearing_Rolls",
                        Path = "upcominghearings-rolls-list",
                        Icon = "panorama_fish_eye",
                        Claim = "Permissions.Submenu.CMS.UpcomingHearingRollsList",
                        Tags = new [] { "dataview", "grid", "table" },
                        ModuleId = new int[] {(int) ModuleEnum.CaseManagement }
                    },
                    new SideMenu()
                    {
                        Name = "Sector_Users",
                        Path = "sector-users",
                        Icon = "panorama_fish_eye",
                        Claim = "Permissions.CMS.SectorUsersList",
                        Tags = new [] { "dataview", "grid", "table" },
                        ModuleId = new int[] {(int) ModuleEnum.CaseManagement }
                    }
                }
            },
            new SideMenu()
            {
                Name = "Archived_Cases",
                Icon = "archive",
                Claim = "Permissions.ARC.ArchivedCases.List",
                Path = "archivedcases-list",
                Class = "parent-menu",
                ModuleId = new int[] {(int)ModuleEnum.CaseManagement }
            },
            new SideMenu()
            {
                Name = "Archiving_Reports",
                Icon = "library_books",
                Claim = "Permissions.ARC.ArchivedCases.List",
                Path = "archiving-report-list",
                Class = "parent-menu",
                ModuleId = new int[] {(int)ModuleEnum.CaseManagement }
            },
            new SideMenu()
            {
                Name = "Consultation_Managment",
                Icon = "assessment",
                Claim = "Permissions.Menu.COMS",
                ModuleId = new int[] {(int) ModuleEnum.ConsultationManagement, (int)ModuleEnum.CMSCOMS },
                Children = new []
                {
                    new SideMenu()
                    {
                        Name = "Consultation_Request",
                        Path = "consultationrequest-list/" ,
                        Icon = "panorama_fish_eye",
                        Claim = "Permissions.Submenu.COMS.Request.List",
                        ModuleId = new int[] {(int) ModuleEnum.ConsultationManagement, (int)ModuleEnum.CMSCOMS }
                    },
                    new SideMenu()
                    {
                        Name = "Consultation_Files",
                        Path = "consultationfile-list/",
                        Icon = "panorama_fish_eye",
                        Claim = "Permissions.Submenu.COMS.ConsultationFile.List",
                        Tags = new [] { "dataview", "grid", "table" },
                        ModuleId = new int[] {(int) ModuleEnum.ConsultationManagement, (int)ModuleEnum.CMSCOMS }
                    },
                    new SideMenu()
                    {
                        Name = "Sector_Users",
                        Path = "sector-users",
                        Icon = "panorama_fish_eye",
                        Claim = "Permissions.COMS.SectorUsersList",
                        Tags = new [] { "dataview", "grid", "table" },
                        ModuleId = new int[] {(int) ModuleEnum.ConsultationManagement, (int)ModuleEnum.CMSCOMS }
                    }
                }
            },
            new SideMenu()
            {
                Name = "CMS_COMS",
                Path = "/request-list",
                Icon = "mail",
                Claim = "Permissions.Submenu.CMS/COMS.Request.List",
                ModuleId = new int[] {(int) ModuleEnum.CaseManagement, (int)ModuleEnum.CMSCOMS }
            },
            new SideMenu()
            {
                Name = "Correspondences",
                Path = "/inboxOutbox-list",
                Icon = "mail",
                Claim = "Permissions.Menu.InboxOutbox",
                ModuleId = new int[] { (int)ModuleEnum.CaseManagement,(int) ModuleEnum.ConsultationManagement, (int)ModuleEnum.CMSCOMS, (int)ModuleEnum.Communication }
            },
            new SideMenu()
            {
                Name = "Contact_Management",
                Path = "/contact-list",
                Icon = "contact_page",
                Claim = "Permissions.Menu.Contact",
                ModuleId = new int[] { (int)ModuleEnum.CaseManagement,(int) ModuleEnum.ConsultationManagement, (int)ModuleEnum.CMSCOMS}
            },
            new SideMenu()
            {
            Name = "Organizing_Committee",
            Icon = "interpreter_mode",
            Claim = "Permissions.Menu.OrganizingCommittee",
            ModuleId=new int[] {(int)ModuleEnum.OrganizingCommittee, (int)ModuleEnum.CaseManagement, (int)ModuleEnum.ConsultationManagement, (int)ModuleEnum.CMSCOMS, (int)ModuleEnum.LegalLibrarySystem},
            Children = new []
            {
                new SideMenu()
                {
                    Name = "Committee_List",
                    Path = "/list-committee",
                    Icon = "panorama_fish_eye",
                    Claim = "Permissions.Submenu.OrganizingCommittee.CommitteeList",
                    ModuleId=new int[] {(int)ModuleEnum.OrganizingCommittee,(int)ModuleEnum.CaseManagement,(int)ModuleEnum.ConsultationManagement,(int)ModuleEnum.LegalLibrarySystem },
                },
            }
            },
            new SideMenu()
            {
                Name = "Notifications",
                Path = "/notifications",
                Icon = "notifications",
                Claim = "Permissions.Menu.Notfication",
                ModuleId = new int[] { (int)ModuleEnum.CaseManagement,(int) ModuleEnum.ConsultationManagement, (int)ModuleEnum.CMSCOMS, (int)ModuleEnum.ServiceRequest, (int)ModuleEnum.EmployeeManagement, (int)ModuleEnum.OrganizingCommittee}
            },
            new SideMenu()
            {
                Name = "Meetings",
                Path = "/meeting-list",
                Icon = "groups",
                Claim = "Permissions.Menu.Meeting",
                ModuleId = new int[] { (int)ModuleEnum.CaseManagement,(int) ModuleEnum.ConsultationManagement, (int)ModuleEnum.CMSCOMS}
            },
            new SideMenu()
            {
                Name = "Employee_Management",
                Path = "/employee-list",
                Icon = "groups",
                Claim = "Permissions.Menu.Employee",
                ModuleId = new int[] {(int) ModuleEnum.EmployeeManagement }
            },
            new SideMenu()
            {
                Name = "Logs",
                Icon = "sync_alt",
                Claim = "Permissions.Menu.AuditLogs",
                ModuleId = new int[] { (int)ModuleEnum.EmployeeManagement, (int)ModuleEnum.CMSCOMS },
                Children = new []
                {
                    new SideMenu()
                    {
                        Name = "ErrorLogs",
                        Path = "/errorlogs",
                        Icon = "panorama_fish_eye",
                        ModuleId = new int[] { (int)ModuleEnum.EmployeeManagement, (int)ModuleEnum.CMSCOMS},
                        Claim = "Permissions.Submenu.AuditLogs.ErrorLogs",
                    },
                    new SideMenu()
                    {
                        Name = "Operation_logs",
                        Path = "processlogs",
                        Icon = "panorama_fish_eye",
                        ModuleId = new int[] { (int)ModuleEnum.EmployeeManagement, (int)ModuleEnum.CMSCOMS },
                        Claim = "Permissions.Submenu.AuditLogs.ProcessLogs",
                    },
                }
            },
            new SideMenu()
            {
                Name = "MOJ_Images_Document",
                Path = "/moj-imagedocument-list",
                Icon = "description",
                ModuleId = new int[] {(int) ModuleEnum.DocumentManagement },
                Claim = "Permissions.DMS.MojImage.DocumentList"
            },
            new SideMenu()
            {
                Name = "Document_List",
                Path = "/document-list",
                Icon = "description",
                ModuleId = new int[] {(int) ModuleEnum.CaseManagement, (int)ModuleEnum.LegalPrinciplesManagement, (int)ModuleEnum.LegislationManagement },
                Claim = "Permissions.Submenu.Document.DocumentList"
            },
            new SideMenu()//Parent WorkFlow
            {
                Name = "Workflows",
                Icon = "move_up",
                Claim = "Permissions.WF.Workflow.List",
                ModuleId = new int[] {(int) ModuleEnum.Workflow },
                Children = new []
                {
                    new SideMenu()
                    {
                        Name = "Workflows_List",
                        Path = "/workflows",
                        Icon = "panorama_fish_eye",
                        Claim = "Permissions.WF.Workflow.List",
                        ModuleId = new int[] {(int) ModuleEnum.Workflow }
                    },
                    new SideMenu()
                    {
                        Name = "Workflow_Instances",
                        Path = "/workflow-instances",
                        Icon = "panorama_fish_eye",
                        Claim = "Permissions.WF.WorkflowInstance.List",
                        ModuleId = new int[] {(int) ModuleEnum.Workflow }
                    },
                }
            },
            new SideMenu()
            {
                Name = "On_Demand_Requests",
                Icon = "move_up",
                Claim = "Permissions.Menu.ODRP",
                ModuleId = new int[] {(int) ModuleEnum.ODRP },
                Children = new []
                {
                      new SideMenu()
                    {
                        Name = "MOJ_Rolls",
                        Path = "/rolls-request-list",
                        Icon = "panorama_fish_eye",
                        Claim = "Permissions.ODRP.MojRolls.List",
                        ModuleId = new int[] {(int) ModuleEnum.ODRP }
                    },
                    new SideMenu()
                    {
                        Name = "PACI_Address_Query",
                        Path = "/paci-address-list",
                        Icon = "panorama_fish_eye",
                        Claim = "Permissions.ODRP.PACI.List",
                        ModuleId = new int[] {(int) ModuleEnum.ODRP }
                    },
                    new SideMenu()
                    {
                        Name = "Moj_Statistics",
                        Path = "/statistics-list",
                        Icon = "panorama_fish_eye",
                        Claim = "Permissions.ODRP.MojStatistics.List",
                        ModuleId = new int[] {(int) ModuleEnum.ODRP }
                    },
                    new SideMenu()
                    {
                        Name = "Moj_Statistics_Case_Study",
                        Path = "/case-study-list",
                        Icon = "panorama_fish_eye",
                        Claim = "Permissions.ODRP.MojStatisticsCaseStudy.List",
                        ModuleId = new int[] {(int) ModuleEnum.ODRP }
                    },
                    new SideMenu()
                    {
                        Name = "Kuwait_AlYawm",
                        Path = "/kaydocument-list",
                        Icon = "panorama_fish_eye",
                        Claim = "Permissions.ODRP.KAY.List",
                        ModuleId = new int[] {(int) ModuleEnum.ODRP }
                    },
                }
            },
            new SideMenu()
            {
                Name = "Service_Requests",
                Icon = "description",
                Claim = "Permissions.Menu.SR.ServiceRequest",
                ModuleId = new int[] {(int) ModuleEnum.ServiceRequest },
                Children = new []
                {
                    new SideMenu()
                    {
                        Name = "Service_Requests",
                        Path = "/servicerequest-list",
                        Icon = "panorama_fish_eye",
                        Claim = "Permissions.Submenu.SR.ServiceRequest.List",
                        ModuleId = new int[] {(int) ModuleEnum.ServiceRequest },
                        Tags = new [] { "dataview", "grid", "table" }
                    },
                    new SideMenu()
                    {
                        Name = "Orders",
                        Path = "/orders",
                        Icon = "panorama_fish_eye",
                        Claim = "Permissions.Submenu.INV.Inventory.Orders.List",
                        ModuleId = new int[] {(int) ModuleEnum.InventoryManagement },
                        Tags = new [] { "dataview", "grid", "table" }
                    },
                    new SideMenu()
                    {
                        Name = "List_of_Store",
                        Path = "/list-of-store",
                        Icon = "panorama_fish_eye",
                        Claim = "Permissions.Submenu.INV.Inventory.Store.List",
                        ModuleId = new int[] {(int) ModuleEnum.InventoryManagement },
                        Tags = new [] { "dataview", "grid", "table" }
                    }

                }
            },
            new SideMenu()
            {
                Name = "Time_Log",
                Icon = "schedule",
                Claim = "Permissions.Menu.Timelog",
                ModuleId = new int[] { (int)ModuleEnum.CaseManagement,(int) ModuleEnum.ConsultationManagement, (int)ModuleEnum.CMSCOMS },
                Children = new []
                {
                    new SideMenu()
                            {
                                Name = "Consultation_Request",
                                Path = "time-log-consultation-request-list" ,
                                Icon = "panorama_fish_eye",
                                Claim = "Permissions.Submenu.COMS.Request.List",
                                ModuleId = new int[] {(int) ModuleEnum.ConsultationManagement, (int)ModuleEnum.CMSCOMS }
                            },
                    new SideMenu()
                            {
                                Name = "Consultation_Files",
                                Path = "time-log-consultioation-file-list",
                                Icon = "panorama_fish_eye",
                                Claim = "Permissions.Submenu.COMS.ConsultationFile.List",
                                Tags = new [] { "dataview", "grid", "table" },
                                ModuleId = new int[] {(int) ModuleEnum.ConsultationManagement }
                            },
                    new SideMenu()
                            {
                                Name = "Case_Requests",
                                Path = "time-log-case-request-list",
                                Icon = "panorama_fish_eye",
                                Claim = "Permissions.Submenu.CMS.CaseRequest.List",
                                Tags = new [] { "dataview", "grid", "table" },
                                ModuleId = new int[] {(int) ModuleEnum.CaseManagement, (int)ModuleEnum.CMSCOMS },
                            },
                    new SideMenu()
                        {
                            Name = "Under_Filing",
                            Path = "time-log-Registercase-list",
                            Icon = "panorama_fish_eye",
                            Claim = "Permissions.Submenu.CMS.CaseFile.List",
                            Tags = new [] { "dataview", "grid", "table" },
                            ModuleId = new int[] {(int) ModuleEnum.CaseManagement },
                        },
                    new SideMenu()
                            {
                                Name = "Case_Files",
                                Path = "time-log-case-file",
                                Icon = "panorama_fish_eye",
                                Claim = "Permissions.Submenu.CMS.CaseFile.List",
                                Tags = new [] { "dataview", "grid", "table" },
                                ModuleId = new int[] {(int) ModuleEnum.CaseManagement, (int)ModuleEnum.CMSCOMS },
                            },
                    new SideMenu()
                    {
                        Name = "CMS/COMS",
                        Path = "/timelog-casecoms-request-list",
                        Icon = "mail",
                        Claim = "Permissions.Submenu.CMS/COMS.Request.List",
                        ModuleId = new int[] {(int) ModuleEnum.CaseManagement, (int)ModuleEnum.CMSCOMS }
                    },
                },
            },
            new SideMenu()
              {
                Name = "Bug_Reporting",
                Icon = "bug_report",
                Claim = "Permissions.Menu.BugReporting",
                ModuleId = new int[] { (int)ModuleEnum.CaseManagement,(int) ModuleEnum.ConsultationManagement, (int)ModuleEnum.CMSCOMS, (int)ModuleEnum.LibraryManagement,(int)ModuleEnum.LegalLibrarySystem,(int)ModuleEnum.InventoryManagement,(int)ModuleEnum.ODRP,(int)ModuleEnum.DocumentManagement,(int)ModuleEnum.Workflow,(int)ModuleEnum.EmployeeManagement},
                Children = new [] {

                    new SideMenu()
                    {
                        Name = "List_Bug_Ticket",
                        Path = "/list-bugticket",
                        Claim = "Permissions.SubMenu.Ticket.List",
                        Icon="panorama_fish_eye",
                        ModuleId = new int[] { (int)ModuleEnum.CaseManagement,(int) ModuleEnum.ConsultationManagement, (int)ModuleEnum.CMSCOMS, (int)ModuleEnum.LibraryManagement,(int)ModuleEnum.LegalLibrarySystem,(int)ModuleEnum.InventoryManagement,(int)ModuleEnum.ODRP,(int)ModuleEnum.DocumentManagement,(int)ModuleEnum.Workflow,(int)ModuleEnum.EmployeeManagement}

                    },
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