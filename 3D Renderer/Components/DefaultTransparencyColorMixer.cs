using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Renderer.Models;

namespace Renderer.Components
{
    public class DefaultTransparencyColorMixer : ITransparencyColorMixer
    {
        public Vector3 GetMixedColor(Func<Vector3> getPreviousColor, Vector4 newColor)
        {
            if (newColor.W == 1)
            {
                return newColor.Get3D();
            }
            else
            {
                double newColorAlpha = newColor.W;
                double prevColorAlpha = 1 - newColorAlpha;

                Vector3 prevColor = getPreviousColor();

                return new Vector3(
                    newColor.X * newColorAlpha + prevColor.X * prevColorAlpha,
                    newColor.Y * newColorAlpha + prevColor.Y * prevColorAlpha,
                    newColor.Z * newColorAlpha + prevColor.Z * prevColorAlpha);
            }
        }
    }
}
