using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HelpDesk.Api.Models;
using HelpDesk.Api.Repositories;

namespace HelpDesk.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketController : ControllerBase
    {
        private readonly ITicketRepository _repository;

        public TicketController(ITicketRepository repository)
        {
            _repository = repository;
        }

        // GET /api/Ticket/All
        [HttpGet("All")]
        public async Task<IActionResult> GetAllTickets()
        {
            var tickets = await _repository.GetAllTicketsAsync();
            return Ok(tickets);
        }

        // GET /api/Ticket/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTicketById(int id)
        {
            var ticket = await _repository.GetTicketByIdAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            return Ok(ticket);
        }

        // POST /api/Ticket
        [HttpPost]
        public async Task<IActionResult> CreateTicket([FromBody] Ticket ticket)
        {
            if (ticket == null)
            {
                return BadRequest("Ticket object is null");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Set default created date if not set
            if (ticket.CreatedDate == default)
            {
                ticket.CreatedDate = DateTime.Now;
            }

            var ticketId = await _repository.CreateTicketAsync(ticket);
            ticket.Id = ticketId; // Make sure the returned ticket has the ID populated
            return Ok(ticket);
        }

        // PUT /api/Ticket/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTicket(int id, [FromBody] Ticket ticket)
        {
            if (ticket == null)
            {
                return BadRequest("Ticket object is null");
            }

            if (id != ticket.Id)
            {
                return BadRequest("ID in URL path does not match ID in body");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingTicket = await _repository.GetTicketByIdAsync(id);
            if (existingTicket == null)
            {
                return NotFound();
            }

            await _repository.UpdateTicketAsync(ticket);
            return Ok();
        }

        // DELETE /api/Ticket/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            var existingTicket = await _repository.GetTicketByIdAsync(id);
            if (existingTicket == null)
            {
                return NotFound();
            }

            await _repository.DeleteTicketAsync(id);
            return Ok();
        }

        // GET /api/Ticket/Status/{status}
        [HttpGet("Status/{status}")]
        public async Task<IActionResult> GetTicketsByStatus(string status)
        {
            if (string.IsNullOrEmpty(status))
            {
                return BadRequest("Status cannot be empty");
            }

            var tickets = await _repository.GetTicketsByStatusAsync(status);
            return Ok(tickets);
        }
    }
}
