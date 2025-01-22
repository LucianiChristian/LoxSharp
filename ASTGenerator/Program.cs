if (args.Length != 1)
{
    Console.WriteLine("Usage: ASTGenerator <output directory>");
    Environment.Exit(64);
}

var outputDir = args[0];

DefineAst(outputDir, "Expr", 
    [
        "Binary   : Expr Left, Token Operator, Expr Right",
        "Grouping : Expr Expression",
        "Literal  : object Value",
        "Unary    : Token Operator, Expr Right" 
    ]
);

void DefineAst(string outputDir, string baseName, IReadOnlyList<string> types)
{
    var path = $"{outputDir}/{baseName}.cs";

    List<string> lines = [];
    
    lines.Add("namespace LoxSharp;");
    
    lines.Add("\npublic interface IVisitor<out T>");
    lines.Add("{");
    foreach (var typeDef in types)
    {
        var className = typeDef.Split(":")[0].Trim();
        lines.Add($"   T Visit{className}({className} expr);");
    }
    lines.Add("}");
    
    lines.Add("\npublic abstract record Expr");
    lines.Add("{");
    lines.Add("   public abstract T Accept<T>(IVisitor<T> visitor);");
    lines.Add("}");

    foreach (var typeDef in types)
    {
        var className = typeDef.Split(":")[0].Trim();
        var properties = typeDef.Split(":")[1].Trim().Split(", ");
        
        lines.Add($"\npublic record {className} : Expr");
        lines.Add("{");
        foreach (var prop in properties)
        {
            lines.Add($"   public required {prop}" + " { get; init; }");
        }
        
        lines.Add("\n   public override T Accept<T>(IVisitor<T> visitor)");
        lines.Add("   {");
        lines.Add($"      return visitor.Visit{className}(this);");
        lines.Add("   }");
        
        lines.Add("}");
    }
    
    File.WriteAllLines(path, lines);
}