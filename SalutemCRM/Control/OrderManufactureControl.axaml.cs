using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SalutemCRM.Services;
using SalutemCRM.ViewModels;

namespace SalutemCRM.Control
{
    public partial class OrderManufactureControl : UserControl
    {
        public OrderManufactureControl()
        {
            InitializeComponent();

            this.DataContext = App.Host!.Services.GetService<OrderManufactureControlViewModel>();
        }
    }
}
