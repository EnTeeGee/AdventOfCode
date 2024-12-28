using AdventOfCode.Common;
using ImageMagick;
using ImageMagick.Drawing;
using System.Linq;

namespace AdventOfCode.Renderer._2021
{
    class Day01
    {
        [Renderable(1)]
        public MagickImage GenerateImage(string input)
        {
            var depths = Parser.ToArrayOfInt(input);

            var bottom = depths.Max() + 10;
            var width = depths.Length;

            var output = new MagickImage(MagickColors.Brown, (uint)width, (uint)bottom);

            for(var i = 0; i < depths.Length; i++)
            {
                new Drawables()
                    .FillColor(MagickColors.Blue)
                    .StrokeColor(MagickColors.Blue)
                    .Line(i, 0, i, depths[i])
                    .Draw(output);
            }

            return output;
        }
    }
}
