using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Models.Employee;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployee _IEmployee;
        public EmployeeController(IEmployee iEmployee)
        {
            _IEmployee = iEmployee;
        }

        [HttpPost("CreateEmployee")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> CreateEmployee(Employee Employee)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new RequestFailedResponse
                    {
                        Errors = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
                    });
                }
                await _IEmployee.CreateEmp(Employee);
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }

        [HttpDelete("{id}")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Zain Ul Islam' Date='2022-03-23' Version="1.0" Branch="master"> Delete Literature Classifcation</History>
        public void Delete(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    BadRequest(new RequestFailedResponse
                    {
                        Errors = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
                    });
                }
                _IEmployee.DeleteEmployee(id);
                Ok();
            }
            catch (Exception ex)
            {
                BadRequest(ex.Message);
            }
        }
    }
}
