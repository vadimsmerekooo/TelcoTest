using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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

        public IActionResult Index(int page = 1)
        {
            int pageSize = 3; 

            List<Task> tasks = _context.Tasks.Include(c => c.TaskElements).ToList();
            
            var count = tasks.Count();
            var items = tasks.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            if (pageViewModel.TotalPages < page)
                return RedirectToAction("Index", new { page = pageViewModel.TotalPages });
            IndexViewModel viewModel = new IndexViewModel
            {
                PageViewModel = pageViewModel,
                Tasks = items
            };
            return View(viewModel);            
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
