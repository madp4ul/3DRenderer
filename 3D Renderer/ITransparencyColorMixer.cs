using Renderer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renderer
{
    public interface ITransparencyColorMixer
    {
        Vector3 GetMixedColor(Func<Vector3> getPreviousColor, Vector4 newColor);
    }
}
