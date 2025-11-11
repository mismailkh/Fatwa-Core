using FATWA_DOMAIN.Interfaces;
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
    public class LegalLibraryController : ControllerBase
    {
        private readonly ILegalLibrary _ILegalLibrary;
        public LegalLibraryController(ILegalLibrary iLegalLibrary)
        {
            _ILegalLibrary = iLegalLibrary;
        }

        [HttpGet("SearchLegalLibrary")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Zain Ul Islam' Date='2022-04-22' Version="1.0" Branch="master"> Return List of Legal Document, Legal Principles and Books</History>
        public async Task<IActionResult> SearchLegalLibrary()
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
                return Ok(await _ILegalLibrary.SearchLegalLibrary());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
