using Avalonia.Controls;
using SalutemCRM.ViewModels;

namespace SalutemCRM.FunctionalControl
{
    public partial class ProductTemplateControl : UserControl
    {
        public CRUSProductTemplateControlViewModel? ProductTemplateVM;

        public ProductTemplateControl()
        {
            InitializeComponent();

            ProductTemplateVM = CRUS_PTC.DataContext as CRUSProductTemplateControlViewModel;

            (CRUS_PCC.DataContext as CRUSProductCategoryControlViewModel)!.Source.SelectedItemChangedTrigger = (x) => {
                ProductTemplateVM!.Source.ProductCategory = x;
            };
        }
    }
}
