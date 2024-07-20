using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using FluentTemplate.Helpers;
using FluentTemplate.ViewModels;
using Microsoft.UI;
using Vortice.Direct2D1;
using Vortice.Direct3D11;
using Vortice.DXGI;
using WinUIEx;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FluentTemplate.Views
{
    public sealed partial class FluentTableView
    {
        private FluentTableViewModel viewModel;

        public FluentTableView()
        {
            InitializeComponent();
            
            viewModel=new FluentTableViewModel();
            
            viewModel.InitSwapChain(TableSwapChain,ActualSize);
            SizeChanged += FluentTableView_SizeChanged;

            TableSwapChain.PointerPressed += TableSwapChain_PointerPressed;

        }

        private void TableSwapChain_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            // right tapped
            if (!e.GetCurrentPoint(TableSwapChain).Properties.IsRightButtonPressed) return;
            var position=e.GetCurrentPoint(TableSwapChain).Position;
            var flyoutShowOptions = new FlyoutShowOptions()
            {
                Position = position,
                ShowMode = FlyoutShowMode.TransientWithDismissOnPointerMoveAway
            };
            RightClickMenu.ShowAt(TableSwapChain, flyoutShowOptions);
        }

        private void FluentTableView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // presenter resize
            TableSwapChain.Width= e.NewSize.Width;
            TableSwapChain.Height = e.NewSize.Height;
            // swapchain resize
            viewModel.ResizeView(e.NewSize);
        }
    }
}