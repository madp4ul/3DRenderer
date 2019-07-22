using Renderer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renderer
{
    public interface IVertexShader<TVertexIn, TVertexOut>
    {
        TransformedVertex<TVertexOut> Apply(Vertex<TVertexIn> vertex, Matrix worldTransformation);
    }
}
