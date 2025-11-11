using FATWA_DOMAIN.Models.AdminModels.UserManagement;

namespace FATWA_DOMAIN.Interfaces
{
    //<History Author = 'Umer Zaman' Date='2022-07-26' Version="1.0" Branch="master"> interface for transfer user operations</History>
    public interface ITransferUser
    {
        Task<List<Department>> GetAllDepartmentList();
        Task SaveTransferUser(TransferUser transferUser);
    }
}
