using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Timers;
using FluentTemplate.Helpers;
using System.Windows.Forms;
using static FluentTemplate.Helpers.Win32Helpers;

namespace FluentTemplate.Utils;

public class AnimateTrayIcon
{
    private static Timer _iconChangeTimer;
    private static string[] _iconPaths = { "Assets/edit_active_1.ico", "Assets/edit_inactive.ico" };
    private static int _currentIconIndex = 0;
    private static IntPtr _hWnd;

    private static uint _uID = 1;

    // private static ContextMenuStrip _contextMenu;

    public static void StartAnimateIcon()
    {

        // _hWnd = WindowHelpers.MHwnd;
        // SetupTrayIcon(_iconPaths[_currentIconIndex], Win32Helpers.NIM_ADD);
        //
        // _iconChangeTimer = new Timer(1000); // 每隔1000毫秒（1秒）更改图标
        // _iconChangeTimer.Elapsed += OnTimedEvent;
        // _iconChangeTimer.AutoReset = true;
        // _iconChangeTimer.Enabled = true;
        //raw 事件绑定

      
    }


    private static void SetupTrayIcon(string iconPath, uint message)
    {
        Win32Helpers.NOTIFYICONDATA nid = new Win32Helpers.NOTIFYICONDATA();
        nid.cbSize = (uint)Marshal.SizeOf(nid);

        nid.hWnd = _hWnd;
        nid.uID = _uID;
        nid.uFlags = Win32Helpers.NIF_MESSAGE | Win32Helpers.NIF_ICON | Win32Helpers.NIF_TIP;
        nid.uCallbackMessage = WM_TRAYMOUSEMESSAGE;
        nid.hIcon = LoadIconFromFile(iconPath);
        nid.szTip = "Fiend";

        if (!Win32Helpers.Shell_NotifyIcon(message, ref nid))
        {
            Debug.WriteLine($"Failed to {((message == Win32Helpers.NIM_ADD) ? "add" : "modify")} tray icon");
        }
    }

    private static void OnTimedEvent(Object source, ElapsedEventArgs e)
    {
        _currentIconIndex = (_currentIconIndex + 1) % _iconPaths.Length;
        SetupTrayIcon(_iconPaths[_currentIconIndex], Win32Helpers.NIM_MODIFY);
    }

    private static IntPtr LoadIconFromFile(string iconPath)
    {
        return Win32Helpers.LoadImage(IntPtr.Zero,
            iconPath,
            Win32Helpers.IMAGE_ICON,
            0, 0,
            Win32Helpers.LR_LOADFROMFILE);
    }

}