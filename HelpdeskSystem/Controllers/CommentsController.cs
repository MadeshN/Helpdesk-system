using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using HelpdeskSystem.Data;
using HelpdeskSystem.Models;
using HelpdeskSystem.Hubs;

namespace HelpdeskSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<NotificationHub> _hubContext;

        public CommentsController(ApplicationDbContext context, IHubContext<NotificationHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        [HttpPost]
        [Authorize(Roles = "User,Agent")]
        public async Task<IActionResult> AddComment(Comment comment)
        {
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
            await _hubContext.Clients.All.SendAsync("ReceiveUpdate", $"New comment added to ticket {comment.TicketId}");
            return Ok(comment);
        }

        [HttpGet("{ticketId}")]
        public async Task<IActionResult> GetComments(int ticketId)
        {
            var comments = await _context.Comments.Where(c => c.TicketId == ticketId).ToListAsync();
            return Ok(comments);
        }
    }
}
