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

    // public static readonly string FilePath = "todos.txt";
    public static readonly string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "todos.json");


    private static void Main(string[] args)
    {
        Console.WriteLine("Приложение для задачь. Добавьте задачу");

        GetTask();
    }

    private static void GetTask()
    {
        while (true)
        {
            string command = Console.ReadLine() ?? "";

            //       var nameOption = new Option<string>(
            //name: "--name",
            //aliases: new[] { "-n" }
            //);

            Option<int> taskOption = new(name: "--task", aliases: "-id")
            {
                Description = "Отобразить задачу с конкретным id"
            };

            Option<bool> allOption = new("--all")
            {
                Description = "Вывести все задачи"
            };

            Option<string> titleOption = new(name: "--title", aliases: "-ttl")
            {
                Description = "Задать название задачи"
            };

            Option<string> descOptiion = new(name: "--desc", aliases: "-d")
            {
                Description = "Задать описание задачи"
            };

            Option<bool> doneOption = new("--done")
            {
                Description = "Задача выполнена"
            };

            Command addCommand = new("add", "Создать задачу") //подкоманда
            {
                titleOption,
                descOptiion,
                doneOption
            };

            Command listCommand = new Command("list", "Отобразить задачи")
            {
                allOption, //если нужно отобразить в том числе и выполненные задачи
            };

            Command doneCommand = new Command("done", "Завершить задачу")
            {
                taskOption
            };

            RootCommand rootCommand = new("Todo проект с использованием System.CommandLine")
            {
            addCommand,
            listCommand,
            doneCommand
            };
            // rootCommand.Options.Add(lastOption);

            //rootCommand.Subcommands.Add(addCommand);
            //rootCommand.Subcommands.Add(listCommand);

            addCommand.SetAction(parseResult =>
            {
                if (parseResult.GetValue(titleOption) != null)
                {
                    ToDo todo = new ToDo(parseResult.GetValue(titleOption) ?? "", parseResult.GetValue(descOptiion) ?? "", parseResult.GetValue(doneOption));
                    todo.AddTask(todo);
                }
                else
                {
                    Console.WriteLine("Название не вписано");
                }
            });

            listCommand.SetAction(parseResult =>
            {

                bool all = parseResult.GetValue(allOption);

                List<ToDo> tasks = all ? ToDo.LoadTasks() : ToDo.LoadTasks().Where(v => v.IsCompleted == false).ToList();

                Console.WriteLine($"{(tasks.Count > 0 ? $"\n Отображаем лист {(!all ? "не завершенных" : "всех")} заданий: " : "Данных нет")}");

                foreach (ToDo todo in tasks)
                {
                    Console.WriteLine($"[{todo.CreateAt.ToShortDateString()}] {todo.Name} | {todo.Description} | {(todo.IsCompleted ? $"✓ [{todo.DoneAt.ToShortDateString()}]" : "")} \n");
                }
            });

            doneCommand.SetAction(parseResult => 
            {
                Console.WriteLine($"Было введено значение {parseResult.GetValue(taskOption)}");
                if (parseResult.GetValue(taskOption) >0)
                {
                    ToDo task = ToDo.LoadTasks().Find(v => v.Id == parseResult.GetValue(taskOption));

                    if (task == null) return;

                    if (task.IsCompleted) { Console.WriteLine("Задача уже была выполнена"); return; }


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



}