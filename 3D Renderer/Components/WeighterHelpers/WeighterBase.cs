using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renderer.Components.WeighterHelpers
{
    public abstract class WeighterBase<TToWeight>
    {
        public abstract TToWeight GetWeighted(TToWeight vA, double wA, TToWeight vB, double wB, TToWeight vC, double wC);

        protected virtual double WeightModA(double weightA)
        {
            return weightA;
        }

        protected virtual double WeightModB(double weightB)
        {
            return weightB;
        }

        protected virtual double WeightModC(double weightC)
        {
            return weightC;
        }
    }
}
