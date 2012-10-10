using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Roslyn.Compilers.CSharp;

namespace SyntaxAnalysis
{
	internal class DgmlNode
	{
		private static int nextId;

		private List<DgmlNode> childNodes;

		public DgmlNode(string label, string category, string kind, string span)
		{
			this.Id = nextId++;
			this.Label = label;
			this.Category = category;
			this.Kind = kind;
			this.Span = span;
			this.childNodes = new List<DgmlNode>();
		}

		public int Id { get; private set; }
		public string Label { get; private set; }
		public string Category { get; private set; }
		public string Kind { get; private set; }
		public string Span { get; private set; }

		public IEnumerable<DgmlNode> Childs
		{
			get { return this.childNodes; }
		}

		public override string ToString()
		{
			if (this.Label == this.Kind)
				return string.Format("{0} {1} - {2}", this.Label, this.Span, this.Category);
			else
				return string.Format("{0} {1} - {2}: {3}", this.Kind, this.Span, this.Category, this.Label);
		}

		public static DgmlNode Create(SyntaxNode node)
		{
			return new DgmlNode(node.Kind.ToString(), "SyntaxNode", node.Kind.ToString(), node.Span.ToString());
		}

		public static DgmlNode Create(SyntaxToken token)
		{
			return new DgmlNode(token.ToString(), "SyntaxToken", token.Kind.ToString(), token.Span.ToString());
		}

		public static DgmlNode Create(SyntaxTrivia trivia)
		{
			return new DgmlNode(trivia.Kind.ToString(), "SyntaxTrivia", trivia.Kind.ToString(), trivia.Span.ToString());
		}

		public void AddChild(DgmlNode childNode)
		{
			this.childNodes.Add(childNode);
		}
	}
}
