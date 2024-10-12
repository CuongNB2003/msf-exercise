namespace MsfServer.Application.Contracts.Menu.Dto
{
    public class MenuRoleResponse
    {
        public int Id { get; set; }
        public string? DisplayName { get; set; }
        public string? Url { get; set; }
        public string? IconName { get; set; }
        public bool Status { get; set; }
    }
}
