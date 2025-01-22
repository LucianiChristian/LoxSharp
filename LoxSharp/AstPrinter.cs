namespace LoxSharp;

public class AstPrinter : IVisitor<string>
{
    public string Print(Expr expressionTree)
    {
        return expressionTree.Accept(this);
    }
    
    public string VisitBinary(Binary expr)
    {
        return $"({expr.Operator.Lexeme} {expr.Left.Accept(this)} {expr.Right.Accept(this)})";
    }

    public string VisitGrouping(Grouping expr)
    {
        return $"(group {expr.Expression.Accept(this)})";
    }

    public string VisitLiteral(Literal expr)
    {
        return expr.Value.ToString() ?? "nil";
    }

    public string VisitUnary(Unary expr)
    {
        return $"({expr.Operator.Lexeme} {expr.Right.Accept(this)})";
    }
}