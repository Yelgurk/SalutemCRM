using SalutemCRM.Reactive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalutemCRM.Services;

public class ViewModelSourceNotifyService
{
    private List<IReactiveControlSource> ViewModelSources { get; } = new();

    public void AddVMSource(IReactiveControlSource VMSource) => ViewModelSources.Add(VMSource);

    public void Execute(VMSNServiceDelegate Run) => this.ViewModelSources.DoForEach(x => Run(x));


    public delegate void VMSNServiceDelegate(IReactiveControlSource VMSource);
}
