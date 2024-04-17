using Avalonia.Controls;
using SalutemCRM.Domain.Model;
using SalutemCRM.ViewModels;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace SalutemCRM.Control
{
    public partial class CRUSProductCategoryControl : UserControl
    {
        private ProductCategory? _reselect = null;

        private bool _isReselected = false;

        public CRUSProductCategoryControl()
        {
            InitializeComponent();

            TreeDataGridControl_1.SelectionChanging += TreeDataGridControlSelectionChanging;
            TreeDataGridControl_2.SelectionChanging += TreeDataGridControlSelectionChanging;
            TreeDataGridControl_3.SelectionChanging += TreeDataGridControlSelectionChanging;
        }

        private async void TreeDataGridControlSelectionChanging(object? s, System.ComponentModel.CancelEventArgs e)
        {
            _isReselected = false;
            _reselect = (s as TreeDataGrid)!.RowSelection!.SelectedItem as ProductCategory;
            await GetSelectedFromTreeDataGrid((TreeDataGrid)s!, (CRUSProductCategoryControlViewModel)this.DataContext!);
        }

        private async Task GetSelectedFromTreeDataGrid(TreeDataGrid x, CRUSProductCategoryControlViewModel vm) => await Task.Run(() =>
        {
            while (!_isReselected)
                try
                {
                    while ((x.RowSelection!.SelectedItem as ProductCategory)?.Name == _reselect?.Name) ;
                    vm.Source.SelectedItem = _reselect = x.RowSelection!.SelectedItem as ProductCategory;
                    _isReselected = true;
                } catch { }
        });
    }
}
