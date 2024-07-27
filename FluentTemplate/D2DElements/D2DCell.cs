using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using FluentTemplate.Helpers;
using FluentTemplate.Interfaces;
using Vortice.Direct2D1;
using static FluentTemplate.Helpers.DirectXHelper;


namespace FluentTemplate.D2DElements;

public partial class D2DCell(float x, float y, float width, float height)
    : ObservableRecipient, IBaseElement
{
    public float X { get; set; } = x;
    public float Y { get; set; } = y;
    public float Width { get; set; } = width;
    public float Height { get; set; } = height;

    public ID2D1CommandList CommandList = D2dContext.CreateCommandList();

    public void Draw()
    {
       
    }

    public void OnScale()
    {
        throw new NotImplementedException();
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