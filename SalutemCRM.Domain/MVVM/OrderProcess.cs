using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalutemCRM.Domain.Model;

public partial class OrderProcess
{
    [NotMapped]
    private TimeSpan? _mustBeStartedTimeSpan = null;

    [NotMapped]
    public TimeSpan? MustBeStartedTimeSpan
    {
        get => _mustBeStartedTimeSpan is not null ? _mustBeStartedTimeSpan : (_mustBeStartedTimeSpan = MustBeStartedDT.TimeOfDay);
        set => _mustBeStartedTimeSpan = value;
    }

    [NotMapped]
    private TimeSpan? _deadlineTimeSpan = null;
    
    [NotMapped]
    public TimeSpan? DeadlineTimeSpan
    {
        get => _deadlineTimeSpan is not null ? _deadlineTimeSpan : (_deadlineTimeSpan = DeadlineDT.TimeOfDay);
        set => _deadlineTimeSpan = value;
    }

    [NotMapped]
    public bool IsBuilderMode => Id < 0;

    [NotMapped]
    public bool IsTaskRunning => TaskStatus >= Task_Status.Execution;

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