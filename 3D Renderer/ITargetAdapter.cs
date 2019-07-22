using Renderer.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renderer
{
    public interface ITargetAdapter : IDisposable
    {
        /// <summary>
        /// if not begun, begin
        /// </summary>
        void Begin();
        /// <summary>
        /// if not begun, exception. else end.
        /// </summary>
        void End();

        Vector3 Read(Point position);
        void Write(Point position, Vector3 color);

        void Clear();

        Vector2 TargetSize { get; }
    }
}
