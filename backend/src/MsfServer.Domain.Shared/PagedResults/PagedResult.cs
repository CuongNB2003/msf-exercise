namespace MsfServer.Domain.Shared.PagedResults
{
    public class PagedResult<T>
    {
        public int TotalRecords { get; set; }
        public int Page { get; set; }
        public int Limit { get; set; }
        public required List<T> Data { get; set; }
    }
}
