namespace LoxSharp;

public interface IVisitor<out T>
{
   T VisitBinary(Binary expr);
   T VisitGrouping(Grouping expr);
   T VisitLiteral(Literal expr);
   T VisitUnary(Unary expr);
}

public abstract record Expr
{
   public abstract T Accept<T>(IVisitor<T> visitor);
}

public record Binary : Expr
{
   public required Expr Left { get; init; }
   public required Token Operator { get; init; }
   public required Expr Right { get; init; }

   public override T Accept<T>(IVisitor<T> visitor)
   {
      return visitor.VisitBinary(this);
   }
}

public record Grouping : Expr
{
   public required Expr Expression { get; init; }

   public override T Accept<T>(IVisitor<T> visitor)
   {
      return visitor.VisitGrouping(this);
   }
}

public record Literal : Expr
{
   public required object Value { get; init; }

   public override T Accept<T>(IVisitor<T> visitor)
   {
      return visitor.VisitLiteral(this);
   }
}

public record Unary : Expr
{
   public required Token Operator { get; init; }
   public required Expr Right { get; init; }

   public override T Accept<T>(IVisitor<T> visitor)
   {
      return visitor.VisitUnary(this);
   }
}
