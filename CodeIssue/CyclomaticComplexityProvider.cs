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
	class CyclomaticCOmplexityProvider : ICodeIssueProvider
	{
		public IEnumerable<CodeIssue> GetIssues(IDocument document, CommonSyntaxNode node, CancellationToken cancellationToken)
		{
			var tokens = from nodeOrToken in node.ChildNodesAndTokens()
						 where nodeOrToken.IsToken
						 select nodeOrToken.AsToken();

			foreach (var token in tokens)
			{
				var tokenText = token.ToString();

				if (tokenText.Contains('a'))
				{
					var issueDescription = string.Format("'{0}' contains the letter 'a'", tokenText);
					yield return new CodeIssue(CodeIssueKind.Warning, token.Span, issueDescription);
				}
			}
		}

		public IEnumerable<Type> SyntaxNodeTypes
		{
			get
			{
				yield return typeof(SyntaxNode);
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
