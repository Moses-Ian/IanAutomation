<Query Kind="Program">
  <Reference>&lt;ProgramFilesX86&gt;\IronSoftware\IronOcr\lib\netstandard2.0\BitMiracle.LibTiff.NET.dll</Reference>
  <Reference>&lt;ProgramFilesX86&gt;\IronSoftware\IronOcr\runtimes\win-x64\native\eng.base.traineddata</Reference>
  <Reference>&lt;ProgramFilesX86&gt;\IronSoftware\IronOcr\runtimes\win-x64\native\eng.best.traineddata</Reference>
  <Reference>&lt;ProgramFilesX86&gt;\IronSoftware\IronOcr\runtimes\win-x64\native\eng.fast.traineddata</Reference>
  <Reference>&lt;ProgramFilesX86&gt;\IronSoftware\IronOcr\runtimes\win-x64\native\eng.user-patterns</Reference>
  <Reference>&lt;ProgramFilesX86&gt;\IronSoftware\IronOcr\runtimes\win-x64\native\eng.user-words</Reference>
  <Reference>F:\projects_csharp\IanAutomation\bin\Debug\net7.0\IanAutomation.dll</Reference>
  <Reference>&lt;ProgramFilesX86&gt;\IronSoftware\IronOcr\lib\netstandard2.0\IronOcr.dll</Reference>
  <Reference>&lt;ProgramFilesX86&gt;\IronSoftware\IronOcr\runtimes\win-x64\native\IronOcrInterop.dll</Reference>
  <Reference>&lt;ProgramFilesX86&gt;\IronSoftware\IronOcr\runtimes\win-x64\native\IronPdfInterop.dll</Reference>
  <Reference>&lt;ProgramFilesX86&gt;\IronSoftware\IronOcr\lib\netstandard2.0\IronSoftware.Abstractions.dll</Reference>
  <Reference>&lt;ProgramFilesX86&gt;\IronSoftware\IronOcr\lib\netstandard2.0\IronSoftware.Drawing.Common.dll</Reference>
  <Reference>&lt;ProgramFilesX86&gt;\IronSoftware\IronOcr\lib\netstandard2.0\IronSoftware.Logger.dll</Reference>
  <Reference>&lt;ProgramFilesX86&gt;\IronSoftware\IronOcr\lib\netstandard2.0\IronSoftware.Shared.dll</Reference>
  <Reference>&lt;ProgramFilesX86&gt;\IronSoftware\IronOcr\runtimes\win-x64\native\liblept-5.dll</Reference>
  <Reference>&lt;ProgramFilesX86&gt;\IronSoftware\IronOcr\runtimes\win-x64\native\libtesseract-5.dll</Reference>
  <Reference>&lt;ProgramFilesX86&gt;\IronSoftware\IronOcr\runtimes\win-x64\native\LICENSE</Reference>
  <Reference>&lt;ProgramFilesX86&gt;\IronSoftware\IronOcr\lib\netstandard2.0\Microsoft.Extensions.Configuration.Abstractions.dll</Reference>
  <Reference>&lt;ProgramFilesX86&gt;\IronSoftware\IronOcr\lib\netstandard2.0\Microsoft.Extensions.Configuration.Binder.dll</Reference>
  <Reference>&lt;ProgramFilesX86&gt;\IronSoftware\IronOcr\lib\netstandard2.0\Microsoft.Extensions.Configuration.dll</Reference>
  <Reference>&lt;ProgramFilesX86&gt;\IronSoftware\IronOcr\lib\netstandard2.0\Microsoft.Extensions.Configuration.FileExtensions.dll</Reference>
  <Reference>&lt;ProgramFilesX86&gt;\IronSoftware\IronOcr\lib\netstandard2.0\Microsoft.Extensions.Configuration.Json.dll</Reference>
  <Reference>&lt;ProgramFilesX86&gt;\IronSoftware\IronOcr\lib\netstandard2.0\Microsoft.Extensions.FileProviders.Abstractions.dll</Reference>
  <Reference>&lt;ProgramFilesX86&gt;\IronSoftware\IronOcr\lib\netstandard2.0\Microsoft.Extensions.FileProviders.Physical.dll</Reference>
  <Reference>&lt;ProgramFilesX86&gt;\IronSoftware\IronOcr\lib\netstandard2.0\Microsoft.Extensions.FileSystemGlobbing.dll</Reference>
  <Reference>&lt;ProgramFilesX86&gt;\IronSoftware\IronOcr\lib\netstandard2.0\Microsoft.Extensions.Logging.Abstractions.dll</Reference>
  <Reference>&lt;ProgramFilesX86&gt;\IronSoftware\IronOcr\lib\netstandard2.0\Microsoft.Extensions.Primitives.dll</Reference>
  <Reference>&lt;ProgramFilesX86&gt;\IronSoftware\IronOcr\runtimes\win-x64\native\osd.traineddata</Reference>
  <Reference>&lt;ProgramFilesX86&gt;\IronSoftware\IronOcr\runtimes\win-x64\native\pdf.ttf</Reference>
  <Reference>&lt;ProgramFilesX86&gt;\IronSoftware\IronOcr\runtimes\win-x64\native\pdf.ttx</Reference>
  <Reference>&lt;ProgramFilesX86&gt;\IronSoftware\IronOcr\runtimes\win-x64\native\Pdfium.Native.deployment.json</Reference>
  <Reference>&lt;ProgramFilesX86&gt;\IronSoftware\IronOcr\lib\netstandard2.0\SixLabors.Fonts.dll</Reference>
  <Reference>&lt;ProgramFilesX86&gt;\IronSoftware\IronOcr\lib\netstandard2.0\SixLabors.ImageSharp.dll</Reference>
  <Reference>&lt;ProgramFilesX86&gt;\IronSoftware\IronOcr\lib\netstandard2.0\SixLabors.ImageSharp.Drawing.dll</Reference>
  <Reference>&lt;ProgramFilesX86&gt;\IronSoftware\IronOcr\lib\netstandard2.0\System.Configuration.ConfigurationManager.dll</Reference>
  <Reference>&lt;ProgramFilesX86&gt;\IronSoftware\IronOcr\lib\netstandard2.0\System.Numerics.Vectors.dll</Reference>
  <Reference>&lt;ProgramFilesX86&gt;\IronSoftware\IronOcr\lib\netstandard2.0\System.Runtime.CompilerServices.Unsafe.dll</Reference>
  <Reference>&lt;ProgramFilesX86&gt;\IronSoftware\IronOcr\runtimes\win-x64\native\Tesseract.Windows.deployment.json</Reference>
  <Namespace>IanAutomation</Namespace>
  <Namespace>IanAutomation.FileHelpers</Namespace>
  <Namespace>IanAutomation.ImageFiles</Namespace>
  <Namespace>System.Windows.Forms</Namespace>
</Query>

void Main()
{
	string ReceiptFolderPath = @"F:\projects_uipath\Robot6_OpticalCharacterRecognition\ScannedReceipts";
	RobogasReceipt receipt = null;
	
	//PdfHelper Pdf;
	
	try
	{
		// Check if the folder exists
        if (!Directory.Exists(ReceiptFolderPath))
        {
            Console.WriteLine("Folder does not exist.");
			return;
		}
		
		// Get the files and sort them by CreationDate
		List<FileInfo> files = Directory.GetFiles(ReceiptFolderPath).Select(path => new FileInfo(path)).ToList();
		files.Sort(CompareCreationTimes);
		
		//receipt = new RobogasReceipt(files[4].FullName);
		//receipt.SaveToBitmap();
		//return;
		
		//receipt = new RobogasReceipt(files[0].FullName);
		//var rec = receipt.Parse();
		//Console.WriteLine(rec.ID);
		//Console.WriteLine(rec.Date);
		//Console.WriteLine(rec.SaleAmount);
		//Console.WriteLine(rec.SumAmount);
		//Console.WriteLine(rec.Status);
		//return;
			
		// Iterate over each file in the folder
		var receipts = new List<RobogasReceipt.Receipt>();
		int num = 1;
        foreach (FileInfo file in files)
        {
			Console.WriteLine("Parsing {0} of {1}", num++, files.Count);
			receipt = new RobogasReceipt(file.FullName);
			var inv = receipt.Parse();
			
			receipts.Add(inv);	
        }
		
		var data = CSVHelper.ListToDataTable(receipts);
		CSVHelper.WriteCSV(data, @"C:/receipts.csv");
		Thread.Sleep(5000);
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
