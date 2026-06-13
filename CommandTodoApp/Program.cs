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

    private static void GetTask()
    {
        while (true)
        {
            string command = Console.ReadLine() ?? "";

            Option<string> fileOption = new("--task")
            {
                Description = "Задача. Пока тестовая команда"
            };

            RootCommand rootCommand = new("Todo project with System.CommandLine");
            rootCommand.Options.Add(fileOption);


            //ParseResult parseResult = rootCommand.Parse(command);

            rootCommand.SetAction(parseResult => 
            {
                string parsedFile = parseResult.GetValue(fileOption)??"";
                Console.WriteLine($"Parsed {parsedFile}");
            });

            ParseResult parseResult = rootCommand.Parse(command);
            parseResult.Invoke(); //запуск делегата

        }
    } 
    
}