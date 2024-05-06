using soup_back_end.Data;
using soup_back_end.Models;
using soup_back_end.DTOs.PaymentMethods;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Reflection.Metadata.BlobBuilder;
using Microsoft.AspNetCore.Authorization;

namespace soup_back_end.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentMethodsController : Controller
    {
        
    
        private readonly PaymentMethodsData _paymentMethodsData;

        public PaymentMethodsController(Data.PaymentMethodsData paymentMethodsData)
        {
            _paymentMethodsData = paymentMethodsData;
        }

        [HttpGet("GetAll")]
        [Authorize]
        public IActionResult GetAll()
        {
            try
            {
                List<PaymentMethods> paymentMethods = _paymentMethodsData.GetAll();
                return Ok(paymentMethods);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetById")]
        [Authorize]
        public IActionResult GetById(string paymentId)
        {
            PaymentMethods? paymentMethods = _paymentMethodsData.GetById(paymentId);

            if (paymentMethods == null)
            {
                return NotFound("Data Not Found");
            }

            return Ok(paymentMethods);
        }

        [HttpPost]
        public IActionResult Post([FromBody] PaymentMethodsDTO paymentMethodsDto)
        {

            if (paymentMethodsDto == null)
                return BadRequest("Data Should be Inputed");

            PaymentMethods paymentMethods = new PaymentMethods
            {
                paymentId = paymentMethodsDto.paymentId,
                paymentName = paymentMethodsDto.paymentName,
                paymentImg = paymentMethodsDto.paymentImg,
            };

            bool result = _paymentMethodsData.Insert(paymentMethods);

            if (result)
            {
                return StatusCode(201, paymentMethods.paymentId);
            }
            else
            {
                return StatusCode(500, "error occur");
            }

        }

        [HttpDelete]
        public IActionResult Delete(string paymentId)
        {
            bool result = _paymentMethodsData.Delete(paymentId);

            if (result)
            {
                return NoContent();
            }
            else
            {
                return StatusCode(500, "error occur");
            }
        }
    }
}
