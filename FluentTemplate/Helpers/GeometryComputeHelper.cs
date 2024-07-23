using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vortice.Mathematics;

namespace FluentTemplate.Helpers;

public static class GeometryComputeHelper
{
    public static bool IsPointInRect(float x, float y, Rect targetRect)
    {
        return x >= targetRect.Left && x <= targetRect.Right && y >= targetRect.Top && y <= targetRect.Bottom;
    }
}