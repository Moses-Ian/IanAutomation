using IronOcr;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronSoftware.Drawing;

namespace IanAutomation.ImageFiles
{
    public class RobogasReceipt
    {
        public IronTesseract ocr;
        public OcrResult FullDocumentResult;
        public string Filepath;

        public RobogasReceipt(string Filepath)
        {
            this.Filepath = Filepath;

            // Another way to do it described at https://ironsoftware.com/csharp/ocr/how-to/license-keys/
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile(@"F:/projects_csharp/IanAutomation/appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            string secret = config["IronOCRLicense"];
            if (secret == null)
                throw new Exception("The OCR License is missing");
            License.LicenseKey = secret;
            if (!License.IsLicensed)
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
                FullDocumentResult.Pages[0].ToBitmap(input).SaveAs(@"C:/measure-me.bmp");
                Console.WriteLine(@"File saved to C:/measure-me.bmp");
            }
        }

        public string ID
        {
            get
            {
                using (var OcrInput = new OcrInput())
                {
                    Rectangle area = new Rectangle(341, 383, 144, 40);
                    OcrInput.LoadPdf(Filepath, ContentArea: area);
                    var ocrResult = ocr.Read(OcrInput);
                    return ocrResult.Text;
                }
            }
        }

        public DateTime Date
        {
            get
            {
                using (var OcrInput = new OcrInput())
                {
                    Rectangle area = new Rectangle(341, 423, 177, 39);
                    OcrInput.LoadPdf(Filepath, ContentArea: area);
                    var ocrResult = ocr.Read(OcrInput);
                    return DateTime.Parse(ocrResult.Text);
                }
            }
        }

        public decimal SaleAmount
        {
            get
            {
                using (var OcrInput = new OcrInput())
                {
                    Rectangle area = new Rectangle(371, 849, 115, 42);
                    OcrInput.LoadPdf(Filepath, ContentArea: area);
                    var ocrResult = ocr.Read(OcrInput);
                    return Decimal.Parse(ocrResult.Text);
                }
            }
        }

        public Receipt Parse()
        {
            var receipt = new Receipt();
            receipt.Status = "Success";
            try { receipt.ID = ID; }                 catch { receipt.Status = "Fail"; }
            try { receipt.Date = Date; }             catch { receipt.Status = "Fail"; }
            try { receipt.SaleAmount = SaleAmount; } catch { receipt.Status = "Fail"; }
            return receipt;
        }

        public struct Receipt
        {
            public string ID;
            public DateTime Date;
            public decimal SaleAmount;
            public string Status;
        }
    }
}
