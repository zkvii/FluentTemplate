using System.Diagnostics;
using CommunityToolkit.WinUI.Helpers;
using Microsoft.UI.Xaml.Controls;
using SharpGen.Runtime;
using Vortice.DCommon;
using Vortice.Direct2D1;
using Vortice.Direct3D;
using Vortice.Direct3D11;
using Vortice.DirectWrite;
using Vortice.DXGI;
using Vortice.Mathematics;
using AlphaMode = Vortice.DXGI.AlphaMode;
using FactoryType = Vortice.Direct2D1.FactoryType;
using FeatureLevel = Vortice.Direct3D.FeatureLevel;

namespace FluentTemplate.Helpers;

public static class DirectXHelper
{
    // private ID3D11DeviceContext deviceContext;
    public static IDXGIDevice DxgiDevice;
    public static IDXGISwapChain1 SwapChain;
    public static ID3D11Texture2D BackBuffer;
    public static ID2D1RenderTarget RenderTargetView;

    public static IDXGISurface DxgiBackBuffer;
    public static ID2D1Factory1 D2dFactory;
    public static ID2D1Device D2dDevice;
    public static ID2D1DeviceContext D2dContext;
    public static ID2D1Bitmap1 D2dTargetBitmap1;

    public static Vortice.WinUI.ISwapChainPanelNative SwapChainPanel;
    public static ID3D11Device D3Ddevice;
    public static IDWriteFactory D2DWriteFactory;

    public static BitmapProperties1 D2DBitmapProperties = new()
    {
        BitmapOptions = BitmapOptions.Target|BitmapOptions.CannotDraw,
        PixelFormat = new PixelFormat(Format.B8G8R8A8_UNorm, Vortice.DCommon.AlphaMode.Premultiplied),
    };

    public static RenderTargetProperties D2DRenderTargetProperties = new()
    {
        Type = RenderTargetType.Hardware,
        PixelFormat = new PixelFormat(Format.B8G8R8A8_UNorm, Vortice.DCommon.AlphaMode.Premultiplied)
    };

    public static void InitDirectX()
    {
        FeatureLevel[] featureLevels =
        [
            FeatureLevel.Level_12_1,
            FeatureLevel.Level_12_0,
            FeatureLevel.Level_11_1,
            FeatureLevel.Level_11_0,
            FeatureLevel.Level_10_1,
            FeatureLevel.Level_10_0,
            FeatureLevel.Level_9_3,
            FeatureLevel.Level_9_2,
            FeatureLevel.Level_9_1
        ];

        D3D11.D3D11CreateDevice(
            null,
            DriverType.Hardware,
            DeviceCreationFlags.BgraSupport | DeviceCreationFlags.Debug,
            featureLevels,
            out var tempDevice,
            // out D3Ddevice,
            out ID3D11DeviceContext _).CheckError();
        D3Ddevice = tempDevice;
        // deviceContext = tempContext;
        DxgiDevice = D3Ddevice.QueryInterface<IDXGIDevice>();
    }


    public static void ResizeSwapChain(int width, int height)
    {
        D2dContext.Target = null;
        // renderTargetView.Dispose();

        // RenderTargetView.Dispose();

        BackBuffer.Dispose();
        DxgiBackBuffer.Dispose();
        D2dTargetBitmap1.Dispose();

        SwapChain.ResizeBuffers(2, width, height, Format.B8G8R8A8_UNorm,
            SwapChainFlags.None);
        BackBuffer = SwapChain.GetBuffer<ID3D11Texture2D>(0);
        DxgiBackBuffer = BackBuffer.QueryInterface<IDXGISurface>();
     
        D2dTargetBitmap1= D2dContext.CreateBitmapFromDxgiSurface(DxgiBackBuffer, D2DBitmapProperties);
        // RenderTargetView = D2dFactory.CreateDxgiSurfaceRenderTarget(DxgiBackBuffer, D2DRenderTargetProperties);

        // D2dContext.Target = D2dTargetBitmap1;
    }

    public static void CreateSwapChain(SwapChainPanel swapChainCanvas)
    {
        ComObject comObject = new ComObject(swapChainCanvas);
        SwapChainPanel = comObject.QueryInterfaceOrNull<Vortice.WinUI.ISwapChainPanelNative>();
        comObject.Dispose();

        SwapChainDescription1 swapChainDesc = new SwapChainDescription1()
        {
            Stereo = false,
            Width = (int)swapChainCanvas.Width,
            Height = (int)swapChainCanvas.Height,
            BufferCount = 2,
            BufferUsage = Usage.RenderTargetOutput,
            Format = Format.B8G8R8A8_UNorm,
            SampleDescription = new SampleDescription(1, 0),
            Scaling = Scaling.Stretch,
            AlphaMode = AlphaMode.Premultiplied,
            Flags = SwapChainFlags.None,
            SwapEffect = SwapEffect.FlipSequential
        };

        IDXGIAdapter1 dxgiAdapter = DxgiDevice.GetParent<IDXGIAdapter1>();
        IDXGIFactory2 dxgiFactory2 = dxgiAdapter.GetParent<IDXGIFactory2>();

        // resize window flick bug
        SwapChain = dxgiFactory2.CreateSwapChainForComposition(D3Ddevice, swapChainDesc);

        BackBuffer = SwapChain.GetBuffer<ID3D11Texture2D>(0);
        // renderTargetView = device.CreateRenderTargetView(backBuffer);
        DxgiBackBuffer = BackBuffer.QueryInterface<IDXGISurface>();
        if (SwapChainPanel != null) SwapChainPanel.SetSwapChain(SwapChain);

        D2dFactory = D2D1.D2D1CreateFactory<ID2D1Factory1>(FactoryType.MultiThreaded);
        D2dDevice = D2dFactory.CreateDevice(DxgiDevice);
        D2dContext = D2dDevice.CreateDeviceContext(DeviceContextOptions.EnableMultithreadedOptimizations);
        D2DWriteFactory = DWrite.DWriteCreateFactory<IDWriteFactory>();


   
        // RenderTargetView = D2dFactory.CreateDxgiSurfaceRenderTarget(DxgiBackBuffer, D2DRenderTargetProperties);

        D2dTargetBitmap1 = D2dContext.CreateBitmapFromDxgiSurface(DxgiBackBuffer, D2DBitmapProperties);

        LoadStaticDrawResources();

        DxgiDevice.Dispose();

        // d2dFactory.DesktopDpi.X, d2dFactory.DesktopDpi.Y,
        // BitmapOptions.Target | BitmapOptions.CannotDraw);
    }

    public static ID2D1SolidColorBrush CellGridBrush;

    public static ID2D1SolidColorBrush ColumnRowFillBrush;


    public static ID2D1SolidColorBrush CellTextBrush;

    public static ID2D1SolidColorBrush CellBackgroundBrush;

    public static ID2D1SolidColorBrush CellSelectedBrush;

    public static float BorderThick = 2.0f;

    private static void LoadStaticDrawResources()
    {
        CellGridBrush= D2dContext.CreateSolidColorBrush("#CADABF".ToColor());
        CellTextBrush= D2dContext.CreateSolidColorBrush("#000000".ToColor());
        CellBackgroundBrush= D2dContext.CreateSolidColorBrush("#E7E8D8".ToColor());
        CellSelectedBrush= D2dContext.CreateSolidColorBrush("#F9E400".ToColor());
        ColumnRowFillBrush= D2dContext.CreateSolidColorBrush("#5F6F65".ToColor());
    }
}