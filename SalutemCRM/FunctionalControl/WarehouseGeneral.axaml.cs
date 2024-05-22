using Avalonia.Controls;
using SalutemCRM.Control;
using SalutemCRM.Domain.Model;
using SalutemCRM.ViewModels;

namespace SalutemCRM.FunctionalControl
{
    public partial class WarehouseGeneral : UserControl
    {
        public CRUSWarehouseItemControlViewModel? WarehouseItemVM;

        public WarehouseGeneral()
        {
            InitializeComponent();

            this.DataContext = new FunctionalControlUIProvider()
            {
                IsFuncAddNewAvailableSetter = (x) => {
                    WarehouseItemVM!.Source.IsFuncAddNewAvailable = x;
                    (CRUS_WCC.DataContext as CRUSWarehouseCategoryControlViewModel)!.Source.IsFuncAddNewAvailable = x;
                },
                IsFuncEditAvailableSetter = (x) => {
                    WarehouseItemVM!.Source.IsFuncEditAvailable = x;
                    (CRUS_WCC.DataContext as CRUSWarehouseCategoryControlViewModel)!.Source.IsFuncEditAvailable = x;
                },
                IsResponsiveControlSetter = (x) => {
                    WarehouseItemVM!.Source.IsResponsiveControl = x;
                    (CRUS_WCC.DataContext as CRUSWarehouseCategoryControlViewModel)!.Source.IsResponsiveControl = x;
                }
            };

            WarehouseItemVM = CRUS_WIC.DataContext as CRUSWarehouseItemControlViewModel;

            (CRUS_WCC.DataContext as CRUSWarehouseCategoryControlViewModel)!.Source.SelectedItemChangedTrigger += (x) => {
                WarehouseItemVM!.Source.WarehouseCategory = x;
            };
        }
    }
}
