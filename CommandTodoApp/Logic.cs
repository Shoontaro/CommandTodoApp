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

        public async Task ShowTasksAsync(string stat)
        {
            List<ToDo> fromDB = await dataProvider.LoadTasksAsync();

            List<ToDo> tasks = stat.Trim().ToLower() switch { //Switch Pattern Matching
                "all" => fromDB,
                "in-progress" => fromDB.Where(v => v.status == Status.inProgress).ToList(),
                "done" => fromDB.Where(v => v.status == Status.done).ToList(),
                "todo" => fromDB.Where(v => v.status == Status.todo).ToList(),
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

        public async Task AddTaskAsync(ToDo task)
        {
            view.WriteLine(
                $"\n Создаем таск. \n" +
                $" Название {task.Name}\n" +
                $" Готовность {task.status}");

            List<ToDo> tasks = await dataProvider.LoadTasksAsync();
            task.Id = tasks.Count > 0 ? tasks.Last().Id + 1 : 1;

            tasks.Add(task);
            await dataProvider.SaveTasksAsync(tasks);

            view.WriteLine($"Задача успешно добавлена! Ее id {task.Id}");
        }

        public async Task UpdateTaskAsync(int id, string name)
        {
            List<ToDo> tasks = await dataProvider.LoadTasksAsync();

            ToDo task = tasks.Find(v => v.Id == id)?? new ToDo();
            if (task == null) { view.WriteLine("Нет задачи с таким id"); return; }

            task.Name = name;

            await dataProvider.SaveTasksAsync(tasks);

            view.WriteLine($"Задача {id} изменена");
        }

        public async Task TaskInProgressAsync(int id)
        {
            List<ToDo> tasks = await dataProvider.LoadTasksAsync();

            ToDo task = tasks.Find(v => v.Id == id)?? new ToDo();

            if (task == null) { view.WriteLine("Нет задачи с таким id"); return; }

            task.InProgress();

            await dataProvider.SaveTasksAsync(tasks);

            view.WriteLine($"Задача {id} отмечена как в процессе");
        }

        public async Task DoneTaskAsync(int id)
        {
            List<ToDo> tasks = await dataProvider.LoadTasksAsync();

            ToDo task = tasks.Find(v => v.Id == id)?? new ToDo();

            if (task == null) { view.WriteLine("Нет задачи с таким id"); return; }

            task.Done();

            await dataProvider.SaveTasksAsync(tasks);

            view.WriteLine($"Задача {id} отмечена как завершенная");
        }

        public async Task DeleteTaskAsync(int id)
        {
            List<ToDo> tasks = await dataProvider.LoadTasksAsync();

            ToDo task = tasks.Find(v => v.Id == id) ?? new ToDo();

            if (task == null) { view.WriteLine("Нет задачи с таким id"); return; }

            tasks.Remove(task);

            await dataProvider.SaveTasksAsync(tasks);

            view.WriteLine("Задача удалена");
        }
    }
}
