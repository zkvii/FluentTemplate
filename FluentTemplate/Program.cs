﻿using Microsoft.UI.Dispatching;
using Microsoft.Windows.AppLifecycle;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using FluentTemplate.Utils;
using Microsoft.UI.Xaml;

namespace FluentTemplate;

public static class Program
{
    private static int activationCount = 1;

    public static ManualResetEvent UiThreadInitialized = new(false);
    public static List<string> OutputStack { get; private set; }


    // Replaces the standard App.g.i.cs.
    // Note: We can't declare Main to be async because in a WinUI app
    // this prevents Narrator from reading XAML elements.
    [STAThread]
    static void Main(string[] args)
    {
        SettingsHelper.LoadSettings();
        WinRT.ComWrappersSupport.InitializeComWrappers();

        OutputStack = new();

        bool isRedirect = DecideRedirection();
        if (!isRedirect)
        {
            var uiThread = new Thread(() =>
            {
                Application.Start((p) =>
                {
                    var context = new DispatcherQueueSynchronizationContext(
                        DispatcherQueue.GetForCurrentThread());
                    SynchronizationContext.SetSynchronizationContext(context);
                    new App();
                });
            });
            uiThread.Start();


            // var bgThread = new Thread(() =>
            // {
            //     UiThreadInitialized.WaitOne();
            //     AnimateTrayIcon.StartAnimateIcon();
            // });
            // bgThread.Start();


            uiThread.Join();
            // bgThread.Join();
        }
    }


    #region Report helpers

    public static void ReportInfo(string message)
    {
        // If we already have a form, display the message now.
        // Otherwise, add it to the collection for displaying later.
        if (Application.Current is App thisApp && thisApp.AppWindow != null
                                               && thisApp.AppWindow is MainWindow mainWindow)
        {
            mainWindow.OutputMessage(message);
        }
        else
        {
            OutputStack.Add(message);
        }
    }

    private static void ReportFileArgs(string callSite, AppActivationArguments args)
    {
        ReportInfo($"called from {callSite}");
        if (args.Data is IFileActivatedEventArgs fileArgs)
        {
            IStorageItem item = fileArgs.Files.FirstOrDefault();
            if (item is StorageFile file)
            {
                ReportInfo($"file: {file.Name}");
            }
        }
    }

    private static void ReportLaunchArgs(string callSite, AppActivationArguments args)
    {
        ReportInfo($"called from {callSite}");
        if (args.Data is ILaunchActivatedEventArgs launchArgs)
        {
            string[] argStrings = launchArgs.Arguments.Split();
            for (int i = 0; i < argStrings.Length; i++)
            {
                string argString = argStrings[i];
                if (!string.IsNullOrWhiteSpace(argString))
                {
                    ReportInfo($"arg[{i}] = {argString}");
                }
            }
        }
    }

    private static void OnActivated(object sender, AppActivationArguments args)
    {
        ExtendedActivationKind kind = args.Kind;
        if (kind == ExtendedActivationKind.Launch)
        {
            ReportLaunchArgs($"OnActivated ({activationCount++})", args);
        }
        else if (kind == ExtendedActivationKind.File)
        {
            ReportFileArgs($"OnActivated ({activationCount++})", args);
        }
    }

    public static void GetActivationInfo()
    {
        AppActivationArguments args = AppInstance.GetCurrent().GetActivatedEventArgs();
        ExtendedActivationKind kind = args.Kind;
        ReportInfo($"ActivationKind: {kind}");

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
                        ReportInfo(arg);
                    }
                }
            }
        }
        else if (kind == ExtendedActivationKind.File)
        {
            if (args.Data is IFileActivatedEventArgs fileArgs)
            {
                IStorageItem file = fileArgs.Files.FirstOrDefault();
                if (file != null)
                {
                    ReportInfo(file.Name);
                }
            }
        }
    }

    #endregion


    #region Redirection

    // Decide if we want to redirect the incoming activation to another instance.
    private static bool DecideRedirection()
    {
        bool isRedirect = false;

        // Find out what kind of activation this is.
        AppActivationArguments args = AppInstance.GetCurrent().GetActivatedEventArgs();
        ExtendedActivationKind kind = args.Kind;
        ReportInfo($"ActivationKind={kind}");
        if (kind == ExtendedActivationKind.Launch)
        {
            // This is a launch activation.
            ReportLaunchArgs("Main", args);
        }
        else if (kind == ExtendedActivationKind.File)
        {
            ReportFileArgs("Main", args);

            try
            {
                // This is a file activation: here we'll get the file information,
                // and register the file name as our instance key.
                if (args.Data is IFileActivatedEventArgs fileArgs)
                {
                    IStorageItem file = fileArgs.Files[0];
                    AppInstance keyInstance = AppInstance.FindOrRegisterForKey(file.Name);
                    ReportInfo($"Registered key = {keyInstance.Key}");

                    // If we successfully registered the file name, we must be the
                    // only instance running that was activated for this file.
                    if (keyInstance.IsCurrent)
                    {
                        // Report successful file name key registration.
                        ReportInfo($"IsCurrent=true; registered this instance for {file.Name}");

                        // Hook up the Activated event, to allow for this instance of the app
                        // getting reactivated as a result of multi-instance redirection.
                        keyInstance.Activated += OnActivated;
                    }
                    else
                    {
                        isRedirect = true;
                        RedirectActivationTo(args, keyInstance);
                    }
                }
            }
            catch (Exception ex)
            {
                ReportInfo($"Error getting instance information: {ex.Message}");
            }
        }

        return isRedirect;
    }

    private static IntPtr redirectEventHandle = IntPtr.Zero;

    // Do the redirection on another thread, and use a non-blocking
    // wait method to wait for the redirection to complete.
    public static void RedirectActivationTo(
        AppActivationArguments args, AppInstance keyInstance)
    {
        var redirectSemaphore = new Semaphore(0, 1);
        Task.Run(() =>
        {
            keyInstance.RedirectActivationToAsync(args).AsTask().Wait();
            redirectSemaphore.Release();
        });
        redirectSemaphore.WaitOne();
    }

    #endregion
}