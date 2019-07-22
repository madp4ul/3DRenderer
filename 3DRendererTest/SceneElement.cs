using Renderer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DRendererTest
{
    public class SceneElement<TVertexInfo>
    {
        public Model<TVertexInfo> Model { get; set; }
        public Matrix Projection { get; set; }
        public Matrix View { get; set; }
    }
}
