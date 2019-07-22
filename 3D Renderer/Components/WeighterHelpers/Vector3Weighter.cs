using Renderer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renderer.Components.WeighterHelpers
{
    public class Vector3Weighter : WeighterBase<Vector3>, IPositionWeighter
    {
        public override Vector3 GetWeighted(Vector3 vA, double wA, Vector3 vB, double wB, Vector3 vC, double wC)
        {
            return new Vector3(
                  vA.X * WeightModA(wA)
                + vB.X * WeightModB(wB)
                + vC.X * WeightModC(wC),
                  vA.Y * WeightModA(wA)
                + vB.Y * WeightModB(wB)
                + vC.Y * WeightModC(wC),
                  vA.Z * WeightModA(wA)
                + vB.Z * WeightModB(wB)
                + vC.Z * WeightModC(wC));
        }

        public Vector3 GetWeightedPosition<TVertexInfo>(
            Weighted<Vertex<TVertexInfo>> vA,
            Weighted<Vertex<TVertexInfo>> vB,
            Weighted<Vertex<TVertexInfo>> vC)
        {
            return GetWeighted(
                vA.Object.Position, vA.Weight,
                vB.Object.Position, vB.Weight,
                vC.Object.Position, vC.Weight);
        }
    }
}
