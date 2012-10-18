using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Roslyn.Compilers;
using Roslyn.Compilers.Common;
using Roslyn.Compilers.CSharp;
using Roslyn.Services;
using Roslyn.Services.Editor;

namespace CodeIssueAndQuickFix
{
	[ExportCodeIssueProvider("CodeIssue", LanguageNames.CSharp)]
	class DefaultCaseProvider : ICodeIssueProvider
	{
		public IEnumerable<CodeIssue> GetIssues(IDocument document, CommonSyntaxNode node, CancellationToken cancellationToken)
		{
			var switchNode = (SwitchStatementSyntax)node;

			var switchSelectionNodes = switchNode.ChildNodes().OfType<SwitchSectionSyntax>();

			if (!switchSelectionNodes
				.Any(n => n.ChildNodes().OfType<SwitchLabelSyntax>()
					.Any(l => l.Kind == SyntaxKind.DefaultSwitchLabel)))
			{
				var message = "Missing default case!";
				yield return new CodeIssue(CodeIssueKind.Error, switchNode.SwitchKeyword.Span, message,
					new DefaultCaseCodeAction(document, switchNode));
					
				// Todo: How to break the build?
			}
		}

		public IEnumerable<Type> SyntaxNodeTypes
		{
			get
			{
				yield return typeof(SwitchStatementSyntax);
			}
		}

		#region Unimplemented ICodeIssueProvider members

		public IEnumerable<CodeIssue> GetIssues(IDocument document, CommonSyntaxToken token, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<int> SyntaxTokenKinds
		{
			get
			{
				return null;
			}
		}

		#endregion
	}
}
