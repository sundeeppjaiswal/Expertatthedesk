using MongoDB.Entities;

namespace SearchService;

public class Item : Entity
{
   
    public int GivenAdvise { get; set; } = 0;
    public string ExpertName { get; set; }
    public string ExpertDept { get; set; }

    public int ExpertExp { get; set; }
    public int Available { get; set; }
    public Int64 ContactNo { get; set; }
    public string Email { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string LastAdviseGivenTo { get; set; }
    public DateTime LastAdviseGivenAt { get; set; }
    public string Testimonial { get; set; }
    public string Status { get; set; }
    public string TopicName { get; set; }
    public string Description { get; set; }
    public string TopicExp { get; set; }
   
}
