using Renderer;
using Renderer.Components;
using Renderer.Shaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DRendererTest
{
    class SceneLooper<TVertexIn, TVertexOut>
    {
        private IScene<TVertexIn, TVertexOut> _Scene;
        private IDrawer<TVertexIn, TVertexOut> _Drawer;

        private VertexShaderBase<TVertexIn, TVertexOut> _VertexShader;

        public readonly TimeSpan PlayTime;

        public SceneLooper(
            IScene<TVertexIn, TVertexOut> scene,
            TimeSpan playtime, ITargetAdapter target = null)
        {
            PlayTime = playtime;
            _Scene = scene;
            var shaderInfo = _Scene.GetShaderInfo();
            _VertexShader = shaderInfo.VertexShader;

            _Drawer = new ParallelDrawer<TVertexIn, TVertexOut>(
                shaderInfo.VertexShader,
                shaderInfo.PixelShader,
                shaderInfo.VertexWeighter,
                targetAdapter: target);
        }

        public void Loop()
        {
            TimeSpan current = TimeSpan.Zero;

            while (current < PlayTime)
            {
                DateTime start = DateTime.Now;

                _Drawer.Clear();

                foreach (var elem in _Scene.Contents(current))
                {
                    DrawSceneElement(elem);
                }

                current = current + (DateTime.Now - start);
            }
        }

        protected virtual void DrawSceneElement(SceneElement<TVertexIn> sceneElem)
        {
            _VertexShader.ViewMatrix = sceneElem.View;
            _VertexShader.ProjectionMatrix = sceneElem.Projection;

            _Drawer.Draw(sceneElem.Model);
        }
    }
}
