using System;
using System.Collections.Generic;
using System.Text;

namespace CommandTodoApp
{
    public interface IView
    {
        void Write(string text);
        void WriteLine(string text);
        string Read();
    }
    public interface IDataProvider
    {
        public Task<List<ToDo>> LoadTasksAsync();

        public Task SaveTasksAsync(List<ToDo> todos);
    }
    public interface IDataProcessor
    {
        Task ShowTasksAsync(string stat);
        Task AddTaskAsync(ToDo task);
        Task UpdateTaskAsync(int id, string name);
        Task TaskInProgressAsync(int id);
        Task DoneTaskAsync(int id);
        Task DeleteTaskAsync(int id);
    }
}
