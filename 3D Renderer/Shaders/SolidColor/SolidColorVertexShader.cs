using Renderer.VertexInformation.Color;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Renderer.Models;

namespace Renderer.Shaders.SolidColor
{
    public class SolidColorVertexShader : VertexShaderBase<VertexColor, VertexColor>
    {
        public override TransformedVertex<VertexColor> Apply(Vertex<VertexColor> vertex, Matrix worldTransformation)
        {
            Matrix wvp = this.ViewProjectionMatrix.Multiply(worldTransformation);

            return new TransformedVertex<VertexColor>(
                wvp.Transform(vertex.Position), vertex.Information);
        }
    }
}
