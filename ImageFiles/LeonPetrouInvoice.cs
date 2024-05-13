using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronOcr;
using Microsoft.Extensions.Configuration;

namespace IanAutomation.ImageFiles
{
    public class LeonPetrouInvoice
    {
        public IronTesseract ocr;
        public OcrResult OcrResult;
        public int XSpacing = 10;
        public int YSpacing = 10;

        public LeonPetrouInvoice(string Filepath)
        {
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

            using (var ocrInput = new OcrInput())
            {
                ocrInput.LoadPdf(Filepath);
                OcrResult = ocr.Read(ocrInput);
            }
        }

        public string InvoiceNumber
        {
            get
            {
                return OcrResult.Blocks.Where(block =>
                    block.Location.X > 1235 - XSpacing &&
                    block.Location.X < 1235 + XSpacing &&
                    block.Location.Y > 459 - XSpacing &&
                    block.Location.Y < 459 + YSpacing
                ).First().Text;
            }
        }
    }
}
