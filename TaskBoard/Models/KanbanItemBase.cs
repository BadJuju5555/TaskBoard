using System;


namespace TaskBoard.Models;

public abstract class KanbanItemBase
{
    public string Title { get; set; }
    public string Description { get; set; }
}

public class KanbanTask : KanbanItemBase
{
    public DateTime? DueDate { get; set; }
    public string Priority { get; set; }
}