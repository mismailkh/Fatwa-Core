using FATWA_DOMAIN.Models;
using System.Text.RegularExpressions;

namespace FATWA_DOMAIN.Common.Service
{
    public class LmsLiteratureService
    {
        #region Barcode
        public string GenerateRandomString12DigitsLiteratureBarcocode(int size)
        {
            String BarcodeNumber = "";
            var digitsnumber = Guid.NewGuid().ToString().Replace("-", string.Empty);
            BarcodeNumber = Regex.Replace(digitsnumber, "[a-zA-Z]", string.Empty).Substring(0, size);
            return BarcodeNumber;
        }
        public async Task<List<LmsLiteratureBarcode>> GenerateListof12DigitsLiteratureBarcocode(int size, int copyCount)
        {
            try
            {
                List<LmsLiteratureBarcode> lmsLiteratureBarcodes = new List<LmsLiteratureBarcode>();
                for (int i = 0; i < copyCount; i++)
                {
                    string uniqueNumber = new string(Guid.NewGuid().ToString("N").Where(char.IsDigit).Take(12).ToArray());
                    while (uniqueNumber.Length < 12)
                    {
                        uniqueNumber += new string(Guid.NewGuid().ToString("N").Where(char.IsDigit).Take(12 - uniqueNumber.Length).ToArray());
                    }
                    lmsLiteratureBarcodes.Add(new LmsLiteratureBarcode { BarCodeNumber = uniqueNumber, Active = true });
                }
                return lmsLiteratureBarcodes;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


    }
}
