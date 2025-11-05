namespace HelpdeskSystem.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string AddedBy { get; set; }
        public DateTime AddedAt { get; set; } = DateTime.Now;
        public int TicketId { get; set; }
        public Ticket Ticket { get; set; }
    }
}
