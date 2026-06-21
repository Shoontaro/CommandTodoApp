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
        public List<ToDo> LoadTasks();

        public void SaveTasks(List<ToDo> todos);
    }
    public interface IDataProcessor
    {
        void ShowTasks(string stat);
        void AddTask(ToDo task);
        void UpdateTask(int id, string name);
        void TaskInProgress(int id);
        void DoneTask(int id);
        void DeleteTask(int id);
    }
}
