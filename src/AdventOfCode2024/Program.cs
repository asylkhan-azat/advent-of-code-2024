using AdventOfCode2024;

var problemFactory = new Dictionary<int, Func<int, IProblem>>
{
    [1] = static problemId => new Problem1(GetProblemInputFile(problemId), Console.Out)
};

var problemId = ReadProblemId();
var problem = problemFactory[problemId].Invoke(problemId);
problem.Solve();
return 0;

static string GetProblemInputFile(int problemId)
{
    var folder = Environment.GetEnvironmentVariable("ADVENT_OF_CODE_INPUTS_FOLDER");

    if (folder is null)
    {
        throw new InvalidOperationException("Specify ADVENT_OF_CODE_INPUTS_FOLDER in environment variables");
    }

    return Path.Combine(folder, $"input_{problemId}.txt");
}

static int ReadProblemId()
{
    Console.Write("Enter problem ID: ");
    int problemId;
    
    while (!int.TryParse(Console.ReadLine(), out problemId))
    {
        Console.Clear();
        Console.Write("Enter valid problem ID: ");
    }

    return problemId;
}