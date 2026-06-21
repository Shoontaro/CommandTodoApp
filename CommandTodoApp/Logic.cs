using System;
using System.Collections.Generic;
using System.Text;

namespace CommandTodoApp
{
    public class Logic : IDataProcessor
    {
        public IDataProvider dataProvider { get; set; }
        public IView view { get; set; }
        public Logic(IDataProvider dataProvider, IView view) {
        this.dataProvider = dataProvider;
            this.view = view;
        }

        public void ShowTasks(string stat)
        {
            List<ToDo> tasks = stat.Trim().ToLower() switch { //Switch Pattern Matching
                "all" => dataProvider.LoadTasks(),
                "in-progress" => dataProvider.LoadTasks().Where(v => v.status == Status.inProgress).ToList(),
                "done" => dataProvider.LoadTasks().Where(v => v.status == Status.done).ToList(),
                "todo" => dataProvider.LoadTasks().Where(v => v.status == Status.todo).ToList(),
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

            view.WriteLine($"{(tasks.Count > 0 ? mess : "Данных нет")}");

            foreach (ToDo todo in tasks)
            {
                view.WriteLine($"[{todo.CreateAt.ToShortDateString()}] (ID: {todo.Id}) {todo.Name} {todo.status}");
            }
        }

        public void AddTask(ToDo task)
        {
            view.WriteLine(
                $"\n Создаем таск. \n" +
                $" Название {task.Name}\n" +
                $" Готовность {task.status}");

            List<ToDo> tasks = dataProvider.LoadTasks();
            task.Id = tasks.Count > 0 ? tasks.Last().Id + 1 : 1;

            tasks.Add(task);
            dataProvider.SaveTasks(tasks);

            view.WriteLine($"Задача успешно добавлена! Ее id {task.Id}");
        }

        public void UpdateTask(int id, string name)
        {
            List<ToDo> tasks = dataProvider.LoadTasks();

            ToDo task = tasks.Find(v => v.Id == id);
            if (task == null) { view.WriteLine("Нет задачи с таким id"); return; }

            task.Name = name;

            dataProvider.SaveTasks(tasks);

            view.WriteLine($"Задача {id} изменена");
        }

        public void TaskInProgress(int id)
        {
            List<ToDo> tasks = dataProvider.LoadTasks();

            ToDo task = tasks.Find(v => v.Id == id);

            if (task == null) { view.WriteLine("Нет задачи с таким id"); return; }

            task.InProgress();

            dataProvider.SaveTasks(tasks);

            view.WriteLine($"Задача {id} отмечена как в процессе");
        }

        public void DoneTask(int id)
        {
            List<ToDo> tasks = dataProvider.LoadTasks();

            ToDo task = tasks.Find(v => v.Id == id);

            if (task == null) { view.WriteLine("Нет задачи с таким id"); return; }

            task.Done();

            dataProvider.SaveTasks(tasks);

            view.WriteLine($"Задача {id} отмечена как завершенная");
        }

        public void DeleteTask(int id)
        {
            List<ToDo> tasks = dataProvider.LoadTasks();

            ToDo task = tasks.Find(v => v.Id == id);

            if (task == null) { view.WriteLine("Нет задачи с таким id"); return; }

            tasks.Remove(task);

            dataProvider.SaveTasks(tasks);

            view.WriteLine("Задача удалена");
        }

    }
}
