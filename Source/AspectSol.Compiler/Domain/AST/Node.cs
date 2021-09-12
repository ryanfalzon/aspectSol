using System.Text;

namespace AspectSol.Compiler.Domain.AST
{
    public abstract class Node
    {
        public override abstract string ToString();

        private StringBuilder indentation;

        protected string GetIndentation()
        {
            if(indentation == null)
            {
                indentation = new StringBuilder();
            }

            return indentation.ToString();
        }

        protected void IncreaseIndentation()
        {
            indentation = indentation == null ? new StringBuilder() : indentation.Append('\t');
        }

        protected void DecreaseIndentation()
        {
            indentation = indentation == null ? new StringBuilder() : indentation.Remove(indentation.Length, 1);
        }
    }
}