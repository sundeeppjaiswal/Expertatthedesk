
using System.ComponentModel.DataAnnotations;
public class CreateExpertDto
{
    [Required]
    public string TopicName { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public string TopicExp { get; set; }
    [Required]
    public string ImageUrl { get; set; }

}