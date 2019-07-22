using Renderer.Components.WeighterHelpers;
using Renderer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renderer.VertexInformation.Color.Normal
{
    public class VertexColorNormalWeighter : IVertexToPixelWeighter<VertexColorNormal>
    {
        private Vector3Weighter _V3Weighter;
        private Vector4Weighter _V4Weighter;

        public VertexColorNormalWeighter()
        {
            _V3Weighter = new Vector3Weighter();
            _V4Weighter = new Vector4Weighter();
        }

        public VertexColorNormal GetWeightedInformation(Weighted<Vertex<VertexColorNormal>> vA, Weighted<Vertex<VertexColorNormal>> vB, Weighted<Vertex<VertexColorNormal>> vC)
        {
            return new VertexColorNormal
            {
                Color = _V4Weighter.GetWeighted(
                    vA.Object.Information.Color, vA.Weight,
                    vB.Object.Information.Color, vB.Weight,
                    vC.Object.Information.Color, vC.Weight),
                Normal = _V3Weighter.GetWeighted(
                    vA.Object.Information.Normal, vA.Weight,
                    vB.Object.Information.Normal, vB.Weight,
                    vC.Object.Information.Normal, vC.Weight)
            };
        }
    }
}
