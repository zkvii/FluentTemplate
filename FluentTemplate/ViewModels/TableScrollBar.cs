using System.Drawing.Printing;
using Vortice.Direct2D1;
using Vortice.Mathematics;
using static FluentTemplate.Helpers.DirectXHelper;
using static FluentTemplate.ViewModels.FluentTableViewModel;

namespace FluentTemplate.ViewModels;

public class TableScrollBar
{
    public static float ScrollBarRegionWidth = 22;
    public static Margins ScrollBarMargins = new(5, 5, 5, 5);
    public static float ScrollBarHeight;
    public static Rect ScrollRegionRect;
    public static Rect ScrollBarRect;
    public static Rect IndicatorRect;
    public static ID2D1SolidColorBrush ScrollRegionBrush;
    public static ID2D1SolidColorBrush ScrollBarBrush;
    public static ID2D1SolidColorBrush IndicatorBrush;
    public static bool IsScrollBarEntered = false;
    public static bool IsScrollBarPressed = false;
    public static int ScrollBarPressedPosition = 0;
    public static int CurrentPosition = 0;

    public static void InitScrollBarResources()
    {
        ScrollRegionBrush = D2dContext.CreateSolidColorBrush(Colors.Gray);
        ScrollBarBrush = D2dContext.CreateSolidColorBrush(Colors.Black);
        IndicatorBrush = D2dContext.CreateSolidColorBrush(Colors.Blue);
    }

    public static void DrawScrollBar(ID2D1DeviceContext dContext)
    {
        dContext.FillRectangle(ScrollRegionRect, ScrollRegionBrush);

        if (IsScrollBarEntered)
            dContext.DrawRectangle(IndicatorRect, IndicatorBrush, 2);

        dContext.FillRectangle(ScrollBarRect, ScrollBarBrush);
    }

    public static void ComputeScrollRegionRect()
    {
        ScrollRegionRect = new Rect(ViewBoxWidth - ScrollBarRegionWidth, 0, ScrollBarRegionWidth, ViewBoxHeight);
    }

    public static void ComputeScrollBarRect()
    {
        var percent = (double)ViewBoxHeight / TotalHeight;
        ScrollBarHeight = (float)((ViewBoxHeight - ScrollBarMargins.Bottom - ScrollBarMargins.Top) * percent);

        var x = ViewBoxWidth - ScrollBarRegionWidth + ScrollBarMargins.Left;
        var y = CurrentPosition + ScrollBarMargins.Top;
        var barW = ScrollBarRegionWidth - ScrollBarMargins.Right - ScrollBarMargins.Left;
        var barH = ScrollBarHeight;

        ScrollBarRect = new Rect(x, y, barW, barH);
    }

    public static void ComputeIndicatorRect()
    {
        var indicatorHeight = ViewBoxHeight - ScrollBarMargins.Bottom - ScrollBarMargins.Top;

        var x = ViewBoxWidth - ScrollBarRegionWidth + ScrollBarMargins.Left;
        var y = ScrollBarMargins.Top;
        var barW = ScrollBarRegionWidth - ScrollBarMargins.Right - ScrollBarMargins.Left;
        var barH = indicatorHeight;

        IndicatorRect = new Rect(x, y, barW, barH);
    }
}
