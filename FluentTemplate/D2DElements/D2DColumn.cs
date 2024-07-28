using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using FluentTemplate.Interfaces;
using Vortice;
using Vortice.Direct2D1;
using Vortice.Mathematics;
using static FluentTemplate.Helpers.DirectXHelper;

namespace FluentTemplate.D2DElements
{
    public partial class D2DColumn : ObservableRecipient, IBaseElement
    {
        public float X { get; set; }
        public float Y { get; set; }

        public float Width { get; set; }
        public float Height { get; set; }

        public bool IsSelected { get; set; }

        public ID2D1CommandList CommandList;

        public D2DColumn(float x, float y,float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            LoadResources();
        }

        private void LoadResources()
        {
            D2dContext.Target = D2dTargetBitmap1;
            CommandList = D2dContext.CreateCommandList();

            D2dContext.Target = CommandList;
            D2dContext.BeginDraw();
            // D2dContext.Clear(Colors.AliceBlue);
            // D2dContext.DrawRectangle();
            D2dContext.DrawLine(new Vector2(X, Y), 
                new Vector2(X, Height),
                CellGridBrush,BorderThick);
            D2dContext.DrawLine(new Vector2(X + Width, Y), 
                new Vector2(X + Width, Height),
                CellGridBrush,
                BorderThick);
            

            if (IsSelected)
            {
                D2dContext.FillRectangle(new Rect(X, 0, Width, Height), CellBackgroundBrush);
            }

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
}