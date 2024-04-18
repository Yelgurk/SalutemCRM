using Avalonia.Controls;
using SalutemCRM.Control;
using SalutemCRM.Domain.Model;
using SalutemCRM.ViewModels;

namespace SalutemCRM.FunctionalControl
{
    public partial class WarehouseGeneral : UserControl
    {
        private CRUSWarehouseItemControlViewModel? WarehouseItemVM;

        public WarehouseGeneral()
        {
            InitializeComponent();

            WarehouseItemVM = CRUS_WIC.DataContext as CRUSWarehouseItemControlViewModel;

            (CRUS_WCC.DataContext as CRUSWarehouseCategoryControlViewModel)!.Source.SelectedItemChangedTrigger = (x) => {
                WarehouseItemVM!.Source.WarehouseCategory = x;
            };
        }
    }
}
