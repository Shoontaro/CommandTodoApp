using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace CommandTodoApp
{
    public enum Status
    { 
        todo,
        inProgress,
        done
    }

    public class ToDo
    {
        public int Id { get; set; }
        public string Name { get; set; }// = "Task";
        public string Description { get; set; }// = "Description";
        public Status status { get; set; } = Status.todo;
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
            if (isCompleted)
            {
                this.status = Status.done;
                DoneAt = DateTime.Now;
            }
        }

        //public ToDo(string name, string description, DateTime createAt, DateTime doneAt, bool isCompleted) : this(name, description, isCompleted)
        //{
        //    CreateAt = createAt;
        //    DoneAt = doneAt;
        //}

        public void Done() {
            if (this.status == Status.done) return;

            //this.IsCompleted = true;
            this.status = Status.done;
            this.DoneAt = DateTime.Now;
        }
        public void InProgress() {
            if (this.status == Status.inProgress) return;

            this.status = Status.inProgress;
            this.DoneAt = DateTime.Now;
        }

        public void AddTask(ToDo task)
        {
            Console.WriteLine($"\n Создаем таск. \n" +
                   $" Название {task.Name}\n" +
                   $" Описание {task.Description} \n" +
                   $" Готовность {task.status}");

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

           // Console.WriteLine(Program.FilePath);
            var json = JsonSerializer.Serialize(todos, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(Program.FilePath, json);  //вписывание объектов в файл
        }

        public static void TaskInProgress(int id) {
            List<ToDo> tasks = LoadTasks();

            ToDo task = tasks.Find(v => v.Id == id);

            if (task == null) { Console.WriteLine("Нет задачи с таким id"); return; }

            task.InProgress();

            SaveTasks(tasks);
        }

        public static void DoneTask(int id) 
        {
            List<ToDo> tasks = LoadTasks();

            ToDo task = tasks.Find(v => v.Id == id);

            if (task == null) { Console.WriteLine("Нет задачи с таким id"); return; }

            task.Done();

            SaveTasks(tasks);
        }

        public static void DeleteTask(int id) 
        {
            List<ToDo> tasks = LoadTasks();

            ToDo task = tasks.Find(v => v.Id == id);

            if (task == null) { Console.WriteLine("Нет задачи с таким id"); return; }

            tasks.Remove(task);

            SaveTasks(tasks);

            Console.WriteLine("Задача удалена");
        }
    }
}
