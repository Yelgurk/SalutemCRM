using Avalonia.Markup.Xaml.Converters;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using DynamicData;
using SalutemCRM.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalutemCRM.Interface;

public partial class ReactiveControlSource<T> : ObservableObject
{
    [ObservableProperty]
    private bool[] _activePage = { false };

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

    public bool IsItemSelected => SelectedItem != null;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(DynamicColor))]
    private bool _isResponsiveControl = false;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsItemSelected))]
    [NotifyPropertyChangedFor(nameof(DynamicColor))]
    private T? _selectedItem = default(T?);
}
