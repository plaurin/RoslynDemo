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
            Console.WriteLine(""Hello, World!"");
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
			//Console.WriteLine(string.Join("", Enumerable.Repeat(" ", indent)) + dgmlNode);
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
			//Console.WriteLine(string.Join("", Enumerable.Repeat(" ", indent)) + dgmlNode);
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
			//Console.WriteLine(string.Join("", Enumerable.Repeat(" ", indent)) + dgmlNode);
			PrintNode(dgmlNode, indent);

			return dgmlNode;
		}

		private static void PrintNode(DgmlNode dgmlNode, int indent)
		{
			Console.WriteLine(string.Join("", Enumerable.Repeat(" ", indent)) + dgmlNode);
		}

		private static void WriteAndOpenDgml(DgmlNode dgmlNodes)
		{
			//var fileName = Path.ChangeExtension(Path.GetTempFileName(), "dgml");
			var fileName = @"..\..\syntax.dgml";

			var document = new XDocument();
			XNamespace ns = "http://schemas.microsoft.com/vs/2009/dgml";

			document.Add(new XElement(ns + "DirectedGraph",
				new XElement(ns + "Nodes",
					new XElement(ns + "Node", 
						new XAttribute("Id", "Toto"))),
				new XElement(ns + "Links")));

			document.Save(fileName);
		}
	}
}
