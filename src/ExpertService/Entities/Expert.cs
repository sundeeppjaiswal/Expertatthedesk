namespace ExpertService.Entities;

public class Expert
{
    public Guid Id { get; set; }
    public int GivenAdvise { get; set; } = 0;
    public string ExpertName { get; set; }
    public string ExpertDept { get; set; }

    public int ExpertExp { get; set; }
    public string CurrentlyConsulting { get; set; }

    public int Available { get; set; }
    public Int64 ContactNo { get; set; }
    public string Email { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public string LastAdviseGivenTo { get; set; }
    public DateTime LastAdviseGivenAt { get; set; }
    public string Testimonial { get; set; }
    public Status Status { get; set; }
    public Topics Topics { get; set; }

    // public bool HasReservePrice() => ReservePrice > 0;
}
