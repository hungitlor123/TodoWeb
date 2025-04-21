namespace TodoWeb.Appllication.Common;

public class PagaResult<T>
{
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }

    public int PageIndex { get; set; }
    public int PageSize { get; set; }

    public List<T> Items { get; set; }
}