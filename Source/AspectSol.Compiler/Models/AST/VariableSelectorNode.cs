namespace AspectSol.Compiler.Models.AST
{
    public class VariableSelectorNode : SelectorNode
    {
        public VariableNode VariableSyntax { get; set; }

        public SelectorNode VariableType { get; set; }

        public SelectorNode VariableLocation { get; set; }

        public SelectorNode VariableNameSelector { get; set; }

        public VariableDecoratorNode VariableDecorator { get; set; }

        public override void Visit()
        {
            throw new System.NotImplementedException();
        }
    }
}