using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Input;
using FluentTemplate.ViewModels;

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

            PointerPressed += FluentTableView_PointerPressed;
            PointerMoved += FluentTableView_PointerMoved;
            PointerReleased+=FluentTableView_PointerReleased;
            PointerExited += FluentTableView_PointerExited;

        }

        private void FluentTableView_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            var position = e.GetCurrentPoint(TableSwapChain).Position;
            viewModel.OnPointerReleased(position);
        }

        private void FluentTableView_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            viewModel.OnPointerExited();
        }

        private void FluentTableView_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            var position = e.GetCurrentPoint(TableSwapChain).Position;
            viewModel.OnPointerMoved(position);
        }

        private void FluentTableView_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            var position = e.GetCurrentPoint(TableSwapChain).Position;
            viewModel.OnPointerPressed(position);
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