using System.Text;

namespace AspectSol.Lib.Domain.AST;

public abstract class Node
{
    /// <summary>
    /// Used to output AST node into an XML representation
    /// </summary>
    /// <returns>Encoded XML version of AST node</returns>
    public abstract override string ToString();

    private StringBuilder _indentation;

    protected string GetIndentation()
    {
        _indentation ??= new();

        return _indentation.ToString();
    }

    protected void IncreaseIndentation()
    {
        _indentation = _indentation == null ? new() : _indentation.Append('\t');
    }

    protected void DecreaseIndentation()
    {
        _indentation = _indentation == null ? new() : _indentation.Remove(_indentation.Length, 1);
    }
}