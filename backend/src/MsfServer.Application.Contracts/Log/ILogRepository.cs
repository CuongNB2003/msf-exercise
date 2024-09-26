using MsfServer.Application.Contracts.Log.LogDtos;
using MsfServer.Domain.Shared.PagedResults;
using MsfServer.Domain.Shared.Responses;

namespace MsfServer.Application.Contracts.Log
{
    public interface ILogRepository
    {
        // hàm trong controller 
        Task<ResponseObject<PagedResult<LogDto>>> GetLogsAsync(int page, int limit);
        Task<ResponseObject<LogDto>> GetLogByIdAsync(int id);
        //Task<ResponseText> CreateLogAsync(LogDto input);
        //Task<ResponseText> UpdateUserLogAsync(UserLogDto input, int id);
        //Task<ResponseText> DeleteUserLogAsync(int id);
        // hàm phụ việc truy vấn
        //Task<bool> CheckRoleNameExistsAsync(string name);
        //Task<int> GetUserCountByRoleIdAsync(int id);
    }
}
