namespace LoxSharp;

/// <summary>
/// Represents a valid token in the grammar of the language.
/// </summary>
/// <param name="Type">The type of token.</param>
/// <param name="Lexeme">The exact string value extracted during scanning.</param>
/// <param name="Literal">The literal value of the token.</param>
/// <param name="LineNumber">The line number at which the token was scanned.</param>
public record Token (
    TokenType Type, 
    string Lexeme,
    object? Literal,
    int LineNumber
);