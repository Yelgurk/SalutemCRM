using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Microsoft.Extensions.DependencyInjection;
using SalutemCRM.ViewModels;
using System;
using System.Linq;

namespace SalutemCRM.Control
{
    public partial class WarehouseKeeperOrders : UserControl
    {
        public WarehouseKeeperOrders()
        {
            InitializeComponent();
            this.DataContext = App.Host!.Services.GetService<WarehouseKeeperOrdersViewModel>()!;
        }

        private void UniformGrid_SizeChanged(object? sender, Avalonia.Controls.SizeChangedEventArgs e)
        {
            int size = 400;
            UniformGrid UG = (UniformGrid)sender!;
            UG.Columns = Convert.ToInt32(Double.IsNaN(UG.Bounds.Width) ? size : UG.Bounds.Width / size);
        }
    }
}
