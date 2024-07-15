using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FluentTemplate
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        private Window m_window;
        public Window AppWindow
        {
            get { return m_window; }
            private set { }
        }

        public App()
        {
            InitializeComponent();
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
            Windows.ApplicationModel.Activation.ActivationKind kind
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
            m_window = new MainWindow();
            m_window.Activate();
        }
    }
}
