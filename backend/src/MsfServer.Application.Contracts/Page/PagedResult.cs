
namespace MsfServer.Application.Page
{
    public class PagedResult<T>
    {
        public int TotalRecords { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public required List<T> Data { get; set; }
    }
}
