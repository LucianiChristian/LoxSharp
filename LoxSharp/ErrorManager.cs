namespace LoxSharp;

public class ErrorManager
{
    public bool HasError { get; private set; }

    public void Error(int lineNumber, string message)
    {
        Report(lineNumber, "", message);
    }
    
    private void Report(int lineNumber, string whereItOccurs, string message)
    {
        Console.Error.WriteLine("[line " + lineNumber + "] Error" + whereItOccurs + ": " + message);
        HasError = true;
    }
}