<Query Kind="Program">
  <Reference>F:\projects_csharp\IanAutomation\bin\Debug\net7.0\IanAutomation.dll</Reference>
  <Namespace>IanAutomation</Namespace>
</Query>

// Do TheAutomationChallenge

void Main()
{
	TheAutomationChallenge Page = null;
	try
	{
		// read the excel file
		DataTable CompanyData = ReadCSV(@"F:\projects_csharp\the-automation-challenge\challenge.csv");
		
		// navigate to TheAutomationChallenge
		Page = new TheAutomationChallenge();

		// complete the challenge
		Page.Start();
		bool reCAPTCHASeen = false;

//		Thread.Sleep(5000);
		foreach(DataRow row in CompanyData.Rows)
		{
			try
			{
				Page.EIN = row["employer_identification_number"].ToString();
				Page.CompanyName = row["company_name"].ToString();
				Page.Sector = row["sector"].ToString();
				Page.Address = row["company_address"].ToString();
				Page.AutomationTool = row["automation_tool"].ToString();
				Page.AnnualSaving = row["annual_automation_saving"].ToString();
				Page.Date = row["date_of_first_project"].ToString();
			}
			catch (Exception e)
			{ 
				Console.WriteLine(e.Message);
				Thread.Sleep(20000);
			}
			Page.Submit();
			if (!reCAPTCHASeen)
			{
				try
				{
					Thread.Sleep(1000);
					Page.ClickReCAPTCHA();
					Thread.Sleep(2000);
				}
				catch { }
			}
		}
		Console.WriteLine("done");
	}
	catch (Exception e)
	{
		Console.WriteLine(e.Message);
	}
	finally
	{
		Thread.Sleep(10000);
		if (Page != null)
			Page.Shutdown();
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




