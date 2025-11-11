using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_INFRASTRUCTURE.Database;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace FATWA_INFRASTRUCTURE.Repository
{
    public class LegalLibraryRepository : ILegalLibrary
    {
        private readonly DatabaseContext _dbContext;
        private LegalLibraryVM _LegalLibraryVMVM = new LegalLibraryVM()
        {
            Books = new List<LmsLiterature>()
        };

        private List<LmsLiterature>? literatures = new List<LmsLiterature>();

        public LegalLibraryRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<LegalLibraryVM> SearchLegalLibrary()
        {
            try
            {
                using (var cnn = _dbContext.Database.GetDbConnection())
                {
                    var cmm = cnn.CreateCommand();
                    cmm.CommandType = System.Data.CommandType.StoredProcedure;
                    cmm.CommandText = "[dbo].[pLegalLibrarySelAll]";
                    cmm.Connection = cnn;
                    cnn.Open();
                    var reader = cmm.ExecuteReader();
                    while (reader.Read())
                    {
                        AddElementsToList("literature", reader);
                    }
                    reader.NextResult(); //move the next record set
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            if (literatures.Any())
                _LegalLibraryVMVM.Books.AddRange(literatures);

            return _LegalLibraryVMVM;
        }

        private void AddElementsToList(string itemType, DbDataReader reader)
        {
            if (itemType == "literature")
            {
                literatures.Add(
                    new LmsLiterature
                    {
                        LiteratureId = (int)reader["LiteratureId"],
                        Name = (string)reader["Name"],
                        Subject_En = (string)reader["Subject_En"],
                        Subject_Ar = (string)reader["Subject_Ar"],
                        ISBN = (string)reader["ISBN"],
                        Characters = (string)reader["Characters"],
                        CopyCount = (int)reader["CopyCount"],
                        NumberOfPages = (int)reader["NumberOfPages"],
                        IsSeries = (bool)reader["IsSeries"],
                        SeriesNumber = (int)reader["SeriesNumber"],
                        IsViewable = (bool)reader["IsViewable"],
                        IsBorrowable = (bool)reader["IsBorrowable"],
                        AllowtoPublish = (bool)reader["AllowToPublish"],
                        NumberOfBorrowedBooks = (int)reader["NumberOfBorrowedBooks"],
                        CreatedBy = (string)reader["CreatedBy"],
                        CreatedDate = (DateTime)reader["CreatedDate"],
                        //ModifiedBy = (string)reader["ModifiedBy"],
                        ModifiedBy = (reader["ModifiedBy"] == DBNull.Value) ? string.Empty : (string)reader["ModifiedBy"],
                        //ModifiedDate = (DateTime)reader["ModifiedDate"],
                        ModifiedDate = (reader["ModifiedDate"] == DBNull.Value) ? null : (DateTime)reader["ModifiedDate"],
                        //DeletedBy = (string)reader["DeletedBy"],
                        DeletedBy = (reader["DeletedBy"] == DBNull.Value) ? string.Empty : (string)reader["DeletedBy"],
                        //DeletedDate = (DateTime)reader["DeletedDate"],
                        DeletedDate = (reader["DeletedDate"] == DBNull.Value) ? null : (DateTime)reader["DeletedDate"],
                        IsDeleted = (bool)reader["IsDeleted"],
                        TypeId = (int)reader["TypeId"],
                        ClassificationId = (int)reader["ClassificationId"],
                        IndexId = (int)reader["IndexId"]
                    }
                );
            }
        }
    }
}
