using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentTemplate.Helpers;
using Microsoft.UI.Xaml;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using H.NotifyIcon.Core;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FluentTemplate.Views;

[ObservableObject]
public sealed partial class TrayIconView
{
    [ObservableProperty] private bool _isWindowVisible = false;

    // public static Guid TrayIconId { get; } = Guid.NewGuid();
    // public static IntPtr TrayWindowHandle = IntPtr.Zero;
    private DispatcherTimer timer;
    private static string[] _iconPaths = { "Assets/edit_active_1.ico", "Assets/edit_inactive.ico" };
    private static int _currentIconIndex = 0;
    private static IntPtr _hWnd;

    public TrayIconView()
    {
        InitializeComponent();
    
        timer=new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1)
        };
        timer.Tick += Timer_Tick;
        Loaded += TrayIconView_Loaded;
    }
    private void Timer_Tick(object sender, object e)
    { 
        ChangeIcon();
    }

    private void ChangeIcon()
    {
     
        using var iconStream = new FileStream(_iconPaths[(_currentIconIndex++)%2], FileMode.Open, FileAccess.Read);
       
        using var icon = new Icon(iconStream);

        TrayIconB.UpdateIcon(icon);
        // TrayIconB.UpdateIcon(icon);
    }

    private void TrayIconView_Loaded(object sender, RoutedEventArgs e)
    {
        timer.Start();
    }

    [RelayCommand]
    public void ShowHideWindow()
    {
        Debug.WriteLine(TrayIconB.Id);

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