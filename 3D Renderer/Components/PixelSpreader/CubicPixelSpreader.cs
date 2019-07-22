using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Renderer.Models;

namespace Renderer.Components.PixelSpreader
{
    public class CubicPixelSpreader : PixelSpreaderBase
    {
        protected override double GetFaktor(Vector2 cutOff)
        {
            return cutOff.LengthSquared;
        }
    }
}
