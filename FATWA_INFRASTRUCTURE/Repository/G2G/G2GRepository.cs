using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_INFRASTRUCTURE.Database;
using FATWA_INFRASTRUCTURE.Repository.CaseManagement;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_INFRASTRUCTURE.Repository.G2G
{
    //<!-- <History Author = 'Hassan Abbas' Date='2024-12-01' Version="1.0" Branch="master">Shared Repo for Interacting with G2G Database</History> -->
    public class G2GRepository
    {
        private readonly G2GDbContext _g2gDbContext;
        private readonly IServiceScopeFactory _serviceScopeFactory;


        #region Constructor
        public G2GRepository(G2GDbContext g2gDbContext, IServiceScopeFactory serviceScopeFactory)
        {
            _g2gDbContext = g2gDbContext;
            _serviceScopeFactory = serviceScopeFactory;
        }
        #endregion

        #region
        public async Task<Tuple<string, int>> GetNextGEUserForRequestAssignment(int govtEntityId, bool isCase)
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var _g2gDbContext = scope.ServiceProvider.GetRequiredService<G2GDbContext>();
                var nextUserIdParam = new SqlParameter
                {
                    ParameterName = "@NextUserId",
                    SqlDbType = SqlDbType.NVarChar,
                    Size = 450,
                    Direction = ParameterDirection.Output
                };
                var nextUserDepartIdParam = new SqlParameter
                {
                    ParameterName = "@DepartmentId",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Output
                };

                await _g2gDbContext.Database.ExecuteSqlRawAsync($"EXEC pGetNextGEUserForRequestAssignment @GovtEntityId, @IsCase, @NextUserId OUTPUT, @DepartmentId OUTPUT", new SqlParameter("@GovtEntityId", govtEntityId), new SqlParameter("@IsCase", isCase), nextUserIdParam, nextUserDepartIdParam);
                return Tuple.Create(nextUserIdParam.Value != DBNull.Value ? (string)nextUserIdParam.Value : string.Empty, nextUserDepartIdParam.Value != DBNull.Value ? (int)nextUserDepartIdParam.Value : 0);
            }
            catch(Exception ex)
            {
                throw;
            }
        }
        #endregion
    }
}
