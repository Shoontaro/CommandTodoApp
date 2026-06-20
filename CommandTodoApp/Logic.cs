using System;
using System.Collections.Generic;
using System.Text;

namespace CommandTodoApp
{
    public class Logic
    {
        public static void ShowTasks(string stat)
        {
            List<ToDo> tasks = stat.Trim().ToLower() switch { //Switch Pattern Matching
                "all" => FileRepo.LoadTasks(),
                "in-progress" => FileRepo.LoadTasks().Where(v => v.status == Status.inProgress).ToList(),
                "done" => FileRepo.LoadTasks().Where(v => v.status == Status.done).ToList(),
                "todo" => FileRepo.LoadTasks().Where(v => v.status == Status.todo).ToList(),
                _=> new List<ToDo>()
            };

           string mess = stat.Trim().ToLower() switch 
            { 
                "all"=> "\n Отображаем лист всех заданий:",
                "done"=> "\n Отображаем лист выполненных заданий:",
                "todo"=> "\n Отображаем лист запланированных заданий:",
                "in-progress" => "\n Отображаем лист заданий в процессе выполнения:",
                _ => ""
            };

            Console.WriteLine($"{(tasks.Count > 0 ? mess : "Данных нет")}");

            foreach (ToDo todo in tasks)
            {
                Console.WriteLine($"[{todo.CreateAt.ToShortDateString()}] (ID: {todo.Id}) {todo.Name} {todo.status}");
            }
        }

        public static void AddTask(ToDo task)
        {
            Console.WriteLine(
                $"\n Создаем таск. \n" +
                $" Название {task.Name}\n" +
                $" Готовность {task.status}");

            List<ToDo> tasks = FileRepo.LoadTasks();
            task.Id = tasks.Count > 0 ? tasks.Last().Id + 1 : 1;

            tasks.Add(task);
            FileRepo.SaveTasks(tasks);

            Console.WriteLine($"Задача успешно добавлена! Ее id {task.Id}");
        }

        public static void UpdateTask(int id, string name)
        {
            List<ToDo> tasks = FileRepo.LoadTasks();

            ToDo task = tasks.Find(v => v.Id == id);
            if (task == null) { Console.WriteLine("Нет задачи с таким id"); return; }

            task.Name = name;

            FileRepo.SaveTasks(tasks);

            Console.WriteLine($"Задача {id} изменена");
        }

        public static void TaskInProgress(int id)
        {
            List<ToDo> tasks = FileRepo.LoadTasks();

            ToDo task = tasks.Find(v => v.Id == id);

            if (task == null) { Console.WriteLine("Нет задачи с таким id"); return; }

            task.InProgress();

            FileRepo.SaveTasks(tasks);

            Console.WriteLine($"Задача {id} отмечена как в процессе");
        }

        public static void DoneTask(int id)
        {
            List<ToDo> tasks = FileRepo.LoadTasks();

            ToDo task = tasks.Find(v => v.Id == id);

            if (task == null) { Console.WriteLine("Нет задачи с таким id"); return; }

            task.Done();

            FileRepo.SaveTasks(tasks);

            Console.WriteLine($"Задача {id} отмечена как завершенная");
        }

        public static void DeleteTask(int id)
        {
            List<ToDo> tasks = FileRepo.LoadTasks();

            ToDo task = tasks.Find(v => v.Id == id);

            if (task == null) { Console.WriteLine("Нет задачи с таким id"); return; }

            tasks.Remove(task);

            FileRepo.SaveTasks(tasks);

            Console.WriteLine("Задача удалена");
        }

    }
}
