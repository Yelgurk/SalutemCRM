using Avalonia.Markup.Xaml.Converters;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using DynamicData;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SalutemCRM.Database;
using SalutemCRM.Domain.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SalutemCRM.Reactive;

public interface IReactiveControlSource
{
    public void SearchByInput(string keyword);

    public void ServerResponse(object model);

    public void ServerResponse(List<object> models);
}

public partial class ReactiveControlSource<T> : ObservableObject, IReactiveControlSource
{
    /* UI behavior of the control */
    [ObservableProperty]
    private bool _isFuncAddNewAvailable = true;

    [ObservableProperty]
    private bool _isFuncEditAvailable = true;

    [ObservableProperty]
    private bool[] _activePage = { false };

    [ObservableProperty]
    private bool _isListPreview = true;

    [ObservableProperty]
    private bool _isMoneyStatVisible = false;

    [ObservableProperty]
    private bool _isBossStatVisible = true;

    public required int PagesCount { set => ResetPages(value, 0); }

    private void ResetPages(int _size, int _isActive)
    {
        bool[] _pages = new bool[_size];

        for (int i = 0; i < _size; i++)
            _pages[i] = false;

        _pages[_isActive] = true;
        this.ActivePage = _pages;
    }

    public void SetActivePage(int Page) => ResetPages(ActivePage.Length, Page);

    public IBrush DynamicColor
    {
        get
        {
            App.Current!.TryGetResource("Control_Green", App.Current!.ActualThemeVariant, out var res1);
            App.Current!.TryGetResource("Control_Orange", App.Current!.ActualThemeVariant, out var res2);

            return (IBrush)ColorToBrushConverter.Convert(
                !IsResponsiveControl ? Brushes.White : new SolidColorBrush(IsItemSelected ? (Color)res1! : (Color)res2!)
                , typeof(string)
            )!;
        }
    }



    /* Initialization of common source parameters of a control */
    public bool IsItemSelected => SelectedItem != null;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(DynamicColor))]
    private bool _isResponsiveControl = false;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsItemSelected))]
    [NotifyPropertyChangedFor(nameof(DynamicColor))]
    private T? _selectedItem = default(T?);

    [ObservableProperty]
    private T? _tempItem = default(T?);

    [ObservableProperty]
    private T? _editItem = default(T?);



    [ObservableProperty]
    private string _searchInputStr = "";
    partial void OnSearchInputStrChanged(string? oldValue, string newValue) => SearchByInput(SearchInputStr);
    public virtual void SearchByInput(string keyword) { }



    /* Block of external influence processing on all view model sources with possibility of logic overriding */
    public void ServerResponse(List<object> models) => models.DoForEach(this.ServerResponse);

    public void ServerResponse(object model) => model.DoIf(x => HandleServerResponse((T)x), x => x.GetType() == typeof(T));

    public virtual void HandleServerResponse(T model) => Debug.WriteLine($"{typeof(T)} proceed");




    /* Selected item bridge through delegate */

    partial void OnSelectedItemChanged(T? value) => SelectedItemChangedTrigger?.Invoke(value!);

    public SelectedItemChangedCommandDelegate? SelectedItemChangedTrigger { get; set; }

    public delegate void SelectedItemChangedCommandDelegate(T item);
}
