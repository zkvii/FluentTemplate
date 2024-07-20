using FluentTemplate.Helpers;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;
using Vortice.DXGI;
using Vortice.Mathematics;
using Size = Windows.Foundation.Size;

namespace FluentTemplate.ViewModels;

public class FluentTableViewModel
{
    private DispatcherTimer timer;

    private void Timer_Tick(object sender, object e)
    {
        Draw();
    }

    public void Draw()
    {
        DirectXHelper.D2dContext.BeginDraw();

        DirectXHelper.D2dContext.Clear(Colors.AliceBlue);

        //draw a line
        DirectXHelper.D2dContext.DrawLine(new Vector2(0, 0), new Vector2(100, 100), DirectXHelper.D2dbrush, 2);

        DirectXHelper.D2dContext.EndDraw();

        DirectXHelper.SwapChain.Present(1, PresentFlags.None);
    }

    public void InitSwapChain(SwapChainPanel tableSwapChain, Vector2 actualSize)
    {
        timer = new DispatcherTimer();
        timer.Tick += Timer_Tick;
        timer.Interval = TimeSpan.FromMilliseconds(16D);
        DirectXHelper.InitDirectX();
        DirectXHelper.CreateSwapChain(tableSwapChain);
        timer.Start();
    }

    public void ResizeView(Size NewSize)
    {
        DirectXHelper.ResizeSwapChain((int)NewSize.Width,(int)NewSize.Height);
    }
}