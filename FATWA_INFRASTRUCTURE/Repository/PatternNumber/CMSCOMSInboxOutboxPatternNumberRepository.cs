using FATWA_DOMAIN.Interfaces.PatternNumber;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.Lookups;
using FATWA_INFRASTRUCTURE.Database;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using FATWA_DOMAIN.Enums.Common;
using Microsoft.Extensions.DependencyInjection;
using FATWA_DOMAIN.Models.ViewModel.CMSCOMSRequestNumberVMs;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using MailKit;
using System.Collections;

namespace FATWA_INFRASTRUCTURE.Repository.PatternNumber
{
    public class CMSCOMSInboxOutboxPatternNumberRepository : ICMSCOMSInboxOutboxRequestPatternNumber
    {
        #region Variables
        private readonly DatabaseContext _dbContext;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        #endregion

        #region Constructor
        public CMSCOMSInboxOutboxPatternNumberRepository(DatabaseContext dbContext, IServiceScopeFactory serviceScopeFactory)
        {
            _dbContext = dbContext;
            _serviceScopeFactory = serviceScopeFactory;
        }
        #endregion

        #region Get New Pattern Number
        //< History Author = 'Attique ur Rehman' Date = '10/Oct/2024' Version = "2.0" Branch = "master"> Functionality of Generating Pattern Number </History>
        // Generates a pattern number for CMS/COMS Requests,CMS/COMS Files, Inbox/Outbox.
        // Usage: Call with GovtEntityId (only for CMS/COMS requests) and NumberTypeId(CmsComsNumPatternTypeEnum)
        public async Task<NumberPatternResult?> GenerateNumberPattern(int govtEntityId, int NumberTypeId, dynamic dbContext = null)
        {
            try
            {

                using var scope = _serviceScopeFactory.CreateScope();
                var _dbContext = dbContext != null ? (DatabaseContext)dbContext : scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                NumberPatternResult sequenceDetails;
                var PatternId = await _dbContext.CmsGovtEntityNumPattern.Where(x => x.GovtEntityId == govtEntityId).Select(x => x.CMSRequestPatternId).FirstOrDefaultAsync();
                var Pattern = await _dbContext.CmsComsNumPatterns
                    .Where(x => (PatternId == Guid.Empty && x.IsDefault == true && x.PatternTypId == NumberTypeId) || (PatternId != Guid.Empty && x.Id == PatternId && x.IsDeleted == false))
                    .FirstOrDefaultAsync();

                string SequenceFormat = string.Empty;
                int CompareDate = 0;

                switch (NumberTypeId)
                {
                    case (int)CmsComsNumPatternTypeEnum.CaseRequestNumber:
                        var getLatestCMSRecord = await _dbContext.CaseRequests
                            .Where(y => y.GovtEntityId == govtEntityId && y.PatternSequenceResult == Pattern.SequanceResult && y.RequestNumber != null).OrderByDescending(x => x.CreatedDate)
                            .FirstOrDefaultAsync();
                        SequenceFormat = getLatestCMSRecord?.RequestNumberFormat ?? Pattern.SequanceFormatResult;
                        CompareDate = getLatestCMSRecord?.CreatedDate.Year ?? 0;
                        break;

                    case (int)CmsComsNumPatternTypeEnum.ConsultationRequestNumber:
                        var getLatestCOMSRecord = await _dbContext.ConsultationRequests
                            .Where(y => y.GovtEntityId == govtEntityId && y.PatternSequenceResult == Pattern.SequanceResult && y.RequestNumber != null).OrderByDescending(x => x.CreatedDate)
                            .FirstOrDefaultAsync();
                        SequenceFormat = getLatestCOMSRecord?.RequestNumberFormat ?? Pattern.SequanceFormatResult;
                        CompareDate = getLatestCOMSRecord?.CreatedDate.Year ?? 0;
                        break;

                    case (int)CmsComsNumPatternTypeEnum.CaseFileNumber:
                        var getLatestCaseFileRecord = await _dbContext.CaseFiles
                            .Where(y => y.PatternSequenceResult == Pattern.SequanceResult && y.FileNumber != null).OrderByDescending(x => x.CreatedDate).FirstOrDefaultAsync();
                        SequenceFormat = getLatestCaseFileRecord?.CaseFileNumberFormat ?? Pattern.SequanceFormatResult;
                        CompareDate = getLatestCaseFileRecord?.CreatedDate.Year ?? 0;
                        break;

                    case (int)CmsComsNumPatternTypeEnum.ConsultationFileNumber:
                        var getLatestConsultationFileRecord = await _dbContext.ConsultationFiles
                            .Where(y => y.PatternSequenceResult == Pattern.SequanceResult && y.FileNumber != null).OrderByDescending(x => x.CreatedDate).FirstOrDefaultAsync();
                        SequenceFormat = getLatestConsultationFileRecord?.ComsFileNumberFormat ?? Pattern.SequanceFormatResult;
                        CompareDate = getLatestConsultationFileRecord?.CreatedDate.Year ?? 0;
                        break;

                    case (int)CmsComsNumPatternTypeEnum.InboxNumber:
                        var getLatestInboxRecord = await _dbContext.Communications
                            .Where(y => y.PatternSequenceResult == Pattern.SequanceResult && y.InboxNumber != null).OrderByDescending(x => x.CreatedDate).FirstOrDefaultAsync();
                        SequenceFormat = getLatestInboxRecord?.InboxNumberFormat ?? Pattern.SequanceFormatResult;
                        CompareDate = getLatestInboxRecord?.CreatedDate.Year ?? 0;
                        break;

                    case (int)CmsComsNumPatternTypeEnum.OutboxNumber:
                        var getLatestOutboxRecord = await _dbContext.Communications
                            .Where(y => y.PatternSequenceResult == Pattern.SequanceResult && y.OutboxNumber != null).OrderByDescending(x => x.CreatedDate).FirstOrDefaultAsync();
                        SequenceFormat = getLatestOutboxRecord?.OutBoxNumberFormat ?? Pattern.SequanceFormatResult;
                        CompareDate = getLatestOutboxRecord?.CreatedDate.Year ?? 0;
                        break;
                }
                sequenceDetails = GenerateCMSCOMSRequestNumberPattern(SequenceFormat, Pattern, CompareDate);
                // Check for Existence of any Pattern
                sequenceDetails = await CheckAndGeneratePatternUntilUnique(NumberTypeId, sequenceDetails, Pattern, govtEntityId, _dbContext);
                return sequenceDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }
        //< History Author = 'Attique ur Rehman' Date = '20/Oct/2024' Version = "1.0" Branch = "master"> Functionality of Re-Generating Pattern Number Incase of Exitance </History>
        private async Task<NumberPatternResult> CheckAndGeneratePatternUntilUnique(int numberTypeId, NumberPatternResult sequenceDetails, CmsComsNumPattern pattern, int govtEntityId, DatabaseContext _dbContext)
        {
            try
            {
                bool isUnique = false; // Flag for unique Number
                while (!isUnique)
                {
                    string format = null; // Format for Generating the Pattern Against
                    int createdYear = 0; // For Comparing the Created year of Entity incase of Reset the Yearly pattern Functionality
                    switch (numberTypeId)
                    {
                        case (int)CmsComsNumPatternTypeEnum.CaseRequestNumber:
                            {
                                var latestRequest = await _dbContext.CaseRequests.Where(x => x.RequestNumber == sequenceDetails.GenerateRequestNumber && x.GovtEntityId == govtEntityId)
                                    .OrderByDescending(x => x.CreatedDate).FirstOrDefaultAsync();
                                if (latestRequest != null)
                                {
                                    format = latestRequest.RequestNumberFormat;
                                    createdYear = latestRequest.CreatedDate.Year;
                                }
                                break;
                            }

                        case (int)CmsComsNumPatternTypeEnum.ConsultationRequestNumber:
                            {
                                var latestRequest = await _dbContext.ConsultationRequests
                                    .Where(x => x.RequestNumber == sequenceDetails.GenerateRequestNumber && x.GovtEntityId == govtEntityId)
                                    .OrderByDescending(x => x.CreatedDate)
                                    .FirstOrDefaultAsync();

                                if (latestRequest != null)
                                {
                                    format = latestRequest.RequestNumberFormat;
                                    createdYear = latestRequest.CreatedDate.Year;
                                }
                                break;
                            }

                        case (int)CmsComsNumPatternTypeEnum.CaseFileNumber:
                            {
                                var latestRequest = await _dbContext.CaseFiles
                                    .Where(x => x.FileNumber == sequenceDetails.GenerateRequestNumber)
                                    .OrderByDescending(x => x.CreatedDate)
                                    .FirstOrDefaultAsync();

                                if (latestRequest != null)
                                {
                                    format = latestRequest.CaseFileNumberFormat;
                                    createdYear = latestRequest.CreatedDate.Year;
                                }
                                break;
                            }

                        case (int)CmsComsNumPatternTypeEnum.ConsultationFileNumber:
                            {
                                var latestRequest = await _dbContext.ConsultationFiles
                                    .Where(x => x.FileNumber == sequenceDetails.GenerateRequestNumber)
                                    .OrderByDescending(x => x.CreatedDate)
                                    .FirstOrDefaultAsync();

                                if (latestRequest != null)
                                {
                                    format = latestRequest.ComsFileNumberFormat;
                                    createdYear = latestRequest.CreatedDate.Year;
                                }
                                break;
                            }

                        case (int)CmsComsNumPatternTypeEnum.InboxNumber:
                            {
                                var latestRequest = await _dbContext.Communications
                                    .Where(x => x.InboxNumber == sequenceDetails.GenerateRequestNumber)
                                    .OrderByDescending(x => x.CreatedDate)
                                    .FirstOrDefaultAsync();

                                if (latestRequest != null)
                                {
                                    format = latestRequest.InboxNumberFormat;
                                    createdYear = latestRequest.CreatedDate.Year;
                                }
                                break;
                            }

                        case (int)CmsComsNumPatternTypeEnum.OutboxNumber:
                            {
                                var latestRequest = await _dbContext.Communications
                                    .Where(x => x.OutboxNumber == sequenceDetails.GenerateRequestNumber)
                                    .OrderByDescending(x => x.CreatedDate)
                                    .FirstOrDefaultAsync();

                                if (latestRequest != null)
                                {
                                    format = latestRequest.OutBoxNumberFormat;
                                    createdYear = latestRequest.CreatedDate.Year;
                                }
                                break;
                            }
                    }
                    if (!string.IsNullOrEmpty(format))
                    {
                        sequenceDetails = GenerateCMSCOMSRequestNumberPattern(format, pattern, createdYear);
                    }
                    isUnique = !await IsNumberExists(numberTypeId, sequenceDetails.GenerateRequestNumber, govtEntityId, _dbContext);
                }
                return sequenceDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<bool> IsNumberExists(int NumberTypeId, string newlyGeneratedPatternNumber, int GovtEntityId, DatabaseContext _dbContext)
        {
            try
            {
                switch (NumberTypeId)
                {
                    case (int)CmsComsNumPatternTypeEnum.CaseRequestNumber:
                        return await _dbContext.CaseRequests.AnyAsync(x => x.RequestNumber == newlyGeneratedPatternNumber && x.GovtEntityId == GovtEntityId);

                    case (int)CmsComsNumPatternTypeEnum.ConsultationRequestNumber:
                        return await _dbContext.ConsultationRequests.AnyAsync(x => x.RequestNumber == newlyGeneratedPatternNumber && x.GovtEntityId == GovtEntityId);

                    case (int)CmsComsNumPatternTypeEnum.CaseFileNumber:
                        return await _dbContext.CaseFiles.AnyAsync(x => x.FileNumber == newlyGeneratedPatternNumber);

                    case (int)CmsComsNumPatternTypeEnum.ConsultationFileNumber:
                        return await _dbContext.ConsultationFiles.AnyAsync(x => x.FileNumber == newlyGeneratedPatternNumber);

                    case (int)CmsComsNumPatternTypeEnum.InboxNumber:
                        return await _dbContext.Communications.AnyAsync(x => x.InboxNumber == newlyGeneratedPatternNumber);

                    case (int)CmsComsNumPatternTypeEnum.OutboxNumber:
                        return await _dbContext.Communications.AnyAsync(x => x.OutboxNumber == newlyGeneratedPatternNumber);

                    default:
                        return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //< History Author = 'Attique ur Rehman' Date = '09/Oct/2024' Version = "2.0" Branch = "master"> Functionality of Generating Pattern Number </History>
        public NumberPatternResult GenerateCMSCOMSRequestNumberPattern(string sequenceFormatResult, CmsComsNumPattern Pattern, int CompareDateYear)
        {
            try
            {
                string FormatRequestNumber = string.Empty;
                string GenerateRequestNumber = string.Empty;
                int yearPartIndexlatest = -1;
                string yearValue = string.Empty;

                string numberPattern = @"\d+";
                string textPattern = @"[A-Za-z]+";
                Dictionary<string, string> patternToDateTimeFormat = new Dictionary<string, string> { { "YY", "yy" }, { "YYYY", "yyyy" }, { "MM", "MM" }, { "MMM", "MMM" }, { "MMMM", "MMMM" } };
                // Split the input string into an array of parts
                string[] SequenceParts = Regex.Split(sequenceFormatResult, $"({numberPattern})|({textPattern})");
                // Filter out empty or null values in the array
                SequenceParts = Array.FindAll(SequenceParts, s => !string.IsNullOrEmpty(s));

                // Join the parts with "/-/" as the delimiter
                string result = string.Join("/-/", SequenceParts);

                for (int i = 0; i < SequenceParts.Length; i++)
                {
                    if (SequenceParts[i].Length == Pattern.Year.Length)
                    {
                        yearPartIndexlatest = i;
                        if (yearPartIndexlatest != -1)
                        {
                            yearValue = SequenceParts[yearPartIndexlatest];
                        }
                    }
                }
                string[] parts = sequenceFormatResult.Split(new string[] { "/-/" }, StringSplitOptions.None);

                int numericPartIndex = -1;
                int characterPartIndex = -1;
                int yearPartIndex = -1;
                int monthPartIndex = -1;
                int dayPartIndex = -1;

                foreach (var (part, index) in parts.Select((part, index) => (part, index)))
                {
                    if (Regex.IsMatch(part, @"^\d+$") && part.Length >= Pattern.SequanceNumber.Length)
                    {
                        numericPartIndex = index;
                    }
                    else if (part.Equals(Pattern.StaticTextPattern, StringComparison.OrdinalIgnoreCase) )
                    {
                        characterPartIndex = index;
                    }
                    else if (part == Pattern.Year)
                    {
                        yearPartIndex = index;
                    }
                    else if (part == Pattern.Month)
                    {
                        monthPartIndex = index;
                    }
                    else if (part == Pattern.Day)
                    {
                        dayPartIndex = index;
                    }
                }

                foreach (var (part, index) in parts.Select((part, index) => (part, index)))
                {
                    if (index == numericPartIndex && numericPartIndex != -1)
                    {
                        if (int.TryParse(part, out int numericPart))
                        {
                            if (Pattern.ResetYearly)
                            {
                                if (DateTime.Now.Year > CompareDateYear)
                                {
                                    numericPart = 0;
                                }
                            }
                            numericPart++; // Increment the numeric part
                            string incrementedNumericPart = numericPart.ToString("D" + parts[numericPartIndex].Length);
                            FormatRequestNumber += incrementedNumericPart + "/-/";
                            parts[numericPartIndex] = incrementedNumericPart;
                        }
                    }
                    else if (index == characterPartIndex && characterPartIndex != -1)
                    {
                        parts[characterPartIndex] = Pattern.StaticTextPattern.ToUpper();
                        FormatRequestNumber += parts[characterPartIndex] + "/-/";
                    }
                    else if (index == yearPartIndex && yearPartIndex != -1)
                    {
                        if (Pattern.Year.Length == 2)
                        {
                            parts[yearPartIndex] = DateTime.Now.ToString("yy");
                            FormatRequestNumber += "YY" + "/-/";
                        }
                        else if (Pattern.Year.Length == 4)
                        {
                            parts[yearPartIndex] = DateTime.Now.ToString("yyyy");
                            FormatRequestNumber += "YYYY" + "/-/";
                        }
                    }
                    else if (index == monthPartIndex && monthPartIndex != -1)
                    {
                        string dateTimeFormat;
                        if (patternToDateTimeFormat.TryGetValue(Pattern.Month, out dateTimeFormat))
                        {
                            parts[monthPartIndex] = DateTime.Now.ToString(dateTimeFormat).ToUpper();
                            FormatRequestNumber += Pattern.Month + "/-/";
                        }
                    }
                    else if (index == dayPartIndex && dayPartIndex != -1)
                    {
                        parts[dayPartIndex] = DateTime.Now.ToString("dd");
                        FormatRequestNumber += "DD" + "/-/";
                    }
                }

                // Trim last "/-/" and concatenate parts to get the final pattern number
                FormatRequestNumber = FormatRequestNumber.TrimEnd(new char[] { '/', '-' });
                GenerateRequestNumber = string.Join("", parts);

                return new NumberPatternResult
                {
                    GenerateRequestNumber = GenerateRequestNumber,
                    FormatRequestNumber = FormatRequestNumber,
                    RequestPatternId = Pattern.Id,
                    PatternSequenceResult = Pattern.SequanceResult,
                };
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion
    }
}
