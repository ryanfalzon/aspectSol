using System.Text;

namespace AspectSol.Lib.Domain.AST;

public abstract class Node
{
    public override abstract string ToString();

    private StringBuilder indentation;

    protected string GetIndentation()
    {
        if (indentation == null)
        {
            indentation = new();
        }

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