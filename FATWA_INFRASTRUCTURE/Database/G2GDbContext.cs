using FATWA_DOMAIN.Models.DigitalSignature;
using FATWA_DOMAIN.Models.Dms;
using FATWA_DOMAIN.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;

namespace FATWA_INFRASTRUCTURE.Database
{
    //< History Author = 'Hassan Abbas' Date = '2024-01-12' Version = "1.0" Branch = "master">Created DbContext for G2G</History>
    public partial class G2GDbContext : IdentityDbContext
    {
        public G2GDbContext(DbContextOptions<G2GDbContext> options) : base(options)
        {
        }


        #region Entities/Models

        #endregion

        #region View Models

        #endregion
    }
}
