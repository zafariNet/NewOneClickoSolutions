using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OneClickSolutions.Infrastructure.Web.Results
{
    public class InternalServerErrorObjectResult : ObjectResult
    {
        public InternalServerErrorObjectResult(object error)
            : base(error)
        {
            StatusCode = StatusCodes.Status500InternalServerError;
        }
    }
}