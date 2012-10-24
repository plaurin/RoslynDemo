A simple demo application for Roslyn CTP September 2012

You will need to install the Roslyn CTP from http://go.microsoft.com/fwlink/?LinkID=228400

List of demos:
Syntax Analysis Demo
- Set the SyntaxtAnalysis project as the startup project
- You can change the code to parse in Program.cs line 18
- Run the console application (F5)
- Close the console window
- Take a look at the syntax.dgml file in the project

Code Issue and QuickFix Demo
- Set the CodeIssueAndQuickFix project as the startup project
- Run the VS extension project (F5)
- Open or create a new project 
 - Write a method with a switch statement without a default label
 - Apply the QuickFix (Ctrl+. or click on the light bulb)
 - Write a method with a lot of ifs, whiles, fors, foreachs and switches
 - Notice the warning when complexity reach 10
 - Write a method of at least 30 lines
 - Notice the new warning

Script Files Demo
- Build the solution
- Take a look at the Output Window for Build
- A post-build event is set for the project WpfApplication to zip a copy the project's outputs
- Go to the WpfApplication project properties in the Build Events tab
- Notice the Post-build event command line field
- Open the PostBuild.csx file in the project
- This code is executed after each build of the project

ScriptEngine Demo
- Set the WpfApplication project as the startup project
- Run the WPF application (F5)
- Enter any expression in the Formula field
- Enter an expression using the grid column headers
- You can also create functions.  Enter the following in the Formula field
  int ABS(int v) { return Math.Abs(v); } ABS(-12)

C# Interactive window Demo
- Right-click on the WpfApplication project and select Reset C# Interactive from Project
- In the C# Interactive window, type:
 - var m = new MainWindow();
 - m.Show();
 - m.Topmost = true;
- Playing with the DataContext, type:
 - m.DataContext
 - var vm = (ViewModel)m.DataContext;
 - vm.Formula = "Temp * 2";
- Playing with the MainWindow, type:
 - m.Content
 - using System.Windows.Controls;
 - var sp = (StackPanel)m.Content;
 - var b = new Button();
 - b.Content = "Close!";
 - b.Click += (s, e) => m.Close();
 - sp.Children.Add(b);
- Finally click on the Close! button