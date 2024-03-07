using Avalonia.Controls;
using Microsoft.EntityFrameworkCore;
using SalutemCRM.Database;
using SalutemCRM.Domain.Model;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace SalutemCRM.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
}
