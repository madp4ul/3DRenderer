using Renderer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renderer.Shaders
{
    public static class ShaderMath
    {
        public static double Limit(double value, double min, double max)
        {
            return Math.Max(Math.Min(value, max), min);
        }

        public static Vector4 MultiplyRGB(Vector4 color, double scalar)
        {
            return new Vector4(color.X * scalar, color.Y * scalar, color.Z * scalar, color.W);
        }
    }
}
