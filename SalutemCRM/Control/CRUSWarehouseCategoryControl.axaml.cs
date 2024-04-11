using Avalonia.Controls;
using SalutemCRM.Domain.Model;
using SalutemCRM.ViewModels;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace SalutemCRM.Control
{
    public partial class CRUSWarehouseCategoryControl : UserControl
    {
        private WarehouseCategory? _reselect = null;

        private bool _isReselected = false;

        public CRUSWarehouseCategoryControl()
        {
            InitializeComponent();

            TreeDataGridControl_1.SelectionChanging += TreeDataGridControlSelectionChanging;
            TreeDataGridControl_2.SelectionChanging += TreeDataGridControlSelectionChanging;
            TreeDataGridControl_3.SelectionChanging += TreeDataGridControlSelectionChanging;
        }

        private async void TreeDataGridControlSelectionChanging(object? s, System.ComponentModel.CancelEventArgs e)
        {
            _isReselected = false;
            _reselect = (s as TreeDataGrid)!.RowSelection!.SelectedItem as WarehouseCategory;
            await GetSelectedFromTreeDataGrid((TreeDataGrid)s!, (CRUSWarehouseCategoryControlViewModel)this.DataContext!);
        }

        private async Task GetSelectedFromTreeDataGrid(TreeDataGrid x, CRUSWarehouseCategoryControlViewModel vm) => await Task.Run(() =>
        {
            while (!_isReselected)
                try
                {
                    while ((x.RowSelection!.SelectedItem as WarehouseCategory)?.Name == _reselect?.Name) ;
                    vm.Source.SelectedItem = _reselect = x.RowSelection!.SelectedItem as WarehouseCategory;
                    _isReselected = true;
                } catch { }
        });
    }
}
