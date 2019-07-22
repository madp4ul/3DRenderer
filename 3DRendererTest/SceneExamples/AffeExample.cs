using _3DRendererTest.Scenes;
using ModelParser.Parsers.obj;
using Renderer.Components.TargetAdapters;
using Renderer.Models;
using Renderer.VertexInformation.Color.Normal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DRendererTest.SceneExamples
{
    class AffeExample : IExample
    {
        SceneLooper<VertexColorNormal, VertexColorNormal> _SceneLooper;

        public AffeExample()
        {
            var raw = File.ReadAllText("Models/affe.obj");
            var parser = new ObjParserPosition();

            var scene = new AffeScene(parser, raw);

            _SceneLooper = new SceneLooper<VertexColorNormal, VertexColorNormal>(
                scene, TimeSpan.FromMinutes(2), new DesktopImageAdapter { TargetSize = new Vector2(600, 600) });
        }

        public void Show()
        {
            _SceneLooper.Loop();
        }
    }
}
