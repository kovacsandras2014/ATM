using Microsoft.AspNetCore.Mvc;

namespace ATM.Controllers
{
    [Route("api")]
    [ApiController]
    public class AtmController : ControllerBase
    {
        [HttpGet]
        [Route("ping")]
        public string Index() => "☻☻☻ Listening ☻☻☻"; // just for fun

    }
}
