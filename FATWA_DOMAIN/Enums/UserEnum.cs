using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace FATWA_DOMAIN.Enums
{
    public class UserEnum
    {
        public enum Roles
        {
            SuperAdmin,
            [Display(Name = "Fatwa Admin")]
            FatwaAdmin,
            Basic,
            [Display(Name = "Cases HOS")]
            HOS,
            [Display(Name = "Cases Lawyer")]
            Lawyer,
            [Display(Name = "Cases Supervisor")]
            Supervisor,
            [Display(Name = "Messenger")]
            Messenger,
            [Display(Name = "Consultation HOS")]
            COMSHOS,
            [Display(Name = "Consultation Lawyer")]
            COMSLAWYER,
            [Display(Name = "Consultation Supervisor")]
            COMSSupervisor,
            [Display(Name = "Library Admin")]
            LMSAdmin
        }



        public enum DepartmentEnum
        {
            [Display(Name = "Operational")]
            Operational = 1,
            [Display(Name = "Administrative")]
            Administrative = 2,
        }
        public enum EmployeeTypeEnum
        {
            Internal = 1,
            External = 2
        }

        public enum EmployeeStatusEnum
        {
            Active = 1,
            InActive = 2,
            Resigned = 3,
            Terminated = 4,
            Suspended = 5
        }

        public enum DesignationEnum
        {
            HeadOfSector = 1,
            ViceHOS = 2,
            Lawyer = 3,
            Supervisor = 4
        }

        public enum GenderEnum
        {
            Male = 1,
            Female = 2
        }
    }
}
