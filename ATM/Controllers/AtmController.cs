using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using ATM.Model.Exceptions;
using ATM.Model.Interfaces;
using Microsoft.Extensions.Logging;


namespace ATM.Controllers
{
    [Route("api")]
    [ApiController]
    public class AtmController : ControllerBase
    {

        private readonly IAtm _atmMachine;
        private readonly ILogger _logger;

        public AtmController(IAtm atm, ILogger logger)
        {
            _atmMachine = atm;
            _logger = logger;
        }


        [HttpGet]
        [Route("ping")]
        public string Index() => "☻☻☻ Listening ☻☻☻"; // just for fun


        [HttpPost]
        [Route("deposit")]
        public ActionResult<int> Deposit([FromBody] IDictionary<int, int> charge)
        {
            try
            {
                _logger.LogInformation("Charge request arrived.");
                int result = _atmMachine.Charge(charge);
                _logger.LogInformation($"ATM successfully charged. Current amount is {result}.");
                return Ok(result);
            }
            catch (AtmUnacceptedValueException e)
            {
                _logger.LogError(e.ToString());
                return StatusCode((int)HttpStatusCode.BadRequest);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("withdrawal")]
        public ActionResult<IDictionary<int, int>> Payment([FromBody] int requestedAmount)
        {
            try
            {
                _logger.LogInformation($"Payment request arrived. Requested amount is {requestedAmount}");
                IDictionary<int, int> result = _atmMachine.Pay(requestedAmount);
                _logger.LogInformation("Payment successfully executed.");
                return Ok(result);

            }
            catch (AtmPaymentException e)
            {
                _logger.LogError(e.ToString());
                return StatusCode((int)HttpStatusCode.ServiceUnavailable);
            }
            catch (AtmUnacceptedValueException e)   // nem osztható ezerrel, ezt mondjuk helyben is el lehetne dönteni,
            {                                       // de szerintem elegánsabb az ATM-hez delegálni, végülis annak a működéséhez tartozik
                _logger.LogError(e.ToString());
                return StatusCode((int)HttpStatusCode.BadRequest);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }


    }
}
