using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TasksApp.Models
{
    public class TaskItem
    {
        public TaskItem() { }
        public int Id { get; set; }

        [Required(ErrorMessage = "כותרת היא שדה חובה")]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required(ErrorMessage = "תאריך יעד הוא שדה חובה")]
        public DateTime DueDate { get; set; }
    }
}
