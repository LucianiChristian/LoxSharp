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