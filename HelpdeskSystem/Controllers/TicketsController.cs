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
    public class TicketsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<NotificationHub> _hubContext;

        public TicketsController(ApplicationDbContext context, IHubContext<NotificationHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        [HttpGet]
        [Authorize(Roles = "Agent")]
        public async Task<IActionResult> GetAllTickets() => Ok(await _context.Tickets.ToListAsync());

        [HttpPost]
        [Authorize(Roles = "User,Agent")]
        public async Task<IActionResult> CreateTicket(Ticket ticket)
        {
            ticket.CreatedAt = DateTime.Now;
            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();
            return Ok(ticket);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Agent")]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null) return NotFound();
            ticket.Status = status;
            ticket.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            await _hubContext.Clients.All.SendAsync("ReceiveUpdate", $"Ticket {ticket.Id} status updated to {status}");
            return Ok(ticket);
        }
    }
}
