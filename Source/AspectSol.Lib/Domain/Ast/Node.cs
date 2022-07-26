using System.Text;

namespace AspectSol.Lib.Domain.Ast;

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
        _indentation ??= new StringBuilder();

        return _indentation.ToString();
    }

    protected void IncreaseIndentation()
    {
        _indentation = _indentation == null ? new StringBuilder() : _indentation.Append('\t');
    }

    protected void DecreaseIndentation()
    {
        _indentation = _indentation == null ? new StringBuilder() : _indentation.Remove(_indentation.Length, 1);
    }
}