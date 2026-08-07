using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using HelpDesk.Api.Controllers;
using HelpDesk.Api.Models;
using HelpDesk.Api.Repositories;

namespace HelpDesk.Tests
{
    public class TicketControllerTests
    {
        private readonly Mock<ITicketRepository> _mockRepo;
        private readonly TicketController _controller;

        public TicketControllerTests()
        {
            _mockRepo = new Mock<ITicketRepository>();
            _controller = new TicketController(_mockRepo.Object);
        }

        // ==========================================
        // MANDATORY TEST CASES
        // ==========================================

        [Fact]
        public async Task GetAllTickets_ReturnsOkResult_WhenTicketsExist()
        {
            // Arrange
            var tickets = new List<Ticket>
            {
                new Ticket { Id = 1, Title = "Test Ticket 1", Description = "Desc 1", Priority = "Low", Status = "Open", RaisedBy = "User 1", CreatedDate = DateTime.Now },
                new Ticket { Id = 2, Title = "Test Ticket 2", Description = "Desc 2", Priority = "High", Status = "Closed", RaisedBy = "User 2", CreatedDate = DateTime.Now }
            };
            _mockRepo.Setup(repo => repo.GetAllTicketsAsync()).ReturnsAsync(tickets);

            // Act
            var result = await _controller.GetAllTickets();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedTickets = Assert.IsAssignableFrom<List<Ticket>>(okResult.Value);
            Assert.Equal(2, returnedTickets.Count);
        }

        [Fact]
        public async Task GetTicketById_ReturnsOkResult_WhenTicketExists()
        {
            // Arrange
            int id = 1;
            var ticket = new Ticket { Id = id, Title = "Test Ticket 1", Description = "Desc 1", Priority = "Low", Status = "Open", RaisedBy = "User 1", CreatedDate = DateTime.Now };
            _mockRepo.Setup(repo => repo.GetTicketByIdAsync(id)).ReturnsAsync(ticket);

            // Act
            var result = await _controller.GetTicketById(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedTicket = Assert.IsType<Ticket>(okResult.Value);
            Assert.Equal(id, returnedTicket.Id);
        }

        [Fact]
        public async Task GetTicketById_ReturnsNotFound_WhenTicketDoesNotExist()
        {
            // Arrange
            int id = 99;
            _mockRepo.Setup(repo => repo.GetTicketByIdAsync(id)).ReturnsAsync((Ticket)null);

            // Act
            var result = await _controller.GetTicketById(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task CreateTicket_ReturnsOkResult_WhenTicketIsCreatedSuccessfully()
        {
            // Arrange
            var ticketInput = new Ticket { Title = "New Ticket", Description = "Desc", Priority = "Medium", Status = "Open", RaisedBy = "User" };
            _mockRepo.Setup(repo => repo.CreateTicketAsync(It.IsAny<Ticket>())).ReturnsAsync(5); // Returns new ID = 5

            // Act
            var result = await _controller.CreateTicket(ticketInput);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedTicket = Assert.IsType<Ticket>(okResult.Value);
            Assert.Equal(5, returnedTicket.Id);
            Assert.Equal("New Ticket", returnedTicket.Title);
        }

        [Fact]
        public async Task CreateTicket_ReturnsBadRequest_WhenTicketIsNull()
        {
            // Arrange
            Ticket ticketInput = null;

            // Act
            var result = await _controller.CreateTicket(ticketInput);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GetTicketsByStatus_ReturnsOkResult_WhenMatchingTicketExist()
        {
            // Arrange
            string status = "Open";
            var tickets = new List<Ticket>
            {
                new Ticket { Id = 1, Title = "Open Ticket", Description = "Desc", Priority = "Low", Status = "Open", RaisedBy = "User" }
            };
            _mockRepo.Setup(repo => repo.GetTicketsByStatusAsync(status)).ReturnsAsync(tickets);

            // Act
            var result = await _controller.GetTicketsByStatus(status);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedTickets = Assert.IsAssignableFrom<List<Ticket>>(okResult.Value);
            Assert.Single(returnedTickets);
            Assert.Equal(status, returnedTickets[0].Status);
        }

        // ==========================================
        // OPTIONAL TEST CASES
        // ==========================================

        [Fact]
        public async Task UpdateTicket_ReturnsOkResult_WhenUpdateIsSuccessful()
        {
            // Arrange
            int id = 1;
            var ticketInput = new Ticket { Id = id, Title = "Updated Ticket", Description = "Desc", Priority = "Low", Status = "Open", RaisedBy = "User" };
            _mockRepo.Setup(repo => repo.GetTicketByIdAsync(id)).ReturnsAsync(ticketInput);
            _mockRepo.Setup(repo => repo.UpdateTicketAsync(It.IsAny<Ticket>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateTicket(id, ticketInput);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task UpdateTicket_ReturnsNotFound_WhenTicketDoesNotExist()
        {
            // Arrange
            int id = 99;
            var ticketInput = new Ticket { Id = id, Title = "Updated Ticket", Description = "Desc", Priority = "Low", Status = "Open", RaisedBy = "User" };
            _mockRepo.Setup(repo => repo.GetTicketByIdAsync(id)).ReturnsAsync((Ticket)null);

            // Act
            var result = await _controller.UpdateTicket(id, ticketInput);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteTicket_ReturnsOkResult_WhenTicketIsDeletedSuccessfully()
        {
            // Arrange
            int id = 1;
            var ticket = new Ticket { Id = id, Title = "Ticket to delete", Description = "Desc", Priority = "Low", Status = "Open", RaisedBy = "User" };
            _mockRepo.Setup(repo => repo.GetTicketByIdAsync(id)).ReturnsAsync(ticket);
            _mockRepo.Setup(repo => repo.DeleteTicketAsync(id)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteTicket(id);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task DeleteTicket_ReturnsNotFound_WhenTicketDoesNotExist()
        {
            // Arrange
            int id = 99;
            _mockRepo.Setup(repo => repo.GetTicketByIdAsync(id)).ReturnsAsync((Ticket)null);

            // Act
            var result = await _controller.DeleteTicket(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetAllTickets_ReturnsEmptyList_WhenNoTicketExist()
        {
            // Arrange
            var emptyList = new List<Ticket>();
            _mockRepo.Setup(repo => repo.GetAllTicketsAsync()).ReturnsAsync(emptyList);

            // Act
            var result = await _controller.GetAllTickets();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedTickets = Assert.IsAssignableFrom<List<Ticket>>(okResult.Value);
            Assert.Empty(returnedTickets);
        }

        [Fact]
        public async Task GetTicketsByStatus_ReturnsEmptyList_WhenNoMatchingTicketExist()
        {
            // Arrange
            string status = "Closed";
            var emptyList = new List<Ticket>();
            _mockRepo.Setup(repo => repo.GetTicketsByStatusAsync(status)).ReturnsAsync(emptyList);

            // Act
            var result = await _controller.GetTicketsByStatus(status);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedTickets = Assert.IsAssignableFrom<List<Ticket>>(okResult.Value);
            Assert.Empty(returnedTickets);
        }
    }
}
