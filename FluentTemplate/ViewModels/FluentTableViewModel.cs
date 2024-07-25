using FluentTemplate.Helpers;
using Microsoft.UI.Xaml;
using System;
using System.Diagnostics;
using System.Numerics;
using Windows.Foundation;
using Microsoft.UI.Xaml.Controls;
using Vortice.DXGI;
using Vortice.Mathematics;
using Size = Windows.Foundation.Size;
using CommunityToolkit.Mvvm.ComponentModel;
using Vortice.Direct2D1;
using static FluentTemplate.ViewModels.TableScrollBar;
using static FluentTemplate.Helpers.DirectXHelper;
using static FluentTemplate.Helpers.GeometryComputeHelper;
using Rect = Vortice.Mathematics.Rect;

namespace FluentTemplate.ViewModels;

public partial class FluentTableViewModel : ObservableRecipient
{
    private DispatcherTimer _timer;

    [ObservableProperty] private float _totalWidth;

    [ObservableProperty] private float _totalHeight;

    [ObservableProperty] private float _viewBoxWidth;

    [ObservableProperty] private float _viewBoxHeight;


    partial void OnTotalWidthChanged(float value)
    {
        // TotalWidth = value;
        FScrollBar.TotalWidth = value;
    }

    partial void OnTotalHeightChanged(float value)
    {
        FScrollBar.TotalHeight = value;
    }

    partial void OnViewBoxWidthChanged(float value)
    {
        FScrollBar.ViewBoxWidth = value;
    }

    partial void OnViewBoxHeightChanged(float value)
    {
        FScrollBar.ViewBoxHeight = value;
    }

    public TableScrollBar FScrollBar { get; set; } = new();


    private void Timer_Tick(object sender, object e)
    {
        Draw();
    }


    public void Draw()
    {
        D2dContext.BeginDraw();
        D2dContext.Clear(Colors.AliceBlue);
        FScrollBar.DrawScrollBar(D2dContext);

        //sample drawing
        D2dContext.AntialiasMode = AntialiasMode.Aliased;
        var blackBrush=D2dContext.CreateSolidColorBrush(Colors.Black);
        D2dContext.FillRectangle(new Rect(300f,100f,100f,100f),blackBrush);
        D2dContext.AntialiasMode = AntialiasMode.PerPrimitive;
        D2dContext.FillRectangle(new Rect(500f,100f,100f,100f),blackBrush);

        var textFormat=D2DWriteFactory.CreateTextFormat("Arial", 20f);

        var textLayout=D2DWriteFactory.CreateTextLayout("Hello World",textFormat,100f,100f);

        D2dContext.DrawTextLayout(new Vector2(200,200),textLayout,blackBrush);

        D2dContext.EndDraw();
        SwapChain.Present(2, PresentFlags.None);
    }

    public void InitSwapChain(SwapChainPanel tableSwapChain, Vector2 actualSize)
    {
        _timer = new DispatcherTimer();
        _timer.Tick += Timer_Tick;
        _timer.Interval = TimeSpan.FromMilliseconds(10D);
        InitDirectX();
        CreateSwapChain(tableSwapChain);
        _timer.Start();
    }

    public void ResizeView(Size newSize)
    {
        // ViewBoxWidth = (int)newSize.Width;
        // ViewBoxHeight = (int)newSize.Height;
        // TotalHeight = (int)newSize.Height + 100;
        // TotalWidth = (int)newSize.Width;
        ViewBoxWidth = newSize._width;
        ViewBoxHeight = newSize._height;
        TotalHeight = newSize._height + 100;
        TotalWidth = newSize._width;

        ResizeSwapChain((int)newSize.Width, (int)newSize.Height);
    }

    public void OnPointerPressed(Point position)
    {
    }

    public void OnPointerMoved(Point position)
    {
        if (IsPointInRect(position._x, position._y, FScrollBar.ScrollRegionRect))
        {
            FScrollBar.IsScrollBarEntered=true;
        }
        else
        {
            FScrollBar.IsScrollBarEntered=false;
        }
    }

    public void OnPointerReleased(Point position)
    {
    }

    public void OnPointerExited()
    {
    }

    public void OnPointerWheelChanged(int delta)
    {
    }
}