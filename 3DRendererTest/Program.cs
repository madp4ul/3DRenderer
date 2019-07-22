using Renderer.Components;
using Renderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Renderer.VertexInformation.Color;
using Renderer.Shaders.SolidColor;
using ModelParser.Parsers.obj;
using System.IO;
using Renderer.Models;
using Renderer.Components.TargetAdapters;
using Renderer.Shaders.DiffuseLighting;
using Renderer.VertexInformation.Color.Normal;
using System.Diagnostics;
using _3DRendererTest.SceneExamples;

namespace _3DRendererTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press any key to start");
            Console.Read();

            new AffeExample().Show();
            Console.Read();
        }


        private static string GetModelLocation()
        {
            Console.WriteLine("Enter model name: ");
            string name = Console.ReadLine();
            name = name == "" ? "affe" : name;

            return "Models/" + name + ".obj";
        }

        private static void ShowParallelDiffuseExample()
        {
            var vertexShader = new DiffuseLightingVertexShader();
            var pixelShader = new DiffuseLightingPixelShader();

            using (var drawer = new ParallelDrawer<VertexColorNormal, VertexColorNormal>(
                vertexShader,
                pixelShader,
                new VertexColorNormalWeighter(),
                targetAdapter: new DesktopImageAdapter { TargetSize = new Vector2(600, 600) }))
            {
                vertexShader.Culling =
                    drawer.RasterizerFactory.Cullmode = Cullmode.Clockwise;


                var parser = new ObjParserPosition();

                string fileText = File.ReadAllText(GetModelLocation());

                int index = 0;

                var modelPos = parser.Parse(fileText);
                double colorFaktor = 1d / (modelPos.Vertices.Count() - 1);
                var modelColor = modelPos.Copy((p, v) => new VertexColor
                {
                    Color = new Vector4(colorFaktor * index, 1 - colorFaktor * index++, 0, 1),
                }
                );

                var modelColorNormal = modelColor.Copy((t, p, v) =>
                {
                    Vector3 direction1 = t.B.Position.Subtraction(t.A.Position);
                    Vector3 direction2 = t.C.Position.Subtraction(t.A.Position);

                    return new VertexColorNormal
                    {
                        Color = v.Color,
                        Normal = direction1.Cross(direction2).Multiply(-1).Normalize(),
                    };
                });
                vertexShader.ViewMatrix = Matrix.CreateLookAtView(
                    new Vector3(0, 1, 2), Vector3.Zero, new Vector3(0, 1, 0));

                vertexShader.ProjectionMatrix = Matrix.CreatePerspectiveProjection(60, 60, 0.1, 50);
                vertexShader.LightDirection = new Vector3(0, 0, -4);
                vertexShader.AmbientLight = 0.3;


                for (int i = 0; i < 5000; i++)
                {
                    modelColorNormal.Transformation = Matrix.CreateRotationY(i * 10);

                    Stopwatch time = new Stopwatch();
                    time.Start();
                    drawer.Draw(modelColorNormal);
                    drawer.Clear();
                    //drawer.DepthBuffer.Clear();
                    time.Stop();
                    Console.WriteLine(time.ElapsedMilliseconds + "ms");
                }

            }
        }

        private static void ShowDiffuseExample()
        {
            var vertexShader = new DiffuseLightingVertexShader();
            var pixelShader = new DiffuseLightingPixelShader();
            var targetAdapter = new DesktopImageAdapter { TargetSize = new Vector2(600, 600) };

            var drawer = new DefaultDrawer<VertexColorNormal, VertexColorNormal>(
                vertexShader,
                pixelShader,
                new VertexColorNormalWeighter(),
                targetAdapter: targetAdapter);
            drawer.RasterizerFactory.Cullmode = Cullmode.CounterClockwise;

            var parser = new ObjParserPosition();

            string fileText = File.ReadAllText(GetModelLocation());

            int index = 0;

            var modelPos = parser.Parse(fileText);
            double colorFaktor = 1d / (modelPos.Vertices.Count() - 1);
            var modelColor = modelPos.Copy((p, v) => new VertexColor
            {
                Color = new Vector4(colorFaktor * index, 0, 1 - colorFaktor * index++, 1),
            }
            );

            var modelColorNormal = modelColor.Copy((t, p, v) =>
            {
                Vector3 direction1 = t.B.Position.Subtraction(t.A.Position);
                Vector3 direction2 = t.C.Position.Subtraction(t.A.Position);

                return new VertexColorNormal
                {
                    Color = v.Color,
                    Normal = direction1.Cross(direction2).Multiply(-1).Normalize(),
                };
            });
            vertexShader.ViewMatrix = Matrix.CreateLookAtView(
                new Vector3(0, 1, 2), Vector3.Zero, new Vector3(0, 1, 0));

            vertexShader.ProjectionMatrix = Matrix.CreatePerspectiveProjection(60, 60, 0.1, 10);
            vertexShader.LightDirection = new Vector3(-1, 1, -4);
            vertexShader.AmbientLight = 0.3;


            for (int i = 0; i < 50; i++)
            {
                modelColorNormal.Transformation = Matrix.CreateRotationY(i * 10);

                Stopwatch time = new Stopwatch();
                time.Start();
                drawer.Draw(modelColorNormal);
                drawer.DepthBuffer.Clear();
                time.Stop();
                Console.WriteLine(time.ElapsedMilliseconds + "ms");
            }

            drawer.TargetAdapter.Dispose();
        }

        private static void ShowSolidColorExample()
        {
            var vertexShader = new SolidColorVertexShader();
            var pixelShader = new SolidColorPixelShader();
            var targetAdapter = new DesktopImageAdapter { TargetSize = new Vector2(600, 600) };

            var drawer = new DefaultDrawer<VertexColor, VertexColor>(
                vertexShader,
                pixelShader,
                new VertexColorWeighter(),
                targetAdapter: targetAdapter);

            var parser = new ObjParserPosition();

            string fileText = File.ReadAllText(GetModelLocation());

            int index = 0;

            var model = parser.Parse(fileText).Copy((p, v) =>
            {
                index++;
                return new VertexColor
                {
                    Color = new Vector4(0.1 * index, 0, 1 - 0.1 * index, 1),
                };
            });
            model.Transformation = Matrix.CreateTranslation(0, 0, 0);
            vertexShader.ViewMatrix = Matrix.CreateLookAtView(
                new Vector3(-1, 1, 1), Vector3.Zero, new Vector3(0, 1, 0));

            vertexShader.ProjectionMatrix = Matrix.CreatePerspectiveProjection(60, 60, 0.1, 100);

            drawer.Draw(model);

            drawer.TargetAdapter.Dispose();
        }
    }
}
