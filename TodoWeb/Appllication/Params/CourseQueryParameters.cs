namespace TodoWeb.Appllication.Params;

public class CourseQueryParameters
{
    public string SortBy { get; set; } = "Id";
    public bool SortDesc { get; set; } = false;

    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;

    public string? Keyword { get; set; }
}