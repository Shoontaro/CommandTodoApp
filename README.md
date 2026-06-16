#The application run from the command line, accept user actions and inputs as arguments, and store the tasks in a JSON file. The user able to:
- Add, Update, and Delete tasks
- Mark a task as in progress or done
- List all tasks
- List all tasks that are done
- List all tasks that are not done
- List all tasks that are in progress

#Example

```csharp
// Help
-h -? -help

// Adding a new task
add "new task"

// Updating and deleting tasks
update 1 "task was update"
delete 1

// Marking a task as in progress or done
mark-in-progress 1
mark-done 1

// Listing all tasks
list

// Listing tasks by status
list done
list todo
list in-progress
```
