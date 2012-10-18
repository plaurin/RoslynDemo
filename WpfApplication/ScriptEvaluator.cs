using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Roslyn.Scripting.CSharp;

namespace WpfApplication
{
	public class ScriptEvaluator
	{
		public object Evaluate(IEnumerable<string> variableDeclarations, string formula)
		{
			var engine = new ScriptEngine();
			engine.AddReference("System");

			var usings = "using System;";
			var init = string.Join("", variableDeclarations);

			var code = usings + Environment.NewLine + init + Environment.NewLine + formula;

			var session = engine.CreateSession();
			return session.Execute(code);
		}

		public string CreateVarDeclaration<T>(Expression<Func<T>> func)
		{
			var type = func.ReturnType;
			var member = ((MemberExpression)func.Body).Member;
			var value = func.Compile().Invoke();

			string valueText;
			if (value == null) valueText = "null";
			else if (type == typeof(string)) valueText = string.Format("\"{0}\"", value);
			else valueText = value.ToString();

			return string.Format("{0} {1} = {2};", type.Name, member.Name, valueText);
		}
	}
}
