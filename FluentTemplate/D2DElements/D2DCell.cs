using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using FluentTemplate.Helpers;
using FluentTemplate.Interfaces;
using FluentTemplate.Views;
using Vortice.Direct2D1;
using Vortice.Mathematics;
using static FluentTemplate.Helpers.DirectXHelper;


namespace FluentTemplate.D2DElements;

public partial class D2DCell
    : ObservableRecipient, IBaseElement
{
    public float X { get; set; } 
    public float Y { get; set; } 
    public float Width { get; set; } 
    public float Height { get; set; } 

    public D2DCell(float x, float y, float width, float height)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
        LoadResources();
    }

    public ID2D1CommandList CommandList;

    public void LoadResources()
    {
        D2dContext.Target = D2dTargetBitmap1;
        CommandList = D2dContext.CreateCommandList();

        D2dContext.Target = CommandList;
        D2dContext.BeginDraw();
        // D2dContext.Clear(Colors.AliceBlue);
        // D2dContext.DrawRectangle();
     
        D2dContext.EndDraw();
        CommandList.Close();
    }


    public void OnHover()
    {
        throw new NotImplementedException();
    }

    public void OnClick()
    {
        throw new NotImplementedException();
    }

    public void OnDoubleClick()
    {
        throw new NotImplementedException();
    }

    public void OnRightClick()
    {
        throw new NotImplementedException();
    }
}