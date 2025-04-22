using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TasksApp.Data;
using TasksApp.Models;
using TasksApp.Services;

namespace TasksApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly TaskContext _context;
        private readonly LogService _logService;

        public TasksController(TaskContext context, LogService logService)
        {
            _context = context;
            _logService = logService;
        }
        private bool WriteToLog(string message, string developerName)
        {
            var _writeToLog = false;
            
            if (!string.IsNullOrEmpty(developerName))
            {
                _logService.Debug(message, developerName);
                // continue only if has a developer name
                _writeToLog = true;
            }
            return _writeToLog;
        }
        // GET: api/tasks
        [HttpGet]
        public ActionResult<IEnumerable<TaskItem>> GetTasks()
        {
            var developerName = Request.Headers["Name-Developer"].ToString();
            WriteToLog("get tasks", developerName);

            // get all tasks
            var tasks = _context.Tasks.ToList();
            return Ok(tasks);
            
                
        }

        // GET: api/tasks/5
        [HttpGet("{id}")]
        public ActionResult<TaskItem> GetTask(int id)
        {
            var developerName = Request.Headers["Name-Developer"].ToString();
            WriteToLog("get tasks", developerName);

            var task = _context.Tasks.Find(id);
            if (task == null)
                return NotFound();
            return Ok(task);
        }

        // POST: api/tasks
        [HttpPost]
        public ActionResult<TaskItem> CreateTask([FromBody] TaskItem newTask)
        {

            var developerName = Request.Headers["Name-Developer"].ToString();
            WriteToLog("create task", developerName); 
            
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (newTask.DueDate.Date < System.DateTime.Today)
            {
                return BadRequest("תאריך היעד לא יכול להיות בעבר.");
            }
            // מזהה חדש Auto-Increment
            newTask.Id = _context.Tasks.Any() ? _context.Tasks.Max(t => t.Id) + 1 : 1;
            _context.Tasks.Add(newTask);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetTask), new { id = newTask.Id }, newTask);
        }

        // PUT: api/tasks/5
        [HttpPut("{id}")]
        public IActionResult UpdateTask(int id, [FromBody] TaskItem updatedTask)
        {
            var developerName = Request.Headers["Name-Developer"].ToString();
            WriteToLog("update task", developerName);

            if (id != updatedTask.Id)
                return BadRequest("לא מתאימות מזהי המשימה.");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (updatedTask.DueDate.Date < System.DateTime.Today)
            {
                return BadRequest("תאריך היעד לא יכול להיות בעבר.");
            }

            var existingTask = _context.Tasks.Find(id);
            if (existingTask == null)
            {
                return NotFound();
            }
            // updates
            existingTask.Title = updatedTask.Title;
            existingTask.Description = updatedTask.Description;
            existingTask.DueDate = updatedTask.DueDate;
            _context.SaveChanges();
            return NoContent(); // 204 for no content
        }

        // DELETE: api/tasks/5
        [HttpDelete("{id}")]
        public IActionResult DeleteTask(int id)
        {
            var developerName = Request.Headers["Name-Developer"].ToString();
            WriteToLog("delete task", developerName);

            var task = _context.Tasks.Find(id);
            if (task == null)
            {
                return NotFound();
            }
            _context.Tasks.Remove(task);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
