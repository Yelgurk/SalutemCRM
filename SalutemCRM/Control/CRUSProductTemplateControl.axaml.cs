using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using SalutemCRM.Domain.Model;
using SalutemCRM.Reactive;
using SalutemCRM.Services;
using SalutemCRM.ViewModels;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace SalutemCRM.Control
{
    public partial class CRUSProductTemplateControl : UserControl
    {
        public CRUSProductTemplateControl()
        {
            InitializeComponent();
        }
    }
}
