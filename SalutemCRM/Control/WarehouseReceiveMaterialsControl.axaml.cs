using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using SalutemCRM.ViewModels;

namespace SalutemCRM.Control
{
    public partial class WarehouseReceiveMaterialsControl : UserControl
    {
        public WarehouseReceiveMaterialsControl()
        {
            InitializeComponent();
            this.DataContext = App.Host!.Services.GetService<WarehouseReceiveMaterialsControlViewModel>()!;
        }
    }
}
