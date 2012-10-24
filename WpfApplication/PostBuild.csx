#r "System.IO.Compression"
#r "System.IO.Compression.FileSystem"

using System;
using System.IO;
using System.IO.Compression;
using System.Linq;

var zipFile = @"package.zip";
var destinationFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), zipFile);

if (File.Exists(zipFile)) File.Delete(zipFile);

var files = Directory.GetFiles(@".", "*.*", SearchOption.AllDirectories)
	.Where(x => !x.EndsWith(".zip"))
	.Where(x => !x.EndsWith(".pdb"))
	.Where(x => !x.Contains(".vshost."));

Console.WriteLine("Zipping files:");
using (var archive = ZipFile.Open(zipFile, ZipArchiveMode.Update))
{
	foreach (var file in files)
	{
		Console.WriteLine(file);
		archive.CreateEntryFromFile(file, Path.GetFileName(file));
	}
}

Console.WriteLine();
Console.WriteLine("Copying package to {0}", destinationFile);
File.Copy(zipFile, destinationFile, true);