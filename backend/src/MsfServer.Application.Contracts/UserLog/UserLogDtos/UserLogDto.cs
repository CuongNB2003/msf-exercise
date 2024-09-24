
using MsfServer.Application.Contracts.User.UserDtos;
using System.ComponentModel.DataAnnotations.Schema;

namespace MsfServer.Application.Contracts.UserLog.UserLogDtos
{
    public class UserLogDto
    {
        public int UserId { get; set; }
        public string? Path { get; set; }
        public string? Method { get; set; }
        // dùng để gán khi truy vấn
        public string? RoleName { get; set; }
        public string? UserName { get; set; }
        public string? UserEmail { get; set; }


        // Phương thức tạo UserLogDto
        public static UserLogDto CreateUserLog(int userId, string? path, string? method)
        {
            return new UserLogDto
            {
                UserId = userId,
                Path = path,
                Method = method
            };
        }
    }
}
