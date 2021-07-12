using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using TelcoTestAsp.Models;

namespace TelcoTestAsp.Controllers
{
    public class HomeController : Controller
    {
        TelcoContext _context;
        public HomeController(TelcoContext context)
        {
            _context = context;
        }

        public IActionResult Index() => View(_context.Tasks.Include(c => c.TaskElements).ToList());


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
