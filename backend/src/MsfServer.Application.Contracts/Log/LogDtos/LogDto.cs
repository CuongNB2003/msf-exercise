
namespace MsfServer.Application.Contracts.Log.LogDtos
{
    public class LogDto
    {
        public int UserId { get; set; }
        public string? Path { get; set; }
        public string? Method { get; set; }
        // dùng để gán khi truy vấn
        public string? RoleName { get; set; }
        public string? UserName { get; set; }
        public string? UserEmail { get; set; }


        // Phương thức tạo UserLogDto
        public static LogDto CreateUserLog(int userId, string? path, string? method)
        {
            return new LogDto
            {
                UserId = userId,
                Path = path,
                Method = method
            };
        }
    }
}
