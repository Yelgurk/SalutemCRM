﻿using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalutemCRM.Domain.Model;

public partial class CurrencyUnit : ClonableObservableObject<CurrencyUnit>
{
    [NotMapped]
    [ObservableProperty]
    private string _name = null!;
}