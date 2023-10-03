namespace SearchService;

public class SearchParams
{
    public string SearchTerm { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 4;
    public string User { get; set; }
    public string ExpertName { get; set; }
    public string UserSelectingTopics { get; set; }
    public string FilterBy { get; set; }
}
