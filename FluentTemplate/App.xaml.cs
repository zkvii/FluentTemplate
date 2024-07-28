using Microsoft.UI.Xaml;
using Windows.ApplicationModel.Activation;
using FluentTemplate.Helpers;
using System;
using FluentTemplate.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FluentTemplate;

/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public partial class App : Application
{

    public IHost Host
    {
        get;
    }

    public static T GetService<T>()
        where T : class
    {
        if ((App.Current as App)!.Host.Services.GetService(typeof(T)) is not T service)
        {
            throw new ArgumentException($"{typeof(T)} needs to be registered in ConfigureServices within App.xaml.cs.");
        }

        return service;
    }

    private Window _mWindow;
    public Window AppWindow
    {
        get => _mWindow;
        private set => _mWindow = value;
    }

    public App()
    {
        InitializeComponent();
        Host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder().
            UseContentRoot(AppContext.BaseDirectory).
            ConfigureServices((context, services) =>
            {
                services.AddTransient<FluentTableViewModel>();
                services.AddTransient<FluentPlotViewModel>();

            }).Build();

        UnhandledException += App_UnhandledException;

    }

    private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
    {
        throw new NotImplementedException();
    }

    // NOTE: WinUI's App.OnLaunched is given a Microsoft.UI.Xaml.LaunchActivatedEventArgs,
    // where the UWPLaunchActivatedEventArgs property will be one of the 
    // Windows.ApplicationModel.Activation.*ActivatedEventArgs types.
    // Conversely, AppInstance.GetActivatedEventArgs will return a
    // Microsoft.Windows.AppLifecycle.AppActivationArguments, where the Data property
    // will be one of the Windows.ApplicationModel.Activation.*ActivatedEventArgs types.
    protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
    {
        // NOTE: OnLaunched will always report that the ActivationKind == Launch,
        // even when it isn't.
        ActivationKind kind
            = args.UWPLaunchActivatedEventArgs.Kind;
        Program.ReportInfo($"OnLaunched: Kind={kind}");

        // NOTE: AppInstance is ambiguous between
        // Microsoft.Windows.AppLifecycle.AppInstance and
        // Windows.ApplicationModel.AppInstance
        var currentInstance =
            Microsoft.Windows.AppLifecycle.AppInstance.GetCurrent();
        if (currentInstance != null)
        {
            // AppInstance.GetActivatedEventArgs will report the correct ActivationKind,
            // even in WinUI's OnLaunched.
            Microsoft.Windows.AppLifecycle.AppActivationArguments activationArgs
                = currentInstance.GetActivatedEventArgs();
            if (activationArgs != null)
            {
                Microsoft.Windows.AppLifecycle.ExtendedActivationKind extendedKind
                    = activationArgs.Kind;
                Program.ReportInfo($"activationArgs.Kind={extendedKind}");
            }
        }

        // Go ahead and do standard window initialization regardless.
        _mWindow = WindowHelpers.CreateMainWindow();
        _mWindow.Activate();
    }
}