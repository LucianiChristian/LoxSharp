using LoxSharp;

public static class LoxRunner
{
    private static readonly ErrorManager ErrorManager = new();
    
    public static void RunFile(string filePath)
    {
        var source = File.ReadAllText(filePath);
        
        Run(source);

        if (ErrorManager.HasError)
        {
            Environment.Exit(65);
        }
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
        var scanner = new Scanner(source, ErrorManager);

        var tokens = scanner.ScanTokens();

        foreach (var token in tokens)
        {
            Console.WriteLine(token);
        }
    }
}