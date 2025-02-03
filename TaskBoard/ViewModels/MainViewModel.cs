using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using TaskBoard.Models;
using TaskBoard.ViewModels;
using TaskBoard.Views;

namespace TaskBoard.ViewModels;

public class MainViewModel : BaseViewModel
{
    //
    public ObservableCollection<KanbanColumnViewModel> Columns { get; } = new ObservableCollection<KanbanColumnViewModel>();

    // Commands für die Spaltenverwaltung
    public ICommand AddColumnCommand { get; }
    public ICommand DeleteColumnCommand { get; }
    public ICommand UndoCommand { get; }
    public ICommand RedoCommand { get; }


    



    public MainViewModel()
    {
        AddColumnCommand = new RelayCommand(_ => AddColumn());
        DeleteColumnCommand = new RelayCommand(DeleteColumn, _ => SelectedColumn != null);


        // Beispielspalten
        var todo = new KanbanColumnViewModel() { ColumnName = "To Do" };
        var inProgress = new KanbanColumnViewModel() { ColumnName = "In Progress" };
        var done = new KanbanColumnViewModel() { ColumnName = "Done" };

        // Beispielaufgaben
        todo.Tasks.Add(new KanbanTask()
        {
            Title = "Aufgabe 1",
            Description = "Beschreibung 1",
            DueDate = System.DateTime.Now.AddDays(3),
            Priority = "High"
        });
        todo.Tasks.Add(new KanbanTask()
        {
            Title = "Aufgabe 2",
            Description = "Beschreibung 2",
            DueDate = System.DateTime.Now.AddDays(5),
            Priority = "Medium"
        });

        Columns.Add(todo);
        Columns.Add(inProgress);
        Columns.Add(done);
    }

    //
    private KanbanColumnViewModel _selectedColumn;
    public KanbanColumnViewModel SelectedColumn
    {
        get => _selectedColumn;
        set => SetProperty(ref _selectedColumn, value);
    }

    //
    private void AddColumn()
    {
        // Spalte über einen Dialog hinzufügen
        InputDialog dialog = new InputDialog("Neue Spalte:", "Neue Spalte");
        dialog.Owner = Application.Current.MainWindow;

        if(dialog.ShowDialog() == true)
        {
            Columns.Add(new KanbanColumnViewModel() { ColumnName = dialog.InputText });
        }
    }

    private void DeleteColumn(object parameter)
    {
        if(parameter is KanbanColumnViewModel col)
        {
            if(MessageBox.Show($"Spalte '{col.ColumnName}' wirklich löschen?", "Löschen", MessageBoxButton.YesNo, MessageBoxImage.Warning)== MessageBoxResult.Yes)
            {
                Columns.Remove(col);
            }
        }
    }
}