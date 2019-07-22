using Renderer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renderer
{
    public interface IScreenScaler
    {
        Vector3 GetScaled(Vector3 vector);

        Vector2 ScreenSize { get; set; }
    }
}
