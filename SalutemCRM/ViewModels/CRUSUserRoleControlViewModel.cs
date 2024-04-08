using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;
using SalutemCRM.Database;
using SalutemCRM.Domain.Model;
using SalutemCRM.Domain.Modell;
using SalutemCRM.Reactive;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SalutemCRM.ViewModels;

public partial class CRUSUserRoleControlViewModelSource : ReactiveControlSource<UserRole>
{
    [ObservableProperty]
    private ObservableCollection<UserRole> _userRoles = new() {
        new UserRole() { Name = "Test 1" },
        new UserRole() { Name = "Test 2" },
        new UserRole() { Name = "Test 3" },
        new UserRole() { Name = "Test 4" },
        new UserRole() { Name = "Test 5" }
    };

    public override void SearchByInput(string keyword)
    {
        keyword = Regex.Replace(keyword.ToLower(), @"\s+", " ");

        using (DatabaseContext db = new DatabaseContext(DatabaseContext.ConnectionInit()))
            UserRoles = new(
                from c in db.UserRoles.AsEnumerable()
                where keyword.Split(" ").Any(s => c.Name.ToLower().Contains(s))
                select c
            );
    }
}

public class CRUSUserRoleControlViewModel : ViewModelBase<UserRole>
{
    public CRUSUserRoleControlViewModelSource Source { get; } = new() { PagesCount = 1 };

    public CRUSUserRoleControlViewModel()
    {
        IfSearchStrNotNull = this.WhenAnyValue(
            x => x.Source.SearchInputStr,
            (s) =>
                !string.IsNullOrWhiteSpace(s) &&
                s.Length > 0
        );

        GoBackCommand = ReactiveCommand.Create(() => {
            Source.SetActivePage(0);
        });

        ClearSearchCommand = ReactiveCommand.Create(() => {
            Source.SearchInputStr = "";
        }, IfSearchStrNotNull);



        if (!Design.IsDesignMode)
            Source.SearchByInput("");

        Source.SetActivePage(0);
    }
}
