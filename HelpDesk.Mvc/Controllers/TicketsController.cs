using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HelpDesk.Api.Models;
using HelpDesk.Mvc.Services;

namespace HelpDesk.Mvc.Controllers
{
    public class TicketsController : Controller
    {
        private readonly TicketService _ticketService;

        public TicketsController(TicketService ticketService)
        {
            _ticketService = ticketService;
        }

        // GET: Tickets
        // Supports optional status filtering
        public async Task<IActionResult> Index(string statusFilter)
        {
            ViewBag.CurrentFilter = statusFilter;
            
            System.Collections.Generic.List<Ticket> tickets;
            if (string.IsNullOrEmpty(statusFilter) || statusFilter == "All")
            {
                tickets = await _ticketService.GetAllTicketsAsync();
            }
            else
            {
                tickets = await _ticketService.GetTicketsByStatusAsync(statusFilter);
            }

            return View(tickets);
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var ticket = await _ticketService.GetTicketByIdAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            return View(ticket);
        }

        // GET: Tickets/Create
        public IActionResult Create()
        {
            return View(new Ticket());
        }

        // POST: Tickets/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,Priority,RaisedBy")] Ticket ticket)
        {
            // Status must be hardcoded to Open
            ticket.Status = "Open";
            ticket.CreatedDate = DateTime.Now;

            ModelState.Remove("Status");
            ModelState.Remove("CreatedDate");

            if (ModelState.IsValid)
            {
                var result = await _ticketService.CreateTicketAsync(ticket);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Error creating ticket on API side.");
            }
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var ticket = await _ticketService.GetTicketByIdAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Priority,Status,RaisedBy,CreatedDate")] Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var result = await _ticketService.UpdateTicketAsync(ticket);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Error updating ticket on API side.");
            }
            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _ticketService.DeleteTicketAsync(id);
            if (!result)
            {
                TempData["ErrorMessage"] = "Failed to delete ticket.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
