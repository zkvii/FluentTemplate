using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentTemplate.Helpers;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FluentTemplate.Views;

[ObservableObject]
public sealed partial class TrayIconView
{
    [ObservableProperty] private bool _isWindowVisible = false;

    public TrayIconView()
    {
        InitializeComponent();
    }

    [RelayCommand]
    public void ShowHideWindow()
    {
        if (IsWindowVisible)
        {
            WindowHelpers.HideWindow();
            IsWindowVisible = false;
        }
        else
        {
            WindowHelpers.ShowWindow();
            IsWindowVisible = true;
        }
    }


    [RelayCommand]
    public void ExitApplication()
    {
        // TrayIconB.Dispose();
        // WindowHelpers.CloseWindow();
        Environment.Exit(0);
    }
}