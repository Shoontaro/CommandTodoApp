using System;
using System.Collections.Generic;
using System.Text;
using Spectre.Console;

namespace CommandTodoApp
{
    public interface IView {
        void Write(string text);
        void WriteLine(string text);
        string Read();
    }
    public class View : IView
    {
        public void Write(string text) => Console.Write(text);
        
        public void WriteLine(string text) => Console.WriteLine(text);

        public string Read() => Console.ReadLine()??"";
        
    }

    public class Spectre : IView
    {
        public string Read()
        {
           return Console.ReadLine() ?? "";
        }

        public void Write(string text)
        {
            AnsiConsole.Markup(text);
        }

        public void WriteLine(string text)
        {
            AnsiConsole.MarkupLine(text);
        }
    }
}
