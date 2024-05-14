using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronOcr;
using Microsoft.Extensions.Configuration;
using IronSoftware.Drawing;

namespace IanAutomation.ImageFiles
{
    public class LeonPetrouInvoice
    {
        public IronTesseract ocr;
        public OcrResult FullDocumentResult;
        public string Filepath;
        
        public LeonPetrouInvoice(string Filepath)
        {
            this.Filepath = Filepath;

            // Another way to do it described at https://ironsoftware.com/csharp/ocr/how-to/license-keys/
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile(@"F:/projects_csharp/IanAutomation/appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            string secret = config["IronOCRLicense"];
            if (secret == null)
                throw new Exception("The OCR License is missing");
            IronOcr.License.LicenseKey = secret;
            if (!IronOcr.License.IsLicensed)
                throw new Exception("The OCR License is invalid");
            
            ocr = new IronTesseract();
            
            using (var fullOcrInput = new OcrInput())
            {
                fullOcrInput.LoadPdf(Filepath);
                FullDocumentResult = ocr.Read(fullOcrInput);
            }
        }

        public void SaveToBitmap()
        {
            using (var input = new OcrInput())
            {
                input.LoadPdf(Filepath);
                FullDocumentResult.Pages[0].ToBitmap(input).SaveAs(@"C:/Users/IMoses/Documents/measure-me.bmp");
                Console.WriteLine(@"File saved to C:/Users/IMoses/Documents/measure-me.bmp");
            }
        }

        public string InvoiceNumber
        {
            get
            {
                using (var OcrInput = new OcrInput())
                {
                    Rectangle area = new Rectangle(1225, 429, 100, 50);
                    OcrInput.LoadPdf(Filepath, ContentArea: area);
                    var ocrResult = ocr.Read(OcrInput);
                    return ocrResult.Text.Replace(".", "");
                }
            }
        }

        public DateTime Date
        {
            get
            {
                using (var OcrInput = new OcrInput())
                {
                    Rectangle area = new Rectangle(1229, 353, 152, 55);
                    OcrInput.LoadPdf(Filepath, ContentArea: area);
                    var ocrResult = ocr.Read(OcrInput);
                    return DateTime.Parse(ocrResult.Text);
                }
            }
        }

        public string CompanyName
        {
            get
            {
                using (var OcrInput = new OcrInput())
                {
                    Rectangle area = new Rectangle(147, 639, 192, 57);
                    OcrInput.LoadPdf(Filepath, ContentArea: area);
                    var ocrResult = ocr.Read(OcrInput);
                    return ocrResult.Text;
                }
            }
        }

        public decimal Total
        {
            get
            {
                using (var OcrInput = new OcrInput())
                {
                    Rectangle area = new Rectangle(1231, 1231, 148, 78);
                    OcrInput.LoadPdf(Filepath, ContentArea: area);
                    var ocrResult = ocr.Read(OcrInput);
                    return Decimal.Parse(ocrResult.Text);
                }
            }
        }

        public Invoice Parse()
        {
            var invoice = new Invoice();
            invoice.Status = "Success";
            try { invoice.InvoiceNumber = InvoiceNumber; } catch { invoice.Status = "Fail"; }
            try { invoice.Date = Date; }                   catch { invoice.Status = "Fail"; }
            try { invoice.CompanyName = CompanyName; }     catch { invoice.Status = "Fail"; }
            try { invoice.Total = Total; }                 catch { invoice.Status = "Fail"; }
            return invoice;
        }

        public struct Invoice
        {
            public string InvoiceNumber;
            public DateTime Date;
            public string CompanyName;
            public decimal Total;
            public string Status;
        }
    }
}
