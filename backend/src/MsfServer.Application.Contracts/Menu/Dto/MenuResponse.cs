
using System.ComponentModel.DataAnnotations;

namespace MsfServer.Application.Contracts.Menu.Dto
{
    public class MenuResponse
    {
        public int Id { get; set; }
        public string? DisplayName { get; set; }
        public string? Url { get; set; }
        public string? IconName { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Total { get; set; }
        public int CountRole { get; set; }
    }
}
