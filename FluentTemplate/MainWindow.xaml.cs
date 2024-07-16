using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.Windows.AppLifecycle;
using Windows.ApplicationModel.Activation;
using Windows.Graphics;
using Windows.Storage;
using FluentTemplate.Helpers;
using WinUIEx;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FluentTemplate
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Title = "CsWinUiDesktopActivation";
            Closed += MainWindow_Closed;

            Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread().TryEnqueue(
                Microsoft.UI.Dispatching.DispatcherQueuePriority.Low,
                () =>
                {
                    Microsoft.UI.WindowId myWndId =
                        Microsoft.UI.Win32Interop.GetWindowIdFromWindow(WindowHelpers.MHwnd);
                    var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(myWndId);
                    appWindow.Resize(new SizeInt32(500, 800));

            //...
                });
        }

        private void MainWindow_Closed(object sender, WindowEventArgs args)
        {
            args.Handled = true;
            this.Hide();
        }

        public void OutputMessage(string message)
        {
            DispatcherQueue.TryEnqueue(() => { StatusListView.Items.Add(message); });
        }

        private void ActivationInfoButton_Click(object sender, RoutedEventArgs e)
        {
            GetActivationInfo();
        }

        private void GetActivationInfo()
        {
            AppActivationArguments args = AppInstance.GetCurrent().GetActivatedEventArgs();
            ExtendedActivationKind kind = args.Kind;
            OutputMessage($"ActivationKind: {kind}");

            if (kind == ExtendedActivationKind.Launch)
            {
                if (args.Data is ILaunchActivatedEventArgs launchArgs)
                {
                    string argString = launchArgs.Arguments;
                    string[] argStrings = argString.Split();
                    foreach (string arg in argStrings)
                    {
                        if (!string.IsNullOrWhiteSpace(arg))
                        {
                            OutputMessage(arg);
                        }
                    }
                }
            }
            else if (kind == ExtendedActivationKind.File)
            {
                if (args.Data is IFileActivatedEventArgs fileArgs)
                {
                    IStorageItem file = fileArgs.Files.FirstOrDefault();
                    OutputMessage(file.Name);
                }
            }
            else if (kind == ExtendedActivationKind.Protocol)
            {
                if (args.Data is IProtocolActivatedEventArgs protocolArgs)
                {
                    Uri uri = protocolArgs.Uri;
                    OutputMessage(uri.AbsoluteUri);
                }
            }
            else if (kind == ExtendedActivationKind.StartupTask)
            {
                if (args.Data is IStartupTaskActivatedEventArgs startupArgs)
                {
                    OutputMessage(startupArgs.TaskId);
                }
            }
        }
    }
}