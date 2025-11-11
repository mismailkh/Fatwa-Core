namespace FATWA_DOMAIN.Common
{
    //<History Author = 'Hassan Abbas' Date='2023-07-20' Version="1.0" Branch="master"> Static Roles with Keys to be used in the Project</History>
    public static class SystemRoles
    {
        //Admin Roles
        public const string FatwaAdmin = "d6b3075c-3f70-4b44-baa4-1fdc599a6bb2";
        public const string SuperAdmin = "6f46bc3e-19c9-4d41-bb55-280ff7e93d44";
        public const string LLSAdmin = "9578924a-127b-4e33-b15a-5643ecadaf7c";
        public const string BugAdmin = "B69EF02F-96EF-4192-A01F-4C20A0F6809D";
        public static List<string> AdminRoles = new List<string> { FatwaAdmin, SuperAdmin, LLSAdmin , BugAdmin };

        //Case Management Roles
        public const string HOS = "93e5374b-cbd9-494e-92d4-d9d7d44c2c39";
        public const string ViceHOS = "3a07eb32-db29-47a6-8252-900e4d10182c";
        public const string Supervisor = "f2c87c20-3a38-4a20-b238-ec643ebd0df9";
        public const string Lawyer = "8b6cfa36-914a-4430-9feb-627e11715113";
        public const string Messenger = "4eae855f-500f-4912-90fc-fe399fcb6fea";

        public static List<string> CaseRoles = new List<string> { HOS, Supervisor, Lawyer, Messenger };

        //Consultation Management Roles
        public const string ComsHOS = "1dbe8947-fa41-4f1c-a150-fe272e27b06c";
        public const string ComsSupervisor = "ec11b80f-2429-44d0-a5e1-1e144752e579";
        public const string ComsLawyer = "35bae56b-6523-477a-a8ca-bbf6fa2d4647";
        public const string POSHOS = "e1e17355-216f-463b-918e-e4d898e01457";
        public static List<string> ConsultationRoles = new List<string> { ComsHOS, ComsSupervisor, ComsLawyer, POSHOS };

        //Literature Roles
        public const string LMSAdmin = "abe81828-560a-4efa-8bf0-a5f02738bcf6";
        public static List<string> LiteratureRoles = new List<string> { LMSAdmin };

        //Legislation Roles
        public const string LDSReviewer = "cccb3715-0e55-4a1c-a7e2-c12fd20d7a4a";
        public const string LDSUser = "cd8013fc-6d54-45cb-ab92-73286cec48d3";
        public const string LDSEditor = "467af2c7-7bfa-4cf7-bbe8-8198a3340497";
        public static List<string> LegislationRoles = new List<string> { LDSReviewer, LDSUser, LDSEditor };

        //Principle Roles
        public const string LPSReviewer = "a484bb1b-0423-4e12-a3c6-f347a0236d85";
        public const string LPSUser = "fca2fe9b-aba5-416d-bb49-e81819fe563b";
        public const string LPSEditor = "9A203FC5-F85D-423D-B57C-2AC9DDDAEE57";
        public static List<string> PrincipleRoles = new List<string> { LPSReviewer, LPSUser, LPSEditor };

        //Inventory Management Roles
        public const string Procurement = "17908DD3-13B6-407B-AD09-C9E2BED57480";
        public const string Custodian = "89D8B259-B1C0-4213-87A4-34726EF87ECC";
        public const string StoreKeeper = "A6E26590-3F3B-417F-B80E-6F99DFE33FD4";
        // IT SUPPORT ROLE
        public const string ITSupport = "B66BEBA6-675C-436C-8514-5DFEE4690E0A";
        // Add more roles as needed

        //Manager
        public const string Manager = "D047A9CA-29E3-4987-BC4B-A0C911086B63";

        // Leave And Duty Employee Role
        public const string LeaveAndDutyEmployee = "3C2C927A-F7DE-4A84-884E-C694780FB8D8";
        // Add more roles as needed



    }
}
