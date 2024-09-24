using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace MsfServer.HttpApi.Host.Extensions
{
    public class CustomJwtBearerEvents : JwtBearerEvents
    {
        public override Task MessageReceived(MessageReceivedContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault();
            if (!string.IsNullOrEmpty(token))
            {
                context.Token = token;
            }
            return Task.CompletedTask;
        }
    }
}

