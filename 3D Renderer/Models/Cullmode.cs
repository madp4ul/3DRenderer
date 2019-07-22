using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renderer.Models
{
    public enum Cullmode : int
    {
        Clockwise = -1,
        CounterClockwise = 1,
        None = 0
    }
}
