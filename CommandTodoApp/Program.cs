using System.CommandLine;
using System.CommandLine.Parsing;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace CommandTodoApp;
class Program
{
    private static readonly string FilePath = "todos.txt";

    private static void Main(string[] args)
    {
        Console.WriteLine("Приложение для задачь. Добавьте задачу");

        GetTask();
    }

    //public static void CommandTasks()
    //{
    //    while (true) 
    //    {
    //        string command = Console.ReadLine() ?? "";

    //        var allOption = new Option<bool>("--all", "Показать все задачи (включая выполненные)");

    //        var addCommand = new Command("add", "Добавление новой задачи") {
          
    //        };

    //    }
    //}



    private static void GetTask()
    {
        while (true)
        {
            string command = Console.ReadLine() ?? "";

            Option<string> lastOption = new("--task")
            {
                Description = "Отобразить задачу с конкретным id"
            };

            Option<string> allOption = new("--all")
            {
                Description = "Вывести лист последних заданий"
            };


            Option<string> titleOption = new("--title")
            {
                Description = "Задать название задачи"
            };

            Option<string> descOptiion = new("--desc") { 
                Description = "Задать описание задачи"
            };

            Option<bool> doneOption = new("--done")
            {
                Description = "Задача выполнена"
            };

            RootCommand rootCommand = new("Todo проект с использованием System.CommandLine");
            // rootCommand.Options.Add(lastOption);

            Command addCommand = new("add", "Создать задачу") //подкоманда
            {
                titleOption,
                descOptiion,
                doneOption
            };

            rootCommand.Subcommands.Add(addCommand);

            addCommand.SetAction(parseResult =>
            {
                if (parseResult.GetValue(titleOption) != null)
                {
                    ToDo todo = new ToDo(parseResult.GetValue(titleOption)??"", parseResult.GetValue(descOptiion)??"", parseResult.GetValue(doneOption));
                    AddTask(todo);
                }
                else {
                    Console.WriteLine("Название не вписано");
                }
            });

            //ParseResult parseResult = rootCommand.Parse(command);

            //rootCommand.SetAction(parseResult => 
            //{
            //    string parsedFile = parseResult.GetValue(lastOption)??"";
            //    Console.WriteLine($"Parsed {parsedFile}");
            //});

            rootCommand.Parse(command).Invoke(); //запуск делегата
        }
    }

    private static void AddTask(ToDo task) {
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

    private static List<ToDo> LoadTasks()
    {
        if (!File.Exists(FilePath)) return new List<ToDo>();//возвращаем пустой лист, если файла не существует

        var json = File.ReadAllText(FilePath);
        return JsonSerializer.Deserialize<List<ToDo>>(json) ?? new List<ToDo>(); //парсинг джейсона
    }

    private static void SaveTasks(List<ToDo> todos)
    {
        var json = JsonSerializer.Serialize(todos, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(FilePath, json);  //вписывание объектов в файл
    }

}