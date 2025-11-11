using FATWA_DOMAIN.Enums.Common;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.Lookups;
using FATWA_DOMAIN.Models;
using FATWA_INFRASTRUCTURE.Database;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FATWA_INFRASTRUCTURE.Repository.PatternNumber
{
    public class LMSDeweyBookPatternNumberRepository
    {
        private readonly DatabaseContext _dbContext;

        public LMSDeweyBookPatternNumberRepository(DatabaseContext databaseContext)
        {
                _dbContext = databaseContext;
        }

        #region Generate Unique Dewey Book Number

        public async Task<string> GetHighestNumberByIndexNumber(string indexNumber)
        {
            string storedProc = $"exec pGetliteratureDeweyNumberPatternslist";
            var result = await _dbContext.LiteratureDeweyNumberPatternVMs.FromSqlRaw(storedProc).ToListAsync();
            var letestDeweyPatternNumberRecord = result.OrderByDescending(e => e.CreatedDate).FirstOrDefault();
            if (letestDeweyPatternNumberRecord != null)
            {
                var deweyBookNumbers = await _dbContext.LmsLiteratures
                                         .Where(e => EF.Functions.Like(e.DeweyBookNumber, $"{indexNumber}.%"))
                                         .Select(x => x.DeweyBookNumber).ToListAsync() ?? new List<string>();

                if (deweyBookNumbers != null && deweyBookNumbers.Count > 0)
                {
                    var filteredData = deweyBookNumbers.Where(item => {
                        string[] parts = item.Split('.'); return parts.Length > 1 && parts[0] == indexNumber;
                    }).ToList();

                    List<int> SequenceNumber = new();

                    foreach (var maxNumber in filteredData)
                    {
                        int dotIndex = maxNumber.IndexOf('.');
                        int dashIndex = maxNumber.IndexOf('-', dotIndex);
                        if (dotIndex >= 0 && dashIndex >= 0)
                        {
                            string trimmed = maxNumber.Substring(dotIndex + 1, dashIndex - dotIndex - 1);
                            SequenceNumber.Add(Convert.ToInt32(trimmed));
                        }
                    }

                    int maxValue = SequenceNumber.Max();
                    maxValue = maxValue + 1;

                    int DigitalSequenceNumber = int.Parse(letestDeweyPatternNumberRecord.DigitSequenceNumber);
                    DigitalSequenceNumber += maxValue;

                    string output = DigitalSequenceNumber.ToString($"D{letestDeweyPatternNumberRecord.DigitSequenceNumber.Length}");
                    return $"{indexNumber}.{output}.{letestDeweyPatternNumberRecord.SeriesNumber}";
                }
                else
                {
                    int maxValue = 0;
                    maxValue = maxValue + 1;

                    int DigitalSequenceNumber = int.Parse(letestDeweyPatternNumberRecord.DigitSequenceNumber);
                    DigitalSequenceNumber += maxValue;
                    string output = DigitalSequenceNumber.ToString($"D{letestDeweyPatternNumberRecord.DigitSequenceNumber.Length}");
                    return $"{indexNumber}.{output}.{letestDeweyPatternNumberRecord.SeriesNumber}";
                }
            }
            else
            {
                return "Please Add a Number Pattern Lookups First";
            }
        }

        #endregion

    }
}
