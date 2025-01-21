using FluentAssertions;

namespace LoxSharp.Tests;

public class ScannerTests
{
    [Fact]
    public void HandlesSingleCharacterLexemes() 
    {
        // Arrange
        var source = "() { } , .- +;";
        var errorManager = new ErrorManager();
        var sut = new Scanner(source, errorManager);

        // Act
        var tokens = sut.ScanTokens();
        
        // Assert
        // EOF token is implicit, and makes the count 10.
        errorManager.HasError.Should().BeFalse();
        tokens.Should().HaveCount(10);
        tokens[0].Should().BeEquivalentTo(new Token(TokenType.LEFT_PAREN, "(", null, 1));
        tokens[1].Should().BeEquivalentTo(new Token(TokenType.RIGHT_PAREN, ")", null, 1));
        tokens[2].Should().BeEquivalentTo(new Token(TokenType.LEFT_BRACE, "{", null, 1));
        tokens[3].Should().BeEquivalentTo(new Token(TokenType.RIGHT_BRACE, "}", null, 1));
        tokens[4].Should().BeEquivalentTo(new Token(TokenType.COMMA, ",", null, 1));
        tokens[5].Should().BeEquivalentTo(new Token(TokenType.DOT, ".", null, 1));
        tokens[6].Should().BeEquivalentTo(new Token(TokenType.MINUS, "-", null, 1));
        tokens[7].Should().BeEquivalentTo(new Token(TokenType.PLUS, "+", null, 1));
        tokens[8].Should().BeEquivalentTo(new Token(TokenType.SEMICOLON, ";", null, 1));
        tokens[9].Should().BeEquivalentTo(new Token(TokenType.EOF, string.Empty, null, 1));
    }
    
    [Fact]
    public void HandlesOperators()
    {
        // Arrange
        var source = "!*+-/=<> >= <= != ==";
        var errorManager = new ErrorManager();
        var sut = new Scanner(source, errorManager);
        
        // Act
        var tokens = sut.ScanTokens();
        
        // Assert
        // Check the total token count (including EOF)
        errorManager.HasError.Should().BeFalse();
        tokens.Should().HaveCount(13);
        tokens[0].Should().BeEquivalentTo(new Token(TokenType.BANG, "!", null, 1));
        tokens[1].Should().BeEquivalentTo(new Token(TokenType.STAR, "*", null, 1));
        tokens[2].Should().BeEquivalentTo(new Token(TokenType.PLUS, "+", null, 1));
        tokens[3].Should().BeEquivalentTo(new Token(TokenType.MINUS, "-", null, 1));
        tokens[4].Should().BeEquivalentTo(new Token(TokenType.SLASH, "/", null, 1));
        tokens[5].Should().BeEquivalentTo(new Token(TokenType.EQUAL, "=", null, 1));
        tokens[6].Should().BeEquivalentTo(new Token(TokenType.LESS, "<", null, 1));
        tokens[7].Should().BeEquivalentTo(new Token(TokenType.GREATER, ">", null, 1));
        tokens[8].Should().BeEquivalentTo(new Token(TokenType.GREATER_EQUAL, ">=", null, 1));
        tokens[9].Should().BeEquivalentTo(new Token(TokenType.LESS_EQUAL, "<=", null, 1));
        tokens[10].Should().BeEquivalentTo(new Token(TokenType.BANG_EQUAL, "!=", null, 1));
        tokens[11].Should().BeEquivalentTo(new Token(TokenType.EQUAL_EQUAL, "==", null, 1));
        tokens[12].Should().BeEquivalentTo(new Token(TokenType.EOF, string.Empty, null, 1));
    }
    
    [Fact]
    public void HandlesComments()
    {
        // Arrange
        var source = "/ //";
        var errorManager = new ErrorManager();
        var sut = new Scanner(source, errorManager);
        
        // Act
        var tokens = sut.ScanTokens();
        
        // Assert
        // Check the total token count (including EOF)
        errorManager.HasError.Should().BeFalse();
        tokens.Should().HaveCount(2);
        tokens[0].Should().BeEquivalentTo(new Token(TokenType.SLASH, "/", null, 1));
        tokens[1].Should().BeEquivalentTo(new Token(TokenType.EOF, string.Empty, null, 1));
    }

    [Fact]
    public void IgnoresWhitespace()
    {
        // Arrange
        var source = "\r \t \n";
        var errorManager = new ErrorManager();
        var sut = new Scanner(source, errorManager);
        
        // Act
        var tokens = sut.ScanTokens();
        
        // Assert
        // Check the total token count (including EOF)
        errorManager.HasError.Should().BeFalse();
        tokens.Should().HaveCount(1);
        tokens[0].Should().BeEquivalentTo(new Token(TokenType.EOF, string.Empty, null, 2));
    }

    [Fact]
    public void HandlesStringLiterals()
    {
        // Arrange
        var source = 
            """
            "Thomas
            Pynchon"         
            """;
        var errorManager = new ErrorManager();
        var sut = new Scanner(source, errorManager);
        
        // Act
        var tokens = sut.ScanTokens();
        
        // Assert
        // Check the total token count (including EOF)
        errorManager.HasError.Should().BeFalse();
        tokens.Should().HaveCount(2);
        tokens[0].Should().BeEquivalentTo(new Token(TokenType.STRING, "\"Thomas\r\nPynchon\"", "Thomas\r\nPynchon", 2));
        tokens[1].Should().BeEquivalentTo(new Token(TokenType.EOF, string.Empty, null, 2));
    }
    
    [Fact]
    public void HandleNumberLiteralWithoutDecimalPoint()
    {
         // Arrange
         var source = "123";
         var errorManager = new ErrorManager();
         var sut = new Scanner(source, errorManager);
         
         // Act
         var tokens = sut.ScanTokens();
         
         // Assert
         // Check the total token count (including EOF)
         errorManager.HasError.Should().BeFalse();
         tokens.Should().HaveCount(2);
         tokens[0].Should().BeEquivalentTo(new Token(TokenType.NUMBER, "123", 123, 1));
         tokens[1].Should().BeEquivalentTo(new Token(TokenType.EOF, string.Empty, null, 1));       
    }

    [Fact]
    public void HandleNumberLiteralWithDecimalPoint()
    {
         // Arrange
         var source = "123.12";
         var errorManager = new ErrorManager();
         var sut = new Scanner(source, errorManager);
         
         // Act
         var tokens = sut.ScanTokens();
         
         // Assert
         // Check the total token count (including EOF)

         errorManager.HasError.Should().BeFalse();
         tokens.Should().HaveCount(2);
         tokens[0].Should().BeEquivalentTo(new Token(TokenType.NUMBER, "123.12", 123.12, 1));
         tokens[1].Should().BeEquivalentTo(new Token(TokenType.EOF, string.Empty, null, 1));       
    }
    
    [Fact]
    public void HandleNumberLiteralWhileSkippingAFollowingDecimalPointIfNoNumberFollows()
    {
          // Arrange
          var source =
              """
              12. "hi"
              """;
          var errorManager = new ErrorManager();
          var sut = new Scanner(source, errorManager);
          
          // Act
          var tokens = sut.ScanTokens();
          
          // Assert
          errorManager.HasError.Should().BeFalse();
          tokens.Should().HaveCount(4);
          tokens[0].Should().BeEquivalentTo(new Token(TokenType.NUMBER, "12", 12, 1));
          tokens[1].Should().BeEquivalentTo(new Token(TokenType.DOT, ".", null, 1));
          tokens[2].Should().BeEquivalentTo(new Token(TokenType.STRING, "\"hi\"", "hi", 1));       
          tokens[3].Should().BeEquivalentTo(new Token(TokenType.EOF, string.Empty, null, 1));       
    }
    
    [Fact]
    public void HandlesIdentifiers() 
    {
        // Arrange
        var source = "variableName";
        var errorManager = new ErrorManager();
        
        var sut = new Scanner(source, errorManager);

        // Act
        var tokens = sut.ScanTokens();

        // Assert
        tokens.Should().HaveCount(2);
        tokens[0].Should().BeEquivalentTo(new Token(TokenType.IDENTIFIER, "variableName", null, 1));
        tokens[1].Should().BeEquivalentTo(new Token(TokenType.EOF, string.Empty, null, 1));
    }
}