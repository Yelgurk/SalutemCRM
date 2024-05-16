using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using SalutemCRM.Domain.Model;
using SalutemCRM.Reactive;
using SalutemCRM.TCP;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SalutemCRM.ViewModels;

public partial class AuthorizationControlViewModelSource : ReactiveControlSource<User>
{
    [ObservableProperty]
    private bool _isError = false;
}

public class AuthorizationControlViewModel : ViewModelBase<User, AuthorizationControlViewModelSource>
{
    public ReactiveCommand<Unit, Unit> GoToRegistrationFormCommand { get; protected set; }

    public ReactiveCommand<Unit, Unit> TryLogInCommand { get; protected set; }

    public AuthorizationControlViewModel() : base(new() { PagesCount = 2 })
    {
        Source.SelectedItem = User.Default;

        GoToRegistrationFormCommand = ReactiveCommand.Create(() => Source.SetActivePage(1));

        TryLogInCommand = ReactiveCommand.Create(() => {
            App.Host!.Services.GetService<TCPChannel>()!.Send(JsonSerializer.Serialize(Source.SelectedItem), MBEnums.USER_JSON);
            Source.IsError = true;
        });
    }
}
