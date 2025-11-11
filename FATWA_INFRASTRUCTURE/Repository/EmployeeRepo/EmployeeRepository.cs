
//Demo App
using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Models.Employee;
using FATWA_INFRASTRUCTURE.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_INFRASTRUCTURE.Repository
{


    public class EmployeeRepository : IEmployee
    {
        private readonly DatabaseContext _ContextDB;

        private List<Employee> _Employee;

        #region Literature Borrow Detail

        public EmployeeRepository(DatabaseContext dbContext)
        {
            _ContextDB = dbContext;
        }



        public async Task CreateEmp(Employee model)
        {

            await _ContextDB.Employees.AddAsync(model);
            await _ContextDB.SaveChangesAsync();

        }

        public string DeleteEmployee(int Id)
        {
            var employee = _ContextDB.Employees.FirstOrDefault(x => x.Id == x.Id);
            _ContextDB.Employees.Remove(employee);
            _ContextDB.SaveChanges();
            return "Deleted";
        }
        public Employee Get(int Id)
        {
            return _ContextDB.Employees.FirstOrDefault(x => x.Id == x.Id);

        }
        public Employee Update(Employee emp)
        {
            _ContextDB.Employees.Add(emp);
            _ContextDB.SaveChanges();
            return this.Get(emp.Id);

        }
    }

}

#endregion







