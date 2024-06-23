using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Takerman.Dating.Models.DTOs;
using Takerman.Dating.Models.Enum;

namespace Takerman.Dating.Server.Controllers
{
    public class BaseController(ILogger _logger) : ControllerBase
    {
        protected IActionResult ReturnResponse(dynamic model)
        {
            if (model.Status == RequestExecution.Successful)
            {
                return Ok(model);
            }

            return BadRequest(model);
        }

        protected IActionResult HandleError(Exception ex, string customErrorMessage = null)
        {
            _logger.LogError(ex.Message, ex);

            BaseResponse<string> rsp = new BaseResponse<string>();
            rsp.Status = RequestExecution.Error;
#if DEBUG
            rsp.Errors = new List<string>() { $"Error: {(ex?.InnerException?.Message ?? ex.Message)} --> {ex?.StackTrace}" };
            return BadRequest(rsp);
#else
             rsp.Errors = new List<string>() { "An error occurred while processing your request!"};
             return BadRequest(rsp);
#endif
        }
    }

}

