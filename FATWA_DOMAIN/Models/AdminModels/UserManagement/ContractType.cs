using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using FATWA_DOMAIN.Models.BaseModels;

namespace FATWA_DOMAIN.Models.AdminModels.UserManagement
{
    [Table("EP_CONTRACT_TYPE")]

    public class ContractType : TransactionalBaseModel
    {
        [Key]
        public int Id { get; set; }
        public string? Name_En { get; set; }
        public string? Name_Ar { get; set; }
    }
}
