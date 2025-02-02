using System;
using System.Collections.Generic;


namespace TaskBoard.Models;

public class BoardData
{
    public List<ColumnData> Columns { get; set; } = new List<ColumnData>();
}

public class ColumnData
{
    public string ColumnName { get; set; }
    public List<TaskData> Tasks { get; set; } = new List<TaskData>();
}

public class TaskData
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime? DueDate { get; set; }
    public string Priority { get; set; }
}
