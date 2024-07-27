using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using Vortice.Direct2D1;
using Vortice.DirectWrite;
using Vortice.DXGI;
using Vortice.Mathematics;
using static FluentTemplate.Helpers.DirectXHelper;
using static FluentTemplate.Helpers.GeometryComputeHelper;
using Rect = Vortice.Mathematics.Rect;
using Size = Windows.Foundation.Size;

namespace FluentTemplate.ViewModels;

public partial class FluentTableViewModel:ObservableRecipient
{
    private DispatcherTimer _timer;

    [ObservableProperty] private float _totalWidth;

    [ObservableProperty] private float _totalHeight;

    [ObservableProperty] private float _viewBoxWidth;

    [ObservableProperty] private float _viewBoxHeight;


    partial void OnTotalWidthChanged(float value)
    {
        // TotalWidth = value;
    }

    partial void OnTotalHeightChanged(float value)
    {
    }

    partial void OnViewBoxWidthChanged(float value)
    {
    }

    partial void OnViewBoxHeightChanged(float value)
    {
    }

    public TableScrollBar FScrollBar { get; set; } = new();


    private void Timer_Tick(object sender, object e)
    {
        Draw();
    }


    public void Draw()
    {
        D2dContext.BeginDraw();
        D2dContext.Clear(Colors.Transparent);
        FScrollBar.DrawScrollBar(D2dContext);

        //sample drawing
        D2dContext.AntialiasMode = AntialiasMode.Aliased;
        var blackBrush = D2dContext.CreateSolidColorBrush(Colors.Black);
        D2dContext.FillRectangle(new Rect(300f, 100f, 100f, 100f), blackBrush);
        D2dContext.AntialiasMode = AntialiasMode.PerPrimitive;
        D2dContext.FillRectangle(new Rect(500f, 100f, 100f, 100f), blackBrush);

        var textFormat = D2DWriteFactory.CreateTextFormat("Arial", 25f);

        var textLayout = D2DWriteFactory.CreateTextLayout("Sample Text", textFormat, 100f, 100f);

        D2dContext.DrawTextLayout(new Vector2(200, 200), textLayout, blackBrush);

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
        TotalHeight = newSize._height;
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
            FScrollBar.IsScrollBarEntered = true;
        }
        else
        {
            FScrollBar.IsScrollBarEntered = false;
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