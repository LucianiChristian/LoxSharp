using LoxSharp;

if (args.Length > 1)
{
    Console.WriteLine("Usage: dotnet run [script]");
    Environment.Exit(64);
}
else if (args.Length == 1)
{
    LoxRunner.RunFile(args[0]);
}
else
{
    LoxRunner.RunPrompt();
}

public class LoxRunner
{
    public static void RunFile(string filePath)
    {
        var source = File.ReadAllText(filePath);
        
        Run(source);
    }

    public static void RunPrompt()
    {
        while (true)
        {
            Console.Write("> ");
            
            var line = Console.ReadLine();

            if (line is null) break;
            
            Run(line);
        }
    }

    private static void Run(string source)
    {
        var scanner = new Scanner(source);

        var tokens = scanner.ScanTokens();

        foreach (var token in tokens)
        {
            Console.WriteLine(token);
        }
    }
}