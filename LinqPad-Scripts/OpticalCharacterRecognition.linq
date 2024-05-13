<Query Kind="Program">
  <Reference Relative="..\bin\Debug\net7.0\IanAutomation.dll">F:\projects_csharp\IanAutomation\bin\Debug\net7.0\IanAutomation.dll</Reference>
  <Namespace>IanAutomation</Namespace>
</Query>

// Do TheAutomationChallenge

void Main()
{
	try
	{
		Console.WriteLine("done");
	}
	catch (Exception e)
	{
		Console.WriteLine(e.Message);
	}
	finally
	{
		Thread.Sleep(10000);
	}
}

// You can define other methods, fields, classes and namespaces here
class Company
{
	public string EID;
	public string Name;
	public string Sector;
	public string Address;
	public string AutomationTool;
	public string AnnualSaving;
	public string Date;
}

public DataTable ReadCSV(string FilePath)
{
	string[] lines = File.ReadAllLines(FilePath); // reads each line of the file into the array
	DataTable Data = new DataTable();
	string[] headers = lines[0].Split(',');
	
	foreach(string header in headers)
	{
		Data.Columns.Add(header, typeof(string));
	}
	
	for (int i=1; i<lines.Length; i++)
	{
		string[] data = lines[i].Split(',');
		DataRow row = Data.NewRow();
		for (int j=0; j<headers.Length; j++)
		{
			row[j] = data[j];
		}
		Data.Rows.Add(row);
	}
	
	return Data;
}




