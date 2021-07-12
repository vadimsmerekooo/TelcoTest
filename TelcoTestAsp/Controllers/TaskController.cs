﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TelcoTestAsp.Models;

namespace TelcoTestAsp.Controllers
{
    public class TaskController : Controller
    {
        private TelcoContext _context;

        public TaskController(TelcoContext context)
        {
            _context = context;
        }
        public IActionResult TaskInfo(int idTask, int page = 1)
        {
            if (_context.Tasks.Any(t => t.Id == idTask))
            {
                int pageSize = 2;

                Task task = _context.Tasks.Include(t => t.TaskElements).FirstOrDefault(t => t.Id == idTask);
                List<TaskElement> taskElements = task.TaskElements;

                var count = taskElements.Count();
                var items = taskElements.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);

                if (pageViewModel.TotalPages < page)
                    return RedirectToAction("TaskInfo", new { idTask = idTask, page = pageViewModel.TotalPages });

                TaskViewModel viewModel = new TaskViewModel
                {
                    PageViewModel = pageViewModel,
                    Task = task,
                    TaskElements = items
                };
                return View(viewModel);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(Task task)
        {
            if (task != null)
            {
                try
                {
                    task.Start_date = task.Start_date < DateTime.Now ? DateTime.Now : task.Start_date;
                    task.End_date = task.End_date <= task.Start_date ? DateTime.Now.AddDays(1) : task.End_date;
                    task.Status = task.Start_date > DateTime.Now ? TaskStatus.Closed : TaskStatus.Opened;


                    _context.Tasks.Add(task);
                    _context.SaveChanges();
                }
                catch
                {
                    return RedirectToAction("Create");
                }
                return Redirect("/Home/Index");
            }

            return RedirectToAction("Create");
        }
        public IActionResult Delete(int idTask)
        {
            if (_context.Tasks.Any(t => t.Id == idTask))
            {
                Task task = _context.Tasks.Include(e => e.TaskElements).FirstOrDefault(t => t.Id == idTask);
                _context.Tasks.Remove(task);
                _context.SaveChanges();
            }
            return Redirect("/Home/Index");
        }

        [HttpGet]
        public IActionResult Update(int idTask)
        {
            if(_context.Tasks.Any(t => t.Id == idTask))
            {
                return View(_context.Tasks.Include(e => e.TaskElements).FirstOrDefault(t => t.Id == idTask));
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult Update(Task task)
        {
            if(task != null)
            {
                task.Start_date = task.Start_date < DateTime.Now ? DateTime.Now : task.Start_date;
                task.End_date = task.End_date <= task.Start_date ? DateTime.Now.AddDays(1) : task.End_date;
                task.Status = task.Start_date > DateTime.Now ? TaskStatus.Closed : TaskStatus.Opened;

                _context.Tasks.Update(task);
                _context.SaveChanges();

                return RedirectToAction("TaskInfo", new { idTask = task.Id });

            }
            return Redirect("/Home/Index");
        }
    }
}
