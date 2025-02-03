using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TaskBoard.Models;
using TaskBoard.ViewModels;
using TaskBoard.Services;

namespace TaskBoard.Views;


public partial class InputDialog : Window
{
    public string InputText { get; private set; }


    public InputDialog(string message, string defaultInput = "")
    {
        InitializeComponent();
        lblMessage.Text = message;
        txtInput.Text = defaultInput;
    }

    public void OK_Click(object sender, RoutedEventArgs e)
    {
        InputText = txtInput.Text;
        DialogResult = true;
        Close();
    }
}
