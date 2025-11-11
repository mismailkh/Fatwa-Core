using FATWA_DOMAIN.Models.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Interfaces
{
    public interface IEmployee
    {
        string DeleteEmployee(int Id);
        Task CreateEmp(Employee model);
    }
}
