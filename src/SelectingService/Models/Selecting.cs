using MongoDB.Entities;

namespace SelectingService;

public class Selecting : Entity
{
    public string ExpertId { get; set; }
    public string User { get; set; }
    public DateTime UserSelectTime { get; set; } = DateTime.Now;
    public string UserSelectingTopics { get; set; }
    public SelectingStatus SelectingStatus { get; set; }
}
