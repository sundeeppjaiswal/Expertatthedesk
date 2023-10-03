using System.ComponentModel.DataAnnotations.Schema;
using ExpertService.Entities;

namespace ExpertService;

[Table("Topics")]
public class Topics
{
    public Guid Id { get; set; }
    public string TopicName { get; set; }
    public string Description { get; set; }
    public string TopicExp { get; set; }
    public string ImageUrl { get; set; }

    // nav properties
    public Expert Expert { get; set; }
    public Guid ExpertId { get; set; }
}
