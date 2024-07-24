using System.Drawing.Printing;
using CommunityToolkit.Mvvm.ComponentModel;
using Vortice.Direct2D1;
using Vortice.Mathematics;
using static FluentTemplate.Helpers.DirectXHelper;

namespace FluentTemplate.ViewModels;

public enum ScrollBarOrientation
{
    Horizontal,
    Vertical
}

public partial class TableScrollBar : ObservableRecipient
{
    [ObservableProperty] private float _scrollBarRegionWidth = 20;
    [ObservableProperty] private float _scrollBarRegionHeight;

    [ObservableProperty] private Margins _scrollBarMargins = new(5, 5, 5, 5);

    [ObservableProperty] private float _viewBoxHeight;
    [ObservableProperty] private float _viewBoxWidth;

    [ObservableProperty] private float _totalHeight;
    [ObservableProperty] private float _totalWidth;

    [ObservableProperty] private ScrollBarOrientation _orientation = ScrollBarOrientation.Vertical;

    //scroll region position
    [ObservableProperty] private float _scrollPositionX;
    [ObservableProperty] private float _scrollPositionY=0;


    partial void OnScrollBarRegionHeightChanged(float value)
    {
        // ScrollBarRegionHeight = value;
        UpdateComputeProperty();
        UpdateAllRect();
    }

    partial void OnTotalWidthChanged(float value)
    {
        // TotalWidth = value;
        UpdateComputeProperty();
        UpdateAllRect();
    }

    partial void OnOrientationChanged(ScrollBarOrientation value)
    {
        // Orientation = value;
        UpdateComputeProperty();
        UpdateAllRect();
    }


    partial void OnViewBoxWidthChanged(float value)
    {
        // ViewBoxWidth = value;
        UpdateComputeProperty();
        UpdateAllRect();
    }

    partial void OnScrollBarRegionWidthChanged(float value)
    {
        // ScrollBarRegionWidth = value;
        UpdateComputeProperty();
        UpdateAllRect();
    }

    partial void OnScrollBarMarginsChanged(Margins value)
    {
        // ScrollBarMargins = value;
        UpdateComputeProperty();
        UpdateAllRect();
    }

    partial void OnViewBoxHeightChanged(float value)
    {
        // ViewBoxHeight = value;
        UpdateComputeProperty();
        UpdateAllRect();
    }

    partial void OnTotalHeightChanged(float value)
    {
        // TotalHeight = value;
        UpdateComputeProperty();
        UpdateAllRect();
    }


    public float ScrollBarWidth;
    public float ScrollBarHeight;

    public float ScrollBarMinStart;
    public float ScrollBarMaxStart;

    public float CurrentPosition;
    public double ViewHoldingPercent;

    //region
    public Rect ScrollRegionRect;
    public Rect ScrollBarRect;
    // public Rect IndicatorRect;

    public ID2D1SolidColorBrush ScrollRegionBrush;
    public ID2D1SolidColorBrush ScrollBarBrush;
    public ID2D1SolidColorBrush IndicatorBrush;

    public bool IsScrollBarEntered = false;
    public bool IsScrollBarPressed = false;
    public int ScrollBarPressedPosition = 0;


    // public void InitTableScrollBar(float viewBoxHeight, float viewBoxWidth, float totalWidth, float totalHeight)
    // {
    //     _viewBoxWidth = viewBoxWidth;
    //     _viewBoxHeight = viewBoxHeight;
    //     _totalWidth = totalWidth;
    //     _totalHeight = totalHeight;
    //
    //     //hard property
    //     _scrollBarRegionHeight = ViewBoxHeight;
    //
    //     _scrollPositionX = ViewBoxWidth - ScrollBarRegionWidth;
    //     _scrollPositionY = 0;
    //
    //     //default
    // }


    public void LoadDrawResources()
    {
        if (ScrollBarBrush == null)
        {
            ScrollRegionBrush = D2dContext.CreateSolidColorBrush(Colors.Gray);
        }

        if (ScrollBarBrush == null)
        {
            ScrollBarBrush = D2dContext.CreateSolidColorBrush(Colors.Black);
        }

        if (IndicatorBrush == null)
        {
            IndicatorBrush = D2dContext.CreateSolidColorBrush(Colors.Blue);
        }
    }


    //simple draw style
    public void DrawScrollBar(ID2D1DeviceContext dContext)
    {
        LoadDrawResources();

        //add bound rect

        dContext.FillRectangle(ScrollRegionRect, ScrollRegionBrush);

        if (IsScrollBarEntered)
            dContext.DrawRectangle(ScrollRegionRect, ScrollBarBrush, 1f);

        dContext.FillRectangle(ScrollBarRect, ScrollBarBrush);
    }

    public void UpdateComputeProperty()
    {
        if (Orientation == ScrollBarOrientation.Horizontal)
            UpdateHorizontalComputeProperty();
        else
            UpdateVerticalComputeProperty();
    }

    public void UpdateHorizontalComputeProperty()
    {
    }

    //vertical property update
    public void UpdateVerticalComputeProperty()
    {
        ScrollPositionX=ViewBoxWidth-ScrollBarRegionWidth;
        ScrollBarRegionHeight=ViewBoxHeight;

        ScrollBarWidth = ScrollBarRegionWidth - ScrollBarMargins.Left - ScrollBarMargins.Right;

        ViewHoldingPercent = (double)ViewBoxHeight / TotalHeight;

        ScrollBarHeight =
            (float)((ScrollBarRegionHeight - ScrollBarMargins.Bottom - ScrollBarMargins.Top) * ViewHoldingPercent);


        ScrollBarMinStart = ScrollPositionY + ScrollBarMargins.Top;
        ScrollBarMaxStart = ScrollPositionY + ScrollBarRegionHeight - ScrollBarHeight - ScrollBarMargins.Bottom -
                            ScrollBarMargins.Top;
    }

    //rect update
    public void UpdateAllRect()
    {
        if (Orientation == ScrollBarOrientation.Horizontal)
            RectsUpdateHorizontal();
        else
            RectsUpdateVertical();
    }

    private void RectsUpdateHorizontal()
    {
    }

    public void RectsUpdateVertical()
    {
        ScrollRegionRect = new Rect(ScrollPositionX,
            ScrollPositionY,
            ScrollBarRegionWidth,
            ScrollBarRegionHeight);


        var barX = ScrollPositionX + ScrollBarMargins.Left;
        var barY = ScrollPositionY + CurrentPosition + ScrollBarMargins.Top;
        var barW = ScrollBarRegionWidth - ScrollBarMargins.Right - ScrollBarMargins.Left;
        var barH = ScrollBarHeight;

        ScrollBarRect = new Rect(barX, barY, barW, barH);


        // var indicatorHeight = ViewBoxHeight - ScrollBarMargins.Bottom - ScrollBarMargins.Top;
        //
        // var indX = ViewBoxWidth - ScrollBarRegionWidth + ScrollBarMargins.Left;
        // var indY = ScrollBarMargins.Top;
        // var indW = ScrollBarRegionWidth - ScrollBarMargins.Right - ScrollBarMargins.Left;
        // var indH = indicatorHeight;
        //
        // IndicatorRect = new Rect(indX, indY, indW, indH);
    }
}