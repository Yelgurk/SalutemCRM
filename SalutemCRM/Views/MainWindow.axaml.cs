using Avalonia.Controls;
using Microsoft.EntityFrameworkCore;
using SalutemCRM.Database;
using SalutemCRM.Domain.Model;
using SalutemCRM.ViewModels;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace SalutemCRM.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        DataContext = this;

        InitializeComponent();

        (SVC.DataContext as CRUSVendorControlViewModel)!
       .DoInst(v => v.Source.IsResponsiveControl = true)
       .DoInst(v => v.Source.IsFuncAddNewAvailable = false)
       .DoInst(v => v.Source.IsFuncEditAvailable = false);
    }
}
