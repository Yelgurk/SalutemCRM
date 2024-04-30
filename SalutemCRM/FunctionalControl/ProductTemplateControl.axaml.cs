using Avalonia.Controls;
using SalutemCRM.ViewModels;

namespace SalutemCRM.FunctionalControl;


public partial class ProductTemplateControl : UserControl
{
    public CRUSProductTemplateControlViewModel? ProductTemplateVM;
    
    public ProductTemplateControl()
    {
        InitializeComponent();

        this.DataContext = new FunctionalControlUIProvider()
        {
            IsFuncAddNewAvailableSetter = (x) => {
                ProductTemplateVM!.Source.IsFuncAddNewAvailable = x;
                (CRUS_PCC.DataContext as CRUSProductCategoryControlViewModel)!.Source.IsFuncAddNewAvailable = x;
            },
            IsFuncEditAvailableSetter = (x) => {
                ProductTemplateVM!.Source.IsFuncEditAvailable = x;
                (CRUS_PCC.DataContext as CRUSProductCategoryControlViewModel)!.Source.IsFuncEditAvailable = x;
            },
            IsResponsiveControlSetter = (x) => {
                ProductTemplateVM!.Source.IsResponsiveControl = x;
                (CRUS_PCC.DataContext as CRUSProductCategoryControlViewModel)!.Source.IsResponsiveControl = x;
            }
        };

        ProductTemplateVM = CRUS_PTC.DataContext as CRUSProductTemplateControlViewModel;

        (CRUS_PCC.DataContext as CRUSProductCategoryControlViewModel)!.Source.SelectedItemChangedTrigger = (x) => {
            ProductTemplateVM!.Source.ProductCategory = x;
        };
    }
}
