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
            Console.WriteLine();
            string command = Console.ReadLine() ?? "";

            Argument<int> idArg = new("id")
            { 
            Description = "id задачи"
            };

            Argument<string> nameArg = new("name")
            {
                Description = "Название задачи"
            };

            Argument<string> statArg = new("status") {
                DefaultValueFactory = (_)=>"all", //объектный инициализатор
                Description = "Статус выводимых данных (done/todo/in-progress)"
            };

            //Option<bool> allOption = new("--all")
            //{
            //    Description = "Вывести все задачи"
            //};

            Option<string> titleOption = new(name: "--title", aliases: "-ttl")
            {
                Description = "Задать название задачи"
            };

            //Option<string> descOptiion = new(name: "--desc", aliases: "-d")
            //{
            //    Description = "Задать описание задачи"
            //};

            Option<bool> doneOption = new("--done")
            {
                Description = "Задача выполнена"
            };

            Command addCommand = new("add", "Создать задачу") //подкоманда
            {
                nameArg,
               // titleOption,
              //  descOptiion,
                doneOption
            };
            addCommand.Aliases.Add("create");

            Command listCommand = new Command("list", "Отобразить задачи")
            {
               // allOption, //если нужно отобразить в том числе и выполненные задачи
                statArg
            };
            listCommand.Aliases.Add("show");

            Command doneCommand = new Command("mark-done", "Завершить задачу")
            {
                idArg
            };
            doneCommand.Aliases.Add("done");

            Command inProgressCommand = new Command("mark-in-progress", "Задача в процессе")
            {
                idArg
            };
            inProgressCommand.Aliases.Add("progress");

            Command delCommand = new Command("delete", "Удалить задачу")
            {
                idArg
            };
            delCommand.Aliases.Add("del");

            Command updateCommand = new Command("update", "Обновить задачу") { 
                idArg,
                nameArg
            };

            RootCommand rootCommand = new("Todo проект с использованием System.CommandLine")
            {
            addCommand,
            listCommand,
            doneCommand,
            delCommand,
            inProgressCommand,
            updateCommand
            };
            
            inProgressCommand.SetAction(parseResult => 
            {
                if (parseResult.GetValue(idArg) > 0)
                {
                    ToDo.TaskInProgress(parseResult.GetValue(idArg));
                }
            });

            updateCommand.SetAction(parseResult => {
                ToDo.UpdateTask(parseResult.GetValue(idArg), parseResult.GetValue(nameArg)??"");
            });

            addCommand.SetAction(parseResult =>
            {
                    ToDo todo = new ToDo(parseResult.GetValue(nameArg) ?? "", parseResult.GetValue(doneOption));
                todo.AddTask(todo);
            });

            listCommand.SetAction(parseResult =>
            {
                ToDo.ShowTasks(parseResult.GetValue(statArg)??"");
            });

            doneCommand.SetAction(parseResult => 
            {
                if (parseResult.GetValue(idArg) >0)
                {
                    ToDo.DoneTask(parseResult.GetValue(idArg));
                }
            });

            delCommand.SetAction(parseResult => {
                Console.WriteLine($"Удаляем запись {parseResult.GetValue(idArg)}");

                ToDo.DeleteTask(parseResult.GetValue(idArg));
            });

            rootCommand.Parse(command).Invoke(); //запуск делегата
        }
    }



}