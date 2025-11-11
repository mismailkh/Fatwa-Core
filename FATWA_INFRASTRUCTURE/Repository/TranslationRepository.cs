using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.Lookups;
using FATWA_DOMAIN.Models.ViewModel.TranslationMobileAppVMs;
using FATWA_INFRASTRUCTURE.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections;
using System.Collections.Generic;

namespace FATWA_INFRASTRUCTURE.Repository
{
    public class TranslationRepository : ITranslation
    {
        private readonly DatabaseContext _dbContext;

        private List<FATWA_DOMAIN.Models.Translation> _Translation;
        private List<Translation> _TranslationVM;
        private List<TranslationMobileAppVM> translationMobileAppVMs;
        public TranslationRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Translation>> GetTranslations()
        {
            try
            {
                if (_TranslationVM == null)
                {

                    string StoredProc = "exec pTranslationGetBulkDataAll ";
                    _TranslationVM = await _dbContext.Translations.FromSqlRaw(StoredProc).ToListAsync();
                }

            }
            catch (Exception)
            {

                throw;
            }
            return _TranslationVM;

        }
        public async Task<TranslationLabelsVM> GetTranslationsByCultureValue(string cultureValue, int channelId, string versionCode)
        {
            TranslationLabelsVM translationLabelsVM = new TranslationLabelsVM();
            try
            {
                if (!string.IsNullOrEmpty(cultureValue) && !string.IsNullOrEmpty(versionCode) && channelId > 0)
                {
                    var appVersion = _dbContext.MobileAppVersions.Where(x => x.ChannelId == channelId).FirstOrDefault();
                    if (appVersion != null)
                    {
                        if (Convert.ToDecimal(appVersion.VersionCode) < Convert.ToDecimal(versionCode))
                        {
                            translationLabelsVM.TranslationLabels = new Dictionary<string, string>();
                            Translation? task = await _dbContext.Translations.Where(x => x.TranslationKey == "Force_App_Update").FirstOrDefaultAsync();
                            translationLabelsVM.TranslationLabels.Add(task?.TranslationKey, cultureValue.ToUpper().Equals("EN") ? task.Value_En : task.Value_Ar);
                            translationLabelsVM.ForceAppUpdate = true;
                            return translationLabelsVM;
                        }
                    }
                    string StoredProc = $"exec pTranslationGetDataByCultureValue @CultureValue = '{cultureValue}'";
                    translationMobileAppVMs = await _dbContext.TranslationMobileAppVMs.FromSqlRaw(StoredProc).ToListAsync();
                    translationLabelsVM.TranslationLabels = translationMobileAppVMs.ToDictionary(obj => obj.TranslationKey, obj => obj.TranslationValue);
                    translationLabelsVM.ForceAppUpdate = false;
                    return translationLabelsVM;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return translationLabelsVM;
        }
        public List<Translation> GetTranslationsSync()
        {
            if (_Translation == null)
            {
                _Translation = _dbContext.Translations.ToList();
            }

            return _Translation;
        }

        public async Task UpdateTranslation(Translation translation)
        {
            try
            {
                _dbContext.Translations.Update(translation);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }

        }
        public async Task DeleteTranslation(int Id)
        {
            try
            {
                Translation? translation = await _dbContext.Translations.FindAsync(Id);
                if (translation != null)
                { 
                    _dbContext.Translations.Remove(translation);
                    var success = await _dbContext.SaveChangesAsync();
                }
                else
                {
                    throw new ArgumentNullException();
                }
            }
            catch
            {
                _dbContext.Entry(_Translation).State = EntityState.Unchanged;
                throw;
            } 
        }

    }
}
