using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renderer.Models
{
    public struct Triangle<TVertexInfo>
    {
        public Vertex<TVertexInfo> A { get; set; }
        public Vertex<TVertexInfo> B { get; set; }
        public Vertex<TVertexInfo> C { get; set; }

        public Triangle(Vertex<TVertexInfo> a, Vertex<TVertexInfo> b, Vertex<TVertexInfo> c)
        {
            A = a;
            B = b;
            C = c;
        }
    }
}
