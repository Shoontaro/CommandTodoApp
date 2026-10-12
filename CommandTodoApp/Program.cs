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
    private static async Task Main(string[] args)
    {
        Console.WriteLine("Приложение для задачь. Добавьте задачу");

       await Commands.GetTask(new Logic(new FileRepo(), new View()));
    }

  
}