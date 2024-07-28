using System;
using Windows.Foundation;
using FluentTemplate.Helpers;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Input;
using FluentTemplate.ViewModels;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System.Diagnostics;
using WinRT.Interop;
using static System.Double;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FluentTemplate.Views;

public sealed partial class FluentPlotView
{
    private FluentPlotViewModel _viewModel;

    public static float ScaleX = 1.0f;
    public static float ScaleY = 1.0f;

    public static float ZoomFactor=1.0f;

    public static float YOffset=0.0f;

    public FluentPlotView()
    {
        _viewModel = App.GetService<FluentPlotViewModel>();

        InitializeComponent();

        InitView();
    }

    private void InitView()
    {
        //this ensured ui created init
        // Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread().TryEnqueue(
        //     Microsoft.UI.Dispatching.DispatcherQueuePriority.Low,
        //     () =>
        //     {



        _viewModel.InitSwapChain(TableSwapChain, ActualSize);
        // SizeChanged += FluentTableView_SizeChanged;
        // SizeChanged += FluentTableView_SizeChanged;


        TableSwapChain.PointerPressed += TableSwapChain_PointerPressed;

        PointerPressed += FluentTableView_PointerPressed;
        PointerMoved += FluentTableView_PointerMoved;
        PointerReleased += FluentTableView_PointerReleased;
        PointerExited += FluentTableView_PointerExited;

        PointerWheelChanged += FluentTableView_PointerWheelChanged;

        SwapScrollViewer.PointerWheelChanged += SwapScrollViewer_PointerWheelChanged;
        SwapScrollViewer.ViewChanged += SwapScrollViewer_ViewChanged;

        SizeChanged += SwapChainContainer_SizeChanged;
        TableSwapChain.CompositionScaleChanged += TableSwapChain_CompositionScaleChanged;
    }

    private void SwapScrollViewer_PointerWheelChanged(object sender, PointerRoutedEventArgs e)
    {
        UpdateInfinite();
    }

    private void UpdateInfinite()
    {
        var currentBottomPosition = SwapScrollViewer.VerticalOffset + SwapScrollViewer.ViewportHeight;
        if (Math.Abs(currentBottomPosition - VirtualContainer.ActualSize.Y) != 0) return;
        if (IsNaN(VirtualContainer.Height))
            VirtualContainer.Height = VirtualContainer.ActualSize.Y + 100;
        else
            VirtualContainer.Height += 100;
    }


    private void SwapScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
    {
        UpdateInfinite();
        ZoomFactor=SwapScrollViewer.ZoomFactor;
        YOffset=(float)SwapScrollViewer.VerticalOffset;
        Debug.WriteLine($"ZoomFactor:{ZoomFactor}");
        Debug.WriteLine($"YOffset:{YOffset}");
    }


    private void SwapChainContainer_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        Debug.WriteLine($"Scroll size:{SwapScrollViewer.ActualSize}");
        Debug.WriteLine($"swapchain size{TableSwapChain.ActualSize}");
        Debug.WriteLine($"virtual size{VirtualContainer.ActualSize}");


        if (Math.Abs(ScaleX - 1.0) < 0.1)
        {
            TableSwapChain.Width = e.NewSize.Width * TableSwapChain.CompositionScaleX;
            TableSwapChain.Height = e.NewSize.Height * TableSwapChain.CompositionScaleY;
        }
        else
        {
            TableSwapChain.Width = e.NewSize.Width / ScaleX;
            TableSwapChain.Height = e.NewSize.Height / ScaleY;
        }

        // swapchain resize

        var newSize = new Size((int)TableSwapChain.Width, (int)TableSwapChain.Height);
        _viewModel.ResizeView(newSize);
    }

    private void TableSwapChain_CompositionScaleChanged(SwapChainPanel sender, object args)
    {
        var nDpi = Win32Helpers.GetDpiForWindow(WindowHelpers.MHwnd);
        ScaleX = 96.0f / nDpi;
        ScaleY = 96.0f / nDpi;
        Debug.WriteLine($"Scale:{ScaleX}");
        TableSwapChain.RenderTransform = new ScaleTransform() { ScaleX = ScaleX, ScaleY = ScaleY };
    }

    private void FluentTableView_PointerWheelChanged(object sender, PointerRoutedEventArgs e)
    {
        var delta = e.GetCurrentPoint(TableSwapChain).Properties.MouseWheelDelta;
        _viewModel.OnPointerWheelChanged(delta);
    }

    private void FluentTableView_PointerReleased(object sender, PointerRoutedEventArgs e)
    {
        var position = e.GetCurrentPoint(TableSwapChain).Position;
        _viewModel.OnPointerReleased(position);
    }

    private void FluentTableView_PointerExited(object sender, PointerRoutedEventArgs e)
    {
        _viewModel.OnPointerExited();
    }

    private void FluentTableView_PointerMoved(object sender, PointerRoutedEventArgs e)
    {
        var position = e.GetCurrentPoint(TableSwapChain).Position;
        _viewModel.OnPointerMoved(position);
    }

    private void FluentTableView_PointerPressed(object sender, PointerRoutedEventArgs e)
    {
        var position = e.GetCurrentPoint(TableSwapChain).Position;
        _viewModel.OnPointerPressed(position);
    }

    private void TableSwapChain_PointerPressed(object sender, PointerRoutedEventArgs e)
    {
        // right tapped
        if (!e.GetCurrentPoint(TableSwapChain).Properties.IsRightButtonPressed) return;
        var position = e.GetCurrentPoint(TableSwapChain).Position;
        var flyoutShowOptions = new FlyoutShowOptions()
        {
            Position = position,
            ShowMode = FlyoutShowMode.TransientWithDismissOnPointerMoveAway
        };
        RightClickMenu.ShowAt(TableSwapChain, flyoutShowOptions);
    }
}