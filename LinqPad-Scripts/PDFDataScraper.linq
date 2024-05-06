<Query Kind="Program">
  <Reference Relative="..\bin\Debug\net7.0\IanAutomation.dll">F:\projects_csharp\IanAutomation\bin\Debug\net7.0\IanAutomation.dll</Reference>
  <Namespace>IanAutomation</Namespace>
  <Namespace>IanAutomation.Redfin</Namespace>
  <Namespace>System.Windows.Forms</Namespace>
</Query>

void Main()
{
	string InvoiceFolderPath = @"F:\projects_uipath\Robot5_PDFDataScraper\input";
	
	//PdfHelper Pdf;
	
	try
	{
		// Check if the folder exists
        if (!Directory.Exists(InvoiceFolderPath))
        {
            Console.WriteLine("Folder does not exist.");
			return;
		}
		
		// Get the files and sort them by CreationDate
		List<FileInfo> files = Directory.GetFiles(InvoiceFolderPath).Select(path => new FileInfo(path)).ToList();
		files.Sort(CompareCreationTimes);
		
			
		// Iterate over each file in the folder
        foreach (FileInfo file in files)
        {
			using (PdfHelper Pdf = new PdfHelper(file.FullName))
			{
				Console.WriteLine($"Processing {file.Name}: {file.CreationTime}");
			}
        }
		
		// You downloaded Can Opener and you need it to read the invoice field
		
	}
	catch (Exception e)
	{
		Console.WriteLine(e.Message);
		if (e.InnerException != null)
			Console.WriteLine(e.InnerException.Message);
	}
	finally
	{
		//if (Pdf != null)
		//	Pdf.Shutdown();
		Thread.Sleep(2000);
	}
}

// You can define other methods, fields, classes and namespaces here
public static int CompareCreationTimes(FileInfo A, FileInfo B)
{
	return A.CreationTime.CompareTo(B.CreationTime);
}
