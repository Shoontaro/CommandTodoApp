using System;
using System.Collections.Generic;
using System.Text;

namespace CommandTodoApp
{
    public class ToDo
    {
        public string Name { get; set; }// = "Task";
        public string Description { get; set; }// = "Description";
        public DateTime CreateAt { get; set; }
        public DateTime DoneAt { get; set; }
        public bool IsCompleted { get; set; }

        public ToDo() { }
        public ToDo(string name, string desc)
        {
            this.Name = name;
            this.Description = desc;
            CreateAt = DateTime.Now;
        }

        public ToDo(string name, string description, DateTime createAt, DateTime doneAt, bool isCompleted) : this(name, description)
        {
            CreateAt = createAt;
            DoneAt = doneAt;
            IsCompleted = isCompleted;
        }
    }
}
