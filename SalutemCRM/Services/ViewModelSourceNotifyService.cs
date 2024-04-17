using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalutemCRM.Services;

public class ViewModelSourceNotifyService
{
    private List<object> ViewModelSources { get; } = new();

    public void AddVMSource(object VMSource) => ViewModelSources.Add(VMSource);

    public void Execute(VMSNServiceDelegate Run) => this.ViewModelSources.DoForEach(x => Run(x));


    public delegate void VMSNServiceDelegate(dynamic VMSource);
}
