using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TelcoTestAsp.Models;
using TelcoTestAsp.Generate_file;
using System.IO;

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
            List<Task> tasks = _context.Tasks.Include(c => c.TaskElements).ToList();

            foreach (Task taskItem in tasks)
            {
                if (taskItem.End_date < DateTime.Now.AddDays(1))
                {
                    taskItem.Status = TaskStatus.Closed;
                    _context.Tasks.FirstOrDefault(task => task.Id == taskItem.Id).Status = TaskStatus.Closed;
                }
            }
            _context.SaveChanges();

            return View(GetIndexViewModel(page, tasks.OrderBy(d => d.End_date).OrderBy(s => s.Status).ToList()));
        }

        [HttpPost]
        public IActionResult Search(SearchModel searchModel, Task searchTaskModel, bool isAdvancedSearch)
        {
            if (!isAdvancedSearch)
            {
                if (String.IsNullOrWhiteSpace(searchModel.Query))
                {
                    return RedirectToAction("Index");
                }

                List<Task> tasks = _context.Tasks
                    .Where(task => task.Name.Contains(searchModel.Query))
                    .Include(c => c.TaskElements)
                    .OrderBy(s => s.Status)
                    .OrderBy(d => d.Start_date)
                    .ToList();

                SearchViewModel viewModel = new SearchViewModel
                {
                    ViewModel = tasks,
                    SearchModel = searchModel
                };
                return View(viewModel);
            }
            else
            {
                List<Task> tasks = new List<Task>();
                if (!String.IsNullOrWhiteSpace(searchTaskModel.Name))
                {
                    tasks = _context.Tasks.Where(task =>
                        task.Name.Contains(searchTaskModel.Name)
                           && task.Status == searchTaskModel.Status
                           && task.Start_date >= searchTaskModel.Start_date
                           && task.End_date <= searchTaskModel.End_date)
                        .Include(e => e.TaskElements).ToList();
                }
                else
                {
                    tasks = _context.Tasks.Where(task =>
                        task.Status == searchTaskModel.Status
                        && task.Start_date >= searchTaskModel.Start_date
                        && task.End_date <= searchTaskModel.End_date)
                        .Include(e => e.TaskElements).ToList();
                }
                SearchViewModel viewModel = new SearchViewModel
                {
                    ViewModel = tasks,
                    SearchModel = new SearchModel()
                    {
                        FilterSearch = searchTaskModel
                    }
                };
                return View(viewModel);
            }
        }


        private IndexViewModel GetIndexViewModel(int page, List<Task> tasks)
        {
            int pageSize = 3;

            var count = tasks.Count();
            var items = tasks.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);

            if (pageViewModel.TotalPages < page)
                RedirectToAction("Index", new { page = pageViewModel.TotalPages });

            IndexViewModel viewModel = new IndexViewModel
            {
                PageViewModel = pageViewModel,
                Tasks = items
            };

            return viewModel;
        }

        public FileResult GenerateExcelFile()
        {
            string pathFileStructure = Path.GetFullPath("Generate file/templatefile/TemplateStructure.xml");
            string pathFileTaskRow = Path.GetFullPath("Generate file/templatefile/TemplateRowTask.txt");
            string pathFileTaskElementHeader = Path.GetFullPath("Generate file/templatefile/TemplateRowHeaderTaskElement.txt");
            string pathFileTaskElementRow = Path.GetFullPath("Generate file/templatefile/TemplateRowTaskElement.txt");
            TemplateFilesModel template = new TemplateFilesModel(pathFileStructure, pathFileTaskRow, pathFileTaskElementHeader, pathFileTaskElementRow);

            List<Task> tasks = _context.Tasks.Include(c => c.TaskElements).ToList();

            GenerateXlsx generate = new GenerateXlsx(template, tasks);

            return File(generate.GenerateXlsxFile<byte[]>(), "application/force-download", "Telco_tasks_info.xls");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
