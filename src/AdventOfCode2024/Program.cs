using System.Diagnostics;
using AdventOfCode2024;


var problemId = ReadInt("Enter problem ID: ", "Please, enter valid problemID: ");
var problem = GetProblem(problemId);

var partNumber = ReadInt("Enter part number: ", "Please, enter valid part number: ");
problem.Solve(partNumber);
return 0;

static IProblem GetProblem(int problemId)
{
    var type = Type.GetType($"AdventOfCode2024.Problem{problemId}");

    if (type is null) throw new InvalidOperationException("Problem not found");

    var constructor = type.GetConstructor([typeof(string), typeof(TextWriter)]);

    if (constructor is null) throw new InvalidOperationException("Problem does not contain constructor");

    var problem = constructor.Invoke([GetProblemInputFile(problemId), Console.Out]) as IProblem;
    
    Debug.Assert(problem is not null);

    return problem;
}

static string GetProblemInputFile(int problemId)
{
    var folder = Environment.GetEnvironmentVariable("ADVENT_OF_CODE_INPUTS_FOLDER");

    if (folder is null)
    {
        throw new InvalidOperationException("Specify ADVENT_OF_CODE_INPUTS_FOLDER in environment variables");
    }

    return Path.Combine(folder, $"input_{problemId}.txt");
}

static int ReadInt(string onFirstInputText, string? onErrorText = null)
{
    onErrorText ??= onFirstInputText;

    Console.Write(onFirstInputText);
    int problemId;

    while (!int.TryParse(Console.ReadLine(), out problemId))
    {
        Console.Clear();
        Console.Write(onErrorText);
    }

    return problemId;
}