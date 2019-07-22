using Renderer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renderer
{
    public interface IPositionWeighter
    {
        Vector3 GetWeightedPosition<TVertexInfo>(
            Weighted<Vertex<TVertexInfo>> vA, Weighted<Vertex<TVertexInfo>> vB, Weighted<Vertex<TVertexInfo>> vC);
    }
}
