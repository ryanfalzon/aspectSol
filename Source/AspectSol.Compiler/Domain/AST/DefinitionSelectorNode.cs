using System.Collections.Generic;

namespace AspectSol.Compiler.Domain.AST
{
    public class DefinitionSelectorNode : SelectorNode
    {
        public DefinitionSyntaxNode DefinitionSyntax { get; set; }

        public List<ParameterNode> Parameters { get; set; }

        public DefinitionDecoratorNode DefinitionDecorator { get; set; }

        public override void Visit()
        {
            throw new System.NotImplementedException();
        }
    }
}