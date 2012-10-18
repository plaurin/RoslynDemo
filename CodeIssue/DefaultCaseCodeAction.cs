using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Roslyn.Compilers.CSharp;
using Roslyn.Services;

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
			return null;
			//var switchSection = new SwitchSectionSyntax();
			//var newSwitchNode = this.switchNode.AddSections();
		}
	}
}
