using Microsoft.AspNetCore.Authorization;

namespace MsfServer.HttpApi.Sercurity
{
    public class PermissionRequirement(string permission) : IAuthorizationRequirement
    {
        public string Permission { get; } = permission;
    }

}
