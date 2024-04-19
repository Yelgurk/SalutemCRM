using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using SalutemCRM.Domain.Model;
using SalutemCRM.Reactive;
using SalutemCRM.Services;
using SalutemCRM.ViewModels;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace SalutemCRM.Control
{
    public partial class CRUSWarehouseItemControl : UserControl
    {
        public CRUSWarehouseItemControl()
        {
            InitializeComponent();
        }

        public void Image_Loaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (!Design.IsDesignMode)
                (sender as Image)
                    .Do(x => x!.Tag as WarehouseItem)
                    .Do(x => x!.QRBitmap = App.Host!.Services.GetService<QRCodeGeneratorService>()!.Generate(x.InnerCode));
        }
    }
}
