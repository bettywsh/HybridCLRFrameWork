using System;
using Microsoft.CodeAnalysis;

namespace SourceGenerator
{
    [Generator]
    public class UISourceGenerator : ISourceGenerator
    {
        /// <inheritdoc>
        public void Execute(GeneratorExecutionContext context)
        {
            //string moduleName = context.Compilation.SourceModule.Name;
            //context.AddSource("", )
        }

        public void Initialize(GeneratorInitializationContext context)
        {

        }
    }

}

file class SyntaxReceiver : ISyntaxReceiver
{
    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
        throw new NotImplementedException();
    }
}

