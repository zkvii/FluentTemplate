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

    public static float XOffset=0.0f;

    public FluentPlotView()
    {
        _viewModel = App.GetService<FluentPlotViewModel>();

        InitializeComponent();

        InitView();
    }

    private void InitView()
    {



        _viewModel.InitSwapChain(TableSwapChain, ActualSize);


        TableSwapChain.PointerPressed += TableSwapChain_PointerPressed;

        PointerPressed += FluentTableView_PointerPressed;
        PointerMoved += FluentTableView_PointerMoved;
        PointerReleased += FluentTableView_PointerReleased;
        PointerExited += FluentTableView_PointerExited;

        PointerWheelChanged += FluentTableView_PointerWheelChanged;



        TableSwapChain.CompositionScaleChanged += TableSwapChain_CompositionScaleChanged;
    }

    private void SwapScrollViewer_PointerWheelChanged(object sender, PointerRoutedEventArgs e)
    {
        UpdateInfinite();
    }

    private void UpdateInfinite()
    {
 
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