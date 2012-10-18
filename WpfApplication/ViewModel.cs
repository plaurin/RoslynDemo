using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace WpfApplication
{
	public class ViewModel : NotifiableBase
	{
		private string formula;
		private string formulaError;
		private ScriptEvaluator scriptEngine;

		public ViewModel(IEnumerable<City> cities, ScriptEvaluator scriptEngine)
		{
			this.Cities = cities;
			this.scriptEngine = scriptEngine;
		}

		public IEnumerable<City> Cities { get; private set; }

		public string Formula 
		{
			get { return this.formula; }
			set
			{
				if (this.formula != value)
				{
					this.formula = value;
					this.OnFormulaChanged();
					this.OnPropertyChanged();
				}
			}
		}

		public string FormulaError
		{
			get { return this.formulaError; }
			set
			{
				if (this.formulaError != value)
				{
					this.formulaError = value;
					this.OnPropertyChanged();
				}
			}
		}

		private void OnFormulaChanged()
		{
			try
			{
				foreach (var city in this.Cities)
				{
					var variableDeclarations = new List<string>
					{
						this.scriptEngine.CreateVarDeclaration(() => city.Name),
						this.scriptEngine.CreateVarDeclaration(() => city.Country),
						this.scriptEngine.CreateVarDeclaration(() => city.Forecast),
						this.scriptEngine.CreateVarDeclaration(() => city.Temp)
					};

					city.Calc = scriptEngine.Evaluate(variableDeclarations, this.Formula);
				}

				this.FormulaError = string.Empty;
			}
			catch (Exception ex)
			{
				this.FormulaError = ex.Message;
			}
		}
	}
}
