using System;
using System.Collections.Generic;
using System.CommandLine;
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
      //  public string Description { get; set; }// = "Description";
        public Status status { get; set; } = Status.todo;
        public DateTime CreateAt { get; set; }
        public DateTime DoneAt { get; set; }
        public bool IsCompleted { get; set; }

        public ToDo() { }
        public ToDo(string name)
        {
            this.Name = name;
           // this.Description = desc;
            CreateAt = DateTime.Now;
        }

        public ToDo(string name, bool isCompleted) : this(name)
        {
            IsCompleted = isCompleted;
            if (isCompleted)
            {
                this.status = Status.done;
                DoneAt = DateTime.Now;
            }
        }

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

        public static List<ToDo> LoadTasks()
        {
            if (!File.Exists(Program.FilePath)) return new List<ToDo>();//возвращаем пустой лист, если файла не существует

            var json = File.ReadAllText(Program.FilePath);
            return JsonSerializer.Deserialize<List<ToDo>>(json) ?? new List<ToDo>(); //парсинг джейсона
        }

        public static void SaveTasks(List<ToDo> todos)
        {

           // Console.WriteLine(Program.FilePath);
            var json = JsonSerializer.Serialize(todos, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(Program.FilePath, json);  //вписывание объектов в файл
        }
    }
}
