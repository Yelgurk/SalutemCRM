using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalutemCRM.Domain.Model;

public partial class OrderProcess
{
    [NotMapped]
    public TimeSpan? MustBeStartedTimeSpan { get; set; } = null;

    [NotMapped]
    public TimeSpan? DeadlineTimeSpan { get; set; } = null;

    [NotMapped]
    public bool IsBuilderMode => Id < 0;


    /* indexing starts from 1 in VM of control, for UI */
    [NotMapped]
    private int _inQueueCount = 1;

    [NotMapped]
    public int InQueueCount
    {
        get => _inQueueCount;
        set
        {
            _inQueueCount = value;
            OnPropertyChanged(nameof(InQueueCount));
            OnPropertyChanged(nameof(IsLastInQueue));
        }
    }
    
    [NotMapped]
    public bool IsFirstInQueue => this.Queue <= 1;

    [NotMapped]
    public bool IsLastInQueue => this.Queue >= InQueueCount;
}