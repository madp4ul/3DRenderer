using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Renderer.Components;

namespace _3DRenderer.ComponentTests
{
    [TestClass]
    public class DefaultDepthBufferTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var depthBuffer = new DefaultDepthBuffer(1);

            Assert.AreEqual(1, depthBuffer.MaxDepth);
        }
    }
}
