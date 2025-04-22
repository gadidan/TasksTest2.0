using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TasksApp.Models;

namespace TasksApp.Data
{

    public class TaskContext : DbContext
    {

        public DbSet<TaskItem> Tasks { get; set; }

        // נתיב לקובץ ה-JSON לאחסון הנתונים
        private static string? _dataFilePath;
        public static string DataFilePath
        {
            get => _dataFilePath ?? @"C:\Users\USER\source\repos\TasksApp\Data\Tasks.json";
            set => _dataFilePath = value;
        }

        public TaskContext(DbContextOptions<TaskContext> options, IConfiguration configuration) : base(options)
        {
            var relativePath = configuration["TaskData:JsonFilePath"];
            var basePath = Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName;
            DataFilePath = Path.Combine(basePath, relativePath);
        }

        // טעינת נתוני משימות מקובץ JSON (נקרא בעת אתחול השרת)
        public void LoadDataFromJson()
        {
            if (!File.Exists(DataFilePath))
            {
                // אם הקובץ לא קיים, ניצור קובץ ריק
                File.WriteAllText(DataFilePath, "[]");
            }
            else
            {
                try
                {
                    string json = File.ReadAllText(DataFilePath);
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true 
                    };
                    var tasksFromFile = JsonSerializer.Deserialize<List<TaskItem>>(json,options);

                    // הוספת המשימות מהקובץ למסד הנתונים בזיכרון
                    this.Tasks.AddRange(tasksFromFile);
                    this.SaveChanges(); // שמירה לזיכרון (תגרום לכתיבה חוזרת, זה בסדר כי זה אותו מידע)
                }

                catch
                {
                    // במקרה של שגיאת קריאה/פיענוח, לא נטען דבר (ניתן לשפר: לוג וכו')
                }
            }
        }

        /*public override int SaveChanges()
        {
            int result = base.SaveChanges(); // שמירה בהקשר (ל-InMemory DB)
            try
            {
                // עדכון קובץ ה-JSON לאחר כל שמירה
                var allTasks = Tasks.ToList();
                string json = JsonSerializer.Serialize(allTasks, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(DataFilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Log: אירעה שגיאה בעת כתיבה לקובץ הנתונים: {ex.Message}");
            }
            return result;
        }*/

        public override int SaveChanges()
        {
            int result = base.SaveChanges(); // In-memory update

            try
            {
                var allTasks = Tasks.ToList();

                var json = JsonSerializer.Serialize(allTasks, new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                });

                File.WriteAllText(DataFilePath, json, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Failed to save JSON: {ex.Message}");
                Console.WriteLine(ex.ToString());
            }

            return result;
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // ניתן להגדיר אילוצי מודל נוספים אם צריך (כגון מגבלות, שמות טבלאות וכו').
        }

    }
}
