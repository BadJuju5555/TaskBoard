using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using TaskBoard.Models;
using TaskBoard.ViewModels;
using TaskBoard.Services;
using TaskBoard.Views;

namespace TaskBoard.Views;


public partial class MainWindow : Window
{
    //
    private Point _startPoint;
   




    public MainWindow()
    {
        InitializeComponent();
    }


    // Startposition für Drag & Drop merken
    private void ListBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        _startPoint = e.GetPosition(null);
    }

    // Drag & Drop innerhalb bzw. zwischen Listen (Aufgaben)
    private void ListBox_PreviewMouseMove(object sender, MouseEventArgs e)
    {
        if(e.LeftButton != MouseButtonState.Pressed)
        {
            return;
        }

        Point mousePos = e.GetPosition(null);
        Vector diff = _startPoint - mousePos;

        if(Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
            Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance)
        {
            ListBox listBox = sender as ListBox;
            if(listBox == null)
            {
                return;
            }

            ListBoxItem listBoxItem = FindAncestor<ListBoxItem>((DependencyObject)e.OriginalSource);
            if (listBoxItem == null)
            {
                return;
            }

            KanbanTask task = listBox.ItemContainerGenerator.ItemFromContainer(listBoxItem) as KanbanTask;
            if(task == null)
            {
                return;
            }

            DataObject dragData = new DataObject("kanbanTaskFormat", task);
            DragDrop.DoDragDrop(listBoxItem, dragData, DragDropEffects.Move);
        }
    }

    // Beim Drop einer Aufgabe: Zuerst aus der Quellspalte entfernen, dann in die Zielspalte einfügen
    private void ListBox_Drop(object sender, DragEventArgs e)
    {
        if (e.Data.GetDataPresent("kanbanTaskFormat"))
        {
            KanbanTask task = e.Data.GetData("kanbanTaskFormat") as KanbanTask;
            ListBox targetListBox = sender as ListBox;
            if (task == null || targetListBox == null)
            {
                return;
            }

            KanbanColumnViewModel targetColumn = targetListBox.DataContext as KanbanColumnViewModel;
            if (targetColumn == null)
            {
                return;
            }

            MainViewModel mainVM = DataContext as MainViewModel;
            if (mainVM != null)
            {
                foreach (var column in mainVM.Columns)
                {
                    if (column.Tasks.Contains(task))
                    {
                        column.Tasks.Remove(task);
                        break;
                    }
                }
            }
            targetColumn.Tasks.Add(task);

        }
    }

    // Doppelclick auf Aufgabe: Öffnet den TaskEditorWindow
    private void ListBox_MouseDoubleClick(object sender, MouseEventArgs e)
    {
        ListBox listBox = sender as ListBox;
        if(listBox == null)
        {
            return;
        }

        KanbanTask task = listBox.SelectedItem as KanbanTask;
        if(task == null)
        {
            return;
        }

        TaskEditorWindow editor = new TaskEditorWindow();
        editor.DataContext = new TaskEditorViewModel(task, editor);
        editor.Owner = this;
        editor.ShowDialog();
    }

    // Kontextmenü - Bearbeiten einer Aufgabe
    private void Task_Edit_Click(object sender, RoutedEventArgs e)
    {
        if(((MenuItem)sender).DataContext is KanbanTask task)
        {
            TaskEditorWindow editor = new TaskEditorWindow();
            editor.DataContext = new TaskEditorViewModel(task, editor);
            editor.Owner = this;
            editor.ShowDialog();
        }
    }

    // Kontextmenü - Löschen einer Aufgabe
    private void Task_Delete_Click(object sender, RoutedEventArgs e)
    {
        if(((MenuItem)sender).DataContext is KanbanTask task)
        {
            MainViewModel mainVM = DataContext as MainViewModel;

            if(mainVM != null)
            {
                foreach(var column in mainVM.Columns)
                {
                    if(column.Tasks.Contains(task))
                    {
                        if(MessageBox.Show("Aufgabe wirklich löschen?", "Löschen", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                        {
                            column.Tasks.Remove(task);
                        }
                        break;
                    }
                }
            }
        }
    }

    // Menüaktionen: Speichern und Laden
    private void MenuItem_Save_Click(object sender, RoutedEventArgs e)
    {
        MainViewModel vm = DataContext as MainViewModel;
        if(vm != null)
        {
            DataService.SaveBoard(vm);
            MessageBox.Show("Board saved.", "Save", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    private void MenuItem_Load_Click(object sender, RoutedEventArgs e)
    {
        MainViewModel vm = DataContext as MainViewModel;
        if(vm != null)
        {
            DataService.LoadBoard(vm);
            MessageBox.Show("Board loaded.", "Load", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }


    // Hilfsmethode zum Suchen der letzten im visual tree
    private static T FindAncestor<T>(DependencyObject current) where T : DependencyObject
    {
        while(current != null)
        {
            if(current is T)
            {
                return (T)current;
            }
            current = VisualTreeHelper.GetParent(current);
        }
        return null;
    }

}