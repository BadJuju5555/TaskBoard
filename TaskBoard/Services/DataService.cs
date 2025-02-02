using System.IO;
using System.Text.Json;
using TaskBoard.ViewModels;
using TaskBoard.Models;


namespace TaskBoard.Services;

public static class DataService
{
    private static readonly string FilePath = "kanbanBoard.json";

    public static void SaveBoard(MainViewModel board)
    {
        BoardData data = new BoardData();

        foreach(var column in board.Columns)
        {
            ColumnData colData = new ColumnData
            {
                ColumnName = column.ColumnName
            };

            foreach(var task in column.Tasks)
            {
                colData.Tasks.Add(new TaskData()
                {
                    Title = task.Title,
                    Description = task.Description,
                    DueDate = task.DueDate,
                    Priority = task.Priority
                });
            }
            data.Columns.Add(colData);
        }

        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(data, options);
        File.WriteAllText(FilePath, json);
    }

    public static void LoadBoard(MainViewModel board)
    {
        if(!File.Exists(FilePath))
        {
            return;
        }

        string json = File.ReadAllText(FilePath);
        BoardData data = JsonSerializer.Deserialize<BoardData>(json);

        board.Columns.Clear();

        foreach(var colData in data.Columns)
        {
            var column = new KanbanColumnViewModel() { ColumnName = colData.ColumnName };

            foreach(var taskData in colData.Tasks)
            {
                column.Tasks.Add(new KanbanTask()
                {
                    Title = taskData.Title,
                    Description = taskData.Description,
                    DueDate = taskData.DueDate,
                    Priority = taskData.Priority
                });
            }
            board.Columns.Add(column);
        }
    }
}