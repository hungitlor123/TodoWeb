namespace TodoWeb.Appllication.Params;

public class StudentQueryParameters
{
    public int? SchoolId { get; set; }
    public string? Keyword { get; set; }

    public string SortBy { get; set; } = "Id";
    public bool SortDesc { get; set; } = false;

    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}