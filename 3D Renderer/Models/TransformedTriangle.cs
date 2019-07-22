using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renderer.Models
{
    public struct TransformedTriangle<TVertexInfo>
    {
        public TransformedVertex<TVertexInfo> A { get; set; }
        public TransformedVertex<TVertexInfo> B { get; set; }
        public TransformedVertex<TVertexInfo> C { get; set; }

        public TransformedTriangle(TransformedVertex<TVertexInfo> a, TransformedVertex<TVertexInfo> b, TransformedVertex<TVertexInfo> c)
        {
            A = a;
            B = b;
            C = c;
        }
    }
}
