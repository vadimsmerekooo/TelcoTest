using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using TelcoTestAsp.Models;

namespace TelcoTestAsp.Controllers
{
    public class ElementController : Controller
    {
        TelcoContext _context;
        public ElementController(TelcoContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Create(int idTask) => View(new CreateElementViewModel()
        {
            Task = _context.Tasks.FirstOrDefault(t => t.Id == idTask)
        });

        [HttpPost]
        public IActionResult Create(CreateElementViewModel viewModel)
        {
            if(_context.Tasks.Any(t => t.Id == viewModel.TaskElement.TaskId))
            {
                if (viewModel.TaskElement != null)
                {
                    Task task = _context.Tasks.FirstOrDefault(t => t.Id == viewModel.TaskElement.TaskId);
                    viewModel.TaskElement.Task = task;
                    _context.TaskElements.Add(viewModel.TaskElement);
                    _context.SaveChanges();
                }
                return Redirect("/Task/TaskInfo?idTask=" + viewModel.TaskElement.TaskId);
            }
            return Redirect("/Home/Index");
        }

        public IActionResult Delete(int idElement)
        {
            if(_context.TaskElements.Any(e => e.Id == idElement))
            {
                TaskElement element = _context.TaskElements.FirstOrDefault(e => e.Id == idElement);
                _context.TaskElements.Remove(element);
                _context.SaveChanges();
                return Redirect("/Task/TaskInfo?idTask=" + element.TaskId);
            }
            return Redirect("/Home/Index");
        }
        [HttpGet]
        public IActionResult Update(int idElement)
        {
            if (_context.TaskElements.Any(t => t.Id == idElement))
            {
                return View(_context.TaskElements.FirstOrDefault(t => t.Id == idElement));
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult Update(TaskElement taskElement)
        {
            if (taskElement != null)
            {
                Task task = _context.Tasks.FirstOrDefault(t => t.Id == taskElement.TaskId);
                taskElement.Task = task;


                _context.TaskElements.Update(taskElement);
                _context.SaveChanges();

                return Redirect("/Task/TaskInfo?idTask=" + taskElement.TaskId);

            }
            return Redirect("/Home/Index");
        }
    }
}
