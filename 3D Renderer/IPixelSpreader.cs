using Renderer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renderer
{
    public interface IPixelSpreader
    {
        bool Active { get; set; }

        Vector2 ScreenSize { get; set; }

        IEnumerable<PixelColor> GetPixels(PositionColor posColor);
    }
}
