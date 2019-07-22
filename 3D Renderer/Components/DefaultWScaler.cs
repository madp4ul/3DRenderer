using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Renderer.Models;

namespace Renderer.Components
{
    public class DefaultWScaler : IWScaler
    {
        public Vector3 GetScaled(Vector4 position)
        {
            return new Vector3(position.X / position.W, position.Y / position.W, position.Z / position.W);
        }
    }
}
