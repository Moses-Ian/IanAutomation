<Query Kind="Program">
  <Reference>F:\projects_csharp\IanAutomation\bin\Debug\net7.0\IanAutomation.dll</Reference>
  <Namespace>IanAutomation</Namespace>
  <Namespace>IanAutomation.Redfin</Namespace>
  <Namespace>System.Windows.Forms</Namespace>
</Query>

void Main()
{
	string Location = "new york";

	Redfin Page = null;
	
	try
	{
		
		Page = new Redfin();
		
		var ResultsPage = Page.Search(Location);
		Thread.Sleep(1000);
		ResultsPage.SetMode(new SearchMode()
		{
			SaleType = SaleType.ForSale,
			Active = true
		});
		ResultsPage.SetHomeType(new HomeType() { House = true });
		Thread.Sleep(4000);
		
		var listings = ResultsPage.GetListings(MaxPages: 1);
		
		// before we write the listings to a datatable, let's remove the commas from the addresses
		CleanListings(listings);
		
		var data = CSVHelper.ListToDataTable(listings);
		CSVHelper.WriteCSV(data, @"C:/listings.csv");
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
		if (Page != null)
			Page.Shutdown();
	}
}

// You can define other methods, fields, classes and namespaces here
public void CleanListings(List<Listing> listings)
{
	listings.ForEach(listing => listing.Address = listing.Address.Replace(",", ""));
}
