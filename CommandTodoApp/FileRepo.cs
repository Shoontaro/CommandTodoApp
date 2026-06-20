using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace CommandTodoApp
{
    interface IFileRep {
        public static abstract List<ToDo> LoadTasks();
    }

    public class FileRepo
    {
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
