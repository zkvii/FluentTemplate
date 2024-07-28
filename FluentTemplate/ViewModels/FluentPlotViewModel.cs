using System;
using System.Collections.Generic;
using System.Numerics;
using Windows.Foundation;
using CommunityToolkit.Mvvm.ComponentModel;
using FluentTemplate.D2DElements;
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

public partial class FluentPlotViewModel : ObservableRecipient
{
    private DispatcherTimer _timer;


    [ObservableProperty] private float _viewBoxWidth;

    [ObservableProperty] private float _viewBoxHeight;

    [ObservableProperty] private float _rowHeight=20;

    [ObservableProperty] private float _columnWidth=48;

    partial void OnRowHeightChanged(float value)
    {
    }
    partial void OnColumnWidthChanged(float value)
    {
    }


    partial void OnViewBoxWidthChanged(float value)
    {
        LoadResources();
    }

    partial void OnViewBoxHeightChanged(float value)
    {
    }


    private void Timer_Tick(object sender, object e)
    {
        Draw();
    }

    public List<ID2D1CommandList> ColumCommandLists=new();

    public void LoadResources()
    {
        ColumCommandLists.Clear();
        for(var x = 20f; x <ViewBoxWidth; x+=ColumnWidth)
        {
            var column = new D2DColumn(x,20f, ColumnWidth, ViewBoxHeight);
            ColumCommandLists.Add(column.CommandList);
        }
        // D2dContext.Target = D2dTargetBitmap1;
        // CommandList = D2dContext.CreateCommandList();
        //
        // D2dContext.Target = CommandList;
        // D2dContext.BeginDraw();
        // // D2dContext.Clear(Colors.AliceBlue);
        //
        //
        // var blackBrush = D2dContext.CreateSolidColorBrush(Colors.Black);
        // var textFormat = D2DWriteFactory.CreateTextFormat("Arial", 25f / FluentPlotView.ScaleX);
        // D2dContext.DrawTextLayout(new Vector2(200f, 100f),
        //     D2DWriteFactory.CreateTextLayout("Sample Text", textFormat, 300f, 100f), blackBrush);
        //
        // D2dContext.EndDraw();
        // CommandList.Close();
    }

    public void Draw()
    {
        D2dContext.Target = D2dTargetBitmap1;

        D2dContext.BeginDraw();
        D2dContext.Clear(Colors.AliceBlue);
        var zoomMatrix= Matrix3x2.CreateScale((1 / FluentPlotView.ScaleX) * FluentPlotView.ZoomFactor);
        var offsetMatrix = Matrix3x2.CreateTranslation(0, -FluentPlotView.YOffset);
       
        D2dContext.Transform= zoomMatrix * offsetMatrix;
        foreach (var columCommandList in ColumCommandLists)
        {
            D2dContext.DrawImage(columCommandList);
        }
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