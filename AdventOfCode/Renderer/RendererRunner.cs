using AdventOfCode.Core;
using ImageMagick;
using System;
using System.Linq;
using System.Reflection;

namespace AdventOfCode.Renderer
{
    // Useful links about Magik.NET:
    // https://github.com/dlemstra/Magick.NET/blob/main/docs/Readme.md
    // https://stackoverflow.com/questions/1196322/how-to-create-an-animated-gif-in-net

    class RendererRunner : IRunner
    {
        private RenderableMapping[] solutions;

        public void SetYear(int year)
        {
            var targets = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(it => it.FullName.Contains(year.ToString()))
                .SelectMany(it => it.GetMethods()
                    .Where(m => m.GetCustomAttribute<RenderableAttribute>() != null)
                    .Select(m => new { Type = it, Method = m, Info = m.GetCustomAttribute<RenderableAttribute>() }));

            solutions = targets.Select(it => new RenderableMapping
            {
                Day = it.Info.Day,
                Version = it.Info.Version,
                ClassType = it.Type,
                Method = it.Method,
            }).OrderByDescending(it => it.Day)
                .ThenBy(it => it.Version ?? 0)
                .ToArray();
        }

        public bool CheckRequest(string[] args)
        {
            if (args.Length == 0 || args[0].ToLower() != "draw")
                return false;

            if(args.Length == 1)
            {
                var match = solutions.FirstOrDefault();
                RunInstance(match);
            }
            else if(args.Length == 2 && int.TryParse(args[1], out var justDay))
            {
                var match = solutions.Where(it => it.Day == justDay).FirstOrDefault();
                RunInstance(match);
            }
            else if(args.Length == 3 && int.TryParse(args[1], out var day) && int.TryParse(args[2], out var version))
            {
                var match = solutions.FirstOrDefault(it => it.Day == day && it.Version == version);
                RunInstance(match);
            } 
            else
                Console.WriteLine("Invalid arguments for draw command");

            return true;
        }

        public void PrintStartupMessage()
        {
            // Unused for this runner
        }

        private void RunInstance(RenderableMapping mapping)
        {
            if (mapping == null)
            {
                Console.WriteLine($"Unable to find solution in for given date and version");

                return;
            }

            var input = Clipboard.Read();
            var instance = Activator.CreateInstance(mapping.ClassType);

            try
            {
                Console.WriteLine($"Running renderer for {mapping.GetDescription()}");
                var result = mapping.Method.Invoke(instance, new object[] { input });
                if (result is MagickImage)
                {
                    var pngImage = result as MagickImage;
                    pngImage.Write($"{mapping.GetDescription()} {DateTime.Now.ToString("yyyyMMdd")}.png");
                }
                else if (result is MagickImageCollection)
                {
                    var gifImage = result as MagickImageCollection;
                    gifImage.Write($"{mapping.GetDescription()} {DateTime.Now.ToString("yyyyMMdd")}.gif");
                }
                else
                {
                    Console.WriteLine($"Instance of method for {mapping.GetDescription()} generated unexpected object {result.GetType()}");

                    return;
                }

                Console.WriteLine("Genereted and saved image");
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to run solution: " + e.Message);
            }
        }

        private class RenderableMapping
        {
            public int Day { get; set; }

            public int? Version { get; set; }

            public Type ClassType { get; set; }

            public MethodInfo Method { get; set; }

            public string GetDescription()
            {
                if (Version == null)
                    return $"Day {Day}";
                else
                    return $"Day {Day} Version {Version}";
            }
        }
    }
}
