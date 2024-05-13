<Query Kind="Program">
  <Reference>&lt;ProgramFilesX86&gt;\AutoIt3\AutoItX\AutoItX3.Assembly.dll</Reference>
  <Reference>&lt;ProgramFilesX86&gt;\AutoIt3\AutoItX\AutoItX3_x64.dll</Reference>
  <Reference Relative="..\bin\Debug\net7.0\IanAutomation.dll">F:\projects_csharp\IanAutomation\bin\Debug\net7.0\IanAutomation.dll</Reference>
  <Namespace>IanAutomation</Namespace>
  <Namespace>IanAutomation.Apps</Namespace>
  <Namespace>System.Windows.Forms</Namespace>
</Query>

void Main()
{
	try
	{
		Notepad np = new Notepad();
	}
	catch (Exception e)
	{
		Console.WriteLine(e.Message);
		if (e.InnerException != null)
			Console.WriteLine(e.InnerException.Message);
	}
	finally
	{
		Thread.Sleep(2000);
	}
}

// You can define other methods, fields, classes and namespaces here
public static int CompareCreationTimes(FileInfo A, FileInfo B)
{
	return A.CreationTime.CompareTo(B.CreationTime);
}
