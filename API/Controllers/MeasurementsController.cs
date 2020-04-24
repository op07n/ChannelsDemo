using ChannelExample.Services;
using ChannelExample.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ChannelExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeasurementsController : ControllerBase
    {
        [HttpPost]
        public void Post([FromServices] Batcher batcher, [FromBody] MeasurementViewModel request)
        {
            batcher.AddMeasurement(request);
        }
    }
}