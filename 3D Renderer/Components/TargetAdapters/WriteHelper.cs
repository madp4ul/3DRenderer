using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Renderer.Models;

namespace Renderer.Components.TargetAdapters
{
    public static class WriteHelper
    {
        public static Color ToColor(Vector3 color)
        {
            return Color.FromArgb((int)(color.X * 255), (int)(color.Y * 255), (int)(color.Z * 255));
        }
    }
}
