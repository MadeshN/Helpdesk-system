using System.ComponentModel.DataAnnotations;

namespace HelpdeskSystem.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        [Required] public string Title { get; set; }
        public string Description { get; set; }
        [Required] public string Priority { get; set; }
        public string Status { get; set; } = "Open";
        public string CreatedBy { get; set; }
        public string? AssignedTo { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public ICollection<Comment> Comments { get; set; }
    }
}
