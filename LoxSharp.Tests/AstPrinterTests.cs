using FluentAssertions;

namespace LoxSharp.Tests;

public class AstPrinterTests 
{
    [Fact]
    public void GeneratesCorrectly()
    {
        // Arrange
        var sut = new AstPrinter();

        var expressionTree = new Binary
        {
            Left = new Unary
            {
                Operator = new Token(TokenType.MINUS, "-", null, 1),
                Right = new Literal
                {
                    Value = 123
                }
            },
            Operator = new Token(TokenType.STAR, "*", null, 1),
            Right = new Grouping
            {
                Expression = new Literal
                {
                    Value = 45.67
                }
            }
        };

        // Act
        var result = sut.Print(expressionTree);

        // Assert
        result.Should().Be("(* (- 123) (group 45.67))");
    }
}