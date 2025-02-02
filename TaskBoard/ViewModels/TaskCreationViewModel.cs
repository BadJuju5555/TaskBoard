using System;
using System.Windows;
using System.Windows.Input;
using TaskBoard.Models;
using TaskBoard.ViewModels;
using TaskBoard.Services;

namespace TaskBoard.ViewModels;

public class TaskCreationViewModel : BaseViewModel
{
    // private Prop
    private Window _window;
    private string _title;
    private string _description;
    private DateTime? _dueDate;
    private string _priority;

    //
    public TaskCreationViewModel(Window window)
    {
        _window = window;
        SaveCommand = new RelayCommand(_ => Save(), _ => CanSave());
        CancelCommand = new RelayCommand(_ => Cancel());

        // Standardwerte
        DueDate = DateTime.Now;
        Priority = "Medium";
    }


    //
    public string Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }

    public string Description
    {
        get => _description;
        set => SetProperty(ref _description, value);
    }

    public DateTime? DueDate
    {
        get => _dueDate;
        set => SetProperty( ref _dueDate, value);
    }

    public string Priority
    {
        get => _priority;
        set => SetProperty(ref _priority, value);
    }

    // Commands
    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    public KanbanTask CreatedTask { get; private set; }

    //
    private bool CanSave()
    {
        // Einfache Validierung: Titel darf nicht leer sein
        return !string.IsNullOrWhiteSpace(Title);
    }

    private void Save()
    {
        CreatedTask = new KanbanTask()
        {
            Title = this.Title,
            Description = this.Description,
            DueDate = this.DueDate,
            Priority = this.Priority
        };
        _window.DialogResult = true;
        _window.Close();
    }


    private void Cancel()
    {
        _window.DialogResult = false;
        _window.Close();
    }



}
