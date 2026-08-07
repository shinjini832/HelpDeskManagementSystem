using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using HelpDesk.Api.Models;

namespace HelpDesk.Mvc.Services
{
    public class TicketService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public TicketService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _baseUrl = configuration.GetValue<string>("ApiSettings:BaseUrl");
        }

        public async Task<List<Ticket>> GetAllTicketsAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<Ticket>>($"{_baseUrl}/All");
                return response ?? new List<Ticket>();
            }
            catch
            {
                return new List<Ticket>();
            }
        }

        public async Task<Ticket> GetTicketByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<Ticket>();
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> CreateTicketAsync(Ticket ticket)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(_baseUrl, ticket);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateTicketAsync(Ticket ticket)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}/{ticket.Id}", ticket);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteTicketAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_baseUrl}/{id}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<Ticket>> GetTicketsByStatusAsync(string status)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<Ticket>>($"{_baseUrl}/Status/{status}");
                return response ?? new List<Ticket>();
            }
            catch
            {
                return new List<Ticket>();
            }
        }
    }
}
