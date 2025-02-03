using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using TaskBoard.Models;
using TaskBoard.ViewModels;
using TaskBoard.Services;
using TaskBoard.Views;

namespace TaskBoard.ViewModels;

public class KanbanColumnViewModel : BaseViewModel
{
    //
    private string _columnName;
    public string ColumnName
    {
        get => _columnName;
        set => SetProperty(ref _columnName, value); 
    }

    public ObservableCollection<KanbanTask> Tasks { get; } = new ObservableCollection<KanbanTask>();

    public ICommand AddTaskCommand { get; }
    public ICommand EditColumnCommand { get; }
    public ICommand DeleteColumnCommand { get; }

    //
    public KanbanColumnViewModel()
    {
        AddTaskCommand = new RelayCommand(_ => AddTask());
        EditColumnCommand = new RelayCommand(_ => EditColumn());
    }

    //
    private void AddTask()
    {
        // Öffnet den TaskCreationWindow zur detaillierten Aufgabenerfassung
        TaskCreationWindow window = new TaskCreationWindow();
        var viewModel = new TaskCreationViewModel(window);
        window.DataContext = viewModel;
        window.Owner = Application.Current.MainWindow;
        bool? result = window.ShowDialog();

        if(result == true && viewModel.CreatedTask != null)
        {
            Tasks.Add(viewModel.CreatedTask);
        }
    }

    private void EditColumn()
    {
        // Ein einfacher Dialog zum umbenennen der Spalte
        InputDialog dialog = new InputDialog("Spalte umbennen:", ColumnName);
        dialog.Owner = Application.Current.MainWindow;

        if(dialog.ShowDialog() == true)
        {
            ColumnName = dialog.InputText;
        }
    }
}
