using Renderer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelParser
{
    public interface IModelParser<TVertexInfo>
    {
        Model<TVertexInfo> Parse(string text);
    }
}
