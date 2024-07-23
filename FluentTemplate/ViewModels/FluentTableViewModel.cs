using FluentTemplate.Helpers;
using Microsoft.UI.Xaml;
using System;
using System.Numerics;
using Windows.Foundation;
using Microsoft.UI.Xaml.Controls;
using Vortice.DXGI;
using Vortice.Mathematics;
using Size = Windows.Foundation.Size;
using CommunityToolkit.Mvvm.ComponentModel;
using static FluentTemplate.ViewModels.TableScrollBar;
using static FluentTemplate.Helpers.DirectXHelper;
using static FluentTemplate.Helpers.GeometryComputeHelper;

namespace FluentTemplate.ViewModels;

public partial class FluentTableViewModel : ObservableRecipient
{
    private DispatcherTimer timer;

    public static int TotalWidth;

    public static int TotalHeight;

    public static int ViewBoxWidth;

    public static int ViewBoxHeight;

    [ObservableProperty]
    private static int _scrollBarMovedLength;
    partial void OnScrollBarMovedLengthChanged(int value)
    {
        CurrentPosition = Math.Max(0, Math.Min(TotalHeight - ViewBoxHeight, CurrentPosition + value));
        ComputeScrollBarRect();
    }

    private void Timer_Tick(object sender, object e)
    {
        ResourcesInit();
        Draw();
    }

    private void ResourcesInit()
    {
        InitScrollBarResources();
    }

    public void Draw()
    {
        D2dContext.BeginDraw();
        D2dContext.Clear(Colors.AliceBlue);
        DrawScrollBar(D2dContext);
        D2dContext.EndDraw();
        SwapChain.Present(1, PresentFlags.None);
    }

    public void InitSwapChain(SwapChainPanel tableSwapChain, Vector2 actualSize)
    {
        timer = new DispatcherTimer();
        timer.Tick += Timer_Tick;
        timer.Interval = TimeSpan.FromMilliseconds(10D);
        InitDirectX();
        CreateSwapChain(tableSwapChain);
        timer.Start();
    }

    public void ResizeView(Size newSize)
    {
        ViewBoxWidth = (int)newSize.Width;
        ViewBoxHeight = (int)newSize.Height;
        TotalHeight = (int)newSize.Height + 100;
        TotalWidth = (int)newSize.Width;
        ResizeSwapChain((int)newSize.Width, (int)newSize.Height);
        ComputeScrollRegionRect();
        ComputeScrollBarRect();
        ComputeIndicatorRect();
    }

    public void OnPointerPressed(Point position)
    {
        if (IsScrollBarEntered)
        {
            ScrollBarPressedPosition = (int)position.Y;
            IsScrollBarPressed = true;
        }
        else
        {
            IsScrollBarPressed = false;
        }
    }

    public void OnPointerMoved(Point position)
    {
        IsScrollBarEntered = IsPointInRect((float)position.X, (float)position.Y, ScrollRegionRect);

        if (IsScrollBarPressed)
        {
            ScrollBarMovedLength = (int)position.Y - ScrollBarPressedPosition;
            ScrollBarPressedPosition = (int)position.Y; // Update the pressed position to the current position
        }
    }

    public void OnPointerReleased(Point position)
    {
        if (IsScrollBarPressed)
        {
            IsScrollBarPressed = false;
            ScrollBarMovedLength = 0;
            ScrollBarPressedPosition = 0;
        }
    }

    public void OnPointerExited()
    {
        IsScrollBarEntered = false;
        IsScrollBarPressed = false;

    }
}
