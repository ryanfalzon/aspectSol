using System.Text;

namespace AspectSol.Lib.Domain.AST;

public abstract class Node
{
    public abstract override string ToString();

    private StringBuilder indentation;

    protected string GetIndentation()
    {
        indentation ??= new();

        return indentation.ToString();
    }

    protected void IncreaseIndentation()
    {
        indentation = indentation == null ? new() : indentation.Append('\t');
    }

    protected void DecreaseIndentation()
    {
        indentation = indentation == null ? new() : indentation.Remove(indentation.Length, 1);
    }
}