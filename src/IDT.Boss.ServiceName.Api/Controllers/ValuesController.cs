using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace IDT.Boss.ServiceName.Api.Controllers
{
    /// <summary>
    /// Sample controller to work with test values.
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    [SwaggerTag("Sample values controller")]
    public sealed class ValuesController : ControllerBase
    {
        private readonly string[] _values = new string[] {"value1", "value2"};

        /// <summary>
        /// Get list of the values.
        /// </summary>
        /// <returns>Returns the list with values.</returns>
        [HttpGet]
        public ActionResult<IEnumerable<string>> GetAllValues()
        {
            return Ok(_values);
        }

        /// <summary>
        /// Get specific value by the index
        /// </summary>
        /// <param name="index">Index.</param>
        /// <returns>Returns the value.</returns>
        [HttpGet]
        [Route("{index}")]
        public ActionResult<string> GetValue(int index)
        {
            if (_values.Length < index - 1)
            {
                return NotFound();
            }

            return Ok(_values[index]);
        }
    }
}