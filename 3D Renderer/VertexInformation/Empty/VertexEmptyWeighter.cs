using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Renderer.Models;

namespace Renderer.VertexInformation.Empty
{
    class VertexEmptyWeighter : IVertexToPixelWeighter<VertexEmpty>
    {
        public VertexEmpty GetWeightedInformation(Weighted<Vertex<VertexEmpty>> vA, Weighted<Vertex<VertexEmpty>> vB, Weighted<Vertex<VertexEmpty>> vC)
        {
            return vA.Object.Information;
        }
    }
}
