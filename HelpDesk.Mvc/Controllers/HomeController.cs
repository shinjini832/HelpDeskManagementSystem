using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HelpDesk.Mvc.Models;
using HelpDesk.Mvc.Services;

namespace HelpDesk.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly TicketService _ticketService;

        public HomeController(TicketService ticketService)
        {
            _ticketService = ticketService;
        }

        public async Task<IActionResult> Index()
        {
            var tickets = await _ticketService.GetAllTicketsAsync();
            
            ViewBag.TotalTickets = tickets.Count;
            ViewBag.OpenTickets = tickets.Count(t => "Open".Equals(t.Status, StringComparison.OrdinalIgnoreCase));
            ViewBag.ClosedTickets = tickets.Count(t => "Closed".Equals(t.Status, StringComparison.OrdinalIgnoreCase));
            
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
