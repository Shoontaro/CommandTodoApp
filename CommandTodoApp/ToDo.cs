using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Text;
using System.Text.Json;

namespace CommandTodoApp
{
    public enum Status
    { 
        todo,
        inProgress,
        done
    }

    public class ToDo
    {
        public int Id { get; set; }
        public string Name { get; set; }// = "Task";
        public Status status { get; set; } = Status.todo;
        public DateTime CreateAt { get; set; }
        public DateTime DoneAt { get; set; }
        public bool IsCompleted { get; set; }

        public ToDo() { }
        public ToDo(string name)
        {
            this.Name = name;
            CreateAt = DateTime.Now;
        }

        public ToDo(string name, bool isCompleted) : this(name)
        {
            IsCompleted = isCompleted;
            if (isCompleted)
            {
                this.status = Status.done;
                DoneAt = DateTime.Now;
            }
        }

        public void Done() {
            if (this.status == Status.done) return;

            //this.IsCompleted = true;
            this.status = Status.done;
            this.DoneAt = DateTime.Now;
        }
        public void InProgress() {
            if (this.status == Status.inProgress) return;

            this.status = Status.inProgress;
            this.DoneAt = DateTime.Now;
        }
    }
}
