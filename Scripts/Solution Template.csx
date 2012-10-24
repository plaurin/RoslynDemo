#r "Roslyn.Services"

using System;
using System.IO;
using Roslyn.Services;

// This demo is not working because of not implemented portions of Roslyn as of the September 2012 (CTP 3)
CreateSolution(@"Toto.sln");

void GetUserInput()
{
	Console.Write("Name the new solution: ");
	var input = Console.ReadLine();

	var solutionFile = Path.ChangeExtension(input, "sln");
	var solutionPath = Environment.CurrentDirectory;

	Console.WriteLine("A new solution file {0} will be created in folder {1}", solutionFile, solutionPath);
	Console.Write("Proceed? (Y/N): ");

	var inputKey = Console.ReadKey();
	if (inputKey.Key == ConsoleKey.Y)
	{
		Console.WriteLine("Go!");
		CreateSolution(Path.Combine(solutionPath, solutionFile));
	}
}

void CreateSolution(string solutionFile)
{
	CreateEmptySolution(solutionFile);
	var workspace = Workspace.LoadSolution(solutionFile);

	var newProject = workspace.CurrentSolution.AddCSharpProject("Test.Domain", "Test.Domain");

	workspace.ApplyChanges(workspace.CurrentSolution, newProject.Solution);
}

void CreateEmptySolution(string solutionFile)
{
	File.Delete(solutionFile);
	File.AppendAllText(solutionFile, @"
Microsoft Visual Studio Solution File, Format Version 12.00

# Visual Studio 2012
Global
EndGlobal");
}