using Microsoft.AspNetCore.Authorization;

namespace MsfServer.HttpApi.Sercurity
{
    public class AuthorizePermissionAttribute(string permission) : AuthorizeAttribute
    {
        public string Permission { get; } = permission;
    }

}
