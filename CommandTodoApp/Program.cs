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
    public static readonly string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "todos.json");

    private static void Main(string[] args)
    {
        View.WriteLine("Приложение для задачь. Добавьте задачу");

        Commands.GetTask();
    }

  
}