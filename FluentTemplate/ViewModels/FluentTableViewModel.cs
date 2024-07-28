using System;
using System.Numerics;
using Windows.Foundation;
using CommunityToolkit.Mvvm.ComponentModel;
using FluentTemplate.Views;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using Vortice.Direct2D1;
using Vortice.DXGI;
using Vortice.Mathematics;
using static FluentTemplate.Helpers.DirectXHelper;
using static FluentTemplate.Helpers.GeometryComputeHelper;
using Rect = Vortice.Mathematics.Rect;
using Size = Windows.Foundation.Size;

namespace FluentTemplate.ViewModels;

public partial class FluentTableViewModel : ObservableRecipient
{
    private DispatcherTimer _timer;

    // [ObservableProperty] private float _totalWidth;

    // [ObservableProperty] private float _totalHeight;

    [ObservableProperty] private float _viewBoxWidth;

    [ObservableProperty] private float _viewBoxHeight;

    //
    // partial void OnTotalWidthChanged(float value)
    // {
    //     // TotalWidth = value;
    // }
    //
    // partial void OnTotalHeightChanged(float value)
    // {
    // }

    public ID2D1Bitmap D2dTargetBitmapExample { get; set; }

    partial void OnViewBoxWidthChanged(float value)
    {
    }

    partial void OnViewBoxHeightChanged(float value)
    {
    }


    private void Timer_Tick(object sender, object e)
    {
        Draw();
    }

    public ID2D1CommandList commandlist;

    public void LoadResources()
    {
        D2dContext.Target = D2dTargetBitmap1;
        commandlist = D2dContext.CreateCommandList();

        D2dContext.Target = commandlist;
        D2dContext.BeginDraw();
        // D2dContext.Clear(Colors.AliceBlue);

        var blackBrush = D2dContext.CreateSolidColorBrush(Colors.Black);
        var textFormat = D2DWriteFactory.CreateTextFormat("Arial", 25f / FluentTableView.ScaleX);
        D2dContext.DrawTextLayout(new Vector2(200f, 100f), D2DWriteFactory.CreateTextLayout("Sample Text", textFormat, 300f, 100f), blackBrush);

        D2dContext.EndDraw();
        commandlist.Close();
    }

    public void Draw()
    {


        // RenderTargetView.BeginDraw();
        // RenderTargetView.Clear(Colors.AliceBlue);

        //
      
        // D2dContext.Target = commandlist;
        // D2dContext.BeginDraw();
        //
        // var blackBrush = RenderTargetView.CreateSolidColorBrush(Colors.Black);
        // var textFormat = D2DWriteFactory.CreateTextFormat("Arial", 25f / FluentTableView.ScaleX);
        //
        // var textLayout = D2DWriteFactory.CreateTextLayout("Sample Text", textFormat, 300f, 100f);
        // D2dContext.DrawTextLayout(new Vector2(200f, 100f), textLayout, blackBrush);
        //
        // D2dContext.EndDraw();
        // commandlist.Close();

        D2dContext.Target= D2dTargetBitmap1;
        
        D2dContext.BeginDraw();
        D2dContext.Clear(Colors.AliceBlue);
        D2dContext.DrawImage(commandlist);
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
        LoadResources();
        _timer.Start();
    }

    public void ResizeView(Size newSize)
    {
        ViewBoxWidth = newSize._width;
        ViewBoxHeight = newSize._height;

        ResizeSwapChain((int)newSize.Width, (int)newSize.Height);
    }

    public void OnPointerPressed(Point position)
    {
    }

    public void OnPointerMoved(Point position)
    {
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