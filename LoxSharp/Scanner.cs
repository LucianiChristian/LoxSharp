namespace LoxSharp;

public class Scanner(string source, ErrorManager errorManager)
{
    // List of tokens munched by the scanner.
    private readonly List<Token> _tokens = [];
    
    // Used to consume characters and manage the related pointers.
    private readonly LexemeEater _lexemeEater = new(source, errorManager);

    // Handles scanning process.
    public IReadOnlyList<Token> ScanTokens()
    {
        while (!_lexemeEater.IsAtEndOfSource)
        {
            _lexemeEater.StartNewMunch();
            
            // Scan in the next detectable token.
            // Scanning will move the current pointer. 
            ScanToken();
        }
        
        // Added to the end of the tokens to simplify the parsing process that follows.
        _tokens.Add(new Token(TokenType.EOF, "", null, _lexemeEater.LineNumber));
        
        return _tokens;
    }

    private void ScanToken()
    {
        var c = _lexemeEater.EatChar();
        
        switch (c)
        {
            case '(': AddToken(TokenType.LEFT_PAREN); break;
            case ')': AddToken(TokenType.RIGHT_PAREN); break;
            case '{': AddToken(TokenType.LEFT_BRACE); break;
            case '}': AddToken(TokenType.RIGHT_BRACE); break;
            case ',': AddToken(TokenType.COMMA); break;
            case '.': AddToken(TokenType.DOT); break;
            case '-': AddToken(TokenType.MINUS); break;
            case '+': AddToken(TokenType.PLUS); break;
            case ';': AddToken(TokenType.SEMICOLON); break;
            case '*': AddToken(TokenType.STAR); break; 
            case '/':
                if (_lexemeEater.TryEatChar('/'))
                {
                    _lexemeEater.SkipComment();
                }
                else
                {
                    AddToken(TokenType.SLASH);
                }
                break;
            case '!': AddToken(_lexemeEater.TryEatChar('=') ? TokenType.BANG_EQUAL : TokenType.BANG); break;
            case '=': AddToken(_lexemeEater.TryEatChar('=') ? TokenType.EQUAL_EQUAL: TokenType.EQUAL); break;
            case '<': AddToken(_lexemeEater.TryEatChar('=') ? TokenType.LESS_EQUAL: TokenType.LESS); break;
            case '>': AddToken(_lexemeEater.TryEatChar('=') ? TokenType.GREATER_EQUAL: TokenType.GREATER); break;
            case '"': 
                var stringValue = _lexemeEater.MunchStringLiteral();
                AddToken(TokenType.STRING, stringValue);
                break;
            case '\t':
            case '\r':
            case ' ':
                // Ignore whitespace
                break;
            case '\n':
                _lexemeEater.LineNumber++;
                break;
            default:
                if (char.IsDigit(c))
                {
                    var numericValue = _lexemeEater.MunchNumberLiteral();
                    AddToken(TokenType.NUMBER, numericValue);
                    break;
                }
                else if (char.IsLetter(c))
                {
                    _lexemeEater.MunchIdentifierOrReservedWord();
                    AddToken(TokenType.IDENTIFIER);
                    break;
                }
                
                errorManager.Error(_lexemeEater.LineNumber, "Unexpected character.");
                break;
        }
    }

    private void AddToken(TokenType tokenType, object? literalValue = null)
    {
        var lexeme = source[_lexemeEater.Start.._lexemeEater.Current];

        _tokens.Add(new Token(tokenType, lexeme, literalValue, _lexemeEater.LineNumber));
    }
}