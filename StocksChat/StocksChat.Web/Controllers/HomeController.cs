using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StocksChat.Business.Interfaces.Services;
using StocksChat.Business.Models;
using StocksChat.Web.Models;

namespace StocksChat.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMessagesService _messagesService;

        public HomeController(ILogger<HomeController> logger, IMessagesService messagesService)
        {
            _logger = logger; 
            _messagesService = messagesService;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Message> messages = await _messagesService.GetAllMessages();
            return View(messages);
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
