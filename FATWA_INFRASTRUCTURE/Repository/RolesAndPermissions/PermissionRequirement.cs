using Microsoft.AspNetCore.Authorization;

namespace FATWA_INFRASTRUCTURE.Repository.RolesAndPermissions
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public string Permission { get; private set; }

        public PermissionRequirement(string permission)
        {
            Permission = permission;
        }
    }
}