using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace MobileRequestsService.ViewModels.Base;

public partial class BaseVM : ObservableObject
{
    [ObservableProperty] private bool _isLoading;

    [RelayCommand]
    protected virtual Task DisappearingView() { return Task.CompletedTask; }

    [RelayCommand]
    protected virtual Task AppearingView() { return Task.CompletedTask; }
}
