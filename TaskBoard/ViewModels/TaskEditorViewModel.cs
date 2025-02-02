using System.Windows;
using System.Windows.Input;
using TaskBoard.Models;
using TaskBoard.ViewModels;
using TaskBoard.Services;

namespace TaskBoard.ViewModels;

public class TaskEditorViewModel : BaseViewModel
{
    //
    private KanbanTask _task;
    private Window _window;

    //
    public TaskEditorViewModel(KanbanTask task, Window window)
    {
        _task = task;
        _window = window;
        Title = task.Title;
        Description = task.Description;
        DueDate = task.DueDate;
        Priority = task.Priority;
        SaveCommand = new RelayCommand(_ => Save(), _ => CanSave());
        CancelCommand = new RelayCommand(_ => Cancel());

    }

    //

    private string _title;
    public string Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }

    private string _description;
    public string Description
    {
        get => _description;
        set => SetProperty(ref _description, value);
    }

    private DateTime? _dueDate;
    public DateTime? DueDate
    {
        get => _dueDate;
        set => SetProperty( ref _dueDate, value);
    }

    private string _priority;
    public string Priority
    {
        get => _priority;
        set => SetProperty( ref _priority, value);
    }

    //
    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    private bool CanSave() => !string.IsNullOrWhiteSpace(Title);

    //

    private void Save()
    {
        _task.Title = Title;
        _task.Description = Description;
        _task.DueDate = DueDate;
        _task.Priority = Priority;
        _window.DialogResult = true;
        _window.Close();
    }

    private void Cancel()
    {
        _window.DialogResult = false;
        _window.Close();
    }
}
