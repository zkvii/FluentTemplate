using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.WinUI.Helpers;

namespace FluentTemplate.Helpers;

public static class TypeHelper
{
    public static Vortice.Mathematics.Color ToColor(this string colorString)
    {
        var color = ColorHelper.ToColor(colorString);
        return new Vortice.Mathematics.Color(color.R, color.G, color.B, color.A);
    }
}