using Avalonia.Controls;
using SalutemCRM.ViewModels;

namespace SalutemCRM.FunctionalControl
{
    public partial class ProductTemplateBuilder : UserControl
    {
        public ProductSchemaBuilderViewModel? ProductSchemaVM;

        public ProductTemplateBuilder()
        {
            InitializeComponent();

            ProductSchemaVM = (this.DataContext as ProductSchemaBuilderViewModel)!;

            //(this.FC_WHG.CRUS_WCC.DataContext as dynamic)!.Source.IsResponsiveControl = true;
            (this.FC_WHG.CRUS_WIC.DataContext as dynamic)!.Source.IsResponsiveControl = true;
            (this.FC_PTC.CRUS_PTC.DataContext as dynamic)!.Source.IsResponsiveControl = true;
            //(this.FC_PTC.CRUS_PCC.DataContext as dynamic)!.Source.IsResponsiveControl = true;

            this.FC_WHG.WarehouseItemVM!.Source.SelectedItemChangedTrigger = (x) => {
                ProductSchemaVM.Source.WarehouseItem = x?.Clone() ?? null;
            };

            this.FC_PTC.ProductTemplateVM!.Source.SelectedItemChangedTrigger = (x) => {
                ProductSchemaVM.Source.ProductTemplate = x?.Clone() ?? null;
            };
        }
    }
}
