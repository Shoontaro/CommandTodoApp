using System;
using System.Collections.Generic;
using System.Text;
using Spectre.Console;

namespace CommandTodoApp
{
    public class View
    {
        public static void Write(string text) => Console.Write(text);
        
        public static void WriteLine(string text) => Console.WriteLine(text);

        public static string Read() => Console.ReadLine()??"";
        
    }
}
