namespace LoxSharp;

public class LexemeEater(string source, ErrorManager errorManager)
{
    // Index pointers used to munch tokens.
    // Points to the start of the lexeme that is currently being scanned.
    public int Start = 0;
    
    // Points to the current character being scanned as part of the lexeme.
    public int Current = 0;
    
    // Line pointer used for error reporting.
    // Example ->  "there's an invalid character at line number 2"
    public int LineNumber = 1;

    public bool IsAtEndOfSource => Current >= source.Length;

    // Consumes the next character
    public char EatChar()
    {
        return source[Current++];
    }

    public char Peek()
    {
        if (Current >= source.Length) return '\0';;

        return source[Current];
    }

    public char PeekNext()
    {
        if (Current + 1 >= source.Length) return '\0';;

        return source[Current + 1];
    }
    
    // Move the start pointer to where you last left off.
    // Where you last left off will be the character that immediately
    // follows the last scanned character.
    public void StartNewMunch() => Start = Current;

    public bool TryEatChar(char c)
    {
        if (Peek() != c) return false;

        EatChar();

        return true;
    }

    public void SkipComment()
    {
        while (!IsAtEndOfSource && Peek() != '\n')
        {
            EatChar();
        }
    }

    public string MunchStringLiteral()
    {
        // Happy Path
        while (Peek() != '"' && !IsAtEndOfSource)
        {
            if (Peek() == '\n')
            {
                LineNumber++;
            }
            
            EatChar();
        }

        if (IsAtEndOfSource)
        {
            errorManager.Error(LineNumber, "Unterminated string.");
        }

        if (Peek() == '"')
        {
            EatChar();
        }

        return source[Start..Current].Trim('"');
    }

    public double MunchNumberLiteral()
    {
        while (!IsAtEndOfSource && char.IsDigit(Peek()))
        {
            EatChar();
        }

        if (!IsAtEndOfSource && Peek() == '.' && char.IsDigit(PeekNext()))
        {
            EatChar();
            
            while (!IsAtEndOfSource && char.IsDigit(Peek()))
            {
                EatChar();
            }
        }
        
        return double.Parse(source[Start..Current]);
    }

    // var username = "cluciani";
    public void MunchIdentifierOrReservedWord()
    {
        while (!IsAtEndOfSource && IsLetterOrUnderscore(Peek()))
        {
            EatChar();
        }
        
        bool IsLetterOrUnderscore(char c) => char.IsLetter(c) || c == '_';
    }
}