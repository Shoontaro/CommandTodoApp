using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Text;

namespace CommandTodoApp
{
    public interface ICommands
    {
        void GetTask(IDataProcessor dataProcessor);
    }

    public class Commands
    {
        public static void GetTask(IDataProcessor dataProcessor)
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

                Argument<string> statArg = new("status")
                {
                    DefaultValueFactory = (_) => "all", //объектный инициализатор
                    Description = "Статус выводимых данных (done/todo/in-progress)"
                };

                Option<string> titleOption = new(name: "--title", aliases: "-ttl")
                {
                    Description = "Задать название задачи"
                };

                Option<bool> doneOption = new("--done")
                {
                    Description = "Задача выполнена"
                };

                Command addCommand = new("add", "Создать задачу") //подкоманда
            {
                nameArg,
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
                        dataProcessor.TaskInProgress(parseResult.GetValue(idArg));
                    }
                });

                updateCommand.SetAction(parseResult => {
                    dataProcessor.UpdateTask(parseResult.GetValue(idArg), parseResult.GetValue(nameArg) ?? "");
                });

                addCommand.SetAction(parseResult =>
                {
                    ToDo todo = new ToDo(parseResult.GetValue(nameArg) ?? "", parseResult.GetValue(doneOption));
                    dataProcessor.AddTask(todo);
                });
                
                listCommand.SetAction(parseResult =>
                {
                    dataProcessor.ShowTasks(parseResult.GetValue(statArg) ?? "");
                });

                doneCommand.SetAction(parseResult =>
                {
                    if (parseResult.GetValue(idArg) > 0)
                    {
                        dataProcessor.DoneTask(parseResult.GetValue(idArg));
                    }
                });

                delCommand.SetAction(parseResult => {
                    Console.WriteLine($"Удаляем запись {parseResult.GetValue(idArg)}");

                    dataProcessor.DeleteTask(parseResult.GetValue(idArg));
                });

                rootCommand.Parse(command).Invoke();
            }
        }
    }
}
