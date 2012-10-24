using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Roslyn.Compilers;
using Roslyn.Compilers.CSharp;
using Roslyn.Services;
using Roslyn.Services.Shared.Utilities;

namespace CodeIssueAndQuickFix
{
    public class DefaultCaseCodeAction : ICodeAction
    {
        private IDocument document;
        private Roslyn.Compilers.CSharp.SwitchStatementSyntax switchNode;

        public DefaultCaseCodeAction(IDocument document, Roslyn.Compilers.CSharp.SwitchStatementSyntax switchNode)
        {
            this.document = document;
            this.switchNode = switchNode;
        }
        public string Description
        {
            get { return "Add default case"; }
        }

        public CodeActionEdit GetEdit(CancellationToken cancellationToken)
		{
            var leadingTrivia = this.switchNode.Sections.Last().GetLeadingTrivia();

            var statement = Syntax.ThrowStatement(
                Syntax.ObjectCreationExpression(
                    Syntax.IdentifierName("InvalidOperationException").WithLeadingTrivia(Syntax.Space),
                    Syntax.ArgumentList(),
                    null).WithLeadingTrivia(Syntax.Space))
                .WithLeadingTrivia(Syntax.Space);

            var switchSection = Syntax.SwitchSection(
                Syntax.SwitchLabel(SyntaxKind.DefaultSwitchLabel).WithLeadingTrivia(leadingTrivia),
                statement.WithTrailingTrivia(Syntax.CarriageReturnLineFeed));

            var newSwitchSection = this.switchNode.AddSections(switchSection);

            // Replace the old local declaration with the new local declaration.
            var oldRoot = document.GetSyntaxRoot(cancellationToken);
            var newRoot = oldRoot.ReplaceNode(this.switchNode, newSwitchSection);

            return new CodeActionEdit(document.UpdateSyntaxRoot(newRoot));
        }
    }
}
