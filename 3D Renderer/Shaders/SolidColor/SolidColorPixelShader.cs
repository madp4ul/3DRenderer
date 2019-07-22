using Renderer.VertexInformation.Color;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Renderer.Models;

namespace Renderer.Shaders.SolidColor
{
    public class SolidColorPixelShader : IPixelShader<VertexColor>
    {
        public PositionColor GetPixelColor(Vertex<VertexColor> input)
        {
            return new PositionColor
            {
                Position = input.Position.Get2D(),
                Color = input.Information.Color,
            };
        }
    }
}
