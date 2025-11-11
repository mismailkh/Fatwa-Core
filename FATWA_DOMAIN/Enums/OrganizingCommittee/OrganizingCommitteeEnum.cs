using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Enums.OrganizingCommittee
{
    public class OrganizingCommitteeEnum
    {
        public enum CommitteeRoleEnum
        {
            CommitteeHead = 1,
            TeamMember = 2,
            Spectator = 4,
            Organizer = 8,
            ViceCommitteeHead = 16,
        }
        public enum CommitteeStatusEnum
        {
            Created = 1,
            Dissolved = 2,
            Draft = 4,
        }
    }
}
