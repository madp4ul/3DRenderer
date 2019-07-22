using Renderer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renderer.Components.WeighterHelpers
{
    class Vector4Weighter : WeighterBase<Vector4>
    {
        public override Vector4 GetWeighted(Vector4 vA, double wA, Vector4 vB, double wB, Vector4 vC, double wC)
        {
            return new Vector4(
                  vA.X * WeightModA(wA)
                + vB.X * WeightModB(wB)
                + vC.X * WeightModC(wC),
                  vA.Y * WeightModA(wA)
                + vB.Y * WeightModB(wB)
                + vC.Y * WeightModC(wC),
                  vA.Z * WeightModA(wA)
                + vB.Z * WeightModB(wB)
                + vC.Z * WeightModC(wC),
                  vA.W * WeightModA(wA)
                + vB.W * WeightModB(wB)
                + vC.W * WeightModC(wC));
        }
    }
}
