// So far, you really don't need to use this. All of this is easy to add to your script

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace IanAutomation.FileHelpers
{
    public class PdfHelper : IDisposable
    {
        public PdfReader PDF;
        public PdfDocument Document;

        public PdfHelper(string FilePath)
        {
            PDF = new PdfReader(FilePath);
            Document = new PdfDocument(PDF);
        }

        public void Shutdown()
        {
            if (PDF != null)
            {
                PDF.Close();
            }
            if (Document != null)
            {
                Document.Close();
            }
        }

        public void Dispose()
        {
            Shutdown();
        }
    }
}
