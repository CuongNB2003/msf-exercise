using MsfServer.Application.Contracts.UserLog.UserLogDtos;
using MsfServer.Application.Page;
using MsfServer.Domain.Shared.Responses;

namespace MsfServer.Application.Contracts.UserLog
{
    public interface IUserLogRepository
    {
        // hàm trong controller 
        Task<ResponseObject<PagedResult<UserLogDto>>> GetUserLogsAsync(int page, int limit);
        //Task<ResponseObject<UserLogDto>> GetUserLogByUserIdAsync(int id);
        Task<ResponseText> CreateUserLogAsync(UserLogDto input);
        //Task<ResponseText> UpdateUserLogAsync(UserLogDto input, int id);
        //Task<ResponseText> DeleteUserLogAsync(int id);
        // hàm phụ việc truy vấn
        //Task<bool> CheckRoleNameExistsAsync(string name);
        //Task<int> GetUserCountByRoleIdAsync(int id);
    }
}
