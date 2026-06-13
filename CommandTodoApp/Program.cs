using System.CommandLine;
using System.CommandLine.Parsing;
using System.IO;
using System.Linq;
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
            Console.WriteLine($" Создаем таск. \n" +
            $" Название {parseResult.GetValue(titleOption)}\n" +
            $" Описание {parseResult.GetValue(descOptiion)} \n" +
            $" Готовность {parseResult.GetValue(doneOption)}"));

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