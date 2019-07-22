using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Renderer.Models;
using Renderer.Components.WeighterHelpers;

namespace Renderer.VertexInformation.Color
{
    public class VertexColorWeighter : IVertexToPixelWeighter<VertexColor>
    {
        private Vector4Weighter _V4Weighter;

        public VertexColorWeighter()
        {
            _V4Weighter = new Vector4Weighter();
        }

        public VertexColor GetWeightedInformation(Weighted<Vertex<VertexColor>> vA, Weighted<Vertex<VertexColor>> vB, Weighted<Vertex<VertexColor>> vC)
        {
            return new VertexColor
            {
                Color = _V4Weighter.GetWeighted(
                    vA.Object.Information.Color, vA.Weight,
                    vB.Object.Information.Color, vB.Weight,
                    vC.Object.Information.Color, vC.Weight),
            };
        }
    }
}
