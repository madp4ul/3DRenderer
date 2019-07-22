using Renderer.VertexInformation.Color.Normal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Renderer.Models;

namespace Renderer.Shaders.DiffuseLighting
{
    public class DiffuseLightingPixelShader : IPixelShader<VertexColorNormal>
    {


        public PositionColor GetPixelColor(Vertex<VertexColorNormal> input)
        {
            return new PositionColor
            {
                Color = input.Information.Color,
                Position = input.Position.Get2D(),
            };
        }
    }
}
