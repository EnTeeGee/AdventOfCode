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
        private SolutionMapping[] solutions;

        public void SetYear(int year)
        {
            var targets = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(it => it.FullName.Contains(year.ToString()))
                .SelectMany(it => it.GetMethods()
                    .Where(m => m.GetCustomAttribute<RenderableAttribute>() != null)
                    .Select(m => new { Type = it, Method = m, Info = m.GetCustomAttribute<RenderableAttribute>() }));

            solutions = targets.Select(it => new SolutionMapping
            {
                Day = it.Info.Day,
                Problem = it.Info.Problem,
                ClassType = it.Type,
                Method = it.Method,
            }).ToArray();
        }

        public bool CheckRequest(string[] args)
        {
            if (args.Length == 0 || args[0].ToLower() != "draw")
                return false;

            if(args.Length == 3 && int.TryParse(args[1], out var day) && int.TryParse(args[2], out var problem))
            {
                var match = solutions.FirstOrDefault(it => it.Day == day && it.Problem == problem);

                if(match == null)
                {
                    Console.WriteLine($"Unable to find solution in set year for day {day}, problem {problem}");

                    return false;
                }

                var input = Clipboard.Read();
                var instance = Activator.CreateInstance(match.ClassType);

                try
                {
                    Console.WriteLine($"Running renderer for day {day} problem {problem}");
                    var result = match.Method.Invoke(instance, new object[] { input });
                    if(result is MagickImage)
                    {
                        var pngImage = result as MagickImage;
                        pngImage.Write($"Day {day} Problem {problem} {DateTime.Now.ToString("yyyyMMdd")}.png");
                    }
                    else if (result is MagickImageCollection)
                    {
                        var gifImage = result as MagickImageCollection;
                        gifImage.Write($"Day {day} Problem {problem} {DateTime.Now.ToString("yyyyMMdd")}.gif");
                    }
                    else
                    {
                        Console.WriteLine($"Instance of method for day {day} problem {problem} generated unexpected object {result.GetType()}");

                        return false;
                    }

                    Console.WriteLine("Genereted and saved image");
                    return true;

                }
                catch (Exception e)
                {
                    Console.WriteLine("Failed to run solution: " + e.Message);

                    return false;
                }
            } 
            else
            {
                Console.WriteLine("Invalid arguments for draw command");

                return false;
            }

            
        }

        public void PrintStartupMessage()
        {
            // Unused for this runner
        }
    }
}
