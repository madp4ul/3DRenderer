using Renderer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DRendererTest
{
    public interface IScene<TVertexIn, TVertexOut>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="time">timeoffset from start of scene</param>
        /// <returns></returns>
        IEnumerable<SceneElement<TVertexIn>> Contents(TimeSpan time);

        /// <summary>
        /// gets shaders etc
        /// </summary>
        /// <returns></returns>
        SceneConfig<TVertexIn, TVertexOut> GetShaderInfo();
    }
}
