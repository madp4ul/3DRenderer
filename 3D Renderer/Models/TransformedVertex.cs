using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renderer.Models
{
    public class TransformedVertex<TInfo>
    {
        public Vector4 Position { get; set; }

        public TInfo Information { get; set; }

        public TransformedVertex(Vector4 transformedPosition, TInfo info)
        {
            Position = transformedPosition;
            Information = info;
        }
    }
}
