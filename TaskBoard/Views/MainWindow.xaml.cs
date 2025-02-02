using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using TaskBoard.Models;
using TaskBoard.ViewModels;
using TaskBoard.Services;

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

    }
}