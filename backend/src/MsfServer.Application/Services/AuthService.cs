using MsfServer.Application.Contracts.Authentication;
using MsfServer.Application.Contracts.Authentication.AuthDtos;
using MsfServer.Application.Contracts.Users.UserDtos;
using MsfServer.Domain.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsfServer.Application.Services
{
    public class AuthService : IAuthService
    {
        public Task<ResponseObject<UserResultDto>> GetUserAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ResponseObject<LoginResultDto>> LoginAsync(LoginInputDto input)
        {
            throw new NotImplementedException();
        }

        public Task LogoutAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ResponseText> RegisterAsync(RegisterInputDto input)
        {
            throw new NotImplementedException();
        }
    }
}
