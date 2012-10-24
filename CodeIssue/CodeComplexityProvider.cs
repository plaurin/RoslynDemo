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
	class CodeComplexityProvider : ICodeIssueProvider
	{
		public IEnumerable<CodeIssue> GetIssues(IDocument document, CommonSyntaxNode node, CancellationToken cancellationToken)
		{
			var methodNode = (MethodDeclarationSyntax)node;

			// Code Complexity
			var nodes = methodNode.DescendantNodes();

			var totalCount = nodes.Count();
			var switchSectionCount = nodes.OfType<SwitchSectionSyntax>().Count();
			var loopCount = (nodes.OfType<ForEachStatementSyntax>().Count()
				+ nodes.OfType<WhileStatementSyntax>().Count()
				+ nodes.OfType<ForStatementSyntax>().Count());
			
			var ifCount = nodes.OfType<IfStatementSyntax>().Count();
			var elseifCount = nodes.OfType<ElseClauseSyntax>()
				.Where(x => typeof(IfStatementSyntax).IsAssignableFrom(x.Statement.GetType()))
				.Count();

			var switchBranchs = switchSectionCount;
			var loopBranchs = loopCount * 2;
			var conditionalBranchs = ifCount * 2 - elseifCount;

			var complexity = switchBranchs + loopBranchs + conditionalBranchs;

			var message = string.Format("{0} complexity: {1} switch branchs, {2} for/foreach/while branchs, {3} if/else branchs, {4} total nodes",
				complexity, switchBranchs, loopBranchs, conditionalBranchs, totalCount);

			if (complexity < 10)
				yield return new CodeIssue(CodeIssueKind.Info, methodNode.Identifier.Span, message);
			else
				yield return new CodeIssue(CodeIssueKind.Warning, methodNode.Identifier.Span, message);

			// Code Length
			var methodLineSpan = methodNode.SyntaxTree.GetLineSpan(methodNode.Body.Span, true);

			var startLine = methodLineSpan.StartLinePosition.Line + 1;
			var endLine = methodLineSpan.EndLinePosition.Line + 1;
			var numberOfLines = endLine - startLine + 1;

			var msg = string.Format("Line {0} to line {1}: {2} total lines", startLine, endLine, numberOfLines);

			if (numberOfLines < 30)
				yield return new CodeIssue(CodeIssueKind.Info, methodNode.Identifier.Span, msg);
			else
				yield return new CodeIssue(CodeIssueKind.Warning, methodNode.Identifier.Span, msg);
		}

		public IEnumerable<Type> SyntaxNodeTypes
		{
			get
			{
				yield return typeof(MethodDeclarationSyntax);
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
