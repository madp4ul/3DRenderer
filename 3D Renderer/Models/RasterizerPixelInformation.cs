using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renderer.Models
{
    public class RasterizerPixelInformation<TVertexInfo>
    {
        public Weighted<Vertex<TVertexInfo>> VertexWeightA { get; set; }
        public Weighted<Vertex<TVertexInfo>> VertexWeightB { get; set; }
        public Weighted<Vertex<TVertexInfo>> VertexWeightC { get; set; }
    }
}
