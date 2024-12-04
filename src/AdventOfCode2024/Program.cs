using AdventOfCode2024;

var problemFactory = new Dictionary<int, Func<int, IProblem>>
{
    [1] = static problemId => new Problem1(GetProblemInputFile(problemId), Console.Out),
    [2] = static problemId => new Problem2(GetProblemInputFile(problemId), Console.Out),
    [3] = static problemId => new Problem3(GetProblemInputFile(problemId), Console.Out),
    [4] = static problemId => new Problem4(GetProblemInputFile(problemId), Console.Out)
};

var problemId = ReadInt("Enter problem ID: ", "Please, enter valid problemID: ");
var problem = problemFactory[problemId].Invoke(problemId);

var partNumber = ReadInt("Enter part number: ", "Please, enter valid part number: ");
problem.Solve(partNumber);
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