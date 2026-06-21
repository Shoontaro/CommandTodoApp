using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace CommandTodoApp
{
    public interface IDataProvider
    {
       public List<ToDo> LoadTasks();

       public void SaveTasks(List<ToDo> todos);
    }

    public class FileRepo : IDataProvider
    {
        private readonly string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "todos.json");
        public List<ToDo> LoadTasks()
        {
            if (!File.Exists(FilePath)) return new List<ToDo>();//возвращаем пустой лист, если файла не существует

            var json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<List<ToDo>>(json) ?? new List<ToDo>(); //парсинг джейсона
        }

        public void SaveTasks(List<ToDo> todos)
        {

            // Console.WriteLine(Program.FilePath);
            var json = JsonSerializer.Serialize(todos, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);  //вписывание объектов в файл
        }
    }
}
