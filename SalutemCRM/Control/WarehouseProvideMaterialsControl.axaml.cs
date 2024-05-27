using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using SalutemCRM.ViewModels;

namespace SalutemCRM.Control
{
    public partial class WarehouseProvideMaterialsControl : UserControl
    {
        public WarehouseProvideMaterialsControl()
        {
            InitializeComponent();
            this.DataContext = App.Host!.Services.GetService<WarehouseProvideMaterialsControlViewModel>()!;
        }
    }
}
