using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renderer.Models
{
    public class Vertex<TInfo>
    {
        public Vector3 Position { get; set; }

        public TInfo Information { get; set; }

        public Vertex(Vector3 position, TInfo info)
        {
            Position = position;
            Information = info;
        }
    }
}
