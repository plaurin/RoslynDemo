using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Roslyn.Compilers.CSharp;

namespace SyntaxAnalysis
{
	class Program
	{
		static void Main(string[] args)
		{
			SyntaxTree tree = SyntaxTree.ParseText(
@"using System;
using System.Collections;
using System.Linq;
using System.Text;
 
namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            // Console.WriteLine(""Hello, World!"");
			switch (args.Length)
			{
				case 1: break;
				default: break;
			}
        }
    }
}");

			var root = (CompilationUnitSyntax)tree.GetRoot();
			var dgmlNode = ProcessNode(root);

			Console.ReadLine();

			WriteAndOpenDgml(dgmlNode);
		}

		private static DgmlNode ProcessNode(SyntaxNode node, int indent = 0)
		{
			var dgmlNode = DgmlNode.Create(node);
			PrintNode(dgmlNode, indent);

			foreach (var childNodeOrToken in node.ChildNodesAndTokens())
			{
				if (childNodeOrToken.IsNode)
					dgmlNode.AddChild(ProcessNode((SyntaxNode)childNodeOrToken, indent + 1));
				else if (childNodeOrToken.IsToken)
					dgmlNode.AddChild(ProcessToken((SyntaxToken)childNodeOrToken, indent + 1));
			}

			return dgmlNode;
		}

		private static DgmlNode ProcessToken(SyntaxToken token, int indent = 0)
		{
			var dgmlNode = DgmlNode.Create(token);
			PrintNode(dgmlNode, indent);

			foreach (var trivia in token.GetAllTrivia())
			{
				dgmlNode.AddChild(ProcessTrivia(trivia, indent + 1));
			}

			return dgmlNode;
		}

		private static DgmlNode ProcessTrivia(SyntaxTrivia trivia, int indent = 0)
		{
			var dgmlNode = DgmlNode.Create(trivia);
			PrintNode(dgmlNode, indent);

			return dgmlNode;
		}

		private static void PrintNode(DgmlNode dgmlNode, int indent)
		{
			Console.WriteLine(string.Join("", Enumerable.Repeat(" ", indent)) + dgmlNode);
		}

		private static void WriteAndOpenDgml(DgmlNode dgmlNodes)
		{
			var fileName = @"..\..\syntax.dgml";

			var document = new XDocument();
			XNamespace ns = "http://schemas.microsoft.com/vs/2009/dgml";

			var nodes = new XElement(ns + "Nodes");
			var links = new XElement(ns + "Links");
			var styles = new XElement(ns + "Styles");

			CreateNodes(ns, nodes, dgmlNodes);
			CreateLinks(ns, links, dgmlNodes);
			CreateStyles(ns, styles);

			document.Add(new XElement(ns + "DirectedGraph",	nodes, links, styles));

			document.Save(fileName);
		}

		private static void CreateNodes(XNamespace ns, XElement nodes, DgmlNode dgmlNode)
		{
			XElement node;
			if (dgmlNode.Category == "SyntaxToken")
			{
				node = new XElement(ns + "Node",
					new XAttribute("Id", dgmlNode.Id),
					new XAttribute("Category", dgmlNode.Category),
					new XAttribute("Label", dgmlNode.Label));
			}
			else
			{
				node = new XElement(ns + "Node",
					new XAttribute("Id", dgmlNode.Id),
					new XAttribute("Category", dgmlNode.Category),
					new XAttribute("Label", dgmlNode.Kind));
			}

			nodes.Add(node);

			foreach (var childNode in dgmlNode.Childs)
			{
				CreateNodes(ns, nodes, childNode);
			}
		}

		private static void CreateLinks(XNamespace ns, XElement links, DgmlNode dgmlNode)
		{
			foreach (var childNode in dgmlNode.Childs)
			{
				var link = new XElement(ns + "Link",
					new XAttribute("Source", dgmlNode.Id),
					new XAttribute("Target", childNode.Id));

				links.Add(link);

				CreateLinks(ns, links, childNode);
			}
		}

		private static void CreateStyles(XNamespace ns, XElement styles)
		{
			styles.Add(
				new XElement(ns + "Style",
					new XAttribute("TargetType", "Node"),
					new XAttribute("GroupLabel", "SyntaxToken"),
					new XAttribute("ValueLabel", "True"),
					new XElement(ns + "Condition",
						new XAttribute("Expression", "HasCategory('SyntaxToken')")),
					new XElement(ns + "Setter",
						new XAttribute("Property", "Background"),
						new XAttribute("Value", "#FF44EE44"))),
				new XElement(ns + "Style",
					new XAttribute("TargetType", "Node"),
					new XAttribute("GroupLabel", "SyntaxNode"),
					new XAttribute("ValueLabel", "True"),
					new XElement(ns + "Condition",
						new XAttribute("Expression", "HasCategory('SyntaxNode')")),
					new XElement(ns + "Setter",
						new XAttribute("Property", "Background"),
						new XAttribute("Value", "#FF4444EE"))));
		}
	}
}
