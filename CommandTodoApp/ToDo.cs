using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace CommandTodoApp
{
    public class ToDo
    {
        public int Id { get; set; }
        public string Name { get; set; }// = "Task";
        public string Description { get; set; }// = "Description";
        public DateTime CreateAt { get; set; }
        public DateTime DoneAt { get; set; }
        public bool IsCompleted { get; set; }

        public ToDo() { }
        public ToDo(string name, string desc)
        {
            this.Name = name;
            this.Description = desc;
            CreateAt = DateTime.Now;
        }

        public ToDo(string name, string desc, bool isCompleted) : this(name, desc)
        {
            IsCompleted = isCompleted;
            DoneAt = DateTime.Now;
        }

        public ToDo(string name, string description, DateTime createAt, DateTime doneAt, bool isCompleted) : this(name, description, isCompleted)
        {
            CreateAt = createAt;
            DoneAt = doneAt;
        }

        public void Done() {
            if (this.IsCompleted) return;

            this.IsCompleted = true;
            this.DoneAt = DateTime.Now;
        }

        public void AddTask(ToDo task)
        {
            Console.WriteLine($"\n Создаем таск. \n" +
                   $" Название {task.Name}\n" +
                   $" Описание {task.Description} \n" +
                   $" Готовность {task.IsCompleted}");

            List<ToDo> tasks = LoadTasks();
            task.Id = tasks.Count > 0 ? tasks.Last().Id + 1 : 1;

            tasks.Add(task);
            SaveTasks(tasks);

            Console.WriteLine($"Задача успешно добавлена! Ее id {task.Id}");
        }

        public static List<ToDo> LoadTasks()
        {
            if (!File.Exists(Program.FilePath)) return new List<ToDo>();//возвращаем пустой лист, если файла не существует

            var json = File.ReadAllText(Program.FilePath);
            return JsonSerializer.Deserialize<List<ToDo>>(json) ?? new List<ToDo>(); //парсинг джейсона
        }

        private static void SaveTasks(List<ToDo> todos)
        {

            Console.WriteLine(Program.FilePath);
            var json = JsonSerializer.Serialize(todos, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(Program.FilePath, json);  //вписывание объектов в файл
        }
    }
}
