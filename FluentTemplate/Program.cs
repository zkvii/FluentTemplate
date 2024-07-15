using Microsoft.UI.Dispatching;
using Microsoft.Windows.AppLifecycle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Win32;
using Windows.Win32.Foundation;
using FluentTemplate.Helpers;

namespace FluentTemplate;

class Program
{

    private static IntPtr _redirectEventHandle=IntPtr.Zero;
    [STAThread]
    public static async Task Main(string[] args)
    {
        WinRT.ComWrappersSupport.InitializeComWrappers();
        var isRedirect = await DecideRedirection();
        if (!isRedirect)
        {
            Microsoft.UI.Xaml.Application.Start((p) =>
            {
                var context = new DispatcherQueueSynchronizationContext(
                    DispatcherQueue.GetForCurrentThread());
                SynchronizationContext.SetSynchronizationContext(context);
            });
        }
    }

    private static async Task<bool> DecideRedirection()
    {
        bool isRedirect = false;
        AppActivationArguments args = AppInstance.GetCurrent().GetActivatedEventArgs();
        ExtendedActivationKind kind = args.Kind;

        try
        {
            AppInstance keyInstance = AppInstance.FindOrRegisterForKey("randomKey");

            if (keyInstance.IsCurrent)
            {
                keyInstance.Activated += OnActivated;
            }
            else
            {
                isRedirect = true;
                RedirectActivationTo(args, keyInstance);
            }
        }

        catch (Exception ex)
        {

        }

        return isRedirect;
    }
    public static void RedirectActivationTo(
        AppActivationArguments args, AppInstance keyInstance)
    {
        _redirectEventHandle = PInvoke.CreateEvent(IntPtr.Zero, true, false, null);
        Task.Run(() =>
        {
            keyInstance.RedirectActivationToAsync(args).AsTask().Wait();
            PInvoke.SetEvent(_redirectEventHandle);
        });
        uint CWMO_DEFAULT = 0;
        uint INFINITE = 0xFFFFFFFF;
        _ = PInvoke.CoWaitForMultipleObjects(
            CWMO_DEFAULT, INFINITE, 1,
            new IntPtr[] { _redirectEventHandle }, out uint handleIndex);
    }

    private static void OnActivated(object sender, AppActivationArguments args)
    {
        ExtendedActivationKind kind = args.Kind;
    }



}