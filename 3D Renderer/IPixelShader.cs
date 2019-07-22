using Renderer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renderer
{
    public interface IPixelShader<TPixelIn>
    {
        PositionColor GetPixelColor(Vertex<TPixelIn> input);
    }
}
