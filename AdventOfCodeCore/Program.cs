using AdventOfCodeCore.Core;

var yearManager = new YearManager();
var solutionRunner = new SolutionRunner(yearManager);
var filterRate = new FilterRate();
//var rendererRunner = new RendererRunner(yearManager);

//var runners = new IRunner[] { solutionRunner, rendererRunner, yearManager };
var runners = new IRunner[] { solutionRunner, yearManager, filterRate };

foreach (var item in runners)
    item.PrintStartupMessage();

while (true)
{
    var values = (Console.ReadLine() ?? string.Empty).Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
    var result = runners.FirstOrDefault(it => it.CheckRequest(values));
    if (result == null)
        Console.WriteLine("unexpected command");
}
