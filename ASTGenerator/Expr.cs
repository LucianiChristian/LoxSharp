using LoxSharp;

namespace ASTGenerator;

public abstract record Expr {}

public record Binary : Expr
{
   public required Expr Left { get; init; }
   public required Token Operator { get; init; }
   public required Expr Right { get; init; }
}

public record Grouping : Expr
{
   public required Expr Expression { get; init; }
}

public record Literal : Expr
{
   public required object Value { get; init; }
}

public record Unary : Expr
{
   public required Token Operator { get; init; }
   public required Expr Right { get; init; }
}
