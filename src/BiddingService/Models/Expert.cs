using MongoDB.Entities;

namespace SelectingService;

public class Expert : Entity
{
    public DateTime LastAdviseGivenAt { get; set; }
    public string ExpertName { get; set; }
    public int Available { get; set; }
    public string Accepted { get; set; }


}
