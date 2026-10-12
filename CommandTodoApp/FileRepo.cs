using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace CommandTodoApp
{
    public class FileRepo : IDataProvider
    {
        private readonly string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "todos.json");

        private static readonly JsonSerializerOptions Options = new()
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        };

        public async Task<List<ToDo>> LoadTasksAsync()
        {
            if (!File.Exists(FilePath)) return new List<ToDo>();

            using FileStream openStream = File.OpenRead(FilePath);

            var result = await JsonSerializer.DeserializeAsync<List<ToDo>>(openStream, Options);

            return result ?? new List<ToDo>();

            //var json = File.ReadAllText(FilePath);
            //return JsonSerializer.Deserialize<List<ToDo>>(json) ?? new List<ToDo>(); //парсинг джейсона
        }

        public async Task SaveTasksAsync(List<ToDo> todos)
        {
            using FileStream createStream = File.Create(FilePath);

            await JsonSerializer.SerializeAsync(createStream, todos, Options);
            //var json = JsonSerializer.Serialize(todos, new JsonSerializerOptions { WriteIndented = true });
            //await File.WriteAllTextAsync(FilePath, json);  //вписывание объектов в файл
        }
    }
}
