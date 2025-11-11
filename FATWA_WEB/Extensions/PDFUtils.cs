using FATWA_DOMAIN.Models.CaseManagment;
using SelectPdf;

namespace FATWA_WEB.Extensions
{
    public class PDFUtils
    {
        public static List<PdfHtmlSection> ConvertHtmlToPdfTemplate(List<CaseTemplate> templates)
        {
            List<PdfHtmlSection> sectionList= new List<PdfHtmlSection>();
            // header
            var htmlString = templates.Where(x => x.NameEn ==  "HeaderEn" || x.NameEn == "HeaderAr").Select(x => x.Content).FirstOrDefault();
            PdfHtmlSection pdfHtmlSec = new PdfHtmlSection(htmlString);
            sectionList.Add(pdfHtmlSec);
            // footer
            htmlString = templates.Where(x => x.NameEn == "Footer").Select(x => x.Content).FirstOrDefault();
            pdfHtmlSec = new PdfHtmlSection(htmlString);
            sectionList.Add(pdfHtmlSec);
            return sectionList;
        }
    }
}
