using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renderer.Models
{
    public class TriangleTransformation<TVertexInfo>
    {
        public Triangle<TVertexInfo> Triangle { get; set; }
        public Matrix WorldTransformation { get; set; }
    }
}
