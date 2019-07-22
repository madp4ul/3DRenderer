using Renderer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renderer
{
    /// <summary>
    /// not optimal because it contains two responsibilities:
    /// -it is responsible for making the weighting type-specific.
    /// -it decides on how to weight the information, especially the method "GetWeightedPosition"
    /// has only this responsibility
    /// </summary>
    public interface IVertexToPixelWeighter<TVertexInfo>
    {
        TVertexInfo GetWeightedInformation(
            Weighted<Vertex<TVertexInfo>> vA, Weighted<Vertex<TVertexInfo>> vB, Weighted<Vertex<TVertexInfo>> vC);
    }
}
