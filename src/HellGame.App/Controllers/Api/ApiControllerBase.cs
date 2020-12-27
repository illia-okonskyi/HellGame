using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace HellGame.App.Controllers.Api
{
    public class ApiControllerBase : ControllerBase
    {
        public virtual ObjectResult ApiError([ActionResultObjectValue] object value)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, value);
        }
    }
}
